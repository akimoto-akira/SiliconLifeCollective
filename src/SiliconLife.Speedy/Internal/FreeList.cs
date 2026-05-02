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

namespace SiliconLife.Speedy.Internal;

/// <summary>
/// Free block table. Maintains a collection of free blocks as (offset, length)
/// pairs, supporting allocation and release. On release, automatically merges
/// with adjacent blocks so that SpeedyPack can reuse space in-place like a real disk.
/// </summary>
/// <remarks>
/// - Not persisted; rebuilt on startup by <see cref="PackFileWriter"/> from Directory + FileLength;
/// - Internally uses <see cref="SortedDictionary{TKey,TValue}"/> (offset -> length).
///   Release merging of adjacent blocks is O(n); Allocate is first-fit and also O(n),
///   which satisfies the project principle of "implementation before performance";
/// - Thread-safe: all public methods are executed under internal lock protection.
/// </remarks>
internal sealed class FreeList
{
    // key = free block start offset, value = free block length (bytes). Sorted by offset ascending.
    private readonly SortedDictionary<long, long> _blocks = new();
    private readonly object _lock = new();

    /// <summary>
    /// Current number of free blocks (useful for debugging and diagnostics).
    /// </summary>
    public int BlockCount
    {
        get { lock (_lock) return _blocks.Count; }
    }

    /// <summary>
    /// Total bytes of all current free blocks (useful for diagnosing "garbage ratio").
    /// </summary>
    public long TotalFreeBytes
    {
        get
        {
            lock (_lock)
            {
                long total = 0;
                foreach (var len in _blocks.Values)
                    total += len;
                return total;
            }
        }
    }

    /// <summary>
    /// Release a block of space. Automatically merges with adjacent preceding/successor
    /// blocks to avoid fragmentation buildup.
    /// </summary>
    public void Release(long offset, long length)
    {
        if (length <= 0) return;
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));

        lock (_lock)
        {
            long mergedOffset = offset;
            long mergedLength = length;

            // Look for the immediately adjacent "preceding block": key + length == offset.
            // SortedDictionary iterates in key ascending order; stop once key >= offset.
            long? prevKey = null;
            foreach (var kvp in _blocks)
            {
                if (kvp.Key >= offset) break;
                if (kvp.Key + kvp.Value == offset)
                {
                    prevKey = kvp.Key;
                    // No further rightward preceding block exists (_blocks has no overlap); safe to break.
                    break;
                }
            }
            if (prevKey.HasValue)
            {
                var prevLen = _blocks[prevKey.Value];
                _blocks.Remove(prevKey.Value);
                mergedOffset = prevKey.Value;
                mergedLength = prevLen + length;
            }

            // Look for the immediately adjacent "successor block": key == mergedOffset + mergedLength
            var nextKey = mergedOffset + mergedLength;
            if (_blocks.TryGetValue(nextKey, out var nextLen))
            {
                _blocks.Remove(nextKey);
                mergedLength += nextLen;
            }

            _blocks[mergedOffset] = mergedLength;
        }
    }

    /// <summary>
    /// Try to allocate a free block of at least <paramref name="requiredLength"/> bytes (first-fit).
    /// On success, the remaining portion (if any) is re-added to the free list.
    /// </summary>
    /// <returns>true on success with the start position returned via <paramref name="offset"/>; false on failure.</returns>
    public bool TryAllocate(long requiredLength, out long offset)
    {
        offset = -1;
        if (requiredLength <= 0) return false;

        lock (_lock)
        {
            // Take the first block with sufficient capacity (first-fit).
            long? hitKey = null;
            long hitLen = 0;
            foreach (var kvp in _blocks)
            {
                if (kvp.Value >= requiredLength)
                {
                    hitKey = kvp.Key;
                    hitLen = kvp.Value;
                    break;
                }
            }

            if (!hitKey.HasValue) return false;

            _blocks.Remove(hitKey.Value);
            offset = hitKey.Value;

            var remaining = hitLen - requiredLength;
            if (remaining > 0)
                _blocks[offset + requiredLength] = remaining;

            return true;
        }
    }

    /// <summary>
    /// Clear the free list. Only for use during Compact/rebuild.
    /// </summary>
    public void Clear()
    {
        lock (_lock) _blocks.Clear();
    }
}
