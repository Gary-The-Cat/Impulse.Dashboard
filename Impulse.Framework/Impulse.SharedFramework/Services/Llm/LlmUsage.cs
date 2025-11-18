namespace Impulse.SharedFramework.Services.Llm;

public sealed record LlmUsage(int? PromptTokens, int? CompletionTokens, int? TotalTokens);
