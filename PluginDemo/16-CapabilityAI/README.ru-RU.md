# PluginDemo-16: Capability.AI — Декларативное разрешение сервиса ИИ

## Обзор

Этот плагин демонстрирует использование `[PluginCapability(Capability.AI)]` для объявления, что плагину нужен доступ к сервису ИИ. В отличие от других возможностей, `Capability.AI` **не** освобождает ни одного запрещённого пространства имён — вместо этого она позволяет хосту внедрить ссылку `IAIService` в плагин.

## Ключевая концепция: Capability.AI не предоставляет сетевой доступ

`Capability.AI` принципиально отличается от других возможностей:

| Возможность | Что освобождает | Как работает |
|-----------|---------------|-------------|
| `Capability.Network` | Пространства имён `System.Net.*` | Ослабляет правила сканирования TypeRef/ILString |
| `Capability.FileIO` | Пространство имён `System.IO` | Ослабляет правила сканирования TypeRef/ILString |
| `Capability.Process` | Типы `Process*` | Ослабляет правила сканирования TypeRef/ILString |
| `Capability.AI` | **Ничего** | Включает внедрение IAIService хостом |

`IAIService` находится в пространстве имён `SiliconLife.Collective` — он никогда не входит в списки запретов. Декларация возможности — это **сигнал opt-in** хосту о том, что этот плагин должен получить ссылку на сервис ИИ.

## Стек возможностей: ИИ + Сеть

Если вашему ИИ-клиенту нужен прямой сетевой доступ (например, вызов удалённого ИИ-эндпоинта), вы должны объявить **обе** возможности:

```csharp
[PluginCapability(Capability.Network, Reason = "Calls remote AI endpoint")]
[PluginCapability(Capability.AI, Reason = "Provides IAIService to downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

См. **17-CapabilityStacked** для полных примеров стека.

## Модель контролируемой точки входа

| Ресурс | Контролируемая точка входа | Необходимая возможность |
|--------|--------------------------|----------------------|
| Файлы | `PermissionedStreamFactory` | Нет |
| Сеть | `NetworkExecutor` | Нет |
| Процессы | `CommandLineExecutor` | Нет |
| Хранилище данных | `SpeedyPack` | Нет |
| Сервис ИИ | `IAIService` | `Capability.AI` |

`IAIService` уникален: он **требует** декларации возможности. Доступ к сервису ИИ — это функция opt-in, а не возможность по умолчанию, доступная всем плагинам.

## Файлы

- `Plugin.cs` — Демо-плагин с декларацией Capability.AI
- `README.md` — Этот файл (Английский)
- `README.zh-CN.md` — Упрощённый китайский
- Переводы: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Связанные примеры

- **17-CapabilityStacked**: Стек из нескольких возможностей (Сеть + ИИ)
- **18-CapabilityDenied**: Антипаттерн недекларируемых возможностей
