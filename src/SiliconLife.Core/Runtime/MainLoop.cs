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
    private static readonly Thread _thread;
    private static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private static readonly object _lock = new object();
    private static bool _isRunning = false;

    static MainLoop()
    {
        _thread = new Thread(Run);
        _thread.IsBackground = true;
        _thread.Name = "MainLoop";
    }

    /// <summary>
    /// Gets whether the main loop is running
    /// </summary>
    public static bool IsRunning => _isRunning;

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
            TimeSpan currentTickTime = stopwatch.Elapsed;
            TimeSpan deltaTime = currentTickTime - lastTickTime;
            lastTickTime = currentTickTime;

            UpdateTickObjects(deltaTime);
        }

        stopwatch.Stop();
    }

    /// <summary>
    /// Updates all registered tick objects
    /// </summary>
    /// <param name="deltaTime">Time elapsed since the last update</param>
    private static void UpdateTickObjects(TimeSpan deltaTime)
    {
        List<TickObject> tickObjectsCopy;

        lock (_lock)
        {
            tickObjectsCopy = new List<TickObject>(_tickObjects);
        }

        foreach (TickObject tickObject in tickObjectsCopy)
        {
            tickObject.Tick(deltaTime);
        }
    }

    /// <summary>
    /// Sorts tick objects by priority (lower value = higher priority)
    /// </summary>
    private static void SortTickObjects()
    {
        _tickObjects.Sort((a, b) => a.Priority.CompareTo(b.Priority));
    }
}
