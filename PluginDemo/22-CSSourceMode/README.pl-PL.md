# Demo Trybu Ładowania przez Kompilację Kodu Źródłowego CS

Wtyczka ładowana z surowych plików źródłowych `.cs` zamiast prekompilowanej biblioteki DLL, demonstrowanie trybu kompilacji kodu źródłowego CS w PluginLoader (wprowadzony przez task-389).

## Jak Działa Tryb Kodu Źródłowego CS

Gdy PluginLoader skanuje katalog wtyczki i **nie znajduje biblioteki DLL**, automatycznie przechodzi do trybu kodu źródłowego CS:

```
1. PluginLoader skanuje katalog wtyczki → brak DLL
2. Przechodzi do trybu kodu źródłowego CS
3. Znaleziono cs.txt → odczyt wiersz po wierszu, ładowanie tylko wymienionych plików .cs
   (Brak cs.txt → ładowanie wszystkich plików *.cs w katalogu)
4. Skanowanie bibliotek DLL równorzędnych → zaufane DLL dodawane bezpośrednio jako odwołania;
   niezaufane DLL muszą przejść ScanForbiddenReferences
5. CompilationCore (tryb ograniczony) kompiluje pliki .cs do biblioteki DLL w pamięci
6. Bajty biblioteki DLL w pamięci są zapisywane do pliku tymczasowego w celu skanowania ScanForbiddenReferences
7. Skanowanie zakończone pomyślnie → odbicie znajduje implementację IPlugin → instancjonowanie
8. Dziennik pokazuje: "Plugin loaded [CS-Source]: {Id} v{Version} from {DirName}"
```

## cs.txt — Biała Lista Selektywnego Ładowania

Plik `cs.txt` określa, które pliki `.cs` skompilować, po jednej nazwie pliku w wierszu:

```
Plugin.cs
```

- **Wymienione pliki**: Kompilowane i ładowane (np.: `Plugin.cs`)
- **Niewymienione pliki**: Ignorowane przez kompilator (np.: `Helpers.cs`)
- **Wiersze zaczynające się od `#`**: Traktowane jako komentarze
- **Puste wiersze**: Ignorowane
- **Brak cs.txt**: Wszystkie pliki `*.cs` w katalogu są ładowane

## Tryb Kodu Źródłowego CS vs Tryb DLL

| Aspekt | Tryb DLL | Tryb Kodu Źródłowego CS |
|--------|----------|------------------------|
| Format wtyczki | Prekompilowana biblioteka `.dll` | Surowe pliki źródłowe `.cs` |
| Wyzwalacz ładowania | Biblioteka DLL znaleziona w katalogu | Brak DLL, pliki `.cs` obecne |
| Kompilacja | W czasie budowania | W czasie ładowania przez PluginLoader |
| Wydajność | Brak narzutu kompilacji | Narzut kompilacji Roslyn przy uruchamianiu |
| Skanowanie bezpieczeństwa | Bezpośrednie skanowanie metadanych PE | Kompilacja → tymczasowa DLL → skanowanie metadanych PE |
| Prefiks dziennika | `Plugin loaded:` | `Plugin loaded [CS-Source]:` |
| Najlepsze dla | Wdrożenie produkcyjne | Iteracja deweloperska |

## Obsługa Błędów

| Scenariusz | Zachowanie |
|-----------|------------|
| Brak DLL, brak plików .cs | Ostrzeżenie: "No DLL and no CS source files found" |
| Błędy kompilacji | Błąd: Szczegółowe komunikaty diagnostyczne zarejestrowane |
| Niepowodzenie skanowania bezpieczeństwa | Błąd: Wszystkie naruszenia wymienione, wtyczka odrzucona |
| Wpis cs.txt nie znaleziony | Ostrzeżenie: "cs.txt entry not found or not a .cs file" |
| Niepowodzenie skanowania biblioteki DLL równorzędnej | Ostrzeżenie: DLL nie dodana jako odwołanie, kompilacja kontynuowana |

## Uwaga dotycząca Bezpieczeństwa

Wtyczki w trybie kodu źródłowego CS podlegają **temu samemu skanowaniu bezpieczeństwa** co wtyczki w trybie DLL. Skompilowany zestaw jest zapisywany do tymczasowego pliku DLL i skanowany przy użyciu `ScanForbiddenReferences` — to samo skanowanie, które przechodzą prekompilowane biblioteki DLL. Wszystkie reguły dotyczące zabronionych przestrzeni nazw/typów/elementów/ciągów mają zastosowanie w taki sam sposób.

Wtyczki są nadal ładowane w odizolowanym kontekście i skanowane pod kątem zabronionych odwołań do przestrzeni nazw (np.: `System.IO`, `System.Net.Http`). Zobacz [Dokumentację Bezpieczeństwa](../../docs/pl-PL/security.md) po szczegóły.
