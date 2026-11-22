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

using System;
using System.Collections.Generic;

namespace SiliconLife.Collective;

/// <summary>
/// 插件设置项的类型标识。取值与宿主 Web 设置界面 ConfigItem.PropertyType 对齐。
/// </summary>
public static class PluginSettingTypes
{
    public const string String = "string";
    public const string Int = "int";
    public const string Double = "double";
    public const string Bool = "bool";
    public const string Enum = "enum";
}

/// <summary>
/// 插件设置项定义：声明一个可被宿主设置界面渲染、编辑并持久化的选项。
/// 宿主设置菜单会为每个实现 <see cref="IPluginSettings"/> 的插件渲染一个"插件"分组。
/// </summary>
public class PluginSettingDefinition
{
    /// <summary>设置项唯一键（插件内唯一；宿主保存时以 "plugin.&lt;pluginId&gt;.&lt;key&gt;" 路由）。</summary>
    public string Key { get; set; } = "";

    /// <summary>设置界面显示的名称。</summary>
    public string DisplayName { get; set; } = "";

    /// <summary>设置界面显示的说明（可空）。</summary>
    public string? Description { get; set; }

    /// <summary>设置项类型：string / int / double / bool / enum（见 <see cref="PluginSettingTypes"/>）。</summary>
    public string Type { get; set; } = PluginSettingTypes.String;

    /// <summary>默认值（"恢复默认值"按钮的数据来源；类型与 <see cref="Type"/> 一致）。</summary>
    public object? DefaultValue { get; set; }

    /// <summary>枚举选项值列表（Type 为 enum 时必填）。</summary>
    public List<string>? EnumValues { get; set; }

    /// <summary>枚举选项显示名列表（与 <see cref="EnumValues"/> 一一对应；缺省时直接用值）。</summary>
    public List<string>? EnumDisplayNames { get; set; }

    /// <summary>分组内显示顺序（升序）。</summary>
    public int Order { get; set; }
}

/// <summary>
/// 可选插件接口：声明设置选项，让主项目设置菜单直接出现并储存、更新这些设置。
///
/// 实现约定：
/// 1. 插件同时实现 <see cref="IPlugin"/>（宿主加载时即收集本接口实例）；
/// 2. 插件自行负责设置值的存储与读取（本接口不注入宿主配置对象），
///    建议用 SpeedyPack 持久化；设置界面通过
///    <see cref="GetSettingValue"/> 展示、<see cref="SetSettingValue"/> 保存、
///    <see cref="ResetSettingValue"/> 恢复默认；
/// 3. <see cref="GetSettingDefinitions"/> 返回的每一项需在
///    <see cref="GetSettingValue"/> 中可读、<see cref="SetSettingValue"/> 中可写。
/// </summary>
public interface IPluginSettings
{
    /// <summary>设置项定义列表（渲染元数据，调用时保持稳定）。</summary>
    IReadOnlyList<PluginSettingDefinition> GetSettingDefinitions();

    /// <summary>读取当前设置值；未设置时返回 null（宿主回退显示默认值）。</summary>
    object? GetSettingValue(string key);

    /// <summary>
    /// 更新设置值并持久化。value 的类型与定义 <see cref="PluginSettingDefinition.Type"/> 一致
    /// （string 传 string、int 传 int、double 传 double、bool 传 bool、enum 传字符串枚举名）。
    /// </summary>
    void SetSettingValue(string key, object? value);

    /// <summary>恢复单个设置为默认值。</summary>
    void ResetSettingValue(string key);
}
