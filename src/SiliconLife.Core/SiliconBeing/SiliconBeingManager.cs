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
using System.Reflection;

namespace SiliconLife.Collective;

/// <summary>
/// Manages the execution lifecycle of a single silicon being.
/// Executes Tick on a temporary thread per invocation with proper timeout handling.
/// </summary>
public class SiliconBeingRunner
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger(typeof(SiliconBeingRunner));
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

        _logger.Debug("Executing being tick: {0} ({1})", _being.Name, _being.Id);

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
                }
                catch (Exception ex)
                {
                    _logger.Error("Being tick failed with exception: {0}", _being.Name, ex);
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
                _logger.Debug("Being tick completed: {0} in {1}ms", _being.Name, timeout.TotalMilliseconds);
                return true;
            }

            _logger.Warn("Being tick timed out: {0} after {1}ms", _being.Name, timeout.TotalMilliseconds);
            KillThread(worker, _being.Name);
            HandleTimeout();
            return false;
        }
        catch (ThreadInterruptedException)
        {
            return false;
        }
        catch
        {
            KillThread(worker, _being.Name);
            return false;
        }
    }

    /// <summary>
    /// Kill a thread aggressively: Interrupt → Abort → verify.
    /// </summary>
    private static void KillThread(Thread? worker, string beingName)
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
            }

            if (!worker.Join(TimeSpan.FromMilliseconds(500)))
            {
                _logger.Error("Worker thread for being {0} is out of control", beingName);
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
            _logger.Warn("Circuit breaker tripped for being {0}, cooldown for 1 minute", _being.Name);
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
            _logger.Info("Circuit breaker reset for being {0}", _being.Name);
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
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<SiliconBeingManager>();
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
        _logger.Info("SiliconBeingManager starting...");
    }

    public void Stop()
    {
        _managerRunning = false;

        lock (_lock)
        {
            _logger.Info("SiliconBeingManager stopping, clearing {0} beings", _beings.Count);
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
                bool isCurator = Config.Instance?.Data?.CuratorGuid == being.Id;
                _logger.Info("Registered being: {0} ({1}), isCurator={2}", being.Name, being.Id, isCurator);
            }
        }
    }

    public void UnregisterBeing(SiliconBeingBase being)
    {
        lock (_lock)
        {
            _runners.Remove(being);
            _beings.Remove(being);
            _logger.Info("Unregistered being: {0} ({1})", being.Name, being.Id);
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
                _logger.Debug("Skipping being {0}: circuit breaker tripped", runner.BeingId);
                continue;
            }

            try
            {
                MainLoop.UpdateHeartbeat();
                var sw = System.Diagnostics.Stopwatch.StartNew();
                runner.Execute(deltaTime);
                sw.Stop();
                MainLoop.PerformanceMonitor.Record($"SiliconBeing-{runner.BeingId}", sw.Elapsed);
            }
            catch
            {
            }
        }
    }

    #region Phase 7: Dynamic Compilation — Replace & Migrate

    /// <summary>
    /// Replaces a silicon being's implementation with a dynamically compiled type.
    /// Creates a new instance from the compiled type and migrates state from the old instance.
    /// The old instance is unregistered and the new one takes its place.
    /// </summary>
    /// <param name="beingId">The GUID of the being to replace</param>
    /// <param name="newType">The compiled Type (must inherit SiliconBeingBase)</param>
    /// <returns>True if replacement succeeded</returns>
    public bool ReplaceBeing(Guid beingId, Type newType)
    {
        if (!typeof(SiliconBeingBase).IsAssignableFrom(newType))
        {
            _logger.Error("Cannot replace being {0}: type {1} does not inherit SiliconBeingBase", beingId, newType.Name);
            return false;
        }

        lock (_lock)
        {
            SiliconBeingBase? oldBeing = _beings.FirstOrDefault(b => b.Id == beingId);
            if (oldBeing == null)
            {
                return false;
            }

            _logger.Info("Replacing being {0} with compiled type {1}", beingId, newType.Name);

            SiliconBeingBase? newBeing = null;
            try
            {
                newBeing = (SiliconBeingBase?)Activator.CreateInstance(newType, oldBeing.Id, oldBeing.Name);
            }
            catch
            {
                try
                {
                    newBeing = (SiliconBeingBase?)Activator.CreateInstance(newType);
                }
                catch
                {
                    _logger.Error("Failed to create instance of {0} for being {1}", newType.Name, beingId);
                    return false;
                }
            }

            if (newBeing == null)
            {
                _logger.Error("Failed to create instance of {0} for being {1}", newType.Name, beingId);
                return false;
            }

            // Migrate state from old to new
            MigrateState(oldBeing, newBeing);

            // Swap registration
            _runners.Remove(oldBeing);
            _beings.Remove(oldBeing);

            _beings.Add(newBeing);
            _runners[newBeing] = new SiliconBeingRunner(newBeing, MaxTimeoutCount, Config.Instance?.Data!);

            // Update PermissionManager registry
            if (newBeing.PermissionManager != null)
            {
                ServiceLocator.Instance.RegisterPermissionManager(newBeing.Id, newBeing.PermissionManager);
            }

            return true;
        }
    }

    /// <summary>
    /// Replaces a silicon being with its default implementation.
    /// Used when custom code is removed or fails to compile.
    /// The factory creates a new default instance and state is migrated.
    /// </summary>
    /// <param name="beingId">The GUID of the being to revert</param>
    public void ReplaceWithDefault(Guid beingId)
    {
        ISiliconBeingFactory? factory = ServiceLocator.Instance.BeingFactory;
        if (factory == null)
        {
            return;
        }

        lock (_lock)
        {
            SiliconBeingBase? oldBeing = _beings.FirstOrDefault(b => b.Id == beingId);
            if (oldBeing == null)
            {
                return;
            }

            _logger.Info("Reverting being {0} to default implementation", beingId);

            SiliconBeingBase? newBeing = factory.CreateBeing(oldBeing.Id, oldBeing.Name);
            if (newBeing == null)
            {
                return;
            }

            MigrateState(oldBeing, newBeing);

            _runners.Remove(oldBeing);
            _beings.Remove(oldBeing);

            _beings.Add(newBeing);
            _runners[newBeing] = new SiliconBeingRunner(newBeing, MaxTimeoutCount, Config.Instance?.Data!);

            if (newBeing.PermissionManager != null)
            {
                ServiceLocator.Instance.RegisterPermissionManager(newBeing.Id, newBeing.PermissionManager);
            }
        }
    }

    /// <summary>
    /// Replaces the permission callback for a specific being.
    /// Loads the compiled IPermissionCallback and injects it into the being's PermissionManager.
    /// Stealth channel — this operation is not visible to the AI.
    /// </summary>
    /// <param name="beingId">The GUID of the being</param>
    /// <param name="callbackType">The compiled Type implementing IPermissionCallback</param>
    public void ReplacePermissionCallback(Guid beingId, Type callbackType)
    {
        if (!typeof(IPermissionCallback).IsAssignableFrom(callbackType))
        {
            _logger.Warn("ReplacePermissionCallback: type {0} does not implement IPermissionCallback", callbackType.Name);
            return;
        }

        lock (_lock)
        {
            SiliconBeingBase? being = _beings.FirstOrDefault(b => b.Id == beingId);
            if (being == null)
            {
                _logger.Warn("ReplacePermissionCallback: being {0} not found in manager", beingId);
                return;
            }
            if (being.PermissionManager == null)
            {
                _logger.Warn("ReplacePermissionCallback: being {0} has no PermissionManager", beingId);
                return;
            }

            try
            {
                // Get the app data directory from config
                string appDataDirectory = Config.Instance?.Data?.DataDirectory?.FullName ?? string.Empty;
                
                // Find the constructor that takes a single string parameter
                ConstructorInfo? ctor = callbackType.GetConstructor(new[] { typeof(string) });
                if (ctor == null)
                {
                    _logger.Warn("ReplacePermissionCallback: no constructor(string) found on type {0}", callbackType.Name);
                    return;
                }
                
                // Invoke constructor directly with appDataDirectory
                IPermissionCallback? callback = (IPermissionCallback?)ctor.Invoke(new object[] { appDataDirectory });
                    
                if (callback != null)
                {
                    being.PermissionManager.SetCustomCallback(callback);
                    _logger.Info("Custom permission callback applied for being {0}, type={1}", beingId, callbackType.Name);
                }
                else
                {
                    _logger.Warn("ReplacePermissionCallback: constructor returned null for being {0}", beingId);
                }
            }
            catch (TargetInvocationException tie) when (tie.InnerException != null)
            {
                _logger.Warn("Failed to instantiate permission callback for being {0}: constructor threw {1}", beingId, tie.InnerException.Message);
            }
            catch (Exception ex)
            {
                _logger.Warn("Failed to instantiate permission callback for being {0}: {1}", beingId, ex.Message);
            }
        }
    }

    /// <summary>
    /// Resets a being's permission callback to the system default.
    /// </summary>
    /// <param name="beingId">The GUID of the being</param>
    public void ResetPermissionCallback(Guid beingId)
    {
        lock (_lock)
        {
            SiliconBeingBase? being = _beings.FirstOrDefault(b => b.Id == beingId);
            if (being?.PermissionManager == null)
            {
                return;
            }

            being.PermissionManager.ResetCallback();
        }
    }

    /// <summary>
    /// Migrates state from an old silicon being to a new one.
    /// Copies all mutable properties: AIClient, SoulContent, ToolManager, PermissionManager.
    /// </summary>
    /// <param name="oldBeing">The old being instance</param>
    /// <param name="newBeing">The new being instance</param>
    private static void MigrateState(SiliconBeingBase oldBeing, SiliconBeingBase newBeing)
    {
        _logger.Debug("Migrating state from {0} to {1} for being {2}", oldBeing.GetType().Name, newBeing.GetType().Name, oldBeing.Id);

        newBeing.AIClient = oldBeing.AIClient;
        newBeing.SoulContent = oldBeing.SoulContent;
        newBeing.ToolManager = oldBeing.ToolManager;

        // Migrate PermissionManager — create new one with same owner (newBeing) but keep the old callback
        PermissionManager? oldPm = oldBeing.PermissionManager;
        if (oldPm != null)
        {
            PermissionManager newPm = new PermissionManager(
                newBeing,
                oldPm.GlobalAcl,
                oldPm.CustomCallback,
                oldPm.AskHandler);
            newBeing.PermissionManager = newPm;
        }
    }

    #endregion
}
