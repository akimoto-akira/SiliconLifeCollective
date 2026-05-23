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

using SiliconLife.Speedy;

namespace SiliconLife.Fast;

/// <summary>
/// Process-wide singleton that owns the one and only <see cref="SpeedyPack"/> used by
/// the entire Fast application.
/// <para>
/// All storage adapters (<see cref="SpeedyStorage"/>, <see cref="SpeedyTimeStorage"/>,
/// <see cref="SpeedyWorkNoteStorage"/>) read and write through this single pack instance,
/// eliminating duplicate file handles and write-queue races.
/// </para>
/// </summary>
/// <remarks>
/// Thread-safe. Call <see cref="Initialize"/> once at application startup before any
/// storage access, and <see cref="Dispose"/> once during shutdown to flush and close
/// the file handle.
/// </remarks>
public static class SpeedyPackRegistry
{
    private static SpeedyPack? _pack;
    private static SpeedyPackAutoCompactor? _autoCompactor;
    private static readonly Lock _lock = new();

    // ─── Public API ───────────────────────────────────────────────────────────

    /// <summary>
    /// The file name of the single pack file, relative to
    /// <see cref="AppDomain.CurrentDomain.BaseDirectory"/>.
    /// </summary>
    public const string PackFileName = "siliconlife_storage.spk";

    /// <summary>
    /// Full path to the single pack file.
    /// </summary>
    public static string PackFilePath =>
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PackFileName);

    /// <summary>
    /// Opens (or creates) the single <see cref="SpeedyPack"/> at
    /// <see cref="PackFilePath"/>. Must be called once before any storage access.
    /// Subsequent calls are no-ops.
    /// </summary>
    public static void Initialize(SpeedyPackOptions? options = null)
    {
        lock (_lock)
        {
            if (_pack is not null) return;
            _pack = SpeedyPack.Open(PackFilePath, options);
            _autoCompactor = new SpeedyPackAutoCompactor(_pack);
        }
    }

    /// <summary>
    /// Returns the single shared <see cref="SpeedyPack"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if <see cref="Initialize"/> has not been called yet.
    /// </exception>
    public static SpeedyPack Pack
    {
        get
        {
            lock (_lock)
            {
                if (_pack is null)
                    throw new InvalidOperationException(
                        "SpeedyPackRegistry has not been initialized. " +
                        "Call SpeedyPackRegistry.Initialize() at application startup.");
                return _pack;
            }
        }
    }

    /// <summary>
    /// Flushes all pending writes and closes the pack file handle.
    /// Call once during application shutdown.
    /// </summary>
    public static void Dispose()
    {
        lock (_lock)
        {
            _autoCompactor = null;
            _pack?.Dispose();
            _pack = null;
        }
    }
}
