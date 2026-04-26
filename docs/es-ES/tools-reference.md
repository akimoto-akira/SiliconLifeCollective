# Referencia de Herramientas

Este documento detalla todas las herramientas integradas de la plataforma Silicon Life Collective.

[English](../en/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | **Español** | [Deutsch](../de-DE/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md)

## Resumen

El sistema de herramientas permite a los Seres Silicona interactuar con el mundo exterior a través de una interfaz estandarizada. Cada herramienta implementa la interfaz `ITool` y es descubierta y registrada automáticamente por `ToolManager` a través de reflexión.

### Categorías de Herramientas

- **Herramientas de administración del sistema** — Configuración, permisos, compilación dinámica
- **Herramientas de comunicación** — Chat, solicitudes de red
- **Herramientas de almacenamiento de datos** — Operaciones de disco, base de datos, memoria, notas de trabajo
- **Herramientas de gestión del tiempo** — Calendario, temporizadores, tareas
- **Herramientas de desarrollo** — Ejecución de código, consulta de registros
- **Herramientas de utilidad** — Información del sistema, auditoría de tokens, documentos de ayuda, red de conocimiento
- **Herramientas de navegador** — Automatización de navegador WebView

---

## Lista de Herramientas Integradas

### 1. Herramienta de Calendario (CalendarTool)

**Nombre de herramienta**: `calendar`

**Descripción**: Soporte para conversión y cálculo de fechas en 32 sistemas de calendario.

**Operaciones soportadas**:
- `now` — Obtener tiempo actual
- `format` — Formatear fecha
- `add_days` — Sumar/restar días a fecha
- `diff` — Calcular diferencia entre fechas
- `list_calendars` — Listar todos los calendarios soportados
- `get_components` — Obtener componentes de fecha
- `get_now_components` — Obtener componentes de tiempo actual
- `convert` — Convertir entre sistemas de calendario

**Sistemas de calendario soportados** (32):
- Gregoriano (Gregorian)
- Lunar Chino (Chinese Lunar)
- Histórico Chino (Chinese Historical) — ciclo Ganzhi, años de reinado imperial
- Islámico (Islamic)
- Hebreo (Hebrew)
- Japonés (Japanese)
- Persa (Persian)
- Maya (Mayan)
- Budista (Buddhist)
- Tibetano (Tibetan)
- Y 24 calendarios más...

**Ejemplo de uso**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_lunar"
}
```

---

### 2. Herramienta de Chat (ChatTool)

**Nombre de herramienta**: `chat`

**Descripción**: Gestionar sesiones de chat y envío de mensajes.

**Operaciones soportadas**:
- `send_message` — Enviar mensaje
- `get_messages` — Obtener mensajes históricos
- `create_group` — Crear chat grupal
- `add_member` — Añadir miembro al grupo
- `remove_member` — Remover miembro del grupo
- `get_chat_info` — Obtener información de chat
- `terminate_chat` — Terminar chat (leer sin responder)

**Ejemplo de uso**:
```json
{
  "action": "send_message",
  "target_id": "being-uuid-or-user-0",
  "message": "你好，让我们协作吧！"
}
```

---

### 3. Herramienta de Configuración (ConfigTool)

**Nombre de herramienta**: `config`

**Descripción**: Leer y modificar configuración del sistema.

**Operaciones soportadas**:
- `read` — Leer elemento de configuración
- `write` — Escribir elemento de configuración
- `list` — Listar todos los elementos de configuración
- `reset` — Restablecer configuración a valores predeterminados

---

### 4. Herramienta de Disco (DiskTool)

**Nombre de herramienta**: `disk`

**Descripción**: Operaciones de archivo y directorio.

**Operaciones soportadas**:
- `read` — Leer archivo
- `write` — Escribir archivo
- `list` — Listar directorio
- `delete` — Eliminar archivo/directorio
- `exists` — Verificar existencia
- `create_directory` — Crear directorio

---

### 5. Herramienta de Red (NetworkTool)

**Nombre de herramienta**: `network`

**Descripción**: Solicitudes HTTP y operaciones de red.

**Operaciones soportadas**:
- `get` — Solicitud HTTP GET
- `post` — Solicitud HTTP POST
- `download` — Descargar archivo
- `upload` — Subir archivo

---

### 6. Herramienta de Memoria (MemoryTool)

**Nombre de herramienta**: `memory`

**Descripción**: Gestionar memoria a largo plazo del ser.

**Operaciones soportadas**:
- `save` — Guardar memoria
- `load` — Cargar memoria
- `search` — Buscar memoria
- `delete` — Eliminar memoria
- `list` — Listar memorias

---

### 7. Herramienta de Tareas (TaskTool)

**Nombre de herramienta**: `task`

**Descripción**: Gestionar sistema de tareas.

**Operaciones soportadas**:
- `create` — Crear tarea
- `update` — Actualizar tarea
- `delete` — Eliminar tarea
- `list` — Listar tareas
- `complete` — Marcar tarea como completada

---

### 8. Herramienta de Temporizador (TimerTool)

**Nombre de herramienta**: `timer`

**Descripción**: Gestionar sistema de temporizadores.

**Operaciones soportadas**:
- `create` — Crear temporizador
- `start` — Iniciar temporizador
- `pause` — Pausar temporizador
- `resume` — Reanudar temporizador
- `cancel` — Cancelar temporizador
- `list` — Listar temporizadores

---

### 9. Herramienta de Notas de Trabajo (WorkNoteTool)

**Nombre de herramienta**: `worknote`

**Descripción**: Gestionar sistema de notas de trabajo.

**Operaciones soportadas**:
- `create` — Crear nota
- `read` — Leer nota
- `update` — Actualizar nota
- `delete` — Eliminar nota
- `search` — Buscar notas
- `index` — Generar índice de notas

---

### 10. Herramienta de Conocimiento (KnowledgeTool)

**Nombre de herramienta**: `knowledge`

**Descripción**: Gestionar red de conocimiento basada en tripletas.

**Operaciones soportadas**:
- `add` — Añadir tripleta de conocimiento
- `query` — Consultar conocimiento
- `update` — Actualizar conocimiento
- `delete` — Eliminar conocimiento
- `search` — Buscar conocimiento
- `get_path` — Obtener ruta entre conceptos
- `validate` — Validar conocimiento
- `stats` — Estadísticas de red de conocimiento

---

### 11. Herramienta de Auditoría de Tokens (TokenAuditTool)

**Nombre de herramienta**: `token_audit`

**Descripción**: Consultar uso de tokens de IA. **[Solo Curador]**

**Operaciones soportadas**:
- `query` — Consultar uso de tokens
- `summary` — Obtener resumen de uso
- `export` — Exportar datos de auditoría

---

### 12. Herramienta de Permiso (PermissionTool)

**Nombre de herramienta**: `permission`

**Descripción**: Gestionar sistema de permisos. **[Solo Curador]**

**Operaciones soportadas**:
- `evaluate` — Evaluar permiso (sin activar prompt)
- `add_rule` — Añadir regla de permiso
- `remove_rule` — Eliminar regla de permiso
- `list_rules` — Listar reglas de permiso

---

### 13. Herramienta de Compilación Dinámica (DynamicCompilationTool)

**Nombre de herramienta**: `dynamic_compilation`

**Descripción**: Compilar y reemplazar código del ser en tiempo de ejecución. **[Solo Curador]**

**Operaciones soportadas**:
- `compile` — Compilar código
- `replace` — Reemplazar clase del ser
- `validate` — Validar código

---

### 14. Herramienta de Registro (LogTool)

**Nombre de herramienta**: `log`

**Descripción**: Consultar registros del sistema.

**Operaciones soportadas**:
- `query` — Consultar registros
- `filter` — Filtrar registros por nivel/fecha

---

### 15. Herramienta de Información del Sistema (SystemTool)

**Nombre de herramienta**: `system`

**Descripción**: Obtener información del sistema.

**Operaciones soportadas**:
- `info` — Información del sistema
- `stats` — Estadísticas de rendimiento

---

### 16. Herramienta de Ayuda (HelpTool)

**Nombre de herramienta**: `help`

**Descripción**: Acceder a documentos de ayuda.

**Operaciones soportadas**:
- `search` — Buscar documentos de ayuda
- `list` — Listar temas de ayuda
- `get` — Obtener documento específico

---

### 17. Herramienta de Navegador WebView (WebViewBrowserTool)

**Nombre de herramienta**: `webview_browser`

**Descripción**: Automatización de navegador basada en Playwright.

**Operaciones soportadas**:
- `navigate` — Navegar a URL
- `click` — Hacer clic en elemento
- `input` — Entrada de texto
- `get_content` — Obtener contenido de página
- `execute_js` — Ejecutar JavaScript
- `screenshot` — Tomar captura de pantalla
- `wait_for` — Esperar elemento

---

### 18-23. Otras Herramientas

- **Herramienta de Proyecto (ProjectTool)**: Gestionar proyectos y espacios de trabajo
- **Herramienta de Ejecutor (ExecutorTool)**: Gestionar ejecutores (solo curador)
- **Herramienta de Localización (LocalizationTool)**: Gestionar localización
- **Herramienta de Chat History (ChatHistoryTool)**: Consultar historial de chat
- **Herramienta de Broadcast (BroadcastTool)**: Gestionar canales de broadcast
- **Herramienta de Código (CodeTool)**: Ejecutar y analizar código

---

## Mejores Prácticas

### 1. Verificar Permisos Antes de Ejecutar

Usar `PermissionTool.EvaluatePermission()` para verificar estado de permisos antes de intentar operaciones.

### 2. Manejar Errores Gracefully

Siempre verificar `ToolResult.Success` y manejar fallos apropiadamente.

### 3. Usar Herramientas Apropiadas

Elegir la herramienta correcta para el trabajo:
- Operaciones de archivo → `DiskTool`
- Solicitudes HTTP → `NetworkTool`
- Gestión de tiempo → `CalendarTool`, `TimerTool`

---

## Próximos Pasos

- 📚 Leer la [Guía de Arquitectura](architecture.md)
- 🛠️ Consultar la [Guía de Desarrollo](development-guide.md)
- 🌐 Ver la [Guía de Web UI](web-ui-guide.md)
- 🚀 Comenzar con la [Guía de Inicio Rápido](getting-started.md)
