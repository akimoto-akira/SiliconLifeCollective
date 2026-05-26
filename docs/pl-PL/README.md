![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**Wersja: v0.2.0-alpha** | **Kolektyw Życia Krzemowego** — platforma współpracy wieloagentowa oparta na .NET 9, w której agenty AI nazywane są **Istotami Krzemowymi**, realizująca samewolucję poprzez dynamiczną kompilację Roslyn.

[English](../README.md) | [Deutsch](../de-DE/README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Français](../fr-FR/README.md) | [Čeština](../cs-CZ/README.md) | [Italiano](../it-IT/README.md) | **Polski** | [Português](../pt-PT/README.md) | [Русский](../ru-RU/README.md)

## 🌟 Kluczowe cechy

### System agentów
- **Orkiestracja wieloagentowa** — zarządzana przez *Kuratora Krzemowego*, z mechanizmem sprawiedliwego planowania opartym na szczelinach czasowych sterowanych zegarem
- **Sterowanie Plikiem Duszy** — każda Istota Krzemowa jest napędzana przez główny plik podpowiedzi (`soul.md`), definiujący unikalną osobowość i wzorce zachowań
- **Architektura Ciało-Mózg** — *Ciało* (SiliconBeing) utrzymuje parametry życiowe i wykrywa scenariusze wyzwalania; *Mózg* (Menedżer Kontekstu) odpowiada za ładowanie historii, wywoływanie AI, wykonywanie narzędzi i trwałość odpowiedzi
- **Zdolność samewolucji** — poprzez technologię dynamicznej kompilacji Roslyn, Istoty Krzemowe mogą przepisywać swój własny kod, realizując ewolucję
- **Zarządzanie stanem aktywności** — obsługa czterech stanów aktywności: Idle (bezczynny), Working (pracujący), Error (błąd), Stopped (zatrzymany); po 10 kolejnych błędach automatycznie przechodzi w stan Stopped

### System wtyczek
- **Architektura rozszerzeń wtyczek** — rozszerzanie funkcjonalności poprzez interfejs IPlugin, obsługa dynamicznego ładowania bibliotek DLL wtyczek z katalogu
- **Bezpieczna piaskownica** — ładowarka wtyczek wykonuje rygorystyczne skanowanie bezpieczeństwa, zabraniając dostępu do przestrzeni nazw takich jak System.IO, System.Net
- **Ładowanie izolowane** — wykorzystanie niestandardowego AssemblyLoadContext do izolowanego ładowania, zapobiegające wpływowi wtyczek na stabilność programu głównego
- **Integracja narzędzi** — wtyczki mogą rejestrować niestandardowe narzędzia poprzez interfejs ITool, automatycznie integrując się z pętlą wywołań narzędzi

### Narzędzia i wykonywanie
- **24 wbudowane narzędzia** — obejmujące kalendarz, czat, konfigurację, dysk, sieć, pamięć, zadania, czasomierz, bazę wiedzy, notatki pracy, przestrzeń projektową, WebView, gorące przeładowanie itp.
- **Izolacja scenariuszy narzędzi** — każde narzędzie deklaruje dostępne scenariusze poprzez atrybut `ToolScenario` (Chat, Task, Timer, MemoryCompression, Project), atrybut `ChatOnly` ogranicza narzędzie wyłącznie do scenariusza czatu
- **Narzędzie gorącego przeładowania** — obsługa automatycznej kompilacji, aktualizacji plików i restartu SiliconLife.Fast w trakcie działania, bez konieczności ręcznej interwencji
- **Pętla wywołań narzędzi** — AI zwraca wywołanie narzędzia → wykonanie narzędzia → wynik przekazywany do AI → ciągła pętla aż do zwrócenia czystej tekstowej odpowiedzi
- **Bezpieczeństwo Wykonawca-Uprawnienia** — wszystkie operacje I/O przechodzą przez rygorystyczną weryfikację uprawnień wykonawcy
  - 3-poziomowy łańcuch weryfikacji uprawnień: Pamięć Podręczna Częstotliwości Użytkownika → Interfejs Wywołania Zwrotnego Uprawnień → (IsCurator: Obsługa Zapytań o Uprawnienia | Non-curator: Globalna ACL → domyślna odmowa)
  - Kompletny dziennik audytu rejestrujący wszystkie decyzje dotyczące uprawnień

### AI i wiedza
- **Obsługa wielu backendów AI**
  - **Ollama** — lokalne wdrażanie modeli, wykorzystujące natywne HTTP API
  - **Alibaba Cloud Bailian (DashScope)** — chmurowa usługa AI, kompatybilna z OpenAI API, obsługa 13+ modeli, wdrożenie wieloregionalne
  - **Volcengine Ark (VolcengineArk)** — chmurowa usługa AI ByteDance, obsługa trybu strumieniowego i niestrumieniowego, wbudowana kontrola szybkości
- **32 systemy kalendarzowe** — pełne pokrycie głównych kalendarzy świata, w tym gregoriański, chiński księżycowy, islamski, hebrajski, japoński, perski, majański, chińskie kalendarze historyczne itp.
- **System Sieci Wiedzy** — graf wiedzy oparty na trójkach (podmiot-relacja-obiekt), obsługujący przechowywanie, zapytania i odkrywanie ścieżek
- **Przestrzeń projektowa** — zarządzanie przestrzenią projektową, obsługa tworzenia/archiwizacji/niszczenia projektów, przypisywania ról, notatek pracy, śledzenia zadań i izolacji uprawnień narzędzi
- **Silnik przepływu pracy** — silnik maszyny stanów oparty na szablonach, obsługujący niestandardowe szablony przepływu pracy, przejścia stanów, wykonywanie sterowane Tick i zarządzanie cyklem życia instancji
- **Mechanizm zanikania pamięci** — usługa zanikania (MemoryFadeService), automatycznie co godzinę przeprowadzająca spadek ważności i automatyczną archiwizację pamięci wszystkich Istot Krzemowych

### Interfejs Web
- **Nowoczesny Web UI** — wbudowany serwer HTTP, obsługa aktualizacji w czasie rzeczywistym SSE
- **7 motywów skórek** — wersja administracyjna, czat, twórcza, deweloperska, wysoki kontrast, jasna, minimalistyczna, obsługa automatycznego wykrywania i przełączania
- **24 kontrolery** — kompletne zarządzanie systemem, czat, konfiguracja, funkcje monitorowania
- **Brak zależności od frameworków frontendowych** — generowanie HTML/CSS/JS po stronie serwera za pomocą `H`, `CssBuilder` i `JsBuilder`

### Internacjonalizacja i lokalizacja
- **34 warianty językowe** pełne wsparcie, obejmujące 2 systemy pisma i wiele wariantów regionalnych
  - **Chiński uproszczony**: zh-CN (Chiny kontynentalne), zh-SG (Singapur), zh-MY (Malezja) (3)
  - **Chiński tradycyjny**: zh-HK (Hongkong), zh-TW (Tajwan), zh-MO (Makau) (3)
  - **Angielski**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10)
  - **Hiszpański**: es-ES, es-MX (2)
  - **Niemiecki**: de-DE, de-AT, de-CH, de-LU, de-LI (5)
  - **Francuski**: fr-FR, fr-CA, fr-CH (3)
  - **Japoński**: ja-JP | **Koreański**: ko-KR | **Czeski**: cs-CZ (3)
  - **Włoski**: it-IT | **Polski**: pl-PL | **Portugalski**: pt-PT, pt-BR (4)

### Dane i przechowywanie
- **Wysokowydajne przechowywanie SpeedyPack** — wersja Fast wykorzystuje autorski silnik przechowywania .spk, mapowanie katalogów w pamięci + pamięć podręczna wpisów + asynchroniczna kolejka zapisu
- **Przechowywanie systemu plików** — wersja Default wykorzystuje przechowywanie JSON w czystym systemie plików
- **Zapytania indeksowane czasowo** — poprzez interfejs `ITimeStorage` obsługa wydajnych zapytań według zakresu czasu
- **Automatyczna kompakcja** — SpeedyPack obsługuje okresową automatyczną kompakcję, odzyskując wolną przestrzeń
- **Minimalne zależności** — biblioteka główna zależy wyłącznie od Microsoft.CodeAnalysis.CSharp do kompilacji dynamicznej

## 🔄 Architektura dwuwersyjna

Projekt oferuje dwie wersje implementacji, spełniające różne potrzeby scenariuszowe:

### SiliconLife.Default (wersja domyślna)
- **Pozycjonowanie**: domyślna implementacja, głównie do weryfikacji wykonalności architektury
- **Tryb działania**: aplikacja konsolowa
- **Metoda przechowywania**: przechowywanie JSON w systemie plików
- **Scenariusze zastosowania**: wysokie wymagania bezpieczeństwa danych, ograniczone zasoby pamięci, mała ilość danych
- **Cechy**: prosta i niezawodna, natychmiastowa trwałość danych, brak ryzyka utraty pamięci
- **Opis roli**: jako implementacja odniesienia do weryfikacji architektury, odpowiednia do pierwszego kontaktu, debugowania deweloperskiego lub scenariuszy priorytetowego bezpieczeństwa danych
- **Polecenie uruchomienia**: `dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast (wersja wysokowydajna)
- **Pozycjonowanie**: zalecana wersja produkcyjna
- **Tryb działania**: aplikacja desktopowa (Windows/macOS zasobnik systemowy / Linux okno statusu)
- **Metoda przechowywania**: przechowywanie w pamięci SpeedyPack + asynchroniczna trwałość wsadowa (format pliku .spk)
- **Scenariusze zastosowania**: wysoka współbieżność, niskie opóźnienie, duże ilości danych
- **Obsługa platform**: Windows/macOS (pełna funkcjonalność, z zasobnikiem systemowym), Linux (okno statusu, bez ikony zasobnika)
- **Cechy**:
  - Ekstremalna optymalizacja wydajności
  - Windows/macOS działanie w tle w zasobniku, obsługa monitorowania w czasie rzeczywistym przez okno statusu zasobnika; Linux bezpośrednie wyświetlanie okna statusu
  - Silnik SpeedyPack + automatyczna kompakcja gwarantująca bezpieczeństwo danych
  - Architektura Component UI, 27 komponentów deklaratywnych
  - 7 motywów skórek, obsługa automatycznego wykrywania i przełączania
  - Narzędzie gorącego przeładowania obsługujące aktualizacje online i restart
- **Poprawa wydajności**: opóźnienie odczytu przechowywania zmniejszone 1000-krotnie, opóźnienie zapisu zmniejszone 15000-krotnie, zdolność obsługi współbieżnej zwiększona 50-krotnie
- **Opis roli**: głęboko zoptymalizowana implementacja produkcyjna, zalecana do długotrwałego działania i rzeczywistych środowisk produkcyjnych
- **Polecenie uruchomienia**: `dotnet run --project src/SiliconLife.Fast`

### Porównanie wersji

| Cecha | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| **Tryb działania** | Aplikacja konsolowa | Aplikacja desktopowa (Windows/macOS zasobnik systemowy / Linux okno statusu) |
| **Interfejs użytkownika** | Web UI (dostęp przez przeglądarkę) | Windows/macOS: ikona zasobnika + okno zasobnika + Web UI; Linux: okno statusu + Web UI |
| **Zasobnik systemowy** | ❌ Brak | ✅ Windows/macOS obsługa minimalizacji do zasobnika; Linux bez ikony zasobnika |
| **Działanie w tle** | ❌ Zamknięcie konsoli = wyjście | ✅ Windows/macOS ciągłe działanie w tle zasobnika; Linux działanie okna statusu |
| **Metoda przechowywania** | Przechowywanie JSON w systemie plików | Przechowywanie w pamięci SpeedyPack + asynchroniczna trwałość |
| **Silnik przechowywania** | I/O systemu plików | SiliconLife.Speedy (format .spk) |
| **Opóźnienie odczytu** | ~10ms (I/O dysku) | ~0.01ms (operacja w pamięci) |
| **Opóźnienie zapisu** | ~15ms (zapis synchroniczny) | ~0.001ms (zapis asynchroniczny) |
| **Zdolność współbieżna** | ~100 req/s | ~5000 req/s |
| **Zużycie pamięci** | ~200MB | ~500MB |
| **Bezpieczeństwo danych** | Bardzo wysokie (natychmiastowa trwałość) | Wysokie (asynchroniczna trwałość + automatyczna kompakcja) |
| **Scenariusze zastosowania** | Priorytet bezpieczeństwa danych, mała ilość danych | Priorytet wydajności, duże ilości danych, wysoka współbieżność |

## 🛠️ Stos technologiczny

| Komponent | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| Środowisko uruchomieniowe | .NET 9 | .NET 9 (Windows/macOS/Linux) |
| Język programowania | C# | C# |
| Typ aplikacji | Aplikacja konsolowa | Aplikacja desktopowa (Windows/macOS zasobnik systemowy / Linux okno statusu) |
| Integracja AI | Ollama (lokalna), Alibaba Cloud Bailian (chmura), Volcengine Ark (chmura) | Ollama (lokalna), Alibaba Cloud Bailian (chmura), Volcengine Ark (chmura) |
| Przechowywanie danych | System plików (JSON + katalogi indeksowane czasowo) | SpeedyPack (format .spk, mapowanie pamięci + asynchroniczna trwałość) |
| Serwer Web | HttpListener (wbudowany w .NET) | HttpListener (wbudowany w .NET) |
| Kompilacja dynamiczna | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Automatyzacja przeglądarki | Playwright (WebView) | Playwright (WebView) |
| System wtyczek | ✅ Obsługa (IPlugin + PluginLoader) | ✅ Obsługa (IPlugin + PluginLoader) |
| Zasobnik systemowy | ❌ Brak obsługi | ✅ Windows/macOS obsługa (NotifyIcon); Linux bez ikony zasobnika |
| Licencja | Apache-2.0 | Apache-2.0 |

## 📁 Struktura projektu

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Biblioteka główna (interfejsy, klasy abstrakcyjne)
│   │   ├── AI/                            # Interfejsy klientów AI, Menedżer Kontekstu, modele wiadomości
│   │   ├── Audit/                         # System audytu wykorzystania tokenów
│   │   ├── Chat/                          # System czatu, zarządzanie sesjami, kanał transmisyjny
│   │   ├── Compilation/                   # Kompilacja dynamiczna, skanowanie bezpieczeństwa, szyfrowanie kodu
│   │   ├── Config/                        # System zarządzania konfiguracją
│   │   ├── Executors/                     # Wykonawcy (dyskowy, sieciowy, wiersza poleceń)
│   │   ├── IM/                            # Interfejs dostawcy komunikatora
│   │   ├── Knowledge/                     # System Sieci Wiedzy
│   │   ├── Localization/                  # System lokalizacji
│   │   ├── Logging/                       # System logowania
│   │   ├── Plugins/                       # System wtyczek (interfejs IPlugin, ładowarka PluginLoader)
│   │   ├── Project/                       # System zarządzania projektami
│   │   ├── Runtime/                       # Pętla główna, obiekty Tick, główny host
│   │   ├── Security/                      # System zarządzania uprawnieniami
│   │   ├── SiliconBeing/                  # Klasa bazowa Istoty Krzemowej, menedżer, fabryka
│   │   ├── Storage/                       # Interfejsy przechowywania
│   │   ├── Time/                          # Niepełna data (zapytania zakresu czasu)
│   │   ├── Tools/                         # Interfejsy narzędzi i Menedżer Narzędzi
│   │   ├── WebView/                       # Interfejs przeglądarki WebView
│   │   ├── Workflow/                      # Silnik przepływu pracy (szablony, instancje, przejścia stanów)
│   │   └── ServiceLocator.cs              # Globalny lokalizator usług
│   │
│   ├── SiliconLife.Common/                # Współdzielone implementacje (wspólne dla obu wersji)
│   │   ├── AI/                            # Klienci AI i fabryki (Ollama, DashScope, VolcengineArk)
│   │   ├── Calendar/                      # 32 implementacje kalendarzy
│   │   ├── Localization/                  # Baza lokalizacji i 34 implementacje wariantów językowych/regionalnych
│   │   ├── Resources/                     # Współdzielone pliki zasobów
│   │   ├── Security/                      # Menedżer Uprawnień
│   │   ├── SiliconBeing/                  # Domyślna implementacja Istoty Krzemowej
│   │   ├── Tools/                         # 23 implementacje narzędzi ogólnych
│   │   ├── Web/                           # Infrastruktura Web
│   │   └── WebView/                       # Implementacja Playwright WebView
│   │
│   ├── SiliconLife.App/                   # Warstwa aplikacji (Web UI + dokumentacja pomocy, współdzielona przez Default i Fast)
│   │   ├── Config/                        # Konfiguracja aplikacji
│   │   ├── Data/                          # Katalog danych
│   │   ├── Help/                          # Lokalizacja dokumentacji pomocy (wielojęzyczna)
│   │   ├── Tools/                         # HelpTool (narzędzie zapytań dokumentacji pomocy)
│   │   └── Web/                           # Implementacja Web UI
│   │       ├── Component/                 # Biblioteka komponentów UI (27 komponentów)
│   │       ├── Controllers/               # 24 kontrolery
│   │       ├── Models/                    # Modele widoków
│   │       ├── Views/                     # Widoki HTML
│   │       └── Skins/                     # 7 motywów skórek
│   │
│   ├── SiliconLife.Default/               # Domyślna implementacja + punkt wejścia aplikacji (wersja konsolowa)
│   │   ├── Program.cs                     # Punkt wejścia (asemblacja wszystkich komponentów)
│   │   ├── Config/                        # Domyślne dane konfiguracyjne
│   │   ├── Knowledge/                     # Implementacja Sieci Wiedzy
│   │   ├── Logging/                       # Implementacja dostawców logowania (konsola + system plików)
│   │   ├── Project/                       # Implementacja systemu projektów
│   │   └── Storage/                       # Implementacja przechowywania w systemie plików
│   │
│   ├── SiliconLife.Fast/                  # Wysokowydajna implementacja + punkt wejścia aplikacji (wersja okienkowa)
│   │   ├── Program.cs                     # Punkt wejścia (aplikacja okienkowa)
│   │   ├── App.axaml / App.cs             # Definicja aplikacji Avalonia
│   │   ├── Config/                        # Dane konfiguracyjne (współdzielone z Default)
│   │   ├── Knowledge/                     # Implementacja Sieci Wiedzy (optymalizacja pamięci)
│   │   ├── Logging/                       # Wysokowydajny dostawca logowania
│   │   ├── Project/                       # Implementacja systemu projektów
│   │   ├── Storage/                       # Adaptery przechowywania SpeedyPack
│   │   └── Tray/                          # Zasobnik systemowy (lokalizacja 34 wariantów językowych)
│   │
│   ├── SiliconLife.Speedy/                # Wysokowydajny silnik przechowywania SpeedyPack
│   │   ├── SpeedyPack.cs                  # Klasa główna (mapowanie katalogów w pamięci + pamięć podręczna + asynchroniczny zapis)
│   │   ├── SpeedyPackOptions.cs           # Opcje konfiguracji (TTL pamięci podręcznej, maks. liczba wpisów itp.)
│   │   ├── IPackTransaction.cs            # Interfejs transakcji
│   │   ├── SpkFileInfo.cs                 # Informacje o pliku
│   │   └── Internal/                      # Implementacja wewnętrzna
│   │       ├── DirectoryMap.cs            # Mapowanie katalogów w pamięci
│   │       ├── EntryCache.cs              # Pamięć podręczna wpisów
│   │       ├── FreeList.cs                # Zarządzanie wolną przestrzenią
│   │       ├── PackFileReader.cs          # Czytnik plików pakietu
│   │       ├── PackFileWriter.cs          # Zapisywacz plików pakietu
│   │       ├── WriteQueue.cs              # Asynchroniczna kolejka zapisu
│   │       ├── WriteOperation.cs          # Operacja zapisu
│   │       ├── SpeedyTransaction.cs       # Implementacja transakcji
│   │       ├── SpkHeader.cs               # Nagłówek pliku pakietu
│   │       └── PathNormalizer.cs          # Normalizacja ścieżek
│   │
│   └── SiliconLife.Speedy.Manager/        # Narzędzie zarządzania SpeedyPack (Avalonia UI)
│       ├── MainForm.cs                    # Główny formularz
│       ├── Program.cs                     # Punkt wejścia
│       └── slc.ico                        # Ikona aplikacji
│
├── docs/                                  # Dokumentacja wielojęzyczna
│   ├── zh-CN/                             # Dokumentacja chińska uproszczona
│   ├── en/                                # Dokumentacja angielska
│   └── ...                                # Dokumentacja w innych językach
│
└── 总文档/                                 # Dokumentacja wymagań i architektury
    ├── 需求文档.md
    ├── 架构大纲.md
    └── 实现顺序.md
```

## 🏗️ Przegląd architektury

### Architektura planowania
```
Pętla główna (dedykowany wątek, watchdog + circuit breaker)
  └── Obiekty Tick (posortowane według priorytetu)
       └── Menedżer Istot Krzemowych
            └── Runner Istot Krzemowych (wątek tymczasowy, timeout + circuit breaker)
                 └── Istota Krzemowa.Tick()
                      └── Menedżer Kontekstu.Myśl()
                           └── Klient AI.Czatuj()
                                └── Pętla wywołań narzędzi → trwałość w systemie czatu
```

### Architektura bezpieczeństwa
Wszystkie operacje I/O inicjowane przez AI muszą przejść przez rygorystyczny łańcuch bezpieczeństwa:

```
Wywołanie narzędzia → Wykonawca → Menedżer Uprawnień → [Pamięć podręczna częstotliwości → Wywołanie zwrotne → (IsCurator: zapytaj użytkownika | Non-curator: Globalna ACL)]
```

## 🚀 Szybki start

### Wymagania wstępne

- **.NET 9 SDK** — [Link do pobrania](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Backend AI** (wybierz jeden):
  - **Ollama**: [Zainstaluj Ollama](https://ollama.com) i pobierz model (np. `ollama pull llama3`)
  - **Alibaba Cloud Bailian**: uzyskaj klucz API z [konsoli Bailian](https://bailian.console.aliyun.com/)
  - **Volcengine Ark**: uzyskaj klucz API z [konsoli Volcengine](https://console.volcengine.com/ark)

### Budowanie projektu

```bash
dotnet restore
dotnet build
```

### Uruchamianie systemu

#### Sposób 1: Uruchomienie wersji Default (aplikacja konsolowa)

```bash
dotnet run --project src/SiliconLife.Default
```

Aplikacja uruchomi serwer Web i automatycznie otworzy Web UI w przeglądarce.

**Scenariusze zastosowania**:
- ✅ Bardzo wysokie wymagania bezpieczeństwa danych
- ✅ Ograniczone zasoby pamięci (RAM < 2GB)
- ✅ Mała ilość danych, krótkotrwałe użytkowanie
- ✅ Faza debugowania deweloperskiego

#### Sposób 2: Uruchomienie wersji Fast (aplikacja desktopowa)

```bash
dotnet run --project src/SiliconLife.Fast
```

**Windows/macOS**: aplikacja uruchomi się w trybie okienkowym, zminimalizuje do zasobnika systemowego i będzie działać w tle.

**Linux**: aplikacja wyświetli okno statusu (bez ikony zasobnika systemowego) i automatycznie otworzy przeglądarkę z Web UI. Można również użyć parametru `--no-tray`, aby pominąć automatyczne otwieranie przeglądarki:

```bash
dotnet run --project src/SiliconLife.Fast -- --no-tray
```

**Scenariusze zastosowania**:
- ✅ Scenariusze wysokiej współbieżności (> 5 użytkowników)
- ✅ Duże ilości danych (użytkowanie powyżej 3 miesięcy)
- ✅ Potrzeba odpowiedzi o niskim opóźnieniu
- ✅ Potrzeba działania w tle w zasobniku systemowym

### Publikacja jako pojedynczy plik

```bash
# Windows - wersja Default
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Windows - wersja Fast
dotnet publish src/SiliconLife.Fast -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux - wersja Default
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# Linux - wersja Fast
dotnet publish src/SiliconLife.Fast -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS - wersja Default
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true

# macOS - wersja Fast
dotnet publish src/SiliconLife.Fast -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 Plan rozwoju

### ✅ Ukończone
- [x] Etap 1: Konsolowy czat AI
- [x] Etap 2: Szkielet frameworka (pętla główna + obiekty Tick + watchdog + circuit breaker)
- [x] Etap 3: Pierwsza Istota Krzemowa z Plikiem Duszy (architektura Ciało-Mózg)
- [x] Etap 4: Trwała pamięć (system czatu + interfejs Pamięci Czasowej)
- [x] Etap 5: System narzędzi + Wykonawcy
- [x] Etap 6: System uprawnień (łańcuch 5 poziomów, audytor logów, Globalna ACL)
- [x] Etap 7: Kompilacja dynamiczna + samewolucja (Roslyn)
- [x] Etap 8: Pamięć długoterminowa + zadania + czasomierze
- [x] Etap 9: Główny Host + współpraca wieloagentowa
- [x] Etap 10: Web UI (HTTP + SSE, 24 kontrolery, 7 skórek)
- [x] Etap 10.5: Ulepszenia przyrostowe (kanał transmisyjny, audyt tokenów, 32 kalendarze, ulepszenia narzędzi, lokalizacja 34 wariantów językowych)
- [x] Etap 10.6: Doskonalenie i optymalizacja (WebView, system pomocy, przestrzeń projektowa, Sieć Wiedzy, silnik przepływu pracy)
- [x] Etap 11: Silnik przechowywania SpeedyPack (zastąpienie LiteDB, mapowanie pamięci, asynchroniczna kolejka zapisu, automatyczna kompakcja)
- [x] Etap 12: System wtyczek (interfejs IPlugin, Bezpieczna Piaskownica PluginLoader, ładowanie izolowane, integracja narzędzi)

### 🚧 W planach
- [ ] Etap 13: Integracja z zewnętrznymi komunikatorami (Feishu / WhatsApp / Telegram)
- [ ] Etap 14: Ekosystem umiejętności (marketplace wtyczek, dystrybucja pakietów umiejętności)

## 📚 Dokumentacja

- [Projekt architektury](architecture.md) — projekt systemu, mechanizm planowania, architektura komponentów
- [Model bezpieczeństwa](security.md) — model uprawnień, Wykonawcy, bezpieczeństwo kompilacji dynamicznej
- [Przewodnik deweloperski](development-guide.md) — rozwój narzędzi, przewodnik rozszerzeń
- [Referencja API](api-reference.md) — dokumentacja endpointów Web API
- [Referencja narzędzi](tools-reference.md) — szczegółowy opis wbudowanych narzędzi
- [Przewodnik Web UI](web-ui-guide.md) — przewodnik korzystania z interfejsu Web
- [Przewodnik Istoty Krzemowej](silicon-being-guide.md) — przewodnik rozwoju agentów
- [System uprawnień](permission-system.md) — szczegółowy opis zarządzania uprawnieniami
- [System kalendarza](calendar-system.md) — opis 32 systemów kalendarzowych
- [Szybki start](getting-started.md) — szczegółowy przewodnik dla początkujących
- [Rozwiązywanie problemów](troubleshooting.md) — odpowiedzi na częste pytania
- [Plan rozwoju](roadmap.md) — pełny plan rozwoju
- [Dziennik zmian](changelog.md) — historia aktualizacji wersji
- [Przewodnik współpracy](contributing.md) — jak uczestniczyć w projekcie

## 🤝 Współpraca

Witamy wszelkie formy współpracy! Szczegóły znajdują się w [Przewodniku współpracy](contributing.md).

### Przepływ pracy deweloperskiej
1. Forkuj to repozytorium
2. Utwórz gałąź funkcjonalną (`git checkout -b feature/AmazingFeature`)
3. Zatwierdź zmiany (`git commit -m 'feat: add some AmazingFeature'`)
4. Wypchnij do gałęzi (`git push origin feature/AmazingFeature`)
5. Złóż Pull Request

## 💡 Przewodnik wyboru wersji

### Której wersji powinienem używać?

**SiliconLife.Default (domyślna implementacja — weryfikacja wykonalności architektury):**
- 📌 Pierwszy kontakt z tym projektem, chcesz szybko poznać architekturę systemu
- 📌 Jesteś w fazie debugowania deweloperskiego, potrzebujesz prostego i bezpośredniego sposobu uruchomienia
- 📌 Bezpieczeństwo danych jest Twoim priorytetem
- 📌 Twój system ma mniej niż 4GB pamięci
- 📌 Potrzebujesz tylko użytkowania jednoosobowego lub masz mało danych

**SiliconLife.Fast (zalecana wersja produkcyjna):**
- ⚡ Potrzebujesz stabilnie działającego środowiska produkcyjnego przez długi czas
- ⚡ Znasz już architekturę systemu i jesteś gotowy do wdrożenia produkcyjnego
- ⚡ Musisz obsługiwać współbieżny dostęp wielu użytkowników
- ⚡ Potrzebujesz działania w tle w zasobniku systemowym
- ⚡ Zależy Ci na ekstremalnej wydajności

> **Ogólna rekomendacja**: SiliconLife.Default jest odpowiedni jako weryfikacja architektury i wprowadzenie; dla rzeczywistych środowisk produkcyjnych zdecydowanie zalecamy SiliconLife.Fast.

### Czy można migrować z Default do Fast?

**Oczywiście!** Obie wersje współdzielą te same:
- ✅ Format plików konfiguracyjnych (config.json)
- ✅ Interfejsy narzędzi
- ✅ Konfiguracja Being
- ✅ Interfejs Web UI

**Kroki migracji:**
1. Utwórz kopię zapasową katalogu danych Default
2. Uruchom wersję Fast z tym samym katalogiem danych
3. Fast automatycznie zaimportuje istniejące dane do silnika przechowywania SpeedyPack
4. Po weryfikacji poprawnego działania, możesz na co dzień używać wersji Fast

### Czy obie wersje mogą współistnieć?

**Tak!** Zalecana strategia wdrożenia:

**Strategia 1: Default do weryfikacji, Fast do produkcji**
```
Środowisko deweloperskie/weryfikacyjne: SiliconLife.Default (weryfikacja architektury, debugowanie funkcji)
Środowisko produkcyjne: SiliconLife.Fast (wysoka wydajność, działanie w tle, obsługa żądań w czasie rzeczywistym)
```

**Strategia 2: Fast jako główne uruchomienie, Default do okresowych kopii zapasowych**
```
SiliconLife.Fast (codzienne użytkowanie, obsługa żądań w czasie rzeczywistym)
    ↓ Okresowa kopia zapasowa
SiliconLife.Default (archiwizacja zimnych danych, zabezpieczenie bezpieczeństwa danych)
```

## 📄 Licencja

Projekt jest objęty licencją Apache License 2.0 — szczegóły w pliku [LICENSE](../../LICENSE).

## 👨‍💻 Autor

**天源垦骥**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- Gitee: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili: [617827040](https://space.bilibili.com/617827040)

## 🙏 Podziękowania

Podziękowania dla wszystkich deweloperów i dostawców platform AI, którzy wnieśli swój wkład w ten projekt.

---

**Silicon Life Collective** — spraw, by agenty AI naprawdę "ożyły"
