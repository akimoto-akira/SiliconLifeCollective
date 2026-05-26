# Referencia de Herramientas

> **Versión: v0.2.0-alpha**

Este documento describe en detalle todas las herramientas integradas de la plataforma Silicon Life Collective.

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | **Español** | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md) | [Русский](../ru-RU/tools-reference.md)

## Resumen

El sistema de herramientas permite a los Seres de Silicio interactuar con el mundo exterior a través de una interfaz estandarizada. Cada herramienta implementa la interfaz `ITool`, descubierta y registrada automáticamente por `ToolManager` mediante reflexión.

### Clasificación de Herramientas

- **Herramientas de gestión del sistema** — Configuración, permisos, compilación dinámica, gestión del Curador
- **Herramientas de comunicación** — Chat, solicitudes de red
- **Herramientas de almacenamiento de datos** — Operaciones de disco, base de datos, memoria, notas de trabajo
- **Herramientas de gestión de tiempo** — Calendario, temporizadores, tareas
- **Herramientas de desarrollo** — Ejecución de código, consulta de registros
- **Herramientas de utilidad** — Información del sistema, auditoría de tokens, documentación de ayuda, red de conocimiento
- **Herramientas de navegador** — Automatización del navegador WebView
- **Herramientas de proyecto** — Gestión de proyectos, tareas de proyecto, notas de trabajo de proyecto, trabajo de proyecto
- **Herramientas de plugins** — Herramientas de terceros registradas a través del sistema de plugins

### Sistema de Escenarios de Herramientas

Cada herramienta declara sus escenarios disponibles mediante el atributo `[ToolScenario]`:

| Bandera de escenario | Valor | Descripción |
|----------|------|-------------|
| `Chat` | `1 << 0` | Escenario de chat (cuando el usuario dialoga con el Ser de Silicio) |
| `Task` | `1 << 1` | Escenario de tarea (cuando el Ser de Silicio ejecuta una tarea) |
| `Timer` | `1 << 2` | Escenario de temporizador (cuando el Ser de Silicio ejecuta una tarea programada) |
| `MemoryCompression` | `1 << 3` | Escenario de compresión de memoria |
| `Project` | `1 << 4` | Escenario de proyecto (modo ThinkOnProject) |
| `All` | Todos los anteriores | Disponible en todos los escenarios |

Además, las herramientas marcadas con el atributo `[ChatOnly]` solo están disponibles en el escenario de chat (como HelpTool), y no aparecerán en los escenarios de tareas y temporizadores.

---

## Lista de Herramientas Integradas

### 1. Herramienta de Calendario (CalendarTool)

**Nombre de herramienta**: `calendar`

**Descripción**: Soporta conversión y cálculo de fechas en 32 sistemas de calendario.

**Operaciones soportadas**:
- `now` — Obtener la hora actual
- `format` — Formatear fecha
- `add_days` — Sumar/restar días a una fecha
- `diff` — Calcular diferencia entre fechas
- `list_calendars` — Listar todos los calendarios soportados
- `get_components` — Obtener componentes de fecha
- `get_now_components` — Obtener componentes de la hora actual
- `convert` — Conversión entre sistemas de calendario

**Sistemas de calendario soportados** (32):
- Gregoriano (Gregorian)
- Lunar chino (Chinese Lunar)
- Histórico chino (Chinese Historical) — Ciclos sexagenarios, eras imperiales
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

**Descripción**: Gestión de sesiones de chat y envío de mensajes.

**Operaciones soportadas**:
- `send_message` — Enviar mensaje
- `get_messages` — Obtener mensajes históricos
- `create_group` — Crear chat grupal
- `add_member` — Agregar miembro al grupo
- `remove_member` — Eliminar miembro del grupo
- `get_chat_info` — Obtener información del chat
- `terminate_chat` — Terminar chat (leído sin responder)

**Ejemplo de uso**:
```json
{
  "action": "send_message",
  "target_id": "being-uuid-or-user-0",
  "message": "¡Hola, colaboremos!"
}
```

---

### 3. Herramienta de Configuración (ConfigTool)

**Nombre de herramienta**: `config`

**Descripción**: Leer y modificar la configuración del sistema.

**Operaciones soportadas**:
- `read` — Leer elemento de configuración
- `write` — Escribir elemento de configuración
- `list` — Listar toda la configuración
- `get_ai_config` — Obtener configuración del cliente de IA
- `set_ai_config` — Establecer configuración del cliente de IA

**Ejemplo de uso**:
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

---

### 4. Herramienta del Curador (CuratorTool) 🔒

**Nombre de herramienta**: `silicon_manager`

**Requisito de permisos**: Solo para el Curador de Silicio (`[SiliconManagerOnly]`)

**Escenarios disponibles**: Chat, Task, Timer

**Descripción**: Herramienta de gestión del sistema exclusiva del Curador de Silicio, para gestionar la creación, visualización y reinicio de Seres de Silicio.

**Operaciones soportadas**:
- `list_beings` — Listar todos los Seres de Silicio y su estado
- `create_being` — Crear nuevo Ser de Silicio (requiere parámetros `name` y `soul`)
- `get_code` — Ver el código fuente personalizado del Ser de Silicio
- `reset` — Restablecer el Ser de Silicio a la implementación por defecto

**Ejemplo de uso**:
```json
{
  "action": "create_being",
  "name": "Asistente",
  "soul": "Eres un asistente útil..."
}
```

---

### 5. Herramienta de Base de Datos (DatabaseTool)

**Nombre de herramienta**: `database`

**Descripción**: Consultas y operaciones de base de datos estructurada.

**Operaciones soportadas**:
- `query` — Consultar datos
- `insert` — Insertar datos
- `update` — Actualizar datos
- `delete` — Eliminar datos
- `create_table` — Crear tabla
- `list_tables` — Listar todas las tablas

**Ejemplo de uso**:
```json
{
  "action": "query",
  "table": "users",
  "conditions": {"status": "active"},
  "limit": 100
}
```

---

### 6. Herramienta de Disco (DiskTool)

**Nombre de herramienta**: `disk`

**Descripción**: Operaciones del sistema de archivos y búsqueda local.

**Operaciones soportadas**:
- `read` — Leer archivo
- `write` — Escribir archivo
- `list` — Listar directorio
- `delete` — Eliminar archivo
- `create_directory` — Crear directorio
- `search_files` — Buscar archivos
- `search_content` — Buscar contenido de archivos
- `count_lines` — Contar líneas
- `read_lines` — Leer líneas específicas
- `replace_text` — Reemplazar texto

**Requisito de permisos**: `FileAccess`

**Ejemplo de uso**:
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

---

### 7. Herramienta de Compilación Dinámica (DynamicCompileTool) 🔒

**Nombre de herramienta**: `compile`

**Descripción**: Compilación dinámica de código C# (para la autoevolución de los Seres de Silicio).

**Operaciones soportadas**:
- `compile_class` — Compilar clase
- `compile_callback` — Compilar función de retrollamada de permisos
- `validate_code` — Validar seguridad del código

**Mecanismos de seguridad**:
- Control de referencias en tiempo de compilación (excluye ensamblados peligrosos)
- Escaneo estático de código en tiempo de ejecución
- Almacenamiento cifrado con AES-256

**Ejemplo de uso**:
```json
{
  "action": "compile_class",
  "code": "public class MyBeing : SiliconBeingBase { ... }"
}
```

---

### 8. Herramienta de Ejecución de Código (ExecuteCodeTool) 🔒

**Nombre de herramienta**: `execute_code`

**Requisito de permisos**: Solo para el Curador de Silicio

**Descripción**: Compilar y ejecutar fragmentos de código C#.

**Operaciones soportadas**:
- `run_script` — Ejecutar script de código

**Ejemplo de uso**:
```json
{
  "action": "run_script",
  "code": "return DateTime.Now.ToString();",
  "timeout": 30
}
```

---

### 9. Herramienta de Ayuda (HelpTool)

**Nombre de herramienta**: `help`

**Escenarios disponibles**: Chat (`[ChatOnly]`, solo disponible en escenario de chat)

**Descripción**: Buscar y obtener contenido de la documentación de ayuda del sistema, permitiendo a la IA consultar métodos de uso de las funcionalidades del sistema.

**Operaciones soportadas**:
- `list` — Listar todos los IDs de temas de ayuda
- `search` — Buscar documentación de ayuda por palabra clave
- `get` — Obtener contenido de documentación de ayuda por ID

**Ejemplo de uso**:
```json
{
  "action": "search",
  "keyword": "permisos"
}
```

---

### 10. Herramienta de Red de Conocimiento (KnowledgeTool)

**Nombre de herramienta**: `knowledge`

**Descripción**: Operaciones del grafo de conocimiento (basado en tripletas: sujeto-relación-objeto).

**Operaciones soportadas**:
- `add` — Agregar tripleta de conocimiento
- `query` — Consultar conocimiento
- `update` — Actualizar conocimiento
- `delete` — Eliminar conocimiento
- `search` — Buscar conocimiento
- `get_path` — Obtener ruta de conocimiento
- `validate` — Validar conocimiento
- `stats` — Obtener estadísticas

**Ejemplo de uso**:
```json
{
  "action": "add",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95
}
```

---

### 11. Herramienta de Registros (LogTool)

**Nombre de herramienta**: `log`

**Descripción**: Consultar historial de operaciones y historial de conversaciones.

**Operaciones soportadas**:
- `query_logs` — Consultar registros del sistema
- `query_conversations` — Consultar historial de conversaciones
- `get_stats` — Obtener estadísticas de registros

**Ejemplo de uso**:
```json
{
  "action": "query_logs",
  "being_id": "being-uuid",
  "start_time": "2026-04-20T00:00:00Z",
  "end_time": "2026-04-26T23:59:59Z",
  "level": "info"
}
```

---

### 12. Herramienta de Memoria (MemoryTool)

**Nombre de herramienta**: `memory`

**Descripción**: Gestionar la memoria a corto y largo plazo de los Seres de Silicio.

**Operaciones soportadas**:
- `read` — Leer memoria
- `write` — Escribir memoria
- `search` — Buscar memoria
- `delete` — Eliminar memoria
- `list` — Listar memoria
- `get_stats` — Obtener estadísticas de memoria
- `compress` — Comprimir memoria

**Ejemplo de uso**:
```json
{
  "action": "read",
  "key": "important_fact",
  "time_range": {
    "start": "2026-04-01",
    "end": "2026-04-26"
  }
}
```

---

### 13. Herramienta de Red (NetworkTool)

**Nombre de herramienta**: `network`

**Descripción**: Realizar solicitudes HTTP/HTTPS.

**Operaciones soportadas**:
- `get` — Solicitud GET
- `post` — Solicitud POST
- `put` — Solicitud PUT
- `delete` — Solicitud DELETE
- `download` — Descargar archivo
- `upload` — Cargar archivo

**Requisito de permisos**: `network:http`

**Ejemplo de uso**:
```json
{
  "action": "get",
  "url": "https://api.example.com/data"
}
```

---

### 14. Herramienta de Permisos (PermissionTool) 🔒

**Nombre de herramienta**: `permission`

**Requisito de permisos**: Solo para el Curador de Silicio

**Descripción**: Gestionar permisos y listas de control de acceso.

**Operaciones soportadas**:
- `query_permission` — Consultar permisos
- `manage_acl` — Gestionar ACL global
- `get_callback` — Obtener función de retrollamada de permisos
- `set_callback` — Establecer función de retrollamada de permisos

**Ejemplo de uso**:
```json
{
  "action": "manage_acl",
  "acl_action": "add_rule",
  "permission_type": "filesystem",
  "resource_prefix": "/data/",
  "acl_result": "allow"
}
```

---

### 15. Herramienta de Proyecto (ProjectTool) 🔒

**Nombre de herramienta**: `project`

**Requisito de permisos**: Solo para el Curador de Silicio (`[SiliconManagerOnly]`)

**Escenarios disponibles**: Chat, Task, Timer

**Descripción**: Gestionar espacios de trabajo de proyectos, soporta gestión del ciclo de vida del proyecto, asignación de miembros y gestión de roles.

**Operaciones soportadas**:
- `create` — Crear nuevo espacio de proyecto
- `archive` — Archivar proyecto
- `restore` — Restaurar proyecto archivado
- `destroy` — Destruir proyecto y limpiar datos (irrecuperable)
- `list` — Listar todos los proyectos
- `get` — Obtener detalles del proyecto
- `assign` — Asignar Ser de Silicio al proyecto
- `remove` — Eliminar Ser de Silicio del proyecto
- `update` — Actualizar nombre/descripción del proyecto
- `list-workflow-templates` — Listar plantillas de flujo de trabajo disponibles
- `assign_role` — Asignar rol de proyecto a un Ser de Silicio
- `remove_role` — Eliminar rol de proyecto de un Ser de Silicio
- `list_roles` — Listar asignaciones de roles del proyecto

**Ejemplo de uso**:
```json
{
  "action": "create",
  "name": "Mi Proyecto",
  "description": "Descripción del proyecto"
}
```

---

### 16. Herramienta de Tareas de Proyecto (ProjectTaskTool)

**Nombre de herramienta**: `project_task`

**Escenarios disponibles**: Chat, Task, Timer

**Descripción**: Gestionar tareas dentro del espacio del proyecto, soporta el ciclo de vida completo de las tareas.

**Operaciones soportadas**:
- `create` — Crear tarea de proyecto
- `list` — Listar tareas del proyecto
- `get` — Obtener detalles de la tarea
- `update` — Actualizar título/descripción/prioridad de la tarea
- `assign` — Asignar responsable a la tarea
- `remove_assignee` — Eliminar responsable de la tarea
- `start` — Iniciar tarea
- `complete` — Marcar tarea como completada
- `fail` — Marcar tarea como fallida
- `cancel` — Cancelar tarea
- `delete` — Eliminar tarea
- `stats` — Obtener estadísticas de tareas

**Ejemplo de uso**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "Descripción de la tarea a completar",
  "priority": 5
}
```

---

### 17. Herramienta de Notas de Trabajo de Proyecto (ProjectWorkNoteTool)

**Nombre de herramienta**: `project_work_note`

**Escenarios disponibles**: Chat, Task, Timer

**Descripción**: Gestionar notas de trabajo dentro del espacio del proyecto (públicas, similares a un cuaderno de trabajo), soporta gestión de notas por páginas.

**Operaciones soportadas**:
- `create` — Crear página de nota (requiere `project_id`, `summary` y `content`, opcional `keywords`)
- `read` — Leer página de nota (requiere `project_id` y `page_number` o `note_id`)
- `update` — Actualizar página de nota (requiere `project_id`, `page_number` y `content`, opcional `summary` y `keywords`)
- `delete` — Eliminar página de nota (requiere `project_id` y `page_number` o `note_id`)
- `list` — Listar resúmenes de todas las páginas de notas del proyecto
- `directory` — Generar directorio/resumen de notas
- `search` — Buscar notas por palabra clave (requiere `project_id` y `keyword`, opcional `max_results`)

**Ejemplo de uso**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "Completado el módulo de autenticación de usuarios",
  "content": "## Detalles de implementación\n\n- Uso de token JWT",
  "keywords": "autenticación,JWT"
}
```

---

### 18. Herramienta de Trabajo de Proyecto (ProjectWorkTool) 🔒

**Nombre de herramienta**: `project_work`

**Requisito de permisos**: Solo para el Curador de Silicio (`[SiliconManagerOnly]`)

**Escenarios disponibles**: Project (`[ToolScenario(ToolScenarioFlag.Project)]`, solo disponible en escenario de proyecto)

**Descripción**: Herramienta de operaciones de trabajo del proyecto, para que el Curador gestione los flujos de trabajo del proyecto en el escenario ThinkOnProject.

**Operaciones soportadas**:
- `create-task` — Crear tarea de proyecto
- `assign-task` — Asignar Ser de Silicio a una tarea
- `chat` — Enviar mensaje al chat grupal del proyecto
- `broadcast` — Difundir mensaje al canal del proyecto
- `complete` — Marcar proyecto como completado
- `status` — Obtener estado del proyecto

**Ejemplo de uso**:
```json
{
  "action": "create-task",
  "project_id": "project-uuid",
  "title": "Implementar autenticación de usuarios"
}
```

---

### 19. Herramienta del Sistema (SystemTool)

**Nombre de herramienta**: `system`

**Descripción**: Obtener información del sistema y uso de recursos.

**Operaciones soportadas**:
- `info` — Obtener información del sistema
- `resource_usage` — Obtener uso de recursos
- `find_process` — Buscar proceso
- `list_beings` — Listar Seres de Silicio

**Ejemplo de uso**:
```json
{
  "action": "info"
}
```

---

### 20. Herramienta de Tareas (TaskTool)

**Nombre de herramienta**: `task`

**Descripción**: Gestionar tareas personales del Ser de Silicio.

**Operaciones soportadas**:
- `create` — Crear tarea
- `list` — Listar tareas
- `update` — Actualizar tarea
- `complete` — Completar tarea
- `delete` — Eliminar tarea
- `get_dependencies` — Obtener dependencias

**Ejemplo de uso**:
```json
{
  "action": "create",
  "description": "Revisar código",
  "priority": 5
}
```

---

### 21. Herramienta de Temporizadores (TimerTool)

**Nombre de herramienta**: `timer`

**Descripción**: Crear y gestionar temporizadores.

**Operaciones soportadas**:
- `create` — Crear temporizador
- `list` — Listar temporizadores
- `delete` — Eliminar temporizador
- `pause` — Pausar temporizador
- `resume` — Reanudar temporizador
- `get_execution_history` — Obtener historial de ejecución

**Ejemplo de uso**:
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true,
  "message": "Recordatorio cada hora"
}
```

---

### 22. Herramienta de Auditoría de Tokens (TokenAuditTool) 🔒

**Nombre de herramienta**: `token_audit`

**Requisito de permisos**: Solo para el Curador de Silicio (`[SiliconManagerOnly]`)

**Escenarios disponibles**: Chat, Task, Timer

**Descripción**: Consultar estadísticas y datos de tendencias del uso de tokens de IA.

**Operaciones soportadas**:
- `summary` — Obtener estadísticas resumidas del uso de tokens
- `trend` — Obtener puntos de datos de tendencias del uso de tokens

**Rangos de tiempo soportados**:
- `today` — Últimas 24 horas
- `week` — Últimas 7×24 horas
- `month` — Estadísticas por día
- `year` — Estadísticas por mes

**Ejemplo de uso**:
```json
{
  "action": "summary",
  "time_range": "week"
}
```

---

### 23. Herramienta de Navegador WebView (WebViewBrowserTool)

**Nombre de herramienta**: `webview_browser`

**Escenarios disponibles**: Chat, Task, Timer

**Descripción**: Operaciones de automatización de navegador basadas en Playwright, proporcionando capacidades completas de navegación web, interacción y extracción de datos.

**Operaciones soportadas**:
- `open` — Abrir navegador
- `close` — Cerrar navegador
- `navigate` — Navegar a URL
- `click` — Hacer clic en elemento
- `input` — Ingresar texto
- `scroll` — Desplazar página
- `execute_script` — Ejecutar JavaScript
- `get_page_text` — Obtener texto de la página
- `get_screenshot` — Obtener captura de pantalla
- `wait_for_element` — Esperar que aparezca un elemento
- `get_element_info` — Obtener información del elemento
- `upload_file` — Cargar archivo
- `get_browser_status` — Obtener estado del navegador
- `set_timeout` — Establecer tiempo de espera
- `clear_session` — Limpiar sesión del navegador

**Características**:
- Instancia independiente por cada Ser de Silicio
- Cookies y sesión completamente aislados
- Completamente invisible para el usuario (modo sin cabeza)
- Soporte completo de JavaScript y CSS

**Ejemplo de uso**:
```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

---

### 24. Herramienta de Notas de Trabajo (WorkNoteTool)

**Nombre de herramienta**: `work_note`

**Descripción**: Gestionar notas de trabajo personales del Ser de Silicio (privadas, similares a un diario).

**Operaciones soportadas**:
- `create` — Crear nota
- `read` — Leer nota
- `update` — Actualizar nota
- `delete` — Eliminar nota
- `list` — Listar notas
- `search` — Buscar notas
- `directory` — Generar directorio

**Ejemplo de uso**:
```json
{
  "action": "create",
  "summary": "Completado el módulo de autenticación de usuarios",
  "content": "## Detalles de implementación\n\n- Uso de token JWT\n- Soporte para OAuth2",
  "keywords": "autenticación,JWT,OAuth2"
}
```

---

### 25. Herramienta de Recarga en Caliente (HotReloadTool)

**Nombre de herramienta**: `hot_reload`

**Descripción**: Soporta la compilación automática, actualización de archivos y reinicio de SiliconLife.Fast en ejecución, sin intervención manual.

**Operaciones soportadas**:
- `execute` — Ejecutar el flujo completo de construcción, copia y reinicio
- `build_only` — Solo construir el proyecto, sin copiar ni reiniciar

**Flujo de trabajo**:
1. Compilar el proyecto SiliconLife.Fast
2. Apagar elegantemente la instancia Fast en ejecución (a través de API HTTP)
3. Esperar a que el proceso termine y el puerto se libere
4. Copiar la salida de la construcción al directorio destino (omitir los archivos de HotReload)
5. Reiniciar la instancia Fast

**Características**:
- Detección automática y cierre de procesos antiguos
- Copia segura de archivos (no sobrescribe HotReload.exe)
- Mecanismo de espera de liberación de puerto
- Soporta configuración de puerto personalizado

**Ejemplo de uso**:
```json
{
  "action": "execute",
  "project_path": "src/SiliconLife.Fast",
  "source_path": "src/SiliconLife.Fast/bin/Debug/net9.0",
  "configuration": "Debug",
  "port": 8080
}
```

**Descripción de parámetros**:
- `project_path`: Ruta del proyecto (relativa a la raíz de la solución)
- `source_path`: Directorio de salida de la construcción
- `configuration`: Configuración de construcción (Debug/Release)
- `port`: Puerto Web de la instancia Fast (predeterminado 8080)

**Precauciones**:
- Solo aplica a la versión SiliconLife.Fast
- Requiere que HotReload.exe esté en el directorio tools/HotReload
- Habrá una breve interrupción del servicio durante el reinicio (aproximadamente 3-5 segundos)

---

## Flujo de Llamadas a Herramientas

```
┌──────────┐
│   IA     │ Retorna tool_calls
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ Buscar y verificar derechos de uso de herramientas
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ Verificar cadena de permisos
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Ejecutor    │ Ejecutar operaciones de acceso a recursos
└────┬─────────┘
     ↓
┌──────────┐
│   IA     │ Recibir resultado de la herramienta, continuar pensando
└──────────┘
```

## Verificación de Permisos

Todas las ejecuciones de herramientas pasan por la cadena de verificación de permisos:

1. **UserFrequencyCache** — Caché de decisiones frecuentes del usuario (HighDeny tiene prioridad sobre HighAllow)
2. **IPermissionCallback** — Función de retrollamada de permisos personalizada (Allowed/Denied/AskUser)
3. **Ramificación IsCurator** — El Curador pregunta al usuario a través de IPermissionAskHandler; los no-curadores consultan GlobalACL, sin reglas coincidentes se deniega por defecto

## Crear Herramientas Personalizadas

### Paso 1: Implementar la interfaz ITool

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    
    public string Description => "Descripción de la herramienta";
    
    public ToolDefinition Definition => new ToolDefinition
    {
        Name = Name,
        Description = Description,
        Parameters = new Dictionary<string, object>
        {
            ["param1"] = new { type = "string", description = "Descripción del parámetro" }
        }
    };
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        try
        {
            var param1 = call.Parameters["param1"]?.ToString();
            var result = await DoWork(param1);
            
            return new ToolResult
            {
                Success = true,
                Output = result
            };
        }
        catch (Exception ex)
        {
            return new ToolResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }
}
```

### Paso 2: Agregar al Proyecto

Colocar el archivo de herramienta en el directorio `src/SiliconLife.Common/Tools/` (herramientas compartidas) o en `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` (herramientas específicas de versión). `ToolManager` la descubrirá y registrará automáticamente mediante reflexión al inicio.

### Paso 2a: Registrar Herramientas a través de Plugins

También se pueden registrar herramientas personalizadas a través del sistema de plugins:

1. Implementar la interfaz `ITool` en el proyecto del plugin
2. Compilar la DLL del plugin y colocarla en el directorio de plugins
3. `ToolManager.ScanAllPluginAssemblies()` escaneará automáticamente las implementaciones de ITool en todos los plugins cargados
4. Las herramientas de plugins están sujetas al mismo sistema de permisos

### Paso 3: (Opcional) Marcar como Exclusiva del Curador

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // Solo accesible por el Curador de Silicio
}
```

## Mejores Prácticas

### 1. Siempre Validar Parámetros

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("Falta el parámetro requerido: required_param");
}
```

### 2. Manejar Errores Elegántemente

```csharp
try
{
    // Ejecutar operación
}
catch (Exception ex)
{
    Logger.Error($"Herramienta {Name} falló: {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. Respetar el Sistema de Permisos

Nunca eludir las verificaciones de permisos. Siempre acceder a los recursos a través de los ejecutores:

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return ToolResult.Denied("Permission denied");
}
```

### 4. Proporcionar Descripciones Claras de Herramientas

Ayudar a la IA a entender cuándo y cómo usar la herramienta:

```csharp
public string Description => 
    "Utilizado para convertir fechas entre diferentes sistemas de calendario." +
    "Requiere los parámetros 'date', 'from_calendar' y 'to_calendar'.";
```

## Solución de Problemas

### Herramienta no encontrada

**Problema**: La IA intenta llamar una herramienta que no existe.

**Solución**:
- Verificar que el nombre de la herramienta coincida exactamente
- Verificar que el archivo de herramienta esté en el directorio `Tools/`
- Reconstruir el proyecto (`dotnet build`)

### Permiso denegado

**Problema**: La ejecución de la herramienta falla, devuelve error de permisos.

**Solución**:
- Verificar los registros de auditoría de permisos
- Verificar que el Ser de Silicio tenga los permisos necesarios
- Revisar la configuración de ACL global
- Si es el Curador, verificar si se usó la marca `[SiliconManagerOnly]`

### La ejecución de la herramienta devuelve error

**Problema**: La herramienta se ejecuta pero devuelve un resultado fallido.

**Solución**:
- Verificar el mensaje de error devuelto por la herramienta
- Verificar que el formato de los parámetros de entrada sea correcto
- Revisar los registros del sistema para obtener información detallada del error
- Probar la funcionalidad de la herramienta de forma independiente

## Siguientes Pasos

- 📚 Leer la [guía de arquitectura](architecture.md)
- 🛠️ Consultar la [guía de desarrollo](development-guide.md)
- 🔒 Conocer el [sistema de permisos](permission-system.md)
- 🚀 Ver la [guía de inicio rápido](getting-started.md)
