using System;
using System.Runtime.Serialization;

namespace Impulse.Logging.Domain.Models;

[DataContract]
public abstract record LogRecordModel
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Message { get; set; } = string.Empty;

    public int Criticality { get; set; }

    public DateTime Timestamp { get; set; }
}

public record InfoLogRecordModel : LogRecordModel;

public record WarningLogRecordModel : LogRecordModel;

public record ErrorLogRecordModel : LogRecordModel;

public record ExceptionLogRecordModel : ErrorLogRecordModel
{
    public string StackTrace { get; set; } = string.Empty;

    public string ExceptionType { get; set; } = string.Empty;

    public string ExceptionMessage { get; set; } = string.Empty;
}
