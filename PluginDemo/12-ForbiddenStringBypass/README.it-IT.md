# PluginDemo-12: Anti-Pattern di Bypass tramite Stringhe di Riflessione Vietate

## Panoramica

Questo plugin dimostra tentativi **vietati** di bypass basati su stringhe di riflessione nel sistema di plugin SiliconLife. Mostra perché la concatenazione, l'interpolazione, la codifica e altre tecniche di offuscamento **non possono** eludere la scansione dell'heap #US (User String) di PluginLoader — l'**ultima linea di difesa**.

## Cos'è l'heap #US?

Nei metadati .NET PE (Portable Executable), l'**heap #US (User String)** memorizza tutti gli operandi di stringhe letterali usati dalle istruzioni IL `ldstr`. Ogni volta che scrivi una stringa letterale in codice C#, il compilatore la memorizza in questo heap.

```
Sorgente C#:  string s = "System.IO.File";
    ↓ compilazione
Codice IL:    ldstr "System.IO.File"    ← riferisce token nell'heap #US
    ↓ scansione PluginLoader
Heap #US:     [..., "System.IO.File", ...]  ← RILEVATO per corrispondenza di prefisso!
```

Il metodo `ScanUserStrings()` di PluginLoader itera su **ogni voce** dell'heap #US, verificando se qualche stringa inizia con un prefisso vietato.

## Prefissi di stringhe vietati

I seguenti prefissi attivano violazioni `[ILString]` quando trovati nell'heap #US:

| Prefisso | Categoria |
|----------|-----------|
| `System.IO.` | Tipi di file system |
| `System.Net.Http` | Client HTTP |
| `System.Net.WebSockets` | WebSocket |
| `System.Net.Sockets` | Socket grezzi |
| `System.Net.Mail` | SMTP |
| `System.Net.NetworkInformation` | Sondaggio di rete |
| `System.Net.Security` | SslStream |
| `System.Diagnostics.Process` | Processo/riga di comando |
| `Microsoft.CodeAnalysis` | Compilatore Roslyn |
| `System.Reflection.Emit` | Emissione IL |
| `System.Runtime.Loader` | AssemblyLoadContext |
| `System.CodeDom.Compiler` | CodeDom legacy |
| `Microsoft.Win32` | Registro Windows |

## Violazioni dimostrate

### Violazione 1: Stringa diretta del nome del tipo

```csharp
// ❌ VIETATO — la stringa completa è nell'heap #US
Type? fileType = Type.GetType("System.IO.File, System.Runtime");
```

**Violazione**: `[ILString] "System.IO.File, System.Runtime" matches forbidden prefix "System.IO."`

### Violazione 2: Concatenazione di stringhe (compilazione)

```csharp
// ❌ VIETATO — il compilatore piega const+const in una voce #US
const string ns = "System.Net.Http";
const string typeName = ".HttpClient";
const string assembly = ", System.Net.Http";
Type? type = Type.GetType(ns + typeName + assembly);
```

**Violazione**: `[ILString] "System.Net.Http.HttpClient, System.Net.Http" matches forbidden prefix "System.Net.Http"`

### Violazione 3: Interpolazione di stringhe

```csharp
// ❌ VIETATO — le parti letterali vengono memorizzate nell'heap #US
string className = "FileStream";
string fullName = $"System.IO.{className}, System.Runtime";
```

**Violazione**: `[ILString] "System.IO." matches forbidden prefix "System.IO."`

### Violazione 4: Campi Const

```csharp
// ❌ VIETATO — i valori const vengono inlineati → appaiono nell'heap #US
private const string ProcessType = "System.Diagnostics.Process";
private const string AssemblyName = ", System.Runtime";
Type? type = Type.GetType(ProcessType + AssemblyName);
```

**Violazione**: `[ILString] "System.Diagnostics.Process" matches forbidden prefix "System.Diagnostics.Process"`

### Violazione 5: Frammenti di stringhe parziali

```csharp
// ❌ VIETATO — ogni parte è un ldstr separato, scansionato indipendentemente
string part1 = "System.Reflection.Emit";
string part2 = ".AssemblyBuilder";
string fullType = part1 + part2;
```

**Violazione**: `[ILString] "System.Reflection.Emit" matches forbidden prefix "System.Reflection.Emit"`

## Perché le tecniche di offuscamento falliscono tutte

| Tecnica | Perché fallisce |
|---------|----------------|
| Concatenazione const | Il compilatore piega in una singola voce #US |
| Interpolazione di stringhe | Parti letterali memorizzate nell'heap #US |
| Campi const | Valori inlineati → appaiono in #US |
| Divisione in variabili | Ogni operando `ldstr` scansionato indipendentemente |
| Codifica Base64 | La decodifica richiede metodi runtime, ma `Type.GetType` è bloccato da MemberRef |
| Costruzione array di char | Nessun `ldstr` emesso, ma `Type.GetType` resta bloccato da MemberRef |
| Crittografia XOR | Stringa cifrata illeggibile in #US, ma decrittazione + `Type.GetType` = MemberRef bloccato |

**Insight chiave**: La scansione #US blocca la **stringa**. La scansione MemberRef blocca il **metodo**. Per caricare dinamicamente un tipo, servono ENTRAMBI. PluginLoader blocca ENTRAMBI indipendentemente.

## La catena di difesa completa

| Passo | Meccanismo | Cosa rileva |
|-------|------------|------------|
| 1 | Tabella TypeRef | Riferimenti diretti a tipi vietati |
| 2 | Tabella ExportedType | Tipi inoltrati da namespace vietati |
| 3 | Tabella MemberRef | Chiamate a `Type.GetType`, `Assembly.Load`, `Activator.CreateInstance` |
| 4 | Marcatori Unsafe | `[DllImport]`, blocchi unsafe, flag PinvokeImpl |
| **5** | **Scansione heap #US** | **Costanti stringa corrispondenti a prefissi vietati (questa demo)** |

## File

- `Plugin.cs` - Plugin dimostrativo anti-pattern
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Questo file (Italiano)
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## Esempi correlati

- **10-ForbiddenReflection**: Metodi di riflessione vietati (scansione MemberRef)
- **11-ForbiddenPInvoke**: P/Invoke e codice unsafe vietati
- **02-TypeRegistryUsage**: Uso corretto di ITypeRegistry
- **03-ObjectFactoryUsage**: Uso corretto di IObjectFactory
