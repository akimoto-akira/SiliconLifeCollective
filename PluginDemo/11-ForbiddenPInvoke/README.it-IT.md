# PluginDemo-11: Anti-pattern P/Invoke e codice unsafe vietati

## Panoramica

Questo plugin dimostra le operazioni P/Invoke e codice unsafe **vietate** nel sistema di plugin SiliconLife. A differenza di altre categorie vietate (I/O file, rete, processi, reflection) che hanno alternative sicure, P/Invoke e codice unsafe sono **divieti assoluti** — senza alternativa sicura e non esentabili da nessuna dichiarazione `PluginCapability`.

## Perché P/Invoke è la minaccia definitiva?

P/Invoke e il codice unsafe rappresentano la **minaccia più fondamentale** perché operano **completamente al di fuori del runtime gestito**:

- Il codice nativo viene eseguito con pieni privilegi di processo
- Nessuna sicurezza dei tipi gestita, sicurezza della memoria o garbage collection
- Impossibile intercettare, verificare o isolare le chiamate native
- Crash del codice nativo = crash dell'intero processo (nessuna gestione delle eccezioni)
- Accesso possibile a qualsiasi indirizzo di memoria dello spazio del processo

## Meccanismo di tripla assicurazione

PluginLoader utilizza **tre livelli di rilevamento indipendenti**:

### Livello 1: Scansione tabella TypeRef

Rileva riferimenti diretti a tipi vietati nei metadati PE:

| Tipo vietato | Namespace | Minaccia |
|--------------|-----------|----------|
| `DllImportAttribute` | System.Runtime.InteropServices | Dichiara importazione funzione nativa |
| `Marshal` | System.Runtime.InteropServices | Ponte memoria gestita/non gestita |
| `NativeMemory` | System.Runtime.InteropServices | Malloc/free heap nativo |
| `NativeLibrary` | System.Runtime.InteropServices | Caricamento dinamico librerie native |
| `GCHandle` | System.Runtime.InteropServices | Fissare oggetto gestito, esporre puntatore |
| `Unsafe` | System.Runtime.CompilerServices | Classe helper Unsafe |
| `UnverifiableCodeAttribute` | System.Security | Marcatore codice non verificabile |

### Livello 2: Scansione marcatori Unsafe (ScanUnsafeMarkers)

| Marcatore | Metodo di rilevamento | Fonte |
|-----------|---------------------|-------|
| `[assembly: UnverifiableCode]` | Tabella CustomAttribute assembly | Parola chiave C# `unsafe` |
| `[module: UnverifiableCode]` | Tabella CustomAttribute modulo | Parola chiave C# `unsafe` |
| `MethodAttributes.PinvokeImpl` | Flag tabella MethodDef | Attributo `[DllImport]` |

### Livello 3: Scansione stringhe IL (heap #US)

```
"System.Runtime.InteropServices.Marshal"  → Segnalato
"System.Runtime.InteropServices.*"        → Segnalato per corrispondenza prefisso
```

## Violazioni dimostrate

### Violazione 1: Dichiarazione [DllImport]

```csharp
// ❌ VIETATO
[DllImport("kernel32.dll")]
private static extern ulong GetTickCount64();
```

### Violazione 2: Uso di Marshal

```csharp
// ❌ VIETATO
IntPtr ptr = Marshal.AllocHGlobal(1024);
string? str = Marshal.PtrToStringAnsi(ptr);
Marshal.FreeHGlobal(ptr);
```

### Violazione 3: Uso di NativeMemory

```csharp
// ❌ VIETATO
unsafe
{
    void* buffer = NativeMemory.Alloc(4096);
    NativeMemory.Free(buffer);
}
```

### Violazione 4: Fissaggio GCHandle

```csharp
// ❌ VIETATO
GCHandle handle = GCHandle.Alloc(managedArray, GCHandleType.Pinned);
IntPtr ptr = handle.AddrOfPinnedObject();
handle.Free();
```

### Violazione 5: Blocco unsafe

```csharp
// ❌ VIETATO
unsafe
{
    int* ptr = &value;
    *ptr = 100;
    byte* stack = stackalloc byte[256];
}
```

### Violazione 6: Caricamento NativeLibrary

```csharp
// ❌ VIETATO
IntPtr lib = NativeLibrary.Load("evil.dll");
IntPtr funcPtr = NativeLibrary.GetExport(lib, "malicious_function");
NativeLibrary.Free(lib);
```

## Nessuna alternativa sicura — Confronto

| Categoria vietata | Wrapper sicuro | Verificabile | Dichiarabile via PluginCapability |
|-------------------|---------------|-------------|-----------------------------------|
| I/O file | PermissionedStreamFactory | ✅ Sì | ✅ Capability.FileIO |
| Rete | NetworkExecutor | ✅ Sì | ✅ Capability.Network |
| Processo | CommandLineExecutor | ✅ Sì | ✅ Capability.Process |
| Reflection | ITypeRegistry + IObjectFactory | ✅ Sì | ❌ Sempre vietato |
| **P/Invoke e unsafe** | **❌ Nessuno** | **❌ Impossibile** | **❌ Sempre vietato** |

## Se un plugin ha davvero bisogno di codice nativo

1. **Audit manuale da parte del manutentore del progetto**
2. **Aggiunta alla whitelist `TrustedAssemblies`** in PluginLoader
3. **Identificazione tramite `AssemblyDefinition.Name` dei metadati PE** (non il nome file)

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

- **04-SafeSystemIO**: Tipi sicuri della whitelist System.IO
- **06-TrustedDependency**: Meccanismo whitelist TrustedAssemblies
- **10-ForbiddenReflection**: Operazioni di reflection vietate
- **12-ForbiddenStringBypass**: Tentativi di bypass tramite stringhe di reflection
