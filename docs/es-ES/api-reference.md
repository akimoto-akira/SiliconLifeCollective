# Referencia de API

> **Versión: v0.2.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | **Español** | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md) | [Русский](../ru-RU/api-reference.md)

## Endpoints de Web API

URL base: `http://localhost:8080`

### Autenticación

La mayoría de los endpoints requieren autenticación a través de cookie de sesión gestionada por Web UI.

---

## Gestión de Seres Silicona

### Obtener Todos los Seres

**GET** `/api/beings`

**Respuesta**:
```json
{
  "beings": [
    {
      "id": "being-uuid",
      "name": "Assistant",
      "activity": "Idle",
      "soul": "path/to/soul.md"
    }
  ]
}
```

**Valores de actividad**: `Idle` | `SingleChat` | `GroupChat` | `Task` | `Timer` | `Broadcast` | `Project` | `MemoryCompression` | `Stopped`

### Crear Ser

**POST** `/api/beings`

**Solicitud**:
```json
{
  "name": "New Being",
  "soul": "# Personality\nYou are helpful..."
}
```

**Respuesta**: `201 Created`

### Iniciar Ser

**POST** `/api/beings/{id}/start`

### Detener Ser

**POST** `/api/beings/{id}/stop`

### Obtener Detalles del Ser

**GET** `/api/beings/{id}`

---

## Sistema de Chat

### Enviar Mensaje

**POST** `/api/chat/send`

**Solicitud**:
```json
{
  "channelId": "session-uuid",
  "content": "Hello, how are you?"
}
```

**Respuesta**:
```json
{
  "success": true,
  "messageId": "50156b26-f3b9-4735-be3d-51e547bd3a4a"
}
```

### Obtener Lista de Sesiones

**GET** `/api/chat/conversations`

**Respuesta**:
```json
[
  {
    "sessionId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
    "beingId": "being-uuid",
    "type": "single",
    "displayName": "Chat con Being",
    "lastMessage": "Último mensaje",
    "lastTime": "2026-04-20T10:30:00Z"
  }
]
```

### Obtener Historial de Mensajes

**GET** `/api/chat/messages?channelId={sessionId}`

**Respuesta**:
```json
{
  "messages": [
    {
      "role": "user",
      "content": "Hello",
      "timestamp": "2026-04-20T10:30:00Z"
    },
    {
      "role": "assistant",
      "content": "Hi there!",
      "timestamp": "2026-04-20T10:30:05Z"
    }
  ]
}
```

### Chat en Streaming (SSE)

**GET** `/api/chat/stream`

Eventos enviados por el servidor para actualizaciones de chat en tiempo real.

### Obtener Historial de Chat

**GET** `/api/chat/history`

Devuelve sesiones de historial de chat.

### Detener Pensamiento de IA

**POST** `/api/chat/stop`

Detiene la respuesta de streaming de IA actual.

### Subir Archivo

**POST** `/api/chat/upload`

Sube un archivo a la sesión de chat.

---

## Configuración

### Obtener Configuración

**GET** `/api/config`

**Respuesta**:
```json
{
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:7b"
    }
  },
  "storage": {
    "basePath": "./data"
  }
}
```

### Actualizar Configuración

**POST** `/api/config`

**Solicitud**:
```json
{
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:14b"
    }
  }
}
```

---

## Sistema de Permisos

### Obtener Permisos

**GET** `/api/permissions`

**Respuesta**:
```json
{
  "rules": [
    {
      "userId": "user-uuid",
      "resource": "disk:read",
      "allowed": true,
      "expiresAt": "2026-04-21T00:00:00Z"
    }
  ]
}
```

### Conceder Permiso

**POST** `/api/permissions`

**Solicitud**:
```json
{
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "duration": 3600
}
```

### Revocar Permiso

**DELETE** `/api/permissions/{id}`

### Verificar Permiso

**POST** `/api/permissions/check`

**Solicitud**:
```json
{
  "userId": "user-uuid",
  "resource": "network:http"
}
```

**Respuesta**:
```json
{
  "allowed": true,
  "reason": "Granted by curator"
}
```

---

## Sistema de Tareas y Temporizadores

### Crear Tarea

**POST** `/api/tasks`

**Solicitud**:
```json
{
  "beingId": "being-uuid",
  "description": "Review code",
  "priority": 5,
  "dueDate": "2026-04-21T12:00:00Z"
}
```

### Obtener Tareas

**GET** `/api/tasks?beingId={id}&status=pending`

### Actualizar Estado de Tarea

**PATCH** `/api/tasks/{id}`

**Solicitud**:
```json
{
  "status": "completed"
}
```

### Crear Temporizador

**POST** `/api/timers`

**Solicitud**:
```json
{
  "beingId": "being-uuid",
  "interval": 3600,
  "action": "think",
  "repeat": true
}
```

### Eliminar Temporizador

**DELETE** `/api/timers/{id}`

---

## Auditoría y Registros

### Obtener Uso de Tokens

**GET** `/api/audit/tokens?startDate={date}&endDate={date}`

**Respuesta**:
```json
{
  "summary": {
    "totalTokens": 150000,
    "promptTokens": 100000,
    "completionTokens": 50000,
    "totalCost": 0.15
  },
  "byModel": {
    "qwen2.5:7b": {
      "tokens": 100000,
      "cost": 0.10
    }
  }
}
```

### Obtener Registros

**GET** `/api/logs?level=error&limit=100`

**Respuesta**:
```json
{
  "logs": [
    {
      "timestamp": "2026-04-20T10:30:00Z",
      "level": "error",
      "message": "Failed to connect to AI service",
      "source": "OllamaClient"
    }
  ]
}
```

---

## API de Almacenamiento

### Leer Valor

**GET** `/api/storage?key={key}`

**Respuesta**:
```json
{
  "key": "being:uuid:memory",
  "value": "{...}",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### Escribir Valor

**POST** `/api/storage`

**Solicitud**:
```json
{
  "key": "being:uuid:memory",
  "value": "{...}"
}
```

### Consultar por Rango de Tiempo

**GET** `/api/storage/time?start={start}&end={end}&prefix={prefix}`

**Respuesta**:
```json
{
  "entries": [
    {
      "key": "being:uuid:chat:2026-04-20",
      "value": "{...}",
      "timestamp": "2026-04-20T10:30:00Z"
    }
  ]
}
```

---

## API de Memoria

### Obtener Lista de Memoria

**GET** `/api/memory/list`

**Parámetros de Consulta**: `beingId`, `type`, `limit`

### Obtener Detalle de Memoria

**GET** `/api/memory/detail/{id}`

### Obtener Estadísticas de Memoria

**GET** `/api/memory/stats`

**Parámetros de Consulta**: `beingId`

### Buscar Memoria

**GET** `/api/memory/search`

**Parámetros de Consulta**: `beingId`, `keyword`, `limit`

### Obtener Beings con Memoria

**GET** `/api/memory/beings`

Devuelve lista de beings con datos de memoria.

### Rastrear Origen de Memoria

**GET** `/api/memory/trace/{id}`

Rastrea la fuente original de una entrada de memoria.

### Obtener HTML de Línea de Tiempo de Memoria

**GET** `/api/memory/timeline-html`

**Parámetros de Consulta**: `beingId`

Devuelve fragmento HTML para visualización de línea de tiempo de memoria.

---

## API de Navegador de Código

### Obtener Tipos de Código

**GET** `/api/code/types`

Devuelve todos los tipos disponibles para navegación de código.

### Obtener Detalle de Código

**GET** `/api/code/detail`

**Parámetros de Consulta**: `type`, `member`

Devuelve información detallada sobre un tipo o miembro específico.

---

## API de Ejecutores

### Obtener Estado de Ejecutores

**GET** `/api/executors/status`

**Respuesta**:
```json
[
  { "name": "DiskExecutor", "status": "Idle", "queueCount": 0 },
  { "name": "NetworkExecutor", "status": "Idle", "queueCount": 0 },
  { "name": "CommandLineExecutor", "status": "Idle", "queueCount": 0 }
]
```

---

## Información del Sistema

### Obtener Página Acerca de

**GET** `/about`

Devuelve la página acerca de, incluyendo información del sistema y la lista de plugins cargados.

**Datos de Lista de Plugins**:
```json
{
  "plugins": {
    "plugin-id": {
      "name": "My Plugin",
      "version": "1.0.0",
      "description": "Plugin description",
      "author": "Author Name"
    }
  }
}
```

### Solicitud de Permiso

**GET** `/permission/request?userId={id}&type={type}&resource={resource}`

Muestra la página de solicitud de permiso, permitiendo al usuario aprobar o denegar la solicitud de permiso del Ser Silicona.

**Parámetros de Consulta**:

| Parámetro | Tipo | Descripción |
|------|------|------|
| `userId` | `Guid` | ID del Ser Silicona que solicita el permiso |
| `type` | `string` | Tipo de permiso |
| `resource` | `string` | Ruta del recurso solicitado |
| `allowCode` | `string` | Código de identificación de la operación permitida |
| `denyCode` | `string` | Código de identificación de la operación denegada |

**Verificar Solicitud de Permiso Pendiente**:

**GET** `/permission/check?userId={id}`

**Respuesta**:
```json
{
  "pending": true
}
```

**Responder Solicitud de Permiso**:

**GET** `/permission/respond?userId={id}&allowed={bool}&addToCache={bool}&cacheDuration={hours}`

**Parámetros de Consulta**:

| Parámetro | Tipo | Descripción |
|------|------|------|
| `userId` | `Guid` | ID del Ser Silicona |
| `allowed` | `bool` | Si se permite |
| `addToCache` | `bool` | Si se almacena en caché la decisión |
| `cacheDuration` | `double` | Duración de la caché (horas) |

**Respuesta**:
```json
{
  "success": true
}
```

### Obtener Datos del Panel de Control

**GET** `/api/dashboard`

**Respuesta**:
```json
{
  "beings": {
    "total": 5,
    "running": 3,
    "stopped": 2
  },
  "performance": {
    "cpu": 45.2,
    "memory": 1024,
    "uptime": 86400
  },
  "aiUsage": {
    "todayTokens": 50000,
    "todayCost": 0.05
  }
}
```

### Obtener Estado del Sistema

**GET** `/api/status`

**Respuesta**:
```json
{
  "version": "1.0.0",
  "runtime": ".NET 9.0",
  "uptime": 86400,
  "health": "healthy"
}
```

---

## Respuestas de Error

Todos los endpoints devuelven respuestas de error estandarizadas:

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: disk:write, Current: disk:read"
  }
}
```

### Códigos de Error Comunes

| Código | Estado HTTP | Descripción |
|------|-------------|-------------|
| `PERMISSION_DENIED` | 403 | Permisos insuficientes |
| `NOT_FOUND` | 404 | Recurso no encontrado |
| `VALIDATION_ERROR` | 400 | Parámetros de solicitud inválidos |
| `INTERNAL_ERROR` | 500 | Error interno del servidor |
| `SERVICE_UNAVAILABLE` | 503 | Servicio de IA no disponible |
