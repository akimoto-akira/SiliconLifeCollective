# Bezpečnostní Návrh

> **Verze: v0.1.0-alpha**

[English](../en/security.md) | [Deutsch](../de-DE/security.md) | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | [Español](../es-ES/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | **Čeština**

## Přehled

Bezpečnost Silicon Life Collective je postavena na modelu **vrstvené obrany**. Core princip: **Všechny I/O operace musí procházet přes exekutory**, které vynucují kontroly oprávnění před provedením.

```
Volání nástroje → Exekutor → Správce oprávnění → Vysoké zamítnutí → Vysoké povolení → Callback → Dotaz uživatele
```

---

## Model Oprávnění

### Typy Oprávnění

| Typ | Popis |
|------|-------------|
| `NetworkAccess` | Odchozí HTTP/HTTPS požadavky |
| `CommandLine` | Provádění shell příkazů |
| `FileAccess` | Operace se soubory a adresáři |
| `Function` | Volání citlivých funkcí |
| `DataAccess` | Přístup k systémovým nebo uživatelským datům |

### Výsledky Oprávnění

Každá kontrola oprávnění vrací jeden ze tří výsledků:

| Výsledek | Chování |
|--------|----------|
| **Allowed (Povoleno)** | Operace okamžitě pokračuje |
| **Denied (Zamítnuto)** | Operace je blokována, zaznamenána do auditního logu |
| **AskUser (Dotaz uživatele)** | Operace pozastavena, vyžaduje potvrzení uživatele |

### Speciální Role: Silikonový Kurátor

Silikonový kurátor má nejvyšší úroveň oprávnění (`IsCurator = true`). Kontroly oprávnění pro kurátora jsou zkratkovány na **Povoleno**, pokud uživatel explicitně nepřepíše.

### Soukromý Správce Oprávnění

Každá silikonová bytost má svou vlastní **soukromou instanci PermissionManager**. Stav oprávnění není sdílen mezi bytostmi.

---

## Proces Ověřování Oprávnění

Priorita dotazu je: **1. Uživatelské vysoké zamítnutí → 2. Uživatelské vysoké povolení → 3. Callback funkce**

```
┌─────────────┐
│ Volání       │
│ nástroje     │
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Exekutor    │────▶│ Soukromý správce     │
│ (Disk/Síť/  │     │ oprávnění            │
│  Příkaz...) │     │ (každá bytost)       │
└─────────────┘     └────────┬────────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │ 1. IsCurator?   │──Ano──▶ Povoleno
                    └────────┬────────┘
                             │ Ne
                             ▼
                    ┌─────────────────┐
                    │ 2. Vysoké       │──Shoda──▶ Zamítnuto
                    │ zamítnutí       │
                    │ (Memory cache)  │
                    └────────┬────────┘
                             │ Žádná shoda
                             ▼
                    ┌─────────────────┐
                    │ 3. Vysoké       │──Shoda──▶ Povoleno
                    │ povolení        │
                    │ (Memory cache)  │
                    └────────┬────────┘
                             │ Žádná shoda
                             ▼
                    ┌─────────────────┐
                    │ 4. Oprávnění    │
                    │ callback        │──▶ Povoleno / Zamítnuto / Dotaz uživatele
                    └─────────────────┘
```

**Klíčový bod**: Exekutor vidí pouze boolean (Povoleno/Zamítnuto). Správce oprávnění interně zpracovává tříhodnotové rozhodnutí (Povoleno/Zamítnuto/Dotaz uživatele) a řeší Dotaz uživatele před vrácením exekutoru.

---

## Exekutory (Bezpečnostní Hranice)

Exekutory jsou **jedinou** cestou pro I/O operace. Vynucují:

### Nezávislá Plánovací Vlákna

Každý exekutor má své vlastní **nezávislé plánovací vlákno**:

- Izolace vláken mezi exekutory — blokování vlákna jednoho exekutoru neovlivní ostatní.
- Každý exekutor může nastavit nezávislé limity zdrojů (CPU, paměť atd.).
- Správa fondu vláken pro vlákna exekutorů.

### Fronta Požadavků

Každý exekutor udržuje frontu požadavků:

- Požadavky jsou směrovány podle typu na příslušný exekutor.
- Podpora prioritního řazení.
- Kontrola časového limitu pro každý požadavek.

### Zámek Vlákna pro Ověřování Oprávnění

Když nástroj iniciová přístup ke zdroji:

1. Exekutor přijme požadavek a **zamkne své vlákno**.
2. Exekutor dotazuje soukromého správce oprávnění bytosti.
3. Pokud callback vrátí Dotaz uživatele, vlákno exekutoru **zůstane zamčené** čekající na odpověď uživatele.
4. Bytost vidí pouze konečný výsledek (Úspěch nebo Zamítnuto) — nikdy nevidí přechodný stav "Čeká" nebo "Pending".
5. Pouze silikonový kurátor spouští skutečný uživatelský prompt. Běžné bytosti synchronně dotazují globální ACL bez blokování.
6. Při časovém limitu je požadavek považován za zamítnutý a zámek vlákna je uvolněn.

### Typy Exekutorů

| Exekutor | Rozsah | Výchozí časový limit |
|----------|-------|---------------------|
| `DiskExecutor` | Čtení/zápis souborů, operace s adresáři | 30 sekund |
| `NetworkExecutor` | HTTP požadavky, WebSocket připojení | 60 sekund |
| `CommandLineExecutor` | Provádění shell příkazů | 120 sekund |
| `DynamicCompilationExecutor` | Paměťová kompilace Roslyn | 60 sekund |

### Izolace Výjimek a Tolerance Chyb

- Výjimka jednoho exekutoru neovlivní ostatní exekutory.
- Automatický restart při pádu vlákna.
- Jistič (Circuit Breaker): Dočasné zastavení exekutoru po opakovaných selháních k prevenci kaskádových selhání.

---

## Globální ACL (Seznam Řízení Přístupu)

Sdílená tabulka pravidel perzistentní do úložiště, spravovaná pouze silikonovým kurátorem:

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
- Pouze silikonový kurátor může upravovat globální ACL (prostřednictvím svého specializovaného nástroje).
- Změny jsou okamžitě platné.
- Globální ACL **není** v výše uvedeném prioritním řetězci pro každý dotaz — interně je odkazováno callback funkcí.

---

## Uživatelská Frekvenční Cache

Pro snížení opakovaných promptů oprávnění systém udržuje dvě **cache pouze v paměti, pro každou bytost**:

| Cache | Použití |
|-------|---------|
| **HighAllow (Vysoké povolení)** | Zdroje často povolené uživatelem |
| **HighDeny (Vysoké zamítnutí)** | Zdroje často zamítnuté uživatelem |

### Jak to Funguje

- **Uživatelská volba, ne automatická detekce**: Když je spuštěn Dotaz uživatele, uživatel si zvolí, zda přidat zdroj do cache.
- **Shoda prefixu**: Podporuje shodu prefixu cesty zdroje (např. `network:api.example.com/*`).
- **Priorita**: HighDeny má vyšší prioritu než HighAllow.
- **Pouze paměť**: Cache nejsou perzistentní. Ztrácí se při restartu.
- **Konfigurovatelná expirace**: Uživatel může nastavit dobu platnosti položek cache.

### Proces Aktualizace Cache

1. Oprávnění callback vrátí `AskUser`.
2. Systém oprávnění odešle dotaz do systému karet (Web UI nebo IM).
3. Uživatel provede rozhodnutí (Povolit/Zamítnout) a **zvolí zda cacheovat**.
4. Systém karet vrátí rozhodnutí + příznak cache.
5. Systém oprávnění aktualizuje příslušný seznam cache.
6. Budoucí požadavky odpovídající prefixu cache jsou okamžitě vyřešeny.

---

## Mechanismus Dotazování Uživatele

Když kontrola oprávnění vrátí `AskUser`:

### Web UI: Interaktivní Karty

Webový frontend okamžitě zobrazí **interaktivní kartu** zobrazující:

- Typ a cestu zdroje
- Popis operace
- Tlačítka Povolit / Zamítnout
- Volitelný checkbox "Vždy povolit" / "Vždy zamítnout" (přidat do frekvenční cache)

### IM (Bez Podpory Karet): Náhodný Kód

Pro zasílací platformy bez podpory interaktivních karet:

1. Systém generuje dva náhodné 6místné kódy: **kód povolení** a **kód zamítnutí**.
2. Odešle zprávu obsahující informace o zdroji a oba kódy.
3. Uživatel musí odpovědět přesným kódem povolení pro autorizaci. Jakákoli jiná odpověď je považována za zamítnutí.
4. Kódy jsou jednorázové pro prevenci útoků opakováním.

### Časový Limit

- Pro všechny požadavky Dotaz uživatele je nastaven časový limit.
- Při vypršení časového limitu je požadavek považován za **zamítnutý** a zámek vlákna exekutoru je uvolněn.

---

## Auditní Logy

Všechna rozhodnutí o oprávněních jsou zaznamenávána:

```
[2026-04-01 15:30:25] ALLOWED  | Being:AssistantA | Type:NetworkAccess | Resource:api.github.com | Source:HighAllowCache
[2026-04-01 15:30:26] DENIED   | Being:AssistantB | Type:FileAccess    | Resource:C:\Windows\System32 | Source:HighDenyCache
[2026-04-01 15:30:27] ASK_USER | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:Callback
[2026-04-01 15:30:28] ALLOWED  | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:UserDecision
```

Logy jsou perzistentní do úložiště, prohlížitelné prostřednictvím Web UI (kontrolér logů).

---

## Audit Použití Tokenů

`TokenUsageAuditManager` poskytuje sledování spotřeby AI tokenů související s bezpečností:

- **Záznam každého požadavku** — Každé volání AI zaznamenává ID bytosti, model, prompt tokeny, doplňovací tokeny a časové razítko.
- **Detekce anomálií** — Neobvyklé vzory spotřeby tokenů mohou naznačovat prompt injekci nebo zneužití zdrojů.
- **Přístup pouze pro kurátora** — `TokenAuditTool` (označený `[SiliconManagerOnly]`) umožňuje kurátorovi dotazovat se a shrnovat využití tokenů.
- **Webový dashboard** — `AuditController` poskytuje dashboard založený na prohlížeči s trendy a exportem dat.
- **Perzistentní úložiště** — Záznamy jsou uloženy prostřednictvím `ITimeStorage` pro časové řady dotazů a dlouhodobou analýzu.

---

## Zabezpečení Pluginů

Systém pluginů zavádí bezpečnostní rizika spouštění kódu třetích stran, která jsou zmírňována následujícími mechanismy:

### Bezpečnostní Sandbox

`PluginLoader` provádí přísné bezpečnostní skenování při načítání pluginů:

1. **Kontrola zakázaných jmenných prostorů** — Pluginy nemohou odkazovat na následující jmenné prostory:
   - `System.IO` — přístup k souborovému systému
   - `System.Net.Http` — HTTP požadavky
   - `System.Net.WebSockets` — WebSocket připojení
   - `System.Net.Sockets` — raw sockety
   - `Microsoft.CodeAnalysis` — API kompilátoru

2. **Whitelist důvěryhodných sestavení** — Reference na následující sestavení jsou povoleny:
   - `Google.Protobuf`, `Newtonsoft.Json`, `MessagePack`
   - `Serilog`, `Microsoft.Extensions.Logging.Abstractions`
   - `Dapper`

3. **Kontrola zakázaných typů** — Skenování nebezpečných typů odkazovaných v pluginu

4. **Kontrola zakázaných členů** — Skenování nebezpečných metod volaných v pluginu

### Izolované Načítání

- Každý plugin je izolovaně načten pomocí vlastního `AssemblyLoadContext`
- Typy a sestavení mezi pluginy se navzájem neovlivňují
- Při uvolnění pluginu lze uvolnit související zdroje

### Omezení Oprávnění Nástrojů

- Nástroje registrované pluginy prostřednictvím rozhraní `ITool` podléhají stejnému systému oprávnění
- Nástroje pluginů nemohou obejít 5-úrovňový řetězec oprávnění
- Nástroje pluginů podléhají označení `[SiliconManagerOnly]`

---

## Dynamická Kompilace: Bezpečnost

Samoevoluce (přepis tříd) přináší jedinečné bezpečnostní riziko. Systém je zmírňuje pomocí **vrstvené strategie**:

### Vrstva 1: Kontrola Referencí při Kompilaci (Primární Obrana)

- Kompilátor získá pouze **povolený seznam referencí na sestavení**.
- **Povoleno**: `System.Runtime`, `System.Private.CoreLib`, projektová sestavení (rozhraní ITool atd.)
- **Blokováno**: `System.IO`, `System.Reflection`, `System.Runtime.InteropServices` atd.
- Pokud kód odkazuje na blokované sestavení, **kompilátor sám kód odmítne**.
- Toto je spolehlivější než běhové skenování — nebezpečné operace jsou nemožné na úrovni typů.

### Vrstva 2: Běhová Statická Analýza (Sekundární Obrana)

- I po úspěšné kompilaci je kód podroben statickému skenování vzorů.
- Detekce nebezpečných vzorů operací (přímé I/O, systémová volání atd.).
- Pokud je nalezen nebezpečný kód, načtení je odmítnuto a systém se vrátí k výchozí funkčnosti.

### Omezení Dědičnosti

Všechny vlastní třídy silikonových bytostí **musí** dědit z `SiliconBeingBase`. Kompilátor vynucuje toto omezení na úrovni typů.

### Šifrované Úložiště

Zkompilovaný kód je na disku uložen šifrovaný pomocí AES-256:

- **Odvození klíče**: Z GUID bytosti (velká písmena) pomocí PBKDF2.
- **Selhání dešifrování**: Návrat k výchozí implementaci.
- **Běhová rekompilace**: Nový kód je nejprve zkompilován v paměti; perzistence probíhá až po úspěšné kompilaci a nahrazení instance.

### Atomické Nahrazení

Proces nahrazení je atomický:

1. Zkompilujte nový kód v paměti → získejte `Type`.
2. Vytvořte novou instanci z `Type`.
3. Migrujte stav ze staré instance do nové instance.
4. Prohoďte reference.
5. Perzistujte šifrovaný kód.

Pokud kterýkoli krok selže, stará instance zůstává aktivní.

---

## Callback Funkce Oprávnění

### Návrh

Každý PermissionManager drží **proměnnou callback funkce**:

- **Výchozí**: Odkazuje na vestavěnou výchozí funkci oprávnění.
- **Po dynamické kompilaci**: Přepsáno vlastní funkcí oprávnění bytosti.
- **Výběr jednoho ze dvou**: V každém okamžiku je aktivní pouze jeden callback.
- **Selhání kompilace**: Neovlivňuje aktuální callback — výchozí nebo poslední úspěšná vlastní funkce zůstává platná.

### Signatura Callbacku

```
PermissionResult Callback(PermissionType type, string resourcePath, Guid callerId)
```

Vrací `Allowed`, `Denied` nebo `AskUser`.

---

## Nejlepší Praktiky

### 1. Vždy Používejte Exekutory

Nikdy nepřistupujte ke zdrojům přímo:

```csharp
// ❌ Špatně - Přímý přístup k souboru
var content = File.ReadAllText("config.json");

// ✅ Správně - Použijte exekutor
var result = await executor.ExecuteAsync(new DiskReadRequest("config.json"));
```

### 2. Nastavte Přiměřené Časové Limity

```csharp
var request = new NetworkRequest
{
    Url = "https://api.example.com",
    Timeout = TimeSpan.FromSeconds(30) // Ne příliš dlouhý, ne příliš krátký
};
```

### 3. Monitorujte Auditní Logy

Pravidelně kontrolujte:
- Zamítnuté operace
- Neobvyklé vzory přístupu
- Časté Dotazy uživatele

### 4. Implementujte Vlastní Callbacky

Pro specifická pravidla vaší organizace:

```csharp
public class MyPermissionCallback : IPermissionCallback
{
    public async Task<PermissionResult> CheckAsync(PermissionRequest request)
    {
        // Vlastní logika
        if (IsOfficeHours() && IsInternalResource(request.Resource))
        {
            return PermissionResult.Allowed("Pracovní hodiny, interní zdroj");
        }
        
        return PermissionResult.Undecided();
    }
}
```

---

## Řešení Problémů

### Operace Trvale Zamítnuta

**Zkontrolujte**:
1. Stav IsCurator
2. HighDeny cache
3. Globální ACL pravidla
4. Logiku callbacku
5. Auditní logy pro detaily

### Dotaz Uživatele se Nikdy Nezobrazí

**Zkontrolujte**:
- Správně registrovaný IPermissionAskHandler
- Komunikační kanál je aktivní
- Žádný časový limit před odpovědí

### Výkon Exekutoru Je Pomalý

**Optimalizujte**:
- Zvyšte limit fondu vláken
- Upravte časové limity požadavků
- Zkontrolujte blokování operací

---

## Další Kroky

- 📚 Přečtěte si [Průvodce Architektury](architecture.md)
- 🛠️ Podívejte se na [Vývojářskou Příručku](development-guide.md)
- 🔒 Podívejte se na [Systém Oprávnění](permission-system.md)
- 🚀 Začněte s [Průvodcem Rychlým Startem](getting-started.md)
