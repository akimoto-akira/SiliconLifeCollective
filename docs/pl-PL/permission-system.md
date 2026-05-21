# System uprawnień

> **Wersja: v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | [Polski](../pl-PL/permission-system.md)

## Przegląd

System uprawnień zapewnia, że wszystkie operacje inicjowane przez AI są odpowiednio weryfikowane i audytowane.

## Łańcuch uprawnień 3 poziomów

```
┌─────────────────────────────────────────────┐
│          Weryfikacja uprawnień              │
├─────────────────────────────────────────────┤
│  Poziom 1: UserFrequencyCache               │
│  ↓ Ograniczenie szybkości / buforowane      │
│  Poziom 2: IPermissionCallback              │
│  ↓ Niestandardowa logika                    │
│  Poziom 3: Rozgałęzienie                    │
│  ├─ IsCurator → IPermissionAskHandler       │
│  │  ↓ Kurator: zapytanie użytkownika        │
│  └─ Non-curator → GlobalACL                 │
│     ↓ Nie-kurator: lista kontroli dostępu   │
│  Wynik: Zezwól lub Odmów                   │
└─────────────────────────────────────────────┘
```

## Poziom 1: UserFrequencyCache

Pamięć podręczna buforująca poprzednie decyzje użytkownika, aby uniknąć powtarzających się monitów. **HighDeny** ma priorytet nad **HighAllow**. Pamięć podręczna jest tylko w pamięci, tracona po restarcie.

```csharp
var cache = new UserFrequencyCache();
if (cache.CheckLimit(callerId, resource, out var cachedResult))
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

## Poziom 2: IPermissionCallback

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
    public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
    {
        // Niestandardowa logika
        if (IsSafeOperation(permissionType, resource))
        {
            return PermissionResult.Allowed;
        }

        return PermissionResult.AskUser;
    }
}
```

## Poziom 3: Rozgałęzienie (IsCurator / GlobalACL)

Gdy poziom 1 i poziom 2 nie podejmą decyzji, system rozgałęzia się na podstawie tożsamości wywołującego:

### Gałąź kuratora (IsCurator = true)

Jeśli wywołujący jest kuratorem, system pyta użytkownika o decyzję za pośrednictwem komunikacji natychmiastowej:

```csharp
if (IsCurator)
{
    if (_askHandler != null)
    {
        var result = _askHandler.AskUser(callerId, permissionType, resource);
    }
}
```

### Gałąź nie-kuratora (IsCurator = false)

Jeśli wywołujący nie jest kuratorem, sprawdza `GlobalACL` listę kontroli dostępu. Jeśli nie znaleziono pasującej reguły, żądanie jest domyślnie odrzucane.

### Struktura GlobalACL

```json
{
  "rules": [
    {
      "prefix": "network:api.github.com",
      "result": "Allowed"
    },
    {
      "prefix": "file:C:\\Windows",
      "result": "Denied"
    },
    {
      "prefix": "cli:rm -rf",
      "result": "Denied"
    }
  ]
}
```

Reguły są oceniane w kolejności; pierwsze dopasowanie wygrywa. Tylko Kurator Krzemowy może modyfikować globalne ACL.

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

## IPermissionAskHandler

Zapytanie użytkownika o uprawnienia, gdy wszystkie inne poziomy są nierozstrzygnięte.

### Implementacja IMPermissionAskHandler

`IMPermissionAskHandler` wysyła żądania uprawnień do użytkownika przez Web UI:

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        // Wysłanie wiadomości do użytkownika przez komunikację natychmiastową
        SendMessage($"Zezwolić na {resource}?");

        // Oczekiwanie na odpowiedź użytkownika
        var response = WaitForResponse();

        return new AskPermissionResult
        {
            Allowed = response.Approved,
            AddToCache = response.AddToCache,
            CacheDuration = response.CacheDuration
        };
    }
}
```

### Kolejka żądań uprawnień PermissionRequestQueue

`PermissionRequestQueue` zarządza oczekującymi żądaniami uprawnień, obsługując asynchroniczne oczekiwanie na odpowiedź użytkownika:

- **Kolejkowanie żądań** — gdy łańcuch uprawnień osiąga rozgałęzienie IsCurator, tworzy `TaskCompletionSource<AskPermissionResult>` i kolejkuje
- **Wyświetlanie w Web UI** — wyświetla oczekujące żądania uprawnień w Web UI przez `PermissionRequestController`
- **Odpowiedź użytkownika** — użytkownik zatwierdza lub odrzuca w Web UI, z opcją buforowania decyzji i ustawienia czasu trwania bufora
- **Opcje buforowania** — użytkownik może buforować decyzje uprawnień na 1 godzinę, 24 godziny, 7 dni lub 30 dni
- **Mechanizm przekroczenia czasu** — automatyczne zamknięcie strony żądania po 30 sekundach braku odpowiedzi

## System audytu

Wszystkie decyzje dotyczące uprawnień są rejestrowane:

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "level": "GlobalACL",
  "reason": "Prefix match: disk:write"
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
3. **Rozgałęzienie kuratora** - jeśli to kurator, zwraca `AskUser` (wymaga potwierdzenia); jeśli nie-kurator, sprawdza **GlobalACL**, następnie domyślnie odrzuca

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
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "beingId": "being-uuid",
    "permissionType": "Disk",
    "resourcePrefix": "disk:write",
    "result": "Allowed",
    "description": "Zezwolenie na zapis na dysku"
  }'
```

### Odwoływanie uprawnień

Przez stronę zarządzania uprawnieniami w Web UI.

### Przegląd uprawnień

```bash
curl http://localhost:8080/api/permissions/list?beingId=being-uuid
```

## Najlepsze praktyki

### 1. Zasada najmniejszych uprawnień

Nadawaj tylko minimalne wymagane uprawnienia:

```json
{
  "resourcePrefix": "disk:read",  // nie disk:*
  "result": "Allowed",
  "description": "Tylko odczyt dysku"
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
1. Pamięć podręczna częstotliwości? Brak buforowanej decyzji
2. IPermissionCallback? Zwraca AskUser (nie jawnie zezwolone)
3. IsCurator? Nie → Sprawdź GlobalACL
4. GlobalACL? Znaleziono regułę: file:... = Zezwolone
5. Wynik: Zezwól
```

### Scenariusz 2: AI chce wykonać kod

```
AI: "Chcę skompilować i uruchomić kod"
↓
Łańcuch uprawnień:
1. Pamięć podręczna częstotliwości? Brak buforowanej decyzji
2. IPermissionCallback? Zwraca AskUser
3. IsCurator? Tak → IPermissionAskHandler
4. Użytkownik zatwierdza
5. Wynik: Zezwól
```

### Scenariusz 3: Przekroczenie ograniczenia szybkości

```
AI: "Muszę wysłać 100 żądań HTTP"
↓
Łańcuch uprawnień:
1. Pamięć podręczna częstotliwości? Znaleziono w buforze HighDeny
2. Wynik: Odmów (brak dalszych sprawdzeń)
```

## Rozwiązywanie problemów

### Nieoczekiwane odmowy uprawnień

**Sprawdź**:
1. Status IsCurator użytkownika
2. Ustawienia pamięci podręcznej częstotliwości
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
