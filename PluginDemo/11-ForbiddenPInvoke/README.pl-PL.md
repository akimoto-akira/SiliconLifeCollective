# PluginDemo-11: Zabronione operacje P/Invoke i kodu unsafe — Anty-wzorzec

## Przegląd

Ten plugin demonstruje **zabronione** operacje P/Invoke i kodu unsafe w systemie pluginów SiliconLife. W przeciwieństwie do innych zabronionych kategorii (I/O plików, sieć, procesy, refleksja), które mają bezpieczne alternatywy, P/Invoke i kod unsafe są **bezwzględnie zabronione** — bez bezpiecznej alternatywy i bez możliwości zwolnienia przez jakąkolwiek deklarację `PluginCapability`.

## Dlaczego P/Invoke jest ostatecznym zagrożeniem?

P/Invoke i kod unsafe stanowią **najbardziej fundamentalne zagrożenie** ponieważ działają **całkowicie poza zarządzanym środowiskiem uruchomieniowym**:

- Kod natywny wykonuje się z pełnymi uprawnieniami procesu
- Brak zarządzanego bezpieczeństwa typów, bezpieczeństwa pamięci lub garbage collection
- Niemożliwe przechwycenie, audyt lub sandboxing wywołań natywnych
- Awaria kodu natywnego = awaria całego procesu (brak obsługi wyjątków)
- Możliwy dostęp do dowolnego adresu pamięci w przestrzeni procesu

## Mechanizm potrójnego zabezpieczenia

PluginLoader wykorzystuje **trzy niezależne warstwy wykrywania**:

### Warstwa 1: Skanowanie tabeli TypeRef

Wykrywa bezpośrednie referencje do zabronionych typów w metadanych PE:

| Zabroniony typ | Przestrzeń nazw | Zagrożenie |
|----------------|-----------------|------------|
| `DllImportAttribute` | System.Runtime.InteropServices | Deklaruje import funkcji natywnej |
| `Marshal` | System.Runtime.InteropServices | Most pamięci zarządzanej/niezarządzanej |
| `NativeMemory` | System.Runtime.InteropServices | Natywna sterta malloc/free |
| `NativeLibrary` | System.Runtime.InteropServices | Dynamiczne ładowanie bibliotek natywnych |
| `GCHandle` | System.Runtime.InteropServices | Przypięcie obiektu zarządzanego |
| `Unsafe` | System.Runtime.CompilerServices | Klasa pomocnicza Unsafe |
| `UnverifiableCodeAttribute` | System.Security | Znacznik kodu nieweryfikowalnego |

### Warstwa 2: Skanowanie znaczników Unsafe (ScanUnsafeMarkers)

| Znacznik | Metoda wykrywania | Źródło |
|----------|-------------------|--------|
| `[assembly: UnverifiableCode]` | Tabela CustomAttribute assembly | Słowo kluczowe C# `unsafe` |
| `[module: UnverifiableCode]` | Tabela CustomAttribute modułu | Słowo kluczowe C# `unsafe` |
| `MethodAttributes.PinvokeImpl` | Flaga tabeli MethodDef | Atrybut `[DllImport]` |

### Warstwa 3: Skanowanie ciągów IL (sterta #US)

```
"System.Runtime.InteropServices.Marshal"  → Oznaczony
"System.Runtime.InteropServices.*"        → Oznaczony przez dopasowanie prefiksu
```

## Zademonstrowane naruszenia

### Naruszenie 1: Deklaracja [DllImport]

```csharp
// ❌ ZABRONIONE
[DllImport("kernel32.dll")]
private static extern ulong GetTickCount64();
```

### Naruszenie 2: Użycie Marshal

```csharp
// ❌ ZABRONIONE
IntPtr ptr = Marshal.AllocHGlobal(1024);
string? str = Marshal.PtrToStringAnsi(ptr);
Marshal.FreeHGlobal(ptr);
```

### Naruszenie 3: Użycie NativeMemory

```csharp
// ❌ ZABRONIONE
unsafe
{
    void* buffer = NativeMemory.Alloc(4096);
    NativeMemory.Free(buffer);
}
```

### Naruszenie 4: Przypięcie GCHandle

```csharp
// ❌ ZABRONIONE
GCHandle handle = GCHandle.Alloc(managedArray, GCHandleType.Pinned);
IntPtr ptr = handle.AddrOfPinnedObject();
handle.Free();
```

### Naruszenie 5: Blok unsafe

```csharp
// ❌ ZABRONIONE
unsafe
{
    int* ptr = &value;
    *ptr = 100;
    byte* stack = stackalloc byte[256];
}
```

### Naruszenie 6: Ładowanie NativeLibrary

```csharp
// ❌ ZABRONIONE
IntPtr lib = NativeLibrary.Load("evil.dll");
IntPtr funcPtr = NativeLibrary.GetExport(lib, "malicious_function");
NativeLibrary.Free(lib);
```

## Brak bezpiecznej alternatywy — Porównanie

| Zabroniona kategoria | Bezpieczny wrapper | Audytowalny | Deklrowalny przez PluginCapability |
|---------------------|-------------------|-------------|-------------------------------------|
| I/O plików | PermissionedStreamFactory | ✅ Tak | ✅ Capability.FileIO |
| Sieć | NetworkExecutor | ✅ Tak | ✅ Capability.Network |
| Proces | CommandLineExecutor | ✅ Tak | ✅ Capability.Process |
| Refleksja | ITypeRegistry + IObjectFactory | ✅ Tak | ❌ Zawsze zabronione |
| **P/Invoke i unsafe** | **❌ Brak** | **❌ Niemożliwe** | **❌ Zawsze zabronione** |

## Jeśli plugin naprawdę potrzebuje kodu natywnego

1. **Ręczny audyt przez opiekuna projektu**
2. **Dodanie do białej listy `TrustedAssemblies`** w PluginLoader
3. **Identyfikacja przez `AssemblyDefinition.Name` metadanych PE** (nie nazwę pliku)

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

- **04-SafeSystemIO**: Bezpieczne typy białej listy System.IO
- **06-TrustedDependency**: Mechanizm białej listy TrustedAssemblies
- **10-ForbiddenReflection**: Zabronione operacje refleksji
- **12-ForbiddenStringBypass**: Próby obejścia przez ciągi refleksji
