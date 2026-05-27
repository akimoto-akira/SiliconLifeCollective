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

using System;
using SiliconLife.Collective;

namespace SiliconLife.Demo.TickObject;

/// <summary>
/// Demonstrates using TickObject to integrate with MainLoop for periodic/continuous logic.
///
/// TickObject is the base class for objects that can be ticked by MainLoop's main loop.
/// Plugins can inherit from TickObject to implement timer-like behavior within the
/// unified main loop, rather than using System.Threading.Timer or Task.Delay.
///
/// Key mechanisms:
///   - Inherit TickObject, implement OnTick(TimeSpan deltaTime)
///   - Constructor specifies Interval (tick interval) and autoRegister (auto-register to MainLoop)
///   - Priority property controls execution order (lower = higher priority)
///   - MainLoop iterates all TickObjects sorted by Priority, accumulates elapsedTime,
///     and triggers OnTick when Interval is reached
///   - MainLoop has built-in circuit breaker: consecutive timeouts trip 1-minute cooldown
///   - Supports PreTick/PostTick callback registration
///
/// Demonstrated scenarios:
///   1. Basic timer: autoRegister=true, prints status every 5 seconds
///   2. Manual registration: autoRegister=false, register in OnStart
///   3. Priority: two TickObjects with different Priorities, observe execution order
///   4. Cleanup: MainLoop.Unregister in OnStop
///
/// TickObject vs System.Threading.Timer:
///   - TickObject: unified main loop, single thread, deterministic execution order
///   - Timer: independent threads, non-deterministic order, harder to debug
/// </summary>
public class TickObjectPlugin : IPlugin
{
    public string Id => "com.siliconlife.demo.tickobject";
    public string Version => "1.0.0";
    public string GetName(Language language) => "TickObject Demo";
    public string GetDescription(Language language) =>
        "Demonstrates using TickObject for periodic tasks in MainLoop. " +
        "Shows auto-register, manual register, priority, and cleanup.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    // Demo 1: Basic timer with auto-registration
    private StatusTimer? _statusTimer;

    // Demo 2: Manual registration timer
    private HeartbeatTimer? _heartbeatTimer;

    // Demo 3: Priority demo timers
    private HighPriorityTimer? _highPriority;
    private LowPriorityTimer? _lowPriority;

    public void OnLoad()
    {
        Console.WriteLine("[TickObject] Plugin loaded.");

        // Demo 1: autoRegister=true — automatically registers to MainLoop in constructor
        _statusTimer = new StatusTimer();

        // Demo 2: autoRegister=false — will register manually in OnStart
        _heartbeatTimer = new HeartbeatTimer(autoRegister: false);

        // Demo 3: Two timers with different priorities
        _highPriority = new HighPriorityTimer();
        _lowPriority = new LowPriorityTimer();
    }

    public void OnStart()
    {
        Console.WriteLine("\n========== TickObject Demo ==========\n");

        // Demo 2: Manual registration in OnStart
        MainLoop.Register(_heartbeatTimer!);

        Console.WriteLine("[Demo 1] StatusTimer: autoRegister=true, Interval=5s, Priority=100 (default)");
        Console.WriteLine("[Demo 2] HeartbeatTimer: autoRegister=false, registered manually in OnStart, Interval=3s");
        Console.WriteLine("[Demo 3] HighPriorityTimer: Priority=10, LowPriorityTimer: Priority=200");
        Console.WriteLine();
        Console.WriteLine("TickObject lifecycle:");
        Console.WriteLine("  1. Constructor → specify Interval and autoRegister");
        Console.WriteLine("  2. MainLoop.Register (auto or manual)");
        Console.WriteLine("  3. MainLoop.Tick → accumulates elapsedTime → OnTick when Interval reached");
        Console.WriteLine("  4. MainLoop.Unregister (in OnStop to cleanup)");
        Console.WriteLine();
        Console.WriteLine("MainLoop circuit breaker:");
        Console.WriteLine("  - If OnTick exceeds TickTimeout (default 1s), timeout count increases");
        Console.WriteLine("  - After maxTimeoutCount (default 3) consecutive timeouts, circuit breaker trips");
        Console.WriteLine("  - Tripped TickObject is skipped for 1 minute cooldown");
        Console.WriteLine();
        Console.WriteLine("TickObject vs System.Threading.Timer:");
        Console.WriteLine("  TickObject: unified main loop, single thread, deterministic execution order");
        Console.WriteLine("  Timer: independent threads, non-deterministic order, harder to debug");
    }

    public void OnStop()
    {
        // Cleanup: unregister all TickObjects
        if (_statusTimer != null) MainLoop.Unregister(_statusTimer);
        if (_heartbeatTimer != null) MainLoop.Unregister(_heartbeatTimer);
        if (_highPriority != null) MainLoop.Unregister(_highPriority);
        if (_lowPriority != null) MainLoop.Unregister(_lowPriority);

        Console.WriteLine("[TickObject] All TickObjects unregistered. Plugin stopped.");
    }

    public void OnUnload()
    {
    }
}

/// <summary>
/// Demo 1: Basic timer with auto-registration.
/// Prints status every 5 seconds.
/// </summary>
public class StatusTimer : TickObject
{
    private int _tickCount;

    public StatusTimer() : base(interval: TimeSpan.FromSeconds(5), autoRegister: true)
    {
        // autoRegister=true → MainLoop.Register(this) called in base constructor
        Priority = 100; // default priority
    }

    protected override void OnTick(TimeSpan deltaTime)
    {
        _tickCount++;
        Console.WriteLine($"[StatusTimer] Tick #{_tickCount}, deltaTime={deltaTime.TotalMilliseconds:F0}ms");
    }
}

/// <summary>
/// Demo 2: Manual registration timer.
/// Heartbeat every 3 seconds, registered manually in OnStart.
/// </summary>
public class HeartbeatTimer : TickObject
{
    private int _beatCount;

    public HeartbeatTimer(bool autoRegister = true) : base(interval: TimeSpan.FromSeconds(3), autoRegister: autoRegister)
    {
        Priority = 50;
    }

    protected override void OnTick(TimeSpan deltaTime)
    {
        _beatCount++;
        Console.WriteLine($"[HeartbeatTimer] Beat #{_beatCount}, deltaTime={deltaTime.TotalMilliseconds:F0}ms");
    }
}

/// <summary>
/// Demo 3a: High priority timer (runs first).
/// </summary>
public class HighPriorityTimer : TickObject
{
    public HighPriorityTimer() : base(interval: TimeSpan.FromSeconds(10), autoRegister: true)
    {
        Priority = 10; // Lower value = higher priority → runs first
    }

    protected override void OnTick(TimeSpan deltaTime)
    {
        Console.WriteLine($"[HighPriorityTimer] OnTick (Priority=10, runs first)");
    }
}

/// <summary>
/// Demo 3b: Low priority timer (runs after high priority).
/// </summary>
public class LowPriorityTimer : TickObject
{
    public LowPriorityTimer() : base(interval: TimeSpan.FromSeconds(10), autoRegister: true)
    {
        Priority = 200; // Higher value = lower priority → runs after
    }

    protected override void OnTick(TimeSpan deltaTime)
    {
        Console.WriteLine($"[LowPriorityTimer] OnTick (Priority=200, runs after)");
    }
}
