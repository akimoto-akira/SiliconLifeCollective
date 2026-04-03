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

namespace SiliconLife.Default;

/// <summary>
/// File system implementation of <see cref="ITimeStorage"/>.
/// Time-indexed entries are stored as individual files organized by
/// <c>{key}/{yyyy}/{MM}/{dd}/{HH}/{mm}/{ss}.json</c>.
/// </summary>
public class FileSystemTimeStorage : ITimeStorage
{
    private readonly string _baseDirectory;

    public FileSystemTimeStorage(string baseDirectory)
    {
        _baseDirectory = baseDirectory;
        if (!Directory.Exists(_baseDirectory))
        {
            Directory.CreateDirectory(_baseDirectory);
        }
    }

    // ── IStorage ────────────────────────────────────

    public byte[]? Read(string key)
    {
        string dir = GetKeyDirectory(key);
        if (!Directory.Exists(dir)) return null;

        byte[]? latest = null;
        DateTime latestTime = DateTime.MinValue;

        foreach (string file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
        {
            string? dirName = Path.GetDirectoryName(file);
            if (dirName == null) continue;

            string relDir = dirName[dir.Length..].TrimStart(Path.DirectorySeparatorChar);
            if (!TryParseTimeFromPath(relDir, out DateTime fileTime)) continue;

            if (fileTime > latestTime)
            {
                latestTime = fileTime;
                latest = File.ReadAllBytes(file);
            }
        }

        return latest;
    }

    public void Write(string key, byte[] data)
    {
        string filePath = Path.Combine(GetKeyDirectory(key), "latest.json");
        string? dir = Path.GetDirectoryName(filePath);
        if (dir != null) Directory.CreateDirectory(dir);
        File.WriteAllBytes(filePath, data);
    }

    public bool Exists(string key)
    {
        return Read(key) != null;
    }

    public void Delete(string key)
    {
        string dir = GetKeyDirectory(key);
        if (Directory.Exists(dir))
        {
            Directory.Delete(dir, true);
        }
    }

    // ── ITimeStorage ────────────────────────────────

    public void Write(string key, DateTime timestamp, byte[] data)
    {
        string filePath = GetTimeFilePath(key, timestamp);
        string? dir = Path.GetDirectoryName(filePath);
        if (dir != null) Directory.CreateDirectory(dir);
        File.WriteAllBytes(filePath, data);
    }

    public void Write(string key, IncompleteDate timestamp, byte[] data)
    {
        string filePath = GetTimeFilePath(key, timestamp);
        string? dir = Path.GetDirectoryName(filePath);
        if (dir != null) Directory.CreateDirectory(dir);
        
        foreach (var f in Directory.GetFiles(dir, "*.json"))
        {
            File.Delete(f);
        }
        
        File.WriteAllBytes(filePath.Replace("*.json", "0.json"), data);
    }

    public byte[]? Read(string key, DateTime timestamp)
    {
        string dir = GetKeyDirectory(key);
        if (!Directory.Exists(dir)) return null;

        byte[]? best = null;
        long bestDiff = long.MaxValue;

        foreach (string file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
        {
            if (TryParseTimestampFromFile(file, out DateTime fileTime))
            {
                long diff = Math.Abs((fileTime - timestamp).Ticks);
                if (diff < bestDiff)
                {
                    bestDiff = diff;
                    best = File.ReadAllBytes(file);
                }
            }
        }

        return best;
    }

    public byte[]? Read(string key, IncompleteDate timestamp)
    {
        string dir = GetKeyDirectory(key);
        string patternPath = GetTimeFilePath(key, timestamp);
        string? dirPattern = Path.GetDirectoryName(patternPath);
        
        if (dirPattern == null || !Directory.Exists(dirPattern))
            return null;

        var files = Directory.GetFiles(dirPattern, "*.json");
        if (files.Length == 0)
            return null;

        return File.ReadAllBytes(files[0]);
    }

    public bool Exists(string key, DateTime timestamp)
    {
        return Read(key, timestamp) != null;
    }

    public bool Exists(string key, IncompleteDate timestamp)
    {
        return Read(key, timestamp) != null;
    }

    public void Delete(string key, DateTime timestamp)
    {
        string filePath = GetTimeFilePath(key, timestamp);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public void Delete(string key, IncompleteDate timestamp)
    {
        string dir = GetKeyDirectory(key);
        string patternPath = GetTimeFilePath(key, timestamp);
        string? dirPattern = Path.GetDirectoryName(patternPath);
        
        if (dirPattern == null || !Directory.Exists(dirPattern))
            return;

        foreach (var f in Directory.GetFiles(dirPattern, "*.json"))
        {
            File.Delete(f);
        }
    }

    public List<TimeEntry> Query(string key, IncompleteDate range)
    {
        List<TimeEntry> result = [];
        string dir = GetKeyDirectory(key);
        if (!Directory.Exists(dir)) return result;

        foreach (string file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
        {
            if (TryParseTimestampFromFile(file, out DateTime fileTime) && range.Matches(fileTime))
            {
                result.Add(new(key, fileTime, File.ReadAllBytes(file)));
            }
        }

        result.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
        return result;
    }

    public List<TimeEntry> Query(IncompleteDate range)
    {
        List<TimeEntry> result = [];

        if (!Directory.Exists(_baseDirectory)) return result;

        foreach (string keyDir in Directory.GetDirectories(_baseDirectory))
        {
            string key = Path.GetFileName(keyDir);
            foreach (string file in Directory.GetFiles(keyDir, "*.json", SearchOption.AllDirectories))
            {
                if (TryParseTimestampFromFile(file, out DateTime fileTime) && range.Matches(fileTime))
                {
                    result.Add(new(key, fileTime, File.ReadAllBytes(file)));
                }
            }
        }

        result.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
        return result;
    }

    public int Count(string key, IncompleteDate range)
    {
        return Query(key, range).Count;
    }

    public int Count(IncompleteDate range)
    {
        return Query(range).Count;
    }

    public int DeleteRange(string key, IncompleteDate range)
    {
        string dir = GetKeyDirectory(key);
        if (!Directory.Exists(dir)) return 0;

        int deleted = 0;
        foreach (string file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
        {
            if (TryParseTimestampFromFile(file, out DateTime fileTime) && range.Matches(fileTime))
            {
                File.Delete(file);
                deleted++;
            }
        }

        CleanEmptyDirs(dir);
        return deleted;
    }

    // ── Path helpers ────────────────────────────────

    private string GetKeyDirectory(string key)
    {
        string safeKey = key.Replace("..", "");
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

    private string GetTimeFilePath(string key, IncompleteDate timestamp)
    {
        var parts = new List<string> { GetKeyDirectory(key) };

        if (timestamp.Year > 0)
            parts.Add(timestamp.Year.ToString());
        if (timestamp.Month.HasValue)
            parts.Add(timestamp.Month.Value.ToString("D2"));
        if (timestamp.Day.HasValue)
            parts.Add(timestamp.Day.Value.ToString("D2"));
        if (timestamp.Hour.HasValue)
            parts.Add(timestamp.Hour.Value.ToString("D2"));
        if (timestamp.Minute.HasValue)
            parts.Add(timestamp.Minute.Value.ToString("D2"));
        if (timestamp.Second.HasValue)
            parts.Add($"{timestamp.Second.Value:D2}.json");
        else
            parts.Add("*.json");

        return Path.Combine(parts.ToArray());
    }

    /// <summary>
    /// Extracts the DateTime from a time-stamped file path.
    /// Path format: .../{yyyy}/{MM}/{dd}/{HH}/{mm}/{ss}.json
    /// </summary>
    private static bool TryParseTimestampFromFile(string filePath, out DateTime result)
    {
        result = default;
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        if (!int.TryParse(fileName, out int second) || second is < 0 or > 59)
            return false;

        string? dir = Path.GetDirectoryName(filePath);
        if (dir == null) return false;

        // Walk up 5 directory levels: mm, HH, dd, MM, yyyy
        if (!TryIntSegment(dir, 0, out int minute) || minute is < 0 or > 59) return false;
        if (!TryIntSegment(dir, 1, out int hour) || hour is < 0 or > 23) return false;
        if (!TryIntSegment(dir, 2, out int day) || day is < 1 or > 31) return false;
        if (!TryIntSegment(dir, 3, out int month) || month is < 1 or > 12) return false;
        if (!TryIntSegment(dir, 4, out int year)) return false;

        try { result = new DateTime(year, month, day, hour, minute, second); return true; }
        catch { return false; }
    }

    /// <summary>
    /// Reads a directory name segment at the given depth above the path.
    /// Depth 1 = immediate parent, 2 = grandparent, etc.
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

    private static bool TryParseTimeFromPath(string relativeDir, out DateTime result)
    {
        result = default;
        string[] segs = relativeDir.Split(Path.DirectorySeparatorChar);
        if (segs.Length < 1 || !int.TryParse(segs[0], out int year)) return false;

        int month = 1;
        if (segs.Length >= 2) int.TryParse(segs[1], out month);

        try { result = new DateTime(year, month, 1); return true; }
        catch { return false; }
    }

    private static void CleanEmptyDirs(string rootDir)
    {
        if (!Directory.Exists(rootDir)) return;
        foreach (string dir in Directory.GetDirectories(rootDir))
        {
            CleanEmptyDirs(dir);
            if (!Directory.EnumerateFileSystemEntries(dir).Any())
            {
                Directory.Delete(dir);
            }
        }
    }
}
