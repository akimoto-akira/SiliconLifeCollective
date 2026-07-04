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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\GeoProvince.cs
// 迁移变更：去除 OSMapi/OutPut 依赖（FlushOsmSub 改为空壳）
//           去除 MediaWikiTool 依赖（GetWikiDocuments() 改为自行实现）
//           去除 using System.Xml
//           保留全部数据属性（provinceCode/capital/type/pop/area/adminLevel/postal/phone/license + sub）

using System.ComponentModel;
using System.Linq;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 省级地理位置类，表示省级地理位置对象 / Province-level geographic location class representing province-level geographic location objects
/// </summary>
public class GeoProvince : GeoLocation
{
    /// <summary>
    /// 地理位置标识符 / Geographic location identifier
    /// </summary>
    public override string ID { get; set; } = string.Empty;

    /// <summary>
    /// 构造函数 / Constructor
    /// </summary>
    /// <param name="parent">父地理位置 / Parent geographic location</param>
    public GeoProvince(GeoLocation parent) : base(parent)
    {
    }

    /// <summary>
    /// 基础路径 / Base path
    /// </summary>
    public override string BasePath => Parent?.FullID ?? ID;

    /// <summary>
    /// 省份代码 / Province code
    /// </summary>
    [Description("省份代码")]
    public string provinceCode { get; set; } = string.Empty;

    /// <summary>
    /// 省会城市 / Capital city
    /// </summary>
    [Description("省会")]
    public string capital { get; set; } = string.Empty;

    /// <summary>
    /// 省份类型（省/自治区/直辖市/特别行政区）/ Province type (Province/Autonomous Region/Municipality/SAR)
    /// </summary>
    [Description("省份类型")]
    public string provinceType { get; set; } = string.Empty;

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
    /// 行政级别（对应 adminLevel）/ Administrative level (corresponding to adminLevel)
    /// </summary>
    [Description("行政级别")]
    public int adminLevel { get; set; } = 4;

    /// <summary>
    /// 邮政编码前缀 / Postal code prefix
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
    /// 县市列表 / County/city list
    /// </summary>
    [Description("县市")]
    public GeoList<GeoLocation>? sub { get; set; }

    public override string[] GetPath(int depth)
    {
        throw new NotImplementedException();
    }

    public override IGeoList GetSubArea()
    {
        return sub ?? new GeoList<GeoLocation>(this);
    }

    public override IGeoList GetAttractions()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 创建省级基本信息表格 / Create province base information table
    /// </summary>
    protected override MediaWikiTable? CreateBaseInfoTable(MediaWikiDocument doc)
    {
        MediaWikiTable table = base.CreateBaseInfoTable(doc)!;

        // 省份代码
        if (!string.IsNullOrEmpty(provinceCode))
        {
            AddInfoTableRow(table, "Province Code", new MediaWikiNoLanguage(table) { Content = provinceCode });
        }

        // 省会
        if (!string.IsNullOrEmpty(capital))
        {
            AddInfoTableRow(table, "Capital", new MediaWikiNoLanguage(table) { Content = capital });
        }

        // 省份类型
        if (!string.IsNullOrEmpty(provinceType))
        {
            AddInfoTableRow(table, "Province Type", new MediaWikiNoLanguage(table) { Content = provinceType });
        }

        // 人口
        AddInfoTableRow(table, "Population", new MediaWikiNoLanguage(table) { Content = population.ToString("N0") });

        // 面积
        AddInfoTableRow(table, "Area", new MediaWikiNoLanguage(table) { Content = area.ToString("N0") + " km²" });

        // 邮政编码
        if (!string.IsNullOrEmpty(postalCode))
        {
            AddInfoTableRow(table, "Postal Code", new MediaWikiNoLanguage(table) { Content = postalCode });
        }

        // 电话区号
        if (!string.IsNullOrEmpty(phoneCode))
        {
            AddInfoTableRow(table, "Phone Code", new MediaWikiNoLanguage(table) { Content = phoneCode });
        }

        // 车牌代码
        if (!string.IsNullOrEmpty(licensePlateCode))
        {
            AddInfoTableRow(table, "License Plate", new MediaWikiNoLanguage(table) { Content = licensePlateCode });
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

        // 添加县市列表段
        if (sub != null && sub.Count > 0)
        {
            LanguageData subTitle = new LanguageData();
            subTitle.SetZhHans("县市");

            MediaWikiSection subSection = new MediaWikiSection(doc)
            {
                Title = new MediaWikiLanguage(doc) { LanguageData = subTitle },
                Content = new MediaWikiChildWord(doc)
            };

            MediaWikiNoSortList subList = new MediaWikiNoSortList(subSection);
            foreach (GeoLocation child in sub)
            {
                subList.Add(CreateGeoLink(subList, child));
            }
            subSection.Content.Add(subList);
            doc.Contents.Add(subSection);
        }

        return baseDocs;
    }
}
