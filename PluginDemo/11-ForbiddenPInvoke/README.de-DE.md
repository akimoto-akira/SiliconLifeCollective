# PluginDemo-11: Verbotene P/Invoke- und unsafe-Code-Antimuster

## Übersicht

Dieses Plugin demonstriert **verbotene** P/Invoke- und unsafe-Code-Operationen im SiliconLife-Plugin-System. Im Gegensatz zu anderen verbotenen Kategorien (Datei-I/O, Netzwerk, Prozess, Reflection), die sichere Wrapper-Alternativen haben, sind P/Invoke und unsafe-Code **absolut verboten** — ohne sichere Alternative und durch keine `PluginCapability`-Deklaration freistellbar.

## Warum ist P/Invoke die ultimative Bedrohung?

P/Invoke und unsafe-Code stellen die **grundlegendste Bedrohung** für die Plugin-Sicherheit dar, da sie vollständig **außerhalb der verwalteten Laufzeit** operieren:

- Nativer Code wird mit vollen Prozessrechten ausgeführt
- Keine verwaltete Typsicherheit, Speichersicherheit oder Garbage Collection
- Native Aufrufe können nicht abgefangen, auditiert oder in Sandbox ausgeführt werden
- Nativer Code-Absturz = gesamter Prozessabsturz (keine Ausnahmebehandlung)
- Zugriff auf jede Speicheradresse im Prozessraum möglich

## Dreifach-Versicherungsmechanismus

PluginLoader verwendet **drei unabhängige Erkennungsebenen**, um sicherzustellen, dass P/Invoke und unsafe-Code niemals der Erkennung entgehen:

### Ebene 1: TypeRef-Tabellenscan

Erkennt direkte Referenzen auf verbotene Typen in PE-Metadaten:

| Verbotener Typ | Namespace | Bedrohung |
|----------------|-----------|-----------|
| `DllImportAttribute` | System.Runtime.InteropServices | Deklariert nativen Funktionsimport |
| `Marshal` | System.Runtime.InteropServices | Verwaltete/nicht verwaltete Speicherbrücke |
| `NativeMemory` | System.Runtime.InteropServices | Nativer Heap malloc/free |
| `NativeLibrary` | System.Runtime.InteropServices | Dynamisches Laden nativer Bibliotheken |
| `GCHandle` | System.Runtime.InteropServices | Verwaltetes Objekt fixieren, Zeiger freilegen |
| `Unsafe` | System.Runtime.CompilerServices | Unsafe-Hilfsklasse |
| `UnverifiableCodeAttribute` | System.Security | Nicht verifizierbarer Code-Marker |

### Ebene 2: Unsafe-Marker-Scan (ScanUnsafeMarkers)

Erkennt compilergenerierte Marker unabhängig von Typreferenzen:

| Marker | Erkennungsmethode | Quelle |
|--------|-------------------|--------|
| `[assembly: UnverifiableCode]` | Assembly-CustomAttribute-Tabelle | C# `unsafe`-Schlüsselwort |
| `[module: UnverifiableCode]` | Modul-CustomAttribute-Tabelle | C# `unsafe`-Schlüsselwort |
| `MethodAttributes.PinvokeImpl` | MethodDef-Tabellen-Flag | `[DllImport]`-Attribut |

### Ebene 3: IL-String-Scan (#US-Heap)

Fängt Stringkonstanten ab, die InteropServices-Typen referenzieren:

```
"System.Runtime.InteropServices.Marshal"  → Markiert
"System.Runtime.InteropServices.*"        → Präfix-Match markiert
```

## Demonstrierte Verstöße

### Verstoß 1: [DllImport]-Deklaration

```csharp
// ❌ VERBOTEN
[DllImport("kernel32.dll")]
private static extern ulong GetTickCount64();
```

**Erkennung:**
- `[TypeRef] System.Runtime.InteropServices.DllImportAttribute`
- `[PInvoke] GetTickCount64 (native interop)` (PinvokeImpl-Flag)

### Verstoß 2: Marshal-Verwendung

```csharp
// ❌ VERBOTEN
IntPtr ptr = Marshal.AllocHGlobal(1024);
string? str = Marshal.PtrToStringAnsi(ptr);
Marshal.FreeHGlobal(ptr);
```

**Erkennung:** `[TypeRef] System.Runtime.InteropServices.Marshal`

### Verstoß 3: NativeMemory-Verwendung

```csharp
// ❌ VERBOTEN
unsafe
{
    void* buffer = NativeMemory.Alloc(4096);
    NativeMemory.Free(buffer);
}
```

**Erkennung:**
- `[TypeRef] System.Runtime.InteropServices.NativeMemory`
- `[UnsafeMarker] [module: System.Security.UnverifiableCode]`

### Verstoß 4: GCHandle-Fixierung

```csharp
// ❌ VERBOTEN
GCHandle handle = GCHandle.Alloc(managedArray, GCHandleType.Pinned);
IntPtr ptr = handle.AddrOfPinnedObject();
handle.Free();
```

**Erkennung:** `[TypeRef] System.Runtime.InteropServices.GCHandle`

### Verstoß 5: unsafe-Block

```csharp
// ❌ VERBOTEN
unsafe
{
    int* ptr = &value;
    *ptr = 100;
    byte* stack = stackalloc byte[256];
}
```

**Erkennung:** `[UnsafeMarker] [module: System.Security.UnverifiableCode]`

### Verstoß 6: NativeLibrary-Laden

```csharp
// ❌ VERBOTEN
IntPtr lib = NativeLibrary.Load("evil.dll");
IntPtr funcPtr = NativeLibrary.GetExport(lib, "malicious_function");
NativeLibrary.Free(lib);
```

**Erkennung:** `[TypeRef] System.Runtime.InteropServices.NativeLibrary`

## Keine sichere Alternative — Vergleich

| Verbotene Kategorie | Sicherer Wrapper | Auditierbar | Per PluginCapability deklarierbar |
|---------------------|-----------------|-------------|-----------------------------------|
| Datei-I/O | PermissionedStreamFactory | ✅ Ja | ✅ Capability.FileIO |
| Netzwerk | NetworkExecutor | ✅ Ja | ✅ Capability.Network |
| Prozess | CommandLineExecutor | ✅ Ja | ✅ Capability.Process |
| Reflection | ITypeRegistry + IObjectFactory | ✅ Ja | ❌ Immer verboten |
| **P/Invoke & unsafe** | **❌ Keiner** | **❌ Unmöglich** | **❌ Immer verboten** |

## Wenn ein Plugin wirklich nativen Code benötigt

Wenn eine Bibliothek legitimerweise P/Invoke oder unsafe-Code verwendet:

1. **Manuelle Prüfung durch den Projektbetreuer** erforderlich
2. **Hinzufügen zur `TrustedAssemblies`-Whitelist** in PluginLoader erforderlich
3. **Identifikation über PE-Metadaten `AssemblyDefinition.Name`** (nicht Dateiname — verhindert Umbenennungsangriffe)

## Dateien

- `Plugin.cs` - Antimuster-Demoplugin
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

- **04-SafeSystemIO**: System.IO-Whitelist sichere Typen
- **06-TrustedDependency**: TrustedAssemblies-Whitelist-Mechanismus
- **10-ForbiddenReflection**: Verbotene Reflection-Operationen
- **12-ForbiddenStringBypass**: String-basierte Reflection-Bypass-Versuche
