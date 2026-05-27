# PluginDemo-09: Antywzorzec zabronionych operacji procesowych

## Przegląd

Ten plugin demonstruje **zabronione** operacje wykonywania procesów w systemie pluginów SiliconLife. Służy jako referencja antywzorca, pokazując czego NIE robić i zapewniając poprawne alternatywy dla każdego naruszenia.

## Dlaczego typy Process są zabronione?

`System.Diagnostics.Process` i `ProcessStartInfo` są zablokowane w pluginach, ponieważ bezpośrednie wykonywanie procesów stwarza poważne zagrożenia bezpieczeństwa:

1. **Dowolne wykonywanie poleceń**: Pluginy mogą wykonywać dowolne polecenia bez audytu lub sprawdzania uprawnień
2. **Uruchamianie złośliwego oprogramowania**: Złośliwe pluginy mogą uruchamiać niechciane aplikacje lub skrypty
3. **Dostęp do zasobów systemowych**: Procesy mogą uzyskać dostęp do wrażliwych zasobów poza piaskownicą pluginu
4. **Brak walidacji poleceń**: Bezpośredni Process.Start nie ma wbudowanej ochrony przed wstrzykiwaniem poleceń
5. **Brak śladu audytu**: Bezpośrednie operacje procesowe omijają system audytu bezpieczeństwa
6. **Eskalacja uprawnień**: Możliwość tworzenia procesów z wyższymi uprawnieniami niż plugin

## Które typy są zabronione?

Zabronione są tylko typy związane z Process, **NIE cała przestrzeń nazw System.Diagnostics**:

| Zabroniony typ | Zablokowana metoda | Poziom ryzyka |
|---------------|-------------------|--------------|
| `Process` | `Start()`, `Kill()`, `WaitForExit()` | 🔴 Krytyczny |
| `ProcessStartInfo` | Konstruktor, wszystkie właściwości | 🔴 Krytyczny |
| `Process` | `StandardInput`, `StandardOutput`, `StandardError` | 🔴 Krytyczny |
| `Process` | `GetProcesses()`, `GetProcessesByName()` | 🟡 Wysoki |

## Które typy są dozwolone?

Inne typy `System.Diagnostics`, które nie obejmują wykonywania procesów, pozostają dostępne:

| Dozwolony typ | Zastosowanie | Dlaczego bezpieczny |
|--------------|-------------|-------------------|
| `Stopwatch` | Pomiar czasu | Brak wykonywania procesów |
| `Debug` | Wyjście debugowania | Brak ryzyka bezpieczeństwa |
| `Trace` | Śledzenie/logowanie | Brak ryzyka bezpieczeństwa |
| `PerformanceCounter` | Monitorowanie wydajności | Tylko odczyt, audytowane |

## Jak bezpiecznie wykonywać polecenia?

### Użyj CommandLineExecutor (jedyny bezpieczny sposób)

`CommandLineExecutor` jest **kontrolowanym punktem wejścia** do wykonywania poleceń w pluginach:

```csharp
// ✅ POPRAWNIE: Wykonanie polecenia
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);

if (result.Success)
{
    Console.WriteLine(result.Output);
}
else
{
    Console.WriteLine($"Błąd: {result.Error}");
}
```

**Co zapewnia CommandLineExecutor:**
1. **Ochrona przed wstrzykiwaniem poleceń**: Blokuje niebezpieczne separatory (`||`, `&&`, `|`, `&`, `;`)
2. **Wymuszanie limitu czasu**: Domyślny limit 30 sekund (konfigurowalny)
3. **Dziennik audytu**: Wszystkie wykonania poleceń są rejestrowane do przeglądu bezpieczeństwa
4. **Przechwytywanie wyjścia**: Automatyczne przechwytywanie stdout i stderr
5. **Wsparcie wieloplatformowe**: Używa `cmd.exe` w Windows, `/bin/bash` w Unix
6. **Obsługa błędów**: Zwraca ustrukturyzowany wynik ze statusem sukcesu/porażki

## Demonstrowane naruszenia

Ten plugin pokazuje 5 typowych naruszeń wykonywania procesów:

### Naruszenie 1: Process.Start

```csharp
// ❌ ZABRONIONE
Process.Start("notepad.exe");

// ✅ POPRAWNIE
var request = new ExecutorRequest { ResourcePath = "notepad.exe" };
var result = CommandLineExecutor.Execute(request);
```

**Zablokowany TypeRef**: `System.Diagnostics.Process::Start(System.String)`

### Naruszenie 2: ProcessStartInfo

```csharp
// ❌ ZABRONIONE
var psi = new ProcessStartInfo {
    FileName = "cmd.exe",
    Arguments = "/c dir",
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = new Process { StartInfo = psi };
process.Start();

// ✅ POPRAWNIE
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);
Console.WriteLine(result.Output);
```

**Zablokowany TypeRef**: `System.Diagnostics.ProcessStartInfo::.ctor()`

### Naruszenie 3: Process z argumentami

```csharp
// ❌ ZABRONIONE
var psi = new ProcessStartInfo("ping", "127.0.0.1 -n 4") {
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = Process.Start(psi);
process.WaitForExit();

// ✅ POPRAWNIE
var request = new ExecutorRequest { ResourcePath = "ping 127.0.0.1 -n 4" };
var result = CommandLineExecutor.Execute(request, TimeSpan.FromSeconds(10));
Console.WriteLine(result.Output);
```

**Zablokowany TypeRef**: `System.Diagnostics.Process::Start(ProcessStartInfo)`

### Naruszenie 4: Przekierowanie wyjścia procesu

```csharp
// ❌ ZABRONIONE
var psi = new ProcessStartInfo("ipconfig") {
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true
};
using var process = Process.Start(psi);
string output = process.StandardOutput.ReadToEnd();
string error = process.StandardError.ReadToEnd();

// ✅ POPRAWNIE
var request = new ExecutorRequest { ResourcePath = "ipconfig" };
var result = CommandLineExecutor.Execute(request);
if (result.Success) Console.WriteLine(result.Output);
else Console.WriteLine(result.Error);
```

**Zablokowany TypeRef**: `System.Diagnostics.Process::StandardOutput`

### Naruszenie 5: Process.Kill

```csharp
// ❌ ZABRONIONE
Process[] processes = Process.GetProcessesByName("notepad");
foreach (var p in processes) p.Kill();

// ✅ POPRAWNIE
// Ze względów bezpieczeństwa CommandLineExecutor nie obsługuje kończenia procesów.
// W razie potrzeby skontaktuj się z administratorem systemu.
```

**Zablokowany TypeRef**: `System.Diagnostics.Process::Kill()`

## Dlaczego tylko Process, a nie cały System.Diagnostics?

System pluginów stosuje **chirurgiczne podejście** do bezpieczeństwa:

- **Blokuj tylko niebezpieczne typy**: Process/ProcessStartInfo umożliwiają dowolne wykonywanie kodu
- **Zezwalaj na bezpieczne typy**: Stopwatch, Debug, Trace nie mają implikacji bezpieczeństwa
- **Minimalizuj wpływ**: Programiści mogą nadal korzystać z narzędzi diagnostycznych bez ryzyka
- **Jasna granica**: Zabronione są tylko typy mogące tworzyć/kończyć procesy

## Mechanizm bezpieczeństwa PluginLoader

Gdy PluginLoader skanuje ten plugin:

1. **Skanowanie TypeRef**: Wykrywa referencje do zabronionych typów `Process`/`ProcessStartInfo`
2. **Skanowanie MemberRef**: Wykrywa wywołania zablokowanych metod (np. `Process.Start`)
3. **Skanowanie IL String**: Wykrywa próby refleksji bazującej na ciągach znaków
4. **Odrzucenie**: Plugin jest odrzucany podczas ładowania ze szczegółowym komunikatem błędu

## Najlepsze praktyki

1. **Zawsze używaj CommandLineExecutor**: Nigdy nie używaj `Process.Start` bezpośrednio
2. **Ustaw rozsądne limity czasu**: Zapobiegaj nieskończonemu zawieszaniu poleceń
3. **Sprawdzaj wyniki**: Zawsze weryfikuj `result.Success` przed użyciem wyjścia
4. **Sanityzuj dane wejściowe**: Nigdy nie przekazuj danych użytkownika bezpośrednio do poleceń
5. **Deklaruj Capability jeśli konieczne**: Jeśli naprawdę potrzebujesz nieograniczonego wykonywania procesów, zadeklaruj `Capability.Process` (patrz 15-CapabilityProcess)

## Pliki

- `Plugin.cs` - Plugin demonstracyjny antywzorca
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

- **08-ForbiddenNetwork**: Zabronione operacje sieciowe
- **15-CapabilityProcess**: Deklaratywne uprawnienie Process
- **10-ForbiddenReflection**: Zabronione operacje refleksji
- **12-ForbiddenStringBypass**: Próby obejścia przez refleksję bazującą na ciągach znaków
