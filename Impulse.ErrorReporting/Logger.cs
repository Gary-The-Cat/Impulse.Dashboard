// <copyright file="Logger.cs" company="Tutorials with Gary">
// Copyright (c) Tutorials with Gary. All rights reserved.
// </copyright>

namespace Impulse.ErrorReporting;

public class Logger
{
    private List<LogRecord> logRecords = new List<LogRecord>();

    private List<LogType> enabledLogOutputs = new List<LogType>();

    public Logger() => this.enabledLogOutputs.Add(LogType.Memory);

    public void EnableLogType(LogType logType) => this.enabledLogOutputs.Add(logType);

    public void DisableLogType(LogType logType) => this.enabledLogOutputs.Remove(logType);

    public IEnumerable<LogType> EnabledLogOutputs => this.enabledLogOutputs;

    public Task LogRecord(LogRecord record)
    {
        foreach (var output in this.enabledLogOutputs)
        {
            switch (output)
            {
                case LogType.Memory:
                    this.LogRecordToMemory(record);
                    break;
                default:
                    throw new NotImplementedException($"{output} is not supported.");
            }
        }

        return Task.CompletedTask;
    }

    private void LogRecordToMemory(LogRecord record) => this.logRecords.Add(record);

    public IEnumerable<LogRecord> GetLogRecordsForCricicality(Criticality criticality)
    {
        foreach (var record in this.logRecords.Where(r => r.Criticality >= criticality))
        {
            yield return record;
        }
    }
}
