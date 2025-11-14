namespace Impulse.Repository.Persistent;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Impulse.Repository.Constants;
using Impulse.Repository.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public sealed class LogRecordRepository : ILogRecordRepository
{
    private readonly string connectionString;

    public LogRecordRepository()
    {
        EnsureDataDirectory();
        connectionString = BuildConnectionString(LocationConstants.DashboardLogDatabase);
        InitializeDatabase();
    }

    public async Task<IReadOnlyList<LogRecordModel>> GetLogRecordsAsync(int minimumCriticality, CancellationToken cancellationToken = default)
    {
        await using var context = CreateContext();
        return await context.LogRecords
            .Where(r => r.Criticality >= minimumCriticality)
            .OrderBy(r => r.Timestamp)
            .ThenBy(r => r.Id)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task AddRecordAsync(LogRecordModel logRecord, CancellationToken cancellationToken = default)
    {
        _ = logRecord ?? throw new ArgumentNullException(nameof(logRecord));

        await using var context = CreateContext();
        await context.LogRecords.AddAsync(logRecord, cancellationToken).ConfigureAwait(false);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteRecordsAsync(IEnumerable<Guid> recordIds, CancellationToken cancellationToken = default)
    {
        _ = recordIds ?? throw new ArgumentNullException(nameof(recordIds));
        var ids = recordIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        if (ids.Count == 0)
        {
            return;
        }

        await using var context = CreateContext();
        var records = await context.LogRecords
            .Where(r => ids.Contains(r.Id))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (records.Count == 0)
        {
            return;
        }

        context.LogRecords.RemoveRange(records);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private static void EnsureDataDirectory()
    {
        if (string.IsNullOrWhiteSpace(LocationConstants.LocalAppData))
        {
            throw new InvalidOperationException("LocalAppData environment variable was not set.");
        }

        if (!Directory.Exists(LocationConstants.DashboardDataLocation))
        {
            Directory.CreateDirectory(LocationConstants.DashboardDataLocation);
        }
    }

    private static string BuildConnectionString(string databaseFileLocation)
    {
        var dataSourceConnectionString = $"DataSource={databaseFileLocation}";
        return new SqliteConnectionStringBuilder(dataSourceConnectionString)
        {
            ForeignKeys = true,
        }.ToString();
    }

    private void InitializeDatabase()
    {
        using var context = CreateContext();
        context.Database.EnsureCreated();
    }

    private LogDatabaseContext CreateContext() => new LogDatabaseContext(connectionString);
}
