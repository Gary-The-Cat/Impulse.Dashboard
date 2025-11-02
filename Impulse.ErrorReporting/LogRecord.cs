namespace Impulse.ErrorReporting;

public abstract record LogRecord(int Id, DateTime Timestamp, string Message)
{
    public abstract Criticality Criticality { get; }

    public static LogRecord CreateInfo(int id, DateTime timestamp, string message) =>
        new InfoLogRecord(id, timestamp, message);

    public static LogRecord CreateWarning(int id, DateTime timestamp, string message) =>
        new WarningLogRecord(id, timestamp, message);

    public static LogRecord CreateError(int id, DateTime timestamp, string message) =>
        new ErrorLogRecord(id, timestamp, message);

    public static LogRecord CreateException(int id, DateTime timestamp, string message, Exception exception) =>
        new ExceptionLogRecord(
            id,
            timestamp,
            message,
            exception.GetType().FullName ?? exception.GetType().Name,
            exception.Message,
            exception.StackTrace ?? string.Empty);
}

public sealed record InfoLogRecord(int Id, DateTime Timestamp, string Message)
    : LogRecord(Id, Timestamp, Message)
{
    public override Criticality Criticality => Criticality.Info;
}

public sealed record WarningLogRecord(int Id, DateTime Timestamp, string Message)
    : LogRecord(Id, Timestamp, Message)
{
    public override Criticality Criticality => Criticality.Warning;
}

public sealed record ErrorLogRecord(int Id, DateTime Timestamp, string Message)
    : LogRecord(Id, Timestamp, Message)
{
    public override Criticality Criticality => Criticality.Error;
}

public sealed record ExceptionLogRecord(
    int Id,
    DateTime Timestamp,
    string Message,
    string ExceptionType,
    string ExceptionMessage,
    string StackTrace)
    : LogRecord(Id, Timestamp, Message)
{
    public override Criticality Criticality => Criticality.Error;
}
