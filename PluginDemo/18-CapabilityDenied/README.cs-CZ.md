# PluginDemo-18: Schopnost odepřena — Antivzor nedeklarovatelné schopnosti

## Přehled

Tento plugin je **antivzor**, který demonstruje, že deklarace schopnosti NEobchází zákazy nedeklarovatelných schopností. I s `[PluginCapability(Capability.Network)]` zůstávají P/Invoke, Unsafe, Reflection.Emit a přístup k registru **vždy** zablokovány.

## Deklarovatelné vs. nedeklarovatelné schopnosti

### ✅ Deklarovatelné (hodnoty výčtu Capability existují)

| Schopnost | Co vyjímá |
|----------|----------|
| `Capability.Network` | Jmenné prostory System.Net.* a zákazy podle typu |
| `Capability.FileIO` | Jmenný prostor System.IO (nad rámec bílé listiny) |
| `Capability.Process` | Typy Process* pod System.Diagnostics |
| `Capability.AI` | Umožňuje vkládání IAIService (bez výjimky TypeRef) |

### ❌ Nedeklarovatelné (NEEXISTUJE hodnota výčtu Capability)

| Kategorie | Blokované typy | Proč nedeklarovatelné |
|----------|---------------|---------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory`, `NativeLibrary` | Nelze auditovat libovolný nativní kód za běhu |
| Nebezpečný kód | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | Obchází bezpečnost typů CLR a kontrolu hranic |
| IL emise | `System.Reflection.Emit.*` | Může generovat libovolný IL za běhu |
| Načítání sestavení | `System.Runtime.Loader`, `Assembly.Load*` | Může načítat neskennované DLL, čímž obchází bezpečnostní sken |
| Registr | `Microsoft.Win32.*` | Přístup k systému na úrovni OS mimo sandbox pluginu |
| Dynamická kompilace | `Microsoft.CodeAnalysis.*` | Může kompilovat a spouštět libovolný kód |
| Nebezpečná reflexe | `Type.GetType(string)`, `Activator.CreateInstance` | Může instanciovat zakázané typy přes řetězec |

## Proč tyto schopnosti nelze deklarovat

Zásadní důvod: **nelze je bezpečně auditovat za běhu.**

1. **P/Invoke**: Jakmile je zavolán nativní kód, CLR nemá žádnou viditelnost — žádné záruky bezpečnosti
2. **Unsafe**: Obchází systém bezpečnosti typů, na kterém závisí model zabezpečení pluginu
3. **Reflection.Emit**: Může generovat nový IL za běhu, který nebyl nikdy skenován PluginLoaderem
4. **AssemblyLoadContext**: Může načítat DLL, které nebyly nikdy bezpečnostně skenovány
5. **Registry**: Poskytuje přístup ke konfiguraci na úrovni OS mimo sandbox pluginu

## Zpracování „neplatné deklarace" PluginLoaderem

Když PluginLoader narazí na deklaraci schopnosti:

1. Načte hodnotu výčtu int32 z blobu CustomAttribute
2. Zkontroluje `Enum.IsDefined(typeof(Capability), value)`
3. Pokud hodnota není definovaným členem Capability → **tiše ignorována**
4. Pokud je hodnota definována → pravidla výjimek jsou aplikována
5. **Kontroly nedeklarovatelných schopností jsou VŽDY vynucovány** bez ohledu na deklarované schopnosti

To brání pluginům deklarovat „budoucí" schopnosti, které ještě neexistují.

## Srovnání s 13-CapabilityNetwork

| Aspekt | 13-CapabilityNetwork (pozitivní) | 18-CapabilityDenied (antivzor) |
|--------|---------------------------------|-------------------------------|
| Deklarace | `[PluginCapability(Capability.Network)]` | `[PluginCapability(Capability.Network)]` |
| Používá HttpClient | ✅ Vyjmuto | ✅ Vyjmuto |
| Používá DllImport | N/A | ❌ VŽDY zablokováno |
| Používá Unsafe | N/A | ❌ VŽDY zablokováno |
| Výsledek načtení | ✅ NAČTENO | ❌ ZAMÍTNUTO |

## Soubory

- `Plugin.cs` — Demo plugin antivzoru nedeklarovatelné schopnosti
- `README.md` — Tento soubor (Angličtina)
- `README.zh-CN.md` — Zjednodušená čínština
- Překlady: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Související příklady

- **13-CapabilityNetwork**: Pozitivní příklad Capability.Network
- **11-ForbiddenPInvoke**: Antivzor P/Invoke (žádná schopnost nepomůže)
- **10-ForbiddenReflection**: Antivzor Reflection (žádná schopnost nepomůže)
