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

using System.Reflection;
using SiliconLife.Collective;

namespace SiliconLife.Common.IM;

/// <summary>
/// 外部 IM Provider 的抽象基类。
/// 提取所有外部 IM 的公共逻辑（流式累积、权限交互队列、工具更新格式化等），减少子类重复代码。
/// </summary>
public abstract class ExternalIMProviderBase : IIMProvider
{
    protected static readonly ILogger _logger = LogManager.Instance
        .GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

    protected readonly Dictionary<Guid, StreamingBuffer> _streamingBuffers = new();
    protected readonly object _streamingLock = new();
    protected readonly PermissionRequestQueue _permissionQueue;

    public event EventHandler<IMMessageEventArgs>? MessageReceived;
#pragma warning disable CS0067
    public event EventHandler<StreamChunkEventArgs>? StreamChunkReceived;
    public event EventHandler? ExitRequested;
#pragma warning restore CS0067

    protected ExternalIMProviderBase()
    {
        _permissionQueue = new PermissionRequestQueue(SendActivePermissionRequestAsync);
    }

    public abstract Task StartAsync();
    public abstract Task StopAsync();

    /// <summary>
    /// 发送消息的核心实现，子类必须实现。
    /// 返回消息 ID（用于流式更新）。
    /// </summary>
    protected abstract Task<string> SendMessageCoreAsync(
        Guid senderId, Guid channelId, string content,
        string? senderName = null);

    /// <summary>
    /// 更新消息（流式），子类可选实现，默认返回 false（不支持）。
    /// </summary>
    protected virtual Task<bool> UpdateMessageCoreAsync(
        string messageId, string content)
        => Task.FromResult(false);

    // ---- 默认实现：SendMessageAsync ----
    public virtual async Task SendMessageAsync(Guid senderId, Guid channelId,
        string content, string? thinking = null, string? senderName = null,
        int? promptTokens = null, int? completionTokens = null, int? totalTokens = null)
    {
        string fullContent = content;
        if (!string.IsNullOrEmpty(thinking))
            fullContent = $"【思考中】\n{thinking}\n\n{content}";
        if (totalTokens.HasValue)
            fullContent += $"\n\n_(tokens: {totalTokens.Value})_";

        await SendMessageCoreAsync(senderId, channelId, fullContent, senderName);
    }

    // ---- 默认实现：SendStreamChunkAsync ----
    public virtual async Task SendStreamChunkAsync(Guid senderId, Guid channelId, StreamChunk chunk)
    {
        StreamingBuffer buffer;
        lock (_streamingLock)
        {
            if (!_streamingBuffers.TryGetValue(channelId, out buffer!))
            {
                buffer = new StreamingBuffer
                { StreamId = chunk.StreamId, SenderId = senderId, IsActive = true };
                _streamingBuffers[channelId] = buffer;
            }
            if (!string.IsNullOrEmpty(chunk.Content))
                buffer.Content.Append(chunk.Content);
            if (!string.IsNullOrEmpty(chunk.Thinking))
                buffer.Thinking.Append(chunk.Thinking);
        }

        string accumulated = buffer.Content.ToString();

        if (string.IsNullOrEmpty(buffer.FirstMessageId))
        {
            string msgId = await SendMessageCoreAsync(senderId, channelId, accumulated);
            buffer.FirstMessageId = msgId;
        }
        else
        {
            bool updated = await UpdateMessageCoreAsync(buffer.FirstMessageId, accumulated);
            if (!updated)
            {
                // 平台不支持消息更新，降级处理由子类覆盖 SendStreamChunkAsync 实现
            }
        }

        if (chunk.IsFinal)
        {
            buffer.Clear();
        }
    }

    // ---- 默认实现：AskPermissionAsync ----
    public virtual async Task<AskPermissionResult> AskPermissionAsync(
        PermissionType permissionType, string resource,
        string allowCode, string denyCode)
    {
        Guid userId = Config.Instance.Data.CuratorGuid;

        var request = new PendingPermissionRequest
        {
            RequestId = Guid.NewGuid(),
            UserId = userId,
            PermissionType = permissionType,
            Resource = resource,
            AllowCode = allowCode,
            DenyCode = denyCode,
            Tcs = new TaskCompletionSource<AskPermissionResult>(),
            CreatedAt = DateTime.UtcNow,
            TimeoutCts = new CancellationTokenSource(TimeSpan.FromMinutes(1))
        };

        request.TimeoutCts.Token.Register(
            () => _permissionQueue.HandleTimeout(request));

        string prompt = FormatPermissionPrompt(permissionType, resource, allowCode, denyCode);
        await SendMessageCoreAsync(userId, Guid.Empty, prompt);

        return await _permissionQueue.EnqueueAsync(request);
    }

    /// <summary>
    /// 格式化权限请求提示文本。
    /// 子类可覆盖以使用卡片等更友好的方式。
    /// </summary>
    protected virtual string FormatPermissionPrompt(
        PermissionType permissionType, string resource,
        string allowCode, string denyCode)
    {
        return $"⚠ 权限请求\n\n" +
               $"类型: {permissionType}\n" +
               $"资源: {resource}\n\n" +
               $"回复「{allowCode}」允许，回复「{denyCode}」拒绝\n" +
               $"(1 分钟内有效)";
    }

    // ---- 默认实现：SendToolUpdateAsync ----
    public virtual Task SendToolUpdateAsync(Guid senderId, Guid channelId,
        string role, string content, string? toolCallsJson = null,
        string? toolCallId = null, string? thinking = null,
        string? senderName = null, int? promptTokens = null,
        int? completionTokens = null, int? totalTokens = null)
    {
        string text = role == "tool"
            ? $"🔧 工具结果: {content}"
            : $"⚙ 调用工具: {content}";
        return SendMessageCoreAsync(senderId, channelId, text, senderName);
    }

    // ---- 默认实现：SendStreamStoppedAsync ----
    public virtual Task SendStreamStoppedAsync(Guid channelId)
        => Task.CompletedTask;

    // ---- 默认实现：SendQueueStatusAsync ----
    public virtual Task SendQueueStatusAsync(Guid channelId, int position, int totalCount = 0)
        => Task.CompletedTask;

    // ---- 触发事件的保护方法 ----
    protected void OnMessageReceived(IMMessageEventArgs e)
        => MessageReceived?.Invoke(this, e);

    /// <summary>
    /// 权限请求激活时的发送回调（用于 UI 更新，外部 IM 可忽略）。
    /// </summary>
    protected virtual Task SendActivePermissionRequestAsync()
        => Task.CompletedTask;

    /// <summary>
    /// 尝试匹配用户回复的权限码。
    /// </summary>
    protected bool TryMatchPermissionResponse(string userReply, string allowCode, string denyCode, out bool allowed)
        => PermissionTextMatcher.TryMatch(userReply, allowCode, denyCode, out allowed);
}