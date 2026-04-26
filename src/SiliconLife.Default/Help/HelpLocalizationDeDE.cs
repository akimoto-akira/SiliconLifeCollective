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
    
    #endregion
}
