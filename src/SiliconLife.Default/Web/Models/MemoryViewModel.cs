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

namespace SiliconLife.Default.Web.Models;

public class MemoryViewModel : ViewModelBase
{
    /// <summary>Current page memory list</summary>
    public List<MemoryItem> Memories { get; set; } = new();

    /// <summary>Currently selected being ID</summary>
    public Guid? CurrentBeingId { get; set; }

    /// <summary>Search query keyword</summary>
    public string SearchQuery { get; set; } = string.Empty;

    /// <summary>Available beings list for selector</summary>
    public List<BeingInfo> AvailableBeings { get; set; } = new();

    /// <summary>Current page number</summary>
    public int CurrentPage { get; set; } = 1;

    /// <summary>Page size</summary>
    public int PageSize { get; set; } = 20;

    /// <summary>Total count of memories</summary>
    public int TotalCount { get; set; }

    /// <summary>Total pages</summary>
    public int TotalPages { get; set; }

    /// <summary>Memory statistics summary</summary>
    public MemoryStatisticsModel? Statistics { get; set; }

    /// <summary>Filter by memory type</summary>
    public string? FilterType { get; set; }

    /// <summary>Filter by date range start</summary>
    public DateTime? FilterStartDate { get; set; }

    /// <summary>Filter by date range end</summary>
    public DateTime? FilterEndDate { get; set; }

    /// <summary>Show summaries only mode</summary>
    public bool ShowSummariesOnly { get; set; }
}

public class MemoryItem
{
    /// <summary>Unique ID</summary>
    public Guid Id { get; set; }

    /// <summary>Memory content</summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>Whether this is a compression summary</summary>
    public bool IsSummary { get; set; }

    /// <summary>Related being IDs</summary>
    public List<Guid> RelatedBeings { get; set; } = new();

    /// <summary>Creation time (from MemoryEntry.Timestamp)</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Formatted timestamp display string</summary>
    public string TimestampDisplay { get; set; } = string.Empty;

    /// <summary>Memory type (e.g., chat, tool_call, task, timer)</summary>
    public string? Type { get; set; }

    /// <summary>Keywords list</summary>
    public List<string> Keywords { get; set; } = new();
}

/// <summary>
/// Being info for selector dropdown
/// </summary>
public class BeingInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Memory statistics summary model
/// </summary>
public class MemoryStatisticsModel
{
    public int TotalEntries { get; set; }
    public string? OldestEntry { get; set; }
    public string? NewestEntry { get; set; }
    public int SummaryCount { get; set; }
}
