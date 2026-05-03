# Referencia de API

> **Versión: v0.1.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | **Español** | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md)

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
      "status": "running",
      "soul": "path/to/soul.md"
    }
  ]
}
```

**Valores de estado**: `idle` | `running` | `waiting_permission` | `stopped`

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

**POST** `/api/chat`

**Solicitud**:
```json
{
  "beingId": "being-uuid",
  "message": "Hello, how are you?",
  "sessionId": "optional-session-id"
}
```

**Respuesta** (no streaming):
```json
{
  "reply": "I'm doing well, thank you!",
  "sessionId": "session-uuid",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### Chat en Streaming (SSE)

**GET** `/api/chat/stream?beingId={id}&message={msg}`

**Respuesta**: Stream de eventos enviados por el servidor

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### Obtener Historial de Chat

**GET** `/api/chat/{sessionId}/history`

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
