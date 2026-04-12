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

public sealed class FileSystemLoggerProvider : ILoggerProvider
{
    private const string DefaultKey = "default";
    private const string LogFolderName = "Log";

    private readonly FileSystemTimeStorage _storage;
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
            entry.Timestamp,
            entry.Level,
            entry.Category,
            entry.Message,
            entry.Exception?.ToString()
        );

        byte[] data = JsonSerializer.SerializeToUtf8Bytes(dto, _jsonOptions);
        _storage.Write(DefaultKey, entry.Timestamp, data);
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

    private sealed record LogEntryDto(
        DateTime Timestamp,
        LogLevel Level,
        string Category,
        string Message,
        string? Exception
    );
}
