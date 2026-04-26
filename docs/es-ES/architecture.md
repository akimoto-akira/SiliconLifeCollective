# Arquitectura

[English](../en/architecture.md) | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | **Español** | [Deutsch](../de-DE/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Čeština](../cs-CZ/architecture.md)

## Conceptos Centrales

### Ser Silicona

Cada agente de IA en el sistema es un **Ser Silicona** — una entidad autónoma con su propia identidad, personalidad y capacidades. Cada Ser Silicona es impulsado por un **archivo de alma** (indicación en Markdown) que define sus patrones de comportamiento.

### Curador Silicona

El **Curador Silicona** es un Ser Silicona especial con los más altos privilegios del sistema. Actúa como administrador del sistema:

- Crear y gestionar otros Seres Silicona
- Analizar solicitudes de usuarios y descomponerlas en tareas
- Distribuir tareas a los Seres Silicona apropiados
- Monitorear la calidad de ejecución y manejar fallos
- Responder a mensajes de usuario usando **programación prioritaria** (ver más abajo)

### Archivo de Alma

Un archivo Markdown (`soul.md`) almacenado en el directorio de datos de cada Ser Silicona. Se inyecta como indicación del sistema en cada solicitud de IA, definiendo la personalidad, patrones de decisión y restricciones de comportamiento del ser.

---

## Programación: Programación Justa por Intervalo de Tiempo

### Bucle Principal + Objetos de Reloj

El sistema ejecuta un **bucle principal impulsado por reloj** en un hilo de fondo dedicado:

```
Bucle principal (hilo dedicado, watchdog + cortacircuitos)
  └── Objeto de reloj A (prioridad=0, intervalo=100ms)
  └── Objeto de reloj B (prioridad=1, intervalo=500ms)
  └── Gestor de Seres Silicona (activado directamente por reloj del bucle principal)
        └── Ejecutor de Ser Silicona → Ser Silicona 1 → activado por reloj → ejecuta una ronda
        └── Ejecutor de Ser Silicona → Ser Silicona 2 → activado por reloj → ejecuta una ronda
        └── Ejecutor de Ser Silicona → Ser Silicona 3 → activado por reloj → ejecuta una ronda
        └── ...
```

Decisiones clave de diseño:

- **Los Seres Silicona no heredan objetos de reloj.** Tienen su propio método `Tick()`, invocado por `SiliconBeingManager` a través de `SiliconBeingRunner`, en lugar de registrarse directamente en el bucle principal.
- **El Gestor de Seres Silicona** es activado directamente por reloj del bucle principal y actúa como agente único para todos los seres.
- **Los Ejecutores de Ser Silicona** envuelven el `Tick()` de cada ser en un hilo temporal, con timeout y cortacircuitos por ser (3 timeouts consecutivos → 1 minuto de enfriamiento).
- La ejecución de cada ser se limita a **una ronda** de solicitud de IA + invocación de herramientas por activación de reloj, asegurando que ningún ser pueda monopolizar el bucle principal.
- **El Monitor de Rendimiento** rastrea tiempos de ejecución de reloj para observabilidad.

### Respuesta Prioritaria del Curador

Cuando un usuario envía un mensaje al Curador Silicona:

1. El ser actual (ej. Ser A) completa su ronda actual — **sin interrupción**.
2. El gestor **omite la cola restante**.
3. El bucle **reinicia desde el curador**, permitiendo su ejecución inmediata.

Esto asegura respuesta a interacciones de usuario sin interferir con tareas en progreso.

---

## Arquitectura de Componentes

```
┌─────────────────────────────────────────────────────────┐
│                        Host Central                      │
│  (Host unificado — ensambla y gestiona todos los componentes) │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ Bucle     │  │ Localizador   │  │   Configuración   │  │
│  │ Principal │  │ de Servicios  │  │                  │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │        Gestor de Seres Silicona (objeto de reloj)   │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Curador   │ │ Ser A   │ │ Ser B   │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              Servicios Compartidos                │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Sistema de │  │Almacena- │  │  Gestor de       │  │   │
│  │  │  Chat    │  │  miento  │  │   Permisos       │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │Cliente IA│  │Ejecutores│  │  Gestor de       │  │   │
│  │  │          │  │          │  │   Herramientas   │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  Ejecutores                       │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │  Disco   │  │   Red    │  │  Línea de        │  │   │
│  │  │Ejecutor  │  │Ejecutor  │  │  Comandos        │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │            Proveedores de Mensajería              │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Consola  │  │   Web    │  │  Feishu / ...    │  │   │
│  │  │Proveedor │  │Proveedor │  │  Proveedor       │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## Localizador de Servicios

`ServiceLocator` es un registro singleton seguro para hilos que proporciona acceso a todos los servicios centrales:

| Propiedad | Tipo | Descripción |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | Gestor central de sesiones de chat |
| `IMManager` | `IMManager` | Enrutador de proveedores de mensajería instantánea |
| `AuditLogger` | `AuditLogger` | Seguimiento de auditoría de permisos |
| `GlobalAcl` | `GlobalACL` | Lista de control de acceso global |
| `BeingFactory` | `ISiliconBeingFactory` | Fábrica para crear seres |
| `BeingManager` | `SiliconBeingManager` | Gestor de ciclo de vida de seres activos |
| `DynamicBeingLoader` | `DynamicBeingLoader` | Cargador de compilación dinámica |
| `TokenUsageAudit` | `ITokenUsageAudit` | Seguimiento de uso de tokens |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Informes de uso de tokens |

También mantiene un registro de `PermissionManager` por ser, indexado por GUID del ser.

---

## Sistema de Chat

### Tipos de Sesión

El sistema de chat soporta tres tipos de sesión a través de `SessionBase`:

| Tipo | Clase | Descripción |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | Conversación uno-a-uno entre dos participantes |
| `GroupChat` | `GroupChatSession` | Chat grupal con múltiples participantes |
| `Broadcast` | `BroadcastChannel` | Canal abierto con ID fijo; los seres se suscriben dinámicamente, solo reciben mensajes después de suscribirse |

### Canales de Broadcast

`BroadcastChannel` es un tipo especial de sesión para anuncios a nivel del sistema:

- **ID de canal fijo** — A diferencia de `SingleChatSession` y `GroupChatSession`, los IDs de canal son constantes conocidas, no derivadas de GUIDs de miembros.
- **Suscripción dinámica** — Los seres se suscriben/cancelan suscripción en tiempo de ejecución; solo reciben mensajes publicados después de su suscripción.
- **Filtrado de mensajes pendientes** — `GetPendingMessages()` solo devuelve mensajes publicados después del tiempo de suscripción del ser y no leídos aún.
- **Gestionado por sistema de chat** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### Mensajes de Chat

El modelo `ChatMessage` contiene campos para contexto de conversación de IA y seguimiento de tokens:

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `Id` | `Guid` | Identificador único de mensaje |
| `SenderId` | `Guid` | Identificador único del remitente |
| `ChannelId` | `Guid` | Identificador de canal/conversación |
| `Content` | `string` | Contenido del mensaje |
| `Timestamp` | `DateTime` | Cuándo se envió el mensaje |
| `Type` | `MessageType` | Texto, imagen, archivo o notificación del sistema |
| `ReadBy` | `List<Guid>` | IDs de participantes que han leído este mensaje |
| `Role` | `MessageRole` | Rol de conversación de IA (usuario, asistente, herramienta) |
| `ToolCallId` | `string?` | ID de invocación de herramienta para mensajes de resultado de herramienta |
| `ToolCallsJson` | `string?` | JSON serializado de invocaciones de herramienta para mensajes de asistente |
| `Thinking` | `string?` | Razonamiento de cadena de pensamiento de la IA |
| `PromptTokens` | `int?` | Número de tokens en el prompt (entrada) |
| `CompletionTokens` | `int?` | Número de tokens en la completación (salida) |
| `TotalTokens` | `int?` | Total de tokens usados (entrada + salida) |
| `FileMetadata` | `FileMetadata?` | Metadatos de archivo adjunto (si el mensaje contiene archivo) |

### Cola de Mensajes de Chat

`ChatMessageQueue` es un sistema de cola de mensajes seguro para hilos para gestión de procesamiento asíncrono de mensajes de chat:

- **Seguro para hilos** - Usa mecanismo de bloqueo para asegurar acceso concurrente seguro
- **Procesamiento asíncrono** - Soporte para encolar y desencolar mensajes de forma asíncrona
- **Ordenación de mensajes** - Mantiene orden temporal de mensajes
- **Operaciones por lotes** - Soporte para obtención de mensajes por lotes

### Metadatos de Archivo

`FileMetadata` se usa para gestionar información de archivos adjuntos a mensajes de chat:

- **Información de archivo** - Nombre, tamaño, tipo, ruta del archivo
- **Hora de subida** - Marca de tiempo de cuando se subió el archivo
- **Subido por** - ID de usuario o Ser Silicona que subió el archivo

### Gestor de Cancelación de Stream

`StreamCancellationManager` proporciona mecanismo de cancelación para respuestas de streaming de IA:

- **Control de stream** - Soporte para cancelar respuestas de streaming de IA en progreso
- **Limpieza de recursos** - Limpieza adecuada de recursos relacionados al cancelar
- **Seguro para concurrencia** - Soporte para gestionar múltiples streams simultáneamente

### Visualización de Historial de Chat

La funcionalidad de visualización de historial de chat recientemente añadida permite a usuarios navegar conversaciones históricas de Seres Silicona:

- **Lista de sesiones** - Muestra todas las sesiones históricas
- **Detalles de mensajes** - Ver historial completo de mensajes
- **Vista de línea de tiempo** - Mostrar mensajes en orden cronológico
- **Soporte de API** - Proporciona API RESTful para obtener datos de sesiones y mensajes

---

## Sistema de Clientes de IA

El sistema soporta múltiples backends de IA a través de la interfaz `IAIClient`:

### OllamaClient

- **Tipo**: Servicio de IA local
- **Protocolo**: API HTTP nativa de Ollama (`/api/chat`, `/api/generate`)
- **Funcionalidades**: Streaming, invocación de herramientas, alojamiento de modelos locales
- **Configuración**: `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (Alibaba Cloud Bailian)

- **Tipo**: Servicio de IA en la nube
- **Protocolo**: API compatible con OpenAI (`/compatible-mode/v1/chat/completions`)
- **Autenticación**: Token Bearer (clave API)
- **Funcionalidades**: Streaming, invocación de herramientas, contenido de razonamiento (cadena de pensamiento), despliegue multi-región
- **Regiones soportadas**:
  - `beijing` — China del Norte 2 (Beijing)
  - `virginia` — EE.UU. (Virginia)
  - `singapore` — Singapur
  - `hongkong` — Hong Kong, China
  - `frankfurt` — Alemania (Fráncfort)
- **Modelos soportados** (descubrimiento dinámico a través de API, con lista de respaldo):
  - **Serie Tongyi Qianwen**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **Razonamiento**: qwq-plus
  - **Terceros**: deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **Configuración**: `apiKey`, `region`, `model`
- **Descubrimiento de modelos**: Obtiene modelos disponibles de la API de Bailian en tiempo de ejecución; respalda a lista seleccionada en caso de fallo de red

### Patrón de Fábrica de Clientes

Cada tipo de cliente de IA tiene una implementación de fábrica correspondiente que implementa `IAIClientFactory`:

- `OllamaClientFactory` — Crea instancias de OllamaClient
- `DashScopeClientFactory` — Crea instancias de DashScopeClient

Las fábricas proporcionan:
- `CreateClient(Dictionary<string, object> config)` — Instanciar cliente desde configuración
- `GetConfigKeyOptions(string key, ...)` — Devolver opciones dinámicas para claves de configuración (ej. modelos disponibles, regiones)
- `GetDisplayName()` — Nombre de visualización localizado para el tipo de cliente

### Lista de Compatibilidad de Plataformas de IA

#### Leyenda de Estados
- ✅ Implementado
- 🚧 En desarrollo
- 📋 Planificado
- 💡 En consideración

*Nota: Dependiendo del entorno de red del desarrollador, conectar con servicios de IA en la nube de ultramar [En consideración] puede requerir herramientas de proxy de red, y el proceso de depuración puede ser inestable.*

#### Lista de Plataformas

| Plataforma | Estado | Tipo | Descripción |
|------|------|------|------|
| Ollama | ✅ | Local | Servicio de IA local, soporta despliegue de modelos locales |
| DashScope (Alibaba Cloud Bailian) | ✅ | Nube | Servicio de IA Alibaba Cloud Bailian, soporta despliegue multi-región |
| Baidu Qianfan (Wenxin Yiyan) | 📋 | Nube | Servicio de IA Baidu Wenxin Yiyan |
| Zhipu AI (GLM) | 📋 | Nube | Servicio de IA Zhipu Qingyan |
| Moonshot AI (Kimi) | 📋 | Nube | Servicio de IA Moonshot Kimi |
| Volcengine Ark Engine.Doubao | 📋 | Nube | Servicio de IA ByteDance Doubao |
| DeepSeek (conexión directa) | 📋 | Nube | Servicio de IA DeepSeek |
| 01.AI | 📋 | Nube | Servicio de IA 01.AI |
| Tencent Hunyuan | 📋 | Nube | Servicio de IA Tencent Hunyuan |
| SiliconFlow | 📋 | Nube | Servicio de IA SiliconFlow |
| MiniMax | 📋 | Nube | Servicio de IA MiniMax |
| OpenAI | 💡 | Nube | Servicio API OpenAI (serie GPT) |
| Anthropic | 💡 | Nube | Servicio de IA Anthropic Claude |
| Google DeepMind | 💡 | Nube | Servicio de IA Google Gemini |
| Mistral AI | 💡 | Nube | Servicio de IA Mistral |
| Groq | 💡 | Nube | Servicio de inferencia de IA de alta velocidad Groq |
| Together AI | 💡 | Nube | Servicio de modelos de código abierto Together AI |
| xAI | 💡 | Nube | Servicio xAI Grok |
| Cohere | 💡 | Nube | Servicio de NLP empresarial Cohere |
| Replicate | 💡 | Nube | Plataforma de alojamiento de modelos de código abierto Replicate |
| Hugging Face | 💡 | Nube | Comunidad y plataforma de modelos de IA de código abierto Hugging Face |
| Cerebras | 💡 | Nube | Servicio de optimización de inferencia de IA Cerebras |
| Databricks | 💡 | Nube | Plataforma empresarial de IA Databricks (MosaicML) |
| Perplexity AI | 💡 | Nube | Servicio de búsqueda y preguntas y respuestas Perplexity AI |
| NVIDIA NIM | 💡 | Nube | Microservicios de inferencia de IA NVIDIA |

---

## Decisiones Clave de Diseño

### Almacenamiento como Clase de Instancia (no estático)

`IStorage` está diseñado como una instancia inyectable, no como una utilidad estática. Esto asegura:

- Acceso directo al sistema de archivos — IStorage es el canal de persistencia interna del sistema, **no** se enruta a través de ejecutores.
- **La IA no puede controlar IStorage** — Los ejecutores gestionan IO iniciados por herramientas de IA; IStorage gestiona lectura/escritura de datos internos del propio framework. Estas son preocupaciones fundamentalmente diferentes.
- Capacidad de probar con implementaciones simuladas.
- Soporte futuro para diferentes backends de almacenamiento sin modificar consumidores.

### Ejecutores como Límites de Seguridad

Los ejecutores son el **único** camino para operaciones de E/S. Las herramientas que necesitan acceso a disco, red o línea de comandos **deben** pasar por ejecutores. Este diseño refuerza:

- Cada ejecutor tiene su **propio hilo de programación**, con bloqueo de hilo para verificación de permisos.
- Verificación centralizada de permisos — Los ejecutores consultan el **gestor de permisos privado** del ser.
- Cola de solicitudes que soporta control de prioridad y timeout.
- Registro de auditoría para todas las operaciones externas.
- Aislamiento de excepciones — Fallos en un ejecutor no afectan a otros ejecutores.
- Cortacircuitos — Fallos consecutivos detienen temporalmente el ejecutor para prevenir fallos en cascada.

### ContextManager como Objeto Ligero

Cada `ExecuteOneRound()` crea una nueva instancia de `ContextManager`:

1. Cargar archivo de alma + historial de chat reciente.
2. Enviar solicitud al cliente de IA.
3. Procesar invocaciones de herramientas en bucle hasta que la IA devuelva texto puro.
4. Persistir respuesta al sistema de chat.
5. Liberar.

Esto mantiene cada ronda aislada y sin estado.

### Auto-Evolución a través de Reemplazo de Clases

Los Seres Silicona pueden reescribir sus propias clases C# en tiempo de ejecución:

1. La IA genera nuevo código de clase (debe heredar de `SiliconBeingBase`).
2. **Control de referencias en compilación** (defensa principal): El compilador solo obtiene una lista permitida de ensamblados — `System.IO`, `System.Reflection`, etc. están excluidos, por lo que código peligroso es imposible a nivel de tipo.
3. **Análisis estático en tiempo de ejecución** (defensa secundaria): `SecurityScanner` escanea código para patrones peligrosos después de compilación exitosa.
4. Roslyn compila el código en memoria.
5. En éxito: `SiliconBeingManager.ReplaceBeing()` intercambia la instancia actual, migra estado y persiste código cifrado a disco.
6. En fallo: Descartar nuevo código, mantener implementación existente.

Implementaciones personalizadas de `IPermissionCallback` también pueden compilarse e inyectarse a través de `ReplacePermissionCallback()`, permitiendo a los seres personalizar su propia lógica de permisos.

El código se almacena cifrado en disco usando AES-256. La clave de cifrado se deriva del GUID del ser (en mayúsculas) a través de PBKDF2.

---

## Auditoría de Uso de Tokens

`TokenUsageAuditManager` rastrea consumo de tokens de IA para todos los seres:

- `TokenUsageRecord` — Registro por solicitud (ID de ser, modelo, tokens de prompt, tokens de completación, marca de tiempo)
- `TokenUsageSummary` — Estadísticas agregadas
- `TokenUsageQuery` — Parámetros de consulta para filtrar registros
- Persistido a través de `ITimeStorage` para consultas de series de tiempo
- Accesible a través de Web UI (AuditController) y `TokenAuditTool` (solo curador)

---

### Sistema de Calendario

El sistema contiene **32 implementaciones de calendario**, derivadas de la clase abstracta `CalendarBase`, cubriendo los principales sistemas de calendario del mundo:

| Calendario | ID | Descripción |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | Calendario budista (BE), año + 543 |
| CherokeeCalendar | `cherokee` | Sistema de calendario Cherokee |
| ChineseLunarCalendar | `lunar` | Calendario lunar chino, con meses bisiestos |
| ChineseHistoricalCalendar | `chinese_historical` | Calendario histórico chino, soporta ciclo Ganzhi y años de reinado imperial |
| ChulaSakaratCalendar | `chula_sakarat` | Calendario Chula Sakarat (CS), año - 638 |
| CopticCalendar | `coptic` | Calendario copto |
| DaiCalendar | `dai` | Calendario Dai, con cálculo lunar completo |
| DehongDaiCalendar | `dehong_dai` | Variante del calendario Dai de Dehong |
| EthiopianCalendar | `ethiopian` | Calendario etíope |
| FrenchRepublicanCalendar | `french_republican` | Calendario republicano francés |
| GregorianCalendar | `gregorian` | Calendario gregoriano estándar |
| HebrewCalendar | `hebrew` | Calendario hebreo (judío) |
| IndianCalendar | `indian` | Calendario nacional indio |
| InuitCalendar | `inuit` | Sistema de calendario Inuit |
| IslamicCalendar | `islamic` | Calendario Hijri islámico |
| JapaneseCalendar | `japanese` | Calendario de eras japonés (Nengo) |
| JavaneseCalendar | `javanese` | Calendario javanés islámico |
| JucheCalendar | `juche` | Calendario Juche (Corea del Norte), año - 1911 |
| JulianCalendar | `julian` | Calendario juliano |
| KhmerCalendar | `khmer` | Calendario jemer |
| MayanCalendar | `mayan` | Cuenta larga del calendario maya |
| MongolianCalendar | `mongolian` | Calendario mongol |
| PersianCalendar | `persian` | Calendario persa (Hijri solar) |
| RepublicOfChinaCalendar | `roc` | Calendario de la República de China (Minguo), año - 1911 |
| RomanCalendar | `roman` | Calendario romano |
| SakaCalendar | `saka` | Calendario Saka (Indonesia) |
| SexagenaryCalendar | `sexagenary` | Calendario Ganzhi chino (ciclo sexagenario) |
| TibetanCalendar | `tibetan` | Calendario tibetano |
| VietnameseCalendar | `vietnamese` | Calendario lunar vietnamita (variante del zodiaco del gato) |
| VikramSamvatCalendar | `vikram_samvat` | Calendario Vikram Samvat |
| YiCalendar | `yi` | Sistema de calendario Yi |
| ZoroastrianCalendar | `zoroastrian` | Calendario zoroástrico |

`CalendarTool` proporciona operaciones: `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (conversión de fechas entre calendarios).

---

## Arquitectura de Web UI

### Sistema de Pieles

La Web UI tiene un **sistema de pieles enchufable** que permite personalización completa de UI sin cambiar la lógica de la aplicación:

- **Interfaz ISkin** — Define el contrato para todas las pieles, incluyendo:
  - Métodos centrales de renderizado (`RenderHtml`, `RenderError`)
  - 20+ métodos de componentes de UI (botones, entradas, tarjetas, tablas, insignias, burbujas, progreso, etiquetas, etc.)
  - Generación de CSS temático a través de `CssBuilder`
  - `SkinPreviewInfo` — Paleta de colores e iconos para selector de piel en página de inicialización

- **Pieles integradas** — 4 pieles listas para producción:
  - **Admin** — Interfaz profesional y enfocada en datos para administración del sistema
  - **Chat** — Diseño conversacional y centrado en mensajes para interacción con IA
  - **Creative** — Layout artísticamente rico visualmente para flujos de trabajo creativos
  - **Dev** — Interfaz centrada en desarrolladores y código con resaltado de sintaxis

- **Descubrimiento de pieles** — `SkinManager` descubre y registra automáticamente todas las implementaciones de `ISkin` a través de reflexión

### Constructores HTML / CSS / JS

La Web UI evita completamente archivos de plantillas, generando todo el marcado en C#:

- **`H`** — DSL de constructor HTML fluida para construir árboles HTML en código
- **`CssBuilder`** — Constructor CSS con soporte para selectores y media queries
- **`JsBuilder` (`JsSyntax`)** — Constructor JavaScript para scripts en línea

### Sistema de Controladores

La Web UI sigue un **patrón similar a MVC**, con 20+ controladores manejando diferentes aspectos:

| Controlador | Propósito |
|------------|---------|
| About | Página acerca de e información del proyecto |
| Audit | Panel de auditoría de uso de tokens, con gráficos de tendencias y exportación |
| Being | Gestión y estado de Seres Silicona |
| Chat | Interfaz de chat en tiempo real con SSE |
| ChatHistory | Visualización de historial de chat, soporta lista de sesiones y detalles de mensajes |
| CodeBrowser | Visualización y edición de código |
| CodeHover | Indicaciones de código flotantes, con resaltado de sintaxis |
| Config | Gestión de configuración del sistema |
| Dashboard | Resumen del sistema y métricas |
| Executor | Estado y gestión de ejecutores |
| Help | Sistema de documentos de ayuda, soporte multilingüe |
| Init | Asistente de inicialización para primera ejecución |
| Knowledge | Visualización y consulta de gráfico de conocimiento |
| Log | Visor de registro del sistema, soporta filtrado por Ser Silicona |
| Memory | Explorador de memoria a largo plazo, soporta filtrado avanzado, estadísticas y vista de detalles |
| Permission | Gestión de permisos |
| PermissionRequest | Cola de solicitudes de permisos |
| Project | Gestión de proyectos, incluye notas de trabajo y sistema de tareas |
| Task | Interfaz del sistema de tareas |
| Timer | Gestión del sistema de temporizadores, incluye historial de ejecución |
| WorkNote | Gestión de notas de trabajo, soporta búsqueda y generación de índice |

### Actualizaciones en Tiempo Real

- **SSE (Eventos Enviados por el Servidor)** — Empujar actualizaciones de mensajes de chat, estado de seres y eventos del sistema a través de `SSEHandler`
- **Sin WebSocket** — Arquitectura más simple usando SSE que satisface la mayoría de necesidades en tiempo real
- **Reconexión automática** — Lógica de reconexión del cliente para conexión resiliente

### Localización

El sistema soporta localización completa para **21 variantes de idioma**:
- **Chino (6 variantes)**: zh-CN (simplificado), zh-HK (tradicional), zh-SG (Singapur), zh-MO (Macao), zh-TW (Taiwán), zhMY (Malasia)
- **Inglés (10 variantes)**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY
- **Español (2 variantes)**: es-ES, es-MX
- **Otros (3 variantes)**: ja-JP (japonés), ko-KR (coreano), cs-CZ (checo)

El locale activo se selecciona a través de `DefaultConfigData.Language` y se resuelve a través de `LocalizationManager`.

---

### Sistema de Automatización de Navegador WebView (Nuevo)

El sistema integra funcionalidad de automatización de navegador WebView basada en **Playwright**:

- **Aislamiento individual**: Cada Ser Silicona tiene su propia instancia de navegador, cookies y almacenamiento de sesión, completamente aislados sin interferencia mutua.
- **Modo sin cabeza**: El navegador se ejecuta en modo sin cabeza completamente invisible para el usuario, los Seres Silicona operan de forma autónoma en segundo plano.
- **WebViewBrowserTool**: Proporciona capacidades completas de operación de navegador, incluyendo:
  - Navegación de páginas, clics, entrada de texto, obtención de contenido de página
  - Ejecución de JavaScript, obtención de capturas de pantalla, espera de elementos
  - Gestión de estado del navegador y limpieza de recursos
- **Control de seguridad**: Todas las operaciones del navegador requieren pasar por la cadena de verificación de permisos, previniendo acceso a páginas web maliciosas.

### Sistema de Red de Conocimiento (Nuevo)

El sistema tiene incorporado un sistema de gráfico de conocimiento basado en **estructura de tripla**:

- **Representación de conocimiento**: Usa estructura de tripla "sujeto-relación-objeto" (ejemplo: Python-es_un-lenguaje_de_programación)
- **KnowledgeTool**: Proporciona gestión de ciclo de vida completo del conocimiento:
  - `add`/`query`/`update`/`delete` - Operaciones CRUD básicas
  - `search` - Búsqueda de texto completo y coincidencia de palabras clave
  - `get_path` - Descubrir rutas de asociación entre dos conceptos
  - `validate` - Verificación de integridad del conocimiento
  - `stats` - Análisis estadístico de la red de conocimiento
- **Almacenamiento persistente**: Triples de conocimiento persistidos al sistema de archivos, soportando consulta indexada por tiempo.
- **Puntuación de confianza**: Cada entrada de conocimiento tiene una puntuación de confianza (0-1), soportando coincidencia difusa y ordenamiento de conocimiento.
- **Clasificación por etiquetas**: Soporte para añadir etiquetas al conocimiento, facilitando clasificación y recuperación.

---

## Estructura de Directorio de Datos

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # Archivo de alma del curador
    │   ├── state.json       # Estado en tiempo de ejecución
    │   ├── code.enc         # Código de clase personalizada cifrado AES
    │   └── permission.enc   # Callback de permisos personalizado cifrado AES
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```
