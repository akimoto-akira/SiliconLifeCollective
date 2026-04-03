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
/// Time-indexed storage interface extending <see cref="IStorage"/>.
/// Each entry is associated with a <see cref="DateTime"/> timestamp,
/// and can be queried by <see cref="IncompleteDate"/> ranges.
/// </summary>
public interface ITimeStorage : IStorage
{
    /// <summary>
    /// Writes data indexed by a composite key and timestamp.
    /// </summary>
    void Write(string key, DateTime timestamp, byte[] data);

    /// <summary>
    /// Writes data indexed by a composite key and IncompleteDate timestamp.
    /// </summary>
    void Write(string key, IncompleteDate timestamp, byte[] data);

    /// <summary>
    /// Reads data by exact key and timestamp.
    /// </summary>
    byte[]? Read(string key, DateTime timestamp);

    /// <summary>
    /// Reads data by exact key and IncompleteDate timestamp.
    /// </summary>
    byte[]? Read(string key, IncompleteDate timestamp);

    /// <summary>
    /// Checks if an entry exists for the exact key and timestamp.
    /// </summary>
    bool Exists(string key, DateTime timestamp);

    /// <summary>
    /// Checks if an entry exists for the exact key and IncompleteDate timestamp.
    /// </summary>
    bool Exists(string key, IncompleteDate timestamp);

    /// <summary>
    /// Deletes the entry at the exact key and timestamp.
    /// </summary>
    void Delete(string key, DateTime timestamp);

    /// <summary>
    /// Deletes the entry at the exact key and IncompleteDate timestamp.
    /// </summary>
    void Delete(string key, IncompleteDate timestamp);

    /// <summary>
    /// Queries entries matching the given key prefix and time range.
    /// </summary>
    /// <param name="key">The logical key prefix to match.</param>
    /// <param name="range">The time range to match (unspecified components are wildcards).</param>
    /// <returns>All matching entries, ordered by timestamp ascending.</returns>
    List<TimeEntry> Query(string key, IncompleteDate range);

    /// <summary>
    /// Queries all entries matching the time range across all keys.
    /// </summary>
    /// <param name="range">The time range to match.</param>
    /// <returns>All matching entries, ordered by timestamp ascending.</returns>
    List<TimeEntry> Query(IncompleteDate range);

    /// <summary>
    /// Returns the number of entries matching the given key and time range.
    /// </summary>
    int Count(string key, IncompleteDate range);

    /// <summary>
    /// Returns the total number of entries matching the time range across all keys.
    /// </summary>
    int Count(IncompleteDate range);

    /// <summary>
    /// Deletes all entries matching the given key and time range.
    /// </summary>
    /// <returns>The number of entries deleted.</returns>
    int DeleteRange(string key, IncompleteDate range);
}

/// <summary>
/// A single entry returned by time-indexed queries.
/// </summary>
public sealed class TimeEntry
{
    /// <summary>The logical key.</summary>
    public string Key { get; }

    /// <summary>The timestamp associated with this entry.</summary>
    public DateTime Timestamp { get; }

    /// <summary>The payload data.</summary>
    public byte[] Data { get; }

    public TimeEntry(string key, DateTime timestamp, byte[] data)
    {
        Key = key;
        Timestamp = timestamp;
        Data = data;
    }
}
