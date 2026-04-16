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

internal sealed class TokenUsageRecordData
{
    public Guid Id { get; set; }
    public Guid BeingId { get; set; }
    public string AIClientType { get; set; } = string.Empty;
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
    public Guid SessionId { get; set; }
    public bool Success { get; set; }
}

public class TokenUsageAuditManager : ITokenUsageAudit
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<TokenUsageAuditManager>();
    private readonly ITimeStorage? _timeStorage;
    private const string StorageKey = "token-usage";

    public TokenUsageAuditManager()
    {
    }

    public TokenUsageAuditManager(ITimeStorage timeStorage)
    {
        _timeStorage = timeStorage;
    }

    public void Record(TokenUsageRecord record)
    {
        if (_timeStorage == null)
        {
            _logger.Warn("TokenUsageAudit: ITimeStorage not available, record not persisted");
            return;
        }

        try
        {
            var data = new TokenUsageRecordData
            {
                Id = record.Id,
                BeingId = record.BeingId,
                AIClientType = record.AIClientType,
                PromptTokens = record.PromptTokens,
                CompletionTokens = record.CompletionTokens,
                TotalTokens = record.TotalTokens,
                SessionId = record.SessionId,
                Success = record.Success
            };

            _timeStorage.Write(StorageKey, record.Timestamp, data);
            _logger.Debug("Token usage recorded: {0} | Being={1} | Client={2} | Tokens={3}",
                record.Id, record.BeingId, record.AIClientType, record.TotalTokens);
        }
        catch (Exception ex)
        {
            _logger.Warn("Failed to persist token usage record: {0}", ex.Message);
        }
    }

    public List<TokenUsageRecord> QueryRecords(TokenUsageQuery query)
    {
        if (_timeStorage == null)
        {
            return new List<TokenUsageRecord>();
        }

        try
        {
            List<TimeEntry<TokenUsageRecordData>> entries = _timeStorage.Query<TokenUsageRecordData>(StorageKey, query.Range);
            var results = new List<TokenUsageRecord>();

            foreach (TimeEntry<TokenUsageRecordData> entry in entries)
            {
                if (entry.Data == null) continue;

                if (query.BeingId.HasValue && entry.Data.BeingId != query.BeingId.Value)
                    continue;

                if (query.AIClientType != null && entry.Data.AIClientType != query.AIClientType)
                    continue;

                var record = new TokenUsageRecord(
                    entry.Timestamp,
                    entry.Data.BeingId,
                    entry.Data.AIClientType,
                    entry.Data.PromptTokens,
                    entry.Data.CompletionTokens,
                    entry.Data.TotalTokens,
                    entry.Data.SessionId,
                    entry.Data.Success
                );

                results.Add(record);
            }

            return results;
        }
        catch (Exception ex)
        {
            _logger.Warn("Failed to query token usage records: {0}", ex.Message);
            return new List<TokenUsageRecord>();
        }
    }

    public TokenUsageSummary QuerySummary(TokenUsageQuery query)
    {
        List<TokenUsageRecord> records = QueryRecords(query);
        return BuildSummary(records, query.GroupByAIClientType, query.GroupByBeingId);
    }

    public long GetTotalTokensForBeing(Guid beingId, IncompleteDate range)
    {
        var query = new TokenUsageQuery
        {
            Range = range,
            BeingId = beingId
        };

        return QuerySummary(query).TotalTokens;
    }

    public long GetTotalTokensForAIClientType(string aiClientType, IncompleteDate range)
    {
        var query = new TokenUsageQuery
        {
            Range = range,
            AIClientType = aiClientType
        };

        return QuerySummary(query).TotalTokens;
    }

    private static TokenUsageSummary BuildSummary(List<TokenUsageRecord> records, bool groupByAIClientType, bool groupByBeingId)
    {
        if (records.Count == 0)
        {
            return new TokenUsageSummary();
        }

        int requestCount = records.Count;
        int successCount = records.Count(r => r.Success);
        int failureCount = requestCount - successCount;
        long totalPromptTokens = records.Sum(r => (long)r.PromptTokens);
        long totalCompletionTokens = records.Sum(r => (long)r.CompletionTokens);
        long totalTokens = records.Sum(r => (long)r.TotalTokens);

        Dictionary<string, TokenUsageSummary> byAIClientType = groupByAIClientType
            ? records
                .GroupBy(r => r.AIClientType)
                .ToDictionary(g => g.Key, g => BuildSummary(g.ToList(), false, false))
            : new Dictionary<string, TokenUsageSummary>();

        Dictionary<Guid, TokenUsageSummary> byBeingId = groupByBeingId
            ? records
                .GroupBy(r => r.BeingId)
                .ToDictionary(g => g.Key, g => BuildSummary(g.ToList(), false, false))
            : new Dictionary<Guid, TokenUsageSummary>();

        return new TokenUsageSummary
        {
            RequestCount = requestCount,
            SuccessCount = successCount,
            FailureCount = failureCount,
            TotalPromptTokens = totalPromptTokens,
            TotalCompletionTokens = totalCompletionTokens,
            TotalTokens = totalTokens,
            ByAIClientType = byAIClientType,
            ByBeingId = byBeingId
        };
    }
}
