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
/// Represents a logger that can distribute log messages to multiple providers.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Gets the category name for this logger.
    /// </summary>
    string Category { get; }

    /// <summary>
    /// Gets or sets the minimum log level for this logger.
    /// Messages below this level will not be logged.
    /// </summary>
    LogLevel MinimumLevel { get; set; }

    /// <summary>
    /// Logs a message with the specified log level.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="level">The log level.</param>
    /// <param name="message">The log message.</param>
    void Log(Guid? beingId, LogLevel level, string message);

    /// <summary>
    /// Logs a message with the specified log level and exception.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="level">The log level.</param>
    /// <param name="message">The log message.</param>
    /// <param name="exception">The exception to log.</param>
    void Log(Guid? beingId, LogLevel level, string message, Exception exception);

    /// <summary>
    /// Logs a formatted message with the specified log level.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="level">The log level.</param>
    /// <param name="format">The message format string.</param>
    /// <param name="args">The format arguments.</param>
    void Log(Guid? beingId, LogLevel level, string format, params object[] args);

    /// <summary>
    /// Logs a trace-level message.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="message">The log message.</param>
    void Trace(Guid? beingId, string message);

    /// <summary>
    /// Logs a trace-level formatted message.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="format">The message format string.</param>
    /// <param name="args">The format arguments.</param>
    void Trace(Guid? beingId, string format, params object[] args);

    /// <summary>
    /// Logs a debug-level message.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="message">The log message.</param>
    void Debug(Guid? beingId, string message);

    /// <summary>
    /// Logs a debug-level formatted message.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="format">The message format string.</param>
    /// <param name="args">The format arguments.</param>
    void Debug(Guid? beingId, string format, params object[] args);

    /// <summary>
    /// Logs an information-level message.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="message">The log message.</param>
    void Info(Guid? beingId, string message);

    /// <summary>
    /// Logs an information-level formatted message.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="format">The message format string.</param>
    /// <param name="args">The format arguments.</param>
    void Info(Guid? beingId, string format, params object[] args);

    /// <summary>
    /// Logs a warning-level message.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="message">The log message.</param>
    void Warn(Guid? beingId, string message);

    /// <summary>
    /// Logs a warning-level message with an exception.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="message">The log message.</param>
    /// <param name="exception">The exception to log.</param>
    void Warn(Guid? beingId, string message, Exception exception);

    /// <summary>
    /// Logs a warning-level formatted message.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="format">The message format string.</param>
    /// <param name="args">The format arguments.</param>
    void Warn(Guid? beingId, string format, params object[] args);

    /// <summary>
    /// Logs an error-level message.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="message">The log message.</param>
    void Error(Guid? beingId, string message);

    /// <summary>
    /// Logs an error-level message with an exception.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="message">The log message.</param>
    /// <param name="exception">The exception to log.</param>
    void Error(Guid? beingId, string message, Exception exception);

    /// <summary>
    /// Logs an error-level formatted message.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="format">The message format string.</param>
    /// <param name="args">The format arguments.</param>
    void Error(Guid? beingId, string format, params object[] args);

    /// <summary>
    /// Logs a critical-level message.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="message">The log message.</param>
    void Critical(Guid? beingId, string message);

    /// <summary>
    /// Logs a critical-level message with an exception.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="message">The log message.</param>
    /// <param name="exception">The exception to log.</param>
    void Critical(Guid? beingId, string message, Exception exception);

    /// <summary>
    /// Logs a critical-level formatted message.
    /// </summary>
    /// <param name="beingId">The silicon being ID, or null for system-level logs.</param>
    /// <param name="format">The message format string.</param>
    /// <param name="args">The format arguments.</param>
    void Critical(Guid? beingId, string format, params object[] args);

    /// <summary>
    /// Checks if logging is enabled for the specified log level.
    /// </summary>
    /// <param name="level">The log level to check.</param>
    /// <returns>True if logging is enabled for the specified level; otherwise, false.</returns>
    bool IsEnabled(LogLevel level);
}
