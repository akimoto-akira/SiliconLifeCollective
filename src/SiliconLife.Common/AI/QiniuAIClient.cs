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

using SiliconLife.Collective;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SiliconLife.Common.AI;

internal class QiniuAIRequest
{
    public string Model { get; set; } = string.Empty;
    public List<QiniuAIMessage> Messages { get; set; } = new();
    [JsonPropertyName("tools")]
    public List<QiniuAITool>? Tools { get; set; }
    public double? Temperature { get; set; }
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }
    [JsonPropertyName("top_p")]
    public double? TopP { get; set; }
    public bool Stream { get; set; } = false;
}

internal class QiniuAIMessage
{
    public string Role { get; set; } = string.Empty;
    public string? Content { get; set; }
    [JsonPropertyName("reasoning_content")]
    public string? ReasoningContent { get; set; }
    [JsonPropertyName("tool_calls")]
    public List<QiniuAIToolCall>? ToolCalls { get; set; }
    [JsonPropertyName("tool_call_id")]
    public string? ToolCallId { get; set; }
}

internal class QiniuAIToolCall
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    public string Type { get; set; } = "function";
    public QiniuAIToolCallFunction? Function { get; set; }
}

internal class QiniuAIToolCallFunction
{
    public string Name { get; set; } = string.Empty;
    public string? Arguments { get; set; }
}

internal class QiniuAITool
{
    public string Type { get; set; } = "function";
    public QiniuAIToolFunction? Function { get; set; }
}

internal class QiniuAIToolFunction
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Dictionary<string, object>? Parameters { get; set; }
}

/// <summary>
/// Qiniu AI (七牛云AI大模型推理服务) AI client implementation.
/// Uses OpenAI-compatible API format with API Key authentication.
/// Supports tool calling (function calling), streaming, and reasoning content.
/// </summary>
public class QiniuAIClient : IAIClient
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<QiniuAIClient>();
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly string _apiKey;

    /// <summary>
    /// Maximum allowed context window token capacity for Qiniu AI models (128K)
    /// </summary>
    public const int MaxContextWindowTokens = 131072; // 128K

    public string Endpoint { get; }

    public string DefaultModel { get; }

    /// <summary>
    /// User-configured context window token capacity.
    /// When set, overrides the model-name-based mapping.
    /// </summary>
    private readonly int? _contextWindowTokens;

    public bool? StreamingMode => null;

    public bool? SupportsToolCalls => true;

    /// <summary>
    /// Gets the context window token capacity for the current model.
    /// Returns the user-configured value if provided, otherwise maps
    /// by model name (deepseek-r1/v3=128K, qwen-max-2025-01-25=128K,
    /// qwen2.5-72b-instruct/qwen2-72b-instruct=128K, others=null).
    /// </summary>
    public int? ContextWindowTokens
    {
        get
        {
            if (_contextWindowTokens.HasValue)
                return _contextWindowTokens;
            return GetContextWindowTokensForModel(DefaultModel);
        }
    }

    /// <summary>
    /// Gets whether this client supports vision input.
    /// qwen2-vl-72b-instruct returns true; others return false.
    /// </summary>
    public bool? SupportsVision => GetSupportsVisionForModel(DefaultModel);

    /// <summary>
    /// Qiniu AI audio support is not yet mapped; returns null (unknown).
    /// </summary>
    public bool? SupportsAudio => null;

    /// <summary>
    /// Creates a new Qiniu AI client with the specified endpoint and API key
    /// </summary>
    /// <param name="endpoint">Qiniu AI API endpoint URL</param>
    /// <param name="apiKey">API key for authentication</param>
    /// <param name="defaultModel">Default model name</param>
    /// <param name="contextWindowTokens">Optional context window token capacity override.
    /// When provided, enables token-budget-based context trimming. Clamped to MaxContextWindowTokens (128K).</param>
    public QiniuAIClient(string endpoint, string apiKey, string defaultModel = "deepseek-v3", int? contextWindowTokens = null)
    {
        Endpoint = endpoint.TrimEnd('/');
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        DefaultModel = defaultModel;
        _contextWindowTokens = contextWindowTokens.HasValue
            ? Math.Min(contextWindowTokens.Value, MaxContextWindowTokens)
            : null;
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
    /// Maps model names to context window token capacities.
    /// deepseek-r1/v3=128K, qwen-max-2025-01-25=128K,
    /// qwen2.5-72b-instruct/qwen2-72b-instruct=128K.
    /// Returns null for unknown models (ContextManager will fall back to MaxContextMessages).
    /// </summary>
    internal static int? GetContextWindowTokensForModel(string? modelName)
    {
        if (string.IsNullOrEmpty(modelName))
            return null;

        string lower = modelName.ToLowerInvariant();
        if (lower.Contains("deepseek-r1") || lower.Contains("deepseek-v3"))
            return 131072; // 128K
        if (lower.Contains("qwen-max"))
            return 131072; // 128K
        if (lower.Contains("qwen2.5-72b") || lower.Contains("qwen2-72b"))
            return 131072; // 128K

        return null;
    }

    /// <summary>
    /// Maps model names to vision support.
    /// qwen2-vl-72b-instruct returns true; others return false.
    /// </summary>
    internal static bool? GetSupportsVisionForModel(string? modelName)
    {
        if (string.IsNullOrEmpty(modelName))
            return false;

        string lower = modelName.ToLowerInvariant();
        if (lower.Contains("vl") || lower.Contains("vision"))
            return true;

        return false;
    }

    public AIResponse Chat(AIRequest request)
    {
        return ChatAsync(request).GetAwaiter().GetResult();
    }

    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        try
        {
            string model = string.IsNullOrEmpty(request.Model) ? DefaultModel : request.Model;

            _logger.Info(null, "QiniuAI request: model={0}, messages={1}, hasTools={2}",
                model, request.Messages.Count, request.Tools != null && request.Tools.Count > 0);

            string requestBody = BuildRequestBody(request, model, stream: false);
            StringContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(Endpoint + "/chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                string errorBody = await response.Content.ReadAsStringAsync();
                _logger.Error(null, "QiniuAI HTTP error: {0} {1}", (int)response.StatusCode, errorBody);
                return AIResponse.Failed($"HTTP {(int)response.StatusCode}: {errorBody}");
            }

            string json = await response.Content.ReadAsStringAsync();
            AIResponse result = ParseChatResponse(json);

            _logger.Info(null, "QiniuAI response: model={0}, tokens={1}/{2}/{3}, hasToolCalls={4}",
                model, result.PromptTokens, result.CompletionTokens, result.TotalTokens, result.HasToolCalls);

            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.Error(null, "QiniuAI connection error: {0}", ex.Message);
            return AIResponse.Failed($"Connection error: {ex.Message}");
        }
        catch (TaskCanceledException ex)
        {
            _logger.Warn(null, "QiniuAI request timeout: {0}", ex.Message);
            return AIResponse.Failed($"Request timeout: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.Error(null, "QiniuAI request failed: {0}", ex.Message);
            return AIResponse.Failed($"Unexpected error: {ex.Message}");
        }
    }

    public async IAsyncEnumerable<AIResponse> ChatStreamAsync(
        AIRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string model = string.IsNullOrEmpty(request.Model) ? DefaultModel : request.Model;

        _logger.Info(null, "QiniuAI stream started: model={0}", model);

        string requestBody = BuildRequestBody(request, model, stream: true);
        StringContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint + "/chat/completions")
            { Content = content };

        HttpResponseMessage? response = null;
        AIResponse? errorResponse = null;
        try
        {
            response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                string errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.Error(null, "QiniuAI stream HTTP error: {0} {1}", (int)response.StatusCode, errorBody);
                errorResponse = AIResponse.Failed($"HTTP {(int)response.StatusCode}: {errorBody}");
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.Error(null, "QiniuAI stream connection error: {0}", ex.Message);
            errorResponse = AIResponse.Failed($"Connection error: {ex.Message}");
        }
        catch (OperationCanceledException)
        {
            _logger.Debug(null, "QiniuAI stream cancelled");
            yield break;
        }

        if (errorResponse != null)
        {
            yield return errorResponse;
            yield break;
        }

        using var stream = await response!.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        var accumulatedToolCalls = new Dictionary<int, (string Id, string Name, StringBuilder Args)>();

        while (!reader.EndOfStream)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string? line = await reader.ReadLineAsync(cancellationToken);

            if (string.IsNullOrEmpty(line))
                continue;

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

            if (delta.TryGetProperty("content", out var contentElem) &&
                contentElem.ValueKind == JsonValueKind.String)
            {
                chunk.Content = contentElem.GetString() ?? "";
            }

            if (delta.TryGetProperty("reasoning_content", out var thinkElem) &&
                thinkElem.ValueKind == JsonValueKind.String)
            {
                chunk.Thinking = thinkElem.GetString();
            }

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
                    _logger.Warn(null, "QiniuAI stream: failed to parse tool_calls chunk: {0}", ex.Message);
                }
            }

            if (finishReason != null)
            {
                chunk.IsStreamFinal = true;

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

                _logger.Info(null, "QiniuAI stream completed: model={0}, totalTokens={1}",
                    model, chunk.TotalTokens);
            }

            yield return chunk;
        }
    }

    public AIResponse Chat(string userMessage)
    {
        AIRequest request = new AIRequest(DefaultModel);
        request.AddMessage(MessageRole.User, userMessage);
        return Chat(request);
    }

    public async Task<AIResponse> ChatAsync(string userMessage)
    {
        AIRequest request = new AIRequest(DefaultModel);
        request.AddMessage(MessageRole.User, userMessage);
        return await ChatAsync(request);
    }

    public AIResponse Chat(string systemPrompt, string userMessage)
    {
        AIRequest request = new AIRequest(DefaultModel);
        request.AddMessage(MessageRole.System, systemPrompt);
        request.AddMessage(MessageRole.User, userMessage);
        return Chat(request);
    }

    public async Task<AIResponse> ChatAsync(string systemPrompt, string userMessage)
    {
        AIRequest request = new AIRequest(DefaultModel);
        request.AddMessage(MessageRole.System, systemPrompt);
        request.AddMessage(MessageRole.User, userMessage);
        return await ChatAsync(request);
    }

    public AIResponse Generate(string prompt)
    {
        return GenerateAsync(prompt).GetAwaiter().GetResult();
    }

    public async Task<AIResponse> GenerateAsync(string prompt)
    {
        AIRequest request = new AIRequest(DefaultModel);
        request.AddMessage(MessageRole.User, prompt);
        return await ChatAsync(request);
    }

    public AIResponse Generate(string systemPrompt, string prompt)
    {
        return GenerateAsync(systemPrompt, prompt).GetAwaiter().GetResult();
    }

    public async Task<AIResponse> GenerateAsync(string systemPrompt, string prompt)
    {
        AIRequest request = new AIRequest(DefaultModel);
        request.AddMessage(MessageRole.System, systemPrompt);
        request.AddMessage(MessageRole.User, prompt);
        return await ChatAsync(request);
    }

    private string BuildRequestBody(AIRequest request, string model, bool stream)
    {
        QiniuAIRequest qiniuRequest = new QiniuAIRequest
        {
            Model = model,
            Messages = MapMessages(request.Messages),
            Stream = stream
        };

        if (request.Tools != null && request.Tools.Count > 0)
        {
            qiniuRequest.Tools = request.Tools.Select(t => new QiniuAITool
            {
                Type = "function",
                Function = new QiniuAIToolFunction
                {
                    Name = t.Name,
                    Description = t.Description,
                    Parameters = t.Parameters
                }
            }).ToList();
        }

        return JsonSerializer.Serialize(qiniuRequest, _jsonOptions);
    }

    private static List<QiniuAIMessage> MapMessages(List<ChatMessage> messages)
    {
        List<QiniuAIMessage> result = new();

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
            result.Add(new QiniuAIMessage
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

            QiniuAIMessage qiniuMsg = new QiniuAIMessage
            {
                Role = MapRole(role),
                Content = msg.Content,
                ReasoningContent = msg.Thinking
            };

            if (role == MessageRole.Assistant && !string.IsNullOrEmpty(msg.ToolCallsJson))
            {
                try
                {
                    List<ToolCall>? toolCalls = JsonSerializer.Deserialize<List<ToolCall>>(msg.ToolCallsJson);
                    if (toolCalls != null && toolCalls.Count > 0)
                    {
                        qiniuMsg.ToolCalls = toolCalls.Select(tc => new QiniuAIToolCall
                        {
                            Id = tc.Id,
                            Type = "function",
                            Function = new QiniuAIToolCallFunction
                            {
                                Name = tc.Name,
                                Arguments = JsonSerializer.Serialize(tc.Arguments)
                            }
                        }).ToList();
                    }
                }
                catch { }
            }

            if (role == MessageRole.Tool && !string.IsNullOrEmpty(msg.ToolCallId))
            {
                qiniuMsg.ToolCallId = msg.ToolCallId;
            }

            result.Add(qiniuMsg);
        }

        return result;
    }

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

            if (root.TryGetProperty("usage", out var usage) && usage.ValueKind == JsonValueKind.Object)
            {
                if (usage.TryGetProperty("prompt_tokens", out var pt) && pt.ValueKind != JsonValueKind.Null)
                    aiResponse.PromptTokens = pt.GetInt32();
                if (usage.TryGetProperty("completion_tokens", out var ct) && ct.ValueKind != JsonValueKind.Null)
                    aiResponse.CompletionTokens = ct.GetInt32();
                if (usage.TryGetProperty("total_tokens", out var tt) && tt.ValueKind != JsonValueKind.Null)
                    aiResponse.TotalTokens = tt.GetInt32();
            }

            if (message.TryGetProperty("tool_calls", out var toolCalls) &&
                toolCalls.ValueKind == JsonValueKind.Array)
            {
                aiResponse.ToolCalls = ParseToolCalls(toolCalls);
            }

            return aiResponse;
        }
        catch (Exception ex)
        {
            _logger.Error(null, "QiniuAI response parse error: {0}", ex.Message);
            return AIResponse.Failed($"Failed to parse response: {ex.Message}");
        }
    }

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
}
