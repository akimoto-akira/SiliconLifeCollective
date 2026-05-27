# Bezpieczne System.IO — Demo

Demonstruje typy System.IO z białej listy `SystemIOAllowedTypes`: `MemoryStream`, `BinaryReader`/`BinaryWriter`, `GZipStream`. Wyjaśnia, dlaczego `FileStream` wymaga `PermissionedStreamFactory`.

## Biała lista SystemIOAllowedTypes

Środowisko uruchomieniowe pluginów domyślnie blokuje przestrzeń nazw `System.IO`, ale zwalnia typy, które **nie wykonują bezpośredniego we/wy plików**:

| Kategoria | Dozwolone typy | Dlaczego bezpieczne |
|-----------|---------------|-------------------|
| Abstrakcje strumieni | `Stream` | Abstrakcyjna klasa bazowa, brak własnego I/O |
| Strumienie w pamięci | `MemoryStream` | Czysta operacja w pamięci |
| Strumienie kompresji | `GZipStream`, `DeflateStream`, `ZLibStream` | Owijają inny strumień, nie otwierają plików |
| Wrappery binarne | `BinaryReader`, `BinaryWriter` | Owijają dowolny strumień, nie otwierają plików |
| Wyliczenia | `SeekOrigin`, `FileMode`, `FileAccess`, `FileShare`, `CompressionMode`, `CompressionLevel` | Tylko typy wartościowe |
| Wyjątki | `IOException`, `InvalidDataException`, `EndOfStreamException` | Tylko typy błędów |

### Typy poza białą listą

Te typy **bezpośrednio uzyskują dostęp do systemu plików** i są **blokowane** w kodzie pluginów:

| Zablokowany typ | Powód | Bezpieczna alternatywa |
|----------------|-------|----------------------|
| `FileStream` | Bezpośrednio otwiera pliki | `PermissionedStreamFactory.CreateReadStream()` / `CreateWriteStream()` |
| `File` | Statyczne operacje plikowe | `PermissionedStreamFactory` + `SafePath` |
| `Directory` | Statyczne operacje katalogowe | `SafePath` (sprawdzanie uprawnień) |
| `FileInfo` | Owiń ścieżki plików | `SafePath` |
| `DirectoryInfo` | Owiń ścieżki katalogów | `SafePath` |
| `StreamReader` | Bezpośrednio otwiera pliki | `PermissionedStreamFactory` + owiń `PermissionedStream` |
| `StreamWriter` | Bezpośrednio otwiera pliki | `PermissionedStreamFactory` + owiń `PermissionedStream` |

## Dlaczego FileStream wymaga PermissionedStreamFactory

`FileStream` bezpośrednio otwiera pliki na dysku — poważne zagrożenie bezpieczeństwa w systemie pluginów. `PermissionedStreamFactory` wymusza:

1. **Sprawdzanie uprawnień** — `PermissionManager` wywołującego musi nadać `FileAccess` dla ścieżki
2. **Dziennik audytu** — każde otwarcie pliku jest rejestrowane z ID being wywołującego
3. **Walidacja ścieżki** — puste/nieprawidłowe ścieżki są odrzucane przed jakimkolwiek I/O

```
❌ new FileStream("path", FileMode.Open)           → Zablokowane przez skaner TypeRef
✅ PermissionedStreamFactory.CreateReadStream(id, "path")  → Sprawdzanie uprawnień zaliczone
✅ PermissionedStreamFactory.CreateWriteStream(id, "path") → Sprawdzanie uprawnień zaliczone
```

## Potok demonstracyjny

To demo buduje kompletny potok danych w pamięci używając tylko typów z białej listy:

```
┌─────────────────────────────────────────────────────────────────┐
│  Demo 1: MemoryStream                                           │
│  └─ Zapisz bajty → Odczytaj bajty → Dekoduj ciąg                │
│                                                                  │
│  Demo 2: Potok kompresji                                        │
│  └─ string → UTF8 → MemoryStream                                │
│     → GZipStream(kompresuj) → MemoryStream(skompresowane)        │
│     → GZipStream(dekompresuj) → MemoryStream(surowe)             │
│     → UTF8 → string (weryfikacja roundtrip)                      │
│                                                                  │
│  Demo 3: BinaryReader/Writer                                     │
│  └─ Write(int, double, string) → MemoryStream                   │
│     → Read(int, double, string) → Weryfikacja roundtrip         │
└─────────────────────────────────────────────────────────────────┘
```

## To demo

> **⚠️ Uwaga:** To demo używa **tylko** typów z białej listy `SystemIOAllowedTypes`. Nie jest wykonywane żadne we/wy plików. Dostępu do plików szukaj w API `PermissionedStreamFactory`.

| Klasa | Rola |
|-------|------|
| `SafeSystemIOPlugin` | Implementacja `IPlugin` — demonstruje bezpieczne użycie System.IO |

## Uwaga dotycząca bezpieczeństwa

Przestrzeń nazw `System.IO` jest blokowana przez skaner TypeRef pluginów. Przechodzą tylko typy z białej listy. Do rzeczywistego dostępu do plików należy używać `PermissionedStreamFactory`, który wykonuje sprawdzanie uprawnień i prowadzi dziennik audytu. Zobacz [dokumentację bezpieczeństwa](../../docs/pl-PL/security.md).
