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
/// Represents a single memory entry stored in the memory system.
/// </summary>
public sealed class MemoryEntry
{
    /// <summary>
    /// Gets or sets the unique identifier of the memory entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the content of the memory entry.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp of the memory entry.
    /// </summary>
    public IncompleteDate Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the list of related being IDs associated with this memory.
    /// </summary>
    public List<Guid> RelatedBeings { get; set; } = new();

    /// <summary>
    /// Gets or sets whether this entry is a compression summary rather than
    /// an original memory record.
    /// </summary>
    public bool IsSummary { get; set; }

    /// <summary>
    /// Gets or sets the type/category of this memory entry (e.g., "chat", "tool_call", "task", "timer").
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the list of keywords associated with this memory entry for search and retrieval.
    /// </summary>
    public List<string> Keywords { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the MemoryEntry class with the current timestamp.
    /// </summary>
    public MemoryEntry()
    {
        Id = Guid.NewGuid();
        var now = DateTime.Now;
        Timestamp = new IncompleteDate(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
    }

    /// <summary>
    /// Initializes a new instance of the MemoryEntry class with the specified content and current timestamp.
    /// </summary>
    /// <param name="content">The content of the memory entry.</param>
    public MemoryEntry(string content) : this()
    {
        Content = content;
    }

    /// <summary>
    /// Adds a related being ID to this memory entry if it doesn't already exist.
    /// </summary>
    /// <param name="beingId">The ID of the related being.</param>
    public void AddRelatedBeing(Guid beingId)
    {
        if (!RelatedBeings.Contains(beingId))
        {
            RelatedBeings.Add(beingId);
        }
    }
}

/// <summary>
/// Provides statistics about the memory system.
/// </summary>
public sealed class MemoryStatistics
{
    /// <summary>
    /// Gets or sets the total number of memory entries.
    /// </summary>
    public int TotalEntries { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the oldest entry.
    /// </summary>
    public IncompleteDate? OldestEntry { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the newest entry.
    /// </summary>
    public IncompleteDate? NewestEntry { get; set; }
}

/// <summary>
/// Memory system for storing and retrieving memory entries associated with a silicon being.
/// </summary>
public sealed class Memory
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<Memory>();
    private readonly ITimeStorage _timeStorage;
    private readonly string _storageKey;

    /// <summary>
    /// Gets the total number of memory entries.
    /// </summary>
    public int Count
    {
        get
        {
            try
            {
                return _timeStorage.Query<MemoryEntry>(_storageKey, null).Count;
            }
            catch
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the Memory class with the specified time storage.
    /// </summary>
    /// <param name="timeStorage">The time storage to use for persisting memory entries.</param>
    /// <exception cref="ArgumentNullException">Thrown when timeStorage is null.</exception>
    public Memory(ITimeStorage timeStorage)
    {
        _timeStorage = timeStorage ?? throw new ArgumentNullException(nameof(timeStorage));
        _storageKey = "memory";
        _logger.Info(null, "Memory system initialized with {0}", _storageKey);
    }

    private void Save(MemoryEntry entry)
    {
        _timeStorage.Write(_storageKey, entry.Timestamp, entry);
    }

    /// <summary>
    /// Adds a new memory entry with the current timestamp.
    /// </summary>
    /// <param name="content">The content of the memory entry.</param>
    /// <param name="relatedBeings">Optional list of related being IDs.</param>
    /// <returns>The created memory entry.</returns>
    public MemoryEntry Add(string content, List<Guid>? relatedBeings = null)
    {
        var now = DateTime.Now;
        var entry = new MemoryEntry(content)
        {
            Timestamp = new IncompleteDate(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second)
        };

        if (relatedBeings != null)
        {
            foreach (var beingId in relatedBeings)
            {
                entry.AddRelatedBeing(beingId);
            }
        }

        Save(entry);

        _logger.Debug(null, "Memory added: {0}..., id={1}", content.Length > 30 ? content[..30] : content, entry.Id);

        return entry;
    }

    /// <summary>
    /// Adds a new memory entry with the specified timestamp.
    /// </summary>
    /// <param name="timestamp">The timestamp of the memory entry.</param>
    /// <param name="content">The content of the memory entry.</param>
    /// <param name="relatedBeings">Optional list of related being IDs.</param>
    /// <returns>The created memory entry.</returns>
    public MemoryEntry Add(IncompleteDate timestamp, string content, List<Guid>? relatedBeings = null)
    {
        var entry = new MemoryEntry(content)
        {
            Timestamp = timestamp
        };

        if (relatedBeings != null)
        {
            foreach (var beingId in relatedBeings)
            {
                entry.AddRelatedBeing(beingId);
            }
        }

        Save(entry);

        _logger.Debug(null, "Memory added: {0}..., id={1}", content.Length > 30 ? content[..30] : content, entry.Id);

        return entry;
    }

    /// <summary>
    /// Adds a compression summary entry at the resolved timestamp of the given
    /// <paramref name="level"/>. Summary entries are marked with
    /// <see cref="MemoryEntry.IsSummary"/> = <c>true</c> so that
    /// <see cref="HasSummary"/> can detect them without deleting original records.
    /// </summary>
    /// <param name="level">The time level this summary covers.</param>
    /// <param name="content">The compressed summary text.</param>
    /// <returns>The created summary entry.</returns>
    public MemoryEntry AddSummary(IncompleteDate level, string content)
    {
        var entry = new MemoryEntry(content)
        {
            Timestamp = level,
            IsSummary = true
        };

        Save(entry);

        _logger.Debug(null, "Memory summary added: {0}..., level={1}", content.Length > 30 ? content[..30] : content, level);

        return entry;
    }

    /// <summary>
    /// Gets the most recent memory entries by progressively widening the time range:
    /// today ï¿?this month ï¿?this year ï¿?all entries.
    /// </summary>
    /// <param name="count">The maximum number of entries to return.</param>
    /// <returns>A list of recent memory entries ordered by timestamp descending.</returns>
    public List<MemoryEntry> GetRecent(int count = 10)
    {
        var now = DateTime.Now;

        // today
        var results = Query(new IncompleteDate(now.Year, now.Month, now.Day), count);
        if (results.Count >= count) return results;

        // this month
        results = Query(new IncompleteDate(now.Year, now.Month), count);
        if (results.Count >= count) return results;

        // this year
        results = Query(new IncompleteDate(now.Year), count);
        if (results.Count >= count) return results;

        // all
        return QueryAll(count);
    }

    /// <summary>
    /// Queries memory entries within a specific time range.
    /// </summary>
    /// <param name="range">The time range to query (unspecified components are wildcards). Null means all entries.</param>
    /// <param name="count">Maximum number of entries to return. 0 means no limit.</param>
    /// <returns>A list of matching memory entries ordered by timestamp descending.</returns>
    public List<MemoryEntry> Query(IncompleteDate? range, int count = 0)
    {
        var entries = _timeStorage.Query<MemoryEntry>(_storageKey, range)
            .OrderByDescending(e => e.Timestamp)
            .Select(e => e.Data);

        return (count > 0 ? entries.Take(count) : entries).ToList();
    }

    /// <summary>
    /// Returns all memory entries ordered by timestamp descending.
    /// </summary>
    /// <param name="count">Maximum number of entries to return. 0 means no limit.</param>
    public List<MemoryEntry> QueryAll(int count = 0)
    {
        var entries = _timeStorage.Query<MemoryEntry>(_storageKey, null)
            .OrderByDescending(e => e.Timestamp)
            .Select(e => e.Data);
        return (count > 0 ? entries.Take(count) : entries).ToList();
    }

    /// <summary>
    /// Gets statistics about the memory system including total entries and oldest/newest timestamps.
    /// </summary>
    /// <returns>A MemoryStatistics object with memory system statistics.</returns>
    public MemoryStatistics GetStatistics()
    {
        var all = _timeStorage.Query<MemoryEntry>(_storageKey, null);
        var oldest = all.MinBy(e => e.Timestamp);
        var newest = all.MaxBy(e => e.Timestamp);

        return new MemoryStatistics
        {
            TotalEntries = all.Count,
            OldestEntry = oldest != null ? new IncompleteDate(oldest.Timestamp.Year, oldest.Timestamp.Month, oldest.Timestamp.Day, oldest.Timestamp.Hour, oldest.Timestamp.Minute, oldest.Timestamp.Second) : null,
            NewestEntry = newest != null ? new IncompleteDate(newest.Timestamp.Year, newest.Timestamp.Month, newest.Timestamp.Day, newest.Timestamp.Hour, newest.Timestamp.Minute, newest.Timestamp.Second) : null,
        };
    }

    /// <summary>
    /// Determines whether the memory system should be compressed based on entry count.
    /// </summary>
    /// <returns>True if compression is needed; otherwise, false.</returns>
    public bool ShouldCompress()
    {
        var compressLevel = FindLevelToCompress();
        if (compressLevel.HasValue)
        {
            _logger.Debug(null, "Memory compression check: compression needed");
        }
        return compressLevel.HasValue;
    }

    /// <summary>
    /// Finds the time level that needs compression based on the number of entries.
    /// </summary>
    /// <returns>A tuple containing the level and entries to compress, or null if no compression is needed.</returns>
    public (IncompleteDate Level, List<TimeEntry<MemoryEntry>> Entries)? FindLevelToCompress()
    {
        var now = DateTime.Now;

        var second = new IncompleteDate(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        if (!HasSummary(second))
        {
            var entries = _timeStorage.Query<MemoryEntry>(_storageKey, second);
            if (entries.Count > 0)
                return (second, entries);
        }

        var minute = new IncompleteDate(now.Year, now.Month, now.Day, now.Hour, now.Minute);
        if (!HasSummary(minute))
        {
            var entries = _timeStorage.Query<MemoryEntry>(_storageKey, minute);
            if (entries.Count > 0)
                return (minute, entries);
        }

        var hour = new IncompleteDate(now.Year, now.Month, now.Day, now.Hour);
        if (!HasSummary(hour))
        {
            var entries = _timeStorage.Query<MemoryEntry>(_storageKey, hour);
            if (entries.Count > 0)
                return (hour, entries);
        }

        var day = new IncompleteDate(now.Year, now.Month, now.Day);
        if (!HasSummary(day))
        {
            var entries = _timeStorage.Query<MemoryEntry>(_storageKey, day);
            if (entries.Count > 0)
                return (day, entries);
        }

        var month = new IncompleteDate(now.Year, now.Month);
        if (!HasSummary(month))
        {
            var entries = _timeStorage.Query<MemoryEntry>(_storageKey, month);
            if (entries.Count > 0)
                return (month, entries);
        }

        var year = new IncompleteDate(now.Year);
        if (!HasSummary(year))
        {
            var entries = _timeStorage.Query<MemoryEntry>(_storageKey, year);
            if (entries.Count > 0)
                return (year, entries);
        }

        for (int y = now.Year - 1; y >= now.Year - 3; y--)
        {
            var pastYear = new IncompleteDate(y);
            if (!HasSummary(pastYear))
            {
                var entries = _timeStorage.Query<MemoryEntry>(_storageKey, pastYear);
                if (entries.Count > 0)
                    return (pastYear, entries);
            }
        }

        return null;
    }

    private bool HasSummary(IncompleteDate level)
    {
        return _timeStorage.Query<MemoryEntry>(_storageKey, level)
            .Any(e => e.Data.IsSummary);
    }

    /// <summary>
    /// Gets the entries available for compression at the current compression level.
    /// </summary>
    /// <param name="count">The maximum number of entries to retrieve.</param>
    /// <returns>A list of time entries available for compression.</returns>
    public List<TimeEntry<MemoryEntry>> GetEntriesForCompression(int count = 10)
    {
        var compressData = FindLevelToCompress();
        if (!compressData.HasValue)
            return new List<TimeEntry<MemoryEntry>>();

        return compressData.Value.Entries.Take(count).ToList();
    }

    /// <summary>
    /// Searches memory entries by keyword in content and keywords list.
    /// </summary>
    /// <param name="keyword">The keyword to search for.</param>
    /// <param name="count">Maximum number of entries to return. 0 means no limit.</param>
    /// <returns>A list of matching memory entries ordered by timestamp descending.</returns>
    public List<MemoryEntry> Search(string keyword, int count = 0)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return new List<MemoryEntry>();

        var allEntries = _timeStorage.Query<MemoryEntry>(_storageKey, null);
        var keywordLower = keyword.ToLowerInvariant();

        var results = allEntries
            .Where(e => e.Data.Content.ToLowerInvariant().Contains(keywordLower) ||
                        e.Data.Keywords.Any(k => k.ToLowerInvariant().Contains(keywordLower)))
            .OrderByDescending(e => e.Timestamp)
            .Select(e => e.Data);

        return (count > 0 ? results.Take(count) : results).ToList();
    }

    /// <summary>
    /// Queries memory entries by type/category.
    /// </summary>
    /// <param name="type">The memory type to filter by.</param>
    /// <param name="count">Maximum number of entries to return. 0 means no limit.</param>
    /// <returns>A list of matching memory entries ordered by timestamp descending.</returns>
    public List<MemoryEntry> GetByType(string type, int count = 0)
    {
        if (string.IsNullOrWhiteSpace(type))
            return QueryAll(count);

        var allEntries = _timeStorage.Query<MemoryEntry>(_storageKey, null);
        var results = allEntries
            .Where(e => e.Data.Type == type)
            .OrderByDescending(e => e.Timestamp)
            .Select(e => e.Data);

        return (count > 0 ? results.Take(count) : results).ToList();
    }
}