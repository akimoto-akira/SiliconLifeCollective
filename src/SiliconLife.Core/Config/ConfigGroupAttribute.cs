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
/// Attribute for grouping configuration properties
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class ConfigGroupAttribute : Attribute
{
    /// <summary>
    /// Gets the group name for the configuration property
    /// </summary>
    public string GroupName { get; }

    /// <summary>
    /// Gets or sets the display order within the group (default: 0)
    /// </summary>
    public int Order { get; set; } = 0;

    /// <summary>
    /// Gets or sets the description of this configuration property
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the display name for this property (if different from property name)
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Initializes a new instance of the ConfigGroupAttribute class
    /// </summary>
    /// <param name="groupName">The name of the configuration group</param>
    public ConfigGroupAttribute(string groupName)
    {
        GroupName = groupName ?? throw new ArgumentNullException(nameof(groupName));
    }
}
