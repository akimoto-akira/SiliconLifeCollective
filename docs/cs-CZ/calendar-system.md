# Systém Kalendářů

[English](../en/calendar-system.md) | [中文](../zh-CN/calendar-system.md) | [繁體中文](../zh-HK/calendar-system.md) | [Español](../es-ES/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | [Deutsch](../de-DE/calendar-system.md) | **Čeština**

## Přehled

SiliconLifeCollective podporuje 32 různých kalendářových systémů pro převod data a historickou přesnost.

## Podporované Kalendáře (32 typů)

### Hlavní Kalendáře (6 typů)

1. **Gregoriánský (Gregorian)** - Mezinárodní standardní kalendář, ID: `gregorian`
2. **Čínský Lunární (Chinese Lunar)** - Tradiční čínský kalendář s výpočtem přestupných měsíců, ID: `lunar`
3. **Islámský (Islamic)** - Islámský lunární kalendář, ID: `islamic`
4. **Hebrejský (Hebrew)** - Židovský kalendář, ID: `hebrew`
5. **Perský (Persian)** - Íránský solární kalendář, ID: `persian`
6. **Indický (Indian)** - Indický národní kalendář, ID: `indian`

### Čínské Historické Kalendáře (2 typy)

7. **Čínský Historický (Chinese Historical)** - Podpora cyklického letopočtu a éry panovníků, ID: `chinese_historical`
   - **Cyklický Letopočet**: 60letý cyklus (Nebeské kmeny + Zemské větve)
   - **Éry Panovníků**: Podpora éry všech čínských historických dynastií (Kangxi, Qianlong, Zhenguan atd.)
   - **Dynamická Databáze**: Vestavěná kompletní databáze čínských historických dynastií a éry
8. **Sexagenární (Sexagenary)** - 60letý cyklus Nebeských kmenů a Zemských větví, ID: `sexagenary`

### Východoasijské Kalendáře (6 typů)

9. **Japonský (Japanese)** - Japonský kalendář s érami (Nengo), ID: `japanese`
10. **Vietnamský (Vietnamese)** - Vietnamský lunární kalendář (varianta s kočkou), ID: `vietnamese`
11. **Tibetský (Tibetan)** - Tibetský kalendářový systém, ID: `tibetan`
12. **Mongolský (Mongolian)** - Mongolský kalendář, ID: `mongolian`
13. **Dai (Dai)** - Dai kalendář s kompletním lunárním výpočtem, ID: `dai`
14. **Dehong Dai (Dehong Dai)** - Varianta Dehong Dai kalendáře, ID: `dehong_dai`

### Historické Kalendáře (6 typů)

15. **Mayský (Mayan)** - Mayský dlouhý kalendář, ID: `mayan`
16. **Římský (Roman)** - Starořímský kalendář, ID: `roman`
17. **Juliánský (Julian)** - Juliánský kalendář, ID: `julian`
18. **Francouzský Republikánský (French Republican)** - Kalendář francouzské revoluce, ID: `french_republican`
19. **Koptský (Coptic)** - Koptský pravoslavný kalendář, ID: `coptic`
20. **Etiopský (Ethiopian)** - Etiopský kalendář, ID: `ethiopian`

### Regionální Kalendáře (6 typů)

21. **Buddhistický (Buddhist)** - Buddhistický letopočet (BE), rok + 543, ID: `buddhist`
22. **Saka (Saka)** - Letopočet Saka (Indonésie), ID: `saka`
23. **Vikram Samvat (Vikram Samvat)** - Hindský kalendář, ID: `vikram_samvat`
24. **Jávský (Javanese)** - Jávský islámský kalendář, ID: `javanese`
25. **Chula Sakarat (Chula Sakarat)** - Jihovýchodoasijský buddhistický kalendář, rok - 638, ID: `chula_sakarat`
26. **Khmerský (Khmer)** - Khmerský kalendář, ID: `khmer`

### Moderní Kalendáře (3 typy)

27. **ROC (ROC)** - Kalendář Republiky Čína, rok - 1911, ID: `roc`
28. **Čučche (Juche)** - Severokorejský kalendář, rok - 1911, ID: `juche`
29. **Zoroastriánský (Zoroastrian)** - Zoroastriánský kalendář, ID: `zoroastrian`

### Etnické Kalendáře (3 typy)

30. **Yi (Yi)** - Kalendářový systém Yi, ID: `yi`
31. **Čerokézský (Cherokee)** - Čerokézský kalendář, ID: `cherokee`
32. **Inuitský (Inuit)** - Inuitský kalendář, ID: `inuit`

---

## Používání Kalendářového Nástroje

### Základní Převod

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
  "result": "Lunární duben třetí den roku Bing-Wu",
  "year": 2026,
  "month": 4,
  "day": 3,
  "leapMonth": false
}
```

### Dotaz na Více Kalendářů

```json
{
  "action": "query_all",
  "date": "2026-04-20"
}
```

**Odpověď**: Vrátí datum ve všech 32 kalendářových systémech.

---

## Kalendářové API

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

### Příklad: Vlastní Kalendář

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Logika převodu
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Reverzní převod
        return new GregorianDate(year, month, day);
    }
}
```

---

## Speciální Funkce

### Detail Čínského Historického Kalendáře (Nové)

Čínský historický kalendář je jednou z hlavních funkcí tohoto systému, podporující dvě klíčové funkce:

#### 1. Systém Cyklického Letopočtu

Používá 60letý cyklus kombinace Nebeských kmenů a Zemských větví:

```
Nebeské kmeny (10): Jia, Yi, Bing, Ding, Wu, Ji, Geng, Xin, Ren, Gui
Zemské větve (12): Zi, Chou, Yin, Mao, Chen, Si, Wu, Wei, Shen, You, Xu, Hai
```

**Příklad**:
- Rok 2026 = Bing-Wu rok
- Rok 2025 = Yi-Si rok (rok Hada)
- Rok 2024 = Jia-Chen rok (rok Draka)

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
  "result": "Devátý den třetího měsíce roku Bing-Wu",
  "ganzhi_year": "Bing-Wu",
  "zodiac": "Kůň"
}
```

#### 2. Systém Éry Panovníků

Vestavěná kompletní databáze čínských historických dynastií a éry panovníků:

**Podporované dynastie** (částečný seznam):
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
  "result": "Patnáctý den třetího měsíce šedesátého roku éry Kangxi",
  "era": "Kangxi",
  "era_year": 60,
  "dynasty": "Qing"
}
```

### Zpracování Přestupných Měsíců

Kalendáře s přestupnými měsíci:
- Čínský lunární
- Hebrejský
- Buddhistický
- Vietnamský

```json
{
  "isLeapMonth": true,
  "month": 4,
  "note": "Přestupný čtvrtý měsíc"
}
```

---

## Přesnost Kalendáře

### Astronomické Výpočty

- Založeno na skutečných astronomických datech
- Podpora historických dat
- Zpracování kalendářových reforem

### Známé Limity

- Některá starověká data jsou přibližná
- Kalendářové reformy se liší podle regionu
- Nezahrnuje zpracování přestupných sekund

---

## Případy Použití

### Historický Výzkum

Převod historických dat na moderní kalendář:

```
Otázka: "Kdy byla francouzská revoluce?"
Odpověď: "14. července 1789 (gregoriánský)"
         "26. Thermidor roku I (francouzský republikánský)"
```

### Kulturní Aplikace

Podpora tradičních festivalů:

```
Čínský Nový rok 2026:
- Gregoriánský: 17. února 2026
- Lunární: První den prvního měsíce
```

### Multikulturní Plánování

Plánování událostí s respektem k více kalendářům:

```
Schůzka: 2026-04-20
- Vyhnout se islámské páteční modlitbě
- Respektovat židovský šábes
- Zohlednit čínské svátky
```

---

## Nejlepší Praktiky

### 1. Vždy Specifikujte Kalendář

Nikdy nepředpokládejte kalendářový systém:

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // Explicitně specifikujte!
}
```

### 2. Zpracovávejte Neplatná Data

Některá data v některých kalendářích neexistují:

```json
{
  "error": "Neplatné datum: 30. únor neexistuje"
}
```

### 3. Zvažte Časová Pásma

Převod data se může lišit podle časového pásma:

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## Další Kroky

- 📚 Přečtěte si [Průvodce Architektury](architecture.md)
- 🛠️ Podívejte se na [Vývojářskou Příručku](development-guide.md)
- 🔧 Podívejte se na [Referenci Nástrojů](tools-reference.md)
- 🚀 Začněte s [Průvodcem Rychlým Startem](getting-started.md)
