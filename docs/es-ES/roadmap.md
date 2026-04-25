# Hoja de Ruta

[English](roadmap.md) | [简体中文](docs/zh-CN/roadmap.md) | [繁體中文](docs/zh-HK/roadmap.md) | [Español](docs/es-ES/roadmap.md) | [日本語](docs/ja-JP/roadmap.md) | [한국어](docs/ko-KR/roadmap.md) | [Čeština](docs/cs-CZ/roadmap.md)

[English](../en/roadmap.md) | [中文](../zh-CN/roadmap.md) | [繁體中文](../zh-HK/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Español](../es-ES/roadmap.md)

## Principio Rector

Cada fase termina con un sistema **ejecutable y observable**. Ninguna fase produce "un montón de infraestructura sin nada que mostrar."

---

## ~~Fase 1: Puede Chatear~~ ✅ Completado

**Objetivo**: Entrada de consola → Llamada a IA → Salida de consola. Unidad mínima verificable.

| # | Módulo | Descripción |
|---|--------|-------------|
| 1.1 | Estructura de solución y proyectos | Crear `SiliconLifeCollective.sln` con `src/SiliconLife.Core/` (biblioteca principal) y `src/SiliconLife.Default/` (implementación predeterminada + punto de entrada) |
| 1.2 | Config (mínimo) | Singleton + deserialización JSON. Leer `config.json`. Generar predeterminados automáticamente si falta |
| 1.3 | Localización (mínimo) | Clase abstracta `LocalizationBase`, implementación `ZhCN`. Agregar `Language` a la configuración |
| 1.4 | OllamaClient (mínimo) | Interfaz `IAIClient`, llamada HTTP a Ollama local `/api/chat`. Sin streaming, sin ToolCall aún |
| 1.5 | E/S de consola | `while(true) + Console.ReadLine()`, leer entrada → llamar a IA → imprimir respuesta |
| 1.6 | Encabezados de copyright | Agregar encabezados Apache 2.0 a todos los archivos fuente C# |

**Entregable**: Un programa de chat de consola que habla con un modelo Ollama local.

**Verificación**: Ejecutar el programa, escribir "hola", ver una respuesta de IA.

---

## ~~Fase 2: Tiene Esqueleto~~ ✅ Completado

**Objetivo**: Reemplazar el "bucle desnudo" con una estructura de framework. Comportamiento sin cambios.

| # | Módulo | Descripción |
|---|--------|-------------|
| 2.1 | Almacenamiento (mínimo) | Interfaz `IStorage` (Read/Write/Exists/Delete, clave-valor). Implementación `FileSystemStorage`. Clase de instancia (no estática). Acceso directo al sistema de archivos — **la IA no puede controlar IStorage** |
| 2.2 | MainLoop + TickObject | Bucle infinito con intervalos de tick precisos (`Stopwatch` + `Thread.Sleep`). Programación por prioridad |
| 2.3 | Estandarización de IAIClient | Interfaz `IAIClientFactory`. OllamaClient refactorizado para implementar interfaz estándar |
| 2.4 | Migración de consola | Migrar `while(true)` a TickObject impulsado por MainLoop. Comportamiento idéntico a Fase 1 |

**Entregable**: MainLoop ejecutando ticks, chat de consola aún funciona.

**Verificación**: Registrar un TickObject de prueba que imprima conteo de ticks cada segundo; chat de consola aún funciona.

---

## ~~Fase 3: Tiene un Alma~~ ✅ Completado

**Objetivo**: El primer Ser de Silicio está vivo dentro del framework.

| # | Módulo | Descripción |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Clase base abstracta con Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService. `Tick()` y `ExecuteOneRound()` abstractos |
| 3.2 | Carga de archivo de alma | `SoulFileManager`: leer `soul.md` desde directorio de datos del ser |
| 3.3 | ContextManager (mínimo) | Concatenar archivo de alma + mensajes recientes → llamar a IA → obtener respuesta. Sin ToolCall, sin persistencia aún |
| 3.4 | ISiliconBeingFactory | Interfaz de fábrica para crear instancias de seres |
| 3.5 | SiliconBeingManager (mínimo) | Hereda TickObject (Prioridad=0). Itera todos los seres, llama sus Tick en secuencia |
| 3.6 | DefaultSiliconBeing | Implementación de comportamiento estándar. Verificar mensajes no leídos → crear ContextManager → ExecuteOneRound → salida |
| 3.7 | Estructura de directorios de seres | `DataDirectory/SiliconManager/{GUID}/` con `soul.md` y `state.json` |

**Entregable**: Ser de Silicio impulsado por MainLoop, recibiendo entrada de consola, cargando archivo de alma, llamando a IA.

**Verificación**: Entrada de consola → Tick de MainLoop → Ser procesa (con comportamiento guiado por archivo de alma) → respuesta de IA. El estilo de respuesta debería diferir de la Fase 1.

---

## ~~Fase 4: Tiene Memoria~~ ✅ Completado

**Objetivo**: Las conversaciones persisten entre reinicios.

| # | Módulo | Descripción |
|---|--------|-------------|
| 4.1 | ChatSystem | Concepto de canal (dos GUID = un canal). Modelo de mensaje con persistencia. Sin chat grupal aún |
| 4.2 | IIMProvider + IMManager | Interfaz `IIMProvider`. `ConsoleProvider` como canal IM formal. `IMManager` enruta mensajes |
| 4.3 | ContextManager mejorado | Extraer historial de ChatSystem. Persistir respuestas de IA. Soporte para continuación ToolCall de múltiples rondas |
| 4.4 | Modelo IMessage | Modelo de mensaje unificado compartido por ChatSystem e IMManager |

**Entregable**: Un sistema de chat con memoria persistente.

**Verificación**: Chatear unas vueltas → salir → reiniciar → preguntar "¿de qué hablamos?" → el ser puede responder.

---

## ~~Fase 5: Puede Actuar (Sistema de Herramientas)~~ ✅ Completado

**Objetivo**: Los Seres de Silicio pueden ejecutar operaciones, no solo hablar.

| # | Módulo | Descripción |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | Interfaz `ITool` con Name, Description, Execute. `ToolResult` con Success, Message, Data |
| 5.2 | ToolManager | Instancia por ser. Descubrimiento de herramientas basado en reflexión. Soporte para atributo `[SiliconManagerOnly]` |
| 5.3 | IAIClient: Soporte ToolCall | Analizar tool_calls de IA. Bucle: ejecutar herramienta → enviar resultado de vuelta → IA continúa → hasta texto plano |
| 5.4 | Clase base de ejecutor | Clase base abstracta con hilo de programación independiente, cola de solicitudes, control de timeout |
| 5.5 | NetworkExecutor | Solicitudes HTTP a través de ejecutor. Timeout, cola |
| 5.6 | CommandLineExecutor | Ejecución de shell a través de ejecutor. Detección de separador multiplataforma |
| 5.7 | DiskExecutor | Operaciones de archivos a través de ejecutor. Sin verificación de permisos aún (Fase 6) |
| 5.8–5.12 | Herramientas integradas | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**Entregable**: Los Seres de Silicio pueden llamar herramientas para realizar operaciones.

---

## ~~Fase 6: Sistema de Permisos~~ ✅ Completado

**Objetivo**: Control granular sobre qué pueden hacer los seres.

| # | Módulo | Descripción |
|---|--------|-------------|
| 6.1 | PermissionManager | Gestor de permisos privado por ser |
| 6.2 | 5 niveles de permisos | IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler |
| 6.3 | AuditLogger | Registro de todas las decisiones de permisos |
| 6.4 | GlobalACL | Lista de control de acceso global |
| 6.5 | UserFrequencyCache | Caché de frecuencia de usuario para prevenir abuso |
| 6.6 | Integración con ejecutores | Todos los ejecutores verifican permisos antes de ejecutar |

**Entregable**: Sistema de permisos de 5 niveles con auditoría completa.

**Verificación**: El ser intenta ejecutar una operación restringida → se solicita permiso al usuario → el usuario aprueba/rechaza → se registra la decisión.

---

## ~~Fase 7: Compilación Dinámica + Autoevolución~~ ✅ Completado

**Objetivo**: Los Seres de Silicio pueden autoevolucionarse mediante generación de código.

| # | Módulo | Descripción |
|---|--------|-------------|
| 7.1 | DynamicCompilationExecutor | Compilación Roslyn de código C# generado por IA |
| 7.2 | SecurityScanner | Escaneo de seguridad antes de compilar |
| 7.3 | DynamicBeingLoader | Carga dinámica de assemblies compilados |
| 7.4 | CodeEncryption | Encriptación de código compilado sensible |
| 7.5 | Integración con herramientas | Herramienta DynamicCompile para seres |

**Entregable**: Seres capaces de generar, compilar y ejecutar código dinámicamente.

**Verificación**: El ser genera código → pasa escaneo de seguridad → se compila → se ejecuta → resultados retornados.

---

## ~~Fase 8: Memoria a Largo Plazo + Tareas + Temporizadores~~ ✅ Completado

**Objetivo**: Capacidades avanzadas de memoria y planificación.

| # | Módulo | Descripción |
|---|--------|-------------|
| 8.1 | ITimeStorage | Almacenamiento indexado por tiempo para consultas de rango temporal |
| 8.2 | FileSystemTimeStorage | Implementación basada en archivos con estructura de directorios temporal |
| 8.3 | TaskSystem | Sistema de tareas para seres (crear, actualizar, completar) |
| 8.4 | TimerSystem | Sistema de temporizadores para acciones futuras |
| 8.5 | Integración con ChatSystem | Persistencia de mensajes indexada por tiempo |

**Entregable**: Memoria persistente con capacidades de tareas y temporizadores.

---

## ~~Fase 9: CoreHost + Colaboración Multiagente~~ ✅ Completado

**Objetivo**: Múltiples seres trabajando juntos.

| # | Módulo | Descripción |
|---|--------|-------------|
| 9.1 | CoreHost | Host principal que gestiona todos los componentes |
| 9.2 | CoreHostBuilder | Builder para configurar CoreHost |
| 9.3 | BroadcastChannel | Canales de difusión para comunicación entre seres |
| 9.4 | SiliconCurator | Ser especial con privilegios máximos para gestión |
| 9.5 | TokenUsageAudit | Auditoría de uso de tokens de IA |

**Entregable**: Sistema multiagente completo con curador.

---

## ~~Fase 10: Interfaz Web~~ ✅ Completado

**Objetivo**: Interfaz web completa con funcionalidad total.

| # | Módulo | Descripción |
|---|--------|-------------|
| 10.1 | WebHost | Servidor HTTP usando HttpListener |
| 10.2 | Router | Enrutamiento de solicitudes con coincidencia de patrones |
| 10.3 | Controllers | 18 controladores para diferentes funcionalidades |
| 10.4 | Views | 19 vistas HTML con constructores fluentes |
| 10.5 | Skins | 4 pieles intercambiables (Admin, Chat, Creative, Dev) |
| 10.6 | SSE | Server-Sent Events para actualizaciones en tiempo real |
| 10.7 | H, CssBuilder, JsBuilder | Constructores de marcado del lado del servidor |

**Entregable**: Interfaz web completa con múltiples pieles y actualizaciones en tiempo real.

---

## ~~Fase 10.5: Mejoras Incrementales~~ ✅ Completado

**Objetivo**: Pulir y expandir capacidades existentes.

- 32 sistemas de calendario
- Localización en 20 idiomas
- Mejoras de herramientas existentes
- Optimizaciones de rendimiento
- Documentación completa

---

## Fase 11: Integración con IM Externos 🔜

**Objetivo**: Conectar con plataformas de mensajería populares.

- [ ] Integración con Feishu/Lark
- [ ] Integración con WhatsApp
- [ ] Integración con Telegram
- [ ] Adaptador de IM genérico para plataformas personalizadas

---

## Fase 12: Gráfico de Conocimiento + Ecosistema de Plugins 🔜

**Objetivo**: Capacidades avanzadas de conocimiento y extensibilidad.

- [ ] Sistema de gráfico de conocimiento
- [ ] Sistema de plugins
- [ ] Ecosistema de habilidades (skills)
- [ ] Mercado de plugins/habilidades

---

## Estado Actual

✅ Fases 1-10.5: Completadas y funcionando
🔜 Fase 11: Planificada
🔜 Fase 12: Planificada
