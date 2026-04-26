# Guía de Desarrollo

[English](../en/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | **Español** | [Deutsch](../de-DE/development-guide.md) | [日本語](../ja-JP/development-guide.md) | [한국어](../ko-KR/development-guide.md) | [Čeština](../cs-CZ/development-guide.md)

## Resumen de Arquitectura

SiliconLifeCollective sigue la **arquitectura cuerpo-cerebro**, con estricta separación entre interfaces centrales e implementaciones predeterminadas.

### Estructura del Proyecto

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/      # Interfaces, clases abstractas, infraestructura común
│   └── SiliconLife.Default/   # Implementaciones concretas, puntos de entrada
└── docs/                      # Documentación multilingüe
```

**Dirección de dependencia**: `SiliconLife.Default` → `SiliconLife.Core` (unidireccional)

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

1. Crear nueva clase en `src/SiliconLife.Default/Tools/`:

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

1. Implementar `IAIClient` en `src/SiliconLife.Default/AI/`:

```csharp
public class MyAIClient : IAIClient
{
    public async Task<ChatResponse> ChatAsync(ChatRequest request)
    {
        // Implementar lógica de comunicación con IA
    }
}
```

2. Crear fábrica correspondiente:

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(Dictionary<string, object> config)
    {
        return new MyAIClient(config);
    }
}
```

3. Registrar fábrica en la configuración

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

1. Implementar `ISkin` en `src/SiliconLife.Default/Web/Skins/`:

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
- **Métodos**: PascalCase (ej. `ExecuteAsync`, `GetConfig`)
- **Propiedades**: PascalCase (ej. `Name`, `Description`)
- **Parámetros**: camelCase (ej. `config`, `requestId`)

### Estructura de Archivos

```
NombreDeClase.cs
├── Licencia (encabezado Apache 2.0)
├── Usings
├── Namespace
│   └── Clase
│       ├── Propiedades
│       ├── Constructor
│       └── Métodos
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

## Rendimiento

### Optimizaciones

1. **Caché de Frecuencia de Usuario**: Reduce verificaciones de permisos repetitivas
2. **Indexación por Tiempo**: Consultas eficientes por rango de tiempo
3. **Hilos de Ejecutor Independientes**: Aislamiento y paralelismo

### Monitoreo

- Monitorear tiempos de ejecución de reloj
- Rastrear consumo de tokens
- Verificar colas de ejecutor

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
