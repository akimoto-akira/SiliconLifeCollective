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

public static class MainLoop
{
    private static readonly List<TickObject> _tickObjects = [];
    private static readonly Dictionary<TickObject, int> _consecutiveTimeoutCounts = [];
    private static readonly Dictionary<TickObject, DateTime> _circuitBreakerResetTimes = [];
    private static readonly List<Action<TimeSpan>> _preTickCallbacks = [];
    private static readonly List<Action<TimeSpan>> _postTickCallbacks = [];
    private static CancellationTokenSource _mainCts = new();
    private static readonly CancellationTokenSource _watchdogCts = new();
    private static readonly object _lock = new();
    private static Thread? _thread;
    private static Thread? _watchdogThread;
    private static volatile bool _isRunning;
    private static long _lastHeartbeatTicks = DateTime.MinValue.Ticks;
    private static ConfigDataBase? _config;
    private static SiliconBeingManager _beingManager = new();

    /// <summary>
    /// Gets the singleton <see cref="SiliconBeingManager"/> instance.
    /// Not automatically registered to the tick loop.
    /// </summary>
    public static SiliconBeingManager BeingManager => _beingManager;

    public static bool IsRunning => _isRunning;

    /// <summary>
    /// Update the watchdog heartbeat timestamp.
    /// Called by long-running operations to prevent watchdog false-positives.
    /// </summary>
    public static void UpdateHeartbeat()
    {
        Interlocked.Exchange(ref _lastHeartbeatTicks, DateTime.UtcNow.Ticks);
    }

    public static void SetConfig(ConfigDataBase config)
    {
        _config = config;
    }

    public static void Register(TickObject tickObject)
    {
        lock (_lock)
        {
            if (!_tickObjects.Contains(tickObject))
            {
                _tickObjects.Add(tickObject);
                _tickObjects.Sort((a, b) => a.Priority.CompareTo(b.Priority));
            }
        }
    }

    public static void Unregister(TickObject tickObject)
    {
        lock (_lock)
        {
            _tickObjects.Remove(tickObject);
            _consecutiveTimeoutCounts.Remove(tickObject);
            _circuitBreakerResetTimes.Remove(tickObject);
        }
    }

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

    public static void UnregisterPreTickCallback(Action<TimeSpan> callback)
    {
        lock (_lock)
        {
            _preTickCallbacks.Remove(callback);
        }
    }

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

    public static void UnregisterPostTickCallback(Action<TimeSpan> callback)
    {
        lock (_lock)
        {
            _postTickCallbacks.Remove(callback);
        }
    }

    public static void Start()
    {
        if (_isRunning)
        {
            return;
        }

        _isRunning = true;
        Interlocked.Exchange(ref _lastHeartbeatTicks, DateTime.UtcNow.Ticks);

        _thread = new Thread(Run)
        {
            IsBackground = true,
            Name = "MainLoop"
        };
        _thread.Start();

        _watchdogThread = new Thread(WatchdogRun)
        {
            IsBackground = true,
            Name = "MainLoop-Watchdog"
        };
        _watchdogThread.Start();
    }

    public static void Stop()
    {
        if (!_isRunning)
        {
            return;
        }

        _isRunning = false;
        _mainCts.Cancel();
        _watchdogCts.Cancel();

        _watchdogThread?.Join(1000);
        _thread?.Join(1000);
    }

    private static void Run()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        TimeSpan lastTickTime = TimeSpan.Zero;

        while (!_mainCts.Token.IsCancellationRequested)
        {
            TimeSpan currentTickTime = stopwatch.Elapsed;
            TimeSpan deltaTime = currentTickTime - lastTickTime;
            lastTickTime = currentTickTime;

            Interlocked.Exchange(ref _lastHeartbeatTicks, DateTime.UtcNow.Ticks);
            ExecuteTick(deltaTime);
        }

        stopwatch.Stop();
    }

    /// <summary>
    /// Watchdog: monitors MainLoop thread health.
    /// If the main loop stops updating heartbeat, it is considered hung and will be restarted.
    /// </summary>
    private static void WatchdogRun()
    {
        TimeSpan watchdogTimeout = _config?.WatchdogTimeout ?? TimeSpan.FromSeconds(10);

        while (!_watchdogCts.Token.IsCancellationRequested)
        {
            Thread.Sleep(watchdogTimeout);

            if (!_isRunning)
            {
                continue;
            }

            if (_thread is null || !_thread.IsAlive)
            {
                // Main thread is dead, restart it
                RestartMainThread();
            }
            else if ((DateTime.UtcNow - new DateTime(Interlocked.Read(ref _lastHeartbeatTicks))) > watchdogTimeout)
            {
                // Main thread is hung, kill and restart
                try { _thread.Interrupt(); } catch { }
                if (_thread.Join(TimeSpan.FromSeconds(1)))
                {
                    RestartMainThread();
                }
            }
        }
    }

    private static void RestartMainThread()
    {
        _mainCts.Cancel();

        lock (_lock)
        {
            _consecutiveTimeoutCounts.Clear();
            _circuitBreakerResetTimes.Clear();
        }

        _mainCts.Dispose();
        var newCts = new CancellationTokenSource();
        _mainCts = newCts;

        _thread = new Thread(Run)
        {
            IsBackground = true,
            Name = "MainLoop"
        };
        _lastHeartbeatTicks = DateTime.UtcNow.Ticks;
        _thread.Start();
    }

    private static void ExecuteTick(TimeSpan deltaTime)
    {
        TimeSpan timeout = _config?.TickTimeout ?? TimeSpan.FromSeconds(1);
        int maxTimeoutCount = _config?.MaxTimeoutCount ?? 3;

        lock (_lock)
        {
            TickObject[] tickObjectsCopy = [.. _tickObjects];
            Action<TimeSpan>[] preCallbacksCopy = [.. _preTickCallbacks];
            Action<TimeSpan>[] postCallbacksCopy = [.. _postTickCallbacks];

            foreach (Action<TimeSpan> callback in preCallbacksCopy)
            {
                ExecuteWithTimeout(callback, deltaTime, timeout);
            }

            // Tick BeingManager directly, no circuit breaker protection
            // Heartbeat is updated by SiliconBeingManager before each being
            _beingManager?.Tick(deltaTime);

            foreach (TickObject tickObject in tickObjectsCopy)
            {
                // Circuit breaker: skip if in cooldown
                if (_circuitBreakerResetTimes.TryGetValue(tickObject, out DateTime resetTime)
                    && DateTime.Now < resetTime)
                {
                    continue;
                }

                if (_circuitBreakerResetTimes.Remove(tickObject))
                {
                    _consecutiveTimeoutCounts.Remove(tickObject);
                }

                if (ExecuteWithTimeout(tickObject.Tick, deltaTime, timeout))
                {
                    _consecutiveTimeoutCounts[tickObject] = 0;
                }
                else
                {
                    _consecutiveTimeoutCounts.TryGetValue(tickObject, out int count);
                    _consecutiveTimeoutCounts[tickObject] = count + 1;

                    if (_consecutiveTimeoutCounts[tickObject] >= maxTimeoutCount)
                    {
                        _circuitBreakerResetTimes[tickObject] = DateTime.Now + TimeSpan.FromMinutes(1);
                        _consecutiveTimeoutCounts[tickObject] = 0;
                    }
                }
            }

            foreach (Action<TimeSpan> callback in postCallbacksCopy)
            {
                ExecuteWithTimeout(callback, deltaTime, timeout);
            }
        }
    }

    /// <summary>
    /// Execute action on a temporary thread with timeout.
    /// Returns true if completed within timeout, false if timed out.
    /// </summary>
    private static bool ExecuteWithTimeout(Action<TimeSpan> action, TimeSpan deltaTime, TimeSpan timeout)
    {
        bool completed = false;
        Thread? worker = null;

        try
        {
            worker = new Thread(() =>
            {
                try
                {
                    action(deltaTime);
                    completed = true;
                }
                catch (ThreadAbortException)
                {
                    // Thread was killed due to timeout
                }
                catch
                {
                }
            })
            {
                IsBackground = true
            };

            worker.Start();

            if (worker.Join(timeout))
            {
                return true;
            }

            worker.Interrupt();
            if (!worker.Join(TimeSpan.FromMilliseconds(200)))
            {
                try
                {
                    worker.Abort();
                }
                catch (PlatformNotSupportedException)
                {
                    // .NET Core+ does not support Abort; thread must be cancelled cooperatively via Interrupt/CancellationToken
                }

                if (!worker.Join(TimeSpan.FromMilliseconds(500)))
                {
                    // Watchdog: thread is still alive, considered out of control
                    // TODO: Log warning
                }
            }

            return false;
        }
        catch (ThreadInterruptedException)
        {
            // Main thread was interrupted (e.g., by watchdog), worker may still be running fine
            return false;
        }
    }
}
