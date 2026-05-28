# Demo Režimu Načítání pomocí Kompilace Zdrojového Kódu CS

Zásuvný modul načítaný ze zdrojových souborů `.cs` namísto předkompilované DLL, demonstrující režim kompilace zdrojového kódu CS v PluginLoader (zavedeno v task-389).

## Jak Funguje Režim Zdrojového Kódu CS

Když PluginLoader prohledá adresář zásuvného modulu a **nenajde DLL**, automaticky přejde do režimu zdrojového kódu CS:

```
1. PluginLoader prohledá adresář → žádná DLL
2. Přejde do režimu zdrojového kódu CS
3. cs.txt nalezen → čtení řádek po řádku, načítání pouze uvedených souborů .cs
   (Žádný cs.txt → načítání všech souborů *.cs v adresáři)
4. Skenování sourozeneckých DLL → důvěryhodné DLL přidány přímo jako reference;
   nedůvěryhodné DLL musí projít ScanForbiddenReferences
5. CompilationCore (omezený režim) zkompiluje soubory .cs do DLL v paměti
6. Bajty DLL v paměti jsou zapsány do dočasného souboru pro skenování ScanForbiddenReferences
7. Skenování úspěšné → reflexe najde implementaci IPlugin → instanciace
8. Záznam zobrazuje: "Plugin loaded [CS-Source]: {Id} v{Version} from {DirName}"
```

## cs.txt — Whitelist Selektrivního Načítání

Soubor `cs.txt` určuje, které soubory `.cs` zkompilovat, jeden název souboru na řádek:

```
Plugin.cs
```

- **Uvedené soubory**: Zkompilovány a načteny (např.: `Plugin.cs`)
- **Neuvedené soubory**: Ignorovány kompilátorem (např.: `Helpers.cs`)
- **Řádky začínající `#`**: Považovány za komentáře
- **Prázdné řádky**: Ignorovány
- **Žádný cs.txt**: Všechny soubory `*.cs` v adresáři jsou načteny

## Režim Zdrojového Kódu CS vs Režim DLL

| Aspekt | Režim DLL | Režim Zdrojového Kódu CS |
|--------|-----------|--------------------------|
| Formát modulu | Předkompilovaná `.dll` | Zdrojové soubory `.cs` |
| Spouštěč načítání | DLL nalezena v adresáři modulu | Žádná DLL, soubory `.cs` přítomny |
| Kompilace | V době sestavení | V době načítání PluginLoaderem |
| Výkon | Žádná režie kompilace | Režie kompilace Roslyn při spuštění |
| Bezpečnostní skenování | Přímé skenování PE metadat | Kompilace → dočasná DLL → skenování PE metadat |
| Předpona záznamu | `Plugin loaded:` | `Plugin loaded [CS-Source]:` |
| Nejlepší pro | Produkční nasazení | Vývojová iterace |

## Zpracování Chyb

| Scénář | Chování |
|--------|---------|
| Žádná DLL, žádné soubory .cs | Varování: "No DLL and no CS source files found" |
| Chyby kompilace | Chyba: Detailní diagnostické zprávy zaznamenány |
| Selhání bezpečnostního skenování | Chyba: Všechna porušení uvedena, modul odmítnut |
| Položka cs.txt nenalezena | Varování: "cs.txt entry not found or not a .cs file" |
| Selhání skenování sourozenecké DLL | Varování: DLL nepřidána jako reference, kompilace pokračuje |

## Bezpečnostní Poznámka

Zásuvné moduly v režimu zdrojového kódu CS podstupují **stejné bezpečnostní skenování** jako moduly v režimu DLL. Zkompilované sestavení je zapsáno do dočasného souboru DLL a skenováno pomocí `ScanForbiddenReferences` — stejného skenování, jakým procházejí předkompilované DLL. Všechna pravidla zakázaných jmenných prostor/typů/členů/řetězců se uplatňují stejně.

Zásuvné moduly jsou nadále načítány v izolovaném kontextu a skenovány na zakázané odkazy na jmenné prostory (např.: `System.IO`, `System.Net.Http`). Viz [Bezpečnostní Dokumentace](../../docs/cs-CZ/security.md) pro podrobnosti.
