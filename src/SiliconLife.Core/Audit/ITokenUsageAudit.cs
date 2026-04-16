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
/// Core interface for token usage auditing.
/// Records AI request token consumption and provides time-ranged
/// aggregation queries with multi-dimensional breakdowns.
/// </summary>
public interface ITokenUsageAudit
{
    /// <summary>
    /// Records a token usage event.
    /// Implementations should persist the record for later querying.
    /// </summary>
    /// <param name="record">The token usage record to log</param>
    void Record(TokenUsageRecord record);

    /// <summary>
    /// Queries raw token usage records within the specified time range and filters.
    /// Returns individual records without aggregation.
    /// </summary>
    /// <param name="query">Query parameters including time range and optional filters</param>
    /// <returns>List of matching token usage records, ordered by timestamp ascending</returns>
    List<TokenUsageRecord> QueryRecords(TokenUsageQuery query);

    /// <summary>
    /// Queries aggregated token usage summary within the specified time range and filters.
    /// Returns a summary with optional dimensional breakdowns based on query flags.
    /// </summary>
    /// <param name="query">Query parameters including time range, filters, and group-by flags</param>
    /// <returns>Aggregated token usage summary</returns>
    TokenUsageSummary QuerySummary(TokenUsageQuery query);

    /// <summary>
    /// Gets the total token count for a specific silicon being within a time range.
    /// Convenience method equivalent to QuerySummary with BeingId filter.
    /// </summary>
    /// <param name="beingId">The silicon being ID</param>
    /// <param name="range">The time range</param>
    /// <returns>Total tokens consumed by the specified being in the time range</returns>
    long GetTotalTokensForBeing(Guid beingId, IncompleteDate range);

    /// <summary>
    /// Gets the total token count for a specific AI client type within a time range.
    /// Convenience method equivalent to QuerySummary with AIClientType filter.
    /// </summary>
    /// <param name="aiClientType">The AI client type identifier (IAIClient implementation class name)</param>
    /// <param name="range">The time range</param>
    /// <returns>Total tokens consumed via the specified AI client type in the time range</returns>
    long GetTotalTokensForAIClientType(string aiClientType, IncompleteDate range);
}
