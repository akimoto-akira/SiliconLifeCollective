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

using Microsoft.Playwright;
using SiliconLife.Collective;
using System.Text.Json;

namespace SiliconLife.Default;

/// <summary>
/// Playwright跨平台WebView统一实现 / Playwright cross-platform WebView unified implementation
/// 所有平台使用同一套代码 / All platforms use the same code
/// </summary>
public class PlaywrightWebView : IWebViewCore
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<PlaywrightWebView>();
    
    private IBrowser? _browser;
    private IPage? _page;
    private readonly DefaultSiliconBeing _being;
    private int _timeoutSeconds = 30;
    private DateTime _lastOperationTime;
    
    public PlaywrightWebView(DefaultSiliconBeing being)
    {
        _being = being ?? throw new ArgumentNullException(nameof(being));
        _lastOperationTime = DateTime.UtcNow;
    }
    
    public bool IsOpen => _page != null;
    public string? CurrentUrl => _page?.Url;
    public string? PageTitle => _page != null ? _page.TitleAsync().Result : null;
    public bool IsLoading => false; // Playwright doesn't have a direct IsLoading property
    
    public async Task NavigateAsync(string url, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        _logger.Debug(_being.Id, "WebView: Navigating to {0}", url);
        
        await _page!.GotoAsync(url, new() 
        { 
            Timeout = _timeoutSeconds * 1000,
            WaitUntil = WaitUntilState.NetworkIdle 
        });
        
        _lastOperationTime = DateTime.UtcNow;
    }
    
    public async Task GoBackAsync(CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        await _page!.GoBackAsync();
        _lastOperationTime = DateTime.UtcNow;
    }
    
    public async Task GoForwardAsync(CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        await _page!.GoForwardAsync();
        _lastOperationTime = DateTime.UtcNow;
    }
    
    public async Task RefreshAsync(CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        await _page!.ReloadAsync();
        _lastOperationTime = DateTime.UtcNow;
    }
    
    public async Task ClickAsync(string selector, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        _logger.Debug(_being.Id, "WebView: Clicking {0}", selector);
        
        await _page!.ClickAsync(selector, new() { Timeout = _timeoutSeconds * 1000 });
        _lastOperationTime = DateTime.UtcNow;
    }
    
    public async Task InputAsync(string selector, string text, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        _logger.Debug(_being.Id, "WebView: Inputting text into {0}", selector);
        
        await _page!.FillAsync(selector, text, new() { Timeout = _timeoutSeconds * 1000 });
        _lastOperationTime = DateTime.UtcNow;
    }
    
    public async Task ScrollAsync(int x, int y, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        await _page!.EvaluateAsync($"window.scrollTo({x}, {y})");
        _lastOperationTime = DateTime.UtcNow;
    }
    
    public async Task HoverAsync(string selector, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        await _page!.HoverAsync(selector, new() { Timeout = _timeoutSeconds * 1000 });
        _lastOperationTime = DateTime.UtcNow;
    }
    
    public async Task<string?> ExecuteScriptAsync(string script, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        _logger.Debug(_being.Id, "WebView: Executing script");
        
        var result = await _page!.EvaluateAsync<string>(script);
        _lastOperationTime = DateTime.UtcNow;
        return result;
    }
    
    public async Task<string?> ExecuteFunctionAsync(string functionName, object[] args, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        _logger.Debug(_being.Id, "WebView: Calling function {0}", functionName);
        
        var result = await _page!.EvaluateAsync<string>($"{functionName}({string.Join(",", args.Select(a => JsonSerializer.Serialize(a)))})");
        _lastOperationTime = DateTime.UtcNow;
        return result;
    }
    
    public async Task<string> GetPageTextAsync(CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        var text = await _page!.InnerTextAsync("body");
        _lastOperationTime = DateTime.UtcNow;
        return text;
    }
    
    public async Task<string> GetHtmlAsync(CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        var html = await _page!.ContentAsync();
        _lastOperationTime = DateTime.UtcNow;
        return html;
    }
    
    public async Task<byte[]> GetScreenshotAsync(ScreenshotOptions? options = null, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        _logger.Debug(_being.Id, "WebView: Taking screenshot");
        
        var pageScreenshotOptions = new PageScreenshotOptions
        {
            Type = options?.Format == ScreenshotFormat.Jpeg 
                ? ScreenshotType.Jpeg 
                : ScreenshotType.Png,
            FullPage = options?.FullPage ?? false
        };
        
        if (options?.X.HasValue == true && options?.Y.HasValue == true &&
            options?.Width.HasValue == true && options?.Height.HasValue == true)
        {
            pageScreenshotOptions.Clip = new()
            {
                X = options.X.Value,
                Y = options.Y.Value,
                Width = options.Width.Value,
                Height = options.Height.Value
            };
        }
        
        var screenshot = await _page!.ScreenshotAsync(pageScreenshotOptions);
        _lastOperationTime = DateTime.UtcNow;
        return screenshot;
    }
    
    public async Task<ElementInfo> GetElementInfoAsync(string selector, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        
        var element = _page!.Locator(selector);
        var tagName = await element.EvaluateAsync<string>("el => el.tagName");
        var text = await element.InnerTextAsync();
        var isVisible = await element.IsVisibleAsync();
        var isEnabled = await element.IsEnabledAsync();
        
        var attributes = await element.EvaluateAsync<Dictionary<string, string>>(@"el => {
            const attrs = {};
            for (const attr of el.attributes) {
                attrs[attr.name] = attr.value;
            }
            return attrs;
        }");
        
        _lastOperationTime = DateTime.UtcNow;
        
        return new ElementInfo
        {
            TagName = tagName,
            Text = text,
            Attributes = attributes,
            IsVisible = isVisible,
            IsEnabled = isEnabled
        };
    }
    
    public async Task<string> GetElementTextAsync(string selector, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        var text = await _page!.Locator(selector).InnerTextAsync();
        _lastOperationTime = DateTime.UtcNow;
        return text;
    }
    
    public async Task WaitForElementAsync(string selector, int timeoutMs = 30000, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        _logger.Debug(_being.Id, "WebView: Waiting for element {0}", selector);
        
        await _page!.WaitForSelectorAsync(selector, new() { Timeout = timeoutMs });
        _lastOperationTime = DateTime.UtcNow;
    }
    
    public async Task WaitForAnimationAsync(string selector, int timeoutMs = 30000, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        await _page!.Locator(selector).EvaluateAsync(@"el => {
            return new Promise(resolve => {
                const animations = el.getAnimations();
                if (animations.length === 0) {
                    resolve();
                    return;
                }
                Promise.all(animations.map(anim => anim.finished)).then(resolve);
                setTimeout(resolve, {0});
            });
        }".Replace("{0}", timeoutMs.ToString()));
        _lastOperationTime = DateTime.UtcNow;
    }
    
    public async Task WaitForStyleAsync(string selector, string propertyName, string expectedValue, int timeoutMs = 30000, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        await _page!.WaitForFunctionAsync(@"({ selector, prop, expected }) => {
            const el = document.querySelector(selector);
            if (!el) return false;
            const style = window.getComputedStyle(el);
            return style[prop] === expected;
        }", new { selector, prop = propertyName, expected = expectedValue }, new() { Timeout = timeoutMs });
        _lastOperationTime = DateTime.UtcNow;
    }
    
    public async Task UploadFileAsync(string selector, string filePath, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        _logger.Debug(_being.Id, "WebView: Uploading file {0}", filePath);
        
        await _page!.SetInputFilesAsync(selector, filePath);
        _lastOperationTime = DateTime.UtcNow;
    }
    
    public async Task<string> DownloadFileAsync(string url, string savePath, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        _logger.Debug(_being.Id, "WebView: Downloading file from {0}", url);
        
        var downloadTask = _page!.WaitForDownloadAsync();
        await _page!.EvaluateAsync($"window.location.href = '{url}'");
        
        var download = await downloadTask;
        await download.SaveAsAsync(savePath);
        _lastOperationTime = DateTime.UtcNow;
        
        return savePath;
    }
    
    public void SetTimeout(int timeoutSeconds)
    {
        _timeoutSeconds = timeoutSeconds;
        _page?.SetDefaultTimeout(timeoutSeconds * 1000);
    }
    
    public async Task ClearSessionAsync(CancellationToken ct = default)
    {
        _logger.Info(_being.Id, "WebView: Clearing session");
        
        if (_page != null)
        {
            await _page.CloseAsync();
            _page = null;
        }
        
        if (_browser != null)
        {
            await _browser.CloseAsync();
            _browser = null;
        }
    }
    
    public BrowserStatus GetStatus()
    {
        return new BrowserStatus
        {
            IsOpen = IsOpen,
            CurrentUrl = CurrentUrl,
            PageTitle = PageTitle,
            IsLoading = IsLoading,
            LastOperationTime = _lastOperationTime,
            TimeoutSetting = _timeoutSeconds,
            SessionId = _being.Id.ToString()
        };
    }
    
    private async Task EnsureInitializedAsync(CancellationToken ct = default)
    {
        if (_browser == null)
        {
            _logger.Info(_being.Id, "WebView: Initializing browser");
            
            // Auto-install browsers on first run if needed
            // 首次运行时自动安装浏览器
            await InstallBrowsersIfNeeded();
            
            var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new()
            {
                Headless = true, // 无头模式,用户不可见
                Args = new[] { 
                    "--disable-gpu",
                    "--no-sandbox",
                    "--disable-dev-shm-usage"
                }
            });
            
            var contextOptions = new BrowserNewContextOptions
            {
                IgnoreHTTPSErrors = true
            };
            
            // Only load storage state if file exists
            // 只有在文件存在时才加载会话状态
            var statePath = GetSessionStoragePath();
            if (!string.IsNullOrEmpty(statePath) && File.Exists(statePath))
            {
                contextOptions.StorageStatePath = statePath;
                _logger.Debug(_being.Id, "WebView: Loading browser state from {0}", statePath);
            }
            else
            {
                _logger.Debug(_being.Id, "WebView: No existing browser state, starting fresh");
            }
            
            var context = await _browser.NewContextAsync(contextOptions);
            
            _page = await context.NewPageAsync();
            _page.SetDefaultTimeout(_timeoutSeconds * 1000);
            
            _logger.Info(_being.Id, "WebView: Browser initialized successfully");
        }
    }
    
    /// <summary>
    /// Auto-install Playwright browsers on first run
    /// 首次运行时自动安装 Playwright 浏览器
    /// </summary>
    private static async Task InstallBrowsersIfNeeded()
    {
        try
        {
            // Check if browsers are already installed
            // 检查浏览器是否已安装
            var playwrightPath = Environment.GetEnvironmentVariable("PLAYWRIGHT_BROWSERS_PATH");
            if (string.IsNullOrEmpty(playwrightPath))
            {
                // Default location
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                playwrightPath = Path.Combine(appData, "ms-playwright");
            }
            
            if (!Directory.Exists(playwrightPath) || !Directory.EnumerateDirectories(playwrightPath).Any())
            {
                _logger.Info(null, "WebView: First run detected, installing browsers...");
                
                // Use the official Playwright installation script
                // 使用官方的 Playwright 安装脚本
                var assemblyDir = AppDomain.CurrentDomain.BaseDirectory;
                var installScript = Path.Combine(assemblyDir, "playwright.ps1");
                
                if (!File.Exists(installScript))
                {
                    _logger.Warn(null, "WebView: Playwright install script not found at {0}", installScript);
                    throw new FileNotFoundException(
                        $"Playwright install script not found. Please run: pwsh {installScript} install",
                        installScript);
                }
                
                // Install Chromium browser
                // This will download ~200MB on first run
                var installProcess = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{installScript}\" install chromium",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        WorkingDirectory = assemblyDir
                    }
                };
                
                installProcess.Start();
                await installProcess.WaitForExitAsync();
                
                if (installProcess.ExitCode == 0)
                {
                    _logger.Info(null, "WebView: Browsers installed successfully");
                }
                else
                {
                    var error = await installProcess.StandardError.ReadToEndAsync();
                    var output = await installProcess.StandardOutput.ReadToEndAsync();
                    _logger.Error(null, "WebView: Failed to install browsers. Output: {0}, Error: {1}", output, error);
                    throw new InvalidOperationException(
                        $"Failed to install Playwright browsers. Please run: pwsh \"{installScript}\" install chromium",
                        new Exception($"Output: {output}\nError: {error}"));
                }
            }
        }
        catch (Exception ex) when (ex is not InvalidOperationException && ex is not FileNotFoundException)
        {
            _logger.Warn(null, "WebView: Browser installation check failed: {0}", ex.Message);
            // Don't block initialization, let Playwright handle it
        }
    }

    
    private string? GetSessionStoragePath()
    {
        try
        {
            // Use the being's directory directly
            // 直接使用硅基人的目录
            if (string.IsNullOrEmpty(_being.BeingDirectory))
            {
                _logger.Warn(_being.Id, "WebView: Being directory is not set");
                return null;
            }
            
            // Store browser state in the being's directory
            // 将浏览器状态存储在硅基人目录中
            return Path.Combine(_being.BeingDirectory, "browser_state.json");
        }
        catch (Exception ex)
        {
            _logger.Warn(_being.Id, "WebView: Failed to get session storage path: {0}", ex.Message);
            return null;
        }
    }
    
    public void Dispose()
    {
        _logger.Info(_being.Id, "WebView: Disposing");
        ClearSessionAsync().GetAwaiter().GetResult();
    }
}
