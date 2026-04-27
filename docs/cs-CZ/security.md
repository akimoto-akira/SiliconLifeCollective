# Bezpečnostní Návrh

> **Verze: v0.1.0-alpha**

[English](../en/security.md) | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | [Español](../es-ES/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | [Deutsch](../de-DE/security.md) | **Čeština**

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
| **Denied (Zamítnuto)** | Operace je blokována, zaznamenána do auditu |
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

Pro IM kanály bez podpory karet:

1. Systém generuje 6místný náhodný kód.
2. Odešle zprávu uživateli s kódem a popisem.
3. Uživatel odpoví kódem + rozhodnutím.
4. Systém ověří kód a aplikuje rozhodnutí.

---

## Auditní Systém

Všechny operace oprávnění jsou zaznamenávány:

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "beingId": "being-uuid",
  "userId": "user-0",
  "resource": "disk:write",
  "result": "Allowed",
  "level": "GlobalACL",
  "reason": "Explicitní pravidlo uděleno"
}
```

### Auditing Událostí

| Událost | Popis |
|--------|----------|
| `PermissionCheck` | Pokus o kontrolu oprávnění |
| `PermissionAllowed` | Oprávnění uděleno |
| `PermissionDenied` | Oprávnění zamítnuto |
| `PermissionAsked` | Vyžadováno rozhodnutí uživatele |
| `CacheUpdated` | Aktualizace frekvenční cache |

---

## Dynamická Kompilace: Bezpečnostní Mechanismy

### Šifrování Kódu

- Všechny dynamicky kompilované kódy jsou šifrovány pomocí **AES-256**.
- Klíč odvozen z GUID bytosti pomocí **PBKDF2**.
- Kód je dešifrován pouze při kompilaci.

### Bezpečnostní Skenování

Před načtením zkompilovaného kódu:

1. **Statická analýza**: Skenování nebezpečných vzorů kódu.
2. **Kontrola referencí**: Vyloučení nebezpečných sestav (System.IO, Reflection atd.).
3. **Kontrola volání**: Detekce zakázaných API volání.

### Izolace Paměti

- Kompilovaný kód běží v izolované paměťové oblasti.
- Žádný přímý přístup k paměti hlavního procesu.
- Automatické čištění při selhání kompilace.

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
