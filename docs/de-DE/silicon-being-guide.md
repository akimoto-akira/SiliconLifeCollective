# Silicon-Being-Leitfaden

> **Version: v0.2.0-alpha**

[English](../en/silicon-being-guide.md) | **Deutsch** | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md) | [Русский](../ru-RU/silicon-being-guide.md)

## Übersicht

Silicon Beings sind KI-gesteuerte Agenten, die autonom denken, handeln und sich weiterentwickeln können.

## Architektur

### Körper-Gehirn-Trennung

```
┌─────────────────────────────────────┐
│         Silicon Being               │
├──────────────────┬──────────────────┤
│   Körper         │   Gehirn         │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • Zustands-      │ • Historie       │
│   verwaltung     │   laden          │
│ • Trigger-       │ • KI aufrufen    │
│   erkennung      │ • Werkzeuge      │
│ • Lebenszyklus   │   ausführen      │
│                  │ • Antworten      │
│                  │   persistieren   │
└──────────────────┴──────────────────┘
```

## Soul-Datei

### Struktur

```markdown
# Being Name

## Personality
Describe the being's personality traits and characteristics.

## Capabilities
List what this being can do.

## Behavior Guidelines
Define how the being should behave in different situations.

## Knowledge Domain
Specify the being's area of expertise.
```

### Beispiel

```markdown
# Code Review Assistant

## Personality
You are a meticulous code reviewer with 10 years of experience.
You provide constructive feedback and always explain your reasoning.

## Capabilities
- Review code for bugs and best practices
- Suggest performance optimizations
- Explain complex algorithms
- Identify security vulnerabilities

## Behavior Guidelines
- Start with positive observations
- Provide specific examples
- Explain why changes are needed
- Be respectful and professional

## Knowledge Domain
Specialized in C#, .NET, and software architecture.
```

## Lebewesen erstellen

### Über die Web-UI

1. Navigieren Sie zu **Being-Verwaltung**
2. Klicken Sie auf **Neues Being erstellen**
3. Füllen Sie aus:
   - Name
   - Soul-Inhalt
   - Konfigurationsoptionen
4. Klicken Sie auf **Erstellen**

### Über die API

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistant",
    "soul": "# Personality\nYou are helpful..."
  }'
```

## Lebenszyklus eines Beings

### Aktive Zustände

Ein Silicon Being hat folgende aktive Zustände:

| Zustand | Beschreibung |
|---------|--------------|
| `Idle` | Ruhezustand, wartet auf Clock-Trigger |
| `SingleChat` | Einzelchat läuft |
| `GroupChat` | Gruppenchat läuft |
| `Task` | Aufgabe wird ausgeführt |
| `Timer` | Timer wird ausgeführt |
| `Stopped` | Gestoppt, aufgrund aufeinanderfolgender Fehler oder manuellem Stopp |

**Stopped-Zustandsmechanismus**:
- Wenn ein Silicon Being 10 aufeinanderfolgende Fehler verursacht, wechselt es automatisch in den `Stopped`-Zustand
- Im Stopped-Zustand führt das Being keine Aufgaben mehr aus
- Wenn eine neue Chat-Nachricht eintrifft, wird der Fehlerzähler zurückgesetzt und das Being nimmt den Betrieb wieder auf
- Es kann auch durch manuellen Eingriff neu gestartet werden

### Zustandsübergänge

```
Idle → SingleChat → Idle (Chat abgeschlossen)
Idle → GroupChat → Idle (Gruppenchat abgeschlossen)
Idle → Task → Idle (Aufgabe abgeschlossen)
Idle → Timer → Idle (Timer abgeschlossen)
Beliebig → Stopped (10 aufeinanderfolgende Fehler)
Stopped → Idle (Neue Chat-Nachricht oder manueller Neustart)
```

### Operationen

- **Starten**: Initialisieren und mit der Verarbeitung beginnen
- **Stoppen**: Ordentlich herunterfahren
- **Neustarten**: Vom Stopped-Zustand in den Idle-Zustand zurückkehren

## Aufgabensystem

### Aufgabe erstellen

```csharp
var task = new BeingTask
{
    BeingId = being.Id,
    Description = "Review the code",
    Priority = 5,
    DueDate = DateTime.UtcNow.AddHours(2)
};

await taskSystem.CreateAsync(task);
```

### Aufgabenstatus

- `Pending` - Wartet auf Ausführung
- `Running` - Wird ausgeführt
- `SubmittedForReview` - Zur Prüfung eingereicht
- `UnderReview` - In Prüfung
- `Rework` - Nacharbeit erforderlich
- `Completed` - Erfolgreich abgeschlossen
- `Failed` - Ausführung fehlgeschlagen
- `Cancelled` - Manuell abgebrochen

## Timer-System

### Timer-Typen

1. **Einmalig**: Einmalige Ausführung nach Verzögerung
2. **Intervall**: Wiederholte Ausführung in festen Intervallen
3. **Cron**: Ausführung basierend auf Cron-Ausdrücken

### Beispiel

```csharp
// Stündlich ausführen
var timer = new BeingTimer
{
    BeingId = being.Id,
    Interval = TimeSpan.FromHours(1),
    Action = "think",
    Repeat = true
};

await timerSystem.StartAsync(timer);
```

## Speichersystem

### Speichertypen

- **Kurzzeit**: Aktueller Dialogkontext
- **Langzeit**: Persistentes Wissen und Erfahrungen
- **Episodisch**: Zeitindizierte Ereignisse und Interaktionen

### Speicherstruktur

Default-Version:
```
data/
└── beings/
    └── {being-id}/
        ├── soul.md
        ├── memory/
        │   ├── short-term.json
        │   └── long-term/
        │       ├── 2026-04-20.json
        │       └── 2026-04-21.json
        └── tasks/
            └── task-history.json
```

Fast-Version (SpeedyPack-Speicher):
```
data/
├── speedy/
│   ├── {being-id}.spk       # SpeedyPack-Speicherdatei
│   └── {being-id}.spk.idx   # Indexdatei
└── beings/
    └── {being-id}/
        └── soul.md
```

## Arbeitsnotiz-System

### Übersicht

Arbeitsnotizen sind das persönliche Tagebuchsystem des Silicon Beings mit seitenbasiertem Design zur Aufzeichnung von Arbeitsfortschritten, Lernnotizen, Projektnotizen usw.

### Eigenschaften

- **Seitenbasierte Verwaltung**: Jede Notiz bildet eine eigenständige Seite, zugreifbar nach Seitennummer
- **Markdown-Unterstützung**: Inhalte unterstützen Markdown-Formatierung (Text, Listen, Tabellen, Code-Blöcke)
- **Schlüsselwortindex**: Unterstützung zum Hinzufügen von Schlüsselwörtern zu Notizen für die Suche
- **Zusammenfassungsfunktion**: Jede Notiz hat eine kurze Zusammenfassung für schnellen Überblick
- **Inhaltsverzeichnisgenerierung**: Kann eine Verzeichnisübersicht aller Notizen erstellen, um den Gesamtkontext zu verstehen
- **Zeitstempel**: Automatische Aufzeichnung von Erstellungs- und Aktualisierungszeiten
- **Standardmäßig privat**: Nur das Being selbst hat Zugriff (der Curator kann verwalten)

### Anwendungsfälle

1. **Projektfortschrittsaufzeichnung**
   ```
   Zusammenfassung: Benutzerauthentifizierungsmodul abgeschlossen
   Inhalt: JWT-Token-Verifizierung, OAuth2-Integration, Token-Aktualisierungsmechanismus implementiert
   Schlüsselwörter: Authentifizierung,JWT,OAuth2
   ```

2. **Lernnotizen**
   ```
   Zusammenfassung: Best Practices für asynchrone Programmierung in C# gelernt
   Inhalt: Hinweise zur Verwendung von async/await, Verwendungsszenarien von ConfigureAwait...
   Schlüsselwörter: C#,Asynchron,Best Practices
   ```

3. **Besprechungsprotokolle**
   ```
   Zusammenfassung: Produktaanforderungsbesprechung
   Inhalt: Neue Funktionsanforderungen besprochen, Implementierungsplan festgelegt...
   Schlüsselwörter: Produkt,Anforderungen,Besprechung
   ```

### Über Werkzeuge verwenden

Das Being kann Arbeitsnotizen über das `work_note`-Werkzeug verwalten:

```json
// Notiz erstellen
{
  "action": "create",
  "summary": "Benutzerauthentifizierungsmodul abgeschlossen",
  "content": "## Implementierungsdetails\n\n- JWT-Token verwenden\n- OAuth2 unterstützen",
  "keywords": "Authentifizierung,JWT,OAuth2"
}

// Notiz lesen
{
  "action": "read",
  "page_number": 1
}

// Notizen suchen
{
  "action": "search",
  "keyword": "Authentifizierung",
  "max_results": 10
}
```

### Über die Web-UI verwalten

1. Navigieren Sie zu **Being-Verwaltung** → Being auswählen
2. Klicken Sie auf die Registerkarte **Arbeitsnotizen**
3. Notizen anzeigen, suchen und bearbeiten
4. Markdown-Vorschau wird unterstützt

## Wissensnetzwerk

### Übersicht

Das Wissensnetzwerk ist ein auf Tripelstruktur (Subjekt-Prädikat-Objekt) basierendes Wissensrepräsentations- und Managementsystem zur Speicherung und Verwaltung strukturierten Wissens.

### Kernkonzepte

#### Tripelstruktur

```
Subjekt (Subject) --Prädikat (Predicate)--> Objekt (Object)
```

**Beispiele**:
- `Python` --`is_a`--> `programming_language`
- `Peking` --`capital_of`--> `China`
- `Wasser` --`boiling_point`--> `100°C`

#### Konfidenz

Jedes Wissenstripel hat einen Konfidenzwert (0,0–1,0), der die Glaubwürdigkeit des Wissens angibt:
- `1.0`: Absolut sicher (z. B. mathematische Sätze)
- `0,8–0,99`: Hochgradig glaubwürdig (z. B. verifizierte Fakten)
- `0,5–0,79`: Mittelmäßig glaubwürdig (z. B. Schlussfolgerungen oder Hypothesen)
- `<0,5`: Geringe Glaubwürdigkeit (z. B. Vermutungen oder unverifizierte Informationen)

#### Tag-System

Unterstützung zum Hinzufügen von Tags zu Tripeln für Kategorisierung und Suche:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "tags": ["programming", "language", "popular"]
}
```

### Wissensoperationen

#### 1. Wissen hinzufügen

```json
{
  "action": "add",
  "subject": "C#",
  "predicate": "created_by",
  "object": "Microsoft",
  "confidence": 1.0,
  "tags": ["programming", "language"]
}
```

#### 2. Wissen abfragen

```json
{
  "action": "query",
  "subject": "C#",
  "predicate": "created_by"
}
```

#### 3. Wissen suchen

```json
{
  "action": "search",
  "query": "programming language",
  "limit": 10
}
```

#### 4. Wissenspfade entdecken

Zusammenhänge zwischen zwei Konzepten finden:
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

Rückgabe:
```
Python → is_a → programming_language → belongs_to → computer_science
```

#### 5. Wissensvalidierung

Gültigkeit und Konsistenz des Wissens prüfen:
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. Wissensstatistiken

Gesamtstatistiken des Wissensnetzwerks abrufen:
```json
{
  "action": "stats"
}
```

Rückgabe:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Anwendungsfälle

1. **Faktenspeicherung**
   - Objektive Fakten und Allgemeinwissen speichern
   - Beispiel: `Erde` --`is_a`--> `Planet`

2. **Konzeptbeziehungen**
   - Beziehungen zwischen Konzepten aufzeichnen
   - Beispiel: `Vererbung` --`is_a`--> `Objektorientiertes-Programmierkonzept`

3. **Lernakkumulation**
   - Das Being sammelt kontinuierlich Wissen durch Lernen
   - Bildet ein strukturiertes Wissenssystem

4. **Schlussfolgerungsunterstützung**
   - Indirekte Beziehungen durch Wissenspfade entdecken
   - Wissensbasierte Schlussfolgerungen und Entscheidungen unterstützen

### Über die Web-UI verwalten

1. Navigieren Sie zur Seite **Wissensnetzwerk**
2. Wissensstatistiken anzeigen
3. Wissen durchsuchen und navigieren
4. Visuelle Wissensbeziehungsgrafik (geplant)

## WebView-Browseroperationen (Neu)

### Übersicht

Silicon Beings können über das WebView-Browserwerkzeug selbstständig Webseiten durchsuchen, Informationen abrufen und Weboperationen ausführen. Der Browser läuft im Headless-Modus und ist für den Benutzer vollständig unsichtbar.

### Eigenschaften

- **Individuelle Isolierung**: Jedes Being hat eine eigene Browserinstanz, eigene Cookies und Sitzungen
- **Headless-Modus**: Autonome Hintergrundoperation, für den Benutzer unsichtbar
- **Vollständige Funktionalität**: Unterstützung für JavaScript-Ausführung, CSS-Rendering, Formularausfüllung usw.
- **Sicherheitskontrolle**: Alle Operationen müssen die Berechtigungsprüfungskette durchlaufen

### Häufige Operationen

#### 1. Browser öffnen

```json
{
  "action": "open"
}
```

#### 2. Zu einer Webseite navigieren

```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

#### 3. Seiteninhalt abrufen

```json
{
  "action": "get_page_text"
}
```

Gibt den Textinhalt der Seite zurück, damit die KI ihn analysieren und verstehen kann.

#### 4. Element anklicken

```json
{
  "action": "click",
  "selector": "#submit-button"
}
```

#### 5. Text eingeben

```json
{
  "action": "input",
  "selector": "#search-input",
  "text": "Suchbegriff"
}
```

#### 6. JavaScript ausführen

```json
{
  "action": "execute_script",
  "script": "return document.title;"
}
```

#### 7. Screenshot abrufen

```json
{
  "action": "get_screenshot"
}
```

Gibt einen Screenshot der Seite zurück (Base64-kodiert), der für die visuelle Analyse verwendet werden kann.

#### 8. Auf Element warten

```json
{
  "action": "wait_for_element",
  "selector": ".loading-complete",
  "timeout": 10000
}
```

### Anwendungsfälle

1. **Informationsbeschaffung**
   - Nachrichtenwebsites durchsuchen, um aktuelle Informationen zu erhalten
   - Dokumentationen und technische Unterlagen abfragen
   - Webseiteninhaltsänderungen überwachen

2. **Automatisierte Operationen**
   - Formulare ausfüllen und absenden
   - Schaltflächen klicken, um Operationen auszulösen
   - Webseitendaten extrahieren

3. **Webseitenanalyse**
   - Seitenstruktur und -inhalt analysieren
   - Spezifische Informationen extrahieren
   - Visuelle Screenshot-Analyse

### Hinweise

- Browseroperationen können langsam sein; auf vollständiges Laden der Seite warten
- `wait_for_element` verwenden, um sicherzustellen, dass ein Element vorhanden ist, bevor es bedient wird
- Die Nutzungsbedingungen und robots.txt der Website beachten
- Häufige Anfragen vermeiden, um Sperrungen zu verhindern

## Bewährte Verfahren

### Soul-Datei verfassen

1. **Konkret**: Klare Persönlichkeitsmerkmale und Grenzen definieren
2. **Umfang definieren**: Was das Being tun sollte und was nicht
3. **Beispiele einfügen**: Erwartete Verhaltensmuster aufzeigen
4. **Regelmäßig aktualisieren**: Die Soul basierend auf der Leistung weiterentwickeln

### Aufgabenverwaltung

1. **Prioritäten setzen**: Prioritätsskala verwenden (1–10)
2. **Fristen definieren**: Immer Fristen setzen
3. **Fortschritt überwachen**: Regelmäßig Aufgabenstatus prüfen
4. **Fehler behandeln**: Wiederholungslogik implementieren

### Speicheroptimierung

1. **Alte Daten bereinigen**: Regelmäßig alte Speicher archivieren
2. **Wichtige Informationen indizieren**: Schlüsselinformationen markieren
3. **Zeitspeicher nutzen**: Zeitindizierte Abfragen verwenden

### Speicherverblasungsmechanismus

Das System enthält einen integrierten `MemoryFadeService` mit zeitbasierter Zerfallssimulation, der das Vergessen biologischer Speicher nachahmt:

- **Automatischer Zerfall**: Stündlich wird der Wichtigkeitszerfallsalgorithmus auf alle Speichereinträge aller Silicon Beings angewendet
- **Automatische Archivierung**: Speicher mit einer Wichtigkeit unter dem Schwellenwert werden automatisch archiviert und nehmen nicht mehr an der täglichen Suche teil
- **Statistikverfolgung**: Anzahl der Zerfallszyklen und der Statusänderungseinträge wird aufgezeichnet

Dies bedeutet, dass sich die Speicher von Silicon Beings mit der Zeit natürlich verblasen. Wichtige Informationen müssen aktiv über das Speicherwerkzeug als hohe Wichtigkeit markiert werden, um eine automatische Archivierung zu vermeiden.

---

## Projektarbeitsbereich

### Übersicht

Der Projektarbeitsbereich ist ein Raumverwaltungsmechanismus, der die Zusammenarbeit mehrerer Silicon Beings unterstützt. Der Silicon Curator kann Projekträume erstellen, Silicon Beings Projekten zuweisen und ihnen Rollen zuordnen.

### Projektlebenszyklus

```
Erstellen → Aktiv → Archiviert → Vernichtet
              ↑           |
              └── Wiederherstellen ┘
```

### Projektrollen

Silicon Beings können in einem Projekt bestimmte Rollen zugewiesen bekommen:

```json
{
  "action": "assign_role",
  "project_id": "project-uuid",
  "being_id": "being-uuid",
  "role_name": "developer"
}
```

### Projektarbeitsnotizen

Arbeitsnotizen im Projektraum sind öffentlich; alle Projektmitglieder haben Zugriff:

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "Benutzerauthentifizierungsmodul abgeschlossen",
  "content": "## Implementierungsdetails\n\n- JWT-Token verwenden",
  "keywords": "Authentifizierung,JWT"
}
```

### Projektaufgaben

Aufgaben im Projektraum unterstützen das vollständige Lebenszyklusmanagement:

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "title": "Benutzerauthentifizierung implementieren",
  "priority": 5
}
```

### Projekt-Workflows

Projekte können Workflow-Vorlagen binden, die die Zusammenarbeitsprozesse der Silicon Beings steuern:

- Workflows basieren auf Zustandsmaschinenvorlagen
- Unterstützung für Tick-gesteuerte Zustandsübergänge
- Automatische Protokollierung von Zustandsübergängen

### Werkzeugberechtigungsisolierung

Werkzeugberechtigungen auf Projektebene sind unabhängig von den Berechtigungen auf Silicon-Being-Ebene und ermöglichen so eine Berechtigungsisolierung zwischen Projekten. Beispielsweise kann ein Silicon Being in Projekt A Netzwerkzugriff haben, in Projekt B jedoch auf Nur-Lese-Berechtigungen beschränkt sein.

## Fehlerbehebung

### Being lässt sich nicht starten

**Prüfen**:
- Soul-Datei vorhanden und gültig
- KI-Client konfiguriert
- Systemressourcen ausreichend

### Being stoppt unerwartet

**Prüfen**:
- Fehler in den Protokollen
- KI-Dienstverfügbarkeit
- Speicherverbrauch

### Aufgabe wird nicht ausgeführt

**Prüfen**:
- Timer-System läuft
- Aufgabenpriorität und -planung
- Berechtigungseinstellungen

## Nächste Schritte

- 📚 [Architekturleitfaden](architecture.md) lesen
- 🛠️ [Entwicklungsleitfaden](development-guide.md) ansehen
- 🚀 [Schnellstartleitfaden](getting-started.md) ansehen
