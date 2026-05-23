# Hoja de Ruta

> **Versión: v0.2.0-alpha**

[English](../en/roadmap.md) | [中文](../zh-CN/roadmap.md) | [繁體中文](../zh-HK/roadmap.md) | **Español** | [Deutsch](../de-DE/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Čeština](../cs-CZ/roadmap.md)

## Hoja de Ruta de Versión Dual

### SiliconLife.Default (Versión Predeterminada)
- **Posicionamiento**: Implementación predeterminada, utilizada principalmente para verificación de viabilidad de arquitectura
- **Estado Actual**: Las fases actuales 1-10.6 están completadas, el sistema se ejecuta de manera estable
- **Descripción de Rol**: Implementación de referencia para verificación de arquitectura que garantiza la corrección y viabilidad del diseño de arquitectura central

### SiliconLife.Fast (Versión de Alto Rendimiento)
- **Posicionamiento**: Versión de producción principal
- **Estado Actual**: Portería de arquitectura básica completada, motor de almacenamiento SpeedyPack y sistema de plugins implementados
- **Descripción de Rol**: Basada en la arquitectura verificada en la versión Default, realiza optimización profunda de rendimiento y fortalecimiento de características de producción, la mejor opción para despliegue real

**Plan de Desarrollo de Fast**:
- ✅ Fase 1: Portería de estructura de proyecto básica y sistema de configuración
- ✅ Fase 2: Portería de Web UI y controladores
- ✅ Fase 3: Optimización del sistema de almacenamiento (almacenamiento en memoria SpeedyPack + persistencia asíncrona, formato de archivo .spk)
- ✅ Fase 3.5: Herramienta de gestión SpeedyPack (SiliconLife.Speedy.Manager aplicación Windows Forms)
- 📋 Fase 5: Optimización de rendimiento (pool de conexiones, pool de objetos, concurrencia sin bloqueo)
- 📋 Fase 6: Reemplazo del servidor web Kestrel
- 📋 Fase 7: Serialización binaria MessagePack

---

## Principios Rectores

Cada fase termina con un sistema **funcional y observable**. Ninguna fase produce "un montón de infraestructura sin nada que mostrar".

---

## ~~Fase 1: Puede Chatear~~ ✅ Completado

**Objetivo**: Entrada de consola → Llamada de IA → Salida de consola. Unidad mínima verificable.

| # | Módulo | Descripción |
|---|--------|-------------|
| 1.1 | Estructura de solución y proyectos | Crear `SiliconLifeCollective.sln`, con `src/SiliconLife.Core/` (biblioteca central) y `src/SiliconLife.Default/` (implementación predeterminada + punto de entrada) |
| 1.2 | Configuración (mínima) | Singleton + deserialización JSON. Leer `config.json`. Generar automáticamente valores predeterminados si falta |
| 1.3 | Localización (mínima) | Clase abstracta `LocalizationBase`, implementación `ZhCN`. Añadir `Language` en configuración |
| 1.4 | OllamaClient (mínimo) | Interfaz `IAIClient`, llamada HTTP a Ollama local `/api/chat`. Sin streaming, sin invocación de herramientas |
| 1.5 | E/S de consola | `while(true) + Console.ReadLine()`, leer entrada → llamar IA → imprimir respuesta |
| 1.6 | Encabezado de copyright | Añadir encabezado Apache 2.0 a todos los archivos fuente C# |

**Entregable**: Programa de chat de consola con modelo Ollama local.

**Verificación**: Ejecutar programa, escribir "hello", ver respuesta de IA.

---

## ~~Fase 2: Tiene Esqueleto~~ ✅ Completado

**Objetivo**: Reemplazar "bucle desnudo" con estructura de framework. Comportamiento sin cambios.

| # | Módulo | Descripción |
|---|--------|-------------|
| 2.1 | Almacenamiento (mínimo) | Interfaz `IStorage` (Read/Write/Exists/Delete, pares clave-valor). Implementación `FileSystemStorage`. Clase de instancia (no estática). Acceso directo al sistema de archivos — **IA no puede controlar IStorage** |
| 2.2 | Bucle principal + objetos de reloj | Bucle infinito, intervalos de reloj precisos (`Stopwatch` + `Thread.Sleep`). Programación por prioridad |
| 2.3 | Estandarización de IAIClient | Interfaz `IAIClientFactory`. OllamaClient refactorizado para implementar interfaz estándar |
| 2.4 | Migración de consola | Migrar `while(true)` a objeto de reloj impulsado por bucle principal. Mismo comportamiento que Fase 1 |

**Entregable**: Bucle principal ejecutando reloj, chat de consola todavía funciona.

**Verificación**: Registrar objeto de reloj de prueba, imprimir conteo de reloj cada segundo; chat de consola todavía funciona.

---

## ~~Fase 3: Tiene Alma~~ ✅ Completado

**Objetivo**: Primer Ser Silicona vivo en el framework.

| # | Módulo | Descripción |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Clase base abstracta con Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService. `Tick()` y `ExecuteOneRound()` abstractos |
| 3.2 | Carga de archivo de alma | `SoulFileManager`: leer `soul.md` desde directorio de datos del ser |
| 3.3 | ContextManager (mínimo) | Conectar archivo de alma + mensajes recientes → llamar IA → obtener respuesta. Sin invocación de herramientas, sin persistencia |
| 3.4 | ISiliconBeingFactory | Interfaz de fábrica para crear instancias de seres |
| 3.5 | SiliconBeingManager (mínimo) | Hereda objeto de reloj (prioridad=0). Iterar todos los seres, llamar sus Tick secuencialmente |
| 3.6 | DefaultSiliconBeing | Implementación de comportamiento estándar. Verificar mensajes no leídos → crear ContextManager → ExecuteOneRound → salida |
| 3.7 | Estructura de directorio de seres | `DataDirectory/SiliconManager/{GUID}/`, con `soul.md` y `state.json` |

**Entregable**: Ser Silicona impulsado por bucle principal, recibe entrada de consola, carga archivo de alma, llama IA.

**Verificación**: Entrada de consola → activación de reloj de bucle principal → procesamiento de ser (con comportamiento guiado por archivo de alma) → respuesta de IA. Estilo de respuesta debe ser diferente de Fase 1.

---

## ~~Fase 4: Tiene Memoria~~ ✅ Completado

**Objetivo**: Conversaciones persisten después de reinicio.

| # | Módulo | Descripción |
|---|--------|-------------|
| 4.1 | ChatSystem | Concepto de canal (dos GUID = un canal). Modelo de mensaje con persistencia. Sin chat grupal aún |
| 4.2 | IIMProvider + IMManager | Interfaz `IIMProvider`. `ConsoleProvider` como canal de mensajería instantánea formal. `IMManager` enruta mensajes |
| 4.3 | ContextManager mejorado | Extraer historial de sistema de chat. Persistir respuestas de IA. Soporte para continuación de invocación de herramientas de múltiples rondas |
| 4.4 | Modelo IMessage | Modelo de mensaje unificado compartido por sistema de chat y gestor de mensajería instantánea |

**Entregable**: Sistema de chat con memoria persistente.

**Verificación**: Chatear varias rondas → salir → reiniciar → preguntar "¿de qué hablamos?" → el ser puede responder.

---

## ~~Fase 5: Puede Actuar (Sistema de Herramientas)~~ ✅ Completado

**Objetivo**: Seres Silicona pueden ejecutar acciones, no solo chatear.

| # | Módulo | Descripción |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | Interfaz `ITool` con Name, Description, Execute. `ToolResult` con Success, Message, Data |
| 5.2 | ToolManager | Instancia por ser. Descubrimiento de herramientas basado en reflexión. Soporte de atributo `[SiliconManagerOnly]` |
| 5.3 | IAIClient: Soporte de invocación de herramientas | Analizar tool_calls de IA. Bucle: ejecutar herramienta → enviar resultado de vuelta → IA continúa → hasta texto puro |
| 5.4 | Clase base de ejecutor | Clase base abstracta con hilo de programación independiente, cola de solicitudes, control de timeout |
| 5.5 | NetworkExecutor | Solicitudes HTTP a través de ejecutor. Timeout, cola |
| 5.6 | CommandLineExecutor | Ejecución de shell a través de ejecutor. Detección de separador multiplataforma |
| 5.7 | DiskExecutor | Operaciones de archivo a través de ejecutor. Sin verificación de permisos aún (Fase 6) |
| 5.8–5.12 | Herramientas integradas | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**Entregable**: Seres Silicona pueden invocar herramientas para ejecutar acciones.

---

## ~~Fase 6: Seguridad (Sistema de Permisos)~~ ✅ Completado

**Objetivo**: Todas las operaciones de E/S pasan por verificación de permisos.

| # | Módulo | Descripción |
|---|--------|-------------|
| 6.1 | PermissionManager | Gestor de permisos privado por ser. Cadena de 5 niveles |
| 6.2 | GlobalACL | Lista de control de acceso global. Persistida al almacenamiento |
| 6.3 | UserFrequencyCache | Caché de frecuencia de usuario (solo memoria) para reducir prompts repetitivos |
| 6.4 | IPermissionCallback | Interfaz de callback para lógica de permisos personalizada |
| 6.5 | IPermissionAskHandler | Interfaz para preguntar al usuario por permisos |
| 6.6 | AuditLogger | Registro de auditoría para todas las decisiones de permisos |
| 6.7 | Integración de ejecutor | Ejecutores consultan PermissionManager antes de ejecutar |

**Entregable**: Sistema de permisos de 5 niveles con registro de auditoría.

---

## ~~Fase 7: Auto-Evolución (Compilación Dinámica)~~ ✅ Completado

**Objetivo**: Seres pueden reescribir su propio código.

| # | Módulo | Descripción |
|---|--------|-------------|
| 7.1 | DynamicBeingLoader | Cargador de compilación dinámica usando Roslyn |
| 7.2 | SecurityScanner | Análisis estático de código para operaciones peligrosas |
| 7.3 | Control de referencias en compilación | Compilador solo obtiene lista permitida de ensamblados |
| 7.4 | Almacenamiento cifrado | Código cifrado con AES-256 en disco |
| 7.5 | Reemplazo atómico | Compilar en memoria → crear instancia → migrar estado → intercambiar → persistir |
| 7.6 | ReplacePermissionCallback | Permitir a seres compilar e inyectar callbacks de permisos personalizados |

**Entregable**: Seres pueden auto-evolucionarse reescribiendo código.

---

## ~~Fase 8: Memoria a Largo Plazo y Tareas~~ ✅ Completado

**Objetivo**: Seres pueden recordar y planificar.

| # | Módulo | Descripción |
|---|--------|-------------|
| 8.1 | Sistema de memoria a largo plazo | Persistir y consultar experiencias pasadas |
| 8.2 | Sistema de tareas | Crear, gestionar y completar tareas |
| 8.3 | Sistema de temporizadores | Temporizadores de una vez, intervalo y cron |
| 8.4 | Sistema de notas de trabajo | Notas personales para seres con Markdown y palabras clave |

**Entregable**: Seres pueden recordar pasado y planificar futuro.

---

## ~~Fase 9: Host Central~~ ✅ Completado

**Objetivo**: Unificar todos los componentes en host central.

| # | Módulo | Descripción |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | Host unificado con patrón builder. Apagado elegante (Ctrl+C / SIGTERM) |
| 9.2 | Refactorización de Program.Main | Migración al patrón CoreHostBuilder |
| 9.3 | SiliconBeingManager mejorado | Respuesta prioritaria del curador. Aislamiento de excepciones. Persistencia periódica |
| 9.4 | Carga de múltiples seres | Cargar múltiples seres desde directorio de datos. Comunicación entre seres a través de ChatTool |
| 9.5 | Monitoreo de rendimiento | Seguimiento del tiempo de ejecución de cada objeto de reloj |
| 9.6 | ServiceLocator | Localizador de servicios global con métodos Register/Get |

**Entregable**: Múltiples seres ejecutándose simultáneamente, colaborando, gestionados por CoreHost.

**Verificación**: Crear dos seres → A envía mensaje a B → B recibe y responde → framework programa sin errores. Curador responde prioritariamente cuando llega un mensaje de usuario.

---

## ~~Fase 10: Hacia la Web~~ ✅ Completado

**Objetivo**: Migrar de consola a interfaz de navegador.

| # | Módulo | Descripción |
|---|--------|-------------|
| 10.1 | Router | Enrutador de solicitudes HTTP. Rutas de parámetros serializados y servicio de archivos estáticos |
| 10.2 | Clase base Controller | Contexto de solicitud/respuesta. Soporte de respuesta HTML y JSON |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | Constructores del lado del servidor en C#. Cero dependencia de framework frontend |
| 10.6 | SSE (Eventos Enviados por Servidor) | Actualizaciones en tiempo real push para chat, estado de seres y eventos del sistema. Más simple que WebSocket, con reconexión automática del cliente |
| 10.7 | WebUIProvider | Canal de mensajería instantánea en tiempo real basado en SSE. Reemplaza la consola como interfaz principal |
| 10.8 | Seguridad Web | Lista negra/blanca de IP. Atributo `[WebCode]`. Actualización dinámica |
| 10.9–10.17 | Controladores Web | Chat, Dashboard, Seres, Tareas, Permisos, Solicitudes de Permisos, Ejecutores, Registro, Configuración, Memoria, Temporizadores, Inicialización, Acerca de, Navegador de Código, Conocimiento, Proyectos, Auditoría |

**Entregable**: Web UI completa accesible desde el navegador.

**Verificación**: Abrir navegador → chatear con ser → ver dashboard → gestionar permisos → todas las funciones funcionan.

---

## ~~Fase 10.5: Mejoras Incrementales~~ ✅ Completado

**Objetivo**: Mejorar el sistema existente con nuevas funcionalidades descubiertas durante el desarrollo.

| # | Módulo | Descripción |
|---|--------|-------------|
| 10.5.1 | BroadcastChannel | Nuevo tipo de sesión para anuncios a nivel del sistema. ID de canal fijo, suscripción dinámica, filtrado de mensajes pendientes |
| 10.5.2 | ChatMessage mejorado | Campos ToolCallId, ToolCallsJson, Thinking para contexto de IA; PromptTokens, CompletionTokens, TotalTokens para seguimiento de tokens; tipo de mensaje SystemNotification |
| 10.5.3 | TokenUsageAuditManager | Seguimiento de consumo de tokens por solicitud en todos los seres. Estadísticas agregadas, consulta de series temporales, almacenamiento persistente |
| 10.5.4 | TokenAuditTool | Herramienta `[SiliconManagerOnly]` para que el curador consulte y resuma el uso de tokens |
| 10.5.5 | ConfigTool | Herramienta `[SiliconManagerOnly]` para que el curador lea y modifique la configuración del sistema |
| 10.5.6 | AuditController | Dashboard web para auditoría de uso de tokens, con gráficos de tendencia y exportación de datos |
| 10.5.7 | Extensión del sistema de calendario | 32 implementaciones de calendario, cubriendo sistemas de calendario mundiales (Budista, Lunar, Islámico, Hebreo, Japonés, Persa, Maya, etc.) |
| 10.5.8 | DiskTool mejorado | Nuevas operaciones: count_lines, read_lines, clear_file, replace_lines, replace_text, replace_text_all, list_drives |
| 10.5.9 | SystemTool mejorado | Nuevas operaciones: find_process (con soporte wildcard), resource_usage |
| 10.5.10 | CalendarTool mejorado | Nuevas operaciones: diff, list_calendars, get_components, get_now_components, convert (conversión entre calendarios) |
| 10.5.11 | DashScopeClient | Cliente de IA Alibaba Cloud Bailian, compatible con API OpenAI. Soporta streaming, invocación de herramientas, contenido de razonamiento |
| 10.5.12 | DashScopeClientFactory | Fábrica para crear clientes Bailian. Descubrimiento dinámico de modelos a través de API. Soporte multi-región (Beijing, Virginia, Singapur, Hong Kong, Fráncfort) |
| 10.5.13 | Sistema de configuración de clientes de IA | Configuración de cliente de IA por ser. Opciones de claves de configuración dinámicas (modelos, regiones). Nombres de visualización localizados |
| 10.5.14 | Extensión de localización | Localización en chino simplificado, chino tradicional, inglés y japonés para opciones de configuración Bailian, nombres de modelos y nombres de regiones |

**Entregable**: Herramientas mejoradas, observabilidad, cobertura de calendarios y soporte de múltiples backends de IA.

**Verificación**: Curador consulta uso de tokens a través de TokenAuditTool → dashboard de auditoría muestra tendencias → CalendarTool convierte fechas entre 32 sistemas de calendario → cambiar backend de IA a Bailian → chatear con modelo Tongyi Qianwen a través de API en la nube.

---

## ~~Fase 10.6: Perfeccionamiento y Optimización~~ ✅ Completado

**Objetivo**: Perfeccionar funcionalidades del sistema, añadir nuevas características, optimizar experiencia de usuario.

| # | Módulo | Descripción |
|---|--------|-------------|
| 10.6.1 | WebViewBrowserTool | Herramienta de automatización de navegador multiplataforma basada en Playwright, soporta modo sin cabeza, aislamiento individual, soporte completo JS/CSS |
| 10.6.2 | HelpTool | Herramienta del sistema de documentación de ayuda, soporta consulta y visualización de documentación multi-idioma |
| 10.6.3 | ProjectWorkNoteTool | Herramienta de notas de trabajo de proyecto, soporta registro y gestión de trabajo por dimensión de proyecto |
| 10.6.4 | ProjectTaskTool | Herramienta de gestión de tareas de proyecto, soporta asignación de tareas y seguimiento de progreso |
| 10.6.5 | KnowledgeTool | Herramienta de red de conocimiento, soporta CRUD de tripletas de conocimiento y descubrimiento de rutas |
| 10.6.6 | ChatHistoryController | Controlador de visualización de historial de chat, soporta lista de sesiones y detalles de mensajes |
| 10.6.7 | CodeHoverController | Controlador de tooltips de código, soporta resaltado de sintaxis y sugerencias de código |
| 10.6.8 | WorkNoteController | Controlador de gestión de notas de trabajo, soporta búsqueda y generación de directorios |
| 10.6.9 | TimerExecutionHistory | Funcionalidad de historial de ejecución de temporizadores, registrar y ver historial de activaciones |
| 10.6.10 | Extensión de localización | Añadir soporte de localización checo (cs-CZ), total de 21 variantes de idioma |
| 10.6.11 | Optimización de Web UI | Soporte de subida de archivos, indicadores de carga, optimización de renderizado de invocación de herramientas, corrección de modal de notas de trabajo |
| 10.6.12 | Mejora de gestión de memoria | Filtrado avanzado, estadísticas, vista de detalles, optimización de algoritmo de compresión |
| 10.6.13 | Refactorización del sistema de registro | Separación de registros sistema/Ser Silicona, API de lectura de registros, filtro de Ser Silicona |
| 10.6.14 | Mejora del sistema de permisos | Pre-verificación de compilación de callback de permisos, verificación de referencias de ensamblados, lista blanca del servicio meteorológico wttr.in |

**Entregable**: Automatización de navegador WebView completa, sistema de documentación de ayuda, espacio de trabajo de proyectos, red de conocimiento, visualización de historial de chat y otras mejoras.

**Verificación**: Ser Silicona puede operar navegador a través de WebViewBrowserTool → obtener documentación de ayuda a través de HelpTool → gestionar notas de trabajo y tareas de proyecto → consultar red de conocimiento → ver historial de chat.

---

## Fase 11: Integración de Mensajería Instantánea Externa

**Objetivo**: Conectar con plataformas de mensajería externa para mayor accesibilidad de usuarios.

| # | Módulo | Descripción |
|---|--------|-------------|
| 11.1 | FeishuProvider | Integración de bot Feishu (Lark), soporte de tarjetas |
| 11.2 | WhatsAppProvider | Integración de WhatsApp Business API |
| 11.3 | TelegramProvider | Integración de Telegram Bot API, soporte de teclado inline |
| 11.4 | IMManager mejorado | Enrutamiento multi-proveedor, formato de mensaje unificado, manejo de solicitud de permisos multiplataforma |

**Entregable**: Los usuarios pueden interactuar con Seres Silicona a través de plataformas de mensajería instantánea externas.

---

## Fase 12: Funcionalidades Avanzadas

**Objetivo**: Funcionalidades avanzadas opcionales para capacidades mejoradas.

| # | Módulo | Descripción |
|---|--------|-------------|
| 12.1 | Red de Conocimiento | Gráfico de conocimiento compartido usando estructura de tripletas (sujeto-predicado-objeto) |
| 12.2 | ~~Sistema de Plugins~~ ✅ Completado | Carga de plugins externos con verificación de seguridad y sandbox (interfaz IPlugin, PluginLoader, aislamiento AssemblyLoadContext) |
| 12.3 | Ecosistema de Habilidades | Marketplace de habilidades reutilizables para capacidades de seres |

---

## Próximos Pasos

- 📚 Leer la [Guía de Arquitectura](architecture.md)
- 🛠️ Consultar la [Guía de Desarrollo](development-guide.md)
- 🚀 Comenzar con la [Guía de Inicio Rápido](getting-started.md)
