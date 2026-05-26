# System kalendarza

> **Wersja: v0.2.0-alpha**

[English](../en/calendar-system.md) | [Deutsch](../de-DE/calendar-system.md) | [中文](../zh-CN/calendar-system.md) | [繁體中文](../zh-HK/calendar-system.md) | [Español](../es-ES/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | [Čeština](../cs-CZ/calendar-system.md) | [Русский](../ru-RU/calendar-system.md)

## Przegląd

SiliconLifeCollective obsługuje 32 różne systemy kalendarzowe do konwersji dat i zapewnienia dokładności historycznej.

## Obsługiwane kalendarze (32)

### Główne kalendarze (6)

1. **Gregoriański (Gregorian)** — międzynarodowy kalendarz standardowy, ID: `gregorian`
2. **Chiński księżycowy (Chinese Lunar)** — tradycyjny kalendarz chiński, z obliczeniami miesięcy przestępnych, ID: `lunar`
3. **Islamski (Islamic)** — islamski kalendarz hidżry, ID: `islamic`
4. **Hebrajski (Hebrew)** — kalendarz żydowski, ID: `hebrew`
5. **Perski (Persian)** — irański kalendarz słoneczny, ID: `persian`
6. **Indyjski (Indian)** — indyjski kalendarz narodowy, ID: `indian`

### Chińskie kalendarze historyczne (2)

7. **Chiński historyczny (Chinese Historical)** — obsługa er ganzhi i epok cesarskich, ID: `chinese_historical`
   - **Ery ganzhi**: 60-letni cykl (ciała niebieskie + gałęzie ziemskie)
   - **Epoki cesarskie**: obsługa er cesarskich poszczególnych chińskich dynastii (Kangxi, Qianlong, Zhenguan itp.)
   - **Dynamiczna baza danych**: wbudowana kompletna baza chińskich dynastii historycznych i er cesarskich
8. **Ganzhi (Sexagenary)** — 60-letni cykl ganzhi, ID: `sexagenary`

### Kalendarze wschodnioazjatyckie (6)

9. **Japoński (Japanese)** — kalendarz ery japońskiej (Nengo), ID: `japanese`
10. **Wietnamski (Vietnamese)** — wietnamski kalendarz księżycowy (wariant z kotem zodiakalnym), ID: `vietnamese`
11. **Tybetański (Tibetan)** — system kalendarza tybetańskiego, ID: `tibetan`
12. **Mongolski (Mongolian)** — kalendarz mongolski, ID: `mongolian`
13. **Dai** — kalendarz Dai, z pełnymi obliczeniami księżycowymi, ID: `dai`
14. **Dai Dehong (Dehong Dai)** — wariant kalendarza Dai Dehong, ID: `dehong_dai`

### Kalendarze historyczne (6)

15. **Majski (Mayan)** — majski kalendarz długiej rachuby, ID: `mayan`
16. **Rzymski (Roman)** — starożytny kalendarz rzymski, ID: `roman`
17. **Juliański (Julian)** — kalendarz juliański, ID: `julian`
18. **Francuski republikański (French Republican)** — francuski kalendarz rewolucyjny, ID: `french_republican`
19. **Koptyjski (Coptic)** — kalendarz kościoła koptyjskiego, ID: `coptic`
20. **Etiopski (Ethiopian)** — kalendarz etiopski, ID: `ethiopian`

### Kalendarze regionalne (6)

21. **Buddyjski (Buddhist)** — era buddyjska (BE), rok + 543, ID: `buddhist`
22. **Saka** — era Saka (Indonezja), ID: `saka`
23. **Vikram Samvat** — kalendarz hinduski, ID: `vikram_samvat`
24. **Jawajski (Javanese)** — jawajski kalendarz islamski, ID: `javanese`
25. **Chula Sakarat** — buddyjski kalendarz Azji Południowo-Wschodniej, rok - 638, ID: `chula_sakarat`
26. **Khmerski (Khmer)** — kalendarz khmerski, ID: `khmer`

### Kalendarze nowoczesne (3)

27. **Republiki Chińskiej (ROC)** — kalendarz MinGuo, rok - 1911, ID: `roc`
28. **Dżucze (Juche)** — kalendarz północnokoreański, rok - 1911, ID: `juche`
29. **Zaratusztriański (Zoroastrian)** — kalendarz zaratusztriański, ID: `zoroastrian`

### Kalendarze etniczne (3)

30. **Yi** — system kalendarza Yi, ID: `yi`
31. **Czirokei (Cherokee)** — kalendarz Czirokei, ID: `cherokee`
32. **Inuitów (Inuit)** — kalendarz Inuitów, ID: `inuit`

---

## Korzystanie z narzędzia kalendarza

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
  "result": "Chiński księżycowy: trzeci dzień czwartego miesiąca roku Bingwu",
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

**Odpowiedź**: zwraca datę we wszystkich 32 systemach kalendarzowych.

---

## API kalendarza

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

### Przykład: niestandardowy kalendarz

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

Chiński kalendarz historyczny jest jedną z głównych atrakcji tego systemu, obsługując dwie kluczowe funkcje:

#### 1. System er ganzhi

Wykorzystuje 60-letni cykl, utworzony z kombinacji ciał niebieskich i gałęzi ziemskich:

```
Ciała niebieskie (10): 甲, 乙, 丙, 丁, 戊, 己, 庚, 辛, 壬, 癸
Gałęzie ziemskie (12): 子, 丑, 寅, 卯, 辰, 巳, 午, 未, 申, 酉, 戌, 亥
```

**Przykłady**:
- Rok 2026 = rok Bingwu
- Rok 2025 = rok Yisi (rok węża)
- Rok 2024 = rok Jiachen (rok smoka)

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
  "result": "Rok Bingwu, trzeci miesiąc, dziewiąty dzień",
  "ganzhi_year": "Bingwu",
  "zodiac": "Koń"
}
```

#### 2. System er cesarskich

Wbudowana kompletna baza chińskich dynastii historycznych i er cesarskich:

**Obsługiwane dynastie** (częściowo):
- Dynastia Qing: Kangxi, Yongzheng, Qianlong, Jiaqing, Daoguang, Xianfeng, Tongzhi, Guangxu, Xuantong
- Dynastia Ming: Hongwu, Yongle, Jiajing, Wanli, Chongzhen itp.
- Dynastia Tang: Zhenguan, Kaiyuan, Tianbao itp.
- Dynastia Han: Jianyuan, Yuanguang, Yuanshuo itp.
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
  "result": "Era Kangxi, rok 60, trzeci miesiąc, piętnasty dzień",
  "era": "Kangxi",
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
  "note": "Przestępny czwarty miesiąc"
}
```

---

## Dokładność kalendarza

### Obliczenia astronomiczne

- Oparte na rzeczywistych danych astronomicznych
- Obsługa dat historycznych
- Obsługa reform kalendarzowych

### Znane ograniczenia

- Niektóre starożytne daty są przybliżone
- Reformy kalendarzowe różnią się w zależności od regionu
- Brak obsługi sekund przestępnych

---

## Przypadki użycia

### Badania historyczne

Konwersja dat historycznych na współczesne kalendarze:

```
Pytanie: "Kiedy była rewolucja francuska?"
Odpowiedź: "14 lipca 1789 (gregoriański)"
   "26 thermidor roku I (francuski republikański)"
```

### Zastosowania kulturalne

Obsługa tradycyjnych świąt:

```
Chiński Nowy Rok 2026:
- Gregoriański: 17 lutego 2026
- Chiński księżycowy: pierwszy dzień pierwszego miesiąca
```

### Planowanie wielokulturowe

Planowanie wydarzeń z uwzględnieniem wielu kalendarzy:

```
Spotkanie: 2026-04-20
- Unikanie islamskiej piątkowej modlitwy
- Poszanowanie żydowskiego szabatu
- Uwzględnienie chińskich dni wolnych
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

Konwersje dat mogą się różnić w zależności od strefy czasowej:

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## Następne kroki

- 📚 Przeczytaj [przewodnik architektury](architecture.md)
- 🛠️ Zobacz [przewodnik deweloperski](development-guide.md)
- 🔧 Zobacz [referencję narzędzi](tools-reference.md)
- 🚀 Zacznij od [przewodnika szybkiego startu](getting-started.md)
