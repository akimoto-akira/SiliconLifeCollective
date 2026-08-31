# Szybki start

> **Wersja: v0.2.0-alpha**

[English](../en/getting-started.md) | [Deutsch](../de-DE/getting-started.md) | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | [Čeština](../cs-CZ/getting-started.md) | [Русский](../ru-RU/getting-started.md)

## Wybór wersji

Projekt oferuje dwie wersje implementacji:

### SiliconLife.Default (wersja domyślna)
- **Pozycjonowanie**: domyślna implementacja, głównie do weryfikacji wykonalności architektury
- **Tryb działania**: aplikacja konsolowa
- **Metoda przechowywania**: przechowywanie JSON w systemie plików
- **Scenariusze zastosowania**: priorytet bezpieczeństwa danych, mała ilość danych, debugowanie deweloperskie, weryfikacja architektury
- **Obsługa platform**: Windows, Linux, macOS
- **Opis roli**: jako implementacja odniesienia do weryfikacji architektury, zapewnia prosta i niezawodny sposób uruchomienia, odpowiednia do pierwszego kontaktu z projektem lub debugowania deweloperskiego

### SiliconLife.Fast (wersja wysokowydajna)
- **Pozycjonowanie**: zalecana wersja produkcyjna
- **Tryb działania**: aplikacja desktopowa (Windows/macOS zasobnik systemowy / Linux okno statusu)
- **Metoda przechowywania**: przechowywanie w pamięci SpeedyPack + asynchroniczna trwałość (format pliku .spk)
- **Scenariusze zastosowania**: wysoka współbieżność, niskie opóźnienie, duże ilości danych, długotrwałe działanie produkcyjne
- **Obsługa platform**: Windows/macOS (pełna funkcjonalność, z zasobnikiem systemowym), Linux (okno statusu, bez ikony zasobnika)
- **Opis roli**: głęboko zoptymalizowana implementacja produkcyjna, zalecana do długotrwałego działania i rzeczywistych środowisk produkcyjnych

> **Porada dla początkujących**: przy pierwszym użyciu zalecamy rozpoczęcie od **SiliconLife.Default**, aby szybko zweryfikować wykonalność architektury; po zapoznaniu się z systemem, zdecydowanie zalecamy migrację do **SiliconLife.Fast** jako wersji do środowiska produkcyjnego.

## Wymagania wstępne

- **.NET 9 SDK** - [Pobierz](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Pobierz](https://git-scm.com/)
- **Ollama** (opcjonalnie, do lokalnego AI) - [Pobierz](https://ollama.com/)
- **Klucz API Bailian** (opcjonalnie, do chmurowego AI) - [Złóż wniosek](https://bailian.console.aliyun.com/)
- **Klucz API Volcengine Ark** (opcjonalnie, do chmurowego AI) - [Złóż wniosek](https://console.volcengine.com/ark)
- **Herdsman** (opcjonalnie, lokalny/chmurowy silnik wnioskowania) - bez autoryzacji, kompatybilny z formatem OpenAI API
- **Klucz API Meituan LongCat** (opcjonalnie, do chmurowego AI) - autoryzacja przez klucz API
- **Klucz API Qiniu Cloud AI** (opcjonalnie, do chmurowego AI) - autoryzacja przez klucz API
- **Klucz API DeepSeek** (opcjonalnie, do chmurowego AI) - [Złóż wniosek](https://platform.deepseek.com/)
- **Klucz API Zhipu AI** (opcjonalnie, do chmurowego AI) - [Złóż wniosek](https://open.bigmodel.cn/)
- **Klucz API Baidu Qianfan** (opcjonalnie, do chmurowego AI) - [Złóż wniosek](https://qianfan.baidubce.com/)
- **Klucz API Tencent Hunyuan** (opcjonalnie, do chmurowego AI) - [Złóż wniosek](https://hunyuan.tencent.com/)
- **Klucz API MiniMax** (opcjonalnie, do chmurowego AI) - [Złóż wniosek](https://www.minimaxi.com/)
- **Klucz API Moonshot** (opcjonalnie, do chmurowego AI) - [Złóż wniosek](https://platform.moonshot.cn/)
- **Klucz API SiliconFlow** (opcjonalnie, do chmurowego AI) - [Złóż wniosek](https://siliconflow.cn/)

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

Edytuj `src/SiliconLife.Default/Config/DefaultConfigData.cs` lub zmodyfikuj konfigurację w czasie działania przez Web UI.

#### Opcja A: Ollama (lokalna)

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
      "ApiKey": "your-api-key-here",
      "Model": "qwen-plus",
      "Region": "beijing"
    }
  }
}
```

> **Dostępne regiony**: `beijing` (Pekin), `virginia` (Wirginia), `singapore` (Singapur), `hongkong` (Hongkong), `frankfurt` (Frankfurt)

#### Opcja C: Volcengine Ark (chmura)

```json
{
  "AIClients": {
    "VolcengineArk": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://ark.cn-beijing.volces.com/api/v3/chat/completions",
      "Model": "ep-xxxxxxxxxxxxx-xxxxx"
    }
  }
}
```

> **Uwaga**: Parametr Model Volcengine Ark przyjmuje identyfikator punktu dostępowego wnioskowania (np. `ep-20241212123456-abcde`), a nie nazwę modelu.

#### Opcja D: Herdsman (lokalna/chmura)

```json
{
  "AIClients": {
    "Herdsman": {
      "Endpoint": "http://localhost:8000",
      "Model": "twoja-nazwa-modelu"
    }
  }
}
```

> **Cechy**: bez autoryzacji, kompatybilny z formatem OpenAI API, obsługa wywołań narzędzi i treści rozumowania.

#### Opcja E: Meituan LongCat (chmura)

```json
{
  "AIClients": {
    "LongCat": {
      "ApiKey": "twój-klucz-api",
      "Endpoint": "https://api.longcat.chat/openai",
      "Model": "LongCat-2.0"
    }
  }
}
```

#### Opcja F: Qiniu Cloud AI (chmura)

```json
{
  "AIClients": {
    "QiniuAI": {
      "ApiKey": "twój-klucz-api",
      "Endpoint": "https://api.qiniu.com/v1/chat/completions",
      "Model": "twoja-nazwa-modelu"
    }
  }
}
```

#### Opcja G: DeepSeek (chmura)

```json
{
  "AIClients": {
    "DeepSeek": {
      "ApiKey": "twój-klucz-api",
      "Endpoint": "https://api.deepseek.com",
      "Model": "deepseek-v4-flash",
      "ThinkingEnabled": true,
      "ReasoningEffort": "high"
    }
  }
}
```

> **Cechy**: tryb thinking z łańcuchem rozumowania, okno kontekstu 1M tokenów (seria deepseek-v4), konfigurowalny poziom wysiłku rozumowania.

#### Opcja H: Zhipu AI GLM (chmura)

```json
{
  "AIClients": {
    "Zhipu": {
      "ApiKey": "twój-klucz-api",
      "Endpoint": "https://open.bigmodel.cn/api/paas/v4",
      "Model": "glm-4-flash",
      "ThinkingEnabled": false
    }
  }
}
```

> **Cechy**: tryb thinking (seria GLM-5), wizja (modele z przyrostem `v`), okno kontekstu do 1M tokenów, darmowy model `glm-4-flash`.

#### Opcja I: Ernie Baidu Qianfan (chmura)

```json
{
  "AIClients": {
    "Ernie": {
      "ApiKey": "twój-klucz-api",
      "Endpoint": "https://qianfan.baidubce.com/v2",
      "Model": "ernie-5.1"
    }
  }
}
```

> **Cechy**: API kompatybilne z OpenAI v2, okno kontekstu 131K tokenów, wizja (seria ernie-5), darmowe modele `ernie-speed` i `ernie-tiny`.

#### Opcja J: Tencent Hunyuan (chmura)

```json
{
  "AIClients": {
    "Hunyuan": {
      "ApiKey": "twój-klucz-api",
      "Endpoint": "https://tokenhub.tencentmaas.com/v1",
      "Model": "hy3",
      "ThinkingEnabled": false
    }
  }
}
```

> **Cechy**: podwójny endpoint (TokenHub zalecany lub Legacy), okno kontekstu 262K tokenów, tryb thinking (seria hy3).

#### Opcja K: MiniMax (chmura)

```json
{
  "AIClients": {
    "MiniMax": {
      "ApiKey": "twój-klucz-api",
      "Endpoint": "https://api.minimaxi.com/v1",
      "Model": "MiniMax-M3"
    }
  }
}
```

> **Cechy**: adaptacyjny tryb thinking, okno kontekstu 1M tokenów, wizja (MiniMax-M3).

#### Opcja L: Moonshot Kimi (chmura)

```json
{
  "AIClients": {
    "Moonshot": {
      "ApiKey": "twój-klucz-api",
      "Endpoint": "https://api.moonshot.cn/v1",
      "Model": "kimi-k2.6"
    }
  }
}
```

> **Cechy**: tryb thinking, okno kontekstu 262K tokenów, wizja (seria kimi-k2.5+).

#### Opcja M: SiliconFlow (chmura)

```json
{
  "AIClients": {
    "SiliconFlow": {
      "ApiKey": "twój-klucz-api",
      "Endpoint": "https://api.siliconflow.cn/v1",
      "Model": "deepseek-ai/DeepSeek-V3.2"
    }
  }
}
```

> **Cechy**: agreguje ponad 100 modeli open-source od wielu dostawców, dynamiczna lista modeli, okno kontekstu do 1M tokenów.

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

**Windows/macOS**: aplikacja uruchomi się w trybie okienkowym, zminimalizuje do zasobnika systemowego, serwer Web również uruchomi się na `http://localhost:8080`

**Linux**: aplikacja wyświetli okno statusu (bez ikony zasobnika systemowego) i automatycznie otworzy przeglądarkę z Web UI. Można również użyć parametru `--no-tray`, aby pominąć automatyczne otwieranie przeglądarki:

```bash
dotnet run -- --no-tray
```

### 5. Dostęp do Web UI

Otwórz przeglądarkę i przejdź do:

```
http://localhost:8080
```

Zobaczysz panel nawigacyjny zawierający:
- Zarządzanie Istotami Krzemowymi
- Interfejs czatu
- Panel konfiguracji
- Monitorowanie systemu

## Pierwsza Istota Krzemowa

### Tworzenie pierwszej istoty

1. W Web UI przejdź do **Zarządzanie istotami**
2. Kliknij **Utwórz nową istotę**
3. Skonfiguruj Plik Duszy (`soul.md`), zawierający osobowość i zachowanie
4. Uruchom istotę

### Przykład soul.md

```markdown
# My First Silicon Being

## Personality
You are a helpful assistant specializing in code review.

## Capabilities
- Review code quality
- Suggest improvements
- Explain complex concepts

## Behavior
- Always provide constructive feedback
- Use clear examples
- Be concise but thorough
```

## Częste problemy

### Odrzucone połączenie z Ollama

**Problem**: nie można połączyć się z Ollama pod `http://localhost:11434`

**Rozwiązanie**:
```bash
# Sprawdź, czy Ollama jest uruchomiona
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
- 🛠️ Zobacz [przewodnik deweloperski](development-guide.md), aby rozszerzyć system
- 📖 Przeglądaj [referencję API](api-reference.md), aby poznać szczegóły integracji
- 🔒 Zobacz [dokumentację bezpieczeństwa](security.md), aby poznać system uprawnień
- 🧰 Zobacz [referencję narzędzi](tools-reference.md), aby poznać wszystkie wbudowane narzędzia
- 🌐 Zobacz [przewodnik Web UI](web-ui-guide.md), aby poznać funkcje interfejsu

## Struktura projektu

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Główne interfejsy i klasy abstrakcyjne
│   ├── SiliconLife.Common/          # Współdzielone implementacje (wspólne dla obu wersji)
│   ├── SiliconLife.App/             # Warstwa aplikacji współdzielona przez Default i Fast
│   ├── SiliconLife.Default/         # Domyślna implementacja + punkt wejścia (wersja konsolowa)
│   ├── SiliconLife.Fast/            # Wysokowydajna implementacja + punkt wejścia (wersja okienkowa)
│   ├── SiliconLife.Speedy/          # Wysokowydajny silnik przechowywania SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Narzędzie zarządzania SpeedyPack (Avalonia UI)
├── docs/                            # Dokumentacja (wielojęzyczna, 34 warianty językowe)
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

- 📖 Zobacz [system dokumentacji pomocy](web-ui-guide.md#system-dokumentacji-pomocy-nowość) (obsługa wielojęzyczna)
- 📚 Przeczytaj [pełną dokumentację](docs/)
- 🐛 Zgłoś problemy na [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Dołącz do dyskusji społeczności
