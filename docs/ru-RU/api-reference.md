# Справочник API

> **Версия: v0.2.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [Français](../fr-FR/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | **Русский** | [Čeština](../cs-CZ/api-reference.md)

## Конечные точки Web API

Базовый URL: `http://localhost:8080`

### Аутентификация

Большинство конечных точек требуют аутентификации через cookies сессии, управляемые веб-интерфейсом.

---

## Управление Silicon Beings

### Получить все Beings

**GET** `/api/beings`

**Ответ**:
```json
{
  "beings": [
    {
      "id": "being-uuid",
      "name": "Assistant",
      "activity": "Idle",
      "soul": "path/to/soul.md"
    }
  ]
}
```

**Значения активности**: `Idle` | `SingleChat` | `GroupChat` | `Task` | `Timer` | `Broadcast` | `Project` | `MemoryCompression` | `Stopped`

### Создать Being

**POST** `/api/beings`

**Запрос**:
```json
{
  "name": "New Being",
  "soul": "# Personality\nYou are helpful..."
}
```

**Ответ**: `201 Created`

### Запустить Being

**POST** `/api/beings/{id}/start`

### Остановить Being

**POST** `/api/beings/{id}/stop`

### Получить детали Being

**GET** `/api/beings/{id}`

---

## Система чата

### Отправить сообщение

**POST** `/api/chat/send`

**Запрос**:
```json
{
  "channelId": "channel-uuid",
  "content": "Привет, как дела?"
}
```

**Ответ**:
```json
{
  "success": true,
  "messageId": "message-uuid"
}
```

### Потоковый чат (SSE)

**GET** `/api/chat/stream`

Поток Server-Sent Events для обновлений чата в реальном времени.

### Получить разговоры

**GET** `/api/chat/conversations`

### Получить сообщения

**GET** `/api/chat/messages?channelId={sessionId}`

### Получить историю чата

**GET** `/api/chat/history`

Возвращает сессии истории чата.

### Остановить размышление

**POST** `/api/chat/stop`

Останавливает текущий потоковый ответ ИИ.

### Загрузить файл

**POST** `/api/chat/upload`

Загружает файл в сессию чата.

---

## Конфигурация

### Получить конфигурацию

**GET** `/api/config`

**Ответ**:
```json
{
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:7b"
    }
  },
  "storage": {
    "basePath": "./data"
  }
}
```

### Обновить конфигурацию

**POST** `/api/config`

**Запрос**:
```json
{
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:14b"
    }
  }
}
```

---

## Система разрешений

### Получить разрешения

**GET** `/api/permissions`

### Предоставить разрешение

**POST** `/api/permissions`

### Отозвать разрешение

**DELETE** `/api/permissions/{id}`

### Проверить разрешение

**POST** `/api/permissions/check`

---

## Ответы об ошибках

Все конечные точки возвращают стандартизированные ответы об ошибках:

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "У вас нет разрешения на доступ к этому ресурсу",
    "details": "Требуется: disk:write, Текущее: disk:read"
  }
}
```

### Коды распространённых ошибок

| Код | HTTP-статус | Описание |
|------|-------------|-------------|
| `PERMISSION_DENIED` | 403 | Недостаточно разрешений |
| `NOT_FOUND` | 404 | Ресурс не найден |
| `VALIDATION_ERROR` | 400 | Неверные параметры запроса |
| `INTERNAL_ERROR` | 500 | Внутренняя ошибка сервера |
| `SERVICE_UNAVAILABLE` | 503 | ИИ-сервис недоступен |

---

## Следующие шаги

- 🚀 См. [руководство по быстрому старту](getting-started.md)
- 🛠️ Прочитайте [руководство по разработке](development-guide.md)
- 📚 Обратитесь к [документации по архитектуре](architecture.md)
- 🔒 Поймите [модель безопасности](security.md)
