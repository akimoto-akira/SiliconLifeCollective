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

namespace SiliconLife.Default.Logging;

public sealed class FileSystemLoggerProvider : ILoggerProvider, ILogReader
{
    private const string DefaultKey = "default";
    private const string LogFolderName = "Log";

    private readonly ITimeStorage _storage;
    private readonly JsonSerializerOptions _jsonOptions;
    private LogLevel _minimumLevel = LogLevel.Trace;

    public string Name => "FileSystem";

    public LogLevel MinimumLevel
    {
        get => _minimumLevel;
        set => _minimumLevel = value;
    }

    public FileSystemLoggerProvider(ConfigDataBase config)
    {
        string logDirectory = Path.Combine(config.DataDirectory.FullName, LogFolderName);
        _storage = new FileSystemTimeStorage(logDirectory);
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = false
        };
    }

    public void WriteLog(LogEntry entry)
    {
        if (entry == null || !IsEnabled(entry.Level))
            return;

        var dto = new LogEntryDto(
            Guid.NewGuid(),
            entry.BeingId,
            entry.Timestamp,
            entry.Level,
            entry.Category,
            entry.Message,
            entry.Exception?.ToString()
        );

        var ts = entry.Timestamp;
        _storage.Write(DefaultKey, new IncompleteDate(ts.Year, ts.Month, ts.Day, ts.Hour, ts.Minute, ts.Second), dto);
    }

    public bool IsEnabled(LogLevel level)
    {
        return level >= _minimumLevel && level != LogLevel.None;
    }

    public void Flush()
    {
    }

    public void Dispose()
    {
    }

    public List<LogEntry> ReadLogs(
        DateTime? startTime = null,
        DateTime? endTime = null,
        Guid? beingId = null,
        bool systemOnly = false,
        LogLevel? levelFilter = null,
        int maxCount = 0)
    {
        var entries = _storage.Query<LogEntryDto>(DefaultKey, null);
        var results = new List<LogEntry>();

        foreach (var timeEntry in entries)
        {
            var dto = timeEntry.Data;

            // Time range filter - ensure UTC comparison
            var timestampUtc = DateTime.SpecifyKind(
                new DateTime(timeEntry.Timestamp.Year, timeEntry.Timestamp.Month ?? 1, timeEntry.Timestamp.Day ?? 1,
                    timeEntry.Timestamp.Hour ?? 0, timeEntry.Timestamp.Minute ?? 0, timeEntry.Timestamp.Second ?? 0),
                DateTimeKind.Utc);
            if (startTime.HasValue && timestampUtc < startTime.Value)
                continue;
            if (endTime.HasValue && timestampUtc > endTime.Value)
                continue;

            // Being filter
            if (systemOnly && dto.BeingId.HasValue)
                continue;
            if (beingId.HasValue && (!dto.BeingId.HasValue || dto.BeingId.Value != beingId.Value))
                continue;

            // Level filter
            if (levelFilter.HasValue && dto.Level != levelFilter.Value)
                continue;

            // Convert DTO to LogEntry
            var logEntry = new LogEntry(
                dto.BeingId,
                dto.Timestamp,
                dto.Level,
                dto.Category,
                dto.Message,
                dto.Exception != null ? new Exception(dto.Exception) : null
            );

            results.Add(logEntry);
        }

        // Sort by timestamp descending
        results.Sort((a, b) => b.Timestamp.CompareTo(a.Timestamp));

        // Apply maxCount
        if (maxCount > 0 && results.Count > maxCount)
        {
            results = results.Take(maxCount).ToList();
        }

        return results;
    }

    private sealed record LogEntryDto(
        Guid Id,
        Guid? BeingId,
        DateTime Timestamp,
        LogLevel Level,
        string Category,
        string Message,
        string? Exception
    );
}
