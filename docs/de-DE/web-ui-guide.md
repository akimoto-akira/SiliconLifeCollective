# Web-UI-Leitfaden

> **Version: v0.1.0-alpha**

[English](../en/web-ui-guide.md) | [Deutsch](../de-DE/web-ui-guide.md) | [中文](../zh-CN/web-ui-guide.md) | [繁體中文](../zh-HK/web-ui-guide.md) | [Español](../es-ES/web-ui-guide.md) | [日本語](../ja-JP/web-ui-guide.md) | [한국어](../ko-KR/web-ui-guide.md) | [Čeština](../cs-CZ/web-ui-guide.md)

## Übersicht

Die Web-UI bietet eine umfassende Oberfläche zur Verwaltung von Silicon Beings, Überwachung des Systemstatus und Interaktion mit KI-Agenten. Das System verwendet eine reine Server-Side-Rendering-Architektur ohne Frontend-Framework-Abhängigkeiten, generiert HTML, CSS und JavaScript durch die Builder `H`, `CssBuilder` und `JsBuilder`.

## Zugriff

Standard-URL: `http://localhost:8080`

## Navigation

### Hauptbereiche

1. **Dashboard** - Systemübersicht und Metriken
2. **Beings** - Silicon Beings verwalten
3. **Chat** - Mit Beings interagieren (mit Datei-Upload, Live-SSE)
4. **Chat-Verlauf** - Chat-Historie von Silicon Beings anzeigen (Sitzungsliste, Nachrichtendetails)
5. **Aufgaben** - Aufgabenverwaltung (persönliche Aufgaben)
6. **Timer** - Timer-Konfiguration (Erstellen, Pausieren, Ausführungshistorie)
7. **Konfiguration** - Systemeinstellungen (KI-Clients, Lokalisierung)
8. **Berechtigungen** - Zugriffskontrolle (ACL-Verwaltung, Berechtigungsabfragen)
9. **Protokolle** - Systemprotokolle (nach Level filtern, Zeitbereichsabfragen)
10. **Audit** - Token-Nutzung und Audit-Trail
11. **Speicher** - Being-Speicher (Timeline-Ansicht, erweiterte Filterung)
12. **Wissen** - Wissensdatenbank (Tripelverwaltung, Pfadfindung)
13. **Code-Browser** - Code-Exploration (Dateibaum, Syntax-Hervorhebung)
14. **Code-Editor** - Code-Bearbeitung mit Hover-Tooltips (Monaco Editor)
15. **Projekte** - Projektverwaltung (Workspaces, Aufgaben, Arbeitsnotizen)
16. **Executoren** - Executor-Verwaltung (Festplatte, Netzwerk, Befehlszeile)
17. **Hilfe** - Hilfedokumentationssystem (mehrsprachige Unterstützung, Themensuche)
18. **Über** - Systeminformationen und Version

---

## Dashboard

### Funktionen

- Systemleistungsmetriken (CPU, Speicher, Betriebszeit)
- Being-Statusübersicht
- KI-Nutzungsstatistiken
- Schnellaktionen

### Echtzeit-Updates

Verwenden Sie SSE (Server-Sent Events) für Echtzeitdaten:

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
- Aktueller Status (Läuft/Gestoppt/Fehler)
- Soul-Datei-Link
- Schnellaktionen (Start/Stopp/Konfiguration)

### Being-Details

- Vollständige Konfiguration
- Soul-Datei-Editor
- Aufgabenhistorie
- Speicher-Viewer
- Leistungsmetriken

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

- Echtzeit-Nachrichtenstream
- Nachrichtenverlauf
- Multi-Sitzungs-Unterstützung
- Tool-Aufruf-Visualisierung

### Chat verwenden

1. Being auswählen
2. Nachricht eingeben
3. Gestreamte Antwort anzeigen
4. Tool-Ausführung in Echtzeit beobachten

### Tool-Aufruf-Anzeige

Wenn KI ein Tool aufruft:
```
🔧 Tool: calendar
📥 Eingabe: {"date": "2026-04-20"}
📤 Ausgabe: "农历四月初三"
```

---

## Konfiguration

### KI-Clients

KI-Backend konfigurieren:
- Ollama (lokal)
- DashScope (Cloud)
- Benutzerdefinierte Clients

### Storage-Einstellungen

- Basispfad
- Zeitindex
- Bereinigungsstrategie

### Lokalisierung

Zwischen 21 Sprachvarianten wechseln:
- Chinesisch (6): Simplified, Traditional, Singapore, Macau, Taiwan, Malaysia
- Englisch (10): US, UK, Canadian, Australian, Indian, Singapore, South African, Irish, New Zealand, Malaysia
- Spanisch (2): Spanien, Mexiko
- Japanisch, Koreanisch, Tschechisch

---

## Skin-System

### Verfügbare Skins

1. **Admin** - Professionelle Admin-Oberfläche
2. **Chat** - Konversationszentriertes Design
3. **Creative** - Kreativer und künstlerischer Stil
4. **Dev** - Entwicklerorientiertes Layout

### Skin wechseln

1. Auf **Einstellungen** (Zahnrad-Symbol) klicken
2. **Skin** auswählen
3. Gewünschten Skin wählen
4. Oberfläche aktualisiert sich sofort

### Benutzerdefinierter Skin

Erstellen Sie benutzerdefinierte Skins durch Implementierung von `ISkin`:

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

### Berechtigungen anzeigen

- Alle Berechtigungsregeln auflisten
- Nach Benutzer oder Ressource filtern
- Ablaufdatum anzeigen

### Berechtigungsregel hinzufügen

1. Auf **Regel hinzufügen** klicken
2. Konfigurieren:
   - Benutzer
   - Ressource (z.B. `disk:read`)
   - Erlauben/Verweigern
   - Dauer
3. Speichern

### Audit-Trail

Alle Berechtigungsentscheidungen anzeigen:
- Zeitstempel
- Benutzer
- Ressource
- Entscheidung
- Grund

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

## Protokoll-Viewer

### Funktionen

- Nach Level filtern (Info/Warnung/Fehler)
- Nach Schlüsselwort suchen
- Zeitbereichsauswahl
- Echtzeit-Updates

### Protokolldetails

Jeder Protokolleintrag zeigt:
- Zeitstempel
- Level
- Quelle
- Nachricht
- Stack-Trace (für Fehler)

---

## Audit-Berichte

### Token-Nutzung

- Insgesamt verwendete Tokens
- Aufschlüsselung nach Modell
- Kostenberechnung
- Zeitbasierte Diagramme

### Berichte exportieren

Audit-Daten herunterladen:
- CSV-Format
- Datumsbereichsauswahl
- Nach Being oder Modell filtern

---

## Code-Editor

### Funktionen

- Syntax-Hervorhebung (Monaco Editor)
- Code-Vervollständigung
- Hover-Tooltips für Bezeichner
- Echtzeit-Kompilierung

### Hover-Tooltips

Mauszeiger über Bezeichner bewegen für:
- Typinformationen
- Dokumentation
- Definitionsort
- Referenzen

---

## Chat-Verlauf-Anzeige

### Funktionen

- Silicon Being Chat-Historie durchsuchen
- Sitzungslistenanzeige
- Nachrichtendetails anzeigen
- Timeline-Ansicht

### Chat-Verlauf verwenden

1. Zu **Beings**-Seite navigieren
2. Auf **Chat-Verlauf**-Link des Silicon Being klicken
3. Sitzungsliste anzeigen:
   - Sitzungstitel
   - Erstellungszeit
   - Nachrichtenanzahl
4. Sitzung für Details anklicken:
   - Vollständige Nachrichtenhistorie
   - Zeitstempel
   - Senderinformationen
   - Tool-Aufrufprotokoll

### Technische Implementierung

- **Controller**: `ChatHistoryController`
- **ViewModel**: `ChatHistoryViewModel`
- **Views**:
  - `ChatHistoryListView` - Sitzungsliste
  - `ChatHistoryDetailView` - Nachrichtendetails
- **API-Routen**:
  - `/api/chat-history/{beingId}/conversations` - Sitzungsliste abrufen
  - `/api/chat-history/{beingId}/conversation/{conversationId}` - Nachrichtendetails abrufen

---

## Datei-Upload

### Funktionen

- Dateiquelldialog
- Multi-Datei-Upload-Unterstützung
- Datei-Metadatenverwaltung
- Upload-Fortschrittsanzeige

### Datei-Upload verwenden

1. Auf **Datei hochladen**-Button im Chat klicken
2. Dateiquelldialog öffnet sich
3. Dateiquelle wählen:
   - Lokale Datei
   - Dateisystempfad
4. Dateien auswählen (Mehrfachauswahl unterstützt)
5. Upload bestätigen
6. Dateiinformationen werden an Nachricht angehängt

### Unterstützte Dateitypen

- Textdateien (.txt, .md, .json, .xml, etc.)
- Codedateien (.cs, .js, .py, .java, etc.)
- Konfigurationsdateien (.yml, .yaml, .ini, .conf, etc.)
- Dokumentdateien (.csv, .log, etc.)

---

## Ladeindikator

### Funktionen

- Ladestatusanzeige auf Chat-Seite
- Automatische Curator-Sitzungsauswahl
- Datenladefortschrittsfeedback

### Verhalten

- Ladeanimation bei Seitenladung
- Automatisch ausgeblendet nach Datenladung
- Curator-Sitzung automatisch ausgewählt (wenn vorhanden)
- Mehrsprachige Ladetexte

---

## Hilfedokumentationssystem (Neu)

### Funktionsübersicht

Das Hilfedokumentationssystem bietet mehrsprachige Hilfeunterstützung für Silicon Beings und Benutzer.

### Hilfe verwenden

1. Zu **Hilfe**-Seite navigieren
2. Hilfe-Themenliste anzeigen:
   - Schnellstartanleitung
   - Tool-Nutzungsreferenz
   - Berechtigungsverwaltungsleitfaden
   - Fehlerbehebungshandbuch
   - Entwicklungsleitfaden
3. Thema für Details anklicken:
   - Strukturierter Dokumentinhalt (Markdown-Rendering)
   - Mehrsprachige Unterstützung (folgt Systemlokalisierung)
   - Verwandte Themenempfehlungen
4. Suchfunktion für schnelle Lokalisierung:
   - Schlüsselwortsuche (unterstützt Chinesisch, Englisch)
   - Suchergebnisse nach Relevanz sortiert

### Hilfe für Silicon Beings

Silicon Beings können über das `help`-Tool auf Hilfe zugreifen:
```json
{
  "action": "get_topics"
}
```

### Technische Implementierung

- **Controller**: `HelpController`
- **Tool**: `HelpTool`
- **API-Routen**:
  - `/api/help` - Hilfe-Themenliste abrufen
  - `/api/help/{topicId}` - Themandetails abrufen
  - `/api/help/search?q=keyword` - Hilfedokumentation suchen

---

## Projekt-Workspace (Neu)

### Funktionsübersicht

Der Projekt-Workspace bietet eine strukturierte Arbeitsumgebung mit Projektverwaltung, Aufgabenverfolgung und Arbeitsnotizen.

### Projektverwaltung

1. **Projekt erstellen**:
   - Projektname und Beschreibung
   - Projekt-Tags (Kategorisierung)
   - Projektstatus (Aktiv, Abgeschlossen, Archiviert)
2. **Projektdetails anzeigen**:
   - Projektgrundinformationen
   - Zugehörige Aufgabenliste
   - Arbeitsnotizenliste
   - Projektfortschrittsstatistik
3. **Projekt archivieren**: Historische Daten behalten, aber nicht mehr aktiv

### Arbeitsnotizen (Privat)

Persönliche Arbeitsnotizen von Silicon Beings, ähnlich einem Tagebuch:

1. **Notiz erstellen**:
   - Zusammenfassung (kurze Beschreibung)
   - Inhalt (Markdown-Unterstützung)
   - Schlüsselwörter (für Suche)
   - Automatischer Zeitstempel
2. **Notizen verwalten**:
   - Timeline-Browsing (Seitendesign)
   - Notizen suchen (nach Schlüsselwort, Zusammenfassung, Inhalt)
   - Inhaltsverzeichnis generieren (schnelle Strukturanzeige)
   - Notizen aktualisieren und löschen
3. **Berechtigungskontrolle**:
   - Standardmäßig privat, nur für Being selbst zugänglich
   - Silicon Curator kann alle Notizen verwalten

### Technische Implementierung

- **Controller**: `WorkNoteController`
- **Tools**: `WorkNoteTool`, `ProjectTool`, `ProjectWorkNoteTool`
- **API-Routen**:
  - `/api/worknotes` - Arbeitsnotizenliste abrufen
  - `/api/worknotes/{id}` - Notizdetails abrufen
  - `/api/worknotes/search?q=keyword` - Notizen suchen
  - `/api/worknotes/directory` - Notizverzeichnis generieren
  - `/api/projects` - Projektverwaltungs-API

---

## Responsives Design

Die Web-UI passt sich verschiedenen Bildschirmgrößen an:
- Desktop: Vollständiges Layout
- Tablet: Komprimierte Seitenleiste
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

### Keine Verbindung möglich

**Prüfen**:
- Server läuft
- Port 8080 nicht blockiert
- Firewall-Einstellungen

### SSE funktioniert nicht

**Prüfen**:
- Browser unterstützt SSE
- Kein Proxy buffered SSE
- Netzwerkstabilität

### Langsame Leistung

**Optimieren**:
- Protokolldetailgrad reduzieren
- Alte Audit-Daten bereinigen
- Systemressourcen prüfen

---

## Nächste Schritte

- 📚 [Architekturleitfaden](architecture.md) lesen
- 🛠️ [Entwicklungsleitfaden](development-guide.md) prüfen
- 📖 [API-Referenz](api-reference.md) erkunden
- 🚀 [Schnellstart-Leitfaden](getting-started.md) ansehen
