# Berechtigungssystem

> **Version: v0.2.0-alpha**

[English](../en/permission-system.md) | **Deutsch** | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | [Русский](../ru-RU/permission-system.md)

## Übersicht

Das Berechtigungssystem stellt sicher, dass alle KI-initiierten Vorgänge ordnungsgemäß überprüft und auditiert werden.

## Berechtigungsprüfungskette

```
┌─────────────────────────────────────────────┐
│          Berechtigungsprüfung               │
├─────────────────────────────────────────────┤
│  Ebene 1: UserFrequencyCache                │
│  ↓ Hochfrequente Benutzerentscheidungs-     │
│    cache (HighDeny/HighAllow)               │
│  Ebene 2: IPermissionCallback               │
│  ↓ Benutzerdefinierte Logik                 │
│    (Allowed/Denied/AskUser)                 │
│  Ebene 3: IsCurator?                        │
│  ↓ Ja → IPermissionAskHandler               │
│    (Benutzer fragen)                        │
│  ↓ Nein → Globale ACL → Standardmäßig      │
│    verweigert                               │
│  Ergebnis: Erlaubt oder Verweigert          │
└─────────────────────────────────────────────┘
```

> **Hinweis**: Die tatsächliche Abfragepriorität von `PermissionManager.CheckPermission()` ist:
> 1. **Benutzerfrequenz-Cache** — Zuerst den Hochfrequenz-Benutzerentscheidungs-Cache prüfen
> 2. **Berechtigungs-Callback-Schnittstelle** — Benutzerdefinierte Callback-Regeln auswerten
> 3. **Curator-Verzweigung** — Wenn der Callback AskUser zurückgibt oder kein Callback vorhanden ist:
>    - **Curator** → `IPermissionAskHandler` (Benutzer über IM fragen)
>    - **Nicht-Curator** → `Globale ACL` → Standardmäßig verweigern

## Ebene 1: UserFrequencyCache

Der Hochfrequenz-Benutzerentscheidungs-Cache (HighDeny/HighAllow) für jedes Lebewesen, nur im Arbeitsspeicher vorhanden.

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny hat Vorrang vor HighAllow**
- **Nur im Arbeitsspeicher**: Der Cache wird nicht persistent gespeichert und geht nach einem Neustart verloren
- **Konfigurierbare Ablaufzeit**: Benutzer können die Gültigkeitsdauer von Cache-Einträgen festlegen

## Ebene 2: IPermissionCallback

Benutzerdefinierter Callback für dynamische Berechtigungslogik.

### Standardimplementierung DefaultPermissionCallback

`DefaultPermissionCallback` bietet umfassende Standard-Berechtigungsregeln, einschließlich:

#### Netzwerkzugriffsregeln
- **Loopback-Adressen**: localhost, 127.0.0.1, ::1 erlauben
- **Private IP-Adressen**:
  - 192.168.x.x (Klasse C) - Erlaubt
  - 10.x.x.x (Klasse A) - Erlaubt
  - 172.16-31.x.x (Klasse B) - Benutzer fragen
- **Domain-Whitelist**:
  - Suchmaschinen: Google, Bing, DuckDuckGo, Yandex, Sogou usw.
  - KI-Dienste: OpenAI, Anthropic, HuggingFace, Ollama usw.
  - Entwicklerdienste: GitHub, StackOverflow, npm, NuGet usw.
  - Soziale Medien: Weibo, Zhihu, Reddit, Discord usw.
  - Videoplattformen: YouTube, Bilibili, Douyin, TikTok usw.
  - **Wetterinformationen**: wttr.in
  - Regierungswebsites: .gov, .go.jp, .go.kr
- **Domain-Blacklist**:
  - KI-Imitationswebsites: chatgpt, openai, deepseek und andere Phishing-Domains
  - Bösartige KI-Tools: wormgpt, darkgpt, fraudgpt usw.
  - KI-Content-Farmen und Schwarzmarkt-bezogene Domains

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

## Ebene 3: Verzweigungsentscheidung (IsCurator / Globale ACL)

Wenn der Callback `AskUser` zurückgibt oder kein Callback konfiguriert ist, verzweigt das System basierend auf der Curator-Identität:

### Curator-Verzweigung (IsCurator = true)

Für den Silicon Curator fordert das System über Instant Messaging eine Entscheidung vom Benutzer an:

```csharp
if (IsCurator)
{
    if (_askHandler != null)
    {
        AskPermissionResult userDecision = _askHandler.AskUser(callerId, permissionType, resource);
        // Benutzer bestätigt oder lehnt in der Web-UI ab
    }
}
```

### Nicht-Curator-Verzweigung (IsCurator = false)

Für Nicht-Curator-Lebewesen prüft das System die Globale ACL. Wenn keine übereinstimmende Regel gefunden wird, wird die Anfrage standardmäßig verweigert.

### Struktur der Globalen ACL

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

Regeln werden in der Reihenfolge ausgewertet; die erste übereinstimmende Regel gilt. Nur der Silicon Curator kann die Globale ACL ändern.

### Ressourcenformat

```
{type}:{path}

Beispiele:
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## IPermissionAskHandler

Wenn eine Curator-Operation eine Benutzerbestätigung erfordert, wird der Benutzer über `IPermissionAskHandler` um Berechtigung gebeten.

### IMPermissionAskHandler-Implementierung

`IMPermissionAskHandler` sendet Berechtigungsanfragen über die Web-UI an den Benutzer:

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        // Nachricht über Instant Messaging an den Benutzer senden
        SendMessageAsync($"Allow {resource}?");

        // Auf Benutzerantwort warten
        var response = WaitForResponseAsync();

        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

### Berechtigungsanfragewarteschlange

Die `PermissionRequestQueue` verwaltet ausstehende Berechtigungsanfragen und unterstützt asynchrones Warten auf Benutzerantworten:

- **Anfrage einreihen** — Wenn die Berechtigungskette Ebene 5 erreicht, wird ein `TaskCompletionSource<AskPermissionResult>` erstellt und eingereiht
- **Web-UI-Anzeige** — Ausstehende Berechtigungsanfragen werden über den `PermissionRequestController` in der Web-UI angezeigt
- **Benutzerantwort** — Der Benutzer genehmigt oder lehnt in der Web-UI ab, mit der Option, die Entscheidung zu cachen und die Cache-Dauer festzulegen
- **Cache-Optionen** — Der Benutzer kann Berechtigungsentscheidungen für 1 Stunde, 24 Stunden, 7 Tage oder 30 Tage cachen
- **Zeitüberschreitungsmechanismus** — Die Anfrageseite wird nach 60 Sekunden ohne Antwort automatisch geschlossen

## Auditsystem

Alle Berechtigungsentscheidungen werden protokolliert:

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

## Programmgestützte Berechtigungsauswertung

### EvaluatePermission-API

Die Methode `PermissionManager.EvaluatePermission()` bietet eine schreibgeschützte Berechtigungsvorabauswertung, die keine Benutzerabfragen auslöst. Das `PermissionTool` verwendet diese Methode, damit die KI den Berechtigungsstatus vor einem Vorgang prüfen kann.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**Rückgabewert**: Dreiwertiges `PermissionResult`:
- `Allowed` - Vorgang ist erlaubt
- `Denied` - Vorgang ist verweigert
- `AskUser` - Bei Ausführung ist eine Benutzerbestätigung erforderlich

**Auswertungsreihenfolge**:
1. **Frequenz-Cache** - Gecachte Benutzerentscheidungen prüfen
2. **Berechtigungs-Callback-Schnittstelle** - Benutzerdefinierte Callback-Auswertung
3. **Curator-Status** - Wenn Curator, wird `AskUser` zurückgegeben (Bestätigung erforderlich)
4. **Globale ACL** - Zugriffssteuerungsregeln prüfen
5. **Standard** - Verweigern bei keiner übereinstimmenden Regel

> **Hinweis**: Im Gegensatz zur vollständigen Berechtigungskette ruft `EvaluatePermission` **nicht** `IPermissionAskHandler` auf. Es meldet nur, was das Ergebnis bei Ausführung *wäre*.

## Berechtigungen verwalten

### Berechtigungen erteilen

**Über die Web-UI**:
1. Navigieren Sie zu **Berechtigungsverwaltung**
2. Klicken Sie auf **Regel hinzufügen**
3. Konfigurieren Sie:
   - Benutzer
   - Ressource
   - Erlauben/Verweigern
   - Dauer

**Über die API**:
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

### Berechtigungen widerrufen

Über die Berechtigungsverwaltungsseite in der Web-UI.

### Berechtigungen anzeigen

```bash
curl http://localhost:8080/api/permissions/list
```

## Werkzeugberechtigungssystem

Zusätzlich zur Berechtigungsprüfungskette auf Vorgangsebene bietet das System einen **Werkzeugberechtigungsmechanismus** zur Steuerung, welche Werkzeuge ein Silicon Being verwenden darf.

### Zweistufige Werkzeugberechtigungen

Werkzeugberechtigungen sind in zwei Ebenen unterteilt:

1. **Silicon-Being-Ebene** — Steuert, welche Werkzeugoperationen ein einzelnes Silicon Being verwenden darf
2. **Projektebene** — Steuert die im Projektraum verfügbaren Werkzeugoperationen, unabhängig von den Berechtigungen auf Silicon-Being-Ebene

### Werkzeugberechtigungskonfiguration

Jede Operation jedes Werkzeugs kann unabhängig als erlaubt oder verweigert konfiguriert werden:

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

### Berechtigungsvorlagen

Das System bietet vordefinierte Werkzeugberechtigungsvorlagen, die schnell auf ein Silicon Being angewendet werden können:

- **readonly** — Nur-Lese-Berechtigung (Leseoperationen erlaubt, Schreiboperationen verweigert)
- **full** — Vollständige Berechtigung (alle Operationen erlaubt)
- **restricted** — Eingeschränkte Berechtigung (nur grundlegende Operationen erlaubt)

### Web-UI-Verwaltung

Werkzeugberechtigungen über die Web-UI verwalten:

- **Silicon-Being-Werkzeugberechtigungsseite** — `/beings/tool-permissions`
- **Projekt-Werkzeugberechtigungsseite** — `/project/{id}/tool-permissions`

### API-Endpunkte

| Endpunkt | Methode | Beschreibung |
|----------|---------|--------------|
| `/api/beings/tool-permissions` | GET | Silicon-Being-Werkzeugberechtigungen abrufen |
| `/api/beings/tool-permissions` | PUT | Silicon-Being-Werkzeugberechtigungen aktualisieren |
| `/api/beings/tool-permissions/templates` | GET | Berechtigungsvorlagenliste abrufen |
| `/api/beings/tool-permissions/apply-template` | POST | Berechtigungsvorlage anwenden |
| `/api/projects/{id}/tool-permissions` | GET | Projekt-Werkzeugberechtigungen abrufen |
| `/api/projects/{id}/tool-permissions` | PUT | Projekt-Werkzeugberechtigungen aktualisieren |

---

## Bewährte Verfahren

### 1. Prinzip der geringsten Berechtigung

Nur die minimal erforderlichen Berechtigungen erteilen:

```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects\\MyApp\\config.json",
  "result": "Allowed"
}
```

### 2. Zeitlich begrenzte Berechtigungen verwenden

Niemals dauerhafte Berechtigungen erteilen, es sei denn, es ist absolut notwendig.

### 3. Berechtigungsprotokolle überwachen

Regelmäßig Audit-Protokolle überprüfen, um Folgendes zu erkennen:
- Verweigerte Zugriffsversuche
- Anomalie-Muster
- Berechtigungseskalationen

### 4. Benutzerdefinierte Callbacks implementieren

Für komplexe Logik `IPermissionCallback` verwenden:

```csharp
public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
{
    // Zeitbasierte Berechtigungen
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }
    
    // Ressourcenbasierte Berechtigungen
    if (IsSensitiveResource(resource))
    {
        return PermissionResult.AskUser;
    }
    
    return PermissionResult.Allowed;
}
```

## Häufige Szenarien

### Szenario 1: KI möchte eine Datei lesen

```
KI: "Ich muss config.json lesen"
↓
Berechtigungskette:
1. UserFrequencyCache? Keine gecachte Entscheidung
2. IPermissionCallback? Gibt AskUser zurück (nicht ausdrücklich erlaubt)
3. IsCurator? Nein → Globale ACL prüfen
4. Globale ACL? Regel gefunden: file:... = Allowed
5. Ergebnis: Erlaubt
```

### Szenario 2: KI möchte Code ausführen

```
KI: "Ich möchte Code kompilieren und ausführen"
↓
Berechtigungskette:
1. UserFrequencyCache? Keine gecachte Entscheidung
2. IPermissionCallback? Gibt AskUser zurück
3. IsCurator? Ja → IPermissionAskHandler
4. Benutzer genehmigt
5. Ergebnis: Erlaubt
```

### Szenario 3: Gecachte Verweigerung

```
KI: "Ich muss auf C:\Windows zugreifen"
↓
Berechtigungskette:
1. UserFrequencyCache? Im HighDeny-Cache gefunden
2. Ergebnis: Verweigert (keine weitere Prüfung erforderlich)
```

## Fehlerbehebung

### Unerwartet verweigerte Berechtigungen

**Prüfen**:
1. IsCurator-Status des Benutzers
2. HighDeny-Einträge im Frequenz-Cache
3. Globale-ACL-Regeln
4. Callback-Logik
5. Zeitüberschreitung bei Benutzerantwort

### Berechtigungen laufen nicht ab

**Prüfen**:
- `expiresAt`-Feld korrekt gesetzt
- Zeitzone korrekt
- Uhren synchronisiert

### Audit-Protokolle werden nicht aufgezeichnet

**Prüfen**:
- Audit-Logger registriert
- Speicher-Backend zugänglich
- Ausreichend Speicherplatz

## Nächste Schritte

- 📚 [Architekturleitfaden](architecture.md) lesen
- 🛠️ [Entwicklungsleitfaden](development-guide.md) ansehen
- 🔒 [Sicherheitsdokumentation](security.md) ansehen
- 🚀 [Schnellstartleitfaden](getting-started.md) ansehen
