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
/// Streaming buffer state for a single session.
/// Accumulates incremental content from AI streaming responses.
/// </summary>
public class StreamingBuffer
{
    public Guid StreamId { get; set; }
    public StringBuilder Content { get; } = new();
    public StringBuilder Thinking { get; } = new();
    public string? SenderName { get; set; }
    public Guid SenderId { get; set; }
    public bool IsActive { get; set; }

    /// <summary>
    /// 外部 IM 流式更新需要记录首片消息 ID，用于后续 PATCH 更新。
    /// </summary>
    public string? FirstMessageId { get; set; }

    public void Clear()
    {
        StreamId = Guid.Empty;
        Content.Clear();
        Thinking.Clear();
        SenderName = null;
        SenderId = Guid.Empty;
        IsActive = false;
        FirstMessageId = null;
    }
}