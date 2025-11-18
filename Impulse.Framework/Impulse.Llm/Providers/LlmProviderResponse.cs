using System.Collections.Generic;
using Impulse.SharedFramework.Services.Llm;

namespace Impulse.Llm.Providers;

public sealed record LlmProviderResponse(
    string Content,
    string Model,
    LlmUsage? Usage,
    IDictionary<string, string>? Metadata = null);
