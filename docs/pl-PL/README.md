![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**Wersja: v0.2.0-alpha** | **Kolektyw Życia Krzemowego** — platforma wieloagentowej współpracy oparta na .NET 9, w której agenty AI nazywane **Istotami Krzemowymi**, ewoluują samodzielnie dzięki dynamicznej kompilacji Roslyn.

[English](../en/README.md) | [Deutsch](../de-DE/README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Français](../fr-FR/README.md) | [Čeština](../cs-CZ/README.md) | [Italiano](../it-IT/README.md) | [Português](../pt-PT/README.md) | **Polski**

## 🌟 Kluczowe cechy

### System agentów
- **Orkiestracja wieloagentowa** — zarządzana przez *Kuratora Krzemowego*, z mechanizmem sprawiedliwego harmonogramowania opartym na zegarze
- **Sterowanie plikiem duszy** — każda Istota Krzemowa jest napędzana przez rdzenny plik podpowiedzi (`soul.md`), definiujący unikalną osobowość i wzorce zachowań
- **Architektura ciało-mózg** — *Ciało* (SiliconBeing) utrzymuje parametry życiowe i wykrywa scenariusze wyzwalające; *Mózg* (ContextManager) odpowiada za ładowanie historii, wywoływanie AI, wykonywanie narzędzi i utrwalanie odpowiedzi
- **Zdolność samewolucji** — dzięki dynamicznej kompilacji Roslyn, Istoty Krzemowe mogą przepisywać swój własny kod, realizując ewolucję
- **Zarządzanie stanem aktywności** — obsługa czterech stanów aktywności: Idle (bezczynny), Working (pracujący), Error (błąd), Stopped (zatrzymany); po 10 kolejnych błędach automatyczne przejście do stanu Stopped

### System wtyczek
- **Architektura rozszerzeń wtyczek** — rozszerzanie funkcjonalności poprzez interfejs IPlugin, obsługa dynamicznego ładowania bibliotek DLL z katalogu
- **Bezpieczna piaskownica** — moduł ładujący wtyczki wykonuje rygorystyczne skanowanie bezpieczeństwa, zabraniając dostępu do przestrzeni nazw takich jak System.IO, System.Net
- **Izolowane ładowanie** — użycie niestandardowego AssemblyLoadContext do izolowanego ładowania, zapobiegające wpływowi wtyczek na stabilność programu głównego
- **Integracja narzędzi** — wtyczki mogą rejestrować niestandardowe narzędzia poprzez interfejs ITool, automatycznie integrując się z pętlą wywołań narzędzi

### Narzędzia i wykonywanie
- **24 wbudowane narzędzia** — obejmujące kalendarz, czat, konfigurację, dysk, sieć, pamięć, zadania, czasomierze, bazę wiedzy, notatki pracy, przeglądarkę WebView, gorące przeładowanie i inne
- **Narzędzie gorącego przeładowania** — obsługa automatycznej kompilacji, aktualizacji plików i restartu SiliconLife.Fast podczas działania, bez ręcznej interwencji
- **Pętla wywołań narzędzi** — AI zwraca wywołanie narzędzia → wykonanie narzędzia → wynik przekazywany do AI → ciągła pętla aż do zwrócenia czystej odpowiedzi tekstowej
- **Bezpieczeństwo wykonawcy-uprawnień** — wszystkie operacje I/O przechodzą przez wykonawcę z rygorystyczną weryfikacją uprawnień
  - Łańcuch uprawnień 5 poziomów: IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - Kompletny dziennik audytu rejestrujący wszystkie decyzje dotyczące uprawnień

### AI i wiedza
- **Obsługa wielu backendów AI**
  - **Ollama** — lokalne wdrażanie modeli, korzystające z natywnego API HTTP
  - **Alibaba Cloud Bailian (DashScope)** — usługa chmurowa AI, kompatybilna z API OpenAI, obsługująca 13+ modeli, wdrożenie wieloregionowe
  - **Volcengine Ark** — usługa chmurowa AI ByteDance, obsługująca tryb strumieniowy i niestrumieniowy, z wbudowaną kontrolą szybkości
- **32 systemy kalendarzowe** — pełne pokrycie głównych kalendarzy na świecie, w tym gregoriański, księżycowy, islamski, hebrajski, japoński, perski, majański, chiński historyczny i inne
- **System sieci wiedzy** — graf wiedzy oparty na trójkach (podmiot-relacja-obiekt), obsługujący przechowywanie, zapytania i odkrywanie ścieżek

### Interfejs Web
- **Nowoczesny Web UI** — wbudowany serwer HTTP, obsługa aktualizacji w czasie rzeczywistym SSE
- **7 motywów skórek** — zarządczy, czatowy, twórczy, deweloperski, wysoki kontrast, jasny, minimalistyczny, obsługa automatycznego wykrywania i przełączania
- **20+ kontrolerów** — kompletna funkcjonalność zarządzania systemem, czatu, konfiguracji, monitorowania
- **Zero zależności od frameworków frontendowych** — generowanie HTML/CSS/JS po stronie serwera poprzez `H`, `CssBuilder` i `JsBuilder`

### Internacjonalizacja i lokalizacja
- **29 implementacji językowych** pełne wsparcie, obejmujące 2 systemy pisma i wiele wariantów regionalnych
  - **Chiński uproszczony**: zh-CN (Chiny kontynentalne), zh-SG (Singapur), zh-MY (Malezja) (3)
  - **Chiński tradycyjny**: zh-HK (Hongkong), zh-TW (Tajwan), zh-MO (Makau) (3)
  - **Angielski**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10)
  - **Hiszpański**: es-ES, es-MX (2)
  - **Niemiecki**: de-DE, de-AT, de-CH, de-LU, de-LI (5)
  - **Francuski**: fr-FR, fr-CA, fr-CH (3)
  - **Japoński**: ja-JP | **Koreański**: ko-KR | **Czeski**: cs-CZ (3)
  - **Polski**: pl-PL (1)

### Dane i przechowywanie
- **Wysokowydajna pamięć SpeedyPack** — wersja Fast korzysta z autorskiego silnika przechowywania .spk, mapowanie katalogów w pamięci + pamięć podręczna wpisów + asynchroniczna kolejka zapisu
- **Przechowywanie w systemie plików** — wersja Default korzysta z czystego przechowywania JSON w systemie plików
- **Zapytania z indeksem czasowym** — obsługa wydajnych zapytań według zakresu czasu poprzez interfejs `ITimeStorage`
- **Automatyczna kompresja** — SpeedyPack obsługuje okresową automatyczną kompresję, odzyskując wolną przestrzeń
- **Minimalne zależności** — biblioteka rdzenna zależy tylko od Microsoft.CodeAnalysis.CSharp do kompilacji dynamicznej

## 🔄 Architektura dwóch wersji

Ten projekt oferuje dwie wersje implementacji, spełniające różne scenariusze:

### SiliconLife.Default (wersja domyślna)
- **Przeznaczenie**: domyślna implementacja, głównie do weryfikacji wykonalności architektury
- **Tryb działania**: aplikacja konsolowa
- **Sposób przechowywania**: czyste przechowywanie JSON w systemie plików
- **Scenariusze zastosowania**: wysokie wymagania dotyczące bezpieczeństwa danych, ograniczone zasoby pamięci, mała ilość danych
- **Cechy**: prosta i niezawodna, natychmiastowa trwałość danych, brak ryzyka utraty pamięci
- **Opis roli**: jako referencyjna implementacja weryfikacji architektury, odpowiednia do pierwszego kontaktu, debugowania rozwoju lub scenariuszy z priorytetem bezpieczeństwa danych
- **Polecenie uruchomienia**: `dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast (wersja wysokowydajna)
- **Przeznaczenie**: główna wersja produkcyjna
- **Tryb działania**: aplikacja desktopowa (Windows/macOS zasobnik systemowy / Linux okno stanu)
- **Sposób przechowywania**: pamięć SpeedyPack + asynchroniczna trwałość wsadowa (format pliku .spk)
- **Scenariusze zastosowania**: wysoka współbieżność, niskie opóźnienie, duże ilości danych
- **Obsługa platform**: Windows/macOS (pełne funkcje, w tym zasobnik systemowy), Linux (okno stanu, brak ikony zasobnika)
- **Cechy**:
  - Ekstremalna optymalizacja wydajności
  - Windows/macOS działanie w tle w zasobniku z monitorowaniem w czasie rzeczywistym; Linux okno stanu wyświetlane bezpośrednio
  - Silnik SpeedyPack + automatyczna kompresja gwarantująca bezpieczeństwo danych
  - Architektura Component UI, 30+ deklaratywnych komponentów
  - 7 motywów skórek, obsługa automatycznego wykrywania i przełączania
  - Narzędzie gorącego przeładowania obsługujące aktualizacje online i restart
- **Poprawa wydajności**: opóźnienie odczytu przechowywania zmniejszone 1000-krotnie, opóźnienie zapisu zmniejszone 15000-krotnie, zdolność obsługi współbieżnej zwiększona 50-krotnie
- **Opis roli**: głęboko zoptymalizowana implementacja produkcyjna, preferowana do długotrwałego działania i rzeczywistych środowisk produkcyjnych
- **Polecenie uruchomienia**: `dotnet run --project src/SiliconLife.Fast`

### Porównanie wersji

| Cecha | SiliconLife.Default | SiliconLife.Fast |
|-------|---------------------|------------------|
| **Tryb działania** | Aplikacja konsolowa | Aplikacja desktopowa (Windows/macOS zasobnik systemowy / Linux okno stanu) |
| **Interfejs użytkownika** | Web UI (dostęp przez przeglądarkę) | Windows/macOS: Ikona zasobnika + okno zasobnika + Web UI; Linux: Okno stanu + Web UI |
| **Zasobnik systemowy** | ❌ Brak | ✅ Windows/macOS obsługa minimalizacji do zasobnika; Linux brak ikony zasobnika |
| **Działanie w tle** | ❌ Zamknięcie konsoli = wyjście | ✅ Windows/macOS ciągłe działanie w tle w zasobniku; Linux działanie w oknie stanu |
| **Sposób przechowywania** | Przechowywanie JSON w systemie plików | Pamięć SpeedyPack + asynchroniczna trwałość |
| **Silnik przechowywania** | I/O systemu plików | SiliconLife.Speedy (format .spk) |
| **Opóźnienie odczytu** | ~10ms (I/O dysku) | ~0.01ms (operacja w pamięci) |
| **Opóźnienie zapisu** | ~15ms (zapis synchroniczny) | ~0.001ms (zapis asynchroniczny) |
| **Zdolność współbieżna** | ~100 req/s | ~5000 req/s |
| **Zużycie pamięci** | ~200MB | ~500MB |
| **Bezpieczeństwo danych** | Bardzo wysokie (natychmiastowa trwałość) | Wysokie (asynchroniczna trwałość + automatyczna kompresja) |
| **Scenariusze zastosowania** | Priorytet bezpieczeństwa danych, małe ilości danych | Priorytet wydajności, duże ilości danych, wysoka współbieżność |

## 🛠️ Stos technologiczny

| Komponent | SiliconLife.Default | SiliconLife.Fast |
|-----------|---------------------|------------------|
| Środowisko uruchomieniowe | .NET 9 | .NET 9 (Windows/macOS/Linux) |
| Język programowania | C# | C# |
| Typ aplikacji | Aplikacja konsolowa | Aplikacja desktopowa (Windows/macOS zasobnik systemowy / Linux okno stanu) |
| Integracja AI | Ollama (lokalne), Alibaba Cloud Bailian (chmura), Volcengine Ark (chmura) | Ollama (lokalne), Alibaba Cloud Bailian (chmura), Volcengine Ark (chmura) |
| Przechowywanie danych | System plików (JSON + katalogi indeksu czasowego) | SpeedyPack (format .spk, mapowanie w pamięci + asynchroniczna trwałość) |
| Serwer Web | HttpListener (wbudowany w .NET) | HttpListener (wbudowany w .NET) |
| Kompilacja dynamiczna | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Automatyzacja przeglądarki | Playwright (WebView) | Playwright (WebView) |
| System wtyczek | ✅ Obsługa (IPlugin + PluginLoader) | ✅ Obsługa (IPlugin + PluginLoader) |
| Zasobnik systemowy | ❌ Brak obsługi | ✅ Windows/macOS obsługa (NotifyIcon); Linux brak ikony zasobnika |
| Licencja | Apache-2.0 | Apache-2.0 |

## 📁 Struktura projektu

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Biblioteka rdzenna (interfejsy, klasy abstrakcyjne)
│   │   ├── AI/                            # Interfejsy klientów AI, menedżer kontekstu, modele wiadomości
│   │   ├── Audit/                         # System audytu użycia Tokenów
│   │   ├── Chat/                          # System czatu, zarządzanie sesjami, kanały nadawcze
│   │   ├── Compilation/                   # Kompilacja dynamiczna, skanowanie bezpieczeństwa, szyfrowanie kodu
│   │   ├── Config/                        # System zarządzania konfiguracją
│   │   ├── Executors/                     # Wykonawcy (dysk, sieć, wiersz poleceń)
│   │   ├── IM/                            # Interfejsy dostawców komunikacji natychmiastowej
│   │   ├── Knowledge/                     # System sieci wiedzy
│   │   ├── Localization/                  # System lokalizacji
│   │   ├── Logging/                       # System logowania
│   │   ├── Plugins/                       # System wtyczek (interfejs IPlugin, moduł ładujący PluginLoader)
│   │   ├── Project/                       # System zarządzania projektami
│   │   ├── Runtime/                       # Pętla główna, obiekty zegara, rdzenny host
│   │   ├── Security/                      # System zarządzania uprawnieniami
│   │   ├── SiliconBeing/                  # Klasa bazowa Istot Krzemowych, menedżer, fabryka
│   │   ├── Storage/                       # Interfejsy przechowywania
│   │   ├── Time/                          # Niepełne daty (zapytania o zakres czasu)
│   │   ├── Tools/                         # Interfejsy narzędzi i menedżer narzędzi
│   │   ├── WebView/                       # Interfejs przeglądarki WebView
│   │   └── ServiceLocator.cs              # Globalny lokalizator usług
│   │
│   ├── SiliconLife.Common/                # Współdzielona implementacja (wspólna dla obu wersji)
│   │   ├── AI/                            # Klienci AI i fabryki (Ollama, DashScope, VolcengineArk)
│   │   ├── Calendar/                      # 32 implementacje kalendarzy
│   │   ├── Localization/                  # Klasy bazowe lokalizacji i 29 implementacji językowych/wariantów regionalnych
│   │   ├── Resources/                     # Współdzielone pliki zasobów
│   │   ├── Security/                      # Menedżer uprawnień
│   │   ├── SiliconBeing/                  # Domyślna implementacja Istoty Krzemowej
│   │   ├── Tools/                         # 23 implementacje narzędzi ogólnych (w tym narzędzie gorącego przeładowania)
│   │   ├── Web/                           # Infrastruktura Web
│   │   └── WebView/                       # Implementacja Playwright WebView
│   │
│   ├── SiliconLife.App/                   # Warstwa aplikacji (Web UI + dokumentacja pomocy, współdzielona przez Default i Fast)
│   │   ├── Config/                        # Konfiguracja aplikacji
│   │   ├── Data/                          # Katalog danych
│   │   ├── Help/                          # Lokalizacja dokumentacji pomocy (wielojęzyczna)
│   │   └── Web/                           # Implementacja Web UI
│   │       ├── Component/                 # Biblioteka komponentów UI (30+ komponentów)
│   │       ├── Controllers/               # 22 kontrolery
│   │       ├── Models/                    # Modele widoków
│   │       ├── Views/                     # Widoki HTML
│   │       └── Skins/                     # 7 motywów skórek
│   │
│   ├── SiliconLife.Default/               # Domyślna implementacja + punkt wejścia aplikacji (wersja konsolowa)
│   │   ├── Program.cs                     # Punkt wejścia (montowanie wszystkich komponentów)
│   │   ├── Config/                        # Domyślne dane konfiguracyjne
│   │   ├── IM/                            # Dostawca WebUI
│   │   ├── Knowledge/                     # Implementacja sieci wiedzy
│   │   ├── Logging/                       # Implementacja dostawcy logowania
│   │   ├── Project/                       # Implementacja systemu projektów
│   │   ├── Security/                      # Domyślne wywołanie zwrotne uprawnień
│   │   ├── Storage/                       # Implementacja przechowywania w systemie plików
│   │   └── Tools/                         # Implementacje narzędzi specyficzne dla wersji (HelpTool)
│   │
│   ├── SiliconLife.Fast/                  # Wysokowydajna implementacja + punkt wejścia aplikacji (wersja okienkowa)
│   │   ├── Program.cs                     # Punkt wejścia (aplikacja okienkowa)
│   │   ├── Config/                        # Dane konfiguracyjne (współdzielone z Default)
│   │   ├── IM/                            # Dostawca WebUI
│   │   ├── Knowledge/                     # Implementacja sieci wiedzy (optymalizacja pamięci)
│   │   ├── Logging/                       # Wysokowydajny dostawca logowania
│   │   ├── Project/                       # Implementacja systemu projektów
│   │   ├── Security/                      # Optymalizowane wywołanie zwrotne uprawnień
│   │   ├── Storage/                       # Adapter przechowywania SpeedyPack
│   │   ├── Tools/                         # Implementacje narzędzi specyficzne dla wersji (HelpTool)
│   │   └── Tray/                          # Zasobnik systemowy (lokalizacja w 29 wariantach językowych)
│   │
│   ├── SiliconLife.Speedy/                # Wysokowydajny silnik przechowywania SpeedyPack
│   │   ├── SpeedyPack.cs                  # Klasa rdzenna (mapowanie katalogów w pamięci + pamięć podręczna + asynchroniczny zapis)
│   │   ├── SpeedyPackOptions.cs           # Opcje konfiguracji (TTL pamięci podręcznej, maksymalna liczba wpisów itp.)
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
│   └── SiliconLife.Speedy.Manager/        # Narzędzie zarządzania SpeedyPack (Windows Forms)
│       ├── MainForm.cs                    # Formularz główny
│       ├── Program.cs                     # Punkt wejścia
│       └── slc.ico                        # Ikona aplikacji
│
├── docs/                                  # Dokumentacja wielojęzyczna
│   ├── zh-CN/                             # Dokumentacja w języku chińskim uproszczonym
│   ├── en/                                # Dokumentacja w języku angielskim
│   └── ...                                # Dokumentacja w innych językach
│
└── 总文档/                                 # Dokumentacja wymagań i architektury
    ├── 需求文档.md
    ├── 架构大纲.md
    └── 实现顺序.md
```

## 🏗️ Przegląd architektury

### Architektura harmonogramowania
```
Pętla główna (dedykowany wątek, strażnik + bezpiecznik)
  └── Obiekt zegara (sortowany według priorytetu)
       └── Menedżer Istot Krzemowych
            └── Runner Istoty Krzemowej (wątek tymczasowy, timeout + bezpiecznik)
                 └── Istota Krzemowa.Tick()
                      └── Menedżer kontekstu.Myśl()
                           └── Klient AI.Czat()
                                └── Pętla wywołań narzędzi → utrwalenie w systemie czatu
```

### Architektura bezpieczeństwa
Wszystkie operacje I/O inicjowane przez AI muszą przejść przez rygorystyczny łańcuch bezpieczeństwa:

```
Wywołanie narzędzia → Wykonawca → Menedżer uprawnień → [IsCurator → Pamięć podręczna częstotliwości → Globalna ACL → Wywołanie zwrotne → Zapytanie użytkownika]
```

## 🚀 Szybki start

### Wymagania wstępne

- **.NET 9 SDK** — [Link do pobrania](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Backend AI** (wybierz jeden):
  - **Ollama**: [Zainstaluj Ollama](https://ollama.com) i pobierz model (np. `ollama pull llama3`)
  - **Alibaba Cloud Bailian**: Uzyskaj klucz API z [konsoli Bailian](https://bailian.console.aliyun.com/)
  - **Volcengine Ark**: Uzyskaj klucz API z [konsoli Volcengine](https://console.volcengine.com/ark)

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
- ✅ Bardzo wysokie wymagania dotyczące bezpieczeństwa danych
- ✅ Ograniczone zasoby pamięci (RAM < 2GB)
- ✅ Mała ilość danych, krótkotrwałe użytkowanie
- ✅ Faza debugowania rozwoju

#### Sposób 2: Uruchomienie wersji Fast (aplikacja desktopowa)

```bash
dotnet run --project src/SiliconLife.Fast
```

**Windows/macOS**: Aplikacja uruchomi się w trybie okienkowym, zminimalizuje do zasobnika systemowego i będzie działać w tle.

**Linux**: Aplikacja wyświetli okno stanu (brak ikony w zasobniku systemowym) i automatycznie otworzy przeglądarkę, aby uzyskać dostęp do Web UI. Można użyć parametru `--no-tray`, aby pominąć automatyczne otwieranie przeglądarki:

```bash
dotnet run --project src/SiliconLife.Fast -- --no-tray
```

**Scenariusze zastosowania**:
- ✅ Scenariusze wysokiej współbieżności (> 5 użytkowników)
- ✅ Duże ilości danych (ponad 3 miesiące użytkowania)
- ✅ Potrzeba odpowiedzi o niskim opóźnieniu
- ✅ Potrzeba działania w tle w zasobniku

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
- [x] Faza 1: Czat AI w konsoli
- [x] Faza 2: Szkielet frameworka (pętla główna + obiekt zegara + strażnik + bezpiecznik)
- [x] Faza 3: Pierwsza Istota Krzemowa z plikiem duszy (architektura ciało-mózg)
- [x] Faza 4: Trwała pamięć (system czatu + interfejs przechowywania czasowego)
- [x] Faza 5: System narzędzi + wykonawcy
- [x] Faza 6: System uprawnień (łańcuch 5 poziomów, audytor, globalna lista kontroli dostępu)
- [x] Faza 7: Kompilacja dynamiczna + samewolucja (Roslyn)
- [x] Faza 8: Pamięć długoterminowa + zadania + czasomierze
- [x] Faza 9: Rdzenny host + współpraca wieloagentowa
- [x] Faza 10: Web UI (HTTP + SSE, 20+ kontrolerów, 4 motywy skórek)
- [x] Faza 10.5: Rozszerzenia przyrostowe (kanały nadawcze, audyt Tokenów, 32 kalendarze, ulepszenia narzędzi, lokalizacja w 21 językach)
- [x] Faza 10.6: Udoskonalenie i optymalizacja (WebView, system pomocy, obszar roboczy projektów, sieć wiedzy)
- [x] Faza 11: Silnik przechowywania SpeedyPack (zastąpienie LiteDB, mapowanie w pamięci, asynchroniczna kolejka zapisu, automatyczna kompresja)
- [x] Faza 12: System wtyczek (interfejs IPlugin, piaskownica bezpieczeństwa PluginLoader, izolowane ładowanie, integracja narzędzi)

### � W planach
- [ ] Faza 13: Integracja z zewnętrzną komunikacją natychmiastową (Feishu / WhatsApp / Telegram)
- [ ] Faza 14: Ekosystem umiejętności (rynek wtyczek, dystrybucja pakietów umiejętności)

## 📚 Dokumentacja

- [Projekt architektury](architecture.md) — projekt systemu, mechanizm harmonogramowania, architektura komponentów
- [Model bezpieczeństwa](security.md) — model uprawnień, wykonawcy, bezpieczeństwo kompilacji dynamicznej
- [Przewodnik rozwoju](development-guide.md) — rozwój narzędzi, przewodnik rozszerzeń
- [Referencja API](api-reference.md) — dokumentacja endpointów Web API
- [Referencja narzędzi](tools-reference.md) — szczegółowy opis wbudowanych narzędzi
- [Przewodnik Web UI](web-ui-guide.md) — przewodnik korzystania z interfejsu Web
- [Przewodnik Istoty Krzemowej](silicon-being-guide.md) — przewodnik rozwoju agentów
- [System uprawnień](permission-system.md) — szczegółowe omówienie zarządzania uprawnieniami
- [System kalendarzowy](calendar-system.md) — opis 32 systemów kalendarzowych
- [Szybki start](getting-started.md) — szczegółowy przewodnik dla początkujących
- [Rozwiązywanie problemów](troubleshooting.md) — odpowiedzi na najczęstsze pytania
- [Plan rozwoju](roadmap.md) — pełny plan rozwoju
- [Dziennik zmian](changelog.md) — historia aktualizacji wersji
- [Przewodnik współpracy](contributing.md) — jak uczestniczyć w projekcie

## 🤝 Współpraca

Witamy wszelkie formy współpracy! Szczegóły znajdują się w [przewodniku współpracy](contributing.md).

### Przepływ pracy deweloperskiej
1. Fork tego repozytorium
2. Utwórz gałąź funkcji (`git checkout -b feature/AmazingFeature`)
3. Zatwierdź zmiany (`git commit -m 'feat: add some AmazingFeature'`)
4. Wypchnij do gałęzi (`git push origin feature/AmazingFeature`)
5. Złóż Pull Request

## 💡 Przewodnik wyboru wersji

### Którą wersję powinienem używać?

**SiliconLife.Default (implementacja domyślna — weryfikacja wykonalności architektury):**
- 📌 Pierwszy kontakt z tym projektem, chęć szybkiego poznania architektury systemu
- 📌 Trwa faza debugowania rozwoju, potrzeba prostego i bezpośredniego sposobu uruchomienia
- 📌 Bezpieczeństwo danych jest priorytetem
- 📌 Pamięć systemu jest mniejsza niż 4GB
- 📌 Tylko jednoosobowe użytkowanie lub mała ilość danych

**SiliconLife.Fast (główna wersja produkcyjna):**
- ⚡ Potrzeba długotrwałego stabilnego działania w środowisku produkcyjnym
- ⚡ Znajomość architektury systemu, gotowość do formalnego wdrożenia
- ⚡ Potrzeba obsługi współbieżnego dostępu wielu użytkowników
- ⚡ Potrzeba działania w tle w zasobniku systemowym
- ⚡ Dążenie do ekstymalnych osiągów wydajności

> **Ogólna rekomendacja**: SiliconLife.Default jest odpowiedni jako weryfikacja architektury i wprowadzenie; dla rzeczywistych środowisk produkcyjnych zdecydowanie zaleca się używanie SiliconLife.Fast.

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
4. Po weryfikacji poprawnego działania można codziennie używać wersji Fast

### Czy obie wersje mogą współistnieć?

**Tak!** Zalecana strategia wdrożenia:

**Strategia 1: Default do weryfikacji, Fast do produkcji**
```
Środowisko deweloperskie/weryfikacyjne: SiliconLife.Default (weryfikacja architektury, debugowanie funkcji)
Środowisko produkcyjne: SiliconLife.Fast (wysoka wydajność, działanie w tle, przetwarzanie żądań w czasie rzeczywistym)
```

**Strategia 2: Fast jako główne uruchomienie, Default do okresowych kopii zapasowych**
```
SiliconLife.Fast (codzienne użytkowanie, przetwarzanie żądań w czasie rzeczywistym)
    ↓ Okresowe kopie zapasowe
SiliconLife.Default (archiwizacja zimnych danych, zabezpieczenie bezpieczeństwa danych)
```

## 📄 Licencja

Ten projekt jest objęty licencją Apache License 2.0 — szczegóły w pliku [LICENSE](../../LICENSE).

## 👨‍💻 Autor

**天源垦骥**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- Gitee: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili: [617827040](https://space.bilibili.com/617827040)

## 🙏 Podziękowania

Dziękujemy wszystkim deweloperom i dostawcom platform AI, którzy wnieśli swój wkład w ten projekt.

---

**Silicon Life Collective** — spraw, by agenty AI naprawdę "ożyły"
