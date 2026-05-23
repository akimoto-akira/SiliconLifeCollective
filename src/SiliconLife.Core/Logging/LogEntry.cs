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
/// Represents a single log entry with all relevant information.
/// </summary>
public sealed class LogEntry
{
    /// <summary>
    /// Gets the silicon being ID associated with this log entry, if any.
    /// Null for system-level logs not associated with any specific being.
    /// </summary>
    public Guid? BeingId { get; }

    /// <summary>
    /// Gets the timestamp when the log entry was created.
    /// </summary>
    public DateTime Timestamp { get; }

    /// <summary>
    /// Gets the log level of this entry.
    /// </summary>
    public LogLevel Level { get; }

    /// <summary>
    /// Gets the category name for this log entry.
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// Gets the log message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the exception associated with this log entry, if any.
    /// </summary>
    public Exception? Exception { get; }

    /// <summary>
    /// Creates a new log entry.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="timestamp">The timestamp of the log entry.</param>
    /// <param name="level">The log level.</param>
    /// <param name="category">The category name.</param>
    /// <param name="message">The log message.</param>
    /// <param name="exception">The optional exception.</param>
    public LogEntry(Guid? beingId, DateTime timestamp, LogLevel level, string category, string message, Exception? exception = null)
    {
        BeingId = beingId;
        Timestamp = timestamp;
        Level = level;
        Category = category;
        Message = message;
        Exception = exception;
    }

    /// <summary>
    /// Returns a string representation of this log entry.
    /// </summary>
    /// <returns>A formatted string representing the log entry.</returns>
    public override string ToString()
    {
        string levelStr = Level.ToString().ToUpperInvariant().PadLeft(4);
        string timestampStr = Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
        string beingStr = BeingId.HasValue ? $" [Being:{BeingId.Value:N}]" : "";
        string exceptionStr = Exception != null ? $"\n{Exception}" : "";
        return $"[{timestampStr}] [{levelStr}] [{Category}]{beingStr} {Message}{exceptionStr}";
    }
}
