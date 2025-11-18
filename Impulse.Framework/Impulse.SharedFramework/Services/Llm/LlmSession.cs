using System;
using System.Collections.Generic;
using System.Linq;

namespace Impulse.SharedFramework.Services.Llm;

public sealed class LlmSession
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset LastUpdatedUtc { get; set; } = DateTimeOffset.UtcNow;

    public IList<LlmSessionMessage> Messages { get; set; } = new List<LlmSessionMessage>();

    public IDictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

    public LlmSession Clone()
    {
        return new LlmSession
        {
            Id = this.Id,
            Title = this.Title,
            Description = this.Description,
            CreatedAtUtc = this.CreatedAtUtc,
            LastUpdatedUtc = this.LastUpdatedUtc,
            Metadata = new Dictionary<string, string>(this.Metadata),
            Messages = this.Messages.Select(m => m.Clone()).ToList(),
        };
    }
}
