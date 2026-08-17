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
using System.Text;
using System.Text.Json;

namespace SiliconLife.Collective;

/// <summary>
/// MCP connection over stdio: a long-lived subprocess whose stdin/stdout
/// exchange newline-framed JSON-RPC messages (UTF-8). stderr is logged but
/// not part of the protocol. The reader thread dispatches responses to
/// pending requests and forwards notifications (tools/list_changed).
/// </summary>
public sealed class StdioMcpClientConnection : McpClientConnection
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<StdioMcpClientConnection>();

    private readonly Dictionary<int, TaskCompletionSource<JsonElement>> _pending = new();
    private readonly object _pendingLock = new();
    private readonly object _writeLock = new();
    private Process? _process;
    private Thread? _readerThread;
    private volatile bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="StdioMcpClientConnection"/> class.
    /// The subprocess is started lazily by <see cref="Initialize"/>.
    /// </summary>
    /// <param name="serverConfig">The server configuration (command + args + env).</param>
    public StdioMcpClientConnection(McpServerConfig serverConfig) : base(serverConfig)
    {
    }

    /// <summary>Starts the subprocess.</summary>
    /// <returns>True when the process started successfully.</returns>
    private bool StartProcess()
    {
        if (_process != null)
        {
            return true;
        }

        if (string.IsNullOrWhiteSpace(ServerConfig.Command))
        {
            LastError = "stdio transport requires a command";
            return false;
        }

        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = ServerConfig.Command,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
            };

            // ArgumentList passes each argument verbatim (no quoting issues)
            foreach (string arg in ServerConfig.Args)
            {
                psi.ArgumentList.Add(arg);
            }

            foreach (var kvp in ServerConfig.Environment)
            {
                psi.Environment[kvp.Key] = kvp.Value;
            }

            _process = Process.Start(psi);
            if (_process == null)
            {
                LastError = $"Failed to start process: {ServerConfig.Command}";
                return false;
            }

            _process.EnableRaisingEvents = true;
            _process.Exited += OnProcessExited;

            // Drain stderr in the background so the subprocess never blocks
            // on a full stderr pipe; log lines for diagnostics.
            _ = Task.Run(async () =>
            {
                try
                {
                    Process? process = _process;
                    if (process == null)
                    {
                        return;
                    }
                    while (!process.HasExited)
                    {
                        string? errLine = await process.StandardError.ReadLineAsync();
                        if (errLine == null)
                        {
                            break;
                        }
                        if (errLine.Trim().Length > 0)
                        {
                            _logger.Debug(null, "[Mcp] stderr[{0}]: {1}", ServerConfig.Id, errLine);
                        }
                    }
                }
                catch
                {
                    // Process closed — ignore
                }
            });

            _readerThread = new Thread(ReadLoop)
            {
                IsBackground = true,
                Name = $"mcp-stdio-{ServerConfig.Id}",
            };
            _readerThread.Start();

            return true;
        }
        catch (Exception ex)
        {
            LastError = ex.Message;
            _logger.Warn(null, "[Mcp] failed to start '{0} {1}': {2}",
                ServerConfig.Command, string.Join(" ", ServerConfig.Args), ex.Message);
            return false;
        }
    }

    private void OnProcessExited(object? sender, EventArgs e)
    {
        IsConnected = false;
        LastError = $"subprocess exited (code {_process?.ExitCode.ToString() ?? "unknown"})";
        FailAllPending("MCP subprocess exited");
    }

    private void ReadLoop()
    {
        Process? process = _process;
        if (process == null)
        {
            return;
        }

        try
        {
            using var reader = new StreamReader(process.StandardOutput.BaseStream, Encoding.UTF8);
            string? line;
            while (!_disposed && !process.HasExited && (line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (!TryParseFrame(line, out int? id, out string? method, out JsonElement? result, out string? errorMessage))
                {
                    continue;
                }

                if (id != null)
                {
                    TaskCompletionSource<JsonElement>? tcs;
                    lock (_pendingLock)
                    {
                        _pending.TryGetValue(id.Value, out tcs);
                    }

                    if (tcs != null)
                    {
                        if (errorMessage != null)
                        {
                            tcs.TrySetException(new InvalidOperationException(errorMessage));
                        }
                        else if (result != null)
                        {
                            tcs.TrySetResult(result.Value);
                        }
                    }
                }
                else if (method == "notifications/tools/list_changed")
                {
                    _logger.Info(null, "[Mcp] tools/list_changed notification from '{0}'", ServerConfig.Id);
                    OnToolsChanged();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Debug(null, "[Mcp] reader stopped for '{0}': {1}", ServerConfig.Id, ex.Message);
        }

        FailAllPending("MCP reader loop ended");
    }

    private void FailAllPending(string reason)
    {
        List<TaskCompletionSource<JsonElement>> pending;
        lock (_pendingLock)
        {
            pending = _pending.Values.ToList();
            _pending.Clear();
        }
        foreach (var tcs in pending)
        {
            tcs.TrySetException(new InvalidOperationException(reason));
        }
    }

    /// <inheritdoc/>
    protected override JsonElement SendRequest(string method, object? payload, int timeoutSeconds)
    {
        if (_disposed)
        {
            throw new InvalidOperationException("Connection disposed");
        }

        if (!StartProcess())
        {
            throw new InvalidOperationException($"Failed to start MCP subprocess: {LastError}");
        }

        int id = NextRequestId();
        var tcs = new TaskCompletionSource<JsonElement>(TaskCreationOptions.RunContinuationsAsynchronously);
        lock (_pendingLock)
        {
            _pending[id] = tcs;
        }

        try
        {
            string json = BuildRequestJson(id, method, payload);
            lock (_writeLock)
            {
                Process? process = _process;
                if (process == null || process.HasExited)
                {
                    throw new InvalidOperationException("MCP subprocess is not running");
                }
                process.StandardInput.WriteLine(json);
                process.StandardInput.Flush();
            }

            if (!tcs.Task.Wait(TimeSpan.FromSeconds(timeoutSeconds)))
            {
                throw new TimeoutException($"no response for '{method}' within {timeoutSeconds}s");
            }

            return tcs.Task.Result;
        }
        finally
        {
            lock (_pendingLock)
            {
                _pending.Remove(id);
            }
        }
    }

    /// <inheritdoc/>
    protected override void SendNotification(string method)
    {
        if (_disposed)
        {
            return;
        }

        lock (_writeLock)
        {
            Process? process = _process;
            if (process == null || process.HasExited)
            {
                return;
            }
            process.StandardInput.WriteLine(BuildNotificationJson(method));
            process.StandardInput.Flush();
        }
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;

        FailAllPending("Connection disposed");

        try
        {
            Process? process = _process;
            if (process != null && !process.HasExited)
            {
                process.Kill(entireProcessTree: true);
            }
        }
        catch (Exception ex)
        {
            _logger.Debug(null, "[Mcp] kill failed for '{0}': {1}", ServerConfig.Id, ex.Message);
        }

        try
        {
            _process?.Dispose();
        }
        catch
        {
            // Ignore
        }

        _process = null;
        IsConnected = false;
    }
}
