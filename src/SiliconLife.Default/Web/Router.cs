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
    private readonly Dictionary<string, Func<Controller>> _controllers = new();
    private readonly Dictionary<string, string> _mimeTypes = new();
    private string _staticFilesPath = string.Empty;
    private WebSocketRouteHandler? _webSocketHandler;
    private bool _isInitialized = false;
    private Action<string>? _onFirstInit;
    private const string InitPath = "/init";

    public delegate Task WebSocketRouteHandler(WebSocketHandler handler, string message);

    public Router()
    {
        InitializeMimeTypes();
    }

    public void RegisterControllers(SiliconBeingManager beingManager, ChatSystem chatSystem, Guid userId, Func<Guid, TaskCompletionSource<AskPermissionResult>> getPermissionTcs, WebCodeBrowser codeBrowser, DefaultConfigData configData, DefaultLocalizationBase localization, SkinManager skinManager, Action<string>? onFirstInit = null)
    {
        _onFirstInit = onFirstInit;
        RegisterController(() => new DashboardController(beingManager, chatSystem, skinManager), "/dashboard");
        RegisterController(() => new ChatController(beingManager, chatSystem, skinManager, userId), "/chat");
        RegisterController(() => new BeingController(beingManager, skinManager), "/beings");
        RegisterController(() => new TaskController(skinManager), "/tasks");
        RegisterController(() => new PermissionController(skinManager), "/permissions");
        RegisterController(() => new LogController(skinManager), "/logs");
        RegisterController(() => new ConfigController(skinManager), "/config");
        RegisterController(() => new MemoryController(skinManager), "/memory");
        RegisterController(() => new KnowledgeController(skinManager), "/knowledge");
        RegisterController(() => new ProjectController(skinManager), "/project");
        RegisterController(() => new ExecutorController(skinManager), "/executor");
        RegisterController(() => new CodeBrowserController(codeBrowser, skinManager), "/code");
        RegisterController(() => new PermissionRequestController(getPermissionTcs), "/permission-request");
        RegisterController(() => new InitController(configData, localization, skinManager, OnInitialized), "/init", "GET");
        RegisterController(() => new InitController(configData, localization, skinManager, OnInitialized), "/init", "POST");
        RegisterController(() => new InitController(configData, localization, skinManager, OnInitialized), "/init/browse", "GET");
    }

    private static readonly HashSet<string> StaticExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".html", ".htm", ".css", ".js", ".json", ".xml", ".txt",
        ".png", ".jpg", ".jpeg", ".gif", ".ico", ".svg",
        ".woff", ".woff2", ".ttf", ".eot", ".map"
    };

    private bool IsStaticFileRequest(string path)
    {
        var extension = Path.GetExtension(path);
        return !string.IsNullOrEmpty(extension) && StaticExtensions.Contains(extension);
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

    public void SetInitialized(bool initialized)
    {
        _isInitialized = initialized;
    }

    private void OnInitialized(string curatorName)
    {
        _onFirstInit?.Invoke(curatorName);
        SetInitialized(true);
    }

    public void RegisterWebSocket(string path, WebSocketRouteHandler handler)
    {
        _webSocketHandler = handler;
    }

    public void RegisterController(Func<Controller> controllerFactory, string basePath)
    {
        RegisterController(controllerFactory, basePath, "GET");
    }

    public void RegisterController(Func<Controller> controllerFactory, string basePath, string httpMethod)
    {
        var key = httpMethod.ToUpper() + ":" + basePath;
        _controllers[key] = controllerFactory;
    }

    public async Task HandleWebSocket(HttpListenerContext context)
    {
        if (_webSocketHandler != null)
        {
            var handler = new WebSocketHandler();
            handler.OnMessageReceived += async (ws, msg) =>
            {
                try
                {
                    await _webSocketHandler(handler, msg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"WebSocket handler error: {ex.Message}");
                }
            };
            await handler.HandleWebSocketRequest(context);
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
            if (!_isInitialized && path != InitPath && !path.StartsWith(InitPath + "/") && method != "POST")
            {
                if (!IsStaticFileRequest(path))
                {
                    response.StatusCode = 302;
                    response.Headers["Location"] = InitPath;
                    response.Close();
                    return;
                }
            }

            if (_isInitialized && path == InitPath)
            {
                response.StatusCode = 302;
                response.Headers["Location"] = "/chat";
                response.Close();
                return;
            }

            if (_isInitialized && (path == "/" || path == ""))
            {
                response.StatusCode = 302;
                response.Headers["Location"] = "/chat";
                response.Close();
                return;
            }

            var (controllerFactory, parameters) = MatchRoute(path, method);

            if (controllerFactory != null)
            {
                var controller = controllerFactory();
                controller.SetContext(context, parameters);
                controller.Handle();
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

    private (Func<Controller>? controllerFactory, Dictionary<string, string> parameters) MatchRoute(string path, string method)
    {
        foreach (var route in _controllers.Keys)
        {
            if (!route.StartsWith(method + ":"))
                continue;

            var pattern = route.Substring(method.Length + 1);
            var parameters = new Dictionary<string, string>();

            if (TryMatchPattern(pattern, path, parameters))
            {
                return (_controllers[route], parameters);
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
