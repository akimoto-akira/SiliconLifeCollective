# Registro de Cambios

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | **Español** | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md) | [Русский](../ru-RU/changelog.md)

Todos los cambios importantes de este proyecto se documentarán en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
y este proyecto sigue [Versionado Semántico](https://semver.org/spec/v2.0.0.html).

---

## Acerca de este Registro de Cambios

### Doble Versión del Proyecto

Este proyecto proporciona dos versiones de implementación:

- **SiliconLife.Default**: Implementación por defecto, principalmente para verificar la viabilidad de la arquitectura. Aplicación de consola, almacenamiento JSON en sistema de archivos.
- **SiliconLife.Fast**: Versión de producción recomendada. Aplicación de escritorio multiplataforma (Windows / macOS / Linux), almacenamiento en memoria SpeedyPack + persistencia asíncrona, con optimización de rendimiento profunda.

Ambas versiones comparten las mismas interfaces y funcionalidades, difiriendo solo en la implementación de almacenamiento y el modo de ejecución. SiliconLife.Default sirve como referencia de verificación de arquitectura, SiliconLife.Fast como versión de producción recomendada.

### Origen del Proyecto

- Este proyecto se originó el 20 de marzo de 2026.
- Antes de este proyecto, hubo una demo de verificación que fracasó debido a un diseño arquitectónico inadecuado, lo que impidió la integración con múltiples plataformas de IA.

### Herramientas AI IDE Utilizadas

#### Kiro (Amazon AWS)
- El proyecto fue mantenido inicialmente por Kiro, utilizando el modo Spec para su inicio.
- Kiro es un entorno de desarrollo agentic AI construido por Amazon AWS.
- Basado en Code OSS (VS Code), soporta configuraciones de VS Code y plugins compatibles con Open VSX.
- Tiene un flujo de trabajo de desarrollo impulsado por especificaciones para codificación AI estructurada.

#### Comate AI IDE / 文心快码 (Baidu)
- Usado ocasionalmente para trabajo de redacción y documentación.
- Comate AI IDE es una herramienta de entorno de desarrollo nativo de IA lanzada por Baidu Wenxin el 23 de junio de 2025.
- Primer IDE AI del sector con colaboración multimodal y multiagente.
- Funcionalidades incluyen conversión de diseño a código y codificación asistida por IA de flujo completo.
- Impulsado por el modelo Baidu Wenxin 4.0 X1 Turbo.

#### Trae (ByteDance)
- Usado de octubre de 2025 a abril de 2026.
- IDE AI, soporta generación inteligente de código y gestión de proyectos.

#### Qoder (Alibaba)
- Usado para mantenimiento del proyecto desde el 18 de abril de 2026.
- Plataforma de codificación AI, soporta análisis de código, generación de documentación y colaboración multiagente.

#### CatPaw (Meituan)
- Usado en combinación con Qoder desde el 6 de mayo de 2026.
- Basado en la serie de modelos LongCat de desarrollo propio de Meituan, con fuerte capacidad de refactorización de arquitectura de código completo.

#### DuMate (Baidu Qianfan)
- Utilizado desde julio de 2026 para desarrollo de código, localización y documentación.
- Asistente de IA generalista en la plataforma de escritorio Qianfan, con capacidad de orquestación multi-herramienta, operaciones de archivos, automatización de navegador y ejecución de tareas de múltiples pasos.
- Lee y escribe archivos locales directamente, ejecuta comandos de shell y realiza búsquedas web en el escritorio Windows del usuario.

### Documentación de Requisitos

- La documentación de requisitos de este proyecto no es pública.
- Los requisitos fueron validados repetidamente por más de 12 plataformas internacionales de IA y series de modelos grandes, produciendo más de 2000 líneas de documentación de requisitos impulsada por historias de usuario casi incomprensible para humanos.

---

## [No Publicado]

### 2026-08-17

#### Nuevas Funcionalidades
- `c7b575b` - Implementar integración MCP——acceso a herramientas de servidores externos, gestión de configuración y documentación de ayuda
  - Nuevo núcleo MCP (SiliconLife.Core/Mcp/): gestión del ciclo de vida de servidores McpManager, doble transporte stdio/http, encapsulación de conexión McpClientConnection, herramientas de empaquetado por servidor e inyección en todos los Seres de Silicio con nombre `mcp_{serverId}_{toolName}`
  - Nueva página de gestión Web (/mcp) y 7 endpoints API (list-servers/list-tools/add-server/toggle/remove-server/reconnect/test-tool)
  - Nueva herramienta de consulta McpTool (status/list_servers/list_tools, solo lectura); la adición/eliminación de servidores está limitada a usuarios a través de Web UI, la IA no puede modificar la lista de servidores
  - La página de configuración soporta editor de array de servidores MCP (agregar/eliminar en línea dentro de ventana modal)
  - Registrar tema de ayuda MCP (🔌), implementar documentación de ayuda completa en 10 idiomas
  - Las herramientas de empaquetado MCP aparecen en la matriz de permisos con acción `execute`, soporta deshabilitación por Ser de Silicio/proyecto
  - 45 archivos modificados

### 2026-08-16

#### Nuevas Funcionalidades
- `5d76c5a` - Implementar sistema de habilidades——capa de abstracción de reutilización para orquestación de herramientas y plantillas de prompts
  - Nuevo SkillDefinition (id/descripción/schema de parámetros/plantilla de prompt del sistema/lista blanca de herramientas/límite de acciones/rondas máximas/timeout/acción de completado/modo de disparador)
  - Nuevo SkillManager: centro de registro de habilidades + motor de ejecución (bucle secundario de AIRequest, protección de recursión, limitación global de rondas y timeout)
  - Doble modo de disparador: Manual (llamada a función IA, habilidades inyectadas como ToolDefinition, enrutamiento prioritario en lado de programación) + Auto (programación schedule, soporta `HH:mm` / `N s|m|h|d` / subconjunto cron)
  - Almacenamiento con prioridad Markdown (front matter YAML + cuerpo del prompt), Markdown puro con metadatos autocompletados por IA (campos de usuario no sobrescritos)
  - Recarga en caliente (detección de huella cada 30 segundos), archivado de versiones (skills/archive/), 3 habilidades integradas (summarize_document/code_review/research_topic)
  - Nueva herramienta skill (create/list/update/update_from_md/delete/export/export_md/import/import_md)
  - Nueva página de gestión de habilidades (/skill) y 10 endpoints API; cuota MaxCustomSkillsPerBeing (predeterminado 50)
  - Permisos: acción `execute` a nivel de habilidad, lista blanca de herramientas dentro de habilidad y permisos del Ser de Silicio con unión de lado estricto
- `b60fc68` - Actualizar lista de modelos Qianfan y mapeo de ventana de contexto - Nuevos modelos glm-5.2/glm-5.1/deepseek-v4-pro/deepseek-v4-flash/kimi-k2.6/ernie-5.1/qianfan-code-latest, mapeo de ventana de contexto escalonada 1M/128K y capacidades visuales

### 2026-08-15

#### Nuevas Funcionalidades
- `eaa8417` - Implementar asistente de autorización OAuth para plataforma IM y análisis de variables de entorno para claves de configuración
  - Nuevo ImOAuthController/ImOAuthService soportando flujo de autorización OAuth de Feishu (authorize/callback/status), con state anti-CSRF, timeout de 5 minutos, push de estado SSE
  - Nuevo IMProviderRegistry para gestionar uniformemente metadatos de plataforma IM (schema de campos de configuración/plantillas de endpoints OAuth/fábrica de Provider)
  - Nuevo ConfigSecretResolver para analizar marcadores de posición `${ENV_VAR}` en configuración, reemplazo por copia profunda sin escribir de vuelta a la configuración original
  - Página de configuración integrada con asistente de autorización IM UI (área de autorización en línea + estado SSE en tiempo real)
  - Completar traducciones de estado de autorización IM/texto de ayuda para 13 archivos de idioma

### 2026-07-26

#### Refactorización
- `ffc45c2` - Refactorizar plataforma IM a arquitectura de configuración multi-instancia - IMPlatforms como lista (cada plataforma con activación/desactivación independiente), AggregateIMProvider agregando recepción/envío de mensajes multi-plataforma y competición de permisos, editor multi-instancia en página de configuración

### 2026-07-19

#### Nuevas Funcionalidades
- `9bf2103` - Speedy.Manager integración de eliminación de selección múltiple y exportación de selección múltiple en vista de árbol

#### Correcciones
- `0df0674` - Corregir el problema de eliminación de selección múltiple de Speedy.Manager que solo eliminaba el primer elemento

### 2026-07-16

#### Nuevas Funcionalidades
- `7431312` - Completar traducciones de configuración de clientes IA para 13 archivos de idioma - CsCZ/PlPL actualizado de stub a implementación completa de diccionario, los 10 archivos restantes añaden entradas ConfigDisplayNames/ConfigDescriptions/ConfigGroupNames para 7 nuevos clientes (DeepSeek/ZhipuGLM/MoonshotKimi/SiliconFlow/MiniMax/Ernie/Hunyuan), actualización sincronizada de metadatos de clave de configuración de 6 ClientFactory
  - 20 archivo(s) modificado(s)

#### Documentación
- `d6608ea` - Añadir introducción de la herramienta AI IDE DuMate (Baidu Qianfan) a las 13 versiones de idioma del changelog
  - 13 archivo(s) modificado(s)

#### Marco de Colaboración
- `c607c97` - Registrar DuMate (Baidu Qianfan) como actor IA residente en el registro .ai-collab
  - 1 archivo(s) modificado(s)


### 2026-07-15

#### Nuevas Funcionalidades
- `c007263` - Completar documentación de ayuda para 10 clientes IA - HelpTopics registra 10 temas, HelpLocalizationBase añade 30 propiedades abstractas, 12 archivos de idioma implementan contenido de ayuda Markdown completo (introducción de plataforma/pasos de registro/métodos de configuración/modelos disponibles/facturación/preguntas frecuentes), cubriendo Herdsman/LongCat/QiniuAI/DeepSeek/ZhipuGLM/MoonshotKimi/SiliconFlow/MiniMax/Ernie/Hunyuan
  - 12 archivo(s) modificado(s)
- `4634e33` - Implementación de 7 clientes de plataformas IA nacionales (DeepSeek/Zhipu GLM/Moonshot Kimi/SiliconFlow/MiniMax/Baidu Ernie/Tencent Hunyuan) - 14 archivos de clase independientes, siguiendo el estilo LongCatClient, sin herencia, todos compatibles con OpenAI + Bearer Token, soportando Tool Calling/streaming/modo thinking, registrados en DefaultSiliconBeing y DefaultSiliconBeingFactory
  - 16 archivo(s) modificado(s)

#### Documentación
- `108c4ea` - Actualizar toda la documentación en 13 idiomas para reflejar 7 nuevos clientes IA - estado 📋→✅, 01.AI marcado como obsoleto
  - 94 archivo(s) modificado(s)


### 2026-07-14

#### Documentación
- `344b429` - Añadir estado "Obsoleto" al estado de plataforma IA en architecture.md para todos los idiomas, marcar 01.AI como obsoleto (registro de nuevos usuarios detenido)
  - 13 archivo(s) modificado(s)


### 2026-07-07

#### Limpieza
- `e06e6f2` - Eliminar cadena de herramientas OsmStore y plugin TravelCodeWikiWithAI - eliminar tools/OsmStore.* tres proyectos, eliminar src/TravelCodeWikiWithAI/ proyecto de plugin, limpiar referencias sln, el proyecto vuelve a la ruta de desarrollo TCW independiente
  - 45 archivo(s) modificado(s)


### 2026-07-06

#### Correcciones
- `1b15886` - Estandarización del modelo de datos OSM y corrección de seguridad del tipo de elemento
  - 7 archivo(s) modificado(s)


### 2026-07-05

#### Nuevas Funcionalidades
- `be4320b` - TravelCodeWikiWithAI añade módulo de proveedor de datos CLDR
  - 4 archivo(s) modificado(s)


### 2026-07-04

#### Nuevas Funcionalidades
- `dbcabf3` - Mejora del sistema de permisos de plugins - refactorizar IO de red/archivo al modo Executor + exención de lista blanca GeneratedCodeAttribute
  - 34 archivo(s) modificado(s)
- `e84bb63` - Corregir errores de compilación y añadir el proyecto TravelCodeWikiWithAI
  - 53 archivo(s) modificado(s)

#### Refactorización
- `9e5a345` - TravelCodeWikiWithAI migra completamente PBF a la API OSM en línea sincrónica
  - 4 archivo(s) modificado(s)


### 2026-05-31

#### Nuevas Funcionalidades
- `a5f37bd` - Actualizar el pensamiento del proyecto, el sistema de conversación y las funciones de almacenamiento
  - 13 archivo(s) modificado(s)


### 2026-05-30

#### Nuevas Funcionalidades
- `c3cf429` - Añadir cliente IA QiniuAIClient (servicio de inferencia de gran modelo IA Qiniu Cloud) (ref task-409)
  - 20 archivo(s) modificado(s)
- `d04131f` - Añadir cliente IA LongCatClient (gran modelo LongCat Meituan) (ref task-408)
  - 19 archivo(s) modificado(s)

#### Marco de Colaboración
- `e9564f5` - Actualizar todos los archivos modificados
  - 140 archivo(s) modificado(s)
- `9c8b42f` - Archivar sessions y changes del 2026-05-29
  - 20 archivo(s) modificado(s)


### 2026-05-29

#### Nuevas Funcionalidades
- `d548e48` - Página de detalle de pensamiento del proyecto agrupa mensajes por Cycle con secciones plegables (ref task-407)
  - 23 archivo(s) modificado(s)
- `28d893d` - IAIClient añade interfaz de declaración de capacidad multimodal + ChatMessage añade campos multimodales (ref task-402)
  - 13 archivo(s) modificado(s)
- `ebe6a49` - Página de detalle de pensamiento del proyecto añade visualización de estado de sesión, hora de creación y hora de finalización (ref task-406)
  - 22 archivo(s) modificado(s)
- `9a53d55` - IAIClient añade ContextWindowTokens + sistema de presupuesto Token + configuración de fábrica (ref task-401, task-403)
  - 26 archivo(s) modificado(s)
- `202b99c` - Añadir cliente IA HerdsmanClient + corregir que el menú desplegable de la interfaz de inicialización no se actualiza (ref task-399, task-400)
  - 20 archivo(s) modificado(s)
- `285ab2f` - Visualización frontal del registro de procesamiento del proyecto (ref task-397)
  - 25 archivo(s) modificado(s)
- `b4b633f` - Mecanismo de diálogo multi-ronda pseudo-Session de ThinkOnProject (ref task-395)
  - 13 archivo(s) modificado(s)
- `d3e543f` - El contexto de escenario de ThinkOnProject añade información de seres de silicio disponibles (ref task-394)
  - 21 archivo(s) modificado(s)
- `07eb628` - BuildRequest inyecta dinámicamente información de pertenencia de proyecto de seres de silicio (ref task-396)
  - 21 archivo(s) modificado(s)
- `2089696` - Tool añade soporte de escenario Project + refactorización unificada multi-directorio PluginLoader
  - 12 archivo(s) modificado(s)

#### Correcciones
- `b80a33b` - Corregir texto de pista de carga de página de detalle de pensamiento del proyecto codificado en inglés y falta de localización (ref task-405)
  - 6 archivo(s) modificado(s)
- `90b60c5` - Corregir que Content y Thinking del cuerpo de IA se ocultan en las rondas de llamada de herramientas (ref task-404)
  - 8 archivo(s) modificado(s)
- `a7d9a97` - Corregir la continuación de bucle multi-ronda de ThinkOnProject y la pérdida de mensajes de recordatorio de proyecto
  - 6 archivo(s) modificado(s)
- `c0838dd` - Corregir que los mensajes de ProjectThinkSession no se escriben en Cycle y el historial se elimina después de completar (ref task-398)
  - 7 archivo(s) modificado(s)
- `f3d1794` - Corregir la falta de localización del estado Project/Broadcast/Stopped de los seres de silicio y la anomalía de visualización (ref task-393)
  - 20 archivo(s) modificado(s)
- `3eaa90d` - Eliminar referencias de solución al proyecto eliminado TravelCodeWikiWithAI
  - 1 archivo(s) modificado(s)

#### Marco de Colaboración
- `f3cbed7` - Registrar task-394~396 (mejoras de ThinkOnProject)
  - 3 archivo(s) modificado(s)
- `e1971f5` - Registrar task-393 (localización de BeingActivity y corrección de visualización)
  - 1 archivo(s) modificado(s)
- `e710fa4` - Actualizar changes commitHash y fin de sesión de state
  - 2 archivo(s) modificado(s)
- `4cacc4a` - Archivar sessions y changes del 2026-05-28
  - 4 archivo(s) modificado(s)


### 2026-05-28

#### Nuevas Funcionalidades
- `ae8b673` - Configuración de directorio de plugins actualizada de ruta única a lista multi-directorio (ref task-391)
  - 29 archivo(s) modificado(s)
- `aac46c1` - PluginLoader añade modo de fuente CS, compilar-cargar plugins cuando no existe DLL (ref task-389)
  - 6 archivo(s) modificado(s)

#### Correcciones
- `63047b0` - Registrar todos los PluginLoaders en ServiceLocator, corregir reflexión incompleta de plugins multi-directorio (ref task-391)
  - 3 archivo(s) modificado(s)
- `fcad655` - Corregir problema de interacción del botón de exploración de directoryList (ref task-392)
  - 9 archivo(s) modificado(s)

#### Documentación
- `e6d3037` - PluginDemo-22 ejemplo de modo de compilación-carga de código fuente CS (ref task-390)
  - 21 archivo(s) modificado(s)

#### Marco de Colaboración
- `09d9e9c` - Archivar 30 tareas completadas (task-362~task-391)
  - 2 archivo(s) modificado(s)
- `66204a1` - Archivar sessions (8) y changes (8) del 2026-05-28
  - 18 archivo(s) modificado(s)
- `308a8d0` - Actualizar relatedCommit de task-391
  - 1 archivo(s) modificado(s)
- `6fc4e05` - Registrar task-389 (modo fuente CS) y task-390 (PluginDemo-22)
  - 1 archivo(s) modificado(s)


### 2026-05-27

#### Nuevas Funcionalidades
- `e154a18` - Completar ejemplo de flujo de trabajo de negocio completo PluginDemo-21 WorkflowTemplate (ref task-388)
  - 19 archivo(s) modificado(s)
- `aa771b3` - Implementar sistema de permisos declarativo PluginCapability (ref task-379)
  - 9 archivo(s) modificado(s)
- `5e5e9d1` - Añadir ejemplo de tipo seguro de lista blanca 04-SafeSystemIO System.IO (ref task-370)
  - 20 archivo(s) modificado(s)

#### Documentación
- `48f6702` - Alinear traducciones README de 19-TickObject y 20-SpeedyPack en todos los idiomas a la línea base (ref task-386, task-387)
  - 119 archivo(s) modificado(s)
- `5d570e5` - Completar task-378 contraejemplo de omisión de reflexión de cadena prohibida (ref task-378)
  - 19 archivo(s) modificado(s)
- `348c410` - PluginDemo-11 contraejemplo de código P/Invoke y unsafe prohibido (ref task-377)
  - 19 archivo(s) modificado(s)
- `fc92a49` - PluginDemo-10 contraejemplo de operación de reflexión prohibida (ref task-376)
  - 19 archivo(s) modificado(s)
- `826ad2a` - Crear plugin contraejemplo de operación de proceso prohibida PluginDemo-09 (ref task-375)
  - 19 archivo(s) modificado(s)
- `7870b05` - Añadir contraejemplo de operación de red prohibida PluginDemo-08 (ref task-374)
  - 15 archivo(s) modificado(s)
- `8636e31` - PluginDemo-07 contraejemplo de operación de E/S de archivo prohibida (ref task-373)
  - 19 archivo(s) modificado(s)
- `322312e` - Añadir ejemplo de dependencia de confianza PluginDemo-06 TrustedAssemblies (ref task-372)
  - 19 archivo(s) modificado(s)
- `6df98a0` - Añadir ejemplo de plugin de flujo de trabajo IWorkflowPlugin (ref task-371)
  - 20 archivo(s) modificado(s)
- `f3787ba` - PluginDemo-03 ejemplo de registro y creación de IObjectFactory (ref task-369)
  - 20 archivo(s) modificado(s)
- `bb4324d` - PluginDemo-02 ejemplo de registro y consulta de ITypeRegistry (ref task-368)
  - 20 archivo(s) modificado(s)
- `bbdfa3c` - PluginDemo-01 ejemplo de implementación mínima de IPlugin (ref task-367)
  - 19 archivo(s) modificado(s)

#### Marco de Colaboración
- `de44057` - Archivar sessions y changes del 25 y 27 de mayo
  - 58 archivo(s) modificado(s)
- `9e4a84c` - Actualizar lastCommitHash de tasks.json a 48f6702
  - 1 archivo(s) modificado(s)
- `beb58b2` - Completar índice taskIndex (8 pending, 19 completed)
  - 1 archivo(s) modificado(s)
- `63f7bfc` - Actualizar relatedCommit de task-388 (ref task-388)
  - 1 archivo(s) modificado(s)
- `e61be6f` - Actualizar relatedCommit de task-378 (ref task-378)
  - 1 archivo(s) modificado(s)
- `dde579b` - Publicar tarea de ejemplo de uso completo de WorkflowTemplate (task-388)
  - 1 archivo(s) modificado(s)
- `2294fa7` - Publicar tareas de ejemplo TickObject y SpeedyPack (task-386~387)
  - 1 archivo(s) modificado(s)
- `82b9f63` - Publicar 6 tareas de ejemplo PluginCapability (task-380~385)
  - 1 archivo(s) modificado(s)
- `588539b` - Publicar tarea de sistema de permisos declarativo PluginCapability (task-379)
  - 1 archivo(s) modificado(s)
- `37f9c23` - Actualizar referencias de solución y archivo de proyecto
  - 8 archivo(s) modificado(s)
- `e1f7892` - Publicar 12 tareas pendientes PluginDemo (task-367~378)
  - 3 archivo(s) modificado(s)
- `87ae858` - Crear registro de tarea de ejemplo positivo/negativo de plugin PluginDemo (task-367)
  - 2 archivo(s) modificado(s)
- `f77a102` - Archivar sessions y changes del 2026-05-26
  - 7 archivo(s) modificado(s)

## [Alpha-0.2] - 2026-05-16

### 2026-05-16

#### Preparación de Lanzamiento
- `476d839` - Agregar tareas de lanzamiento alpha-0.2
  - Crear task-114 (escritura de CHANGELOG) y task-115 (actualización de número de versión)
  - 1 archivo cambiado

### 2026-05-15

#### Infraestructura
- `672627b` - Agregar workflow de sincronización Gitee (con configuración de permisos)
  - Actualizar configuración de permisos del workflow sync-from-gitee.yml
  - 1 archivo cambiado, 7 líneas añadidas, 4 líneas eliminadas

- `3cd5256` - Agregar GitHub Actions para sincronización automática de código Gitee
  - Nuevo workflow sync-from-gitee.yml
  - 1 archivo cambiado, 50 líneas añadidas

#### Actualización de Documentación
- `aa1d2ad` - Actualizar documentación README/arquitectura/inicio en 11 idiomas, reflejando soporte multiplataforma de SiliconLife.Fast (ref task-112, task-113)
  - Corregir descripción en documentación de que SiliconLife.Fast solo soporta Windows, reflejando soporte multiplataforma real (Windows / macOS / Linux)
  - Actualizar README.md, architecture.md, getting-started.md en 11 idiomas
  - SelectComponent agrega soporte de propiedad hint
  - ConfigView pasa hint a dropdowns de enumeración
  - 11 idiomas de localización agregan clave SelectSearchHint
  - 53 archivos cambiados, 690 líneas añadidas, 194 líneas eliminadas

#### Sistema de Tareas
- `3329f3d` - Agregar mecanismo de inspección del sistema de tareas + tareas de corrección de bugs de localización
  - Crear task-113: corregir problema de localización en página acerca de
  - Actualizar task-112: actualizar documentación de versión Fast para soportar Linux
  - Archivar tareas completadas (11) en .ai-collab/archive/
  - Configuración de mecanismo de inspección completada: inspección rápida (cada 30 minutos) + inspección completa (diario a las 06:00)
  - 2 archivos cambiados, 148 líneas añadidas, 171 líneas eliminadas

#### Marco de Colaboración
- `6038e22` - Registrar coze-agent en registro de colaboración .ai-collab
  - Nueva información de registro de AI residente de plataforma Coze
  - 1 archivo cambiado

### 2026-05-14

#### Marco de Colaboración AI
- `7344fbb` - Eliminar modo handoff, cambiar a impulsado por lista de tareas (v2.0)
  - Refactorizar estructura de directorio .ai-collab, de modo de entrega handoff a impulsado por lista de tareas
  - Nuevo archivo central de lista de tareas tasks.json
  - Nuevo registro de operaciones activity.log
  - Nuevos directorios changes/ y sessions/

- `589a48e` - Agregar registro de sesión .ai-collab
  - Nuevo registro de estado de sesión de colaboración AI

- `5481bcf` - Registrar Qoder AI IDE en registro de colaboración
  - Nueva información de registro de asistente de programación AI Qoder

- `e2d7b61` - Completar relatedCommit y changes commitHash en tasks.json
  - Mejorar asociación de metadatos de tareas

- `a087f0c` - Aceptar todas las tareas task-101~110
  - Confirmar que las 10 correcciones de tareas están completadas

#### Correcciones de Bugs
- `fac9435` - Completar todas las 10 correcciones e implementaciones de tareas task-101~110
  - Corregir componente de selección de búsqueda sin texto de sugerencia
  - Corregir problema de localización en página acerca de
  - Corregir error JS de búsqueda en sistema de ayuda
  - 39 archivos cambiados, 684 líneas añadidas, 121 líneas eliminadas

- `c46dfbc` - Completar todas las tareas pendientes (task-001~006)
  - Completar 6 tareas pendientes iniciales

- `ec176b2` - Sobrescribir lista de tareas - revisión de código descubre 10 nuevos bugs
  - Crear 10 nuevas tareas task-101~110

#### Refactorización
- `ab15915` - Unificar encabezados de derechos de autor + corregir BOM en HelpController y JS de búsqueda en HelpView
  - Unificar encabezados Apache 2.0 en todos los archivos fuente C#
  - Corregir problema de codificación BOM en HelpController
  - Corregir error de JavaScript de búsqueda en HelpView

#### Nuevas Funcionalidades
- `18a6f5d` - Crear servidor de capacidades de navegador MCP (ref task-111)
  - Nuevo proyecto SiliconLife.McpServer
  - Implementar servidor MCP de automatización de navegador Playwright

- `9eb251a` - Eliminar módulo SiliconLife.McpServer (ref task-111)
  - Eliminar servidor MCP independiente, funcionalidad integrada en el proyecto principal

### 2026-05-13

#### Localización
- `7a62590` - Agregar soporte de localización polaco
  - Nueva implementación de localización polaca pl-PL (PlPL.cs, 1089 líneas)
  - Nueva localización de documentación de ayuda polaca (HelpLocalizationPlPL.cs, 3972 líneas)
  - Nuevo soporte de calendario histórico chino en polaco (ChineseHistoricalPlPL.cs, 600 líneas)
  - Nueva localización de bandeja polaca (TrayPlPL.cs, 135 líneas)
  - Nuevo conjunto completo de documentación polaca (15 documentos)
  - Enumeración Language agrega polaco
  - 35 archivos cambiados, 14379 líneas añadidas, 11 líneas eliminadas

- `51f9c8e` - Actualizar referencias Ark AI en documentación y mejoras de terminología
  - Actualizar terminología de clientes AI en documentación multilingüe

- `7587c12` - Agregar entradas de registro de cambios para todos los idiomas
  - Sincronizar actualización de changelog en todas las versiones de idiomas

#### Migración del Sistema de Ventanas
- `b49a07d` - Migrar a modo residente de ventana Avalonia
  - Eliminar dependencia de Windows Forms, migrar completamente al framework Avalonia UI
  - Ventana de estado se muestra correctamente en Linux (verificación por escritorio remoto)
  - Agregar controles de ventana: menú contextual, doble clic para abrir Web, botón de cierre
  - Agregar marco de colaboración AI multiagente (.ai-collab/)
  - Corregir inicialización de icono de bandeja (degradación elegante)
  - Nuevos puntos de entrada de aplicación Avalonia App.axaml y App.cs
  - 13 archivos cambiados, 1442 líneas añadidas, 541 líneas eliminadas

- `d335aaf` - Ventana de plataforma Linux siempre visible + diálogo de confirmación de cierre
  - En Linux, mostrar automáticamente ventana de estado (sin icono de bandeja)
  - En Linux, mostrar diálogo de confirmación al cerrar ventana
  - En Windows/macOS mantener comportamiento de bandeja original
  - Soportar parámetro --no-tray para forzar deshabilitación de bandeja
  - Nuevo método ShowMessageBoxAsync para diálogo de confirmación
  - 3 archivos cambiados, 206 líneas añadidas, 29 líneas eliminadas

#### Refactorización del Sistema de Bandeja
- `841d384` - Refactorizar sistema de bandeja e inicializar marco de colaboración AI
  - Simplificar TrayLocalizationBase eliminando propiedades no utilizadas
  - Agregar elemento de localización ShowStatus
  - App.cs agrega clic en icono de bandeja para mostrar ventana de estado, elementos de menú localizados
  - Program.cs mueve inicialización de icono de bandeja a StartAsync
  - TrayStatusWindow se oculta en lugar de salir al cerrar
  - Registrar trae-glm5 y catpaw en marco de colaboración .ai-collab
  - Actualizar .gitignore asegurando que todos los archivos .ai-collab sean rastreados
  - 22 archivos cambiados, 178 líneas añadidas, 1226 líneas eliminadas

#### Documentación
- `43653bc` - Actualizar descripción del repositorio y registro AI
  - Actualizar README del proyecto e información de registro .ai-collab

### 2026-05-12

#### Vista Web del Sistema de Tareas
- `0891b3c` - Agregar vista de detalles de ejecución de tareas e historial
  - Nueva vista de detalles de ejecución de tareas TaskExecutionDetailView
  - Nueva vista de historial de ejecución de tareas TaskExecutionHistoryView
  - TaskController agrega interfaces de consulta de detalles e historial de ejecución
  - Nuevo modelo de vista de tareas TaskViewModel
  - Mejora del centro de tareas TaskCenter
  - Actualización del sistema de tareas TaskSystem
  - 9 idiomas de localización agregan claves relacionadas con tareas
  - 26 archivos cambiados, 803 líneas añadidas, 55 líneas eliminadas

### 2026-05-11

#### Refactorización de Arquitectura de Componentes Web
- `5e687ad` - Migrar renderizado de componentes de cadenas a H-tree
  - Métodos de renderizado de ComponentBase migrados de modo cadena a estructura H-tree
  - Los 28 componentes adaptados a nueva arquitectura de renderizado (A, Accordion, Button, Calendar, Card, Chart, etc.)
  - SelectComponent refactorizado significativamente (889 líneas mejoradas)
  - Controladores y vistas actualizados sincrónicamente
  - 33 archivos cambiados, 667 líneas añadidas, 435 líneas eliminadas

- `bfd332d` - Migrar Style de cadenas a estilos en línea CssBuilder
  - Nuevo constructor de estilos CssBuilder
  - Sistema de estilos de ComponentBase migrado de cadenas a CssBuilder estructurado
  - LoadingComponent mejorado significativamente (103 líneas añadidas)
  - Migración de estilos de controladores ConfigController, LogController, MemoryController
  - Migración de estilos de vistas ChatView, ConfigView, LogView, MemoryView
  - 37 archivos cambiados, 351 líneas añadidas, 157 líneas eliminadas

#### Optimización del Sistema de Almacenamiento
- `d67a7ee` - Optimizar consulta QueryLatest en conjuntos de datos grandes
  - Optimización de rendimiento del método QueryLatest de SpeedyTimeStorage
  - Mejora del proveedor de registros SpeedyLoggerProvider
  - 2 archivos cambiados, 44 líneas añadidas, 5 líneas eliminadas

#### Refactorización del Sistema de Calendario
- `9629f88` - Extraer TimerExecution y mejorar vista Web de temporizadores
  - TimerSystem extrae lógica TimerExecution (175 líneas eliminadas)
  - SelectComponent mejorado significativamente (427 líneas mejoradas)
  - TimerController y vistas de temporizador mejoradas
  - ContextManager gestor de contexto actualizado
  - 12 archivos cambiados, 458 líneas añadidas, 267 líneas eliminadas

#### Localización
- `5d8ca79` - Agregar clave de localización LogsLoading
  - 9 idiomas agregan clave LogsLoading
  - Nueva definición en clase base DefaultLocalizationBase
  - 11 archivos cambiados, 15 líneas añadidas

### 2026-05-10

#### Refactorización del Sistema de Tareas
- `54394f6` - Fusionar sistema de tareas con ciclos de historial de chat
  - Sistema de tareas de proyecto ProjectTaskSystem simplificado significativamente (411 líneas refactorizadas)
  - Sistema de tareas TaskSystem simplificado (254 líneas refactorizadas)
  - Centro de tareas TaskCenter refactorizado (188 líneas mejoradas)
  - Gestor de contexto ContextManager optimizado (347 líneas refactorizadas)
  - Ser de Silicio DefaultSiliconBeing mejorado
  - Sistema de temporizadores TimerSystem integra tareas
  - Interfaz IWorkNoteStorage actualizada
  - SpeedyWorkNoteStorage y FileSystemWorkNoteStorage adaptados
  - 16 archivos cambiados, 648 líneas añadidas, 897 líneas eliminadas

### 2026-05-09

#### Mejoras de Interfaz Web
- `bc50dd7` - Mejorar vista de chat y agregar funcionalidad de auditoría
  - Nuevo controlador de auditoría AuditController (261 líneas)
  - Nueva vista de auditoría AuditView (379 líneas)
  - Nuevo modelo de vista de auditoría AuditViewModel
  - Vista de chat ChatView mejorada significativamente (171 líneas mejoradas)
  - Controlador de chat ChatController actualizado
  - Componente MarkdownEditorComponent mejorado
  - Controlador de inicialización InitController mejorado
  - Sistema de chat ChatSystem agrega funcionalidades
  - 14 archivos cambiados, 1030 líneas añadidas, 112 líneas eliminadas

- `c9babce` - Mejorar renderizado de llamadas a herramientas en vista de chat
  - Renderizado de bloques de llamadas a herramientas en ChatView mejorado
  - 1 archivo cambiado, 54 líneas añadidas, 11 líneas eliminadas

#### Sistema de Escenarios de Herramientas AI
- `ff2eddd` - Implementar sistema de filtrado de escenarios de herramientas
  - Nuevo atributo de escenario de herramientas ToolScenarioAttribute (36 líneas)
  - Nuevo atributo de escenario solo chat ChatOnlyAttribute (19 líneas)
  - ToolManager gestor de herramientas agrega funcionalidad de filtrado de escenarios (40 líneas)
  - ContextManager gestor de contexto adaptado para filtrado de escenarios
  - 4 archivos cambiados, 115 líneas añadidas, 30 líneas eliminadas

- `5709a33` - Agregar atributos de escenario a clases de herramientas
  - 24 clases de herramientas agregan anotaciones de atributo ToolScenario
  - Incluyendo herramientas de calendario, chat, configuración, curadoría, base de datos, disco, compilación dinámica, etc.
  - 24 archivos cambiados, 46 líneas añadidas, 20 líneas eliminadas

#### Refactorización del Sistema de Tareas
- `2f19a5f` - Refactorizar sistema de tareas con TaskCenter y TaskEnumerator
  - Nuevo centro de tareas TaskCenter (235 líneas)
  - Nuevo enumerador de tareas TaskEnumerator (297 líneas)
  - Sistema de tareas TaskSystem refactorizado y simplificado
  - Ser de Silicio DefaultSiliconBeing adaptado a nueva arquitectura
  - Fábrica DefaultSiliconBeingFactory actualizada
  - Clase base SiliconBeingBase mejorada
  - 7 archivos cambiados, 796 líneas añadidas, 275 líneas eliminadas

#### Migración del Sistema de Permisos
- `a06ed09` - Migrar sistema de IM y permisos al proyecto App
  - PermissionRequestQueue migrado de Default/Fast al proyecto App (443 líneas añadidas)
  - Eliminar WebUIProvider de versión Default (403 líneas eliminadas)
  - Eliminar HelpTool de versión Default (194 líneas eliminadas)
  - Eliminar PermissionRequestQueue duplicado de versiones Default/Fast
  - Eliminar IMPermissionAskHandler de versión Default
  - Controlador PermissionRequestController actualizado
  - 14 archivos cambiados, 496 líneas añadidas, 1183 líneas eliminadas

#### Optimización de Contexto AI
- `4c8aaff` - Optimizar gestor de contexto y mejorar localizador de servicios
  - Gestor de contexto ContextManager simplificado y optimizado
  - Localizador de servicios ServiceLocator mejorado (36 líneas añadidas)
  - Gestor de herramientas ToolManager mejorado (34 líneas añadidas)
  - Clientes DashScopeClient y VolcengineArkClient mejorados
  - Ejecutores (CommandLine, Disk, Network) actualizados
  - 8 archivos cambiados, 116 líneas añadidas, 98 líneas eliminadas

#### Localización
- `5c5eef7` - Agregar claves de localización de auditoría y tareas
  - DefaultLocalizationBase agrega 127 líneas de definiciones de localización
  - 9 idiomas agregan claves relacionadas con auditoría y tareas (26 líneas cada uno)
  - 11 archivos cambiados, 387 líneas añadidas

#### Configuración del Proyecto
- `2067db6` - Actualizar configuración del proyecto y reglas gitignore
  - Reglas .gitignore actualizadas
  - DefaultConfigData y Fast DefaultConfigData mejorados
  - SpeedyWorkNoteStorage almacenamiento mejorado
  - Núcleo de SpeedyPack mejorado
  - 5 archivos cambiados, 32 líneas añadidas, 6 líneas eliminadas

### 2026-05-07

#### Localización Italiana
- `8adc18c` - Agregar soporte de localización italiano y actualizar documentación multilingüe
  - Nueva localización italiana it-IT
  - Nueva implementación de localización ItIT (1909 líneas)
  - Nuevo soporte de calendario histórico chino en italiano ChineseHistoricalItIT (586 líneas)
  - Nueva localización de bandeja italiana TrayItIT (135 líneas)
  - Nuevo conjunto completo de documentación italiana (14 documentos: README, referencia API, arquitectura, sistema de calendario, registro de cambios, guía de contribución, etc.)
  - Actualizar documentación de arquitectura, guía de desarrollo, guía de inicio, etc. en todas las versiones de idiomas
  - Enumeración Language agrega italiano
  - 86 archivos cambiados, 11573 líneas añadidas, 769 líneas eliminadas

#### Sincronización de Documentación
- `12a5deb` - Actualizar documentación multilingüe de arquitectura, registro de cambios y guía de Seres de Silicio
  - Actualización de README en 8 idiomas
  - Actualización de documentación de arquitectura en 8 idiomas
  - Actualización de registro de cambios en 8 idiomas
  - Actualización de guía de Seres de Silicio en 8 idiomas
  - Actualización de referencia de herramientas en 8 idiomas
  - Glosario refactorizado
  - 46 archivos cambiados, 1697 líneas añadidas, 442 líneas eliminadas

### 2026-05-06

#### Refactorización de Módulos a Gran Escala
- `eeb3be6` - Refactorización y reorganización de módulos a gran escala
  - Ajuste de estructura del proyecto SiliconLife.App
  - Reorganización del proyecto SiliconLife.Fast
  - Reorganización del proyecto SiliconLife.Default
  - Reorganización del módulo compartido SiliconLife.Common
  - Reorganización del módulo central SiliconLife.Core
  - Reorganización del motor de almacenamiento SiliconLife.Speedy
  - Reorganización de la herramienta de gestión SiliconLife.Speedy.Manager
  - 119 archivos cambiados, 6926 líneas añadidas, 3066 líneas eliminadas

### 2026-05-04

#### Cliente AI
- `24d2c86` - Agregar VolcengineArkClient y reemplazar Audit con Usage tracking
  - Nuevo cliente AI VolcengineArkClient para Volcengine Ark
  - Soporta modos de streaming y no streaming
  - Control de velocidad de doble capa integrado (auto-control de velocidad + límite de velocidad del servidor)
  - Compatible con protocolo API OpenAI
  - Sistema Audit reemplazado por Usage tracking
  - 24 archivos cambiados, 802 líneas añadidas, 21 líneas eliminadas

#### Sistema de Herramientas
- `f27650a` - Agregar herramienta de recarga en caliente para auto-reinicio de Fast
  - Nueva herramienta de recarga en caliente HotReloadTool
  - Soporta compilación en línea, actualización y reinicio de SiliconLife.Fast
  - Nuevo actualizador independiente HotReload.exe
  - Mecanismo seguro de copia de archivos (no se sobrescribe a sí mismo)
  - Cierre elegante y espera de liberación de puerto
  - 9 archivos cambiados, 581 líneas añadidas

#### Localización
- `6a5aad8` - Actualizar todos los archivos y agregar soporte de localización francés
  - Nueva localización francesa fr-FR
  - Actualizar todas las versiones de idiomas
  - Traducción de documentación de ayuda al francés
  - Traducción de interfaz al francés
  - 100+ archivos cambiados

### 2026-05-03

#### Infraestructura del Proyecto
- `2664b0c` - Actualizar infraestructura del proyecto y dependencias
  - SiliconLife.Speedy.Manager agrega interfaz de gestión WPF (MainForm.Designer.cs, MainForm.resx)
  - Nuevo recurso de icono slc.ico (1.5MB)
  - PluginLoader mejorado significativamente con escaneo de seguridad (622 líneas añadidas)
  - Nueva fábrica de flujos con permisos PermissionedStreamFactory (779 líneas)
  - Nueva cola de solicitudes de permisos PermissionRequestQueue (versiones Default y Fast)
  - Nuevo proveedor de registros de depuración DebugLoggerProvider
  - Clase base de configuración ConfigDataBase mejorada
  - ToolManager agrega funcionalidad de escaneo de herramientas de plugins (ScanAllPluginAssemblies)
  - Mejora de gestión de ciclo de vida de SiliconBeingManager
  - Cliente AI DashScopeClient mejorado significativamente (227 líneas añadidas)
  - Fábrica DefaultSiliconBeingFactory mejorada
  - Vistas y controladores Web actualizados (ChatView, WorkNoteView, PermissionRequestController)
  - 9 idiomas de localización agregan claves
  - 35 archivos cambiados, 28080 líneas añadidas, 336 líneas eliminadas

### 2026-05-02

#### Mejoras del Cliente AI
- `c16f99f` - Actualizar clientes AI, Web UI y componentes de almacenamiento
  - Cliente DashScopeClient mejorado significativamente
  - Auto-compactor SpeedyPackAutoCompactor optimizado
  - Clase base de vista Web y BeingView mejoradas
  - 6 archivos cambiados, 240 líneas añadidas, 81 líneas eliminadas

#### Sistema de Plugins
- `242dc98` - Agregar lista de plugins en página acerca de
  - AboutController agrega visualización de información de plugins
  - AboutViewModel agrega modelo de datos de plugins
  - AboutView agrega renderizado de lista de plugins
  - 9 idiomas de localización agregan claves relacionadas con plugins
  - 14 archivos cambiados, 160 líneas añadidas, 1 línea eliminada

#### Optimización AI
- `147f8f4` - Simplificar texto de indicación de memoria de contexto
  - ContextManager optimiza indicaciones AI
  - 1 archivo cambiado, 1 línea añadida, 1 línea eliminada

#### Optimización de Almacenamiento Speedy
- `8bda2d3` - Actualizar almacenamiento Speedy e implementación del controlador de memoria
  - Corrección de intervalo SpeedyPackAutoCompactor
  - Optimización de procesamiento de rutas SpeedyTimeStorage
  - Controlador de memoria MemoryController mejorado
  - UI de SpeedyPack.Manager actualizada
  - 4 archivos cambiados, 21 líneas añadidas, 18 líneas eliminadas

#### Mejoras de Bandeja
- `8972654` - Mejorar soporte de localización de ventana de estado de bandeja
  - 9 idiomas de localización de bandeja agregan entrada de gestión Speedy
  - TrayStatusWindow agrega elemento de menú de gestión Speedy
  - 11 archivos cambiados, 72 líneas añadidas

#### Optimización de Speedy.Manager
- `6f5db09` - Optimizar UI de SpeedyPack Manager y componentes internos
  - Interfaz MainForm refactorizada
  - Optimización de gestión de memoria FreeList
  - Mejora de cola de escritura WriteQueue
  - Optimización del núcleo SpeedyPack
  - 5 archivos cambiados, 96 líneas añadidas, 88 líneas eliminadas

#### Mejoras del Sistema de Almacenamiento
- `57f9d5d` - Mejorar sistema de almacenamiento, agregar auto-compresión y soporte de fecha incompleta
  - Nuevo temporizador de auto-compresión SpeedyPackAutoCompactor (intervalo de 30 minutos)
  - Gestor singleton SpeedyPackRegistry mejorado
  - SpeedyStorage, SpeedyTimeStorage, SpeedyWorkNoteStorage adaptados y mejorados
  - SpeedyPack agrega gestión de espacio libre FreeList (149 líneas)
  - Escritor PackFileWriter refactorizado y optimizado
  - WriteOperation, WriteQueue cola de escritura mejoradas
  - Opciones de configuración SpeedyPackOptions extendidas
  - IncompleteDate agrega métodos de comparación
  - Cargador de plugins PluginLoader mejorado
  - Flujo de inicialización de Program.cs en versiones Default y Fast actualizado
  - Datos de configuración DefaultConfigData simplificados
  - Red de conocimiento KnowledgeNetwork simplificada
  - Controladores ChatController, MemoryController optimizados
  - MainForm de SpeedyPack.Manager funcionalidad mejorada
  - 22 archivos cambiados, 639 líneas añadidas, 253 líneas eliminadas

#### Actualización de Speedy.Manager
- `b04ed33` - Actualizar archivos de Speedy.Manager

### 2026-05-01

#### Refactorización Arquitectónica: Speedy Reemplaza LiteDB
- `6600972` - Reemplazar LiteDB con almacenamiento Speedy, agregar sistema de plugins y proyecto Speedy
  - **Nuevo proyecto SiliconLife.Speedy**: Motor de almacenamiento .spk de alto rendimiento
    - Clase central SpeedyPack (489 líneas): mapeo de directorios en memoria + caché de entradas + cola de escritura asíncrona
    - Clase de configuración SpeedyPackOptions: TTL de caché, máximo de entradas de caché, modo solo lectura
    - Interfaz de transacciones IPackTransaction: soporta operaciones de escritura atómica
    - Clase de información de archivo SpkFileInfo
    - Directorio Internal: DirectoryMap, EntryCache, PackFileReader, PackFileWriter, WriteQueue, WriteOperation, SpeedyTransaction, SpkHeader, PathNormalizer, FreeList
    - Depende de MessagePack 3.1.4 para serialización binaria (compresión LZ4)
  - **Nuevo proyecto SiliconLife.Speedy.Manager**: Herramienta de gestión WPF
    - Arquitectura MVVM: MainViewModel, DirectoryTreeViewModel, ContentViewerViewModel, etc.
    - Capa de servicios: PackService, FileDialogService, RecentFilesService, NotificationService
    - Convertidores: BoolToVisibility, ByteSizeToString, ContentTypeToIcon, NullToCollapsed
    - Vistas: MainWindow, DirectoryTreeView, ContentViewerPanel, MetadataPanel
    - Diálogos: FileInfoDialog, ImportDialog, NewEntryDialog
  - **Migración de almacenamiento SiliconLife.Fast**: LiteDB → SpeedyPack
    - Nuevo SpeedyStorage (adaptador IStorage)
    - Nuevo SpeedyTimeStorage (adaptador ITimeStorage)
    - Nuevo SpeedyWorkNoteStorage (adaptador IWorkNoteStorage)
    - Nuevo SpeedyPackRegistry (gestión singleton a nivel de proceso)
    - Nuevo SpeedyPackAutoCompactor (temporizador de auto-compresión)
    - Eliminar implementaciones de almacenamiento LiteDB (LiteDBStorage, LiteDBTimeStorage, LiteDBWorkNoteStorage, LiteDBLoggerProvider, LiteDBManager, LiteDBModels)
    - Eliminar código relacionado con ventana de gestión LiteDB
  - **Sistema de Plugins**:
    - Nueva interfaz IPlugin (Core/Plugins/IPlugin.cs)
    - Nuevo cargador de plugins PluginLoader (Core/Plugins/PluginLoader.cs)
    - Soporta carga de DLLs de plugins desde directorios
    - Escaneo de seguridad: verificación de espacios de nombres prohibidos (System.IO, System.Net, Microsoft.CodeAnalysis, etc.)
    - Lista blanca de ensamblados de confianza (Google.Protobuf, Newtonsoft.Json, MessagePack, etc.)
    - Carga aislada con AssemblyLoadContext personalizado
    - ToolManager agrega método ScanAllPluginAssemblies
    - CoreHost integra cargador de plugins
  - 119 archivos cambiados, 6926 líneas añadidas, 3066 líneas eliminadas

#### Mejoras de Seres de Silicio
- `3aef4c3` - Agregar estado de actividad Stopped y mejoras de manejo de errores
  - Ser de Silicio agrega estado Stopped
  - Manejo de errores y mecanismo de recuperación mejorados

#### Actualizaciones de Localización
- `513c65d` - Actualizar todas las versiones de idiomas y documentación
  - Nuevo componente MarkdownEditorComponent (625 líneas)
  - Nuevo componente DetailsComponent (130 líneas)
  - Nuevo componente acordeón AccordionComponent (285 líneas)
  - Controladores BeingController, ChatController, MemoryController, PermissionController actualizados
  - Vistas BeingView, ChatView, MemoryView, SoulEditorView refactorizadas
  - Eliminar antigua MarkdownEditorView
  - Migración de componentes InitController
  - 115 archivos cambiados, 5761 líneas añadidas, 2362 líneas eliminadas

### 2026-04-30

#### Funcionalidad de Bandeja del Sistema
- `101b203` - Implementar ventana de estado de bandeja y ApplicationContext
  - Nuevos recursos de iconos de bandeja (alpha.png, noWord.png, slc.ico, wordIcon.png)
  - Implementar ventana de estado TrayStatusWindow
  - Soportar localización de bandeja en 9 idiomas (TrayCsCZ, TrayDeDE, TrayEnUS, etc.)
  - Clase base abstracta TrayLocalizationBase
  - 24 archivos cambiados, 27995 líneas añadidas, 1 línea eliminada (incluyendo archivos de recursos)

#### Arquitectura UI de Componentes
- `e61cfaa` - Completar arquitectura UI de componentes, implementar 24 componentes
  - Fase MVP (8): ComponentBase, Div, Span, Button, Input, Form, Select, Label
  - Segunda fase (6): Accordion, Card, Tabs, Table, Modal, Message
  - Tercera fase (5): Calendar, Tree, Chart, FileUpload, RichText
  - Nuevas clases auxiliares Js, Behavior, DomUpdate, etc.
  - 25 archivos cambiados, 2666 líneas añadidas

- `7449e51` - Mejorar sistema de componentes y agregar nuevos temas de piel
  - Mejorar componentes A, Button, Div, Form, Input, etc.
  - Agregar 3 temas de piel: HighContrast (alto contraste), Light (claro), Minimal (minimalista)
  - Actualizar pieles existentes (Admin, Chat, Creative, Dev)
  - Migración de componentes InitController
  - 32 archivos cambiados, 1466 líneas añadidas, 1238 líneas eliminadas

- `1ba8636` - Iniciar migración de componentes InitController (en progreso)
  - 9 archivos cambiados, 574 líneas añadidas, 145 líneas eliminadas

#### Unificación del Sistema de Almacenamiento
- `895dff9` - Unificar soul.md y state.json usando interfaz IStorage
  - DefaultSiliconBeing usa IStorage para leer/escribir archivos de alma y estado
  - Nuevo gestor de archivos de estado StateFileManager
  - SoulFileManager refactorizado y adaptado a IStorage
  - 8 archivos cambiados, 201 líneas añadidas, 116 líneas eliminadas

#### Mejoras de Gestión LiteDB
- `a34bef4` - Agregar LiteDBManager y mejorar localización de bandeja
  - Menú de bandeja agrega entrada de gestión LiteDB
  - 9 idiomas de localización de bandeja actualizados
  - 10 archivos cambiados, 196 líneas añadidas

- `c4a79ca` - Agregar fábrica de localización consciente del idioma para ventana de gestión LiteDB
  - 1 archivo cambiado, 78 líneas añadidas

- `5ebc55e` - Convertir LiteDBAdminLocalization en clase base abstracta
  - 10 archivos cambiados, 1356 líneas añadidas

#### Correcciones del Sistema de Configuración
- `2da5256` - Agregar método abstracto ConfigExists y corregir registros de configuración duplicados en LiteDB
  - ConfigDataBase agrega método ConfigExists
  - DefaultConfigData de versión Fast implementa verificación de existencia de configuración LiteDB
  - Corregir problema de claves de configuración duplicadas en LiteDB
  - 9 archivos cambiados, 210 líneas añadidas, 2 líneas eliminadas

#### Optimización de Chat y Vistas
- `d3618ec` - Optimizar sesiones de chat, sistema de almacenamiento, modelo de tiempo y clase base de vistas
  - BroadcastChannel, GroupChatSession, SingleChatSession optimizados
  - ITimeStorage agrega métodos de consulta
  - FileSystemStorage y LiteDBStorage actualizados sincrónicamente
  - ViewBase refactorizado y optimizado (versiones Default y Fast)
  - 11 archivos cambiados, 622 líneas añadidas, 392 líneas eliminadas

### 2026-04-29

#### Refactorización Arquitectónica: Extracción de Módulos Compartidos
- `a102428` - Migrar módulos compartidos de SiliconLife.Default a SiliconLife.Common
  - Extraer 32 implementaciones de calendario al proyecto Common
  - Extraer clase base de localización y 21 implementaciones de idiomas al proyecto Common
  - Extraer gestor de permisos, implementación por defecto de Ser de Silicio al proyecto Common
  - Extraer 23 implementaciones de herramientas integradas al proyecto Common
  - Extraer implementación Playwright WebView al proyecto Common
  - Actualizar espacios de nombres a SiliconLife.Collective
  - 122 archivos cambiados, 586 líneas añadidas, 343 líneas eliminadas

#### Mejoras de Calidad de Código
- `17566fe` - Reemplazar Console.WriteLine con sistema de registros en proyectos Core, Common y Default
  - 6 archivos actualizados incluyendo ContextManager, AuditLogger, DefaultConfigData
  - Unificar uso de interfaz ILogger, mejorar mantenibilidad del código
  - 6 archivos cambiados, 12 líneas añadidas, 8 líneas eliminadas

#### SiliconLife.Fast Versión de Alto Rendimiento
- `54a0307` - Agregar proyecto SiliconLife.Fast y completar correcciones de compilación
  - Punto de entrada completo de aplicación Windows Forms
  - Soporte de bandeja del sistema (NotifyIcon)
  - Migrar todos los controladores Web UI (20+)
  - Migrar todos los componentes de vistas Web
  - Migrar 4 temas de piel (Admin, Chat, Creative, Dev)
  - 125 archivos cambiados, 61186 líneas añadidas

#### Sincronización de Documentación Multilingüe
- `265fde8` - Sincronizar documentación de arquitectura de doble versión a todos los idiomas
  - Actualizar architecture.md, changelog.md en 7 idiomas
  - Actualizar contributing.md en 6 idiomas
  - Actualizar getting-started.md, roadmap.md en 7 idiomas
  - 47 archivos cambiados, 1214 líneas añadidas, 38 líneas eliminadas

#### Sistema de Almacenamiento LiteDB (Versión Fast)
- `4704862` - Agregar dependencia LiteDB e infraestructura
  - Nueva clase de gestión LiteDBManager
  - Nuevos modelos de datos LiteDBModels
  - 3 archivos cambiados, 252 líneas añadidas

- `4220036` - Implementar clases de almacenamiento LiteDB
  - LiteDBStorage: implementa interfaz IStorage
  - LiteDBTimeStorage: implementa interfaz ITimeStorage
  - LiteDBWorkNoteStorage: implementa interfaz IWorkNoteStorage
  - 3 archivos cambiados, 581 líneas añadidas

- `38ebd23` - Migrar configuración y sistema de registros a LiteDB
  - DefaultConfigData adaptado a almacenamiento LiteDB
  - Nuevo proveedor de registros LiteDBLoggerProvider
  - 2 archivos cambiados, 203 líneas añadidas, 67 líneas eliminadas

- `e687157` - Migrar red de conocimiento del sistema de archivos a LiteDB
  - KnowledgeNetwork refactorizado completamente, usando almacenamiento LiteDB para datos de tripletas
  - 1 archivo cambiado, 231 líneas añadidas, 72 líneas eliminadas

- `4220169` - Integrar almacenamiento LiteDB en Program y ProjectManager
  - Program.cs inicializa almacenamiento LiteDB
  - ProjectManager adaptado a almacenamiento de notas de trabajo LiteDB
  - 2 archivos cambiados, 40 líneas añadidas, 17 líneas eliminadas

- `5f3a709` - Eliminar implementaciones de almacenamiento de sistema de archivos obsoletas
  - Eliminar FileSystemLoggerProvider, FileSystemStorage, FileSystemTimeStorage, etc.
  - 6 archivos cambiados, 1518 líneas eliminadas

- `e1a4ef2` - docs: agregar identificador de versión v0.1.0-alpha a toda la documentación
  - 127 archivos cambiados, 2297 líneas añadidas, 2471 líneas eliminadas

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### Refactorización del Sistema de Almacenamiento
- `8dd26e3` - Unificar uso de ITimeStorage con IncompleteDate y agregar API de consulta jerárquica
  - Eliminar sobrecargas DateTime en interfaz ITimeStorage, unificar uso de IncompleteDate
  - IncompleteDate agrega método de comparación CompareTo(DateTime) y método de expansión Expand()
  - Nuevos métodos de consulta jerárquica GetEarliestTimestamp(), GetLatestTimestamp()
  - Nuevos métodos HasSummary() y QueryWithLevel(), soportan consulta por nivel temporal
  - Memory.cs refactoriza algoritmo de compresión, usa nueva API de consulta jerárquica para mejorar eficiencia
  - FileSystemTimeStorage.cs implementa completamente nuevos métodos de interfaz
  - Actualizar sincrónicamente todos los llamadores: ChatSystem, ChatSession, BroadcastChannel, AuditLogger, TokenUsageRecord, etc.
  - Sistema de herramientas actualizado: HelpTool, LogTool, TokenAuditTool adaptados a nueva interfaz
  - Controladores Web actualizados: AuditController, ChatController, ChatHistoryController adaptados a nueva interfaz
  - 41 archivos cambiados, 1820 líneas añadidas, 903 líneas eliminadas

### 2026-04-27

#### Mejoras del Sistema de Documentación de Ayuda
- `9989d79` - Actualizar localización, sistema de ayuda y vistas Web
  - Nueva interfaz de documentación de ayuda de fábrica de clientes AI IAIClientFactoryHelp.cs
  - Completar traducción de toda la documentación de ayuda en 9 idiomas
  - HelpTopics.cs agrega 40 definiciones de temas de ayuda
  - Vistas Web completamente actualizadas: InitController, AuditView, ConfigView, KnowledgeView, LogView, etc.
  - Sistema de localización mejorado: todas las versiones de idiomas agregan nuevas claves de localización
  - Fábricas de clientes AI actualizadas: DashScopeClientFactory, OllamaClientFactory mejoradas
  - 30 archivos cambiados, 10086 líneas añadidas, 15 líneas eliminadas

#### Nuevo Contenido de Documentación de Ayuda
- `e7afe94` - Agregar documentación de ayuda de archivo de alma y registro de auditoría
  - Nueva documentación de ayuda de gestión de archivo de alma
  - Nueva documentación de ayuda de registro de auditoría
  - HelpTopics.cs agrega definiciones de temas
  - HelpView.cs refactorizado significativamente, mejora lógica de renderizado de documentación
  - PermissionView.cs refactorizado, mejora interfaz de gestión de permisos
  - Mejoras de módulos centrales: SiliconBeingManager, TaskSystem, ToolManager mejorados
  - TaskTool.cs refactorizado, mejora funcionalidad de gestión de tareas
  - Vistas Web completamente actualizadas: todos los componentes de vista actualizados sincrónicamente
  - HelpController.cs simplificado, optimiza lógica del controlador
  - 30 archivos cambiados, 7100 líneas añadidas, 897 líneas eliminadas

### 2026-04-26

#### Sistema de Documentación de Ayuda
- `07895d7` - Mejorar sistema de documentación de ayuda, agregar 3 documentos y completar traducción en 9 idiomas
  - Agregar sistema de memoria, guía de instalación y configuración de Ollama, guía de uso de plataforma Bailian de Alibaba Cloud
  - Completar traducción de 10 documentos de ayuda en 9 idiomas
  - Simplificar lógica de renderizado de HelpView
  - 18 archivos cambiados, 14418 líneas añadidas, 1364 líneas eliminadas

#### Localización Alemana
- `0cfd8a1` - Agregar soporte completo de localización alemana (de-DE)
  - Archivo de localización alemana completo
  - Nuevo soporte de calendario histórico chino en alemán
  - Nueva traducción de documentación de ayuda al alemán
  - Sincronización completa de toda la documentación en 9 idiomas
  - 135 archivos cambiados, 26186 líneas añadidas, 14371 líneas eliminadas

#### Sincronización de Documentación
- `3aada7d` - Sincronizar documentación en chino tradicional (zh-HK) con chino simplificado
  - 3 archivos cambiados, 519 líneas añadidas, 422 líneas eliminadas
- `2f6abff` - Agregar localización de nombre de visualización de herramienta de ayuda para todos los idiomas
  - 7 archivos cambiados, 47 líneas añadidas, 7 líneas eliminadas

#### Refactorización del Sistema de Conocimiento
- `60944fe` - Unificar espacio de nombres a SiliconLife.Collective
  - 8 archivos cambiados, 5 líneas añadidas, 8 líneas eliminadas
- `69c51c5` - Agregar sistema de documentación de ayuda y traducir comentarios de código a inglés
  - 29 archivos cambiados, 3385 líneas añadidas, 22 líneas eliminadas

### 2026-04-25

#### Automatización de Navegador WebView
- `41757c3` - Implementar automatización de navegador WebView multiplataforma basada en Playwright
  - 6 archivos cambiados, 1152 líneas añadidas

#### Actualizaciones de Documentación
- `0ff797b` - Agregar documentación de KnowledgeTool y WorkNoteTool (7 idiomas)
  - 28 archivos cambiados, 4983 líneas añadidas
- `ad77415` - Actualizar todos los archivos changelog, agregar registros de historial Git del 2026-04-25
  - 7 archivos cambiados, 168 líneas añadidas

#### Gestión de Espacio de Trabajo de Proyectos
- `785c551` - Implementar gestión de espacio de trabajo de proyectos, incluyendo notas de trabajo y sistema de tareas
  - Nuevo sistema de gestión de espacio de trabajo de proyectos
  - Funcionalidad de notas de trabajo para seguimiento de progreso del proyecto
  - Integración del sistema de gestión de tareas
  - 29 archivos cambiados, 4256 líneas añadidas, 36 líneas eliminadas

#### Localización Checa
- `b4bbf39` - Agregar localización checa completa (cs-CZ) y actualizar documentación de todos los idiomas
  - 116 archivos cambiados, 4933 líneas añadidas, 222 líneas eliminadas
- `faf078f` - Corregir error de compilación de localización checa
  - 3 archivos cambiados, 910 líneas añadidas, 1 línea eliminada

#### Mejoras del Sistema de Conocimiento
- `20adaac` - Agregar KnowledgeTool y soportar localización completa
  - 34 archivos cambiados, 2331 líneas añadidas, 56 líneas eliminadas

### 2026-04-24

#### Mejoras del Sistema de Gestión de Memoria
- `c7b2ecc` - Mejorar funcionalidad de gestión de memoria, agregar filtrado avanzado, estadísticas y vista de detalles
  - Nueva funcionalidad de filtrado avanzado de memoria
  - Implementar funcionalidad de estadísticas de memoria
  - Agregar página de vista de detalles de memoria
  - Soporte de localización multilingüe (6 idiomas)
  - 13 archivos cambiados, 840 líneas añadidas, 86 líneas eliminadas

#### Extensión del Sistema de Permisos
- `4489ad6` - Agregar servicio meteorológico wttr.in a lista blanca de red
  - Actualización sincrónica completa de documentación multilingüe (6 idiomas)
  - 14 archivos cambiados, 417 líneas añadidas, 1 línea eliminada

#### Correcciones de Interfaz Web
- `d9d72e9` - Corregir problema de prioridad CSS en modal de detalles de notas de trabajo
  - 19 archivos cambiados, 1744 líneas añadidas, 6 líneas eliminadas

#### Optimización de Historial de Chat
- `0df599c` - Corregir problema de resultados de herramientas renderizados como mensajes de chat independientes
  - 1 archivo cambiado, 222 líneas añadidas, 21 líneas eliminadas
- `057b09d` - Optimizar visualización de detalles de historial de chat, mejorar renderizado de llamadas a herramientas
  - 3 archivos cambiados, 389 líneas añadidas, 68 líneas eliminadas

#### Historial de Ejecución de Temporizadores
- `fa3f06f` - Agregar funcionalidad de historial de ejecución de temporizadores, incluyendo vista de detalles
  - 8 archivos cambiados, 937 líneas añadidas, 10 líneas eliminadas
- `d824835` - Agregar claves de localización de historial de ejecución de temporizadores (todos los idiomas)
  - 7 archivos cambiados, 88 líneas añadidas

#### Mejoras de Localización
- `c13cb17` - Registrar variante de idioma español
  - 1 archivo cambiado, 4 líneas añadidas
- `9c44f34` - Agregar soporte de localización multilingüe para calendario histórico chino
  - 16 archivos cambiados, 6049 líneas añadidas, 1 línea eliminada

#### Mejoras de Funcionalidad Central
- `1e7c7b2` - Mejorar compresión de memoria y seguimiento de ejecución de herramientas
  - 4 archivos cambiados, 338 líneas añadidas, 86 líneas eliminadas

### 2026-04-23

#### Localización de Herramientas
- `192fc6e` - Agregar localización de nombres de herramientas faltantes para 5 herramientas
  - 6 archivos cambiados, 30 líneas añadidas

#### Actualizaciones de Documentación
- `882c08f` - Actualizar todos los archivos changelog, agregar registros de historial Git completos y eliminar números de versión falsos
  - 45 archivos cambiados, 8815 líneas añadidas, 1611 líneas eliminadas

#### Mejoras de Página de Chat
- `65c157b` - Agregar indicador de carga a página de chat y selección automática de sesión del Curador
  - 10 archivos cambiados, 211 líneas añadidas, 7 líneas eliminadas

#### Funcionalidad de Historial de Chat
- `e483348` - Implementar funcionalidad de visualización de historial de chat de Seres de Silicio
  - Nuevo ChatHistoryController
  - Crear ChatHistoryViewModel
  - Implementar páginas ChatHistoryListView y ChatHistoryDetailView
  - Agregar claves de localización de historial de chat (5 idiomas)
  - 12 archivos cambiados, 1178 líneas añadidas

#### Mejoras de Control de Flujo AI
- `30a2d4e` - Mejorar cancelación de flujo AI, integración IM e inicialización de host central
  - 11 archivos cambiados, 387 líneas añadidas, 12 líneas eliminadas

#### Cola de Mensajes de Chat
- `db48c51` - Agregar cola de mensajes de chat, metadatos de archivos y soporte de cancelación de flujo
  - 4 archivos cambiados, 357 líneas añadidas

#### Soporte de Carga de Archivos
- `28fb344` - Implementar diálogo de origen de archivos y soporte de carga de archivos
  - 3 archivos cambiados, 1100 líneas añadidas, 2 líneas eliminadas
- `1d3e2cc` - Agregar cadenas de localización de diálogo de origen de archivos (6 idiomas)
  - 6 archivos cambiados, 30 líneas añadidas

#### Actualizaciones de Documentación
- `8111e92` - Agregar enlace Wiki en sección de repositorio del README
  - 1 archivo cambiado, 3 líneas añadidas, 1 línea eliminada

### 2026-04-22

#### Localización de Documentación
- `66c11eb` - Traducir comentarios chinos a inglés y actualizar todos los changelog
  - 11 archivos cambiados, 373 líneas añadidas, 163 líneas eliminadas

#### Mejoras de Mensajes SSE
- `b574b2b` - Agregar senderName a mensajes históricos para identificación AI
  - 1 archivo cambiado, 9 líneas añadidas

#### Funcionalidad de Chat
- `601fc14` - Agregar operación mark_read para marca de fin de sesión
  - 7 archivos cambiados, 196 líneas añadidas, 36 líneas eliminadas

#### Optimización del Sistema de Herramientas
- `7a03a19` - Mejorar flexibilidad de consulta de conversación de LogTool
  - 1 archivo cambiado, 57 líneas añadidas, 24 líneas eliminadas

#### Mejoras de Localización
- `0a8d750` - Agregar indicaciones de sistema generales para comportamiento proactivo de Seres de Silicio
  - 8 archivos cambiados, 460 líneas añadidas, 48 líneas eliminadas

#### Refactorización del Sistema de Registros
- `2b771f3` - Desacoplar LogController de E/S de archivos, agregar API de lectura de registros
  - 4 archivos cambiados, 172 líneas añadidas, 137 líneas eliminadas
- `12da302` - Agregar filtro de Ser de Silicio a vista de registros
  - 9 archivos cambiados, 147 líneas añadidas, 10 líneas eliminadas
- `8f6cb1e` - Agregar parámetro beingId a interfaz ILogger, implementar separación de registros sistema/Ser de Silicio
  - 47 archivos cambiados, 524 líneas añadidas, 490 líneas eliminadas

#### Mejoras del Sistema de Permisos
- `4c747ad` - Refactorizar PermissionTool, ExecuteCodeTool, agregar API EvaluatePermission
  - 18 archivos cambiados, 680 líneas añadidas, 492 líneas eliminadas

#### Correcciones de Bugs
- `1c96e99` - Corregir fallo de búsqueda search_files y search_content en directorio raíz
  - 1 archivo cambiado, 98 líneas añadidas, 41 líneas eliminadas

#### Integración de Herramientas
- `135710d` - Eliminar SearchTool, mover búsqueda local a DiskTool
  - 2 archivos cambiados, 185 líneas añadidas, 365 líneas eliminadas

#### Extensión del Sistema de Herramientas
- `70ce7fb` - Implementar DatabaseTool para consultas de base de datos estructurada
  - 1 archivo cambiado, 382 líneas añadidas
- `be29a09` - Implementar LogTool para consulta de historial de operaciones y conversaciones
  - 1 archivo cambiado, 298 líneas añadidas
- `4ea7702` - Implementar PermissionTool para gestión dinámica de permisos
  - 1 archivo cambiado, 457 líneas añadidas
- `1384ff4` - Implementar ExecuteCodeTool para ejecución de código multilenguaje
  - 1 archivo cambiado, 477 líneas añadidas
- `82d1e11` - Implementar SearchTool para recuperación de información
  - 1 archivo cambiado, 363 líneas añadidas

#### Optimización de Interfaz Web
- `0675c45` - Optimizar resaltado de bloques de código markdown en panel de vista previa
  - 1 archivo cambiado, 4 líneas añadidas, 23 líneas eliminadas
- `702b3f3` - Mejorar vista de tareas, agregar insignias de estado y visualización de metadatos
  - 8 archivos cambiados, 221 líneas añadidas, 9 líneas eliminadas
- `6ed9a79` - Mejorar almacenamiento de mensajes de chat y renderizado de vistas
  - 8 archivos cambiados, 140 líneas añadidas, 29 líneas eliminadas

### 2026-04-21

#### Correcciones de Bugs
- `c6b518b` - Corregir paso de mensajes de temporizador y almacenamiento de mensajes de chat
  - 3 archivos cambiados, 297 líneas añadidas, 124 líneas eliminadas

#### Gestión de Configuración
- `4305769` - Agregar .gitattributes para gestión de finales de línea
  - 1 archivo cambiado, 32 líneas añadidas

#### Mejoras de Interfaz Web
- `188c6f8` - Registrar ruta API de lista de tareas y agregar visualización de estado vacío
  - 2 archivos cambiados, 35 líneas añadidas, 2 líneas eliminadas
- `634e8ca` - Agregar enlace de retorno a lista en página de permisos
  - 1 archivo cambiado, 16 líneas añadidas
- `6ba591d` - Agregar editor de configuración AI independiente para Ser de Silicio
  - 11 archivos cambiados, 842 líneas añadidas, 18 líneas eliminadas
- `0a826f5` - Agregar indicación de guardado exitoso en editor de código
  - 1 archivo cambiado, 9 líneas añadidas, 2 líneas eliminadas
- `2940373` - Mejorar interfaz Web, agregar sugerencias flotantes de código y mejoras UI
  - 11 archivos cambiados, 1054 líneas añadidas, 75 líneas eliminadas

#### Correcciones del Sistema de Permisos
- `592c7ab` - Corregir instanciación de retrollamada y orden de registro
  - 2 archivos cambiados, 38 líneas añadidas, 7 líneas eliminadas

#### Mejoras de Seguridad
- `833ead2` - Agregar validación de referencias de ensamblados para compilación dinámica
  - 4 archivos cambiados, 135 líneas añadidas, 8 líneas eliminadas

#### Mejoras del Sistema de Permisos
- `5879621` - Agregar validación de precompilación de retrollamada de permisos y mejorar manejo de errores
  - 21 archivos cambiados, 617 líneas añadidas, 26 líneas eliminadas

#### Actualizaciones de Documentación
- `4dbf659` - Actualizar changelog a v0.5.1, reemplazar URLs placeholder de GitHub, agregar espejo Gitee, localizar nombre de Bilibili por idioma, actualizar email
  - 32 archivos cambiados, 489 líneas añadidas, 180 líneas eliminadas

#### Configuración y Punto de Entrada
- `0fc1693` - Actualizar punto de entrada del programa y configuración del proyecto
  - 2 archivos cambiados, 7 líneas añadidas

#### Refactorización del Sistema de Permisos
- `ea9179a` - Mejorar implementación del sistema de permisos
  - 5 archivos cambiados, 358 líneas añadidas, 152 líneas eliminadas

#### Correcciones de Bugs
- `928a96d` - Corregir implementación de cálculo de calendario
  - 4 archivos cambiados, 12 líneas añadidas, 12 líneas eliminadas

#### AI y Calendario
- `646813e` - Mejorar implementación de fábrica de clientes AI
  - 2 archivos cambiados, 21 líneas añadidas, 20 líneas eliminadas

#### Localización
- `7940d9c` - Agregar soporte de localización coreano
  - 7 archivos cambiados, 2424 líneas añadidas, 10 líneas eliminadas
- `4ff98ad` - Refactorizar documentación, soportar multilingüismo
  - 81 archivos cambiados, 23818 líneas añadidas, 1886 líneas eliminadas

### 2026-04-20

#### Mejoras de Funcionalidad Central
- `28905b5` - Soporte multilingüe completo, fábrica de clientes AI, sistema de permisos y configuración de localización
  - Sistema de registros con gestor, entradas y diferentes niveles de registro
  - Sistema de auditoría de tokens para consulta y seguimiento de uso de tokens
  - Fábrica de clientes AI con descubrimiento automático de diferentes plataformas AI
  - Sistema de retrollamada de permisos con almacenamiento propio
  - Implementación de registrador de consola
  - Soporte multilingüe en inglés y chino simplificado
  - WebUI messenger con WebSocket para chat en tiempo real
  - Ser de Silicio por defecto mejorado con localización
  - 39 archivos cambiados, 4670 líneas añadidas, 175 líneas eliminadas

### 2026-04-19

#### Temporizadores y Calendario
- `c933fd8` - Actualizar localización, sistema de temporizadores, vistas Web y agregar herramientas
  - Mejor gestor de localización
  - Sistema de programación de tareas temporizadas
  - Configuración AI y gestión de contexto
  - Herramienta de calendario soportando 32 tipos de calendario
  - Controlador Web para API de calendario
  - Herramienta de gestión de tareas
  - 46 archivos cambiados, 4018 líneas añadidas, 975 líneas eliminadas

**Mejoras Arquitectónicas**
- Rediseñar arquitectura de vistas Web para mejor soporte de pieles
- Mejorar sistema de gestión de seres con mejor manejo de estados

### 2026-04-18

- `9f585e1` - Actualizar localización, sistema de temporizadores, vistas Web y agregar herramientas
  - Mejoras de temporizadores y programación
  - Mejores vistas Web con componentes UI mejorados
  - Más implementaciones de herramientas
  - 57 archivos cambiados, 3328 líneas añadidas, 389 líneas eliminadas

### 2026-04-17

- `9b71fcd` - Actualizar módulos centrales, agregar documentación zh-HK, canal de difusión, herramienta de configuración y vista Web de auditoría
  - Canal de difusión para múltiples Seres de Silicio chateando juntos
  - Sistema de herramienta de configuración
  - Vista Web de auditoría
  - Documentación en chino tradicional
  - 42 archivos cambiados, 3533 líneas añadidas, 268 líneas eliminadas

### 2026-04-16

- `5040f05` - Actualizar módulos centrales y por defecto
  - Optimización de módulos y correcciones de bugs
  - Actualizaciones de implementación y mejoras
  - 58 archivos cambiados, 9916 líneas añadidas, 111 líneas eliminadas

### 2026-04-15

- `3efab5f` - Actualizar múltiples módulos: AI, Chat, IM, Tools, Web, Localization, Storage
  - Mejoras de clientes AI
  - Mejoras del sistema de chat
  - Actualizaciones de proveedores de mensajería
  - Optimización del sistema de herramientas
  - Mejoras de infraestructura Web
  - Optimización de localización
  - Actualizaciones del sistema de almacenamiento
  - 33 archivos cambiados, 788 líneas añadidas, 232 líneas eliminadas

### 2026-04-14

- `4241a2f` - Funcionalidad de chat básicamente completada, optimización de carga UI
  - Funcionalidad del sistema de chat completada
  - Optimización UI de carga de archivos
  - 16 archivos cambiados, 1234 líneas añadidas, 102 líneas eliminadas

### 2026-04-13

- `c498c31` - Actualizaciones de código
  - Mejoras y optimizaciones generales de código
  - 32 archivos cambiados, 1045 líneas añadidas, 546 líneas eliminadas

### 2026-04-12

#### Documentación y Localización
- `2161002` - Refactorizar documentación y mejorar localización
  - 17 archivos cambiados, 982 líneas añadidas, 92 líneas eliminadas
- `03d94e4` - Mejorar sistema de configuración y localización
  - 25 archivos cambiados, 1378 líneas añadidas, 154 líneas eliminadas
- `9976a35` - Agregar página acerca de y localización
  - 14 archivos cambiados, 699 líneas añadidas, 44 líneas eliminadas

#### Chat y Vistas Web
- `0c8ccfc` - Mejorar sistema de chat, localización y vistas Web
  - 13 archivos cambiados, 402 líneas añadidas, 56 líneas eliminadas
- `a8f1342` - Rediseñar capa de comunicación Web, cambiar de WebSocket a SSE
  - 27 archivos cambiados, 793 líneas añadidas, 935 líneas eliminadas

### 2026-04-11

#### Sistema de Registros
- `e8fe259` - Agregar sistema de registros y optimización de código
  - 37 archivos cambiados, 624 líneas añadidas, 91 líneas eliminadas
- `f01c519` - Agregar sistema de registros, actualizar interfaz AI y vistas Web
  - 31 archivos cambiados, 1758 líneas añadidas, 63 líneas eliminadas

### 2026-04-10

- `4962924` - Mejorar manejador WebSocket, vista de chat e interacción de mensajería
  - Mejoras del gestor de contexto
  - Mejoras del sistema de chat
  - Actualización de interfaz de proveedor de mensajería
  - Rediseño de proveedor WebUI
  - Actualización de constructor JavaScript y enrutador
  - Optimización de vista de chat
  - Mejoras del manejador WebSocket
  - 9 archivos cambiados, 365 líneas añadidas, 134 líneas eliminadas

### 2026-04-09

- `f9302bf` - Mejorar interfaz de proveedor de mensajería, sistema de chat e interacción Web UI
  - Extensión de interfaz de proveedor de mensajería
  - Mejoras de mensajes de chat y sistema
  - Optimización del gestor de contexto
  - Mejora del Ser de Silicio por defecto
  - Mejoras de vista de chat Web UI
  - Actualización del manejador WebSocket
  - 10 archivos cambiados, 427 líneas añadidas, 93 líneas eliminadas

### 2026-04-07

- `6831ee8` - Rediseñar vistas Web y constructor JavaScript
  - Rediseño completo de controladores Web
  - Reescritura completa del constructor JavaScript
  - Actualización de todos los componentes de vista
  - Mejoras del sistema de pieles
  - Mejora de arquitectura de clase base de vista
  - 23 archivos cambiados, 2004 líneas añadidas, 1983 líneas eliminadas

### 2026-04-05

- `41e97fb` - Actualizar múltiples módulos centrales y controladores Web
  - Mejoras del gestor de contexto
  - Sistema de chat y gestión de sesiones
  - Rediseño del localizador de servicios
  - Actualización de clase base y gestor de Seres de Silicio
  - Actualización completa de controladores Web (17 controladores)
  - Mejora de fábrica de Ser de Silicio por defecto
  - 31 archivos cambiados, 681 líneas añadidas, 326 líneas eliminadas
- `67988d4` - Mejorar módulos Web UI, agregar vista de ejecutores, limpiar vistas y módulos centrales
  - 61 archivos cambiados, 3148 líneas añadidas, 3726 líneas eliminadas

### 2026-04-04

- `b58bb1c` - Agregar controlador de inicialización y rediseñar módulos Web
  - Controlador de inicialización
  - Rediseño del módulo de configuración
  - Actualización del módulo de localización
  - Mejoras del sistema de pieles
  - Mejora del enrutador
  - 29 archivos cambiados, 1269 líneas añadidas, 289 líneas eliminadas
- `f03ac0b` - Agregar módulo Web UI, mejorar funcionalidad de mensajería
  - 60 archivos cambiados, 8481 líneas añadidas, 165 líneas eliminadas

### 2026-04-03

- `192e57b` - Actualizar estructura del proyecto y componentes de runtime central
  - 22 archivos cambiados, 446 líneas añadidas, 179 líneas eliminadas
- `59faec8` - Actualizaciones de implementación central y por defecto
  - 25 archivos cambiados, 3056 líneas añadidas, 18 líneas eliminadas
- `d488485` - Agregar funcionalidad de compilación dinámica y módulo de herramientas del Curador
  - 19 archivos cambiados, 1727 líneas añadidas, 11 líneas eliminadas
- `753d1d9` - Agregar módulo de seguridad, actualizar ejecutores, proveedor de mensajería, localización y herramientas
  - 29 archivos cambiados, 2352 líneas añadidas, 93 líneas eliminadas
- `a378697` - Completar fase 5 - Sistema de herramientas + Ejecutores
  - 41 archivos cambiados, 2651 líneas añadidas, 363 líneas eliminadas

### 2026-04-02

- `e6ad94b` - Corregir fallo de carga de historial de chat al eliminar archivo de configuración durante pruebas
  - 4 archivos cambiados, 49 líneas añadidas, 45 líneas eliminadas
- `daa56f5` - Completar fase 4: Memoria persistente (sistema de chat + canal de mensajería)
  - 29 archivos cambiados, 2051 líneas añadidas, 538 líneas eliminadas

### 2026-04-01

- `bbe2dbb` - Corregir carga de configuración y enrutamiento de mensajes del servicio de chat
  - 27 archivos cambiados, 1633 líneas añadidas, 147 líneas eliminadas
- `2fa6305` - Implementar fase 2: Framework de bucle principal y sistema de objetos de reloj
  - 9 archivos cambiados, 594 líneas añadidas, 41 líneas eliminadas
- `32b99a1` - Implementar fase 1 - Funcionalidad de chat básica
  - 19 archivos cambiados, 1185 líneas añadidas
- `358e368` - Commit inicial: documentación del proyecto y licencia
  - 10 archivos cambiados, 1873 líneas añadidas
