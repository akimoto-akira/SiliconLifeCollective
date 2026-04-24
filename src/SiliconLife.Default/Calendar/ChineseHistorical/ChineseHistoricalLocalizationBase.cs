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

namespace SiliconLife.Default.ChineseHistorical;

/// <summary>
/// Abstract base class for Chinese historical dynasty and era localization.
/// 
/// This class defines abstract properties for all Chinese dynasties (28 total) 
/// and all era names (296 total) from the database, ensuring complete translations.
/// 
/// Dynasty Coverage: Xia, Shang, Zhou, Qin, Han, Xin, Wei, Shu, Wu, Jin, 
///                   Northern/Southern Dynasties, Sui, Tang, Five Dynasties, 
///                   Song, Yuan, Ming, Qing, ROC, PRC
/// 
/// Era Coverage: All 296 era names from Xia Dynasty to Common Era
/// </summary>
public abstract class ChineseHistoricalLocalizationBase
{
    #region Dynasty Names (28 Dynasties)

    // Ancient Dynasties
    /// <summary>夏朝 (Xia Dynasty, c. 2070-1600 BCE)</summary>
    public abstract string Xia { get; }
    /// <summary>商朝 (Shang Dynasty, c. 1600-1046 BCE)</summary>
    public abstract string Shang { get; }
    /// <summary>周朝 (Zhou Dynasty, c. 1046-256 BCE)</summary>
    public abstract string Zhou { get; }

    // Imperial China - Early Period
    /// <summary>秦朝 (Qin Dynasty, 221-206 BCE)</summary>
    public abstract string Qin { get; }
    /// <summary>汉朝 (Han Dynasty, 206 BCE-220 CE)</summary>
    public abstract string Han { get; }
    /// <summary>新朝 (Xin Dynasty, 9-23 CE)</summary>
    public abstract string Xin { get; }

    // Three Kingdoms Period
    /// <summary>曹魏 (Wei Dynasty, 220-266 CE)</summary>
    public abstract string Wei { get; }
    /// <summary>蜀汉 (Shu Han, 221-263 CE)</summary>
    public abstract string Shu { get; }
    /// <summary>东吴 (Eastern Wu, 222-280 CE)</summary>
    public abstract string Wu { get; }

    // Jin Dynasty
    /// <summary>晋朝 (Jin Dynasty, 265-420 CE)</summary>
    public abstract string Jin { get; }

    // Southern and Northern Dynasties
    /// <summary>北魏 (Northern Wei, 386-534 CE)</summary>
    public abstract string NorthernWei { get; }
    /// <summary>东魏 (Eastern Wei, 534-550 CE)</summary>
    public abstract string EasternWei { get; }
    /// <summary>西魏 (Western Wei, 535-556 CE)</summary>
    public abstract string WesternWei { get; }
    /// <summary>北齐 (Northern Qi, 550-577 CE)</summary>
    public abstract string NorthernQi { get; }
    /// <summary>北周 (Northern Zhou, 557-581 CE)</summary>
    public abstract string NorthernZhou { get; }

    // Medieval Dynasties
    /// <summary>隋朝 (Sui Dynasty, 581-618 CE)</summary>
    public abstract string Sui { get; }
    /// <summary>唐朝 (Tang Dynasty, 618-907 CE)</summary>
    public abstract string Tang { get; }

    // Five Dynasties and Ten Kingdoms
    /// <summary>后梁 (Later Liang, 907-923 CE)</summary>
    public abstract string LaterLiang { get; }
    /// <summary>后唐 (Later Tang, 923-936 CE)</summary>
    public abstract string LaterTang { get; }
    /// <summary>后晋 (Later Jin, 936-947 CE)</summary>
    public abstract string LaterJin { get; }
    /// <summary>后汉 (Later Han, 947-950 CE)</summary>
    public abstract string LaterHan { get; }
    /// <summary>后周 (Later Zhou, 951-960 CE)</summary>
    public abstract string LaterZhou { get; }

    // Song, Yuan, Ming, Qing
    /// <summary>宋朝 (Song Dynasty, 960-1279 CE)</summary>
    public abstract string Song { get; }
    /// <summary>元朝 (Yuan Dynasty, 1271-1368 CE)</summary>
    public abstract string Yuan { get; }
    /// <summary>明朝 (Ming Dynasty, 1368-1644 CE)</summary>
    public abstract string Ming { get; }
    /// <summary>清朝 (Qing Dynasty, 1644-1912 CE)</summary>
    public abstract string Qing { get; }

    // Modern Era
    /// <summary>中华民国 (Republic of China, 1912-1949)</summary>
    public abstract string ROC { get; }
    /// <summary>中华人民共和国 (People's Republic of China, 1949-present)</summary>
    public abstract string PRC { get; }

    #endregion

    #region Era Names - Xia Dynasty

    /// <summary>夏朝时期 (Xia Dynasty Period)</summary>
    public abstract string EraXiaDynasty { get; }

    #endregion

    #region Era Names - Shang Dynasty

    /// <summary>商朝早期 (Early Shang)</summary>
    public abstract string EraEarlyShang { get; }
    /// <summary>商朝晚期 (Late Shang / Yin Period)</summary>
    public abstract string EraLateShang { get; }

    #endregion

    #region Era Names - Zhou Dynasty

    /// <summary>西周 (Western Zhou)</summary>
    public abstract string EraWesternZhou { get; }
    /// <summary>春秋时期 (Spring and Autumn Period)</summary>
    public abstract string EraSpringAndAutumn { get; }
    /// <summary>战国时期 (Warring States Period)</summary>
    public abstract string EraWarringStates { get; }

    #endregion

    #region Era Names - Qin Dynasty

    /// <summary>秦王政时期 (King Zheng of Qin)</summary>
    public abstract string EraKingZheng { get; }
    /// <summary>秦始皇 (Qin Shi Huang)</summary>
    public abstract string EraQinShiHuang { get; }
    /// <summary>秦二世 (Qin Er Shi)</summary>
    public abstract string EraQinErShi { get; }
    /// <summary>子婴 (Ziying)</summary>
    public abstract string EraZiying { get; }

    #endregion

    #region Era Names - Han Dynasty (Western Han)

    /// <summary>楚汉相争 (Chu-Han Contention)</summary>
    public abstract string EraChuHanContention { get; }
    /// <summary>高祖 (Gaozu / Liu Bang)</summary>
    public abstract string EraGaozu { get; }
    /// <summary>惠帝 (Huidi)</summary>
    public abstract string EraHuidi { get; }
    /// <summary>吕后 (Empress Lü)</summary>
    public abstract string EraEmpressLü { get; }
    /// <summary>文帝 (Wendi)</summary>
    public abstract string EraWendi { get; }
    /// <summary>景帝 (Jingdi)</summary>
    public abstract string EraJingdi { get; }

    #endregion

    #region Era Names - Han Dynasty (Emperor Wu of Han)

    /// <summary>建元 (Jianyuan)</summary>
    public abstract string EraJianyuan { get; }
    /// <summary>元光 (Yuanguang)</summary>
    public abstract string EraYuanguang { get; }
    /// <summary>元朔 (Yuanshuo)</summary>
    public abstract string EraYuanshuo { get; }
    /// <summary>元狩 (Yuanshou)</summary>
    public abstract string EraYuanshou { get; }
    /// <summary>元鼎 (Yuanding)</summary>
    public abstract string EraYuanding { get; }
    /// <summary>元封 (Yuanfeng)</summary>
    public abstract string EraYuanfengHan { get; }
    /// <summary>太初 (Taichu)</summary>
    public abstract string EraTaichu { get; }
    /// <summary>天汉 (Tianhan)</summary>
    public abstract string EraTianhan { get; }
    /// <summary>太始 (Taishi)</summary>
    public abstract string EraTaishiHan { get; }
    /// <summary>征和 (Zhenghe)</summary>
    public abstract string EraZhengheHan { get; }
    /// <summary>后元 (Houyuan)</summary>
    public abstract string EraHouyuan { get; }

    #endregion

    #region Era Names - Han Dynasty (Later Western Han)

    /// <summary>始元 (Shiyuan)</summary>
    public abstract string EraShiyuan { get; }
    /// <summary>元凤 (Yuanfeng)</summary>
    public abstract string EraYuanfengHan2 { get; }
    /// <summary>元平 (Yuanping)</summary>
    public abstract string EraYuanping { get; }
    /// <summary>本始 (Benshi)</summary>
    public abstract string EraBenshi { get; }
    /// <summary>地节 (Dijie)</summary>
    public abstract string EraDijie { get; }
    /// <summary>元康 (Yuankang)</summary>
    public abstract string EraYuankangHan { get; }
    /// <summary>神爵 (Shenjue)</summary>
    public abstract string EraShenjue { get; }
    /// <summary>五凤 (Wufeng)</summary>
    public abstract string EraWufengHan { get; }
    /// <summary>甘露 (Ganlu)</summary>
    public abstract string EraGanluHan { get; }
    /// <summary>黄龙 (Huanglong)</summary>
    public abstract string EraHuanglongHan { get; }
    /// <summary>初元 (Chuyuan)</summary>
    public abstract string EraChuyuan { get; }
    /// <summary>永光 (Yongguang)</summary>
    public abstract string EraYongguang { get; }
    /// <summary>建昭 (Jianzhao)</summary>
    public abstract string EraJianzhao { get; }
    /// <summary>竟宁 (Jingning)</summary>
    public abstract string EraJingning { get; }
    /// <summary>建始 (Jianshi)</summary>
    public abstract string EraJianshi { get; }
    /// <summary>河平 (Heping)</summary>
    public abstract string EraHepingHan { get; }
    /// <summary>阳朔 (Yangshuo)</summary>
    public abstract string EraYangshuo { get; }
    /// <summary>鸿嘉 (Hongjia)</summary>
    public abstract string EraHongjia { get; }
    /// <summary>永始 (Yongshi)</summary>
    public abstract string EraYongshi { get; }
    /// <summary>元延 (Yuanyan)</summary>
    public abstract string EraYuanyan { get; }
    /// <summary>绥和 (Suihe)</summary>
    public abstract string EraSuihe { get; }
    /// <summary>建平 (Jianping)</summary>
    public abstract string EraJianping { get; }
    /// <summary>元寿 (Yuanshou)</summary>
    public abstract string EraYuanshouHan { get; }
    /// <summary>元始 (Yuanshi)</summary>
    public abstract string EraYuanshi { get; }
    /// <summary>居摄 (Jushe)</summary>
    public abstract string EraJushe { get; }

    #endregion

    #region Era Names - Xin Dynasty

    /// <summary>始建国 (Shijianguo)</summary>
    public abstract string EraShijianguo { get; }
    /// <summary>天凤 (Tianfeng)</summary>
    public abstract string EraTianfeng { get; }
    /// <summary>地皇 (Dihuang)</summary>
    public abstract string EraDihuang { get; }

    #endregion

    #region Era Names - Han Dynasty (Gengshi)

    /// <summary>更始 (Gengshi)</summary>
    public abstract string EraGengshi { get; }

    #endregion

    #region Era Names - Eastern Han Dynasty

    /// <summary>建武 (Jianwu)</summary>
    public abstract string EraJianwuEH { get; }
    /// <summary>建武中元 (Jianwuzhongyuan)</summary>
    public abstract string EraJianwuzhongyuan { get; }
    /// <summary>永平 (Yongping)</summary>
    public abstract string EraYongpingEH { get; }
    /// <summary>建初 (Jianchu)</summary>
    public abstract string EraJianchu { get; }
    /// <summary>元和 (Yuanhe)</summary>
    public abstract string EraYuanheEH { get; }
    /// <summary>章和 (Zhanghe)</summary>
    public abstract string EraZhanghe { get; }
    /// <summary>永元 (Yongyuan)</summary>
    public abstract string EraYongyuan { get; }
    /// <summary>元兴 (Yuanxing)</summary>
    public abstract string EraYuanxingEH { get; }
    /// <summary>延平 (Yanping)</summary>
    public abstract string EraYanping { get; }
    /// <summary>永初 (Yongchu)</summary>
    public abstract string EraYongchu { get; }
    /// <summary>元初 (Yuanchu)</summary>
    public abstract string EraYuanchu { get; }
    /// <summary>永宁 (Yongning)</summary>
    public abstract string EraYongning { get; }
    /// <summary>建光 (Jianguang)</summary>
    public abstract string EraJianguang { get; }
    /// <summary>延光 (Yanguang)</summary>
    public abstract string EraYanguang { get; }
    /// <summary>永建 (Yongjian)</summary>
    public abstract string EraYongjian { get; }
    /// <summary>阳嘉 (Yangjia)</summary>
    public abstract string EraYangjia { get; }
    /// <summary>永和 (Yonghe)</summary>
    public abstract string EraYongheEH { get; }
    /// <summary>汉安 (Hanan)</summary>
    public abstract string EraHanan { get; }
    /// <summary>永嘉 (Yongjia)</summary>
    public abstract string EraYongjiaEH { get; }
    /// <summary>本初 (Benchu)</summary>
    public abstract string EraBenchu { get; }
    /// <summary>建和 (Jianhe)</summary>
    public abstract string EraJianhe { get; }
    /// <summary>和平 (Heping)</summary>
    public abstract string EraHepingEH { get; }
    /// <summary>元嘉 (Yuanjia)</summary>
    public abstract string EraYuanjia { get; }
    /// <summary>永兴 (Yongxing)</summary>
    public abstract string EraYongxingEH { get; }
    /// <summary>永寿 (Yongshou)</summary>
    public abstract string EraYongshou { get; }
    /// <summary>延熹 (Yanxi)</summary>
    public abstract string EraYanxi { get; }
    /// <summary>永康 (Yongkang)</summary>
    public abstract string EraYongkang { get; }
    /// <summary>建宁 (Jianning)</summary>
    public abstract string EraJianning { get; }
    /// <summary>熹平 (Xiping)</summary>
    public abstract string EraXiping { get; }
    /// <summary>光和 (Guanghe)</summary>
    public abstract string EraGuanghe { get; }
    /// <summary>中平 (Zhongping)</summary>
    public abstract string EraZhongping { get; }
    /// <summary>光熹 (Guangxi)</summary>
    public abstract string EraGuangxiEH { get; }
    /// <summary>昭宁 (Zhaoning)</summary>
    public abstract string EraZhaoning { get; }
    /// <summary>永汉 (Yonghan)</summary>
    public abstract string EraYonghan { get; }
    /// <summary>初平 (Chuping)</summary>
    public abstract string EraChuping { get; }
    /// <summary>兴平 (Xingping)</summary>
    public abstract string EraXingping { get; }
    /// <summary>建安 (Jian'an)</summary>
    public abstract string EraJianAn { get; }
    /// <summary>延康 (Yankang)</summary>
    public abstract string EraYankang { get; }

    #endregion

    #region Era Names - Wei Dynasty (Three Kingdoms)

    /// <summary>黄初 (Huangchu)</summary>
    public abstract string EraHuangchu { get; }
    /// <summary>太和 (Taihe)</summary>
    public abstract string EraTaiheWei { get; }
    /// <summary>青龙 (Qinglong)</summary>
    public abstract string EraQinglong { get; }
    /// <summary>景初 (Jingchu)</summary>
    public abstract string EraJingchu { get; }
    /// <summary>正始 (Zhengshi)</summary>
    public abstract string EraZhengshi { get; }
    /// <summary>嘉平 (Jiaping)</summary>
    public abstract string EraJiaping { get; }
    /// <summary>正元 (Zhengyuan)</summary>
    public abstract string EraZhengyuan { get; }
    /// <summary>甘露 (Ganlu)</summary>
    public abstract string EraGanluWei { get; }
    /// <summary>景元 (Jingyuan)</summary>
    public abstract string EraJingyuan { get; }
    /// <summary>咸熙 (Xianxi)</summary>
    public abstract string EraXianxi { get; }

    #endregion

    #region Era Names - Shu Han (Three Kingdoms)

    /// <summary>章武 (Zhangwu)</summary>
    public abstract string EraZhangwu { get; }
    /// <summary>建兴 (Jianxing)</summary>
    public abstract string EraJianxingSH { get; }
    /// <summary>延熙 (Yanxi)</summary>
    public abstract string EraYanxiSH { get; }
    /// <summary>景耀 (Jingyao)</summary>
    public abstract string EraJingyao { get; }

    #endregion

    #region Era Names - Eastern Wu (Three Kingdoms)

    /// <summary>黄武 (Huangwu)</summary>
    public abstract string EraHuangwu { get; }
    /// <summary>黄龙 (Huanglong)</summary>
    public abstract string EraHuanglongWu { get; }
    /// <summary>嘉禾 (Jiahe)</summary>
    public abstract string EraJiahe { get; }
    /// <summary>赤乌 (Chiwu)</summary>
    public abstract string EraChiwu { get; }
    /// <summary>太元 (Taiyuan)</summary>
    public abstract string EraTaiyuanWu { get; }
    /// <summary>神凤 (Shenfeng)</summary>
    public abstract string EraShenfeng { get; }
    /// <summary>建兴 (Jianxing)</summary>
    public abstract string EraJianxingWu { get; }
    /// <summary>五凤 (Wufeng)</summary>
    public abstract string EraWufengWu { get; }
    /// <summary>太平 (Taiping)</summary>
    public abstract string EraTaipingWu { get; }
    /// <summary>永安 (Yongan)</summary>
    public abstract string EraYonganWu { get; }
    /// <summary>元兴 (Yuanxing)</summary>
    public abstract string EraYuanxingWu { get; }
    /// <summary>甘露 (Ganlu)</summary>
    public abstract string EraGanluWu { get; }
    /// <summary>宝鼎 (Baoding)</summary>
    public abstract string EraBaoding { get; }
    /// <summary>建衡 (Jianheng)</summary>
    public abstract string EraJianheng { get; }
    /// <summary>凤凰 (Fenghuang)</summary>
    public abstract string EraFenghuang { get; }
    /// <summary>天册 (Tiance)</summary>
    public abstract string EraTianceWu { get; }
    /// <summary>天玺 (Tianxi) - Eastern Wu</summary>
    public abstract string EraTianxiWu { get; }
    /// <summary>天纪 (Tianji)</summary>
    public abstract string EraTianji { get; }

    #endregion

    #region Era Names - Western Jin Dynasty

    /// <summary>泰始 (Taishi)</summary>
    public abstract string EraTaishiJin { get; }
    /// <summary>咸宁 (Xianning)</summary>
    public abstract string EraXianning { get; }
    /// <summary>太康 (Taikang)</summary>
    public abstract string EraTaikang { get; }
    /// <summary>太熙 (Taixi)</summary>
    public abstract string EraTaixi { get; }
    /// <summary>永熙 (Yongxi)</summary>
    public abstract string EraYongxi { get; }
    /// <summary>永平 (Yongping)</summary>
    public abstract string EraYongpingJin { get; }
    /// <summary>元康 (Yuankang)</summary>
    public abstract string EraYuankangJin { get; }
    /// <summary>永康 (Yongkang)</summary>
    public abstract string EraYongkangJin { get; }
    /// <summary>永宁 (Yongan)</summary>
    public abstract string EraYonganJin { get; }
    /// <summary>太安 (Taan)</summary>
    public abstract string EraTaan { get; }
    /// <summary>永兴 (Yongxing)</summary>
    public abstract string EraYongxingJin { get; }
    /// <summary>光熙 (Guangxi)</summary>
    public abstract string EraGuangxiJin { get; }
    /// <summary>永嘉 (Yongjia)</summary>
    public abstract string EraYongjiaJin { get; }
    /// <summary>建兴 (Jianxing)</summary>
    public abstract string EraJianxingJin { get; }

    #endregion

    #region Era Names - Eastern Jin Dynasty

    /// <summary>建武 (Jianwu)</summary>
    public abstract string EraJianwuEJ { get; }
    /// <summary>太兴 (Taixing)</summary>
    public abstract string EraTaixing { get; }
    /// <summary>永昌 (Yongchang)</summary>
    public abstract string EraYongchang { get; }
    /// <summary>太宁 (Taining)</summary>
    public abstract string EraTaining { get; }
    /// <summary>咸和 (Xianhe)</summary>
    public abstract string EraXianhe { get; }
    /// <summary>咸康 (Xiankang)</summary>
    public abstract string EraXiankang { get; }
    /// <summary>建元 (Jianyuan)</summary>
    public abstract string EraJianyuanEJ { get; }
    /// <summary>永和 (Yonghe)</summary>
    public abstract string EraYongheEJ { get; }
    /// <summary>升平 (Shengping)</summary>
    public abstract string EraShengping { get; }
    /// <summary>隆和 (Longhe)</summary>
    public abstract string EraLonghe { get; }
    /// <summary>兴宁 (Xingning)</summary>
    public abstract string EraXingning { get; }
    /// <summary>太和 (Taihe)</summary>
    public abstract string EraTaiheEJ { get; }
    /// <summary>咸安 (Xian'an)</summary>
    public abstract string EraXianAn { get; }
    /// <summary>宁康 (Ningkang)</summary>
    public abstract string EraNingkang { get; }
    /// <summary>太元 (Taiyuan)</summary>
    public abstract string EraTaiyuanEJ { get; }
    /// <summary>隆安 (Longan)</summary>
    public abstract string EraLongan { get; }
    /// <summary>元兴 (Yuanxing)</summary>
    public abstract string EraYuanxingEJ { get; }
    /// <summary>义熙 (Yixi)</summary>
    public abstract string EraYixi { get; }
    /// <summary>元熙 (Yuanxi)</summary>
    public abstract string EraYuanxi { get; }

    #endregion

    #region Era Names - Southern and Northern Dynasties

    /// <summary>北魏时期 (Northern Wei Period)</summary>
    public abstract string EraNorthernWeiPeriod { get; }
    /// <summary>东魏时期 (Eastern Wei Period)</summary>
    public abstract string EraEasternWeiPeriod { get; }
    /// <summary>西魏时期 (Western Wei Period)</summary>
    public abstract string EraWesternWeiPeriod { get; }
    /// <summary>北齐时期 (Northern Qi Period)</summary>
    public abstract string EraNorthernQiPeriod { get; }
    /// <summary>北周时期 (Northern Zhou Period)</summary>
    public abstract string EraNorthernZhouPeriod { get; }

    #endregion

    #region Era Names - Sui Dynasty

    /// <summary>开皇 (Kaihuang)</summary>
    public abstract string EraKaihuang { get; }
    /// <summary>仁寿 (Renshou)</summary>
    public abstract string EraRenshou { get; }
    /// <summary>大业 (Daye)</summary>
    public abstract string EraDaye { get; }

    #endregion

    #region Era Names - Tang Dynasty

    /// <summary>武德 (Wude)</summary>
    public abstract string EraWude { get; }
    /// <summary>贞观 (Zhenguan)</summary>
    public abstract string EraZhenguan { get; }
    /// <summary>永徽 (Yonghui)</summary>
    public abstract string EraYonghui { get; }
    /// <summary>显庆 (Xianqing)</summary>
    public abstract string EraXianqing { get; }
    /// <summary>龙朔 (Longshuo)</summary>
    public abstract string EraLongshuo { get; }
    /// <summary>麟德 (Linde)</summary>
    public abstract string EraLinde { get; }
    /// <summary>乾封 (Qianfeng)</summary>
    public abstract string EraQianfeng { get; }
    /// <summary>总章 (Zongzhang)</summary>
    public abstract string EraZongzhang { get; }
    /// <summary>咸亨 (Xianheng)</summary>
    public abstract string EraXianheng { get; }
    /// <summary>上元 (Shangyuan)</summary>
    public abstract string EraShangyuanTang { get; }
    /// <summary>仪凤 (Yifeng)</summary>
    public abstract string EraYifeng { get; }
    /// <summary>调露 (Tiaolu)</summary>
    public abstract string EraTiaolu { get; }
    /// <summary>永隆 (Yonglong)</summary>
    public abstract string EraYonglong { get; }
    /// <summary>开耀 (Kaiyao)</summary>
    public abstract string EraKaiyao { get; }
    /// <summary>永淳 (Yongchun)</summary>
    public abstract string EraYongchun { get; }
    /// <summary>弘道 (Hongdao)</summary>
    public abstract string EraHongdao { get; }
    /// <summary>嗣圣 (Sisheng)</summary>
    public abstract string EraSisheng { get; }
    /// <summary>文明 (Wenming)</summary>
    public abstract string EraWenming { get; }
    /// <summary>光宅 (Guangzhai)</summary>
    public abstract string EraGuangzhai { get; }
    /// <summary>垂拱 (Chuigong)</summary>
    public abstract string EraChuigong { get; }
    /// <summary>永昌 (Yongchang)</summary>
    public abstract string EraYongchangTang { get; }
    /// <summary>天授 (Tianshou)</summary>
    public abstract string EraTianshou { get; }
    /// <summary>如意 (Ruyi)</summary>
    public abstract string EraRuyi { get; }
    /// <summary>长寿 (Changshou)</summary>
    public abstract string EraChangshou { get; }
    /// <summary>延载 (Yanzai)</summary>
    public abstract string EraYanzai { get; }
    /// <summary>证圣 (Zhengsheng)</summary>
    public abstract string EraZhengsheng { get; }
    /// <summary>天册万岁 (Tiancewansui)</summary>
    public abstract string EraTiancewansui { get; }
    /// <summary>万岁通天 (Wansuitongtian)</summary>
    public abstract string EraWansuitongtian { get; }
    /// <summary>神功 (Shengong)</summary>
    public abstract string EraShengong { get; }
    /// <summary>圣历 (Shengli)</summary>
    public abstract string EraShengli { get; }
    /// <summary>久视 (Jiushi)</summary>
    public abstract string EraJiushi { get; }
    /// <summary>大足 (Dazu)</summary>
    public abstract string EraDazu { get; }
    /// <summary>长安 (Chang'an)</summary>
    public abstract string EraChangAn { get; }
    /// <summary>神龙 (Shenlong)</summary>
    public abstract string EraShenlong { get; }
    /// <summary>景龙 (Jinglong)</summary>
    public abstract string EraJinglong { get; }
    /// <summary>景云 (Jingyun)</summary>
    public abstract string EraJingyun { get; }
    /// <summary>太极 (Taiji)</summary>
    public abstract string EraTaiji { get; }
    /// <summary>延和 (Yanhe)</summary>
    public abstract string EraYanhe { get; }
    /// <summary>先天 (Xiantian)</summary>
    public abstract string EraXiantian { get; }
    /// <summary>开元 (Kaiyuan)</summary>
    public abstract string EraKaiyuan { get; }
    /// <summary>天宝 (Tianbao)</summary>
    public abstract string EraTianbao { get; }
    /// <summary>至德 (Zhide)</summary>
    public abstract string EraZhide { get; }
    /// <summary>乾元 (Qianyuan)</summary>
    public abstract string EraQianyuan { get; }
    /// <summary>上元 (Shangyuan)</summary>
    public abstract string EraShangyuanTang2 { get; }
    /// <summary>宝应 (Baoying)</summary>
    public abstract string EraBaoying { get; }
    /// <summary>广德 (Guangde)</summary>
    public abstract string EraGuangdeTang { get; }
    /// <summary>永泰 (Yongtai)</summary>
    public abstract string EraYongtai { get; }
    /// <summary>大历 (Dali)</summary>
    public abstract string EraDali { get; }
    /// <summary>建中 (Jianzhong)</summary>
    public abstract string EraJianzhongTang { get; }
    /// <summary>兴元 (Xingyuan)</summary>
    public abstract string EraXingyuan { get; }
    /// <summary>贞元 (Zhenyuan)</summary>
    public abstract string EraZhenyuan { get; }
    /// <summary>永贞 (Yongzhen)</summary>
    public abstract string EraYongzhen { get; }
    /// <summary>元和 (Yuanhe)</summary>
    public abstract string EraYuanheTang { get; }
    /// <summary>长庆 (Changqing)</summary>
    public abstract string EraChangqing { get; }
    /// <summary>宝历 (Baoli)</summary>
    public abstract string EraBaoli { get; }
    /// <summary>大和 (Dahe)</summary>
    public abstract string EraDahe { get; }
    /// <summary>开成 (Kaicheng)</summary>
    public abstract string EraKaicheng { get; }
    /// <summary>会昌 (Huichang)</summary>
    public abstract string EraHuichang { get; }
    /// <summary>大中 (Dazhong)</summary>
    public abstract string EraDazhong { get; }
    /// <summary>咸通 (Xiantong)</summary>
    public abstract string EraXiantong { get; }
    /// <summary>乾符 (Qianfu)</summary>
    public abstract string EraQianfu { get; }
    /// <summary>广明 (Guangming)</summary>
    public abstract string EraGuangming { get; }
    /// <summary>中和 (Zhonghe)</summary>
    public abstract string EraZhonghe { get; }
    /// <summary>光启 (Guangqi)</summary>
    public abstract string EraGuangqi { get; }
    /// <summary>文德 (Wende)</summary>
    public abstract string EraWende { get; }
    /// <summary>龙纪 (Longji)</summary>
    public abstract string EraLongji { get; }
    /// <summary>大顺 (Dashun)</summary>
    public abstract string EraDashun { get; }
    /// <summary>景福 (Jingfu)</summary>
    public abstract string EraJingfu { get; }
    /// <summary>乾宁 (Qianning)</summary>
    public abstract string EraQianning { get; }
    /// <summary>光化 (Guanghua)</summary>
    public abstract string EraGuanghua { get; }
    /// <summary>天复 (Tianfu)</summary>
    public abstract string EraTianfuTang { get; }
    /// <summary>天祐 (Tianyou)</summary>
    public abstract string EraTianyouTang { get; }

    #endregion

    #region Era Names - Later Liang (Five Dynasties)

    /// <summary>开平 (Kaiping)</summary>
    public abstract string EraKaiping { get; }
    /// <summary>乾化 (Qianhua)</summary>
    public abstract string EraQianhua { get; }
    /// <summary>凤历 (Fengli)</summary>
    public abstract string EraFengli { get; }
    /// <summary>乾化 (Qianhua2)</summary>
    public abstract string EraQianhua2 { get; }
    /// <summary>贞明 (Zhenming)</summary>
    public abstract string EraZhenming { get; }
    /// <summary>龙德 (Longde)</summary>
    public abstract string EraLongde { get; }

    #endregion

    #region Era Names - Later Tang (Five Dynasties)

    /// <summary>同光 (Tongguang)</summary>
    public abstract string EraTongguang { get; }
    /// <summary>天成 (Tiancheng)</summary>
    public abstract string EraTiancheng { get; }
    /// <summary>长兴 (Changxing)</summary>
    public abstract string EraChangxing { get; }
    /// <summary>应顺 (Yingshun)</summary>
    public abstract string EraYingshun { get; }
    /// <summary>清泰 (Qingtai)</summary>
    public abstract string EraQingtai { get; }

    #endregion

    #region Era Names - Later Jin (Five Dynasties)

    /// <summary>天福 (Tianfu)</summary>
    public abstract string EraTianfuLJ { get; }
    /// <summary>开运 (Kaiyun)</summary>
    public abstract string EraKaiyun { get; }

    #endregion

    #region Era Names - Later Han (Five Dynasties)

    /// <summary>天福 (Tianfu2)</summary>
    public abstract string EraTianfuLH { get; }
    /// <summary>乾祐 (Qianyou)</summary>
    public abstract string EraQianyou { get; }

    #endregion

    #region Era Names - Later Zhou (Five Dynasties)

    /// <summary>广顺 (Guangshun)</summary>
    public abstract string EraGuangshun { get; }
    /// <summary>显德 (Xiande)</summary>
    public abstract string EraXiande { get; }

    #endregion

    #region Era Names - Song Dynasty

    /// <summary>建隆 (Jianlong)</summary>
    public abstract string EraJianlong { get; }
    /// <summary>乾德 (Qiande)</summary>
    public abstract string EraQiande { get; }
    /// <summary>开宝 (Kaibao)</summary>
    public abstract string EraKaibao { get; }
    /// <summary>太平兴国 (Taipingxingguo)</summary>
    public abstract string EraTaipingxingguo { get; }
    /// <summary>雍熙 (Yongxi)</summary>
    public abstract string EraYongxiSong { get; }
    /// <summary>端拱 (Duangong)</summary>
    public abstract string EraDuangong { get; }
    /// <summary>淳化 (Chunhua)</summary>
    public abstract string EraChunhua { get; }
    /// <summary>至道 (Zhidao)</summary>
    public abstract string EraZhidao { get; }
    /// <summary>咸平 (Xianping)</summary>
    public abstract string EraXianping { get; }
    /// <summary>景德 (Jingde)</summary>
    public abstract string EraJingde { get; }
    /// <summary>大中祥符 (Dazhongxiangfu)</summary>
    public abstract string EraDazhongxiangfu { get; }
    /// <summary>天禧 (Tianxi) - Song Dynasty</summary>
    public abstract string EraTianxiSong { get; }
    /// <summary>乾兴 (Qianxing)</summary>
    public abstract string EraQianxing { get; }
    /// <summary>天圣 (Tiansheng)</summary>
    public abstract string EraTiansheng { get; }
    /// <summary>明道 (Mingdao)</summary>
    public abstract string EraMingdao { get; }
    /// <summary>景祐 (Jingyou)</summary>
    public abstract string EraJingyou { get; }
    /// <summary>宝元 (Baoyuan)</summary>
    public abstract string EraBaoyuan { get; }
    /// <summary>庆历 (Qingli)</summary>
    public abstract string EraQingli { get; }
    /// <summary>皇祐 (Huangyou)</summary>
    public abstract string EraHuangyou { get; }
    /// <summary>至和 (Zhihe)</summary>
    public abstract string EraZhiheSong { get; }
    /// <summary>嘉祐 (Jiayou)</summary>
    public abstract string EraJiayou { get; }
    /// <summary>治平 (Zhiping)</summary>
    public abstract string EraZhiping { get; }
    /// <summary>熙宁 (Xining)</summary>
    public abstract string EraXining { get; }
    /// <summary>元丰 (Yuanfeng)</summary>
    public abstract string EraYuanfengSong { get; }
    /// <summary>元祐 (Yuanyou)</summary>
    public abstract string EraYuanyou { get; }
    /// <summary>绍圣 (Shaosheng)</summary>
    public abstract string EraShaosheng { get; }
    /// <summary>元符 (Yuanfu)</summary>
    public abstract string EraYuanfu { get; }
    /// <summary>建中靖国 (Jianzhong)</summary>
    public abstract string EraJianzhongSong { get; }
    /// <summary>崇宁 (Chongning)</summary>
    public abstract string EraChongning { get; }
    /// <summary>大观 (Daguan)</summary>
    public abstract string EraDaguan { get; }
    /// <summary>政和 (Zhenghe)</summary>
    public abstract string EraZhengheSong { get; }
    /// <summary>重和 (Chonghe)</summary>
    public abstract string EraChonghe { get; }
    /// <summary>宣和 (Xuanhe)</summary>
    public abstract string EraXuanhe { get; }
    /// <summary>靖康 (Jingkang)</summary>
    public abstract string EraJingkang { get; }
    /// <summary>建炎 (Jianyan)</summary>
    public abstract string EraJianyan { get; }
    /// <summary>绍兴 (Shaoxing)</summary>
    public abstract string EraShaoxing { get; }
    /// <summary>隆兴 (Longxing)</summary>
    public abstract string EraLongxing { get; }
    /// <summary>乾道 (Qiandao)</summary>
    public abstract string EraQiandao { get; }
    /// <summary>淳熙 (Chunxi)</summary>
    public abstract string EraChunxi { get; }
    /// <summary>绍熙 (Shaoxi)</summary>
    public abstract string EraShaoxi { get; }
    /// <summary>庆元 (Qingyuan)</summary>
    public abstract string EraQingyuan { get; }
    /// <summary>嘉泰 (Jiatai)</summary>
    public abstract string EraJiatai { get; }
    /// <summary>开禧 (Kaixi)</summary>
    public abstract string EraKaixi { get; }
    /// <summary>嘉定 (Jiading)</summary>
    public abstract string EraJiading { get; }
    /// <summary>宝庆 (Baoqing)</summary>
    public abstract string EraBaoqing { get; }
    /// <summary>绍定 (Shaoding)</summary>
    public abstract string EraShaoding { get; }
    /// <summary>端平 (Duanping)</summary>
    public abstract string EraDuanping { get; }
    /// <summary>嘉熙 (Jiaxi)</summary>
    public abstract string EraJiaxi { get; }
    /// <summary>淳祐 (Chunyou)</summary>
    public abstract string EraChunyou { get; }
    /// <summary>宝祐 (Baoyou)</summary>
    public abstract string EraBaoyou { get; }
    /// <summary>开庆 (Kaiqing)</summary>
    public abstract string EraKaiqing { get; }
    /// <summary>景定 (Jingding)</summary>
    public abstract string EraJingding { get; }
    /// <summary>咸淳 (Xianchun)</summary>
    public abstract string EraXianchun { get; }
    /// <summary>德祐 (Deyou)</summary>
    public abstract string EraDeyou { get; }
    /// <summary>景炎 (Jingyan)</summary>
    public abstract string EraJingyan { get; }
    /// <summary>祥兴 (Xiangxing)</summary>
    public abstract string EraXiangxing { get; }

    #endregion

    #region Era Names - Yuan Dynasty

    /// <summary>至元 (Zhiyuan)</summary>
    public abstract string EraZhiyuan { get; }
    /// <summary>元贞 (Yuanzhen)</summary>
    public abstract string EraYuanzhen { get; }
    /// <summary>大德 (Dade)</summary>
    public abstract string EraDade { get; }
    /// <summary>至大 (Zhida)</summary>
    public abstract string EraZhida { get; }
    /// <summary>皇庆 (Huangqing)</summary>
    public abstract string EraHuangqing { get; }
    /// <summary>延祐 (Yanyou)</summary>
    public abstract string EraYanyou { get; }
    /// <summary>至治 (Zhizhi)</summary>
    public abstract string EraZhizhi { get; }
    /// <summary>泰定 (Taiding)</summary>
    public abstract string EraTaiding { get; }
    /// <summary>天顺 (Tianshun)</summary>
    public abstract string EraTianshunYuan { get; }
    /// <summary>致和 (Zhihe)</summary>
    public abstract string EraZhiheYuan { get; }
    /// <summary>天历 (Tianli)</summary>
    public abstract string EraTianli { get; }
    /// <summary>至顺 (Zhishun)</summary>
    public abstract string EraZhishun { get; }
    /// <summary>元统 (Yuantong)</summary>
    public abstract string EraYuantong { get; }
    /// <summary>至元 (Zhiyuan2)</summary>
    public abstract string EraZhiyuan2 { get; }
    /// <summary>至正 (Zhizheng)</summary>
    public abstract string EraZhizheng { get; }

    #endregion

    #region Era Names - Ming Dynasty

    /// <summary>洪武 (Hongwu)</summary>
    public abstract string EraHongwu { get; }
    /// <summary>建文 (Jianwen)</summary>
    public abstract string EraJianwen { get; }
    /// <summary>永乐 (Yongle)</summary>
    public abstract string EraYongle { get; }
    /// <summary>洪熙 (Hongxi)</summary>
    public abstract string EraHongxi { get; }
    /// <summary>宣德 (Xuande)</summary>
    public abstract string EraXuande { get; }
    /// <summary>正统 (Zhengtong)</summary>
    public abstract string EraZhengtong { get; }
    /// <summary>景泰 (Jingtai)</summary>
    public abstract string EraJingtai { get; }
    /// <summary>天顺 (Tianshun)</summary>
    public abstract string EraTianshunMing { get; }
    /// <summary>成化 (Chenghua)</summary>
    public abstract string EraChenghua { get; }
    /// <summary>弘治 (Hongzhi)</summary>
    public abstract string EraHongzhi { get; }
    /// <summary>正德 (Zhengde)</summary>
    public abstract string EraZhengde { get; }
    /// <summary>嘉靖 (Jiajing)</summary>
    public abstract string EraJiajing { get; }
    /// <summary>隆庆 (Longqing)</summary>
    public abstract string EraLongqing { get; }
    /// <summary>万历 (Wanli)</summary>
    public abstract string EraWanli { get; }
    /// <summary>泰昌 (Taichang)</summary>
    public abstract string EraTaichang { get; }
    /// <summary>天启 (Tianqi)</summary>
    public abstract string EraTianqi { get; }
    /// <summary>崇祯 (Chongzhen)</summary>
    public abstract string EraChongzhen { get; }

    #endregion

    #region Era Names - Qing Dynasty

    /// <summary>顺治 (Shunzhi)</summary>
    public abstract string EraShunzhi { get; }
    /// <summary>康熙 (Kangxi)</summary>
    public abstract string EraKangxi { get; }
    /// <summary>雍正 (Yongzheng)</summary>
    public abstract string EraYongzheng { get; }
    /// <summary>乾隆 (Qianlong)</summary>
    public abstract string EraQianlong { get; }
    /// <summary>嘉庆 (Jiaqing)</summary>
    public abstract string EraJiaqing { get; }
    /// <summary>道光 (Daoguang)</summary>
    public abstract string EraDaoguang { get; }
    /// <summary>咸丰 (Xianfeng)</summary>
    public abstract string EraXianfeng { get; }
    /// <summary>同治 (Tongzhi)</summary>
    public abstract string EraTongzhi { get; }
    /// <summary>光绪 (Guangxu)</summary>
    public abstract string EraGuangxu { get; }
    /// <summary>宣统 (Xuantong)</summary>
    public abstract string EraXuantong { get; }

    #endregion

    #region Era Names - Republic of China and Common Era

    /// <summary>民国 (Republic Era, 1912-1949)</summary>
    public abstract string EraRepublic { get; }
    /// <summary>公元 (Common Era, 1949-present)</summary>
    public abstract string EraCommonEra { get; }

    #endregion

    #region Era Notes

    /// <summary>Note: Republic of China era</summary>
    public abstract string NoteROC { get; }
    /// <summary>Note: People's Republic of China, ongoing</summary>
    public abstract string NotePRC { get; }
    /// <summary>Note: Simplified period (Northern Wei)</summary>
    public abstract string NoteNorthernWeiPeriod { get; }
    /// <summary>Note: Simplified period (Eastern Wei)</summary>
    public abstract string NoteEasternWeiPeriod { get; }
    /// <summary>Note: Simplified period (Western Wei)</summary>
    public abstract string NoteWesternWeiPeriod { get; }
    /// <summary>Note: Simplified period (Northern Qi)</summary>
    public abstract string NoteNorthernQiPeriod { get; }
    /// <summary>Note: Simplified period (Northern Zhou)</summary>
    public abstract string NoteNorthernZhouPeriod { get; }
    /// <summary>Note: Legendary first dynasty</summary>
    public abstract string NoteXiaDynasty { get; }
    /// <summary>Note: From Tang to Pan Geng</summary>
    public abstract string NoteEarlyShang { get; }
    /// <summary>Note: Yin period</summary>
    public abstract string NoteLateShang { get; }
    /// <summary>Note: King Wu to King You</summary>
    public abstract string NoteWesternZhou { get; }
    /// <summary>Note: Spring and Autumn Period</summary>
    public abstract string NoteSpringAndAutumn { get; }
    /// <summary>Note: Warring States Period</summary>
    public abstract string NoteWarringStates { get; }
    /// <summary>Note: Ying Zheng as King of Qin, before unification</summary>
    public abstract string NoteKingZheng { get; }
    /// <summary>Note: First Emperor of China, unified China in 221 BCE (Year 26)</summary>
    public abstract string NoteQinShiHuang { get; }
    /// <summary>Note: Second Emperor of Qin</summary>
    public abstract string NoteQinErShi { get; }
    /// <summary>Note: Last ruler of Qin</summary>
    public abstract string NoteZiying { get; }
    /// <summary>Note: Transition period between Qin and Han</summary>
    public abstract string NoteChuHanContention { get; }
    /// <summary>Note: Liu Bang, founder of Han Dynasty</summary>
    public abstract string NoteGaozu { get; }
    /// <summary>Note: Emperor Hui of Han</summary>
    public abstract string NoteHuidi { get; }
    /// <summary>Note: Empress Dowager Lü</summary>
    public abstract string NoteEmpressLü { get; }
    /// <summary>Note: Emperor Wen of Han</summary>
    public abstract string NoteWendi { get; }
    /// <summary>Note: Emperor Jing of Han</summary>
    public abstract string NoteJingdi { get; }
    /// <summary>Note: Gengshi Emperor, transition to Eastern Han</summary>
    public abstract string NoteGengshi { get; }

    #endregion
}
