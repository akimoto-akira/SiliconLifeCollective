# Registro de Cambios

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | **Español** | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md) | [Русский](../ru-RU/changelog.md)

Todos los cambios importantes de este proyecto se registrarán en este archivo.

El formato se basa en [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
y el proyecto sigue [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## Acerca de este Registro de Cambios

### Versiones Duales del Proyecto

Este proyecto ofrece dos versiones de implementación:

- **SiliconLife.Default**: Implementación predeterminada, utilizada principalmente para validar la viabilidad de la arquitectura. Aplicación de consola, almacenamiento JSON en sistema de archivos.
- **SiliconLife.Fast**: Versión de producción principal. Aplicación de escritorio multiplataforma (Windows / macOS / Linux), almacenamiento en memoria SpeedyPack + persistencia asíncrona (formato de archivo .spk), optimizada profundamente para el rendimiento.

Ambas versiones comparten las mismas interfaces y funcionalidades, difiriendo solo en la implementación del almacenamiento y el modo de ejecución. SiliconLife.Default sirve como referencia para la validación de la arquitectura, mientras que SiliconLife.Fast es la versión principal para entornos de producción.

### Origen del Proyecto

- Este proyecto comenzó el 20 de marzo de 2026.
- Antes de este proyecto, hubo un demo de validación que falló debido a un diseño de arquitectura inadecuado, lo que impidió la integración con múltiples plataformas de IA.

### Herramientas de IDE de IA Utilizadas

#### Kiro (Amazon AWS)
- El proyecto fue inicialmente mantenido por Kiro y lanzado en modo Spec.
- Kiro es un entorno de desarrollo de IA agentic desarrollado por Amazon AWS.
- Basado en Code OSS (VS Code), admite configuraciones de VS Code y extensiones compatibles con Open VSX.
- Cuenta con un flujo de trabajo de desarrollo basado en especificaciones para codificación de IA estructurada.

#### Comate AI IDE / 文心快码 (Baidu)
- Usado ocasionalmente para trabajos de redacción y documentación.
- Comate AI IDE es una herramienta de entorno de desarrollo nativo de IA lanzada por Baidu Wenxin el 23 de junio de 2025.
- El primer IDE de IA multimodal y multiagente de la industria.
- Funcionalidades incluyen conversión de diseño a código y codificación asistida por IA en todo el proceso.
- Impulsado por el modelo Baidu Wenxin 4.0 X1 Turbo.

#### Trae (ByteDance)
- Utilizado desde octubre de 2025 hasta abril de 2026.
- IDE de IA con generación inteligente de código y gestión de proyectos.

#### Qoder (Alibaba)
- Utilizado para mantenimiento del proyecto desde el 18 de abril de 2026.
- Plataforma de codificación de IA que soporta análisis de código, generación de documentación y colaboración multi-agente.

#### CatPaw (Meituan)
- Utilizado en combinación con Qoder desde el 6 de mayo de 2026.
- Basado en los modelos LongCat desarrollados internamente por Meituan, con potentes capacidades de refactorización completa de arquitectura de código.

### Documentación de Requisitos

- La documentación de requisitos de este proyecto no es pública.
- Los requisitos han sido validados repetidamente a través de más de 12 plataformas de IA internacionales y series de modelos grandes, generando más de 2000 líneas de documentación de requisitos impulsada por historias de usuario que son casi incomprensibles para los humanos.

---

## [Sin Publicar]

### 2026-05-22

#### Correcciones de consistencia de documentación
- `9e07b27` - Corregir discrepancias de la documentación francesa (fr-FR) con el código fuente (ref task-307)
  - 10 archivos modificados

- `9e3be72` - Corregir discrepancias de la documentación alemana (de-DE) con el código fuente (ref task-308)
  - 5 archivos modificados

- `2bc7151` - Corregir discrepancias de la documentación española (es-ES) con el código fuente (ref task-309)
  - 13 archivos modificados

- `f95088e` - Corregir discrepancias de la documentación italiana (it-IT) con el código fuente (ref task-310)
  - 11 archivos modificados

- `6ea9f4a` - Corregir discrepancias de la documentación polaca (pl-PL) con el código fuente (ref task-311)
  - 16 archivos modificados

- `7646923` - Corregir discrepancias de la documentación portuguesa (pt-PT) con el código fuente (ref task-312)
  - 12 archivos modificados

- `7eaf9db` - Corregir discrepancias de la documentación checa (cs-CZ) con el código fuente (ref task-313)
  - 12 archivos modificados

#### Framework de colaboración
- `3cb7347` - Actualizar task-313 relatedCommit=7eaf9db
  - 1 archivos modificados

### 2026-05-21

#### Nuevas funcionalidades
- `99eca78` - Añadir 'Ver almacenamiento (solo lectura)' al menú contextual, llamada intra-proceso a Speedy.Manager (ref task-301)
  - 26 archivos modificados

#### Correcciones de consistencia de documentación
- `7f65cf1` - Corregir discrepancias de la documentación zh-CN con el código fuente (ref task-303)
  - 15 archivos modificados

- `a9e2a2c` - Corregir discrepancias de la documentación inglesa (en) con el código fuente (ref task-302)
  - 9 archivos modificados

- `2549105` - Corregir discrepancias de la documentación chino tradicional (zh-HK) con el código fuente (ref task-304)
  - 12 archivos modificados

- `277eb50` - Corregir discrepancias de la documentación japonesa con el código fuente (ref task-305)
  - 10 archivos modificados

- `edce413` - Corregir discrepancias de la documentación coreana (ko-KR) con el código fuente (ref task-306)
  - 18 archivos modificados

- `f2adcae` - Corregir inconsistencias de la documentación portuguesa con el código fuente (ref task-220)
  - 15 archivos modificados

- `3332987` - Corregir inconsistencias de la documentación chino tradicional (Hong Kong) con el código fuente (ref task-218)
  - 14 archivos modificados

- `af9f715` - Corregir inconsistencias de la documentación polaca con el código fuente (ref task-217)
  - 15 archivos modificados

- `2e2b18b` - Corregir inconsistencias de la documentación coreana con el código fuente (ref task-216)
  - 16 archivos modificados

- `626ebc9` - Corregir inconsistencias de la documentación japonesa con el código fuente (ref task-215)
  - 19 archivos modificados

- `48d061b` - Corregir inconsistencias de la documentación italiana con el código fuente (ref task-214)
  - 14 archivos modificados

#### Framework de colaboración
- `6683bee` - Registrar equipo Marvis AI, actualizar estado de tareas
  - 3 archivos modificados

- `03fc905` - Archivar task-210~220
  - 5 archivos modificados

### 2026-05-20

#### Nuevas funcionalidades
- `65176d4` - Añadir soporte completo de localización portuguesa (pt-PT + pt-BR) (ref task-208)
  - 41 archivos modificados

#### Correcciones de consistencia de documentación
- `af4dffd` - Corregir todas las inconsistencias de la documentación zh-CN con el código fuente (ref task-209)
  - 11 archivos modificados

- `144b945` - Corregir inconsistencias de la documentación inglesa (en) y checa (cs-CZ) con el código fuente (ref task-219, task-210)
  - 22 archivos modificados

- `08bec55` - Corregir inconsistencias de la documentación alemana (de-DE) con el código fuente (ref task-211)
  - 14 archivos modificados

- `7ff28de` - Corregir inconsistencias de la documentación española (es-ES) con el código fuente (ref task-212)
  - 14 archivos modificados

- `15e2133` - Corregir inconsistencias de la documentación francesa (fr-FR) con el código fuente (ref task-213)
  - 13 archivos modificados

#### Correcciones de errores
- `7dac388` - Corregir la lista de tareas del proyecto que no se muestra (ref task-207)
  - 6 archivos modificados

#### Framework de colaboración
- `7890223` - Archivar task-201~209, publicar tareas de corrección de consistencia de documentación task-210~220
  - 5 archivos modificados

### 2026-05-19

#### Nuevas funcionalidades
- `cd72846` - Implementar alternativa segura para el bypass del escaneo de seguridad de PluginLoader (ref task-203)
  - 13 archivos modificados

- `fc0c00c` - Mejoras de Speedy.Manager - Crear/Importar/Exportar/Jerarquía TreeView/Ventana de progreso (ref task-206)
  - 9 archivos modificados

#### Correcciones de errores
- `ec07118` - Corregir el problema de ITypeRegistry/IObjectFactory no registrados antes de la carga de plugins (ref task-205)
  - 8 archivos modificados

- `9e749db` - Corregir el error Creator ID is required al crear proyecto (ref task-204)
  - 4 archivos modificados

#### Infraestructura
- `43dc092` - Migración CLDR - añadir CldrDataProvider, eliminar .github
  - 1 archivos modificados

- `c09ec1f` - Añadir cldr/ a .gitignore
  - 1 archivos modificados

- `221f818` - Sincronización GitHub cambiada a esquema de espejo push Gitee, workflow conservado solo como backup manual
  - 1 archivos modificados

- `08cdf1a` - Corregir workflow de sincronización GitHub - añadir lógica de reintento y salto sin cambios
  - 1 archivos modificados

- `fb4e77d` - Actualizar SiliconLife.Speedy.Manager.csproj
  - 1 archivos modificados

#### Framework de colaboración
- `df90af0` - Actualizar task-203 relatedCommit=cd72846
  - 1 archivos modificados

### 2026-05-18

#### Refactorización
- `e720d06` - Refactorizar completamente Speedy.Manager de WinForms a Avalonia (ref task-202)
  - 17 archivos modificados

#### Correcciones de errores
- `08894a9` - Corregir error de visualización del nivel de entradas de resumen de la línea temporal de memoria (ref task-201)
  - 3 archivos modificados

#### Framework de colaboración
- `2871afb` - Archivar todas las tareas, limpiar tasks.json
  - 2 archivos modificados

### 2026-05-17

#### Nuevas funcionalidades
- `d6eb994` - Añadir entrada de creación de proyecto y selección de plantilla de flujo de trabajo a la página de lista de proyectos (ref task-203)
  - 14 archivos modificados

- `0872134` - Orquestación impulsada por curador ThinkOnProject para proyectos sin plantilla (ref task-202)
  - 6 archivos modificados

- `cb3188e` - Visualización de @menciones en chat grupal (ref task-208)
  - 4 archivos modificados

- `f9968e5` - Declaración de capacidad ToolCall del cliente IA y degradación elegante (ref task-205)
  - 4 archivos modificados

- `0d2b843` - Lógica de decisión de chat grupal ShouldReplyInGroupChat (ref task-201)
  - 6 archivos modificados

- `277a2b1` - Completitud de red de conocimiento - consultas avanzadas y traversal de grafos (ref task-207)
  - 9 archivos modificados

#### Correcciones de errores
- `6d0b66e` - Corregir TypeError de appendMessage al enviar mensajes en chat grupal (ref task-209)
  - 5 archivos modificados

- `b15167c` - Envío adicional del registro de ruta list-workflow-templates omitido en task-203 (ref task-203)
  - 1 archivos modificados

- `dc549a2` - Corregir workflow de sincronización Gitee - añadir nombre de usuario a la URL del token
  - 1 archivos modificados

#### Infraestructura
- `e5fa3ad` - Desactivar la sincronización automática de GitHub schedule, esperando solución oficial de Gitee
  - 1 archivos modificados

#### Framework de colaboración
- `4a58c82` - Añadir informe de análisis de capacidades del sistema + propuesta de diseño ThinkOnProject
  - 5 archivos modificados

- `8ab29e6` - Archivar informe de análisis de completitud de capacidades del sistema en .ai-collab/docs
  - 2 archivos modificados

- `b412d9c` - Archivar tareas antiguas, republicar task-201~208 basado en análisis integral
  - 2 archivos modificados

- `437884a` - Actualizar metadatos de colaboración - task-202/203/204 completadas (ref task-202, task-203, task-204)
  - 2 archivos modificados

- `bf78d79` - Actualizar metadatos de colaboración - task-201/205/208 completadas
  - 2 archivos modificados

- `de6ee0e` - Registro de fin de sesión catpaw-20260517-2215
  - 5 archivos modificados

- `7223b6f` - Registro de fin de sesión catpaw-20260517-2200
  - 4 archivos modificados


## [Alpha-0.2] - 2026-05-16

### 2026-05-16

#### Preparación del Lanzamiento
- `476d839` - Tareas de lanzamiento alpha-0.2 añadidas
  - Creadas task-114 (redacción de CHANGELOG) y task-115 (actualización de número de versión)
  - 1 archivo cambiado

### 2026-05-15

#### Infraestructura
- `672627b` - Workflow de sincronización de Gitee añadido (con configuración de permisos)
  - Permisos del workflow sync-from-gitee.yml actualizados
  - 1 archivo cambiado, 7 inserciones(+), 4 eliminaciones(-)

- `3cd5256` - Sincronización automática de Gitee con GitHub Actions añadida
  - Workflow sync-from-gitee.yml añadido
  - 1 archivo cambiado, 50 inserciones(+)

#### Actualizaciones de Documentación
- `aa1d2ad` - Actualizados README/arquitectura/primeros-pasos en los 11 idiomas, reflejando soporte multiplataforma de SiliconLife.Fast (ref task-112, task-113)
  - Corregida documentación que describía SiliconLife.Fast como solo Windows, reflejando soporte multiplataforma real (Windows / macOS / Linux)
  - Actualizados README.md, architecture.md, getting-started.md en 11 idiomas
  - Añadido soporte de propiedad hint a SelectComponent
  - ConfigView ahora pasa parámetro hint a desplegables de enumeración
  - Añadida clave SelectSearchHint a localizaciones de 11 idiomas
  - 53 archivos cambiados, 690 inserciones(+), 194 eliminaciones(-)

#### Sistema de Tareas
- `3329f3d` - Mecanismo de inspección del sistema de tareas + tareas de corrección de bugs de localización añadidas
  - Creada task-113: corregir problema de localización en página de información
  - Actualizada task-112: actualizar documentación de versión Fast para soporte Linux
  - Archivadas tareas completadas (11) en .ai-collab/archive/
  - Mecanismo de inspección configurado: inspección rápida (cada 30 min) + inspección completa (diario 06:00)
  - 2 archivos cambiados, 148 inserciones(+), 171 eliminaciones(-)

#### Marco de Colaboración
- `6038e22` - coze-agent registrado en el registro .ai-collab
  - Añadida información de registro de AI residente de plataforma Coze
  - 1 archivo cambiado

### 2026-05-14

#### Marco de Colaboración AI
- `7344fbb` - Modo handoff eliminado, cambiado a enfoque basado en lista de tareas (v2.0)
  - Estructura de directorio .ai-collab reestructurada de modo handoff a basado en lista de tareas
  - Añadido archivo core de lista de tareas tasks.json
  - Añadido registro de operaciones activity.log
  - Añadidos directorios changes/ y sessions/

- `589a48e` - Registros de sesión .ai-collab añadidos
  - Añadidos registros de estado de sesión de colaboración AI

- `5481bcf` - Qoder AI IDE registrado en registro de colaboración
  - Añadida información de registro de asistente de codificación Qoder AI

- `e2d7b61` - relatedCommit y changes commitHash de tasks.json complementados
  - Asociaciones de metadatos de tareas completadas

- `a087f0c` - Todas las tareas task-101~110 aceptadas
  - Confirmado que las 10 correcciones de tareas están completas

#### Corrección de Bugs
- `fac9435` - Completadas todas las correcciones e implementaciones de task-101~110
  - Corregido texto de sugerencia faltante en componente de selección de búsqueda
  - Corregidos problemas de localización en página de información
  - Corregido error JS de búsqueda en sistema de ayuda
  - 39 archivos cambiados, 684 inserciones(+), 121 eliminaciones(-)

- `c46dfbc` - Completadas todas las tareas pendientes (task-001~006)
  - Completadas 6 tareas pendientes iniciales

- `ec176b2` - Lista de tareas sobrescrita - revisión de código encontró 10 nuevos bugs
  - Creadas task-101~110 (10 nuevas tareas)

#### Refactorización
- `ab15915` - Encabezados de copyright unificados + HelpController BOM y HelpView búsqueda JS corregidos
  - Encabezados de copyright Apache 2.0 unificados en todos los archivos fuente C#
  - Corregido problema de codificación BOM de HelpController
  - Corregido error de JavaScript de búsqueda de HelpView

#### Nuevas Funcionalidades
- `18a6f5d` - Servidor de capacidad de navegador MCP creado (ref task-111)
  - Añadido proyecto SiliconLife.McpServer
  - Implementado servidor MCP de automatización de navegador Playwright

- `9eb251a` - Módulo SiliconLife.McpServer eliminado (ref task-111)
  - Eliminado servidor MCP independiente, funcionalidad integrada en proyecto principal

### 2026-05-13

#### Localización
- `7a62590` - Soporte de localización polaca añadido
  - Añadida implementación de localización polaca pl-PL (PlPL.cs, 1089 líneas)
  - Añadida localización de documentación de ayuda polaca (HelpLocalizationPlPL.cs, 3972 líneas)
  - Añadido soporte de calendario histórico chino polaco (ChineseHistoricalPlPL.cs, 600 líneas)
  - Añadida localización de bandeja polaca (TrayPlPL.cs, 135 líneas)
  - Añadido conjunto completo de documentación polaca (15 documentos)
  - Enumeración Language ampliada con polaco
  - 35 archivos cambiados, 14379 inserciones(+), 11 eliminaciones(-)

- `51f9c8e` - Referencias a Ark AI y mejoras de terminología en documentación actualizadas
  - Terminología de cliente AI actualizada en documentación multilingüe

- `7587c12` - Entradas de registro de cambios añadidas para todos los idiomas
  - Actualizaciones de registro de cambios sincronizadas en todas las versiones de idiomas

#### Migración del Sistema de Ventanas
- `b49a07d` - Migrado al modo residente de ventana Avalonia
  - Eliminada dependencia de Windows Forms, migración completa al framework Avalonia UI
  - Ventana de estado se muestra correctamente en Linux (verificado por escritorio remoto)
  - Controles de ventana añadidos: menú contextual, doble clic para abrir Web, botón de cierre
  - Añadido marco de colaboración multi-AI (.ai-collab/)
  - Corregida inicialización del icono de bandeja (degradación elegante)
  - Añadidos App.axaml y App.cs como puntos de entrada de aplicación Avalonia
  - 13 archivos cambiados, 1442 inserciones(+), 541 eliminaciones(-)

- `d335aaf` - Ventana siempre visible en plataforma Linux + diálogo de confirmación de cierre
  - Linux muestra automáticamente la ventana de estado (sin icono de bandeja)
  - Linux muestra diálogo de confirmación al cerrar la ventana
  - Windows/macOS mantienen comportamiento de bandeja original
  - Soportado parámetro --no-tray para deshabilitar forzadamente la bandeja
  - Añadido método ShowMessageBoxAsync para diálogos de confirmación
  - 3 archivos cambiados, 206 inserciones(+), 29 eliminaciones(-)

#### Refactorización del Sistema de Bandeja
- `841d384` - Sistema de bandeja refactorizado y marco de colaboración AI inicializado
  - TrayLocalizationBase simplificado, propiedades no utilizadas eliminadas
  - Añadido elemento de localización ShowStatus
  - App.cs: clic en icono de bandeja muestra ventana de estado, añadidos elementos de menú localizados
  - Program.cs: inicialización de icono de bandeja movida a StartAsync
  - TrayStatusWindow se oculta en lugar de cerrarse al cerrar
  - Registrados trae-glm5 y catpaw en el marco .ai-collab
  - Actualizado .gitignore para asegurar que todos los archivos .ai-collab sean rastreados
  - 22 archivos cambiados, 178 inserciones(+), 1226 eliminaciones(-)

#### Documentación
- `43653bc` - Descripción del repositorio y registro de AI actualizados
  - README del proyecto e información de registro .ai-collab actualizados

### 2026-05-12

#### Vistas Web del Sistema de Tareas
- `0891b3c` - Agregar vistas de detalle e historial de ejecución de tareas
  - Añadida TaskExecutionDetailView vista de detalle de ejecución de tareas
  - Añadida TaskExecutionHistoryView vista de historial de ejecución de tareas
  - TaskController añadidas interfaces de consulta de detalle e historial de ejecución
  - Añadido TaskViewModel modelo de vista de tareas
  - TaskCenter centro de tareas mejorado
  - TaskSystem sistema de tareas actualizado
  - 9 idiomas de localización añadidas claves relacionadas con tareas
  - 26 archivos cambiados, 803 inserciones(+), 55 eliminaciones(-)

### 2026-05-11

#### Refactorización de Arquitectura de Componentes Web
- `5e687ad` - Migrar renderizado de componentes de cadena a H-tree
  - ComponentBase método de renderizado migrado de patrón de cadena a estructura H-tree
  - Los 28 componentes adaptados a la nueva arquitectura de renderizado (A, Accordion, Button, Calendar, Card, Chart, etc.)
  - SelectComponent gran refactorización (889 líneas mejoradas)
  - Controladores y vistas actualizados en consecuencia
  - 33 archivos cambiados, 667 inserciones(+), 435 eliminaciones(-)

- `bfd332d` - Migrar Style de cadena a estilos en línea CssBuilder
  - Añadido CssBuilder constructor de estilos
  - ComponentBase sistema de estilos migrado de cadena a CssBuilder estructurado
  - LoadingComponent significativamente mejorado (103 líneas añadidas)
  - ConfigController, LogController, MemoryController migración de estilos de controladores
  - ChatView, ConfigView, LogView, MemoryView migración de estilos de vistas
  - 37 archivos cambiados, 351 inserciones(+), 157 eliminaciones(-)

#### Optimización del Sistema de Almacenamiento
- `d67a7ee` - Optimizar QueryLatest para conjuntos de datos grandes
  - SpeedyTimeStorage QueryLatest optimización de rendimiento del método
  - SpeedyLoggerProvider proveedor de registro mejorado
  - 2 archivos cambiados, 44 inserciones(+), 5 eliminaciones(-)

#### Refactorización del Sistema de Calendario
- `9629f88` - Extraer TimerExecution y mejorar vistas web del temporizador
  - TimerSystem lógica TimerExecution extraída (175 líneas eliminadas)
  - SelectComponent significativamente mejorado (427 líneas mejoradas)
  - TimerController y vistas del temporizador mejoradas
  - ContextManager gestor de contexto actualizado
  - 12 archivos cambiados, 458 inserciones(+), 267 eliminaciones(-)

#### Localización
- `5d8ca79` - Agregar clave de localización LogsLoading
  - 9 idiomas añadida clave LogsLoading
  - DefaultLocalizationBase clase base añadida definición
  - 11 archivos cambiados, 15 inserciones(+)

### 2026-05-10

#### Refactorización del Sistema de Tareas
- `54394f6` - Fusionar sistema de tareas con ciclos de historial de chat
  - ProjectTaskSystem sistema de tareas del proyecto significativamente simplificado (411 líneas refactorizadas)
  - TaskSystem sistema de tareas simplificado (254 líneas refactorizadas)
  - TaskCenter centro de tareas refactorizado (188 líneas mejoradas)
  - ContextManager gestor de contexto optimizado (347 líneas refactorizadas)
  - DefaultSiliconBeing ser de silicio mejorado
  - TimerSystem sistema de temporizador integrado con tareas
  - IWorkNoteStorage interfaz actualizada
  - SpeedyWorkNoteStorage y FileSystemWorkNoteStorage adaptados
  - 16 archivos cambiados, 648 inserciones(+), 897 eliminaciones(-)

### 2026-05-09

#### Mejora de la Interfaz Web
- `bc50dd7` - Mejorar vista de chat y agregar funcionalidad de auditoría
  - Añadido AuditController controlador de auditoría (261 líneas)
  - Añadida AuditView vista de auditoría (379 líneas)
  - Añadido AuditViewModel modelo de vista de auditoría
  - ChatView vista de chat significativamente mejorada (171 líneas mejoradas)
  - ChatController controlador de chat actualizado
  - MarkdownEditorComponent componente mejorado
  - InitController controlador de inicialización mejorado
  - ChatSystem sistema de chat funcionalidades añadidas
  - 14 archivos cambiados, 1030 inserciones(+), 112 eliminaciones(-)

- `c9babce` - Mejorar renderizado de llamadas de herramientas en vista de chat
  - ChatView renderizado de bloque de llamada de herramientas mejorado
  - 1 archivo cambiado, 54 inserciones(+), 11 eliminaciones(-)

#### Sistema de Escenarios de Herramientas de IA
- `ff2eddd` - Implementar sistema de filtrado de escenarios de herramientas
  - Añadido ToolScenarioAttribute atributo de escenario de herramientas (36 líneas)
  - Añadido ChatOnlyAttribute atributo de escenario solo chat (19 líneas)
  - ToolManager gestor de herramientas añadido filtrado de escenarios (40 líneas)
  - ContextManager gestor de contexto adaptado para filtrado de escenarios
  - 4 archivos cambiados, 115 inserciones(+), 30 eliminaciones(-)

- `5709a33` - Agregar atributos de escenario a clases de herramientas
  - 24 clases de herramientas añadidas anotaciones de atributo ToolScenario
  - Incluyendo calendario, chat, configuración, curador, base de datos, disco, compilación dinámica, etc.
  - 24 archivos cambiados, 46 inserciones(+), 20 eliminaciones(-)

#### Refactorización del Sistema de Tareas
- `2f19a5f` - Reestructurar sistema de tareas con TaskCenter y TaskEnumerator
  - Añadido TaskCenter centro de tareas (235 líneas)
  - Añadido TaskEnumerator enumerador de tareas (297 líneas)
  - TaskSystem sistema de tareas refactorizado y simplificado
  - DefaultSiliconBeing ser de silicio adaptado a nueva arquitectura
  - DefaultSiliconBeingFactory fábrica actualizada
  - SiliconBeingBase clase base mejorada
  - 7 archivos cambiados, 796 inserciones(+), 275 eliminaciones(-)

#### Migración del Sistema de Permisos
- `a06ed09` - Migrar sistema de IM y permisos al proyecto App
  - PermissionRequestQueue migrado de Default/Fast al proyecto App (443 líneas añadidas)
  - Eliminado WebUIProvider de versión Default (403 líneas eliminadas)
  - Eliminado HelpTool de versión Default (194 líneas eliminadas)
  - Eliminados PermissionRequestQueue duplicados de Default/Fast
  - Eliminado IMPermissionAskHandler de versión Default
  - PermissionRequestController controlador actualizado
  - 14 archivos cambiados, 496 inserciones(+), 1183 eliminaciones(-)

#### Optimización de Contexto de IA
- `4c8aaff` - Optimizar gestor de contexto y mejorar localizador de servicios
  - ContextManager gestor de contexto simplificado y optimizado
  - ServiceLocator localizador de servicios mejorado (36 líneas añadidas)
  - ToolManager gestor de herramientas mejorado (34 líneas añadidas)
  - DashScopeClient y VolcengineArkClient clientes mejorados
  - Ejecutores (CommandLine, Disk, Network) actualizados
  - 8 archivos cambiados, 116 inserciones(+), 98 eliminaciones(-)

#### Localización
- `5c5eef7` - Agregar claves de localización de auditoría y tareas
  - DefaultLocalizationBase añadidas 127 líneas de definiciones de localización
  - 9 idiomas añadidas claves relacionadas con auditoría y tareas (26 líneas cada uno)
  - 11 archivos cambiados, 387 inserciones(+)

#### Configuración del Proyecto
- `2067db6` - Actualizar configuraciones del proyecto y reglas gitignore
  - Reglas .gitignore actualizadas
  - DefaultConfigData y Fast DefaultConfigData configuración mejorada
  - SpeedyWorkNoteStorage almacenamiento mejorado
  - SpeedyPack núcleo mejorado
  - 5 archivos cambiados, 32 inserciones(+), 6 eliminaciones(-)

### 2026-05-07

#### Localización Italiana
- `8adc18c` - Agregar soporte de localización italiana y actualizar documentación multilingüe
  - Añadida localización it-IT italiana
  - Añadida implementación de localización ItIT (1909 líneas)
  - Añadido ChineseHistoricalItIT soporte italiano de calendario histórico chino (586 líneas)
  - Añadida TrayItIT localización italiana de bandeja (135 líneas)
  - Añadido conjunto completo de documentación italiana (14 documentos: README, referencia API, arquitectura, sistema de calendario, registro de cambios, guía de contribución, etc.)
  - Actualizadas arquitectura, guía de desarrollo, guía de inicio, etc. para todas las versiones de idiomas
  - Language enumeración de idiomas añadido italiano
  - 86 archivos cambiados, 11573 inserciones(+), 769 eliminaciones(-)

#### Sincronización de Documentación
- `12a5deb` - Actualizar documentación multilingüe para arquitectura, registro de cambios y guía de ser de silicio
  - 8 idiomas README actualizado
  - 8 idiomas documentación de arquitectura actualizada
  - 8 idiomas registro de cambios actualizado
  - 8 idiomas guía de ser de silicio actualizada
  - 8 idiomas referencia de herramientas actualizada
  - Glosario reestructurado
  - 46 archivos cambiados, 1697 inserciones(+), 442 eliminaciones(-)

### 2026-05-06

#### Refactorización Masiva de Módulos
- `eeb3be6` - Refactorización y reorganización masiva de módulos
  - Reestructuración del proyecto SiliconLife.App
  - Reorganización del proyecto SiliconLife.Fast
  - Reorganización del proyecto SiliconLife.Default
  - Reorganización de módulos compartidos SiliconLife.Common
  - Reorganización de módulos centrales SiliconLife.Core
  - Reorganización del motor de almacenamiento SiliconLife.Speedy
  - Reorganización de herramientas de gestión SiliconLife.Speedy.Manager
  - 119 archivos modificados, 6926 líneas agregadas, 3066 líneas eliminadas

### 2026-05-04

#### Cliente de IA
- `24d2c86` - Agregar VolcengineArkClient y reemplazar Audit por Usage tracking
  - Nuevo cliente de IA VolcengineArkClient Volcengine Ark
  - Soporta modos streaming y no-streaming
  - Control de velocidad doble integrado (control propio + limitación del servidor)
  - Compatible con protocolo API OpenAI
  - Reemplazo del sistema Audit por Usage tracking
  - 24 archivos modificados, 802 líneas agregadas, 21 líneas eliminadas

#### Sistema de Herramientas
- `f27650a` - Agregar herramienta de recarga en caliente para reinicio automático de Fast
  - Nueva herramienta HotReloadTool de recarga en caliente
  - Soporta compilación en línea, actualización y reinicio de SiliconLife.Fast
  - Nuevo programa de actualización independiente HotReload.exe
  - Mecanismo de copia segura de archivos (no se sobrescribe a sí mismo)
  - Cierre elegante y espera de liberación de puerto
  - 9 archivos modificados, 581 líneas agregadas

#### Localización
- `6a5aad8` - Actualizar todos los archivos y agregar soporte de localización francesa
  - Nueva localización fr-FR francesa
  - Actualización de todas las versiones de idiomas
  - Traducción francesa de la documentación de ayuda
  - Traducción francesa de la interfaz
  - 100+ archivos modificados

### 2026-05-03

#### Infraestructura del Proyecto
- `2664b0c` - Actualizar infraestructura del proyecto y dependencias
  - SiliconLife.Speedy.Manager nueva interfaz de gestión WPF agregada (MainForm.Designer.cs, MainForm.resx)
  - Nuevo recurso de icono slc.ico agregado (1.5MB)
  - PluginLoader seguridad de escaneo significativamente mejorada (622 líneas agregadas)
  - Nuevo PermissionedStreamFactory fábrica de flujos con permisos (779 líneas)
  - Nueva PermissionRequestQueue cola de solicitudes de permisos (versiones Default y Fast)
  - Nuevo DebugLoggerProvider proveedor de registro de depuración
  - ConfigDataBase clase base de configuración mejorada
  - ToolManager nueva función de escaneo de herramientas de plugins (ScanAllPluginAssemblies)
  - SiliconBeingManager gestión de ciclo de vida mejorada
  - DashScopeClient cliente de IA Alibaba Cloud significativamente mejorado (227 líneas agregadas)
  - DefaultSiliconBeingFactory fábrica mejorada
  - Vista web y controladores actualizados (ChatView, WorkNoteView, PermissionRequestController)
  - 9 idiomas de localización con nuevos valores clave agregados
  - 35 archivos modificados, 28080 líneas agregadas, 336 líneas eliminadas

### 2026-05-02

#### Mejora de Clientes de IA
- `c16f99f` - Actualizar clientes de IA, Web UI y componentes de almacenamiento
  - DashScopeClient cliente Alibaba Cloud significativamente mejorado
  - SpeedyPackAutoCompactor compactador automático optimizado
  - Clase base de vista web y BeingView mejorados
  - 6 archivos modificados, 240 líneas agregadas, 81 líneas eliminadas

#### Sistema de Plugins
- `242dc98` - Agregar lista de plugins en la página Acerca de
  - AboutController nueva visualización de información de plugins
  - AboutViewModel nuevo modelo de datos de plugins
  - AboutView nuevo renderizado de lista de plugins
  - 9 idiomas de localización con nuevos valores clave relacionados con plugins
  - 14 archivos modificados, 160 líneas agregadas, 1 línea eliminada

#### Optimización de IA
- `147f8f4` - Simplificar texto del prompt de memoria contextual
  - ContextManager optimización del prompt de IA
  - 1 archivo modificado, 1 línea agregada, 1 línea eliminada

#### Optimización de Almacenamiento Speedy
- `8bda2d3` - Actualizar almacenamiento Speedy e implementación del controlador de memoria
  - SpeedyPackAutoCompactor intervalo corregido
  - SpeedyTimeStorage optimización del manejo de rutas
  - MemoryController controlador de memoria mejorado
  - SpeedyPack.Manager UI actualizada
  - 4 archivos modificados, 21 líneas agregadas, 18 líneas eliminadas

#### Mejora de Bandeja
- `8972654` - Mejorar soporte de localización de la ventana de estado de la bandeja
  - 9 idiomas de localización de bandeja con nueva entrada de gestión Speedy
  - TrayStatusWindow nuevo elemento de menú de gestión Speedy
  - 11 archivos modificados, 72 líneas agregadas

#### Optimización de Speedy.Manager
- `6f5db09` - Optimizar UI del gestor SpeedyPack y componentes internos
  - MainForm interfaz refactorizada
  - FreeList gestión de memoria optimizada
  - WriteQueue cola de escritura mejorada
  - SpeedyPack núcleo optimizado
  - 5 archivos modificados, 96 líneas agregadas, 88 líneas eliminadas

#### Mejora del Sistema de Almacenamiento
- `57f9d5d` - Mejorar sistema de almacenamiento, agregar compactación automática y soporte de fecha incompleta
  - Nuevo SpeedyPackAutoCompactor temporizador de compactación automática (intervalo de 30 minutos)
  - SpeedyPackRegistry gestor singleton mejorado
  - SpeedyStorage, SpeedyTimeStorage, SpeedyWorkNoteStorage adaptadores mejorados
  - SpeedyPack nuevo FreeList gestión de espacio libre (149 líneas)
  - PackFileWriter escritor refactorizado y optimizado
  - WriteOperation, WriteQueue cola de escritura mejorada
  - SpeedyPackOptions opciones de configuración ampliadas
  - IncompleteDate nuevos métodos de comparación agregados
  - PluginLoader cargador de plugins mejorado
  - Versiones Default y Fast Program.cs flujo de inicialización actualizado
  - DefaultConfigData datos de configuración simplificados
  - KnowledgeNetwork red de conocimiento simplificada
  - ChatController, MemoryController controladores optimizados
  - SpeedyPack.Manager MainForm funcionalidad mejorada
  - 22 archivos modificados, 639 líneas agregadas, 253 líneas eliminadas

#### Actualización de Speedy.Manager
- `b04ed33` - Actualizar archivos de Speedy.Manager

### 2026-05-01

#### Reconstrucción de Arquitectura: Almacenamiento Speedy Reemplaza LiteDB
- `6600972` - Reemplazar LiteDB con almacenamiento Speedy, agregar sistema de plugins y proyecto Speedy
  - **Nuevo proyecto SiliconLife.Speedy**: Motor de almacenamiento .spk de alto rendimiento
    - Clase central SpeedyPack (489 líneas): mapeo de directorios en memoria + caché de entradas + cola de escritura asíncrona
    - Clase de configuración SpeedyPackOptions: TTL de caché, máximo de entradas en caché, modo de solo lectura
    - Interfaz de transacciones IPackTransaction: soporte para operaciones de escritura atómica
    - Clase de información de archivos SpkFileInfo
    - Directorio Internal: DirectoryMap, EntryCache, PackFileReader, PackFileWriter, WriteQueue, WriteOperation, SpeedyTransaction, SpkHeader, PathNormalizer, FreeList
    - Dependencia de MessagePack 3.1.4 para serialización binaria (compresión LZ4)
  - **Nuevo proyecto SiliconLife.Speedy.Manager**: Herramienta de gestión WPF
    - Arquitectura MVVM: MainViewModel, DirectoryTreeViewModel, ContentViewerViewModel, etc.
    - Capa de servicios: PackService, FileDialogService, RecentFilesService, NotificationService
    - Convertidores: BoolToVisibility, ByteSizeToString, ContentTypeToIcon, NullToCollapsed
    - Vistas: MainWindow, DirectoryTreeView, ContentViewerPanel, MetadataPanel
    - Diálogos: FileInfoDialog, ImportDialog, NewEntryDialog
  - **Migración de almacenamiento de SiliconLife.Fast**: LiteDB → SpeedyPack
    - Nuevo SpeedyStorage (adaptador IStorage)
    - Nuevo SpeedyTimeStorage (adaptador ITimeStorage)
    - Nuevo SpeedyWorkNoteStorage (adaptador IWorkNoteStorage)
    - Nuevo SpeedyPackRegistry (gestión singleton a nivel de proceso)
    - Nuevo SpeedyPackAutoCompactor (temporizador de compactación automática)
    - Eliminadas implementaciones de almacenamiento LiteDB (LiteDBStorage, LiteDBTimeStorage, LiteDBWorkNoteStorage, LiteDBLoggerProvider, LiteDBManager, LiteDBModels)
    - Eliminado código relacionado con la ventana de gestión de LiteDB
  - **Sistema de Plugins**:
    - Nueva interfaz IPlugin (Core/Plugins/IPlugin.cs)
    - Nuevo cargador de plugins PluginLoader (Core/Plugins/PluginLoader.cs)
    - Soporte para carga de DLLs de plugins desde directorio
    - Escaneo de seguridad: verificación de espacios de nombres prohibidos (System.IO, System.Net, Microsoft.CodeAnalysis, etc.)
    - Lista blanca de ensamblados de confianza (Google.Protobuf, Newtonsoft.Json, MessagePack, etc.)
    - Carga aislada con AssemblyLoadContext personalizado
    - ToolManager nuevo método ScanAllPluginAssemblies
    - CoreHost integración del cargador de plugins
  - 119 archivos modificados, 6926 líneas agregadas, 3066 líneas eliminadas

#### Mejora de Seres Silicona
- `3aef4c3` - Agregar estado de actividad Stopped y mejora del manejo de errores
  - Seres Silicona nuevo estado Stopped
  - Manejo de errores y mecanismo de recuperación mejorados

#### Actualización de Localización
- `513c65d` - Actualizar todas las versiones de idiomas y documentación
  - Nuevo componente MarkdownEditorComponent (625 líneas)
  - Nuevo componente DetailsComponent (130 líneas)
  - Nuevo componente AccordionComponent (285 líneas)
  - BeingController, ChatController, MemoryController, PermissionController controladores actualizados
  - BeingView, ChatView, MemoryView, SoulEditorView vistas refactorizadas
  - Eliminado antiguo MarkdownEditorView
  - Migración a componentes de InitController
  - 115 archivos modificados, 5761 líneas agregadas, 2362 líneas eliminadas

### 2026-04-30

#### Función de Bandeja del Sistema
- `101b203` - Implementación de la ventana de estado de la bandeja y ApplicationContext
  - Nuevos recursos de iconos de bandeja agregados (alpha.png, noWord.png, slc.ico, wordIcon.png)
  - Ventana de estado TrayStatusWindow implementada
  - Soporte para localización de bandeja en 9 idiomas (TrayCsCZ, TrayDeDE, TrayEnUS, etc.)
  - Clase base abstracta TrayLocalizationBase
  - 24 archivos modificados, 27995 líneas agregadas, 1 línea eliminada (incluyendo archivos de recursos)

#### Arquitectura de UI Componentizada
- `e61cfaa` - Arquitectura de UI componentizada completada, 24 componentes implementados
  - Fase MVP (8): ComponentBase, Div, Span, Button, Input, Form, Select, Label
  - Segunda fase (6): Accordion, Card, Tabs, Table, Modal, Message
  - Tercera fase (5): Calendar, Tree, Chart, FileUpload, RichText
  - Nuevas clases auxiliares Js, Behavior, DomUpdate agregadas
  - 25 archivos modificados, 2666 líneas agregadas

- `7449e51` - Mejora del sistema de componentes y agregación de nuevos temas de skin
  - Mejora de componentes A, Button, Div, Form, Input, etc.
  - 3 nuevos temas de skin agregados: HighContrast (Alto Contraste), Light (Claro), Minimal (Mínimo)
  - Skins existentes actualizados (Admin, Chat, Creative, Dev)
  - Migración a componentes del InitController
  - 32 archivos modificados, 1466 líneas agregadas, 1238 líneas eliminadas

- `1ba8636` - Iniciar migración a componentes del InitController (en curso)
  - 9 archivos modificados, 574 líneas agregadas, 145 líneas eliminadas

#### Unificación del Sistema de Almacenamiento
- `895dff9` - Unificación de soul.md y state.json usando la interfaz IStorage
  - DefaultSiliconBeing usa IStorage para leer/escribir archivos de alma y estados
  - Nuevo administrador de archivos de estado StateFileManager agregado
  - SoulFileManager refactorizado para adaptarse a IStorage
  - 8 archivos modificados, 201 líneas agregadas, 116 líneas eliminadas

#### Mejora de la Gestión de LiteDB
- `a34bef4` - Agregar LiteDBManager y mejorar la localización de la bandeja
  - Nueva entrada de gestión de LiteDB agregada al menú de bandeja
  - Localización de bandeja actualizada en 9 idiomas
  - 10 archivos modificados, 196 líneas agregadas

- `c4a79ca` - Agregar factoría de localizador sensible al idioma para la ventana de gestión de LiteDB
  - 1 archivo modificado, 78 líneas agregadas

- `5ebc55e` - Convertir LiteDBAdminLocalization a clase base abstracta
  - 10 archivos modificados, 1356 líneas agregadas

#### Corrección del Sistema de Configuración
- `2da5256` - Agregar método abstracto ConfigExists y corregir registros duplicados de configuración en LiteDB
  - Método ConfigExists agregado en ConfigDataBase
  - La versión Fast de DefaultConfigData implementa comprobación de existencia de configuración en LiteDB
  - Corrección del problema de claves duplicadas de configuración en LiteDB
  - 9 archivos modificados, 210 líneas agregadas, 2 líneas eliminadas

#### Optimización de Chat y Vistas
- `d3618ec` - Optimización de sesión de chat, sistema de almacenamiento, modelo de tiempo y clase base de vista
  - BroadcastChannel, GroupChatSession, SingleChatSession optimizados
  - Nuevos métodos de consulta agregados en ITimeStorage
  - FileSystemStorage y LiteDBStorage actualizados sincrónicamente
  - ViewBase refactorizada y optimizada (versiones Default y Fast)
  - 11 archivos modificados, 622 líneas agregadas, 392 líneas eliminadas

### 2026-04-29

#### Reconstrucción de Arquitectura: Extracción de Módulos Compartidos
- `a102428` - Migración de módulos compartidos de SiliconLife.Default a SiliconLife.Common
  - 32 implementaciones de calendario extraídas al proyecto Common
  - Clase base de localización y 21 implementaciones de idiomas extraídas al proyecto Common
  - Administrador de permisos y implementación de SiliconBeing predeterminada extraídas al proyecto Common
  - 23 implementaciones de herramientas integradas extraídas al proyecto Common
  - Implementación de Playwright WebView extraída al proyecto Common
  - Espacio de nombres actualizado a SiliconLife.Collective
  - 122 archivos modificados, 586 líneas agregadas, 343 líneas eliminadas

#### Mejora de Calidad del Código
- `17566fe` - Reemplazo de Console.WriteLine por sistema de registro en proyectos Core, Common y Default
  - 6 archivos actualizados: ContextManager, AuditLogger, DefaultConfigData, etc.
  - Uso unificado de la interfaz ILogger, mejora de la mantenibilidad del código
  - 6 archivos modificados, 12 líneas agregadas, 8 líneas eliminadas

#### Versión de Alto Rendimiento SiliconLife.Fast
- `54a0307` - Agregar proyecto SiliconLife.Fast y completar correcciones de compilación
  - Punto de entrada completo de aplicación Windows Forms
  - Soporte de bandeja del sistema (NotifyIcon)
  - Todos los controladores de Web UI migrados (20+)
  - Todos los componentes de vista web migrados
  - 4 temas de skin migrados (Admin, Chat, Creative, Dev)
  - 125 archivos modificados, 61186 líneas agregadas

#### Sincronización de Documentos Multilingüe
- `265fde8` - Sincronizar documentación de arquitectura de dual versión a todos los idiomas
  - architecture.md, changelog.md actualizados en 7 idiomas
  - contributing.md actualizado en 6 idiomas
  - getting-started.md, roadmap.md actualizados en 7 idiomas
  - 47 archivos modificados, 1214 líneas agregadas, 38 líneas eliminadas

#### Sistema de Almacenamiento LiteDB (Versión Fast)
- `4704862` - Agregar dependencia e infraestructura de LiteDB
  - Nueva clase de gestión LiteDBManager agregada
  - Nuevos modelos de datos LiteDBModels agregados
  - 3 archivos modificados, 252 líneas agregadas

- `4220036` - Implementación de clases de almacenamiento LiteDB
  - LiteDBStorage: Implementación de la interfaz IStorage
  - LiteDBTimeStorage: Implementación de la interfaz ITimeStorage
  - LiteDBWorkNoteStorage: Implementación de la interfaz IWorkNoteStorage
  - 3 archivos modificados, 581 líneas agregadas

- `38ebd23` - Migración de sistema de configuración y registro a LiteDB
  - DefaultConfigData adaptado para almacenamiento LiteDB
  - Nuevo proveedor de registro LiteDBLoggerProvider agregado
  - 2 archivos modificados, 203 líneas agregadas, 67 líneas eliminadas

- `e687157` - Migración de red de conocimiento del sistema de archivos a LiteDB
  - KnowledgeNetwork completamente refactorizado, utiliza LiteDB para almacenar datos de tripletas
  - 1 archivo modificado, 231 líneas agregadas, 72 líneas eliminadas

- `4220169` - Integración de almacenamiento LiteDB en Program y ProjectManager
  - Program.cs inicializa almacenamiento LiteDB
  - ProjectManager adaptado para almacenamiento de notas de trabajo LiteDB
  - 2 archivos modificados, 40 líneas agregadas, 17 líneas eliminadas

- `5f3a709` - Eliminar implementaciones de almacenamiento de sistema de archivos obsoletas
  - FileSystemLoggerProvider, FileSystemStorage, FileSystemTimeStorage, etc. eliminados
  - 6 archivos modificados, 1518 líneas eliminadas

- `e1a4ef2` - docs: agregar identificador de versión v0.1.0-alpha a toda la documentación
  - 127 archivos modificados, 2297 líneas agregadas, 2471 líneas eliminadas

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### Reconstrucción del Sistema de Almacenamiento
- `8dd26e3` - Unificación de la interfaz ITimeStorage con IncompleteDate y agregación de API de consulta jerárquica
  - Eliminación de métodos de sobrecarga DateTime de la interfaz ITimeStorage, uso unificado de IncompleteDate
  - IncompleteDate nuevos métodos CompareTo(DateTime) y Expand() agregados
  - Nueva API de consulta jerárquica: GetEarliestTimestamp(), GetLatestTimestamp()
  - Nuevos métodos HasSummary() y QueryWithLevel() agregados, soporte para consulta por nivel de tiempo
  - Algoritmo de compresión de Memory.cs refactorizado, utiliza nueva API de consulta jerárquica para mejorar la eficiencia
  - FileSystemTimeStorage.cs implementa completamente los nuevos métodos de interfaz
  - Actualización sincrónica de todos los llamadores: ChatSystem, ChatSession, BroadcastChannel, AuditLogger, TokenUsageRecord, etc.
  - Sistema de herramientas actualizado: HelpTool, LogTool, TokenAuditTool adaptados a la nueva interfaz
  - Controladores web actualizados: AuditController, ChatController, ChatHistoryController adaptados a la nueva interfaz
  - 41 archivos modificados, 1820 líneas agregadas, 903 líneas eliminadas

### 2026-04-27

#### Mejora del Sistema de Documentación de Ayuda
- `9989d79` - Actualización de localización, sistema de ayuda y vistas web
  - Nueva interfaz de documentación de ayuda de fábrica de clientes de IA IAIClientFactoryHelp.cs agregada
  - Traducción completa de toda la documentación de ayuda en 9 idiomas
  - HelpTopics.cs 40 nuevas definiciones de temas de ayuda agregadas
  - Vistas web completamente actualizadas: InitController, AuditView, ConfigView, KnowledgeView, LogView, etc.
  - Sistema de localización mejorado: todas las versiones de idiomas agregan nuevas claves de localización
  - Fábrica de clientes de IA actualizada: DashScopeClientFactory, OllamaClientFactory mejorados
  - 30 archivos modificados, 10086 líneas agregadas, 15 líneas eliminadas

#### Nuevos Contenidos de Documentación de Ayuda
- `e7afe94` - Nueva documentación de ayuda de archivo de alma y registro de auditoría agregada
  - Nueva documentación de ayuda de gestión de archivos de alma agregada
  - Nueva documentación de ayuda de registro de auditoría agregada
  - HelpTopics.cs nuevas definiciones de temas agregadas
  - HelpView.cs fuertemente refactorizada, mejora de la lógica de renderizado de documentos
  - PermissionView.cs refactorizada, mejora de la interfaz de gestión de permisos
  - Módulos core mejorados: SiliconBeingManager, TaskSystem, ToolManager mejorados
  - TaskTool.cs refactorizado, mejora de la funcionalidad de gestión de tareas
  - Vistas web completamente actualizadas: todos los componentes de vista sincronizados
  - HelpController.cs simplificado, optimización de la lógica del controlador
  - 30 archivos modificados, 7100 líneas agregadas, 897 líneas eliminadas

### 2026-04-26

#### Sistema de Documentación de Ayuda
- `07895d7` - Mejora del sistema de documentación de ayuda, 3 nuevos documentos agregados y traducción en 9 idiomas completada
  - Nuevas guías de uso de sistema de memoria, instalación y configuración de Ollama, plataforma Alibaba Cloud Tongyi agregadas
  - Traducción completa de todos los 10 documentos de ayuda en 9 idiomas
  - Lógica de renderizado de HelpView simplificada
  - 18 archivos modificados, 14418 líneas agregadas, 1364 líneas eliminadas

#### Localización Alemana
- `0cfd8a1` - Agregar soporte completo de localización alemana (de-DE)
  - Archivos de localización alemanes completos
  - Nuevo soporte para Calendario Histórico Chino en alemán
  - Nuevas traducciones alemanas para documentación de ayuda
  - Sincronización completa de todos los documentos en 9 idiomas
  - 135 archivos modificados, 26186 líneas agregadas, 14371 líneas eliminadas

#### Sincronización de Documentos
- `3aada7d` - Sincronizar documentos chino tradicional (zh-HK) para que coincidan con el chino simplificado
  - 3 archivos modificados, 519 líneas agregadas, 422 líneas eliminadas
- `2f6abff` - Agregar localización de nombres de visualización de herramientas de ayuda para todos los idiomas
  - 7 archivos modificados, 47 líneas agregadas, 7 líneas eliminadas

#### Reconstrucción del Sistema de Conocimiento
- `60944fe` - Unificación del espacio de nombres a SiliconLife.Collective
  - 8 archivos modificados, 5 líneas agregadas, 8 líneas eliminadas
- `69c51c5` - Agregar sistema de documentación de ayuda y traducir comentarios de código al inglés
  - 29 archivos modificados, 3385 líneas agregadas, 22 líneas eliminadas

### 2026-04-25

#### Automatización de Navegador WebView
- `41757c3` - Implementación de automatización de navegador WebView multiplataforma basada en Playwright
  - 6 archivos modificados, 1152 líneas agregadas

#### Actualizaciones de Documentos
- `0ff797b` - Agregar documentación de KnowledgeTool y WorkNoteTool (7 idiomas)
  - 28 archivos modificados, 4983 líneas agregadas
- `ad77415` - Actualizar todos los archivos changelog, agregar historial Git del 25 de abril de 2026
  - 7 archivos modificados, 168 líneas agregadas

#### Gestión de Área de Trabajo del Proyecto
- `785c551` - Implementar gestión de área de trabajo del proyecto, incluyendo notas de trabajo y sistema de tareas
  - Nuevo sistema de gestión de área de trabajo del proyecto agregado
  - Función de notas de trabajo para realizar un seguimiento del progreso del proyecto
  - Integración del sistema de gestión de tareas
  - 29 archivos modificados, 4256 líneas agregadas, 36 líneas eliminadas

#### Localización Checa
- `b4bbf39` - Agregar localización checa completa (cs-CZ) y actualizar documentos de todos los idiomas
  - 116 archivos modificados, 4933 líneas agregadas, 222 líneas eliminadas
- `faf078f` - Corregir errores de compilación de localización checa
  - 3 archivos modificados, 910 líneas agregadas, 1 línea eliminada

#### Mejora del Sistema de Conocimiento
- `20adaac` - Agregar KnowledgeTool con soporte completo de localización
  - 34 archivos modificados, 2331 líneas agregadas, 56 líneas eliminadas

### 2026-04-24

#### Mejora del Sistema de Gestión de Memoria
- `c7b2ecc` - Mejorar la funcionalidad de gestión de memoria, agregar filtrado avanzado, estadísticas y vista de detalles
  - Nueva función de filtrado avanzado de memoria agregada
  - Funcionalidad de estadísticas de memoria implementada
  - Página de vista de detalles de memoria agregada
  - Soporte de localización multilingüe (6 idiomas)
  - 13 archivos modificados, 840 líneas agregadas, 86 líneas eliminadas

#### Extensión del Sistema de Permisos
- `4489ad6` - Agregar servicio de clima wttr.in a la lista blanca de red
  - Actualización sincrónica completa de documentos multilingües (6 idiomas)
  - 14 archivos modificados, 417 líneas agregadas, 1 línea eliminada

#### Corrección de Interfaz Web
- `d9d72e9` - Corregir problema de prioridad CSS en modal de detalles de nota de trabajo
  - 19 archivos modificados, 1744 líneas agregadas, 6 líneas eliminadas

#### Optimización del Historial de Chat
- `0df599c` - Corregir resultados de herramientas renderizados como mensajes de chat independientes
  - 1 archivo modificado, 222 líneas agregadas, 21 líneas eliminadas
- `057b09d` - Optimizar visualización de detalles de historial de chat, mejorar renderizado de llamadas de herramientas
  - 3 archivos modificados, 389 líneas agregadas, 68 líneas eliminadas

#### Historial de Ejecución de Temporizador
- `fa3f06f` - Agregar función de historial de ejecución de temporizador, incluyendo vista de detalles
  - 8 archivos modificados, 937 líneas agregadas, 10 líneas eliminadas
- `d824835` - Agregar claves de localización de historial de ejecución de temporizador (todos los idiomas)
  - 7 archivos modificados, 88 líneas agregadas

#### Mejora de Localización
- `c13cb17` - Registrar variantes de idioma español
  - 1 archivo modificado, 4 líneas agregadas
- `9c44f34` - Agregar soporte de localización multilingüe para Calendario Histórico Chino
  - 16 archivos modificados, 6049 líneas agregadas, 1 línea eliminada

#### Mejora de Funcionalidades Core
- `1e7c7b2` - Mejorar compresión de memoria y seguimiento de ejecución de herramientas
  - 4 archivos modificados, 338 líneas agregadas, 86 líneas eliminadas

### 2026-04-23

#### Localización de Herramientas
- `192fc6e` - Agregar localización faltante de nombres de herramientas para 5 herramientas
  - 6 archivos modificados, 30 líneas agregadas

#### Actualizaciones de Documentos
- `882c08f` - Actualizar todos los archivos changelog, agregar historial Git completo y eliminar números de versión falsos
  - 45 archivos modificados, 8815 líneas agregadas, 1611 líneas eliminadas

#### Mejora de Página de Chat
- `65c157b` - Agregar indicador de carga a la página de chat y seleccionar automáticamente la sesión del moderador
  - 10 archivos modificados, 211 líneas agregadas, 7 líneas eliminadas

#### Función de Historial de Chat
- `e483348` - Implementar función de visualización de historial de chat de SiliconBeing
  - Nuevo ChatHistoryController agregado
  - ChatHistoryViewModel creado
  - Páginas ChatHistoryListView y ChatHistoryDetailView implementadas
  - Claves de localización para historial de chat agregadas (5 idiomas)
  - 12 archivos modificados, 1178 líneas agregadas

#### Mejora del Control de Flujo de IA
- `30a2d4e` - Mejorar cancelación de flujo de IA, integración de IM e inicialización del host core
  - 11 archivos modificados, 387 líneas agregadas, 12 líneas eliminadas

#### Cola de Mensajes de Chat
- `db48c51` - Agregar cola de mensajes de chat, metadatos de archivo y soporte de cancelación de flujo
  - 4 archivos modificados, 357 líneas agregadas

#### Soporte de Carga de Archivos
- `28fb344` - Implementar cuadro de diálogo de fuente de archivo y soporte de carga de archivos
  - 3 archivos modificados, 1100 líneas agregadas, 2 líneas eliminadas
- `1d3e2cc` - Agregar cadenas de localización de cuadro de diálogo de fuente de archivo (6 idiomas)
  - 6 archivos modificados, 30 líneas agregadas

#### Actualizaciones de Documentos
- `8111e92` - Agregar enlace de Wiki en la sección de repositorio del README
  - 1 archivo modificado, 3 líneas agregadas, 1 línea eliminada

### 2026-04-22

#### Localización de Documentos
- `66c11eb` - Traducir comentarios chinos al inglés y actualizar todos los changelog
  - 11 archivos modificados, 373 líneas agregadas, 163 líneas eliminadas

#### Mejora de Mensajes SSE
- `b574b2b` - Agregar senderName para mensajes históricos para reconocimiento de IA
  - 1 archivo modificado, 9 líneas agregadas

#### Función de Chat
- `601fc14` - Agregar operación mark_read para marcado de fin de sesión
  - 7 archivos modificados, 196 líneas agregadas, 36 líneas eliminadas

#### Optimización del Sistema de Herramientas
- `7a03a19` - Mejorar flexibilidad de consulta de diálogo de LogTool
  - 1 archivo modificado, 57 líneas agregadas, 24 líneas eliminadas

#### Mejora de Localización
- `0a8d750` - Agregar prompt de sistema general para comportamientos activos de SiliconBeing
  - 8 archivos modificados, 460 líneas agregadas, 48 líneas eliminadas

#### Reconstrucción del Sistema de Registro
- `2b771f3` - Desacoplar LogController de E/S de archivos, agregar API de lectura de registros
  - 4 archivos modificados, 172 líneas agregadas, 137 líneas eliminadas
- `12da302` - Agregar filtro de SiliconBeing para vista de registros
  - 9 archivos modificados, 147 líneas agregadas, 10 líneas eliminadas
- `8f6cb1e` - Agregar parámetro beingId a la interfaz ILogger, implementar separación de registros de sistema/SiliconBeing
  - 47 archivos modificados, 524 líneas agregadas, 490 líneas eliminadas

#### Mejora del Sistema de Permisos
- `4c747ad` - Reconstruir PermissionTool, ExecuteCodeTool, agregar API EvaluatePermission
  - 18 archivos modificados, 680 líneas agregadas, 492 líneas eliminadas

#### Corrección de Errores
- `1c96e99` - Corregir fallo de búsqueda en directorio raíz de search_files y search_content
  - 1 archivo modificado, 98 líneas agregadas, 41 líneas eliminadas

#### Integración de Herramientas
- `135710d` - Eliminar SearchTool, mover búsqueda local a DiskTool
  - 2 archivos modificados, 185 líneas agregadas, 365 líneas eliminadas

#### Extensión del Sistema de Herramientas
- `70ce7fb` - Implementar DatabaseTool para consultas de base de datos estructuradas
  - 1 archivo modificado, 382 líneas agregadas
- `be29a09` - Implementar LogTool para consultas de historial de operaciones y diálogos
  - 1 archivo modificado, 298 líneas agregadas
- `4ea7702` - Implementar PermissionTool para gestión dinámica de permisos
  - 1 archivo modificado, 457 líneas agregadas
- `1384ff4` - Implementar ExecuteCodeTool para ejecución de código multilingüe
  - 1 archivo modificado, 477 líneas agregadas
- `82d1e11` - Implementar SearchTool para recuperación de información
  - 1 archivo modificado, 363 líneas agregadas

#### Optimización de Interfaz Web
- `0675c45` - Optimizar resaltado de bloques de código markdown en panel de vista previa
  - 1 archivo modificado, 4 líneas agregadas, 23 líneas eliminadas
- `702b3f3` - Mejorar vista de tareas, agregar insignias de estado y visualización de metadatos
  - 8 archivos modificados, 221 líneas agregadas, 9 líneas eliminadas
- `6ed9a79` - Mejorar almacenamiento de mensajes de chat y renderizado de vistas
  - 8 archivos modificados, 140 líneas agregadas, 29 líneas eliminadas

### 2026-04-21

#### Corrección de Errores
- `c6b518b` - Corregir transmisión de mensajes de temporizador y almacenamiento de mensajes de chat
  - 3 archivos modificados, 297 líneas agregadas, 124 líneas eliminadas

#### Gestión de Configuración
- `4305769` - Agregar .gitattributes para gestión de finales de línea
  - 1 archivo modificado, 32 líneas agregadas

#### Mejora de Interfaz Web
- `188c6f8` - Registrar ruta de API de lista de tareas y agregar visualización de estado vacío
  - 2 archivos modificados, 35 líneas agregadas, 2 líneas eliminadas
- `634e8ca` - Agregar enlace de retorno a lista en página de permisos
  - 1 archivo modificado, 16 líneas agregadas
- `6ba591d` - Agregar editor de configuración de IA independiente para SiliconBeing
  - 11 archivos modificados, 842 líneas agregadas, 18 líneas eliminadas
- `0a826f5` - Agregar indicación de guardado exitoso en editor de código
  - 1 archivo modificado, 9 líneas agregadas, 2 líneas eliminadas
- `2940373` - Mejorar interfaz web, agregar sugerencias emergentes de código y mejoras de UI
  - 11 archivos modificados, 1054 líneas agregadas, 75 líneas eliminadas

#### Corrección del Sistema de Permisos
- `592c7ab` - Corregir instanciación de callback y orden de registro
  - 2 archivos modificados, 38 líneas agregadas, 7 líneas eliminadas

#### Mejora de Seguridad
- `833ead2` - Agregar validación de referencias de ensamblado para compilación dinámica
  - 4 archivos modificados, 135 líneas agregadas, 8 líneas eliminadas

#### Mejora del Sistema de Permisos
- `5879621` - Agregar validación de precompilación de callback de permisos y manejo de errores mejorado
  - 21 archivos modificados, 617 líneas agregadas, 26 líneas eliminadas

#### Actualizaciones de Documentos
- `4dbf659` - Actualizar changelog a v0.5.1, reemplazar URL de marcador de posición de GitHub, agregar espejo de Gitee, localizar nombre de Bilibili por idioma, actualizar correo electrónico
  - 32 archivos modificados, 489 líneas agregadas, 180 líneas eliminadas

#### Configuración y Entrada
- `0fc1693` - Actualizar entrada del programa y configuración del proyecto
  - 2 archivos modificados, 7 líneas agregadas

#### Reconstrucción del Sistema de Permisos
- `ea9179a` - Mejorar implementación del sistema de permisos
  - 5 archivos modificados, 358 líneas agregadas, 152 líneas eliminadas

#### Corrección de Errores
- `928a96d` - Corregir implementación de cálculo de calendario
  - 4 archivos modificados, 12 líneas agregadas, 12 líneas eliminadas

#### IA y Calendario
- `646813e` - Mejorar implementación de fábrica de clientes de IA
  - 2 archivos modificados, 21 líneas agregadas, 20 líneas eliminadas

#### Localización
- `7940d9c` - Agregar soporte de localización coreana
  - 7 archivos modificados, 2424 líneas agregadas, 10 líneas eliminadas
- `4ff98ad` - Reconstruir documentos, soporte para multilingüismo
  - 81 archivos modificados, 23818 líneas agregadas, 1886 líneas eliminadas

### 2026-04-20

#### Completitud de Funcionalidades Core
- `28905b5` - Soporte multilingüe completo, fábrica de clientes de IA, sistema de permisos y configuración de localización
  - Sistema de registro con administrador, entradas y diferentes niveles de registro
  - Sistema de auditoría de token para consulta y seguimiento del uso de tokens
  - Fábrica de clientes de IA para descubrimiento automático de diferentes plataformas de IA
  - Sistema de callback de permisos con su propio almacenamiento
  - Implementación de registrador de consola
  - Soporte multilingüe para inglés y chino simplificado
  - Mensajero de WebUI con WebSocket para chat en tiempo real
  - SiliconBeing predeterminado mejorado con localización
  - 39 archivos modificados, 4670 líneas agregadas, 175 líneas eliminadas

### 2026-04-19

#### Temporizador y Calendario
- `c933fd8` - Actualizar localización, sistema de temporizador, vistas web y agregar herramientas
  - Mejor administrador de localización
  - Sistema de programación de tareas programadas
  - Configuración de IA y gestión de contexto
  - Herramienta de calendario con soporte para 32 tipos de calendario
  - Controlador web para API de calendario
  - Herramienta de gestión de tareas
  - 46 archivos modificados, 4018 líneas agregadas, 975 líneas eliminadas

**Mejora de Arquitectura**
- Rediseñar arquitectura de vistas web para mejor soporte de skins
- Mejorar sistema de gestión de beings con mejor manejo de estado

### 2026-04-18

- `9f585e1` - Actualizar localización, sistema de temporizador, vistas web y agregar herramientas
  - Mejoras en temporizador y programación
  - Mejores vistas web con componentes de UI mejorados
  - Más implementaciones de herramientas
  - 57 archivos modificados, 3328 líneas agregadas, 389 líneas eliminadas

### 2026-04-17

- `9b71fcd` - Actualizar módulos core, agregar documentos zh-HK, canal de difusión, herramienta de configuración y vista web de auditoría
  - Canal de difusión para chat conjunto de múltiples SiliconBeings
  - Sistema de herramienta de configuración
  - Vista web de auditoría
  - Documentos chinos tradicionales
  - 42 archivos modificados, 3533 líneas agregadas, 268 líneas eliminadas

### 2026-04-16

- `5040f05` - Actualizar módulos core y predeterminados
  - Optimizaciones de módulos y corrección de errores
  - Actualizaciones e mejoras de implementación
  - 58 archivos modificados, 9916 líneas agregadas, 111 líneas eliminadas

### 2026-04-15

- `3efab5f` - Actualizar múltiples módulos: AI, Chat, IM, Tools, Web, Localization, Storage
  - Mejoras en clientes de IA
  - Mejora del sistema de chat
  - Actualización de proveedor de mensajería
  - Optimización del sistema de herramientas
  - Mejora de infraestructura web
  - Optimización de localización
  - Actualización del sistema de almacenamiento
  - 33 archivos modificados, 788 líneas agregadas, 232 líneas eliminadas

### 2026-04-14

- `4241a2f` - Función de chat básicamente completada, optimización de carga de UI
  - Función de sistema de chat completada
  - Optimización de UI para carga de archivos
  - 16 archivos modificados, 1234 líneas agregadas, 102 líneas eliminadas

### 2026-04-13

- `c498c31` - Actualización de código
  - Mejoras y optimizaciones generales de código
  - 32 archivos modificados, 1045 líneas agregadas, 546 líneas eliminadas

### 2026-04-12

#### Documentación y Localización
- `2161002` - Reconstruir documentos y mejorar localización
  - 17 archivos modificados, 982 líneas agregadas, 92 líneas eliminadas
- `03d94e4` - Mejorar sistema de configuración y localización
  - 25 archivos modificados, 1378 líneas agregadas, 154 líneas eliminadas
- `9976a35` - Agregar página About y localización
  - 14 archivos modificados, 699 líneas agregadas, 44 líneas eliminadas

#### Chat y Vistas Web
- `0c8ccfc` - Mejorar sistema de chat, localización y vistas web
  - 13 archivos modificados, 402 líneas agregadas, 56 líneas eliminadas
- `a8f1342` - Rediseñar capa de comunicación web, cambiar de WebSocket a SSE
  - 27 archivos modificados, 793 líneas agregadas, 935 líneas eliminadas

### 2026-04-11

#### Sistema de Registro
- `e8fe259` - Agregar sistema de registro y optimización de código
  - 37 archivos modificados, 624 líneas agregadas, 91 líneas eliminadas
- `f01c519` - Agregar sistema de registro, actualizar interfaz de IA y vistas web
  - 31 archivos modificados, 1758 líneas agregadas, 63 líneas eliminadas

### 2026-04-10

- `4962924` - Mejorar controlador WebSocket, vista de chat e interacción de mensajería
  - Mejoras en administrador de contexto
  - Mejora del sistema de chat
  - Actualización de interfaz de proveedor de mensajería
  - Rediseño de proveedor de WebUI
  - Actualización de constructor JavaScript y enrutador
  - Optimización de vista de chat
  - Mejoras en controlador WebSocket
  - 9 archivos modificados, 365 líneas agregadas, 134 líneas eliminadas

### 2026-04-09

- `f9302bf` - Mejorar interfaz de proveedor de mensajería, sistema de chat e interacción de Web UI
  - Extensión de interfaz de proveedor de mensajería
  - Mejoras en mensajes y sistema de chat
  - Optimización de administrador de contexto
  - Mejora de SiliconBeing predeterminado
  - Mejoras en vista de chat de Web UI
  - Actualización de controlador WebSocket
  - 10 archivos modificados, 427 líneas agregadas, 93 líneas eliminadas

### 2026-04-07

- `6831ee8` - Rediseñar vistas web y constructor JavaScript
  - Rediseño completo de controladores web
  - Constructor JavaScript completamente reescrito
  - Todas las componentes de vista actualizadas
  - Mejora del sistema de skins
  - Mejora de arquitectura de clase base de vista
  - 23 archivos modificados, 2004 líneas agregadas, 1983 líneas eliminadas

### 2026-04-05

- `41e97fb` - Actualizar múltiples módulos core y controladores web
  - Mejoras en administrador de contexto
  - Sistema de chat y gestión de sesiones
  - Rediseño de localizador de servicios
  - Actualización de clase base y administrador de SiliconBeing
  - Actualización completa de controladores web (17 controladores)
  - Mejora de fábrica de SiliconBeing predeterminado
  - 31 archivos modificados, 681 líneas agregadas, 326 líneas eliminadas
- `67988d4` - Mejorar módulo Web UI, agregar vista de ejecutor, limpiar vistas y módulos core
  - 61 archivos modificados, 3148 líneas agregadas, 3726 líneas eliminadas

### 2026-04-04

- `b58bb1c` - Agregar controlador de inicialización y rediseñar módulo web
  - Controlador de inicialización
  - Rediseño de módulo de configuración
  - Actualización de módulo de localización
  - Mejora del sistema de skins
  - Mejora del enrutador
  - 29 archivos modificados, 1269 líneas agregadas, 289 líneas eliminadas
- `f03ac0b` - Agregar módulo Web UI, mejorar funcionalidades de mensajería
  - 60 archivos modificados, 8481 líneas agregadas, 165 líneas eliminadas

### 2026-04-03

- `192e57b` - Actualizar estructura del proyecto y componentes de tiempo de ejecución core
  - 22 archivos modificados, 446 líneas agregadas, 179 líneas eliminadas
- `59faec8` - Actualización de implementación core y predeterminada
  - 25 archivos modificados, 3056 líneas agregadas, 18 líneas eliminadas
- `d488485` - Agregar función de compilación dinámica y módulo de herramientas de moderador
  - 19 archivos modificados, 1727 líneas agregadas, 11 líneas eliminadas
- `753d1d9` - Agregar módulo de seguridad, actualizar ejecutor, proveedor de mensajería, localización y herramientas
  - 29 archivos modificados, 2352 líneas agregadas, 93 líneas eliminadas
- `a378697` - Completar Fase 5 - Sistema de Herramientas + Ejecutor
  - 41 archivos modificados, 2651 líneas agregadas, 363 líneas eliminadas

### 2026-04-02

- `e6ad94b` - Corregir error de carga de historial de chat al eliminar archivo de configuración durante pruebas
  - 4 archivos modificados, 49 líneas agregadas, 45 líneas eliminadas
- `daa56f5` - Completar Fase 4: Memoria Persistente (Sistema de Chat + Canal de Mensajería)
  - 29 archivos modificados, 2051 líneas agregadas, 538 líneas eliminadas

### 2026-04-01

- `bbe2dbb` - Corregir carga de configuración y enrutamiento de mensajes de servicio de chat
  - 27 archivos modificados, 1633 líneas agregadas, 147 líneas eliminadas
- `2fa6305` - Implementar Fase 2: Marco de bucle principal y sistema de objetos de reloj
  - 9 archivos modificados, 594 líneas agregadas, 41 líneas eliminadas
- `32b99a1` - Implementar Fase 1 - Función de chat básica
  - 19 archivos modificados, 1185 líneas agregadas
- `358e368` - Commit inicial: Documentación del proyecto y licencia
  - 10 archivos modificados, 1873 líneas agregadas