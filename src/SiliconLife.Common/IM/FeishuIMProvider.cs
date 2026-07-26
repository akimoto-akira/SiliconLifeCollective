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
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using SiliconLife.Collective;

namespace SiliconLife.Common.IM;

/// <summary>
/// 飞书（Lark）IM Provider 实现。
/// 通过 HttpListener 监听飞书事件回调，支持文本消息收发、流式更新、交互卡片权限请求。
/// </summary>
public class FeishuIMProvider : ExternalIMProviderBase
{
    // ---- 配置字段 ----
    private readonly string _appId;
    private readonly string _appSecret;
    private readonly string _verificationToken;
    private readonly string? _encryptKey;
    private readonly string _callbackPath;
    private readonly int _listenPort;

    // ---- API 客户端 ----
    private readonly FeishuApiClient _apiClient;

    // ---- 渠道映射：飞书 open_id <-> 内部 channelId ----
    private readonly ConcurrentDictionary<string, Guid> _feishuIdToChannel = new();
    private readonly ConcurrentDictionary<Guid, string> _channelToFeishuId = new();

    // ---- 最近活跃用户（用于发送权限请求卡片）----
    private string? _lastActiveOpenId;
    private readonly object _lastActiveLock = new();

    // ---- 权限卡片消息 ID（用于超时后更新卡片）----
    private string? _permissionCardMessageId;
    private readonly object _permissionCardLock = new();

    // ---- 事件去重 ----
    private readonly ConcurrentDictionary<string, DateTime> _processedEventIds = new();

    // ---- HttpListener ----
    private HttpListener? _listener;
    private CancellationTokenSource? _listenerCts;

    /// <summary>
    /// 创建飞书 IM Provider 实例。
    /// </summary>
    /// <param name="config">配置字典，包含 appId、appSecret、verificationToken、encryptKey、callbackPath、listenPort 等项</param>
    public FeishuIMProvider(Dictionary<string, object> config)
    {
        _appId = GetConfigString(config, "appId");
        _appSecret = GetConfigString(config, "appSecret");
        _verificationToken = GetConfigString(config, "verificationToken");
        _encryptKey = GetConfigStringOrNull(config, "encryptKey");
        _callbackPath = GetConfigString(config, "callbackPath", "/feishu/callback");
        _listenPort = GetConfigInt(config, "listenPort", 8080);

        if (string.IsNullOrEmpty(_appId) || string.IsNullOrEmpty(_appSecret))
        {
            throw new ArgumentException("飞书配置缺少 appId 或 appSecret");
        }

        _apiClient = new FeishuApiClient(_appId, _appSecret);

        _logger.Info(null, "FeishuIMProvider 初始化完成: callbackPath={0}, listenPort={1}, encryptKey={2}",
            _callbackPath, _listenPort, string.IsNullOrEmpty(_encryptKey) ? "未配置" : "已配置");
    }

    // ================================================================
    // 生命周期管理
    // ================================================================

    /// <inheritdoc/>
    public override async Task StartAsync()
    {
        _listenerCts = new CancellationTokenSource();
        var token = _listenerCts.Token;

        _ = Task.Run(() => ListenForCallbacksAsync(token), token);

        _logger.Info(null, "FeishuIMProvider 已启动，监听端口 {0}", _listenPort);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public override async Task StopAsync()
    {
        _listenerCts?.Cancel();

        try
        {
            if (_listener?.IsListening == true)
            {
                _listener.Stop();
            }
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "停止 Feishu HttpListener 时出错: {0}", ex.Message);
        }

        _listenerCts?.Dispose();
        _listenerCts = null;
        _listener = null;

        _apiClient.Dispose();
        _logger.Info(null, "FeishuIMProvider 已停止");
        await Task.CompletedTask;
    }

    // ================================================================
    // 消息发送核心实现
    // ================================================================

    /// <inheritdoc/>
    protected override async Task<string> SendMessageCoreAsync(
        Guid senderId, Guid channelId, string content, string? senderName = null)
    {
        if (channelId == Guid.Empty)
        {
            // 基类 AskPermissionAsync 使用 Guid.Empty，这里取最近活跃用户
            string? fallbackOpenId;
            lock (_lastActiveLock)
            {
                fallbackOpenId = _lastActiveOpenId;
            }

            if (fallbackOpenId != null)
            {
                string fallbackContent = JsonSerializer.Serialize(new Dictionary<string, string> { ["text"] = content });
                return await _apiClient.SendMessageAsync(fallbackOpenId, "open_id", "text", fallbackContent);
            }
            _logger.Warn(null, "无法发送飞书消息：channelId 为空且无活跃用户");
            return string.Empty;
        }

        if (!_channelToFeishuId.TryGetValue(channelId, out string? openId) || string.IsNullOrEmpty(openId))
        {
            _logger.Warn(null, "无法发送飞书消息：channelId {0} 未映射到 open_id", channelId);
            return string.Empty;
        }

        string textContent = JsonSerializer.Serialize(new Dictionary<string, string> { ["text"] = content });
        return await _apiClient.SendMessageAsync(openId, "open_id", "text", textContent);
    }

    /// <inheritdoc/>
    protected override async Task<bool> UpdateMessageCoreAsync(string messageId, string content)
    {
        if (string.IsNullOrEmpty(messageId))
            return false;

        string textContent = JsonSerializer.Serialize(new Dictionary<string, string> { ["text"] = content });
        return await _apiClient.UpdateMessageAsync(messageId, textContent);
    }

    // ================================================================
    // 权限请求（使用交互卡片）
    // ================================================================

    /// <inheritdoc/>
    public override async Task<AskPermissionResult> AskPermissionAsync(
        PermissionType permissionType, string resource,
        string allowCode, string denyCode)
    {
        Guid userId = Config.Instance.Data.CuratorGuid;

        // 确定权限卡片的目标用户
        string? targetOpenId;
        lock (_lastActiveLock)
        {
            targetOpenId = _lastActiveOpenId;
        }

        if (string.IsNullOrEmpty(targetOpenId))
        {
            _logger.Warn(null, "无活跃飞书用户，无法发送权限请求卡片");
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

        string cardMessageId = string.Empty;

        request.TimeoutCts.Token.Register(() =>
        {
            _permissionQueue.HandleTimeout(request);
            // 超时后更新卡片显示
            lock (_permissionCardLock)
            {
                cardMessageId = _permissionCardMessageId ?? string.Empty;
            }
            if (!string.IsNullOrEmpty(cardMessageId))
            {
                var timeoutCard = FeishuCardBuilder.BuildTimeoutCard(permissionType.ToString(), resource);
                _ = _apiClient.UpdateMessageAsync(cardMessageId, timeoutCard);
            }
        });

        // 构建并发送交互卡片
        string cardJson = FeishuCardBuilder.BuildPermissionCard(
            permissionType.ToString(), resource, allowCode, denyCode);

        try
        {
            string msgId = await _apiClient.SendMessageAsync(targetOpenId, "open_id", "interactive", cardJson);
            lock (_permissionCardLock)
            {
                _permissionCardMessageId = msgId;
            }
            _logger.Info(userId, "飞书权限请求卡片已发送: {0} -> {1}", request.RequestId, msgId);
        }
        catch (Exception ex)
        {
            _logger.Error(userId, "发送飞书权限请求卡片失败: {0}", ex.Message);
            return new AskPermissionResult { Allowed = false };
        }

        return await _permissionQueue.EnqueueAsync(request);
    }

    // ================================================================
    // HttpListener 回调监听
    // ================================================================

    /// <summary>
    /// 监听飞书事件回调。
    /// </summary>
    private async Task ListenForCallbacksAsync(CancellationToken ct)
    {
        _listener = new HttpListener();

        string path = _callbackPath.TrimEnd('/');
        if (!path.StartsWith('/'))
            path = '/' + path;
        string prefix = $"http://+:{_listenPort}{path}/";

        try
        {
            _listener.Prefixes.Add(prefix);
            _listener.Start();
            _logger.Info(null, "飞书回调监听已启动: {0}", prefix);
        }
        catch (HttpListenerException ex)
        {
            _logger.Error(null, "无法启动飞书 HttpListener 于 {0}: {1}", prefix, ex.Message);
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
                    context = await _listener.GetContextAsync();
                }
                catch (HttpListenerException) when (ct.IsCancellationRequested)
                {
                    break;
                }
                catch (ObjectDisposedException) when (ct.IsCancellationRequested)
                {
                    break;
                }

                // 异步处理请求，不阻塞监听循环
                _ = HandleCallbackAsync(context);
            }
        }
        catch (Exception ex)
        {
            if (!ct.IsCancellationRequested)
                _logger.Error(null, "飞书回调监听异常: {0}", ex.Message);
        }
        finally
        {
            try
            {
                if (_listener?.IsListening == true)
                    _listener.Stop();
            }
            catch { /* 忽略关闭错误 */ }
        }
    }

    /// <summary>
    /// 处理单个飞书回调请求。
    /// </summary>
    private async Task HandleCallbackAsync(HttpListenerContext context)
    {
        try
        {
            string body;
            using (var reader = new StreamReader(context.Request.InputStream, Encoding.UTF8))
            {
                body = await reader.ReadToEndAsync();
            }

            // 签名验证
            if (!string.IsNullOrEmpty(_encryptKey))
            {
                string? signature = context.Request.Headers["X-Lark-Signature"];
                string? timestamp = context.Request.Headers["X-Lark-Request-Timestamp"];
                string? nonce = context.Request.Headers["X-Lark-Request-Nonce"];

                if (!string.IsNullOrEmpty(signature) && !string.IsNullOrEmpty(timestamp) && !string.IsNullOrEmpty(nonce))
                {
                    if (!VerifySignature(timestamp, nonce, _encryptKey, body, signature))
                    {
                        _logger.Warn(null, "飞书回调签名验证失败");
                        context.Response.StatusCode = 403;
                        await WriteResponseAsync(context.Response, """{"code":403}""");
                        return;
                    }
                }
            }

            // 解析请求体，检查是否加密
            string eventJson = body;

            try
            {
                using (var doc = JsonDocument.Parse(body))
                {
                    if (doc.RootElement.TryGetProperty("encrypt", out var encryptEl) && !string.IsNullOrEmpty(_encryptKey))
                    {
                        string? encryptedData = encryptEl.GetString();
                        if (!string.IsNullOrEmpty(encryptedData))
                        {
                            eventJson = DecryptEvent(encryptedData, _encryptKey);
                        }
                    }
                }
            }
            catch (JsonException ex)
            {
                _logger.Warn(null, "解析飞书回调 JSON 失败: {0}", ex.Message);
                context.Response.StatusCode = 400;
                await WriteResponseAsync(context.Response, """{"code":400}""");
                return;
            }

            // 解析（可能已解密的）事件 JSON
            using (var eventDoc = JsonDocument.Parse(eventJson))
            {
                var root = eventDoc.RootElement;

                // URL 验证
                if (root.TryGetProperty("type", out var typeEl) &&
                    typeEl.GetString() == "url_verification")
                {
                    string challenge = root.TryGetProperty("challenge", out var challengeEl)
                        ? challengeEl.GetString() ?? string.Empty
                        : string.Empty;
                    _logger.Info(null, "飞书 URL 验证: challenge={0}", challenge);
                    await WriteResponseAsync(context.Response,
                        JsonSerializer.Serialize(new Dictionary<string, string> { ["challenge"] = challenge }));
                    return;
                }

                // 立即返回 200，避免飞书超时重试
                await WriteResponseAsync(context.Response, """{"code":0}""");

                // 后台处理事件
                string eventJsonCopy = eventJson;
                _ = Task.Run(() => ProcessEventAsync(eventJsonCopy));
            }
        }
        catch (Exception ex)
        {
            _logger.Error(null, "处理飞书回调异常: {0}", ex.Message);
            try
            {
                context.Response.StatusCode = 500;
                await WriteResponseAsync(context.Response, """{"code":500}""");
            }
            catch { /* 忽略响应写入错误 */ }
        }
    }

    // ================================================================
    // 事件处理
    // ================================================================

    /// <summary>
    /// 异步处理飞书事件。
    /// </summary>
    private void ProcessEventAsync(string eventJson)
    {
        try
        {
            using var doc = JsonDocument.Parse(eventJson);
            var root = doc.RootElement;

            // v2 格式：包含 header 字段
            if (root.TryGetProperty("header", out var headerEl))
            {
                string? eventType = headerEl.TryGetProperty("event_type", out var etEl) ? etEl.GetString() : null;
                string? eventId = headerEl.TryGetProperty("event_id", out var idEl) ? idEl.GetString() : null;
                string? token = headerEl.TryGetProperty("token", out var tokenEl) ? tokenEl.GetString() : null;

                // 验证 token
                if (!string.IsNullOrEmpty(token) && token != _verificationToken)
                {
                    _logger.Warn(null, "飞书事件 token 不匹配，已跳过");
                    return;
                }

                // 事件去重
                if (!string.IsNullOrEmpty(eventId) && !TryMarkEventProcessed(eventId))
                {
                    _logger.Debug(null, "跳过重复的飞书事件: {0}", eventId);
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
                    _logger.Debug(null, "未处理的飞书事件类型: {0}", eventType);
                }
            }
            else
            {
                // v1 格式：直接在根层级（如旧版卡片回调）
                if (root.TryGetProperty("action", out _))
                {
                    ProcessCardActionCallback(root);
                }
                else
                {
                    _logger.Debug(null, "未识别的飞书回调格式");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error(null, "处理飞书事件异常: {0}", ex.Message);
        }
    }

    /// <summary>
    /// 处理消息接收事件 (im.message.receive_v1)。
    /// </summary>
    private void ProcessMessageReceiveEvent(JsonElement eventEl)
    {
        try
        {
            // 提取发送者 open_id
            string? openId = null;
            if (eventEl.TryGetProperty("sender", out var senderEl) &&
                senderEl.TryGetProperty("sender_id", out var senderIdEl) &&
                senderIdEl.TryGetProperty("open_id", out var openIdEl))
            {
                openId = openIdEl.GetString();
            }

            if (string.IsNullOrEmpty(openId))
            {
                _logger.Warn(null, "飞书消息事件缺少发送者 open_id");
                return;
            }

            // 提取消息内容
            if (!eventEl.TryGetProperty("message", out var messageEl))
            {
                _logger.Warn(null, "飞书消息事件缺少 message 字段");
                return;
            }

            string? messageType = messageEl.TryGetProperty("message_type", out var msgTypeEl)
                ? msgTypeEl.GetString()
                : null;

            // 仅处理文本消息
            if (messageType != "text")
            {
                _logger.Debug(null, "跳过非文本飞书消息: type={0}", messageType);
                return;
            }

            string? contentJson = messageEl.TryGetProperty("content", out var contentEl)
                ? contentEl.GetString()
                : null;

            if (string.IsNullOrEmpty(contentJson))
            {
                _logger.Warn(null, "飞书消息内容为空");
                return;
            }

            string text = ExtractTextContent(contentJson);
            if (string.IsNullOrWhiteSpace(text))
            {
                _logger.Debug(null, "飞书消息文本为空，已跳过");
                return;
            }

            // 映射 open_id 到内部 channelId
            Guid channelId = GetOrCreateChannelId(openId);
            Guid senderId = channelId; // 外部 IM 场景下，发送者 ID 与渠道 ID 一致

            // 记录最近活跃用户
            lock (_lastActiveLock)
            {
                _lastActiveOpenId = openId;
            }

            // 构造 ChatMessage 并触发事件
            var chatMessage = new ChatMessage
            {
                Id = Guid.NewGuid(),
                SenderId = senderId,
                ChannelId = channelId,
                Content = text,
                Timestamp = DateTime.Now,
                Type = MessageType.Text
            };

            _logger.Info(senderId, "收到飞书消息: channelId={0}, text={1}",
                channelId, text.Length > 50 ? text[..50] + "..." : text);

            OnMessageReceived(new IMMessageEventArgs(chatMessage));
        }
        catch (Exception ex)
        {
            _logger.Error(null, "处理飞书消息接收事件异常: {0}", ex.Message);
        }
    }

    /// <summary>
    /// 处理卡片按钮回调 (card.action.trigger)。
    /// </summary>
    private void ProcessCardActionCallback(JsonElement element)
    {
        try
        {
            // 提取 action.value
            if (!element.TryGetProperty("action", out var actionEl) ||
                !actionEl.TryGetProperty("value", out var valueEl))
            {
                _logger.Debug(null, "飞书卡片回调缺少 action.value");
                return;
            }

            string? code = valueEl.TryGetProperty("code", out var codeEl) ? codeEl.GetString() : null;
            string? action = valueEl.TryGetProperty("action", out var actionNameEl) ? actionNameEl.GetString() : null;
            string? openMessageId = element.TryGetProperty("open_message_id", out var msgIdEl)
                ? msgIdEl.GetString()
                : null;

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(action))
            {
                _logger.Debug(null, "飞书卡片回调缺少 code 或 action");
                return;
            }

            // 获取当前活跃的权限请求
            var activeRequest = _permissionQueue.GetActiveRequest();
            if (activeRequest == null)
            {
                _logger.Warn(null, "收到飞书卡片回调但无活跃权限请求");
                return;
            }

            bool allowed = action == "allow";
            string expectedCode = allowed ? activeRequest.AllowCode : activeRequest.DenyCode;

            if (code != expectedCode)
            {
                _logger.Warn(null, "飞书卡片回调 code 不匹配: 期望={0}, 实际={1}", expectedCode, code);
                return;
            }

            _logger.Info(activeRequest.UserId, "飞书权限请求通过卡片响应: {0} - {1}",
                activeRequest.PermissionType, allowed ? "允许" : "拒绝");

            // 完成权限请求
            _permissionQueue.HandleResponse(activeRequest.UserId, allowed, addToCache: false, cacheDuration: null);

            // 更新卡片显示结果
            if (!string.IsNullOrEmpty(openMessageId))
            {
                string resultCard = FeishuCardBuilder.BuildResultCard(
                    allowed, activeRequest.PermissionType.ToString(), activeRequest.Resource);
                _ = _apiClient.UpdateMessageAsync(openMessageId, resultCard).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                        _logger.Error(null, "更新飞书权限卡片失败: {0}", t.Exception?.GetBaseException().Message);
                });
            }
        }
        catch (Exception ex)
        {
            _logger.Error(null, "处理飞书卡片回调异常: {0}", ex.Message);
        }
    }

    // ================================================================
    // 渠道映射
    // ================================================================

    /// <summary>
    /// 获取或创建飞书 open_id 到内部 channelId 的映射。
    /// 使用基于 open_id 哈希的确定性 Guid，保证同一用户始终映射到同一 channelId。
    /// </summary>
    private Guid GetOrCreateChannelId(string openId)
    {
        if (_feishuIdToChannel.TryGetValue(openId, out Guid existingId))
            return existingId;

        Guid newId = DeterministicGuid("feishu:" + openId);
        if (_feishuIdToChannel.TryAdd(openId, newId))
        {
            _channelToFeishuId[newId] = openId;
            _logger.Info(null, "创建飞书渠道映射: open_id={0} -> channelId={1}", openId, newId);
        }
        else
        {
            // 并发情况下，其他线程可能已添加
            newId = _feishuIdToChannel[openId];
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
    // 签名验证与事件解密
    // ================================================================

    /// <summary>
    /// 验证飞书回调签名。
    /// 签名算法: SHA256(timestamp + nonce + encryptKey + body)
    /// </summary>
    private static bool VerifySignature(string timestamp, string nonce, string encryptKey, string body, string signature)
    {
        string toHash = timestamp + nonce + encryptKey + body;
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(toHash));
        string computed = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        return string.Equals(computed, signature, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 解密飞书加密事件。
    /// 加密方式: AES-256-CBC，key = SHA256(encryptKey)[:32]，IV = 密文前 16 字节。
    /// </summary>
    private static string DecryptEvent(string encryptedBase64, string encryptKey)
    {
        byte[] key = SHA256.HashData(Encoding.UTF8.GetBytes(encryptKey));
        byte[] fullData = Convert.FromBase64String(encryptedBase64);

        if (fullData.Length < 16)
            throw new InvalidOperationException("加密数据长度不足");

        byte[] iv = new byte[16];
        Array.Copy(fullData, 0, iv, 0, 16);

        byte[] ciphertext = new byte[fullData.Length - 16];
        Array.Copy(fullData, 16, ciphertext, 0, ciphertext.Length);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor();
        byte[] plaintext = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
        return Encoding.UTF8.GetString(plaintext);
    }

    // ================================================================
    // 辅助方法
    // ================================================================

    /// <summary>
    /// 从飞书消息内容 JSON 中提取纯文本，并去除 @mention 占位符。
    /// </summary>
    private static string ExtractTextContent(string contentJson)
    {
        try
        {
            using var doc = JsonDocument.Parse(contentJson);
            if (doc.RootElement.TryGetProperty("text", out var textEl))
            {
                string text = textEl.GetString() ?? string.Empty;
                // 去除飞书 @mention 占位符（如 @_user_1、@_all）
                text = Regex.Replace(text, @"@_user_\d+", "");
                text = Regex.Replace(text, "@_all", "");
                return text.Trim();
            }
        }
        catch (JsonException)
        {
            // 内容不是有效 JSON，原样返回
        }
        return contentJson;
    }

    /// <summary>
    /// 写入 HTTP 响应。
    /// </summary>
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

        // 定期清理过期记录（保留 10 分钟内）
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

    // ---- 配置读取辅助 ----

    private static string GetConfigString(Dictionary<string, object> config, string key, string defaultValue = "")
    {
        if (config.TryGetValue(key, out object? value) && value != null)
            return value.ToString() ?? defaultValue;
        return defaultValue;
    }

    private static string? GetConfigStringOrNull(Dictionary<string, object> config, string key)
    {
        if (config.TryGetValue(key, out object? value) && value != null)
        {
            string s = value.ToString() ?? string.Empty;
            return string.IsNullOrEmpty(s) ? null : s;
        }
        return null;
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
    // 内嵌类：飞书 API 客户端
    // ================================================================

    /// <summary>
    /// 封装飞书 Open API 调用，包含 token 管理。
    /// </summary>
    internal class FeishuApiClient : IDisposable
    {
        private static readonly ILogger _logger = LogManager.Instance.GetLogger<FeishuApiClient>();
        private readonly HttpClient _httpClient;
        private readonly FeishuAccessToken _tokenManager;

        private const string BaseUrl = "https://open.feishu.cn/open-apis";

        public FeishuApiClient(string appId, string appSecret)
        {
            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
            _tokenManager = new FeishuAccessToken(appId, appSecret, _httpClient);
        }

        /// <summary>
        /// 发送消息，返回 message_id。
        /// </summary>
        public async Task<string> SendMessageAsync(string receiveId, string receiveIdType, string msgType, string content)
        {
            string token = await _tokenManager.GetTokenAsync();

            var body = new Dictionary<string, string>
            {
                ["receive_id"] = receiveId,
                ["msg_type"] = msgType,
                ["content"] = content
            };
            string bodyJson = JsonSerializer.Serialize(body);

            var request = new HttpRequestMessage(HttpMethod.Post,
                $"{BaseUrl}/im/v1/messages?receive_id_type={receiveIdType}")
            {
                Content = new StringContent(bodyJson, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Authorization", $"Bearer {token}");

            using var response = await _httpClient.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                if (doc.RootElement.TryGetProperty("code", out var codeEl) && codeEl.GetInt32() == 0)
                {
                    if (doc.RootElement.TryGetProperty("data", out var dataEl) &&
                        dataEl.TryGetProperty("message_id", out var msgIdEl))
                    {
                        return msgIdEl.GetString() ?? string.Empty;
                    }
                }
                _logger.Error(null, "飞书发送消息失败: {0}", responseBody);
            }
            catch (JsonException)
            {
                _logger.Error(null, "飞书 API 返回非 JSON 响应: {0}", responseBody);
            }

            return string.Empty;
        }

        /// <summary>
        /// 更新消息内容（用于流式更新），返回是否成功。
        /// </summary>
        public async Task<bool> UpdateMessageAsync(string messageId, string content)
        {
            if (string.IsNullOrEmpty(messageId))
                return false;

            string token = await _tokenManager.GetTokenAsync();

            var body = new Dictionary<string, string> { ["content"] = content };
            string bodyJson = JsonSerializer.Serialize(body);

            var request = new HttpRequestMessage(HttpMethod.Patch,
                $"{BaseUrl}/im/v1/messages/{messageId}")
            {
                Content = new StringContent(bodyJson, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Authorization", $"Bearer {token}");

            using var response = await _httpClient.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                if (doc.RootElement.TryGetProperty("code", out var codeEl) && codeEl.GetInt32() == 0)
                {
                    return true;
                }
                _logger.Error(null, "飞书更新消息失败: {0}", responseBody);
            }
            catch (JsonException)
            {
                _logger.Error(null, "飞书 API 返回非 JSON 响应: {0}", responseBody);
            }

            return false;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }

    // ================================================================
    // 内嵌类：交互卡片构建器
    // ================================================================

    /// <summary>
    /// 构建飞书交互卡片 JSON。
    /// </summary>
    internal class FeishuCardBuilder
    {
        /// <summary>
        /// 构建权限请求卡片，包含"允许"和"拒绝"按钮。
        /// </summary>
        public static string BuildPermissionCard(string permissionType, string resource, string allowCode, string denyCode)
        {
            var card = new
            {
                config = new { wide_screen_mode = true },
                elements = new object[]
                {
                    new
                    {
                        tag = "div",
                        text = new
                        {
                            content = $"**权限请求**\n\n**类型**: {permissionType}\n**资源**: {resource}\n\n请选择是否允许此操作：",
                            tag = "lark_md"
                        }
                    },
                    new
                    {
                        tag = "action",
                        actions = new object[]
                        {
                            new
                            {
                                tag = "button",
                                text = new { content = "允许", tag = "plain_text" },
                                value = new { code = allowCode, action = "allow" },
                                type = "primary"
                            },
                            new
                            {
                                tag = "button",
                                text = new { content = "拒绝", tag = "plain_text" },
                                value = new { code = denyCode, action = "deny" },
                                type = "danger"
                            }
                        }
                    }
                }
            };

            return JsonSerializer.Serialize(card);
        }

        /// <summary>
        /// 构建权限请求结果卡片（允许/拒绝后显示）。
        /// </summary>
        public static string BuildResultCard(bool allowed, string permissionType, string resource)
        {
            string icon = allowed ? "已允许" : "已拒绝";

            var card = new
            {
                config = new { wide_screen_mode = true },
                elements = new object[]
                {
                    new
                    {
                        tag = "div",
                        text = new
                        {
                            content = $"**权限请求{icon}**\n\n**类型**: {permissionType}\n**资源**: {resource}",
                            tag = "lark_md"
                        }
                    }
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
                config = new { wide_screen_mode = true },
                elements = new object[]
                {
                    new
                    {
                        tag = "div",
                        text = new
                        {
                            content = $"**权限请求已超时**\n\n**类型**: {permissionType}\n**资源**: {resource}\n\n未在规定时间内响应，已自动拒绝。",
                            tag = "lark_md"
                        }
                    }
                }
            };

            return JsonSerializer.Serialize(card);
        }

        /// <summary>
        /// 构建纯文本卡片。
        /// </summary>
        public static string BuildTextCard(string content)
        {
            var card = new
            {
                config = new { wide_screen_mode = true },
                elements = new object[]
                {
                    new
                    {
                        tag = "div",
                        text = new { content = content, tag = "lark_md" }
                    }
                }
            };

            return JsonSerializer.Serialize(card);
        }
    }

    // ================================================================
    // 内嵌类：tenant_access_token 管理
    // ================================================================

    /// <summary>
    /// 飞书 tenant_access_token 管理，带缓存和自动刷新。
    /// </summary>
    internal class FeishuAccessToken
    {
        private static readonly ILogger _logger = LogManager.Instance.GetLogger<FeishuAccessToken>();
        private readonly string _appId;
        private readonly string _appSecret;
        private readonly HttpClient _httpClient;
        private readonly SemaphoreSlim _lock = new(1, 1);

        private string? _token;
        private DateTime _expiresAt;

        private const string TokenUrl = "https://open.feishu.cn/open-apis/auth/v3/tenant_access_token/internal";

        public FeishuAccessToken(string appId, string appSecret, HttpClient httpClient)
        {
            _appId = appId;
            _appSecret = appSecret;
            _httpClient = httpClient;
        }

        /// <summary>
        /// 获取有效的 tenant_access_token，过期时自动刷新。
        /// </summary>
        public async Task<string> GetTokenAsync()
        {
            // 快速路径：token 仍然有效
            if (_token != null && DateTime.UtcNow < _expiresAt)
                return _token;

            await _lock.WaitAsync();
            try
            {
                // 双重检查：获取锁后再次确认
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
        /// 刷新 tenant_access_token。
        /// </summary>
        private async Task RefreshTokenAsync()
        {
            var body = new Dictionary<string, string>
            {
                ["app_id"] = _appId,
                ["app_secret"] = _appSecret
            };
            string bodyJson = JsonSerializer.Serialize(body);

            var content = new StringContent(bodyJson, Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(TokenUrl, content);
            string responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                if (doc.RootElement.TryGetProperty("code", out var codeEl) && codeEl.GetInt32() == 0)
                {
                    _token = doc.RootElement.GetProperty("tenant_access_token").GetString();
                    int expire = doc.RootElement.TryGetProperty("expire", out var expireEl)
                        ? expireEl.GetInt32()
                        : 7200;
                    // 提前 5 分钟刷新，避免边界过期
                    _expiresAt = DateTime.UtcNow.AddSeconds(Math.Max(expire - 300, 60));
                    _logger.Debug(null, "飞书 token 已刷新，有效期 {0} 秒", expire);
                }
                else
                {
                    string msg = doc.RootElement.TryGetProperty("msg", out var msgEl)
                        ? msgEl.GetString() ?? "未知错误"
                        : "未知错误";
                    throw new InvalidOperationException($"获取飞书 token 失败: {msg}");
                }
            }
            catch (JsonException)
            {
                throw new InvalidOperationException($"飞书 token 响应解析失败: {responseBody}");
            }
        }
    }

    // ================================================================
    // 内嵌类：事件模型
    // ================================================================

    /// <summary>
    /// 飞书事件回调模型。
    /// </summary>
    internal record FeishuEventMessage
    {
        [JsonPropertyName("schema")]
        public string? Schema { get; init; }

        [JsonPropertyName("header")]
        public FeishuEventHeader? Header { get; init; }

        [JsonPropertyName("event")]
        public JsonElement? Event { get; init; }

        /// <summary>
        /// URL 验证回显字段。
        /// </summary>
        [JsonPropertyName("challenge")]
        public string? Challenge { get; init; }

        /// <summary>
        /// 事件订阅 Verification Token。
        /// </summary>
        [JsonPropertyName("token")]
        public string? Token { get; init; }

        /// <summary>
        /// 回调类型（如 "url_verification"）。
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; init; }

        /// <summary>
        /// 加密事件密文（base64）。
        /// </summary>
        [JsonPropertyName("encrypt")]
        public string? Encrypt { get; init; }
    }

    /// <summary>
    /// 飞书事件头模型。
    /// </summary>
    internal record FeishuEventHeader
    {
        [JsonPropertyName("event_id")]
        public string? EventId { get; init; }

        [JsonPropertyName("event_type")]
        public string? EventType { get; init; }

        [JsonPropertyName("create_time")]
        public string? CreateTime { get; init; }

        [JsonPropertyName("token")]
        public string? Token { get; init; }

        [JsonPropertyName("app_id")]
        public string? AppId { get; init; }

        [JsonPropertyName("tenant_key")]
        public string? TenantKey { get; init; }
    }
}
