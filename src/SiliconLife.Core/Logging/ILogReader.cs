// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace SiliconLife.Collective;

/// <summary>
/// Represents a logger provider that supports reading log entries.
/// Implementations define how logs are retrieved from the storage.
/// </summary>
public interface ILogReader
{
    /// <summary>
    /// Reads log entries with optional filters.
    /// </summary>
    /// <param name="startTime">Optional start time filter</param>
    /// <param name="endTime">Optional end time filter</param>
    /// <param name="beingId">Optional being ID filter. Null means all beings.</param>
    /// <param name="systemOnly">If true, only return system-level logs (BeingId is null)</param>
    /// <param name="levelFilter">Optional log level filter</param>
    /// <param name="maxCount">Maximum number of entries to return. 0 means no limit.</param>
    /// <returns>List of log entries matching the filters</returns>
    List<LogEntry> ReadLogs(
        DateTime? startTime = null,
        DateTime? endTime = null,
        Guid? beingId = null,
        bool systemOnly = false,
        LogLevel? levelFilter = null,
        int maxCount = 0);
}
