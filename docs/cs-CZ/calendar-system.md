# Kalendářní systém

> **Verze: v0.2.0-alpha**

[English](../en/calendar-system.md) | [Deutsch](../de-DE/calendar-system.md) | [中文](../zh-CN/calendar-system.md) | [繁體中文](../zh-HK/calendar-system.md) | [Español](../es-ES/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | **Čeština** | [Русский](../ru-RU/calendar-system.md)

## Přehled

SiliconLifeCollective podporuje 32 různých kalendářních systémů pro konverzi dat a historickou přesnost.

## Podporované kalendáře (32)

### Hlavní kalendáře (6)

1. **Gregoriánský (Gregorian)** - Mezinárodní standardní kalendář, ID: `gregorian`
2. **Čínský lunární (Chinese Lunar)** - Tradiční čínský kalendář s výpočtem přestupných měsíců, ID: `lunar`
3. **Islámský (Islamic)** - Islámský lunární kalendář, ID: `islamic`
4. **Hebrejský (Hebrew)** - Židovský kalendář, ID: `hebrew`
5. **Perský (Persian)** - Íránský solární kalendář, ID: `persian`
6. **Indický (Indian)** - Indický národní kalendář, ID: `indian`

### Čínské historické kalendáře (2)

7. **Čínský historický kalendář (Chinese Historical)** - Podpora ganzhi letopočtu a éry císařů, ID: `chinese_historical`
   - **Ganzhi letopočet**: 60letý cyklus (nebeské kmeny + pozemské větve)
   - **Éry císařů**: Podpora ér čínských historických dynastií (Kangxi, Qianlong, Zhenguan atd.)
   - **Dynamická databáze**: Vestavěná kompletní databáze čínských historických dynastií a ér
8. **Ganzhi kalendář (Sexagenary)** - 60letý ganzhi cyklus, ID: `sexagenary`

### Východoasijské kalendáře (6)

9. **Japonský kalendář (Japanese)** - Japonská éra (Nengo), ID: `japanese`
10. **Vietnamský kalendář (Vietnamese)** - Vietnamský lunární kalendář (varianta s kočkou zvěrokruhu), ID: `vietnamese`
11. **Tibetský kalendář (Tibetan)** - Tibetský kalendářní systém, ID: `tibetan`
12. **Mongolský kalendář (Mongolian)** - Mongolský kalendář, ID: `mongolian`
13. **Dajský kalendář (Dai)** - Dajský kalendář s kompletním lunárním výpočtem, ID: `dai`
14. **Dehong dajský kalendář (Dehong Dai)** - Dehong dajská varianta kalendáře, ID: `dehong_dai`

### Historické kalendáře (6)

15. **Mayský kalendář (Mayan)** - Mayský dlouhý počet, ID: `mayan`
16. **Římský kalendář (Roman)** - Antický římský kalendář, ID: `roman`
17. **Juliánský kalendář (Julian)** - Juliánský kalendář, ID: `julian`
18. **Francouzský republikánský kalendář (French Republican)** - Francouzský revoluční kalendář, ID: `french_republican`
19. **Koptský kalendář (Coptic)** - Koptský pravoslavný kalendář, ID: `coptic`
20. **Etiopský kalendář (Ethiopian)** - Etiopský kalendář, ID: `ethiopian`

### Regionální kalendáře (6)

21. **Buddhistský kalendář (Buddhist)** - Buddhistský letopočet (BE), rok + 543, ID: `buddhist`
22. **Saka kalendář (Saka)** - Saka éra (Indonésie), ID: `saka`
23. **Vikram Samvat kalendář (Vikram Samvat)** - Hinduistický kalendář, ID: `vikram_samvat`
24. **Jávský kalendář (Javanese)** - Jávský islámský kalendář, ID: `javanese`
25. **Chula Sakarat kalendář (Chula Sakarat)** - Jihovýchodoasijský buddhistický kalendář, rok - 638, ID: `chula_sakarat`
26. **Khmerský kalendář (Khmer)** - Khmerský kalendář, ID: `khmer`

### Moderní kalendáře (3)

27. **Kalendář Čínské republiky (ROC)** - ROC kalendář, rok - 1911, ID: `roc`
28. **Džuche kalendář (Juche)** - Korejský kalendář, rok - 1911, ID: `juche`
29. **Zoroastrijský kalendář (Zoroastrian)** - Zoroastrijský kalendář, ID: `zoroastrian`

### Etnické kalendáře (3)

30. **Yiský kalendář (Yi)** - Yiský kalendářní systém, ID: `yi`
31. **Cherokee kalendář (Cherokee)** - Kalendář Cherokee, ID: `cherokee`
32. **Inuitský kalendář (Inuit)** - Inuitský kalendář, ID: `inuit`

---

## Používání kalendářního nástroje

### Základní konverze

```json
{
  "action": "convert",
  "date": "2026-04-20",
  "fromCalendar": "gregorian",
  "toCalendar": "chinese_lunar"
}
```

**Odpověď**:
```json
{
  "result": "Čínský lunární: 4. měsíc, 3. den, rok Bingwu",
  "year": 2026,
  "month": 4,
  "day": 3,
  "leapMonth": false
}
```

### Dotazování více kalendářů

```json
{
  "action": "query_all",
  "date": "2026-04-20"
}
```

**Odpověď**: Vrátí datum ve všech 32 kalendářních systémech.

---

## Kalendářní API

### Rozhraní CalendarBase

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

### Příklad: Vlastní kalendář

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Konverzní logika
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Zpětná konverze
        return new GregorianDate(year, month, day);
    }
}
```

---

## Speciální funkce

### Podrobnosti o čínském historickém kalendáři (nové)

Čínský historický kalendář je jedním z vrcholů tohoto systému, podporuje dvě klíčové funkce:

#### 1. Ganzhi letopočet

Používá 60letý cyklus, složený z nebeských kmenů a pozemských větví:

```
Nebeské kmeny (10): Jia, Yi, Bing, Ding, Wu, Ji, Geng, Xin, Ren, Gui
Pozemské větve (12): Zi, Chou, Yin, Mao, Chen, Si, Wu, Wei, Shen, You, Xu, Hai
```

**Příklady**:
- Rok 2026 = Bingwu (Rok koně)
- Rok 2025 = Yisi (Rok hada)
- Rok 2024 = Jiachen (Rok draka)

**Příklad použití**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**Odpověď**:
```json
{
  "result": "Bingwu, 3. měsíc, den 9",
  "ganzhi_year": "Bingwu",
  "zodiac": "Kůň"
}
```

#### 2. Systém ér císařů

Vestavěná kompletní databáze čínských historických dynastií a ér císařů:

**Podporované dynastie** (částečně):
- Dynastie Qing: Kangxi, Yongzheng, Qianlong, Jiaqing, Daoguang, Xianfeng, Tongzhi, Guangxu, Xuantong
- Dynastie Ming: Hongwu, Yongle, Jiajing, Wanli, Chongzhen atd.
- Dynastie Tang: Zhenguan, Kaiyuan, Tianbao atd.
- Dynastie Han: Jianyuan, Yuanguang, Yuanshuo atd.
- Další dynastie...

**Příklad použití**:
```json
{
  "action": "convert",
  "date": "1721-04-12",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**Odpověď**:
```json
{
  "result": "Kangxi 60. rok, 3. měsíc, 15. den",
  "era": "Kangxi",
  "era_year": 60,
  "dynasty": "Qing"
}
```

### Zpracování přestupných měsíců

Kalendáře s přestupnými měsíci:
- Čínský lunární
- Hebrejský
- Buddhistský
- Vietnamský

```json
{
  "isLeapMonth": true,
  "month": 4,
  "note": "Přestupný 4. měsíc"
}
```

---

## Přesnost kalendáře

### Astronomické výpočty

- Na základě skutečných astronomických dat
- Podpora historických dat
- Zpracování kalendářních reforem

### Známá omezení

- Některá starověká data jsou přibližná
- Kalendářní reformy se liší podle regionu
- Nezahrnuje zpracování přestupných sekund

---

## Případy použití

### Historický výzkum

Konverze historických dat do moderního kalendáře:

```
Otázka: "Kdy byla Francouzská revoluce?"
Odpověď: "14. července 1789 (gregoriánský kalendář)"
   "26. den měsíce Thermidor roku I (francouzský republikánský kalendář)"
```

### Kulturní aplikace

Podpora tradičních svátků:

```
Čínský nový rok 2026:
- Gregoriánský: 17. února 2026
- Čínský lunární: 1. den 1. měsíce
```

### Multikulturní plánování

Plánování událostí s ohledem na více kalendářů:

```
Schůzka: 2026-04-20
- Vyhnout se islámské páteční modlitbě
- Respektovat židovský šabat
- Zvážit čínské svátky
```

---

## Osvědčené postupy

### 1. Vždy specifikujte kalendář

Nikdy nepředpokládejte kalendářní systém:

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // Explicitně specifikováno!
}
```

### 2. Zpracovávejte neplatná data

Některá data v určitých kalendářích neexistují:

```json
{
  "error": "Invalid date: February 30 doesn't exist"
}
```

### 3. Zvažte časová pásma

Konverze dat se může lišit podle časového pásma:

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## Další kroky

- 📚 Přečtěte [příručku architektury](architecture.md)
- 🛠️ Prohlédněte [vývojářskou příručku](development-guide.md)
- 🔧 Prohlédněte [referenci nástrojů](tools-reference.md)
- 🚀 Začněte s [příručkou rychlého startu](getting-started.md)
