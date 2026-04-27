# Fehlerbehebungsleitfaden

> **Version: v0.1.0-alpha**

[English](../en/troubleshooting.md) | [Deutsch](../de-DE/troubleshooting.md) | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | [Čeština](../cs-CZ/troubleshooting.md)

## Häufige Probleme

### Bauen und Kompilieren

#### Problem: Build fehlschlägt, fehlende Abhängigkeiten

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

#### Problem: DashScope 404 Fehler

**Symptome**:
```
HTTP 404: Model not found
```

**Lösung**:
1. API-Schlüssel verifizieren
2. Modellnamen mit DashScope-Katalog abgleichen
3. Regionalen Endpunkt prüfen
4. Konto-Zugriff auf Modell prüfen

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

**Oder Port in Konfiguration ändern**.

#### Problem: Being startet nicht

**Symptome**:
- Being-Status zeigt "Error"
- Logs zeigen Initialisierungsfehler

**Lösung**:
1. Soul-Datei existiert und ist valide prüfen
2. KI-Client konfiguriert verifizieren
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
1. Heap-Größe erhöhen:
```bash
dotnet run --server.gcHeapCount 4
```

2. Alte Daten bereinigen:
```bash
# Alte Logs archivieren
mv logs/ logs-archive/
mkdir logs

# Alte Speicher bereinigen
# Über Web-UI: Speicherverwaltung > Bereinigen
```

---

### Berechtigungsprobleme

#### Problem: Berechtigung verweigert

**Symptome**:
```
Permission denied: disk:write
```

**Lösung**:
1. Aktuelle Berechtigungen prüfen:
```bash
curl http://localhost:8080/api/permissions
```

2. Berechtigung erteilen:
```bash
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

3. Oder Web-UI verwenden: Berechtigungsverwaltung

#### Problem: Berechtigung läuft nicht ab

**Symptome**:
- Berechtigung nach Ablaufzeit noch aktiv

**Lösung**:
1. System-Clock-Synchronisation prüfen
2. `expiresAt`-Feld korrekt gesetzt verifizieren
3. Berechtigungs-Cache leeren

---

### Web-UI-Probleme

#### Problem: Web-UI nicht erreichbar

**Symptome**:
- Browser zeigt "Connection refused"

**Lösung**:
1. Server läuft verifizieren
2. Korrekte URL prüfen: `http://localhost:8080`
3. Firewall-Einstellungen prüfen
4. Logs auf Startfehler prüfen

#### Problem: SSE funktioniert nicht

**Symptome**:
- Echtzeit-Updates erscheinen nicht
- Chat streamt nicht

**Lösung**:
1. Browser SSE-Unterstützung prüfen
2. Proxy-Buffering für SSE deaktivieren
3. Netzwerkstabilität prüfen
4. Anderen Browser versuchen

#### Problem: UI sieht beschädigt aus

**Symptome**:
- Styles falsch
- Layout kaputt

**Lösung**:
1. Browser-Cache leeren
2. Anderen Skin versuchen: Einstellungen > Skin
3. Browser-Konsole auf Fehler prüfen
4. Browser-Erweiterungen deaktivieren

---

### Storage-Probleme

#### Problem: Daten nicht lesbar/schreibbar

**Symptome**:
```
IOException: Access denied
```

**Lösung**:
1. Dateiberechtigungen prüfen
2. Storage-Pfad existiert verifizieren
3. Festplattenspeicher prüfen
4. Mit angemessenen Berechtigungen ausführen

#### Problem: Datenkorruption

**Symptome**:
- JSON-Parse-Fehler
- Datenverlust

**Lösung**:
1. Aus Backup wiederherstellen
2. Storage-Integrität prüfen:
```bash
# Über Web-UI: System > Storage-Prüfung
```

3. Korrupte Dateien manuell reparieren

---

### Tool-Ausführungsprobleme

#### Problem: Tool nicht gefunden

**Symptome**:
```
Tool "xyz" not found
```

**Lösung**:
1. Tool-Name korrekt verifizieren
2. Tool im Tools-Verzeichnis prüfen
3. Projekt neu bauen
4. Tool korrekt implementiert prüfen

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

### Arbeitsnotizen-Probleme

#### Problem: Arbeitsnotiz kann nicht erstellt werden

**Symptome**:
```
Failed to create work note
```

**Lösung**:
1. Being existiert und läuft prüfen
2. Storage-Pfad hat Schreibrechte verifizieren
3. Inhalt nicht leer prüfen (Inhalt erforderlich)
4. Logs auf detaillierte Fehler prüfen

#### Problem: Notizsuche ohne Ergebnisse

**Symptome**:
- Schlüsselwortsuche gibt leere Ergebnisse
- Aber relevante Notizen existieren sicher

**Lösung**:
1. Schlüsselwort-Schreibweise prüfen
2. Generischeres Schlüsselwort versuchen
3. Notiz enthält Schlüsselwort prüfen (Case-sensitive)
4. `max_results`-Parameterwert erhöhen

#### Problem: Notizverzeichnis-Generierung langsam

**Symptome**:
- Lange Antwortzeit bei Verzeichnisgenerierung
- Being hat viele Notizen (>1000 Seiten)

**Lösung**:
1. Normal, muss alle Notizen durchlaufen
2. Alte Notizen regelmäßig archivieren
3. Suchfunktion statt Verzeichnis-Browsing verwenden
4. Geplante Optimierung: Verzeichnis-Cache-Mechanismus

---

### Wissensnetzwerk-Probleme

#### Problem: Wissensabfrage gibt leere Ergebnisse

**Symptome**:
```
No knowledge triples found
```

**Lösung**:
1. Subjekt und Prädikat-Schreibweise verifizieren
2. Wissen wurde zum Netzwerk hinzugefügt prüfen
3. Suchfunktion für Fuzzy-Matching verwenden:
```json
{
  "action": "search",
  "query": "关键词"
}
```

#### Problem: Wissenspfad-Suche fehlschlägt

**Symptome**:
```
No path found between concepts
```

**Lösung**:
1. Beide Konzepte im Wissensnetzwerk existieren prüfen
2. Verbindungspfad existiert prüfen (vielleicht keine direkte/indirekte Beziehung)
3. Mehr Wissen hinzufügen um Verbindung herzustellen
4. Pfadlängenlimit senken (wenn gesetzt)

#### Problem: Wissensvalidierung fehlschlägt

**Symptome**:
```
Knowledge validation failed
```

**Lösung**:
1. Tripel-Format korrekt prüfen (Subjekt, Prädikat, Objekt erforderlich)
2. Konfidenzwert im Bereich 0.0-1.0 verifizieren
3. Duplizierte Tripel prüfen
4. Validierungsfehlerdetails auf spezifisches Problem prüfen

#### Problem: Wissensnetzwerk-Statistiken ungenau

**Symptome**:
- Statistikzahlen wie erwartet
- Statistik nach Wissen-Hinzufügen nicht aktualisiert

**Lösung**:
1. Statistiken brauchen vielleicht Sekunden zum Aktualisieren (Cache)
2. Lösch-Operation erfolgreich ausgeführt prüfen
3. Anwendung neu starten um Statistik zu forcieren
4. Statistik über API neu abfragen

---

### Projektmanagement-Probleme

#### Problem: Projekt kann nicht erstellt werden

**Symptome**:
```
Failed to create project
```

**Lösung**:
1. Projektname nicht leer prüfen (erforderlich)
2. Projektname nicht doppelt verifizieren
3. Storage-Pfad hat Schreibrechte prüfen
4. Logs auf detaillierte Fehler prüfen

#### Problem: Projektdaten verloren

**Symptome**:
- Projektinformationen nicht ladbar
- Projektdateien korrupt

**Lösung**:
1. Projekt-Storage-Verzeichnis existiert prüfen
2. Projektdaten aus Backup wiederherstellen
3. JSON-Dateiformat korrekt verifizieren
4. Korrupte Projektdateien manuell reparieren

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

```bash
# Mit Debugger ausführen
dotnet run --project src/SiliconLife.Default --configuration Debug

# Debugger attachen
# Über IDE: An Prozess attachen > SiliconLife.Default
```

---

## Performance-Probleme

### Langsame Antwortzeiten

**Optimieren**:
1. KI-Modell-Komplexität senken
2. Cache aktivieren
3. Alte Daten bereinigen
4. Systemressourcen erhöhen

### Hohe CPU-Auslastung

**Prüfen**:
- Zu viele Beings laufen
- Endlosschleife in Tools
- Häufige Timer-Ausführung

**Lösung**:
- Parallele Beings reduzieren
- Tool-Code optimieren
- Timer-Intervall anpassen

### Hohe Speichernutzung

**Monitor**:
```bash
# Über Web-UI: Dashboard > Speicher
```

**Optimieren**:
- Alte Speicher bereinigen
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
```bash
dotnet run --project src/SiliconLife.Default
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
3. Audit-Logs prüfen
4. Zugriffskontrolle prüfen
5. Mit eingeschränkten Berechtigungen neu starten

---

## Prävention

### Best Practices

1. **Regelmäßige Backups**
   - Datenverzeichnis backupen
   - Konfiguration backupen
   - Wiederherstellungsprozess testen

2. **Ressourcen überwachen**
   - CPU/Speicher-Nutzung monitorieren
   - Festplattenspeicher überwachen
   - Netzwerkverbindung prüfen

3. **Aktuell bleiben**
   - .NET SDK aktualisieren
   - Abhängigkeiten aktualisieren
   - Sicherheitspatches anwenden

4. **Änderungen testen**
   - Zuerst in Entwicklung testen
   - Versionskontrolle verwenden
   - Änderungen dokumentieren

---

## Nächste Schritte

- 📚 [Architekturleitfaden](architecture.md) lesen
- 🛠️ [Entwicklungsleitfaden](development-guide.md) prüfen
- 🚀 [Schnellstart-Leitfaden](getting-started.md) ansehen
- 🔒 [Sicherheitsdokumentation](security.md) prüfen
