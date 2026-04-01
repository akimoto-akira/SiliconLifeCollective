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
/// Base class for objects that can be ticked by MainLoop
/// </summary>
public abstract class TickObject
{
    private TimeSpan _elapsedTime = TimeSpan.Zero;

    /// <summary>
    /// Gets or sets the priority (lower value = higher priority)
    /// </summary>
    public int Priority { get; set; } = 100;

    /// <summary>
    /// Gets the tick interval (time between ticks)
    /// If zero, ticks on every update
    /// </summary>
    public TimeSpan Interval { get; }

    /// <summary>
    /// Initializes a new instance of the TickObject class
    /// </summary>
    /// <param name="interval">The tick interval (default: TimeSpan.Zero for every update)</param>
    /// <param name="autoRegister">Whether to automatically register to MainLoop (default: true)</param>
    protected TickObject(TimeSpan interval = default, bool autoRegister = true)
    {
        Interval = interval;

        if (autoRegister)
        {
            MainLoop.Register(this);
        }
    }

    /// <summary>
    /// Called by MainLoop to update this tick object
    /// Accumulates time and triggers OnTick when interval is reached
    /// </summary>
    /// <param name="deltaTime">Time elapsed since the last update</param>
    public void Tick(TimeSpan deltaTime)
    {
        if (Interval == TimeSpan.Zero)
        {
            OnTick(deltaTime);
            return;
        }

        _elapsedTime += deltaTime;

        if (_elapsedTime >= Interval)
        {
            OnTick(_elapsedTime);
            _elapsedTime = TimeSpan.Zero;
        }
    }

    /// <summary>
    /// Called when the tick interval has elapsed
    /// </summary>
    /// <param name="deltaTime">Time elapsed since the last tick</param>
    protected abstract void OnTick(TimeSpan deltaTime);
}
