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
/// 自动压缩定时器，每5分钟压缩一次 SpeedyPack 以释放空间
/// </summary>
internal sealed class SpeedyPackAutoCompactor : TickObject
{
    private readonly SpeedyPack _pack;

    /// <summary>
    /// 创建自动压缩定时器
    /// </summary>
    /// <param name="pack">要压缩的 SpeedyPack 实例</param>
    /// <param name="autoRegister">是否自动注册到 MainLoop（默认 true）</param>
    public SpeedyPackAutoCompactor(SpeedyPack pack, bool autoRegister = true)
        : base(TimeSpan.FromMinutes(5), autoRegister)
    {
        _pack = pack ?? throw new ArgumentNullException(nameof(pack));
    }

    /// <summary>
    /// 每5分钟执行一次压缩操作
    /// </summary>
    /// <param name="deltaTime">自上次 Tick 以来经过的时间</param>
    protected override async void OnTick(TimeSpan deltaTime)
    {
        try
        {
            await _pack.CompactAsync();
        }
        catch
        {
            // 压缩失败不影响主循环，仅记录日志
            // TODO: 添加日志记录
        }
    }
}
