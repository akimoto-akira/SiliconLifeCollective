# PluginDemo-17: Стекинг возможностей — Множественные декларативные разрешения

## Обзор

Этот плагин демонстрирует стектинг нескольких атрибутов `[PluginCapability]` на одном классе плагина. `PluginCapabilityAttribute` имеет `AllowMultiple = true`, поэтому можно объявить столько возможностей, сколько необходимо.

## Синтаксис стекинга

```csharp
[PluginCapability(Capability.Network, Reason = "API endpoint access for remote AI models")]
[PluginCapability(Capability.AI, Reason = "AI service provider for downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

## Как PluginLoader обрабатывает стекинговые возможности

1. **Читает все декларации** из таблицы CustomAttribute метаданных PE
2. **Объединяет** правила освобождения всех декларированных возможностей
3. **Независимо регистрирует** каждую декларацию с собственным полем Reason
4. **По-прежнему обеспечивает** запреты недекларируемых возможностей независимо от стекинга

## Объединённые правила освобождения

При стекинге `Capability.Network` + `Capability.AI`:

| Источник | Освобождение |
|----------|-------------|
| Capability.Network | System.Net.Http.*, System.Net.WebSockets.*, System.Net.Sockets.*, System.Net.Mail.*, System.Net.NetworkInformation.*, System.Net.Security.*, System.Net (запреты на уровне типов) |
| Capability.AI | Внедрение IAIService включено |
| **Объединённый результат** | Плагин может использовать HttpClient И IAIService |

## Стекинг не даёт безграничной власти

Даже при стекинге нескольких возможностей эти остаются **всегда заблокированными**:

- ❌ P/Invoke (`DllImport`, `Marshal`, `NativeMemory`)
- ❌ Небезопасный код (`UnverifiableCodeAttribute`, `Unsafe`)
- ❌ IL-генерация (`System.Reflection.Emit.*`)
- ❌ Загрузка сборок (`System.Runtime.Loader`, `Assembly.Load*`)
- ❌ Реестр (`Microsoft.Win32.*`)

Для них не существует значений перечисления `Capability` — они **недекларируемы по конструкции**.

## Аудиторский след для стекинговых возможностей

Каждая возможность регистрируется независимо:

```
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.Network — reason: API endpoint access for remote AI models
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.AI — reason: AI service provider for downstream plugins
```

## Файлы

- `Plugin.cs` — Демо-плагин со стекингом Capability.Network + Capability.AI
- `README.md` — Этот файл (Английский)
- `README.zh-CN.md` — Упрощённый китайский
- Переводы: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Связанные примеры

- **13-CapabilityNetwork**: Одиночная возможность Network
- **16-CapabilityAI**: Одиночная возможность AI
- **18-CapabilityDenied**: Антипаттерн недекларируемых возможностей
