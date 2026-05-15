# Szybki start

> **Wersja: v0.1.0-alpha**

[English](../en/getting-started.md) | [Deutsch](../de-DE/getting-started.md) | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | [Čeština](../cs-CZ/getting-started.md) | [Polski](../pl-PL/getting-started.md)

## Wybór wersji

Ten projekt oferuje dwie wersje implementacji:

### SiliconLife.Default (wersja domyślna)
- **Przeznaczenie**: domyślna implementacja, głównie do weryfikacji wykonalności architektury
- **Tryb działania**: aplikacja konsolowa
- **Sposób przechowywania**: przechowywanie JSON w systemie plików
- **Scenariusze zastosowania**: priorytet bezpieczeństwa danych, małe ilości danych, debugowanie rozwoju, weryfikacja architektury
- **Obsługa platform**: Windows, Linux, macOS
- **Opis roli**: jako referencyjna implementacja weryfikacji architektury, oferuje prosty i niezawodny sposób działania, odpowiedni do pierwszego kontaktu z projektem lub debugowania rozwoju

### SiliconLife.Fast (wersja wysokowydajna)
- **Przeznaczenie**: główna wersja produkcyjna
- **Tryb działania**: aplikacja desktopowa (Windows/macOS zasobnik systemowy / Linux okno stanu)
- **Sposób przechowywania**: pamięć SpeedyPack + asynchroniczna trwałość (format pliku .spk)
- **Scenariusze zastosowania**: wysoka współbieżność, niskie opóźnienie, duże ilości danych, długotrwałe działanie produkcyjne
- **Obsługa platform**: Windows/macOS (pełna funkcjonalność, z zasobnikiem systemowym), Linux (okno stanu, brak ikony w zasobniku)
- **Opis roli**: głęboko zoptymalizowana implementacja produkcyjna, preferowana do długotrwałego działania i rzeczywistych środowisk produkcyjnych

> **Porada dla początkujących**: przy pierwszym użyciu zaleca się rozpoczęcie od **SiliconLife.Default**, aby szybko zweryfikować wykonalność architektury; po zapoznaniu się z systemem, zdecydowanie zaleca się migrację do **SiliconLife.Fast** jako wersji do środowiska produkcyjnego.

## Wymagania wstępne

- **.NET 9 SDK** - [Pobierz](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Pobierz](https://git-scm.com/)
- **Ollama** (opcjonalnie, dla lokalnego AI) - [Pobierz](https://ollama.com/)
- **Klucz API Bailian** (opcjonalnie, dla chmurowego AI) - [Złóż wniosek](https://bailian.console.aliyun.com/)
- **Klucz API Volcengine Ark** (opcjonalnie, dla chmurowego AI) - [Złóż wniosek](https://console.volcengine.com/ark)

## Szybki start

### 1. Klonowanie repozytorium

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. Budowanie projektu

```bash
dotnet build
```

### 3. Konfiguracja backendu AI

Edytuj `src/SiliconLife.Default/Config/DefaultConfigData.cs` lub modyfikuj konfigurację w czasie działania przez Web UI.

#### Opcja A: Ollama (lokalne)

```json
{
  "AIClients": {
    "Ollama": {
      "BaseUrl": "http://localhost:11434",
      "Model": "qwen2.5:7b"
    }
  }
}
```

#### Opcja B: Bailian (chmura)

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "twój-klucz-api-tutaj",
      "Model": "qwen-plus",
      "Region": "cn-hangzhou"
    }
  }
}
```

#### Opcja C: Volcengine Ark (chmura)

```json
{
  "AIClients": {
    "VolcengineArk": {
      "ApiKey": "twój-klucz-api-tutaj",
      "Endpoint": "https://ark.cn-beijing.volces.com/api/v3/chat/completions",
      "Model": "ep-xxxxxxxxxxxxx-xxxxx"
    }
  }
}
```

> **Uwaga**: Parametr Model Volcengine Ark przyjmuje identyfikator punktu dostępowego wnioskowania (np. `ep-20241212123456-abcde`), a nie nazwę modelu.

### 4. Uruchomienie aplikacji

#### Uruchomienie wersji Default

```bash
cd src/SiliconLife.Default
dotnet run
```

Serwer Web uruchomi się na `http://localhost:8080`

#### Uruchomienie wersji Fast

```bash
cd src/SiliconLife.Fast
dotnet run
```

**Windows/macOS**: Aplikacja uruchomi się w trybie okienkowym, minimalizując do zasobnika systemowego, serwer Web również uruchomi się na `http://localhost:8080`

**Linux**: Aplikacja wyświetli okno stanu (brak ikony w zasobniku systemowym) i automatycznie otworzy przeglądarkę, aby uzyskać dostęp do Web UI. Parametr `--no-tray` pozwala pominąć automatyczne otwieranie przeglądarki:

```bash
dotnet run -- --no-tray
```

### 5. Dostęp do Web UI

Otwórz przeglądarkę i przejdź do:

```
http://localhost:8080
```

Zobaczysz pulpit nawigacyjny zawierający:
- Zarządzanie Istotami Krzemowymi
- Interfejs czatu
- Panel konfiguracji
- Monitorowanie systemu

## Twoja pierwsza Istota Krzemowa

### Utworzenie pierwszej istoty

1. W Web UI przejdź do **Zarządzanie istotami**
2. Kliknij **Utwórz nową istotę**
3. Skonfiguruj plik duszy (`soul.md`), zawierający osobowość i zachowanie
4. Uruchom istotę

### Przykład soul.md

```markdown
# Moja pierwsza Istota Krzemowa

## Osobowość
Jesteś pomocnym asystentem specjalizującym się w przeglądzie kodu.

## Zdolności
- Przeglądanie jakości kodu
- Sugerowanie ulepszeń
- Tłumaczenie złożonych koncepcji

## Zachowanie
- Zawsze dostarczaj konstruktywną informację zwrotną
- Używaj jasnych przykładów
- Bądź zwięzły, ale dokładny
```

## Często zadawane pytania

### Odrzucono połączenie z Ollama

**Problem**: Nie można połączyć się z Ollama pod `http://localhost:11434`

**Rozwiązanie**:
```bash
# Sprawdź, czy Ollama jest uruchomione
ollama list

# Jeśli potrzebujesz uruchomić Ollama
ollama serve
```

### Nie znaleziono modelu

**Problem**: `model "qwen2.5:7b" not found`

**Rozwiązanie**:
```bash
# Pobierz wymagany model
ollama pull qwen2.5:7b
```

### Port jest zajęty

**Problem**: `HttpListenerException: Address already in use`

**Rozwiązanie**:
- Zmień port w konfiguracji
- Lub zakończ proces używający portu 8080:

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## Następne kroki

- 📚 Przeczytaj [przewodnik architektury](architecture.md), aby poznać projekt systemu
- 🛠️ Zobacz [przewodnik rozwoju](development-guide.md), aby rozszerzyć system
- 📖 Poznaj [referencję API](api-reference.md), aby uzyskać szczegóły integracji
- 🔒 Zobacz [dokumentację bezpieczeństwa](security.md), aby poznać system uprawnień
- 🧰 Zobacz [referencję narzędzi](tools-reference.md), aby poznać wszystkie wbudowane narzędzia
- 🌐 Zobacz [przewodnik Web UI](web-ui-guide.md), aby poznać funkcje interfejsu

## Struktura projektu

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Rdzenne interfejsy i klasy abstrakcyjne
│   ├── SiliconLife.Common/          # Współdzielona implementacja (wspólna dla obu wersji)
│   ├── SiliconLife.App/             # Warstwa aplikacji współdzielona przez Default i Fast
│   ├── SiliconLife.Default/         # Domyślna implementacja + punkt wejścia (wersja konsolowa)
│   ├── SiliconLife.Fast/            # Wysokowydajna implementacja + punkt wejścia (wersja okienkowa)
│   ├── SiliconLife.Speedy/          # Wysokowydajny silnik przechowywania SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Narzędzie zarządzania SpeedyPack (Windows Forms)
├── docs/                            # Dokumentacja (wielojęzyczna, 29 wariantów językowych)
│   ├── en/                          # Angielski
│   ├── zh-CN/                       # Chiński uproszczony
│   ├── zh-HK/                       # Chiński tradycyjny
│   ├── es-ES/                       # Hiszpański
│   ├── ja-JP/                       # Japoński
│   ├── ko-KR/                       # Koreański
│   └── cs-CZ/                       # Czeski
├── 总文档/                           # Dokumentacja wymagań i architektury (chińska)
└── README.md                        # Opis projektu
```

## Potrzebujesz pomocy?

- 📖 Zobacz [system dokumentacji pomocy](web-ui-guide.md#帮助文档系统新增) (obsługa wielojęzyczna)
- 📚 Przeczytaj [pełną dokumentację](docs/)
- 🐛 Zgłoś problemy na [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Dołącz do dyskusji społeczności
