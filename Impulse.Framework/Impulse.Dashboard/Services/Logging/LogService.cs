// <copyright file="LogService.cs" company="Tutorials with Gary">
// Copyright (c) Tutorials with Gary. All rights reserved.
// </copyright>

namespace Impulse.Framework.Dashboard.Services.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Impulse.ErrorReporting;
using Impulse.Shared.ExtensionMethods;
using Impulse.Shared.Interfaces;
using Impulse.SharedFramework.Services.Logging;

public class LogService : ILogService
{
    private WeakReference<IDateTimeProvider> dateTimeProviderReference;

    private Logger logger = new Logger();

    private int currentRecordId = 0;

    // TODO: Rework how this is loaded first time, when the database is enabled, get the last record Id from there.
    public LogService(IDateTimeProvider dateTimeProvider)
    {
        dateTimeProviderReference = new WeakReference<IDateTimeProvider>(dateTimeProvider);
    }

    private IDateTimeProvider DateTimeProvider => dateTimeProviderReference.Value();

    public Task LogException(string message, Exception exception) =>
        LogMessage(ErrorReporting.LogRecord.CreateException(currentRecordId++, DateTimeProvider.Now, message, exception));

    public Task LogInfo(string message) =>
         LogMessage(ErrorReporting.LogRecord.CreateInfo(currentRecordId++, DateTimeProvider.Now, message));

    public Task LogWarning(string message) =>
        LogMessage(ErrorReporting.LogRecord.CreateWarning(currentRecordId++, DateTimeProvider.Now, message));

    private Task LogMessage(ErrorReporting.LogRecord record)
    {
        logger.LogRecord(record);

        return Task.CompletedTask;
    }

    public IEnumerable<SharedFramework.Services.Logging.LogRecord> GetLogRecordsForCricicality(
        SharedFramework.Services.Logging.Criticality criticality) =>
        this.logger.GetLogRecordsForCricicality((ErrorReporting.Criticality)criticality)
            .Select(r => r.ToSharedLogRecord());
}
