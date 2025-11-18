using System.Collections.Generic;

namespace Impulse.SharedFramework.Services.Llm;

public sealed class LlmSessionCreationOptions
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public IReadOnlyCollection<LlmMessage>? SystemInstructions { get; set; }

    public IDictionary<string, string>? Metadata { get; set; }
}
