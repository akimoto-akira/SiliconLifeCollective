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
/// Immutable record of a single AI request's token usage.
/// Serves as the atomic entry in the token usage audit log.
/// </summary>
public sealed class TokenUsageRecord
{
    /// <summary>Unique identifier for this record</summary>
    public Guid Id { get; }

    /// <summary>Timestamp of the AI request (UTC)</summary>
    public DateTime Timestamp { get; }

    /// <summary>GUID of the silicon being that initiated the request</summary>
    public Guid BeingId { get; }

    /// <summary>
    /// AI client type identifier (IAIClient implementation class name).
    /// Matches AIClientConfigAttribute.ClientType convention (e.g. "OllamaClient").
    /// </summary>
    public string AIClientType { get; }

    /// <summary>Number of tokens in the prompt (input)</summary>
    public int PromptTokens { get; }

    /// <summary>Number of tokens in the completion (output)</summary>
    public int CompletionTokens { get; }

    /// <summary>Total tokens used (input + output)</summary>
    public int TotalTokens { get; }

    /// <summary>Session ID associated with this request</summary>
    public Guid SessionId { get; }

    /// <summary>Whether the request succeeded</summary>
    public bool Success { get; }

    public TokenUsageRecord(
        DateTime timestamp,
        Guid beingId,
        string aiClientType,
        int promptTokens,
        int completionTokens,
        int totalTokens,
        Guid sessionId,
        bool success)
    {
        Id = Guid.NewGuid();
        Timestamp = timestamp;
        BeingId = beingId;
        AIClientType = aiClientType;
        PromptTokens = promptTokens;
        CompletionTokens = completionTokens;
        TotalTokens = totalTokens;
        SessionId = sessionId;
        Success = success;
    }
}
