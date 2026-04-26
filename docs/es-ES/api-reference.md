# Referencia de API

[English](../en/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | **Español** | [Deutsch](../de-DE/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md)

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

**GET** `/api/chat/history?beingId={id}&sessionId={sid}`

---

## Sistema de Configuración

### Obtener Configuración

**GET** `/api/config`

### Actualizar Configuración

**POST** `/api/config`

**Solicitud**:
```json
{
  "key": "AIClients.Ollama.Model",
  "value": "qwen2.5:7b"
}
```

---

## Sistema de Permisos

### Obtener Permisos

**GET** `/api/permissions?userId={uid}`

### Añadir Regla de Permiso

**POST** `/api/permissions`

**Solicitud**:
```json
{
  "userId": "user-uuid",
  "resource": "disk:read",
  "allowed": true,
  "duration": 3600
}
```

### Eliminar Regla de Permiso

**DELETE** `/api/permissions/{rule-id}`

---

## Sistema de Tareas

### Obtener Tareas

**GET** `/api/tasks?beingId={id}`

### Crear Tarea

**POST** `/api/tasks`

**Solicitud**:
```json
{
  "beingId": "being-uuid",
  "description": "Review code",
  "priority": 5,
  "dueDate": "2026-04-21T00:00:00Z"
}
```

### Actualizar Tarea

**PUT** `/api/tasks/{task-id}`

---

## Sistema de Temporizadores

### Obtener Temporizadores

**GET** `/api/timers?beingId={id}`

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

---

## Sistema de Memoria

### Obtener Memoria

**GET** `/api/memory?beingId={id}&date={date}`

### Buscar Memoria

**GET** `/api/memory/search?beingId={id}&query={q}`

---

## Sistema de Conocimiento

### Añadir Conocimiento

**POST** `/api/knowledge`

**Solicitud**:
```json
{
  "subject": "Python",
  "relation": "es_un",
  "object": "lenguaje_de_programación"
}
```

### Consultar Conocimiento

**GET** `/api/knowledge?subject={s}`

### Obtener Ruta

**GET** `/api/knowledge/path?from={subject1}&to={subject2}`

---

## Sistema de Registros

### Obtener Registros

**GET** `/api/logs?level={level}&from={date}&to={date}`

---

## Sistema de Auditoría

### Obtener Uso de Tokens

**GET** `/api/audit/tokens?beingId={id}&from={date}&to={date}`

### Obtener Resumen

**GET** `/api/audit/summary?beingId={id}`

---

## Panel de Control

### Obtener Métricas del Sistema

**GET** `/api/dashboard`

**Respuesta**:
```json
{
  "cpu": 45.2,
  "memory": 1024,
  "uptime": 86400,
  "beings": {
    "total": 5,
    "running": 3
  }
}
```

### Eventos SSE

**GET** `/api/dashboard/events`

---

## Sistema de Archivos

### Listar Archivos

**GET** `/api/files?path={path}`

### Leer Archivo

**GET** `/api/files/read?path={path}`

### Escribir Archivo

**POST** `/api/files/write`

**Solicitud**:
```json
{
  "path": "/path/to/file.txt",
  "content": "File content"
}
```

---

## Sistema de Proyectos

### Obtener Proyectos

**GET** `/api/projects`

### Crear Proyecto

**POST** `/api/projects`

**Solicitud**:
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

---

## Sistema de Notas de Trabajo

### Obtener Notas

**GET** `/api/worknotes?beingId={id}`

### Crear Nota

**POST** `/api/worknotes`

**Solicitud**:
```json
{
  "beingId": "being-uuid",
  "title": "Nota title",
  "content": "# Content\n\nNote content...",
  "keywords": ["keyword1", "keyword2"]
}
```

### Buscar Notas

**GET** `/api/worknotes/search?beingId={id}&query={q}`

### Generar Índice

**GET** `/api/worknotes/index?beingId={id}`

---

## Códigos de Error

| Código | Descripción |
|--------|-------------|
| 200 | Éxito |
| 201 | Creado |
| 400 | Solicitud incorrecta |
| 401 | No autorizado |
| 403 | Prohibido |
| 404 | No encontrado |
| 500 | Error interno del servidor |

---

## Próximos Pasos

- 📚 Leer la [Guía de Arquitectura](architecture.md)
- 🛠️ Consultar la [Guía de Desarrollo](development-guide.md)
- 🔧 Ver la [Referencia de Herramientas](tools-reference.md)
- 🚀 Comenzar con la [Guía de Inicio Rápido](getting-started.md)
