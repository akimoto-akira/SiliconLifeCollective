# Systém oprávnění

> **Verze: v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | **Čeština** | [Русский](../ru-RU/permission-system.md)

## Přehled

Systém oprávnění zajišťuje, že všechny operace iniciované AI jsou řádně ověřeny a auditovány.

## Řetězec ověřování oprávnění

```
┌─────────────────────────────────────────────┐
│          Ověřování oprávnění                │
├─────────────────────────────────────────────┤
│  Úroveň 1: UserFrequencyCache               │
│  ↓ Mezipaměť častých uživatelských rozhodnutí (HighDeny/HighAllow) │
│  Úroveň 2: IPermissionCallback              │
│  ↓ Vlastní logika (Allowed/Denied/AskUser)  │
│  Úroveň 3: IsCurator?                       │
│  ↓ Ano → IPermissionAskHandler (dotaz na uživatele) │
│  ↓ Ne → GlobalACL → výchozí zamítnutí      │
│  Výsledek: Povoleno nebo zamítnuto          │
└─────────────────────────────────────────────┘
```

> **Poznámka**: Skutečná priorita dotazů `PermissionManager.CheckPermission()` je:
> 1. **UserFrequencyCache** — nejprve zkontroluje mezipaměť častých uživatelských rozhodnutí
> 2. **IPermissionCallback** — vyhodnotí vlastní pravidla zpětného volání
> 3. **Větev Kurátora** — když zpětné volání vrátí AskUser nebo neexistuje žádné zpětné volání:
>    - **Kurátor** → `IPermissionAskHandler` (dotaz na uživatele přes IM)
>    - **Ne-Kurátor** → `GlobalACL` → výchozí zamítnutí

## Úroveň 1: UserFrequencyCache

Mezipaměť častých uživatelských rozhodnutí (HighDeny/HighAllow) pro každou bytost, existuje pouze v paměti.

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny má přednost před HighAllow**
- **Pouze v paměti**: mezipaměť není perzistentní, po restartu je ztracena
- **Konfigurovatelná doba expirace**: uživatel může nastavit dobu platnosti položek mezipaměti

## Úroveň 2: IPermissionCallback

Vlastní zpětné volání pro dynamickou logiku oprávnění.

### Výchozí implementace DefaultPermissionCallback

`DefaultPermissionCallback` poskytuje komplexní výchozí pravidla oprávnění, včetně:

#### Pravidla síťového přístupu
- **Loopback adresy**: povoleny localhost, 127.0.0.1, ::1
- **Privátní IP adresy**:
  - 192.168.x.x (Třída C) - povoleno
  - 10.x.x.x (Třída A) - povoleno
  - 172.16-31.x.x (Třída B) - dotaz na uživatele
- **Whitelist domén**:
  - Vyhledávače: Google, Bing, DuckDuckGo, Yandex, Sogou atd.
  - AI služby: OpenAI, Anthropic, HuggingFace, Ollama atd.
  - Vývojářské služby: GitHub, StackOverflow, npm, NuGet atd.
  - Sociální média: Weibo, Zhihu, Reddit, Discord atd.
  - Video platformy: YouTube, Bilibili, Douyin, TikTok atd.
  - **Počasí**: wttr.in
  - Vládní weby: .gov, .go.jp, .go.kr
- **Blacklist domén**:
  - Podvodné AI weby: chatgpt, openai, deepseek a další napodobující domény
  - Škodlivé AI nástroje: wormgpt, darkgpt, fraudgpt atd.
  - Domény související s AI content farmami a černým trhem

```csharp
public class DefaultPermissionCallback : IPermissionCallback
{
    public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
    {
        if (IsSafeOperation(permissionType, resource))
        {
            return PermissionResult.Allowed;
        }
        
        return PermissionResult.AskUser;
    }
}
```

## Úroveň 3: Rozhodnutí větve (IsCurator / GlobalACL)

Když zpětné volání vrátí `AskUser` nebo není nakonfigurováno žádné zpětné volání, systém se rozhoduje podle identity Kurátora:

### Větev Kurátora (IsCurator = true)

Pro Kurátora Křemíku systém požádá uživatele o rozhodnutí prostřednictvím okamžitých zpráv:

```csharp
if (IsCurator)
{
    if (_askHandler != null)
    {
        AskPermissionResult userDecision = _askHandler.AskUser(callerId, permissionType, resource);
        // Uživatel potvrdí nebo zamítne ve Web UI
    }
}
```

### Větev ne-Kurátora (IsCurator = false)

Pro bytosti, které nejsou Kurátory, systém zkontroluje Globální seznam řízení přístupu. Pokud není nalezena odpovídající pravidla, je požadavek ve výchozím nastavení zamítnut.

### Struktura GlobalACL

```json
{
  "rules": [
    {
      "permissionType": "NetworkAccess",
      "resourcePrefix": "api.github.com",
      "result": "Allowed"
    },
    {
      "permissionType": "FileAccess",
      "resourcePrefix": "C:\\Windows",
      "result": "Denied"
    }
  ]
}
```

Pravidla jsou vyhodnocována v pořadí, první odpovídající pravidlo platí. Pouze Kurátor Křemíku může upravovat Globální ACL.

### Formát prostředku

```
{typ}:{cesta}

Příklady:
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## IPermissionAskHandler

Když operace Kurátora vyžaduje potvrzení uživatele, je uživatel dotázán na oprávnění prostřednictvím `IPermissionAskHandler`.

### Implementace IMPermissionAskHandler

`IMPermissionAskHandler` odesílá žádosti o oprávnění uživateli prostřednictvím Web UI:

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        // Odeslání zprávy uživateli přes IM
        SendMessageAsync($"Povolit {resource}?");

        // Čekání na odpověď uživatele
        var response = WaitForResponseAsync();

        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

### Fronta žádostí o oprávnění PermissionRequestQueue

`PermissionRequestQueue` spravuje nevyřízené žádosti o oprávnění, podporuje asynchronní čekání na odpověď uživatele:

- **Zařazení žádosti** — když řetězec oprávnění dosáhne úrovně 5, vytvoří `TaskCompletionSource<AskPermissionResult>` a zařadí jej do fronty
- **Zobrazení ve Web UI** — nevyřízené žádosti o oprávnění jsou zobrazeny ve Web UI prostřednictvím `PermissionRequestController`
- **Odpověď uživatele** — uživatel ve Web UI schválí nebo zamítne, s možností uložení rozhodnutí do mezipaměti a nastavením doby trvání mezipaměti
- **Možnosti mezipaměti** — uživatel může uložit rozhodnutí o oprávnění do mezipaměti na 1 hodinu, 24 hodin, 7 dní nebo 30 dní
- **Mechanismus timeoutu** — po 60 sekundách bez odpovědi je stránka žádosti automaticky uzavřena

## Auditní systém

Všechna rozhodnutí o oprávněních jsou zaznamenávána:

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "callerId": "being-uuid",
  "permissionType": "FileAccess",
  "resource": "C:\\data\\config.json",
  "result": "Allowed",
  "reason": "Global ACL"
}
```

## Programové vyhodnocení oprávnění

### EvaluatePermission API

Metoda `PermissionManager.EvaluatePermission()` poskytuje vyhodnocení oprávnění pouze pro čtení, nespouští výzvy pro uživatele. `PermissionTool` používá tuto metodu, aby AI mohlo zkontrolovat stav oprávnění před pokusem o operaci.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**Návratová hodnota**: Třístavový `PermissionResult`:
- `Allowed` - operace je povolena
- `Denied` - operace je zamítnuta
- `AskUser` - při provedení bude vyžadováno potvrzení uživatele

**Pořadí vyhodnocení**:
1. **Frekvenční mezipaměť** - kontrola uložených uživatelských rozhodnutí
2. **IPermissionCallback** - vyhodnocení vlastního zpětného volání
3. **Stav Kurátora** - pokud je Kurátor, vrátí `AskUser` (vyžaduje potvrzení)
4. **Globální ACL** - kontrola pravidel řízení přístupu
5. **Výchozí** - zamítnutí při žádném odpovídajícím pravidle

> **Poznámka**: Na rozdíl od úplného řetězce oprávnění, `EvaluatePermission` **nezavolá** `IPermissionAskHandler`. Pouze hlásí, jaký *bude* výsledek při provedení.

## Správa oprávnění

### Udělení oprávnění

**Přes Web UI**:
1. Přejděte na **Správa oprávnění**
2. Klikněte na **Přidat pravidlo**
3. Nakonfigurujte:
   - Uživatel
   - Prostředek
   - Povolit/Zamítnout
   - Doba trvání

**Přes API**:
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "permissionType": "FileAccess",
    "resourcePrefix": "C:\\Projects",
    "result": "Allowed",
    "description": "Allow project directory access"
  }'
```

### Odvolání oprávnění

Prostřednictvím stránky správy oprávnění ve Web UI.

### Zobrazení oprávnění

```bash
curl http://localhost:8080/api/permissions/list
```

## Systém oprávnění nástrojů

Kromě řetězce ověřování oprávnění na úrovni operací systém poskytuje mechanismus správy **oprávnění nástrojů** pro řízení, které nástroje mohou Křemíkové Bytosti používat.

### Dvouúrovňová oprávnění nástrojů

Oprávnění nástrojů jsou rozdělena do dvou úrovní:

1. **Úroveň Křemíkové Bytosti** — řídí, které operace nástrojů může jednotlivá Křemíková Bytost používat
2. **Úroveň projektu** — řídí dostupné operace nástrojů v projektovém prostoru, nezávisle na oprávněních na úrovni Křemíkové Bytosti

### Konfigurace oprávnění nástrojů

Každá operace každého nástroje může být nezávisle nakonfigurována jako povolená nebo zamítnutá:

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "network:get": "allowed",
    "network:post": "denied",
    "disk:read": "allowed",
    "disk:write": "denied",
    "database:query": "allowed"
  }
}
```

### Šablony oprávnění

Systém poskytuje předdefinované šablony oprávnění nástrojů, které lze rychle aplikovat na Křemíkové Bytosti:

- **readonly** — oprávnění pouze pro čtení (povoleny operace čtení, zamítnuty operace zápisu)
- **full** — úplná oprávnění (povoleny všechny operace)
- **restricted** — omezená oprávnění (povoleny pouze základní operace)

### Správa přes Web UI

Správa oprávnění nástrojů přes Web UI:

- **Stránka oprávnění nástrojů Křemíkové Bytosti** — `/beings/tool-permissions`
- **Stránka oprávnění nástrojů projektu** — `/project/{id}/tool-permissions`

### API koncové body

| Koncový bod | Metoda | Popis |
|------|------|------|
| `/api/beings/tool-permissions` | GET | Získání oprávnění nástrojů Křemíkové Bytosti |
| `/api/beings/tool-permissions` | PUT | Aktualizace oprávnění nástrojů Křemíkové Bytosti |
| `/api/beings/tool-permissions/templates` | GET | Získání seznamu šablon oprávnění |
| `/api/beings/tool-permissions/apply-template` | POST | Aplikace šablony oprávnění |
| `/api/projects/{id}/tool-permissions` | GET | Získání oprávnění nástrojů projektu |
| `/api/projects/{id}/tool-permissions` | PUT | Aktualizace oprávnění nástrojů projektu |

### Akční oprávnění dovedností

Dovednosti využívají mechanismus akčních oprávnění nástrojů: ID dovednosti slouží jako název nástroje, akce je `execute`.

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "daily_news_digest:execute": "denied",
    "code_review:execute": "allowed"
  }
}
```

- Zakázané dovednosti se neobjeví v AI viditelných definicích nástrojů (AI je vůbec "nevidí")
- Během provádění dovednosti probíhá runtime kontrola, takže i zastaralé schéma nelze obejít
- Oprávnění nástrojů uvnitř dovednosti = oprávnění bytosti ∪ vlastní omezení dovednosti (striektní sjednocení na straně omezení, lze pouze zužovat, ne rozšiřovat)

### Akční oprávnění MCP obalových nástrojů

Každý obalový nástroj injektovaný MCP serverem (`mcp_{serverId}_{toolName}`) automaticky deklaruje jedinou akci `execute`:

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "mcp_filesystem_read_file:execute": "denied",
    "mcp_github_create_issue:execute": "allowed"
  }
}
```

- Dostupnost externích nástrojů lze přesně řídit podle bytosti nebo projektu
- Pokud jsou všechny akce `execute` určitého serveru zakázány, je nástroj jako celek odstraněn z AI viditelného schématu
- Zakázání/odstranění serveru (operace Web UI) okamžitě zruší registraci všech jeho nástrojů

---

## Osvědčené postupy

### 1. Princip nejmenšího oprávnění

Udělte pouze minimální potřebná oprávnění:

```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects\\MyApp\\config.json",
  "result": "Allowed"
}
```

### 2. Používejte časově omezená oprávnění

Nikdy neudělujte trvalá oprávnění, pokud to není absolutně nezbytné.

### 3. Sledujte protokoly oprávnění

Pravidelně kontrolujte auditní protokoly pro:
- Zamítnuté pokusy o přístup
- Abnormální vzorce
- Eskalaci oprávnění

### 4. Implementujte vlastní zpětné volání

Pro komplexní logiku použijte `IPermissionCallback`:

```csharp
public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
{
    // Časově založená oprávnění
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }
    
    // Zdrojově založená oprávnění
    if (IsSensitiveResource(resource))
    {
        return PermissionResult.AskUser;
    }
    
    return PermissionResult.Allowed;
}
```

## Běžné scénáře

### Scénář 1: AI chce číst soubor

```
AI: "Potřebuji přečíst config.json"
↓
Řetězec oprávnění:
1. UserFrequencyCache? Žádné uložené rozhodnutí
2. IPermissionCallback? Vrací AskUser (není výslovně povoleno)
3. IsCurator? Ne → kontrola GlobalACL
4. GlobalACL? Nalezeno pravidlo: file:... = Allowed
5. Výsledek: Povoleno
```

### Scénář 2: AI chce spustit kód

```
AI: "Chci zkompilovat a spustit kód"
↓
Řetězec oprávnění:
1. UserFrequencyCache? Žádné uložené rozhodnutí
2. IPermissionCallback? Vrací AskUser
3. IsCurator? Ano → IPermissionAskHandler
4. Uživatel schválí
5. Výsledek: Povoleno
```

### Scénář 3: Zamítnutí z mezipaměti

```
AI: "Potřebuji přístup k C:\Windows"
↓
Řetězec oprávnění:
1. UserFrequencyCache? Nalezeno v HighDeny mezipaměti
2. Výsledek: Zamítnuto (bez další kontroly)
```

## Řešení problémů

### Neočekávané zamítnutí oprávnění

**Zkontrolujte**:
1. Stav IsCurator uživatele
2. Položky HighDeny ve frekvenční mezipaměti
3. Pravidla GlobalACL
4. Logiku zpětného volání
5. Timeout odpovědi uživatele

### Oprávnění nevypršelo

**Zkontrolujte**:
- Pole `expiresAt` je správně nastaveno
- Časové pásmo je správné
- Hodiny jsou synchronizovány

### Auditní protokol nezaznamenává

**Zkontrolujte**:
- Auditní protokolovač je registrován
- Úložný backend je přístupný
- Dostatek místa na disku

## Další kroky

- 📚 Přečtěte [příručku architektury](architecture.md)
- 🛠️ Prohlédněte [vývojářskou příručku](development-guide.md)
- 🔒 Prohlédněte [dokumentaci zabezpečení](security.md)
- 🚀 Prohlédněte [příručku rychlého startu](getting-started.md)
