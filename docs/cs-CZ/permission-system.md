# Systém Oprávnění

[English](../en/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | **Čeština**

## Přehled

Systém oprávnění zajišťuje, že všechny operace iniciované AI jsou řádně ověřeny a auditovány.

## 5úrovňový Řetězec Oprávnění

```
┌─────────────────────────────────────────────┐
│          Ověření Oprávnění                   │
├─────────────────────────────────────────────┤
│  Úroveň 1: IsCurator                         │
│  ↓ Pokud true, obejde všechny kontroly       │
│  Úroveň 2: UserFrequencyCache                │
│  ↓ Omezení rychlosti                         │
│  Úroveň 3: GlobalACL                         │
│  ↓ Seznam řízení přístupu                    │
│  Úroveň 4: IPermissionCallback               │
│  ↓ Vlastní logika                            │
│  Úroveň 5: IPermissionAskHandler             │
│  ↓ Dotazování uživatele                      │
│  Výsledek: Povoleno nebo Zamítnuto           │
└─────────────────────────────────────────────┘
```

## Úroveň 1: IsCurator

Správci/kurátoři obcházejí všechny kontroly oprávnění.

```csharp
if (user.IsCurator)
{
    return PermissionResult.Allowed("Přístup kurátora");
}
```

## Úroveň 2: UserFrequencyCache

Omezení rychlosti pro každého uživatele k prevenci zneužití.

```csharp
var cache = new UserFrequencyCache();
if (!cache.CheckLimit(userId, resource))
{
    return PermissionResult.Denied("Překročeno omezení rychlosti");
}
```

## Úroveň 3: GlobalACL

Globální seznam řízení přístupu definuje explicitní pravidla.

### Struktura ACL

```json
{
  "rules": [
    {
      "userId": "user-uuid",
      "resource": "disk:read",
      "allowed": true,
      "expiresAt": "2026-04-21T00:00:00Z"
    }
  ]
}
```

### Formát Zdroje

```
{typ}:{akce}

Příklady:
- disk:read
- disk:write
- network:http
- compile:execute
- system:info
```

## Úroveň 4: IPermissionCallback

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
    public async Task<PermissionResult> CheckAsync(PermissionRequest request)
    {
        // Vlastní logika
        if (IsSafeOperation(request))
        {
            return PermissionResult.Allowed("Bezpečná operace");
        }
        
        return PermissionResult.Undecided("Vyžaduje potvrzení uživatele");
    }
}
```

## Úroveň 5: IPermissionAskHandler

Dotazování uživatele na oprávnění, když všechny ostatní úrovně jsou nerozhodné.

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public async Task<AskPermissionResult> AskAsync(PermissionRequest request)
    {
        // Odeslat zprávu uživateli prostřednictvím IM
        await SendMessageAsync($"Povolit {request.Resource}?");
        
        // Čekat na odpověď uživatele
        var response = await WaitForResponseAsync();
        
        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

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
1. **Frekvenční cache** - Kontrola cacheovaných rozhodnutí uživatele
2. **IPermissionCallback** - Vyhodnocení vlastního callbacku
3. **Stav kurátora** - Pokud je kurátor, vrátí `AskUser` (vyžaduje potvrzení)
4. **Globální ACL** - Kontrola pravidel řízení přístupu
5. **Výchozí** - Zamítnutí, když žádná pravidla neodpovídají

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
public async Task<PermissionResult> CheckAsync(PermissionRequest request)
{
    // Oprávnění založená na čase
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied("Mimo pracovní hodiny");
    }
    
    // Oprávnění založená na zdroji
    if (IsSensitiveResource(request.Resource))
    {
        return PermissionResult.Undecided("Vyžaduje schválení");
    }
    
    return PermissionResult.Allowed();
}
```

## Běžné Scénáře

### Scénář 1: AI Chce Číst Soubor

```
AI: "Potřebuji číst config.json"
↓
Řetězec oprávnění:
1. IsCurator? Ne
2. Omezení rychlosti? Normální
3. GlobalACL? Nalezeno pravidlo: disk:read = Povoleno
4. Výsledek: Povoleno
```

### Scénář 2: AI Chce Spustit Kód

```
AI: "Chci kompilovat a spustit kód"
↓
Řetězec oprávnění:
1. IsCurator? Ne
2. Omezení rychlosti? Normální
3. GlobalACL? Žádné pravidlo nenalezeno
4. Callback? Vráceno nerozhodné
5. Dotaz uživatele? Uživatel schválil
6. Výsledek: Povoleno
```

### Scénář 3: Překročení Omezení Rychlosti

```
AI: "Potřebuji provést 100 HTTP požadavků"
↓
Řetězec oprávnění:
1. IsCurator? Ne
2. Omezení rychlosti? Již překročeno
3. Výsledek: Zamítnuto
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
