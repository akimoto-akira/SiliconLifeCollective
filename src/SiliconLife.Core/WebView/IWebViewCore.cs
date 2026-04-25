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

namespace SiliconLife.Collective;

/// <summary>
/// WebView Core Abstract Interface
/// Defines cross-platform browser operation contract
/// </summary>
public interface IWebViewCore : IDisposable
{
    /// <summary>
    /// Is browser opened
    /// </summary>
    bool IsOpen { get; }
    
    /// <summary>
    /// Current page URL
    /// </summary>
    string? CurrentUrl { get; }
    
    /// <summary>
    /// Current page title
    /// </summary>
    string? PageTitle { get; }
    
    /// <summary>
    /// Is page loading
    /// </summary>
    bool IsLoading { get; }
    
    // ========== Basic Navigation ==========
    
    /// <summary>
    /// Navigate to specified URL
    /// </summary>
    Task NavigateAsync(string url, CancellationToken ct = default);
    
    /// <summary>
    /// Go back
    /// </summary>
    Task GoBackAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Go forward
    /// </summary>
    Task GoForwardAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Refresh
    /// </summary>
    Task RefreshAsync(CancellationToken ct = default);
    
    // ========== Page Interaction ==========
    
    /// <summary>
    /// Click element
    /// </summary>
    Task ClickAsync(string selector, CancellationToken ct = default);
    
    /// <summary>
    /// Input text
    /// </summary>
    Task InputAsync(string selector, string text, CancellationToken ct = default);
    
    /// <summary>
    /// Scroll page
    /// </summary>
    Task ScrollAsync(int x, int y, CancellationToken ct = default);
    
    /// <summary>
    /// Hover element
    /// </summary>
    Task HoverAsync(string selector, CancellationToken ct = default);
    
    // ========== Script Execution ==========
    
    /// <summary>
    /// Execute JavaScript code
    /// </summary>
    Task<string?> ExecuteScriptAsync(string script, CancellationToken ct = default);
    
    /// <summary>
    /// Call JavaScript function
    /// </summary>
    Task<string?> ExecuteFunctionAsync(string functionName, object[] args, CancellationToken ct = default);
    
    // ========== Content Retrieval ==========
    
    /// <summary>
    /// Get page text
    /// </summary>
    Task<string> GetPageTextAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Get page HTML
    /// </summary>
    Task<string> GetHtmlAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Get page screenshot
    /// </summary>
    Task<byte[]> GetScreenshotAsync(ScreenshotOptions? options = null, CancellationToken ct = default);
    
    // ========== Element Operations ==========
    
    /// <summary>
    /// Get element info
    /// </summary>
    Task<ElementInfo> GetElementInfoAsync(string selector, CancellationToken ct = default);
    
    /// <summary>
    /// Get element text
    /// </summary>
    Task<string> GetElementTextAsync(string selector, CancellationToken ct = default);
    
    /// <summary>
    /// Wait for element
    /// </summary>
    Task WaitForElementAsync(string selector, int timeoutMs = 30000, CancellationToken ct = default);
    
    /// <summary>
    /// Wait for animation
    /// </summary>
    Task WaitForAnimationAsync(string selector, int timeoutMs = 30000, CancellationToken ct = default);
    
    /// <summary>
    /// Wait for style
    /// </summary>
    Task WaitForStyleAsync(string selector, string propertyName, string expectedValue, int timeoutMs = 30000, CancellationToken ct = default);
    
    // ========== File Operations ==========
    
    /// <summary>
    /// Upload file
    /// </summary>
    Task UploadFileAsync(string selector, string filePath, CancellationToken ct = default);
    
    /// <summary>
    /// Download file
    /// </summary>
    Task<string> DownloadFileAsync(string url, string savePath, CancellationToken ct = default);
    
    // ========== State Management ==========
    
    /// <summary>
    /// Set timeout
    /// </summary>
    void SetTimeout(int timeoutSeconds);
    
    /// <summary>
    /// Clear session
    /// </summary>
    Task ClearSessionAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Get browser status
    /// </summary>
    BrowserStatus GetStatus();
}
