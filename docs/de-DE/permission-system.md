# Berechtigungssystem

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md)

## Übersicht

Das Berechtigungssystem stellt sicher, dass alle von KI initiierten Operationen angemessen validiert und überwacht werden.

## 5-stufige Berechtigungskette

```
┌─────────────────────────────────────────────┐
│          Berechtigungsvalidierung            │
├─────────────────────────────────────────────┤
│  Stufe 1: IsCurator                          │
│  ↓ Umgeht wenn true                          │
│  Stufe 2: UserFrequencyCache                 │
│  ↓ Ratenbegrenzung                           │
│  Stufe 3: GlobalACL                          │
│  ↓ Access Control List                       │
│  Stufe 4: IPermissionCallback                │
│  ↓ Benutzerdefinierte Logik                  │
│  Stufe 5: IPermissionAskHandler              │
│  ↓ Benutzer fragen                           │
│  Ergebnis: Erlaubt oder Verweigert           │
└─────────────────────────────────────────────┘
```

## Stufe 1: IsCurator

Administratoren/Curatoren umgehen alle Berechtigungsprüfungen.

```csharp
if (user.IsCurator)
{
    return PermissionResult.Allowed("Curator access");
}
```

## Stufe 2: UserFrequencyCache

Ratenbegrenzung pro Benutzer zur Missbrauchsprävention.

```csharp
var cache = new UserFrequencyCache();
if (!cache.CheckLimit(userId, resource))
{
    return PermissionResult.Denied("Rate limit exceeded");
}
```

## Stufe 3: GlobalACL

Globale Access Control List definiert explizite Regeln.

### ACL-Struktur

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

### Ressourcenformat

```
{type}:{action}

Beispiele:
- disk:read
- disk:write
- network:http
- compile:execute
- system:info
```

## Stufe 4: IPermissionCallback

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
    public async Task<PermissionResult> CheckAsync(PermissionRequest request)
    {
        // Benutzerdefinierte Logik
        if (IsSafeOperation(request))
        {
            return PermissionResult.Allowed("Safe operation");
        }
        
        return PermissionResult.Undecided("Needs user confirmation");
    }
}
```

## Stufe 5: IPermissionAskHandler

Fragt Benutzer um Erlaubnis, wenn alle anderen Stufen unentschieden sind.

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public async Task<AskPermissionResult> AskAsync(PermissionRequest request)
    {
        // Nachricht über IM an Benutzer senden
        await SendMessageAsync($"Allow {request.Resource}?");
        
        // Auf Benutzerantwort warten
        var response = await WaitForResponseAsync();
        
        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

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

**Rückgabewert**: Drei-Zustand `PermissionResult`:
- `Allowed` - Operation erlaubt
- `Denied` - Operation verweigert
- `AskUser` - Erfordert Benutzerbestätigung bei Ausführung

**Auswertungsreihenfolge**:
1. **Frequency Cache** - Cached-Benutzerentscheidungen prüfen
2. **IPermissionCallback** - Benutzerdefinierte Callback-Auswertung
3. **Curator-Status** - Wenn Curator, gibt `AskUser` zurück (Bestätigung erforderlich)
4. **Global ACL** - Access-Control-Regeln prüfen
5. **Default** - Verweigert wenn keine Regel matcht

> **Hinweis**: Im Gegensatz zur vollständigen Berechtigungskette ruft `EvaluatePermission` **nicht** den `IPermissionAskHandler` auf. Es meldet nur, was das Ergebnis bei Ausführung *sein würde*.

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
