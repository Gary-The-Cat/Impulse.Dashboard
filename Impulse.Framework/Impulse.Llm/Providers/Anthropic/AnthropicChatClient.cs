using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Impulse.Llm.Configuration;
using Impulse.SharedFramework.Services.Llm;

namespace Impulse.Llm.Providers.Anthropic;

public sealed class AnthropicChatClient : ILlmProviderClient, IDisposable
{
    private const string DefaultEndpoint = "https://api.anthropic.com";
    private const string MessagesPath = "v1/messages";
    private const string DefaultVersion = "2023-06-01";

    private readonly HttpClient httpClient;
    private bool disposed;

    public AnthropicChatClient() => httpClient = new HttpClient();

    public LlmProvider Provider => LlmProvider.Anthropic;

    public bool SupportsStreaming => true;

    public async Task<LlmProviderResponse> SendAsync(LlmProviderRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Options.Stream)
        {
            throw new NotSupportedException("Streaming responses are not implemented yet.");
        }

        var configuration = request.Configuration ?? throw new InvalidOperationException("Anthropic configuration is not available.");
        if (string.IsNullOrWhiteSpace(configuration.ApiKey))
        {
            throw new InvalidOperationException("Anthropic API key has not been configured.");
        }

        var model = request.Options.Model
            ?? configuration.DefaultModel
            ?? "claude-3-sonnet-20240229";
        var maxTokens = request.Options.MaxTokens
            ?? configuration.DefaultMaxTokens
            ?? 1024;

        var payload = new AnthropicRequest
        {
            Model = model,
            MaxTokens = maxTokens,
            System = BuildSystemPrompt(request.Messages),
            Messages = request.Messages
                .Where(m => m.Role is not LlmMessageRole.System)
                .Select(MapToAnthropicMessage)
                .ToList(),
            StopSequences = request.Options.StopSequences?.ToArray(),
        };

        var baseUrl = configuration.BaseUrl ?? DefaultEndpoint;
        var uri = new Uri(new Uri(baseUrl.TrimEnd('/') + "/"), MessagesPath);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri);
        httpRequest.Headers.Add("x-api-key", configuration.ApiKey);
        httpRequest.Headers.Add("anthropic-version", configuration.Metadata.TryGetValue("anthropic-version", out var version) ? version : DefaultVersion);
        httpRequest.Content = new StringContent(JsonSerializer.Serialize(payload, SerializerOptions), Encoding.UTF8, "application/json");

        var response = await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Anthropic request failed ({response.StatusCode}): {body}");
        }

        var result = JsonSerializer.Deserialize<AnthropicResponse>(body, SerializerOptions)
            ?? throw new InvalidOperationException("Failed to parse Anthropic response.");

        var message = result.Content?.Where(c => string.Equals(c.Type, "text", StringComparison.OrdinalIgnoreCase))
            .Select(c => c.Text)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();

        if (message == null || message.Count == 0)
        {
            throw new InvalidOperationException("Anthropic response did not include any message content.");
        }

        var usage = result.Usage != null
            ? new LlmUsage(result.Usage.InputTokens, result.Usage.OutputTokens, result.Usage.OutputTokens.HasValue && result.Usage.InputTokens.HasValue
                ? result.Usage.InputTokens + result.Usage.OutputTokens
                : null)
            : null;

        return new LlmProviderResponse(string.Join("\n\n", message), result.Model ?? model, usage);
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        httpClient.Dispose();
    }

    private static string? BuildSystemPrompt(IReadOnlyList<LlmMessage> messages)
    {
        var systemPrompts = messages
            .Where(m => m.Role == LlmMessageRole.System)
            .Select(m => m.Content)
            .Where(text => !string.IsNullOrWhiteSpace(text))
            .ToList();

        if (!systemPrompts.Any())
        {
            return null;
        }

        return string.Join("\n\n", systemPrompts);
    }

    private static AnthropicMessage MapToAnthropicMessage(LlmMessage message)
    {
        var role = message.Role == LlmMessageRole.Assistant ? "assistant" : "user";
        var content = message.Role switch
        {
            LlmMessageRole.Tool => $"[Tool] {message.Content}",
            _ => message.Content,
        };

        return new AnthropicMessage
        {
            Role = role,
            Content = new List<AnthropicTextBlock>
            {
                new AnthropicTextBlock
                {
                    Text = content,
                }
            },
        };
    }

    private sealed record AnthropicRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; init; } = string.Empty;

        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; init; }

        [JsonPropertyName("system")]
        public string? System { get; init; }

        [JsonPropertyName("messages")]
        public List<AnthropicMessage> Messages { get; init; } = new();

        [JsonPropertyName("stop_sequences")]
        public IReadOnlyList<string>? StopSequences { get; init; }
    }

    private sealed record AnthropicMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; init; } = string.Empty;

        [JsonPropertyName("content")]
        public List<AnthropicTextBlock> Content { get; init; } = new();
    }

    private sealed record AnthropicTextBlock
    {
        [JsonPropertyName("type")]
        public string Type { get; init; } = "text";

        [JsonPropertyName("text")]
        public string Text { get; init; } = string.Empty;
    }

    private sealed record AnthropicResponse
    {
        [JsonPropertyName("content")]
        public List<AnthropicTextBlock>? Content { get; init; }

        [JsonPropertyName("usage")]
        public AnthropicUsage? Usage { get; init; }

        [JsonPropertyName("model")]
        public string? Model { get; init; }
    }

    private sealed record AnthropicUsage
    {
        [JsonPropertyName("input_tokens")]
        public int? InputTokens { get; init; }

        [JsonPropertyName("output_tokens")]
        public int? OutputTokens { get; init; }
    }

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };
}
