# Arquitectura

> **Versión: v0.2.0-alpha**

[English](../en/architecture.md) | [Deutsch](../de-DE/architecture.md) | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | **Español** | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Čeština](../cs-CZ/architecture.md) | [Русский](../ru-RU/architecture.md)

## Arquitectura de Doble Versión

Este proyecto proporciona dos versiones de implementación que comparten el mismo diseño arquitectónico, pero difieren en almacenamiento y optimización de rendimiento:

### SiliconLife.Default (Versión por defecto)
- **Posicionamiento**: Implementación por defecto, principalmente para verificar la viabilidad de la arquitectura
- **Modo de ejecución**: Aplicación de consola
- **Método de almacenamiento**: Almacenamiento JSON puro en sistema de archivos
- **Escenarios aplicables**: Alta seguridad de datos, recursos de memoria limitados, volúmenes de datos pequeños
- **Descripción del rol**: Como implementación de referencia para verificación de arquitectura, proporciona una forma de ejecución simple y fiable, adecuada para primeros contactos, depuración de desarrollo o escenarios donde la seguridad de datos es prioritaria

### SiliconLife.Fast (Versión de alto rendimiento)
- **Posicionamiento**: Versión de producción recomendada
- **Modo de ejecución**: Aplicación de escritorio (bandeja del sistema en Windows / ventana de estado en Linux)
- **Método de almacenamiento**: Almacenamiento en memoria SpeedyPack + persistencia por lotes asíncrona (formato de archivo .spk)
- **Escenarios aplicables**: Alta concurrencia, baja latencia, grandes volúmenes de datos
- **Soporte de plataforma**: Windows/macOS (funcionalidad completa, incluyendo bandeja del sistema), Linux (ventana de estado, sin icono en bandeja)
- **Características**:
  - Ejecución en segundo plano en la bandeja del sistema de Windows/macOS, ventana de estado de la bandeja para monitoreo en tiempo real; ventana de estado directa en Linux
  - Motor SpeedyPack + compactación automática para garantizar la seguridad de los datos
  - Arquitectura Component UI, 27 componentes declarativos
  - 7 temas de piel, con soporte para descubrimiento y cambio automático
  - Herramienta de recarga en caliente para actualización y reinicio en línea
  - Linux abre automáticamente el navegador para acceder a la Web UI, soporta el parámetro `--no-tray`
- **Mejora de rendimiento**: Latencia de lectura de almacenamiento reducida 1000 veces, latencia de escritura reducida 15000 veces
- **Descripción del rol**: Implementación de grado de producción profundamente optimizada, con características como ejecución en segundo plano en la bandeja del sistema, motor SpeedyPack + compactación automática, es la opción preferida para ejecución a largo plazo y entornos de producción reales

> **Nota**: La arquitectura descrita en este documento aplica a ambas versiones, difiriendo solo en la implementación de almacenamiento. SiliconLife.Default sirve como referencia de verificación arquitectónica, SiliconLife.Fast como versión de producción recomendada.

---

## Conceptos Centrales

### Ser de Silicio

Cada agente de IA en el sistema es un **Ser de Silicio** — una entidad autónoma con su propia identidad, personalidad y capacidades. Cada Ser de Silicio está impulsado por un **Archivo de Alma** (prompt en Markdown) que define sus patrones de comportamiento.

### Curador de Silicio

El **Curador de Silicio** es un Ser de Silicio especial con los permisos más altos del sistema. Actúa como administrador del sistema:

- Crea y gestiona otros Seres de Silicio
- Analiza las solicitudes de los usuarios y las descompone en tareas
- Distribuye tareas a los Seres de Silicio apropiados
- Monitorea la calidad de ejecución y maneja fallos
- Responde a los mensajes de los usuarios usando **programación prioritaria** (ver más abajo)

### Archivo de Alma

Un archivo Markdown almacenado en el directorio de datos de cada Ser de Silicio (`soul.md`). Se inyecta como prompt del sistema en cada solicitud de IA, definiendo la personalidad del ser, patrones de decisión y restricciones de comportamiento.

---

## Programación: Programación Justa por Intervalos de Tiempo

### Bucle Principal + Objetos de Reloj

El sistema ejecuta un **bucle principal impulsado por reloj** en un hilo dedicado en segundo plano:

```
Bucle Principal (hilo dedicado, Perro Guardián + Interruptor de Circuito)
  └── Objeto de Reloj A (prioridad=0, intervalo=100ms)
  └── Objeto de Reloj B (prioridad=1, intervalo=500ms)
  └── Gestor de Seres de Silicio (activado por reloj del bucle principal)
        └── Ejecutor de Ser → Ser 1 → Activación de reloj → Ejecutar una ronda
        └── Ejecutor de Ser → Ser 2 → Activación de reloj → Ejecutar una ronda
        └── Ejecutor de Ser → Ser 3 → Activación de reloj → Ejecutar una ronda
        └── ...
```

Decisiones de diseño clave:

- **Los Seres de Silicio no heredan de objetos de reloj.** Tienen su propio método `Tick()`, llamado por `SiliconBeingManager` a través de `SiliconBeingRunner`, en lugar de registrarse directamente en el bucle principal.
- **SiliconBeingManager** es activado por el reloj del bucle principal y actúa como proxy único para todos los seres.
- **SiliconBeingRunner** envuelve el `Tick()` de cada ser en un hilo temporal, con tiempo de espera e interruptor de circuito por ser (3 tiempos de espera consecutivos → 1 minuto de enfriamiento).
- La ejecución de cada ser se limita a **una ronda** de solicitud de IA + llamadas a herramientas por activación de reloj, asegurando que ningún ser pueda monopolizar el bucle principal.
- **PerformanceMonitor** rastrea los tiempos de ejecución del reloj para observabilidad.

### Respuesta Prioritaria del Curador

Cuando un usuario envía un mensaje al Curador de Silicio:

1. El ser actual (por ejemplo, Ser A) completa su ronda actual — **sin interrupción**.
2. El gestor **salta el resto de la cola**.
3. El bucle **recomienza desde el Curador**, haciéndolo ejecutar inmediatamente.

Esto garantiza capacidad de respuesta a las interacciones del usuario sin interrumpir tareas en curso.

---

## Arquitectura de Componentes

```
┌─────────────────────────────────────────────────────────┐
│                      Host Principal                      │
│  (Host unificado — ensambla y gestiona todos los         │
│   componentes)                                          │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ Bucle    │  │ Localizador  │  │   Configuración   │  │
│  │ Principal│  │ de Servicios │  │                   │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │       Gestor de Seres de Silicio (Objeto de Reloj)│   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Curador   │ │ Ser A   │ │ Ser B   │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              Servicios Compartidos                 │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Sistema   │  │Almacena- │  │  Gestor de       │  │   │
│  │  │de Chat   │  │ miento   │  │  Permisos        │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │Cliente   │  │Ejecutores│  │  Gestor de       │  │   │
│  │  │de IA     │  │          │  │  Herramientas    │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  │  ┌──────────┐ ┌──────────┐                        │   │
│  │  │Cargador  │  │Red de    │                        │   │
│  │  │de Plugins│  │Conocimiento│                      │   │
│  │  └──────────┘ └──────────┘                        │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  Ejecutores                        │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Ejecutor │  │ Ejecutor │  │  Ejecutor de     │  │   │
│  │  │ de Disco │  │ de Red   │  │  Línea de Comandos│  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │           Proveedores de Mensajería Instantánea   │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Proveedor │  │Proveedor │  │  Proveedor       │  │   │
│  │  │de Consola│  │  Web     │  │  Feishu / ...    │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## Localizador de Servicios

`ServiceLocator` es un registro singleton seguro para hilos que proporciona acceso a todos los servicios principales:

| Propiedad | Tipo | Descripción |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | Gestor central de sesiones de chat |
| `IMManager` | `IMManager` | Enrutador de proveedores de mensajería instantánea |
| `AuditLogger` | `AuditLogger` | Pista de auditoría de permisos |
| `GlobalAcl` | `GlobalACL` | Lista de control de acceso global |
| `BeingFactory` | `ISiliconBeingFactory` | Fábrica para crear seres |
| `BeingManager` | `SiliconBeingManager` | Gestor de ciclo de vida de seres activos |
| `DynamicBeingLoader` | `DynamicBeingLoader` | Cargador de compilación dinámica |
| `TokenUsageAudit` | `ITokenUsageAudit` | Seguimiento de uso de tokens |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Informes de uso de tokens |

También mantiene un registro de `PermissionManager` por ser, indexado por el GUID del ser.

---

## Sistema de Chat

### Tipos de Sesión

El sistema de chat soporta tres tipos de sesión a través de `SessionBase`:

| Tipo | Clase | Descripción |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | Conversación uno a uno entre dos participantes |
| `GroupChat` | `GroupChatSession` | Chat grupal multi-participante |
| `Broadcast` | `BroadcastChannel` | Canal abierto con ID fijo; los seres se suscriben dinámicamente, solo reciben mensajes tras la suscripción |

### Canal de Difusión

`BroadcastChannel` es un tipo de sesión especial para anuncios de todo el sistema:

- **ID de canal fijo** — A diferencia de `SingleChatSession` y `GroupChatSession`, el ID del canal es una constante conocida, no derivada de los GUID de los miembros.
- **Suscripción dinámica** — Los seres se suscriben/desuscriben en tiempo de ejecución; solo reciben mensajes publicados después de su suscripción.
- **Filtrado de mensajes pendientes** — `GetPendingMessages()` solo devuelve mensajes publicados después del tiempo de suscripción del ser y que aún no han sido leídos.
- **Gestionado por el sistema de chat** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### Mensajes de Chat

El modelo `ChatMessage` contiene campos para contexto de conversación de IA y seguimiento de tokens:

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `Id` | `Guid` | Identificador único del mensaje |
| `SenderId` | `Guid` | Identificador único del remitente |
| `ChannelId` | `Guid` | Identificador del canal/conversación |
| `Content` | `string` | Contenido del mensaje |
| `Timestamp` | `DateTime` | Hora de envío del mensaje |
| `Type` | `MessageType` | Texto, imagen, archivo o notificación del sistema |
| `ReadBy` | `List<Guid>` | IDs de participantes que han leído este mensaje |
| `Role` | `MessageRole` | Rol de conversación de IA (usuario, asistente, herramienta) |
| `ToolCallId` | `string?` | ID de llamada a herramienta para mensajes de resultado de herramienta |
| `ToolCallsJson` | `string?` | JSON serializado de llamadas a herramientas para mensajes del asistente |
| `Thinking` | `string?` | Cadena de razonamiento de la IA |
| `PromptTokens` | `int?` | Cantidad de tokens en el prompt (entrada) |
| `CompletionTokens` | `int?` | Cantidad de tokens en la completación (salida) |
| `TotalTokens` | `int?` | Cantidad total de tokens usados (entrada + salida) |
| `FileMetadata` | `FileMetadata?` | Metadatos de archivo adjunto (si el mensaje contiene un archivo) |

### Cola de Mensajes de Chat

`ChatMessageQueue` es un sistema de cola de mensajes seguro para hilos que gestiona el procesamiento asíncrono de mensajes de chat:

- **Seguro para hilos** - Usa mecanismos de bloqueo para garantizar acceso concurrente seguro
- **Procesamiento asíncrono** - Soporta encolado y desencolado asíncrono de mensajes
- **Ordenamiento de mensajes** - Mantiene el orden temporal de los mensajes
- **Operaciones por lotes** - Soporta obtención por lotes de mensajes

### Metadatos de Archivo

`FileMetadata` gestiona la información de archivos adjuntos a mensajes de chat:

- **Información del archivo** - Nombre, tamaño, tipo, ruta del archivo
- **Hora de carga** - Marca de tiempo de carga del archivo
- **Cargador** - ID del usuario o Ser de Silicio que cargó el archivo

### Gestor de Cancelación de Flujo

`StreamCancellationManager` proporciona un mecanismo de cancelación para respuestas de flujo de IA:

- **Control de flujo** - Soporta cancelación de respuestas de flujo de IA en curso
- **Limpieza de recursos** - Limpia correctamente los recursos relacionados al cancelar
- **Seguridad de concurrencia** - Soporta gestión simultánea de múltiples flujos

### Visualización de Historial de Chat

La nueva función de visualización de historial de chat permite a los usuarios navegar las conversaciones históricas de los Seres de Silicio:

- **Lista de sesiones** - Muestra todas las sesiones históricas
- **Detalles de mensajes** - Ver historial completo de mensajes
- **Vista de línea temporal** - Muestra mensajes en orden cronológico
- **Soporte de API** - Proporciona API RESTful para obtener datos de sesiones y mensajes

---

## Sistema de Clientes de IA

El sistema soporta múltiples backends de IA a través de la interfaz `IAIClient`:

### OllamaClient

- **Tipo**: Servicio de IA local
- **Protocolo**: API HTTP nativa de Ollama (`/api/chat`, `/api/generate`)
- **Funcionalidades**: Streaming, llamadas a herramientas, alojamiento de modelos locales
- **Configuración**: `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (Bailian de Alibaba Cloud)

- **Tipo**: Servicio de IA en la nube
- **Protocolo**: API compatible con OpenAI (`/compatible-mode/v1/chat/completions`)
- **Autenticación**: Bearer token (clave API)
- **Funcionalidades**: Streaming, llamadas a herramientas, contenido de razonamiento (cadena de pensamiento), despliegue multirregión
- **Regiones soportadas**:
  - `beijing` — Norte de China 2 (Pekín)
  - `virginia` — EE.UU. (Virginia)
  - `singapore` — Singapur
  - `hongkong` — Hong Kong, China
  - `frankfurt` — Alemania (Fráncfort)
- **Modelos soportados** (descubiertos dinámicamente a través de API, con lista de respaldo):
  - **Serie Qwen**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **Razonamiento**: qwq-plus
  - **Terceros**: deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **Configuración**: `apiKey`, `region`, `model`
- **Descubrimiento de modelos**: Obtiene modelos disponibles de la API de Bailian en tiempo de ejecución; retrocede a lista curada en caso de fallo de red

### VolcengineArkClient (Volcengine Ark)

- **Tipo**: Servicio de IA en la nube
- **Protocolo**: API compatible con OpenAI
- **Autenticación**: Bearer token (clave API)
- **Funcionalidades**: Soporta modos de streaming y no streaming, control de velocidad de doble capa integrado
  - Auto-control de velocidad: Aplica intervalo mínimo entre solicitudes
  - Límite de velocidad del servidor: Maneja errores 429, reintento con retroceso exponencial
- **Configuración**: `apiKey`, `endpoint`, `model`
- **Características**: Servicio de IA de ByteDance, soporta múltiples modelos Doubao

### Patrón de Fábrica de Clientes

Cada tipo de cliente de IA tiene una implementación de fábrica correspondiente `IAIClientFactory`:

- `OllamaClientFactory` — Crea instancias de OllamaClient
- `DashScopeClientFactory` — Crea instancias de DashScopeClient
- `VolcengineArkClientFactory` — Crea instancias de VolcengineArkClient

Las fábricas proporcionan:
- `CreateClient(Dictionary<string, object> config)` — Instancia un cliente desde la configuración
- `GetConfigKeyOptions(string key, ...)` — Devuelve opciones dinámicas para claves de configuración (por ejemplo, modelos disponibles, regiones)
- `GetDisplayName()` — Nombre de visualización localizado del tipo de cliente

### Lista de Soporte de Plataformas de IA

#### Descripción de Estados
- ✅ Implementado
- 🚧 En desarrollo
- 📋 Planificado
- 💡 En consideración

*Nota: Debido al entorno de red del desarrollador, la conexión a servicios de IA en la nube extranjeros marcados como [En consideración] puede requerir herramientas de proxy de red, y el proceso de depuración puede ser inestable.*

#### Lista de Plataformas

| Plataforma | Estado | Tipo | Descripción |
|------|------|------|------|
| Ollama | ✅ | Local | Servicio de IA local, soporta despliegue de modelos locales |
| DashScope (Bailian de Alibaba Cloud) | ✅ | Nube | Servicio de IA Bailian de Alibaba Cloud, soporta despliegue multirregión |
| Baidu Qianfan (Wenxin) | 📋 | Nube | Servicio de IA Wenxin de Baidu |
| Zhipu AI (GLM) | 📋 | Nube | Servicio de IA Zhipu Qingyan |
| Moonshot (Kimi) | 📋 | Nube | Servicio de IA Kimi de Moonshot |
| Volcengine Ark Doubao | ✅ | Nube | Servicio de IA Doubao de ByteDance |
| DeepSeek (conexión directa) | 📋 | Nube | Servicio de IA DeepSeek |
| 01.AI (Yi) | 📋 | Nube | Servicio de IA 01.AI |
| Tencent Hunyuan | 📋 | Nube | Servicio de IA Hunyuan de Tencent |
| SiliconFlow | 📋 | Nube | Servicio de IA SiliconFlow |
| MiniMax | 📋 | Nube | Servicio de IA MiniMax |
| OpenAI | 💡 | Nube | Servicio API de OpenAI (serie GPT) |
| Anthropic | 💡 | Nube | Servicio de IA Claude de Anthropic |
| Google DeepMind | 💡 | Nube | Servicio de IA Gemini de Google |
| Mistral AI | 💡 | Nube | Servicio de IA Mistral |
| Groq | 💡 | Nube | Servicio de inferencia de IA de alta velocidad Groq |
| Together AI | 💡 | Nube | Servicio de modelos de código abierto Together AI |
| xAI | 💡 | Nube | Servicio xAI Grok |
| Cohere | 💡 | Nube | Servicio de NLP empresarial Cohere |
| Replicate | 💡 | Nube | Plataforma de alojamiento de modelos de código abierto Replicate |
| Hugging Face | 💡 | Nube | Comunidad de IA de código abierto y plataforma de modelos Hugging Face |
| Cerebras | 💡 | Nube | Servicio de inferencia de IA optimizada Cerebras |
| Databricks | 💡 | Nube | Plataforma de IA empresarial Databricks (MosaicML) |
| Perplexity AI | 💡 | Nube | Servicio de búsqueda y respuestas de IA Perplexity |
| NVIDIA NIM | 💡 | Nube | Microservicios de inferencia de IA NVIDIA |

---

## Decisiones de Diseño Clave

### Almacenamiento como Clase de Instancia (no estática)

`IStorage` está diseñado como una instancia inyectable, no como una utilidad estática. Esto asegura:

- Acceso directo al sistema de archivos — IStorage es el canal de persistencia interno del sistema, **no** se enruta a través de ejecutores.
- **La IA no puede controlar IStorage** — Los ejecutores gestionan el IO iniciado por herramientas de IA; IStorage gestiona la lectura/escritura de datos internos del framework. Son preocupaciones fundamentalmente diferentes.
- Se pueden usar implementaciones simuladas para pruebas.
- Soporte futuro para diferentes backends de almacenamiento sin modificar a los consumidores.

### Ejecutores como Frontera de Seguridad

Los ejecutores son la **única** ruta para operaciones de E/S. Las herramientas que necesitan acceso a disco, red o línea de comandos **deben** pasar por ejecutores. Este diseño aplica:

- Cada ejecutor tiene su **propio hilo de programación independiente**, con bloqueo de hilo para verificación de permisos.
- Verificación centralizada de permisos — Los ejecutores consultan el **Gestor de Permisos privado** del ser.
- Cola de solicitudes con soporte para prioridad y control de tiempo de espera.
- Registro de auditoría para todas las operaciones externas.
- Aislamiento de excepciones — El fallo de un ejecutor no afecta a otros.
- Interruptor de circuito — Fallos consecutivos detienen temporalmente el ejecutor para prevenir fallos en cascada.

### ContextManager como Objeto Ligero

Cada `ExecuteOneRound()` crea una nueva instancia de `ContextManager`:

1. Carga el Archivo de Alma + historial de chat reciente.
2. Envía la solicitud al cliente de IA.
3. Procesa en bucle las llamadas a herramientas hasta que la IA devuelve texto puro.
4. Persiste la respuesta en el sistema de chat.
5. Se libera.

Esto mantiene cada ronda aislada y sin estado.

### Autoevolución mediante Reescritura de Clases

Los Seres de Silicio pueden reescribir sus propias clases C# en tiempo de ejecución:

1. La IA genera código de nueva clase (debe heredar de `SiliconBeingBase`).
2. **Control de referencias en tiempo de compilación** (defensa principal): El compilador solo obtiene la lista de ensamblados permitidos — `System.IO`, `System.Reflection`, etc. están excluidos, por lo que el código peligroso es imposible a nivel de tipo.
3. **Análisis estático en tiempo de ejecución** (defensa secundaria): `SecurityScanner` escanea el código en busca de patrones peligrosos tras una compilación exitosa.
4. Roslyn compila el código en memoria.
5. En caso de éxito: `SiliconBeingManager.ReplaceBeing()` intercambia la instancia actual, migra el estado y persiste el código cifrado en disco.
6. En caso de fallo: Se descarta el nuevo código, se conserva la implementación existente.

Las implementaciones personalizadas de `IPermissionCallback` también pueden compilarse e inyectarse a través de `ReplacePermissionCallback()`, permitiendo a los seres personalizar su propia lógica de permisos.

El código se almacena cifrado en disco con AES-256. La clave de cifrado se deriva del GUID del ser (en mayúsculas) mediante PBKDF2.

---

## Auditoría de Uso de Tokens

`TokenUsageAuditManager` rastrea el consumo de tokens de IA de todos los seres:

- `TokenUsageRecord` — Registro por solicitud (ID del ser, modelo, tokens de prompt, tokens de completación, marca de tiempo)
- `TokenUsageSummary` — Estadísticas agregadas
- `TokenUsageQuery` — Parámetros de consulta para filtrar registros
- Persistido a través de `ITimeStorage` para consultas de series temporales
- Accesible a través de Web UI (UsageController) y `TokenAuditTool` (solo Curador)

---

### Sistema de Calendario

El sistema incluye **32 implementaciones de calendario**, derivadas de la clase abstracta `CalendarBase`, cubriendo los principales sistemas de calendario del mundo:

| Calendario | ID | Descripción |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | Calendario budista (BE), año + 543 |
| CherokeeCalendar | `cherokee` | Sistema de calendario cherokee |
| ChineseLunarCalendar | `lunar` | Calendario lunar chino, con meses intercalares |
| ChineseHistoricalCalendar | `chinese_historical` | Calendario histórico chino, soporta ciclos sexagenarios y eras imperiales |
| ChulaSakaratCalendar | `chula_sakarat` | Calendario Chula Sakarat (CS), año - 638 |
| CopticCalendar | `coptic` | Calendario copto |
| DaiCalendar | `dai` | Calendario Dai, con cálculo lunar completo |
| DehongDaiCalendar | `dehong_dai` | Variante del calendario Dai de Dehong |
| EthiopianCalendar | `ethiopian` | Calendario etíope |
| FrenchRepublicanCalendar | `french_republican` | Calendario republicano francés |
| GregorianCalendar | `gregorian` | Calendario gregoriano estándar |
| HebrewCalendar | `hebrew` | Calendario hebreo (judío) |
| IndianCalendar | `indian` | Calendario nacional indio |
| InuitCalendar | `inuit` | Sistema de calendario inuit |
| IslamicCalendar | `islamic` | Calendario islámico (Hijri) |
| JapaneseCalendar | `japanese` | Calendario japonés de eras (Nengo) |
| JavaneseCalendar | `javanese` | Calendario javanés islámico |
| JucheCalendar | `juche` | Calendario Juche (Corea del Norte), año - 1911 |
| JulianCalendar | `julian` | Calendario juliano |
| KhmerCalendar | `khmer` | Calendario jemer |
| MayanCalendar | `mayan` | Cuenta larga maya |
| MongolianCalendar | `mongolian` | Calendario mongol |
| PersianCalendar | `persian` | Calendario persa (Hijri solar) |
| RepublicOfChinaCalendar | `roc` | Calendario de la República de China, año - 1911 |
| RomanCalendar | `roman` | Calendario romano |
| SakaCalendar | `saka` | Calendario Saka (Indonesia) |
| SexagenaryCalendar | `sexagenary` | Calendario sexagenario chino (Ganzhi) |
| TibetanCalendar | `tibetan` | Calendario tibetano |
| VietnameseCalendar | `vietnamese` | Calendario lunar vietnamita (variante del gato zodiacal) |
| VikramSamvatCalendar | `vikram_samvat` | Calendario Vikram Samvat |
| YiCalendar | `yi` | Sistema de calendario Yi |
| ZoroastrianCalendar | `zoroastrian` | Calendario zoroástrico |

`CalendarTool` proporciona operaciones: `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (conversión de fechas entre calendarios).

---

## Arquitectura de Web UI

### Sistema de Pieles

La Web UI tiene un **sistema de pieles conectable** que permite personalización completa de la UI sin cambiar la lógica de la aplicación:

- **Interfaz ISkin** — Define el contrato para todas las pieles, incluyendo:
  - Métodos de renderizado principales (`RenderHtml`, `RenderError`)
  - 20+ métodos de componentes UI (botones, entradas, tarjetas, tablas, insignias, burbujas, progreso, pestañas, etc.)
  - Generación de CSS temático a través de `CssBuilder`
  - `SkinPreviewInfo` — Paleta de colores e icono para el selector de pieles en la página de inicialización

- **Pieles integradas** — 7 pieles listas para producción:
  - **Admin** — Interfaz de administración de sistema profesional, enfocada en datos
  - **Chat** — Diseño conversacional centrado en mensajes para interacción con IA
  - **Creative** — Diseño artístico y visualmente rico para flujos de trabajo creativos
  - **Dev** — Interfaz centrada en el desarrollador y el código, con resaltado de sintaxis
  - **HighContrast** — Tema de alto contraste accesible
  - **Light** — Tema claro y fresco
  - **Minimal** — Tema minimalista

- **Descubrimiento de pieles** — `SkinManager` descubre y registra automáticamente todas las implementaciones de `ISkin` mediante reflexión

### Constructores HTML / CSS / JS

La Web UI evita completamente los archivos de plantilla, generando todo el marcado en C#:

- **`H`** — DSL de constructor de HTML en flujo, para construir árboles HTML en código
- **`CssBuilder`** — Constructor de CSS, con soporte para selectores y media queries
- **`JsBuilder`** (`JsSyntax`) — Constructor de JavaScript para scripts en línea

### Sistema de Controladores

La Web UI sigue un **patrón tipo MVC**, con 24 controladores que manejan diferentes aspectos:

| Controlador | Propósito |
|------------|---------|
| About | Página de información y datos del proyecto |
| Audit | Panel de auditoría de uso de tokens |
| Being | Gestión y estado de Seres de Silicio |
| Chat | Interfaz de chat en tiempo real con SSE |
| ChatHistory | Visualización de historial de chat, con lista de sesiones y detalles de mensajes |
| CodeBrowser | Visualización y edición de código |
| CodeHover | Sugerencias flotantes de código, con resaltado de sintaxis |
| Config | Gestión de configuración del sistema |
| Dashboard | Resumen del sistema y métricas |
| Executor | Estado y gestión de ejecutores |
| Help | Sistema de documentación de ayuda, soporte multilingüe |
| Init | Asistente de inicialización para primera ejecución |
| Knowledge | Visualización y consulta del grafo de conocimiento |
| Log | Visor de registros del sistema, con filtro por Ser de Silicio |
| Memory | Navegador de memoria a largo plazo, con filtrado avanzado, estadísticas y vista de detalles |
| Permission | Gestión de permisos |
| PermissionRequest | Cola de solicitudes de permisos |
| Project | Gestión de proyectos, incluyendo notas de trabajo, sistema de tareas y permisos de herramientas |
| System | Gestión del sistema y monitoreo en tiempo de ejecución |
| Task | Interfaz del sistema de tareas |
| Timer | Gestión del sistema de temporizadores, con historial de ejecución |
| ToolPermission | Gestión de permisos de herramientas, soporta configuración de permisos a nivel de Ser de Silicio y de proyecto |
| Usage | Panel de auditoría de uso de tokens, con gráficos de tendencias y exportación |
| WorkNote | Gestión de notas de trabajo, con búsqueda y generación de directorio |

### Actualizaciones en Tiempo Real

- **SSE (Server-Sent Events)** — Actualizaciones push de mensajes de chat, estado de seres y eventos del sistema a través de `SSEHandler`
- **Sin WebSocket** — Arquitectura más simple usando SSE para la mayoría de necesidades en tiempo real
- **Reconexión automática** — Lógica de reconexión del cliente para conexiones resilientes

### Localización

El sistema soporta localización completa en **34 variantes de idioma**:
- **Chino (6 variantes)**: zh-CN (simplificado), zh-HK (tradicional), zh-SG (Singapur), zh-MO (Macao), zh-TW (Taiwán), zh-MY (Malasia)
- **Inglés (10 variantes)**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY
- **Español (2 variantes)**: es-ES, es-MX
- **Alemán (5 variantes)**: de-DE, de-AT, de-CH, de-LU, de-LI
- **Francés (3 variantes)**: fr-FR, fr-CA, fr-CH
- **Otros (8 variantes)**: ja-JP (japonés), ko-KR (coreano), cs-CZ (checo), it-IT (italiano), pl-PL (polaco), pt-PT (portugués), pt-BR (portugués brasileño), ru-RU (ruso)

El locale activo se selecciona a través de `DefaultConfigData.Language` y se resuelve mediante `LocalizationManager`.

---

### Sistema de Automatización de Navegador WebView (Nuevo)

El sistema integra funcionalidad de automatización de navegador WebView basada en **Playwright**:

- **Aislamiento individual**: Cada Ser de Silicio tiene su propia instancia de navegador, cookies y almacenamiento de sesión, completamente aislados entre sí.
- **Modo sin cabeza**: El navegador se ejecuta en modo sin cabeza completamente invisible para el usuario, los Seres de Silicio operan de forma autónoma en segundo plano.
- **WebViewBrowserTool**: Proporciona capacidades completas de operación del navegador, incluyendo:
  - Navegación de páginas, clics, entrada de texto, obtención de contenido de página
  - Ejecución de JavaScript, capturas de pantalla, espera de aparición de elementos
  - Gestión del estado del navegador y limpieza de recursos
- **Control de seguridad**: Todas las operaciones del navegador deben pasar por la cadena de verificación de permisos, previniendo acceso malicioso a páginas web.

### Sistema de Red de Conocimiento (Nuevo)

El sistema incluye un sistema de grafo de conocimiento basado en **estructura de tripletas**:

- **Representación del conocimiento**: Estructura triplete "sujeto-relación-objeto" (por ejemplo: Python-es_un-lenguaje_de_programación)
- **KnowledgeTool**: Proporciona gestión del ciclo de vida completo del conocimiento:
  - `add`/`query`/`update`/`delete` - Operaciones CRUD básicas
  - `search` - Búsqueda de texto completo y coincidencia de palabras clave
  - `get_path` - Descubrimiento de rutas de asociación entre dos conceptos
  - `validate` - Verificación de integridad del conocimiento
  - `stats` - Análisis estadístico de la red de conocimiento
- **Almacenamiento persistente**: Las tripletas de conocimiento se persisten en el sistema de archivos, soportando consultas con índice temporal.
- **Puntuación de confianza**: Cada entrada de conocimiento tiene una puntuación de confianza (0-1), soportando coincidencia difusa y ordenamiento del conocimiento.
- **Clasificación por etiquetas**: Soporta agregar etiquetas al conocimiento para facilitar la clasificación y recuperación.

---

## Estructura del Directorio de Datos

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # Archivo de Alma del Curador
    │   ├── state.json       # Estado en tiempo de ejecución
    │   ├── code.enc         # Código de clase personalizada cifrado con AES
    │   └── permission.enc   # Retrollamada de permisos personalizada cifrada con AES
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```

---

## Motor de Almacenamiento SpeedyPack

SiliconLife.Fast utiliza el motor de almacenamiento SpeedyPack propio (formato .spk), reemplazando la solución LiteDB anterior, logrando rendimiento extremo de lectura/escritura.

### Diseño de Arquitectura

```
┌──────────────────────────────────────────────────────────┐
│                    SpeedyPack                             │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────────┐  │
│  │ DirectoryMap  │  │  EntryCache   │  │  WriteQueue   │  │
│  │ (Mapeo de    │  │  (Caché de    │  │ (Cola de      │  │
│  │  directorios │  │   entradas)   │  │  escritura    │  │
│  │  en memoria) │  │              │  │  asíncrona)   │  │
│  └──────┬───────┘  └──────┬───────┘  └───────┬───────┘  │
│         │                  │                   │          │
│  ┌──────▼──────────────────▼───────────────────▼───────┐  │
│  │              PackFileReader / PackFileWriter          │  │
│  │              (Lector/Escritor de archivos de paquete) │  │
│  └──────────────────────────┬──────────────────────────┘  │
│                              │                             │
│  ┌──────────────────────────▼──────────────────────────┐  │
│  │              Archivo .spk (MessagePack + compresión LZ4)│
│  └─────────────────────────────────────────────────────┘  │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐                      │
│  │  FreeList     │  │ SpeedyPack   │                      │
│  │ (Gestión de  │  │ AutoCompactor│                      │
│  │  espacio     │  │ (Compactación│                      │
│  │  libre)      │  │  automática) │                      │
│  └──────────────┘  └──────────────┘                      │
└──────────────────────────────────────────────────────────┘
```

### Componentes Centrales

| Componente | Descripción |
|------|------|
| `SpeedyPack` | Clase principal, combina DirectoryMap, EntryCache y WriteQueue para proporcionar lectura/escritura de baja latencia |
| `DirectoryMap` | Mapeo de directorios en memoria, mantiene la relación de mapeo de rutas virtuales a entradas de archivos |
| `EntryCache` | Caché de entradas, caché de entradas accedidas recientemente basada en TTL |
| `WriteQueue` | Cola de escritura asíncrona, encola operaciones de escritura para ejecución en hilo en segundo plano |
| `FreeList` | Gestión de espacio libre, rastrea el espacio reutilizable en archivos .spk |
| `PackFileReader` | Lector de archivos de paquete, lee datos de archivos .spk |
| `PackFileWriter` | Escritor de archivos de paquete, escribe datos en archivos .spk |
| `SpeedyPackAutoCompactor` | Temporizador de compactación automática, compacta periódicamente archivos .spk para recuperar espacio libre |
| `SpeedyPackRegistry` | Gestor singleton a nivel de proceso, asegura que toda la aplicación use la misma instancia de SpeedyPack |

### Adaptadores de Almacenamiento

SiliconLife.Fast integra SpeedyPack en las interfaces del sistema a través de los siguientes adaptadores:

| Adaptador | Interfaz | Descripción |
|--------|------|------|
| `SpeedyStorage` | `IStorage` | Adaptador de almacenamiento de clave-valor genérico |
| `SpeedyTimeStorage` | `ITimeStorage` | Adaptador de almacenamiento con índice temporal |
| `SpeedyWorkNoteStorage` | `IWorkNoteStorage` | Adaptador de almacenamiento de notas de trabajo |

### Opciones de Configuración

`SpeedyPackOptions` proporciona las siguientes configuraciones:

| Opción | Tipo | Valor por defecto | Descripción |
|------|------|--------|------|
| `CacheTtl` | `TimeSpan` | 5 minutos | Tiempo de vida de las entradas de caché |
| `MaxCacheEntries` | `int` | 1000 | Número máximo de entradas de caché |
| `ReadOnly` | `bool` | false | Modo de solo lectura |

### Soporte de Transacciones

SpeedyPack soporta operaciones de escritura atómica a través de la interfaz `IPackTransaction`:

- `SpeedyTransaction` implementa el mecanismo de transacciones
- Soporta atomicidad para escrituras por lotes
- Al confirmar la transacción, todas las operaciones de escritura tienen éxito o se revierten completamente

---

## Sistema de Plugins

SiliconLife soporta extensión de funcionalidad a través de un sistema de plugins, permitiendo a desarrolladores de terceros agregar nuevas capacidades a la plataforma.

### Interfaz Central

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### Cargador de Plugins

`PluginLoader` es responsable de cargar DLLs de plugins desde directorios especificados y ejecutar verificaciones de seguridad estrictas:

1. **Escaneo de directorio** — Escanea todos los archivos .dll en el directorio de plugins
2. **Escaneo de seguridad** — Verifica si los plugins referencian espacios de nombres prohibidos
3. **Carga aislada** — Usa `AssemblyLoadContext` personalizado para cargar plugins de forma aislada
4. **Gestión del ciclo de vida** — Llama a los métodos OnLoad, OnStart, OnStop, OnUnload de los plugins

### Sandbox de Seguridad

El cargador de plugins ejecuta las siguientes verificaciones de seguridad:

| Verificación | Descripción |
|--------|------|
| Espacios de nombres prohibidos | System.IO, System.Net.Http, System.Net.WebSockets, System.Net.Sockets, Microsoft.CodeAnalysis |
| Lista blanca de ensamblados de confianza | Google.Protobuf, Newtonsoft.Json, MessagePack, Serilog, Microsoft.Extensions.Logging.Abstractions, Dapper |
| Verificación de tipos prohibidos | Escanea tipos peligrosos referenciados en el plugin |
| Verificación de miembros prohibidos | Escanea métodos peligrosos invocados en el plugin |

### Integración de Herramientas

Los plugins pueden registrar herramientas personalizadas implementando la interfaz `ITool`:

- El método `ToolManager.ScanAllPluginAssemblies()` escanea implementaciones de ITool en todos los plugins cargados
- Las herramientas de plugins se integran automáticamente en el ciclo de llamadas a herramientas
- Las herramientas de plugins están sujetas al mismo sistema de permisos

### Ciclo de Vida del Plugin

```
Carga (OnLoad) → Inicio (OnStart) → En ejecución → Detención (OnStop) → Descarga (OnUnload)
```

---

## Estados de Actividad de los Seres de Silicio

Los Seres de Silicio tienen los siguientes estados de actividad:

| Estado | Descripción |
|------|------|
| `Idle` | Estado inactivo, esperando activación del reloj |
| `SingleChat` | En chat uno a uno |
| `GroupChat` | En chat grupal |
| `Task` | Ejecutando tarea |
| `Timer` | Ejecutando temporizador |
| `Stopped` | Detenido, por errores consecutivos o detención manual |

**Mecanismo de estado Stopped**:
- Cuando un Ser de Silicio tiene 10 errores consecutivos, entra automáticamente en estado `Stopped`
- Tras entrar en estado Stopped, el ser ya no ejecutará ninguna tarea
- Cuando llega un nuevo mensaje de chat, el contador de errores se reinicia y el ser reanuda su ejecución

Transiciones de estado:
```
Idle → SingleChat → Idle (chat completado)
Idle → GroupChat → Idle (chat grupal completado)
Idle → Task → Idle (tarea completada)
Idle → Timer → Idle (temporizador completado)
Cualquiera → Stopped (10 errores consecutivos)
Stopped → Idle (nuevo mensaje de chat o reinicio manual)
```

---

## Motor de Flujos de Trabajo

El motor de flujos de trabajo es un sistema de máquina de estados basado en plantillas, utilizado para impulsar los flujos de colaboración de los Seres de Silicio en el espacio del proyecto:

### Componentes Centrales

| Componente | Descripción |
|------|------|
| `WorkflowEngine` | Núcleo del motor de flujos de trabajo, gestiona plantillas e instancias, ejecuta transiciones de estado impulsadas por Tick |
| `WorkflowTemplate` | Plantilla de flujo de trabajo, define conjuntos de estados y reglas de transición |
| `WorkflowInstance` | Instancia de flujo de trabajo, vinculada a un proyecto específico, rastrea el estado actual |
| `WorkflowLog` | Registro de flujo de trabajo, registra el historial de transiciones de estado |

### Mecanismo de Funcionamiento

- **Registro de plantillas**: Se registran plantillas de flujo de trabajo a través de `RegisterTemplate()`, definiendo estados y reglas de transición
- **Creación de instancias**: Se crean instancias desde plantillas, vinculadas al espacio del proyecto
- **Impulsado por Tick**: Las transiciones de estado son impulsadas por el mecanismo de Tick del bucle principal
- **Registro de logs**: Todas las transiciones de estado se registran automáticamente en logs

---

## Mecanismo de Desvanecimiento de Memoria

`MemoryFadeService` es un servicio de decaimiento temporal que simula la característica de olvido de la memoria biológica:

### Mecanismo de Funcionamiento

- **Ejecución temporizada**: Hereda de `TickObject`, ejecuta un ciclo de decaimiento cada hora por defecto
- **Decaimiento de importancia**: Aplica un algoritmo de decaimiento a las entradas de memoria de cada Ser de Silicio, reduciendo la puntuación de importancia
- **Archivado automático**: Las memorias con importancia por debajo del umbral se archivan automáticamente (`ArchiveFadingMemories()`)
- **Seguimiento estadístico**: Registra estadísticas como número de ciclos de decaimiento, entradas con cambio de estado, etc.

### Flujo de Decaimiento

```
MemoryFadeService.OnTick()
  └── Iterar todos los Seres de Silicio
       └── being.Memory.ApplyDecay()      # Aplicar decaimiento de importancia
       └── being.Memory.ArchiveFadingMemories()  # Archivar memorias de baja importancia
```

---

## Sistema de Espacio de Trabajo de Proyectos

El espacio de trabajo de proyectos es un mecanismo de gestión de espacios que soporta la colaboración de múltiples Seres de Silicio:

### Funcionalidades Centrales

- **Ciclo de vida del proyecto**: Creación → Activo → Archivado → Destrucción
- **Asignación de roles**: Soporta asignación de roles de proyecto a Seres de Silicio
- **Aislamiento de permisos de herramientas**: Configuración de permisos de herramientas a nivel de proyecto, independiente de los permisos a nivel del Ser de Silicio
- **Notas de trabajo**: Sistema de notas por páginas dentro del espacio del proyecto, soporta generación de directorio y búsqueda por palabras clave
- **Seguimiento de tareas**: Gestión de tareas a nivel de proyecto, soporta creación, asignación y seguimiento de estado
- **Integración de flujos de trabajo**: Los proyectos pueden vincular plantillas de flujo de trabajo para impulsar flujos de colaboración

### Herramientas Relacionadas

| Herramienta | Propósito |
|------|------|
| `ProjectTool` | Gestión del espacio del proyecto (creación, archivado, destrucción, asignación de roles) |
| `ProjectTaskTool` | Gestión de tareas del proyecto (creación, asignación, actualización de estado) |
| `ProjectWorkNoteTool` | Notas de trabajo del proyecto (creación, búsqueda, generación de directorio) |
| `ProjectWorkTool` | Operaciones de trabajo del proyecto (crear tareas, chat grupal, difusión, completar proyecto) |
