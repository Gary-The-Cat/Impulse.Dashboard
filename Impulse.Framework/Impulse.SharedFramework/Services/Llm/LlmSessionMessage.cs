using System;
using System.Collections.Generic;

namespace Impulse.SharedFramework.Services.Llm;

public sealed class LlmSessionMessage
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public LlmMessageRole Role { get; init; }

    public string Content { get; init; } = string.Empty;

    public DateTimeOffset TimestampUtc { get; init; } = DateTimeOffset.UtcNow;

    public LlmProvider? Provider { get; init; }

    public string? Model { get; init; }

    public LlmUsage? Usage { get; init; }

    public IDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();

    public LlmMessage ToMessage() => new(this.Role, this.Content, this.Metadata);

    public LlmSessionMessage Clone()
    {
        return new LlmSessionMessage
        {
            Id = this.Id,
            Role = this.Role,
            Content = this.Content,
            TimestampUtc = this.TimestampUtc,
            Provider = this.Provider,
            Model = this.Model,
            Usage = this.Usage,
            Metadata = new Dictionary<string, string>(this.Metadata),
        };
    }
}
