using System.Collections.Generic;

namespace Impulse.SharedFramework.Services.Llm;

public sealed record LlmMessage(
    LlmMessageRole Role,
    string Content,
    IDictionary<string, string>? Metadata = null);
