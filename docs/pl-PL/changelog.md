# Dziennik zmian

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md) | [Polski](../pl-PL/changelog.md)

Wszystkie istotne zmiany w tym projekcie będą dokumentowane w tym pliku.

Format oparty na [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
ten projekt jest zgodny z [Semantycznym Wersjonowaniem](https://semver.org/spec/v2.0.0.html).

---

## O tym dzienniku zmian

### Dwie wersje projektu

Ten projekt oferuje dwie wersje implementacji:

- **SiliconLife.Default**: domyślna implementacja, głównie do weryfikacji wykonalności architektury. Aplikacja konsolowa, przechowywanie JSON w systemie plików.
- **SiliconLife.Fast**: główna wersja produkcyjna. Aplikacja okienkowa Windows, pamięć SpeedyPack + asynchroniczna trwałość, głęboko zoptymalizowana pod kątem wydajności.

Obie wersje współdzielą te same interfejsy i funkcjonalność, różniąc się jedynie implementacją przechowywania i trybem działania. SiliconLife.Default służy jako referencyjna implementacja weryfikacji architektury, a SiliconLife.Fast jako główna wersja do środowisk produkcyjnych.

### Początki projektu

- Ten projekt powstał 20 marca 2026 roku.
- Przed tym projektem istniało demo weryfikacyjne, które nie powiodło się z powodu nieracjonalnej architektury, uniemożliwiając integrację z wieloma platformami AI.

### Używane narzędzia AI IDE

#### Kiro (Amazon AWS)
- Projekt był początkowo utrzymywany przez Kiro i uruchomiony w trybie Spec.
- Kiro to środowisko programistyczne agentic AI zbudowane przez Amazon AWS.
- Oparte na Code OSS (VS Code), obsługujące ustawienia VS Code i wtyczki kompatybilne z Open VSX.
- Posiada napędzany specyfikacjami przepływ pracy programistycznej do ustrukturyzowanego kodowania AI.

#### Comate AI IDE / 文心快码 (Baidu)
- Sporadycznie używane do prac redakcyjnych i dokumentacyjnych.
- Comate AI IDE to natywne dla AI środowisko programistyczne wydane przez Baidu Wenxin 23 czerwca 2025 roku.
- Pierwsze w branży wielomodalne, wieloagentowe współpracujące AI IDE.
- Funkcje obejmują konwersję projektu na kod i pełnoprzepływowe wspierane przez AI kodowanie.
- Napędzane przez model Baidu Wenxin 4.0 X1 Turbo.

#### Trae (ByteDance)
- Używane od października 2025 do kwietnia 2026.
- AI IDE, obsługujące inteligentne generowanie kodu i zarządzanie projektami.

#### Qoder (Alibaba)
- Od 18 kwietnia 2026 do utrzymania projektu.
- Platforma kodowania AI, obsługująca analizę kodu, generowanie dokumentacji i wieloagentową współpracę.

#### CatPaw (Meituan)
- Od 6 maja 2026 używane mieszanie z Qoder.
- Oparte na autorskich modelach serii LongCat firmy Meituan, z silną zdolnością pełnego refaktoringu architektury kodu.

### Dokumentacja wymagań

- Dokumentacja wymagań tego projektu nie jest publicznie dostępna.
- Wymagania zostały wielokrotnie zweryfikowane przez ponad 12 międzynarodowych platform AI i dużych serii modeli, generując ponad 2000 linii, niemal niezrozumiałą dla człowieka dokumentację wymagań opartą na historiach użytkowników.

---

## [Nieopublikowane]

### 2026-05-12

#### Widok Web systemu zadań
- `0891b3c` - Dodano widok szczegółów wykonania zadania i widok historii
  - Nowy TaskExecutionDetailView widok szczegółów wykonania zadania
  - Nowy TaskExecutionHistoryView widok historii wykonania zadania
  - TaskController dodano interfejsy zapytań o szczegóły wykonania i historię
  - Nowy TaskViewModel model widoku zadania
  - Rozszerzenie TaskCenter centrum zadań
  - Aktualizacja TaskSystem systemu zadań
  - 9 języków lokalizacji dodano klucze związane z zadaniami
  - 26 zmienionych plików, 803 dodane linie, 55 usuniętych linii

### 2026-05-11

#### Refaktoring architektury komponentów Web
- `5e687ad` - Migracja renderowania komponentów z ciągów na H-tree
  - Metody renderowania ComponentBase zmigrowane z trybu ciągów do struktury H-tree
  - Wszystkie 28 komponentów dostosowane do nowej architektury renderowania
  - Znaczna refaktoryzacja SelectComponent (889 linii ulepszeń)
  - Synchroniczna aktualizacja kontrolerów i widoków
  - 33 zmienione pliki, 667 dodanych linii, 435 usuniętych linii

- `bfd332d` - Migracja stylów z ciągów na style inline CssBuilder
  - Nowy CssBuilder kreator stylów
  - System stylów ComponentBase zmigrowany z ciągów na ustrukturyzowany CssBuilder
  - Znaczne rozszerzenie LoadingComponent (103 dodane linie)
  - Migracja stylów kontrolerów ConfigController, LogController, MemoryController
  - Migracja stylów widoków ChatView, ConfigView, LogView, MemoryView
  - 37 zmienionych plików, 351 dodanych linii, 157 usuniętych linii

#### Optymalizacja systemu przechowywania
- `d67a7ee` - Optymalizacja QueryLatest dla zapytań o duże zbiory danych
  - Optymalizacja wydajności metody SpeedyTimeStorage QueryLatest
  - Rozszerzenie SpeedyLoggerProvider dostawcy logowania
  - 2 zmienione pliki, 44 dodane linie, 5 usuniętych linii

#### Refaktoring systemu kalendarzowego
- `9629f88` - Ekstrakcja TimerExecution i rozszerzenie widoku Web czasomierza
  - Ekstrakcja logiki TimerExecution z TimerSystem (175 usuniętych linii)
  - Znaczne rozszerzenie SelectComponent (427 linii ulepszeń)
  - Rozszerzenie TimerController i widoków czasomierza
  - Aktualizacja ContextManager menedżera kontekstu
  - 12 zmienionych plików, 458 dodanych linii, 267 usuniętych linii

#### Lokalizacja
- `5d8ca79` - Dodano klucze lokalizacji LogsLoading
  - 9 języków dodano klucze LogsLoading
  - Dodano definicję w DefaultLocalizationBase klasie bazowej
  - 11 zmienionych plików, 15 dodanych linii

### 2026-05-10

#### Refaktoring systemu zadań
- `54394f6` - Połączenie systemu zadań z cyklem historii czatu
  - Znaczne uproszczenie ProjectTaskSystem systemu zadań projektu (411 linii refaktoringu)
  - Uproszczenie TaskSystem systemu zadań (254 linie refaktoringu)
  - Refaktoring TaskCenter centrum zadań (188 linii ulepszeń)
  - Optymalizacja ContextManager menedżera kontekstu (347 linii refaktoringu)
  - Rozszerzenie DefaultSiliconBeing istoty krzemowej
  - Integracja zadań z TimerSystem systemem czasomierza
  - Aktualizacja interfejsu IWorkNoteStorage
  - Adaptacja SpeedyWorkNoteStorage i FileSystemWorkNoteStorage
  - 16 zmienionych plików, 648 dodanych linii, 897 usuniętych linii

### 2026-05-09

#### Rozszerzenie interfejsu Web
- `bc50dd7` - Ulepszenie widoku czatu i dodanie funkcji audytu
  - Nowy AuditController kontroler audytu (261 linii)
  - Nowy AuditView widok audytu (379 linii)
  - Nowy AuditViewModel model widoku audytu
  - Znaczne ulepszenie ChatView widoku czatu (171 linii rozszerzeń)
  - Aktualizacja ChatController kontrolera czatu
  - Rozszerzenie MarkdownEditorComponent komponentu
  - Ulepszenie InitController kontrolera inicjalizacji
  - Nowe funkcje ChatSystem systemu czatu
  - 14 zmienionych plików, 1030 dodanych linii, 112 usuniętych linii

- `c9babce` - Ulepszenie renderowania wywołań narzędzi w widoku czatu
  - Rozszerzenie renderowania bloków wywołań narzędzi w ChatView
  - 1 zmieniony plik, 54 dodane linie, 11 usuniętych linii

#### System scenariuszy narzędzi AI
- `ff2eddd` - Implementacja systemu filtrowania scenariuszy narzędzi
  - Nowy ToolScenarioAttribute atrybut scenariusza narzędzia (36 linii)
  - Nowy ChatOnlyAttribute atrybut scenariusza tylko czat (19 linii)
  - Nowa funkcja filtrowania scenariuszy w ToolManager menedżerze narzędzi (40 linii)
  - Adaptacja ContextManager menedżera kontekstu do filtrowania scenariuszy
  - 4 zmienione pliki, 115 dodanych linii, 30 usuniętych linii

- `5709a33` - Dodanie atrybutów scenariusza do klas narzędzi
  - 24 klasy narzędzi z adnotacjami atrybutów ToolScenario
  - Obejmujące narzędzia kalendarza, czatu, konfiguracji, kuratela, bazy danych, dysku, kompilacji dynamicznej itp.
  - 24 zmienione pliki, 46 dodanych linii, 20 usuniętych linii

#### Refaktoring systemu zadań
- `2f19a5f` - Refaktoring systemu zadań z TaskCenter i TaskEnumerator
  - Nowy TaskCenter centrum zadań (235 linii)
  - Nowy TaskEnumerator enumerator zadań (297 linii)
  - Refaktoring i uproszczenie TaskSystem systemu zadań
  - Adaptacja DefaultSiliconBeing istoty krzemowej do nowej architektury
  - Aktualizacja DefaultSiliconBeingFactory fabryki
  - Rozszerzenie SiliconBeingBase klasy bazowej
  - 7 zmienionych plików, 796 dodanych linii, 275 usuniętych linii

#### Migracja systemu uprawnień
- `a06ed09` - Migracja IM i systemu uprawnień do projektu App
  - PermissionRequestQueue zmigrowany z Default/Fast do projektu App (443 dodane linie)
  - Usunięcie WebUIProvider wersji Default (403 usunięte linie)
  - Usunięcie HelpTool wersji Default (194 usunięte linie)
  - Usunięcie duplikatów PermissionRequestQueue wersji Default/Fast
  - Usunięcie IMPermissionAskHandler wersji Default
  - Aktualizacja PermissionRequestController kontrolera
  - 14 zmienionych plików, 496 dodanych linii, 1183 usunięte linie

#### Optymalizacja kontekstu AI
- `4c8aaff` - Optymalizacja menedżera kontekstu i rozszerzenie lokalizatora usług
  - Uproszczenie i optymalizacja ContextManager menedżera kontekstu
  - Rozszerzenie ServiceLocator lokalizatora usług (36 dodanych linii)
  - Rozszerzenie ToolManager menedżera narzędzi (34 dodane linie)
  - Ulepszenia klientów DashScopeClient i VolcengineArkClient
  - Aktualizacja wykonawców (CommandLine, Disk, Network)
  - 8 zmienionych plików, 116 dodanych linii, 98 usuniętych linii

#### Lokalizacja
- `5c5eef7` - Dodanie kluczy lokalizacji audytu i zadań
  - DefaultLocalizationBase dodano 127 linii definicji lokalizacji
  - 9 języków dodano klucze związane z audytem i zadaniami (26 linii każdy)
  - 11 zmienionych plików, 387 dodanych linii

#### Konfiguracja projektu
- `2067db6` - Aktualizacja konfiguracji projektu i reguł gitignore
  - Aktualizacja reguł .gitignore
  - Rozszerzenie konfiguracji DefaultConfigData i Fast DefaultConfigData
  - Ulepszenia SpeedyWorkNoteStorage przechowywania
  - Rozszerzenie rdzenia SpeedyPack
  - 5 zmienionych plików, 32 dodane linie, 6 usuniętych linii

### 2026-05-07

#### Lokalizacja włoska
- `8adc18c` - Dodano obsługę lokalizacji włoskiej i zaktualizowano wielojęzyczną dokumentację
  - Nowa lokalizacja it-IT włoska
  - Nowa implementacja ItIT lokalizacji (1909 linii)
  - Nowa obsługa włoska dla chińskiego kalendarza historycznego ChineseHistoricalItIT (586 linii)
  - Nowa lokalizacja włoska zasobnika systemowego TrayItIT (135 linii)
  - Nowy kompletny zestaw dokumentacji włoskiej (14 dokumentów: README, referencja API, architektura, system kalendarzowy, dziennik zmian, przewodnik dla współtwórców itp.)
  - Aktualizacja dokumentacji architektury, przewodnika programistycznego, przewodnika początkowego we wszystkich wersjach językowych
  - Dodano język włoski do enumeracji Language
  - 86 zmienionych plików, 11573 dodane linie, 769 usuniętych linii

#### Synchronizacja dokumentacji
- `12a5deb` - Aktualizacja wielojęzycznej dokumentacji architektury, dziennika zmian i przewodnika istoty krzemowej
  - Aktualizacja README w 8 językach
  - Aktualizacja dokumentacji architektury w 8 językach
  - Aktualizacja dziennika zmian w 8 językach
  - Aktualizacja przewodnika istoty krzemowej w 8 językach
  - Aktualizacja referencji narzędzi w 8 językach
  - Refaktoryzacja słownika terminów
  - 46 zmienionych plików, 1697 dodanych linii, 442 usunięte linie

### 2026-05-06

#### Duża refaktoryzacja modułów
- `eeb3be6` - Duża refaktoryzacja i reorganizacja modułów
  - Dostosowanie struktury projektu SiliconLife.App
  - Reorganizacja projektu SiliconLife.Fast
  - Reorganizacja projektu SiliconLife.Default
  - Reorganizacja współdzielonych modułów SiliconLife.Common
  - Reorganizacja modułów rdzeniowych SiliconLife.Core
  - Reorganizacja silnika przechowywania SiliconLife.Speedy
  - Reorganizacja narzędzia zarządzania SiliconLife.Speedy.Manager
  - 119 zmienionych plików, 6926 dodanych linii, 3066 usuniętych linii

### 2026-05-04

#### Klient AI
- `24d2c86` - Dodano VolcengineArkClient i zastąpiono Audit śledzeniem Usage
  - Nowy klient AI VolcengineArkClient Volcengine Ark
  - Obsługa trybu strumieniowego i niestrumieniowego
  - Wbudowana dwuwarstwowa kontrola szybkości (samokontrola szybkości + limit szybkości serwera)
  - Zgodność z protokołem OpenAI API
  - Zastąpienie systemu Audit śledzeniem Usage
  - 24 zmienione pliki, 802 dodane linie, 21 usuniętych linii

#### System narzędzi
- `f27650a` - Dodano narzędzie hot reload do samoponownego uruchomienia Fast
  - Nowe narzędzie HotReloadTool hot reload
  - Obsługa kompilacji online, aktualizacji i ponownego uruchomienia SiliconLife.Fast
  - Nowy niezależny aktualizator HotReload.exe
  - Bezpieczny mechanizm kopiowania plików (bez nadpisywania samego siebie)
  - Eleganckie zamykanie i oczekiwanie na zwolnienie portu
  - 9 zmienionych plików, 581 dodanych linii

#### Lokalizacja
- `6a5aad8` - Aktualizacja wszystkich plików i dodanie obsługi lokalizacji francuskiej
  - Nowa lokalizacja fr-FR francuska
  - Aktualizacja wszystkich wersji językowych
  - Francuskie tłumaczenie dokumentacji pomocy
  - Francuskie tłumaczenie interfejsu
  - Ponad 100 zmienionych plików

### 2026-05-03

#### Infrastruktura projektu
- `2664b0c` - Aktualizacja infrastruktury projektu i zależności
  - SiliconLife.Speedy.Manager dodano interfejs zarządzania WPF (MainForm.Designer.cs, MainForm.resx)
  - Nowy zasób ikony slc.ico (1,5 MB)
  - Znaczne rozszerzenie bezpiecznego skanowania PluginLoader (622 dodane linie)
  - Nowy PermissionedStreamFactory fabryka strumieni uprawnień (779 linii)
  - Nowy PermissionRequestQueue kolejka żądań uprawnień (wersje Default i Fast)
  - Nowy DebugLoggerProvider dostawca logowania debugowania
  - Rozszerzenie ConfigDataBase klasy bazowej konfiguracji
  - ToolManager dodano funkcję skanowania narzędzi wtyczek (ScanAllPluginAssemblies)
  - Rozszerzenie zarządzania cyklem życia SiliconBeingManager
  - Znaczne rozszerzenie klienta AI DashScopeClient Alibaba Cloud (227 dodanych linii)
  - Rozszerzenie fabryki DefaultSiliconBeingFactory
  - Aktualizacja widoków Web i kontrolerów (ChatView, WorkNoteView, PermissionRequestController)
  - 9 języków lokalizacji dodano klucze
  - 35 zmienionych plików, 28080 dodanych linii, 336 usuniętych linii

### 2026-05-02

#### Rozszerzenie klienta AI
- `c16f99f` - Aktualizacja klientów AI, interfejsu Web UI i komponentów przechowywania
  - Znaczne ulepszenie klienta DashScopeClient Alibaba Cloud
  - Optymalizacja SpeedyPackAutoCompactor automatycznego kompresora
  - Ulepszenie klasy bazowej widoków Web i BeingView
  - 6 zmienionych plików, 240 dodanych linii, 81 usuniętych linii

#### System wtyczek
- `242dc98` - Dodanie listy wtyczek na stronie o programie
  - AboutController dodano wyświetlanie informacji o wtyczkach
  - AboutViewModel dodano model danych wtyczek
  - AboutView dodano renderowanie listy wtyczek
  - 9 języków lokalizacji dodano klucze związane z wtyczkami
  - 14 zmienionych plików, 160 dodanych linii, 1 usunięta linia

#### Optymalizacja AI
- `147f8f4` - Uproszczenie tekstu podpowiedzi pamięci kontekstowej
  - Optymalizacja podpowiedzi AI w ContextManager
  - 1 zmieniony plik, 1 dodana linia, 1 usunięta linia

#### Optymalizacja przechowywania Speedy
- `8bda2d3` - Aktualizacja przechowywania Speedy i implementacji kontrolera pamięci
  - Poprawka interwału SpeedyPackAutoCompactor
  - Optymalizacja przetwarzania ścieżek SpeedyTimeStorage
  - Ulepszenie MemoryController kontrolera pamięci
  - Aktualizacja UI SpeedyPack.Manager
  - 4 zmienione pliki, 21 dodanych linii, 18 usuniętych linii

#### Rozszerzenie zasobnika systemowego
- `8972654` - Rozszerzenie obsługi lokalizacji okna stanu zasobnika systemowego
  - 9 języków lokalizacji zasobnika dodano wpis zarządzania Speedy
  - TrayStatusWindow dodano element menu zarządzania Speedy
  - 11 zmienionych plików, 72 dodane linie

#### Optymalizacja Speedy.Manager
- `6f5db09` - Optymalizacja UI menedżera SpeedyPack i komponentów wewnętrznych
  - Refaktoring interfejsu MainForm
  - Optymalizacja zarządzania pamięcią FreeList
  - Ulepszenie kolejki zapisu WriteQueue
  - Optymalizacja rdzenia SpeedyPack
  - 5 zmienionych plików, 96 dodanych linii, 88 usuniętych linii

#### Rozszerzenie systemu przechowywania
- `57f9d5d` - Ulepszenie systemu przechowywania, dodanie automatycznej kompresji i obsługi niekompletnych dat
  - Nowy SpeedyPackAutoCompactor timer automatycznej kompresji (interwał 30 minut)
  - Rozszerzenie SpeedyPackRegistry menedżera singletona
  - Ulepszenia adaptacji SpeedyStorage, SpeedyTimeStorage, SpeedyWorkNoteStorage
  - SpeedyPack dodano FreeList zarządzanie wolną przestrzenią (149 linii)
  - Refaktoryzacja i optymalizacja PackFileWriter zapisywacza
  - Rozszerzenie WriteOperation, WriteQueue kolejki zapisu
  - Rozszerzenie SpeedyPackOptions opcji konfiguracji
  - IncompleteDate dodano metody porównania
  - Ulepszenie PluginLoader ładowacza wtyczek
  - Aktualizacja procesu inicjalizacji Program.cs wersji Default i Fast
  - Uproszczenie DefaultConfigData danych konfiguracyjnych
  - Uproszczenie KnowledgeNetwork sieci wiedzy
  - Optymalizacja kontrolerów ChatController, MemoryController
  - Rozszerzenie funkcji MainForm SpeedyPack.Manager
  - 22 zmienione pliki, 639 dodanych linii, 253 usunięte linie

#### Aktualizacja Speedy.Manager
- `b04ed33` - Aktualizacja plików Speedy.Manager

### 2026-05-01

#### Refaktoring architektury: Speedy zastępuje LiteDB
- `6600972` - Zastąpienie LiteDB przechowywaniem Speedy, dodanie systemu wtyczek i projektów Speedy
  - **Nowy projekt SiliconLife.Speedy**: wysokowydajny silnik przechowywania .spk
    - Klasa rdzeniowa SpeedyPack (489 linii): mapowanie katalogu w pamięci + pamięć podręczna wpisów + asynchroniczna kolejka zapisu
    - Klasa konfiguracji SpeedyPackOptions: TTL pamięci podręcznej, maksymalna liczba wpisów w pamięci podręcznej, tryb tylko do odczytu
    - Interfejs transakcji IPackTransaction: obsługa atomowych operacji zapisu
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
    - Nowy SpeedyPackRegistry (menedżer singletona na poziomie procesu)
    - Nowy SpeedyPackAutoCompactor (timer automatycznej kompresji)
    - Usunięcie implementacji przechowywania LiteDB (LiteDBStorage, LiteDBTimeStorage, LiteDBWorkNoteStorage, LiteDBLoggerProvider, LiteDBManager, LiteDBModels)
    - Usunięcie kodu związanego z oknem zarządzania LiteDB
  - **System wtyczek**:
    - Nowy interfejs IPlugin (Core/Plugins/IPlugin.cs)
    - Nowy ładowacz wtyczek PluginLoader (Core/Plugins/PluginLoader.cs)
    - Obsługa ładowania bibliotek DLL wtyczek z katalogu
    - Bezpieczne skanowanie: zakaz sprawdzania przestrzeni nazw (System.IO, System.Net, Microsoft.CodeAnalysis itp.)
    - Biała lista zaufanych zestawów (Google.Protobuf, Newtonsoft.Json, MessagePack itp.)
    - Izolowane ładowanie z niestandardowym AssemblyLoadContext
    - ToolManager dodano metodę ScanAllPluginAssemblies
    - CoreHost integracja z ładowaczem wtyczek
  - 119 zmienionych plików, 6926 dodanych linii, 3066 usuniętych linii

#### Rozszerzenie istoty krzemowej
- `3aef4c3` - Dodanie stanu aktywności Stopped i ulepszenie obsługi błędów
  - Nowy stan Stopped istoty krzemowej
  - Ulepszenie mechanizmów obsługi błędów i odzyskiwania

#### Aktualizacja lokalizacji
- `513c65d` - Aktualizacja wszystkich wersji językowych i dokumentacji
  - Nowy komponent MarkdownEditorComponent (625 linii)
  - Nowy komponent DetailsComponent (130 linii)
  - Nowy komponent akordeonowy AccordionComponent (285 linii)
  - Aktualizacja kontrolerów BeingController, ChatController, MemoryController, PermissionController
  - Refaktoring widoków BeingView, ChatView, MemoryView, SoulEditorView
  - Usunięcie starego MarkdownEditorView
  - Migracja komponentowa InitController
  - 115 zmienionych plików, 5761 dodanych linii, 2362 usunięte linie

### 2026-04-30

#### Funkcja zasobnika systemowego
- `101b203` - Implementacja okna stanu zasobnika i ApplicationContext
  - Nowe zasoby ikon zasobnika (alpha.png, noWord.png, slc.ico, wordIcon.png)
  - Implementacja TrayStatusWindow okna stanu
  - Obsługa lokalizacji zasobnika w 9 językach (TrayCsCZ, TrayDeDE, TrayEnUS itp.)
  - TrayLocalizationBase abstrakcyjna klasa bazowa
  - 24 zmienione pliki, 27995 dodanych linii, 1 usunięta linia (w tym pliki zasobów)

#### Komponentowa architektura UI
- `e61cfaa` - Ukończenie komponentowej architektury UI, implementacja 24 komponentów
  - Etap MVP (8): ComponentBase, Div, Span, Button, Input, Form, Select, Label
  - Drugi etap (6): Accordion, Card, Tabs, Table, Modal, Message
  - Trzeci etap (5): Calendar, Tree, Chart, FileUpload, RichText
  - Nowe klasy pomocnicze Js, Behavior, DomUpdate
  - 25 zmienionych plików, 2666 dodanych linii

- `7449e51` - Ulepszenie systemu komponentów i dodanie nowych motywów skórek
  - Rozszerzenie komponentów A, Button, Div, Form, Input itp.
  - Nowe 3 motywy skórek: HighContrast (wysoki kontrast), Light (jasny), Minimal (minimalistyczny)
  - Aktualizacja istniejących skórek (Admin, Chat, Creative, Dev)
  - Migracja komponentowa InitController
  - 32 zmienione pliki, 1466 dodanych linii, 1238 usuniętych linii

- `1ba8636` - Rozpoczęcie migracji komponentowej InitController (w toku)
  - 9 zmienionych plików, 574 dodane linie, 145 usuniętych linii

#### Unifikacja systemu przechowywania
- `895dff9` - Unifikacja soul.md i state.json z użyciem interfejsu IStorage
  - DefaultSiliconBeing używa IStorage do odczytu i zapisu plików duszy i stanu
  - Nowy StateFileManager menedżer plików stanu
  - Refaktoring SoulFileManager i adaptacja do IStorage
  - 8 zmienionych plików, 201 dodanych linii, 116 usuniętych linii

#### Rozszerzenie zarządzania LiteDB
- `a34bef4` - Dodanie LiteDBManager i rozszerzenie lokalizacji zasobnika
  - Nowy wpis zarządzania LiteDB w menu zasobnika
  - Aktualizacja lokalizacji zasobnika w 9 językach
  - 10 zmienionych plików, 196 dodanych linii

- `c4a79ca` - Dodanie fabryki lokalizacji z uwzględnieniem języka dla okna zarządzania LiteDB
  - 1 zmieniony plik, 78 dodanych linii

- `5ebc55e` - Konwersja LiteDBAdminLocalization na abstrakcyjną klasę bazową
  - 10 zmienionych plików, 1356 dodanych linii

#### Naprawa systemu konfiguracji
- `2da5256` - Dodanie metody abstrakcyjnej ConfigExists i naprawa duplikatów rekordów konfiguracji LiteDB
  - ConfigDataBase dodano metodę ConfigExists
  - Implementacja DefaultConfigData wersji Fast sprawdzania istnienia konfiguracji LiteDB
  - Naprawa problemu duplikatów kluczy konfiguracji LiteDB
  - 9 zmienionych plików, 210 dodanych linii, 2 usunięte linie

#### Optymalizacja czatu i widoków
- `d3618ec` - Optymalizacja sesji czatu, systemu przechowywania, modelu czasu i klasy bazowej widoków
  - Optymalizacja BroadcastChannel, GroupChatSession, SingleChatSession
  - ITimeStorage dodano metody zapytań
  - Synchroniczna aktualizacja FileSystemStorage i LiteDBStorage
  - Refaktoring i optymalizacja ViewBase (wersje Default i Fast)
  - 11 zmienionych plików, 622 dodane linie, 392 usunięte linie

### 2026-04-29

#### Refaktoring architektury: ekstrakcja współdzielonych modułów
- `a102428` - Migracja współdzielonych modułów z SiliconLife.Default do SiliconLife.Common
  - Ekstrakcja 32 implementacji kalendarza do projektu Common
  - Ekstrakcja klas bazowych lokalizacji i 21 implementacji językowych do projektu Common
  - Ekstrakcja menedżera uprawnień i domyślnej implementacji istoty krzemowej do projektu Common
  - Ekstrakcja 23 wbudowanych implementacji narzędzi do projektu Common
  - Ekstrakcja implementacji Playwright WebView do projektu Common
  - Aktualizacja przestrzeni nazw na SiliconLife.Collective
  - 122 zmienione pliki, 586 dodanych linii, 343 usunięte linie

#### Ulepszenie jakości kodu
- `17566fe` - Zastąpienie Console.WriteLine w projektach Core, Common i Default systemem logowania
  - Aktualizacja 6 plików: ContextManager, AuditLogger, DefaultConfigData itp.
  - Ujednolicenie użycia interfejsu ILogger, poprawa utrzymywalności kodu
  - 6 zmienionych plików, 12 dodanych linii, 8 usuniętych linii

#### SiliconLife.Fast wersja wysokowydajna
- `54a0307` - Dodanie projektu SiliconLife.Fast i naprawa kompilacji
  - Kompletny punkt wejścia aplikacji okienkowej Windows
  - Obsługa zasobnika systemowego (NotifyIcon)
  - Przeniesienie wszystkich kontrolerów Web UI (ponad 20)
  - Przeniesienie wszystkich komponentów widoków Web
  - Przeniesienie 4 motywów skórek (Admin, Chat, Creative, Dev)
  - 125 zmienionych plików, 61186 dodanych linii

#### Synchronizacja wielojęzycznej dokumentacji
- `265fde8` - Synchronizacja dokumentacji dwuwariantowej architektury do wszystkich języków
  - Aktualizacja architecture.md, changelog.md w 7 językach
  - Aktualizacja contributing.md w 6 językach
  - Aktualizacja getting-started.md, roadmap.md w 7 językach
  - 47 zmienionych plików, 1214 dodanych linii, 38 usuniętych linii

#### System przechowywania LiteDB (wersja Fast)
- `4704862` - Dodanie zależności LiteDB i infrastruktury
  - Nowy menedżer LiteDBManager
  - Nowe modele danych LiteDBModels
  - 3 zmienione pliki, 252 dodane linie

- `4220036` - Implementacja klas przechowywania LiteDB
  - LiteDBStorage: implementacja interfejsu IStorage
  - LiteDBTimeStorage: implementacja interfejsu ITimeStorage
  - LiteDBWorkNoteStorage: implementacja interfejsu IWorkNoteStorage
  - 3 zmienione pliki, 581 dodanych linii

- `38ebd23` - Migracja konfiguracji i systemu logowania do LiteDB
  - Adaptacja DefaultConfigData do przechowywania LiteDB
  - Nowy dostawca logowania LiteDBLoggerProvider
  - 2 zmienione pliki, 203 dodane linie, 67 usuniętych linii

- `e687157` - Migracja sieci wiedzy z systemu plików do LiteDB
  - Pełna refaktoryzacja KnowledgeNetwork, użycie LiteDB do przechowywania danych trójek
  - 1 zmieniony plik, 231 dodanych linii, 72 usunięte linie

- `4220169` - Integracja przechowywania LiteDB z Program i ProjectManager
  - Inicjalizacja przechowywania LiteDB w Program.cs
  - Adaptacja ProjectManager do przechowywania notatek roboczych LiteDB
  - 2 zmienione pliki, 40 dodanych linii, 17 usuniętych linii

- `5f3a709` - Usunięcie przestarzałych implementacji przechowywania w systemie plików
  - Usunięcie FileSystemLoggerProvider, FileSystemStorage, FileSystemTimeStorage itp.
  - 6 zmienionych plików, 1518 usuniętych linii

- `e1a4ef2` - docs: dodanie identyfikatora wersji v0.1.0-alpha do całej dokumentacji
  - 127 zmienionych plików, 2297 dodanych linii, 2471 usuniętych linii

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### Refaktoring systemu przechowywania
- `8dd26e3` - Unifikacja interfejsu ITimeStorage z użyciem IncompleteDate i dodanie hierarchicznego API zapytań
  - Usunięcie metod przeciążenia DateTime z interfejsu ITimeStorage, unifikacja z użyciem IncompleteDate
  - IncompleteDate dodano metodę porównania CompareTo(DateTime) i metodę rozszerzenia Expand()
  - Nowe hierarchiczne API zapytań GetEarliestTimestamp(), GetLatestTimestamp()
  - Nowe metody HasSummary() i QueryWithLevel(), obsługa zapytań według poziomu czasu
  - Refaktoryzacja algorytmu kompresji Memory.cs, użycie nowego hierarchicznego API zapytań dla poprawy wydajności
  - Pełna implementacja nowych metod interfejsu w FileSystemTimeStorage.cs
  - Synchroniczna aktualizacja wszystkich wywołań: ChatSystem, ChatSession, BroadcastChannel, AuditLogger, TokenUsageRecord itp.
  - Aktualizacja systemu narzędzi: HelpTool, LogTool, TokenAuditTool adaptacja do nowego interfejsu
  - Aktualizacja kontrolerów Web: AuditController, ChatController, ChatHistoryController adaptacja do nowego interfejsu
  - 41 zmienionych plików, 1820 dodanych linii, 903 usunięte linie

### 2026-04-27

#### Rozszerzenie systemu dokumentacji pomocy
- `9989d79` - Aktualizacja lokalizacji, systemu pomocy i widoków Web
  - Nowy interfejs dokumentacji pomocy fabryki klienta AI IAIClientFactoryHelp.cs
  - Ukończenie tłumaczenia wszystkich dokumentów pomocy na 9 języków
  - HelpTopics.cs dodano 40 definicji tematów pomocy
  - Pełna aktualizacja widoków Web: InitController, AuditView, ConfigView, KnowledgeView, LogView itp.
  - Rozszerzenie systemu lokalizacji: wszystkie wersje językowe dodano nowe klucze lokalizacji
  - Aktualizacja fabryki klientów AI: ulepszenia DashScopeClientFactory, OllamaClientFactory
  - 30 zmienionych plików, 10086 dodanych linii, 15 usuniętych linii

#### Nowa zawartość dokumentacji pomocy
- `e7afe94` - Nowa dokumentacja pomocy pliku duszy i dziennika audytu
  - Nowa dokumentacja pomocy zarządzania plikiem duszy
  - Nowa dokumentacja pomocy dziennika audytu
  - HelpTopics.cs dodano definicje tematów
  - Znaczna refaktoryzacja HelpView.cs, ulepszenie logiki renderowania dokumentacji
  - Refaktoryzacja PermissionView.cs, ulepszenie interfejsu zarządzania uprawnieniami
  - Rozszerzenie modułów rdzeniowych: ulepszenia SiliconBeingManager, TaskSystem, ToolManager
  - Refaktoryzacja TaskTool.cs, ulepszenie funkcji zarządzania zadaniami
  - Pełna aktualizacja widoków Web: synchroniczna aktualizacja wszystkich komponentów widoków
  - Uproszczenie HelpController.cs, optymalizacja logiki kontrolera
  - 30 zmienionych plików, 7100 dodanych linii, 897 usuniętych linii

### 2026-04-26

#### System dokumentacji pomocy
- `07895d7` - Rozszerzenie systemu dokumentacji pomocy, dodanie 3 dokumentów i ukończenie tłumaczenia na 9 języków
  - Nowe przewodniki: system pamięci, instalacja i konfiguracja Ollama, korzystanie z platformy Alibaba Cloud Bailian
  - Ukończenie tłumaczenia wszystkich 10 dokumentów pomocy na 9 języków
  - Uproszczenie logiki renderowania HelpView
  - 18 zmienionych plików, 14418 dodanych linii, 1364 usunięte linie

#### Lokalizacja niemiecka
- `0cfd8a1` - Dodanie pełnej obsługi lokalizacji niemieckiej (de-DE)
  - Kompletny plik lokalizacji niemieckiej
  - Nowa obsługa niemiecka dla chińskiego kalendarza historycznego
  - Nowe tłumaczenie dokumentacji pomocy na język niemiecki
  - Pełna synchronizacja wszystkich dokumentów w 9 językach
  - 135 zmienionych plików, 26186 dodanych linii, 14371 usuniętych linii

#### Synchronizacja dokumentacji
- `3aada7d` - Synchronizacja dokumentacji tradycyjnego chińskiego (zh-HK) z uproszczonym chińskim
  - 3 zmienione pliki, 519 dodanych linii, 422 usunięte linie
- `2f6abff` - Dodanie lokalizacji nazwy wyświetlanej narzędzia pomocy dla wszystkich języków
  - 7 zmienionych plików, 47 dodanych linii, 7 usuniętych linii

#### Refaktoring systemu wiedzy
- `60944fe` - Unifikacja przestrzeni nazw do SiliconLife.Collective
  - 8 zmienionych plików, 5 dodanych linii, 8 usuniętych linii
- `69c51c5` - Dodanie systemu dokumentacji pomocy i tłumaczenie komentarzy kodu na angielski
  - 29 zmienionych plików, 3385 dodanych linii, 22 usunięte linie

### 2026-04-25

#### Automatyzacja przeglądarki WebView
- `41757c3` - Implementacja wieloplatformowej automatyzacji przeglądarki WebView opartej na Playwright
  - 6 zmienionych plików, 1152 dodane linie

#### Aktualizacja dokumentacji
- `0ff797b` - Dodanie dokumentacji KnowledgeTool i WorkNoteTool (7 języków)
  - 28 zmienionych plików, 4983 dodane linie
- `ad77415` - Aktualizacja wszystkich plików changelog, dodanie historii Git z 2026-04-25
  - 7 zmienionych plików, 168 dodanych linii

#### Zarządzanie obszarem roboczym projektu
- `785c551` - Implementacja zarządzania obszarem roboczym projektu, obejmująca notatki robocze i system zadań
  - Nowy system zarządzania obszarem roboczym projektu
  - Funkcja notatek roboczych do śledzenia postępu projektu
  - Integracja systemu zarządzania zadaniami
  - 29 zmienionych plików, 4256 dodanych linii, 36 usuniętych linii

#### Lokalizacja czeska
- `b4bbf39` - Dodanie pełnej lokalizacji czeskiej (cs-CZ) i aktualizacja dokumentacji wszystkich języków
  - 116 zmienionych plików, 4933 dodane linie, 222 usunięte linie
- `faf078f` - Naprawa błędów kompilacji lokalizacji czeskiej
  - 3 zmienione pliki, 910 dodanych linii, 1 usunięta linia

#### Rozszerzenie systemu wiedzy
- `20adaac` - Dodanie KnowledgeTool i obsługa pełnej lokalizacji
  - 34 zmienione pliki, 2331 dodanych linii, 56 usuniętych linii

### 2026-04-24

#### Rozszerzenie systemu zarządzania pamięcią
- `c7b2ecc` - Rozszerzenie funkcji zarządzania pamięcią, dodanie zaawansowanego filtrowania, statystyk i widoku szczegółów
  - Nowa funkcja zaawansowanego filtrowania pamięci
  - Implementacja funkcji statystyk pamięci
  - Dodanie strony widoku szczegółów pamięci
  - Obsługa wielojęzycznej lokalizacji (6 języków)
  - 13 zmienionych plików, 840 dodanych linii, 86 usuniętych linii

#### Rozszerzenie systemu uprawnień
- `4489ad6` - Dodanie usługi pogodowej wttr.in do białej listy sieci
  - Pełna synchroniczna aktualizacja wielojęzycznej dokumentacji (6 języków)
  - 14 zmienionych plików, 417 dodanych linii, 1 usunięta linia

#### Naprawa interfejsu Web
- `d9d72e9` - Naprawa problemu priorytetu CSS modalu szczegółów notatek roboczych
  - 19 zmienionych plików, 1744 dodane linie, 6 usuniętych linii

#### Optymalizacja historii czatu
- `0df599c` - Naprawa problemu renderowania wyników narzędzi jako niezależne wiadomości czatu
  - 1 zmieniony plik, 222 dodane linie, 21 usuniętych linii
- `057b09d` - Optymalizacja wyświetlania szczegółów historii czatu, ulepszenie renderowania wywołań narzędzi
  - 3 zmienione pliki, 389 dodanych linii, 68 usuniętych linii

#### Historia wykonania czasomierza
- `fa3f06f` - Dodanie funkcji historii wykonania czasomierza, obejmującej widok szczegółów
  - 8 zmienionych plików, 937 dodanych linii, 10 usuniętych linii
- `d824835` - Dodanie kluczy lokalizacji historii wykonania czasomierza (wszystkie języki)
  - 7 zmienionych plików, 88 dodanych linii

#### Rozszerzenie lokalizacji
- `c13cb17` - Rejestracja wariantu języka hiszpańskiego
  - 1 zmieniony plik, 4 dodane linie
- `9c44f34` - Dodanie wielojęzycznej obsługi lokalizacji chińskiego kalendarza historycznego
  - 16 zmienionych plików, 6049 dodanych linii, 1 usunięta linia

#### Ulepszenia funkcji rdzeniowych
- `1e7c7b2` - Ulepszenie kompresji pamięci i śledzenia wykonania narzędzi
  - 4 zmienione pliki, 338 dodanych linii, 86 usuniętych linii

### 2026-04-23

#### Lokalizacja narzędzi
- `192fc6e` - Dodanie brakujących lokalizacji nazw narzędzi dla 5 narzędzi
  - 6 zmienionych plików, 30 dodanych linii

#### Aktualizacja dokumentacji
- `882c08f` - Aktualizacja wszystkich plików changelog, dodanie pełnej historii Git i usunięcie fałszywych numerów wersji
  - 45 zmienionych plików, 8815 dodanych linii, 1611 usuniętych linii

#### Rozszerzenie strony czatu
- `65c157b` - Dodanie wskaźnika ładowania na stronie czatu i automatyczny wybór sesji kuratora
  - 10 zmienionych plików, 211 dodanych linii, 7 usuniętych linii

#### Funkcja historii czatu
- `e483348` - Implementacja funkcji przeglądania historii czatu istoty krzemowej
  - Nowy ChatHistoryController
  - Utworzenie ChatHistoryViewModel
  - Implementacja stron ChatHistoryListView i ChatHistoryDetailView
  - Dodanie kluczy lokalizacji historii czatu (5 języków)
  - 12 zmienionych plików, 1178 dodanych linii

#### Rozszerzenie sterowania przepływem AI
- `30a2d4e` - Rozszerzenie anulowania przepływu AI, integracja IM i inicjalizacja rdzenia hosta
  - 11 zmienionych plików, 387 dodanych linii, 12 usuniętych linii

#### Kolejka wiadomości czatu
- `db48c51` - Dodanie kolejki wiadomości czatu, metadanych plików i obsługi anulowania strumienia
  - 4 zmienione pliki, 357 dodanych linii

#### Obsługa przesyłania plików
- `28fb344` - Implementacja okna dialogowego źródła plików i obsługi przesyłania plików
  - 3 zmienione pliki, 1100 dodanych linii, 2 usunięte linie
- `1d3e2cc` - Dodanie ciągów lokalizacji okna dialogowego źródła plików (6 języków)
  - 6 zmienionych plików, 30 dodanych linii

#### Aktualizacja dokumentacji
- `8111e92` - Dodanie linku Wiki w sekcji repozytorium README
  - 1 zmieniony plik, 3 dodane linie, 1 usunięta linia

### 2026-04-22

#### Lokalizacja dokumentacji
- `66c11eb` - Tłumaczenie chińskich komentarzy na angielski i aktualizacja wszystkich changelogów
  - 11 zmienionych plików, 373 dodane linie, 163 usunięte linie

#### Rozszerzenie wiadomości SSE
- `b574b2b` - Dodanie senderName dla wiadomości historycznych do identyfikacji AI
  - 1 zmieniony plik, 9 dodanych linii

#### Funkcja czatu
- `601fc14` - Dodanie operacji mark_read do oznaczania końca sesji
  - 7 zmienionych plików, 196 dodanych linii, 36 usuniętych linii

#### Optymalizacja systemu narzędzi
- `7a03a19` - Ulepszenie elastyczności zapytań konwersacyjnych LogTool
  - 1 zmieniony plik, 57 dodanych linii, 24 usunięte linie

#### Rozszerzenie lokalizacji
- `0a8d750` - Dodanie ogólnego podpowiedzi systemowego dla proaktywnego zachowania istoty krzemowej
  - 8 zmienionych plików, 460 dodanych linii, 48 usuniętych linii

#### Refaktoring systemu logowania
- `2b771f3` - Oddzielenie LogController od wejścia/wyjścia plików, dodanie API odczytu logów
  - 4 zmienione pliki, 172 dodane linie, 137 usuniętych linii
- `12da302` - Dodanie filtra istoty krzemowej do widoku logów
  - 9 zmienionych plików, 147 dodanych linii, 10 usuniętych linii
- `8f6cb1e` - Dodanie parametru beingId do interfejsu ILogger, realizacja separacji logów systemowych/istoty krzemowej
  - 47 zmienionych plików, 524 dodane linie, 490 usuniętych linii

#### Ulepszenie systemu uprawnień
- `4c747ad` - Refaktoring PermissionTool, ExecuteCodeTool, dodanie API EvaluatePermission
  - 18 zmienionych plików, 680 dodanych linii, 492 usunięte linie

#### Naprawa błędów
- `1c96e99` - Naprawa błędu wyszukiwania search_files i search_content w katalogu głównym
  - 1 zmieniony plik, 98 dodanych linii, 41 usuniętych linii

#### Integracja narzędzi
- `135710d` - Usunięcie SearchTool, przeniesienie wyszukiwania lokalnego do DiskTool
  - 2 zmienione pliki, 185 dodanych linii, 365 usuniętych linii

#### Rozszerzenie systemu narzędzi
- `70ce7fb` - Implementacja DatabaseTool do strukturalnych zapytań bazodanowych
  - 1 zmieniony plik, 382 dodane linie
- `be29a09` - Implementacja LogTool do zapytań o historię operacji i konwersacji
  - 1 zmieniony plik, 298 dodanych linii
- `4ea7702` - Implementacja PermissionTool do dynamicznego zarządzania uprawnieniami
  - 1 zmieniony plik, 457 dodanych linii
- `1384ff4` - Implementacja ExecuteCodeTool do wykonywania kodu w wielu językach
  - 1 zmieniony plik, 477 dodanych linii
- `82d1e11` - Implementacja SearchTool do wyszukiwania informacji
  - 1 zmieniony plik, 363 dodane linie

#### Optymalizacja interfejsu Web
- `0675c45` - Optymalizacja podświetlania bloków kodu markdown w okienku podglądu
  - 1 zmieniony plik, 4 dodane linie, 23 usunięte linie
- `702b3f3` - Rozszerzenie widoku zadań, dodanie odznak stanu i wyświetlania metadanych
  - 8 zmienionych plików, 221 dodanych linii, 9 usuniętych linii
- `6ed9a79` - Ulepszenie przechowywania wiadomości czatu i renderowania widoku
  - 8 zmienionych plików, 140 dodanych linii, 29 usuniętych linii

### 2026-04-21

#### Naprawa błędów
- `c6b518b` - Naprawa przekazywania wiadomości czasomierza i przechowywania wiadomości czatu
  - 3 zmienione pliki, 297 dodanych linii, 124 usunięte linie

#### Zarządzanie konfiguracją
- `4305769` - Dodanie .gitattributes do zarządzania końcami linii
  - 1 zmieniony plik, 32 dodane linie

#### Ulepszenia interfejsu Web
- `188c6f8` - Rejestracja tras API listy zadań i dodanie wyświetlania pustego stanu
  - 2 zmienione pliki, 35 dodanych linii, 2 usunięte linie
- `634e8ca` - Dodanie linku powrotu do listy na stronie uprawnień
  - 1 zmieniony plik, 16 dodanych linii
- `6ba591d` - Dodanie niezależnego edytora konfiguracji AI dla istoty krzemowej
  - 11 zmienionych plików, 842 dodane linie, 18 usuniętych linii
- `0a826f5` - Dodanie powiadomienia o pomyślnym zapisie w edytorze kodu
  - 1 zmieniony plik, 9 dodanych linii, 2 usunięte linie
- `2940373` - Rozszerzenie interfejsu Web, dodanie podpowiedzi kodu i ulepszenia UI
  - 11 zmienionych plików, 1054 dodane linie, 75 usuniętych linii

#### Naprawa systemu uprawnień
- `592c7ab` - Naprawa instancjonowania wywołań zwrotnych i kolejności rejestracji
  - 2 zmienione pliki, 38 dodanych linii, 7 usuniętych linii

#### Rozszerzenie bezpieczeństwa
- `833ead2` - Dodanie weryfikacji referencji zestawów dla kompilacji dynamicznej
  - 4 zmienione pliki, 135 dodanych linii, 8 usuniętych linii

#### Rozszerzenie systemu uprawnień
- `5879621` - Dodanie weryfikacji prekompilacji wywołań zwrotnych uprawnień i rozszerzenie obsługi błędów
  - 21 zmienionych plików, 617 dodanych linii, 26 usuniętych linii

#### Aktualizacja dokumentacji
- `4dbf659` - Aktualizacja changelog do v0.5.1, zastąpienie zastępczych URL-i GitHub, dodanie mirrora Gitee, lokalizacja nazwy Bilibili według języka, aktualizacja e-maila
  - 32 zmienione pliki, 489 dodanych linii, 180 usuniętych linii

#### Konfiguracja i punkt wejścia
- `0fc1693` - Aktualizacja punktu wejścia programu i konfiguracji projektu
  - 2 zmienione pliki, 7 dodanych linii

#### Refaktoring systemu uprawnień
- `ea9179a` - Ulepszenie implementacji systemu uprawnień
  - 5 zmienionych plików, 358 dodanych linii, 152 usunięte linie

#### Naprawa błędów
- `928a96d` - Naprawa implementacji obliczeń kalendarzowych
  - 4 zmienione pliki, 12 dodanych linii, 12 usuniętych linii

#### AI i kalendarz
- `646813e` - Ulepszenie implementacji fabryki klientów AI
  - 2 zmienione pliki, 21 dodanych linii, 20 usuniętych linii

#### Lokalizacja
- `7940d9c` - Dodanie obsługi lokalizacji koreańskiej
  - 7 zmienionych plików, 2424 dodane linie, 10 usuniętych linii
- `4ff98ad` - Refaktoring dokumentacji, obsługa wielu języków
  - 81 zmienionych plików, 23818 dodanych linii, 1886 usuniętych linii

### 2026-04-20

#### Doskonalenie funkcji rdzeniowych
- `28905b5` - Kompletna obsługa wielu języków, fabryka klientów AI, system uprawnień i ustawienia lokalizacji
  - System logowania z menedżerem, wpisami i różnymi poziomami logowania
  - System audytu tokenów do odpytywania i śledzenia użycia tokenów
  - Fabryka klientów AI automatycznie odkrywająca różne platformy AI
  - System wywołań zwrotnych uprawnień z własnym przechowywaniem
  - Implementacja logera konsolowego
  - Obsługa wielu języków: angielski i uproszczony chiński
  - WebUI Messenger z WebSocket do czatu na żywo
  - Rozszerzenie domyślnej istoty krzemowej o lokalizację
  - 39 zmienionych plików, 4670 dodanych linii, 175 usuniętych linii

### 2026-04-19

#### Czasomierze i kalendarz
- `c933fd8` - Aktualizacja lokalizacji, systemu czasomierza, widoków Web i dodanie narzędzi
  - Lepszy menedżer lokalizacji
  - System harmonogramowania zadań czasowych
  - Konfiguracja AI i zarządzanie kontekstem
  - Narzędzie kalendarzowe obsługujące 32 typy kalendarzy
  - Kontroler Web dla API kalendarza
  - Narzędzie zarządzania zadaniami
  - 46 zmienionych plików, 4018 dodanych linii, 975 usuniętych linii

**Ulepszenia architektury**
- Przeprojektowanie architektury widoków Web dla lepszego wsparcia skórek
- Ulepszenie systemu zarządzania istotami z lepszą obsługą stanu

### 2026-04-18

- `9f585e1` - Aktualizacja lokalizacji, systemu czasomierza, widoków Web i dodanie narzędzi
  - Ulepszenia czasomierza i harmonogramowania
  - Lepsze widoki Web z ulepszonymi komponentami UI
  - Więcej implementacji narzędzi
  - 57 zmienionych plików, 3328 dodanych linii, 389 usuniętych linii

### 2026-04-17

- `9b71fcd` - Aktualizacja modułów rdzeniowych, dodanie dokumentacji zh-HK, kanału broadcastowego, narzędzia konfiguracji i widoku Web audytu
  - Kanał broadcastowy do wspólnego czatu wielu istot krzemowych
  - System narzędzia konfiguracji
  - Widok Web audytu
  - Dokumentacja tradycyjnego chińskiego
  - 42 zmienione pliki, 3533 dodane linie, 268 usuniętych linii

### 2026-04-16

- `5040f05` - Aktualizacja modułów rdzeniowych i domyślnych
  - Optymalizacja modułów i naprawa błędów
  - Aktualizacje implementacji i ulepszenia
  - 58 zmienionych plików, 9916 dodanych linii, 111 usuniętych linii

### 2026-04-15

- `3efab5f` - Aktualizacja wielu modułów: AI, czat, IM, narzędzia, Web, lokalizacja, przechowywanie
  - Ulepszenia klientów AI
  - Rozszerzenie systemu czatu
  - Aktualizacja dostawców komunikatora
  - Optymalizacja systemu narzędzi
  - Ulepszenia infrastruktury Web
  - Optymalizacja lokalizacji
  - Aktualizacja systemu przechowywania
  - 33 zmienione pliki, 788 dodanych linii, 232 usunięte linie

### 2026-04-14

- `4241a2f` - Podstawowe ukończenie funkcji czatu, optymalizacja przesyłania UI
  - Ukończenie funkcji systemu czatu
  - Optymalizacja UI przesyłania plików
  - 16 zmienionych plików, 1234 dodane linie, 102 usunięte linie

### 2026-04-13

- `c498c31` - Aktualizacja kodu
  - Ogólne ulepszenia i optymalizacje kodu
  - 32 zmienione pliki, 1045 dodanych linii, 546 usuniętych linii

### 2026-04-12

#### Dokumentacja i lokalizacja
- `2161002` - Refaktoring dokumentacji i rozszerzenie lokalizacji
  - 17 zmienionych plików, 982 dodane linie, 92 usunięte linie
- `03d94e4` - Rozszerzenie systemu konfiguracji i lokalizacji
  - 25 zmienionych plików, 1378 dodanych linii, 154 usunięte linie
- `9976a35` - Dodanie strony o programie i lokalizacji
  - 14 zmienionych plików, 699 dodanych linii, 44 usunięte linie

#### Czat i widoki Web
- `0c8ccfc` - Rozszerzenie systemu czatu, lokalizacji i widoków Web
  - 13 zmienionych plików, 402 dodane linie, 56 usuniętych linii
- `a8f1342` - Przeprojektowanie warstwy komunikacji Web, przejście z WebSocket na SSE
  - 27 zmienionych plików, 793 dodane linie, 935 usuniętych linii

### 2026-04-11

#### System logowania
- `e8fe259` - Dodanie systemu logowania i optymalizacja kodu
  - 37 zmienionych plików, 624 dodane linie, 91 usuniętych linii
- `f01c519` - Dodanie systemu logowania, aktualizacja interfejsu AI i widoków Web
  - 31 zmienionych plików, 1758 dodanych linii, 63 usunięte linie

### 2026-04-10

- `4962924` - Rozszerzenie obsługi WebSocket, widoku czatu i interakcji komunikatora
  - Ulepszenia menedżera kontekstu
  - Rozszerzenie systemu czatu
  - Aktualizacja interfejsu dostawcy komunikatora
  - Przeprojektowanie dostawcy WebUI
  - Aktualizacja kreatora JavaScript i routera
  - Optymalizacja widoku czatu
  - Ulepszenia obsługi WebSocket
  - 9 zmienionych plików, 365 dodanych linii, 134 usunięte linie

### 2026-04-09

- `f9302bf` - Rozszerzenie interfejsu dostawcy komunikatora, systemu czatu i interakcji Web UI
  - Rozszerzenie interfejsu dostawcy komunikatora
  - Ulepszenia wiadomości czatu i systemu
  - Optymalizacja menedżera kontekstu
  - Rozszerzenie domyślnej istoty krzemowej
  - Ulepszenia widoku czatu Web UI
  - Aktualizacja obsługi WebSocket
  - 10 zmienionych plików, 427 dodanych linii, 93 usunięte linie

### 2026-04-07

- `6831ee8` - Przeprojektowanie widoków Web i kreatora JavaScript
  - Kompletne przeprojektowanie kontrolerów Web
  - Całkowite przepisanie kreatora JavaScript
  - Aktualizacja wszystkich komponentów widoków
  - Ulepszenia systemu skórek
  - Podniesienie architektury klasy bazowej widoków
  - 23 zmienione pliki, 2004 dodane linie, 1983 usunięte linie

### 2026-04-05

- `41e97fb` - Aktualizacja wielu modułów rdzeniowych i kontrolerów Web
  - Ulepszenia menedżera kontekstu
  - System czatu i zarządzanie sesjami
  - Przeprojektowanie lokalizatora usług
  - Aktualizacja klasy bazowej istoty krzemowej i menedżera
  - Pełna aktualizacja kontrolerów Web (17 kontrolerów)
  - Ulepszenia fabryki domyślnej istoty krzemowej
  - 31 zmienionych plików, 681 dodanych linii, 326 usuniętych linii
- `67988d4` - Ulepszenie modułu Web UI, dodanie widoku wykonawców, czyszczenie widoków i modułów rdzeniowych
  - 61 zmienionych plików, 3148 dodanych linii, 3726 usuniętych linii

### 2026-04-04

- `b58bb1c` - Dodanie kontrolera inicjalizacji i przeprojektowanie modułu Web
  - Kontroler inicjalizacji
  - Przeprojektowanie modułu konfiguracji
  - Aktualizacja modułu lokalizacji
  - Ulepszenia systemu skórek
  - Rozszerzenie routera
  - 29 zmienionych plików, 1269 dodanych linii, 289 usuniętych linii
- `f03ac0b` - Dodanie modułu Web UI, ulepszenie funkcji komunikatora
  - 60 zmienionych plików, 8481 dodanych linii, 165 usuniętych linii

### 2026-04-03

- `192e57b` - Aktualizacja struktury projektu i komponentów rdzenia wykonawczego
  - 22 zmienione pliki, 446 dodanych linii, 179 usuniętych linii
- `59faec8` - Aktualizacja implementacji rdzeniowej i domyślnej
  - 25 zmienionych plików, 3056 dodanych linii, 18 usuniętych linii
- `d488485` - Dodanie kompilacji dynamicznej i modułu narzędzi kuratora
  - 19 zmienionych plików, 1727 dodanych linii, 11 usuniętych linii
- `753d1d9` - Dodanie modułu bezpieczeństwa, aktualizacja wykonawców, dostawców komunikatora, lokalizacji i narzędzi
  - 29 zmienionych plików, 2352 dodane linie, 93 usunięte linie
- `a378697` - Ukończenie etapu 5 - system narzędzi + wykonawcy
  - 41 zmienionych plików, 2651 dodanych linii, 363 usunięte linie

### 2026-04-02

- `e6ad94b` - Naprawa błędu ładowania historii czatu po usunięciu pliku konfiguracji podczas testów
  - 4 zmienione pliki, 49 dodanych linii, 45 usuniętych linii
- `daa56f5` - Ukończenie etapu 4: trwała pamięć (system czatu + kanał komunikatora)
  - 29 zmienionych plików, 2051 dodanych linii, 538 usuniętych linii

### 2026-04-01

- `bbe2dbb` - Naprawa ładowania konfiguracji i routingu wiadomości serwisowych czatu
  - 27 zmienionych plików, 1633 dodane linie, 147 usuniętych linii
- `2fa6305` - Implementacja etapu 2: ramka pętli głównej i system obiektów zegara
  - 9 zmienionych plików, 594 dodane linie, 41 usuniętych linii
- `32b99a1` - Implementacja etapu 1 - podstawowa funkcja czatu
  - 19 zmienionych plików, 1185 dodanych linii
- `358e368` - Pierwsze zatwierdzenie: dokumentacja projektu i licencja
  - 10 zmienionych plików, 1873 dodane linie
