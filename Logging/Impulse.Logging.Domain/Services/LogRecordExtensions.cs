namespace Impulse.Logging.Domain.Services;

using System;
using Impulse.Logging.Contracts;
using Impulse.Logging.Domain.Models;

public static class LogRecordExtensions
{
    internal static LogRecordBase ToSharedLogRecord(this LogRecordModel model)
    {
        _ = model ?? throw new ArgumentNullException(nameof(model));

        return model switch
        {
            ExceptionLogRecordModel exceptionModel => new ExceptionLogRecord
            {
                Id = exceptionModel.Id,
                Timestamp = exceptionModel.Timestamp,
                Message = exceptionModel.Message,
                ExceptionType = exceptionModel.ExceptionType,
                ExceptionMessage = exceptionModel.ExceptionMessage,
                StackTrace = exceptionModel.StackTrace,
            },
            WarningLogRecordModel warningModel => new WarningLogRecord
            {
                Id = warningModel.Id,
                Timestamp = warningModel.Timestamp,
                Message = warningModel.Message,
            },
            ErrorLogRecordModel errorModel => new ErrorLogRecord
            {
                Id = errorModel.Id,
                Timestamp = errorModel.Timestamp,
                Message = errorModel.Message,
            },
            _ => new InfoLogRecord
            {
                Id = model.Id,
                Timestamp = model.Timestamp,
                Message = model.Message,
            },
        };
    }
}
