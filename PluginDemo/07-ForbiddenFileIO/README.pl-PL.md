# Zabronione operacje I/O plików — Antywzorzec

Demonstruje **zabronione** operacje I/O plików w systemie wtyczek. Ten przykład służy jako odniesienie antywzorca, pokazując czego NIE robić i dostarczając prawidłowe alternatywy dla każdego naruszenia.

## Dlaczego System.IO jest globalnie zakazany?

Cała przestrzeń nazw `System.IO` jest zablokowana na poziomie wtyczek, ponieważ bezpośredni dostęp do plików stwarza poważne zagrożenia bezpieczeństwa:

1. **Nieautoryzowany dostęp do plików**: Wtyczki mogą odczytywać wrażliwe pliki spoza przestrzeni roboczej (hasła, klucze, dane osobowe)
2. **Ataki nadpisywania plików**: Złośliwe wtyczki mogą nadpisywać krytyczne pliki systemowe lub konfiguracyjne
3. **Przechodzenie katalogów**: Wtyczki mogą używać ścieżek `../` do wyjścia poza granice przestrzeni roboczej
4. **Wyczerpanie zasobów**: Niekontrolowane tworzenie plików może zapełnić przestrzeń dyskową
5. **Brak śladu audytu**: Bezpośrednie operacje na plikach omijają system audytu bezpieczeństwa wtyczek

## Typy zabronione

Wszystkie typy `System.IO` bezpośrednio uzyskujące dostęp do systemu plików są blokowane:

| Typ zabroniony | Zablokowana metoda | Poziom ryzyka |
|---------------|-------------------|---------------|
| `File` | `ReadAllText`, `WriteAllText`, `AppendAllText` itp. | 🔴 Krytyczny |
| `FileStream` | Konstruktor ze ścieżką pliku | 🔴 Krytyczny |
| `Directory` | `GetFiles`, `GetDirectories`, `CreateDirectory` | 🔴 Krytyczny |
| `StreamReader` | Konstruktor ze ścieżką (string) | 🔴 Krytyczny |
| `StreamWriter` | Konstruktor ze ścieżką (string) | 🔴 Krytyczny |
| `FileInfo` | Wszystkie metody | 🔴 Krytyczny |
| `DirectoryInfo` | Wszystkie metody | 🔴 Krytyczny |

## Typy dozwolone (wyjątki białej listy)

Typy wykonujące **czysto pamięciowe operacje** (bez bezpośredniego dostępu do systemu plików) są dozwolone:

| Typ dozwolony | Zastosowanie | Dlaczego bezpieczny |
|--------------|-------------|---------------------|
| `MemoryStream` | Strumień bajtów w pamięci | Brak dostępu do systemu plików |
| `BinaryReader` | Odczyt z istniejącego strumienia | Opakowuje strumień, nie otwiera plików |
| `BinaryWriter` | Zapis do istniejącego strumienia | Opakowuje strumień, nie tworzy plików |
| `GZipStream` | Kompresja/dekompresja | Opakowuje strumień, brak dostępu do plików |
| `StreamReader` | Konstruktor z parametrem `Stream` | Bezpieczny przy opakowywaniu audytowanych strumieni |
| `StreamWriter` | Konstruktor z parametrem `Stream` | Bezpieczny przy opakowywaniu audytowanych strumieni |

Zobacz przykład **04-SafeSystemIO** dla typów dozwolonych.

## Bezpieczny dostęp do plików przez PermissionedStreamFactory

`PermissionedStreamFactory` jest **kontrolowanym punktem wejścia** dla operacji na plikach we wtyczkach:

```csharp
// ✅ Poprawne: odczyt pliku
using var readStream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(readStream);
string content = reader.ReadToEnd();

// ✅ Poprawne: zapis pliku
using var writeStream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(writeStream);
writer.Write("Dane dziennika");
```

**PermissionedStreamFactory zapewnia:**
1. **Walidacja ścieżki**: Zapobiega atakom przechodzenia katalogów (`../`)
2. **Sprawdzanie uprawnień**: Zapewnia, że plik jest w dozwolonej przestrzeni roboczej
3. **Dziennik audytu**: Wszystkie dostępy do plików są rejestrowane do przeglądu bezpieczeństwa
4. **Czyszczenie zasobów**: Śledzi otwarte strumienie i zapobiega wyciekom

## Naruszenia w tym przykładzie

### Naruszenie 1: File.ReadAllText

```csharp
// ❌ Zabronione — ⚠️ VIOLATION: [TypeRef] System.IO.File::ReadAllText
string content = File.ReadAllText("config.json");

// ✅ Prawidłowa alternatywa
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
string content = reader.ReadToEnd();
```

### Naruszenie 2: File.WriteAllText

```csharp
// ❌ Zabronione — ⚠️ VIOLATION: [TypeRef] System.IO.File::WriteAllText
File.WriteAllText("output.log", "some data");

// ✅ Prawidłowa alternatywa
using var stream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(stream);
writer.Write("some data");
```

### Naruszenie 3: Bezpośredni FileStream

```csharp
// ❌ Zabronione — ⚠️ VIOLATION: [TypeRef] System.IO.FileStream::.ctor
using var fs = new FileStream("data.bin", FileMode.Open);

// ✅ Prawidłowa alternatywa
using var fs = PermissionedStreamFactory.OpenRead("data.bin");
```

### Naruszenie 4: Directory.GetFiles

```csharp
// ❌ Zabronione — ⚠️ VIOLATION: [TypeRef] System.IO.Directory::GetFiles
string[] files = Directory.GetFiles("./logs", "*.txt");

// ✅ Prawidłowa alternatywa (używając SpeedyPack)
using var pack = SpeedyPack.Open("logs.spk");
var entries = pack.ListEntries("/");
```

### Naruszenie 5: StreamReader z bezpośrednią ścieżką

```csharp
// ❌ Zabronione — ⚠️ VIOLATION: [TypeRef] System.IO.StreamReader::.ctor(string)
using var reader = new StreamReader("config.json");

// ✅ Prawidłowa alternatywa
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
```

## Porównanie z innymi przykładami

| Przykład | Temat | Wymagane uprawnienie |
|----------|-------|---------------------|
| **04-SafeSystemIO** | Dozwolone typy pamięciowe (MemoryStream, GZipStream) | Brak |
| **07-ForbiddenFileIO** | Zabronione wzorce dostępu do plików (ten przykład) | Nie dotyczy (zablokowane) |
| **14-CapabilityFileIO** | Deklaracja zdolności FileIO w celu obejścia ograniczeń | `Capability.FileIO` |

## Mechanizm skanowania bezpieczeństwa PluginLoader

Gdy PluginLoader skanuje tę wtyczkę:

1. **Skanowanie TypeRef**: Wykrywa odniesienia do zabronionych typów `System.IO`
2. **Skanowanie MemberRef**: Wykrywa wywołania zablokowanych metod
3. **Skanowanie ciągów IL**: Wykrywa próby obejścia przez refleksję opartą na ciągach
4. **Odrzucenie**: Wtyczka jest odrzucana podczas ładowania ze szczegółowym komunikatem błędu

Obejście przez konkatenację ciągów, refleksję, dynamiczne ładowanie lub zaciemnianie jest niemożliwe — te są przechwytywane przez skanowanie na poziomie IL (zobacz **12-ForbiddenStringBypass**).

## Uwaga dotycząca bezpieczeństwa

Jeśli naprawdę potrzebujesz nieograniczonego dostępu do plików, możesz zadeklarować `Capability.FileIO` (zobacz 14-CapabilityFileIO). Jednak najlepsze praktyki to:
- Preferować **SpeedyPack** do przechowywania danych strukturalnych (bez deklaracji uprawnień)
- Używać **PermissionedStreamFactory** gdy dostęp do plików jest konieczny (kontrolowany punkt wejścia)
- Deklarować `Capability.FileIO` tylko jeśli powyższe rozwiązania nie wystarczą
