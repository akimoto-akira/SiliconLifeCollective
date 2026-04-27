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
    /// today �?this month �?this year �?all entries.
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
    /// Determines whether the memory system should be compressed and returns the compression level information.
    /// </summary>
    /// <param name="compressData">The compression level and entries to compress, or null if no compression is needed.</param>
    /// <returns>True if compression is needed; otherwise, false.</returns>
    public bool ShouldCompress(out (IncompleteDate Level, List<TimeEntry<MemoryEntry>> Entries)? compressData)
    {
        compressData = FindLevelToCompress();
        if (compressData.HasValue)
        {
            _logger.Debug(null, "Memory compression check: compression needed at level {0}", compressData.Value.Level);
        }
        return compressData.HasValue;
    }

    /// <summary>
    /// Finds the time level that needs compression based on the number of entries.
    /// Starts from the earliest memory time and works forward with hierarchical checking.
    /// Returns the most precise uncompressed level with its child summaries or original entries.
    /// </summary>
    /// <returns>A tuple containing the level and entries to compress, or null if no compression is needed.</returns>
    public (IncompleteDate Level, List<TimeEntry<MemoryEntry>> Entries)? FindLevelToCompress()
    {
        IncompleteDate? earliestTime = _timeStorage.GetEarliestTimestamp(_storageKey);
        if (!earliestTime.HasValue)
        {
            return null;
        }
        
        for (int year = earliestTime.Value.Year; year <= DateTime.Now.Year; year++)
        {
            var result = FindYearToCompress(year, earliestTime.Value);
            if (result.HasValue)
            {
                return result;
            }
        }

        return null;
    }

    private (IncompleteDate Level, List<TimeEntry<MemoryEntry>> Entries)? FindYearToCompress(int year, IncompleteDate earliestTime)
    {
        var yearLevel = new IncompleteDate(year);
        
        if (HasSummary(yearLevel))
        {
            // This year is already compressed, skip
            return null;
        }

        int startMonth = (earliestTime.Year == year) ? (earliestTime.Month ?? 1) : 1;
        int fullMonth = (DateTime.Now.Year == year) ? DateTime.Now.Month : 12;
        // This year is not compressed, check all months (1-12)
        for (int month = startMonth; month <= fullMonth; month++)
        {
            var result = FindMonthToCompress(year, month, earliestTime);
            if (result.HasValue)
            {
                return result;
            }
        }
        if (DateTime.Now.Year == year)
        {
            return null;
        }
        // If all months in this year are compressed, return all month-level summaries for this year
        var yearEntries = GetSummaryEntries(year, null, null, null, null);
        if (yearEntries.Count > 0)
        {
            return (yearLevel, yearEntries);
        }
        
        // This year is not compressed, return all memories in this year (original or child summaries)
        var yearMemoryEntries = _timeStorage.QueryWithLevel<MemoryEntry>(_storageKey, yearLevel);
        if (yearMemoryEntries.Count > 0)
        {
            return (yearLevel, yearMemoryEntries);
        }
        
        return null;
    }

    private (IncompleteDate Level, List<TimeEntry<MemoryEntry>> Entries)? FindMonthToCompress(int year, int month, IncompleteDate earliestTime)
    {
        var monthLevel = new IncompleteDate(year, month);
        
        if (HasSummary(monthLevel))
        {
            // This month is already compressed, skip
            return null;
        }

        // This month is not compressed, check all days
        int daysInMonth = DateTime.DaysInMonth(year, month);
        if (DateTime.Now.Year == year && DateTime.Now.Month == month)
        {
            daysInMonth = DateTime.Now.Day;
        }
        int startDay = (earliestTime.Year == year && earliestTime.Month == month) ? (earliestTime.Day ?? 1) : 1;
        for (int day = startDay; day <= daysInMonth; day++)
        {
            var result = FindDayToCompress(year, month, day, earliestTime);
            if (result.HasValue)
            {
                return result;
            }
        }
        
        if (DateTime.Now.Year == year && DateTime.Now.Month == month)
        {
            return null;
        }
        // If all days in this month are compressed, return all day-level summaries for this month
        var monthEntries = GetSummaryEntries(year, month, null, null, null);
        if (monthEntries.Count > 0)
        {
            return (monthLevel, monthEntries);
        }
        
        // This month is not compressed, return all memories in this month (original or child summaries)
        var monthMemoryEntries = _timeStorage.QueryWithLevel<MemoryEntry>(_storageKey, monthLevel);
        if (monthMemoryEntries.Count > 0)
        {
            return (monthLevel, monthMemoryEntries);
        }
        
        return null;
    }

    private (IncompleteDate Level, List<TimeEntry<MemoryEntry>> Entries)? FindDayToCompress(int year, int month, int day, IncompleteDate earliestTime)
    {
        var dayLevel = new IncompleteDate(year, month, day);
        
        if (HasSummary(dayLevel))
        {
            // This day is already compressed, skip
            return null;
        }

        int fullHour = 23;
        if (DateTime.Now.Year == year && DateTime.Now.Month == month && DateTime.Now.Day == day)
        {
            fullHour = DateTime.Now.Hour;
        }
        int startHour = (earliestTime.Year == year && earliestTime.Month == month && earliestTime.Day == day) ? (earliestTime.Hour ?? 0) : 0;
        // This day is not compressed, check all hours (0-23)
        for (int hour = startHour; hour <= fullHour; hour++)
        {
            var result = FindHourToCompress(year, month, day, hour, earliestTime);
            if (result.HasValue)
            {
                return result;
            }
        }
        
        if (DateTime.Now.Year == year && DateTime.Now.Month == month && DateTime.Now.Day == day)
        {
            return null;
        }
        // If all hours in this day are compressed, return all hour-level summaries for this day
        var dayEntries = GetSummaryEntries(year, month, day, null, null);
        if (dayEntries.Count > 0)
        {
            return (dayLevel, dayEntries);
        }
        
        // This day is not compressed, return all memories in this day (original or child summaries)
        var dayMemoryEntries = _timeStorage.QueryWithLevel<MemoryEntry>(_storageKey, dayLevel);
        if (dayMemoryEntries.Count > 0)
        {
            return (dayLevel, dayMemoryEntries);
        }
        
        return null;
    }

    private (IncompleteDate Level, List<TimeEntry<MemoryEntry>> Entries)? FindHourToCompress(int year, int month, int day, int hour, IncompleteDate earliestTime)
    {
        var hourLevel = new IncompleteDate(year, month, day, hour);
        
        if (HasSummary(hourLevel))
        {
            // This hour is already compressed, skip
            return null;
        }
        
        // This hour is not compressed, check all minutes (0-59)
        for (int minute = 0; minute <= 59; minute++)
        {
            if (DateTime.Now.Year == year && DateTime.Now.Month == month && DateTime.Now.Day == day &&
                DateTime.Now.Hour == hour && DateTime.Now.Minute == minute)
            {
                break;
            }
            var minuteLevel = new IncompleteDate(year, month, day, hour, minute);
            
            if (!HasSummary(minuteLevel))
            {
                // Found the most precise uncompressed level: minute
                var entries = _timeStorage.Query<MemoryEntry>(_storageKey, minuteLevel);
                if (entries.Count > 0)
                {
                    return (minuteLevel, entries);
                }
                // This minute has no memories, continue to next minute
            }
        }
        
        if (DateTime.Now.Year == year && DateTime.Now.Month == month && DateTime.Now.Day == day &&
            DateTime.Now.Hour == hour)
        {
            return null;
        }
        // If all minutes in this hour are compressed, return all minute-level summaries for this hour
        var hourEntries = GetSummaryEntries(year, month, day, hour, null);
        if (hourEntries.Count > 0)
        {
            return (hourLevel, hourEntries);
        }
        
        // This hour is not compressed, return all memories in this hour (original or child summaries)
        var hourMemoryEntries = _timeStorage.QueryWithLevel<MemoryEntry>(_storageKey, hourLevel);
        if (hourMemoryEntries.Count > 0)
        {
            return (hourLevel, hourMemoryEntries);
        }
        
        return null;
    }

    /// <summary>
    /// Gets compression summary entries for a specific time level.
    /// Uses QueryWithLevel to query the next finer time level efficiently.
    /// </summary>
    private List<TimeEntry<MemoryEntry>> GetSummaryEntries(int year, int? month, int? day, int? hour, int? minute)
    {
        var summaries = new List<TimeEntry<MemoryEntry>>();
        
        // Build the IncompleteDate for the current level
        IncompleteDate currentLevel;
        
        if (minute.HasValue)
        {
            // Minute level - query all second-level summaries under this minute
            currentLevel = new IncompleteDate(year, month!.Value, day!.Value, hour!.Value, minute.Value);
        }
        else if (hour.HasValue)
        {
            // Hour level - query all minute-level summaries under this hour
            currentLevel = new IncompleteDate(year, month!.Value, day!.Value, hour.Value);
        }
        else if (day.HasValue)
        {
            // Day level - query all hour-level summaries under this day
            currentLevel = new IncompleteDate(year, month!.Value, day.Value);
        }
        else if (month.HasValue)
        {
            // Month level - query all day-level summaries under this month
            currentLevel = new IncompleteDate(year, month.Value);
        }
        else
        {
            // Year level - query all month-level summaries for this year
            currentLevel = new IncompleteDate(year);
        }
        
        // Use QueryWithLevel to get all entries at the next finer level
        var allEntries = _timeStorage.QueryWithLevel<MemoryEntry>(_storageKey, currentLevel);
        summaries = allEntries.Where(e => e.Data.IsSummary).ToList();
        
        return summaries;
    }

    private bool HasSummary(IncompleteDate level)
    {
        return _timeStorage.HasSummary<MemoryEntry>(_storageKey, level, e => e.IsSummary);
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
    /// Gets the recent compression summaries to inject as memory context for chat scenarios.
    /// Loads the highest-available-level summary for each completed period (year/month/day/hour),
    /// and drills down into the current (uncompleted) period until reaching the minute level.
    /// </summary>
    /// <returns>A list of summary entries ordered by timestamp descending (most recent first).</returns>
    public List<MemoryEntry> GetMemoryContextForChat()
    {
        IncompleteDate? earliest = _timeStorage.GetEarliestTimestamp(_storageKey);
        List<MemoryEntry> result = new List<MemoryEntry>();
        if (!earliest.HasValue) return result;

        var now = DateTime.Now;
        for (int year = earliest.Value.Year; year <= now.Year; year++)
        {
            if (year != now.Year)
            {
                // Completed year: load year-level summary
                AddSummaryAt(new IncompleteDate(year), result);
                continue;
            }

            // Current year: drill down to months
            for (int month = 1; month <= now.Month; month++)
            {
                if (month != now.Month)
                {
                    // Completed month: load month-level summary
                    AddSummaryAt(new IncompleteDate(year, month), result);
                    continue;
                }

                // Current month: drill down to days
                for (int day = 1; day <= now.Day; day++)
                {
                    if (day != now.Day)
                    {
                        // Completed day: load day-level summary
                        AddSummaryAt(new IncompleteDate(year, month, day), result);
                        continue;
                    }

                    // Current day: drill down to hours
                    for (int hour = 0; hour <= now.Hour; hour++)
                    {
                        if (hour != now.Hour)
                        {
                            // Completed hour: load hour-level summary
                            AddSummaryAt(new IncompleteDate(year, month, day, hour), result);
                            continue;
                        }

                        // Current hour: drill down to minutes
                        for (int minute = 0; minute < now.Minute; minute++)
                        {
                            // Completed minute: load minute-level summary
                            AddSummaryAt(new IncompleteDate(year, month, day, hour, minute), result);
                        }
                    }
                }
            }
        }

        // Sort by timestamp descending (most recent first)
        result.Sort((a, b) => b.Timestamp.CompareTo(a.Timestamp));
        return result;
    }

    /// <summary>
    /// Loads the summary entry at the given time level (if present) and appends it to the result list.
    /// Uses Query + exact timestamp match because ITimeStorage.Read on a non-second-level
    /// IncompleteDate recursively searches the enclosing directory and returns the first file,
    /// which would otherwise return the same entry for every level inside the same parent.
    /// </summary>
    private void AddSummaryAt(IncompleteDate level, List<MemoryEntry> result)
    {
        var entries = _timeStorage.Query<MemoryEntry>(_storageKey, level);
        foreach (var entry in entries)
        {
            if (entry.Data != null && entry.Data.IsSummary && entry.Timestamp.Equals(level))
            {
                result.Add(entry.Data);
                return;
            }
        }
    }
}