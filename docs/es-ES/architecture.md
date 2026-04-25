# Arquitectura

[English](architecture.md) | [简体中文](docs/zh-CN/architecture.md) | [繁體中文](docs/zh-HK/architecture.md) | [Español](docs/es-ES/architecture.md) | [日本語](docs/ja-JP/architecture.md) | [한국어](docs/ko-KR/architecture.md) | [Čeština](docs/cs-CZ/architecture.md)

## Conceptos Fundamentales

### Ser de Silicio

Cada agente de IA en el sistema es un **Ser de Silicio** — una entidad autónoma con su propia identidad, personalidad y capacidades. Cada Ser de Silicio es impulsado por un **Archivo de Alma** (prompt en Markdown) que define sus patrones de comportamiento.

### Curador de Silicio

El **Curador de Silicio** es un Ser de Silicio especial con los privilegios más altos del sistema. Actúa como administrador del sistema:

- Crea y gestiona otros Seres de Silicio
- Analiza solicitudes de usuarios y las descompone en tareas
- Distribuye tareas a los Seres de Silicio apropiados
- Monitorea la calidad de ejecución y maneja fallos
- Responde a mensajes de usuarios con **programación prioritaria** (ver más abajo)

### Archivo de Alma

Un archivo Markdown (`soul.md`) almacenado en el directorio de datos de cada Ser de Silicio. Sirve como prompt del sistema inyectado en cada solicitud de IA, definiendo la personalidad del ser, patrones de toma de decisiones y restricciones de comportamiento.

---

## Programación: Programación Justa por Segmentos de Tiempo

### MainLoop + TickObject

El sistema ejecuta un **bucle principal basado en ticks** en un hilo dedicado en segundo plano:

```
MainLoop (hilo dedicado, watchdog + circuit breaker)
  └── TickObject A (Prioridad=0, Intervalo=100ms)
  └── TickObject B (Prioridad=1, Intervalo=500ms)
  └── SiliconBeingManager (activado directamente por MainLoop)
        └── SiliconBeingRunner → Ser de Silicio 1 → Tick → ExecuteOneRound
        └── SiliconBeingRunner → Ser de Silicio 2 → Tick → ExecuteOneRound
        └── SiliconBeingRunner → Ser de Silicio 3 → Tick → ExecuteOneRound
        └── ...
```

Decisiones clave de diseño:

- **Los Seres de Silicio NO heredan TickObject.** Tienen su propio método `Tick()`, invocado por `SiliconBeingManager` a través de un `SiliconBeingRunner`, no registrados directamente en el MainLoop.
- **SiliconBeingManager** es activado directamente por MainLoop y actúa como proxy único para todos los seres.
- **SiliconBeingRunner** envuelve el `Tick()` de cada ser en un hilo temporal con timeout y circuit breaker por ser (3 timeouts consecutivos → 1 minuto de cooldown).
- La ejecución de cada ser está limitada a **una ronda** de solicitud de IA + ToolCalls por tick, asegurando que ningún ser pueda monopolizar el bucle principal.
- **PerformanceMonitor** rastrea tiempos de ejecución de ticks para observabilidad.

---

## Arquitectura Cuerpo-Cerebro

Cada Ser de Silicio sigue el patrón de diseño **Cuerpo-Cerebro**:

### Cuerpo (DefaultSiliconBeing)

El "cuerpo" mantiene vivo al ser y detecta desencadenadores:
- Permanece activo durante todo el ciclo de vida
- Detecta eventos desencadenadores (nuevos mensajes de chat, temporizadores, tareas)
- Invoca al cerebro cuando se detecta un desencadenador
- Maneja la persistencia de estado

### Cerebro (ContextManager)

El "cerebro" maneja el procesamiento de IA:
- Carga historial de conversaciones del almacenamiento
- Construye solicitudes de IA con contexto apropiado
- Llama al cliente de IA
- Procesa respuestas (texto puro o tool_calls)
- Ejecuta bucle de llamada a herramientas si es necesario
- Persiste respuestas en el sistema de chat

```
Cuerpo (SiliconBeing)
  ↓ Detecta desencadenador
Cerebro (ContextManager)
  ↓ Carga historial
  ↓ Construye solicitud
Cliente IA
  ↓ Respuesta
¿Tool Calls?
  ├─ Sí → Ejecutar herramientas → Repetir
  └─ No → Respuesta de texto plano → Persistir
```

---

## Bucle de Llamada a Herramientas

Cuando la IA devuelve una respuesta con `tool_calls`:

1. Analizar la respuesta y extraer tool_calls
2. Para cada tool_call:
   - Ejecutar la herramienta correspondiente
   - Capturar resultado
3. Enviar resultados de vuelta a la IA
4. Repetir hasta que la IA devuelva respuesta de texto plano
5. Persistir la respuesta final

Este bucle permite que la IA use herramientas de forma iterativa para completar tareas complejas.

---

## Seguridad Ejecutor-Permiso

Todas las operaciones de I/O iniciadas por IA pasan por una cadena de seguridad de 5 niveles:

```
Tool Call → Ejecutor → PermissionManager → [IsCurator → FrequencyCache → GlobalACL → Callback → AskUser]
```

### Ejecutores

Los ejecutores envuelven operaciones reales de I/O:
- **DiskExecutor**: Operaciones de archivos
- **NetworkExecutor**: Solicitudes HTTP/red
- **CommandLineExecutor**: Ejecución de comandos
- **DynamicCompilationExecutor**: Compilación y ejecución de código

### Cadena de Permisos

1. **IsCurator**: Si el solicitante es el Curador de Silicio, permitir automáticamente
2. **UserFrequencyCache**: Verificar límites de frecuencia del usuario
3. **GlobalACL**: Verificar lista de control de acceso global
4. **IPermissionCallback**: Callback personalizable del usuario
5. **IPermissionAskHandler**: Preguntar al usuario (a través de interfaz web u otro canal)

Todas las decisiones de permisos se registran para auditoría.

---

## Sistema de Almacenamiento

### IStorage

Almacenamiento simple de clave-valor:
```csharp
void Save(string key, string content);
string? Load(string key);
void Delete(string key);
```

### ITimeStorage

Almacenamiento indexado por tiempo para consultas de rango temporal:
```csharp
void SaveByTime(string key, string content, DateTime time);
List<string> LoadByTimeRange(DateTime start, DateTime end);
```

Los datos se almacenan en archivos JSON en estructura de directorios basada en tiempo:
```
data/
  beings/
    {being-id}/
      chat/
        2024/01/15/14/30/25/
          message-123.json
```

---

## Sistema de Compilación Dinámica

Los Seres de Silicio pueden autoevolucionarse mediante compilación dinámica Roslyn:

1. El Ser genera código C# como respuesta de IA
2. El código pasa por escaneo de seguridad
3. Compilación usando Microsoft.CodeAnalysis.CSharp
4. Carga dinámica del assembly compilado
5. Ejecución con límites de permisos
6. Resultados retornados al Ser

### Seguridad de Compilación

- **SecurityScanner**: Analiza código para llamadas API peligrosas
- **Aislamiento**: Código compilado se ejecuta en contexto restringido
- **Permisos**: Todas las operaciones de I/O verificadas

---

## Sistema de Localización

Soporte multilingüe a través del patrón de localización:

- **LocalizationBase**: Clase base abstracta con todos los textos localizables
- **LocalizationManager**: Resuelve la localización apropiada basada en Language enum
- **Implementaciones de Idioma**: Una clase por variante de idioma (EsES, EsMX, EnUS, ZhCN, etc.)
- **Herencia**: Las variantes de idioma pueden heredar de idiomas base (EsMX hereda de EsES)

---

## Interfaz Web

### Arquitectura

```
WebHost (HttpListener)
  ↓
Router (coincidencia de patrones)
  ↓
Controladores (18 controladores)
  ↓
Vistas (constructores HTML/CSS/JS)
  ↓
Respuesta HTTP (+ SSE opcional)
```

### Sistema de Pieles

4 pieles integradas intercambiables:
- **Admin**: Interfaz profesional de gestión
- **Chat**: Interfaz conversacional enfocada en chat
- **Creative**: Interfaz artística para creatividad
- **Dev**: Interfaz enfocada en desarrolladores

Cada piel implementa `ISkin` y proporciona:
- Métodos de construcción de vista
- Recursos CSS/JS
- Información de vista previa

---

## Sistema de IA

### Backends Soportados

1. **OllamaClient**: API HTTP nativa de Ollama para alojamiento local de modelos
2. **DashScopeClient**: API compatible con OpenAI para Alibaba Cloud DashScope

### Fábrica de Clientes

```csharp
IAIClientFactory factory = ...;
IAIClient client = factory.CreateClient(config);
```

Permite configuración de múltiples clientes de IA y conmutación dinámica.

---

## Gestión de Tareas y Temporizadores

### TaskSystem

Los Seres de Silicio pueden crear y gestionar tareas:
- Crear tareas con descripciones y estados
- Actualizar progreso
- Marcar como completadas
- Consultar historial de tareas

### TimerSystem

Los Seres pueden configurar temporizadores para acciones futuras:
- Temporizadores de una sola vez
- Temporizadores recurrentes
- Cancelación de temporizadores
- Desencadenamiento de acciones al expirar

---

## Auditoría de Uso de Tokens

Seguimiento integral del uso de tokens de IA:

- Registro de cada solicitud de IA
- Seguimiento de tokens de entrada y salida
- Reportes resumidos por Ser y período de tiempo
- Consultas por rango de tiempo
- Integración con interfaz web para visualización

---

## Diagrama de Componentes

```
┌─────────────────────────────────────────┐
│           Aplicación Principal          │
├─────────────────────────────────────────┤
│  CoreHost                               │
│  ├─ MainLoop                            │
│  ├─ SiliconBeingManager                 │
│  ├─ ChatSystem                          │
│  └─ ToolManager                         │
├─────────────────────────────────────────┤
│  WebHost                                │
│  ├─ Router                              │
│  ├─ Controllers (18)                    │
│  └─ Skins (4)                           │
├─────────────────────────────────────────┤
│  Servicios                              │
│  ├─ AI Clients (Ollama, DashScope)      │
│  ├─ Executors (4 tipos)                 │
│  ├─ Storage (File-based)                │
│  ├─ Localization (20 variantes)         │
│  └─ PermissionManager                   │
└─────────────────────────────────────────┘
```

---

## Decisiones de Diseño Clave

1. **Sin Base de Datos**: Almacenamiento basado en archivos para simplicidad
2. **Sin Framework Web**: HttpListener nativo con constructores HTML para control total
3. **Arquitectura Modular**: Interfaz en Core, implementación en Default
4. **Seguridad Primero**: Cadena de permisos para todas las operaciones de I/O
5. **Localización Integral**: 20 variantes de idioma con herencia
6. **Extensibilidad**: Sistema de plugins para herramientas, pieles y calendarios

---

## Cola de Mensajes de Chat

`ChatMessageQueue` es un sistema de cola de mensajes seguro para subprocesos para gestionar el procesamiento asincrónico de mensajes de chat:

- **Seguro para subprocesos** - Usa mecanismo de bloqueo para garantizar acceso concurrente seguro
- **Procesamiento asincrónico** - Soporta encolado y desencolado asincrónico de mensajes
- **Ordenamiento de mensajes** - Mantiene el orden cronológico de los mensajes
- **Operaciones por lotes** - Soporta recuperación de mensajes por lotes

## Metadatos de Archivos

`FileMetadata` se usa para gestionar información de archivos adjuntos a mensajes de chat:

- **Información del archivo** - Nombre del archivo, tamaño, tipo, ruta
- **Hora de carga** - Marca de tiempo de carga del archivo
- **Cargador** - ID del usuario o Ser de Silicio que cargó el archivo

## Gestor de Cancelación de Flujo

`StreamCancellationManager` proporciona mecanismo de cancelación para respuestas de flujo de IA:

- **Control de flujo** - Soporta cancelar respuestas de flujo de IA en curso
- **Limpieza de recursos** - Limpia adecuadamente los recursos relacionados al cancelar
- **Seguridad de concurrencia** - Soporta gestionar múltiples flujos simultáneamente

## Vista de Historial de Chat

La nueva función de vista de historial de chat permite a los usuarios navegar conversaciones históricas de Seres de Silicio:

- **Lista de conversaciones** - Muestra todas las conversaciones históricas
- **Detalles de mensajes** - Ver historial completo de mensajes
- **Vista de línea de tiempo** - Muestra mensajes en orden cronológico
- **Soporte API** - Proporciona API RESTful para recuperar datos de conversación y mensajes
