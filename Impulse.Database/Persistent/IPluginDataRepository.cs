namespace Impulse.Repository.Persistent;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface IPluginDataRepository
{
    Task SaveSingletonAsync<T>(T model, CancellationToken cancellationToken = default)
        where T : class;

    Task<T?> GetSingletonAsync<T>(CancellationToken cancellationToken = default)
        where T : class;

    Task DeleteSingletonAsync<T>(CancellationToken cancellationToken = default)
        where T : class;

    Task SaveCollectionAsync<T>(IEnumerable<T> models, CancellationToken cancellationToken = default)
        where T : class;

    Task<IReadOnlyList<T>> GetCollectionAsync<T>(CancellationToken cancellationToken = default)
        where T : class;

    Task AddToCollectionAsync<T>(T model, CancellationToken cancellationToken = default)
        where T : class;

    Task ClearCollectionAsync<T>(CancellationToken cancellationToken = default)
        where T : class;
}
