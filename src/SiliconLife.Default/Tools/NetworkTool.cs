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

using System.Text.Json;
using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Network request tool.
/// Makes HTTP requests through NetworkExecutor.
/// Verifies the network executor pipeline.
/// </summary>
public class NetworkTool : ITool
{
    public string Name => "network";

    public string Description =>
        "Make HTTP requests to URLs. Supports GET and POST methods. " +
        "Returns the response body. Use for fetching web content, APIs, etc.";

    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return Name;
    }

    public Dictionary<string, object> GetParameterSchema()
    {
        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["url"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The URL to request"
                },
                ["method"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "HTTP method: GET (default) or POST",
                    ["enum"] = new[] { "GET", "POST" }
                },
                ["body"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Request body (for POST method, sent as application/json)"
                },
                ["headers"] = new Dictionary<string, object>
                {
                    ["type"] = "object",
                    ["description"] = "Additional HTTP headers as key-value pairs"
                }
            },
            ["required"] = new[] { "url" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("url", out object? urlObj) || string.IsNullOrWhiteSpace(urlObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'url' parameter");
        }

        string url = urlObj.ToString()!;

        // Validate URL scheme
        if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return ToolResult.Failed("URL must start with http:// or https://");
        }

        string method = parameters.TryGetValue("method", out object? methodObj)
            ? (methodObj?.ToString()?.ToUpperInvariant() ?? "GET")
            : "GET";

        var requestParams = new Dictionary<string, object>();

        if (parameters.TryGetValue("body", out object? bodyObj) && bodyObj != null)
        {
            requestParams["body"] = bodyObj.ToString()!;
        }

        if (parameters.TryGetValue("headers", out object? headersObj) && headersObj != null)
        {
            requestParams["headers"] = FlattenToStringDictionary(headersObj);
        }

        ExecutorRequest request = new(callerId, url, method, requestParams);
        ExecutorResult result = NetworkExecutor.Execute(request);

        if (result.Success)
        {
            // Truncate very long responses
            string output = result.Output ?? "";
            if (output.Length > 5000)
            {
                output = output.Substring(0, 5000) + "\n... (truncated)";
            }
            return ToolResult.Successful(output);
        }

        return ToolResult.Failed(result.Error ?? "Network request failed");
    }

    private static Dictionary<string, object> FlattenToStringDictionary(object obj)
    {
        Dictionary<string, object> result = new Dictionary<string, object>();

        if (obj is Dictionary<string, object> dict)
        {
            foreach (KeyValuePair<string, object> kv in dict)
                result[kv.Key] = kv.Value?.ToString() ?? "";
        }
        else if (obj is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Object)
        {
            foreach (JsonProperty property in jsonElement.EnumerateObject())
                result[property.Name] = property.Value.ToString();
        }

        return result;
    }
}
