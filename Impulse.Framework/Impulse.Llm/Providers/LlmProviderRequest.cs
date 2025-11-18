using System;
using System.Collections.Generic;
using Impulse.Llm.Configuration;
using Impulse.SharedFramework.Services.Llm;

namespace Impulse.Llm.Providers;

public sealed record LlmProviderRequest(
    Guid SessionId,
    IReadOnlyList<LlmMessage> Messages,
    LlmRequestOptions Options,
    LlmProviderConfiguration Configuration);
