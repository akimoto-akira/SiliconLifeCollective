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

using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Database of Chinese historical dynasties and era names (年号)
/// 
/// This class maintains a comprehensive database of Chinese dynasties and their era names,
/// including start and end years in the Gregorian calendar.
/// 
/// Supported periods:
/// - Ancient dynasties (Xia, Shang, Zhou)
/// - Qin Dynasty (秦朝): 221-206 BCE
/// - Han Dynasty (汉朝): 206 BCE - 220 CE
/// - Three Kingdoms (三国): 220-280 CE
/// - Jin Dynasty (晋朝): 265-420 CE
/// - Southern and Northern Dynasties (南北朝): 420-589 CE
/// - Sui Dynasty (隋朝): 581-618 CE
/// - Tang Dynasty (唐朝): 618-907 CE
/// - Five Dynasties and Ten Kingdoms (五代十国): 907-979 CE
/// - Song Dynasty (宋朝): 960-1279 CE
/// - Yuan Dynasty (元朝): 1271-1368 CE
/// - Ming Dynasty (明朝): 1368-1644 CE
/// - Qing Dynasty (清朝): 1644-1912 CE
/// - Republic of China (民国): 1912-1949 CE
/// - Common Era (公元): 1949-present
/// </summary>
public class ChineseHistoricalEraDatabase
{
    /// <summary>
    /// Era data record
    /// </summary>
    public record EraData
    {
        /// <summary>
        /// Dynasty key for localization (e.g., "qing", "ming", "tang")
        /// </summary>
        public string DynastyKey { get; init; } = "";
        
        /// <summary>
        /// Era key for localization (e.g., "kangxi", "qianlong", "wanli")
        /// </summary>
        public string EraKey { get; init; } = "";
        
        /// <summary>
        /// Localized dynasty name (set after localization)
        /// </summary>
        public string LocalizedDynastyName { get; set; } = "";
        
        /// <summary>
        /// Localized era name (set after localization)
        /// </summary>
        public string LocalizedEraName { get; set; } = "";
        
        /// <summary>
        /// Localized notes (set after localization)
        /// </summary>
        public string LocalizedNotes { get; set; } = "";
        
        public int StartYear { get; init; }
        public int EndYear { get; init; }
        public int FirstYear { get; init; } = 1;
        public string Notes { get; init; } = "";
    }

    private readonly List<EraData> _eras = [];

    /// <summary>
    /// Constructor - Initialize era database
    /// </summary>
    public ChineseHistoricalEraDatabase()
    {
        InitializeEras();
    }

    /// <summary>
    /// Initialize era database with historical data
    /// </summary>
    private void InitializeEras()
    {
        // Get Chinese historical localization instance
        var localization = ServiceLocator.Instance.Get<DefaultLocalizationBase>();
        var chineseHistorical = localization?.GetChineseHistoricalLocalization();
        
        if (chineseHistorical == null)
            return;
        
        AddAncientDynastyEras(chineseHistorical);
        AddQinDynastyEras(chineseHistorical);
        AddWesternHanEras(chineseHistorical);
        AddXinDynastyEras(chineseHistorical);
        AddEasternHanEras(chineseHistorical);
        AddThreeKingdomsEras(chineseHistorical);
        AddJinDynastyEras(chineseHistorical);
        AddSouthernNorthernDynastiesEras(chineseHistorical);
        AddSuiDynastyEras(chineseHistorical);
        AddTangDynastyEras(chineseHistorical);
        AddFiveDynastiesTenKingdomsEras(chineseHistorical);
        AddSongDynastyEras(chineseHistorical);
        AddYuanDynastyEras(chineseHistorical);
        AddMingDynastyEras(chineseHistorical);
        AddQingDynastyEras(chineseHistorical);
    }

    /// <summary>
    /// Get era by dynasty key and era key
    /// </summary>
    /// <param name="dynastyKey">Dynasty localization key</param>
    /// <param name="eraKey">Era localization key</param>
    /// <returns>Era data or null if not found</returns>
    public EraData? GetEra(string dynastyKey, string eraKey)
    {
        var dynastyLower = dynastyKey.ToLowerInvariant();
        var eraLower = eraKey.ToLowerInvariant();

        foreach (var era in _eras)
        {
            if (era.DynastyKey.ToLowerInvariant() == dynastyLower &&
                era.EraKey.ToLowerInvariant() == eraLower)
                return era;
        }
        return null;
    }

    /// <summary>
    /// Get era by Gregorian date
    /// </summary>
    /// <param name="year">Gregorian year</param>
    /// <param name="month">Gregorian month</param>
    /// <param name="day">Gregorian day</param>
    /// <returns>Era data or null if not found</returns>
    public EraData? GetEraByGregorianDate(int year, int month, int day)
    {
        EraData? matchedEra = null;
        var useYearOnly = year < 1912;

        if (useYearOnly)
        {
            foreach (var era in _eras)
            {
                if (year >= era.StartYear && year <= era.EndYear)
                    matchedEra = era;
            }
        }
        else
        {
            var targetDate = year * 10000 + month * 100 + day;

            foreach (var era in _eras)
            {
                if (era.EndYear < 1912)
                    continue;

                var startDate = era.StartYear * 10000 + 101;
                var endDate = era.EndYear * 10000 + 1231;

                if (targetDate >= startDate && targetDate <= endDate)
                    matchedEra = era;
            }
        }

        return matchedEra;
    }

    /// <summary>
    /// Get all eras for a dynasty
    /// </summary>
    /// <param name="dynastyKey">Dynasty localization key</param>
    /// <returns>Array of era data</returns>
    public List<EraData> GetErasByDynasty(string dynastyKey)
    {
        return _eras.Where(e => e.DynastyKey == dynastyKey).ToList();
    }

    /// <summary>
    /// Get all dynasty keys
    /// </summary>
    /// <returns>Array of unique dynasty keys</returns>
    public List<string> GetAllDynastyKeys()
    {
        return _eras.Select(e => e.DynastyKey).Distinct().ToList();
    }

    /// <summary>
    /// Check if a dynasty exists in the database
    /// </summary>
    /// <param name="dynastyKey">Dynasty localization key</param>
    /// <returns>True if dynasty exists, false otherwise</returns>
    public bool HasDynasty(string dynastyKey)
    {
        return _eras.Any(e => e.DynastyKey == dynastyKey);
    }

    private void AddQingDynastyEras(ChineseHistorical.ChineseHistoricalLocalizationBase loc)
    {
        var qingEras = new[]
        {
            new { EraName = loc.EraShunzhi, Start = 1644, End = 1661 },
            new { EraName = loc.EraKangxi, Start = 1662, End = 1722 },
            new { EraName = loc.EraYongzheng, Start = 1723, End = 1735 },
            new { EraName = loc.EraQianlong, Start = 1736, End = 1795 },
            new { EraName = loc.EraJiaqing, Start = 1796, End = 1820 },
            new { EraName = loc.EraDaoguang, Start = 1821, End = 1850 },
            new { EraName = loc.EraXianfeng, Start = 1851, End = 1861 },
            new { EraName = loc.EraTongzhi, Start = 1862, End = 1874 },
            new { EraName = loc.EraGuangxu, Start = 1875, End = 1908 },
            new { EraName = loc.EraXuantong, Start = 1909, End = 1911 },
        };

        foreach (var era in qingEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "qing",
                LocalizedDynastyName = loc.Qing,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }

        _eras.Add(new EraData
        {
            DynastyKey = "roc",
            LocalizedDynastyName = loc.ROC,
            LocalizedEraName = loc.EraRepublic,
            LocalizedNotes = loc.NoteROC,
            StartYear = 1912,
            EndYear = 1949,
        });

        _eras.Add(new EraData
        {
            DynastyKey = "prc",
            LocalizedDynastyName = loc.PRC,
            LocalizedEraName = loc.EraCommonEra,
            LocalizedNotes = loc.NotePRC,
            StartYear = 1949,
            EndYear = 2100,
        });
    }

    private void AddMingDynastyEras(ChineseHistorical.ChineseHistoricalLocalizationBase loc)
    {
        var mingEras = new[]
        {
            new { EraName = loc.EraHongwu, Start = 1368, End = 1398 },
            new { EraName = loc.EraJianwen, Start = 1399, End = 1402 },
            new { EraName = loc.EraYongle, Start = 1403, End = 1424 },
            new { EraName = loc.EraHongxi, Start = 1425, End = 1425 },
            new { EraName = loc.EraXuande, Start = 1426, End = 1435 },
            new { EraName = loc.EraZhengtong, Start = 1436, End = 1449 },
            new { EraName = loc.EraJingtai, Start = 1450, End = 1457 },
            new { EraName = loc.EraTianshunMing, Start = 1457, End = 1464 },
            new { EraName = loc.EraChenghua, Start = 1465, End = 1487 },
            new { EraName = loc.EraHongzhi, Start = 1488, End = 1505 },
            new { EraName = loc.EraZhengde, Start = 1506, End = 1521 },
            new { EraName = loc.EraJiajing, Start = 1522, End = 1566 },
            new { EraName = loc.EraLongqing, Start = 1567, End = 1572 },
            new { EraName = loc.EraWanli, Start = 1573, End = 1620 },
            new { EraName = loc.EraTaichang, Start = 1620, End = 1620 },
            new { EraName = loc.EraTianqi, Start = 1621, End = 1627 },
            new { EraName = loc.EraChongzhen, Start = 1628, End = 1644 },
        };

        foreach (var era in mingEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Ming".ToLowerInvariant(),
                LocalizedDynastyName = loc.Ming,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }
    }

    private void AddYuanDynastyEras(ChineseHistorical.ChineseHistoricalLocalizationBase loc)
    {
        var yuanEras = new[]
        {
            new { EraName = loc.EraZhiyuan, Start = 1264, End = 1294 },
            new { EraName = loc.EraYuanzhen, Start = 1295, End = 1297 },
            new { EraName = loc.EraDade, Start = 1297, End = 1307 },
            new { EraName = loc.EraZhida, Start = 1308, End = 1311 },
            new { EraName = loc.EraHuangqing, Start = 1312, End = 1313 },
            new { EraName = loc.EraYanyou, Start = 1314, End = 1320 },
            new { EraName = loc.EraZhizhi, Start = 1321, End = 1323 },
            new { EraName = loc.EraTaiding, Start = 1324, End = 1328 },
            new { EraName = loc.EraTianshunYuan, Start = 1328, End = 1328 },
            new { EraName = loc.EraZhiheYuan, Start = 1328, End = 1329 },
            new { EraName = loc.EraTianli, Start = 1329, End = 1330 },
            new { EraName = loc.EraZhishun, Start = 1330, End = 1333 },
            new { EraName = loc.EraYuantong, Start = 1333, End = 1335 },
            new { EraName = loc.EraZhiyuan2, Start = 1335, End = 1340 },
            new { EraName = loc.EraZhizheng, Start = 1341, End = 1368 },
        };

        foreach (var era in yuanEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Yuan".ToLowerInvariant(),
                LocalizedDynastyName = loc.Yuan,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }
    }

    private void AddSongDynastyEras(ChineseHistorical.ChineseHistoricalLocalizationBase loc)
    {
        var songEras = new[]
        {
            new { EraName = loc.EraJianlong, Start = 960, End = 963 },
            new { EraName = loc.EraQiande, Start = 963, End = 968 },
            new { EraName = loc.EraKaibao, Start = 968, End = 976 },
            new { EraName = loc.EraTaipingxingguo, Start = 976, End = 984 },
            new { EraName = loc.EraYongxiSong, Start = 984, End = 987 },
            new { EraName = loc.EraDuangong, Start = 988, End = 989 },
            new { EraName = loc.EraChunhua, Start = 990, End = 994 },
            new { EraName = loc.EraZhidao, Start = 995, End = 997 },
            new { EraName = loc.EraXianping, Start = 998, End = 1003 },
            new { EraName = loc.EraJingde, Start = 1004, End = 1007 },
            new { EraName = loc.EraDazhongxiangfu, Start = 1008, End = 1016 },
            new { EraName = loc.EraTianxiSong, Start = 1017, End = 1021 },
            new { EraName = loc.EraQianxing, Start = 1022, End = 1022 },
            new { EraName = loc.EraTiansheng, Start = 1023, End = 1032 },
            new { EraName = loc.EraMingdao, Start = 1032, End = 1033 },
            new { EraName = loc.EraJingyou, Start = 1034, End = 1038 },
            new { EraName = loc.EraBaoyuan, Start = 1038, End = 1040 },
            new { EraName = loc.EraQingli, Start = 1041, End = 1048 },
            new { EraName = loc.EraHuangyou, Start = 1049, End = 1054 },
            new { EraName = loc.EraZhiheSong, Start = 1054, End = 1056 },
            new { EraName = loc.EraJiayou, Start = 1056, End = 1063 },
            new { EraName = loc.EraZhiping, Start = 1064, End = 1067 },
            new { EraName = loc.EraXining, Start = 1068, End = 1077 },
            new { EraName = loc.EraYuanfengSong, Start = 1078, End = 1085 },
            new { EraName = loc.EraYuanyou, Start = 1086, End = 1094 },
            new { EraName = loc.EraShaosheng, Start = 1094, End = 1098 },
            new { EraName = loc.EraYuanfu, Start = 1098, End = 1100 },
            new { EraName = loc.EraJianzhongSong, Start = 1101, End = 1101 },
            new { EraName = loc.EraChongning, Start = 1102, End = 1106 },
            new { EraName = loc.EraDaguan, Start = 1107, End = 1110 },
            new { EraName = loc.EraZhengheSong, Start = 1111, End = 1118 },
            new { EraName = loc.EraChonghe, Start = 1118, End = 1119 },
            new { EraName = loc.EraXuanhe, Start = 1119, End = 1125 },
            new { EraName = loc.EraJingkang, Start = 1126, End = 1127 },
            new { EraName = loc.EraJianyan, Start = 1127, End = 1130 },
            new { EraName = loc.EraShaoxing, Start = 1131, End = 1162 },
            new { EraName = loc.EraLongxing, Start = 1163, End = 1164 },
            new { EraName = loc.EraQiandao, Start = 1165, End = 1173 },
            new { EraName = loc.EraChunxi, Start = 1174, End = 1189 },
            new { EraName = loc.EraShaoxi, Start = 1190, End = 1194 },
            new { EraName = loc.EraQingyuan, Start = 1195, End = 1200 },
            new { EraName = loc.EraJiatai, Start = 1201, End = 1204 },
            new { EraName = loc.EraKaixi, Start = 1205, End = 1207 },
            new { EraName = loc.EraJiading, Start = 1208, End = 1224 },
            new { EraName = loc.EraBaoqing, Start = 1225, End = 1227 },
            new { EraName = loc.EraShaoding, Start = 1228, End = 1233 },
            new { EraName = loc.EraDuanping, Start = 1234, End = 1236 },
            new { EraName = loc.EraJiaxi, Start = 1237, End = 1240 },
            new { EraName = loc.EraChunyou, Start = 1241, End = 1252 },
            new { EraName = loc.EraBaoyou, Start = 1253, End = 1258 },
            new { EraName = loc.EraKaiqing, Start = 1259, End = 1260 },
            new { EraName = loc.EraJingding, Start = 1260, End = 1264 },
            new { EraName = loc.EraXianchun, Start = 1265, End = 1274 },
            new { EraName = loc.EraDeyou, Start = 1275, End = 1276 },
            new { EraName = loc.EraJingyan, Start = 1276, End = 1278 },
            new { EraName = loc.EraXiangxing, Start = 1278, End = 1279 },
        };

        foreach (var era in songEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Song".ToLowerInvariant(),
                LocalizedDynastyName = loc.Song,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }
    }

    private void AddTangDynastyEras(ChineseHistorical.ChineseHistoricalLocalizationBase loc)
    {
        var tangEras = new[]
        {
            new { EraName = loc.EraWude, Start = 618, End = 626 },
            new { EraName = loc.EraZhenguan, Start = 627, End = 649 },
            new { EraName = loc.EraYonghui, Start = 650, End = 655 },
            new { EraName = loc.EraXianqing, Start = 656, End = 661 },
            new { EraName = loc.EraLongshuo, Start = 661, End = 663 },
            new { EraName = loc.EraLinde, Start = 664, End = 665 },
            new { EraName = loc.EraQianfeng, Start = 666, End = 668 },
            new { EraName = loc.EraZongzhang, Start = 668, End = 670 },
            new { EraName = loc.EraXianheng, Start = 670, End = 674 },
            new { EraName = loc.EraShangyuanTang, Start = 674, End = 676 },
            new { EraName = loc.EraYifeng, Start = 676, End = 679 },
            new { EraName = loc.EraTiaolu, Start = 679, End = 680 },
            new { EraName = loc.EraYonglong, Start = 680, End = 681 },
            new { EraName = loc.EraKaiyao, Start = 681, End = 682 },
            new { EraName = loc.EraYongchun, Start = 682, End = 683 },
            new { EraName = loc.EraHongdao, Start = 683, End = 684 },
            new { EraName = loc.EraSisheng, Start = 684, End = 684 },
            new { EraName = loc.EraWenming, Start = 684, End = 684 },
            new { EraName = loc.EraGuangzhai, Start = 684, End = 684 },
            new { EraName = loc.EraChuigong, Start = 685, End = 688 },
            new { EraName = loc.EraYongchangTang, Start = 689, End = 689 },
            new { EraName = loc.EraTianshou, Start = 690, End = 692 },
            new { EraName = loc.EraRuyi, Start = 692, End = 692 },
            new { EraName = loc.EraChangshou, Start = 692, End = 694 },
            new { EraName = loc.EraYanzai, Start = 694, End = 695 },
            new { EraName = loc.EraZhengsheng, Start = 695, End = 695 },
            new { EraName = loc.EraTiancewansui, Start = 695, End = 696 },
            new { EraName = loc.EraWansuitongtian, Start = 696, End = 697 },
            new { EraName = loc.EraShengong, Start = 697, End = 697 },
            new { EraName = loc.EraShengli, Start = 698, End = 700 },
            new { EraName = loc.EraJiushi, Start = 700, End = 701 },
            new { EraName = loc.EraDazu, Start = 701, End = 701 },
            new { EraName = loc.EraChangAn, Start = 701, End = 705 },
            new { EraName = loc.EraShenlong, Start = 705, End = 707 },
            new { EraName = loc.EraJinglong, Start = 707, End = 710 },
            new { EraName = loc.EraJingyun, Start = 710, End = 712 },
            new { EraName = loc.EraTaiji, Start = 712, End = 712 },
            new { EraName = loc.EraYanhe, Start = 712, End = 713 },
            new { EraName = loc.EraXiantian, Start = 712, End = 713 },
            new { EraName = loc.EraKaiyuan, Start = 713, End = 741 },
            new { EraName = loc.EraTianbao, Start = 742, End = 756 },
            new { EraName = loc.EraZhide, Start = 756, End = 758 },
            new { EraName = loc.EraQianyuan, Start = 758, End = 760 },
            new { EraName = loc.EraShangyuanTang2, Start = 760, End = 762 },
            new { EraName = loc.EraBaoying, Start = 762, End = 763 },
            new { EraName = loc.EraGuangdeTang, Start = 763, End = 764 },
            new { EraName = loc.EraYongtai, Start = 765, End = 766 },
            new { EraName = loc.EraDali, Start = 766, End = 779 },
            new { EraName = loc.EraJianzhongTang, Start = 780, End = 783 },
            new { EraName = loc.EraXingyuan, Start = 784, End = 784 },
            new { EraName = loc.EraZhenyuan, Start = 785, End = 805 },
            new { EraName = loc.EraYongzhen, Start = 805, End = 805 },
            new { EraName = loc.EraYuanheTang, Start = 806, End = 820 },
            new { EraName = loc.EraChangqing, Start = 821, End = 824 },
            new { EraName = loc.EraBaoli, Start = 825, End = 827 },
            new { EraName = loc.EraDahe, Start = 827, End = 835 },
            new { EraName = loc.EraKaicheng, Start = 836, End = 840 },
            new { EraName = loc.EraHuichang, Start = 841, End = 846 },
            new { EraName = loc.EraDazhong, Start = 847, End = 860 },
            new { EraName = loc.EraXiantong, Start = 860, End = 874 },
            new { EraName = loc.EraQianfu, Start = 874, End = 879 },
            new { EraName = loc.EraGuangming, Start = 880, End = 881 },
            new { EraName = loc.EraZhonghe, Start = 881, End = 885 },
            new { EraName = loc.EraGuangqi, Start = 885, End = 888 },
            new { EraName = loc.EraWende, Start = 888, End = 888 },
            new { EraName = loc.EraLongji, Start = 889, End = 889 },
            new { EraName = loc.EraDashun, Start = 890, End = 891 },
            new { EraName = loc.EraJingfu, Start = 892, End = 893 },
            new { EraName = loc.EraQianning, Start = 894, End = 898 },
            new { EraName = loc.EraGuanghua, Start = 898, End = 901 },
            new { EraName = loc.EraTianfuTang, Start = 901, End = 904 },
            new { EraName = loc.EraTianyouTang, Start = 904, End = 907 },
        };

        foreach (var era in tangEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Tang".ToLowerInvariant(),
                LocalizedDynastyName = loc.Tang,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }
    }

    private void AddSuiDynastyEras(ChineseHistorical.ChineseHistoricalLocalizationBase loc)
    {
        var suiEras = new[]
        {
            new { EraName = loc.EraKaihuang, Start = 581, End = 600 },
            new { EraName = loc.EraRenshou, Start = 601, End = 604 },
            new { EraName = loc.EraDaye, Start = 605, End = 618 },
        };

        foreach (var era in suiEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Sui".ToLowerInvariant(),
                LocalizedDynastyName = loc.Sui,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }
    }

    private void AddThreeKingdomsEras(ChineseHistorical.ChineseHistoricalLocalizationBase loc)
    {
        var weiEras = new[]
        {
            new { EraName = loc.EraHuangchu, Start = 220, End = 226 },
            new { EraName = loc.EraTaiheWei, Start = 227, End = 233 },
            new { EraName = loc.EraQinglong, Start = 233, End = 237 },
            new { EraName = loc.EraJingchu, Start = 237, End = 239 },
            new { EraName = loc.EraZhengshi, Start = 240, End = 249 },
            new { EraName = loc.EraJiaping, Start = 249, End = 254 },
            new { EraName = loc.EraZhengyuan, Start = 254, End = 256 },
            new { EraName = loc.EraGanluWei, Start = 256, End = 260 },
            new { EraName = loc.EraJingyuan, Start = 260, End = 264 },
            new { EraName = loc.EraXianxi, Start = 264, End = 265 },
        };

        foreach (var era in weiEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Wei".ToLowerInvariant(),
                LocalizedDynastyName = loc.Wei,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }

        var shuEras = new[]
        {
            new { EraName = loc.EraZhangwu, Start = 221, End = 223 },
            new { EraName = loc.EraJianxingSH, Start = 223, End = 237 },
            new { EraName = loc.EraYanxiSH, Start = 238, End = 257 },
            new { EraName = loc.EraJingyao, Start = 258, End = 263 },
        };

        foreach (var era in shuEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Shu".ToLowerInvariant(),
                LocalizedDynastyName = loc.Shu,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }

        var wuEras = new[]
        {
            new { EraName = loc.EraHuangwu, Start = 222, End = 229 },
            new { EraName = loc.EraHuanglongWu, Start = 229, End = 231 },
            new { EraName = loc.EraJiahe, Start = 232, End = 238 },
            new { EraName = loc.EraChiwu, Start = 238, End = 251 },
            new { EraName = loc.EraTaipingWu, Start = 251, End = 252 },
            new { EraName = loc.EraShenfeng, Start = 252, End = 252 },
            new { EraName = loc.EraJianxingWu, Start = 252, End = 253 },
            new { EraName = loc.EraWufengWu, Start = 254, End = 256 },
            new { EraName = loc.EraTaipingWu, Start = 256, End = 258 },
            new { EraName = loc.EraYonganWu, Start = 258, End = 264 },
            new { EraName = loc.EraYuanxingWu, Start = 264, End = 265 },
            new { EraName = loc.EraGanluWu, Start = 265, End = 266 },
            new { EraName = loc.EraBaoding, Start = 266, End = 269 },
            new { EraName = loc.EraJianheng, Start = 269, End = 271 },
            new { EraName = loc.EraFenghuang, Start = 272, End = 274 },
            new { EraName = loc.EraTianceWu, Start = 275, End = 276 },
            new { EraName = loc.EraTianxiWu, Start = 276, End = 276 },
            new { EraName = loc.EraTianji, Start = 277, End = 280 },
        };

        foreach (var era in wuEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Wu".ToLowerInvariant(),
                LocalizedDynastyName = loc.Wu,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }
    }

    private void AddJinDynastyEras(ChineseHistorical.ChineseHistoricalLocalizationBase loc)
    {
        var westernJinEras = new[]
        {
            new { EraName = loc.EraTaishiJin, Start = 265, End = 274 },
            new { EraName = loc.EraXianning, Start = 275, End = 280 },
            new { EraName = loc.EraTaikang, Start = 280, End = 289 },
            new { EraName = loc.EraTaixi, Start = 290, End = 290 },
            new { EraName = loc.EraYongxi, Start = 290, End = 291 },
            new { EraName = loc.EraYongpingJin, Start = 291, End = 291 },
            new { EraName = loc.EraYuankangJin, Start = 291, End = 299 },
            new { EraName = loc.EraYongkangJin, Start = 300, End = 301 },
            new { EraName = loc.EraYonganJin, Start = 302, End = 303 },
            new { EraName = loc.EraTaan, Start = 302, End = 303 },
            new { EraName = loc.EraYongxingJin, Start = 304, End = 306 },
            new { EraName = loc.EraGuangxiJin, Start = 306, End = 306 },
            new { EraName = loc.EraYongjiaJin, Start = 307, End = 313 },
            new { EraName = loc.EraJianxingJin, Start = 313, End = 317 },
        };

        foreach (var era in westernJinEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Jin".ToLowerInvariant(),
                LocalizedDynastyName = loc.Jin,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }

        var easternJinEras = new[]
        {
            new { EraName = loc.EraJianwuEJ, Start = 317, End = 318 },
            new { EraName = loc.EraTaixing, Start = 318, End = 321 },
            new { EraName = loc.EraYongchang, Start = 322, End = 323 },
            new { EraName = loc.EraTaining, Start = 323, End = 326 },
            new { EraName = loc.EraXianhe, Start = 326, End = 334 },
            new { EraName = loc.EraXiankang, Start = 335, End = 342 },
            new { EraName = loc.EraJianyuanEJ, Start = 343, End = 344 },
            new { EraName = loc.EraYongheEJ, Start = 345, End = 356 },
            new { EraName = loc.EraShengping, Start = 357, End = 361 },
            new { EraName = loc.EraLonghe, Start = 362, End = 363 },
            new { EraName = loc.EraXingning, Start = 363, End = 365 },
            new { EraName = loc.EraTaiyuanEJ, Start = 366, End = 396 },
            new { EraName = loc.EraLongan, Start = 397, End = 401 },
            new { EraName = loc.EraYuanxingEJ, Start = 402, End = 404 },
            new { EraName = loc.EraYixi, Start = 405, End = 418 },
            new { EraName = loc.EraYuanxi, Start = 419, End = 420 },
        };

        foreach (var era in easternJinEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Jin".ToLowerInvariant(),
                LocalizedDynastyName = loc.Jin,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }
    }

    private void AddSouthernNorthernDynastiesEras(ChineseHistorical.ChineseHistoricalLocalizationBase loc)
    {
        _eras.Add(new EraData
        {
            DynastyKey = "Northern Wei".ToLowerInvariant(),
            LocalizedDynastyName = loc.NorthernWei,
            LocalizedEraName = loc.EraNorthernWeiPeriod,
            LocalizedNotes = loc.NoteNorthernWeiPeriod,
            StartYear = 386,
            EndYear = 534,
        });

        _eras.Add(new EraData
        {
            DynastyKey = "Eastern Wei".ToLowerInvariant(),
            LocalizedDynastyName = loc.EasternWei,
            LocalizedEraName = loc.EraEasternWeiPeriod,
            LocalizedNotes = loc.NoteEasternWeiPeriod,
            StartYear = 534,
            EndYear = 550,
        });

        _eras.Add(new EraData
        {
            DynastyKey = "Western Wei".ToLowerInvariant(),
            LocalizedDynastyName = loc.WesternWei,
            LocalizedEraName = loc.EraWesternWeiPeriod,
            LocalizedNotes = loc.NoteWesternWeiPeriod,
            StartYear = 535,
            EndYear = 556,
        });

        _eras.Add(new EraData
        {
            DynastyKey = "Northern Qi".ToLowerInvariant(),
            LocalizedDynastyName = loc.NorthernQi,
            LocalizedEraName = loc.EraNorthernQiPeriod,
            LocalizedNotes = loc.NoteNorthernQiPeriod,
            StartYear = 550,
            EndYear = 577,
        });

        _eras.Add(new EraData
        {
            DynastyKey = "Northern Zhou".ToLowerInvariant(),
            LocalizedDynastyName = loc.NorthernZhou,
            LocalizedEraName = loc.EraNorthernZhouPeriod,
            LocalizedNotes = loc.NoteNorthernZhouPeriod,
            StartYear = 557,
            EndYear = 581,
        });
    }

    private void AddFiveDynastiesTenKingdomsEras(ChineseHistorical.ChineseHistoricalLocalizationBase loc)
    {
        var laterLiangEras = new[]
        {
            new { EraName = loc.EraKaiping, Start = 907, End = 911 },
            new { EraName = loc.EraQianhua, Start = 911, End = 913 },
            new { EraName = loc.EraFengli, Start = 913, End = 913 },
            new { EraName = loc.EraQianhua2, Start = 913, End = 915 },
            new { EraName = loc.EraZhenming, Start = 915, End = 921 },
            new { EraName = loc.EraLongde, Start = 921, End = 923 },
        };

        foreach (var era in laterLiangEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Later Liang".ToLowerInvariant(),
                LocalizedDynastyName = loc.LaterLiang,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }

        var laterTangEras = new[]
        {
            new { EraName = loc.EraTongguang, Start = 923, End = 926 },
            new { EraName = loc.EraTiancheng, Start = 926, End = 930 },
            new { EraName = loc.EraChangxing, Start = 930, End = 933 },
            new { EraName = loc.EraYingshun, Start = 934, End = 934 },
            new { EraName = loc.EraQingtai, Start = 934, End = 936 },
        };

        foreach (var era in laterTangEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Later Tang".ToLowerInvariant(),
                LocalizedDynastyName = loc.LaterTang,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }

        var laterJinEras = new[]
        {
            new { EraName = loc.EraTianfuLJ, Start = 936, End = 944 },
            new { EraName = loc.EraKaiyun, Start = 944, End = 947 },
        };

        foreach (var era in laterJinEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Later Jin".ToLowerInvariant(),
                LocalizedDynastyName = loc.LaterJin,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }

        var laterHanEras = new[]
        {
            new { EraName = loc.EraTianfuLH, Start = 947, End = 948 },
            new { EraName = loc.EraQianyou, Start = 948, End = 950 },
        };

        foreach (var era in laterHanEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Later Han".ToLowerInvariant(),
                LocalizedDynastyName = loc.LaterHan,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }

        var laterZhouEras = new[]
        {
            new { EraName = loc.EraGuangshun, Start = 951, End = 953 },
            new { EraName = loc.EraXiande, Start = 954, End = 960 },
        };

        foreach (var era in laterZhouEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Later Zhou".ToLowerInvariant(),
                LocalizedDynastyName = loc.LaterZhou,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }
    }

    private void AddAncientDynastyEras(ChineseHistorical.ChineseHistoricalLocalizationBase loc)
    {
        _eras.Add(new EraData
        {
            DynastyKey = "Xia".ToLowerInvariant(),
            LocalizedDynastyName = loc.Xia,
            LocalizedEraName = loc.EraXiaDynasty,
            LocalizedNotes = loc.NoteXiaDynasty,
            StartYear = -2070,
            EndYear = -1600,
        });

        _eras.Add(new EraData
        {
            DynastyKey = "Shang".ToLowerInvariant(),
            LocalizedDynastyName = loc.Shang,
            LocalizedEraName = loc.EraEarlyShang,
            LocalizedNotes = loc.NoteEarlyShang,
            StartYear = -1600,
            EndYear = -1300,
        });

        _eras.Add(new EraData
        {
            DynastyKey = "Shang".ToLowerInvariant(),
            LocalizedDynastyName = loc.Shang,
            LocalizedEraName = loc.EraLateShang,
            LocalizedNotes = loc.NoteLateShang,
            StartYear = -1300,
            EndYear = -1046,
        });

        _eras.Add(new EraData
        {
            DynastyKey = "Zhou".ToLowerInvariant(),
            LocalizedDynastyName = loc.Zhou,
            LocalizedEraName = loc.EraWesternZhou,
            LocalizedNotes = loc.NoteWesternZhou,
            StartYear = -1046,
            EndYear = -771,
        });

        _eras.Add(new EraData
        {
            DynastyKey = "Zhou".ToLowerInvariant(),
            LocalizedDynastyName = loc.Zhou,
            LocalizedEraName = loc.EraSpringAndAutumn,
            LocalizedNotes = loc.NoteSpringAndAutumn,
            StartYear = -770,
            EndYear = -476,
        });

        _eras.Add(new EraData
        {
            DynastyKey = "Zhou".ToLowerInvariant(),
            LocalizedDynastyName = loc.Zhou,
            LocalizedEraName = loc.EraWarringStates,
            LocalizedNotes = loc.NoteWarringStates,
            StartYear = -475,
            EndYear = -248,
        });
    }

    private void AddQinDynastyEras(ChineseHistorical.ChineseHistoricalLocalizationBase loc)
    {
        _eras.Add(new EraData
        {
            DynastyKey = "Qin".ToLowerInvariant(),
            LocalizedDynastyName = loc.Qin,
            LocalizedEraName = loc.EraKingZheng,
            LocalizedNotes = loc.NoteKingZheng,
            StartYear = -247,
            EndYear = -222,
        });

        _eras.Add(new EraData
        {
            DynastyKey = "Qin".ToLowerInvariant(),
            LocalizedDynastyName = loc.Qin,
            LocalizedEraName = loc.EraQinShiHuang,
            LocalizedNotes = loc.NoteQinShiHuang,
            StartYear = -221,
            EndYear = -210,
            FirstYear = 26,
        });

        _eras.Add(new EraData
        {
            DynastyKey = "Qin".ToLowerInvariant(),
            LocalizedDynastyName = loc.Qin,
            LocalizedEraName = loc.EraQinErShi,
            LocalizedNotes = loc.NoteQinErShi,
            StartYear = -209,
            EndYear = -207,
        });

        _eras.Add(new EraData
        {
            DynastyKey = "Qin".ToLowerInvariant(),
            LocalizedDynastyName = loc.Qin,
            LocalizedEraName = loc.EraZiying,
            LocalizedNotes = loc.NoteZiying,
            StartYear = -206,
            EndYear = -206,
        });
    }

    private void AddWesternHanEras(ChineseHistorical.ChineseHistoricalLocalizationBase loc)
    {
        _eras.Add(new EraData
        {
            DynastyKey = "Han".ToLowerInvariant(),
            LocalizedDynastyName = loc.Han,
            LocalizedEraName = loc.EraChuHanContention,
            LocalizedNotes = loc.NoteChuHanContention,
            StartYear = -206,
            EndYear = -202,
        });

        _eras.Add(new EraData
        {
            DynastyKey = "Han".ToLowerInvariant(),
            LocalizedDynastyName = loc.Han,
            LocalizedEraName = loc.EraGaozu,
            LocalizedNotes = loc.NoteGaozu,
            StartYear = -202,
            EndYear = -195,
        });

        _eras.Add(new EraData
        {
            DynastyKey = "Han".ToLowerInvariant(),
            LocalizedDynastyName = loc.Han,
            LocalizedEraName = loc.EraHuidi,
            LocalizedNotes = loc.NoteHuidi,
            StartYear = -194,
            EndYear = -188,
        });

        _eras.Add(new EraData
        {
            DynastyKey = "Han".ToLowerInvariant(),
            LocalizedDynastyName = loc.Han,
            LocalizedEraName = loc.EraEmpressLü,
            LocalizedNotes = loc.NoteEmpressLü,
            StartYear = -187,
            EndYear = -180,
        });

        _eras.Add(new EraData
        {
            DynastyKey = "Han".ToLowerInvariant(),
            LocalizedDynastyName = loc.Han,
            LocalizedEraName = loc.EraWendi,
            LocalizedNotes = loc.NoteWendi,
            StartYear = -179,
            EndYear = -157,
        });

        _eras.Add(new EraData
        {
            DynastyKey = "Han".ToLowerInvariant(),
            LocalizedDynastyName = loc.Han,
            LocalizedEraName = loc.EraJingdi,
            LocalizedNotes = loc.NoteJingdi,
            StartYear = -156,
            EndYear = -141,
        });

        var wudiEras = new[]
        {
            new { EraName = loc.EraJianyuan, Start = -140, End = -135 },
            new { EraName = loc.EraYuanguang, Start = -134, End = -129 },
            new { EraName = loc.EraYuanshuo, Start = -128, End = -123 },
            new { EraName = loc.EraYuanshou, Start = -122, End = -117 },
            new { EraName = loc.EraYuanding, Start = -116, End = -111 },
            new { EraName = loc.EraYuanfengHan, Start = -110, End = -105 },
            new { EraName = loc.EraTaichu, Start = -104, End = -101 },
            new { EraName = loc.EraTianhan, Start = -100, End = -97 },
            new { EraName = loc.EraTaishiHan, Start = -96, End = -93 },
            new { EraName = loc.EraZhengheHan, Start = -92, End = -89 },
            new { EraName = loc.EraHouyuan, Start = -88, End = -87 },
        };

        foreach (var era in wudiEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Han".ToLowerInvariant(),
                LocalizedDynastyName = loc.Han,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }

        var westernHanEras = new[]
        {
            new { EraName = loc.EraShiyuan, Start = -86, End = -81 },
            new { EraName = loc.EraYuanfengHan2, Start = -80, End = -75 },
            new { EraName = loc.EraYuanping, Start = -74, End = -74 },
            new { EraName = loc.EraBenshi, Start = -73, End = -70 },
            new { EraName = loc.EraDijie, Start = -69, End = -66 },
            new { EraName = loc.EraYuankangHan, Start = -65, End = -62 },
            new { EraName = loc.EraShenjue, Start = -61, End = -58 },
            new { EraName = loc.EraWufengHan, Start = -57, End = -54 },
            new { EraName = loc.EraGanluHan, Start = -53, End = -50 },
            new { EraName = loc.EraHuanglongHan, Start = -49, End = -49 },
            new { EraName = loc.EraChuyuan, Start = -48, End = -44 },
            new { EraName = loc.EraYongguang, Start = -43, End = -39 },
            new { EraName = loc.EraJianzhao, Start = -38, End = -34 },
            new { EraName = loc.EraJingning, Start = -33, End = -33 },
            new { EraName = loc.EraJianshi, Start = -32, End = -29 },
            new { EraName = loc.EraHepingHan, Start = -28, End = -25 },
            new { EraName = loc.EraYangshuo, Start = -24, End = -21 },
            new { EraName = loc.EraHongjia, Start = -20, End = -17 },
            new { EraName = loc.EraYongshi, Start = -16, End = -13 },
            new { EraName = loc.EraYuanyan, Start = -12, End = -9 },
            new { EraName = loc.EraSuihe, Start = -8, End = -7 },
            new { EraName = loc.EraJianping, Start = -6, End = -3 },
            new { EraName = loc.EraYuanshouHan, Start = -2, End = 1 },
            new { EraName = loc.EraYuanshi, Start = 1, End = 5 },
            new { EraName = loc.EraJushe, Start = 6, End = 8 },
        };

        foreach (var era in westernHanEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Han".ToLowerInvariant(),
                LocalizedDynastyName = loc.Han,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }
    }

    private void AddXinDynastyEras(ChineseHistorical.ChineseHistoricalLocalizationBase loc)
    {
        var xinEras = new[]
        {
            new { EraName = loc.EraShijianguo, Start = 9, End = 13 },
            new { EraName = loc.EraTianfeng, Start = 14, End = 19 },
            new { EraName = loc.EraDihuang, Start = 20, End = 23 },
        };

        foreach (var era in xinEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Xin".ToLowerInvariant(),
                LocalizedDynastyName = loc.Xin,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }

        _eras.Add(new EraData
        {
            DynastyKey = "Han".ToLowerInvariant(),
            LocalizedDynastyName = loc.Han,
            LocalizedEraName = loc.EraGengshi,
            LocalizedNotes = loc.NoteGengshi,
            StartYear = 23,
            EndYear = 25,
        });
    }

    private void AddEasternHanEras(ChineseHistorical.ChineseHistoricalLocalizationBase loc)
    {
        var easternHanEras = new[]
        {
            new { EraName = loc.EraJianwuEH, Start = 25, End = 56 },
            new { EraName = loc.EraJianwuzhongyuan, Start = 56, End = 57 },
            new { EraName = loc.EraYongpingEH, Start = 58, End = 75 },
            new { EraName = loc.EraJianchu, Start = 76, End = 84 },
            new { EraName = loc.EraYuanheEH, Start = 84, End = 87 },
            new { EraName = loc.EraZhanghe, Start = 87, End = 88 },
            new { EraName = loc.EraYongyuan, Start = 89, End = 105 },
            new { EraName = loc.EraYuanxingEH, Start = 105, End = 105 },
            new { EraName = loc.EraYanping, Start = 106, End = 106 },
            new { EraName = loc.EraYongchu, Start = 107, End = 113 },
            new { EraName = loc.EraYuanchu, Start = 114, End = 120 },
            new { EraName = loc.EraYongning, Start = 120, End = 121 },
            new { EraName = loc.EraJianguang, Start = 121, End = 122 },
            new { EraName = loc.EraYanguang, Start = 122, End = 125 },
            new { EraName = loc.EraYongjian, Start = 126, End = 132 },
            new { EraName = loc.EraYangjia, Start = 132, End = 135 },
            new { EraName = loc.EraYongheEH, Start = 136, End = 141 },
            new { EraName = loc.EraHanan, Start = 142, End = 144 },
            new { EraName = loc.EraYongjiaEH, Start = 145, End = 145 },
            new { EraName = loc.EraBenchu, Start = 146, End = 146 },
            new { EraName = loc.EraJianhe, Start = 147, End = 149 },
            new { EraName = loc.EraHepingEH, Start = 150, End = 150 },
            new { EraName = loc.EraYuanjia, Start = 151, End = 153 },
            new { EraName = loc.EraYongxingEH, Start = 153, End = 154 },
            new { EraName = loc.EraYongshou, Start = 155, End = 158 },
            new { EraName = loc.EraYanxi, Start = 158, End = 167 },
            new { EraName = loc.EraYongkang, Start = 167, End = 167 },
            new { EraName = loc.EraJianning, Start = 168, End = 172 },
            new { EraName = loc.EraXiping, Start = 172, End = 178 },
            new { EraName = loc.EraGuanghe, Start = 178, End = 184 },
            new { EraName = loc.EraZhongping, Start = 184, End = 189 },
            new { EraName = loc.EraGuangxiEH, Start = 189, End = 189 },
            new { EraName = loc.EraZhaoning, Start = 189, End = 189 },
            new { EraName = loc.EraYonghan, Start = 189, End = 189 },
            new { EraName = loc.EraChuping, Start = 190, End = 193 },
            new { EraName = loc.EraXingping, Start = 194, End = 195 },
            new { EraName = loc.EraJianAn, Start = 196, End = 220 },
            new { EraName = loc.EraYankang, Start = 220, End = 220 },
        };

        foreach (var era in easternHanEras)
        {
            _eras.Add(new EraData
            {
                DynastyKey = "Han".ToLowerInvariant(),
                LocalizedDynastyName = loc.Han,
                LocalizedEraName = era.EraName,
                StartYear = era.Start,
                EndYear = era.End
            });
        }
    }
}
