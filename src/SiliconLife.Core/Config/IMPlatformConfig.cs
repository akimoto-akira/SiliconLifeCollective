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
/// Represents the configuration for a single IM platform instance.
/// Used by <see cref="ConfigDataBase.IMPlatforms"/> to support multiple IM platforms simultaneously.
/// </summary>
public class IMPlatformConfig
{
    /// <summary>
    /// Gets or sets the IM platform identifier (e.g., "webui", "feishu", "wecom", "dingtalk").
    /// </summary>
    public string Platform { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether this IM platform is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the platform-specific configuration dictionary.
    /// Contains keys like appId, appSecret, verificationToken, etc. depending on the platform.
    /// </summary>
    public Dictionary<string, object> Config { get; set; } = new();
}
