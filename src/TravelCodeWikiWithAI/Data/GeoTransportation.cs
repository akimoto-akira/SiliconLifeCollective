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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\GeoTransportation.cs
// 迁移变更：纯数据抽象基类，原样迁移全部数据属性
//           去除 using System.Xml / TravelCodeWikiWithAI.Core
//           去除 MediaWikiTool 依赖（GetWikiDocuments() 改为自行实现）

using System.ComponentModel;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 交通设施地理位置抽象基类，表示交通设施地理位置对象 / Transportation facility geographic location abstract base class
/// 挂载在 GeoCounty 下 / Mounted under GeoCounty
/// </summary>
public abstract class GeoTransportation : GeoLocation
{
    /// <summary>
    /// 地理位置标识符 / Geographic location identifier
    /// </summary>
    public override string ID { get; set; } = string.Empty;

    /// <summary>
    /// 构造函数 / Constructor
    /// </summary>
    /// <param name="parent">父地理位置 / Parent geographic location</param>
    public GeoTransportation(GeoLocation parent) : base(parent)
    {
    }

    /// <summary>
    /// 交通设施代码 / Transportation facility code
    /// </summary>
    [Description("设施代码")]
    public string facilityCode { get; set; } = string.Empty;

    /// <summary>
    /// 交通设施名称（多语言）/ Transportation facility name (multilingual)
    /// </summary>
    [Description("设施名称")]
    public LanguageData facilityName { get; set; } = new LanguageData();

    /// <summary>
    /// 交通设施类型 / Transportation facility type
    /// </summary>
    [Description("设施类型")]
    public abstract string FacilityType { get; }

    /// <summary>
    /// 设施等级 / Facility grade
    /// </summary>
    [Description("设施等级")]
    public string facilityGrade { get; set; } = string.Empty;

    /// <summary>
    /// 设施状态（运营中/建设中/规划中/停用）/ Facility status
    /// </summary>
    [Description("设施状态")]
    public string facilityStatus { get; set; } = string.Empty;

    /// <summary>
    /// 经度 / Longitude
    /// </summary>
    [Description("经度")]
    public double longitude { get; set; }

    /// <summary>
    /// 纬度 / Latitude
    /// </summary>
    [Description("纬度")]
    public double latitude { get; set; }

    /// <summary>
    /// 海拔高度（米）/ Elevation (meters)
    /// </summary>
    [Description("海拔高度")]
    public double elevation { get; set; }

    /// <summary>
    /// 详细地址（多语言）/ Detailed address (multilingual)
    /// </summary>
    [Description("详细地址")]
    public LanguageData address { get; set; } = new LanguageData();

    /// <summary>
    /// 建设时间 / Construction date
    /// </summary>
    [Description("建设时间")]
    public string constructionDate { get; set; } = string.Empty;

    /// <summary>
    /// 开通时间 / Opening date
    /// </summary>
    [Description("开通时间")]
    public string openingDate { get; set; } = string.Empty;

    /// <summary>
    /// 运营单位 / Operating company
    /// </summary>
    [Description("运营单位")]
    public string operatingCompany { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话 / Contact phone
    /// </summary>
    [Description("联系电话")]
    public string contactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 官方网站 / Official website
    /// </summary>
    [Description("官方网站")]
    public string officialWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 年客流量 / Annual passenger volume
    /// </summary>
    [Description("年客流量")]
    public long annualPassengers { get; set; }

    /// <summary>
    /// 年货运量（吨）/ Annual freight volume (tons)
    /// </summary>
    [Description("年货运量")]
    public long annualFreight { get; set; }

    /// <summary>
    /// 服务范围（多语言）/ Service coverage (multilingual)
    /// </summary>
    [Description("服务范围")]
    public LanguageData serviceCoverage { get; set; } = new LanguageData();

    /// <summary>
    /// 设施描述（多语言）/ Facility description (multilingual)
    /// </summary>
    [Description("设施描述")]
    public LanguageData description { get; set; } = new LanguageData();

    /// <summary>
    /// 返回交通设施的字符串表示 / Return string representation of the transportation facility
    /// </summary>
    public override string[] GetPath(int depth)
    {
        return Array.Empty<string>();
    }

    public override string ToString()
    {
        return facilityName?.ToString() ?? ID;
    }

    /// <summary>
    /// 创建交通设施基本信息表格 / Create transportation facility base information table
    /// </summary>
    protected override MediaWikiTable? CreateBaseInfoTable(MediaWikiDocument doc)
    {
        MediaWikiTable table = base.CreateBaseInfoTable(doc)!;

        // 设施名称
        if (facilityName != null && facilityName.Count > 0)
        {
            AddInfoTableRow(table, "Facility Name", new MediaWikiLanguage(table) { LanguageData = facilityName });
        }

        // 设施类型
        AddInfoTableRow(table, "Facility Type", new MediaWikiNoLanguage(table) { Content = FacilityType });

        // 设施等级
        if (!string.IsNullOrEmpty(facilityGrade))
            AddInfoTableRow(table, "Grade", new MediaWikiNoLanguage(table) { Content = facilityGrade });

        // 设施状态
        if (!string.IsNullOrEmpty(facilityStatus))
            AddInfoTableRow(table, "Status", new MediaWikiNoLanguage(table) { Content = facilityStatus });

        // 坐标
        if (longitude != 0 || latitude != 0)
            AddInfoTableRow(table, "Coordinates", new MediaWikiNoLanguage(table) { Content = $"{latitude:F4}°N, {longitude:F4}°E" });

        // 海拔
        if (elevation > 0)
            AddInfoTableRow(table, "Elevation", new MediaWikiNoLanguage(table) { Content = elevation + " m" });

        // 地址
        if (address != null && address.Count > 0)
            AddInfoTableRow(table, "Address", new MediaWikiLanguage(table) { LanguageData = address });

        // 年客流量
        if (annualPassengers > 0)
            AddInfoTableRow(table, "Annual Passengers", new MediaWikiNoLanguage(table) { Content = annualPassengers.ToString("N0") });

        // 联系电话
        if (!string.IsNullOrEmpty(contactPhone))
            AddInfoTableRow(table, "Contact", new MediaWikiNoLanguage(table) { Content = contactPhone });

        // 官方网站
        if (!string.IsNullOrEmpty(officialWebsite))
        {
            MediaWikiExternalLink websiteLink = new MediaWikiExternalLink(table)
            {
                URL = new MediaWikiNoLanguage(table) { Content = officialWebsite },
                Display = new MediaWikiNoLanguage(table) { Content = officialWebsite }
            };
            AddInfoTableRow(table, "Website", websiteLink);
        }

        return table;
    }
}
