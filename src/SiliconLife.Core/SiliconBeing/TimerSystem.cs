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

namespace SiliconLife.Collective;

/// <summary>
/// Represents the type of a timer.
/// </summary>
public enum TimerType
{
    /// <summary>
    /// The timer triggers only once.
    /// </summary>
    Once,

    /// <summary>
    /// The timer triggers repeatedly at specified intervals.
    /// </summary>
    Recurring
}

/// <summary>
/// Represents the status of a timer.
/// </summary>
public enum TimerStatus
{
    /// <summary>
    /// The timer is active and waiting to trigger.
    /// </summary>
    Active,

    /// <summary>
    /// The timer is paused and not triggering.
    /// </summary>
    Paused,

    /// <summary>
    /// The timer has been triggered (for one-shot timers).
    /// </summary>
    Triggered,

    /// <summary>
    /// The timer has been cancelled.
    /// </summary>
    Cancelled
}

/// <summary>
/// Represents a single timer item.
/// </summary>
public sealed class TimerItem
{
    /// <summary>
    /// Gets or sets the unique identifier of the timer.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the timer.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the timer.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the type of the timer.
    /// </summary>
    public TimerType Type { get; set; }

    /// <summary>
    /// Gets or sets the current status of the timer.
    /// </summary>
    public TimerStatus Status { get; set; } = TimerStatus.Active;

    /// <summary>
    /// Gets or sets the next trigger time.
    /// </summary>
    public DateTime TriggerTime { get; set; }

    /// <summary>
    /// Gets or sets the interval for recurring timers.
    /// </summary>
    public TimeSpan? Interval { get; set; }

    /// <summary>
    /// Gets or sets the number of times the timer has been triggered.
    /// </summary>
    public int TimesTriggered { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of triggers for recurring timers.
    /// </summary>
    public int? MaxTriggers { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the timer was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the timer was last triggered.
    /// </summary>
    public DateTime? LastTriggeredAt { get; set; }

    /// <summary>
    /// Gets or sets additional metadata for the timer.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the TimerItem class.
    /// </summary>
    public TimerItem()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    /// <summary>
    /// Initializes a new instance of the TimerItem class with the specified name and trigger time.
    /// </summary>
    /// <param name="name">The name of the timer.</param>
    /// <param name="triggerTime">The time when the timer should trigger.</param>
    /// <param name="type">The type of the timer.</param>
    public TimerItem(string name, DateTime triggerTime, TimerType type = TimerType.Once) : this()
    {
        Name = name;
        TriggerTime = triggerTime;
        Type = type;
    }

    /// <summary>
    /// Determines whether the timer should trigger now.
    /// </summary>
    /// <returns>True if the timer is active and its trigger time has passed; otherwise, false.</returns>
    public bool ShouldTrigger()
    {
        if (Status != TimerStatus.Active)
            return false;

        return DateTime.Now >= TriggerTime;
    }

    /// <summary>
    /// Triggers the timer and updates its state.
    /// </summary>
    public void Trigger()
    {
        LastTriggeredAt = DateTime.Now;
        TimesTriggered++;

        if (Type == TimerType.Once)
        {
            Status = TimerStatus.Triggered;
        }
        else if (MaxTriggers.HasValue && TimesTriggered >= MaxTriggers.Value)
        {
            Status = TimerStatus.Triggered;
        }
        else if (Interval.HasValue)
        {
            TriggerTime = TriggerTime.Add(Interval.Value);
        }
    }

    /// <summary>
    /// Pauses the timer if it is active.
    /// </summary>
    public void Pause()
    {
        if (Status == TimerStatus.Active)
        {
            Status = TimerStatus.Paused;
        }
    }

    /// <summary>
    /// Resumes a paused timer.
    /// </summary>
    public void Resume()
    {
        if (Status == TimerStatus.Paused)
        {
            Status = TimerStatus.Active;
        }
    }

    /// <summary>
    /// Cancels the timer.
    /// </summary>
    public void Cancel()
    {
        Status = TimerStatus.Cancelled;
    }

    /// <summary>
    /// Resets the timer to its initial state.
    /// </summary>
    public void Reset()
    {
        Status = TimerStatus.Active;
        TimesTriggered = 0;
        LastTriggeredAt = null;
    }

    /// <summary>
    /// Gets the time remaining until the next trigger.
    /// </summary>
    /// <returns>The time until next trigger, or null if the timer is not active.</returns>
    public TimeSpan? GetTimeUntilNextTrigger()
    {
        if (Status != TimerStatus.Active)
            return null;

        var diff = TriggerTime - DateTime.Now;
        return diff > TimeSpan.Zero ? diff : TimeSpan.Zero;
    }
}

/// <summary>
/// Provides statistics about the timer system.
/// </summary>
public sealed class TimerStatistics
{
    /// <summary>
    /// Gets or sets the total number of timers.
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// Gets or sets the number of active timers.
    /// </summary>
    public int Active { get; set; }

    /// <summary>
    /// Gets or sets the number of paused timers.
    /// </summary>
    public int Paused { get; set; }

    /// <summary>
    /// Gets or sets the number of triggered timers.
    /// </summary>
    public int Triggered { get; set; }

    /// <summary>
    /// Gets or sets the number of cancelled timers.
    /// </summary>
    public int Cancelled { get; set; }

    /// <summary>
    /// Gets or sets the next trigger time across all active timers.
    /// </summary>
    public DateTime? NextTrigger { get; set; }
}

/// <summary>
/// Contains information about a timer trigger event.
/// </summary>
public sealed class TimerCallbackInfo
{
    /// <summary>
    /// Gets or sets the ID of the triggered timer.
    /// </summary>
    public Guid TimerId { get; set; }

    /// <summary>
    /// Gets or sets the name of the triggered timer.
    /// </summary>
    public string TimerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of times the timer has been triggered.
    /// </summary>
    public int TimesTriggered { get; set; }

    /// <summary>
    /// Gets or sets the metadata associated with the timer.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = new();
}

/// <summary>
/// System for managing timers with one-shot and recurring functionality.
/// </summary>
public sealed class TimerSystem
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<TimerSystem>();
    private readonly IStorage _storage;
    private readonly string _storageKey;
    private readonly object _lock = new();

    private List<TimerItem> _timers = new();
    private readonly List<Action<TimerCallbackInfo>> _callbacks = new();

    /// <summary>
    /// Gets the total number of timers.
    /// </summary>
    public int Count => _timers.Count;

    /// <summary>
    /// Gets the number of active timers.
    /// </summary>
    public int ActiveCount => _timers.Count(t => t.Status == TimerStatus.Active);

    /// <summary>
    /// Event raised when a timer is triggered.
    /// </summary>
    public event Action<TimerCallbackInfo>? OnTimerTriggered;

    /// <summary>
    /// Initializes a new instance of the TimerSystem class with the specified storage.
    /// </summary>
    /// <param name="storage">The storage to use for persisting timers.</param>
    /// <exception cref="ArgumentNullException">Thrown when storage is null.</exception>
    public TimerSystem(IStorage storage)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _storageKey = "timers";

        Load();
    }

    private void Load()
    {
        try
        {
            byte[]? data = _storage.Read(_storageKey);
            if (data == null || data.Length == 0)
                return;

            string json = System.Text.Encoding.UTF8.GetString(data);
            _timers = JsonSerializer.Deserialize<List<TimerItem>>(json) ?? new List<TimerItem>();

            foreach (var timer in _timers.Where(t => t.Status == TimerStatus.Triggered))
            {
                if (timer.Type == TimerType.Recurring && !timer.MaxTriggers.HasValue ||
                    timer.TimesTriggered < timer.MaxTriggers)
                {
                    timer.Status = TimerStatus.Active;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Warn("Failed to load timers from storage", ex);
            _timers = new List<TimerItem>();
        }
    }

    /// <summary>
    /// Saves all timers to storage.
    /// </summary>
    public void Save()
    {
        try
        {
            string json = JsonSerializer.Serialize(_timers);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(json);
            _storage.Write(_storageKey, data);
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to save timers to storage", ex);
        }
    }

    /// <summary>
    /// Creates a one-shot timer that triggers once at the specified time.
    /// </summary>
    /// <param name="name">The name of the timer.</param>
    /// <param name="triggerTime">The time when the timer should trigger.</param>
    /// <param name="metadata">Optional metadata for the timer.</param>
    /// <returns>The created timer item.</returns>
    public TimerItem CreateOneShot(string name, DateTime triggerTime, Dictionary<string, string>? metadata = null)
    {
        lock (_lock)
        {
            var timer = new TimerItem(name, triggerTime, TimerType.Once)
            {
                Metadata = metadata ?? new Dictionary<string, string>()
            };

            _timers.Add(timer);
            Save();

            _logger.Info("Timer added: {0} ({1}), type={2}, triggerAt={3}", name, timer.Id, TimerType.Once, triggerTime);

            return timer;
        }
    }

    /// <summary>
    /// Creates a recurring timer that triggers at the specified interval.
    /// </summary>
    /// <param name="name">The name of the timer.</param>
    /// <param name="interval">The interval between triggers.</param>
    /// <param name="startTime">The optional start time (defaults to now + interval).</param>
    /// <param name="maxTriggers">Optional maximum number of triggers.</param>
    /// <param name="metadata">Optional metadata for the timer.</param>
    /// <returns>The created timer item.</returns>
    public TimerItem CreateRecurring(string name, TimeSpan interval, DateTime? startTime = null, int? maxTriggers = null, Dictionary<string, string>? metadata = null)
    {
        lock (_lock)
        {
            var triggerTime = startTime ?? DateTime.Now.Add(interval);

            var timer = new TimerItem(name, triggerTime, TimerType.Recurring)
            {
                Interval = interval,
                MaxTriggers = maxTriggers,
                Metadata = metadata ?? new Dictionary<string, string>()
            };

            _timers.Add(timer);
            Save();

            _logger.Info("Timer added: {0} ({1}), type={2}, triggerAt={3}", name, timer.Id, TimerType.Recurring, triggerTime);

            return timer;
        }
    }

    /// <summary>
    /// Gets a timer by its ID.
    /// </summary>
    /// <param name="timerId">The ID of the timer to retrieve.</param>
    /// <returns>The timer item if found; otherwise, null.</returns>
    public TimerItem? Get(Guid timerId)
    {
        lock (_lock)
        {
            return _timers.FirstOrDefault(t => t.Id == timerId);
        }
    }

    /// <summary>
    /// Gets all timers, optionally filtered by status.
    /// </summary>
    /// <param name="status">The status to filter by (null for all timers).</param>
    /// <returns>A list of timer items.</returns>
    public List<TimerItem> GetAll(TimerStatus? status = null)
    {
        lock (_lock)
        {
            if (status == null)
                return _timers.ToList();

            return _timers.Where(t => t.Status == status).ToList();
        }
    }

    /// <summary>
    /// Gets all active timers ordered by trigger time.
    /// </summary>
    /// <returns>A list of active timer items.</returns>
    public List<TimerItem> GetActive()
    {
        lock (_lock)
        {
            return _timers
                .Where(t => t.Status == TimerStatus.Active)
                .OrderBy(t => t.TriggerTime)
                .ToList();
        }
    }

    /// <summary>
    /// Checks whether there are any timers that should trigger.
    /// </summary>
    /// <returns>True if there are pending timers; otherwise, false.</returns>
    public bool HasPendingTimers()
    {
        lock (_lock)
        {
            int pendingCount = _timers.Count(t => t.Status == TimerStatus.Active && t.ShouldTrigger());
            _logger.Debug("Checking pending timers: {0} pending", pendingCount);
            return pendingCount > 0;
        }
    }

    /// <summary>
    /// Gets all timers that should trigger, ordered by trigger time.
    /// </summary>
    /// <returns>A list of timer items ready to trigger.</returns>
    public List<TimerItem> GetPendingTimers()
    {
        lock (_lock)
        {
            return _timers
                .Where(t => t.ShouldTrigger())
                .OrderBy(t => t.TriggerTime)
                .ToList();
        }
    }

    /// <summary>
    /// Pauses a timer by its ID.
    /// </summary>
    /// <param name="timerId">The ID of the timer to pause.</param>
    public void Pause(Guid timerId)
    {
        lock (_lock)
        {
            var timer = _timers.FirstOrDefault(t => t.Id == timerId);
            if (timer != null)
            {
                timer.Pause();
                Save();
                _logger.Debug("Timer paused: {0} ({1})", timer.Name, timer.Id);
            }
        }
    }

    /// <summary>
    /// Resumes a paused timer by its ID.
    /// </summary>
    /// <param name="timerId">The ID of the timer to resume.</param>
    public void Resume(Guid timerId)
    {
        lock (_lock)
        {
            var timer = _timers.FirstOrDefault(t => t.Id == timerId);
            if (timer != null)
            {
                timer.Resume();
                Save();
                _logger.Debug("Timer resumed: {0} ({1})", timer.Name, timer.Id);
            }
        }
    }

    /// <summary>
    /// Cancels a timer by its ID.
    /// </summary>
    /// <param name="timerId">The ID of the timer to cancel.</param>
    public void Cancel(Guid timerId)
    {
        lock (_lock)
        {
            var timer = _timers.FirstOrDefault(t => t.Id == timerId);
            if (timer != null)
            {
                timer.Cancel();
                Save();
            }
        }
    }

    /// <summary>
    /// Resets a timer by its ID to its initial state.
    /// </summary>
    /// <param name="timerId">The ID of the timer to reset.</param>
    public void Reset(Guid timerId)
    {
        lock (_lock)
        {
            var timer = _timers.FirstOrDefault(t => t.Id == timerId);
            if (timer != null)
            {
                timer.Reset();
                Save();
            }
        }
    }

    /// <summary>
    /// Updates the trigger time of a timer.
    /// </summary>
    /// <param name="timerId">The ID of the timer.</param>
    /// <param name="newTime">The new trigger time.</param>
    /// <returns>True if the trigger time was updated; otherwise, false.</returns>
    public bool UpdateTriggerTime(Guid timerId, DateTime newTime)
    {
        lock (_lock)
        {
            var timer = _timers.FirstOrDefault(t => t.Id == timerId);
            if (timer != null && (timer.Status == TimerStatus.Active || timer.Status == TimerStatus.Paused))
            {
                timer.TriggerTime = newTime;
                Save();
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Deletes a timer by its ID.
    /// </summary>
    /// <param name="timerId">The ID of the timer to delete.</param>
    /// <returns>True if the timer was deleted; otherwise, false.</returns>
    public bool Delete(Guid timerId)
    {
        lock (_lock)
        {
            var timer = _timers.FirstOrDefault(t => t.Id == timerId);
            if (timer != null)
            {
                _timers.Remove(timer);
                Save();
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Clears all timers or timers with a specific status.
    /// </summary>
    /// <param name="status">The status to clear (null to clear all timers).</param>
    public void Clear(TimerStatus? status = null)
    {
        lock (_lock)
        {
            if (status == null)
            {
                _timers.Clear();
            }
            else
            {
                _timers = _timers.Where(t => t.Status != status).ToList();
            }

            Save();
        }
    }

    /// <summary>
    /// Processes all timers that should trigger and returns triggered timer information.
    /// </summary>
    /// <returns>A list of triggered timer callback info.</returns>
    public List<TimerItem> Tick()
    {
        lock (_lock)
        {
            var triggered = new List<TimerItem>();

            foreach (var timer in _timers.Where(t => t.ShouldTrigger()).ToList())
            {
                timer.Trigger();
                triggered.Add(timer);

                _logger.Info("Timer triggered: {0} ({1}), timesTriggered={2}", timer.Name, timer.Id, timer.TimesTriggered);

                if (timer.Type == TimerType.Once || (timer.MaxTriggers.HasValue && timer.TimesTriggered >= timer.MaxTriggers.Value))
                {
                    _logger.Debug("One-shot timer completed: {0} ({1})", timer.Name, timer.Id);
                }

                var info = new TimerCallbackInfo
                {
                    TimerId = timer.Id,
                    TimerName = timer.Name,
                    TimesTriggered = timer.TimesTriggered,
                    Metadata = timer.Metadata
                };

                try
                {
                    OnTimerTriggered?.Invoke(info);
                }
                catch
                {
                }
            }

            if (triggered.Count > 0)
            {
                Save();
            }

            return triggered;
        }
    }

    /// <summary>
    /// Gets statistics about the timer system.
    /// </summary>
    /// <returns>A TimerStatistics object with counts for each status.</returns>
    public TimerStatistics GetStatistics()
    {
        lock (_lock)
        {
            var active = _timers.Where(t => t.Status == TimerStatus.Active).ToList();

            return new TimerStatistics
            {
                Total = _timers.Count,
                Active = active.Count,
                Paused = _timers.Count(t => t.Status == TimerStatus.Paused),
                Triggered = _timers.Count(t => t.Status == TimerStatus.Triggered),
                Cancelled = _timers.Count(t => t.Status == TimerStatus.Cancelled),
                NextTrigger = active.MinBy(t => t.TriggerTime)?.TriggerTime
            };
        }
    }

    /// <summary>
    /// Gets the next timer that should trigger.
    /// </summary>
    /// <returns>The next timer that should trigger, or null if none.</returns>
    public TimerItem? GetNextTimer()
    {
        lock (_lock)
        {
            return _timers
                .Where(t => t.Status == TimerStatus.Active)
                .OrderBy(t => t.TriggerTime)
                .FirstOrDefault();
        }
    }
}
