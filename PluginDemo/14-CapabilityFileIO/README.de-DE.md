# PluginDemo-14: Capability.FileIO — Deklarative Datei-E/A-Berechtigung

## Übersicht

Dieses Plugin demonstriert die Verwendung von `[PluginCapability(Capability.FileIO)]` zur Deklaration des direkten Dateisystemzugriffs. Mit dieser Deklaration erhält das Plugin Zugriff auf alle `System.IO`-Typen über die integrierte `SystemIOAllowedTypes`-Whitelist hinaus.

## PluginCapability-Deklarationssyntax

```csharp
[PluginCapability(Capability.FileIO, Reason = "Direct log file access for audit trail")]
public class CapabilityFileIOPlugin : IPlugin { ... }
```

## Funktionsweise von Capability.FileIO

1. **Standardzustand**: Der `System.IO`-Namespace ist vollständig gesperrt; nur `SystemIOAllowedTypes`-Whitelist-Typen sind erlaubt (MemoryStream, BinaryReader, GZipStream usw.)
2. **Mit Deklaration**: Die gesamte `System.IO`-Namespace-Sperre wird aufgehoben — File, FileStream, Directory, StreamReader(string) usw. werden zugänglich
3. **ILString-Befreiung**: Zeichenfolgenkonstanten, die mit `"System.IO."` beginnen, werden nicht markiert
4. **Nicht deklarierbare Einschränkungen**: P/Invoke, Unsafe, Reflection.Emit usw. bleiben blockiert

## Befreiungsbereich von Capability.FileIO

### TypeRef-Befreiung

Alle `System.IO`-Typen werden befreit:

| Kategorie | Befreite Typen |
|-----------|---------------|
| Dateioperationen | `File`, `FileInfo` |
| Verzeichnisoperationen | `Directory`, `DirectoryInfo` |
| Stream-Typen | `FileStream`, `StreamReader(path)`, `StreamWriter(path)` |
| Dateisystem | `FileSystemWatcher`, `DriveInfo`, `Path` |

### ILString-Befreiung

- Zeichenfolgen, die mit `"System.IO."` beginnen, werden nicht markiert

### Weiterhin verbotene Funktionen

| Kategorie | Weiterhin blockiert |
|-----------|-------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` |
| Unsicherer Code | `UnverifiableCodeAttribute`, `Unsafe` |
| IL-Emission | `System.Reflection.Emit.*` |
| Assembly-Laden | `System.Runtime.Loader`, `Assembly.Load*` |
| Registrierung | `Microsoft.Win32.*` |

## Vergleich mit anderen Beispielen

| Beispiel | Deklaration | Dateizugriff | Hinweise |
|----------|------------|-------------|---------|
| **04-SafeSystemIO** | Keine | MemoryStream, BinaryReader, GZipStream | Nur Whitelist-Typen |
| **07-ForbiddenFileIO** | Keine | ❌ ABGELEHNT | Antimuster-Beispiel |
| **14-CapabilityFileIO** | `[PluginCapability(Capability.FileIO)]` | ✅ Voller System.IO-Zugriff | Dieses Beispiel |
| **20-SpeedyPack** | Keine | Über SpeedyPack-API (keine Capability nötig) | Empfohlene Datenspeicherung |

## Prioritätsreihenfolge für Dateizugriff

1. **SpeedyPack** — Keine Capability-Deklaration nötig. Integriertes Caching, WAL, Transaktionen. **Empfohlen für strukturierte Datenspeicherung.**
2. **PermissionedStreamFactory** — Keine Capability-Deklaration nötig. Überwachter Zugriff mit Pfadvalidierung und Zugriffskontrolle.
3. **Capability.FileIO + direktes System.IO** — Nur wenn die obigen Optionen nicht ausreichen.

## Warum PermissionedStreamFactory / SpeedyPack bevorzugen?

Auch mit `Capability.FileIO` wird die Verwendung kontrollierter Einstiegspunkte empfohlen, weil:

1. **Audit-Trail**: Jeder Zugriff wird protokolliert und ist nachverfolgbar
2. **Pfadvalidierung**: Verhindert Directory-Traversal-Angriffe (`../`)
3. **Zugriffskontrolle**: Workspace-Grenzen werden durchgesetzt
4. **Ressourcenüberwachung**: Verhindert Stream-Lecks und Ressourcenerschöpfung
5. **Compliance**: Kontrollierte Zugriffsmuster erleichtern Sicherheitsüberprüfungen

## Sicherheitsbest Practices

1. **FileIO nur wenn wirklich nötig deklarieren**: Kann SpeedyPack oder PermissionedStreamFactory verwendet werden?
2. **Klare Reason angeben**: "Direct log file access for audit trail" ist besser als "file access"
3. **Pfade selbst validieren**: Auch mit Capability.FileIO alle Dateipfade vor der Verwendung validieren
4. **Using-Anweisungen verwenden**: FileStream/StreamReader/StreamWriter immer ordnungsgemäß entsorgen
5. **Prinzip der geringsten Rechte**: Nur die Fähigkeiten deklarieren, die das Plugin tatsächlich benötigt

## Dateien

- `Plugin.cs` — Demo-Plugin mit Capability.FileIO-Deklaration
- `README.md` — Diese Datei (Englisch)
- `README.zh-CN.md` — Vereinfachtes Chinesisch
- Übersetzungen: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Verwandte Beispiele

- **04-SafeSystemIO**: Erlaubte In-Memory-System.IO-Typen (keine Deklaration nötig)
- **07-ForbiddenFileIO**: Antimuster für blockierte Dateioperationen
- **20-SpeedyPack**: Empfohlener Datenspeicher ohne Capability-Deklaration
- **18-CapabilityDenied**: Antimuster für nicht deklarierbare Fähigkeiten
