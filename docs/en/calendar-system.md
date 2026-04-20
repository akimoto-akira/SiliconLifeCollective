# Calendar System

## Overview

SiliconLifeCollective supports 32 different calendar systems for date conversion and historical accuracy.

## Supported Calendars

### Major Calendars

1. **Gregorian** - International standard calendar
2. **Chinese Lunar** - Traditional Chinese calendar
3. **Islamic** - Islamic Hijri calendar
4. **Hebrew** - Jewish calendar
5. **Persian** - Iranian calendar
6. **Indian** - Indian national calendar

### East Asian Calendars

7. **Chinese Historical** - Historical Chinese eras
8. **Japanese** - Japanese imperial eras
9. **Korean** - Korean calendar systems
10. **Vietnamese** - Vietnamese calendar
11. **Tibetan** - Tibetan calendar
12. **Mongolian** - Mongolian calendar

### Historical Calendars

13. **Mayan** - Mayan long count
14. **Roman** - Ancient Roman calendar
15. **Julian** - Julian calendar
16. **French Republican** - French Revolution calendar
17. **Coptic** - Coptic Orthodox calendar
18. **Ethiopian** - Ethiopian calendar

### Regional Calendars

19. **Buddhist** - Buddhist era
20. **Saka** - Saka era (India)
21. **Vikram Samvat** - Hindu calendar
22. **Javanese** - Javanese calendar
23. **Balinese** - Balinese calendar

### Modern Calendars

24. **Republic of China (Taiwan)** - Minguo calendar
25. **Juche** - North Korean calendar
26. **Chula Sakarat** - Southeast Asian Buddhist

### Ethnic Calendars (China)

27. **Dai** - Dai calendar
28. **Dehong Dai** - Dehong Dai calendar
29. **Yi** - Yi calendar
30. **Cherokee** - Cherokee calendar
31. **Inuit** - Inuit calendar
32. **Sexagenary** - 60-year cycle

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
  "result": "农历丙午年四月初三",
  "year": 2026,
  "month": 4,
  "day": 3,
  "leapMonth": false
}
```

### Multiple Calendar Query

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

### Chinese Historical Eras

Supports dynamic era database:

```json
{
  "era": "康熙",
  "year": 60,
  "month": 3,
  "day": 15
}
```

Converts to: `1721-04-12` (Gregorian)

### Sexagenary Cycle

60-year cycle with heavenly stems and earthly branches:

```
天干: 甲、乙、丙、丁、戊、己、庚、辛、壬、癸
地支: 子、丑、寅、卯、辰、巳、午、未、申、酉、戌、亥
```

Example: 2026 = 丙午年

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
  "note": "闰四月"
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
- Leap second handling not included

---

## Use Cases

### Historical Research

Convert historical dates to modern calendar:

```
Q: "When was the French Revolution?"
A: "14 July 1789 (Gregorian)"
   "26 Messidor, Year I (French Republican)"
```

### Cultural Applications

Support traditional festivals:

```
Chinese New Year 2026:
- Gregorian: February 17, 2026
- Chinese Lunar: 正月初一
```

### Multi-Cultural Scheduling

Schedule events respecting multiple calendars:

```
Meeting: 2026-04-20
- Avoids Islamic Friday prayers
- Respects Jewish Sabbath
- Considers Chinese holidays
```

---

## Best Practices

### 1. Always Specify Calendar

Never assume the calendar system:

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // Explicit!
}
```

### 2. Handle Invalid Dates

Some dates don't exist in certain calendars:

```json
{
  "error": "Invalid date: February 30 doesn't exist"
}
```

### 3. Consider Timezones

Date conversion may vary by timezone:

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## Troubleshooting

### Conversion Returns Wrong Date

**Check**:
- Correct calendar specified
- Date is valid for that calendar
- Historical accuracy noted

### Leap Month Issues

**Check**:
- Calendar supports leap months
- Year actually has leap month
- Leap month calculation is correct

### Historical Era Not Found

**Check**:
- Era name spelling
- Era date range
- Database includes that era

---

## Next Steps

- 📚 Read the [Architecture Guide](architecture.md)
- 🛠️ Check the [Development Guide](development-guide.md)
- 🔧 See the [Tools Reference](tools-reference.md)
- 🚀 Get started with the [Quick Start Guide](getting-started.md)
