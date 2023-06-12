namespace Impulse.Framework.Dashboard.Services.Logging;

using Impulse.SharedFramework.Services.Logging;

public static class LogRecordExtensions
{
    // TODO: This needs to be done with a mapper.
    internal static LogRecord ToSharedLogRecord(this ErrorReporting.LogRecord record) =>
        new LogRecord
        {
            Id = record.Id,
            Timestamp = record.Timestamp,
            Criticality = (Criticality)(int)record.Criticality,
            Message = record.Message,
            StackTrace = record.StackTrace,
        };
}
