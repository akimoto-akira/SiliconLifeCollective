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
/// Provides automatic JSON serialization/deserialization.
/// </summary>
public interface ITimeStorage : IStorage
{
    /// <summary>
    /// Writes data indexed by a composite key and timestamp with automatic JSON serialization.
    /// </summary>
    void Write<T>(string key, IncompleteDate timestamp, T data);

    /// <summary>
    /// Reads data by exact key and timestamp and deserializes to type T.
    /// </summary>
    T? Read<T>(string key, IncompleteDate timestamp);

    /// <summary>
    /// Checks if an entry exists for the exact key and timestamp.
    /// </summary>
    bool Exists(string key, IncompleteDate timestamp);

    /// <summary>
    /// Deletes the entry at the exact key and timestamp.
    /// </summary>
    void Delete(string key, IncompleteDate timestamp);

    /// <summary>
    /// Queries entries matching the given key prefix and time range.
    /// </summary>
    /// <param name="key">The logical key prefix to match.</param>
    /// <param name="range">The time range to match (unspecified components are wildcards). Null means all entries.</param>
    /// <returns>All matching entries, ordered by timestamp ascending.</returns>
    List<TimeEntry<T>> Query<T>(string key, IncompleteDate? range);

    /// <summary>
    /// Queries all entries matching the time range across all keys.
    /// </summary>
    /// <param name="range">The time range to match (null means all entries without filtering).</param>
    /// <returns>All matching entries, ordered by timestamp ascending.</returns>
    List<TimeEntry<T>> Query<T>(IncompleteDate? range);


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

    /// <summary>
    /// Searches entries by keyword across all time levels for the given key.
    /// Searches in both content and keywords fields of the stored data.
    /// </summary>
    /// <param name="key">The logical key prefix to match.</param>
    /// <param name="keyword">The keyword to search for (case-insensitive).</param>
    /// <param name="maxCount">Maximum number of entries to return. 0 means no limit.</param>
    /// <returns>All matching entries ordered by timestamp descending.</returns>
    List<TimeEntry<T>> Search<T>(string key, string keyword, int maxCount = 0);

    /// <summary>
    /// Gets the earliest recorded timestamp for the given key.
    /// </summary>
    /// <param name="key">The logical key prefix to match.</param>
    /// <returns>The earliest timestamp, or null when the key has no entries.</returns>
    IncompleteDate? GetEarliestTimestamp(string key);

    /// <summary>
    /// Gets the latest recorded timestamp for the given key.
    /// </summary>
    /// <param name="key">The logical key prefix to match.</param>
    /// <returns>The latest timestamp, or null when the key has no entries.</returns>
    IncompleteDate? GetLatestTimestamp(string key);

    /// <summary>
    /// Gets the earliest recorded timestamp across all keys.
    /// </summary>
    /// <returns>The earliest timestamp, or null when the storage is empty.</returns>
    IncompleteDate? GetEarliestTimestamp();

    /// <summary>
    /// Gets the latest recorded timestamp across all keys.
    /// </summary>
    /// <returns>The latest timestamp, or null when the storage is empty.</returns>
    IncompleteDate? GetLatestTimestamp();

    /// <summary>
    /// Checks if any entry at the given IncompleteDate location has a summary flag.
    /// The IncompleteDate represents a specific file path, not a range.
    /// </summary>
    /// <typeparam name="T">The type of data stored in entries.</typeparam>
    /// <param name="key">The logical key prefix to match.</param>
    /// <param name="timestamp">The IncompleteDate representing a specific file location.</param>
    /// <param name="summaryPropertySelector">A function to extract the summary flag from an entry's data.</param>
    /// <returns>True if at least one entry at the location has the summary flag set to true.</returns>
    bool HasSummary<T>(string key, IncompleteDate timestamp, Func<T, bool> summaryPropertySelector);

    /// <summary>
    /// Queries all entries at the next finer time level under the given IncompleteDate.
    /// The IncompleteDate determines the directory level, and the method reads all direct child files
    /// at the next level without recursively searching deeper levels.
    /// </summary>
    /// <param name="key">The logical key prefix to match.</param>
    /// <param name="level">The IncompleteDate specifying the time level (e.g., hour level to query all minutes under it).</param>
    /// <returns>All entries at the next finer level, ordered by timestamp ascending.</returns>
    /// <example>
    /// QueryWithLevel(key, new IncompleteDate(2026, 4, 15, 10)) → Returns all minute-level entries under hour 10
    /// QueryWithLevel(key, new IncompleteDate(2026, 4)) → Returns all day-level entries under April 2026
    /// </example>
    List<TimeEntry<T>> QueryWithLevel<T>(string key, IncompleteDate level);
}

/// <summary>
/// A single entry returned by time-indexed queries.
/// </summary>
public sealed class TimeEntry<T>
{
    /// <summary>The logical key.</summary>
    public string Key { get; }

    /// <summary>The timestamp associated with this entry.</summary>
    public IncompleteDate Timestamp { get; }

    /// <summary>The deserialized payload data.</summary>
    public T Data { get; }

    public TimeEntry(string key, IncompleteDate timestamp, T data)
    {
        Key = key;
        Timestamp = timestamp;
        Data = data;
    }
}
