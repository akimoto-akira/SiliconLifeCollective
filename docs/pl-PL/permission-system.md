# System uprawnień

> **Wersja: v0.1.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | [Polski](../pl-PL/permission-system.md)

## Przegląd

System uprawnień zapewnia, że wszystkie operacje inicjowane przez AI są odpowiednio weryfikowane i audytowane.

## Łańcuch uprawnień 5 poziomów

```
┌─────────────────────────────────────────────┐
│          Weryfikacja uprawnień              │
├─────────────────────────────────────────────┤
│  Poziom 1: IsCurator                        │
│  ↓ Jeśli prawda, pomija                     │
│  Poziom 2: UserFrequencyCache               │
│  ↓ Ograniczenie szybkości                   │
│  Poziom 3: GlobalACL                        │
│  ↓ Lista kontroli dostępu                   │
│  Poziom 4: IPermissionCallback              │
│  ↓ Niestandardowa logika                    │
│  Poziom 5: IPermissionAskHandler            │
│  ↓ Zapytanie użytkownika                    │
│  Wynik: Zezwól lub Odmów                   │
└─────────────────────────────────────────────┘
```

## Poziom 1: IsCurator

Administrator/Kurator pomija wszystkie sprawdzanie uprawnień.

```csharp
if (user.IsCurator)
{
    return PermissionResult.Allowed("Curator access");
}
```

## Poziom 2: UserFrequencyCache

Ograniczenie szybkości dla każdego użytkownika w celu zapobiegania nadużyciom.

```csharp
var cache = new UserFrequencyCache();
if (!cache.CheckLimit(userId, resource))
{
    return PermissionResult.Denied("Rate limit exceeded");
}
```

## Poziom 3: GlobalACL

Globalna lista kontroli dostępu definiująca jawne reguły.

### Struktura ACL

```json
{
  "rules": [
    {
      "userId": "user-uuid",
      "resource": "disk:read",
      "allowed": true,
      "expiresAt": "2026-04-21T00:00:00Z"
    }
  ]
}
```

### Format zasobów

```
{typ}:{akcja}

Przykłady:
- disk:read
- disk:write
- network:http
- compile:execute
- system:info
```

## Poziom 4: IPermissionCallback

Niestandardowe wywołanie zwrotne dla dynamicznej logiki uprawnień.

### Domyślna implementacja DefaultPermissionCallback

`DefaultPermissionCallback` zapewnia kompleksowe domyślne reguły uprawnień, w tym:

#### Reguły dostępu sieciowego
- **Adresy pętli zwrotnej**: zezwolenie na localhost, 127.0.0.1, ::1
- **Prywatne adresy IP**:
  - 192.168.x.x (Klasa C) - zezwolenie
  - 10.x.x.x (Klasa A) - zezwolenie
  - 172.16-31.x.x (Klasa B) - zapytanie użytkownika
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
    public async Task<PermissionResult> CheckAsync(PermissionRequest request)
    {
        // Niestandardowa logika
        if (IsSafeOperation(request))
        {
            return PermissionResult.Allowed("Safe operation");
        }

        return PermissionResult.Undecided("Needs user confirmation");
    }
}
```

## Poziom 5: IPermissionAskHandler

Zapytanie użytkownika o uprawnienia, gdy wszystkie inne poziomy są nierozstrzygnięte.

### Implementacja IMPermissionAskHandler

`IMPermissionAskHandler` wysyła żądania uprawnień do użytkownika przez Web UI:

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public async Task<AskPermissionResult> AskAsync(PermissionRequest request)
    {
        // Wysłanie wiadomości do użytkownika przez komunikację natychmiastową
        await SendMessageAsync($"Zezwolić na {request.Resource}?");

        // Oczekiwanie na odpowiedź użytkownika
        var response = await WaitForResponseAsync();

        return response.Approved
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

### Kolejka żądań uprawnień PermissionRequestQueue

`PermissionRequestQueue` zarządza oczekującymi żądaniami uprawnień, obsługując asynchroniczne oczekiwanie na odpowiedź użytkownika:

- **Kolejkowanie żądań** — gdy łańcuch uprawnień osiągnie poziom 5, tworzy `TaskCompletionSource<AskPermissionResult>` i kolejkuje
- **Wyświetlanie w Web UI** — wyświetla oczekujące żądania uprawnień w Web UI przez `PermissionRequestController`
- **Odpowiedź użytkownika** — użytkownik zatwierdza lub odrzuca w Web UI, z opcją buforowania decyzji i ustawienia czasu trwania bufora
- **Opcje buforowania** — użytkownik może buforować decyzje uprawnień na 1 godzinę, 24 godziny, 7 dni lub 30 dni
- **Mechanizm przekroczenia czasu** — automatyczne zamknięcie strony żądania po 60 sekundach braku odpowiedzi

## System audytu

Wszystkie decyzje dotyczące uprawnień są rejestrowane:

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "level": "GlobalACL",
  "reason": "Explicit rule granted"
}
```

## Programowa ocena uprawnień

### API EvaluatePermission

Metoda `PermissionManager.EvaluatePermission()` zapewnia ocenę wstępną uprawnień tylko do odczytu, nie wyzwalając monitów użytkownika. `PermissionTool` używa tej metody, aby pozwolić AI sprawdzić stan uprawnień przed próbą operacji.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**Wartość zwracana**: Trójstanowy `PermissionResult`:
- `Allowed` - operacja jest dozwolona
- `Denied` - operacja jest odrzucona
- `AskUser` - wymagane potwierdzenie użytkownika przy wykonaniu

**Kolejność oceny**:
1. **Pamięć podręczna częstotliwości** - sprawdzenie buforowanych decyzji użytkownika
2. **IPermissionCallback** - ocena niestandardowego wywołania zwrotnego
3. **Status kuratora** - jeśli to kurator, zwraca `AskUser` (wymaga potwierdzenia)
4. **Globalne ACL** - sprawdzenie reguł kontroli dostępu
5. **Domyślnie** - odrzucenie, gdy brak pasującej reguły

> **Uwaga**: W przeciwieństwie do pełnego łańcucha uprawnień, `EvaluatePermission` **nie wywołuje** `IPermissionAskHandler`. Raportuje jedynie, jaki wynik *będzie* przy wykonaniu.

## Zarządzanie uprawnieniami

### Nadawanie uprawnień

**Przez Web UI**:
1. Przejdź do **Zarządzanie uprawnieniami**
2. Kliknij **Dodaj regułę**
3. Skonfiguruj:
   - Użytkownika
   - Zasób
   - Zezwól/Odmów
   - Czas trwania

**Przez API**:
```bash
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "user-uuid",
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

### Odwoływanie uprawnień

```bash
curl -X DELETE http://localhost:8080/api/permissions/{rule-id}
```

### Przegląd uprawnień

```bash
curl http://localhost:8080/api/permissions?userId=user-uuid
```

## Najlepsze praktyki

### 1. Zasada najmniejszych uprawnień

Nadawaj tylko minimalne wymagane uprawnienia:

```json
{
  "resource": "disk:read",  // nie disk:*
  "allowed": true,
  "expiresAt": "2026-04-21T00:00:00Z"  // zawsze ustawiaj wygaśnięcie
}
```

### 2. Używaj uprawnień z ograniczeniem czasowym

Nigdy nie nadawaj stałych uprawnień, chyba że jest to absolutnie konieczne.

### 3. Monitoruj logi uprawnień

Regularnie przeglądaj logi audytu, aby zrozumieć:
- Odrzucone próby dostępu
- Nietypowe wzorce
- Eskalację uprawnień

### 4. Implementuj niestandardowe wywołania zwrotne

Dla złożonej logiki używaj `IPermissionCallback`:

```csharp
public async Task<PermissionResult> CheckAsync(PermissionRequest request)
{
    // Uprawnienia oparte na czasie
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied("Outside business hours");
    }

    // Uprawnienia oparte na zasobach
    if (IsSensitiveResource(request.Resource))
    {
        return PermissionResult.Undecided("Requires approval");
    }

    return PermissionResult.Allowed();
}
```

## Typowe scenariusze

### Scenariusz 1: AI chce odczytać plik

```
AI: "Muszę odczytać config.json"
↓
Łańcuch uprawnień:
1. IsCurator? Nie
2. Ograniczenie szybkości? Normalne
3. GlobalACL? Znaleziono regułę: disk:read = zezwolone
4. Wynik: Zezwól
```

### Scenariusz 2: AI chce wykonać kod

```
AI: "Chcę skompilować i uruchomić kod"
↓
Łańcuch uprawnień:
1. IsCurator? Nie
2. Ograniczenie szybkości? Normalne
3. GlobalACL? Nie znaleziono reguły
4. Wywołanie zwrotne? Zwraca nierozstrzygnięte
5. Zapytanie użytkownika? Użytkownik zatwierdza
6. Wynik: Zezwól
```

### Scenariusz 3: Przekroczenie ograniczenia szybkości

```
AI: "Muszę wysłać 100 żądań HTTP"
↓
Łańcuch uprawnień:
1. IsCurator? Nie
2. Ograniczenie szybkości? Przekroczone
3. Wynik: Odmów
```

## Rozwiązywanie problemów

### Nieoczekiwane odmowy uprawnień

**Sprawdź**:
1. Status IsCurator użytkownika
2. Ustawienia ograniczenia szybkości
3. Reguły GlobalACL
4. Logikę wywołania zwrotnego
5. Przekroczenie czasu odpowiedzi użytkownika

### Uprawnienia nie wygasają

**Sprawdź**:
- Pole `expiresAt` jest ustawione poprawnie
- Strefa czasowa jest poprawna
- Zegar jest zsynchronizowany

### Logi audytu nie są rejestrowane

**Sprawdź**:
- Rejestrator audytu jest zarejestrowany
- Backend przechowywania jest dostępny
- Wystarczająca ilość miejsca na dysku

## Następne kroki

- 📚 Przeczytaj [Przewodnik architektury](architecture.md)
- 🛠️ Zobacz [Przewodnik rozwoju](development-guide.md)
- 🔒 Zobacz [Dokumentację bezpieczeństwa](security.md)
- 🚀 Zobacz [Przewodnik szybkiego startu](getting-started.md)
