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

using System.Text.Json.Serialization;

namespace SiliconLife.App.Web.Component;

/// <summary>
/// DOM update instruction - for SSE push partial updates
/// </summary>
public class DomUpdate
{
    /// <summary>
    /// Target selector (e.g., "#chat-messages")
    /// </summary>
    [JsonPropertyName("target")]
    public string Target { get; set; } = "";

    /// <summary>
    /// Action type (append/replace/remove/prepend)
    /// </summary>
    [JsonPropertyName("action")]
    public string Action { get; set; } = "append";

    /// <summary>
    /// HTML content
    /// </summary>
    [JsonPropertyName("html")]
    public string Html { get; set; } = "";

    /// <summary>
    /// Create DOM update instruction
    /// </summary>
    public static DomUpdate Append(string target, string html)
    {
        return new DomUpdate
        {
            Target = target,
            Action = "append",
            Html = html
        };
    }

    /// <summary>
    /// Create replace instruction
    /// </summary>
    public static DomUpdate Replace(string target, string html)
    {
        return new DomUpdate
        {
            Target = target,
            Action = "replace",
            Html = html
        };
    }

    /// <summary>
    /// Create remove instruction
    /// </summary>
    public static DomUpdate Remove(string target)
    {
        return new DomUpdate
        {
            Target = target,
            Action = "remove",
            Html = ""
        };
    }

    /// <summary>
    /// Create prepend instruction
    /// </summary>
    public static DomUpdate Prepend(string target, string html)
    {
        return new DomUpdate
        {
            Target = target,
            Action = "prepend",
            Html = html
        };
    }

    /// <summary>
    /// Serialize to JSON
    /// </summary>
    public string ToJson()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}
