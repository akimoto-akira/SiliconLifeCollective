# Plan działania

> **Wersja: v0.2.0-alpha**

[English](../en/roadmap.md) | [Deutsch](../de-DE/roadmap.md) | [中文](../zh-CN/roadmap.md) | [繁體中文](../zh-HK/roadmap.md) | [Español](../es-ES/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Čeština](../cs-CZ/roadmap.md) | [Русский](../ru-RU/roadmap.md) | [Polski](../pl-PL/roadmap.md)

## Plan działania dwóch wersji

### SiliconLife.Default (wersja domyślna)
- **Przeznaczenie**: domyślna implementacja, głównie do weryfikacji wykonalności architektury
- **Obecny status**: etapy 1-10.6 zakończone, system działa stabilnie
- **Opis roli**: jako referencyjna implementacja weryfikacji architektury, zapewniająca poprawność i wykonalność rdzennego projektu architektury

### SiliconLife.Fast (wersja wysokowydajna)
- **Przeznaczenie**: główna wersja produkcyjna
- **Obecny status**: zakończona migracja podstawowej architektury, silnik przechowywania SpeedyPack i system wtyczek zaimplementowane
- **Opis roli**: na podstawie architektury zweryfikowanej przez wersję Default, głęboka optymalizacja wydajności i rozszerzenie funkcji produkcyjnych, preferowana do rzeczywistego wdrożenia

**Plan rozwoju wersji Fast**:
- ✅ Etap 1: Migracja podstawowej struktury projektu i systemu konfiguracji
- ✅ Etap 2: Migracja Web UI i kontrolerów
- ✅ Etap 3: Optymalizacja systemu przechowywania (pamięć SpeedyPack + asynchroniczna trwałość)
- ✅ Etap 3.5: Narzędzie zarządzania SpeedyPack (aplikacja Avalonia UI SiliconLife.Speedy.Manager)
- ✅ Etap 3.6: System wtyczek (interfejs IPlugin, bezpieczna piaskownica, izolacja AssemblyLoadContext)
- 🚧 Etap 4: Rozwój zasobnika systemowego i aplikacji okienkowej
- 📋 Etap 5: Optymalizacja wydajności (pule połączeń, pule obiektów, współbieżność bez blokad)
- 📋 Etap 6: Zastąpienie serwera Web Kestrel
- 📋 Etap 7: Serializacja binarna MessagePack

---

## Zasady przewodnie

Każdy etap kończy się **działającym, obserwowalnym** systemem. Żaden etap nie tworzy "stosu infrastruktury bez niczego do pokazania".

---

## ~~Etap 1: Można rozmawiać~~ ✅ Zakończony

**Cel**: Wejście konsoli → wywołanie AI → wyjście konsoli. Minimalna jednostka weryfikowalna.

| # | Moduł | Opis |
|---|--------|-------------|
| 1.1 | Rozwiązanie i struktura projektu | Utworzenie `SiliconLifeCollective.sln`, zawierającego `src/SiliconLife.Core/` (biblioteka rdzenna) i `src/SiliconLife.Default/` (domyślna implementacja + punkt wejścia) |
| 1.2 | Konfiguracja (minimalna) | Singleton + deserializacja JSON. Odczyt `config.json`. Automatyczne generowanie wartości domyślnych w przypadku braku |
| 1.3 | Lokalizacja (minimalna) | Klasa abstrakcyjna `LocalizationBase`, implementacja `ZhCN`. Dodanie `Language` do konfiguracji |
| 1.4 | OllamaClient (minimalny) | Interfejs `IAIClient`, wywołanie HTTP lokalnego Ollama `/api/chat`. Brak strumieniowania, brak wywołań narzędzi |
| 1.5 | Wejście/Wyjście konsoli | `while(true) + Console.ReadLine()`, odczyt wejścia → wywołanie AI → wydruk odpowiedzi |
| 1.6 | Nagłówek praw autorskich | Dodanie nagłówka Apache 2.0 do wszystkich plików źródłowych C# |

**Dostarczane**: Program czatu konsolowego rozmawiający z lokalnym modelem Ollama.

**Weryfikacja**: Uruchom program, wpisz "hello", zobacz odpowiedź AI.

---

## ~~Etap 2: Posiada szkielet~~ ✅ Zakończony

**Cel**: Zastąpienie "gołej pętli" strukturą szkieletową. Zachowanie bez zmian.

| # | Moduł | Opis |
|---|--------|-------------|
| 2.1 | Przechowywanie (minimalne) | Interfejs `IStorage` (Read/Write/Exists/Delete, pary klucz-wartość). Implementacja `FileSystemStorage`. Klasa instancyjna (niestatyczna). Bezpośredni dostęp do systemu plików — **AI nie kontroluje IStorage** |
| 2.2 | Pętla główna + obiekty zegara | Nieskończona pętla, dokładne interwały zegara (`Stopwatch` + `Thread.Sleep`). Harmonogramowanie priorytetowe |
| 2.3 | Standaryzacja IAIClient | Interfejs `IAIClientFactory`. Refaktoring OllamaClient do implementacji standardowego interfejsu |
| 2.4 | Migracja konsoli | Migracja `while(true)` do obiektu zegara napędzanego pętlą główną. Zachowanie identyczne z etapem 1 |

**Dostarczane**: Pętla główna uruchamia zegary, czat konsolowy nadal działa.

**Weryfikacja**: Zarejestruj testowy obiekt zegara, drukujący liczbę tyknięć co sekundę; czat konsolowy nadal działa.

---

## ~~Etap 3: Posiada duszę~~ ✅ Zakończony

**Cel**: Pierwsza Istota Krzemowa żyje w szkieletzie.

| # | Moduł | Opis |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Klasa abstrakcyjna bazowa, zawierająca Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService. Abstrakcyjne `Tick()` i `ExecuteOneRound()` |
| 3.2 | Ładowanie pliku duszy | `SoulFileManager`: odczyt `soul.md` z katalogu danych istoty |
| 3.3 | ContextManager (minimalny) | Połączenie pliku duszy + ostatnie wiadomości → wywołanie AI → uzyskanie odpowiedzi. Brak wywołań narzędzi, brak utrwalania |
| 3.4 | ISiliconBeingFactory | Interfejs fabryki do tworzenia instancji istot |
| 3.5 | SiliconBeingManager (minimalny) | Dziedziczy obiekt zegara (priorytet=0). Iteruje wszystkie istoty, wywołując ich Tick po kolei |
| 3.6 | DefaultSiliconBeing | Standardowa implementacja zachowania. Sprawdza nieprzeczytane wiadomości → tworzy ContextManager → ExecuteOneRound → wyjście |
| 3.7 | Struktura katalogu istoty | `DataDirectory/SiliconManager/{GUID}/`, zawierający `soul.md` i `state.json` |

**Dostarczane**: Istota Krzemowa napędzana pętlą główną, przyjmująca wejście konsoli, ładująca plik duszy, wywołująca AI.

**Weryfikacja**: Wejście konsoli → wyzwolenie zegara pętli głównej → przetwarzanie istoty (z zachowaniem kierowanym przez plik duszy) → odpowiedź AI. Styl odpowiedzi powinien różnić się od etapu 1.

---

## ~~Etap 4: Posiada pamięć~~ ✅ Zakończony

**Cel**: Rozmowy utrzymują się po restarcie.

| # | Moduł | Opis |
|---|--------|-------------|
| 4.1 | ChatSystem | Koncepcja kanału (dwa GUID = jeden kanał). Model wiadomości z utrwalaniem. Brak czatu grupowego |
| 4.2 | IIMProvider + IMManager | Interfejs `IIMProvider`. `ConsoleProvider` jako formalny kanał komunikacji natychmiastowej. `IMManager` kieruje wiadomości |
| 4.3 | Rozszerzenie ContextManager | Pobieranie historii z systemu czatu. Utrwalanie odpowiedzi AI. Obsługa wieloobrotowej kontynuacji wywołań narzędzi |
| 4.4 | Model IMessage | Ujednolicony model wiadomości współdzielony przez system czatu i menedżera komunikacji natychmiastowej |

**Dostarczane**: System czatu z utrwaloną pamięcią.

**Weryfikacja**: Czatuj kilka rund → wyjdź → zrestartuj → zapytaj "O czym rozmawialiśmy?" → istota potrafi odpowiedzieć.

---

## ~~Etap 5: Może działać (system narzędzi)~~ ✅ Zakończony

**Cel**: Istoty Krzemowe mogą wykonywać operacje, a nie tylko rozmawiać.

| # | Moduł | Opis |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | Interfejs `ITool`, zawierający Name, Description, Execute. `ToolResult` zawierający Success, Message, Data |
| 5.2 | ToolManager | Instancja dla każdej istoty. Odkrywanie narzędzi oparte na refleksji. Obsługa atrybutu `[SiliconManagerOnly]` |
| 5.3 | IAIClient: obsługa wywołań narzędzi | Analiza AI tool_calls. Pętla: wykonanie narzędzia → odesłanie wyniku → AI kontynuuje → aż do czystego tekstu |
| 5.4 | Klasa bazowa wykonawcy | Klasa abstrakcyjna bazowa, z niezależnym wątkiem dyspozytorskim, kolejką żądań, kontrolą limitu czasu |
| 5.5 | NetworkExecutor | Żądania HTTP przez wykonawcę. Limit czasu, kolejkowanie |
| 5.6 | CommandLineExecutor | Wykonanie Shell przez wykonawcę. Wykrywanie separatorów wieloplatformowych |
| 5.7 | DiskExecutor | Operacje na plikach przez wykonawcę. Brak sprawdzania uprawnień (etap 6) |
| 5.8–5.12 | Wbudowane narzędzia | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**Dostarczane**: Istoty Krzemowe mogą wywoływać narzędzia do wykonywania operacji.

**Weryfikacja**: Zapytaj "Jaki dziś dzień tygodnia" → CalendarTool odpowiada; zapytaj "Sprawdź procesy" → SystemTool wykonuje; powiedz istocie, aby wysłała wiadomość do innej istoty → ChatTool działa.

---

## ~~Etap 6: Przestrzega reguł (system uprawnień)~~ ✅ Zakończony

**Cel**: Istoty Krzemowe nie mogą uzyskać dostępu do wrażliwych zasobów bez autoryzacji.

| # | Moduł | Opis |
|---|--------|-------------|
| 6.1 | PermissionManager | Prywatna instancja dla każdej istoty. Oparta na wywołaniach zwrotnych, wynik trójstanowy (Allowed/Deny/AskUser). Priorytet zapytania: HighDeny → HighAllow → Callback. Flaga IsCurator |
| 6.2 | Enum PermissionType | NetworkAccess, CommandLine, FileAccess, Function, DataAccess |
| 6.3 | DefaultPermissionCallback | Biała/czarna lista sieciowa, klasyfikacja CLI, reguły bezpieczeństwa ścieżek plików |
| 6.4 | GlobalACL | Tabela reguł z dopasowaniem prefiksu, utrwalana w przechowywaniu |
| 6.5 | UserFrequencyCache | Lista HighAllow/HighDeny. Wybór użytkownika (nie automatyczne wykrywanie). Dopasowanie prefiksu, tylko w pamięci, konfigurowalna ważność |
| 6.6 | Mechanizm UserAsk (konsola) | Monit konsoli y/n gdy zwrócony AskUser |
| 6.7 | Integracja uprawnień wykonawcy | Wszyscy wykonawcy sprawdzają uprawnienia przed wykonaniem |
| 6.8 | Wyjaśnienie izolacji IStorage | IStorage to wewnętrzne utrwalanie systemu — bezpośredni dostęp do plików, **nie** kierowany przez wykonawcę, **nie** kontrolowany przez AI. Wykonawcy zarządzają tylko I/O inicjowanym przez narzędzia AI |
| 6.9 | Dziennik audytu | Rejestrowanie wszystkich decyzji uprawnień, z sygnaturą czasową, żądającym, zasobem, wynikiem |

**Dostarczane**: Monity uprawnień pojawiają się, gdy istota próbuje wrażliwej operacji.

**Weryfikacja**: Powiedz istocie usunąć plik → konsola wyświetla monit uprawnień → wpisz `n` → operacja odrzucona. Powiedz istocie odwiedzić whitelisted'ową stronę → natychmiast zezwolone.

---

## ~~Etap 7: Może ewoluować (kompilacja dynamiczna)~~ ✅ Zakończony

**Cel**: Istoty Krzemowe mogą przepisywać swój własny kod.

| # | Moduł | Opis |
|---|--------|-------------|
| 7.1 | CodeEncryption | Szyfrowanie/deszyfrowanie AES-256. Klucz PBKDF2 wyprowadzany z GUID |
| 7.2 | DynamicCompilationExecutor | Piaskownica kompilacji w pamięci oparta na Roslyn. Kontrola referencji montowanych w czasie kompilacji (główna obrona: wykluczenie System.IO, Reflection itp.) |
| 7.3 | Skanowanie bezpieczeństwa | Analiza statyczna w czasie działania niebezpiecznych wzorców kodu (obrona wtórna). Zablokowanie ładowania w przypadku niepowodzenia skanowania |
| 7.4 | Rozszerzenie cyklu życia istoty | Ładowanie: deszyfrowanie → skanowanie → kompilacja → instancjonowanie. Czas działania: kompilacja w pamięci → atomowa zamiana → utrwalanie zaszyfrowane |
| 7.5 | SiliconCurator | Abstrakcyjna klasa bazowa Kuratora. IsCurator=true. Najwyższe uprawnienia |
| 7.6 | DefaultCurator | Domyślna implementacja Kuratora, z wbudowanym plikiem duszy i narzędziami zarządzania |
| 7.7 | CuratorTool | Narzędzie `[SiliconManagerOnly]`: list_beings, create_being, get_code, reset |
| 7.8 | Nadpisanie wywołania zwrotnego uprawnień | Istoty mogą kompilować niestandardowe wywołania zwrotne uprawnień |
| 7.9 | Rozszerzenie SiliconBeingManager | Metoda Replace (wymiana instancji w czasie działania). MigrateState (transfer stanu między starą a nową instancją) |

**Dostarczane**: Istoty Krzemowe mogą generować nowy kod przez AI, kompilować i zastępować same siebie.

**Weryfikacja**: Powiedz istocie "dodaj sobie nową funkcję" → obserwuj kompilację → restart → nowa funkcja działa.

---

## ~~Etap 8: Pamięć i planowanie~~ ✅ Zakończony

**Cel**: Długoterminowa pamięć, zarządzanie zadaniami, wyzwalanie czasowe.

| # | Moduł | Opis |
|---|--------|-------------|
| 8.1 | FileSystemMemory | Przechowywanie segmentowe krótkoterminowe/długoterminowe. Rozpad czasowy. Kompresja (łączenie podobnych wspomnień). Wyszukiwanie wielowymiarowe |
| 8.2 | TaskSystem | Zadania jednorazowe + zależności DAG. Harmonogramowanie priorytetowe. Śledzenie stanu |
| 8.3 | TimerSystem | Jednorazowe alarmy + czasomierze okresowe. Dokładność milisekundowa. Utrwalanie w przechowywaniu |
| 8.4 | IncompleteDate | Struktura rozmytego zakresu dat (np. "kwiecień 2026", "wiosna 2026") |
| 8.5–8.7 | Narzędzia pamięci/zadań/czasomierza | Narzędzia dla istot do odpytywania pamięci, zarządzania zadaniami, ustawiania alarmów |

**Dostarczane**: Istoty mogą pamiętać kluczowe punkty, tworzyć/śledzić zadania, ustawiać alarmy.

**Weryfikacja**: Utwórz zadanie → sprawdź listę zadań → ustaw alarm na 1 minutę → otrzymaj powiadomienie po upływie czasu.

---

## ~~Etap 9: Szkielet ukończony~~ ✅ Zakończony

**Cel**: Ujednolicony punkt wejścia, współpraca wieloistotowa.

| # | Moduł | Opis |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | Ujednolicony host z wzorcem budowniczego. Eleganckie zamykanie (Ctrl+C / SIGTERM) |
| 9.2 | Refaktoring Program.Main | Migracja do wzorca CoreHostBuilder |
| 9.3 | Rozszerzenie SiliconBeingManager | Priorytetowa odpowiedź Kuratora. Izolacja wyjątków. Regularne utrwalanie |
| 9.4 | Ładowanie wielu istot | Ładowanie wielu istot z katalogu danych. Komunikacja między istotami przez ChatTool |
| 9.5 | Monitorowanie wydajności | Śledzenie czasu wykonania każdego obiektu zegara |
| 9.6 | ServiceLocator | Globalny lokalizator usług, z metodami Register/Get |

**Dostarczane**: Wiele istot działających jednocześnie, współpracujących, zarządzanych przez CoreHost.

**Weryfikacja**: Utwórz dwie istoty → A wysyła wiadomość do B → B odbiera i odpowiada → szkielet harmonogramuje bez błędów. Kurator odpowiada priorytetowo, gdy przychodzi wiadomość użytkownika.

---

## ~~Etap 10: Przejście do Web~~ ✅ Zakończony

**Cel**: Migracja z konsoli do interfejsu przeglądarkowego.

| # | Moduł | Opis |
|---|--------|-------------|
| 10.1 | Router | Router żądań HTTP. Trasy z parametrami seryjnymi i serwowanie plików statycznych |
| 10.2 | Klasa bazowa Controller | Kontekst żądania/odpowiedzi. Obsługa odpowiedzi HTML i JSON |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | Konstruktory server-side C#. Zerowe zależności od frameworków frontendowych |
| 10.6 | SSE (zdarzenia wysyłane przez serwer) | Aktualizacje w czasie rzeczywistym typu push dla czatu, stanu istot i zdarzeń systemowych. Prostsze niż WebSocket, z automatycznym ponownym połączeniem klienta |
| 10.7 | WebUIProvider | Kanał komunikacji natychmiastowej w czasie rzeczywistym oparty na SSE. Zastępuje konsolę jako główny interfejs |
| 10.8 | Bezpieczeństwo Web | Czarna/biała lista IP. Atrybut `[WebCode]`. Dynamiczne aktualizacje |
| 10.9–10.17 | Kontrolery Web | Czat, pulpit nawigacyjny, istoty, zadania, uprawnienia, żądania uprawnień, wykonawcy, logi, konfiguracja, pamięć, czasomierze, inicjalizacja, o systemie, przeglądarka kodu, wiedza, projekty, audyt |

**Dostarczane**: Kompletny Web UI dostępny z przeglądarki.

**Weryfikacja**: Otwórz przeglądarkę → czatuj z istotą → zobacz pulpit nawigacyjny → zarządzaj uprawnieniami → wszystkie funkcje działają.

---

## ~~Etap 10.5: Rozszerzenia przyrostowe~~ ✅ Zakończony

**Cel**: Rozszerzenie istniejącego systemu o nowe funkcje odkryte podczas rozwoju.

| # | Moduł | Opis |
|---|--------|-------------|
| 10.5.1 | BroadcastChannel | Nowy typ sesji dla ogłoszeń ogólnosystemowych. Stałe ID kanału, dynamiczne subskrypcje, filtrowanie oczekujących wiadomości |
| 10.5.2 | Rozszerzenie ChatMessage | Pola ToolCallId, ToolCallsJson, Thinking dla kontekstu AI; PromptTokens, CompletionTokens, TotalTokens dla śledzenia tokenów; typ wiadomości SystemNotification |
| 10.5.3 | TokenUsageAuditManager | Śledzenie zużycia tokenów na żądanie we wszystkich istotach. Zagregowane statystyki, zapytania szeregów czasowych, utrwalane przechowywanie |
| 10.5.4 | TokenAuditTool | Narzędzie `[SiliconManagerOnly]` do odpytywania i podsumowywania użycia tokenów przez Kuratora |
| 10.5.5 | ConfigTool | Narzędzie `[SiliconManagerOnly]` do odczytu i modyfikacji konfiguracji systemu przez Kuratora |
| 10.5.6 | AuditController | Pulpit nawigacyjny Web do audytu użycia tokenów, z wykresami trendów i eksportem danych |
| 10.5.7 | Rozszerzenie systemu kalendarzowego | 32 implementacje kalendarzy, obejmujące światowe systemy kalendarzowe (buddyjski, chiński księżycowy, islamski, hebrajski, japoński, perski, majański itp.) |
| 10.5.8 | Rozszerzenie DiskTool | Nowe operacje: count_lines, read_lines, clear_file, replace_lines, replace_text, replace_text_all, list_drives |
| 10.5.9 | Rozszerzenie SystemTool | Nowe operacje: find_process (obsługa symboli wieloznacznych), resource_usage |
| 10.5.10 | Rozszerzenie CalendarTool | Nowe operacje: diff, list_calendars, get_components, get_now_components, convert (konwersja między kalendarzami) |
| 10.5.11 | DashScopeClient | Klient AI Alibaba Cloud Bailian, kompatybilny z API OpenAI. Obsługa strumieniowania, wywołań narzędzi, treści wnioskowania |
| 10.5.12 | DashScopeClientFactory | Fabryka do tworzenia klientów Bailian. Dynamiczne odkrywanie modeli przez API. Obsługa wielu regionów (Pekin, Wirginia, Singapur, Hongkong, Frankfurt) |
| 10.5.13 | System konfiguracji klienta AI | Konfiguracja klienta AI dla każdej istoty. Dynamiczne opcje kluczy konfiguracji (model, region). Zlokalizowane nazwy wyświetlane |
| 10.5.14 | Rozszerzenie lokalizacji | Lokalizacja chińska uproszczona, chińska tradycyjna, angielska i japońska dla opcji konfiguracji Bailian, nazw modeli i nazw regionów |

**Dostarczane**: Rozszerzone narzędzia, obserwowalność, pokrycie kalendarzowe i obsługa wielu backendów AI.

**Weryfikacja**: Kurator odpytuje użycie tokenów przez TokenAuditTool → pulpit audytu pokazuje trendy → CalendarTool konwertuje daty między 32 systemami kalendarzowymi → przełącz backend AI na Bailian → czatuj z modelem Qwen przez API chmurowe.

---

## ~~Etap 10.6: Udoskonalenie i optymalizacja~~ ✅ Zakończony

**Cel**: Udoskonalenie funkcji systemu, dodanie nowych cech, optymalizacja doświadczenia użytkownika.

| # | Moduł | Opis |
|---|--------|-------------|
| 10.6.1 | WebViewBrowserTool | Oparte na Playwright narzędzie automatyzacji przeglądarki wieloplatformowej, obsługa trybu bezgłowego, izolacja indywidualna, pełna obsługa JS/CSS |
| 10.6.2 | HelpTool | Narzędzie systemu dokumentacji pomocy, obsługa wielojęzycznego odpytywania i wyświetlania dokumentacji |
| 10.6.3 | ProjectWorkNoteTool | Narzędzie notatek roboczych projektu, obsługa zapisów pracy w wymiarze projektu i zarządzania |
| 10.6.4 | ProjectTaskTool | Narzędzie zarządzania zadaniami projektu, obsługa przypisywania zadań i śledzenia postępu |
| 10.6.5 | KnowledgeTool | Narzędzie sieci wiedzy, obsługa operacji CRUD na wiedzy trójkowej i odkrywania ścieżek |
| 10.6.6 | ChatHistoryController | Kontroler przeglądania historii czatu, obsługa listy sesji i szczegółów wiadomości |
| 10.6.7 | CodeHoverController | Kontroler podpowiedzi kodu przy najechaniu, obsługa podświetlania składni i podpowiedzi kodu |
| 10.6.8 | WorkNoteController | Kontroler zarządzania notatkami roboczymi, obsługa wyszukiwania i generowania katalogu |
| 10.6.9 | TimerExecutionHistory | Funkcja historii wykonania czasomierza, rejestrowanie i przeglądanie historii wyzwalaczy czasomierza |
| 10.6.10 | Rozszerzenie lokalizacji | Dodanie obsługi lokalizacji czeskiej (cs-CZ), łącznie 21 wariantów językowych |
| 10.6.11 | Optymalizacja Web UI | Obsługa przesyłania plików, wskaźniki ładowania, optymalizacja renderowania wywołań narzędzi, naprawa modalu notatek roboczych |
| 10.6.12 | Rozszerzenie zarządzania pamięcią | Zaawansowane filtrowanie, statystyki, widok szczegółowy, optymalizacja algorytmu kompresji |
| 10.6.13 | Refaktoring systemu logów | Separacja logów systemowych/istot krzemowych, API odczytu logów, filtry istot krzemowych |
| 10.6.14 | Rozszerzenie systemu uprawnień | Weryfikacja prekompilacji wywołań zwrotnych uprawnień, weryfikacja referencji zestawów, biała lista usługi pogodowej wttr.in |

**Dostarczane**: Kompletna automatyzacja przeglądarki WebView, system dokumentacji pomocy, obszar roboczy projektów, sieć wiedzy, przeglądanie historii czatu i inne rozszerzenia.

**Weryfikacja**: Istoty Krzemowe mogą operować przeglądarką przez WebViewBrowserTool → uzyskiwać dokumentację pomocy przez HelpTool → zarządzać notatkami roboczymi i zadaniami projektów → odpytywać sieć wiedzy → przeglądać historię czatu.

---

## Etap 11: Integracja z zewnętrzną komunikacją natychmiastową

**Cel**: Połączenie z zewnętrznymi platformami komunikacyjnymi dla szerszej dostępności dla użytkowników.

| # | Moduł | Opis |
|---|--------|-------------|
| 11.1 | FeishuProvider | Integracja bota Feishu (Lark), obsługa kart |
| 11.2 | WhatsAppProvider | Integracja WhatsApp Business API |
| 11.3 | TelegramProvider | Integracja Telegram Bot API, obsługa klawiatur inline |
| 11.4 | Rozszerzenie IMManager | Routing wielodostawców, ujednolicony format wiadomości, obsługa zapytań o uprawnienia między platformami |

**Dostarczane**: Użytkownicy mogą wchodzić w interakcję z Istotami Krzemowymi przez zewnętrzne platformy komunikacji natychmiastowej.

---

## Etap 12: Funkcje zaawansowane

**Cel**: Opcjonalne funkcje zaawansowane dla rozszerzonych możliwości.

| # | Moduł | Opis |
|---|--------|-------------|
| 12.1 | Sieć wiedzy | Współdzielony graf wiedzy wykorzystujący strukturę trójkową (podmiot-orzeczenie-dopełnienie) |
| 12.2 | ~~System wtyczek~~ ✅ Zakończony | Ładowanie zewnętrznych wtyczek, z sprawdzaniem bezpieczeństwa i piaskownicą (interfejs IPlugin, PluginLoader, izolacja AssemblyLoadContext) |
| 12.3 | Ekosystem umiejętności | Rynek wielokrotnego użytku umiejętności dla możliwości istot
