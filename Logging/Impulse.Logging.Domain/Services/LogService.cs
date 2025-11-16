// <copyright file="LogService.cs" company="Tutorials with Gary">
// Copyright (c) Tutorials with Gary. All rights reserved.
// </copyright>

namespace Impulse.Logging.Domain.Services;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Impulse.Logging.Contracts;
using Impulse.Logging.Domain.Models;
using Impulse.Shared.ExtensionMethods;
using Impulse.Shared.Interfaces;

public class LogService : ILogService
{
    private readonly WeakReference<IDateTimeProvider> dateTimeProviderReference;

    private readonly List<IObserver<LogRecordBase>> observers = new();

    private readonly object observersLock = new();

    private readonly object recordsLock = new();

    private readonly List<LogRecordBase> cachedRecords = new();

    private readonly string logFilePath;

    private readonly object fileLock = new();

    public LogService(IDateTimeProvider dateTimeProvider)
    {
        _ = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        dateTimeProviderReference = new WeakReference<IDateTimeProvider>(dateTimeProvider);
        logFilePath = DetermineLogFilePath();
    }

    private IDateTimeProvider DateTimeProvider => dateTimeProviderReference.Value();

    public Task LogException(string message, Exception exception) =>
        LogMessage(CreateExceptionRecord(message, exception));

    public Task LogInfo(string message) =>
         LogMessage(CreateInfoRecord(message));

    public Task LogWarning(string message) =>
        LogMessage(CreateWarningRecord(message));

    public Task LogError(string message) =>
        LogMessage(CreateErrorRecord(message));

    public Task DeleteRecordsAsync(IEnumerable<Guid> recordIds)
    {
        _ = recordIds ?? throw new ArgumentNullException(nameof(recordIds));
        var ids = recordIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        if (ids.Count == 0)
        {
            return Task.CompletedTask;
        }

        var idSet = new HashSet<Guid>(ids);

        lock (recordsLock)
        {
            cachedRecords.RemoveAll(record => idSet.Contains(record.Id));
        }

        return Task.CompletedTask;
    }

    public IEnumerable<LogRecordBase> GetLogRecordsForCricicality(Criticality criticality)
    {
        lock (recordsLock)
        {
            return cachedRecords
                .Where(r => LogRecordCriticality.Get(r) >= criticality)
                .OrderBy(r => r.Timestamp)
                .ThenBy(r => r.Id)
                .ToList();
        }
    }

    public IDisposable Subscribe(IObserver<LogRecordBase> observer)
    {
        _ = observer ?? throw new ArgumentNullException(nameof(observer));

        lock (observersLock)
        {
            observers.Add(observer);
        }

        foreach (var existing in GetLogRecordsForCricicality(Criticality.Info))
        {
            observer.OnNext(existing);
        }

        return new Unsubscriber(observersLock, observers, observer);
    }

    private Task LogMessage(LogRecordModel record)
    {
        _ = record ?? throw new ArgumentNullException(nameof(record));

        var sharedRecord = record.ToSharedLogRecord();
        lock (recordsLock)
        {
            cachedRecords.Add(sharedRecord);
        }

        AppendToLogFile(sharedRecord);
        Publish(sharedRecord);
        return Task.CompletedTask;
    }

    private void Publish(LogRecordBase record)
    {
        List<IObserver<LogRecordBase>> snapshot;
        lock (observersLock)
        {
            snapshot = observers.ToList();
        }

        foreach (var observer in snapshot)
        {
            observer.OnNext(record);
        }
    }

    private void AppendToLogFile(LogRecordBase record)
    {
        try
        {
            var severity = LogRecordCriticality.Get(record).ToString().ToUpperInvariant();
            var builder = new StringBuilder()
                .Append(record.Timestamp.ToString("O", CultureInfo.InvariantCulture))
                .Append(" [")
                .Append(severity)
                .Append("] ")
                .Append(record.Message ?? string.Empty);

            if (record is ExceptionLogRecord exceptionRecord)
            {
                if (!string.IsNullOrWhiteSpace(exceptionRecord.ExceptionType) ||
                    !string.IsNullOrWhiteSpace(exceptionRecord.ExceptionMessage))
                {
                    builder.Append(" (")
                        .Append(exceptionRecord.ExceptionType ?? "Exception")
                        .Append(") ")
                        .Append(exceptionRecord.ExceptionMessage ?? string.Empty);
                }

                if (!string.IsNullOrWhiteSpace(exceptionRecord.StackTrace))
                {
                    builder.AppendLine()
                        .Append(exceptionRecord.StackTrace);
                }
            }

            var entry = builder.ToString();
            lock (fileLock)
            {
                File.AppendAllText(logFilePath, entry + Environment.NewLine);
            }
        }
        catch
        {
            // File logging is best-effort; swallow to avoid impacting dashboard execution.
        }
    }

    private static string DetermineLogFilePath()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        if (string.IsNullOrWhiteSpace(localAppData))
        {
            return Path.Combine(AppContext.BaseDirectory, "Impulse.Dashboard.log");
        }

        var folder = Path.Combine(localAppData, "Impulse.Dashboard");
        Directory.CreateDirectory(folder);
        return Path.Combine(folder, "Dashboard.log");
    }

    private InfoLogRecordModel CreateInfoRecord(string message) =>
        new InfoLogRecordModel
        {
            Id = Guid.NewGuid(),
            Message = message,
            Timestamp = DateTimeProvider.Now,
            Criticality = (int)Criticality.Info,
        };

    private WarningLogRecordModel CreateWarningRecord(string message) =>
        new WarningLogRecordModel
        {
            Id = Guid.NewGuid(),
            Message = message,
            Timestamp = DateTimeProvider.Now,
            Criticality = (int)Criticality.Warning,
        };

    private ErrorLogRecordModel CreateErrorRecord(string message) =>
        new ErrorLogRecordModel
        {
            Id = Guid.NewGuid(),
            Message = message,
            Timestamp = DateTimeProvider.Now,
            Criticality = (int)Criticality.Error,
        };

    private ExceptionLogRecordModel CreateExceptionRecord(string message, Exception exception)
    {
        _ = exception ?? throw new ArgumentNullException(nameof(exception));

        var exceptionType = exception.GetType();
        return new ExceptionLogRecordModel
        {
            Id = Guid.NewGuid(),
            Message = message,
            Timestamp = DateTimeProvider.Now,
            Criticality = (int)Criticality.Error,
            StackTrace = exception.StackTrace ?? string.Empty,
            ExceptionMessage = exception.Message ?? string.Empty,
            ExceptionType = exceptionType.FullName ?? exceptionType.Name,
        };
    }

    private sealed class Unsubscriber : IDisposable
    {
        private readonly object gate;
        private readonly List<IObserver<LogRecordBase>> observers;
        private readonly IObserver<LogRecordBase> observer;

        public Unsubscriber(object gate, List<IObserver<LogRecordBase>> observers, IObserver<LogRecordBase> observer)
        {
            this.gate = gate;
            this.observers = observers;
            this.observer = observer;
        }

        public void Dispose()
        {
            lock (gate)
            {
                observers.Remove(observer);
            }
        }
    }
}
