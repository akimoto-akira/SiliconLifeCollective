# Systém Oprávnění

> **Verze: v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | **Čeština**

## Přehled

Systém oprávnění zajišťuje, že všechny operace iniciované AI jsou řádně ověřeny a auditovány.

## 3úrovňový Řetězec Oprávnění

```
┌─────────────────────────────────────────────┐
│          Ověření Oprávnění                   │
├─────────────────────────────────────────────┤
│  Úroveň 1: UserFrequencyCache                │
│  ↓ Cacheovaná rozhodnutí uživatele (HighDeny/HighAllow)│
│  Úroveň 2: IPermissionCallback               │
│  ↓ Vlastní logika (Povoleno/Zamítnuto/AskUser)│
│  Úroveň 3: Větvení kurátora                  │
│  ↓ Ano → IPermissionAskHandler (dotaz uživatele)│
│  ↓ Ne  → GlobalACL → Výchozí zamítnutí       │
│  Výsledek: Povoleno nebo Zamítnuto           │
└─────────────────────────────────────────────┘
```

## Úroveň 1: UserFrequencyCache

Cache rozhodnutí uživatele s vysokou frekvencí. Priorita `HighDeny` > `HighAllow`. Pouze v paměti, nepřetrvává po restartu.

```csharp
var cache = new UserFrequencyCache();
if (cache.TryGet(userId, resource, out var cachedResult))
{
    return cachedResult;
}
```

## Úroveň 2: IPermissionCallback

Vlastní callbacky pro dynamickou logiku oprávnění.

### Výchozí Implementace DefaultPermissionCallback

`DefaultPermissionCallback` poskytuje komplexní výchozí pravidla oprávnění, včetně:

#### Pravidla Síťového Přístupu
- **Loopback adresy**: Povoleno localhost, 127.0.0.1, ::1
- **Privátní IP adresy**:
  - 192.168.x.x (Třída C) - Povoleno
  - 10.x.x.x (Třída A) - Povoleno
  - 172.16-31.x.x (Třída B) - Dotázat se uživatele
- **Bílá listina domén**:
  - Vyhledávače: Google, Bing, DuckDuckGo, Yandex, Sogou atd.
  - AI služby: OpenAI, Anthropic, HuggingFace, Ollama atd.
  - Vývojářské služby: GitHub, StackOverflow, npm, NuGet atd.
  - Sociální média: Weibo, Zhihu, Reddit, Discord atd.
  - Video platformy: YouTube, Bilibili, Douyin, TikTok atd.
  - **Informace o počasí**: wttr.in
  - Vládní stránky: .gov, .go.jp, .go.kr
- **Černá listina domén**:
  - AI impostor stránky: chatgpt, openai, deepseek a další padělané domény
  - Malicious AI nástroje: wormgpt, darkgpt, fraudgpt atd.
  - Domény související s AI obsahovými farmami a černými trhy

```csharp
public class DefaultPermissionCallback : IPermissionCallback
{
    public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
    {
        if (IsSafeOperation(resource))
        {
            return PermissionResult.Allowed;
        }
        
        return PermissionResult.AskUser;
    }
}
```

## Úroveň 3: Větvení kurátora

Když `IPermissionCallback` vrátí `AskUser`, systém se větví podle role volajícího:

- **Kurátor** (`IsCurator = true`) → Zobrazí výzvu uživateli prostřednictvím `IPermissionAskHandler`
- **Nekurátor** → Zkontroluje `GlobalACL`, pokud nenajde shodu, zamítne

### GlobalACL

Globální seznam řízení přístupu je sdílená tabulka pravidel perzistentní do úložiště, spravovaná pouze silikonovým kurátorem:

### Struktura ACL

```json
{
  "rules": [
    {
      "prefix": "network:api.github.com",
      "result": "Allowed"
    },
    {
      "prefix": "file:C:\\Windows",
      "result": "Denied"
    }
  ]
}
```

### Formát Zdroje

```
{typ}:{cesta}

Příklady:
- file:C:\Users\data
- network:api.github.com
- compile:execute
- system:info
```

## IPermissionAskHandler

Dotazování uživatele na oprávnění, když všechny ostatní úrovně jsou nerozhodné.

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        SendMessage($"Povolit {resource}?");

        var response = WaitForResponse();

        return new AskPermissionResult
        {
            Allowed = response.Approved,
            AddToCache = response.AddToCache,
            CacheDuration = response.CacheDuration
        };
    }
}
```

### Fronta Požadavků na Oprávnění PermissionRequestQueue

`PermissionRequestQueue` spravuje čekající požadavky na oprávnění s podporou asynchronního čekání na odpověď uživatele:

- **Zařazení požadavku** — Když řetězec oprávnění dosáhne větvení kurátora, vytvoří `TaskCompletionSource<AskPermissionResult>` a zařadí jej do fronty
- **Zobrazení ve Web UI** — Čekající požadavky na oprávnění se zobrazují ve Web UI prostřednictvím `PermissionRequestController`
- **Odpověď uživatele** — Uživatel může ve Web UI schválit nebo zamítnout, s možností cacheování rozhodnutí a nastavením doby trvání cache
- **Možnosti cache** — Uživatel může cacheovat rozhodnutí o oprávnění na 1 hodinu, 24 hodin, 7 dní nebo 30 dní
- **Mechanismus časového limitu** — Po 30 minutách bez odpovědi se požadavek automaticky zamítne

## Auditní Systém

Všechna rozhodnutí o oprávněních jsou zaznamenávána:

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "level": "GlobalACL",
  "reason": "Explicitní pravidlo uděleno"
}
```

## Programové Vyhodnocení Oprávnění

### API EvaluatePermission

Metoda `PermissionManager.EvaluatePermission()` poskytuje pouze readonly předběžné vyhodnocení oprávnění, které nespouští uživatelské výzvy. `PermissionTool` používá tuto metodu k umožnění AI zkontrolovat stav oprávnění před pokusem o operaci.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**Návratová hodnota**: Tříhodnotový `PermissionResult`:
- `Allowed` - Operace je povolena
- `Denied` - Operace je zamítnuta
- `AskUser` - Vyžaduje potvrzení uživatele při provádění

**Pořadí vyhodnocení**:
1. **UserFrequencyCache** - Kontrola cacheovaných rozhodnutí uživatele
2. **IPermissionCallback** - Vyhodnocení vlastního callbacku
3. **Větvení kurátora** - Pokud je kurátor, vrátí `AskUser` (vyžaduje potvrzení); pokud není kurátor, zkontroluje **GlobalACL**, poté zamítne ve výchozím stavu

> **Poznámka**: Na rozdíl od úplného řetězce oprávnění, `EvaluatePermission` **nevolá** `IPermissionAskHandler`. Pouze hlásí, jaký by výsledek *byl* při provádění.

## Správa Oprávnění

### Udělení Oprávnění

**Prostřednictvím Web UI**:
1. Navigujte na **Správa Oprávnění**
2. Klikněte na **Přidat Pravidlo**
3. Konfigurujte:
   - Uživatel
   - Zdroj
   - Povolit/Zamítnout
   - Doba trvání

**Prostřednictvím API**:
```bash
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "user-uuid",
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

### Odvolání Oprávnění

```bash
curl -X DELETE http://localhost:8080/api/permissions/{rule-id}
```

### Zobrazení Oprávnění

```bash
curl http://localhost:8080/api/permissions?userId=user-uuid
```

## Nejlepší Praktiky

### 1. Princip Minimálních Oprávnění

Udělte pouze minimální požadovaná oprávnění:

```json
{
  "resource": "disk:read",  // Ne disk:*
  "allowed": true,
  "expiresAt": "2026-04-21T00:00:00Z"  // Vždy nastavte expiraci
}
```

### 2. Používejte Oprávnění s Časovým Limitem

Nikdy neudělujte trvalá oprávnění, pokud to není absolutně nutné.

### 3. Monitorujte Logy Oprávnění

Pravidelně kontrolujte auditní logy pro:
- Zamítnuté pokusy o přístup
- Neobvyklé vzory
- Elevace oprávnění

### 4. Implementujte Vlastní Callbacky

Pro komplexní logiku použijte `IPermissionCallback`:

```csharp
public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
{
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }

    if (IsSensitiveResource(resource))
    {
        return PermissionResult.AskUser;
    }

    return PermissionResult.Allowed;
}
```

## Běžné Scénáře

### Scénář 1: AI Chce Číst Soubor

```
AI: "Potřebuji číst config.json"
↓
Řetězec oprávnění:
1. UserFrequencyCache? Žádné cacheované rozhodnutí
2. IPermissionCallback? Vráceno AskUser (není explicitně povoleno)
3. IsCurator? Ne → Zkontrolovat GlobalACL
4. GlobalACL? Nalezeno pravidlo: file:... = Povoleno
5. Výsledek: Povoleno
```

### Scénář 2: AI Chce Spustit Kód

```
AI: "Chci kompilovat a spustit kód"
↓
Řetězec oprávnění:
1. UserFrequencyCache? Žádné cacheované rozhodnutí
2. IPermissionCallback? Vráceno AskUser
3. IsCurator? Ano → IPermissionAskHandler
4. Uživatel schválil
5. Výsledek: Povoleno
```

### Scénář 3: Zamítnutí v Cache

```
AI: "Potřebuji přistupovat k C:\Windows"
↓
Řetězec oprávnění:
1. UserFrequencyCache? Nalezeno HighDeny v cache
2. Výsledek: Zamítnuto (žádné další kontroly potřeba)
```

## Řešení Problémů

### Neočekávané Zamítnutí Oprávnění

**Zkontrolujte**:
1. Stav IsCurator uživatele
2. Nastavení omezení rychlosti
3. Pravidla GlobalACL
4. Logiku callbacku
5. Časový limit odpovědi uživatele

### Oprávnění Nevyprší

**Zkontrolujte**:
- Pole `expiresAt` je správně nastaveno
- Časové pásmo je správné
- Synchronizace hodin

### Auditní Logy Nejsou Zaznamenávány

**Zkontrolujte**:
- Audit logger je registrován
- Storage backend je přístupný
- Dostatek místa na disku

## Další Kroky

- 📚 Přečtěte si [Průvodce Architektury](architecture.md)
- 🛠️ Podívejte se na [Vývojářskou Příručku](development-guide.md)
- 🔒 Podívejte se na [Bezpečnostní Dokumentaci](security.md)
- 🚀 Podívejte se na [Průvodce Rychlým Startem](getting-started.md)
