# Bezpečnostní design

> **Verze: v0.2.0-alpha**

[English](../en/security.md) | [Deutsch](../de-DE/security.md) | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | [Español](../es-ES/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | **Čeština** | [Русский](../ru-RU/security.md)

## Přehled

Bezpečnost Silicon Life Collective je postavena na modelu **vrstvené obrany**. Základní princip: **všechny I/O operace musí projít exekutorem**, který vynucuje kontrolu oprávnění před provedením.

```
Volání nástroje → Exekutor → Správce Oprávnění → Frekvenční mezipaměť → Zpětné volání → (IsCurator: Dotaz na uživatele | Non-curator: Globální ACL)
```

---

## Model oprávnění

### Typy oprávnění

| Typ | Popis |
|------|-------------|
| `NetworkAccess` | Odchozí HTTP/HTTPS požadavky |
| `CommandLine` | Spouštění shell příkazů |
| `FileAccess` | Operace se soubory a adresáři |
| `Function` | Volání citlivých funkcí |
| `DataAccess` | Přístup k systémovým nebo uživatelským datům |

### Výsledky oprávnění

Každá kontrola oprávnění vrací jeden ze tří výsledků:

| Výsledek | Chování |
|--------|----------|
| **Allowed (Povoleno)** | Operace okamžitě proběhne |
| **Denied (Zamítnuto)** | Operace je zablokována, zaznamenáno v auditním protokolu |
| **AskUser (Dotaz na uživatele)** | Operace je pozastavena, vyžaduje potvrzení uživatelem |

### Speciální role: Kurátor Křemíku

Kurátor Křemíku má nejvyšší úroveň oprávnění (`IsCurator = true`). Když řetězec oprávnění dosáhne rozhodovací větve, operace Kurátora jsou odeslány uživateli k potvrzení prostřednictvím `IPermissionAskHandler`, nikoli zkratovány na povoleno. Ne-Kurátoři se dotazují na globální ACL.

### Privátní Správce Oprávnění

Každá Křemíková Bytost má svou vlastní **privátní instanci PermissionManager**. Stav oprávnění není sdílen mezi bytostmi.

---

## Průběh ověřování oprávnění

Priorita dotazů: **1. Frekvenční mezipaměť → 2. Funkce zpětného volání → 3. Rozhodovací větev (IsCurator/GlobalACL)**

```
┌─────────────┐
│ Volání      │
│ nástroje    │
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Exekutor   │────▶│ Privátní Správce    │
│ (disk/síť/  │     │ Oprávnění (každá    │
│  příkazový  │     │ bytost)             │
│  řádek...)  │     └────────┬────────────┘
└─────────────┘            │
                           ▼
                  ┌─────────────────┐
                  │ 1. Frekvenční   │──Shoda──▶ Povoleno / Zamítnuto
                  │ mezipaměť       │
                  │ (HighDeny má    │
                  │ přednost před   │
                  │ HighAllow)      │
                  └────────┬────────┘
                           │ Neshoda
                           ▼
                  ┌─────────────────┐
                  │ 2. Zpětné       │
                  │ volání          │──▶ Povoleno / Zamítnuto / Dotaz na uživatele
                  │ oprávnění       │
                  └────────┬────────┘
                           │ Dotaz na uživatele
                           ▼
                  ┌─────────────────┐
                  │ 3. IsCurator?   │
                  └────────┬────────┘
                           │
                 ┌─────────┴─────────┐
                 │                   │
                 ▼ Ano               ▼ Ne
          ┌─────────────┐    ┌─────────────┐
          │ Dotaz na     │    │ Globální ACL │
          │ uživatele    │    │ Dotaz na     │
          │ (AskHandler) │    │ pravidla     │
          └─────────────┘    └─────────────┘
```

**Klíčový bod**: Exekutor vidí pouze booleovskou hodnotu (povoleno/zamítnuto). Správce Oprávnění interně zpracovává trojstavové rozhodnutí (povoleno/zamítnuto/dotaz na uživatele) a řeší dotaz na uživatele před vrácením výsledku exekutoru.

---

## Exekutoři (bezpečnostní hranice)

Exekutoři jsou **jedinou** cestou pro I/O operace. Vynucují:

### Nezávislé dispečerské vlákno

Každý exekutor má **nezávislé dispečerské vlákno**:

- Izolace vláken mezi exekutory — zablokování vlákna jednoho exekutoru neovlivňuje ostatní exekutory.
- Každý exekutor může mít nastavena nezávislá omezení prostředků (CPU, paměť atd.).
- Správa vláknového poolu exekutoru.

### Fronta požadavků

Každý exekutor udržuje frontu požadavků:

- Požadavky jsou směrovány podle typu do příslušného exekutoru.
- Podpora prioritního řazení.
- Řízení timeoutu pro každý požadavek.

### Zamykání vláken pro ověřování oprávnění

Když nástroj iniciová přístup k prostředkům:

1. Exekutor přijme požadavek a **uzamkne své vlákno**.
2. Exekutor se dotazuje privátního Správce Oprávnění bytosti.
3. Pokud zpětné volání vrátí dotaz na uživatele, vlákno exekutoru **zůstává uzamčeno** a čeká na odpověď uživatele.
4. Bytost vidí pouze konečný výsledek (úspěch nebo zamítnutí) — nikdy nevidí přechodný stav "čekající" nebo "čeká na".
5. Pouze Kurátor Křemíku spustí skutečný dotaz na uživatele. Běžné bytosti se synchronně dotazují na globální ACL bez blokování.
6. Při timeoutu je požadavek považován za zamítnutý a zámek vlákna je uvolněn.

### Typy exekutorů

| Exekutor | Rozsah | Výchozí timeout |
|----------|--------|-----------------|
| `DiskExecutor` | Čtení/zápis souborů, operace s adresáři | 30 sekund |
| `NetworkExecutor` | HTTP požadavky, WebSocket připojení | 60 sekund |
| `CommandLineExecutor` | Spouštění shell příkazů | 120 sekund |

> **Poznámka**: `DynamicCompilationExecutor` (v jmenném prostoru `SiliconLife.Core.Compilation`) je zodpovědný za Roslyn kompilaci v paměti, nepatří do kategorie I/O exekutorů, ale je rovněž podřízen systému oprávnění.

### Izolace výjimek a odolnost proti chybám

- Výjimka jednoho exekutoru neovlivňuje ostatní exekutory.
- Automatický restart při pádu vlákna.
- Jistič: po sobě jdoucí selhání dočasně zastaví exekutor pro zabránění kaskádovým selháním.

---

## Globální ACL (seznam řízení přístupu)

Sdílená tabulka pravidel perzistentní v úložišti, spravovaná pouze Kurátorem Křemíku:

```json
{
  "rules": [
    { "prefix": "network:api.github.com", "result": "Allowed" },
    { "prefix": "file:C:\\Windows", "result": "Denied" },
    { "prefix": "cli:rm -rf", "result": "Denied" }
  ]
}
```

- Pravidla jsou vyhodnocována v pořadí; první shoda platí.
- Pouze Kurátor Křemíku může upravovat globální ACL (prostřednictvím svého dedikovaného nástroje).
- Změny nabývají účinnosti okamžitě.
- Globální ACL **není** ve výše uvedeném řetězci priorit dotazů — je interně odkazováno funkcí zpětného volání.

---

## Uživatelská frekvenční mezipaměť

Pro snížení opakovaných dotazů na oprávnění systém udržuje dvě **per-bytost, pouze v paměti** mezipaměti:

| Mezipaměť | Účel |
|-------|---------|
| **HighAllow (Vysoké povolení)** | Prostředky často povolované uživatelem |
| **HighDeny (Vysoké zamítnutí)** | Prostředky často zamítané uživatelem |

### Jak to funguje

- **Výběr uživatelem, nikoli automatická detekce**: Když je spuštěn dotaz na uživatele, uživatel si vybere, zda přidat prostředek do mezipaměti.
- **Shoda předpony**: Podpora shody předpony cesty prostředku (např. `network:api.example.com/*`).
- **Priorita**: HighDeny má přednost před HighAllow.
- **Pouze v paměti**: Mezipaměť není perzistentní. Při restartu je ztracena.
- **Konfigurovatelná expirace**: Uživatel může nastavit dobu platnosti položek mezipaměti.

### Průběh aktualizace mezipaměti

1. Zpětné volání oprávnění vrátí `AskUser`.
2. Systém oprávnění odešle dotaz do karetního systému (Web UI nebo IM).
3. Uživatel učiní rozhodnutí (povolit/zamítnout) a **vybere, zda uložit do mezipaměti**.
4. Karetní systém vrátí rozhodnutí + příznak mezipaměti.
5. Systém oprávnění aktualizuje příslušný seznam mezipaměti.
6. Budoucí požadavky odpovídající předponě mezipaměti jsou okamžitě vyřešeny.

---

## Mechanismus dotazování uživatele

Když kontrola oprávnění vrátí `AskUser`:

### Web UI: Interaktivní karta

Web frontend okamžitě zobrazí **interaktivní kartu** s:

- Typem a cestou prostředku
- Popisem operace
- Tlačítky Povolit / Zamítnout
- Volitelným zaškrtávacím políčkem "Vždy povolit" / "Vždy zamítnout" (přidání do frekvenční mezipaměti)

### Okamžité zprávy (bez podpory karet): Náhodný kód

Pro platformy okamžitých zpráv nepodporující interaktivní karty:

1. Systém vygeneruje dva náhodné 6místné kódy: **povolovací kód** a **zamítavací kód**.
2. Odešle zprávu s informacemi o prostředku a oběma kódy.
3. Uživatel musí odpovědět přesným povolovacím kódem pro autorizaci. Jakákoliv jiná odpověď je považována za zamítnutí.
4. Kódy jsou jednorázové pro zabránění útokům opakováním.

### Timeout

- Pro všechny dotazy na uživatele je nastaven timeout.
- Při timeoutu je požadavek považován za **zamítnutý** a zámek vlákna exekutoru je uvolněn.

---

## Bezpečnost dynamické kompilace

Sebeevoluce (přepsání třídy) přináší unikátní bezpečnostní rizika. Systém je zmírňuje pomocí **vrstvené strategie**:

### Vrstva 1: Kontrola referencí při kompilaci (primární obrana)

- Kompilátor získá pouze **povolený seznam referencí na sestavení**.
- **Povoleno**: `System.Runtime`, `System.Private.CoreLib`, projektová sestavení (rozhraní ITool atd.)
- **Zakázáno**: `System.IO`, `System.Reflection`, `System.Runtime.InteropServices` atd.
- Pokud kód odkazuje na zakázané sestavení, **kompilátor samotný kód odmítne**.
- To je spolehlivější než runtime skenování — nebezpečné operace jsou nemožné na úrovni typů.

### Vrstva 2: Statická analýza za běhu (sekundární obrana)

- I po úspěšné kompilaci je kód podroben statickému skenování vzorů.
- Detekce nebezpečných vzorů operací (přímé I/O, systémová volání atd.).
- Pokud je nalezen nebezpečný kód, načtení je zamítnuto a systém se vrátí k výchozí implementaci.

### Omezení dědičnosti

Všechny vlastní třídy Křemíkových Bytostí **musí** dědit z `SiliconBeingBase`. Kompilátor vynucuje toto omezení na úrovni typů.

### Šifrované úložiště

Kompilovaný kód je na disku uložen šifrovaně pomocí AES-256:

- **Odvození klíče**: Z GUID bytosti (velkými písmeny) pomocí PBKDF2.
- **Selhání dešifrování**: Návrat k výchozí implementaci.
- **Runtime rekompilace**: Nový kód je nejprve kompilován v paměti; perzistence šifrovaného kódu probíhá až po úspěšné kompilaci a výměně instance.

### Atomická výměna

Proces výměny je atomický:

1. Kompilace nového kódu v paměti → získání `Type`.
2. Vytvoření nové instance z `Type`.
3. Migrace stavu ze staré instance do nové.
4. Výměna reference.
5. Perzistence šifrovaného kódu.

Pokud kterýkoliv krok selže, stará instance zůstává aktivní.

---

## Funkce zpětného volání oprávnění

### Design

Každý PermissionManager drží **proměnnou funkce zpětného volání**:

- **Výchozí**: Odkazuje na vestavěnou výchozí funkci oprávnění.
- **Po dynamické kompilaci**: Přepsána vlastní funkcí oprávnění bytosti.
- **Výběr jednoho ze dvou**: Kdykoliv je aktivní pouze jedno zpětné volání.
- **Selhání kompilace**: Neovlivňuje aktuální zpětné volání — výchozí nebo poslední úspěšná vlastní funkce zůstává v platnosti.

### Signatura zpětného volání

```
PermissionResult Callback(PermissionType type, string resourcePath, Guid callerId)
```

Vrací `Allowed`, `Denied` nebo `AskUser`.

---

## Auditní protokol

Všechna rozhodnutí o oprávněních jsou zaznamenávána:

```
[2026-04-01 15:30:25] ALLOWED  | Being:AssistantA | Type:NetworkAccess | Resource:api.github.com | Source:HighAllowCache
[2026-04-01 15:30:26] DENIED   | Being:AssistantB | Type:FileAccess    | Resource:C:\Windows\System32 | Source:HighDenyCache
[2026-04-01 15:30:27] ASK_USER | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:Callback
[2026-04-01 15:30:28] ALLOWED  | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:UserDecision
```

Protokoly jsou perzistentní v úložišti a prohlížitelné přes Web UI (kontroler protokolů).

---

## Audit využití Tokenů

`TokenUsageAuditManager` poskytuje sledování spotřeby AI tokenů související s bezpečností:

- **Záznam pro každý požadavek** — každé volání AI zaznamenává ID bytosti, model, tokeny výzvy, tokeny doplnění a časové razítko.
- **Detekce anomálií** — abnormální vzorce spotřeby tokenů mohou indikovat injektáž výzev nebo zneužití prostředků.
- **Přístup pouze pro Kurátora** — `TokenAuditTool` (označený `[SiliconManagerOnly]`) umožňuje Kurátorovi dotazovat se a shrnovat využití tokenů.
- **Webový dashboard** — `UsageController` poskytuje webový dashboard s grafy trendů a exportem dat.
- **Perzistentní úložiště** — záznamy jsou uloženy prostřednictvím `ITimeStorage` pro časové řady dotazy a dlouhodobou analýzu.

---

## Bezpečnost zásuvných modulů

Systém zásuvných modulů přináší bezpečnostní rizika spojená s prováděním kódu třetích stran, která jsou zmírňována následujícími mechanismy:

### Bezpečnostní Sandbox a deklarace schopností

`PluginLoader` provádí při načítání zásuvných modulů bezpečnostní kontroly a současně podporuje mechanismus deklarace schopností:

1. **Deklarovatelné schopnosti** — zásuvné moduly deklarují požadované schopnosti pomocí atributu `[PluginCapability]`:
   - `Network` — síťový přístup (umožňuje reference na `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`)
   - `FileIO` — čtení/zápis souborů (umožňuje reference na `System.IO`)
   - `Process` — správa procesů
   - `AI` — volání AI

2. **Nedeklarovatelné schopnosti** — následující schopnosti jsou vždy blokovány:
   - P/Invoke (`System.Runtime.InteropServices`)
   - Unsafe kód (`System.Runtime.CompilerServices.Unsafe`)
   - Reflection Emit (`System.Reflection.Emit`)
   - API kompilátoru (`Microsoft.CodeAnalysis`)

3. **Whitelist důvěryhodných sestavení** — reference na následující sestavení jsou povoleny:
   - `Google.Protobuf`, `Newtonsoft.Json`, `MessagePack`
   - `Serilog`, `Microsoft.Extensions.Logging.Abstractions`
   - `Dapper`

4. **Kontrola zakázaných typů** — skenování nebezpečných typů odkazovaných v zásuvném modulu

5. **Kontrola zakázaných členů** — skenování nebezpečných metod volaných v zásuvném modulu

### Izolované načítání

- Použití vlastního `AssemblyLoadContext` pro izolované načítání každého zásuvného modulu
- Typy a sestavení mezi zásuvnými moduly se navzájem neovlivňují
- Při uvolnění zásuvného modulu lze uvolnit související prostředky

### Omezení oprávnění nástrojů

- Nástroje registrované zásuvnými moduly prostřednictvím rozhraní `ITool` podléhají stejnému systému oprávnění
- Nástroje zásuvných modulů nemohou obejít řetězec ověřování oprávnění
- Nástroje zásuvných modulů podléhají omezení atributu `[SiliconManagerOnly]`

---

## Bezpečnost oprávnění nástrojů

Systém oprávnění nástrojů poskytuje další bezpečnostní vrstvu, která řídí, které operace nástrojů mohou Křemíkové Bytosti používat:

### Dvouúrovňová izolace oprávnění

1. **Úroveň Křemíkové Bytosti** — každá Křemíková Bytost má nezávislou konfiguraci oprávnění nástrojů
2. **Úroveň projektu** — oprávnění nástrojů v projektovém prostoru jsou nezávislá na úrovni Křemíkové Bytosti, čímž se dosahuje izolace oprávnění mezi projekty

### Šablony oprávnění

Systém poskytuje předdefinované šablony oprávnění pro zajištění bezpečnostní základny:

- **readonly** — minimální oprávnění, pouze operace čtení
- **restricted** — omezená oprávnění, pouze základní operace
- **full** — úplná oprávnění (pouze pro Kurátora)

### Bezpečnostní vlastnosti

- **Výchozí zamítnutí** — nástrojové operace, které nejsou výslovně povoleny, jsou ve výchozím stavu zamítnuty
- **Granularita operací** — každá operace každého nástroje je řízena nezávisle (např. `network:get` povoleno, ale `network:post` zamítnuto)
- **Správa Kurátorem** — oprávnění nástrojů může konfigurovat pouze Kurátor Křemíku
- **Auditní stopa** — změny oprávnění nástrojů jsou zaznamenávány v auditním protokolu
