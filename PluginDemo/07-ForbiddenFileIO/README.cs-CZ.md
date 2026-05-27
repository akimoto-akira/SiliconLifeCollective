# Zakázané souborové I/O operace — Anti-vzor

Demonstruje **zakázané** souborové I/O operace v systému pluginů. Tento příklad slouží jako reference anti-vzoru, ukazuje co NEDĚLAT a poskytuje správné alternativy pro každé porušení.

## Proč je System.IO globálně zakázán?

Celý namespace `System.IO` je blokován na úrovni pluginů, protože přímý přístup k souborům představuje vážná bezpečnostní rizika:

1. **Neautorizovaný přístup k souborům**: Pluginy mohou číst citlivé soubory mimo pracovní prostor (hesla, klíče, osobní údaje)
2. **Útoky přepisováním souborů**: Škodlivé pluginy mohou přepsat kritické systémové nebo konfigurační soubory
3. **Procházení adresářů**: Pluginy mohou použít cesty `../` k úniku z hranic pracovního prostoru
4. **Vyčerpání zdrojů**: Nekontrolované vytváření souborů může zaplnit diskový prostor
5. **Žádná auditní stopa**: Přímé souborové operace obcházejí systém bezpečnostního auditu pluginů

## Zakázané typy

Všechny typy `System.IO` přímo přistupující k souborovému systému jsou blokovány:

| Zakázaný typ | Blokovaná metoda | Úroveň rizika |
|-------------|-----------------|----------------|
| `File` | `ReadAllText`, `WriteAllText`, `AppendAllText` atd. | 🔴 Kritický |
| `FileStream` | Konstruktor s cestou k souboru | 🔴 Kritický |
| `Directory` | `GetFiles`, `GetDirectories`, `CreateDirectory` | 🔴 Kritický |
| `StreamReader` | Konstruktor s cestou (řetězec) | 🔴 Kritický |
| `StreamWriter` | Konstruktor s cestou (řetězec) | 🔴 Kritický |
| `FileInfo` | Všechny metody | 🔴 Kritický |
| `DirectoryInfo` | Všechny metody | 🔴 Kritický |

## Povolené typy (výjimky bílé listiny)

Typy provádějící **čistě paměťové operace** (bez přímého přístupu k souborovému systému) jsou povoleny:

| Povolený typ | Použití | Proč je bezpečný |
|-------------|---------|------------------|
| `MemoryStream` | Bajtový proud v paměti | Žádný přístup k souborovému systému |
| `BinaryReader` | Čtení z existujícího proudu | Obaluje proud, neotevírá soubory |
| `BinaryWriter` | Zápis do existujícího proudu | Obaluje proud, nevytváří soubory |
| `GZipStream` | Komprese/dekomprese | Obaluje proud, žádný přístup k souborům |
| `StreamReader` | Konstruktor s parametrem `Stream` | Bezpečný při obalování auditovaných proudů |
| `StreamWriter` | Konstruktor s parametrem `Stream` | Bezpečný při obalování auditovaných proudů |

Viz příklad **04-SafeSystemIO** pro povolené typy.

## Bezpečný přístup k souborům přes PermissionedStreamFactory

`PermissionedStreamFactory` je **řízený vstupní bod** pro souborové operace v pluginech:

```csharp
// ✅ Správně: čtení souboru
using var readStream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(readStream);
string content = reader.ReadToEnd();

// ✅ Správně: zápis souboru
using var writeStream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(writeStream);
writer.Write("Data protokolu");
```

**PermissionedStreamFactory poskytuje:**
1. **Validace cesty**: Zabraňuje útokům procházením adresářů (`../`)
2. **Kontrola oprávnění**: Zajišťuje, že soubor je v povoleném pracovním prostoru
3. **Auditní protokol**: Všechny přístupy k souborům jsou zaznamenávány pro bezpečnostní přezkoumání
4. **Čištění zdrojů**: Sleduje otevřené proudy a zabraňuje únikům

## Porušení v tomto příkladu

### Porušení 1: File.ReadAllText

```csharp
// ❌ Zakázáno — ⚠️ VIOLATION: [TypeRef] System.IO.File::ReadAllText
string content = File.ReadAllText("config.json");

// ✅ Správná alternativa
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
string content = reader.ReadToEnd();
```

### Porušení 2: File.WriteAllText

```csharp
// ❌ Zakázáno — ⚠️ VIOLATION: [TypeRef] System.IO.File::WriteAllText
File.WriteAllText("output.log", "some data");

// ✅ Správná alternativa
using var stream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(stream);
writer.Write("some data");
```

### Porušení 3: Přímý FileStream

```csharp
// ❌ Zakázáno — ⚠️ VIOLATION: [TypeRef] System.IO.FileStream::.ctor
using var fs = new FileStream("data.bin", FileMode.Open);

// ✅ Správná alternativa
using var fs = PermissionedStreamFactory.OpenRead("data.bin");
```

### Porušení 4: Directory.GetFiles

```csharp
// ❌ Zakázáno — ⚠️ VIOLATION: [TypeRef] System.IO.Directory::GetFiles
string[] files = Directory.GetFiles("./logs", "*.txt");

// ✅ Správná alternativa (s použitím SpeedyPack)
using var pack = SpeedyPack.Open("logs.spk");
var entries = pack.ListEntries("/");
```

### Porušení 5: StreamReader s přímou cestou

```csharp
// ❌ Zakázáno — ⚠️ VIOLATION: [TypeRef] System.IO.StreamReader::.ctor(string)
using var reader = new StreamReader("config.json");

// ✅ Správná alternativa
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
```

## Porovnání s dalšími příklady

| Příklad | Zaměření | Vyžadované oprávnění |
|---------|----------|---------------------|
| **04-SafeSystemIO** | Povolené paměťové typy (MemoryStream, GZipStream) | Žádné |
| **07-ForbiddenFileIO** | Zakázané vzory přístupu k souborům (tento příklad) | Nepoužitelné (blokováno) |
| **14-CapabilityFileIO** | Deklarace schopnosti FileIO pro obejití omezení | `Capability.FileIO` |

## Mechanismus bezpečnostního skenování PluginLoader

Když PluginLoader skenuje tento plugin:

1. **Skenování TypeRef**: Detekuje reference na zakázané typy `System.IO`
2. **Skenování MemberRef**: Detekuje volání blokovaných metod
3. **Skenování IL řetězců**: Detekuje pokusy o obejití pomocí řetězcové reflexe
4. **Odmítnutí**: Plugin je odmítnut při načtení s podrobnou chybovou zprávou

Obejití pomocí konkatenace řetězců, reflexe, dynamického načtení nebo obfuskace je nemožné — vše je zachyceno skenováním na úrovni IL (viz **12-ForbiddenStringBypass**).

## Bezpečnostní poznámka

Pokud opravdu potřebujete neomezený přístup k souborům, můžete deklarovat `Capability.FileIO` (viz 14-CapabilityFileIO). Nejlepší postupy jsou však:
- Preferovat **SpeedyPack** pro ukládání strukturovaných dat (bez deklarace oprávnění)
- Používat **PermissionedStreamFactory** když je přístup k souborům nutný (řízený vstupní bod)
- Deklarovat `Capability.FileIO` pouze pokud výše uvedená řešení nestačí
