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

using Microsoft.Extensions.Logging;
using SiliconLife.Common.WebView;
using SiliconLife.Common.SiliconBeing;

namespace SiliconLife.McpServer.Tools;

/// <summary>
/// Browser pool interface for server
/// </summary>
public interface IBrowserPool
{
    Task<PlaywrightWebView> GetWebViewAsync();
    Task ClearAsync();
}

/// <summary>
/// Simple browser pool for server
/// Manages a shared WebView instance
/// </summary>
public class BrowserPool : IBrowserPool, IAsyncDisposable
{
    private readonly ILogger<BrowserPool> _logger;
    private PlaywrightWebView? _webView;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public BrowserPool(ILogger<BrowserPool> logger)
    {
        _logger = logger;
    }

    public async Task<PlaywrightWebView> GetWebViewAsync()
    {
        await _lock.WaitAsync();
        try
        {
            if (_webView == null)
            {
                _logger.LogInformation("Creating new WebView instance");
                var dummyBeing = new McpContextSiliconBeing();
                _webView = new PlaywrightWebView(dummyBeing);
            }
            return _webView;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task ClearAsync()
    {
        await _lock.WaitAsync();
        try
        {
            if (_webView != null)
            {
                await _webView.ClearSessionAsync();
                _webView = null;
                _logger.LogInformation("Browser session cleared");
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        await ClearAsync();
        _lock.Dispose();
    }
}

/// <summary>
/// Dummy silicon being for WebView context
/// </summary>
public class McpContextSiliconBeing : DefaultSiliconBeing
{
    public McpContextSiliconBeing() : base(
        id: Guid.Parse("00000000-0000-0000-0000-000000000001"),
        name: "MCP Browser Agent"
    )
    {
    }
}
