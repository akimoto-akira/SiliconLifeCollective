# Registro de Cambios

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | **Español** | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md)

Todos los cambios importantes de este proyecto serán documentados en este archivo.

El formato se basa en [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
y este proyecto sigue [Versionado Semántico](https://semver.org/spec/v2.0.0.html).

---

## Acerca de este Registro de Cambios

### Origen del Proyecto

- Este proyecto se originó el 20 de marzo de 2026.
- Antes de este proyecto, hubo un demo de verificación que falló debido a un diseño de arquitectura irracional, lo que impidió la integración con múltiples plataformas de IA.

### Herramientas AI IDE Utilizadas

#### Kiro (Amazon AWS)
- El proyecto fue inicialmente mantenido por Kiro y comenzado usando el modo Spec.
- Kiro es un entorno de desarrollo AI agentic construido por Amazon AWS.
- Basado en Code OSS (VS Code), soporta configuraciones de VS Code y extensiones compatibles con Open VSX.
- Tiene un flujo de trabajo de desarrollo impulsado por especificaciones para codificación AI estructurada.

#### Comate AI IDE / 文心快码 (Baidu)
- Ocasionalmente usado para trabajo de redacción y documentación.
- Comate AI IDE es una herramienta de entorno de desarrollo AI nativo lanzada por Baidu Wenxin el 23 de junio de 2025.
- El primer AI IDE de la industria con colaboración multimodal y multi-agente.
- Las funciones incluyen conversión de diseño a código y asistencia de codificación AI en todo el flujo de trabajo.
- Impulsado por el modelo Baidu Wenxin 4.0 X1 Turbo.

#### Trae (ByteDance)
- La mayor parte del tiempo, este proyecto fue mantenido principalmente usando Trae.
- Trae es un AI IDE desarrollado por SPRING PTE, subsidiaria de ByteDance en Singapur.
- Como 10x AI Engineer, capaz de construir soluciones de software de forma independiente.
- Tiene herramientas de productividad inteligente, adaptación flexible al ritmo de desarrollo y funciones de entrega colaborativa de proyectos.
- Proporciona rendimiento de nivel empresarial con sistema de agentes configurables.

#### Qoder (Alibaba)
- Desde el 18 de abril de 2026, este proyecto ha sido mantenido usando Qoder.
- Qoder sobresale en análisis de código fuente y generación de documentación de dominio, con excelente comprensión de bases de código complejas.
- Adopta un modelo de precios de costo computacional cero, lo que lo hace altamente rentable para procesamiento automatizado de documentación y tareas rutinarias.
- Una plataforma de codificación agentic impulsada por AI, diseñada para desarrollo de software real.
- Tiene generación inteligente de código, programación conversacional, motor avanzado de análisis de contexto y colaboración multi-agente.
- Proporciona comprensión profunda de código con consumo mínimo de recursos, ideal para mantenimiento de proyectos a largo plazo y acumulación de conocimiento.

### Documentos de Requisitos

- Los documentos de requisitos de este proyecto no son públicos.
- Los requisitos fueron verificados repetidamente por más de 12 plataformas AI internacionales y series de modelos grandes, generando un documento de requisitos impulsado por historias de usuario de más de 2000 líneas que casi los humanos no pueden entender.

---

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### Refactorización del Sistema de Almacenamiento
- `8dd26e3` - Unificó la interfaz ITimeStorage para usar IncompleteDate y agregó API de consulta jerárquica
  - Eliminó los métodos sobrecargados DateTime de la interfaz ITimeStorage, unificado para usar IncompleteDate
  - Agregó el método de comparación CompareTo(DateTime) y el método de expansión Expand() a IncompleteDate
  - Agregó API de consulta jerárquica GetEarliestTimestamp(), GetLatestTimestamp()
  - Agregó métodos HasSummary() y QueryWithLevel(), soportando consultas por nivel de tiempo
  - Memory.cs refactorizó el algoritmo de compresión, usando la nueva API de consulta jerárquica para mejorar la eficiencia
  - FileSystemTimeStorage.cs implementa completamente los nuevos métodos de interfaz
  - Actualizaciones sincronizadas a todos los llamadores: ChatSystem, ChatSession, BroadcastChannel, AuditLogger, TokenUsageRecord, etc.
  - Actualizaciones del sistema de herramientas: HelpTool, LogTool, TokenAuditTool adaptados a la nueva interfaz
  - Actualizaciones del controlador web: AuditController, ChatController, ChatHistoryController adaptados a la nueva interfaz
  - Total: 41 archivos cambiados (+1820/-903 líneas)

### 2026-04-27

#### Mejora del Sistema de Documentación de Ayuda
- `9989d79` - Actualizó localización, sistema de ayuda y vistas web
  - Agregó interfaz de documentación de ayuda de fábrica de cliente IA IAIClientFactoryHelp.cs
  - Completó traducción a 9 idiomas para todos los documentos de ayuda
  - HelpTopics.cs agregó 40 definiciones de temas de ayuda
  - Vistas web actualizadas integralmente: InitController, AuditView, ConfigView, KnowledgeView, LogView, etc.
  - Mejora del sistema de localización: todas las versiones de idiomas agregaron nuevas claves de localización
  - Actualizaciones de fábrica de cliente IA: mejoras en DashScopeClientFactory, OllamaClientFactory

#### Nuevo Contenido de Documentación de Ayuda
- `e7afe94` - Agregó documentación de ayuda de archivos de alma y registro de auditoría
  - Agregó documentación de ayuda de gestión de archivos de alma
  - Agregó documentación de ayuda de registro de auditoría
  - HelpTopics.cs agregó definiciones de temas
  - HelpView.cs significativamente refactorizado, mejoró la lógica de renderizado de documentos
  - PermissionView.cs refactorizado, mejoró la interfaz de gestión de permisos
  - Mejora del módulo central: mejoras en SiliconBeingManager, TaskSystem, ToolManager
  - TaskTool.cs refactorizado, mejoró la funcionalidad de gestión de tareas
  - Vistas web actualizadas integralmente: todos los componentes de vista sincronizados
  - HelpController.cs simplificado, optimizó la lógica del controlador

### 2026-04-26

#### Sistema de Documentación de Ayuda
- `07895d7` - Mejoró el sistema de documentación de ayuda, agregó 3 documentos y completó traducción a 9 idiomas
  - Agregó guía de uso del sistema de memoria, configuración de instalación de Ollama, plataforma Alibaba Cloud Bailian
  - Completó traducción a 9 idiomas para los 10 documentos de ayuda
  - Simplificó la lógica de renderizado de HelpView

#### Localización Alemana
- `0cfd8a1` - Agregó soporte completo de localización alemana (de-DE)
  - Archivos de localización alemana completos
  - Agregó soporte alemán para calendario histórico chino
  - Agregó traducción alemana de documentación de ayuda
  - Sincronizó completamente todos los documentos en 9 idiomas

#### Sincronización de Documentación
- `3aada7d` - Sincronizó documentación en chino tradicional (zh-HK) con chino simplificado
- `2f6abff` - Agregó localización de nombre de visualización de herramientas de ayuda para todos los idiomas

#### Refactorización del Sistema de Conocimiento
- `60944fe` - Unificó el espacio de nombres a SiliconLife.Collective
- `69c51c5` - Agregó sistema de documentación de ayuda y tradujo comentarios de código al inglés

### 2026-04-25

#### Automatización de Navegador WebView
- `41757c3` - Implementó automatización de navegador WebView multiplataforma basada en Playwright

#### Actualizaciones de Documentación
- `0ff797b` - Agregó documentación de KnowledgeTool y WorkNoteTool (7 idiomas)
- `ad77415` - Actualizó todos los archivos de changelog, agregó historial Git de 2026-04-25

#### Gestión de Espacio de Trabajo del Proyecto
- `785c551` - Implementó gestión de espacio de trabajo del proyecto con notas de trabajo y sistema de tareas
  - Agregó sistema de gestión de espacio de trabajo del proyecto
  - Funcionalidad de notas de trabajo para rastrear el progreso del proyecto
  - Integración del sistema de gestión de tareas

#### Localización Checa
- `b4bbf39` - Agregó localización checa completa (cs-CZ) y actualizó toda la documentación de idiomas
- `faf078f` - Corrigió errores de compilación de localización checa

#### Mejora del Sistema de Conocimiento
- `20adaac` - Agregó KnowledgeTool con soporte completo de localización

### 2026-04-24

#### Mejora de Gestión de Memoria
- `c7b2ecc` - Mejoró la gestión de memoria con filtrado avanzado, estadísticas y vistas detalladas
  - Agregó filtrado avanzado de memoria
  - Implementó estadísticas de memoria
  - Agregó página de vista detallada de memoria
  - Soporte de localización multi-idioma (6 idiomas)

#### Extensión del Sistema de Permisos
- `4489ad6` - Agregó servicio meteorológico wttr.in a la lista blanca de red
  - Sincronización completa de documentación multi-idioma (6 idiomas)

#### Correcciones de Interfaz Web
- `d9d72e9` - Corrigió problema de prioridad CSS en modal de detalle de notas de trabajo

#### Optimización del Historial de Chat
- `0df599c` - Corrigió resultados de herramientas renderizados como mensajes de chat separados
- `057b09d` - Optimizó visualización de detalles del historial de chat, mejoró renderizado de llamadas a herramientas

#### Historial de Ejecución de Temporizador
- `fa3f06f` - Agregó función de historial de ejecución de temporizador con vista detallada
- `d824835` - Agregó claves de localización de historial de ejecución de temporizador (todos los idiomas)

#### Mejora de Localización
- `c13cb17` - Registró variante de idioma español
- `9c44f34` - Agregó soporte de localización multi-idioma para calendario histórico chino

#### Mejoras de Funcionalidad Central
- `1e7c7b2` - Mejoró compresión de memoria y rastreo de ejecución de herramientas

### 2026-04-23

#### Localización de Herramientas
- `192fc6e` - Agregó localización de nombre de herramienta faltante para 5 herramientas

#### Actualizaciones de Documentación
- `882c08f` - Actualizó todos los archivos de changelog, agregó historial Git completo y eliminó números de versión falsos

#### Mejora de Página de Chat
- `65c157b` - Agregó indicador de carga a página de chat y seleccionó automáticamente sesión de curator

#### Función de Historial de Chat
- `e483348` - Implementó función de visualización de historial de chat de silicon being
  - Agregó ChatHistoryController
  - Creó ChatHistoryViewModel
  - Implementó páginas ChatHistoryListView y ChatHistoryDetailView
  - Agregó claves de localización para historial de chat (5 idiomas)

#### Mejora de Control de Flujo IA
- `30a2d4e` - Mejoró cancelación de flujo IA, integración IM e inicialización de host central

#### Cola de Mensajes de Chat
- `db48c51` - Agregó cola de mensajes de chat, metadatos de archivos y soporte de cancelación de flujo

#### Soporte de Carga de Archivos
- `28fb344` - Implementó diálogo de origen de archivos y soporte de carga de archivos
- `1d3e2cc` - Agregó cadenas de localización de diálogo de origen de archivos (6 idiomas)

#### Actualizaciones de Documentación
- `8111e92` - Agregó enlace Wiki a sección de repositorio en README

### 2026-04-22

#### Localización de Documentación
- `66c11eb` - Tradujo comentarios chinos al inglés y actualizó todos los changelogs

#### Mejora de Mensajes SSE
- `b574b2b` - Agregó senderName a mensajes históricos para identificación IA

#### Funciones de Chat
- `601fc14` - Agregó acción mark_read para marcado de fin de sesión

#### Optimización del Sistema de Herramientas
- `7a03a19` - Mejoró flexibilidad de consulta de conversación de LogTool

#### Mejora de Localización
- `0a8d750` - Agregó prompt de sistema común para comportamientos activos de silicon being

#### Refactorización del Sistema de Registro
- `2b771f3` - Desacopló LogController de E/S de archivos, agregó API de lectura de registro
- `12da302` - Agregó filtro de silicon being a vista de registro
- `8f6cb1e` - Agregó parámetro beingId a interfaz ILogger, implementó separación de registros sistema/silicon being

#### Mejoras del Sistema de Permisos
- `4c747ad` - Refactorizó PermissionTool, ExecuteCodeTool, agregó API EvaluatePermission

#### Correcciones de Errores
- `1c96e99` - Corrigió fallo de búsqueda en directorio raíz de search_files y search_content

#### Integración de Herramientas
- `135710d` - Eliminó SearchTool, movió búsqueda local a DiskTool

#### Extensión del Sistema de Herramientas
- `70ce7fb` - Implementó DatabaseTool para consultas de base de datos estructurada
- `be29a09` - Implementó LogTool para consultas de historial de operaciones y conversaciones
- `4ea7702` - Implementó PermissionTool para gestión dinámica de permisos
- `1384ff4` - Implementó ExecuteCodeTool para ejecución de código multi-idioma
- `82d1e11` - Implementó SearchTool para recuperación de información

#### Optimización de Interfaz Web
- `0675c45` - Optimizó resaltado de bloques de código markdown en panel de vista previa
- `702b3f3` - Mejoró vista de tareas con insignias de estado y visualización de metadatos
- `6ed9a79` - Mejoró almacenamiento de mensajes de chat y renderizado de vistas

### 2026-04-21

#### Correcciones de Errores
- `c6b518b` - Corrigió entrega de mensajes de temporizador y almacenamiento de mensajes de chat

#### Gestión de Configuración
- `4305769` - Agregó .gitattributes para gestión de finales de línea

#### Mejoras de Interfaz Web
- `188c6f8` - Registró ruta de API de lista de tareas y agregó visualización de estado vacío
- `634e8ca` - Agregó enlace de retorno a lista de página de permisos
- `6ba591d` - Agregó editor de configuración IA independiente para silicon beings
- `0a826f5` - Agregó prompt de éxito de guardado en editor de código
- `2940373` - Mejoró interfaz web con sugerencias de hover de código y mejoras de UI

#### Correcciones del Sistema de Permisos
- `592c7ab` - Corrigió instanciación de callback y orden de registro

#### Mejora de Seguridad
- `833ead2` - Agregó verificación de referencia de ensamblado para compilación dinámica

#### Mejora del Sistema de Permisos
- `5879621` - Agregó verificación de pre-compilación de callback de permisos y manejo mejorado de errores

#### Actualizaciones de Documentación
- `4dbf659` - Actualizó changelog a v0.5.1, reemplazó URLs de marcador de posición de GitHub, agregó espejo Gitee, localizó nombre de Bilibili por idioma, actualizó correo electrónico

#### Configuración y Entrada
- `0fc1693` - Actualizó entrada de programa y configuración de proyecto

#### Refactorización del Sistema de Permisos
- `ea9179a` - Mejoró implementación del sistema de permisos

#### Correcciones de Errores
- `928a96d` - Corrigió implementación de cálculo de calendario

#### IA y Calendario
- `646813e` - Mejoró implementación de fábrica de cliente IA

#### Localización
- `7940d9c` - Agregó soporte de localización coreana
- `4ff98ad` - Refactorizó documentación para soporte multi-idioma

### 2026-04-20

#### Finalización de Funcionalidad Central
- `28905b5` - Soporte multi-idioma completo, fábrica de cliente IA, sistema de permisos y configuración de localización
  - Sistema de registro con gerente, entradas y diferentes niveles de registro
  - Sistema de auditoría de tokens para consultar y rastrear uso de tokens
  - Fábricas de cliente IA para descubrimiento automático de diferentes plataformas IA
  - Sistema de callback de permisos con su propio almacenamiento
  - Implementación de registrador de consola
  - Soporte multi-idioma para inglés y chino simplificado
  - Mensajero WebUI con WebSocket para chat en tiempo real
  - Silicon being por defecto mejorado con localización

### 2026-04-19

#### Temporizador y Calendario
- `c933fd8` - Actualizó localización, sistema de temporizador, vistas web y agregó herramientas
  - Mejor gerente de localización
  - Sistema de programación para tareas programadas
  - Configuración IA y gestión de contexto
  - Herramienta de calendario soportando 32 tipos de calendario
  - Controlador web para APIs de calendario
  - Herramienta de gestión de tareas

**Mejoras de Arquitectura**
- Rediseñó arquitectura de vista web para mejor soporte de skins
- Mejoró sistema de gestión de beings con mejor manejo de estado

### 2026-04-18

- `9f585e1` - Actualizó localización, sistema de temporizador, vistas web y agregó herramientas
  - Mejoras de temporizador y programación
  - Mejores vistas web con componentes de UI mejorados
  - Más implementaciones de herramientas

### 2026-04-17

- `9b71fcd` - Actualizó módulos centrales, agregó documentación zh-HK, canal de broadcast, herramientas de configuración y vistas web de auditoría
  - Canal de broadcast para múltiples silicon beings chateando juntos
  - Sistema de herramientas de configuración
  - Vistas web de auditoría
  - Documentación en chino tradicional

### 2026-04-16

- `5040f05` - Actualizó módulos centrales y por defecto
  - Optimización de módulos y correcciones de errores
  - Actualizaciones y mejoras de implementación

### 2026-04-15

- `3efab5f` - Actualizó múltiples módulos: AI, Chat, IM, Tools, Web, Localization, Storage
  - Mejoras de cliente IA
  - Mejora del sistema de chat
  - Actualizaciones de proveedor de mensajero
  - Optimización del sistema de herramientas
  - Mejoras de infraestructura web
  - Optimización de localización
  - Actualizaciones del sistema de almacenamiento

### 2026-04-14

- `4241a2f` - Funciones de chat básicamente completas, optimización de carga de UI
  - Funcionalidad del sistema de chat completada
  - Optimización de UI para cargas de archivos

### 2026-04-13

- `c498c31` - Actualizaciones de código
  - Mejoras generales de código y optimización

### 2026-04-12

#### Documentación y Localización
- `2161002` - Refactorizó documentación y mejoró localización
- `03d94e4` - Mejoró sistema de configuración y localización
- `9976a35` - Agregó página acerca de y localización

#### Chat y Vistas Web
- `0c8ccfc` - Mejoró sistema de chat, localización y vistas web
- `a8f1342` - Rediseñó capa de comunicación web, cambió de WebSocket a SSE

### 2026-04-11

#### Sistema de Registro
- `e8fe259` - Agregó sistema de registro y optimización de código
- `f01c519` - Agregó sistema de registro, actualizó interfaz IA y vistas web

### 2026-04-10

- `4962924` - Mejoró manejador WebSocket, vistas de chat e interacción de mensajero
  - Mejoras de gestor de contexto
  - Mejora del sistema de chat
  - Actualizaciones de interfaz de proveedor de mensajero
  - Rediseño de proveedor WebUI
  - Actualizaciones de constructor de JavaScript y enrutador
  - Optimización de vista de chat
  - Mejoras de manejador WebSocket

### 2026-04-09

- `f9302bf` - Mejoró interfaz de proveedor de mensajero, sistema de chat e interacción de UI web
  - Extensión de interfaz de proveedor de mensajero
  - Mejoras de mensajes de chat y sistema
  - Optimización de gestor de contexto
  - Mejora de silicon being por defecto
  - Mejoras de vista de chat de UI web
  - Actualizaciones de manejador WebSocket

### 2026-04-07

- `6831ee8` - Rediseñó vistas web y constructor de JavaScript
  - Rediseño completo de controladores web
  - Reescritura completa de constructor de JavaScript
  - Todos los componentes de vista actualizados
  - Mejoras del sistema de skins
  - Actualización de arquitectura de clase base de vista

### 2026-04-05

- `41e97fb` - Actualizó múltiples módulos centrales y controladores web
  - Mejoras de gestor de contexto
  - Sistema de chat y gestión de sesiones
  - Rediseño de localizador de servicios
  - Actualizaciones de clase base y gestor de silicon being
  - Controladores web actualizados integralmente (17 controladores)
  - Mejoras de fábrica de silicon being por defecto
- `67988d4` - Mejoró módulo de UI web, agregó vista de ejecutor, limpió vistas y módulos centrales

### 2026-04-04

- `b58bb1c` - Agregó controlador de inicialización y rediseñó módulo web
  - Controlador de inicialización
  - Rediseño de módulo de configuración
  - Actualizaciones de módulo de localización
  - Mejoras del sistema de skins
  - Mejora de enrutador
- `f03ac0b` - Agregó módulo de UI web, mejoró funcionalidad de mensajero

### 2026-04-03

- `192e57b` - Actualizó estructura de proyecto y componentes de runtime centrales
- `59faec8` - Actualizaciones de implementación central y por defecto
- `d488485` - Agregó funcionalidad de compilación dinámica y módulo de herramientas de curator
- `753d1d9` - Agregó módulo de seguridad, actualizó ejecutores, proveedores de mensajero, localización y herramientas
- `a378697` - Completó etapa 5 - sistema de herramientas + ejecutores

### 2026-04-02

- `e6ad94b` - Corrigió fallo de carga de historial de chat al eliminar archivos de configuración durante pruebas
- `daa56f5` - Completó etapa 4: memoria persistente (sistema de chat + canal de mensajero)

### 2026-04-01

- `bbe2dbb` - Corrigió carga de configuración y enrutamiento de mensajes de servicio de chat
- `2fa6305` - Implementó etapa 2: marco de bucle principal y sistema de objetos de reloj
- `32b99a1` - Implementó etapa 1 - funcionalidad básica de chat
- `358e368` - Commit inicial: documentación de proyecto y licencia
