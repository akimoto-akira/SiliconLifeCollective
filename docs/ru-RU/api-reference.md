# Справочник API

> **Версия: v0.2.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md) | **Русский**

## Web API эндпоинты

Базовый URL: `http://localhost:8080`

### Аутентификация

Большинство эндпоинтов требуют аутентификации через сеансовый cookie, управляемый Web UI. До инициализации системы все запросы, кроме страницы справки, будут перенаправлены на страницу инициализации.

---

## Панель управления

### Получение статистики панели управления

**GET** `/api/dashboard/stats`

Возвращает данные обзора системы (количество существ, статус работы и др.).

### Получение метрик производительности

**GET** `/api/dashboard/metrics`

Возвращает данные метрик производительности в реальном времени.

---

## Система чата

### Страница чата

**GET** `/chat`

Возвращает страницу интерфейса чата.

### Потоковый чат (SSE)

**GET** `/api/chat/stream`

Потоковый чат через Server-Sent Events (SSE).

**Ответ**: поток серверных событий

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### Получение списка сессий

**GET** `/api/chat/conversations`

Возвращает список всех активных чат-сессий.

**Пример ответа**:
```json
{
  "conversations": [
    {
      "sessionId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
      "beingId": "being-uuid",
      "type": "single",
      "displayName": "Чат с Сяою",
      "lastMessage": "Содержание последнего сообщения",
      "lastTime": "2026-05-20T10:30:00Z"
    }
  ]
}
```

### Получение истории сообщений

**GET** `/api/chat/messages`

Параметр запроса: `channelId` — ID канала/сессии

Возвращает историю сообщений указанной сессии.

### Получение истории чата

**GET** `/api/chat/history`

Возвращает глобальную историю чата.

### Отправка сообщения

**POST** `/api/chat/send`

**Тело запроса**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
  "content": "Тестовое содержимое сообщения"
}
```

**Ответ**:
```json
{
  "success": true,
  "messageId": "50156b26-f3b9-4735-be3d-51e547bd3a4a"
}
```

### Остановка размышлений AI

**POST** `/api/chat/stop`

Останавливает текущую генерацию ответа AI.

**Тело запроса**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d"
}
```

### Загрузка файла

**POST** `/api/chat/upload`

Загружает файл в чат-сессию (поддерживается multipart/form-data).

---

## Управление Кремниевыми Существами

### Страница управления существами

**GET** `/beings`

Возвращает страницу интерфейса управления Кремниевыми Существами.

### Получение списка существ

**GET** `/api/beings` или **GET** `/api/beings/list`

Возвращает список всех зарегистрированных Кремниевых Существ.

**Пример ответа**:
```json
{
  "beings": [
    {
      "id": "being-uuid",
      "name": "Assistant",
      "status": "running",
      "soulPath": "path/to/soul.md"
    }
  ]
}
```

**Значения статуса**: `idle` | `running` | `waiting_permission` | `stopped`

### Получение подробных сведений о существе

**GET** `/api/beings/detail`

Параметр запроса: `beingId` — ID существа

Возвращает подробную информацию об указанном существе.

### Получение статуса активности существ

**GET** `/api/beings/activity`

Возвращает информацию о статусе активности каждого существа.

### Страница редактора файла души

**GET** `/beings/soul`

Возвращает интерфейс редактора файла души.

### Сохранение файла души

**POST** `/api/beings/soul/save`

**Тело запроса**:
```json
{
  "beingId": "being-uuid",
  "soulContent": "# Personality\nYou are helpful..."
}
```

### Страница редактора конфигурации AI

**GET** `/beings/ai-config`

Возвращает интерфейс редактора конфигурации AI.

### Сохранение конфигурации AI

**POST** `/api/beings/ai-config/save`

**Тело запроса**:
```json
{
  "beingId": "being-uuid",
  "aiClientType": "DashScope",
  "config": {
    "apiKey": "...",
    "region": "beijing",
    "model": "qwen3.6-plus"
  }
}
```

### Получение списка доступных моделей AI

**GET** `/api/beings/ai-config/models`

Параметры запроса: `clientType`, `apiKey`, `region`

Возвращает список доступных моделей указанного AI-клиента.

---

## Просмотр истории чата

### Страница истории чата

**GET** `/chat-history`

Возвращает главную страницу истории чата.

### Страница подробной истории чата

**GET** `/chat-history-detail`

Возвращает страницу подробной истории чата указанной сессии.

### Страница подробной истории группового чата

**GET** `/group-chat-history-detail`

Возвращает страницу подробной истории группового чата.

### Страница подробной истории канала вещания

**GET** `/broadcast-history-detail`

Возвращает страницу подробной истории канала вещания.

### Получение списка прошлых сессий

**GET** `/api/chat-history/conversations`

Возвращает список всех прошлых сессий.

### Получение прошлых сообщений

**GET** `/api/chat-history/messages`

Параметр запроса: `sessionId` — ID сессии

Возвращает записи сообщений указанной прошлой сессии.

---

## Управление таймерами

### Страница таймеров

**GET** `/timers`

Возвращает страницу интерфейса управления таймерами.

### Получение списка таймеров

**GET** `/api/timers/list`

Возвращает список всех таймеров.

### Страница подробных сведений о циклах таймера

**GET** `/timer-cycles/{timerId}`

Возвращает страницу подробных сведений о циклах выполнения указанного таймера.

### Получение списка циклов таймера

**GET** `/api/timer-cycles/list`

Параметр запроса: `timerId` — ID таймера

Возвращает список всех циклов выполнения указанного таймера.

### Страница подробных сведений об отдельном цикле выполнения

**GET** `/timer-cycle/{cycleIndex}`

Возвращает страницу подробных сведений об отдельном выполнении.

### Получение сообщений цикла

**GET** `/api/timer-cycle/messages`

Параметр запроса: `cycleIndex` — индекс цикла

Возвращает сообщения, связанные с указанным циклом выполнения.

---

## Управление задачами

### Страница задач

**GET** `/tasks`

Возвращает страницу интерфейса управления задачами.

### Получение списка задач

**GET** `/api/tasks/list`

Возвращает список всех задач.

### Страница подробных сведений о циклах задачи

**GET** `/task-cycles/{taskId}`

Возвращает страницу подробных сведений о циклах выполнения указанной задачи.

### Получение списка циклов задачи

**GET** `/api/task-cycles/list`

Параметр запроса: `taskId` — ID задачи

Возвращает список всех циклов выполнения указанной задачи.

### Страница подробных сведений об отдельном цикле выполнения

**GET** `/task-cycle/{cycleIndex}`

Возвращает страницу подробных сведений об отдельном выполнении задачи.

### Получение сообщений цикла

**GET** `/api/task-cycle/messages`

Параметр запроса: `cycleIndex` — индекс цикла

Возвращает сообщения, связанные с указанным циклом выполнения задачи.

---

## Система разрешений

### Страница управления разрешениями

**GET** `/permissions`

Возвращает страницу интерфейса управления разрешениями.

### Получение списка правил разрешений

**GET** `/api/permissions/list`

Возвращает все настроенные на данный момент правила разрешений.

**Пример ответа**:
```json
{
  "rules": [
    {
      "permissionType": "NetworkAccess",
      "resourcePrefix": "api.github.com",
      "result": "Allowed",
      "description": "Allow GitHub API access"
    }
  ]
}
```

### Сохранение правила разрешений

**POST** `/api/permissions/save`

**Тело запроса**:
```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects",
  "result": "Allowed",
  "description": "Allow project directory access"
}
```

### Страница запроса разрешений

**GET** `/permission/request`

Отображает страницу запроса разрешений, позволяя пользователю одобрить или отклонить запросы разрешений Кремниевого Существа.

**Параметры запроса**:

| Параметр | Тип | Описание |
|------|------|------|
| `userId` | `Guid` | ID Кремниевого Существа, запрашивающего разрешение |
| `type` | `string` | Тип разрешения |
| `resource` | `string` | Запрашиваемый путь к ресурсу |
| `allowCode` | `string` | Кодовый идентификатор для разрешения действия |
| `denyCode` | `string` | Кодовый идентификатор для отклонения действия |

### Проверка ожидающих запросов разрешений

**GET** `/permission/check`

Параметр запроса: `userId` — ID Кремниевого Существа

**Ответ**:
```json
{
  "pending": true
}
```

### Ответ на запрос разрешений

**GET** `/permission/respond`

**Параметры запроса**:

| Параметр | Тип | Описание |
|------|------|------|
| `userId` | `Guid` | ID Кремниевого Существа |
| `allowed` | `bool` | Разрешить или нет |
| `addToCache` | `bool` | Кэшировать ли решение |
| `cacheDuration` | `double` | Длительность кэширования (в часах) |

**Ответ**:
```json
{
  "success": true
}
```

---

## Система журналирования

### Страница журналов

**GET** `/logs`

Возвращает страницу интерфейса просмотра журналов.

### Получение списка журналов

**GET** `/api/logs/list`

Параметры запроса поддерживают фильтрацию по уровню и диапазону времени.

**Пример ответа**:
```json
{
  "logs": [
    {
      "timestamp": "2026-04-20T10:30:00Z",
      "level": "error",
      "message": "Failed to connect to AI service",
      "source": "OllamaClient"
    }
  ]
}
```

### Получение журналов, сгруппированных по существам

**GET** `/api/logs/beings`

Статистика журналов, сгруппированная по Кремниевым Существам.

### Получение доступных уровней журналов

**GET** `/api/logs/levels`

Возвращает список доступных в системе уровней журналов.

---

## Статистика использования

### Страница статистики использования

**GET** `/usage`

Возвращает страницу интерфейса статистики использования.

### Получение сводки использования

**GET** `/api/usage/summary`

Возвращает сводку использования токенов и расходов.

### Получение данных трендов

**GET** `/api/usage/trend`

Параметры запроса: `startDate`, `endDate`

Возвращает данные трендов использования за указанный период времени.

### Экспорт данных использования

**GET** `/api/usage/export`

Экспортирует данные использования в загружаемом формате.

---

## Аудиторский след

### Страница аудита

**GET** `/audit`

Возвращает страницу интерфейса аудиторского следа.

### Получение списка аудита

**GET** `/api/audit/list`

Возвращает список записей журнала аудита.

### Получение сводки аудита

**GET** `/api/audit/summary`

Возвращает сводную статистику данных аудита.

### Получение аудита, сгруппированного по существам

**GET** `/api/audit/beings`

Статистика аудита, сгруппированная по Кремниевым Существам.

---

## Управление конфигурацией

### Страница конфигурации

**GET** `/config`

Возвращает страницу интерфейса конфигурации системы.

### Сохранение конфигурации

**POST** `/config/save`

**Тело запроса**:
```json
{
  "language": "ZhCN",
  "port": 8080,
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:7b"
    },
    "DashScope": {
      "apiKey": "...",
      "region": "beijing",
      "model": "qwen3.6-plus"
    },
    "VolcengineArk": {
      "apiKey": "...",
      "endpoint": "...",
      "model": "..."
    },
    "Herdsman": {
      "endpoint": "http://localhost:8000",
      "model": "..."
    },
    "LongCat": {
      "apiKey": "...",
      "endpoint": "https://api.longcat.chat/openai",
      "model": "LongCat-2.0"
    },
    "QiniuAI": {
      "apiKey": "...",
      "endpoint": "...",
      "model": "..."
    },
    "DeepSeek": {
      "apiKey": "...",
      "model": "..."
    },
    "Zhipu": {
      "apiKey": "...",
      "model": "..."
    },
    "Ernie": {
      "apiKey": "...",
      "model": "..."
    },
    "Hunyuan": {
      "apiKey": "...",
      "model": "..."
    },
    "MiniMax": {
      "apiKey": "...",
      "model": "..."
    },
    "Moonshot": {
      "apiKey": "...",
      "model": "..."
    },
    "SiliconFlow": {
      "apiKey": "...",
      "model": "..."
    }
  },
  "imPlatforms": [
    {
      "platform": "webui",
      "enabled": true,
      "config": {}
    },
    {
      "platform": "feishu",
      "enabled": true,
      "config": {
        "appId": "cli_xxx",
        "appSecret": "${FEISHU_APP_SECRET}",
        "verificationToken": "..."
      }
    }
  ]
}
```

`imPlatforms` — список с несколькими экземплярами: каждый элемент представляет собой экземпляр IM-платформы; можно одновременно включить несколько платформ (каждая включается и отключается независимо). Плейсхолдеры `${ENV_VAR}` в `config` разрешаются во время выполнения из переменных окружения; секреты в открытом виде не записываются обратно в config.json.

### Получение параметров конфигурации AI

**GET** `/config/aioptions`

Возвращает доступные типы AI-клиентов и их динамические параметры (доступные модели, регионы и т.д.).

### Получение параметров IM-платформ

**GET** `/config/imoptions`

Возвращает метаданные IM-платформ (для динамической отрисовки форм мастером настройки):

```json
{
  "success": true,
  "platforms": [
    {
      "value": "feishu",
      "display": "Feishu",
      "authModes": ["manual", "oauth"],
      "needsPublicCallback": false,
      "help": "...",
      "helpUrl": "https://open.feishu.cn/app",
      "fields": [
        { "key": "appId", "label": "App ID", "type": "text", "required": true },
        { "key": "appSecret", "label": "App Secret", "type": "password", "required": true, "isSecret": true }
      ]
    }
  ]
}
```

### Просмотр конфигурации

**GET** `/config/browse`

Возвращает данные просмотра элементов конфигурации (для отображения по группам в интерфейсе настройки).

---

## Система памяти

### Страница памяти

**GET** `/memory`

Возвращает страницу интерфейса управления памятью.

### Получение списка записей памяти

**GET** `/api/memory/list`

Возвращает список записей памяти Кремниевого Существа.

### Получение подробных сведений о записи памяти

**GET** `/api/memory/detail/{id}`

Параметр пути: `id` — ID записи памяти

Возвращает полное содержимое указанной записи памяти.

### Получение статистики памяти

**GET** `/api/memory/stats`

Возвращает статистическую информацию о системе памяти.

### Поиск в памяти

**GET** `/api/memory/search`

Параметр запроса: `keyword` — ключевое слово поиска

Ищет совпадающие записи памяти.

### Получение памяти, сгруппированной по существам

**GET** `/api/memory/beings`

Статистика памяти, сгруппированная по Кремниевым Существам.

### Получение трассировки памяти

**GET** `/api/memory/trace/{id}`

Параметр пути: `id` — ID записи памяти

Возвращает цепочку трассировки источника указанной записи памяти.

### Получение HTML временной шкалы памяти

**GET** `/api/memory/timeline-html`

Возвращает HTML-представление временной шкалы памяти.

---

## Рабочие заметки

### Страница рабочих заметок

**GET** `/work-notes`

Возвращает страницу интерфейса рабочих заметок.

### Получение списка рабочих заметок

**GET** `/api/work-notes/list`

Возвращает список рабочих заметок.

### Чтение рабочей заметки

**GET** `/api/work-notes/read`

Параметр запроса: `noteId` — ID заметки

Возвращает содержимое указанной заметки.

### Получение каталога заметок

**GET** `/api/work-notes/directory`

Возвращает структуру каталога заметок.

### Поиск рабочих заметок

**GET** `/api/work-notes/search`

Параметр запроса: `keyword` — ключевое слово поиска

Ищет совпадающие рабочие заметки.

### Создание рабочей заметки

**POST** `/api/work-notes/create`

**Тело запроса**:
```json
{
  "title": "Заголовок заметки",
  "content": "Содержание заметки",
  "keywords": ["Ключевое слово 1", "Ключевое слово 2"]
}
```

### Обновление рабочей заметки

**POST** `/api/work-notes/update`

**Тело запроса**:
```json
{
  "noteId": "note-uuid",
  "title": "Обновленный заголовок",
  "content": "Обновленное содержание"
}
```

### Удаление рабочей заметки

**POST** `/api/work-notes/delete`

**Тело запроса**:
```json
{
  "noteId": "note-uuid"
}
```

---

## Сеть знаний

### Страница сети знаний

**GET** `/knowledge`

Возвращает страницу интерфейса управления сетью знаний.

### Получение графа знаний

**GET** `/api/knowledge/graph`

Возвращает данные графа триплетов знаний (субъект-отношение-объект).

---

## Управление проектами

### Страница проектов

**GET** `/project`

Возвращает страницу интерфейса управления проектами.

### Страница рабочих заметок проекта

**GET** `/project/{id}/work-notes`

Параметр пути: `id` — ID проекта

Возвращает страницу рабочих заметок указанного проекта.

### Страница задач проекта

**GET** `/project/{id}/tasks`

Параметр пути: `id` — ID проекта

Возвращает страницу управления задачами указанного проекта.

### Страница разрешений инструментов проекта

**GET** `/project/{id}/tool-permissions`

Параметр пути: `id` — ID проекта

Возвращает страницу управления разрешениями инструментов указанного проекта.

### Страница рабочего процесса проекта

**GET** `/project/{id}/workflow`

Параметр пути: `id` — ID проекта

Возвращает страницу управления рабочим процессом указанного проекта.

### Получение подробных сведений о рабочем процессе проекта

**GET** `/api/projects/workflow-detail`

Параметр запроса: `projectId` — ID проекта

Возвращает подробные сведения о рабочем процессе, связанном с проектом.

### Назначение роли в проекте

**POST** `/api/projects/assign-role`

**Тело запроса**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### Удаление роли из проекта

**POST** `/api/projects/remove-role`

**Тело запроса**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### Получение списка проектов

**GET** `/api/projects/list`

Возвращает список всех проектов.

### Получение списка шаблонов рабочих процессов проекта

**GET** `/api/projects/list-workflow-templates`

Возвращает список доступных шаблонов рабочих процессов.

### Создание проекта

**POST** `/api/projects/create`

**Тело запроса**:
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### Архивация проекта

**POST** `/api/projects/{id}/archive`

Параметр пути: `id` — ID проекта

Архивирует указанный проект.

### Восстановление проекта

**POST** `/api/projects/{id}/restore`

Параметр пути: `id` — ID проекта

Восстанавливает архивированный проект.

### Уничтожение проекта

**POST** `/api/projects/{id}/destroy`

Параметр пути: `id` — ID проекта

Безвозвратно удаляет указанный проект (восстановление невозможно).

### Получение подробных сведений о проекте

**GET** `/api/projects/detail`

Параметр запроса: `projectId` — ID проекта

Возвращает подробную информацию о проекте.

### Обновление проекта

**POST** `/api/projects/update`

**Тело запроса**:
```json
{
  "projectId": "project-uuid",
  "name": "Updated Name",
  "description": "Updated description"
}
```

### Назначение участника в проект

**POST** `/api/projects/assign`

**Тело запроса**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Удаление участника из проекта

**POST** `/api/projects/remove`

**Тело запроса**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Получение списка рабочих заметок проекта

**GET** `/api/projects/{id}/work-notes/list`

Параметр пути: `id` — ID проекта

Возвращает список рабочих заметок указанного проекта.

### Чтение рабочей заметки проекта

**GET** `/api/projects/{id}/work-notes/read`

Параметр пути: `id` — ID проекта

Возвращает содержимое рабочей заметки указанного проекта.

### Создание рабочей заметки проекта

**POST** `/api/projects/{id}/work-notes/create`

Параметр пути: `id` — ID проекта

Создает новую рабочую заметку в указанном проекте.

### Обновление рабочей заметки проекта

**POST** `/api/projects/{id}/work-notes/update`

Параметр пути: `id` — ID проекта

Обновляет рабочую заметку в указанном проекте.

### Удаление рабочей заметки проекта

**POST** `/api/projects/{id}/work-notes/delete`

Параметр пути: `id` — ID проекта

Удаляет рабочую заметку в указанном проекте.

### Получение списка задач проекта

**GET** `/api/projects/{id}/tasks/list`

Параметр пути: `id` — ID проекта

Возвращает список задач указанного проекта.

### Создание задачи проекта

**POST** `/api/projects/{id}/tasks/create`

Параметр пути: `id` — ID проекта

Создает новую задачу в указанном проекте.

### Обновление задачи проекта

**POST** `/api/projects/{id}/tasks/update`

Параметр пути: `id` — ID проекта

Обновляет задачу в указанном проекте.

### Удаление задачи проекта

**POST** `/api/projects/{id}/tasks/delete`

Параметр пути: `id` — ID проекта

Удаляет задачу в указанном проекте.

### Назначение ответственного за задачу

**POST** `/api/projects/{id}/tasks/assign`

Параметр пути: `id` — ID проекта

Назначает ответственного за задачу проекта.

### Удаление ответственного за задачу

**POST** `/api/projects/{id}/tasks/remove-assignee`

Параметр пути: `id` — ID проекта

Удаляет ответственного за задачу проекта.

### Отметка задачи как выполненной

**POST** `/api/projects/{id}/tasks/complete`

Параметр пути: `id` — ID проекта

Отмечает задачу проекта как выполненную.

### Отметка задачи как неудачной

**POST** `/api/projects/{id}/tasks/fail`

Параметр пути: `id` — ID проекта

Отмечает задачу проекта как неудачную.

### Отмена задачи

**POST** `/api/projects/{id}/tasks/cancel`

Параметр пути: `id` — ID проекта

Отменяет задачу проекта.

---

## Управление разрешениями инструментов

### Получение разрешений инструментов Кремниевого Существа

**GET** `/api/beings/tool-permissions`

Параметр запроса: `beingId` — ID Кремниевого Существа

Возвращает конфигурацию разрешений инструментов указанного Кремниевого Существа.

### Обновление разрешений инструментов Кремниевого Существа

**PUT** `/api/beings/tool-permissions`

**Тело запроса**:
```json
{
  "beingId": "being-uuid",
  "permissions": {
    "network": "allowed",
    "disk_read": "allowed",
    "disk_write": "denied"
  }
}
```

### Получение шаблонов разрешений инструментов

**GET** `/api/beings/tool-permissions/templates`

Возвращает список доступных шаблонов разрешений инструментов.

### Применение шаблона разрешений инструментов

**POST** `/api/beings/tool-permissions/apply-template`

**Тело запроса**:
```json
{
  "beingId": "being-uuid",
  "templateName": "readonly"
}
```

### Получение разрешений инструментов проекта

**GET** `/api/projects/{id}/tool-permissions`

Параметр пути: `id` — ID проекта

Возвращает конфигурацию разрешений инструментов указанного проекта.

### Обновление разрешений инструментов проекта

**PUT** `/api/projects/{id}/tool-permissions`

Параметр пути: `id` — ID проекта

**Тело запроса**:
```json
{
  "permissions": {
    "network": "allowed",
    "disk_read": "allowed",
    "disk_write": "denied"
  }
}
```

---

## Управление исполнителями

### Страница исполнителей

**GET** `/executor`

Возвращает страницу интерфейса управления исполнителями.

### Получение статуса исполнителей

**GET** `/api/executors/status`

Возвращает статус работы исполнителей (диск, сеть, командная строка).

---

## Браузер кода

### Страница браузера кода

**GET** `/code`

Возвращает страницу интерфейса браузера кода.

### Получение списка типов кода

**GET** `/api/code/types`

Возвращает список поддерживаемых типов кода/языков.

### Получение подробных сведений о коде

**GET** `/api/code/detail`

Параметры запроса: `filePath`, `lineNumber`

Возвращает подробные сведения о коде указанного файла.

---

## Всплывающие подсказки кода

### Получение всплывающей подсказки

**GET** `/api/code/hover`
**POST** `/api/code/hover`

Получает информацию всплывающей подсказки для позиции в коде (аналогично интеллектуальным подсказкам IDE).

### Регистрация позиции в коде

**POST** `/api/code/register`

Регистрирует позиции в коде, требующие мониторинга.

### Обновление позиции в коде

**POST** `/api/code/update`

Обновляет информацию о зарегистрированных позициях в коде.

### Отмена регистрации позиции в коде

**POST** `/api/code/unregister`

Отменяет регистрацию мониторинга позиций в коде, которые больше не нужны.

---

## Управление навыками

### Страница управления навыками

**GET** `/skill` или **GET** `/skill/index`

Параметр запроса: `beingId` — ID существа (обязательно)

Возвращает страницу управления навыками указанного Кремниевого Существа (список навыков + редактор Markdown).

### Получение списка навыков

**GET** `/api/skills/list`

Параметр запроса: `beingId` — ID существа (обязательно)

Возвращает все навыки существа (id, description, version, tags, source, triggerMode, toolWhitelist, maxToolRound, timeoutSeconds, parameterCount), а также статистическую информацию (общее число навыков / число пользовательских навыков / предельная квота).

### Получение Markdown навыка

**GET** `/api/skills/get-md`

Параметры запроса: `beingId`, `skillId`

Возвращает Markdown-текст указанного навыка (метаданные YAML front matter + основной текст промпта).

### Сохранение Markdown навыка

**POST** `/api/skills/update-md?beingId={beingId}`

Тело запроса (`application/json`):

```json
{
  "markdown": "---\nid: my_skill\n...\n---\n\nОсновной текст промпта",
  "skillId": "my_skill"
}
```

Обновляет или создает навык на основе Markdown (семантика upsert). Отсутствующие метаданные автоматически дополняются AI; для навыков, сохраненных через Web UI, `Source` помечается как `User`. Ограничено квотой `MaxCustomSkillsPerBeing`.

### Импорт навыка (JSON)

**POST** `/api/skills/import?beingId={beingId}`

Тело запроса: `{ "json": "<JSON определения навыка>" }`

Импортирует навык из JSON, также ограничивается квотой.

### Импорт навыка (Markdown)

**POST** `/api/skills/import-md?beingId={beingId}`

Тело запроса: `{ "markdown": "<Markdown-текст>" }`

Импортирует новый навык из Markdown; отсутствующие метаданные автоматически дополняются AI.

### Удаление навыка

**POST** `/api/skills/delete?beingId={beingId}`

Тело запроса: `{ "skillId": "my_skill" }`

Удаляет навык (одновременно удаляются соответствующие файлы хранения `.md` и `.json`).

### Экспорт навыка (JSON)

**GET** `/api/skills/export?beingId={beingId}&skillId={skillId}`

Скачивает определение навыка в виде JSON-вложения (`{id}.json`).

### Экспорт навыка (Markdown)

**GET** `/api/skills/export-md?beingId={beingId}&skillId={skillId}`

Скачивает навык в виде Markdown-вложения (`{id}.md`).

### Тестовое выполнение навыка

**POST** `/api/skills/test?beingId={beingId}`

Тело запроса:

```json
{
  "skillId": "my_skill",
  "parametersJson": "{ \"topic\": \"Новости AI\" }"
}
```

Выполняет навык один раз с заданными параметрами и возвращает `ToolResult` (включая количество раундов выполнения AI и итоговый вывод).

---

## Управление MCP

### Страница управления MCP

**GET** `/mcp`

Параметр запроса: `beingId` — ID существа (необязательно; используется для отображения инструментов MCP, видимых этому существу)

Возвращает страницу управления серверами MCP.

### Получение списка серверов

**GET** `/api/mcp/list-servers`

Возвращает статус всех настроенных серверов MCP:

```json
{
  "success": true,
  "data": [
    {
      "id": "filesystem",
      "name": "Filesystem",
      "transport": "stdio",
      "state": "connected",
      "enabled": true,
      "toolCount": 8,
      "endpoint": null,
      "lastError": null
    }
  ],
  "mcpEnabled": true,
  "connected": 1,
  "toolTotal": 8
}
```

Возможные значения `state`: `connected` / `disconnected` / `connecting` / `error`.

### Получение списка инструментов сервера

**GET** `/api/mcp/list-tools?serverId={serverId}`

Возвращает инструменты, предоставляемые указанным сервером (`name` — полное имя с префиксом `mcp_{serverId}_{toolName}`, `description`, `schema`). Если сервер не подключен, возвращает ошибку.

### Добавление сервера

**POST** `/api/mcp/add-server`

Тело запроса (`McpServerConfig`):

```json
{
  "id": "filesystem",
  "name": "Filesystem",
  "transport": "stdio",
  "command": "npx",
  "arguments": ["-y", "@modelcontextprotocol/server-filesystem", "/data"],
  "env": {},
  "endpoint": null,
  "enabled": true
}
```

`transport` поддерживает `stdio` (локальный процесс: `command` + `arguments`) и `http` (удаленный эндпоинт: `endpoint`). В ID сервера допускаются только строчные буквы, цифры и знак подчеркивания. После добавления сразу выполняется подключение и синхронизация со всеми Кремниевыми Существами.

### Включение/отключение сервера

**POST** `/api/mcp/toggle`

Тело запроса: `{ "serverId": "filesystem", "enabled": true }`

### Удаление сервера

**POST** `/api/mcp/remove-server`

Тело запроса: `{ "serverId": "filesystem" }`

Удаляет конфигурацию сервера и отменяет регистрацию его инструментов у всех существ.

### Переподключение сервера

**POST** `/api/mcp/reconnect`

Тело запроса: `{ "serverId": "filesystem" }`

Принудительно разрывает соединение и устанавливает его заново, обновляя список инструментов.

### Тестовый вызов инструмента

**POST** `/api/mcp/test-tool`

Тело запроса:

```json
{
  "serverId": "filesystem",
  "toolName": "read_file",
  "argumentsJson": "{ \"path\": \"/data/hello.txt\" }"
}
```

Вызывает инструмент сервера MCP напрямую (без участия AI); используется для проверки соединения.

---

## OAuth-авторизация IM-платформ

### Инициирование авторизации

**GET** `/im/{platform}/authorize`

Параметр пути: `platform` — идентификатор IM-платформы (например, `feishu`)

Генерирует случайный `state` для защиты от CSRF, регистрирует сессию авторизации сроком действия 5 минут, возвращает URL авторизации и автоматически открывает системный браузер по умолчанию. Повторная инициация для той же платформы перезаписывает старую сессию.

### Обратный вызов авторизации

**GET** `/im/{platform}/callback?code={code}&state={state}`

Вызывается редиректом со стороны IM-платформы. После проверки `state` обменивает код авторизации на токен доступа, записывает `accessToken`, `refreshToken`, `tokenExpiresAt`, `authMode=oauth` обратно в конфигурацию платформы и сохраняет ее, после чего отображает целевую страницу результата авторизации (успех/провал).

### Запрос статуса авторизации

**GET** `/im/{platform}/status`

Возвращает `{ platform, status, tokenExpiresAt }`. Возможные значения `status`: `pending` / `success` / `failed` / `timeout` / `none`. Фронтенд в первую очередь получает push-обновления статуса через SSE-событие `im_auth_status`; этот интерфейс служит резервным механизмом опроса.

---

## Система справочной документации

### Страница справки

**GET** `/help` или **GET** `/help/index`

Возвращает главную страницу справочной документации.

### Страница темы справки

**GET** `/help/{topic}`

Параметр пути: `topic` — идентификатор темы

Возвращает страницу справочной документации указанной темы.

### Поиск в справочной документации

**GET** `/api/help/search`

Параметр запроса: `keyword` — ключевое слово поиска

Ищет совпадающие темы справочной документации.

---

## Инициализация

### Страница мастера инициализации

**GET** `/init`

Возвращает страницу мастера инициализации первого запуска.

### Отправка инициализации

**POST** `/init`

Отправляет конфигурацию инициализации первого запуска.

### Обзор и выбор каталога данных

**GET** `/init/browse`

Открывает обозреватель каталогов для выбора места хранения данных.

### Получение метаданных конфигурации AI

**GET** `/init/ai-config-metadata`

Возвращает доступные типы AI-клиентов и метаданные их полей конфигурации.

---

## Управление системой

### Корректное завершение работы

**POST** `/api/system/shutdown`

> **Примечание**: разрешены только запросы с localhost

Запускает процесс корректного завершения работы приложения:

1. Остановка главного цикла (MainLoop)
2. Сохранение текущей конфигурации
3. Закрытие HTTP-прослушивателя

**Ответ**:
```json
{
  "status": "shutting_down",
  "message": "Application is shutting down gracefully"
}
```

---

## О программе

### Страница «О программе»

**GET** `/about`

Возвращает страницу «О программе», содержащую информацию о системе и список загруженных плагинов.

**Данные списка плагинов**:
```json
{
  "plugins": {
    "plugin-id": {
      "name": "My Plugin",
      "version": "1.0.0",
      "description": "Plugin description",
      "author": "Author Name"
    }
  }
}
```

---

## Ответы об ошибках

Все эндпоинты возвращают стандартизированные ответы об ошибках:

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: FileAccess, Denied by GlobalACL"
  }
}
```

### Распространенные коды ошибок

| Код | Статус HTTP | Описание |
|------|-------------|-------------|
| `PERMISSION_DENIED` | 403 | Недостаточно разрешений |
| `NOT_FOUND` | 404 | Ресурс не найден |
| `VALIDATION_ERROR` | 400 | Недопустимые параметры запроса |
| `INTERNAL_ERROR` | 500 | Внутренняя ошибка сервера |
| `SERVICE_UNAVAILABLE` | 503 | Служба AI недоступна |

---

## События SSE

События, отправляемые сервером, используются для обновлений в реальном времени:

### События чата

```javascript
const eventSource = new EventSource('/api/chat/stream');

eventSource.onmessage = (event) => {
  const data = JSON.parse(event.data);
  
  switch(data.type) {
    case 'chunk':
      console.log('Streaming:', data.content);
      break;
    case 'tool_call':
      console.log('Tool executing:', data.tool);
      break;
    case 'complete':
      console.log('Chat complete, session:', data.sessionId);
      break;
    case 'error':
      console.error('Error:', data.message);
      break;
  }
};
```

### События статуса IM-авторизации

Мастер OAuth-авторизации IM-платформы отправляет статус через общее SSE-соединение (имя события `im_auth_status`):

```javascript
eventSource.addEventListener('im_auth_status', (event) => {
  const data = JSON.parse(event.data);
  // data.platform — идентификатор платформы (feishu / wecom / dingtalk)
  // data.status  — pending / success / failed / timeout
  // data.message — дополнительные пояснения
  updateAuthStatus(data.platform, data.status);
});
```

---

## Интерфейсы AI-клиентов

### Интерфейс IAIClient

```csharp
public interface IAIClient
{
    string Endpoint { get; }
    string DefaultModel { get; }
    bool? StreamingMode { get; }
    bool? SupportsToolCalls { get; }
    int? ContextWindowTokens { get; }
    bool? SupportsVision { get; }
    bool? SupportsAudio { get; }
    
    AIResponse Chat(AIRequest request);
}
```

| Свойство | Тип | Описание |
|------|------|------|
| `Endpoint` | `string` | URL эндпоинта службы AI |
| `DefaultModel` | `string` | Название модели по умолчанию |
| `StreamingMode` | `bool?` | Потоковый режим: true=только потоковый, false=только непотоковый, null=поддерживаются оба |
| `SupportsToolCalls` | `bool?` | Поддержка вызова инструментов: true=поддерживается, false=не поддерживается (внедрение инструментов пропускается), null=неизвестно |
| `ContextWindowTokens` | `int?` | Размер контекстного окна (в токенах), используется для усечения по бюджету токенов |
| `SupportsVision` | `bool?` | Поддержка визуального ввода: true=поддерживаются изображения, false=не поддерживается, null=неизвестно |
| `SupportsAudio` | `bool?` | Поддержка аудиоввода: true=поддерживается аудио, false=не поддерживается, null=неизвестно |

### Структура AIRequest

```csharp
public class AIRequest
{
    public List<Message> Messages { get; set; }
    public List<ToolDefinition> Tools { get; set; }
    public double Temperature { get; set; } = 0.7;
    public int MaxTokens { get; set; } = 2000;
    public string Model { get; set; }
}
```

### Структура AIResponse

```csharp
public class AIResponse
{
    public string Content { get; set; }
    public List<ToolCall> ToolCalls { get; set; }
    public TokenUsage Usage { get; set; }
    public string Model { get; set; }
}
```

---

## Интерфейсы системы инструментов

### Интерфейс ITool

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    ToolDefinition Definition { get; }
    
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### Структура ToolCall

```csharp
public class ToolCall
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}
```

### Структура ToolResult

```csharp
public class ToolResult
{
    public bool Success { get; set; }
    public string Output { get; set; }
    public string Error { get; set; }
}
```

---

## Следующие шаги

- 🚀 Ознакомьтесь с [руководством по быстрому началу работы](getting-started.md)
- 🛠️ Прочтите [руководство по разработке](development-guide.md)
- 📚 Ознакомьтесь с [документацией по архитектуре](architecture.md)
- 🔒 Узнайте больше о [модели безопасности](security.md)
