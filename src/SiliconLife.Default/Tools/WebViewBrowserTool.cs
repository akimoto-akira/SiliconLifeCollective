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

using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// WebView browser operation tool
/// Provides browser automation capabilities for silicon beings
/// </summary>
public class WebViewBrowserTool : ITool
{
    public string Name => "webview_browser";

    public string Description =>
        "Browser automation tool for web page navigation, interaction, and data extraction. " +
        "Actions: 'open' (open browser), 'close' (close browser), 'navigate' (navigate to URL), " +
        "'click' (click element), 'input' (input text), 'scroll' (scroll page), " +
        "'execute_script' (execute JavaScript), 'get_page_text' (get page text), " +
        "'get_screenshot' (take screenshot), 'wait_for_element' (wait for element), " +
        "'get_element_info' (get element info), 'upload_file' (upload file), " +
        "'get_browser_status' (get browser status), 'set_timeout' (set timeout), " +
        "'clear_session' (clear browser session).";

    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return "WebView Browser";
    }

    public Dictionary<string, object> GetParameterSchema()
    {
        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The action to perform",
                    ["enum"] = new[] 
                    { 
                        "open", "close", "navigate", "click", "input", "scroll",
                        "execute_script", "get_page_text", "get_screenshot",
                        "wait_for_element", "get_element_info", "upload_file",
                        "get_browser_status", "set_timeout", "clear_session"
                    }
                },
                ["url"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "URL to navigate to (for navigate action)"
                },
                ["selector"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "CSS selector for element targeting (for click, input, wait, get_element_info actions)"
                },
                ["text"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Text to input (for input action)"
                },
                ["script"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "JavaScript code to execute (for execute_script action)"
                },
                ["x"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "X coordinate for scroll or screenshot region"
                },
                ["y"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Y coordinate for scroll or screenshot region"
                },
                ["width"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Width for screenshot region"
                },
                ["height"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Height for screenshot region"
                },
                ["full_page"] = new Dictionary<string, object>
                {
                    ["type"] = "boolean",
                    ["description"] = "Whether to capture full page screenshot"
                },
                ["format"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Screenshot format: 'png' or 'jpeg'",
                    ["enum"] = new[] { "png", "jpeg" }
                },
                ["timeout_ms"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Timeout in milliseconds for wait actions"
                },
                ["timeout_seconds"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Timeout in seconds for browser operations (for set_timeout action)"
                },
                ["file_path"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "File path for upload (for upload_file action)"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj))
        {
            return ToolResult.Failed("Missing 'action' parameter");
        }

        string action = actionObj?.ToString()?.ToLowerInvariant() ?? "";

        try
        {
            // Get the silicon being instance
            var beingManager = MainLoop.BeingManager;
            if (beingManager == null)
            {
                return ToolResult.Failed("SiliconBeingManager is not available");
            }

            var being = beingManager.GetAllBeings().FirstOrDefault(b => b.Id == callerId);
            if (being == null)
            {
                return ToolResult.Failed($"Silicon being {callerId} not found");
            }

            // Cast to DefaultSiliconBeing to access WebView
            if (being is not DefaultSiliconBeing defaultBeing)
            {
                return ToolResult.Failed("WebView is only supported in DefaultSiliconBeing");
            }

            return action switch
            {
                "open" => ExecuteOpen(defaultBeing),
                "close" => ExecuteClose(defaultBeing),
                "navigate" => ExecuteNavigate(defaultBeing, parameters).Result,
                "click" => ExecuteClick(defaultBeing, parameters).Result,
                "input" => ExecuteInput(defaultBeing, parameters).Result,
                "scroll" => ExecuteScroll(defaultBeing, parameters).Result,
                "execute_script" => ExecuteScript(defaultBeing, parameters).Result,
                "get_page_text" => ExecuteGetPageText(defaultBeing).Result,
                "get_screenshot" => ExecuteGetScreenshot(defaultBeing, parameters).Result,
                "wait_for_element" => ExecuteWaitForElement(defaultBeing, parameters).Result,
                "get_element_info" => ExecuteGetElementInfo(defaultBeing, parameters).Result,
                "upload_file" => ExecuteUploadFile(defaultBeing, parameters).Result,
                "get_browser_status" => ExecuteGetBrowserStatus(defaultBeing),
                "set_timeout" => ExecuteSetTimeout(defaultBeing, parameters),
                "clear_session" => ExecuteClearSession(defaultBeing).Result,
                _ => ToolResult.Failed($"Unknown action: {action}")
            };
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"WebView error: {ex.Message}");
        }
    }

    private ToolResult ExecuteOpen(DefaultSiliconBeing being)
    {
        var webView = being.GetWebView();
        return ToolResult.Successful("Browser opened successfully");
    }

    private ToolResult ExecuteClose(DefaultSiliconBeing being)
    {
        being.CloseWebView();
        return ToolResult.Successful("Browser closed successfully");
    }

    private async Task<ToolResult> ExecuteNavigate(DefaultSiliconBeing being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("url", out object? urlObj) || string.IsNullOrWhiteSpace(urlObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'url' parameter for navigate action");
        }

        string url = urlObj!.ToString()!;
        var webView = being.GetWebView();
        
        await webView.NavigateAsync(url);
        return ToolResult.Successful($"Navigated to {url}");
    }

    private async Task<ToolResult> ExecuteClick(DefaultSiliconBeing being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("selector", out object? selectorObj) || string.IsNullOrWhiteSpace(selectorObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'selector' parameter for click action");
        }

        string selector = selectorObj!.ToString()!;
        var webView = being.GetWebView();
        
        await webView.ClickAsync(selector);
        return ToolResult.Successful($"Clicked element: {selector}");
    }

    private async Task<ToolResult> ExecuteInput(DefaultSiliconBeing being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("selector", out object? selectorObj) || string.IsNullOrWhiteSpace(selectorObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'selector' parameter for input action");
        }

        if (!parameters.TryGetValue("text", out object? textObj))
        {
            return ToolResult.Failed("Missing 'text' parameter for input action");
        }

        string selector = selectorObj!.ToString()!;
        string text = textObj?.ToString() ?? "";
        var webView = being.GetWebView();
        
        await webView.InputAsync(selector, text);
        return ToolResult.Successful($"Input text into element: {selector}");
    }

    private async Task<ToolResult> ExecuteScroll(DefaultSiliconBeing being, Dictionary<string, object> parameters)
    {
        int x = parameters.TryGetValue("x", out var xObj) ? Convert.ToInt32(xObj) : 0;
        int y = parameters.TryGetValue("y", out var yObj) ? Convert.ToInt32(yObj) : 0;
        
        var webView = being.GetWebView();
        await webView.ScrollAsync(x, y);
        
        return ToolResult.Successful($"Scrolled to ({x}, {y})");
    }

    private async Task<ToolResult> ExecuteScript(DefaultSiliconBeing being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("script", out object? scriptObj) || string.IsNullOrWhiteSpace(scriptObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'script' parameter for execute_script action");
        }

        string script = scriptObj!.ToString()!;
        var webView = being.GetWebView();
        
        var result = await webView.ExecuteScriptAsync(script);
        return ToolResult.Successful($"Script executed. Result: {result ?? "null"}");
    }

    private async Task<ToolResult> ExecuteGetPageText(DefaultSiliconBeing being)
    {
        var webView = being.GetWebView();
        var text = await webView.GetPageTextAsync();
        return ToolResult.Successful(text);
    }

    private async Task<ToolResult> ExecuteGetScreenshot(DefaultSiliconBeing being, Dictionary<string, object> parameters)
    {
        var options = new ScreenshotOptions
        {
            Format = parameters.TryGetValue("format", out var fmtObj) && fmtObj?.ToString()?.ToLower() == "jpeg" 
                ? ScreenshotFormat.Jpeg 
                : ScreenshotFormat.Png,
            FullPage = parameters.TryGetValue("full_page", out var fullObj) && Convert.ToBoolean(fullObj)
        };

        if (parameters.TryGetValue("x", out var xObj) && parameters.TryGetValue("y", out var yObj) &&
            parameters.TryGetValue("width", out var wObj) && parameters.TryGetValue("height", out var hObj))
        {
            options = options with
            {
                X = Convert.ToInt32(xObj),
                Y = Convert.ToInt32(yObj),
                Width = Convert.ToInt32(wObj),
                Height = Convert.ToInt32(hObj)
            };
        }

        var webView = being.GetWebView();
        var screenshot = await webView.GetScreenshotAsync(options);
        
        // Save screenshot to being's directory
        var screenshotDir = Path.Combine(being.BeingDirectory ?? ".", "screenshots");
        if (!Directory.Exists(screenshotDir))
        {
            Directory.CreateDirectory(screenshotDir);
        }

        var filename = $"screenshot_{DateTime.UtcNow:yyyyMMdd_HHmmss}.{(options.Format == ScreenshotFormat.Jpeg ? "jpg" : "png")}";
        var filepath = Path.Combine(screenshotDir, filename);
        
        await File.WriteAllBytesAsync(filepath, screenshot);
        
        return ToolResult.Successful($"Screenshot saved to {filepath} ({screenshot.Length} bytes)");
    }

    private async Task<ToolResult> ExecuteWaitForElement(DefaultSiliconBeing being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("selector", out object? selectorObj) || string.IsNullOrWhiteSpace(selectorObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'selector' parameter for wait_for_element action");
        }

        string selector = selectorObj!.ToString()!;
        int timeoutMs = parameters.TryGetValue("timeout_ms", out var timeoutObj) ? Convert.ToInt32(timeoutObj) : 30000;
        
        var webView = being.GetWebView();
        await webView.WaitForElementAsync(selector, timeoutMs);
        
        return ToolResult.Successful($"Element appeared: {selector}");
    }

    private async Task<ToolResult> ExecuteGetElementInfo(DefaultSiliconBeing being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("selector", out object? selectorObj) || string.IsNullOrWhiteSpace(selectorObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'selector' parameter for get_element_info action");
        }

        string selector = selectorObj!.ToString()!;
        var webView = being.GetWebView();
        
        var info = await webView.GetElementInfoAsync(selector);
        
        var result = $"Tag: {info.TagName}\nText: {info.Text}\nVisible: {info.IsVisible}\nEnabled: {info.IsEnabled}\n";
        if (info.Attributes.Count > 0)
        {
            result += "Attributes:\n";
            foreach (var attr in info.Attributes)
            {
                result += $"  {attr.Key}: {attr.Value}\n";
            }
        }
        
        return ToolResult.Successful(result);
    }

    private async Task<ToolResult> ExecuteUploadFile(DefaultSiliconBeing being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("selector", out object? selectorObj) || string.IsNullOrWhiteSpace(selectorObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'selector' parameter for upload_file action");
        }

        if (!parameters.TryGetValue("file_path", out object? fileObj) || string.IsNullOrWhiteSpace(fileObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'file_path' parameter for upload_file action");
        }

        string selector = selectorObj!.ToString()!;
        string filePath = fileObj!.ToString()!;
        
        var webView = being.GetWebView();
        await webView.UploadFileAsync(selector, filePath);
        
        return ToolResult.Successful($"File uploaded: {filePath}");
    }

    private ToolResult ExecuteGetBrowserStatus(DefaultSiliconBeing being)
    {
        var webView = being.GetWebView();
        var status = webView.GetStatus();
        
        var result = $"Open: {status.IsOpen}\n" +
                     $"URL: {status.CurrentUrl ?? "N/A"}\n" +
                     $"Title: {status.PageTitle ?? "N/A"}\n" +
                     $"Loading: {status.IsLoading}\n" +
                     $"Timeout: {status.TimeoutSetting}s\n" +
                     $"Session: {status.SessionId}\n" +
                     $"Last Operation: {status.LastOperationTime:yyyy-MM-dd HH:mm:ss}";
        
        return ToolResult.Successful(result);
    }

    private ToolResult ExecuteSetTimeout(DefaultSiliconBeing being, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("timeout_seconds", out object? timeoutObj))
        {
            return ToolResult.Failed("Missing 'timeout_seconds' parameter for set_timeout action");
        }

        int timeoutSeconds = Convert.ToInt32(timeoutObj);
        var webView = being.GetWebView();
        webView.SetTimeout(timeoutSeconds);
        
        return ToolResult.Successful($"Timeout set to {timeoutSeconds} seconds");
    }

    private async Task<ToolResult> ExecuteClearSession(DefaultSiliconBeing being)
    {
        var webView = being.GetWebView();
        await webView.ClearSessionAsync();
        
        return ToolResult.Successful("Browser session cleared");
    }
}
