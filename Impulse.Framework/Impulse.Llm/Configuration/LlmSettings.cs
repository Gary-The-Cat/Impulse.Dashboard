using System.Collections.Generic;
using Impulse.SharedFramework.Services.Llm;

namespace Impulse.Llm.Configuration;

public sealed class LlmSettings
{
    public IDictionary<LlmProvider, LlmProviderConfiguration> Providers { get; set; } = new Dictionary<LlmProvider, LlmProviderConfiguration>();

    public LlmProviderConfiguration GetOrCreate(LlmProvider provider)
    {
        if (!Providers.TryGetValue(provider, out var configuration))
        {
            configuration = new LlmProviderConfiguration();
            Providers[provider] = configuration;
        }

        return configuration;
    }
}
