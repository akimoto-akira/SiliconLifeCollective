# Referencia de API

> **Versión: v0.2.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | **Español** | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md) | [Русский](../ru-RU/api-reference.md)

## Endpoints de la API Web

URL base: `http://localhost:8080`

### Autenticación

La mayoría de los endpoints requieren autenticación mediante cookies de sesión gestionadas a través de la Web UI. Antes de la inicialización del sistema, todas las solicitudes, excepto la página de ayuda, serán redirigidas a la página de inicialización.

---

## Panel

### Obtener estadísticas del panel

**GET** `/api/dashboard/stats`

Retorna datos de resumen del sistema (cantidad de seres, estado de ejecución, etc.).

### Obtener métricas de rendimiento

**GET** `/api/dashboard/metrics`

Retorna datos de métricas de rendimiento en tiempo real.

---

## Sistema de Chat

### Página de chat

**GET** `/chat`

Retorna la página de la interfaz de chat.

### Chat en flujo (SSE)

**GET** `/api/chat/stream`

Chat en flujo mediante Server-Sent Events (SSE).

**Respuesta**: Flujo de eventos enviados por el servidor

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### Obtener lista de conversaciones

**GET** `/api/chat/conversations`

Retorna la lista de todas las sesiones de chat activas.

**Ejemplo de respuesta**:
```json
{
  "conversations": [
    {
      "sessionId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
      "beingId": "being-uuid",
      "type": "single",
      "displayName": "Chatear con Xiaoyou",
      "lastMessage": "Contenido del último mensaje",
      "lastTime": "2026-05-20T10:30:00Z"
    }
  ]
}
```

### Obtener historial de mensajes

**GET** `/api/chat/messages`

Parámetro de consulta: `channelId` — ID del canal/sesión

Retorna el historial de mensajes de la sesión especificada.

### Obtener historial de chat

**GET** `/api/chat/history`

Retorna el historial global de chat.

### Enviar mensaje

**POST** `/api/chat/send`

**Cuerpo de la solicitud**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
  "content": "Contenido del mensaje de prueba"
}
```

**Respuesta**:
```json
{
  "success": true,
  "messageId": "50156b26-f3b9-4735-be3d-51e547bd3a4a"
}
```

### Detener pensamiento de IA

**POST** `/api/chat/stop`

Detiene la generación de respuesta de IA en curso.

**Cuerpo de la solicitud**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d"
}
```

### Cargar archivo

**POST** `/api/chat/upload`

Carga un archivo a la sesión de chat (soporta multipart/form-data).

---

## Gestión de Seres de Silicio

### Página de gestión de seres

**GET** `/beings`

Retorna la página de la interfaz de gestión de Seres de Silicio.

### Obtener lista de seres

**GET** `/api/beings` o **GET** `/api/beings/list`

Retorna la lista de todos los Seres de Silicio registrados.

**Ejemplo de respuesta**:
```json
{
  "beings": [
    {
      "id": "being-uuid",
      "name": "Assistant",
      "status": "running",
      "soulPath": "path/to/soul.md"
    }
  ]
}
```

**Valores de estado**: `idle` | `running` | `waiting_permission` | `stopped`

### Obtener detalles del ser

**GET** `/api/beings/detail`

Parámetro de consulta: `beingId` — ID del Ser de Silicio

Retorna información detallada del Ser de Silicio especificado.

### Obtener estado de actividad del ser

**GET** `/api/beings/activity`

Retorna la información del estado de actividad de cada Ser de Silicio.

### Página del editor de Archivo de Alma

**GET** `/beings/soul`

Retorna la interfaz del editor de Archivo de Alma.

### Guardar Archivo de Alma

**POST** `/api/beings/soul/save`

**Cuerpo de la solicitud**:
```json
{
  "beingId": "being-uuid",
  "soulContent": "# Personality\nYou are helpful..."
}
```

### Página del editor de configuración de IA

**GET** `/beings/ai-config`

Retorna la interfaz del editor de configuración de IA.

### Guardar configuración de IA

**POST** `/api/beings/ai-config/save`

**Cuerpo de la solicitud**:
```json
{
  "beingId": "being-uuid",
  "aiClientType": "DashScope",
  "config": {
    "apiKey": "...",
    "region": "beijing",
    "model": "qwen3.6-plus"
  }
}
```

### Obtener lista de modelos de IA disponibles

**GET** `/api/beings/ai-config/models`

Parámetros de consulta: `clientType`, `apiKey`, `region`

Retorna la lista de modelos disponibles para el cliente de IA especificado.

---

## Visualización del Historial de Chat

### Página del historial de chat

**GET** `/chat-history`

Retorna la página principal del historial de chat.

### Página de detalles del historial de chat

**GET** `/chat-history-detail`

Retorna la página de detalles del historial de chat de la sesión especificada.

### Página de detalles del historial de chat grupal

**GET** `/group-chat-history-detail`

Retorna la página de detalles del historial de chat grupal.

### Página de detalles del historial de difusión

**GET** `/broadcast-history-detail`

Retorna la página de detalles del historial del canal de difusión.

### Obtener lista de sesiones históricas

**GET** `/api/chat-history/conversations`

Retorna la lista de todas las sesiones históricas.

### Obtener mensajes históricos

**GET** `/api/chat-history/messages`

Parámetro de consulta: `sessionId` — ID de la sesión

Retorna los registros de mensajes de la sesión histórica especificada.

---

## Gestión de Temporizadores

### Página de temporizadores

**GET** `/timers`

Retorna la página de la interfaz de gestión de temporizadores.

### Obtener lista de temporizadores

**GET** `/api/timers/list`

Retorna la lista de todos los temporizadores.

### Página de detalles del ciclo del temporizador

**GET** `/timer-cycles/{timerId}`

Retorna la página de detalles del ciclo de ejecución del temporizador especificado.

### Obtener lista de ciclos del temporizador

**GET** `/api/timer-cycles/list`

Parámetro de consulta: `timerId` — ID del temporizador

Retorna la lista de todos los ciclos de ejecución del temporizador especificado.

### Página de detalles de un ciclo de ejecución individual

**GET** `/timer-cycle/{cycleIndex}`

Retorna la página de detalles de una ejecución individual.

### Obtener mensajes del ciclo

**GET** `/api/timer-cycle/messages`

Parámetro de consulta: `cycleIndex` — Índice del ciclo

Retorna los mensajes relacionados con el ciclo de ejecución especificado.

---

## Gestión de Tareas

### Página de tareas

**GET** `/tasks`

Retorna la página de la interfaz de gestión de tareas.

### Obtener lista de tareas

**GET** `/api/tasks/list`

Retorna la lista de todas las tareas.

### Página de detalles del ciclo de la tarea

**GET** `/task-cycles/{taskId}`

Retorna la página de detalles del ciclo de ejecución de la tarea especificada.

### Obtener lista de ciclos de la tarea

**GET** `/api/task-cycles/list`

Parámetro de consulta: `taskId` — ID de la tarea

Retorna la lista de todos los ciclos de ejecución de la tarea especificada.

### Página de detalles de un ciclo de ejecución individual

**GET** `/task-cycle/{cycleIndex}`

Retorna la página de detalles de una ejecución de tarea individual.

### Obtener mensajes del ciclo

**GET** `/api/task-cycle/messages`

Parámetro de consulta: `cycleIndex` — Índice del ciclo

Retorna los mensajes relacionados con el ciclo de ejecución de la tarea especificada.

---

## Sistema de Permisos

### Página de gestión de permisos

**GET** `/permissions`

Retorna la página de la interfaz de gestión de permisos.

### Obtener lista de reglas de permisos

**GET** `/api/permissions/list`

Retorna todas las reglas de permisos configuradas actualmente.

**Ejemplo de respuesta**:
```json
{
  "rules": [
    {
      "permissionType": "NetworkAccess",
      "resourcePrefix": "api.github.com",
      "result": "Allowed",
      "description": "Allow GitHub API access"
    }
  ]
}
```

### Guardar regla de permiso

**POST** `/api/permissions/save`

**Cuerpo de la solicitud**:
```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects",
  "result": "Allowed",
  "description": "Allow project directory access"
}
```

### Página de solicitud de permiso

**GET** `/permission/request`

Muestra la página de solicitud de permiso, permitiendo al usuario aprobar o denegar las solicitudes de permisos de los Seres de Silicio.

**Parámetros de consulta**:

| Parámetro | Tipo | Descripción |
|------|------|------|
| `userId` | `Guid` | ID del Ser de Silicio que solicita el permiso |
| `type` | `string` | Tipo de permiso |
| `resource` | `string` | Ruta del recurso solicitado |
| `allowCode` | `string` | Código de identificación para permitir la operación |
| `denyCode` | `string` | Código de identificación para denegar la operación |

### Verificar solicitudes de permiso pendientes

**GET** `/permission/check`

Parámetro de consulta: `userId` — ID del Ser de Silicio

**Respuesta**:
```json
{
  "pending": true
}
```

### Responder a solicitud de permiso

**GET** `/permission/respond`

**Parámetros de consulta**:

| Parámetro | Tipo | Descripción |
|------|------|------|
| `userId` | `Guid` | ID del Ser de Silicio |
| `allowed` | `bool` | Si se permite |
| `addToCache` | `bool` | Si se almacena en caché la decisión |
| `cacheDuration` | `double` | Duración de la caché (horas) |

**Respuesta**:
```json
{
  "success": true
}
```

---

## Sistema de Registros

### Página de registros

**GET** `/logs`

Retorna la página de la interfaz de visualización de registros.

### Obtener lista de registros

**GET** `/api/logs/list`

Los parámetros de consulta soportan filtrado por nivel y rango de tiempo.

**Ejemplo de respuesta**:
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

### Obtener registros agrupados por ser

**GET** `/api/logs/beings`

Estadísticas de registros agrupadas por Ser de Silicio.

### Obtener niveles de registro disponibles

**GET** `/api/logs/levels`

Retorna la lista de niveles de registro disponibles en el sistema.

---

## Estadísticas de Uso

### Página de estadísticas de uso

**GET** `/usage`

Retorna la página de la interfaz de estadísticas de uso.

### Obtener resumen de uso

**GET** `/api/usage/summary`

Retorna el resumen de uso de tokens y costos.

### Obtener datos de tendencias

**GET** `/api/usage/trend`

Parámetros de consulta: `startDate`, `endDate`

Retorna los datos de tendencias de uso para el período especificado.

### Exportar datos de uso

**GET** `/api/usage/export`

Exporta los datos de uso en un formato descargable.

---

## Pista de Auditoría

### Página de auditoría

**GET** `/audit`

Retorna la página de la interfaz de pista de auditoría.

### Obtener lista de auditoría

**GET** `/api/audit/list`

Retorna la lista de entradas del registro de auditoría.

### Obtener resumen de auditoría

**GET** `/api/audit/summary`

Retorna las estadísticas resumidas de los datos de auditoría.

### Obtener auditoría agrupada por ser

**GET** `/api/audit/beings`

Estadísticas de auditoría agrupadas por Ser de Silicio.

---

## Gestión de Configuración

### Página de configuración

**GET** `/config`

Retorna la página de la interfaz de configuración del sistema.

### Guardar configuración

**POST** `/config/save`

**Cuerpo de la solicitud**:
```json
{
  "language": "ZhCN",
  "port": 8080,
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:7b"
    },
    "DashScope": {
      "apiKey": "...",
      "region": "beijing",
      "model": "qwen3.6-plus"
    },
    "VolcengineArk": {
      "apiKey": "...",
      "endpoint": "...",
      "model": "..."
    },
    "Herdsman": {
      "endpoint": "http://localhost:8000",
      "model": "..."
    },
    "LongCat": {
      "apiKey": "...",
      "endpoint": "...",
      "model": "..."
    },
    "QiniuAI": {
      "apiKey": "...",
      "endpoint": "...",
      "model": "..."
    },
    "DeepSeek": {
      "apiKey": "...",
      "endpoint": "https://api.deepseek.com",
      "model": "deepseek-v4-flash",
      "thinkingEnabled": true,
      "reasoningEffort": "high"
    },
    "Zhipu": {
      "apiKey": "...",
      "endpoint": "https://open.bigmodel.cn/api/paas/v4",
      "model": "glm-4-flash",
      "thinkingEnabled": false
    },
    "Ernie": {
      "apiKey": "...",
      "endpoint": "https://qianfan.baidubce.com/v2",
      "model": "ernie-5.1"
    },
    "Hunyuan": {
      "apiKey": "...",
      "endpoint": "https://tokenhub.tencentmaas.com/v1",
      "model": "hy3",
      "thinkingEnabled": false
    },
    "MiniMax": {
      "apiKey": "...",
      "endpoint": "domestic",
      "model": "MiniMax-M3"
    },
    "Moonshot": {
      "apiKey": "...",
      "endpoint": "https://api.moonshot.cn/v1",
      "model": "kimi-k2.6"
    },
    "SiliconFlow": {
      "apiKey": "...",
      "endpoint": "https://api.siliconflow.cn/v1",
      "model": "deepseek-ai/DeepSeek-V3.2"
    }
  }
}
```

### Obtener opciones de configuración de IA

**GET** `/config/aioptions`

Retorna los tipos de clientes de IA disponibles y sus opciones dinámicas (modelos disponibles, regiones, etc.).

---

## Sistema de Memoria

### Página de memoria

**GET** `/memory`

Retorna la página de la interfaz de gestión de memoria.

### Obtener lista de memorias

**GET** `/api/memory/list`

Retorna la lista de entradas de memoria de los Seres de Silicio.

### Obtener detalles de memoria

**GET** `/api/memory/detail/{id}`

Parámetro de ruta: `id` — ID de la entrada de memoria

Retorna el contenido completo de la entrada de memoria especificada.

### Obtener estadísticas de memoria

**GET** `/api/memory/stats`

Retorna la información estadística del sistema de memoria.

### Buscar memoria

**GET** `/api/memory/search`

Parámetro de consulta: `keyword` — Palabra clave de búsqueda

Busca entradas de memoria que coincidan.

### Obtener memoria agrupada por ser

**GET** `/api/memory/beings`

Estadísticas de memoria agrupadas por Ser de Silicio.

### Obtener trazabilidad de memoria

**GET** `/api/memory/trace/{id}`

Parámetro de ruta: `id` — ID de la entrada de memoria

Retorna la cadena de trazabilidad de origen de la entrada de memoria especificada.

### Obtener HTML de la línea temporal de memoria

**GET** `/api/memory/timeline-html`

Retorna la vista HTML de la línea temporal de memoria.

---

## Notas de Trabajo

### Página de notas de trabajo

**GET** `/work-notes`

Retorna la página de la interfaz de notas de trabajo.

### Obtener lista de notas de trabajo

**GET** `/api/work-notes/list`

Retorna la lista de notas de trabajo.

### Leer nota de trabajo

**GET** `/api/work-notes/read`

Parámetro de consulta: `noteId` — ID de la nota

Retorna el contenido de la nota especificada.

### Obtener directorio de notas

**GET** `/api/work-notes/directory`

Retorna la estructura del directorio de notas.

### Buscar notas de trabajo

**GET** `/api/work-notes/search`

Parámetro de consulta: `keyword` — Palabra clave de búsqueda

Busca notas de trabajo que coincidan.

### Crear nota de trabajo

**POST** `/api/work-notes/create`

**Cuerpo de la solicitud**:
```json
{
  "title": "Título de la nota",
  "content": "Contenido de la nota",
  "keywords": ["palabra clave 1", "palabra clave 2"]
}
```

### Actualizar nota de trabajo

**POST** `/api/work-notes/update`

**Cuerpo de la solicitud**:
```json
{
  "noteId": "note-uuid",
  "title": "Título actualizado",
  "content": "Contenido actualizado"
}
```

### Eliminar nota de trabajo

**POST** `/api/work-notes/delete`

**Cuerpo de la solicitud**:
```json
{
  "noteId": "note-uuid"
}
```

---

## Red de Conocimiento

### Página de la red de conocimiento

**GET** `/knowledge`

Retorna la página de la interfaz de gestión de la red de conocimiento.

### Obtener grafo de conocimiento

**GET** `/api/knowledge/graph`

Retorna los datos del grafo de tripletas de conocimiento (sujeto-relación-objeto).

---

## Gestión de Proyectos

### Página de proyectos

**GET** `/project`

Retorna la página de la interfaz de gestión de proyectos.

### Página de notas de trabajo del proyecto

**GET** `/project/{id}/work-notes`

Parámetro de ruta: `id` — ID del proyecto

Retorna la página de notas de trabajo del proyecto especificado.

### Página de tareas del proyecto

**GET** `/project/{id}/tasks`

Parámetro de ruta: `id` — ID del proyecto

Retorna la página de gestión de tareas del proyecto especificado.

### Página de permisos de herramientas del proyecto

**GET** `/project/{id}/tool-permissions`

Parámetro de ruta: `id` — ID del proyecto

Retorna la página de gestión de permisos de herramientas del proyecto especificado.

### Página de flujo de trabajo del proyecto

**GET** `/project/{id}/workflow`

Parámetro de ruta: `id` — ID del proyecto

Retorna la página de gestión de flujo de trabajo del proyecto especificado.

### Obtener detalles del flujo de trabajo del proyecto

**GET** `/api/projects/workflow-detail`

Parámetro de consulta: `projectId` — ID del proyecto

Retorna los detalles del flujo de trabajo asociado al proyecto.

### Asignar rol de proyecto

**POST** `/api/projects/assign-role`

**Cuerpo de la solicitud**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### Eliminar rol de proyecto

**POST** `/api/projects/remove-role`

**Cuerpo de la solicitud**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### Obtener lista de proyectos

**GET** `/api/projects/list`

Retorna la lista de todos los proyectos.

### Obtener lista de plantillas de flujo de trabajo del proyecto

**GET** `/api/projects/list-workflow-templates`

Retorna la lista de plantillas de flujo de trabajo disponibles.

### Crear proyecto

**POST** `/api/projects/create`

**Cuerpo de la solicitud**:
```json
{
  "name": "Mi Proyecto",
  "description": "Descripción del proyecto"
}
```

### Archivar proyecto

**POST** `/api/projects/{id}/archive`

Parámetro de ruta: `id` — ID del proyecto

Archiva el proyecto especificado.

### Restaurar proyecto

**POST** `/api/projects/{id}/restore`

Parámetro de ruta: `id` — ID del proyecto

Restaura un proyecto archivado.

### Destruir proyecto

**POST** `/api/projects/{id}/destroy`

Parámetro de ruta: `id` — ID del proyecto

Elimina permanentemente el proyecto especificado (irrecuperable).

### Obtener detalles del proyecto

**GET** `/api/projects/detail`

Parámetro de consulta: `projectId` — ID del proyecto

Retorna la información detallada del proyecto.

### Actualizar proyecto

**POST** `/api/projects/update`

**Cuerpo de la solicitud**:
```json
{
  "projectId": "project-uuid",
  "name": "Nombre actualizado",
  "description": "Descripción actualizada"
}
```

### Asignar miembro al proyecto

**POST** `/api/projects/assign`

**Cuerpo de la solicitud**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Eliminar miembro del proyecto

**POST** `/api/projects/remove`

**Cuerpo de la solicitud**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Obtener lista de notas de trabajo del proyecto

**GET** `/api/projects/{id}/work-notes/list`

Parámetro de ruta: `id` — ID del proyecto

Retorna la lista de notas de trabajo del proyecto especificado.

### Leer nota de trabajo del proyecto

**GET** `/api/projects/{id}/work-notes/read`

Parámetro de ruta: `id` — ID del proyecto

Retorna el contenido de las notas de trabajo del proyecto especificado.

### Crear nota de trabajo del proyecto

**POST** `/api/projects/{id}/work-notes/create`

Parámetro de ruta: `id` — ID del proyecto

Crea una nueva nota de trabajo en el proyecto especificado.

### Actualizar nota de trabajo del proyecto

**POST** `/api/projects/{id}/work-notes/update`

Parámetro de ruta: `id` — ID del proyecto

Actualiza una nota de trabajo en el proyecto especificado.

### Eliminar nota de trabajo del proyecto

**POST** `/api/projects/{id}/work-notes/delete`

Parámetro de ruta: `id` — ID del proyecto

Elimina una nota de trabajo en el proyecto especificado.

### Obtener lista de tareas del proyecto

**GET** `/api/projects/{id}/tasks/list`

Parámetro de ruta: `id` — ID del proyecto

Retorna la lista de tareas del proyecto especificado.

### Crear tarea del proyecto

**POST** `/api/projects/{id}/tasks/create`

Parámetro de ruta: `id` — ID del proyecto

Crea una nueva tarea en el proyecto especificado.

### Actualizar tarea del proyecto

**POST** `/api/projects/{id}/tasks/update`

Parámetro de ruta: `id` — ID del proyecto

Actualiza una tarea en el proyecto especificado.

### Eliminar tarea del proyecto

**POST** `/api/projects/{id}/tasks/delete`

Parámetro de ruta: `id` — ID del proyecto

Elimina una tarea en el proyecto especificado.

### Asignar responsable a la tarea

**POST** `/api/projects/{id}/tasks/assign`

Parámetro de ruta: `id` — ID del proyecto

Asigna un responsable a la tarea del proyecto.

### Eliminar responsable de la tarea

**POST** `/api/projects/{id}/tasks/remove-assignee`

Parámetro de ruta: `id` — ID del proyecto

Elimina el responsable de la tarea del proyecto.

### Marcar tarea como completada

**POST** `/api/projects/{id}/tasks/complete`

Parámetro de ruta: `id` — ID del proyecto

Marca la tarea del proyecto como completada.

### Marcar tarea como fallida

**POST** `/api/projects/{id}/tasks/fail`

Parámetro de ruta: `id` — ID del proyecto

Marca la tarea del proyecto como fallida.

### Cancelar tarea

**POST** `/api/projects/{id}/tasks/cancel`

Parámetro de ruta: `id` — ID del proyecto

Cancela la tarea del proyecto.

---

## Gestión de Permisos de Herramientas

### Obtener permisos de herramientas del Ser de Silicio

**GET** `/api/beings/tool-permissions`

Parámetro de consulta: `beingId` — ID del Ser de Silicio

Retorna la configuración de permisos de herramientas del Ser de Silicio especificado.

### Actualizar permisos de herramientas del Ser de Silicio

**PUT** `/api/beings/tool-permissions`

**Cuerpo de la solicitud**:
```json
{
  "beingId": "being-uuid",
  "permissions": {
    "network": "allowed",
    "disk_read": "allowed",
    "disk_write": "denied"
  }
}
```

### Obtener plantillas de permisos de herramientas

**GET** `/api/beings/tool-permissions/templates`

Retorna la lista de plantillas de permisos de herramientas disponibles.

### Aplicar plantilla de permisos de herramientas

**POST** `/api/beings/tool-permissions/apply-template`

**Cuerpo de la solicitud**:
```json
{
  "beingId": "being-uuid",
  "templateName": "readonly"
}
```

### Obtener permisos de herramientas del proyecto

**GET** `/api/projects/{id}/tool-permissions`

Parámetro de ruta: `id` — ID del proyecto

Retorna la configuración de permisos de herramientas del proyecto especificado.

### Actualizar permisos de herramientas del proyecto

**PUT** `/api/projects/{id}/tool-permissions`

Parámetro de ruta: `id` — ID del proyecto

**Cuerpo de la solicitud**:
```json
{
  "permissions": {
    "network": "allowed",
    "disk_read": "allowed",
    "disk_write": "denied"
  }
}
```

---

## Gestión de Ejecutores

### Página de ejecutores

**GET** `/executor`

Retorna la página de la interfaz de gestión de ejecutores.

### Obtener estado de los ejecutores

**GET** `/api/executors/status`

Retorna el estado de ejecución de cada ejecutor (disco, red, línea de comandos).

---

## Navegador de Código

### Página del navegador de código

**GET** `/code`

Retorna la página de la interfaz del navegador de código.

### Obtener lista de tipos de código

**GET** `/api/code/types`

Retorna la lista de tipos/lenguajes de código soportados.

### Obtener detalles del código

**GET** `/api/code/detail`

Parámetros de consulta: `filePath`, `lineNumber`

Retorna los detalles del código del archivo especificado.

---

## Sugerencias Flotantes de Código

### Obtener sugerencia flotante

**GET** `/api/code/hover`
**POST** `/api/code/hover`

Obtiene la información de sugerencia flotante para una posición de código (similar a IntelliSense de IDE).

### Registrar posición de código

**POST** `/api/code/register`

Registra una posición de código que necesita monitoreo.

### Actualizar posición de código

**POST** `/api/code/update`

Actualiza la información de una posición de código registrada.

### Eliminar posición de código

**POST** `/api/code/unregister`

Elimina el monitoreo de una posición de código que ya no se necesita.

---

## Sistema de Documentación de Ayuda

### Página de ayuda

**GET** `/help` o **GET** `/help/index`

Retorna la página principal de la documentación de ayuda.

### Página de tema de ayuda

**GET** `/help/{topic}`

Parámetro de ruta: `topic` — Identificador del tema

Retorna la página de documentación de ayuda del tema especificado.

### Buscar documentación de ayuda

**GET** `/api/help/search`

Parámetro de consulta: `keyword` — Palabra clave de búsqueda

Busca temas de documentación de ayuda que coincidan.

---

## Inicialización

### Página del asistente de inicialización

**GET** `/init`

Retorna la página del asistente de inicialización para la primera ejecución.

### Enviar inicialización

**POST** `/init`

Envía la configuración de inicialización para la primera ejecución.

### Explorar para seleccionar directorio de datos

**GET** `/init/browse`

Abre el explorador de directorios para seleccionar la ubicación de almacenamiento de datos.

### Obtener metadatos de configuración de IA

**GET** `/init/ai-config-metadata`

Retorna los tipos de clientes de IA disponibles y los metadatos de los campos de configuración.

---

## Control del Sistema

### Apagado elegante

**POST** `/api/system/shutdown`

> **Nota**: Solo se permiten solicitudes desde localhost

Inicia el proceso de apagado elegante de la aplicación:

1. Detener el bucle principal (MainLoop)
2. Guardar la configuración actual
3. Cerrar el escuchador HTTP

**Respuesta**:
```json
{
  "status": "shutting_down",
  "message": "Application is shutting down gracefully"
}
```

---

## Acerca de

### Página de acerca de

**GET** `/about`

Retorna la página de acerca de, incluyendo información del sistema y la lista de plugins cargados.

**Datos de la lista de plugins**:
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

---

## Respuestas de Error

Todos los endpoints retornan respuestas de error estandarizadas:

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: FileAccess, Denied by GlobalACL"
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

---

## Eventos SSE

Los eventos enviados por el servidor se usan para actualizaciones en tiempo real:

### Eventos de Chat

```javascript
const eventSource = new EventSource('/api/chat/stream');

eventSource.onmessage = (event) => {
  const data = JSON.parse(event.data);
  
  switch(data.type) {
    case 'chunk':
      console.log('Streaming:', data.content);
      break;
    case 'tool_call':
      console.log('Tool executing:', data.tool);
      break;
    case 'complete':
      console.log('Chat complete, session:', data.sessionId);
      break;
    case 'error':
      console.error('Error:', data.message);
      break;
  }
};
```

---

## Interfaz del Cliente de IA

### Interfaz IAIClient

```csharp
public interface IAIClient
{
    string Name { get; }
    
    Task<AIResponse> ChatAsync(AIRequest request);
    
    IAsyncEnumerable<string> StreamChatAsync(AIRequest request);
}
```

### Estructura AIRequest

```csharp
public class AIRequest
{
    public List<Message> Messages { get; set; }
    public List<ToolDefinition> Tools { get; set; }
    public double Temperature { get; set; } = 0.7;
    public int MaxTokens { get; set; } = 2000;
    public string Model { get; set; }
}
```

### Estructura AIResponse

```csharp
public class AIResponse
{
    public string Content { get; set; }
    public List<ToolCall> ToolCalls { get; set; }
    public TokenUsage Usage { get; set; }
    public string Model { get; set; }
}
```

---

## Interfaz del Sistema de Herramientas

### Interfaz ITool

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    ToolDefinition Definition { get; }
    
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### Estructura ToolCall

```csharp
public class ToolCall
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}
```

### Estructura ToolResult

```csharp
public class ToolResult
{
    public bool Success { get; set; }
    public string Output { get; set; }
    public string Error { get; set; }
}
```

---

## Siguientes Pasos

- 🚀 Ver la [guía de inicio rápido](getting-started.md)
- 🛠️ Leer la [guía de desarrollo](development-guide.md)
- 📚 Ver la [documentación de arquitectura](architecture.md)
- 🔒 Conocer el [modelo de seguridad](security.md)
