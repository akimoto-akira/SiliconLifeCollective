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

namespace SiliconLife.Common.IM;

/// <summary>
/// 聚合多个 IM Provider，支持同时启用多个平台。
/// 消息接收：任一平台的消息都会触发 MessageReceived 事件。
/// 消息发送：发送到所有启用的平台。
/// 权限交互：第一个响应用户的平台决定结果（其余忽略）。
/// </summary>
public class AggregateIMProvider : IIMProvider, IDisposable
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<AggregateIMProvider>();
    private readonly List<IIMProvider> _providers = new();
    private bool _disposed;

    public event EventHandler<IMMessageEventArgs>? MessageReceived;
    public event EventHandler<StreamChunkEventArgs>? StreamChunkReceived;
    public event EventHandler? ExitRequested;

    public IReadOnlyList<IIMProvider> Providers => _providers.AsReadOnly();

    /// <summary>
    /// 根据已创建的 Provider 列表创建聚合 Provider。
    /// Provider 的创建由调用方负责（支持不同层级和依赖）。
    /// </summary>
    public AggregateIMProvider(IEnumerable<IIMProvider> providers)
    {
        foreach (var provider in providers)
        {
            _providers.Add(provider);
            provider.MessageReceived += (s, e) => MessageReceived?.Invoke(this, e);
            provider.StreamChunkReceived += (s, e) => StreamChunkReceived?.Invoke(this, e);
            provider.ExitRequested += (s, e) => ExitRequested?.Invoke(this, e);
        }

        _logger.Info(null, "AggregateIMProvider created with {0} provider(s)", _providers.Count);
    }

    public async Task StartAsync()
    {
        var tasks = _providers.Select(p => SafeStartAsync(p));
        await Task.WhenAll(tasks);
        _logger.Info(null, "AggregateIMProvider started: {0} active", _providers.Count);
    }

    private async Task SafeStartAsync(IIMProvider provider)
    {
        try
        {
            await provider.StartAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to start {0}: {1}", provider.GetType().Name, ex.Message);
        }
    }

    public async Task StopAsync()
    {
        var tasks = _providers.Select(p => SafeStopAsync(p));
        await Task.WhenAll(tasks);
        _logger.Info(null, "AggregateIMProvider stopped");
    }

    private async Task SafeStopAsync(IIMProvider provider)
    {
        try
        {
            await provider.StopAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to stop {0}: {1}", provider.GetType().Name, ex.Message);
        }
    }

    // ---- 消息发送：广播到所有平台 ----
    public async Task SendMessageAsync(Guid senderId, Guid channelId, string content,
        string? thinking = null, string? senderName = null,
        int? promptTokens = null, int? completionTokens = null, int? totalTokens = null)
    {
        var tasks = _providers.Select(p => SafeSendMessageAsync(p, senderId, channelId, content,
            thinking, senderName, promptTokens, completionTokens, totalTokens));
        await Task.WhenAll(tasks);
    }

    private async Task SafeSendMessageAsync(IIMProvider provider, Guid senderId, Guid channelId,
        string content, string? thinking, string? senderName,
        int? promptTokens, int? completionTokens, int? totalTokens)
    {
        try
        {
            await provider.SendMessageAsync(senderId, channelId, content, thinking, senderName,
                promptTokens, completionTokens, totalTokens);
        }
        catch (Exception ex)
        {
            _logger.Error(null, "SendMessage failed on {0}: {1}", provider.GetType().Name, ex.Message);
        }
    }

    // ---- 流式发送：广播到所有平台 ----
    public async Task SendStreamChunkAsync(Guid senderId, Guid channelId, StreamChunk chunk)
    {
        var tasks = _providers.Select(p => SafeSendStreamChunkAsync(p, senderId, channelId, chunk));
        await Task.WhenAll(tasks);
    }

    private async Task SafeSendStreamChunkAsync(IIMProvider provider, Guid senderId, Guid channelId, StreamChunk chunk)
    {
        try
        {
            await provider.SendStreamChunkAsync(senderId, channelId, chunk);
        }
        catch (Exception ex)
        {
            _logger.Error(null, "SendStreamChunk failed on {0}: {1}", provider.GetType().Name, ex.Message);
        }
    }

    // ---- 权限交互：第一个响应者决定结果 ----
    public async Task<AskPermissionResult> AskPermissionAsync(
        PermissionType permissionType, string resource, string allowCode, string denyCode)
    {
        var tcs = new TaskCompletionSource<AskPermissionResult>();
        var tasks = _providers.Select(p => AskPermissionWithRaceAsync(p, permissionType, resource, allowCode, denyCode, tcs));

        // 等待第一个响应
        var completedTask = await Task.WhenAny(tasks);

        try
        {
            return await completedTask;
        }
        catch
        {
            // 所有平台都失败，返回拒绝
            return new AskPermissionResult { Allowed = false };
        }
    }

    private async Task<AskPermissionResult> AskPermissionWithRaceAsync(
        IIMProvider provider, PermissionType permissionType, string resource,
        string allowCode, string denyCode, TaskCompletionSource<AskPermissionResult> raceTcs)
    {
        try
        {
            var result = await provider.AskPermissionAsync(permissionType, resource, allowCode, denyCode);
            // 第一个完成的结果设置到 raceTcs
            raceTcs.TrySetResult(result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error(null, "AskPermission failed on {0}: {1}", provider.GetType().Name, ex.Message);
            throw;
        }
    }

    // ---- 其他方法：广播到所有平台 ----
    public async Task SendToolUpdateAsync(Guid senderId, Guid channelId, string role, string content,
        string? toolCallsJson = null, string? toolCallId = null, string? thinking = null,
        string? senderName = null, int? promptTokens = null, int? completionTokens = null, int? totalTokens = null)
    {
        var tasks = _providers.Select(p => SafeCallAsync(() =>
            p.SendToolUpdateAsync(senderId, channelId, role, content, toolCallsJson, toolCallId,
                thinking, senderName, promptTokens, completionTokens, totalTokens)));
        await Task.WhenAll(tasks);
    }

    public async Task SendStreamStoppedAsync(Guid channelId)
    {
        var tasks = _providers.Select(p => SafeCallAsync(() => p.SendStreamStoppedAsync(channelId)));
        await Task.WhenAll(tasks);
    }

    public async Task SendQueueStatusAsync(Guid channelId, int position, int totalCount = 0)
    {
        var tasks = _providers.Select(p => SafeCallAsync(() => p.SendQueueStatusAsync(channelId, position, totalCount)));
        await Task.WhenAll(tasks);
    }

    private async Task SafeCallAsync(Func<Task> action)
    {
        try { await action(); }
        catch { /* 静默处理 */ }
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        foreach (var provider in _providers)
        {
            try
            {
                if (provider is IDisposable disposable)
                    disposable.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Error(null, "Failed to dispose {0}: {1}", provider.GetType().Name, ex.Message);
            }
        }

        _providers.Clear();
        _disposed = true;
    }
}