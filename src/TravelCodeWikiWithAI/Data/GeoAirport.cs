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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\GeoAirport.cs
// 迁移变更：纯数据类，原样迁移全部数据属性
//           去除 using System.Xml / TravelCodeWikiWithAI.Core

using System.ComponentModel;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 机场地理位置类，表示机场交通设施地理位置对象 / Airport geographic location class
/// 挂载在 GeoCounty 下 / Mounted under GeoCounty
/// </summary>
public class GeoAirport : GeoTransportation
{
    /// <summary>
    /// 构造函数 / Constructor
    /// </summary>
    /// <param name="parent">父地理位置 / Parent geographic location</param>
    public GeoAirport(GeoLocation parent) : base(parent)
    {
    }

    /// <summary>
    /// 基础路径 / Base path
    /// </summary>
    public override string BasePath => "airports";

    public override IGeoList GetSubArea()
    {
        throw new NotImplementedException();
    }

    public override IGeoList GetAttractions()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 交通设施类型 / Transportation facility type
    /// </summary>
    public override string FacilityType => "机场";

    /// <summary>
    /// 机场代码（IATA）/ Airport code (IATA)
    /// </summary>
    [Description("IATA代码")]
    public string iataCode { get; set; } = string.Empty;

    /// <summary>
    /// 机场代码（ICAO）/ Airport code (ICAO)
    /// </summary>
    [Description("ICAO代码")]
    public string icaoCode { get; set; } = string.Empty;

    /// <summary>
    /// 机场类型（国际/国内/军用/通用）/ Airport type
    /// </summary>
    [Description("机场类型")]
    public string airportType { get; set; } = string.Empty;

    /// <summary>
    /// 跑道数量 / Number of runways
    /// </summary>
    [Description("跑道数量")]
    public int runwayCount { get; set; }

    /// <summary>
    /// 航站楼数量 / Number of terminals
    /// </summary>
    [Description("航站楼数量")]
    public int terminalCount { get; set; }

    /// <summary>
    /// 年起降架次 / Annual aircraft movements
    /// </summary>
    [Description("年起降架次")]
    public long annualMovements { get; set; }

    /// <summary>
    /// 货邮吞吐量（吨）/ Cargo throughput (tons)
    /// </summary>
    [Description("货邮吞吐量")]
    public long cargoThroughput { get; set; }

    /// <summary>
    /// 占地面积（平方公里）/ Area (square kilometers)
    /// </summary>
    [Description("占地面积")]
    public double area { get; set; }

    /// <summary>
    /// 是否为枢纽机场 / Whether it is a hub airport
    /// </summary>
    [Description("是否枢纽机场")]
    public bool isHubAirport { get; set; }

    /// <summary>
    /// 航空公司基地 / Airline bases
    /// </summary>
    [Description("航空公司基地")]
    public string airlineBases { get; set; } = string.Empty;

    /// <summary>
    /// 通航城市数量 / Number of connected cities
    /// </summary>
    [Description("通航城市数量")]
    public int connectedCities { get; set; }

    /// <summary>
    /// 国际航线数量 / Number of international routes
    /// </summary>
    [Description("国际航线数量")]
    public int internationalRoutes { get; set; }

    /// <summary>
    /// 国内航线数量 / Number of domestic routes
    /// </summary>
    [Description("国内航线数量")]
    public int domesticRoutes { get; set; }

    /// <summary>
    /// 交通接驳（多语言）/ Transportation connections (multilingual)
    /// </summary>
    [Description("交通接驳")]
    public LanguageData transportationConnections { get; set; } = new LanguageData();

    /// <summary>
    /// 停车位数量 / Number of parking spaces
    /// </summary>
    [Description("停车位数量")]
    public int parkingSpaces { get; set; }

    /// <summary>
    /// 免税店数量 / Number of duty-free shops
    /// </summary>
    [Description("免税店数量")]
    public int dutyFreeShops { get; set; }

    /// <summary>
    /// 餐饮店数量 / Number of restaurants
    /// </summary>
    [Description("餐饮店数量")]
    public int restaurants { get; set; }

    /// <summary>
    /// 创建机场基本信息表格 / Create airport base information table
    /// </summary>
    protected override MediaWikiTable? CreateBaseInfoTable(MediaWikiDocument doc)
    {
        MediaWikiTable table = base.CreateBaseInfoTable(doc)!;

        // IATA 代码
        if (!string.IsNullOrEmpty(iataCode))
            AddInfoTableRow(table, "IATA Code", new MediaWikiNoLanguage(table) { Content = iataCode });

        // ICAO 代码
        if (!string.IsNullOrEmpty(icaoCode))
            AddInfoTableRow(table, "ICAO Code", new MediaWikiNoLanguage(table) { Content = icaoCode });

        // 机场类型
        if (!string.IsNullOrEmpty(airportType))
            AddInfoTableRow(table, "Airport Type", new MediaWikiNoLanguage(table) { Content = airportType });

        // 跑道数量
        if (runwayCount > 0)
            AddInfoTableRow(table, "Runways", new MediaWikiNoLanguage(table) { Content = runwayCount.ToString() });

        // 航站楼数量
        if (terminalCount > 0)
            AddInfoTableRow(table, "Terminals", new MediaWikiNoLanguage(table) { Content = terminalCount.ToString() });

        // 是否枢纽
        if (isHubAirport)
            AddInfoTableRow(table, "Hub Airport", new MediaWikiNoLanguage(table) { Content = "Yes" });

        // 面积
        if (area > 0)
            AddInfoTableRow(table, "Area", new MediaWikiNoLanguage(table) { Content = area.ToString("N0") + " km²" });

        return table;
    }
}
