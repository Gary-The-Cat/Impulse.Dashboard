namespace Impulse.ErrorReporting;

public record Error
{
    public required DateTime Timestamp { get; init; }

    public required Criticality Criticality { get; init; }

    public required string Message { get; init; }

    public Exception? Exception { get; init; }
}