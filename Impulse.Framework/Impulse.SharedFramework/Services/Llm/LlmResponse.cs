using System.Collections.Generic;

namespace Impulse.SharedFramework.Services.Llm;

public sealed record LlmResponse(
    LlmSession Session,
    LlmSessionMessage AssistantMessage,
    LlmRequest Request,
    IDictionary<string, string>? Metadata = null);
