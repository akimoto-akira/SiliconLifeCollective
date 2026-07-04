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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\GeoContinent.cs
// 迁移变更：去除 MediaWikiTool 依赖（GetWikiDocuments() 改为自行实现）
//           去除 GeoProject.Self 依赖
//           去除 using System.ComponentModel（仅保留 [Description]）
//           去除 FlushOsmSub 中 OSMapi/OutPut 依赖（改为空壳）
//           保留全部数据属性（8个 WordBase 内容段 + Countries 列表）

using System.ComponentModel;
using System.Linq;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 大洲地理位置类，表示地理上的大洲区域 / Continent geographic location class representing geographic continental regions
/// </summary>
[Description("大洲")]
public class GeoContinent : GeoLocation
{
    /// <summary>
    /// 地理位置标识符 / Geographic location identifier
    /// </summary>
    public override string ID { get; set; } = string.Empty;

    /// <summary>
    /// 构造函数 / Constructor
    /// </summary>
    /// <param name="parent">父地理位置（通常为世界）/ Parent geographic location (usually world)</param>
    public GeoContinent(GeoLocation parent) : base(parent)
    {
    }

    /// <summary>
    /// 基础路径，返回大洲的路径 / Base path, returns the continent's path
    /// </summary>
    public override string? BasePath => ID;

    public override string[] GetPath(int depth)
    {
        throw new NotImplementedException();
    }

    public override IGeoList GetSubArea()
    {
        return Countries;
    }

    public override IGeoList GetAttractions()
    {
        throw new NotImplementedException();
    }

    public override string FullID => ID;

    [Description("地理分区")]
    public WordBase? GeographicRegion { get; set; }

    [Description("最佳旅游时间")]
    public WordBase? BestTravelTime { get; set; }

    [Description("签证便利性")]
    public WordBase? VisaConvenience { get; set; }

    [Description("预算层次")]
    public WordBase? BudgetLevel { get; set; }

    [Description("主题旅游")]
    public WordBase? ThemeTourism { get; set; }

    [Description("跨国经典路线")]
    public WordBase? CrossCountryClassicRoutes { get; set; }

    [Description("实用建议")]
    public WordBase? PracticalAdvice { get; set; }

    [Description("主要交通枢纽")]
    public WordBase? MajorTransportationHubs { get; set; }

    [Description("国家列表")]
    public GeoList<GeoCountry>? Countries { get; set; }

    public override string GetAIPath()
    {
        if (Name != null && Name.ContainsKey("zh-cn"))
        {
            return Name["zh-cn"];
        }
        return Name?.ToString() ?? ID;
    }

    /// <summary>
    /// 创建大洲基本信息表格 / Create continent base information table
    /// </summary>
    protected override MediaWikiTable? CreateBaseInfoTable(MediaWikiDocument doc)
    {
        MediaWikiTable table = base.CreateBaseInfoTable(doc)!;

        // 国家数量
        int countryCount = Countries?.Count ?? 0;
        AddInfoTableRow(table, "Country Count", new MediaWikiNoLanguage(table) { Content = countryCount.ToString() });

        return table;
    }

    /// <summary>
    /// 获取维基文档列表 / Get wiki documents list
    /// 在基类文档后追加国家列表段
    /// </summary>
    public override DocumentBase[] GetWikiDocuments(Dictionary<string, byte[]> file)
    {
        DocumentBase[] baseDocs = base.GetWikiDocuments(file);
        if (baseDocs.Length == 0) return baseDocs;

        MediaWikiDocument doc = (MediaWikiDocument)baseDocs[0];

        // 添加国家列表段
        if (Countries != null && Countries.Count > 0)
        {
            LanguageData countryTitle = new LanguageData();
            countryTitle.SetZhHans("国家列表");

            MediaWikiSection countrySection = new MediaWikiSection(doc)
            {
                Title = new MediaWikiLanguage(doc) { LanguageData = countryTitle },
                Content = new MediaWikiChildWord(doc)
            };

            // 按 AreaType 分组
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
                    foreach (GeoCountry child in Countries)
                    {
                        if (child.AreaType == kv.Key)
                        {
                            groupList.Add(CreateGeoLink(groupList, child));
                        }
                    }
                    groupSection.Content.Add(groupList);
                    countrySection.Content.Add(groupSection);
                }
            }

            // 未分类国家
            MediaWikiNoSortList uncategorized = new MediaWikiNoSortList(countrySection);
            bool hasUncategorized = false;
            foreach (GeoCountry child in Countries)
            {
                if (SubAreaTypes == null || !SubAreaTypes.ContainsKey(child.AreaType))
                {
                    uncategorized.Add(CreateGeoLink(uncategorized, child));
                    hasUncategorized = true;
                }
            }
            if (hasUncategorized)
            {
                countrySection.Content.Add(uncategorized);
            }

            doc.Contents.Add(countrySection);
        }

        return baseDocs;
    }
}
