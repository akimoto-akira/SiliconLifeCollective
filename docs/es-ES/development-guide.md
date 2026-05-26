# Guía de Desarrollo

> **Versión: v0.2.0-alpha**

[English](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | **Español** | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md) | [Русский](../ru-RU/development-guide.md)

## Resumen de Arquitectura

SiliconLifeCollective sigue una **arquitectura Cuerpo-Cerebro**, con separación estricta entre interfaces centrales e implementaciones por defecto.

### Estructura del Proyecto

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfaces, clases abstractas, infraestructura común
│   ├── SiliconLife.Common/          # Implementaciones compartidas (usadas por ambas versiones)
│   ├── SiliconLife.Default/         # Implementación por defecto, punto de entrada (verificación de viabilidad de arquitectura)
│   ├── SiliconLife.Fast/            # Implementación de alto rendimiento, punto de entrada (versión de producción recomendada)
│   ├── SiliconLife.Speedy/          # Motor de almacenamiento de alto rendimiento SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Herramienta de gestión SpeedyPack (Avalonia UI)
└── docs/                            # Documentación multilingüe
```

**Dirección de dependencias**:
- `SiliconLife.Default` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Fast` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Common` → `SiliconLife.Core` (unidireccional)

**Descripción de roles de las versiones**:
- **SiliconLife.Default**: Implementación por defecto, principalmente para verificar la viabilidad de la arquitectura. Proporciona una implementación de almacenamiento en sistema de archivos simple y fiable, adecuada para depuración de desarrollo y verificación de arquitectura.
- **SiliconLife.Fast**: Versión de producción recomendada. Sobre la base de la arquitectura verificada en Default, adopta almacenamiento en memoria SpeedyPack + persistencia asíncrona, proporcionando optimización de rendimiento extrema, siendo la opción preferida para ejecución a largo plazo y entornos de producción reales.

## Conceptos Centrales

### 1. Ser de Silicio

Cada agente de IA se compone de:
- **Cuerpo** (`DefaultSiliconBeing`): Mantiene los signos vitales, detecta escenas de activación
- **Cerebro** (`ContextManager`): Carga el historial, invoca la IA, ejecuta herramientas, persiste las respuestas

### 2. Sistema de Herramientas

Las herramientas se descubren y registran automáticamente mediante reflexión:

```csharp
// Todas las herramientas implementan la interfaz ITool
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. Sistema de Permisos

Cadena de verificación de permisos de 3 niveles:
```
UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curator: GlobalACL → denegación por defecto)
```

### 4. Localizador de Servicios

Registro y recuperación global de servicios:
```csharp
// Registrar
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// Obtener
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## Sistema de Extensión

### Agregar una Nueva Herramienta

1. Crear una nueva clase en `src/SiliconLife.Common/Tools/` (herramientas compartidas por ambas versiones):

> **Nota**: `SiliconLife.Default` y `SiliconLife.Fast` ya no tienen directorios `Tools/` independientes, todas las herramientas compartidas se colocan uniformemente en `SiliconLife.Common/Tools/`.

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Description of what this tool does";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // Analizar parámetros
        var param1 = call.Parameters["param1"]?.ToString();
        
        // Ejecutar lógica
        var result = await DoSomething(param1);
        
        // Retornar resultado
        return new ToolResult 
        { 
            Success = true, 
            Output = result 
        };
    }
}
```

2. La herramienta se descubre automáticamente mediante reflexión — ¡no requiere registro manual!

3. (Opcional) Marcar como solo para administradores:
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

4. (Opcional) Marcar escenarios disponibles de la herramienta:
```csharp
[ToolScenario(ToolScenarioFlag.Chat | ToolScenarioFlag.Task)]
public class MyTool : ITool { ... }
```

5. (Opcional) Marcar como solo disponible en escenario de chat:
```csharp
[ChatOnly]
public class HelpTool : ITool { ... }
```

6. (Opcional) Marcar como solo disponible en escenario de proyecto:
```csharp
[ToolScenario(ToolScenarioFlag.Project)]
[SiliconManagerOnly]
public class ProjectWorkTool : ITool { ... }
```

### Agregar un Nuevo Cliente de IA

1. Implementar `IAIClient` en `src/SiliconLife.Common/AI/`:

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";
    
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        // Llamar a su API de IA
        var response = await CallMyAPI(request);
        
        return new AIResponse
        {
            Content = response.Message,
            ToolCalls = response.ToolCalls,
            Usage = response.Usage
        };
    }
    
    public async IAsyncEnumerable<string> StreamChatAsync(AIRequest request)
    {
        // Implementar streaming
        await foreach (var chunk in StreamFromAPI(request))
        {
            yield return chunk;
        }
    }
}
```

2. Crear una fábrica:

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(AIClientConfig config)
    {
        return new MyAIClient(config);
    }
}
```

3. La fábrica se descubre y registra automáticamente.

### Agregar un Nuevo Backend de Almacenamiento

1. Implementar `IStorage` y `ITimeStorage` en `src/SiliconLife.Default/Storage/` (implementación en sistema de archivos) o `src/SiliconLife.Fast/Storage/` (adaptador SpeedyPack):

```csharp
public class DatabaseStorage : IStorage, ITimeStorage
{
    public async Task<string> ReadAsync(string key)
    {
        // Leer desde su base de datos
    }
    
    public async Task WriteAsync(string key, string value)
    {
        // Escribir en su base de datos
    }
    
    public async Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end)
    {
        // Consulta con índice temporal
    }
}
```

### Agregar un Nuevo Plugin

1. Crear un proyecto de biblioteca de clases, implementando la interfaz `IPlugin`:

```csharp
using SiliconLife.Collective;
using SiliconLife.Collective.Localization;
using SiliconLife.Collective.Tools;

public class MyPlugin : IPlugin
{
    public string Id => "my-plugin";
    public string Version => "1.0.0";
    
    public string GetName(Language language) => "My Plugin";
    public string GetDescription(Language language) => "A custom plugin";
    public string GetAuthor(Language language) => "Author Name";
    
    public void OnLoad() { }
    public void OnStart() { }
    public void OnStop() { }
    public void OnUnload() { }
}
```

2. (Opcional) Implementar la interfaz `ITool` en el plugin para registrar herramientas personalizadas:

```csharp
public class MyPluginTool : ITool
{
    public string Name => "my_plugin_tool";
    public string Description => "A tool provided by my plugin";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        return new ToolResult { Success = true, Output = "Done" };
    }
}
```

3. Colocar la DLL compilada en el directorio de plugins, `PluginLoader` la cargará automáticamente.

> **Restricciones de seguridad**: Los plugins no pueden referenciar espacios de nombres como `System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis`. Los plugins se cargan de forma aislada mediante `AssemblyLoadContext`.

### Agregar una Nueva Piel

1. Implementar `ISkin` en `src/SiliconLife.App/Web/Skins/`:

```csharp
public class MyCustomSkin : ISkin
{
    public string Name => "MySkin";
    public string Description => "A custom skin description";
    
    public string GetCss()
    {
        return @"
            :root {
                --primary-color: #your-color;
                --bg-color: #your-bg;
            }
            /* Your custom styles */
        ";
    }
}
```

2. La piel se descubre automáticamente por `SkinManager`.

## Guía de Estilo de Código

### Convenciones de Nomenclatura

- **Clases**: PascalCase, con prefijo funcional (por ejemplo, `DefaultSiliconBeing`)
- **Interfaces**: Comienzan con `I` (por ejemplo, `IAIClient`, `ITool`)
- **Implementaciones**: Terminan con el nombre de la interfaz (por ejemplo, `OllamaClient` implementa `IAIClient`)
- **Herramientas**: Terminan con `Tool` (por ejemplo, `CalendarTool`, `ChatTool`)
- **Modelos de vista**: Terminan con `ViewModel` (por ejemplo, `BeingViewModel`)

### Organización del Código

```
SiliconLife.Common/
├── AI/                    # Implementaciones de clientes y fábricas de IA
├── Calendar/              # 32 implementaciones de calendario
├── Localization/          # Clase base de localización y 34 variantes de idioma
├── Security/              # Gestor de permisos
├── SiliconBeing/          # Implementación por defecto del Ser de Silicio
├── Tools/                 # Herramientas integradas compartidas (25)
├── Web/                   # Infraestructura Web
└── WebView/               # Implementación Playwright WebView

SiliconLife.App/          # Capa de aplicación compartida por Default y Fast
├── Config/                # Configuración de la aplicación
├── Help/                  # Localización de documentación de ayuda
├── Project/               # Sistema de proyectos (motor de flujos de trabajo, roles de proyecto)
└── Web/                   # Implementación de Web UI
    ├── Component/         # 27 componentes UI
    ├── Controllers/       # 24 controladores de enrutamiento
    ├── Models/            # Modelos de vista
    ├── Views/             # Vistas HTML
    └── Skins/             # 7 temas de piel

SiliconLife.Default/      # Directorios específicos de la versión
├── Config/                # Datos de configuración por defecto
├── Knowledge/             # Implementación de red de conocimiento
├── Logging/               # Implementación de proveedor de registros (consola + sistema de archivos)
├── Project/               # Implementación del sistema de proyectos
└── Storage/               # Implementación de almacenamiento en sistema de archivos

SiliconLife.Fast/         # Directorios específicos de la versión
├── Config/                # Datos de configuración de la versión Fast
├── Logging/               # Implementación de proveedor de registros (consola + sistema de archivos)
├── Storage/               # Adaptadores de almacenamiento SpeedyPack
└── Tray/                  # Localización de la bandeja del sistema
```

### Documentación

- Todas las APIs públicas deben tener comentarios de documentación XML
- Todos los archivos fuente usan encabezado de licencia Apache 2.0
- Aprovechar las características de .NET 9 (using implícito, tipos de referencia anulables)

## Flujo de Trabajo de Desarrollo

### 1. Configurar el Entorno de Desarrollo

```bash
# Clonar el repositorio
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective

# Restaurar dependencias
dotnet restore

# Construir
dotnet build
```

### 2. Ejecutar Pruebas

```bash
# Ejecutar todas las pruebas
dotnet test

# Ejecutar un proyecto de pruebas específico
dotnet test tests/SiliconLife.Core.Tests
```

### 3. Depurar

```bash
# Ejecutar con salida de depuración
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. Formatear Código

```bash
# Formatear código
dotnet format
```

## Construir Funcionalidades Personalizadas

### Ejemplo: Agregar un Calendario Personalizado

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Su lógica de conversión
        return new CalendarDate(year, month, day);
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Conversión inversa
        return new GregorianDate(year, month, day);
    }
}
```

### Ejemplo: Agregar un Ejecutor Personalizado

```csharp
public class CustomExecutor : ExecutorBase
{
    public override string Name => "custom";
    
    public override async Task<ExecutorResult> ExecuteAsync(ExecutorRequest request)
    {
        var permission = await CheckPermissionAsync(request);
        if (!permission.Allowed)
        {
            return ExecutorResult.Denied(permission.Reason);
        }
        
        var result = await PerformOperation(request);
        
        return ExecutorResult.Success(result);
    }
}
```

### Ejemplo: Agregar una Plantilla de Flujo de Trabajo Personalizada

```csharp
public class MyWorkflowTemplate : WorkflowTemplate
{
    public override string Name => "my_workflow";
    public override string Description => "A custom workflow template";
    
    public override void DefineStates()
    {
        AddState("start", "Inicio", isInitial: true);
        AddState("processing", "Procesando");
        AddState("review", "Revisión");
        AddState("done", "Completado", isFinal: true);
    }
    
    public override void DefineTransitions()
    {
        AddTransition("start", "processing", "Iniciar procesamiento");
        AddTransition("processing", "review", "Enviar para revisión");
        AddTransition("review", "done", "Revisión aprobada");
        AddTransition("review", "processing", "Revisión devuelta");
    }
}
```

### Ejemplo: Agregar un Rol de Proyecto

Los roles de proyecto se gestionan a través de las operaciones `assign_role` y `remove_role` de `ProjectTool`. Los nombres de roles son cadenas personalizadas, utilizadas para distinguir las responsabilidades de los Seres de Silicio en los flujos de trabajo y la asignación de tareas.

## Guía de Pruebas

### Pruebas Unitarias

```csharp
[TestClass]
public class MyToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccess()
    {
        // Preparar
        var tool = new MyCustomTool();
        var call = new ToolCall 
        { 
            Name = "my_custom_tool",
            Parameters = new Dictionary<string, object> 
            { 
                ["param1"] = "test" 
            }
        };
        
        // Ejecutar
        var result = await tool.ExecuteAsync(call);
        
        // Verificar
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Output);
    }
}
```

### Pruebas de Integración

Probar flujos completos:
1. La IA retorna una llamada a herramienta
2. La herramienta se ejecuta
3. El resultado se retroalimenta a la IA
4. La IA retorna la respuesta final

## Consideraciones de Rendimiento

### Sistema de Almacenamiento

- La versión Default usa almacenamiento JSON basado en archivos
- La versión Fast usa el motor de almacenamiento en memoria SpeedyPack (formato .spk)
- SpeedyPack adopta mapeo de directorios en memoria + caché de entradas + cola de escritura asíncrona
- Las consultas con índice temporal usan la interfaz `ITimeStorage`

### Programador del Bucle Principal

- Programación justa por intervalos de tiempo basada en reloj
- Perro guardián para detectar operaciones bloqueadas
| Interruptor de circuito para prevenir fallos en cascada

## Mejores Prácticas

### 1. Siempre Verificar Permisos

Cualquier operación iniciada por la IA debe pasar por la cadena de permisos:

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return Result.Denied("Permission denied");
}
```

### 2. Usar el Localizador de Servicios

Registrar y recuperar servicios globalmente:

```csharp
// Durante la inicialización
ServiceLocator.Instance.Register<ICustomService>(myService);

// Cuando se necesite
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. Seguir la Separación Cuerpo-Cerebro

- El cuerpo maneja estado y activación
- El cerebro maneja interacción con IA y ejecución de herramientas

### 4. Implementar Manejo Adecuado de Errores

```csharp
try
{
    var result = await operation();
    return Result.Success(result);
}
catch (Exception ex)
{
    Logger.Error($"Operation failed: {ex.Message}");
    return Result.Failure(ex.Message);
}
```

## Guía de Contribución

1. Hacer fork del repositorio
2. Crear una rama de funcionalidad (`git checkout -b feature/amazing-feature`)
3. Confirmar sus cambios usando commits convencionales
4. Empujar a la rama (`git push origin feature/amazing-feature`)
5. Abrir un Pull Request

### Formato de Mensajes de Confirmación

```
<type>(<scope>): <description>

Ejemplos:
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## Siguientes Pasos

- 📚 Leer la [guía de arquitectura](architecture.md)
- 📖 Explorar la [referencia de API](api-reference.md)
- 🔒 Consultar la [documentación de seguridad](security.md)
- 🚀 Ver la [guía de inicio rápido](getting-started.md)
