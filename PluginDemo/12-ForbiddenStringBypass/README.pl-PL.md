# PluginDemo-12: Anty-wzorzec obejścia przez zabronione ciągi refleksji

## Przegląd

Ten plugin demonstruje **zabronione** próby obejścia oparte na ciągach refleksji w systemie wtyczek SiliconLife. Pokazuje, dlaczego konkatenacja, interpolacja, kodowanie i inne techniki zaciemniania **nie mogą** ominąć skanowania sterty #US (User String) PluginLoadera — **ostatniej linii obrony**.

## Czym jest sterta #US?

W metadanych .NET PE (Portable Executable) **sterta #US (User String)** przechowuje wszystkie operandy literałów łańcuchowych używanych przez instrukcje IL `ldstr`. Za każdym razem, gdy piszesz literał łańcuchowy w kodzie C#, kompilator zapisuje go na tej stercie.

```
Źródło C#:    string s = "System.IO.File";
    ↓ kompilacja
Kod IL:       ldstr "System.IO.File"    ← odwołuje się do tokenu na stercie #US
    ↓ skanowanie PluginLoader
Sterta #US:   [..., "System.IO.File", ...]  ← WYKRYTO przez dopasowanie prefiksu!
```

Metoda `ScanUserStrings()` PluginLoadera iteruje po **każdym wpisie** sterty #US, sprawdzając czy jakikolwiek ciąg zaczyna się od zabronionego prefiksu.

## Zabronione prefiksy ciągów

Następujące prefiksy wywołują naruszenia `[ILString]` po znalezieniu na stercie #US:

| Prefiks | Kategoria |
|---------|-----------|
| `System.IO.` | Typy systemu plików |
| `System.Net.Http` | Klient HTTP |
| `System.Net.WebSockets` | WebSocket |
| `System.Net.Sockets` | Surowe gniazda |
| `System.Net.Mail` | SMTP |
| `System.Net.NetworkInformation` | Sondowanie sieci |
| `System.Net.Security` | SslStream |
| `System.Diagnostics.Process` | Proces/wiersz poleceń |
| `Microsoft.CodeAnalysis` | Kompilator Roslyn |
| `System.Reflection.Emit` | Emisja IL |
| `System.Runtime.Loader` | AssemblyLoadContext |
| `System.CodeDom.Compiler` | Starszy CodeDom |
| `Microsoft.Win32` | Rejestr Windows |

## Zademonstrowane naruszenia

### Naruszenie 1: Bezpośredni ciąg nazwy typu

```csharp
// ❌ ZABRONIONE — pełny ciąg jest na stercie #US
Type? fileType = Type.GetType("System.IO.File, System.Runtime");
```

**Naruszenie**: `[ILString] "System.IO.File, System.Runtime" matches forbidden prefix "System.IO."`

### Naruszenie 2: Konkatenacja ciągów (czas kompilacji)

```csharp
// ❌ ZABRONIONE — kompilator składa const+const w jeden wpis #US
const string ns = "System.Net.Http";
const string typeName = ".HttpClient";
const string assembly = ", System.Net.Http";
Type? type = Type.GetType(ns + typeName + assembly);
```

**Naruszenie**: `[ILString] "System.Net.Http.HttpClient, System.Net.Http" matches forbidden prefix "System.Net.Http"`

### Naruszenie 3: Interpolacja ciągów

```csharp
// ❌ ZABRONIONE — części literalne są przechowywane na stercie #US
string className = "FileStream";
string fullName = $"System.IO.{className}, System.Runtime";
```

**Naruszenie**: `[ILString] "System.IO." matches forbidden prefix "System.IO."`

### Naruszenie 4: Pola Const

```csharp
// ❌ ZABRONIONE — wartości const są inline → pojawiają się na stercie #US
private const string ProcessType = "System.Diagnostics.Process";
private const string AssemblyName = ", System.Runtime";
Type? type = Type.GetType(ProcessType + AssemblyName);
```

**Naruszenie**: `[ILString] "System.Diagnostics.Process" matches forbidden prefix "System.Diagnostics.Process"`

### Naruszenie 5: Fragmenty częściowych ciągów

```csharp
// ❌ ZABRONIONE — każda część to osobny ldstr, skanowany niezależnie
string part1 = "System.Reflection.Emit";
string part2 = ".AssemblyBuilder";
string fullType = part1 + part2;
```

**Naruszenie**: `[ILString] "System.Reflection.Emit" matches forbidden prefix "System.Reflection.Emit"`

## Dlaczego techniki zaciemniania zawodzą

| Technika | Dlaczego zawodzi |
|----------|-----------------|
| Konkatenacja const | Kompilator składa w pojedynczy wpis #US |
| Interpolacja ciągów | Części literalne przechowywane na stercie #US |
| Pola const | Wartości inline → pojawiają się w #US |
| Podział na zmienne | Każdy operand `ldstr` skanowany niezależnie |
| Kodowanie Base64 | Dekodowanie wymaga metod runtime, ale `Type.GetType` blokowany przez MemberRef |
| Budowa tablicy char | Nie generuje `ldstr`, ale `Type.GetType` nadal blokowany przez MemberRef |
| Szyfrowanie XOR | Zaszyfrowany ciąg nieczytelny w #US, ale deszyfrowanie + `Type.GetType` = MemberRef blokada |

**Kluczowy wniosek**: Skanowanie #US blokuje **ciąg**. Skanowanie MemberRef blokuje **metodę**. Aby dynamicznie załadować typ, potrzebujesz OBU. PluginLoader blokuje OBA niezależnie.

## Kompletny łańcuch obrony

| Krok | Mechanizm | Co wykrywa |
|------|-----------|-----------|
| 1 | Tabela TypeRef | Bezpośrednie odwołania do zabronionych typów |
| 2 | Tabela ExportedType | Przekierowane typy z zabronionych przestrzeni nazw |
| 3 | Tabela MemberRef | Wywołania `Type.GetType`, `Assembly.Load`, `Activator.CreateInstance` |
| 4 | Markery Unsafe | `[DllImport]`, bloki unsafe, flaga PinvokeImpl |
| **5** | **Skanowanie sterty #US** | **Stałe łańcuchowe pasujące do zabronionych prefiksów (to demo)** |

## Pliki

- `Plugin.cs` - Plugin demonstracyjny anty-wzorca
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Ten plik (Polski)
- `README.cs-CZ.md` - Čeština

## Powiązane przykłady

- **10-ForbiddenReflection**: Zabronione metody refleksji (skanowanie MemberRef)
- **11-ForbiddenPInvoke**: Zabroniony P/Invoke i kod unsafe
- **02-TypeRegistryUsage**: Prawidłowe użycie ITypeRegistry
- **03-ObjectFactoryUsage**: Prawidłowe użycie IObjectFactory
