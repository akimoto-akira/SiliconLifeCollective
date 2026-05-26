# 日曆系統

> **版本：v0.2.0-alpha**

[English](../en/calendar-system.md) | [Deutsch](../de-DE/calendar-system.md) | [中文](../zh-CN/calendar-system.md) | **繁體中文** | [Español](../es-ES/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | [Čeština](../cs-CZ/calendar-system.md) | [Русский](../ru-RU/calendar-system.md)

## 概述

SiliconLifeCollective 支援 32 種不同的日曆系統，用於日期轉換和歷史準確性。

## 支援的日曆（32 種）

### 主要日曆（6 種）

1. **公曆 (Gregorian)** - 國際標準日曆，ID: `gregorian`
2. **農曆 (Chinese Lunar)** - 中國傳統日曆，帶閏月計算，ID: `lunar`
3. **伊斯蘭曆 (Islamic)** - 伊斯蘭回曆，ID: `islamic`
4. **希伯來曆 (Hebrew)** - 猶太日曆，ID: `hebrew`
5. **波斯曆 (Persian)** - 伊朗太陽曆，ID: `persian`
6. **印度曆 (Indian)** - 印度國曆，ID: `indian`

### 中國歷史曆法（2 種）

7. **中國歷史曆法 (Chinese Historical)** - 支援干支紀年和帝王年號，ID: `chinese_historical`
   - **干支紀年**：60 年週期（天干 + 地支）
   - **帝王年號**：支援中國歷史各朝代年號（康熙、乾隆、貞觀等）
   - **動態資料庫**：內建完整的中國歷史朝代和年號資料庫
8. **干支曆 (Sexagenary)** - 60 年干支週期，ID: `sexagenary`

### 東亞日曆（6 種）

9. **日本曆 (Japanese)** - 日本年號（Nengo）曆，ID: `japanese`
10. **越南曆 (Vietnamese)** - 越南農曆（貓生肖變體），ID: `vietnamese`
11. **藏曆 (Tibetan)** - 藏曆系統，ID: `tibetan`
12. **蒙古曆 (Mongolian)** - 蒙古曆，ID: `mongolian`
13. **傣曆 (Dai)** - 傣曆，帶完整農曆計算，ID: `dai`
14. **德宏傣曆 (Dehong Dai)** - 德宏傣曆變體，ID: `dehong_dai`

### 歷史日曆（6 種）

15. **瑪雅曆 (Mayan)** - 瑪雅長計曆，ID: `mayan`
16. **羅馬曆 (Roman)** - 古羅馬日曆，ID: `roman`
17. **儒略曆 (Julian)** - 儒略曆，ID: `julian`
18. **法國共和曆 (French Republican)** - 法國革命日曆，ID: `french_republican`
19. **科普特曆 (Coptic)** - 科普特正教日曆，ID: `coptic`
20. **衣索比亞曆 (Ethiopian)** - 衣索比亞曆，ID: `ethiopian`

### 區域日曆（6 種）

21. **佛曆 (Buddhist)** - 佛曆紀元（BE），年份 + 543，ID: `buddhist`
22. **薩卡曆 (Saka)** - 薩卡紀元（印度尼西亞），ID: `saka`
23. **維克拉姆桑巴特曆 (Vikram Samvat)** - 印度教日曆，ID: `vikram_samvat`
24. **爪哇曆 (Javanese)** - 爪哇伊斯蘭曆，ID: `javanese`
25. **朱拉薩卡拉特曆 (Chula Sakarat)** - 東南亞佛曆，年份 - 638，ID: `chula_sakarat`
26. **高棉曆 (Khmer)** - 高棉曆，ID: `khmer`

### 現代日曆（3 種）

27. **中華民國曆 (ROC)** - 民國曆，年份 - 1911，ID: `roc`
28. **主體曆 (Juche)** - 朝鮮日曆，年份 - 1911，ID: `juche`
29. **祆曆 (Zoroastrian)** - 祆教曆，ID: `zoroastrian`

### 民族日曆（3 種）

30. **彝曆 (Yi)** - 彝曆系統，ID: `yi`
31. **切羅基曆 (Cherokee)** - 切羅基曆，ID: `cherokee`
32. **因紐特曆 (Inuit)** - 因紐特曆，ID: `inuit`

---

## 使用日曆工具

### 基本轉換

```json
{
  "action": "convert",
  "date": "2026-04-20",
  "fromCalendar": "gregorian",
  "toCalendar": "chinese_lunar"
}
```

**回應**：
```json
{
  "result": "農曆丙午年四月初三",
  "year": 2026,
  "month": 4,
  "day": 3,
  "leapMonth": false
}
```

### 多日曆查詢

```json
{
  "action": "query_all",
  "date": "2026-04-20"
}
```

**回應**：傳回所有 32 種日曆系統中的日期。

---

## 日曆 API

### CalendarBase 介面

```csharp
public abstract class CalendarBase
{
    public abstract string Name { get; }

    public abstract CalendarDate ConvertFromGregorian(GregorianDate date);

    public abstract GregorianDate ConvertToGregorian(CalendarDate date);

    public virtual bool IsLeapYear(int year) => false;

    public virtual int GetDaysInMonth(int year, int month) => 30;
}
```

### 範例：自訂日曆

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";

    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // 轉換邏輯
        return new CalendarDate(year, month, day);
    }

    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // 反向轉換
        return new GregorianDate(year, month, day);
    }
}
```

---

## 特殊功能

### 中國歷史曆法詳解（新增）

中國歷史曆法是本系統的一大亮點，支援兩種核心功能：

#### 1. 干支紀年系統

採用 60 年週期，由天干和地支組合而成：

```
天干（10）：甲、乙、丙、丁、戊、己、庚、辛、壬、癸
地支（12）：子、丑、寅、卯、辰、巳、午、未、申、酉、戌、亥
```

**範例**：
- 2026 年 = 丙午年
- 2025 年 = 乙巳年（蛇年）
- 2024 年 = 甲辰年（龍年）

**使用範例**：
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**回應**：
```json
{
  "result": "丙午年 三月 初九",
  "ganzhi_year": "丙午",
  "zodiac": "馬"
}
```

#### 2. 帝王年號系統

內建完整的中國歷史朝代和帝王年號資料庫：

**支援的朝代**（部分）：
- 清朝：康熙、雍正、乾隆、嘉慶、道光、咸豐、同治、光緒、宣統
- 明朝：洪武、永樂、嘉靖、萬曆、崇禎等
- 唐朝：貞觀、開元、天寶等
- 漢朝：建元、元光、元朔等
- 其他朝代...

**使用範例**：
```json
{
  "action": "convert",
  "date": "1721-04-12",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**回應**：
```json
{
  "result": "康熙六十年 三月 十五日",
  "era": "康熙",
  "era_year": 60,
  "dynasty": "清"
}
```

### 閏月處理

帶閏月的日曆：
- 農曆
- 希伯來曆
- 佛曆
- 越南曆

```json
{
  "isLeapMonth": true,
  "month": 4,
  "note": "閏四月"
}
```

---

## 日曆準確性

### 天文計算

- 基於實際天文資料
- 支援歷史日期
- 處理日曆改革

### 已知限制

- 一些古代日期是近似的
- 日曆改革因地區而異
- 不包含閏秒處理

---

## 使用案例

### 歷史研究

將歷史日期轉換為現代日曆：

```
問：「法國大革命是什麼時候？」
答：「1789 年 7 月 14 日（公曆）」
   「元年熱月 26 日（法國共和曆）」
```

### 文化應用

支援傳統節日：

```
2026 年春節：
- 公曆：2026 年 2 月 17 日
- 農曆：正月初一
```

### 多文化排程

安排尊重多個日曆的事件：

```
會議：2026-04-20
- 避開伊斯蘭週五祈禱
- 尊重猶太安息日
- 考慮中國假期
```

---

## 最佳實踐

### 1. 始終指定日曆

永遠不要假設日曆系統：

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // 明確指定！
}
```

### 2. 處理無效日期

某些日期在某些日曆中不存在：

```json
{
  "error": "Invalid date: February 30 doesn't exist"
}
```

### 3. 考慮時區

日期轉換可能因時區而異：

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## 下一步

- 📚 閱讀[架構指南](architecture.md)
- 🛠️ 檢視[開發指南](development-guide.md)
- 🔧 檢視[工具參考](tools-reference.md)
- 🚀 開始使用[快速開始指南](getting-started.md)
