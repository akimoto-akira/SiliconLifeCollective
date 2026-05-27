# PluginDemo-15: Capability.Process — Deklarativní oprávnění procesu

## Přehled

Tento plugin demonstruje použití `[PluginCapability(Capability.Process)]` k deklaraci schopnosti pluginu spouštět podřízené procesy. S touto deklarací plugin získá přístup k `System.Diagnostics.Process` a souvisejícím typům.

## Syntaxe deklarace

```csharp
[PluginCapability(Capability.Process, Reason = "Launch build tools for CI pipeline")]
public class CapabilityProcessPlugin : IPlugin { ... }
```

## Rozsah výjimky Capability.Process

### Výjimky TypeRef

Vyjmuty jsou pouze typy související s Process pod `System.Diagnostics`:

| Vyjmutý typ | Použití |
|------------|--------|
| `Process` | Spouštění, správa a monitorování podřízených procesů |
| `ProcessStartInfo` | Konfigurace parametrů spuštění procesu |
| `ProcessThread` | Přístup k informacím o vláknech procesu |
| `ProcessModule` | Přístup k informacím o modulech procesu |
| `ProcessPriorityClass` | Nastavení priority procesu |
| `ProcessWindowStyle` | Konfigurace stylu okna procesu |

Typy vždy povolené (nikdy v seznamu zákazů): `Stopwatch`, `Debug`, `Trace`, `Activity`

### Výjimka ILString

- Řetězce začínající `"System.Diagnostics.Process"` nejsou označovány

## Srovnání s 09-ForbiddenProcess

| Aspekt | 09-ForbiddenProcess | 15-CapabilityProcess |
|--------|-------------------|---------------------|
| Deklarace | Žádná | `[PluginCapability(Capability.Process)]` |
| Process.Start | ❌ ZAMÍTNUTO | ✅ POVOLENO |
| ProcessStartInfo | ❌ ZAMÍTNUTO | ✅ POVOLENO |

## Doporučení: CommandLineExecutor

I s `Capability.Process` se doporučuje preferovat `CommandLineExecutor`:

| Funkce | CommandLineExecutor | Přímý Process |
|--------|-------------------|--------------|
| Deklarace schopnosti | Nepotřebná | Potřebná |
| Sandbox | Povolená lista příkazů | Žádný |
| Časové limity | Vestavěné | Ruční |
| Zachycení výstupu | Strukturované | Ruční |
| Auditní logování | Automatické | Ruční |

Použijte `Capability.Process` + přímý `Process` pouze když potřebujete detailní kontrolu nad I/O streamy, zpracování událostí procesu nebo když povolená lista příkazů CommandLineExecutor je příliš restriktivní.

## Osvědčené bezpečnostní postupy

1. **Preferovat CommandLineExecutor**: Používat řízený vstupní bod, když je to možné
2. **Poskytnout jasnou Reason**: "Launch build tools for CI pipeline" vs vágní "process access"
3. **Validovat všechny vstupy**: Nikdy nepředávat nedůvěryhodné vstupy přímo do ProcessStartInfo
4. **Používat WaitForExit**: Vždy čekat na dokončení procesu pro prevenci zombie procesů
5. **Přesměrovat streamy**: Nastavit `RedirectStandardOutput = true` a `UseShellExecute = false`

## Soubory

- `Plugin.cs` — Demo plugin deklarující Capability.Process
- `README.md` — Tento soubor (Angličtina)
- `README.zh-CN.md` — Zjednodušená čínština
- Překlady: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Související příklady

- **09-ForbiddenProcess**: Antipattern blokovaných procesních operací
- **18-CapabilityDenied**: Antipattern nedeklarovatelných schopností
