# Beitragsleitfaden

> **Version: v0.2.0-alpha**

[English](../en/contributing.md) | **Deutsch** | [中文](../zh-CN/contributing.md) | [繁體中文](../zh-HK/contributing.md) | [Español](../es-ES/contributing.md) | [日本語](../ja-JP/contributing.md) | [한국어](../ko-KR/contributing.md) | [Čeština](../cs-CZ/contributing.md) | [Русский](../ru-RU/contributing.md)

Vielen Dank für Ihr Interesse an einem Beitrag zu SiliconLifeCollective!

## Dual-Version-Beiträge

Dieses Projekt hat zwei Implementierungsversionen. Sie können je nach Interesse einen Beitrag leisten:

### SiliconLife.Default (Standardversion)
- **Technologie-Stack**: .NET 9 Konsolenanwendung
- **Beitragsrichtung**: Kernfunktionsentwicklung, Tool-Implementierung, Lokalisierung, Dokumentation
- **Zielgruppe**: Alle Entwickler

### SiliconLife.Fast (Hochleistungsversion)
- **Technologie-Stack**: .NET 9 Windows Forms-Anwendung
- **Beitragsrichtung**: Performance-Optimierung, SpeedyPack-Speicher, System Tray, Lock-Free-Parallelität
- **Zielgruppe**: Entwickler mit Windows-Erfahrung und Interesse an Performance-Optimierung

> **Wichtiger Hinweis**: Beide Versionen teilen sich die Projekte SiliconLife.Core und SiliconLife.Common. Verbesserungen an Kernschnittstellen wirken sich auf beide Versionen aus.

## Verhaltenskodex

Dieses Projekt folgt der Apache 2.0-Lizenz. Bleiben Sie in allen Interaktionen respektvoll und professionell.

---

## Schnellstart

### 1. Repository forken

Klicken Sie auf "Fork" auf GitHub, um Ihre eigene Kopie zu erstellen.

### 2. Ihren Fork klonen

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. Entwicklungsumgebung einrichten

```bash
# .NET 9 SDK installieren
# https://dotnet.microsoft.com/download/dotnet/9.0

# Abhängigkeiten wiederherstellen
dotnet restore

# Projekt bauen
dotnet build

# Tests ausführen
dotnet test
```

### 4. Feature-Branch erstellen

```bash
git checkout -b feature/your-feature-name
```

### 5. Entwicklungsprojekt auswählen

Wählen Sie das passende Projekt je nach Beitragstyp:

- **Kernschnittstellen/abstrakte Klassen** → `SiliconLife.Core` modifizieren
- **Gemeinsame Implementierung** → `SiliconLife.Common` modifizieren
- **Default-Version-spezifisch** → `SiliconLife.Default` modifizieren
- **Fast-Version-spezifisch** → `SiliconLife.Fast` modifizieren
- **Speicher-Engine** → `SiliconLife.Speedy` modifizieren
- **Speicher-Verwaltungstool** → `SiliconLife.Speedy.Manager` modifizieren
- **Plugin-Entwicklung** → `SiliconLife.Core/Plugins` modifizieren
- **Mehrsprachige Dokumentation** → `docs/`-Verzeichnis modifizieren

---

## Entwicklungs-Workflow

### Code-Stil

- C#-Codierungskonventionen befolgen
- Klassennamen in PascalCase
- Methodenparameter in camelCase
- Private Felder in `_camelCase`
- Alle öffentlichen APIs müssen XML-Dokumentation haben

### Commit-Nachrichten

Folgen Sie dem **Conventional Commits**-Format:

```
<type>(<scope>): <description>
```

**Typen**:
- `feat`: Neue Funktion
- `fix`: Bug-Fix
- `docs`: Dokumentationsänderung
- `style`: Code-Formatierung
- `refactor`: Code-Refactoring
- `test`: Test-Änderung
- `chore`: Build/Tool-Änderung

**Beispiele**:
```bash
feat(localization): add Korean language support
fix(permission): fix null pointer in callback
docs: update contributing guide
refactor(web): simplify controller structure
```

### Änderungen vornehmen

1. **Code schreiben**
   - Bestehende Muster befolgen
   - Tests für neue Funktionen hinzufügen
   - Dokumentation aktualisieren

2. **Änderungen testen**
   ```bash
   # Alle Tests ausführen
   dotnet test
   
   # Im Release-Modus bauen
   dotnet build --configuration Release
   ```

3. **Code formatieren**
   ```bash
   dotnet format
   ```

4. **Änderungen committen**
   ```bash
   git add .
   git commit -m "feat(scope): description"
   ```

5. **Zu Ihrem Fork pushen**
   ```bash
   git push origin feature/your-feature-name
   ```

6. **Pull Request erstellen**
   - Zum Original-Repository navigieren
   - "Compare & pull request" klicken
   - PR-Vorlage ausfüllen
   - Einreichen

---

## Pull-Request-Leitfaden

### PR-Titel

Verwenden Sie dasselbe Format wie bei Commit-Nachrichten:
```
feat(localization): add Korean language support
```

### PR-Beschreibung

Beinhalten Sie:

1. **Was** - Was macht dieser PR?
2. **Warum** - Warum wird diese Änderung benötigt?
3. **Wie** - Wie haben Sie es implementiert?
4. **Test** - Wie wurde getestet?

### PR-Beschreibungsbeispiel

```markdown
## Was
Koreanische Lokalisierung für alle UI-Komponenten und Dokumentation hinzugefügt.

## Warum
Erweiterung der Projektzugänglichkeit für koreanischsprachige Benutzer.

## Wie
- KoKR.cs Lokalisierungsdatei erstellt
- 500+ Übersetzungsschlüssel hinzugefügt
- Alle Views zur Verwendung der Lokalisierung aktualisiert
- Koreanische Dokumentation in docs/ko-KR/ erstellt

## Test
- Verifiziert, dass alle UI-Elemente Koreanisch korrekt anzeigen
- Sprachwechsel-Funktionalität getestet
- Übersetzungen mit Muttersprachlern überprüft
```

---

## Beitragstypen

### 1. Bug-Fix

**Prozess**:
1. Bestehende Issues prüfen
2. Issue erstellen falls nicht vorhanden
3. Bug beheben
4. Testfall hinzufügen
5. PR einreichen

**Anforderungen**:
- Klar beschriebener Bug
- Reproduktionsschritte
- Test zur Verhinderung von Regressionen

### 2. Neue Funktion

**Prozess**:
1. Funktion in Issues/Discussions diskutieren
2. Genehmigung von Maintainer einholen
3. Funktion implementieren
4. Umfassende Tests hinzufügen
5. Dokumentation aktualisieren
6. PR einreichen

**Anforderungen**:
- Funktionsvorschlag genehmigt
- Vollständige Testabdeckung
- Dokumentation aktualisiert
- Abwärtskompatibel

### 3. Dokumentation

**Prozess**:
1. Dokumentationslücke identifizieren
2. Dokumentation schreiben/aktualisieren
3. PR einreichen

**Anforderungen**:
- Klar und prägnant
- Beispiele enthalten
- Mehrsprachig wo zutreffend

### 4. Code-Refactoring

**Prozess**:
1. Refactoring in Issue vorschlagen
2. Genehmigung einholen
3. Code refaktorieren
4. Sicherstellen, dass alle Tests bestehen
5. PR einreichen

**Anforderungen**:
- Keine Funktionsänderung
- Alle Tests bestehen
- Code-Qualität verbessert
- Klare Erklärung

---

## Test-Leitfaden

### Unit-Tests

```csharp
[TestMethod]
public void MyFeature_ShouldWork_AsExpected()
{
    // Arrange
    var service = new MyService();
    
    // Act
    var result = service.DoSomething();
    
    // Assert
    Assert.IsTrue(result.Success);
}
```

### Integrationstests

Vollständige Workflows testen:
- KI-Interaktion
- Tool-Ausführung
- Berechtigungsvalidierung
- Speicheroperationen

### Manuelle Tests

Für UI-Änderungen:
- In mehreren Browsern testen
- Responsives Design verifizieren
- Barrierefreiheit prüfen

---

## Dokumentations-Leitfaden

### Code-Kommentare

- XML-Kommentare für alle öffentlichen APIs
- Inline-Kommentare für komplexe Logik
- Code-Kommentare auf Englisch

### Dokumentationsdateien

- In `docs/{language}/` ablegen
- Alle Sprachversionen aktualisieren
- Bestehende Struktur befolgen

### Mehrsprachige Dokumentation

Beim Hinzufügen von Dokumentation:
1. Zunächst englische Version erstellen
2. In andere Sprachen übersetzen
3. Inhalte synchron halten

---

## Review-Prozess

### Was Maintainer prüfen

1. **Code-Qualität**
   - Konventionen befolgt
   - Klar und lesbar
   - Gut dokumentiert

2. **Tests**
   - Ausreichende Abdeckung
   - Alle Tests bestehen
   - Randfälle abgedeckt

3. **Dokumentation**
   - Aktualisiert
   - Klare Erklärungen
   - Mehrsprachig

4. **Kompatibilität**
   - Abwärtskompatibel
   - Keine Breaking Changes (außer nach Ankündigung)
   - Semantische Versionierung befolgt

### Review-Zeitleiste

- Erstes Review: 1-3 Tage
- Feedback-Integration: Nach Bedarf
- Merge: Nach Genehmigung

---

## Häufige Probleme

### PR abgelehnt

**Gründe**:
- Leitlinien nicht befolgt
- Unzureichende Tests
- Nicht angekündigte Breaking Changes
- Schlechte Code-Qualität

**Lösung**:
- Feedback umsetzen
- PR aktualisieren
- Erneut einreichen

### Merge-Konflikte

**Lösung**:
```bash
# Branch aktualisieren
git fetch origin
git rebase origin/master

# Konflikte lösen
# Konfliktdateien bearbeiten
git add .
git rebase --continue

# Force-Push
git push --force-with-lease
```

---

## Hilfe erhalten

### Ressourcen

- **Dokumentation**: [docs/](../)
- **Issues**: GitHub Issues
- **Diskussionen**: GitHub Discussions
- **Verhaltenskodex**: CODE_OF_CONDUCT.md

### Kontakt

- Issue für Bugs erstellen
- Discussion für Fragen starten
- Maintainer für dringende Angelegenheiten markieren

---

## Danksagung

Beitragende werden an folgenden Orten gewürdigt:
- README.md Mitwirkende-Sektion
- Release-Notes
- Projektdokumentation

---

## Lizenz

Durch Ihren Beitrag stimmen Sie zu, dass dieser unter der Apache 2.0-Lizenz lizenziert wird.

---

## Nächste Schritte

- 📚 [Dokumentation](../) lesen
- 🐛 [Offene Issues](https://github.com/akimoto-akira/SiliconLifeCollective/issues) ansehen
- 💬 [Diskussion](https://github.com/akimoto-akira/SiliconLifeCollective/discussions) starten
- 🚀 Forken und mit dem Beitrag beginnen!

Vielen Dank für Ihren Beitrag zu SiliconLifeCollective!🎉
