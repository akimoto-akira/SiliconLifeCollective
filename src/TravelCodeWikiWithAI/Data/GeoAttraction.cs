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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\GeoAttraction.cs
// 迁移变更：纯数据类，无副作用方法，原样迁移全部数据属性
//           去除 using System.Xml / TravelCodeWikiWithAI.Core

using System.ComponentModel;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 景点地理位置类，表示旅游景点地理位置对象 / Attraction geographic location class representing tourist attraction geographic location objects
/// 挂载在 GeoCounty 下 / Mounted under GeoCounty
/// </summary>
public class GeoAttraction : GeoLocation
{
    /// <summary>
    /// 地理位置标识符 / Geographic location identifier
    /// </summary>
    public override string ID { get; set; } = string.Empty;

    /// <summary>
    /// 构造函数 / Constructor
    /// </summary>
    /// <param name="parent">父地理位置 / Parent geographic location</param>
    public GeoAttraction(GeoLocation parent) : base(parent)
    {
    }

    /// <summary>
    /// 基础路径 / Base path
    /// </summary>
    public override string BasePath => "attractions";

    /// <summary>
    /// 景点代码 / Attraction code
    /// </summary>
    [Description("景点代码")]
    public string attractionCode { get; set; } = string.Empty;

    /// <summary>
    /// 景点名称（多语言）/ Attraction name (multilingual)
    /// </summary>
    [Description("景点名称")]
    public LanguageData attractionName { get; set; } = new LanguageData();

    /// <summary>
    /// 景点类型（自然景观/人文景观/主题公园/博物馆/寺庙/古迹等）/ Attraction type
    /// </summary>
    [Description("景点类型")]
    public string attractionType { get; set; } = string.Empty;

    /// <summary>
    /// 景点等级（5A/4A/3A/2A/1A）/ Attraction rating
    /// </summary>
    [Description("景点等级")]
    public string attractionRating { get; set; } = string.Empty;

    /// <summary>
    /// 景点简介（多语言）/ Attraction description (multilingual)
    /// </summary>
    [Description("景点简介")]
    public LanguageData description { get; set; } = new LanguageData();

    /// <summary>
    /// 开放时间 / Opening hours
    /// </summary>
    [Description("开放时间")]
    public string openingHours { get; set; } = string.Empty;

    /// <summary>
    /// 门票价格（元）/ Ticket price (yuan)
    /// </summary>
    [Description("门票价格")]
    public decimal ticketPrice { get; set; }

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
    /// 详细地址（多语言）/ Detailed address (multilingual)
    /// </summary>
    [Description("详细地址")]
    public LanguageData address { get; set; } = new LanguageData();

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
    /// 占地面积（平方公里）/ Area (square kilometers)
    /// </summary>
    [Description("占地面积")]
    public double area { get; set; }

    /// <summary>
    /// 年游客量 / Annual visitor count
    /// </summary>
    [Description("年游客量")]
    public long annualVisitors { get; set; }

    /// <summary>
    /// 建立时间 / Establishment date
    /// </summary>
    [Description("建立时间")]
    public string establishmentDate { get; set; } = string.Empty;

    /// <summary>
    /// 最佳游览季节 / Best visiting season
    /// </summary>
    [Description("最佳游览季节")]
    public string bestSeason { get; set; } = string.Empty;

    /// <summary>
    /// 建议游览时长（小时）/ Recommended visit duration (hours)
    /// </summary>
    [Description("建议游览时长")]
    public double recommendedDuration { get; set; }

    /// <summary>
    /// 是否为世界遗产 / Whether it is a World Heritage Site
    /// </summary>
    [Description("是否世界遗产")]
    public bool isWorldHeritage { get; set; }

    /// <summary>
    /// 是否为国家重点文物保护单位 / Whether it is a National Key Cultural Relic Protection Unit
    /// </summary>
    [Description("是否国家重点文物")]
    public bool isNationalCulturalRelic { get; set; }

    /// <summary>
    /// 交通方式（多语言）/ Transportation (multilingual)
    /// </summary>
    [Description("交通方式")]
    public LanguageData transportation { get; set; } = new LanguageData();

    /// <summary>
    /// 周边设施（多语言）/ Nearby facilities (multilingual)
    /// </summary>
    [Description("周边设施")]
    public LanguageData nearbyFacilities { get; set; } = new LanguageData();

    /// <summary>
    /// 特色活动（多语言）/ Special activities (multilingual)
    /// </summary>
    [Description("特色活动")]
    public LanguageData specialActivities { get; set; } = new LanguageData();

    /// <summary>
    /// 返回景点的字符串表示 / Return string representation of the attraction
    /// </summary>
    public override string[] GetPath(int depth)
    {
        return Array.Empty<string>();
    }

    public override string ToString()
    {
        return attractionName?.ToString() ?? ID;
    }

    public override IGeoList GetSubArea()
    {
        throw new NotImplementedException();
    }

    public override IGeoList GetAttractions()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 创建景点基本信息表格 / Create attraction base information table
    /// </summary>
    protected override MediaWikiTable? CreateBaseInfoTable(MediaWikiDocument doc)
    {
        MediaWikiTable table = base.CreateBaseInfoTable(doc)!;

        // 景点名称（如果有独立于 Name 的 attractionName）
        if (attractionName != null && attractionName.Count > 0)
        {
            AddInfoTableRow(table, "Attraction Name", new MediaWikiLanguage(table) { LanguageData = attractionName });
        }

        // 景点类型
        if (!string.IsNullOrEmpty(attractionType))
            AddInfoTableRow(table, "Type", new MediaWikiNoLanguage(table) { Content = attractionType });

        // 景点等级
        if (!string.IsNullOrEmpty(attractionRating))
            AddInfoTableRow(table, "Rating", new MediaWikiNoLanguage(table) { Content = attractionRating });

        // 门票价格
        if (ticketPrice > 0)
            AddInfoTableRow(table, "Ticket Price", new MediaWikiNoLanguage(table) { Content = "¥" + ticketPrice });

        // 开放时间
        if (!string.IsNullOrEmpty(openingHours))
            AddInfoTableRow(table, "Opening Hours", new MediaWikiNoLanguage(table) { Content = openingHours });

        // 地址
        if (address != null && address.Count > 0)
            AddInfoTableRow(table, "Address", new MediaWikiLanguage(table) { LanguageData = address });

        // 坐标
        if (longitude != 0 || latitude != 0)
            AddInfoTableRow(table, "Coordinates", new MediaWikiNoLanguage(table) { Content = $"{latitude:F4}°N, {longitude:F4}°E" });

        // 海拔
        if (elevation > 0)
            AddInfoTableRow(table, "Elevation", new MediaWikiNoLanguage(table) { Content = elevation + " m" });

        // 面积
        if (area > 0)
            AddInfoTableRow(table, "Area", new MediaWikiNoLanguage(table) { Content = area.ToString("N0") + " km²" });

        // 世界遗产
        if (isWorldHeritage)
            AddInfoTableRow(table, "World Heritage", new MediaWikiNoLanguage(table) { Content = "Yes" });

        // 国家重点文物
        if (isNationalCulturalRelic)
            AddInfoTableRow(table, "National Cultural Relic", new MediaWikiNoLanguage(table) { Content = "Yes" });

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
