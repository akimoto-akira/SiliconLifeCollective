# Demo zaufanej zależności

Demonstruje użycie `Newtonsoft.Json` — biblioteki intensywnie korzystającej z refleksji wewnętrznie — jako zaufanego zestawu. Skaner bezpieczeństwa PluginLoader całkowicie pomija zaufane zestawy, pozwalając wtyczkom odwoływać się do nich bez wyzwalania naruszeń.

## Mechanizm białej listy TrustedAssemblies

`PluginLoader` utrzymuje statyczną białą listę bibliotek open-source, które są **zaufane domyślnie**:

```csharp
private static readonly HashSet<string> TrustedAssemblies = new(StringComparer.Ordinal)
{
    // Serializacja
    "Google.Protobuf",
    "protobuf-net",
    "Newtonsoft.Json",        // ← To demo używa tej biblioteki
    "MessagePack",
    "YamlDotNet",

    // Logowanie
    "Serilog", "NLog",

    // Microsoft.Extensions.*
    "Microsoft.Extensions.Logging.Abstractions",
    "Microsoft.Extensions.DependencyInjection.Abstractions",
    // ...

    // Dostęp do danych / mapowanie
    "Dapper", "AutoMapper",

    // Walidacja i dystrybucja wiadomości
    "FluentValidation", "MediatR",
};
```

### Kryteria przyjęcia

Biblioteka może zostać dodana do `TrustedAssemblies`, jeśli spełnia **wszystkie trzy** kryteria:

| # | Kryterium | Uzasadnienie |
|---|-----------|-------------|
| 1 | Szeroko używany projekt open-source (MIT / Apache 2.0 / BSD) | Publicznie audytowalny kod |
| 2 | Publicznie dostępny kod źródłowy | Nadzór społeczności zapewnia brak złośliwego zachowania |
| 3 | Utrzymywany pakiet NuGet od zaufanego dostawcy/społeczności | Integralność łańcucha dostaw |

### Podstawa identyfikacji

Skaner identyfikuje zaufane zestawy po ich `AssemblyDefinition.Name` w metadanych PE — **nie po nazwie pliku DLL**. Zapobiega to obejściu kontroli przez atakujących poprzez zmianę nazwy złośliwej DLL na `Newtonsoft.Json.dll`.

## CollectTrustedTypeRefs — Zwolnienie przechodnie

Gdy PluginLoader ładuje katalog wtyczki, wykonuje dwufazowe skanowanie:

```
Faza 1: CollectTrustedTypeRefs(pluginDir)
├── Wyliczenie wszystkich plików *.dll w katalogu wtyczki
├── Dla każdej DLL: odczyt metadanych PE → sprawdzenie AssemblyDefinition.Name
├── Jeśli nazwa ∈ TrustedAssemblies:
│   └── Zebranie WSZYSTKICH wpisów TypeReference → pary (namespace, typeName)
└── Zwraca: HashSet<(string Namespace, string Name)>

Faza 2: ScanForbiddenReferences(pluginMainDll, trustedTypeRefs)
├── Warstwa 0:   Szybkie wyjście z białej listy (jeśli główna DLL jest zaufana → przejście)
├── Warstwa 0.5: Zwolnienie przechodnie (pominięcie TypeRefs w zbiorze trustedTypeRefs)
├── Warstwa 1:   Skanowanie tabeli TypeRef
├── Warstwa 2:   Skanowanie tabeli ExportedType
├── Warstwa 3:   Skanowanie tabeli MemberRef (niebezpieczne metody)
├── Warstwa 4:   Znaczniki niebezpiecznego kodu + P/Invoke
└── Warstwa 5:   Skanowanie sterty ciągów #US
```

### Dlaczego zwolnienie przechodnie jest ważne

Newtonsoft.Json wewnętrznie odwołuje się do typów takich jak `System.Reflection.MemberInfo`, `System.IO.TextReader` itp. Gdy twoja wtyczka odwołuje się do Newtonsoft.Json, kompilator może osadzić te przechodnie TypeRef w DLL **twojej** wtyczki. Bez zwolnienia przechodniego twoja wtyczka zostałaby oznaczona za odwoływanie się do `System.IO.TextReader` — mimo że nigdy go bezpośrednio nie używasz.

`CollectTrustedTypeRefs` rozwiązuje to, zbierając wcześniej wszystkie TypeRef z zaufanych DLL i oznaczając je jako „znane bezpieczne" podczas głównego skanowania.

## Jak dodać nową zaufaną zależność

Aby dodać nową bibliotekę do białej listy:

1. Zweryfikuj, czy spełnia trzy powyższe kryteria przyjęcia
2. Dodaj linię do HashSet `TrustedAssemblies` w `PluginLoader.cs`:
   ```csharp
   "YourLibraryName",  // Krótki opis dlaczego jest zaufana
   ```
3. Umieść DLL biblioteki w katalogu wtyczki (obok głównej DLL wtyczki)
4. Skaner automatycznie zbierze jej TypeRef i zwolni je

> **⚠️ Ważne:** Dodanie biblioteki do `TrustedAssemblies` oznacza, że skaner **nie** sprawdzi jej wewnętrznego kodu. Dodawaj tylko biblioteki, którym w pełni ufasz.

## To demo

Ta wtyczka używa Newtonsoft.Json bez żadnej deklaracji `PluginCapability`:

| Funkcja | Wewnętrzne zachowanie Newtonsoft.Json | Dlaczego działa |
|---------|---------------------------------------|-----------------|
| `JsonConvert.SerializeObject` | Używa refleksji do wyliczania właściwości | DLL Newtonsoft.Json przechodzi białą listę warstwy 0 |
| `JsonConvert.DeserializeObject<T>` | Wywołuje `Activator.CreateInstance`, ustawia właściwości przez refleksję | Przechodnie TypeRef zwolnione w warstwie 0.5 |
| Manipulacja `JObject` / `JArray` | Używa `System.Linq.Expressions`, dynamicznego wysyłania | Wszystkie wewnętrzne ref zebrane przez `CollectTrustedTypeRefs` |

### Kluczowa różnica wobec PluginCapability

| Mechanizm | Zakres | Przypadek użycia |
|-----------|--------|------------------|
| `TrustedAssemblies` | Zwalnia całą **bibliotekę** (i jej przechodnie ref) ze skanowania | Znane zależności open-source |
| `PluginCapability` | Zwalnia **kod twojej wtyczki** z zakazów określonych przestrzeni nazw | Wtyczka potrzebuje bezpośredniego dostępu do System.Net/IO/Process |

Wtyczka używająca tylko zaufanych zależności **nie potrzebuje** żadnej deklaracji `PluginCapability`. Skaner obsługuje wszystko automatycznie.

## Uwaga dotycząca bezpieczeństwa

Zaufane zestawy są zwolnione ze skanowania bezpieczeństwa, ponieważ są audytowalnymi projektami open-source. Jednak **kod twojej wtyczki** jest nadal w pełni skanowany. Jeśli twoja wtyczka bezpośrednio odwołuje się do `System.IO.File` lub `System.Net.Http.HttpClient`, nadal będzie zablokowana — chyba że zadeklarujesz odpowiednią `PluginCapability`. Zobacz [dokumentację bezpieczeństwa](../../docs/pl-PL/security.md).
