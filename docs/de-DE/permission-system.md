# Berechtigungssystem

> **Version: v0.2.0-alpha**

[English](../en/permission-system.md) | **Deutsch** | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | [Русский](../ru-RU/permission-system.md)

## Übersicht

Das Berechtigungssystem stellt sicher, dass alle von KI initiierten Operationen angemessen validiert und überwacht werden.

## 3-stufige Berechtigungskette

```
┌─────────────────────────────────────────────┐
│          Berechtigungsvalidierung            │
├─────────────────────────────────────────────┤
│  Stufe 1: UserFrequencyCache                 │
│  ↓ Zwischengespeicherte Benutzerentscheidungen (HighDeny/HighAllow)│
│  Stufe 2: IPermissionCallback                │
│  ↓ Benutzerdefinierte Logik (Erlaubt/Verweigert/BenutzerFragen)│
│  Stufe 3: IsCurator?                         │
│  ↓ Ja → IPermissionAskHandler (Benutzer fragen)│
│  ↓ Nein → GlobalACL → Standardverweigerung    │
│  Ergebnis: Erlaubt oder Verweigert           │
└─────────────────────────────────────────────┘
```

> **Hinweis**: Die tatsächliche Abfragepriorität in `PermissionManager.CheckPermission()` ist:
> 1. **UserFrequencyCache** — Zuerst zwischengespeicherte Benutzerentscheidungen prüfen
> 2. **IPermissionCallback** — Benutzerdefinierte Callback-Regeln auswerten
> 3. **Kurator-Verzweigung** — Wenn Callback AskUser zurückgibt oder kein Callback konfiguriert:
>    - **Kurator** → `IPermissionAskHandler` (Benutzer über IM fragen)
>    - **Nicht-Kurator** → `GlobalACL` → Standardverweigerung

## Stufe 1: UserFrequencyCache

Pro-Being, nur im Speicher befindlicher Cache häufiger Benutzerentscheidungen (HighDeny/HighAllow).

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny** hat Priorität über **HighAllow**
- **Nur im Speicher**: Caches werden nicht persistent gespeichert, bei Neustart verloren
- **Konfigurierbarer Ablauf**: Benutzer können Gültigkeitsdauer für Cache-Einträge setzen

## Stufe 2: IPermissionCallback

Benutzerdefinierte Callbacks für dynamische Berechtigungslogik.

### DefaultPermissionCallback Standardimplementierung

`DefaultPermissionCallback` bietet umfassende Standard-Berechtigungsregeln, einschließlich:

#### Netzwerkzugriffsregeln
- **Loopback-Adressen**: localhost, 127.0.0.1, ::1 erlaubt
- **Private IP-Adressen**:
  - 192.168.x.x (Class C) - Erlaubt
  - 10.x.x.x (Class A) - Erlaubt
  - 172.16-31.x.x (Class B) - Benutzer fragen
- **Domain-Whitelist**:
  - Suchmaschinen: Google, Bing, DuckDuckGo, Yandex, Sogou etc.
  - KI-Dienste: OpenAI, Anthropic, HuggingFace, Ollama etc.
  - Entwicklerdienste: GitHub, StackOverflow, npm, NuGet etc.
  - Soziale Medien: Weibo, Zhihu, Reddit, Discord etc.
  - Videoplattformen: YouTube, Bilibili, Douyin, TikTok etc.
  - **Wetterinformationen**: wttr.in
  - Regierungswebsites: .gov, .go.jp, .go.kr
- **Domain-Blacklist**:
  - AI-Imitationswebsites: chatgpt, openai, deepseek etc. gefälschte Domains
  - Bösartige KI-Tools: wormgpt, darkgpt, fraudgpt etc.
  - AI-Content-Farmen und Schwarzmarkt-bezogene Domains

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

## Stufe 3: Kurator-Verzweigung (IsCurator → AskHandler / GlobalACL)

Wenn der Callback `AskUser` zurückgibt oder kein Callback konfiguriert ist, verzweigt das System basierend auf dem Kurator-Status:

### Kurator-Pfad: IPermissionAskHandler

Für den Silicon Curator fragt das System den Benutzer über IM nach einer Entscheidung.

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        SendMessage($"Allow {resource}?");

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

### Nicht-Kurator-Pfad: GlobalACL → Standardverweigerung

Für Nicht-Kurator-Beings prüft das System die globale Zugriffssteuerungsliste. Wenn keine passende Regel gefunden wird, wird die Anfrage standardmäßig verweigert.

#### GlobalACL-Struktur

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

Regeln werden sequentiell ausgewertet; erster Match gewinnt. Nur der Silicon Curator kann die globale ACL modifizieren.

#### Ressourcenformat

```
{type}:{path}

Beispiele:
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## IPermissionAskHandler (Kurator-Pfad)

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        SendMessage($"Allow {resource}?");

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

### PermissionRequestQueue Berechtigungsanfrage-Warteschlange

`PermissionRequestQueue` verwaltet ausstehende Berechtigungsanfragen und unterstützt asynchrones Warten auf Benutzerantworten:

- **Anfrage einreihen** — Wenn der Curator-Pfad erreicht wird, wird ein `TaskCompletionSource<AskPermissionResult>` erstellt und eingereiht
- **Web-UI-Anzeige** — Ausstehende Berechtigungsanfragen werden über den `PermissionRequestController` in der Web-UI angezeigt
- **Benutzerantwort** — Benutzer genehmigen oder verweigern in der Web-UI, mit optionaler Zwischenspeicherung der Entscheidung und Cache-Dauer
- **Cache-Optionen** — Benutzer können Berechtigungsentscheidungen für 1 Stunde, 24 Stunden, 7 Tage oder 30 Tage zwischenspeichern
- **Timeout-Mechanismus** — Automatisches Schließen der Anfrageseite nach 30 Minuten ohne Antwort

## Audit-System

Alle Berechtigungsentscheidungen werden protokolliert:

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "level": "GlobalACL",
  "reason": "Explicit rule granted"
}
```

## Programmatische Berechtigungsauswertung

### EvaluatePermission API

Die `PermissionManager.EvaluatePermission()`-Methode bietet schreibgeschützte Vorab-Berechtigungsauswertung, ohne Benutzer-Prompts auszulösen. `PermissionTool` verwendet diese Methode, damit die KI den Berechtigungsstatus prüfen kann, bevor sie eine Operation versucht.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**Rückgabewert**: Drei-Zustands-`PermissionResult`:
- `Allowed` - Operation ist erlaubt
- `Denied` - Operation ist verweigert
- `AskUser` - Benutzerbestätigung bei Ausführung erforderlich

**Auswertungsreihenfolge**:
1. **UserFrequencyCache** - Zwischengespeicherte Benutzerentscheidungen prüfen
2. **IPermissionCallback** - Benutzerdefinierte Callback-Auswertung
3. **Kurator-Verzweigung** - Wenn Kurator, gibt `AskUser` zurück (Bestätigung erforderlich); wenn Nicht-Kurator, prüft **GlobalACL**, dann Standardverweigerung

> **Hinweis**: Im Gegensatz zur vollständigen Berechtigungskette ruft `EvaluatePermission` **nicht** `IPermissionAskHandler` auf. Es meldet nur, was das Ergebnis bei Ausführung *wäre*.

## Berechtigungen verwalten

### Berechtigungen erteilen

**Über Web-UI**:
1. Zu **Berechtigungsverwaltung** navigieren
2. Auf **Regel hinzufügen** klicken
3. Konfigurieren:
   - Benutzer
   - Ressource
   - Erlauben/Verweigern
   - Dauer

**Über API**:
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

### Berechtigungen widerrufen

```bash
curl -X DELETE http://localhost:8080/api/permissions/{rule-id}
```

### Berechtigungen anzeigen

```bash
curl http://localhost:8080/api/permissions?userId=user-uuid
```

## Best Practices

### 1. Prinzip der geringsten Rechte

Nur die minimal erforderlichen Berechtigungen erteilen:

```json
{
  "resource": "disk:read",  // Nicht disk:*
  "allowed": true,
  "expiresAt": "2026-04-21T00:00:00Z"  // Immer Ablauf setzen
}
```

### 2. Zeitlich begrenzte Berechtigungen verwenden

Niemals permanente Berechtigungen erteilen, es ist absolut notwendig.

### 3. Berechtigungsprotokolle überwachen

Regelmäßig Audit-Logs prüfen für:
- Verweigerte Zugriffsversuche
- Ungewöhnliche Muster
- Berechtigungseskalationen

### 4. Benutzerdefinierte Callbacks implementieren

Für komplexe Logik, verwenden Sie `IPermissionCallback`:

```csharp
public async Task<PermissionResult> CheckAsync(PermissionRequest request)
{
    // Zeitbasierte Berechtigungen
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied("Outside business hours");
    }
    
    // Ressourcenbasierte Berechtigungen
    if (IsSensitiveResource(request.Resource))
    {
        return PermissionResult.Undecided("Requires approval");
    }
    
    return PermissionResult.Allowed();
}
```

## Häufige Szenarien

### Szenario 1: KI möchte Datei lesen

```
KI: "Ich muss config.json lesen"
↓
Berechtigungskette:
1. IsCurator? Nein
2. Ratenbegrenzung? OK
3. GlobalACL? Regel gefunden: disk:read = Erlaubt
4. Ergebnis: Erlaubt
```

### Szenario 2: KI möchte Code ausführen

```
KI: "Ich möchte Code kompilieren und ausführen"
↓
Berechtigungskette:
1. IsCurator? Nein
2. Ratenbegrenzung? OK
3. GlobalACL? Keine Regel gefunden
4. Callback? Gibt unentschieden zurück
5. Benutzer fragen? Benutzer genehmigt
6. Ergebnis: Erlaubt
```

### Szenario 3: Ratenbegrenzung überschritten

```
KI: "Ich muss 100 HTTP-Anfragen stellen"
↓
Berechtigungskette:
1. IsCurator? Nein
2. Ratenbegrenzung? Bereits überschritten
3. Ergebnis: Verweigert
```

## Fehlerbehebung

### Unerwartete Berechtigungsverweigerung

**Prüfen**:
1. Benutzer-IsCurator-Status
2. Ratenbegrenzungseinstellungen
3. GlobalACL-Regeln
4. Callback-Logik
5. Benutzerantwort-Timeout

### Berechtigung läuft nicht ab

**Prüfen**:
- `expiresAt`-Feld korrekt gesetzt
- Zeitzone korrekt
- Clock-Synchronisation

### Audit-Logs nicht aufgezeichnet

**Prüfen**:
- Audit-Logger registriert
- Storage-Backend zugänglich
- Ausreichend Festplattenspeicher

## Nächste Schritte

- 📚 [Architekturleitfaden](architecture.md) lesen
- 🛠️ [Entwicklungsleitfaden](development-guide.md) prüfen
- 🔒 [Sicherheitsdokumentation](security.md) ansehen
- 🚀 [Schnellstart-Leitfaden](getting-started.md) prüfen
