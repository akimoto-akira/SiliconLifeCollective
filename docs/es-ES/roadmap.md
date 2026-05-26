# Hoja de Ruta

> **Versión: v0.2.0-alpha**

[English](../en/roadmap.md) | [Deutsch](../de-DE/roadmap.md) | [中文](../zh-CN/roadmap.md) | [繁體中文](../zh-HK/roadmap.md) | **Español** | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Čeština](../cs-CZ/roadmap.md) | [Русский](../ru-RU/roadmap.md)

## Hoja de Ruta de Doble Versión

### SiliconLife.Default (Versión por defecto)
- **Posicionamiento**: Implementación por defecto, principalmente para verificar la viabilidad de la arquitectura
- **Estado actual**: Fases 1-10.6 completadas, el sistema funciona de manera estable
- **Descripción del rol**: Como implementación de referencia para verificación de arquitectura, garantiza la corrección y viabilidad del diseño arquitectónico principal

### SiliconLife.Fast (Versión de alto rendimiento)
- **Posicionamiento**: Versión de producción recomendada
- **Estado actual**: Migración de arquitectura base completada, motor de almacenamiento SpeedyPack y sistema de plugins implementados
- **Descripción del rol**: Sobre la base de la arquitectura verificada en la versión Default, realiza optimización profunda de rendimiento y mejoras de características de grado de producción, siendo la opción preferida para despliegue real

**Plan de desarrollo de la versión Fast**:
- ✅ Fase 1: Estructura base del proyecto y migración del sistema de configuración
- ✅ Fase 2: Migración de Web UI y controladores
- ✅ Fase 3: Optimización del sistema de almacenamiento (almacenamiento en memoria SpeedyPack + persistencia asíncrona)
- ✅ Fase 3.5: Herramienta de gestión SpeedyPack (aplicación Avalonia UI SiliconLife.Speedy.Manager)
- ✅ Fase 3.6: Sistema de plugins (interfaz IPlugin, Sandbox de Seguridad, aislamiento AssemblyLoadContext)
- ✅ Fase 4: Aplicación de ventana Avalonia (aplicación de escritorio multiplataforma, bandeja del sistema en Windows/macOS, ventana de estado en Linux)

---

## Principios Guía

Cada fase termina con un sistema **ejecutable y observable**. Ninguna fase produce "un montón de infraestructura sin nada que mostrar".

---

## ~~Fase 1: Se puede chatear~~ ✅ Completado

**Objetivo**: Entrada por consola → Llamada a IA → Salida por consola. Unidad mínima verificable.

| # | Módulo | Descripción |
|---|--------|-------------|
| 1.1 | Estructura de solución y proyecto | Crear `SiliconLifeCollective.sln`, con `src/SiliconLife.Core/` (biblioteca principal) y `src/SiliconLife.Default/` (implementación por defecto + punto de entrada) |
| 1.2 | Configuración (mínima) | Singleton + deserialización JSON. Leer `config.json`. Auto-generar valores por defecto si falta |
| 1.3 | Localización (mínima) | Clase abstracta `LocalizationBase`, implementación `ZhCN`. Agregar `Language` a la configuración |
| 1.4 | OllamaClient (mínimo) | Interfaz `IAIClient`, llamada HTTP a Ollama local `/api/chat`. Sin streaming, sin llamadas a herramientas |
| 1.5 | E/S por consola | `while(true) + Console.ReadLine()`, leer entrada → llamar IA → imprimir respuesta |
| 1.6 | Encabezado de copyright | Agregar encabezado Apache 2.0 a todos los archivos fuente C# |

**Entregable**: Programa de chat por consola que dialoga con el modelo Ollama local.

**Verificación**: Ejecutar el programa, escribir "hello", ver la respuesta de la IA.

---

## ~~Fase 2: Con esqueleto~~ ✅ Completado

**Objetivo**: Reemplazar el "bucle desnudo" con una estructura de framework. El comportamiento no cambia.

| # | Módulo | Descripción |
|---|--------|-------------|
| 2.1 | Almacenamiento (mínimo) | Interfaz `IStorage` (Read/Write/Exists/Delete, pares clave-valor). Implementación `FileSystemStorage`. Clase de instancia (no estática). Acceso directo al sistema de archivos — **la IA no controla IStorage** |
| 2.2 | Bucle principal + Objetos de reloj | Bucle infinito, intervalos de reloj precisos (`Stopwatch` + `Thread.Sleep`). Programación por prioridad |
| 2.3 | Estandarización de IAIClient | Interfaz `IAIClientFactory`. OllamaClient refactorizado para implementar la interfaz estándar |
| 2.4 | Migración de consola | Migrar `while(true)` a objetos de reloj impulsados por el bucle principal. Comportamiento idéntico a la Fase 1 |

**Entregable**: El bucle principal ejecuta relojes, el chat por consola sigue funcionando.

**Verificación**: Registrar un objeto de reloj de prueba que imprima el conteo de reloj cada segundo; el chat por consola sigue funcionando.

---

## ~~Fase 3: Con alma~~ ✅ Completado

**Objetivo**: El primer Ser de Silicio vive en el framework.

| # | Módulo | Descripción |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Clase base abstracta, con Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService. `Tick()` y `ExecuteOneRound()` abstractos |
| 3.2 | Carga del Archivo de Alma | `SoulFileManager`: lee `soul.md` desde el directorio de datos del ser |
| 3.3 | ContextManager (mínimo) | Conectar Archivo de Alma + mensajes recientes → llamar IA → obtener respuesta. Sin llamadas a herramientas, sin persistencia |
| 3.4 | ISiliconBeingFactory | Interfaz de fábrica para crear instancias de seres |
| 3.5 | SiliconBeingManager (mínimo) | Hereda de objeto de reloj (prioridad=0). Itera todos los seres, llama a su Tick secuencialmente |
| 3.6 | DefaultSiliconBeing | Implementación de comportamiento estándar. Verifica mensajes no leídos → crea ContextManager → ExecuteOneRound → salida |
| 3.7 | Estructura de directorios del ser | `DataDirectory/SiliconManager/{GUID}/`, con `soul.md` y `state.json` |

**Entregable**: Ser de Silicio impulsado por el bucle principal, recibe entrada por consola, carga el Archivo de Alma, llama a la IA.

**Verificación**: Entrada por consola → Tick del bucle principal → el ser procesa (comportamiento guiado por el Archivo de Alma) → respuesta de IA. El estilo de respuesta debe diferir de la Fase 1.

---

## ~~Fase 4: Con memoria~~ ✅ Completado

**Objetivo**: Las conversaciones persisten después de reiniciar.

| # | Módulo | Descripción |
|---|--------|-------------|
| 4.1 | ChatSystem | Concepto de canal (dos GUID = un canal). Modelo de mensajes con persistencia. Sin chat grupal |
| 4.2 | IIMProvider + IMManager | Interfaz `IIMProvider`. `ConsoleProvider` como canal de mensajería instantánea formal. `IMManager` enruta mensajes |
| 4.3 | Mejora de ContextManager | Extraer historial del sistema de chat. Persistir respuestas de IA. Soporte para continuación de llamadas a herramientas de múltiples turnos |
| 4.4 | Modelo IMessage | Modelo de mensaje unificado compartido por el sistema de chat y el gestor de mensajería instantánea |

**Entregable**: Sistema de chat con memoria persistente.

**Verificación**: Chatear varias rondas → salir → reiniciar → preguntar "¿de qué hablamos?" → el ser puede responder.

---

## ~~Fase 5: Puede actuar (Sistema de herramientas)~~ ✅ Completado

**Objetivo**: Los Seres de Silicio pueden ejecutar acciones, no solo chatear.

| # | Módulo | Descripción |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | Interfaz `ITool`, con Name, Description, Execute. `ToolResult` con Success, Message, Data |
| 5.2 | ToolManager | Instancia por ser. Descubrimiento de herramientas basado en reflexión. Soporte de atributo `[SiliconManagerOnly]` |
| 5.3 | IAIClient: Soporte de llamadas a herramientas | Analizar tool_calls de IA. Bucle: ejecutar herramienta → enviar resultado → IA continúa → hasta texto puro |
| 5.4 | Clase base de Ejecutor | Clase base abstracta, con hilo de programación independiente, cola de solicitudes, control de tiempo de espera |
| 5.5 | NetworkExecutor | Solicitudes HTTP a través del ejecutor. Tiempo de espera, cola |
| 5.6 | CommandLineExecutor | Ejecución de Shell a través del ejecutor. Detección de separadores multiplataforma |
| 5.7 | DiskExecutor | Operaciones de archivos a través del ejecutor. Sin verificación de permisos (Fase 6) |
| 5.8–5.12 | Herramientas integradas | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**Entregable**: Los Seres de Silicio pueden llamar herramientas para ejecutar acciones.

**Verificación**: Preguntar "¿qué día es hoy" → CalendarTool responde; preguntar "verificar procesos" → SystemTool ejecuta; decirle al ser que envíe un mensaje a otro ser → ChatTool funciona.

---

## ~~Fase 6: Sigue las reglas (Sistema de permisos)~~ ✅ Completado

**Objetivo**: Los Seres de Silicio no pueden acceder a recursos sensibles sin autorización.

| # | Módulo | Descripción |
|---|--------|-------------|
| 6.1 | PermissionManager | Instancia privada por ser. Basado en retrollamada, resultado ternario (Allowed/Deny/AskUser). Prioridad de consulta: HighDeny → HighAllow → Callback. Bandera IsCurator |
| 6.2 | Enumeración PermissionType | NetworkAccess, CommandLine, FileAccess, Function, DataAccess |
| 6.3 | DefaultPermissionCallback | Lista blanca/negra de red, clasificación de CLI, reglas de seguridad de rutas de archivos |
| 6.4 | GlobalACL | Tabla de reglas de coincidencia de prefijos, persistida en almacenamiento |
| 6.5 | UserFrequencyCache | Listas HighAllow/HighDeny. Selección del usuario (no detección automática). Coincididencia de prefijos, solo en memoria, expiración configurable |
| 6.6 | Mecanismo UserAsk (consola) | Cuando devuelve AskUser, la consola solicita y/n |
| 6.7 | Integración de permisos en ejecutores | Todos los ejecutores verifican permisos antes de ejecutar |
| 6.8 | Nota de aislamiento de IStorage | IStorage es persistencia interna del sistema — acceso directo a archivos, **no** enrutado a través de ejecutores, **no** controlable por IA. Los ejecutores solo gestionan IO iniciado por herramientas de IA |
| 6.9 | Registro de auditoría | Registrar todas las decisiones de permisos, con marca de tiempo, solicitante, recurso, resultado |

**Entregable**: Aparecen indicaciones de permisos cuando el ser intenta operaciones sensibles.

**Verificación**: Decirle al ser que elimine un archivo → la consola muestra indicación de permiso → ingresar `n` → operación denegada. Decirle al ser que acceda a sitios en lista blanca → permitido inmediatamente.

---

## ~~Fase 7: Puede evolucionar (Compilación dinámica)~~ ✅ Completado

**Objetivo**: Los Seres de Silicio pueden reescribir su propio código.

| # | Módulo | Descripción |
|---|--------|-------------|
| 7.1 | CodeEncryption | Cifrado/descifrado AES-256. Derivar clave PBKDF2 desde GUID |
| 7.2 | DynamicCompilationExecutor | Sandbox de compilación en memoria basado en Roslyn. Control de referencias de ensamblado en tiempo de compilación (defensa principal: excluir System.IO, Reflection, etc.) |
| 7.3 | Escaneo de seguridad | Análisis estático en tiempo de ejecución de patrones de código peligroso (defensa secundaria). Bloquear carga si el escaneo falla |
| 7.4 | Mejora del ciclo de vida del ser | Carga: descifrar → escanear → compilar → instanciar. Tiempo de ejecución: compilar en memoria → reemplazo atómico → persistir cifrado |
| 7.5 | SiliconCurator | Clase base abstracta del Curador. IsCurator=true. Permisos máximos |
| 7.6 | DefaultCurator | Implementación por defecto del Curador, con Archivo de Alma integrado y herramientas de gestión |
| 7.7 | CuratorTool | Herramienta `[SiliconManagerOnly]`: list_beings, create_being, get_code, reset |
| 7.8 | Sobrescritura de retrollamada de permisos | Los seres pueden compilar retrollamadas de permisos personalizadas |
| 7.9 | Mejora de SiliconBeingManager | Método Replace (intercambio de instancias en tiempo de ejecución). MigrateState (transferir estado entre instancia antigua y nueva) |

**Entregable**: Los Seres de Silicio pueden generar nuevo código a través de IA, compilarlo y reemplazarse a sí mismos.

**Verificación**: Decirle al ser "agrégate una nueva función" → observar compilación → reiniciar → la nueva función funciona.

---

## ~~Fase 8: Memoria y planificación~~ ✅ Completado

**Objetivo**: Memoria a largo plazo, gestión de tareas, activación por temporizador.

| # | Módulo | Descripción |
|---|--------|-------------|
| 8.1 | FileSystemMemory | Almacenamiento segmentado a corto/largo plazo. Decaimiento temporal. Compresión (fusionar memorias similares). Búsqueda multidimensional |
| 8.2 | TaskSystem | Tareas únicas + dependencias DAG. Programación por prioridad. Seguimiento de estado |
| 8.3 | TimerSystem | Alarmas únicas + temporizadores periódicos. Precisión de milisegundos. Persistidos en almacenamiento |
| 8.4 | IncompleteDate | Estructura de rango de fecha difusa (por ejemplo, "abril de 2026", "primavera de 2026") |
| 8.5–8.7 | Herramientas de memoria/tarea/temporizador | Herramientas para que los seres consulten memoria, gestionen tareas, configuren temporizadores |

**Entregable**: Los seres pueden recordar puntos clave, crear/rastrear tareas, configurar alarmas.

**Verificación**: Crear tarea → verificar lista de tareas → configurar alarma de 1 minuto → recibir notificación cuando llegue el momento.

---

## ~~Fase 9: Framework completado~~ ✅ Completado

**Objetivo**: Punto de entrada unificado, colaboración multi-ser.

| # | Módulo | Descripción |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | Host unificado usando patrón builder. Apagado elegante (Ctrl+C / SIGTERM) |
| 9.2 | Refactorización de Program.Main | Migrar al patrón CoreHostBuilder |
| 9.3 | Mejora de SiliconBeingManager | Respuesta prioritaria del Curador. Aislamiento de excepciones. Persistencia periódica |
| 9.4 | Carga de múltiples seres | Cargar múltiples seres desde el directorio de datos. Comunicación entre seres a través de ChatTool |
| 9.5 | Monitoreo de rendimiento | Seguimiento del tiempo de ejecución de cada objeto de reloj |
| 9.6 | ServiceLocator | Localizador de servicios global, con métodos Register/Get |

**Entregable**: Múltiples seres ejecutándose simultáneamente, colaborando, gestionados por CoreHost.

**Verificación**: Crear dos seres → A envía mensaje a B → B recibe y responde → el framework programa sin errores. El Curador responde con prioridad cuando llegan mensajes de usuario.

---

## ~~Fase 10: Hacia la Web~~ ✅ Completado

**Objetivo**: Migrar de la consola a la interfaz del navegador.

| # | Módulo | Descripción |
|---|--------|-------------|
| 10.1 | Router | Enrutador de solicitudes HTTP. Rutas de parámetros de serie y servicio de archivos estáticos |
| 10.2 | Clase base Controller | Contexto de solicitud/respuesta. Soporte de respuestas HTML y JSON |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | Constructores del lado del servidor en C#. Cero dependencias de frameworks frontend |
| 10.6 | SSE (Server-Sent Events) | Actualizaciones push en tiempo real para chat, estado de seres y eventos del sistema. Más simple que WebSocket, con reconexión automática del cliente |
| 10.7 | WebUIProvider | Canal de mensajería instantánea en tiempo real basado en SSE. Reemplaza la consola como interfaz principal |
| 10.8 | Seguridad Web | Lista negra/blanca de IP. Atributo `[WebCode]`. Actualización dinámica |
| 10.9–10.17 | Controladores Web | Chat, panel, seres, tareas, permisos, solicitudes de permisos, ejecutores, registros, configuración, memoria, temporizadores, inicialización, acerca de, navegador de código, conocimiento, proyectos, auditoría |

**Entregable**: Web UI completa accesible desde el navegador.

**Verificación**: Abrir navegador → chatear con un ser → ver el panel → gestionar permisos → todo funciona correctamente.

---

## ~~Fase 10.5: Mejoras incrementales~~ ✅ Completado

**Objetivo**: Mejorar el sistema existente con nuevas funcionalidades descubiertas durante el desarrollo.

| # | Módulo | Descripción |
|---|--------|-------------|
| 10.5.1 | BroadcastChannel | Nuevo tipo de sesión para anuncios de todo el sistema. ID de canal fijo, suscripción dinámica, filtrado de mensajes pendientes |
| 10.5.2 | Mejora de ChatMessage | Campos ToolCallId, ToolCallsJson, Thinking para contexto de IA; PromptTokens, CompletionTokens, TotalTokens para seguimiento de tokens; tipo de mensaje SystemNotification |
| 10.5.3 | TokenUsageAuditManager | Seguimiento del consumo de tokens por solicitud en todos los seres. Estadísticas agregadas, consultas de series temporales, almacenamiento persistente |
| 10.5.4 | TokenAuditTool | Herramienta `[SiliconManagerOnly]` para que el Curador consulte y resuma el uso de tokens |
| 10.5.5 | ConfigTool | Herramienta `[SiliconManagerOnly]` para que el Curador lea y modifique la configuración del sistema |
| 10.5.6 | AuditController | Panel web para auditoría de uso de tokens, con gráficos de tendencias y exportación de datos |
| 10.5.7 | Extensión del sistema de calendario | 32 implementaciones de calendario, cubriendo sistemas calendarios del mundo (budista, lunar chino, islámico, hebreo, japonés, persa, maya, etc.) |
| 10.5.8 | Mejora de DiskTool | Nuevas operaciones: count_lines, read_lines, clear_file, replace_lines, replace_text, replace_text_all, list_drives |
| 10.5.9 | Mejora de SystemTool | Nuevas operaciones: find_process (soporte de comodines), resource_usage |
| 10.5.10 | Mejora de CalendarTool | Nuevas operaciones: diff, list_calendars, get_components, get_now_components, convert (conversión entre calendarios) |
| 10.5.11 | DashScopeClient | Cliente de IA Bailian de Alibaba Cloud, compatible con API de OpenAI. Soporta streaming, llamadas a herramientas, contenido de razonamiento |
| 10.5.12 | DashScopeClientFactory | Fábrica para crear clientes de Bailian. Descubrimiento dinámico de modelos a través de API. Soporte multirregión (Pekín, Virginia, Singapur, Hong Kong, Fráncfort) |
| 10.5.13 | Sistema de configuración de clientes de IA | Configuración de cliente de IA por ser. Opciones de claves de configuración dinámica (modelo, región). Nombres de visualización localizados |
| 10.5.14 | Extensión de localización | Localización en chino simplificado, chino tradicional, inglés y japonés para opciones de configuración de Bailian, nombres de modelos y nombres de regiones |

**Entregable**: Herramientas mejoradas, observabilidad, cobertura de calendarios y soporte de múltiples backends de IA.

**Verificación**: El Curador consulta el uso de tokens a través de TokenAuditTool → el panel de auditoría muestra tendencias → CalendarTool convierte fechas entre 32 sistemas de calendario → cambiar el backend de IA a Bailian → chatear con el modelo Qwen a través de la API en la nube.

---

## ~~Fase 10.6: Perfeccionamiento y optimización~~ ✅ Completado

**Objetivo**: Perfeccionar las funciones del sistema, agregar nuevas características, optimizar la experiencia del usuario.

| # | Módulo | Descripción |
|---|--------|-------------|
| 10.6.1 | WebViewBrowserTool | Herramienta de automatización de navegador multiplataforma basada en Playwright, soporta modo sin cabeza, aislamiento individual, soporte completo de JS/CSS |
| 10.6.2 | HelpTool | Herramienta del sistema de documentación de ayuda, soporta consulta y visualización de documentación multilingüe |
| 10.6.3 | ProjectWorkNoteTool | Herramienta de notas de trabajo de proyecto, soporta registros y gestión de trabajo por dimensión de proyecto |
| 10.6.4 | ProjectTaskTool | Herramienta de gestión de tareas de proyecto, soporta asignación de tareas y seguimiento de progreso |
| 10.6.5 | KnowledgeTool | Herramienta de red de conocimiento, soporta CRUD de conocimiento de tripletas y descubrimiento de rutas |
| 10.6.6 | ChatHistoryController | Controlador de visualización de historial de chat, soporta lista de sesiones y detalles de mensajes |
| 10.6.7 | CodeHoverController | Controlador de sugerencias flotantes de código, soporta resaltado de sintaxis y sugerencias de código |
| 10.6.8 | WorkNoteController | Controlador de gestión de notas de trabajo, soporta búsqueda y generación de directorio |
| 10.6.9 | TimerExecutionHistory | Función de historial de ejecución de temporizadores, registro y visualización del historial de activaciones |
| 10.6.10 | Extensión de localización | Agregar soporte de localización checo (cs-CZ), total de 21 variantes de idioma |
| 10.6.11 | Optimización de Web UI | Soporte de carga de archivos, indicadores de carga, optimización de renderizado de llamadas a herramientas, corrección de modal de notas de trabajo |
| 10.6.12 | Mejora de gestión de memoria | Filtrado avanzado, estadísticas, vista de detalles, optimización de algoritmo de compresión |
| 10.6.13 | Refactorización del sistema de registros | Separación de registros de sistema/Ser de Silicio, API de lectura de registros, filtro de Ser de Silicio |
| 10.6.14 | Mejora del sistema de permisos | Validación de precompilación de retrollamada de permisos, validación de referencias de ensamblado, lista blanca del servicio meteorológico wttr.in |

**Entregable**: Automatización completa de navegador WebView, sistema de documentación de ayuda, espacio de trabajo de proyectos, red de conocimiento, visualización de historial de chat y otras mejoras.

**Verificación**: Los Seres de Silicio pueden operar el navegador a través de WebViewBrowserTool → obtener documentación de ayuda a través de HelpTool → gestionar notas de trabajo y tareas de proyectos → consultar la red de conocimiento → ver el historial de chat.

---

## ~~Fase 10.7: Colaboración de proyectos y flujos de trabajo~~ ✅ Completado

**Objetivo**: Agregar espacio de trabajo de proyectos, motor de flujos de trabajo, mecanismo de desvanecimiento de memoria y sistema de permisos de herramientas.

| # | Módulo | Descripción |
|---|--------|-------------|
| 10.7.1 | Gestión de roles de proyecto | ProjectTool agrega operaciones assign_role, remove_role, list_roles |
| 10.7.2 | Motor de flujos de trabajo | Motor central WorkflowEngine, soporta definición de plantillas, transiciones de estado, ejecución impulsada por Tick |
| 10.7.3 | Plantillas de flujo de trabajo | Clase base WorkflowTemplate, define conjuntos de estados y reglas de transición |
| 10.7.4 | Instancias de flujo de trabajo | Gestión de instancias WorkflowInstance, vinculadas a proyectos específicos, seguimiento del estado actual |
| 10.7.5 | Registros de flujo de trabajo | WorkflowLog registra el historial de transiciones de estado |
| 10.7.6 | Mecanismo de desvanecimiento de memoria | MemoryFadeService servicio de decaimiento temporal, aplica automáticamente decaimiento de importancia y archivado a las memorias cada hora |
| 10.7.7 | Sistema de permisos de herramientas | Permisos de herramientas de dos niveles (nivel del Ser de Silicio + nivel de proyecto), plantillas de permisos, control granular de operaciones |
| 10.7.8 | ToolPermissionController | Controlador web de gestión de permisos de herramientas |
| 10.7.9 | ProjectWorkTool | Herramienta de operaciones de trabajo de proyecto ([SiliconManagerOnly], [ToolScenario(Project)]) |
| 10.7.10 | Sistema de escenarios de herramientas | ToolScenarioAttribute y ChatOnlyAttribute, soporta filtrado de escenarios Chat/Task/Timer/MemoryCompression/Project |
| 10.7.11 | Extensión de localización | Agregar localización en ruso, portugués, italiano, neerlandés, polaco, sueco, total de 34 variantes de idioma |

**Entregable**: Sistema completo de colaboración de proyectos, motor de flujos de trabajo, mecanismo de desvanecimiento de memoria y gestión de permisos de herramientas.

**Verificación**: Crear proyecto → asignar roles → vincular plantilla de flujo de trabajo → los seres colaboran dentro del espacio del proyecto → la memoria se archiva automáticamente por decaimiento → el aislamiento de permisos de herramientas funciona.

---

## Fase 11: Integración de mensajería instantánea externa

**Objetivo**: Conectar con plataformas de mensajería externas para mayor accesibilidad de los usuarios.

| # | Módulo | Descripción |
|---|--------|-------------|
| 11.1 | FeishuProvider | Integración de robot Feishu (Lark), soporta tarjetas |
| 11.2 | WhatsAppProvider | Integración de WhatsApp Business API |
| 11.3 | TelegramProvider | Integración de Telegram Bot API, soporta teclados en línea |
| 11.4 | Mejora de IMManager | Enrutamiento multiproveedor, formato de mensaje unificado, manejo de consultas de permisos multiplataforma |

**Entregable**: Los usuarios pueden interactuar con los Seres de Silicio a través de plataformas de mensajería instantánea externas.

---

## Fase 12: Funciones avanzadas

**Objetivo**: Funciones avanzadas opcionales para capacidades mejoradas.

| # | Módulo | Descripción |
|---|--------|-------------|
| 12.1 | ~~Red de conocimiento~~ ✅ Completado | Grafo de conocimiento de estructura triplete (sujeto-predicado-objeto), soporta CRUD, descubrimiento de rutas, consultas avanzadas y recorrido del grafo |
| 12.2 | ~~Sistema de plugins~~ ✅ Completado | Carga de plugins externos, con verificación de seguridad y sandbox (interfaz IPlugin, PluginLoader, aislamiento AssemblyLoadContext) |
| 12.3 | Ecosistema de habilidades | Mercado de habilidades reutilizables para capacidades de los seres |
