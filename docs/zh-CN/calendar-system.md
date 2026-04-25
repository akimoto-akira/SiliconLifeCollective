# 日历系统

[English](calendar-system.md) | [中文](docs/zh-CN/calendar-system.md) | [繁體中文](docs/zh-HK/calendar-system.md) | [Español](docs/es-ES/calendar-system.md) | [日本語](docs/ja-JP/calendar-system.md) | [한국어](docs/ko-KR/calendar-system.md) | [Čeština](docs/cs-CZ/calendar-system.md)

## 概述

SiliconLifeCollective 支持 32 种不同的日历系统，用于日期转换和历史准确性。

## 支持的日历

### 主要日历

1. **公历** - 国际标准日历
2. **农历** - 中国传统日历
3. **伊斯兰历** - 伊斯兰回历
4. **希伯来历** - 犹太日历
5. **波斯历** - 伊朗日历
6. **印度历** - 印度国历

### 东亚日历

7. **中国历史历** - 中国历史朝代
8. **日本历** - 日本 imperial 朝代
9. **韩国历** - 韩国日历系统
10. **越南历** - 越南日历
11. **藏历** - 藏历
12. **蒙古历** - 蒙古历

### 历史日历

13. **玛雅历** - 玛雅长计历
14. **罗马历** - 古罗马日历
15. **儒略历** - 儒略历
16. **法国共和历** - 法国革命日历
17. **科普特历** - 科普特正教日历
18. **埃塞俄比亚历** - 埃塞俄比亚历

### 区域日历

19. **佛历** - 佛历纪元
20. **萨卡历** - 萨卡纪元（印度）
21. **维克拉姆桑巴特历** - 印度教日历
22. **爪哇历** - 爪哇日历
23. **巴厘历** - 巴厘日历

### 现代日历

24. **中华民国（台湾）历** - 民国历
25. **主体历** - 朝鲜日历
26. **朱拉萨卡拉特历** - 东南亚佛历

### 民族日历（中国）

27. **傣历** - 傣历
28. **德宏傣历** - 德宏傣历
29. **彝历** - 彝历
30. **切罗基历** - 切罗基历
31. **因纽特历** - 因纽特历
32. **干支历** - 60 年周期

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

### 中国历史朝代

支持动态朝代数据库：

```json
{
  "era": "康熙",
  "year": 60,
  "month": 3,
  "day": 15
}
```

转换为：`1721-04-12`（公历）

### 干支周期

60 年周期，带天干地支：

```
天干：甲、乙、丙、丁、戊、己、庚、辛、壬、癸
地支：子、丑、寅、卯、辰、巳、午、未、申、酉、戌、亥
```

示例：2026 = 丙午年

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

## 故障排除

### 转换返回错误日期

**检查**：
- 指定的日历正确
- 日期对该日历有效
- 注意到历史准确性

### 闰月问题

**检查**：
- 日历支持闰月
- 年份确实有闰月
- 闰月计算正确

### 未找到历史朝代

**检查**：
- 朝代名称拼写
- 朝代日期范围
- 数据库包含该朝代

---

## 下一步

- 📚 阅读[架构指南](architecture.md)
- 🛠️ 查看[开发指南](development-guide.md)
- 🔧 查看[工具参考](tools-reference.md)
- 🚀 开始使用[快速开始指南](getting-started.md)
