# PluginDemo-13: Capability.Network — Декларативное сетевое разрешение

## Обзор

Этот плагин демонстрирует использование `[PluginCapability(Capability.Network)]` для объявления необходимости сетевого доступа. С этой декларацией плагин получает доступ к типам `System.Net.*`, которые иначе были бы заблокированы сканированием безопасности PluginLoader.

## Синтаксис декларации

```csharp
[PluginCapability(Capability.Network, Reason = "Calls weather REST API to retrieve forecast data")]
public class CapabilityNetworkPlugin : IPlugin { ... }
```

**Ключевые элементы:**
- **Цель атрибута**: Должен быть на классе, который непосредственно реализует `IPlugin`
- **AllowMultiple = true**: Можно накладывать несколько атрибутов `[PluginCapability]` (см. 17-CapabilityStacked)
- **Поле Reason**: Читаемое человеком объяснение, записываемое в журнал аудита безопасности при загрузке. **Настоятельно рекомендуется указывать ясный Reason для всех производственных плагинов.**

## Как PluginLoader обрабатывает декларации возможностей

1. **Чтение метаданных PE**: PluginLoader читает декларации возможностей из таблицы CustomAttribute PE-файла **до** начала сканирования безопасности
2. **Смягчение правил сканирования**: Декларированные возможности освобождают соответствующие ссылки на типы от проверок запрещённых пространств имён и запрещённых типов
3. **Журналирование аудита**: Все декларации (включая Reason) записываются в журнал аудита безопасности
4. **Недекларируемые возможности**: P/Invoke, Unsafe, Reflection.Emit и т.д. остаются заблокированными независимо от любых деклараций

## Область исключения Capability.Network

### Исключения TypeRef

Когда декларирована `Capability.Network`, следующие основанные на пространствах имён и типах запреты смягчаются:

| Исключённое пространство имён | Разрешённые типы |
|------------------------------|-----------------|
| `System.Net.Http` | `HttpClient`, `HttpRequestMessage`, `HttpResponseMessage` и т.д. |
| `System.Net.WebSockets` | `ClientWebSocket`, `WebSocket` и т.д. |
| `System.Net.Sockets` | `TcpClient`, `UdpClient`, `Socket` и т.д. |
| `System.Net.Mail` | `SmtpClient`, `MailMessage` и т.д. |
| `System.Net.NetworkInformation` | `Ping`, `NetworkInterface` и т.д. |
| `System.Net.Security` | `SslStream` и т.д. |
| `System.Net` (запреты по типам) | `HttpWebRequest`, `WebClient`, `Dns`, `FtpWebRequest` и т.д. |

### Исключения ILString

Строковые константы, начинающиеся с этих префиксов, не помечаются при сканировании кучи #US:
- `"System.Net.Http"`
- `"System.Net.WebSockets"`
- `"System.Net.Sockets"`
- `"System.Net.Mail"`
- `"System.Net.NetworkInformation"`
- `"System.Net.Security"`

### Что остаётся запрещённым

Даже с `Capability.Network` эти возможности **всегда** заблокированы (недекларируемые возможности):

| Категория | Заблокированные типы | Почему недекларируемые |
|----------|---------------------|----------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` | Невозможно безопасно аудировать во время выполнения |
| Unsafe-код | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | Обходит гарантии безопасности типов |
| IL-генерация | `System.Reflection.Emit.*` | Может генерировать произвольный код во время выполнения |
| Загрузка сборок | `System.Runtime.Loader`, `Assembly.Load*` | Может обойти сканирование безопасности, загружая непроверенные DLL |
| Реестр | `Microsoft.Win32.*` | Доступ к системе на уровне ОС за пределами песочницы плагина |

## Поле Reason — Роль аудита

Поле `Reason` служит **аудиторским следом** для деклараций возможностей:

```
Security audit: [CapabilityNetworkPlugin] com.siliconlife.demo.capabilitynetwork declared Capability.Network — reason: Calls weather REST API to retrieve forecast data
```

**Почему Reason важен:**
1. **Проверка безопасности**: Аудиторы могут проверить, что декларированные возможности соответствуют фактическому поведению плагина
2. **Принцип наименьших привилегий**: Заставляет авторов плагинов обосновывать необходимость каждой возможности
3. **Соответствие требованиям**: Необходимо для сертификаций безопасности и расследований инцидентов
4. **Мониторинг во время выполнения**: Инструменты безопасности могут предупреждать, если использование декларированной возможности превышает указанную причину

## Сравнение с 08-ForbiddenNetwork

| Аспект | 08-ForbiddenNetwork | 13-CapabilityNetwork |
|--------|-------------------|---------------------|
| Декларация | Нет | `[PluginCapability(Capability.Network)]` |
| Результат загрузки | ❌ Отклонён | ✅ Успешно загружен |
| Использование HttpClient | Заблокировано сканированием TypeRef | Исключено по декларации |
| Использование TcpClient | Заблокировано сканированием TypeRef | Исключено по декларации |
| Reason | Неприменимо | Записано в журнал аудита |

**Ключевое различие**: 08-ForbiddenNetwork показывает, что происходит при использовании сетевых типов **без** декларации возможности. 13-CapabilityNetwork показывает **правильный** способ декларативно запросить сетевой доступ.

## Рекомендации по безопасности

1. **Декларировать только необходимое**: Если вам нужен только HTTP, не декларируйте Capability.Network только потому что это возможно — но учтите, что Capability.Network — единственная сетевая возможность; нет более детализированных опций
2. **Предпочитать NetworkExecutor**: `NetworkExecutor` — контролируемая точка входа для сетевого доступа, не требующая декларации возможности
3. **Указывать ясный Reason**: Расплывчатые причины вроде «сетевой доступ» — тревожный сигнал при проверке безопасности
4. **Учитывать недекларируемые ограничения**: Никакая декларация возможности не может обойти запреты P/Invoke, Unsafe или Reflection.Emit

## Файлы

- `Plugin.cs` — Демо-плагин, декларирующий Capability.Network
- `README.md` — English
- `README.zh-CN.md` — 简体中文
- `README.zh-HK.md` — 繁體中文
- `README.ja-JP.md` — 日本語
- `README.ko-KR.md` — 한국어
- `README.de-DE.md` — Deutsch
- `README.fr-FR.md` — Français
- `README.es-ES.md` — Español
- `README.it-IT.md` — Italiano
- `README.ru-RU.md` — Этот файл (Русский)
- `README.pt-PT.md` — Português
- `README.pl-PL.md` — Polski
- `README.cs-CZ.md` — Čeština

## Связанные примеры

- **08-ForbiddenNetwork**: Антипаттерн заблокированных сетевых операций
- **14-CapabilityFileIO**: Декларативная возможность FileIO
- **15-CapabilityProcess**: Декларативная возможность Process
- **16-CapabilityAI**: Декларативная возможность сервиса ИИ
- **17-CapabilityStacked**: Несколько наложенных возможностей
- **18-CapabilityDenied**: Антипаттерн недекларируемой возможности
