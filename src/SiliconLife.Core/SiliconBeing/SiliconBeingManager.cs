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

using System.Diagnostics;

namespace SiliconLife.Collective;

/// <summary>
/// Manages the execution lifecycle of a single silicon being.
/// Executes Tick on a temporary thread per invocation with proper timeout handling.
/// </summary>
public class SiliconBeingRunner
{
    private readonly SiliconBeingBase _being;
    private readonly int _maxTimeoutCount;
    private readonly ConfigDataBase _config;

    private int _consecutiveTimeoutCount;
    private bool _circuitBreakerTripped;
    private DateTime _circuitBreakerResetTime;

    public Guid BeingId => _being.Id;

    public SiliconBeingRunner(SiliconBeingBase being, int maxTimeoutCount, ConfigDataBase config)
    {
        _being = being;
        _maxTimeoutCount = maxTimeoutCount;
        _config = config;
    }

    /// <summary>
    /// Execute the being's Tick on a temporary thread with timeout.
    /// Returns true if completed within timeout, false if timed out.
    /// </summary>
    public bool Execute(TimeSpan deltaTime)
    {
        TimeSpan timeout = _config?.TickTimeout ?? TimeSpan.FromSeconds(1);
        bool success = false;
        Thread? worker = null;

        try
        {
            worker = new Thread(() =>
            {
                try
                {
                    _being.Tick(deltaTime);
                    success = true;
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
                IsBackground = true,
                Name = $"SiliconBeing-{_being.Name}"
            };

            worker.Start();

            if (worker.Join(timeout))
            {
                _consecutiveTimeoutCount = 0;
                return true;
            }

            // Timeout: try to kill the thread
            KillThread(worker);
            HandleTimeout();
            return false;
        }
        catch (ThreadInterruptedException)
        {
            // Main thread was interrupted (e.g., by watchdog), worker may still be running fine
            return false;
        }
        catch
        {
            KillThread(worker);
            return false;
        }
    }

    /// <summary>
    /// Kill a thread aggressively: Interrupt → Abort → verify.
    /// </summary>
    private static void KillThread(Thread? worker)
    {
        if (worker is null || !worker.IsAlive)
        {
            return;
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
                // .NET Core+ does not support Abort; thread must be cancelled cooperatively
            }

            if (!worker.Join(TimeSpan.FromMilliseconds(500)))
            {
                // Thread is still alive, considered out of control
                // TODO: Log warning
            }
        }
    }

    /// <summary>
    /// Returns true if the circuit breaker is currently tripped (being is in cooldown).
    /// </summary>
    public bool IsCircuitBreakerTripped => _circuitBreakerTripped;

    /// <summary>
    /// Increment consecutive timeout count and trip breaker if threshold reached.
    /// </summary>
    private void HandleTimeout()
    {
        _consecutiveTimeoutCount++;

        if (_consecutiveTimeoutCount >= _maxTimeoutCount)
        {
            _circuitBreakerTripped = true;
            _circuitBreakerResetTime = DateTime.UtcNow + TimeSpan.FromMinutes(1);
            _consecutiveTimeoutCount = 0;
        }
    }

    /// <summary>
    /// Check if the circuit breaker cooldown has elapsed and reset if so.
    /// </summary>
    public void ResetIfNeeded()
    {
        if (_circuitBreakerTripped && DateTime.UtcNow > _circuitBreakerResetTime)
        {
            _circuitBreakerTripped = false;
            _consecutiveTimeoutCount = 0;
        }
    }
}

/// <summary>
/// Manages all silicon beings. Ticks them sequentially (synchronous surface).
/// Each being has its own circuit breaker and timeout handling.
/// Not automatically registered to MainLoop.
/// </summary>
public class SiliconBeingManager : TickObject
{
    private readonly List<SiliconBeingBase> _beings = [];
    private readonly Dictionary<SiliconBeingBase, SiliconBeingRunner> _runners = [];
    private readonly object _lock = new();
    private bool _managerRunning;

    public SiliconBeingManager()
        : base(TimeSpan.Zero, false)
    {
        Priority = 0;
    }

    private TimeSpan TickTimeout => Config.Instance?.Data?.TickTimeout ?? TimeSpan.FromSeconds(1);
    private int MaxTimeoutCount => Config.Instance?.Data?.MaxTimeoutCount ?? 3;

    public int BeingCount
    {
        get
        {
            lock (_lock)
            {
                return _beings.Count;
            }
        }
    }

    public void Start()
    {
        _managerRunning = true;
    }

    public void Stop()
    {
        _managerRunning = false;

        lock (_lock)
        {
            _runners.Clear();
            _beings.Clear();
        }
    }

    public void RegisterBeing(SiliconBeingBase being)
    {
        lock (_lock)
        {
            if (!_beings.Contains(being))
            {
                _beings.Add(being);
                _runners[being] = new SiliconBeingRunner(being, MaxTimeoutCount, Config.Instance?.Data!);
            }
        }
    }

    public void UnregisterBeing(SiliconBeingBase being)
    {
        lock (_lock)
        {
            _runners.Remove(being);
            _beings.Remove(being);
        }
    }

    public SiliconBeingBase? GetBeing(Guid beingId)
    {
        lock (_lock)
        {
            return _beings.FirstOrDefault(b => b.Id == beingId);
        }
    }

    public List<SiliconBeingBase> GetAllBeings()
    {
        lock (_lock)
        {
            return [.. _beings];
        }
    }

    /// <summary>
    /// Tick all beings sequentially. Curator being gets priority.
    /// Each being runs on a temporary thread with timeout + circuit breaker.
    /// </summary>
    protected override void OnTick(TimeSpan deltaTime)
    {
        List<SiliconBeingRunner> runnersCopy;
        Guid curatorGuid;

        lock (_lock)
        {
            runnersCopy = [.. _runners.Values];
            curatorGuid = Config.Instance?.Data?.CuratorGuid ?? Guid.Empty;
        }

        // Curator being first
        runnersCopy.Sort((a, b) =>
        {
            bool aIsCurator = a.BeingId == curatorGuid;
            bool bIsCurator = b.BeingId == curatorGuid;
            if (aIsCurator && !bIsCurator) return -1;
            if (!aIsCurator && bIsCurator) return 1;
            return 0;
        });

        foreach (SiliconBeingRunner runner in runnersCopy)
        {
            runner.ResetIfNeeded();

            if (runner.IsCircuitBreakerTripped)
            {
                continue;
            }

            MainLoop.UpdateHeartbeat();
            runner.Execute(deltaTime);
        }
    }
}
