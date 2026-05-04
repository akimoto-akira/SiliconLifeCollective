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
/// Volcengine Ark API request model (OpenAI-compatible format)
/// </summary>
internal class VolcengineArkRequest
{
    public string Model { get; set; } = string.Empty;
    public List<VolcengineArkMessage> Messages { get; set; } = new();
    [JsonPropertyName("tools")]
    public List<VolcengineArkTool>? Tools { get; set; }
    public bool Stream { get; set; } = false;
}

/// <summary>
/// Volcengine Ark API message model (OpenAI-compatible format)
/// </summary>
internal class VolcengineArkMessage
{
    public string Role { get; set; } = string.Empty;
    public string? Content { get; set; }
    [JsonPropertyName("reasoning_content")]
    public string? ReasoningContent { get; set; }
    [JsonPropertyName("tool_calls")]
    public List<VolcengineArkToolCall>? ToolCalls { get; set; }
    [JsonPropertyName("tool_call_id")]
    public string? ToolCallId { get; set; }
}

/// <summary>
/// Volcengine Ark tool call in assistant response
/// </summary>
internal class VolcengineArkToolCall
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    public string Type { get; set; } = "function";
    public VolcengineArkToolCallFunction? Function { get; set; }
}

/// <summary>
/// Volcengine Ark tool call function definition
/// </summary>
internal class VolcengineArkToolCallFunction
{
    public string Name { get; set; } = string.Empty;
    public string? Arguments { get; set; }
}

/// <summary>
/// Volcengine Ark tool definition for request
/// </summary>
internal class VolcengineArkTool
{
    public string Type { get; set; } = "function";
    public VolcengineArkToolFunction? Function { get; set; }
}

/// <summary>
/// Volcengine Ark tool function definition
/// </summary>
internal class VolcengineArkToolFunction
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Dictionary<string, object>? Parameters { get; set; }
}

/// <summary>
/// Volcengine Ark AI client implementation.
/// Uses OpenAI-compatible API format with Bearer token authentication.
/// Supports tool calling (function calling), streaming, and reasoning content.
/// </summary>
public class VolcengineArkClient : IAIClient
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<VolcengineArkClient>();
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    // Rate control fields
    private readonly SemaphoreSlim _rateSemaphore = new SemaphoreSlim(1, 1);
    private DateTime _lastRequestEndTime = DateTime.MinValue;
    private readonly TimeSpan _minRequestInterval;
    private const int MaxRetryCount = 3;
    private const int BaseRetryDelayMs = 1000;

    /// <summary>
    /// Gets the endpoint URL of the Volcengine Ark service
    /// </summary>
    public string Endpoint { get; }

    /// <summary>
    /// Gets the default model (inference endpoint ID)
    /// </summary>
    public string DefaultModel { get; }

    /// <summary>
    /// Volcengine Ark supports both streaming and non-streaming modes.
    /// Returns null to indicate both are supported, with streaming preferred.
    /// </summary>
    public bool? StreamingMode => null;

    /// <summary>
    /// Creates a new Volcengine Ark client with the specified configuration
    /// </summary>
    /// <param name="apiKey">Volcengine Ark API key for authentication</param>
    /// <param name="endpoint">The Volcengine Ark API endpoint URL</param>
    /// <param name="defaultModel">The default inference endpoint ID to use</param>
    /// <param name="minRequestIntervalMs">Minimum interval between requests in milliseconds (default: 200ms)</param>
    public VolcengineArkClient(string apiKey, string endpoint, string defaultModel, int minRequestIntervalMs = 200)
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
    /// Sends a chat request to Volcengine Ark and returns the response
    /// </summary>
    public AIResponse Chat(AIRequest request)
    {
        return ChatAsync(request).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Sends a chat request to Volcengine Ark and returns the response asynchronously.
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
                await EnforceRateLimitAsync();

                string model = string.IsNullOrEmpty(request.Model) ? DefaultModel : request.Model;

                _logger.Info(null, "VolcengineArk request: model={0}, messages={1}, hasTools={2}, retry={3}",
                    model, request.Messages.Count, request.Tools != null && request.Tools.Count > 0, retryCount);

                string requestBody = BuildRequestBody(request, model, stream: false);
                StringContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                DateTime requestStartTime = DateTime.UtcNow;
                HttpResponseMessage response = await _httpClient.PostAsync(Endpoint, content);
                DateTime requestEndTime = DateTime.UtcNow;

                if (!response.IsSuccessStatusCode)
                {
                    string errorBody = await response.Content.ReadAsStringAsync();
                    _logger.Error(null, "VolcengineArk HTTP error: {0} {1}", (int)response.StatusCode, errorBody);

                    if ((int)response.StatusCode == 429)
                    {
                        retryCount++;
                        if (retryCount <= MaxRetryCount)
                        {
                            TimeSpan waitTime = CalculateRetryDelay(retryCount, errorBody);
                            _logger.Warn(null, "VolcengineArk rate limited (429), retry {0}/{1} after {2}ms",
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

                UpdateLastRequestTime(requestEndTime);

                _logger.Info(null, "VolcengineArk response: model={0}, tokens={1}/{2}/{3}, hasToolCalls={4}",
                    model, result.PromptTokens, result.CompletionTokens, result.TotalTokens, result.HasToolCalls);

                return result;
            }
            catch (HttpRequestException ex)
            {
                lastException = ex;
                _logger.Error(null, "VolcengineArk connection error: {0}", ex.Message);

                retryCount++;
                if (retryCount <= MaxRetryCount)
                {
                    TimeSpan waitTime = CalculateRetryDelay(retryCount, null);
                    _logger.Warn(null, "VolcengineArk connection error, retry {0}/{1} after {2}ms",
                        retryCount, MaxRetryCount, waitTime.TotalMilliseconds);
                    await Task.Delay(waitTime);
                    continue;
                }
                return AIResponse.Failed($"Connection error after {MaxRetryCount} retries: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                _logger.Warn(null, "VolcengineArk request timeout: {0}", ex.Message);
                return AIResponse.Failed($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                lastException = ex;
                _logger.Error(null, "VolcengineArk request failed: {0}", ex.Message);

                retryCount++;
                if (retryCount <= MaxRetryCount)
                {
                    TimeSpan waitTime = CalculateRetryDelay(retryCount, null);
                    _logger.Warn(null, "VolcengineArk unexpected error, retry {0}/{1} after {2}ms",
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
    /// Sends a streaming chat request to Volcengine Ark, yielding incremental token responses.
    /// Each yielded AIResponse contains only the new token content.
    /// The final yield has IsStreamFinal = true and contains usage statistics.
    /// </summary>
    public async IAsyncEnumerable<AIResponse> ChatStreamAsync(
        AIRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string model = string.IsNullOrEmpty(request.Model) ? DefaultModel : request.Model;

        _logger.Info(null, "VolcengineArk stream started: model={0}", model);

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
                _logger.Error(null, "VolcengineArk stream HTTP error: {0} {1}", (int)response.StatusCode, errorBody);
                errorResponse = AIResponse.Failed($"HTTP {(int)response.StatusCode}: {errorBody}");
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.Error(null, "VolcengineArk stream connection error: {0}", ex.Message);
            errorResponse = AIResponse.Failed($"Connection error: {ex.Message}");
        }
        catch (OperationCanceledException)
        {
            _logger.Debug(null, "VolcengineArk stream cancelled");
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

            // Accumulate tool_calls
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
                    _logger.Warn(null, "VolcengineArk stream: failed to parse tool_calls chunk: {0}", ex.Message);
                }
            }

            // Stream end marker
            if (finishReason != null)
            {
                chunk.IsStreamFinal = true;

                // Extract usage statistics from the final chunk
                if (root.TryGetProperty("usage", out var usage) &&
                    usage.ValueKind == JsonValueKind.Object)
                {
                    if (usage.TryGetProperty("prompt_tokens", out var pt))
                        chunk.PromptTokens = pt.GetInt32();
                    if (usage.TryGetProperty("completion_tokens", out var ct))
                        chunk.CompletionTokens = ct.GetInt32();
                    if (usage.TryGetProperty("total_tokens", out var tt))
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

                _logger.Info(null, "VolcengineArk stream completed: model={0}, totalTokens={1}",
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
    /// Sends a generation request asynchronously
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
    /// Builds the JSON request body for Volcengine Ark API (OpenAI-compatible format)
    /// </summary>
    private string BuildRequestBody(AIRequest request, string model, bool stream)
    {
        VolcengineArkRequest arkRequest = new VolcengineArkRequest
        {
            Model = model,
            Messages = MapMessages(request.Messages),
            Stream = stream
        };

        // Add tool definitions if present
        if (request.Tools != null && request.Tools.Count > 0)
        {
            arkRequest.Tools = request.Tools.Select(t => new VolcengineArkTool
            {
                Type = "function",
                Function = new VolcengineArkToolFunction
                {
                    Name = t.Name,
                    Description = t.Description,
                    Parameters = t.Parameters
                }
            }).ToList();
        }

        return JsonSerializer.Serialize(arkRequest, _jsonOptions);
    }

    /// <summary>
    /// Maps core ChatMessage list to Volcengine Ark message list, preserving tool_calls and tool role
    /// </summary>
    private static List<VolcengineArkMessage> MapMessages(List<ChatMessage> messages)
    {
        List<VolcengineArkMessage> result = new();

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
            result.Add(new VolcengineArkMessage
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

            VolcengineArkMessage arkMsg = new VolcengineArkMessage
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
                        arkMsg.ToolCalls = toolCalls.Select(tc => new VolcengineArkToolCall
                        {
                            Id = tc.Id,
                            Type = "function",
                            Function = new VolcengineArkToolCallFunction
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
                arkMsg.ToolCallId = msg.ToolCallId;
            }

            result.Add(arkMsg);
        }

        return result;
    }

    /// <summary>
    /// Maps MessageRole to Volcengine Ark/OpenAI role string
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
            if (root.TryGetProperty("usage", out var usage))
            {
                aiResponse.PromptTokens = usage.GetProperty("prompt_tokens").GetInt32();
                aiResponse.CompletionTokens = usage.GetProperty("completion_tokens").GetInt32();
                aiResponse.TotalTokens = usage.GetProperty("total_tokens").GetInt32();
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
            _logger.Error(null, "VolcengineArk response parse error: {0}", ex.Message);
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

    #region Rate Control

    /// <summary>
    /// First layer rate control: ensures minimum interval between consecutive requests
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
                _logger.Debug(null, "VolcengineArk rate control: waiting {0}ms before next request", waitTime.TotalMilliseconds);
                await Task.Delay(waitTime);
            }
        }
        finally
        {
            _rateSemaphore.Release();
        }
    }

    /// <summary>
    /// Updates the last request end time
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
    /// Second layer rate control: calculates retry delay with exponential backoff
    /// </summary>
    private TimeSpan CalculateRetryDelay(int retryCount, string? errorBody)
    {
        int delayMs = BaseRetryDelayMs * (int)Math.Pow(2, retryCount - 1);

        int jitter = Random.Shared.Next(0, 500);
        delayMs += jitter;

        if (!string.IsNullOrEmpty(errorBody))
        {
            try
            {
                using var doc = JsonDocument.Parse(errorBody);
                if (doc.RootElement.TryGetProperty("error", out var error) &&
                    error.TryGetProperty("message", out var message))
                {
                    string messageStr = message.GetString() ?? "";

                    if (messageStr.Contains("try again after") &&
                        int.TryParse(new string(messageStr.Where(char.IsDigit).ToArray()), out int seconds))
                    {
                        if (seconds > 0 && seconds <= 60)
                        {
                            _logger.Info(null, "VolcengineArk: server suggests retry after {0}s", seconds);
                            return TimeSpan.FromSeconds(seconds);
                        }
                    }
                }
            }
            catch
            {
                // Ignore parse errors, use default backoff strategy
            }
        }

        if (delayMs > 30000)
            delayMs = 30000;

        return TimeSpan.FromMilliseconds(delayMs);
    }

    #endregion
}
