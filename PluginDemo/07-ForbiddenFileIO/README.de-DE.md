# Verbotene Datei-I/O-Operationen — Antimuster

Demonstriert **verbotene** Datei-I/O-Operationen im Plugin-System. Dieses Beispiel dient als Antimuster-Referenz, zeigt was man NICHT tun sollte und bietet für jeden Verstoß die korrekte Alternative.

## Warum ist System.IO global verboten?

Der gesamte `System.IO`-Namespace wird auf Plugin-Ebene blockiert, da direkter Dateizugriff schwerwiegende Sicherheitsrisiken birgt:

1. **Unbefugter Dateizugriff**: Plugins könnten sensible Dateien außerhalb des Arbeitsbereichs lesen (Passwörter, Schlüssel, persönliche Daten)
2. **Datei-Überschreibungsangriffe**: Bösartige Plugins könnten kritische System- oder Konfigurationsdateien überschreiben
3. **Verzeichnisdurchquerung**: Plugins könnten mit `../`-Pfaden die Arbeitsbereichsgrenzen verlassen
4. **Ressourcenerschöpfung**: Unkontrollierte Dateierstellung könnte den Speicherplatz füllen
5. **Keine Prüfspur**: Direkte Dateioperationen umgehen das Plugin-Sicherheitsauditsystem

## Verbotene Typen

Alle `System.IO`-Typen, die direkt auf das Dateisystem zugreifen, werden blockiert:

| Verbotener Typ | Blockierte Methode | Risikostufe |
|----------------|-------------------|-------------|
| `File` | `ReadAllText`, `WriteAllText`, `AppendAllText` etc. | 🔴 Kritisch |
| `FileStream` | Konstruktor mit Dateipfad | 🔴 Kritisch |
| `Directory` | `GetFiles`, `GetDirectories`, `CreateDirectory` | 🔴 Kritisch |
| `StreamReader` | Konstruktor mit Dateipfad (String) | 🔴 Kritisch |
| `StreamWriter` | Konstruktor mit Dateipfad (String) | 🔴 Kritisch |
| `FileInfo` | Alle Methoden | 🔴 Kritisch |
| `DirectoryInfo` | Alle Methoden | 🔴 Kritisch |

## Erlaubte Typen (Whitelist-Ausnahmen)

Typen, die **reine Speicheroperationen** (kein direkter Dateisystemzugriff) ausführen, sind erlaubt:

| Erlaubter Typ | Verwendung | Warum sicher |
|---------------|-----------|--------------|
| `MemoryStream` | In-Memory-Byte-Stream | Kein Dateisystemzugriff |
| `BinaryReader` | Lesen aus vorhandenem Stream | Umhüllt Stream, öffnet keine Dateien |
| `BinaryWriter` | Schreiben in vorhandenen Stream | Umhüllt Stream, erstellt keine Dateien |
| `GZipStream` | Komprimierung/Dekomprimierung | Umhüllt Stream, kein Dateizugriff |
| `StreamReader` | Konstruktor mit `Stream`-Parameter | Sicher beim Umhüllen auditierter Streams |
| `StreamWriter` | Konstruktor mit `Stream`-Parameter | Sicher beim Umhüllen auditierter Streams |

Siehe **04-SafeSystemIO** für Beispiele erlaubter Typen.

## Sicherer Dateizugriff über PermissionedStreamFactory

`PermissionedStreamFactory` ist der **kontrollierte Einstiegspunkt** für Dateioperationen in Plugins:

```csharp
// ✅ Korrekt: Datei lesen
using var readStream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(readStream);
string content = reader.ReadToEnd();

// ✅ Korrekt: Datei schreiben
using var writeStream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(writeStream);
writer.Write("Logdaten");
```

**PermissionedStreamFactory bietet:**
1. **Pfadvalidierung**: Verhindert Verzeichnisdurchquerungsangriffe (`../`)
2. **Berechtigungsprüfung**: Stellt sicher, dass sich die Datei im erlaubten Arbeitsbereich befindet
3. **Prüfprotokoll**: Alle Dateizugriffe werden zur Sicherheitsüberprüfung aufgezeichnet
4. **Ressourcenbereinigung**: Verfolgt offene Streams und verhindert Lecks

## Verstöße in diesem Beispiel

### Verstoß 1: File.ReadAllText

```csharp
// ❌ Verboten — ⚠️ VIOLATION: [TypeRef] System.IO.File::ReadAllText
string content = File.ReadAllText("config.json");

// ✅ Korrekte Alternative
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
string content = reader.ReadToEnd();
```

### Verstoß 2: File.WriteAllText

```csharp
// ❌ Verboten — ⚠️ VIOLATION: [TypeRef] System.IO.File::WriteAllText
File.WriteAllText("output.log", "some data");

// ✅ Korrekte Alternative
using var stream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(stream);
writer.Write("some data");
```

### Verstoß 3: Direkter FileStream

```csharp
// ❌ Verboten — ⚠️ VIOLATION: [TypeRef] System.IO.FileStream::.ctor
using var fs = new FileStream("data.bin", FileMode.Open);

// ✅ Korrekte Alternative
using var fs = PermissionedStreamFactory.OpenRead("data.bin");
```

### Verstoß 4: Directory.GetFiles

```csharp
// ❌ Verboten — ⚠️ VIOLATION: [TypeRef] System.IO.Directory::GetFiles
string[] files = Directory.GetFiles("./logs", "*.txt");

// ✅ Korrekte Alternative (mit SpeedyPack)
using var pack = SpeedyPack.Open("logs.spk");
var entries = pack.ListEntries("/");
```

### Verstoß 5: StreamReader mit direktem Pfad

```csharp
// ❌ Verboten — ⚠️ VIOLATION: [TypeRef] System.IO.StreamReader::.ctor(string)
using var reader = new StreamReader("config.json");

// ✅ Korrekte Alternative
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
```

## Vergleich mit anderen Beispielen

| Beispiel | Schwerpunkt | Erforderliche Berechtigung |
|----------|-------------|---------------------------|
| **04-SafeSystemIO** | Erlaubte Speichertypen (MemoryStream, GZipStream) | Keine |
| **07-ForbiddenFileIO** | Verbotene Dateizugriffsmuster (dieses Beispiel) | Nicht zutreffend (blockiert) |
| **14-CapabilityFileIO** | FileIO-Fähigkeit deklarieren zur Umgehung der Einschränkungen | `Capability.FileIO` |

## PluginLoader-Sicherheitsscan-Mechanismus

Wenn PluginLoader dieses Plugin scannt:

1. **TypeRef-Scan**: Erkennt Referenzen auf verbotene `System.IO`-Typen
2. **MemberRef-Scan**: Erkennt Aufrufe blockierter Methoden
3. **IL-String-Scan**: Erkennt stringbasierte Reflektions-Umgehungsversuche
4. **Ablehnung**: Plugin wird beim Laden mit detaillierter Fehlermeldung abgelehnt

Umgehung durch Stringverkettung, Reflektion, dynamisches Laden oder Verschleierung ist nicht möglich — diese werden durch IL-Level-Scanning erfasst (siehe **12-ForbiddenStringBypass**).

## Sicherheitshinweis

Wenn Sie wirklich uneingeschränkten Dateizugriff benötigen, können Sie `Capability.FileIO` deklarieren (siehe 14-CapabilityFileIO). Best Practices sind jedoch:
- **SpeedyPack** für strukturierte Datenspeicherung bevorzugen (keine Berechtigungsdeklaration nötig)
- **PermissionedStreamFactory** verwenden, wenn Dateizugriff erforderlich ist (kontrollierter Einstiegspunkt)
- `Capability.FileIO` nur deklarieren, wenn obige Lösungen nicht ausreichen
