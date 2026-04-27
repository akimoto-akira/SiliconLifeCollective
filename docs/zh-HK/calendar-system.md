# 日歷系統

> **版本：v0.1.0-alpha**

[English](../en/calendar-system.md) | [中文](../zh-CN/calendar-system.md) | **繁體中文** | [Español](../es-ES/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | [Čeština](../cs-CZ/calendar-system.md)

## 概述

SiliconLifeCollective 支援 32 种不同的日歷系統，用於日期转换和歷史准确性。

## 支援的日歷

### 主要日歷

1. **公歷** - 国际標準日歷
2. **農歷** - 中国傳统日歷
3. **伊斯蘭歷** - 伊斯蘭回歷
4. **希伯來歷** - 犹太日歷
5. **波斯歷** - 伊朗日歷
6. **印度歷** - 印度国歷

### 東亞日歷

7. **中国歷史歷** - 中国歷史朝代
8. **日本歷** - 日本 imperial 朝代
9. **韩国歷** - 韩国日歷系統
10. **越南歷** - 越南日歷
11. **藏歷** - 藏歷
12. **蒙古歷** - 蒙古歷

### 歷史日歷

13. **瑪雅歷** - 瑪雅長計歷
14. **罗馬歷** - 古罗馬日歷
15. **儒略歷** - 儒略歷
16. **法国共和歷** - 法国革命日歷
17. **科普特歷** - 科普特正教日歷
18. **埃塞俄比亞歷** - 埃塞俄比亞歷

### 區域日歷

19. **佛歷** - 佛歷紀元
20. **萨卡歷** - 萨卡紀元（印度）
21. **维克拉姆桑巴特歷** - 印度教日歷
22. **爪哇歷** - 爪哇日歷
23. **巴厘歷** - 巴厘日歷

### 现代日歷

24. **中華民国（臺湾）歷** - 民国歷
25. **主体歷** - 朝鮮日歷
26. **朱拉萨卡拉特歷** - 東南亞佛歷

### 民族日歷（中国）

27. **傣歷** - 傣歷
28. **德宏傣歷** - 德宏傣歷
29. **彝歷** - 彝歷
30. **切罗基歷** - 切罗基歷
31. **因纽特歷** - 因纽特歷
32. **幹支歷** - 60 年周期

---

## 使用日歷工具

### 基本转换

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
  "result": "農歷丙午年四月初三",
  "year": 2026,
  "month": 4,
  "day": 3,
  "leapMonth": false
}
```

### 多日歷查詢

```json
{
  "action": "query_all",
  "date": "2026-04-20"
}
```

**回應**：返回所有 32 种日歷系統中的日期。

---

## 日歷 API

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

### 示例：自定義日歷

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

### 中国歷史朝代

支援動态朝代資料庫：

```json
{
  "era": "康熙",
  "year": 60,
  "month": 3,
  "day": 15
}
```

转换為：`1721-04-12`（公歷）

### 幹支周期

60 年周期，带天幹地支：

```
天幹：甲、乙、丙、丁、戊、己、庚、辛、壬、癸
地支：子、丑、寅、卯、辰、巳、午、未、申、酉、戌、亥
```

示例：2026 = 丙午年

### 闰月處理

带闰月的日歷：
- 農歷
- 希伯來歷
- 佛歷
- 越南歷

```json
{
  "isLeapMonth": true,
  "month": 4,
  "note": "闰四月"
}
```

---

## 日歷准确性

### 天文計算

- 基於实际天文資料
- 支援歷史日期
- 處理日歷改革

### 已知限制

- 一些古代日期是近似的
- 日歷改革因地區而異
- 不包含闰秒處理

---

## 用例

### 歷史研究

将歷史日期转换為现代日歷：

```
問："法国大革命是什麼时候？"
答："1789 年 7 月 14 日（公歷）"
   "元年热月 26 日（法国共和歷）"
```

### 文化應用程式

支援傳统節日：

```
2026 年春節：
- 公歷：2026 年 2 月 17 日
- 農歷：正月初一
```

### 多文化排程

安排尊重多個日歷的事件：

```
會議：2026-04-20
- 避開伊斯蘭周五祈祷
- 尊重犹太安息日
- 考虑中国假期
```

---

## 最佳实践

### 1. 始终指定日歷

永遠不要假設日歷系統：

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // 明确指定！
}
```

### 2. 處理无效日期

某些日期在某些日歷中不存在：

```json
{
  "error": "Invalid date: February 30 doesn't exist"
}
```

### 3. 考虑时區

日期转换可能因时區而異：

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## 故障排除

### 转换返回錯誤日期

**檢查**：
- 指定的日歷正确
- 日期對该日歷有效
- 注意到歷史准确性

### 闰月問題

**檢查**：
- 日歷支援闰月
- 年份确实有闰月
- 闰月計算正确

### 未找到歷史朝代

**檢查**：
- 朝代名称拼寫
- 朝代日期范围
- 資料庫包含该朝代

---

## 下一步

- 📚 阅读[架構指南](architecture.md)
- 🛠️ 查看[開發指南](development-guide.md)
- 🔧 查看[工具参考](tools-reference.md)
- 🚀 開始使用[快速開始指南](getting-started.md)
