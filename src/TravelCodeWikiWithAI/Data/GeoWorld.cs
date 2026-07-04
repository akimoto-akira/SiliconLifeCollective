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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\GeoWorld.cs
// 迁移变更：去除 CoreTools.DataDir 依赖（GetPath() 改为空壳）
//           去除 MediaWikiTool 依赖（GetWikiDocuments() 改为自行实现）
//           保留数据属性（Continents）和层级结构

using System.Linq;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 世界地理位置类，表示世界根节点 / World geographic location class representing the world root node
/// </summary>
public class GeoWorld : GeoLocation
{
    /// <summary>
    /// 地理位置标识符，固定为 "world" / Geographic location identifier, fixed as "world"
    /// </summary>
    public override string ID { get => "world"; set { } }

    /// <summary>
    /// 大洲列表 / Continent list
    /// </summary>
    public GeoList<GeoContinent> Continents { get; set; }

    /// <summary>
    /// 构造函数 / Constructor
    /// </summary>
    public GeoWorld() : base(null)
    {
        Continents = new GeoList<GeoContinent>(this);
    }

    public override string BasePath => ID;

    public override IGeoList GetSubArea()
    {
        return Continents;
    }

    public override IGeoList GetAttractions()
    {
        return new GeoList<GeoAttraction>(this);
    }

    public override void FillInfo()
    {
        Continents.FillInfo();
    }

    /// <summary>
    /// 创建世界基本信息表格 / Create world base information table
    /// </summary>
    protected override MediaWikiTable? CreateBaseInfoTable(MediaWikiDocument doc)
    {
        MediaWikiTable table = base.CreateBaseInfoTable(doc)!;

        // 大洲数量
        int continentCount = Continents?.Count ?? 0;
        AddInfoTableRow(table, "Continent Count", new MediaWikiNoLanguage(table) { Content = continentCount.ToString() });

        return table;
    }

    /// <summary>
    /// 获取维基文档列表 / Get wiki documents list
    /// 在基类文档后追加各大洲列表段
    /// </summary>
    public override DocumentBase[] GetWikiDocuments(Dictionary<string, byte[]> file)
    {
        DocumentBase[] baseDocs = base.GetWikiDocuments(file);
        if (baseDocs.Length == 0) return baseDocs;

        MediaWikiDocument doc = (MediaWikiDocument)baseDocs[0];

        // 添加大洲列表段
        if (Continents != null && Continents.Count > 0)
        {
            LanguageData continentTitle = new LanguageData();
            continentTitle.SetZhHans("大洲");

            MediaWikiSection continentSection = new MediaWikiSection(doc)
            {
                Title = new MediaWikiLanguage(doc) { LanguageData = continentTitle },
                Content = new MediaWikiChildWord(doc)
            };

            MediaWikiNoSortList continentList = new MediaWikiNoSortList(continentSection);
            foreach (GeoContinent child in Continents)
            {
                continentList.Add(CreateGeoLink(continentList, child));
            }
            continentSection.Content.Add(continentList);
            doc.Contents.Add(continentSection);
        }

        return baseDocs;
    }
}
