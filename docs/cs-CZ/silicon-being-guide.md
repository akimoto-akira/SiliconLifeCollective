# Průvodce Silikonovou Bytostí

[English](../en/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | **Čeština**

## Přehled

Silikonové bytosti jsou AI-powered agenti, kteří mohou samostatně myslet, jednat a vyvíjet se.

## Architektura

### Oddělení Tělo-Mozek

```
┌─────────────────────────────────────┐
│         Silikonová bytost            │
├──────────────────┬──────────────────┤
│   Tělo            │   Mozek           │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • Správa stavu    │ • Načtení historie│
│ • Detekce spouště │ • Volání AI       │
│ • Životní cyklus  │ • Provádění nástrojů│
│                  │ • Persist odpovědí │
└──────────────────┴──────────────────┘
```

## Soubor duše

### Struktura

```markdown
# Název bytosti

## Osobnost
Popište osobnostní rysy a charakteristiky bytosti.

## Schopnosti
Vymenujte, co tato bytost dokáže.

## Pravidla chování
Definujte, jak by se bytost měla chovat v různých situacích.

## Znalostní doména
Specifikujte oblast odbornosti bytosti.
```

### Příklad

```markdown
# Asistent pro revizi kódu

## Osobnost
Jste pečlivý recenzent kódu s 10 lety zkušeností.
Poskytujete konstruktivní feedback a vždy vysvětlujete své zdůvodnění.

## Schopnosti
- Revize kódu pro chyby a nejlepší postupy
- Návrh optimalizací výkonu
- Vysvětlení komplexních algoritmů
- Identifikace bezpečnostních vulnerabilit

## Pravidla chování
- Začněte pozitivními pozorováními
- Poskytněte konkrétní příklady
- Vysvětlete, proč jsou změny potřeba
- Buďte respektující a profesionální

## Znalostní doména
Specializace na C#, .NET a softwarovou architekturu.
```

## Vytvoření bytosti

### Přes Web UI

1. Navigujte na **Správa bytostí**
2. Klikněte na **Vytvořit novou bytost**
3. Vyplňte:
   - Název
   - Obsah duše
   - Konfigurační možnosti
4. Klikněte na **Vytvořit**

### Přes API

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Asistent",
    "soul": "# Osobnost\nJste užitečný..."
  }'
```

## Životní cyklus bytosti

### Stavy

```
Vytvořeno → Spouštění → Běží → Zastavování → Zastaveno
                    ↓
                  Chyba
```

### Operace

- **Start**: Inicializace a začátek zpracování
- **Stop**: Elegantní vypnutí
- **Pozastavit**: Dočasné pozastavení (zachování stavu)
- **Obnovit**: Pokračování z pozastaveného stavu

## Systém úkolů

### Vytvoření úkolu

```csharp
var task = new BeingTask
{
    BeingId = being.Id,
    Description = "Zkontrolujte kód",
    Priority = 5,
    DueDate = DateTime.UtcNow.AddHours(2)
};

await taskSystem.CreateAsync(task);
```

### Stavy úkolů

- `Pending` - Čeká na provedení
- `Running` - Právě se provádí
- `Completed` - Úspěšně dokončeno
- `Failed` - Provedení selhalo
- `Cancelled` - Ručně zrušeno

## Systém timerů

### Typy timerů

1. **Jednorázový**: Provede se jednou po zpoždění
2. **Interval**: Opakuje se v pevných intervalech
3. **Cron**: Provede se na základě cron výrazu

### Příklad

```csharp
// Provede se každou hodinu
var timer = new BeingTimer
{
    BeingId = being.Id,
    Interval = TimeSpan.FromHours(1),
    Action = "think",
    Repeat = true
};

await timerSystem.StartAsync(timer);
```

## Systém paměti

### Typy paměti

- **Krátkodobá**: Kontext aktuální konverzace
- **Dlouhodobá**: Persistované znalosti a zkušenosti
- **Epizodická**: Časově indexované události a interakce

### Struktura úložiště

```
data/
└── beings/
    └── {being-id}/
        ├── soul.md
        ├── memory/
        │   ├── short-term.json
        │   └── long-term/
        │       ├── 2026-04-20.json
        │       └── 2026-04-21.json
        └── tasks/
            └── task-history.json
```

## Systém pracovních poznámek

### Přehled

Pracovní poznámky jsou osobní deníkový systém silikonových bytostí, navržený jako stránkový systém pro zaznamenávání pracovního pokroku, studijních poznámek, projektových poznámek atd.

### Funkce

- **Správa stránek**: Každá poznámka je samostatná stránka, přístupná podle čísla stránky
- **Podpora Markdown**: Obsah podporuje formát Markdown (text, seznamy, tabulky, bloky kódu)
- **Indexace klíčových slov**: Podpora přidávání klíčových slov k poznámkám pro snadné vyhledávání
- **Funkce shrnutí**: Každá poznámka má krátké shrnutí pro rychlé prohlížení
- **Generování obsahu**: Lze generovat obsah všech poznámek pro pochopení celkového kontextu
- **Časové značky**: Automatické zaznamenávání času vytvoření a aktualizace
- **Výchozí soukromé**: Přístupné pouze samotné bytosti (kurátor může spravovat)

### Použití scénáře

1. **Záznam pokroku projektu**
   ```
   Shrnutí: Dokončen modul autentizace uživatelů
   Obsah: Implementováno ověřování JWT token, integrace OAuth2, mechanismus refresh tokenu
   Klíčová slova: autentizace,JWT,OAuth2
   ```

2. **Studijní poznámky**
   ```
   Shrnutí: Studium nejlepších postupů C# asynchronního programování
   Obsah: Poznámky k použití async/await, scénáře použití ConfigureAwait...
   Klíčová slova: C#,asynchronní,nejlepší postupy
   ```

3. **Zápisy z meetingů**
   ```
   Shrnutí: Diskuse o požadavcích produktu
   Obsah: Diskutovány požadavky nové funkce, určeno implementační řešení...
   Klíčová slova: produkt,požadavky,meeting
   ```

### Použití přes nástroj

Bytosti mohou spravovat pracovní poznámky pomocí nástroje `work_note`:

```json
// Vytvořit poznámku
{
  "action": "create",
  "summary": "Dokončen modul autentizace uživatelů",
  "content": "## Implementační detaily\n\n- Použito JWT token\n- Podpora OAuth2",
  "keywords": "autentizace,JWT,OAuth2"
}

// Číst poznámku
{
  "action": "read",
  "page_number": 1
}

// Vyhledat poznámky
{
  "action": "search",
  "keyword": "autentizace",
  "max_results": 10
}
```

### Správa přes Web UI

1. Navigujte na **Správa bytostí** → Vyberte bytost
2. Klikněte na záložku **Pracovní poznámky**
3. Můžete zobrazit, vyhledávat, upravovat poznámky
4. Podpora náhledu Markdown

## Systém sítě znalostí

### Přehled

Síť znalostí je systém reprezentace a správy znalostí založený na struktuře trojic (předmět-přísudek-objekt) pro ukládání a správu strukturovaných znalostí.

### Základní koncepty

#### Struktura trojic

```
Předmět (Subject) --Přísudek (Predicate)--> Objekt (Object)
```

**Příklady**:
- `Python` --`is_a`--> `programovací_jazyk`
- `Praha` --`capital_of`--> `Česká republika`
- `Voda` --`boiling_point`--> `100°C`

#### Úroveň důvěry

Každá znalostní trojice má skóre důvěry (0.0-1.0), které indikuje úroveň spolehlivosti znalosti:
- `1.0`: Absolutně jisté (jako matematické teorémy)
- `0.8-0.99`: Vysoce důvěryhodné (jako ověřené fakty)
- `0.5-0.79`: Středně důvěryhodné (jako inference nebo hypotézy)
- `<0.5`: Nízká důvěra (jako odhady nebo neověřené informace)

#### Systém štítků

Podpora přidávání štítků k trojicím pro klasifikaci a vyhledávání:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programovací_jazyk",
  "tags": ["programování", "jazyk", "populární"]
}
```

### Operace znalostí

#### 1. Přidání znalosti

```json
{
  "action": "add",
  "subject": "C#",
  "predicate": "created_by",
  "object": "Microsoft",
  "confidence": 1.0,
  "tags": ["programování", "jazyk"]
}
```

#### 2. Dotazování znalostí

```json
{
  "action": "query",
  "subject": "C#",
  "predicate": "created_by"
}
```

#### 3. Vyhledávání znalostí

```json
{
  "action": "search",
  "query": "programovací jazyk",
  "limit": 10
}
```

#### 4. Objevování cest znalostí

Najděte cestu propojení mezi dvěma koncepty:
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "informatika"
}
```

Návrat:
```
Python → is_a → programovací_jazyk → belongs_to → informatika
```

#### 5. Validace znalostí

Zkontrolujte platnost a konzistenci znalostí:
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programovací_jazyk"
}
```

#### 6. Statistiky znalostí

Získejte celkové statistické informace o síti znalostí:
```json
{
  "action": "stats"
}
```

Návrat:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Použití scénáře

1. **Ukládání faktů**
   - Ukládání objektivních faktů a obecných znalostí
   - Příklad: `Země` --`is_a`--> `planeta`

2. **Konceptuální vztahy**
   - Záznam vztahů mezi koncepty
   - Příklad: `Dědičnost` --`is_a`--> `Koncept objektově orientovaného programování`

3. **Akumulace učení**
   - Bytosti neustále akumulují znalosti prostřednictvím učení
   - Tvoří strukturovaný systém znalostí

4. **Podpora inference**
   - Objevování nepřímých vztahů prostřednictvím cest znalostí
   - Podpora inference a rozhodování založeného na znalostech

### Správa přes Web UI

1. Navigujte na stránku **Síť znalostí**
2. Zobrazte statistické informace o znalostech
3. Vyhledávejte a procházejte znalosti
4. Vizualizace grafu vztahů znalostí (plánováno)

## Operace WebView prohlížeče (Nové)

### Přehled

Silikonové bytosti mohou samostatně procházet webové stránky, získávat informace a provádět webové operace pomocí nástroje WebView prohlížeče. Prohlížeč běží v headless režimu, zcela neviditelný pro uživatele.

### Funkce

- **Individuální izolace**: Každá bytost má vlastní instanci prohlížeče, cookies a relace
- **Headless režim**: Autonomní operace na pozadí, neviditelné pro uživatele
- **Plná funkčnost**: Podpora provádění JavaScript, vykreslování CSS, vyplňování formulářů atd.
- **Bezpečnostní kontrola**: Všechny operace musí projít řetězcem ověřování oprávnění

### Běžné operace

#### 1. Otevření prohlížeče

```json
{
  "action": "open_browser"
}
```

#### 2. Navigace na webovou stránku

```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

#### 3. Získání obsahu stránky

```json
{
  "action": "get_page_text"
}
```

Návrat textového obsahu stránky pro analýzu a pochopení AI.

#### 4. Kliknutí na prvek

```json
{
  "action": "click",
  "selector": "#submit-button"
}
```

#### 5. Vložení textu

```json
{
  "action": "input",
  "selector": "#search-input",
  "text": "Vyhledávací klíčové slovo"
}
```

#### 6. Provedení JavaScript

```json
{
  "action": "execute_script",
  "script": "return document.title;"
}
```

#### 7. Získání screenshotu

```json
{
  "action": "get_screenshot"
}
```

Návrat screenshotu stránky (kódováno Base64), lze použít pro vizuální analýzu.

#### 8. Čekání na zobrazení prvku

```json
{
  "action": "wait_for_element",
  "selector": ".loading-complete",
  "timeout": 10000
}
```

### Použití scénáře

1. **Získávání informací**
   - Procházení zpravodajských webů pro získání nejnovějších zpráv
   - Dotazování dokumentace a technických materiálů
   - Monitorování změn obsahu webové stránky

2. **Automatizované operace**
   - Vyplňování a odesílání formulářů
   - Kliknutí na tlačítka pro spuštění operací
   - Scrapování webových dat

3. **Webová analýza**
   - Analýza struktury a obsahu stránky
   - Extrakce specifických informací
   - Vizuální analýza screenshotu stránky

### Poznámky

- Operace prohlížeče mohou být pomalejší, je třeba počkat na dokončení načítání stránky
- Použijte `wait_for_element` pro zajištění, že se prvek zobrazil před operací
- Dodržujte podmínky použití webových stránek a robots.txt
- Vyhněte se častým požadavkům, které mohou vést k zákazu

## Nejlepší postupy

### Psaní souboru duše

1. **Konkrétní**: Jasné osobnostní rysy a hranice
2. **Definujte rozsah**: Co by bytost měla a neměla dělat
3. **Zahrňte příklady**: Ukažte očekávané vzorce chování
4. **Pravidelně aktualizujte**: Evolvujte duši na základě výkonu

### Správa úkolů

1. **Nastavte priority**: Použijte prioritu (1-10)
2. **Definujte termíny**: Vždy nastavte termín
3. **Sledujte pokrok**: Pravidelně kontrolujte stav úkolů
4. **Zpracovávejte selhání**: Implementujte logiku opakování

### Optimalizace paměti

1. **Čistěte stará data**: Pravidelně archivujte staré paměti
2. **Indexujte důležité informace**: Označte klíčové informace
3. **Použijte časové úložiště**: Využijte dotazy s časovým indexem

## Odstraňování problémů

### Bytost se nemůže spustit

**Zkontrolujte**:
- Soubor duše existuje a je platný
- AI klient je nakonfigurován
- Systémové zdroje jsou dostatečné

### Bytost se neočekávaně zastaví

**Zkontrolujte**:
- Chyby v logu
- Dostupnost AI služby
- Využití paměti

### Úkol se neprovede

**Zkontrolujte**:
- Systém timerů běží
- Priorita a plán úkolu
- Nastavení oprávnění

## Další kroky

- 📚 Přečtěte si [Průvodce architekturou](architecture.md)
- 🛠️ Podívejte se na [Vývojový průvodce](development-guide.md)
- 🚀 Podívejte se na [Průvodce rychlým startem](getting-started.md)
