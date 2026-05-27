# PluginDemo-19: TickObject — Periodic Task in MainLoop

## Overview

This plugin demonstrates how to use `TickObject` to integrate with `MainLoop` for periodic/continuous logic. TickObject is the base class for objects that can be ticked by MainLoop's main loop, providing a unified alternative to `System.Threading.Timer` or `Task.Delay`.

## TickObject Lifecycle

```
Constructor(interval, autoRegister)
    │
    ├── autoRegister=true → MainLoop.Register(this) called automatically
    │
    ├── autoRegister=false → call MainLoop.Register(this) manually later
    │
    ▼
MainLoop.Tick() loop
    │
    ├── Sort all registered TickObjects by Priority (ascending)
    ├── Accumulate elapsedTime for each TickObject
    ├── If elapsedTime >= Interval → call OnTick(deltaTime)
    │
    ├── Circuit breaker: if OnTick exceeds TickTimeout → increment timeout count
    │   └── After maxTimeoutCount consecutive timeouts → 1-minute cooldown
    │
    ▼
MainLoop.Unregister(tickObject) — cleanup in OnStop
```

## Key Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Interval` | `TimeSpan` | Required | How often OnTick is called |
| `Priority` | `int` | 100 | Execution order (lower = higher priority) |
| `autoRegister` | `bool` | `true` | Auto-register to MainLoop in constructor |

## Key Methods

| Method | Description |
|--------|-------------|
| `OnTick(TimeSpan deltaTime)` | Override to implement periodic logic |
| `MainLoop.Register(TickObject)` | Manually register to MainLoop |
| `MainLoop.Unregister(TickObject)` | Remove from MainLoop (cleanup) |

## Demo Scenarios

### 1. Basic Timer (autoRegister=true)
```csharp
public class StatusTimer : TickObject
{
    public StatusTimer() : base(interval: TimeSpan.FromSeconds(5), autoRegister: true)
    {
        Priority = 100;
    }

    protected override void OnTick(TimeSpan deltaTime)
    {
        Console.WriteLine($"Tick, deltaTime={deltaTime.TotalMilliseconds:F0}ms");
    }
}
```

### 2. Manual Registration (autoRegister=false)
```csharp
// In constructor: don't auto-register
_heartbeatTimer = new HeartbeatTimer(autoRegister: false);

// In OnStart: register manually
MainLoop.Register(_heartbeatTimer);
```

### 3. Priority Ordering
- `Priority = 10` → High priority, runs first
- `Priority = 200` → Low priority, runs after

### 4. Cleanup
```csharp
// In OnStop: always unregister to prevent leaks
MainLoop.Unregister(_statusTimer);
```

## MainLoop Circuit Breaker

MainLoop has a built-in circuit breaker to prevent slow TickObjects from blocking the entire loop:

1. If `OnTick` exceeds `TickTimeout` (default 1 second) → timeout count increases
2. After `maxTimeoutCount` (default 3) consecutive timeouts → circuit breaker trips
3. Tripped TickObject is **skipped** for 1 minute cooldown
4. After cooldown, the TickObject is given another chance

## TickObject vs System.Threading.Timer

| Aspect | TickObject + MainLoop | System.Threading.Timer |
|--------|----------------------|----------------------|
| Thread model | Single main loop thread | Thread pool threads |
| Execution order | Deterministic (by Priority) | Non-deterministic |
| Circuit breaker | Built-in | None |
| Debugging | Easy (single thread) | Hard (race conditions) |
| Resource usage | Minimal (no thread pool) | Thread pool overhead |
| Interval accuracy | Best-effort (affected by other TickObjects) | More precise |

## Security Note

TickObject itself requires **no** capability declaration. It's a safe, built-in framework mechanism.

## Related Examples

- **13-CapabilityNetwork**: Capability.Network declaration
- **20-SpeedyPack**: Data storage without Capability.FileIO
