# Demo důvěryhodné závislosti

Demonstruje použití `Newtonsoft.Json` — knihovny, která interně intenzivně využívá reflexi — jako důvěryhodného sestavení. Bezpečnostní skener PluginLoader důvěryhodná sestavení zcela přeskakuje a umožňuje pluginům na ně odkazovat bez vyvolání porušení.

## Mechanismus whitelistu TrustedAssemblies

`PluginLoader` udržuje statický whitelist open-source knihoven, které jsou **standardně důvěryhodné**:

```csharp
private static readonly HashSet<string> TrustedAssemblies = new(StringComparer.Ordinal)
{
    // Serializace
    "Google.Protobuf",
    "protobuf-net",
    "Newtonsoft.Json",        // ← Toto demo používá tuto knihovnu
    "MessagePack",
    "YamlDotNet",

    // Logování
    "Serilog", "NLog",

    // Microsoft.Extensions.*
    "Microsoft.Extensions.Logging.Abstractions",
    "Microsoft.Extensions.DependencyInjection.Abstractions",
    // ...

    // Přístup k datům / mapování
    "Dapper", "AutoMapper",

    // Validace a distribuce zpráv
    "FluentValidation", "MediatR",
};
```

### Kritéria přijetí

Knihovna může být přidána do `TrustedAssemblies`, pokud splňuje **všechna tři** kritéria:

| # | Kritérium | Důvod |
|---|-----------|-------|
| 1 | Široce používaný open-source projekt (MIT / Apache 2.0 / BSD) | Veřejně auditovatelný kód |
| 2 | Veřejně přístupný zdrojový kód | Komunitní dohled zajišťuje absenci škodlivého chování |
| 3 | Udržovaný NuGet balíček od důvěryhodného dodavatele/komunity | Integrita dodavatelského řetězce |

### Základ identifikace

Skener identifikuje důvěryhodná sestavení podle `AssemblyDefinition.Name` v PE metadatech — **nikoli podle názvu DLL souboru**. To zabraňuje útočníkům přejmenovat škodlivou DLL na `Newtonsoft.Json.dll` k obejití kontrol.

## CollectTrustedTypeRefs — tranzitivní výjimka

Když PluginLoader načítá adresář pluginu, provádí dvoufázové skenování:

```
Fáze 1: CollectTrustedTypeRefs(pluginDir)
├── Výčet všech *.dll souborů v adresáři pluginu
├── Pro každou DLL: čtení PE metadat → kontrola AssemblyDefinition.Name
├── Pokud název ∈ TrustedAssemblies:
│   └── Sběr VŠECH TypeReference záznamů → páry (namespace, typeName)
└── Vrací: HashSet<(string Namespace, string Name)>

Fáze 2: ScanForbiddenReferences(pluginMainDll, trustedTypeRefs)
├── Vrstva 0:   Rychlé ukončení whitelistem (pokud hlavní DLL je důvěryhodná → projde)
├── Vrstva 0.5: Tranzitivní výjimka (přeskočení TypeRefs v sadě trustedTypeRefs)
├── Vrstva 1:   Sken tabulky TypeRef
├── Vrstva 2:   Sken tabulky ExportedType
├── Vrstva 3:   Sken tabulky MemberRef (nebezpečné metody)
├── Vrstva 4:   Značky nebezpečného kódu + P/Invoke
└── Vrstva 5:   Sken haldy #US řetězců
```

### Proč je tranzitivní výjimka důležitá

Newtonsoft.Json interně odkazuje na typy jako `System.Reflection.MemberInfo`, `System.IO.TextReader` atd. Když váš plugin odkazuje na Newtonsoft.Json, kompilátor může tyto tranzitivní TypeRef vložit do **vaší** DLL pluginu. Bez tranzitivní výjimky by byl váš plugin označen za porušení kvůli odkazu na `System.IO.TextReader` — i když jej přímo nikdy nepoužíváte.

`CollectTrustedTypeRefs` řeší tento problém předem sbíráním všech TypeRef z důvěryhodných DLL a jejich označením jako „známé bezpečné" během hlavního skenu.

## Jak přidat novou důvěryhodnou závislost

Pro přidání nové knihovny na whitelist:

1. Ověřte, že splňuje výše uvedená tři kritéria přijetí
2. Přidejte řádek do `TrustedAssemblies` HashSet v `PluginLoader.cs`:
   ```csharp
   "YourLibraryName",  // Stručný popis proč je důvěryhodná
   ```
3. Umístěte DLL knihovny do adresáře pluginu (vedle hlavní DLL pluginu)
4. Skener automaticky shromáždí její TypeRef a udělí výjimku

> **⚠️ Důležité:** Přidání knihovny do `TrustedAssemblies` znamená, že skener **nebude** kontrolovat její interní kód. Přidávejte pouze knihovny, kterým plně důvěřujete.

## Toto demo

Tento plugin používá Newtonsoft.Json bez jakékoli deklarace `PluginCapability`:

| Funkce | Interní chování Newtonsoft.Json | Proč to funguje |
|--------|----------------------------------|----------------|
| `JsonConvert.SerializeObject` | Používá reflexi k výčtu vlastností | DLL Newtonsoft.Json prochází whitelistem vrstvy 0 |
| `JsonConvert.DeserializeObject<T>` | Volá `Activator.CreateInstance`, nastavuje vlastnosti reflexí | Tranzitivní TypeRef vyjmuty ve vrstvě 0.5 |
| Manipulace s `JObject` / `JArray` | Používá `System.Linq.Expressions`, dynamické odeslání | Všechny interní odkazy sebrány `CollectTrustedTypeRefs` |

### Klíčový rozdíl od PluginCapability

| Mechanismus | Rozsah | Případ použití |
|-------------|--------|----------------|
| `TrustedAssemblies` | Vyjímá celou **knihovnu** (a její tranzitivní odkazy) ze skenování | Známé open-source závislosti |
| `PluginCapability` | Vyjímá **kód vašeho pluginu** ze zákazů specifických jmenných prostorů | Plugin potřebuje přímý přístup k System.Net/IO/Process |

Plugin používající pouze důvěryhodné závislosti **nepotřebuje** žádnou deklaraci `PluginCapability`. Skener vše zpracuje automaticky.

## Bezpečnostní poznámka

Důvěryhodná sestavení jsou vyjmuta z bezpečnostního skenování, protože jsou auditovatelné open-source projekty. Nicméně **kód vašeho pluginu** je stále plně skenován. Pokud váš plugin přímo odkazuje na `System.IO.File` nebo `System.Net.Http.HttpClient`, bude stále blokován — pokud nedeklarujete příslušnou `PluginCapability`. Viz [bezpečnostní dokumentace](../../docs/cs-CZ/security.md).
