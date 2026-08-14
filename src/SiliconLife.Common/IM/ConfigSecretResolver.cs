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

using System.Text.Json;
using System.Text.RegularExpressions;
using SiliconLife.Collective;

namespace SiliconLife.Common.IM;

/// <summary>
/// 解析 IM 平台配置中的 <c>${ENV_VAR}</c> 环境变量占位符。
/// 通过深拷贝副本在内存中替换 <see cref="IMPlatformConfig.Config"/> 的字符串值，
/// 原始配置对象保持占位符原样，绝不将解析后的明文写回 config.json。
/// 占位符对应的环境变量存在时替换为其值，不存在时保持原值不变。
/// </summary>
public static class ConfigSecretResolver
{
    /// <summary>
    /// 匹配 ${ENV_VAR} 占位符：变量名以字母或下划线开头，后跟字母、数字或下划线。
    /// 支持整值占位符（如 "${APP_SECRET}"）与值内嵌占位符（如 "prefix-${VAR}"）。
    /// </summary>
    private static readonly Regex PlaceholderRegex = new(
        @"\$\{([A-Za-z_][A-Za-z0-9_]*)\}", RegexOptions.Compiled);

    /// <summary>
    /// 深拷贝平台配置列表与各自的 Config 字典，并在副本上解析字符串值内的
    /// 环境变量占位符；原对象不做任何修改，保证后续 SaveConfig 落盘的仍是占位符。
    /// 值可能是 <see cref="string"/>，也可能是 JSON 反序列化产生的
    /// <see cref="JsonElement"/>（Kind 为 String），两种类型均处理。
    /// </summary>
    /// <param name="platforms">IM 平台配置列表，允许为 null 或空。</param>
    /// <returns>解析后的深拷贝列表；入参为 null 或空时返回空列表。</returns>
    public static List<IMPlatformConfig> CreateResolvedCopy(List<IMPlatformConfig>? platforms)
    {
        var result = new List<IMPlatformConfig>();
        if (platforms == null || platforms.Count == 0)
        {
            return result;
        }

        foreach (IMPlatformConfig platform in platforms)
        {
            if (platform == null)
            {
                continue;
            }

            var copy = new IMPlatformConfig
            {
                Platform = platform.Platform,
                Enabled = platform.Enabled,
                Config = new Dictionary<string, object>(),
            };

            if (platform.Config != null)
            {
                foreach (var pair in platform.Config)
                {
                    string? raw = ExtractStringValue(pair.Value);
                    copy.Config[pair.Key] = raw != null ? ResolveValue(raw) : pair.Value;
                }
            }

            result.Add(copy);
        }

        return result;
    }

    /// <summary>
    /// 从配置值中提取字符串：直接的 string，或 Kind 为 String 的 JsonElement；
    /// 其他类型返回 null（不处理）。
    /// </summary>
    private static string? ExtractStringValue(object? value)
    {
        return value switch
        {
            string s => s,
            JsonElement { ValueKind: JsonValueKind.String } element => element.GetString(),
            _ => null,
        };
    }

    /// <summary>
    /// 替换字符串中的所有 ${ENV_VAR} 占位符；环境变量不存在时保留原占位符文本。
    /// </summary>
    private static string ResolveValue(string raw)
    {
        return PlaceholderRegex.Replace(raw, match =>
        {
            string? envValue = Environment.GetEnvironmentVariable(match.Groups[1].Value);
            return envValue ?? match.Value;
        });
    }
}
