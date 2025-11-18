using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Impulse.Llm.Configuration;

public sealed class LlmProviderConfiguration
{
    public bool IsEnabled { get; set; } = true;

    public string? ApiKey { get; set; }

    public string? BaseUrl { get; set; }

    public string? DefaultModel { get; set; }

    public int? DefaultMaxTokens { get; set; }

    public IDictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

    [JsonIgnore]
    public bool HasApiKey => !string.IsNullOrWhiteSpace(ApiKey);

    public LlmProviderConfiguration Clone()
    {
        return new LlmProviderConfiguration
        {
            IsEnabled = this.IsEnabled,
            ApiKey = this.ApiKey,
            BaseUrl = this.BaseUrl,
            DefaultModel = this.DefaultModel,
            DefaultMaxTokens = this.DefaultMaxTokens,
            Metadata = new Dictionary<string, string>(this.Metadata),
        };
    }
}
