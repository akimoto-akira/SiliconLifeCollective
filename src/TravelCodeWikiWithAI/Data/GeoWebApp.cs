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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\GeoWebApp.cs
// 迁移变更：去除 [Category]/[XmlSerialization] WinForms 特性
//           保留 [Description] 和 LanguageData 属性
//           去除 BuildDocument/CheckParent/AutoSetLanguage stub

using System.ComponentModel;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 网站及应用程序信息类 / Website and Application Information Class
/// </summary>
public class GeoWebApp : GeoDataBase
{
    /// <summary>
    /// 构造函数 / Constructor
    /// </summary>
    /// <param name="parent">父对象 / Parent object</param>
    public GeoWebApp(GeoDataBase parent) : base(parent)
    {
    }

    /// <summary>
    /// 无参构造函数 / Parameterless Constructor
    /// </summary>
    public GeoWebApp() : base(null) { }

    /// <summary>
    /// 名称（多语言） / Name (Multi-language)
    /// </summary>
    [Description("网站或应用的名称")]
    public LanguageData? Name { get; set; }

    /// <summary>
    /// 描述（多语言） / Description (Multi-language)
    /// </summary>
    [Description("网站或应用的详细描述")]
    public LanguageData? Description { get; set; }

    /// <summary>
    /// 分类（多语言） / Category (Multi-language)
    /// </summary>
    [Description("网站或应用的分类，如：社交、购物、旅游等")]
    public LanguageData? Category { get; set; }

    /// <summary>
    /// 官方网站URL（多语言） / Official Website URL (Multi-language)
    /// </summary>
    [Description("官方网站地址，不同语言可能有不同的网站链接")]
    public LanguageData? WebsiteUrl { get; set; }

    /// <summary>
    /// 是否免费 / Is Free
    /// </summary>
    [Description("是否为免费服务")]
    public bool IsFree { get; set; } = true;

    /// <summary>
    /// 是否需要注册 / Requires Registration
    /// </summary>
    [Description("是否需要注册账号")]
    public bool RequiresRegistration { get; set; }

    /// <summary>
    /// 支持的语言列表 / Supported Languages List
    /// </summary>
    [Description("支持的语言代码列表，如：zh-CN, en-US, ja-JP")]
    public List<string> SupportedLanguages { get; set; } = new List<string>();

    /// <summary>
    /// 支持的国家或地区 / Supported Countries or Regions
    /// </summary>
    [Description("支持的国家或地区代码列表")]
    public List<string> SupportedRegions { get; set; } = new List<string>();

    /// <summary>
    /// 开发商（多语言） / Developer (Multi-language)
    /// </summary>
    [Description("开发商或公司名称，不同语言可能有不同的名称")]
    public LanguageData? Developer { get; set; }

    /// <summary>
    /// 内容 / Content
    /// </summary>
    [Description("内容")]
    public WordBaseWithChild? Content { get; set; }
}
