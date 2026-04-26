# Kalendersystem

[English](../en/calendar-system.md) | [Deutsch](../de-DE/calendar-system.md) | [中文](../zh-CN/calendar-system.md) | [繁體中文](../zh-HK/calendar-system.md) | [Español](../es-ES/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | [Čeština](../cs-CZ/calendar-system.md)

## Übersicht

SiliconLifeCollective unterstützt 32 verschiedene Kalendersysteme für Datumskonvertierung und historische Genauigkeit.

## Unterstützte Kalender (32)

### Hauptkalender (6)

1. **Gregorianischer Kalender (Gregorian)** - Internationaler Standardkalender, ID: `gregorian`
2. **Chinesischer Lunarkalender (Chinese Lunar)** - Traditioneller chinesischer Kalender mit Schaltmonat-Berechnung, ID: `lunar`
3. **Islamischer Kalender (Islamic)** - Islamischer Hidschra-Kalender, ID: `islamic`
4. **Hebräischer Kalender (Hebrew)** - Jüdischer Kalender, ID: `hebrew`
5. **Persischer Kalender (Persian)** - Iranischer Sonnenkalender, ID: `persian`
6. **Indischer Kalender (Indian)** - Indischer Nationalkalender, ID: `indian`

### Chinesische historische Kalender (2)

7. **Chinesischer historischer Kalender (Chinese Historical)** - Unterstützt Ganzhi-Zyklus und Kaiser-Ären, ID: `chinese_historical`
   - **Ganzhi-Zyklus**: 60-Jahre-Zyklus (Himmelsstämme + Erdzweige)
   - **Kaiser-Ären**: Unterstützt Ären aller chinesischen Dynastien (Kangxi, Qianlong, Zhenguan etc.)
   - **Dynamische Datenbank**: Eingebaute vollständige Datenbank chinesischer Dynastien und Ären
8. **Sexagenary-Kalender (Sexagenary)** - 60-Jahre-Ganzhi-Zyklus, ID: `sexagenary`

### Ostasiatische Kalender (6)

9. **Japanischer Kalender (Japanese)** - Japanischer Nengo-Ären-Kalender, ID: `japanese`
10. **Vietnamesischer Kalender (Vietnamese)** - Vietnamesischer Lunarkalender (Katzen-Zodiak-Variante), ID: `vietnamese`
11. **Tibetischer Kalender (Tibetan)** - Tibetisches Kalendersystem, ID: `tibetan`
12. **Mongolischer Kalender (Mongolian)** - Mongolischer Kalender, ID: `mongolian`
13. **Dai-Kalender (Dai)** - Dai-Kalender mit vollständiger Lunarb erechnung, ID: `dai`
14. **Dehong-Dai-Kalender (Dehong Dai)** - Dehong-Dai-Variante, ID: `dehong_dai`

### Historische Kalender (6)

15. **Maya-Kalender (Mayan)** - Maya-Langzeitkalender, ID: `mayan`
16. **Römischer Kalender (Roman)** - Altrömischer Kalender, ID: `roman`
17. **Julianischer Kalender (Julian)** - Julianischer Kalender, ID: `julian`
18. **Französischer republikanischer Kalender (French Republican)** - Französischer Revolutionskalender, ID: `french_republican`
19. **Koptischer Kalender (Coptic)** - Koptisch-orthodoxer Kalender, ID: `coptic`
20. **Äthiopischer Kalender (Ethiopian)** - Äthiopischer Kalender, ID: `ethiopian`

### Regionale Kalender (6)

21. **Buddhistischer Kalender (Buddhist)** - Buddhistische Zeitrechnung (BE), Jahr + 543, ID: `buddhist`
22. **Saka-Kalender (Saka)** - Saka-Ära (Indonesien), ID: `saka`
23. **Vikram-Samvat-Kalender (Vikram Samvat)** - Hinduistischer Kalender, ID: `vikram_samvat`
24. **Javanischer Kalender (Javanese)** - Javanisch-islamischer Kalender, ID: `javanese`
25. **Chula-Sakarat-Kalender (Chula Sakarat)** - Südostasiatischer buddhistischer Kalender, Jahr - 638, ID: `chula_sakarat`
26. **Khmer-Kalender (Khmer)** - Khmer-Kalender, ID: `khmer`

### Moderne Kalender (3)

27. **ROC-Kalender (ROC)** - Minguo-Kalender, Jahr - 1911, ID: `roc`
28. **Juche-Kalender (Juche)** - Nordkoreanischer Kalender, Jahr - 1911, ID: `juche`
29. **Zoroastrischer Kalender (Zoroastrian)** - Zoroastrischer Kalender, ID: `zoroastrian`

### Ethnische Kalender (3)

30. **Yi-Kalender (Yi)** - Yi-Kalendersystem, ID: `yi`
31. **Cherokee-Kalender (Cherokee)** - Cherokee-Kalender, ID: `cherokee`
32. **Inuit-Kalender (Inuit)** - Inuit-Kalender, ID: `inuit`

---

## Kalender-Tool verwenden

### Grundlegende Konvertierung

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

**Antwort**: Gibt Datum in allen 32 Kalendersystemen zurück.

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

## Spezielle Funktionen

### Chinesischer historischer Kalender im Detail (Neu)

Der chinesische historische Kalender ist ein Highlight des Systems mit zwei Kernfunktionen:

#### 1. Ganzhi-Jahreszyklus-System

Verwendet 60-Jahre-Zyklus, kombiniert aus Himmelsstämmen und Erdzweigen:

```
Himmelsstämme (10): 甲、乙、丙、丁、戊、己、庚、辛、壬、癸
Erdzweige (12): 子、丑、寅、卯、辰、巳、午、未、申、酉、戌、亥
```

**Beispiele**:
- 2026 = 丙午年
- 2025 = 乙巳年 (Jahr der Schlange)
- 2024 = 甲辰年 (Jahr des Drachens)

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

#### 2. Kaiser-Ären-System

Eingebaute vollständige Datenbank chinesischer Dynastien und Kaiser-Ären:

**Unterstützte Dynastien** (Auszug):
- Qing-Dynastie: Kangxi, Yongzheng, Qianlong, Jiaqing, Daoguang, Xianfeng, Tongzhi, Guangxu, Xuantong
- Ming-Dynastie: Hongwu, Yongle, Jiajing, Wanli, Chongzhen etc.
- Tang-Dynastie: Zhenguan, Kaiyuan, Tianbao etc.
- Han-Dynastie: Jianyuan, Yuanguang, Yuanshuo etc.
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

### Schaltmonat-Behandlung

Kalender mit Schaltmonaten:
- Chinesischer Lunarkalender
- Hebräischer Kalender
- Buddhistischer Kalender
- Vietnamesischer Kalender

```json
{
  "isLeapMonth": true,
  "month": 4,
  "note": "闰四月"
}
```

---

## Kalender-Genauigkeit

### Astronomische Berechnungen

- Basierend auf tatsächlichen astronomischen Daten
- Unterstützt historische Daten
- Behandelt Kalenderreformen

### Bekannte Einschränkungen

- Einige antike Daten sind approximativ
- Kalenderreformen variieren nach Region
- Enthält keine Schaltsekunden-Behandlung

---

## Anwendungsfälle

### Historische Forschung

Historische Daten in moderne Kalender konvertieren:

```
Frage: "Wann war die Französische Revolution?"
Antwort: "14. Juli 1789 (Gregorianisch)"
        "26. Thermidor I (Französisch republikanisch)"
```

### Kulturelle Anwendungen

Unterstützung traditioneller Feiertage:

```
Chinesisches Neujahr 2026:
- Gregorianisch: 17. Februar 2026
- Lunarkalender: 1. Tag des 1. Monats
```

### Multikulturelles Scheduling

Events planen unter Berücksichtigung mehrerer Kalender:

```
Meeting: 2026-04-20
- Islamisches Freitagsgebet vermeiden
- Jüdischen Schabbat respektieren
- Chinesische Feiertage beachten
```

---

## Best Practices

### 1. Immer Kalender spezifizieren

Niemals Kalendersystem annehmen:

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // Explizit angeben!
}
```

### 2. Ungültige Daten behandeln

Einige Daten existieren in bestimmten Kalendern nicht:

```json
{
  "error": "Invalid date: February 30 doesn't exist"
}
```

### 3. Zeitzonen berücksichtigen

Datumskonvertierung kann je nach Zeitzone variieren:

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## Nächste Schritte

- 📚 [Architekturleitfaden](architecture.md) lesen
- 🛠️ [Entwicklungsleitfaden](development-guide.md) prüfen
- 🔧 [Tool-Referenz](tools-reference.md) ansehen
- 🚀 [Schnellstart-Leitfaden](getting-started.md) beginnen
