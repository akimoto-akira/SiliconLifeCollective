# PluginDemo-14: Capability.FileIO — Deklarativní oprávnění souborového I/O

## Přehled

Tento plugin demonstruje použití `[PluginCapability(Capability.FileIO)]` k deklaraci přímého přístupu k souborovému systému. S touto deklarací plugin získá přístup ke všem typům `System.IO` nad rámec vestavěné bílé listiny `SystemIOAllowedTypes`.

## Syntaxe deklarace PluginCapability

```csharp
[PluginCapability(Capability.FileIO, Reason = "Direct log file access for audit trail")]
public class CapabilityFileIOPlugin : IPlugin { ... }
```

## Jak funguje Capability.FileIO

1. **Výchozí stav**: Namespace `System.IO` je kompletně zakázán; pouze typy z bílé listiny `SystemIOAllowedTypes` jsou povoleny (MemoryStream, BinaryReader, GZipStream atd.)
2. **S deklarací**: Zákaz celého namespace `System.IO` je zrušen — File, FileStream, Directory, StreamReader(string) atd. se stávají přístupnými
3. **Výjimka ILString**: Řetězcové konstanty začínající `"System.IO."` nejsou označovány
4. **Nedeklarovatelná omezení**: P/Invoke, Unsafe, Reflection.Emit atd. zůstávají zablokovány

## Rozsah výjimky Capability.FileIO

### Výjimky TypeRef

Všechny typy `System.IO` jsou vyjmuty:

| Kategorie | Vyjmuté typy |
|-----------|-------------|
| Souborové operace | `File`, `FileInfo` |
| Adresářové operace | `Directory`, `DirectoryInfo` |
| Typy streamů | `FileStream`, `StreamReader(path)`, `StreamWriter(path)` |
| Souborový systém | `FileSystemWatcher`, `DriveInfo`, `Path` |

### Výjimka ILString

- Řetězce začínající `"System.IO."` nejsou označovány

### Co zůstává zakázáno

| Kategorie | Stále blokováno |
|-----------|----------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` |
| Nebezpečný kód | `UnverifiableCodeAttribute`, `Unsafe` |
| IL emise | `System.Reflection.Emit.*` |
| Načítání sestavení | `System.Runtime.Loader`, `Assembly.Load*` |
| Registr | `Microsoft.Win32.*` |

## Srovnání s jinými příklady

| Příklad | Deklarace | Přístup k souborům | Poznámky |
|---------|----------|-------------------|----------|
| **04-SafeSystemIO** | Žádná | MemoryStream, BinaryReader, GZipStream | Používá pouze typy z bílé listiny |
| **07-ForbiddenFileIO** | Žádná | ❌ ZAMÍTNUTO | Příklad antipatternu |
| **14-CapabilityFileIO** | `[PluginCapability(Capability.FileIO)]` | ✅ Plný přístup k System.IO | Tento příklad |
| **20-SpeedyPack** | Žádná | Přes API SpeedyPack (bez Capability) | Doporučené úložiště dat |

## Pořadí priority pro přístup k souborům

1. **SpeedyPack** — Bez deklarace schopnosti. Vestavěné cachování, WAL, transakce. **Doporučeno pro ukládání strukturovaných dat.**
2. **PermissionedStreamFactory** — Bez deklarace. Auditovaný přístup s validací cesty a kontrolou přístupu.
3. **Capability.FileIO + přímý System.IO** — Pouze když výše uvedené možnosti nestačí.

## Proč preferovat PermissionedStreamFactory / SpeedyPack?

I s `Capability.FileIO` se doporučuje používat řízené vstupní body, protože:

1. **Auditní stopa**: Veškerý přístup je zaznamenáván a sledovatelný
2. **Validace cesty**: Zabraňuje útokům directory traversal (`../`)
3. **Řízení přístupu**: Vynucování hranic pracovního prostoru
4. **Sledování zdrojů**: Zabraňuje únikům streamů a vyčerpání zdrojů
5. **Shoda s předpisy**: Řízené vzorce přístupu usnadňují bezpečnostní audity

## Osvědčené bezpečnostní postupy

1. **Deklarovat FileIO pouze pokud je skutečně nutné**: Lze použít SpeedyPack nebo PermissionedStreamFactory?
2. **Poskytnout jasnou Reason**: "Direct log file access for audit trail" je lepší než "file access"
3. **Samostatně validovat cesty**: I s Capability.FileIO validovat všechny cesty k souborům před použitím
4. **Používat using příkazy**: Vždy uvolňovat FileStream/StreamReader/StreamWriter
5. **Princip nejmenších oprávnění**: Deklarovat pouze schopnosti, které plugin skutečně potřebuje

## Soubory

- `Plugin.cs` — Demo plugin deklarující Capability.FileIO
- `README.md` — Tento soubor (Angličtina)
- `README.zh-CN.md` — Zjednodušená čínština
- Překlady: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Související příklady

- **04-SafeSystemIO**: Povolené typy System.IO v paměti (bez deklarace)
- **07-ForbiddenFileIO**: Antipattern blokovaných souborových operací
- **20-SpeedyPack**: Doporučené úložiště dat bez deklarace schopnosti
- **18-CapabilityDenied**: Antipattern nedeklarovatelných schopností
