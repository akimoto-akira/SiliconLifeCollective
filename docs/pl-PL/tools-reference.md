# Referencja narzędzi

> **Wersja: v0.2.0-alpha**

Ten dokument szczegółowo opisuje wszystkie wbudowane narzędzia platformy Silicon Life Collective.

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md) | [Русский](../ru-RU/tools-reference.md)

## Przegląd

System narzędzi pozwala Istotom Krzemowym na interakcję ze światem zewnętrznym przez ustandaryzowany interfejs. Każde narzędzie implementuje interfejs `ITool`, automatycznie odkrywany i rejestrowany przez `ToolManager` przez refleksję.

### Klasyfikacja narzędzi

- **Narzędzia zarządzania systemem** — konfiguracja, uprawnienia, kompilacja dynamiczna, zarządzanie Kuratorem
- **Narzędzia komunikacyjne** — czat, żądania sieciowe
- **Narzędzia przechowywania danych** — operacje dyskowe, baza danych, pamięć, notatki pracy
- **Narzędzia zarządzania czasem** — kalendarz, czasomierze, zadania
- **Narzędzia deweloperskie** — wykonywanie kodu, zapytania logów
- **Narzędzia użytkowe** — informacje systemowe, audyt tokenów, dokumentacja pomocy, Sieć Wiedzy
- **Narzędzia przeglądarki** — automatyzacja przeglądarki WebView
- **Narzędzia projektowe** — zarządzanie projektami, zadania projektowe, notatki pracy projektu, praca projektowa
- **Narzędzia wtyczek** — narzędzia firm trzecich zarejestrowane przez system wtyczek

### System scenariuszy narzędzi

Każde narzędzie deklaruje swoje dostępne scenariusze przez atrybut `[ToolScenario]`:

| Flaga scenariusza | Wartość | Opis |
|----------|------|-------------|
| `Chat` | `1 << 0` | Scenariusz czatu (gdy użytkownik rozmawia z Istotą Krzemową) |
| `Task` | `1 << 1` | Scenariusz zadania (gdy Istota Krzemowa wykonuje zadanie) |
| `Timer` | `1 << 2` | Scenariusz czasomierza (gdy Istota Krzemowa wykonuje zadanie czasowe) |
| `MemoryCompression` | `1 << 3` | Scenariusz kompresji pamięci |
| `Project` | `1 << 4` | Scenariusz projektu (tryb ThinkOnProject) |
| `All` | Powyższe wszystkie | Wszystkie scenariusze dostępne |

Dodatkowo, narzędzia oznaczone atrybutem `[ChatOnly]` są dostępne tylko w scenariuszu czatu (np. HelpTool) i nie pojawiają się w scenariuszach zadań i czasomierzy.

---

## Lista wbudowanych narzędzi

### 1. Narzędzie kalendarza (CalendarTool)

**Nazwa narzędzia**: `calendar`

**Opis funkcji**: obsługa konwersji i obliczeń dat w 32 systemach kalendarzowych.

**Obsługiwane operacje**:
- `now` — pobranie bieżącego czasu
- `format` — formatowanie daty
- `add_days` — dodawanie/odejmowanie dni
- `diff` — obliczanie różnicy dat
- `list_calendars` — lista wszystkich obsługiwanych kalendarzy
- `get_components` — pobranie komponentów daty
- `get_now_components` — pobranie komponentów bieżącego czasu
- `convert` — konwersja między systemami kalendarzowymi

**Obsługiwane systemy kalendarzowe** (32):
- Gregoriański (Gregorian)
- Chiński księżycowy (Chinese Lunar)
- Chiński historyczny (Chinese Historical) — era ganzhi, epoki cesarskie
- Islamski (Islamic)
- Hebrajski (Hebrew)
- Japoński (Japanese)
- Perski (Persian)
- Majski (Mayan)
- Buddyjski (Buddhist)
- Tybetański (Tibetan)
- I 24 inne kalendarze...

**Przykład użycia**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_lunar"
}
```

---

### 2. Narzędzie czatu (ChatTool)

**Nazwa narzędzia**: `chat`

**Opis funkcji**: zarządzanie sesjami czatu i wysyłanie wiadomości.

**Obsługiwane operacje**:
- `send_message` — wysłanie wiadomości
- `get_messages` — pobranie historii wiadomości
- `create_group` — utworzenie czatu grupowego
- `add_member` — dodanie członka grupy
- `remove_member` — usunięcie członka grupy
- `get_chat_info` — pobranie informacji o czacie
- `terminate_chat` — zakończenie czatu (przeczytane bez odpowiedzi)

**Przykład użycia**:
```json
{
  "action": "send_message",
  "target_id": "being-uuid-or-user-0",
  "message": "Cześć, współpracujmy!"
}
```

---

### 3. Narzędzie konfiguracji (ConfigTool)

**Nazwa narzędzia**: `config`

**Opis funkcji**: odczyt i modyfikacja konfiguracji systemu.

**Obsługiwane operacje**:
- `read` — odczyt elementu konfiguracji
- `write` — zapis elementu konfiguracji
- `list` — lista wszystkich konfiguracji
- `get_ai_config` — pobranie konfiguracji klienta AI
- `set_ai_config` — ustawienie konfiguracji klienta AI

**Przykład użycia**:
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

---

### 4. Narzędzie Kuratora (CuratorTool) 🔒

**Nazwa narzędzia**: `silicon_manager`

**Wymagania uprawnień**: tylko dla Kuratora Krzemowego (`[SiliconManagerOnly]`)

**Dostępne scenariusze**: Chat, Task, Timer

**Opis funkcji**: narzędzie zarządzania systemem wyłącznie dla Kuratora Krzemowego, służące do zarządzania tworzeniem, przeglądaniem i resetowaniem Istot Krzemowych.

**Obsługiwane operacje**:
- `list_beings` — lista wszystkich Istot Krzemowych i ich statusu
- `create_being` — utworzenie nowej Istoty Krzemowej (wymaga parametrów `name` i `soul`)
- `get_code` — przeglądanie niestandardowego kodu źródłowego Istoty Krzemowej
- `reset` — przywrócenie Istoty Krzemowej do domyślnej implementacji

**Przykład użycia**:
```json
{
  "action": "create_being",
  "name": "Asystent",
  "soul": "Jesteś pomocnym asystentem..."
}
```

---

### 5. Narzędzie bazy danych (DatabaseTool)

**Nazwa narzędzia**: `database`

**Opis funkcji**: ustrukturyzowane zapytania i operacje bazodanowe.

**Obsługiwane operacje**:
- `query` — zapytanie o dane
- `insert` — wstawienie danych
- `update` — aktualizacja danych
- `delete` — usunięcie danych
- `create_table` — utworzenie tabeli
- `list_tables` — lista wszystkich tabel

**Przykład użycia**:
```json
{
  "action": "query",
  "table": "users",
  "conditions": {"status": "active"},
  "limit": 100
}
```

---

### 6. Narzędzie dyskowe (DiskTool)

**Nazwa narzędzia**: `disk`

**Opis funkcji**: operacje systemu plików i wyszukiwanie lokalne.

**Obsługiwane operacje**:
- `read` — odczyt pliku
- `write` — zapis pliku
- `list` — lista katalogu
- `delete` — usunięcie pliku
- `create_directory` — utworzenie katalogu
- `search_files` — wyszukiwanie plików
- `search_content` — wyszukiwanie treści plików
- `count_lines` — zliczanie linii
- `read_lines` — odczyt określonych linii
- `replace_text` — zastąpienie tekstu

**Wymagania uprawnień**: `FileAccess`

**Przykład użycia**:
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

---

### 7. Narzędzie kompilacji dynamicznej (DynamicCompileTool) 🔒

**Nazwa narzędzia**: `compile`

**Opis funkcji**: dynamiczna kompilacja kodu C# (do samewolucji Istot Krzemowych).

**Obsługiwane operacje**:
- `compile_class` — kompilacja klasy
- `compile_callback` — kompilacja funkcji wywołania zwrotnego uprawnień
- `validate_code` — walidacja bezpieczeństwa kodu

**Mechanizmy bezpieczeństwa**:
- Kontrola referencji w czasie kompilacji (wykluczenie niebezpiecznych zestawów)
- Statyczne skanowanie kodu w czasie działania
- Szyfrowane przechowywanie AES-256

**Przykład użycia**:
```json
{
  "action": "compile_class",
  "code": "public class MyBeing : SiliconBeingBase { ... }"
}
```

---

### 8. Narzędzie wykonywania kodu (ExecuteCodeTool) 🔒

**Nazwa narzędzia**: `execute_code`

**Wymagania uprawnień**: tylko dla Kuratora Krzemowego

**Opis funkcji**: kompilacja i wykonywanie fragmentów kodu C#.

**Obsługiwane operacje**:
- `run_script` — wykonanie skryptu kodu

**Przykład użycia**:
```json
{
  "action": "run_script",
  "code": "return DateTime.Now.ToString();",
  "timeout": 30
}
```

---

### 9. Narzędzie pomocy (HelpTool)

**Nazwa narzędzia**: `help`

**Dostępne scenariusze**: Chat (`[ChatOnly]`, dostępne tylko w scenariuszu czatu)

**Opis funkcji**: wyszukiwanie i pobieranie treści dokumentacji pomocy systemowej, pozwalające AI na zapytania o metody korzystania z funkcji systemu.

**Obsługiwane operacje**:
- `list` — lista wszystkich identyfikatorów tematów pomocy
- `search` — wyszukiwanie dokumentacji pomocy po słowach kluczowych
- `get` — pobranie treści dokumentacji pomocy o określonym identyfikatorze

**Przykład użycia**:
```json
{
  "action": "search",
  "keyword": "uprawnienia"
}
```

---

### 10. Narzędzie Sieci Wiedzy (KnowledgeTool)

**Nazwa narzędzia**: `knowledge`

**Opis funkcji**: operacje na grafie wiedzy (oparte na trójkach: podmiot-relacja-obiekt).

**Obsługiwane operacje**:
- `add` — dodanie trójki wiedzy
- `query` — zapytanie o wiedzę
- `update` — aktualizacja wiedzy
- `delete` — usunięcie wiedzy
- `search` — wyszukiwanie wiedzy
- `get_path` — pobranie ścieżki wiedzy
- `validate` — walidacja wiedzy
- `stats` — pobranie statystyk

**Przykład użycia**:
```json
{
  "action": "add",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95
}
```

---

### 11. Narzędzie logów (LogTool)

**Nazwa narzędzia**: `log`

**Opis funkcji**: zapytania o historię operacji i historię konwersacji.

**Obsługiwane operacje**:
- `query_logs` — zapytanie o logi systemowe
- `query_conversations` — zapytanie o historię konwersacji
- `get_stats` — pobranie statystyk logów

**Przykład użycia**:
```json
{
  "action": "query_logs",
  "being_id": "being-uuid",
  "start_time": "2026-04-20T00:00:00Z",
  "end_time": "2026-04-26T23:59:59Z",
  "level": "info"
}
```

---

### 12. Narzędzie pamięci (MemoryTool)

**Nazwa narzędzia**: `memory`

**Opis funkcji**: zarządzanie pamięcią długotrwałą i krótkotrwałą Istoty Krzemowej.

**Obsługiwane operacje**:
- `read` — odczyt pamięci
- `write` — zapis pamięci
- `search` — wyszukiwanie pamięci
- `delete` — usunięcie pamięci
- `list` — lista pamięci
- `get_stats` — pobranie statystyk pamięci
- `compress` — kompresja pamięci

**Przykład użycia**:
```json
{
  "action": "read",
  "key": "important_fact",
  "time_range": {
    "start": "2026-04-01",
    "end": "2026-04-26"
  }
}
```

---

### 13. Narzędzie sieciowe (NetworkTool)

**Nazwa narzędzia**: `network`

**Opis funkcji**: wysyłanie żądań HTTP/HTTPS.

**Obsługiwane operacje**:
- `get` — żądanie GET
- `post` — żądanie POST
- `put` — żądanie PUT
- `delete` — żądanie DELETE
- `download` — pobieranie pliku
- `upload` — przesyłanie pliku

**Wymagania uprawnień**: `network:http`

**Przykład użycia**:
```json
{
  "action": "get",
  "url": "https://api.example.com/data"
}
```

---

### 14. Narzędzie uprawnień (PermissionTool) 🔒

**Nazwa narzędzia**: `permission`

**Wymagania uprawnień**: tylko dla Kuratora Krzemowego

**Opis funkcji**: zarządzanie uprawnieniami i listami kontroli dostępu.

**Obsługiwane operacje**:
- `query_permission` — zapytanie o uprawnienia
- `manage_acl` — zarządzanie globalną ACL
- `get_callback` — pobranie funkcji wywołania zwrotnego uprawnień
- `set_callback` — ustawienie funkcji wywołania zwrotnego uprawnień

**Przykład użycia**:
```json
{
  "action": "manage_acl",
  "acl_action": "add_rule",
  "permission_type": "filesystem",
  "resource_prefix": "/data/",
  "acl_result": "allow"
}
```

---

### 15. Narzędzie projektowe (ProjectTool) 🔒

**Nazwa narzędzia**: `project`

**Wymagania uprawnień**: tylko dla Kuratora Krzemowego (`[SiliconManagerOnly]`)

**Dostępne scenariusze**: Chat, Task, Timer

**Opis funkcji**: zarządzanie przestrzenią projektową, obsługa zarządzania cyklem życia projektu, przypisywania członków i zarządzania rolami.

**Obsługiwane operacje**:
- `create` — utworzenie nowej przestrzeni projektowej
- `archive` — archiwizacja projektu
- `restore` — przywrócenie zarchiwizowanego projektu
- `destroy` — zniszczenie projektu i oczyszczenie danych (nieodwracalne)
- `list` — lista wszystkich projektów
- `get` — pobranie szczegółów projektu
- `assign` — przypisanie Istoty Krzemowej do projektu
- `remove` — usunięcie Istoty Krzemowej z projektu
- `update` — aktualizacja nazwy/opisu projektu
- `list-workflow-templates` — lista dostępnych szablonów przepływu pracy
- `assign_role` — przypisanie roli projektowej Istocie Krzemowej
- `remove_role` — usunięcie roli projektowej Istoty Krzemowej
- `list_roles` — lista przypisań ról w projekcie

**Przykład użycia**:
```json
{
  "action": "create",
  "name": "Mój Projekt",
  "description": "Opis projektu"
}
```

---

### 16. Narzędzie zadań projektowych (ProjectTaskTool)

**Nazwa narzędzia**: `project_task`

**Dostępne scenariusze**: Chat, Task, Timer

**Opis funkcji**: zarządzanie zadaniami w przestrzeni projektowej, obsługa pełnego cyklu życia zadań.

**Obsługiwane operacje**:
- `create` — utworzenie zadania projektowego
- `list` — lista zadań projektowych
- `get` — pobranie szczegółów zadania
- `update` — aktualizacja tytułu/opisu/priorytetu zadania
- `assign` — przypisanie osoby odpowiedzialnej do zadania
- `remove_assignee` — usunięcie osoby odpowiedzialnej za zadanie
- `start` — rozpoczęcie zadania
- `complete` — oznaczenie zadania jako ukończone
- `fail` — oznaczenie zadania jako nieudane
- `cancel` — anulowanie zadania
- `delete` — usunięcie zadania
- `stats` — pobranie statystyk zadań

**Przykład użycia**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "Opis zadania do ukończenia",
  "priority": 5
}
```

---

### 17. Narzędzie notatek pracy projektu (ProjectWorkNoteTool)

**Nazwa narzędzia**: `project_work_note`

**Dostępne scenariusze**: Chat, Task, Timer

**Opis funkcji**: zarządzanie notatkami pracy w przestrzeni projektowej (publicznymi, podobnymi do zeszytu roboczego), obsługa stronicowego zarządzania notatkami.

**Obsługiwane operacje**:
- `create` — utworzenie strony notatki (wymaga `project_id`, `summary` i `content`, opcjonalnie `keywords`)
- `read` — odczyt strony notatki (wymaga `project_id` i `page_number` lub `note_id`)
- `update` — aktualizacja strony notatki (wymaga `project_id`, `page_number` i `content`, opcjonalnie `summary` i `keywords`)
- `delete` — usunięcie strony notatki (wymaga `project_id` i `page_number` lub `note_id`)
- `list` — lista podsumowań wszystkich stron notatek projektu
- `directory` — generowanie spisu treści/przeglądu notatek
- `search` — wyszukiwanie notatek po słowach kluczowych (wymaga `project_id` i `keyword`, opcjonalnie `max_results`)

**Przykład użycia**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "Ukończono moduł uwierzytelniania użytkowników",
  "content": "## Szczegóły implementacji\n\n- Użycie JWT token",
  "keywords": "uwierzytelnianie,JWT"
}
```

---

### 18. Narzędzie pracy projektowej (ProjectWorkTool) 🔒

**Nazwa narzędzia**: `project_work`

**Wymagania uprawnień**: tylko dla Kuratora Krzemowego (`[SiliconManagerOnly]`)

**Dostępne scenariusze**: Project (`[ToolScenario(ToolScenarioFlag.Project)]`, dostępne tylko w scenariuszu projektu)

**Opis funkcji**: narzędzie operacji pracy projektowej, służące Kuratorowi do zarządzania przepływem pracy projektu w scenariuszu ThinkOnProject.

**Obsługiwane operacje**:
- `create-task` — utworzenie zadania projektowego
- `assign-task` — przypisanie Istoty Krzemowej do zadania
- `chat` — wysłanie wiadomości do czatu grupowego projektu
- `broadcast` — transmisja wiadomości do kanału projektu
- `complete` — oznaczenie projektu jako ukończony
- `status` — pobranie statusu projektu

**Przykład użycia**:
```json
{
  "action": "create-task",
  "project_id": "project-uuid",
  "title": "Implementacja uwierzytelniania użytkowników"
}
```

---

### 19. Narzędzie systemowe (SystemTool)

**Nazwa narzędzia**: `system`

**Opis funkcji**: pobieranie informacji o systemie i użyciu zasobów.

**Obsługiwane operacje**:
- `info` — pobranie informacji o systemie
- `resource_usage` — pobranie użycia zasobów
- `find_process` — wyszukiwanie procesu
- `list_beings` — lista Istot Krzemowych

**Przykład użycia**:
```json
{
  "action": "info"
}
```

---

### 20. Narzędzie zadań (TaskTool)

**Nazwa narzędzia**: `task`

**Opis funkcji**: zarządzanie osobistymi zadaniami Istoty Krzemowej.

**Obsługiwane operacje**:
- `create` — utworzenie zadania
- `list` — lista zadań
- `update` — aktualizacja zadania
- `complete` — ukończenie zadania
- `delete` — usunięcie zadania
- `get_dependencies` — pobranie zależności

**Przykład użycia**:
```json
{
  "action": "create",
  "description": "Przegląd kodu",
  "priority": 5
}
```

---

### 21. Narzędzie czasomierzy (TimerTool)

**Nazwa narzędzia**: `timer`

**Opis funkcji**: tworzenie i zarządzanie czasomierzami.

**Obsługiwane operacje**:
- `create` — utworzenie czasomierza
- `list` — lista czasomierzy
- `delete` — usunięcie czasomierza
- `pause` — wstrzymanie czasomierza
- `resume` — wznowienie czasomierza
- `get_execution_history` — pobranie historii wykonania

**Przykład użycia**:
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true,
  "message": "Przypomnienie co godzinę"
}
```

---

### 22. Narzędzie audytu tokenów (TokenAuditTool) 🔒

**Nazwa narzędzia**: `token_audit`

**Wymagania uprawnień**: tylko dla Kuratora Krzemowego (`[SiliconManagerOnly]`)

**Dostępne scenariusze**: Chat, Task, Timer

**Opis funkcji**: zapytania o statystyki użycia tokenów AI i dane trendów.

**Obsługiwane operacje**:
- `summary` — pobranie podsumowania statystyk użycia tokenów
- `trend` — pobranie punktów danych trendu użycia tokenów

**Obsługiwane zakresy czasowe**:
- `today` — ostatnie 24 godziny
- `week` — ostatnie 7×24 godzin
- `month` — statystyki dzienne
- `year` — statystyki miesięczne

**Przykład użycia**:
```json
{
  "action": "summary",
  "time_range": "week"
}
```

---

### 23. Narzędzie przeglądarki WebView (WebViewBrowserTool)

**Nazwa narzędzia**: `webview_browser`

**Dostępne scenariusze**: Chat, Task, Timer

**Opis funkcji**: automatyzacja przeglądarki oparta na Playwright, zapewniająca pełne możliwości nawigacji po stronach, interakcji i ekstrakcji danych.

**Obsługiwane operacje**:
- `open` — otwarcie przeglądarki
- `close` — zamknięcie przeglądarki
- `navigate` — nawigacja do URL
- `click` — kliknięcie elementu
- `input` — wprowadzenie tekstu
- `scroll` — przewijanie strony
- `execute_script` — wykonanie JavaScript
- `get_page_text` — pobranie tekstu strony
- `get_screenshot` — pobranie zrzutu ekranu
- `wait_for_element` — oczekiwanie na pojawienie się elementu
- `get_element_info` — pobranie informacji o elemencie
- `upload_file` — przesłanie pliku
- `get_browser_status` — pobranie statusu przeglądarki
- `set_timeout` — ustawienie czasu oczekiwania
- `clear_session` — wyczyszczenie sesji przeglądarki

**Cechy**:
- Niezależna instancja dla każdej Istoty Krzemowej
- Całkowicie odizolowane ciasteczka i sesje
- Całkowicie niewidoczne dla użytkownika (tryb headless)
- Pełna obsługa JavaScript i CSS

**Przykład użycia**:
```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

---

### 24. Narzędzie notatek pracy (WorkNoteTool)

**Nazwa narzędzia**: `work_note`

**Opis funkcji**: zarządzanie osobistymi notatkami pracy Istoty Krzemowej (prywatne, podobne do dziennika).

**Obsługiwane operacje**:
- `create` — utworzenie notatki
- `read` — odczyt notatki
- `update` — aktualizacja notatki
- `delete` — usunięcie notatki
- `list` — lista notatek
- `search` — wyszukiwanie notatek
- `directory` — generowanie spisu treści

**Przykład użycia**:
```json
{
  "action": "create",
  "summary": "Ukończono moduł uwierzytelniania użytkowników",
  "content": "## Szczegóły implementacji\n\n- Użycie JWT token\n- Obsługa OAuth2",
  "keywords": "uwierzytelnianie,JWT,OAuth2"
}
```

---

### 25. Narzędzie gorącego przeładowania (HotReloadTool)

**Nazwa narzędzia**: `hot_reload`

**Opis funkcji**: obsługa automatycznej kompilacji, aktualizacji plików i restartu SiliconLife.Fast w trakcie działania, bez konieczności ręcznej interwencji.

**Obsługiwane operacje**:
- `execute` — wykonanie pełnego procesu budowania, kopiowania i restartu
- `build_only` — tylko budowanie projektu, bez kopiowania i restartu

**Przepływ pracy**:
1. Kompilacja projektu SiliconLife.Fast
2. Eleganckie zamknięcie bieżącej instancji Fast (przez HTTP API)
3. Oczekiwanie na zakończenie procesu i zwolnienie portu
4. Kopiowanie wyników budowania do katalogu docelowego (pomijanie plików HotReload)
5. Ponowne uruchomienie instancji Fast

**Cechy**:
- Automatyczne wykrywanie i zamykanie starego procesu
- Bezpieczne kopiowanie plików (bez nadpisywania HotReload.exe)
- Mechanizm oczekiwania na zwolnienie portu
- Obsługa niestandardowej konfiguracji portu

**Przykład użycia**:
```json
{
  "action": "execute",
  "project_path": "src/SiliconLife.Fast",
  "source_path": "src/SiliconLife.Fast/bin/Debug/net9.0",
  "configuration": "Debug",
  "port": 8080
}
```

**Opis parametrów**:
- `project_path`: ścieżka projektu (względem katalogu głównego rozwiązania)
- `source_path`: katalog wyników budowania
- `configuration`: konfiguracja budowania (Debug/Release)
- `port`: port Web instancji Fast (domyślnie 8080)

**Uwagi**:
- Dotyczy tylko wersji SiliconLife.Fast
- Wymaga HotReload.exe w katalogu tools/HotReload
- Podczas restartu wystąpi krótka przerwa w działaniu usługi (około 3-5 sekund)

---

## Przepływ wywołań narzędzi

```
┌──────────┐
│   AI     │ Zwraca tool_calls
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ Wyszukuje i weryfikuje prawo użycia narzędzia
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ Sprawdza łańcuch uprawnień
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ Wykonuje operacje dostępu do zasobów
└────┬─────────┘
     ↓
┌──────────┐
│   AI     │ Otrzymuje wyniki narzędzia, kontynuuje myślenie
└──────────┘
```

## Weryfikacja uprawnień

Wszystkie wykonania narzędzi przechodzą przez łańcuch weryfikacji uprawnień:

1. **UserFrequencyCache** — pamięć podręczna decyzji użytkownika o wysokiej częstotliwości (HighDeny ma priorytet nad HighAllow)
2. **IPermissionCallback** — niestandardowa funkcja wywołania zwrotnego uprawnień (Allowed/Denied/AskUser)
3. **Gałąź IsCurator** — Kurator pyta użytkownika przez IPermissionAskHandler; nie-Kurator odpytuje GlobalACL, domyślnie odmawia przy braku pasującej reguły

## Tworzenie niestandardowych narzędzi

### Krok 1: Implementacja interfejsu ITool

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    
    public string Description => "Opis narzędzia";
    
    public ToolDefinition Definition => new ToolDefinition
    {
        Name = Name,
        Description = Description,
        Parameters = new Dictionary<string, object>
        {
            ["param1"] = new { type = "string", description = "Opis parametru" }
        }
    };
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        try
        {
            var param1 = call.Parameters["param1"]?.ToString();
            var result = await DoWork(param1);
            
            return new ToolResult
            {
                Success = true,
                Output = result
            };
        }
        catch (Exception ex)
        {
            return new ToolResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }
}
```

### Krok 2: Dodanie do projektu

Umieść plik narzędzia w katalogu `src/SiliconLife.Common/Tools/` (narzędzia współdzielone) lub w katalogu `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` (narzędzia specyficzne dla wersji). `ToolManager` automatycznie odkryje i zarejestruje narzędzie przez refleksję podczas uruchomienia.

### Krok 2a: Rejestracja narzędzia przez wtyczkę

Można również zarejestrować niestandardowe narzędzia przez system wtyczek:

1. Zaimplementuj interfejs `ITool` w projekcie wtyczki
2. Skompiluj DLL wtyczki i umieść w katalogu wtyczek
3. `ToolManager.ScanAllPluginAssemblies()` automatycznie przeskanuje wszystkie załadowane wtyczki w poszukiwaniu implementacji ITool
4. Narzędzia wtyczek podlegają tym samym ograniczeniom systemu uprawnień

### Krok 3: (Opcjonalnie) Oznaczenie jako wyłącznie dla Kuratora

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // Dostępne tylko dla Kuratora Krzemowego
}
```

## Najlepsze praktyki

### 1. Zawsze waliduj parametry

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("Brak wymaganego parametru: required_param");
}
```

### 2. Elegancko obsługuj błędy

```csharp
try
{
    // Wykonaj operację
}
catch (Exception ex)
{
    Logger.Error($"Narzędzie {Name} nie powiodło się: {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. Szanuj system uprawnień

Nigdy nie omijaj sprawdzania uprawnień. Zawsze uzyskuj dostęp do zasobów przez wykonawców:

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return ToolResult.Denied("Permission denied");
}
```

### 4. Dostarczaj jasne opisy narzędzi

Pomóż AI zrozumieć, kiedy i jak używać narzędzia:

```csharp
public string Description => 
    "Służy do konwersji dat między różnymi systemami kalendarzowymi." +
    "Wymaga podania parametrów 'date', 'from_calendar' i 'to_calendar'.";
```

## Rozwiązywanie problemów

### Narzędzie nie znalezione

**Problem**: AI próbuje wywołać nieistniejące narzędzie.

**Rozwiązanie**:
- Sprawdź, czy nazwa narzędzia dokładnie pasuje
- Zweryfikuj, czy plik narzędzia znajduje się w katalogu `Tools/`
- Przebuduj projekt (`dotnet build`)

### Uprawnienia odrzucone

**Problem**: wykonanie narzędzia nie powiodło się, zwrócono błąd uprawnień.

**Rozwiązanie**:
- Sprawdź dziennik audytu uprawnień
- Zweryfikuj, czy Istota Krzemowa posiada wymagane uprawnienia
- Przejrzyj ustawienia globalnej ACL
- Jeśli to Kurator, sprawdź, czy użyto znacznika `[SiliconManagerOnly]`

### Wykonanie narzędzia zwraca błąd

**Problem**: narzędzie wykonuje się, ale zwraca niepowodzenie.

**Rozwiązanie**:
- Sprawdź komunikat błędu zwrócony przez narzędzie
- Zweryfikuj poprawność formatu parametrów wejściowych
- Przejrzyj logi systemowe w celu uzyskania szczegółowych informacji o błędzie
- Przetestuj funkcjonalność narzędzia niezależnie

## Następne kroki

- 📚 Przeczytaj [przewodnik architektury](architecture.md)
- 🛠️ Zobacz [przewodnik deweloperski](development-guide.md)
- 🔒 Poznaj [system uprawnień](permission-system.md)
- 🚀 Zobacz [przewodnik szybkiego startu](getting-started.md)
