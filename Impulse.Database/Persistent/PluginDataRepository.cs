namespace Impulse.Repository.Persistent;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Impulse.Storage;
using Microsoft.EntityFrameworkCore;

public sealed class PluginDataRepository : IPluginDataRepository
{
    private readonly string connectionString;

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public PluginDataRepository()
    {
        connectionString = DashboardStorage.GetPluginDataConnectionString();
        using var context = CreateContext();
        context.Database.EnsureCreated();
    }

    public async Task SaveSingletonAsync<T>(T model, CancellationToken cancellationToken = default)
        where T : class
    {
        _ = model ?? throw new ArgumentNullException(nameof(model));

        await using var context = CreateContext();
        var typeKey = GetTypeKey<T>();
        var payload = Serialize(model);
        var entity = await context.SingletonModels
            .SingleOrDefaultAsync(e => e.TypeName == typeKey, cancellationToken)
            .ConfigureAwait(false);

        if (entity is null)
        {
            await context.SingletonModels.AddAsync(new StoredSingletonModel
            {
                TypeName = typeKey,
                Payload = payload,
                UpdatedOn = DateTime.UtcNow,
            }, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            entity.Payload = payload;
            entity.UpdatedOn = DateTime.UtcNow;
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<T?> GetSingletonAsync<T>(CancellationToken cancellationToken = default)
        where T : class
    {
        await using var context = CreateContext();
        var typeKey = GetTypeKey<T>();
        var entity = await context.SingletonModels
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.TypeName == typeKey, cancellationToken)
            .ConfigureAwait(false);

        if (entity is null)
        {
            return null;
        }

        return Deserialize<T>(entity.Payload);
    }

    public async Task DeleteSingletonAsync<T>(CancellationToken cancellationToken = default)
        where T : class
    {
        await using var context = CreateContext();
        var typeKey = GetTypeKey<T>();
        var entity = await context.SingletonModels
            .SingleOrDefaultAsync(e => e.TypeName == typeKey, cancellationToken)
            .ConfigureAwait(false);

        if (entity is null)
        {
            return;
        }

        context.SingletonModels.Remove(entity);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task SaveCollectionAsync<T>(IEnumerable<T> models, CancellationToken cancellationToken = default)
        where T : class
    {
        _ = models ?? throw new ArgumentNullException(nameof(models));
        var modelList = models.ToList();

        await using var context = CreateContext();
        var typeKey = GetTypeKey<T>();
        var existing = await context.CollectionModels
            .Where(e => e.TypeName == typeKey)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (existing.Count > 0)
        {
            context.CollectionModels.RemoveRange(existing);
        }

        if (modelList.Count > 0)
        {
            var timestamp = DateTime.UtcNow;
            var entities = modelList
                .Select((model, index) => new StoredCollectionModel
                {
                    TypeName = typeKey,
                    Payload = Serialize(model),
                    Position = index,
                    CreatedOn = timestamp,
                })
                .ToList();

            await context.CollectionModels.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<T>> GetCollectionAsync<T>(CancellationToken cancellationToken = default)
        where T : class
    {
        await using var context = CreateContext();
        var typeKey = GetTypeKey<T>();
        var entities = await context.CollectionModels
            .AsNoTracking()
            .Where(e => e.TypeName == typeKey)
            .OrderBy(e => e.Position)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return entities
            .Select(e => Deserialize<T>(e.Payload))
            .ToList();
    }

    public async Task AddToCollectionAsync<T>(T model, CancellationToken cancellationToken = default)
        where T : class
    {
        _ = model ?? throw new ArgumentNullException(nameof(model));

        await using var context = CreateContext();
        var typeKey = GetTypeKey<T>();
        var nextPosition = await context.CollectionModels
            .Where(e => e.TypeName == typeKey)
            .Select(e => e.Position)
            .DefaultIfEmpty(-1)
            .MaxAsync(cancellationToken)
            .ConfigureAwait(false) + 1;

        var entity = new StoredCollectionModel
        {
            TypeName = typeKey,
            Payload = Serialize(model),
            Position = nextPosition,
            CreatedOn = DateTime.UtcNow,
        };

        await context.CollectionModels.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task ClearCollectionAsync<T>(CancellationToken cancellationToken = default)
        where T : class
    {
        await using var context = CreateContext();
        var typeKey = GetTypeKey<T>();
        var entities = await context.CollectionModels
            .Where(e => e.TypeName == typeKey)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (entities.Count == 0)
        {
            return;
        }

        context.CollectionModels.RemoveRange(entities);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private PluginDataContext CreateContext() => new(connectionString);

    private static string GetTypeKey<T>() => typeof(T).AssemblyQualifiedName ?? typeof(T).FullName ?? typeof(T).Name;

    private static string Serialize<T>(T model) where T : class => JsonSerializer.Serialize(model, SerializerOptions);

    private static T Deserialize<T>(string payload) where T : class
    {
        var model = JsonSerializer.Deserialize<T>(payload, SerializerOptions);
        if (model is null)
        {
            throw new InvalidOperationException($"Unable to deserialize payload for '{typeof(T).FullName}'.");
        }

        return model;
    }
}
