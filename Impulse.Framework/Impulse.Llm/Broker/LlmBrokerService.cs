using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Impulse.Llm.Configuration;
using Impulse.Llm.Providers;
using Impulse.SharedFramework.Services.Llm;

namespace Impulse.Llm.Broker;

public sealed class LlmBrokerService : ILlmBrokerService
{
    private readonly ILlmSessionStore sessionStore;
    private readonly ILlmSettingsService settingsService;
    private readonly IReadOnlyDictionary<LlmProvider, ILlmProviderClient> providers;
    private readonly ConcurrentDictionary<Guid, SemaphoreSlim> sessionLocks = new();

    public LlmBrokerService(
        ILlmSessionStore sessionStore,
        ILlmSettingsService settingsService,
        IEnumerable<ILlmProviderClient> providerClients)
    {
        this.sessionStore = sessionStore;
        this.settingsService = settingsService;
        providers = providerClients.ToDictionary(client => client.Provider);
    }

    public event EventHandler<LlmSessionChangedEventArgs>? SessionChanged;

    public async Task<LlmSession> CreateSessionAsync(LlmSessionCreationOptions options, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(options);

        var session = new LlmSession
        {
            Title = string.IsNullOrWhiteSpace(options.Title)
                ? $"Session {DateTimeOffset.UtcNow:yyyy-MM-dd HH:mm}"
                : options.Title,
            Description = options.Description,
            Metadata = options.Metadata != null
                ? new Dictionary<string, string>(options.Metadata)
                : new Dictionary<string, string>(),
        };

        if (options.SystemInstructions != null)
        {
            foreach (var instruction in options.SystemInstructions)
            {
                session.Messages.Add(new LlmSessionMessage
                {
                    Role = instruction.Role,
                    Content = instruction.Content,
                    TimestampUtc = DateTimeOffset.UtcNow,
                });
            }
        }

        await sessionStore.SaveAsync(session, cancellationToken).ConfigureAwait(false);
        var clone = session.Clone();
        OnSessionChanged(LlmSessionChangeReason.Created, clone);
        return clone;
    }

    public async Task<IReadOnlyList<LlmSession>> GetSessionsAsync(CancellationToken cancellationToken)
    {
        var sessions = await sessionStore.GetAllAsync(cancellationToken).ConfigureAwait(false);
        return sessions.Select(session => session.Clone()).ToList();
    }

    public async Task<LlmSession?> GetSessionAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        var session = await sessionStore.GetAsync(sessionId, cancellationToken).ConfigureAwait(false);
        return session?.Clone();
    }

    public async Task<LlmResponse> SendAsync(LlmRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var session = await RequireSessionAsync(request.SessionId, cancellationToken).ConfigureAwait(false);
        var locker = sessionLocks.GetOrAdd(session.Id, _ => new SemaphoreSlim(1, 1));

        await locker.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var userMessage = new LlmSessionMessage
            {
                Role = request.Message.Role,
                Content = request.Message.Content,
                TimestampUtc = DateTimeOffset.UtcNow,
                Metadata = request.Message.Metadata != null
                    ? new Dictionary<string, string>(request.Message.Metadata)
                    : new Dictionary<string, string>(),
            };

            session.Messages.Add(userMessage);
            session.LastUpdatedUtc = DateTimeOffset.UtcNow;

            var provider = ResolveProvider(request.Options.Provider);
            var configuration = await RequireConfigurationAsync(request.Options.Provider, cancellationToken).ConfigureAwait(false);
            var providerRequest = new LlmProviderRequest(
                session.Id,
                session.Messages.Select(m => m.ToMessage()).ToList(),
                request.Options,
                configuration);

            LlmProviderResponse providerResponse;
            try
            {
                providerResponse = await provider.SendAsync(providerRequest, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                session.Messages.Remove(userMessage);
                session.LastUpdatedUtc = DateTimeOffset.UtcNow;
                throw;
            }

            var assistantMessage = new LlmSessionMessage
            {
                Role = LlmMessageRole.Assistant,
                Content = providerResponse.Content,
                TimestampUtc = DateTimeOffset.UtcNow,
                Provider = request.Options.Provider,
                Model = providerResponse.Model,
                Usage = providerResponse.Usage,
                Metadata = providerResponse.Metadata ?? new Dictionary<string, string>(),
            };

            session.Messages.Add(assistantMessage);
            session.LastUpdatedUtc = DateTimeOffset.UtcNow;

            await sessionStore.SaveAsync(session, cancellationToken).ConfigureAwait(false);

            var clone = session.Clone();
            var response = new LlmResponse(clone, assistantMessage.Clone(), request, providerResponse.Metadata);
            OnSessionChanged(LlmSessionChangeReason.Updated, clone);
            return response;
        }
        finally
        {
            locker.Release();
        }
    }

    public async Task RenameSessionAsync(Guid sessionId, string newTitle, CancellationToken cancellationToken)
    {
        var session = await RequireSessionAsync(sessionId, cancellationToken).ConfigureAwait(false);
        session.Title = newTitle;
        session.LastUpdatedUtc = DateTimeOffset.UtcNow;
        await sessionStore.SaveAsync(session, cancellationToken).ConfigureAwait(false);
        OnSessionChanged(LlmSessionChangeReason.Updated, session.Clone());
    }

    public async Task DeleteSessionAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        var existing = await sessionStore.GetAsync(sessionId, cancellationToken).ConfigureAwait(false);
        await sessionStore.DeleteAsync(sessionId, cancellationToken).ConfigureAwait(false);
        if (sessionLocks.TryRemove(sessionId, out var locker))
        {
            locker.Dispose();
        }

        var snapshot = existing?.Clone() ?? new LlmSession { Id = sessionId };
        OnSessionChanged(LlmSessionChangeReason.Deleted, snapshot);
    }

    private async Task<LlmSession> RequireSessionAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        var session = await sessionStore.GetAsync(sessionId, cancellationToken).ConfigureAwait(false);
        if (session == null)
        {
            throw new InvalidOperationException($"Session '{sessionId}' was not found.");
        }

        return session;
    }

    private ILlmProviderClient ResolveProvider(LlmProvider provider)
    {
        if (!providers.TryGetValue(provider, out var client))
        {
            throw new InvalidOperationException($"No LLM client is registered for '{provider}'.");
        }

        return client;
    }

    private async Task<LlmProviderConfiguration> RequireConfigurationAsync(LlmProvider provider, CancellationToken cancellationToken)
    {
        var configuration = await settingsService.GetProviderConfigurationAsync(provider, cancellationToken).ConfigureAwait(false);
        if (configuration == null)
        {
            throw new InvalidOperationException($"No configuration was found for '{provider}'.");
        }

        return configuration;
    }

    private void OnSessionChanged(LlmSessionChangeReason reason, LlmSession session)
    {
        SessionChanged?.Invoke(this, new LlmSessionChangedEventArgs(reason, session));
    }
}
