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

public class PerformanceRecord
{
    public string Name { get; set; } = string.Empty;
    public TimeSpan AverageExecutionTime { get; set; }
    public TimeSpan MaxExecutionTime { get; set; }
    public TimeSpan MinExecutionTime { get; set; }
    public int ExecutionCount { get; set; }
    public DateTime LastExecutedAt { get; set; }
}

public class PerformanceMonitor
{
    private readonly Dictionary<string, List<TimeSpan>> _executionTimes = new();
    private readonly object _lock = new();
    private const int MaxSamples = 100;

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
            }
        }
    }

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

    public void Clear()
    {
        lock (_lock)
        {
            _executionTimes.Clear();
        }
    }
}
