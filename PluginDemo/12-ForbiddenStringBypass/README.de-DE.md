# PluginDemo-12: Verbotenes String-Reflection-Bypass Anti-Pattern

## Überblick

Dieses Plugin demonstriert **verbotene** stringbasierte Reflection-Bypass-Versuche im SiliconLife-Plugin-System. Es zeigt, warum String-Verkettung, Interpolation, Kodierung und andere Verschleierungstechniken das #US (User String) Heap-Scanning des PluginLoaders — die **letzte Verteidigungslinie** — **nicht umgehen können**.

## Was ist der #US-Heap?

In .NET PE (Portable Executable) Metadaten speichert der **#US (User String) Heap** alle String-Literal-Operanden, die von `ldstr` IL-Anweisungen verwendet werden. Jedes Mal, wenn Sie ein String-Literal in C#-Code schreiben, speichert der Compiler es in diesem Heap.

```
C# Quelle:   string s = "System.IO.File";
    ↓ Kompilierung
IL Code:      ldstr "System.IO.File"    ← referenziert Token im #US-Heap
    ↓ PluginLoader-Scan
#US Heap:     [..., "System.IO.File", ...]  ← durch Präfix-Matching erkannt!
```

Die `ScanUserStrings()`-Methode des PluginLoaders iteriert über **jeden einzelnen Eintrag** im #US-Heap und prüft, ob ein String mit einem verbotenen Präfix beginnt.

## Verbotene String-Präfixe

Die folgenden Präfixe lösen `[ILString]`-Verstöße aus, wenn sie im #US-Heap gefunden werden:

| Präfix | Kategorie |
|--------|-----------|
| `System.IO.` | Dateisystem-Typen |
| `System.Net.Http` | HTTP-Client |
| `System.Net.WebSockets` | WebSocket |
| `System.Net.Sockets` | Raw-Sockets |
| `System.Net.Mail` | SMTP |
| `System.Net.NetworkInformation` | Netzwerk-Probing |
| `System.Net.Security` | SslStream |
| `System.Diagnostics.Process` | Prozess/Kommandozeile |
| `Microsoft.CodeAnalysis` | Roslyn-Compiler |
| `System.Reflection.Emit` | IL-Emission |
| `System.Runtime.Loader` | AssemblyLoadContext |
| `System.CodeDom.Compiler` | Legacy CodeDom |
| `Microsoft.Win32` | Windows-Registry |

## Demonstrierte Verstöße

### Verstoß 1: Direkter Typname-String

```csharp
// ❌ VERBOTEN — der vollständige String befindet sich im #US-Heap
Type? fileType = Type.GetType("System.IO.File, System.Runtime");
```

**Verstoß**: `[ILString] "System.IO.File, System.Runtime" matches forbidden prefix "System.IO."`

### Verstoß 2: String-Verkettung (Compile-Time)

```csharp
// ❌ VERBOTEN — Compiler faltet const+const in einen #US-Eintrag
const string ns = "System.Net.Http";
const string typeName = ".HttpClient";
const string assembly = ", System.Net.Http";
Type? type = Type.GetType(ns + typeName + assembly);
// Sowohl die Teile als auch das gefaltete Ergebnis sind im #US-Heap!
```

**Verstoß**: `[ILString] "System.Net.Http.HttpClient, System.Net.Http" matches forbidden prefix "System.Net.Http"`

### Verstoß 3: String-Interpolation

```csharp
// ❌ VERBOTEN — Literal-Teile werden im #US-Heap gespeichert
string className = "FileStream";
string fullName = $"System.IO.{className}, System.Runtime";
// "System.IO." allein stimmt bereits mit dem Präfix überein!
```

**Verstoß**: `[ILString] "System.IO." matches forbidden prefix "System.IO."`

### Verstoß 4: Const-Felder

```csharp
// ❌ VERBOTEN — const-Werte werden an der Verwendungsstelle eingebettet → erscheinen im #US-Heap
private const string ProcessType = "System.Diagnostics.Process";
private const string AssemblyName = ", System.Runtime";
Type? type = Type.GetType(ProcessType + AssemblyName);
```

**Verstoß**: `[ILString] "System.Diagnostics.Process" matches forbidden prefix "System.Diagnostics.Process"`

### Verstoß 5: Teilstring-Fragmente

```csharp
// ❌ VERBOTEN — jeder Teil ist ein separater ldstr, der unabhängig gescannt wird
string part1 = "System.Reflection.Emit";
string part2 = ".AssemblyBuilder";
string fullType = part1 + part2;
// "System.Reflection.Emit" allein stimmt bereits mit dem Präfix überein!
```

**Verstoß**: `[ILString] "System.Reflection.Emit" matches forbidden prefix "System.Reflection.Emit"`

### Verstoß 6: Mehrere verbotene Ziele

```csharp
// ❌ VERBOTEN — ALLE Strings in der gesamten Assembly werden gescannt
string tcp = "System.Net.Sockets.TcpClient";
string registry = "Microsoft.Win32.Registry";
string loader = "System.Runtime.Loader.AssemblyLoadContext";
```

**Mehrere Verstöße** — der Scanner stoppt NICHT beim ersten Treffer.

## Warum alle Verschleierungstechniken scheitern

| Technik | Warum sie scheitert |
|---------|---------------------|
| Const-Verkettung | Compiler faltet in einzelnen #US-Eintrag |
| String-Interpolation | Literal-Teile im #US-Heap gespeichert |
| Const-Felder | Werte eingebettet → erscheinen im #US |
| Aufteilung in Variablen | Jeder `ldstr`-Operand wird unabhängig gescannt |
| Base64-Kodierung | Dekodierung benötigt Runtime-Methoden, aber `Type.GetType` ist MemberRef-blockiert |
| Char-Array-Aufbau | Kein `ldstr` erzeugt, aber `Type.GetType` bleibt MemberRef-blockiert |
| XOR-Verschlüsselung | Verschlüsselter String im #US nicht lesbar, aber Entschlüsselung + `Type.GetType` = MemberRef-blockiert |
| String-Umkehr | Umgekehrtes Literal kann immer noch zum Präfix passen, reverse + GetType = MemberRef-blockiert |

**Kernaussage**: Der #US-Scan blockiert den **String**. Der MemberRef-Scan blockiert die **Methode**. Um einen Typ dynamisch zu laden, benötigt man BEIDES. PluginLoader blockiert BEIDES unabhängig voneinander.

## Die vollständige Verteidigungskette

Das 5-Schritte-Scanning des PluginLoaders schafft eine undurchbrechbare Verteidigung:

| Schritt | Mechanismus | Was erkannt wird |
|---------|-------------|-----------------|
| 1 | TypeRef-Tabelle | Direkte Referenzen auf verbotene Typen |
| 2 | ExportedType-Tabelle | Weitergeleitete Typen aus verbotenen Namespaces |
| 3 | MemberRef-Tabelle | Aufrufe von `Type.GetType`, `Assembly.Load`, `Activator.CreateInstance` |
| 4 | Unsafe-Marker | `[DllImport]`, unsafe-Blöcke, PinvokeImpl-Flag |
| **5** | **#US-Heap-Scan** | **String-Konstanten, die mit verbotenen Präfixen übereinstimmen (dieses Demo)** |

Schritt 5 ist die **letzte Verteidigungslinie**, weil:
- Schritte 1-4 **aktiven Code** erkennen (Typen, Methoden, Attribute)
- Schritt 5 **passive Daten** erkennt (Strings, die für Umgehung verwendet werden KÖNNTEN)
- Selbst wenn der MemberRef-Scan eine unbekannte Methode übersieht, MUSS der verbotene Typname-String irgendwo in der Assembly existieren → #US-Scan erkennt ihn

## Kompromisse

Der #US-Heap-Scan kann **geringfügige Fehlalarme** erzeugen:
- Log-Nachrichten: `logger.Info("Failed to connect to System.Net.Http endpoint")` → markiert
- Dokumentations-Strings: In XML-Docs kompilierte Kommentare sind NICHT im #US-Heap (sicher)
- `nameof()`-Ausdrücke: Compile-Time-Strings, die nicht mit Präfixen übereinstimmen, sind sicher

**Lösung für Fehlalarme**: String-Literale umformulieren, um verbotene Präfixe zu vermeiden.

## Vergleich mit anderen Beispielen

| Beispiel | Fokus | Beziehung |
|----------|-------|-----------|
| **10-ForbiddenReflection** | Verbotene Reflection-Methoden (MemberRef-Scan) | Erkennt die METHODE |
| **11-ForbiddenPInvoke** | Verbotener P/Invoke und unsafe Code | Anderer Bedrohungsvektor |
| **12-ForbiddenStringBypass** | Stringbasierte Bypass-Versuche (dieses Beispiel) | Erkennt den STRING |
| **02-TypeRegistryUsage** | Sichere Typerkennung | Korrekte Alternative |
| **03-ObjectFactoryUsage** | Sichere Objekterstellung | Korrekte Alternative |

## Best Practices

1. **Verbotene Präfixe in ALLEN Strings vermeiden** — einschließlich Log-Nachrichten und in IL kompilierter Kommentare
2. **ITypeRegistry/IObjectFactory verwenden** — die sichere Alternative zu String-Reflection
3. **Verstehen, dass String-Aufteilung nicht hilft** — jedes Fragment wird unabhängig gescannt
4. **Runtime-Konstruktion (char[]) umgeht #US** — aber wird durch MemberRef-Scan von Type.GetType blockiert
5. **Tiefenverteidigung funktioniert** — selbst beim Umgehen einer Schicht erkennen die anderen

## Dateien

- `Plugin.cs` - Anti-Pattern-Demo-Plugin
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Diese Datei (Deutsch)
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## Verwandte Beispiele

- **10-ForbiddenReflection**: Verbotene Reflection-Methoden (MemberRef-Scan)
- **11-ForbiddenPInvoke**: Verbotener P/Invoke und unsafe Code
- **02-TypeRegistryUsage**: Korrekte Verwendung von ITypeRegistry
- **03-ObjectFactoryUsage**: Korrekte Verwendung von IObjectFactory
