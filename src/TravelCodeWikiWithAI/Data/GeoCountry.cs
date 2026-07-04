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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\GeoCountry.cs
// 迁移变更：CurrencySelect → string（§4.7 决策）
//           去除 MediaWikiTool 依赖（GetWikiDocuments() 改为自行实现）
//           去除 GeoProject.Self 依赖
//           去除 OSMapi/OutPut 依赖（FlushOsmSub 改为空壳）
//           去除 [MarkdownDisable]/[MarkdownParentName] WinForms 特性
//           保留全部数据属性（20+ WordBase 内容段 + SubArea 列表）

using System.ComponentModel;
using System.Linq;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 国家地理位置类，表示国家级地理位置对象 / Country geographic location class representing country-level geographic location objects
/// </summary>
[Description("国家")]
public class GeoCountry : GeoLocation
{
    /// <summary>
    /// 地理位置标识符 / Geographic location identifier
    /// </summary>
    public override string ID { get; set; } = string.Empty;

    /// <summary>
    /// 构造函数 / Constructor
    /// </summary>
    /// <param name="parent">父地理位置 / Parent geographic location</param>
    public GeoCountry(GeoLocation parent) : base(parent)
    {
    }

    /// <summary>
    /// 基础路径 / Base path
    /// </summary>
    public override string BasePath
    {
        get
        {
            if (Parent is GeoList<GeoCountry> a)
            {
                string b = a._parent.BasePath ?? "";
                return b;
            }
            else
            {
                return Parent?.FullID ?? ID;
            }
        }
    }

    /// <summary>
    /// 首都 / Capital city
    /// </summary>
    [Description("首都")]
    public GeoLocation? capital { get; set; }

    /// <summary>
    /// 人口数量 / Population
    /// </summary>
    [Description("人口")]
    public long population { get; set; }

    /// <summary>
    /// 面积（平方公里）/ Area (square kilometers)
    /// </summary>
    [Description("面积")]
    public double area { get; set; }

    /// <summary>
    /// 货币代码（ISO 4217）/ Currency code (ISO 4217)
    /// </summary>
    /// <remarks>
    /// 旧项目类型为 CurrencySelect，迁移后改为 string（§4.7 决策）
    /// Legacy type was CurrencySelect; migrated to string (Decision §4.7)
    /// </remarks>
    [Description("货币代码")]
    public string currencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 官方语言 / Official languages
    /// </summary>
    [Description("官方语言")]
    public WordBase? officialLanguages { get; set; }

    /// <summary>
    /// 时区 / Time zone
    /// </summary>
    [Description("时区")]
    public WordBase? timeZone { get; set; }

    /// <summary>
    /// 国际长途区号 / International dialing code
    /// </summary>
    [Description("国际长途区号")]
    public string dialingCode { get; set; } = string.Empty;

    /// <summary>
    /// 互联网根域名 / Internet top-level domain
    /// </summary>
    [Description("互联网根域名")]
    public string topLevelDomain { get; set; } = string.Empty;

    /// <summary>
    /// 官方媒体 / Official media
    /// </summary>
    [Description("官方媒体")]
    public WordBase? officialMedia { get; set; }

    /// <summary>
    /// 衣 / Clothing
    /// </summary>
    [Description("衣")]
    public WordBase? clothing { get; set; }

    /// <summary>
    /// 食 / Food
    /// </summary>
    [Description("食")]
    public WordBase? food { get; set; }

    /// <summary>
    /// 住 / Accommodation
    /// </summary>
    [Description("住")]
    public WordBase? accommodation { get; set; }

    /// <summary>
    /// 行 / Transportation
    /// </summary>
    [Description("行")]
    public WordBase? transportation { get; set; }

    /// <summary>
    /// 签证信息 / Visa information
    /// </summary>
    [Description("签证类型")]
    public WordBase? visaInfo { get; set; }

    /// <summary>
    /// 旅游体验 / Tourism experience
    /// </summary>
    [Description("旅游体验")]
    public WordBase? tourismExperience { get; set; }

    /// <summary>
    /// 安全健康 / Safety and health
    /// </summary>
    [Description("安全健康")]
    public WordBase? safetyHealth { get; set; }

    /// <summary>
    /// 货币与支付 / Currency and payment
    /// </summary>
    [Description("货币与支付")]
    public WordBase? currencyPayment { get; set; }

    /// <summary>
    /// 文化礼仪 / Culture and etiquette
    /// </summary>
    [Description("文化礼仪")]
    public WordBase? cultureEtiquette { get; set; }

    /// <summary>
    /// 网络与通信 / Internet and communication
    /// </summary>
    [Description("网络和通信")]
    public WordBase? internetCommunication { get; set; }

    /// <summary>
    /// 地理 / Geography
    /// </summary>
    [Description("地理")]
    public WordBase? geography { get; set; }

    /// <summary>
    /// 气候 / Climate
    /// </summary>
    [Description("气候")]
    public WordBase? climate { get; set; }

    /// <summary>
    /// 生态 / Ecology
    /// </summary>
    [Description("生态")]
    public WordBase? ecology { get; set; }

    /// <summary>
    /// 历史 / History
    /// </summary>
    [Description("历史")]
    public WordBase? history { get; set; }

    /// <summary>
    /// 宗教 / Religion
    /// </summary>
    [Description("宗教")]
    public WordBase? religion { get; set; }

    /// <summary>
    /// 自然资源 / Natural resources
    /// </summary>
    [Description("自然资源")]
    public WordBase? naturalResources { get; set; }

    /// <summary>
    /// 紧急电话 / Emergency phone numbers
    /// </summary>
    [Description("紧急电话")]
    public WordBase? emergencyPhone { get; set; }

    /// <summary>
    /// 最佳旅游季节 / Best travel season
    /// </summary>
    [Description("最佳旅游季节")]
    public WordBase? bestTravelSeason { get; set; }

    /// <summary>
    /// 国旗 / Flag
    /// </summary>
    [Description("国旗")]
    public WordBase? flag { get; set; }

    /// <summary>
    /// 子区域列表（省/直辖市/自治区/特别行政区）/ Sub-area list (provinces/municipalities/autonomous regions/SARs)
    /// </summary>
    public GeoList<GeoLocation>? SubArea { get; set; }

    public override string[] GetPath(int depth)
    {
        return new string[0];
    }

    public override IGeoList GetSubArea()
    {
        return SubArea ?? new GeoList<GeoLocation>(this);
    }

    public override bool DebugNeedChild()
    {
        return false;
    }

    public override IGeoList GetAttractions()
    {
        throw new NotImplementedException();
    }

    public override string GetAIPath()
    {
        if (Name != null && Name.ContainsKey("zh-cn"))
        {
            return Name["zh-cn"];
        }
        else if (Name != null && Name.ContainsKey("zh"))
        {
            return Name["zh"];
        }
        else
        {
            return Name?.ToString() ?? ID;
        }
    }

    /// <summary>
    /// 创建国家基本信息表格 / Create country base information table
    /// 包含：首都、人口、面积、货币、区号、域名
    /// </summary>
    protected override MediaWikiTable? CreateBaseInfoTable(MediaWikiDocument doc)
    {
        MediaWikiTable table = base.CreateBaseInfoTable(doc)!;

        // 全名
        if (FullName != null)
        {
            AddInfoTableRow(table, "Official Name", FullName);
        }

        // 首都
        if (capital != null)
        {
            AddInfoTableRow(table, "Capital", capital.Name != null
                ? new MediaWikiLanguage(table) { LanguageData = capital.Name }
                : null);
        }

        // 国旗
        if (flag != null)
        {
            AddInfoTableRow(table, "Flag", flag);
        }

        // 人口
        AddInfoTableRow(table, "Population", new MediaWikiNoLanguage(table) { Content = population.ToString("N0") });

        // 面积
        AddInfoTableRow(table, "Area", new MediaWikiNoLanguage(table) { Content = area.ToString("N0") + " km²" });

        // 货币
        if (!string.IsNullOrEmpty(currencyCode))
        {
            AddInfoTableRow(table, "Currency", new MediaWikiNoLanguage(table) { Content = currencyCode });
        }

        // 官方语言
        if (officialLanguages != null)
        {
            AddInfoTableRow(table, "Official Languages", officialLanguages);
        }

        // 国际区号
        if (!string.IsNullOrEmpty(dialingCode))
        {
            AddInfoTableRow(table, "Dialing Code", new MediaWikiNoLanguage(table) { Content = "+" + dialingCode });
        }

        // 互联网域名
        if (!string.IsNullOrEmpty(topLevelDomain))
        {
            AddInfoTableRow(table, "Top Level Domain", new MediaWikiNoLanguage(table) { Content = "." + topLevelDomain });
        }

        return table;
    }

    /// <summary>
    /// 获取维基文档列表 / Get wiki documents list
    /// 在基类文档后追加子区域列表段
    /// </summary>
    public override DocumentBase[] GetWikiDocuments(Dictionary<string, byte[]> file)
    {
        DocumentBase[] baseDocs = base.GetWikiDocuments(file);
        if (baseDocs.Length == 0) return baseDocs;

        MediaWikiDocument doc = (MediaWikiDocument)baseDocs[0];

        // 添加子区域列表段
        if (SubArea != null && SubArea.Count > 0)
        {
            LanguageData subAreaTitle = new LanguageData();
            subAreaTitle.SetZhHans("子区域");

            MediaWikiSection subSection = new MediaWikiSection(doc)
            {
                Title = new MediaWikiLanguage(doc) { LanguageData = subAreaTitle },
                Content = new MediaWikiChildWord(doc)
            };

            // 按 AreaType 分组展示
            if (SubAreaTypes != null)
            {
                foreach (KeyValuePair<string, string> kv in SubAreaTypes)
                {
                    LanguageData groupTitle = new LanguageData();
                    groupTitle.SetZhHans(kv.Value);

                    MediaWikiSection groupSection = new MediaWikiSection(doc)
                    {
                        Title = new MediaWikiLanguage(doc) { LanguageData = groupTitle },
                        Content = new MediaWikiChildWord(doc)
                    };

                    MediaWikiNoSortList groupList = new MediaWikiNoSortList(groupSection);
                    foreach (GeoLocation child in SubArea)
                    {
                        if (child.AreaType == kv.Key)
                        {
                            groupList.Add(CreateGeoLink(groupList, child));
                        }
                    }
                    groupSection.Content.Add(groupList);
                    subSection.Content.Add(groupSection);
                }
            }

            // 未分类子区域
            MediaWikiNoSortList uncategorized = new MediaWikiNoSortList(subSection);
            bool hasUncategorized = false;
            foreach (GeoLocation child in SubArea)
            {
                if (SubAreaTypes == null || !SubAreaTypes.ContainsKey(child.AreaType))
                {
                    uncategorized.Add(CreateGeoLink(uncategorized, child));
                    hasUncategorized = true;
                }
            }
            if (hasUncategorized)
            {
                subSection.Content.Add(uncategorized);
            }

            doc.Contents.Add(subSection);
        }

        return baseDocs;
    }
}
