using System.Threading;
using System.Threading.Tasks;
using Impulse.SharedFramework.Services.Llm;

namespace Impulse.Llm.Providers;

public interface ILlmProviderClient
{
    LlmProvider Provider { get; }

    bool SupportsStreaming { get; }

    Task<LlmProviderResponse> SendAsync(LlmProviderRequest request, CancellationToken cancellationToken);
}
