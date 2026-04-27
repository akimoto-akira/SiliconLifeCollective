// Copyright (c) 2026 Silicon Life Collective
// Licensed under the Apache License, Version 2.0

using SiliconLife.Collective;

namespace SiliconLife.Default.Help;

/// <summary>
/// German (Germany) help documentation implementation
/// </summary>
public class HelpLocalizationDeDE : HelpLocalizationBase
{
    #region Help Documents
    
    public override string GettingStarted_Title => "Schnellstart";
    public override string BeingManagement_Title => "Being-Verwaltung";
    public override string ChatSystem_Title => "Chat-System";
    public override string Dashboard_Title => "Dashboard";
    public override string Task_Title => "Aufgaben";
    public override string Timer_Title => "Timer";
    public override string Permission_Title => "Berechtigungsverwaltung";
    public override string Config_Title => "Konfigurationsverwaltung";
    public override string FAQ_Title => "Häufig gestellte Fragen";
    public override string Memory_Title => "Speichersystem";
    public override string OllamaSetup_Title => "Ollama-Installation und Modell-Download";
    public override string BailianDashScope_Title => "Leitfaden für die Alibaba Cloud Bailian-Plattform";
    public override string AIClients_Title => "KI-Client-Konfiguration";
    
    public override string BeingSoul_Title => "Soul-Datei";
    public override string AuditLog_Title => "Prüfprotokoll";
    public override string KnowledgeGraph_Title => "Wissensgraph";
    public override string WorkNotes_Title => "Arbeitsnotizen";
    public override string Projects_Title => "Projektmanagement";
    public override string Logging_Title => "Protokollierungssystem";

    public override string[] GettingStarted_Tags => new[] { "Installation", "Start", "Einrichtung", "Schnellstart", "Erste Schritte", "Beginnen", "Initialisierung", "Umgebung" };
    public override string[] BeingManagement_Tags => new[] { "Being", "Erstellen", "Einstellung", "Being-Verwaltung", "Silicon Being", "Profil", "Konfiguration", "Verwaltung" };
    public override string[] ChatSystem_Tags => new[] { "Chat", "Nachricht", "Konversation", "Chat-System", "Dialog", "Kommunikation", "Gespräch", "Diskussion" };
    public override string[] Dashboard_Tags => new[] { "Dashboard", "Monitor", "Statistik", "Status", "System", "Nachrichtenhäufigkeit", "Betriebszeit", "Speicher" };
    public override string[] Task_Tags => new[] { "aufgabe", "arbeit", "ausführung", "priorität", "abhängigkeit", "status", "automatisierung", "verwaltung" };
    public override string[] Timer_Tags => new[] { "timer", "zeitplan", "auslösen", "wiederholung", "kalender", "erinnerung", "automatisch", "cron" };
    public override string[] Permission_Tags => new[] { "Berechtigung", "Sicherheit", "Zugriffskontrolle", "Berechtigungsverwaltung", "Authentifizierung", "Autorisierung", "Datenschutz", "Schutz" };
    public override string[] Config_Tags => new[] { "Konfiguration", "Einstellung", "Option", "Konfigurationsverwaltung", "Präferenzen", "Anpassung", "System", "Parameter" };
    public override string[] FAQ_Tags => new[] { "FAQ", "Hilfe", "Problem", "Support", "Fehlerbehebung", "Leitfaden", "Unterstützung", "Antwort" };
    public override string[] Memory_Tags => new[] { "Speicher", "Verlauf", "Protokoll", "Speichersystem", "Aktivität", "Nachverfolgung", "Suche", "Log" };
    public override string[] OllamaSetup_Tags => new[] { "Ollama", "Installation", "Modell", "Download", "Lokale KI", "Konfiguration", "Einrichtung", "Ausführen" };
    public override string[] BailianDashScope_Tags => new[] { "Bailian", "DashScope", "Alibaba Cloud", "Cloud-KI", "API", "Konfiguration", "Modell", "Bezahlung" };
    public override string[] AIClients_Tags => new[] { "KI-Client", "KI-Dienst", "Modell", "Konfiguration", "Lokal", "Cloud", "Ollama", "DashScope", "Einrichtung" };
    
    public override string[] BeingSoul_Tags => new[] { "Soul-Datei", "Persönlichkeit", "Prompt", "Rolle", "Verhalten", "Konfiguration", "Charakter", "Arbeitsanleitung", "System-Prompt" };
    public override string[] AuditLog_Tags => new[] { "Prüfprotokoll", "Token", "Nutzungsstatistik", "Monitor", "Verbrauch", "Analyse", "Trend", "Export", "CSV" };
    public override string[] KnowledgeGraph_Tags => new[] { "Wissensgraph", "Wissen", "Visualisierung", "Tripel", "Entität", "Relation", "Netzwerk", "Lernen", "Verwaltung" };
    public override string[] WorkNotes_Tags => new[] { "Arbeitsnotizen", "Notizen", "Aufzeichnungen", "Tagebuch", "Protokoll", "Markdown", "Schlüsselwörter", "Version", "Suche" };
    public override string[] Projects_Tags => new[] { "Projektmanagement", "Projekt", "Zusammenarbeit", "Aufgabe", "Mitglied", "Archiv", "Team", "Arbeitsbereich", "Fortschritt" };
    public override string[] Logging_Tags => new[] { "Protokollierungssystem", "Protokoll", "Aufzeichnungen", "Debug", "Fehler", "Warnung", "Monitor", "Verfolgung", "Konsole", "Datei" };
    
    public override string GettingStarted => @"
# Schnellstart

## Systemstart

### Doppelklick-Start (Empfohlen)

Finden Sie die Programmdatei und doppelklicken Sie zum Starten:
- **Windows**: `SiliconLife.Default.exe`
- Das System startet automatisch und **öffnet automatisch den Browser**

So einfach ist das! Keine Konfiguration erforderlich.

## Erste Verwendung

Beim ersten Start führt das System **automatisch alle Initialisierungen durch**:
- ✅ Erstellt automatisch den Silicon Curator
- ✅ Verwendet die eingebaute Soul-Datei (Prompt)
- ✅ Speichert die Konfiguration automatisch
- ✅ Alle Dienste sind automatisch bereit

Sie müssen nur warten, bis der Browser geöffnet wird, und schon können Sie loslegen!

## Übersicht der Benutzeroberfläche

Die Systemoberfläche besteht aus zwei Hauptteilen:

### Linke Navigationsleiste

Enthält die folgenden Funktionsmodule:

- **💬 Chat** - Mit Silicon Beings kommunizieren
- **📊 Dashboard** - Systemstatus anzeigen
- **🧠 Beings** - Silicon Beings anzeigen und verwalten
- **🔍 Audit** - Betriebsprotokolle anzeigen
- **📚 Wissen** - Wissensgraph verwalten
- **📁 Projekte** - Codeprojekte verwalten
- **📝 Protokolle** - Systemprotokolle anzeigen
- **⚙ Konfiguration** - Systemeinstellungen
- **❓ Hilfe** - Dieses Dokument
- **ℹ Über** - Systeminformationen

### Hauptinhaltsbereich

Zeigt den Inhalt der aktuellen Seite an, der sich je nach ausgewähltem Funktionsmodul ändert.

## Schnellstart

### 1. Mit einem Silicon Being chatten

Dies ist die am häufigsten verwendete Funktion:

1. Klicken Sie auf das **💬 Chat**-Symbol links
2. Wählen Sie ein Silicon Being aus der linken Liste aus (standardmäßig ist der Silicon Curator verfügbar)
3. Geben Sie Ihre Nachricht in das Eingabefeld unten ein
4. Drücken Sie `Enter` zum Senden
5. Die KI antwortet Ihnen in Echtzeit

**Tipps:** 
- Drücken Sie `Shift + Enter` für einen Zeilenumbruch
- Klicken Sie auf die ⏹-Taste, um die KI-Antwort zu stoppen

### 2. Silicon Being-Informationen anzeigen

Möchten Sie detaillierte Informationen über ein Silicon Being sehen:

1. Klicken Sie auf das **🧠 Beings**-Symbol links
2. Klicken Sie auf eine beliebige Silicon Being-Karte
3. Detaillierte Informationen werden rechts angezeigt:
   - Status (Leerlauf/In Betrieb)
   - Anzahl der Timer und Aufgaben
   - Links zu Speicher, Berechtigungen, Chatverlauf usw.

### 3. Systemeinstellungen ändern

Müssen Sie die Systemkonfiguration anpassen:

1. Klicken Sie auf das **⚙ Konfiguration**-Symbol links
2. Finden Sie den Konfigurationseintrag, den Sie ändern möchten
3. Klicken Sie auf die ""Bearbeiten""-Taste
4. Geben Sie den neuen Wert ein und speichern Sie

**Häufige Einstellungen:**
- Schnittstellensprache ändern
- Design-Skin wechseln
- KI-Modell anpassen
- Zugriffsport ändern

## Datei-Upload

Lassen Sie die KI Dateiinhalte analysieren:

1. Klicken Sie im Chat-Interface auf die **📁**-Taste
2. Geben Sie den vollständigen Pfad der Datei ein
   - Zum Beispiel: `C:\Users\IhrBenutzername\Dokumente\Bericht.pdf`
3. Klicken Sie auf ""Upload bestätigen""
4. Die KI wird die Datei lesen und analysieren

**Unterstützte Dateitypen:**
- Textdateien: .txt, .md, .json
- Codedateien: .cs, .js, .py
- Konfigurationsdateien: .yml, .yaml
- Andere: .csv, .log usw.

## Chatverlauf anzeigen

Möchten Sie frühere Konversationen überprüfen:

1. Gehen Sie zur Seite **🧠 Beings**
2. Klicken Sie auf das Silicon Being, das Sie anzeigen möchten
3. Klicken Sie auf den Link ""Chatverlauf""
4. Durchsuchen Sie alle historischen Sitzungen

## Hilfe erhalten

Wenn Sie auf Probleme treffen:

- **Hilfe anzeigen**: Klicken Sie auf das **❓ Hilfe**-Symbol links
- **Protokolle anzeigen**: Klicken Sie auf das **📝 Protokolle**-Symbol links
- **System neustarten**: Viele Probleme können durch Neustart gelöst werden

## Nächste Schritte

Jetzt, da Sie die grundlegenden Operationen verstanden haben, können Sie:

- 📖 Andere Hilfedokumente lesen, um detaillierte Funktionen zu erfahren
- 💬 Mit dem Silicon Curator chatten und ihn bitten, Ihnen bei Aufgaben zu helfen
- ⚙ Konfigurationsoptionen erkunden, um Ihr System zu personalisieren

Viel Spaß bei der Verwendung!
";

    public override string BeingManagement => @"
# Being-Verwaltung

## Was ist ein Silicon Being?

Ein Silicon Being ist die Kernentität des Systems. Jedes Silicon Being ist ein unabhängiger KI-Agent mit:
- **Soul-Datei**: Kern-Prompt, der Verhaltensmuster, Persönlichkeit und Fähigkeiten definiert
- **Speichersystem**: Speichert Konversationsverlauf und wichtige Informationen
- **Aufgabensystem**: Führt geplante Aufgaben und automatisierte Operationen aus
- **Tool-Set**: Verschiedene aufrufbare Funktionstools

## Silicon Beings anzeigen

### Being-Liste

Das Öffnen der ""Beings""-Seite zeigt alle Silicon Beings im Kartenformat:
- **Name**: Der Anzeigename des Silicon Beings
- **Status**: Leerlauf (grün) oder In Betrieb (blau)
- **Typ**: Wenn benutzerdefinierter kompilierter Code geladen ist, wird ein Typ-Tag angezeigt

### Being-Details anzeigen

Klicken Sie auf eine beliebige Silicon Being-Karte, und detaillierte Informationen werden rechts angezeigt:
- **ID**: Eindeutiger Bezeichner des Silicon Beings
- **Status**: Aktueller Betriebsstatus
- **Benutzerdefinierte Kompilierung**: Ob benutzerdefinierter Code geladen ist
- **Timer-Anzahl**: Klicken Sie, um die Timer-Verwaltung anzuzeigen
- **Aufgabenanzahl**: Klicken Sie, um die Aufgabenliste anzuzeigen
- **Speicher**: Klicken Sie, um das Speichersystem anzuzeigen
- **Berechtigungen**: Klicken Sie, um die Berechtigungskonfiguration anzuzeigen
- **Chatverlauf**: Historische Konversationsprotokolle anzeigen
- **Arbeitsnotizen**: Arbeitsnotizen anzeigen
- **KI-Client**: Klicken Sie, um die KI-Konfiguration anzuzeigen und zu ändern
- **Soul-Datei**: Klicken Sie, um den Prompt anzuzeigen und zu bearbeiten

## Silicon Being bearbeiten

### Soul-Datei bearbeiten

Die Soul-Datei bestimmt die Verhaltensmuster und Fähigkeiten des Silicon Beings.

1. Klicken Sie auf den Link ""Soul-Datei"" in der Being-Detailseite
2. Öffnen Sie den Soul-Datei-Editor (unterstützt Markdown-Format)
3. Ändern Sie den Prompt-Inhalt
4. Änderungen speichern

### KI-Konfiguration bearbeiten

Sie können unabhängige KI-Dienste für jedes Silicon Being konfigurieren:

1. Klicken Sie auf den Link ""KI-Client"" in der Being-Detailseite
2. Wählen Sie den KI-Client-Typ (wie Ollama, OpenAI usw.)
3. Konfigurieren Sie API-Endpunkt, Modell, Schlüssel und andere Parameter
4. Nimmt sofort nach dem Speichern wirksam

## Leitfaden zum Schreiben von Soul-Dateien

### Grundstruktur

```markdown
# Rolleneinstellung

Sie sind eine [Rollenbeschreibung], spezialisiert auf:
- Fähigkeit 1
- Fähigkeit 2
- Fähigkeit 3

# Verhaltensrichtlinien

1. Richtlinie 1
2. Richtlinie 2
3. Richtlinie 3

# Arbeitsablauf

Bei Erhalt einer Aufgabe:
1. Anforderungen verstehen
2. Ansatz analysieren
3. Operationen ausführen
4. Ergebnisse melden
```

### Schreibratschläge

1. **Klare Rollendefinition**: Definieren Sie klar die Verantwortlichkeiten und Fachgebiete des Silicon Beings
2. **Verhaltensgrenzen setzen**: Erklären Sie, was getan werden kann und was nicht getan werden sollte
3. **Arbeitsabläufe bereitstellen**: Leiten Sie das Silicon Being an, wie es Aufgaben behandeln soll
4. **Markdown-Format verwenden**: Unterstützt Überschriften, Listen, Codeblöcke usw.

### Beispiel: Programmierassistent

```markdown
# Rolleneinstellung

Sie sind ein professioneller Full-Stack-Entwicklungsassistent, spezialisiert auf:
- C# / .NET-Entwicklung
- Architekturdesign und Code-Review
- Datenbankdesign und -optimierung
- Web-Frontend-Entwicklung

# Verhaltensrichtlinien

1. Stellen Sie immer ausführbare Codebeispiele bereit
2. Erklären Sie die Schlüsselcodelogik und das Designkonzept
3. Geben Sie Best-Practice-Empfehlungen
4. Bei Unsicherheit informieren Sie den Benutzer klar

# Code-Standards

- SOLID-Prinzipien befolgen
- Klare Namensgebung verwenden
- Notwendige Kommentare hinzufügen
- Fehlerbehandlung und Grenzfälle berücksichtigen
```

## Silicon Being-Status

### Betriebsstatus

- **Leerlauf**: Wartet auf Aufgaben oder Konversationen (grüne Anzeige)
- **In Betrieb**: Führt derzeit eine Aufgabe aus oder ist in Konversation (blaue Anzeige)

### Silicon Beings überwachen

Über das Dashboard können Sie Folgendes anzeigen:
- Gesamtzahl der aktuellen Silicon Beings
- Aufgabenausführungsstatus für jedes Being
- Ressourcennutzungsstatistiken

## Best Practices

1. **Aufgabentrennung**: Verschiedene Beings behandeln verschiedene Bereiche (z. B. Programmierassistent, Kundenservice-Assistent, Datenanalyse usw.)
2. **Kontinuierliche Optimierung**: Optimieren Sie Soul-Dateien kontinuierlich basierend auf tatsächlichem Nutzungsfeedback, um die Being-Leistung zu verbessern
3. **Konfiguration sichern**: Es wird empfohlen, Soul-Dateien wichtiger Beings zu sichern

## Fehlerbehebung

### F: Silicon Being reagiert nicht?

Überprüfen Sie:
1. Läuft der KI-Dienst normal?
2. Ist die Netzwerkverbindung normal?
3. Ist die Soul-Datei korrekt konfiguriert?
4. Überprüfen Sie die Systemprotokolle auf detaillierte Fehlerinformationen

### F: Wie ändere ich das KI-Modell für ein Silicon Being?

Klicken Sie auf den Link ""KI-Client"" in der Being-Detailseite, wählen Sie ein neues KI-Modell aus und konfigurieren Sie es. Nimmt sofort nach dem Speichern wirksam. Neue Konversationen werden das neue Modell verwenden.

### F: Das Verhalten des Silicon Beings entspricht nicht den Erwartungen?

1. Überprüfen Sie, ob die Soul-Datei klar und deutlich ist
2. Fügen Sie weitere Verhaltensrichtlinien und Einschränkungen hinzu
3. Geben Sie spezifische Arbeitsablaufanleitungen
4. Testen und kontinuierlich optimieren
";

    public override string ChatSystem => @"
# Chat-System

## Konversation starten

1. Klicken Sie auf das **💬 Chat**-Symbol in der linken Navigationsleiste
2. Wählen Sie das Silicon Being aus der linken Liste aus, mit dem Sie chatten möchten
3. Geben Sie Ihre Nachricht in das Eingabefeld unten ein
4. Drücken Sie `Enter` oder klicken Sie auf die ""Senden""-Taste
5. Die KI antwortet in Echtzeit (Text erscheint Zeichen für Zeichen)

## Beschreibung der Oberfläche

### Oberflächenlayout

- **Linke Liste**: Zeigt alle Silicon Beings, klicken Sie zum Wechseln des Konversationsziels
- **Mittlerer Bereich**: Zeigt Konversationsnachrichten an
  - Ihre Nachrichten erscheinen rechts
  - KI-Antworten erscheinen links
- **Unterer Eingabebereich**: Eingabefeld und Senden-Taste

### Tastenbeschreibung

- **Senden-Taste**: Sendet Ihre eingegebene Nachricht
- **⏹ Stoppen-Taste**: Erscheint, wenn die KI antwortet, klicken Sie, um die KI-Antwort zu unterbrechen
- **📁 Datei-Taste**: Dateien zum Analysieren durch die KI hochladen

## Grundlegende Operationen

### Nachricht senden

- Drücken Sie `Enter` zum Senden nach dem Eingeben Ihrer Nachricht
- Drücken Sie `Shift + Enter` für einen Zeilenumbruch

### Antwort stoppen

Wenn die KI derzeit antwortet, können Sie:
- Auf die ""⏹ Stoppen""-Taste klicken
- Oder eine neue Nachricht senden (unterbricht automatisch die aktuelle Antwort)

### Datei hochladen

Lassen Sie die KI Dateiinhalte analysieren:

1. Klicken Sie auf die **📁**-Taste neben dem Eingabefeld
2. Geben Sie den Dateipfad im Popup-Panel ein
   - Zum Beispiel: `C:\Users\IhrBenutzername\Dokumente\Bericht.pdf`
3. Klicken Sie auf ""Upload bestätigen""
4. Die KI wird die Datei lesen und analysieren

**Unterstützte Dateitypen**:
- Textdateien: .txt, .md, .json, .xml
- Codedateien: .cs, .js, .py, .java usw.
- Konfigurationsdateien: .yml, .yaml, .ini, .conf
- Andere Dateien: .csv, .log usw.

## Konversationsmerkmale

### Echtzeit-Streaming-Anzeige

KI-Antworten werden Zeichen für Zeichen angezeigt, sodass Sie nicht auf die vollständige Antwort warten müssen, um den Inhalt zu sehen.

### Multi-Turn-Konversation

- Das System speichert automatisch den Konversationsverlauf
- Die KI erinnert sich an ранее Gesagtes
- Sie können sich direkt auf frühere Konversationen beziehen

### Tool-Aufrufe

Die KI kann während der Konversation automatisch Tools aufrufen, um:
- Kalender abzufragen
- Systemkonfiguration zu verwalten
- Code auszuführen
- Dateien zu lesen
- Hilfe zu suchen
- Notizen zu erstellen
- Speicher abzufragen

Wenn die KI ein Tool aufruft, sehen Sie den Tool-Namen und das Ausführungsergebnis.

### Mehrsprachige Konversation

Sie können mit der KI in jeder Sprache kommunizieren, und die KI antwortet automatisch in derselben Sprache.

## Chatverlauf anzeigen

Wenn Sie frühere Konversationsprotokolle anzeigen möchten:

1. Klicken Sie auf das **🧠 Beings**-Symbol in der linken Navigationsleiste
2. Klicken Sie auf die Silicon Being-Karte, die Sie anzeigen möchten
3. Finden Sie den Link ""Chatverlauf"" in den rechten Details
4. Klicken Sie, um alle historischen Sitzungen anzuzeigen

## Häufige Fragen

### F: Was tun, wenn die KI langsam antwortet?

**Mögliche Gründe**:
- Das verwendete Modell ist groß und benötigt mehr Rechenzeit
- Netzwerkverzögerung (bei Verwendung von Cloud-Modellen)
- Langer Konversationsverlauf

**Lösungen**:
- Versuchen Sie, lokale Modelle zu verwenden (wie Ollama)
- Wählen Sie ein leichteres Modell

### F: Die KI ruft keine Tools auf?

**Überprüfen Sie Folgendes**:
1. Bestätigen Sie, ob das Tool aktiviert ist
2. Überprüfen Sie, ob es Berechtigungseinschränkungen gibt
3. Bestätigen Sie, ob das KI-Modell Tool-Aufrufe unterstützt

### F: Wie lade ich Dateien hoch?

Klicken Sie auf die ""📁""-Taste neben dem Eingabefeld, geben Sie den vollständigen Pfad der Datei ein (z. B. `C:\Dokumente\datei.pdf`), dann klicken Sie auf ""Upload bestätigen"".

### F: Wie sehe ich frühere Konversationen?

Auf der ""Beings""-Seite klicken Sie auf den Link ""Chatverlauf"" des entsprechenden Silicon Beings, um alle historischen Sitzungen zu sehen.

## Verwendungsvorschläge

1. **Klar ausdrücken**: Beschreiben Sie Ihre Anforderungen in klarer Sprache
2. **Schrittweise fragen**: Teilen Sie komplexe Fragen in mehrere kleinere Fragen auf
3. **Hintergrund bereitstellen**: Erklären Sie bei Bedarf relevante Hintergrundinformationen
4. **Datei-Upload nutzen**: Wenn Sie möchten, dass die KI Dateien analysiert, geben Sie direkt den Dateipfad an
5. **Auf Tool-Aufrufe achten**: Beachten Sie die von der KI aufgerufenen Tools, um sicherzustellen, dass die Operationen den Erwartungen entsprechen
";

    public override string Dashboard => @"
# Dashboard

## Übersicht

Das Dashboard ist Ihr Systemüberwachungszentrum, das Ihnen ermöglicht, den Ausführungsstatus der Silicon Beings auf einen Blick zu verstehen. Durch intuitive Statistikkarten und Diagramme können Sie jederzeit die Gesamtsituation des Systems im Blick behalten.

## Hauptfunktionen

- **Echtzeitüberwachung**: Zeigt die Anzahl der Silicon Beings, aktiven Status und Systemressourcennutzung an
- **Nachrichtenstatistiken**: Zeigt die Häufigkeit kürzlicher Chat-Nachrichten in Diagrammform an
- **Automatische Aktualisierung**: Daten werden automatisch aktualisiert, kein manuelles Eingreifen erforderlich

## Beschreibung der Oberfläche

### Statistikkarten

Oben auf der Seite werden 4 Statistikkarten angezeigt, die wichtige Systeminformationen darstellen:

| Karte | Beschreibung |
|------|------|
| 🧠 Silicon Beings Gesamt | Die Gesamtzahl aller Silicon Beings, die Sie erstellt haben |
| ⚡ Aktive Silicon Beings | Die Anzahl der Silicon Beings, die derzeit arbeiten oder Aufgaben verarbeiten |
| ⏱️ System-Betriebszeit | Die Zeit, die das Programm seit dem Start läuft |
| 💾 Speichernutzung | Die Größe des Computerspeichers, die das Programm derzeit verwendet |

### Nachrichtenhäufigkeitsdiagramm

Unter den Statistikkarten wird ein Balkendiagramm angezeigt, das die Anzahl der Nachrichten an jedem Zeitpunkt in den letzten 20 Minuten darstellt:
- Je höher der Balken, desto häufiger war die Nachrichteninteraktion in diesem Zeitraum
- Wenn der Balken leer ist, bedeutet dies, dass es in diesem Zeitraum keine Nachrichteninteraktion gab

## So greifen Sie zu

1. Starten Sie das Silicon Life Collective-Programm

2. Öffnen Sie den Browser und greifen Sie auf die Weboberfläche des Programms zu

3. Klicken Sie auf den Menüpunkt **📊 Dashboard** in der linken Navigationsleiste

## So verstehen Sie die Daten

### Silicon Being-Aktivität

- Wenn die aktive Zahl nahe an der Gesamtzahl ist: bedeutet dies, dass die meisten Beings beschäftigt sind
- Wenn die aktive Zahl 0 ist: bedeutet dies, dass alle Beings ruhen und auf neue Aufgaben warten

### Speichernutzung
- Normaler Bereich: 50-300 MB (abhängig von der Anzahl der erstellten Beings und der Länge des Konversationsverlaufs)
- Wenn Sie feststellen, dass der Speicher kontinuierlich wächst und 500 MB überschreitet, wird empfohlen, das Programm neu zu starten

### Nachrichtenhäufigkeitstrend
- Beobachten Sie die Veränderungen der Balkenhöhen, um zu verstehen, zu welchen Zeiten Sie das System am häufigsten nutzen

## Häufige Fragen

### F: Warum werden die Statistiken nicht aktualisiert?

**A**: Überprüfen Sie die folgenden Punkte:
1. Gibt es JavaScript-Fehler in der Browserkonsole
2. Sind Netzwerkanfragen normal (F12 → Registerkarte Network)
3. Läuft der Backend-Dienst normal
4. Versuchen Sie, die Seite zu aktualisieren (F5)

### F: Diagramm zeigt leer oder keine Daten?

**A**: Mögliche Gründe:
1. Das System wurde gerade gestartet, es gibt noch keine Nachrichteninteraktionsprotokolle
2. In den letzten 20 Minuten wurden keine Nachrichten generiert
3. Der ChatSystem-Dienst wurde nicht korrekt initialisiert

### F: Betriebszeit wird falsch angezeigt?

**A**: Die Betriebszeit wird ab dem Start der Anwendung berechnet, wenn sie abnormal angezeigt wird:
1. Überprüfen Sie, ob die Systemzeit korrekt ist
2. Starten Sie die Anwendung neu, um die Zeitmessung zurückzusetzen

### F: Wie aktualisiere ich die Daten manuell?

**A**: Die aktuelle Version wird automatisch aktualisiert, wenn Sie manuell aktualisieren müssen:
- Drücken Sie F5, um die gesamte Seite zu aktualisieren
- Oder drücken Sie Ctrl+F5, um die Aktualisierung zu erzwingen (Cache löschen)

## Verwendungsvorschläge

### Tägliche Überwachung

1. **Regelmäßig überprüfen**: Es wird empfohlen, es jeden Tag zu öffnen, um den Systemausführungsstatus zu verstehen
2. **Trends beobachten**: Durch das Nachrichtenhäufigkeitsdiagramm verstehen, zu welchen Zeiten Sie es am häufigsten verwenden
3. **Auf Speicher achten**: Wenn die Speichernutzung 500 MB überschreitet, können Sie einen Neustart des Programms in Betracht ziehen
4. **Aktivität verstehen**: Durch die Anzahl aktiver Beings beurteilen, ob das System normal funktioniert

### Optimierungsvorschläge

1. **Anzahl der Beings kontrollieren**: Zu viele Beings erstellen wird mehr Speicher belegen
2. **Alte Konversationen bereinigen**: Bereinigen Sie regelmäßig nicht benötigte Konversationsverläufe, um Speicherplatz freizugeben
3. **Mit Systemtools kombinieren**: Sie können die Systemressourcen zusammen mit dem Task-Manager des Computers überwachen

### Was tun bei Problemen?

Wenn das Dashboard nicht korrekt angezeigt wird, können Sie die folgenden Schritte versuchen:

1. **Browser überprüfen**: Öffnen Sie die Browserkonsole (drücken Sie F12), überprüfen Sie, ob es Fehlermeldungen gibt
2. **Netzwerkverbindung überprüfen**: Bestätigen Sie, dass das Programm läuft und das Netzwerk normal ist
3. **Programmprotokolle überprüfen**: Überprüfen Sie, ob das Programm abnormale Protokolle hat
4. **Programm neu starten**: Wenn keine der oben genannten Methoden funktioniert, versuchen Sie, das Programm zu schließen und erneut zu öffnen
";

    public override string Task => @"
# Aufgaben

## Übersicht

Das Aufgaben- und Timer-System protokolliert den automatisierten Ausführungsstatus von Silicon Beings. Sie können die Aufgabenliste und den Timer-Status anzeigen, um zu verstehen, was das Silicon Being tut und wann es ausgeführt wird.

## Aufgabensystem

### Was ist eine Aufgabe?

Aufgaben sind Arbeitselemente, die ein Silicon Being ausführt oder abgeschlossen hat, wie:
- Verarbeitungsaufgaben, die automatisch von der KI erstellt wurden
- Arbeitselemente, die vom System generiert wurden
- Ausführungsaufgaben, die durch Timer ausgelöst wurden

### Aufgabenliste anzeigen

**Methode 1: Alle Aufgaben anzeigen**

1. Klicken Sie auf das ""Aufgaben""-Symbol in der linken Navigationsleiste (falls vorhanden)
2. Die Seite zeigt die Aufgabenliste aller Silicon Beings an

**Methode 2: Aufgaben eines bestimmten Beings anzeigen**

1. Gehen Sie zur Seite **🧠 Beings**
2. Klicken Sie auf das Silicon Being, das Sie anzeigen möchten
3. Finden Sie den Link ""Aufgaben"" in den Details
4. Klicken Sie, um zur Aufgabenseite zu gelangen

### Aufgabeninformationen

Jede Aufgabe zeigt die folgenden Informationen:

- **Aufgabenname**: Der Titel der Aufgabe
- **Status**:
  - Wartend (gelb)
  - In Betrieb (blau)
  - Abgeschlossen (grün)
  - Fehlgeschlagen (rot)
  - Abgesagt (grau)
- **Priorität**: Die Prioritätsstufe der Aufgabe
- **Zugewiesen an**: Das Silicon Being, das diese Aufgabe ausführt
- **Erstellungszeit**: Wann die Aufgabe erstellt wurde
- **Beschreibung**: Detaillierte Beschreibung der Aufgabe

### Aufgabenstatus-Erklärung

- **Wartend**: Aufgabe wurde erstellt, wartet auf Ausführung
- **In Betrieb**: Aufgabe wird derzeit ausgeführt
- **Abgeschlossen**: Aufgabe erfolgreich abgeschlossen
- **Fehlgeschlagen**: Aufgabenausführung fehlgeschlagen, Fehlerinformationen können angezeigt werden
- **Abgesagt**: Aufgabe wurde abgesagt

## Timer-System

### Was ist ein Timer?

Ein Timer ist ein automatischer Auslösemechanismus, der Silicon Beings ermöglicht, Operationen zu bestimmten Zeiten auszuführen. Das System verwendet ein Kalendersystem, um Auslösebedingungen zu definieren.

### Timer-Liste anzeigen

**Methode 1: Alle Timer anzeigen**

1. Klicken Sie auf das ""Timer""-Symbol in der linken Navigationsleiste (falls vorhanden)
2. Die Seite zeigt die Timer-Liste aller Silicon Beings an

**Methode 2: Timer eines bestimmten Beings anzeigen**

1. Gehen Sie zur Seite **🧠 Beings**
2. Klicken Sie auf das Silicon Being, das Sie anzeigen möchten
3. Finden Sie den Link ""Timer"" in den Details
4. Klicken Sie, um zur Timer-Seite zu gelangen

### Timer-Informationen

Jeder Timer zeigt die folgenden Informationen:

- **Timer-Name**: Bezeichner des Timers
- **Status**: In Betrieb oder Gestoppt
- **Typ**: Auslösetyp des Timers
- **Auslösezeit**: Nächste Auslösezeit
- **Kalendersystem**: Verwendeter Kalender (z. B. Gregorianisch, Lunarkalender usw.)
- **Auslöseanzahl**: Gesamtzahl der bisherigen Auslösungen
- **Erstellungszeit**: Wann der Timer erstellt wurde
- **Letzte Auslösezeit**: Zeitpunkt der letzten Auslösung

### Timer-Typen

Das System unterstützt mehrere Auslösemethoden:

- **Intervall-Auslösung**: Löst in festen Intervallen aus
  - Zum Beispiel: Alle 2 Stunden, alle 30 Minuten
  
- **Kalender-Auslösung**: Löst basierend auf Kalenderbedingungen aus
  - Zum Beispiel: Täglich um 9 Uhr, jeden Montag, am 1. jedes Monats
  - Unterstützt mehrere Kalendersysteme einschließlich Gregorianisch, Lunarkalender usw.

## Ausführungsverlauf anzeigen

### Timer-Ausführungsverlauf

Verstehen Sie den Ausführungsstatus von Timern:

1. Gehen Sie zur Timer-Seite
2. Finden Sie den Timer, den Sie anzeigen möchten
3. Klicken Sie auf den Link ""Ausführungsverlauf""
4. Zeigen Sie alle Auslöseprotokolle an

### Ausführungsdetails

Detaillierte Informationen für jede Ausführung:

1. Finden Sie eine bestimmte Ausführung im Ausführungsverlauf
2. Klicken Sie, um Details anzuzeigen
3. Sie können sehen:
   - Ausführungszeit
   - Ausführungsergebnis
   - Verwandte Konversationsnachrichten
   - Fehlerinformationen (wenn fehlgeschlagen)

### Ausführungsnachrichten

Zeigen Sie die vollständige Konversation für eine bestimmte Ausführung an:

1. Finden Sie den Link ""Nachrichten"" in der Ausführungsdetailseite
2. Zeigen Sie die vollständige Konversation zwischen KI und Benutzer an
3. Verstehen Sie, wie die KI diesen Auslöser behandelt hat

## Häufige Fragen

### F: Wie erstelle ich eine neue Aufgabe?

**A:** Aufgaben werden automatisch vom System generiert und unterstützen keine manuelle Erstellung. Wenn ein Silicon Being eine Aufgabe ausführen muss, erstellt es automatisch eine.

### F: Wie erstelle ich einen neuen Timer?

**A:** Timer werden automatisch von Silicon Beings verwaltet und unterstützen keine manuelle Erstellung. Silicon Beings richten Timer nach Bedarf ein, um periodische Aufgaben auszuführen.

### F: Kann ich Aufgaben oder Timer löschen?

**A:** Das System bietet keine manuelle Löschfunktionalität. Aufgaben und Timer werden automatisch von Silicon Beings verwaltet.

### F: Was tun, wenn eine Aufgabe ""Fehlgeschlagen"" anzeigt?

**Vorschläge:**
1. Zeigen Sie die Fehlerinformationen der Aufgabe an
2. Verstehen Sie den Grund für das Scheitern
3. Wenn es ein temporäres Problem ist, könnte die Aufgabe es erneut versuchen
4. Wenn es weiterhin fehlschlägt, können Sie mit dem Silicon Being chatten, um die Situation zu verstehen

### F: Timer löst nicht aus?

**Überprüfen:**
1. Ist der Timer im Betriebsstatus?
2. Sind die Auslösebedingungen erfüllt?
3. Läuft das Silicon Being normal?
4. Zeigen Sie den Ausführungsverlauf an, um die Situation zu verstehen

### F: Wie erfahre ich, was das Silicon Being tut?

**Methoden:**
1. Zeigen Sie die Aufgabenliste an, um aktuelle ausführende Aufgaben zu verstehen
2. Zeigen Sie die Timer-Liste an, um bevorstehende ausgelöste Operationen zu verstehen
3. Zeigen Sie den Ausführungsverlauf an, um vergangene Aktivitäten zu verstehen
4. Chatten Sie direkt mit dem Silicon Being, um zu fragen

### F: Was bedeutet Aufgabenpriorität?

**A:** Priorität zeigt die Wichtigkeit einer Aufgabe an. Je kleiner die Zahl, desto höher die Priorität. Hochprioritäre Aufgaben werden zuerst verarbeitet.

## Verwendungsvorschläge

1. **Regelmäßig überprüfen**: Verstehen Sie den automatisierten Ausführungsstatus von Silicon Beings
2. **Auf fehlgeschlagene Aufgaben achten**: Behandeln Sie abnormale Situationen umgehend
3. **Ausführungsverlauf anzeigen**: Verstehen Sie die Arbeitsmuster der KI
4. **Mit Chat kombinieren**: Diskutieren Sie Aufgaben- und Timer-Status mit Silicon Beings

## Technische Hinweise

### Datenspeicherung

Aufgaben- und Timer-Daten werden im Systemdatenverzeichnis gespeichert, assoziiert mit Silicon Beings:
```
data/
  beings/
    {BeingID}/
      tasks/      (Aufgabendaten)
      timers/     (Timer-Daten)
```

### Automatische Verwaltung

Das System wird automatisch:
- Aufgaben erstellen und verwalten
- Timer auslösen
- Ausführungsverlauf protokollieren
- Abgelaufene Daten bereinigen

Sie müssen nicht manuell verwalten; das System wird alles handhaben.
";

    public override string Timer => @"
# Timer

## Was sind Timer?

Timer sind der automatische Erinnerungsmechanismus der Silizium-Lebensformen. Wenn die eingestellte Zeit erreicht ist, führt die Silizium-Lebensform automatisch die entsprechende Aufgabe aus.

## Zwei Arten von Timern

### Einmaliger Timer

Ein Timer, der nur einmal auslöst. Nach dem Auslösen endet er automatisch.

**Geeignete Szenarien:**
- Sie daran erinnern, zu einer bestimmten Zeit etwas zu tun
- Eine einmalige Aufgabe an einem bestimmten Datum ausführen

### Periodischer Timer

Ein Timer, der wiederholt auslöst. Nach jedem Auslösen berechnet das System automatisch die nächste Auslösezeit.

**Geeignete Szenarien:**
- Aufgaben, die täglich zu einer festen Zeit ausgeführt werden (z. B. Morgenbericht jeden Tag um 9 Uhr)
- Aufgaben, die wöchentlich oder monatlich regelmäßig ausgeführt werden
- Aufgaben, die regelmäßig nach dem Mondkalender Feiertagen ausgeführt werden (z. B. jedes Mond-Neujahr)

## Status der Timer

Timer haben vier Status:

| Status | Beschreibung |
|------|------|
| **Läuft** | Der Timer funktioniert normal und wartet auf das Auslösen |
| **Pausiert** | Der Timer ist vorübergehend gestoppt und wird nicht auslösen |
| **Ausgelöst** | Einmaliger Timer hat das Auslösen abgeschlossen |
| **Abgebrochen** | Der Timer wurde abgebrochen und wird nicht mehr auslösen |

## Wie werden Timer angezeigt?

### Timer einer bestimmten Silizium-Lebensform anzeigen

1. Gehen Sie zur Seite **🧠 Silizium-Lebensform**
2. Wählen Sie die Silizium-Lebensform aus, die Sie anzeigen möchten
3. Finden Sie die Option ""Timer"", um alle Timer dieser Lebensform anzuzeigen

## Bei Timern angezeigte Informationen

Beim Anzeigen von Timern können Sie folgende Informationen sehen:

| Informationselement | Beschreibung |
|------|------|
| **Name** | Der Name des Timers |
| **Beschreibung** | Die detaillierte Beschreibung des Timers (falls vorhanden) |
| **Status** | Aktueller Status (Läuft, Pausiert usw.) |
| **Typ** | Einmalig oder periodisch |
| **Nächstes Auslösedatum** | Die genaue Zeit, wann der Timer als nächstes auslöst |
| **Kalendersystem** | Verwendeter Kalender (z. B. Gregorianisch, Mondkalender usw.) |
| **Auslöseanzahl** | Wie oft der Timer bereits ausgelöst hat |
| **Erstellungszeit** | Wann der Timer erstellt wurde |
| **Letztes Auslösedatum** | Die Zeit des letzten Auslösens (falls bereits ausgelöst) |

## Unterstützte Kalendersysteme

Timer unterstützen verschiedene Kalendersysteme, einschließlich:

- **Gregorianischer Kalender** (Gregorian): International verwendetes Sonnensystem
- **Mondkalender** (Chinese Lunar): Traditioneller chinesischer Mondkalender
- **Andere Kalendersysteme**: Islamischer Kalender, Himmelsstämme-Kalender usw.
- **Intervall-Kalender** (Interval): Auslösen in festen Zeitintervallen (z. B. alle 2 Stunden)

## Auslöseverlauf der Timer

Jedes Mal, wenn ein Timer auslöst, zeichnet das System detaillierte Ausführungsinformationen auf.

### Auslöseverlauf anzeigen

1. Wählen Sie einen bestimmten Timer aus der Timer-Liste aus
2. Zeigen Sie den Ausführungsverlauf dieses Timers an
3. Sie können die Details jedes Auslösens sehen:
   - Auslösezeit
   - Ausführungsstatus (Erfolg, Fehler usw.)
   - Konversationsnachrichten während der Ausführung (falls vorhanden)
   - Fehlerinformationen (wenn die Ausführung fehlgeschlagen ist)

### Status des Ausführungsverlaufs

Jede Ausführung hat die folgenden Status:

| Status | Beschreibung |
|------|------|
| **Nicht gestartet** | Die Ausführung hat noch nicht begonnen |
| **Gestartet** | Startbenachrichtigung wurde gesendet |
| **Ausführend** | Die Aufgabe wird ausgeführt |
| **Abgeschlossen** | Die Aufgabe wurde erfolgreich abgeschlossen |
| **Fehlgeschlagen** | Die Aufgabenausführung ist fehlgeschlagen |

## Häufig gestellte Fragen

### F: Wie erstelle ich einen neuen Timer?

**A:** Timer werden automatisch von den Silizium-Lebensformen verwaltet. Silizium-Lebensformen erstellen Timer nach Bedarf, um regelmäßige Aufgaben auszuführen.

### F: Kann ich Timer löschen oder pausieren?

**A:** Ja. Das System unterstützt folgende Operationen:
- **Timer pausieren**: Den Timer vorübergehend anhalten, kann bei Bedarf wiederhergestellt werden
- **Timer wiederherstellen**: Einen pausierten Timer in den laufenden Status wiederherstellen. Wenn während der Pause eine Auslösezeit verpasst wurde, berechnet das System automatisch die nächste Auslösezeit
- **Timer abbrechen**: Einen Timer dauerhaft abbrechen, wird nicht mehr auslösen
- **Timer löschen**: Einen Timer vollständig aus dem System entfernen

### F: Was tun, wenn der Timer nicht auslöst?

**Überprüfen Sie Folgendes:**
1. Ob sich der Timer im Status **Läuft** befindet (nicht Pausiert oder Abgerochen)
2. Ob die Auslösezeit erreicht ist (überprüfen Sie ""Nächstes Auslösedatum"")
3. Ob die Silizium-Lebensform normal läuft

### F: Wie erfahre ich, wie die Timer arbeiten?

**Methoden:**
1. Überprüfen Sie die Timer-Liste, um bevorstehende Operationen zu verstehen
2. Überprüfen Sie den Ausführungsverlauf, um vergangene Aktivitäten zu verstehen
3. Fragen Sie direkt die Silizium-Lebensform im Gespräch.

### F: Wie berechnet ein periodischer Timer die nächste Auslösezeit?

**A:** Nach jedem Auslösen eines periodischen Timers berechnet das System automatisch die nächste Auslösezeit basierend auf dem Kalendersystem und den eingestellten Bedingungen. Zum Beispiel:
- Wenn Gregorianisch ""jeden Tag um 9 Uhr"" eingestellt ist, wird das System jeden Tag um 9 Uhr auslösen und dann 9 Uhr des nächsten Tages als nächste Auslösezeit berechnen
- Wenn Mondkalender ""jedes Jahr erster Tag des ersten Monats"" eingestellt ist, wird das System während des Mond-Neujahrs auslösen und dann das Datum des nächsten Mond-Neujahrs berechnen

## Verwendungsempfehlungen

### Tägliche Überwachung

1. **Regelmäßig überprüfen**: Verstehen Sie die automatische Ausführung der Silizium-Lebensformen
2. **Ausführungsverlauf beachten**: Verstehen Sie den Arbeitsmodus der KI
3. **Anomalien überprüfen**: Überprüfen Sie, ob es Aufzeichnungen über fehlgeschlagene Ausführungen gibt

### Optimierungsvorschläge

1. **Mit Gespräch kombinieren**: Diskutieren Sie den Status der Timer mit der Silizium-Lebensform
2. **Ausführungsergebnisse beobachten**: Verstehen Sie die Timer-Effekte durch den Ausführungsverlauf
3. **Strategie anpassen**: Lassen Sie bei Bedarf die Silizium-Lebensform die Timer-Einstellungen durch Gespräch anpassen
";

    public override string Permission => @"
# Berechtigungsverwaltung

## Was ist das Berechtigungssystem?

Das Berechtigungssystem schützt Ihre Systemsicherheit und verhindert, dass die KI nicht autorisierte Operationen ausführt. Wenn die KI versucht, bestimmte Operationen auszuführen (wie Dateizugriff, Befehle ausführen usw.), überprüft das System, ob es erlaubt ist.

## Wie funktionieren Berechtigungen?

### Automatisches Berechtigungs-Popup

Wenn die KI versucht, eine Operation auszuführen, die eine Berechtigung erfordert, wird das System ein Popup anzeigen, um Sie zu fragen:

**Popup-Inhalt umfasst:**
- Berechtigungstyp (z. B. Dateizugriff, Befehlausführung usw.)
- Angeforderte Ressource (z. B. Dateipfad)
- Detaillierte Informationen

**Sie können wählen:**
- **Erlauben**: Diese Operation ausführen
- **Verweigern**: Diese Operation blockieren

### Berechtigungsprüfreihenfolge

Das System überprüft Berechtigungen in der folgenden Reihenfolge:

1. **Silicon Curator**: Wenn es die Operation des Curators ist, automatisch erlauben
2. **Frequenzlimit**: Verhindert eine große Anzahl von Anfragen in kurzer Zeit
3. **Globale Regeln**: Vordefinierte Erlaubnis/Verweigerungs-Regeln
4. **Benutzerdefinierte Regeln**: Von Ihnen geschriebene Berechtigungsregeln (falls vorhanden)
5. **Benutzer fragen**: Wenn keine der oben genannten entscheiden kann, Popup anzeigen, um Sie zu fragen

## Eingebaute Berechtigungsregeln

Das System hat vordefinierte sichere Berechtigungsregeln:

### Dateizugriffsregeln

**Erlaubter Zugriff:**
- Eigenes temporäres Verzeichnis des Silicon Beings
- Häufige Benutzerordner (Desktop, Downloads, Dokumente, Bilder, Musik, Videos)
- Öffentliche Benutzerordner

**Verweigerter Zugriff:**
- Systemkritische Verzeichnisse (Windows-Systemverzeichnis, Linux /etc /boot usw.)
- Datenverzeichnisse anderer Silicon Beings

**Nicht übereinstimmende Pfade:**
- Wird Popup anzeigen, um zu fragen, ob Sie es erlauben

## Benutzerdefinierte Berechtigungsregeln (Erweiterte Funktion)

Wenn Sie granularere Berechtigungskontrolle benötigen, können Sie benutzerdefinierte Berechtigungsregeln schreiben.

### Berechtigungsbearbeitungsseite zugreifen

1. Gehen Sie zur Seite **🧠 Beings**
2. Klicken Sie auf das Silicon Being, das Sie konfigurieren möchten
3. Finden Sie den Link ""Berechtigungen"" in den Details
4. Öffnen Sie den Berechtigungscode-Editor

### Berechtigungscode-Editor

Der Berechtigungseditor ist eine Code-Bearbeitungsoberfläche, die unterstützt:
- C#-Code-Syntaxhervorhebung
- Code-Autovervollständigung
- Echtzeit-Speicherung
- Sicherheitsprüfung (zur Verhinderung von bösartigem Code)

**Speichermethode:**
- Klicken Sie auf die ""Speichern""-Taste im Editor
- Das System wird zuerst kompilieren und prüfen
- Nimmt nur wirksam nach erfolgreicher Sicherheitsprüfung

### Standardvorlage

Wenn noch kein benutzerdefinierter Berechtigungscode vorhanden ist, wird das System eine Standardvorlage bereitstellen. Sie können sie basierend auf der Vorlage modifizieren.

## Berechtigungsregeln anzeigen

### Aktuelle Regelliste anzeigen

1. Gehen Sie zur Berechtigungsbearbeitungsseite
2. Die Seite zeigt alle Berechtigungsregeln für dieses Silicon Being an
3. Jede Regel umfasst:
   - Berechtigungstyp
   - Ressourcenpfad
   - Erlauben/Verweigern
   - Beschreibung

## Berechtigungsanfrageverlauf

Alle Berechtigungsanfragen werden im Audit-Protokoll protokolliert:

1. Klicken Sie auf das **🔍 Audit**-Symbol links
2. Filtern Sie berechtigungsbezogene Protokolle
3. Zeigen Sie historische Anfragen und Ihre Entscheidungen an

## Häufige Fragen

### F: Warum wurde die KI-Operation verweigert?

**Mögliche Gründe:**
- Die Operation ist in einer Verweigerungsregel
- Frequenzlimit ausgelöst
- Sie haben zuvor verweigert gewählt

**Lösungen:**
1. Überprüfen Sie das Audit-Protokoll, um den spezifischen Grund zu verstehen
2. Ändern Sie Berechtigungsregeln bei Bedarf
3. Führen Sie die Operation erneut aus

### F: Was tun, wenn es zu viele Berechtigungs-Popups gibt?

**Vorschläge:**
- Für häufig verwendete sichere Operationen, erwägen Sie, benutzerdefinierte Regeln zu schreiben, um automatisch zu erlauben
- Überprüfen Sie, ob Sie Popups durch Ändern von Regeln reduzieren können

### F: Ist benutzerdefinierter Berechtigungscode gefährlich?

**Sicherheitsgarantien:**
- Code wird durch Sicherheitsprüfung gehen
- Bösartiger Code wird abgelehnt
- Kompilierungsfehler werden nicht wirksam

**Vorschläge:**
- Wenn Sie mit Programmierung nicht vertraut sind, wird empfohlen, Standardregeln zu verwenden
- Sichern Sie Originalcode vor dem Modifizieren
- Testen Sie vor dem Anwenden auf Produktionsumgebung

### F: Berechtigungskonfigurationsfehler verursacht Unbenutzbarkeit?

**Lösungen:**
1. Als Silicon Curator operieren (Curator hat höchste Berechtigung)
2. Benutzerdefinierten Berechtigungscode löschen (Code leeren und speichern)
3. Das System wird Standardregeln wiederherstellen

### F: Kann ich verschiedene Berechtigungen für verschiedene Beings einstellen?

**A:** Ja. Jedes Silicon Being hat unabhängige Berechtigungskonfiguration, und sie beeinflussen sich nicht gegenseitig.

## Sicherheitsempfehlungen

1. **Seien Sie vorsichtig bei sensiblen Operationen**: Wie Löschen von Dateien, Ausführen von Befehlen usw.
2. **Überprüfen Sie regelmäßig Audit-Protokolle**: Verstehen Sie die Operationshistorie der KI
3. **Ändern Sie Berechtigungsregeln nicht willkürlich**: Es sei denn, Sie verstehen deren Auswirkungen
4. **Halten Sie das System aktualisiert**: Erhalten Sie den neuesten Sicherheitsschutz

## Berechtigungstyp-Beschreibung

Das System unterstützt die folgenden Berechtigungstypen:

- **Netzwerkzugriff**: KI versucht, auf Netzwerkressourcen zuzugreifen
- **Befehlausführung**: KI versucht, Kommandozeilenprogramme auszuführen
- **Dateizugriff**: KI versucht, Dateien zu lesen oder zu schreiben
- **Funktionsaufruf**: KI versucht, bestimmte Funktionen aufzurufen
- **Datenzugriff**: KI versucht, auf Systemdaten zuzugreifen

Jeder Typ hat unterschiedliche Sicherheitsstufen und Behandlungsmethoden.
";

    public override string Config => @"
# Konfigurationsverwaltung

## Was ist Konfigurationsverwaltung?

Die Konfigurationsverwaltungsseite ermöglicht es Ihnen, verschiedene Systemeinstellungen anzupassen, einschließlich KI-Dienste, Netzwerk, Sprache, Oberflächenthemes usw.

## Wie verwende die Konfigurationsseite?

1. Klicken Sie auf das **⚙ Konfiguration**-Symbol in der linken Navigationsleiste
2. Die Seite zeigt mehrere Konfigurationsgruppen an, jede enthält mehrere Konfigurationseinträge
3. Finden Sie den Konfigurationseintrag, den Sie ändern möchten, klicken Sie auf die ""Bearbeiten""-Taste rechts
4. Geben Sie den neuen Wert im Popup-Bearbeitungsfeld ein
5. Klicken Sie auf die ""Speichern""-Taste

## Konfigurationsgruppenbeschreibung

### Grundeinstellungen

Enthält grundlegende Systemkonfiguration:

- **Datenverzeichnis**: Der Ordnerspeicherort für alle Systemdaten
  - Standard: `./data`
  - Empfehlung: Behalten Sie den Standard, es sei denn, Sie haben spezielle Anforderungen

- **Sprache**: Die in der Systemoberfläche angezeigte Sprache
  - Unterstützt: Chinesisch (vereinfacht), Chinesisch (traditionell), Englisch, Japanisch, Koreanisch, Deutsch, Spanisch usw.
  - Nach Änderung: Die Seite wird automatisch aktualisieren, um die neue Sprache anzuwenden

### KI-Einstellungen

Konfigurieren Sie KI-Dienstverbindung und Modell:

- **KI-Client-Typ**: Wählen Sie den zu verwendenden KI-Dienst
  - Ollama (läuft lokal, empfohlen)
  - OpenAI (Cloud-Dienst)
  - Andere mit OpenAI API kompatible Dienste

- **KI-Konfiguration**: Detaillierte Konfiguration für KI-Dienst
  - `endpoint`: API-Adresse (z. B. `http://localhost:11434`)
  - `model`: Zu verwendender Modellname (z. B. `qwen3.5:cloud`)
  - `temperature`: Antwortkreativitätsstufe (0-1, Standard 0.7)
  - `maxTokens`: Maximale Antwortlänge (Standard 4096)

**KI-Konfiguration bearbeiten**:
1. Klicken Sie auf die ""KI-Konfiguration""-Bearbeiten-Taste
2. Ein Diktionseditor wird geöffnet
3. Sie können Konfigurationseinträge hinzufügen, ändern oder löschen
4. Klicken Sie auf ""Speichern"" zum Anwenden

### Laufzeiteinstellungen

Steuern Sie Systemlaufzeitverhalten:

- **Ausführungs-Timeout**: Maximale Ausführungszeit für eine einzelne Aufgabe
  - Standard: 10 Minuten
  - Empfehlung: Behalten Sie den Standard, es sei denn, Aufgaben sind besonders komplex

- **Maximale Timeout-Anzahl**: Wie viele aufeinanderfolgende Timeouts den Schutzmechanismus auslösen
  - Standard: 3 Mal
  - Zweck: Verhindert Endlos-Wiederholungsschleifen

- **Watchdog-Timeout**: Wie lange warten, bevor bei nicht reagierendem System neugestartet wird
  - Standard: 10 Minuten
  - Zweck: Automatische Wiederherstellung hängender Systeme

- **Minimale Protokollebene**: Welche Protokollebenen protokolliert werden sollen
  - Trace: Am detailliertesten (enthält alle Debug-Informationen)
  - Debug: Debug-Informationen
  - Info: Allgemeine Informationen (empfohlen)
  - Warning: Nur Warnungen
  - Error: Nur Fehler

### Web-Einstellungen

Konfigurieren Sie Webserver-Parameter:

- **Web-Port**: Systemzugriffsport
  - Standard: 8080
  - Zugriffsadresse: `http://localhost:8080`
  - Nach Änderung: Systemneustart erforderlich, um wirksam zu werden

- **LAN-Zugriff erlauben**: Ob andere Geräte im lokalen Netzwerk zugreifen dürfen
  - Aus (Standard): Nur von diesem Rechner zugreifbar
  - An: Andere Geräte im selben Netzwerk können ebenfalls zugreifen
  - Hinweis: Administratorberechtigungen erforderlich, wenn aktiviert

- **Web-Skin**: Oberflächentheme
  - Sie können verschiedene Skins wählen, um das Oberflächenaussehen zu ändern
  - Nimmt sofort nach Änderung wirksam

### Benutzereinstellungen

- **Benutzer-Spitzname**: Ihr Anzeigename im System
  - Standard: User
  - Kann in jeden beliebigen Namen geändert werden

## Konfigurationseinträge bearbeiten

### Bearbeitungsmethoden für verschiedene Typen

Das System wird verschiedene Bearbeitungsoberflächen basierend auf dem Konfigurationseintragstyp anzeigen:

**Texttyp**:
- Zeigt ein Texteingabefeld an
- Geben Sie den neuen Wert direkt ein

**Zahlentyp**:
- Zeigt ein Zahleneingabefeld an
- Kann Ganzzahlen oder Dezimalzahlen eingeben

**Boolescher Typ (Ja/Nein)**:
- Zeigt ein Kontrollkästchen an
- Aktiviert bedeutet ""Ja"", deaktiviert bedeutet ""Nein""

**Enum-Typ (Dropdown-Auswahl)**:
- Zeigt eine Dropdown-Liste an
- Wählen Sie eine aus vordefinierten Optionen

**Zeitintervall**:
- Zeigt vier Eingabefelder für Tage, Stunden, Minuten und Sekunden an
- Füllen Sie die entsprechenden Werte separat aus

**Verzeichnispfad**:
- Zeigt ein Pfadeingabefeld und ""Durchsuchen""-Taste an
- Klicken Sie auf ""Durchsuchen"", um einen Ordner auszuwählen
- Oder geben Sie den Pfad direkt ein

**Diktionstyp (Schlüssel-Wert-Paare)**:
- Zeigt einen Schlüssel-Wert-Paar-Editor an
- Kann mehrere Zeilen von Schlüssel-Wert-Paaren hinzufügen
- Klicken Sie auf ""Hinzufügen""-Taste, um eine neue Zeile hinzuzufügen
- Klicken Sie auf ""Löschen""-Taste, um eine Zeile zu entfernen

### Konfiguration speichern

- Klicken Sie nach jeder Konfigurationsänderung auf ""Speichern""
- Die meisten Konfigurationen nehmen sofort wirksam
- Einige Konfigurationen (wie Port) erfordern Systemneustart

## Häufige Fragen

### F: Kann nicht auf das System zugreifen nach Portänderung?

**Lösung**:
1. Überprüfen Sie, ob der Port von einem anderen Programm belegt ist
2. Bestätigen Sie, ob die Firewall diesen Port erlaubt
3. Verwenden Sie den neuen Port zum Zugreifen: `http://localhost:NeuerPort`

### F: Wie stelle ich die Standardkonfiguration wieder her?

**Methode 1**: Manuelle Änderung
1. Gehen Sie zur Konfigurationsseite
2. Ändern Sie Konfigurationseinträge einzeln zurück auf Standardwerte

**Methode 2**: Konfigurationsdatei löschen
1. Schließen Sie das System
2. Löschen Sie die `config.json`-Datei
3. Starten Sie das System neu (erstellt automatisch Standardkonfiguration)

### F: Was tun, wenn die KI-Verbindung fehlschlägt?

**Überprüfen Sie Folgendes**:
1. Läuft der KI-Dienst normal?
2. Ist die Endpunktadresse korrekt?
3. Wenn Cloud-Dienst verwendet wird, ist der API-Schlüssel korrekt?
4. Ist die Netzwerkverbindung normal?

**Lösung**:
1. Gehen Sie zur ""KI-Einstellungen""-Gruppe
2. Klicken Sie auf die ""KI-Konfiguration""-Bearbeiten-Taste
3. Überprüfen Sie, ob `endpoint` und `model` korrekt sind
4. Speichern Sie nach Änderung

### F: Wann werden Konfigurationsänderungen wirksam?

- **Sofort wirksam**: Sprache, Skin, KI-Konfiguration, Benutzer-Spitzname usw.
- **Erfordert Neustart**: Web-Port, LAN-Zugriffseinstellungen

### F: Wo ist die Konfigurationsdatei?

Die Konfigurationsdatei ist die `config.json`-Datei im Systemlaufzeitverzeichnis.

## Verwendungsvorschläge

1. **Mit Vorsicht ändern**: Behalten Sie unsichere Konfigurationseinträge auf Standardwerten
2. **Änderungen protokollieren**: Protokollieren Sie, was und warum Sie nach Konfigurationsänderung geändert haben
3. **Konfiguration sichern**: Sie können die `config.json`-Datei als Sicherung vor wichtigen Änderungen kopieren
4. **Testumgebung**: Wenn möglich, überprüfen Sie Konfiguration zuerst in einer Testumgebung
5. **Sicherheit zuerst**: Stellen Sie Netzwerksicherheit sicher, bevor LAN-Zugriff aktiviert wird
";

    public override string FAQ => @"
# Häufig gestellte Fragen

## Erste Schritte

### F: Wie starte ich das System?

**A:** Doppelklicken Sie auf die Programmdatei zum Starten. Das System wird automatisch den Browser öffnen und die Oberfläche betreten.

### F: Was muss ich beim ersten Start tun?

**A:** Nichts! Das System wird automatisch die Initialisierung abschließen, einschließlich des Erstellens des Silicon Curators. Sie müssen nur warten, bis der Browser geöffnet wird, und können es verwenden.

### F: Der Browser öffnete sich nicht nach dem Systemstart?

**A:** Besuchen Sie manuell `http://localhost:8080`.

## KI-Konversation

### F: Was tun, wenn die KI langsam antwortet?

**Mögliche Gründe:**
- Das verwendete Modell ist groß
- Netzwerkverzögerung (bei Verwendung von Cloud-KI)
- Langer Konversationsverlauf

**Lösungen:**
- Verwenden Sie lokale KI-Dienste (wie Ollama)
- Wählen Sie ein leichteres Modell

### F: Die Antwort der KI entspricht nicht den Erwartungen?

**Vorschläge:**
1. Überprüfen Sie, ob die Soul-Datei klar und deutlich ist
2. Geben Sie mehr Hintergrundinformationen in der Konversation an
3. Versuchen Sie, Ihre Anforderungen spezifischer zu beschreiben

### F: Die KI ruft keine Tools auf?

**Überprüfen:**
1. Bestätigen Sie, dass das Tool aktiviert ist
2. Überprüfen Sie, ob es Berechtigungseinschränkungen gibt
3. Bestätigen Sie, dass das KI-Modell Tool-Aufrufe unterstützt

### F: Wie lasse ich die KI Dateien analysieren?

**Methode:**
1. Klicken Sie auf die ""📁 Datei""-Taste im Chat-Interface
2. Geben Sie den vollständigen Pfad der Datei ein (z. B. `C:\Dokumente\bericht.pdf`)
3. Klicken Sie auf ""Upload bestätigen""
4. Die KI wird die Datei lesen und analysieren

## Silicon Beings

### F: Wie erstelle ich ein neues Silicon Being?

**A:** Derzeit unterstützt das System nicht das direkte Erstellen von Silicon Beings. Der Silicon Curator kann andere Beings erstellen und verwalten. Sie können mit dem Curator chatten und ihn bitten, Ihnen bei der Erstellung zu helfen.

### F: Wie ändere ich das Verhalten eines Silicon Beings?

**Methode:**
1. Gehen Sie zur ""Beings""-Seite
2. Klicken Sie auf das Being, das Sie ändern möchten
3. Klicken Sie auf den Link ""Soul-Datei""
4. Ändern Sie den Prompt-Inhalt
5. Speichern

### F: Wie konfiguriere ich verschiedene KIs für verschiedene Beings?

**Methode:**
1. Gehen Sie zur ""Beings""-Seite
2. Klicken Sie auf das Ziel-Being
3. Klicken Sie auf den Link ""KI-Client""
4. Wählen Sie KI-Dienst aus und konfigurieren Sie
5. Speichern

### F: Silicon Being reagiert nicht?

**Überprüfen:**
1. Läuft der KI-Dienst normal?
2. Ist die Netzwerkverbindung normal?
3. Überprüfen Sie Systemprotokolle auf detaillierte Fehler

## Systemeinstellungen

### F: Wie ändere ich die Systemsprache?

**Methode:**
1. Klicken Sie auf das ""⚙ Konfiguration""-Symbol links
2. Finden Sie den Konfigurationseintrag ""Sprache""
3. Klicken Sie auf ""Bearbeiten""
4. Wählen Sie Sprache aus der Dropdown-Liste
5. Speichern (Seite wird automatisch aktualisieren)

### F: Wie wechsle ich das Oberflächentheme?

**Methode:**
1. Gehen Sie zur ""Konfiguration""-Seite
2. Finden Sie den Konfigurationseintrag ""Web-Skin""
3. Klicken Sie auf ""Bearbeiten""
4. Wählen Sie Ihr bevorzugtes Theme
5. Speichern

### F: Wie ändere ich den Zugriffsport?

**Methode:**
1. Gehen Sie zur ""Konfiguration""-Seite
2. Finden Sie den Konfigurationseintrag ""Web-Port""
3. Klicken Sie auf ""Bearbeiten""
4. Geben Sie die neue Portnummer ein (z. B. 9000)
5. Speichern und starten Sie das System neu

**Hinweis:** Nach dem Ändern des Ports müssen Sie den neuen Port verwenden, um zuzugreifen, z. B. `http://localhost:9000`

### F: Wie erlaube ich anderen Geräten im lokalen Netzwerk den Zugriff?

**Methode:**
1. Gehen Sie zur ""Konfiguration""-Seite
2. Finden Sie den Konfigurationseintrag ""LAN-Zugriff erlauben""
3. Klicken Sie auf ""Bearbeiten""
4. Aktivieren Sie ""Ja""
5. Speichern

**Hinweis:** Administratorberechtigungen erforderlich. Nach Änderung können andere Geräte über `http://IhreIP:8080` zugreifen

## Chatverlauf

### F: Wie sehe ich frühere Konversationen?

**Methode:**
1. Gehen Sie zur ""Beings""-Seite
2. Klicken Sie auf das Being, das Sie anzeigen möchten
3. Finden Sie den Link ""Chatverlauf"" in den Details
4. Klicken Sie, um alle historischen Sitzungen zu durchsuchen

### F: Wie lösche ich Konversationsverlauf?

**A:** Das System bietet derzeit keine Funktionalität zum Löschen des Konversationsverlaufs. Konversationsverlauf wird automatisch gespeichert, damit Silicon Beings sich an frühere Konversationen erinnern können.

## Daten und Speicherung

### F: Wo werden Daten gespeichert?

**A:** Standardmäßig werden sie im `data`-Ordner unter dem Programmlaufzeitverzeichnis gespeichert.

### F: Wie sichere ich Daten?

**Methode:** Einfach den gesamten `data`-Ordner an einen sicheren Ort kopieren.

### F: Wie migriere ich auf einen neuen Computer?

**Schritte:**
1. Schließen Sie das System
2. Kopieren Sie den gesamten `data`-Ordner
3. Installieren Sie das System auf dem neuen Computer
4. Legen Sie den `data`-Ordner im Programmverzeichnis des neuen Computers ab
5. Starten Sie das System

## Konfigurationsdatei

### F: Wo ist die Konfigurationsdatei?

**A:** Die `config.json`-Datei im Programmlaufzeitverzeichnis.

### F: Kann ich die Konfigurationsdatei direkt bearbeiten?

**A:** Ja, aber nicht empfohlen. Es wird empfohlen, über die Konfigurationsseite im Web-Interface zu modifizieren, was sicherer und weniger fehleranfällig ist.

### F: Was tun, wenn ich einen Fehler in der Konfiguration gemacht habe?

**Lösung:**
1. Schließen Sie das System
2. Löschen Sie die `config.json`-Datei
3. Starten Sie das System neu (erstellt automatisch Standardkonfiguration)

**Oder:** Wenn Sie eine Sicherung haben, können Sie die gesicherte Konfigurationsdatei wiederherstellen.

## Leistungsprobleme

### F: Das System läuft langsam?

**Vorschläge:**
- Verwenden Sie lokale KI-Dienste (wie Ollama)
- Wählen Sie leichtere KI-Modelle
- Reduzieren Sie die Anzahl gleichzeitig laufender Aufgaben

### F: Hohe Speicherauslastung?

**Vorschläge:**
- Verwenden Sie leichtere KI-Modelle
- Bereinigen Sie regelmäßig unnötige Daten

## Hilfe erhalten

### F: Was tun bei Problemen?

**Empfohlene Schritte:**
1. **Hilfedokumentation anzeigen**: Klicken Sie auf das ""❓ Hilfe""-Symbol links
2. **Protokolle anzeigen**: Überprüfen Sie Systemlaufzeitprotokolle auf der ""📝 Protokolle""-Seite
3. **System neustarten**: Viele Probleme können durch Neustart gelöst werden

### F: Wie sehe ich Systemprotokolle?

**Methode:**
1. Klicken Sie auf das ""📝 Protokolle""-Symbol links
2. Durchsuchen Sie die Protokollliste
3. Sie können nach Ebene filtern (Fehler, Warnungen usw.)

## Andere Fragen

### F: Welche Sprachen unterstützt das System?

**A:** Unterstützt Chinesisch (vereinfacht), Chinesisch (traditionell), Englisch, Japanisch, Koreanisch, Deutsch, Spanisch und mehrere andere Sprachen.

### F: Brauche ich Internetverbindung, um es zu verwenden?

**A:** Es hängt vom verwendeten KI-Dienst ab:
- **Lokale KI (wie Ollama)**: Keine Internetverbindung erforderlich
- **Cloud-KI (wie OpenAI)**: Internetverbindung erforderlich

### F: Ist das System sicher?

**A:** Ja. Das System hat eingebaute Berechtigungsmanagement-Mechanismen. Alle KI-Operationen durchlaufen die Berechtigungsprüfung, und sensible Operationen werden Ihre Bestätigung anfordern.

### F: Kann ich Funktionen anpassen?

**A:** Das System unterstützt die Erweiterung von Funktionalität durch Code-Schreiben, aber dies erfordert einige Programmierkenntnisse. Regelmäßigen Benutzern wird empfohlen, die vom System bereitgestellten Funktionen zu verwenden.
";

    public override string Memory => @"
# Speichersystem

## Was ist das Speichersystem?

Das Speichersystem protokolliert den gesamten Aktivitätsverlauf von Silicon Beings, einschließlich Konversationen, Tool-Aufrufen, Systemereignissen usw. Durch das Speichersystem können Sie verstehen, was das Silicon Being getan hat, wann es es getan hat und was die Ergebnisse waren.

## Wie greife ich auf das Speichersystem zu?

Zugriff über die Being-Seite:

1. Klicken Sie auf das **🧠 Beings**-Symbol links
2. Klicken Sie auf die Silicon Being-Karte, die Sie anzeigen möchten
3. Finden Sie den Link ""Speicher"" in den rechten Details
4. Klicken Sie, um zur Speicherseite zu gelangen

## Beschreibung der Speicherseite

### Seitenlayout

- **Oben**: Being-Auswahl und Statistiken
- **Filterbereich**: Filterbedingungen für Typ, Zeit, Schlüsselwörter usw.
- **Listenbereich**: Zeigt die Speicher-Eintragsliste an
- **Detailbereich**: Zeigt detaillierten Inhalt nach dem Klicken auf einen Speichereintrag an

### Speichertypen

Das System protokolliert die folgenden Speichertypen:

- **Konversation**: Konversationen zwischen Benutzer und KI
- **Tool-Aufruf**: Ausführungsprotokolle der KI beim Aufrufen von Tools
- **Systemereignis**: Wichtige Ereignisse während des Systembetriebs
- **Zusammenfassung**: Komprimierte Zusammenfassungen von Konversationen oder Ereignissen

## Speicher anzeigen

### Speicherliste durchsuchen

1. Wählen Sie das Being aus, das Sie anzeigen möchten
2. Die Seite wird die Speicherliste für dieses Being anzeigen
3. Jeder Speicher zeigt:
   - Typ-Symbol
   - Inhaltszusammenfassung
   - Zeit
   - Status (Erfolg/Fehlschlag)

### Speicherdetails anzeigen

Klicken Sie auf einen beliebigen Speichereintrag, um Folgendes anzuzeigen:
- Vollständiger Inhalt
- Zeitstempel
- Verwandte Parameter
- Ausführungsergebnis (wenn es ein Tool-Aufruf ist)

### Originalkontext nachverfolgen

Für bestimmte Speichereinträge bietet das System eine ""Nachverfolgung""-Funktion:
1. Klicken Sie auf die ""Nachverfolgung""-Taste in den Speicherdetails
2. Das System wird den vollständigen Kontext anzeigen, als dieser Speicher stattfand
3. Hilft Ihnen zu verstehen, warum die KI dies zu dieser Zeit getan hat

## Speicher filtern

### Nach Typ filtern

Klicken Sie auf den Typfilter, um den anzuzeigenden Speichertyp auszuwählen:
- Nur Konversationen
- Nur Tool-Aufrufe
- Nur Systemereignisse
- Nur Zusammenfassungen

### Nach Zeit filtern

Sie können einen Zeitbereich auswählen:
- Startdatum eingeben
- Enddatum eingeben
- Nur Speicher innerhalb dieses Zeitraums anzeigen

### Schlüsselwortsuche

Geben Sie Schlüsselwörter in das Suchfeld ein:
- Unterstützt Chinesisch und Englisch
- Wird den vollständigen Inhalt von Speichern durchsuchen
- Zeigt automatisch übereinstimmende Ergebnisse nach Eingabe an

**Suchtipps:**
- Verwendung spezifischer Schlüsselwörter macht es einfacher, Ergebnisse zu finden
- Kann mit Typ- und Zeitfiltern kombiniert werden
- Wenn es zu viele Ergebnisse gibt, versuchen Sie spezifischere Schlüsselwörter

### Zusammenfassung oder Originalprotokolle anzeigen

- **Alle anzeigen**: Zeigt alle Speicher an
- **Nur Zusammenfassung**: Zeigt nur komprimierte Zusammenfassungsprotokolle an
- **Nur Original**: Zeigt nur originale detaillierte Protokolle an

## Speicherstatistiken

Der obere Teil der Seite zeigt statistische Informationen an:
- Gesamte Anzahl von Speichern
- Anzahl der Speicher nach Typ
- Speichernutzung

Durch diese Statistiken können Sie verstehen:
- Aktivitätsniveau des Beings
- Haupttypen von Aktivitäten
- Ob alte Speicher bereinigt werden müssen

## Paginiertes Durchsuchen

Wenn es viele Speicher gibt, wird das System sie seitenweise anzeigen:
- Standardmäßig 20 Protokolle pro Seite
- Verwenden Sie Seitenzahl-Tasten zum Umblättern
- Kann die Anzahl der pro Seite angezeigten Protokolle anpassen

## Häufige Fragen

### F: Wie finde ich eine bestimmte Konversation?

**Methode:**
1. Geben Sie Schlüsselwörter aus der Konversation in das Suchfeld ein
2. Wählen Sie ""Konversation"" im Typfilter
3. Wenn Sie die ungefähre Zeit kennen, können Sie den Zeitbereich einstellen
4. Durchsuchen Sie die Suchergebnisse

### F: Was tun, wenn Speicher zu viel Platz beanspruchen?

**Vorschläge:**
- Speicher werden automatisch verwaltet und erfordern normalerweise kein manuelles Eingreifen
- Das System erstellt Zusammenfassungen, um historische Protokolle zu komprimieren
- Wenn wirklich notwendig, können Sie den Systemadministrator kontaktieren

### F: Kann ich Speicher löschen?

**A:** Das System bietet keine Funktionalität zum Löschen von Speichern. Speicher sind wichtige Historie für Silicon Beings, und das Behalten von Speichern hilft der KI, besser zu verstehen und Fragen zu beantworten.

### F: Kann ich Speicher exportieren?

**A:** Die aktuelle Version unterstützt keine Exportfunktionalität. Speicherdaten werden im Systemdatenverzeichnis gespeichert.

### F: Warum sind einige Speicher ""Zusammenfassungen""?

**A:** Das System komprimiert automatisch längere Konversationen oder Ereignisse in Zusammenfassungen, um Speicherplatz zu sparen und die Abfrageeffizienz zu verbessern. Zusammenfassungen behalten Schlüsselinformationen, lassen aber Details aus.

### F: Wie sehe ich detaillierte Informationen über KI-Tool-Aufrufe?

**Methode:**
1. Wählen Sie ""Tool-Aufruf"" im Typfilter
2. Finden Sie den entsprechenden Tool-Aufruf-Protokoll
3. Klicken Sie, um detaillierte Informationen anzuzeigen
4. Sie können Tool-Name, Parameter, Ausführungsergebnisse usw. sehen

### F: Speichersuche ergibt keine Ergebnisse?

**Vorschläge:**
1. Überprüfen Sie, ob das Schlüsselwort korrekt ist
2. Versuchen Sie verschiedene Schlüsselwörter
3. Überprüfen Sie, ob der Zeitbereich korrekt eingestellt ist
4. Bestätigen Sie, dass das ausgewählte Being korrekt ist
5. Versuchen Sie, alle Speicher anzuzeigen, ohne Filter einzustellen

## Verwendungsvorschläge

1. **Regelmäßig überprüfen**: Verstehen Sie den Aktivitätsstatus von Silicon Beings
2. **Filter nutzen**: Schnell die benötigten Informationen lokalisieren
3. **Nachverfolgung verwenden**: Verstehen Sie den Entscheidungsprozess der KI
4. **Auf Statistiken achten**: Verstehen Sie den Systembetriebsstatus

## Technische Hinweise

### Datenspeicherung

Speicherdaten werden im Systemdatenverzeichnis gespeichert:
```
data/
  beings/
    {BeingID}/
      memory/
        (Speicherdateien)
```

### Automatische Verwaltung

Das System wird automatisch:
- Wichtige Aktivitäten protokollieren
- Konversationszusammenfassungen erstellen
- Zeitindizes pflegen
- Abfrageleistung optimieren

Sie müssen Speicher nicht manuell verwalten; das System wird alles handhaben.
";

    public override string OllamaSetup => @"
# Ollama-Installation und Modell-Download

## Was ist Ollama?

Ollama ist ein Open-Source-Lokalki-Modell-Laufzeitwerkzeug, das es Ihnen ermöglicht, große Sprachmodelle auf Ihrem eigenen Computer auszuführen, ohne Internetverbindung (nach dem Download des Modells).

**Vorteile:**
- Läuft komplett lokal, schützt Privatsphäre
- Unterstützt mehrere KI-Modelle
- Einfach zu installieren und zu verwenden
- Kostenlos und Open-Source

## Ollama herunterladen und installieren

### Windows-System

**Schritt 1: Installationspaket herunterladen**

Besuchen Sie die Ollama-Website-Downloadseite:
- URL: https://ollama.com/download
- Lädt automatisch das Windows-Installationspaket herunter (ollama-setup.exe)

**Schritt 2: Installationsprogramm ausführen**

1. Doppelklicken Sie auf die heruntergeladene `ollama-setup.exe`-Datei
2. Folgen Sie den Installationsassistenten-Hinweisen, um die Installation abzuschließen
3. Nach der Installation wird Ollama automatisch starten

**Schritt 3: Installation überprüfen**

1. Öffnen Sie die Eingabeaufforderung (drücken Sie `Win + R`, geben Sie `cmd` ein, drücken Sie Enter)
2. Geben Sie den folgenden Befehl ein:
   ```
   ollama --version
   ```
3. Wenn eine Versionsnummer angezeigt wird, war die Installation erfolgreich

### Mac-System

**Methode 1: Installationspaket herunterladen**

1. Besuchen Sie https://ollama.com/download
2. Laden Sie das Mac-Installationspaket herunter
3. Doppelklicken Sie auf das Installationspaket, ziehen Sie es in den Anwendungsordner

**Methode 2: Installation mit Terminal**

Öffnen Sie Terminal und geben Sie ein:
```bash
brew install ollama
```

**Installation überprüfen:**
```bash
ollama --version
```

### Linux-System

**Ein-Befehls-Installation:**

Öffnen Sie Terminal und führen Sie aus:
```bash
curl -fsSL https://ollama.com/install.sh | sh
```

**Installation überprüfen:**
```bash
ollama --version
```

## Ollama starten

### Windows

- Ollama startet automatisch nach der Installation
- Sie können das Ollama-Symbol im Systemtray (unten rechts) sehen
- Rechtsklick auf das Symbol zum Verwalten

### Mac / Linux

Im Terminal ausführen:
```bash
ollama serve
```

Oder direkt ausführen:
```bash
ollama
```

Dies wird ein interaktives Menü öffnen.

## Modelle herunterladen und ausführen

### Was ist ein Modell?

Ein Modell ist das ""Gehirn"" der KI, das die Fähigkeiten der KI bestimmt. Verschiedene Modelle haben unterschiedliche Merkmale:
- **Verschiedene Größen**: Größere Modelle sind leistungsfähiger, benötigen aber mehr Speicher
- **Verschiedene Spezialgebiete**: Einige sind gut im Konversieren, andere im Programmieren

### Modell-Intelligenz (B-Einheit)

Die ""Intelligenz"" von KI-Modellen wird normalerweise in **B (Billion, Milliarden Parameter)** gemessen:
- **7B-8B**: Grundlegendes Niveau, kann einfache Aufgaben erledigen, kann aber in komplexen Szenarien schlecht abschneiden
- **13B-14B**: Mittleres Niveau, leistet gute Arbeit bei den meisten täglichen Aufgaben
- **32B und höher**: Höheres Niveau, stärkere komplexe Argumentation und Langtextverständnis

**Dieses System empfiehlt die Verwendung von Modellen über 8B** für eine bessere Benutzererfahrung.

### Lokale Modelle vs. Cloud-Modelle

Ollama unterstützt zwei Modellausführungsmethoden:

**Lokale Modelle:**
- Modelldateien werden auf Ihren Computer heruntergeladen
- Läuft komplett lokal, kein Internet erforderlich (nach Download)
- Beschränkt durch Ihre Hardwarekonfiguration (Speicher, Grafikkarte)
- Normalerweise 4B-70B Parameter
- Kostenlos zu verwenden, keine Einschränkungen

**Cloud-Modelle:**
- Modelle laufen auf Ollama-Cloud-Servern
- Müssen nur Modellidentifikator herunterladen (sehr klein)
- Können ultragroße Modelle ausführen, die Heimcomputer nicht handhaben können (normalerweise 200B+)
- Erfordert Internetverbindung
- Hat Nutzungskontingenteinschränkungen (**wöchentlich aktualisiert**)
- Aktivieren Sie die Cloud-Funktionen des Ollama-Clients zur Verwendung

### Hardwarekonfigurations-Empfehlungen

### Empfohlene Modelle

Hier sind häufig verwendete kostenlose Modelle:

| Modellname | Intelligenz | Größe | Merkmale | Geeignete Szenarien |
|-----------|--------------|------|----------|-------------------|
| **qwen3.5:8b** | 8B | Etwa 4-5GB | Starke chinesische Fähigkeiten, gute Gesamtleistung | Tägliche Konversation, Schreiben, Übersetzung |
| **qwen3.5:14b** | 14B | Etwa 8-9GB | Stärkere chinesische Fähigkeiten, verbesserte Argumentation | Komplexe Aufgaben, Langtextverarbeitung |
| **qwen3.5:32b** | 32B | Etwa 18-20GB | Hohe Intelligenz, ausgezeichnete komplexe Argumentation | Professionelle Aufgaben, tiefgehende Analyse |
| **llama3:8b** | 8B | Etwa 4-5GB | Starke englische Fähigkeiten, gute Vielseitigkeit | Englische Konversation, allgemeine Aufgaben |
| **llama3:70b** | 70B | Etwa 40GB | Ultrahohe Intelligenz, erstklassig auf Englisch | Schwierige englische Aufgaben |
| **gemma3:4b** | 4B | Etwa 2-3GB | Leichtgewichtig, schnelle Geschwindigkeit | Schnelle Antwort, Low-Spec-Computer |
| **gemma3:12b** | 12B | Etwa 7-8GB | Balance Leistung und Ressourcen | Tägliche Nutzung |
| **mistral:7b** | 7B | Etwa 4GB | Balance Leistung und Geschwindigkeit | Allgemeine Szenarien |
| **codellama:7b** | 7B | Etwa 4GB | Gut im Programmieren | Code-Generierung, Debugging |
| **codellama:13b** | 13B | Etwa 7-8GB | Stärkere Programmierfähigkeiten | Komplexe Code-Aufgaben |

**Empfohlen für chinesische Benutzer: qwen3.5:8b oder qwen3.5:14b**

### Modelle herunterladen

**Methode 1: Befehlszeilen-Download**

Öffnen Sie Terminal (oder Eingabeaufforderung) und geben Sie ein:

```bash
ollama pull qwen3.5
```

Das System wird das Modell automatisch herunterladen, was einige Zeit in Anspruch nimmt (abhängig von Netzwerkgeschwindigkeit und Modellgröße).

**Methode 2: Ausführen und automatisch herunterladen**

```bash
ollama run qwen3.5
```

Wenn das Modell nicht heruntergeladen ist, wird es automatisch mit dem Download beginnen.

### Modelle ausführen

Nach dem Download führen Sie das Modell aus:

```bash
ollama run qwen3.5
```

Dies wird eine interaktive Konversationsoberfläche öffnen, wo Sie direkt mit der KI chatten können.

**Beispielkonversation:**
```
>>> Hallo!
Hallo! Ich bin Qwen, wie kann ich Ihnen helfen?

>>> Bitte schreiben Sie ein Gedicht über den Frühling
Frühlingswind weht, Blumen blühen,
Grün und duftend füllt den Garten.
Schwalben kehren zurück, um alte Nester zu finden,
April ist gute Zeit in der Welt.
```

Drücken Sie `Ctrl + D` oder geben Sie `/bye` ein, um die Konversation zu beenden.

### Heruntergeladene Modelle anzeigen

```bash
ollama list
```

Wird eine Liste aller heruntergeladenen Modelle anzeigen.

### Nicht benötigte Modelle löschen

```bash
ollama rm qwen3.5
```

## Ollama in Silicon Life verwenden

### Verbindung konfigurieren

1. Stellen Sie sicher, dass Ollama gestartet und läuft
2. Öffnen Sie das Silicon Life-System
3. Gehen Sie zur Seite **⚙ Konfiguration**
4. Finden Sie ""KI-Client-Typ"" und wählen Sie `OllamaClient`
5. In ""KI-Konfiguration"" einstellen:
   - **endpoint**: `http://localhost:11434` (Standard)
   - **model**: `qwen3.5` (oder andere Modelle, die Sie heruntergeladen haben)
6. Konfiguration speichern

### Verbindung testen

1. Gehen Sie zur Seite **💬 Chat**
2. Wählen Sie ein Silicon Being
3. Senden Sie eine Nachricht
4. Wenn Sie eine Antwort erhalten, war die Verbindung erfolgreich

## Häufige Fragen

### F: Ollama-Download ist sehr langsam?

**Lösung:**
- Modelldateien sind normalerweise groß (2-8GB), Download benötigt Zeit
- Stellen Sie stabile Netzwerkverbindung sicher
- Können nachts oder wenn das Netzwerk leer ist herunterladen

### F: Was tun, wenn Download unterbrochen wird?

**Lösung:**
Führen Sie den Download-Befehl erneut aus, er wird fortsetzen:
```bash
ollama pull qwen3.5
```

### F: Wie weiß ich, welche Modellgröße mein Computer ausführen kann?

**Speicher- und Modellgrößen-Empfehlungen:**
- **4GB RAM**: Empfehlen Sie Modelle unter 2GB (etwa 2B-3B)
- **8GB RAM**: Kann 4GB-Modelle ausführen (etwa 7B-8B)
- **16GB RAM**: Kann 8GB-Modelle ausführen (etwa 13B-14B)
- **32GB RAM**: Kann 16GB-Modelle ausführen, aber wird spürbare Verzögerung und erhöhte Erwärmung erfahren (etwa 32B)
- **64GB und höher**: Kann größere Modelle flüssig ausführen

**Wichtiger Hinweis:**
- Laptops mit 32GB RAM, die Modelle um 16B ausführen, werden **spürbare Verzögerung** und **erhöhte Erwärmung** erfahren
- Dies ist keine Fehlfunktion, sondern normales Verhalten aufgrund unzureichender Hardware-Ressourcen
- **Empfehlung**: In diesem Fall wählen Sie kleinere Modelle (8B-14B) oder aktualisieren Sie auf höherwertige Hardware

**Beginnen Sie mit dem Testen von leichten Modellen**, und wenn es flüssig läuft, versuchen Sie größere Modelle.

### F: Was tun, wenn Ollama nicht startet?

**Überprüfen:**
1. Ist Port 11434 von einem anderen Programm belegt?
2. Installieren Sie Ollama neu
3. Überprüfen Sie Ollama-Protokolle auf Fehlerinformationen

### F: Was tun, wenn das Modell langsam läuft?

**Vorschläge:**
- Verwenden Sie ein kleineres Modell (z. B. gemma3 statt qwen3.5)
- Schließen Sie andere speicherintensive Programme
- Überprüfen Sie, ob die Computerkonfiguration den Anforderungen entspricht

### F: Kann ich mehrere Modelle gleichzeitig in Silicon Life verwenden?

**A:** Ja. Laden Sie mehrere Modelle in Ollama herunter, dann wählen Sie verschiedene Modelle für verschiedene Beings in der Silicon Life Being-KI-Konfiguration.

### F: Benötigt Ollama Internetverbindung?

**A:** 
- **Beim Herunterladen von Modellen**: Internetverbindung erforderlich
- **Beim Ausführen von Modellen**: Kein Internet erforderlich (Modell ist lokal heruntergeladen)

### F: Wie viel Festplattenspeicher belegen Modelle?

**A:** 
- Kleine Modelle: Etwa 2-4GB
- Mittlere Modelle: Etwa 4-8GB
- Große Modelle: 8GB und mehr

Empfehlen Sie, ausreichenden Festplattenspeicher freizuhalten.

## Weitere Hilfe erhalten

- **Ollama-Website**: https://ollama.com
- **Ollama-Dokumentation**: https://docs.ollama.com

## Nächste Schritte

Nach der Installation von Ollama und dem Download von Modellen können Sie:
- Lokale KI in Silicon Life konfigurieren und verwenden
- Vollständig lokale KI-Dienste genießen
- Ihre Privatsphäre und Datensicherheit schützen

Viel Spaß bei der Verwendung!
";

    public override string BailianDashScope => @"
# Leitfaden für die Alibaba Cloud Bailian-Plattform

## Was ist Alibaba Cloud Bailian?

Alibaba Cloud Bailian (DashScope) ist eine große Modellserviceplattform von Alibaba Cloud, die mehrere hochwertige KI-Modelle bietet, einschließlich Tongyi Qianwen, DeepSeek, GLM, Kimi usw.

**Vorteile:**
- Hohe Modellintelligenz (bis zu hunderten von B)
- Keine lokale Hardware erforderlich, läuft in der Cloud
- Unterstützt mehrere erstklassige KI-Modelle
- Pay-per-Use, Kosten kontrollierbar
- Kompatibel mit OpenAI API-Format

## Registrierung und Service-Aktivierung

### Schritt 1: Alibaba Cloud-Konto registrieren

1. Besuchen Sie die Alibaba Cloud-Website: https://www.aliyun.com
2. Klicken Sie auf ""Kostenlos registrieren""
3. Schließen Sie die Registrierung nach den Anweisungen ab (unterstützt Telefonnummer, E-Mail-Registrierung)
4. Schließen Sie die Echtzeit-Authentifizierung ab (erfordert Alipay oder Bankkarte)

### Schritt 2: Bailian-Service aktivieren

1. Melden Sie sich bei der Alibaba Cloud Console an
2. Suchen Sie nach ""Bailian"" oder ""DashScope""
3. Klicken Sie, um die Bailian-Produktseite zu betreten
4. Klicken Sie auf ""Jetzt aktivieren""
5. Lesen und vereinbaren Sie die Servicevereinbarung
6. Schließen Sie die Aktivierung ab

### Schritt 3: API-Schlüssel erhalten

1. Betreten Sie die Bailian Console
2. Finden Sie ""API Key Management"" oder ""Key Management"" im linken Menü
3. Klicken Sie auf ""API Key erstellen""
4. Benennen Sie den Schlüssel (z. B. ""SiliconLife"")
5. Kopieren und speichern Sie den API-Schlüssel (**wird nur einmal angezeigt, bitte ordnungsgemäß speichern**)

## Bailian in Silicon Life konfigurieren

### Konfigurationsschritte

1. Öffnen Sie das Silicon Life-System
2. Gehen Sie zur Seite **⚙ Konfiguration**
3. Finden Sie ""KI-Client-Typ"" und wählen Sie `DashScopeClient`
4. In ""KI-Konfiguration"" ausfüllen:
   - **API Key**: Fügen Sie den kopierten API-Schlüssel ein
   - **Region**: Wählen Sie Server-Region (z. B. beijing)
   - **Model**: Wählen Sie Modell (**nach dem Ausfüllen des API-Schlüssels und Auswählen der Region wird das System automatisch alle verfügbaren Modelle für diese Region abrufen**)
5. Konfiguration speichern

**Hinweis:**
- Sie müssen zuerst den API-Schlüssel ausfüllen und eine Region auswählen, bevor die Modell-Dropdown-Liste laden wird
- Wenn die Modellliste nicht lädt, wird eine empfohlene Modellliste angezeigt

### Region-Auswahl

Das System unterstützt die folgenden Regionen:

| Region | Standort | Beschreibung |
|--------|----------|-------------|
| **beijing** | Peking, China | Standardempfehlung, schnelle Inlandszugriffsgeschwindigkeit |
| **virginia** | Virginia, USA | Geeignet für Übersee-Benutzer |
| **singapore** | Singapur | Asien-Pazifik-Region |
| **hongkong** | Hongkong, China | Asien-Pazifik-Region |
| **frankfurt** | Frankfurt, Deutschland | Europa-Region |

**Regionsauswahl-Empfehlungen:**
- **Festlandchina-Benutzer**: Wählen Sie beijing (Peking), schnellste Zugriffsgeschwindigkeit, **aber schwächere Übersetzungsunterstützung**
- **Hochwertige Übersetzung benötigt**: Wählen Sie singapore (Singapur) oder hongkong (Hongkong), bessere Übersetzungsqualität, auch für Festlandchina-Benutzer zugänglich
- **Übersee-Benutzer**: Wählen Sie die Region, die Ihnen am nächsten ist

### Modell-Auswahl

Bei der Konfiguration des Modells wird das System automatisch verfügbare Modelle von der Bailian-Plattform abrufen. Wenn das Abrufen fehlschlägt, werden empfohlene Modelle angezeigt:

**Empfohlene Modelle:**

| Modellname | Intelligenz | Merkmale | Geeignete Szenarien |
|-----------|--------------|----------|-------------------|
| **qwen3-max** | Ultragroß | Stärkste Tongyi Qianwen-Version | Komplexe Argumentation, professionelle Aufgaben |
| **qwen3.6-plus** | Groß | Balance Leistung und Kosten | Tägliche Nutzung (empfohlen) |
| **qwen3.6-flash** | Mittel | Schnelle Geschwindigkeit, niedrige Kosten | Schnelle Antwort |
| **qwen-max** | Ultragroß | Vorgänger-Flaggschiff-Modell | Schwierige Aufgaben |
| **qwen-plus** | Groß | Ausgeglichene Leistung | Allgemeine Szenarien |
| **qwen-turbo** | Mittel | Schnellste Geschwindigkeit | Einfache Aufgaben |
| **qwen3-coder-plus** | Groß | Gut im Programmieren | Code-Generierung, Debugging |
| **qwq-plus** | Groß | Starke Argumentationsfähigkeit | Mathematik, logische Argumentation |
| **deepseek-v3.2** | Ultragroß | DeepSeek neueste Version | Starke umfassende Fähigkeit |
| **deepseek-r1** | Ultragroß | Spezialisiert auf Argumentation | Komplexe Argumentation |
| **glm-5.1** | Groß | Zhipu AI-Modell | Chinesische Szenarien |
| **kimi-k2.5** | Groß | Moonshot-Modell | Langtextverarbeitung |
| **llama-4-maverick** | Groß | Meta Open-Source-Modell | Englische Szenarien |

**Empfohlen für chinesische Benutzer: qwen3.6-plus oder qwen3-max**

## Kostenerklärung

### Abrechnungsmethode

Die Bailian-Plattform verwendet **Pay-per-Use**-Abrechnung:
- Abrechnung nach eingegebener Token-Anzahl
- Verschiedene Modelle haben verschiedene Preise
- Höhere Modellintelligenz, höherer Preis

### Freies Kontingent

- Neue Benutzer haben normalerweise kostenloses Testkontingent
- Einige Modelle haben kostenloses Aufrufkontingent
- Spezifisches Kontingent unterliegt den Ankündigungen der Bailian-Plattform

### Nutzung anzeigen

1. Melden Sie sich bei der Bailian Console an
2. Gehen Sie zu ""Nutzungsabfrage"" oder ""Abrechnungsmanagement""
3. Zeigen Sie Aufrufanzahl und Kosten an

### Kostenspar-Empfehlungen

- Wählen Sie geeignete Modelle (nicht unbedingt die teuersten)
- Vermeiden Sie das Senden zu langer Texte
- Überprüfen Sie regelmäßig die Nutzung, um Kosten zu kontrollieren

### Token-Nutzungsstatistik-Erklärung

**Wichtiger Hinweis:** Da die API-Antwort der Alibaba Cloud Bailian-Plattform (DashScope) **keine stabilen Token-Nutzungsfelder zurückgibt**, kann dieses System **keine Token-Nutzung** bei der Verwendung von Bailian-Modellen protokollieren.

**Das bedeutet:**
- Das System wird nicht anzeigen, wie viele Token verwendet wurden
- Kann historische Token-Verbrauchsprotokolle nicht im System anzeigen
- Kann keine Kostenanalyse basierend auf Token-Nutzung durchführen

**So zeigen Sie die Nutzung an:**
- Bitte melden Sie sich bei der Alibaba Cloud Bailian Console an, um tatsächliche Nutzung und Kosten anzuzeigen
- Bailian Console bietet detaillierte Aufrufstatistiken und Abrechnungsinformationen

## Häufige Fragen

### F: Wo erhalte ich den API-Schlüssel?

**A:** 
1. Melden Sie sich bei der Alibaba Cloud Bailian Console an
2. Finden Sie ""API Key Management""
3. Erstellen Sie einen neuen API-Schlüssel
4. Kopieren und speichern Sie ihn ordnungsgemäß

### F: Was tun, wenn der API-Schlüssel durchgesickert ist?

**A:**
1. Melden Sie sich sofort bei der Bailian Console an
2. Löschen Sie den durchgesickerten API-Schlüssel
3. Erstellen Sie einen neuen API-Schlüssel
4. Aktualisieren Sie die Konfiguration in Silicon Life

### F: Welche Region ist am besten zu wählen?

**A:**
- **Inlandsbenutzer**: Wählen Sie beijing (Peking), schnellste Geschwindigkeit
- **Übersee-Benutzer**: Wählen Sie die Region, die Ihnen am nächsten ist
- Region beeinflusst nicht die Modellqualität, nur die Zugriffsgeschwindigkeit

### F: Warum lädt die Modellliste nicht?

**Mögliche Gründe:**
1. API-Schlüssel ist inkorrekt oder abgelaufen
2. Netzwerkverbindungsproblem
3. Bailian-Service-Ausnahme

**Lösungen:**
1. Überprüfen Sie, ob der API-Schlüssel korrekt ist
2. Überprüfen Sie die Netzwerkverbindung
3. Versuchen Sie es später erneut

### F: Kann ich mehrere Modelle verwenden?

**A:** Ja. In Silicon Life können Sie verschiedene Bailian-Modelle für verschiedene Silicon Beings konfigurieren.

### F: Was ist der Unterschied zwischen Bailian und Ollama?

| Merkmal | Bailian (DashScope) | Ollama |
|---------|---------------------|--------|
| Ausführungsort | Cloud | Lokaler Computer |
| Hardwareanforderungen | Keine | Erfordert höhere Konfiguration |
| Modellgröße | Bis zu hunderten von B | Normalerweise 4B-70B |
| Kosten | Pay-per-Use | Kostenlos |
| Internet | Muss verbunden sein | Nicht erforderlich nach Download |
| Privatsphäre | Daten werden an Cloud gesendet | Vollständig lokal |

**Auswahl-Empfehlungen:**
- Niedrige Hardwarekonfiguration → Wählen Sie Bailian
- Ultrahohe Intelligenz-Modelle benötigt → Wählen Sie Bailian
- Privatsphäre wichtig, kein Geld ausgeben → Wählen Sie Ollama
- Verwendung in Offline-Umgebung → Wählen Sie Ollama

### F: Was tun, wenn Aufruf fehlschlägt?

**Überprüfen:**
1. Ist API-Schlüssel korrekt?
2. Ist Kontoguthaben ausreichend?
3. Ist kostenloses Kontingent überschritten?
4. Überprüfen Sie Fehlerinformationen in Bailian Console

### F: Wie kontrolliere ich Kosten?

**Vorschläge:**
1. Setzen Sie Budget-Warnungen (Bailian Console)
2. Überprüfen Sie regelmäßig die Nutzung
3. Wählen Sie kosteneffektive Modelle
4. Vermeiden Sie häufige Aufrufe großer Modelle

## Best Practices

### 1. Geeignete Modelle wählen

- **Tägliche Konversation**: qwen3.6-plus (ausgeglichen)
- **Komplexe Argumentation**: qwen3-max oder deepseek-r1
- **Programmieraufgaben**: qwen3-coder-plus
- **Schnelle Antwort**: qwen3.6-flash oder qwen-turbo

### 2. API-Schlüssel verwalten

- Ordnungsgemäß aufbewahren, nicht öffentlich teilen
- Regelmäßig rotieren (neue erstellen, alte löschen)
- Erstellen Sie verschiedene Schlüssel für verschiedene Zwecke

### 3. Nutzung überwachen

- Überprüfen Sie die Nutzung einmal pro Woche
- Setzen Sie Budget-Warnungen
- Untersuchen Sie abnormale Nutzung umgehend

### 4. Nutzung optimieren

- Eingabeinhalt straffen, unnötigen Text reduzieren
- Konversationsverlauflänge vernünftig einstellen
- Verwenden Sie geeignete Modelle, streben Sie nicht blind nach großen Modellen

## Weitere Hilfe erhalten

- **Bailian-Website**: https://bailian.console.aliyun.com
- **API-Dokumentation**: https://help.aliyun.com/zh/model-studio
- **Modellliste**: https://help.aliyun.com/zh/model-studio/models
- **Preisgestaltung**: https://www.aliyun.com/price/product#/dashscope
- **Technischer Support**: Ticket einreichen oder Alibaba Cloud-Kundenservice kontaktieren

## Nächste Schritte

Nach der Konfiguration von Bailian können Sie:
- Hochwertige Cloud-KI-Modelle in Silicon Life verwenden
- Ultrahohe Intelligenz-KI-Dienste erleben
- Sich keine Sorgen über lokale Hardwarekonfiguration machen

Viel Spaß bei der Verwendung!
";
    
    public override string AIClients => @"
# KI-Client-Konfiguration

## Übersicht

KI-Clients sind die ""Brain-Connectors"" des Silicon Life-Systems und verantwortlich für die Kommunikation mit künstlichen Intelligenzmodellen. Das System unterstützt mehrere KI-Clients, und Sie können den appropriate Service basierend auf Ihren Bedürfnissen auswählen.

## Unterstützte KI-Clients

Das System unterstützt mehrere KI-Clients, und Sie können den appropriate Service basierend auf Ihren Bedürfnissen auswählen.

### Lokaler KI-Client

**Merkmale:**
- 🏠 **Lokaler Betrieb**: KI-Modelle laufen auf Ihrem Computer
- 🔒 **Datenschutz und Sicherheit**: Daten werden nicht in die Cloud hochgeladen
- 💰 **Völlig kostenlos**: Keine Nutzungsbeschränkungen
- ⚡ **Schnelle Antwort**: Keine Netzwerklatenz (lokale Modelle)

**Geeignete Szenarien:**
- Datenschutzorientierte Benutzer
- Bessere Computerkonfiguration (empfohlen 16GB+ RAM)
- Unbegrenzte KI-Nutzung
- Schlechte Netzwerkbedingungen

**Konfigurationsanforderungen:**
- Muss entsprechende KI-Service-Software installieren
- Muss KI-Modelldateien herunterladen (normalerweise 4-20GB)
- Empfohlen 16GB+ RAM, dedizierte Grafikkarte ist besser

### Cloud-KI-Client

**Merkmale:**
- ☁️ **Cloud-Service**: KI-Modelle laufen auf Remote-Servern
- 🚀 **Leistungsstark**: Kann ultragroße Modelle verwenden (200B+ Parameter)
- 💳 **Pay-per-use**: Hat Freikontingent, nach Überschreiten wird nach Nutzung abgerechnet
- 🌍 **Multi-Region-Unterstützung**: Kann näher gelegene Server auswählen

**Geeignete Szenarien:**
- Niedrigere Computerkonfiguration
- Muss High-End-Modelle verwenden
- Gelegentliche Nutzung, geringe Nutzung
- Will schnell starten ohne lokale Konfiguration

**Konfigurationsanforderungen:**
- Benötigt Konto und API-Schlüssel für entsprechende Plattform
- Benötigt Internetverbindung
- Hat Nutzungslimit (kann periodisch aktualisieren)

## Wie wählt man einen KI-Client?

### Auswahl-Flowchart

```
Wie ist Ihre Computerkonfiguration?
├─ Hohe Konfiguration (16GB+ RAM)
│  └─ Datenschutzorientiert?
│     ├─ Ja → Wählen Sie lokalen Client (z.B. Ollama)
│     └─ Nein → Beides ist möglich
└─ Niedrige Konfiguration (8GB oder weniger)
   └─ Wählen Sie Cloud-Client (z.B. DashScope)
```

### Vergleichstabelle

| Merkmal | Lokaler Client | Cloud-Client |
|------|--------------|-----------------|
| Installationsschwierigkeit | Mittel (muss Software und Modelle installieren) | Einfach (nur API-Schlüssel benötigt) |
| Betriebskosten | Kostenlos (verbraucht Strom) | Hat Freikontingent, nach Überschreiben Abrechnung |
| Datenschutz | ⭐⭐⭐⭐⭐ Völlig lokal | ⭐⭐⭐ Daten gehen durch Cloud |
| Modellauswahl | Begrenzt durch Computerkonfiguration | Kann verschiedene große Modelle auswählen |
| Netzwerkanforderung | Nur beim Herunterladen von Modellen benötigt | Immer benötigt |
| Antwortgeschwindigkeit | Schnell (lokal) | Hängt vom Netzwerk ab |
| Nutzungsbeschränkungen | Keine Beschränkungen | Hat Kontingentbeschränkungen |

## KI-Client konfigurieren

### Schritt 1: Konfigurationsseite betreten

1. Öffnen Sie das Silicon Life-System
2. Klicken Sie auf das **⚙ Konfiguration**-Menü in der oberen Navigationsleiste

### Schritt 2: KI-Client-Typ auswählen

1. Finden Sie die Option **""KI-Client-Typ""** auf der Konfigurationsseite
2. Wählen Sie Ihren gewünschten Client aus dem Dropdown-Menü:
   - Lokaler Client (z.B. `OllamaClient`)
   - Cloud-Client (z.B. `DashScopeClient`)

### Schritt 3: Konfigurationsinformationen ausfüllen

Nach der Auswahl des Clients erscheinen unten entsprechende Konfigurationselemente:

#### Lokaler Client-Konfiguration (z.B. Ollama)

| Konfigurationselement | Beschreibung | Standardwert | Beispiel |
|--------|------|--------|------|
| **endpoint** | KI-Service-Adresse | Abhängig vom spezifischen Service | z.B. `http://localhost:11434` |
| **model** | Zu verwendendes Modell | Abhängig vom spezifischen Service | z.B. `qwen3.5:8b` |
| **temperature** | Kreativitätsniveau (0-1) | `0.7` | `0.5` konservativer, `0.9` kreativer |
| **maxTokens** | Maximale Antwortlänge | `2048` | `4096` erlaubt längere Antworten |

**Konfigurationsbeispiel:**
```
KI-Client-Typ: Lokaler Client-Typ (z.B. OllamaClient)
endpoint: http://localhost:11434 (gemäß tatsächlicher Service-Adresse ausfüllen)
model: qwen3.5:8b (gemäß heruntergeladenem Modell ausfüllen)
temperature: 0.7
maxTokens: 2048
```

#### Cloud-Client-Konfiguration (z.B. DashScope)

| Konfigurationselement | Beschreibung | Standardwert | Beispiel |
|--------|------|--------|------|
| **apiKey** | API-Schlüssel | Keiner | Von spezifischer Plattform bereitgestellt |
| **region** | Server-Region | Abhängig von spezifischer Plattform | z.B. `beijing` |
| **model** | Zu verwendendes Modell | Abhängig von spezifischer Plattform | z.B. `qwen3.6-plus` |
| **temperature** | Kreativitätsniveau (0-1) | `0.7` | `0.5` |
| **maxTokens** | Maximale Antwortlänge | `2048` | `4096` |

**Verfügbare Regionen:**

| Regionscode | Ort | Geeignete Benutzer |
|---------|------|---------|
| `beijing` | Peking, China | Benutzer in Festlandchina (empfohlen) |
| `singapore` | Singapur | Benutzer in Südostasien |
| `hongkong` | Hongkong, China | Benutzer in Hongkong, Macao, Taiwan |
| `virginia` | USA | Nordamerikanische Benutzer |
| `frankfurt` | Deutschland | Europäische Benutzer |

**Verfügbare Modelle:**

| Modellname | Merkmale | Geeignete Szenarien |
|---------|------|---------|
| `qwen3.6-plus` | Ausgewogene Leistung (empfohlen) | Täglicher Gebrauch |
| `qwen3-max` | Stärkste Fähigkeit | Komplexe Aufgaben |
| `qwen3.6-flash` | Schnelle Antwort | Einfache Fragen und Antworten |
| `qwen-max` | Flaggschiff der vorherigen Generation | Komplexe Schlussfolgerung |
| `qwen-plus` | Verstärkt der vorherigen Generation | Allgemeine Szenarien |
| `qwen-turbo` | Schnell der vorherigen Generation | Einfache Aufgaben |
| `qwen3-coder-plus` | Programmierung spezifisch | Code-Generierung |
| `qwq-plus` | Schlussfolgerung spezifisch | Mathematik, Logik |
| `deepseek-v3.2` | Drittanbieter-Modell | Allgemeine Szenarien |
| `deepseek-r1` | Schlussfolgerungsmodell | Tiefes Denken |
| `glm-5.1` | Zhipu-Modell | Chinesische Szenarien |
| `kimi-k2.5` | Langer Kontext | Lange Textverarbeitung |
| `llama-4-maverick` | Meta-Modell | Englische Szenarien |

**Konfigurationsbeispiel:**
```
KI-Client-Typ: Cloud-Client-Typ (z.B. DashScopeClient)
apiKey: Ihr API-Schlüssel (von entsprechender Plattform erhalten)
region: beijing (wählen Sie die nächstgelegene Region)
model: qwen3.6-plus (gemäß verfügbaren Modellen auswählen)
temperature: 0.7
maxTokens: 2048
```

### Schritt 4: Konfiguration speichern

1. Nach dem Ausfüllen aller notwendigen Informationen
2. Klicken Sie auf die Schaltfläche **""Konfiguration speichern""** am unteren Rand der Seite
3. Das System wird melden, dass das Speichern erfolgreich war

### Schritt 5: Verbindung testen

1. Klicken Sie auf das **💬 Chat**-Menü in der oberen Navigationsleiste
2. Wählen Sie ein Silicon Being
3. Senden Sie eine Testnachricht, z.B. ""Hallo""
4. Wenn Sie eine Antwort erhalten, war die Konfiguration erfolgreich

## FAQ

### F1: Ich weiß nicht, welchen Client ich wählen soll?

**Vorschläge:**
- Wenn Sie Anfänger sind, wird empfohlen, zuerst **Cloud-Client** zu verwenden, einfache Konfiguration, schnelles Starten
- Wenn Sie Datenschutz schätzen oder bessere Computerkonfiguration haben, wählen Sie **lokalen Client**

### F2: Kann ich zwei Clients gleichzeitig verwenden?

Nein. Das System kann nur einen KI-Client gleichzeitig verwenden. Aber Sie können jederzeit auf der Konfigurationsseite wechseln.

### F3: Werden Chat-Protokolle nach dem Wechseln von Clients verloren?

Nein. Chat-Protokolle werden im System gespeichert und sind unabhängig vom KI-Client. Sie können auch nach dem Wechsel historische Gespräche ansehen.

### F4: Lokaler Client zeigt Verbindungsfehler an?

**Lösungen:**
1. Bestätigen Sie, dass die entsprechende KI-Service-Software läuft (System-Tray oder Prozesse prüfen)
2. Prüfen Sie, ob die endpoint-Adresse korrekt ist (Standardkonfiguration dieses Service referenzieren)
3. Greifen Sie auf die Service-Adresse in einem Browser zu, Sie sollten eine Antwort sehen
4. Bestätigen Sie, dass Modelle heruntergeladen sind: Verwenden Sie den Befehl des spezifischen Service, um die Modellliste anzusehen

### F5: Cloud-Client zeigt Authentifizierungsfehler an?

**Lösungen:**
1. Prüfen Sie, ob der API-Schlüssel korrekt ist (Format bezieht sich auf die Anforderungen der entsprechenden Plattform)
2. Bestätigen Sie, dass das Konto nicht im Rückstand ist
3. Prüfen Sie, ob das Nutzungskontingent überschritten ist
4. Generieren Sie den API-Schlüssel neu und aktualisieren Sie die Konfiguration

### F6: Wie erhält man den API-Schlüssel für Cloud-Client?

**Allgemeine Schritte:**
1. Besuchen Sie die Konsolen-Website des entsprechenden KI-Service-Anbieters
2. Melden Sie sich bei Ihrem Konto an
3. Betreten Sie die Seite ""Schlüsselverwaltung"" oder ""API-Verwaltung""
4. Klicken Sie auf ""Schlüssel erstellen"" oder ""API-Schlüssel generieren""
5. Kopieren Sie den generierten API-Schlüssel und speichern Sie ihn ordnungsgemäß

### F7: Was tun, wenn die Antwort sehr langsam ist?

**Lokaler Client:**
- Prüfen Sie die Computerressourcennutzung (CPU, RAM, Grafikkarte)
- Versuchen Sie ein kleineres Modell (z.B. 8B statt 32B)
- Schließen Sie andere ressourcenverbrauchende Programme

**Cloud-Client:**
- Prüfen Sie die Netzwerkverbindungsqualität
- Versuchen Sie eine näher gelegene Server-Region
- Vermeiden Sie Netzwerk-Hauptlastzeiten

### F8: Was ist der temperature-Parameter?

temperature steuert die Kreativität von KI-Antworten:
- **0.0-0.3**: Sehr konservativ, vorhersehbare Antworten, geeignet für faktische Fragen
- **0.4-0.7**: Ausgewogener Modus, geeignet für tägliche Konversation (empfohlen)
- **0.8-1.0**: Sehr kreativ, vielfältige Antworten, geeignet für kreatives Schreiben

### F9: Wie viel sollte maxTokens eingestellt werden?

- **1024**: Kurze Antworten, geeignet für einfache Fragen und Antworten
- **2048**: Mittlere Länge, geeignet für allgemeine Konversation (empfohlen)
- **4096+**: Lange Antworten, geeignet für komplexe Aufgaben oder lange Textgenerierung

Hinweis: Je größer die Einstellung, je mehr Ressourcen und Zeit verbraucht werden.

### F10: Können verschiedene Beings verschiedene Clients verwenden?

Ja. Jedes Silicon Being kann unabhängig den KI-Client-Typ konfigurieren.

**Einrichtungsmethode:**
1. Betreten Sie die Being-Verwaltungsseite
2. Wählen Sie das zu konfigurierende Being
3. Setzen Sie AIClientType in der Konfiguration dieses Beings
4. Wenn das Being nicht eingestellt ist, wird die globale Konfiguration verwendet

## Best Practices

### 1. Modelle basierend auf Aufgaben auswählen

- **Tägliche Konversation**: Verwenden Sie kleine bis mittlere Modelle (8B-14B oder schnelle Modelle)
- **Komplexe Analyse**: Verwenden Sie große Modelle (32B+ oder fortgeschrittene Modelle)
- **Code-Generierung**: Verwenden Sie dedizierte Programmiermodelle
- **Kreatives Schreiben**: Verwenden Sie höhere temperature (0.8-0.9)

### 2. Kosten optimieren (Cloud-Client)

- Regelmäßig Nutzung prüfen, um Überschreiten des Freikontingents zu vermeiden
- Verwenden Sie schnelle Modelle für einfache Aufgaben
- Verwenden Sie fortgeschrittene Modelle für komplexe Aufgaben
- Setzen Sie maxTokens vernünftig, um Verschwendung zu vermeiden

### 3. Leistung verbessern (Lokaler Client)

- Priorisieren Sie das Herunterladen häufig verwendeter Modelle, um Herunterladen zur Laufzeit zu vermeiden
- Halten Sie KI-Service resident, um Startzeit zu reduzieren
- Verwenden Sie Grafikkarten-Beschleunigung (wenn Sie NVIDIA-Grafikkarte haben)
- Regelmäßig ungenutzte Modelle bereinigen, um Speicherplatz freizugeben

### 4. Sicherheitsempfehlungen

- Teilen Sie API-Schlüssel nicht mit anderen
- Regelmäßig API-Schlüssel ändern
- Lokale Clients sind nur auf lokalen Zugang beschränkt, nicht zum öffentlichen Netzwerk exponieren
- Wichtige Konfigurationsdateien sichern

## Hilfe erhalten

Wenn Sie Probleme haben:
1. Prüfen Sie den Abschnitt [FAQ](#faq)
2. Prüfen Sie [Konfigurationsverwaltung](./config) für Systemkonfigurationsanweisungen
3. Prüfen Sie Systemprotokolle für Fehlerdetails
";
    
    public override string BeingSoul => @"
# Soul-Datei

## Übersicht

Die Soul-Datei ist die Kernkonfigurationsdatei eines Silicon Being und bestimmt die **Persönlichkeit, Verhaltensmuster, fachlichen Fähigkeiten und Arbeitsmethoden** jedes Beings.

Sie können die Soul-Datei als ""Persönlichkeitseinstellung"" oder ""Arbeitsanleitung"" des Beings verstehen. Sie wird bei jeder KI-Konversation automatisch geladen und leitet das Silicon Being, so zu denken und zu handeln, wie Sie es erwarten.

## Rolle der Soul-Datei

Die Soul-Datei ist die **Kernantriebskraft** eines Silicon Being und definiert:

- 🎭 **Rollenpositionierung**: Wer dieses Silicon Being ist, in welchen Bereichen es sich auszeichnet
- 📋 **Verhaltensrichtlinien**: Wie es auf Benutzer antworten sollte, welchen Prinzipien es folgen soll
- 🔄 **Arbeitsablauf**: Wie Aufgaben nach Erhalt bearbeitet werden, in wie vielen Schritten
- ⚠️ **Verhaltensgrenzen**: Was getan werden kann, was nicht getan werden sollte
- 💡 **Fachliche Anforderungen**: Code-Standards, Ausgabeformat, Sprachstil usw.

## So bearbeiten Sie die Soul-Datei

### Über die Web-Oberfläche bearbeiten

1. Gehen Sie zur Seite **Silicon Beings**
2. Klicken Sie auf die Karte des Silicon Being, das Sie bearbeiten möchten
3. Klicken Sie auf den Link **Soul-Datei**
4. Ändern Sie den Inhalt im Markdown-Editor
5. Klicken Sie auf die Schaltfläche **Speichern**

### Über den KI-Assistenten bearbeiten

Sie können auch direkt mit dem **Silicon Curator** sprechen und ihn bitten, die Soul-Datei zu ändern:

```
Bitte helfen Sie mir, die Soul-Datei des Programmierassistenten zu ändern, um Python-Entwicklungsunterstützung hinzuzufügen
```

Der Silicon Curator wird Ihnen helfen, den Inhalt der Soul-Datei zu aktualisieren.

## Leitfaden zum Schreiben von Soul-Dateien

### Grundstruktur

Soul-Dateien werden im **Markdown-Format** geschrieben. Die folgende Struktur wird empfohlen:

```markdown
# Rolleneinstellung

Sie sind ein [Rollenbeschreibung], spezialisiert auf:
- Fähigkeit 1
- Fähigkeit 2
- Fähigkeit 3

# Verhaltensrichtlinien

1. Richtlinie 1
2. Richtlinie 2
3. Richtlinie 3

# Arbeitsablauf

Beim Empfang einer Aufgabe:
1. Anforderungen verstehen
2. Ansatz analysieren
3. Operationen ausführen
4. Ergebnisse melden

# Code-Standards

- Bestimmte Codierungsstandards befolgen
- Erforderliche Kommentare bereitstellen
- Randfälle berücksichtigen
```

### Schreibtipps

1. **Klare Rollendefinition**: Geben Sie klar die Verantwortlichkeiten und Fachgebiete des Silicon Being an
2. **Verhaltensgrenzen setzen**: Erklären Sie, was getan werden kann und was nicht getan werden sollte
3. **Arbeitsablauf bereitstellen**: Leiten Sie das Silicon Being, wie Aufgaben zu bearbeiten sind
4. **Markdown-Format verwenden**: Unterstützt Überschriften, Listen, Codeblöcke usw.
5. **Spezifisch statt vage**: Verwenden Sie konkrete Beispiele anstelle abstrakter Beschreibungen

## Praktische Beispiele

### Beispiel 1: Programmierassistent

```markdown
# Rolleneinstellung

Sie sind ein professioneller Full-Stack-Entwicklungsassistent, spezialisiert auf:
- C# / .NET-Entwicklung
- Architekturdesign und Code-Review
- Datenbankdesign und -optimierung
- Web-Frontend-Entwicklung

# Verhaltensrichtlinien

1. Immer ausführbare Codebeispiele bereitstellen
2. Wichtige Code-Logik und Designgedanken erklären
3. Best-Practice-Empfehlungen geben
4. Bei Unsicherheit den Benutzer klar informieren

# Code-Standards

- SOLID-Prinzipien befolgen
- Klare Namensgebung verwenden
- Erforderliche Kommentare hinzufügen
- Ausnahmebehandlung und Randfälle berücksichtigen
```

### Beispiel 2: Kundenservice-Assistent

```markdown
# Rolleneinstellung

Sie sind ein freundlicher Kundenservice-Assistent, verantwortlich für:
- Häufige Benutzerfragen beantworten
- Einfache Beschwerden bearbeiten
- Benutzer durch Vorgänge führen
- Benutzerfeedback sammeln

# Verhaltensrichtlinien

1. Immer höflich und geduldig bleiben
2. In einfacher, leicht verständlicher Sprache erklären
3. Bei Nichtlösbarkeit sofort an menschlichen Agenten übergeben
4. Benutzerfragen und Feedback aufzeichnen

# Arbeitsablauf

1. Benutzer begrüßen
2. Benutzerbedürfnisse verstehen
3. Lösungen bereitstellen
4. Bestätigen, ob das Problem gelöst ist
5. Benutzer danken und Gespräch beenden
```

### Beispiel 3: Datenanalyse-Assistent

```markdown
# Rolleneinstellung

Sie sind ein Datenanalyseexperte, spezialisiert auf:
- Datenbereinigung und -vorverarbeitung
- Statistische Analyse und Visualisierung
- Trendvorhersage und Anomalieerkennung
- Generieren von Datenberichten

# Ausgabeanforderungen

1. Klare Analyseergebnisse bereitstellen
2. Diagramme zur unterstützenden Erklärung verwenden
3. Datenquellen und Annahmen annotieren
4. Umsetzbare Empfehlungen geben
```

## Speicherort der Soul-Datei

Die Soul-Datei jedes Silicon Being wird in seinem Datenverzeichnis gespeichert:

```
DataDirectory/SiliconManager/{BeingGUID}/soul.md
```

Das System verwaltet diese Datei automatisch, und Sie müssen das Dateisystem nicht manuell bedienen.

## Häufig gestellte Fragen

### F: Werden Änderungen an der Soul-Datei sofort wirksam?

**A:** Ja, sie werden sofort nach dem Speichern wirksam. Beim nächsten Mal, wenn das Silicon Being antwortet, wird es die neue Soul-Datei verwenden.

### F: Gibt es eine Größenbeschränkung für Soul-Dateien?

**A:** Es gibt keine strenge Größenbeschränkung, aber es wird empfohlen, sie in einem vernünftigen Bereich zu halten (innerhalb weniger tausend Wörter). Zu lange Soul-Dateien können die Antwortgeschwindigkeit beeinträchtigen.

### F: Kann ich die Soul-Datei vollständig löschen?

**A:** Es wird nicht empfohlen, die Soul-Datei zu löschen. Wenn der Inhalt leer ist, verliert das Silicon Being die Verhaltensanleitung und kann unerwartete Antworten erzeugen.

### F: Wie sichere ich die Soul-Datei?

**A:** Es wird empfohlen, die Soul-Dateien wichtiger Silicon Beings regelmäßig zu sichern. Sie können:
1. Den Inhalt über die Weboberfläche in eine lokale Datei kopieren
2. Die Exportfunktion des Systems verwenden (falls unterstützt)
3. Das Datenverzeichnis direkt sichern

### F: Was ist die Beziehung zwischen der Soul-Datei und dem Speichersystem?

**A:** Die Soul-Datei definiert **langfristige Verhaltensmuster**, während das Speichersystem **kurzfristige Konversationsverläufe** aufzeichnet. Sie arbeiten zusammen:
- Soul-Datei: Sagt dem Silicon Being ""welche Rolle du hast""
- Speichersystem: Sagt dem Silicon Being ""worüber wir vorher gesprochen haben""

### F: Können verschiedene Silicon Beings dieselbe Soul-Datei verwenden?

**A:** Ja, aber es wird nicht empfohlen. Jedes Silicon Being sollte eine einzigartige Rollenpositionierung haben, um funktionale Duplikate zu vermeiden.

## Best Practices

1. **Kontinuierliche Optimierung**: Optimieren Sie die Soul-Datei kontinuierlich basierend auf tatsächlichem Nutzungsfeedback
2. **Versionsverwaltung**: Sichern Sie die aktuelle Version vor größeren Änderungen
3. **Testverifikation**: Testen Sie die Wirkung nach der Änderung durch Konversation
4. **Prägnant halten**: Drücken Sie Kernanforderungen in prägnanter Sprache aus
5. **Widersprüche vermeiden**: Stellen Sie sicher, dass es keine Konflikte zwischen verschiedenen Richtlinien gibt
6. **Regelmäßige Überprüfung**: Überprüfen Sie regelmäßig, ob die Soul-Datei noch anwendbar ist

## Fehlerbehebung

### Problem: Das Verhalten des Silicon Being entspricht nicht den Erwartungen

**Checkliste:**
1. Ist der Inhalt der Soul-Datei klar und genau?
2. Haben Sie ausreichende Verhaltensanleitung hinzugefügt?
3. Gibt es widersprüchliche Anweisungen?
4. Haben Sie einen spezifischen Arbeitsablauf bereitgestellt?

**Lösungen:**
1. Vage Beschreibungen neu schreiben
2. Weitere spezifische Verhaltensrichtlinien hinzufügen
3. Beispiele bereitstellen, um die erwartete Ausgabe zu veranschaulichen
4. Testen und kontinuierlich optimieren

### Problem: Soul-Datei konnte nicht gespeichert werden

**Mögliche Ursachen:**
1. Dateisystem-Berechtigungsprobleme
2. Unzureichender Festplattenspeicher
3. Datei ist von einem anderen Prozess belegt

**Lösungen:**
1. Systemprotokolle auf detaillierte Fehlerinformationen prüfen
2. Bestätigen, dass das Datenverzeichnis beschreibbar ist
3. Nach dem Neustart des Systems erneut versuchen

## Verwandte Funktionen

- 🤖 [Being-Verwaltung](being-management) - Silicon Beings erstellen und verwalten
- 💬 [Chat-System](chat-system) - Mit Silicon Beings konversieren
- 🧠 [KI-Client-Konfiguration](ai-clients) - KI-Dienste konfigurieren
- 📝 [Speichersystem](memory) - Konversationsverlauf verwalten
";

    public override string AuditLog => @"
# Prüfprotokoll

## Übersicht

Das Prüfprotokoll verfolgt und analysiert den Token-Verbrauch aller KI-Aufrufe im System. Damit können Sie:
- 📊 Token-Verbrauchsmuster visualisieren
- 💰 Kosten überwachen und vorhersagen
- 🔍 Anomale Nutzung erkennen
- 📈 Nutzungstrends analysieren

### Was ist ein Token?

Ein Token ist die grundlegende Einheit, mit der KI-Modelle Text verarbeiten:
- ca. 1 Token ≈ 0,75 englische Wörter oder 1,5 chinesische/koreanische Zeichen
- **Eingabe-Token**: Vom Benutzer gesendete Nachrichten (Fragen, Befehle)
- **Ausgabe-Token**: Von der KI generierte Antworten (Antworten, Ausführungsergebnisse)
- **Gesamt-Token** = Eingabe-Token + Ausgabe-Token
Je höher der Token-Verbrauch, desto höher die Rechenkosten.

## Zugriff auf das Prüfprotokoll

### Berechtigungsanforderungen
- Nur Systemadministratoren können darauf zugreifen (höchste Berechtigungsstufe)
- Normale Benutzer können es nicht sehen (Sicherheit und Datenschutz)

### Zugriffsmethode
1. Im Hauptmenü auf ""Systemverwaltung"" klicken (🔧 Symbol)
2. Im linken Navigationsmenü auf ""Prüfung"" klicken (📊 Symbol)
3. Das Prüfprotokoll-Dashboard wird automatisch geladen.
4. Bei fehlenden Berechtigungen wird eine Zugriff-verweigert-Meldung angezeigt.
5. Die Daten werden automatisch auf dem neuesten Stand gehalten (kein manuelles Aktualisieren erforderlich).
## Prüfprotokoll-Dashboard-Funktionen

### 1. Nutzungstrend-Diagramm (Liniendiagramm)
**Position**: Im oberen Hauptbereich des Dashboards angezeigt.
**Funktion**:
- Zeigt den täglichen Token-Nutzungstrend innerhalb des ausgewählten Zeitraums.
- Zwei Datenpunkte angezeigt:
  - 🔵 **Tägliche Token**: Gesamtzahl der an einem Tag verbrauchten Token.
  - 🟢 **Tägliche Kosten**: Gesamtkosten an einem Tag (EUR).
- **Zeitraumauswahl**:
  - Schnellauswahl-Buttons (""Letzte 7 Tage"", ""Letzte 30 Tage"", ""Dieser Monat"" usw.).
  - Benutzerdefinierter Datumsbereich (Kalenderauswahl unterstützt).
- **Daten-Zoom**: Direktes Zoomen im Diagramm zur Anzeige detaillierter Daten.
- **Daten-Hinweis**: Mauszeiger über Datenpunkte bewegen, um genaue Zahlen anzuzeigen.

### 2. Zusammenfassungs-Statistikkarten (4 Hauptindikatoren)

4 Zusammenfassungskarten oben im Dashboard angezeigt:

- 🔢 **Gesamter Token-Verbrauch**: Gesamtzahl der Token im ausgewählten Zeitraum.
- 💰 **Gesamtkosten**: Gesamtbetrag im ausgewählten Zeitraum (EUR).
- 📊 **Tagesdurchschnitt**: Durchschnittlicher täglicher Token-Verbrauch im ausgewählten Zeitraum.
- 📈 **Maximale tägliche Nutzung**: Höchster täglicher Token-Verbrauch im ausgewählten Zeitraum.
Diese Indikatoren helfen Ihnen, die Gesamtnutzung und Verbrauchstrends schnell zu erfassen.
### 3. Nutzungs-Verteilung (Donut-Diagramm)
**Position**: Unter oder neben den Zusammenfassungskarten angezeigt.
**Funktion**:
- Zeigt den Token-Nutzungsanteil jedes KI-Modells.
- Intuitive Anzeige durch farbliche Unterscheidung der Modelle.
- Klicken Sie auf den Modellnamen, um nur die Daten dieses Modells anzuzeigen.
- **Verwendungsbeispiel**:
  - Ermitteln, welches Modell die meisten Token verbraucht.
  - Wenn der Anteil teurer Modelle zu hoch ist, auf günstigere Modelle umstellen.
### 4. Nutzung nach Zeitstunde (Balkendiagramm)
**Position**: Neben dem Verteilungsdiagramm angezeigt.
**Funktion**:
- Zeigt den Token-Verbrauch jeder Stunde innerhalb von 24 Stunden.
- **Hotspot-Identifizierung**: Spitzenzeiten identifizieren (z.B. 14:00-16:00).
- **Inaktive Zeiten**: Zeiten identifizieren, in denen das System kaum genutzt wird.
**Verwendungsbeispiel**:
- Vermeiden Sie das Planen großer Aufgaben während der Spitzenzeiten und verschieben Sie sie in Leerlaufzeiten.
- Prüfen Sie, ob die hohe Nutzung während der Spitzenzeiten normal ist oder auf anomale Aufgaben zurückzuführen ist.
### 5. Erweiterte Filterung und Suche (schnelle Datenexploration)
**Position**: Oben im Dashboard oder im Filterbereich.
**Unterstützte Filterbedingungen**:
- 📅 **Datumsbereich**: Start- und Enddatum auswählen.
- 🤖 **KI-Modell**: Nur Daten bestimmter Modelle anzeigen (z.B. ""gpt-4"", ""qwen-plus"").
- 🛠️ **Tool-Name**: Nur Nutzung bestimmter Tools anzeigen (z.B. ""CalendarTool"", ""ChatTool"").
- 📊 **Token-Bereich**: Nur Daten anzeigen, bei denen die Token-Zahl in einem bestimmten Bereich liegt (z.B. 1000-5000).
- 🔍 **Schlüsselwort**: Schlüsselwortsuche nach Tool- oder Modellnamen.
**Verwendungsbeispiel**:
- ""Wie hoch sind die Kosten für gpt-4?"" → Modellfilter auf ""gpt-4"" setzen.
- ""Wie hoch ist die Nutzung von CalendarTool?"" → Tool-Filter auf ""CalendarTool"" setzen.
- ""Welche Aufrufe haben zu hohe Kosten?"" → Token-Bereich hoch einstellen.
- ""Wie hoch war die Nutzung letzte Woche?"" → Datumsbereich auf letzte Woche setzen.
### 6. Datenexport (Analysebericht)
**Position**: Oben rechts im Dashboard.
**Funktion**:
- Auf ""CSV exportieren"" klicken.
- Alle Daten entsprechend den aktuellen Filterbedingungen werden als CSV-Datei exportiert.
- Kann mit Excel oder anderer Tabellenkalkulationssoftware geöffnet werden.
**CSV-Datei enthält**:
- Aufrufzeit (genaue Uhrzeit)
- KI-Modellname
- Verwendeter Tool-Name.
- Token-Zahl (Eingabe/Ausgabe/Gesamt)
- Kosten (EUR)
- Aufrufstatus (Erfolg/Fehler)
**Verwendungsbeispiel**:
- Monatsberichte erstellen.
- Daten mit dem Team teilen.
- Langfristige Trendanalyse und Kostenoptimierung.
## Verwendungsszenario-Beispiele.
### Szenario 1: Kostenkontrolle und -optimierung.
**Problem**: Die monatlichen Kosten des Systems sind zu hoch.
**Lösung**.
1. Prüfprotokoll-Dashboard öffnen.
2. Zeitraum auf ""Dieser Monat"" setzen.
3. ""Tagesdurchschnitt"" und ""Gesamtkosten"" prüfen.
4. Teure Modelle im Donut-Diagramm identifizieren.
5. Erweiterte Filter verwenden, um kostenintensive Aufrufe zu suchen.
6. Als CSV exportieren und mit dem Team besprechen.
7. Optimierungsmaßnahmen:
   - Auf günstigere Modelle umstellen.
   - Unnötige Aufrufe reduzieren.
   - Cache-Mechanismen einführen.
**Ergebnis**: 30% Kosteneinsparung.
### Szenario 2: Anomale Nutzungserkennung.
**Problem**: Plötzlicher Anstieg des Token-Verbrauchs.
**Lösung**.
1. Prüfprotokoll-Dashboard öffnen.
2. Zeitraum auf ""Letzte 7 Tage"" setzen.
3. Anstiegszeiten im Liniendiagramm identifizieren.
4. Erweiterte Filter verwenden, um Daten für diesen Zeitraum zu suchen.
5. Tool-Name, Modellname und Token-Zahl prüfen.
6. Ursache der Anomalie analysieren:
   - Großaufgabe ausgeführt?
   - Endlosschleife?
   - Böswilliger Angriff?
7. Gegenmaßnahmen ergreifen.
**Ergebnis**: Anomale Probleme rechtzeitig erkannt und behoben.
### Szenario 3: Leistungsoptimierung.
**Problem**: Langsame Systemantwortzeiten.
**Lösung**.
1. Prüfprotokoll-Dashboard öffnen.
2. Zeitraum auf ""Letzte 30 Tage"" setzen.
3. Spitzenzeiten im Balkendiagramm identifizieren.
4. Tool-Aufrufe während der Spitzenzeiten prüfen.
5. Optimierungsmaßnahmen:
   - Aufgabenplanung während der Spitzenzeiten anpassen.
   - Kostenintensive Tool-Aufrufe optimieren.
   - Cache-Mechanismen einführen.
**Ergebnis**: 40% Verbesserung der Antwortzeiten.
### Szenario 4: Teamzusammenarbeit und Berichte.
**Problem**: Monatlicher Nutzungsbericht muss beim Team eingereicht werden.
**Lösung**.
1. Prüfprotokoll-Dashboard öffnen.
2. Zeitraum auf ""Dieser Monat"" setzen.
3. Daten prüfen und analysieren.
4. Auf ""CSV exportieren"" klicken.
5. CSV-Datei in Excel öffnen und Tabellen und Diagramme erstellen.
6. Bericht erstellen:
   - Gesamt-Token und Kosten.
   - Hauptverwendete Tools und Modelle.
   - Trendanalyse und Optimierungsvorschläge.
7. Mit dem Team teilen.
**Ergebnis**: Effiziente Berichtserstellung, datengestützt.
## Prüfungstool (KI-Aufruf).
Systemadministratoren können der KI erlauben, Prüfprotokolldaten abzufragen.
### Verwendungsmethode.
1. Chat-Interface öffnen.
2. KI fragen (z.B. ""Wie hoch war der Token-Verbrauch letzte Woche?"").
3. KI ruft automatisch das Prüfungstool auf.
4. KI zeigt die zusammengefassten Ergebnisse an.
### Unterstützte Fragetypen.
- ""Wie viele Token habe ich in den letzten 7 Tagen verbraucht?""
- ""Wie hoch sind die Gesamtkosten diesen Monat?""
- ""Welches Modell habe ich am häufigsten verwendet?""
- ""Wie oft wurde CalendarTool verwendet?""
### Vorsichtsmaßnahmen.
- Administratorberechtigungen erforderlich.
- Abfrage-Zeitbereichsbeschränkung (z.B. maximal 90 Tage).
- Zeitüberschreitung bei Abfragen großer Datenmengen möglich.
## Datenbeschreibung.
### Datenquelle.
- Alle Daten stammen aus tatsächlichen KI-Aufrufprotokollen.
- Automatische Aufzeichnung startet beim Systemstart.
- Gespeichert in lokalen Dateien, nicht in die Cloud hochgeladen.
### Datengenauigkeit.
- Token-Zahl: Tatsächliche vom KI-Modell zurückgegebene Zahl.
- Kosten: Berechnet als Modellpreis × Token-Zahl.
- Zeit: Basierend auf der Systemzeit (Kalendereinstellungen können angewendet werden).
### Datenaufbewahrung.
- Standardmäßig 365 Tage (1 Jahr) aufbewahrt.
- Abgelaufene Daten werden automatisch bereinigt.
- Aufbewahrungsdauer kann in den Einstellungen angepasst werden.
## Häufig gestellte Fragen (FAQ).
### F: Warum werden keine Prüfprotokolldaten angezeigt?
**A:** Mögliche Gründe:
1. Das System wurde gerade gestartet und es gibt noch keine Aufrufprotokolle.
2. Der Datenbereich ist zu kurz (wählen Sie einen breiteren Zeitraum).
3. Filterbedingungen sind zu streng (Filter zurücksetzen).
4. Noch keine Aufrufprotokolle vorhanden (nach dem Starten eines Chats erneut prüfen).
### F: Können Prüfprotokolldaten geändert werden?
**A:** Nein. Prüfprotokolldaten sind ursprüngliche Aufzeichnungen und können nicht geändert werden. (Datenintegrität und Rückverfolgbarkeit gewährleisten).
### F: Können Daten für einen bestimmten Zeitraum gelöscht werden?
**A:** Durch Anpassen der Aufbewahrungsdauer in den Einstellungen werden abgelaufene Daten automatisch bereinigt. (Manuelles Löschen nicht unterstützt).
### F: Die Kosten stimmen nicht mit der tatsächlichen Abrechnung überein?
**A:** Mögliche Gründe:
1. Veraltete Preise (aktuelle Preise in den Einstellungen aktualisieren).
2. Zeitzonenunterschiede (Kalendereinstellungen prüfen).
3. Einige Aufrufe wurden nicht aufgezeichnet (Systemneustart). (Durch Systemneustart beheben).
### F: Können Prüfprotokolldaten mit anderen Benutzern geteilt werden?
**A:** Durch CSV-Export können Sie Daten mit dem Team teilen. (Direkte Kontofreigabe nicht unterstützt, Sicherheitsprobleme).
### F: Datenexport fehlgeschlagen?
**A:** Mögliche Gründe:
1. Unzureichender Festplattenspeicher (Festplattenspeicher freigeben).
2. Datei von einem anderen Programm belegt (entsprechende Programme schließen).
3. Berechtigungsproblem (mit Administratorberechtigungen ausführen).
### F: Wann werden Echtzeitdaten aktualisiert?
**A:** Wenn die Seite geöffnet ist, wird sie automatisch aktualisiert (alle 5 Sekunden). (Wenn die Seite geschlossen ist, werden die Daten nicht aktualisiert, erneut öffnen).
## Best Practices.
1. **Regelmäßige Überprüfung**: Prüfprotokolldaten wöchentlich oder monatlich prüfen.
2. **Kostenwarnung**: Kostenschwellenwert einstellen, Benachrichtigung bei Überschreitung.
3. **Optimierungsanpassung**: Modellauswahl und Aufrufstrategie basierend auf Daten optimieren.
4. **Datenbackup**: Regelmäßig als CSV exportieren und backupen.
5. **Team-Sharing**: Regelmäßig Daten mit dem Team teilen, Zusammenarbeit optimieren.
## Verwandte Funktionen.
- 💰 [Kostenmanagement](cost-management) - KI-Service-Kosten verfolgen und optimieren.
- 🛠️ [Tool-Verwaltung](tool-management) - Systemtool-Nutzung und -Status.
- 🧠 [KI-Client-Konfiguration](ai-clients) - KI-Dienste konfigurieren.
- 📊 [Dashboard](dashboard) - Systemstatus überwachen.
";
    
    public override string KnowledgeGraph => @"
# Wissensgraph

## Übersicht

Der Wissensgraph ist das **Wissensmanagement- und Visualisierungssystem** für Silicon Beings, das das von Silicon Beings gelernte und angesammelte Wissen auf grafische Weise darstellt. Durch intuitive Knoten und Verbindungen können Sie die Beziehungen zwischen Wissen klar erkennen.

## Was ist ein Wissenstripel?

Die Grundeinheit des Wissensgraphen ist das **Wissenstripel**, das eine ""Subjekt-Prädikat-Objekt""-Struktur verwendet:

```
(Subjekt) -[Relation]-> (Objekt)
```

**Beispiele:**
- `(Xiao Ming) -[mag]-> (Äpfel)`
- `(Python) -[ist]-> (Programmiersprache)`
- `(Erde) -[umkreist]-> (Sonne)`

Diese Struktur ermöglicht es Silicon Beings, Wissen wie Menschen zu verstehen und zu organisieren.

## Zugriff auf den Wissensgraphen

### Einstiegspunkt

1. Klicken Sie auf das **📚 Wissensdatenbank**-Symbol in der linken Navigationsleiste
2. Das System lädt und zeigt automatisch die Wissensgraphen-Visualisierungsseite

### Interface-Layout

Die Wissensgraphen-Seite enthält:

- **Obere Titel**: Zeigt ""Wissensgraph-Visualisierung"" an
- **Statistik-Panel**: Zeigt Knotenanzahl, Kantenanzahl und andere Statistiken
- **Haupt-Canvas-Bereich**: Zeigt die Wissensgraphen-Visualisierung
- **Ladeindikator**: Zeigt Lademeldung während des Datenladens

## Graph-Visualisierung

### Knoten

Knoten repräsentieren **Entitäten** (Subjekte oder Objekte) im Wissen:

- **Kreisförmige Knoten**: Jeder Knoten repräsentiert eine Entität
- **Knoten-Labels**: Zeigen den Entitätsnamen
- **Knotengröße**: Automatisch angepasst basierend auf Verbindungsanzahl (mehr Verbindungen = größerer Knoten)
- **Knotenfarben**:
  - Blauer Rand: Reguläre Knoten
  - Hervorgehoben: Beim Hovern mit der Maus

### Kanten

Kanten repräsentieren **Relationen** (Prädikate) im Wissen:

- **Linien mit Pfeilen**: Zeigen Relationen von Subjekt zu Objekt
- **Linienfarbe**: Halbtransparentes Blau `rgba(100, 150, 255, 0.6)`
- **Pfeil-Markierungen**: Zeigen klar die Relationsrichtung

### Layout-Algorithmus

Der Wissensgraph verwendet einen **kreisförmigen Layout-Algorithmus**:

- Alle Knoten sind gleichmäßig auf kreisförmigen Bahnen verteilt
- Angemessener Abstand zwischen Knoten
- Passt sich automatisch an die Canvas-Größe an

## Interaktive Funktionen

### Mausoperationen

- **Knoten hovern**: Knoten wird hervorgehoben für einfache Identifizierung
- **Relationen anzeigen**: Relationsrichtung durch Pfeile verstehen
- **Canvas zoomen**: Browser-native Zoom-Funktion (Strg + Scrollrad)

### Datenladen

- **Automatisches Laden**: Lädt automatisch Wissensdaten vom System beim Seitenaufruf
- **Echtzeit-Aktualisierung**: Seite erneut besuchen für neuestes Wissen
- **Ladeindikator**: Zeigt ""Wissensgraph-Daten werden geladen..."" während des Ladens

## Wissensmanagement

### Wie wird Wissen generiert?

Silicon Beings akkumulieren Wissen durch:

1. **Konversationslernen**: Extrahieren von Schlüsselinformationen aus Gesprächen mit Benutzern
2. **Tool-Aufruf**: Hinzufügen von Wissen durch KnowledgeTool
3. **Dateianalyse**: Analysieren hochgeladener Dateiinhalte zum Extrahieren von Wissen
4. **Aufgabenausführung**: Sammeln von Domänenwissen während der Aufgabenausführung

### KnowledgeTool-Operationen

Silicon Curatoren oder Silicon Beings können KI bitten, Wissensoperationen durch Konversation durchzuführen:

**Wissen hinzufügen:**
```
Bitte füge Wissen hinzu: (Python) -[ist]-> (Programmiersprache)
```

**Wissen abfragen:**
```
Finde alles Wissen über ""Python""
```

**Wissen suchen:**
```
Suche nach Wissen mit ""Programmierung""
```

**Relationspfad finden:**
```
Finde den Relationspfad zwischen ""Python"" und ""KI""
```

### Wissensattribute

Jeder Wissenseintrag enthält reiche Metadaten:

- **Eindeutige ID**: Eindeutiger Identifikator für jedes Wissen
- **Versionsnummer**: Unterstützt Wissensaktualisierungen und Versionsverwaltung
- **Konfidenz**: 0.0-1.0, zeigt Wissenszuverlässigkeit an
- **Quelle**: Wissensquelle (Konversation, Datei, Tool usw.)
- **Kategorie**: Domäne oder Kategorie, zu der das Wissen gehört
- **Tags**: Benutzerdefinierte Tags für einfache Klassifizierung und Suche
- **Ersteller**: Das Silicon Being, das dieses Wissen hinzugefügt hat
- **Erstellungszeit**: Zeitstempel, wann Wissen hinzugefügt wurde
- **Validierungsstatus**: Unvalidiert/Validiert/Abgelehnt

## Statistiken

Die Wissensgraphen-Seite zeigt folgende Statistiken:

- **Gesamtknoten**: Anzahl aller Entitäten im Wissensgraphen
- **Gesamtkanten**: Anzahl der Relationen zwischen Entitäten
- **Wissenseinträge**: Gesamtzahl der Wissenstripel im System

Diese Statistiken helfen Ihnen, das Wissensakkumulationsniveau von Silicon Beings zu verstehen.

## Anwendungsfälle

### 1. Wissensvisualisierung

Intuitiv anzeigen, was Silicon Beings gelernt haben:

1. Wissensgraphen-Seite aufrufen
2. Knotenverteilung und Verbindungen beobachten
3. Wissensorganisationsstruktur und -assoziationen verstehen

### 2. Wissensvalidierung

Lernergebnisse von Silicon Beings überprüfen:

1. Wissensknoten in spezifischen Domänen anzeigen
2. Wissensgenauigkeit validieren
3. Wissen durch Konversation ergänzen oder korrigieren

### 3. Wissensforschung

Assoziationen zwischen Wissen entdecken:

1. Hoch verbundene Knoten (Kernkonzepte) beobachten
2. Relationspfade verfolgen, um indirekte Assoziationen zu entdecken
3. Wissenslücken identifizieren, um weiteres Lernen zu leiten

## Häufig gestellte Fragen

### F: Warum ist der Wissensgraph leer?

**A:** Wenn Silicon Beings noch kein Wissen gelernt haben, wird der Graph leer angezeigt. Sie können Wissen hinzufügen durch:
- Mit Silicon Beings konversieren, um sie neue Informationen lernen zu lassen
- Dateien hochladen für Silicon Beings zur Analyse
- Silicon Beings direkt bitten, spezifisches Wissen hinzuzufügen

### F: Aktualisiert sich der Wissensgraph automatisch?

**A:** Wissensgraphen-Daten werden in Echtzeit gespeichert, aber die Seite muss aktualisiert werden, um die neuesten Daten anzuzeigen. Nachdem Silicon Beings neues Wissen gelernt haben, besuchen Sie die Wissensgraphen-Seite erneut, um Aktualisierungen zu sehen.

### F: Wie anzeigen, wenn es zu viele Knoten gibt?

**A:** Wenn das Wissensvolumen groß ist:
- Browser-Zoom verwenden (Strg + Scrollrad)
- Auf hoch verbundene Kernknoten fokussieren
- Nach Wissen zu spezifischen Themen durch Query-Tools suchen

### F: Wie falsches Wissen löschen?

**A:** Sie können Silicon Beings bitten, Wissen durch Konversation zu löschen oder zu korrigieren:
```
Bitte lösche inkorrektes Wissen über ""...""
```
oder
```
Bitte aktualisiere Wissen: (altes Wissen) zu (neues Wissen)
```

### F: Wo werden die Wissensgraphen-Daten gespeichert?

**A:** Wissensdaten werden im Datenverzeichnis des Systems im JSON-Format gespeichert. Das System verwaltet automatisch Wissensspeicherung, -indizierung und -abfrage.

### F: Kann ich den Wissensgraphen exportieren?

**A:** Derzeit wird der Wissensgraph hauptsächlich für Online-Visualisierung verwendet. Um Wissensdaten zu exportieren, können Sie Silicon Beings bitten, durch Tools abzufragen und Berichte zu generieren.

## Beste Praktiken

### 1. Wissensakkumulation regelmäßig überprüfen

- Wissensgraphen wöchentlich überprüfen, um Lernfortschritt von Silicon Beings zu verstehen
- Auf neu hinzugefügte Knoten und Relationen fokussieren
- Genauigkeit wichtigen Wissens validieren

### 2. Wissenslernen leiten

- Proaktiv hochwertige Wissensinputs bereitstellen
- Falsches Wissen korrigieren, um Wissensdatenbank-Genauigkeit zu erhalten
- Kategorien und Tags zu Wissen hinzufügen für einfachere Verwaltung

### 3. Wissensgraphen zur Unterstützung von Konversationen verwenden

- In Konversationen auf bestehendes Wissen referenzieren
- Silicon Beings bitten, basierend auf bestehendem Wissen zu schlussfolgern
- Assoziationen zwischen Wissen erforschen, um neue Erkenntnisse zu entdecken

### 4. Wissensqualitätskontrolle

- Auf Wissenskonfidenz-Scores achten
- Genauigkeit kritischen Wissens validieren
- Veraltetes oder inkorrektes Wissen zeitnah aktualisieren

## Verwandte Dokumentation

- [Being-Verwaltung](/help/being-management) - Verwaltung der Silicon Being-Erstellung und -Konfiguration
- [Chat-System](/help/chat-system) - Mit Silicon Beings konversieren, um Wissen zu lernen
- [Aufgaben-System](/help/task) - Wissenslern-Aufgaben konfigurieren
- [Speicher-System](/help/memory) - Verstehen, wie Wissen das Konversations-Speicher beeinflusst
";
    
    public override string WorkNotes => @"
# Arbeitsnotizen

## Übersicht

Arbeitsnotizen sind das**persönliche Wissensaufzeichnungssystem** von Silicon Beings, ähnlich einem digitalen Tagebuch oder Arbeitsprotokoll. Jedes Silicon Being kann eigene Arbeitsnotizen erstellen, verwalten und abrufen, um Lernerfahrungen, Arbeitserkenntnisse, Gedanken und Reflexionen aufzuzeichnen.

Arbeitsnotizen unterstützen zwei Modi:
- **Persönliche Arbeitsnotizen**: Dem Silicon Being privat gehörend, standardmäßig nicht öffentlich
- **Projekt-Arbeitsnotizen**: Dem Projektraum gehörend, Projektmitglieder können zusammenarbeiten

## Hauptfunktionen

### Kernmerkmale

- **Seitenbasierte Aufzeichnung**: Jede Notiz ist eine unabhängige Seite, nach Seitennummer geordnet
- **Markdown-Unterstützung**: Inhalte unterstützen Markdown-Format, Rich-Text-Bearbeitung möglich
- **Schlagwort-Markierung**: Notizen können mit Schlagworten versehen werden, praktisch für Suche und Klassifizierung
- **Versionsverwaltung**: Bei Bearbeitung wird automatisch die Versionsnummer aktualisiert, Änderungshistorie wird aufgezeichnet
- **Autorenverfolgung**: Informationen über Ersteller und letzten Bearbeiter werden aufgezeichnet
- **Timeline-Ansicht**: Alle Notizen in chronologischer Reihenfolge anzeigen
- **Volltextsuche**: Notizen nach Schlagworten, Zusammenfassung oder Inhalt durchsuchen

### Zwei Arten von Arbeitsnotizen

| Typ | ownership | Standard-Sichtbarkeit | Anwendungsszenario |
|------|-----------|----------------------|-------------------|
| Persönliche Arbeitsnotizen | Silicon Being | Privat | Persönliches Tagebuch, Lernnotizen, Gedankenprotokoll |
| Projekt-Arbeitsnotizen | Projektraum | Öffentlich | Projektfortschritt, Zusammenarbeitsprotokoll, Besprechungsnotizen |

## Zugriff auf Arbeitsnotizen

### Einstiegspunkte

**Persönliche Arbeitsnotizen**:
1. Klicken Sie in der linken Navigationsleiste auf das Symbol **📝 Arbeitsnotizen**
2. Oder finden Sie den Arbeitsnotizen-Einstiegspunkt auf der Silicon Being-Detailseite

**Projekt-Arbeitsnotizen**:
1. Betreten Sie die Projektdetailseite
2. Klicken Sie auf den Reiter **Arbeitsnotizen**

### Oberflächenbeschreibung

Die Arbeitsnotizen-Seite enthält zwei Hauptbereiche:

**Notizraster-Bereich**:
- Zeigt alle Notizen in Kartenform an
- Jede Karte zeigt Seitennummer, Zusammenfassung, Aktualisierungszeit
- Beim Hover-Effekt schwebt die Karte nach oben
- Klicken Sie auf eine Karte, um Notizdetails zu öffnen

**Notizdetail-Bereich**:
- Zeigt den vollständigen Inhalt der Notiz im Vollbild
- Zeigt Metadaten (Zusammenfassung, Schlagworte, Aktualisierungszeit)
- Inhalt wird im Markdown-Format gerendert
- Klicken Sie auf die Schließen-Schaltfläche, um zur Liste zurückzukehren

## Notiz erstellen

### Erstellen mit AI-Tools

Silicon Beings können Notizen durch Konversation mit AI erstellen:

**Persönliche Notiz erstellen**:
```
Bitte erstelle eine Arbeitsnotiz für mich:
- Zusammenfassung: Konzept des Wissensgraphen gelernt
- Inhalt: Ein Wissensgraph ist eine Methode, Wissen in Graphstruktur darzustellen...
- Schlagworte: Wissensgraph, AI, Lernen
```

**Projekt-Notiz erstellen**:
```
Bitte erstelle eine Arbeitsnotiz im Projekt XXX:
- Zusammenfassung: Projektfortschritt Update - Woche 1
- Inhalt: Diese Woche wurde die Anforderungsanalysephase abgeschlossen...
- Schlagworte: Fortschritt, Wochenbericht
```

### Erforderliche Notizfelder

- **Zusammenfassung**: Kurze Beschreibung des Notizinhalts (erforderlich)
- **Inhalt**: Detaillierter Inhalt der Notiz, Markdown unterstützt (erforderlich)
- **Schlagworte**: Durch Kommas getrennte Schlagworte (optional)

### Automatisch aufgezeichnete Informationen

- **Erstellungszeit**: UTC-Zeit wird automatisch aufgezeichnet
- **Ersteller**: GUID des Erstellers wird automatisch aufgezeichnet
- **Seitennummer**: System vergibt automatisch eine eindeutige Seitennummer
- **Versionsnummer**: Anfangsversion ist 1

## Notizen anzeigen und verwalten

### Notizliste durchsuchen

1. Arbeitsnotizen-Seite öffnen
2. Notizkartenraster anzeigen
3. Oben auf der Seite wird die Gesamtzahl der Seiten angezeigt
4. Karten sind nach Seitennummer geordnet

### Notizdetails anzeigen

1. Klicken Sie auf eine beliebige Notizkarte
2. Notizdetails werden im Vollbild erweitert
3. Folgende Informationen anzeigen: Seitennummer-Titel, Zusammenfassung, Schlagworte, Aktualisierungszeit, vollständiger Inhalt (Markdown-Rendering)
4. Klicken Sie auf **×** oben rechts, um Details zu schließen

### Notizkarteninformationen

Jede Karte zeigt:
- **Page X**: Seitennummern-Identifikator
- **Zusammenfassung**: Kurze Beschreibung der Notiz
- **Aktualisierungszeit**: Letzte Bearbeitungszeit

## Notizen durchsuchen

### Suchmethoden

**Suche mit AI-Tools**: Arbeitsnotizen suchen, die Schlagworte enthalten
**Suchbereich**: Notizzusammenfassung, Notizinhalt, Schlagwort-Feld

### Notizverzeichnis generieren

Sie können AI bitten, ein Verzeichnis aller Notizen zu generieren:
```
Bitte generiere ein Verzeichnis meiner Arbeitsnotizen
```

## Notizen aktualisieren

### Notizinhalt bearbeiten

Aktualisieren Sie Notizen mit AI-Tools, Sie können nur Zusammenfassung, Inhalt oder Schlagworte einzeln aktualisieren

### Aktualisierungsmerkmale

- **Selektives Update**: Nur einige Felder können aktualisiert werden
- **Versionserhöhung**: Bei jeder Bearbeitung wird die Versionsnummer automatisch +1 erhöht
- **Änderungsverfolgung**: GUID des letzten Bearbeiters wird aufgezeichnet
- **Zeitaktualisierung**: Aktualisierungszeit wird automatisch aktualisiert

## Notizen löschen

### Löschvorgang

Löschen Sie Notizen mit AI-Tools, Seitennummer oder Notiz-ID erforderlich

### Löschbestätigung

- Nach dem Löschen nicht wiederherstellbar, bitte vorsichtig handeln
- Löschvorgang wird im Audit-Log aufgezeichnet

## Berechtigungsverwaltung

### Berechtigungen für persönliche Arbeitsnotizen

- **Ersteller**: Vollständige Kontrolle (Erstellen, Anzeigen, Bearbeiten, Löschen)
- **Silicon Kurator**: Kann Notizen aller Silicon Beings verwalten
- **Andere Benutzer**: Keine Zugriffsberechtigung

### Berechtigungen für Projekt-Arbeitsnotizen

- **Projektmitglieder**: Können Projektnotizen erstellen, anzeigen, bearbeiten
- **Nicht-Projektmitglieder**: Keine Zugriffsberechtigung
- **Silicon Kurator**: Kann alle Projektnotizen verwalten

## Anwendungsszenarien

### Persönliche Lernaufzeichnung

Aufzeichnen des Prozesses beim Lernen neuen Wissens, Zusammenfassung, Inhalt, Schlagworte hinzufügen

### Projektfortschritt-Verfolgung

Wöchentlichen Projektfortschritt aufzeichnen, Aufgabenerledigung verfolgen

### Arbeitserfahrung-Zusammenfassung

Zusammenfassung von Arbeitserfahrungen und Lehren, für zukünftige Referenz speichern

### Besprechungsprotokoll

Projektbesprechungsinhalte aufzeichnen, Entscheidungen und Diskussionsergebnisse speichern

## Beste Praktiken

### 1. Zusammenfassung standardisiert verfassen

- Zusammenfassung auf unter 20 Zeichen beschränken, Kernaussage der Notiz klar ausdrücken

### 2. Schlagworte vernünftig verwenden

- 3-5 Schlagworte verwenden, durch Kommas getrennt

### 3. Inhalt strukturieren

- Markdown-Überschriftenhierarchie verwenden, wichtige Informationen mit Listen

### 4. Notizen regelmäßig organisieren

- Regelmäßig Verzeichnisübersicht generieren, abgelaufene oder unnötige Notizen löschen

### 5. Projekt-Notizen-Zusammenarbeit

- Autoreninformationen klar aufzeichnen, bei Bearbeitung Änderungsgrund angeben

## Häufig gestellte Fragen

### F1: Was ist der Unterschied zwischen Arbeitsnotizen und Speichersystem?

**A**: Arbeitsnotizen sind aktiv aufgezeichnete strukturierte Inhalte, ähnlich einem Tagebuch; das Speichersystem speichert automatisch oder manuell gespeicherte Gesprächsfragmente und Fakten.

### F2: Kann ich Notizen mit Passwortschutz versehen?

**A**: Die aktuelle Version unterstützt keinen Passwortschutz, aber der Zugriff wird durch das Berechtigungssystem kontrolliert.

### F3: Gibt es eine Begrenzung für die Anzahl der Notizen?

**A**: Es gibt keine harte Begrenzung, aber wir empfehlen, regelmäßig zu organisieren, um die Notizqualität zu erhalten.

### F4: Kann ich Notizen exportieren?

**A**: Die aktuelle Version unterstützt keinen direkten Export, aber Notizdaten können über die API abgerufen werden.

### F5: Kann ich die Versionshistorie von Notizen anzeigen?

**A**: Die aktuelle Version behält nur die neueste Version, historische Versionen werden nicht gespeichert.

### F6: Können Projekt-Notizen und persönliche Notizen ineinander umgewandelt werden?

**A**: Direkte Umwandlung wird nicht unterstützt, Sie können eine neue Notiz erstellen und den Inhalt kopieren.

## Verwandte Dokumentation

- [Silicon Being-Verwaltung](/help/being-management) - Verstehen, wie Silicon Beings verwaltet werden
- [Projektmanagement](/help/projects) - Verstehen von Projektraum und Zusammenarbeit
- [Aufgabenverwaltung](/help/task) - Verstehen von Aufgabenzuweisung und -verfolgung
- [Wissensgraph](/help/knowledge-graph) - Verstehen der Wissensvisualisierung
- [Berechtigungsverwaltung](/help/permission) - Verstehen des Berechtigungskontrollmechanismus
";
    
    public override string Projects => @"
# Projektmanagement

## Übersicht

Projektmanagement ist der**Zusammenarbeitsarbeitsraum** des Silicon Being-Systems, der mehreren Silicon Beings eine Umgebung für gemeinsames Arbeiten bietet. Durch Projekträume kann der Silicon Kurator Silicon Being-Teams organisieren und verwalten, Aufgaben zuweisen, Fortschritt verfolgen und komplexe Zusammenarbeitsziele erreichen.

Projekte sind die Kernorganisationseinheit des Silicon Being-Systems, jedes Projekt hat ein unabhängiges Aufgabensystem, Arbeitsnotizen und Mitgliederliste.

## Hauptfunktionen

### Kernmerkmale

- **Projekt-Lebenszyklus-Verwaltung**: Erstellen, Archivieren, Wiederherstellen, Löschen von Projekten
- **Mitgliederverwaltung**: Zuweisen und Entfernen von Projektmitgliedern
- **Aufgaben-Zusammenarbeit**: Projektspezifisches Aufgabenverwaltungssystem
- **Arbeitsnotizen**: Projekt-Level gemeinsame Arbeitsnotizen
- **Projektinformationen**: Verwaltung von Metadaten wie Name, Beschreibung, Status
- **Archivierungsmechanismus**: Unterstützt Projektarchivierung und -wiederherstellung, schützt historische Daten

### Projektstatus

| Status | Beschreibung | Ausführbare Aktionen |
|--------|--------------|---------------------|
| Aktiv | Normal betriebenes Projekt | Alle Aktionen |
| Archiviert | Pausiertes Projekt, Daten erhalten | Wiederherstellen, Anzeigen |
| Gelöscht | Permanent gelöschtes Projekt | Keine |

## Zugriff auf Projektmanagement

### Einstiegspunkte

**Exklusiv für Silicon Kurator**:
- Projektmanagement-Tools nur für Silicon Kurator verfügbar
- AI durch Konversation bitten, Projektmanagement-Aktionen auszuführen
- Projektzuordnung auf der Silicon Being-Verwaltungsoberfläche anzeigen

### Projektseite

Jedes Projekt hat eine eigene Seite:
1. Besuchen Sie `/project/{ProjektID}`, um Projektdetails anzuzeigen
2. Projektgrundinformationen anzeigen (Name, Beschreibung, Status)
3. Auf Projekt-Arbeitsnotizen zugreifen
4. Auf Projekt-Aufgabenliste zugreifen
5. Projektmitgliederliste anzeigen

## Projekt erstellen

### Erstellvorgang

Projekt mit AI-Tools erstellen:
```
Bitte erstellen Sie ein neues Projekt:
- Name: AI-Assistent-Entwicklungsprojekt
- Beschreibung: Entwicklung der Kernfunktionen des intelligenten Assistenzsystems
```

### Projektinformationen

Bei Projekterstellung erforderlich:
- **Projektname**: Projektziel klar ausdrücken (erforderlich)
- **Projektbeschreibung**: Projektinhalt detailliert beschreiben (optional, aber empfohlen)

### Automatische Einstellungen nach Erstellung

- **Projekt-ID**: System generiert automatisch eindeutige GUID
- **Erstellungszeit**: UTC-Erstellungszeit aufzeichnen
- **Anfangsstatus**: Aktiver Status
- **Mitgliederliste**: Anfangs leer, muss später zugewiesen werden

## Projektmitglieder verwalten

### Mitglieder dem Projekt zuweisen

Silicon Being zum Projekt hinzufügen:
```
Bitte weisen Sie Silicon Being XXX dem Projekt YYY zu
```

Erforderlich:
- **Projekt-ID**: GUID des Zielprojekts
- **Silicon Being-ID**: GUID des zuzuweisenden Silicon Beings

### Mitglieder aus Projekt entfernen

Silicon Being aus Projekt entfernen:
```
Bitte entfernen Sie Silicon Being XXX aus dem Projekt YYY
```

### Projektmitglieder anzeigen

Alle Mitglieder des Projekts auflisten:
```
Bitte listen Sie alle Mitglieder des Projekts XXX auf
```

### Mitgliederberechtigungen

- **Projektmitglieder**: Können Projektaufgaben erstellen, Projektnotizen schreiben
- **Nicht-Projektmitglieder**: Kein Zugriff auf Projektressourcen
- **Silicon Kurator**: Kann Mitglieder aller Projekte verwalten

## Projektinformationen aktualisieren

### Projektname ändern

```
Bitte ändern Sie den Namen des Projekts XXX in neuen Projektnamen
```

### Projektbeschreibung ändern

```
Bitte ändern Sie die Beschreibung des Projekts XXX in neue Projektbeschreibung
```

### Name und Beschreibung gleichzeitig aktualisieren

```
Bitte aktualisieren Sie Projekt XXX:
- Name: Neuer Name
- Beschreibung: Neue Beschreibung
```

## Projekt-Lebenszyklus-Verwaltung

### Projekt archivieren

Vorübergehend inaktive Projekte archivieren:
```
Bitte archivieren Sie Projekt XXX
```

**Archivierungseffekt**:
- Projektstatus ändert sich zu ""Archiviert""
- Projektdaten vollständig erhalten
- Mitglieder bleiben zugeordnet
- Kann aus Archivliste wiederhergestellt werden

**Archivierungsszenarien**:
- Projekt vorübergehend ausgesetzt
- Phasenprojekt abgeschlossen
- Projektliste muss organisiert werden

### Projekt wiederherstellen

Projekt aus Archivstatus wiederherstellen:
```
Bitte stellen Sie Projekt XXX wieder her
```

**Wiederherstellungseffekt**:
- Projektstatus wechselt zurück zu ""Aktiv""
- Alle Daten vollständig wiederhergestellt
- Mitgliederbeziehungen unverändert
- Alle Funktionen normal nutzbar

### Projekt löschen

Projekt permanent löschen (nicht wiederherstellbar):
```
Bitte löschen Sie Projekt XXX
```

**Löschungseffekt**:
- Projektdaten permanent gelöscht
- Zugehörige Aufgaben und Arbeitsnotizen bereinigt
- Mitgliederbeziehungen aufgehoben
- **Aktion nicht rückgängig machbar**

**Vor Löschung bestätigen**:
- Bestätigen, dass Projekt wirklich nicht mehr benötigt wird
- Wichtige Daten backupen (falls erforderlich)
- Projektmitglieder benachrichtigen

### Projektliste und Details anzeigen

**Alle aktiven Projekte auflisten**:
```
Bitte listen Sie alle Projekte auf
```

**Einschließlich archivierter Projekte**:
```
Bitte listen Sie alle Projekte auf, einschließlich archivierter
```

### Projektdetails anzeigen

Vollständige Projektinformationen abrufen:
```
Bitte holen Sie sich die Details von Projekt XXX
```

Zurückgegebene Informationen:
- Projekt-ID
- Projektname
- Projektbeschreibung
- Projektstatus
- Erstellungszeit
- Mitgliederliste
- Aufgabenstatistik

## Projekt-Zusammenarbeitsfunktionen

### Projektaufgaben

Jedes Projekt hat ein unabhängiges Aufgabensystem:

**Projektaufgabe erstellen**:
```
Bitte erstellen Sie eine Aufgabe im Projekt XXX:
- Titel: Benutzer-Login-Funktion implementieren
- Beschreibung: Benutzer-Authentifizierungs- und Autorisierungsmodul vervollständigen
- Priorität: 1
```

**Aufgabenpriorität**:
- Kleinere Zahl = höhere Priorität
- Priorität 1: Höchste Priorität
- Priorität 5: Normale Priorität

**Aufgabenstatus-Fluss**:
1. **Ausstehend** (Pending): Aufgabe erstellt, wartet auf Start
2. **Laufend** (Running): Aufgabe gestartet
3. **Abgeschlossen** (Completed): Aufgabe erfolgreich abgeschlossen
4. **Fehlgeschlagen** (Failed): Aufgabenausführung fehlgeschlagen
5. **Storniert** (Cancelled): Aufgabe storniert

### Projekt-Arbeitsnotizen

Jedes Projekt hat ein gemeinsames Arbeitsnotizensystem:

**Projektnotiz erstellen**:
```
Bitte erstellen Sie eine Arbeitsnotiz im Projekt XXX:
- Zusammenfassung: Projektfortschritt Update - Woche 1
- Inhalt: Diese Woche wurde Anforderungsanalyse abgeschlossen...
- Schlagworte: Fortschritt, Wochenbericht
```

**Projektnotiz-Merkmale**:
- Alle Projektmitglieder können erstellen und bearbeiten
- Nach Seitennummer organisiert, praktisch für Nachverfolgung
- Autoreninformationen aufzeichnen, Beitrag nachverfolgen
- Markdown-Format unterstützt

### Vergleich von Projekt- und persönlichen Ressourcen

| Ressourcentyp | Persönlich | Projekt |
|--------------|------------|---------|
| Arbeitsnotizen | Privates Tagebuch | Gemeinsame Zusammenarbeitsnotizen |
| Aufgaben | Persönliche Aufgaben | Projekt-Zusammenarbeitsaufgaben |
| Wissensgraph | Persönliches Wissen | Projekt-Wissen (Zukunft) |
| Speicher | Persönlicher Speicher | Projekt-Speicher (Zukunft) |

## Anwendungsszenarien

### Software-Entwicklungsprojekt

**Szenario**: Team für Software-Entwicklung organisieren

**Beispiel**:
```
1. Projekt erstellen: Web-Anwendung-Entwicklung
2. Mitglieder zuweisen: Frontend-Entwicklung, Backend-Entwicklung, Testing
3. Aufgaben erstellen:
   - Benutzer-Authentifizierungsmodul implementieren (Priorität 1)
   - Datenbank-Architektur entwerfen (Priorität 1)
   - API-Dokumentation erstellen (Priorität 2)
4. Wöchentlich Projekt-Notizen aktualisieren, um Fortschritt aufzuzeichnen
5. Aufgabenerledigung verfolgen
```

### Forschungsprojekt

**Szenario**: AI-Forschung zusammenarbeiten

**Beispiel**:
```
1. Projekt erstellen: Wissensgraph-Optimierungsforschung
2. Mitglieder zuweisen: Forscher A, Forscher B
3. Aufgaben erstellen:
   - Literaturreview (Priorität 1)
   - Algorithmus-Verbesserungsexperiment (Priorität 2)
   - Forschungsbericht erstellen (Priorität 3)
4. Experiment-Notizen im Projekt aufzeichnen
5. Abgeschlossene Forschungsphasen archivieren
```

### Lernprojekt

**Szenario**: Lerngruppe organisieren

**Beispiel**:
```
1. Projekt erstellen: Python-Lerngruppe
2. Mitglieder zuweisen: Lernender A, Lernender B, Mentor
3. Aufgaben erstellen:
   - Woche 1: Grundlegende Syntax lernen (Priorität 1)
   - Woche 2: Objektorientierte Programmierung (Priorität 2)
   - Woche 3: Projekt-Praxis (Priorität 3)
4. Lernnotizen und Reflexionen aufzeichnen
5. Abgeschlossene Lern-Meilensteine markieren
```

## Beste Praktiken

### 1. Projekt-Namenskonvention

- Klaren Projektnamen verwenden
- Projekttyp oder Ziel enthalten
- Mehrdeutige Abkürzungen vermeiden

**Gute Beispiele**:
- ""Web-Anwendung-Backend-Entwicklung""
- ""AI-Modell-Optimierungsforschung""
- ""2024-Technologie-Dokument-Organisation""

**Schlechte Beispiele**:
- ""Projekt 1""
- ""Test""
- ""Neues Projekt""

### 2. Projekt detailliert beschreiben

- Projektziel und -umfang detailliert beschreiben
- Wichtige Meilensteine aufzeichnen
- Erwartete Ergebnisse beschreiben

### 3. Mitglieder vernünftig zuweisen

- Mitglieder basierend auf Fähigkeiten und Rollen zuweisen
- Vermeiden, dass ein Mitglied an zu vielen Projekten teilnimmt
- Regelmäßig Mitgliederbeteiligung überprüfen

### 4. Aufgabenprioritäts-Verwaltung

- Wichtige Aufgaben hohe Priorität setzen (1-2)
- Normale Aufgaben mittlere Priorität setzen (3-4)
- Optionale Aufgaben niedrige Priorität setzen (5+)

### 5. Projekt-Notizen regelmäßig aktualisieren

- Mindestens einmal pro Woche Fortschritt aktualisieren
- Wichtige Entscheidungen und Änderungen aufzeichnen
- Kontinuität und Vollständigkeit der Notizen erhalten

### 6. Projekte rechtzeitig archivieren

- Abgeschlossene Projekte rechtzeitig archivieren
- Langfristig inaktive Projekte zur Archivierung in Betracht ziehen
- Vor Archivierung bestätigen, dass Daten gespeichert sind

### 7. Löschfunktion vorsichtig verwenden

- Vor Löschung doppelt bestätigen
- Erst archivieren und eine Zeit lang beobachten in Betracht ziehen
- Bestätigen, dass historische Daten nicht benötigt werden

## Berechtigungsverwaltung

### Projektmanagement-Berechtigungen

- **Silicon Kurator**:
  - Projekt erstellen, archivieren, wiederherstellen, löschen
  - Projektmitglieder zuweisen und entfernen
  - Projektinformationen aktualisieren
  - Alle Projekte anzeigen

- **Projektmitglieder**:
  - Projektinformationen anzeigen
  - Projektaufgaben erstellen und bearbeiten
  - Projektnotizen erstellen und bearbeiten
  - Kann Projekt selbst nicht verwalten

- **Nicht-Projektmitglieder**:
  - Kein Zugriff auf Projektressourcen
  - Kann Projektinformationen nicht anzeigen

### Tool-Berechtigungen

- `project` Tool: Mit `[SiliconManagerOnly]` markiert, nur Silicon Kurator
- `project_task` Tool: Verfügbar für Projektmitglieder und Silicon Kurator
- `project_work_note` Tool: Verfügbar für Projektmitglieder und Silicon Kurator

## Häufig gestellte Fragen

### F1: Gibt es eine Obergrenze für die Anzahl der Projekte?

**A**: Die aktuelle Version hat keine harte Begrenzung. Wir empfehlen jedoch, eine vernünftige Anzahl von Projekten beizubehalten, um die Verwaltung zu erleichtern. Zu viele Projekte können zu Verwaltungskonfusion führen.

### F2: Belegen archivierte Projekte Speicherplatz?

**A**: Ja, alle Daten archivierter Projekte (Aufgaben, Notizen usw.) werden erhalten. Wenn Speicherplatz knapp wird, können Sie die Löschung nicht mehr benötigter Projekte in Betracht ziehen.

### F3: Kann ich gleichzeitig mehreren Projekten angehören?

**A**: Ja. Ein Silicon Being kann gleichzeitig mehreren Projekten zugewiesen werden und zwischen verschiedenen Projekten wechseln.

### F4: Können Projektmitglieder selbst aus dem Projekt austreten?

**A**: Die aktuelle Version unterstützt keinen selbstständigen Austritt von Mitgliedern. Der Silicon Kurator muss die Entfernungsaktion ausführen.

### F5: Können archivierten Projekte Aufgaben hinzugefügt werden?

**A**: Nein. Archivierte Projekte sind im Nur-Lese-Status und müssen in den aktiven Status wiederhergestellt werden, um weiterhin hinzuzufügen oder zu modifizieren.

### F6: Werden beim Löschen eines Projekts auch Aufgaben und Notizen gelöscht?

**A**: Ja. Das Löschen eines Projekts bereinigt alle zugehörigen Daten (Aufgaben, Arbeitsnotizen usw.). Diese Aktion ist nicht wiederherstellbar.

### F7: Kann ich ein Projekt kopieren?

**A**: Die aktuelle Version unterstützt keine direkte Projektkopie. Sie müssen ein neues Projekt manuell erstellen und Mitglieder und Aufgaben neu konfigurieren.

### F8: Haben Projekte einen Endtermin?

**A**: Die aktuelle Version erzwingt keine Endtermine. Sie können jedoch den geplanten Zeitplan in der Projektbeschreibung aufzeichnen oder die Zeit durch Aufgaben verwalten.

## Verwandte Dokumentation

- [Aufgabenverwaltung](/help/task) - Persönliche Aufgabenverwaltung verstehen
- [Arbeitsnotizen](/help/work-notes) - Arbeitsnotizensystem verstehen
- [Silicon Being-Verwaltung](/help/being-management) - Verstehen, wie Silicon Beings verwaltet werden
- [Berechtigungsverwaltung](/help/permission) - Berechtigungskontrollmechanismus verstehen
- [Audit-Log](/help/audit-log) - Projektaktionsprotokoll anzeigen
";
    
    public override string Logging => @"
# Protokollierungssystem

## Übersicht

Das Protokollierungssystem ist eine Kerninfrastruktur der Silicon Life Collective Plattform und dient zur Aufzeichnung des Systembetriebsstatus, des Verhaltens von Silicon Beings, Fehlermeldungen und Debug-Daten. Das System gibt gleichzeitig an Konsole und Dateisystem aus und bietet flexible Log-Level-Filterung.

## Hauptfunktionen

- **Multi-Level-Protokollierung**: Unterstützt 6 Log-Level (Trace, Debug, Information, Warning, Error, Critical)
- **Mehrere Ausgabeziele**: Unterstützt gleichzeitig farbige Konsolenausgabe und Dateisystem-Persistenz
- **Intelligentes Filtern**: Unterstützt Filterung nach Log-Level, Zeitraum und Silicon Being
- **Kategorisierte Verwaltung**: Organisiert Logs nach Kategorie zur einfacheren Problemlokalisierung
- **Ausnahmeaufzeichnung**: Zeichnet automatisch Exception-Stack-Traces zur Fehlerbehebung auf

## Log-Level-Beschreibung

Das System definiert folgende Log-Level (in aufsteigender Schwere):

| Level | Beschreibung | Anwendungsfall |
|-------|-------------|----------------|
| **Trace** | Detaillierteste Logs | Entwicklung-Debugging, enthält sensitive Daten |
| **Debug** | Debug-Informationen | Interaktive Untersuchung, Debugging-Zwecke |
| **Information** | Allgemeine Informationen | Verfolgung des normalen Anwendungsflusses, langfristiger Wert |
| **Warning** | Warnmeldungen | Anormale oder unerwartete Ereignisse, stoppt aber die Ausführung nicht |
| **Error** | Fehlermeldungen | Aktueller Ausführungsfluss aufgrund von Fehler gestoppt |
| **Critical** | Kritische Fehler | Systemabsturz oder katastrophaler Ausfall, sofortige Behandlung erforderlich |
| **None** | Keine Protokollierung | Zum Deaktivieren der Protokollierung für eine bestimmte Kategorie |

## Bedienungsanleitung

### Wo kann ich Logs ansehen?

Das System bietet zwei Möglichkeiten zur Log-Anzeige:

1. **Konsolen-Logs**: Beim Ausführen des Programms zeigt das Terminal-Fenster farbige Logs in Echtzeit an
   - Verschiedene Farben repräsentieren unterschiedliche Schweregrade (rot=Fehler, gelb=Warnung etc.)
   - Geeignet für Entwicklung-Debugging und Echtzeit-Monitoring

2. **Datei-Logs**: Alle Logs werden automatisch im `data/Log/` Verzeichnis gespeichert
   - Permanent gespeichert, Verlauf jederzeit einsehbar
   - Geeignet für Problemverfolgung und Nachanalyse

## Konfigurationsoptionen

### Log-Ausführlichkeit anpassen

Sie können die Log-Ausführlichkeit in der Konfigurationsdatei einstellen:

- **Während Entwicklung/Debugging**: Auf ""Debug"" oder ""Trace"" setzen, um detailliertere Informationen zu sehen
- **Tägliche Nutzung**: Auf ""Information"" setzen, um normale Operationen aufzuzeichnen
- **Produktionsumgebung**: Auf ""Warning"" setzen, um nur Probleme und Fehler aufzuzeichnen

## Log-Anzeige und Filterung

Log-Dateien werden im `data/Log/` Verzeichnis gespeichert, automatisch nach Zeit organisiert für einfache Verlaufssuche.

## Log-Anzeigeformat

### Konsolen-Anzeigestil

```
[2026-04-27 10:30:00.123] [INFO] [Category] [Being:guid] Log-Nachricht
```

- Zeitstempel: Precise auf Millisekunden
- Level: Rechtsbündig, 4 Zeichen breit
- Kategorie: Log-Quellkennung
- Silicon Being ID: Nur angezeigt, wenn Log sich auf bestimmtes Being bezieht

### Log-Eintragsattribute

Jeder Log-Eintrag enthält folgende Informationen:

- **BeingId**: Zugehörige Silicon Being ID (leer für System-Logs)
- **Timestamp**: Log-Erstellungszeit (UTC)
- **Level**: Log-Level
- **Category**: Log-Kategorie
- **Message**: Log-Nachricht
- **Exception**: Exception-Information (falls vorhanden)

## Best Practices

### 1. Angemessene Log-Level wählen

- **Entwicklungsumgebung**: Debug oder Trace Level verwenden
- **Produktionsumgebung**: Information oder Warning Level verwenden
- **Fehlerbehebung**: Temporär auf Debug Level senken

### 2. Auf welche Logs sollten Sie achten?

Es wird empfohlen, folgende Situationen zu重点关注:
- Systemstart- und Shutdown-Informationen
- Silicon Being-Erstellung, Laufzeitanomalien und Shutdown
- Berechtigungsvalidierungsfehler und Sicherheitsprobleme
- KI-Aufruffehler und Antwortanomalien
- Tool-Ausführungsfehler und Timeouts

### 3. Performance-Beeinträchtigung durch übermäßige Logs vermeiden

Wenn Log-Dateien zu groß werden, können Sie die Log-Ausführlichkeitseinstellung erhöhen (z.B. auf ""Warning"" oder ""Error"" ändern), um die Log-Menge zu reduzieren.

### 4. Schutz sensibler Informationen

Das System zeichnet NICHT folgende sensitive Informationen in Logs auf:
- Passwörter und Keys
- Personenbezogene Daten
- API-Keys und Tokens
- Datenbank-Verbindungsstrings

## FAQ

### F: Warum kann ich einige Log-Nachrichten nicht sehen?

**A**: Mögliche Gründe:
1. Log-Ausführlichkeitseinstellung ist zu hoch, filtert einige Informationen heraus (kann in Konfiguration auf ""Debug"" oder ""Trace"" angepasst werden)
2. System startete nicht korrekt, Logging-Funktionalität nicht initialisiert

### F: Wie kann ich detailliertere Logs ansehen?

**A**: Ändern Sie die Log-Ausführlichkeitseinstellung in der Konfigurationsdatei auf ""Debug"" oder ""Trace"", dann starten Sie das System neu.

### F: Wo befinden sich die Log-Dateien?

**A**: Log-Dateien werden im `data/Log/` Verzeichnis gespeichert, nach Zeit indiziert.

## Fehlerbehebung

### Problem: Historische Log-Aufzeichnungen nicht auffindbar

**Symptome**: Vergangene Logs nicht einsehbar

**Lösungen**:
1. Prüfen, ob `data/Log/` Verzeichnis existiert und Lese-/Schreibberechtigungen hat
2. Bestätigen, dass ausreichender Festplattenspeicher verfügbar ist

## Hinweise

- Das Logging-System gewährleistet stabilen Betrieb; Ausfall eines einzelnen Ausgabeziels beeinflusst andere Ziele nicht
- Wenn Log-Dateien zu groß werden, können Sie die Log-Ausführlichkeitseinstellung erhöhen (z.B. auf ""Warning"" oder ""Error"" ändern), um das Log-Volumen zu reduzieren
- Bei großen Log-Schreibmengen wird empfohlen, das Log-Level angemessen zu erhöhen
";
    
    #endregion
}
