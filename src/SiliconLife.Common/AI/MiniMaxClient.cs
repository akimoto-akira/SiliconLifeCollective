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

internal class MiniMaxRequest
{
    public string Model { get; set; } = string.Empty;
    public List<MiniMaxMessage> Messages { get; set; } = new();
    [JsonPropertyName("tools")]
    public List<MiniMaxTool>? Tools { get; set; }
    public bool Stream { get; set; } = false;
    [JsonPropertyName("reasoning_split")]
    public bool? ReasoningSplit { get; set; } = true;
    [JsonPropertyName("thinking")]
    public MiniMaxThinkingConfig? Thinking { get; set; }
    [JsonPropertyName("max_completion_tokens")]
    public int? MaxCompletionTokens { get; set; }
}

internal class MiniMaxThinkingConfig
{
    public string Type { get; set; } = "adaptive";
}

internal class MiniMaxMessage
{
    public string Role { get; set; } = string.Empty;
    public string? Content { get; set; }
    [JsonPropertyName("reasoning_content")]
    public string? ReasoningContent { get; set; }
    [JsonPropertyName("tool_calls")]
    public List<MiniMaxToolCall>? ToolCalls { get; set; }
    [JsonPropertyName("tool_call_id")]
    public string? ToolCallId { get; set; }
}

internal class MiniMaxToolCall
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    public string Type { get; set; } = "function";
    public MiniMaxToolCallFunction? Function { get; set; }
}

internal class MiniMaxToolCallFunction
{
    public string Name { get; set; } = string.Empty;
    public string? Arguments { get; set; }
}

internal class MiniMaxTool
{
    public string Type { get; set; } = "function";
    public MiniMaxToolFunction? Function { get; set; }
}

internal class MiniMaxToolFunction
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Dictionary<string, object>? Parameters { get; set; }
}

/// <summary>
/// MiniMax (稀宇科技) AI client implementation.
/// Uses OpenAI-compatible API format with Bearer token authentication.
/// Supports tool calling, streaming, thinking mode with reasoning_split.
/// M3 supports native multimodal (image + video).
/// </summary>
public class MiniMaxClient : IAIClient
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<MiniMaxClient>();
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly string _apiKey;

    public const int MaxContextWindowTokens = 1048576;

    public string Endpoint { get; }
    public string DefaultModel { get; }
    private readonly int? _contextWindowTokens;

    public bool? StreamingMode => null;
    public bool? SupportsToolCalls => true;

    public int? ContextWindowTokens
    {
        get
        {
            if (_contextWindowTokens.HasValue)
                return _contextWindowTokens;
            return GetContextWindowTokensForModel(DefaultModel);
        }
    }

    public bool? SupportsVision => GetSupportsVisionForModel(DefaultModel);
    public bool? SupportsAudio => null;

    public MiniMaxClient(string apiKey, string endpoint,
        string defaultModel = "MiniMax-M3",
        int? contextWindowTokens = null)
    {
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        Endpoint = endpoint.TrimEnd('/');
        DefaultModel = defaultModel;
        _contextWindowTokens = contextWindowTokens.HasValue
            ? Math.Min(contextWindowTokens.Value, MaxContextWindowTokens)
            : null;
        _httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    internal static int? GetContextWindowTokensForModel(string? modelName)
    {
        if (string.IsNullOrEmpty(modelName)) return null;
        string m = modelName.ToLowerInvariant();
        if (m.Contains("m3")) return 1048576;
        if (m.Contains("m2.7")) return 196608;
        if (m.Contains("m2.5") || m.Contains("m2.1")) return 204800;
        if (m.Contains("m2")) return 196608;
        if (m.Contains("m1") || m.Contains("text-01")) return 1048576;
        return null;
    }

    internal static bool? GetSupportsVisionForModel(string? modelName)
    {
        if (string.IsNullOrEmpty(modelName)) return null;
        string m = modelName.ToLowerInvariant();
        if (m.Contains("m3")) return true;
        if (m.StartsWith("minimax-m2")) return false;
        return null;
    }

    public AIResponse Chat(AIRequest request) => ChatAsync(request).GetAwaiter().GetResult();

    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        try
        {
            string model = string.IsNullOrEmpty(request.Model) ? DefaultModel : request.Model;
            _logger.Info(null, "MiniMax request: model={0}, messages={1}, hasTools={2}",
                model, request.Messages.Count, request.Tools != null && request.Tools.Count > 0);

            string requestBody = BuildRequestBody(request, model, stream: false);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint + "/chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                string errorBody = await response.Content.ReadAsStringAsync();
                _logger.Error(null, "MiniMax HTTP error: {0} {1}", (int)response.StatusCode, errorBody);
                return AIResponse.Failed($"HTTP {(int)response.StatusCode}: {errorBody}");
            }

            string json = await response.Content.ReadAsStringAsync();
            AIResponse result = ParseChatResponse(json);
            _logger.Info(null, "MiniMax response: model={0}, tokens={1}/{2}/{3}, hasToolCalls={4}",
                model, result.PromptTokens, result.CompletionTokens, result.TotalTokens, result.HasToolCalls);
            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.Error(null, "MiniMax connection error: {0}", ex.Message);
            return AIResponse.Failed($"Connection error: {ex.Message}");
        }
        catch (TaskCanceledException ex)
        {
            _logger.Warn(null, "MiniMax request timeout: {0}", ex.Message);
            return AIResponse.Failed($"Request timeout: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.Error(null, "MiniMax request failed: {0}", ex.Message);
            return AIResponse.Failed($"Unexpected error: {ex.Message}");
        }
    }

    public async IAsyncEnumerable<AIResponse> ChatStreamAsync(
        AIRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string model = string.IsNullOrEmpty(request.Model) ? DefaultModel : request.Model;
        _logger.Info(null, "MiniMax stream started: model={0}", model);

        string requestBody = BuildRequestBody(request, model, stream: true);
        var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint + "/chat/completions") { Content = content };

        HttpResponseMessage? response = null;
        AIResponse? errorResponse = null;
        try
        {
            response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                string errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.Error(null, "MiniMax stream HTTP error: {0} {1}", (int)response.StatusCode, errorBody);
                errorResponse = AIResponse.Failed($"HTTP {(int)response.StatusCode}: {errorBody}");
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.Error(null, "MiniMax stream connection error: {0}", ex.Message);
            errorResponse = AIResponse.Failed($"Connection error: {ex.Message}");
        }
        catch (OperationCanceledException) { _logger.Debug(null, "MiniMax stream cancelled"); yield break; }

        if (errorResponse != null) { yield return errorResponse; yield break; }

        using var stream = await response!.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);
        var accumulatedToolCalls = new Dictionary<int, (string Id, string Name, StringBuilder Args)>();

        while (!reader.EndOfStream)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string? line = await reader.ReadLineAsync(cancellationToken);
            if (string.IsNullOrEmpty(line)) continue;

            string data;
            if (line.StartsWith("data: ")) data = line.Substring(6);
            else if (line.StartsWith("data:")) data = line.Substring(5);
            else continue;
            if (data == "[DONE]") break;

            JsonElement root;
            try { using var doc = JsonDocument.Parse(data); root = doc.RootElement.Clone(); }
            catch { continue; }

            if (!root.TryGetProperty("choices", out var choices) || choices.GetArrayLength() == 0) continue;
            var choice = choices[0];
            if (!choice.TryGetProperty("delta", out var delta)) continue;

            string? finishReason = choice.TryGetProperty("finish_reason", out var fr) && fr.ValueKind != JsonValueKind.Null
                ? fr.GetString() : null;

            var chunk = new AIResponse { Success = true };

            if (delta.TryGetProperty("content", out var contentElem) && contentElem.ValueKind == JsonValueKind.String)
                chunk.Content = contentElem.GetString() ?? "";

            if (delta.TryGetProperty("reasoning_content", out var thinkElem) && thinkElem.ValueKind == JsonValueKind.String)
                chunk.Thinking = thinkElem.GetString();

            if (delta.TryGetProperty("tool_calls", out var tcDelta))
            {
                try
                {
                    foreach (JsonElement tc in tcDelta.EnumerateArray())
                    {
                        int index = tc.TryGetProperty("index", out var idxElem) ? idxElem.GetInt32() : accumulatedToolCalls.Count;
                        if (!accumulatedToolCalls.ContainsKey(index))
                        {
                            string id = tc.TryGetProperty("id", out var idElem) && idElem.ValueKind == JsonValueKind.String ? idElem.GetString() ?? "" : "";
                            string name = "";
                            if (tc.TryGetProperty("function", out var fnInit) && fnInit.TryGetProperty("name", out var nameElem) && nameElem.ValueKind == JsonValueKind.String)
                                name = nameElem.GetString() ?? "";
                            accumulatedToolCalls[index] = (id, name, new StringBuilder());
                        }
                        if (tc.TryGetProperty("function", out var fn) && fn.TryGetProperty("arguments", out var args) && args.ValueKind == JsonValueKind.String)
                            accumulatedToolCalls[index].Args.Append(args.GetString() ?? "");
                    }
                }
                catch (Exception ex) { _logger.Warn(null, "MiniMax stream: failed to parse tool_calls chunk: {0}", ex.Message); }
            }

            if (finishReason != null)
            {
                chunk.IsStreamFinal = true;
                if (root.TryGetProperty("usage", out var usage) && usage.ValueKind == JsonValueKind.Object)
                {
                    if (usage.TryGetProperty("prompt_tokens", out var pt) && pt.ValueKind != JsonValueKind.Null) chunk.PromptTokens = pt.GetInt32();
                    if (usage.TryGetProperty("completion_tokens", out var ct) && ct.ValueKind != JsonValueKind.Null) chunk.CompletionTokens = ct.GetInt32();
                    if (usage.TryGetProperty("total_tokens", out var tt) && tt.ValueKind != JsonValueKind.Null) chunk.TotalTokens = tt.GetInt32();
                }
                if (accumulatedToolCalls.Count > 0)
                {
                    chunk.ToolCalls = accumulatedToolCalls.Values.Select(tc => new ToolCall
                    {
                        Id = tc.Id, Name = tc.Name,
                        Arguments = JsonSerializer.Deserialize<Dictionary<string, object>>(tc.Args.ToString(), _jsonOptions) ?? new()
                    }).ToList();
                }
                _logger.Info(null, "MiniMax stream completed: model={0}, totalTokens={1}", model, chunk.TotalTokens);
            }
            yield return chunk;
        }
    }

    public AIResponse Chat(string userMessage)
    {
        var request = new AIRequest(DefaultModel); request.AddMessage(MessageRole.User, userMessage); return Chat(request);
    }
    public async Task<AIResponse> ChatAsync(string userMessage)
    {
        var request = new AIRequest(DefaultModel); request.AddMessage(MessageRole.User, userMessage); return await ChatAsync(request);
    }
    public AIResponse Chat(string systemPrompt, string userMessage)
    {
        var request = new AIRequest(DefaultModel); request.AddMessage(MessageRole.System, systemPrompt); request.AddMessage(MessageRole.User, userMessage); return Chat(request);
    }
    public async Task<AIResponse> ChatAsync(string systemPrompt, string userMessage)
    {
        var request = new AIRequest(DefaultModel); request.AddMessage(MessageRole.System, systemPrompt); request.AddMessage(MessageRole.User, userMessage); return await ChatAsync(request);
    }
    public AIResponse Generate(string prompt) => GenerateAsync(prompt).GetAwaiter().GetResult();
    public async Task<AIResponse> GenerateAsync(string prompt)
    {
        var request = new AIRequest(DefaultModel); request.AddMessage(MessageRole.User, prompt); return await ChatAsync(request);
    }
    public AIResponse Generate(string systemPrompt, string prompt) => GenerateAsync(systemPrompt, prompt).GetAwaiter().GetResult();
    public async Task<AIResponse> GenerateAsync(string systemPrompt, string prompt)
    {
        var request = new AIRequest(DefaultModel); request.AddMessage(MessageRole.System, systemPrompt); request.AddMessage(MessageRole.User, prompt); return await ChatAsync(request);
    }

    private string BuildRequestBody(AIRequest request, string model, bool stream)
    {
        var req = new MiniMaxRequest
        {
            Model = model,
            Messages = MapMessages(request.Messages),
            Stream = stream,
            ReasoningSplit = true  // Always separate thinking to reasoning_content
        };

        // M3 can disable thinking; M2.x cannot
        string lower = model.ToLowerInvariant();
        if (lower.Contains("m3"))
            req.Thinking = new MiniMaxThinkingConfig { Type = "adaptive" };

        if (request.Tools != null && request.Tools.Count > 0)
        {
            req.Tools = request.Tools.Select(t => new MiniMaxTool
            {
                Type = "function",
                Function = new MiniMaxToolFunction { Name = t.Name, Description = t.Description, Parameters = t.Parameters }
            }).ToList();
        }
        return JsonSerializer.Serialize(req, _jsonOptions);
    }

    private static List<MiniMaxMessage> MapMessages(List<ChatMessage> messages)
    {
        var result = new List<MiniMaxMessage>();
        var systemContents = new List<string>();
        foreach (var msg in messages)
            if (msg.Role == MessageRole.System) systemContents.Add(msg.Content);
        if (systemContents.Count > 0)
            result.Add(new MiniMaxMessage { Role = "system", Content = string.Join("\n", systemContents) });

        foreach (var msg in messages)
        {
            if (msg.Role == MessageRole.System) continue;
            var mmMsg = new MiniMaxMessage
            {
                Role = MapRole(msg.Role),
                Content = msg.Content,
                ReasoningContent = msg.Thinking
            };
            if (msg.Role == MessageRole.Assistant && !string.IsNullOrEmpty(msg.ToolCallsJson))
            {
                try
                {
                    var toolCalls = JsonSerializer.Deserialize<List<ToolCall>>(msg.ToolCallsJson);
                    if (toolCalls != null && toolCalls.Count > 0)
                    {
                        mmMsg.ToolCalls = toolCalls.Select(tc => new MiniMaxToolCall
                        {
                            Id = tc.Id, Type = "function",
                            Function = new MiniMaxToolCallFunction { Name = tc.Name, Arguments = JsonSerializer.Serialize(tc.Arguments) }
                        }).ToList();
                    }
                }
                catch { }
            }
            if (msg.Role == MessageRole.Tool && !string.IsNullOrEmpty(msg.ToolCallId))
                mmMsg.ToolCallId = msg.ToolCallId;
            result.Add(mmMsg);
        }
        return result;
    }

    private static string MapRole(MessageRole role) => role switch
    {
        MessageRole.System => "system",
        MessageRole.User => "user",
        MessageRole.Assistant => "assistant",
        MessageRole.Tool => "tool",
        _ => "user"
    };

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
                Content = message.TryGetProperty("content", out var c) && c.ValueKind == JsonValueKind.String ? c.GetString() ?? "" : "",
                Thinking = message.TryGetProperty("reasoning_content", out var r) && r.ValueKind == JsonValueKind.String ? r.GetString() : null,
                Success = true
            };

            if (root.TryGetProperty("usage", out var usage) && usage.ValueKind == JsonValueKind.Object)
            {
                if (usage.TryGetProperty("prompt_tokens", out var pt) && pt.ValueKind != JsonValueKind.Null) aiResponse.PromptTokens = pt.GetInt32();
                if (usage.TryGetProperty("completion_tokens", out var ct) && ct.ValueKind != JsonValueKind.Null) aiResponse.CompletionTokens = ct.GetInt32();
                if (usage.TryGetProperty("total_tokens", out var tt) && tt.ValueKind != JsonValueKind.Null) aiResponse.TotalTokens = tt.GetInt32();
            }

            if (message.TryGetProperty("tool_calls", out var toolCalls) && toolCalls.ValueKind == JsonValueKind.Array)
                aiResponse.ToolCalls = ParseToolCalls(toolCalls);

            // Check base_resp for error status (MiniMax extension)
            if (root.TryGetProperty("base_resp", out var baseResp) &&
                baseResp.TryGetProperty("status_code", out var statusCode) &&
                statusCode.GetInt32() != 0)
            {
                string statusMsg = baseResp.TryGetProperty("status_msg", out var msg) ? msg.GetString() ?? "" : "";
                _logger.Warn(null, "MiniMax base_resp error: code={0}, msg={1}", statusCode.GetInt32(), statusMsg);
            }

            return aiResponse;
        }
        catch (Exception ex)
        {
            _logger.Error(null, "MiniMax response parse error: {0}", ex.Message);
            return AIResponse.Failed($"Failed to parse response: {ex.Message}");
        }
    }

    private List<ToolCall> ParseToolCalls(JsonElement toolCallsElement)
    {
        var toolCalls = new List<ToolCall>();
        foreach (JsonElement tc in toolCallsElement.EnumerateArray())
        {
            var function = tc.GetProperty("function");
            string argumentsStr = function.GetProperty("arguments").GetString() ?? "{}";
            Dictionary<string, object> arguments;
            try { arguments = JsonSerializer.Deserialize<Dictionary<string, object>>(argumentsStr, _jsonOptions) ?? new(); }
            catch { arguments = new Dictionary<string, object>(); }
            toolCalls.Add(new ToolCall { Id = tc.GetProperty("id").GetString() ?? "", Name = function.GetProperty("name").GetString() ?? "", Arguments = arguments });
        }
        return toolCalls;
    }
}
