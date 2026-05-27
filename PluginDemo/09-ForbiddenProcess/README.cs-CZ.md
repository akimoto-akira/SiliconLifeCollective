# PluginDemo-09: Anti-vzor zakázaných procesových operací

## Přehled

Tento plugin demonstruje **zakázané** operace spouštění procesů v systému pluginů SiliconLife. Slouží jako reference anti-vzoru, ukazuje co NEDĚLAT a poskytuje správné alternativy pro každé porušení.

## Proč jsou typy Process zakázány?

`System.Diagnostics.Process` a `ProcessStartInfo` jsou v pluginech blokovány, protože přímé spouštění procesů představuje vážná bezpečnostní rizika:

1. **Libovolné spouštění příkazů**: Pluginy mohou spouštět jakýkoliv příkaz bez auditu nebo kontroly oprávnění
2. **Spuštění malwaru**: Škodlivé pluginy mohou spouštět nežádoucí aplikace nebo skripty
3. **Přístup k systémovým prostředkům**: Procesy mohou přistupovat k citlivým prostředkům mimo sandbox pluginu
4. **Žádná validace příkazů**: Přímý Process.Start nemá vestavěnou ochranu proti vkládání příkazů
5. **Žádná auditní stopa**: Přímé procesové operace obcházejí systém bezpečnostního auditu
6. **Eskalace oprávnění**: Možnost vytvoření procesů s vyššími oprávněními než má plugin

## Které typy jsou zakázány?

Zakázány jsou pouze typy související s Process, **NE celý jmenný prostor System.Diagnostics**:

| Zakázaný typ | Blokovaná metoda | Úroveň rizika |
|-------------|-----------------|---------------|
| `Process` | `Start()`, `Kill()`, `WaitForExit()` | 🔴 Kritické |
| `ProcessStartInfo` | Konstruktor, všechny vlastnosti | 🔴 Kritické |
| `Process` | `StandardInput`, `StandardOutput`, `StandardError` | 🔴 Kritické |
| `Process` | `GetProcesses()`, `GetProcessesByName()` | 🟡 Vysoké |

## Které typy jsou povoleny?

Ostatní typy `System.Diagnostics`, které nezahrnují spouštění procesů, zůstávají dostupné:

| Povolený typ | Použití | Proč bezpečný |
|-------------|---------|--------------|
| `Stopwatch` | Měření času | Žádné spouštění procesů |
| `Debug` | Ladící výstup | Žádné bezpečnostní riziko |
| `Trace` | Trasování/logování | Žádné bezpečnostní riziko |
| `PerformanceCounter` | Monitorování výkonu | Pouze čtení, auditováno |

## Jak bezpečně spouštět příkazy?

### Použití CommandLineExecutor (jediný bezpečný způsob)

`CommandLineExecutor` je **řízený vstupní bod** pro spouštění příkazů v pluginech:

```csharp
// ✅ SPRÁVNĚ: Spuštění příkazu
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);

if (result.Success)
{
    Console.WriteLine(result.Output);
}
else
{
    Console.WriteLine($"Chyba: {result.Error}");
}
```

**Co CommandLineExecutor poskytuje:**
1. **Ochrana proti vkládání příkazů**: Blokuje nebezpečné separátory (`||`, `&&`, `|`, `&`, `;`)
2. **Vynucení časového limitu**: Výchozí limit 30 sekund (konfigurovatelný)
3. **Auditní protokol**: Všechna spuštění příkazů jsou zaznamenána pro bezpečnostní kontrolu
4. **Zachytávání výstupu**: Automatické zachytávání stdout a stderr
5. **Multiplatformní podpora**: Používá `cmd.exe` ve Windows, `/bin/bash` v Unixu
6. **Zpracování chyb**: Vrací strukturovaný výsledek se stavem úspěch/selhání

## Demonstrovaná porušení

Tento plugin ukazuje 5 běžných porušení spouštění procesů:

### Porušení 1: Process.Start

```csharp
// ❌ ZAKÁZÁNO
Process.Start("notepad.exe");

// ✅ SPRÁVNĚ
var request = new ExecutorRequest { ResourcePath = "notepad.exe" };
var result = CommandLineExecutor.Execute(request);
```

**Blokovaný TypeRef**: `System.Diagnostics.Process::Start(System.String)`

### Porušení 2: ProcessStartInfo

```csharp
// ❌ ZAKÁZÁNO
var psi = new ProcessStartInfo {
    FileName = "cmd.exe",
    Arguments = "/c dir",
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = new Process { StartInfo = psi };
process.Start();

// ✅ SPRÁVNĚ
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);
Console.WriteLine(result.Output);
```

**Blokovaný TypeRef**: `System.Diagnostics.ProcessStartInfo::.ctor()`

### Porušení 3: Process s argumenty

```csharp
// ❌ ZAKÁZÁNO
var psi = new ProcessStartInfo("ping", "127.0.0.1 -n 4") {
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = Process.Start(psi);
process.WaitForExit();

// ✅ SPRÁVNĚ
var request = new ExecutorRequest { ResourcePath = "ping 127.0.0.1 -n 4" };
var result = CommandLineExecutor.Execute(request, TimeSpan.FromSeconds(10));
Console.WriteLine(result.Output);
```

**Blokovaný TypeRef**: `System.Diagnostics.Process::Start(ProcessStartInfo)`

### Porušení 4: Přesměrování výstupu procesu

```csharp
// ❌ ZAKÁZÁNO
var psi = new ProcessStartInfo("ipconfig") {
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true
};
using var process = Process.Start(psi);
string output = process.StandardOutput.ReadToEnd();
string error = process.StandardError.ReadToEnd();

// ✅ SPRÁVNĚ
var request = new ExecutorRequest { ResourcePath = "ipconfig" };
var result = CommandLineExecutor.Execute(request);
if (result.Success) Console.WriteLine(result.Output);
else Console.WriteLine(result.Error);
```

**Blokovaný TypeRef**: `System.Diagnostics.Process::StandardOutput`

### Porušení 5: Process.Kill

```csharp
// ❌ ZAKÁZÁNO
Process[] processes = Process.GetProcessesByName("notepad");
foreach (var p in processes) p.Kill();

// ✅ SPRÁVNĚ
// Z bezpečnostních důvodů CommandLineExecutor nepodporuje ukončování procesů.
// V případě potřeby kontaktujte správce systému.
```

**Blokovaný TypeRef**: `System.Diagnostics.Process::Kill()`

## Proč jen Process a ne celý System.Diagnostics?

Systém pluginů používá **chirurgický přístup** k bezpečnosti:

- **Blokovat pouze nebezpečné typy**: Process/ProcessStartInfo umožňují spouštění libovolného kódu
- **Povolit bezpečné typy**: Stopwatch, Debug, Trace nemají bezpečnostní důsledky
- **Minimalizovat dopad**: Vývojáři mohou nadále používat diagnostické nástroje bez rizika
- **Jasná hranice**: Zakázány jsou pouze typy schopné vytvářet/ukončovat procesy

## Bezpečnostní mechanismus PluginLoader

Když PluginLoader skenuje tento plugin:

1. **Skenování TypeRef**: Detekuje reference na zakázané typy `Process`/`ProcessStartInfo`
2. **Skenování MemberRef**: Detekuje volání blokovaných metod (např. `Process.Start`)
3. **Skenování IL String**: Detekuje pokusy o reflexi založenou na řetězcích
4. **Odmítnutí**: Plugin je odmítnut při načítání s podrobnou chybovou zprávou

## Nejlepší postupy

1. **Vždy používat CommandLineExecutor**: Nikdy nepoužívat `Process.Start` přímo
2. **Nastavit rozumné časové limity**: Zabránit nekonečnému zaseknutí příkazů
3. **Kontrolovat výsledky**: Vždy ověřit `result.Success` před použitím výstupu
4. **Sanitizovat vstupy**: Nikdy nepředávat uživatelský vstup přímo příkazům
5. **Deklarovat Capability pokud nutné**: Pokud je skutečně potřeba neomezené spouštění procesů, deklarovat `Capability.Process` (viz 15-CapabilityProcess)

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

- **08-ForbiddenNetwork**: Zakázané síťové operace
- **15-CapabilityProcess**: Deklarativní oprávnění Process
- **10-ForbiddenReflection**: Zakázané operace reflexe
- **12-ForbiddenStringBypass**: Pokusy o obejití reflexí založenou na řetězcích
