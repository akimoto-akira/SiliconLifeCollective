# PluginDemo-20: SpeedyPack — Strukturierte Datenspeicherung

## Übersicht

Dieses Plugin demonstriert die Verwendung von `SpeedyPack` für strukturierte Datenspeicherung **ohne jegliche Capability-Deklaration**. SpeedyPack ist die **empfohlene** Methode für die Datenpersistenz von Plugins.

## Warum SpeedyPack?

| Funktion | SpeedyPack | PermissionedStreamFactory | Capability.FileIO + System.IO |
|---------|-----------|--------------------------|------------------------------|
| Capability benötigt | **Keine** | Keine | `Capability.FileIO` |
| Caching | ✅ Integriert | ❌ | ❌ |
| WAL (Absturzwiederherstellung) | ✅ | ❌ | ❌ |
| Transaktionen | ✅ `IPackTransaction` | ❌ | ❌ |
| Thread-sicher | ✅ | ❌ | ❌ |
| Strukturierte Serialisierung | ✅ `Read<T>` | ❌ Rohe Bytes | ❌ Manuell |
| Audit-Trail | ✅ Automatisch | ✅ Automatisch | ❌ Manuell |

## Basis-CRUD

```csharp
// SpeedyPack-Datendatei öffnen
using var pack = SpeedyPack.Open("mydata.spk");

// Schlüssel-Wert-Paare schreiben
pack.Write("user:name", "Alice");
pack.Write("user:age", 30);

// Werte lesen (typisiert)
string name = pack.Read<string>("user:name");  // "Alice"
int age = pack.Read<int>("user:age");           // 30

// Schlüssel löschen
pack.Delete("user:age");

// Existenz prüfen
bool exists = pack.Contains("user:name");  // true
```

## Typisierter Zugriff mit strukturierten Objekten

```csharp
public class UserProfile
{
    public string Name { get; set; }
    public int Level { get; set; }
    public string[] Tags { get; set; }
}

// Strukturiertes Objekt schreiben
var profile = new UserProfile { Name = "Bob", Level = 42, Tags = new[] { "admin" } };
pack.Write("profile:bob", profile);

// Typisiertes Objekt lesen
var loaded = pack.Read<UserProfile>("profile:bob");
Console.WriteLine($"{loaded.Name}, Level {loaded.Level}");
```

## Transaktionen

```csharp
using (var tx = pack.BeginTransaction())
{
    try
    {
        tx.Write("account:a", 1000);
        tx.Write("account:b", 500);
        tx.Commit();   // Atomar — beide Schreibvorgänge werden persistiert oder keiner
    }
    catch
    {
        tx.Rollback();  // Alle Schreibvorgänge in dieser Transaktion verwerfen
    }
}
```

### IPackTransaction-Methoden

| Methode | Beschreibung |
|---------|-------------|
| `Write(key, value)` | Schreibvorgang in die Warteschlange einreihen |
| `Delete(key)` | Löschvorgang in die Warteschlange einreihen |
| `Commit()` | Alle eingereihten Vorgänge atomar anwenden |
| `Rollback()` | Alle eingereihten Vorgänge verwerfen |

## Konfiguration mit SpeedyPackOptions

```csharp
var options = new SpeedyPackOptions
{
    MaxCacheSize = 1024 * 1024,              // 1 MB Cache
    AutoFlushInterval = TimeSpan.FromSeconds(30),
    CompressionLevel = CompressionLevel.Optimal
};
using var pack = SpeedyPack.Open("data.spk", options);
```

### SpeedyPackOptions-Eigenschaften

| Eigenschaft | Typ | Standard | Beschreibung |
|------------|-----|----------|-------------|
| `MaxCacheSize` | `long` | 64 MB | Maximale In-Memory-Cache-Größe |
| `AutoFlushInterval` | `TimeSpan` | 10 Sekunden | Intervall zum Leeren des Caches auf die Festplatte |
| `CompressionLevel` | `CompressionLevel` | `Fastest` | Kompressionsstufe für gespeicherte Daten |

## Sicherheitshinweis

SpeedyPack erfordert **keine** Capability-Deklaration. Es ist ein sicherer, kontrollierter Einstiegspunkt für Datenspeicherung:
- Validiert alle Pfade gegen Arbeitsbereichsgrenzen
- Bietet vollständigen Audit-Trail aller Lese-/Schreibvorgänge
- Verhindert Directory-Traversal-Angriffe
- Verwaltet den Ressourcenlebenszyklus automatisch

## Dateien

- `Plugin.cs` — SpeedyPack-Demo-Plugin
- `README.md` — Diese Datei (Englisch)
- `README.zh-CN.md` — Vereinfachtes Chinesisch
- Übersetzungen: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Verwandte Beispiele

- **04-SafeSystemIO**: Erlaubte In-Memory-System.IO-Typen (keine Deklaration nötig)
- **07-ForbiddenFileIO**: Antimuster für blockierte Dateioperationen
- **14-CapabilityFileIO**: Wenn SpeedyPack nicht ausreicht
