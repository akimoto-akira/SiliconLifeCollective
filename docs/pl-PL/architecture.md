# Architektura

> **Wersja: v0.2.0-alpha**

[English](../en/architecture.md) | [Deutsch](../de-DE/architecture.md) | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Čeština](../cs-CZ/architecture.md) | [Русский](../ru-RU/architecture.md)

## Architektura dwuwersyjna

Projekt oferuje dwie wersje implementacji, współdzielące ten sam projekt architektury, ale różniące się w przechowywaniu i optymalizacji wydajności:

### SiliconLife.Default (wersja domyślna)
- **Pozycjonowanie**: domyślna implementacja, głównie do weryfikacji wykonalności architektury
- **Tryb działania**: aplikacja konsolowa
- **Metoda przechowywania**: przechowywanie JSON w czystym systemie plików
- **Scenariusze zastosowania**: wysokie wymagania bezpieczeństwa danych, ograniczone zasoby pamięci, mała ilość danych
- **Opis roli**: jako implementacja odniesienia do weryfikacji architektury, zapewnia prosta i niezawodny sposób uruchomienia, odpowiednia do pierwszego kontaktu z projektem, debugowania deweloperskiego lub scenariuszy priorytetowego bezpieczeństwa danych

### SiliconLife.Fast (wersja wysokowydajna)
- **Pozycjonowanie**: zalecana wersja produkcyjna
- **Tryb działania**: aplikacja desktopowa (Windows zasobnik systemowy / Linux okno statusu)
- **Metoda przechowywania**: przechowywanie w pamięci SpeedyPack + asynchroniczna trwałość wsadowa (format pliku .spk)
- **Scenariusze zastosowania**: wysoka współbieżność, niskie opóźnienie, duże ilości danych
- **Obsługa platform**: Windows/macOS (pełna funkcjonalność, z zasobnikiem systemowym), Linux (okno statusu, bez ikony zasobnika)
- **Cechy**:
  - Windows/macOS działanie w tle w zasobniku systemowym, monitorowanie w czasie rzeczywistym przez okno statusu zasobnika; Linux bezpośrednie wyświetlanie okna statusu
  - Silnik SpeedyPack + automatyczna kompakcja gwarantująca bezpieczeństwo danych
  - Architektura Component UI, 27 komponentów deklaratywnych
  - 7 motywów skórek, obsługa automatycznego wykrywania i przełączania
  - Narzędzie gorącego przeładowania obsługujące aktualizacje online i restart
  - Linux automatycznie otwiera przeglądarkę z Web UI, obsługa parametru `--no-tray`
- **Poprawa wydajności**: opóźnienie odczytu przechowywania zmniejszone 1000-krotnie, opóźnienie zapisu zmniejszone 15000-krotnie
- **Opis roli**: głęboko zoptymalizowana implementacja produkcyjna, z funkcjami takimi jak działanie w tle w zasobniku systemowym, silnik SpeedyPack + automatyczna kompakcja, zalecana do długotrwałego działania i rzeczywistych środowisk produkcyjnych

> **Uwaga**: Architektura opisana w tym dokumencie dotyczy obu wersji, z wyjątkiem części dotyczącej implementacji przechowywania. SiliconLife.Default służy jako punkt odniesienia do weryfikacji architektury, a SiliconLife.Fast jako zalecana wersja do środowisk produkcyjnych.

---

## Kluczowe koncepcje

### Istota Krzemowa

Każdy agent AI w systemie jest **Istotą Krzemową** — autonomiczną jednostką posiadającą własną tożsamość, osobowość i zdolności. Każda Istota Krzemowa jest napędzana przez **Plik Duszy** (podpowiedź Markdown), definiujący jej wzorce zachowań.

### Kurator Krzemowy

**Kurator Krzemowy** to specjalna Istota Krzemowa z najwyższymi uprawnieniami systemowymi. Pełni rolę administratora systemu:

- Tworzy i zarządza innymi Istotami Krzemowymi
- Analizuje żądania użytkowników i rozkłada je na zadania
- Rozdziela zadania odpowiednim Istotom Krzemowym
- Monitoruje jakość wykonania i obsługuje niepowodzenia
- Używa **priorytetowego planowania** do odpowiadania na wiadomości użytkowników (patrz poniżej)

### Plik Duszy

Plik Markdown (`soul.md`) przechowywany w katalogu danych każdej Istoty Krzemowej. Jest wstrzykiwany jako podpowiedź systemowa do każdego żądania AI, definiując osobowość istoty, wzorce decyzyjne i ograniczenia zachowań.

---

## Planowanie: sprawiedliwe planowanie w szczelinach czasowych

### Pętla główna + obiekty Tick

System uruchamia **sterowaną zegarem pętlę główną** na dedykowanym wątku w tle:

```
Pętla główna (dedykowany wątek, watchdog + circuit breaker)
  └── Obiekt Tick A (priorytet=0, interwał=100ms)
  └── Obiekt Tick B (priorytet=1, interwał=500ms)
  └── Menedżer Istot Krzemowych (bezpośrednio wyzwalany zegarem przez pętlę główną)
        └── Runner Istoty Krzemowej → Istota Krzemowa 1 → wyzwalenie zegarem → wykonanie jednej rundy
        └── Runner Istoty Krzemowej → Istota Krzemowa 2 → wyzwalenie zegarem → wykonanie jednej rundy
        └── Runner Istoty Krzemowej → Istota Krzemowa 3 → wyzwalenie zegarem → wykonanie jednej rundy
        └── ...
```

Kluczowe decyzje projektowe:

- **Istoty Krzemowe nie dziedziczą po obiekcie Tick.** Mają własną metodę `Tick()`, wywoływaną przez `SiliconBeingManager` za pomocą `SiliconBeingRunner`, a nie bezpośrednio rejestrowaną w pętli głównej.
- **Menedżer Istot Krzemowych** jest bezpośrednio wyzwalany zegarem przez pętlę główną i działa jako pojedynczy proxy dla wszystkich istot.
- **Runner Istoty Krzemowej** opakowuje metodę `Tick()` każdej istoty na wątku tymczasowym, z timeout i indywidualnym circuit breaker dla każdej istoty (3 kolejne przekroczenia czasu → 1 minuta ostygnięcia).
- Wykonanie każdej istoty jest ograniczone do **jednej rundy** żądania AI + wywołań narzędzi na wyzwalenie zegarem, zapewniając, że żadna istota nie zmonopolizuje pętli głównej.
- **Monitor Wydajności** śledzi czas wykonania zegara w celu zapewnienia obserwowalności.

### Priorytetowa odpowiedź Kuratora

Gdy użytkownik wysyła wiadomość do Kuratora Krzemowego:

1. Bieżąca istota (np. istota A) kończy swoją bieżącą rundę — **bez przerywania**.
2. Menedżer **pomija pozostałą kolejkę**.
3. Pętla **rozpoczyna się ponownie od Kuratora**, umożliwiając mu natychmiastowe wykonanie.

Zapewnia to odpowiedź na interakcje użytkownika bez zakłócania trwających zadań.

---

## Architektura komponentów

```
┌─────────────────────────────────────────────────────────┐
│                        Główny Host                       │
│  (ujednolicony host — asemblacja i zarządzanie wszystkimi komponentami)  │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ Pętla główna │  │ Lokalizator usług │  │   Konfiguracja   │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │        Menedżer Istot Krzemowych (obiekt Tick)     │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Kurator    │ │Istota A  │ │Istota B  │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              Usługi współdzielone                  │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │System czatu│ │Przechowywanie│ │Menedżer Uprawnień│  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │Klienci AI │ │Wykonawcy   │ │Menedżer Narzędzi │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  │  ┌──────────┐ ┌──────────┐                        │   │
│  │  │Ładowacz wtyczek│ │Sieć Wiedzy │                        │   │
│  │  └──────────┘ └──────────┘                        │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  Wykonawcy                         │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Wykonawca  │ │Wykonawca  │ │Wykonawca          │  │   │
│  │  │dyskowy    │ │sieciowy   │ │wiersza poleceń    │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              Dostawcy komunikatora                 │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Dostawca  │ │Dostawca  │ │Dostawca           │  │   │
│  │  │konsoli   │ │Web       │ │Feishu / ...       │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## Lokalizator usług

`ServiceLocator` to bezpieczny dla wątków rejestr singletonów, zapewniający dostęp do wszystkich głównych usług:

| Właściwość | Typ | Opis |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | Centralny menedżer sesji czatu |
| `IMManager` | `IMManager` | Router dostawców komunikatora |
| `AuditLogger` | `AuditLogger` | Ślad audytu uprawnień |
| `GlobalAcl` | `GlobalACL` | Globalna lista kontroli dostępu |
| `BeingFactory` | `ISiliconBeingFactory` | Fabryka tworzenia istot |
| `BeingManager` | `SiliconBeingManager` | Menedżer cyklu życia aktywnych istot |
| `DynamicBeingLoader` | `DynamicBeingLoader` | Ładowarka kompilacji dynamicznej |
| `TokenUsageAudit` | `ITokenUsageAudit` | Śledzenie wykorzystania tokenów |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Raportowanie wykorzystania tokenów |

Utrzymuje również rejestr `PermissionManager` dla każdej istoty, z kluczem GUID istoty.

---

## System czatu

### Typy sesji

System czatu obsługuje trzy typy sesji poprzez `SessionBase`:

| Typ | Klasa | Opis |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | Rozmowa jeden na jednego między dwoma uczestnikami |
| `GroupChat` | `GroupChatSession` | Czat grupowy z wieloma uczestnikami |
| `Broadcast` | `BroadcastChannel` | Otwarty kanał ze stałym ID; istoty dynamicznie subskrybują i otrzymują wiadomości tylko po subskrypcji |

### Kanał transmisyjny

`BroadcastChannel` to specjalny typ sesji używany do ogłoszeń ogólnosystemowych:

- **Stałe ID kanału** — w przeciwieństwie do `SingleChatSession` i `GroupChatSession`, ID kanału jest znaną stałą, a nie wyprowadzaną z identyfikatorów GUID członków.
- **Dynamiczna subskrypcja** — istoty subskrybują/odsubskrybują w czasie działania; otrzymują tylko wiadomości opublikowane po subskrypcji.
- **Filtrowanie oczekujących wiadomości** — `GetPendingMessages()` zwraca tylko wiadomości opublikowane po czasie subskrypcji istoty i jeszcze nieprzeczytane.
- **Zarządzany przez System Czatu** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### Wiadomość czatu

Model `ChatMessage` zawiera pola kontekstu konwersacji AI i śledzenia tokenów:

| Pole | Typ | Opis |
|-------|------|-------------|
| `Id` | `Guid` | Unikalny identyfikator wiadomości |
| `SenderId` | `Guid` | Unikalny identyfikator nadawcy |
| `ChannelId` | `Guid` | Identyfikator kanału/konwersacji |
| `Content` | `string` | Treść wiadomości |
| `Timestamp` | `DateTime` | Czas wysłania wiadomości |
| `Type` | `MessageType` | Tekst, obraz, plik lub powiadomienie systemowe |
| `ReadBy` | `List<Guid>` | Identyfikatory uczestników, którzy przeczytali tę wiadomość |
| `Role` | `MessageRole` | Rola konwersacji AI (użytkownik, asystent, narzędzie) |
| `ToolCallId` | `string?` | Identyfikator wywołania narzędzia dla wiadomości wyniku narzędzia |
| `ToolCallsJson` | `string?` | Zserializowany JSON wywołań narzędzi dla wiadomości asystenta |
| `Thinking` | `string?` | Łańcuch rozumowania AI |
| `PromptTokens` | `int?` | Liczba tokenów w podpowiedzi (wejście) |
| `CompletionTokens` | `int?` | Liczba tokenów w uzupełnieniu (wyjście) |
| `TotalTokens` | `int?` | Całkowita liczba wykorzystanych tokenów (wejście + wyjście) |
| `FileMetadata` | `FileMetadata?` | Metadane załączonego pliku (jeśli wiadomość zawiera plik) |

### Kolejka wiadomości czatu

`ChatMessageQueue` to bezpieczny dla wątków system kolejki wiadomości do zarządzania asynchronicznym przetwarzaniem wiadomości czatu:

- **Bezpieczeństwo wątków** - wykorzystuje mechanizm blokad zapewniający bezpieczny dostęp współbieżny
- **Przetwarzanie asynchroniczne** - obsługa asynchronicznego umieszczania i pobierania wiadomości z kolejki
- **Porządkowanie wiadomości** - zachowanie chronologicznego porządku wiadomości
- **Operacje wsadowe** - obsługa pobierania wiadomości w partiach

### Metadane pliku

`FileMetadata` zarządza informacjami o plikach załączonych do wiadomości czatu:

- **Informacje o pliku** - nazwa, rozmiar, typ, ścieżka pliku
- **Czas przesłania** - znacznik czasu przesłania pliku
- **Przesyłający** - identyfikator użytkownika lub Istoty Krzemowej, który przesłał plik

### Menedżer anulowania strumienia

`StreamCancellationManager` zapewnia mechanizm anulowania strumieniowych odpowiedzi AI:

- **Kontrola strumienia** - obsługa anulowania trwającej strumieniowej odpowiedzi AI
- **Oczyszczanie zasobów** - poprawne oczyszczanie powiązanych zasobów przy anulowaniu
- **Bezpieczeństwo współbieżności** - obsługa jednoczesnego zarządzania wieloma strumieniami

### Przegląd historii czatu

Nowa funkcja przeglądu historii czatu pozwala użytkownikom przeglądać historyczne rozmowy Istot Krzemowych:

- **Lista sesji** - wyświetlanie wszystkich historycznych sesji
- **Szczegóły wiadomości** - przeglądanie pełnej historii wiadomości
- **Widok osi czasu** - wyświetlanie wiadomości w porządku chronologicznym
- **Obsługa API** - dostarczanie RESTful API do pobierania danych sesji i wiadomości

---

## System klientów AI

System obsługuje wiele backendów AI poprzez interfejs `IAIClient`:

### OllamaClient

- **Typ**: lokalna usługa AI
- **Protokół**: natywne Ollama HTTP API (`/api/chat`, `/api/generate`)
- **Funkcje**: przesyłanie strumieniowe, wywołania narzędzi, lokalne hostowanie modeli
- **Konfiguracja**: `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (Alibaba Cloud Bailian)

- **Typ**: chmurowa usługa AI
- **Protokół**: API kompatybilne z OpenAI (`/compatible-mode/v1/chat/completions`)
- **Uwierzytelnianie**: Bearer token (klucz API)
- **Funkcje**: przesyłanie strumieniowe, wywołania narzędzi, treść wnioskowania (łańcuch myśli), wdrożenie wieloregionalne
- **Obsługiwane regiony**:
  - `beijing` — Chiny Północne 2 (Pekin)
  - `virginia` — USA (Wirginia)
  - `singapore` — Singapur
  - `hongkong` — Hongkong, Chiny
  - `frankfurt` — Niemcy (Frankfurt)
- **Obsługiwane modele** (dynamicznie odkrywane przez API, z listą zapasową):
  - **Seria Qwen**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **Wnioskowanie**: qwq-plus
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
- **Cechy**: usługa AI ByteDance, obsługa wielu modeli Doubao

### Wzorzec fabryki klientów

Każdy typ klienta AI ma odpowiednią implementację fabryki `IAIClientFactory`:

- `OllamaClientFactory` — tworzy instancje OllamaClient
- `DashScopeClientFactory` — tworzy instancje DashScopeClient
- `VolcengineArkClientFactory` — tworzy instancje VolcengineArkClient

Fabryki dostarczają:
- `CreateClient(Dictionary<string, object> config)` — tworzy instancję klienta z konfiguracji
- `GetConfigKeyOptions(string key, ...)` — zwraca dynamiczne opcje klucza konfiguracji (np. dostępne modele, regiony)
- `GetDisplayName()` — zlokalizowana nazwa wyświetlana typu klienta

### Lista obsługiwanych platform AI

#### Opis statusów
- ✅ Zaimplementowane
- 🚧 W rozwoju
- 📋 Planowane
- 💡 Rozważane

*Uwaga: Ze względu na środowisko sieciowe dewelopera, łączenie z rozważanymi zagranicznymi chmurowymi usługami AI może wymagać użycia narzędzi proxy sieciowego, a proces debugowania może być niestabilny.*

#### Lista platform

| Platforma | Status | Typ | Opis |
|------|------|------|------|
| Ollama | ✅ | Lokalna | Lokalna usługa AI, obsługa lokalnego wdrażania modeli |
| DashScope (Alibaba Cloud Bailian) | ✅ | Chmurowa | Usługa AI Alibaba Cloud Bailian, obsługa wdrożenia wieloregionalnego |
| Baidu Qianfan (Wenxin Yiyan) | 📋 | Chmurowa | Usługa AI Baidu Wenxin Yiyan |
| Zhipu AI (GLM) | 📋 | Chmurowa | Usługa AI Zhipu Qingyan |
| Moonshot (Kimi) | 📋 | Chmurowa | Usługa AI Moonshot Kimi |
| Volcengine Ark Doubao | ✅ | Chmurowa | Usługa AI ByteDance Doubao |
| DeepSeek (bezpośrednie połączenie) | 📋 | Chmurowa | Usługa AI DeepSeek |
| 01.AI (Yi) | 📋 | Chmurowa | Usługa AI 01.AI |
| Tencent Hunyuan | 📋 | Chmurowa | Usługa AI Tencent Hunyuan |
| SiliconFlow | 📋 | Chmurowa | Usługa AI SiliconFlow |
| MiniMax | 📋 | Chmurowa | Usługa AI MiniMax |
| OpenAI | 💡 | Chmurowa | Usługa OpenAI API (seria GPT) |
| Anthropic | 💡 | Chmurowa | Usługa AI Anthropic Claude |
| Google DeepMind | 💡 | Chmurowa | Usługa AI Google Gemini |
| Mistral AI | 💡 | Chmurowa | Usługa AI Mistral |
| Groq | 💡 | Chmurowa | Usługa wnioskowania AI wysokiej prędkości Groq |
| Together AI | 💡 | Chmurowa | Usługa modeli open source Together AI |
| xAI | 💡 | Chmurowa | Usługa xAI Grok |
| Cohere | 💡 | Chmurowa | Usługa NLP klasy enterprise Cohere |
| Replicate | 💡 | Chmurowa | Platforma hostingu modeli open source Replicate |
| Hugging Face | 💡 | Chmurowa | Społeczność i platforma modeli open source AI Hugging Face |
| Cerebras | 💡 | Chmurowa | Usługa optymalizacji wnioskowania AI Cerebras |
| Databricks | 💡 | Chmurowa | Platforma AI enterprise Databricks (MosaicML) |
| Perplexity AI | 💡 | Chmurowa | Usługa wyszukiwania i odpowiedzi AI Perplexity |
| NVIDIA NIM | 💡 | Chmurowa | Mikrousługi wnioskowania AI NVIDIA |

---

## Kluczowe decyzje projektowe

### Przechowywanie jako klasa instancji (nie statyczna)

`IStorage` jest zaprojektowane jako wstrzykiwalna instancja, a nie statyczne narzędzie. Zapewnia to:

- Bezpośredni dostęp do systemu plików — IStorage jest wewnętrznym kanałem trwałości systemu, **nie** kierowanym przez wykonawców.
- **AI nie kontroluje IStorage** — wykonawcy zarządzają IO inicjowanymi przez narzędzia AI; IStorage zarządza wewnętrznym odczytem/zapisem danych frameworka. To fundamentalnie różne kwestie.
- Możliwość testowania z implementacjami mock.
- Przyszła obsługa różnych backendów przechowywania bez modyfikacji konsumentów.

### Wykonawcy jako granica bezpieczeństwa

Wykonawcy są **jedyną** ścieżką operacji I/O. Narzędzia wymagające dostępu do dysku, sieci lub wiersza poleceń **muszą** przejść przez wykonawców. Ten projekt wymusza:

- Każdy wykonawca posiada **niezależny wątek dyspozytora** z blokadą wątku do weryfikacji uprawnień.
- Scentralizowane sprawdzanie uprawnień — wykonawcy odpytują **prywatny Menedżer Uprawnień** istoty.
- Kolejka żądań z obsługą priorytetów i kontrolą timeout.
- Dziennik audytu wszystkich zewnętrznych operacji.
- Izolacja wyjątków — awaria jednego wykonawcy nie wpływa na inne.
- Circuit breaker — kolejne niepowodzenia tymczasowo zatrzymują wykonawcę, zapobiegając kaskadowym awariom.

### Menedżer Kontekstu jako lekki obiekt

Każde wywołanie `ExecuteOneRound()` tworzy nową instancję `ContextManager`:

1. Ładuje Plik Duszy + ostatnią historię czatu.
2. Wysyła żądanie do klienta AI.
3. Przetwarza w pętli wywołania narzędzi, aż AI zwrócić czysty tekst.
4. Utrwala odpowiedź w systemie czatu.
5. Zwalnia zasoby.

Dzięki temu każda runda pozostaje izolowana i bezstanowa.

### Samewolucja poprzez nadpisanie klas

Istoty Krzemowe mogą w czasie działania nadpisywać własne klasy C#:

1. AI generuje nowy kod klasy (musi dziedziczyć po `SiliconBeingBase`).
2. **Kontrola referencji w czasie kompilacji** (główna obrona): kompilator otrzymuje tylko dozwoloną listę zestawów — `System.IO`, `System.Reflection` itp. są wykluczone, więc niebezpieczny kod jest niemożliwy na poziomie typu.
3. **Statyczna analiza w czasie działania** (obrona dodatkowa): `Skaner Bezpieczeństwa` skanuje kod pod kątem niebezpiecznych wzorców po udanej kompilacji.
4. Roslyn kompiluje kod w pamięci.
5. W przypadku sukcesu: `SiliconBeingManager.ReplaceBeing()` zamienia bieżącą instancję, migruje stan i utrwala zaszyfrowany kod na dysku.
6. W przypadku niepowodzenia: nowy kod jest odrzucany, zachowana zostaje istniejąca implementacja.

Niestandardowa implementacja `IPermissionCallback` może być również kompilowana i wstrzykiwana za pomocą `ReplacePermissionCallback()`, pozwalając istotom na dostosowanie własnej logiki uprawnień.

Kod jest przechowywany na dysku w formie zaszyfrowanej AES-256. Klucz szyfrowania jest wyprowadzany z identyfikatora GUID istoty (wielkie litery) za pomocą PBKDF2.

---

## Audyt wykorzystania tokenów

`TokenUsageAuditManager` śledzi zużycie tokenów AI przez wszystkie istoty:

- `TokenUsageRecord` — rekord dla każdego żądania (ID istoty, model, tokeny podpowiedzi, tokeny uzupełnienia, znacznik czasu)
- `TokenUsageSummary` — zagregowane statystyki
- `TokenUsageQuery` — parametry zapytania do filtrowania rekordów
- Utrwalane przez `ITimeStorage` do zapytań szeregów czasowych
- Dostępne przez Web UI (UsageController) i `TokenAuditTool` (tylko Kurator)

---

### System kalendarza

System zawiera **32 implementacje kalendarzy**, wyprowadzone z abstrakcyjnej klasy `CalendarBase`, obejmujące główne systemy kalendarzowe świata:

| Kalendarz | ID | Opis |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | Kalendarz buddyjski (BE), rok + 543 |
| CherokeeCalendar | `cherokee` | System kalendarza Czirokei |
| ChineseLunarCalendar | `lunar` | Chiński kalendarz księżycowy, z miesiącami przestępnymi |
| ChineseHistoricalCalendar | `chinese_historical` | Chiński kalendarz historyczny, obsługa er ganzhi i epok cesarskich |
| ChulaSakaratCalendar | `chula_sakarat` | Kalendarz Chula Sakarat (CS), rok - 638 |
| CopticCalendar | `coptic` | Kalendarz koptyjski |
| DaiCalendar | `dai` | Kalendarz Dai, z pełnymi obliczeniami księżycowymi |
| DehongDaiCalendar | `dehong_dai` | Wariant kalendarza Dai Dehong |
| EthiopianCalendar | `ethiopian` | Kalendarz etiopski |
| FrenchRepublicanCalendar | `french_republican` | Francuski kalendarz republikański |
| GregorianCalendar | `gregorian` | Standardowy kalendarz gregoriański |
| HebrewCalendar | `hebrew` | Kalendarz hebrajski (żydowski) |
| IndianCalendar | `indian` | Indyjski kalendarz narodowy |
| InuitCalendar | `inuit` | System kalendarza Inuitów |
| IslamicCalendar | `islamic` | Islamski kalendarz hidżry |
| JapaneseCalendar | `japanese` | Japoński kalendarz ery (Nengo) |
| JavaneseCalendar | `javanese` | Jawajski kalendarz islamski |
| JucheCalendar | `juche` | Kalendarz Dżucze (Korea Północna), rok - 1911 |
| JulianCalendar | `julian` | Kalendarz juliański |
| KhmerCalendar | `khmer` | Kalendarz khmerski |
| MayanCalendar | `mayan` | Majski kalendarz długiej rachuby |
| MongolianCalendar | `mongolian` | Kalendarz mongolski |
| PersianCalendar | `persian` | Kalendarz perski (słoneczny hidżra) |
| RepublicOfChinaCalendar | `roc` | Kalendarz Republiki Chińskiej (MinGuo), rok - 1911 |
| RomanCalendar | `roman` | Kalendarz rzymski |
| SakaCalendar | `saka` | Kalendarz Saka (Indonezja) |
| SexagenaryCalendar | `sexagenary` | Chiński kalendarz ganzhi (sześćdziesięcioletni) |
| TibetanCalendar | `tibetan` | Kalendarz tybetański |
| VietnameseCalendar | `vietnamese` | Wietnamski kalendarz księżycowy (wariant z kotem zodiakalnym) |
| VikramSamvatCalendar | `vikram_samvat` | Kalendarz Vikram Samvat |
| YiCalendar | `yi` | System kalendarza Yi |
| ZoroastrianCalendar | `zoroastrian` | Kalendarz zaratusztriański |

`CalendarTool` dostarcza operacje: `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (konwersja dat między kalendarzami).

---

## Architektura Web UI

### System skórek

Web UI posiada **wtyczkowy system skórek**, pozwalający na pełne dostosowanie UI bez zmiany logiki aplikacji:

- **Interfejs ISkin** — definiuje kontrakt dla wszystkich skórek, w tym:
  - Główne metody renderowania (`RenderHtml`, `RenderError`)
  - 20+ metod komponentów UI (przycisk, pole wejściowe, karta, tabela, odznaka, dymek, pasek postępu, zakładka itp.)
  - Generowanie motywu CSS przez `CssBuilder`
  - `SkinPreviewInfo` — paleta kolorów i ikona dla selektora skórek na stronie inicjalizacyjnej

- **Wbudowane skórki** — 7 produkcyjnie gotowych skórek:
  - **Admin** — profesjonalny, zorientowany na dane interfejs zarządzania systemem
  - **Chat** — konwersacyjny, zorientowany na wiadomości design do interakcji AI
  - **Creative** — artystyczny, bogaty wizualnie układ kreatywnego przepływu pracy
  - **Dev** — zorientowany na deweloperów, zorientowany na kod interfejs z podświetlaniem składni
  - **HighContrast** — motyw o wysokim kontraście z dostępnością
  - **Light** — świeży jasny motyw
  - **Minimal** — motyw minimalistyczny

- **Odkrywanie skórek** — `SkinManager` automatycznie odkrywa i rejestruje wszystkie implementacje `ISkin` przez refleksję

### Konstruktory HTML / CSS / JS

Web UI całkowicie unika plików szablonów, generując wszystkie znaczniki w C#:

- **`H`** — strumieniowy DSL konstruktora HTML do budowania drzew HTML w kodzie
- **`CssBuilder`** — konstruktor CSS z obsługą selektorów i zapytań medialnych
- **`JsBuilder` (`JsSyntax`)** — konstruktor JavaScript do skryptów inline

### System kontrolerów

Web UI stosuje **wzorzec podobny do MVC**, z 24 kontrolerami obsługującymi różne aspekty:

| Kontroler | Przeznaczenie |
|------------|---------|
| About | Strona o projekcie i informacje o projekcie |
| Audit | Panel audytu wykorzystania tokenów |
| Being | Zarządzanie Istotami Krzemowymi i status |
| Chat | Interfejs czatu w czasie rzeczywistym z SSE |
| ChatHistory | Przegląd historii czatu, z listą sesji i szczegółami wiadomości |
| CodeBrowser | Przeglądanie i edycja kodu |
| CodeHover | Podpowiedzi kodu z podświetlaniem składni |
| Config | Zarządzanie konfiguracją systemu |
| Dashboard | Przegląd systemu i wskaźniki |
| Executor | Status i zarządzanie wykonawcami |
| Help | System dokumentacji pomocy, obsługa wielojęzyczna |
| Init | Kreator inicjalizacji pierwszego uruchomienia |
| Knowledge | Wizualizacja grafu wiedzy i zapytania |
| Log | Przeglądarka logów systemowych z filtrowaniem Istot Krzemowych |
| Memory | Przeglądarka pamięci długoterminowej z zaawansowanym filtrowaniem, statystykami i widokiem szczegółów |
| Permission | Zarządzanie uprawnieniami |
| PermissionRequest | Kolejka żądań uprawnień |
| Project | Zarządzanie projektami z notatkami pracy, systemem zadań i uprawnieniami narzędzi |
| System | Zarządzanie systemem i monitorowanie w czasie działania |
| Task | Interfejs systemu zadań |
| Timer | Zarządzanie systemem czasomierzy z historią wykonania |
| ToolPermission | Zarządzanie uprawnieniami narzędzi, obsługa konfiguracji uprawnień na poziomie Istot Krzemowych i projektów |
| Usage | Panel audytu wykorzystania tokenów z wykresami trendów i eksportem |
| WorkNote | Zarządzanie notatkami pracy z wyszukiwaniem i generowaniem spisu treści |

### Aktualizacje w czasie rzeczywistym

- **SSE (Server-Sent Events)** — aktualizacje wiadomości czatu, statusu istot i zdarzeń systemowych przez `SSEHandler`
- **Brak WebSocket** — prostsza architektura wykorzystująca SSE do większości potrzeb w czasie rzeczywistym
- **Automatyczne ponowne łączenie** — logika ponownego łączenia klienta zapewniająca elastyczne połączenie

### Lokalizacja

System obsługuje pełną lokalizację w **34 wariantach językowych**:
- **Chiński (6)**: zh-CN (uproszczony), zh-HK (tradycyjny), zh-SG (Singapur), zh-MO (Makau), zh-TW (Tajwan), zh-MY (Malezja)
- **Angielski (10)**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY
- **Hiszpański (2)**: es-ES, es-MX
- **Niemiecki (5)**: de-DE, de-AT, de-CH, de-LU, de-LI
- **Francuski (3)**: fr-FR, fr-CA, fr-CH
- **Inne (8)**: ja-JP (japoński), ko-KR (koreański), cs-CZ (czeski), it-IT (włoski), pl-PL (polski), pt-PT (portugalski), pt-BR (brazylijski portugalski), ru-RU (rosyjski)

Aktywne środowisko językowe jest wybierane przez `DefaultConfigData.Language` i rozwiązywane przez `LocalizationManager`.

---

### System automatyzacji przeglądarki WebView (nowość)

System integruje funkcję automatyzacji przeglądarki WebView opartą na **Playwright**:

- **Indywidualna izolacja**: każda Istota Krzemowa posiada niezależną instancję przeglądarki, ciasteczka i pamięć sesji, całkowicie odizolowane i niekolidujące ze sobą.
- **Tryb headless**: przeglądarka działa w całkowicie niewidocznym dla użytkownika trybie headless, Istota Krzemowa operuje autonomicznie w tle.
- **WebViewBrowserTool**: zapewnia pełne możliwości operacji przeglądarki, w tym:
  - Nawigacja po stronach, klikanie, wprowadzanie tekstu, pobieranie treści strony
  - Wykonywanie JavaScript, pobieranie zrzutów ekranu, oczekiwanie na pojawienie się elementów
  - Zarządzanie stanem przeglądarki i oczyszczanie zasobów
- **Kontrola bezpieczeństwa**: wszystkie operacje przeglądarki muszą przejść przez łańcuch weryfikacji uprawnień, zapobiegając złośliwym dostępom do stron.

### System Sieci Wiedzy (nowość)

System posiada wbudowany system grafu wiedzy oparty na **strukturze trójkowej**:

- **Reprezentacja wiedzy**: wykorzystuje strukturę trójkową „podmiot-relacja-obiekt" (np. Python-is_a-programming_language)
- **KnowledgeTool**: zapewnia zarządzanie pełnym cyklem życia wiedzy:
  - `add`/`query`/`update`/`delete` - podstawowe operacje CRUD
  - `search` - pełnotekstowe wyszukiwanie i dopasowywanie słów kluczowych
  - `get_path` - odkrywanie ścieżek powiązań między dwoma pojęciami
  - `validate` - sprawdzanie kompletności wiedzy
  - `stats` - analiza statystyczna sieci wiedzy
- **Trwałe przechowywanie**: trójki wiedzy są utrwalane w systemie plików, z obsługą zapytań indeksowanych czasowo.
- **Ocena pewności**: każdy wpis wiedzy posiada ocenę pewności (0-1), obsługującą rozmyte dopasowanie i sortowanie wiedzy.
- **Klasyfikacja tagami**: obsługa dodawania tagów do wiedzy, ułatwiająca kategoryzację i wyszukiwanie.

---

## Struktura katalogu danych

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # Plik Duszy Kuratora
    │   ├── state.json       # Stan w czasie działania
    │   ├── code.enc         # Zaszyfrowany kod AES niestandardowej klasy
    │   └── permission.enc   # Zaszyfrowane AES wywołanie zwrotne niestandardowych uprawnień
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```

---

## Silnik przechowywania SpeedyPack

SiliconLife.Fast wykorzystuje autorski silnik przechowywania SpeedyPack (format .spk), zastępując poprzednie rozwiązanie LiteDB, realizując ekstremalną wydajność odczytu i zapisu.

### Projekt architektury

```
┌──────────────────────────────────────────────────────────┐
│                    SpeedyPack                             │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────────┐  │
│  │ DirectoryMap  │  │  EntryCache   │  │  WriteQueue   │  │
│  │ (mapowanie katalogów w pamięci) │  │  (pamięć podręczna wpisów) │  │ (asynchroniczna kolejka zapisu) │  │
│  └──────┬───────┘  └──────┬───────┘  └───────┬───────┘  │
│         │                  │                   │          │
│  ┌──────▼──────────────────▼───────────────────▼───────┐  │
│  │              PackFileReader / PackFileWriter          │  │
│  │              (czytnik/zapisywacz plików pakietu)       │  │
│  └──────────────────────────┬──────────────────────────┘  │
│                              │                             │
│  ┌──────────────────────────▼──────────────────────────┐  │
│  │              plik .spk (MessagePack + kompresja LZ4)  │  │
│  └─────────────────────────────────────────────────────┘  │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐                      │
│  │  FreeList     │  │ SpeedyPack   │                      │
│  │ (zarządzanie wolną przestrzenią) │  │ AutoCompactor│                      │
│  │              │  │ (automatyczna kompakcja) │                      │
│  └──────────────┘  └──────────────┘                      │
└──────────────────────────────────────────────────────────┘
```

### Główne komponenty

| Komponent | Opis |
|------|------|
| `SpeedyPack` | Klasa główna, łącząca DirectoryMap, EntryCache i WriteQueue zapewniająca odczyt/zapis o niskim opóźnieniu |
| `DirectoryMap` | Mapowanie katalogów w pamięci, utrzymujące relację mapowania ścieżek wirtualnych do wpisów plików |
| `EntryCache` | Pamięć podręczna wpisów, oparta na TTL pamięć podręczna ostatnio dostępnych wpisów |
| `WriteQueue` | Asynchroniczna kolejka zapisu, kolejkująca operacje zapisu do wykonania w wątku w tle |
| `FreeList` | Zarządzanie wolną przestrzenią, śledzące wielokrotnie używaną przestrzeń w plikach .spk |
| `PackFileReader` | Czytnik plików pakietu, odczytujący dane z plików .spk |
| `PackFileWriter` | Zapisywacz plików pakietu, zapisujący dane do plików .spk |
| `SpeedyPackAutoCompactor` | Automatyczny kompaktor czasowy, okresowo kompakujący pliki .spk w celu odzyskania wolnej przestrzeni |
| `SpeedyPackRegistry` | Menedżer singletonów na poziomie procesu, zapewniający, że cała aplikacja używa tej samej instancji SpeedyPack |

### Adaptery przechowywania

SiliconLife.Fast integruje SpeedyPack z interfejsami systemowymi poprzez następujące adaptery:

| Adapter | Interfejs | Opis |
|--------|------|-------------|
| `SpeedyStorage` | `IStorage` | Adapter ogólnego przechowywania klucz-wartość |
| `SpeedyTimeStorage` | `ITimeStorage` | Adapter przechowywania indeksowanego czasowo |
| `SpeedyWorkNoteStorage` | `IWorkNoteStorage` | Adapter przechowywania notatek pracy |

### Opcje konfiguracji

`SpeedyPackOptions` dostarcza następujące opcje konfiguracji:

| Opcja | Typ | Wartość domyślna | Opis |
|------|------|--------|------|
| `CacheTtl` | `TimeSpan` | 5 minut | Czas życia wpisów w pamięci podręcznej |
| `MaxCacheEntries` | `int` | 1000 | Maksymalna liczba wpisów w pamięci podręcznej |
| `ReadOnly` | `bool` | false | Tryb tylko do odczytu |

### Obsługa transakcji

SpeedyPack obsługuje atomowe operacje zapisu poprzez interfejs `IPackTransaction`:

- `SpeedyTransaction` implementuje mechanizm transakcji
- Obsługuje atomowość zapisów wsadowych
- Przy zatwierdzaniu transakcji wszystkie operacje zapisu kończą się sukcesem lub wszystkie są wycofywane

---

## System wtyczek

SiliconLife obsługuje rozszerzanie funkcjonalności poprzez system wtyczek, pozwalając zewnętrznym deweloperom na dodawanie nowych funkcji do platformy.

### Główne interfejsy

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
3. **Ładowanie izolowane** — wykorzystanie niestandardowego `AssemblyLoadContext` do izolowanego ładowania wtyczek
4. **Zarządzanie cyklem życia** — wywoływanie metod OnLoad, OnStart, OnStop, OnUnload wtyczek

### Bezpieczna piaskownica

Ładowarka wtyczek wykonuje następujące kontrole bezpieczeństwa:

| Punkt kontroli | Opis |
|--------|------|
| Zabronione przestrzenie nazw | System.IO, System.Net.Http, System.Net.WebSockets, System.Net.Sockets, Microsoft.CodeAnalysis |
| Biała lista zaufanych zestawów | Google.Protobuf, Newtonsoft.Json, MessagePack, Serilog, Microsoft.Extensions.Logging.Abstractions, Dapper |
| Sprawdzanie zabronionych typów | Skanowanie niebezpiecznych typów odwoływanych w wtyczce |
| Sprawdzanie zabronionych członków | Skanowanie niebezpiecznych metod wywoływanych w wtyczce |

### Integracja narzędzi

Wtyczki mogą rejestrować niestandardowe narzędzia poprzez implementację interfejsu `ITool`:

- Metoda `ToolManager.ScanAllPluginAssemblies()` skanuje wszystkie załadowane wtyczki w poszukiwaniu implementacji ITool
- Narzędzia wtyczek automatycznie integrują się z pętlą wywołań narzędzi
- Narzędzia wtyczek podlegają tym samym ograniczeniom systemu uprawnień

### Cykl życia wtyczki

```
Ładowanie (OnLoad) → Uruchomienie (OnStart) → Działanie → Zatrzymanie (OnStop) → Rozładowanie (OnUnload)
```

---

## Stan aktywności Istoty Krzemowej

Istoty Krzemowe posiadają następujące stany aktywności:

| Stan | Opis |
|------|------|
| `Idle` | Stan bezczynności, oczekiwanie na wyzwalanie zegarem |
| `SingleChat` | Trwa czat jeden na jednego |
| `GroupChat` | Trwa czat grupowy |
| `Task` | Wykonywanie zadania |
| `Timer` | Wykonywanie czasomierza |
| `Stopped` | Zatrzymana, z powodu kolejnych błędów lub ręcznego zatrzymania |

**Mechanizm stanu Stopped**:
- Gdy Istota Krzemowa napotka 10 kolejnych błędów, automatycznie przechodzi w stan `Stopped`
- Po wejściu w stan Stopped, istota nie będzie wykonywać żadnych zadań
- Gdy nadejdzie nowa wiadomość czatu, licznik błędów jest resetowany, a istota wznawia działanie

Przejścia stanów:
```
Idle → SingleChat → Idle (czat zakończony)
Idle → GroupChat → Idle (czat grupowy zakończony)
Idle → Task → Idle (zadanie zakończone)
Idle → Timer → Idle (czasomierz zakończony)
Dowolny → Stopped (10 kolejnych błędów)
Stopped → Idle (nowa wiadomość czatu lub ręczny restart)
```

---

## Silnik przepływu pracy

Silnik przepływu pracy to oparty na szablonach system maszyny stanów, służący do napędzania procesów współpracy Istot Krzemowych w przestrzeni projektowej:

### Główne komponenty

| Komponent | Opis |
|------|------|
| `WorkflowEngine` | Rdzeń silnika przepływu pracy, zarządzający szablonami i instancjami, wykonujący przejścia stanów sterowane Tick |
| `WorkflowTemplate` | Szablon przepływu pracy, definiujący zbiór stanów i reguł przejść |
| `WorkflowInstance` | Instancja przepływu pracy, powiązana z konkretnym projektem, śledząca bieżący stan |
| `WorkflowLog` | Dziennik przepływu pracy, rejestrujący historię przejść stanów |

### Mechanizm działania

- **Rejestracja szablonów**: rejestracja szablonów przepływu pracy przez `RegisterTemplate()`, definiująca stany i reguły przejść
- **Tworzenie instancji**: tworzenie instancji z szablonu, powiązanie z przestrzenią projektową
- **Sterowanie Tick**: przejścia stanów napędzane przez mechanizm Tick pętli głównej
- **Rejestrowanie logów**: wszystkie przejścia stanów są automatycznie rejestrowane w dzienniku

---

## Mechanizm zanikania pamięci

`MemoryFadeService` to usługa okresowego zanikania, symulująca właściwość zapominania pamięci biologicznej:

### Mechanizm działania

- **Okresowe wykonywanie**: dziedziczy po `TickObject`, domyślnie wykonuje cykl zanikania co godzinę
- **Zanikanie ważności**: stosuje algorytm zanikania do wpisów pamięci każdej Istoty Krzemowej, obniżając ocenę ważności
- **Automatyczna archiwizacja**: pamięć o ważności poniżej progu jest automatycznie archiwizowana (`ArchiveFadingMemories()`)
- **Śledzenie statystyk**: rejestrowanie statystyk takich jak liczba cykli zanikania, liczba wpisów ze zmienionym stanem

### Przepływ zanikania

```
MemoryFadeService.OnTick()
  └── Iteracja przez wszystkie Istoty Krzemowe
       └── being.Memory.ApplyDecay()      # Zastosowanie zanikania ważności
       └── being.Memory.ArchiveFadingMemories()  # Archiwizacja pamięci o niskiej ważności
```

---

## System przestrzeni projektowej

Przestrzeń projektowa to mechanizm zarządzania przestrzenią wspierającą współpracę wielu Istot Krzemowych:

### Główne funkcje

- **Cykl życia projektu**: tworzenie → aktywny → archiwizacja → zniszczenie
- **Przypisywanie ról**: obsługa przypisywania ról projektowych Istotom Krzemowym
- **Izolacja uprawnień narzędzi**: konfiguracja uprawnień narzędzi na poziomie projektu, niezależna od uprawnień na poziomie Istoty Krzemowej
- **Notatki pracy**: system notatek stronicowych w przestrzeni projektowej, z obsługą generowania spisu treści i wyszukiwania słów kluczowych
- **Śledzenie zadań**: zarządzanie zadaniami na poziomie projektu, z obsługą tworzenia, przypisywania i śledzenia statusu
- **Integracja przepływu pracy**: projekty mogą być powiązane z szablonami przepływu pracy, napędzającymi procesy współpracy

### Powiązane narzędzia

| Narzędzie | Przeznaczenie |
|------|------|
| `ProjectTool` | Zarządzanie przestrzenią projektową (tworzenie, archiwizacja, niszczenie, przypisywanie ról) |
| `ProjectTaskTool` | Zarządzanie zadaniami projektu (tworzenie, przypisywanie, aktualizacja statusu) |
| `ProjectWorkNoteTool` | Notatki pracy projektu (tworzenie, wyszukiwanie, generowanie spisu treści) |
| `ProjectWorkTool` | Operacje pracy projektu (tworzenie zadań, czat grupowy, transmisja, ukończenie projektu) |
