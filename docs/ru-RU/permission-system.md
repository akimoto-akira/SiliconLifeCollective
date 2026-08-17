# Система разрешений

> **Версия: v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | **Русский**

## Обзор

Система разрешений обеспечивает надлежащую проверку и аудит всех операций, инициированных AI.

## Цепочка проверки разрешений

```
┌─────────────────────────────────────────────┐
│          Проверка разрешений                 │
├─────────────────────────────────────────────┤
│  Уровень 1: UserFrequencyCache               │
│  ↓ Кэш высокочастотных пользовательских решений (HighDeny/HighAllow) │
│  Уровень 2: IPermissionCallback              │
│  ↓ Пользовательская логика (Allowed/Denied/AskUser) │
│  Уровень 3: IsCurator?                       │
│  ↓ Да → IPermissionAskHandler (запрос пользователю) │
│  ↓ Нет → GlobalACL → отказ по умолчанию      │
│  Результат: разрешено или отказано            │
└─────────────────────────────────────────────┘
```

> **Примечание**: Фактический приоритет запросов `PermissionManager.CheckPermission()`:
> 1. **UserFrequencyCache** — сначала проверяется кэш высокочастотных пользовательских решений
> 2. **IPermissionCallback** — оценка пользовательских правил обратного вызова
> 3. **Ветвление Куратора** — когда обратный вызов возвращает AskUser или обратный вызов отсутствует:
>    - **Куратор** → `IPermissionAskHandler` (запрос пользователю через IM)
>    - **Не куратор** → `GlobalACL` → отказ по умолчанию

## Уровень 1: UserFrequencyCache

Кэш высокочастотных пользовательских решений (HighDeny/HighAllow) для каждого существа, существует только в памяти.

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny имеет приоритет над HighAllow**
- **Только в памяти**: кэш не персистентен, теряется при перезапуске
- **Настраиваемое время истечения**: пользователь может установить срок действия записей кэша

## Уровень 2: IPermissionCallback

Пользовательский обратный вызов для динамической логики разрешений.

### Реализация по умолчанию DefaultPermissionCallback

`DefaultPermissionCallback` предоставляет комплексные правила разрешений по умолчанию, включая:

#### Правила сетевого доступа
- **Адреса обратной связи**: разрешены localhost, 127.0.0.1, ::1
- **Частные IP-адреса**:
  - 192.168.x.x (Класс C) - разрешено
  - 10.x.x.x (Класс A) - разрешено
  - 172.16-31.x.x (Класс B) - запрос пользователю
- **Белый список доменов**:
  - Поисковые системы: Google, Bing, DuckDuckGo, Yandex, Sogou и др.
  - AI-сервисы: OpenAI, Anthropic, HuggingFace, Ollama и др.
  - Сервисы для разработчиков: GitHub, StackOverflow, npm, NuGet и др.
  - Социальные сети: Weibo, Zhihu, Reddit, Discord и др.
  - Видеоплатформы: YouTube, Bilibili, Douyin, TikTok и др.
  - **Погодная информация**: wttr.in
  - Правительственные сайты: .gov, .go.jp, .go.kr
- **Чёрный список доменов**:
  - Сайты-подделки AI: chatgpt, openai, deepseek и др. фишинговые домены
  - Вредоносные AI-инструменты: wormgpt, darkgpt, fraudgpt и др.
  - AI-фермы контента и домены, связанные с чёрным рынком

```csharp
public class DefaultPermissionCallback : IPermissionCallback
{
    public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
    {
        if (IsSafeOperation(permissionType, resource))
        {
            return PermissionResult.Allowed;
        }
        
        return PermissionResult.AskUser;
    }
}
```

## Уровень 3: Ветвление (IsCurator / GlobalACL)

Когда обратный вызов возвращает `AskUser` или обратный вызов не настроен, система выполняет ветвление на основе статуса Куратора:

### Ветвь Куратора (IsCurator = true)

Для Кремниевого Куратора система запрашивает решение у пользователя через мгновенные сообщения:

```csharp
if (IsCurator)
{
    if (_askHandler != null)
    {
        AskPermissionResult userDecision = _askHandler.AskUser(callerId, permissionType, resource);
        // Пользователь подтверждает или отклоняет в Web UI
    }
}
```

### Ветвь не-Куратора (IsCurator = false)

Для существ, не являющихся Кураторами, система проверяет Глобальный ACL. Если подходящего правила не найдено, запрос отклоняется по умолчанию.

### Структура GlobalACL

```json
{
  "rules": [
    {
      "permissionType": "NetworkAccess",
      "resourcePrefix": "api.github.com",
      "result": "Allowed"
    },
    {
      "permissionType": "FileAccess",
      "resourcePrefix": "C:\\Windows",
      "result": "Denied"
    }
  ]
}
```

Правила оцениваются по порядку, первое совпадающее правило вступает в силу. Только Кремниевый Куратор может изменять Глобальный ACL.

### Формат ресурсов

```
{type}:{path}

Примеры:
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## IPermissionAskHandler

Когда операции Куратора требуют подтверждения пользователя, разрешения запрашиваются через `IPermissionAskHandler`.

### Реализация IMPermissionAskHandler

`IMPermissionAskHandler` отправляет запросы на разрешения пользователю через Web UI:

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        // Отправка сообщения пользователю через мгновенные сообщения
        SendMessageAsync($"Allow {resource}?");

        // Ожидание ответа пользователя
        var response = WaitForResponseAsync();

        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

### Очередь Запросов Разрешений

`PermissionRequestQueue` управляет ожидающими запросами на разрешения, поддерживая асинхронное ожидание ответа пользователя:

- **Постановка в очередь** — когда цепочка разрешений достигает уровня 5, создаётся `TaskCompletionSource<AskPermissionResult>` и ставится в очередь
- **Отображение в Web UI** — ожидающие запросы на разрешения отображаются через `PermissionRequestController`
- **Ответ пользователя** — пользователь одобряет или отклоняет в Web UI, с возможностью кэширования решения и установки длительности кэша
- **Параметры кэша** — пользователь может кэшировать решение о разрешении на 1 час, 24 часа, 7 дней или 30 дней
- **Механизм тайм-аута** — автоматическое закрытие страницы запроса при отсутствии ответа в течение 60 секунд

## Система аудита

Все решения о разрешениях регистрируются:

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "callerId": "being-uuid",
  "permissionType": "FileAccess",
  "resource": "C:\\data\\config.json",
  "result": "Allowed",
  "reason": "Global ACL"
}
```

## Программная оценка разрешений

### EvaluatePermission API

Метод `PermissionManager.EvaluatePermission()` предоставляет оценку разрешений только для чтения, не вызывая запросы пользователю. `PermissionTool` использует этот метод, чтобы позволить AI проверить статус разрешений перед попыткой выполнения операции.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**Возвращаемое значение**: трёхзначный `PermissionResult`:
- `Allowed` - операция разрешена
- `Denied` - операция отклонена
- `AskUser` - при выполнении потребуется подтверждение пользователя

**Порядок оценки**:
1. **Кэш частоты** - проверка кэшированных пользовательских решений
2. **IPermissionCallback** - оценка пользовательского обратного вызова
3. **Статус Куратора** - если Куратор, возвращается `AskUser` (требуется подтверждение)
4. **Глобальный ACL** - проверка правил контроля доступа
5. **По умолчанию** - отказ при отсутствии подходящего правила

> **Примечание**: в отличие от полной цепочки разрешений, `EvaluatePermission` **не** вызывает `IPermissionAskHandler`. Он только сообщает, каким *будет* результат при выполнении.

## Управление разрешениями

### Предоставление разрешений

**Через Web UI**:
1. Перейдите в **Управление разрешениями**
2. Нажмите **Добавить правило**
3. Настройте:
   - Пользователь
   - Ресурс
   - Разрешить/Отказать
   - Длительность

**Через API**:
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "permissionType": "FileAccess",
    "resourcePrefix": "C:\\Projects",
    "result": "Allowed",
    "description": "Allow project directory access"
  }'
```

### Отзыв разрешений

Через страницу управления разрешениями в Web UI.

### Просмотр разрешений

```bash
curl http://localhost:8080/api/permissions/list
```

## Система разрешений инструментов

Помимо цепочки проверки разрешений на уровне операций, система также предоставляет механизм управления **разрешениями инструментов** для контроля того, какие инструменты могут использовать Кремниевые Существа.

### Двухуровневые разрешения инструментов

Разрешения инструментов делятся на два уровня:

1. **Уровень Кремниевого Существа** — управление тем, какие операции инструментов может использовать отдельное Кремниевое Существо
2. **Уровень проекта** — управление доступными операциями инструментов в проектном пространстве, независимо от разрешений уровня Кремниевого Существа

### Конфигурация разрешений инструментов

Каждая операция каждого инструмента может быть независимо настроена как разрешённая или отклонённая:

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "network:get": "allowed",
    "network:post": "denied",
    "disk:read": "allowed",
    "disk:write": "denied",
    "database:query": "allowed"
  }
}
```

### Шаблоны разрешений

Система предоставляет предопределённые шаблоны разрешений инструментов для быстрого применения к Кремниевым Существам:

- **readonly** — разрешение только для чтения (разрешены операции чтения, отклонены операции записи)
- **full** — полные разрешения (разрешены все операции)
- **restricted** — ограниченные разрешения (разрешены только базовые операции)

### Управление через Web UI

Управление разрешениями инструментов через Web UI:

- **Страница разрешений инструментов Кремниевого Существа** — `/beings/tool-permissions`
- **Страница разрешений инструментов проекта** — `/project/{id}/tool-permissions`

### API-эндпоинты

| Эндпоинт | Метод | Описание |
|------|------|------|
| `/api/beings/tool-permissions` | GET | Получить разрешения инструментов Кремниевого Существа |
| `/api/beings/tool-permissions` | PUT | Обновить разрешения инструментов Кремниевого Существа |
| `/api/beings/tool-permissions/templates` | GET | Получить список шаблонов разрешений |
| `/api/beings/tool-permissions/apply-template` | POST | Применить шаблон разрешений |
| `/api/projects/{id}/tool-permissions` | GET | Получить разрешения инструментов проекта |
| `/api/projects/{id}/tool-permissions` | PUT | Обновить разрешения инструментов проекта |

### Разрешения на действия навыков

Навыки используют тот же механизм разрешений на действия инструментов: идентификатор навыка выступает в качестве имени инструмента, а действие — `execute`.

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "daily_news_digest:execute": "denied",
    "code_review:execute": "allowed"
  }
}
```

- Отключённые навыки не появляются в видимых для AI определениях инструментов (AI их просто «не видит»)
- Во время выполнения навыка проводится повторная проверка во время выполнения, поэтому даже устаревшая схема не может её обойти
- Разрешения инструментов внутри навыка = разрешения существа ∪ собственные ограничения навыка (пересечение по строгой стороне — может только сужать, но не расширять)

### Разрешения на действия обёрнутых инструментов MCP

Каждый обёрнутый инструмент, внедряемый MCP-сервером (`mcp_{serverId}_{toolName}`), автоматически объявляет единое действие `execute`:

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "mcp_filesystem_read_file:execute": "denied",
    "mcp_github_create_issue:execute": "allowed"
  }
}
```

- Доступность внешних инструментов можно точно контролировать по существу или проекту
- Когда все действия `execute` сервера отключены, инструмент полностью удаляется из схемы, видимой для AI
- Отключение/удаление сервера (операция через Web UI) немедленно отменяет регистрацию всех его инструментов

---

## Лучшие практики

### 1. Принцип минимальных привилегий

Предоставляйте только минимально необходимые разрешения:

```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects\\MyApp\\config.json",
  "result": "Allowed"
}
```

### 2. Использование разрешений с ограничением по времени

Никогда не предоставляйте постоянные разрешения, если это абсолютно необходимо.

### 3. Мониторинг журнала разрешений

Регулярно просматривайте аудитный журнал для понимания:
- Отклонённых попыток доступа
- Необычных паттернов
- Повышения привилегий

### 4. Реализация пользовательского обратного вызова

Для сложной логики используйте `IPermissionCallback`:

```csharp
public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
{
    // Разрешения на основе времени
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }
    
    // Разрешения на основе ресурса
    if (IsSensitiveResource(resource))
    {
        return PermissionResult.AskUser;
    }
    
    return PermissionResult.Allowed;
}
```

## Типичные сценарии

### Сценарий 1: AI хочет прочитать файл

```
AI: "Мне нужно прочитать config.json"
↓
Цепочка разрешений:
1. UserFrequencyCache? Нет кэшированного решения
2. IPermissionCallback? Возвращает AskUser (явно не разрешено)
3. IsCurator? Нет → Проверка GlobalACL
4. GlobalACL? Найдено правило: file:... = Allowed
5. Результат: Разрешено
```

### Сценарий 2: AI хочет выполнить код

```
AI: "Я хочу скомпилировать и запустить код"
↓
Цепочка разрешений:
1. UserFrequencyCache? Нет кэшированного решения
2. IPermissionCallback? Возвращает AskUser
3. IsCurator? Да → IPermissionAskHandler
4. Пользователь одобряет
5. Результат: Разрешено
```

### Сценарий 3: Кэшированный отказ

```
AI: "Мне нужно получить доступ к C:\Windows"
↓
Цепочка разрешений:
1. UserFrequencyCache? Найдено в кэше HighDeny
2. Результат: Отказано (дальнейшая проверка не требуется)
```

## Устранение неполадок

### Неожиданный отказ в разрешении

**Проверьте**:
1. Статус IsCurator пользователя
2. Записи HighDeny в кэше частоты
3. Правила GlobalACL
4. Логику обратного вызова
5. Тайм-аут ответа пользователя

### Разрешение не истекает

**Проверьте**:
- Поле `expiresAt` установлено правильно
- Часовой пояс корректен
- Синхронизация часов

### Аудитный журнал не записывается

**Проверьте**:
- Аудитный журнал зарегистрирован
- Бэкенд хранилища доступен
- Достаточно дискового пространства

## Следующие шаги

- 📚 Прочитайте [руководство по архитектуре](architecture.md)
- 🛠️ Ознакомьтесь с [руководством разработчика](development-guide.md)
- 🔒 Ознакомьтесь с [документацией по безопасности](security.md)
- 🚀 Ознакомьтесь с [руководством быстрого старта](getting-started.md)
