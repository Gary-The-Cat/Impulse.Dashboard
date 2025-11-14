namespace Impulse.Framework.Dashboard.Services.Logging;

using System;
using Impulse.Repository.Models;
using Impulse.SharedFramework.Services.Logging;
using SharedInfoLogRecord = Impulse.SharedFramework.Services.Logging.InfoLogRecord;
using SharedWarningLogRecord = Impulse.SharedFramework.Services.Logging.WarningLogRecord;
using SharedErrorLogRecord = Impulse.SharedFramework.Services.Logging.ErrorLogRecord;
using SharedExceptionLogRecord = Impulse.SharedFramework.Services.Logging.ExceptionLogRecord;

public static class LogRecordExtensions
{
    internal static LogRecordBase ToSharedLogRecord(this LogRecordModel model)
    {
        _ = model ?? throw new ArgumentNullException(nameof(model));

        return model switch
        {
            ExceptionLogRecordModel exceptionModel => new SharedExceptionLogRecord
            {
                Id = exceptionModel.Id,
                Timestamp = exceptionModel.Timestamp,
                Message = exceptionModel.Message,
                ExceptionType = exceptionModel.ExceptionType,
                ExceptionMessage = exceptionModel.ExceptionMessage,
                StackTrace = exceptionModel.StackTrace,
            },
            WarningLogRecordModel warningModel => new SharedWarningLogRecord
            {
                Id = warningModel.Id,
                Timestamp = warningModel.Timestamp,
                Message = warningModel.Message,
            },
            ErrorLogRecordModel errorModel => new SharedErrorLogRecord
            {
                Id = errorModel.Id,
                Timestamp = errorModel.Timestamp,
                Message = errorModel.Message,
            },
            _ => new SharedInfoLogRecord
            {
                Id = model.Id,
                Timestamp = model.Timestamp,
                Message = model.Message,
            },
        };
    }
}
