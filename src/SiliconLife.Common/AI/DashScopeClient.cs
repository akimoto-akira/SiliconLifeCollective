// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using SiliconLife.Collective;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SiliconLife.Common.AI;

/// <summary>
/// DashScope (Alibaba Cloud Bailian) API request model (OpenAI-compatible format)
/// </summary>
internal class DashScopeRequest
{
    public string Model { get; set; } = string.Empty;
    public List<DashScopeMessage> Messages { get; set; } = new();
    [JsonPropertyName("tools")]
    public List<DashScopeTool>? Tools { get; set; }
    public bool Stream { get; set; } = false;
}

/// <summary>
/// DashScope API message model (OpenAI-compatible format)
/// </summary>
internal class DashScopeMessage
{
    public string Role { get; set; } = string.Empty;
    public string? Content { get; set; }
    [JsonPropertyName("reasoning_content")]
    public string? ReasoningContent { get; set; }
    [JsonPropertyName("tool_calls")]
    public List<DashScopeToolCall>? ToolCalls { get; set; }
    [JsonPropertyName("tool_call_id")]
    public string? ToolCallId { get; set; }
}

/// <summary>
/// DashScope tool call in assistant response
/// </summary>
internal class DashScopeToolCall
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    public string Type { get; set; } = "function";
    public DashScopeToolCallFunction? Function { get; set; }
}

/// <summary>
/// DashScope tool call function definition
/// </summary>
internal class DashScopeToolCallFunction
{
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Arguments as a JSON string (DashScope/OpenAI returns arguments as a string, not an object)
    /// </summary>
    public string? Arguments { get; set; }
}

/// <summary>
/// DashScope tool definition for request
/// </summary>
internal class DashScopeTool
{
    public string Type { get; set; } = "function";
    public DashScopeToolFunction? Function { get; set; }
}

/// <summary>
/// DashScope tool function definition
/// </summary>
internal class DashScopeToolFunction
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Dictionary<string, object>? Parameters { get; set; }
}

/// <summary>
/// Alibaba Cloud DashScope (Bailian) AI client implementation.
/// Uses OpenAI-compatible API format with Bearer token authentication.
/// Supports tool calling (function calling), streaming, and reasoning content.
/// </summary>
public class DashScopeClient : IAIClient
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<DashScopeClient>();
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    // Rate control related fields
    private readonly SemaphoreSlim _rateSemaphore = new SemaphoreSlim(1, 1);
    private DateTime _lastRequestEndTime = DateTime.MinValue;
    private readonly TimeSpan _minRequestInterval;
    private const int MaxRetryCount = 3;
    private const int BaseRetryDelayMs = 1000;

    /// <summary>
    /// Gets the endpoint URL of the DashScope service
    /// </summary>
    public string Endpoint { get; }

    /// <summary>
    /// Gets the default model name
    /// </summary>
    public string DefaultModel { get; }

    /// <summary>
    /// DashScope supports both streaming and non-streaming modes.
    /// Returns null to indicate both are supported, with streaming preferred.
    /// </summary>
    public bool? StreamingMode => null;

    /// <summary>
    /// DashScope supports function/tool calling via the OpenAI-compatible tools API.
    /// </summary>
    public bool? SupportsToolCalls => true;

    /// <summary>
    /// Gets the context window token capacity for the current DashScope model.
    /// Uses a mapping of known Qwen model names to their context window sizes.
    /// Returns null for unknown models (ContextManager will fall back to MaxContextMessages).
    /// </summary>
    public int? ContextWindowTokens => GetContextWindowForModel(DefaultModel);

    /// <summary>
    /// DashScope supports vision input for qwen-vl series models.
    /// </summary>
    public bool? SupportsVision => GetSupportsVisionForModel(DefaultModel);

    /// <summary>
    /// DashScope audio support is not yet mapped; returns null (unknown).
    /// </summary>
    public bool? SupportsAudio => null;

    /// <summary>
    /// Creates a new DashScope client with the specified configuration
    /// </summary>
    /// <param name="apiKey">DashScope API key for authentication</param>
    /// <param name="endpoint">The DashScope API endpoint URL</param>
    /// <param name="defaultModel">The default model name to use</param>
    /// <param name="minRequestIntervalMs">Minimum interval between requests in milliseconds (default: 200ms for 300 RPM)</param>
    public DashScopeClient(string apiKey, string endpoint, string defaultModel, int minRequestIntervalMs = 200)
    {
        _apiKey = apiKey;
        Endpoint = endpoint.TrimEnd('/');
        DefaultModel = defaultModel;
        _minRequestInterval = TimeSpan.FromMilliseconds(minRequestIntervalMs);
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(5)
        };
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    /// <summary>
    /// Sends a chat request to DashScope and returns the response
    /// </summary>
    public AIResponse Chat(AIRequest request)
    {
        return ChatAsync(request).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Sends a chat request to DashScope and returns the response asynchronously.
    /// Supports tool definitions in the request and tool_calls in the response.
    /// Implements two-layer rate control:
    /// 1. Self rate control: enforces minimum interval between requests
    /// 2. Server rate limit: handles 429 errors with exponential backoff retry
    /// </summary>
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        int retryCount = 0;
        Exception? lastException = null;

        while (retryCount <= MaxRetryCount)
        {
            try
            {
                // First-layer rate control: self-imposed rate limiting
                await EnforceRateLimitAsync();

                string model = string.IsNullOrEmpty(request.Model) ? DefaultModel : request.Model;

                _logger.Info(null, "DashScope request: model={0}, messages={1}, hasTools={2}, retry={3}",
                    model, request.Messages.Count, request.Tools != null && request.Tools.Count > 0, retryCount);

                string requestBody = BuildRequestBody(request, model, stream: false);
                StringContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                DateTime requestStartTime = DateTime.UtcNow;
                HttpResponseMessage response = await _httpClient.PostAsync(Endpoint, content);
                DateTime requestEndTime = DateTime.UtcNow;

                if (!response.IsSuccessStatusCode)
                {
                    string errorBody = await response.Content.ReadAsStringAsync();
                    _logger.Error(null, "DashScope HTTP error: {0} {1}", (int)response.StatusCode, errorBody);

                    // Second-layer rate control: handle server rate limiting (429 errors)
                    if ((int)response.StatusCode == 429)
                    {
                        retryCount++;
                        if (retryCount <= MaxRetryCount)
                        {
                            TimeSpan waitTime = CalculateRetryDelay(retryCount, errorBody);
                            _logger.Warn(null, "DashScope rate limited (429), retry {0}/{1} after {2}ms",
                                retryCount, MaxRetryCount, waitTime.TotalMilliseconds);
                            await Task.Delay(waitTime);
                            continue;
                        }
                        return AIResponse.Failed($"Rate limit exceeded after {MaxRetryCount} retries: {errorBody}");
                    }

                    return AIResponse.Failed($"HTTP {(int)response.StatusCode}: {errorBody}");
                }

                string json = await response.Content.ReadAsStringAsync();
                AIResponse result = ParseChatResponse(json);

                // Update the last request end time
                UpdateLastRequestTime(requestEndTime);

                _logger.Info(null, "DashScope response: model={0}, tokens={1}/{2}/{3}, hasToolCalls={4}",
                    model, result.PromptTokens, result.CompletionTokens, result.TotalTokens, result.HasToolCalls);

                return result;
            }
            catch (HttpRequestException ex)
            {
                lastException = ex;
                _logger.Error(null, "DashScope connection error: {0}", ex.Message);
                
                // Retry on network connection errors
                retryCount++;
                if (retryCount <= MaxRetryCount)
                {
                    TimeSpan waitTime = CalculateRetryDelay(retryCount, null);
                    _logger.Warn(null, "DashScope connection error, retry {0}/{1} after {2}ms",
                        retryCount, MaxRetryCount, waitTime.TotalMilliseconds);
                    await Task.Delay(waitTime);
                    continue;
                }
                return AIResponse.Failed($"Connection error after {MaxRetryCount} retries: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                _logger.Warn(null, "DashScope request timeout: {0}", ex.Message);
                return AIResponse.Failed($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                lastException = ex;
                _logger.Error(null, "DashScope request failed: {0}", ex.Message);
                
                // Retry on other exceptions
                retryCount++;
                if (retryCount <= MaxRetryCount)
                {
                    TimeSpan waitTime = CalculateRetryDelay(retryCount, null);
                    _logger.Warn(null, "DashScope unexpected error, retry {0}/{1} after {2}ms",
                        retryCount, MaxRetryCount, waitTime.TotalMilliseconds);
                    await Task.Delay(waitTime);
                    continue;
                }
                return AIResponse.Failed($"Unexpected error after {MaxRetryCount} retries: {ex.Message}");
            }
        }

        return AIResponse.Failed($"Failed after {MaxRetryCount} retries: {lastException?.Message}");
    }

    /// <summary>
    /// Sends a streaming chat request to DashScope, yielding incremental token responses.
    /// Each yielded AIResponse contains only the new token content.
    /// The final yield has IsStreamFinal = true and contains usage statistics.
    /// </summary>
    public async IAsyncEnumerable<AIResponse> ChatStreamAsync(
        AIRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string model = string.IsNullOrEmpty(request.Model) ? DefaultModel : request.Model;

        _logger.Info(null, "DashScope stream started: model={0}", model);

        string requestBody = BuildRequestBody(request, model, stream: true);
        StringContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) { Content = content };

        HttpResponseMessage? response = null;
        AIResponse? errorResponse = null;
        try
        {
            response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                string errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.Error(null, "DashScope stream HTTP error: {0} {1}", (int)response.StatusCode, errorBody);
                errorResponse = AIResponse.Failed($"HTTP {(int)response.StatusCode}: {errorBody}");
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.Error(null, "DashScope stream connection error: {0}", ex.Message);
            errorResponse = AIResponse.Failed($"Connection error: {ex.Message}");
        }
        catch (OperationCanceledException)
        {
            _logger.Debug(null, "DashScope stream cancelled");
            yield break;
        }

        if (errorResponse != null)
        {
            yield return errorResponse;
            yield break;
        }

        using var stream = await response!.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        // Accumulate tool_calls across multiple chunks (streaming sends them incrementally)
        var accumulatedToolCalls = new Dictionary<int, (string Id, string Name, StringBuilder Args)>();

        while (!reader.EndOfStream)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string? line = await reader.ReadLineAsync(cancellationToken);

            if (string.IsNullOrEmpty(line))
                continue;

            // Support both "data: {...}" and "data:{...}" formats
            string data;
            if (line.StartsWith("data: "))
                data = line.Substring(6);
            else if (line.StartsWith("data:"))
                data = line.Substring(5);
            else
                continue;

            if (data == "[DONE]")
                break;

            JsonElement root;
            try
            {
                using var doc = JsonDocument.Parse(data);
                root = doc.RootElement.Clone();
            }
            catch
            {
                continue;
            }

            if (!root.TryGetProperty("choices", out var choices) ||
                choices.GetArrayLength() == 0)
                continue;

            JsonElement choice = choices[0];

            // Defensive: skip chunks without delta
            if (!choice.TryGetProperty("delta", out var delta))
                continue;

            string? finishReason = choice.TryGetProperty("finish_reason", out var fr) &&
                                   fr.ValueKind != JsonValueKind.Null
                ? fr.GetString()
                : null;

            var chunk = new AIResponse { Success = true };

            // Extract content
            if (delta.TryGetProperty("content", out var contentElem) &&
                contentElem.ValueKind == JsonValueKind.String)
            {
                chunk.Content = contentElem.GetString() ?? "";
            }

            // Extract reasoning content (thinking)
            if (delta.TryGetProperty("reasoning_content", out var thinkElem) &&
                thinkElem.ValueKind == JsonValueKind.String)
            {
                chunk.Thinking = thinkElem.GetString();
            }

            // Accumulate tool_calls (wrapped in try/catch for robustness)
            if (delta.TryGetProperty("tool_calls", out var tcDelta))
            {
                try
                {
                    foreach (JsonElement tc in tcDelta.EnumerateArray())
                    {
                        int index = tc.TryGetProperty("index", out var idxElem)
                            ? idxElem.GetInt32()
                            : accumulatedToolCalls.Count;
                        if (!accumulatedToolCalls.ContainsKey(index))
                        {
                            string id = tc.TryGetProperty("id", out var idElem) &&
                                        idElem.ValueKind == JsonValueKind.String
                                ? idElem.GetString() ?? ""
                                : "";
                            string name = "";
                            if (tc.TryGetProperty("function", out var fnInit) &&
                                fnInit.TryGetProperty("name", out var nameElem) &&
                                nameElem.ValueKind == JsonValueKind.String)
                            {
                                name = nameElem.GetString() ?? "";
                            }
                            accumulatedToolCalls[index] = (id, name, new StringBuilder());
                        }

                        if (tc.TryGetProperty("function", out var fn) &&
                            fn.TryGetProperty("arguments", out var args) &&
                            args.ValueKind == JsonValueKind.String)
                        {
                            accumulatedToolCalls[index].Args.Append(args.GetString() ?? "");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Warn(null, "DashScope stream: failed to parse tool_calls chunk: {0}", ex.Message);
                }
            }

            // Stream end marker
            if (finishReason != null)
            {
                chunk.IsStreamFinal = true;

                // Extract usage statistics from the final chunk (defensive)
                if (root.TryGetProperty("usage", out var usage) &&
                    usage.ValueKind == JsonValueKind.Object)
                {
                    if (usage.TryGetProperty("prompt_tokens", out var pt) && pt.ValueKind != JsonValueKind.Null)
                        chunk.PromptTokens = pt.GetInt32();
                    if (usage.TryGetProperty("completion_tokens", out var ct) && ct.ValueKind != JsonValueKind.Null)
                        chunk.CompletionTokens = ct.GetInt32();
                    if (usage.TryGetProperty("total_tokens", out var tt) && tt.ValueKind != JsonValueKind.Null)
                        chunk.TotalTokens = tt.GetInt32();
                }

                // Convert accumulated tool calls
                if (accumulatedToolCalls.Count > 0)
                {
                    chunk.ToolCalls = accumulatedToolCalls.Values.Select(tc => new ToolCall
                    {
                        Id = tc.Id,
                        Name = tc.Name,
                        Arguments = JsonSerializer.Deserialize<Dictionary<string, object>>(
                            tc.Args.ToString(), _jsonOptions) ?? new()
                    }).ToList();
                }

                _logger.Info(null, "DashScope stream completed: model={0}, totalTokens={1}",
                    model, chunk.TotalTokens);
            }

            yield return chunk;
        }
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
    /// Sends a generation request (delegates to ChatAsync with single user message)
    /// </summary>
    public AIResponse Generate(string prompt)
    {
        return GenerateAsync(prompt).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Sends a generation request asynchronously (delegates to ChatAsync with single user message)
    /// </summary>
    public async Task<AIResponse> GenerateAsync(string prompt)
    {
        AIRequest request = new AIRequest(DefaultModel);
        request.AddMessage(MessageRole.User, prompt);
        return await ChatAsync(request);
    }

    /// <summary>
    /// Sends a generation request with system prompt
    /// </summary>
    public AIResponse Generate(string systemPrompt, string prompt)
    {
        return GenerateAsync(systemPrompt, prompt).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Sends a generation request with system prompt asynchronously
    /// </summary>
    public async Task<AIResponse> GenerateAsync(string systemPrompt, string prompt)
    {
        AIRequest request = new AIRequest(DefaultModel);
        request.AddMessage(MessageRole.System, systemPrompt);
        request.AddMessage(MessageRole.User, prompt);
        return await ChatAsync(request);
    }

    /// <summary>
    /// Builds the JSON request body for DashScope API (OpenAI-compatible format)
    /// </summary>
    private string BuildRequestBody(AIRequest request, string model, bool stream)
    {
        DashScopeRequest dashScopeRequest = new DashScopeRequest
        {
            Model = model,
            Messages = MapMessages(request.Messages),
            Stream = stream
        };

        // Add tool definitions if present
        if (request.Tools != null && request.Tools.Count > 0)
        {
            dashScopeRequest.Tools = request.Tools.Select(t => new DashScopeTool
            {
                Type = "function",
                Function = new DashScopeToolFunction
                {
                    Name = t.Name,
                    Description = t.Description,
                    Parameters = t.Parameters
                }
            }).ToList();
        }

        return JsonSerializer.Serialize(dashScopeRequest, _jsonOptions);
    }

    /// <summary>
    /// Maps core ChatMessage list to DashScope message list, preserving tool_calls and tool role
    /// </summary>
    private static List<DashScopeMessage> MapMessages(List<ChatMessage> messages)
    {
        List<DashScopeMessage> result = new();

        // Merge all system messages into one
        List<string> systemContents = new();
        foreach (ChatMessage msg in messages)
        {
            if (msg.Role == MessageRole.System)
            {
                systemContents.Add(msg.Content);
            }
        }

        if (systemContents.Count > 0)
        {
            result.Add(new DashScopeMessage
            {
                Role = "system",
                Content = string.Join("\n", systemContents)
            });
        }

        foreach (ChatMessage msg in messages)
        {
            if (msg.Role == MessageRole.System)
                continue;

            MessageRole role = msg.Role;

            DashScopeMessage dashScopeMsg = new DashScopeMessage
            {
                Role = MapRole(role),
                Content = msg.Content,
                ReasoningContent = msg.Thinking
            };

            // Reconstruct tool_calls for assistant messages
            if (role == MessageRole.Assistant && !string.IsNullOrEmpty(msg.ToolCallsJson))
            {
                try
                {
                    List<ToolCall>? toolCalls = JsonSerializer.Deserialize<List<ToolCall>>(msg.ToolCallsJson);
                    if (toolCalls != null && toolCalls.Count > 0)
                    {
                        dashScopeMsg.ToolCalls = toolCalls.Select(tc => new DashScopeToolCall
                        {
                            Id = tc.Id,
                            Type = "function",
                            Function = new DashScopeToolCallFunction
                            {
                                Name = tc.Name,
                                Arguments = JsonSerializer.Serialize(tc.Arguments)
                            }
                        }).ToList();
                    }
                }
                catch { /* ignore deserialization errors */ }
            }

            // Add tool_call_id for tool role messages
            if (role == MessageRole.Tool && !string.IsNullOrEmpty(msg.ToolCallId))
            {
                dashScopeMsg.ToolCallId = msg.ToolCallId;
            }

            result.Add(dashScopeMsg);
        }

        return result;
    }

    /// <summary>
    /// Maps MessageRole to DashScope/OpenAI role string
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

    /// <summary>
    /// Parses the non-streaming chat response JSON into an AIResponse
    /// </summary>
    private AIResponse ParseChatResponse(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var choice = root.GetProperty("choices")[0];
            var message = choice.GetProperty("message");

            var aiResponse = new AIResponse
            {
                Model = root.GetProperty("model").GetString() ?? "",
                Content = message.TryGetProperty("content", out var c) &&
                          c.ValueKind == JsonValueKind.String
                    ? c.GetString() ?? ""
                    : "",
                Thinking = message.TryGetProperty("reasoning_content", out var r) &&
                           r.ValueKind == JsonValueKind.String
                    ? r.GetString()
                    : null,
                Success = true
            };

            // Parse usage
            if (root.TryGetProperty("usage", out var usage) && usage.ValueKind == JsonValueKind.Object)
            {
                if (usage.TryGetProperty("prompt_tokens", out var pt) && pt.ValueKind != JsonValueKind.Null)
                    aiResponse.PromptTokens = pt.GetInt32();
                if (usage.TryGetProperty("completion_tokens", out var ct) && ct.ValueKind != JsonValueKind.Null)
                    aiResponse.CompletionTokens = ct.GetInt32();
                if (usage.TryGetProperty("total_tokens", out var tt) && tt.ValueKind != JsonValueKind.Null)
                    aiResponse.TotalTokens = tt.GetInt32();
            }

            // Parse tool_calls
            if (message.TryGetProperty("tool_calls", out var toolCalls) &&
                toolCalls.ValueKind == JsonValueKind.Array)
            {
                aiResponse.ToolCalls = ParseToolCalls(toolCalls);
            }

            return aiResponse;
        }
        catch (Exception ex)
        {
            _logger.Error(null, "DashScope response parse error: {0}", ex.Message);
            return AIResponse.Failed($"Failed to parse response: {ex.Message}");
        }
    }

    /// <summary>
    /// Parses tool_calls from the response JSON element
    /// </summary>
    private List<ToolCall> ParseToolCalls(JsonElement toolCallsElement)
    {
        var toolCalls = new List<ToolCall>();
        foreach (JsonElement tc in toolCallsElement.EnumerateArray())
        {
            JsonElement function = tc.GetProperty("function");
            string argumentsStr = function.GetProperty("arguments").GetString() ?? "{}";
            Dictionary<string, object> arguments;

            try
            {
                arguments = JsonSerializer.Deserialize<Dictionary<string, object>>(argumentsStr, _jsonOptions)
                            ?? new Dictionary<string, object>();
            }
            catch
            {
                arguments = new Dictionary<string, object>();
            }

            toolCalls.Add(new ToolCall
            {
                Id = tc.GetProperty("id").GetString() ?? "",
                Name = function.GetProperty("name").GetString() ?? "",
                Arguments = arguments
            });
        }

        return toolCalls;
    }

    /// <summary>
    /// Maps known DashScope/Qwen model names to their context window sizes in tokens.
    /// Reference: https://help.aliyun.com/zh/model-studio/getting-started/models
    /// </summary>
    private static int? GetContextWindowForModel(string modelName)
    {
        if (string.IsNullOrEmpty(modelName)) return null;

        string m = modelName.ToLowerInvariant();

        // Qwen Long series - 1M context
        if (m.Contains("qwen-long")) return 1000000;

        // Qwen Turbo series - 1M context (qwen-turbo-latest, qwen-turbo-2024-11-01+)
        if (m.Contains("qwen-turbo")) return 1000000;

        // Qwen Plus series - 128K context
        if (m.Contains("qwen-plus")) return 131072;

        // Qwen Max series - 32K context
        if (m.Contains("qwen-max")) return 32768;

        // Qwen3 series
        if (m.Contains("qwen3")) return 131072;       // 128K

        // Qwen2.5 series
        if (m.Contains("qwen2.5-coder")) return 131072; // 128K
        if (m.Contains("qwen2.5")) return 131072;      // 128K

        // Qwen2 series
        if (m.Contains("qwen2-vl")) return 32768;      // 32K
        if (m.Contains("qwen2")) return 131072;        // 128K

        // Qwen1.5 series
        if (m.Contains("qwen1.5")) return 32768;       // 32K

        // Qwen VL series
        if (m.Contains("qwen-vl") || m.Contains("qwen2.5-vl")) return 32768; // 32K
        if (m.Contains("qwen-omni")) return 32768;     // 32K

        // QWQ series
        if (m.Contains("qwq")) return 131072;           // 128K

        // DeepSeek models on DashScope
        if (m.Contains("deepseek-r1")) return 65536;  // 64K
        if (m.Contains("deepseek-v3")) return 65536;  // 64K

        // Default: DashScope models typically support at least 8K
        return null;
    }

    /// <summary>
    /// Determines whether the specified model supports vision (image understanding).
    /// </summary>
    private static bool? GetSupportsVisionForModel(string modelName)
    {
        if (string.IsNullOrEmpty(modelName)) return null;

        string m = modelName.ToLowerInvariant();

        // Qwen VL series support vision
        if (m.Contains("qwen-vl") || m.Contains("qwen2-vl") || m.Contains("qwen2.5-vl")) return true;
        // Qwen Omni series support vision
        if (m.Contains("qwen-omni")) return true;
        // Qwen3 VL series
        if (m.Contains("qwen3-vl") || m.Contains("qwen3.5-vl")) return true;

        // Other Qwen models do not support vision
        if (m.Contains("qwen")) return false;

        // Unknown models: assume not supported
        return null;
    }

    #region 速率控制

    /// <summary>
    /// First-layer rate control: ensures a minimum interval between the end of the previous request and the start of this request
    /// </summary>
    private async Task EnforceRateLimitAsync()
    {
        await _rateSemaphore.WaitAsync();
        try
        {
            TimeSpan elapsed = DateTime.UtcNow - _lastRequestEndTime;
            if (elapsed < _minRequestInterval)
            {
                TimeSpan waitTime = _minRequestInterval - elapsed;
                _logger.Debug(null, "DashScope rate control: waiting {0}ms before next request", waitTime.TotalMilliseconds);
                await Task.Delay(waitTime);
            }
        }
        finally
        {
            _rateSemaphore.Release();
        }
    }

    /// <summary>
    /// Update the last request end time
    /// </summary>
    private void UpdateLastRequestTime(DateTime endTime)
    {
        _rateSemaphore.Wait();
        try
        {
            _lastRequestEndTime = endTime;
        }
        finally
        {
            _rateSemaphore.Release();
        }
    }

    /// <summary>
    /// Second-layer rate control: calculates retry delay (exponential backoff strategy)
    /// Parses Retry-After header from 429 error response (if present)
    /// </summary>
    private TimeSpan CalculateRetryDelay(int retryCount, string? errorBody)
    {
        // Exponential backoff: 1s, 2s, 4s, 8s...
        int delayMs = BaseRetryDelayMs * (int)Math.Pow(2, retryCount - 1);
        
        // Add random jitter (0-500ms) to avoid multiple clients retrying simultaneously
        int jitter = Random.Shared.Next(0, 500);
        delayMs += jitter;

        // Try to parse Retry-After information from the error response
        if (!string.IsNullOrEmpty(errorBody))
        {
            try
            {
                using var doc = JsonDocument.Parse(errorBody);
                if (doc.RootElement.TryGetProperty("error", out var error) &&
                    error.TryGetProperty("message", out var message))
                {
                    string messageStr = message.GetString() ?? "";
                    
                    // Try to parse "Please try again after X seconds" format
                    if (messageStr.Contains("try again after") && 
                        int.TryParse(new string(messageStr.Where(char.IsDigit).ToArray()), out int seconds))
                    {
                        if (seconds > 0 && seconds <= 60)
                        {
                            _logger.Info(null, "DashScope: server suggests retry after {0}s", seconds);
                            return TimeSpan.FromSeconds(seconds);
                        }
                    }
                }
            }
            catch
            {
                // Ignore parsing errors, use default backoff strategy
            }
        }

        // Maximum delay does not exceed 30 seconds
        if (delayMs > 30000)
            delayMs = 30000;

        return TimeSpan.FromMilliseconds(delayMs);
    }

    #endregion
}
