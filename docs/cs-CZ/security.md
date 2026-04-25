# Bezpečnostní návrh

[English](../en/security.md) | [中文文档](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | [Español](../es-ES/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | [Čeština](../cs-CZ/security.md)

## Přehled

Bezpečnost Silicon Life Collective je postavena na modelu **vrstvené obrany**. Základní princip: **Všechny I/O operace musí procházet přes Executory**, které před provedením vynucují kontroly oprávnění.

```
Volání nástroje → Executor → Permission Manager → High Deny Cache → High Allow Cache → Callback → Dotaz uživatele
```

---

## Model oprávnění

### Typy oprávnění

| Typ | Popis |
|------|-------------|
| `NetworkAccess` | Odchozí HTTP/HTTPS požadavky |
| `CommandLine` | Provádění příkazů shellu |
| `FileAccess` | Operace se soubory a adresáři |
| `Function` | Volání citlivých funkcí |
| `DataAccess` | Přístup k systémovým nebo uživatelským datům |

### Výsledky oprávnění

Každá kontrola oprávnění vrací jeden ze tří výsledků:

| Výsledek | Chování |
|--------|----------|
| **Allowed (Povoleno)** | Operace okamžitě pokračuje |
| **Denied (Zamítnuto)** | Operace je blokována, zaznamenána v audit logu |
| **AskUser (Dotaz uživatele)** | Operace je pozastavena, vyžaduje potvrzení uživatele |

### Speciální role: Křemíkový Kurátor

Křemíkový Kurátor má nejvyšší úroveň oprávnění (`IsCurator = true`). Kontroly oprávnění pro Kurátora jsou zkratkovány na **Povoleno**, pokud uživatel výslovně nepřepíše.

### Soukromý Permission Manager

Každá křemíková bytost má svou vlastní instanci **soukromého Permission Manageru**. Stav oprávnění není sdílen mezi bytostmi.

---

## Proces ověřování oprávnění

Priorita dotazování: **1. User High Deny → 2. User High Allow → 3. Callback funkce**

```
┌─────────────┐
│ Volání       │
│ nástroje     │
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Executor    │────▶│ Soukromý            │
│ (Disk/Síť/   │     │ Permission           │
│  Příkaz...)  │     │ Manager (na bytost)  │
└─────────────┘     └────────┬────────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │ 1. IsCurator?   │──Ano──▶ Povoleno
                    └────────┬────────┘
                             │ Ne
                             ▼
                    ┌─────────────────┐
                    │ 2. User High    │──Shoda──▶ Zamítnuto
                    │ Deny (cache)    │
                    └────────┬────────┘
                             │ Žádná shoda
                             ▼
                    ┌─────────────────┐
                    │ 3. User High    │──Shoda──▶ Povoleno
                    │ Allow (cache)   │
                    └────────┬────────┘
                             │ Žádná shoda
                             ▼
                    ┌─────────────────┐
                    │ 4. Permission   │
                    │ Callback        │──▶ Povoleno / Zamítnuto / Dotaz uživatele
                    └─────────────────┘
```

**Klíčový bod**: Executor vidí pouze boolean (Povoleno/Zamítnuto). Permission Manager interně zpracovává trojrozměrné rozhodnutí (Povoleno/Zamítnuto/Dotaz uživatele) a vyřeší Dotaz uživatele před vrácením Executoru.

---

## Executory (Bezpečnostní hranice)

Executory jsou **jedinou** cestou pro I/O operace. Vynucují:

### Samostatné dispatch thready

Každý executor má svůj **vlastní dispatch thread**:

- Thread izolace mezi executory — blokování threadu jednoho executoru neovlivní ostatní executory.
- Každý executor může mít nezávislé limity zdrojů (CPU, paměť atd.).
- Správa thread poolu pro thready executorů.

### Fronta požadavků

Každý executor udržuje frontu požadavků:

- Požadavky jsou směrovány podle typu na odpovídající executor.
- Podpora prioritního řazení.
- Kontrola timeoutu pro každý požadavek.

### Thread lock pro ověřování oprávnění

Když nástroj iniciuje přístup ke zdrojům:

1. Executor přijme požadavek a **zamkne svůj thread**.
2. Executor dotazuje soukromý Permission Manager bytosti.
3. Pokud callback vrátí Dotaz uživatele, thread executoru **zůstane zamčený** a čeká na odpověď uživatele.
4. Bytost vidí pouze konečný výsledek (úspěch nebo zamítnutí) — nikdy nevidí přechodný stav "čekající" nebo "čekání".
5. Pouze Křemíkový Kurátor spouští skutečný dotaz uživatele. Běžné bytosti synchronně dotazují globální ACL bez blokování.
6. Při timeoutu je požadavek považován za zamítnutý a zámek threadu je uvolněn.

### Typy executorů

| Executor | Rozsah | Výchozí timeout |
|----------|-------|-----------------|
| `DiskExecutor` | Čtení/zápis souborů, operace s adresáři | 30 sekund |
| `NetworkExecutor` | HTTP požadavky, WebSocket připojení | 60 sekund |
| `CommandLineExecutor` | Provádění příkazů shellu | 120 sekund |
| `DynamicCompilationExecutor` | Roslyn kompilace v paměti | 60 sekund |

### Izolace výjimek a tolerance chyb

- Výjimka jednoho executoru neovlivní ostatní executory.
- Automatický restart při pádu threadu.
- Jistič: Dočasné pozastavení executoru po sérii selhání k prevenci kaskádových selhání.

---

## Globální ACL (Access Control List)

Tabulka sdílených pravidel persistovaná do storage, spravovaná pouze Křemíkovým Kurátorem:

```json
{
  "rules": [
    { "prefix": "network:api.github.com", "result": "Allowed" },
    { "prefix": "file:C:\\Windows", "result": "Denied" },
    { "prefix": "cli:rm -rf", "result": "Denied" }
  ]
}
```

- Pravidla jsou vyhodnocována v pořadí; první shoda vyhrává.
- Pouze Křemíkový Kurátor může upravovat globální ACL (prostřednictvím svého specializovaného nástroje).
- Změny se projeví okamžitě.
- Globální ACL **není** v prioritním řetězci pro každý dotaz výše — je interně odkazováno callback funkcí.

---

## User Frequency Cache

Pro snížení opakovaných dotazů na oprávnění systém udržuje dva **per-bytost, pouze v paměti** cache:

| Cache | Použití |
|-------|---------|
| **HighAllow** | Zdroje často povolované uživatelem |
| **HighDeny** | Zdroje často zamítané uživatelem |

### Jak to funguje

- **Výběr uživatele, ne automatická detekce**: Když je spuštěn Dotaz uživatele, uživatel zvolí, zda přidat zdroj do cache.
- **Prefix matching**: Podporuje shodu prefixu cesty zdroje (např. `network:api.example.com/*`).
- **Priorita**: HighDeny má vyšší prioritu než HighAllow.
- **Pouze paměť**: Cache nejsou persistovány. Ztrácejí se při restartu.
- **Konfigurovatelná expirace**: Uživatel může nastavit dobu platnosti položek cache.

### Proces aktualizace cache

1. Permission callback vrátí `AskUser`.
2. Permission systém odešle dotaz na Card systém (Web UI nebo IM).
3. Uživatel učiní rozhodnutí (Povolit/Zamítnout) a **zvolí zda cachovat**.
4. Card systém vrátí rozhodnutí + flag cache.
5. Permission systém aktualizuje odpovídající seznam cache.
6. Budoucí požadavky odpovídající prefixu cache jsou vyřešeny okamžitě.

---

## Mechanismus Dotazu uživatele

Když kontrola oprávnění vrátí `AskUser`:

### Web UI: Interaktivní karty

Webový frontend okamžitě zobrazí **interaktivní kartu** zobrazující:

- Typ a cestu zdroje
- Popis akce
- Tlačítka Povolit / Zamítnout
- Volitelné checkboxy "Vždy povolit" / "Vždy zamítnout" (přidat do frequency cache)

### IM (bez podpory karet): Náhodné kódy

Pro messaging platformy nepodporující interaktivní karty:

1. Systém vygeneruje dva náhodné 6místné kódy: **Allow kód** a **Deny kód**.
2. Odešle zprávu s informacemi o zdroji a oběma kódy.
3. Uživatel musí odpovědět přesným Allow kódem pro autorizaci. Jakákoliv jiná odpověď je považována za zamítnutí.
4. Kódy jsou jednorázové k prevenci replay útoků.

### Timeout

- Všechny požadavky Dotazu uživatele mají nastavený timeout.
- Při timeoutu je požadavek považován za **Zamítnuto** a zámek threadu Executoru je uvolněn.

---

## Bezpečnost dynamické kompilace

Sebe-evoluce (přepis tříd) představuje jedinečná bezpečnostní rizika. Systém je mitiguje pomocí **vrstvené strategie**:

### Vrstva 1: Kontrola referencí při kompilaci (primární obrana)

- Kompilátor získá pouze **seznam povolených assembly referencí**.
- **Povoleno**: `System.Runtime`, `System.Private.CoreLib`, projektové assembly (ITool rozhraní atd.)
- **Blokováno**: `System.IO`, `System.Reflection`, `System.Runtime.InteropServices` atd.
- Pokud kód odkazuje na blokovanou assembly, **kompilátor sám kód odmítne**.
- To je spolehlivější než runtime scanning — nebezpečné operace jsou nemožné na úrovni typů.

### Vrstva 2: Statická analýza za běhu (sekundární obrana)

- I po úspěšné kompilaci je kód skenován na statické vzory.
- Detekuje vzory nebezpečných operací (přímé I/O, systémová volání atd.).
- Pokud je nalezen nebezpečný kód, načtení je zamítnuto a systém se vrátí k výchozí funkčnosti.

### Omezení dědičnosti

Všechny vlastní třídy křemíkových bytostí **musí** dědit `SiliconBeingBase`. Kompilator toto omezení vynucuje na úrovni typů.

### Šifrované úložiště

Kompilovaný kód je uložen na disku šifrovaný AES-256:

- **Odvození klíče**: Z GUID bytosti (velká písmena) pomocí PBKDF2.
- **Selhání dešifrování**: Návrat k výchozí implementaci.
- **Rekompilace za běhu**: Nový kód je nejprve kompilován v paměti; persistován pouze po úspěšné kompilaci a výměně instance.

### Atomická výměna

Proces výměny je atomický:

1. Kompilace nového kódu v paměti → získání `Type`.
2. Vytvoření nové instance z `Type`.
3. Migrace stavu ze staré instance na novou.
4. Prohození referencí.
5. Persistování šifrovaného kódu.

Pokud jakýkoliv krok selže, stará instance zůstane aktivní.

---

## Permission Callback Funkce

### Návrh

Každý Permission Manager drží **proměnnou callback funkce**:

- **Výchozí**: Ukazuje na vestavěnou výchozí permission funkci.
- **Po dynamické kompilaci**: Přepsáno vlastní permission funkcí bytosti.
- **Buď-anebo**: V kterémkoliv čase je aktivní pouze jeden callback.
- **Selhání kompilace**: Neovlivní aktuální callback — výchozí nebo poslední úspěšná vlastní funkce zůstane platná.

### Signatura callbacku

```
PermissionResult Callback(PermissionType type, string resourcePath, Guid callerId)
```

Vrací `Allowed`, `Denied` nebo `AskUser`.

---

## Audit Log

Všechna rozhodnutí o oprávněních jsou zaznamenána:

```
[2026-04-01 15:30:25] ALLOWED  | Being:AssistantA | Type:NetworkAccess | Resource:api.github.com | Source:HighAllowCache
[2026-04-01 15:30:26] DENIED   | Being:AssistantB | Type:FileAccess    | Resource:C:\Windows\System32 | Source:HighDenyCache
[2026-04-01 15:30:27] ASK_USER | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:Callback
[2026-04-01 15:30:28] ALLOWED  | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:UserDecision
```

Logy jsou persistovány do storage a lze je zobrazit prostřednictvím Web UI (Log Controller).

---

## Audit použití Tokenů

`TokenUsageAuditManager` poskytuje sledování spotřeby AI tokenů související s bezpečností:

- **Záznam na požadavek** — Každé volání AI zaznamenává ID bytosti, model, prompt tokeny, completion tokeny a časové razítko.
- **Detekce anomálií** — Neobvyklé vzory spotřeby tokenů mohou indikovat prompt injection nebo zneužití zdrojů.
- **Přístup pouze pro Kurátora** — `TokenAuditTool` (označen `[SiliconManagerOnly]`) umožňuje Kurátorovi dotazovat a sumarizovat použití tokenů.
- **Web Dashboard** — `AuditController` poskytuje dashboard založený na prohlížeči s grafy trendů a exportem dat.
- **Persistované úložiště** — Záznamy jsou uloženy prostřednictvím `ITimeStorage` pro dotazy časových řad a dlouhodobou analýzu.
