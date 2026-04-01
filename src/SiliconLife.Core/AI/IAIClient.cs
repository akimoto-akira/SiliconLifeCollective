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
}
