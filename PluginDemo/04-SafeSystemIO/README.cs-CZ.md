# Demo bezpečného System.IO

Demonstruje typy System.IO na bílé listině `SystemIOAllowedTypes`: `MemoryStream`, `BinaryReader`/`BinaryWriter`, `GZipStream`. Vysvětluje, proč `FileStream` vyžaduje `PermissionedStreamFactory`.

## Bílá listina SystemIOAllowedTypes

Runtime pluginů ve výchozím nastavení blokuje jmenný prostor `System.IO`, ale osvobozuje typy, které **neprovádějí přímý souborový I/O**:

| Kategorie | Povolené typy | Proč bezpečné |
|-----------|--------------|--------------|
| Abstrakce streamů | `Stream` | Abstraktní základní třída, žádné vlastní I/O |
| Streamy v paměti | `MemoryStream` | Čistě paměťová operace |
| Kompresní streamy | `GZipStream`, `DeflateStream`, `ZLibStream` | Obalují jiný stream, neotevírají soubory |
| Binární wrappery | `BinaryReader`, `BinaryWriter` | Obalují libovolný stream, neotevírají soubory |
| Výčty | `SeekOrigin`, `FileMode`, `FileAccess`, `FileShare`, `CompressionMode`, `CompressionLevel` | Pouze typy hodnot |
| Výjimky | `IOException`, `InvalidDataException`, `EndOfStreamException` | Pouze typy chyb |

### Typy mimo bílou listinu

Tyto typy **přímo přistupují k souborovému systému** a jsou **blokovány** v kódu pluginů:

| Blokovaný typ | Důvod | Bezpečná alternativa |
|--------------|-------|---------------------|
| `FileStream` | Přímo otevírá soubory | `PermissionedStreamFactory.CreateReadStream()` / `CreateWriteStream()` |
| `File` | Statické souborové operace | `PermissionedStreamFactory` + `SafePath` |
| `Directory` | Statické adresářové operace | `SafePath` (kontrola oprávnění) |
| `FileInfo` | Zabaluje cesty k souborům | `SafePath` |
| `DirectoryInfo` | Zabaluje cesty k adresářům | `SafePath` |
| `StreamReader` | Přímo otevírá soubory | `PermissionedStreamFactory` + zabalit `PermissionedStream` |
| `StreamWriter` | Přímo otevírá soubory | `PermissionedStreamFactory` + zabalit `PermissionedStream` |

## Proč FileStream vyžaduje PermissionedStreamFactory

`FileStream` přímo otevírá soubory na disku — vážné bezpečnostní riziko v pluginovém systému. `PermissionedStreamFactory` vynucuje:

1. **Kontrolu oprávnění** — `PermissionManager` volajícího musí udělit `FileAccess` pro cestu
2. **Auditní záznam** — každé otevření souboru je zaznamenáno s ID being volajícího
3. **Validaci cesty** — prázdné/neplatné cesty jsou odmítnuty před jakýmkoliv I/O

```
❌ new FileStream("path", FileMode.Open)           → Blokováno skenerem TypeRef
✅ PermissionedStreamFactory.CreateReadStream(id, "path")  → Kontrola oprávnění úspěšná
✅ PermissionedStreamFactory.CreateWriteStream(id, "path") → Kontrola oprávnění úspěšná
```

## Demo pipeline

Toto demo buduje kompletní datový pipeline v paměti pomocí pouze typů z bílé listiny:

```
┌─────────────────────────────────────────────────────────────────┐
│  Demo 1: MemoryStream                                           │
│  └─ Zapsat bajty → Číst bajty → Dekódovat řetězec              │
│                                                                  │
│  Demo 2: Kompresní pipeline                                     │
│  └─ string → UTF8 → MemoryStream                                │
│     → GZipStream(komprimovat) → MemoryStream(komprimované)      │
│     → GZipStream(dekomprimovat) → MemoryStream(surová data)     │
│     → UTF8 → string (zpětný převod)                             │
│                                                                  │
│  Demo 3: BinaryReader/Writer                                     │
│  └─ Write(int, double, string) → MemoryStream                   │
│     → Read(int, double, string) → Ověřit zpětný převod         │
└─────────────────────────────────────────────────────────────────┘
```

## Toto demo

> **⚠️ Poznámka:** Toto demo používá **pouze** typy z bílé listiny `SystemIOAllowedTypes`. Neprovádí se žádný souborový I/O. Pro přístup k souborům viz API `PermissionedStreamFactory`.

| Třída | Role |
|-------|------|
| `SafeSystemIOPlugin` | Implementace `IPlugin` — demonstruje bezpečné použití System.IO |

## Bezpečnostní poznámka

Jmenný prostor `System.IO` je blokován skenerem TypeRef pluginů. Procházejí pouze typy z bílé listiny. Pro skutečný přístup k souborům je nutné použít `PermissionedStreamFactory`, který provádí kontrolu oprávnění a auditní záznamy. Viz [dokumentace zabezpečení](../../docs/cs-CZ/security.md).
