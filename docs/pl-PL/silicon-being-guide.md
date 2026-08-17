# Przewodnik Istoty Krzemowej

> **Wersja: v0.2.0-alpha**

[English](../en/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | [한국어](../ko-KR/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md) | [Русский](../ru-RU/silicon-being-guide.md)

## Przegląd

Istota Krzemowa to inteligentny agent napędzany przez AI, zdolny do autonomicznego myślenia, działania i ewolucji.

## Architektura

### Separacja Ciało-Mózg

```
┌─────────────────────────────────────┐
│         Istota Krzemowa              │
├──────────────────┬──────────────────┤
│   Ciało           │   Mózg            │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • Zarządzanie stanem │ • Ładowanie historii    │
│ • Wykrywanie wyzwalaczy │ • Wywoływanie AI         │
│ • Cykl życia      │ • Wykonywanie narzędzi        │
│                  │ • Trwałość odpowiedzi      │
└──────────────────┴──────────────────┘
```

## Plik Duszy

### Struktura

```markdown
# Being Name

## Personality
Describe the being's personality traits and characteristics.

## Capabilities
List what this being can do.

## Behavior Guidelines
Define how the being should behave in different situations.

## Knowledge Domain
Specify the being's area of expertise.
```

### Przykład

```markdown
# Code Review Assistant

## Personality
You are a meticulous code reviewer with 10 years of experience.
You provide constructive feedback and always explain your reasoning.

## Capabilities
- Review code for bugs and best practices
- Suggest performance optimizations
- Explain complex algorithms
- Identify security vulnerabilities

## Behavior Guidelines
- Start with positive observations
- Provide specific examples
- Explain why changes are needed
- Be respectful and professional

## Knowledge Domain
Specialized in C#, .NET, and software architecture.
```

## Tworzenie Istoty

### Przez Web UI

1. Przejdź do **Zarządzanie istotami**
2. Kliknij **Utwórz nową istotę**
3. Wypełnij:
   - Nazwa
   - Treść Pliku Duszy
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

## Cykl życia Istoty

### Stany aktywności

Istoty Krzemowe posiadają następujące stany aktywności:

| Stan | Opis |
|------|------|
| `Idle` | Stan bezczynności, oczekiwanie na wyzwalanie zegarem |
| `SingleChat` | Trwa czat jeden na jednego |
| `GroupChat` | Trwa czat grupowy |
| `Task` | Wykonywanie zadania |
| `Timer` | Wykonywanie czasomierza |
| `Broadcast` | Przetwarzanie wiadomości transmisji |
| `Project` | Praca nad projektem |
| `MemoryCompression` | Kompresja pamięci |
| `Stopped` | Zatrzymana, z powodu kolejnych błędów lub ręcznego zatrzymania |

**Mechanizm stanu Stopped**:
- Gdy Istota Krzemowa napotka 10 kolejnych błędów, automatycznie przechodzi w stan `Stopped`
- Po wejściu w stan Stopped, istota nie będzie wykonywać żadnych zadań
- Gdy nadejdzie nowa wiadomość czatu, licznik błędów jest resetowany, a istota wznawia działanie
- Można również zrestartować istotę przez ręczną interwencję

### Przejścia stanów

```
Idle → SingleChat → Idle (czat zakończony)
Idle → GroupChat → Idle (czat grupowy zakończony)
Idle → Task → Idle (zadanie zakończone)
Idle → Timer → Idle (czasomierz zakończony)
Idle → Broadcast → Idle (transmisja przetworzona)
Idle → Project → Idle (praca nad projektem zakończona)
Idle → MemoryCompression → Idle (kompresja pamięci zakończona)
Dowolny → Stopped (10 kolejnych błędów)
Stopped → Idle (nowa wiadomość czatu lub ręczny restart)
```

### Operacje

- **Uruchomienie**: inicjalizacja i rozpoczęcie przetwarzania
- **Zatrzymanie**: eleganckie zamknięcie
- **Restart**: powrót ze stanu Stopped do stanu Idle

## System zadań

### Tworzenie zadania

```csharp
var task = new BeingTask
{
    BeingId = being.Id,
    Description = "Review the code",
    Priority = 5,
    DueDate = DateTime.UtcNow.AddHours(2)
};

await taskSystem.CreateAsync(task);
```

### Statusy zadań

- `Pending` - oczekiwanie na wykonanie
- `Running` - trwa wykonywanie
- `SubmittedForReview` - przesłane do przeglądu
- `UnderReview` - w trakcie przeglądu
- `Rework` - poprawki wymagane
- `Completed` - pomyślnie ukończone
- `Failed` - wykonanie nie powiodło się
- `Cancelled` - ręcznie anulowane

## System czasomierzy

### Typy czasomierzy

1. **Jednorazowy**: wykonanie raz po opóźnieniu
2. **Interwałowy**: powtarzanie w stałych odstępach
3. **Cron**: wykonywanie na podstawie wyrażenia cron

### Przykład

```csharp
// Wykonywanie co godzinę
var timer = new BeingTimer
{
    BeingId = being.Id,
    Interval = TimeSpan.FromHours(1),
    Action = "think",
    Repeat = true
};

await timerSystem.StartAsync(timer);
```

## System Umiejętności

Umiejętność (Skill) to jednostka zdolności wielokrotnego użytku Istoty Krzemowej — enkapsuluje „orkiestrację narzędzi + szablon podpowiedzi" w deklarowalną, ewoluującą, automatycznie planowalną funkcję, którą AI wywołuje jak zwykłe narzędzie.

### Struktura Umiejętności

| Element | Opis |
|------|------|
| `id` / `description` | Unikalny identyfikator i opis jednym zdaniem (wyświetlany AI, decyduje kiedy AI wybiera tę umiejętność) |
| `parameter_schema` | Schemat JSON parametrów, deklarujący każdy placeholder `{param}` używany w podpowiedzi |
| `system_prompt_template` | Szablon podpowiedzi systemowej, w czasie wykonania placeholdery są wypełniane parametrami |
| `tool_whitelist` | Lista narzędzi dozwolonych podczas wykonania (puste = dziedziczenie wszystkich narzędzi istoty) |
| `max_tool_round` / `timeout` | Limit rund narzędzi i timeout (ograniczane przez globalne limity) |
| `on_complete` | Akcja zakończenia: `none` / `write_memory` / `notify_curator` / `broadcast` |
| `trigger_mode` | `Manual` (autonomiczne wywołanie AI) lub `Auto` + planowanie `schedule` |

### Cztery źródła

- **Builtin** — wbudowane w framework (`summarize_document` podsumowanie dokumentu, `code_review` przegląd kodu, `research_topic` badanie tematu)
- **Plugin** — wtyczki rejestrowane przez `ISkillProvider`
- **Being** — istota tworzy w czasie działania przez narzędzie `skill`
- **User** — użytkownik tworzy przez stronę zarządzania umiejętnościami Web UI

### Sposoby wyzwalania

1. **Ręczne (Manual)**: umiejętność jest wstrzykiwana jako zwykła definicja narzędzia do żądania AI, AI decyduje kiedy wywołać; strona planowania priorytetowo kieruje wywołania tej samej nazwy do umiejętności
2. **Automatyczne (Auto + schedule)**: wyrażenie planowania przechowywane w `metadata.schedule`, obsługa trzech formatów:
   - `"09:30"` — codziennie o stałej porze
   - `"6h"` / `"30 m"` / `"2 d"` — okres interwału
   - `"0 9 * * *"` / `"*/15 * * * *"` — podzbiór cron

### Pisanie w Markdown

Umiejętności są przechowywane w Markdown (`skills/{id}.md`, metadane YAML + treść podpowiedzi):

```markdown
---
id: daily_news_digest
description: Wyszukaj dzisiejsze wiadomości technologiczne i wygeneruj podsumowanie
tool_whitelist: [network, work_note]
on_complete: write_memory
---

Użyj narzędzia network do wyszukania najnowszych wiadomości na temat {topic}, wygeneruj 500-znakowe podsumowanie i zapisz je w notatkach pracy.
```

Można napisać tylko treść (pomijając YAML): przy zapisie AI automatycznie uzupełni metadane takie jak id, description, schemat parametrów itp. — pola wypełnione przez użytkownika nigdy nie są nadpisywane.

### Samozarządzanie Istoty

Istota może zarządzać własną biblioteką umiejętności przez narzędzie `skill`:

```json
{ "action": "list" }
{ "action": "create", "id": "my_skill", "system_prompt": "...", "description": "..." }
{ "action": "update_from_md", "skill_id": "my_skill", "markdown": "..." }
{ "action": "delete", "skill_id": "my_skill" }
```

### Gorące przeładowanie i ewolucja

- Istota co 30 sekund sprawdza zmiany w katalogu `skills/` (porównanie odcisku palca), modyfikacje z Web UI lub innych istot wchodzą w życie automatycznie, bez restartu
- Każda aktualizacja umiejętności automatycznie archiwizuje historyczną wersję do `skills/archive/{id}/{version}.md`, tworząc historię ewolucji umiejętności
- Liczba niestandardowych umiejętności jest ograniczona limitem (`MaxCustomSkillsPerBeing`, domyślnie 50)

### Zabezpieczenia wykonania

- Uprawnienia akcji `execute` na poziomie umiejętności (można wyłączyć przez macierz uprawnień, po wyłączeniu umiejętność jest niewidoczna dla AI)
- Parametry wykonania są ograniczane przez globalne limity: rundy ≤ `GlobalMaxToolRound` (domyślnie 10), timeout ≤ `GlobalSkillTimeoutSeconds` (domyślnie 300 sekund)
- Umiejętności nie mogą rekurencyjnie wywoływać same siebie
- Wywołania narzędzi spoza białej listy kończą się niepowodzeniem

## System pamięci

### Typy pamięci

- **Krótkotrwała**: kontekst bieżącej konwersacji
- **Długotrwała**: utrwalona wiedza i doświadczenia
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

## System notatek pracy

### Przegląd

Notatki pracy to osobisty system dziennika Istoty Krzemowej, z projektem stronicowym, służący do rejestrowania postępów w pracy, wniosków z nauki, notatek projektowych itp.

### Cechy

- **Zarządzanie stronicowe**: każda notatka stanowi niezależną stronę, dostępną po numerze strony
- **Obsługa Markdown**: treść obsługuje formatowanie Markdown (tekst, listy, tabele, bloki kodu)
- **Indeksowanie słów kluczowych**: obsługa dodawania słów kluczowych do notatek, ułatwiająca wyszukiwanie
- **Funkcja podsumowania**: każda notatka ma krótkie podsumowanie do szybkiego przeglądu
- **Generowanie spisu treści**: możliwość wygenerowania przeglądu spisu treści wszystkich notatek, pomagającego zrozumieć ogólny kontekst
- **Znaczniki czasu**: automatyczne rejestrowanie czasu utworzenia i aktualizacji
- **Domyślnie prywatne**: dostępne tylko dla samej istoty (Kurator może zarządzać)

### Scenariusze użycia

1. **Rejestr postępów projektu**
   ```
   Podsumowanie: Ukończono moduł uwierzytelniania użytkowników
   Treść: Zaimplementowano weryfikację JWT token, integrację OAuth2, mechanizm odświeżania tokenów
   Słowa kluczowe: uwierzytelnianie,JWT,OAuth2
   ```

2. **Notatki z nauki**
   ```
   Podsumowanie: Nauka najlepszych praktyk programowania asynchronicznego w C#
   Treść: Uwagi dotyczące używania async/await, scenariusze użycia ConfigureAwait...
   Słowa kluczowe: C#,asynchroniczne,najlepsze praktyki
   ```

3. **Podsumowanie spotkania**
   ```
   Podsumowanie: Spotkanie dyskusyjne o wymaganiach produktowych
   Treść: Omówiono wymagania dotyczące nowych funkcji, ustalono plan implementacji...
   Słowa kluczowe: produkt,wymagania,spotkanie
   ```

### Użycie przez narzędzia

Istota może zarządzać notatkami pracy przez narzędzie `work_note`:

```json
// Tworzenie notatki
{
  "action": "create",
  "summary": "Ukończono moduł uwierzytelniania użytkowników",
  "content": "## Szczegóły implementacji\n\n- Użycie JWT token\n- Obsługa OAuth2",
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
2. Kliknij zakładkę **Notatki pracy**
3. Możesz przeglądać, wyszukiwać i edytować notatki
4. Obsługa podglądu Markdown

## System Sieci Wiedzy

### Przegląd

Sieć Wiedzy to system reprezentacji i zarządzania wiedzą oparty na strukturze trójkowej (podmiot-orzecznik-obiekt), służący do przechowywania i zarządzania ustrukturyzowaną wiedzą.

### Kluczowe koncepcje

#### Struktura trójkowa

```
Podmiot (Subject) --Orzecznik (Predicate)--> Obiekt (Object)
```

**Przykłady**:
- `Python` --`is_a`--> `programming_language`
- `Pekin` --`capital_of`--> `Chiny`
- `Woda` --`boiling_point`--> `100°C`

#### Pewność

Każda trójka wiedzy posiada ocenę pewności (0.0-1.0), wskazującą stopień wiarygodności wiedzy:
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

#### 2. Zapytania o wiedzę

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

Odkrywanie ścieżki powiązań między dwoma pojęciami:
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

1. Przejdź do strony **Sieć Wiedzy**
2. Przeglądaj statystyki wiedzy
3. Wyszukuj i przeglądaj wiedzę
4. Wizualizacja grafu relacji wiedzy (w planach)

## Operacje przeglądarki WebView (nowość)

### Przegląd

Istoty Krzemowe mogą autonomicznie przeglądać strony internetowe, pozyskiwać informacje i wykonywać operacje na stronach przez narzędzie przeglądarki WebView. Przeglądarka działa w trybie headless, całkowicie niewidocznym dla użytkownika.

### Cechy

- **Indywidualna izolacja**: każda istota posiada niezależną instancję przeglądarki, ciasteczka i sesję
- **Tryb headless**: autonomiczne działanie w tle, niewidoczne dla użytkownika
- **Pełna funkcjonalność**: obsługa wykonywania JavaScript, renderowania CSS, wypełniania formularzy itp.
- **Kontrola bezpieczeństwa**: wszystkie operacje muszą przejść przez łańcuch weryfikacji uprawnień

### Typowe operacje

#### 1. Otwarcie przeglądarki

```json
{
  "action": "open"
}
```

#### 2. Nawigacja do strony

```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

#### 3. Pobranie treści strony

```json
{
  "action": "get_page_text"
}
```

Zwraca tekstową treść strony do analizy i zrozumienia przez AI.

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

Zwraca zrzut ekranu strony (kodowanie Base64), który może być użyty do analizy wizualnej.

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
   - Przeglądanie witryn informacyjnych w celu uzyskania najnowszych wiadomości
   - Wyszukiwanie dokumentacji i materiałów technicznych
   - Monitorowanie zmian treści na stronach internetowych

2. **Operacje automatyczne**
   - Wypełnianie i przesyłanie formularzy
   - Klikanie przycisków w celu wyzwolenia operacji
   - Pobieranie danych ze stron internetowych

3. **Analiza stron**
   - Analiza struktury i treści strony
   - Ekstrakcja określonych informacji
   - Analiza wizualna zrzutów ekranu

### Uwagi

- Operacje przeglądarki mogą być powolne, należy poczekać na załadowanie strony
- Używaj `wait_for_element`, aby upewnić się, że element pojawił się przed operacją
- Przestrzegaj warunków korzystania z witryn i plików robots.txt
- Unikaj częstych żądań, które mogą prowadzić do zablokowania

## Najlepsze praktyki

### Tworzenie Pliku Duszy

1. **Bądź konkretny**: jasne cechy osobowości i granice
2. **Zdefiniuj zakres**: co istota powinna i czego nie powinna robić
3. **Dołącz przykłady**: pokazuj oczekiwane wzorce zachowań
4. **Aktualizuj regularnie**: ewoluuj duszę na podstawie wyników

### Zarządzanie zadaniami

1. **Ustawiaj priorytety**: używaj priorytetów (1-10)
2. **Definiuj terminy**: zawsze ustawiaj terminy realizacji
3. **Monitoruj postępy**: regularnie sprawdzaj status zadań
4. **Obsługuj niepowodzenia**: implementuj logikę ponownych prób

### Optymalizacja pamięci

1. **Oczyszczaj stare dane**: regularnie archiwizuj stare wspomnienia
2. **Indeksuj ważne informacje**: oznaczaj kluczowe informacje
3. **Używaj przechowywania czasowego**: wykorzystuj zapytania indeksowane czasowo

### Mechanizm zanikania pamięci

System posiada wbudowaną usługę okresowego zanikania `MemoryFadeService`, symulującą właściwość zapominania pamięci biologicznej:

- **Automatyczne zanikanie**: co godzinę stosuje algorytm zanikania ważności do wpisów pamięci wszystkich Istot Krzemowych
- **Automatyczna archiwizacja**: pamięć o ważności poniżej progu jest automatycznie archiwizowana i nie bierze udziału w codziennym wyszukiwaniu
- **Śledzenie statystyk**: rejestrowanie liczby cykli zanikania i liczby wpisów ze zmienionym stanem

Oznacza to, że pamięć Istot Krzemowych naturalnie zanika z czasem, a ważne informacje muszą być aktywnie oznaczane jako wysoko ważne przez narzędzia pamięci, aby uniknąć automatycznej archiwizacji.

---

## Przestrzeń projektowa

### Przegląd

Przestrzeń projektowa to mechanizm zarządzania przestrzenią wspierającą współpracę wielu Istot Krzemowych. Kurator Krzemowy może tworzyć przestrzenie projektowe, przypisywać Istoty Krzemowe do projektów i przydzielać im role.

### Cykl życia projektu

```
Tworzenie → Aktywny → Archiwizacja → Zniszczenie
              ↑       |
              └─ Przywrócenie ┘
```

### Role projektowe

Istoty Krzemowe mogą mieć przypisane określone role w projektach:

```json
{
  "action": "assign_role",
  "project_id": "project-uuid",
  "being_id": "being-uuid",
  "role_name": "developer"
}
```

### Notatki pracy projektu

Notatki pracy w przestrzeni projektowej są publiczne, dostępne dla wszystkich członków projektu:

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "Ukończono moduł uwierzytelniania użytkowników",
  "content": "## Szczegóły implementacji\n\n- Użycie JWT token",
  "keywords": "uwierzytelnianie,JWT"
}
```

### Zadania projektu

Zadania w przestrzeni projektowej obsługują pełne zarządzanie cyklem życia:

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "title": "Implementacja uwierzytelniania użytkowników",
  "priority": 5
}
```

### Przepływ pracy projektu

Projekty mogą być powiązane z szablonami przepływu pracy, napędzającymi procesy współpracy Istot Krzemowych:

- Przepływy pracy oparte na szablonach maszyny stanów
- Obsługa przejść stanów napędzanych przez Tick
- Automatyczne rejestrowanie logów przejść stanów

### Izolacja uprawnień narzędzi

Uprawnienia narzędzi na poziomie projektu są niezależne od uprawnień na poziomie Istoty Krzemowej, realizując izolację uprawnień między projektami. Na przykład, Istota Krzemowa może mieć uprawnienia dostępu sieciowego w projekcie A, ale być ograniczona do uprawnień tylko do odczytu w projekcie B.

## Rozwiązywanie problemów

### Istota nie może się uruchomić

**Sprawdź**:
- Plik Duszy istnieje i jest prawidłowy
- Klient AI jest skonfigurowany
- Zasoby systemowe są wystarczające

### Istota nieoczekiwanie się zatrzymuje

**Sprawdź**:
- Błędy w logach
- Dostępność usługi AI
- Użycie pamięci

### Zadania nie są wykonywane

**Sprawdź**:
- System czasomierzy działa
- Priorytety i harmonogram zadań
- Ustawienia uprawnień

## Następne kroki

- 📚 Przeczytaj [przewodnik architektury](architecture.md)
- 🛠️ Zobacz [przewodnik deweloperski](development-guide.md)
- 🚀 Zobacz [przewodnik szybkiego startu](getting-started.md)
