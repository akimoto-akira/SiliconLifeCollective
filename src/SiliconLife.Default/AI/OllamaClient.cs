// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Ollama API request model (native Ollama format)
/// </summary>
internal class OllamaRequest
{
    public string Model { get; set; } = string.Empty;
    public List<OllamaMessage> Messages { get; set; } = new();
    [JsonPropertyName("tools")]
    public List<OllamaTool>? Tools { get; set; }
    public double? Temperature { get; set; }
    [JsonPropertyName("num_predict")]
    public int? NumPredict { get; set; }
    public bool Stream { get; set; } = false;
}

/// <summary>
/// Ollama API message model
/// </summary>
internal class OllamaMessage
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Thinking { get; set; }
    [JsonPropertyName("tool_calls")]
    public List<OllamaToolCall>? ToolCalls { get; set; }
    [JsonPropertyName("tool_call_id")]
    public string? ToolCallId { get; set; }
}

/// <summary>
/// Ollama tool call in assistant response
/// </summary>
internal class OllamaToolCall
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    public OllamaToolCallFunction? Function { get; set; }
}

/// <summary>
/// Ollama tool call function definition
/// </summary>
internal class OllamaToolCallFunction
{
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Arguments as a JSON object (Ollama returns arguments as an object, not a string)
    /// </summary>
    public Dictionary<string, object>? Arguments { get; set; }
}

/// <summary>
/// Ollama tool definition for request
/// </summary>
internal class OllamaTool
{
    public string Type { get; set; } = "function";
    public OllamaToolFunction? Function { get; set; }
}

/// <summary>
/// Ollama tool function definition
/// </summary>
internal class OllamaToolFunction
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Dictionary<string, object>? Parameters { get; set; }
}

/// <summary>
/// Ollama API response model
/// </summary>
internal class OllamaResponse
{
    public string Model { get; set; } = string.Empty;
    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }
    public OllamaMessage? Message { get; set; }
    public bool Done { get; set; }
    [JsonPropertyName("done_reason")]
    public string? DoneReason { get; set; }
    [JsonPropertyName("total_duration")]
    public long? TotalDuration { get; set; }
    [JsonPropertyName("load_duration")]
    public long? LoadDuration { get; set; }
    [JsonPropertyName("prompt_eval_count")]
    public int? PromptEvalCount { get; set; }
    [JsonPropertyName("prompt_eval_duration")]
    public long? PromptEvalDuration { get; set; }
    [JsonPropertyName("eval_count")]
    public int? EvalCount { get; set; }
    [JsonPropertyName("eval_duration")]
    public long? EvalDuration { get; set; }
    public string? Error { get; set; }
}

/// <summary>
/// Ollama AI client implementation.
/// Supports tool calling (function calling) via Ollama native API.
/// Models that don't support tools will degrade to plain text responses.
/// </summary>
public class OllamaClient : IAIClient
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<OllamaClient>();
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Gets the endpoint URL of the Ollama service
    /// </summary>
    public string Endpoint { get; }

    /// <summary>
    /// Gets the default model name
    /// </summary>
    public string DefaultModel { get; }

    /// <summary>
    /// Ollama supports both streaming and non-streaming modes.
    /// Returns null to indicate both are supported, with streaming preferred.
    /// </summary>
    public bool? StreamingMode => null;

    /// <summary>
    /// Creates a new Ollama client with the specified endpoint
    /// </summary>
    public OllamaClient(string endpoint = "http://localhost:11434", string defaultModel = "llama3.2")
    {
        Endpoint = endpoint.TrimEnd('/');
        DefaultModel = defaultModel;
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(5)
        };
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    /// <summary>
    /// Sends a chat request to Ollama and returns the response
    /// </summary>
    public AIResponse Chat(AIRequest request)
    {
        return ChatAsync(request).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Sends a chat request to Ollama and returns the response asynchronously.
    /// Supports tool definitions in the request and tool_calls in the response.
    /// </summary>
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        try
        {
            string model = string.IsNullOrEmpty(request.Model) ? DefaultModel : request.Model;

            _logger.Info("AI request: model={0}, messages={1}, hasTools={2}", model, request.Messages.Count, request.Tools != null && request.Tools.Count > 0);

            OllamaRequest ollamaRequest = new OllamaRequest
            {
                Model = model,
                Messages = MapMessages(request.Messages),
                Stream = false
            };

            // Add tool definitions if present
            if (request.Tools != null && request.Tools.Count > 0)
            {
                ollamaRequest.Tools = request.Tools.Select(t => new OllamaTool
                {
                    Type = "function",
                    Function = new OllamaToolFunction
                    {
                        Name = t.Name,
                        Description = t.Description,
                        Parameters = t.Parameters
                    }
                }).ToList();
            }

            string json = JsonSerializer.Serialize(ollamaRequest, _jsonOptions);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            AIResponse? result = await SendAndParseAsync(content);
            if (result != null)
            {
                _logger.Info("AI response: model={0}, tokens={1}/{2}/{3}, hasToolCalls={4}", model, result.PromptTokens, result.CompletionTokens, result.TotalTokens, result.HasToolCalls);
                return result;
            }

            _logger.Warn("Model '{0}' is still loading, retrying in 3s...", model);
            await Task.Delay(3000);
            result = await SendAndParseAsync(content);
            if (result != null)
            {
                _logger.Info("AI response (after retry): model={0}, tokens={1}/{2}/{3}, hasToolCalls={4}", model, result.PromptTokens, result.CompletionTokens, result.TotalTokens, result.HasToolCalls);
                return result;
            }

            return AIResponse.Failed($"Model '{model}' is still loading after retry.");
        }
        catch (HttpRequestException ex)
        {
            _logger.Error("AI connection error: {0}", ex.Message);
            return AIResponse.Failed($"Connection error: {ex.Message}");
        }
        catch (TaskCanceledException ex)
        {
            _logger.Warn("AI request timeout: {0}", ex.Message);
            return AIResponse.Failed($"Request timeout: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.Error("AI request failed: {0}", ex.Message);
            return AIResponse.Failed($"Unexpected error: {ex.Message}");
        }
    }

    /// <summary>
    /// Maps core Message list to Ollama message list, preserving tool_calls and tool role
    /// </summary>
    private static List<OllamaMessage> MapMessages(List<Message> messages)
    {
        List<OllamaMessage> result = new();

        foreach (Message msg in messages)
        {
            OllamaMessage ollamaMsg = new OllamaMessage
            {
                Role = MapRole(msg.Role),
                Content = msg.Content,
                Thinking = msg.Thinking
            };

            // Preserve tool_calls in assistant messages
            if (msg.ToolCalls != null && msg.ToolCalls.Count > 0)
            {
                ollamaMsg.ToolCalls = msg.ToolCalls.Select(tc => new OllamaToolCall
                {
                    Id = tc.Id,
                    Function = new OllamaToolCallFunction
                    {
                        Name = tc.Name,
                        Arguments = tc.Arguments
                    }
                }).ToList();
            }

            // Set tool_call_id for tool result messages
            if (msg.Role == MessageRole.Tool && !string.IsNullOrEmpty(msg.ToolCallId))
            {
                ollamaMsg.ToolCallId = msg.ToolCallId;
            }

            result.Add(ollamaMsg);
        }

        return result;
    }

    /// <summary>
    /// Sends a streaming chat request to Ollama, yielding incremental token responses.
    /// Each yielded AIResponse contains only the new token content.
    /// The final yield has IsStreamFinal = true and contains usage statistics.
    /// </summary>
    public async IAsyncEnumerable<AIResponse> ChatStreamAsync(AIRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string model = string.IsNullOrEmpty(request.Model) ? DefaultModel : request.Model;

        _logger.Info("AI stream started: model={0}", model);

        OllamaRequest ollamaRequest = new OllamaRequest
        {
            Model = model,
            Messages = MapMessages(request.Messages),
            Stream = true
        };

        if (request.Tools != null && request.Tools.Count > 0)
        {
            ollamaRequest.Tools = request.Tools.Select(t => new OllamaTool
            {
                Type = "function",
                Function = new OllamaToolFunction
                {
                    Name = t.Name,
                    Description = t.Description,
                    Parameters = t.Parameters
                }
            }).ToList();
        }

        string json = JsonSerializer.Serialize(ollamaRequest, _jsonOptions);
        StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage? response = null;
        AIResponse? errorResponse = null;
        try
        {
            response = await _httpClient.PostAsync($"{Endpoint}/api/chat", content, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.Error("AI stream connection error: {0}", ex.Message);
            errorResponse = AIResponse.Failed($"Connection error: {ex.Message}");
        }
        catch (OperationCanceledException)
        {
            _logger.Debug("AI stream cancelled");
            yield break;
        }

        if (errorResponse != null)
        {
            yield return errorResponse;
            yield break;
        }

        Stream stream = await response!.Content.ReadAsStreamAsync(cancellationToken);
        using StreamReader reader = new StreamReader(stream);

        List<ToolCall>? accumulatedToolCalls = null;
        string accumulatedContent = string.Empty;
        string? accumulatedThinking = null;

        while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            string? line = await reader.ReadLineAsync(cancellationToken);
            if (string.IsNullOrEmpty(line))
                continue;

            OllamaResponse? ollamaResponse;
            try
            {
                ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(line, _jsonOptions);
            }
            catch
            {
                continue;
            }

            if (ollamaResponse == null)
                continue;

            if (!string.IsNullOrEmpty(ollamaResponse.Error))
            {
                yield return AIResponse.Failed(ollamaResponse.Error);
                yield break;
            }

            if (ollamaResponse.Message != null)
            {
                string tokenContent = ollamaResponse.Message.Content ?? string.Empty;
                string? tokenThinking = ollamaResponse.Message.Thinking;

                if (ollamaResponse.Message.ToolCalls != null && ollamaResponse.Message.ToolCalls.Count > 0)
                {
                    accumulatedToolCalls = new List<ToolCall>();
                    foreach (OllamaToolCall? ollamaToolCall in ollamaResponse.Message.ToolCalls)
                    {
                        if (ollamaToolCall?.Function != null)
                        {
                            accumulatedToolCalls.Add(new ToolCall
                            {
                                Id = ollamaToolCall.Id ?? Guid.NewGuid().ToString(),
                                Name = ollamaToolCall.Function.Name,
                                Arguments = ollamaToolCall.Function.Arguments ?? new Dictionary<string, object>()
                            });
                        }
                    }
                }

                if (!string.IsNullOrEmpty(tokenContent) || tokenThinking != null)
                {
                    AIResponse chunk = new AIResponse
                    {
                        Model = ollamaResponse.Model,
                        Content = tokenContent,
                        Thinking = tokenThinking,
                        Success = true,
                        IsStreamFinal = false
                    };
                    yield return chunk;
                }
            }

            if (ollamaResponse.Done)
            {
                _logger.Info("AI stream completed: model={0}, totalTokens={1}", ollamaResponse.Model, (ollamaResponse.PromptEvalCount ?? 0) + (ollamaResponse.EvalCount ?? 0));

                AIResponse finalChunk = new AIResponse
                {
                    Model = ollamaResponse.Model,
                    Content = string.Empty,
                    Success = true,
                    IsStreamFinal = true,
                    PromptTokens = ollamaResponse.PromptEvalCount,
                    CompletionTokens = ollamaResponse.EvalCount,
                    TotalTokens = (ollamaResponse.PromptEvalCount ?? 0) + (ollamaResponse.EvalCount ?? 0)
                };

                if (accumulatedToolCalls != null && accumulatedToolCalls.Count > 0)
                {
                    finalChunk.ToolCalls = accumulatedToolCalls;
                }

                yield return finalChunk;
                yield break;
            }
        }
    }

    /// <summary>
    /// Sends the request and parses the response.
    /// Returns null if done_reason is "load" (model still loading, caller should retry).
    /// Returns a non-null AIResponse for success or other failures.
    /// </summary>
    private async Task<AIResponse?> SendAndParseAsync(StringContent content)
    {
        HttpResponseMessage response = await _httpClient.PostAsync($"{Endpoint}/api/chat", content);
        response.EnsureSuccessStatusCode();

        string responseJson = await response.Content.ReadAsStringAsync();
        OllamaResponse? ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(responseJson, _jsonOptions);

        if (ollamaResponse == null)
            return AIResponse.Failed("Failed to deserialize Ollama response");

        if (!string.IsNullOrEmpty(ollamaResponse.Error))
            return AIResponse.Failed(ollamaResponse.Error);

        if (ollamaResponse.DoneReason == "load")
            return null; // Signal caller to retry

        // Build AIResponse with optional tool_calls
        AIResponse aiResponse = new AIResponse
        {
            Model = ollamaResponse.Model,
            Content = ollamaResponse.Message?.Content ?? string.Empty,
            Thinking = ollamaResponse.Message?.Thinking,
            PromptTokens = ollamaResponse.PromptEvalCount,
            CompletionTokens = ollamaResponse.EvalCount,
            TotalTokens = (ollamaResponse.PromptEvalCount ?? 0) + (ollamaResponse.EvalCount ?? 0),
            Success = true
        };

        // Parse tool_calls from response if present
        if (ollamaResponse.Message?.ToolCalls != null && ollamaResponse.Message.ToolCalls.Count > 0)
        {
            aiResponse.ToolCalls = new List<ToolCall>();
            foreach (OllamaToolCall? ollamaToolCall in ollamaResponse.Message.ToolCalls)
            {
                if (ollamaToolCall?.Function != null)
                {
                    aiResponse.ToolCalls.Add(new ToolCall
                    {
                        Id = ollamaToolCall.Id ?? Guid.NewGuid().ToString(),
                        Name = ollamaToolCall.Function.Name,
                        Arguments = ollamaToolCall.Function.Arguments ?? new Dictionary<string, object>()
                    });
                }
            }
        }

        return aiResponse;
    }

    /// <summary>
    /// Sends a chat request with a single user message
    /// </summary>
    public AIResponse Chat(string userMessage)
    {
        AIRequest request = new AIRequest(DefaultModel);
        request.AddMessage(MessageRole.User, userMessage);
        return Chat(request);
    }

    /// <summary>
    /// Sends a chat request with a single user message asynchronously
    /// </summary>
    public async Task<AIResponse> ChatAsync(string userMessage)
    {
        AIRequest request = new AIRequest(DefaultModel);
        request.AddMessage(MessageRole.User, userMessage);
        return await ChatAsync(request);
    }

    /// <summary>
    /// Sends a chat request with system prompt and user message
    /// </summary>
    public AIResponse Chat(string systemPrompt, string userMessage)
    {
        AIRequest request = new AIRequest(DefaultModel);
        request.AddMessage(MessageRole.System, systemPrompt);
        request.AddMessage(MessageRole.User, userMessage);
        return Chat(request);
    }

    /// <summary>
    /// Sends a chat request with system prompt and user message asynchronously
    /// </summary>
    public async Task<AIResponse> ChatAsync(string systemPrompt, string userMessage)
    {
        AIRequest request = new AIRequest(DefaultModel);
        request.AddMessage(MessageRole.System, systemPrompt);
        request.AddMessage(MessageRole.User, userMessage);
        return await ChatAsync(request);
    }

    /// <summary>
    /// Maps MessageRole to Ollama role string
    /// </summary>
    private static string MapRole(MessageRole role)
    {
        return role switch
        {
            MessageRole.System => "system",
            MessageRole.User => "user",
            MessageRole.Assistant => "assistant",
            MessageRole.Tool => "tool",
            _ => "user"
        };
    }

    public AIResponse Generate(string prompt)
    {
        return GenerateAsync(prompt).GetAwaiter().GetResult();
    }

    public async Task<AIResponse> GenerateAsync(string prompt)
    {
        return await GenerateAsync(null, prompt);
    }

    public AIResponse Generate(string systemPrompt, string prompt)
    {
        return GenerateAsync(systemPrompt, prompt).GetAwaiter().GetResult();
    }

    public async Task<AIResponse> GenerateAsync(string? systemPrompt, string prompt)
    {
        var requestBody = new
        {
            model = DefaultModel,
            prompt = prompt,
            system = systemPrompt,
            stream = false
        };

        string json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        try
        {
            _logger.Debug("AI generate request: model={0}", DefaultModel);
            HttpResponseMessage response = await _httpClient.PostAsync($"{Endpoint}/api/generate", content);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                return new AIResponse
                {
                    Success = false,
                    ErrorMessage = $"Ollama Generate failed: {response.StatusCode} - {error}"
                };
            }

            string responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);
            string responseText = doc.RootElement.GetProperty("response").GetString() ?? "";

            return new AIResponse
            {
                Success = true,
                Content = responseText
            };
        }
        catch (Exception ex)
        {
            _logger.Error("AI generate error: {0}", ex.Message);
            return new AIResponse
            {
                Success = false,
                ErrorMessage = $"Ollama Generate error: {ex.Message}"
            };
        }
    }
}
