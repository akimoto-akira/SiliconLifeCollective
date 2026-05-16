# Przewodnik Istoty Krzemowej

> **Wersja: v0.2.0-alpha**

[English](../en/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md) | [Polski](../pl-PL/silicon-being-guide.md)

## Przegląd

Istoty Krzemowe to napędzane przez AI agenty, które mogą autonomicznie myśleć, działać i ewoluować.

## Architektura

### Separacja ciało-mózg

```
┌─────────────────────────────────────┐
│         Istota Krzemowa             │
├──────────────────┬──────────────────┤
│   Ciało          │   Mózg           │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • Zarządzanie    │ • Ładowanie      │
│   stanem         │   historii       │
│ • Wykrywanie     │ • Wywoływanie AI │
│   wyzwalaczy     │ • Wykonywanie    │
│ • Cykl życia     │   narzędzi       │
│                  │ • Utrwalanie     │
│                  │   odpowiedzi     │
└──────────────────┴──────────────────┘
```

## Plik duszy

### Struktura

```markdown
# Nazwa Istoty

## Osobowość
Opisz cechy osobowości i charakterystykę istoty.

## Zdolności
Wymień, co ta istota potrafi zrobić.

## Wytyczne zachowania
Zdefiniuj, jak istota powinna zachowywać się w różnych sytuacjach.

## Dziedzina wiedzy
Określ obszar specjalizacji istoty.
```

### Przykład

```markdown
# Asystent Przeglądu Kodu

## Osobowość
Jesteś skrupulatnym recenzentem kodu z 10-letnim doświadczeniem.
Dostarczasz konstruktywną informację zwrotną i zawsze tłumaczysz swoje rozumowanie.

## Zdolności
- Przeglądanie kodu pod kątem błędów i najlepszych praktyk
- Sugerowanie optymalizacji wydajności
- Tłumaczenie złożonych algorytmów
- Identyfikacja luk bezpieczeństwa

## Wytyczne zachowania
- Zacznij od pozytywnych obserwacji
- Podaj konkretne przykłady
- Wyjaśnij, dlaczego zmiany są potrzebne
- Bądź pełen szacunku i profesjonalny

## Dziedzina wiedzy
Specjalizacja w C#, .NET i architekturze oprogramowania.
```

## Tworzenie istoty

### Przez Web UI

1. Przejdź do **Zarządzanie istotami**
2. Kliknij **Utwórz nową istotę**
3. Wypełnij:
   - Nazwę
   - Treść duszy
   - Opcje konfiguracji
4. Kliknij **Utwórz**

### Przez API

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistant",
    "soul": "# Personality\nYou are helpful..."
  }'
```

## Cykl życia istoty

### Stany aktywności

Istoty Krzemowe mają następujące stany aktywności:

| Stan | Opis |
|------|------|
| `Idle` | Stan bezczynności, oczekiwanie na wyzwolenie zegara |
| `Working` | Wykonywanie jednej rundy żądania AI + wywołań narzędzi |
| `Error` | Wystąpił błąd podczas wykonywania |
| `Stopped` | Zatrzymana, z powodu kolejnych błędów lub ręcznego zatrzymania |

**Mechanizm stanu Stopped**:
- Gdy Istota Krzemowa napotka 10 kolejnych błędów, automatycznie przechodzi do stanu `Stopped`
- Po wejściu w stan Stopped, istota nie będzie wykonywać żadnych zadań
- Wymagana jest ręczna interwencja, aby ponownie uruchomić

### Przejścia stanów

```
Idle → Working → Idle (normalne zakończenie)
Working → Error → Working (odzyskiwanie po błędzie)
Working → Stopped (10 kolejnych błędów lub ręczne zatrzymanie)
Stopped → Idle (ponowne uruchomienie)
```

### Operacje

- **Uruchomienie**: inicjalizacja i rozpoczęcie przetwarzania
- **Zatrzymanie**: eleganckie zamknięcie
- **Restart**: odzyskanie ze stanu Stopped do stanu Idle

## System zadań

### Tworzenie zadania

```csharp
var task = new BeingTask
{
    BeingId = being.Id,
    Description = "Przegląd kodu",
    Priority = 5,
    DueDate = DateTime.UtcNow.AddHours(2)
};

await taskSystem.CreateAsync(task);
```

### Stany zadań

- `Pending` - Oczekujące na wykonanie
- `Running` - Wykonywane
- `Completed` - Pomyślnie ukończone
- `Failed` - Wykonanie nie powiodło się
- `Cancelled` - Ręcznie anulowane

## System czasomierza

### Typy czasomierzy

1. **Jednorazowy**: wykonanie raz po opóźnieniu
2. **Interwałowy**: powtarzanie w stałych odstępach
3. **Cron**: wykonanie na podstawie wyrażenia cron

### Przykład

```csharp
// Wykonywane co godzinę
var timer = new BeingTimer
{
    BeingId = being.Id,
    Interval = TimeSpan.FromHours(1),
    Action = "think",
    Repeat = true
};

await timerSystem.StartAsync(timer);
```

## System pamięci

### Typy pamięci

- **Krótkoterminowa**: bieżący kontekst rozmowy
- **Długoterminowa**: utrwalona wiedza i doświadczenia
- **Epizodyczna**: zdarzenia i interakcje indeksowane czasowo

### Struktura przechowywania

Wersja Default:
```
data/
└── beings/
    └── {being-id}/
        ├── soul.md
        ├── memory/
        │   ├── short-term.json
        │   └── long-term/
        │       ├── 2026-04-20.json
        │       └── 2026-04-21.json
        └── tasks/
            └── task-history.json
```

Wersja Fast (przechowywanie SpeedyPack):
```
data/
├── speedy/
│   ├── {being-id}.spk       # Plik przechowywania SpeedyPack
│   └── {being-id}.spk.idx   # Plik indeksu
└── beings/
    └── {being-id}/
        └── soul.md
```

## System notatek roboczych

### Przegląd

Notatki robocze to osobisty system dziennika istoty krzemowej, z projektowaniem opartym na stronach, służący do rejestrowania postępu pracy, wniosków z nauki, notatek projektowych itp.

### Funkcje

- **Zarządzanie stronami**: każda notatka jest niezależną stroną, dostępną według numeru strony
- **Obsługa Markdown**: treść obsługuje formatowanie Markdown (tekst, listy, tabele, bloki kodu)
- **Indeks słów kluczowych**: obsługa dodawania słów kluczowych do notatek, ułatwiająca wyszukiwanie
- **Funkcja podsumowania**: każda notatka ma krótkie podsumowanie do szybkiego przeglądu
- **Generowanie spisu treści**: można wygenerować przegląd spisu treści wszystkich notatek, pomagający zrozumieć ogólny kontekst
- **Znaczniki czasu**: automatyczne rejestrowanie czasu utworzenia i aktualizacji
- **Domyślnie prywatne**: dostępne tylko dla samej istoty (kurator może zarządzać)

### Scenariusze użycia

1. **Rejestracja postępu projektu**
   ```
   Podsumowanie: Ukończono moduł uwierzytelniania użytkowników
   Treść: Zaimplementowano weryfikację tokenów JWT, integrację OAuth2, mechanizm odświeżania tokenów
   Słowa kluczowe: uwierzytelnianie,JWT,OAuth2
   ```

2. **Notatki z nauki**
   ```
   Podsumowanie: Nauka najlepszych praktyk programowania asynchronicznego w C#
   Treść: Uwagi dotyczące używania async/await, scenariusze użycia ConfigureAwait...
   Słowa kluczowe: C#,asynchroniczne,najlepsze praktyki
   ```

3. **Podsumowanie spotkań**
   ```
   Podsumowanie: Spotkanie dyskusyjne o wymaganiach produktowych
   Treść: Omówiono wymagania dotyczące nowych funkcji, określono plan implementacji...
   Słowa kluczowe: produkt,wymagania,spotkanie
   ```

### Użycie przez narzędzia

Istota może zarządzać notatkami roboczymi za pomocą narzędzia `work_note`:

```json
// Tworzenie notatki
{
  "action": "create",
  "summary": "Ukończono moduł uwierzytelniania użytkowników",
  "content": "## Szczegóły implementacji\n\n- Użycie tokenów JWT\n- Obsługa OAuth2",
  "keywords": "uwierzytelnianie,JWT,OAuth2"
}

// Odczyt notatki
{
  "action": "read",
  "page_number": 1
}

// Wyszukiwanie notatek
{
  "action": "search",
  "keyword": "uwierzytelnianie",
  "max_results": 10
}
```

### Zarządzanie przez Web UI

1. Przejdź do **Zarządzanie istotami** → wybierz istotę
2. Kliknij zakładkę **Notatki robocze**
3. Możesz przeglądać, wyszukiwać, edytować notatki
4. Obsługa podglądu Markdown

## System sieci wiedzy

### Przegląd

Sieć wiedzy to system reprezentacji i zarządzania wiedzą oparty na strukturze trójek (podmiot-orzeczenie-dopełnienie), służący do przechowywania i zarządzania ustrukturyzowaną wiedzą.

### Podstawowe pojęcia

#### Struktura trójki

```
Podmiot (Subject) --Orzeczenie (Predicate)--> Dopełnienie (Object)
```

**Przykłady**:
- `Python` --`is_a`--> `programming_language`
- `Pekin` --`capital_of`--> `Chiny`
- `Woda` --`boiling_point`--> `100°C`

#### Poziom ufności

Każda trójka wiedzy ma ocenę ufności (0.0-1.0), określającą wiarygodność wiedzy:
- `1.0`: absolutna pewność (np. twierdzenia matematyczne)
- `0.8-0.99`: wysoka wiarygodność (np. zweryfikowane fakty)
- `0.5-0.79`: średnia wiarygodność (np. wnioski lub hipotezy)
- `<0.5`: niska wiarygodność (np. przypuszczenia lub niezweryfikowane informacje)

#### System tagów

Obsługa dodawania tagów do trójek, ułatwiająca kategoryzację i wyszukiwanie:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "tags": ["programming", "language", "popular"]
}
```

### Operacje na wiedzy

#### 1. Dodawanie wiedzy

```json
{
  "action": "add",
  "subject": "C#",
  "predicate": "created_by",
  "object": "Microsoft",
  "confidence": 1.0,
  "tags": ["programming", "language"]
}
```

#### 2. Zapytanie o wiedzę

```json
{
  "action": "query",
  "subject": "C#",
  "predicate": "created_by"
}
```

#### 3. Wyszukiwanie wiedzy

```json
{
  "action": "search",
  "query": "programming language",
  "limit": 10
}
```

#### 4. Odkrywanie ścieżek wiedzy

Znajdowanie ścieżek powiązań między dwoma pojęciami:
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

Zwraca:
```
Python → is_a → programming_language → belongs_to → computer_science
```

#### 5. Walidacja wiedzy

Sprawdzanie ważności i spójności wiedzy:
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. Statystyki wiedzy

Uzyskiwanie ogólnych statystyk sieci wiedzy:
```json
{
  "action": "stats"
}
```

Zwraca:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Scenariusze użycia

1. **Przechowywanie faktów**
   - Przechowywanie obiektywnych faktów i wiedzy powszechnej
   - Przykład: `Ziemia` --`is_a`--> `planeta`

2. **Relacje między pojęciami**
   - Rejestrowanie relacji między pojęciami
   - Przykład: `dziedziczenie` --`is_a`--> `koncepcja_programowania_obiektowego`

3. **Akumulacja wiedzy**
   - Istota nieustannie akumuluje wiedzę przez naukę
   - Tworzenie ustrukturyzowanego systemu wiedzy

4. **Wsparcie wnioskowania**
   - Odkrywanie pośrednich relacji przez ścieżki wiedzy
   - Wsparcie wnioskowania i podejmowania decyzji opartych na wiedzy

### Zarządzanie przez Web UI

1. Przejdź do strony **Sieć wiedzy**
2. Przeglądaj statystyki wiedzy
3. Wyszukuj i przeglądaj wiedzę
4. Wizualizacja grafu relacji wiedzy (planowane)

## Operacje przeglądarki WebView (nowość)

### Przegląd

Istoty Krzemowe mogą autonomicznie przeglądać strony internetowe, pobierać informacje i wykonywać operacje na stronach za pomocą narzędzia przeglądarki WebView. Przeglądarka działa w trybie bezgłowym, całkowicie niewidocznym dla użytkownika.

### Funkcje

- **Izolacja indywidualna**: każda istota posiada niezależną instancję przeglądarki, ciasteczka i sesję
- **Tryb bezgłowy**: autonomiczne operacje w tle, niewidoczne dla użytkownika
- **Pełna funkcjonalność**: obsługa wykonywania JavaScript, renderowania CSS, wypełniania formularzy itp.
- **Kontrola bezpieczeństwa**: wszystkie operacje wymagają przejścia przez łańcuch weryfikacji uprawnień

### Typowe operacje

#### 1. Otwarcie przeglądarki

```json
{
  "action": "open_browser"
}
```

#### 2. Nawigacja do strony

```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

#### 3. Pobranie zawartości strony

```json
{
  "action": "get_page_text"
}
```

Zwraca treść tekstową strony do analizy i zrozumienia przez AI.

#### 4. Kliknięcie elementu

```json
{
  "action": "click",
  "selector": "#submit-button"
}
```

#### 5. Wprowadzenie tekstu

```json
{
  "action": "input",
  "selector": "#search-input",
  "text": "słowo kluczowe wyszukiwania"
}
```

#### 6. Wykonanie JavaScript

```json
{
  "action": "execute_script",
  "script": "return document.title;"
}
```

#### 7. Pobranie zrzutu ekranu

```json
{
  "action": "get_screenshot"
}
```

Zwraca zrzut ekranu strony (kodowanie Base64), może być użyty do analizy wizualnej.

#### 8. Oczekiwanie na pojawienie się elementu

```json
{
  "action": "wait_for_element",
  "selector": ".loading-complete",
  "timeout": 10000
}
```

### Scenariusze użycia

1. **Pozyskiwanie informacji**
   - Przeglądanie stron informacyjnych w celu uzyskania najnowszych wiadomości
   - Wyszukiwanie dokumentacji i materiałów technicznych
   - Monitorowanie zmian treści na stronach

2. **Operacje automatyczne**
   - Wypełnianie i przesyłanie formularzy
   - Klikanie przycisków w celu wyzwolenia operacji
   - Pobieranie danych ze stron

3. **Analiza stron**
   - Analiza struktury i zawartości strony
   - Ekstrakcja określonych informacji
   - Analiza wizualna zrzutów ekranu

### Uwagi

- Operacje przeglądarki mogą być powolne, należy poczekać na załadowanie strony
- Używaj `wait_for_element`, aby upewnić się, że element pojawił się przed operacją
- Przestrzegaj warunków korzystania ze stron i plików robots.txt
- Unikaj częstych żądań, które mogą prowadzić do zablokowania

## Najlepsze praktyki

### Tworzenie pliku duszy

1. **Bądź konkretny**: jasne cechy osobowości i granice
2. **Zdefiniuj zakres**: co istota powinna i czego nie powinna robić
3. **Dołącz przykłady**: pokazuj oczekiwane wzorce zachowania
4. **Aktualizuj regularnie**: ewoluuj duszę na podstawie wyników

### Zarządzanie zadaniami

1. **Ustawiaj priorytety**: używaj priorytetów (1-10)
2. **Definiuj terminy**: zawsze ustawiaj terminy realizacji
3. **Monitoruj postęp**: regularnie sprawdzaj status zadań
4. **Obsługuj niepowodzenia**: implementuj logikę ponownych prób

### Optymalizacja pamięci

1. **Czyść stare dane**: regularnie archiwizuj stare wspomnienia
2. **Indeksuj ważne informacje**: oznaczaj kluczowe informacje
3. **Używaj przechowywania czasowego**: wykorzystuj zapytania z indeksem czasowym

## Rozwiązywanie problemów

### Istota nie może się uruchomić

**Sprawdź**:
- Plik duszy istnieje i jest prawidłowy
- Klient AI jest skonfigurowany
- Zasoby systemowe są wystarczające

### Istota nieoczekiwanie się zatrzymuje

**Sprawdź**:
- Błędy w logach
- Dostępność usługi AI
- Użycie pamięci

### Zadania nie są wykonywane

**Sprawdź**:
- System czasomierza działa
- Priorytety i harmonogram zadań
- Ustawienia uprawnień

## Następne kroki

- 📚 Przeczytaj [przewodnik po architekturze](architecture.md)
- 🛠️ Zobacz [przewodnik programistyczny](development-guide.md)
- 🚀 Zobacz [przewodnik szybkiego startu](getting-started.md)
