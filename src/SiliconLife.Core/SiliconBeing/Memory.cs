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

using System.Text.Json;

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
    private readonly ITimeStorage _timeStorage;
    private readonly string _storageKey;

    private List<MemoryEntry> _memories = new();

    /// <summary>
    /// Gets the total number of memory entries.
    /// </summary>
    public int Count => _memories.Count;

    /// <summary>
    /// Initializes a new instance of the Memory class with the specified time storage.
    /// </summary>
    /// <param name="timeStorage">The time storage to use for persisting memory entries.</param>
    /// <exception cref="ArgumentNullException">Thrown when timeStorage is null.</exception>
    public Memory(ITimeStorage timeStorage)
    {
        _timeStorage = timeStorage ?? throw new ArgumentNullException(nameof(timeStorage));
        _storageKey = "memory";

        Load();
    }

    private void Load()
    {
        try
        {
            var entries = _timeStorage.Query(_storageKey, new IncompleteDate(DateTime.Now.Year));
            _memories = entries
                .Select(e => JsonSerializer.Deserialize<MemoryEntry>(System.Text.Encoding.UTF8.GetString(e.Data)))
                .Where(e => e != null)
                .Cast<MemoryEntry>()
                .ToList();
        }
        catch
        {
            _memories = new List<MemoryEntry>();
        }
    }

    private void Save(MemoryEntry entry)
    {
        string json = JsonSerializer.Serialize(entry);
        byte[] data = System.Text.Encoding.UTF8.GetBytes(json);
        _timeStorage.Write(_storageKey, entry.Timestamp, data);
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

        _memories.Add(entry);
        Save(entry);

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

        _memories.Add(entry);
        Save(entry);

        return entry;
    }

    /// <summary>
    /// Gets the most recent memory entries, searching from the current time backwards.
    /// </summary>
    /// <param name="count">The maximum number of entries to retrieve per time period.</param>
    /// <returns>A list of recent memory entries.</returns>
    public List<MemoryEntry> GetRecent(int count = 2)
    {
        var now = DateTime.Now;
        var results = new List<MemoryEntry>();
        var seenIds = new HashSet<Guid>();

        var hour = now.Hour;
        for (int h = hour; h >= 0 && results.Count < count * 10; h--)
        {
            var entries = _timeStorage.Query(_storageKey, new IncompleteDate(now.Year, now.Month, now.Day, h));
            foreach (var e in entries.OrderByDescending(e => e.Timestamp).Take(count))
            {
                var entry = JsonSerializer.Deserialize<MemoryEntry>(System.Text.Encoding.UTF8.GetString(e.Data));
                if (entry != null && seenIds.Add(entry.Id))
                    results.Add(entry);
            }
        }

        var day = now.Day;
        for (int d = day - 1; d > 0 && results.Count < count * 10; d--)
        {
            var entries = _timeStorage.Query(_storageKey, new IncompleteDate(now.Year, now.Month, d));
            foreach (var e in entries.OrderByDescending(e => e.Timestamp).Take(count))
            {
                var entry = JsonSerializer.Deserialize<MemoryEntry>(System.Text.Encoding.UTF8.GetString(e.Data));
                if (entry != null && seenIds.Add(entry.Id))
                    results.Add(entry);
            }
        }

        var month = now.Month;
        for (int m = month - 1; m > 0 && results.Count < count * 10; m--)
        {
            var entries = _timeStorage.Query(_storageKey, new IncompleteDate(now.Year, m));
            foreach (var e in entries.OrderByDescending(e => e.Timestamp).Take(count))
            {
                var entry = JsonSerializer.Deserialize<MemoryEntry>(System.Text.Encoding.UTF8.GetString(e.Data));
                if (entry != null && seenIds.Add(entry.Id))
                    results.Add(entry);
            }
        }

        for (int y = now.Year - 1; results.Count < count * 10; y--)
        {
            var entries = _timeStorage.Query(_storageKey, new IncompleteDate(y));
            foreach (var e in entries.OrderByDescending(e => e.Timestamp).Take(count))
            {
                var entry = JsonSerializer.Deserialize<MemoryEntry>(System.Text.Encoding.UTF8.GetString(e.Data));
                if (entry != null && seenIds.Add(entry.Id))
                    results.Add(entry);
            }
            if (y <= 0) break;
        }

        return results.Take(count).ToList();
    }

    /// <summary>
    /// Gets statistics about the memory system including total entries and oldest/newest timestamps.
    /// </summary>
    /// <returns>A MemoryStatistics object with memory system statistics.</returns>
    public MemoryStatistics GetStatistics()
    {
        var oldest = _memories.MinBy(m => m.Timestamp);
        var newest = _memories.MaxBy(m => m.Timestamp);

        return new MemoryStatistics
        {
            TotalEntries = _memories.Count,
            OldestEntry = oldest?.Timestamp,
            NewestEntry = newest?.Timestamp
        };
    }

    /// <summary>
    /// Determines whether the memory system should be compressed based on entry count.
    /// </summary>
    /// <returns>True if compression is needed; otherwise, false.</returns>
    public bool ShouldCompress()
    {
        var compressLevel = FindLevelToCompress();
        return compressLevel.HasValue;
    }

    /// <summary>
    /// Finds the time level that needs compression based on the number of entries.
    /// </summary>
    /// <returns>A tuple containing the level and entries to compress, or null if no compression is needed.</returns>
    public (IncompleteDate Level, List<TimeEntry> Entries)? FindLevelToCompress()
    {
        var now = DateTime.Now;

        var second = new IncompleteDate(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        if (!HasSummary(second))
        {
            var entries = _timeStorage.Query(_storageKey, second);
            if (entries.Count > 0)
                return (second, entries);
        }

        var minute = new IncompleteDate(now.Year, now.Month, now.Day, now.Hour, now.Minute);
        if (!HasSummary(minute))
        {
            var entries = _timeStorage.Query(_storageKey, minute);
            if (entries.Count > 0)
                return (minute, entries);
        }

        var hour = new IncompleteDate(now.Year, now.Month, now.Day, now.Hour);
        if (!HasSummary(hour))
        {
            var entries = _timeStorage.Query(_storageKey, hour);
            if (entries.Count > 0)
                return (hour, entries);
        }

        var day = new IncompleteDate(now.Year, now.Month, now.Day);
        if (!HasSummary(day))
        {
            var entries = _timeStorage.Query(_storageKey, day);
            if (entries.Count > 0)
                return (day, entries);
        }

        var month = new IncompleteDate(now.Year, now.Month);
        if (!HasSummary(month))
        {
            var entries = _timeStorage.Query(_storageKey, month);
            if (entries.Count > 0)
                return (month, entries);
        }

        var year = new IncompleteDate(now.Year);
        if (!HasSummary(year))
        {
            var entries = _timeStorage.Query(_storageKey, year);
            if (entries.Count > 0)
                return (year, entries);
        }

        for (int y = now.Year - 1; y >= now.Year - 3; y--)
        {
            var pastYear = new IncompleteDate(y);
            if (!HasSummary(pastYear))
            {
                var entries = _timeStorage.Query(_storageKey, pastYear);
                if (entries.Count > 0)
                    return (pastYear, entries);
            }
        }

        return null;
    }

    private bool HasSummary(IncompleteDate level)
    {
        if (level.Second.HasValue)
            return _timeStorage.Exists(_storageKey, level);
        if (level.Minute.HasValue)
        {
            var secondLevel = new IncompleteDate(level.Year, level.Month, level.Day, level.Hour, level.Minute);
            return _timeStorage.Exists(_storageKey, secondLevel);
        }
        if (level.Hour.HasValue)
        {
            var minuteLevel = new IncompleteDate(level.Year, level.Month, level.Day, level.Hour);
            return _timeStorage.Exists(_storageKey, minuteLevel);
        }
        if (level.Day.HasValue)
        {
            var hourLevel = new IncompleteDate(level.Year, level.Month, level.Day);
            return _timeStorage.Exists(_storageKey, hourLevel);
        }
        if (level.Month.HasValue)
        {
            var dayLevel = new IncompleteDate(level.Year, level.Month);
            return _timeStorage.Exists(_storageKey, dayLevel);
        }
        return _timeStorage.Exists(_storageKey, level);
    }

    /// <summary>
    /// Gets the entries available for compression at the current compression level.
    /// </summary>
    /// <param name="count">The maximum number of entries to retrieve.</param>
    /// <returns>A list of time entries available for compression.</returns>
    public List<TimeEntry> GetEntriesForCompression(int count = 10)
    {
        var compressData = FindLevelToCompress();
        if (!compressData.HasValue)
            return new List<TimeEntry>();

        return compressData.Value.Entries.Take(count).ToList();
    }

    /// <summary>
    /// Removes memory entries by their IDs.
    /// </summary>
    /// <param name="entryIds">The list of entry IDs to remove.</param>
    public void RemoveEntries(List<Guid> entryIds)
    {
        _memories.RemoveAll(m => entryIds.Contains(m.Id));
    }

    /// <summary>
    /// Removes memory entries within the specified timestamp range.
    /// </summary>
    /// <param name="range">The timestamp range to delete entries from.</param>
    public void RemoveEntriesByTimestamp(IncompleteDate range)
    {
        _timeStorage.DeleteRange(_storageKey, range);
    }
}
