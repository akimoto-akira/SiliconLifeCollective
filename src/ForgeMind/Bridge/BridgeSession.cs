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
using System.Net.Sockets;
using System.Text.Json;
using SiliconLife.Collective;

namespace ForgeMind.Bridge;

/// <summary>
/// One authenticated companion connection. Owns the TCP socket, performs the
/// auth + handshake exchange, then runs the read loop: responses complete
/// pending requests, requests are routed to the host handler, events are
/// forwarded to the server.
/// </summary>
internal sealed class BridgeSession : IDisposable
{
    private readonly TcpClient _client;
    private readonly NetworkStream _stream;
    private readonly SemaphoreSlim _writeLock = new(1, 1);
    private readonly string _expectedToken;
    private readonly ILogger _logger = LogManager.Instance.GetLogger<BridgeSession>();
    private readonly ConcurrentDictionary<string, TaskCompletionSource<BridgeMessage>> _pending = new();
    private CancellationTokenSource? _cts;

    /// <summary>Absolute .uproject path reported by the companion.</summary>
    public string ProjectFile { get; private set; } = "";

    /// <summary>Engine version reported by the companion (e.g. "5.6.1").</summary>
    public string EngineVersion { get; private set; } = "";

    /// <summary>Editor process id reported by the companion.</summary>
    public int EditorPid { get; private set; }

    /// <summary>Command names the companion accepts (dynamic action registry).</summary>
    public string[] Commands { get; private set; } = [];

    public bool IsReady { get; private set; }

    public DateTime LastReceivedUtc { get; private set; } = DateTime.UtcNow;

    public BridgeSession(TcpClient client, string expectedToken)
    {
        _client = client;
        _stream = client.GetStream();
        _expectedToken = expectedToken;
    }

    /// <summary>
    /// Performs auth + handshake, then runs the read loop until the
    /// connection drops. Returns when the session is over.
    /// </summary>
    public async Task RunAsync(Func<BridgeSession, BridgeMessage, Task> onRequest,
        Action<BridgeSession, BridgeMessage> onEvent, CancellationToken hostStopping)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(hostStopping);
        CancellationToken ct = _cts.Token;

        try
        {
            // 1. Auth — the very first frame must carry the session token
            BridgeMessage? auth = await ReadWithTimeoutAsync(TimeSpan.FromSeconds(10), ct);
            if (auth == null || auth.Type != BridgeMessageType.Request || auth.Name != "auth" ||
                auth.Payload?.GetProperty("token").GetString() != _expectedToken)
            {
                _logger.Warn(null, "[Bridge] Rejecting connection: bad or missing auth");
                return;
            }

            // 2. Handshake — companion reports identity and supported commands
            BridgeMessage? handshake = await ReadWithTimeoutAsync(TimeSpan.FromSeconds(10), ct);
            if (handshake == null || handshake.Type != BridgeMessageType.Request || handshake.Name != "handshake")
            {
                _logger.Warn(null, "[Bridge] Rejecting connection: handshake expected");
                return;
            }

            JsonElement payload = handshake.Payload ?? default;
            ProjectFile = payload.TryGetProperty("projectFile", out JsonElement pf) ? pf.GetString() ?? "" : "";
            EngineVersion = payload.TryGetProperty("engineVersion", out JsonElement ev) ? ev.GetString() ?? "" : "";
            EditorPid = payload.TryGetProperty("pid", out JsonElement pid) && pid.TryGetInt32(out int pidValue) ? pidValue : 0;
            Commands = payload.TryGetProperty("commands", out JsonElement commands) && commands.ValueKind == JsonValueKind.Array
                ? commands.EnumerateArray().Where(e => e.ValueKind == JsonValueKind.String).Select(e => e.GetString()!).ToArray()
                : [];

            await SendAsync(BridgeMessage.NewResponse(handshake.Id ?? "", "handshake", new { ready = true }), ct);
            IsReady = true;
            _logger.Info(null, "[Bridge] Companion connected: {0} (engine {1}, pid {2}, commands: {3})",
                ProjectFile, EngineVersion, EditorPid, string.Join(",", Commands));

            // 3. Message loop
            while (!ct.IsCancellationRequested)
            {
                BridgeMessage? message = await FrameCodec.ReadFrameAsync(_stream, ct);
                if (message == null)
                    break;

                LastReceivedUtc = DateTime.UtcNow;
                switch (message.Type)
                {
                    case BridgeMessageType.Response when message.Id != null:
                        if (_pending.TryRemove(message.Id, out TaskCompletionSource<BridgeMessage>? tcs))
                            tcs.TrySetResult(message);
                        break;
                    case BridgeMessageType.Request:
                        await onRequest(this, message);
                        break;
                    case BridgeMessageType.Event:
                        onEvent(this, message);
                        break;
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Host shutting down — normal
        }
        catch (Exception ex)
        {
            _logger.Debug(null, "[Bridge] Session ended: {0}", ex.Message);
        }
        finally
        {
            IsReady = false;
            foreach (TaskCompletionSource<BridgeMessage> tcs in _pending.Values)
                tcs.TrySetCanceled();
            _logger.Info(null, "[Bridge] Companion disconnected: {0}", ProjectFile);
        }
    }

    /// <summary>Sends a request and awaits the matching response.</summary>
    public async Task<BridgeMessage> CallAsync(string name, JsonElement? payload, TimeSpan timeout)
    {
        if (!IsReady)
            throw new InvalidOperationException("Session is not ready");

        string id = Guid.NewGuid().ToString("N");
        var tcs = new TaskCompletionSource<BridgeMessage>(TaskCreationOptions.RunContinuationsAsynchronously);
        _pending[id] = tcs;

        try
        {
            BridgeMessage request = BridgeMessage.NewRequest(id, name);
            request.Payload = payload;
            await SendAsync(request, _cts?.Token ?? CancellationToken.None);

            using var timeoutCts = new CancellationTokenSource(timeout);
            Task completed = await Task.WhenAny(tcs.Task, Task.Delay(Timeout.Infinite, timeoutCts.Token));
            if (completed != tcs.Task)
                throw new TimeoutException($"Bridge request '{name}' timed out after {timeout.TotalSeconds:0}s");

            return await tcs.Task;
        }
        finally
        {
            _pending.TryRemove(id, out _);
        }
    }

    /// <summary>Sends a frame; writes are serialized to keep frames intact.</summary>
    public async Task SendAsync(BridgeMessage message, CancellationToken ct)
    {
        await _writeLock.WaitAsync(ct);
        try
        {
            await FrameCodec.WriteFrameAsync(_stream, message, ct);
        }
        finally
        {
            _writeLock.Release();
        }
    }

    /// <summary>Reads one frame or fails after the given timeout (handshake guard).</summary>
    private async Task<BridgeMessage?> ReadWithTimeoutAsync(TimeSpan timeout, CancellationToken ct)
    {
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        timeoutCts.CancelAfter(timeout);
        return await FrameCodec.ReadFrameAsync(_stream, timeoutCts.Token);
    }

    public void Dispose()
    {
        _cts?.Cancel();
        try { _client.Dispose(); } catch { /* already gone */ }
        _writeLock.Dispose();
    }
}
