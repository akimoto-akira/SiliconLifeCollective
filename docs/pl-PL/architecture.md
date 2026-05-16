# Architektura

> **Wersja: v0.2.0-alpha**

[English](../en/architecture.md) | [Deutsch](../de-DE/architecture.md) | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Čeština](../cs-CZ/architecture.md) | **Polski**

## Architektura dwóch wersji

Ten projekt oferuje dwie wersje implementacji, współdzielące tę samą architekturę, ale różniące się w przechowywaniu i optymalizacji wydajności:

### SiliconLife.Default (wersja domyślna)
- **Przeznaczenie**: domyślna implementacja, głównie do weryfikacji wykonalności architektury
- **Tryb działania**: aplikacja konsolowa
- **Sposób przechowywania**: czyste przechowywanie JSON w systemie plików
- **Scenariusze zastosowania**: wysokie wymagania dotyczące bezpieczeństwa danych, ograniczone zasoby pamięci, mała ilość danych
- **Opis roli**: jako referencyjna implementacja weryfikacji architektury, oferuje prosty i niezawodny sposób działania, odpowiedni do pierwszego kontaktu z projektem, debugowania rozwoju lub scenariuszy z priorytetem bezpieczeństwa danych

### SiliconLife.Fast (wersja wysokowydajna)
- **Przeznaczenie**: główna wersja produkcyjna
- **Tryb działania**: aplikacja desktopowa (Windows zasobnik systemowy / Linux okno stanu)
- **Sposób przechowywania**: pamięć SpeedyPack + asynchroniczna trwałość wsadowa (format pliku .spk)
- **Scenariusze zastosowania**: wysoka współbieżność, niskie opóźnienie, duże ilości danych
- **Obsługa platform**: Windows/macOS (pełna funkcjonalność, z zasobnikiem systemowym), Linux (okno stanu, brak ikony w zasobniku)
- **Cechy**:
  - Windows/macOS działanie w tle w zasobniku systemowym, okno stanu zasobnika z monitorowaniem w czasie rzeczywistym; Linux okno stanu wyświetlane bezpośrednio
  - Silnik SpeedyPack + automatyczna kompresja gwarantująca bezpieczeństwo danych
  - Architektura Component UI, 30+ deklaratywnych komponentów
  - 7 motywów skórek, obsługa automatycznego wykrywania i przełączania
  - Narzędzie gorącego przeładowania obsługujące aktualizacje online i restart
  - Linux automatycznie otwiera przeglądarkę dla dostępu do Web UI, obsługuje parametr `--no-tray`
- **Poprawa wydajności**: opóźnienie odczytu przechowywania zmniejszone 1000-krotnie, opóźnienie zapisu zmniejszone 15000-krotnie
- **Opis roli**: głęboko zoptymalizowana implementacja produkcyjna, z funkcjami takimi jak działanie w tle w zasobniku systemowym, silnik SpeedyPack + automatyczna kompresja, preferowana do długotrwałego działania i rzeczywistych środowisk produkcyjnych

> **Uwaga**: Architektura opisana w tym dokumencie dotyczy obu wersji, z wyjątkiem części dotyczącej implementacji przechowywania. SiliconLife.Default służy jako referencyjna implementacja weryfikacji architektury, a SiliconLife.Fast jako główna wersja do środowisk produkcyjnych.

---

## Kluczowe koncepcje

### Istota Krzemowa

Każdy agent AI w systemie jest **Istotą Krzemową** — autonomiczną jednostką z własną tożsamością, osobowością i zdolnościami. Każda Istota Krzemowa jest napędzana przez **plik duszy** (podpowiedź Markdown), definiujący jej wzorce zachowań.

### Kurator Krzemowy

**Kurator Krzemowy** to specjalna Istota Krzemowa z najwyższymi uprawnieniami systemowymi. Pełni rolę administratora systemu:

- Tworzenie i zarządzanie innymi Istotami Krzemowymi
- Analizowanie żądań użytkowników i rozkładanie ich na zadania
- Przekazywanie zadań odpowiednim Istotom Krzemowym
- Monitorowanie jakości wykonania i obsługa niepowodzeń
- Używanie **harmonogramowania priorytetowego** do odpowiadania na wiadomości użytkowników (patrz poniżej)

### Plik duszy

Plik Markdown przechowywany w katalogu danych każdej Istoty Krzemowej (`soul.md`). Jest wstrzykiwany jako podpowiedź systemowa do każdego żądania AI, definiując osobowość, wzorce decyzyjne i ograniczenia zachowania istoty.

---

## Harmonogramowanie: sprawiedliwe harmonogramowanie oparte na szczelinach czasowych

### Pętla główna + obiekty zegara

System uruchamia **pętlę główną napędzaną zegarem** na dedykowanym wątku w tle:

```
Pętla główna (dedykowany wątek, watchdog + bezpiecznik)
  └── Obiekt zegara A (priorytet=0, interwał=100ms)
  └── Obiekt zegara B (priorytet=1, interwał=500ms)
  └── Menedżer Istot Krzemowych (bezpośrednio wyzwalany zegarem przez pętlę główną)
        └── Runner Istoty Krzemowej → Istota 1 → Zegar wyzwala → Wykonanie jednej rundy
        └── Runner Istoty Krzemowej → Istota 2 → Zegar wyzwala → Wykonanie jednej rundy
        └── Runner Istoty Krzemowej → Istota 3 → Zegar wyzwala → Wykonanie jednej rundy
        └── ...
```

Kluczowe decyzje projektowe:

- **Istoty Krzemowe nie dziedziczą obiektu zegara.** Mają własną metodę `Tick()`, wywoływaną przez `SiliconBeingManager` za pośrednictwem `SiliconBeingRunner`, a nie bezpośrednio rejestrowaną w pętli głównej.
- **Menedżer Istot Krzemowych** jest bezpośrednio wyzwalany zegarem przez pętlę główną i działa jako pojedynczy proxy dla wszystkich istot.
- **Runner Istoty Krzemowej** opakowuje `Tick()` każdej istoty na tymczasowym wątku, z limitem czasu i bezpiecznikiem dla każdej istoty (3 kolejne przekroczenia czasu → 1 minuta ostygnięcia).
- Wykonanie każdej istoty jest ograniczone do **jednej rundy** żądania AI + wywołania narzędzi na każde wyzwolenie zegara, zapewniając, że żadna istota nie może zmonopolizować pętli głównej.
- **Monitor wydajności** śledzi czasy wykonania zegara w celu zapewnienia obserwowalności.

### Priorytetowa odpowiedź Kuratora

Gdy użytkownik wyśle wiadomość do Kuratora Krzemowego:

1. Bieżąca istota (np. Istota A) kończy swoją bieżącą rundę — **bez przerywania**.
2. Menedżer **pomija pozostałą kolejkę**.
3. Pętla **rozpoczyna się ponownie od Kuratora**, umożliwiając mu natychmiastowe wykonanie.

Zapewnia to odpowiedź na interakcje użytkownika bez zakłócania trwających zadań.

---

## Architektura komponentów

```
┌─────────────────────────────────────────────────────────┐
│                        Host rdzenny                     │
│  (ujednolicony host — montowanie i zarządzanie wszystkimi komponentami) │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ Pętla    │  │ Lokalizator  │  │   Konfiguracja    │  │
│  │ główna   │  │ usług       │  │                   │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │        Menedżer Istot Krzemowych (obiekt zegara)  │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Kurator  │ │Istota A │ │Istota B │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              Usługi współdzielone                │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │System    │  │Przechowy-│  │  Menedżer        │  │   │
│  │  │czatu     │  │wanie    │  │  uprawnień       │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │Klient AI │  │Wykonawca │  │  Menedżer        │  │   │
│  │  │          │  │          │  │  narzędzi        │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  │  ┌──────────┐ ┌──────────┐                        │   │
│  │  │Ładowarka │  │Sieć      │                        │   │
│  │  │wtyczek   │  │wiedzy   │                        │   │
│  │  └──────────┘ └──────────┘                        │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  Wykonawcy                        │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Dyskowy  │  │ Sieciowy │  │  Wiersza         │  │   │
│  │  │wykonawca │  │wykonawca │  │  poleceń wykonawca│  │   │
│  │  └──────────┘ └──────────┘ └──────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              Dostawcy komunikacji natychmiastowej │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Konsola  │  │  Web     │  │  Feishu / ...    │  │   │
│  │  │dostawca  │  │dostawca  │  │  dostawca        │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## Lokalizator usług

`ServiceLocator` to bezpieczny dla wątków rejestr singletonowy, zapewniający dostęp do wszystkich rdzennych usług:

| Właściwość | Typ | Opis |
|------------|------|-------------|
| `ChatSystem` | `ChatSystem` | Centralny menedżer sesji czatu |
| `IMManager` | `IMManager` | Router dostawców komunikacji natychmiastowej |
| `AuditLogger` | `AuditLogger` | Ślad audytu uprawnień |
| `GlobalAcl` | `GlobalACL` | Globalna lista kontroli dostępu |
| `BeingFactory` | `ISiliconBeingFactory` | Fabryka tworząca istoty |
| `BeingManager` | `SiliconBeingManager` | Menedżer cyklu życia aktywnych istot |
| `DynamicBeingLoader` | `DynamicBeingLoader` | Moduł ładujący kompilacji dynamicznej |
| `TokenUsageAudit` | `ITokenUsageAudit` | Śledzenie użycia Tokenów |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Raportowanie użycia Tokenów |

Utrzymuje również rejestr `PermissionManager` dla każdej istoty, z kluczem GUID istoty.

---

## System czatu

### Typy sesji

System czatu obsługuje trzy typy sesji poprzez `SessionBase`:

| Typ | Klasa | Opis |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | Rozmowa jeden na jeden między dwoma uczestnikami |
| `GroupChat` | `GroupChatSession` | Czat grupowy z wieloma uczestnikami |
| `Broadcast` | `BroadcastChannel` | Otwarty kanał ze stałym identyfikatorem; istoty dynamicznie subskrybują, otrzymując wiadomości tylko po subskrypcji |

### Kanały nadawcze

`BroadcastChannel` to specjalny typ sesji używany do ogłoszeń ogólnosystemowych:

- **Stały identyfikator kanału** — w przeciwieństwie do `SingleChatSession` i `GroupChatSession`, identyfikator kanału jest dobrze znaną stałą, a nie wyprowadzaną z identyfikatorów GUID członków.
- **Dynamiczna subskrypcja** — istoty subskrybują/odsubskrybowują w czasie działania; otrzymują wiadomości opublikowane dopiero po subskrypcji.
- **Filtrowanie oczekujących wiadomości** — `GetPendingMessages()` zwraca tylko wiadomości opublikowane po czasie subskrypcji istoty i jeszcze nieodczytane.
- **Zarządzany przez system czatu** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### Wiadomości czatu

Model `ChatMessage` zawiera pola dla kontekstu rozmowy AI i śledzenia tokenów:

| Pole | Typ | Opis |
|-------|------|-------------|
| `Id` | `Guid` | Unikalny identyfikator wiadomości |
| `SenderId` | `Guid` | Unikalny identyfikator nadawcy |
| `ChannelId` | `Guid` | Identyfikator kanału/rozmowy |
| `Content` | `string` | Treść wiadomości |
| `Timestamp` | `DateTime` | Czas wysłania wiadomości |
| `Type` | `MessageType` | Tekst, obraz, plik lub powiadomienie systemowe |
| `ReadBy` | `List<Guid>` | Identyfikatory uczestników, którzy przeczytali tę wiadomość |
| `Role` | `MessageRole` | Rola w rozmowie AI (użytkownik, asystent, narzędzie) |
| `ToolCallId` | `string?` | Identyfikator wywołania narzędzia dla wiadomości z wynikiem narzędzia |
| `ToolCallsJson` | `string?` | Zserializowany JSON wywołań narzędzi dla wiadomości asystenta |
| `Thinking` | `string?` | Łańcuch rozumowania myślowego AI |
| `PromptTokens` | `int?` | Liczba tokenów w podpowiedzi (wejście) |
| `CompletionTokens` | `int?` | Liczba tokenów w uzupełnieniu (wyjście) |
| `TotalTokens` | `int?` | Całkowita liczba użytych tokenów (wejście + wyjście) |
| `FileMetadata` | `FileMetadata?` | Metadane załączonego pliku (jeśli wiadomość zawiera plik) |

### Kolejka wiadomości czatu

`ChatMessageQueue` to bezpieczny dla wątków system kolejki wiadomości, zarządzający asynchronicznym przetwarzaniem wiadomości czatu:

- **Bezpieczeństwo wątkowe** - używa mechanizmów blokad zapewniających bezpieczny dostęp współbieżny
- **Przetwarzanie asynchroniczne** - obsługa asynchronicznego umieszczania wiadomości w kolejce i pobierania z niej
- **Porządkowanie wiadomości** - zachowanie kolejności czasowej wiadomości
- **Operacje wsadowe** - obsługa pobierania wiadomości w partiach

### Metadane pliku

`FileMetadata` służy do zarządzania informacjami o plikach załączonych do wiadomości czatu:

- **Informacje o pliku** - nazwa pliku, rozmiar, typ, ścieżka
- **Czas przesłania** - znacznik czasu przesłania pliku
- **Przesyłający** - identyfikator użytkownika lub Istoty Krzemowej, która przesłała plik

### Menedżer anulowania strumienia

`StreamCancellationManager` zapewnia mechanizm anulowania dla strumieniowych odpowiedzi AI:

- **Kontrola strumienia** - obsługa anulowania trwającej strumieniowej odpowiedzi AI
- **Czyszczenie zasobów** - prawidłowe czyszczenie powiązanych zasobów po anulowaniu
- **Bezpieczeństwo współbieżne** - obsługa jednoczesnego zarządzania wieloma strumieniami

### Przeglądanie historii czatu

Nowa funkcja przeglądania historii czatu pozwala użytkownikom przeglądać historyczne rozmowy Istot Krzemowych:

- **Lista sesji** - wyświetlanie wszystkich historycznych sesji
- **Szczegóły wiadomości** - przeglądanie pełnej historii wiadomości
- **Widok osi czasu** - wyświetlanie wiadomości w kolejności chronologicznej
- **Obsługa API** - dostarczanie RESTful API do pobierania danych sesji i wiadomości

---

## System klienta AI

System obsługuje wiele zapleczy AI poprzez interfejs `IAIClient`:

### OllamaClient

- **Typ**: lokalna usługa AI
- **Protokół**: natywne API HTTP Ollama (`/api/chat`, `/api/generate`)
- **Funkcje**: przesyłanie strumieniowe, wywołania narzędzi, lokalne hostowanie modeli
- **Konfiguracja**: `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (Alibaba Cloud Bailian)

- **Typ**: chmurowa usługa AI
- **Protokół**: API kompatybilne z OpenAI (`/compatible-mode/v1/chat/completions`)
- **Uwierzytelnianie**: Bearer token (klucz API)
- **Funkcje**: przesyłanie strumieniowe, wywołania narzędzi, treść rozumowania (łańcuch myślowy), wdrożenie wieloregionowe
- **Obsługiwane regiony**:
  - `beijing` — Chiny Północne 2 (Pekin)
  - `virginia` — USA (Wirginia)
  - `singapore` — Singapur
  - `hongkong` — Hongkong
  - `frankfurt` — Niemcy (Frankfurt)
- **Obsługiwane modele** (dynamicznie odkrywane przez API, z listą rezerwową):
  - **Seria Qianwen**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **Rozumowanie**: qwq-plus
  - **Strony trzecie**: deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **Konfiguracja**: `apiKey`, `region`, `model`
- **Odkrywanie modeli**: w czasie działania pobiera dostępne modele z API Bailian; w przypadku awarii sieci powraca do wyselekcjonowanej listy

### VolcengineArkClient (Volcengine Ark)

- **Typ**: chmurowa usługa AI
- **Protokół**: API kompatybilne z OpenAI
- **Uwierzytelnianie**: Bearer token (klucz API)
- **Funkcje**: obsługa trybu strumieniowego i niestrumieniowego, wbudowana dwuwarstwowa kontrola szybkości
  - Samokontrola szybkości: wymuszanie minimalnego odstępu między żądaniami
  - Limit szybkości serwera: obsługa błędów 429, ponawianie z wykładniczym wycofywaniem
- **Konfiguracja**: `apiKey`, `endpoint`, `model`
- **Cechy**: usługa AI firmy ByteDance, obsługa wielu modeli Doubao

### Wzorzec fabryki klientów

Każdy typ klienta AI ma odpowiednią implementację fabryki `IAIClientFactory`:

- `OllamaClientFactory` — tworzy instancje OllamaClient
- `DashScopeClientFactory` — tworzy instancje DashScopeClient
- `VolcengineArkClientFactory` — tworzy instancje VolcengineArkClient

Fabryki zapewniają:
- `CreateClient(Dictionary<string, object> config)` — tworzenie instancji klienta z konfiguracji
- `GetConfigKeyOptions(string key, ...)` — zwraca dynamiczne opcje dla klucza konfiguracji (np. dostępne modele, regiony)
- `GetDisplayName()` — zlokalizowana nazwa wyświetlana typu klienta

### Lista obsługiwanych platform AI

#### Legenda statusów
- ✅ Zaimplementowane
- 🚧 W rozwoju
- 📋 Planowane
- 💡 Rozważane

*Uwaga: Ze względu na środowisko sieciowe dewelopera, łączenie z rozważanymi zagranicznymi chmurowymi usługami AI może wymagać użycia narzędzi proxy sieciowego, a proces debugowania może być niestabilny.*

#### Lista platform

| Platforma | Status | Typ | Opis |
|-----------|--------|-----|------|
| Ollama | ✅ | Lokalna | Lokalna usługa AI, obsługa lokalnego wdrażania modeli |
| DashScope (Alibaba Cloud Bailian) | ✅ | Chmurowa | Usługa AI Alibaba Cloud Bailian, obsługa wdrożenia wieloregionowego |
| Baidu Qianfan (Wenxin Yiyan) | 📋 | Chmurowa | Usługa AI Baidu Wenxin Yiyan |
| Zhipu AI (GLM) | 📋 | Chmurowa | Usługa AI Zhipu Qingyan |
| Moonshot (Kimi) | 📋 | Chmurowa | Usługa AI Moonshot Kimi |
| Volcengine Ark · Doubao | ✅ | Chmurowa | Usługa AI Doubao firmy ByteDance |
| DeepSeek (bezpośrednie połączenie) | 📋 | Chmurowa | Usługa AI DeepSeek |
| 01.AI (Yi) | 📋 | Chmurowa | Usługa AI 01.AI |
| Tencent Hunyuan | 📋 | Chmurowa | Usługa AI Tencent Hunyuan |
| SiliconFlow | 📋 | Chmurowa | Usługa AI SiliconFlow |
| MiniMax | 📋 | Chmurowa | Usługa AI MiniMax |
| OpenAI | 💡 | Chmurowa | Usługa OpenAI API (seria GPT) |
| Anthropic | 💡 | Chmurowa | Usługa AI Anthropic Claude |
| Google DeepMind | 💡 | Chmurowa | Usługa Google Gemini |
| Mistral AI | 💡 | Chmurowa | Usługa AI Mistral |
| Groq | 💡 | Chmurowa | Szybka usługa wnioskowania AI Groq |
| Together AI | 💡 | Chmurowa | Usługa modeli open source Together AI |
| xAI | 💡 | Chmurowa | Usługa xAI Grok |
| Cohere | 💡 | Chmurowa | Korporacyjna usługa NLP Cohere |
| Replicate | 💡 | Chmurowa | Platforma hostingu modeli open source Replicate |
| Hugging Face | 💡 | Chmurowa | Społeczność open source AI i platforma modeli Hugging Face |
| Cerebras | 💡 | Chmurowa | Usługa optymalizacji wnioskowania AI Cerebras |
| Databricks | 💡 | Chmurowa | Korporacyjna platforma AI Databricks (MosaicML) |
| Perplexity AI | 💡 | Chmurowa | Usługa wyszukiwania i odpowiedzi AI Perplexity |
| NVIDIA NIM | 💡 | Chmurowa | Mikrousługi wnioskowania AI NVIDIA |

---

## Kluczowe decyzje projektowe

### Przechowywanie jako klasa instancji (nie statyczna)

`IStorage` jest zaprojektowany jako wstrzykiwalna instancja, a nie statyczne narzędzie. Zapewnia to:

- Bezpośredni dostęp do systemu plików — IStorage jest wewnętrznym kanałem trwałości systemu, **nie** przekierowywanym przez wykonawców.
- **AI nie ma kontroli nad IStorage** — wykonawcy zarządzają IO inicjowanym przez narzędzia AI; IStorage zarządza wewnętrznym odczytem i zapisem danych samego frameworka. Są to fundamentalnie różne kwestie.
- Możliwość testowania z użyciem implementacji mock.
- Przyszłe wsparcie dla różnych zapleczy przechowywania bez modyfikacji konsumentów.

### Wykonawcy jako granica bezpieczeństwa

Wykonawcy są **jedyną** ścieżką dla operacji I/O. Narzędzia wymagające dostępu do dysku, sieci lub wiersza poleceń **muszą** przechodzić przez wykonawców. Ten projekt wymusza:

- Każdy wykonawca posiada **niezależny wątek harmonogramowania** z blokadą wątku do weryfikacji uprawnień.
- Scentralizowane sprawdzanie uprawnień — wykonawcy odpytują **prywatny menedżer uprawnień** istoty.
- Kolejka żądań z obsługą priorytetów i limitów czasu.
- Dziennik audytu wszystkich zewnętrznych operacji.
- Izolacja wyjątków — awaria jednego wykonawcy nie wpływa na inne.
- Bezpiecznik — kolejne niepowodzenia tymczasowo zatrzymują wykonawcę, aby zapobiec kaskadowym awariom.

### ContextManager jako lekki obiekt

Każde wywołanie `ExecuteOneRound()` tworzy nową instancję `ContextManager`:

1. Ładuje plik duszy + ostatnią historię czatu.
2. Wysyła żądanie do klienta AI.
3. W pętli przetwarza wywołania narzędzi, aż AI zwróci czysty tekst.
4. Utrwala odpowiedź w systemie czatu.
5. Zwalnia zasoby.

Dzięki temu każda runda pozostaje izolowana i bezstanowa.

### Samewolucja poprzez nadpisywanie klas

Istoty Krzemowe mogą w czasie działania nadpisywać własne klasy C#:

1. AI generuje nowy kod klasy (musi dziedziczyć po `SiliconBeingBase`).
2. **Kontrola referencji podczas kompilacji** (główna obrona): kompilator otrzymuje tylko listę dozwolonych zestawów — `System.IO`, `System.Reflection` itp. są wykluczone, więc niebezpieczny kod jest niemożliwy na poziomie typu.
3. **Statyczna analiza w czasie działania** (obrona dodatkowa): `SecurityScanner` skanuje kod pod kątem niebezpiecznych wzorców po udanej kompilacji.
4. Roslyn kompiluje kod w pamięci.
5. W przypadku sukcesu: `SiliconBeingManager.ReplaceBeing()` zamienia bieżącą instancję, migruje stan i utrwala zaszyfrowany kod na dysku.
6. W przypadku niepowodzenia: nowy kod jest odrzucany, zachowana zostaje istniejąca implementacja.

Niestandardowa implementacja `IPermissionCallback` może być również skompilowana i wstrzyknięta przez `ReplacePermissionCallback()`, pozwalając istocie na dostosowanie własnej logiki uprawnień.

Kod jest przechowywany na dysku w formie zaszyfrowanej AES-256. Klucz szyfrowania jest wyprowadzany z GUID istoty (wielkimi literami) za pomocą PBKDF2.

---

## Audyt użycia Tokenów

`TokenUsageAuditManager` śledzi zużycie tokenów AI wszystkich istot:

- `TokenUsageRecord` — rekord dla każdego żądania (ID istoty, model, tokeny podpowiedzi, tokeny uzupełnienia, znacznik czasu)
- `TokenUsageSummary` — zagregowane statystyki
- `TokenUsageQuery` — parametry zapytania do filtrowania rekordów
- Utrwalane przez `ITimeStorage` dla zapytań szeregów czasowych
- Dostępne przez Web UI (UsageController) i `TokenAuditTool` (tylko dla Kuratora)

---

### System kalendarzy

System zawiera **32 implementacje kalendarzy**, pochodzące od abstrakcyjnej klasy `CalendarBase`, obejmujące główne systemy kalendarzowe świata:

| Kalendarz | ID | Opis |
|-----------|-----|-------------|
| BuddhistCalendar | `buddhist` | Kalendarz buddyjski (BE), rok + 543 |
| CherokeeCalendar | `cherokee` | System kalendarza Czirokezów |
| ChineseLunarCalendar | `lunar` | Chiński kalendarz księżycowy, z miesiącami przestępnymi |
| ChineseHistoricalCalendar | `chinese_historical` | Chiński kalendarz historyczny, obsługa er ganzhi i er imperialnych |
| ChulaSakaratCalendar | `chula_sakarat` | Kalendarz Chula Sakarat (CS), rok - 638 |
| CopticCalendar | `coptic` | Kalendarz koptyjski |
| DaiCalendar | `dai` | Kalendarz Dai, z pełnymi obliczeniami księżycowymi |
| DehongDaiCalendar | `dehong_dai` | Odmiana kalendarza Dehong Dai |
| EthiopianCalendar | `ethiopian` | Kalendarz etiopski |
| FrenchRepublicanCalendar | `french_republican` | Francuski kalendarz republikański |
| GregorianCalendar | `gregorian` | Standardowy kalendarz gregoriański |
| HebrewCalendar | `hebrew` | Kalendarz hebrajski (żydowski) |
| IndianCalendar | `indian` | Indyjski kalendarz narodowy |
| InuitCalendar | `inuit` | System kalendarza Inuitów |
| IslamicCalendar | `islamic` | Muzułmański kalendarz hidżry |
| JapaneseCalendar | `japanese` | Japoński kalendarz er (Nengo) |
| JavaneseCalendar | `javanese` | Jawajski kalendarz islamski |
| JucheCalendar | `juche` | Kalendarz Dżucze (Korea Północna), rok - 1911 |
| JulianCalendar | `julian` | Kalendarz juliański |
| KhmerCalendar | `khmer` | Kalendarz khmerski |
| MayanCalendar | `mayan` | Długi kalendarz Majów |
| MongolianCalendar | `mongolian` | Kalendarz mongolski |
| PersianCalendar | `persian` | Kalendarz perski (słoneczna hidżra) |
| RepublicOfChinaCalendar | `roc` | Kalendarz Republiki Chińskiej (MinGuo), rok - 1911 |
| RomanCalendar | `roman` | Kalendarz rzymski |
| SakaCalendar | `saka` | Kalendarz Saka (Indonezja) |
| SexagenaryCalendar | `sexagenary` | Chiński kalendarz ganzhi (Sześćdziesięciolecie) |
| TibetanCalendar | `tibetan` | Kalendarz tybetański |
| VietnameseCalendar | `vietnamese` | Wietnamski kalendarz księżycowy (odmiana ze znakiem Kota) |
| VikramSamvatCalendar | `vikram_samvat` | Kalendarz Wikram Samwat |
| YiCalendar | `yi` | System kalendarza Yi |
| ZoroastrianCalendar | `zoroastrian` | Kalendarz zaratusztriański |

`CalendarTool` zapewnia operacje: `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (konwersja dat między kalendarzami).

---

## Architektura Web UI

### System skórek

Web UI posiada **system skórek typu plug-in**, pozwalający na pełne dostosowanie interfejsu bez zmiany logiki aplikacji:

- **Interfejs ISkin** — definiuje kontrakt dla wszystkich skórek, w tym:
  - Rdzenne metody renderowania (`RenderHtml`, `RenderError`)
  - 20+ metod komponentów UI (przyciski, pola wejściowe, karty, tabele, odznaki, dymki, postępy, etykiety itp.)
  - Generowanie motywu CSS przez `CssBuilder`
  - `SkinPreviewInfo` — paleta kolorów i ikony dla selektora skórek na stronie inicjalizacyjnej

- **Wbudowane skórki** — 7 gotowych do produkcji skórek:
  - **Admin** — profesjonalny, zorientowany na dane interfejs zarządzania systemem
  - **Chat** — konwersacyjny, zorientowany na wiadomości design do interakcji AI
  - **Creative** — artystyczny, bogaty wizualnie układ kreatywnych przepływów pracy
  - **Dev** — zorientowany na deweloperów interfejs z podświetlaniem składni
  - **HighContrast** — motyw o wysokim kontraście z ułatwieniami dostępu
  - **Light** — świeży jasny motyw
  - **Minimal** — minimalistyczny motyw

- **Odkrywanie skórek** — `SkinManager` automatycznie odkrywa i rejestruje wszystkie implementacje `ISkin` przez refleksję

### Konstruktory HTML / CSS / JS

Web UI całkowicie unika plików szablonów, generując wszystkie znaczniki w C#:

- **`H`** — strumieniowy konstruktor HTML DSL do budowania drzew HTML w kodzie
- **`CssBuilder`** — konstruktor CSS z obsługą selektorów i zapytań medialnych
- **`JsBuilder` (`JsSyntax`)** — konstruktor JavaScript dla skryptów inline

### System kontrolerów

Web UI jest zgodny ze **wzorcem podobnym do MVC**, z 22 kontrolerami obsługującymi różne aspekty:

| Kontroler | Przeznaczenie |
|------------|---------|
| About | Strona o projekcie i informacje o projekcie |
| Being | Zarządzanie i status Istot Krzemowych |
| Chat | Interfejs czatu w czasie rzeczywistym z SSE |
| ChatHistory | Przeglądanie historii czatu, obsługa listy sesji i szczegółów wiadomości |
| CodeBrowser | Przeglądanie i edycja kodu |
| CodeHover | Podpowiedzi kodu z podświetlaniem składni |
| Config | Zarządzanie konfiguracją systemu |
| Dashboard | Przegląd systemu i wskaźniki |
| Executor | Status i zarządzanie wykonawcami |
| Help | System dokumentacji pomocy, obsługa wielojęzyczna |
| Init | Kreator inicjalizacji pierwszego uruchomienia |
| Knowledge | Wizualizacja i zapytania grafu wiedzy |
| Log | Przeglądarka logów systemowych, obsługa filtrowania Istot Krzemowych |
| Memory | Przeglądarka pamięci długoterminowej, obsługa zaawansowanego filtrowania, statystyk i widoku szczegółów |
| Permission | Zarządzanie uprawnieniami |
| PermissionRequest | Kolejka żądań uprawnień |
| Project | Zarządzanie projektami, obejmujące notatki robocze i system zadań |
| System | Zarządzanie systemem i monitorowanie czasu działania |
| Task | Interfejs systemu zadań |
| Timer | Zarządzanie systemem timerów, obejmujące historię wykonania |
| Usage | Pulpit audytu użycia Tokenów, z wykresami trendów i eksportem |
| WorkNote | Zarządzanie notatkami roboczymi, obsługa wyszukiwania i generowania katalogów |

### Aktualizacje w czasie rzeczywistym

- **SSE (Server-Sent Events)** — aktualizacje wiadomości czatu, statusu istot i zdarzeń systemowych przekazywane przez `SSEHandler`
- **Brak WebSocket** — prostsza architektura używająca SSE dla większości potrzeb czasu rzeczywistego
- **Automatyczne ponowne łączenie** — logika ponownego łączenia klienta zapewniająca elastyczne połączenie

### Lokalizacja

System obsługuje pełną lokalizację w **29 wariantach językowych**:
- **Chiński (6 wariantów)**: zh-CN (uproszczony), zh-HK (tradycyjny), zh-SG (Singapur), zh-MO (Makau), zh-TW (Tajwan), zh-MY (Malezja)
- **Angielski (10 wariantów)**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY
- **Hiszpański (2 warianty)**: es-ES, es-MX
- **Niemiecki (5 wariantów)**: de-DE, de-AT, de-CH, de-LU, de-LI
- **Francuski (3 warianty)**: fr-FR, fr-CA, fr-CH
- **Inne (3 warianty)**: ja-JP (japoński), ko-KR (koreański), cs-CZ (czeski)

Aktywne ustawienie językowe jest wybierane przez `DefaultConfigData.Language` i rozwiązywane przez `LocalizationManager`.

---

### System automatyzacji przeglądarki WebView (nowość)

System integruje funkcję automatyzacji przeglądarki WebView opartą na **Playwright**:

- **Izolacja indywidualna**: każda Istota Krzemowa posiada niezależną instancję przeglądarki, ciasteczka i pamięć sesji, całkowicie odizolowane od siebie nawzajem.
- **Tryb bezgłowy**: przeglądarka działa w całkowicie niewidocznym dla użytkownika trybie bezgłowym, Istoty Krzemowe operują autonomicznie w tle.
- **WebViewBrowserTool**: zapewnia pełne możliwości operacji przeglądarki, w tym:
  - Nawigacja po stronach, klikanie, wprowadzanie tekstu, pobieranie treści strony
  - Wykonywanie JavaScript, robienie zrzutów ekranu, oczekiwanie na pojawienie się elementów
  - Zarządzanie stanem przeglądarki i czyszczenie zasobów
- **Kontrola bezpieczeństwa**: wszystkie operacje przeglądarki muszą przejść przez łańcuch weryfikacji uprawnień, zapobiegając złośliwemu dostępowi do stron.

### System sieci wiedzy (nowość)

System posiada wbudowany system grafu wiedzy oparty na **strukturze trójkowej**:

- **Reprezentacja wiedzy**: przy użyciu struktury trójkowej "podmiot-relacja-orzeczenie" (np.: Python-jest_językiem_programowania)
- **KnowledgeTool**: zapewnia zarządzanie pełnym cyklem życia wiedzy:
  - `add`/`query`/`update`/`delete` - podstawowe operacje CRUD
  - `search` - pełnotekstowe wyszukiwanie i dopasowywanie słów kluczowych
  - `get_path` - odkrywanie ścieżki powiązań między dwoma pojęciami
  - `validate` - sprawdzanie kompletności wiedzy
  - `stats` - analiza statystyczna sieci wiedzy
- **Trwałe przechowywanie**: trójki wiedzy są utrwalane w systemie plików, z obsługą zapytań z indeksem czasowym.
- **Ocena pewności**: każdy wpis wiedzy posiada ocenę pewności (0-1), obsługującą rozmyte dopasowanie i sortowanie wiedzy.
- **Klasyfikacja tagów**: obsługa dodawania tagów do wiedzy w celu ułatwienia kategoryzacji i wyszukiwania.

---

## Struktura katalogu danych

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # Plik duszy Kuratora
    │   ├── state.json       # Stan czasu działania
    │   ├── code.enc         # Zaszyfrowany kod niestandardowej klasy (AES)
    │   └── permission.enc   # Zaszyfrowane niestandardowe wywołanie zwrotne uprawnień (AES)
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```

---

## Silnik przechowywania SpeedyPack

SiliconLife.Fast używa autorskiego silnika przechowywania SpeedyPack (format .spk), zastępującego poprzednie rozwiązanie LiteDB, osiągając ekstremalną wydajność odczytu i zapisu.

### Projekt architektury

```
┌──────────────────────────────────────────────────────────┐
│                    SpeedyPack                             │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────────┐  │
│  │ DirectoryMap  │  │  EntryCache   │  │  WriteQueue   │  │
│  │ (mapowanie    │  │  (pamięć      │  │ (kolejka      │  │
│  │  katalogu     │  │   podręczna   │  │  zapisu       │  │
│  │  w pamięci)   │  │   wpisów)     │  │  asynchroniczn.)│  │
│  └──────┬───────┘  └──────┬───────┘  └───────┬───────┘  │
│         │                  │                   │          │
│  ┌──────▼──────────────────▼───────────────────▼───────┐  │
│  │              PackFileReader / PackFileWriter          │  │
│  │              (czytnik / zapisywacz plików pakietu)    │  │
│  └──────────────────────────┬──────────────────────────┘  │
│                              │                             │
│  ┌──────────────────────────▼──────────────────────────┐  │
│  │              Plik .spk (MessagePack + kompresja LZ4)  │  │
│  └─────────────────────────────────────────────────────┘  │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐                      │
│  │  FreeList     │  │ SpeedyPack   │                      │
│  │ (zarządzanie  │  │ AutoCompactor│                      │
│  │  wolną        │  │ (automatyczna│                      │
│  │  przestrzenią)│  │  kompresja)  │                      │
│  └──────────────┘  └──────────────┘                      │
└──────────────────────────────────────────────────────────┘
```

### Rdzenne komponenty

| Komponent | Opis |
|-----------|------|
| `SpeedyPack` | Klasa rdzenna, łącząca DirectoryMap, EntryCache i WriteQueue zapewniająca odczyt i zapis o niskim opóźnieniu |
| `DirectoryMap` | Mapowanie katalogu w pamięci, utrzymujące relację mapowania ścieżek wirtualnych do wpisów plików |
| `EntryCache` | Pamięć podręczna wpisów, oparta na TTL pamięć podręczna ostatnio dostępnych wpisów |
| `WriteQueue` | Asynchroniczna kolejka zapisu, kolejkująca operacje zapisu do wykonania na wątku w tle |
| `FreeList` | Zarządzanie wolną przestrzenią, śledzące wielokrotnie używalną przestrzeń w plikach .spk |
| `PackFileReader` | Czytnik plików pakietu, odczytujący dane z plików .spk |
| `PackFileWriter` | Zapisywacz plików pakietu, zapisujący dane do plików .spk |
| `SpeedyPackAutoCompactor` | Timer automatycznej kompresji, okresowo kompresujący pliki .spk w celu odzyskania wolnej przestrzeni |
| `SpeedyPackRegistry` | Menedżer singletonowy na poziomie procesu, zapewniający, że cała aplikacja używa tej samej instancji SpeedyPack |

### Adaptery przechowywania

SiliconLife.Fast integruje SpeedyPack z interfejsami systemowymi przez następujące adaptery:

| Adapter | Interfejs | Opis |
|---------|-----------|------|
| `SpeedyStorage` | `IStorage` | Adapter ogólnego magazynu klucz-wartość |
| `SpeedyTimeStorage` | `ITimeStorage` | Adapter przechowywania z indeksem czasowym |
| `SpeedyWorkNoteStorage` | `IWorkNoteStorage` | Adapter przechowywania notatek roboczych |

### Opcje konfiguracji

`SpeedyPackOptions` zapewnia następującą konfigurację:

| Opcja | Typ | Wartość domyślna | Opis |
|-------|------|------------------|------|
| `CacheTtl` | `TimeSpan` | 5 minut | Czas życia wpisów w pamięci podręcznej |
| `MaxCacheEntries` | `int` | 1000 | Maksymalna liczba wpisów w pamięci podręcznej |
| `ReadOnly` | `bool` | false | Tryb tylko do odczytu |

### Obsługa transakcji

SpeedyPack obsługuje atomowe operacje zapisu przez interfejs `IPackTransaction`:

- `SpeedyTransaction` implementuje mechanizm transakcji
- Obsługuje atomowość zapisów wsadowych
- Przy zatwierdzaniu transakcji wszystkie operacje zapisu kończą się sukcesem lub wszystkie są wycofywane

---

## System wtyczek

SiliconLife obsługuje rozszerzanie funkcjonalności przez system wtyczek, pozwalając deweloperom trzecim na dodawanie nowych funkcji do platformy.

### Rdzenny interfejs

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### Ładowarka wtyczek

`PluginLoader` odpowiada za ładowanie bibliotek DLL wtyczek z określonego katalogu i wykonywanie rygorystycznych kontroli bezpieczeństwa:

1. **Skanowanie katalogu** — skanowanie wszystkich plików .dll w katalogu wtyczek
2. **Skanowanie bezpieczeństwa** — sprawdzanie, czy wtyczka odwołuje się do zabronionych przestrzeni nazw
3. **Ładowanie izolowane** — używanie niestandardowego `AssemblyLoadContext` do izolowanego ładowania wtyczek
4. **Zarządzanie cyklem życia** — wywoływanie metod OnLoad, OnStart, OnStop, OnUnload wtyczki

### Piaskownica bezpieczeństwa

Ładowarka wtyczek wykonuje następujące kontrole bezpieczeństwa:

| Punkt kontroli | Opis |
|----------------|------|
| Zabronione przestrzenie nazw | System.IO, System.Net.Http, System.Net.WebSockets, System.Net.Sockets, Microsoft.CodeAnalysis |
| Biała lista zaufanych zestawów | Google.Protobuf, Newtonsoft.Json, MessagePack, Serilog, Microsoft.Extensions.Logging.Abstractions, Dapper |
| Sprawdzanie zabronionych typów | Skanowanie niebezpiecznych typów odwoływanych w wtyczce |
| Sprawdzanie zabronionych członków | Skanowanie niebezpiecznych metod wywoływanych w wtyczce |

### Integracja narzędzi

Wtyczki mogą rejestrować niestandardowe narzędzia implementując interfejs `ITool`:

- Metoda `ToolManager.ScanAllPluginAssemblies()` skanuje wszystkie załadowane wtyczki w poszukiwaniu implementacji ITool
- Narzędzia wtyczek są automatycznie integrowane z pętlą wywołań narzędzi
- Narzędzia wtyczek podlegają tym samym ograniczeniom systemu uprawnień

### Cykl życia wtyczki

```
Ładowanie (OnLoad) → Uruchomienie (OnStart) → Działanie → Zatrzymanie (OnStop) → Rozładowanie (OnUnload)
```

---

## Stany aktywności Istoty Krzemowej

Istoty Krzemowe posiadają następujące stany aktywności:

| Stan | Opis |
|------|------|
| `Idle` | Stan bezczynności, oczekiwanie na wyzwolenie zegara |
| `Working` | Wykonywanie jednej rundy żądania AI + wywołania narzędzi |
| `Error` | Wystąpił błąd podczas wykonywania |
| `Stopped` | Zatrzymana, z powodu kolejnych błędów lub ręcznego zatrzymania |

**Mechanizm stanu Stopped**:
- Gdy Istota Krzemowa napotka 10 kolejnych błędów, automatycznie przechodzi w stan `Stopped`
- Po wejściu w stan Stopped istota nie będzie wykonywać żadnych zadań
- Wymagana jest ręczna interwencja, aby ponownie uruchomić

Przejścia stanów:
```
Idle → Working → Idle (normalne zakończenie)
Working → Error → Working (odzyskiwanie po błędzie)
Working → Stopped (10 kolejnych błędów lub ręczne zatrzymanie)
Stopped → Idle (ponowne uruchomienie)
```
