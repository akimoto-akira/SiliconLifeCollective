# Beitragshandbuch

> **Version: v0.2.0-alpha**

[English](../en/contributing.md) | **Deutsch** | [中文](../zh-CN/contributing.md) | [繁體中文](../zh-HK/contributing.md) | [Español](../es-ES/contributing.md) | [日本語](../ja-JP/contributing.md) | [한국어](../ko-KR/contributing.md) | [Čeština](../cs-CZ/contributing.md) | [Русский](../ru-RU/contributing.md)

Vielen Dank für Ihr Interesse an einem Beitrag zu SiliconLifeCollective!

## Beiträge zu zwei Versionen

Dieses Projekt hat zwei Implementierungsversionen. Sie können je nach Interesse eine Beitragsrichtung wählen:

### SiliconLife.Default (Standardversion)
- **Technologie-Stack**: .NET 9 Konsolenanwendung
- **Beitragsrichtung**: Kernfunktionsentwicklung, Werkzeugimplementierung, Lokalisierung, Dokumentation
- **Geeignet für**: Alle Entwickler

### SiliconLife.Fast (Hochleistungsversion)
- **Technologie-Stack**: .NET 9 plattformübergreifende Desktop-Anwendung (Avalonia UI)
- **Beitragsrichtung**: Leistungsoptimierung, SpeedyPack-Speicher, System-Tray, lockenfreie Nebenläufigkeit
- **Geeignet für**: Entwickler mit Desktop-Entwicklungserfahrung und Interesse an Leistungsoptimierung

> **Wichtiger Hinweis**: Beide Versionen teilen sich die Projekte SiliconLife.Core und SiliconLife.Common. Verbesserungen an den Kernschnittstellen wirken sich auf beide Versionen aus.

## Verhaltenskodex

Dieses Projekt steht unter der Apache 2.0-Lizenz. Bitte bleiben Sie in allen Interaktionen respektvoll und professionell.

---

## Schnellstart

### 1. Repository forken

Klicken Sie auf den „Fork"-Button auf GitHub, um Ihre eigene Kopie zu erstellen.

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

Wählen Sie das passende Projekt je nach Art Ihres Beitrags:

- **Kernschnittstellen/abstrakte Klassen** → `SiliconLife.Core` bearbeiten
- **Gemeinsame Implementierungen** → `SiliconLife.Common` bearbeiten
- **Default-Version spezifisch** → `SiliconLife.Default` bearbeiten
- **Fast-Version spezifisch** → `SiliconLife.Fast` bearbeiten
- **Speicher-Engine** → `SiliconLife.Speedy` bearbeiten
- **Speicher-Verwaltungswerkzeug** → `SiliconLife.Speedy.Manager` bearbeiten
- **Plugin-Entwicklung** → `SiliconLife.Core/Plugins` bearbeiten
- **Mehrsprachige Dokumentation** → Verzeichnis `docs/` bearbeiten

---

## Entwicklungs-Workflow

### Code-Stil

- C#-Codierungskonventionen befolgen
- Klassennamen in PascalCase
- Methodenparameter in camelCase
- Private Felder in `_camelCase`
- Alle öffentlichen APIs müssen XML-Dokumentation haben

### Commit-Nachrichten

Dem **Conventional Commits**-Format folgen:

```
<type>(<scope>): <description>
```

**Typen**:
- `feat`: Neue Funktion
- `fix`: Bug-Fix
- `docs`: Dokumentationsänderung
- `style`: Code-Formatierung
- `refactor`: Code-Refactoring
- `test`: Teständerung
- `chore`: Build/Werkzeug-Änderung

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

5. **Zum Fork pushen**
   ```bash
   git push origin feature/your-feature-name
   ```

6. **Pull Request erstellen**
   - Zum Original-Repository gehen
   - Auf „Compare & pull request" klicken
   - PR-Vorlage ausfüllen
   - Einreichen

---

## Pull-Request-Leitfaden

### PR-Titel

Dasselbe Format wie bei Commit-Nachrichten verwenden:
```
feat(localization): add Korean language support
```

### PR-Beschreibung

Folgendes einschließen:

1. **Was** - Was macht dieser PR?
2. **Warum** - Warum ist diese Änderung erforderlich?
3. **Wie** - Wie haben Sie es implementiert?
4. **Tests** - Wie wurde getestet?

### Beispiel für eine PR-Beschreibung

```markdown
## Was
Koreanische Lokalisierung für alle UI-Komponenten und Dokumentation hinzugefügt.

## Warum
Die Zugänglichkeit des Projekts für koreanischsprachige Nutzer erweitern.

## Wie
- KoKR.cs Lokalisierungsdatei erstellt
- 500+ Übersetzungsschlüssel hinzugefügt
- Alle Ansichten zur Verwendung der Lokalisierung aktualisiert
- Koreanische Dokumentation in docs/ko-KR/ erstellt

## Tests
- Korrekte Anzeige aller UI-Elemente auf Koreanisch verifiziert
- Sprachwechselfunktion getestet
- Übersetzungen mit Muttersprachlern überprüft
```

---

## Beitragsarten

### 1. Bug-Fix

**Ablauf**:
1. Vorhandene Issues prüfen
2. Falls nicht vorhanden, ein Issue erstellen
3. Bug beheben
4. Testfall hinzufügen
5. PR einreichen

**Anforderungen**:
- Klar beschriebener Bug
- Schritte zur Reproduktion
- Test zur Verhinderung von Regressionen

### 2. Neue Funktion

**Ablauf**:
1. Funktion in Issues/Discussions besprechen
2. Genehmigung der Maintainer einholen
3. Funktion implementieren
4. Umfassende Tests hinzufügen
5. Dokumentation aktualisieren
6. PR einreichen

**Anforderungen**:
- Funktionsvorschlag ist genehmigt
- Vollständige Testabdeckung
- Dokumentation aktualisiert
- Abwärtskompatibel

### 3. Dokumentation

**Ablauf**:
1. Dokumentationslücke identifizieren
2. Dokumentation schreiben/aktualisieren
3. PR einreichen

**Anforderungen**:
- Klar und prägnise
- Beispiele enthalten
- Falls zutreffend, mehrsprachig unterstützen

### 4. Code-Refactoring

**Ablauf**:
1. Refactoring in einem Issue vorschlagen
2. Genehmigung einholen
3. Code refaktorieren
4. Sicherstellen, dass alle Tests bestehen
5. PR einreichen

**Anforderungen**:
- Keine Funktionsänderung
- Alle Tests bestehen
- Code-Qualität verbessert
- Klar erklärt

---

## Testleitfaden

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

Vollständigen Workflow testen:
- KI-Interaktion
- Werkzeugausführung
- Berechtigungsprüfung
- Speicheroperationen

### Manuelle Tests

Bei UI-Änderungen:
- In mehreren Browsern testen
- Responsives Design verifizieren
- Barrierefreiheit prüfen

---

## Dokumentationsleitfaden

### Code-Kommentare

- XML-Kommentare für alle öffentlichen APIs verwenden
- Inline-Kommentare für komplexe Logik verwenden
- Code-Kommentare auf Englisch verfassen

### Dokumentationsdateien

- In `docs/{language}/` ablegen
- Alle Sprachversionen aktualisieren
- Bestehende Struktur befolgen

### Mehrsprachige Dokumentation

Beim Hinzufügen von Dokumentation:
1. Zunächst die englische Version erstellen
2. In andere Sprachen übersetzen
3. Inhalte synchron halten

---

## Review-Prozess

### Worauf Maintainer achten

1. **Code-Qualität**
   - Konventionen eingehalten
   - Klar und lesbar
   - Gut dokumentiert

2. **Tests**
   - Ausreichende Abdeckung
   - Alle Tests bestehen
   - Randfälle abgedeckt

3. **Dokumentation**
   - Aktualisiert
   - Klar erklärt
   - Mehrsprachig

4. **Kompatibilität**
   - Abwärtskompatibel
   - Keine Breaking Changes (außer nach Ankündigung)
   - Semantische Versionierung befolgen

### Review-Zeitrahmen

- Erstes Review: 1–3 Tage
- Feedback-Einarbeitung: nach Bedarf
- Merge: nach Genehmigung

---

## Häufige Fragen

### PR wurde abgelehnt

**Ursachen**:
- Leitfaden nicht befolgt
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

# Mit Force-Push aktualisieren
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
- Maintainer für dringende Anliegen markieren

---

## Danksagung

Beitragende werden an folgenden Stellen gewürdigt:
- README.md im Beitragenden-Bereich
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
