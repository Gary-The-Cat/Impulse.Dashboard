namespace Impulse.Framework.Dashboard.Services.Logging;

using Impulse.SharedFramework.Services.Logging;
using ErrorLogRecord = Impulse.ErrorReporting.LogRecord;
using ErrorInfoLogRecord = Impulse.ErrorReporting.InfoLogRecord;
using ErrorWarningLogRecord = Impulse.ErrorReporting.WarningLogRecord;
using ErrorErrorLogRecord = Impulse.ErrorReporting.ErrorLogRecord;
using ErrorExceptionLogRecord = Impulse.ErrorReporting.ExceptionLogRecord;
using SharedInfoLogRecord = Impulse.SharedFramework.Services.Logging.InfoLogRecord;
using SharedWarningLogRecord = Impulse.SharedFramework.Services.Logging.WarningLogRecord;
using SharedErrorLogRecord = Impulse.SharedFramework.Services.Logging.ErrorLogRecord;
using SharedExceptionLogRecord = Impulse.SharedFramework.Services.Logging.ExceptionLogRecord;

public static class LogRecordExtensions
{
    internal static LogRecordBase ToSharedLogRecord(this ErrorLogRecord record) =>
        record switch
        {
            ErrorExceptionLogRecord exceptionRecord => new SharedExceptionLogRecord
            {
                Id = exceptionRecord.Id,
                Timestamp = exceptionRecord.Timestamp,
                Message = exceptionRecord.Message,
                ExceptionType = exceptionRecord.ExceptionType,
                ExceptionMessage = exceptionRecord.ExceptionMessage,
                StackTrace = exceptionRecord.StackTrace,
            },
            ErrorWarningLogRecord warningRecord => new SharedWarningLogRecord
            {
                Id = warningRecord.Id,
                Timestamp = warningRecord.Timestamp,
                Message = warningRecord.Message,
            },
            ErrorErrorLogRecord errorRecord => new SharedErrorLogRecord
            {
                Id = errorRecord.Id,
                Timestamp = errorRecord.Timestamp,
                Message = errorRecord.Message,
            },
            _ => new SharedInfoLogRecord
            {
                Id = record.Id,
                Timestamp = record.Timestamp,
                Message = record.Message,
            },
        };
}
