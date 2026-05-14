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

using SiliconLife.McpServer.Tools;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001); // Server port
});

// Register services
builder.Services.AddSingleton<IBrowserPool, BrowserPool>();

var app = builder.Build();

// Health check
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

// Browser tools API
app.MapPost("/api/browser/navigate", async (BrowserRequest request, IBrowserPool pool) =>
{
    var webView = await pool.GetWebViewAsync();
    await webView.NavigateAsync(request.Url);
    var title = webView.PageTitle ?? "Unknown";
    return Results.Ok(new { message = $"Navigated to {request.Url}", title });
});

app.MapPost("/api/browser/click", async (BrowserRequest request, IBrowserPool pool) =>
{
    var webView = await pool.GetWebViewAsync();
    await webView.ClickAsync(request.Selector);
    return Results.Ok(new { message = $"Clicked element: {request.Selector}" });
});

app.MapPost("/api/browser/input", async (BrowserInputRequest request, IBrowserPool pool) =>
{
    var webView = await pool.GetWebViewAsync();
    await webView.InputAsync(request.Selector, request.Text);
    return Results.Ok(new { message = $"Input text into {request.Selector}" });
});

app.MapGet("/api/browser/get_page_text", async (IBrowserPool pool) =>
{
    var webView = await pool.GetWebViewAsync();
    var text = await webView.GetPageTextAsync();
    return Results.Ok(new { text });
});

app.MapPost("/api/browser/screenshot", async (ScreenshotRequest request, IBrowserPool pool) =>
{
    var webView = await pool.GetWebViewAsync();
    var bytes = await webView.GetScreenshotAsync(new() { FullPage = request.FullPage });
    var base64 = Convert.ToBase64String(bytes);
    return Results.Ok(new { base64, dataUrl = $"data:image/png;base64,{base64}" });
});

app.MapPost("/api/browser/evaluate", async (ScriptRequest request, IBrowserPool pool) =>
{
    var webView = await pool.GetWebViewAsync();
    var result = await webView.ExecuteScriptAsync(request.Script);
    return Results.Ok(new { result = result ?? "null" });
});

app.MapPost("/api/browser/wait_for_element", async (WaitRequest request, IBrowserPool pool) =>
{
    var webView = await pool.GetWebViewAsync();
    await webView.WaitForElementAsync(request.Selector, request.TimeoutMs);
    return Results.Ok(new { message = $"Element appeared: {request.Selector}" });
});

app.MapGet("/api/browser/status", async (IBrowserPool pool) =>
{
    var webView = await pool.GetWebViewAsync();
    var status = webView.GetStatus();
    return Results.Ok(new
    {
        isOpen = status.IsOpen,
        url = status.CurrentUrl ?? "N/A",
        title = status.PageTitle ?? "N/A"
    });
});

app.MapPost("/api/browser/close", async (IBrowserPool pool) =>
{
    await pool.ClearAsync();
    return Results.Ok(new { message = "Browser closed and session cleared" });
});

app.MapPost("/api/browser/run_test", async (TestRequest request, IBrowserPool pool) =>
{
    var webView = await pool.GetWebViewAsync();
    await webView.NavigateAsync(request.Url);
    var result = await webView.ExecuteScriptAsync(request.TestScript);
    return Results.Ok(new { result = result ?? "null" });
});

Console.WriteLine("🦞 SiliconLife Browser API Server started on http://localhost:5001");
Console.WriteLine("📡 API endpoint: http://localhost:5001/api/browser/*");
Console.WriteLine("🔧 Tools available for Cursor/Trae via HTTP");

await app.RunAsync();

// Request/Response models
public record BrowserRequest(string Url, string? Selector = null);
public record BrowserInputRequest(string Selector, string Text);
public record ScreenshotRequest(bool FullPage = false);
public record ScriptRequest(string Script);
public record WaitRequest(string Selector, int TimeoutMs = 30000);
public record TestRequest(string Url, string TestScript);
