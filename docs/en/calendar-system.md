# Calendar System

[English](../en/calendar-system.md) | [中文](../zh-CN/calendar-system.md) | [繁體中文](../zh-HK/calendar-system.md) | [Español](../es-ES/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | [Deutsch](../de-DE/calendar-system.md) | [Čeština](../cs-CZ/calendar-system.md)

## Overview

SiliconLifeCollective supports 32 different calendar systems for date conversion and historical accuracy.

## Supported Calendars (32 total)

### Primary Calendars (6)

1. **Gregorian** - International standard calendar, ID: `gregorian`
2. **Chinese Lunar** - Traditional Chinese calendar with leap month calculation, ID: `lunar`
3. **Islamic** - Islamic Hijri calendar, ID: `islamic`
4. **Hebrew** - Jewish calendar, ID: `hebrew`
5. **Persian** - Iranian solar calendar, ID: `persian`
6. **Indian** - Indian national calendar, ID: `indian`

### Chinese Historical Calendars (2)

7. **Chinese Historical** - Supports Ganzhi year system and imperial era names, ID: `chinese_historical`
   - **Ganzhi Year System**: 60-year cycle (Heavenly Stems + Earthly Branches)
   - **Imperial Era Names**: Supports era names from Chinese historical dynasties (Kangxi, Qianlong, Zhenguan, etc.)
   - **Dynamic Database**: Built-in complete database of Chinese historical dynasties and era names
8. **Sexagenary** - 60-year Ganzhi cycle, ID: `sexagenary`

### East Asian Calendars (6)

9. **Japanese** - Japanese era (Nengo) calendar, ID: `japanese`
10. **Vietnamese** - Vietnamese lunar calendar (Cat zodiac variant), ID: `vietnamese`
11. **Tibetan** - Tibetan calendar system, ID: `tibetan`
12. **Mongolian** - Mongolian calendar, ID: `mongolian`
13. **Dai** - Dai calendar with complete lunar calculations, ID: `dai`
14. **Dehong Dai** - Dehong Dai calendar variant, ID: `dehong_dai`

### Historical Calendars (6)

15. **Mayan** - Mayan Long Count calendar, ID: `mayan`
16. **Roman** - Ancient Roman calendar, ID: `roman`
17. **Julian** - Julian calendar, ID: `julian`
18. **French Republican** - French revolutionary calendar, ID: `french_republican`
19. **Coptic** - Coptic Orthodox calendar, ID: `coptic`
20. **Ethiopian** - Ethiopian calendar, ID: `ethiopian`

### Regional Calendars (6)

21. **Buddhist** - Buddhist Era (BE), year + 543, ID: `buddhist`
22. **Saka** - Saka Era (Indonesia), ID: `saka`
23. **Vikram Samvat** - Hindu calendar, ID: `vikram_samvat`
24. **Javanese** - Javanese Islamic calendar, ID: `javanese`
25. **Chula Sakarat** - Southeast Asian Buddhist calendar, year - 638, ID: `chula_sakarat`
26. **Khmer** - Khmer calendar, ID: `khmer`

### Modern Calendars (3)

27. **ROC** - Republic of China (Minguo) calendar, year - 1911, ID: `roc`
28. **Juche** - North Korean calendar, year - 1911, ID: `juche`
29. **Zoroastrian** - Zoroastrian calendar, ID: `zoroastrian`

### Ethnic Calendars (3)

30. **Yi** - Yi calendar system, ID: `yi`
31. **Cherokee** - Cherokee calendar, ID: `cherokee`
32. **Inuit** - Inuit calendar, ID: `inuit`

---

## Using the Calendar Tool

### Basic Conversion

```json
{
  "action": "convert",
  "date": "2026-04-20",
  "fromCalendar": "gregorian",
  "toCalendar": "chinese_lunar"
}
```

**Response**:
```json
{
  "result": "Lunar calendar Bingwu year, fourth month, third day",
  "year": 2026,
  "month": 4,
  "day": 3,
  "leapMonth": false
}
```

### Multi-Calendar Query

```json
{
  "action": "query_all",
  "date": "2026-04-20"
}
```

**Response**: Returns the date in all 32 calendar systems.

---

## Calendar API

### CalendarBase Interface

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

### Example: Custom Calendar

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Conversion logic
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Reverse conversion
        return new GregorianDate(year, month, day);
    }
}
```

---

## Special Features

### Chinese Historical Calendar Details (New)

The Chinese Historical Calendar is a highlight of this system, supporting two core features:

#### 1. Ganzhi Year System

Uses a 60-year cycle composed of Heavenly Stems and Earthly Branches:

```
Heavenly Stems (10): Jia, Yi, Bing, Ding, Wu, Ji, Geng, Xin, Ren, Gui
Earthly Branches (12): Zi, Chou, Yin, Mao, Chen, Si, Wu, Wei, Shen, You, Xu, Hai
```

**Examples**:
- 2026 = Bingwu year
- 2025 = Yisi year (Year of the Snake)
- 2024 = Jiachen year (Year of the Dragon)

**Usage Example**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**Response**:
```json
{
  "result": "Bingwu year, third month, ninth day",
  "ganzhi_year": "Bingwu",
  "zodiac": "Horse"
}
```

#### 2. Imperial Era Name System

Built-in complete database of Chinese historical dynasties and imperial era names:

**Supported Dynasties** (partial):
- Qing Dynasty: Kangxi, Yongzheng, Qianlong, Jiaqing, Daoguang, Xianfeng, Tongzhi, Guangxu, Xuantong
- Ming Dynasty: Hongwu, Yongle, Jiajing, Wanli, Chongzhen, etc.
- Tang Dynasty: Zhenguan, Kaiyuan, Tianbao, etc.
- Han Dynasty: Jianyuan, Yuanguang, Yuanshuo, etc.
- Other dynasties...

**Usage Example**:
```json
{
  "action": "convert",
  "date": "1721-04-12",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**Response**:
```json
{
  "result": "60th year of Kangxi, third month, fifteenth day",
  "era": "Kangxi",
  "era_year": 60,
  "dynasty": "Qing"
}
```

### Leap Month Handling

Calendars with leap months:
- Chinese Lunar
- Hebrew
- Buddhist
- Vietnamese

```json
{
  "isLeapMonth": true,
  "month": 4,
  "note": "Leap fourth month"
}
```

---

## Calendar Accuracy

### Astronomical Calculations

- Based on actual astronomical data
- Supports historical dates
- Handles calendar reforms

### Known Limitations

- Some ancient dates are approximate
- Calendar reforms vary by region
- Does not include leap second handling

---

## Use Cases

### Historical Research

Convert historical dates to modern calendars:

```
Q: "When was the French Revolution?"
A: "July 14, 1789 (Gregorian)"
   "26 Thermidor, Year 1 (French Republican)"
```

### Cultural Applications

Support for traditional festivals:

```
2026 Chinese New Year:
- Gregorian: February 17, 2026
- Lunar: First month, first day
```

### Multi-Cultural Scheduling

Schedule events that respect multiple calendars:

```
Meeting: 2026-04-20
- Avoid Islamic Friday prayers
- Respect Jewish Sabbath
- Consider Chinese holidays
```

---

## Best Practices

### 1. Always Specify Calendar

Never assume the calendar system:

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // Specify explicitly!
}
```

### 2. Handle Invalid Dates

Some dates don't exist in certain calendars:

```json
{
  "error": "Invalid date: February 30 doesn't exist"
}
```

### 3. Consider Time Zones

Date conversion may vary by time zone:

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## Next Steps

- 📚 Read the [Architecture Guide](architecture.md)
- 🛠️ Check the [Development Guide](development-guide.md)
- 🔧 See the [Tools Reference](tools-reference.md)
- 🚀 Get started with the [Quick Start Guide](getting-started.md)
