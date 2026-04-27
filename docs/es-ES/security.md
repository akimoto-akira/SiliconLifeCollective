# Diseño de Seguridad

> **Versión: v0.1.0-alpha**

[English](../en/security.md) | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | **Español** | [Deutsch](../de-DE/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | [Čeština](../cs-CZ/security.md)

## Resumen

La seguridad de Silicon Life Collective se basa en un modelo de **defensa en profundidad**. Principio central: **todas las operaciones de E/S deben pasar por ejecutores**, y los ejecutores refuerzan verificaciones de permisos antes de la ejecución.

```
Invocación de herramienta → Ejecutor → Gestor de permisos → caché de alta denegación → caché de alta允许 → callback → preguntar al usuario
```

---

## Modelo de Permisos

### Tipos de Permisos

| Tipo | Descripción |
|------|-------------|
| `NetworkAccess` | Solicitudes HTTP/HTTPS salientes |
| `CommandLine` | Ejecución de comandos de shell |
| `FileAccess` | Operaciones de archivos y directorios |
| `Function` | Llamadas a funciones sensibles |
| `DataAccess` | Acceso a datos del sistema o usuario |

### Resultados de Permisos

Cada verificación de permisos devuelve uno de tres resultados:

| Resultado | Comportamiento |
|--------|----------|
| [Deutsch](../de-DE/security.md) | **Allowed (Permitido)** | La operación procede inmediatamente |
| [Deutsch](../de-DE/security.md) | **Denied (Denegado)** | La operación se bloquea, se registra en auditoría |
| [Deutsch](../de-DE/security.md) | **AskUser (Preguntar al usuario)** | La operación se pausa, requiere confirmación del usuario |

### Rol Especial: Curador Silicona

El Curador Silicona tiene el nivel más alto de permisos (`IsCurator = true`). Las verificaciones de permisos del curador se cortocircuitan a **Permitido**, a menos que el usuario las sobrescriba explícitamente.

### Gestor de Permisos Privado

Cada Ser Silicona tiene su propia instancia **privada de PermissionManager**. El estado de permisos no se comparte entre seres.

---

## Flujo de Verificación de Permisos

Prioridad de consulta: **1. Alta denegación de usuario → 2. Alta permitida de usuario → 3. Función callback**

```
┌─────────────┐
│ Invocación   │
│ de herramienta│
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Ejecutor    │────▶│ Gestor de Permisos  │
│(Disco/Red/   │     │ Privado (por ser)   │
│ Línea cmd...)│     └────────┬────────────┘
└─────────────┘            │
                           ▼
                  ┌─────────────────┐
                  │ 1. ¿IsCurator?  │──Sí──▶ Permitido
                  └────────┬────────┘
                           │ No
                           ▼
                  ┌─────────────────┐
                  │ 2. Alta         │──Coincide──▶ Denegado
                  │ Denegación      │
                  │ (caché memoria) │
                  └────────┬────────┘
                           │ Sin coincidencia
                           ▼
                  ┌─────────────────┐
                  │ 3. Alta         │──Coincide──▶ Permitido
                  │ Permitida       │
                  │ (caché memoria) │
                  └────────┬────────┘
                           │ Sin coincidencia
                           ▼
                  ┌─────────────────┐
                  │ 4. Función      │
                  │ Callback de     │──▶ Permitido / Denegado / Preguntar
                  │ Permisos        │
                  └─────────────────┘
```

**Punto clave**: Los ejecutores solo ven booleanos (permitido/denegado). El gestor de permisos maneja internamente la decisión ternaria (permitido/denegado/preguntar) y resuelve preguntar al usuario antes de devolver al ejecutor.

---

## Ejecutores (Límites de Seguridad)

Los ejecutores son el **único** camino para operaciones de E/S. Refuerzan:

### Hilos de Programación Independientes

Cada ejecutor tiene su **propio hilo de programación independiente**:

- Aislamiento de hilos entre ejecutores — el bloqueo del hilo de un ejecutor no afecta a otros ejecutores.
- Cada ejecutor puede establecer límites de recursos independientes (CPU, memoria, etc.).
- Gestión de pool de hilos para hilos de ejecutores.

### Cola de Solicitudes

Cada ejecutor mantiene una cola de solicitudes:

- Las solicitudes se enrutan por tipo al ejecutor correspondiente.
- Soporte para cola prioritaria.
- Control de timeout por solicitud.

### Bloqueo de Hilos para Verificación de Permisos

Cuando una herramienta inicia acceso a recursos:

1. El ejecutor recibe la solicitud y **bloquea su hilo**.
2. El ejecutor consulta el gestor de permisos privado del ser.
3. Si el callback devuelve preguntar al usuario, el hilo del ejecutor **permanece bloqueado** esperando respuesta del usuario.
4. El ser solo ve el resultado final (éxito o denegado) — nunca ve el estado intermedio "pendiente" o "esperando".
5. Solo el Curador Silicona activará prompts reales de usuario. Los seres normales consultan la ACL global sincrónicamente sin bloquear.
6. En timeout, la solicitud se trata como denegada, y el bloqueo del hilo se libera.

### Tipos de Ejecutores

| Ejecutor | Alcance | Timeout Predeterminado |
|----------|-------|-----------------|
| `DiskExecutor` | Lectura/escritura de archivos, operaciones de directorio | 30 segundos |
| `NetworkExecutor` | Solicitudes HTTP, conexiones WebSocket | 60 segundos |
| `CommandLineExecutor` | Ejecución de comandos de shell | 120 segundos |
| `DynamicCompilationExecutor` | Compilación en memoria Roslyn | 60 segundos |

### Aislamiento de Excepciones y Tolerancia a Fallos

- Las excepciones de un ejecutor no afectan a otros ejecutores.
- Reinicio automático cuando los hilos colapsan.
- Cortacircuitos: detener temporalmente ejecutores después de fallos consecutivos para prevenir fallos en cascada.

---

## ACL Global (Lista de Control de Acceso)

Tabla de reglas compartidas persistida al almacenamiento, gestionada solo por el Curador Silicona:

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
- Solo el Curador Silicona puede modificar la ACL global (a través de su herramienta dedicada).
- Los cambios surten efecto inmediatamente.
- La ACL global **no está** en la cadena de prioridad para cada consulta mencionada arriba — es referenciada internamente por la función callback.

---

## Caché de Frecuencia de Usuario

Para reducir prompts de permisos repetitivos, el sistema mantiene dos cachés **por ser, solo en memoria**:

| Caché | Uso |
|-------|---------|
| [Deutsch](../de-DE/security.md) | **HighAllow (Alta Permitida)** | Recursos frecuentemente permitidos por el usuario |
| [Deutsch](../de-DE/security.md) | **HighDeny (Alta Denegación)** | Recursos frecuentemente denegados por el usuario |

### Cómo Funciona

- **Selección de usuario, no detección automática**: Cuando se activa preguntar al usuario, el usuario elige si añadir el recurso al caché.
- **Coincidencia de prefijo**: Soporte para coincidencia de prefijo de ruta de recurso (ej. `network:api.example.com/*`).
- **Prioridad**: Alta denegación tiene prioridad sobre alta permitida.
- **Solo memoria**: Los cachés no persisten. Se pierden en reinicio.
- **Expiración configurable**: Los usuarios pueden establecer período de validez para entradas de caché.

### Flujo de Actualización de Caché

1. Callback de permisos devuelve `AskUser`.
2. El sistema de permisos envía una consulta al sistema de tarjetas (Web UI o mensajería instantánea).
3. El usuario toma una decisión (permitir/denegar) y **elige si cachear o no**.
4. El sistema de tarjetas devuelve decisión + bandera de caché.
5. El sistema de permisos actualiza la lista de caché correspondiente.
6. Solicitudes futuras que coincidan con prefijos de caché se resuelven inmediatamente.

---

## Mecanismo de Pregunta al Usuario

Cuando la verificación de permisos devuelve `AskUser`:

### Web UI: Tarjetas Interactivas

El frontend web muestra inmediatamente una **tarjeta interactiva** mostrando:

- Tipo y ruta del recurso
- Descripción de la operación
- Botones de Permitir / Denegar
- Casilla de verificación opcional "Permitir siempre" / "Denegar siempre" (añadir al caché de frecuencia)

### Mensajería Instantánea (sin soporte de tarjetas): Código Aleatorio

Para plataformas de mensajería que no soportan tarjetas interactivas:

1. El sistema genera dos códigos aleatorios de 6 dígitos: **código de permitir** y **código de denegar**.
2. Envía un mensaje con información del recurso y ambos códigos.
3. El usuario debe responder con el código exacto de permitir para autorizar. Cualquier otra respuesta se trata como denegación.
4. Los códigos son de un solo uso para prevenir ataques de repetición.

### Timeout

- Se establece timeout para todas las solicitudes de preguntar al usuario.
- En timeout, la solicitud se trata como **denegada**, y el bloqueo del hilo del ejecutor se libera.

---

## Seguridad de Compilación Dinámica

La auto-evolución (reemplazo de clases) introduce riesgos de seguridad únicos. El sistema los mitiga usando una **estrategia en capas**:

### Capa 1: Control de Referencias en Compilación (Defensa Principal)

- El compilador solo obtiene una **lista permitida de referencias de ensamblados**.
- **Permitidos**: `System.Runtime`, `System.Private.CoreLib`, ensamblados del proyecto (interfaz ITool, etc.)
- **Bloqueados**: `System.IO`, `System.Reflection`, `System.Runtime.InteropServices`, etc.
- Si el código referencia un ensamblado bloqueado, el **compilador mismo rechaza** el código.
- Esto es más confiable que escaneo en tiempo de ejecución — operaciones peligrosas son imposibles a nivel de tipo.

### Capa 2: Análisis Estático en Tiempo de Ejecución (Defensa Secundaria)

- Incluso después de compilación exitosa, el código se escanea para patrones estáticos.
- Detecta patrones de operaciones peligrosas (E/S directa, llamadas al sistema, etc.).
- Si se encuentra código peligroso, la carga se rechaza, el sistema vuelve a funcionalidad predeterminada.

### Restricción de Herencia

Todas las clases personalizadas de Ser Silicona **deben** heredar de `SiliconBeingBase`. El compilador refuerza esta restricción a nivel de tipo.

### Almacenamiento Cifrado

El código compilado se almacena cifrado en disco usando AES-256:

- **Derivación de clave**: Desde el GUID del ser (en mayúsculas) usando PBKDF2.
- **Fallo de descifrado**: Volver a implementación predeterminada.
- **Recompilación en tiempo de ejecución**: El nuevo código se compila primero en memoria; solo persiste después de compilación exitosa y reemplazo de instancia.

### Reemplazo Atómico

El proceso de reemplazo es atómico:

1. Compilar nuevo código en memoria → obtener `Type`.
2. Crear nueva instancia desde `Type`.
3. Migrar estado de instancia antigua a nueva instancia.
4. Intercambiar referencias.
5. Persistir código cifrado.

Si algún paso falla, la instancia antigua permanece activa.

---

## Funciones Callback de Permisos

### Diseño

Cada PermissionManager mantiene una **variable de función callback**:

- **Predeterminado**: Apunta a la función de permisos predeterminada incorporada.
- **Después de compilación dinámica**: Sobrescrito por la función de permisos personalizada del ser.
- **Uno u otro**: Solo un callback está activo en cualquier momento.
- **Fallo de compilación**: No afecta el callback actual — predeterminado o la última función personalizada exitosa permanece vigente.

### Firma del Callback

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

Los registros persisten al almacenamiento y son visibles a través de la Web UI (controlador de registros).

---

## Auditoría de Uso de Tokens

`TokenUsageAuditManager` proporciona seguimiento de consumo de tokens de IA relacionado con seguridad:

- **Registro por solicitud** — Cada llamada de IA registra ID de ser, modelo, tokens de prompt, tokens de completación y marca de tiempo.
- **Detección de anomalías** — Patrones de consumo de tokens inusuales pueden indicar inyección de prompt o abuso de recursos.
- **Acceso solo para curador** — `TokenAuditTool` (marcado con `[SiliconManagerOnly]`) permite al curador consultar y resumir uso de tokens.
- **Panel web** — `AuditController` proporciona panel basado en navegador con gráficos de tendencias y exportación de datos.
- **Almacenamiento persistente** — Registros almacenados a través de `ITimeStorage` para consultas de series de tiempo y análisis a largo plazo.
