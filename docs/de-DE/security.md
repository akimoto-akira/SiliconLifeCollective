# Sicherheitsdesign

> **Version: v0.2.0-alpha**

[English](../en/security.md) | **Deutsch** | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | [Español](../es-ES/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | [Čeština](../cs-CZ/security.md) | [Русский](../ru-RU/security.md)

## Übersicht

Die Sicherheit von Silicon Life Collective basiert auf einem **Mehrschichtigen Verteidigungs**modell. Kernprinzip: **Alle I/O-Operationen müssen durch Executoren**, Executoren erzwingen Berechtigungsprüfungen vor der Ausführung.

```
Werkzeugaufruf → Executor → Berechtigungsmanager → Frequenz-Cache → Callback → (IsCurator: Benutzer fragen | Nicht-Kurator: Globale ACL)
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
| **Allowed (Erlaubt)** | Operation wird sofort fortgesetzt |
| **Denied (Verweigert)** | Operation blockiert, Audit-Protokoll erfasst |
| **AskUser (Benutzer fragen)** | Operation pausiert, erfordert Benutzerbestätigung |

### Sonderrolle: Silicon Curator

Der Silicon Curator hat die höchste Berechtigungsstufe (`IsCurator = true`). Wenn die Berechtigungsprüfung die Verzweigung erreicht, werden die Aktionen des Curators über `IPermissionAskHandler` den Benutzer zur Bestätigung gefragt, anstatt direkt zu Allowed kurzgeschlossen zu werden. Nicht-Kuratoren fragen die globale ACL ab.

### Privater Berechtigungsmanager

Jedes Silicon Being hat seine eigene **private Berechtigungsmanager**-Instanz. Berechtigungsstatus werden nicht zwischen Beings geteilt.

---

## Berechtigungsvalidierungsablauf

Abfragepriorität: **1. Frequenz-Cache → 2. Callback-Funktion → 3. Verzweigungsentscheidung (IsCurator/Globale ACL)**

```
┌─────────────┐
│ Werkzeugaufruf │
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Executor    │────▶│ Privater            │
│(Disk/Netzwerk/│     │ Berechtigungs-     │
│  Kommando...) │     │ manager (pro Being) │
└─────────────┘     └────────┬────────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │ 1. Frequenz-    │──Treffer──▶ Erlaubt / Verweigert
                    │    Cache        │
                    │(HighDeny hat    │
                    │ Vorrang vor     │
                    │ HighAllow)      │
                    └────────┬────────┘
                             │ Kein Treffer
                             ▼
                    ┌─────────────────┐
                    │ 2. Berechtigungs│──▶ Erlaubt / Verweigert / Benutzer fragen
                    │    Callback     │
                    └────────┬────────┘
                             │ Benutzer fragen
                             ▼
                    ┌─────────────────┐
                    │ 3. IsCurator?   │
                    └────────┬────────┘
                             │
                    ┌────────┴────────┐
                    │                 │
                    ▼ Ja              ▼ Nein
             ┌─────────────┐   ┌─────────────┐
             │ Benutzer    │   │ Globale ACL │
             │ fragen      │   │ Regeln      │
             │(AskHandler) │   │ abfragen    │
             └─────────────┘   └─────────────┘
```

**Wichtiger Punkt**: Executor sieht nur Boolean-Werte (Erlaubt/Verweigert). Der Berechtigungsmanager verarbeitet intern die Ternärentscheidung (Erlaubt/Verweigert/Benutzer fragen) und löst „Benutzer fragen" auf, bevor er an den Executor zurückgibt.

---

## Executoren (Sicherheitsgrenze)

Executoren sind der **einzige** Pfad für I/O-Operationen. Sie erzwingen:

### Unabhängige Dispatcher-Threads

Jeder Executor besitzt einen **unabhängigen Dispatcher-Thread**:

- Thread-Isolation zwischen Executoren — ein blockierter Executor-Thread beeinflusst andere Executoren nicht.
- Jeder Executor kann eigene Ressourcenlimits (CPU, Speicher etc.) setzen.
- Thread-Pool-Management für Executor-Threads.

### Anfragewarteschlange

Jeder Executor verwaltet eine Anfragewarteschlange:

- Anfragen werden nach Typ an den entsprechenden Executor geroutet.
- Prioritäts-Warteschlange wird unterstützt.
- Timeout-Kontrolle pro Anfrage.

### Thread-Sperrung für Berechtigungsvalidierung

Wenn ein Tool Ressourcenzugriff initiiert:

1. Executor empfängt Anfrage und **sperrt seinen Thread**.
2. Executor fragt den privaten Berechtigungsmanager des Beings ab.
3. Wenn der Callback „Benutzer fragen" zurückgibt, bleibt der Executor-Thread **gesperrt** und wartet auf die Benutzerantwort.
4. Das Being sieht nur das Endergebnis (Erfolg oder Verweigerung) — es sieht niemals den intermediären „Ausstehend"- oder „Wartend"-Status.
5. Nur der Silicon Curator löst echte Benutzerabfragen aus. Normale Beings fragen die globale ACL synchron ohne Blockierung ab.
6. Bei Timeout wird die Anfrage als verweigert behandelt und die Thread-Sperre freigegeben.

### Executor-Typen

| Executor | Umfang | Standard-Timeout |
|----------|-------|-----------------|
| `DiskExecutor` | Datei Lesen/Schreiben, Verzeichnisoperationen | 30 Sekunden |
| `NetworkExecutor` | HTTP-Anfragen, WebSocket-Verbindungen | 60 Sekunden |
| `CommandLineExecutor` | Shell-Befehlsausführung | 120 Sekunden |

> **Hinweis**: `DynamicCompilationExecutor` (im Namespace `SiliconLife.Core.Compilation`) ist für Roslyn-In-Memory-Kompilierung zuständig und gehört nicht zum I/O-Executor-Bereich, unterliegt jedoch ebenfalls dem Berechtigungssystem.

### Ausnahmeisolation und Fehlertoleranz

- Ausnahme eines Executors beeinflusst andere Executoren nicht.
- Automatischer Neustart bei Thread-Absturz.
- Circuit Breaker: Temporäre Executor-Pausierung nach aufeinanderfolgenden Fehlern verhindert Kaskadenfehler.

---

## Globale ACL (Access Control List)

Gemeinsame Regeltabelle persistent im Speicher, nur vom Silicon Curator verwaltet:

```json
{
  "rules": [
    { "prefix": "network:api.github.com", "result": "Allowed" },
    { "prefix": "file:C:\\Windows", "result": "Denied" },
    { "prefix": "cli:rm -rf", "result": "Denied" }
  ]
}
```

- Regeln werden sequentiell bewertet; der erste Treffer gewinnt.
- Nur der Silicon Curator kann die globale ACL modifizieren (durch sein dediziertes Tool).
- Änderungen sind sofort wirksam.
- Die globale ACL steht **nicht** in der oben genannten Abfrageprioritätskette — sie wird intern von der Callback-Funktion referenziert.

---

## Benutzer-Frequenz-Cache

Zur Reduzierung wiederholter Berechtigungsabfragen verwaltet das System zwei **pro Being, nur im Speicher** Caches:

| Cache | Verwendung |
|-------|---------|
| **HighAllow** | Ressourcen, die vom Benutzer häufig erlaubt wurden |
| **HighDeny** | Ressourcen, die vom Benutzer häufig verweigert wurden |

### Funktionsweise

- **Benutzerwahl, nicht Auto-Erkennung**: Wenn „Benutzer fragen" ausgelöst wird, wählt der Benutzer, ob die Ressource zum Cache hinzugefügt wird.
- **Präfix-Matching**: Unterstützt Ressourcenpfad-Präfix-Matching (z.B. `network:api.example.com/*`).
- **Priorität**: HighDeny hat Vorrang vor HighAllow.
- **Nur im Speicher**: Caches werden nicht persistent gespeichert. Bei Neustart verloren.
- **Konfigurierbarer Ablauf**: Benutzer kann Gültigkeitsdauer für Cache-Einträge festlegen.

### Cache-Aktualisierungsablauf

1. Berechtigungs-Callback gibt `AskUser` zurück.
2. Berechtigungssystem sendet Anfrage an das Card-System (Web-UI oder IM).
3. Benutzer trifft Entscheidung (Erlaubt/Verweigert) und **wählt ob Caching**.
4. Card-System gibt Entscheidung + Cache-Flag zurück.
5. Berechtigungssystem aktualisiert die entsprechende Cache-Liste.
6. Zukünftige Anfragen mit passendem Cache-Präfix werden sofort aufgelöst.

---

## Benutzerabfragemechanismus

Wenn die Berechtigungsprüfung `AskUser` zurückgibt:

### Web-UI: Interaktive Cards

Das Web-Frontend zeigt sofort eine **interaktive Card** mit:

- Ressourcentyp und -pfad
- Aktionsbeschreibung
- Erlauben / Verweigern Buttons
- Optionale „Immer erlauben" / „Immer verweigern" Checkbox (zum Frequenz-Cache hinzufügen)

### Instant Messaging (ohne Card-Unterstützung): Zufallscode

Für Messaging-Plattformen ohne interaktive Card-Unterstützung:

1. Das System generiert zwei zufällige 6-stellige Codes: **Allow-Code** und **Deny-Code**.
2. Sendet Nachricht mit Ressourceninfo und beiden Codes.
3. Der Benutzer muss den exakten Allow-Code antworten, um zu autorisieren. Jede andere Antwort wird als Verweigerung behandelt.
4. Codes sind einmalig verwendbar, um Replay-Angriffe zu verhindern.

### Timeout

- Timeout für alle „Benutzer fragen"-Anfragen gesetzt.
- Bei Timeout wird die Anfrage als **verweigert** behandelt und die Executor-Thread-Sperre freigegeben.

---

## Dynamische Kompilierungssicherheit

Selbstevolution (Klassenüberschreibung) bringt einzigartige Sicherheitsrisiken mit sich. Das System mildert sie durch eine **mehrschichtige Strategie**:

### Schicht 1: Kompilierzeit-Referenzkontrolle (Primärverteidigung)

- Der Compiler erhält nur eine **Liste erlaubter Assembly-Referenzen**.
- **Erlaubt**: `System.Runtime`, `System.Private.CoreLib`, Projekt-Assemblies (ITool-Schnittstelle etc.)
- **Blockiert**: `System.IO`, `System.Reflection`, `System.Runtime.InteropServices` etc.
- Wenn Code eine blockierte Assembly referenziert, **lehnt der Compiler selbst** den Code ab.
- Dies ist zuverlässiger als Runtime-Scanning — gefährliche Operationen sind auf Typebene unmöglich.

### Schicht 2: Runtime-Statische Analyse (Sekundärverteidigung)

- Selbst nach erfolgreicher Kompilierung wird der Code auf statische Muster gescannt.
- Erkennt gefährliche Operationsmuster (direktes I/O, Systemaufrufe etc.).
- Bei gefährlichem Code wird das Laden abgelehnt und das System fällt auf Standardfunktionalität zurück.

### Vererbungsbeschränkung

Alle benutzerdefinierten Silicon Being-Klassen **müssen** `SiliconBeingBase` erben. Der Compiler erzwingt diese Beschränkung auf Typebene.

### Verschlüsselte Speicherung

Kompilierter Code wird auf der Festplatte mit AES-256 verschlüsselt gespeichert:

- **Schlüsselableitung**: Von der GUID des Beings (Großschreibung) mittels PBKDF2.
- **Entschlüsselungsfehler**: Fällt zurück auf Standardimplementierung.
- **Runtime-Neukompilierung**: Neuer Code wird zuerst im Speicher kompiliert; erst nach erfolgreicher Kompilierung und Instanzersetzung persistent gespeichert.

### Atomare Ersetzung

Der Ersetzungsprozess ist atomar:

1. Neuen Code im Speicher kompilieren → `Type` erhalten.
2. Neue Instanz aus `Type` erstellen.
3. Status von alter Instanz zu neuer Instanz migrieren.
4. Referenz austauschen.
5. Verschlüsselten Code persistent speichern.

Wenn irgendein Schritt fehlschlägt, bleibt die alte Instanz aktiv.

---

## Berechtigungs-Callback-Funktionen

### Design

Jeder Berechtigungsmanager hält eine **Callback-Funktionsvariable**:

- **Standard**: Zeigt auf integrierte Standard-Berechtigungsfunktion.
- **Nach dynamischer Kompilierung**: Überschrieben durch benutzerdefinierte Berechtigungsfunktion des Beings.
- **Entweder-Oder**: Nur ein Callback ist jederzeit aktiv.
- **Kompilierungsfehler**: Beeinflusst den aktuellen Callback nicht — Standard oder letzte erfolgreiche benutzerdefinierte Funktion bleibt aktiv.

### Callback-Signatur

```
PermissionResult Callback(PermissionType type, string resourcePath, Guid callerId)
```

Gibt `Allowed`, `Denied` oder `AskUser` zurück.

---

## Audit-Protokoll

Alle Berechtigungsentscheidungen werden protokolliert:

```
[2026-04-01 15:30:25] ALLOWED  | Being:AssistantA | Type:NetworkAccess | Resource:api.github.com | Source:HighAllowCache
[2026-04-01 15:30:26] DENIED   | Being:AssistantB | Type:FileAccess    | Resource:C:\Windows\System32 | Source:HighDenyCache
[2026-04-01 15:30:27] ASK_USER | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:Callback
[2026-04-01 15:30:28] ALLOWED  | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:UserDecision
```

Protokolle werden persistent im Speicher abgelegt und sind über die Web-UI (Log-Controller) einsehbar.

---

## Token-Nutzungsaudit

Der `TokenUsageAuditManager` bietet sicherheitsrelevantes KI-Token-Verbrauchs-Tracking:

- **Pro-Anfrage-Datensatz** — Jeder KI-Aufruf protokolliert Being-ID, Modell, Prompt-Token, Completion-Token und Zeitstempel.
- **Anomalie-Erkennung** — Ungewöhnliche Token-Verbrauchsmuster können auf Prompt-Injection oder Ressourcenmissbrauch hindeuten.
- **Nur Curator-Zugriff** — Das `TokenAuditTool` (markiert mit `[SiliconManagerOnly]`) erlaubt dem Curator, die Token-Nutzung abzufragen und zusammenzufassen.
- **Web-Dashboard** — Der `UsageController` bietet ein browserbasiertes Dashboard mit Trendgrafiken und Datenexport.
- **Persistenter Speicher** — Datensätze werden über `ITimeStorage` gespeichert, für Zeitreihenabfragen und Langzeitanalyse.

---

## Plugin-Sicherheit

Das Plugin-System führt Sicherheitsrisiken durch Drittanbieter-Code-Ausführung ein, die durch folgende Mechanismen gemildert werden:

### Sicherheits-Sandbox und Fähigkeitsdeklaration

Der `PluginLoader` führt beim Laden Sicherheitsprüfungen durch und unterstützt gleichzeitig den Fähigkeitsdeklarationsmechanismus:

1. **Deklarierbare Fähigkeiten** — Plugins deklarieren erforderliche Fähigkeiten über das `[PluginCapability]`-Attribut:
   - `Network` — Netzwerkzugriff (erlaubt Referenzen auf `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`)
   - `FileIO` — Datei-Lese-/Schreibzugriff (erlaubt Referenzen auf `System.IO`)
   - `Process` — Prozessverwaltung
   - `AI` — KI-Aufrufe

2. **Nicht deklarierbare Fähigkeiten** — Die folgenden Fähigkeiten werden immer blockiert:
   - P/Invoke (`System.Runtime.InteropServices`)
   - Unsafe-Code (`System.Runtime.CompilerServices.Unsafe`)
   - Reflection Emit (`System.Reflection.Emit`)
   - Compiler-API (`Microsoft.CodeAnalysis`)

3. **Vertrauenswürdige Assembly-Whitelist** — Referenzen auf folgende Assemblys sind erlaubt:
   - `Google.Protobuf`, `Newtonsoft.Json`, `MessagePack`
   - `Serilog`, `Microsoft.Extensions.Logging.Abstractions`
   - `Dapper`

4. **Verbotene Typ-Prüfung** — Scannt nach gefährlichen Typen, die im Plugin referenziert werden

5. **Verbotene Member-Prüfung** — Scannt nach gefährlichen Methoden, die im Plugin aufgerufen werden

### Isoliertes Laden

- Jedes Plugin wird durch benutzerdefinierten `AssemblyLoadContext` isoliert geladen
- Typen und Assemblys zwischen Plugins stören sich nicht gegenseitig
- Beim Entladen eines Plugins können zugehörige Ressourcen freigegeben werden

### Tool-Berechtigungsbeschränkungen

- Plugins, die Tools über die `ITool`-Schnittstelle registrieren, unterliegen demselben Berechtigungssystem
- Plugin-Tools können die Berechtigungsvalidierungskette nicht umgehen
- Plugin-Tools unterliegen der `[SiliconManagerOnly]`-Markierung

---

## Tool-Berechtigungssicherheit

Das Tool-Berechtigungssystem bietet eine zusätzliche Sicherheitsschicht, die kontrolliert, welche Tool-Operationen Silicon Beings verwenden dürfen:

### Zwei-Ebenen-Berechtigungsisolation

1. **Silicon Being-Ebene** — Jedes Silicon Being hat eine unabhängige Tool-Berechtigungskonfiguration
2. **Projektebene** — Tool-Berechtigungen innerhalb eines Projektbereichs sind unabhängig von der Being-Ebene, wodurch bereichsübergreifende Berechtigungsisolation erreicht wird

### Berechtigungsvorlagen

Das System bietet vordefinierte Berechtigungsvorlagen, die eine Sicherheitsbaseline gewährleisten:

- **readonly** — Minimale Berechtigung, nur Leseoperationen erlaubt
- **restricted** — Eingeschränkte Berechtigung, nur grundlegende Operationen erlaubt
- **full** — Volle Berechtigung (nur für Curator)

### Sicherheitsmerkmale

- **Standardmäßig verweigert** — Nicht explizit erlaubte Tool-Operationen werden standardmäßig verweigert
- **Operationsgranularität** — Jede Operation jedes Tools wird unabhängig kontrolliert (z.B. `network:get` erlaubt, aber `network:post` verweigert)
- **Curator-Verwaltung** — Tool-Berechtigungen können nur vom Silicon Curator konfiguriert werden
- **Audit-Trail** — Tool-Berechtigungsänderungen werden im Audit-Protokoll erfasst
