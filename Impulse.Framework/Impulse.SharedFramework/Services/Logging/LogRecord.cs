namespace Impulse.SharedFramework.Services.Logging;

using System;

public record LogRecord
{
    required public int Id { get; set; }

    required public DateTime Timestamp { get; init; }

    required public Criticality Criticality { get; init; }

    required public string Message { get; init; }

    public string? StackTrace { get; init; }
}
