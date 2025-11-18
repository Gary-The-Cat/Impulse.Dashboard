using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Impulse.SharedFramework.Services.Llm;

namespace Impulse.Llm.Configuration;

public sealed class FileSystemLlmSettingsService : ILlmSettingsService
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
    };

    private readonly string settingsPath;
    private readonly SemaphoreSlim gate = new(1, 1);

    public FileSystemLlmSettingsService()
        : this(null)
    {
    }

    public FileSystemLlmSettingsService(string? rootDirectory)
    {
        var baseDirectory = rootDirectory ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Impulse.Dashboard",
            "LlmBroker");

        Directory.CreateDirectory(baseDirectory);
        settingsPath = Path.Combine(baseDirectory, "settings.json");
    }

    public async Task<LlmSettings> GetSettingsAsync(CancellationToken cancellationToken)
    {
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (!File.Exists(settingsPath))
            {
                var defaults = CreateDefaultSettings();
                await PersistAsync(defaults, cancellationToken).ConfigureAwait(false);
                return defaults;
            }

            await using var stream = File.OpenRead(settingsPath);
            var settings = await JsonSerializer.DeserializeAsync<LlmSettings>(stream, SerializerOptions, cancellationToken)
                .ConfigureAwait(false);

            if (settings == null)
            {
                settings = CreateDefaultSettings();
            }

            return settings;
        }
        catch (JsonException)
        {
            var defaults = CreateDefaultSettings();
            await PersistAsync(defaults, cancellationToken).ConfigureAwait(false);
            return defaults;
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task<LlmProviderConfiguration?> GetProviderConfigurationAsync(LlmProvider provider, CancellationToken cancellationToken)
    {
        var settings = await GetSettingsAsync(cancellationToken).ConfigureAwait(false);
        return settings.Providers.TryGetValue(provider, out var configuration)
            ? configuration
            : null;
    }

    public async Task SetProviderConfigurationAsync(LlmProvider provider, LlmProviderConfiguration configuration, CancellationToken cancellationToken)
    {
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var settings = await GetSettingsInternalAsync(cancellationToken).ConfigureAwait(false);
            settings.Providers[provider] = configuration;
            await PersistAsync(settings, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            gate.Release();
        }
    }

    private async Task<LlmSettings> GetSettingsInternalAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(settingsPath))
        {
            var defaults = CreateDefaultSettings();
            await PersistAsync(defaults, cancellationToken).ConfigureAwait(false);
            return defaults;
        }

        await using var stream = File.OpenRead(settingsPath);
        var settings = await JsonSerializer.DeserializeAsync<LlmSettings>(stream, SerializerOptions, cancellationToken)
            .ConfigureAwait(false);

        return settings ?? CreateDefaultSettings();
    }

    private async Task PersistAsync(LlmSettings settings, CancellationToken cancellationToken)
    {
        var tempFile = settingsPath + ".tmp";
        await using (var stream = File.Create(tempFile))
        {
            await JsonSerializer.SerializeAsync(stream, settings, SerializerOptions, cancellationToken).ConfigureAwait(false);
        }

        File.Move(tempFile, settingsPath, overwrite: true);
    }

    private static LlmSettings CreateDefaultSettings()
    {
        var settings = new LlmSettings();
        settings.Providers[LlmProvider.OpenAi] = new LlmProviderConfiguration
        {
            DefaultModel = "gpt-4o-mini",
            DefaultMaxTokens = 8000,
        };

        settings.Providers[LlmProvider.Anthropic] = new LlmProviderConfiguration
        {
            DefaultModel = "claude-3-sonnet-20240229",
            DefaultMaxTokens = 4000,
            Metadata = new Dictionary<string, string>
            {
                ["anthropic-version"] = "2023-06-01",
            },
        };

        return settings;
    }
}
