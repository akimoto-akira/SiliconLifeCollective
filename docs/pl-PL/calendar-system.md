# System kalendarzowy

> **Wersja: v0.2.0-alpha**

[English](../en/calendar-system.md) | [Deutsch](../de-DE/calendar-system.md) | [中文](../zh-CN/calendar-system.md) | [繁體中文](../zh-HK/calendar-system.md) | [Español](../es-ES/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | [Čeština](../cs-CZ/calendar-system.md) | [Polski](../pl-PL/calendar-system.md)

## Przegląd

SiliconLifeCollective obsługuje 32 różne systemy kalendarzowe do konwersji dat i zapewnienia dokładności historycznej.

## Obsługiwane kalendarze (32)

### Główne kalendarze (6)

1. **Gregoriański** - Międzynarodowy kalendarz standardowy, ID: `gregorian`
2. **Chiński księżycowy** - Tradycyjny kalendarz chiński, z obliczaniem miesięcy przestępnych, ID: `lunar`
3. **Islamski** - Kalendarz hidżry, ID: `islamic`
4. **Hebrajski** - Kalendarz żydowski, ID: `hebrew`
5. **Perski** - Irański kalendarz słoneczny, ID: `persian`
6. **Indyjski** - Indyjski kalendarz narodowy, ID: `indian`

### Chińskie kalendarze historyczne (2)

7. **Chiński historyczny** - Obsługa cyklu ganzhi i er cesarskich, ID: `chinese_historical`
   - **Cykl ganzhi**: 60-letni cykl (Niebiańskie Pnie + Ziemskie Gałęzie)
   - **Ery cesarskie**: Obsługa er cesarskich z różnych chińskich dynastii (Kangxi, Qianlong, Zhenguan itp.)
   - **Dynamiczna baza danych**: Wbudowana kompletna baza chińskich dynastii historycznych i er cesarskich
8. **Ganzhi** - 60-letni cykl ganzhi, ID: `sexagenary`

### Kalendarze wschodnioazjatyckie (6)

9. **Japoński** - Kalendarz ery japońskiej (Nengo), ID: `japanese`
10. **Wietnamski** - Wietnamski kalendarz księżycowy (wariant ze znakiem Kota), ID: `vietnamese`
11. **Tybetański** - System kalendarza tybetańskiego, ID: `tibetan`
12. **Mongolski** - Kalendarz mongolski, ID: `mongolian`
13. **Dai** - Kalendarz Dai, z pełnymi obliczeniami księżycowymi, ID: `dai`
14. **Dehong Dai** - Wariant kalendarza Dehong Dai, ID: `dehong_dai`

### Kalendarze historyczne (6)

15. **Majański** - Długa rachuba Majów, ID: `mayan`
16. **Rzymski** - Starożytny kalendarz rzymski, ID: `roman`
17. **Juliański** - Kalendarz juliański, ID: `julian`
18. **Francuski republikański** - Kalendarz rewolucji francuskiej, ID: `french_republican`
19. **Koptyjski** - Kalendarz kościoła koptyjskiego, ID: `coptic`
20. **Etiopski** - Kalendarz etiopski, ID: `ethiopian`

### Kalendarze regionalne (6)

21. **Buddyjski** - Era buddyjska (BE), rok + 543, ID: `buddhist`
22. **Saka** - Era Saka (Indonezja), ID: `saka`
23. **Vikram Samvat** - Kalendarz hinduski, ID: `vikram_samvat`
24. **Jawajski** - Kalendarz islamski jawajski, ID: `javanese`
25. **Chula Sakarat** - Kalendarz buddyjski Azji Południowo-Wschodniej, rok - 638, ID: `chula_sakarat`
26. **Khmerski** - Kalendarz khmerski, ID: `khmer`

### Kalendarze nowoczesne (3)

27. **ROC** - Kalendarz Republiki Chińskiej, rok - 1911, ID: `roc`
28. **Dżucze** - Kalendarz północnokoreański, rok - 1911, ID: `juche`
29. **Zaratusztriański** - Kalendarz zaratusztriański, ID: `zoroastrian`

### Kalendarze etniczne (3)

30. **Yi** - System kalendarza Yi, ID: `yi`
31. **Czirokeski** - Kalendarz czirokeski, ID: `cherokee`
32. **Inuicki** - Kalendarz inuicki, ID: `inuit`

---

## Korzystanie z narzędzia kalendarzowego

### Podstawowa konwersja

```json
{
  "action": "convert",
  "date": "2026-04-20",
  "fromCalendar": "gregorian",
  "toCalendar": "chinese_lunar"
}
```

**Odpowiedź**:
```json
{
  "result": "Księżycowy 4. miesiąc, 3. dzień roku Bingwu",
  "year": 2026,
  "month": 4,
  "day": 3,
  "leapMonth": false
}
```

### Zapytanie wielokalendarzowe

```json
{
  "action": "query_all",
  "date": "2026-04-20"
}
```

**Odpowiedź**: Zwraca datę we wszystkich 32 systemach kalendarzowych.

---

## API kalendarzowe

### Interfejs CalendarBase

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

### Przykład: Niestandardowy kalendarz

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";

    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Logika konwersji
        return new CalendarDate(year, month, day);
    }

    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Konwersja odwrotna
        return new GregorianDate(year, month, day);
    }
}
```

---

## Funkcje specjalne

### Szczegóły chińskiego kalendarza historycznego (nowość)

Chiński kalendarz historyczny to jedna z głównych atrakcji tego systemu, obsługująca dwie rdzenne funkcje:

#### 1. System ganzhi (cykl sześćdziesięcioletni)

Wykorzystuje 60-letni cykl, utworzony z kombinacji Niebiańskich Pni i Ziemskich Gałęzi:

```
Niebiańskie Pnie (10): 甲, 乙, 丙, 丁, 戊, 己, 庚, 辛, 壬, 癸
Ziemskie Gałęzie (12): 子, 丑, 寅, 卯, 辰, 巳, 午, 未, 申, 酉, 戌, 亥
```

**Przykłady**:
- 2026 = Rok Bingwu
- 2025 = Rok Yisi (Rok Węża)
- 2024 = Rok Jiachen (Rok Smoka)

**Przykład użycia**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**Odpowiedź**:
```json
{
  "result": "Rok Bingwu, 3. miesiąc, 9. dzień",
  "ganzhi_year": "丙午",
  "zodiac": "Koń"
}
```

#### 2. System er cesarskich

Wbudowana kompletna baza chińskich dynastii historycznych i er cesarskich:

**Obsługiwane dynastie** (część):
- Dynastia Qing: Kangxi, Yongzheng, Qianlong, Jiaqing, Daoguang, Xianfeng, Tongzhi, Guangxu, Xuantong
- Dynastia Ming: Hongwu, Yongle, Jiajing, Wanli, Chongzhen i inne
- Dynastia Tang: Zhenguan, Kaiyuan, Tianbao i inne
- Dynastia Han: Jianyuan, Yuanguang, Yuanshuo i inne
- Inne dynastie...

**Przykład użycia**:
```json
{
  "action": "convert",
  "date": "1721-04-12",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**Odpowiedź**:
```json
{
  "result": "Kangxi, 60. rok, 3. miesiąc, 15. dzień",
  "era": "康熙",
  "era_year": 60,
  "dynasty": "Qing"
}
```

### Obsługa miesięcy przestępnych

Kalendarze z miesiącami przestępnymi:
- Chiński księżycowy
- Hebrajski
- Buddyjski
- Wietnamski

```json
{
  "isLeapMonth": true,
  "month": 4,
  "note": "Przestępny 4. miesiąc"
}
```

---

## Dokładność kalendarzowa

### Obliczenia astronomiczne

- Na podstawie rzeczywistych danych astronomicznych
- Obsługa dat historycznych
- Obsługa reform kalendarzowych

### Znane ograniczenia

- Niektóre starożytne daty są przybliżone
- Reformy kalendarzowe różnią się w zależności od regionu
- Nie obejmuje obsługi sekund przestępnych

---

## Przypadki użycia

### Badania historyczne

Konwersja dat historycznych na współczesny kalendarz:

```
Pytanie: "Kiedy była rewolucja francuska?"
Odpowiedź: "14 lipca 1789 (gregoriański)"
   "26 Thermidor Roku I (republikański francuski)"
```

### Zastosowania kulturalne

Obsługa tradycyjnych świąt:

```
Chiński Nowy Rok 2026:
- Gregoriański: 17 lutego 2026
- Chiński księżycowy: 1. dzień 1. miesiąca
```

### Planowanie wielokulturowe

Planowanie wydarzeń z uwzględnieniem wielu kalendarzy:

```
Spotkanie: 2026-04-20
- Unikaj islamskiej piątkowej modlitwy
- Szanuj żydowski szabat
- Uwzględnij chińskie święta
```

---

## Najlepsze praktyki

### 1. Zawsze określaj kalendarz

Nigdy nie zakładaj systemu kalendarzowego:

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // Wyraźnie określ!
}
```

### 2. Obsługuj nieprawidłowe daty

Niektóre daty nie istnieją w niektórych kalendarzach:

```json
{
  "error": "Invalid date: February 30 doesn't exist"
}
```

### 3. Uwzględniaj strefy czasowe

Konwersja dat może się różnić w zależności od strefy czasowej:

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## Następne kroki

- 📚 Przeczytaj [Przewodnik architektury](architecture.md)
- 🛠️ Zobacz [Przewodnik rozwoju](development-guide.md)
- 🔧 Zobacz [Referencję narzędzi](tools-reference.md)
- 🚀 Zacznij z [Przewodnikiem szybkiego startu](getting-started.md)
