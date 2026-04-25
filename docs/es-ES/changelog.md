# Registro de Cambios

[English](changelog.md) | [简体中文](docs/zh-CN/changelog.md) | [繁體中文](docs/zh-HK/changelog.md) | [Español](docs/es-ES/changelog.md) | [日本語](docs/ja-JP/changelog.md) | [한국어](docs/ko-KR/changelog.md) | [Čeština](docs/cs-CZ/changelog.md)

Todos los cambios notables de este proyecto se documentarán en este archivo.

El formato se basa en [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
y este proyecto sigue [Versionado Semántico](https://semver.org/spec/v2.0.0.html).

**Nota: Este proyecto aún no ha publicado ninguna versión oficial. Todo el contenido siguiente está en desarrollo.**

---

## Acerca de este Registro de Cambios

### Origen del Proyecto

- Este proyecto se originó el 20 de marzo de 2026.
- Antes de este proyecto, hubo una Demo de validación que falló debido a un diseño de arquitectura poco razonable, lo que imposibilitó la integración con múltiples plataformas de IA.

### Herramientas AI IDE Utilizadas

#### Kiro (Amazon AWS)
- El proyecto fue mantenido inicialmente por Kiro y lanzado usando el modo Spec.
- Kiro es un entorno de desarrollo de IA agéntica construido por Amazon AWS.
- Basado en Code OSS (VS Code), soporta configuraciones de VS Code y plugins compatibles con Open VSX.
- Características de flujo de trabajo de desarrollo impulsado por especificaciones para codificación estructurada de IA.

#### Comate AI IDE / Wenxin Kuaima (Baidu)
- Ocasionalmente usado para trabajo de redacción y documentación.
- Comate AI IDE es una herramienta de entorno de desarrollo nativo de IA lanzada por Baidu Wenxin el 23 de junio de 2025.
- El primer IDE de IA colaborativo multi-modal y multi-agente de la industria.
- Las características incluyen conversión de diseño a código y codificación asistida por IA de proceso completo.
- Impulsado por el modelo Wenxin 4.0 X1 Turbo de Baidu.

#### Trae (ByteDance)
- Este proyecto fue mantenido principalmente usando Trae durante la mayor parte del tiempo.
- Trae es un IDE de IA desarrollado por la subsidiaria de ByteDance con sede en Singapur, SPRING PTE.
- Funciona como un Ingeniero de IA 10x capaz de construir soluciones de software de forma independiente.
- Características de herramientas de productividad inteligentes, adaptación flexible del ritmo de desarrollo y entrega colaborativa de proyectos.
- Ofrece rendimiento de nivel empresarial con sistemas de agentes configurables.

#### Qoder (Alibaba)
- Desde el 18 de abril de 2026, este proyecto ha sido mantenido usando Qoder.
- Qoder destaca en análisis de código fuente y generación de documentación de dominio, demostrando capacidad excepcional en la comprensión de bases de código complejas.
- Opera con un modelo de precios de costo de cómputo cero, lo que lo hace altamente rentable para procesamiento automatizado de documentación y manejo de tareas rutinarias.
- Una plataforma de codificación agéntica impulsada por IA diseñada para desarrollo de software del mundo real.
- Características de generación inteligente de código, programación basada en conversaciones, motor avanzado de análisis de contexto y colaboración multi-agente.
- Proporciona comprensión profunda del código con consumo mínimo de recursos, ideal para mantenimiento de proyectos a largo plazo y acumulación de conocimiento.

### Documentación de Requisitos

- La documentación de requisitos para este proyecto no se divulga públicamente.
- Los requisitos fueron validados repetidamente por más de 12 plataformas de IA internacionales y series de modelos grandes, resultando en un documento de requisitos impulsado por historias de usuario de más de 2,000 líneas que es casi incomprensible para los humanos.

---

## [No publicado]

### 2026-04-24

#### Mejora de Gestión de Memoria
- `c7b2ecc` - Mejorar la gestión de memoria con filtrado avanzado, estadísticas y vista de detalles
  - Nueva función de filtrado avanzado de memoria con filtrado multidimensional por tipo, rango de tiempo, etiquetas, etc.
  - Función de estadísticas de memoria implementada que muestra el recuento de memorias, distribución de tipos y otras estadísticas
  - Página de vista de detalles de memoria agregada para ver información completa de memorias individuales
  - Interfaz de gestión de memoria optimizada para mejorar la experiencia del usuario y la eficiencia operativa
  - Soporte de localización multiidioma (6 idiomas)

#### Extensión del Sistema de Permisos
- `4489ad6` - Agregar servicio meteorológico wttr.in a la lista blanca de red
  - Permitir que los Seres de Silicio accedan a la API meteorológica wttr.in para obtener información del clima
  - Documentación del sistema de permisos actualizada explicando la configuración de permisos del servicio meteorológico
  - Sincronización completa de documentación multiidioma (6 idiomas)

#### Historial de ejecución del temporizador
- `fa3f06f` - Añadir función de historial de ejecución del temporizador con vista de detalles
  - Nueva función de visualización del historial de ejecución del temporizador
  - Página de detalles del historial de ejecución implementada
  - Soporte para ver registros detallados de cada ejecución del temporizador
- `d824835` - Añadir claves de localización del historial de ejecución del temporizador para todos los idiomas
  - Soporte de localización completo para el historial de ejecución del temporizador (6 idiomas)
  - Incluye localización para estado de ejecución, hora, resultados y otra información

#### Optimización del historial de chat
- `057b09d` - Optimizar la visualización de detalles del historial de chat con renderizado mejorado de llamadas a herramientas
  - Optimización de la visualización de llamadas a herramientas en la página de detalles del historial de chat
  - Mejora en la visualización formateada de los parámetros de llamada a herramientas
  - Mejora en la legibilidad de los mensajes históricos
- `0df599c` - Corregir que los resultados de herramientas se rendericen como mensajes de chat separados
  - Los resultados de ejecución de herramientas ahora están correctamente asociados con los mensajes originales
  - Evitar que los resultados de herramientas se muestren como respuestas de IA separadas
  - Mejorar la coherencia de los mensajes de chat

#### Correcciones de la interfaz web
- `d9d72e9` - Corregir problema de prioridad CSS en el modal de detalles de notas de trabajo
  - Corrección de estilo del modal de notas de trabajo
  - Ajuste de prioridad CSS para asegurar que los estilos se apliquen correctamente
  - Mejora en los efectos visuales del modal

#### Mejoras de funcionalidad principal
- `1e7c7b2` - Mejorar la compresión de memoria y el seguimiento de ejecución de herramientas
  - Optimización del algoritmo de compresión de memoria
  - Mejora del mecanismo de seguimiento de ejecución de herramientas
  - Mejora en la eficiencia de gestión de memoria de los seres de silicio

#### Mejora de localización
- `c13cb17` - Registrar variantes del idioma español
  - Soporte de localización para español (España)
  - Expansión del sistema de múltiples idiomas
- `9c44f34` - Añadir soporte de localización multilingüe para el Calendario Histórico Chino
  - Localización completa para el Calendario Histórico Chino
  - Soporte multilingüe para nombres de eras históricas, tablas de dinastías y otra información
- `192fc6e` - Añadir localizaciones de nombres de herramientas faltantes para 5 herramientas
  - Complementar la visualización de nombres de herramientas localizados
  - Mejorar la experiencia multilingüe de la interfaz de herramientas

### 2026-04-23

#### Historial de Chat e Indicadores de Carga
- `e483348` - Implementar función de vista de historial de chat de silicon being
  - Nuevo ChatHistoryController con APIs de lista de conversaciones y detalles de mensajes
  - Creado ChatHistoryViewModel para transferencia de datos
  - Implementadas páginas ChatHistoryListView y ChatHistoryDetailView
  - Agregadas claves de localización para historial de chat (5 idiomas)
  - Actualizado Router para incluir rutas de historial de chat
  - Agregado enlace de entrada de historial de chat en página de detalle de BeingView
- `65c157b` - Agregar indicadores de carga para páginas de chat y auto-seleccionar sesión de curator
  - Indicador de estado de carga de página de chat
  - Función de auto-selección de sesión de curator
  - Soporte multi-idioma (6 idiomas)

#### Mejora de Control de Flujo de IA
- `30a2d4e` - Mejorar cancelación de flujo de IA, integración IM e inicialización de host principal
  - Mejora del mecanismo de cancelación de flujo de ContextManager
  - Mejora de integración del sistema IM
  - Optimización de inicialización de CoreHost
  - Mejora de funcionalidad de DiskExecutor
  - Actualización de WebUIProvider

#### Soporte de Carga de Archivos
- `28fb344` - Implementar diálogo de fuente de archivos y soporte de carga de archivos
  - Diálogo de fuente de archivos de Web UI
  - Implementación de funcionalidad de carga de archivos
- `1d3e2cc` - Agregar cadenas de localización para diálogo de fuente de archivos
  - Soporte multi-idioma para diálogo de fuente de archivos (6 idiomas)

#### Cola de Mensajes de Chat
- `db48c51` - Agregar cola de mensajes de chat, metadatos de archivos y soporte de cancelación de flujo
  - Nuevo sistema de cola de mensajes de chat ChatMessageQueue
  - Nueva gestión de metadatos de archivos FileMetadata
  - Nuevo gestor de cancelación de flujo StreamCancellationManager

### 2026-04-22

#### Mejora de Localización
- `b574b2b` - Agregar senderName a mensajes históricos para identificación de IA
  - Agregado campo de nombre de remitente a mensajes históricos de SSE
  - Soporte de identificación de identidad de mensajes de IA
- `0a8d750` - Agregar prompt de sistema común para comportamiento proactivo de silicon being
  - Agregada plantilla de prompt de sistema común al sistema de localización
  - Soporte de guía de comportamiento proactivo para silicon beings

#### Expansión del Sistema de Herramientas
- `70ce7fb` - Implementar DatabaseTool para consultas de base de datos estructuradas
  - Nueva herramienta de consulta de base de datos
  - Soporte de operaciones de datos estructurados
- `be29a09` - Implementar LogTool para consultas de historial de operaciones y conversaciones
  - Nueva herramienta de consulta de registros
  - Soporte de recuperación de historial de operaciones e historial de conversaciones
- `4ea7702` - Implementar PermissionTool para gestión dinámica de permisos
  - Nueva herramienta de gestión de permisos
  - Soporte de consulta y gestión dinámica de permisos
- `1384ff4` - Implementar ExecuteCodeTool para ejecución de código multi-idioma
  - Nueva herramienta de ejecución de código
  - Soporte de compilación y ejecución de código multi-idioma
- `82d1e11` - Implementar SearchTool para recuperación de información
  - Nueva herramienta de búsqueda de información
  - Soporte de recuperación de información externa

#### Refactorización del Sistema de Registros
- `8f6cb1e` - Agregar parámetro beingId a la interfaz ILogger para separación de registros de sistema/silicon being
  - Extensión de interfaz ILogger
  - Soporte de separación de registros de sistema y registros de silicon being
  - Nuevo parámetro beingId
- `2b771f3` - Desacoplar LogController de E/S de archivos y agregar API de lectura de registros
  - Refactorización de arquitectura de LogController
  - Nueva API de lectura de registros independiente
  - Operaciones de E/S de archivos separadas
- `12da302` - Agregar filtro de silicon being a la vista de registros
  - Web UI agregó función de filtro de silicon being
  - Puede filtrar registros por silicon being

#### Mejora del Sistema de Permisos
- `4c747ad` - Refactorizar PermissionTool, ExecuteCodeTool y agregar API EvaluatePermission
  - Refactorización de PermissionTool y ExecuteCodeTool
  - API EvaluatePermission integrada

#### Optimización de Web UI
- `702b3f3` - Mejorar vista de tareas con insignias de estado y visualización de metadatos
  - Mejora de UI de vista de tareas
  - Nuevas insignias de estado y visualización de metadatos
- `6ed9a79` - Mejorar almacenamiento de mensajes de chat y renderizado de vistas
  - Optimización del mecanismo de almacenamiento de mensajes de chat
  - Mejora del rendimiento de renderizado de vistas
- `0675c45` - Optimizar resaltado de bloques de código markdown en panel de vista previa
  - Optimización de resaltado de código de vista previa markdown
  - Efecto de visualización de bloques de código mejorado

#### Integración de Herramientas
- `135710d` - Eliminar SearchTool, mover búsqueda local a DiskTool
  - Eliminación de SearchTool
  - Funcionalidad de búsqueda local integrada en DiskTool
- `7a03a19` - Mejorar flexibilidad de consulta de conversaciones de LogTool
  - Optimización de lógica de consulta de conversaciones de LogTool
  - Flexibilidad de consulta mejorada

#### Gestión de Configuración
- `4305769` - Agregar .gitattributes para gestión de finales de línea
  - Configuración de finales de línea para compatibilidad multi-plataforma

#### Correcciones de Errores
- `1c96e99` - Corregir falla de búsqueda en directorio raíz de search_files y search_content
  - Corrección de funcionalidad de búsqueda en directorio raíz de DiskTool
  - Corrección de lógica de búsqueda de archivos y contenido

### 2026-04-21

#### Mejora del Sistema de Permisos
- `5879621` - Agregar validación de pre-compilación de callback de permisos y manejo mejorado de errores
  - Función de validación de pre-compilación de callback de permisos
  - Compilar código de callback de permisos antes de guardar para verificar corrección
  - Prevenir que código inválido se guarde en disco
  - Manejo detallado de errores para operaciones de guardado de permisos
  - Mensajes de error mejorados con soporte de localización
  - Pasos de compilación y escaneo de seguridad separados para mejor reporte de errores
- `833ead2` - Agregar validación de referencias de ensamblado para compilación dinámica
  - Mejora de seguridad de compilación dinámica
  - Mecanismo de validación de referencias de ensamblado

#### Mejora de Web UI
- `0a826f5` - Agregar alerta de éxito de guardado en editor de código
  - Función de alerta de éxito de operación de guardado de editor de código
  - Retroalimentación mejorada de operación de usuario
- `2940373` - Mejorar interfaz web con sugerencias de hover de código y mejoras de UI
  - Función de sugerencia de hover de editor de código
  - Optimización de UI de interfaz web
- `6ba591d` - Agregar editor de configuración de IA independiente para silicon beings
- `634e8ca` - Agregar enlace de retorno a lista en página de permisos
- `188c6f8` - Registrar ruta de API de lista de tareas y agregar visualización de estado vacío

#### Correcciones de Errores
- `592c7ab` - Corregir orden de instanciación y registro de callbacks
  - Corrección de sistema de callback de permisos
  - Optimización de orden de registro de callbacks
- `c6b518b` - Corregir entrega de mensajes de temporizador y almacenamiento de mensajes de chat
  - Corrección de mecanismo de entrega de mensajes de temporizador
  - Optimización de almacenamiento de mensajes de chat

#### Localización
- `7940d9c` - Agregar soporte de localización en coreano
  - Archivos de localización en coreano
  - Mejora del sistema multi-idioma
- `4ff98ad` - Reestructurar documentación con soporte multi-idioma
  - Reorganización de estructura de documentación
  - Sincronización de documentación multi-idioma

#### IA y Calendario
- `646813e` - Mejorar implementación de fábrica de clientes de IA
  - Refactorización de fábrica de clientes de IA
  - Optimización de mecanismo de descubrimiento de clientes
- `928a96d` - Corregir implementaciones de cálculo de calendario
  - Corrección de lógica de cálculo de calendario
  - Mejora de precisión para múltiples tipos de calendario

#### Configuración y Entrada
- `0fc1693` - Actualizar entrada de programa y configuración de proyecto
  - Optimización de punto de entrada de programa
  - Mejora de configuración de proyecto

### 2026-04-20

#### Finalización de Funciones Principales
- `28905b5` - Completar soporte multi-idioma, fábrica de clientes de IA, sistema de permisos y configuración de localización
  - Sistema de registros con gestor, entradas y diferentes niveles de registro
  - Sistema de auditoría de tokens para consultar y rastrear uso de tokens
  - Fábrica de clientes de IA para descubrimiento automático de diferentes plataformas de IA
  - Sistema de callback de permisos con su propio almacenamiento
  - Implementación de registrador de consola
  - Soporte multi-idioma para inglés y chino simplificado
  - Mensajero WebUI con WebSocket para chat en tiempo real
  - Silicon being predeterminado mejorado con localización

### 2026-04-19

#### Temporizador y Calendario
- `c933fd8` - Actualizar localización, sistema de temporizador, vistas web y agregar nuevas herramientas
  - Mejor gestor de localización
  - Sistema de programación para tareas programadas
  - Configuración de IA y gestión de contexto
  - Herramienta de calendario que soporta 32 tipos de calendario
  - Controlador web para API de calendario
  - Herramienta de gestión de tareas

**Mejora de Arquitectura**
- Arquitectura de vista web rediseñada para soportar mejor skins
- Sistema de gestión de beings mejorado con mejor manejo de estado

### 2026-04-18

- `9f585e1` - Actualizar localización, sistema de temporizador, vistas web y agregar nuevas herramientas
  - Mejoras de temporizador y programación
  - Mejores vistas web con componentes de UI mejorados
  - Más implementaciones de herramientas

### 2026-04-17

- `9b71fcd` - Actualizar módulos principales, agregar documentos zh-HK, canal de broadcast, herramienta de configuración y vistas web de auditoría
  - Canal de broadcast para múltiples silicon beings para chatear juntos
  - Sistema de herramienta de configuración
  - Vistas web de auditoría
  - Documentación en chino tradicional

### 2026-04-16

- `5040f05` - Actualizar módulos principales y predeterminados
  - Optimización de módulos y correcciones de errores
  - Actualizaciones y mejoras de implementación

### 2026-04-15

- `3efab5f` - Actualizar múltiples módulos: AI, Chat, IM, Tools, Web, Localization, Storage
  - Mejoras de cliente de IA
  - Mejora de sistema de chat
  - Actualizaciones de proveedor de mensajero
  - Optimización de sistema de herramientas
  - Mejoras de infraestructura web
  - Optimización de localización
  - Actualizaciones de sistema de almacenamiento

### 2026-04-14

- `4241a2f` - Funcionalidad de chat básicamente completa, optimización de carga de UI
  - Finalización de funcionalidad de sistema de chat
  - Optimización de UI para cargas de archivos

### 2026-04-13

- `c498c31` - Actualización de código
  - Mejoras generales de código y optimización

### 2026-04-12

- `2161002` - Reestructurar documentación y mejorar localización
  - Reorganización de documentación
  - Mejora de sistema de localización
- `03d94e4` - Mejorar sistema de configuración y localización
  - Mejora de sistema de configuración
  - Soporte de idiomas adicionales
- `9976a35` - Agregar página acerca de y localización
  - Página acerca de
  - Mejora de localización
- `0c8ccfc` - Mejorar sistema de chat, localización y vistas web
  - Mejoras de sistema de chat
  - Actualizaciones de localización
  - Mejora de vistas web
- `a8f1342` - Rediseñar capa de comunicación web, cambiar de WebSocket a SSE
  - Comunicación web ahora usa Server-Sent Events

### 2026-04-11

- `e8fe259` - Agregar sistema de registros y optimización de código
  - Infraestructura de registros
  - Implementación de registrador
- `f01c519` - Agregar sistema de registros, actualizar interfaz de IA y vistas web
  - Actualizaciones de interfaz de IA
  - Mejoras de vistas web

### 2026-04-10

- `4962924` - Mejorar manejador de WebSocket, vistas de chat e interacción de mensajero
  - Mejoras de gestor de contexto
  - Mejora de sistema de chat
  - Actualizaciones de interfaz de proveedor de mensajero
  - Rediseño de proveedor WebUI
  - Actualizaciones de constructor y enrutador JavaScript
  - Optimización de vista de chat
  - Mejoras de manejador de WebSocket

### 2026-04-09

- `f9302bf` - Mejorar interfaz de proveedor de mensajero, sistema de chat e interacción de Web UI
  - Extensión de interfaz de proveedor de mensajero
  - Mejoras de mensajes de chat y sistema
  - Optimización de gestor de contexto
  - Mejora de silicon being predeterminado
  - Mejoras de vista de chat de Web UI
  - Actualizaciones de manejador de WebSocket

### 2026-04-07

- `6831ee8` - Rediseñar vistas web y constructor JavaScript
  - Rediseño completo de controlador web
  - Reescritura completa de constructor JavaScript
  - Actualizaciones de todos los componentes de vista (vistas de beings, vistas de chat, vistas de navegador de código, vistas de configuración, etc.)
  - Mejoras de sistema de skins (skins de Admin, Chat, Creative, Dev)
  - Mejora de arquitectura de clase base de vista

### 2026-04-05

- `41e97fb` - Actualizar múltiples módulos principales y controladores web
  - Mejoras de gestor de contexto
  - Sistema de chat y gestión de sesiones
  - Rediseño de localizador de servicios
  - Actualizaciones de clase base y gestor de silicon being
  - Actualizaciones completas de controlador web (17 controladores)
  - Mejora de fábrica de silicon being predeterminado
- `67988d4` - Mejorar módulo Web UI, agregar vista de ejecutor, limpiar vistas y módulos principales
  - Vista de ejecutor
  - Limpieza y organización de módulos

### 2026-04-04

- `b58bb1c` - Agregar controlador de inicialización y rediseñar módulos web
  - Controlador de inicialización
  - Rediseño de módulo de configuración
  - Actualizaciones de módulo de localización
  - Mejoras de sistema de skins
  - Mejora de enrutador
- `f03ac0b` - Agregar módulo Web UI, mejorar funcionalidad de mensajero
  - Módulo Web UI
  - Mejoras de funcionalidad de mensajero

### 2026-04-03

- `192e57b` - Actualizar estructura de proyecto y componentes de tiempo de ejecución principales
  - Actualizaciones de sistema de tiempo de ejecución
  - Mejora de estructura de proyecto
- `59faec8` - Actualizaciones de implementación principal y predeterminada
  - Mejora de módulo principal
  - Actualizaciones de implementación predeterminada
- `d488485` - Agregar funcionalidad de compilación dinámica y módulo de herramienta de curator
  - Ejecutor de compilación dinámica
  - Implementación de herramienta de curator
- `753d1d9` - Agregar módulo de seguridad, actualizar ejecutores, proveedores de mensajero, localización y herramientas
  - Sistema de seguridad
  - Actualizaciones de ejecutor
  - Mejora de proveedor de mensajero
  - Mejoras de localización
  - Actualizaciones de sistema de herramientas
- `a378697` - Completar Fase 5 - Sistema de Herramientas + Ejecutores
  - Gestión y definición de herramientas
  - Ejecutor de línea de comandos
  - Ejecutor de disco
  - Ejecutor de red
  - Implementaciones de herramientas

### 2026-04-02

- `e6ad94b` - Corregir falla de carga de historial de chat al eliminar archivo de configuración durante pruebas
  - Mejora de manejo de errores de cliente Ollama
  - Validación de datos de configuración
  - Limpieza de referencias de proyecto
- `daa56f5` - Completar Fase 4: Memoria Persistente (Sistema de Chat + Canal de Mensajero)
  - Sistema de chat con chat grupal y chat privado
  - Interfaces de proveedor y gestor de mensajero
  - Almacenamiento indexado por tiempo
  - Manejo de fechas incompletas
  - Almacenamiento de tiempo de sistema de archivos
  - Proveedor de mensajero de consola
  - Mejora de fábrica de silicon being
  - Actualizaciones de inicialización de programa

### 2026-04-01

- `bbe2dbb` - Corregir carga de configuración y enrutamiento de mensajes de servicio de chat
  - Implementación de gestor de contexto (agregadas 188 líneas de código)
  - Interfaz de cliente de IA
  - Interfaz de servicio de chat e implementación simple
  - Sistema de configuración con convertidores
  - Mejoras de programador de bucle principal
  - Sistema de gestión de silicon being
  - Implementación de cliente Ollama
  - Configuración de sistema de localización
  - Rediseño de inicialización de programa
- `2fa6305` - Implementar Fase 2: Marco de Bucle Principal y Sistema de Objetos Tick
  - Programador de bucle principal
  - Clase base de objeto tick
  - Interfaz de fábrica de cliente de IA
  - Interfaz de almacenamiento
  - Fábrica de cliente Ollama
  - Objeto tick de consola
  - Objeto tick de prueba
  - Almacenamiento de sistema de archivos
- `32b99a1` - Implementar Fase 1 - Funcionalidad Básica de Chat
  - Modelos de solicitud y respuesta de IA
  - Interfaz de cliente de IA
  - Modelo de mensaje
  - Sistema de localización (idioma, clase base de localización, gestor de localización)
  - Clase base de datos de configuración
  - Implementación de cliente Ollama
  - Gestión de configuración
  - Localización multi-idioma (inglés, chino simplificado)
  - Punto de entrada de programa
- `358e368` - Commit inicial: documentación de proyecto y licencia
  - README de proyecto (inglés y chino)
  - Documentación de arquitectura
  - Documentación de hoja de ruta
  - Documentación de seguridad
  - Licencia (Apache 2.0)
  - Configuración de git ignore
