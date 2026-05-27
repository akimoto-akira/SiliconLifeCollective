# PluginDemo-11: Zakázané operace P/Invoke a unsafe kódu — Anti-vzor

## Přehled

Tento plugin demonstruje **zakázané** operace P/Invoke a unsafe kódu v systému pluginů SiliconLife. Na rozdíl od jiných zakázaných kategorií (souborové I/O, síť, procesy, reflexe), které mají bezpečné alternativy, P/Invoke a unsafe kód jsou **absolutně zakázány** — bez bezpečné alternativy a bez možnosti výjimky jakoukoliv deklarací `PluginCapability`.

## Proč je P/Invoke ultimátní hrozba?

P/Invoke a unsafe kód představují **nejzákladnější hrozbu** protože operují **zcela mimo spravované runtime**:

- Nativní kód se spouští s plnými oprávněními procesu
- Žádná spravovaná typová bezpečnost, bezpečnost paměti nebo garbage collection
- Nemožné zachytit, auditovat nebo izolovat nativní volání
- Pád nativního kódu = pád celého procesu (bez zpracování výjimek)
- Možný přístup k jakékoliv adrese paměti v prostoru procesu

## Mechanismus trojitého pojištění

PluginLoader používá **tři nezávislé detekční vrstvy**:

### Vrstva 1: Skenování tabulky TypeRef

Detekuje přímé reference na zakázané typy v PE metadatech:

| Zakázaný typ | Jmenný prostor | Hrozba |
|--------------|----------------|--------|
| `DllImportAttribute` | System.Runtime.InteropServices | Deklaruje import nativní funkce |
| `Marshal` | System.Runtime.InteropServices | Most spravované/nespravované paměti |
| `NativeMemory` | System.Runtime.InteropServices | Nativní halda malloc/free |
| `NativeLibrary` | System.Runtime.InteropServices | Dynamické načítání nativních knihoven |
| `GCHandle` | System.Runtime.InteropServices | Připnutí spravovaného objektu |
| `Unsafe` | System.Runtime.CompilerServices | Pomocná třída Unsafe |
| `UnverifiableCodeAttribute` | System.Security | Značka neověřitelného kódu |

### Vrstva 2: Skenování značek Unsafe (ScanUnsafeMarkers)

| Značka | Metoda detekce | Zdroj |
|--------|---------------|-------|
| `[assembly: UnverifiableCode]` | Tabulka CustomAttribute assembly | Klíčové slovo C# `unsafe` |
| `[module: UnverifiableCode]` | Tabulka CustomAttribute modulu | Klíčové slovo C# `unsafe` |
| `MethodAttributes.PinvokeImpl` | Příznak tabulky MethodDef | Atribut `[DllImport]` |

### Vrstva 3: Skenování řetězců IL (halda #US)

```
"System.Runtime.InteropServices.Marshal"  → Označeno
"System.Runtime.InteropServices.*"        → Označeno shodou prefixu
```

## Demonstrovaná porušení

### Porušení 1: Deklarace [DllImport]

```csharp
// ❌ ZAKÁZÁNO
[DllImport("kernel32.dll")]
private static extern ulong GetTickCount64();
```

### Porušení 2: Použití Marshal

```csharp
// ❌ ZAKÁZÁNO
IntPtr ptr = Marshal.AllocHGlobal(1024);
string? str = Marshal.PtrToStringAnsi(ptr);
Marshal.FreeHGlobal(ptr);
```

### Porušení 3: Použití NativeMemory

```csharp
// ❌ ZAKÁZÁNO
unsafe
{
    void* buffer = NativeMemory.Alloc(4096);
    NativeMemory.Free(buffer);
}
```

### Porušení 4: Připnutí GCHandle

```csharp
// ❌ ZAKÁZÁNO
GCHandle handle = GCHandle.Alloc(managedArray, GCHandleType.Pinned);
IntPtr ptr = handle.AddrOfPinnedObject();
handle.Free();
```

### Porušení 5: Blok unsafe

```csharp
// ❌ ZAKÁZÁNO
unsafe
{
    int* ptr = &value;
    *ptr = 100;
    byte* stack = stackalloc byte[256];
}
```

### Porušení 6: Načtení NativeLibrary

```csharp
// ❌ ZAKÁZÁNO
IntPtr lib = NativeLibrary.Load("evil.dll");
IntPtr funcPtr = NativeLibrary.GetExport(lib, "malicious_function");
NativeLibrary.Free(lib);
```

## Žádná bezpečná alternativa — Srovnání

| Zakázaná kategorie | Bezpečný wrapper | Auditovatelné | Deklarovatelné přes PluginCapability |
|-------------------|-----------------|---------------|--------------------------------------|
| Souborové I/O | PermissionedStreamFactory | ✅ Ano | ✅ Capability.FileIO |
| Síť | NetworkExecutor | ✅ Ano | ✅ Capability.Network |
| Proces | CommandLineExecutor | ✅ Ano | ✅ Capability.Process |
| Reflexe | ITypeRegistry + IObjectFactory | ✅ Ano | ❌ Vždy zakázáno |
| **P/Invoke a unsafe** | **❌ Žádný** | **❌ Nemožné** | **❌ Vždy zakázáno** |

## Pokud plugin skutečně potřebuje nativní kód

1. **Ruční audit správcem projektu**
2. **Přidání na bílou listinu `TrustedAssemblies`** v PluginLoader
3. **Identifikace přes `AssemblyDefinition.Name` PE metadat** (ne název souboru)

## Soubory

- `Plugin.cs` - Demonstrační plugin anti-vzoru
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

- **04-SafeSystemIO**: Bezpečné typy bílé listiny System.IO
- **06-TrustedDependency**: Mechanismus bílé listiny TrustedAssemblies
- **10-ForbiddenReflection**: Zakázané operace reflexe
- **12-ForbiddenStringBypass**: Pokusy o obejití pomocí řetězců reflexe
