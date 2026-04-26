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
    public override string TaskTimer_Title => "Aufgaben und Timer";
    public override string Permission_Title => "Berechtigungsverwaltung";
    public override string Config_Title => "Konfiguration";
    public override string FAQ_Title => "Häufig gestellte Fragen";

    public override string[] GettingStarted_Tags => new[] { "Installation", "Start", "Einrichtung", "Schnellstart", "Erste Schritte", "Beginnen", "Initialisierung", "Umgebung" };
    public override string[] BeingManagement_Tags => new[] { "Being", "Erstellen", "Einstellung", "Being-Verwaltung", "Silicon Being", "Profil", "Konfiguration", "Verwaltung" };
    public override string[] ChatSystem_Tags => new[] { "Chat", "Nachricht", "Konversation", "Chat-System", "Dialog", "Kommunikation", "Gespräch", "Diskussion" };
    public override string[] TaskTimer_Tags => new[] { "Aufgabe", "Timer", "Zeitplan", "Aufgaben und Timer", "cron", "Automatisierung", "Wiederholung", "Erinnerung" };
    public override string[] Permission_Tags => new[] { "Berechtigung", "Sicherheit", "Zugriffskontrolle", "Berechtigungsverwaltung", "Authentifizierung", "Autorisierung", "Datenschutz", "Schutz" };
    public override string[] Config_Tags => new[] { "Konfiguration", "Einstellung", "Option", "Konfigurationsverwaltung", "Präferenzen", "Anpassung", "System", "Parameter" };
    public override string[] FAQ_Tags => new[] { "FAQ", "Hilfe", "Problem", "Support", "Fehlerbehebung", "Leitfaden", "Unterstützung", "Antwort" };
    
    public override string GettingStarted => @"
# Schnellstart

## Systemstart

Führen Sie das Programm in der Befehlszeile aus. Das System startet automatisch und lauscht auf Port 8080.

```bash
dotnet run
```

## Zugriff auf die Weboberfläche

Öffnen Sie Ihren Browser und navigieren Sie zu `http://localhost:8080`, um auf die Web-Verwaltungsoberfläche zuzugreifen.

## Erster Start

Beim ersten Start erstellt das System automatisch einen Silicon Curator (Administrator). Sie müssen Folgendes konfigurieren:

1. Curator-Namen festlegen
2. Soul-Datei (Prompt) konfigurieren
3. AI-Modell auswählen

## Grundlegende Bedienung

- **Dashboard**: Systemstatus und Statistiken anzeigen
- **Beings**: Silicon-Beings erstellen und verwalten
- **Chat**: Mit Silicon-Beings interagieren
- **Aufgaben**: Wiederkehrende Aufgaben konfigurieren
- **Konfiguration**: Systemeinstellungen anpassen
- **Hilfe**: Dieses Dokument anzeigen

## Tastenkürzel

- `F1` - Hilfsdokumentation öffnen
- `Ctrl+F` - Suchfeld fokussieren
";

    public override string BeingManagement => @"
# Being-Verwaltung

## Silicon-Being erstellen

1. Zur Seite ""Beings"" navigieren
2. Auf ""Neues Being erstellen"" klicken
3. Folgende Informationen eingeben:
   - **Name**: Anzeigename des Beings
   - **Soul-Datei**: Core-Prompt, der das Verhalten bestimmt (Markdown-Format unterstützt)
   - **AI-Modell**: Zu verwendendes AI-Modell auswählen
4. Auf ""Erstellen"" klicken, um abzuschließen

## Konfiguration der Soul-Datei

Die Soul-Datei ist der Core-Prompt des Silicon-Beings und bestimmt dessen Verhaltensmuster, Persönlichkeitsmerkmale und Fähigkeitsbereich.

### Erstellungsrichtlinien

```markdown
# Rollendefinition

Sie sind ein professioneller Programmier-Assistent. Fachgebiete:
- C#-Entwicklung
- Architekturdesign
- Code-Review

# Verhaltensrichtlinien

1. Immer ausführbare Codebeispiele bereitstellen
2. Wichtige Codeteile erläutern
3. Best-Practice-Empfehlungen geben
```

## Being-Verwaltung

- **Bearbeiten**: Namen, Soul-Datei und andere Einstellungen ändern
- **Löschen**: Being vollständig löschen (nicht wiederherstellbar)
- **Klonen**: Neue Version basierend auf bestehendem Being erstellen
";

    public override string ChatSystem => @"
# Chat-System

## Konversation starten

1. Silicon-Being für die Konversation auswählen
2. Nachricht in das Eingabefeld eingeben
3. Enter-Taste drücken oder auf Senden klicken

## Chat-Funktionen

- **Echtzeit-Antworten**: Streaming-Ausgabe mit SSE-Technologie
- **Tool-Aufrufe**: AI ruft Tools auf, um Operationen auszuführen
- **Kontextspeicher**: Konversationsverlauf speichern
- **Mehrsprachig**: Konversationen in mehreren Sprachen unterstützt

## Tool-Nutzung

Silicon-Beings können automatisch Tools aufrufen, um Folgendes auszuführen:
- Kalenderinformationen abfragen
- Systemeinstellungen verwalten
- Code ausführen
- Auf Dateisystem zugreifen (Berechtigung erforderlich)

## Konversation unterbrechen

Wenn die AI nachdenkt, können Sie:
- Auf ""Stoppen"" klicken
- Oder eine neue Nachricht senden, um automatisch zu unterbrechen
";

    public override string TaskTimer => @"
# Aufgaben und Timer

## Wiederkehrende Aufgabe erstellen

1. Zur Seite ""Aufgaben"" navigieren
2. Auf ""Neue Aufgabe"" klicken
3. Aufgabe konfigurieren:
   - **Aufgabenname**: Beschreibender Name
   - **Trigger**: Cron-Ausdruck
   - **Aktion**: Auszuführende Operation auswählen
   - **Ziel-Being**: Ausführendes Being auswählen

## Cron-Ausdruck

```
Minute (0-59)
| Stunde (0-23)
| | Tag (1-31)
| | | Monat (1-12)
| | | | Wochentag (0-6)
| | | | |
* * * * *
```

### Beispiele

- `0 * * * *` - Jede Stunde zur Minute 0
- `0 9 * * *` - Täglich um 9:00 Uhr
- `*/5 * * * *` - Alle 5 Minuten
- `0 9 * * 1-5` - Werktags um 9:00 Uhr

## Aufgabenverwaltung

- **Aktivieren/Deaktivieren**: Aufgabe vorübergehend deaktivieren
- **Bearbeiten**: Aufgabeneinstellungen ändern
- **Löschen**: Aufgabe löschen
- **Ausführungsverlauf**: Aufgabenausführungsprotokoll anzeigen
";

    public override string Permission => @"
# Berechtigungsverwaltung

## Berechtigungsebenen

Das System verwendet eine 5-stufige Berechtigungskontrolle:

1. **IsCurator**: Curator hat höchste Berechtigung
2. **UserFrequencyCache**: Benutzer-Frequenz-Cache-Einschränkung
3. **GlobalACL**: Globale Access Control List
4. **IPermissionCallback**: Benutzerdefinierter Berechtigungs-Callback
5. **IPermissionAskHandler**: Benutzer nach Berechtigung fragen

## Berechtigungstypen

- **Lesen**: Informationen und Daten anzeigen
- **Schreiben**: Daten ändern und erstellen
- **Ausführen**: Tools und Befehle ausführen
- **Verwalten**: Systemeinstellungen verwalten

## Berechtigungen konfigurieren

1. Zur Seite ""Konfiguration"" navigieren
2. ""Berechtigungseinstellungen"" auswählen
3. Berechtigungen konfigurieren:
   - Bestimmte Operationen erlauben/verweigern
   - Frequenzlimits festlegen
   - Whitelist/Blacklist konfigurieren

## Sicherheitsempfehlungen

- Berechtigungseinstellungen regelmäßig überprüfen
- Zugriff auf sensible Operationen einschränken
- Operationsprotokolle aktivieren
- Prinzip der minimalen Berechtigung befolgen
";

    public override string Config => @"
# Konfiguration

## Systemeinstellungen

### AI-Modell-Konfiguration

```json
{
  ""AI"": {
    ""DefaultProvider"": ""ollama"",
    ""Models"": {
      ""ollama"": {
        ""Endpoint"": ""http://localhost:11434"",
        ""Model"": ""llama3""
      }
    }
  }
}
```

### Netzwerkeinstellungen

- **Port**: Standard 8080
- **Host**: Standard localhost
- **HTTPS**: Optional aktivieren

### Speichereinstellungen

- **Datenverzeichnis**: Speicherort für Silicon-Being-Daten
- **Log-Level**: Debug/Info/Warning/Error
- **Backup-Strategie**: Automatische Backup-Häufigkeit

## Skin-Themes

Das System unterstützt mehrere Oberflächenthemes:

- **Light**: Helles Theme (Standard)
- **Dark**: Dunkles Theme
- **Custom**: Eigene Themes erstellen

## Lokalisierung

Unterstützte Sprachen:
- Chinesisch (vereinfacht) (zh-CN)
- Chinesisch (traditionell) (zh-HK)
- Englisch (en-US, en-GB)
- Japanisch (ja-JP)
- Koreanisch (ko-KR)
- Spanisch (es-ES, es-MX)
- Tschechisch (cs-CZ)
- Deutsch (de-DE)
";

    public override string FAQ => @"
# Häufig gestellte Fragen

## Systembezogen

### F: Was tun, wenn das System nicht startet?

A: Bitte überprüfen Sie:
1. Ob Port 8080 bereits verwendet wird
2. Ob .NET 9 Runtime korrekt installiert ist
3. Log-Dateien für detaillierte Fehlermeldungen

### F: Wie ändere den Listening-Port?

A: Ändern Sie `WebHost:Port` in der Konfigurationsdatei oder verwenden Sie Befehlszeilenargumente.

### F: Wo werden die Daten gespeichert?

A: Standardmäßig im `data`-Ordner im Root-Verzeichnis des Silicon-Beings.

## AI-bezogen

### F: Was tun, wenn die AI-Antworten langsam sind?

A: Mögliche Ursachen:
1. Modell ist zu groß und benötigt mehr Rechenressourcen
2. Netzwerkverzögerung (bei Verwendung von Cloud-Modellen)
3. Verwendung lokaler Modelle (z. B. Ollama) in Betracht ziehen

### F: Wie wechsle ich das AI-Modell?

A: Ändern Sie `AI:DefaultProvider` in der Konfigurationsdatei oder wählen Sie ein anderes Modell in der Being-Konfiguration.

### F: Was tun, wenn die AI keine Tools aufrufen kann?

A: Überprüfen Sie:
1. Ob Tools korrekt registriert sind
2. Ob Berechtigungen Tool-Aufrufe erlauben
3. Ob das AI-Modell Tool-Aufruf-Funktionen unterstützt

## Being-bezogen

### F: Wie backuppe ich Being-Daten?

A: Kopieren Sie alle Dateien im Being-Verzeichnis an einen Backup-Speicherort.

### F: Kann ich Soul-Dateien importieren/exportieren?

A: Ja. Auf der Being-Bearbeitungsseite können Sie Soul-Dateien im Markdown-Format importieren/exportieren.

### F: Wie klone ich ein Being?

A: Wählen Sie ""Klonen"" in der Being-Liste. Das System erstellt eine Kopie, deren Einstellungen Sie ändern können.

## Hilfe erhalten

Wenn Ihr Problem weiterhin besteht:

1. [Projekt-Dokumentation](https://github.com/your-repo/docs) überprüfen
2. Issue eröffnen, um das Problem zu melden
3. An Community-Diskussionen teilnehmen
";
    
    #endregion
}
