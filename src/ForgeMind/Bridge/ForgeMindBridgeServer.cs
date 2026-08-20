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

using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text.Json;
using SiliconLife.Collective;

namespace ForgeMind.Bridge;

/// <summary>
/// TCP bridge server on the host side. Listens on a loopback-only,
/// dynamically assigned port and publishes <c>{port, token, pid}</c> to
/// <c>%USERPROFILE%\.forgemind\bridge.json</c> for companions to discover.
/// Single-host model: when a bridge file of a still-living host exists,
/// this server refuses to start.
/// </summary>
internal sealed class ForgeMindBridgeServer : TickObject
{
    /// <summary>Discovery file directory: {UserProfile}/.forgemind.</summary>
    private static readonly string DiscoveryDirectory =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".forgemind");

    /// <summary>Discovery file: bridge.json under <see cref="DiscoveryDirectory"/>.</summary>
    public static readonly string DiscoveryFile = Path.Combine(DiscoveryDirectory, "bridge.json");

    /// <summary>Silence window before a session is considered dead.</summary>
    private static readonly TimeSpan SessionTimeout = TimeSpan.FromSeconds(90);

    private readonly ILogger _logger = LogManager.Instance.GetLogger<ForgeMindBridgeServer>();
    private readonly List<BridgeSession> _sessions = [];
    private readonly object _sessionLock = new();
    private TcpListener? _listener;
    private CancellationTokenSource? _cts;
    private string _token = "";

    public int Port { get; private set; }

    public bool IsRunning { get; private set; }

    /// <summary>
    /// Snapshot of the current companion sessions (project file, engine
    /// version, advertised commands, ready state) for context reporting.
    /// </summary>
    internal IReadOnlyList<(string ProjectFile, string EngineVersion, string[] Commands, bool IsReady)> GetSessionSnapshot()
    {
        lock (_sessionLock)
        {
            return _sessions
                .Select(s => (s.ProjectFile, s.EngineVersion, s.Commands, s.IsReady))
                .ToList();
        }
    }

    /// <summary>Tick every 15 seconds for the silent-session sweep; MainLoop registration happens in the base ctor.</summary>
    public ForgeMindBridgeServer() : base(TimeSpan.FromSeconds(15))
    {
    }

    /// <summary>
    /// Starts the listener and publishes the discovery file.
    /// Returns false (with a warning log) when another host already owns the bridge.
    /// </summary>
    public bool Start()
    {
        if (IsRunning)
            return true;

        // Single-host guard: refuse when a live host still owns the discovery file
        if (File.Exists(DiscoveryFile))
        {
            try
            {
                using var existing = JsonDocument.Parse(File.ReadAllText(DiscoveryFile));
                if (existing.RootElement.TryGetProperty("pid", out JsonElement pidEl) &&
                    pidEl.TryGetInt32(out int existingPid))
                {
                    using var process = Process.GetProcessById(existingPid);
                    if (!process.HasExited)
                    {
                        _logger.Warn(null,
                            "[Bridge] Another host (pid {0}) already runs the bridge — skipping bridge startup", existingPid);
                        return false;
                    }
                }
            }
            catch (ArgumentException)
            {
                // Process id not found — stale file, safe to take over
            }
            catch (Exception ex)
            {
                _logger.Warn(null, "[Bridge] Could not inspect existing bridge.json: {0}", ex.Message);
            }
        }

        _token = Convert.ToHexString(RandomNumberGenerator.GetBytes(16)).ToLowerInvariant();

        // Dynamic port: bind loopback port 0 and read back the OS assignment
        _listener = new TcpListener(IPAddress.Loopback, 0);
        _listener.Start();
        Port = ((IPEndPoint)_listener.LocalEndpoint).Port;

        var info = new
        {
            port = Port,
            token = _token,
            pid = Environment.ProcessId,
            hostVersion = typeof(ForgeMindBridgeServer).Assembly.GetName().Version?.ToString() ?? ""
        };
        Directory.CreateDirectory(DiscoveryDirectory);
        File.WriteAllText(DiscoveryFile, JsonSerializer.Serialize(info, new JsonSerializerOptions { WriteIndented = true }));

        _cts = new CancellationTokenSource();
        _ = AcceptLoopAsync(_cts.Token);

        IsRunning = true;
        _logger.Info(null, "[Bridge] Bridge server listening on 127.0.0.1:{0} (discovery: {1})", Port, DiscoveryFile);
        return true;
    }

    /// <summary>Stops the listener, closes all sessions and removes the discovery file.</summary>
    public void Stop()
    {
        if (!IsRunning)
            return;

        _cts?.Cancel();
        try { _listener?.Stop(); } catch { /* already stopped */ }

        lock (_sessionLock)
        {
            foreach (BridgeSession session in _sessions)
                session.Dispose();
            _sessions.Clear();
        }

        try { if (File.Exists(DiscoveryFile)) File.Delete(DiscoveryFile); } catch { /* best effort */ }

        IsRunning = false;
        _logger.Info(null, "[Bridge] Bridge server stopped");
    }

    /// <summary>Ready sessions only.</summary>
    public BridgeSession[] GetSessions()
    {
        lock (_sessionLock)
            return _sessions.Where(s => s.IsReady).ToArray();
    }

    /// <summary>Locates the session for a project by absolute .uproject path (case-insensitive).</summary>
    public BridgeSession? GetSessionByProject(string projectFile)
    {
        string query = NormalizePath(projectFile);
        lock (_sessionLock)
            return _sessions.FirstOrDefault(s =>
                s.IsReady && string.Equals(NormalizePath(s.ProjectFile), query, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Normalizes a path for cross-platform comparison: the companion reports
    /// forward-slash paths (UE FPaths convention) while .NET produces
    /// backslashes, so both sides are canonicalized before matching.
    /// Falls back to separator-normalized text when the path is not resolvable.
    /// </summary>
    private static string NormalizePath(string path)
    {
        try
        {
            return Path.GetFullPath(path);
        }
        catch
        {
            return path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }
    }

    /// <summary>Accept loop — one session task per companion connection.</summary>
    private async Task AcceptLoopAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            TcpClient client;
            try
            {
                client = await _listener!.AcceptTcpClientAsync(ct);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.Warn(null, "[Bridge] Accept failed: {0}", ex.Message);
                break;
            }

            var session = new BridgeSession(client, _token);
            lock (_sessionLock)
                _sessions.Add(session);

            _ = RunSessionAsync(session, ct);
        }
    }

    private async Task RunSessionAsync(BridgeSession session, CancellationToken ct)
    {
        try
        {
            await session.RunAsync(HandleRequestAsync, HandleEvent, ct);
        }
        finally
        {
            lock (_sessionLock)
                _sessions.Remove(session);
            session.Dispose();
        }
    }

    /// <summary>Host-side request handler: companion-originated requests.</summary>
    private async Task HandleRequestAsync(BridgeSession session, BridgeMessage request)
    {
        try
        {
            switch (request.Name)
            {
                case "ping":
                    await session.SendAsync(BridgeMessage.NewResponse(request.Id ?? "", "ping", new { pong = true }), CancellationToken.None);
                    break;
                default:
                    await session.SendAsync(
                        BridgeMessage.NewErrorResponse(request.Id ?? "", request.Name, $"Unknown host command '{request.Name}'"),
                        CancellationToken.None);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "[Bridge] Handling '{0}' failed: {1}", request.Name, ex.Message);
        }
    }

    /// <summary>Host-side event handler: companion-originated events.</summary>
    private void HandleEvent(BridgeSession session, BridgeMessage message)
    {
        _logger.Info(null, "[Bridge] Event '{0}' from {1}: {2}",
            message.Name, session.ProjectFile, message.Payload?.GetRawText() ?? "{}");
    }

    /// <summary>MainLoop heartbeat (15s interval): drop silent sessions.</summary>
    protected override void OnTick(TimeSpan deltaTime)
    {
        if (!IsRunning)
            return;

        BridgeSession[] silent;
        lock (_sessionLock)
            silent = _sessions.Where(s => DateTime.UtcNow - s.LastReceivedUtc > SessionTimeout).ToArray();

        foreach (BridgeSession session in silent)
        {
            _logger.Warn(null, "[Bridge] Dropping silent session: {0}", session.ProjectFile);
            lock (_sessionLock)
                _sessions.Remove(session);
            session.Dispose();
        }
    }
}
