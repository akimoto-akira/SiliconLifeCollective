# Referencja narzędzi

> **Wersja: v0.2.0-alpha**

Ten dokument szczegółowo opisuje wszystkie wbudowane narzędzia platformy Silicon Life Collective.

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md) | [Polski](../pl-PL/tools-reference.md)

## Przegląd

System narzędzi pozwala Istotom Krzemowym na interakcję ze światem zewnętrznym poprzez ustandaryzowany interfejs. Każde narzędzie implementuje interfejs `ITool`, automatycznie odkrywany i rejestrowany przez `ToolManager` poprzez refleksję.

### Klasyfikacja narzędzi

- **Narzędzia zarządzania systemem** — konfiguracja, uprawnienia, kompilacja dynamiczna
- **Narzędzia komunikacyjne** — czat, żądania sieciowe
- **Narzędzia przechowywania danych** — operacje dyskowe, baza danych, pamięć, notatki pracy
- **Narzędzia zarządzania czasem** — kalendarz, czasomierze, zadania
- **Narzędzia deweloperskie** — wykonywanie kodu, zapytania logów
- **Narzędzia użytkowe** — informacje systemowe, audyt Tokenów, dokumentacja pomocy, sieć wiedzy
- **Narzędzia przeglądarki** — automatyzacja przeglądarki WebView
- **Narzędzia wtyczek** — narzędzia firm trzecich rejestrowane przez system wtyczek

---

## Lista wbudowanych narzędzi

### 1. Narzędzie kalendarzowe (CalendarTool)

**Nazwa narzędzia**: `calendar`

**Opis funkcji**: Obsługa konwersji dat i obliczeń w 32 systemach kalendarzowych.

**Obsługiwane operacje**:
- `now` — uzyskanie bieżącego czasu
- `format` — formatowanie daty
- `add_days` — dodawanie/odejmowanie dni
- `diff` — obliczanie różnicy dat
- `list_calendars` — lista wszystkich obsługiwanych kalendarzy
- `get_components` — uzyskanie komponentów daty
- `get_now_components` — uzyskanie komponentów bieżącego czasu
- `convert` — konwersja między systemami kalendarzowymi

**Obsługiwane systemy kalendarzowe** (32):
- Gregoriański (Gregorian)
- Chiński księżycowy (Chinese Lunar)
- Chiński historyczny (Chinese Historical) — cykl ganzhi, ery cesarskie
- Islamski (Islamic)
- Hebrajski (Hebrew)
- Japoński (Japanese)
- Perski (Persian)
- Majański (Mayan)
- Buddyjski (Buddhist)
- Tybetański (Tibetan)
- i 24 inne kalendarze...

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

**Opis funkcji**: Zarządzanie sesjami czatu i wysyłanie wiadomości.

**Obsługiwane operacje**:
- `send_message` — wysłanie wiadomości
- `get_messages` — uzyskanie historii wiadomości
- `create_group` — utworzenie czatu grupowego
- `add_member` — dodanie członka grupy
- `remove_member` — usunięcie członka grupy
- `get_chat_info` — uzyskanie informacji o czacie
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

**Opis funkcji**: Odczyt i modyfikacja konfiguracji systemu.

**Obsługiwane operacje**:
- `read` — odczyt elementu konfiguracji
- `write` — zapis elementu konfiguracji
- `list` — lista wszystkich konfiguracji
- `get_ai_config` — uzyskanie konfiguracji klienta AI
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

**Nazwa narzędzia**: `curator`

**Wymagania uprawnień**: Tylko dla Kuratora Krzemowego

**Opis funkcji**: Narzędzie zarządzania systemem przeznaczone wyłącznie dla Kuratora Krzemowego.

**Obsługiwane operacje**:
- `create_being` — utworzenie nowej Istoty Krzemowej
- `list_beings` — lista wszystkich Istot Krzemowych
- `get_being_info` — uzyskanie informacji o istocie
- `assign_task` — przypisanie zadania
- `manage_permissions` — zarządzanie uprawnieniami

**Przykład użycia**:
```json
{
  "action": "create_being",
  "name": "Asystent",
  "soul_file": "assistant_soul.md"
}
```

---

### 5. Narzędzie bazy danych (DatabaseTool)

**Nazwa narzędzia**: `database`

**Opis funkcji**: Strukturalne zapytania bazodanowe i operacje.

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

**Opis funkcji**: Operacje na systemie plików i wyszukiwanie lokalne.

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
- `replace_text` — zamiana tekstu

**Wymagania uprawnień**: `disk:read`, `disk:write`

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

**Opis funkcji**: Dynamiczna kompilacja kodu C# (do samorozwoju Istot Krzemowych).

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

**Wymagania uprawnień**: Tylko dla Kuratora Krzemowego

**Opis funkcji**: Kompilacja i wykonywanie fragmentów kodu C#.

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

**Opis funkcji**: Uzyskanie dokumentacji pomocy systemu i przewodników użytkowania.

**Obsługiwane operacje**:
- `get_topics` — uzyskanie listy tematów pomocy
- `get_topic` — uzyskanie szczegółów konkretnego tematu
- `search` — wyszukiwanie dokumentacji pomocy

**Przykład użycia**:
```json
{
  "action": "get_topics"
}
```

---

### 10. Narzędzie sieci wiedzy (KnowledgeTool)

**Nazwa narzędzia**: `knowledge`

**Opis funkcji**: Operacje na grafie wiedzy (oparte na trójkach: podmiot-relacja-obiekt).

**Obsługiwane operacje**:
- `add` — dodanie trójki wiedzy
- `query` — zapytanie o wiedzę
- `update` — aktualizacja wiedzy
- `delete` — usunięcie wiedzy
- `search` — wyszukiwanie wiedzy
- `get_path` — uzyskanie ścieżki wiedzy
- `validate` — walidacja wiedzy
- `stats` — uzyskanie statystyk

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

**Opis funkcji**: Zapytania o historię operacji i historię rozmów.

**Obsługiwane operacje**:
- `query_logs` — zapytanie o logi systemowe
- `query_conversations` — zapytanie o historię rozmów
- `get_stats` — uzyskanie statystyk logów

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

**Opis funkcji**: Zarządzanie długoterminową i krótkoterminową pamięcią Istot Krzemowych.

**Obsługiwane operacje**:
- `read` — odczyt pamięci
- `write` — zapis pamięci
- `search` — wyszukiwanie pamięci
- `delete` — usunięcie pamięci
- `list` — lista pamięci
- `get_stats` — uzyskanie statystyk pamięci
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

**Opis funkcji**: Inicjowanie żądań HTTP/HTTPS.

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

**Wymagania uprawnień**: Tylko dla Kuratora Krzemowego

**Opis funkcji**: Zarządzanie uprawnieniami i listami kontroli dostępu.

**Obsługiwane operacje**:
- `query_permission` — zapytanie o uprawnienia
- `manage_acl` — zarządzanie globalnym ACL
- `get_callback` — uzyskanie funkcji wywołania zwrotnego uprawnień
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

### 15. Narzędzie projektów (ProjectTool)

**Nazwa narzędzia**: `project`

**Opis funkcji**: Zarządzanie obszarem roboczym projektów.

**Obsługiwane operacje**:
- `create` — utworzenie projektu
- `list` — lista projektów
- `get_info` — uzyskanie informacji o projekcie
- `update` — aktualizacja projektu
- `archive` — archiwizacja projektu

**Przykład użycia**:
```json
{
  "action": "create",
  "name": "My Project",
  "description": "Opis projektu"
}
```

---

### 16. Narzędzie zadań projektowych (ProjectTaskTool)

**Nazwa narzędzia**: `project_task`

**Opis funkcji**: Zarządzanie zadaniami projektowymi.

**Obsługiwane operacje**:
- `create` — utworzenie zadania
- `list` — lista zadań
- `update` — aktualizacja zadania
- `complete` — ukończenie zadania
- `get_stats` — uzyskanie statystyk zadań

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

### 17. Narzędzie notatek pracy projektów (ProjectWorkNoteTool)

**Nazwa narzędzia**: `project_work_note`

**Opis funkcji**: Zarządzanie notatkami pracy projektów (publiczne, podobne do notesu roboczego).

**Obsługiwane operacje**:
- `create` — utworzenie notatki
- `read` — odczyt notatki
- `update` — aktualizacja notatki
- `delete` — usunięcie notatki
- `list` — lista notatek
- `search` — wyszukiwanie notatek
- `directory` — generowanie katalogu

**Przykład użycia**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "Ukończono moduł uwierzytelniania użytkowników",
  "content": "## Szczegóły implementacji\n\n- Użycie tokena JWT",
  "keywords": "uwierzytelnianie,JWT"
}
```

---

### 18. Narzędzie systemowe (SystemTool)

**Nazwa narzędzia**: `system`

**Opis funkcji**: Uzyskanie informacji o systemie i użyciu zasobów.

**Obsługiwane operacje**:
- `info` — uzyskanie informacji o systemie
- `resource_usage` — uzyskanie użycia zasobów
- `find_process` — znalezienie procesu
- `list_beings` — lista Istot Krzemowych

**Przykład użycia**:
```json
{
  "action": "info"
}
```

---

### 19. Narzędzie zadań (TaskTool)

**Nazwa narzędzia**: `task`

**Opis funkcji**: Zarządzanie zadaniami osobistymi Istoty Krzemowej.

**Obsługiwane operacje**:
- `create` — utworzenie zadania
- `list` — lista zadań
- `update` — aktualizacja zadania
- `complete` — ukończenie zadania
- `delete` — usunięcie zadania
- `get_dependencies` — uzyskanie relacji zależności

**Przykład użycia**:
```json
{
  "action": "create",
  "description": "Przegląd kodu",
  "priority": 5
}
```

---

### 20. Narzędzie czasomierza (TimerTool)

**Nazwa narzędzia**: `timer`

**Opis funkcji**: Tworzenie i zarządzanie czasomierzami.

**Obsługiwane operacje**:
- `create` — utworzenie czasomierza
- `list` — lista czasomierzy
- `delete` — usunięcie czasomierza
- `pause` — wstrzymanie czasomierza
- `resume` — wznowienie czasomierza
- `get_execution_history` — uzyskanie historii wykonania

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

### 21. Narzędzie audytu Tokenów (TokenAuditTool) 🔒

**Nazwa narzędzia**: `token_audit`

**Wymagania uprawnień**: Tylko dla Kuratora Krzemowego

**Opis funkcji**: Zapytania i podsumowanie użycia tokenów AI.

**Obsługiwane operacje**:
- `get_usage` — uzyskanie statystyk użycia tokenów
- `get_by_being` — uzyskanie użycia według istoty
- `get_by_model` — uzyskanie użycia według modelu
- `get_trend` — uzyskanie trendu użycia
- `export` — eksport danych

**Przykład użycia**:
```json
{
  "action": "get_usage",
  "start_date": "2026-04-01",
  "end_date": "2026-04-26"
}
```

---

### 22. Narzędzie przeglądarki WebView (WebViewBrowserTool)

**Nazwa narzędzia**: `webview`

**Opis funkcji**: Automatyzacja przeglądarki oparta na Playwright.

**Obsługiwane operacje**:
- `open_browser` — otwarcie przeglądarki
- `close_browser` — zamknięcie przeglądarki
- `navigate` — nawigacja do URL
- `click` — kliknięcie elementu
- `input` — wprowadzenie tekstu
- `get_page_text` — uzyskanie tekstu strony
- `get_screenshot` — uzyskanie zrzutu ekranu
- `execute_script` — wykonanie JavaScript
- `wait_for_element` — oczekiwanie na pojawienie się elementu
- `get_browser_status` — uzyskanie stanu przeglądarki

**Cechy**:
- Niezależna instancja dla każdej Istoty Krzemowej
- Całkowicie odizolowane ciasteczka i sesje
- Całkowicie niewidoczne dla użytkownika (tryb bezgłowy)
- Pełna obsługa JavaScript i CSS

**Przykład użycia**:
```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

---

### 23. Narzędzie notatek pracy (WorkNoteTool)

**Nazwa narzędzia**: `work_note`

**Opis funkcji**: Zarządzanie osobistymi notatkami pracy Istoty Krzemowej (prywatne, podobne do dziennika).

**Obsługiwane operacje**:
- `create` — utworzenie notatki
- `read` — odczyt notatki
- `update` — aktualizacja notatki
- `delete` — usunięcie notatki
- `list` — lista notatek
- `search` — wyszukiwanie notatek
- `directory` — generowanie katalogu

**Przykład użycia**:
```json
{
  "action": "create",
  "summary": "Ukończono moduł uwierzytelniania użytkowników",
  "content": "## Szczegóły implementacji\n\n- Użycie tokena JWT\n- Obsługa OAuth2",
  "keywords": "uwierzytelnianie,JWT,OAuth2"
}
```

---

### 24. Narzędzie gorącego przeładowania (HotReloadTool)

**Nazwa narzędzia**: `hot_reload`

**Opis funkcji**: Obsługa automatycznej kompilacji, aktualizacji plików i restartu SiliconLife.Fast w czasie działania, bez ręcznej interwencji.

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

## Przepływ wywołania narzędzi

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
│   AI     │ Otrzymuje wynik narzędzia, kontynuuje myślenie
└──────────┘
```

## Weryfikacja uprawnień

Wszystkie wykonania narzędzi przechodzą przez 3-poziomowy łańcuch uprawnień:

1. **UserFrequencyCache** — pamięć podręczna częstych zezwoleń/odmów użytkownika
2. **IPermissionCallback** — niestandardowa funkcja wywołania zwrotnego uprawnień
3. **Rozgałęzienie** — IsCurator: `IPermissionAskHandler` (zapytanie użytkownika) | Non-curator: `GlobalACL` (lista kontroli dostępu)

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

Umieść plik narzędzia w katalogu `src/SiliconLife.Common/Tools/` (narzędzia współdzielone). `ToolManager` automatycznie odkryje i zarejestruje narzędzie podczas uruchamiania poprzez refleksję.

> **Uwaga**: `SiliconLife.Default` i `SiliconLife.Fast` nie mają już oddzielnych katalogów `Tools/`, wszystkie współdzielone narzędzia są umieszczane w `SiliconLife.Common/Tools/`.

### Krok 2a: Rejestracja narzędzia przez wtyczkę

Można również zarejestrować niestandardowe narzędzia przez system wtyczek:

1. Zaimplementuj interfejs `ITool` w projekcie wtyczki
2. Skompiluj DLL wtyczki i umieść w katalogu wtyczek
3. `ToolManager.ScanAllPluginAssemblies()` automatycznie przeskanuje wszystkie załadowane wtyczki w poszukiwaniu implementacji ITool
4. Narzędzia wtyczek podlegają tym samym ograniczeniom systemu uprawnień

### Krok 3: (Opcjonalnie) Oznaczenie jako przeznaczone tylko dla Kuratora

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
    // Wykonanie operacji
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
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return ToolResult.Denied(permission.Reason);
}
```

### 4. Zapewnij jasne opisy narzędzi

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

**Problem**: Wykonanie narzędzia nie powiodło się, zwrócono błąd uprawnień.

**Rozwiązanie**:
- Sprawdź dziennik audytu uprawnień
- Zweryfikuj, czy Istota Krzemowa posiada wymagane uprawnienia
- Sprawdź ustawienia globalnego ACL
- Jeśli jesteś Kuratorem, sprawdź, czy użyto atrybutu `[SiliconManagerOnly]`

### Narzędzie zwraca błąd wykonania

**Problem**: Narzędzie zostało wykonane, ale zwróciło wynik niepowodzenia.

**Rozwiązanie**:
- Sprawdź komunikat błędu zwrócony przez narzędzie
- Zweryfikuj poprawność formatu parametrów wejściowych
- Zobacz logi systemowe, aby uzyskać szczegółowe informacje o błędzie
- Przetestuj funkcjonalność narzędzia niezależnie

## Następne kroki

- 📚 Przeczytaj [przewodnik architektury](architecture.md)
- 🛠️ Zobacz [przewodnik programistyczny](development-guide.md)
- 🔒 Poznaj [system uprawnień](permission-system.md)
- 🚀 Zobacz [przewodnik szybkiego startu](getting-started.md)
