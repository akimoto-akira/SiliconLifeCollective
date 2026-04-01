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
/// Abstract base class for configuration data
/// </summary>
public abstract class ConfigDataBase
{
    /// <summary>
    /// Gets or sets the configuration type identifier for polymorphic deserialization
    /// </summary>
    public abstract string ConfigType { get; set; }

    /// <summary>
    /// Gets or sets the data directory path for storing all application data
    /// </summary>
    public abstract string DataDirectory { get; set; }

    /// <summary>
    /// Gets or sets the GUID of the curator (main administrator)
    /// </summary>
    public abstract Guid CuratorGuid { get; set; }

    /// <summary>
    /// Gets or sets the language setting for the application
    /// </summary>
    public abstract Language Language { get; set; }

    /// <summary>
    /// Gets or sets the timeout duration for each tick execution
    /// </summary>
    public abstract TimeSpan TickTimeout { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of consecutive timeouts allowed before circuit breaker triggers
    /// </summary>
    public abstract int MaxTimeoutCount { get; set; }
}
