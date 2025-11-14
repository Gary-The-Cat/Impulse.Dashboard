namespace Impulse.Repository.Persistent;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Impulse.Repository.Models;

public interface ILogRecordRepository
{
    Task<IReadOnlyList<LogRecordModel>> GetLogRecordsAsync(int minimumCriticality, CancellationToken cancellationToken = default);

    Task AddRecordAsync(LogRecordModel logRecord, CancellationToken cancellationToken = default);

    Task DeleteRecordsAsync(IEnumerable<Guid> recordIds, CancellationToken cancellationToken = default);
}
