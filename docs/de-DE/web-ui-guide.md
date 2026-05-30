# Web-UI-Leitfaden

> **Version: v0.2.0-alpha**

[English](../en/web-ui-guide.md) | **Deutsch** | [中文](../zh-CN/web-ui-guide.md) | [繁體中文](../zh-HK/web-ui-guide.md) | [Español](../es-ES/web-ui-guide.md) | [日本語](../ja-JP/web-ui-guide.md) | [한국어](../ko-KR/web-ui-guide.md) | [Čeština](../cs-CZ/web-ui-guide.md) | [Русский](../ru-RU/web-ui-guide.md)

## Übersicht

Die Web-UI bietet eine umfassende Oberfläche zur Verwaltung von Silicon Beings, Überwachung des Systemstatus und Interaktion mit KI-Agenten. Das System verwendet eine reine Server-Side-Rendering-Architektur ohne Frontend-Framework-Abhängigkeiten, generiert HTML, CSS und JavaScript durch die Builder `H`, `CssBuilder` und `JsBuilder`.

## Zugriff

Standard-URL: `http://localhost:8080`

## Navigation

### Hauptbereiche

1. **Dashboard** - Systemübersicht und Kennzahlen
2. **Beings** - Silicon Beings verwalten
3. **Chat** - Mit Beings interagieren (Datei-Upload, Echtzeit-SSE unterstützt)
4. **Chat-Historie** - Chat-Historie der Silicon Beings einsehen (Sitzungsliste, Nachrichtendetails)
5. **Aufgaben** - Aufgabenverwaltung (persönliche Aufgaben)
6. **Timer** - Timer-Konfiguration (erstellen, anhalten, Ausführungshistorie)
7. **Konfiguration** - Systemeinstellungen (KI-Clients, Lokalisierung)
8. **Berechtigungen** - Zugriffssteuerung (ACL-Verwaltung, Berechtigungsabfrage)
9. **Protokolle** - Systemprotokolle (Filterung nach Stufe, Zeitbereichsabfrage)
10. **Audit** - Token-Nutzung und Audit-Trail
11. **Speicher** - Being-Speicher (Zeitleistenansicht, erweiterte Filterung)
12. **Wissen** - Wissensdatenbank (Tripel-Verwaltung, Pfadfindung)
13. **Code-Browser** - Code-Exploration (Dateibaum, Syntaxhervorhebung)
14. **Code-Editor** - Code-Bearbeitung mit Hover-Tooltips (Monaco Editor)
15. **Projekte** - Projektverwaltung (Arbeitsbereiche, Aufgaben, Arbeitsnotizen)
16. **Executoren** - Executor-Verwaltung (Disk, Netzwerk, Kommandozeile)
17. **Hilfe** - Hilfedokumentationssystem (Mehrsprachigkeitsunterstützung, Themensuche)
18. **Über** - Systeminformationen und Version

---

## Dashboard

### Funktionen

- Systemleistungskennzahlen (CPU, Speicher, Laufzeit)
- Being-Statusübersicht
- KI-Nutzungsstatistiken
- Schnellaktionen

### Echtzeit-Updates

Echtzeitdaten über SSE (Server-Sent Events) abrufen:

```javascript
const dashboard = new EventSource('/api/dashboard/events');
dashboard.onmessage = (event) => {
    const data = JSON.parse(event.data);
    updateMetrics(data);
};
```

---

## Being-Verwaltung

### Being-Liste

Zeigt alle Beings mit:
- Name und ID
- Aktuellem Status (Läuft/Gestoppt/Fehler)
- Soul-Datei-Link
- Schnellaktionen (Starten/Stoppen/Konfigurieren)

### Being-Details

- Vollständige Konfiguration
- Soul-Datei-Editor
- Aufgabenhistorie
- Speicher-Betrachter
- Leistungskennzahlen

### Being erstellen

1. Auf **Neues Being erstellen** klicken
2. Ausfüllen:
   - Name
   - Soul-Inhalt (Markdown-Editor)
   - Anfangskonfiguration
3. Auf **Erstellen** klicken

---

## Chat-Oberfläche

### Funktionen

- Echtzeit-Nachrichtenstrom
- Nachrichtenverlauf
- Mehrere Sitzungen unterstützt
- Werkzeugaufruf-Visualisierung

### Chat verwenden

1. Being auswählen
2. Nachricht eingeben
3. Streaming-Antwort anzeigen
4. Werkzeugausführung in Echtzeit verfolgen

### Werkzeugaufruf-Anzeige

Wenn die KI ein Werkzeug aufruft:
```
🔧 Werkzeug: calendar
📥 Eingabe: {"date": "2026-04-20"}
📤 Ausgabe: "Dritter Tag des vierten Mondmonats"
```

---

## Konfiguration

### KI-Clients

KI-Backends konfigurieren:
- Ollama (lokal)
- Bailian (Cloud)
- Volcano Engine Ark (Cloud)
- Herdsman (lokal/Cloud, keine Authentifizierung)
- Meituan LongCat (Cloud)
- Qiniu Cloud AI (Cloud)
- Benutzerdefinierter Client

### Speichereinstellungen

- Default-Version: Basispfad, Zeitindex, Bereinigungsrichtlinie
- Fast-Version: SpeedyPack-Speicher-Engine-Konfiguration, .spk-Dateiverwaltung, automatische Komprimierungseinstellungen

### Lokalisierung

Umschalten zwischen 34 Sprachvarianten:
- Chinesisch (6 Varianten): Vereinfachtes Chinesisch, Traditionelles Chinesisch, Singapur-Chinesisch, Macau-Chinesisch, Taiwan-Chinesisch, Malaysia-Chinesisch
- Englisch (10 Varianten): Amerikanisch, Britisch, Kanadisch, Australisch, Indisch, Singapur, Südafrika, Irisch, Neuseeland, Malaysia
- Spanisch (2 Varianten): Spanien, Mexiko
- Deutsch (5 Varianten): Deutschland, Österreich, Schweiz, Luxemburg, Liechtenstein
- Französisch (3 Varianten): Frankreich, Kanada, Schweiz
- Japanisch, Koreanisch, Tschechisch
- Russisch, Portugiesisch (2 Varianten), Italienisch, Niederländisch, Polnisch, Schwedisch

---

## Skin-System

### Verfügbare Skins

1. **Admin** - Professionelle Verwaltungsoberfläche
2. **Chat** - Chat-zentriertes Design
3. **Creative** - Kreativer und künstlerischer Stil
4. **Dev** - Entwicklerorientiertes Layout
5. **HighContrast** - Hochkontrast-Theme (Fast-Version)
6. **Minimal** - Minimalistischer Stil (Fast-Version)
7. **Light** - Helles Theme (Fast-Version)

### Skin wechseln

1. Auf **Einstellungen** (Zahnrad-Symbol) klicken
2. **Skin** auswählen
3. Gewünschten Skin wählen
4. Oberfläche aktualisiert sich sofort

### Benutzerdefinierte Skins

Benutzerdefinierte Skins durch Implementierung von `ISkin` erstellen:

```csharp
public class MySkin : ISkin
{
    public string Name => "MySkin";
    
    public string GetCss()
    {
        return ":root { --primary: #color; }";
    }
}
```

---

## Berechtigungsverwaltung

### Berechtigungen einsehen

- Alle Berechtigungsregeln auflisten
- Nach Benutzer oder Ressource filtern
- Ablaufdaten einsehen

### Berechtigungsregel hinzufügen

1. Auf **Regel hinzufügen** klicken
2. Konfigurieren:
   - Berechtigungstyp (z. B. `FileAccess`, `NetworkAccess`)
   - Ressourcenpräfix (z. B. `C:\Projects`, `api.github.com`)
   - Erlauben/Verweigern
   - Beschreibung
3. Speichern

### Audit-Trail

Alle Berechtigungsentscheidungen einsehen:
- Zeitstempel
- Benutzer
- Ressource
- Entscheidung
- Begründung

### Werkzeug-Berechtigungsverwaltung

Werkzeug-Operationsberechtigungen für Silicon Beings und Projekte verwalten:

1. **Silicon Being Werkzeugberechtigungen**:
   - Zu **Beings** navigieren → Being auswählen → **Werkzeugberechtigungen**
   - Aktuelle Berechtigungskonfiguration einsehen
   - Pro Operation Erlauben/Verweigern festlegen
   - Berechtigungsvorlagen anwenden (readonly/restricted/full)

2. **Projekt-Werkzeugberechtigungen**:
   - Zu **Projekte** navigieren → Projekt auswählen → **Werkzeugberechtigungen**
   - Projektweite Werkzeugberechtigungen sind unabhängig von Being-Ebene
   - Berechtigungsisolierung zwischen Projekten umsetzen

---

## Aufgabenverwaltung

### Aufgabenliste

- Alle Aufgaben mit Status
- Nach Being oder Status filtern
- Prioritätsindikatoren

### Aufgabendetails

- Beschreibung
- Priorität
- Fälligkeitsdatum
- Ausführungshistorie
- Ergebnisausgabe

### Aufgabe erstellen

1. Auf **Aufgabe erstellen** klicken
2. Ausfüllen:
   - Being-Zuweisung
   - Beschreibung
   - Priorität (1-10)
   - Fälligkeitsdatum
3. Erstellen

---

## Timer-Verwaltung

### Aktive Timer

- Liste laufender Timer
- Nächste Ausführungszeit
- Wiederholungsstatus

### Timer erstellen

1. Auf **Timer erstellen** klicken
2. Konfigurieren:
   - Being-Zuweisung
   - Intervall oder Cron-Ausdruck
   - Auszuführende Aktion
   - Wiederholungseinstellungen
3. Starten

---

## Protokoll-Betrachter

### Funktionen

- Nach Stufe filtern (Info/Warnung/Fehler)
- Nach Schlüsselwort suchen
- Zeitbereichsauswahl
- Echtzeit-Updates

### Protokolldetails

Jeder Protokolleintrag zeigt:
- Zeitstempel
- Stufe
- Quelle
- Nachricht
- Stack-Trace (bei Fehlern)

---

## Audit-Berichte

### Token-Nutzung

- Verwendete Gesamt-Token
- Aufschlüsselung nach Modell
- Kostenberechnung
- Zeitbasierte Diagramme

### Bericht exportieren

Audit-Daten herunterladen:
- CSV-Format
- Datumsbereichsauswahl
- Nach Being oder Modell filtern

---

## Code-Editor

### Funktionen

- Syntaxhervorhebung (Monaco Editor)
- Code-Vervollständigung
- Hover-Tooltips für Bezeichner
- Echtzeit-Kompilierung

### Hover-Tooltips

Maus über einen beliebigen Bezeichner bewegen, um zu sehen:
- Typinformationen
- Dokumentation
- Definitionsposition
- Verweise

---

## Chat-Historie-Anzeige

### Funktionen

- Chat-Historie der Silicon Beings durchsuchen
- Sitzungsliste anzeigen
- Nachrichtendetails einsehen
- Zeitleistenansicht

### Chat-Historie verwenden

1. Zur **Beings**-Seite navigieren
2. Auf den **Chat-Historie**-Link des Silicon Beings klicken
3. Sitzungsliste einsehen:
   - Sitzungstitel
   - Erstellungszeit
   - Nachrichtenanzahl
4. Auf Sitzung klicken, um Details zu sehen:
   - Vollständige Nachrichtenhistorie
   - Zeitstempel
   - Absenderinformationen
   - Werkzeugaufruf-Datensätze

### Technische Implementierung

- **Controller**: `ChatHistoryController`
- **View-Modell**: `ChatHistoryViewModel`
- **Views**:
  - `ChatHistoryListView` - Sitzungsliste
  - `ChatHistoryDetailView` - Nachrichtendetails
- **API-Routen**:
  - `/api/chat-history/{beingId}/conversations` - Sitzungsliste abrufen
  - `/api/chat-history/{beingId}/conversation/{conversationId}` - Nachrichtendetails abrufen

---

## Datei-Upload

### Funktionen

- Dateiquellen-Dialog
- Multi-Datei-Upload unterstützt
- Dateimetadaten-Verwaltung
- Upload-Fortschrittsanzeige

### Datei-Upload verwenden

1. In der Chat-Oberfläche auf **Datei hochladen** klicken
2. Dateiquellen-Dialog öffnet sich
3. Dateiquelle auswählen:
   - Lokale Datei
   - Dateisystempfad
4. Datei(en) auswählen (Mehrfachauswahl unterstützt)
5. Upload bestätigen
6. Dateiinformationen werden an die Nachricht angehängt

### Unterstützte Dateitypen

- Textdateien (.txt, .md, .json, .xml, usw.)
- Codedateien (.cs, .js, .py, .java, usw.)
- Konfigurationsdateien (.yml, .yaml, .ini, .conf, usw.)
- Dokumentdateien (.csv, .log, usw.)

---

## Ladeanzeige

### Funktionen

- Ladezustandsanzeige auf der Chat-Seite
- Automatische Curator-Sitzungsauswahl
- Datenlade-Fortschrittsrückmeldung

### Verhalten

- Ladeanimation beim Laden der Seite anzeigen
- Automatisch ausblenden, wenn Daten geladen sind
- Curator-Sitzung automatisch auswählen (falls vorhanden)
- Mehrsprachige Ladetipp-Texte

---

## Hilfedokumentationssystem (Neu)

### Funktionsübersicht

Das Hilfedokumentationssystem bietet mehrsprachige Hilfedokumentation für Silicon Beings und Benutzer.

### Hilfedokumentation verwenden

1. Zur **Hilfe**-Seite navigieren
2. Hilfethemen-Liste einsehen:
   - Schnellstart-Leitfaden
   - Werkzeug-Referenz
   - Berechtigungsverwaltungs-Leitfaden
   - Fehlerbehebungs-Handbuch
   - Entwicklungsleitfaden
3. Auf Thema klicken, um detaillierten Inhalt zu sehen:
   - Strukturierte Dokumentation (Markdown-gerendert)
   - Mehrsprachige Unterstützung (folgt System-Lokalisierungseinstellungen)
   - Verwandte Themen-Empfehlungen
4. Suchfunktion zur schnellen Lokalisierung verwenden:
   - Schlüsselwortsuche (Chinesisch und Englisch unterstützt)
   - Suchergebnisse nach Relevanz sortiert

### Silicon Being Zugriff auf Hilfe

Silicon Beings können über das `help`-Werkzeug auf die Hilfedokumentation zugreifen:
```json
{
  "action": "get_topics"
}
```

### Technische Implementierung

- **Controller**: `HelpController`
- **Werkzeug**: `HelpTool`
- **API-Routen**:
  - `/api/help` - Hilfethemen-Liste abrufen
  - `/api/help/{topicId}` - Themendetails abrufen
  - `/api/help/search?q=keyword` - Hilfedokumentation durchsuchen

---

## Projektarbeitsbereich (Neu)

### Funktionsübersicht

Der Projektarbeitsbereich bietet eine strukturierte Arbeitsumgebung mit Unterstützung für Projektverwaltung, Aufgabenverfolgung und Arbeitsnotizen.

### Projektverwaltung

1. **Projekt erstellen**:
   - Projektname und Beschreibung
   - Projekt-Tags (Kategorisierung)
   - Projektstatus (In Bearbeitung, Abgeschlossen, Archiviert)
2. **Projektdetails einsehen**:
   - Projekt-Basisinformationen
   - Zugehörige Aufgabenliste
   - Arbeitsnotizen-Liste
   - Projektfortschrittsstatistiken
3. **Projekt archivieren**: Historische Daten behalten, aber nicht mehr aktiv
4. **Projektrollenverwaltung**:
   - Silicon Beings Projektrollen zuweisen (z. B. developer, reviewer, manager)
   - Rollenzuweisungen entfernen
   - Projektmitglieder und Rollenliste einsehen
5. **Projekt-Workflows**:
   - Workflow-Vorlagenliste einsehen
   - Workflow-Vorlage an Projekt binden
   - Workflow-Instanzstatus einsehen
   - Workflow-Ausführungsprotokolle einsehen

### Arbeitsnotizen (Privat)

Persönliche Arbeitsnotizen der Silicon Beings, ähnlich einem Tagebuch:

1. **Notiz erstellen**:
   - Zusammenfassung (Kurzbeschreibung)
   - Inhalt (Markdown-Format unterstützt)
   - Schlüsselwörter (für die Suche)
   - Automatische Zeitstempel-Erfassung
2. **Notizen verwalten**:
   - Zeitleistenbasiertes Durchblättern (seitenbasiertes Design)
   - Notizen durchsuchen (nach Schlüsselwort, Zusammenfassung, Inhalt)
   - Verzeichnis generieren (schnelle Übersicht der Notizstruktur)
   - Notizen aktualisieren und löschen
3. **Berechtigungssteuerung**:
   - Standardmäßig privat, nur das Being selbst hat Zugriff
   - Silicon Curator kann alle Notizen verwalten

### Technische Implementierung

- **Controller**: `WorkNoteController`
- **Werkzeuge**: `WorkNoteTool`, `ProjectTool`, `ProjectWorkNoteTool`
- **API-Routen**:
  - `/api/worknotes` - Arbeitsnotizen-Liste abrufen
  - `/api/worknotes/{id}` - Notizdetails abrufen
  - `/api/worknotes/search?q=keyword` - Notizen durchsuchen
  - `/api/worknotes/directory` - Notizverzeichnis generieren
  - `/api/projects` - Projektverwaltungs-API

---

## Responsives Design

Die Web-UI passt sich verschiedenen Bildschirmgrößen an:
- Desktop: Vollständiges Layout
- Tablet: Verkleinerte Seitenleiste
- Mobil: Einklappbares Menü

---

## Tastaturkürzel

| Kürzel | Aktion |
|----------|--------|
| `Ctrl+K` | Schnellsuche |
| `Ctrl+B` | Seitenleiste umschalten |
| `Ctrl+Enter` | Nachricht senden |
| `Esc` | Abbrechen/Schließen |

---

## Fehlerbehebung

### Verbindung nicht möglich

**Prüfen**:
- Server läuft
- Port 8080 wird nicht blockiert
- Firewall-Einstellungen

### SSE funktioniert nicht

**Prüfen**:
- Browser unterstützt SSE
- Kein Proxy puffert SSE
- Netzwerkstabilität

### Langsame Leistung

**Optimieren**:
- Protokollierungsdetaillierungsgrad verringern
- Alte Audit-Daten bereinigen
- Systemressourcen prüfen

---

## Nächste Schritte

- 📚 [Architektur-Leitfaden](architecture.md) lesen
- 🛠️ [Entwicklungsleitfaden](development-guide.md) ansehen
- 📖 [API-Referenz](api-reference.md) erkunden
- 🚀 [Schnellstart-Leitfaden](getting-started.md) ansehen
