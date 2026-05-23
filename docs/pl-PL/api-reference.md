# Referencja API

> **Wersja: v0.2.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md) | [Polski](../pl-PL/api-reference.md)

## Punkty końcowe Web API

Bazowy URL: `http://localhost:8080`

### Uwierzytelnianie

Większość punktów końcowych wymaga uwierzytelnienia za pomocą ciasteczka sesji zarządzanego przez Web UI. Przed inicjalizacją systemu, wszystkie żądania z wyjątkiem strony pomocy zostaną przekierowane na stronę inicjalizacji.

---

## Pulpit nawigacyjny

### Pobranie statystyk pulpitu nawigacyjnego

**GET** `/api/dashboard/stats`

Zwraca dane przeglądu systemu (liczba istot, stan działania itp.).

### Pobranie wskaźników wydajności

**GET** `/api/dashboard/metrics`

Zwraca dane wskaźników wydajności w czasie rzeczywistym.

---

## System czatu

### Strona czatu

**GET** `/chat`

Zwraca stronę interfejsu czatu.

### Czat strumieniowy (SSE)

**GET** `/api/chat/stream`

Strumieniowy czat za pomocą zdarzeń wysyłanych przez serwer (SSE).

**Parametry zapytania**: `channelId` — identyfikator kanału czatu

**Odpowiedź**: Strumień zdarzeń wysyłanych przez serwer

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### Pobranie listy konwersacji

**GET** `/api/chat/conversations`

Zwraca listę wszystkich aktywnych konwersacji czatowych.

**Przykładowa odpowiedź**:
```json
{
  "conversations": [
    {
      "sessionId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
      "beingId": "being-uuid",
      "type": "single",
      "displayName": "Czat z asystentem",
      "lastMessage": "Treść ostatniej wiadomości",
      "lastTime": "2026-05-20T10:30:00Z"
    }
  ]
}
```

### Pobranie historii wiadomości

**GET** `/api/chat/messages`

Parametr zapytania: `channelId` — identyfikator kanału/konwersacji

Zwraca historię wiadomości określonej konwersacji.

### Pobranie historii czatu

**GET** `/api/chat/history`

Zwraca globalną historię czatu.

### Wysłanie wiadomości

**POST** `/api/chat/send`

**Treść żądania**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
  "content": "Treść wiadomości testowej"
}
```

**Odpowiedź**:
```json
{
  "success": true,
  "messageId": "50156b26-f3b9-4735-be3d-51e547bd3a4a"
}
```

### Zatrzymanie myślenia AI

**POST** `/api/chat/stop`

Zatrzymuje trwające generowanie odpowiedzi AI.

**Treść żądania**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d"
}
```

### Przesłanie pliku

**POST** `/api/chat/upload`

Przesłanie pliku do konwersacji czatowej (obsługuje multipart/form-data).

---

## Zarządzanie Istotami Krzemowymi

### Strona zarządzania istotami

**GET** `/beings`

Zwraca stronę interfejsu zarządzania istotami krzemowymi.

### Pobranie listy istot

**GET** `/api/beings` lub **GET** `/api/beings/list`

Zwraca listę wszystkich zarejestrowanych istot krzemowych.

**Przykładowa odpowiedź**:
```json
[
  {
    "id": "being-uuid",
    "name": "Assistant",
    "activity": "Idle",
    "isCustomCompiled": false,
    "customTypeName": ""
  }
]
```

**Wartości aktywności**: `Idle` | `SingleChat` | `GroupChat` | `Task` | `Timer` | `Broadcast` | `Project` | `MemoryCompression` | `Stopped`

### Pobranie szczegółów istoty

**GET** `/api/beings/detail`

Parametr zapytania: `beingId` — identyfikator istoty

Zwraca szczegółowe informacje o określonej istocie.

### Pobranie stanu aktywności istot

**GET** `/api/beings/activity`

Parametr zapytania: `id` — identyfikator istoty

Zwraca informacje o stanie aktywności określonej istoty.

**Przykładowa odpowiedź**:
```json
{
  "id": "being-uuid",
  "name": "Assistant",
  "activity": "Idle"
}
```

### Strona edytora pliku duszy

**GET** `/beings/soul`

Zwraca stronę interfejsu edytora pliku duszy.

### Zapisanie pliku duszy

**POST** `/api/beings/soul/save`

**Treść żądania**:
```json
{
  "beingId": "being-uuid",
  "soulContent": "# Personality\nYou are helpful..."
}
```

### Strona edytora konfiguracji AI

**GET** `/beings/ai-config`

Zwraca stronę interfejsu edytora konfiguracji AI.

### Zapisanie konfiguracji AI

**POST** `/api/beings/ai-config/save`

**Treść żądania**:
```json
{
  "beingId": "being-uuid",
  "aiClientType": "DashScope",
  "config": {
    "apiKey": "...",
    "region": "beijing",
    "model": "qwen3.6-plus"
  }
}
```

### Pobranie listy dostępnych modeli AI

**GET** `/api/beings/ai-config/models`

Parametry zapytania: `clientType`, `apiKey`, `region`

Zwraca listę dostępnych modeli dla określonego klienta AI.

---

## Przeglądanie historii czatu

### Strona historii czatu

**GET** `/chat-history`

Zwraca główną stronę historii czatu.

### Strona szczegółów historii czatu

**GET** `/chat-history-detail`

Zwraca stronę szczegółów historii określonej konwersacji.

### Strona szczegółów historii czatu grupowego

**GET** `/group-chat-history-detail`

Zwraca stronę szczegółów historii czatu grupowego.

### Strona szczegółów historii transmisji

**GET** `/broadcast-history-detail`

Zwraca stronę szczegółów historii kanału transmisji.

### Pobranie listy konwersacji historycznych

**GET** `/api/chat-history/conversations`

Zwraca listę wszystkich historycznych konwersacji.

### Pobranie wiadomości historycznych

**GET** `/api/chat-history/messages`

Parametr zapytania: `sessionId` — identyfikator konwersacji

Zwraca rekordy wiadomości określonej historycznej konwersacji.

---

## Zarządzanie czasomierzami

### Strona czasomierzy

**GET** `/timers`

Zwraca stronę interfejsu zarządzania czasomierzami.

### Pobranie listy czasomierzy

**GET** `/api/timers/list`

Zwraca listę wszystkich czasomierzy.

### Strona szczegółów cyklu czasomierza

**GET** `/timer-cycles/{timerId}`

Zwraca stronę szczegółów cyklu wykonania określonego czasomierza.

### Pobranie listy cykli czasomierza

**GET** `/api/timer-cycles/list`

Parametr zapytania: `timerId` — identyfikator czasomierza

Zwraca listę wszystkich cykli wykonania określonego czasomierza.

### Strona pojedynczego cyklu wykonania

**GET** `/timer-cycle/{cycleIndex}`

Zwraca stronę szczegółów pojedynczego wykonania.

### Pobranie wiadomości cyklu

**GET** `/api/timer-cycle/messages`

Parametr zapytania: `cycleIndex` — indeks cyklu

Zwraca wiadomości powiązane z określonym cyklem wykonania.

---

## Zarządzanie zadaniami

### Strona zadań

**GET** `/tasks`

Zwraca stronę interfejsu zarządzania zadaniami.

### Pobranie listy zadań

**GET** `/api/tasks/list`

Zwraca listę wszystkich zadań.

### Strona szczegółów cyklu zadania

**GET** `/task-cycles/{taskId}`

Zwraca stronę szczegółów cyklu wykonania określonego zadania.

### Pobranie listy cykli zadania

**GET** `/api/task-cycles/list`

Parametr zapytania: `taskId` — identyfikator zadania

Zwraca listę wszystkich cykli wykonania określonego zadania.

### Strona pojedynczego cyklu wykonania

**GET** `/task-cycle/{cycleIndex}`

Zwraca stronę szczegółów pojedynczego wykonania zadania.

### Pobranie wiadomości cyklu

**GET** `/api/task-cycle/messages`

Parametr zapytania: `cycleIndex` — indeks cyklu

Zwraca wiadomości powiązane z określonym cyklem wykonania zadania.

---

## System uprawnień

### Strona zarządzania uprawnieniami

**GET** `/permissions`

Zwraca stronę interfejsu zarządzania uprawnieniami.

### Pobranie listy reguł uprawnień

**GET** `/api/permissions/list`

Zwraca wszystkie aktualnie skonfigurowane reguły uprawnień.

**Przykładowa odpowiedź**:
```json
[
  {
    "permissionType": "Network",
    "resourcePrefix": "network:api.github.com",
    "result": "Allowed",
    "description": "GitHub API access"
  },
  {
    "permissionType": "Disk",
    "resourcePrefix": "file:C:\\Windows",
    "result": "Denied",
    "description": ""
  }
]
```

### Zapisanie reguły uprawnień

**POST** `/api/permissions/save`

**Treść żądania**:
```json
{
  "beingId": "being-uuid",
  "permissionType": "Disk",
  "resourcePrefix": "disk:write",
  "result": "Allowed",
  "description": "Zezwolenie na zapis na dysku"
}
```

### Strona żądania uprawnień

**GET** `/permission/request`

Wyświetla stronę żądania uprawnień, pozwalając użytkownikowi zatwierdzić lub odrzucić żądanie uprawnień Istoty Krzemowej.

**Parametry zapytania**:

| Parametr | Typ | Opis |
|----------|-----|------|
| `userId` | `Guid` | ID Istoty Krzemowej żądającej uprawnień |
| `type` | `string` | Typ uprawnień |
| `resource` | `string` | Ścieżka żądanego zasobu |
| `allowCode` | `string` | Identyfikator kodu operacji zezwolenia |
| `denyCode` | `string` | Identyfikator kodu operacji odmowy |

### Sprawdzenie oczekujących żądań uprawnień

**GET** `/permission/check`

Parametr zapytania: `userId` — ID Istoty Krzemowej

**Odpowiedź**:
```json
{
  "pending": true
}
```

### Odpowiedź na żądanie uprawnień

**GET** `/permission/respond`

**Parametry zapytania**:

| Parametr | Typ | Opis |
|----------|-----|------|
| `userId` | `Guid` | ID Istoty Krzemowej |
| `allowed` | `bool` | Czy zezwolić |
| `addToCache` | `bool` | Czy zapisać decyzję w pamięci podręcznej |
| `cacheDuration` | `double` | Czas trwania pamięci podręcznej (godziny) |

**Odpowiedź**:
```json
{
  "success": true
}
```

---

## System logów

### Strona logów

**GET** `/logs`

Zwraca stronę interfejsu przeglądania logów.

### Pobranie listy logów

**GET** `/api/logs/list`

Parametry zapytania obsługują filtrowanie według poziomu i zakresu czasu.

**Przykładowa odpowiedź**:
```json
{
  "logs": [
    {
      "timestamp": "2026-04-20T10:30:00Z",
      "level": "error",
      "message": "Failed to connect to AI service",
      "source": "OllamaClient"
    }
  ]
}
```

### Pobranie logów zgrupowanych według istot

**GET** `/api/logs/beings`

Statystyki logów zgrupowane według istot krzemowych.

### Pobranie dostępnych poziomów logów

**GET** `/api/logs/levels`

Zwraca listę dostępnych poziomów logów w systemie.

---

## Statystyki użycia

### Strona statystyk użycia

**GET** `/usage`

Zwraca stronę interfejsu statystyk użycia.

### Pobranie podsumowania użycia

**GET** `/api/usage/summary`

Zwraca podsumowanie użycia tokenów i kosztów.

### Pobranie danych trendu

**GET** `/api/usage/trend`

Parametry zapytania: `startDate`, `endDate`

Zwraca dane trendu użycia w określonym przedziale czasowym.

### Eksport danych użycia

**GET** `/api/usage/export`

Eksportuje dane użycia w formacie do pobrania.

---

## Śledzenie audytu

### Strona audytu

**GET** `/audit`

Zwraca stronę interfejsu śledzenia audytu.

### Pobranie listy audytów

**GET** `/api/audit/list`

Zwraca listę wpisów dziennika audytu.

### Pobranie podsumowania audytu

**GET** `/api/audit/summary`

Zwraca zagregowane statystyki danych audytu.

### Pobranie audytów zgrupowanych według istot

**GET** `/api/audit/beings`

Statystyki audytu zgrupowane według istot krzemowych.

---

## Zarządzanie konfiguracją

### Strona konfiguracji

**GET** `/config`

Zwraca stronę interfejsu konfiguracji systemu.

### Zapisanie konfiguracji

**POST** `/config/save`

**Treść żądania**:
```json
{
  "language": "ZhCN",
  "port": 8080,
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:7b"
    },
    "DashScope": {
      "apiKey": "...",
      "region": "beijing",
      "model": "qwen3.6-plus"
    }
  }
}
```

### Pobranie opcji konfiguracji AI

**GET** `/config/aioptions`

Zwraca dostępne typy klientów AI i ich dynamiczne opcje (dostępne modele, regiony itp.).

---

## System pamięci

### Strona pamięci

**GET** `/memory`

Zwraca stronę interfejsu zarządzania pamięcią.

### Pobranie listy pamięci

**GET** `/api/memory/list`

Zwraca listę wpisów pamięci istot krzemowych.

### Pobranie szczegółów pamięci

**GET** `/api/memory/detail/{id}`

Parametr ścieżki: `id` — identyfikator wpisu pamięci

Zwraca pełną treść określonego wpisu pamięci.

### Pobranie statystyk pamięci

**GET** `/api/memory/stats`

Zwraca informacje statystyczne systemu pamięci.

### Wyszukanie w pamięci

**GET** `/api/memory/search`

Parametr zapytania: `keyword` — słowo kluczowe wyszukiwania

Wyszukuje pasujące wpisy pamięci.

### Pobranie pamięci zgrupowanej według istot

**GET** `/api/memory/beings`

Statystyki pamięci zgrupowane według istot krzemowych.

### Pobranie śladu pamięci

**GET** `/api/memory/trace/{id}`

Parametr ścieżki: `id` — identyfikator wpisu pamięci

Zwraca łańcuch śledzenia źródła określonego wpisu pamięci.

### Pobranie osi czasu pamięci HTML

**GET** `/api/memory/timeline-html`

Zwraca widok HTML osi czasu pamięci.

---

## Notatki pracy

### Strona notatek pracy

**GET** `/work-notes`

Zwraca stronę interfejsu notatek pracy.

### Pobranie listy notatek pracy

**GET** `/api/work-notes/list`

Zwraca listę notatek pracy.

### Odczytanie notatki pracy

**GET** `/api/work-notes/read`

Parametr zapytania: `noteId` — identyfikator notatki

Zwraca treść określonej notatki.

### Pobranie katalogu notatek

**GET** `/api/work-notes/directory`

Zwraca strukturę katalogu notatek.

### Wyszukanie notatek pracy

**GET** `/api/work-notes/search`

Parametr zapytania: `keyword` — słowo kluczowe wyszukiwania

Wyszukuje pasujące notatki pracy.

### Utworzenie notatki pracy

**POST** `/api/work-notes/create`

**Treść żądania**:
```json
{
  "title": "Tytuł notatki",
  "content": "Treść notatki",
  "keywords": ["słowo kluczowe 1", "słowo kluczowe 2"]
}
```

### Aktualizacja notatki pracy

**POST** `/api/work-notes/update`

**Treść żądania**:
```json
{
  "noteId": "note-uuid",
  "title": "Zaktualizowany tytuł",
  "content": "Zaktualizowana treść"
}
```

### Usunięcie notatki pracy

**POST** `/api/work-notes/delete`

**Treść żądania**:
```json
{
  "noteId": "note-uuid"
}
```

---

## Sieć wiedzy

### Strona sieci wiedzy

**GET** `/knowledge`

Zwraca stronę interfejsu zarządzania siecią wiedzy.

### Pobranie grafu wiedzy

**GET** `/api/knowledge/graph`

Zwraca dane grafu trójek wiedzy (podmiot-relacja-orzeczenie).

---

## Zarządzanie projektami

### Strona projektów

**GET** `/project`

Zwraca stronę interfejsu zarządzania projektami.

### Strona notatek pracy projektu

**GET** `/project/{id}/work-notes`

Parametr ścieżki: `id` — identyfikator projektu

Zwraca stronę notatek pracy określonego projektu.

### Strona zadań projektu

**GET** `/project/{id}/tasks`

Parametr ścieżki: `id` — identyfikator projektu

Zwraca stronę zarządzania zadaniami określonego projektu.

### Pobranie listy projektów

**GET** `/api/projects/list`

Zwraca listę wszystkich projektów.

### Pobranie listy szablonów przepływu pracy

**GET** `/api/projects/list-workflow-templates`

Zwraca listę dostępnych szablonów przepływu pracy.

### Utworzenie projektu

**POST** `/api/projects/create`

**Treść żądania**:
```json
{
  "name": "Mój projekt",
  "description": "Opis projektu"
}
```

### Zarchiwizowanie projektu

**POST** `/api/projects/{id}/archive`

Parametr ścieżki: `id` — identyfikator projektu

Archiwizuje określony projekt.

### Przywrócenie projektu

**POST** `/api/projects/{id}/restore`

Parametr ścieżki: `id` — identyfikator projektu

Przywraca zarchiwizowany projekt.

### Zniszczenie projektu

**POST** `/api/projects/{id}/destroy`

Parametr ścieżki: `id` — identyfikator projektu

Trwale usuwa określony projekt (nieodwracalne).

### Pobranie szczegółów projektu

**GET** `/api/projects/detail`

Parametr zapytania: `projectId` — identyfikator projektu

Zwraca szczegółowe informacje o projekcie.

### Aktualizacja projektu

**POST** `/api/projects/update`

**Treść żądania**:
```json
{
  "projectId": "project-uuid",
  "name": "Zaktualizowana nazwa",
  "description": "Zaktualizowany opis"
}
```

### Przypisanie członka do projektu

**POST** `/api/projects/assign`

**Treść żądania**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Usunięcie członka z projektu

**POST** `/api/projects/remove`

**Treść żądania**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Pobranie listy notatek pracy projektu

**GET** `/api/projects/{id}/work-notes/list`

Parametr ścieżki: `id` — identyfikator projektu

Zwraca listę notatek pracy określonego projektu.

### Odczytanie notatki pracy projektu

**GET** `/api/projects/{id}/work-notes/read`

Parametr ścieżki: `id` — identyfikator projektu

Zwraca treść notatek pracy określonego projektu.

### Utworzenie notatki pracy projektu

**POST** `/api/projects/{id}/work-notes/create`

Parametr ścieżki: `id` — identyfikator projektu

Tworzy nową notatkę pracy w określonym projekcie.

### Aktualizacja notatki pracy projektu

**POST** `/api/projects/{id}/work-notes/update`

Parametr ścieżki: `id` — identyfikator projektu

Aktualizuje notatkę pracy w określonym projekcie.

### Usunięcie notatki pracy projektu

**POST** `/api/projects/{id}/work-notes/delete`

Parametr ścieżki: `id` — identyfikator projektu

Usuwa notatkę pracy z określonego projektu.

### Pobranie listy zadań projektu

**GET** `/api/projects/{id}/tasks/list`

Parametr ścieżki: `id` — identyfikator projektu

Zwraca listę zadań określonego projektu.

### Utworzenie zadania projektu

**POST** `/api/projects/{id}/tasks/create`

Parametr ścieżki: `id` — identyfikator projektu

Tworzy nowe zadanie w określonym projekcie.

### Aktualizacja zadania projektu

**POST** `/api/projects/{id}/tasks/update`

Parametr ścieżki: `id` — identyfikator projektu

Aktualizuje zadanie w określonym projekcie.

### Usunięcie zadania projektu

**POST** `/api/projects/{id}/tasks/delete`

Parametr ścieżki: `id` — identyfikator projektu

Usuwa zadanie z określonego projektu.

### Przypisanie odpowiedzialnego za zadanie

**POST** `/api/projects/{id}/tasks/assign`

Parametr ścieżki: `id` — identyfikator projektu

Przypisuje odpowiedzialnego do zadania projektu.

### Usunięcie odpowiedzialnego za zadanie

**POST** `/api/projects/{id}/tasks/remove-assignee`

Parametr ścieżki: `id` — identyfikator projektu

Usuwa odpowiedzialnego z zadania projektu.

### Oznaczenie zadania jako ukończone

**POST** `/api/projects/{id}/tasks/complete`

Parametr ścieżki: `id` — identyfikator projektu

Oznacza zadanie projektu jako ukończone.

### Oznaczenie zadania jako nieudane

**POST** `/api/projects/{id}/tasks/fail`

Parametr ścieżki: `id` — identyfikator projektu

Oznacza zadanie projektu jako nieudane.

### Anulowanie zadania

**POST** `/api/projects/{id}/tasks/cancel`

Parametr ścieżki: `id` — identyfikator projektu

Anuluje zadanie projektu.

---

## Zarządzanie wykonawcami

### Strona wykonawców

**GET** `/executors`

Zwraca stronę interfejsu zarządzania wykonawcami.

### Pobranie stanu wykonawców

**GET** `/api/executors/status`

Zwraca stan działania poszczególnych wykonawców (dysk, sieć, wiersz poleceń).

---

## Przeglądarka kodu

### Strona przeglądarki kodu

**GET** `/code`

Zwraca stronę interfejsu przeglądarki kodu.

### Pobranie listy typów kodu

**GET** `/api/code/types`

Zwraca listę obsługiwanych typów/języków kodu.

### Pobranie szczegółów kodu

**GET** `/api/code/detail`

Parametry zapytania: `filePath`, `lineNumber`

Zwraca szczegóły kodu określonego pliku.

---

## Podpowiedzi kodu

### Pobranie podpowiedzi

**GET** `/api/code/hover`
**POST** `/api/code/hover`

Pobiera informacje podpowiedzi dla lokalizacji kodu (podobnie jak inteligentne podpowiedzi IDE).

### Rejestracja lokalizacji kodu

**POST** `/api/code/register`

Rejestruje lokalizacje kodu do monitorowania.

### Aktualizacja lokalizacji kodu

**POST** `/api/code/update`

Aktualizuje informacje o zarejestrowanej lokalizacji kodu.

### Wyrejestrowanie lokalizacji kodu

**POST** `/api/code/unregister`

Wyrejestrowuje niepotrzebne już monitorowanie lokalizacji kodu.

---

## System dokumentacji pomocy

### Strona pomocy

**GET** `/help` lub **GET** `/help/index`

Zwraca główną stronę dokumentacji pomocy.

### Strona tematu pomocy

**GET** `/help/{topic}`

Parametr ścieżki: `topic` — identyfikator tematu

Zwraca stronę dokumentacji pomocy dla określonego tematu.

### Wyszukanie dokumentacji pomocy

**GET** `/api/help/search`

Parametr zapytania: `keyword` — słowo kluczowe wyszukiwania

Wyszukuje pasujące tematy dokumentacji pomocy.

---

## Inicjalizacja

### Strona kreatora inicjalizacji

**GET** `/init`

Zwraca stronę kreatora inicjalizacji przy pierwszym uruchomieniu.

### Przesłanie inicjalizacji

**POST** `/init`

Przesyła konfigurację inicjalizacji pierwszego uruchomienia.

### Przeglądanie wyboru katalogu danych

**GET** `/init/browse`

Otwiera przeglądarkę katalogów w celu wyboru lokalizacji przechowywania danych.

### Pobranie metadanych konfiguracji AI

**GET** `/init/ai-config-metadata`

Zwraca dostępne typy klientów AI i metadane ich pól konfiguracyjnych.

---

## Sterowanie systemem

### Łagodne wyłączenie

**POST** `/api/system/shutdown`

> **Uwaga**: Dozwolone tylko żądania z localhost

Wyzwala proces łagodnego wyłączenia aplikacji:

1. Zatrzymanie pętli głównej (MainLoop)
2. Zapisanie bieżącej konfiguracji
3. Zamknięcie nasłuchiwacza HTTP

**Odpowiedź**:
```json
{
  "status": "shutting_down",
  "message": "Application is shutting down gracefully"
}
```

---

## O systemie

### Strona o systemie

**GET** `/about`

Zwraca stronę o systemie, zawierającą informacje o systemie i listę załadowanych wtyczek.

**Dane listy wtyczek**:
```json
{
  "plugins": {
    "plugin-id": {
      "name": "My Plugin",
      "version": "1.0.0",
      "description": "Plugin description",
      "author": "Author Name"
    }
  }
}
```

---

## Odpowiedzi błędów

Wszystkie punkty końcowe zwracają ustandaryzowane odpowiedzi błędów:

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: disk:write, Current: disk:read"
  }
}
```

### Typowe kody błędów

| Kod | Status HTTP | Opis |
|-----|-------------|------|
| `PERMISSION_DENIED` | 403 | Niewystarczające uprawnienia |
| `NOT_FOUND` | 404 | Zasób nie znaleziony |
| `VALIDATION_ERROR` | 400 | Nieprawidłowe parametry żądania |
| `INTERNAL_ERROR` | 500 | Wewnętrzny błąd serwera |
| `SERVICE_UNAVAILABLE` | 503 | Usługa AI niedostępna |

---

## Zdarzenia SSE

Zdarzenia wysyłane przez serwer są używane do aktualizacji w czasie rzeczywistym:

### Zdarzenia czatu

```javascript
const eventSource = new EventSource('/api/chat/stream?channelId=channel-uuid');

eventSource.onmessage = (event) => {
  const data = JSON.parse(event.data);
  
  switch(data.type) {
    case 'chunk':
      console.log('Streaming:', data.content);
      break;
    case 'tool_call':
      console.log('Tool executing:', data.tool);
      break;
    case 'complete':
      console.log('Chat complete, session:', data.sessionId);
      break;
    case 'error':
      console.error('Error:', data.message);
      break;
  }
};
```

---

## Interfejs klienta AI

### Interfejs IAIClient

```csharp
public interface IAIClient
{
    string Name { get; }
    
    Task<AIResponse> ChatAsync(AIRequest request);
    
    IAsyncEnumerable<string> StreamChatAsync(AIRequest request);
}
```

### Struktura AIRequest

```csharp
public class AIRequest
{
    public List<Message> Messages { get; set; }
    public List<ToolDefinition> Tools { get; set; }
    public double Temperature { get; set; } = 0.7;
    public int MaxTokens { get; set; } = 2000;
    public string Model { get; set; }
}
```

### Struktura AIResponse

```csharp
public class AIResponse
{
    public string Content { get; set; }
    public List<ToolCall> ToolCalls { get; set; }
    public TokenUsage Usage { get; set; }
    public string Model { get; set; }
}
```

---

## Interfejs systemu narzędzi

### Interfejs ITool

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    ToolDefinition Definition { get; }
    
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### Struktura ToolCall

```csharp
public class ToolCall
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}
```

### Struktura ToolResult

```csharp
public class ToolResult
{
    public bool Success { get; set; }
    public string Output { get; set; }
    public string Error { get; set; }
}
```

---

## Następne kroki

- 🚀 Zobacz [przewodnik szybkiego startu](getting-started.md)
- 🛠️ Przeczytaj [przewodnik programistyczny](development-guide.md)
- 📚 Zobacz [dokumentację architektury](architecture.md)
- 🔒 Poznaj [model bezpieczeństwa](security.md)
