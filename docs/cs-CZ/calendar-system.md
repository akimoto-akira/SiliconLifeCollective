# Kalendářový Systém

[English](../en/calendar-system.md) | [中文文档](../zh-CN/calendar-system.md) | [繁體中文](../zh-HK/calendar-system.md) | [Español](../es-ES/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | [Čeština](../cs-CZ/calendar-system.md)

## Přehled

Systém obsahuje **32 kalendářních implementací**, pokrývajících hlavní světové kalendářní systémy. Všechny kalendáře dědí od abstraktní třídy `CalendarBase`.

## Podporované Kalendáře

| Kalendář | ID | Typ | Popis |
|----------|-----|------|-------|
| BuddhistCalendar | `buddhist` | Solární | Buddhistský kalendář (BE), rok + 543 |
| CherokeeCalendar | `cherokee` | Lunisolární | Kalendářní systém Cherokee |
| ChineseLunarCalendar | `lunar` | Lunární | Čínský lunární kalendář s přestupnými měsíci |
| ChulaSakaratCalendar | `chula_sakarat` | Solární | Chula Sakarat (CS), rok - 638 |
| CopticCalendar | `coptic` | Solární | Koptský kalendář |
| DaiCalendar | `dai` | Lunisolární | Dai kalendář |
| DehongDaiCalendar | `dehong_dai` | Lunisolární | Varianta Dehong Dai |
| EthiopianCalendar | `ethiopian` | Solární | Etiopský kalendář |
| FrenchRepublicanCalendar | `french_republican` | Solární | Francouzský republikánský kalendář |
| GregorianCalendar | `gregorian` | Solární | Standardní gregoriánský kalendář |
| HebrewCalendar | `hebrew` | Lunisolární | Hebrejský (židovský) kalendář |
| IndianCalendar | `indian` | Solární | Indický národní kalendář |
| InuitCalendar | `inuit` | Solární | Kalendářní systém Inuit |
| IslamicCalendar | `islamic` | Lunární | Islámský Hijri kalendář |
| JapaneseCalendar | `japanese` | Solární | Japonský kalendář éry (Nengo) |
| JavaneseCalendar | `javanese` | Lunární | Jávský islámský kalendář |
| JucheCalendar | `juche` | Solární | Kalendář Juche (Severní Korea) |
| JulianCalendar | `julian` | Solární | Juliánský kalendář |
| KhmerCalendar | `khmer` | Lunisolární | Khmerský kalendář |
| MayanCalendar | `mayan` | Dlouhý | Mayský dlouhý kalendář |
| MongolianCalendar | `mongolian` | Lunisolární | Mongolský kalendář |
| PersianCalendar | `persian` | Solární | Perský (Solar Hijri) kalendář |
| RepublicOfChinaCalendar | `roc` | Solární | Kalendář Čínské republiky (Minguo) |
| RomanCalendar | `roman` | Lunární | Římský kalendář |
| SakaCalendar | `saka` | Solární | Saka kalendář (Indonésie) |
| SexagenaryCalendar | `sexagenary` | Cyklický | Čínský kalendář Ganzhi (60letý cyklus) |
| TibetanCalendar | `tibetan` | Lunisolární | Tibetský kalendář |
| VietnameseCalendar | `vietnamese` | Lunární | Vietnamský lunární kalendář |
| VikramSamvatCalendar | `vikram_samvat` | Lunisolární | Kalendář Vikram Samvat |
| YiCalendar | `yi` | Solární | Kalendářní systém Yi |
| ZoroastrianCalendar | `zoroastrian` | Solární | Zoroastriánský kalendář |

## CalendarTool Operace

### now

Získá aktuální datum v zadaném kalendáři.

**Parametry**:
- `calendar`: ID kalendáře (např. "gregorian", "lunar")

**Návrat**:
```json
{
  "calendar": "gregorian",
  "year": 2026,
  "month": 4,
  "day": 25,
  "formatted": "2026-04-25"
}
```

### format

Formátuje datum do čitelného řetězce.

**Parametry**:
- `calendar`: ID kalendáře
- `year`: Rok
- `month`: Měsíc
- `day`: Den
- `format`: Formátovací řetězec (volitelné)

### add_days

Přidá dny k datu.

**Parametry**:
- `calendar`: ID kalendáře
- `year`: Rok
- `month`: Měsíc
- `day`: Den
- `days`: Počet dnů k přidání (může být záporný)

### diff

Vypočítá rozdíl mezi dvěma daty.

**Parametry**:
- `calendar`: ID kalendáře
- `year1`, `month1`, `day1`: První datum
- `year2`, `month2`, `day2`: Druhé datum

**Návrat**: Počet dnů mezi daty

### list_calendars

Vypíše všechny dostupné kalendáře.

**Návrat**: Seznam ID a názvů kalendářů

### get_components

Získá komponenty data (rok, měsíc, den, den v týdnu, atd.).

**Parametry**:
- `calendar`: ID kalendáře
- `year`, `month`, `day`: Datum

### get_now_components

Získá komponenty aktuálního data.

**Parametry**:
- `calendar`: ID kalendáře

### convert

Převede datum mezi kalendáři.

**Parametry**:
- `from_calendar`: Zdrojový kalendář
- `to_calendar`: Cílový kalendář
- `year`, `month`, `day`: Datum k převodu

**Návrat**:
```json
{
  "from": {
    "calendar": "gregorian",
    "year": 2026,
    "month": 4,
    "day": 25
  },
  "to": {
    "calendar": "lunar",
    "year": 2026,
    "month": 3,
    "day": 8
  }
}
```

## Použití

### Příklad: Získání aktuálního data

```json
{
  "action": "now",
  "calendar": "gregorian"
}
```

### Příklad: Převod data

```json
{
  "action": "convert",
  "from_calendar": "gregorian",
  "to_calendar": "lunar",
  "year": 2026,
  "month": 4,
  "day": 25
}
```

### Příklad: Rozdíl mezi daty

```json
{
  "action": "diff",
  "calendar": "gregorian",
  "year1": 2026,
  "month1": 4,
  "day1": 25,
  "year2": 2026,
  "month2": 12,
  "day2": 25
}
```

## Rozšíření

Pro přidání nového kalendáře:

1. Vytvořte novou třídu dědící `CalendarBase`
2. Implementujte abstraktní metody:
   - `ConvertFromGregorian(GregorianDate date)`
   - `ConvertToGregorian(CalendarDate date)`
   - `GetDaysInMonth(int year, int month)`
   - `IsLeapYear(int year)`
3. Umístěte třídu do `src/SiliconLife.Default/Calendar/`
4. CalendarTool automaticky objeví nový kalendář

## Poznámky

- Všechny kalendáře podporují převod z/na gregoriánský kalendář
- Některé kalendáře mají specifické éry nebo cykly
- Přestupné roky se liší podle kalendáře
- Historické kalendáře mohou mít omezený rozsah platnosti
