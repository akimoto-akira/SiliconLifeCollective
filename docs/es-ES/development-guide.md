# Guía de Desarrollo

> **Versión: v0.2.0-alpha**

[English](../en/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | **Español** | [Deutsch](../de-DE/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md)

## Resumen de Arquitectura

SiliconLifeCollective sigue la **arquitectura cuerpo-cerebro**, con estricta separación entre interfaces centrales e implementaciones predeterminadas.

### Estructura del Proyecto

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # Interfaces, clases abstractas, infraestructura común
│   ├── SiliconLife.Common/          # Implementaciones compartidas (usadas por ambas versiones)
│   ├── SiliconLife.Default/         # Implementación predeterminada, punto de entrada (verificación de viabilidad de arquitectura)
│   ├── SiliconLife.Fast/            # Implementación de alto rendimiento, punto de entrada (versión de producción principal)
│   ├── SiliconLife.Speedy/          # Motor de almacenamiento de alto rendimiento SpeedyPack
│   └── SiliconLife.Speedy.Manager/  # Herramienta de gestión SpeedyPack (Windows Forms)
└── docs/                            # Documentación multilingüe
```

**Dirección de dependencia**:
- `SiliconLife.Default` → `SiliconLife.Core` (unidireccional)
- `SiliconLife.Fast` → `SiliconLife.Core` (unidireccional)
- `SiliconLife.Common` → `SiliconLife.Core` (unidireccional)

**Descripción de Roles de Versión**:
- **SiliconLife.Default**: Implementación predeterminada, utilizada principalmente para verificación de viabilidad de arquitectura. Proporciona una implementación de almacenamiento en sistema de archivos simple y confiable, adecuada para depuración de desarrollo y verificación de arquitectura.
- **SiliconLife.Fast**: Versión de producción principal. Basada en la arquitectura verificada en Default, adopta almacenamiento en memoria SpeedyPack + persistencia asíncrona (formato de archivo .spk) para proporcionar optimización extrema de rendimiento. La mejor opción para operaciones a largo plazo y entornos de producción reales.

## Conceptos Centrales

### 1. Ser Silicona

Cada agente de IA consiste en:
- **Cuerpo** (`DefaultSiliconBeing`): Mantiene estado de vida, detecta escenarios de activación
- **Cerebro** (`ContextManager`): Carga historial, invoca IA, ejecuta herramientas, persiste respuestas

### 2. Sistema de Herramientas

Las herramientas se descubren y registran automáticamente a través de reflexión:

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

Cadena de verificación de permisos de 5 niveles:
```
IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
```

### 4. Localizador de Servicios

Registro y recuperación global de servicios:
```csharp
// Registrar
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// Obtener
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## Extender el Sistema

### Añadir Nueva Herramienta

1. Crear nueva clase en `src/SiliconLife.Common/Tools/` (herramientas compartidas entre ambas versiones) o `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` (herramientas específicas de versión):

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
        
        // Devolver resultado
        return new ToolResult 
        { 
            Success = true, 
            Output = result 
        };
    }
}
```

2. ¡La herramienta se descubre automáticamente a través de reflexión - no se necesita registro manual!

3. (Opcional) Marcar como solo para administrador:
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

### Añadir Nuevo Cliente de IA

1. Implementar `IAIClient` en `src/SiliconLife.Common/AI/`:

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";
    
    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        // Llamar a tu API de IA
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

2. Crear fábrica:

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

### Añadir Nuevo Backend de Almacenamiento

1. Implementar `IStorage` e `ITimeStorage` en `src/SiliconLife.Default/Storage/` (implementación de sistema de archivos) o `src/SiliconLife.Fast/Storage/` (adaptador SpeedyPack):

```csharp
public class DatabaseStorage : IStorage, ITimeStorage
{
    public async Task<string> ReadAsync(string key)
    {
        // Leer de tu base de datos
    }
    
    public async Task WriteAsync(string key, string value)
    {
        // Escribir en tu base de datos
    }
    
    public async Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end)
    {
        // Consulta indexada por tiempo
    }
}
```

### Añadir Nuevo Plugin

1. Crear un proyecto de biblioteca de clases que implemente la interfaz `IPlugin`:

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

> **Restricciones de seguridad**: Los plugins no pueden referenciar espacios de nombres como `System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis`. Los plugins se cargan de forma aislada a través de `AssemblyLoadContext`.

### Añadir Nuevo Calendario

1. Heredar de `CalendarBase`:

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";
    
    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // Lógica de conversión
    }
    
    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // Lógica de conversión inversa
    }
}
```

2. El calendario se registra automáticamente

### Añadir Nueva Piel

1. Implementar `ISkin` en `src/SiliconLife.App/Web/Skins/`:

```csharp
public class MyCustomSkin : ISkin
{
    public string Name => "MySkin";
    
    public string GetCss()
    {
        return ":root { --primary: #color; }";
    }
    
    // Implementar otros métodos de UI
}
```

2. La piel se descubre automáticamente a través de reflexión

## Estilo de Código

### Convenciones de Nomenclatura

- **Clases**: PascalCase, con prefijo funcional (ej. `DefaultSiliconBeing`)
- **Interfaces**: Prefijo I (ej. `IAIClient`, `ITool`)
- **Implementaciones**: Terminan con el nombre de la interfaz (ej. `OllamaClient` implementa `IAIClient`)
- **Herramientas**: Terminan con `Tool` (ej. `CalendarTool`, `ChatTool`)
- **Modelos de Vista**: Terminan con `ViewModel` (ej. `BeingViewModel`)

### Organización del Código

```
SiliconLife.Common/
├── AI/                    # Implementaciones de clientes IA y fábricas
├── Calendar/              # 32 implementaciones de calendario
├── Localization/          # Clase base de localización y 29 implementaciones de idioma
├── Security/              # Gestor de permisos
├── SiliconBeing/          # Implementación predeterminada de Ser Silicona
├── Tools/                 # Herramientas integradas compartidas
├── Web/                   # Infraestructura Web
└── WebView/               # Implementación Playwright WebView

SiliconLife.App/          # Capa de aplicación compartida entre Default y Fast
├── Config/                # Configuración de la aplicación
├── Help/                  # Localización de documentación de ayuda
└── Web/                   # Implementación de Web UI
    ├── Component/         # Biblioteca de componentes UI
    ├── Controllers/       # Controladores de rutas
    ├── Models/            # Modelos de vista
    ├── Views/             # Vistas HTML
    └── Skins/             # Temas de piel

SiliconLife.Default/      # Directorios específicos de versión
├── Config/                # Datos de configuración predeterminados
├── IM/                    # Proveedor WebUI
├── Knowledge/             # Implementación de red de conocimiento
├── Logging/               # Implementaciones de proveedores de registro
├── Project/               # Implementación del sistema de proyectos
├── Security/              # Callbacks de permisos predeterminados
├── Storage/               # Implementación de almacenamiento en sistema de archivos
└── Tools/                 # Herramientas específicas de versión (HelpTool)
```

### Comentarios

- Usar XML docs para APIs públicas
- Comentarios en inglés para consistencia
- Documentar parámetros y valores de retorno

```csharp
/// <summary>
/// Ejecutar una ronda de pensamiento para el ser.
/// </summary>
/// <param name="cancellationToken">Token de cancelación</param>
/// <returns>Tarea que representa la operación asíncrona</returns>
public async Task ExecuteOneRoundAsync(CancellationToken cancellationToken)
{
    // Implementación
}
```

## Pruebas

### Pruebas Unitarias

Crear pruebas en proyecto de pruebas separado:

```csharp
[TestClass]
public class MyCustomToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidParameters_ReturnsSuccess()
    {
        // Arrange
        var tool = new MyCustomTool();
        var call = new ToolCall { Parameters = new Dictionary<string, object>() };
        
        // Act
        var result = await tool.ExecuteAsync(call);
        
        // Assert
        Assert.IsTrue(result.Success);
    }
}
```

### Pruebas de Integración

Probar interacción de componentes:

```csharp
[TestMethod]
public async Task BeingLifecycle_CreateStartStop_WorksCorrectly()
{
    // Crear ser
    // Iniciar
    // Verificar estado
    // Detener
    // Verificar estado
}
```

## Depuración

### Habilitar Registro Detallado

```csharp
config.Logging.Level = LogLevel.Debug;
```

### Usar Punto de Interrupción

Establecer puntos de interrupción en:
- Ejecución de herramientas
- Verificación de permisos
- Comunicación con IA

### Inspeccionar Estado del Ser

```csharp
var state = being.GetState();
Console.WriteLine($"Estado: {state.Status}");
```

## Consideraciones de Rendimiento

### Sistema de Almacenamiento

- La versión Default usa almacenamiento JSON basado en archivos
- La versión Fast usa el motor de almacenamiento en memoria SpeedyPack (formato .spk)
- SpeedyPack adopta mapeo de directorios en memoria + caché de entradas + cola de escritura asíncrona
- Las consultas indexadas por tiempo usan la interfaz `ITimeStorage`

### Programador del Bucle Principal

- Programación justa por intervalo de tiempo basada en reloj
- Temporizador watchdog para detectar operaciones bloqueadas
- Cortacircuitos para prevenir fallos en cascada

## Guía de Contribución

### Flujo de Trabajo de Desarrollo

1. Hacer fork del repositorio
2. Crear rama de característica (`git checkout -b feature/AmazingFeature`)
3. Implementar característica
4. Añadir pruebas
5. Confirmar cambios (`git commit -m 'feat: add AmazingFeature'`)
6. Push a la rama (`git push origin feature/AmazingFeature`)
7. Enviar Pull Request

### Revisión de Código

- Código debe seguir convenciones de estilo
- Todas las pruebas deben pasar
- Documentación actualizada
- Sin regresiones de rendimiento

### Política de Fusión

- Al menos una aprobación requerida
- Todas las verificaciones de CI deben pasar
- Actualizar documentación si es necesario

## Próximos Pasos

- 📚 Leer la [Guía de Arquitectura](architecture.md)
- 🔧 Ver la [Referencia de Herramientas](tools-reference.md)
- 🌐 Consultar la [Guía de Web UI](web-ui-guide.md)
- 🚀 Comenzar con la [Guía de Inicio Rápido](getting-started.md)
