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

namespace SiliconLife.Default.Web.Models;

public class ConfigViewModel : ViewModelBase
{
    public List<ConfigGroup> Groups { get; set; } = new();

    public List<AIClientInfo> AvailableAIClients { get; set; } = new();
}

public class ConfigGroup
{
    public string Name { get; set; } = string.Empty;
    public List<ConfigItem> Items { get; set; } = new();
}

public class ConfigItem
{
    public string PropertyName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; }
    public string PropertyType { get; set; } = "string";
    public List<string>? EnumValues { get; set; }
    public List<string>? EnumDisplayNames { get; set; }

    public string? DependsOn { get; set; }
    public string? DependsOnValue { get; set; }
}

public class AIClientInfo
{
    public string TypeName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
}
