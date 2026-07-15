# Быстрый старт

> **Версия: v0.2.0-alpha**

[English](../en/getting-started.md) | [Deutsch](../de-DE/getting-started.md) | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | [한국어](../ko-KR/getting-started.md) | [Čeština](../cs-CZ/getting-started.md) | **Русский**

## Выбор версии

Этот проект предоставляет две версии реализации:

### SiliconLife.Default (версия по умолчанию)
- **Назначение**: реализация по умолчанию, в основном для проверки жизнеспособности архитектуры
- **Режим работы**: консольное приложение
- **Способ хранения**: хранение JSON в файловой системе
- **Сценарии применения**: приоритет безопасности данных, небольшой объём данных, отладка разработки, проверка архитектуры
- **Поддержка платформ**: Windows, Linux, macOS
- **Описание роли**: базовая реализация для проверки архитектуры, обеспечивает простой и надёжный способ работы, подходит для первого знакомства с проектом или отладки разработки

### SiliconLife.Fast (высокопроизводительная версия)
- **Назначение**: основная производственная версия
- **Режим работы**: настольное приложение (системный трей Windows/macOS / окно состояния Linux)
- **Способ хранения**: хранение в памяти SpeedyPack + асинхронная персистентность (формат файла .spk)
- **Сценарии применения**: высокая параллельность, низкая задержка, большие объёмы данных, длительная производственная эксплуатация
- **Поддержка платформ**: Windows/macOS (полная функциональность, включая системный трей), Linux (окно состояния, без значка в трее)
- **Описание роли**: глубоко оптимизированная производственная реализация, предпочтительный выбор для длительной работы и реальных производственных сред

> **Совет для новичков**: при первом использовании рекомендуется начать с **SiliconLife.Default** для быстрой проверки жизнеспособности архитектуры; после ознакомления с системой настоятельно рекомендуется перейти на **SiliconLife.Fast** в качестве производственной версии.

## Предварительные требования

- **.NET 9 SDK** - [загрузка](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [загрузка](https://git-scm.com/)
- **Ollama** (опционально, для локального AI) - [загрузка](https://ollama.com/)
- **API-ключ Bailian** (опционально, для облачного AI) - [получить](https://bailian.console.aliyun.com/)
- **API-ключ Volcengine Ark** (опционально, для облачного AI) - [получить](https://console.volcengine.com/ark)
- **Herdsman** (опционально, локальный/облачный движок вывода) - без аутентификации, совместим с форматом OpenAI API
- **API-ключ Meituan LongCat** (опционально, для облачного AI) - аутентификация по API Key
- **API-ключ Qiniu Cloud AI** (опционально, для облачного AI) - аутентификация по API Key
- **API-ключ DeepSeek** (опционально, для облачного AI) - [получить](https://platform.deepseek.com/)
- **API-ключ Zhipu AI** (опционально, для облачного AI) - [получить](https://open.bigmodel.cn/)
- **API-ключ Baidu Qianfan** (опционально, для облачного AI) - [получить](https://console.bce.baidu.com/qianfan/)
- **API-ключ Tencent Hunyuan** (опционально, для облачного AI) - [получить](https://console.cloud.tencent.com/hunyuan/)
- **API-ключ MiniMax** (опционально, для облачного AI) - [получить](https://platform.minimaxi.com/)
- **API-ключ Moonshot** (опционально, для облачного AI) - [получить](https://platform.moonshot.cn/)
- **API-ключ SiliconFlow** (опционально, для облачного AI) - [получить](https://cloud.siliconflow.cn/)

## Быстрый старт

### 1. Клонирование репозитория

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. Сборка проекта

```bash
dotnet build
```

### 3. Настройка AI-бэкенда

Отредактируйте `src/SiliconLife.Default/Config/DefaultConfigData.cs` или измените конфигурацию через Web UI во время работы.

#### Вариант A: Ollama (локальный)

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

#### Вариант B: Bailian (облачный)

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

> **Доступные регионы**: `beijing` (Пекин), `virginia` (Вирджиния), `singapore` (Сингапур), `hongkong` (Гонконг), `frankfurt` (Франкфурт)

#### Вариант C: Volcengine Ark (облачный)

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

> **Примечание**: параметр Model для Volcengine Ark принимает ID конечной точки вывода (например, `ep-20241212123456-abcde`), а не имя модели.

#### Вариант D: Herdsman (локальный/облачный)

```json
{
  "AIClients": {
    "Herdsman": {
      "Endpoint": "http://localhost:8000",
      "Model": "your-model-name"
    }
  }
}
```

> **Особенности**: без аутентификации, совместим с форматом OpenAI API, поддержка вызова инструментов и содержимого рассуждений.

#### Вариант E: Meituan LongCat (облачный)

```json
{
  "AIClients": {
    "LongCat": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://api.longcat.ai/v1/chat/completions",
      "Model": "your-model-name"
    }
  }
}
```

#### Вариант F: Qiniu Cloud AI (облачный)

```json
{
  "AIClients": {
    "QiniuAI": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://api.qiniu.com/v1/chat/completions",
      "Model": "your-model-name"
    }
  }
}
```

#### Вариант G: DeepSeek (облачный)

```json
{
  "AIClients": {
    "DeepSeek": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://api.deepseek.com",
      "Model": "deepseek-v4-flash",
      "ThinkingEnabled": true,
      "ReasoningEffort": "high"
    }
  }
}
```

> **Примечание**: DeepSeek поддерживает режим thinking с reasoning_content. Модель `deepseek-v4-flash` предоставляет контекст до 1M токенов.

#### Вариант H: Zhipu AI / GLM (облачный)

```json
{
  "AIClients": {
    "Zhipu": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://open.bigmodel.cn/api/paas/v4",
      "Model": "glm-4-flash",
      "ThinkingEnabled": false
    }
  }
}
```

> **Примечание**: `glm-4-flash` — бесплатная модель. Режим thinking доступен только для серии GLM-5. Vision поддерживается моделями `glm-4v` и `glm-5v`.

#### Вариант I: Baidu Qianfan / Ernie (облачный)

```json
{
  "AIClients": {
    "Ernie": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://qianfan.baidubce.com/v2",
      "Model": "ernie-5.1"
    }
  }
}
```

> **Примечание**: Qianfan v2 использует OpenAI-совместимый API. Модели `ernie-speed` и `ernie-tiny` — бесплатные.

#### Вариант J: Tencent Hunyuan (облачный)

```json
{
  "AIClients": {
    "Hunyuan": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://tokenhub.tencentmaas.com/v1",
      "Model": "hy3",
      "ThinkingEnabled": false
    }
  }
}
```

> **Доступные endpoints**: `https://tokenhub.tencentmaas.com/v1` (TokenHub, рекомендуется) или `https://api.hunyuan.cloud.tencent.com/v1` (Legacy).

#### Вариант K: MiniMax (облачный)

```json
{
  "AIClients": {
    "MiniMax": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://api.minimaxi.com/v1",
      "Model": "MiniMax-M3"
    }
  }
}
```

> **Примечание**: MiniMax M3 поддерживает нативный multimodal ввод (изображение + видео) и режим thinking с reasoning_split. Контекст до 1M токенов.

#### Вариант L: Moonshot / Kimi (облачный)

```json
{
  "AIClients": {
    "Moonshot": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://api.moonshot.cn/v1",
      "Model": "kimi-k2.6"
    }
  }
}
```

> **Примечание**: Модели kimi-k2.5/k2.6/k2.7 поддерживают vision. Режим thinking доступен для kimi-k2.7. Контекст до 262K токенов.

#### Вариант M: SiliconFlow (облачный, агрегатор моделей)

```json
{
  "AIClients": {
    "SiliconFlow": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://api.siliconflow.cn/v1",
      "Model": "deepseek-ai/DeepSeek-V3.2"
    }
  }
}
```

> **Примечание**: SiliconFlow агрегирует 100+ моделей открытого исходного кода от нескольких провайдеров. Поддерживается префикс `pro/` для профессиональных моделей. Контекст до 1M токенов в зависимости от модели.

### 4. Запуск приложения

#### Запуск версии Default

```bash
cd src/SiliconLife.Default
dotnet run
```

Веб-сервер запустится на `http://localhost:8080`

#### Запуск версии Fast

```bash
cd src/SiliconLife.Fast
dotnet run
```

**Windows/macOS**: приложение запустится в оконном режиме, свернётся в системный трей, веб-сервер также запустится на `http://localhost:8080`

**Linux**: приложение отобразит окно состояния (без значка в системном трее) и автоматически откроет браузер для доступа к Web UI. Также можно использовать параметр `--no-tray`, чтобы пропустить автоматическое открытие браузера:

```bash
dotnet run -- --no-tray
```

### 5. Доступ к Web UI

Откройте браузер и перейдите по адресу:

```
http://localhost:8080
```

Вы увидите панель управления, содержащую:
- Управление Кремниевыми Существами
- Интерфейс чата
- Панель конфигурации
- Мониторинг системы

## Первое Кремниевое Существо

### Создание первого существа

1. В Web UI перейдите в **Управление существами**
2. Нажмите **Создать новое существо**
3. Настройте Файл Души (`soul.md`), включив личность и поведение
4. Запустите существо

### Пример soul.md

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

## Часто задаваемые вопросы

### Ollama — отказ соединения

**Проблема**: невозможно подключиться к Ollama по адресу `http://localhost:11434`

**Решение**:
```bash
# Проверьте, запущен ли Ollama
ollama list

# Если нужно запустить Ollama
ollama serve
```

### Модель не найдена

**Проблема**: `model "qwen2.5:7b" not found`

**Решение**:
```bash
# Загрузите нужную модель
ollama pull qwen2.5:7b
```

### Порт уже занят

**Проблема**: `HttpListenerException: Address already in use`

**Решение**:
- Измените порт в конфигурации
- Или завершите процесс, использующий порт 8080:

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## Следующие шаги

- 📚 Прочитайте [руководство по архитектуре](architecture.md) для понимания проектирования системы
- 🛠️ Ознакомьтесь с [руководством разработчика](development-guide.md) для расширения системы
- 📖 Изучите [справочник API](api-reference.md) для деталей интеграции
- 🔒 Ознакомьтесь с [документацией по безопасности](security.md) для понимания системы разрешений
- 🧰 Просмотрите [справочник инструментов](tools-reference.md) для всех встроенных инструментов
- 🌐 Ознакомьтесь с [руководством по Web UI](web-ui-guide.md) для функциональности интерфейса

## Структура проекта

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Основные интерфейсы и абстрактные классы
│   ├── SiliconLife.Common/          # Общие реализации (используются обеими версиями)
│   ├── SiliconLife.App/             # Уровень приложения, общий для Default и Fast
│   ├── SiliconLife.Default/         # Реализация по умолчанию + точка входа (консольная версия)
│   ├── SiliconLife.Fast/            # Высокопроизводительная реализация + точка входа (оконная версия)
│   ├── SiliconLife.Speedy/          # Высокопроизводительный Движок Хранения SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Менеджер SpeedyPack (Avalonia UI)
├── docs/                            # Документация (многоязычная, 34 языковых варианта)
│   ├── en/                          # Английский
│   ├── zh-CN/                       # Упрощённый китайский
│   ├── zh-HK/                       # Традиционный китайский
│   ├── es-ES/                       # Испанский
│   ├── ja-JP/                       # Японский
│   ├── ko-KR/                       # Корейский
│   └── cs-CZ/                       # Чешский
├── 总文档/                           # Документация требований и архитектуры (китайский)
└── README.md                        # Описание проекта
```

## Нужна помощь?

- 📖 Ознакомьтесь с [системой справочной документации](web-ui-guide.md#帮助文档系统新增) (многоязычная поддержка)
- 📚 Прочитайте [полную документацию](docs/)
- 🐛 Сообщите о проблемах на [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Участвуйте в обсуждениях сообщества
