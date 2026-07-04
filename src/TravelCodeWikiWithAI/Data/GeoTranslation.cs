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

// 参考源（只读）：细需求\TravelCodeWikiWithAI\TravelCodeWikiWithAI.Data\GeoTranslation.cs
// 迁移变更：去除 GeoProject 构造器中 1700 行默认值初始化代码（改用 [DefaultTranslation] 声明式默认值）
//           去除 using TravelCodeWikiWithAI.Core
//           保留全部属性声明（每个翻译字段都有对应 UI 展示位置）
//           新增 [DefaultTranslation] 特性标注每个 LanguageData 属性的中文基准文本

using System.ComponentModel;
using System.Reflection;

namespace TravelCodeWikiWithAI.Data;

/// <summary>
/// 翻译默认值特性，为 AI 提供翻译基准文本 / Translation default attribute providing baseline text for AI
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class DefaultTranslationAttribute : Attribute
{
    /// <summary>
    /// 中文基准文本 / Chinese baseline text
    /// </summary>
    public string ZhHans { get; }

    /// <summary>
    /// 是否所有语言已完整 / Whether all languages are complete
    /// </summary>
    public bool AllCode { get; }

    /// <summary>
    /// 构造函数 / Constructor
    /// </summary>
    /// <param name="zhHans">中文基准文本</param>
    /// <param name="allCode">是否所有语言已完整（默认 true）</param>
    public DefaultTranslationAttribute(string zhHans, bool allCode = true)
    {
        ZhHans = zhHans;
        AllCode = allCode;
    }
}

/// <summary>
/// 翻译默认值初始化器 / Translation defaults initializer
/// 递归扫描 GeoTranslation 及子类的所有 LanguageData 属性，
/// 如果属性为空则根据 [DefaultTranslation] 特性创建基准翻译
/// </summary>
public static class TranslationDefaults
{
    /// <summary>
    /// 递归应用默认翻译 / Recursively apply default translations
    /// </summary>
    /// <param name="translation">翻译根对象 / Translation root object</param>
    public static void ApplyDefaults(GeoTranslation translation)
    {
        ApplyDefaultsRecursive(translation);
    }

    private static void ApplyDefaultsRecursive(object obj)
    {
        foreach (var prop in obj.GetType().GetProperties())
        {
            if (prop.PropertyType == typeof(LanguageData))
            {
                var attr = prop.GetCustomAttribute<DefaultTranslationAttribute>();
                if (attr != null)
                {
                    var ld = prop.GetValue(obj) as LanguageData;
                    if (ld == null)
                    {
                        ld = new LanguageData();
                        prop.SetValue(obj, ld);
                    }
                    if (!ld.ContainsKey("zh"))
                    {
                        ld.SetZhHans(attr.ZhHans);
                        // HasAllCode 是只读计算属性，根据已有语言代码自动判定
                    }
                }
            }
            else if (prop.PropertyType.IsSubclassOf(typeof(GeoDataBase)))
            {
                var child = prop.GetValue(obj) as GeoDataBase;
                if (child == null)
                {
                    if (obj is GeoDataBase parentObj && prop.CanWrite)
                    {
                        child = (GeoDataBase?)parentObj.CreateChild(prop.PropertyType, parentObj);
                        if (child != null)
                        {
                            prop.SetValue(obj, child);
                        }
                    }
                }
                if (child != null)
                {
                    ApplyDefaultsRecursive(child);
                }
            }
        }
    }
}

/// <summary>
/// 地理翻译类，管理项目的翻译数据 / Geographic translation class that manages project translation data
/// </summary>
public class GeoTranslation : GeoDataBase
{
    public GeoTranslation(GeoDataBase Parent) : base(Parent)
    {
    }

    public GeoTranslation() : base(null) { }

    /// <summary>
    /// 基础翻译数据 / Base translation data
    /// </summary>
    [Description("基础翻译")]
    public GeoBaseTranslation? Base { get; set; }

    /// <summary>
    /// 页面翻译数据 / Page translation data
    /// </summary>
    [Description("页面翻译")]
    public GeoPageTranslation? Page { get; set; }

    /// <summary>
    /// 地理位置翻译数据 / Location translation data
    /// </summary>
    [Description("地理位置翻译")]
    public GeoLocationTranslation? Location { get; set; }

    /// <summary>
    /// 物价基准翻译数据 / Price benchmark translation data
    /// </summary>
    [Description("物价基准翻译")]
    public GeoPriceBenchmarkTranslation? PriceBenchmark { get; set; }

    /// <summary>
    /// Wiki 翻译数据 / Wiki translation data
    /// </summary>
    [Description("Wiki翻译")]
    public GeoWikiTranslation? Wiki { get; set; }
}

/// <summary>
/// 地理基础翻译类 / Geographic base translation class
/// </summary>
public class GeoBaseTranslation : GeoDataBase
{
    public GeoBaseTranslation(GeoDataBase Parent) : base(Parent) { }

    [Description("网站标题")]
    [DefaultTranslation("旅游编码计划")]
    public LanguageData? SiteName { get; set; }

    [Description("公司名称")]
    [DefaultTranslation("硅基生命集体")]
    public LanguageData? CompontName { get; set; }

    [Description("跳转语言提示")]
    [DefaultTranslation("此页面有其他语言版本")]
    public LanguageData? LangLink { get; set; }

    [Description("复制剪切板文本")]
    [DefaultTranslation("已复制到剪贴板")]
    public LanguageData? CopyText { get; set; }

    [Description("汇率提示")]
    [DefaultTranslation("汇率数据仅供参考")]
    public LanguageData? ExchangeRateNotice { get; set; }

    [Description("未找到景点")]
    [DefaultTranslation("未找到该景点")]
    public LanguageData? AttractionNotFound { get; set; }

    [Description("本月推荐景点")]
    [DefaultTranslation("本月推荐")]
    public LanguageData? MonthlyRecommendedAttractions { get; set; }

    [Description("本日推荐景点")]
    [DefaultTranslation("今日推荐")]
    public LanguageData? DailyRecommendedAttractions { get; set; }

    [Description("哔哩哔哩")]
    [DefaultTranslation("哔哩哔哩")]
    public LanguageData? bilibili { get; set; }

    [Description("小红书")]
    [DefaultTranslation("小红书")]
    public LanguageData? xhs { get; set; }

    [Description("抖音（中国版）")]
    [DefaultTranslation("抖音")]
    public LanguageData? dy { get; set; }

    [Description("微博")]
    [DefaultTranslation("微博")]
    public LanguageData? weibo { get; set; }

    [Description("微信视频号")]
    [DefaultTranslation("微信视频号")]
    public LanguageData? WeChatVideo { get; set; }

    [Description("快手")]
    [DefaultTranslation("快手")]
    public LanguageData? Kuai { get; set; }

    public LanguageData? YouToBe { get; set; }
    public LanguageData? X { get; set; }
    public LanguageData? AcFun { get; set; }
    public LanguageData? FaceBook { get; set; }
    public LanguageData? Instagram { get; set; }
    public LanguageData? AccountName { get; set; }
}

/// <summary>
/// 地理位置翻译类 / Geographic location translation class
/// </summary>
public class GeoLocationTranslation : GeoDataBase
{
    public GeoLocationTranslation(GeoDataBase Parent) : base(Parent) { }

    [Description("首都")]
    [DefaultTranslation("首都")]
    public LanguageData? Capital { get; set; }

    [Description("国旗")]
    [DefaultTranslation("国旗")]
    public LanguageData? Flag { get; set; }

    [Description("语言")]
    [DefaultTranslation("官方语言")]
    public LanguageData? Language { get; set; }

    [Description("省州数量")]
    [DefaultTranslation("省州数量")]
    public LanguageData? ProvinceCount { get; set; }

    [Description("国际长途区号")]
    [DefaultTranslation("国际长途区号")]
    public LanguageData? DialingCode { get; set; }

    [Description("互联网根域名")]
    [DefaultTranslation("互联网根域名")]
    public LanguageData? InternetDomain { get; set; }

    [Description("时区")]
    [DefaultTranslation("时区")]
    public LanguageData? TimeZone { get; set; }

    [Description("官方媒体")]
    [DefaultTranslation("官方媒体")]
    public LanguageData? OfficialMedia { get; set; }

    [Description("签证信息")]
    [DefaultTranslation("签证")]
    public LanguageData? VisaInfo { get; set; }

    [Description("旅游体验")]
    [DefaultTranslation("旅游体验")]
    public LanguageData? TravelExperience { get; set; }

    [Description("安全健康")]
    [DefaultTranslation("安全健康")]
    public LanguageData? SafetyHealth { get; set; }

    [Description("货币与支付")]
    [DefaultTranslation("货币与支付")]
    public LanguageData? CurrencyPayment { get; set; }

    [Description("文化礼仪")]
    [DefaultTranslation("文化礼仪")]
    public LanguageData? CultureEtiquette { get; set; }

    [Description("网络和通信")]
    [DefaultTranslation("网络和通信")]
    public LanguageData? InternetCommunication { get; set; }

    [Description("地理")]
    [DefaultTranslation("地理")]
    public LanguageData? Geography { get; set; }

    [Description("气候")]
    [DefaultTranslation("气候")]
    public LanguageData? Climate { get; set; }

    [Description("生态")]
    [DefaultTranslation("生态")]
    public LanguageData? Ecology { get; set; }

    [Description("历史")]
    [DefaultTranslation("历史")]
    public LanguageData? History { get; set; }

    [Description("宗教")]
    [DefaultTranslation("宗教")]
    public LanguageData? Religion { get; set; }

    [Description("自然资源")]
    [DefaultTranslation("自然资源")]
    public LanguageData? NaturalResources { get; set; }

    [Description("紧急电话")]
    [DefaultTranslation("紧急电话")]
    public LanguageData? EmergencyPhone { get; set; }

    [Description("官方名称")]
    [DefaultTranslation("官方名称")]
    public LanguageData? OfficialName { get; set; }

    [Description("省州列表")]
    [DefaultTranslation("省州列表")]
    public LanguageData? ProvinceList { get; set; }

    [Description("车牌识别与规则")]
    public GeoLicensePlateTranslation? LicensePlate { get; set; }
}

/// <summary>
/// 页面翻译类 / Page translation class
/// </summary>
public class GeoPageTranslation : GeoDataBase
{
    public GeoPageTranslation(GeoDataBase Parent) : base(Parent) { }

    [Description("概览")]
    [DefaultTranslation("概览")]
    public LanguageData? Overview { get; set; }

    [Description("地理分区")]
    [DefaultTranslation("地理分区")]
    public LanguageData? GeographicDivision { get; set; }

    [Description("最佳旅游时间")]
    [DefaultTranslation("最佳旅游时间")]
    public LanguageData? BestTravelTime { get; set; }

    [Description("主要交通枢纽")]
    [DefaultTranslation("主要交通枢纽")]
    public LanguageData? MajorTransportationHubs { get; set; }

    [Description("签证便利性")]
    [DefaultTranslation("签证便利性")]
    public LanguageData? VisaConvenience { get; set; }

    [Description("预算层次")]
    [DefaultTranslation("预算层次")]
    public LanguageData? BudgetLevel { get; set; }

    [Description("主题旅游")]
    [DefaultTranslation("主题旅游")]
    public LanguageData? ThemeTourism { get; set; }

    [Description("跨国经典路线")]
    [DefaultTranslation("跨国经典路线")]
    public LanguageData? CrossBorderClassicRoutes { get; set; }

    [Description("实用建议")]
    [DefaultTranslation("实用建议")]
    public LanguageData? PracticalAdvice { get; set; }

    [Description("国家列表")]
    [DefaultTranslation("国家列表")]
    public LanguageData? CountryList { get; set; }

    [Description("基本信息")]
    [DefaultTranslation("基本信息")]
    public LanguageData? BaseInfo { get; set; }

    [Description("属性")]
    [DefaultTranslation("属性")]
    public LanguageData? Property { get; set; }

    [Description("值")]
    [DefaultTranslation("值")]
    public LanguageData? Value { get; set; }

    [Description("名称")]
    [DefaultTranslation("名称")]
    public LanguageData? Name { get; set; }

    [Description("完整名称")]
    [DefaultTranslation("完整名称")]
    public LanguageData? FullName { get; set; }

    [Description("OSM类型")]
    [DefaultTranslation("OSM类型")]
    public LanguageData? OSMType { get; set; }

    [Description("OSM序号")]
    [DefaultTranslation("OSM序号")]
    public LanguageData? OSMID { get; set; }

    [Description("维基数据")]
    [DefaultTranslation("维基数据")]
    public LanguageData? wikidata { get; set; }

    [Description("国家数量")]
    [DefaultTranslation("国家数量")]
    public LanguageData? CountryCount { get; set; }

    [Description("未分类地区")]
    [DefaultTranslation("未分类地区")]
    public LanguageData? UncategorizedRegion { get; set; }

    [Description("衣食住行")]
    [DefaultTranslation("衣食住行")]
    public LanguageData? BasicNeeds { get; set; }

    [Description("衣")]
    [DefaultTranslation("衣")]
    public LanguageData? Clothing { get; set; }

    [Description("食")]
    [DefaultTranslation("食")]
    public LanguageData? Food { get; set; }

    [Description("住")]
    [DefaultTranslation("住")]
    public LanguageData? Accommodation { get; set; }

    [Description("行")]
    [DefaultTranslation("行")]
    public LanguageData? Transportation { get; set; }

    [Description("当前位置")]
    [DefaultTranslation("当前位置")]
    public LanguageData? CurrentLocation { get; set; }

    [Description("货币种类")]
    [DefaultTranslation("货币种类")]
    public LanguageData? CurrencyType { get; set; }
}

/// <summary>
/// 车牌翻译类 / License plate translation class
/// </summary>
public class GeoLicensePlateTranslation : GeoDataBase
{
    public GeoLicensePlateTranslation(GeoDataBase Parent) : base(Parent) { }

    [Description("车牌类型与颜色")]
    public LanguageData? TypesAndColors { get; set; }
    [Description("新能源汽车专用号牌")]
    public LanguageData? NewEnergyPlate { get; set; }
    [Description("小型新能源汽车号牌")]
    public LanguageData? SmallNewEnergyPlate { get; set; }
    [Description("大型新能源汽车号牌")]
    public LanguageData? LargeNewEnergyPlate { get; set; }
    [Description("背景颜色")]
    public LanguageData? BackgroundColor { get; set; }
    [Description("字体颜色")]
    public LanguageData? FontColor { get; set; }
    [Description("号牌位数")]
    public LanguageData? PlateDigits { get; set; }
    [Description("纯电动车")]
    public LanguageData? PureElectric { get; set; }
    [Description("非纯电动车")]
    public LanguageData? NonPureElectric { get; set; }
    [Description("示例")]
    public LanguageData? Example { get; set; }
    [Description("识别要点")]
    public LanguageData? IdentificationPoints { get; set; }
    [Description("大型汽车号牌")]
    public LanguageData? LargeVehiclePlate { get; set; }
    [Description("基本特征")]
    public LanguageData? BasicFeatures { get; set; }
    [Description("适用车辆类型")]
    public LanguageData? ApplicableVehicleTypes { get; set; }
    [Description("挂车号牌")]
    public LanguageData? TrailerPlate { get; set; }
    [Description("号牌格式")]
    public LanguageData? PlateFormat { get; set; }
    [Description("小型汽车号牌")]
    public LanguageData? SmallVehiclePlate { get; set; }
    [Description("动力类型")]
    public LanguageData? PowerType { get; set; }
    [Description("使、领馆汽车号牌")]
    public LanguageData? EmbassyConsulatePlate { get; set; }
    [Description("特殊标识")]
    public LanguageData? SpecialMarkings { get; set; }
    [Description("使馆号牌")]
    public LanguageData? EmbassyPlate { get; set; }
    [Description("领馆号牌")]
    public LanguageData? ConsulatePlate { get; set; }
    [Description("适用对象")]
    public LanguageData? ApplicableObjects { get; set; }
    [Description("港澳出入境车牌")]
    public LanguageData? HKMacauCrossBorderPlate { get; set; }
    [Description("使用限制")]
    public LanguageData? UsageRestrictions { get; set; }
    [Description("教练车车牌")]
    public LanguageData? DrivingSchoolPlate { get; set; }
    [Description("车辆特点")]
    public LanguageData? VehicleCharacteristics { get; set; }
    [Description("警车号牌")]
    public LanguageData? PolicePlate { get; set; }
    [Description("注意事项")]
    public LanguageData? Precautions { get; set; }
    [Description("应急救援车辆号牌")]
    public LanguageData? EmergencyRescuePlate { get; set; }
    [Description("类型标识")]
    public LanguageData? TypeIdentification { get; set; }
    [Description("武警车牌")]
    public LanguageData? ArmedPolicePlate { get; set; }
    [Description("车牌结构")]
    public LanguageData? PlateStructure { get; set; }
    [Description("上半部分")]
    public LanguageData? UpperSection { get; set; }
    [Description("下半部分")]
    public LanguageData? LowerSection { get; set; }
    [Description("车牌特点")]
    public LanguageData? PlateCharacteristics { get; set; }
    [Description("军车车牌")]
    public LanguageData? MilitaryPlate { get; set; }
    [Description("小型军车车牌")]
    public LanguageData? SmallMilitaryPlate { get; set; }
    [Description("车牌形状")]
    public LanguageData? PlateShape { get; set; }
    [Description("大型军车车牌")]
    public LanguageData? LargeMilitaryPlate { get; set; }
    [Description("车牌编号规则")]
    public LanguageData? NumberingRules { get; set; }
    [Description("号牌结构")]
    public LanguageData? PlateStructureFormat { get; set; }
    [Description("省份简称对照")]
    public LanguageData? ProvinceAbbreviations { get; set; }
    [Description("城市字母编码")]
    public LanguageData? CityLetterCodes { get; set; }
    [Description("号码组成规则")]
    public LanguageData? NumberCompositionRules { get; set; }
    [Description("限行政策")]
    public LanguageData? TrafficRestrictionPolicy { get; set; }
    [Description("政策背景")]
    public LanguageData? PolicyBackground { get; set; }
    [Description("常见限行方式")]
    public LanguageData? CommonRestrictionMethods { get; set; }
    [Description("尾号限行")]
    public LanguageData? TailNumberRestriction { get; set; }
    [Description("主要限行城市")]
    public LanguageData? MajorRestrictedCities { get; set; }
    [Description("特殊说明")]
    public LanguageData? SpecialNotes { get; set; }
    [Description("游客建议")]
    public LanguageData? TouristSuggestions { get; set; }
    [Description("实用提示")]
    public LanguageData? PracticalTips { get; set; }
    [Description("车牌号的重要性")]
    public LanguageData? PlateNumberImportance { get; set; }
    [Description("遇到事故或纠纷时")]
    public LanguageData? AccidentsOrDisputes { get; set; }
    [Description("车牌号的作用")]
    public LanguageData? PlateNumberFunctions { get; set; }
    [Description("信息保护")]
    public LanguageData? InformationProtection { get; set; }
    [Description("其他实用建议")]
    public LanguageData? OtherPracticalSuggestions { get; set; }
    [Description("驾照计分机制")]
    public LanguageData? LicensePointSystem { get; set; }
    [Description("重要提醒")]
    public LanguageData? ImportantReminders { get; set; }
    [Description("驾驶证携带规定")]
    public LanguageData? LicenseCarryingRegulations { get; set; }
    [Description("驾驶证与行驶证的区别")]
    public LanguageData? LicenseVsRegistration { get; set; }
    [Description("驾驶证")]
    public LanguageData? DriversLicense { get; set; }
    [Description("行驶证")]
    public LanguageData? VehicleRegistration { get; set; }
    [Description("两证缺一不可")]
    public LanguageData? BothCertificatesRequired { get; set; }
    [Description("驾驶安全建议")]
    public LanguageData? DrivingSafetySuggestions { get; set; }
    [Description("紧急联系方式")]
    public LanguageData? EmergencyContacts { get; set; }
}

/// <summary>
/// 物价基准翻译类 / Price benchmark translation class
/// </summary>
public class GeoPriceBenchmarkTranslation : GeoDataBase
{
    public GeoPriceBenchmarkTranslation(GeoDataBase Parent) : base(Parent) { }

    // ========== 分类标题 ==========
    [Description("饮料"), DefaultTranslation("饮料")]
    public LanguageData? Beverages { get; set; }
    [Description("快餐"), DefaultTranslation("快餐")]
    public LanguageData? FastFood { get; set; }
    [Description("食品"), DefaultTranslation("食品")]
    public LanguageData? Food { get; set; }
    [Description("餐饮"), DefaultTranslation("餐饮")]
    public LanguageData? Dining { get; set; }
    [Description("交通"), DefaultTranslation("交通")]
    public LanguageData? Transportation { get; set; }
    [Description("住宿"), DefaultTranslation("住宿")]
    public LanguageData? Accommodation { get; set; }
    [Description("日用品"), DefaultTranslation("日用品")]
    public LanguageData? DailyNecessities { get; set; }
    [Description("服务"), DefaultTranslation("服务")]
    public LanguageData? Services { get; set; }
    [Description("通讯"), DefaultTranslation("通讯")]
    public LanguageData? Communication { get; set; }
    [Description("烟酒"), DefaultTranslation("烟酒")]
    public LanguageData? TobaccoAlcohol { get; set; }
    [Description("服装"), DefaultTranslation("服装")]
    public LanguageData? Clothing { get; set; }

    // ========== 饮料类 ==========
    [Description("可口可乐 330ml"), DefaultTranslation("可口可乐 330ml")]
    public LanguageData? CocaCola330ml { get; set; }
    [Description("可口可乐 500ml"), DefaultTranslation("可口可乐 500ml")]
    public LanguageData? CocaCola500ml { get; set; }
    [Description("可口可乐 1.5L"), DefaultTranslation("可口可乐 1.5L")]
    public LanguageData? CocaCola1500ml { get; set; }
    [Description("瓶装水 500ml"), DefaultTranslation("瓶装水 500ml")]
    public LanguageData? Water500ml { get; set; }
    [Description("瓶装水 1.5L"), DefaultTranslation("瓶装水 1.5L")]
    public LanguageData? Water1500ml { get; set; }
    [Description("咖啡（星巴克中杯或同等）"), DefaultTranslation("咖啡（星巴克中杯或同等）")]
    public LanguageData? CoffeeMedium { get; set; }
    [Description("啤酒 330ml（本地品牌）"), DefaultTranslation("啤酒 330ml（本地品牌）")]
    public LanguageData? BeerLocal330ml { get; set; }
    [Description("啤酒 500ml（本地品牌）"), DefaultTranslation("啤酒 500ml（本地品牌）")]
    public LanguageData? BeerLocal500ml { get; set; }

    // ========== 快餐类 ==========
    [Description("麦当劳巨无霸套餐"), DefaultTranslation("麦当劳巨无霸套餐")]
    public LanguageData? BigMacMeal { get; set; }
    [Description("麦当劳巨无霸单品"), DefaultTranslation("麦当劳巨无霸单品")]
    public LanguageData? BigMac { get; set; }
    [Description("肯德基套餐"), DefaultTranslation("肯德基套餐")]
    public LanguageData? KFCMeal { get; set; }
    [Description("披萨（必胜客中号或同等）"), DefaultTranslation("披萨（必胜客中号或同等）")]
    public LanguageData? PizzaMedium { get; set; }

    // ========== 食品类 ==========
    [Description("面包 500g"), DefaultTranslation("面包 500g")]
    public LanguageData? Bread500g { get; set; }
    [Description("牛奶 1L"), DefaultTranslation("牛奶 1L")]
    public LanguageData? Milk1L { get; set; }
    [Description("鸡蛋 12个"), DefaultTranslation("鸡蛋 12个")]
    public LanguageData? Eggs12 { get; set; }
    [Description("大米 1kg"), DefaultTranslation("大米 1kg")]
    public LanguageData? Rice1kg { get; set; }
    [Description("鸡胸肉 1kg"), DefaultTranslation("鸡胸肉 1kg")]
    public LanguageData? ChickenBreast1kg { get; set; }
    [Description("牛肉 1kg"), DefaultTranslation("牛肉 1kg")]
    public LanguageData? Beef1kg { get; set; }
    [Description("苹果 1kg"), DefaultTranslation("苹果 1kg")]
    public LanguageData? Apples1kg { get; set; }
    [Description("香蕉 1kg"), DefaultTranslation("香蕉 1kg")]
    public LanguageData? Bananas1kg { get; set; }
    [Description("橙子 1kg"), DefaultTranslation("橙子 1kg")]
    public LanguageData? Oranges1kg { get; set; }
    [Description("西红柿 1kg"), DefaultTranslation("西红柿 1kg")]
    public LanguageData? Tomatoes1kg { get; set; }
    [Description("土豆 1kg"), DefaultTranslation("土豆 1kg")]
    public LanguageData? Potatoes1kg { get; set; }
    [Description("洋葱 1kg"), DefaultTranslation("洋葱 1kg")]
    public LanguageData? Onions1kg { get; set; }

    // ========== 餐饮类 ==========
    [Description("廉价餐厅一餐"), DefaultTranslation("廉价餐厅一餐")]
    public LanguageData? InexpensiveRestaurantMeal { get; set; }
    [Description("中档餐厅两人餐"), DefaultTranslation("中档餐厅两人餐")]
    public LanguageData? MidRangeRestaurantMeal2People { get; set; }

    // ========== 交通类 ==========
    [Description("出租车起步价"), DefaultTranslation("出租车起步价")]
    public LanguageData? TaxiStartFare { get; set; }
    [Description("出租车每公里价格"), DefaultTranslation("出租车每公里价格")]
    public LanguageData? TaxiPricePerKm { get; set; }
    [Description("公交车单程票"), DefaultTranslation("公交车单程票")]
    public LanguageData? BusTicket { get; set; }
    [Description("地铁单程票"), DefaultTranslation("地铁单程票")]
    public LanguageData? MetroTicket { get; set; }
    [Description("汽油 1L"), DefaultTranslation("汽油 1L")]
    public LanguageData? Gasoline1L { get; set; }

    // ========== 住宿类 ==========
    [Description("经济型酒店（单晚）"), DefaultTranslation("经济型酒店（单晚）")]
    public LanguageData? BudgetHotelPerNight { get; set; }
    [Description("三星级酒店（单晚）"), DefaultTranslation("三星级酒店（单晚）")]
    public LanguageData? ThreeStarHotelPerNight { get; set; }
    [Description("青年旅舍床位（单晚）"), DefaultTranslation("青年旅舍床位（单晚）")]
    public LanguageData? HostelBedPerNight { get; set; }

    // ========== 日用品类 ==========
    [Description("卫生纸 4卷装"), DefaultTranslation("卫生纸 4卷装")]
    public LanguageData? ToiletPaper4Rolls { get; set; }
    [Description("洗发水 400ml"), DefaultTranslation("洗发水 400ml")]
    public LanguageData? Shampoo400ml { get; set; }
    [Description("牙膏"), DefaultTranslation("牙膏")]
    public LanguageData? Toothpaste { get; set; }
    [Description("肥皂"), DefaultTranslation("肥皂")]
    public LanguageData? Soap { get; set; }

    // ========== 服务类 ==========
    [Description("理发（男士基础）"), DefaultTranslation("理发（男士基础）")]
    public LanguageData? MensHaircut { get; set; }
    [Description("电影票"), DefaultTranslation("电影票")]
    public LanguageData? CinemaTicket { get; set; }
    [Description("健身房月卡"), DefaultTranslation("健身房月卡")]
    public LanguageData? GymMonthlyMembership { get; set; }

    // ========== 通讯类 ==========
    [Description("手机预付费卡（含流量）"), DefaultTranslation("手机预付费卡（含流量）")]
    public LanguageData? MobilePrepaidCard { get; set; }
    [Description("网吧上网1小时"), DefaultTranslation("网吧上网1小时")]
    public LanguageData? InternetCafe1Hour { get; set; }

    // ========== 烟酒类 ==========
    [Description("万宝路香烟一包"), DefaultTranslation("万宝路香烟一包")]
    public LanguageData? MarlboroCigarettes1Pack { get; set; }
    [Description("本地啤酒（餐厅）"), DefaultTranslation("本地啤酒（餐厅）")]
    public LanguageData? LocalBeerRestaurant { get; set; }
    [Description("进口啤酒（餐厅）"), DefaultTranslation("进口啤酒（餐厅）")]
    public LanguageData? ImportedBeerRestaurant { get; set; }

    // ========== 服装类 ==========
    [Description("牛仔裤（Levi's或同等）"), DefaultTranslation("牛仔裤（Levi's或同等）")]
    public LanguageData? Jeans { get; set; }
    [Description("运动鞋（Nike或同等）"), DefaultTranslation("运动鞋（Nike或同等）")]
    public LanguageData? SportsShoes { get; set; }
    [Description("夏季连衣裙（Zara或同等）"), DefaultTranslation("夏季连衣裙（Zara或同等）")]
    public LanguageData? SummerDress { get; set; }

    // ========== 通用术语 ==========
    [Description("价格"), DefaultTranslation("价格")]
    public LanguageData? Price { get; set; }
    [Description("物价水平"), DefaultTranslation("物价水平")]
    public LanguageData? PriceLevel { get; set; }
    [Description("本地品牌"), DefaultTranslation("本地品牌")]
    public LanguageData? LocalBrand { get; set; }
    [Description("国际品牌"), DefaultTranslation("国际品牌")]
    public LanguageData? InternationalBrand { get; set; }
    [Description("或同等产品"), DefaultTranslation("或同等产品")]
    public LanguageData? OrEquivalent { get; set; }
    [Description("参考价格"), DefaultTranslation("参考价格")]
    public LanguageData? ReferencePrice { get; set; }
    [Description("价格可能因地区而异"), DefaultTranslation("价格可能因地区而异")]
    public LanguageData? PriceMayVaryByRegion { get; set; }
}

/// <summary>
/// Wiki 翻译类，包含 MediaWiki CurrencyExchange 扩展的翻译信息 / Wiki translation class for MediaWiki CurrencyExchange extension
/// </summary>
public class GeoWikiTranslation : GeoDataBase
{
    public GeoWikiTranslation(GeoDataBase Parent) : base(Parent) { }

    [Description("为 MediaWiki 提供货币汇率和转换功能")]
    public LanguageData? CurrencyExchangeDesc { get; set; }
    [Description("汇率管理")]
    public LanguageData? ExchangeRates { get; set; }
    [Description("汇率管理")]
    public LanguageData? CurrencyExchangeRatesManagement { get; set; }
    [Description("查看和管理货币汇率数据。此页面允许管理员通过在线编辑或文件上传来更新汇率。")]
    public LanguageData? CurrencyExchangeRatesManagementDesc { get; set; }
    [Description("错误：无法写入汇率文件。请检查文件权限。")]
    public LanguageData? CurrencyExchangeErrorFilePermissions { get; set; }
    [Description("当前汇率")]
    public LanguageData? CurrencyExchangeCurrentRates { get; set; }
    [Description("未找到汇率数据。请上传或输入汇率数据。")]
    public LanguageData? CurrencyExchangeNoRatesFound { get; set; }
    [Description("源货币")]
    public LanguageData? CurrencyExchangeFromCurrency { get; set; }
    [Description("目标货币")]
    public LanguageData? CurrencyExchangeToCurrency { get; set; }
    [Description("汇率")]
    public LanguageData? CurrencyExchangeExchangeRate { get; set; }
    [Description("总计：$1 个汇率")]
    public LanguageData? CurrencyExchangeTotalRates { get; set; }
    [Description("未找到汇率文件。")]
    public LanguageData? CurrencyExchangeFileNotFound { get; set; }
    [Description("文件信息：")]
    public LanguageData? CurrencyExchangeFileInfo { get; set; }
    [Description("文件大小：$1")]
    public LanguageData? CurrencyExchangeFileSize { get; set; }
    [Description("最后修改：$1")]
    public LanguageData? CurrencyExchangeLastModified { get; set; }
    [Description("数据时间戳：$1")]
    public LanguageData? CurrencyExchangeDataTimestamp { get; set; }
    [Description("编辑汇率")]
    public LanguageData? CurrencyExchangeEditRates { get; set; }
    [Description("汇率数据 (JSON)：")]
    public LanguageData? CurrencyExchangeRatesDataLabel { get; set; }
    [Description("以 JSON 格式输入汇率数据。")]
    public LanguageData? CurrencyExchangeRatesDataHelp { get; set; }
    [Description("保存更改")]
    public LanguageData? CurrencyExchangeSaveChanges { get; set; }
    [Description("上传汇率文件")]
    public LanguageData? CurrencyExchangeUploadFile { get; set; }
    [Description("上传包含汇率数据的 JSON 文件。文件将在保存前进行验证。")]
    public LanguageData? CurrencyExchangeUploadDesc { get; set; }
    [Description("最大文件大小：$1")]
    public LanguageData? CurrencyExchangeMaxFileSize { get; set; }
    [Description("选择 JSON 文件：")]
    public LanguageData? CurrencyExchangeSelectFile { get; set; }
    [Description("上传文件")]
    public LanguageData? CurrencyExchangeUploadButton { get; set; }
    [Description("会话失败。请重试。")]
    public LanguageData? CurrencyExchangeSessionFailure { get; set; }
    [Description("验证失败。请更正以下错误：")]
    public LanguageData? CurrencyExchangeValidationFailed { get; set; }
    [Description("汇率更新成功。")]
    public LanguageData? CurrencyExchangeUpdateSuccess { get; set; }
    [Description("汇率更新失败。请重试。")]
    public LanguageData? CurrencyExchangeUpdateFailed { get; set; }
    [Description("未上传文件。请选择一个文件。")]
    public LanguageData? CurrencyExchangeNoFileUploaded { get; set; }
    [Description("文件大小 ($1) 超过最大允许大小 ($2)。")]
    public LanguageData? CurrencyExchangeFileTooLarge { get; set; }
    [Description("无效的文件类型。只允许 JSON 文件。")]
    public LanguageData? CurrencyExchangeInvalidFileType { get; set; }
    [Description("读取上传文件失败。")]
    public LanguageData? CurrencyExchangeFailedReadFile { get; set; }
    [Description("文件验证失败。请更正以下错误：")]
    public LanguageData? CurrencyExchangeFileValidationFailed { get; set; }
    [Description("汇率文件上传成功。")]
    public LanguageData? CurrencyExchangeUploadSuccess { get; set; }
    [Description("汇率文件上传失败。请重试。")]
    public LanguageData? CurrencyExchangeUploadFailed { get; set; }
    [Description("缺少必需参数:$1")]
    public LanguageData? CurrencyExchangeErrorMissingParameter { get; set; }
    [Description("无效的金额:$1。金额必须是数字。")]
    public LanguageData? CurrencyExchangeErrorInvalidAmount { get; set; }
    [Description("无效的货币代码:$1。请使用有效的三字母货币代码。")]
    public LanguageData? CurrencyExchangeErrorInvalidCurrency { get; set; }
    [Description("无效的精度:$1。精度必须是 0 到 10 之间的数字。")]
    public LanguageData? CurrencyExchangeErrorInvalidPrecision { get; set; }
    [Description("处理货币操作时发生错误。")]
    public LanguageData? CurrencyExchangeErrorGeneral { get; set; }
    [Description("超过最大递归深度。请简化您的请求。")]
    public LanguageData? CurrencyExchangeErrorRecursionLimit { get; set; }
    [Description("无效的货币代码格式。货币代码必须是恰好3个大写字母。")]
    public LanguageData? CurrencyExchangeErrorInvalidCurrencyFormat { get; set; }
    [Description("检测到安全违规。为了安全起见，请求已被阻止。")]
    public LanguageData? CurrencyExchangeSecurityViolation { get; set; }
    [Description("汇率数据 JSON 格式无效。使用默认汇率 1.0。")]
    public LanguageData? CurrencyExchangeErrorInvalidJson { get; set; }
    [Description("汇率数据似乎已损坏。请检查数据格式。")]
    public LanguageData? CurrencyExchangeErrorDataCorruption { get; set; }
    [Description("读取系统消息失败：$1")]
    public LanguageData? CurrencyExchangeErrorSystemMessageRead { get; set; }
    [Description("写入系统消息失败：$1")]
    public LanguageData? CurrencyExchangeErrorSystemMessageWrite { get; set; }
    [Description("未找到 $1 到 $2 的汇率。使用默认汇率 1.0。")]
    public LanguageData? CurrencyExchangeErrorRateNotFound { get; set; }
    [Description("系统不支持货币 $1。")]
    public LanguageData? CurrencyExchangeErrorCurrencyNotSupported { get; set; }
    [Description("计算结果溢出。请使用较小的金额。")]
    public LanguageData? CurrencyExchangeErrorCalculationOverflow { get; set; }
    [Description("金额不能为负数：$1")]
    public LanguageData? CurrencyExchangeErrorNegativeAmount { get; set; }
    [Description("汇率计算中出现除零错误。")]
    public LanguageData? CurrencyExchangeErrorZeroDivision { get; set; }
    [Description("支持的货币：$1")]
    public LanguageData? CurrencyExchangeSupportedCurrencies { get; set; }
    [Description("、")]
    public LanguageData? CurrencyExchangeCurrencyListSeparator { get; set; }
    [Description("汇率计算的默认货币")]
    public LanguageData? CurrencyExchangePrefDefaultCurrency { get; set; }
    [Description("选择您偏好的货币用于自动转换。选择\"自动\"以使用系统默认值。")]
    public LanguageData? CurrencyExchangePrefDefaultCurrencyHelp { get; set; }
    [Description("自动（使用系统默认）")]
    public LanguageData? CurrencyExchangePrefCurrencyAuto { get; set; }
    [Description("转换货币金额。用法：{{#exchange:金额|源货币|目标货币|精度}}。")]
    public LanguageData? CurrencyExchangeHelpExchange { get; set; }
    [Description("获取货币间汇率。用法：{{#exchangerate:源货币|目标货币}}")]
    public LanguageData? CurrencyExchangeHelpExchangeRate { get; set; }
    [Description("获取本地化货币名称。用法：{{#currencyname:货币代码}}")]
    public LanguageData? CurrencyExchangeHelpCurrencyName { get; set; }
    [Description("获取货币符号。用法：{{#currencysymbol:货币代码|类型}}。")]
    public LanguageData? CurrencyExchangeHelpCurrencySymbol { get; set; }
    [Description("示例：{{#exchange:100|USD|EUR}} 将 100 美元转换为欧元")]
    public LanguageData? CurrencyExchangeExampleExchange { get; set; }
    [Description("示例：{{#exchangerate:USD|EUR}} 显示美元到欧元的汇率")]
    public LanguageData? CurrencyExchangeExampleExchangeRate { get; set; }
    [Description("示例：{{#currencyname:USD}} 显示\"美元\"")]
    public LanguageData? CurrencyExchangeExampleCurrencyName { get; set; }
    [Description("示例：{{#currencysymbol:USD}} 显示\"$\"，{{#currencysymbol:USD|wide}} 显示\"US$\"")]
    public LanguageData? CurrencyExchangeExampleCurrencySymbol { get; set; }
    [Description("只有管理员可以编辑汇率。您可以查看下面的当前汇率。")]
    public LanguageData? CurrencyExchangeAdminOnlyNotice { get; set; }
    [Description("编辑汇率需要管理员权限。")]
    public LanguageData? CurrencyExchangeEditPermissionRequired { get; set; }
    [Description("上传汇率文件需要管理员权限。")]
    public LanguageData? CurrencyExchangeUploadPermissionRequired { get; set; }
    [Description("默认货币")]
    public LanguageData? CurrencyExchangePrefCurrencyLabel { get; set; }
    [Description("选择您偏好的货币用于自动转换。选择\"自动\"以使用系统默认货币。")]
    public LanguageData? CurrencyExchangePrefCurrencyHelp { get; set; }
}
