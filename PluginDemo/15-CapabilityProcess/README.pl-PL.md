# PluginDemo-15: Capability.Process — Deklaratywne uprawnienie procesu

## Przegląd

Ten plugin demonstruje użycie `[PluginCapability(Capability.Process)]` do deklaracji zdolności pluginu do uruchamiania procesów potomnych. Dzięki tej deklaracji plugin uzyskuje dostęp do `System.Diagnostics.Process` i powiązanych typów.

## Składnia deklaracji

```csharp
[PluginCapability(Capability.Process, Reason = "Launch build tools for CI pipeline")]
public class CapabilityProcessPlugin : IPlugin { ... }
```

## Zakres zwolnienia Capability.Process

### Zwolnienia TypeRef

Tylko typy związane z Process pod `System.Diagnostics` są zwolnione:

| Zwolniony typ | Zastosowanie |
|-------------|------------|
| `Process` | Uruchamianie, zarządzanie i monitorowanie procesów potomnych |
| `ProcessStartInfo` | Konfiguracja parametrów uruchamiania procesu |
| `ProcessThread` | Dostęp do informacji o wątkach procesu |
| `ProcessModule` | Dostęp do informacji o modułach procesu |
| `ProcessPriorityClass` | Ustawienie priorytetu procesu |
| `ProcessWindowStyle` | Konfiguracja stylu okna procesu |

Typy zawsze dozwolone (nigdy na liście zakazów): `Stopwatch`, `Debug`, `Trace`, `Activity`

### Zwolnienie ILString

- Łańcuchy zaczynające się od `"System.Diagnostics.Process"` nie są oznaczane

## Porównanie z 09-ForbiddenProcess

| Aspekt | 09-ForbiddenProcess | 15-CapabilityProcess |
|--------|-------------------|---------------------|
| Deklaracja | Brak | `[PluginCapability(Capability.Process)]` |
| Process.Start | ❌ ODRZUCONY | ✅ DOZWOLONY |
| ProcessStartInfo | ❌ ODRZUCONY | ✅ DOZWOLONY |

## Zalecenie: CommandLineExecutor

Nawet z `Capability.Process` zaleca się preferowanie `CommandLineExecutor`:

| Funkcja | CommandLineExecutor | Bezpośredni Process |
|---------|-------------------|-------------------|
| Deklaracja możliwości | Nie wymagana | Wymagana |
| Sandbox | Biała lista poleceń | Brak |
| Limity czasu | Wbudowane | Ręczne |
| Przechwytywanie wyjścia | Strukturalne | Ręczne |
| Logowanie audytowe | Automatyczne | Ręczne |

Używaj `Capability.Process` + bezpośredniego `Process` tylko wtedy, gdy potrzebujesz szczegółowej kontroli nad strumieniami I/O, obsługi zdarzeń procesu lub gdy biała lista poleceń CommandLineExecutor jest zbyt restrykcyjna.

## Najlepsze praktyki bezpieczeństwa

1. **Preferować CommandLineExecutor**: Używać kontrolowanego punktu wejścia, gdy to możliwe
2. **Podać jasną Reason**: "Launch build tools for CI pipeline" zamiast ogólnego "process access"
3. **Walidować wszystkie dane wejściowe**: Nigdy nie przekazywać niezaufanych danych bezpośrednio do ProcessStartInfo
4. **Używać WaitForExit**: Zawsze czekać na zakończenie procesu, aby zapobiegać procesom zombie
5. **Przekierowywać strumienie**: Ustawić `RedirectStandardOutput = true` i `UseShellExecute = false`

## Pliki

- `Plugin.cs` — Plugin demonstracyjny deklarujący Capability.Process
- `README.md` — Ten plik (Angielski)
- `README.zh-CN.md` — Chiński uproszczony
- Tłumaczenia: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Powiązane przykłady

- **09-ForbiddenProcess**: Antywzorzec zablokowanych operacji procesowych
- **18-CapabilityDenied**: Antywzorzec niedeklarowalnych możliwości
