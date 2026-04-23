# Silicon Life Collective

**⚠️ ADVERTENCIA: La compilación dinámica funciona pero requiere plantillas de código para funcionar correctamente. Las pruebas exhaustivas están en curso.**

Una plataforma de colaboración multiagente en .NET 9 donde los agentes de IA llamados **Seres de Silicio** se autoevolucionan mediante compilación dinámica Roslyn.

[English](../en/README.md) | [中文文档](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Español](../es-ES/README.md)

## Características

- **Orquestación Multiagente** — Gestionada por un *Curador de Silicio* con programación justa por segmentos de tiempo basada en ticks (MainLoop + TickObject + Watchdog + Circuit Breaker)
- **Impulsado por Archivo de Alma** — Cada Ser de Silicio es impulsado por un archivo de prompt principal (`soul.md`) que define su personalidad y comportamiento
- **Arquitectura Cuerpo-Cerebro** — El *Cuerpo* (SiliconBeing) se mantiene vivo y detecta desencadenadores; el *Cerebro* (ContextManager) carga historial, llama a IA, ejecuta herramientas y persiste respuestas
- **Bucle de Llamada a Herramientas** — La IA devuelve tool_calls → ejecutar herramientas → devolver resultados → la IA continúa → hasta respuesta de texto plano
- **Seguridad Ejecutor-Permiso** — Todas las operaciones de disco, red y línea de comandos pasan por ejecutores con verificación de permisos
  - Cadena de permisos de 5 niveles: IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - Registro de auditoría para todas las decisiones de permisos
- **Auditoría de Uso de Tokens** — Seguimiento y reporte integrado de uso de tokens mediante `ITokenUsageAudit` / `TokenUsageAuditManager`
- **Múltiples Backends de IA** — Soporte para Ollama (local) y Alibaba Cloud DashScope (nube)
  - **Ollama** — Alojamiento local de modelos con API HTTP nativa
  - **DashScope (百炼)** — Servicio de IA en la nube con API compatible con OpenAI, despliegue multirregional y 13+ modelos (Qwen, DeepSeek, GLM, Kimi, Llama)
- **32 Sistemas de Calendario** — Soporte multicalendario incluyendo Gregoriano, Lunar Chino, Islámico, Hebreo, Japonés, Persa, Maya y más
- **Dependencias Mínimas** — La biblioteca principal solo depende de Microsoft.CodeAnalysis.CSharp para compilación dinámica Roslyn
- **Cero Dependencia de Base de Datos** — Almacenamiento basado en archivos (JSON) con consultas indexadas por tiempo mediante `ITimeStorage`
- **Localización** — Soporte multilingüe integral con 20 variantes de idioma
  - Chino: zh-CN, zh-HK, zh-SG, zh-MO, zh-TW, zhMY (6 variantes)
  - Inglés: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 variantes)
  - Japonés: ja-JP
  - Coreano: ko-KR
  - Español: es-ES, es-MX (2 variantes)
- **Interfaz Web** — Servidor HTTP integrado con soporte SSE, múltiples pieles y panel integral
  - **Sistema de Pieles** — 4 pieles integradas (Admin, Chat, Creative, Dev) con interfaz ISkin enchufable y autodescubrimiento
  - **20+ Controladores** — About, Audit, Being, Chat, ChatHistory, CodeBrowser, CodeHover, Config, Dashboard, Executor, Init, Knowledge, Log, Memory, Permission, PermissionRequest, Project, Task, Timer
  - **Actualizaciones en Tiempo Real** — SSE (Server-Sent Events) para mensajes de chat, estado de seres y eventos del sistema
  - **Constructores HTML/CSS/JS** — Generación de marcado del lado del servidor mediante `H`, `CssBuilder` y `JsBuilder` (cero dependencia de framework frontend)
  - **Localización** — Veinte variantes de idioma integradas con resolución de LocalizationManager
  - **Vista de Historial de Chat** — Navegación completa del historial de chat de Seres de Silicio con lista de conversaciones y detalles de mensajes
  - **Soporte de Carga de Archivos** — Diálogo de origen de archivos y funcionalidad de carga de archivos
  - **Indicadores de Carga** — Indicadores de estado de carga para páginas de chat y selección automática de sesión del curador

## Stack Tecnológico

| Componente | Tecnología |
|-----------|-----------|
| Runtime | .NET 9 |
| Lenguaje | C# |
| Integración de IA | Ollama (local), Alibaba Cloud DashScope (nube) |
| Almacenamiento | Sistema de archivos (JSON + directorios indexados por tiempo) |
| Servidor Web | HttpListener (integrado en .NET) |
| Compilación Dinámica | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Licencia | Apache-2.0 |

## Estructura del Proyecto

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Biblioteca principal (interfaces, abstracciones)
│   │   ├── ServiceLocator.cs             # Localizador de servicios global: Register/Get, ChatSystem, IMManager, AuditLogger, GlobalACL, BeingFactory, BeingManager, DynamicBeingLoader, TokenUsageAudit
│   │   ├── Runtime/                       # MainLoop, TickObject, CoreHost, CoreHostBuilder, PerformanceMonitor
│   │   ├── SiliconBeing/                  # SiliconBeingBase, SiliconBeingManager, SiliconCurator, ISiliconBeingFactory, SoulFileManager, Memory, TaskSystem, TimerSystem
│   │   ├── AI/                            # IAIClient, IAIClientFactory, ContextManager ("cerebro"), Message, AIRequest/AIResponse
│   │   ├── Audit/                         # ITokenUsageAudit, TokenUsageAuditManager, TokenUsageRecord, TokenUsageSummary, TokenUsageQuery
│   │   ├── Chat/                          # ChatSystem, IChatService, SimpleChatService, SessionBase, SingleChatSession, GroupChatSession, BroadcastChannel, ChatMessage
│   │   ├── Executors/                     # ExecutorBase, DiskExecutor, NetworkExecutor, CommandLineExecutor, ExecutorRequest, ExecutorResult
│   │   ├── Tools/                         # ITool, ToolManager (escaneo por reflexión), ToolCall/ToolResult, ToolDefinition, SiliconManagerOnlyAttribute
│   │   ├── Security/                      # PermissionManager, GlobalACL, AuditLogger, UserFrequencyCache, PermissionResult, PermissionType, IPermissionCallback, IPermissionAskHandler
│   │   ├── IM/                            # IIMProvider, IMManager (enrutamiento de mensajes)
│   │   ├── Storage/                       # IStorage, ITimeStorage (clave-valor + indexado por tiempo)
│   │   ├── Config/                        # ConfigDataBase, Config (singleton + JSON), ConfigDataBaseConverter, GuidConverter, AIClientConfigAttribute, ConfigGroupAttribute, ConfigIgnoreAttribute, DirectoryInfoConverter
│   │   ├── Localization/                  # LocalizationBase, LocalizationManager, enum Language
│   │   ├── Logging/                       # ILogger, ILoggerProvider, LogEntry, LogLevel, LogManager
│   │   ├── Compilation/                   # DynamicBeingLoader, DynamicCompilationExecutor, SecurityScanner, CodeEncryption
│   │   └── Time/                          # IncompleteDate (consultas por rango de tiempo)
│   │
│   └── SiliconLife.Default/               # Implementaciones predeterminadas + punto de entrada
│       ├── Program.cs                     # Entrada de aplicación (conexión de todos los componentes)
│       ├── AI/                            # OllamaClient, OllamaClientFactory (API HTTP nativa de Ollama); DashScopeClient, DashScopeClientFactory (Alibaba Cloud Bailian)
│       ├── SiliconBeing/                  # DefaultSiliconBeing, DefaultSiliconBeingFactory
│       ├── Calendar/                      # 32 implementaciones de calendario: Buddhist, Cherokee, ChineseLunar, ChulaSakarat, Coptic, Dai, DehongDai, Ethiopian, FrenchRepublican, Gregorian, Hebrew, Indian, Inuit, Islamic, Japanese, Javanese, Juche, Julian, Khmer, Mayan, Mongolian, Persian, RepublicOfChina, Roman, Saka, Sexagenary, Tibetan, Vietnamese, VikramSamvat, Yi, Zoroastrian
│       ├── Executors/                     # Implementaciones predeterminadas de ejecutores
│       ├── IM/                            # WebUIProvider (Interfaz web como canal IM), IMPermissionAskHandler
│       ├── Tools/                         # Herramientas integradas: Calendar, Chat, Config, Curator, Disk, DynamicCompile, Memory, Network, System, Task, Timer, TokenAudit
│       ├── Config/                        # DefaultConfigData
│       ├── Localization/                  # ZhCN, ZhHK, EnUS, JaJP, KoKR, EsES, EsMX, DefaultLocalizationBase, EnOther (EnGB, EnCA, EnAU, EnIN, EnSG, EnZA, EnIE, EnNZ, EnMY), ZhOther (ZhSG, ZhMO, ZhTW, ZhMY), EsOther (EsMX)
│       ├── Logging/                       # ConsoleLoggerProvider, FileSystemLoggerProvider
│       ├── Storage/                       # FileSystemStorage, FileSystemTimeStorage
│       ├── Security/                      # DefaultPermissionCallback
│       ├── Runtime/                       # TestTickObject
│       └── Web/                           # Implementación de interfaz web
│           ├── Controllers/               # 20+ controladores: About, Audit, Being, Chat, ChatHistory, CodeBrowser, CodeHover, Config, Dashboard, Executor, Init, Knowledge, Log, Memory, Permission, PermissionRequest, Project, Task, Timer
│           ├── Models/                    # ViewModels: AboutViewModel, AuditViewModel, BeingViewModel, ChatMessage, ChatViewModel, CodeBrowserViewModel, ConfigViewModel, DashboardViewModel, ExecutorViewModel, KnowledgeViewModel, LogViewModel, MemoryViewModel, PermissionViewModel, PermissionRequestViewModel, ProjectViewModel, TaskViewModel, TimerViewModel, ViewModelBase
│           ├── Views/                     # 19 vistas HTML: ViewBase, AboutView, AuditView, BeingView, ChatView, CodeBrowserView, CodeEditorView, ConfigView, DashboardView, ExecutorView, KnowledgeView, LogView, MarkdownEditorView, MemoryView, PermissionView, ProjectView, SoulEditorView, TaskView, TimerView
│           ├── Skins/                     # 4 pieles: Admin (profesional), Chat (conversacional), Creative (artística), Dev (enfocada en desarrolladores)
│           ├── ISkin.cs                   # Interfaz Skin + SkinPreviewInfo + SkinManager (autodescubrimiento)
│           ├── Controller.cs              # Clase base de controlador
│           ├── WebHost.cs                 # Servidor HTTP (HttpListener)
│           ├── Router.cs                  # Enrutamiento de solicitudes con coincidencia de patrones
│           ├── SSEHandler.cs              # Server-Sent Events
│           ├── WebSecurity.cs             # Utilidades de seguridad web
│           ├── H.cs                       # DSL de constructor HTML fluido
│           ├── CssBuilder.cs              # Utilidad de constructor CSS
│           └── JsBuilder.cs               # Utilidad de constructor JavaScript
│
├── docs/
│   └── es-ES/                             # Documentación en español
```

## Resumen de Arquitectura

```
MainLoop (hilo dedicado, watchdog + circuit breaker)
  └── TickObject (ordenado por prioridad)
       └── SiliconBeingManager
            └── SiliconBeingRunner (hilo temporal por tick, timeout + circuit breaker)
                 └── DefaultSiliconBeing.Tick()
                      └── ContextManager.ThinkOnChat()
                           └── IAIClient.Chat() -> Bucle de Llamada a Herramientas -> Persistir en ChatSystem
```

Todas las operaciones de I/O iniciadas por IA pasan por la cadena de seguridad:

```
Tool Call -> Executor -> PermissionManager -> [IsCurator -> FrequencyCache -> GlobalACL -> Callback -> AskUser]
```

## Primeros Pasos

### Requisitos Previos

- .NET 9 SDK
- Backend de IA (elegir uno):
  - **Ollama**: [Ollama](https://ollama.com) ejecutándose localmente con un modelo descargado (ej., `ollama pull llama3`)
  - **Alibaba Cloud DashScope**: Clave API válida desde [Bailian Console](https://bailian.console.aliyun.com/)

### Compilar

```bash
dotnet restore
dotnet build
```

### Ejecutar

```bash
dotnet run --project src/SiliconLife.Default
```

La aplicación iniciará un servidor web y abrirá automáticamente la interfaz web en tu navegador.

### Publicar (archivo único)

```bash
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

## Hoja de Ruta

- [x] Fase 1: Chat de IA en consola
- [x] Fase 2: Esqueleto de framework (MainLoop + TickObject + Watchdog + Circuit Breaker)
- [x] Fase 3: Primer Ser de Silicio con archivo de alma (arquitectura Cuerpo-Cerebro)
- [x] Fase 4: Memoria persistente (ChatSystem + ITimeStorage)
- [x] Fase 5: Sistema de herramientas + Ejecutores
- [x] Fase 6: Sistema de permisos (cadena de 5 niveles, AuditLogger, GlobalACL)
- [x] Fase 7: Compilación dinámica + autoevolución (Roslyn)
- [x] Fase 8: Memoria a largo plazo + Tareas + Temporizador
- [x] Fase 9: CoreHost + colaboración multiagente
- [x] Fase 10: Interfaz web (HTTP + SSE, 18 controladores, 4 pieles)
- [x] Fase 10.5: Mejoras incrementales (BroadcastChannel, TokenAudit, 32 calendarios, mejoras de herramientas, localización en 20 idiomas)
- [ ] Fase 11: Integración con IM externos (Feishu / WhatsApp / Telegram)
- [ ] Fase 12: Gráfico de conocimiento, sistema de plugins y ecosistema de habilidades

## Documentación

- [Arquitectura](architecture.md) — Diseño del sistema, programación, arquitectura de componentes
- [Seguridad](security.md) — Modelo de permisos, ejecutores, seguridad de compilación dinámica
- [Hoja de Ruta](roadmap.md) — Plan de desarrollo detallado de 12 fases

## Licencia

Este proyecto está licenciado bajo la Licencia Apache 2.0 — consulte el archivo [LICENSE](../../LICENSE) para más detalles.

## Autor

Hoshino Kennji — [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective) | [Gitee](https://gitee.com/hoshinokennji/SiliconLifeCollective) | [YouTube](https://www.youtube.com/@hoshinokennji) | [Bilibili](https://space.bilibili.com/617827040)
