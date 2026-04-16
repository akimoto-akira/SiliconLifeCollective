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

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = null,
        Converters = { new FlexibleEnumConverter() }
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
        string dir = GetKeyDirectory(key);
        if (!Directory.Exists(dir)) return default;

        T? latest = default;
        DateTime latestTime = DateTime.MinValue;

        foreach (string file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
        {
            if (!TryParseTimestampFromFile(file, out DateTime fileTime)) continue;

            if (fileTime > latestTime)
            {
                latestTime = fileTime;
                latest = ReadSingleLine<T>(file);
            }
        }

        return latest;
    }

    /// <summary>
    /// Writes a record using <see cref="DateTime.UtcNow"/> as the timestamp.
    /// </summary>
    public void Write<T>(string key, T data)
    {
        Write(key, DateTime.UtcNow, data);
    }

    /// <summary>
    /// Returns <c>true</c> if at least one record exists for <paramref name="key"/>.
    /// </summary>
    public bool Exists(string key)
    {
        string dir = GetKeyDirectory(key);
        return Directory.Exists(dir)
            && Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories).Length > 0;
    }

    /// <summary>
    /// Deletes all records for <paramref name="key"/> by removing its directory tree.
    /// </summary>
    public void Delete(string key)
    {
        string dir = GetKeyDirectory(key);
        if (Directory.Exists(dir))
            Directory.Delete(dir, true);
    }

    // ── ITimeStorage ─────────────────────────────────────────────────────────

    /// <summary>
    /// Writes a record at the given <paramref name="timestamp"/>.
    /// The value is serialized as a single-line JSON string.
    /// </summary>
    public void Write<T>(string key, DateTime timestamp, T data)
    {
        string filePath = GetTimeFilePath(key, timestamp);
        EnsureDirectory(filePath);
        WriteSingleLine(filePath, data);
    }

    /// <summary>
    /// Writes a record at the given <see cref="IncompleteDate"/> timestamp.
    /// The timestamp is resolved to the start of the specified range.
    /// Any existing record at the same resolved timestamp is replaced.
    /// </summary>
    public void Write<T>(string key, IncompleteDate timestamp, T data)
    {
        DateTime resolved = ResolveTimestamp(timestamp);
        string filePath = GetTimeFilePath(key, resolved);
        EnsureDirectory(filePath);

        // Clear sibling files in the same directory before writing
        string? dir = Path.GetDirectoryName(filePath);
        if (dir != null && Directory.Exists(dir))
        {
            foreach (string f in Directory.GetFiles(dir, "*.json"))
                File.Delete(f);
        }

        WriteSingleLine(filePath, data);
    }

    /// <summary>
    /// Returns the record whose timestamp is closest to <paramref name="timestamp"/>.
    /// </summary>
    public T? Read<T>(string key, DateTime timestamp)
    {
        string dir = GetKeyDirectory(key);
        if (!Directory.Exists(dir)) return default;

        T? best = default;
        long bestDiff = long.MaxValue;

        foreach (string file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
        {
            if (!TryParseTimestampFromFile(file, out DateTime fileTime)) continue;

            long diff = Math.Abs((fileTime - timestamp).Ticks);
            if (diff < bestDiff)
            {
                bestDiff = diff;
                best = ReadSingleLine<T>(file);
            }
        }

        return best;
    }

    /// <summary>
    /// Returns the first record whose timestamp matches the given <see cref="IncompleteDate"/>.
    /// </summary>
    public T? Read<T>(string key, IncompleteDate timestamp)
    {
        string dir = GetKeyDirectory(key);
        string? patternDir = Path.GetDirectoryName(GetTimeFilePath(key, timestamp));

        if (patternDir == null || !Directory.Exists(patternDir)) return default;

        string[] files = Directory.GetFiles(patternDir, "*.json");
        return files.Length > 0 ? ReadSingleLine<T>(files[0]) : default;
    }

    public bool Exists(string key, DateTime timestamp)
        => Read<object>(key, timestamp) != null;

    public bool Exists(string key, IncompleteDate timestamp)
        => Read<object>(key, timestamp) != null;

    /// <summary>
    /// Deletes the record at the exact <paramref name="timestamp"/> (second precision).
    /// </summary>
    public void Delete(string key, DateTime timestamp)
    {
        string filePath = GetTimeFilePath(key, timestamp);
        if (File.Exists(filePath))
            File.Delete(filePath);

        CleanEmptyDirs(GetKeyDirectory(key));
    }

    /// <summary>
    /// Deletes all records whose timestamp matches the given <see cref="IncompleteDate"/>.
    /// </summary>
    public void Delete(string key, IncompleteDate timestamp)
    {
        string dir = GetKeyDirectory(key);
        string? patternDir = Path.GetDirectoryName(GetTimeFilePath(key, timestamp));

        if (patternDir == null || !Directory.Exists(patternDir)) return;

        foreach (string f in Directory.GetFiles(patternDir, "*.json"))
            File.Delete(f);

        CleanEmptyDirs(dir);
    }

    /// <summary>
    /// Returns all records for <paramref name="key"/> that fall within
    /// <paramref name="range"/>, sorted by timestamp ascending.
    /// </summary>
    public List<TimeEntry<T>> Query<T>(string key, IncompleteDate range)
    {
        var result = new List<TimeEntry<T>>();
        string dir = GetKeyDirectory(key);
        if (!Directory.Exists(dir)) return result;

        foreach (string file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
        {
            if (!TryParseTimestampFromFile(file, out DateTime fileTime)) continue;
            if (!range.Matches(fileTime)) continue;

            foreach (T data in ReadAllLines<T>(file))
                result.Add(new TimeEntry<T>(key, fileTime, data));
        }

        result.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
        return result;
    }

    /// <summary>
    /// Returns all records across every key that fall within <paramref name="range"/>,
    /// sorted by timestamp ascending.
    /// </summary>
    public List<TimeEntry<T>> Query<T>(IncompleteDate range)
    {
        var result = new List<TimeEntry<T>>();
        if (!Directory.Exists(_baseDirectory)) return result;

        foreach (string keyDir in Directory.GetDirectories(_baseDirectory))
        {
            string key = Path.GetFileName(keyDir);

            foreach (string file in Directory.GetFiles(keyDir, "*.json", SearchOption.AllDirectories))
            {
                if (!TryParseTimestampFromFile(file, out DateTime fileTime)) continue;
                if (!range.Matches(fileTime)) continue;

                foreach (T data in ReadAllLines<T>(file))
                    result.Add(new TimeEntry<T>(key, fileTime, data));
            }
        }

        result.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
        return result;
    }

    public int Count(string key, IncompleteDate range)
        => Query<object>(key, range).Count;

    public int Count(IncompleteDate range)
        => Query<object>(range).Count;

    /// <summary>
    /// Deletes all records for <paramref name="key"/> that fall within
    /// <paramref name="range"/> and returns the number of deleted records.
    /// </summary>
    public int DeleteRange(string key, IncompleteDate range)
    {
        string dir = GetKeyDirectory(key);
        if (!Directory.Exists(dir)) return 0;

        int deleted = 0;

        foreach (string file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
        {
            if (!TryParseTimestampFromFile(file, out DateTime fileTime)) continue;
            if (!range.Matches(fileTime)) continue;

            // Count every record line in the file, then delete the whole file
            deleted += File.ReadLines(file).Count(l => !string.IsNullOrWhiteSpace(l));
            File.Delete(file);
        }

        if (deleted > 0)
            CleanEmptyDirs(dir);

        return deleted;
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
                catch (JsonException) { /* malformed line – skip */ }
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
        // Second is the filename component — return the directory only
        // (callers use GetFiles("*.json") on the returned path's directory)
        if (timestamp.Second.HasValue)
            parts.Add($"{timestamp.Second.Value:D2}.json");

        return Path.Combine(parts.ToArray());
    }

    private static void EnsureDirectory(string filePath)
    {
        string? dir = Path.GetDirectoryName(filePath);
        if (dir != null && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }

    /// <summary>
    /// Extracts the <see cref="DateTime"/> encoded in a time-stamped file path.
    /// Expected path format: <c>.../{yyyy}/{MM}/{dd}/{HH}/{mm}/{ss}.json</c>
    /// </summary>
    private static bool TryParseTimestampFromFile(string filePath, out DateTime result)
    {
        result = default;
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        if (!int.TryParse(fileName, out int second) || second is < 0 or > 59)
            return false;

        string? dir = Path.GetDirectoryName(filePath);
        if (dir == null) return false;

        // Walk up 5 directory levels: mm → HH → dd → MM → yyyy
        if (!TryIntSegment(dir, 0, out int minute) || minute is < 0 or > 59) return false;
        if (!TryIntSegment(dir, 1, out int hour)   || hour   is < 0 or > 23) return false;
        if (!TryIntSegment(dir, 2, out int day)    || day    is < 1 or > 31) return false;
        if (!TryIntSegment(dir, 3, out int month)  || month  is < 1 or > 12) return false;
        if (!TryIntSegment(dir, 4, out int year)) return false;

        try { result = new DateTime(year, month, day, hour, minute, second); return true; }
        catch { return false; }
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
}
