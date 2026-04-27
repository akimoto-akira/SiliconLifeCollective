# Silicon-Being-Leitfaden

> **Version: v0.1.0-alpha**

[English](../en/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md)

## Übersicht

Silicon Beings sind KI-gesteuerte Agenten, die autonom denken, handeln und sich entwickeln können.

## Architektur

### Body-Brain-Trennung

```
┌─────────────────────────────────────┐
│         Silicon Being                │
├──────────────────┬──────────────────┤
│   Body           │   Brain          │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • Statusverwaltung │ • Verlauf laden │
│ • Trigger-Erkennung│ • KI aufrufen   │
│ • Lebenszyklus     │ • Tools ausführen│
│                    │ • Antwort speichern│
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

## Being erstellen

### Über Web-UI

1. Zu **Being-Verwaltung** navigieren
2. Auf **Neues Being erstellen** klicken
3. Ausfüllen:
   - Name
   - Soul-Inhalt
   - Konfigurationsoptionen
4. Auf **Erstellen** klicken

### Über API

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistant",
    "soul": "# Personality\nYou are helpful..."
  }'
```

## Being-Lebenszyklus

### Status

```
Created → Starting → Running → Stopping → Stopped
                    ↓
                  Error
```

### Operationen

- **Start**: Initialisieren und mit Verarbeitung beginnen
- **Stopp**: Graceful Shutdown
- **Pause**: Temporär suspendieren (Status beibehalten)
- **Fortsetzen**: Von Pause-Status fortfahren

## Aufgaben-System

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
- `Completed` - Erfolgreich abgeschlossen
- `Failed` - Ausführung fehlgeschlagen
- `Cancelled` - Manuell abgebrochen

## Timer-System

### Timer-Typen

1. **Einmalig**: Einmalige Ausführung nach Verzögerung
2. **Intervall**: Wiederholung in festen Intervallen
3. **Cron**: Ausführung basierend auf Cron-Ausdruck

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

- **Kurzfristig**: Aktueller Konversationskontext
- **Langfristig**: Persistiertes Wissen und Erfahrungen
- **Episodisch**: Zeitindexierte Ereignisse und Interaktionen

### Speicherstruktur

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

## Arbeitsnotizen-System

### Übersicht

Arbeitsnotizen sind ein persönliches Diary-System von Silicon Beings mit seitenbasiertem Design zur Aufzeichnung von Arbeitsfortschritt, Lernerfahrungen, Projektnotizen etc.

### Features

- **Seitenverwaltung**: Jede Notiz ist eine eigene Seite, Zugriff über Seitennummer
- **Markdown-Unterstützung**: Inhalt unterstützt Markdown-Format (Text, Listen, Tabellen, Code-Blöcke)
- **Schlüsselwort-Index**: Notizen können mit Schlüsselwörtern versehen werden für Suche
- **Zusammenfassung**: Jede Notiz hat kurze Zusammenfassung für schnelles Browsen
- **Verzeichnisgenerierung**: Kann Verzeichnis aller Notizen generieren für Gesamtüberblick
- **Zeitstempel**: Automatische Aufzeichnung von Erstellungs- und Update-Zeit
- **Standardmäßig privat**: Nur Being selbst hat Zugriff (Curator kann verwalten)

### Anwendungsszenarien

1. **Projektfortschritt dokumentieren**
   ```
   Zusammenfassung: Benutzerauthentifizierungsmodul abgeschlossen
   Inhalt: JWT-Token-Verifizierung, OAuth2-Integration, Refresh-Token-Mechanismus implementiert
   Schlüsselwörter: Authentifizierung,JWT,OAuth2
   ```

2. **Lernnotizen**
   ```
   Zusammenfassung: C# Async-Programming Best Practices gelernt
   Inhalt: Hinweise zu async/await, Use-Cases für ConfigureAwait...
   Schlüsselwörter: C#,Async,Best Practices
   ```

3. **Meeting-Protokolle**
   ```
   Zusammenfassung: Produktanforderungsbesprechung
   Inhalt: Neue Feature-Anforderungen diskutiert, Implementierungsansatz festgelegt...
   Schlüsselwörter: Produkt,Anforderungen,Meeting
   ```

### Über Tool verwenden

Beings können Arbeitsnotizen über das `work_note`-Tool verwalten:

```json
// Notiz erstellen
{
  "action": "create",
  "summary": "完成用户认证模块",
  "content": "## 实现细节\n\n- 使用 JWT token\n- 支持 OAuth2",
  "keywords": "认证,JWT,OAuth2"
}

// Notiz lesen
{
  "action": "read",
  "page_number": 1
}

// Notizen suchen
{
  "action": "search",
  "keyword": "认证",
  "max_results": 10
}
```

### Über Web-UI verwalten

1. Zu **Being-Verwaltung** → Being auswählen navigieren
2. Auf **Arbeitsnotizen**-Tab klicken
3. Notizen anzeigen, suchen, bearbeiten
4. Markdown-Vorschau unterstützt

---

## Wissensnetzwerk-System

### Übersicht

Das Wissensnetzwerk ist ein Wissensrepräsentations- und Managementsystem basierend auf Tripel-Struktur (Subjekt-Prädikat-Objekt) zur Speicherung und Verwaltung von strukturiertem Wissen.

### Kernkonzepte

#### Tripel-Struktur

```
Subjekt (Subject) --Prädikat (Predicate)--> Objekt (Object)
```

**Beispiele**:
- `Python` --`is_a`--> `programming_language`
- `北京` --`capital_of`--> `中国`
- `水` --`boiling_point`--> `100°C`

#### Konfidenzwert

Jedes Wissens-Tripel hat einen Konfidenzwert (0.0-1.0), der die Vertrauenswürdigkeit des Wissens angibt:
- `1.0`: Absolut sicher (wie mathematische Theoreme)
- `0.8-0.99`: Hoch vertrauenswürdig (wie verifizierte Fakten)
- `0.5-0.79`: Mittlere Vertrauenswürdigkeit (wie Inferenzen oder Hypothesen)
- `<0.5`: Niedrige Vertrauenswürdigkeit (wie Vermutungen oder nicht verifizierte Informationen)

#### Tag-System

Unterstützt das Hinzufügen von Tags zu Tripeln für Klassifizierung und Suche:
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

Findet Verbindungspfade zwischen zwei Konzepten:
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

#### 5. Wissen validieren

Prüft Gültigkeit und Konsistenz des Wissens:
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. Wissensstatistiken

Erhält Gesamtstatistiken des Wissensnetzwerks:
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

### Anwendungsszenarien

1. **Faktenspeicherung**
   - Objektive Fakten und Allgemeinwissen speichern
   - Beispiel: `地球` --`is_a`--> `行星`

2. **Konzeptbeziehungen**
   - Beziehungen zwischen Konzepten aufzeichnen
   - Beispiel: `继承` --`is_a`--> `面向对象编程概念`

3. **Lernakkumulation**
   - Beings akkumulieren kontinuierlich Wissen durch Lernen
   - Bilden strukturierte Wissenssysteme

4. **Inferenzunterstützung**
   - Indirekte Beziehungen durch Wissenspfade entdecken
   - Unterstützt wissensbasierte Inferenz und Entscheidungsfindung

### Über Web-UI verwalten

1. Zu **Wissensnetzwerk**-Seite navigieren
2. Wissensstatistiken anzeigen
3. Wissen suchen und browsen
4. Wissensbeziehungsdiagramm visualisieren (geplant)

---

## WebView-Browseroperationen (Neu)

### Übersicht

Silicon Beings können über das WebView-Browser-Tool autonom Webseiten browsen, Informationen abrufen und Weboperationen ausführen. Der Browser läuft im Headless-Modus, für Benutzer vollständig unsichtbar.

### Features

- **Individuelle Isolation**: Jedes Being hat eigene Browser-Instanz, Cookies und Sitzungen
- **Headless-Modus**: Autonome Operation im Hintergrund, für Benutzer unsichtbar
- **Vollständige Funktionalität**: Unterstützt JavaScript-Ausführung, CSS-Rendering, Formularausfüllung etc.
- **Sicherheitskontrolle**: Alle Operationen müssen Berechtigungskette durchlaufen

### Häufige Operationen

#### 1. Browser öffnen

```json
{
  "action": "open_browser"
}
```

#### 2. Zu Webseite navigieren

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

Rückgabe: Seiten-Textinhalt zur KI-Analyse und -Verständnis.

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
  "text": "搜索关键词"
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

Rückgabe: Seiten-Screenshot (Base64-codiert), kann für visuelle Analyse verwendet werden.

#### 8. Auf Element warten

```json
{
  "action": "wait_for_element",
  "selector": ".loading-complete",
  "timeout": 10000
}
```

### Anwendungsszenarien

1. **Informationsbeschaffung**
   - News-Webseiten browsen für aktuelle Informationen
   - Dokumentation und technische Materialien nachschlagen
   - Webseiten-Inhaltsänderungen überwachen

2. **Automatisierte Operationen**
   - Formulare ausfüllen und absenden
   - Buttons klicken für Operationen
   - Web-Daten scrapen

3. **Webanalyse**
   - Seitenstruktur und -inhalt analysieren
   - Spezifische Informationen extrahieren
   - Visuelle Screenshot-Analyse

### Hinweise

- Browser-Operationen können langsamer sein, auf Seitenladung warten
- `wait_for_element` verwenden um sicherzustellen, dass Element vorhanden ist
- Nutzungsbedingungen und robots.txt der Webseiten beachten
- Häufige Anfragen vermeiden um Bans zu verhindern

---

## Best Practices

### Soul-Datei schreiben

1. **Konkret**: Klare Persönlichkeitsmerkmale und Grenzen
2. **Scope definieren**: Was Being tun soll und nicht tun soll
3. **Beispiele einfügen**: Erwartete Verhaltensmuster zeigen
4. **Regelmäßig aktualisieren**: Soul basierend auf Performance entwickeln

### Aufgabenverwaltung

1. **Prioritäten setzen**: Priorität (1-10) verwenden
2. **Deadlines definieren**: Immer Deadline setzen
3. **Fortschritt überwachen**: Regelmäßig Aufgabenstatus prüfen
4. **Fehler behandeln**: Retry-Logik implementieren

### Speicheroptimierung

1. **Alte Daten bereinigen**: Regelmäßig alte Speicher archivieren
2. **Wichtige Informationen indexieren**: Schlüsselinformationen markieren
3. **Time-Storage verwenden**: Zeitindex-Abfragen nutzen

## Fehlerbehebung

### Being startet nicht

**Prüfen**:
- Soul-Datei existiert und ist valide
- KI-Client konfiguriert
- Ausreichend Systemressourcen

### Being stoppt unerwartet

**Prüfen**:
- Fehler in Protokollen
- KI-Service-Verfügbarkeit
- Speichernutzung

### Aufgaben werden nicht ausgeführt

**Prüfen**:
- Timer-System läuft
- Aufgabenpriorität und -planung
- Berechtigungseinstellungen

## Nächste Schritte

- 📚 [Architekturleitfaden](architecture.md) lesen
- 🛠️ [Entwicklungsleitfaden](development-guide.md) prüfen
- 🚀 [Schnellstart-Leitfaden](getting-started.md) ansehen
