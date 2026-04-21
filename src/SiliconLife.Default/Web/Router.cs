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

namespace SiliconLife.Default.Web;

public class Router
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<Router>();
    private readonly Dictionary<string, Func<Controller>> _controllers = new();
    private SSEHandler? _sharedSSEHandler;
    private bool _isInitialized = false;
    private Action<string>? _onFirstInit;
    private const string InitPath = "/init";

    public void RegisterControllers()
    {
        RegisterController(() => new DashboardController(), "/dashboard");
        RegisterController(() => new DashboardController(), "/api/dashboard/stats");
        RegisterController(() => new DashboardController(), "/api/dashboard/metrics");
        RegisterController(() => new ChatController(), "/chat");
        RegisterController(() => new ChatController(), "/api/chat/stream");
        RegisterController(() => new ChatController(), "/api/chat/conversations");
        RegisterController(() => new ChatController(), "/api/chat/messages");
        RegisterController(() => new ChatController(), "/api/chat/history");
        RegisterController(() => new ChatController(), "/api/chat/send", "POST");
        RegisterController(() => new BeingController(), "/beings");
        RegisterController(() => new BeingController(), "/beings/soul");
        RegisterController(() => new BeingController(), "/api/beings/list");
        RegisterController(() => new BeingController(), "/api/beings/detail");
        RegisterController(() => new BeingController(), "/api/beings/soul/save", "POST");
        RegisterController(() => new TimerController(), "/timers");
        RegisterController(() => new TimerController(), "/api/timers/list");
        RegisterController(() => new TaskController(), "/tasks");
        RegisterController(() => new PermissionController(), "/permissions");
        RegisterController(() => new PermissionController(), "/api/permissions/list");
        RegisterController(() => new PermissionController(), "/api/permissions/save", "POST");
        RegisterController(() => new LogController(), "/logs");
        RegisterController(() => new LogController(), "/api/logs/list");
        RegisterController(() => new AuditController(), "/audit");
        RegisterController(() => new AuditController(), "/api/audit/list");
        RegisterController(() => new AuditController(), "/api/audit/summary");
        RegisterController(() => new AuditController(), "/api/audit/trend");
        RegisterController(() => new AuditController(), "/api/audit/export");
        RegisterController(() => new ConfigController(), "/config");
        RegisterController(() => new ConfigController(), "/config/save", "POST");
        RegisterController(() => new ConfigController(), "/config/aioptions", "GET");
        RegisterController(() => new MemoryController(), "/memory");
        RegisterController(() => new MemoryController(), "/api/memory/list");
        RegisterController(() => new MemoryController(), "/api/memory/detail/{id}");
        RegisterController(() => new MemoryController(), "/api/memory/stats");
        RegisterController(() => new MemoryController(), "/api/memory/search");
        RegisterController(() => new MemoryController(), "/api/memory/beings");
        RegisterController(() => new MemoryController(), "/api/memory/trace/{id}");
        RegisterController(() => new KnowledgeController(), "/knowledge");
        RegisterController(() => new ProjectController(), "/project");
        RegisterController(() => new ExecutorController(), "/executor");
        RegisterController(() => new CodeBrowserController(), "/code");
        RegisterController(() => new CodeHoverController(), "/api/code/hover", "GET");
        RegisterController(() => new CodeHoverController(), "/api/code/hover", "POST");
        RegisterController(() => new CodeHoverController(), "/api/code/register", "POST");
        RegisterController(() => new CodeHoverController(), "/api/code/update", "POST");
        RegisterController(() => new CodeHoverController(), "/api/code/unregister", "POST");
        RegisterController(() => new PermissionRequestController(), "/permission/request");
        RegisterController(() => new PermissionRequestController(), "/permission/check");
        RegisterController(() => new PermissionRequestController(), "/permission/respond");
        RegisterController(() => new AboutController(), "/about");
        RegisterController(() => new InitController(), "/init", "GET");
        RegisterController(() => new InitController(), "/init", "POST");
        RegisterController(() => new InitController(), "/init/browse", "GET");
    }

    /// <summary>Sets the callback invoked when first-run initialization completes.</summary>
    public void SetOnFirstInit(Action<string>? callback)
    {
        _onFirstInit = callback;
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

    public void SetInitialized(bool initialized)
    {
        _isInitialized = initialized;
    }

    public void NotifyInitialized(string curatorName)
    {
        _logger.Info("First-time initialization triggered");
        _onFirstInit?.Invoke(curatorName);
        SetInitialized(true);
    }

    public void SetSharedSSEHandler(SSEHandler handler)
    {
        _sharedSSEHandler = handler;
    }

    public void RegisterController(Func<Controller> controllerFactory, string basePath)
    {
        RegisterController(controllerFactory, basePath, "GET");
    }

    public void RegisterController(Func<Controller> controllerFactory, string basePath, string httpMethod)
    {
        string key = httpMethod.ToUpper() + ":" + basePath;
        _controllers[key] = controllerFactory;
    }

    public async Task HandleSSE(HttpListenerContext context, Guid userId, Guid? channelId)
    {
        if (_sharedSSEHandler != null)
        {
            await _sharedSSEHandler.HandleSSERequest(context, userId, channelId);
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
                _logger.Debug($"Route matched: {method} {path} -> {pattern}");
                return (_controllers[route], parameters);
            }
        }

        _logger.Warn($"No route matched: {method} {path}");
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

    private async Task Send404(HttpListenerResponse response)
    {
        byte[] buffer = Encoding.UTF8.GetBytes("404 Not Found");
        response.StatusCode = 404;
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
