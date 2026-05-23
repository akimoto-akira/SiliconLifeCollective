# Referencia de Herramientas

> **Versión: v0.2.0-alpha**

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
- **Herramientas de plugins** — Herramientas de terceros registradas a través del sistema de plugins

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
  "message": "¡Hola, colaboremos!"
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
- `get_ai_config` — Obtener configuración de cliente de IA
- `set_ai_config` — Establecer configuración de cliente de IA

**Ejemplo de uso**:
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

---

### 4. Herramienta de Curador (CuratorTool) 🔒

**Nombre de herramienta**: `curator`

**Requisito de permiso**: Solo para Curador Silicona

**Descripción**: Herramienta de administración del sistema exclusiva del Curador Silicona.

**Operaciones soportadas**:
- `create_being` — Crear nuevo Ser Silicona
- `list_beings` — Listar todos los Seres Silicona
- `get_being_info` — Obtener información del ser
- `assign_task` — Asignar tarea
- `manage_permissions` — Gestionar permisos

**Ejemplo de uso**:
```json
{
  "action": "create_being",
  "name": "Asistente",
  "soul_file": "assistant_soul.md"
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

**Descripción**: Operaciones de sistema de archivos y búsqueda local.

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

**Requisito de permiso**: `disk:read`, `disk:write`

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

**Descripción**: Compilación dinámica de código C# (para auto-evolución de Seres Silicona).

**Operaciones soportadas**:
- `compile_class` — Compilar clase
- `compile_callback` — Compilar función callback de permisos
- `validate_code` — Validar seguridad del código

**Mecanismos de seguridad**:
- Control de referencias en compilación (excluir ensamblados peligrosos)
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

**Requisito de permiso**: Solo para Curador Silicona

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

**Descripción**: Obtener documentación de ayuda del sistema y guías de uso.

**Operaciones soportadas**:
- `get_topics` — Obtener lista de temas de ayuda
- `get_topic` — Obtener detalles de un tema específico
- `search` — Buscar documentación de ayuda

**Ejemplo de uso**:
```json
{
  "action": "get_topics"
}
```

---

### 10. Herramienta de Red de Conocimiento (KnowledgeTool)

**Nombre de herramienta**: `knowledge`

**Descripción**: Operaciones de grafo de conocimiento (basado en tripletas: sujeto-relación-objeto).

**Operaciones soportadas**:
- `add` — Añadir tripleta de conocimiento
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

### 11. Herramienta de Registro (LogTool)

**Nombre de herramienta**: `log`

**Descripción**: Consultar historial de operaciones e historial de conversaciones.

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

**Descripción**: Gestionar memoria a largo y corto plazo de los Seres Silicona.

**Operaciones soportadas**:
- `read` — Leer memoria
- `write` — Escribir memoria
- `search` — Buscar memoria
- `delete` — Eliminar memoria
- `list` — Listar memorias
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
- `upload` — Subir archivo

**Requisito de permiso**: `network:http`

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

**Requisito de permiso**: Solo para Curador Silicona

**Descripción**: Gestionar permisos y listas de control de acceso.

**Operaciones soportadas**:
- `query_permission` — Consultar permiso
- `manage_acl` — Gestionar ACL global
- `get_callback` — Obtener función callback de permisos
- `set_callback` — Establecer función callback de permisos

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

### 15. Herramienta de Proyecto (ProjectTool)

**Nombre de herramienta**: `project`

**Descripción**: Gestionar espacios de trabajo de proyectos.

**Operaciones soportadas**:
- `create` — Crear proyecto
- `list` — Listar proyectos
- `get_info` — Obtener información del proyecto
- `update` — Actualizar proyecto
- `archive` — Archivar proyecto

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

**Descripción**: Gestionar tareas de proyecto.

**Operaciones soportadas**:
- `create` — Crear tarea
- `list` — Listar tareas
- `update` — Actualizar tarea
- `complete` — Completar tarea
- `get_stats` — Obtener estadísticas de tareas

**Ejemplo de uso**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "Descripción de la tarea",
  "priority": 5
}
```

---

### 17. Herramienta de Notas de Trabajo de Proyecto (ProjectWorkNoteTool)

**Nombre de herramienta**: `project_work_note`

**Descripción**: Gestionar notas de trabajo de proyecto (públicas, similar a cuaderno de trabajo).

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
  "project_id": "project-uuid",
  "summary": "Módulo de autenticación completado",
  "content": "## Detalles de implementación\n\n- Usando JWT token",
  "keywords": "autenticación,JWT"
}
```

---

### 18. Herramienta del Sistema (SystemTool)

**Nombre de herramienta**: `system`

**Descripción**: Obtener información del sistema y uso de recursos.

**Operaciones soportadas**:
- `info` — Obtener información del sistema
- `resource_usage` — Obtener uso de recursos
- `find_process` — Buscar proceso
- `list_beings` — Listar Seres Silicona

**Ejemplo de uso**:
```json
{
  "action": "info"
}
```

---

### 19. Herramienta de Tareas (TaskTool)

**Nombre de herramienta**: `task`

**Descripción**: Gestionar tareas personales de los Seres Silicona.

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

### 20. Herramienta de Temporizadores (TimerTool)

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

### 21. Herramienta de Auditoría de Tokens (TokenAuditTool) 🔒

**Nombre de herramienta**: `token_audit`

**Requisito de permiso**: Solo para Curador Silicona

**Descripción**: Consultar y resumir uso de tokens de IA.

**Operaciones soportadas**:
- `get_usage` — Obtener estadísticas de uso de tokens
- `get_by_being` — Obtener uso por ser
- `get_by_model` — Obtener uso por modelo
- `get_trend` — Obtener tendencia de uso
- `export` — Exportar datos

**Ejemplo de uso**:
```json
{
  "action": "get_usage",
  "start_date": "2026-04-01",
  "end_date": "2026-04-26"
}
```

---

### 22. Herramienta de Navegador WebView (WebViewBrowserTool)

**Nombre de herramienta**: `webview`

**Descripción**: Automatización de navegador basada en Playwright.

**Operaciones soportadas**:
- `open_browser` — Abrir navegador
- `close_browser` — Cerrar navegador
- `navigate` — Navegar a URL
- `click` — Hacer clic en elemento
- `input` — Introducir texto
- `get_page_text` — Obtener texto de la página
- `get_screenshot` — Obtener captura de pantalla
- `execute_script` — Ejecutar JavaScript
- `wait_for_element` — Esperar aparición de elemento
- `get_browser_status` — Obtener estado del navegador

**Características**:
- Instancia independiente por Ser Silicona
- Cookies y sesiones completamente aisladas
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

### 23. Herramienta de Notas de Trabajo (WorkNoteTool)

**Nombre de herramienta**: `work_note`

**Descripción**: Gestionar notas de trabajo personales de los Seres Silicona (privadas, similar a diario).

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
  "summary": "Módulo de autenticación completado",
  "content": "## Detalles de implementación\n\n- Usando JWT token\n- Soporte OAuth2",
  "keywords": "autenticación,JWT,OAuth2"
}
```

---

### 24. Herramienta de Recarga en Caliente (HotReloadTool)

**Nombre de la herramienta**: `hot_reload`

**Descripción**: Soporta compilación automática, actualización de archivos y reinicio de SiliconLife.Fast durante la ejecución, sin intervención manual.

**Operaciones soportadas**:
- `execute` — Ejecuta el proceso completo de build, copia y reinicio
- `build_only` — Solo compila el proyecto, sin copiar ni reiniciar

**Flujo de trabajo**:
1. Compila el proyecto SiliconLife.Fast
2. Cierra elegantemente la instancia Fast en ejecución (mediante API HTTP)
3. Espera la salida del proceso y la liberación del puerto
4. Copia la salida de build al directorio objetivo (excluye archivos de HotReload)
5. Reinicia la instancia Fast

**Características**:
- Detección y cierre automático del proceso anterior
- Copia segura de archivos (no sobrescribe HotReload.exe)
- Mecanismo de espera de liberación de puerto
- Soporte para configuración de puerto personalizado

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
- `project_path`: Ruta del proyecto (relativa al directorio raíz de la solución)
- `source_path`: Directorio de salida de build
- `configuration`: Configuración de build (Debug/Release)
- `port`: Puerto Web de la instancia Fast (por defecto 8080)

**Notas**:
- Aplicable solo a la versión SiliconLife.Fast
- Requiere HotReload.exe en el directorio tools/HotReload
- Breve interrupción del servicio durante el reinicio (aproximadamente 3-5 segundos)

## Flujo de Invocación de Herramientas

```
┌──────────┐
│   IA     │ Devuelve tool_calls
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
│  Executor    │ Ejecutar operaciones de acceso a recursos
└────┬─────────┘
     ↓
┌──────────┐
│   IA     │ Recibir resultados de herramientas, continuar pensando
└──────────┘
```

## Verificación de Permisos

Todas las ejecuciones de herramientas pasan por la cadena de permisos de 5 niveles:

1. **UserFrequencyCache** — Decisiones de usuario en caché (HighDeny/HighAllow)
2. **IPermissionCallback** — Función callback de permisos personalizada
3. **Bifurcación IsCurator** — Curador→IPermissionAskHandler / No-Curador→GlobalACL→Denegado

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

### Paso 2: Añadir al proyecto

Colocar el archivo de herramienta en el directorio `src/SiliconLife.Common/Tools/` (herramientas compartidas) o en el directorio `src/SiliconLife.App/Tools/` (herramientas específicas de versión). `ToolManager` descubrirá y registrará automáticamente a través de reflexión al inicio.

### Paso 2a: Registrar herramientas a través de plugins

También se pueden registrar herramientas personalizadas a través del sistema de plugins:

1. Implementar la interfaz `ITool` en el proyecto del plugin
2. Compilar el DLL del plugin y colocarlo en el directorio de plugins
3. `ToolManager.ScanAllPluginAssemblies()` escaneará automáticamente todas las implementaciones de ITool en los plugins cargados
4. Las herramientas de plugins están sujetas al mismo sistema de permisos

### Paso 3: (Opcional) Marcar como exclusivo del curador

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // Solo accesible por el Curador Silicona
}
```

## Mejores Prácticas

### 1. Siempre validar parámetros

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("Falta parámetro requerido: required_param");
}
```

### 2. Manejar errores elegantemente

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

### 3. Respetar el sistema de permisos

Nunca eludir las verificaciones de permisos. Siempre acceder a recursos a través de ejecutores:

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return ToolResult.Denied(permission.Reason);
}
```

### 4. Proporcionar descripciones claras de herramientas

Ayudar a la IA a entender cuándo y cómo usar la herramienta:

```csharp
public string Description => 
    "Para convertir fechas entre diferentes sistemas de calendario." +
    "Requiere parámetros 'date', 'from_calendar' y 'to_calendar'.";
```

## Solución de Problemas

### Herramienta no encontrada

**Problema**: La IA intenta invocar una herramienta que no existe.

**Solución**:
- Verificar que el nombre de la herramienta coincida exactamente
- Validar que el archivo de herramienta está en el directorio `Tools/`
- Reconstruir el proyecto (`dotnet build`)

### Permiso denegado

**Problema**: La ejecución de la herramienta falla, devuelve error de permisos.

**Solución**:
- Verificar registros de auditoría de permisos
- Validar que el Ser Silicona tiene los permisos requeridos
- Revisar configuración de ACL global
- Si es el curador, verificar si se usó la marca `[SiliconManagerOnly]`

### Herramienta devuelve error en ejecución

**Problema**: La herramienta se ejecuta pero devuelve resultado fallido.

**Solución**:
- Verificar el mensaje de error devuelto por la herramienta
- Validar que el formato de los parámetros de entrada es correcto
- Revisar registros del sistema para información detallada del error
- Probar la funcionalidad de la herramienta de forma independiente

## Próximos Pasos

- 📚 Leer la [Guía de Arquitectura](architecture.md)
- 🛠️ Consultar la [Guía de Desarrollo](development-guide.md)
- 🔒 Entender el [Sistema de Permisos](permission-system.md)
- 🚀 Ver la [Guía de Inicio Rápido](getting-started.md)
