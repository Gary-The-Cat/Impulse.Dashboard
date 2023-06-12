namespace Impulse.ErrorReporting;

public record LogRecord
{
    private LogRecord() { }

    required public int Id { get; set; }

    required public DateTime Timestamp { get; init; }

    required public Criticality Criticality { get; init; }

    required public string Message { get; init; }

    public string? StackTrace { get; init; }

    public static LogRecord CreateInfo(int id, DateTime timeStamp, string message) => new()
    {
        Id = id,
        Timestamp = timeStamp,
        Criticality = Criticality.Info,
        Message = message,
    };

    public static LogRecord CreateWarning(int id, DateTime timeStamp, string message) => new()
    {
        Id = id,
        Timestamp = timeStamp,
        Criticality = Criticality.Warning,
        Message = message,
    };

    public static LogRecord CreateException(int id, DateTime timeStamp, string message, Exception exception) => new()
    {
        Id = id,
        Timestamp = timeStamp,
        Criticality = Criticality.Info,
        Message = message,
        StackTrace = exception.StackTrace,
    };
}