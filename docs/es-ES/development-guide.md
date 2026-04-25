# Guía de Desarrollo

[English](development-guide.md) | [简体中文](docs/zh-CN/development-guide.md) | [繁體中文](docs/zh-HK/development-guide.md) | [Español](docs/es-ES/development-guide.md) | [日本語](docs/ja-JP/development-guide.md) | [한국어](docs/ko-KR/development-guide.md) | [Čeština](docs/cs-CZ/development-guide.md)

## Configuración del Entorno

### Requisitos

- **.NET 9 SDK**: [Descargar](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git**: [Descargar](https://git-scm.com/)
- **Editor**: VS Code, JetBrains Rider, o Visual Studio 2022+
- **Ollama** (opcional): Para pruebas locales de IA

### Clonar Repositorio

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### Compilar

```bash
dotnet restore
dotnet build
```

### Ejecutar

```bash
dotnet run --project src/SiliconLife.Default
```

---

## Estructura del Proyecto

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/         # Interfaces y abstracciones
│   │   ├── AI/                   # IAIClient, ContextManager
│   │   ├── SiliconBeing/         # SiliconBeingBase, Managers
│   │   ├── Tools/                # ITool, ToolManager
│   │   ├── Security/             # Permisos, Audit
│   │   ├── Chat/                 # ChatSystem, Sessions
│   │   ├── Storage/              # IStorage, ITimeStorage
│   │   ├── Localization/         # LocalizationBase
│   │   └── ...
│   │
│   └── SiliconLife.Default/      # Implementaciones concretas
│       ├── AI/                   # OllamaClient, DashScopeClient
│       ├── SiliconBeing/         # DefaultSiliconBeing
│       ├── Tools/                # Herramientas integradas
│       ├── Web/                  # Interfaz web
│       ├── Localization/         # Implementaciones de idioma
│       └── ...
```

---

## Agregar Nueva Herramienta

### 1. Crear Clase de Herramienta

```csharp
// src/SiliconLife.Default/Tools/MyCustomTool.cs
using SiliconLife.Core.Tools;

namespace SiliconLife.Default.Tools
{
    public class MyCustomTool : ITool
    {
        public string Name => "MiHerramienta";
        public string Description => "Descripción de mi herramienta";
        
        public ToolResult Execute(string method, object parameters)
        {
            try
            {
                // Lógica de la herramienta
                return new ToolResult 
                { 
                    Success = true, 
                    Message = "Éxito",
                    Data = "resultado"
                };
            }
            catch (Exception ex)
            {
                return new ToolResult 
                { 
                    Success = false, 
                    Message = ex.Message 
                };
            }
        }
    }
}
```

### 2. Registro Automático

La herramienta se registra automáticamente mediante reflexión en `ToolManager`.

### 3. Herramienta Solo para Curador

```csharp
[SiliconManagerOnlyAttribute]
public class AdminTool : ITool
{
    // Solo el Curador puede usar esta herramienta
}
```

---

## Agregar Nuevo Idioma

### 1. Crear Clase de Localización

```csharp
// src/SiliconLife.Default/Localization/PtBR.cs
using SiliconLife.Core.Localization;

namespace SiliconLife.Default.Localization
{
    public class PtBR : DefaultLocalizationBase
    {
        public override string LanguageCode => "pt-BR";
        public override string LanguageName => "Português (Brasil)";
        
        // Implementar todos los miembros abstractos
        public override string WelcomeMessage => "Bem-vindo ao Silicon Life Collective!";
        // ... más traducciones
    }
}
```

### 2. Agregar al Enum Language

```csharp
// src/SiliconLife.Core/Localization/Language.cs
public enum Language
{
    // ... idiomas existentes
    PtBR  // Nuevo idioma
}
```

### 3. Herencia (Opcional)

Si el idioma es variante de otro:

```csharp
public class PtPT : PtBR
{
    public override string LanguageCode => "pt-PT";
    public override string LanguageName => "Português (Portugal)";
    
    // SoloOverride de textos diferentes
}
```

---

## Agregar Nuevo Calendario

### 1. Implementar ICalendar

```csharp
// src/SiliconLife.Default/Calendar/MyCalendar.cs
using SiliconLife.Core.Calendar;

namespace SiliconLife.Default.Calendar
{
    public class MyCalendar : ICalendar
    {
        public string Name => "MiCalendario";
        
        public DateTime ToGregorian(int year, int month, int day)
        {
            // Lógica de conversión
        }
        
        public (int year, int month, int day) FromGregorian(DateTime date)
        {
            // Lógica inversa
        }
        
        public string GetMonthName(int month)
        {
            // Nombres de meses
        }
    }
}
```

---

## Crear Nueva Piel Web

### 1. Implementar ISkin

```csharp
// src/SiliconLife.Default/Web/Skins/MySkin.cs
using SiliconLife.Default.Web;

namespace SiliconLife.Default.Web.Skins
{
    public class MySkin : ISkin
    {
        public string Name => "MiPiel";
        public string Description => "Descripción de mi piel";
        
        public string BuildPage(ViewModelBase model)
        {
            // Construir HTML
            return H.Html(
                H.Head(
                    H.Title(model.PageTitle),
                    H.Style(GetCss())
                ),
                H.Body(
                    // Contenido
                )
            );
        }
        
        public string GetCss()
        {
            return @"
                body {
                    background: #f0f0f0;
                }
            ";
        }
    }
}
```

### 2. Autodescubrimiento

La piel se descubre automáticamente mediante reflexión.

---

## Agregar Nuevo Controlador Web

### 1. Heredar Controller

```csharp
// src/SiliconLife.Default/Web/Controllers/MyController.cs
using SiliconLife.Default.Web;

namespace SiliconLife.Default.Web.Controllers
{
    public class MyController : Controller
    {
        public MyController() : base("/mi-ruta")
        {
        }
        
        public override string HandleRequest(RequestContext context)
        {
            // Manejar solicitud
            return H.Html(
                H.Body(
                    H.H1("Mi Página")
                )
            );
        }
    }
}
```

---

## Pruebas

### Ejecutar Pruebas

```bash
dotnet test
```

### Depuración

```bash
dotnet run --project src/SiliconLife.Default --debug
```

---

## Convenciones de Código

### Nombrado

- **Clases**: PascalCase (`MyCustomTool`)
- **Métodos**: PascalCase (`ExecuteMethod`)
- **Variables**: camelCase (`myVariable`)
- **Constantes**: UPPER_SNAKE_CASE (`MAX_VALUE`)
- **Interfaces**: Prefijo I (`ITool`)

### Archivos

- Un archivo por clase
- Nombrar archivo igual que clase
- Usar namespaces completos

### Comentarios

```csharp
/// <summary>
/// Descripción del método
/// </summary>
/// <param name="param">Descripción del parámetro</param>
/// <returns>Descripción del retorno</returns>
public string MyMethod(string param)
{
    // Implementación
}
```

---

## Git Workflow

### Commits Atómicos

```bash
# Hacer cambios
git add src/SiliconLife.Default/Tools/MyCustomTool.cs
git commit -m "feat(tools): add MyCustomTool for custom operations"
```

### Convención de Commits

- `feat`: Nueva funcionalidad
- `fix`: Corrección de bug
- `docs`: Documentación
- `refactor`: Refactorización
- `test`: Pruebas
- `chore`: Mantenimiento

---

## Recursos Adicionales

- [Documentación Principal](../es-ES/README.md)
- [Arquitectura](architecture.md)
- [Guía de API](api-reference.md)
- [Contribuir](contributing.md)
