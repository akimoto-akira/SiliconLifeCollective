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
/// 空闲区间表。维护一组 (offset, length) 的空闲块集合，支持分配与归还，
/// 归还时自动与前后相邻块合并，使 SpeedyPack 能像真实硬盘那样就地复用空间。
/// </summary>
/// <remarks>
/// - 不做持久化，启动时由 <see cref="PackFileWriter"/> 从 Directory + FileLength 重建；
/// - 内部使用 <see cref="SortedDictionary{TKey,TValue}"/>（offset -> length），
///   Release 合并相邻块耗 O(n)，Allocate 为 first-fit 也耗 O(n)，
///   已满足"实现优先于性能"的项目原则；
/// - 线程安全：所有公共方法均在内部锁保护下执行。
/// </remarks>
internal sealed class FreeList
{
    // key = 空闲块起始 offset，value = 空闲块长度（字节）。按 offset 升序。
    private readonly SortedDictionary<long, long> _blocks = new();
    private readonly object _lock = new();

    /// <summary>
    /// 当前空闲块数量（便于调试与诊断）。
    /// </summary>
    public int BlockCount
    {
        get { lock (_lock) return _blocks.Count; }
    }

    /// <summary>
    /// 当前所有空闲块的总字节数（便于诊断"垃圾率"）。
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
    /// 归还一段空间。会自动与紧邻的前块/后块合并，避免碎片堆积。
    /// </summary>
    public void Release(long offset, long length)
    {
        if (length <= 0) return;
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));

        lock (_lock)
        {
            long mergedOffset = offset;
            long mergedLength = length;

            // 查找紧邻的"前块"：key + length == offset
            // SortedDictionary 按 key 升序遍历，一旦 key >= offset 即可停。
            long? prevKey = null;
            foreach (var kvp in _blocks)
            {
                if (kvp.Key >= offset) break;
                if (kvp.Key + kvp.Value == offset)
                {
                    prevKey = kvp.Key;
                    // 不会再有更靠右的前块（_blocks 无重叠），可直接 break。
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

            // 查找紧邻的"后块"：key == mergedOffset + mergedLength
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
    /// 尝试分配一块不小于 <paramref name="requiredLength"/> 的空闲区间（first-fit）。
    /// 成功时将剩余部分重新加入空闲表。
    /// </summary>
    /// <returns>分配成功返回 true 并通过 <paramref name="offset"/> 返回起始位置；失败返回 false。</returns>
    public bool TryAllocate(long requiredLength, out long offset)
    {
        offset = -1;
        if (requiredLength <= 0) return false;

        lock (_lock)
        {
            // 取第一个容量足够的块（first-fit）。
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
    /// 清空空闲表。仅供 Compact/重建使用。
    /// </summary>
    public void Clear()
    {
        lock (_lock) _blocks.Clear();
    }
}
