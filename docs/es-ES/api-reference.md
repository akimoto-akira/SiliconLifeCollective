# Referencia de API

[English](api-reference.md) | [简体中文](docs/zh-CN/api-reference.md) | [繁體中文](docs/zh-HK/api-reference.md) | [Español](docs/es-ES/api-reference.md) | [日本語](docs/ja-JP/api-reference.md) | [한국어](docs/ko-KR/api-reference.md) | [Čeština](docs/cs-CZ/api-reference.md)

## Endpoints de API Web

URL Base: `http://localhost:8080`

---

## Autenticación

La mayoría de endpoints requieren autenticación mediante cookies de sesión gestionadas por la interfaz web.

---

## Gestión de Seres de Silicio

### Obtener Todos los Seres

**GET** `/api/beings`

**Respuesta**:
```json
{
  "beings": [
    {
      "id": "being-uuid",
      "name": "Asistente",
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
  "name": "Nuevo Ser",
  "soul": "# Personalidad\nEres útil..."
}
```

**Respuesta**: `201 Created`

### Iniciar Ser

**POST** `/api/beings/{id}/start`

**Respuesta**: `200 OK`

### Detener Ser

**POST** `/api/beings/{id}/stop`

**Respuesta**: `200 OK`

### Eliminar Ser

**DELETE** `/api/beings/{id}`

**Respuesta**: `204 No Content`

---

## Chat

### Enviar Mensaje

**POST** `/api/chat/{channelId}/message`

**Solicitud**:
```json
{
  "content": "Hola, ¿cómo estás?"
}
```

**Respuesta**: `200 OK`

### Obtener Mensajes

**GET** `/api/chat/{channelId}/messages?count=50`

**Respuesta**:
```json
{
  "messages": [
    {
      "id": "msg-uuid",
      "sender": "user",
      "content": "Hola",
      "timestamp": "2024-01-15T14:30:00Z"
    }
  ]
}
```

### Crear Canal

**POST** `/api/channels`

**Solicitud**:
```json
{
  "name": "Canal General"
}
```

**Respuesta**: `201 Created`

---

## Configuración

### Obtener Configuración

**GET** `/api/config`

**Respuesta**:
```json
{
  "AIClients": {
    "Ollama": {
      "BaseUrl": "http://localhost:11434",
      "Model": "qwen2.5:7b"
    }
  },
  "Language": "EsES",
  "WebHost": {
    "Port": 8080
  }
}
```

### Actualizar Configuración

**PUT** `/api/config`

**Solicitud**:
```json
{
  "Language": "en-US"
}
```

**Respuesta**: `200 OK`

---

## Permisos

### Obtener Solicitudes Pendientes

**GET** `/api/permission-requests`

**Respuesta**:
```json
{
  "requests": [
    {
      "id": "req-uuid",
      "beingId": "being-uuid",
      "permissionType": "FileAccess",
      "resource": "/data/file.txt",
      "timestamp": "2024-01-15T14:30:00Z"
    }
  ]
}
```

### Aprobar Solicitud

**POST** `/api/permission-requests/{id}/approve`

**Respuesta**: `200 OK`

### Rechazar Solicitud

**POST** `/api/permission-requests/{id}/deny`

**Respuesta**: `200 OK`

---

## Auditoría

### Obtener Registros de Permisos

**GET** `/api/audit/permissions?start=2024-01-01&end=2024-01-31`

**Respuesta**:
```json
{
  "logs": [
    {
      "timestamp": "2024-01-15T14:30:00Z",
      "beingId": "being-uuid",
      "permissionType": "FileAccess",
      "resource": "/data/file.txt",
      "result": "Allowed"
    }
  ]
}
```

### Obtener Uso de Tokens

**GET** `/api/audit/tokens?beingId=uuid&start=2024-01-01`

**Respuesta**:
```json
{
  "usage": {
    "totalTokens": 15000,
    "inputTokens": 10000,
    "outputTokens": 5000,
    "requests": 50
  }
}
```

---

## Herramientas

### Ejecutar Herramienta

**POST** `/api/tools/{toolName}/execute`

**Solicitud**:
```json
{
  "method": "GetValue",
  "parameters": {
    "key": "config_key"
  }
}
```

**Respuesta**:
```json
{
  "success": true,
  "message": "Éxito",
  "data": "valor"
}
```

### Listar Herramientas

**GET** `/api/tools`

**Respuesta**:
```json
{
  "tools": [
    {
      "name": "CalendarTool",
      "description": "Consulta de calendarios"
    },
    {
      "name": "ChatTool",
      "description": "Enviar mensajes"
    }
  ]
}
```

---

## Tareas

### Crear Tarea

**POST** `/api/tasks`

**Solicitud**:
```json
{
  "title": "Investigar API",
  "description": "Buscar documentación"
}
```

**Respuesta**: `201 Created`

### Actualizar Tarea

**PUT** `/api/tasks/{id}`

**Solicitud**:
```json
{
  "status": "completed"
}
```

**Respuesta**: `200 OK`

### Listar Tareas

**GET** `/api/tasks?status=active`

**Respuesta**:
```json
{
  "tasks": [
    {
      "id": "task-uuid",
      "title": "Investigar API",
      "status": "active",
      "created": "2024-01-15T14:30:00Z"
    }
  ]
}
```

---

## Temporizadores

### Configurar Temporizador

**POST** `/api/timers`

**Solicitud**:
```json
{
  "name": "recordatorio",
  "delay": "00:30:00",
  "action": "Enviar mensaje"
}
```

**Respuesta**: `201 Created`

### Cancelar Temporizador

**DELETE** `/api/timers/{name}`

**Respuesta**: `204 No Content`

---

## Logs

### Obtener Logs

**GET** `/api/logs?level=Error&count=100`

**Respuesta**:
```json
{
  "logs": [
    {
      "timestamp": "2024-01-15T14:30:00Z",
      "level": "Error",
      "message": "Error de conexión",
      "source": "NetworkExecutor"
    }
  ]
}
```

---

## Memoria

### Guardar en Memoria

**POST** `/api/memory`

**Solicitud**:
```json
{
  "key": "preferencia",
  "value": "respuestas cortas"
}
```

**Respuesta**: `200 OK`

### Cargar de Memoria

**GET** `/api/memory/{key}`

**Respuesta**:
```json
{
  "key": "preferencia",
  "value": "respuestas cortas"
}
```

---

## Sistema

### Obtener Información del Sistema

**GET** `/api/system/info`

**Respuesta**:
```json
{
  "version": "1.0.5",
  "uptime": "24:00:00",
  "memoryUsage": "256MB",
  "cpuUsage": "15%"
}
```

---

## Server-Sent Events (SSE)

### Suscribirse a Eventos

**GET** `/sse`

**Eventos**:

- `chat_message`: Nuevo mensaje
- `being_status`: Cambio de estado
- `permission_request`: Nueva solicitud
- `log_entry`: Nueva entrada de log

**Ejemplo JavaScript**:

```javascript
const eventSource = new EventSource('/sse');

eventSource.addEventListener('chat_message', (event) => {
    const message = JSON.parse(event.data);
    console.log('Nuevo mensaje:', message);
});
```

---

## Códigos de Error

| Código | Descripción |
|--------|-------------|
| 200 | Éxito |
| 201 | Creado |
| 204 | Sin contenido |
| 400 | Solicitud incorrecta |
| 401 | No autorizado |
| 403 | Prohibido |
| 404 | No encontrado |
| 500 | Error del servidor |

---

## Ejemplos de Uso

### cURL

```bash
# Obtener seres
curl http://localhost:8080/api/beings

# Crear ser
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{"name": "MiSer", "soul": "# Alma"}'

# Enviar mensaje
curl -X POST http://localhost:8080/api/chat/channel-id/message \
  -H "Content-Type: application/json" \
  -d '{"content": "Hola"}'
```

### JavaScript (Fetch)

```javascript
// Obtener seres
const response = await fetch('http://localhost:8080/api/beings');
const data = await response.json();

// Crear ser
await fetch('http://localhost:8080/api/beings', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name: 'MiSer', soul: '# Alma' })
});
```

---

## API de Notas de Trabajo

### Obtener Lista de Notas de Trabajo

**GET** `/api/beings/{id}/work-notes`

**Respuesta**:
```json
{
  "notes": [
    {
      "id": "note-uuid",
      "pageNumber": 1,
      "summary": "Módulo de autenticación completado",
      "keywords": ["autenticación", "JWT", "OAuth2"],
      "createdAt": "2026-04-25T10:00:00Z",
      "updatedAt": "2026-04-25T10:00:00Z"
    }
  ],
  "totalCount": 15
}
```

### Obtener Detalles de Una Nota

**GET** `/api/beings/{id}/work-notes/{pageNumber}`

**Respuesta**:
```json
{
  "id": "note-uuid",
  "pageNumber": 1,
  "summary": "Módulo de autenticación completado",
  "content": "## Detalles de implementación\n\n- Uso de token JWT\n- Soporte OAuth2",
  "keywords": ["autenticación", "JWT", "OAuth2"],
  "createdAt": "2026-04-25T10:00:00Z",
  "updatedAt": "2026-04-25T10:00:00Z"
}
```

### Crear Nueva Nota

**POST** `/api/beings/{id}/work-notes`

**Solicitud**:
```json
{
  "summary": "Módulo de autenticación completado",
  "content": "## Detalles de implementación\n\n- Uso de token JWT",
  "keywords": "autenticación,JWT,OAuth2"
}
```

**Respuesta**: `201 Created`

### Actualizar Nota

**PUT** `/api/beings/{id}/work-notes/{pageNumber}`

**Solicitud**:
```json
{
  "summary": "Módulo de autenticación y pruebas completados",
  "content": "## Contenido actualizado\n\nPruebas unitarias añadidas",
  "keywords": "autenticación,JWT,OAuth2,pruebas"
}
```

### Eliminar Nota

**DELETE** `/api/beings/{id}/work-notes/{pageNumber}`

### Buscar Notas

**GET** `/api/beings/{id}/work-notes/search?keyword=autenticación&maxResults=10`

### Obtener Directorio de Notas

**GET** `/api/beings/{id}/work-notes/directory`

---

## API de Red de Conocimiento

### Obtener Estadísticas de Conocimiento

**GET** `/api/knowledge/stats`

**Respuesta**:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Agregar Triple de Conocimiento

**POST** `/api/knowledge/triples`

**Solicitud**:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95,
  "tags": ["programming", "language"]
}
```

**Respuesta**: `201 Created`

### Consultar Conocimiento

**GET** `/api/knowledge/query?subject=Python&predicate=is_a`

**Respuesta**:
```json
{
  "triples": [
    {
      "subject": "Python",
      "predicate": "is_a",
      "object": "programming_language",
      "confidence": 0.95,
      "tags": ["programming", "language"]
    }
  ]
}
```

### Buscar Conocimiento

**GET** `/api/knowledge/search?query=programming+language&limit=10`

### Obtener Ruta de Conocimiento

**GET** `/api/knowledge/path?from=Python&to=computer_science`

**Respuesta**:
```json
{
  "path": [
    {"subject": "Python", "predicate": "is_a", "object": "programming_language"},
    {"subject": "programming_language", "predicate": "belongs_to", "object": "computer_science"}
  ],
  "length": 2
}
```

### Validar Conocimiento

**POST** `/api/knowledge/validate`

**Solicitud**:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

### Eliminar Conocimiento

**DELETE** `/api/knowledge/triples/{id}`

---

## API de Gestión de Proyectos

### Obtener Lista de Proyectos

**GET** `/api/projects`

**Respuesta**:
```json
{
  "projects": [
    {
      "id": "project-uuid",
      "name": "My Project",
      "description": "Project description",
      "createdAt": "2026-04-25T10:00:00Z"
    }
  ]
}
```

### Crear Proyecto

**POST** `/api/projects`

**Solicitud**:
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### Obtener Detalles del Proyecto

**GET** `/api/projects/{id}`

### Actualizar Proyecto

**PUT** `/api/projects/{id}`

### Eliminar Proyecto

**DELETE** `/api/projects/{id}`
