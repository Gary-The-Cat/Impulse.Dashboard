namespace Impulse.SharedFramework.Services.Logging;

using System;

public abstract record LogRecordBase
{
    required public int Id { get; init; }

    required public DateTime Timestamp { get; init; }

    required public string Message { get; init; }

    public virtual bool HasDetails => false;
}

public record InfoLogRecord : LogRecordBase;

public record WarningLogRecord : LogRecordBase;

public record ErrorLogRecord : LogRecordBase;

public record ExceptionLogRecord : ErrorLogRecord
{
    public string? StackTrace { get; init; }

    public string? ExceptionType { get; init; }

    public string? ExceptionMessage { get; init; }

    public override bool HasDetails =>
        !string.IsNullOrWhiteSpace(ExceptionMessage) ||
        !string.IsNullOrWhiteSpace(StackTrace);
}

public static class LogRecordCriticality
{
    public static Criticality Get(LogRecordBase record) => record switch
    {
        ExceptionLogRecord => Criticality.Error,
        ErrorLogRecord => Criticality.Error,
        WarningLogRecord => Criticality.Warning,
        InfoLogRecord => Criticality.Info,
        _ => Criticality.Info,
    };
}
