# PluginDemo-12: Anti-vzor obcházení zakázanými řetězci reflexe

## Přehled

Tento plugin demonstruje **zakázané** pokusy o obcházení založené na řetězcích reflexe v systému pluginů SiliconLife. Ukazuje, proč konkatenace, interpolace, kódování a další techniky zamlžování **nemohou** obejít skenování haldy #US (User String) PluginLoaderu — **poslední linii obrany**.

## Co je halda #US?

V metadatech .NET PE (Portable Executable) **halda #US (User String)** uchovává všechny operandy řetězcových literálů používané IL instrukcemi `ldstr`. Pokaždé, když napíšete řetězcový literál v kódu C#, kompilátor ho uloží na tuto haldu.

```
Zdroj C#:     string s = "System.IO.File";
    ↓ kompilace
IL kód:       ldstr "System.IO.File"    ← odkazuje na token v haldě #US
    ↓ skenování PluginLoader
Halda #US:    [..., "System.IO.File", ...]  ← DETEKOVÁNO shodou prefixu!
```

Metoda `ScanUserStrings()` PluginLoaderu iteruje přes **každý záznam** haldy #US a kontroluje, zda nějaký řetězec začíná zakázaným prefixem.

## Zakázané prefixy řetězců

Následující prefixy spouštějí porušení `[ILString]` při nalezení v haldě #US:

| Prefix | Kategorie |
|--------|-----------|
| `System.IO.` | Typy souborového systému |
| `System.Net.Http` | HTTP klient |
| `System.Net.WebSockets` | WebSocket |
| `System.Net.Sockets` | Surové sokety |
| `System.Net.Mail` | SMTP |
| `System.Net.NetworkInformation` | Průzkum sítě |
| `System.Net.Security` | SslStream |
| `System.Diagnostics.Process` | Proces/příkazová řádka |
| `Microsoft.CodeAnalysis` | Kompilátor Roslyn |
| `System.Reflection.Emit` | Emise IL |
| `System.Runtime.Loader` | AssemblyLoadContext |
| `System.CodeDom.Compiler` | Starší CodeDom |
| `Microsoft.Win32` | Registr Windows |

## Demonstrovaná porušení

### Porušení 1: Přímý řetězec názvu typu

```csharp
// ❌ ZAKÁZÁNO — úplný řetězec je v haldě #US
Type? fileType = Type.GetType("System.IO.File, System.Runtime");
```

**Porušení**: `[ILString] "System.IO.File, System.Runtime" matches forbidden prefix "System.IO."`

### Porušení 2: Konkatenace řetězců (čas kompilace)

```csharp
// ❌ ZAKÁZÁNO — kompilátor složí const+const do jednoho záznamu #US
const string ns = "System.Net.Http";
const string typeName = ".HttpClient";
const string assembly = ", System.Net.Http";
Type? type = Type.GetType(ns + typeName + assembly);
```

**Porušení**: `[ILString] "System.Net.Http.HttpClient, System.Net.Http" matches forbidden prefix "System.Net.Http"`

### Porušení 3: Interpolace řetězců

```csharp
// ❌ ZAKÁZÁNO — literální části jsou uloženy v haldě #US
string className = "FileStream";
string fullName = $"System.IO.{className}, System.Runtime";
```

**Porušení**: `[ILString] "System.IO." matches forbidden prefix "System.IO."`

### Porušení 4: Pole Const

```csharp
// ❌ ZAKÁZÁNO — hodnoty const jsou inlinovány → objevují se v haldě #US
private const string ProcessType = "System.Diagnostics.Process";
private const string AssemblyName = ", System.Runtime";
Type? type = Type.GetType(ProcessType + AssemblyName);
```

**Porušení**: `[ILString] "System.Diagnostics.Process" matches forbidden prefix "System.Diagnostics.Process"`

### Porušení 5: Fragmenty částečných řetězců

```csharp
// ❌ ZAKÁZÁNO — každá část je samostatný ldstr, skenovaný nezávisle
string part1 = "System.Reflection.Emit";
string part2 = ".AssemblyBuilder";
string fullType = part1 + part2;
```

**Porušení**: `[ILString] "System.Reflection.Emit" matches forbidden prefix "System.Reflection.Emit"`

## Proč techniky zamlžování selhávají

| Technika | Proč selhává |
|----------|-------------|
| Konkatenace const | Kompilátor složí do jednoho záznamu #US |
| Interpolace řetězců | Literální části uloženy v haldě #US |
| Pole const | Hodnoty inlinovány → objeví se v #US |
| Rozdělení do proměnných | Každý operand `ldstr` skenován nezávisle |
| Kódování Base64 | Dekódování vyžaduje runtime metody, ale `Type.GetType` blokován MemberRef |
| Stavba pole char | Negeneruje `ldstr`, ale `Type.GetType` stále blokován MemberRef |
| Šifrování XOR | Šifrovaný řetězec nečitelný v #US, ale dešifrování + `Type.GetType` = MemberRef blokace |

**Klíčový poznatek**: Skenování #US blokuje **řetězec**. Skenování MemberRef blokuje **metodu**. Pro dynamické načtení typu potřebujete OBĚ. PluginLoader blokuje OBĚ nezávisle.

## Kompletní obranný řetězec

| Krok | Mechanismus | Co detekuje |
|------|------------|------------|
| 1 | Tabulka TypeRef | Přímé odkazy na zakázané typy |
| 2 | Tabulka ExportedType | Přesměrované typy ze zakázaných jmenných prostorů |
| 3 | Tabulka MemberRef | Volání `Type.GetType`, `Assembly.Load`, `Activator.CreateInstance` |
| 4 | Markery Unsafe | `[DllImport]`, bloky unsafe, příznak PinvokeImpl |
| **5** | **Skenování haldy #US** | **Řetězcové konstanty odpovídající zakázaným prefixům (toto demo)** |

## Soubory

- `Plugin.cs` - Plugin demonstrující anti-vzor
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
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Tento soubor (Čeština)

## Související příklady

- **10-ForbiddenReflection**: Zakázané metody reflexe (skenování MemberRef)
- **11-ForbiddenPInvoke**: Zakázaný P/Invoke a unsafe kód
- **02-TypeRegistryUsage**: Správné použití ITypeRegistry
- **03-ObjectFactoryUsage**: Správné použití IObjectFactory
