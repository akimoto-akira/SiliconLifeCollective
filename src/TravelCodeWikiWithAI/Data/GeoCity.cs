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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\GeoCity.cs
// 迁移变更：去除 OSMapi 依赖（FlushOsmSub 改为空壳）
//           去除 MediaWikiTool 依赖（GetWikiDocuments() 改为自行实现）
//           保留全部数据属性（pop/area/postal/phone/license + counties）

using System.ComponentModel;
using System.Linq;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 地级城市地理位置类，表示地级城市地理位置对象 / Prefecture-level city geographic location class representing prefecture-level city geographic location objects
/// </summary>
public class GeoCity : GeoLocation
{
    /// <summary>
    /// 地理位置标识符 / Geographic location identifier
    /// </summary>
    public override string ID { get; set; } = string.Empty;

    /// <summary>
    /// 构造函数 / Constructor
    /// </summary>
    /// <param name="parent">父地理位置 / Parent geographic location</param>
    public GeoCity(GeoLocation parent) : base(parent)
    {
    }

    /// <summary>
    /// 基础路径 / Base path
    /// </summary>
    public override string BasePath => Parent?.FullID ?? ID;

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
    /// 邮政编码 / Postal code
    /// </summary>
    [Description("邮政编码")]
    public string postalCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话区号 / Phone area code
    /// </summary>
    [Description("电话区号")]
    public string phoneCode { get; set; } = string.Empty;

    /// <summary>
    /// 车牌代码 / License plate code
    /// </summary>
    [Description("车牌代码")]
    public string licensePlateCode { get; set; } = string.Empty;

    /// <summary>
    /// 县区列表 / County list
    /// </summary>
    [Description("县区列表")]
    public GeoList<GeoCounty>? counties { get; set; }

    public override string[] GetPath(int depth)
    {
        throw new NotImplementedException();
    }

    public override IGeoList GetSubArea()
    {
        return counties ?? new GeoList<GeoCounty>(this);
    }

    public override IGeoList GetAttractions()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 创建城市基本信息表格 / Create city base information table
    /// </summary>
    protected override MediaWikiTable? CreateBaseInfoTable(MediaWikiDocument doc)
    {
        MediaWikiTable table = base.CreateBaseInfoTable(doc)!;

        AddInfoTableRow(table, "Population", new MediaWikiNoLanguage(table) { Content = population.ToString("N0") });
        AddInfoTableRow(table, "Area", new MediaWikiNoLanguage(table) { Content = area.ToString("N0") + " km²" });

        if (!string.IsNullOrEmpty(postalCode))
            AddInfoTableRow(table, "Postal Code", new MediaWikiNoLanguage(table) { Content = postalCode });

        if (!string.IsNullOrEmpty(phoneCode))
            AddInfoTableRow(table, "Phone Code", new MediaWikiNoLanguage(table) { Content = phoneCode });

        if (!string.IsNullOrEmpty(licensePlateCode))
            AddInfoTableRow(table, "License Plate", new MediaWikiNoLanguage(table) { Content = licensePlateCode });

        return table;
    }

    /// <summary>
    /// 获取维基文档列表 / Get wiki documents list
    /// 在基类文档后追加县区列表段
    /// </summary>
    public override DocumentBase[] GetWikiDocuments(Dictionary<string, byte[]> file)
    {
        DocumentBase[] baseDocs = base.GetWikiDocuments(file);
        if (baseDocs.Length == 0) return baseDocs;

        MediaWikiDocument doc = (MediaWikiDocument)baseDocs[0];

        if (counties != null && counties.Count > 0)
        {
            LanguageData subTitle = new LanguageData();
            subTitle.SetZhHans("县区");

            MediaWikiSection subSection = new MediaWikiSection(doc)
            {
                Title = new MediaWikiLanguage(doc) { LanguageData = subTitle },
                Content = new MediaWikiChildWord(doc)
            };

            MediaWikiNoSortList subList = new MediaWikiNoSortList(subSection);
            foreach (GeoCounty child in counties)
            {
                subList.Add(CreateGeoLink(subList, child));
            }
            subSection.Content.Add(subList);
            doc.Contents.Add(subSection);
        }

        return baseDocs;
    }
}
