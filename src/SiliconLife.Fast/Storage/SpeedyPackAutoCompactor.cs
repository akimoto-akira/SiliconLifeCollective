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

using SiliconLife.Collective;
using SiliconLife.Speedy;

namespace SiliconLife.Fast;

/// <summary>
/// Auto-compaction timer that compacts SpeedyPack every 5 minutes to reclaim space
/// </summary>
internal sealed class SpeedyPackAutoCompactor : TickObject
{
    private readonly SpeedyPack _pack;

    /// <summary>
    /// Creates an auto-compaction timer
    /// </summary>
    /// <param name="pack">The SpeedyPack instance to compact</param>
    /// <param name="autoRegister">Whether to auto-register with MainLoop (default true)</param>
    public SpeedyPackAutoCompactor(SpeedyPack pack, bool autoRegister = true)
        : base(TimeSpan.FromMinutes(30), autoRegister)
    {
        _pack = pack ?? throw new ArgumentNullException(nameof(pack));
    }

    /// <summary>
    /// Executes a compaction operation every 5 minutes
    /// </summary>
    /// <param name="deltaTime">Time elapsed since the last Tick</param>
    protected override async void OnTick(TimeSpan deltaTime)
    {
        try
        {
            await _pack.CompactAsync();
        }
        catch
        {
            // Compaction failure does not affect the main loop; only log the error
            // TODO: Add log recording
        }
    }
}
