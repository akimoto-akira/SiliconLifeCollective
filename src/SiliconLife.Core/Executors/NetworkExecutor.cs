// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace SiliconLife.Collective;

/// <summary>
/// Static executor for HTTP network requests.
/// Provides timeout control for AI-initiated network operations.
/// Permission checking via <see cref="PermissionManager"/> through <see cref="ServiceRegistry"/>.
/// Circuit breaker is not implemented yet.
/// </summary>
public static class NetworkExecutor
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);
    private static readonly HttpClient HttpClient = new() { Timeout = DefaultTimeout };

    /// <summary>
    /// Executes a network request synchronously with timeout.
    /// Checks permission via the caller's PermissionManager before executing.
    /// </summary>
    public static ExecutorResult Execute(ExecutorRequest request, TimeSpan? timeout = null)
    {
        if (!CheckPermission(request))
        {
            return ExecutorResult.Failed($"Permission denied: network access to '{request.ResourcePath}'");
        }

        TimeSpan actualTimeout = timeout ?? DefaultTimeout;

        try
        {
            Task<ExecutorResult> task = Task.Run(() => ExecuteCore(request));
            if (task.Wait(actualTimeout))
            {
                return task.Result;
            }
            return ExecutorResult.Failed("Operation timed out");
        }
        catch (AggregateException ex)
        {
            Exception? inner = ex.InnerException;
            return ExecutorResult.Failed(inner?.Message ?? ex.Message);
        }
    }

    private static ExecutorResult ExecuteCore(ExecutorRequest request)
    {
        string method = request.Parameters.TryGetValue("method", out object? methodObj)
            ? methodObj?.ToString()?.ToUpperInvariant() ?? "GET"
            : "GET";

        string? body = request.Parameters.TryGetValue("body", out object? bodyObj)
            ? bodyObj?.ToString()
            : null;

        try
        {
            HttpMethod httpMethod = new(method);
            using HttpRequestMessage httpRequest = new(httpMethod, request.ResourcePath);

            if (body != null && (method == "POST" || method == "PUT" || method == "PATCH"))
            {
                httpRequest.Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
            }

            // Add custom headers
            if (request.Parameters.TryGetValue("headers", out object? headersObj) && headersObj is Dictionary<string, object> headers)
            {
                foreach (KeyValuePair<string, object> header in headers)
                {
                    httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value?.ToString());
                }
            }

            HttpResponseMessage httpResponse = HttpClient.SendAsync(httpRequest).GetAwaiter().GetResult();
            string responseContent = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            if (httpResponse.IsSuccessStatusCode)
            {
                return ExecutorResult.Successful(responseContent, (int)httpResponse.StatusCode);
            }

            return ExecutorResult.Failed(
                $"HTTP {(int)httpResponse.StatusCode} {httpResponse.ReasonPhrase}: {responseContent}",
                (int)httpResponse.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            return ExecutorResult.Failed($"Network error: {ex.Message}");
        }
        catch (TaskCanceledException)
        {
            return ExecutorResult.Failed("Request timed out");
        }
        catch (Exception ex)
        {
            return ExecutorResult.Failed($"Network error: {ex.Message}");
        }
    }

    /// <summary>
    /// Checks permission for a network operation via the caller's PermissionManager.
    /// </summary>
    private static bool CheckPermission(ExecutorRequest request)
    {
        PermissionManager? pm = ServiceRegistry.Instance.GetPermissionManager(request.CallerId);
        if (pm == null) return true;
        return pm.CheckPermission(request.CallerId, PermissionType.NetworkAccess, request.ResourcePath);
    }
}
