// <copyright file="LogService.cs" company="Tutorials with Gary">
// Copyright (c) Tutorials with Gary. All rights reserved.
// </copyright>

namespace Impulse.Framework.Dashboard.Services.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Impulse.Shared.ExtensionMethods;
using Impulse.Shared.Interfaces;
using Impulse.SharedFramework.Services.Logging;
using ErrorLogRecord = Impulse.ErrorReporting.LogRecord;
using ErrorLogger = Impulse.ErrorReporting.Logger;
using ErrorCriticality = Impulse.ErrorReporting.Criticality;
using SharedCriticality = Impulse.SharedFramework.Services.Logging.Criticality;

public class LogService : ILogService
{
    private readonly WeakReference<IDateTimeProvider> dateTimeProviderReference;

    private readonly ErrorLogger logger = new ErrorLogger();

    private readonly List<IObserver<LogRecordBase>> observers = new();

    private readonly object observersLock = new();

    private int currentRecordId = -1;

    // TODO: Rework how this is loaded first time, when the database is enabled, get the last record Id from there.
    public LogService(IDateTimeProvider dateTimeProvider)
    {
        dateTimeProviderReference = new WeakReference<IDateTimeProvider>(dateTimeProvider);
    }

    private IDateTimeProvider DateTimeProvider => dateTimeProviderReference.Value();

    public Task LogException(string message, Exception exception) =>
        LogMessage(ErrorLogRecord.CreateException(GetNextRecordId(), DateTimeProvider.Now, message, exception));

    public Task LogInfo(string message) =>
         LogMessage(ErrorLogRecord.CreateInfo(GetNextRecordId(), DateTimeProvider.Now, message));

    public Task LogWarning(string message) =>
        LogMessage(ErrorLogRecord.CreateWarning(GetNextRecordId(), DateTimeProvider.Now, message));

    public Task LogError(string message) =>
        LogMessage(ErrorLogRecord.CreateError(GetNextRecordId(), DateTimeProvider.Now, message));

    public IEnumerable<LogRecordBase> GetLogRecordsForCricicality(SharedCriticality criticality) =>
        this.logger.GetLogRecordsForCricicality((ErrorCriticality)criticality)
            .Select(r => r.ToSharedLogRecord());

    public IDisposable Subscribe(IObserver<LogRecordBase> observer)
    {
        if (observer == null)
        {
            throw new ArgumentNullException(nameof(observer));
        }

        lock (observersLock)
        {
            observers.Add(observer);
        }

        foreach (var existing in GetLogRecordsForCricicality(SharedCriticality.Info))
        {
            observer.OnNext(existing);
        }

        return new Unsubscriber(observersLock, observers, observer);
    }

    private async Task LogMessage(ErrorLogRecord record)
    {
        await logger.LogRecord(record);
        Publish(record.ToSharedLogRecord());
    }

    private int GetNextRecordId() => Interlocked.Increment(ref currentRecordId);

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
