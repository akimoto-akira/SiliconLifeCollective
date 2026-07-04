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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\GeoPort.cs
// 迁移变更：纯数据类，原样迁移全部数据属性
//           去除 using System.Xml / TravelCodeWikiWithAI.Core

using System.ComponentModel;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 港口地理位置类，表示港口交通设施地理位置对象 / Port geographic location class
/// 挂载在 GeoCounty 下 / Mounted under GeoCounty
/// </summary>
public class GeoPort : GeoTransportation
{
    /// <summary>
    /// 构造函数 / Constructor
    /// </summary>
    /// <param name="parent">父地理位置 / Parent geographic location</param>
    public GeoPort(GeoLocation parent) : base(parent)
    {
    }

    /// <summary>
    /// 基础路径 / Base path
    /// </summary>
    public override string BasePath => "ports";

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
    public override string FacilityType => "港口";

    /// <summary>
    /// 港口代码 / Port code
    /// </summary>
    [Description("港口代码")]
    public string portCode { get; set; } = string.Empty;

    /// <summary>
    /// 港口类型（海港/河港/湖港）/ Port type
    /// </summary>
    [Description("港口类型")]
    public string portType { get; set; } = string.Empty;

    /// <summary>
    /// 港口性质（客运港/货运港/综合港/渔港/军港）/ Port nature
    /// </summary>
    [Description("港口性质")]
    public string portNature { get; set; } = string.Empty;

    /// <summary>
    /// 泊位数量 / Number of berths
    /// </summary>
    [Description("泊位数量")]
    public int berthCount { get; set; }

    /// <summary>
    /// 深水泊位数量 / Number of deep-water berths
    /// </summary>
    [Description("深水泊位数量")]
    public int deepWaterBerths { get; set; }

    /// <summary>
    /// 最大靠泊吨位 / Maximum berthing tonnage
    /// </summary>
    [Description("最大靠泊吨位")]
    public long maxBerthingTonnage { get; set; }

    /// <summary>
    /// 港区面积（平方公里）/ Port area (square kilometers)
    /// </summary>
    [Description("港区面积")]
    public double portArea { get; set; }

    /// <summary>
    /// 水域面积（平方公里）/ Water area (square kilometers)
    /// </summary>
    [Description("水域面积")]
    public double waterArea { get; set; }

    /// <summary>
    /// 陆域面积（平方公里）/ Land area (square kilometers)
    /// </summary>
    [Description("陆域面积")]
    public double landArea { get; set; }

    /// <summary>
    /// 年货物吞吐量（万吨）/ Annual cargo throughput (10,000 tons)
    /// </summary>
    [Description("年货物吞吐量")]
    public long annualCargoThroughput { get; set; }

    /// <summary>
    /// 年集装箱吞吐量（TEU）/ Annual container throughput (TEU)
    /// </summary>
    [Description("年集装箱吞吐量")]
    public long annualContainerThroughput { get; set; }

    /// <summary>
    /// 主要货种（多语言）/ Main cargo types (multilingual)
    /// </summary>
    [Description("主要货种")]
    public LanguageData mainCargoTypes { get; set; } = new LanguageData();

    /// <summary>
    /// 通航城市（多语言）/ Connected cities (multilingual)
    /// </summary>
    [Description("通航城市")]
    public LanguageData connectedCities { get; set; } = new LanguageData();

    /// <summary>
    /// 是否为自由贸易港 / Whether it is a free trade port
    /// </summary>
    [Description("是否自贸港")]
    public bool isFreeTradePort { get; set; }

    /// <summary>
    /// 是否为国际港口 / Whether it is an international port
    /// </summary>
    [Description("是否国际港口")]
    public bool isInternationalPort { get; set; }

    /// <summary>
    /// 海关等级 / Customs grade
    /// </summary>
    [Description("海关等级")]
    public string customsGrade { get; set; } = string.Empty;

    /// <summary>
    /// 航道水深（米）/ Channel depth (meters)
    /// </summary>
    [Description("航道水深")]
    public double channelDepth { get; set; }

    /// <summary>
    /// 潮差（米）/ Tidal range (meters)
    /// </summary>
    [Description("潮差")]
    public double tidalRange { get; set; }

    /// <summary>
    /// 仓储面积（平方米）/ Storage area (square meters)
    /// </summary>
    [Description("仓储面积")]
    public double storageArea { get; set; }

    /// <summary>
    /// 起重设备数量 / Number of cranes
    /// </summary>
    [Description("起重设备数量")]
    public int craneCount { get; set; }

    /// <summary>
    /// 铁路专用线 / Railway siding
    /// </summary>
    [Description("铁路专用线")]
    public bool hasRailwaySiding { get; set; }

    /// <summary>
    /// 高速公路连接 / Highway connection
    /// </summary>
    [Description("高速公路连接")]
    public bool hasHighwayConnection { get; set; }

    /// <summary>
    /// 港口服务（多语言）/ Port services (multilingual)
    /// </summary>
    [Description("港口服务")]
    public LanguageData portServices { get; set; } = new LanguageData();

    /// <summary>
    /// 创建港口基本信息表格 / Create port base information table
    /// </summary>
    protected override MediaWikiTable? CreateBaseInfoTable(MediaWikiDocument doc)
    {
        MediaWikiTable table = base.CreateBaseInfoTable(doc)!;

        // 港口代码
        if (!string.IsNullOrEmpty(portCode))
            AddInfoTableRow(table, "Port Code", new MediaWikiNoLanguage(table) { Content = portCode });

        // 港口类型
        if (!string.IsNullOrEmpty(portType))
            AddInfoTableRow(table, "Port Type", new MediaWikiNoLanguage(table) { Content = portType });

        // 港口性质
        if (!string.IsNullOrEmpty(portNature))
            AddInfoTableRow(table, "Port Nature", new MediaWikiNoLanguage(table) { Content = portNature });

        // 泊位数量
        if (berthCount > 0)
            AddInfoTableRow(table, "Berths", new MediaWikiNoLanguage(table) { Content = berthCount.ToString() });

        // 是否国际港口
        if (isInternationalPort)
            AddInfoTableRow(table, "International Port", new MediaWikiNoLanguage(table) { Content = "Yes" });

        // 是否自贸港
        if (isFreeTradePort)
            AddInfoTableRow(table, "Free Trade Port", new MediaWikiNoLanguage(table) { Content = "Yes" });

        // 港区面积
        if (portArea > 0)
            AddInfoTableRow(table, "Port Area", new MediaWikiNoLanguage(table) { Content = portArea.ToString("N0") + " km²" });

        // 航道水深
        if (channelDepth > 0)
            AddInfoTableRow(table, "Channel Depth", new MediaWikiNoLanguage(table) { Content = channelDepth + " m" });

        return table;
    }
}
