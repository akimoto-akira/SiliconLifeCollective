# PluginDemo-14: Capability.FileIO — Deklaratywne uprawnienie we/wy plików

## Przegląd

Ten plugin demonstruje użycie `[PluginCapability(Capability.FileIO)]` do deklaracji bezpośredniego dostępu do systemu plików. Dzięki tej deklaracji plugin uzyskuje dostęp do wszystkich typów `System.IO` poza wbudowaną białą listą `SystemIOAllowedTypes`.

## Składnia deklaracji PluginCapability

```csharp
[PluginCapability(Capability.FileIO, Reason = "Direct log file access for audit trail")]
public class CapabilityFileIOPlugin : IPlugin { ... }
```

## Jak działa Capability.FileIO

1. **Stan domyślny**: Przestrzeń nazw `System.IO` jest całkowicie zabroniona; dozwolone są tylko typy z białej listy `SystemIOAllowedTypes` (MemoryStream, BinaryReader, GZipStream itd.)
2. **Z deklaracją**: Zakaz całej przestrzeni nazw `System.IO` jest zniesiony — File, FileStream, Directory, StreamReader(string) itd. stają się dostępne
3. **Zwolnienie ILString**: Stałe łańcuchowe zaczynające się od `"System.IO."` nie są oznaczane
4. **Niedeclarowalne ograniczenia**: P/Invoke, Unsafe, Reflection.Emit itd. pozostają zablokowane

## Zakres zwolnienia Capability.FileIO

### Zwolnienia TypeRef

Wszystkie typy `System.IO` są zwolnione:

| Kategoria | Zwolnione typy |
|-----------|---------------|
| Operacje na plikach | `File`, `FileInfo` |
| Operacje na katalogach | `Directory`, `DirectoryInfo` |
| Typy strumieni | `FileStream`, `StreamReader(path)`, `StreamWriter(path)` |
| System plików | `FileSystemWatcher`, `DriveInfo`, `Path` |

### Zwolnienie ILString

- Łańcuchy zaczynające się od `"System.IO."` nie są oznaczane

### Co pozostaje zabronione

| Kategoria | Nadal zablokowane |
|-----------|------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` |
| Niebezpieczny kod | `UnverifiableCodeAttribute`, `Unsafe` |
| Emisja IL | `System.Reflection.Emit.*` |
| Ładowanie zestawów | `System.Runtime.Loader`, `Assembly.Load*` |
| Rejestr | `Microsoft.Win32.*` |

## Porównanie z innymi przykładami

| Przykład | Deklaracja | Dostęp do plików | Uwagi |
|----------|-----------|-----------------|-------|
| **04-SafeSystemIO** | Brak | MemoryStream, BinaryReader, GZipStream | Używa tylko typów z białej listy |
| **07-ForbiddenFileIO** | Brak | ❌ ODRZUCONY | Przykład antywzorca |
| **14-CapabilityFileIO** | `[PluginCapability(Capability.FileIO)]` | ✅ Pełny dostęp do System.IO | Ten przykład |
| **20-SpeedyPack** | Brak | Przez API SpeedyPack (bez Capability) | Zalecane przechowywanie danych |

## Kolejność priorytetów dostępu do plików

1. **SpeedyPack** — Bez deklaracji możliwości. Wbudowane buforowanie, WAL, transakcje. **Zalecany do przechowywania danych strukturalnych.**
2. **PermissionedStreamFactory** — Bez deklaracji. Audytowany dostęp z walidacją ścieżki i kontrolą dostępu.
3. **Capability.FileIO + bezpośredni System.IO** — Tylko gdy powyższe opcje nie wystarczają.

## Dlaczego warto preferować PermissionedStreamFactory / SpeedyPack?

Nawet z `Capability.FileIO` zaleca się używanie kontrolowanych punktów wejścia, ponieważ:

1. **Ślad audytu**: Każdy dostęp jest rejestrowany i możliwy do śledzenia
2. **Walidacja ścieżki**: Zapobiega atakom traversalu katalogów (`../`)
3. **Kontrola dostępu**: Egzekwowanie granic obszaru roboczego
4. **Śledzenie zasobów**: Zapobiega wyciekom strumieni i wyczerpaniu zasobów
5. **Zgodność**: Kontrolowane wzorce dostępu ułatwiają audyty bezpieczeństwa

## Najlepsze praktyki bezpieczeństwa

1. **Deklarować FileIO tylko gdy naprawdę konieczne**: Czy SpeedyPack lub PermissionedStreamFactory wystarczą?
2. **Podać jasną Reason**: "Direct log file access for audit trail" jest lepsze niż "file access"
3. **Samodzielnie walidować ścieżki**: Nawet z Capability.FileIO walidować wszystkie ścieżki plików przed użyciem
4. **Używać instrukcji using**: Zawsze likwidować FileStream/StreamReader/StreamWriter
5. **Zasada najmniejszych uprawnień**: Deklarować tylko te możliwości, których plugin faktycznie potrzebuje

## Pliki

- `Plugin.cs` — Plugin demonstracyjny deklarujący Capability.FileIO
- `README.md` — Ten plik (Angielski)
- `README.zh-CN.md` — Chiński uproszczony
- Tłumaczenia: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Powiązane przykłady

- **04-SafeSystemIO**: Dozwolone typy System.IO w pamięci (bez deklaracji)
- **07-ForbiddenFileIO**: Antywzorzec zablokowanych operacji na plikach
- **20-SpeedyPack**: Zalecane przechowywanie danych bez deklaracji możliwości
- **18-CapabilityDenied**: Antywzorzec niedeklarowalnych możliwości
