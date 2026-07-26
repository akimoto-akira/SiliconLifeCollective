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

using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SiliconLife.Collective;

namespace SiliconLife.Common.IM;

/// <summary>
/// 钉钉（DingTalk）IM Provider 实现。
/// 支持 Stream 模式（WebSocket）和 HTTP 回调模式两种事件接收方式，
/// 支持文本消息收发、互动卡片流式更新、互动卡片权限请求。
/// </summary>
public class DingTalkIMProvider : ExternalIMProviderBase
{
    // ---- 配置字段 ----
    private readonly string _appKey;
    private readonly string _appSecret;
    private readonly string _robotCode;
    private readonly string _eventMode;
    private readonly string _callbackPath;
    private readonly int _listenPort;

    // ---- API 客户端 ----
    private readonly DingTalkApiClient _apiClient;

    // ---- 渠道映射：钉钉 userId <-> 内部 channelId ----
    private readonly ConcurrentDictionary<string, Guid> _dingTalkUserIdToChannel = new();
    private readonly ConcurrentDictionary<Guid, string> _channelToDingTalkUserId = new();

    // ---- 卡片实例映射：messageId <-> cardInstanceId（用于 UpdateMessageCoreAsync）----
    private readonly ConcurrentDictionary<string, string> _messageIdToCardInstanceId = new();
    private readonly ConcurrentDictionary<string, string> _cardInstanceIdToMessageId = new();

    // ---- 最近活跃用户（用于发送权限请求卡片）----
    private string? _lastActiveUserId;
    private readonly object _lastActiveLock = new();

    // ---- 权限卡片实例 ID（用于超时后更新卡片）----
    private string? _permissionCardInstanceId;
    private readonly object _permissionCardLock = new();

    // ---- 事件去重 ----
    private readonly ConcurrentDictionary<string, DateTime> _processedEventIds = new();

    // ---- Stream 模式 ----
    private ClientWebSocket? _streamWebSocket;
    private CancellationTokenSource? _streamCts;
    private Task? _streamReceiveTask;
    private Task? _streamHeartbeatTask;

    // ---- HTTP 回调模式 ----
    private HttpListener? _httpListener;
    private CancellationTokenSource? _httpListenerCts;

    /// <summary>
    /// 创建钉钉 IM Provider 实例。
    /// </summary>
    /// <param name="config">配置字典，包含 appKey、appSecret、robotCode、eventMode、callbackPath、listenPort 等项</param>
    public DingTalkIMProvider(Dictionary<string, object> config)
    {
        _appKey = GetConfigString(config, "appKey");
        _appSecret = GetConfigString(config, "appSecret");
        _robotCode = GetConfigString(config, "robotCode");
        _eventMode = GetConfigString(config, "eventMode", "stream");
        _callbackPath = GetConfigString(config, "callbackPath", "/dingtalk/callback");
        _listenPort = GetConfigInt(config, "listenPort", 8080);

        if (string.IsNullOrEmpty(_appKey) || string.IsNullOrEmpty(_appSecret))
            throw new ArgumentException("钉钉配置缺少 appKey 或 appSecret");
        if (string.IsNullOrEmpty(_robotCode))
            throw new ArgumentException("钉钉配置缺少 robotCode");

        _apiClient = new DingTalkApiClient(_appKey, _appSecret, _robotCode);

        _logger.Info(null, "DingTalkIMProvider 初始化完成: eventMode={0}, robotCode={1}, callbackPath={2}, listenPort={3}",
            _eventMode, _robotCode, _callbackPath, _listenPort);
    }

    // ================================================================
    // 生命周期管理
    // ================================================================

    /// <inheritdoc/>
    public override async Task StartAsync()
    {
        if (string.Equals(_eventMode, "stream", StringComparison.OrdinalIgnoreCase))
        {
            await StartStreamModeAsync();
        }
        else
        {
            await StartHttpModeAsync();
        }

        _logger.Info(null, "DingTalkIMProvider 已启动，模式: {0}", _eventMode);
    }

    /// <inheritdoc/>
    public override async Task StopAsync()
    {
        if (string.Equals(_eventMode, "stream", StringComparison.OrdinalIgnoreCase))
        {
            await StopStreamModeAsync();
        }
        else
        {
            await StopHttpModeAsync();
        }

        _apiClient.Dispose();
        _logger.Info(null, "DingTalkIMProvider 已停止");
    }

    // ---- Stream 模式 ----

    private async Task StartStreamModeAsync()
    {
        _streamCts = new CancellationTokenSource();
        var token = _streamCts.Token;

        string endpoint = await _apiClient.OpenStreamConnectionAsync();
        _logger.Info(null, "钉钉 Stream 模式连接端点已获取: {0}", endpoint.Length > 50 ? endpoint[..50] + "..." : endpoint);

        _streamWebSocket = new ClientWebSocket();
        await _streamWebSocket.ConnectAsync(new Uri(endpoint), token);

        _streamReceiveTask = Task.Run(() => StreamReceiveLoopAsync(token), token);
        _streamHeartbeatTask = Task.Run(() => StreamHeartbeatLoopAsync(token), token);

        _logger.Info(null, "钉钉 Stream 模式 WebSocket 连接已建立");
    }

    private async Task StopStreamModeAsync()
    {
        _streamCts?.Cancel();

        try
        {
            if (_streamWebSocket?.State == WebSocketState.Open)
            {
                await _streamWebSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "shutdown",
                    CancellationToken.None);
            }
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "关闭钉钉 Stream WebSocket 时出错: {0}", ex.Message);
        }

        try
        {
            if (_streamReceiveTask != null)
                await _streamReceiveTask.WaitAsync(TimeSpan.FromSeconds(5));
        }
        catch { /* 忽略任务等待超时 */ }

        try
        {
            if (_streamHeartbeatTask != null)
                await _streamHeartbeatTask.WaitAsync(TimeSpan.FromSeconds(5));
        }
        catch { /* 忽略任务等待超时 */ }

        _streamWebSocket?.Dispose();
        _streamCts?.Dispose();
        _streamWebSocket = null;
        _streamCts = null;
        _streamReceiveTask = null;
        _streamHeartbeatTask = null;
    }

    private async Task StreamReceiveLoopAsync(CancellationToken ct)
    {
        var buffer = new byte[8192];
        var ms = new MemoryStream();

        try
        {
            while (!ct.IsCancellationRequested && _streamWebSocket?.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result;
                ms.SetLength(0);

                do
                {
                    result = await _streamWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), ct);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        _logger.Warn(null, "钉钉 Stream WebSocket 收到关闭帧");
                        return;
                    }
                    ms.Write(buffer, 0, result.Count);
                }
                while (!result.EndOfMessage);

                string message = Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
                _ = Task.Run(() => HandleStreamMessageAsync(message), ct);
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            // 正常取消
        }
        catch (Exception ex)
        {
            if (!ct.IsCancellationRequested)
                _logger.Error(null, "钉钉 Stream 接收循环异常: {0}", ex.Message);
        }
    }

    private async Task StreamHeartbeatLoopAsync(CancellationToken ct)
    {
        try
        {
            while (!ct.IsCancellationRequested && _streamWebSocket?.State == WebSocketState.Open)
            {
                await Task.Delay(TimeSpan.FromSeconds(25), ct);

                if (_streamWebSocket?.State == WebSocketState.Open)
                {
                    var pingMsg = new Dictionary<string, object>
                    {
                        ["type"] = "ping"
                    };
                    string pingJson = JsonSerializer.Serialize(pingMsg);
                    byte[] buffer = Encoding.UTF8.GetBytes(pingJson);
                    await _streamWebSocket.SendAsync(
                        new ArraySegment<byte>(buffer),
                        WebSocketMessageType.Text,
                        true,
                        ct);
                    _logger.Debug(null, "钉钉 Stream 心跳已发送");
                }
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            // 正常取消
        }
        catch (Exception ex)
        {
            if (!ct.IsCancellationRequested)
                _logger.Error(null, "钉钉 Stream 心跳循环异常: {0}", ex.Message);
        }
    }

    private void HandleStreamMessageAsync(string message)
    {
        try
        {
            using var doc = JsonDocument.Parse(message);
            var root = doc.RootElement;

            string? msgType = root.TryGetProperty("type", out var typeEl) ? typeEl.GetString() : null;

            if (msgType == "pong")
            {
                _logger.Debug(null, "钉钉 Stream 收到 pong 响应");
                return;
            }

            if (msgType == "event")
            {
                if (root.TryGetProperty("header", out var headerEl) &&
                    root.TryGetProperty("event", out var eventEl))
                {
                    ProcessStreamEvent(headerEl, eventEl);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error(null, "处理钉钉 Stream 消息异常: {0}", ex.Message);
        }
    }

    private void ProcessStreamEvent(JsonElement headerEl, JsonElement eventEl)
    {
        try
        {
            string? eventId = headerEl.TryGetProperty("eventId", out var idEl) ? idEl.GetString() : null;
            string? eventType = headerEl.TryGetProperty("eventType", out var etEl) ? etEl.GetString() : null;

            if (!string.IsNullOrEmpty(eventId) && !TryMarkEventProcessed(eventId))
            {
                _logger.Debug(null, "跳过重复的钉钉事件: {0}", eventId);
                return;
            }

            if (eventType == "im.message.receive_v1")
            {
                ProcessMessageReceiveEvent(eventEl);
            }
            else if (eventType == "card.action.trigger")
            {
                ProcessCardActionCallback(eventEl);
            }
            else
            {
                _logger.Debug(null, "未处理的钉钉事件类型: {0}", eventType);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(null, "处理钉钉 Stream 事件异常: {0}", ex.Message);
        }
    }

    // ---- HTTP 回调模式 ----

    private async Task StartHttpModeAsync()
    {
        _httpListenerCts = new CancellationTokenSource();
        var token = _httpListenerCts.Token;

        _ = Task.Run(() => ListenForCallbacksAsync(token), token);

        _logger.Info(null, "钉钉 HTTP 回调模式已启动，监听端口 {0}", _listenPort);
        await Task.CompletedTask;
    }

    private async Task StopHttpModeAsync()
    {
        _httpListenerCts?.Cancel();

        try
        {
            if (_httpListener?.IsListening == true)
            {
                _httpListener.Stop();
            }
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "停止钉钉 HttpListener 时出错: {0}", ex.Message);
        }

        _httpListenerCts?.Dispose();
        _httpListenerCts = null;
        _httpListener = null;
    }

    private async Task ListenForCallbacksAsync(CancellationToken ct)
    {
        _httpListener = new HttpListener();

        string path = _callbackPath.TrimEnd('/');
        if (!path.StartsWith('/'))
            path = '/' + path;
        string prefix = $"http://+:{_listenPort}{path}/";

        try
        {
            _httpListener.Prefixes.Add(prefix);
            _httpListener.Start();
            _logger.Info(null, "钉钉 HTTP 回调监听已启动: {0}", prefix);
        }
        catch (HttpListenerException ex)
        {
            _logger.Error(null, "无法启动钉钉 HttpListener 于 {0}: {1}", prefix, ex.Message);
            _logger.Info(null, "如遇权限问题，请执行: netsh http add urlacl url={0} user=Everyone", prefix);
            return;
        }

        try
        {
            while (!ct.IsCancellationRequested)
            {
                HttpListenerContext context;
                try
                {
                    context = await _httpListener.GetContextAsync();
                }
                catch (HttpListenerException) when (ct.IsCancellationRequested)
                {
                    break;
                }
                catch (ObjectDisposedException) when (ct.IsCancellationRequested)
                {
                    break;
                }

                _ = HandleCallbackAsync(context);
            }
        }
        catch (Exception ex)
        {
            if (!ct.IsCancellationRequested)
                _logger.Error(null, "钉钉 HTTP 回调监听异常: {0}", ex.Message);
        }
        finally
        {
            try
            {
                if (_httpListener?.IsListening == true)
                    _httpListener.Stop();
            }
            catch { /* 忽略关闭错误 */ }
        }
    }

    private async Task HandleCallbackAsync(HttpListenerContext context)
    {
        try
        {
            string body;
            using (var reader = new StreamReader(context.Request.InputStream, Encoding.UTF8))
            {
                body = await reader.ReadToEndAsync();
            }

            await WriteResponseAsync(context.Response, JsonSerializer.Serialize(new { code = 0, message = "success" }));

            string eventJsonCopy = body;
            _ = Task.Run(() => ProcessHttpEventAsync(eventJsonCopy));
        }
        catch (Exception ex)
        {
            _logger.Error(null, "处理钉钉 HTTP 回调异常: {0}", ex.Message);
            try
            {
                context.Response.StatusCode = 500;
                await WriteResponseAsync(context.Response, JsonSerializer.Serialize(new { code = 500, message = "error" }));
            }
            catch { /* 忽略响应写入错误 */ }
        }
    }

    private void ProcessHttpEventAsync(string eventJson)
    {
        try
        {
            using var doc = JsonDocument.Parse(eventJson);
            var root = doc.RootElement;

            if (root.TryGetProperty("header", out var headerEl))
            {
                string? eventId = headerEl.TryGetProperty("eventId", out var idEl) ? idEl.GetString() : null;
                string? eventType = headerEl.TryGetProperty("eventType", out var etEl) ? etEl.GetString() : null;

                if (!string.IsNullOrEmpty(eventId) && !TryMarkEventProcessed(eventId))
                {
                    _logger.Debug(null, "跳过重复的钉钉事件: {0}", eventId);
                    return;
                }

                if (eventType == "im.message.receive_v1")
                {
                    if (root.TryGetProperty("event", out var eventEl))
                        ProcessMessageReceiveEvent(eventEl);
                }
                else if (eventType == "card.action.trigger")
                {
                    if (root.TryGetProperty("event", out var eventEl))
                        ProcessCardActionCallback(eventEl);
                }
                else
                {
                    _logger.Debug(null, "未处理的钉钉事件类型: {0}", eventType);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error(null, "处理钉钉 HTTP 事件异常: {0}", ex.Message);
        }
    }

    // ================================================================
    // 消息发送核心实现
    // ================================================================

    /// <inheritdoc/>
    protected override async Task<string> SendMessageCoreAsync(
        Guid senderId, Guid channelId, string content, string? senderName = null)
    {
        string? userId = ResolveTargetUserId(channelId);
        if (string.IsNullOrEmpty(userId))
        {
            _logger.Warn(null, "无法发送钉钉消息：未找到目标 userId（channelId={0}）", channelId);
            return string.Empty;
        }

        return await _apiClient.SendTextMessageAsync(userId, content);
    }

    /// <inheritdoc/>
    protected override async Task<bool> UpdateMessageCoreAsync(string messageId, string content)
    {
        if (string.IsNullOrEmpty(messageId))
            return false;

        if (_messageIdToCardInstanceId.TryGetValue(messageId, out string? cardInstanceId) &&
            !string.IsNullOrEmpty(cardInstanceId))
        {
            string cardJson = DingTalkCardBuilder.BuildTextCard(content);
            return await _apiClient.UpdateCardInstanceAsync(cardInstanceId, cardJson);
        }

        return false;
    }

    private string? ResolveTargetUserId(Guid channelId)
    {
        if (channelId == Guid.Empty)
        {
            lock (_lastActiveLock)
            {
                return _lastActiveUserId;
            }
        }

        if (_channelToDingTalkUserId.TryGetValue(channelId, out string? uid) && !string.IsNullOrEmpty(uid))
            return uid;

        return null;
    }

    // ================================================================
    // 权限请求（使用互动卡片）
    // ================================================================

    /// <inheritdoc/>
    public override async Task<AskPermissionResult> AskPermissionAsync(
        PermissionType permissionType, string resource,
        string allowCode, string denyCode)
    {
        Guid userId = Config.Instance.Data.CuratorGuid;

        string? targetUserId;
        lock (_lastActiveLock)
        {
            targetUserId = _lastActiveUserId;
        }

        if (string.IsNullOrEmpty(targetUserId))
        {
            _logger.Warn(null, "无活跃钉钉用户，无法发送权限请求卡片");
            return new AskPermissionResult { Allowed = false };
        }

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

        string cardInstanceId = string.Empty;

        request.TimeoutCts.Token.Register(() =>
        {
            _permissionQueue.HandleTimeout(request);
            string capturedCardId;
            lock (_permissionCardLock)
            {
                capturedCardId = _permissionCardInstanceId ?? string.Empty;
            }
            if (!string.IsNullOrEmpty(capturedCardId))
            {
                string timeoutCard = DingTalkCardBuilder.BuildTimeoutCard(
                    permissionType.ToString(), resource);
                _ = _apiClient.UpdateCardInstanceAsync(capturedCardId, timeoutCard)
                    .ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                            _logger.Error(null, "更新钉钉权限卡片(超时)失败: {0}",
                                t.Exception?.GetBaseException().Message);
                    });
            }
        });

        string cardJson = DingTalkCardBuilder.BuildPermissionCard(
            permissionType.ToString(), resource, allowCode, denyCode);

        try
        {
            string outTrackId = Guid.NewGuid().ToString();
            cardInstanceId = await _apiClient.CreateCardInstanceAsync(
                targetUserId, cardJson, outTrackId);

            lock (_permissionCardLock)
            {
                _permissionCardInstanceId = cardInstanceId;
            }

            _logger.Info(userId, "钉钉权限请求卡片已发送: {0} -> cardInstanceId={1}",
                request.RequestId, cardInstanceId);
        }
        catch (Exception ex)
        {
            _logger.Error(userId, "发送钉钉权限请求卡片失败: {0}", ex.Message);
            return new AskPermissionResult { Allowed = false };
        }

        return await _permissionQueue.EnqueueAsync(request);
    }

    // ================================================================
    // 事件处理
    // ================================================================

    /// <summary>
    /// 处理消息接收事件 (im.message.receive_v1)。
    /// </summary>
    private void ProcessMessageReceiveEvent(JsonElement eventEl)
    {
        try
        {
            string? userId = null;
            if (eventEl.TryGetProperty("sender", out var senderEl) &&
                senderEl.TryGetProperty("senderId", out var senderIdEl) &&
                senderIdEl.TryGetProperty("userId", out var userIdEl))
            {
                userId = userIdEl.GetString();
            }

            if (string.IsNullOrEmpty(userId))
            {
                _logger.Warn(null, "钉钉消息事件缺少发送者 userId");
                return;
            }

            if (!eventEl.TryGetProperty("message", out var messageEl))
            {
                _logger.Warn(null, "钉钉消息事件缺少 message 字段");
                return;
            }

            string? messageType = messageEl.TryGetProperty("messageType", out var msgTypeEl)
                ? msgTypeEl.GetString()
                : null;

            if (messageType != "text")
            {
                _logger.Debug(null, "跳过非文本钉钉消息: type={0}", messageType);
                return;
            }

            string? contentJson = messageEl.TryGetProperty("text", out var textEl)
                ? textEl.GetString()
                : null;

            if (string.IsNullOrEmpty(contentJson))
            {
                _logger.Warn(null, "钉钉消息内容为空");
                return;
            }

            string text = contentJson.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                _logger.Debug(null, "钉钉消息文本为空，已跳过");
                return;
            }

            var activeRequest = _permissionQueue.GetActiveRequest();
            if (activeRequest != null &&
                TryMatchPermissionResponse(text, activeRequest.AllowCode, activeRequest.DenyCode, out bool allowed))
            {
                _logger.Info(activeRequest.UserId, "钉钉权限请求通过文本响应: {0} - {1}",
                    activeRequest.PermissionType, allowed ? "允许" : "拒绝");
                _permissionQueue.HandleResponse(activeRequest.UserId, allowed, addToCache: false, cacheDuration: null);
                UpdatePermissionCardAfterResponse(allowed, activeRequest);
                return;
            }

            Guid channelId = GetOrCreateChannelId(userId);
            Guid senderId = channelId;

            lock (_lastActiveLock)
            {
                _lastActiveUserId = userId;
            }

            var chatMessage = new ChatMessage
            {
                Id = Guid.NewGuid(),
                SenderId = senderId,
                ChannelId = channelId,
                Content = text,
                Timestamp = DateTime.Now,
                Type = MessageType.Text
            };

            _logger.Info(senderId, "收到钉钉消息: channelId={0}, text={1}",
                channelId, text.Length > 50 ? text[..50] + "..." : text);

            OnMessageReceived(new IMMessageEventArgs(chatMessage));
        }
        catch (Exception ex)
        {
            _logger.Error(null, "处理钉钉消息接收事件异常: {0}", ex.Message);
        }
    }

    /// <summary>
    /// 处理卡片按钮回调 (card.action.trigger)。
    /// </summary>
    private void ProcessCardActionCallback(JsonElement element)
    {
        try
        {
            string? actionType = element.TryGetProperty("actionType", out var atEl) ? atEl.GetString() : null;

            if (actionType != "url" && element.TryGetProperty("outTrackId", out _))
            {
            }

            string? cardInstanceId = element.TryGetProperty("cardInstanceId", out var cidEl) ? cidEl.GetString() : null;

            string? code = null;
            string? action = null;

            if (element.TryGetProperty("cardPrivateData", out var privateDataEl) &&
                privateDataEl.TryGetProperty("params", out var paramsEl))
            {
                code = paramsEl.TryGetProperty("code", out var codeEl) ? codeEl.GetString() : null;
                action = paramsEl.TryGetProperty("action", out var actionEl) ? actionEl.GetString() : null;
            }

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(action))
            {
                _logger.Debug(null, "钉钉卡片回调缺少 code 或 action");
                return;
            }

            var activeRequest = _permissionQueue.GetActiveRequest();
            if (activeRequest == null)
            {
                _logger.Warn(null, "收到钉钉卡片回调但无活跃权限请求");
                return;
            }

            bool allowed = action == "allow";
            string expectedCode = allowed ? activeRequest.AllowCode : activeRequest.DenyCode;

            if (code != expectedCode)
            {
                _logger.Warn(null, "钉钉卡片回调 code 不匹配: 期望={0}, 实际={1}", expectedCode, code);
                return;
            }

            _logger.Info(activeRequest.UserId, "钉钉权限请求通过卡片响应: {0} - {1}",
                activeRequest.PermissionType, allowed ? "允许" : "拒绝");

            _permissionQueue.HandleResponse(activeRequest.UserId, allowed, addToCache: false, cacheDuration: null);
            UpdatePermissionCardAfterResponse(allowed, activeRequest);
        }
        catch (Exception ex)
        {
            _logger.Error(null, "处理钉钉卡片回调异常: {0}", ex.Message);
        }
    }

    private void UpdatePermissionCardAfterResponse(bool allowed, PendingPermissionRequest request)
    {
        string? cardInstanceId;
        lock (_permissionCardLock)
        {
            cardInstanceId = _permissionCardInstanceId;
        }

        if (string.IsNullOrEmpty(cardInstanceId))
            return;

        string resultCard = DingTalkCardBuilder.BuildResultCard(
            allowed, request.PermissionType.ToString(), request.Resource);
        _ = _apiClient.UpdateCardInstanceAsync(cardInstanceId, resultCard).ContinueWith(t =>
        {
            if (t.IsFaulted)
                _logger.Error(null, "更新钉钉权限卡片(结果)失败: {0}", t.Exception?.GetBaseException().Message);
        });
    }

    // ================================================================
    // 渠道映射
    // ================================================================

    /// <summary>
    /// 获取或创建钉钉 userId 到内部 channelId 的映射。
    /// 使用基于 userId 哈希的确定性 Guid，保证同一用户始终映射到同一 channelId。
    /// </summary>
    private Guid GetOrCreateChannelId(string userId)
    {
        if (_dingTalkUserIdToChannel.TryGetValue(userId, out Guid existingId))
            return existingId;

        Guid newId = DeterministicGuid("dingtalk:" + userId);
        if (_dingTalkUserIdToChannel.TryAdd(userId, newId))
        {
            _channelToDingTalkUserId[newId] = userId;
            _logger.Info(null, "创建钉钉渠道映射: userId={0} -> channelId={1}", userId, newId);
        }
        else
        {
            newId = _dingTalkUserIdToChannel[userId];
        }
        return newId;
    }

    /// <summary>
    /// 基于字符串生成确定性 Guid（使用 SHA256 前 16 字节）。
    /// </summary>
    private static Guid DeterministicGuid(string input)
    {
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        byte[] bytes = new byte[16];
        Array.Copy(hash, bytes, 16);
        return new Guid(bytes);
    }

    // ================================================================
    // 辅助方法
    // ================================================================

    private static async Task WriteResponseAsync(HttpListenerResponse response, string content)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(content);
        response.ContentType = "application/json; charset=utf-8";
        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer);
        response.OutputStream.Close();
    }

    /// <summary>
    /// 标记事件已处理，用于去重。返回 true 表示首次处理，false 表示重复事件。
    /// </summary>
    private bool TryMarkEventProcessed(string eventId)
    {
        if (!_processedEventIds.TryAdd(eventId, DateTime.UtcNow))
            return false;

        if (_processedEventIds.Count > 500)
        {
            var cutoff = DateTime.UtcNow.AddMinutes(-10);
            foreach (var kvp in _processedEventIds)
            {
                if (kvp.Value < cutoff)
                {
                    ((ICollection<KeyValuePair<string, DateTime>>)_processedEventIds).Remove(kvp);
                }
            }
        }
        return true;
    }

    private static string GetConfigString(Dictionary<string, object> config, string key, string defaultValue = "")
    {
        if (config.TryGetValue(key, out object? value) && value != null)
            return value.ToString() ?? defaultValue;
        return defaultValue;
    }

    private static int GetConfigInt(Dictionary<string, object> config, string key, int defaultValue)
    {
        if (config.TryGetValue(key, out object? value) && value != null)
        {
            if (value is int i) return i;
            if (value is long l) return (int)l;
            if (int.TryParse(value.ToString(), out int parsed)) return parsed;
        }
        return defaultValue;
    }

    // ================================================================
    // 内嵌类：钉钉 API 客户端
    // ================================================================

    /// <summary>
    /// 封装钉钉 Open API 调用，包含 access_token 管理、消息发送、互动卡片操作。
    /// </summary>
    internal class DingTalkApiClient : IDisposable
    {
        private static readonly ILogger _logger = LogManager.Instance.GetLogger<DingTalkApiClient>();
        private readonly HttpClient _httpClient;
        private readonly DingTalkAccessToken _tokenManager;
        private readonly string _robotCode;

        private const string BaseUrl = "https://api.dingtalk.com/v1.0";

        public DingTalkApiClient(string appKey, string appSecret, string robotCode)
        {
            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
            _tokenManager = new DingTalkAccessToken(appKey, appSecret, _httpClient);
            _robotCode = robotCode;
        }

        /// <summary>
        /// 打开 Stream 模式连接，返回 WebSocket 端点 URL。
        /// </summary>
        public async Task<string> OpenStreamConnectionAsync()
        {
            string token = await _tokenManager.GetTokenAsync();

            var body = new Dictionary<string, string>
            {
                ["clientId"] = _robotCode,
                ["clientSecret"] = string.Empty
            };
            string bodyJson = JsonSerializer.Serialize(body);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/gateway/connections/open")
            {
                Content = new StringContent(bodyJson, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("x-acs-dingtalk-access-token", token);

            using var response = await _httpClient.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                if (doc.RootElement.TryGetProperty("endpoint", out var endpointEl))
                {
                    return endpointEl.GetString() ?? string.Empty;
                }
                _logger.Error(null, "打开钉钉 Stream 连接失败: {0}", responseBody);
                throw new InvalidOperationException($"打开钉钉 Stream 连接失败: {responseBody}");
            }
            catch (JsonException)
            {
                throw new InvalidOperationException($"钉钉 Stream 连接响应解析失败: {responseBody}");
            }
        }

        /// <summary>
        /// 发送单聊文本消息，返回 processQueryKey 作为消息 ID。
        /// </summary>
        public async Task<string> SendTextMessageAsync(string userId, string content)
        {
            string token = await _tokenManager.GetTokenAsync();

            var msgParam = new Dictionary<string, string> { ["content"] = content };
            string msgParamJson = JsonSerializer.Serialize(msgParam);

            var body = new Dictionary<string, object>
            {
                ["robotCode"] = _robotCode,
                ["userIds"] = new[] { userId },
                ["msgKey"] = "sampleText",
                ["msgParam"] = msgParamJson
            };
            string bodyJson = JsonSerializer.Serialize(body);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/robot/oToMessages/batchSend")
            {
                Content = new StringContent(bodyJson, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("x-acs-dingtalk-access-token", token);

            using var response = await _httpClient.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                if (doc.RootElement.TryGetProperty("processQueryKey", out var keyEl))
                {
                    return keyEl.GetString() ?? string.Empty;
                }
                _logger.Error(null, "钉钉发送消息失败: {0}", responseBody);
            }
            catch (JsonException)
            {
                _logger.Error(null, "钉钉 API 返回非 JSON 响应: {0}", responseBody);
            }

            return string.Empty;
        }

        /// <summary>
        /// 创建互动卡片实例，返回 cardInstanceId。
        /// </summary>
        public async Task<string> CreateCardInstanceAsync(string userId, string cardDataJson, string outTrackId)
        {
            string token = await _tokenManager.GetTokenAsync();

            JsonElement cardData;
            using (var doc = JsonDocument.Parse(cardDataJson))
            {
                cardData = doc.RootElement.Clone();
            }

            var body = new Dictionary<string, object>
            {
                ["cardTemplateId"] = _robotCode,
                ["outTrackId"] = outTrackId,
                ["cardData"] = cardData,
                ["userIdType"] = 1,
                ["openSpaceType"] = "application",
                ["conversationType"] = 1,
                ["userId"] = userId
            };
            string bodyJson = JsonSerializer.Serialize(body);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/card/instances")
            {
                Content = new StringContent(bodyJson, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("x-acs-dingtalk-access-token", token);

            using var response = await _httpClient.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                if (doc.RootElement.TryGetProperty("cardInstanceId", out var idEl))
                {
                    return idEl.GetString() ?? string.Empty;
                }
                _logger.Error(null, "钉钉创建互动卡片失败: {0}", responseBody);
            }
            catch (JsonException)
            {
                _logger.Error(null, "钉钉 API 返回非 JSON 响应: {0}", responseBody);
            }

            return string.Empty;
        }

        /// <summary>
        /// 更新互动卡片实例，返回是否成功。
        /// </summary>
        public async Task<bool> UpdateCardInstanceAsync(string cardInstanceId, string cardDataJson)
        {
            if (string.IsNullOrEmpty(cardInstanceId))
                return false;

            string token = await _tokenManager.GetTokenAsync();

            JsonElement cardData;
            using (var doc = JsonDocument.Parse(cardDataJson))
            {
                cardData = doc.RootElement.Clone();
            }

            var body = new Dictionary<string, object>
            {
                ["cardData"] = cardData
            };
            string bodyJson = JsonSerializer.Serialize(body);

            var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/card/instances/{Uri.EscapeDataString(cardInstanceId)}")
            {
                Content = new StringContent(bodyJson, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("x-acs-dingtalk-access-token", token);

            using var response = await _httpClient.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                _logger.Error(null, "钉钉更新互动卡片失败: {0}", responseBody);
            }
            catch (JsonException)
            {
                _logger.Error(null, "钉钉 API 返回非 JSON 响应: {0}", responseBody);
            }

            return false;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }

    // ================================================================
    // 内嵌类：access_token 管理
    // ================================================================

    /// <summary>
    /// 钉钉 access_token 管理，带缓存和自动刷新。
    /// </summary>
    internal class DingTalkAccessToken
    {
        private static readonly ILogger _logger = LogManager.Instance.GetLogger<DingTalkAccessToken>();
        private readonly string _appKey;
        private readonly string _appSecret;
        private readonly HttpClient _httpClient;
        private readonly SemaphoreSlim _lock = new(1, 1);

        private string? _token;
        private DateTime _expiresAt;

        private const string TokenUrl = "https://api.dingtalk.com/v1.0/oauth2/accessToken";

        public DingTalkAccessToken(string appKey, string appSecret, HttpClient httpClient)
        {
            _appKey = appKey;
            _appSecret = appSecret;
            _httpClient = httpClient;
        }

        /// <summary>
        /// 获取有效的 access_token，过期时自动刷新。
        /// </summary>
        public async Task<string> GetTokenAsync()
        {
            if (_token != null && DateTime.UtcNow < _expiresAt)
                return _token;

            await _lock.WaitAsync();
            try
            {
                if (_token != null && DateTime.UtcNow < _expiresAt)
                    return _token;

                await RefreshTokenAsync();
                return _token!;
            }
            finally
            {
                _lock.Release();
            }
        }

        /// <summary>
        /// 刷新 access_token。
        /// </summary>
        private async Task RefreshTokenAsync()
        {
            var body = new Dictionary<string, string>
            {
                ["appKey"] = _appKey,
                ["appSecret"] = _appSecret
            };
            string bodyJson = JsonSerializer.Serialize(body);

            var content = new StringContent(bodyJson, Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(TokenUrl, content);
            string responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                if (doc.RootElement.TryGetProperty("accessToken", out var tokenEl))
                {
                    _token = tokenEl.GetString();
                    int expireIn = doc.RootElement.TryGetProperty("expireIn", out var expireEl)
                        ? expireEl.GetInt32()
                        : 7200;
                    _expiresAt = DateTime.UtcNow.AddSeconds(Math.Max(expireIn - 300, 60));
                    _logger.Debug(null, "钉钉 access_token 已刷新，有效期 {0} 秒", expireIn);
                }
                else
                {
                    string msg = doc.RootElement.TryGetProperty("message", out var msgEl)
                        ? (msgEl.GetString() ?? "未知错误")
                        : "未知错误";
                    throw new InvalidOperationException($"获取钉钉 access_token 失败: {msg}");
                }
            }
            catch (JsonException)
            {
                throw new InvalidOperationException($"钉钉 access_token 响应解析失败: {responseBody}");
            }
        }
    }

    // ================================================================
    // 内嵌类：互动卡片构建器
    // ================================================================

    /// <summary>
    /// 构建钉钉互动卡片 JSON。
    /// </summary>
    internal class DingTalkCardBuilder
    {
        /// <summary>
        /// 构建权限请求卡片，包含"允许"和"拒绝"按钮。
        /// </summary>
        public static string BuildPermissionCard(string permissionType, string resource, string allowCode, string denyCode)
        {
            var card = new
            {
                cardParamMap = new Dictionary<string, string>
                {
                    ["title"] = "权限请求",
                    ["content"] = $"**类型**: {permissionType}\n**资源**: {resource}\n\n请选择是否允许此操作：",
                    ["allowCode"] = allowCode,
                    ["denyCode"] = denyCode
                }
            };

            return JsonSerializer.Serialize(card);
        }

        /// <summary>
        /// 构建权限请求结果卡片（允许/拒绝后显示）。
        /// </summary>
        public static string BuildResultCard(bool allowed, string permissionType, string resource)
        {
            string status = allowed ? "已允许" : "已拒绝";
            var card = new
            {
                cardParamMap = new Dictionary<string, string>
                {
                    ["title"] = $"权限请求 - {status}",
                    ["content"] = $"**类型**: {permissionType}\n**资源**: {resource}\n\n本卡片按钮已失效。"
                }
            };

            return JsonSerializer.Serialize(card);
        }

        /// <summary>
        /// 构建权限请求超时卡片。
        /// </summary>
        public static string BuildTimeoutCard(string permissionType, string resource)
        {
            var card = new
            {
                cardParamMap = new Dictionary<string, string>
                {
                    ["title"] = "权限请求 - 已超时",
                    ["content"] = $"**类型**: {permissionType}\n**资源**: {resource}\n\n未在规定时间内响应，已自动拒绝。"
                }
            };

            return JsonSerializer.Serialize(card);
        }

        /// <summary>
        /// 构建纯文本卡片（用于流式更新）。
        /// </summary>
        public static string BuildTextCard(string content)
        {
            var card = new
            {
                cardParamMap = new Dictionary<string, string>
                {
                    ["title"] = "消息",
                    ["content"] = content
                }
            };

            return JsonSerializer.Serialize(card);
        }
    }

    // ================================================================
    // 内嵌类：事件模型
    // ================================================================

    /// <summary>
    /// 钉钉事件回调模型。
    /// </summary>
    internal record DingTalkEventMessage
    {
        [JsonPropertyName("schema")]
        public string? Schema { get; init; }

        [JsonPropertyName("header")]
        public DingTalkEventHeader? Header { get; init; }

        [JsonPropertyName("event")]
        public JsonElement? Event { get; init; }
    }

    /// <summary>
    /// 钉钉事件头模型。
    /// </summary>
    internal record DingTalkEventHeader
    {
        [JsonPropertyName("eventId")]
        public string? EventId { get; init; }

        [JsonPropertyName("eventType")]
        public string? EventType { get; init; }

        [JsonPropertyName("eventBornTime")]
        public string? EventBornTime { get; init; }

        [JsonPropertyName("tenantId")]
        public string? TenantId { get; init; }

        [JsonPropertyName("userId")]
        public string? UserId { get; init; }

        [JsonPropertyName("role")]
        public string? Role { get; init; }

        [JsonPropertyName("connectionId")]
        public string? ConnectionId { get; init; }
    }
}
