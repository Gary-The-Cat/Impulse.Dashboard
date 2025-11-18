using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Impulse.SharedFramework.Services.Llm;

namespace Impulse.Llm.Broker;

public sealed class FileSystemLlmSessionStore : ILlmSessionStore
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
    };

    private readonly string sessionsDirectory;
    private readonly SemaphoreSlim gate = new(1, 1);

    public FileSystemLlmSessionStore()
        : this(null)
    {
    }

    public FileSystemLlmSessionStore(string? rootDirectory)
    {
        sessionsDirectory = rootDirectory ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Impulse.Dashboard",
            "LlmBroker",
            "Sessions");
        Directory.CreateDirectory(sessionsDirectory);
    }

    public async Task SaveAsync(LlmSession session, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(session);

        var snapshot = session.Clone();
        var destination = GetSessionPath(snapshot.Id);
        var tempFile = destination + ".tmp";

        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await using (var stream = File.Create(tempFile))
            {
                await JsonSerializer.SerializeAsync(stream, snapshot, SerializerOptions, cancellationToken).ConfigureAwait(false);
            }

            File.Move(tempFile, destination, overwrite: true);
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task<LlmSession?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var path = GetSessionPath(id);
            if (!File.Exists(path))
            {
                return null;
            }

            await using var stream = File.OpenRead(path);
            var session = await JsonSerializer.DeserializeAsync<LlmSession>(stream, SerializerOptions, cancellationToken)
                .ConfigureAwait(false);

            return session;
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task<IReadOnlyList<LlmSession>> GetAllAsync(CancellationToken cancellationToken)
    {
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var sessions = new List<LlmSession>();
            foreach (var file in Directory.EnumerateFiles(sessionsDirectory, "*.json", SearchOption.TopDirectoryOnly))
            {
                await using var stream = File.OpenRead(file);
                var session = await JsonSerializer.DeserializeAsync<LlmSession>(stream, SerializerOptions, cancellationToken)
                    .ConfigureAwait(false);

                if (session != null)
                {
                    sessions.Add(session);
                }
            }

            return sessions
                .OrderByDescending(s => s.LastUpdatedUtc)
                .ToList();
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var path = GetSessionPath(id);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        finally
        {
            gate.Release();
        }
    }

    private string GetSessionPath(Guid id) => Path.Combine(sessionsDirectory, $"{id}.json");
}
