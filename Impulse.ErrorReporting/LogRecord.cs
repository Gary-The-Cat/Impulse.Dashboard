namespace Impulse.ErrorReporting;

public record LogRecord
{
    private LogRecord() { }

    public required DateTime Timestamp { get; init; }

    public required Criticality Criticality { get; init; }

    public required string Message { get; init; }

    public string? StackTrace { get; init; }

    public static LogRecord CreateInfo(DateTime timeStamp, string message) => new()
    {
        Timestamp = timeStamp,
        Criticality = Criticality.Info,
        Message = message,
    };

    public static LogRecord CreateWarning(DateTime timeStamp, string message) => new()
    {
        Timestamp = timeStamp,
        Criticality = Criticality.Warning,
        Message = message,
    };

    public static LogRecord CreateException(DateTime timeStamp, string message, Exception exception) => new()
    {
        Timestamp = timeStamp,
        Criticality = Criticality.Info,
        Message = message,
        StackTrace = exception.StackTrace,
    };
}