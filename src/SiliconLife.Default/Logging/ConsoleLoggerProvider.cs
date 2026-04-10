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

namespace SiliconLife.Default.Logging;

/// <summary>
/// A logger provider that writes log entries to the console.
/// Supports colored output based on log level.
/// </summary>
public sealed class ConsoleLoggerProvider : ILoggerProvider
{
    private readonly object _lock = new();
    private LogLevel _minimumLevel = LogLevel.Trace;
    private bool _disposed = false;
    private bool _useColors = true;

    /// <summary>
    /// Gets the name of this provider.
    /// </summary>
    public string Name => "Console";

    /// <summary>
    /// Gets or sets the minimum log level for this provider.
    /// </summary>
    public LogLevel MinimumLevel
    {
        get
        {
            return _minimumLevel;
        }
        set
        {
            _minimumLevel = value;
        }
    }

    /// <summary>
    /// Gets or sets whether to use colored output.
    /// </summary>
    public bool UseColors
    {
        get
        {
            return _useColors;
        }
        set
        {
            _useColors = value;
        }
    }

    /// <summary>
    /// Writes a log entry to the console.
    /// </summary>
    /// <param name="entry">The log entry to write.</param>
    public void WriteLog(LogEntry entry)
    {
        if (entry == null)
        {
            return;
        }

        lock (_lock)
        {
            ConsoleColor originalColor = Console.ForegroundColor;

            try
            {
                if (_useColors)
                {
                    Console.ForegroundColor = GetLevelColor(entry.Level);
                }

                string formattedMessage = FormatEntry(entry);
                Console.WriteLine(formattedMessage);

                if (entry.Exception != null)
                {
                    if (_useColors)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.WriteLine(entry.Exception.ToString());
                }
            }
            finally
            {
                if (_useColors)
                {
                    Console.ForegroundColor = originalColor;
                }
            }
        }
    }

    /// <summary>
    /// Checks if this provider is enabled for the specified log level.
    /// </summary>
    /// <param name="level">The log level to check.</param>
    /// <returns>True if enabled for the specified level; otherwise, false.</returns>
    public bool IsEnabled(LogLevel level)
    {
        return level >= _minimumLevel;
    }

    /// <summary>
    /// Flushes any buffered log entries. For console output, this is a no-op.
    /// </summary>
    public void Flush()
    {
        // Console output is not buffered, so nothing to flush
    }

    /// <summary>
    /// Releases all resources used by this provider.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
    }

    /// <summary>
    /// Gets the console color for a specific log level.
    /// </summary>
    /// <param name="level">The log level.</param>
    /// <returns>The console color for the level.</returns>
    private static ConsoleColor GetLevelColor(LogLevel level)
    {
        return level switch
        {
            LogLevel.Trace => ConsoleColor.DarkGray,
            LogLevel.Debug => ConsoleColor.Gray,
            LogLevel.Information => ConsoleColor.White,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Critical => ConsoleColor.Magenta,
            LogLevel.None => ConsoleColor.White,
            _ => ConsoleColor.White
        };
    }

    /// <summary>
    /// Formats a log entry for console output.
    /// </summary>
    /// <param name="entry">The log entry to format.</param>
    /// <returns>A formatted string.</returns>
    private static string FormatEntry(LogEntry entry)
    {
        string levelStr = entry.Level.ToString().ToUpperInvariant().PadLeft(4);
        string timestampStr = entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
        return $"[{timestampStr}] [{levelStr}] [{entry.Category}] {entry.Message}";
    }
}
