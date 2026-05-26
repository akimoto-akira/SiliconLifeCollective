# Fehlerbehebungsleitfaden

> **Version: v0.2.0-alpha**

[English](../en/troubleshooting.md) | **Deutsch** | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | [Čeština](../cs-CZ/troubleshooting.md) | [Русский](../ru-RU/troubleshooting.md)

## Häufige Probleme

### Bauen und Kompilieren

#### Problem: Build schlägt fehl, fehlende Abhängigkeiten

**Symptome**:
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**Lösung**:
```bash
dotnet restore
dotnet build
```

#### Problem: .NET SDK nicht gefunden

**Symptome**:
```
The .NET SDK could not be found
```

**Lösung**:
1. .NET 9 SDK installieren: https://dotnet.microsoft.com/download/dotnet/9.0
2. Installation verifizieren:
```bash
dotnet --version
```

---

### KI-Verbindungsprobleme

#### Problem: Ollama-Verbindung abgelehnt

**Symptome**:
```
Failed to connect to Ollama at http://localhost:11434
```

**Lösung**:
```bash
# Prüfen ob Ollama läuft
ollama list

# Ollama starten
ollama serve

# Verbindung testen
curl http://localhost:11434/api/tags
```

#### Problem: Modell nicht gefunden

**Symptome**:
```
model "qwen2.5:7b" not found
```

**Lösung**:
```bash
# Benötigtes Modell pullen
ollama pull qwen2.5:7b

# Verfügbare Modelle auflisten
ollama list
```

#### Problem: DashScope 404-Fehler

**Symptome**:
```
HTTP 404: Model not found
```

**Lösung**:
1. API-Schlüssel auf Richtigkeit prüfen
2. Modellnamen mit dem DashScope-Katalog abgleichen
3. Regionalen Endpunkt verifizieren
4. Kontozugriff auf das Modell prüfen

#### Problem: Volcengine Ark-Verbindung fehlgeschlagen

**Symptome**:
```
HTTP 401: Unauthorized
oder
HTTP 404: Endpoint not found
```

**Lösung**:
1. API-Schlüssel auf Richtigkeit prüfen
2. Endpunkt-URL-Format überprüfen (Standard: `https://ark.cn-beijing.volces.com/api/v3/chat/completions`)
3. Bestätigen, dass der Model-Parameter eine Inferenz-Endpunkt-ID verwendet (z.B. `ep-20241212123456-abcde`), keinen Modellnamen
4. Kontozugriff auf den Endpunkt prüfen

---

### Laufzeitprobleme

#### Problem: Port bereits belegt

**Symptome**:
```
HttpListenerException: Address already in use
```

**Lösung**:

**Windows**:
```bash
netstat -ano | findstr :8080
taskkill /PID <PID> /F
```

**Linux/Mac**:
```bash
lsof -ti:8080 | xargs kill -9
```

**Oder den Port in der Konfiguration ändern**.

#### Problem: Being startet nicht

**Symptome**:
- Being-Status zeigt „Error"
- Logs zeigen Initialisierungsfehler

**Lösung**:
1. Prüfen, ob die Soul-Datei existiert und gültig ist
2. Verifizieren, dass der KI-Client konfiguriert ist
3. Logs auf spezifische Fehler prüfen:
```bash
tail -f logs/*.log
```

#### Problem: Nicht genügend Speicher

**Symptome**:
```
OutOfMemoryException
```

**Lösung**:
1. **SiliconLife.Default**: Heap-Größe erhöhen:
```bash
dotnet run --project src/SiliconLife.Default --server.gcHeapCount 4
```

2. **SiliconLife.Fast**: Die Fast-Version hat selbst einen höheren Speicherverbrauch (~500 MB). Wenn der Speicher dauerhaft knapp ist, wird empfohlen:
   - Anzahl der gleichzeitig laufenden Silicon Beings reduzieren
   - Alte Daten bereinigen, um Speicher freizugeben

3. Alte Daten bereinigen:
```bash
# Alte Logs archivieren
mv logs/ logs-archive/
mkdir logs

# Alte Erinnerungen bereinigen
# Über Web-UI: Speicherverwaltung > Bereinigen
```

> **Tipp**: SiliconLife.Default hat einen niedrigen Speicherverbrauch (~200 MB), geeignet für Umgebungen mit begrenztem Speicher; SiliconLife.Fast hat einen höheren Speicherverbrauch, aber bessere Performance, geeignet für Produktionsumgebungen.

---

### Berechtigungsprobleme

#### Problem: Berechtigung verweigert

**Symptome**:
```
Permission denied: FileAccess C:\Windows
```

**Lösung**:
1. Aktuelle Berechtigungen prüfen:
```bash
curl http://localhost:8080/api/permissions/list
```

2. Berechtigung erteilen:
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

3. Oder Web-UI verwenden: Berechtigungsverwaltung

#### Problem: Berechtigung läuft nicht ab

**Symptome**:
- Berechtigung nach Ablaufzeit noch aktiv

**Lösung**:
1. System-Clock-Synchronisation prüfen
2. Verifizieren, dass das `expiresAt`-Feld korrekt gesetzt ist
3. Berechtigungs-Cache leeren

---

### Web-UI-Probleme

#### Problem: Web-UI nicht erreichbar

**Symptome**:
- Browser zeigt „Connection refused"

**Lösung**:
1. Verifizieren, dass der Server läuft
2. Korrekte URL prüfen: `http://localhost:8080`
3. Firewall-Einstellungen prüfen
4. Logs auf Startfehler prüfen

#### Problem: SSE funktioniert nicht

**Symptome**:
- Echtzeit-Updates erscheinen nicht
- Chat streamt nicht

**Lösung**:
1. Browser-SSE-Unterstützung prüfen
2. Proxy-Buffering für SSE deaktivieren
3. Netzwerkstabilität prüfen
4. Anderen Browser versuchen

#### Problem: UI sieht beschädigt aus

**Symptome**:
- Styles sind falsch
- Layout ist beschädigt

**Lösung**:
1. Browser-Cache leeren
2. Anderen Skin versuchen: Einstellungen > Skin
3. Browser-Konsole auf Fehler prüfen
4. Browser-Erweiterungen deaktivieren

---

### Speicherprobleme

#### Problem: Daten nicht lesbar/schreibbar

**Symptome**:
```
IOException: Access denied
```

**Lösung**:
1. Dateiberechtigungen prüfen
2. Verifizieren, dass der Speicherpfad existiert
3. Festplattenspeicher prüfen
4. Mit angemessenen Berechtigungen ausführen

#### Problem: Datenkorruption

**Symptome**:
- JSON-Parse-Fehler
- Datenverlust

**Lösung**:
1. Aus Backup wiederherstellen
2. Speicherintegrität prüfen:
```bash
# Über Web-UI: System > Speicherprüfung
```

3. Korrupte Dateien manuell reparieren

#### Problem: SpeedyPack-Speicherdatei beschädigt (Fast-Version)

**Symptome**:
- `.spk`-Datei kann nicht geladen werden
- SpeedyStorage-Initialisierung schlägt fehl

**Lösung**:
1. `SiliconLife.Speedy.Manager`-Tool zum Prüfen und Reparieren von `.spk`-Dateien verwenden
2. Prüfen, ob die `.spk.idx`-Indexdatei mit der `.spk`-Datei übereinstimmt
3. Wenn die Indexdatei beschädigt ist, `.spk.idx`-Datei löschen — das System erstellt den Index automatisch neu
4. `.spk`-Datei aus Backup wiederherstellen

#### Problem: SpeedyPack-Auto-Komprimierung schlägt fehl (Fast-Version)

**Symptome**:
- `.spk`-Datei wächst kontinuierlich
- Festplattenspeicher wird knapp

**Lösung**:
1. Prüfen, ob `SpeedyPackAutoCompactor` ordnungsgemäß läuft
2. Komprimierungsoperation manuell auslösen
3. Komprimierungsschwellenwert-Konfiguration prüfen
4. `SiliconLife.Speedy.Manager`-Tool für manuelle Komprimierung verwenden

---

### Tool-Ausführungsprobleme

#### Problem: Tool nicht gefunden

**Symptome**:
```
Tool "xyz" not found
```

**Lösung**:
1. Tool-Namen auf Richtigkeit prüfen
2. Tool im Tools-Verzeichnis prüfen
3. Projekt neu bauen
4. Prüfen, ob das Tool korrekt implementiert ist

#### Problem: Tool gibt Fehler zurück

**Symptome**:
```
Tool execution failed: ...
```

**Lösung**:
1. Tool-Logs prüfen
2. Eingabeparameter verifizieren
3. Tool unabhängig testen
4. Berechtigungen prüfen

---

### Plugin-Probleme

#### Problem: Plugin-Laden fehlgeschlagen

**Symptome**:
```
Plugin load failed: Security check failed
```

**Lösung**:
1. Prüfen, ob das Plugin verbotene Namespaces referenziert (`System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis`)
2. Verifizieren, dass das Plugin nur Assemblys aus der vertrauenswürdigen Assembly-Whitelist referenziert
3. Prüfen, ob das Plugin die `IPlugin`-Schnittstelle korrekt implementiert
4. Logs auf detaillierte Sicherheitsprüfungsfehler prüfen

#### Problem: Plugin-Tools nicht registriert

**Symptome**:
- Plugin erfolgreich geladen, aber Tools erscheinen nicht in der Tool-Liste

**Lösung**:
1. Bestätigen, dass die Tool-Klasse im Plugin die `ITool`-Schnittstelle korrekt implementiert
2. Prüfen, ob die Tool-Klasse public ist
3. Verifizieren, dass `ToolManager.ScanAllPluginAssemblies()` aufgerufen wurde
4. Plugin neu bauen und Anwendung neu starten

---

### Arbeitsnotizen-Probleme

#### Problem: Arbeitsnotiz kann nicht erstellt werden

**Symptome**:
```
Failed to create work note
```

**Lösung**:
1. Prüfen, ob das Being existiert und läuft
2. Verifizieren, dass der Speicherpfad Schreibrechte hat
3. Prüfen, ob der Inhalt nicht leer ist (Inhalt ist erforderlich)
4. Logs auf detaillierte Fehlerinformationen prüfen

#### Problem: Notizsuche ohne Ergebnisse

**Symptome**:
- Schlüsselwortsuche gibt leere Ergebnisse zurück
- Es sind jedoch sicher relevante Notizen vorhanden

**Lösung**:
1. Schreibweise des Schlüsselworts prüfen
2. Generischeres Schlüsselwort versuchen
3. Verifizieren, dass die Notiz das Schlüsselwort enthält (Groß-/Kleinschreibung beachten)
4. `max_results`-Parameterwert erhöhen

#### Problem: Notizverzeichnis-Generierung langsam

**Symptome**:
- Lange Antwortzeit bei Verzeichnisgenerierung
- Being hat viele Notizen (>1000 Seiten)

**Lösung**:
1. Dies ist normal, da alle Notizen durchlaufen werden müssen
2. Alte Notizen regelmäßig archivieren
3. Suchfunktion statt Verzeichnis-Browsing verwenden
4. Geplante Optimierung: Verzeichnis-Cache-Mechanismus hinzufügen

---

### Wissensnetzwerk-Probleme

#### Problem: Wissensabfrage gibt leere Ergebnisse

**Symptome**:
```
No knowledge triples found
```

**Lösung**:
1. Schreibweise von Subjekt und Prädikat verifizieren
2. Prüfen, ob das Wissen zum Netzwerk hinzugefügt wurde
3. Suchfunktion für Fuzzy-Matching verwenden:
```json
{
  "action": "search",
  "query": "Schlüsselwort"
}
```

#### Problem: Wissenspfad-Suche schlägt fehl

**Symptome**:
```
No path found between concepts
```

**Lösung**:
1. Prüfen, ob beide Konzepte im Wissensnetzwerk existieren
2. Prüfen, ob ein Verbindungspfad existiert (möglicherweise keine direkte oder indirekte Beziehung)
3. Mehr Wissen hinzufügen, um Verbindungen herzustellen
4. Pfadlängenlimit senken (falls gesetzt)

#### Problem: Wissensvalidierung schlägt fehl

**Symptome**:
```
Knowledge validation failed
```

**Lösung**:
1. Prüfen, ob das Tripel-Format korrekt ist (Subjekt, Prädikat, Objekt sind erforderlich)
2. Konfidenzwert im Bereich 0.0–1.0 verifizieren
3. Auf duplizierte Tripel prüfen
4. Validierungsfehlerdetails auf spezifische Probleme prüfen

#### Problem: Wissensnetzwerk-Statistiken ungenau

**Symptome**:
- Statistikzahlen entsprechen nicht den Erwartungen
- Statistik nach Hinzufügen von Wissen nicht aktualisiert

**Lösung**:
1. Statistiken benötigen möglicherweise einige Sekunden zum Aktualisieren (Cache)
2. Prüfen, ob Löschoperationen erfolgreich ausgeführt wurden
3. Anwendung neu starten, um Statistikaktualisierung zu erzwingen
4. Statistik über API neu abfragen

---

### Projektmanagement-Probleme

#### Problem: Projekt kann nicht erstellt werden

**Symptome**:
```
Failed to create project
```

**Lösung**:
1. Prüfen, ob der Projektname nicht leer ist (erforderlich)
2. Verifizieren, dass der Projektname nicht bereits existiert
3. Prüfen, ob der Speicherpfad Schreibrechte hat
4. Logs auf detaillierte Fehlerinformationen prüfen

#### Problem: Projektdaten verloren

**Symptome**:
- Projektinformationen können nicht geladen werden
- Projektdateien sind beschädigt

**Lösung**:
1. Prüfen, ob das Projekt-Speicherverzeichnis existiert
2. Projektdaten aus Backup wiederherstellen
3. Verifizieren, dass das JSON-Dateiformat korrekt ist
4. Beschädigte Projektdateien manuell reparieren

#### Problem: Projektrollenzuweisung schlägt fehl

**Symptome**:
```
Failed to assign role
```

**Lösung**:
1. Bestätigen, dass das Silicon Being dem Projekt beigetreten ist
2. Prüfen, ob der Rollenname gültig ist
3. Verifizieren, dass der Ausführende der Silicon Curator ist
4. Logs auf detaillierte Fehlerinformationen prüfen

#### Problem: Workflow kann nicht gestartet werden

**Symptome**:
- Workflow-Instanzerstellung schlägt fehl
- Zustandsübergänge werden nicht ausgeführt

**Lösung**:
1. Prüfen, ob eine Workflow-Vorlage definiert ist
2. Verifizieren, dass der Anfangszustand korrekt gesetzt ist
3. Bestätigen, dass das Projekt an eine Workflow-Vorlage gebunden ist
4. Workflow-Logs auf Übergangsfehler prüfen

---

### Tool-Berechtigungsprobleme

#### Problem: Tool-Operation verweigert

**Symptome**:
```
Tool operation denied: network:post
```

**Lösung**:
1. Tool-Berechtigungskonfiguration des Silicon Beings prüfen:
```bash
curl http://localhost:8080/api/beings/tool-permissions?beingId=<id>
```

2. Tool-Berechtigungen aktualisieren:
```bash
curl -X PUT http://localhost:8080/api/beings/tool-permissions \
  -H "Content-Type: application/json" \
  -d '{
    "beingId": "being-uuid",
    "permissions": {
      "network:post": "allowed"
    }
  }'
```

3. Oder Web-UI verwenden: Beings → Tool-Berechtigungen

#### Problem: Projekt-Tool-Berechtigungen werden nicht wirksam

**Symptome**:
- Tool-Berechtigungen auf Projektebene funktionieren nicht wie erwartet

**Lösung**:
1. Bestätigen, dass die Berechtigungen auf Projektebene korrekt konfiguriert sind
2. Prüfen, ob Konflikte zwischen Being-Ebene und Projektebene bestehen
3. Berechtigungen auf Projektebene sind unabhängig von der Being-Ebene; es gilt die Schnittmenge beider
4. Audit-Protokoll prüfen, um Berechtigungsprüfungsergebnisse zu bestätigen

---

## Debugging

### Detaillierte Logs aktivieren

Konfiguration bearbeiten:
```json
{
  "logging": {
    "level": "debug"
  }
}
```

### Logs prüfen

Logs gespeichert in:
```
logs/
├── system.log
├── ai.log
├── permission.log
└── error.log
```

Echtzeit anzeigen:
```bash
tail -f logs/*.log
```

### Debugger verwenden

**SiliconLife.Default (Standardimplementierung)**:
```bash
# Mit Debugger ausführen
dotnet run --project src/SiliconLife.Default --configuration Debug

# Debugger anhängen
# Über IDE: An Prozess anhängen > SiliconLife.Default
```

**SiliconLife.Fast (Hochleistungsversion)**:
```bash
# Mit Debugger ausführen
dotnet run --project src/SiliconLife.Fast --configuration Debug

# Debugger anhängen
# Über IDE: An Prozess anhängen > SiliconLife.Fast
```

> **Empfehlung**: Für die Entwicklungs- und Debugging-Phase wird SiliconLife.Default empfohlen. Nach erfolgreicher Architekturverifizierung SiliconLife.Fast für Produktionsbereitstellung verwenden.

---

## Performance-Probleme

### Langsame Antwortzeiten

**Optimierung**:
1. KI-Modell-Komplexität senken
2. Cache aktivieren
3. Alte Daten bereinigen
4. Systemressourcen erhöhen

### Hohe CPU-Auslastung

**Prüfen**:
- Zu viele Beings laufen
- Endlosschleifen in Tools
- Häufige Timer-Ausführung

**Lösung**:
- Parallele Beings reduzieren
- Tool-Code optimieren
- Timer-Intervall anpassen

### Hohe Speichernutzung

**Überwachen**:
```bash
# Über Web-UI: Dashboard > Speicher
```

**Optimierung**:
- Alte Erinnerungen bereinigen
- Kontextgröße reduzieren
- Paginierung implementieren

---

## Hilfe erhalten

### Dokumentation ansehen

- [Schnellstart-Leitfaden](getting-started.md)
- [Entwicklungsleitfaden](development-guide.md)
- [API-Referenz](api-reference.md)
- [Architekturleitfaden](architecture.md)

### Logs prüfen

Immer zuerst Logs auf Fehlerdetails prüfen.

### Community-Support

- GitHub Issues: Bugs melden
- Discussions: Fragen stellen
- Dokumentation: Lösungen suchen

---

## Notfall-Prozeduren

### Systemabsturz

1. Logs auf Ursache prüfen
2. Anwendung neu starten:

**SiliconLife.Default (Standardimplementierung)**:
```bash
dotnet run --project src/SiliconLife.Default
```

**SiliconLife.Fast (Haupt-Produktionsversion)**:
```bash
dotnet run --project src/SiliconLife.Fast
```

3. Bei Bedarf aus Backup wiederherstellen

### Datenverlust

1. Anwendung sofort stoppen
2. Backup-Dateien prüfen
3. Daten wiederherstellen
4. Integrität verifizieren

### Sicherheitslücke

1. Alle Beings stoppen
2. Alle Berechtigungen widerrufen
3. Audit-Protokolle prüfen
4. Zugriffskontrolle überprüfen
5. Mit eingeschränkten Berechtigungen neu starten

---

## Prävention

### Best Practices

1. **Regelmäßige Backups**
   - Datenverzeichnis sichern
   - Konfiguration sichern
   - Wiederherstellungsprozess testen

2. **Ressourcen überwachen**
   - CPU/Speicher-Nutzung überwachen
   - Festplattenspeicher überwachen
   - Netzwerkverbindung prüfen

3. **Aktuell bleiben**
   - .NET SDK aktualisieren
   - Abhängigkeiten aktualisieren
   - Sicherheitspatches anwenden

4. **Änderungen testen**
   - Zuerst in der Entwicklungsumgebung testen
   - Versionskontrolle verwenden
   - Änderungen dokumentieren

---

## Nächste Schritte

- 📚 [Architekturleitfaden](architecture.md) lesen
- 🛠️ [Entwicklungsleitfaden](development-guide.md) ansehen
- 🚀 [Schnellstart-Leitfaden](getting-started.md) lesen
- 🔒 [Sicherheitsdokumentation](security.md) ansehen
