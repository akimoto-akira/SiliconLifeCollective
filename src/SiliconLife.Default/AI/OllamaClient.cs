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

using System.Text.Json;
using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Ollama API request model (native Ollama format)
/// </summary>
internal class OllamaRequest
{
    public string Model { get; set; } = string.Empty;
    public List<OllamaMessage> Messages { get; set; } = new List<OllamaMessage>();
    public double? Temperature { get; set; }
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
}

/// <summary>
/// Ollama API response model
/// </summary>
internal class OllamaResponse
{
    public string Model { get; set; } = string.Empty;
    public OllamaMessage? Message { get; set; }
    public bool Done { get; set; }
    public OllamaUsage? Usage { get; set; }
    public string? Error { get; set; }
}

/// <summary>
/// Ollama usage statistics
/// </summary>
internal class OllamaUsage
{
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
}

/// <summary>
/// Ollama AI client implementation
/// </summary>
public class OllamaClient : IAIClient
{
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
    /// Sends a chat request to Ollama and returns the response asynchronously
    /// </summary>
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        try
        {
            string model = string.IsNullOrEmpty(request.Model) ? DefaultModel : request.Model;

            OllamaRequest ollamaRequest = new OllamaRequest
            {
                Model = model,
                Messages = request.Messages.Select(m => new OllamaMessage
                {
                    Role = MapRole(m.Role),
                    Content = m.Content
                }).ToList(),
                Temperature = request.Temperature,
                NumPredict = request.MaxTokens,
                Stream = false
            };

            string json = JsonSerializer.Serialize(ollamaRequest, _jsonOptions);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{Endpoint}/api/chat", content);
            response.EnsureSuccessStatusCode();

            string responseJson = await response.Content.ReadAsStringAsync();
            OllamaResponse? ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(responseJson, _jsonOptions);

            if (ollamaResponse == null)
            {
                return AIResponse.Failed("Failed to deserialize Ollama response");
            }

            if (!string.IsNullOrEmpty(ollamaResponse.Error))
            {
                return AIResponse.Failed(ollamaResponse.Error);
            }

            return new AIResponse
            {
                Model = ollamaResponse.Model,
                Content = ollamaResponse.Message?.Content ?? string.Empty,
                PromptTokens = ollamaResponse.Usage?.PromptTokens,
                CompletionTokens = ollamaResponse.Usage?.CompletionTokens,
                TotalTokens = ollamaResponse.Usage?.TotalTokens,
                Success = true
            };
        }
        catch (HttpRequestException ex)
        {
            return AIResponse.Failed($"Connection error: {ex.Message}");
        }
        catch (TaskCanceledException ex)
        {
            return AIResponse.Failed($"Request timeout: {ex.Message}");
        }
        catch (Exception ex)
        {
            return AIResponse.Failed($"Unexpected error: {ex.Message}");
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
    /// Maps MessageRole to Ollama role string
    /// </summary>
    private static string MapRole(MessageRole role)
    {
        return role switch
        {
            MessageRole.System => "system",
            MessageRole.User => "user",
            MessageRole.Assistant => "assistant",
            _ => "user"
        };
    }
}
