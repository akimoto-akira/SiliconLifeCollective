# Sicherheitsdesign

> **Version: v0.2.0-alpha**

[English](../en/security.md) | **Deutsch** | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | [Español](../es-ES/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | [Čeština](../cs-CZ/security.md)

## Übersicht

Die Sicherheit von Silicon Life Collective basiert auf einem **Mehrschichtigen Verteidigungs**modell. Kernprinzip: **Alle I/O-Operationen müssen durch Executoren**, Executoren erzwingen Berechtigungsprüfungen vor Ausführung.

```
Tool-Aufruf → Executor → PermissionManager → HighDeny-Cache → HighAllow-Cache → Callback → Benutzer fragen
```

---

## Berechtigungsmodell

### Berechtigungstypen

| Typ | Beschreibung |
|------|-------------|
| `NetworkAccess` | Ausgehende HTTP/HTTPS-Anfragen |
| `CommandLine` | Shell-Befehlsausführung |
| `FileAccess` | Datei- und Verzeichnisoperationen |
| `Function` | Sensible Funktionsaufrufe |
| `DataAccess` | Zugriff auf System- oder Benutzerdaten |

### Berechtigungsergebnisse

Jede Berechtigungsprüfung gibt eines von drei Ergebnissen zurück:

| Ergebnis | Verhalten |
|--------|----------|
| **Allowed (Erlaubt)** | Operation fährt sofort fort |
| **Denied (Verweigert)** | Operation blockiert, Audit-Protokoll erfasst |
| **AskUser (Benutzer fragen)** | Operation pausiert, erfordert Benutzerbestätigung |

### Sonderrolle: Silicon Curator

Der Silicon Curator hat höchste Berechtigungsstufe (`IsCurator = true`). Berechtigungsprüfungen des Curators lösen Benutzer-Eingabeaufforderung aus, andere Beings fragen GlobalACL synchron.

### Privater PermissionManager

Jedes Silicon Being hat seine eigene **private PermissionManager**-Instanz. Berechtigungsstatus werden nicht zwischen Beings geteilt.

---

## Berechtigungsvalidierungsablauf

Abfragepriorität: **1. Benutzer HighDeny → 2. Benutzer HighAllow → 3. Callback-Funktion**

```
┌─────────────┐
│ Tool-Aufruf  │
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Executor    │────▶│ Privater            │
│(Disk/Network/│     │ PermissionManager    │
│  Command...) │     │ (pro Being)          │
└─────────────┘     └────────┬────────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │ 1. UserFrequency│
                    │    Cache        │──HighDeny──▶ Denied
                    │(HighDeny/       │──HighAllow─▶ Allowed
                    │ HighAllow)      │
                    └────────┬────────┘
                             │ Kein Cache-Treffer
                             ▼
                    ┌─────────────────┐
                    │ 2. IPermission  │──▶ Allowed / Denied / AskUser
                    │    Callback     │
                    └────────┬────────┘
                             │ AskUser
                             ▼
                    ┌─────────────────┐
                    │ 3. IsCurator?   │──Ja──▶ IPermissionAskHandler
                    └────────┬────────┘       (Benutzer fragen)
                             │ Nein
                             ▼
                    ┌─────────────────┐
                    │ 4. GlobalACL    │──▶ Allowed / Denied
                    └────────┬────────┘
                             │ Keine Regel
                             ▼
                        Verweigert
```

**Wichtig**: Executor sieht nur Boolean (Allowed/Denied). PermissionManager verarbeitet intern Ternärentscheidung (Allowed/Denied/AskUser) und löst AskUser auf bevor Rückgabe an Executor.

---

## Executoren (Sicherheitsgrenze)

Executoren sind der **einzige** Pfad für I/O-Operationen. Sie erzwingen:

### Statische Klassen-Modell

Executoren sind als **statische Klassen** implementiert:

- Jeder Executor ist eine statische Klasse mit synchroner `Execute`-Methode
- Berechtigungsvalidierung blockiert den Aufruf-Thread bis Entscheidung getroffen
- Thread-Sicherheit durch interne Sperren gewährleistet
- Timeout-Kontrolle pro Anfrage

### Thread-Sperrung für Berechtigungsvalidierung

Wenn Tool Ressourcenzugriff initiiert:

1. Executor empfängt Anfrage und **blockiert den Aufruf-Thread**.
2. Executor queryt privaten PermissionManager des Beings.
3. Wenn Callback AskUser zurückgibt und Being ist Curator, Executor-Thread **bleibt blockiert** wartend auf Benutzerantwort.
4. Being sieht nur Endergebnis (Erfolg oder Verweigerung) — es sieht nie intermediären "Pending" oder "Waiting" Status.
5. Normale Beings (Nicht-Curator) queryen synchron GlobalACL ohne Blockierung.
6. Bei Timeout, Anfrage als Denied behandelt, Thread-Sperre freigegeben.

### Executor-Typen

| Executor | Umfang | Standard-Timeout |
|----------|-------|-----------------|
| `DiskExecutor` | Datei Lesen/Schreiben, Verzeichnisoperationen | 30 Sekunden |
| `NetworkExecutor` | HTTP-Anfragen, WebSocket-Verbindungen | 30 Sekunden |
| `CommandLineExecutor` | Shell-Befehlsausführung | 30 Sekunden |
| `DynamicCompilationExecutor` | Roslyn In-Memory-Kompilierung | 30 Sekunden |

### Ausnahmeisolation und Fehlertoleranz

- Ausnahme eines Executors beeinflusst andere Executoren nicht.
- Automatischer Neustart bei Thread-Absturz.
- Circuit Breaker: Temporäre Executor-Pausierung nach aufeinanderfolgenden Fehlern verhindert Kaskadenfehler.

---

## Global ACL (Access Control List)

Gemeinsame Regeltabelle persistent im Storage, nur vom Silicon Curator verwaltet:

```json
{
  "rules": [
    { "prefix": "network:api.github.com", "result": "Allowed" },
    { "prefix": "file:C:\\Windows", "result": "Denied" },
    { "prefix": "cli:rm -rf", "result": "Denied" }
  ]
}
```

- Regeln sequentiell bewertet; erster Match gewinnt.
- Nur Silicon Curator kann Global ACL modifizieren (durch dediziertes Tool).
- Änderungen sofort wirksam.
- Global ACL ist **nicht** in oben genannter Prioritätskette pro Abfrage — intern durch Callback-Funktion referenziert.

---

## Benutzer-Frequenz-Cache

Zur Reduzierung wiederholter Berechtigungsabfragen verwaltet System zwei **pro Being, nur Speicher** Caches:

| Cache | Verwendung |
|-------|---------|
| **HighAllow** | Ressourcen häufig vom Benutzer erlaubt |
| **HighDeny** | Ressourcen häufig vom Benutzer verweigert |

### Funktionsweise

- **Benutzerwahl, nicht Auto-Erkennung**: Bei AskUser-Triggern wählt Benutzer ob Ressource zum Cache hinzugefügt.
- **Prefix-Matching**: Unterstützt Ressourcenpfad-Prefix-Matching (z.B. `network:api.example.com/*`).
- **Priorität**: HighDeny priorisiert über HighAllow.
- **Nur Speicher**: Caches nicht persistent. Bei Neustart verloren.
- **Konfigurierbarer Ablauf**: Benutzer kann Gültigkeitsdauer für Cache-Einträge setzen.

### Cache-Aktualisierungsablauf

1. Permission-Callback gibt `AskUser` zurück.
2. Berechtigungssystem sendet Anfrage an Card-System (Web UI oder IM).
3. Benutzer trifft Entscheidung (Allowed/Denied) und **wählt ob Caching**.
4. Card-System gibt Entscheidung + Cache-Flag zurück.
5. Berechtigungssystem aktualisiert entsprechende Cache-Liste.
6. Zukünftige Anfragen mit Cache-Prefix sofort aufgelöst.

---

## Benutzerabfragemechanismus

Wenn Berechtigungsprüfung `AskUser` zurückgibt:

### Web UI: Interaktive Cards

Web-Frontend zeigt sofort **interaktive Card** mit:

- Ressourcentyp und -pfad
- Aktionsbeschreibung
| Allowed / Denied Buttons
- Optional "Immer erlauben" / "Immer verweigern" Checkbox (zum Frequenz-Cache hinzufügen)

### Instant Messaging (ohne Card-Unterstützung): Zufallscode

Für Messaging-Plattformen ohne interaktive Card-Unterstützung:

1. System generiert zwei zufällige 6-stellige Codes: **Allow-Code** und **Deny-Code**.
2. Sendet Nachricht mit Ressourceninfo und beiden Codes.
3. Benutzer muss exakten Allow-Code antworten zur Autorisierung. Jede andere Antwort als Denied behandelt.
4. Codes einmalig verwendbar, verhindert Replay-Angriffe.

### Timeout

- Timeout für alle AskUser-Anfragen gesetzt.
| Bei Timeout, Anfrage als **Denied** behandelt, Executor-Thread-Sperre freigegeben.

---

## Dynamische Kompilierungssicherheit

Selbstentwicklung (Klassenüberschreibung) introduces einzigartige Sicherheitsrisiken. System mildert sie durch **Mehrschichtige Strategie**:

### Schicht 1: Kompilierzeit-Referenzkontrolle (Primärverteidigung)

- Compiler erhält nur **Liste erlaubter Assembly-Referenzen**.
- **Erlaubt**: `System.Runtime`, `System.Private.CoreLib`, Projekt-Assemblies (ITool-Schnittstelle etc.)
- **Blockiert**: `System.IO`, `System.Reflection`, `System.Runtime.InteropServices` etc.
- Wenn Code blockierte Assembly referenziert, **Compiler selbst lehnt** Code ab.
| Zuverlässiger als Runtime-Scanning — gefährliche Operationen auf Typebene unmöglich.

### Schicht 2: Runtime-Statische Analyse (Sekundärverteidigung)

- Selbst nach erfolgreicher Kompilierung, Code gescannt nach statischen Mustern.
| Erkennt gefährliche Operationsmuster (direktes I/O, Systemaufrufe etc.).
| Bei gefährlichem Code, Laden abgelehnt, System fällt zurück auf Standardfunktionalität.

### Vererbungsbeschränkung

Alle benutzerdefinierten Silicon Being-Klassen **müssen** `SiliconBeingBase` erben. Compiler erzwingt diese Beschränkung auf Typebene.

### Verschlüsselte Speicherung

Kompilierter Code auf Festplatte mit AES-256 verschlüsselt gespeichert:

- **Schlüsselableitung**: Von Being-GUID (Großschrift) mittels PBKDF2.
- **Entschlüsselungsfehler**: Fällt zurück auf Standardimplementierung.
- **Runtime-Neukompilierung**: Neuer Code zuerst im Speicher kompiliert; erst nach erfolgreicher Kompilierung und Instanzersetzung persistent.

### Atomare Ersetzung

Ersetzungsprozess ist atomar:

1. Neuen Code im Speicher kompilieren → `Type` erhalten.
2. Neue Instanz aus `Type` erstellen.
3. Status von alter Instanz zu neuer Instanz migrieren.
4. Referenz austauschen.
5. Verschlüsselten Code persistent speichern.

Wenn irgendein Schritt fehlschlägt, alte Instanz bleibt aktiv.

---

## Permission-Callback-Funktionen

### Design

Jeder PermissionManager hält **Callback-Funktionsvariable**:

- **Standard**: Zeigt auf integrierte Standard-Permission-Funktion.
- **Nach dynamischer Kompilierung**: Überschrieben durch benutzerdefinierte Permission-Funktion des Beings.
- **Entweder-Oder**: Nur ein Callback jederzeit aktiv.
- **Kompilierungsfehler**: Beeinflusst aktuellen Callback nicht — Standard oder letzte erfolgreiche benutzerdefinierte Funktion bleibt aktiv.

### Callback-Signatur

```
PermissionResult Callback(PermissionType type, string resourcePath, Guid callerId)
```

Gibt `Allowed`, `Denied` oder `AskUser` zurück.

---

## Audit-Protokoll

Alle Berechtigungsentscheidungen protokolliert:

```
[2026-04-01 15:30:25] ALLOWED  | Being:AssistantA | Type:NetworkAccess | Resource:api.github.com | Source:HighAllowCache
[2026-04-01 15:30:26] DENIED   | Being:AssistantB | Type:FileAccess    | Resource:C:\Windows\System32 | Source:HighDenyCache
[2026-04-01 15:30:27] ASK_USER | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:Callback
[2026-04-01 15:30:28] ALLOWED  | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:UserDecision
```

Protokolle persistent im Storage, einsehbar durch Web UI (Log-Controller).

---

## Token-Nutzungsaudit

`TokenUsageAuditManager` bietet sicherheitsrelevantes KI-Token-Verbrauchs-Tracking:

- **Pro Anfrage Datensatz** — Jeder KI-Aufruf protokolliert Being-ID, Modell, Prompt-Token, Completion-Token und Zeitstempel.
- **Anomalie-Erkennung** — Ungewöhnliche Token-Verbrauchsmuster können Prompt-Injection oder Ressourcenmissbrauch indizieren.
- **Nur Curator-Zugriff** — `TokenAuditTool` (markiert `[SiliconManagerOnly]`) erlaubt Curator Token-Nutzung abzufragen und zusammenzufassen.
- **Web-Dashboard** — `UsageController` bietet browserbasiertes Dashboard mit Trendgrafiken und Datenexport.
- **Persistenter Storage** — Datensätze gespeichert durch `ITimeStorage` für Zeitreihenabfragen und Langzeitanalyse.

---

## Plugin-Sicherheit

Das Plugin-System führt Sicherheitsrisiken durch Drittanbieter-Code-Ausführung ein, die durch folgende Mechanismen gemildert werden:

### Sicherheits-Sandbox

`PluginLoader` führt beim Laden strege Sicherheitsprüfungen durch:

1. **Verbotene Namespace-Prüfung** — Plugins dürfen nicht auf folgende Namespaces verweisen:
   - `System.IO` — Dateisystemzugriff
   - `System.Net.Http` — HTTP-Anfragen
   - `System.Net.WebSockets` — WebSocket-Verbindungen
   - `System.Net.Sockets` — Raw-Sockets
   - `Microsoft.CodeAnalysis` — Compiler-API

2. **Vertrauenswürdige Assembly-Whitelist** — Referenzen auf folgende Assemblys sind erlaubt:
   - `Google.Protobuf`, `Newtonsoft.Json`, `MessagePack`
   - `Serilog`, `Microsoft.Extensions.Logging.Abstractions`
   - `Dapper`

3. **Verbotene Typ-Prüfung** — Scannt nach gefährlichen Typen, die im Plugin referenziert werden

4. **Verbotene Member-Prüfung** — Scannt nach gefährlichen Methoden, die im Plugin aufgerufen werden

### Isoliertes Laden

- Jedes Plugin wird durch benutzerdefinierten `AssemblyLoadContext` isoliert geladen
- Typen und Assemblys zwischen Plugins stören sich nicht gegenseitig
- Beim Entladen eines Plugins können zugehörige Ressourcen freigegeben werden

### Tool-Berechtigungsbeschränkungen

- Plugins, die Tools über `ITool`-Schnittstelle registrieren, unterliegen demselben Berechtigungssystem
- Plugin-Tools können die 5-stufige Berechtigungskette nicht umgehen
- Plugin-Tools unterliegen der `[SiliconManagerOnly]`-Markierung
