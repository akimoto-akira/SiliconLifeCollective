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
/// Attribute to mark a configuration property as hidden from the UI
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class ConfigIgnoreAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the reason why this property is hidden (optional)
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// Initializes a new instance of the ConfigIgnoreAttribute class
    /// </summary>
    public ConfigIgnoreAttribute() { }

    /// <summary>
    /// Initializes a new instance of the ConfigIgnoreAttribute class with a reason
    /// </summary>
    /// <param name="reason">The reason why this property is hidden from the UI</param>
    public ConfigIgnoreAttribute(string reason)
    {
        Reason = reason;
    }
}
