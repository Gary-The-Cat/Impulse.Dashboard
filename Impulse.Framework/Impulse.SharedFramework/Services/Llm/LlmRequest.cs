using System;

namespace Impulse.SharedFramework.Services.Llm;

public sealed record LlmRequest(Guid SessionId, LlmMessage Message, LlmRequestOptions Options);
