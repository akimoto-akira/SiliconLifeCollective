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

using System.Text;

namespace SiliconLife.Common.IM;

/// <summary>
/// 流式分段发送降级工具。
/// 对于不支持消息更新的平台，按最小间隔或最小长度发送分段消息。
/// </summary>
internal class StreamingSegmentSender
{
    private readonly Func<Guid, string, Task<string>> _sendFunc;
    private readonly TimeSpan _minInterval;
    private readonly int _minLength;
    private DateTime _lastSendTime = DateTime.MinValue;
    private readonly StringBuilder _pending = new();

    public StreamingSegmentSender(
        Func<Guid, string, Task<string>> sendFunc,
        TimeSpan? minInterval = null,
        int minLength = 200)
    {
        _sendFunc = sendFunc;
        _minInterval = minInterval ?? TimeSpan.FromSeconds(3);
        _minLength = minLength;
    }

    public async Task SendChunkAsync(Guid channelId, string content, bool isFinal)
    {
        _pending.Append(content);

        bool shouldSend = isFinal
            || (DateTime.UtcNow - _lastSendTime >= _minInterval)
            || (_pending.Length >= _minLength);

        if (shouldSend)
        {
            await _sendFunc(channelId, _pending.ToString());
            _pending.Clear();
            _lastSendTime = DateTime.UtcNow;
        }
    }

    public void Reset()
    {
        _pending.Clear();
        _lastSendTime = DateTime.MinValue;
    }
}