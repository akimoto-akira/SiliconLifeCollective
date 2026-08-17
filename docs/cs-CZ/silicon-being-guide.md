# Příručka Křemíkové Bytosti

> **Verze: v0.2.0-alpha**

[English](../en/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | **Čeština** | [Русский](../ru-RU/silicon-being-guide.md)

## Přehled

Křemíková Bytost je AI řízený agent, který může autonomně přemýšlet, jednat a vyvíjet se.

## Architektura

### Oddělení Tělo-Mozek

```
┌─────────────────────────────────────┐
│         Křemíková Bytost            │
├──────────────────┬──────────────────┤
│   Tělo            │   Mozek          │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • Správa stavu    │ • Načítání historie │
│ • Detekce spouště │ • Volání AI      │
│ • Životní cyklus  │ • Provádění nástrojů │
│                  │ • Perzistence odpovědí │
└──────────────────┴──────────────────┘
```

## Soubor Duše

### Struktura

```markdown
# Being Name

## Personality
Describe the being's personality traits and characteristics.

## Capabilities
List what this being can do.

## Behavior Guidelines
Define how the being should behave in different situations.

## Knowledge Domain
Specify the being's area of expertise.
```

### Příklad

```markdown
# Code Review Assistant

## Personality
You are a meticulous code reviewer with 10 years of experience.
You provide constructive feedback and always explain your reasoning.

## Capabilities
- Review code for bugs and best practices
- Suggest performance optimizations
- Explain complex algorithms
- Identify security vulnerabilities

## Behavior Guidelines
- Start with positive observations
- Provide specific examples
- Explain why changes are needed
- Be respectful and professional

## Knowledge Domain
Specialized in C#, .NET, and software architecture.
```

## Vytvoření bytosti

### Přes Web UI

1. Přejděte na **Správa bytostí**
2. Klikněte na **Vytvořit novou bytost**
3. Vyplňte:
   - Název
   - Obsah duše
   - Možnosti konfigurace
4. Klikněte na **Vytvořit**

### Přes API

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistant",
    "soul": "# Personality\nYou are helpful..."
  }'
```

## Životní cyklus bytosti

### Aktivní stavy

Křemíková Bytost má následující aktivní stavy:

| Stav | Popis |
|------|------|
| `Idle` | Nečinný stav, čeká na aktivaci |
| `SingleChat` | Probíhá individuální chat |
| `GroupChat` | Probíhá skupinový chat |
| `Task` | Provádění úkolu |
| `Timer` | Provádění časovače |
| `Broadcast` | Zpracování broadcast zprávy |
| `Project` | Práce na projektu |
| `MemoryCompression` | Komprese paměti |
| `Stopped` | Zastaveno, z důvodu po sobě jdoucích chyb nebo ručního zastavení |

**Mechanismus stavu Stopped**:
- Když Křemíková Bytost zaznamená 10 po sobě jdoucích chyb, automaticky přejde do stavu `Stopped`
- Po přechodu do stavu Stopped bytost již neprovádí žádné úkoly
- Když dorazí nová chatovací zpráva, čítač chyb je resetován a bytost obnoví činnost
- Lze také restartovat ručním zásahem

### Přechody stavů

```
Idle → SingleChat → Idle (chat dokončen)
Idle → GroupChat → Idle (skupinový chat dokončen)
Idle → Task → Idle (úkol dokončen)
Idle → Timer → Idle (časovač dokončen)
Idle → Broadcast → Idle (broadcast dokončen)
Idle → Project → Idle (práce na projektu dokončena)
Idle → MemoryCompression → Idle (komprese paměti dokončena)
Jakýkoliv → Stopped (10 po sobě jdoucích chyb)
Stopped → Idle (nová chatovací zpráva dorazila nebo ruční restart)
```

### Operace

- **Spuštění**: Inicializace a zahájení zpracování
- **Zastavení**: Elegantní vypnutí
- **Restart**: Obnovení ze stavu Stopped do stavu Idle

## Systém úkolů

### Vytvoření úkolu

```csharp
var task = new BeingTask
{
    BeingId = being.Id,
    Description = "Review the code",
    Priority = 5,
    DueDate = DateTime.UtcNow.AddHours(2)
};

await taskSystem.CreateAsync(task);
```

### Stavy úkolu

- `Pending` - Čeká na provedení
- `Running` - Provádí se
- `SubmittedForReview` - Odesláno ke kontrole
- `UnderReview` - Probíhá kontrola
- `Rework` - Vráceno k přepracování
- `Completed` - Úspěšně dokončeno
- `Failed` - Provedení selhalo
- `Cancelled` - Ručně zrušeno

## Systém časovačů

### Typy časovačů

1. **Jednorázový**: Provede se jednou po zpoždění
2. **Intervalový**: Opakuje se v pevných intervalech
3. **Cron**: Provádění na základě cron výrazu

### Příklad

```csharp
// Provedení každou hodinu
var timer = new BeingTimer
{
    BeingId = being.Id,
    Interval = TimeSpan.FromHours(1),
    Action = "think",
    Repeat = true
};

await timerSystem.StartAsync(timer);
```

## Systém dovedností

Dovednost je opakovaně použitelná jednotka schopností Křemíkové Bytosti – zapouzdřuje "orchestraci nástrojů + šablonu promptu" do deklarovatelné, vyvíjející se a automaticky plánovatelné funkce, kterou AI volá stejně jako běžný nástroj.

### Struktura dovednosti

| Prvek | Popis |
|------|------|
| `id` / `description` | Jedinečný identifikátor a jednovětný popis (zobrazeno AI, určuje kdy AI vybere dovednost) |
| `parameter_schema` | JSON Schema parametrů, deklaruje každý zástupný symbol `{param}` použitý v promptu |
| `system_prompt_template` | Šablona systémového promptu, při spuštění se zástupné symboly vyplní parametry |
| `tool_whitelist` | Seznam nástrojů povolených během provádění (prázdné = dědí všechny nástroje bytosti) |
| `max_tool_round` / `timeout` | Omezení počtu kol nástrojů a časového limitu (omezováno globálními limity) |
| `on_complete` | Akce po dokončení: `none` / `write_memory` / `notify_curator` / `broadcast` |
| `trigger_mode` | `Manual` (autonomní volání AI) nebo `Auto` + plán `schedule` |

### Čtyři zdroje

- **Builtin** — vestavěné v rámci (`summarize_document` shrnutí dokumentu, `code_review` kontrola kódu, `research_topic` průzkum tématu)
- **Plugin** — pluginy registrované přes `ISkillProvider`
- **Being** — bytost vytváří za běhu pomocí nástroje `skill`
- **User** — uživatel vytváří přes stránku správy dovedností ve Web UI

### Způsoby spuštění

1. **Manuální (Manual)**: Dovednost je vložena do požadavku AI jako definice běžného nástroje, AI rozhoduje kdy ji zavolat; plánovač upřednostňuje směrování volání se stejným názvem do dovednosti
2. **Automatický (Auto + schedule)**: Plánovací výraz uložen v `metadata.schedule`, podporuje tři formáty:
   - `"09:30"` — denní pevný čas
   - `"6h"` / `"30 m"` / `"2 d"` — intervalové období
   - `"0 9 * * *"` / `"*/15 * * * *"` — podmnožina cron

### Psaní v Markdown

Dovednosti jsou ukládány v Markdown (`skills/{id}.md`, YAML front matter + tělo promptu):

```markdown
---
id: daily_news_digest
description: Vyhledat dnešní technologické zprávy a vygenerovat shrnutí
tool_whitelist: [network, work_note]
on_complete: write_memory
---

Použijte nástroj network k vyhledání nejnovějších zpráv o {topic}, vygenerujte 500znakové shrnutí a uložte jej do pracovních poznámek.
```

Lze napsat pouze tělo (vynechat YAML): při uložení AI automaticky doplní id, description, schéma parametrů a další metadata – pole již vyplněná uživatelem nikdy nejsou přepsána.

### Samospráva bytosti

Bytost může spravovat svou knihovnu dovedností prostřednictvím nástroje `skill`:

```json
{ "action": "list" }
{ "action": "create", "id": "my_skill", "system_prompt": "...", "description": "..." }
{ "action": "update_from_md", "skill_id": "my_skill", "markdown": "..." }
{ "action": "delete", "skill_id": "my_skill" }
```

### Hot reload a evoluce

- Bytost každých 30 sekund kontroluje změny v adresáři `skills/` (porovnáním otisku), úpravy z Web UI nebo jiných bytostí se automaticky projeví bez restartu
- Při každé aktualizaci dovednosti se automaticky archivuje historická verze do `skills/archive/{id}/{version}.md`, čímž vzniká historie evoluce dovedností
- Počet vlastních dovedností je omezen kvótou (`MaxCustomSkillsPerBeing`, výchozí 50)

### Zábrany provádění

- Oprávnění akce `execute` na úrovni dovednosti (může být zakázáno maticí oprávnění, při zákazu je pro AI neviditelné)
- Parametry provádění jsou omezovány globálními limity: počet kol ≤ `GlobalMaxToolRound` (výchozí 10), časový limit ≤ `GlobalSkillTimeoutSeconds` (výchozí 300 sekund)
- Dovednost nemůže rekurzivně volat sama sebe
- Volání nástrojů mimo whitelist přímo selže

## Systém paměti

### Typy paměti

- **Krátkodobá**: Kontext aktuální konverzace
- **Dlouhodobá**: Perzistentní znalosti a zkušenosti
- **Epizodická**: Časově indexované události a interakce

### Struktura úložiště

Verze Default:
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

Verze Fast (úložiště SpeedyPack):
```
data/
├── speedy/
│   ├── {being-id}.spk       # SpeedyPack soubor úložiště
│   └── {being-id}.spk.idx   # Indexový soubor
└── beings/
    └── {being-id}/
        └── soul.md
```

## Systém pracovních poznámek

### Přehled

Pracovní poznámky jsou osobní deníkový systém Křemíkové Bytosti, využívající stránkový design pro zaznamenávání pracovního postupu, poznatků z učení, projektových poznámek atd.

### Vlastnosti

- **Stránková správa**: Každá poznámka tvoří samostatnou stránku, přístupnou podle čísla stránky
- **Podpora Markdown**: Obsah podporuje formát Markdown (text, seznamy, tabulky, bloky kódu)
- **Index klíčových slov**: Podpora přidávání klíčových slov k poznámkám pro snadné vyhledávání
- **Funkce shrnutí**: Každá poznámka má krátké shrnutí pro rychlý přehled
- **Generování obsahu**: Lze vygenerovat přehled obsahu všech poznámek pro pochopení celkového kontextu
- **Časová razítka**: Automatické zaznamenávání času vytvoření a aktualizace
- **Výchozí soukromí**: Pouze bytost sama má přístup (Kurátor může spravovat)

### Scénáře použití

1. **Záznam pracovního postupu**
   ```
   Shrnutí: Dokončení modulu uživatelské autentizace
   Obsah: Implementace JWT token ověření, OAuth2 integrace, mechanismus obnovy tokenů
   Klíčová slova: autentizace,JWT,OAuth2
   ```

2. **Poznámky z učení**
   ```
   Shrnutí: Učení se osvědčeným postupům asynchronního programování v C#
   Obsah: Poznámky k použití async/await, scénáře použití ConfigureAwait...
   Klíčová slova: C#,asynchronní,osvědčené postupy
   ```

3. **Zápis ze schůzky**
   ```
   Shrnutí: Diskuse o produktových požadavcích
   Obsah: Diskutováno o nových funkčních požadavcích, určeno řešení implementace...
   Klíčová slova: produkt,požadavky,schůzka
   ```

### Použití prostřednictvím nástroje

Bytost může spravovat pracovní poznámky pomocí nástroje `work_note`:

```json
// Vytvoření poznámky
{
  "action": "create",
  "summary": "Dokončení modulu uživatelské autentizace",
  "content": "## Detaily implementace\n\n- Použití JWT token\n- Podpora OAuth2",
  "keywords": "autentizace,JWT,OAuth2"
}

// Čtení poznámky
{
  "action": "read",
  "page_number": 1
}

// Vyhledávání poznámek
{
  "action": "search",
  "keyword": "autentizace",
  "max_results": 10
}
```

### Správa přes Web UI

1. Přejděte na **Správa bytostí** → vyberte bytost
2. Klikněte na záložku **Pracovní poznámky**
3. Můžete prohlížet, vyhledávat a upravovat poznámky
4. Podpora náhledu Markdown

## Systém znalostní sítě

### Přehled

Znalostní síť je systém pro reprezentaci a správu znalostí založený na trojicové struktuře (subjekt-predikát-objekt), určený pro ukládání a správu strukturovaných znalostí.

### Základní koncepty

#### Trojicová struktura

```
Subjekt (Subject) --Predikát (Predicate)--> Objekt (Object)
```

**Příklady**:
- `Python` --`is_a`--> `programming_language`
- `Peking` --`capital_of`--> `Čína`
- `Voda` --`boiling_point`--> `100°C`

#### Skóre důvěry

Každá trojice znalostí má skóre důvěry (0.0-1.0), vyjadřující věrohodnost znalosti:
- `1.0`: Absolutní jistota (např. matematické věty)
- `0.8-0.99`: Vysoká důvěryhodnost (např. ověřená fakta)
- `0.5-0.79`: Střední důvěryhodnost (např. odvození nebo hypotézy)
- `<0.5`: Nízká důvěryhodnost (např. odhady nebo neověřené informace)

#### Systém štítků

Podpora přidávání štítků k trojicím pro snadnější kategorizaci a vyhledávání:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "tags": ["programming", "language", "popular"]
}
```

### Operace se znalostmi

#### 1. Přidání znalosti

```json
{
  "action": "add",
  "subject": "C#",
  "predicate": "created_by",
  "object": "Microsoft",
  "confidence": 1.0,
  "tags": ["programming", "language"]
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
  "query": "programming language",
  "limit": 10
}
```

#### 4. Objevení znalostní cesty

Nalezení asociační cesty mezi dvěma koncepty:
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

Výsledek:
```
Python → is_a → programming_language → belongs_to → computer_science
```

#### 5. Validace znalostí

Kontrola platnosti a konzistence znalostí:
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. Statistiky znalostí

Získání celkových statistických informací o znalostní síti:
```json
{
  "action": "stats"
}
```

Výsledek:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Scénáře použití

1. **Ukládání faktů**
   - Ukládání objektivních faktů a obecných znalostí
   - Příklad: `Země` --`is_a`--> `planeta`

2. **Vztahy konceptů**
   - Zaznamenávání vztahů mezi koncepty
   - Příklad: `Dědičnost` --`is_a`--> `koncept_objektově_orientovaného_programování`

3. **Akumulace učení**
   - Bytost neustále akumuluje znalosti prostřednictvím učení
   - Tvorba strukturovaného systému znalostí

4. **Podpora uvažování**
   - Objevování nepřímých vztahů prostřednictvím znalostních cest
   - Podpora uvažování a rozhodování na základě znalostí

### Správa přes Web UI

1. Přejděte na stránku **Znalostní síť**
2. Zobrazení statistických informací o znalostech
3. Vyhledávání a procházení znalostí
4. Vizualizace grafu znalostních vztahů (plánováno)

## Operace WebView prohlížeče (nové)

### Přehled

Křemíková Bytost může autonomně procházet webové stránky, získávat informace a provádět webové operace pomocí nástroje WebView prohlížeče. Prohlížeč běží v headless režimu, zcela neviditelný pro uživatele.

### Vlastnosti

- **Individuální izolace**: Každá bytost má nezávislou instanci prohlížeče, cookies a relaci
- **Headless režim**: Autonomní operace na pozadí, neviditelná pro uživatele
- **Kompletní funkce**: Podpora spouštění JavaScriptu, CSS renderování, vyplňování formulářů atd.
- **Bezpečnostní řízení**: Všechny operace musí projít řetězcem ověřování oprávnění

### Běžné operace

#### 1. Otevření prohlížeče

```json
{
  "action": "open"
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

Vrací textový obsah stránky pro analýzu a porozumění AI.

#### 4. Kliknutí na prvek

```json
{
  "action": "click",
  "selector": "#submit-button"
}
```

#### 5. Zadání textu

```json
{
  "action": "input",
  "selector": "#search-input",
  "text": "Vyhledávací klíčové slovo"
}
```

#### 6. Spuštění JavaScriptu

```json
{
  "action": "execute_script",
  "script": "return document.title;"
}
```

#### 7. Pořízení snímku obrazovky

```json
{
  "action": "get_screenshot"
}
```

Vrací snímek obrazovky stránky (kódování Base64), použitelný pro vizuální analýzu.

#### 8. Čekání na výskyt prvku

```json
{
  "action": "wait_for_element",
  "selector": ".loading-complete",
  "timeout": 10000
}
```

### Scénáře použití

1. **Získávání informací**
   - Procházení zpravodajských webů pro aktuální zprávy
   - Vyhledávání dokumentace a technických materiálů
   - Sledování změn obsahu webových stránek

2. **Automatizované operace**
   - Vyplňování a odesílání formulářů
   - Kliknutí na tlačítka pro spuštění operací
   - Stahování dat z webových stránek

3. **Analýza webových stránek**
   - Analýza struktury a obsahu stránky
   - Extrakce specifických informací
   - Vizuální analýza snímků obrazovky

### Upozornění

- Operace prohlížeče mohou být pomalejší, je nutné počkat na načtení stránky
- Použijte `wait_for_element` pro zajištění přítomnosti prvku před operací
- Dodržujte podmínky používání webů a robots.txt
- Vyhněte se častým požadavkům, které mohou vést k zablokování

## Osvědčené postupy

### Psaní Souboru Duše

1. **Konkrétnost**: Jasné osobnostní rysy a hranice
2. **Definice rozsahu**: Co bytost má a nemá dělat
3. **Zahrnutí příkladů**: Ukázka očekávaných vzorců chování
4. **Pravidelné aktualizace**: Evoluce duše na základě výkonu

### Správa úkolů

1. **Nastavení priority**: Použití priority (1-10)
2. **Definice termínů**: Vždy nastavte termín dokončení
3. **Sledování postupu**: Pravidelná kontrola stavu úkolů
4. **Zpracování selhání**: Implementace logiky opakování

### Optimalizace paměti

1. **Čištění starých dat**: Pravidelná archivace starých pamětí
2. **Indexování důležitých informací**: Označení klíčových informací
3. **Použití časového úložiště**: Využití časově indexovaných dotazů

### Mechanismus zapomínání paměti

Systém obsahuje vestavěnou službu časového útlumu `MemoryFadeService`, která simuluje vlastnost zapomínání biologické paměti:

- **Automatický útlum**: Každou hodinu aplikuje algoritmus útlumu důležitosti na položky paměti všech Křemíkových Bytostí
- **Automatická archivace**: Paměti s důležitostí pod prahem jsou automaticky archivovány a již se neúčastní denního vyhledávání
- **Sledování statistik**: Zaznamenává počet cyklů útlumu a počet změněných položek

To znamená, že paměť Křemíkové Bytosti se časem přirozeně vytrácí. Důležité informace je nutné pomocí nástroje paměti aktivně označit jako vysoce důležité, aby se zabránilo automatické archivaci.

---

## Projektový pracovní prostor

### Přehled

Projektový pracovní prostor je mechanismus správy prostoru podporující spolupráci více Křemíkových Bytostí. Kurátor Křemíku může vytvářet projektové prostory, přiřazovat Křemíkové Bytosti do projektů a přidělovat jim role.

### Životní cyklus projektu

```
Vytvoření → Aktivní → Archivace → Zničení
              ↑       |
              └─ Obnovení ┘
```

### Projektové role

Křemíkové Bytosti mohou být v projektu přiřazeny ke konkrétním rolím:

```json
{
  "action": "assign_role",
  "project_id": "project-uuid",
  "being_id": "being-uuid",
  "role_name": "developer"
}
```

### Projektové pracovní poznámky

Pracovní poznámky v projektovém prostoru jsou veřejné, všichni členové projektu k nim mají přístup:

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "Dokončení modulu uživatelské autentizace",
  "content": "## Detaily implementace\n\n- Použití JWT token",
  "keywords": "autentizace,JWT"
}
```

### Projektové úkoly

Úkoly v projektovém prostoru podporují kompletní správu životního cyklu:

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "title": "Implementace uživatelské autentizace",
  "priority": 5
}
```

### Projektové pracovní postupy

Projekty mohou být vázány na šablony pracovních postupů, které řídí procesy spolupráce Křemíkových Bytostí:

- Pracovní postupy jsou založeny na šablonách stavového stroje
- Podpora Tick-řízených přechodů stavů
- Automatické zaznamenávání protokolů přechodů stavů

### Izolace oprávnění nástrojů

Oprávnění nástrojů na úrovni projektu jsou nezávislá na oprávněních na úrovni Křemíkové Bytosti, čímž se dosahuje izolace oprávnění mezi projekty. Například Křemíková Bytost může mít v projektu A práva síťového přístupu, ale v projektu B může být omezena na práva pouze pro čtení.

## Řešení problémů

### Bytost nelze spustit

**Zkontrolujte**:
- Soubor Duše existuje a je platný
- AI klient je nakonfigurován
- Dostatek systémových prostředků

### Bytost se neočekávaně zastavila

**Zkontrolujte**:
- Chyby v protokolech
- Dostupnost AI služby
- Využití paměti

### Úkol nebyl proveden

**Zkontrolujte**:
- Systém časovačů běží
- Prioritu a plán úkolu
- Nastavení oprávnění

## Další kroky

- 📚 Přečtěte [příručku architektury](architecture.md)
- 🛠️ Prohlédněte [vývojářskou příručku](development-guide.md)
- 🚀 Prohlédněte [příručku rychlého startu](getting-started.md)
