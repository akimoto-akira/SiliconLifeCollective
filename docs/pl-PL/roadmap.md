# Plan rozwoju

> **Wersja: v0.2.0-alpha**

[English](../en/roadmap.md) | [Deutsch](../de-DE/roadmap.md) | [中文](../zh-CN/roadmap.md) | [繁體中文](../zh-HK/roadmap.md) | [Español](../es-ES/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Čeština](../cs-CZ/roadmap.md) | [Русский](../ru-RU/roadmap.md)

## Plan rozwoju dwóch wersji

### SiliconLife.Default (wersja domyślna)
- **Pozycja**: implementacja domyślna, głównie do weryfikacji wykonalności architektury
- **Stan obecny**: etapy 1–10.6 ukończone, system działa stabilnie
- **Opis roli**: jako implementacja referencyjna weryfikacji architektury, zapewniająca poprawność i wykonalność głównego projektu architektonicznego

### SiliconLife.Fast (wersja wysokowydajna)
- **Pozycja**: rekomendowana wersja produkcyjna
- **Stan obecny**: zakończono migrację podstawowej architektury, silnik przechowywania SpeedyPack i system wtyczek zostały zaimplementowane
- **Opis roli**: na podstawie architektury zweryfikowanej w wersji Default, przeprowadza głęboką optymalizację wydajności i wzmocnienie cech produkcyjnych — jest to preferowany wybór do faktycznego wdrożenia

**Plan rozwoju wersji Fast**:
- ✅ Etap 1: migracja podstawowej struktury projektu i systemu konfiguracji
- ✅ Etap 2: migracja Web UI i kontrolerów
- ✅ Etap 3: optymalizacja systemu przechowywania (pamięć masowa SpeedyPack + asynchroniczne utrwalanie)
- ✅ Etap 3.5: narzędzie zarządzania SpeedyPack (aplikacja Avalonia UI SiliconLife.Speedy.Manager)
- ✅ Etap 3.6: system wtyczek (interfejs IPlugin, bezpieczna piaskownica, izolacja AssemblyLoadContext)
- ✅ Etap 4: aplikacja okienkowa Avalonia (wieloplatformowa aplikacja desktopowa, zasobnik systemowy Windows/macOS, okno stanu Linux)

---

## Zasady przewodnie

Każdy etap kończy się **działającym, obserwowalnym** systemem. Żaden etap nie tworzy „sterty infrastruktury bez niczego do pokazania".

---

## ~~Etap 1: Można rozmawiać~~ ✅ Ukończono

**Cel**: wejście konsoli → wywołanie AI → wyjście konsoli. Minimalna weryfikowalna jednostka.

| # | Moduł | Opis |
|---|--------|-------------|
| 1.1 | Rozwiązanie i struktura projektu | Utworzenie `SiliconLifeCollective.sln` z `src/SiliconLife.Core/` (biblioteka podstawowa) i `src/SiliconLife.Default/` (implementacja domyślna + punkt wejścia) |
| 1.2 | Konfiguracja (minimalna) | Singleton + deserializacja JSON. Odczyt `config.json`. Automatyczne generowanie wartości domyślnych w przypadku braku |
| 1.3 | Lokalizacja (minimalna) | Klasa abstrakcyjna `LocalizationBase`, implementacja `ZhCN`. Dodanie `Language` do konfiguracji |
| 1.4 | OllamaClient (minimalny) | Interfejs `IAIClient`, wywołanie HTTP lokalnego Ollama `/api/chat`. Brak strumieniowania, brak wywołań narzędzi |
| 1.5 | Wejście/wyjście konsoli | `while(true) + Console.ReadLine()`, odczyt wejścia → wywołanie AI → wyświetlenie odpowiedzi |
| 1.6 | Nagłówek licencji | Dodanie nagłówka Apache 2.0 do wszystkich plików źródłowych C# |

**Dostarczane**: program rozmowy konsolowej z lokalnym modelem Ollama.

**Weryfikacja**: uruchom program, wpisz „hello", zobacz odpowiedź AI.

---

## ~~Etap 2: Ma szkielet~~ ✅ Ukończono

**Cel**: zastąpienie „gołej pętli" strukturą szkieletową. Zachowanie bez zmian.

| # | Moduł | Opis |
|---|--------|-------------|
| 2.1 | Przechowywanie (minimalne) | Interfejs `IStorage` (Read/Write/Exists/Delete, pary klucz-wartość). Implementacja `FileSystemStorage`. Klasa instancyjna (niestatyczna). Bezpośredni dostęp do systemu plików — **AI nie ma kontroli nad IStorage** |
| 2.2 | Pętla główna + obiekty Tick | Nieskończona pętla, precyzyjne interwały czasowe (`Stopwatch` + `Thread.Sleep`). Harmonogram z priorytetami |
| 2.3 | Standaryzacja IAIClient | Interfejs `IAIClientFactory`. Refaktoryzacja OllamaClient do standardowego interfejsu |
| 2.4 | Migracja konsoli | Migracja `while(true)` do obiektu Tick napędzanego pętlą główną. Zachowanie identyczne z etapem 1 |

**Dostarczane**: pętla główna uruchamia Tick, rozmowa konsolowa nadal działa.

**Weryfikacja**: zarejestruj testowy obiekt Tick drukujący licznik co sekundę; rozmowa konsolowa nadal działa.

---

## ~~Etap 3: Ma duszę~~ ✅ Ukończono

**Cel**: pierwsza Istota Krzemowa żyje w szkieletie.

| # | Moduł | Opis |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Abstrakcyjna klasa bazowa z Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService. Abstrakcyjne `Tick()` i `ExecuteOneRound()` |
| 3.2 | Ładowanie Pliku Duszy | `SoulFileManager`: odczyt `soul.md` z katalogu danych istoty |
| 3.3 | ContextManager (minimalny) | Połączenie Pliku Duszy + ostatnie wiadomości → wywołanie AI → uzyskanie odpowiedzi. Brak wywołań narzędzi, brak utrwalania |
| 3.4 | ISiliconBeingFactory | Interfejs fabryki do tworzenia instancji istot |
| 3.5 | SiliconBeingManager (minimalny) | Dziedziczy obiekt Tick (priorytet=0). Iteruje wszystkie istoty, wywołuje ich Tick po kolei |
| 3.6 | DefaultSiliconBeing | Standardowa implementacja zachowania. Sprawdza nieprzeczytane wiadomości → tworzy ContextManager → ExecuteOneRound → wyjście |
| 3.7 | Struktura katalogów istoty | `DataDirectory/SiliconManager/{GUID}/`, zawierająca `soul.md` i `state.json` |

**Dostarczane**: Istota Krzemowa napędzana pętlą główną, przyjmująca wejście konsolowe, ładująca Plik Duszy, wywołująca AI.

**Weryfikacja**: wejście konsolowe → wyzwolenie Tick pętli głównej → przetwarzanie istoty (z zachowaniem kierowanym przez Plik Duszy) → odpowiedź AI. Styl odpowiedzi powinien różnić się od etapu 1.

---

## ~~Etap 4: Ma pamięć~~ ✅ Ukończono

**Cel**: rozmowy są utrwalane po ponownym uruchomieniu.

| # | Moduł | Opis |
|---|--------|-------------|
| 4.1 | ChatSystem | Koncepcja kanału (dwa GUID = jeden kanał). Model wiadomości z utrwalaniem. Brak czatu grupowego |
| 4.2 | IIMProvider + IMManager | Interfejs `IIMProvider`. `ConsoleProvider` jako oficjalny kanał komunikatora. `IMManager` kieruje wiadomościami |
| 4.3 | Ulepszenie ContextManager | Pobieranie historii z systemu czatu. Utrwalanie odpowiedzi AI. Obsługa wieloetapowej kontynuacji wywołań narzędzi |
| 4.4 | Model IMessage | Ujednolicony model wiadomości współdzielony przez system czatu i menedżera komunikatora |

**Dostarczane**: system czatu z utrwaloną pamięcią.

**Weryfikacja**: kilka rund czatu → wyjście → ponowne uruchomienie → zapytaj „o czym rozmawialiśmy?" → istota potrafi odpowiedzieć.

---

## ~~Etap 5: Może działać (system narzędzi)~~ ✅ Ukończono

**Cel**: Istoty Krzemowe mogą wykonywać operacje, a nie tylko rozmawiać.

| # | Moduł | Opis |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | Interfejs `ITool` z Name, Description, Execute. `ToolResult` zawiera Success, Message, Data |
| 5.2 | ToolManager | Instancja dla każdej istoty. Odkrywanie narzędzi oparte na refleksji. Obsługa atrybutu `[SiliconManagerOnly]` |
| 5.3 | IAIClient: obsługa wywołań narzędzi | Parsowanie AI tool_calls. Pętla: wykonaj narzędzie → wyślij wyniki z powrotem → AI kontynuuje → aż do czystego tekstu |
| 5.4 | Klasa bazowa wykonawcy | Abstrakcyjna klasa bazowa z niezależnym wątkiem dyspozytora, kolejką żądań, kontrolą limitu czasu |
| 5.5 | NetworkExecutor | Żądania HTTP przez wykonawcę. Limit czasu, kolejkowanie |
| 5.6 | CommandLineExecutor | Wykonywanie poleceń powłoki przez wykonawcę. Wykrywanie separatorów wieloplatformowych |
| 5.7 | DiskExecutor | Operacje na plikach przez wykonawcę. Brak sprawdzania uprawnień (etap 6) |
| 5.8–5.12 | Wbudowane narzędzia | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**Dostarczane**: Istoty Krzemowe mogą wywoływać narzędzia do wykonywania operacji.

**Weryfikacja**: zapytaj „jaki jest dziś dzień tygodnia" → CalendarTool odpowiada; zapytaj „sprawdź procesy" → SystemTool wykonuje; każ istocie wysłać wiadomość innej istocie → ChatTool działa.

---

## ~~Etap 6: Przestrzega reguł (system uprawnień)~~ ✅ Ukończono

**Cel**: Istoty Krzemowe nie mogą uzyskać dostępu do wrażliwych zasobów bez autoryzacji.

| # | Moduł | Opis |
|---|--------|-------------|
| 6.1 | PermissionManager | Prywatna instancja dla każdej istoty. Oparta na wywołaniach zwrotnych, wynik trójstanowy (Allowed/Deny/AskUser). Priorytet zapytań: HighDeny → HighAllow → Callback. Flaga IsCurator |
| 6.2 | Enum PermissionType | NetworkAccess, CommandLine, FileAccess, Function, DataAccess |
| 6.3 | DefaultPermissionCallback | Biała/czarna lista sieci, klasyfikacja CLI, reguły bezpieczeństwa ścieżek plików |
| 6.4 | GlobalACL | Tabela reguł z dopasowaniem prefiksu, utrwalana w pamięci masowej |
| 6.5 | UserFrequencyCache | Listy HighAllow/HighDeny. Wybór użytkownika (nie automatyczne wykrywanie). Dopasowanie prefiksu, tylko w pamięci, konfigurowalna ważność |
| 6.6 | Mechanizm UserAsk (konsola) | Monit konsoli y/n gdy zwrócono AskUser |
| 6.7 | Integracja uprawnień wykonawców | Wszyscy wykonawcy sprawdzają uprawnienia przed wykonaniem |
| 6.8 | Wyjaśnienie izolacji IStorage | IStorage jest wewnętrznym utrwalaniem systemu — bezpośredni dostęp do plików, **nie** kierowany przez wykonawców, **nie** kontrolowany przez AI. Wykonawcy zarządzają jedynie IO inicjowanym przez narzędzia AI |
| 6.9 | Dziennik audytu | Rejestrowanie wszystkich decyzji uprawnień ze znacznikiem czasowym, żądającym, zasobem, wynikiem |

**Dostarczane**: monity uprawnień pojawiają się, gdy istota próbuje wrażliwej operacji.

**Weryfikacja**: każ istocie usunąć plik → konsola wyświetla monit uprawnień → wpisz `n` → operacja odrzucona. Każ istocie odwiedzić stronę z białej listy → natychmiast dozwolone.

---

## ~~Etap 7: Może ewoluować (kompilacja dynamiczna)~~ ✅ Ukończono

**Cel**: Istoty Krzemowe mogą przepisywać swój własny kod.

| # | Moduł | Opis |
|---|--------|-------------|
| 7.1 | CodeEncryption | Szyfrowanie/deszyfrowanie AES-256. Derywacja klucza PBKDF2 z GUID |
| 7.2 | DynamicCompilationExecutor | Piaskownica kompilacji w pamięci oparta na Roslyn. Kontrola referencji zestawów w czasie kompilacji (główna obrona: wykluczenie System.IO, Reflection itp.) |
| 7.3 | Skanowanie bezpieczeństwa | Statyczna analiza w czasie wykonywania niebezpiecznych wzorców kodu (obrona dodatkowa). Zablokowanie ładowania w przypadku niepowodzenia skanowania |
| 7.4 | Ulepszenie cyklu życia istoty | Ładowanie: deszyfrowanie → skanowanie → kompilacja → instancjonowanie. Czas wykonywania: kompilacja w pamięci → zastąpienie atomowe → utrwalenie zaszyfrowane |
| 7.5 | SiliconCurator | Abstrakcyjna klasa bazowa kuratora. IsCurator=true. Najwyższe uprawnienia |
| 7.6 | DefaultCurator | Domyślna implementacja kuratora z wbudowanym Plikiem Duszy i narzędziami zarządzania |
| 7.7 | CuratorTool | Narzędzie `[SiliconManagerOnly]`: list_beings, create_being, get_code, reset |
| 7.8 | Nadpisanie wywołania zwrotnego uprawnień | Istoty mogą kompilować niestandardowe wywołanie zwrotne uprawnień |
| 7.9 | Ulepszenie SiliconBeingManager | Metoda Replace (wymiana instancji w czasie wykonywania). MigrateState (transfer stanu między starą a nową instancją) |

**Dostarczane**: Istoty Krzemowe mogą generować nowy kod przez AI, kompilować i zastępować same siebie.

**Weryfikacja**: każ istocie „dodaj sobie nową funkcję" → obserwuj kompilację → ponowne uruchomienie → nowa funkcja działa.

---

## ~~Etap 8: Pamięć i planowanie~~ ✅ Ukończono

**Cel**: pamięć długoterminowa, zarządzanie zadaniami, wyzwalanie czasowe.

| # | Moduł | Opis |
|---|--------|-------------|
| 8.1 | FileSystemMemory | Przechowywanie segmentowane krótko/długoterminowe. Rozpad czasowy. Kompresja (łączenie podobnych wspomnień). Wyszukiwanie wielowymiarowe |
| 8.2 | TaskSystem | Zadania jednorazowe + zależności DAG. Harmonogram z priorytetami. Śledzenie stanu |
| 8.3 | TimerSystem | Jednorazowe alarmy + czasomierze cykliczne. Precyzja milisekundowa. Utrwalanie w pamięci masowej |
| 8.4 | IncompleteDate | Struktura rozmytego zakresu dat (np. „kwiecień 2026", „wiosna 2026") |
| 8.5–8.7 | Narzędzia pamięci/zadań/czasomierza | Narzędzia dla istot do odpytywania wspomnień, zarządzania zadaniami, ustawiania czasomierzy |

**Dostarczane**: istoty mogą pamiętać kluczowe punkty, tworzyć/śledzić zadania, ustawiać alarmy.

**Weryfikacja**: utwórz zadanie → sprawdź listę zadań → ustaw alarm na 1 minutę → otrzymaj powiadomienie po upływie czasu.

---

## ~~Etap 9: Szkielet ukończony~~ ✅ Ukończono

**Cel**: ujednolicony punkt wejścia, współpraca wielu istot.

| # | Moduł | Opis |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | Ujednolicony host ze wzorcem budowniczego. Eleganckie zamykanie (Ctrl+C / SIGTERM) |
| 9.2 | Refaktoryzacja Program.Main | Migracja do wzorca CoreHostBuilder |
| 9.3 | Ulepszenie SiliconBeingManager | Priorytetowa odpowiedź kuratora. Izolacja wyjątków. Okresowe utrwalanie |
| 9.4 | Ładowanie wielu istot | Ładowanie wielu istot z katalogu danych. Komunikacja między istotami przez ChatTool |
| 9.5 | Monitorowanie wydajności | Śledzenie czasu wykonania każdego obiektu Tick |
| 9.6 | ServiceLocator | Globalny lokalizator usług z metodami Register/Get |

**Dostarczane**: wiele istot działających jednocześnie, współpracujących, zarządzanych przez CoreHost.

**Weryfikacja**: utwórz dwie istoty → A wysyła wiadomość do B → B odbiera i odpowiada → szkielet harmonizuje bez błędów. Kurator odpowiada priorytetowo na wiadomości użytkownika.

---

## ~~Etap 10: Przejście do Web~~ ✅ Ukończono

**Cel**: migracja z konsoli do interfejsu przeglądarkowego.

| # | Moduł | Opis |
|---|--------|-------------|
| 10.1 | Router | Router żądań HTTP. Trasowanie parametryczne i serwowanie plików statycznych |
| 10.2 | Klasa bazowa Controller | Kontekst żądania/odpowiedzi. Obsługa odpowiedzi HTML i JSON |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | Konstruktory server-side C#. Zero zależności od frameworków frontendowych |
| 10.6 | SSE (Server-Sent Events) | Aktualizacje w czasie rzeczywistym typu push dla czatu, stanu istot i zdarzeń systemowych. Prostsze niż WebSocket, z automatycznym ponownym połączeniem klienta |
| 10.7 | WebUIProvider | Kanał komunikatora w czasie rzeczywistym oparty na SSE. Zastępuje konsolę jako główny interfejs |
| 10.8 | Bezpieczeństwo Web | Czarna/biała lista IP. Atrybut `[WebCode]`. Dynamiczne aktualizacje |
| 10.9–10.17 | Kontrolery Web | Czat, pulpit, istoty, zadania, uprawnienia, żądania uprawnień, wykonawcy, dzienniki, konfiguracja, pamięć, czasomierze, inicjalizacja, informacje, przeglądarka kodu, wiedza, projekty, audyt |

**Dostarczane**: pełny Web UI dostępny z przeglądarki.

**Weryfikacja**: otwórz przeglądarkę → rozmawiaj z istotą → zobacz pulpit → zarządzaj uprawnieniami → wszystkie funkcje działają.

---

## ~~Etap 10.5: Ulepszenia przyrostowe~~ ✅ Ukończono

**Cel**: ulepszenie istniejącego systemu o nowe funkcje odkryte podczas rozwoju.

| # | Moduł | Opis |
|---|--------|-------------|
| 10.5.1 | BroadcastChannel | Nowy typ sesji dla ogłoszeń ogólnosystemowych. Stały identyfikator kanału, dynamiczne subskrypcje, filtrowanie oczekujących wiadomości |
| 10.5.2 | Ulepszenie ChatMessage | Pola ToolCallId, ToolCallsJson, Thinking dla kontekstu AI; PromptTokens, CompletionTokens, TotalTokens do śledzenia tokenów; typ wiadomości SystemNotification |
| 10.5.3 | TokenUsageAuditManager | Śledzenie zużycia tokenów na żądanie dla wszystkich istot. Zagregowane statystyki, zapytania szeregów czasowych, trwałe przechowywanie |
| 10.5.4 | TokenAuditTool | Narzędzie `[SiliconManagerOnly]` dla kuratora do odpytywania i podsumowywania wykorzystania tokenów |
| 10.5.5 | ConfigTool | Narzędzie `[SiliconManagerOnly]` dla kuratora do odczytu i modyfikacji konfiguracji systemu |
| 10.5.6 | AuditController | Pulpit webowy do audytu wykorzystania tokenów z wykresami trendów i eksportem danych |
| 10.5.7 | Rozszerzenie systemu kalendarza | 32 implementacje kalendarzy obejmujące światowe systemy kalendarzowe (buddyjski, chiński księżycowy, islamski, hebrajski, japoński, perski, majański itp.) |
| 10.5.8 | Ulepszenie DiskTool | Nowe operacje: count_lines, read_lines, clear_file, replace_lines, replace_text, replace_text_all, list_drives |
| 10.5.9 | Ulepszenie SystemTool | Nowe operacje: find_process (obsługa symboli wieloznacznych), resource_usage |
| 10.5.10 | Ulepszenie CalendarTool | Nowe operacje: diff, list_calendars, get_components, get_now_components, convert (konwersja między kalendarzami) |
| 10.5.11 | DashScopeClient | Klient AI Alibaba Cloud DashScope, kompatybilny z API OpenAI. Obsługa strumieniowania, wywołań narzędzi, treści wnioskowania |
| 10.5.12 | DashScopeClientFactory | Fabryka do tworzenia klientów DashScope. Dynamiczne odkrywanie modeli przez API. Obsługa wielu regionów (Pekin, Wirginia, Singapur, Hongkong, Frankfurt) |
| 10.5.13 | System konfiguracji klienta AI | Konfiguracja klienta AI dla każdej istoty. Opcje dynamicznych kluczy konfiguracyjnych (model, region). Zlokalizowane nazwy wyświetlane |
| 10.5.14 | Rozszerzenie lokalizacji | Lokalizacja chińska uproszczona, chińska tradycyjna, angielska i japońska dla opcji konfiguracji DashScope, nazw modeli i nazw regionów |

**Dostarczane**: ulepszone narzędzia, obserwowalność, pokrycie kalendarzowe i obsługa wielu backendów AI.

**Weryfikacja**: kurator odpytuje wykorzystanie tokenów przez TokenAuditTool → pulpit audytu wyświetla trendy → CalendarTool konwertuje daty między 32 systemami kalendarzowymi → przełącz backend AI na DashScope → rozmawiaj z modelem Qwen przez API chmurowe.

---

## ~~Etap 10.6: Doskonalenie i optymalizacja~~ ✅ Ukończono

**Cel**: doskonalenie funkcji systemu, dodanie nowych cech, optymalizacja doświadczenia użytkownika.

| # | Moduł | Opis |
|---|--------|-------------|
| 10.6.1 | WebViewBrowserTool | Narzędzie automatyzacji przeglądarki wieloplatformowej oparte na Playwright, obsługa trybu headless, izolacja indywidualna, pełne wsparcie JS/CSS |
| 10.6.2 | HelpTool | Narzędzie systemu dokumentacji pomocy, obsługa wielojęzycznego wyszukiwania i wyświetlania dokumentacji |
| 10.6.3 | ProjectWorkNoteTool | Narzędzie notatek pracy projektu, obsługa rejestrowania i zarządzania pracą w wymiarze projektowym |
| 10.6.4 | ProjectTaskTool | Narzędzie zarządzania zadaniami projektu, obsługa przypisywania zadań i śledzenia postępu |
| 10.6.5 | KnowledgeTool | Narzędzie Sieci Wiedzy, obsługa operacji CRUD na trójkach wiedzy i odkrywania ścieżek |
| 10.6.6 | ChatHistoryController | Kontroler podglądu historii czatu, obsługa listy sesji i szczegółów wiadomości |
| 10.6.7 | CodeHoverController | Kontroler podpowiedzi kodu, obsługa podświetlania składni i podpowiedzi kodu |
| 10.6.8 | WorkNoteController | Kontroler zarządzania notatkami pracy, obsługa wyszukiwania i generowania spisu treści |
| 10.6.9 | TimerExecutionHistory | Funkcja historii wykonania czasomierzy, rejestrowanie i przeglądanie historii wyzwalaczy |
| 10.6.10 | Rozszerzenie lokalizacji | Dodanie obsługi lokalizacji czeskiej (cs-CZ), łącznie 21 wariantów językowych |
| 10.6.11 | Optymalizacja Web UI | Obsługa przesyłania plików, wskaźniki ładowania, optymalizacja renderowania wywołań narzędzi, poprawka modalna notatek pracy |
| 10.6.12 | Ulepszenie zarządzania pamięcią | Zaawansowane filtrowanie, statystyki, widok szczegółów, optymalizacja algorytmu kompresji |
| 10.6.13 | Refaktoryzacja systemu dzienników | Separacja dzienników system/istoty krzemowej, API odczytu dzienników, filtr istot krzemowych |
| 10.6.14 | Ulepszenie systemu uprawnień | Weryfikacja prekompilacji wywołań zwrotnych uprawnień, weryfikacja referencji zestawów, biała lista usługi pogodowej wttr.in |

**Dostarczane**: pełna automatyzacja przeglądarki WebView, system dokumentacji pomocy, obszar roboczy projektu, Sieć Wiedzy, podgląd historii czatu i inne ulepszenia.

**Weryfikacja**: Istota Krzemowa może operować przeglądarką przez WebViewBrowserTool → uzyskać dokumentację pomocy przez HelpTool → zarządzać notatkami pracy i zadaniami projektu → odpytywać Sieć Wiedzy → przeglądać historię czatu.

---

## ~~Etap 10.7: Współpraca projektowa i przepływy pracy~~ ✅ Ukończono

**Cel**: dodanie obszaru roboczego projektu, silnika przepływów pracy, mechanizmu zanikania pamięci i systemu uprawnień narzędzi.

| # | Moduł | Opis |
|---|--------|-------------|
| 10.7.1 | Zarządzanie rolami projektu | ProjectTool dodaje operacje assign_role, remove_role, list_roles |
| 10.7.2 | Silnik przepływów pracy | WorkflowEngine — główny silnik obsługujący definicje szablonów, przejścia stanów, wykonywanie napędzane Tick |
| 10.7.3 | Szablony przepływów pracy | WorkflowTemplate — klasa bazowa definiująca zbiory stanów i reguły przejść |
| 10.7.4 | Instancje przepływów pracy | WorkflowInstance — zarządzanie instancjami, powiązanie z konkretnym projektem, śledzenie bieżącego stanu |
| 10.7.5 | Dzienniki przepływów pracy | WorkflowLog — rejestrowanie historii przejść stanów |
| 10.7.6 | Mechanizm zanikania pamięci | MemoryFadeService — usługa okresowego rozpadu, automatyczne zmniejszanie ważności wspomnień i archiwizacja co godzinę |
| 10.7.7 | System uprawnień narzędzi | Dwupoziomowe uprawnienia narzędzi (poziom Istoty Krzemowej + poziom projektu), szablony uprawnień, kontrola granularności operacji |
| 10.7.8 | ToolPermissionController | Kontroler webowy zarządzania uprawnieniami narzędzi |
| 10.7.9 | ProjectWorkTool | Narzędzie operacji pracy projektu (`[SiliconManagerOnly]`, `[ToolScenario(Project)]`) |
| 10.7.10 | System scenariuszy narzędzi | ToolScenarioAttribute i ChatOnlyAttribute, obsługa filtrowania scenariuszy Chat/Task/Timer/MemoryCompression/Project |
| 10.7.11 | Rozszerzenie lokalizacji | Dodanie lokalizacji rosyjskiej, portugalskiej, włoskiej, holenderskiej, polskiej, szwedzkiej, łącznie 34 wariantów językowych |

**Dostarczane**: kompletny system współpracy projektowej, silnik przepływów pracy, mechanizm zanikania pamięci i zarządzanie uprawnieniami narzędzi.

**Weryfikacja**: utwórz projekt → przypisz role → powiąż szablon przepływu pracy → istoty współpracują w przestrzeni projektu → pamięć automatycznie zanika i jest archiwizowana → izolacja uprawnień narzędzi działa.

---

## Etap 11: Integracja z zewnętrznymi komunikatorami

**Cel**: połączenie z zewnętrznymi platformami komunikacyjnymi dla szerszej dostępności dla użytkowników.

| # | Moduł | Opis |
|---|--------|-------------|
| 11.1 | FeishuProvider | Integracja bota Feishu (Lark), obsługa kart |
| 11.2 | WhatsAppProvider | Integracja WhatsApp Business API |
| 11.3 | TelegramProvider | Integracja Telegram Bot API, obsługa klawiatury wbudowanej |
| 11.4 | Ulepszenie IMManager | Trasowanie wielodostawców, ujednolicony format wiadomości, obsługa zapytań o uprawnienia wieloplatformowych |

**Dostarczane**: użytkownicy mogą wchodzić w interakcje z Istotami Krzemowymi przez zewnętrzne platformy komunikacyjne.

---

## Etap 11.5: System umiejętności i integracja MCP

**Cel**: Warstwa abstrakcji wielokrotnie użytkowanych zdolności i integracja z ekosystemem narzędzi zewnętrznych.

| # | Moduł | Opis |
|---|--------|-------------|
| 11.5.1 | ~~System umiejętności~~ ✅ Ukończono | Warstwa abstrakcji wielokrotnie użytkowanej orchestracji narzędzi + szablonu podpowiedzi (SkillManager, podwójny tryb wyzwalania, gorące przeładowanie, archiwizacja wersji, automatyczne uzupełnianie metadanych przez AI) |
| 11.5.2 | ~~Integracja MCP~~ ✅ Ukończono | Integracja narzędzi zewnętrznych serwerów MCP (podwójny transport stdio/http, wstrzykiwanie nazewnictwa `mcp_{serverId}_{toolName}`, strona zarządzania Web, integracja z macierzą uprawnień) |

**Dostarczane**: Strona zarządzania umiejętnościami (/skill), strona zarządzania MCP (/mcp), wbudowane narzędzia `skill` i `mcp`, dokumentacja pomocy umiejętności/MCP.

---

## Etap 12: Funkcje zaawansowane

**Cel**: opcjonalne funkcje zaawansowane dla rozszerzonych możliwości.

| # | Moduł | Opis |
|---|--------|-------------|
| 12.1 | ~~Sieć Wiedzy~~ ✅ Ukończono | Graf wiedzy o strukturze trójkowej (podmiot-orzeczenie-dopełnienie), obsługa operacji CRUD, odkrywania ścieżek, zaawansowanych zapytań i przechodzenia grafu |
| 12.2 | ~~System wtyczek~~ ✅ Ukończono | Ładowanie zewnętrznych wtyczek z kontrolą bezpieczeństwa i piaskownicą (interfejs IPlugin, PluginLoader, izolacja AssemblyLoadContext) |
| 12.3 | Ekosystem umiejętności | Rynek wielokrotnego użytku umiejętności dla możliwości istot |
