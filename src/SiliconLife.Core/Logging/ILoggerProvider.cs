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
/// Represents a provider that can write log entries to a specific destination.
/// Implementations define where and how logs are written (console, file, database, etc.).
/// </summary>
public interface ILoggerProvider
{
    /// <summary>
    /// Gets the name of this provider for identification purposes.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets or sets the minimum log level for this provider.
    /// Log entries below this level will not be written by this provider.
    /// </summary>
    LogLevel MinimumLevel { get; set; }

    /// <summary>
    /// Writes a log entry to the provider's destination.
    /// </summary>
    /// <param name="entry">The log entry to write.</param>
    void WriteLog(LogEntry entry);

    /// <summary>
    /// Checks if this provider is enabled for the specified log level.
    /// </summary>
    /// <param name="level">The log level to check.</param>
    /// <returns>True if this provider is enabled for the specified level; otherwise, false.</returns>
    bool IsEnabled(LogLevel level);

    /// <summary>
    /// Flushes any buffered log entries to the destination.
    /// </summary>
    void Flush();

    /// <summary>
    /// Releases all resources used by this provider.
    /// </summary>
    void Dispose();
}
