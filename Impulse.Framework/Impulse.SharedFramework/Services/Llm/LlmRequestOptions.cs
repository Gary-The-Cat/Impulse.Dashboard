using System.Collections.Generic;

namespace Impulse.SharedFramework.Services.Llm;

public sealed class LlmRequestOptions
{
    public LlmProvider Provider { get; set; }

    public string? Model { get; set; }

    public int? MaxTokens { get; set; }

    public float? Temperature { get; set; }

    public IReadOnlyList<string>? StopSequences { get; set; }

    public bool Stream { get; set; }

    public IDictionary<string, string>? Metadata { get; set; }
}
