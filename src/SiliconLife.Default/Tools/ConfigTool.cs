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

using System.Reflection;
using System.Text;
using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Configuration reading tool. Only available to the Silicon Curator (main administrator).
/// Reads the current runtime configuration via Config.Instance.
/// </summary>
[SiliconManagerOnly]
public class ConfigTool : ITool
{
    public string Name => "config";

    public string Description =>
        "Read the Silicon Life system configuration (curator only). " +
        "Actions: 'get_all' (all configuration fields), " +
        "'get_group' (fields in a specific group), " +
        "'get_field' (a single field by name), " +
        "'get_enum_values' (all possible values for an enum field).";

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
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The action to perform: get_all, get_group, get_field",
                    ["enum"] = new[] { "get_all", "get_group", "get_field" }
                },
                ["group"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Config group name (for get_group action, e.g. 'Basic', 'Runtime', 'User')"
                },
                ["field"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Config field name (for get_field action)"
                },
                ["enum_field"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Enum field name to get available values (for get_enum_values action)"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj))
            return ToolResult.Failed("Missing 'action' parameter");

        ConfigDataBase? data = Config.Instance?.Data;
        if (data == null)
            return ToolResult.Failed("Configuration is not initialized");

        string action = actionObj?.ToString() ?? "";

        return action.ToLowerInvariant() switch
        {
            "get_all"       => ExecuteGetAll(data),
            "get_group"     => ExecuteGetGroup(data, parameters),
            "get_field"     => ExecuteGetField(data, parameters),
            "get_enum_values" => ExecuteGetEnumValues(data, parameters),
            _               => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    private static ToolResult ExecuteGetAll(ConfigDataBase data)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Config file: {data.GetConfigPath()}");
        sb.AppendLine();

        foreach ((string group, List<(string name, object? value, string desc)> fields) in ReadGroups(data))
        {
            sb.AppendLine($"[{group}]");
            foreach (var (name, value, desc) in fields)
                AppendField(sb, name, value, desc);
            sb.AppendLine();
        }

        return ToolResult.Successful(sb.ToString().TrimEnd());
    }

    private static ToolResult ExecuteGetGroup(ConfigDataBase data, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("group", out object? groupObj) || string.IsNullOrWhiteSpace(groupObj?.ToString()))
            return ToolResult.Failed("Missing 'group' parameter for get_group");

        string targetGroup = groupObj.ToString()!;
        var groups = ReadGroups(data);

        var match = groups.FirstOrDefault(g =>
            string.Equals(g.group, targetGroup, StringComparison.OrdinalIgnoreCase));

        if (match.fields == null)
            return ToolResult.Failed($"Group '{targetGroup}' not found");

        var sb = new StringBuilder();
        sb.AppendLine($"[{match.group}]");
        foreach (var (name, value, desc) in match.fields)
            AppendField(sb, name, value, desc);

        return ToolResult.Successful(sb.ToString().TrimEnd());
    }

    private static ToolResult ExecuteGetField(ConfigDataBase data, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("field", out object? fieldObj) || string.IsNullOrWhiteSpace(fieldObj?.ToString()))
            return ToolResult.Failed("Missing 'field' parameter for get_field");

        string fieldName = fieldObj.ToString()!;
        PropertyInfo? prop = data.GetType().GetProperty(fieldName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        if (prop == null)
            return ToolResult.Failed($"Field '{fieldName}' not found");

        // Respect [ConfigIgnore]
        if (prop.GetCustomAttribute<ConfigIgnoreAttribute>() != null)
            return ToolResult.Failed($"Field '{fieldName}' is marked as internal and cannot be read");

        object? value = prop.GetValue(data);
        string desc = prop.GetCustomAttribute<ConfigGroupAttribute>()?.DescriptionKey ?? "";
        var sb = new StringBuilder();
        AppendField(sb, prop.Name, value, desc);

        return ToolResult.Successful(sb.ToString().TrimEnd());
    }

    private static ToolResult ExecuteGetEnumValues(ConfigDataBase data, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("enum_field", out object? fieldObj) || string.IsNullOrWhiteSpace(fieldObj?.ToString()))
            return ToolResult.Failed("Missing 'enum_field' parameter for get_enum_values");

        string fieldName = fieldObj.ToString()!;
        PropertyInfo? prop = data.GetType().GetProperty(fieldName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        if (prop == null)
            return ToolResult.Failed($"Field '{fieldName}' not found");

        // Respect [ConfigIgnore]
        if (prop.GetCustomAttribute<ConfigIgnoreAttribute>() != null)
            return ToolResult.Failed($"Field '{fieldName}' is marked as internal and cannot be read");

        Type propType = prop.PropertyType;
        
        // Handle nullable types
        if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            propType = Nullable.GetUnderlyingType(propType)!;
        }

        if (!propType.IsEnum)
            return ToolResult.Failed($"Field '{fieldName}' is not an enum type (it is {propType.Name})");

        var sb = new StringBuilder();
        sb.AppendLine($"Enum field: {prop.Name} (Type: {propType.Name})");
        sb.AppendLine($"Current value: {prop.GetValue(data)}");
        sb.AppendLine();
        sb.AppendLine("Available values:");

        foreach (object value in Enum.GetValues(propType))
        {
            string enumName = value.ToString()!;
            int intValue = Convert.ToInt32(value);
            sb.AppendLine($"  {enumName} = {intValue}");
        }

        return ToolResult.Successful(sb.ToString().TrimEnd());
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static List<(string group, List<(string name, object? value, string desc)> fields)> ReadGroups(
        ConfigDataBase data)
    {
        var grouped = new Dictionary<string, List<(string, object?, string)>>(StringComparer.OrdinalIgnoreCase);
        var ungrouped = new List<(string, object?, string)>();

        foreach (PropertyInfo prop in data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            // Skip [ConfigIgnore] properties
            if (prop.GetCustomAttribute<ConfigIgnoreAttribute>() != null)
                continue;

            object? value = prop.GetValue(data);
            ConfigGroupAttribute? groupAttr = prop.GetCustomAttribute<ConfigGroupAttribute>();

            if (groupAttr != null)
            {
                if (!grouped.TryGetValue(groupAttr.GroupKey, out var list))
                {
                    list = new List<(string, object?, string)>();
                    grouped[groupAttr.GroupKey] = list;
                }
                list.Add((prop.Name, value, groupAttr.DescriptionKey ?? ""));
            }
            else
            {
                ungrouped.Add((prop.Name, value, ""));
            }
        }

        var result = grouped
            .Select(kv => (kv.Key, kv.Value))
            .ToList();

        if (ungrouped.Count > 0)
            result.Add(("Other", ungrouped));

        return result;
    }

    private static void AppendField(StringBuilder sb, string name, object? value, string desc)
    {
        string display = value switch
        {
            null              => "(null)",
            DirectoryInfo di  => di.FullName,
            _                 => value.ToString() ?? "(null)"
        };

        if (!string.IsNullOrEmpty(desc))
            sb.AppendLine($"  {name} = {display}  # {desc}");
        else
            sb.AppendLine($"  {name} = {display}");
    }
}
