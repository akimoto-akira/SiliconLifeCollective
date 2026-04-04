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
using System.Text.Json;

namespace SiliconLife.Default.Web;

public abstract class Controller
{
    protected HttpListenerContext Context { get; private set; } = null!;
    protected HttpListenerRequest Request => Context.Request;
    protected HttpListenerResponse Response => Context.Response;
    protected Dictionary<string, string> Parameters { get; private set; } = null!;

    internal void SetContext(HttpListenerContext context, Dictionary<string, string> parameters)
    {
        Context = context;
        Parameters = parameters;
    }

    public abstract void Handle();

    protected void RenderHtml(string html, int statusCode = 200)
    {
        Response.StatusCode = statusCode;
        Response.ContentType = "text/html; charset=utf-8";
        Response.Headers["Cache-Control"] = "no-cache";
        
        byte[] buffer = Encoding.UTF8.GetBytes(html);
        Response.ContentLength64 = buffer.Length;
        Response.OutputStream.Write(buffer);
        Response.Close();
    }

    protected void RenderJson(object? data, int statusCode = 200)
    {
        Response.StatusCode = statusCode;
        Response.ContentType = "application/json; charset=utf-8";
        Response.Headers["Cache-Control"] = "no-cache";
        
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        });
        
        byte[] buffer = Encoding.UTF8.GetBytes(json);
        Response.ContentLength64 = buffer.Length;
        Response.OutputStream.Write(buffer);
        Response.Close();
    }

    protected void RenderText(string text, string contentType = "text/plain; charset=utf-8", int statusCode = 200)
    {
        Response.StatusCode = statusCode;
        Response.ContentType = contentType;
        
        byte[] buffer = Encoding.UTF8.GetBytes(text);
        Response.ContentLength64 = buffer.Length;
        Response.OutputStream.Write(buffer);
        Response.Close();
    }

    protected void Redirect(string location, int statusCode = 302)
    {
        Response.StatusCode = statusCode;
        Response.Headers["Location"] = location;
        Response.Close();
    }

    protected string GetQueryValue(string key, string defaultValue = "")
    {
        return Request.QueryString[key] ?? defaultValue;
    }

    protected string GetRequestBody()
    {
        using var reader = new StreamReader(Request.InputStream, Request.ContentEncoding);
        return reader.ReadToEnd();
    }

    protected T? GetJsonBody<T>() where T : class
    {
        try
        {
            using var reader = new StreamReader(Request.InputStream, Request.ContentEncoding);
            var body = reader.ReadToEnd();
            return JsonSerializer.Deserialize<T>(body, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
        catch
        {
            return null;
        }
    }

    protected void SetCookie(string name, string value, int expiresMinutes = 60)
    {
        var cookie = new Cookie(name, value)
        {
            Path = "/",
            Expires = DateTime.Now.AddMinutes(expiresMinutes),
            HttpOnly = true
        };
        Response.Cookies.Add(cookie);
    }

    protected string? GetCookie(string name)
    {
        return Request.Cookies[name]?.Value;
    }

    protected void SetHeader(string name, string value)
    {
        Response.Headers[name] = value;
    }

    protected void SetStatusCode(int statusCode)
    {
        Response.StatusCode = statusCode;
    }

    protected bool IsAjaxRequest()
    {
        return Request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }

    protected string GetClientIp()
    {
        var ip = Request.Headers["X-Forwarded-For"];
        if (string.IsNullOrEmpty(ip))
        {
            ip = Request.RemoteEndPoint?.Address?.ToString();
        }
        return ip ?? "unknown";
    }
}

public class ControllerActionResult
{
    public bool IsRedirect { get; set; }
    public string RedirectUrl { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string ContentType { get; set; } = "text/html; charset=utf-8";
    public int StatusCode { get; set; } = 200;
    public object? JsonData { get; set; }
}
