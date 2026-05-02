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

using SiliconLife.Collective;
using SiliconLife.Speedy;

namespace SiliconLife.Fast;

/// <summary>
/// <see cref="ITimeStorage"/> adapter backed by a <see cref="SpeedyPack"/> (.spk) file.
/// Time entries are stored at paths: <c>{key}/{yyyy}/{MM}/{dd}/{HH}/{mm}/{ss}.json</c>.
/// Each path stores a JSON array (<c>List&lt;T&gt;</c>) to support multiple records per timestamp.
/// </summary>
/// <remarks>
/// The underlying <see cref="SpeedyPack"/> is the single instance owned by
/// <see cref="SpeedyPackRegistry"/>. Disposing this wrapper does <em>not</em> close
/// the pack — call <see cref="SpeedyPackRegistry.Dispose"/> during application shutdown.
/// </remarks>
public sealed class SpeedyTimeStorage : ITimeStorage, IDisposable
{
    private readonly SpeedyPack _pack;
    private readonly string _keyPrefix;

    /// <summary>
    /// Wraps the single shared <see cref="SpeedyPack"/> from
    /// <see cref="SpeedyPackRegistry"/> as an <see cref="ITimeStorage"/> implementation.
    /// </summary>
    /// <param name="dir">Optional directory path used to extract a GUID-based key prefix.</param>
    public SpeedyTimeStorage(string dir = "")
    {
        _pack = SpeedyPackRegistry.Pack;
        _keyPrefix = ExtractKeyPrefix(dir);
    }

    // ─── Key prefix ───────────────────────────────────────────────────────────

    private static string ExtractKeyPrefix(string dir)
    {
        if (string.IsNullOrWhiteSpace(dir)) return "";
        
        // Extract relative path from full directory path
        // e.g., "d:\data\SiliconManager\{GUID}" → "SiliconManager/{GUID}"
        string currentDir = Environment.CurrentDirectory;
        string relativePath = dir;
        
        if (relativePath.StartsWith(currentDir, StringComparison.OrdinalIgnoreCase))
        {
            relativePath = relativePath.Substring(currentDir.Length).TrimStart('\\', '/');
        }
        
        // Normalize path separators to forward slashes
        relativePath = relativePath.Replace('\\', '/').TrimEnd('/');
        
        return string.IsNullOrEmpty(relativePath) ? "" : relativePath + "/";
    }

    private string PrefixKey(string key) => _keyPrefix + key;

    // ─── Path mapping ─────────────────────────────────────────────────────────

    private string GetTimeFilePath(string key, IncompleteDate timestamp)
    {
        string safeKey = PrefixKey(key).Replace("..", string.Empty);
        var parts = new List<string> { safeKey };

        parts.Add(timestamp.Year.ToString());
        if (timestamp.Month.HasValue)  parts.Add(timestamp.Month.Value.ToString("D2"));
        if (timestamp.Day.HasValue)    parts.Add(timestamp.Day.Value.ToString("D2"));
        if (timestamp.Hour.HasValue)   parts.Add(timestamp.Hour.Value.ToString("D2"));
        if (timestamp.Minute.HasValue) parts.Add(timestamp.Minute.Value.ToString("D2"));
        if (timestamp.Second.HasValue) parts.Add(timestamp.Second.Value.ToString("D2"));

        string path = string.Join("/", parts);
        if (timestamp.Second.HasValue) path += ".json";
        return path;
    }

    private string GetKeyDirectoryPrefix(string key) =>
        PrefixKey(key).Replace("..", string.Empty).TrimEnd('/') + "/";

    // ─── Timestamp parsing ────────────────────────────────────────────────────

    private bool TryParseTimestampFromPath(string keyDirPrefix, string path, out IncompleteDate result)
    {
        result = default;
        if (!path.StartsWith(keyDirPrefix, StringComparison.OrdinalIgnoreCase)) return false;

        string relative = path[keyDirPrefix.Length..];
        if (relative.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            relative = relative[..^5];
        if (string.IsNullOrEmpty(relative)) return false;

        string[] segments = relative.Split('/');
        if (!int.TryParse(segments[0], out int year)) return false;

        int? month = null, day = null, hour = null, minute = null, second = null;
        if (segments.Length >= 2 && int.TryParse(segments[1], out int m) && m is >= 1 and <= 12)  month  = m;
        if (segments.Length >= 3 && int.TryParse(segments[2], out int d) && d is >= 1 and <= 31)  day    = d;
        if (segments.Length >= 4 && int.TryParse(segments[3], out int h) && h is >= 0 and <= 23)  hour   = h;
        if (segments.Length >= 5 && int.TryParse(segments[4], out int mn) && mn is >= 0 and <= 59) minute = mn;
        if (segments.Length >= 6 && int.TryParse(segments[5], out int s) && s is >= 0 and <= 59)  second = s;

        try { result = new IncompleteDate(year, month, day, hour, minute, second); return true; }
        catch { return false; }
    }

    // ─── Array-based read/write helpers ──────────────────────────────────────

    private List<T> ReadArray<T>(string path) =>
        _pack.Read<List<T>>(path) ?? [];

    private void WriteArray<T>(string path, List<T> list) =>
        _pack.Write(path, list);

    private void UpsertIntoArray<T>(string path, T data)
    {
        var list = ReadArray<T>(path);
        object? newId = typeof(T).GetProperty("Id")?.GetValue(data);
        if (newId != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                object? existingId = typeof(T).GetProperty("Id")?.GetValue(list[i]);
                if (existingId?.ToString() == newId.ToString())
                {
                    list[i] = data;
                    WriteArray(path, list);
                    return;
                }
            }
        }
        list.Add(data);
        WriteArray(path, list);
    }

    // ─── IStorage ─────────────────────────────────────────────────────────────

    public T? Read<T>(string key)
    {
        string keyDirPrefix = GetKeyDirectoryPrefix(key);
        IncompleteDate latestTime = new IncompleteDate(1);
        T? latest = default;
        bool found = false;

        foreach (string entryPath in EnumerateAllEntries(keyDirPrefix))
        {
            if (!TryParseTimestampFromPath(keyDirPrefix, entryPath, out IncompleteDate fileTime)) continue;
            if (!found || fileTime > latestTime)
            {
                var list = ReadArray<T>(entryPath);
                if (list.Count > 0) { latestTime = fileTime; latest = list[^1]; found = true; }
            }
        }
        return latest;
    }

    public void Write<T>(string key, T data)
    {
        var now = DateTime.UtcNow;
        Write(key, new IncompleteDate(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second), data);
    }

    public bool Exists(string key) => EnumerateAllEntries(GetKeyDirectoryPrefix(key)).Any();

    public void Delete(string key)
    {
        foreach (string p in EnumerateAllEntries(GetKeyDirectoryPrefix(key)).ToList())
            _pack.Delete(p);
    }

    // ─── ITimeStorage ─────────────────────────────────────────────────────────

    public void Write<T>(string key, IncompleteDate timestamp, T data) =>
        UpsertIntoArray(GetTimeFilePath(key, timestamp), data);

    public T? Read<T>(string key, IncompleteDate timestamp)
    {
        if (timestamp.Second.HasValue)
        {
            var list = ReadArray<T>(GetTimeFilePath(key, timestamp));
            return list.Count > 0 ? list[^1] : default;
        }
        foreach (string p in EnumerateAllEntriesUnder(GetTimeFilePath(key, timestamp) + "/"))
        {
            var list = ReadArray<T>(p);
            if (list.Count > 0) return list[^1];
        }
        return default;
    }

    public bool Exists(string key, IncompleteDate timestamp) => Read<object>(key, timestamp) != null;

    public void Delete(string key, IncompleteDate timestamp)
    {
        if (timestamp.Second.HasValue) { _pack.Delete(GetTimeFilePath(key, timestamp)); return; }
        foreach (string p in EnumerateAllEntriesUnder(GetTimeFilePath(key, timestamp) + "/").ToList())
            _pack.Delete(p);
    }

    public List<TimeEntry<T>> Query<T>(string key, IncompleteDate? range)
    {
        string keyDirPrefix = GetKeyDirectoryPrefix(key);
        var result = new List<TimeEntry<T>>();
        foreach (string p in EnumerateAllEntries(keyDirPrefix))
        {
            if (!TryParseTimestampFromPath(keyDirPrefix, p, out IncompleteDate ft)) continue;
            if (range.HasValue && !range.Value.Matches(new DateTime(ft.Year, ft.Month ?? 1, ft.Day ?? 1, ft.Hour ?? 0, ft.Minute ?? 0, ft.Second ?? 0))) continue;
            foreach (T data in ReadArray<T>(p)) result.Add(new TimeEntry<T>(key, ft, data));
        }
        result.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
        return result;
    }

    public List<TimeEntry<T>> Query<T>(IncompleteDate? range)
    {
        var result = new List<TimeEntry<T>>();
        string rootPrefix = string.IsNullOrEmpty(_keyPrefix) ? "" : _keyPrefix.TrimEnd('/');
        foreach (string keyDir in _pack.ListDirectories(rootPrefix))
        {
            string keyName = keyDir;
            if (!string.IsNullOrEmpty(_keyPrefix) && keyName.StartsWith(_keyPrefix, StringComparison.OrdinalIgnoreCase))
                keyName = keyName[_keyPrefix.Length..];
            string keyDirPrefix = keyDir.TrimEnd('/') + "/";
            foreach (string p in EnumerateAllEntries(keyDirPrefix))
            {
                if (!TryParseTimestampFromPath(keyDirPrefix, p, out IncompleteDate ft)) continue;
                if (range.HasValue && !range.Value.Matches(new DateTime(ft.Year, ft.Month ?? 1, ft.Day ?? 1, ft.Hour ?? 0, ft.Minute ?? 0, ft.Second ?? 0))) continue;
                foreach (T data in ReadArray<T>(p)) result.Add(new TimeEntry<T>(keyName, ft, data));
            }
        }
        result.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
        return result;
    }

    public List<TimeEntry<T>> QueryLatest<T>(string key, int limit)
    {
        string keyDirPrefix = GetKeyDirectoryPrefix(key);
        var result = new List<TimeEntry<T>>();
        foreach (string p in EnumerateAllEntries(keyDirPrefix))
        {
            if (!TryParseTimestampFromPath(keyDirPrefix, p, out IncompleteDate ft)) continue;
            foreach (T data in ReadArray<T>(p)) result.Add(new TimeEntry<T>(key, ft, data));
        }
        result.Sort((a, b) => b.Timestamp.CompareTo(a.Timestamp));
        return limit > 0 ? result.Take(limit).ToList() : result;
    }

    public int Count(string key, IncompleteDate range) => Query<object>(key, range).Count;
    public int Count(IncompleteDate range) => Query<object>(range).Count;

    public int DeleteRange(string key, IncompleteDate range)
    {
        string keyDirPrefix = GetKeyDirectoryPrefix(key);
        int deleted = 0;
        foreach (string p in EnumerateAllEntries(keyDirPrefix).ToList())
        {
            if (!TryParseTimestampFromPath(keyDirPrefix, p, out IncompleteDate ft)) continue;
            if (!range.Matches(new DateTime(ft.Year, ft.Month ?? 1, ft.Day ?? 1, ft.Hour ?? 0, ft.Minute ?? 0, ft.Second ?? 0))) continue;
            deleted += ReadArray<object>(p).Count;
            _pack.Delete(p);
        }
        return deleted;
    }

    public List<TimeEntry<T>> Search<T>(string key, string keyword, int maxCount = 0)
    {
        if (string.IsNullOrWhiteSpace(keyword)) return [];
        string keyDirPrefix = GetKeyDirectoryPrefix(key);
        var results = new List<TimeEntry<T>>();
        var kw = keyword.ToLowerInvariant();

        foreach (string p in EnumerateAllEntries(keyDirPrefix))
        {
            if (!TryParseTimestampFromPath(keyDirPrefix, p, out IncompleteDate ft)) continue;
            try
            {
                foreach (T entry in ReadArray<T>(p))
                {
                    if (entry == null) continue;
                    var t = typeof(T);
                    bool found = (t.GetProperty("Content")?.GetValue(entry) as string)?.ToLowerInvariant().Contains(kw) ?? false;
                    if (!found)
                    {
                        var kwProp = t.GetProperty("Keywords");
                        if (kwProp?.GetValue(entry) is System.Collections.IEnumerable kwList)
                            foreach (var k in kwList)
                                if (k?.ToString()?.ToLowerInvariant().Contains(kw) == true) { found = true; break; }
                    }
                    if (found) results.Add(new TimeEntry<T>(key, ft, entry));
                }
            }
            catch { }
        }

        var ordered = results.OrderByDescending(e => e.Timestamp);
        return (maxCount > 0 ? ordered.Take(maxCount) : ordered).ToList();
    }

    public IncompleteDate? GetEarliestTimestamp(string key) => FindExtremeTimestamp(GetKeyDirectoryPrefix(key), false);
    public IncompleteDate? GetLatestTimestamp(string key)   => FindExtremeTimestamp(GetKeyDirectoryPrefix(key), true);
    public IncompleteDate? GetEarliestTimestamp() => ScanAllKeysForExtremeTimestamp(false);
    public IncompleteDate? GetLatestTimestamp()   => ScanAllKeysForExtremeTimestamp(true);

    private IncompleteDate? ScanAllKeysForExtremeTimestamp(bool pickMax)
    {
        string rootPrefix = string.IsNullOrEmpty(_keyPrefix) ? "" : _keyPrefix.TrimEnd('/');
        IncompleteDate? extreme = null;
        foreach (string keyDir in _pack.ListDirectories(rootPrefix))
        {
            var candidate = FindExtremeTimestamp(keyDir.TrimEnd('/') + "/", pickMax);
            if (candidate == null) continue;
            if (extreme == null || (pickMax ? candidate.Value > extreme.Value : candidate.Value < extreme.Value))
                extreme = candidate;
        }
        return extreme;
    }

    private IncompleteDate? FindExtremeTimestamp(string keyDirPrefix, bool pickMax)
    {
        string root = keyDirPrefix.TrimEnd('/');
        foreach (var (year, yearDir) in EnumerateIntDirectories(root, pickMax))
        foreach (var (month, monthDir) in EnumerateIntDirectories(yearDir, pickMax))
        foreach (var (day, dayDir) in EnumerateIntDirectories(monthDir, pickMax))
        foreach (var (hour, hourDir) in EnumerateIntDirectories(dayDir, pickMax))
        foreach (var (minute, minuteDir) in EnumerateIntDirectories(hourDir, pickMax))
        {
            int? second = PickExtremeSecondEntry(minuteDir, pickMax);
            if (second == null) continue;
            try { return new IncompleteDate(year, month, day, hour, minute, second.Value); } catch { }
        }
        return null;
    }

    private List<(int value, string dir)> EnumerateIntDirectories(string dirPath, bool pickMax)
    {
        var items = new List<(int, string)>();
        foreach (string child in _pack.ListDirectories(dirPath))
        {
            string name = child.TrimEnd('/');
            string segment = name[(name.LastIndexOf('/') + 1)..];
            if (int.TryParse(segment, out int value))
                items.Add((value, name));
        }
        items.Sort((a, b) => pickMax ? b.Item1.CompareTo(a.Item1) : a.Item1.CompareTo(b.Item1));
        return items;
    }

    private int? PickExtremeSecondEntry(string minuteDir, bool pickMax)
    {
        int? extreme = null;
        foreach (string entry in _pack.ListEntries(minuteDir))
        {
            string name = entry[(entry.LastIndexOf('/') + 1)..];
            if (name.EndsWith(".json", StringComparison.OrdinalIgnoreCase)) name = name[..^5];
            if (!int.TryParse(name, out int s) || s is < 0 or > 59) continue;
            if (extreme == null || (pickMax ? s > extreme.Value : s < extreme.Value)) extreme = s;
        }
        return extreme;
    }

    public bool HasSummary<T>(string key, IncompleteDate timestamp, Func<T, bool> summaryPropertySelector)
    {
        string path = GetTimeFilePath(key, timestamp);
        return _pack.Exists(path) && ReadArray<T>(path).Any(summaryPropertySelector);
    }

    public List<TimeEntry<T>> QueryWithLevel<T>(string key, IncompleteDate level)
    {
        string keyDirPrefix = GetKeyDirectoryPrefix(key);
        var result = new List<TimeEntry<T>>();
        foreach (var timestamp in level.Expand())
        {
            string filePath = GetTimeFilePath(key, timestamp);
            if (!_pack.Exists(filePath)) continue;
            if (!TryParseTimestampFromPath(keyDirPrefix, filePath, out IncompleteDate ft)) continue;
            foreach (T data in ReadArray<T>(filePath)) result.Add(new TimeEntry<T>(key, ft, data));
        }
        result.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
        return result;
    }

    // ─── Enumeration helpers ──────────────────────────────────────────────────

    private IEnumerable<string> EnumerateAllEntries(string keyDirPrefix) =>
        EnumerateAllEntriesUnder(keyDirPrefix);

    private IEnumerable<string> EnumerateAllEntriesUnder(string dirPrefix)
    {
        string dir = dirPrefix.TrimEnd('/');
        foreach (string entry in _pack.ListEntries(dir))
            if (entry.EndsWith(".json", StringComparison.OrdinalIgnoreCase)) yield return entry;
        foreach (string subDir in _pack.ListDirectories(dir))
            foreach (string entry in EnumerateAllEntriesUnder(subDir.TrimEnd('/') + "/"))
                yield return entry;
    }

    // ─── IDisposable ──────────────────────────────────────────────────────────

    /// <summary>
    /// No-op. The underlying <see cref="SpeedyPack"/> lifetime is controlled by
    /// <see cref="SpeedyPackRegistry.Dispose"/>.
    /// </summary>
    public void Dispose() { }
}
