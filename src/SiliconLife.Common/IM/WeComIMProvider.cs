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
using System.Xml.Linq;
using SiliconLife.Collective;

namespace SiliconLife.Common.IM;

/// <summary>
/// 企业微信（WeCom）IM Provider 实现。
/// 通过 HttpListener 监听企业微信回调，支持文本消息收发、流式累积、模板卡片权限请求。
/// 使用 <see cref="WXBizMsgCrypt"/> 进行回调消息加解密与签名验证。
/// </summary>
public class WeComIMProvider : ExternalIMProviderBase
{
    // ---- 配置字段 ----
    private readonly string _corpId;
    private readonly string _appSecret;
    private readonly int _agentId;
    private readonly string _token;
    private readonly string _encodingAESKey;
    private readonly string _callbackPath;
    private readonly int _listenPort;

    // ---- 加解密工具 ----
    private readonly WXBizMsgCrypt _msgCrypt;

    // ---- API 客户端 ----
    private readonly WeComApiClient _apiClient;

    // ---- 渠道映射：企业微信 UserID <-> 内部 channelId（确定性 Guid）----
    private readonly ConcurrentDictionary<string, Guid> _userIdToChannel = new();
    private readonly ConcurrentDictionary<Guid, string> _channelToUserId = new();

    // ---- 最近活跃用户（用于发送权限请求卡片）----
    private string? _lastActiveUserId;
    private readonly object _lastActiveLock = new();

    // ---- 权限卡片状态（用于响应/超时后更新卡片）----
    private string? _permissionCardResponseCode;
    private string? _permissionCardUserId;
    private readonly object _permissionCardLock = new();

    // ---- 事件去重 ----
    private readonly ConcurrentDictionary<string, DateTime> _processedMsgKeys = new();

    // ---- HttpListener ----
    private HttpListener? _listener;
    private CancellationTokenSource? _listenerCts;

    /// <summary>
    /// 创建企业微信 IM Provider 实例。
    /// </summary>
    /// <param name="config">配置字典，包含 corpId、appSecret、agentId、token、encodingAESKey、callbackPath、listenPort</param>
    public WeComIMProvider(Dictionary<string, object> config)
    {
        _corpId = GetConfigString(config, "corpId");
        _appSecret = GetConfigString(config, "appSecret");
        _agentId = GetConfigInt(config, "agentId", 0);
        _token = GetConfigString(config, "token");
        _encodingAESKey = GetConfigString(config, "encodingAESKey");
        _callbackPath = GetConfigString(config, "callbackPath", "/wecom/callback");
        _listenPort = GetConfigInt(config, "listenPort", 8080);

        if (string.IsNullOrEmpty(_corpId) || string.IsNullOrEmpty(_appSecret))
            throw new ArgumentException("企业微信配置缺少 corpId 或 appSecret");
        if (string.IsNullOrEmpty(_token) || string.IsNullOrEmpty(_encodingAESKey))
            throw new ArgumentException("企业微信配置缺少 token 或 encodingAESKey");
        if (_agentId <= 0)
            throw new ArgumentException("企业微信配置缺少有效的 agentId");

        _msgCrypt = new WXBizMsgCrypt(_token, _encodingAESKey, _corpId);
        _apiClient = new WeComApiClient(_corpId, _appSecret);

        _logger.Info(null, "WeComIMProvider 初始化完成: callbackPath={0}, listenPort={1}, agentId={2}",
            _callbackPath, _listenPort, _agentId);
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

        _logger.Info(null, "WeComIMProvider 已启动，监听端口 {0}", _listenPort);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public override async Task StopAsync()
    {
        _listenerCts?.Cancel();

        try
        {
            if (_listener?.IsListening == true)
                _listener.Stop();
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "停止 WeCom HttpListener 时出错: {0}", ex.Message);
        }

        _listenerCts?.Dispose();
        _listenerCts = null;
        _listener = null;

        _apiClient.Dispose();
        _logger.Info(null, "WeComIMProvider 已停止");
        await Task.CompletedTask;
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
            _logger.Warn(null, "无法发送企业微信消息：未找到目标 userid（channelId={0}）", channelId);
            return string.Empty;
        }

        return await _apiClient.SendTextMessageAsync(userId, _agentId, content);
    }

    /// <inheritdoc/>
    /// <remarks>企业微信文本消息不支持更新，流式更新将降级为首片发送后静默忽略后续更新。</remarks>
    protected override Task<bool> UpdateMessageCoreAsync(string messageId, string content)
        => Task.FromResult(false);

    /// <summary>
    /// 解析目标 userid：channelId 为 Guid.Empty 时取最近活跃用户，否则查渠道映射。
    /// </summary>
    private string? ResolveTargetUserId(Guid channelId)
    {
        if (channelId == Guid.Empty)
        {
            lock (_lastActiveLock)
            {
                return _lastActiveUserId;
            }
        }

        if (_channelToUserId.TryGetValue(channelId, out string? uid) && !string.IsNullOrEmpty(uid))
            return uid;

        return null;
    }

    // ================================================================
    // 权限请求（使用模板卡片）
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
            _logger.Warn(null, "无活跃企业微信用户，无法发送权限请求卡片");
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

        string cardJson = BuildPermissionCard(permissionType.ToString(), resource, allowCode, denyCode);

        string responseCode;
        string msgId;
        try
        {
            (msgId, responseCode) = await _apiClient.SendTemplateCardAsync(targetUserId, _agentId, cardJson);
            lock (_permissionCardLock)
            {
                _permissionCardResponseCode = responseCode;
                _permissionCardUserId = targetUserId;
            }
            _logger.Info(userId, "企业微信权限请求卡片已发送: {0} -> msgid={1}, responseCode={2}",
                request.RequestId, msgId, responseCode);
        }
        catch (Exception ex)
        {
            _logger.Error(userId, "发送企业微信权限请求卡片失败: {0}", ex.Message);
            return new AskPermissionResult { Allowed = false };
        }

        // 超时后更新卡片为超时态
        string capturedResponseCode = responseCode;
        string capturedUserId = targetUserId;
        request.TimeoutCts.Token.Register(() =>
        {
            _permissionQueue.HandleTimeout(request);
            if (!string.IsNullOrEmpty(capturedResponseCode))
            {
                string timeoutCard = BuildTimeoutCard(permissionType.ToString(), resource);
                _ = _apiClient.UpdateTemplateCardAsync(capturedUserId, _agentId, capturedResponseCode, timeoutCard)
                    .ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                            _logger.Error(null, "更新企业微信权限卡片(超时)失败: {0}",
                                t.Exception?.GetBaseException().Message);
                    });
            }
        });

        return await _permissionQueue.EnqueueAsync(request);
    }

    // ================================================================
    // 模板卡片构建
    // ================================================================

    /// <summary>
    /// 构建权限请求模板卡片。
    /// 按钮使用 type=2（事件回调），key 编码动作与权限码（allow:{code} / deny:{code}）。
    /// </summary>
    private static string BuildPermissionCard(string permissionType, string resource, string allowCode, string denyCode)
    {
        var card = new
        {
            card_type = "text_notice",
            main_title = new { title = "权限请求" },
            sub_title_text = $"类型: {permissionType}\n资源: {resource}\n\n请选择是否允许此操作（1 分钟内有效）：",
            button_list = new[]
            {
                new { text = $"允许 ({allowCode})", type = 2, key = $"allow:{allowCode}" },
                new { text = $"拒绝 ({denyCode})", type = 2, key = $"deny:{denyCode}" }
            }
        };
        return JsonSerializer.Serialize(card);
    }

    /// <summary>
    /// 构建权限请求结果卡片（允许/拒绝后显示，按钮失效）。
    /// </summary>
    private static string BuildResultCard(bool allowed, string permissionType, string resource)
    {
        var card = new
        {
            card_type = "text_notice",
            main_title = new { title = allowed ? "权限请求 - 已允许" : "权限请求 - 已拒绝" },
            sub_title_text = $"类型: {permissionType}\n资源: {resource}\n\n本卡片按钮已失效。"
        };
        return JsonSerializer.Serialize(card);
    }

    /// <summary>
    /// 构建权限请求超时卡片。
    /// </summary>
    private static string BuildTimeoutCard(string permissionType, string resource)
    {
        var card = new
        {
            card_type = "text_notice",
            main_title = new { title = "权限请求 - 已超时" },
            sub_title_text = $"类型: {permissionType}\n资源: {resource}\n\n未在规定时间内响应，已自动拒绝。"
        };
        return JsonSerializer.Serialize(card);
    }

    // ================================================================
    // HttpListener 回调监听
    // ================================================================

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
            _logger.Info(null, "企业微信回调监听已启动: {0}", prefix);
        }
        catch (HttpListenerException ex)
        {
            _logger.Error(null, "无法启动企业微信 HttpListener 于 {0}: {1}", prefix, ex.Message);
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

                _ = HandleCallbackAsync(context);
            }
        }
        catch (Exception ex)
        {
            if (!ct.IsCancellationRequested)
                _logger.Error(null, "企业微信回调监听异常: {0}", ex.Message);
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
    /// 处理单个企业微信回调请求。
    /// GET：URL 验证（解密 echostr 返回明文）。
    /// POST：消息/事件回调（解密 XML 后处理）。
    /// </summary>
    private async Task HandleCallbackAsync(HttpListenerContext context)
    {
        try
        {
            var req = context.Request;
            var query = req.QueryString;
            string? msgSignature = query["msg_signature"];
            string? timestamp = query["timestamp"];
            string? nonce = query["nonce"];

            // GET: URL 验证
            if (string.Equals(req.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                string? echoStr = query["echostr"];
                if (string.IsNullOrEmpty(msgSignature) || string.IsNullOrEmpty(timestamp) ||
                    string.IsNullOrEmpty(nonce) || string.IsNullOrEmpty(echoStr))
                {
                    _logger.Warn(null, "企业微信 URL 验证缺少参数");
                    context.Response.StatusCode = 400;
                    await WriteResponseAsync(context.Response, "bad request");
                    return;
                }

                string echo = string.Empty;
                int ret = _msgCrypt.VerifyURL(msgSignature!, timestamp!, nonce!, echoStr!, ref echo);
                if (ret == WXBizMsgCrypt.OK)
                {
                    _logger.Info(null, "企业微信 URL 验证成功");
                    await WriteResponseAsync(context.Response, echo);
                }
                else
                {
                    _logger.Warn(null, "企业微信 URL 验证失败: ret={0}", ret);
                    context.Response.StatusCode = 403;
                    await WriteResponseAsync(context.Response, "verify failed");
                }
                return;
            }

            // POST: 消息/事件回调
            if (!string.Equals(req.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = 405;
                await WriteResponseAsync(context.Response, "method not allowed");
                return;
            }

            string body;
            using (var reader = new StreamReader(req.InputStream, Encoding.UTF8))
            {
                body = await reader.ReadToEndAsync();
            }

            if (string.IsNullOrEmpty(msgSignature) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(nonce))
            {
                _logger.Warn(null, "企业微信 POST 回调缺少签名参数");
                context.Response.StatusCode = 400;
                await WriteResponseAsync(context.Response, "bad request");
                return;
            }

            string plainXml = string.Empty;
            int code = _msgCrypt.DecryptMsg(msgSignature!, timestamp!, nonce!, body, ref plainXml);
            if (code != WXBizMsgCrypt.OK)
            {
                _logger.Warn(null, "企业微信回调消息解密失败: ret={0}", code);
                context.Response.StatusCode = 403;
                await WriteResponseAsync(context.Response, "decrypt failed");
                return;
            }

            // 立即返回 success，避免企业微信超时重试
            await WriteResponseAsync(context.Response, "success");

            // 后台处理
            string xmlCopy = plainXml;
            _ = Task.Run(() => ProcessCallbackXml(xmlCopy));
        }
        catch (Exception ex)
        {
            _logger.Error(null, "处理企业微信回调异常: {0}", ex.Message);
            try
            {
                context.Response.StatusCode = 500;
                await WriteResponseAsync(context.Response, "error");
            }
            catch { /* 忽略响应写入错误 */ }
        }
    }

    // ================================================================
    // 回调消息处理
    // ================================================================

    private void ProcessCallbackXml(string xml)
    {
        try
        {
            var msg = ParseWeComMessage(xml);
            if (msg == null)
            {
                _logger.Warn(null, "企业微信回调 XML 解析失败");
                return;
            }

            // 事件去重
            string dedupKey = BuildDedupKey(msg);
            if (!string.IsNullOrEmpty(dedupKey) && !TryMarkEventProcessed(dedupKey))
            {
                _logger.Debug(null, "跳过重复的企业微信消息: {0}", dedupKey);
                return;
            }

            // 记录最近活跃用户
            if (!string.IsNullOrEmpty(msg.FromUserName))
            {
                lock (_lastActiveLock)
                {
                    _lastActiveUserId = msg.FromUserName;
                }
            }

            if (string.Equals(msg.MsgType, "text", StringComparison.OrdinalIgnoreCase))
            {
                ProcessTextMessage(msg);
            }
            else if (string.Equals(msg.MsgType, "event", StringComparison.OrdinalIgnoreCase) &&
                     string.Equals(msg.Event, "template_card_event", StringComparison.OrdinalIgnoreCase))
            {
                ProcessCardCallback(msg);
            }
            else
            {
                _logger.Debug(null, "未处理的企业微信消息类型: MsgType={0}, Event={1}", msg.MsgType, msg.Event);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(null, "处理企业微信回调 XML 异常: {0}", ex.Message);
        }
    }

    /// <summary>
    /// 处理文本消息。若存在活跃权限请求且文本匹配权限码，则作为权限响应处理。
    /// </summary>
    private void ProcessTextMessage(WeComMessage msg)
    {
        try
        {
            if (string.IsNullOrEmpty(msg.FromUserName))
            {
                _logger.Warn(null, "企业微信文本消息缺少 FromUserName");
                return;
            }

            string text = msg.Content ?? string.Empty;
            if (string.IsNullOrWhiteSpace(text))
            {
                _logger.Debug(null, "企业微信消息文本为空，已跳过");
                return;
            }

            // 文本权限码回退匹配（卡片不可用时的降级路径）
            var activeRequest = _permissionQueue.GetActiveRequest();
            if (activeRequest != null &&
                TryMatchPermissionResponse(text, activeRequest.AllowCode, activeRequest.DenyCode, out bool allowed))
            {
                _logger.Info(activeRequest.UserId, "企业微信权限请求通过文本响应: {0} - {1}",
                    activeRequest.PermissionType, allowed ? "允许" : "拒绝");
                _permissionQueue.HandleResponse(activeRequest.UserId, allowed, addToCache: false, cacheDuration: null);
                UpdatePermissionCardAfterResponse(allowed, activeRequest);
                return;
            }

            // 映射 UserID 到内部 channelId
            Guid channelId = GetOrCreateChannelId(msg.FromUserName);
            Guid senderId = channelId;

            var chatMessage = new ChatMessage
            {
                Id = Guid.NewGuid(),
                SenderId = senderId,
                ChannelId = channelId,
                Content = text,
                Timestamp = DateTime.Now,
                Type = MessageType.Text
            };

            _logger.Info(senderId, "收到企业微信消息: channelId={0}, text={1}",
                channelId, text.Length > 50 ? text[..50] + "..." : text);

            OnMessageReceived(new IMMessageEventArgs(chatMessage));
        }
        catch (Exception ex)
        {
            _logger.Error(null, "处理企业微信文本消息异常: {0}", ex.Message);
        }
    }

    /// <summary>
    /// 处理模板卡片按钮回调 (template_card_event)。
    /// EventKey 格式：allow:{code} / deny:{code}。
    /// </summary>
    private void ProcessCardCallback(WeComMessage msg)
    {
        try
        {
            string? eventKey = msg.EventKey;
            if (string.IsNullOrEmpty(eventKey))
            {
                _logger.Debug(null, "企业微信卡片回调缺少 EventKey");
                return;
            }

            int sep = eventKey.IndexOf(':');
            if (sep <= 0 || sep >= eventKey.Length - 1)
            {
                _logger.Debug(null, "企业微信卡片回调 EventKey 格式无法解析: {0}", eventKey);
                return;
            }

            string action = eventKey[..sep];
            string code = eventKey[(sep + 1)..];

            var activeRequest = _permissionQueue.GetActiveRequest();
            if (activeRequest == null)
            {
                _logger.Warn(null, "收到企业微信卡片回调但无活跃权限请求");
                return;
            }

            bool allowed;
            string expectedCode;
            if (string.Equals(action, "allow", StringComparison.OrdinalIgnoreCase))
            {
                allowed = true;
                expectedCode = activeRequest.AllowCode;
            }
            else if (string.Equals(action, "deny", StringComparison.OrdinalIgnoreCase))
            {
                allowed = false;
                expectedCode = activeRequest.DenyCode;
            }
            else
            {
                _logger.Debug(null, "企业微信卡片回调 action 未识别: {0}", action);
                return;
            }

            if (code != expectedCode)
            {
                _logger.Warn(null, "企业微信卡片回调 code 不匹配: 期望={0}, 实际={1}", expectedCode, code);
                return;
            }

            _logger.Info(activeRequest.UserId, "企业微信权限请求通过卡片响应: {0} - {1}",
                activeRequest.PermissionType, allowed ? "允许" : "拒绝");

            _permissionQueue.HandleResponse(activeRequest.UserId, allowed, addToCache: false, cacheDuration: null);
            UpdatePermissionCardAfterResponse(allowed, activeRequest);
        }
        catch (Exception ex)
        {
            _logger.Error(null, "处理企业微信卡片回调异常: {0}", ex.Message);
        }
    }

    /// <summary>
    /// 权限请求响应后，将模板卡片更新为结果态。
    /// </summary>
    private void UpdatePermissionCardAfterResponse(bool allowed, PendingPermissionRequest request)
    {
        string? responseCode;
        string? userId;
        lock (_permissionCardLock)
        {
            responseCode = _permissionCardResponseCode;
            userId = _permissionCardUserId;
        }

        if (string.IsNullOrEmpty(responseCode) || string.IsNullOrEmpty(userId))
            return;

        string resultCard = BuildResultCard(allowed, request.PermissionType.ToString(), request.Resource);
        _ = _apiClient.UpdateTemplateCardAsync(userId, _agentId, responseCode, resultCard).ContinueWith(t =>
        {
            if (t.IsFaulted)
                _logger.Error(null, "更新企业微信权限卡片(结果)失败: {0}", t.Exception?.GetBaseException().Message);
        });
    }

    // ================================================================
    // 渠道映射
    // ================================================================

    /// <summary>
    /// 获取或创建企业微信 UserID 到内部 channelId 的映射。
    /// 使用基于 UserID 哈希的确定性 Guid，保证同一用户始终映射到同一 channelId。
    /// </summary>
    private Guid GetOrCreateChannelId(string userId)
    {
        if (_userIdToChannel.TryGetValue(userId, out Guid existingId))
            return existingId;

        Guid newId = DeterministicGuid("wecom:" + userId);
        if (_userIdToChannel.TryAdd(userId, newId))
        {
            _channelToUserId[newId] = userId;
            _logger.Info(null, "创建企业微信渠道映射: userId={0} -> channelId={1}", userId, newId);
        }
        else
        {
            // 并发情况下，其他线程可能已添加
            newId = _userIdToChannel[userId];
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
    // XML 解析与去重
    // ================================================================

    /// <summary>
    /// 解析企业微信回调 XML 为 <see cref="WeComMessage"/>。
    /// </summary>
    private static WeComMessage? ParseWeComMessage(string xml)
    {
        try
        {
            var doc = XDocument.Parse(xml);
            XElement? root = doc.Root;
            if (root == null)
                return null;

            return new WeComMessage
            {
                ToUserName = root.Element("ToUserName")?.Value,
                FromUserName = root.Element("FromUserName")?.Value,
                CreateTime = long.TryParse(root.Element("CreateTime")?.Value, out long ct) ? ct : 0,
                MsgType = root.Element("MsgType")?.Value,
                Content = root.Element("Content")?.Value,
                MsgId = root.Element("MsgId")?.Value,
                AgentID = int.TryParse(root.Element("AgentID")?.Value, out int ai) ? ai : null,
                Event = root.Element("Event")?.Value,
                TaskId = root.Element("TaskId")?.Value,
                EventKey = root.Element("EventKey")?.Value
            };
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 构造去重键：文本消息使用 MsgId；事件使用 FromUserName+CreateTime+Event+EventKey 复合键。
    /// </summary>
    private static string BuildDedupKey(WeComMessage msg)
    {
        if (!string.IsNullOrEmpty(msg.MsgId))
            return "msgid:" + msg.MsgId;
        return $"evt:{msg.FromUserName}_{msg.CreateTime}_{msg.Event}_{msg.EventKey}";
    }

    // ================================================================
    // 辅助方法
    // ================================================================

    private static async Task WriteResponseAsync(HttpListenerResponse response, string content)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(content);
        response.ContentType = "text/plain; charset=utf-8";
        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer);
        response.OutputStream.Close();
    }

    /// <summary>
    /// 标记事件已处理，用于去重。返回 true 表示首次处理，false 表示重复事件。
    /// </summary>
    private bool TryMarkEventProcessed(string key)
    {
        if (!_processedMsgKeys.TryAdd(key, DateTime.UtcNow))
            return false;

        // 定期清理过期记录（保留 10 分钟内）
        if (_processedMsgKeys.Count > 500)
        {
            var cutoff = DateTime.UtcNow.AddMinutes(-10);
            foreach (var kvp in _processedMsgKeys)
            {
                if (kvp.Value < cutoff)
                {
                    ((ICollection<KeyValuePair<string, DateTime>>)_processedMsgKeys).Remove(kvp);
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
    // 内嵌类：企业微信 API 客户端
    // ================================================================

    /// <summary>
    /// 封装企业微信服务端 API 调用，包含 access_token 管理。
    /// </summary>
    internal class WeComApiClient : IDisposable
    {
        private static readonly ILogger _logger = LogManager.Instance.GetLogger<WeComApiClient>();
        private readonly HttpClient _httpClient;
        private readonly WeComAccessToken _tokenManager;

        private const string BaseUrl = "https://qyapi.weixin.qq.com/cgi-bin";

        public WeComApiClient(string corpId, string appSecret)
        {
            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
            _tokenManager = new WeComAccessToken(corpId, appSecret, _httpClient);
        }

        /// <summary>
        /// 发送文本消息，返回 msgid。
        /// </summary>
        public async Task<string> SendTextMessageAsync(string userId, int agentId, string content)
        {
            string token = await _tokenManager.GetTokenAsync();

            var body = new
            {
                touser = userId,
                msgtype = "text",
                agentid = agentId,
                text = new { content }
            };
            string bodyJson = JsonSerializer.Serialize(body);

            string resp = await PostAsync($"{BaseUrl}/message/send?access_token={token}", bodyJson);
            return ParseMsgId(resp);
        }

        /// <summary>
        /// 发送模板卡片消息，返回 (msgid, response_code)。
        /// response_code 用于后续 update_template_card 调用。
        /// </summary>
        public async Task<(string MessageId, string ResponseCode)> SendTemplateCardAsync(
            string userId, int agentId, string cardJson)
        {
            string token = await _tokenManager.GetTokenAsync();

            string bodyJson;
            using (var cardDoc = JsonDocument.Parse(cardJson))
            {
                var body = new
                {
                    touser = userId,
                    msgtype = "template_card",
                    agentid = agentId,
                    template_card = cardDoc.RootElement
                };
                bodyJson = JsonSerializer.Serialize(body);
            }

            string resp = await PostAsync($"{BaseUrl}/message/send?access_token={token}", bodyJson);
            return ParseCardResponse(resp);
        }

        /// <summary>
        /// 更新模板卡片（用于响应/超时后更新卡片状态），返回是否成功。
        /// </summary>
        public async Task<bool> UpdateTemplateCardAsync(
            string userId, int agentId, string responseCode, string cardJson)
        {
            string token = await _tokenManager.GetTokenAsync();

            string bodyJson;
            using (var cardDoc = JsonDocument.Parse(cardJson))
            {
                var body = new
                {
                    userids = new[] { userId },
                    agentid = agentId,
                    response_code = responseCode,
                    template_card = cardDoc.RootElement
                };
                bodyJson = JsonSerializer.Serialize(body);
            }

            string resp = await PostAsync($"{BaseUrl}/message/update_template_card?access_token={token}", bodyJson);

            try
            {
                using var doc = JsonDocument.Parse(resp);
                if (doc.RootElement.TryGetProperty("errcode", out var ec) && ec.GetInt32() == 0)
                    return true;
                _logger.Error(null, "企业微信更新模板卡片失败: {0}", resp);
            }
            catch (JsonException)
            {
                _logger.Error(null, "企业微信 API 返回非 JSON 响应: {0}", resp);
            }
            return false;
        }

        private async Task<string> PostAsync(string url, string bodyJson)
        {
            var content = new StringContent(bodyJson, Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(url, content);
            return await response.Content.ReadAsStringAsync();
        }

        private static string ParseMsgId(string responseBody)
        {
            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                if (doc.RootElement.TryGetProperty("errcode", out var ec) && ec.GetInt32() == 0)
                {
                    if (doc.RootElement.TryGetProperty("msgid", out var mid))
                        return mid.GetString() ?? string.Empty;
                    return string.Empty;
                }
                _logger.Error(null, "企业微信发送消息失败: {0}", responseBody);
            }
            catch (JsonException)
            {
                _logger.Error(null, "企业微信 API 返回非 JSON 响应: {0}", responseBody);
            }
            return string.Empty;
        }

        private static (string MessageId, string ResponseCode) ParseCardResponse(string responseBody)
        {
            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                if (doc.RootElement.TryGetProperty("errcode", out var ec) && ec.GetInt32() == 0)
                {
                    string msgId = doc.RootElement.TryGetProperty("msgid", out var mid)
                        ? (mid.GetString() ?? string.Empty)
                        : string.Empty;
                    string responseCode = doc.RootElement.TryGetProperty("response_code", out var rc)
                        ? (rc.GetString() ?? string.Empty)
                        : string.Empty;
                    return (msgId, responseCode);
                }
                _logger.Error(null, "企业微信发送模板卡片失败: {0}", responseBody);
            }
            catch (JsonException)
            {
                _logger.Error(null, "企业微信 API 返回非 JSON 响应: {0}", responseBody);
            }
            return (string.Empty, string.Empty);
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
    /// 企业微信 access_token 管理，带缓存与自动刷新（每 7000 秒刷新，留 200 秒缓冲）。
    /// </summary>
    internal class WeComAccessToken
    {
        private static readonly ILogger _logger = LogManager.Instance.GetLogger<WeComAccessToken>();
        private readonly string _corpId;
        private readonly string _appSecret;
        private readonly HttpClient _httpClient;
        private readonly SemaphoreSlim _lock = new(1, 1);

        private string? _token;
        private DateTime _expiresAt;

        private const string TokenUrl = "https://qyapi.weixin.qq.com/cgi-bin/gettoken";

        public WeComAccessToken(string corpId, string appSecret, HttpClient httpClient)
        {
            _corpId = corpId;
            _appSecret = appSecret;
            _httpClient = httpClient;
        }

        /// <summary>
        /// 获取有效的 access_token，过期时自动刷新。
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
        /// 刷新 access_token。
        /// GET https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={corpId}&corpsecret={appSecret}
        /// </summary>
        private async Task RefreshTokenAsync()
        {
            string url = $"{TokenUrl}?corpid={Uri.EscapeDataString(_corpId)}&corpsecret={Uri.EscapeDataString(_appSecret)}";

            using var response = await _httpClient.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                if (doc.RootElement.TryGetProperty("errcode", out var ec) && ec.GetInt32() == 0)
                {
                    _token = doc.RootElement.GetProperty("access_token").GetString();
                    int expiresIn = doc.RootElement.TryGetProperty("expires_in", out var ee)
                        ? ee.GetInt32()
                        : 7200;
                    // 每 7000 秒刷新（留缓冲，避免边界过期）
                    _expiresAt = DateTime.UtcNow.AddSeconds(Math.Min(expiresIn, 7000));
                    _logger.Debug(null, "企业微信 access_token 已刷新，有效期 {0} 秒", expiresIn);
                }
                else
                {
                    string errmsg = doc.RootElement.TryGetProperty("errmsg", out var em)
                        ? (em.GetString() ?? "未知错误")
                        : "未知错误";
                    throw new InvalidOperationException($"获取企业微信 access_token 失败: {errmsg}");
                }
            }
            catch (JsonException)
            {
                throw new InvalidOperationException($"企业微信 access_token 响应解析失败: {responseBody}");
            }
        }
    }

    // ================================================================
    // 内嵌类：消息模型
    // ================================================================

    /// <summary>
    /// 解析后的企业微信回调消息模型。
    /// </summary>
    internal record WeComMessage
    {
        /// <summary>企业 corpId</summary>
        public string? ToUserName { get; init; }

        /// <summary>发送者 UserID</summary>
        public string? FromUserName { get; init; }

        public long CreateTime { get; init; }

        /// <summary>消息类型：text / event 等</summary>
        public string? MsgType { get; init; }

        /// <summary>文本消息内容</summary>
        public string? Content { get; init; }

        public string? MsgId { get; init; }

        public int? AgentID { get; init; }

        /// <summary>事件类型（如 template_card_event）</summary>
        public string? Event { get; init; }

        /// <summary>卡片任务 ID</summary>
        public string? TaskId { get; init; }

        /// <summary>卡片按钮事件键</summary>
        public string? EventKey { get; init; }
    }
}
