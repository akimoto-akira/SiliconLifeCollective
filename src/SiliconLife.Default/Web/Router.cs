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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SiliconLife.Collective;
using SiliconLife.Default.IM;

namespace SiliconLife.Default.Web;

public class Router
{
    private readonly Dictionary<string, RouteHandler> _routes = new();
    private readonly Dictionary<string, string> _mimeTypes = new();
    private string _staticFilesPath = string.Empty;
    private WebSocketHandler? _webSocketHandler;

    public delegate Task RouteHandler(HttpListenerContext context, Dictionary<string, string> parameters);
    public delegate Task WebSocketRouteHandler(WebSocketHandler handler, string message);

    public Router()
    {
        InitializeMimeTypes();
    }

    public void RegisterControllers(SiliconBeingManager beingManager, ChatSystem chatSystem, Guid userId, Func<Guid, TaskCompletionSource<AskPermissionResult>> getPermissionTcs, WebCodeBrowser codeBrowser, DefaultConfigData configData)
    {
        Register("GET:/", async (context, parameters) =>
        {
            var html = HtmlBuilder.Create()
                .DocType()
                .Html()
                .Head()
                    .MetaCharset()
                    .MetaViewport()
                    .Title("Silicon Life Collective")
                    .Style(@"
                        body { font-family: Arial, sans-serif; margin: 0; padding: 20px; background: #f5f5f5; }
                        .container { max-width: 800px; margin: 0 auto; background: white; padding: 20px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
                        h1 { color: #333; }
                        .status { padding: 10px; background: #e7f3e7; border-left: 4px solid #4caf50; margin: 10px 0; }
                        .info { color: #666; margin: 5px 0; }
                    ")
                .Body()
                    .Div()
                        .H1("Silicon Life Collective")
                        .Div("Welcome to Silicon Life Collective")
                        .Div()
                            .Class("status")
                            .Text($"System Status: Running")
                        .Div()
                            .Class("info")
                            .Text($"Curator GUID: {configData.CuratorGuid}")
                        .Div()
                            .Class("info")
                            .Text($"Data Directory: {configData.DataDirectory}")
                    .Build();

            await context.Response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes(html));
            context.Response.ContentType = "text/html; charset=utf-8";
            context.Response.StatusCode = 200;
            context.Response.Close();
        });

        Register("GET:/health", async (context, parameters) =>
        {
            var json = System.Text.Json.JsonSerializer.Serialize(new
            {
                status = "ok",
                timestamp = DateTime.UtcNow,
                curator = configData.CuratorGuid.ToString()
            });

            await context.Response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes(json));
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = 200;
            context.Response.Close();
        });

        RegisterController(new DashboardController(beingManager, chatSystem), "/dashboard");
        RegisterController(new ChatController(beingManager, chatSystem, userId), "/chat");
        RegisterController(new BeingController(beingManager), "/beings");
        RegisterController(new TaskController(), "/tasks");
        RegisterController(new PermissionController(), "/permissions");
        RegisterController(new LogController(), "/logs");
        RegisterController(new ConfigController(), "/config");
        RegisterController(new MemoryController(), "/memory");
        RegisterController(new KnowledgeController(), "/knowledge");
        RegisterController(new ProjectController(), "/project");
        RegisterController(new ExecutorController(), "/executor");
        RegisterController(new CodeBrowserController(codeBrowser), "/code");
        RegisterController(new PermissionRequestController(getPermissionTcs), "/permission-request");
    }

    private void InitializeMimeTypes()
    {
        _mimeTypes[".html"] = "text/html; charset=utf-8";
        _mimeTypes[".htm"] = "text/html; charset=utf-8";
        _mimeTypes[".css"] = "text/css; charset=utf-8";
        _mimeTypes[".js"] = "application/javascript; charset=utf-8";
        _mimeTypes[".json"] = "application/json; charset=utf-8";
        _mimeTypes[".xml"] = "application/xml; charset=utf-8";
        _mimeTypes[".txt"] = "text/plain; charset=utf-8";
        _mimeTypes[".png"] = "image/png";
        _mimeTypes[".jpg"] = "image/jpeg";
        _mimeTypes[".jpeg"] = "image/jpeg";
        _mimeTypes[".gif"] = "image/gif";
        _mimeTypes[".ico"] = "image/x-icon";
        _mimeTypes[".svg"] = "image/svg+xml";
        _mimeTypes[".woff"] = "application/font-woff";
        _mimeTypes[".woff2"] = "application/font-woff2";
        _mimeTypes[".ttf"] = "application/font-ttf";
        _mimeTypes[".eot"] = "application/vnd.ms-fontobject";
    }

    public void SetStaticFilesPath(string path)
    {
        _staticFilesPath = path;
    }

    public void Register(string pattern, RouteHandler handler)
    {
        _routes[pattern] = handler;
    }

    public void RegisterWebSocket(string path, WebSocketRouteHandler handler)
    {
        _webSocketHandler = new WebSocketHandler();
        _webSocketHandler.OnMessageReceived += async (ws, msg) =>
        {
            try
            {
                await handler(_webSocketHandler, msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket handler error: {ex.Message}");
            }
        };
    }

    public void RegisterController(Controller controller, string basePath)
    {
        Register("GET:" + basePath, async (ctx, p) =>
        {
            controller.SetContext(ctx, p);
            await controller.HandleAsync();
        });
    }

    public void RegisterController(Controller controller, string basePath, string httpMethod)
    {
        var key = httpMethod.ToUpper() + ":" + basePath;
        _routes[key] = async (ctx, p) =>
        {
            controller.SetContext(ctx, p);
            await controller.HandleAsync();
        };
    }

    public async Task HandleWebSocket(HttpListenerContext context)
    {
        if (_webSocketHandler != null)
        {
            await _webSocketHandler.HandleWebSocketRequest(context);
        }
        else
        {
            context.Response.StatusCode = 404;
            context.Response.Close();
        }
    }

    public async Task HandleRequest(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;
        var path = request.Url?.AbsolutePath ?? "/";
        var method = request.HttpMethod;

        try
        {
            var (handler, parameters) = MatchRoute(path, method);

            if (handler != null)
            {
                await handler(context, parameters);
            }
            else if (!string.IsNullOrEmpty(_staticFilesPath))
            {
                await ServeStaticFile(context, path);
            }
            else
            {
                await Send404(response);
            }
        }
        catch (Exception ex)
        {
            await Send500(response, ex.Message);
        }
    }

    private (RouteHandler? handler, Dictionary<string, string> parameters) MatchRoute(string path, string method)
    {
        foreach (var route in _routes.Keys)
        {
            if (!route.StartsWith(method + ":"))
                continue;

            var pattern = route.Substring(method.Length + 1);
            var parameters = new Dictionary<string, string>();

            if (TryMatchPattern(pattern, path, parameters))
            {
                return (_routes[route], parameters);
            }
        }

        return (null, new Dictionary<string, string>());
    }

    private bool TryMatchPattern(string pattern, string path, Dictionary<string, string> parameters)
    {
        if (pattern == path)
            return true;

        var patternParts = pattern.Trim('/').Split('/');
        var pathParts = path.Trim('/').Split('/');

        if (patternParts.Length != pathParts.Length)
            return false;

        for (int i = 0; i < patternParts.Length; i++)
        {
            if (patternParts[i].StartsWith('{') && patternParts[i].EndsWith('}'))
            {
                var paramName = patternParts[i].Trim('{', '}');
                parameters[paramName] = Uri.UnescapeDataString(pathParts[i]);
            }
            else if (!string.Equals(patternParts[i], pathParts[i], StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        return true;
    }

    private async Task ServeStaticFile(HttpListenerContext context, string path)
    {
        var request = context.Request;
        var response = context.Response;

        var filePath = Path.GetFullPath(Path.Combine(_staticFilesPath, path.TrimStart('/')));

        if (!filePath.StartsWith(Path.GetFullPath(_staticFilesPath), StringComparison.OrdinalIgnoreCase))
        {
            await Send403(response);
            return;
        }

        if (!File.Exists(filePath))
        {
            await Send404(response);
            return;
        }

        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        var contentType = _mimeTypes.GetValueOrDefault(extension, "application/octet-stream");

        var content = await File.ReadAllBytesAsync(filePath);

        response.ContentType = contentType;
        response.ContentLength64 = content.Length;
        response.StatusCode = 200;

        await response.OutputStream.WriteAsync(content);
        response.Close();
    }

    private async Task Send404(HttpListenerResponse response)
    {
        byte[] buffer = Encoding.UTF8.GetBytes("404 Not Found");
        response.StatusCode = 404;
        response.ContentType = "text/plain; charset=utf-8";
        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer);
        response.Close();
    }

    private async Task Send403(HttpListenerResponse response)
    {
        byte[] buffer = Encoding.UTF8.GetBytes("403 Forbidden");
        response.StatusCode = 403;
        response.ContentType = "text/plain; charset=utf-8";
        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer);
        response.Close();
    }

    private async Task Send500(HttpListenerResponse response, string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes($"500 Internal Server Error: {message}");
        response.StatusCode = 500;
        response.ContentType = "text/plain; charset=utf-8";
        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer);
        response.Close();
    }
}
