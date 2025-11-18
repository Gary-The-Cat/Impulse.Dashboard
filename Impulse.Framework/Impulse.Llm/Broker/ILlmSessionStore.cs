using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Impulse.SharedFramework.Services.Llm;

namespace Impulse.Llm.Broker;

public interface ILlmSessionStore
{
    Task SaveAsync(LlmSession session, CancellationToken cancellationToken);

    Task<LlmSession?> GetAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<LlmSession>> GetAllAsync(CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
