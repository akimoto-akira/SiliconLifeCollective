# System uprawnień

> **Wersja: v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | [Русский](../ru-RU/permission-system.md)

## Przegląd

System uprawnień zapewnia, że wszystkie operacje inicjowane przez AI przechodzą przez odpowiednią weryfikację i audyt.

## Łańcuch weryfikacji uprawnień

```
┌─────────────────────────────────────────────┐
│          Weryfikacja uprawnień               │
├─────────────────────────────────────────────┤
│  Poziom 1: UserFrequencyCache                │
│  ↓ Pamięć podręczna decyzji użytkownika o wysokiej częstotliwości (HighDeny/HighAllow) │
│  Poziom 2: IPermissionCallback               │
│  ↓ Niestandardowa logika (Allowed/Denied/AskUser) │
│  Poziom 3: IsCurator?                        │
│  ↓ Tak → IPermissionAskHandler (zapytaj użytkownika) │
│  ↓ Nie → GlobalACL → domyślnie odmowa       │
│  Wynik: zezwolenie lub odmowa                │
└─────────────────────────────────────────────┘
```

> **Uwaga**: Rzeczywisty priorytet zapytań `PermissionManager.CheckPermission()` to:
> 1. **UserFrequencyCache** — najpierw sprawdzana jest pamięć podręczna decyzji użytkownika o wysokiej częstotliwości
> 2. **IPermissionCallback** — ocena niestandardowych reguł wywołania zwrotnego
> 3. **Gałąź Kuratora** — gdy wywołanie zwrotne zwraca AskUser lub brak wywołania zwrotnego:
>    - **Kurator** → `IPermissionAskHandler` (zapytanie do użytkownika przez IM)
>    - **Nie-Kurator** → `GlobalACL` → domyślnie odmowa

## Poziom 1: UserFrequencyCache

Pamięć podręczna decyzji użytkownika o wysokiej częstotliwości (HighDeny/HighAllow) dla każdej istoty, istnieje tylko w pamięci.

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny ma priorytet nad HighAllow**
- **Tylko w pamięci**: pamięć podręczna nie jest utrwalana, po restarcie jest tracona
- **Konfigurowalny czas wygaśnięcia**: użytkownik może ustawić okres ważności wpisów pamięci podręcznej

## Poziom 2: IPermissionCallback

Niestandardowe wywołanie zwrotne do dynamicznej logiki uprawnień.

### Domyślna implementacja DefaultPermissionCallback

`DefaultPermissionCallback` zapewnia kompleksowe domyślne reguły uprawnień, w tym:

#### Reguły dostępu sieciowego
- **Adresy pętli zwrotnej**: zezwolenie na localhost, 127.0.0.1, ::1
- **Prywatne adresy IP**:
  - 192.168.x.x (Klasa C) - zezwolenie
  - 10.x.x.x (Klasa A) - zezwolenie
  - 172.16-31.x.x (Klasa B) - zapytaj użytkownika
- **Biała lista domen**:
  - Wyszukiwarki: Google, Bing, DuckDuckGo, Yandex, Sogou itp.
  - Usługi AI: OpenAI, Anthropic, HuggingFace, Ollama itp.
  - Usługi deweloperskie: GitHub, StackOverflow, npm, NuGet itp.
  - Media społecznościowe: Weibo, Zhihu, Reddit, Discord itp.
  - Platformy wideo: YouTube, Bilibili, Douyin, TikTok itp.
  - **Informacje pogodowe**: wttr.in
  - Strony rządowe: .gov, .go.jp, .go.kr
- **Czarna lista domen**:
  - Witryny podszywające się pod AI: chatgpt, openai, deepseek i inne domeny imitujące
  - Złośliwe narzędzia AI: wormgpt, darkgpt, fraudgpt itp.
  - Farmy treści AI i domeny związane z czarnym rynkiem

```csharp
public class DefaultPermissionCallback : IPermissionCallback
{
    public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
    {
        if (IsSafeOperation(permissionType, resource))
        {
            return PermissionResult.Allowed;
        }
        
        return PermissionResult.AskUser;
    }
}
```

## Poziom 3: Rozgałęzienie (IsCurator / GlobalACL)

Gdy wywołanie zwrotne zwraca `AskUser` lub nie ma skonfigurowanego wywołania zwrotnego, system rozgałęzia się na podstawie statusu Kuratora:

### Gałąź Kuratora (IsCurator = true)

Dla Kuratora Krzemowego system prosi użytkownika o decyzję przez komunikator:

```csharp
if (IsCurator)
{
    if (_askHandler != null)
    {
        AskPermissionResult userDecision = _askHandler.AskUser(callerId, permissionType, resource);
        // Użytkownik potwierdza lub odmawia w Web UI
    }
}
```

### Gałąź nie-Kuratora (IsCurator = false)

Dla istot nie-Kuratorów system sprawdza Globalną Listę Kontroli Dostępu. Jeśli nie ma pasującej reguły, domyślnie odmawia żądania.

### Struktura GlobalACL

```json
{
  "rules": [
    {
      "permissionType": "NetworkAccess",
      "resourcePrefix": "api.github.com",
      "result": "Allowed"
    },
    {
      "permissionType": "FileAccess",
      "resourcePrefix": "C:\\Windows",
      "result": "Denied"
    }
  ]
}
```

Reguły są oceniane w kolejności, pierwsza pasująca reguła ma zastosowanie. Tylko Kurator Krzemowy może modyfikować Globalną ACL.

### Format zasobu

```
{typ}:{ścieżka}

Przykłady:
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## IPermissionAskHandler

Gdy operacja Kuratora wymaga potwierdzenia użytkownika, `IPermissionAskHandler` pyta o uprawnienia.

### Implementacja IMPermissionAskHandler

`IMPermissionAskHandler` wysyła żądanie uprawnień do użytkownika przez Web UI:

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        // Wysyłanie wiadomości do użytkownika przez komunikator
        SendMessageAsync($"Zezwolić na {resource}?");

        // Oczekiwanie na odpowiedź użytkownika
        var response = WaitForResponseAsync();

        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

### Kolejka żądań uprawnień PermissionRequestQueue

`PermissionRequestQueue` zarządza oczekującymi żądaniami uprawnień, obsługując asynchroniczne oczekiwanie na odpowiedź użytkownika:

- **Kolejkowanie żądań** — gdy łańcuch uprawnień osiągnie poziom 5, tworzy `TaskCompletionSource<AskPermissionResult>` i dodaje do kolejki
- **Wyświetlanie w Web UI** — przez `PermissionRequestController` wyświetla oczekujące żądania uprawnień w Web UI
- **Odpowiedź użytkownika** — użytkownik zatwierdza lub odmawia w Web UI, z opcją buforowania decyzji i ustawienia czasu trwania pamięci podręcznej
- **Opcje pamięci podręcznej** — użytkownik może buforować decyzje uprawnień na 1 godzinę, 24 godziny, 7 dni lub 30 dni
- **Mechanizm timeout** — brak odpowiedzi przez 60 sekund automatycznie zamyka stronę żądania

## System audytu

Wszystkie decyzje dotyczące uprawnień są rejestrowane:

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "callerId": "being-uuid",
  "permissionType": "FileAccess",
  "resource": "C:\\data\\config.json",
  "result": "Allowed",
  "reason": "Global ACL"
}
```

## Programowa ocena uprawnień

### EvaluatePermission API

Metoda `PermissionManager.EvaluatePermission()` zapewnia ocenę wstępną uprawnień tylko do odczytu, nie wyzwalając monitów dla użytkownika. `PermissionTool` wykorzystuje tę metodę, aby pozwolić AI sprawdzić status uprawnień przed próbą operacji.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**Wartość zwracana**: trójstanowe `PermissionResult`:
- `Allowed` - operacja jest dozwolona
- `Denied` - operacja jest odrzucona
- `AskUser` - wymagane potwierdzenie użytkownika przy wykonaniu

**Kolejność oceny**:
1. **Pamięć podręczna częstotliwości** - sprawdzenie zbuforowanych decyzji użytkownika
2. **IPermissionCallback** - ocena niestandardowego wywołania zwrotnego
3. **Status Kuratora** - jeśli Kurator, zwraca `AskUser` (wymagane potwierdzenie)
4. **Globalna ACL** - sprawdzenie reguł kontroli dostępu
5. **Domyślnie** - odmowa przy braku pasującej reguły

> **Uwaga**: W przeciwieństwie do pełnego łańcucha uprawnień, `EvaluatePermission` **nie wywołuje** `IPermissionAskHandler`. Raportuje jedynie, jaki wynik *będzie* przy wykonaniu.

## Zarządzanie uprawnieniami

### Nadawanie uprawnień

**Przez Web UI**:
1. Przejdź do **Zarządzanie uprawnieniami**
2. Kliknij **Dodaj regułę**
3. Skonfiguruj:
   - Użytkownik
   - Zasób
   - Zezwól/Odmów
   - Czas trwania

**Przez API**:
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "permissionType": "FileAccess",
    "resourcePrefix": "C:\\Projects",
    "result": "Allowed",
    "description": "Allow project directory access"
  }'
```

### Odbieranie uprawnień

Przez stronę zarządzania uprawnieniami w Web UI.

### Przeglądanie uprawnień

```bash
curl http://localhost:8080/api/permissions/list
```

## System uprawnień narzędzi

Oprócz łańcucha weryfikacji uprawnień na poziomie operacji, system zapewnia mechanizm zarządzania **uprawnieniami narzędzi**, kontrolujący które narzędzia mogą być używane przez Istoty Krzemowe.

### Dwupoziomowe uprawnienia narzędzi

Uprawnienia narzędzi są podzielone na dwa poziomy:

1. **Poziom Istoty Krzemowej** — kontroluje, które operacje narzędzi może używać pojedyncza Istota Krzemowa
2. **Poziom projektu** — kontroluje operacje narzędzi dostępne w przestrzeni projektowej, niezależnie od uprawnień na poziomie Istoty Krzemowej

### Konfiguracja uprawnień narzędzi

Każda operacja każdego narzędzia może być niezależnie skonfigurowana jako dozwolona lub odrzucona:

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "network:get": "allowed",
    "network:post": "denied",
    "disk:read": "allowed",
    "disk:write": "denied",
    "database:query": "allowed"
  }
}
```

### Szablony uprawnień

System dostarcza predefiniowane szablony uprawnień narzędzi, które można szybko zastosować do Istot Krzemowych:

- **readonly** — uprawnienia tylko do odczytu (zezwalają na operacje odczytu, odmawiają operacji zapisu)
- **full** — pełne uprawnienia (zezwalają na wszystkie operacje)
- **restricted** — ograniczone uprawnienia (zezwalają tylko na podstawowe operacje)

### Zarządzanie przez Web UI

Zarządzanie uprawnieniami narzędzi przez Web UI:

- **Strona uprawnień narzędzi Istoty Krzemowej** — `/beings/tool-permissions`
- **Strona uprawnień narzędzi projektu** — `/project/{id}/tool-permissions`

### Endpointy API

| Endpoint | Metoda | Opis |
|------|------|------|
| `/api/beings/tool-permissions` | GET | Pobierz uprawnienia narzędzi Istoty Krzemowej |
| `/api/beings/tool-permissions` | PUT | Aktualizuj uprawnienia narzędzi Istoty Krzemowej |
| `/api/beings/tool-permissions/templates` | GET | Pobierz listę szablonów uprawnień |
| `/api/beings/tool-permissions/apply-template` | POST | Zastosuj szablon uprawnień |
| `/api/projects/{id}/tool-permissions` | GET | Pobierz uprawnienia narzędzi projektu |
| `/api/projects/{id}/tool-permissions` | PUT | Aktualizuj uprawnienia narzędzi projektu |

### Uprawnienia akcji umiejętności

Umiejętności wykorzystują mechanizm uprawnień akcji narzędzi: id umiejętności jest używane jako nazwa narzędzia, a akcja to `execute`.

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "daily_news_digest:execute": "denied",
    "code_review:execute": "allowed"
  }
}
```

- Wyłączone umiejętności nie pojawiają się w definicjach narzędzi widocznych dla AI (AI ich w ogóle "nie widzi")
- Wykonywanie umiejętności posiada dodatkową weryfikację w czasie wykonywania; nawet nieaktualne schematy nie mogą jej obejść
- Uprawnienia narzędzi wewnętrznych umiejętności = uprawnienia Istoty ∪ ograniczenia samej umiejętności (przecięcie strony restrykcyjnej; może tylko zawężać, nigdy rozszerzać)

### Uprawnienia akcji narzędzi opakowujących MCP

Każde narzędzie opakowujące wstrzyknięte przez serwer MCP (`mcp_{serverId}_{toolName}`) automatycznie deklaruje pojedynczą akcję `execute`:

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "mcp_filesystem_read_file:execute": "denied",
    "mcp_github_create_issue:execute": "allowed"
  }
}
```

- Można precyzyjnie kontrolować dostępność narzędzi zewnętrznych per Istota lub per projekt
- Gdy wszystkie akcje `execute` serwera są zabronione, narzędzie jest całkowicie usuwane ze schematu widocznego dla AI
- Wyłączenie/usunięcie serwera (operacja w Web UI) natychmiast wyrejestrowuje wszystkie jego narzędzia

---

## Najlepsze praktyki

### 1. Zasada najmniejszych uprawnień

Nadawaj tylko minimalne wymagane uprawnienia:

```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects\\MyApp\\config.json",
  "result": "Allowed"
}
```

### 2. Używaj uprawnień ograniczonych czasowo

Nigdy nie nadawaj stałych uprawnień, chyba że jest to absolutnie konieczne.

### 3. Monitoruj dzienniki uprawnień

Regularnie przeglądaj dzienniki audytu, aby zrozumieć:
- Odrzucone próby dostępu
- Nietypowe wzorce
- Eskalację uprawnień

### 4. Implementuj niestandardowe wywołania zwrotne

Dla złożonej logiki używaj `IPermissionCallback`:

```csharp
public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
{
    // Uprawnienia oparte na czasie
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }
    
    // Uprawnienia oparte na zasobach
    if (IsSensitiveResource(resource))
    {
        return PermissionResult.AskUser;
    }
    
    return PermissionResult.Allowed;
}
```

## Typowe scenariusze

### Scenariusz 1: AI chce odczytać plik

```
AI: "Muszę odczytać config.json"
↓
Łańcuch uprawnień:
1. UserFrequencyCache? Brak zbuforowanej decyzji
2. IPermissionCallback? Zwraca AskUser (niewyraźnie dozwolone)
3. IsCurator? Nie → sprawdź GlobalACL
4. GlobalACL? Znaleziono regułę: file:... = Allowed
5. Wynik: zezwolenie
```

### Scenariusz 2: AI chce wykonać kod

```
AI: "Chcę skompilować i uruchomić kod"
↓
Łańcuch uprawnień:
1. UserFrequencyCache? Brak zbuforowanej decyzji
2. IPermissionCallback? Zwraca AskUser
3. IsCurator? Tak → IPermissionAskHandler
4. Użytkownik zatwierdza
5. Wynik: zezwolenie
```

### Scenariusz 3: Odmowa z pamięci podręcznej

```
AI: "Muszę uzyskać dostęp do C:\Windows"
↓
Łańcuch uprawnień:
1. UserFrequencyCache? Znaleziono w pamięci podręcznej HighDeny
2. Wynik: odmowa (bez dalszego sprawdzania)
```

## Rozwiązywanie problemów

### Nieoczekiwana odmowa uprawnień

**Sprawdź**:
1. Status IsCurator użytkownika
2. Wpisy HighDeny w pamięci podręcznej częstotliwości
3. Reguły GlobalACL
4. Logikę wywołania zwrotnego
5. Timeout odpowiedzi użytkownika

### Uprawnienia nie wygasają

**Sprawdź**:
- Pole `expiresAt` jest poprawnie ustawione
- Strefa czasowa jest poprawna
- Zegary są zsynchronizowane

### Dziennik audytu nie rejestruje

**Sprawdź**:
- Audytor logów jest zarejestrowany
- Backend przechowywania jest dostępny
- Dostępna wystarczająca przestrzeń na dysku

## Następne kroki

- 📚 Przeczytaj [przewodnik architektury](architecture.md)
- 🛠️ Zobacz [przewodnik deweloperski](development-guide.md)
- 🔒 Zobacz [dokumentację bezpieczeństwa](security.md)
- 🚀 Zobacz [przewodnik szybkiego startu](getting-started.md)
