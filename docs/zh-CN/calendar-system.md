# 日历系统

> **版本：v0.1.0-alpha**

[English](../en/calendar-system.md) | [Deutsch](../de-DE/calendar-system.md) | **中文** | [繁體中文](../zh-HK/calendar-system.md) | [Español](../es-ES/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | [Čeština](../cs-CZ/calendar-system.md)

## 概述

SiliconLifeCollective 支持 32 种不同的日历系统，用于日期转换和历史准确性。

## 支持的日历（32 种）

### 主要日历（6 种）

1. **公历 (Gregorian)** - 国际标准日历，ID: `gregorian`
2. **农历 (Chinese Lunar)** - 中国传统日历，带闰月计算，ID: `lunar`
3. **伊斯兰历 (Islamic)** - 伊斯兰回历，ID: `islamic`
4. **希伯来历 (Hebrew)** - 犹太日历，ID: `hebrew`
5. **波斯历 (Persian)** - 伊朗太阳历，ID: `persian`
6. **印度历 (Indian)** - 印度国历，ID: `indian`

### 中国历史历法（2 种）

7. **中国历史历法 (Chinese Historical)** - 支持干支纪年和帝王年号，ID: `chinese_historical`
   - **干支纪年**：60 年周期（天干 + 地支）
   - **帝王年号**：支持中国历史各朝代年号（康熙、乾隆、贞观等）
   - **动态数据库**：内置完整的中国历史朝代和年号数据库
8. **干支历 (Sexagenary)** - 60 年干支周期，ID: `sexagenary`

### 东亚日历（6 种）

9. **日本历 (Japanese)** - 日本年号（Nengo）历，ID: `japanese`
10. **越南历 (Vietnamese)** - 越南农历（猫生肖变体），ID: `vietnamese`
11. **藏历 (Tibetan)** - 藏历系统，ID: `tibetan`
12. **蒙古历 (Mongolian)** - 蒙古历，ID: `mongolian`
13. **傣历 (Dai)** - 傣历，带完整农历计算，ID: `dai`
14. **德宏傣历 (Dehong Dai)** - 德宏傣历变体，ID: `dehong_dai`

### 历史日历（6 种）

15. **玛雅历 (Mayan)** - 玛雅长计历，ID: `mayan`
16. **罗马历 (Roman)** - 古罗马日历，ID: `roman`
17. **儒略历 (Julian)** - 儒略历，ID: `julian`
18. **法国共和历 (French Republican)** - 法国革命日历，ID: `french_republican`
19. **科普特历 (Coptic)** - 科普特正教日历，ID: `coptic`
20. **埃塞俄比亚历 (Ethiopian)** - 埃塞俄比亚历，ID: `ethiopian`

### 区域日历（6 种）

21. **佛历 (Buddhist)** - 佛历纪元（BE），年份 + 543，ID: `buddhist`
22. **萨卡历 (Saka)** - 萨卡纪元（印度尼西亚），ID: `saka`
23. **维克拉姆桑巴特历 (Vikram Samvat)** - 印度教日历，ID: `vikram_samvat`
24. **爪哇历 (Javanese)** - 爪哇伊斯兰历，ID: `javanese`
25. **朱拉萨卡拉特历 (Chula Sakarat)** - 东南亚佛历，年份 - 638，ID: `chula_sakarat`
26. **高棉历 (Khmer)** - 高棉历，ID: `khmer`

### 现代日历（3 种）

27. **中华民国历 (ROC)** - 民国历，年份 - 1911，ID: `roc`
28. **主体历 (Juche)** - 朝鲜日历，年份 - 1911，ID: `juche`
29. **祆历 (Zoroastrian)** - 祆教历，ID: `zoroastrian`

### 民族日历（3 种）

30. **彝历 (Yi)** - 彝历系统，ID: `yi`
31. **切罗基历 (Cherokee)** - 切罗基历，ID: `cherokee`
32. **因纽特历 (Inuit)** - 因纽特历，ID: `inuit`

---

## 使用日历工具

### 基本转换

```json
{
  "action": "convert",
  "date": "2026-04-20",
  "fromCalendar": "gregorian",
  "toCalendar": "chinese_lunar"
}
```

**响应**：
```json
{
  "result": "农历丙午年四月初三",
  "year": 2026,
  "month": 4,
  "day": 3,
  "leapMonth": false
}
```

### 多日历查询

```json
{
  "action": "query_all",
  "date": "2026-04-20"
}
```

**响应**：返回所有 32 种日历系统中的日期。

---

## 日历 API

### CalendarBase 接口

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

### 示例：自定义日历

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // 转换逻辑
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // 反向转换
        return new GregorianDate(year, month, day);
    }
}
```

---

## 特殊功能

### 中国历史历法详解（新增）

中国历史历法是本系统的一大亮点，支持两种核心功能：

#### 1. 干支纪年系统

采用 60 年周期，由天干和地支组合而成：

```
天干（10）：甲、乙、丙、丁、戊、己、庚、辛、壬、癸
地支（12）：子、丑、寅、卯、辰、巳、午、未、申、酉、戌、亥
```

**示例**：
- 2026 年 = 丙午年
- 2025 年 = 乙巳年（蛇年）
- 2024 年 = 甲辰年（龙年）

**使用示例**：
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**响应**：
```json
{
  "result": "丙午年 三月 初九",
  "ganzhi_year": "丙午",
  "zodiac": "马"
}
```

#### 2. 帝王年号系统

内置完整的中国历史朝代和帝王年号数据库：

**支持的朝代**（部分）：
- 清朝：康熙、雍正、乾隆、嘉庆、道光、咸丰、同治、光绪、宣统
- 明朝：洪武、永乐、嘉靖、万历、崇祯等
- 唐朝：贞观、开元、天宝等
- 汉朝：建元、元光、元朔等
- 其他朝代...

**使用示例**：
```json
{
  "action": "convert",
  "date": "1721-04-12",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**响应**：
```json
{
  "result": "康熙六十年 三月 十五日",
  "era": "康熙",
  "era_year": 60,
  "dynasty": "清"
}
```

### 闰月处理

带闰月的日历：
- 农历
- 希伯来历
- 佛历
- 越南历

```json
{
  "isLeapMonth": true,
  "month": 4,
  "note": "闰四月"
}
```

---

## 日历准确性

### 天文计算

- 基于实际天文数据
- 支持历史日期
- 处理日历改革

### 已知限制

- 一些古代日期是近似的
- 日历改革因地区而异
- 不包含闰秒处理

---

## 用例

### 历史研究

将历史日期转换为现代日历：

```
问："法国大革命是什么时候？"
答："1789 年 7 月 14 日（公历）"
   "元年热月 26 日（法国共和历）"
```

### 文化应用

支持传统节日：

```
2026 年春节：
- 公历：2026 年 2 月 17 日
- 农历：正月初一
```

### 多文化调度

安排尊重多个日历的事件：

```
会议：2026-04-20
- 避开伊斯兰周五祈祷
- 尊重犹太安息日
- 考虑中国假期
```

---

## 最佳实践

### 1. 始终指定日历

永远不要假设日历系统：

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // 明确指定！
}
```

### 2. 处理无效日期

某些日期在某些日历中不存在：

```json
{
  "error": "Invalid date: February 30 doesn't exist"
}
```

### 3. 考虑时区

日期转换可能因时区而异：

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## 下一步

- 📚 阅读[架构指南](architecture.md)
- 🛠️ 查看[开发指南](development-guide.md)
- 🔧 查看[工具参考](tools-reference.md)
- 🚀 开始使用[快速开始指南](getting-started.md)
