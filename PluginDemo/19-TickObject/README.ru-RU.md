# PluginDemo-19: TickObject — Периодическая задача в MainLoop

## Обзор

Этот плагин демонстрирует использование `TickObject` для интеграции с `MainLoop` для периодической/непрерывной логики. TickObject — это базовый класс для объектов, которые могут быть тикнуты основным циклом MainLoop, предоставляя унифицированную альтернативу `System.Threading.Timer` или `Task.Delay`.

## Жизненный цикл TickObject

```
Constructor(interval, autoRegister)
    │
    ├── autoRegister=true → MainLoop.Register(this) вызывается автоматически в конструкторе
    │
    ├── autoRegister=false → вызвать MainLoop.Register(this) вручную позже
    │
    ▼
MainLoop.Tick() цикл
    │
    ├── Сортировать все зарегистрированные TickObject по Priority (по возрастанию)
    ├── Накапливать elapsedTime для каждого TickObject
    ├── Если elapsedTime >= Interval → вызвать OnTick(deltaTime)
    │
    ├── Автоматический выключатель: если OnTick превышает TickTimeout → увеличить счётчик таймаутов
    │   └── После maxTimeoutCount последовательных таймаутов → 1-минутное охлаждение
    │
    ▼
MainLoop.Unregister(tickObject) — очистка в OnStop
```

## Ключевые свойства

| Свойство | Тип | По умолчанию | Описание |
|---------|-----|-------------|----------|
| `Interval` | `TimeSpan` | Обязательное | Как часто вызывается OnTick |
| `Priority` | `int` | 100 | Порядок выполнения (ниже = выше приоритет) |
| `autoRegister` | `bool` | `true` | Автоматическая регистрация в MainLoop в конструкторе |

## Ключевые методы

| Метод | Описание |
|-------|----------|
| `OnTick(TimeSpan deltaTime)` | Переопределить для реализации периодической логики |
| `MainLoop.Register(TickObject)` | Ручная регистрация в MainLoop |
| `MainLoop.Unregister(TickObject)` | Удаление из MainLoop (очистка) |

## Демонстрационные сценарии

### 1. Базовый таймер (autoRegister=true)
```csharp
public class StatusTimer : TickObject
{
    public StatusTimer() : base(interval: TimeSpan.FromSeconds(5), autoRegister: true)
    {
        Priority = 100;
    }

    protected override void OnTick(TimeSpan deltaTime)
    {
        Console.WriteLine($"Tick, deltaTime={deltaTime.TotalMilliseconds:F0}ms");
    }
}
```

### 2. Ручная регистрация (autoRegister=false)
```csharp
// В конструкторе: не регистрировать автоматически
_heartbeatTimer = new HeartbeatTimer(autoRegister: false);

// В OnStart: зарегистрировать вручную
MainLoop.Register(_heartbeatTimer);
```

### 3. Порядок приоритета
- `Priority = 10` → Высокий приоритет, выполняется первым
- `Priority = 200` → Низкий приоритет, выполняется после

### 4. Очистка
```csharp
// В OnStop: всегда отменять регистрацию для предотвращения утечек
MainLoop.Unregister(_statusTimer);
```

## Автоматический выключатель MainLoop

MainLoop имеет встроенный автоматический выключатель для предотвращения блокировки всего цикла медленными TickObject:

1. Если `OnTick` превышает `TickTimeout` (по умолчанию 1 секунда) → счётчик таймаутов увеличивается
2. После `maxTimeoutCount` (по умолчанию 3) последовательных таймаутов → автоматический выключатель срабатывает
3. Сработавший TickObject **пропускается** на 1 минуту охлаждения
4. После охлаждения TickObject получает ещё один шанс

## TickObject vs System.Threading.Timer

| Аспект | TickObject + MainLoop | System.Threading.Timer |
|--------|----------------------|----------------------|
| Потоковая модель | Один поток основного цикла | Потоки из пула потоков |
| Порядок выполнения | Детерминированный (по Priority) | Недетерминированный |
| Автоматический выключатель | Встроенный | Нет |
| Отладка | Простая (один поток) | Сложная (состояния гонки) |
| Использование ресурсов | Минимальное (без пула потоков) | Накладные расходы пула потоков |
| Точность интервала | Best-effort (зависит от других TickObject) | Более точный |

## Примечание по безопасности

TickObject сам по себе **не требует** декларации возможности. Это безопасный встроенный механизм фреймворка.

## Файлы

- `Plugin.cs` — Демо-плагин TickObject
- `README.md` — Этот файл (Английский)
- `README.zh-CN.md` — Упрощённый китайский
- Переводы: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Связанные примеры

- **13-CapabilityNetwork**: Декларация Capability.Network
- **20-SpeedyPack**: Хранилище данных без Capability.FileIO
