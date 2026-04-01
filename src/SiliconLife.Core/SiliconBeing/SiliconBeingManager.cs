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
/// Manages all silicon beings and drives their execution
/// </summary>
public class SiliconBeingManager : TickObject
{
    private readonly List<SiliconBeingBase> _beings;
    private readonly object _lock;

    /// <summary>
    /// Initializes a new instance of the SiliconBeingManager class
    /// Priority is set to 0 (highest priority)
    /// </summary>
    public SiliconBeingManager()
        : base(TimeSpan.Zero, true)
    {
        Priority = 0;
        _beings = new List<SiliconBeingBase>();
        _lock = new object();
    }

    /// <summary>
    /// Gets the number of registered silicon beings
    /// </summary>
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

    /// <summary>
    /// Registers a silicon being to be managed
    /// </summary>
    /// <param name="being">The silicon being to register</param>
    public void RegisterBeing(SiliconBeingBase being)
    {
        lock (_lock)
        {
            if (!_beings.Contains(being))
            {
                _beings.Add(being);
            }
        }
    }

    /// <summary>
    /// Unregisters a silicon being from management
    /// </summary>
    /// <param name="being">The silicon being to unregister</param>
    public void UnregisterBeing(SiliconBeingBase being)
    {
        lock (_lock)
        {
            _beings.Remove(being);
        }
    }

    /// <summary>
    /// Gets all registered silicon beings
    /// </summary>
    /// <returns>A copy of the list of registered beings</returns>
    public List<SiliconBeingBase> GetAllBeings()
    {
        lock (_lock)
        {
            return new List<SiliconBeingBase>(_beings);
        }
    }

    /// <summary>
    /// Called when the tick interval has elapsed
    /// Iterates through all registered beings and calls their Tick method
    /// </summary>
    /// <param name="deltaTime">Time elapsed since the last tick</param>
    protected override void OnTick(TimeSpan deltaTime)
    {
        List<SiliconBeingBase> beingsCopy;

        lock (_lock)
        {
            beingsCopy = new List<SiliconBeingBase>(_beings);
        }

        foreach (SiliconBeingBase being in beingsCopy)
        {
            try
            {
                being.Tick(deltaTime);
            }
            catch (Exception)
            {
            }
        }
    }
}
