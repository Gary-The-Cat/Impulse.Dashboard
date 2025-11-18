using System.Threading;
using System.Threading.Tasks;
using Impulse.SharedFramework.Services.Llm;

namespace Impulse.Llm.Configuration;

public interface ILlmSettingsService
{
    Task<LlmSettings> GetSettingsAsync(CancellationToken cancellationToken);

    Task<LlmProviderConfiguration?> GetProviderConfigurationAsync(LlmProvider provider, CancellationToken cancellationToken);

    Task SetProviderConfigurationAsync(LlmProvider provider, LlmProviderConfiguration configuration, CancellationToken cancellationToken);
}
