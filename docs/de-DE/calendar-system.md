# Kalendersystem

> **Version: v0.2.0-alpha**

[English](../en/calendar-system.md) | **Deutsch** | [中文](../zh-CN/calendar-system.md) | [繁體中文](../zh-HK/calendar-system.md) | [Español](../es-ES/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | [Čeština](../cs-CZ/calendar-system.md) | [Русский](../ru-RU/calendar-system.md)

## Übersicht

SiliconLifeCollective unterstützt 32 verschiedene Kalendersysteme für Datumsumrechnungen und historische Genauigkeit.

## Unterstützte Kalender (32)

### Hauptkalender (6)

1. **Gregorianischer Kalender (Gregorian)** - Internationaler Standardkalender, ID: `gregorian`
2. **Chinesischer Mondkalender (Chinese Lunar)** - Traditioneller chinesischer Kalender mit Schaltmonatsberechnung, ID: `lunar`
3. **Islamischer Kalender (Islamic)** - Islamischer Hidschra-Kalender, ID: `islamic`
4. **Hebräischer Kalender (Hebrew)** - Jüdischer Kalender, ID: `hebrew`
5. **Persischer Kalender (Persian)** - Iranischer Sonnenkalender, ID: `persian`
6. **Indischer Nationalkalender (Indian)** - Indischer Nationalkalender, ID: `indian`

### Chinesische historische Kalender (2)

7. **Chinesischer historischer Kalender (Chinese Historical)** - Unterstützt Sexagenarzyklus und Kaiserära-Namen, ID: `chinese_historical`
   - **Sexagenarzyklus**: 60-Jahres-Zyklus (Himmelsstämme + Erdzweige)
   - **Kaiserära-Namen**: Unterstützt Ära-Namen verschiedener chinesischer Dynastien (Kangxi, Qianlong, Zhenguan usw.)
   - **Dynamische Datenbank**: Integrierte vollständige Datenbank chinesischer historischer Dynastien und Ära-Namen
8. **Sexagenarzyklus-Kalender (Sexagenary)** - 60-Jahres-Sexagenarzyklus, ID: `sexagenary`

### Ostasiatische Kalender (6)

9. **Japanischer Kalender (Japanese)** - Japanischer Nengo-Kalender, ID: `japanese`
10. **Vietnamesischer Kalender (Vietnamese)** - Vietnamesischer Mondkalender (Katze-Tierkreis-Variante), ID: `vietnamese`
11. **Tibetischer Kalender (Tibetan)** - Tibetisches Kalendersystem, ID: `tibetan`
12. **Mongolischer Kalender (Mongolian)** - Mongolischer Kalender, ID: `mongolian`
13. **Dai-Kalender (Dai)** - Dai-Kalender mit vollständiger Mondkalenderberechnung, ID: `dai`
14. **Dehong-Dai-Kalender (Dehong Dai)** - Dehong-Dai-Kalender-Variante, ID: `dehong_dai`

### Historische Kalender (6)

15. **Maya-Kalender (Mayan)** - Maya-Langzählungskalender, ID: `mayan`
16. **Römischer Kalender (Roman)** - Antiker römischer Kalender, ID: `roman`
17. **Julianischer Kalender (Julian)** - Julianischer Kalender, ID: `julian`
18. **Französischer Republikanischer Kalender (French Republican)** - Französischer Revolutionskalender, ID: `french_republican`
19. **Koptischer Kalender (Coptic)** - Koptisch-orthodoxer Kalender, ID: `coptic`
20. **Äthiopischer Kalender (Ethiopian)** - Äthiopischer Kalender, ID: `ethiopian`

### Regionale Kalender (6)

21. **Buddhistischer Kalender (Buddhist)** - Buddhistische Ära (BE), Jahr + 543, ID: `buddhist`
22. **Saka-Kalender (Saka)** - Saka-Ära (Indonesien), ID: `saka`
23. **Vikram-Samvat-Kalender (Vikram Samvat)** - Hinduistischer Kalender, ID: `vikram_samvat`
24. **Javanischer Kalender (Javanese)** - Javanisch-islamischer Kalender, ID: `javanese`
25. **Chula-Sakarat-Kalender (Chula Sakarat)** - Südostasiatischer buddhistischer Kalender, Jahr - 638, ID: `chula_sakarat`
26. **Khmer-Kalender (Khmer)** - Khmer-Kalender, ID: `khmer`

### Moderne Kalender (3)

27. **ROC-Kalender (ROC)** - Republik-China-Kalender, Jahr - 1911, ID: `roc`
28. **Juche-Kalender (Juche)** - Nordkoreanischer Kalender, Jahr - 1911, ID: `juche`
29. **Zoroastrischer Kalender (Zoroastrian)** - Zoroastrischer Kalender, ID: `zoroastrian`

### Ethnische Kalender (3)

30. **Yi-Kalender (Yi)** - Yi-Kalendersystem, ID: `yi`
31. **Cherokee-Kalender (Cherokee)** - Cherokee-Kalender, ID: `cherokee`
32. **Inuit-Kalender (Inuit)** - Inuit-Kalender, ID: `inuit`

---

## Kalender-Werkzeug verwenden

### Einfache Umrechnung

```json
{
  "action": "convert",
  "date": "2026-04-20",
  "fromCalendar": "gregorian",
  "toCalendar": "chinese_lunar"
}
```

**Antwort**:
```json
{
  "result": "农历丙午年四月初三",
  "year": 2026,
  "month": 4,
  "day": 3,
  "leapMonth": false
}
```

### Multi-Kalender-Abfrage

```json
{
  "action": "query_all",
  "date": "2026-04-20"
}
```

**Antwort**: Gibt das Datum in allen 32 Kalendersystemen zurück.

---

## Kalender-API

### CalendarBase-Schnittstelle

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

### Beispiel: Benutzerdefinierter Kalender

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Konvertierungslogik
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Rückkonvertierung
        return new GregorianDate(year, month, day);
    }
}
```

---

## Besondere Funktionen

### Chinesischer historischer Kalender im Detail (Neu)

Der chinesische historische Kalender ist ein Highlight dieses Systems und unterstützt zwei Kernfunktionen:

#### 1. Sexagenarzyklus-System

Verwendet einen 60-Jahres-Zyklus, der aus Himmelsstämmen und Erdzweigen kombiniert wird:

```
Himmelsstämme (10): 甲、乙、丙、丁、戊、己、庚、辛、壬、癸
Erdzweige (12): 子、丑、寅、卯、辰、巳、午、未、申、酉、戌、亥
```

**Beispiele**:
- 2026 = 丙午年
- 2025 = 乙巳年 (Schlangenjahr)
- 2024 = 甲辰年 (Drachenjahr)

**Verwendungsbeispiel**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**Antwort**:
```json
{
  "result": "丙午年 三月 初九",
  "ganzhi_year": "丙午",
  "zodiac": "马"
}
```

#### 2. Kaiserära-System

Integrierte vollständige Datenbank chinesischer historischer Dynastien und Kaiserära-Namen:

**Unterstützte Dynastien** (Auswahl):
- Qing-Dynastie: Kangxi, Yongzheng, Qianlong, Jiaqing, Daoguang, Xianfeng, Tongzhi, Guangxu, Xuantong
- Ming-Dynastie: Hongwu, Yongle, Jiajing, Wanli, Chongzhen u. a.
- Tang-Dynastie: Zhenguan, Kaiyuan, Tianbao u. a.
- Han-Dynastie: Jianyuan, Yuanguang, Yuanshuo u. a.
- Weitere Dynastien...

**Verwendungsbeispiel**:
```json
{
  "action": "convert",
  "date": "1721-04-12",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**Antwort**:
```json
{
  "result": "康熙六十年 三月 十五日",
  "era": "康熙",
  "era_year": 60,
  "dynasty": "清"
}
```

### Schaltmonatsbehandlung

Kalender mit Schaltmonaten:
- Chinesischer Mondkalender
- Hebräischer Kalender
- Buddhistischer Kalender
- Vietnamesischer Kalender

```json
{
  "isLeapMonth": true,
  "month": 4,
  "note": "Schaltmonat 4"
}
```

---

## Kalendergenauigkeit

### Astronomische Berechnungen

- Basierend auf tatsächlichen astronomischen Daten
- Unterstützung historischer Daten
- Behandlung von Kalenderreformen

### Bekannte Einschränkungen

- Einige antike Daten sind Näherungswerte
- Kalenderreformen variieren je nach Region
- Keine Schaltsekundenbehandlung

---

## Anwendungsbeispiele

### Historische Forschung

Historische Daten in moderne Kalender umrechnen:

```
Frage: "Wann war die Französische Revolution?"
Antwort: "14. Juli 1789 (Gregorianisch)"
   "26. Thermidor des Jahres I (Französischer Republikanischer Kalender)"
```

### Kulturelle Anwendungen

Unterstützung traditioneller Feste:

```
Chinesisches Neujahrsfest 2026:
- Gregorianisch: 17. Februar 2026
- Mondkalender: 1. Tag des 1. Monats
```

### Multikulturelle Terminplanung

Termine planen, die mehrere Kalender berücksichtigen:

```
Besprechung: 2026-04-20
- Islamischen Freitagsgebet ausweichen
- Jüdischen Schabbat respektieren
- Chinesische Feiertage berücksichtigen
```

---

## Best Practices

### 1. Kalender immer angeben

Das Kalendersystem niemals voraussetzen:

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // Explizit angeben!
}
```

### 2. Ungültige Daten behandeln

Bestimmte Daten existieren in bestimmten Kalendern nicht:

```json
{
  "error": "Invalid date: February 30 doesn't exist"
}
```

### 3. Zeitzonen berücksichtigen

Datumsumrechnungen können je nach Zeitzone variieren:

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## Nächste Schritte

- 📚 [Architekturhandbuch](architecture.md) lesen
- 🛠️ [Entwicklungshandbuch](development-guide.md) ansehen
- 🔧 [Werkzeugreferenz](tools-reference.md) ansehen
- 🚀 Mit der [Schnellstartanleitung](getting-started.md) beginnen
