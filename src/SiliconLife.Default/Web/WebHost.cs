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

using System.Net;
using SiliconLife.Collective;

namespace SiliconLife.Default.Web;

public class WebHost : IDisposable
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<WebHost>();
    private HttpListener? _listener;
    private readonly Router _router;
    private readonly int _port;
    private readonly bool _allowIntranet;
    private CancellationTokenSource? _cts;
    private Task? _listenerTask;
    private bool _isRunning;

    public bool IsRunning => _isRunning;

    public WebHost(int port, Router router, bool allowIntranet = false)
    {
        _port = port;
        _router = router;
        _allowIntranet = allowIntranet;
    }

    public static WebHostBuilder CreateBuilder() => new();

    public async Task StartAsync()
    {
        if (_isRunning)
            return;

        _logger.Info($"Web server starting on port {_port}");
        _listener = new HttpListener();
        string prefix = _allowIntranet ? $"http://+:{_port}/" : $"http://localhost:{_port}/";
        _listener.Prefixes.Add(prefix);
        
        try
        {
            _listener.Start();
        }
        catch (HttpListenerException ex)
        {
            _logger.Error($"Failed to start web server on port {_port}: {ex.Message}");
            Console.WriteLine("Try running as administrator or use netsh to reserve the port.");
            throw;
        }

        _cts = new CancellationTokenSource();
        _isRunning = true;

        _listenerTask = Task.Run(() => ListenAsync(_cts.Token));
        
        _logger.Info($"Web server started on port {_port}");
    }

    public async Task StopAsync()
    {
        if (!_isRunning)
            return;

        _logger.Info("Web server stopping...");
        _cts?.Cancel();
        
        if (_listenerTask != null)
        {
            try
            {
                await _listenerTask.WaitAsync(TimeSpan.FromSeconds(5));
            }
            catch (TimeoutException)
            {
            }
            catch (OperationCanceledException)
            {
            }
        }

        _listener?.Stop();
        _listener?.Close();
        _isRunning = false;

        _logger.Info("Web server stopped");
    }

    private async Task ListenAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested && _listener != null && _listener.IsListening)
        {
            try
            {
                var context = await _listener.GetContextAsync().WaitAsync(token);
                
                _logger.Trace($"Accepted request: {context.Request.HttpMethod} {context.Request.Url?.AbsolutePath}");
                _ = Task.Run(() => HandleRequestAsync(context), token);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (HttpListenerException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error accepting request: {ex.Message}");
            }
        }
    }

    private async Task HandleRequestAsync(HttpListenerContext context)
    {
        try
        {
            await _router.HandleRequest(context);
        }
        catch (Exception ex)
        {
            _logger.Error($"Error handling request: {ex.Message}");
            try
            {
                var response = context.Response;
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes("500 Internal Server Error");
                response.StatusCode = 500;
                response.ContentType = "text/plain";
                await response.OutputStream.WriteAsync(buffer);
                response.Close();
            }
            catch
            {
            }
        }
    }

    public void Dispose()
    {
        StopAsync().Wait();
        _cts?.Dispose();
        _listener?.Close();
    }
}

public class WebHostBuilder
{
    private int _port = 8080;
    private Router? _router;

    public WebHostBuilder UseRouter(Router router)
    {
        _router = router;
        return this;
    }

    public WebHostBuilder UsePort(int port)
    {
        _port = port;
        return this;
    }

    public WebHost Build()
    {
        if (_router == null)
            _router = new Router();
        
        return new WebHost(_port, _router);
    }
}
