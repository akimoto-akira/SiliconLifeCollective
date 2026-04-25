# Guía de Interfaz Web

[English](web-ui-guide.md) | [简体中文](docs/zh-CN/web-ui-guide.md) | [繁體中文](docs/zh-HK/web-ui-guide.md) | [Español](docs/es-ES/web-ui-guide.md) | [日本語](docs/ja-JP/web-ui-guide.md) | [한국어](docs/ko-KR/web-ui-guide.md) | [Čeština](docs/cs-CZ/web-ui-guide.md)

## Resumen

La interfaz web de Silicon Life Collective proporciona una interfaz completa para gestionar y interactuar con Seres de Silicio.

---

## Arquitectura

```
WebHost (HttpListener)
  ↓
Router (coincidencia de patrones)
  ↓
Controladores (18)
  ↓
Vistas (constructores HTML)
  ↓
Respuesta HTTP
```

---

## Acceso

- **URL**: `http://localhost:8080`
- **Puerto**: Configurable en `config.json`
- **Protocolo**: HTTP + SSE (Server-Sent Events)

---

## Sistema de Pieles

### Pieles Disponibles

1. **Admin**: Interfaz profesional de gestión
2. **Chat**: Interfaz conversacional
3. **Creative**: Interfaz artística
4. **Dev**: Interfaz para desarrolladores

### Cambiar Piel

1. Ir a Configuración
2. Seleccionar piel
3. Guardar cambios

---

## Controladores

### About (`/about`)

Información sobre el sistema:
- Versión
- Autor
- Licencia
- Estadísticas

### Audit (`/audit`)

Registros de auditoría:
- Decisiones de permisos
- Uso de tokens
- Ejecución de herramientas

### Being (`/beings`)

Gestión de Seres de Silicio:
- Lista de seres
- Crear/eliminar seres
- Iniciar/detener
- Editar archivo de alma

### Chat (`/chat`)

Interfaz de chat:
- Seleccionar ser
- Enviar mensajes
- Ver historial
- Actualizaciones en tiempo real (SSE)

### CodeBrowser (`/code`)

Navegador de código:
- Explorar estructura
- Ver archivos
- Buscar código

### CodeHover (`/api/code/hover`)

Información de código al pasar el cursor:
- Tooltips de código
- Documentación inline

### Config (`/config`)

Configuración del sistema:
- Editar configuración
- Backend de IA
- Localización
- Piel

### Dashboard (`/`)

Panel principal:
- Estados de seres
- Métricas de rendimiento
- Uso de recursos
- Actividad reciente

### Executor (`/executors`)

Estado de ejecutores:
- DiskExecutor
- NetworkExecutor
- CommandLineExecutor
- DynamicCompilationExecutor

### Init (`/init`)

Inicialización del sistema:
- Primer arranque
- Configuración inicial

### Knowledge (`/knowledge`)

Base de conocimiento:
- Documentos
- Referencias
- Guías

### Log (`/logs`)

Registros del sistema:
- Logs en tiempo real
- Filtrar por nivel
- Buscar

### Memory (`/memory`)

Gestión de memoria:
- Ver memoria
- Buscar
- Limpiar

### Permission (`/permissions`)

Gestión de permisos:
- Ver permisos
- Configurar ACL
- Cachés

### PermissionRequest (`/permission-requests`)

Solicitudes de permiso pendientes:
- Ver solicitudes
- Aprobar/rechazar
- Recordar decisión

### Project (`/project`)

Información del proyecto:
- Estructura
- Documentación
- Recursos

### Task (`/tasks`)

Gestión de tareas:
- Lista de tareas
- Crear/actualizar
- Completar

### Timer (`/timers`)

Gestión de temporizadores:
- Temporizadores activos
- Crear/cancelar
- Historial

---

## Actualizaciones en Tiempo Real

### SSE (Server-Sent Events)

La interfaz usa SSE para actualizaciones en tiempo real:

```javascript
const eventSource = new EventSource('/sse');

eventSource.onmessage = (event) => {
    const data = JSON.parse(event.data);
    // Actualizar UI
};
```

### Eventos

- `chat_message`: Nuevo mensaje de chat
- `being_status`: Cambio de estado de ser
- `permission_request`: Nueva solicitud de permiso
- `log_entry`: Nueva entrada de log

---

## Constructores HTML

### H (HTML Builder)

DSL fluente para construir HTML:

```csharp
H.Html(
    H.Head(
        H.Title("Mi Página"),
        H.Style("body { margin: 0; }")
    ),
    H.Body(
        H.Div(
            H.Class("container"),
            H.H1("Título"),
            H.P("Párrafo")
        )
    )
)
```

### CssBuilder

Constructor de CSS:

```csharp
var css = new CssBuilder()
    .Add("body", new { margin = "0", padding = "0" })
    .Add(".container", new { "max-width" = "1200px" })
    .Build();
```

### JsBuilder

Constructor de JavaScript:

```csharp
var js = new JsBuilder()
    .Function("init", new string[0], @"
        console.log('Inicializado');
    ")
    .Build();
```

---

## Personalización

### Agregar Nueva Página

1. Crear controlador
2. Crear vista
3. Registrar ruta

### Modificar Piel

1. Editar archivo de piel
2. Modificar CSS
3. Actualizar constructores

---

## API REST

### Obtener Seres

```bash
GET /api/beings
```

### Crear Ser

```bash
POST /api/beings
Content-Type: application/json

{
  "name": "Nuevo Ser",
  "soul": "# Personalidad..."
}
```

### Enviar Mensaje

```bash
POST /api/chat/{channelId}/message
Content-Type: application/json

{
  "content": "Hola"
}
```

---

## Vista de Historial de Chat

### Funcionalidades

- Navegación del historial de chat de Seres de Silicio
- Visualización de lista de conversaciones
- Vista de detalles de mensajes
- Vista de línea de tiempo

### Uso del Historial de Chat

1. Navegue a la página **Seres**
2. Haga clic en el enlace **Historial de Chat** de un Ser de Silicio
3. Vea la lista de conversaciones:
   - Título de conversación
   - Hora de creación
   - Cantidad de mensajes
4. Haga clic en una conversación para ver detalles:
   - Historial completo de mensajes
   - Marcas de tiempo
   - Información del remitente
   - Registros de llamadas a herramientas

### Implementación Técnica

- **Controlador**: `ChatHistoryController`
- **ViewModel**: `ChatHistoryViewModel`
- **Vistas**:
  - `ChatHistoryListView` - Lista de conversaciones
  - `ChatHistoryDetailView` - Detalles de mensajes
- **Rutas API**:
  - `/api/chat-history/{beingId}/conversations` - Obtener lista de conversaciones
  - `/api/chat-history/{beingId}/conversation/{conversationId}` - Obtener detalles de mensajes

---

## Carga de Archivos

### Funcionalidades

- Diálogo de origen de archivos
- Soporte de carga de múltiples archivos
- Gestión de metadatos de archivos
- Visualización de progreso de carga

### Uso de Carga de Archivos

1. Haga clic en el botón **Cargar Archivo** en la interfaz de chat
2. Se abre el diálogo de origen de archivos
3. Seleccione origen de archivos:
   - Archivos locales
   - Ruta del sistema de archivos
4. Seleccione archivos (selección múltiple soportada)
5. Confirme la carga
6. La información del archivo se adjuntará al mensaje

### Tipos de Archivos Soportados

- Archivos de texto (.txt, .md, .json, .xml, etc.)
- Archivos de código (.cs, .js, .py, .java, etc.)
- Archivos de configuración (.yml, .yaml, .ini, .conf, etc.)
- Archivos de documento (.csv, .log, etc.)

---

## Indicadores de Carga

### Funcionalidades

- Visualización de estado de carga para páginas de chat
- Selección automática de sesión del curador
- Retroalimentación de progreso de carga de datos

### Comportamiento

- Animación de carga mostrada cuando la página se carga
- Ocultado automáticamente después de que la carga de datos se completa
- Sesión del curador seleccionada automáticamente (si existe)
- Texto de aviso de carga multiidioma

---

## Seguridad

### Autenticación

- Sesiones basadas en cookies
- CSRF protection
- Input validation

### Autorización

- Permisos por operación
- Role-based access
- Audit logging

---

## Recursos Adicionales

- [Arquitectura](architecture.md)
- [Referencia de API](api-reference.md)
- [Guía de Desarrollo](development-guide.md)
