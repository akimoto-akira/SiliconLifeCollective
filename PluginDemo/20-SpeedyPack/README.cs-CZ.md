# PluginDemo-20: SpeedyPack — Strukturované datové úložiště

## Přehled

Tento plugin demonstruje použití `SpeedyPack` pro strukturované ukládání dat **bez jakékoliv deklarace schopnosti**. SpeedyPack je **doporučený** způsob pro perzistenci dat pluginů.

## Proč SpeedyPack?

| Funkce | SpeedyPack | PermissionedStreamFactory | Capability.FileIO + System.IO |
|--------|-----------|--------------------------|------------------------------|
| Vyžadovaná schopnost | **Žádná** | Žádná | `Capability.FileIO` |
| Mezipaměť | ✅ Vestavěná | ❌ | ❌ |
| WAL (obnova po havárii) | ✅ | ❌ | ❌ |
| Transakce | ✅ `IPackTransaction` | ❌ | ❌ |
| Bezpečné pro vlákna | ✅ | ❌ | ❌ |
| Strukturovaná serializace | ✅ `Read<T>` | ❌ Surové bajty | ❌ Ruční |
| Auditní stopa | ✅ Automatická | ✅ Automatická | ❌ Ruční |

## Základní CRUD

```csharp
// Otevřít datový soubor SpeedyPack
using var pack = SpeedyPack.Open("mydata.spk");

// Zapsat páry klíč-hodnota
pack.Write("user:name", "Alice");
pack.Write("user:age", 30);

// Číst hodnoty (typované)
string name = pack.Read<string>("user:name");  // "Alice"
int age = pack.Read<int>("user:age");           // 30

// Smazat klíč
pack.Delete("user:age");

// Zkontrolovat existenci
bool exists = pack.Contains("user:name");  // true
```

## Typovaný přístup se strukturovanými objekty

```csharp
public class UserProfile
{
    public string Name { get; set; }
    public int Level { get; set; }
    public string[] Tags { get; set; }
}

// Zapsat strukturovaný objekt
var profile = new UserProfile { Name = "Bob", Level = 42, Tags = new[] { "admin" } };
pack.Write("profile:bob", profile);

// Číst typovaný objekt
var loaded = pack.Read<UserProfile>("profile:bob");
Console.WriteLine($"{loaded.Name}, Level {loaded.Level}");
```

## Transakce

```csharp
using (var tx = pack.BeginTransaction())
{
    try
    {
        tx.Write("account:a", 1000);
        tx.Write("account:b", 500);
        tx.Commit();   // Atomicky — oba zápisy se uloží nebo žádný
    }
    catch
    {
        tx.Rollback();  // Zahodit všechny zápisy v této transakci
    }
}
```

### Metody IPackTransaction

| Metoda | Popis |
|--------|-------|
| `Write(key, value)` | Zařadit operaci zápisu do fronty |
| `Delete(key)` | Zařadit operaci smazání do fronty |
| `Commit()` | Atomicky aplikovat všechny operace ve frontě |
| `Rollback()` | Zahodit všechny operace ve frontě |

## Konfigurace s SpeedyPackOptions

```csharp
var options = new SpeedyPackOptions
{
    MaxCacheSize = 1024 * 1024,              // 1 MB mezipaměti
    AutoFlushInterval = TimeSpan.FromSeconds(30),
    CompressionLevel = CompressionLevel.Optimal
};
using var pack = SpeedyPack.Open("data.spk", options);
```

### Vlastnosti SpeedyPackOptions

| Vlastnost | Typ | Výchozí | Popis |
|----------|-----|--------|-------|
| `MaxCacheSize` | `long` | 64 MB | Maximální velikost mezipaměti v paměti |
| `AutoFlushInterval` | `TimeSpan` | 10 sekund | Interval vyprazdňování mezipaměti na disk |
| `CompressionLevel` | `CompressionLevel` | `Fastest` | Úroveň komprese uložených dat |

## Bezpečnostní poznámka

SpeedyPack **nevyžaduje** žádnou deklaraci schopnosti. Je to bezpečný, kontrolovaný vstupní bod pro ukládání dat, který:
- Validuje všechny cesty proti hranicím pracovního prostoru
- Poskytuje úplnou auditní stopu všech operací čtení/zápisu
- Zabraňuje útokům directory traversal
- Automaticky spravuje životní cyklus zdrojů

## Soubory

- `Plugin.cs` — Demo plugin SpeedyPack
- `README.md` — Tento soubor (Angličtina)
- `README.zh-CN.md` — Zjednodušená čínština
- Překlady: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Související příklady

- **04-SafeSystemIO**: Povolené typy System.IO v paměti (bez deklarace)
- **07-ForbiddenFileIO**: Antivzor blokovaných souborových operací
- **14-CapabilityFileIO**: Když SpeedyPack nestačí
