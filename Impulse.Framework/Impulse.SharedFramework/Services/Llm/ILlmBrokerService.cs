using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Impulse.SharedFramework.Services.Llm;

public interface ILlmBrokerService
{
    event EventHandler<LlmSessionChangedEventArgs>? SessionChanged;

    Task<LlmSession> CreateSessionAsync(LlmSessionCreationOptions options, CancellationToken cancellationToken);

    Task<IReadOnlyList<LlmSession>> GetSessionsAsync(CancellationToken cancellationToken);

    Task<LlmSession?> GetSessionAsync(Guid sessionId, CancellationToken cancellationToken);

    Task<LlmResponse> SendAsync(LlmRequest request, CancellationToken cancellationToken);

    Task RenameSessionAsync(Guid sessionId, string newTitle, CancellationToken cancellationToken);

    Task DeleteSessionAsync(Guid sessionId, CancellationToken cancellationToken);
}
