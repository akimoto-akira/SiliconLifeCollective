# Sistema di calendario

> **Versione : v0.1.0-alpha**

[English](../en/calendar-system.md) | [Deutsch](../de-DE/calendar-system.md) | [Français](../fr-FR/calendar-system.md) | [中文](../zh-CN/calendar-system.md) | [繁體中文](../zh-HK/calendar-system.md) | [Español](../es-ES/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | [Čeština](../cs-CZ/calendar-system.md) | **Italiano**

## Panoramica

SiliconLifeCollective supporta 32 diversi sistemi di calendario per la conversione delle date e la precisione storica.

## Calendari supportati (32)

### Calendari principali (6)

1. **Calendario gregoriano (Gregorian)** - Calendario standard internazionale, ID : `gregorian`
2. **Calendario lunare cinese (Chinese Lunar)** - Calendario cinese tradizionale con calcolo dei mesi intercalari, ID : `lunar`
3. **Calendario islamico (Islamic)** - Calendario dell'Egira islamica, ID : `islamic`
4. **Calendario ebraico (Hebrew)** - Calendario ebraico, ID : `hebrew`
5. **Calendario persiano (Persian)** - Calendario solare iraniano, ID : `persian`
6. **Calendario indiano (Indian)** - Calendario nazionale indiano, ID : `indian`

### Calendari storici cinesi (2)

7. **Calendario storico cinese (Chinese Historical)** - Supporta il ciclo Ganzhi e le ere imperiali, ID : `chinese_historical`
   - **Ciclo Ganzhi** : Ciclo di 60 anni (Steli celesti + Rami terrestri)
   - **Ere imperiali** : Supporta le ere di tutte le dinastie cinesi (Kangxi, Qianlong, Zhenguan, ecc.)
   - **Database dinamico** : Database completo integrato di dinastie ed ere cinesi
8. **Calendario sessagesimale (Sexagenary)** - Ciclo Ganzhi di 60 anni, ID : `sexagenary`

### Calendari dell'Asia orientale (6)

9. **Calendario giapponese (Japanese)** - Calendario delle ere Nengo giapponesi, ID : `japanese`
10. **Calendario vietnamita (Vietnamese)** - Calendario lunare vietnamita (variante zodiacale del Gatto), ID : `vietnamese`
11. **Calendario tibetano (Tibetan)** - Sistema di calendario tibetano, ID : `tibetan`
12. **Calendario mongolo (Mongolian)** - Calendario mongolo, ID : `mongolian`
13. **Calendario Dai (Dai)** - Calendario Dai con calcolo lunare completo, ID : `dai`
14. **Calendario Dai Dehong (Dehong Dai)** - Variante Dai Dehong, ID : `dehong_dai`

### Calendari storici (6)

15. **Calendario maya (Mayan)** - Calendario lungo maya, ID : `mayan`
16. **Calendario romano (Roman)** - Calendario romano antico, ID : `roman`
17. **Calendario giuliano (Julian)** - Calendario giuliano, ID : `julian`
18. **Calendario repubblicano francese (French Republican)** - Calendario della Rivoluzione francese, ID : `french_republican`
19. **Calendario copto (Coptic)** - Calendario copto ortodosso, ID : `coptic`
20. **Calendario etiope (Ethiopian)** - Calendario etiope, ID : `ethiopian`

### Calendari regionali (6)

21. **Calendario buddista (Buddhist)** - Era buddista (BE), anno + 543, ID : `buddhist`
22. **Calendario Saka (Saka)** - Era Saka (Indonesia), ID : `saka`
23. **Calendario Vikram Samvat (Vikram Samvat)** - Calendario indù, ID : `vikram_samvat`
24. **Calendario giavanese (Javanese)** - Calendario islamico giavanese, ID : `javanese`
25. **Calendario Chula Sakarat (Chula Sakarat)** - Calendario buddista del sud-est asiatico, anno - 638, ID : `chula_sakarat`
26. **Calendario khmer (Khmer)** - Calendario khmer, ID : `khmer`

### Calendari moderni (3)

27. **Calendario ROC (ROC)** - Calendario Minguo, anno - 1911, ID : `roc`
28. **Calendario Juche (Juche)** - Calendario nordcoreano, anno - 1911, ID : `juche`
29. **Calendario zoroastriano (Zoroastrian)** - Calendario zoroastriano, ID : `zoroastrian`

### Calendari etnici (3)

30. **Calendario Yi (Yi)** - Sistema di calendario Yi, ID : `yi`
31. **Calendario Cherokee (Cherokee)** - Calendario Cherokee, ID : `cherokee`
32. **Calendario Inuit (Inuit)** - Calendario Inuit, ID : `inuit`

---

## Utilizzare lo strumento calendario

### Conversione di base

```json
{
  "action": "convert",
  "date": "2026-04-20",
  "fromCalendar": "gregorian",
  "toCalendar": "chinese_lunar"
}
```

**Risposta** :
```json
{
  "result": "农历丙午年四月初三",
  "year": 2026,
  "month": 4,
  "day": 3,
  "leapMonth": false
}
```

### Richiesta multi-calendario

```json
{
  "action": "query_all",
  "date": "2026-04-20"
}
```

**Risposta** : Restituisce la data in tutti i 32 sistemi di calendario.

---

## API di calendario

### Interfaccia CalendarBase

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

### Esempio : Calendario personalizzato

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Logica di conversione
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Conversione inversa
        return new GregorianDate(year, month, day);
    }
}
```

---

## Funzionalità speciali

### Calendario storico cinese in dettaglio (Nuovo)

Il calendario storico cinese è una funzionalità di punta del sistema con due funzioni principali :

#### 1. Sistema di ciclo annuale Ganzhi

Utilizza un ciclo di 60 anni, combinando gli Steli celesti e i Rami terrestri :

```
Steli celesti (10) : 甲、乙、丙、丁、戊、己、庚、辛、壬、癸
Rami terrestri (12) : 子、丑、寅、卯、辰、巳、午、未、申、酉、戌、亥
```

**Esempi** :
- 2026 = 丙午年
- 2025 = 乙巳年 (Anno del Serpente)
- 2024 = 甲辰年 (Anno del Drago)

**Esempio di utilizzo** :
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**Risposta** :
```json
{
  "result": "丙午年 三月 初九",
  "ganzhi_year": "丙午",
  "zodiac": "马"
}
```

#### 2. Sistema di ere imperiali

Database completo integrato di dinastie ed ere imperiali cinesi :

**Dinastie supportate** (estratto) :
- Dinastia Qing : Kangxi, Yongzheng, Qianlong, Jiaqing, Daoguang, Xianfeng, Tongzhi, Guangxu, Xuantong
- Dinastia Ming : Hongwu, Yongle, Jiajing, Wanli, Chongzhen, ecc.
- Dinastia Tang : Zhenguan, Kaiyuan, Tianbao, ecc.
- Dinastia Han : Jianyuan, Yuanguang, Yuanshuo, ecc.
- Altre dinastie...

**Esempio di utilizzo** :
```json
{
  "action": "convert",
  "date": "1721-04-12",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**Risposta** :
```json
{
  "result": "康熙六十年 三月 十五日",
  "era": "康熙",
  "era_year": 60,
  "dynasty": "清"
}
```

### Gestione dei mesi intercalari

Calendari con mesi intercalari :
- Calendario lunare cinese
- Calendario ebraico
- Calendario buddista
- Calendario vietnamita

```json
{
  "isLeapMonth": true,
  "month": 4,
  "note": "闰四月"
}
```

---

## Precisione dei calendari

### Calcoli astronomici

- Basati su dati astronomici reali
- Supporta le date storiche
- Gestisce le riforme calendariali

### Limitazioni note

- Alcune date antiche sono approssimative
- Le riforme calendariali variano in base alla regione
- Non include la gestione dei secondi intercalari

---

## Casi d'uso

### Ricerca storica

Convertire date storiche in calendari moderni :

```
Domanda : "Quando è avvenuta la Rivoluzione francese ?"
Risposta : "14 luglio 1789 (Gregoriano)"
           "26 Termidoro I (Repubblicano francese)"
```

### Applicazioni culturali

Supporto per le feste tradizionali :

```
Capodanno cinese 2026 :
- Gregoriano : 17 febbraio 2026
- Calendario lunare : 1° giorno del 1° mese
```

### Pianificazione multiculturale

Pianificare eventi tenendo conto di più calendari :

```
Riunione : 2026-04-20
- Evitare la preghiera del venerdì islamico
- Rispettare lo Shabbat ebraico
- Considerare le festività cinesi
```

---

## Buone pratiche

### 1. Specificare sempre il calendario

Non presupporre mai il sistema di calendario :

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // Specificare esplicitamente !
}
```

### 2. Gestire le date non valide

Alcune date non esistono in alcuni calendari :

```json
{
  "error": "Invalid date: February 30 doesn't exist"
}
```

### 3. Considerare i fusi orari

La conversione delle date può variare in base al fuso orario :

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## Prossimi passi

- 📚 Leggere la [guida all'architettura](architecture.md)
- 🛠️ Consultare la [guida di sviluppo](development-guide.md)
- 🔧 Vedere il [riferimento degli strumenti](tools-reference.md)
- 🚀 Iniziare con la [guida rapida](getting-started.md)
