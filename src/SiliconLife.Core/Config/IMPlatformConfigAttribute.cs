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
/// Attribute for marking configuration properties that are specific to a particular IM platform type.
/// Properties with this attribute will only be displayed when the corresponding IM platform is selected.
/// This is similar to AIClientConfigAttribute but for IM platform configurations.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class IMPlatformConfigAttribute : Attribute
{
    /// <summary>
    /// Gets the IM platform type name that this configuration property belongs to.
    /// This should match the Platform identifier (e.g., "feishu", "wecom", "dingtalk").
    /// </summary>
    public string Platform { get; }

    /// <summary>
    /// Gets or sets the display order within the IM platform configuration section (default: 0)
    /// </summary>
    public int Order { get; set; } = 0;

    /// <summary>
    /// Initializes a new instance of the IMPlatformConfigAttribute class
    /// </summary>
    /// <param name="platform">The IM platform identifier (e.g., "feishu", "wecom")</param>
    public IMPlatformConfigAttribute(string platform)
    {
        Platform = platform ?? throw new ArgumentNullException(nameof(platform));
    }
}