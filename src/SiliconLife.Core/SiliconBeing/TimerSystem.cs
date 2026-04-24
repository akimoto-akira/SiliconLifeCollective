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
    /// The timer triggers repeatedly based on calendar conditions.
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
/// Represents the execution state of a timer for step-by-step processing.
/// </summary>
public enum TimerExecutionState
{
    /// <summary>
    /// Not started.
    /// </summary>
    Idle,

    /// <summary>
    /// Start notification sent.
    /// </summary>
    Started,

    /// <summary>
    /// Tool loop in progress.
    /// </summary>
    Executing,

    /// <summary>
    /// Successfully completed.
    /// </summary>
    Completed,

    /// <summary>
    /// Execution failed.
    /// </summary>
    Failed
}

/// <summary>
/// Resolves calendar-specific component conditions to the next DateTime
/// that satisfies those conditions, starting from the given anchor time.
/// </summary>
/// <param name="anchor">The time after which to find the next occurrence</param>
/// <param name="calendarId">The calendar system identifier (e.g., "chinese_lunar", "gregorian")</param>
/// <param name="componentConditions">
/// Calendar component conditions keyed by component ID.
/// Both date-level components (e.g., "month", "day", "isLeap", "era") and
/// time-level components ("hour", "minute", "second") are allowed.
/// The resolver separates them internally: date components for day-by-day matching,
/// time components for constructing the final DateTime.
/// Example: { "month": 1, "day": 1, "hour": 8 } means "1st day of 1st month at 08:00".
/// </param>
/// <returns>The next DateTime that matches the conditions, or null if unresolvable</returns>
public delegate DateTime? CalendarNextOccurrenceResolver(
    DateTime anchor,
    string calendarId,
    Dictionary<string, int> componentConditions);

/// <summary>
/// Determines whether a timer should be considered pending (ready to trigger).
/// Used by HasPendingTimers and GetPendingTimers to decide which timers qualify.
/// </summary>
/// <param name="timer">The timer item to evaluate</param>
/// <returns>True if the timer is pending; otherwise, false</returns>
public delegate bool TimerPendingChecker(TimerItem timer);

/// <summary>
/// Converts a DateTime to a calendar-specific component representation.
/// Used to enrich TimerCallbackInfo with calendar context.
/// </summary>
/// <param name="dateTime">The DateTime to convert</param>
/// <param name="calendarId">The calendar system identifier</param>
/// <returns>Component values keyed by component ID, or null if the calendar is unavailable</returns>
public delegate Dictionary<string, int>? CalendarDateTimeConverter(
    DateTime dateTime,
    string calendarId);

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
    /// Gets or sets the calendar system identifier for this timer.
    /// Required. Examples: "gregorian", "chinese_lunar", "islamic", "sexagenary", "interval".
    /// </summary>
    public string CalendarId { get; set; } = "gregorian";

    /// <summary>
    /// Gets or sets the calendar component conditions that define when this timer triggers.
    /// Keyed by component ID (e.g., "month", "day", "hour") with required values.
    /// Both date-level components ("month", "day", "isLeap", "era", etc.) and
    /// time-level components ("hour", "minute", "second") are allowed.
    /// The resolver separates them internally: date components for day-by-day matching,
    /// time components for constructing the final DateTime.
    /// Example: { "month": 1, "day": 15, "hour": 8 } = "15th of the 1st month at 08:00".
    /// </summary>
    public Dictionary<string, int> CalendarConditions { get; set; } = new();

    /// <summary>
    /// Gets or sets the number of times the timer has been triggered.
    /// </summary>
    public int TimesTriggered { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the timer was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the timer was last triggered.
    /// Persisted to storage. Used to prevent duplicate triggers within the same cycle.
    /// </summary>
    public DateTime? LastTriggeredAt { get; set; }

    /// <summary>
    /// Gets or sets additional metadata for the timer.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the current execution state for step-by-step processing.
    /// </summary>
    public TimerExecutionState ExecutionState { get; set; } = TimerExecutionState.Idle;

    /// <summary>
    /// Gets or sets the current execution step number.
    /// </summary>
    public int CurrentStep { get; set; } = 0;

    /// <summary>
    /// Gets or sets the maximum steps allowed (prevent infinite loops).
    /// </summary>
    public int MaxSteps { get; set; } = 20;

    /// <summary>
    /// Gets or sets the file path of the current execution context (optional, for reference).
    /// </summary>
    public string? CurrentExecutionFile { get; set; }

    /// <summary>
    /// Gets all execution history files for this timer.
    /// Returns a list of TimerExecution objects sorted by triggered time (newest first).
    /// </summary>
    public List<TimerExecution> GetExecutionHistory()
    {
        var result = new List<TimerExecution>();

        if (string.IsNullOrEmpty(CurrentExecutionFile))
            return result;

        var execDir = Path.GetDirectoryName(CurrentExecutionFile);
        if (string.IsNullOrEmpty(execDir) || !Directory.Exists(execDir))
            return result;

        var files = Directory.GetFiles(execDir, "*.json")
            .OrderByDescending(f => new FileInfo(f).CreationTime);

        foreach (var file in files)
        {
            try
            {
                var execution = TimerExecution.Load(file);
                if (execution != null && execution.TimerId == Id)
                {
                    result.Add(execution);
                }
            }
            catch
            {
                // Skip invalid files
            }
        }

        return result;
    }

    /// <summary>
    /// Gets a specific execution by ID.
    /// </summary>
    /// <param name="executionId">The execution ID to find</param>
    /// <returns>The TimerExecution if found, otherwise null</returns>
    public TimerExecution? GetExecution(Guid executionId)
    {
        if (string.IsNullOrEmpty(CurrentExecutionFile))
            return null;

        var execDir = Path.GetDirectoryName(CurrentExecutionFile);
        if (string.IsNullOrEmpty(execDir) || !Directory.Exists(execDir))
            return null;

        // Try direct lookup by ExecutionId-based filename first
        var execFile = Path.Combine(execDir, $"{executionId}.json");
        var execution = TimerExecution.Load(execFile);
        if (execution != null && execution.TimerId == Id)
            return execution;

        // Fallback: scan all files for matching ExecutionId (supports old format filenames)
        var files = Directory.GetFiles(execDir, "*.json");
        foreach (var file in files)
        {
            try
            {
                var ex = TimerExecution.Load(file);
                if (ex != null && ex.ExecutionId == executionId && ex.TimerId == Id)
                    return ex;
            }
            catch
            {
                // Skip invalid files
            }
        }

        return null;
    }

    /// <summary>
    /// Initializes a new instance of the TimerItem class.
    /// </summary>
    public TimerItem()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    /// <summary>
    /// Determines whether the timer should trigger now.
    /// Three conditions must all be satisfied:
    /// 1. Status is Active
    /// 2. Current time has reached or passed TriggerTime
    /// 3. The current cycle has not already been triggered (LastTriggeredAt &lt; TriggerTime)
    /// </summary>
    public bool ShouldTrigger()
    {
        if (Status != TimerStatus.Active)
            return false;

        if (DateTime.Now < TriggerTime)
            return false;

        if (LastTriggeredAt.HasValue && LastTriggeredAt.Value >= TriggerTime)
            return false;

        return true;
    }

    /// <summary>
    /// Triggers the timer and updates its state.
    /// Does NOT advance TriggerTime for recurring timers �?
    /// that is handled by TimerSystem.Tick() via the calendar resolver.
    /// </summary>
    public void Trigger()
    {
        LastTriggeredAt = DateTime.Now;
        TimesTriggered++;

        if (Type == TimerType.Once)
        {
            Status = TimerStatus.Triggered;
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

        TimeSpan diff = TriggerTime - DateTime.Now;
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

    /// <summary>
    /// Gets or sets the count of timers grouped by calendar ID.
    /// </summary>
    public Dictionary<string, int> ByCalendar { get; set; } = new();
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
    /// Gets or sets the calendar system identifier of the triggered timer.
    /// </summary>
    public string CalendarId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of times the timer has been triggered.
    /// </summary>
    public int TimesTriggered { get; set; }

    /// <summary>
    /// Gets or sets the metadata associated with the timer.
    /// Calendar component context is stored under "calendar.{componentId}" keys.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = new();
}

/// <summary>
/// System for managing timers with calendar-based scheduling.
/// All timers use calendar component matching for scheduling,
/// including Gregorian timers (calendarId="gregorian") and interval timers (calendarId="interval").
/// </summary>
public sealed class TimerSystem
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<TimerSystem>();
    private readonly SiliconBeingBase _owner;
    private readonly IStorage _storage;
    private readonly string _storageKey;
    private readonly object _lock = new();

    private readonly CalendarNextOccurrenceResolver _calendarResolver;
    private readonly CalendarDateTimeConverter _calendarConverter;
    private readonly TimerPendingChecker _pendingChecker;

    private List<TimerItem> _timers = new();
    private readonly List<Action<TimerCallbackInfo>> _callbacks = new();

    /// <summary>
    /// Gets the owner being's GUID (computed in real-time from the owner)
    /// </summary>
    public Guid OwnerId => _owner.Id;

    /// <summary>
    /// Gets the name of the owner being (computed in real-time from the owner)
    /// </summary>
    public string OwnerName => _owner.Name;

    /// <summary>
    /// Gets whether the owner is a curator (computed in real-time from the owner)
    /// </summary>
    public bool IsCurator => _owner.IsCurator;

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
    /// Initializes a new instance of the TimerSystem class with calendar-based scheduling support.
    /// Each being holds its own TimerSystem instance; the owner reference enables real-time
    /// identity queries (OwnerId, OwnerName, IsCurator) without duplicating state.
    /// </summary>
    /// <param name="owner">The silicon being that owns this TimerSystem</param>
    /// <param name="storage">The storage to use for persisting timers.</param>
    /// <param name="calendarResolver">Resolves calendar conditions to next occurrence DateTime</param>
    /// <param name="calendarConverter">Converts DateTime to calendar component representation</param>
    /// <param name="pendingChecker">Determines whether a timer is pending (ready to trigger)</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    public TimerSystem(
        SiliconBeingBase owner,
        IStorage storage,
        CalendarNextOccurrenceResolver calendarResolver,
        CalendarDateTimeConverter calendarConverter,
        TimerPendingChecker pendingChecker)
    {
        _owner = owner ?? throw new ArgumentNullException(nameof(owner));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _calendarResolver = calendarResolver ?? throw new ArgumentNullException(nameof(calendarResolver));
        _calendarConverter = calendarConverter ?? throw new ArgumentNullException(nameof(calendarConverter));
        _pendingChecker = pendingChecker ?? throw new ArgumentNullException(nameof(pendingChecker));
        _storageKey = "timers";

        _logger.Info(_owner.Id, "TimerSystem created for being {0} ({1})", owner.Name, owner.Id);

        Load();
    }

    private void Load()
    {
        try
        {
            List<TimerItem>? timers = _storage.Read<List<TimerItem>>(_storageKey);
            _timers = timers ?? new List<TimerItem>();

            foreach (TimerItem timer in _timers.Where(t => t.Status == TimerStatus.Triggered))
            {
                if (timer.Type == TimerType.Recurring)
                {
                    timer.Status = TimerStatus.Active;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Warn(_owner.Id, "Failed to load timers from storage", ex);
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
            _storage.Write(_storageKey, _timers);
        }
        catch (Exception ex)
        {
            _logger.Error(_owner.Id, "Failed to save timers to storage", ex);
        }
    }

    /// <summary>
    /// Creates a one-shot timer that triggers once when the specified calendar conditions are met.
    /// </summary>
    /// <param name="name">The name of the timer.</param>
    /// <param name="calendarId">The calendar system identifier.</param>
    /// <param name="componentConditions">
    /// Calendar component conditions (e.g., { "month": 1, "day": 1, "hour": 8 } for "1st of 1st month at 08:00").
    /// </param>
    /// <param name="metadata">Optional metadata for the timer.</param>
    /// <returns>The created timer item, or null if the resolver cannot compute the first trigger time.</returns>
    public TimerItem? CreateOneShot(string name, string calendarId, Dictionary<string, int> componentConditions, Dictionary<string, string>? metadata = null)
    {
        lock (_lock)
        {
            DateTime? triggerTime = _calendarResolver(DateTime.Now, calendarId, componentConditions);
            if (!triggerTime.HasValue)
            {
                _logger.Warn(_owner.Id, "Cannot create one-shot timer: resolver returned null for calendar={0}", calendarId);
                return null;
            }

            TimerItem timer = new TimerItem()
            {
                Name = name,
                TriggerTime = triggerTime.Value,
                Type = TimerType.Once,
                CalendarId = calendarId,
                CalendarConditions = componentConditions,
                Metadata = metadata ?? new Dictionary<string, string>()
            };

            _timers.Add(timer);
            Save();

            _logger.Info(_owner.Id, "Timer added: {0} ({1}), type={2}, calendar={3}, triggerAt={4}", name, timer.Id, TimerType.Once, calendarId, triggerTime.Value);

            return timer;
        }
    }

    /// <summary>
    /// Creates a recurring timer that triggers when the specified calendar components match the conditions.
    /// After each trigger, the next occurrence is computed via the calendar resolver.
    /// </summary>
    /// <param name="name">The name of the timer.</param>
    /// <param name="calendarId">The calendar system identifier.</param>
    /// <param name="componentConditions">
    /// Calendar component conditions (e.g., { "month": 1, "day": 1, "hour": 8 } for "1st of 1st month at 08:00").
    /// </param>
    /// <param name="metadata">Optional metadata for the timer.</param>
    /// <returns>The created timer item, or null if the resolver cannot compute the first trigger time.</returns>
    public TimerItem? CreateRecurring(string name, string calendarId, Dictionary<string, int> componentConditions, Dictionary<string, string>? metadata = null)
    {
        lock (_lock)
        {
            DateTime? triggerTime = _calendarResolver(DateTime.Now, calendarId, componentConditions);
            if (!triggerTime.HasValue)
            {
                _logger.Warn(_owner.Id, "Cannot create recurring timer: resolver returned null for calendar={0}", calendarId);
                return null;
            }

            TimerItem timer = new TimerItem()
            {
                Name = name,
                TriggerTime = triggerTime.Value,
                Type = TimerType.Recurring,
                CalendarId = calendarId,
                CalendarConditions = componentConditions,
                Metadata = metadata ?? new Dictionary<string, string>()
            };

            _timers.Add(timer);
            Save();

            _logger.Info(_owner.Id, "Timer added: {0} ({1}), type={2}, calendar={3}, triggerAt={4}", name, timer.Id, TimerType.Recurring, calendarId, triggerTime.Value);

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
            int pendingCount = _timers.Count(t => _pendingChecker(t));
            _logger.Debug(_owner.Id, "Checking pending timers: {0} pending", pendingCount);
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
                .Where(t => _pendingChecker(t))
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
            TimerItem? timer = _timers.FirstOrDefault(t => t.Id == timerId);
            if (timer != null)
            {
                timer.Pause();
                Save();
                _logger.Debug(_owner.Id, "Timer paused: {0} ({1})", timer.Name, timer.Id);
            }
        }
    }

    /// <summary>
    /// Resumes a paused timer by its ID.
    /// If the timer's TriggerTime has passed while paused,
    /// the calendar resolver is used to compute the next valid occurrence.
    /// </summary>
    /// <param name="timerId">The ID of the timer to resume.</param>
    public void Resume(Guid timerId)
    {
        lock (_lock)
        {
            TimerItem? timer = _timers.FirstOrDefault(t => t.Id == timerId);
            if (timer == null || timer.Status != TimerStatus.Paused)
                return;

            timer.Status = TimerStatus.Active;

            if (timer.TriggerTime < DateTime.Now && timer.Type == TimerType.Recurring)
            {
                DateTime? nextTime = _calendarResolver(DateTime.Now, timer.CalendarId, timer.CalendarConditions);
                if (nextTime.HasValue)
                {
                    timer.TriggerTime = nextTime.Value;
                }
                else
                {
                    timer.Status = TimerStatus.Cancelled;
                    _logger.Warn(_owner.Id, "Timer cancelled on resume: resolver returned null for {0}", timer.Name);
                }
            }

            Save();
            _logger.Debug(_owner.Id, "Timer resumed: {0} ({1})", timer.Name, timer.Id);
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
            TimerItem? timer = _timers.FirstOrDefault(t => t.Id == timerId);
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
            TimerItem? timer = _timers.FirstOrDefault(t => t.Id == timerId);
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
            TimerItem? timer = _timers.FirstOrDefault(t => t.Id == timerId);
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
            TimerItem? timer = _timers.FirstOrDefault(t => t.Id == timerId);
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
    /// For recurring timers, advances TriggerTime via the calendar resolver after triggering.
    /// </summary>
    /// <returns>A list of triggered timer items.</returns>
    public List<TimerItem> Tick()
    {
        lock (_lock)
        {
            List<TimerItem> triggered = new List<TimerItem>();

            foreach (TimerItem timer in _timers.Where(t => _pendingChecker(t)).ToList())
            {
                timer.Trigger();
                triggered.Add(timer);

                _logger.Info(_owner.Id, "Timer triggered: {0} ({1}), timesTriggered={2}", timer.Name, timer.Id, timer.TimesTriggered);

                if (timer.Type == TimerType.Once)
                {
                    _logger.Debug(_owner.Id, "One-shot timer completed: {0} ({1})", timer.Name, timer.Id);
                }

                TimerCallbackInfo info = new TimerCallbackInfo
                {
                    TimerId = timer.Id,
                    TimerName = timer.Name,
                    CalendarId = timer.CalendarId,
                    TimesTriggered = timer.TimesTriggered,
                    Metadata = timer.Metadata
                };

                Dictionary<string, int>? calendarComponents = _calendarConverter(DateTime.Now, timer.CalendarId);
                if (calendarComponents != null)
                {
                    info.Metadata["calendarId"] = timer.CalendarId;
                    foreach (KeyValuePair<string, int> kv in calendarComponents)
                    {
                        info.Metadata[$"calendar.{kv.Key}"] = kv.Value.ToString();
                    }
                }

                try
                {
                    OnTimerTriggered?.Invoke(info);
                }
                catch
                {
                }
            }

            foreach (TimerItem timer in triggered)
            {
                if (timer.Type == TimerType.Recurring && timer.Status == TimerStatus.Active)
                {
                    DateTime? nextTime = _calendarResolver(timer.TriggerTime, timer.CalendarId, timer.CalendarConditions);
                    if (nextTime.HasValue)
                    {
                        timer.TriggerTime = nextTime.Value;
                        _logger.Debug(_owner.Id, "Timer advanced: {0} next trigger at {1}", timer.Name, nextTime.Value);
                    }
                    else
                    {
                        timer.Status = TimerStatus.Cancelled;
                        _logger.Warn(_owner.Id, "Timer cancelled: resolver returned null for {0}", timer.Name);
                    }
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
            List<TimerItem> active = _timers.Where(t => t.Status == TimerStatus.Active).ToList();

            return new TimerStatistics
            {
                Total = _timers.Count,
                Active = active.Count,
                Paused = _timers.Count(t => t.Status == TimerStatus.Paused),
                Triggered = _timers.Count(t => t.Status == TimerStatus.Triggered),
                Cancelled = _timers.Count(t => t.Status == TimerStatus.Cancelled),
                NextTrigger = active.MinBy(t => t.TriggerTime)?.TriggerTime,
                ByCalendar = _timers
                    .GroupBy(t => t.CalendarId)
                    .ToDictionary(g => g.Key, g => g.Count())
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

/// <summary>
/// Represents a single timer execution with full context.
/// Reuses ChatMessage structure for message history.
/// </summary>
public sealed class TimerExecution
{
    /// <summary>
    /// Unique execution identifier.
    /// </summary>
    public Guid ExecutionId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Associated timer ID.
    /// </summary>
    public Guid TimerId { get; set; }

    /// <summary>
    /// Timer name (for display).
    /// </summary>
    public string TimerName { get; set; } = "";

    /// <summary>
    /// Trigger timestamp.
    /// </summary>
    public DateTime TriggeredAt { get; set; }

    /// <summary>
    /// Completion timestamp (null if still running).
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Current execution state.
    /// </summary>
    public TimerExecutionState State { get; set; } = TimerExecutionState.Idle;

    /// <summary>
    /// Current step number.
    /// </summary>
    public int CurrentStep { get; set; } = 0;

    /// <summary>
    /// Message history (reuses ChatMessage structure).
    /// </summary>
    public List<ChatMessage> Messages { get; set; } = new();

    /// <summary>
    /// File path for persistence.
    /// </summary>
    public string FilePath { get; set; } = "";

    /// <summary>
    /// Load execution from JSON file.
    /// </summary>
    /// <param name="filePath">Path to the JSON file</param>
    /// <returns>The loaded TimerExecution, or null if file doesn't exist</returns>
    public static TimerExecution? Load(string filePath)
    {
        if (!File.Exists(filePath)) return null;
        string json = File.ReadAllText(filePath);
        var execution = JsonSerializer.Deserialize<TimerExecution>(json);
        if (execution != null)
            execution.FilePath = filePath;
        return execution;
    }

    /// <summary>
    /// Save execution to JSON file.
    /// </summary>
    public void Save()
    {
        string? directory = Path.GetDirectoryName(FilePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        string json = JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(FilePath, json);
    }
}
