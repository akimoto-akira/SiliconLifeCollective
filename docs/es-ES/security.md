# Diseño de Seguridad

> **Versión: v0.2.0-alpha**

[English](../en/security.md) | [Deutsch](../de-DE/security.md) | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | **Español** | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | [Čeština](../cs-CZ/security.md) | [Русский](../ru-RU/security.md)

## Resumen

La seguridad de Silicon Life Collective se basa en un modelo de **defensa en profundidad**. El principio fundamental: **todas las operaciones de E/S deben pasar por los ejecutores**, los cuales aplican verificaciones de permisos antes de la ejecución.

```
Llamada a herramienta → Ejecutor → Gestor de Permisos → Caché de frecuencia → Retrollamada → (IsCurator: preguntar al usuario | Non-curator: ACL Global)
```

---

## Modelo de Permisos

### Tipos de Permisos

| Tipo | Descripción |
|------|-------------|
| `NetworkAccess` | Solicitudes HTTP/HTTPS salientes |
| `CommandLine` | Ejecución de comandos de Shell |
| `FileAccess` | Operaciones de archivos y directorios |
| `Function` | Llamadas a funciones sensibles |
| `DataAccess` | Acceso a datos del sistema o del usuario |

### Resultados de Permisos

Cada verificación de permisos devuelve uno de tres resultados:

| Resultado | Comportamiento |
|-----------|----------------|
| **Allowed (Permitido)** | La operación se ejecuta inmediatamente |
| **Denied (Denegado)** | La operación se bloquea, se registra en el registro de auditoría |
| **AskUser (Preguntar al usuario)** | La operación se pausa, requiere confirmación del usuario |

### Rol Especial: Curador de Silicio

El Curador de Silicio posee el nivel más alto de permisos (`IsCurator = true`). Cuando la cadena de permisos llega al punto de ramificación, las operaciones del Curador se confirman con el usuario a través de `IPermissionAskHandler`, en lugar de permitir directamente. Los no-curadores consultan la ACL Global.

### Gestor de Permisos Privado

Cada Ser de Silicio tiene su propia **instancia privada de PermissionManager**. El estado de los permisos no se comparte entre seres.

---

## Flujo de Verificación de Permisos

La prioridad de consulta es: **1. Caché de frecuencia → 2. Función de retrollamada → 3. Ramificación (IsCurator/GlobalACL)**

```
┌─────────────┐
│ Llamada a    │
│ herramienta  │
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Ejecutor   │────▶│ Gestor de permisos  │
│(disco/red/  │     │ privado (por ser)    │
│  cmd...)    │     └────────┬────────────┘
└─────────────┘              │
                             ▼
                  ┌─────────────────┐
                  │ 1. Caché de     │──coincide──▶ Permitir / Denegar
                  │ frecuencia      │
                  │(Alta denegación │
                  │ prioridad sobre │
                  │ alta permisión) │
                  └────────┬────────┘
                           │ Sin coincidencia
                           ▼
                  ┌─────────────────┐
                  │ 2. Retrollamada │
                  │ de permisos     │──▶ Permitir / Denegar / Preguntar usuario
                  └────────┬────────┘
                           │ Preguntar usuario
                           ▼
                  ┌─────────────────┐
                  │ 3. IsCurator?   │
                  └────────┬────────┘
                           │
                 ┌─────────┴─────────┐
                 │                   │
                 ▼ Sí                ▼ No
          ┌─────────────┐    ┌─────────────┐
          │ Preguntar    │    │ ACL Global  │
          │ al usuario   │    │ Consultar   │
          │(AskHandler)  │    │ reglas      │
          └─────────────┘    └─────────────┘
```

**Punto clave**: El ejecutor solo ve un valor booleano (permitir/denegar). El Gestor de Permisos maneja internamente la decisión ternaria (permitir/denegar/preguntar al usuario) y resuelve la consulta al usuario antes de devolver el resultado al ejecutor.

---

## Ejecutores (Frontera de Seguridad)

Los ejecutores son la **única** ruta para las operaciones de E/S. Aplican:

### Hilo de Programación Independiente

Cada ejecutor tiene su **propio hilo de programación independiente**:

- Aislamiento de hilos entre ejecutores — el bloqueo del hilo de un ejecutor no afecta a otros.
- Cada ejecutor puede configurar límites de recursos independientes (CPU, memoria, etc.).
- Gestión de pool de hilos para los hilos del ejecutor.

### Cola de Solicitudes

Cada ejecutor mantiene una cola de solicitudes:

- Las solicitudes se enrutan por tipo al ejecutor correspondiente.
- Soporta cola por prioridad.
- Control de tiempo de espera por solicitud.

### Bloqueo de Hilo para Verificación de Permisos

Cuando una herramienta inicia un acceso a recursos:

1. El ejecutor recibe la solicitud y **bloquea su hilo**.
2. El ejecutor consulta el Gestor de Permisos privado del ser.
3. Si la retrollamada devuelve "preguntar al usuario", el hilo del ejecutor **permanece bloqueado** esperando la respuesta del usuario.
4. El ser solo ve el resultado final (éxito o denegación) — nunca ve el estado intermedio de "pendiente" o "esperando".
5. Solo el Curador de Silicio activa solicitudes reales al usuario. Los seres ordinarios consultan la ACL Global de forma síncrona sin bloquearse.
6. En caso de tiempo de espera, la solicitud se trata como denegada y el bloqueo del hilo se libera.

### Tipos de Ejecutores

| Ejecutor | Alcance | Tiempo de espera predeterminado |
|----------|---------|-------------------------------|
| `DiskExecutor` | Lectura/escritura de archivos, operaciones de directorio | 30 segundos |
| `NetworkExecutor` | Solicitudes HTTP, conexiones WebSocket | 60 segundos |
| `CommandLineExecutor` | Ejecución de comandos de Shell | 120 segundos |

> **Nota**: `DynamicCompilationExecutor` (ubicado en el espacio de nombres `SiliconLife.Core.Compilation`) gestiona la compilación en memoria con Roslyn, no pertenece a la categoría de ejecutores de E/S, pero está igualmente sujeto al sistema de permisos.

### Aislamiento de Excepciones y Tolerancia a Fallos

- Las excepciones de un ejecutor no afectan a otros ejecutores.
- Reinicio automático en caso de fallo del hilo.
| Interruptor de circuito: detiene temporalmente el ejecutor tras fallos consecutivos para prevenir fallos en cascada.

---

## ACL Global (Lista de Control de Acceso)

Tabla de reglas compartida persistida en almacenamiento, gestionada exclusivamente por el Curador de Silicio:

```json
{
  "rules": [
    { "prefix": "network:api.github.com", "result": "Allowed" },
    { "prefix": "file:C:\\Windows", "result": "Denied" },
    { "prefix": "cli:rm -rf", "result": "Denied" }
  ]
}
```

- Las reglas se evalúan en orden; la primera coincidencia gana.
- Solo el Curador de Silicio puede modificar la ACL Global (a través de sus herramientas dedicadas).
- Los cambios surten efecto inmediatamente.
- La ACL Global **no está** en la cadena de prioridad por consulta mencionada anteriormente — es referenciada internamente por la función de retrollamada.

---

## Caché de Frecuencia del Usuario

Para reducir las solicitudes de permisos repetitivas, el sistema mantiene dos cachés **por ser, solo en memoria**:

| Caché | Propósito |
|-------|-----------|
| **HighAllow (Alta permisión)** | Recursos que el usuario permite frecuentemente |
| **HighDeny (Alta denegación)** | Recursos que el usuario deniega frecuentemente |

### Cómo Funciona

- **Selección del usuario, no detección automática**: Cuando se activa "preguntar al usuario", el usuario elige si agregar el recurso a la caché.
- **Coincidencia de prefijos**: Soporta coincidencia de prefijos de rutas de recursos (por ejemplo, `network:api.example.com/*`).
- **Prioridad**: Alta denegación tiene prioridad sobre alta permisión.
- **Solo en memoria**: La caché no se persiste. Se pierde al reiniciar.
- **Expiración configurable**: El usuario puede establecer el período de validez de las entradas de la caché.

### Flujo de Actualización de la Caché

1. La retrollamada de permisos devuelve `AskUser`.
2. El sistema de permisos envía una consulta al sistema de tarjetas (Web UI o mensajería instantánea).
3. El usuario toma una decisión (permitir/denegar) y **elige si cachear**.
4. El sistema de tarjetas devuelve la decisión + indicador de caché.
5. El sistema de permisos actualiza la lista de caché correspondiente.
6. Las solicitudes futuras que coincidan con el prefijo de la caché se resuelven inmediatamente.

---

## Mecanismo de Pregunta al Usuario

Cuando la verificación de permisos devuelve `AskUser`:

### Web UI: Tarjeta Interactiva

El frontend web muestra inmediatamente una **tarjeta interactiva** que muestra:

- Tipo de recurso y ruta
- Descripción de la operación
- Botones Permitir / Denegar
- Casillas opcionales "Permitir siempre" / "Denegar siempre" (agrega a la caché de frecuencia)

### Mensajería Instantánea (sin soporte de tarjetas): Código Aleatorio

Para plataformas de mensajería que no soportan tarjetas interactivas:

1. El sistema genera dos códigos aleatorios de 6 dígitos: **código de permisión** y **código de denegación**.
2. Envía un mensaje con la información del recurso y ambos códigos.
3. El usuario debe responder con el código de permisión exacto para autorizar. Cualquier otra respuesta se trata como denegación.
4. Los códigos son de un solo uso para prevenir ataques de repetición.

### Tiempo de Espera

- Se establece un tiempo de espera para todas las solicitudes de "preguntar al usuario".
- Al expirar el tiempo, la solicitud se trata como **denegada** y el bloqueo del hilo del ejecutor se libera.

---

## Seguridad de Compilación Dinámica

La autoevolución (reescritura de clases) introduce riesgos de seguridad únicos. El sistema los mitiga con una **estrategia por capas**:

### Capa 1: Control de Referencias en Tiempo de Compilación (Defensa principal)

- El compilador solo obtiene una **lista de referencias de ensamblado permitidas**.
- **Permitido**: `System.Runtime`, `System.Private.CoreLib`, ensamblados del proyecto (interfaz ITool, etc.)
- **Bloqueado**: `System.IO`, `System.Reflection`, `System.Runtime.InteropServices`, etc.
- Si el código referencia un ensamblado bloqueado, **el compilador mismo rechaza** el código.
- Esto es más confiable que el escaneo en tiempo de ejecución — las operaciones peligrosas son imposibles a nivel de tipo.

### Capa 2: Análisis Estático en Tiempo de Ejecución (Defensa secundaria)

- Incluso después de una compilación exitosa, el código se somete a escaneo de patrones estáticos.
- Detecta patrones de operaciones peligrosas (E/S directa, llamadas al sistema, etc.).
- Si se encuentra código peligroso, la carga se rechaza y el sistema retrocede a la funcionalidad por defecto.

### Restricción de Herencia

Todas las clases personalizadas de Seres de Silicio **deben** heredar de `SiliconBeingBase`. El compilador aplica esta restricción a nivel de tipo.

### Almacenamiento Cifrado

El código compilado se almacena cifrado en disco con AES-256:

- **Derivación de clave**: Desde el GUID del ser (en mayúsculas) usando PBKDF2.
- **Fallo de descifrado**: Retrocede a la implementación por defecto.
- **Recompilación en tiempo de ejecución**: El nuevo código se compila primero en memoria; solo se persiste tras una compilación exitosa y el reemplazo de la instancia.

### Reemplazo Atómico

El proceso de reemplazo es atómico:

1. Compilar nuevo código en memoria → obtener `Type`.
2. Crear nueva instancia desde `Type`.
3. Migrar estado de la instancia antigua a la nueva.
4. Intercambiar referencias.
5. Persistir el código cifrado.

Si cualquier paso falla, la instancia antigua permanece activa.

---

## Función de Retrollamada de Permisos

### Diseño

Cada PermissionManager tiene una **variable de función de retrollamada**:

- **Por defecto**: Apunta a la función de permisos por defecto integrada.
- **Tras compilación dinámica**: Sobrescrita por la función de permisos personalizada del ser.
- **Exclusiva**: Solo una retrollamada está activa en cualquier momento.
- **Fallo de compilación**: No afecta la retrollamada actual — la función por defecto o la última función personalizada exitosa permanece vigente.

### Firma de la Retrollamada

```
PermissionResult Callback(PermissionType type, string resourcePath, Guid callerId)
```

Devuelve `Allowed`, `Denied` o `AskUser`.

---

## Registro de Auditoría

Todas las decisiones de permisos se registran:

```
[2026-04-01 15:30:25] ALLOWED  | Being:AssistantA | Type:NetworkAccess | Resource:api.github.com | Source:HighAllowCache
[2026-04-01 15:30:26] DENIED   | Being:AssistantB | Type:FileAccess    | Resource:C:\Windows\System32 | Source:HighDenyCache
[2026-04-01 15:30:27] ASK_USER | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:Callback
[2026-04-01 15:30:28] ALLOWED  | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:UserDecision
```

Los registros se persisten en almacenamiento y se pueden ver a través de la Web UI (controlador de registros).

---

## Auditoría de Uso de Tokens

`TokenUsageAuditManager` proporciona seguimiento del consumo de tokens de IA relacionado con la seguridad:

- **Registro por solicitud** — Cada llamada a IA registra el ID del ser, modelo, tokens de prompt, tokens de completación y marca de tiempo.
- **Detección de anomalías** — Patrones de consumo de tokens inusuales pueden indicar inyección de prompts o abuso de recursos.
- **Acceso solo del Curador** — `TokenAuditTool` (marcado como `[SiliconManagerOnly]`) permite al Curador consultar y resumir el uso de tokens.
- **Panel web** — `UsageController` proporciona un panel basado en navegador con gráficos de tendencias y exportación de datos.
- **Almacenamiento persistente** — Los registros se almacenan a través de `ITimeStorage` para consultas de series temporales y análisis a largo plazo.

---

## Seguridad de Plugins

El sistema de plugins introduce riesgos de seguridad por la ejecución de código de terceros, mitigados mediante los siguientes mecanismos:

### Sandbox de Seguridad y Declaración de Capacidades

`PluginLoader` ejecuta escaneos de seguridad al cargar plugins y soporta simultáneamente el mecanismo de declaración de capacidades:

1. **Capacidades declarables** — Los plugins declaran las capacidades necesarias mediante el atributo `[PluginCapability]`:
   - `Network` — Acceso a la red (permite referencias a `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`)
   - `FileIO` — Lectura/escritura de archivos (permite referencias a `System.IO`)
   - `Process` — Gestión de procesos
   - `AI` — Llamadas a IA

2. **Capacidades no declarables** — Las siguientes capacidades siempre están bloqueadas:
   - P/Invoke (`System.Runtime.InteropServices`)
   - Código Unsafe (`System.Runtime.CompilerServices.Unsafe`)
   - Reflection Emit (`System.Reflection.Emit`)
   - API del compilador (`Microsoft.CodeAnalysis`)

3. **Lista blanca de ensamblados de confianza** — Se permiten referencias a los siguientes ensamblados:
   - `Google.Protobuf`, `Newtonsoft.Json`, `MessagePack`
   - `Serilog`, `Microsoft.Extensions.Logging.Abstractions`
   - `Dapper`

4. **Verificación de tipos prohibidos** — Escanea tipos peligrosos referenciados en el plugin

5. **Verificación de miembros prohibidos** — Escanea métodos peligrosos invocados en el plugin

### Carga Aislada

- Usa `AssemblyLoadContext` personalizado para cargar cada plugin de forma aislada
- Los tipos y ensamblados entre plugins no interfieren entre sí
- Al descargar un plugin se pueden liberar los recursos asociados

### Restricciones de Permisos de Herramientas

- Las herramientas registradas por plugins a través de la interfaz `ITool` están sujetas al mismo sistema de permisos
- Las herramientas de plugins no pueden eludir la cadena de verificación de permisos
- Las herramientas de plugins están sujetas a la marca `[SiliconManagerOnly]`

---

## Seguridad de Permisos de Herramientas

El sistema de permisos de herramientas proporciona una capa de seguridad adicional que controla qué operaciones de herramientas pueden usar los Seres de Silicio:

### Aislamiento de Permisos de Dos Niveles

1. **Nivel del Ser de Silicio** — Cada Ser de Silicio tiene una configuración de permisos de herramientas independiente
2. **Nivel de proyecto** — Los permisos de herramientas dentro del espacio del proyecto son independientes del nivel del Ser de Silicio, logrando aislamiento de permisos entre proyectos

### Plantillas de Permisos

El sistema proporciona plantillas de permisos predefinidas para garantizar una línea base de seguridad:

- **readonly** — Permisos mínimos, solo permite operaciones de lectura
- **restricted** — Permisos limitados, solo permite operaciones básicas
- **full** — Permisos completos (solo para uso del Curador)

### Características de Seguridad

- **Denegación por defecto** — Las operaciones de herramientas no explícitamente permitidas se deniegan por defecto
- **Granularidad de operaciones** — Cada operación de cada herramienta se controla independientemente (por ejemplo, `network:get` permitido pero `network:post` denegado)
- **Gestión del Curador** — Los permisos de herramientas solo pueden ser configurados por el Curador de Silicio
- **Pista de auditoría** — Los cambios en los permisos de herramientas se registran en el registro de auditoría
