# Dziennik zmian

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md) | [Русский](../ru-RU/changelog.md)

Wszystkie istotne zmiany w tym projekcie będą dokumentowane w tym pliku.

Format oparty na [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
projekt stosuje [wersjonowanie semantyczne](https://semver.org/spec/v2.0.0.html).

---

## O tym dzienniku zmian

### Dwie wersje projektu

Projekt oferuje dwie wersje implementacji:

- **SiliconLife.Default**: implementacja domyślna, głównie do weryfikacji wykonalności architektury. Aplikacja konsolowa, przechowywanie JSON w systemie plików.
- **SiliconLife.Fast**: rekomendowana wersja produkcyjna. Wieloplatformowa aplikacja desktopowa (Windows / macOS / Linux), przechowywanie w pamięci SpeedyPack + asynchroniczne utrwalanie, głęboko zoptymalizowana pod kątem wydajności.

Obie wersje współdzielą te same interfejsy i funkcjonalności, różnią się jedynie implementacją przechowywania i trybem działania. SiliconLife.Default służy jako referencyjna weryfikacja architektury, SiliconLife.Fast jako główna wersja do środowisk produkcyjnych.

### Pochodzenie projektu

- Projekt powstał 20 marca 2026 roku.
- Przed tym projektem istniało demo weryfikacyjne, które nie powiodło się z powodu nieodpowiedniego projektu architektonicznego, co uniemożliwiło integrację z wieloma platformami AI.

### Używane narzędzia AI IDE

#### Kiro (Amazon AWS)
- Projekt był początkowo utrzymywany przez Kiro i uruchomiony w trybie Spec.
- Kiro to środowisko programistyczne agentic AI zbudowane przez Amazon AWS.
- Oparte na Code OSS (VS Code), obsługuje ustawienia VS Code i wtyczki kompatybilne z Open VSX.
- Posiada napędzany specyfikacjami przepływ pracy programistycznej do ustrukturyzowanego kodowania AI.

#### Comate AI IDE / 文心快码 (Baidu)
- Sporadycznie używany do prac redakcyjnych i dokumentacyjnych.
- Comate AI IDE to natywne środowisko programistyczne AI wydane przez Baidu Wenxin 23 czerwca 2025 roku.
- Pierwsze w branży wielomodalne, wieloagentowe współpracujące AI IDE.
- Funkcje obejmują konwersję projektu na kod i pełnoprzepływowe wspierane przez AI kodowanie.
- Napędzane przez model Baidu Wenxin 4.0 X1 Turbo.

#### Trae (ByteDance)
- Używane od października 2025 do kwietnia 2026.
- AI IDE wspierające inteligentne generowanie kodu i zarządzanie projektami.

#### Qoder (Alibaba)
- Od 18 kwietnia 2026 używane do utrzymania projektu.
- Platforma kodowania AI wspierająca analizę kodu, generowanie dokumentacji i wieloagentową współpracę.

#### CatPaw (Meituan)
- Od 6 maja 2026 używane mieszanie z Qoder.
- Oparte na autorskiej serii modeli LongCat firmy Meituan, posiada silne możliwości pełnej refaktoryzacji architektury kodu.

### Dokumentacja wymagań

- Dokumentacja wymagań tego projektu nie jest publicznie dostępna.
- Wymagania zostały wielokrotnie zweryfikowane przez ponad 12 międzynarodowych platform AI i dużych serii modeli, generując ponad 2000 linii dokumentu wymagań napędzanego historiami użytkownika, niemal niezrozumiałego dla człowieka.

---

## [Nieopublikowane]

### 2026-05-26

#### Nowe funkcje
- `a49041b` - Dodanie obsługi lokalizacji rosyjskiej (ru-RU) (ref task-364)
  - 216 zmienionych plików

#### Poprawki
- `79096f2` - Zmiana formatu tabeli glossary na standardowy Markdown, usunięcie nadmiarowych spacji wyrównujących
  - 1 zmieniony plik

#### Dokumentacja
- `174a954` - Uzupełnienie brakujących tłumaczeń terminów w kolumnach Deutsch/Polski/Português w glossary
  - 1 zmieniony plik

#### Ramy współpracy
- `5b03d53` - Aktualizacja rekordów zadań .ai-collab — task-364 lokalizacja rosyjska (ref task-364)
  - 5 zmienionych plików

- `018947d` - Archiwizacja sesji i zmian z 2026-05-25
  - 2 zmienione pliki

### 2026-05-25

#### Nowe funkcje
- `14721a9` - Uszczegółowienie podpowiedzi personelu ThinkOnProject jako szczegółowego planu działania (ref task-363)
  - 20 zmienionych plików

#### Poprawki
- `abb4285` - Naprawa błędu pozycji wywołania .join() w beingsHtml (ref task-361)
  - 1 zmieniony plik

- `1c0b9ed` - Usunięcie błędu duplikacji ciągu state-initial w renderowaniu states-overview w WorkflowDetailView (ref task-362)
  - 6 zmienionych plików

#### Ramy współpracy
- `ecc48a1` - Aktualizacja metadanych .ai-collab (relatedCommit i activity log) (ref task-361)
  - 4 zmienione pliki

- `64529a7` - Archiwizacja sesji i zmian z 2026-05-24 (ręczne uzupełnienie wykonania)
  - 28 zmienionych plików

- `4150e52` - Archiwizacja ukończonych zadań task-341~361 (ref archive)
  - 2 zmienione pliki

### 2026-05-24

#### Nowe funkcje
- `db60fd9` - Lista uprawnień narzędzi wyświetla narzędzia bez deklaracji ToolAction i oznacza je jako niemożliwe do skonfigurowania (ref task-331, task-332, task-333)
  - 21 zmienionych plików

- `6004a7f` - WorkflowTemplate dodaje obsługę definicji ról + lokalizacja 12 języków + poprawka DiskTool (ref task-346)
  - 24 zmienione pliki

- `75ce452` - Pula ról ProjectSpace i akcje zarządzania rolami ProjectTool (ref task-347)
  - 12 zmienionych plików

- `edfb600` - BuildProjectScenarioContext dodaje informacje o rolach (ref task-348)
  - 21 zmienionych plików

- `6a2d713` - Rozszerzenie HasProjectsWithoutTemplate na HasProjectsNeedingAttention (ref task-349)
  - 21 zmienionych plików

- `a773224` - Tworzenie zadań przepływu pracy używa puli ról do przypisywania wykonawców (ref task-350)
  - 6 zmienionych plików

- `77a27f9` - Rozszerzenie TravelCodeWikiTool jako punktu wejścia jednostek geograficznych (ref task-353)
  - 8 zmienionych plików

- `873ef23` - Implementacja GeoDataTool ukończona, aktualizacja stanu .ai-collab (ref task-352)
  - 7 zmienionych plików

- `feaccab` - Implementacja GeoContentTool ukończona, aktualizacja stanu .ai-collab (ref task-351)
  - 6 zmienionych plików

- `6e60ad1` - Rozszerzenie GeoLanguageTool (obsługa ObjectPath + set_word), uzupełnienie metadanych (ref task-356, task-355)
  - 7 zmienionych plików

- `4eff807` - Implementacja GetWikiDocuments() w podklasach GeoLocation (ref task-357)
  - 5 zmienionych plików

- `baad5df` - Implementacja usługi publikacji MediaWiki API (ref task-358)
  - 6 zmienionych plików

- `b846a21` - Implementacja strony szczegółów przepływu pracy (ref task-361)
  - 24 zmienione pliki

#### Poprawki
- `a290088` - Utrata istot krzemowych utworzonych przez CuratorTool po ponownym uruchomieniu (ref task-334)
  - 11 zmienionych plików

- `69a8cba` - Naprawa błędu braku filtrowania strony zadań według beingId (ref task-360)
  - 8 zmienionych plików

- `7dd1a65` - Rejestracja trasy strony szczegółów przepływu pracy w Router.cs (ref task-361)
  - 1 zmieniony plik

#### Refaktoryzacja
- `5e02711` - Refaktoryzacja abstrakcji ścieżek przechowywania warstwy wspólnej, eliminacja hardcodowania systemu plików (ref task-335)
  - 12 zmienionych plików

- `0ec0929` - DynamicBeingLoader.SaveBeingCode używa IStorage zamiast bezpośrednich operacji systemu plików (ref task-336)
  - 7 zmienionych plików

- `9a44b48` - Most IStorage PlaywrightWebView + dekorelacja klasy bazowej WebViewBrowserTool (ref task-337, task-340)
  - 11 zmienionych plików

- `8fea742` - Zapis zrzutów ekranu WebViewBrowserTool używa IStorage zamiast bezpośrednich operacji systemu plików (ref task-338)
  - 6 zmienionych plików

- `4c24e6d` - DefaultPermissionCallback używa BeingPathResolver zamiast hardkodowanych ścieżek (ref task-339)
  - 6 zmienionych plików

- `ab428cd` - Usunięcie downcastingu DefaultSiliconBeing, bezpośrednie wywołanie SaveState() klasy bazowej (ref task-344)
  - 7 zmienionych plików

- `1e6eb80` - Most plików tymczasowych stanu przeglądarki PlaywrightWebView zmieniony na bezpośredni odczyt/zapis IStorage (ref task-341)
  - 7 zmienionych plików

- `17f00e9` - Operacja wyszukiwania DiskTool przekierowana przez DiskExecutor (ref task-342)
  - 8 zmienionych plików

- `8158703` - Sprawdzanie załączników ChatController przekierowane przez DiskExecutor (ref task-343)
  - 7 zmienionych plików

- `3243ae6` - Przepisanie TravelCodeWikiPublishWorkflow jako 7-etapowej maszyny stanów, usunięcie wymuszonego śledzenia pliku TravelCodeWikiWithAI (ref task-355)
  - 6 zmienionych plików

#### Czyszczenie
- `d685288` - Usunięcie HotReloadTool.cs i katalogu tools/HotReload (ref task-345)
  - 8 zmienionych plików

#### Dokumentacja
- `f1789d1` - Optymalizacja wierszy opisowych README.md (ref task-359)
  - 9 zmienionych plików

#### Ramy współpracy
- `982c6bb` - Uzupełnienie brakujących pól relatedCommit i commitHash w .ai-collab
  - 6 zmienionych plików

- `d91e9f8` - Archiwizacja task-331~340, wyczyszczenie tablicy zadań
  - 2 zmienione pliki

- `9135e30` - Publikacja task-341~344 refaktoryzacja IStorage warstwy wspólnej + korekta abstrakcji
  - 1 zmieniony plik

- `f70b350` - Dodanie 13 zadań przebudowy architektury TravelCodeWikiWithAI (ref task-346~358)
  - 2 zmienione pliki

- `f81d38b` - Aktualizacja sesji ai-collab i plików śledzenia zadań
  - 3 zmienione pliki

### 2026-05-23

#### Poprawki
- `9c3c64e` - Naprawa obejścia weryfikacji uprawnień ExecuteTool w czasie wykonywania dla ograniczeń na poziomie projektu (ref task-324)
  - 7 zmienionych plików

- `94a9e35` - Naprawa niespójności między definicją szablonu uprawnień a deklaracją ToolActionAttribute (ref task-325)
  - 6 zmienionych plików

- `e8d8371` - Narzędzia ze wszystkimi akcjami wyłączonymi są całkowicie usuwane z żądań AI (ref task-326)
  - 6 zmienionych plików

- `32c7d8a` - API uprawnień narzędzi dodaje weryfikację nazw akcji + naprawa renderowania Markdown historii czatu (ref task-327, task-328, task-329)
  - 9 zmienionych plików

- `797db8c` - Renderowanie Markdown fallback błędnie ustawia mdRendered, powodując brak ponownego renderowania po załadowaniu marked (ref task-330)
  - 9 zmienionych plików

#### Ramy współpracy
- `1496094` - Publikacja zadań naprawy ramy autoryzacji narzędzi task-324~327
  - 776 zmienionych plików

- `0d16e63` - Aktualizacja stanu zadań współpracy, powiązanie task-330 z zatwierdzeniem 797db8c, przygotowanie do archiwizacji
  - 2 zmienione pliki

- `e602e1c` - Archiwizacja task-316~330, wyczyszczenie tablicy zadań (ref task-316~330)
  - 2 zmienione pliki

- `20291ce` - Codzienna archiwizacja sesji i zmian (13–22 maja)
  - 106 zmienionych plików

### 2026-05-22

#### Poprawki spójności dokumentacji
- `9e07b27` - Korekta różnic spójności dokumentacji francuskiej (fr-FR) z kodem źródłowym (ref task-307)
  - 10 zmienionych plików

- `9e3be72` - Korekta spójności dokumentacji niemieckiej (de-DE) z kodem źródłowym (ref task-308)
  - 5 zmienionych plików

- `2bc7151` - Korekta różnic spójności dokumentacji hiszpańskiej (es-ES) z kodem źródłowym (ref task-309)
  - 13 zmienionych plików

- `f95088e` - Korekta spójności dokumentacji włoskiej (it-IT) z kodem źródłowym (ref task-310)
  - 11 zmienionych plików

- `6ea9f4a` - Korekta spójności dokumentacji polskiej (pl-PL) z kodem źródłowym (ref task-311)
  - 16 zmienionych plików

- `7646923` - Korekta spójności dokumentacji portugalskiej (pt-PT) z kodem źródłowym (ref task-312)
  - 12 zmienionych plików

- `7eaf9db` - Korekta spójności dokumentacji czeskiej (cs-CZ) z kodem źródłowym (ref task-313)
  - 12 zmienionych plików

#### Ramy współpracy
- `3cb7347` - Aktualizacja relatedCommit=7eaf9db dla task-313
  - 1 zmieniony plik

### 2026-05-21

#### Nowe funkcje
- `99eca78` - Dodanie funkcji „Wyświetl przechowywanie (tylko do odczytu)" w menu kontekstowym, wywołanie Speedy.Manager w procesie (ref task-301)
  - 26 zmienionych plików

#### Poprawki spójności dokumentacji
- `7f65cf1` - Korekta różnic spójności dokumentacji zh-CN z kodem źródłowym (ref task-303)
  - 15 zmienionych plików

- `a9e2a2c` - Korekta różnic spójności dokumentacji angielskiej (en) z kodem źródłowym (ref task-302)
  - 9 zmienionych plików

- `2549105` - Korekta różnic spójności dokumentacji tradycyjnego chińskiego (zh-HK) z kodem źródłowym (ref task-304)
  - 12 zmienionych plików

- `277eb50` - Korekta różnic spójności dokumentacji japońskiej z kodem źródłowym (ref task-305)
  - 10 zmienionych plików

- `edce413` - Korekta różnic spójności dokumentacji koreańskiej (ko-KR) z kodem źródłowym (ref task-306)
  - 18 zmienionych plików

- `f2adcae` - Naprawa niespójności dokumentacji portugalskiej z kodem źródłowym (ref task-220)
  - 15 zmienionych plików

- `3332987` - Naprawa niespójności dokumentacji tradycyjnego chińskiego (Hongkong) z kodem źródłowym (ref task-218)
  - 14 zmienionych plików

- `af9f715` - Naprawa niespójności dokumentacji polskiej z kodem źródłowym (ref task-217)
  - 15 zmienionych plików

- `2e2b18b` - Naprawa niespójności dokumentacji koreańskiej z kodem źródłowym (ref task-216)
  - 16 zmienionych plików

- `626ebc9` - Naprawa niespójności dokumentacji japońskiej z kodem źródłowym (ref task-215)
  - 19 zmienionych plików

- `48d061b` - Naprawa niespójności dokumentacji włoskiej z kodem źródłowym (ref task-214)
  - 14 zmienionych plików

#### Ramy współpracy
- `6683bee` - Rejestracja zespołu Marvis AI, aktualizacja stanu zadań
  - 3 zmienione pliki

- `03fc905` - Archiwizacja task-210~220
  - 5 zmienionych plików

### 2026-05-20

#### Nowe funkcje
- `65176d4` - Dodanie pełnej obsługi lokalizacji portugalskiej (pt-PT + pt-BR) (ref task-208)
  - 41 zmienionych plików

#### Poprawki spójności dokumentacji
- `af4dffd` - Korekta wszystkich niespójności dokumentacji zh-CN z kodem źródłowym (ref task-209)
  - 11 zmienionych plików

- `144b945` - Korekta niespójności dokumentacji angielskiej (en) i czeskiej (cs-CZ) z kodem źródłowym (ref task-219, task-210)
  - 22 zmienione pliki

- `08bec55` - Korekta niespójności dokumentacji niemieckiej (de-DE) z kodem źródłowym (ref task-211)
  - 14 zmienionych plików

- `7ff28de` - Naprawa niespójności dokumentacji hiszpańskiej (es-ES) z kodem źródłowym (ref task-212)
  - 14 zmienionych plików

- `15e2133` - Naprawa niespójności dokumentacji francuskiej (fr-FR) z kodem źródłowym (ref task-213)
  - 13 zmienionych plików

#### Poprawki
- `7dac388` - Naprawa braku możliwości wyświetlania listy zadań projektu (ref task-207)
  - 6 zmienionych plików

#### Ramy współpracy
- `7890223` - Archiwizacja task-201~209, publikacja zadań naprawy spójności dokumentacji task-210~220
  - 5 zmienionych plików

### 2026-05-19

#### Nowe funkcje
- `cd72846` - Implementacja bezpiecznej alternatywy obejścia skanowania bezpieczeństwa PluginLoader (ref task-203)
  - 13 zmienionych plików

- `fc0c00c` - Ulepszenie funkcji Speedy.Manager — tworzenie/import/eksport/TreeView hierarchii/okno postępu (ref task-206)
  - 9 zmienionych plików

#### Poprawki
- `ec07118` - Naprawa problemu ITypeRegistry/IObjectFactory niezarejestrowanych przed ładowaniem wtyczek (ref task-205)
  - 8 zmienionych plików

- `9e749db` - Naprawa błędu „Creator ID is required" przy tworzeniu projektu (ref task-204)
  - 4 zmienione pliki

#### Infrastruktura
- `43dc092` - Migracja CLDR — dodanie CldrDataProvider, usunięcie .github
  - 1 zmieniony plik

- `c09ec1f` - Dodanie cldr/ do .gitignore
  - 1 zmieniony plik

- `221f818` - Zmiana synchronizacji GitHub na schemat mirroringu push z Gitee, workflow zachowany tylko jako ręczny zapas
  - 1 zmieniony plik

- `08cdf1a` - Naprawa workflow synchronizacji GitHub — dodanie logiki ponownych prób i pomijania braku zmian
  - 1 zmieniony plik

- `fb4e77d` - Aktualizacja SiliconLife.Speedy.Manager.csproj
  - 1 zmieniony plik

#### Ramy współpracy
- `df90af0` - Aktualizacja relatedCommit=cd72846 dla task-203
  - 1 zmieniony plik

### 2026-05-18

#### Refaktoryzacja
- `e720d06` - Całkowita przebudowa Speedy.Manager z WinForms na Avalonia (ref task-202)
  - 17 zmienionych plików

#### Poprawki
- `08894a9` - Naprawa błędu wyświetlania poziomu wpisów podsumowania osi czasu pamięci (ref task-201)
  - 3 zmienione pliki

#### Ramy współpracy
- `2871afb` - Archiwizacja wszystkich zadań, wyczyszczenie tasks.json
  - 2 zmienione pliki

### 2026-05-17

#### Nowe funkcje
- `d6eb994` - Dodanie punktu tworzenia projektu i wyboru szablonu przepływu pracy na stronie listy projektów (ref task-203)
  - 14 zmienionych plików

- `0872134` - Napędzane przez kuratora aranżowanie projektów bez szablonu ThinkOnProject (ref task-202)
  - 6 zmienionych plików

- `cb3188e` - Wizualizacja @wspomnień w czacie grupowym (ref task-208)
  - 4 zmienione pliki

- `f9968e5` - Deklaracja zdolności ToolCall klienta AI i elegancka degradacja (ref task-205)
  - 4 zmienione pliki

- `0d2b843` - Logika decyzyjna czatu grupowego ShouldReplyInGroupChat (ref task-201)
  - 6 zmienionych plików

- `277a2b1` - Uzupełnienie Sieci Wiedzy — zaawansowane zapytania i przechodzenie grafu (ref task-207)
  - 9 zmienionych plików

#### Poprawki
- `6d0b66e` - Naprawa TypeError appendMessage podczas wysyłania wiadomości w czacie grupowym (ref task-209)
  - 5 zmienionych plików

- `b15167c` - Uzupełnienie pominiętej rejestracji trasy list-workflow-templates w task-203 (ref task-203)
  - 1 zmieniony plik

- `dc549a2` - Naprawa workflow synchronizacji Gitee — dodanie nazwy użytkownika do URL tokenu
  - 1 zmieniony plik

#### Infrastruktura
- `e5fa3ad` - Wyłączenie automatycznego harmonogramu synchronizacji GitHub, oczekiwanie na oficjalne rozwiązanie synchronizacji Gitee
  - 1 zmieniony plik

#### Ramy współpracy
- `4a58c82` - Dodanie raportu analizy zdolności systemu + projekt ThinkOnProject
  - 5 zmienionych plików

- `8ab29e6` - Archiwizacja raportu kompletności zdolności systemu do .ai-collab/docs
  - 2 zmienione pliki

- `b412d9c` - Archiwizacja starych zadań, ponowna publikacja task-201~208 na podstawie analizy kompleksowej
  - 2 zmienione pliki

- `437884a` - Aktualizacja metadanych współpracy — ukończenie task-202/203/204 (ref task-202, task-203, task-204)
  - 2 zmienione pliki

- `bf78d79` - Aktualizacja metadanych współpracy — ukończenie task-201/205/208
  - 2 zmienione pliki

- `de6ee0e` - Rekord zakończenia sesji catpaw-20260517-2215
  - 5 zmienionych plików

- `7223b6f` - Rekord zakończenia sesji catpaw-20260517-2200
  - 4 zmienione pliki


## [Alpha-0.2] - 2026-05-16

### 2026-05-16

#### Przygotowanie wydania
- `476d839` - Dodanie zadań wydania alpha-0.2
  - Utworzenie task-114 (pisanie CHANGELOG) i task-115 (aktualizacja numeru wersji)
  - 1 zmieniony plik

### 2026-05-15

#### Infrastruktura
- `672627b` - Dodanie workflow synchronizacji Gitee (z konfiguracją uprawnień)
  - Aktualizacja konfiguracji uprawnień workflow sync-from-gitee.yml
  - 1 zmieniony plik, 7 linii dodanych, 4 linii usuniętych

- `3cd5256` - Dodanie automatycznej synchronizacji kodu Gitee przez GitHub Actions
  - Nowy workflow sync-from-gitee.yml
  - 1 zmieniony plik, 50 linii dodanych

#### Aktualizacja dokumentacji
- `aa1d2ad` - Aktualizacja dokumentacji README/architecture/getting-started dla wszystkich 11 języków, odzwierciedlenie wieloplatformowej obsługi SiliconLife.Fast (ref task-112, task-113)
  - Korekta opisów SiliconLife.Fast jako tylko Windows, odzwierciedlenie faktycznej obsługi wieloplatformowej (Windows / macOS / Linux)
  - Aktualizacja README.md, architecture.md, getting-started.md w 11 językach
  - Dodanie obsługi atrybutu hint w SelectComponent
  - Przekazanie hint do rozwijanej listy wyliczeniowej w ConfigView
  - 11 języków lokalizacji dodaje klucz SelectSearchHint
  - 53 zmienione pliki, 690 linii dodanych, 194 linii usuniętych

#### System zadań
- `3329f3d` - Dodanie mechanizmu inspekcji systemu zadań + zadania naprawy błędu lokalizacji
  - Utworzenie task-113: naprawa problemu lokalizacji strony o
  - Aktualizacja task-112: aktualizacja dokumentacji wersji Fast dla obsługi Linux
  - Archiwizacja ukończonych zadań (11) do .ai-collab/archive/
  - Konfiguracja mechanizmu inspekcji: szybka inspekcja (co 30 minut) + pełna inspekcja (codziennie 06:00)
  - 2 zmienione pliki, 148 linii dodanych, 171 linii usuniętych

#### Ramy współpracy
- `6038e22` - Rejestracja coze-agent w rejestrze współpracy .ai-collab
  - Dodanie informacji o rejestracji stałego AI platformy Coze
  - 1 zmieniony plik

### 2026-05-14

#### Ramy współpracy AI
- `7344fbb` - Usunięcie trybu handoff, zmiana na napędzany listą zadań (v2.0)
  - Refaktoryzacja struktury katalogu .ai-collab z trybu przekazania handoff na napędzany listą zadań
  - Nowy plik rdzeniowy listy zadań tasks.json
  - Nowy dziennik operacji activity.log
  - Nowe katalogi changes/ i sessions/

- `589a48e` - Dodanie rekordów sesji .ai-collab
  - Nowe rekordy stanu sesji współpracy AI

- `5481bcf` - Rejestracja Qoder AI IDE w rejestrze współpracy
  - Nowe informacje o rejestracji asystenta programowania AI Qoder

- `e2d7b61` - Uzupełnienie relatedCommit w tasks.json i commitHash w changes
  - Udoskonalenie powiązania metadanych zadań

- `a087f0c` - Akceptacja wszystkich zadań task-101~110
  - Potwierdzenie ukończenia napraw wszystkich 10 zadań

#### Poprawki błędów
- `fac9435` - Ukończenie napraw i implementacji wszystkich 10 zadań task-101~110
  - Naprawa brakującego tekstu podpowiedzi w komponencie wyboru wyszukiwania
  - Naprawa problemu lokalizacji strony o
  - Naprawa błędu JS wyszukiwania systemu pomocy
  - 39 zmienionych plików, 684 linii dodanych, 121 linii usuniętych

- `c46dfbc` - Ukończenie wszystkich zadań oczekujących (task-001~006)
  - Ukończenie początkowych 6 zadań oczekujących

- `ec176b2` - Nadpisanie listy zadań — przegląd kodu odkrył 10 nowych błędów
  - Utworzenie 10 nowych zadań task-101~110

#### Refaktoryzacja
- `ab15915` - Ujednolicenie nagłówków praw autorskich + naprawa BOM HelpController i JS wyszukiwania HelpView
  - Ujednolicenie nagłówków Apache 2.0 we wszystkich plikach źródłowych C#
  - Naprawa problemu kodowania BOM w HelpController
  - Naprawa błędu JavaScript wyszukiwania w HelpView

#### Nowe funkcje
- `18a6f5d` - Utworzenie serwera możliwości przeglądarki MCP (ref task-111)
  - Nowy projekt SiliconLife.McpServer
  - Implementacja serwera MCP automatyzacji przeglądarki Playwright

- `9eb251a` - Usunięcie modułu SiliconLife.McpServer (ref task-111)
  - Usunięcie niezależnego serwera MCP, funkcja zintegrowana z głównym projektem

### 2026-05-13

#### Lokalizacja
- `7a62590` - Dodanie obsługi lokalizacji polskiej
  - Nowa implementacja lokalizacji pl-PL (PlPL.cs, 1089 linii)
  - Nowa lokalizacja dokumentacji pomocy polskiej (HelpLocalizationPlPL.cs, 3972 linie)
  - Nowa obsługa chińskiego kalendarza historycznego w języku polskim (ChineseHistoricalPlPL.cs, 600 linii)
  - Nowa lokalizacja zasobnika polskiego (TrayPlPL.cs, 135 linii)
  - Nowy kompletny zestaw dokumentacji polskiej (15 dokumentów)
  - Dodanie języka polskiego do enumeracji Language
  - 35 zmienionych plików, 14379 linii dodanych, 11 linii usuniętych

- `51f9c8e` - Aktualizacja referencji AI Ark w dokumentacji i ulepszenie terminologii
  - Aktualizacja terminologii klienta AI w wielojęzycznej dokumentacji

- `7587c12` - Dodanie wpisów dziennika zmian dla wszystkich języków
  - Synchroniczna aktualizacja changelog we wszystkich wersjach językowych

#### Migracja systemu okien
- `b49a07d` - Migracja do trybu rezydencji okna Avalonia
  - Usunięcie zależności Windows Forms, pełna migracja do frameworka Avalonia UI
  - Okno stanu wyświetla się poprawnie na Linux (weryfikacja pulpitu zdalnego)
  - Dodanie kontroli okna: menu kontekstowe, podwójne kliknięcie otwiera Web, przycisk zamykania
  - Dodanie ramy współpracy wieloagentowej (.ai-collab/)
  - Naprawa inicjalizacji ikony zasobnika (elegancka degradacja)
  - Nowe wejście aplikacji Avalonia App.axaml i App.cs
  - 13 zmienionych plików, 1442 linie dodane, 541 linii usuniętych

- `d335aaf` - Okno platformy Linux zawsze widoczne + okno potwierdzenia zamykania
  - Automatyczne wyświetlanie okna stanu na Linux (brak ikony zasobnika)
  - Okno potwierdzenia przy zamykaniu okna na Linux
  - Windows/macOS zachowują oryginalne zachowanie zasobnika
  - Obsługa parametru --no-tray wymuszającego wyłączenie zasobnika
  - Nowa metoda ShowMessageBoxAsync dla okna potwierdzenia
  - 3 zmienione pliki, 206 linii dodanych, 29 linii usuniętych

#### Refaktoryzacja systemu zasobnika
- `841d384` - Refaktoryzacja systemu zasobnika i inicjalizacja ramy współpracy AI
  - Uproszczenie TrayLocalizationBase, usunięcie nieużywanych właściwości
  - Dodanie elementu lokalizacji ShowStatus
  - App.cs dodaje kliknięcie ikony zasobnika wyświetlające okno stanu, zlokalizowane elementy menu
  - Program.cs przenosi inicjalizację ikony zasobnika do StartAsync
  - TrayStatusWindow ukrywa się przy zamykaniu zamiast wychodzić
  - Rejestracja trae-glm5 i catpaw w ramie współpracy .ai-collab
  - Aktualizacja .gitignore zapewniająca śledzenie wszystkich plików .ai-collab
  - 22 zmienione pliki, 178 linii dodanych, 1226 linii usuniętych

#### Dokumentacja
- `43653bc` - Aktualizacja opisu repozytorium i rejestru AI
  - Aktualizacja README projektu i informacji o rejestracji .ai-collab

### 2026-05-12

#### Widok webowy systemu zadań
- `0891b3c` - Dodanie widoku szczegółów wykonania zadań i widoku historii
  - Nowy widok szczegółów wykonania zadań TaskExecutionDetailView
  - Nowy widok historii wykonania zadań TaskExecutionHistoryView
  - TaskController dodaje interfejsy zapytań o szczegóły wykonania i historię
  - Nowy model widoku zadań TaskViewModel
  - Ulepszenie centrum zadań TaskCenter
  - Aktualizacja systemu zadań TaskSystem
  - 9 języków lokalizacji dodaje klucze związane z zadaniami
  - 26 zmienionych plików, 803 linie dodane, 55 linii usuniętych

### 2026-05-11

#### Refaktoryzacja architektury komponentów Web
- `5e687ad` - Migracja renderowania komponentów z ciągów znaków na H-tree
  - Metoda renderowania ComponentBase migrowana z trybu ciągów znaków na strukturę H-tree
  - Wszystkie 28 komponentów dostosowane do nowej architektury renderowania (A, Accordion, Button, Calendar, Card, Chart itp.)
  - Znaczna refaktoryzacja SelectComponent (889 linii ulepszeń)
  - Synchroniczna aktualizacja kontrolerów i widoków
  - 33 zmienione pliki, 667 linii dodanych, 435 linii usuniętych

- `bfd332d` - Migracja Style z ciągów znaków na style inline CssBuilder
  - Nowy kreator stylów CssBuilder
  - System stylów ComponentBase migrowany z ciągów znaków na ustrukturyzowany CssBuilder
  - Znaczne ulepszenie LoadingComponent (103 linie dodane)
  - Migracja stylów kontrolerów ConfigController, LogController, MemoryController
  - Migracja stylów widoków ChatView, ConfigView, LogView, MemoryView
  - 37 zmienionych plików, 351 linii dodanych, 157 linii usuniętych

#### Optymalizacja systemu przechowywania
- `d67a7ee` - Optymalizacja QueryLatest dla dużych zbiorów danych
  - Optymalizacja wydajności metody SpeedyTimeStorage QueryLatest
  - Ulepszenie dostawcy dzienników SpeedyLoggerProvider
  - 2 zmienione pliki, 44 linie dodane, 5 linii usuniętych

#### Refaktoryzacja systemu kalendarza
- `9629f88` - Ekstrakcja TimerExecution i ulepszenie widoku webowego czasomierzy
  - Ekstrakcja logiki TimerExecution z TimerSystem (175 linii usuniętych)
  - Znaczne ulepszenie SelectComponent (427 linii ulepszeń)
  - Ulepszenie TimerController i widoku czasomierzy
  - Aktualizacja menedżera kontekstu ContextManager
  - 12 zmienionych plików, 458 linii dodanych, 267 linii usuniętych

#### Lokalizacja
- `5d8ca79` - Dodanie klucza lokalizacji LogsLoading
  - 9 języków dodaje klucz LogsLoading
  - Nowa definicja w klasie bazowej DefaultLocalizationBase
  - 11 zmienionych plików, 15 linii dodanych

### 2026-05-10

#### Refaktoryzacja systemu zadań
- `54394f6` - Połączenie systemu zadań z cyklem historii czatu
  - Znaczne uproszczenie ProjectTaskSystem (411 linii refaktoryzacji)
  - Uproszczenie TaskSystem (254 linie refaktoryzacji)
  - Refaktoryzacja centrum zadań TaskCenter (188 linii ulepszeń)
  - Optymalizacja menedżera kontekstu ContextManager (347 linii refaktoryzacji)
  - Ulepszenie DefaultSiliconBeing
  - Integracja zadań z TimerSystem
  - Aktualizacja interfejsu IWorkNoteStorage
  - Adaptacja SpeedyWorkNoteStorage i FileSystemWorkNoteStorage
  - 16 zmienionych plików, 648 linii dodanych, 897 linii usuniętych

### 2026-05-09

#### Ulepszenie interfejsu Web
- `bc50dd7` - Ulepszenie widoku czatu i dodanie funkcji audytu
  - Nowy kontroler audytu AuditController (261 linii)
  - Nowy widok audytu AuditView (379 linii)
  - Nowy model widoku audytu AuditViewModel
  - Znaczne ulepszenie widoku czatu ChatView (171 linii ulepszeń)
  - Aktualizacja kontrolera czatu ChatController
  - Ulepszenie komponentu MarkdownEditorComponent
  - Ulepszenie kontrolera inicjalizacji InitController
  - Nowe funkcje w systemie czatu ChatSystem
  - 14 zmienionych plików, 1030 linii dodanych, 112 linii usuniętych

- `c9babce` - Ulepszenie renderowania wywołań narzędzi w widoku czatu
  - Ulepszenie renderowania bloków wywołań narzędzi w ChatView
  - 1 zmieniony plik, 54 linie dodane, 11 linii usuniętych

#### System scenariuszy narzędzi AI
- `ff2eddd` - Implementacja systemu filtrowania scenariuszy narzędzi
  - Nowy atrybut scenariusza narzędzia ToolScenarioAttribute (36 linii)
  - Nowy atrybut scenariusza tylko czat ChatOnlyAttribute (19 linii)
  - Menedżer narzędzi ToolManager dodaje filtrowanie scenariuszy (40 linii)
  - Adaptacja menedżera kontekstu ContextManager do filtrowania scenariuszy
  - 4 zmienione pliki, 115 linii dodanych, 30 linii usuniętych

- `5709a33` - Dodanie atrybutów scenariuszy do klas narzędzi
  - 24 klasy narzędzi z adnotacją atrybutu ToolScenario
  - Obejmuje narzędzia kalendarza, czatu, konfiguracji, kuratora, bazy danych, dysku, kompilacji dynamicznej itp.
  - 24 zmienione pliki, 46 linii dodanych, 20 linii usuniętych

#### Refaktoryzacja systemu zadań
- `2f19a5f` - Refaktoryzacja systemu zadań z TaskCenter i TaskEnumerator
  - Nowe centrum zadań TaskCenter (235 linii)
  - Nowy enumerator zadań TaskEnumerator (297 linii)
  - Refaktoryzacja i uproszczenie TaskSystem
  - Adaptacja DefaultSiliconBeing do nowej architektury
  - Aktualizacja fabryki DefaultSiliconBeingFactory
  - Ulepszenie klasy bazowej SiliconBeingBase
  - 7 zmienionych plików, 796 linii dodanych, 275 linii usuniętych

#### Migracja systemu uprawnień
- `a06ed09` - Migracja IM i systemu uprawnień do projektu App
  - Migracja PermissionRequestQueue z Default/Fast do projektu App (443 linie dodane)
  - Usunięcie WebUIProvider wersji Default (403 linie usunięte)
  - Usunięcie HelpTool wersji Default (194 linie usunięte)
  - Usunięcie duplikatów PermissionRequestQueue z wersji Default/Fast
  - Usunięcie IMPermissionAskHandler wersji Default
  - Aktualizacja kontrolera PermissionRequestController
  - 14 zmienionych plików, 496 linii dodanych, 1183 linie usunięte

#### Optymalizacja kontekstu AI
- `4c8aaff` - Optymalizacja menedżera kontekstu i ulepszenie lokalizatora usług
  - Uproszczenie i optymalizacja menedżera kontekstu ContextManager
  - Ulepszenie lokalizatora usług ServiceLocator (36 linii dodanych)
  - Ulepszenie menedżera narzędzi ToolManager (34 linie dodane)
  - Ulepszenia klientów DashScopeClient i VolcengineArkClient
  - Aktualizacja wykonawców (CommandLine, Disk, Network)
  - 8 zmienionych plików, 116 linii dodanych, 98 linii usuniętych

#### Lokalizacja
- `5c5eef7` - Dodanie kluczy lokalizacji audytu i zadań
  - DefaultLocalizationBase dodaje 127 linii definicji lokalizacji
  - 9 języków dodaje klucze związane z audytem i zadaniami (po 26 linii każdy)
  - 11 zmienionych plików, 387 linii dodanych

#### Konfiguracja projektu
- `2067db6` - Aktualizacja konfiguracji projektu i reguł gitignore
  - Aktualizacja reguł .gitignore
  - Ulepszenie konfiguracji DefaultConfigData i Fast DefaultConfigData
  - Ulepszenie przechowywania SpeedyWorkNoteStorage
  - Ulepszenie rdzenia SpeedyPack
  - 5 zmienionych plików, 32 linie dodane, 6 linii usuniętych

### 2026-05-07

#### Lokalizacja włoska
- `8adc18c` - Dodanie obsługi lokalizacji włoskiej i aktualizacja wielojęzycznej dokumentacji
  - Nowa lokalizacja włoska it-IT
  - Nowa implementacja lokalizacji ItIT (1909 linii)
  - Nowa obsługa chińskiego kalendarza historycznego we włoskim (ChineseHistoricalItIT, 586 linii)
  - Nowa lokalizacja zasobnika włoskiego (TrayItIT, 135 linii)
  - Nowy kompletny zestaw dokumentacji włoskiej (14 dokumentów: README, referencja API, architektura, system kalendarza, dziennik zmian, przewodnik współtworzenia itp.)
  - Aktualizacja dokumentacji architektury, przewodnika rozwoju, przewodnika szybkiego startu we wszystkich wersjach językowych
  - Dodanie języka włoskiego do enumeracji Language
  - 86 zmienionych plików, 11573 linie dodane, 769 linii usuniętych

#### Synchronizacja dokumentacji
- `12a5deb` - Aktualizacja wielojęzycznej dokumentacji architektury, dziennika zmian i przewodnika istot krzemowych
  - Aktualizacja README w 8 językach
  - Aktualizacja dokumentacji architektury w 8 językach
  - Aktualizacja dziennika zmian w 8 językach
  - Aktualizacja przewodnika istot krzemowych w 8 językach
  - Aktualizacja referencji narzędzi w 8 językach
  - Refaktoryzacja słowniczka
  - 46 zmienionych plików, 1697 linii dodanych, 442 linie usunięte

### 2026-05-06

#### Duża refaktoryzacja modułów
- `eeb3be6` - Duża refaktoryzacja i reorganizacja modułów
  - Dostosowanie struktury projektu SiliconLife.App
  - Reorganizacja projektu SiliconLife.Fast
  - Reorganizacja projektu SiliconLife.Default
  - Reorganizacja współdzielonego modułu SiliconLife.Common
  - Reorganizacja modułu rdzeniowego SiliconLife.Core
  - Reorganizacja silnika przechowywania SiliconLife.Speedy
  - Reorganizacja narzędzia zarządzania SiliconLife.Speedy.Manager
  - 119 zmienionych plików, 6926 linii dodanych, 3066 linii usuniętych

### 2026-05-04

#### Klienci AI
- `24d2c86` - Dodanie VolcengineArkClient i zastąpienie Audit śledzeniem Usage
  - Nowy klient AI VolcengineArkClient Volcengine Ark
  - Obsługa trybu strumieniowego i niestrumieniowego
  - Wbudowana dwuwarstwowa kontrola szybkości (samoograniczenie + limity serwera)
  - Kompatybilny z protokołem API OpenAI
  - Zastąpienie systemu Audit śledzeniem Usage
  - 24 zmienione pliki, 802 linie dodane, 21 linii usuniętych

#### System narzędzi
- `f27650a` - Dodanie narzędzia hot-reload do samoponownego uruchomienia Fast
  - Nowe narzędzie HotReloadTool
  - Obsługa kompilacji online, aktualizacji i ponownego uruchomienia SiliconLife.Fast
  - Nowy niezależny aktualizator HotReload.exe
  - Bezpieczny mechanizm kopiowania plików (nie nadpisuje samego siebie)
  - Eleganckie zamykanie i oczekiwanie na zwolnienie portu
  - 9 zmienionych plików, 581 linii dodanych

#### Lokalizacja
- `6a5aad8` - Aktualizacja wszystkich plików i dodanie obsługi lokalizacji francuskiej
  - Nowa lokalizacja francuska fr-FR
  - Aktualizacja wszystkich wersji językowych
  - Tłumaczenie dokumentacji pomocy na francuski
  - Tłumaczenie interfejsu na francuski
  - 100+ zmienionych plików

### 2026-05-03

#### Infrastruktura projektu
- `2664b0c` - Aktualizacja infrastruktury projektu i zależności
  - SiliconLife.Speedy.Manager dodaje interfejs zarządzania WPF (MainForm.Designer.cs, MainForm.resx)
  - Nowy zasób ikony slc.ico (1,5 MB)
  - Znaczne ulepszenie skanowania bezpieczeństwa PluginLoader (622 linie dodane)
  - Nowa fabryka strumieni uprawnień PermissionedStreamFactory (779 linii)
  - Nowe kolejki żądań uprawnień PermissionRequestQueue (wersje Default i Fast)
  - Nowy dostawca dzienników debugowania DebugLoggerProvider
  - Ulepszenie klasy bazowej konfiguracji ConfigDataBase
  - ToolManager dodaje funkcję skanowania narzędzi wtyczek (ScanAllPluginAssemblies)
  - Ulepszenie zarządzania cyklem życia SiliconBeingManager
  - Znaczne ulepszenie klienta AI DashScopeClient Alibaba Cloud (227 linii dodanych)
  - Ulepszenie fabryki DefaultSiliconBeingFactory
  - Aktualizacja widoków i kontrolerów Web (ChatView, WorkNoteView, PermissionRequestController)
  - 9 języków lokalizacji dodaje klucze
  - 35 zmienionych plików, 28080 linii dodanych, 336 linii usuniętych

### 2026-05-02

#### Ulepszenie klientów AI
- `c16f99f` - Aktualizacja klientów AI, Web UI i komponentów przechowywania
  - Znaczne ulepszenie klienta DashScopeClient Alibaba Cloud
  - Optymalizacja automatycznego kompaktora SpeedyPackAutoCompactor
  - Ulepszenie klasy bazowej widoków Web i BeingView
  - 6 zmienionych plików, 240 linii dodanych, 81 linii usuniętych

#### System wtyczek
- `242dc98` - Dodanie listy wtyczek na stronie o
  - AboutController dodaje wyświetlanie informacji o wtyczkach
  - AboutViewModel dodaje model danych wtyczek
  - AboutView dodaje renderowanie listy wtyczek
  - 9 języków lokalizacji dodaje klucze związane z wtyczkami
  - 14 zmienionych plików, 160 linii dodanych, 1 linia usunięta

#### Optymalizacja AI
- `147f8f4` - Uproszczenie tekstu podpowiedzi pamięci kontekstowej
  - Optymalizacja podpowiedzi AI w ContextManager
  - 1 zmieniony plik, 1 linia dodana, 1 linia usunięta

#### Optymalizacja przechowywania Speedy
- `8bda2d3` - Aktualizacja implementacji przechowywania Speedy i kontrolera pamięci
  - Korekta interwału SpeedyPackAutoCompactor
  - Optymalizacja przetwarzania ścieżek SpeedyTimeStorage
  - Ulepszenie kontrolera pamięci MemoryController
  - Aktualizacja UI SpeedyPack.Manager
  - 4 zmienione pliki, 21 linii dodanych, 18 linii usuniętych

#### Ulepszenie zasobnika
- `8972654` - Ulepszenie obsługi lokalizacji okna stanu zasobnika
  - 9 języków lokalizacji zasobnika dodaje wpis Speedy management
  - TrayStatusWindow dodaje element menu Speedy management
  - 11 zmienionych plików, 72 linie dodane

#### Optymalizacja Speedy.Manager
- `6f5db09` - Optymalizacja UI menedżera SpeedyPack i komponentów wewnętrznych
  - Refaktoryzacja interfejsu MainForm
  - Optymalizacja zarządzania pamięcią FreeList
  - Ulepszenie kolejki zapisu WriteQueue
  - Optymalizacja rdzenia SpeedyPack
  - 5 zmienionych plików, 96 linii dodanych, 88 linii usuniętych

#### Ulepszenie systemu przechowywania
- `57f9d5d` - Ulepszenie systemu przechowywania, dodanie automatycznej kompakcji i obsługi niepełnych dat
  - Nowy czasomierz automatycznej kompakcji SpeedyPackAutoCompactor (interwał 30 minut)
  - Ulepszenie menedżera singletona SpeedyPackRegistry
  - Ulepszenia adaptacyjne SpeedyStorage, SpeedyTimeStorage, SpeedyWorkNoteStorage
  - SpeedyPack dodaje zarządzanie wolną przestrzenią FreeList (149 linii)
  - Refaktoryzacja i optymalizacja zapisywacza PackFileWriter
  - Ulepszenie WriteOperation i WriteQueue
  - Rozszerzenie opcji konfiguracyjnych SpeedyPackOptions
  - IncompleteDate dodaje metody porównania
  - Ulepszenie ładowacza wtyczek PluginLoader
  - Aktualizacja przepływu inicjalizacji Program.cs w wersjach Default i Fast
  - Uproszczenie danych konfiguracyjnych DefaultConfigData
  - Uproszczenie KnowledgeNetwork
  - Optymalizacja kontrolerów ChatController, MemoryController
  - Ulepszenie funkcji MainForm w SpeedyPack.Manager
  - 22 zmienione pliki, 639 linii dodanych, 253 linie usunięte

#### Aktualizacja Speedy.Manager
- `b04ed33` - Aktualizacja plików Speedy.Manager

### 2026-05-01

#### Refaktoryzacja architektury: Speedy zastępuje LiteDB
- `6600972` - Zastąpienie LiteDB przechowywaniem Speedy, dodanie systemu wtyczek i projektu Speedy
  - **Nowy projekt SiliconLife.Speedy**: wysokowydajny silnik przechowywania .spk
    - Klasa rdzeniowa SpeedyPack (489 linii): mapowanie katalogów w pamięci + pamięć podręczna wpisów + asynchroniczna kolejka zapisu
    - Klasa konfiguracyjna SpeedyPackOptions: TTL pamięci podręcznej, maksymalna liczba wpisów pamięci podręcznej, tryb tylko do odczytu
    - Interfejs transakcyjny IPackTransaction: obsługa atomowych operacji zapisu
    - Klasa informacji o pliku SpkFileInfo
    - Katalog Internal: DirectoryMap, EntryCache, PackFileReader, PackFileWriter, WriteQueue, WriteOperation, SpeedyTransaction, SpkHeader, PathNormalizer, FreeList
    - Zależność od MessagePack 3.1.4 do serializacji binarnej (kompresja LZ4)
  - **Nowy projekt SiliconLife.Speedy.Manager**: narzędzie zarządzania WPF
    - Architektura MVVM: MainViewModel, DirectoryTreeViewModel, ContentViewerViewModel itp.
    - Warstwa usług: PackService, FileDialogService, RecentFilesService, NotificationService
    - Konwertery: BoolToVisibility, ByteSizeToString, ContentTypeToIcon, NullToCollapsed
    - Widoki: MainWindow, DirectoryTreeView, ContentViewerPanel, MetadataPanel
    - Okna dialogowe: FileInfoDialog, ImportDialog, NewEntryDialog
  - **Migracja przechowywania SiliconLife.Fast**: LiteDB → SpeedyPack
    - Nowy SpeedyStorage (adapter IStorage)
    - Nowy SpeedyTimeStorage (adapter ITimeStorage)
    - Nowy SpeedyWorkNoteStorage (adapter IWorkNoteStorage)
    - Nowy SpeedyPackRegistry (zarządzanie singletonem na poziomie procesu)
    - Nowy SpeedyPackAutoCompactor (czasomierz automatycznej kompakcji)
    - Usunięcie implementacji przechowywania LiteDB (LiteDBStorage, LiteDBTimeStorage, LiteDBWorkNoteStorage, LiteDBLoggerProvider, LiteDBManager, LiteDBModels)
    - Usunięcie kodu związanego z oknem zarządzania LiteDB
  - **System wtyczek**:
    - Nowy interfejs IPlugin (Core/Plugins/IPlugin.cs)
    - Nowy ładowacz wtyczek PluginLoader (Core/Plugins/PluginLoader.cs)
    - Obsługa ładowania DLL wtyczek z katalogu
    - Skanowanie bezpieczeństwa: sprawdzanie zakazanych przestrzeni nazw (System.IO, System.Net, Microsoft.CodeAnalysis itp.)
    - Biała lista zaufanych zestawów (Google.Protobuf, Newtonsoft.Json, MessagePack itp.)
    - Izolowane ładowanie z niestandardowym AssemblyLoadContext
    - ToolManager dodaje metodę ScanAllPluginAssemblies
    - CoreHost integruje ładowacz wtyczek
  - 119 zmienionych plików, 6926 linii dodanych, 3066 linii usuniętych

#### Ulepszenie Istot Krzemowych
- `3aef4c3` - Dodanie stanu aktywności Stopped i ulepszenie obsługi błędów
  - Istoty Krzemowe dodają stan Stopped
  - Ulepszenie obsługi błędów i mechanizmu odzyskiwania

#### Aktualizacja lokalizacji
- `513c65d` - Aktualizacja wszystkich wersji językowych i dokumentacji
  - Nowy komponent MarkdownEditorComponent (625 linii)
  - Nowy komponent DetailsComponent (130 linii)
  - Nowy komponent akordeonu AccordionComponent (285 linii)
  - Aktualizacja kontrolerów BeingController, ChatController, MemoryController, PermissionController
  - Refaktoryzacja widoków BeingView, ChatView, MemoryView, SoulEditorView
  - Usunięcie starego MarkdownEditorView
  - Migracja komponentowa InitController
  - 115 zmienionych plików, 5761 linii dodanych, 2362 linie usunięte

### 2026-04-30

#### Funkcja zasobnika systemowego
- `101b203` - Implementacja okna stanu zasobnika i ApplicationContext
  - Nowe zasoby ikony zasobnika (alpha.png, noWord.png, slc.ico, wordIcon.png)
  - Implementacja okna stanu TrayStatusWindow
  - Obsługa lokalizacji zasobnika w 9 językach (TrayCsCZ, TrayDeDE, TrayEnUS itp.)
  - Abstrakcyjna klasa bazowa TrayLocalizationBase
  - 24 zmienione pliki, 27995 linii dodanych, 1 linia usunięta (w tym pliki zasobów)

#### Komponentowa architektura UI
- `e61cfaa` - Ukończenie komponentowej architektury UI, implementacja 24 komponentów
  - Etap MVP (8): ComponentBase, Div, Span, Button, Input, Form, Select, Label
  - Etap drugi (6): Accordion, Card, Tabs, Table, Modal, Message
  - Etap trzeci (5): Calendar, Tree, Chart, FileUpload, RichText
  - Nowe klasy pomocnicze Js, Behavior, DomUpdate itp.
  - 25 zmienionych plików, 2666 linii dodanych

- `7449e51` - Ulepszenie systemu komponentów i dodanie nowych motywów skórek
  - Ulepszenie komponentów A, Button, Div, Form, Input itp.
  - Dodanie 3 motywów skórek: HighContrast (wysoki kontrast), Light (jasny), Minimal (minimalistyczny)
  - Aktualizacja istniejących skórek (Admin, Chat, Creative, Dev)
  - Migracja komponentowa InitController
  - 32 zmienione pliki, 1466 linii dodanych, 1238 linii usuniętych

- `1ba8636` - Rozpoczęcie migracji komponentowej InitController (w toku)
  - 9 zmienionych plików, 574 linie dodane, 145 linii usuniętych

#### Ujednolicenie systemu przechowywania
- `895dff9` - Ujednolicenie soul.md i state.json do użycia interfejsu IStorage
  - DefaultSiliconBeing używa IStorage do odczytu/zapisu Pliku Duszy i stanu
  - Nowy menedżer plików stanu StateFileManager
  - Refaktoryzacja SoulFileManager do adaptacji IStorage
  - 8 zmienionych plików, 201 linii dodanych, 116 linii usuniętych

#### Ulepszenie zarządzania LiteDB
- `a34bef4` - Dodanie LiteDBManager i ulepszenie lokalizacji zasobnika
  - Nowy wpis LiteDB management w menu zasobnika
  - Aktualizacja lokalizacji zasobnika w 9 językach
  - 10 zmienionych plików, 196 linii dodanych

- `c4a79ca` - Dodanie fabryki lokalizacji świadomej języka do okna zarządzania LiteDB
  - 1 zmieniony plik, 78 linii dodanych

- `5ebc55e` - Konwersja LiteDBAdminLocalization na abstrakcyjną klasę bazową
  - 10 zmienionych plików, 1356 linii dodanych

#### Naprawa systemu konfiguracji
- `2da5256` - Dodanie metody abstrakcyjnej ConfigExists i naprawa duplikatów rekordów konfiguracji LiteDB
  - ConfigDataBase dodaje metodę ConfigExists
  - Implementacja DefaultConfigData wersji Fast sprawdza istnienie konfiguracji LiteDB
  - Naprawa problemu duplikatów kluczy konfiguracji LiteDB
  - 9 zmienionych plików, 210 linii dodanych, 2 linie usunięte

#### Optymalizacja czatu i widoków
- `d3618ec` - Optymalizacja sesji czatu, systemu przechowywania, modelu czasu i klasy bazowej widoków
  - Optymalizacja BroadcastChannel, GroupChatSession, SingleChatSession
  - Nowe metody zapytań w ITimeStorage
  - Synchroniczna aktualizacja FileSystemStorage i LiteDBStorage
  - Refaktoryzacja i optymalizacja ViewBase (wersje Default i Fast)
  - 11 zmienionych plików, 622 linie dodane, 392 linie usunięte

### 2026-04-29

#### Refaktoryzacja architektury: ekstrakcja modułów współdzielonych
- `a102428` - Migracja modułów współdzielonych z SiliconLife.Default do SiliconLife.Common
  - Ekstrakcja 32 implementacji kalendarzy do projektu Common
  - Ekstrakcja klas bazowych lokalizacji i 21 implementacji językowych do projektu Common
  - Ekstrakcja menedżera uprawnień, domyślnej implementacji Istoty Krzemowej do projektu Common
  - Ekstrakcja 23 implementacji wbudowanych narzędzi do projektu Common
  - Ekstrakcja implementacji Playwright WebView do projektu Common
  - Aktualizacja przestrzeni nazw na SiliconLife.Collective
  - 122 zmienione pliki, 586 linii dodanych, 343 linie usunięte

#### Ulepszenie jakości kodu
- `17566fe` - Zastąpienie Console.WriteLine systemem dzienników w projektach Core, Common i Default
  - Aktualizacja 6 plików: ContextManager, AuditLogger, DefaultConfigData itp.
  - Ujednolicenie użycia interfejsu ILogger, poprawa utrzywalności kodu
  - 6 zmienionych plików, 12 linii dodanych, 8 linii usuniętych

#### SiliconLife.Fast — wersja wysokowydajna
- `54a0307` - Dodanie projektu SiliconLife.Fast i ukończenie napraw kompilacji
  - Kompletny punkt wejścia aplikacji Windows Forms
  - Obsługa zasobnika systemowego (NotifyIcon)
  - Migracja wszystkich kontrolerów Web UI (20+)
  - Migracja wszystkich komponentów widoków Web
  - Migracja 4 motywów skórek (Admin, Chat, Creative, Dev)
  - 125 zmienionych plików, 61186 linii dodanych

#### Synchronizacja wielojęzycznej dokumentacji
- `265fde8` - Synchronizacja dokumentacji architektury dwóch wersji do wszystkich języków
  - Aktualizacja architecture.md, changelog.md w 7 językach
  - Aktualizacja contributing.md w 6 językach
  - Aktualizacja getting-started.md, roadmap.md w 7 językach
  - 47 zmienionych plików, 1214 linii dodanych, 38 linii usuniętych

#### System przechowywania LiteDB (wersja Fast)
- `4704862` - Dodanie zależności LiteDB i infrastruktury
  - Nowa klasa zarządzania LiteDBManager
  - Nowe modele danych LiteDBModels
  - 3 zmienione pliki, 252 linie dodane

- `4220036` - Implementacja klas przechowywania LiteDB
  - LiteDBStorage: implementacja interfejsu IStorage
  - LiteDBTimeStorage: implementacja interfejsu ITimeStorage
  - LiteDBWorkNoteStorage: implementacja interfejsu IWorkNoteStorage
  - 3 zmienione pliki, 581 linii dodanych

- `38ebd23` - Migracja konfiguracji i systemu dzienników do LiteDB
  - Adaptacja DefaultConfigData do przechowywania LiteDB
  - Nowy dostawca dzienników LiteDBLoggerProvider
  - 2 zmienione pliki, 203 linie dodane, 67 linii usuniętych

- `e687157` - Migracja Sieci Wiedzy z systemu plików do LiteDB
  - Pełna refaktoryzacja KnowledgeNetwork, użycie LiteDB do przechowywania danych trójkowych
  - 1 zmieniony plik, 231 linii dodanych, 72 linie usunięte

- `4220169` - Integracja przechowywania LiteDB z Program i ProjectManager
  - Inicjalizacja przechowywania LiteDB w Program.cs
  - Adaptacja ProjectManager do przechowywania notatek pracy LiteDB
  - 2 zmienione pliki, 40 linii dodanych, 17 linii usuniętych

- `5f3a709` - Usunięcie przestarzałych implementacji przechowywania systemu plików
  - Usunięcie FileSystemLoggerProvider, FileSystemStorage, FileSystemTimeStorage itp.
  - 6 zmienionych plików, 1518 linii usuniętych

- `e1a4ef2` - docs: add v0.1.0-alpha version identifier to all documentation
  - 127 zmienionych plików, 2297 linii dodanych, 2471 linii usuniętych

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### Refaktoryzacja systemu przechowywania
- `8dd26e3` - Ujednolicenie interfejsu ITimeStorage do użycia IncompleteDate i dodanie warstwowego API zapytań
  - Usunięcie przeciążeń DateTime w interfejsie ITimeStorage, ujednolicenie użycia IncompleteDate
  - IncompleteDate dodaje metodę porównania CompareTo(DateTime) i metodę rozszerzenia Expand()
  - Nowe API zapytań warstwowych GetEarliestTimestamp(), GetLatestTimestamp()
  - Nowe metody HasSummary() i QueryWithLevel(), obsługa zapytań według poziomu czasu
  - Refaktoryzacja algorytmu kompresji Memory.cs, użycie nowego API zapytań warstwowych dla poprawy wydajności
  - Pełna implementacja nowych metod interfejsu w FileSystemTimeStorage.cs
  - Synchroniczna aktualizacja wszystkich wywołań: ChatSystem, ChatSession, BroadcastChannel, AuditLogger, TokenUsageRecord itp.
  - Aktualizacja systemu narzędzi: HelpTool, LogTool, TokenAuditTool adaptowane do nowego interfejsu
  - Aktualizacja kontrolerów Web: AuditController, ChatController, ChatHistoryController adaptowane do nowego interfejsu
  - 41 zmienionych plików, 1820 linii dodanych, 903 linie usunięte

### 2026-04-27

#### Ulepszenie systemu dokumentacji pomocy
- `9989d79` - Aktualizacja lokalizacji, systemu pomocy i widoków Web
  - Nowy interfejs dokumentacji pomocy fabryki klienta AI IAIClientFactoryHelp.cs
  - Ukończenie tłumaczenia dokumentacji pomocy na 9 języków
  - HelpTopics.cs dodaje 40 definicji tematów pomocy
  - Pełna aktualizacja widoków Web: InitController, AuditView, ConfigView, KnowledgeView, LogView itp.
  - Ulepszenie systemu lokalizacji: wszystkie wersje językowe dodają nowe klucze lokalizacji
  - Aktualizacja fabryki klientów AI: ulepszenia DashScopeClientFactory, OllamaClientFactory
  - 30 zmienionych plików, 10086 linii dodanych, 15 linii usuniętych

#### Nowa zawartość dokumentacji pomocy
- `e7afe94` - Dodanie dokumentacji pomocy Pliku Duszy i dziennika audytu
  - Nowa dokumentacja pomocy zarządzania Plikiem Duszy
  - Nowa dokumentacja pomocy dziennika audytu
  - HelpTopics.cs dodaje definicje tematów
  - Znaczna refaktoryzacja HelpView.cs, ulepszenie logiki renderowania dokumentacji
  - Refaktoryzacja PermissionView.cs, ulepszenie interfejsu zarządzania uprawnieniami
  - Ulepszenie modułów rdzeniowych: SiliconBeingManager, TaskSystem, ToolManager
  - Refaktoryzacja TaskTool.cs, ulepszenie funkcji zarządzania zadaniami
  - Pełna aktualizacja widoków Web: synchroniczna aktualizacja wszystkich komponentów widoków
  - Uproszczenie HelpController.cs, optymalizacja logiki kontrolera
  - 30 zmienionych plików, 7100 linii dodanych, 897 linii usuniętych

### 2026-04-26

#### System dokumentacji pomocy
- `07895d7` - Ulepszenie systemu dokumentacji pomocy, dodanie 3 dokumentów i ukończenie tłumaczenia na 9 języków
  - Nowe przewodniki: system pamięci, instalacja i konfiguracja Ollama, korzystanie z platformy Alibaba Cloud DashScope
  - Ukończenie tłumaczenia wszystkich 10 dokumentów pomocy na 9 języków
  - Uproszczenie logiki renderowania HelpView
  - 18 zmienionych plików, 14418 linii dodanych, 1364 linie usunięte

#### Lokalizacja niemiecka
- `0cfd8a1` - Dodanie pełnej obsługi lokalizacji niemieckiej (de-DE)
  - Kompletny plik lokalizacji niemieckiej
  - Nowa obsługa chińskiego kalendarza historycznego w niemieckim
  - Nowe tłumaczenie dokumentacji pomocy na niemiecki
  - Pełna synchronizacja dokumentacji we wszystkich 9 językach
  - 135 zmienionych plików, 26186 linii dodanych, 14371 linii usuniętych

#### Synchronizacja dokumentacji
- `3aada7d` - Synchronizacja dokumentacji tradycyjnego chińskiego (zh-HK) z uproszczonym chińskim
  - 3 zmienione pliki, 519 linii dodanych, 422 linie usunięte
- `2f6abff` - Dodanie lokalizacji nazw wyświetlanych narzędzia pomocy dla wszystkich języków
  - 7 zmienionych plików, 47 linii dodanych, 7 linii usuniętych

#### Refaktoryzacja systemu wiedzy
- `60944fe` - Ujednolicenie przestrzeni nazw do SiliconLife.Collective
  - 8 zmienionych plików, 5 linii dodanych, 8 linii usuniętych
- `69c51c5` - Dodanie systemu dokumentacji pomocy i tłumaczenie komentarzy kodu na angielski
  - 29 zmienionych plików, 3385 linii dodanych, 22 linie usunięte

### 2026-04-25

#### Automatyzacja przeglądarki WebView
- `41757c3` - Implementacja wieloplatformowej automatyzacji przeglądarki WebView opartej na Playwright
  - 6 zmienionych plików, 1152 linie dodane

#### Aktualizacja dokumentacji
- `0ff797b` - Dodanie dokumentacji KnowledgeTool i WorkNoteTool (7 języków)
  - 28 zmienionych plików, 4983 linie dodane
- `ad77415` - Aktualizacja wszystkich plików changelog, dodanie rekordów historii Git z 2026-04-25
  - 7 zmienionych plików, 168 linii dodanych

#### Zarządzanie obszarem roboczym projektu
- `785c551` - Implementacja zarządzania obszarem roboczym projektu, obejmująca notatki pracy i system zadań
  - Nowy system zarządzania obszarem roboczym projektu
  - Funkcja notatek pracy do śledzenia postępu projektu
  - Integracja systemu zarządzania zadaniami
  - 29 zmienionych plików, 4256 linii dodanych, 36 linii usuniętych

#### Lokalizacja czeska
- `b4bbf39` - Dodanie pełnej lokalizacji czeskiej (cs-CZ) i aktualizacja dokumentacji wszystkich języków
  - 116 zmienionych plików, 4933 linie dodane, 222 linie usunięte
- `faf078f` - Naprawa błędu kompilacji lokalizacji czeskiej
  - 3 zmienione pliki, 910 linii dodanych, 1 linia usunięta

#### Ulepszenie systemu wiedzy
- `20adaac` - Dodanie KnowledgeTool i obsługa pełnej lokalizacji
  - 34 zmienione pliki, 2331 linii dodanych, 56 linii usuniętych

### 2026-04-24

#### Ulepszenie systemu zarządzania pamięcią
- `c7b2ecc` - Ulepszenie funkcji zarządzania pamięcią, dodanie zaawansowanego filtrowania, statystyk i widoku szczegółów
  - Nowa funkcja zaawansowanego filtrowania pamięci
  - Implementacja funkcji statystyk pamięci
  - Dodanie strony widoku szczegółów pamięci
  - Obsługa lokalizacji wielojęzycznej (6 języków)
  - 13 zmienionych plików, 840 linii dodanych, 86 linii usuniętych

#### Rozszerzenie systemu uprawnień
- `4489ad6` - Dodanie usługi pogodowej wttr.in do białej listy sieci
  - Pełna synchroniczna aktualizacja wielojęzycznej dokumentacji (6 języków)
  - 14 zmienionych plików, 417 linii dodanych, 1 linia usunięta

#### Naprawa interfejsu Web
- `d9d72e9` - Naprawa problemu priorytetu CSS modalnego szczegółów notatek pracy
  - 19 zmienionych plików, 1744 linie dodane, 6 linii usuniętych

#### Optymalizacja historii czatu
- `0df599c` - Naprawa problemu wyników narzędzi renderowanych jako niezależne wiadomości czatu
  - 1 zmieniony plik, 222 linie dodane, 21 linii usuniętych
- `057b09d` - Optymalizacja wyświetlania szczegółów historii czatu, ulepszenie renderowania wywołań narzędzi
  - 3 zmienione pliki, 389 linii dodanych, 68 linii usuniętych

#### Historia wykonania czasomierzy
- `fa3f06f` - Dodanie funkcji historii wykonania czasomierzy z widokiem szczegółów
  - 8 zmienionych plików, 937 linii dodanych, 10 linii usuniętych
- `d824835` - Dodanie kluczy lokalizacji historii wykonania czasomierzy (wszystkie języki)
  - 7 zmienionych plików, 88 linii dodanych

#### Ulepszenie lokalizacji
- `c13cb17` - Rejestracja wariantu języka hiszpańskiego
  - 1 zmieniony plik, 4 linie dodane
- `9c44f34` - Dodanie wielojęzycznej obsługi lokalizacji chińskiego kalendarza historycznego
  - 16 zmienionych plików, 6049 linii dodanych, 1 linia usunięta

#### Ulepszenie funkcji rdzeniowych
- `1e7c7b2` - Ulepszenie kompresji pamięci i śledzenia wykonania narzędzi
  - 4 zmienione pliki, 338 linii dodanych, 86 linii usuniętych

### 2026-04-23

#### Lokalizacja narzędzi
- `192fc6e` - Dodanie brakującej lokalizacji nazw narzędzi dla 5 narzędzi
  - 6 zmienionych plików, 30 linii dodanych

#### Aktualizacja dokumentacji
- `882c08f` - Aktualizacja wszystkich plików changelog, dodanie pełnej historii Git i usunięcie fałszywych numerów wersji
  - 45 zmienionych plików, 8815 linii dodanych, 1611 linii usuniętych

#### Ulepszenie strony czatu
- `65c157b` - Dodanie wskaźnika ładowania i automatycznego wyboru sesji kuratora na stronie czatu
  - 10 zmienionych plików, 211 linii dodanych, 7 linii usuniętych

#### Funkcja historii czatu
- `e483348` - Implementacja funkcji podglądu historii czatu Istot Krzemowych
  - Nowy ChatHistoryController
  - Utworzenie ChatHistoryViewModel
  - Implementacja stron ChatHistoryListView i ChatHistoryDetailView
  - Dodanie kluczy lokalizacji historii czatu (5 języków)
  - 12 zmienionych plików, 1178 linii dodanych

#### Ulepszenie sterowania strumieniem AI
- `30a2d4e` - Ulepszenie anulowania strumienia AI, integracji IM i inicjalizacji głównego hosta
  - 11 zmienionych plików, 387 linii dodanych, 12 linii usuniętych

#### Kolejka wiadomości czatu
- `db48c51` - Dodanie kolejki wiadomości czatu, metadanych plików i obsługi anulowania strumienia
  - 4 zmienione pliki, 357 linii dodanych

#### Obsługa przesyłania plików
- `28fb344` - Implementacja okna dialogowego źródła plików i obsługi przesyłania plików
  - 3 zmienione pliki, 1100 linii dodanych, 2 linie usunięte
- `1d3e2cc` - Dodanie ciągów lokalizacji okna dialogowego źródła plików (6 języków)
  - 6 zmienionych plików, 30 linii dodanych

#### Aktualizacja dokumentacji
- `8111e92` - Dodanie linku Wiki w sekcji repozytorium README
  - 1 zmieniony plik, 3 linie dodane, 1 linia usunięta

### 2026-04-22

#### Lokalizacja dokumentacji
- `66c11eb` - Tłumaczenie chińskich komentarzy na angielski i aktualizacja wszystkich changelogów
  - 11 zmienionych plików, 373 linie dodane, 163 linie usunięte

#### Ulepszenie wiadomości SSE
- `b574b2b` - Dodanie senderName dla wiadomości historycznych do identyfikacji AI
  - 1 zmieniony plik, 9 linii dodanych

#### Funkcja czatu
- `601fc14` - Dodanie operacji mark_read do oznaczania końca sesji
  - 7 zmienionych plików, 196 linii dodanych, 36 linii usuniętych

#### Optymalizacja systemu narzędzi
- `7a03a19` - Ulepszenie elastyczności zapytań dialogowych LogTool
  - 1 zmieniony plik, 57 linii dodanych, 24 linie usunięte

#### Ulepszenie lokalizacji
- `0a8d750` - Dodanie ogólnej podpowiedzi systemowej dla proaktywnych zachowań Istot Krzemowych
  - 8 zmienionych plików, 460 linii dodanych, 48 linii usuniętych

#### Refaktoryzacja systemu dzienników
- `2b771f3` - Dekorelacja LogController od we/wy plików, dodanie API odczytu dzienników
  - 4 zmienione pliki, 172 linie dodane, 137 linii usuniętych
- `12da302` - Dodanie filtru Istot Krzemowych do widoku dzienników
  - 9 zmienionych plików, 147 linii dodanych, 10 linii usuniętych
- `8f6cb1e` - Dodanie parametru beingId do interfejsu ILogger, implementacja separacji dzienników system/istota krzemowa
  - 47 zmienionych plików, 524 linie dodane, 490 linii usuniętych

#### Ulepszenie systemu uprawnień
- `4c747ad` - Refaktoryzacja PermissionTool, ExecuteCodeTool, dodanie API EvaluatePermission
  - 18 zmienionych plików, 680 linii dodanych, 492 linie usunięte

#### Poprawki błędów
- `1c96e99` - Naprawa niepowodzenia wyszukiwania search_files i search_content w katalogu głównym
  - 1 zmieniony plik, 98 linii dodanych, 41 linii usuniętych

#### Integracja narzędzi
- `135710d` - Usunięcie SearchTool, migracja wyszukiwania lokalnego do DiskTool
  - 2 zmienione pliki, 185 linii dodanych, 365 linii usuniętych

#### Rozszerzenie systemu narzędzi
- `70ce7fb` - Implementacja DatabaseTool do ustrukturyzowanych zapytań bazodanowych
  - 1 zmieniony plik, 382 linie dodane
- `be29a09` - Implementacja LogTool do zapytań o historię operacji i dialogów
  - 1 zmieniony plik, 298 linii dodanych
- `4ea7702` - Implementacja PermissionTool do dynamicznego zarządzania uprawnieniami
  - 1 zmieniony plik, 457 linii dodanych
- `1384ff4` - Implementacja ExecuteCodeTool do wykonywania kodu w wielu językach
  - 1 zmieniony plik, 477 linii dodanych
- `82d1e11` - Implementacja SearchTool do wyszukiwania informacji
  - 1 zmieniony plik, 363 linie dodane

#### Optymalizacja interfejsu Web
- `0675c45` - Optymalizacja podświetlania bloków kodu markdown w oknie podglądu
  - 1 zmieniony plik, 4 linie dodane, 23 linie usunięte
- `702b3f3` - Ulepszenie widoku zadań, dodanie odznak stanu i wyświetlania metadanych
  - 8 zmienionych plików, 221 linii dodanych, 9 linii usuniętych
- `6ed9a79` - Ulepszenie przechowywania wiadomości czatu i renderowania widoku
  - 8 zmienionych plików, 140 linii dodanych, 29 linii usuniętych

### 2026-04-21

#### Poprawki błędów
- `c6b518b` - Naprawa przekazywania wiadomości czasomierzy i przechowywania wiadomości czatu
  - 3 zmienione pliki, 297 linii dodanych, 124 linie usunięte

#### Zarządzanie konfiguracją
- `4305769` - Dodanie .gitattributes do zarządzania końcami linii
  - 1 zmieniony plik, 32 linie dodane

#### Ulepszenia interfejsu Web
- `188c6f8` - Rejestracja trasy API listy zadań i dodanie wyświetlania pustego stanu
  - 2 zmienione pliki, 35 linii dodanych, 2 linie usunięte
- `634e8ca` - Dodanie linku powrotu do listy na stronie uprawnień
  - 1 zmieniony plik, 16 linii dodanych
- `6ba591d` - Dodanie niezależnego edytora konfiguracji AI dla Istot Krzemowych
  - 11 zmienionych plików, 842 linie dodane, 18 linii usuniętych
- `0a826f5` - Dodanie podpowiedzi o pomyślnym zapisie w edytorze kodu
  - 1 zmieniony plik, 9 linii dodanych, 2 linie usunięte
- `2940373` - Ulepszenie interfejsu Web, dodanie podpowiedzi kodu i ulepszenia UI
  - 11 zmienionych plików, 1054 linie dodane, 75 linii usuniętych

#### Naprawa systemu uprawnień
- `592c7ab` - Naprawa instancjonowania wywołania zwrotnego i kolejności rejestracji
  - 2 zmienione pliki, 38 linii dodanych, 7 linii usuniętych

#### Ulepszenie bezpieczeństwa
- `833ead2` - Dodanie weryfikacji referencji zestawów dla kompilacji dynamicznej
  - 4 zmienione pliki, 135 linii dodanych, 8 linii usuniętych

#### Ulepszenie systemu uprawnień
- `5879621` - Dodanie weryfikacji prekompilacji wywołań zwrotnych uprawnień i ulepszonej obsługi błędów
  - 21 zmienionych plików, 617 linii dodanych, 26 linii usuniętych

#### Aktualizacja dokumentacji
- `4dbf659` - Aktualizacja changelog do v0.5.1, zastąpienie zastępczych URL GitHub, dodanie mirrora Gitee, lokalizacja nazwy Bilibili według języka, aktualizacja email
  - 32 zmienione pliki, 489 linii dodanych, 180 linii usuniętych

#### Konfiguracja i punkt wejścia
- `0fc1693` - Aktualizacja punktu wejścia programu i konfiguracji projektu
  - 2 zmienione pliki, 7 linii dodanych

#### Refaktoryzacja systemu uprawnień
- `ea9179a` - Ulepszenie implementacji systemu uprawnień
  - 5 zmienionych plików, 358 linii dodanych, 152 linie usunięte

#### Poprawki błędów
- `928a96d` - Naprawa implementacji obliczeń kalendarza
  - 4 zmienione pliki, 12 linii dodanych, 12 linii usuniętych

#### AI i kalendarz
- `646813e` - Ulepszenie implementacji fabryki klientów AI
  - 2 zmienione pliki, 21 linii dodanych, 20 linii usuniętych

#### Lokalizacja
- `7940d9c` - Dodanie obsługi lokalizacji koreańskiej
  - 7 zmienionych plików, 2424 linie dodane, 10 linii usuniętych
- `4ff98ad` - Refaktoryzacja dokumentacji, obsługa wielojęzyczna
  - 81 zmienionych plików, 23818 linii dodanych, 1886 linii usuniętych

### 2026-04-20

#### Udoskonalenie funkcji rdzeniowych
- `28905b5` - Pełna obsługa wielojęzyczna, fabryka klientów AI, system uprawnień i ustawienia lokalizacji
  - System dzienników z menedżerem, wpisami i różnymi poziomami dzienników
  - System audytu tokenów do odpytywania i śledzenia wykorzystania tokenów
  - Fabryka klientów AI z automatycznym odkrywaniem różnych platform AI
  - System wywołań zwrotnych uprawnień z własnym przechowywaniem
  - Implementacja loggera konsolowego
  - Obsługa wielojęzyczna angielska i chińska uproszczona
  - WebUI messenger z WebSocket do czatu w czasie rzeczywistym
  - Ulepszenie domyślnej Istoty Krzemowej z lokalizacją
  - 39 zmienionych plików, 4670 linii dodanych, 175 linii usuniętych

### 2026-04-19

#### Czasomierze i kalendarz
- `c933fd8` - Aktualizacja lokalizacji, systemu czasomierzy, widoków Web i dodanie narzędzi
  - Lepszy menedżer lokalizacji
  - System harmonogramowania zadań czasowych
  - Konfiguracja AI i zarządzanie kontekstem
  - Narzędzie kalendarza obsługujące 32 typy kalendarzy
  - Kontroler Web dla API kalendarza
  - Narzędzie zarządzania zadaniami
  - 46 zmienionych plików, 4018 linii dodanych, 975 linii usuniętych

**Ulepszenia architektury**
- Przeprojektowanie architektury widoków Web dla lepszego wsparcia skórek
- Ulepszenie systemu zarządzania istotami z lepszą obsługą stanu

### 2026-04-18

- `9f585e1` - Aktualizacja lokalizacji, systemu czasomierzy, widoków Web i dodanie narzędzi
  - Ulepszenia czasomierzy i harmonogramowania
  - Lepsze widoki Web z ulepszonymi komponentami UI
  - Więcej implementacji narzędzi
  - 57 zmienionych plików, 3328 linii dodanych, 389 linii usuniętych

### 2026-04-17

- `9b71fcd` - Aktualizacja modułów rdzeniowych, dodanie dokumentacji zh-HK, kanału transmisyjnego, narzędzia konfiguracji i widoku Web audytu
  - Kanał transmisyjny do czatu wielu Istot Krzemowych razem
  - System narzędzia konfiguracji
  - Widok Web audytu
  - Dokumentacja tradycyjnego chińskiego
  - 42 zmienione pliki, 3533 linie dodane, 268 linii usuniętych

### 2026-04-16

- `5040f05` - Aktualizacja modułów rdzeniowych i domyślnych
  - Optymalizacja modułów i poprawki błędów
  - Aktualizacje implementacji i ulepszenia
  - 58 zmienionych plików, 9916 linii dodanych, 111 linii usuniętych

### 2026-04-15

- `3efab5f` - Aktualizacja wielu modułów: AI, Chat, IM, Tools, Web, Localization, Storage
  - Ulepszenia klienta AI
  - Ulepszenia systemu czatu
  - Aktualizacje dostawcy komunikatora
  - Optymalizacja systemu narzędzi
  - Ulepszenia infrastruktury Web
  - Optymalizacja lokalizacji
  - Aktualizacja systemu przechowywania
  - 33 zmienione pliki, 788 linii dodanych, 232 linie usunięte

### 2026-04-14

- `4241a2f` - Podstawowe ukończenie funkcji czatu, optymalizacja przesyłania UI
  - Ukończenie funkcji systemu czatu
  - Optymalizacja UI przesyłania plików
  - 16 zmienionych plików, 1234 linie dodane, 102 linie usunięte

### 2026-04-13

- `c498c31` - Aktualizacja kodu
  - Ogólne ulepszenia i optymalizacje kodu
  - 32 zmienione pliki, 1045 linii dodanych, 546 linii usuniętych

### 2026-04-12

#### Dokumentacja i lokalizacja
- `2161002` - Refaktoryzacja dokumentacji i ulepszenie lokalizacji
  - 17 zmienionych plików, 982 linie dodane, 92 linie usunięte
- `03d94e4` - Ulepszenie systemu konfiguracji i lokalizacji
  - 25 zmienionych plików, 1378 linii dodanych, 154 linie usunięte
- `9976a35` - Dodanie strony o i lokalizacji
  - 14 zmienionych plików, 699 linii dodanych, 44 linie usunięte

#### Czat i widoki Web
- `0c8ccfc` - Ulepszenie systemu czatu, lokalizacji i widoków Web
  - 13 zmienionych plików, 402 linie dodane, 56 linii usuniętych
- `a8f1342` - Przeprojektowanie warstwy komunikacji Web, przejście z WebSocket na SSE
  - 27 zmienionych plików, 793 linie dodane, 935 linii usuniętych

### 2026-04-11

#### System dzienników
- `e8fe259` - Dodanie systemu dzienników i optymalizacja kodu
  - 37 zmienionych plików, 624 linie dodane, 91 linii usuniętych
- `f01c519` - Dodanie systemu dzienników, aktualizacja interfejsów AI i widoków Web
  - 31 zmienionych plików, 1758 linii dodanych, 63 linie usunięte

### 2026-04-10

- `4962924` - Ulepszenie obsługi WebSocket, widoku czatu i interakcji komunikatora
  - Ulepszenia menedżera kontekstu
  - Ulepszenia systemu czatu
  - Aktualizacja interfejsu dostawcy komunikatora
  - Przeprojektowanie dostawcy WebUI
  - Aktualizacja kreatora JavaScript i routera
  - Optymalizacja widoku czatu
  - Ulepszenia obsługi WebSocket
  - 9 zmienionych plików, 365 linii dodanych, 134 linie usunięte

### 2026-04-09

- `f9302bf` - Ulepszenie interfejsu dostawcy komunikatora, systemu czatu i interakcji Web UI
  - Rozszerzenie interfejsu dostawcy komunikatora
  - Ulepszenia wiadomości i systemu czatu
  - Optymalizacja menedżera kontekstu
  - Ulepszenie domyślnej Istoty Krzemowej
  - Ulepszenia widoku czatu Web UI
  - Aktualizacja obsługi WebSocket
  - 10 zmienionych plików, 427 linii dodanych, 93 linie usunięte

### 2026-04-07

- `6831ee8` - Przeprojektowanie widoków Web i kreatora JavaScript
  - Pełne przeprojektowanie kontrolerów Web
  - Całkowite przepisanie kreatora JavaScript
  - Aktualizacja wszystkich komponentów widoków
  - Ulepszenie systemu skórek
  - Podniesienie architektury klasy bazowej widoków
  - 23 zmienione pliki, 2004 linie dodane, 1983 linie usunięte

### 2026-04-05

- `41e97fb` - Aktualizacja wielu modułów rdzeniowych i kontrolerów Web
  - Ulepszenia menedżera kontekstu
  - System czatu i zarządzanie sesjami
  - Przeprojektowanie lokalizatora usług
  - Aktualizacja klasy bazowej i menedżera Istot Krzemowych
  - Pełna aktualizacja kontrolerów Web (17 kontrolerów)
  - Ulepszenie fabryki domyślnej Istoty Krzemowej
  - 31 zmienionych plików, 681 linii dodanych, 326 linii usuniętych
- `67988d4` - Ulepszenie modułów Web UI, dodanie widoku wykonawców, czyszczenie widoków i modułów rdzeniowych
  - 61 zmienionych plików, 3148 linii dodanych, 3726 linii usuniętych

### 2026-04-04

- `b58bb1c` - Dodanie kontrolera inicjalizacji i przeprojektowanie modułów Web
  - Kontroler inicjalizacji
  - Przeprojektowanie modułu konfiguracji
  - Aktualizacja modułu lokalizacji
  - Ulepszenie systemu skórek
  - Ulepszenie routera
  - 29 zmienionych plików, 1269 linii dodanych, 289 linii usuniętych
- `f03ac0b` - Dodanie modułu Web UI, ulepszenie funkcji komunikatora
  - 60 zmienionych plików, 8481 linii dodanych, 165 linii usuniętych

### 2026-04-03

- `192e57b` - Aktualizacja struktury projektu i komponentów runtime rdzeniowych
  - 22 zmienione pliki, 446 linii dodanych, 179 linii usuniętych
- `59faec8` - Aktualizacja implementacji rdzeniowej i domyślnej
  - 25 zmienionych plików, 3056 linii dodanych, 18 linii usuniętych
- `d488485` - Dodanie kompilacji dynamicznej i modułu narzędzi kuratora
  - 19 zmienionych plików, 1727 linii dodanych, 11 linii usuniętych
- `753d1d9` - Dodanie modułu bezpieczeństwa, aktualizacja wykonawców, dostawcy komunikatora, lokalizacji i narzędzi
  - 29 zmienionych plików, 2352 linie dodane, 93 linie usunięte
- `a378697` - Ukończenie etapu 5 — system narzędzi + wykonawcy
  - 41 zmienionych plików, 2651 linii dodanych, 363 linie usunięte

### 2026-04-02

- `e6ad94b` - Naprawa niepowodzenia ładowania historii czatu po usunięciu pliku konfiguracji podczas testowania
  - 4 zmienione pliki, 49 linii dodanych, 45 linii usuniętych
- `daa56f5` - Ukończenie etapu 4: utrwalona pamięć (system czatu + kanał komunikatora)
  - 29 zmienionych plików, 2051 linii dodanych, 538 linii usuniętych

### 2026-04-01

- `bbe2dbb` - Naprawa ładowania konfiguracji i routingu wiadomości usługi czatu
  - 27 zmienionych plików, 1633 linie dodane, 147 linii usuniętych
- `2fa6305` - Implementacja etapu 2: szkielet pętli głównej i system obiektów Tick
  - 9 zmienionych plików, 594 linie dodane, 41 linii usuniętych
- `32b99a1` - Implementacja etapu 1 — podstawowa funkcja czatu
  - 19 zmienionych plików, 1185 linii dodanych
- `358e368` - Początkowe zatwierdzenie: dokumentacja projektu i licencja
  - 10 zmienionych plików, 1873 linie dodane
