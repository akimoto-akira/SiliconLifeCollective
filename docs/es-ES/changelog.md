# Registro de Cambios

[English](../en/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | **Español** | [Deutsch](../de-DE/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md)

Todos los cambios importantes de este proyecto serán documentados en este archivo.

El formato se basa en [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
y este proyecto sigue [Versionado Semántico](https://semver.org/spec/v2.0.0.html).

**Nota: Este proyecto aún no ha lanzado ninguna versión oficial, todo el contenido está en desarrollo.**

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

## [No Publicado]

### 2026-04-26

#### Mejora del Sistema de Documentos de Ayuda
- Añadir documentos de Sistema de Memoria, Configuración de Ollama y Guía de la Plataforma Alibaba Cloud Bailian (3 nuevos documentos)
- Completar la traducción de 10 documentos de ayuda a 9 idiomas (zh-CN, zh-HK, en-US, de-DE, cs-CZ, es-ES, ja-JP, ko-KR)
- Simplificar la lógica de renderizado de HelpView
- Total: aproximadamente 14,000 líneas de documentación en todos los idiomas

### 2026-04-25

#### Gestión de Espacio de Trabajo de Proyectos
- `785c551` - Implementar gestión de espacio de trabajo de proyectos con notas de trabajo y sistema de tareas
  - Nuevo sistema de gestión de espacio de trabajo de proyectos
  - Función de notas de trabajo para seguimiento de progreso de proyectos
  - Integración del sistema de gestión de tareas
  - Mejora de capacidades de organización y seguimiento de proyectos

#### Localización en Checo
- `b4bbf39` - Añadir localización completa en checo (cs-CZ) y actualizar documentos en todos los idiomas
  - Soporte completo para checo
  - Actualización de documentos en todos los idiomas para incluir checo
  - Localización completa de elementos de UI y mensajes
- `faf078f` - Corregir errores de compilación en localización checa
  - Resolver problemas de compilación en archivos de localización checa
  - Asegurar integración correcta del paquete de idioma checo

#### Mejora del Sistema de Conocimiento
- `20adaac` - Añadir KnowledgeTool y soportar localización completa
  - Nuevo KnowledgeTool para gestión de conocimiento
  - Soporte completo de localización multiidioma
  - Mejora de capacidades de red de conocimiento

### 2026-04-24

#### Mejora del Sistema de Gestión de Memoria
- `c7b2ecc` - Mejorar funciones de gestión de memoria, añadir filtrado avanzado, estadísticas y vista de detalles
  - Nueva función de filtrado avanzado de memoria, soporta filtrado multidimensional por tipo, rango de tiempo, etiquetas, etc.
  - Implementación de función de estadísticas de memoria, muestra información estadística como cantidad de memorias, distribución por tipo, etc.
  - Añadir página de vista de detalles de memoria, soporta ver información completa de una sola memoria
  - Optimización de interfaz de gestión de memoria, mejora experiencia de usuario y eficiencia operativa
  - Soporte de localización multiidioma (6 idiomas)

#### Extensión del Sistema de Permisos
- `4489ad6` - Añadir servicio meteorológico wttr.in a la lista blanca de red
  - Permitir que Seres Silicona accedan a API meteorológica wttr.in para obtener información del clima
  - Actualizar documentos del sistema de permisos, explicar configuración de permisos del servicio meteorológico
  - Actualización sincronizada de documentos multiidioma completos (6 idiomas)

#### Optimización del Historial de Chat
- `057b09d` - Optimizar visualización de detalles del historial de chat, mejorar renderizado de invocación de herramientas
  - Optimización de visualización de invocación de herramientas en página de detalles del historial de chat
  - Mejorar visualización formateada de parámetros de invocación de herramientas
  - Mejorar legibilidad de mensajes históricos
- `0df599c` - Corregir problema de resultados de herramientas renderizados como mensajes de chat independientes
  - Los resultados de ejecución de herramientas ahora están correctamente asociados al mensaje original
  - Evitar que resultados de herramientas se muestren como respuestas de IA separadas
  - Mejorar coherencia de mensajes de chat

#### Corrección de Interfaz Web
- `d9d72e9` - Corregir problema de prioridad CSS en modal de detalles de notas de trabajo
  - Corrección de estilo de modal de notas de trabajo
  - Ajuste de prioridad CSS para asegurar aplicación correcta de estilos
  - Mejorar efecto visual del modal

#### Mejora de Funciones Centrales
- `1e7c7b2` - Mejorar compresión de memoria y seguimiento de ejecución de herramientas
  - Optimización de algoritmo de compresión de memoria
  - Mejora de mecanismo de seguimiento de ejecución de herramientas
  - Mejorar eficiencia de gestión de memoria de Seres Silicona

#### Mejora de Localización
- `c13cb17` - Registrar variante de idioma español
  - Soporte de localización para español (España)
  - Expansión del sistema multiidioma
- `9c44f34` - Añadir soporte de localización multiidioma para calendario histórico chino
  - Localización completa de idioma para calendario histórico chino
  - Soporte multiidioma para información de años de reinado histórico, dinastías, etc.
- `192fc6e` - Añadir localización de nombres de herramientas faltantes para 5 herramientas
  - Complementar visualización de nombres localizados de herramientas
  - Mejorar experiencia multiidioma de interfaz de herramientas

### 2026-04-23

#### Historial de Chat e Indicador de Carga
- `e483348` - Implementar función de visualización de historial de chat de Seres Silicona
  - Nuevo ChatHistoryController, incluye API de lista de sesiones y detalles de mensajes
  - Crear ChatHistoryViewModel para transferencia de datos
  - Implementar páginas ChatHistoryListView y ChatHistoryDetailView
  - Añadir claves de localización para historial de chat (5 idiomas)
  - Actualizar Router para incluir rutas de historial de chat
  - Añadir enlace de entrada de historial de chat en página de detalles de BeingView
- `65c157b` - Añadir indicador de carga para página de chat y seleccionar automáticamente sesión del curador
  - Indicador de estado de carga en página de chat
  - Función de selección automática de sesión del curador
  - Soporte multiidioma (6 idiomas)

#### Mejora de Control de Flujo de IA
- `30a2d4e` - Mejorar cancelación de flujo de IA, integración IM e inicialización de host central
  - Mejora de mecanismo de cancelación de flujo en ContextManager
  - Mejora de integración del sistema IM
  - Optimización de inicialización de CoreHost
  - Mejora de funcionalidad de DiskExecutor
  - Actualización de WebUIProvider

#### Soporte de Carga de Archivos
- `28fb344` - Implementar diálogo de origen de archivo y soporte de carga de archivos
  - Diálogo de origen de archivo en interfaz web
  - Implementación de función de carga de archivos
- `1d3e2cc` - Añadir cadenas de localización para diálogo de origen de archivo
  - Soporte multiidioma para diálogo de origen de archivo (6 idiomas)

#### Cola de Mensajes de Chat
- `db48c51` - Añadir cola de mensajes de chat, metadatos de archivo y soporte de cancelación de flujo
  - Nuevo sistema de cola de mensajes ChatMessageQueue
  - Nueva gestión de metadatos de archivo FileMetadata
  - Nuevo gestor de cancelación de flujo StreamCancellationManager

### 2026-04-22

#### Mejora de Localización
- `b574b2b` - Añadir senderName para mensajes históricos para identificación de IA
  - Añadir campo de nombre de remitente en mensajes históricos SSE
  - Soporte para identificación de identidad de mensajes de IA
- `0a8d750` - Añadir indicación de sistema genérica para comportamiento proactivo de Seres Silicona
  - Nueva plantilla de indicación de sistema genérica en sistema de localización
  - Soporte para guía de comportamiento proactivo de Seres Silicona

#### Expansión del Sistema de Herramientas
- `70ce7fb` - Implementar DatabaseTool para consultas de base de datos estructurada
  - Nueva herramienta de consulta de base de datos
  - Soporte para operaciones de datos estructurados
- `be29a09` - Implementar LogTool para consulta de historial de operaciones y conversaciones
  - Nueva herramienta de consulta de registros
  - Soporte para recuperación de historial de operaciones y conversaciones
- `4ea7702` - Implementar PermissionTool para gestión dinámica de permisos
  - Nueva herramienta de gestión de permisos
  - Soporte para consulta y gestión dinámica de permisos
- `1384ff4` - Implementar ExecuteCodeTool para ejecución de código multiidioma
  - Nueva herramienta de ejecución de código
  - Soporte para compilación y ejecución de código multiidioma
- `82d1e11` - Implementar SearchTool para recuperación de información
  - Nueva herramienta de búsqueda de información
  - Soporte para recuperación de información externa

#### Refactorización del Sistema de Registros
- `8f6cb1e` - Añadir parámetro beingId a interfaz ILogger, implementar separación de registros de sistema/Seres Silicona
  - Extensión de interfaz ILogger
  - Soporte para separación de registros de sistema y Seres Silicona
  - Nuevo parámetro beingId
- `2b771f3` - Desacoplar LogController de E/S de archivos, añadir API de lectura de registros
  - Refactorización de arquitectura de LogController
  - Nueva API independiente de lectura de registros
  - Separación de operaciones de E/S de archivos
- `12da302` - Añadir filtro de Seres Silicona para vista de registros
  - Nueva función de filtro de Seres Silicona en interfaz web
  - Filtrado de registros por Ser Silicona

#### Mejora del Sistema de Permisos
- `4c747ad` - Refactorizar PermissionTool, ExecuteCodeTool, añadir API EvaluatePermission
  - Refactorización de PermissionTool y ExecuteCodeTool
  - Integración de API EvaluatePermission

#### Optimización de Interfaz Web
- `702b3f3` - Mejorar vista de tareas, añadir insignias de estado y visualización de metadatos
  - Mejora de UI de vista de tareas
  - Nueva visualización de insignias de estado y metadatos
- `6ed9a79` - Mejorar almacenamiento de mensajes de chat y renderizado de vistas
  - Optimización de mecanismo de almacenamiento de mensajes de chat
  - Mejora de rendimiento de renderizado de vistas
- `0675c45` - Optimizar resaltado de bloques de código markdown en panel de vista previa
  - Optimización de resaltado de código en vista previa Markdown
  - Mejorar efecto de visualización de bloques de código

#### Integración de Herramientas
- `135710d` - Eliminar SearchTool, mover búsqueda local a DiskTool
  - Eliminación de SearchTool
  - Integración de función de búsqueda local en DiskTool
- `7a03a19` - Mejorar flexibilidad de consulta de conversaciones de LogTool
  - Optimización de lógica de consulta de conversaciones de LogTool
  - Mejorar flexibilidad de consultas

#### Gestión de Configuración
- `4305769` - Añadir .gitattributes para gestión de finales de línea
  - Configuración de finales de línea para compatibilidad multiplataforma

#### Corrección de Bugs
- `1c96e99` - Corregir fallo de búsqueda en directorio raíz de search_files y search_content
  - Corrección de función de búsqueda en directorio raíz de DiskTool
  - Corrección de lógica de búsqueda de archivos y contenido

### 2026-04-21

#### Mejora del Sistema de Permisos
- `5879621` - Añadir verificación de precompilación de callback de permisos y mejora de manejo de errores
  - Función de verificación de precompilación de callback de permisos
  - Compilar código de callback de permisos antes de guardar para verificar corrección
  - Prevenir que código inválido se guarde en disco
  - Manejo detallado de errores para operaciones de guardado de permisos
  - Mensajes de error mejorados, soportan localización
  - Separación de pasos de compilación y escaneo de seguridad, proporciona mejor informe de errores
- `833ead2` - Añadir verificación de referencias de ensamblado para compilación dinámica
  - Mejora de seguridad de compilación dinámica
  - Mecanismo de verificación de referencias de ensamblado

#### Mejora de Interfaz Web
- `0a826f5` - Añadir indicación de guardado exitoso en editor de código
  - Función de indicación de éxito para operación de guardado en editor de código
  - Mejorar retroalimentación de operación de usuario
- `2940373` - Mejorar interfaz web, añadir indicación flotante de código y mejora de UI
  - Función de indicación flotante en editor de código
  - Optimización de UI de interfaz web
- `6ba591d` - Añadir editor de configuración de IA independiente para Seres Silicona
- `634e8ca` - Añadir enlace de retorno a lista en página de permisos
- `188c6f8` - Registrar ruta de API de lista de tareas y añadir visualización de estado vacío

#### Corrección de Bugs
- `592c7ab` - Corregir orden de instanciación y registro de callbacks
  - Corrección de sistema de callback de permisos
  - Optimización de orden de registro de callbacks
- `c6b518b` - Corregir传递 de mensajes de temporizador y almacenamiento de mensajes de chat
  - Corrección de mecanismo de传递 de mensajes de temporizador
  - Optimización de almacenamiento de mensajes de chat

#### Localización
- `7940d9c` - Añadir soporte de localización en coreano
  - Archivos de localización en coreano
  - Mejora de sistema multiidioma
- `4ff98ad` - Refactorizar documentos, soportar multiidioma
  - Reorganización de estructura de documentos
  - Sincronización de documentos multiidioma

#### IA y Calendario
- `646813e` - Mejorar implementación de fábrica de clientes de IA
  - Refactorización de fábrica de clientes de IA
  - Optimización de mecanismo de descubrimiento de clientes
- `928a96d` - Corregir implementación de cálculo de calendario
  - Corrección de lógica de cálculo de calendario
  - Mejora de precisión de cálculo para múltiples tipos de calendario

#### Configuración y Punto de Entrada
- `0fc1693` - Actualizar punto de entrada del programa y configuración del proyecto
  - Optimización de punto de entrada del programa
  - Mejora de configuración del proyecto

### 2026-04-20

#### Perfeccionamiento de Funciones Centrales
- `28905b5` - Soporte multiidioma completo, fábrica de clientes de IA, sistema de permisos y configuración de localización
  - Sistema de registros con gestor, entradas y diferentes niveles de registros
  - Sistema de auditoría de tokens para consulta y seguimiento de uso de tokens
  - Fábrica de clientes de IA con descubrimiento automático de diferentes plataformas de IA
  - Sistema de callback de permisos con almacenamiento propio
  - Implementación de registrador de consola
  - Soporte multiidioma para inglés y chino simplificado
  - Mensajero WebUI con WebSocket para chat en tiempo real
  - Ser Silicona predeterminado mejorado con localización

### 2026-04-19

#### Temporizadores y Calendario
- `c933fd8` - Actualizar localización, sistema de temporizadores, vistas web y añadir herramientas
  - Mejor gestor de localización
  - Sistema de programación para tareas programadas
  - Configuración de IA y gestión de contexto
  - Herramienta de calendario con soporte para 32 tipos de calendario
  - Controlador web para API de calendario
  - Herramienta de gestión de tareas

**Mejoras de Arquitectura**
- Rediseñar arquitectura de vistas web para mejor soporte de pieles
- Mejorar sistema de gestión de seres con mejor manejo de estados

### 2026-04-18

- `9f585e1` - Actualizar localización, sistema de temporizadores, vistas web y añadir herramientas
  - Mejoras de temporizadores y programación
  - Mejores vistas web con componentes de UI mejorados
  - Más implementaciones de herramientas

### 2026-04-17

- `9b71fcd` - Actualizar módulos centrales, añadir documentos zh-HK, canales de broadcast, herramienta de configuración y vista web de auditoría
  - Canales de broadcast para que múltiples Seres Silicona chateen juntos
  - Sistema de herramienta de configuración
  - Vista web de auditoría
  - Documentos en chino tradicional

### 2026-04-16

- `5040f05` - Actualizar módulos centrales y predeterminados
  - Optimización de módulos y corrección de bugs
  - Actualizaciones y mejoras de implementación

### 2026-04-15

- `3efab5f` - Actualizar múltiples módulos: AI, Chat, IM, Tools, Web, Localization, Storage
  - Mejoras de clientes de IA
  - Mejoras de sistema de chat
  - Actualizaciones de proveedores de mensajería
  - Optimización de sistema de herramientas
  - Mejoras de infraestructura web
  - Optimización de localización
  - Actualizaciones de sistema de almacenamiento

### 2026-04-14

- `4241a2f` - Funciones de chat básicamente completadas, optimización de carga de UI
  - Finalización de funciones de sistema de chat
  - Optimización de UI para carga de archivos

### 2026-04-13

- `c498c31` - Actualización de código
  - Mejoras y optimizaciones generales de código

### 2026-04-12

- `2161002` - Refactorizar documentos y mejorar localización
  - Reorganización de documentos
  - Mejoras de sistema de localización
- `03d94e4` - Mejorar sistema de configuración y localización
  - Mejoras de sistema de configuración
  - Soporte de idiomas adicionales
- `9976a35` - Añadir página Acerca de y localización
  - Página Acerca de
  - Mejora de localización
- `0c8ccfc` - Mejorar sistema de chat, localización y vistas web
  - Mejoras de sistema de chat
  - Actualizaciones de localización
  - Mejora de vistas web
- `a8f1342` - Rediseñar capa de comunicación web, cambiar de WebSocket a SSE
  - Comunicación web ahora usa eventos enviados por servidor

### 2026-04-11

- `e8fe259` - Añadir sistema de registros y optimización de código
  - Infraestructura de registros
  - Implementación de registradores
- `f01c519` - Añadir sistema de registros, actualizar interfaces de IA y vistas web
  - Actualizaciones de interfaces de IA
  - Mejoras de vistas web

### 2026-04-10

- `4962924` - Mejorar manejador WebSocket, vista de chat e interacción de mensajería
  - Mejoras de ContextManager
  - Mejoras de sistema de chat
  - Actualizaciones de interfaz de proveedor de mensajería
  - Rediseño de proveedor WebUI
  - Actualizaciones de constructor JavaScript y router
  - Optimización de vista de chat
  - Mejoras de manejador WebSocket

### 2026-04-09

- `f9302bf` - Mejorar interfaz de proveedor de mensajería, sistema de chat e interacción de Web UI
  - Extensión de interfaz de proveedor de mensajería
  - Mejoras de mensajes y sistema de chat
  - Optimización de ContextManager
  - Mejora de Ser Silicona predeterminado
  - Mejoras de vista de chat de Web UI
  - Actualización de manejador WebSocket

### 2026-04-07

- `6831ee8` - Rediseñar vistas web y constructor JavaScript
  - Rediseño completo de controladores web
  - Reescritura completa de constructor JavaScript
  - Actualización de todos los componentes de vista (vista de seres, vista de chat, vista de explorador de código, vista de configuración, etc.)
  - Mejoras de sistema de pieles (pieles de administración, chat, creatividad, desarrollo)
  - Mejora de arquitectura de clase base de vistas

### 2026-04-05

- `41e97fb` - Actualizar múltiples módulos centrales y controladores web
  - Mejoras de ContextManager
  - Sistema de chat y gestión de sesiones
  - Rediseño de localizador de servicios
  - Actualizaciones de clase base y gestor de Seres Silicona
  - Actualización completa de controladores web (17 controladores)
  - Mejora de fábrica de Seres Silicona predeterminados
- `67988d4` - Mejorar módulos de Web UI, añadir vista de ejecutores, limpiar vistas y módulos centrales
  - Vista de ejecutores
  - Limpieza y organización de módulos

### 2026-04-04

- `b58bb1c` - Añadir controlador de inicialización y rediseñar módulos web
  - Controlador de inicialización
  - Rediseño de módulo de configuración
  - Actualización de módulo de localización
  - Mejoras de sistema de pieles
  - Mejora de router
- `f03ac0b` - Añadir módulo de Web UI, mejorar funcionalidad de mensajería
  - Módulo de Web UI
  - Mejoras de funcionalidad de mensajería

### 2026-04-03

- `192e57b` - Actualizar estructura de proyecto y componentes centrales de runtime
  - Actualizaciones de sistema de runtime
  - Mejoras de estructura de proyecto
- `59faec8` - Actualizaciones de implementación central y predeterminada
  - Mejoras de módulos centrales
  - Actualizaciones de implementación predeterminada
- `d488485` - Añadir función de compilación dinámica y módulo de herramientas del curador
  - Ejecutor de compilación dinámica
  - Implementación de herramientas del curador
- `753d1d9` - Añadir módulo de seguridad, actualizar ejecutores, proveedores de mensajería, localización y herramientas
  - Sistema de seguridad
  - Actualizaciones de ejecutores
  - Mejoras de proveedores de mensajería
  - Mejoras de localización
  - Actualizaciones de sistema de herramientas
- `a378697` - Completar Fase 5 - Sistema de herramientas + Ejecutores
  - Gestión y definición de herramientas
  - Ejecutor de línea de comandos
  - Ejecutor de disco
  - Ejecutor de red
  - Implementaciones de herramientas

### 2026-04-02

- `e6ad94b` - Corregir fallo de carga de historial de chat al eliminar archivo de configuración durante pruebas
  - Mejoras de manejo de errores de cliente Ollama
  - Validación de datos de configuración
  - Limpieza de referencias de proyecto
- `daa56f5` - Completar Fase 4: Memoria persistente (sistema de chat + canal de mensajería)
  - Sistema de chat con chat grupal y privado
  - Interfaces de proveedor y gestor de mensajería
  - Almacenamiento indexado por tiempo
  - Manejo de fechas incompletas
  - Almacenamiento de tiempo de sistema de archivos
  - Proveedor de mensajería de consola
  - Mejora de fábrica de Seres Silicona
  - Actualización de inicialización de programa

### 2026-04-01

- `bbe2dbb` - Corregir carga de configuración y enrutamiento de mensajes de servicio de chat
  - Implementación de ContextManager (añadir 188 líneas de código)
  - Interfaces de cliente de IA
  - Interfaces e implementación simple de servicio de chat
  - Sistema de configuración con convertidores
  - Mejoras de programador de bucle principal
  - Sistema de gestión de Seres Silicona
  - Implementación de cliente Ollama
  - Configuración de sistema de localización
  - Rediseño de inicialización de programa
- `2fa6305` - Implementar Fase 2: Framework de bucle principal y sistema de objetos de reloj
  - Programador de bucle principal
  - Clase base de objetos de reloj
  - Interfaz de fábrica de clientes de IA
  - Interfaces de almacenamiento
  - Fábrica de cliente Ollama
  - Objeto de reloj de consola
  - Objeto de reloj de prueba
  - Almacenamiento de sistema de archivos
- `32b99a1` - Implementar Fase 1 - Función básica de chat
  - Modelos de solicitud y respuesta de IA
  - Interfaces de cliente de IA
  - Modelos de mensajes
  - Sistema de localización (idioma, clase base de localización, gestor de localización)
  - Clase base de datos de configuración
  - Implementación de cliente Ollama
  - Gestión de configuración
  - Localización multiidioma (inglés, chino simplificado)
  - Punto de entrada de programa
- `358e368` - Commit inicial: documentos de proyecto y licencia
  - README del proyecto (inglés y chino)
  - Documentos de arquitectura
  - Documentos de hoja de ruta
  - Documentos de seguridad
  - Licencia (Apache 2.0)
  - Configuración de Git ignore
