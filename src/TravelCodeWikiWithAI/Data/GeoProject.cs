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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\GeoProject.cs
// 迁移变更：
//   去除 static Self 全局单例（改为普通实例类，由 TravelCodeWikiWithAIPlugin 持有）
//   去除构造器中 1700 行默认值初始化代码（改用 TranslationDefaults.ApplyDefaults()）
//   去除 XML 加载/保存（改用 IStorage JSON 序列化）
//   去除 CommonJSContent / CreateChild / BuildDocument / SaveXML / RemoveEmptyXML
//   去除 OllamaClient / HttpClient / OutPut / CLDRCtrl 依赖
//   保留纯数据角色：Translation/World/ExchangeRate/WordTable/APPdoc

using TravelCodeWikiWithAI.TCWTool;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 地理项目数据类，由 TravelCodeWikiWithAIPlugin 持有
/// / Geographic project data class, held by TravelCodeWikiWithAIPlugin
/// 不含 static Self，不含 XML/Ollama/HttpClient/OutPut/CLDRCtrl
/// </summary>
public class GeoProject : GeoDataBase
{
    /// <summary>
    /// 构造函数 / Constructor
    /// </summary>
    /// <param name="parent">父对象 / Parent object</param>
    public GeoProject(GeoDataBase parent) : base(parent)
    {
    }

    public GeoProject() : base(null) { }

    public override string? GetStorageKey() => "geo/root";

    /// <summary>
    /// 翻译数据 / Translation data
    /// </summary>
    public GeoTranslation? Translation { get; set; }

    /// <summary>
    /// 世界地理层级树 / World geographic hierarchy tree
    /// </summary>
    public GeoWorld? World { get; set; }

    /// <summary>
    /// 汇率数据管理 / Exchange rate data management
    /// </summary>
    public ExchangeRate? ExchangeRate { get; set; }

    /// <summary>
    /// 中-外文对照表 / Chinese-foreign language lookup table
    /// </summary>
    public GeoWordTable? WordTable { get; set; }

    /// <summary>
    /// 网站及应用程序信息列表 / Website and application information list
    /// </summary>
    public List<GeoWebApp>? APPdoc { get; set; }

    /// <summary>
    /// 获取所有可用的语言代码 / Get all available language codes
    /// </summary>
    /// <returns>语言代码数组 / Language code array</returns>
    public string[] GetAvailableLanguages()
    {
        return SysTool.GetBaseLanguage();
    }

    /// <summary>
    /// 初始化翻译默认值 / Initialize translation defaults
    /// 根据 [DefaultTranslation] 特性自动填充缺失的翻译基准文本
    /// </summary>
    public void InitializeDefaults()
    {
        if (Translation != null)
        {
            TranslationDefaults.ApplyDefaults(Translation);
        }
    }
}
