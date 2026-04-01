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

using System.Diagnostics;

namespace SiliconLife.Collective;

/// <summary>
/// Main loop that drives all tick objects
/// </summary>
public static class MainLoop
{
    private static readonly List<TickObject> _tickObjects = new List<TickObject>();
    private static readonly List<Action<TimeSpan>> _preTickCallbacks = new List<Action<TimeSpan>>();
    private static readonly List<Action<TimeSpan>> _postTickCallbacks = new List<Action<TimeSpan>>();
    private static readonly Thread _thread;
    private static readonly Thread _watchdogThread;
    private static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private static readonly object _lock = new object();
    private static bool _isRunning = false;
    private static ConfigDataBase? _config;
    private static readonly object _executionLock = new object();
    private static DateTime _lastTickStartTime;
    private static int _consecutiveTimeoutCount = 0;
    private static bool _circuitBreakerTripped = false;
    private static DateTime _circuitBreakerResetTime;
    private static bool _temporarilyDisableCircuitBreaker = false;

    static MainLoop()
    {
        _thread = new Thread(Run);
        _thread.IsBackground = true;
        _thread.Name = "MainLoop";

        _watchdogThread = new Thread(WatchdogRun);
        _watchdogThread.IsBackground = true;
        _watchdogThread.Name = "MainLoopWatchdog";
    }

    /// <summary>
    /// Gets whether the main loop is running
    /// </summary>
    public static bool IsRunning => _isRunning;

    /// <summary>
    /// Gets whether the circuit breaker is tripped
    /// </summary>
    public static bool IsCircuitBreakerTripped => _circuitBreakerTripped;

    /// <summary>
    /// Temporarily disables circuit breaker for the next operation
    /// This is only valid for the next tick callback or TickObject execution
    /// </summary>
    public static void TemporarilyDisableCircuitBreaker()
    {
        _temporarilyDisableCircuitBreaker = true;
    }

    /// <summary>
    /// Manually resets the circuit breaker
    /// </summary>
    public static void ResetCircuitBreakerManually()
    {
        ResetCircuitBreaker();
    }

    /// <summary>
    /// Sets the configuration for the main loop
    /// </summary>
    /// <param name="config">The configuration data</param>
    public static void SetConfig(ConfigDataBase config)
    {
        _config = config;
    }

    /// <summary>
    /// Registers a tick object to be updated by the main loop
    /// </summary>
    /// <param name="tickObject">The tick object to register</param>
    public static void Register(TickObject tickObject)
    {
        lock (_lock)
        {
            if (!_tickObjects.Contains(tickObject))
            {
                _tickObjects.Add(tickObject);
                SortTickObjects();
            }
        }
    }

    /// <summary>
    /// Unregisters a tick object from the main loop
    /// </summary>
    /// <param name="tickObject">The tick object to unregister</param>
    public static void Unregister(TickObject tickObject)
    {
        lock (_lock)
        {
            _tickObjects.Remove(tickObject);
        }
    }

    /// <summary>
    /// Registers a pre-tick callback function (executed before TickObjects)
    /// </summary>
    /// <param name="callback">The callback function that receives deltaTime</param>
    public static void RegisterPreTickCallback(Action<TimeSpan> callback)
    {
        lock (_lock)
        {
            if (!_preTickCallbacks.Contains(callback))
            {
                _preTickCallbacks.Add(callback);
            }
        }
    }

    /// <summary>
    /// Unregisters a pre-tick callback function
    /// </summary>
    /// <param name="callback">The callback function to unregister</param>
    public static void UnregisterPreTickCallback(Action<TimeSpan> callback)
    {
        lock (_lock)
        {
            _preTickCallbacks.Remove(callback);
        }
    }

    /// <summary>
    /// Registers a post-tick callback function (executed after TickObjects)
    /// </summary>
    /// <param name="callback">The callback function that receives deltaTime</param>
    public static void RegisterPostTickCallback(Action<TimeSpan> callback)
    {
        lock (_lock)
        {
            if (!_postTickCallbacks.Contains(callback))
            {
                _postTickCallbacks.Add(callback);
            }
        }
    }

    /// <summary>
    /// Unregisters a post-tick callback function
    /// </summary>
    /// <param name="callback">The callback function to unregister</param>
    public static void UnregisterPostTickCallback(Action<TimeSpan> callback)
    {
        lock (_lock)
        {
            _postTickCallbacks.Remove(callback);
        }
    }

    /// <summary>
    /// Starts the main loop
    /// </summary>
    public static void Start()
    {
        if (_isRunning)
        {
            return;
        }

        _isRunning = true;
        _thread.Start();
        _watchdogThread.Start();
    }

    /// <summary>
    /// Stops the main loop
    /// </summary>
    public static void Stop()
    {
        if (!_isRunning)
        {
            return;
        }

        _isRunning = false;
        _cancellationTokenSource.Cancel();
        _thread.Join(1000);
        _watchdogThread.Join(1000);
    }

    /// <summary>
    /// Main loop execution method
    /// Runs at full speed, measuring time between ticks
    /// </summary>
    private static void Run()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        TimeSpan lastTickTime = stopwatch.Elapsed;

        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            // Check if circuit breaker is tripped and if it's time to reset
            if (_circuitBreakerTripped && DateTime.Now > _circuitBreakerResetTime)
            {
                ResetCircuitBreaker();
            }

            // Skip execution if circuit breaker is still tripped and not temporarily disabled
            if (_circuitBreakerTripped && !_temporarilyDisableCircuitBreaker)
            {
                Thread.Sleep(100); // Wait a bit before checking again
                continue;
            }

            TimeSpan currentTickTime = stopwatch.Elapsed;
            TimeSpan deltaTime = currentTickTime - lastTickTime;
            lastTickTime = currentTickTime;

            UpdateTickObjects(deltaTime);
        }

        stopwatch.Stop();
    }

    /// <summary>
    /// Watchdog thread that monitors tick execution time
    /// </summary>
    private static void WatchdogRun()
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            try
            {
                if (_isRunning && _config != null && !_circuitBreakerTripped)
                {
                    TimeSpan timeout = _config.TickTimeout;
                    DateTime currentTime = DateTime.Now;

                    lock (_executionLock)
                    {
                        if (currentTime - _lastTickStartTime > timeout)
                        {
                            // Tick execution timed out
                            HandleTimeout();
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Watchdog should never crash
            }

            Thread.Sleep(50); // Check every 50ms
        }
    }

    /// <summary>
    /// Updates all registered tick objects
    /// </summary>
    /// <param name="deltaTime">Time elapsed since the last update</param>
    private static void UpdateTickObjects(TimeSpan deltaTime)
    {
        // Update last tick start time (without holding the lock)
        DateTime tickStart = DateTime.Now;
        lock (_executionLock)
        {
            _lastTickStartTime = tickStart;
        }

        try
        {
            List<TickObject> tickObjectsCopy;
            List<Action<TimeSpan>> preTickCallbacksCopy;
            List<Action<TimeSpan>> postTickCallbacksCopy;

            lock (_lock)
            {
                tickObjectsCopy = new List<TickObject>(_tickObjects);
                preTickCallbacksCopy = new List<Action<TimeSpan>>(_preTickCallbacks);
                postTickCallbacksCopy = new List<Action<TimeSpan>>(_postTickCallbacks);
            }

            // Track if any timeout occurred during this tick
            bool timeoutOccurred = false;

            // Execute pre-tick callbacks with timeout monitoring
            foreach (Action<TimeSpan> callback in preTickCallbacksCopy)
            {
                try
                {
                    DateTime callbackStart = DateTime.Now;
                    TimeSpan timeout = _config?.TickTimeout ?? TimeSpan.FromSeconds(5);
                    
                    // Create a task to execute the callback
                    Task task = new Task(() => callback(deltaTime));
                    task.Start();
                    
                    // Wait for the task to complete or timeout
                    if (!task.Wait(timeout))
                    {
                        // Callback timed out
                        timeoutOccurred = true;
                        HandleTimeout();
                    }
                }
                catch (Exception)
                {
                    // Exception occurred, treat as timeout
                    timeoutOccurred = true;
                    HandleTimeout();
                }
            }

            // Execute tick objects with timeout monitoring
            foreach (TickObject tickObject in tickObjectsCopy)
            {
                try
                {
                    DateTime tickObjectStart = DateTime.Now;
                    TimeSpan timeout = _config?.TickTimeout ?? TimeSpan.FromSeconds(5);
                    
                    // Create a task to execute the tick object
                    Task task = new Task(() => tickObject.Tick(deltaTime));
                    task.Start();
                    
                    // Wait for the task to complete or timeout
                    if (!task.Wait(timeout))
                    {
                        // TickObject timed out
                        timeoutOccurred = true;
                        HandleTimeout();
                    }
                }
                catch (Exception)
                {
                    // Exception occurred, treat as timeout
                    timeoutOccurred = true;
                    HandleTimeout();
                }
            }

            // Execute post-tick callbacks with timeout monitoring
            foreach (Action<TimeSpan> callback in postTickCallbacksCopy)
            {
                try
                {
                    DateTime callbackStart = DateTime.Now;
                    TimeSpan timeout = _config?.TickTimeout ?? TimeSpan.FromSeconds(5);
                    
                    // Create a task to execute the callback
                    Task task = new Task(() => callback(deltaTime));
                    task.Start();
                    
                    // Wait for the task to complete or timeout
                    if (!task.Wait(timeout))
                    {
                        // Callback timed out
                        timeoutOccurred = true;
                        HandleTimeout();
                    }
                }
                catch (Exception)
                {
                    // Exception occurred, treat as timeout
                    timeoutOccurred = true;
                    HandleTimeout();
                }
            }

            // Reset timeout count only if no timeouts occurred in this tick
            if (!timeoutOccurred)
            {
                _consecutiveTimeoutCount = 0;
            }

            // Reset temporary circuit breaker disable flag
            _temporarilyDisableCircuitBreaker = false;
        }
        catch (Exception)
        {
            // Reset timeout count even on exceptions
            _consecutiveTimeoutCount = 0;
            // Reset temporary circuit breaker disable flag
            _temporarilyDisableCircuitBreaker = false;
        }
    }

    /// <summary>
    /// Executes an action with timeout monitoring
    /// </summary>
    /// <param name="action">The action to execute</param>
    private static void ExecuteWithTimeout(Action action)
    {
        if (_circuitBreakerTripped)
        {
            return;
        }

        DateTime startTime = DateTime.Now;
        TimeSpan timeout = _config?.TickTimeout ?? TimeSpan.FromSeconds(5);

        // Create a task to execute the action
        Task task = new Task(action);
        task.Start();

        // Wait for the task to complete or timeout
        if (!task.Wait(timeout))
        {
            // Action timed out
            HandleTimeout();
        }
    }

    /// <summary>
    /// Handles a timeout event
    /// </summary>
    private static void HandleTimeout()
    {
        _consecutiveTimeoutCount++;

        if (_config != null && _consecutiveTimeoutCount >= _config.MaxTimeoutCount)
        {
            TripCircuitBreaker();
        }
    }

    /// <summary>
    /// Trips the circuit breaker
    /// </summary>
    private static void TripCircuitBreaker()
    {
        _circuitBreakerTripped = true;
        _circuitBreakerResetTime = DateTime.Now + TimeSpan.FromMinutes(1); // Reset after 1 minute
        _consecutiveTimeoutCount = 0;
    }

    /// <summary>
    /// Resets the circuit breaker
    /// </summary>
    private static void ResetCircuitBreaker()
    {
        _circuitBreakerTripped = false;
        _consecutiveTimeoutCount = 0;
    }

    /// <summary>
    /// Sorts tick objects by priority (lower value = higher priority)
    /// </summary>
    private static void SortTickObjects()
    {
        _tickObjects.Sort((a, b) => a.Priority.CompareTo(b.Priority));
    }
}
