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
/// Represents a snapshot of performance statistics for a named operation,
/// including average, min, max execution times and invocation count.
/// </summary>
public class PerformanceRecord
{
    /// <summary>The name of the tracked operation.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>The average execution time across all recorded samples.</summary>
    public TimeSpan AverageExecutionTime { get; set; }

    /// <summary>The maximum observed execution time.</summary>
    public TimeSpan MaxExecutionTime { get; set; }

    /// <summary>The minimum observed execution time.</summary>
    public TimeSpan MinExecutionTime { get; set; }

    /// <summary>Total number of recorded executions.</summary>
    public int ExecutionCount { get; set; }

    /// <summary>UTC timestamp of the most recent execution sample.</summary>
    public DateTime LastExecutedAt { get; set; }
}

/// <summary>
/// Thread-safe performance monitor that records execution times for named
/// operations and computes rolling statistics over the most recent
/// <see cref="MaxSamples"/> samples.
/// </summary>
public class PerformanceMonitor
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<PerformanceMonitor>();
    private readonly Dictionary<string, List<TimeSpan>> _executionTimes = new();
    private readonly object _lock = new();
    private const int MaxSamples = 100;

    /// <summary>
    /// Records a single execution time sample for the named operation.
    /// When the sample count exceeds <see cref="MaxSamples"/>, the oldest
    /// sample is discarded to maintain a rolling window.
    /// </summary>
    /// <param name="name">The operation name to record under.</param>
    /// <param name="executionTime">The elapsed time of this execution.</param>
    public void Record(string name, TimeSpan executionTime)
    {
        lock (_lock)
        {
            if (!_executionTimes.ContainsKey(name))
            {
                _executionTimes[name] = new List<TimeSpan>();
            }

            var samples = _executionTimes[name];
            samples.Add(executionTime);

            if (samples.Count > MaxSamples)
            {
                samples.RemoveAt(0);
                _logger.Debug("Performance sample overflow for {0}, discarding oldest", name);
            }

            _logger.Trace("Performance recorded: {0} = {1}ms", name, executionTime.TotalMilliseconds);
        }
    }

    /// <summary>
    /// Returns a computed <see cref="PerformanceRecord"/> for the named
    /// operation, or <c>null</c> if no samples have been recorded.
    /// </summary>
    public PerformanceRecord? GetRecord(string name)
    {
        lock (_lock)
        {
            if (!_executionTimes.TryGetValue(name, out var samples) || samples.Count == 0)
            {
                return null;
            }

            return new PerformanceRecord
            {
                Name = name,
                AverageExecutionTime = TimeSpan.FromTicks((long)samples.Average(s => s.Ticks)),
                MaxExecutionTime = samples.Max(),
                MinExecutionTime = samples.Min(),
                ExecutionCount = samples.Count,
                LastExecutedAt = DateTime.UtcNow
            };
        }
    }

    /// <summary>
    /// Returns performance records for all tracked operations.
    /// </summary>
    public Dictionary<string, PerformanceRecord> GetAllRecords()
    {
        lock (_lock)
        {
            var result = new Dictionary<string, PerformanceRecord>();

            foreach (var kvp in _executionTimes)
            {
                var record = GetRecord(kvp.Key);
                if (record != null)
                {
                    result[kvp.Key] = record;
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Clears all recorded samples for every operation.
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _executionTimes.Clear();
        }
    }
}
