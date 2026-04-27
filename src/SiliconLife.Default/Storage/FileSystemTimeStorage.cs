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
using SiliconLife.Collective;
using SiliconLife.Default.Storage;

namespace SiliconLife.Default;

/// <summary>
/// File system implementation of <see cref="ITimeStorage"/>.
/// Time-indexed entries are stored as individual files organized by
/// <c>{key}/{yyyy}/{MM}/{dd}/{HH}/{mm}/{ss}.json</c>.
/// Each file contains exactly one record serialized as a single-line JSON string.
/// </summary>
public class FileSystemTimeStorage : ITimeStorage
{
    private readonly string _baseDirectory;
    private readonly ReaderWriterLockSlim _rwLock = new();

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = null,
        Converters = { new FlexibleEnumConverter(), new IncompleteDateJsonConverter() }
    };

    public FileSystemTimeStorage(string baseDirectory)
    {
        _baseDirectory = baseDirectory;
        if (!Directory.Exists(_baseDirectory))
            Directory.CreateDirectory(_baseDirectory);
    }

    // ── IStorage ────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns the most recently timestamped entry for <paramref name="key"/>,
    /// or <c>default</c> if the key does not exist.
    /// </summary>
    public T? Read<T>(string key)
    {
        _rwLock.EnterReadLock();
        try
        {
            string dir = GetKeyDirectory(key);
            if (!Directory.Exists(dir)) return default;

            T? latest = default;
            IncompleteDate latestTime = new IncompleteDate(1);

            foreach (string file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
            {
                if (!TryParseTimestampFromFile(dir, file, out IncompleteDate fileTime)) continue;

                if (fileTime > latestTime)
                {
                    latestTime = fileTime;
                    latest = ReadSingleLine<T>(file);
                }
            }

            return latest;
        }
        finally
        {
            _rwLock.ExitReadLock();
        }
    }

    /// <summary>
    /// Writes a record using <see cref="DateTime.UtcNow"/> as the timestamp.
    /// </summary>
    public void Write<T>(string key, T data)
    {
        var now = DateTime.UtcNow;
        Write(key, new IncompleteDate(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second), data);
    }

    /// <summary>
    /// Returns <c>true</c> if at least one record exists for <paramref name="key"/>.
    /// </summary>
    public bool Exists(string key)
    {
        _rwLock.EnterReadLock();
        try
        {
            string dir = GetKeyDirectory(key);
            return Directory.Exists(dir)
                   && Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories).Length > 0;
        }
        finally
        {
            _rwLock.ExitReadLock();
        }
    }

    /// <summary>
    /// Deletes all records for <paramref name="key"/> by removing its directory tree.
    /// </summary>
    public void Delete(string key)
    {
        _rwLock.EnterWriteLock();
        try
        {
            string dir = GetKeyDirectory(key);
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
        }
        finally
        {
            _rwLock.ExitWriteLock();
        }
    }

    // ── ITimeStorage ─────────────────────────────────────────────────────────

    /// <summary>
    /// Writes a record at the given <paramref name="timestamp"/>.
    /// The value is serialized as a single-line JSON string.
    /// </summary>
    /// <summary>
    /// Writes a record at the given <see cref="IncompleteDate"/> timestamp.
    /// The timestamp is resolved to the start of the specified range.
    /// Any existing record at the same resolved timestamp is replaced.
    /// </summary>
    public void Write<T>(string key, IncompleteDate timestamp, T data)
    {
        _rwLock.EnterWriteLock();
        try
        {
            string filePath = GetTimeFilePath(key, timestamp);
            EnsureDirectory(filePath);
            WriteSingleLine(filePath, data);
        }
        finally
        {
            _rwLock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Returns the record whose timestamp matches the given <see cref="IncompleteDate"/>.
    /// If the IncompleteDate specifies all components down to second, reads that exact file.
    /// Otherwise, searches recursively in the directory represented by the IncompleteDate and returns the first match.
    /// </summary>
    public T? Read<T>(string key, IncompleteDate timestamp)
    {
        _rwLock.EnterReadLock();
        try
        {
            string filePath = GetTimeFilePath(key, timestamp);

            // If all components are specified (down to second), read the exact file
            if (timestamp.Second.HasValue)
            {
                return File.Exists(filePath) ? ReadSingleLine<T>(filePath) : default;
            }

            // Otherwise, search recursively in the directory represented by the IncompleteDate
            string? dirPath = Path.GetDirectoryName(filePath);
            if (dirPath == null || !Directory.Exists(dirPath)) return default;

            string[] files = Directory.GetFiles(dirPath, "*.json", SearchOption.AllDirectories);
            return files.Length > 0 ? ReadSingleLine<T>(files[0]) : default;
        }
        finally
        {
            _rwLock.ExitReadLock();
        }
    }

    public bool Exists(string key, IncompleteDate timestamp)
        => Read<object>(key, timestamp) != null;

    /// <summary>
    /// Deletes all records whose timestamp matches the given <see cref="IncompleteDate"/>.
    /// </summary>
    public void Delete(string key, IncompleteDate timestamp)
    {
        _rwLock.EnterWriteLock();
        try
        {
            string dir = GetKeyDirectory(key);
            string? patternDir = Path.GetDirectoryName(GetTimeFilePath(key, timestamp));

            if (patternDir == null || !Directory.Exists(patternDir)) return;

            foreach (string f in Directory.GetFiles(patternDir, "*.json"))
                File.Delete(f);

            CleanEmptyDirs(dir);
        }
        finally
        {
            _rwLock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Returns all records for <paramref name="key"/> that fall within
    /// <paramref name="range"/>, sorted by timestamp ascending.
    /// </summary>
    public List<TimeEntry<T>> Query<T>(string key, IncompleteDate? range)
    {
        _rwLock.EnterReadLock();
        try
        {
            var result = new List<TimeEntry<T>>();
            string dir = GetKeyDirectory(key);
            if (!Directory.Exists(dir)) return result;

            foreach (string file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
            {
                if (!TryParseTimestampFromFile(dir, file, out IncompleteDate fileTime)) continue;
                if (range.HasValue && !range.Value.Matches(
                        new DateTime(fileTime.Year, fileTime.Month ?? 1, fileTime.Day ?? 1,
                            fileTime.Hour ?? 0, fileTime.Minute ?? 0, fileTime.Second ?? 0))) continue;

                foreach (T data in ReadAllLines<T>(file))
                    result.Add(new TimeEntry<T>(key, fileTime, data));
            }

            result.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
            return result;
        }
        finally
        {
            _rwLock.ExitReadLock();
        }
    }

    /// <summary>
    /// Returns all records across every key that fall within <paramref name="range"/>,
    /// sorted by timestamp ascending. If <paramref name="range"/> is null, returns all records.
    /// </summary>
    public List<TimeEntry<T>> Query<T>(IncompleteDate? range)
    {
        _rwLock.EnterReadLock();
        try
        {
            var result = new List<TimeEntry<T>>();
            if (!Directory.Exists(_baseDirectory)) return result;

            foreach (string keyDir in Directory.GetDirectories(_baseDirectory))
            {
                string key = Path.GetFileName(keyDir);

                foreach (string file in Directory.GetFiles(keyDir, "*.json", SearchOption.AllDirectories))
                {
                    if (!TryParseTimestampFromFile(keyDir, file, out IncompleteDate fileTime)) continue;
                    if (range.HasValue && !range.Value.Matches(
                            new DateTime(fileTime.Year, fileTime.Month ?? 1, fileTime.Day ?? 1,
                                fileTime.Hour ?? 0, fileTime.Minute ?? 0, fileTime.Second ?? 0))) continue;

                    foreach (T data in ReadAllLines<T>(file))
                        result.Add(new TimeEntry<T>(key, fileTime, data));
                }
            }

            result.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
            return result;
        }
        finally
        {
            _rwLock.ExitReadLock();
        }
    }

    public int Count(string key, IncompleteDate range)
        => Query<object>(key, range).Count;

    public int Count(IncompleteDate range)
        => Query<object>(range).Count;

    /// <summary>
    /// Queries all entries at the next finer time level under the given IncompleteDate.
    /// Uses IncompleteDate.Expand() to get all possible values at the next level.
    /// </summary>
    public List<TimeEntry<T>> QueryWithLevel<T>(string key, IncompleteDate level)
    {
        _rwLock.EnterReadLock();
        try
        {
            var result = new List<TimeEntry<T>>();

            // Expand to the next finer level
            var expanded = level.Expand();

            // Read each expanded timestamp
            foreach (var timestamp in expanded)
            {
                string filePath = GetTimeFilePath(key, timestamp);

                if (!File.Exists(filePath)) continue;

                // Parse the actual timestamp from file path for accurate TimeEntry
                if (!TryParseTimestampFromFile(GetKeyDirectory(key), filePath, out IncompleteDate fileTime)) continue;

                foreach (T data in ReadAllLines<T>(filePath))
                    result.Add(new TimeEntry<T>(key, fileTime, data));
            }

            result.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
            return result;
        }
        finally
        {
            _rwLock.ExitReadLock();
        }
    }

    /// <summary>
    /// Deletes all records for <paramref name="key"/> that fall within
    /// <paramref name="range"/> and returns the number of deleted records.
    /// </summary>
    public int DeleteRange(string key, IncompleteDate range)
    {
        _rwLock.EnterWriteLock();
        try
        {
            string dir = GetKeyDirectory(key);
            if (!Directory.Exists(dir)) return 0;

            int deleted = 0;

            foreach (string file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
            {
                if (!TryParseTimestampFromFile(dir, file, out IncompleteDate fileTime)) continue;
                if (!range.Matches(
                        new DateTime(fileTime.Year, fileTime.Month ?? 1, fileTime.Day ?? 1,
                            fileTime.Hour ?? 0, fileTime.Minute ?? 0, fileTime.Second ?? 0))) continue;

                // Count every record line in the file, then delete the whole file
                deleted += File.ReadLines(file).Count(l => !string.IsNullOrWhiteSpace(l));
                File.Delete(file);
            }

            if (deleted > 0)
                CleanEmptyDirs(dir);

            return deleted;
        }
        finally
        {
            _rwLock.ExitWriteLock();
        }
    }

    // ── File I/O helpers ─────────────────────────────────────────────────────

    /// <summary>
    /// Serializes <paramref name="data"/> as a single-line JSON string and writes it
    /// to <paramref name="filePath"/>. If the file already contains a line whose
    /// <c>Id</c> field matches the one on <paramref name="data"/>, that line is
    /// replaced in-place; otherwise the new line is appended at the end.
    /// </summary>
    private static void WriteSingleLine<T>(string filePath, T data)
    {
        string newLine = JsonSerializer.Serialize(data, _jsonOptions);

        // Try to extract the Id value via reflection so we can do upsert logic.
        object? newId = typeof(T).GetProperty("Id")?.GetValue(data);

        if (newId != null && File.Exists(filePath))
        {
            string[] existingLines = File.ReadAllLines(filePath);
            bool replaced = false;

            for (int i = 0; i < existingLines.Length; i++)
            {
                string existing = existingLines[i];
                if (string.IsNullOrWhiteSpace(existing)) continue;

                try
                {
                    using JsonDocument doc = JsonDocument.Parse(existing);
                    if (doc.RootElement.TryGetProperty("Id", out JsonElement idEl) &&
                        idEl.ToString() == newId.ToString())
                    {
                        existingLines[i] = newLine;
                        replaced = true;
                        break;
                    }
                }
                catch (JsonException)
                {
                    /* malformed line – skip */
                }
            }

            if (replaced)
            {
                File.WriteAllLines(filePath, existingLines);
                return;
            }
        }

        // No matching Id found (or no Id property) – append as a new line.
        EnsureDirectory(filePath);
        File.AppendAllText(filePath, newLine + Environment.NewLine);
    }

    /// <summary>
    /// Reads the last non-empty line from <paramref name="filePath"/> and
    /// deserializes it to <typeparamref name="T"/>.
    /// When multiple records share the same timestamp file the most recently
    /// appended record is returned.
    /// </summary>
    private static T? ReadSingleLine<T>(string filePath)
    {
        string? lastLine = null;

        foreach (string line in File.ReadLines(filePath))
        {
            if (!string.IsNullOrWhiteSpace(line))
                lastLine = line;
        }

        return lastLine != null
            ? JsonSerializer.Deserialize<T>(lastLine, _jsonOptions)
            : default;
    }

    /// <summary>
    /// Reads all non-empty lines from <paramref name="filePath"/> and deserializes
    /// each to <typeparamref name="T"/>. Used by query methods to return every record
    /// stored at the same timestamp.
    /// </summary>
    private static IEnumerable<T> ReadAllLines<T>(string filePath)
    {
        foreach (string line in File.ReadLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            T? item = JsonSerializer.Deserialize<T>(line, _jsonOptions);
            if (item != null) yield return item;
        }
    }

    // ── Path helpers ─────────────────────────────────────────────────────────

    private string GetKeyDirectory(string key)
    {
        string safeKey = key.Replace("..", string.Empty);
        return Path.GetFullPath(Path.Combine(_baseDirectory, safeKey));
    }

    private string GetTimeFilePath(string key, DateTime timestamp)
    {
        return Path.Combine(
            GetKeyDirectory(key),
            timestamp.Year.ToString(),
            timestamp.Month.ToString("D2"),
            timestamp.Day.ToString("D2"),
            timestamp.Hour.ToString("D2"),
            timestamp.Minute.ToString("D2"),
            $"{timestamp.Second:D2}.json");
    }

    /// <summary>
    /// Builds the deepest directory path for an <see cref="IncompleteDate"/>.
    /// Unspecified components are omitted, so the path represents the most specific
    /// directory that can be derived from the given timestamp.
    /// </summary>
    private string GetTimeFilePath(string key, IncompleteDate timestamp)
    {
        var parts = new List<string> { GetKeyDirectory(key) };

        if (timestamp.Year > 0) parts.Add(timestamp.Year.ToString());
        if (timestamp.Month.HasValue) parts.Add(timestamp.Month.Value.ToString("D2"));
        if (timestamp.Day.HasValue) parts.Add(timestamp.Day.Value.ToString("D2"));
        if (timestamp.Hour.HasValue) parts.Add(timestamp.Hour.Value.ToString("D2"));
        if (timestamp.Minute.HasValue) parts.Add(timestamp.Minute.Value.ToString("D2"));
        if (timestamp.Second.HasValue) parts.Add(timestamp.Second.Value.ToString("D2"));

        return Path.Combine(parts.ToArray()) + ".json";
    }

    private static void EnsureDirectory(string filePath)
    {
        string? dir = Path.GetDirectoryName(filePath);
        if (dir != null && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }

    /// <summary>
    /// Extracts the <see cref="IncompleteDate"/> encoded in a time-stamped file path.
    /// Expected path format: <c>{_baseDirectory}/{key}/{yyyy}/{MM}/{dd}/{HH}/{mm}/{ss}.json</c>
    /// The precision of the IncompleteDate is determined by the depth of the path relative to the key directory.
    /// </summary>
    private bool TryParseTimestampFromFile(string keyDir, string filePath, out IncompleteDate result)
    {
        result = default;
        // Get relative path from base directory
        string? relativePath = Path.GetRelativePath(keyDir, filePath);
        int a = relativePath.LastIndexOf(".", StringComparison.Ordinal);
        relativePath = relativePath.Substring(0, a);
        if (string.IsNullOrEmpty(relativePath)) return false;

        // Split into segments: [key, yyyy, MM, dd, HH, mm, ss.json]
        string[] segments = relativePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        // Need at least key + year + second (4 segments minimum)
        if (segments.Length < 1) return false;

        // segments[0] = key, segments[1] = year, etc.
        if (!int.TryParse(segments[0], out int year)) return false;

        int? month = null, day = null, hour = null, minute = null, second = null;

        if (segments.Length >= 2 && int.TryParse(segments[1], out int m) && m >= 1 && m <= 12)
            month = m;
        if (segments.Length >= 3 && int.TryParse(segments[2], out int d) && d >= 1 && d <= 31)
            day = d;
        if (segments.Length >= 4 && int.TryParse(segments[3], out int h) && h >= 0 && h <= 23)
            hour = h;
        if (segments.Length >= 5 && int.TryParse(segments[4], out int min) && min >= 0 && min <= 59)
            minute = min;
        if (segments.Length >= 6 && int.TryParse(segments[5], out int s) && s >= 0 && s <= 59)
            second = s;

        try
        {
            result = new IncompleteDate(year, month, day, hour, minute, second);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Returns the directory-name segment at <paramref name="depth"/> levels above
    /// <paramref name="path"/> as an integer.
    /// Depth 0 = the last segment of <paramref name="path"/> itself.
    /// </summary>
    private static bool TryIntSegment(string path, int depth, out int value)
    {
        value = 0;
        for (int i = 0; i < depth; i++)
        {
            string? parent = Path.GetDirectoryName(path);
            if (parent == null) return false;
            path = parent;
        }

        return int.TryParse(Path.GetFileName(path), out value);
    }

    /// <summary>
    /// Resolves an <see cref="IncompleteDate"/> to a concrete <see cref="DateTime"/>
    /// by filling unspecified components with their minimum values.
    /// </summary>
    private static DateTime ResolveTimestamp(IncompleteDate d)
        => new(d.Year, d.Month ?? 1, d.Day ?? 1,
            d.Hour ?? 0, d.Minute ?? 0, d.Second ?? 0);

    private static void CleanEmptyDirs(string rootDir)
    {
        if (!Directory.Exists(rootDir)) return;
        foreach (string dir in Directory.GetDirectories(rootDir))
        {
            CleanEmptyDirs(dir);
            if (!Directory.EnumerateFileSystemEntries(dir).Any())
                Directory.Delete(dir);
        }
    }

    // ── Search Implementation ────────────────────────────────────────────────

    /// <summary>
    /// Checks if any entry at the given IncompleteDate location has a summary flag.
    /// The IncompleteDate represents a specific file path, not a range.
    /// </summary>
    public bool HasSummary<T>(string key, IncompleteDate timestamp, Func<T, bool> summaryPropertySelector)
    {
        _rwLock.EnterReadLock();
        try
        {
            string filePath = GetTimeFilePath(key, timestamp);

            if (!File.Exists(filePath)) return false;

            // Read all entries in the file and check if any is a summary
            foreach (var entry in ReadAllLines<T>(filePath))
            {
                if (summaryPropertySelector(entry))
                    return true;
            }

            return false;
        }
        finally
        {
            _rwLock.ExitReadLock();
        }
    }

    /// <summary>
    /// Searches entries by keyword across all time levels for the given key.
    /// </summary>
    public List<TimeEntry<T>> Search<T>(string key, string keyword, int maxCount = 0)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return new List<TimeEntry<T>>();

        _rwLock.EnterReadLock();
        try
        {
            string dir = GetKeyDirectory(key);
            if (!Directory.Exists(dir))
                return new List<TimeEntry<T>>();

            var results = new List<TimeEntry<T>>();
            var keywordLower = keyword.ToLowerInvariant();

            foreach (string file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
            {
                if (!TryParseTimestampFromFile(dir, file, out IncompleteDate fileTime))
                    continue;

                try
                {
                    var entry = ReadSingleLine<T>(file);
                    if (entry == null)
                        continue;

                    // Search in Content and Keywords fields using reflection
                    bool found = false;
                    var entryType = typeof(T);

                    // Check Content property
                    var contentProp = entryType.GetProperty("Content");
                    if (contentProp != null)
                    {
                        var content = contentProp.GetValue(entry) as string;
                        if (!string.IsNullOrEmpty(content) && content.ToLowerInvariant().Contains(keywordLower))
                            found = true;
                    }

                    // Check Keywords property
                    if (!found)
                    {
                        var keywordsProp = entryType.GetProperty("Keywords");
                        if (keywordsProp != null)
                        {
                            var keywords = keywordsProp.GetValue(entry) as System.Collections.IEnumerable;
                            if (keywords != null)
                            {
                                foreach (var kw in keywords)
                                {
                                    var kwStr = kw?.ToString();
                                    if (!string.IsNullOrEmpty(kwStr) && kwStr.ToLowerInvariant().Contains(keywordLower))
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (found)
                    {
                        results.Add(new TimeEntry<T>(key, fileTime, entry));
                    }
                }
                catch
                {
                    // Skip files that cannot be read or deserialized
                }
            }

            // Order by timestamp descending and apply limit
            var ordered = results.OrderByDescending(e => e.Timestamp);
            return (maxCount > 0 ? ordered.Take(maxCount) : ordered).ToList();
        }
        finally
        {
            _rwLock.ExitReadLock();
        }
    }

    // ── Earliest / Latest Timestamp ──────────────────────────────────────────

    /// <summary>
    /// Gets the earliest recorded timestamp for the given key by walking the
    /// deepest timestamp directory chain, picking the smallest numeric segment
    /// at each level.
    /// </summary>
    public IncompleteDate? GetEarliestTimestamp(string key)
    {
        _rwLock.EnterReadLock();
        try
        {
            return FindExtremeTimestamp(GetKeyDirectory(key), pickMax: false);
        }
        finally
        {
            _rwLock.ExitReadLock();
        }
    }

    /// <summary>
    /// Gets the latest recorded timestamp for the given key by walking the
    /// deepest timestamp directory chain, picking the largest numeric segment
    /// at each level.
    /// </summary>
    public IncompleteDate? GetLatestTimestamp(string key)
    {
        _rwLock.EnterReadLock();
        try
        {
            return FindExtremeTimestamp(GetKeyDirectory(key), pickMax: true);
        }
        finally
        {
            _rwLock.ExitReadLock();
        }
    }

    /// <summary>
    /// Gets the earliest recorded timestamp across every key under the base directory.
    /// </summary>
    public IncompleteDate? GetEarliestTimestamp()
    {
        _rwLock.EnterReadLock();
        try
        {
            return ScanAllKeysForExtremeTimestamp(pickMax: false);
        }
        finally
        {
            _rwLock.ExitReadLock();
        }
    }

    /// <summary>
    /// Gets the latest recorded timestamp across every key under the base directory.
    /// </summary>
    public IncompleteDate? GetLatestTimestamp()
    {
        _rwLock.EnterReadLock();
        try
        {
            return ScanAllKeysForExtremeTimestamp(pickMax: true);
        }
        finally
        {
            _rwLock.ExitReadLock();
        }
    }

    private IncompleteDate? ScanAllKeysForExtremeTimestamp(bool pickMax)
    {
        if (!Directory.Exists(_baseDirectory)) return null;

        IncompleteDate? extreme = null;
        foreach (string keyDir in Directory.GetDirectories(_baseDirectory))
        {
            IncompleteDate? candidate = FindExtremeTimestamp(keyDir, pickMax);
            if (candidate == null) continue;

            if (extreme == null ||
                (pickMax && candidate.Value > extreme.Value) ||
                (!pickMax && candidate.Value < extreme.Value))
            {
                extreme = candidate;
            }
        }

        return extreme;
    }

    /// <summary>
    /// Walks the {yyyy}/{MM}/{dd}/{HH}/{mm}/{ss}.json directory layout under
    /// <paramref name="keyDir"/>, picking the minimum or maximum numeric child
    /// at each level. Falls back to the next candidate when a branch is empty,
    /// so dangling directories do not break the traversal.
    /// </summary>
    private static IncompleteDate? FindExtremeTimestamp(string keyDir, bool pickMax)
    {
        if (!Directory.Exists(keyDir)) return null;

        // Candidate lists pre-sorted so that "next candidate" on fallback is trivial
        List<(int value, string dir)> years = EnumerateIntChildren(keyDir, pickMax);
        foreach (var (year, yearDir) in years)
        {
            foreach (var (month, monthDir) in EnumerateIntChildren(yearDir, pickMax))
            {
                foreach (var (day, dayDir) in EnumerateIntChildren(monthDir, pickMax))
                {
                    foreach (var (hour, hourDir) in EnumerateIntChildren(dayDir, pickMax))
                    {
                        foreach (var (minute, minuteDir) in EnumerateIntChildren(hourDir, pickMax))
                        {
                            int? second = PickExtremeSecondFile(minuteDir, pickMax);
                            if (second == null) continue;

                            try
                            {
                                return new IncompleteDate(year, month, day, hour, minute, second.Value);
                            }
                            catch
                            {
                                // Malformed segment combination – skip
                            }
                        }
                    }
                }
            }
        }

        return null;
    }

    private static List<(int value, string dir)> EnumerateIntChildren(string dir, bool pickMax)
    {
        if (!Directory.Exists(dir)) return new List<(int, string)>();

        var items = new List<(int value, string dir)>();
        foreach (string child in Directory.GetDirectories(dir))
        {
            if (int.TryParse(Path.GetFileName(child), out int value))
                items.Add((value, child));
        }

        items.Sort((a, b) => pickMax
            ? b.value.CompareTo(a.value)
            : a.value.CompareTo(b.value));
        return items;
    }

    private static int? PickExtremeSecondFile(string dir, bool pickMax)
    {
        if (!Directory.Exists(dir)) return null;

        int? extreme = null;
        foreach (string file in Directory.GetFiles(dir, "*.json"))
        {
            if (!int.TryParse(Path.GetFileNameWithoutExtension(file), out int second)) continue;
            if (second is < 0 or > 59) continue;

            if (extreme == null ||
                (pickMax && second > extreme.Value) ||
                (!pickMax && second < extreme.Value))
            {
                extreme = second;
            }
        }

        return extreme;
    }
}