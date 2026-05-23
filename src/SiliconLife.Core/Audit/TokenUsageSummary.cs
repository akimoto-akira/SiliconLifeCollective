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
/// Aggregated token usage statistics for a time range.
/// Returned by query methods of <see cref="ITokenUsageAudit"/>.
/// </summary>
public sealed class TokenUsageSummary
{
    /// <summary>Total number of AI requests in the time range</summary>
    public int RequestCount { get; init; }

    /// <summary>Number of successful requests</summary>
    public int SuccessCount { get; init; }

    /// <summary>Number of failed requests</summary>
    public int FailureCount { get; init; }

    /// <summary>Total prompt tokens consumed</summary>
    public long TotalPromptTokens { get; init; }

    /// <summary>Total completion tokens consumed</summary>
    public long TotalCompletionTokens { get; init; }

    /// <summary>Total tokens consumed (prompt + completion)</summary>
    public long TotalTokens { get; init; }

    /// <summary>Breakdown by AI client type (IAIClient implementation class name)</summary>
    public Dictionary<string, TokenUsageSummary> ByAIClientType { get; init; } = new();

    /// <summary>Breakdown by silicon being ID</summary>
    public Dictionary<Guid, TokenUsageSummary> ByBeingId { get; init; } = new();
}
