# PluginDemo-20: SpeedyPack — Strukturyzowany magazyn danych

## Przegląd

Ten plugin demonstruje użycie `SpeedyPack` do strukturyzowanego przechowywania danych **bez jakiejkolwiek deklaracji możliwości**. SpeedyPack jest **zalecaną** metodą dla persystencji danych wtyczek.

## Dlaczego SpeedyPack?

| Funkcja | SpeedyPack | PermissionedStreamFactory | Capability.FileIO + System.IO |
|---------|-----------|--------------------------|------------------------------|
| Wymagana możliwość | **Brak** | Brak | `Capability.FileIO` |
| Buforowanie | ✅ Wbudowane | ❌ | ❌ |
| WAL (odtwarzanie po awarii) | ✅ | ❌ | ❌ |
| Transakcje | ✅ `IPackTransaction` | ❌ | ❌ |
| Bezpieczny wątkowo | ✅ | ❌ | ❌ |
| Strukturyzowana serializacja | ✅ `Read<T>` | ❌ Surowe bajty | ❌ Ręcznie |
| Ścieżka audytu | ✅ Automatyczna | ✅ Automatyczna | ❌ Ręcznie |

## Podstawowy CRUD

```csharp
// Otworzyć plik danych SpeedyPack
using var pack = SpeedyPack.Open("mydata.spk");

// Zapisać pary klucz-wartość
pack.Write("user:name", "Alice");
pack.Write("user:age", 30);

// Odczytać wartości (typowane)
string name = pack.Read<string>("user:name");  // "Alice"
int age = pack.Read<int>("user:age");           // 30

// Usunąć klucz
pack.Delete("user:age");

// Sprawdzić istnienie
bool exists = pack.Contains("user:name");  // true
```

## Typowany dostęp z obiektami strukturalnymi

```csharp
public class UserProfile
{
    public string Name { get; set; }
    public int Level { get; set; }
    public string[] Tags { get; set; }
}

// Zapisać obiekt strukturalny
var profile = new UserProfile { Name = "Bob", Level = 42, Tags = new[] { "admin" } };
pack.Write("profile:bob", profile);

// Odczytać typowany obiekt
var loaded = pack.Read<UserProfile>("profile:bob");
Console.WriteLine($"{loaded.Name}, Level {loaded.Level}");
```

## Transakcje

```csharp
using (var tx = pack.BeginTransaction())
{
    try
    {
        tx.Write("account:a", 1000);
        tx.Write("account:b", 500);
        tx.Commit();   // Atomowo — oba zapisy są utrwalane lub żaden
    }
    catch
    {
        tx.Rollback();  // Odrzucić wszystkie zapisy w tej transakcji
    }
}
```

### Metody IPackTransaction

| Metoda | Opis |
|--------|------|
| `Write(key, value)` | Kolejkować operację zapisu |
| `Delete(key)` | Kolejkować operację usunięcia |
| `Commit()` | Atomowo zastosować wszystkie operacje w kolejce |
| `Rollback()` | Odrzucić wszystkie operacje w kolejce |

## Konfiguracja z SpeedyPackOptions

```csharp
var options = new SpeedyPackOptions
{
    MaxCacheSize = 1024 * 1024,              // 1 MB bufora
    AutoFlushInterval = TimeSpan.FromSeconds(30),
    CompressionLevel = CompressionLevel.Optimal
};
using var pack = SpeedyPack.Open("data.spk", options);
```

### Właściwości SpeedyPackOptions

| Właściwość | Typ | Domyślnie | Opis |
|------------|-----|----------|------|
| `MaxCacheSize` | `long` | 64 MB | Maksymalny rozmiar bufora w pamięci |
| `AutoFlushInterval` | `TimeSpan` | 10 sekund | Interwał opróżniania bufora na dysk |
| `CompressionLevel` | `CompressionLevel` | `Fastest` | Poziom kompresji przechowywanych danych |

## Uwaga dotycząca bezpieczeństwa

SpeedyPack **nie wymaga** deklaracji możliwości. Jest bezpiecznym, kontrolowanym punktem wejścia do przechowywania danych, który:
- Waliduje wszystkie ścieżki względem granic obszaru roboczego
- Zapewnia pełną ścieżkę audytu wszystkich operacji odczytu/zapisu
- Zapobiega atakom traversal katalogów
- Automatycznie zarządza cyklem życia zasobów

## Pliki

- `Plugin.cs` — Plugin demonstracyjny SpeedyPack
- `README.md` — Ten plik (Angielski)
- `README.zh-CN.md` — Chiński uproszczony
- Tłumaczenia: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Powiązane przykłady

- **04-SafeSystemIO**: Dozwolone typy System.IO w pamięci (bez deklaracji)
- **07-ForbiddenFileIO**: Antywzorzec zablokowanych operacji na plikach
- **14-CapabilityFileIO**: Gdy SpeedyPack jest niewystarczający
