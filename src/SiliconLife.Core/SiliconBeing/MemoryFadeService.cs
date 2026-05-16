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
/// Periodic service that applies memory fading (importance decay) to all silicon beings' memories.
/// Inherits from TickObject to run on the MainLoop with a configurable interval (default: 1 hour).
/// </summary>
public sealed class MemoryFadeService : TickObject
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<MemoryFadeService>();

    /// <summary>
    /// Gets the singleton instance of the MemoryFadeService.
    /// </summary>
    public static MemoryFadeService Instance { get; } = new();

    /// <summary>
    /// Gets the total number of decay cycles that have been executed.
    /// </summary>
    public int DecayCycleCount { get; private set; }

    /// <summary>
    /// Gets the total number of entries that have had state changes across all decay cycles.
    /// </summary>
    public int TotalStateChangedEntries { get; private set; }

    private MemoryFadeService()
        : base(TimeSpan.FromHours(1), autoRegister: false)
    {
        _logger.Info(null, "MemoryFadeService created (not yet registered to MainLoop)");
    }

    /// <summary>
    /// Called periodically by MainLoop to apply memory decay to all silicon beings.
    /// </summary>
    /// <param name="deltaTime">Time elapsed since the last tick.</param>
    protected override void OnTick(TimeSpan deltaTime)
    {
        _logger.Info(null, "MemoryFadeService: Starting decay cycle #{0}", DecayCycleCount + 1);

        var beings = MainLoop.BeingManager.GetAllBeings();
        int totalStateChanged = 0;
        int totalArchived = 0;
        int beingsProcessed = 0;

        foreach (var being in beings)
        {
            if (being.Memory == null) continue;

            try
            {
                int stateChanged = being.Memory.ApplyDecay();
                int archived = being.Memory.ArchiveFadingMemories();
                totalStateChanged += stateChanged;
                totalArchived += archived;
                beingsProcessed++;
            }
            catch (Exception ex)
            {
                _logger.Error(null, "MemoryFadeService: Error applying decay for being {0}: {1}", being.Id, ex.Message);
            }
        }

        DecayCycleCount++;
        TotalStateChangedEntries += totalStateChanged;

        _logger.Info(null,
            "MemoryFadeService: Decay cycle #{0} completed. Beings processed: {1}, State changes: {2}, Auto-archived: {3}",
            DecayCycleCount, beingsProcessed, totalStateChanged, totalArchived);
    }

    /// <summary>
    /// Manually triggers a decay cycle, regardless of the tick interval.
    /// Useful for testing or administrative purposes.
    /// </summary>
    public void TriggerDecayNow()
    {
        OnTick(TimeSpan.Zero);
    }

    /// <summary>
    /// Gets a summary of the current memory state across all beings.
    /// </summary>
    /// <returns>A dictionary mapping each being's ID to its memory state statistics.</returns>
    public Dictionary<Guid, Dictionary<MemoryState, int>> GetAllBeingsStateStatistics()
    {
        var result = new Dictionary<Guid, Dictionary<MemoryState, int>>();
        var beings = MainLoop.BeingManager.GetAllBeings();

        foreach (var being in beings)
        {
            if (being.Memory == null) continue;

            try
            {
                result[being.Id] = being.Memory.GetStateStatistics();
            }
            catch (Exception ex)
            {
                _logger.Error(null, "MemoryFadeService: Error getting statistics for being {0}: {1}", being.Id, ex.Message);
            }
        }

        return result;
    }
}
