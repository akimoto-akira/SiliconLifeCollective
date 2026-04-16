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
/// Query parameters for token usage audit.
/// All filter fields are optional; unspecified fields act as wildcards.
/// </summary>
public sealed class TokenUsageQuery
{
    /// <summary>Time range for the query (required). Uses IncompleteDate for flexible range matching.</summary>
    public IncompleteDate Range { get; init; }

    /// <summary>Filter by silicon being ID. Null = all beings.</summary>
    public Guid? BeingId { get; init; }

    /// <summary>Filter by AI client type. Null = all client types.</summary>
    public string? AIClientType { get; init; }

    /// <summary>Whether to include breakdown by AI client type in the result</summary>
    public bool GroupByAIClientType { get; init; }

    /// <summary>Whether to include breakdown by silicon being ID in the result</summary>
    public bool GroupByBeingId { get; init; }
}
