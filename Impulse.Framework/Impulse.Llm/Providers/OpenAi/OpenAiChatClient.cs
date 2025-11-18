using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Impulse.Llm.Configuration;
using Impulse.SharedFramework.Services.Llm;

namespace Impulse.Llm.Providers.OpenAi;

public sealed class OpenAiChatClient : ILlmProviderClient, IDisposable
{
    private const string DefaultEndpoint = "https://api.openai.com/v1";
    private const string ChatPath = "chat/completions";
    private readonly HttpClient httpClient;
    private bool disposed;

    public OpenAiChatClient() => httpClient = new HttpClient();

    public LlmProvider Provider => LlmProvider.OpenAi;

    public bool SupportsStreaming => true;

    public async Task<LlmProviderResponse> SendAsync(LlmProviderRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Options.Stream)
        {
            throw new NotSupportedException("Streaming responses are not implemented yet.");
        }

        var configuration = request.Configuration ?? throw new InvalidOperationException("OpenAI configuration is not available.");
        if (string.IsNullOrWhiteSpace(configuration.ApiKey))
        {
            throw new InvalidOperationException("OpenAI API key has not been configured.");
        }

        var model = request.Options.Model
            ?? configuration.DefaultModel
            ?? "gpt-4o-mini";

        var payload = new OpenAiRequest
        {
            Model = model,
            Messages = request.Messages.Select(MapToOpenAiMessage).ToList(),
            MaxTokens = request.Options.MaxTokens ?? configuration.DefaultMaxTokens,
            Temperature = request.Options.Temperature,
            Stop = request.Options.StopSequences?.ToArray(),
        };

        var baseUrl = configuration.BaseUrl ?? DefaultEndpoint;
        var uri = new Uri(new Uri(baseUrl.TrimEnd('/') + "/"), ChatPath);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri);
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", configuration.ApiKey);
        httpRequest.Content = new StringContent(JsonSerializer.Serialize(payload, SerializerOptions), Encoding.UTF8, "application/json");

        var response = await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"OpenAI request failed ({response.StatusCode}): {body}");
        }

        var result = JsonSerializer.Deserialize<OpenAiResponse>(body, SerializerOptions)
            ?? throw new InvalidOperationException("Failed to parse OpenAI response.");

        var message = result.Choices?.FirstOrDefault()?.Message?.Content;
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new InvalidOperationException("OpenAI response did not include any message content.");
        }

        var usage = result.Usage != null
            ? new LlmUsage(result.Usage.PromptTokens, result.Usage.CompletionTokens, result.Usage.TotalTokens)
            : null;

        return new LlmProviderResponse(message, result.Model ?? model, usage);
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

    private static OpenAiMessage MapToOpenAiMessage(LlmMessage message)
    {
        var role = message.Role switch
        {
            LlmMessageRole.System => "system",
            LlmMessageRole.Assistant => "assistant",
            LlmMessageRole.Tool => "tool",
            _ => "user",
        };

        return new OpenAiMessage
        {
            Role = role,
            Content = message.Content,
        };
    }

    private sealed record OpenAiRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; init; } = string.Empty;

        [JsonPropertyName("messages")]
        public List<OpenAiMessage> Messages { get; init; } = new();

        [JsonPropertyName("max_tokens")]
        public int? MaxTokens { get; init; }

        [JsonPropertyName("temperature")]
        public float? Temperature { get; init; }

        [JsonPropertyName("stop")]
        public IReadOnlyList<string>? Stop { get; init; }
    }

    private sealed record OpenAiMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; init; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; init; } = string.Empty;
    }

    private sealed record OpenAiResponse
    {
        [JsonPropertyName("choices")]
        public List<OpenAiChoice>? Choices { get; init; }

        [JsonPropertyName("usage")]
        public OpenAiUsage? Usage { get; init; }

        [JsonPropertyName("model")]
        public string? Model { get; init; }
    }

    private sealed record OpenAiChoice
    {
        [JsonPropertyName("message")]
        public OpenAiMessage? Message { get; init; }
    }

    private sealed record OpenAiUsage
    {
        [JsonPropertyName("prompt_tokens")]
        public int? PromptTokens { get; init; }

        [JsonPropertyName("completion_tokens")]
        public int? CompletionTokens { get; init; }

        [JsonPropertyName("total_tokens")]
        public int? TotalTokens { get; init; }
    }

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };
}
