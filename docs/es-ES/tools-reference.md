# Referencia de Herramientas

## Herramientas Integradas

Silicon Life Collective incluye varias herramientas integradas que los Seres de Silicio pueden usar.

---

## CalendarTool

**Descripción**: Consulta y conversión entre 32 sistemas de calendario.

### Métodos

- `ConvertDate(DateTime date, string targetCalendar)`: Convertir fecha a otro calendario
- `GetCurrentDate(string calendar)`: Obtener fecha actual en calendario específico
- `GetSupportedCalendars()`: Listar calendarios soportados

### Ejemplo

```csharp
var tool = new CalendarTool();
var result = tool.Execute("ConvertDate", new { 
    date = DateTime.Now, 
    targetCalendar = "ChineseLunar" 
});
```

### Calendarios Soportados

Gregorian, ChineseLunar, Islamic, Hebrew, Japanese, Persian, Mayan, Buddhist, Indian, y 23 más.

---

## ChatTool

**Descripción**: Enviar y recibir mensajes a través del sistema de chat.

### Métodos

- `SendMessage(string channelId, string message)`: Enviar mensaje
- `GetMessages(string channelId, int count)`: Obtener mensajes recientes
- `CreateChannel(string name)`: Crear nuevo canal

### Ejemplo

```csharp
var tool = new ChatTool();
var result = tool.Execute("SendMessage", new {
    channelId = "channel-guid",
    message = "Hola desde herramienta"
});
```

---

## ConfigTool

**Descripción**: Leer y modificar configuración del sistema.

### Métodos

- `GetValue(string key)`: Obtener valor de configuración
- `SetValue(string key, object value)`: Establecer valor
- `GetAll()`: Obtener toda la configuración

### Ejemplo

```csharp
var tool = new ConfigTool();
var aiConfig = tool.Execute("GetValue", new { key = "AIClients:Ollama:Model" });
```

---

## CuratorTool

**Descripción**: Herramienta exclusiva del Curador para gestionar seres.

### Métodos

- `CreateBeing(string name, string soul)`: Crear nuevo ser
- `DeleteBeing(string beingId)`: Eliminar ser
- `ListBeings()`: Listar todos los seres
- `StartBeing(string beingId)`: Iniciar ser
- `StopBeing(string beingId)`: Detener ser

### Ejemplo

```csharp
var tool = new CuratorTool();
var result = tool.Execute("CreateBeing", new {
    name = "Asistente",
    soul = "# Personalidad\nEres útil..."
});
```

**Nota**: Esta herramienta tiene el atributo `[SiliconManagerOnly]`.

---

## DiskTool

**Descripción**: Operaciones de archivos y directorios.

### Métodos

- `ReadFile(string path)`: Leer archivo
- `WriteFile(string path, string content)`: Escribir archivo
- `ListDirectory(string path)`: Listar directorio
- `CreateDirectory(string path)`: Crear directorio
- `DeleteFile(string path)`: Eliminar archivo

### Ejemplo

```csharp
var tool = new DiskTool();
var content = tool.Execute("ReadFile", new { path = "/data/file.txt" });
```

**Nota**: Requiere permiso `FileAccess`.

---

## DynamicCompileTool

**Descripción**: Compilar y ejecutar código C# dinámicamente.

### Métodos

- `Compile(string code)`: Compilar código
- `Execute(string code, string[] args)`: Compilar y ejecutar
- `ScanSecurity(string code)`: Escanear código por seguridad

### Ejemplo

```csharp
var tool = new DynamicCompileTool();
var result = tool.Execute("Execute", new {
    code = "public class Program { public static string Run() => \"Hola\"; }",
    args = new string[0]
});
```

**Nota**: El código pasa por SecurityScanner antes de compilar.

---

## MemoryTool

**Descripción**: Gestionar memoria a largo plazo de los seres.

### Métodos

- `Save(string key, string value)`: Guardar en memoria
- `Load(string key)`: Cargar de memoria
- `Delete(string key)`: Eliminar de memoria
- `Search(string query)`: Buscar en memoria

### Ejemplo

```csharp
var tool = new MemoryTool();
tool.Execute("Save", new {
    key = "user_preference",
    value = "prefiere respuestas cortas"
});
```

---

## NetworkTool

**Descripción**: Realizar solicitudes HTTP.

### Métodos

- `Get(string url)`: Solicitud GET
- `Post(string url, string body)`: Solicitud POST
- `Put(string url, string body)`: Solicitud PUT
- `Delete(string url)`: Solicitud DELETE

### Ejemplo

```csharp
var tool = new NetworkTool();
var response = tool.Execute("Get", new { url = "https://api.example.com/data" });
```

**Nota**: Requiere permiso `NetworkAccess`.

---

## SystemTool

**Descripción**: Información del sistema y operaciones.

### Métodos

- `GetSystemInfo()`: Información del sistema
- `GetMemoryUsage()`: Uso de memoria
- `GetCpuUsage()`: Uso de CPU
- `GetUptime()`: Tiempo de actividad

### Ejemplo

```csharp
var tool = new SystemTool();
var info = tool.Execute("GetSystemInfo", new { });
```

---

## TaskTool

**Descripción**: Gestionar tareas de los seres.

### Métodos

- `CreateTask(string title, string description)`: Crear tarea
- `UpdateTask(string taskId, string status)`: Actualizar tarea
- `CompleteTask(string taskId)`: Completar tarea
- `ListTasks(string status)`: Listar tareas

### Ejemplo

```csharp
var tool = new TaskTool();
var task = tool.Execute("CreateTask", new {
    title = "Investigar tema",
    description = "Buscar información sobre..."
});
```

---

## TimerTool

**Descripción**: Configurar temporizadores.

### Métodos

- `SetTimer(string name, TimeSpan delay, string action)`: Configurar temporizador
- `CancelTimer(string name)`: Cancelar temporizador
- `ListTimers()`: Listar temporizadores activos

### Ejemplo

```csharp
var tool = new TimerTool();
tool.Execute("SetTimer", new {
    name = "recordatorio",
    delay = TimeSpan.FromMinutes(30),
    action = "Enviar mensaje de recordatorio"
});
```

---

## TokenAuditTool

**Descripción**: Consultar uso de tokens de IA.

### Métodos

- `GetUsage(string beingId, DateTime start, DateTime end)`: Uso de tokens
- `GetSummary(string beingId)`: Resumen de uso
- `GetAllUsage()`: Uso de todos los seres

### Ejemplo

```csharp
var tool = new TokenAuditTool();
var usage = tool.Execute("GetUsage", new {
    beingId = "being-guid",
    start = DateTime.Today,
    end = DateTime.Now
});
```

---

## DatabaseTool

**Descripción**: Consulta y operaciones de base de datos estructurada.

### Acciones

- `query`: Consultar datos
- `insert`: Insertar datos
- `update`: Actualizar datos
- `delete`: Eliminar datos

### Ejemplo

```json
{
  "action": "query",
  "table": "users",
  "conditions": {"status": "active"},
  "limit": 100
}
```

### Permiso

Requiere permisos de acceso a base de datos apropiados.

---

## LogTool

**Descripción**: Consultas de historial de operaciones y conversaciones.

### Acciones

- `query_logs`: Consultar registros de operaciones
- `query_conversations`: Consultar historial de conversaciones

### Ejemplo (query_logs)

```json
{
  "action": "query_logs",
  "being_id": "being-uuid",
  "start_time": "2026-04-20T00:00:00Z",
  "end_time": "2026-04-23T23:59:59Z",
  "level": "info"
}
```

### Ejemplo (query_conversations)

```json
{
  "action": "query_conversations",
  "being_id": "being-uuid",
  "session_id": "session-uuid",
  "limit": 50
}
```

### Funcionalidades

- Soporte para filtrar registros por Ser de Silicio
- Soporte para consultas de rango de tiempo
- Soporte para filtrado por nivel de registro
- Recuperación de historial de conversaciones

---

## Mejoras de DiskTool

DiskTool ahora incluye funcionalidad de búsqueda local (integrada desde SearchTool):

### Nuevas Acciones

- `search_files`: Buscar archivos
- `search_content`: Buscar contenido

### Ejemplo (search_files)

```json
{
  "action": "search_files",
  "path": "/data",
  "pattern": "*.json",
  "recursive": true
}
```

### Ejemplo (search_content)

```json
{
  "action": "search_content",
  "path": "/data",
  "query": "término de búsqueda",
  "filePattern": "*.md"
}
```

---

## Crear Herramientas Personalizadas

Puedes crear herramientas personalizadas implementando `ITool`:

```csharp
public class MyCustomTool : ITool
{
    public string Name => "MiHerramienta";
    public string Description => "Mi herramienta personalizada";
    
    public ToolResult Execute(string method, object parameters)
    {
        // Lógica de la herramienta
        return new ToolResult { Success = true, Message = "Éxito" };
    }
}
```

La herramienta se registrará automáticamente mediante reflexión.
