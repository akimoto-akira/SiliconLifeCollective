# Sistema di Calendario

> **Versione: v0.2.0-alpha**

[English](../en/calendar-system.md) | [Deutsch](../de-DE/calendar-system.md) | [中文](../zh-CN/calendar-system.md) | [繁體中文](../zh-HK/calendar-system.md) | [Español](../es-ES/calendar-system.md) | [日本語](../ja-JP/calendar-system.md) | [한국어](../ko-KR/calendar-system.md) | [Čeština](../cs-CZ/calendar-system.md) | [Русский](../ru-RU/calendar-system.md) | **Italiano**

## Panoramica

SiliconLifeCollective supporta 32 diversi sistemi calendariali per la conversione delle date e la precisione storica.

## Calendari Supportati (32)

### Calendari Principali (6)

1. **Calendario Gregoriano (Gregorian)** - Calendario internazionale standard, ID: `gregorian`
2. **Calendario Lunare Cinese (Chinese Lunar)** - Calendario tradizionale cinese, con calcolo del mese intercalare, ID: `lunar`
3. **Calendario Islamico (Islamic)** - Calendario Hijri islamico, ID: `islamic`
4. **Calendario Ebraico (Hebrew)** - Calendario ebraico, ID: `hebrew`
5. **Calendario Persiano (Persian)** - Calendario solare iraniano, ID: `persian`
6. **Calendario Indiano (Indian)** - Calendario nazionale indiano, ID: `indian`

### Calendari Storici Cinesi (2)

7. **Calendario Storico Cinese (Chinese Historical)** - Supporta la cronologia Ganzhi e le ere imperiali, ID: `chinese_historical`
   - **Cronologia Ganzhi**: Ciclo di 60 anni (Tronchi Celesti + Rami Terrestri)
   - **Ere imperiali**: Supporta le ere delle dinastie della storia cinese (Kangxi, Qianlong, Zhenguan, ecc.)
   - **Database dinamico**: Database integrato completo delle dinastie e delle ere della storia cinese
8. **Calendario Ganzhi (Sexagenary)** - Ciclo sessagenario di 60 anni, ID: `sexagenary`

### Calendari dell'Asia Orientale (6)

9. **Calendario Giapponese (Japanese)** - Calendario delle ere giapponesi (Nengo), ID: `japanese`
10. **Calendario Vietnamita (Vietnamese)** - Calendario lunare vietnamita (variante con il Gatto come zodiaco), ID: `vietnamese`
11. **Calendario Tibetano (Tibetan)** - Sistema calendariale tibetano, ID: `tibetan`
12. **Calendario Mongolo (Mongolian)** - Calendario mongolo, ID: `mongolian`
13. **Calendario Dai (Dai)** - Calendario Dai, con calcolo lunare completo, ID: `dai`
14. **Calendario Dai di Dehong (Dehong Dai)** - Variante del calendario Dai di Dehong, ID: `dehong_dai`

### Calendari Storici (6)

15. **Calendario Maya (Mayan)** - Calendario lungo maya, ID: `mayan`
16. **Calendario Romano (Roman)** - Calendario romano antico, ID: `roman`
17. **Calendario Giuliano (Julian)** - Calendario giuliano, ID: `julian`
18. **Calendario Repubblicano Francese (French Republican)** - Calendario della Rivoluzione Francese, ID: `french_republican`
19. **Calendario Copto (Coptic)** - Calendario della Chiesa copta ortodossa, ID: `coptic`
20. **Calendario Etiope (Ethiopian)** - Calendario etiope, ID: `ethiopian`

### Calendari Regionali (6)

21. **Calendario Buddhista (Buddhist)** - Era buddista (BE), anno + 543, ID: `buddhist`
22. **Calendario Saka (Saka)** - Era Saka (Indonesia), ID: `saka`
23. **Calendario Vikram Samvat (Vikram Samvat)** - Calendario indù, ID: `vikram_samvat`
24. **Calendario Giavanese (Javanese)** - Calendario islamico giavanese, ID: `javanese`
25. **Calendario Chula Sakarat (Chula Sakarat)** - Calendario buddista del Sud-est asiatico, anno - 638, ID: `chula_sakarat`
26. **Calendario Khmer (Khmer)** - Calendario khmer, ID: `khmer`

### Calendari Moderni (3)

27. **Calendario della Repubblica di Cina (ROC)** - Calendario della Repubblica, anno - 1911, ID: `roc`
28. **Calendario Juche (Juche)** - Calendario nord-coreano, anno - 1911, ID: `juche`
29. **Calendario Zoroastriano (Zoroastrian)** - Calendario zoroastriano, ID: `zoroastrian`

### Calendari Etnici (3)

30. **Calendario Yi (Yi)** - Sistema calendariale Yi, ID: `yi`
31. **Calendario Cherokee (Cherokee)** - Calendario Cherokee, ID: `cherokee`
32. **Calendario Inuit (Inuit)** - Calendario Inuit, ID: `inuit`

---

## Utilizzo dello Strumento Calendario

### Conversione Base

```json
{
  "action": "convert",
  "date": "2026-04-20",
  "fromCalendar": "gregorian",
  "toCalendar": "chinese_lunar"
}
```

**Risposta**:
```json
{
  "result": "农历丙午年四月初三",
  "year": 2026,
  "month": 4,
  "day": 3,
  "leapMonth": false
}
```

### Query Multi-Calendario

```json
{
  "action": "query_all",
  "date": "2026-04-20"
}
```

**Risposta**: Restituisce la data in tutti i 32 sistemi calendariali.

---

## API del Calendario

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

### Esempio: Calendario Personalizzato

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

## Funzionalità Speciali

### Dettagli del Calendario Storico Cinese (Nuovo)

Il calendario storico cinese è una delle caratteristiche principali di questo sistema, e supporta due funzionalità fondamentali:

#### 1. Sistema di Cronologia Ganzhi

Adotta un ciclo di 60 anni, formato dalla combinazione di Tronchi Celesti e Rami Terrestri:

```
Tronchi Celesti (10): 甲、乙、丙、丁、戊、己、庚、辛、壬、癸
Rami Terrestri (12): 子、丑、寅、卯、辰、巳、午、未、申、酉、戌、亥
```

**Esempi**:
- 2026 = 丙午年
- 2025 = 乙巳年 (Anno del Serpente)
- 2024 = 甲辰年 (Anno del Drago)

**Esempio di utilizzo**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**Risposta**:
```json
{
  "result": "丙午年 三月 初九",
  "ganzhi_year": "丙午",
  "zodiac": "马"
}
```

#### 2. Sistema delle Ere Imperiali

Database integrato completo delle dinastie e delle ere imperiali della storia cinese:

**Dinastie supportate** (in parte):
- Dinastia Qing: Kangxi, Yongzheng, Qianlong, Jiaqing, Daoguang, Xianfeng, Tongzhi, Guangxu, Xuantong
- Dinastia Ming: Hongwu, Yongle, Jiajing, Wanli, Chongzhen, ecc.
- Dinastia Tang: Zhenguan, Kaiyuan, Tianbao, ecc.
- Dinastia Han: Jianyuan, Yuanguang, Yuanshuo, ecc.
- Altre dinastie...

**Esempio di utilizzo**:
```json
{
  "action": "convert",
  "date": "1721-04-12",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_historical"
}
```

**Risposta**:
```json
{
  "result": "康熙六十年 三月 十五日",
  "era": "康熙",
  "era_year": 60,
  "dynasty": "清"
}
```

### Gestione del Mese Intercalare

Calendari con mese intercalare:
- Calendario Lunare Cinese
- Calendario Ebraico
- Calendario Buddhista
- Calendario Vietnamita

```json
{
  "isLeapMonth": true,
  "month": 4,
  "note": "闰四月"
}
```

---

## Accuratezza del Calendario

### Calcoli Astronomici

- Basati su dati astronomici reali
- Supporto per date storiche
- Gestione delle riforme calendariali

### Limitazioni Note

- Alcune date antiche sono approssimative
- Le riforme calendariali variano per regione
- Non include la gestione dei secondi intercalari

---

## Casi d'Uso

### Ricerca Storica

Convertire date storiche in calendari moderni:

```
Domanda: "Quando è avvenuta la Rivoluzione Francese?"
Risposta: "14 luglio 1789 (calendario gregoriano)"
         "26 Termidoro dell'Anno I (calendario repubblicano francese)"
```

### Applicazioni Culturali

Supporto per le festività tradizionali:

```
Capodanno cinese 2026:
- Calendario gregoriano: 17 febbraio 2026
- Calendario lunare: Primo giorno del primo mese
```

### Pianificazione Multiculturale

Organizzare eventi che rispettino più calendari:

```
Riunione: 2026-04-20
- Evitare la preghiera del venerdì islamica
- Rispettare lo Shabbat ebraico
- Considerare le festività cinesi
```

---

## Best Practices

### 1. Specificare Sempre il Calendario

Non dare mai per scontato il sistema calendariale:

```json
{
  "date": "2026-04-20",
  "calendar": "gregorian"  // Specifica esplicitamente!
}
```

### 2. Gestire le Date Non Valide

Alcune date non esistono in certi calendari:

```json
{
  "error": "Invalid date: February 30 doesn't exist"
}
```

### 3. Considerare i Fusi Orari

La conversione delle date può variare in base al fuso orario:

```
2026-04-20 23:00 UTC+8
≠
2026-04-20 23:00 UTC-5
```

---

## Prossimi Passi

- 📚 Leggi la [Guida all'Architettura](architecture.md)
- 🛠️ Consulta la [Guida allo Sviluppo](development-guide.md)
- 🔧 Consulta il [Riferimento degli Strumenti](tools-reference.md)
- 🚀 Consulta la [Guida Rapida](getting-started.md)
