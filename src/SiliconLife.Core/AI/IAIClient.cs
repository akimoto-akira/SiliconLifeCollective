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

namespace SiliconLife.Collective;

/// <summary>
/// Interface for AI client implementations
/// </summary>
public interface IAIClient
{
    /// <summary>
    /// Gets the endpoint URL of the AI service
    /// </summary>
    string Endpoint { get; }

    /// <summary>
    /// Gets the default model name
    /// </summary>
    string DefaultModel { get; }

    /// <summary>
    /// Gets the streaming mode supported by this AI client.
    /// true  = must use streaming access only;
    /// false = must use non-streaming access only;
    /// null  = both modes supported, prefer streaming.
    /// </summary>
    bool? StreamingMode { get; }

    /// <summary>
    /// Sends a chat request to the AI service and returns the response
    /// </summary>
    /// <param name="request">The AI request to send</param>
    /// <returns>The AI response</returns>
    AIResponse Chat(AIRequest request);

    /// <summary>
    /// Sends a chat request to the AI service and returns the response asynchronously
    /// </summary>
    /// <param name="request">The AI request to send</param>
    /// <returns>The AI response</returns>
    Task<AIResponse> ChatAsync(AIRequest request);

    /// <summary>
    /// Sends a streaming chat request to the AI service, yielding incremental responses.
    /// Each yielded AIResponse contains only the incremental token content.
    /// The final yield has IsStreamFinal set to true and contains usage stats.
    /// </summary>
    /// <param name="request">The AI request to send</param>
    /// <param name="cancellationToken">Cancellation token to abort the stream</param>
    /// <returns>An async enumerable of incremental AI responses</returns>
    IAsyncEnumerable<AIResponse> ChatStreamAsync(AIRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a chat request with a single user message
    /// </summary>
    /// <param name="userMessage">The user message to send</param>
    /// <returns>The AI response</returns>
    AIResponse Chat(string userMessage);

    /// <summary>
    /// Sends a chat request with a single user message asynchronously
    /// </summary>
    /// <param name="userMessage">The user message to send</param>
    /// <returns>The AI response</returns>
    Task<AIResponse> ChatAsync(string userMessage);

    /// <summary>
    /// Sends a chat request with system prompt and user message
    /// </summary>
    /// <param name="systemPrompt">The system prompt</param>
    /// <param name="userMessage">The user message</param>
    /// <returns>The AI response</returns>
    AIResponse Chat(string systemPrompt, string userMessage);

    /// <summary>
    /// Sends a chat request with system prompt and user message asynchronously
    /// </summary>
    /// <param name="systemPrompt">The system prompt</param>
    /// <param name="userMessage">The user message</param>
    /// <returns>The AI response</returns>
    Task<AIResponse> ChatAsync(string systemPrompt, string userMessage);

    /// <summary>
    /// Sends a generation request to the AI service (non-chat, e.g. Ollama /api/generate)
    /// </summary>
    /// <param name="prompt">The prompt to generate from</param>
    /// <returns>The AI response</returns>
    AIResponse Generate(string prompt);

    /// <summary>
    /// Sends a generation request to the AI service asynchronously
    /// </summary>
    /// <param name="prompt">The prompt to generate from</param>
    /// <returns>The AI response</returns>
    Task<AIResponse> GenerateAsync(string prompt);

    /// <summary>
    /// Sends a generation request with system prompt
    /// </summary>
    /// <param name="systemPrompt">The system prompt</param>
    /// <param name="prompt">The user prompt</param>
    /// <returns>The AI response</returns>
    AIResponse Generate(string systemPrompt, string prompt);

    /// <summary>
    /// Sends a generation request with system prompt asynchronously
    /// </summary>
    /// <param name="systemPrompt">The system prompt</param>
    /// <param name="prompt">The user prompt</param>
    /// <returns>The AI response</returns>
    Task<AIResponse> GenerateAsync(string systemPrompt, string prompt);
}
