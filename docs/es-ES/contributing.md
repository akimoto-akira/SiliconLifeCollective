# Contribuir

[English](contributing.md) | [简体中文](docs/zh-CN/contributing.md) | [繁體中文](docs/zh-HK/contributing.md) | [Español](docs/es-ES/contributing.md) | [日本語](docs/ja-JP/contributing.md) | [한국어](docs/ko-KR/contributing.md) | [Čeština](docs/cs-CZ/contributing.md)

## ¡Bienvenido!

Gracias por tu interés en contribuir a Silicon Life Collective. Todas las contribuciones son bienvenidas.

---

## Cómo Contribuir

### 1. Reportar Bugs

Usar GitHub Issues para reportar bugs:

- Descripción clara del problema
- Pasos para reproducir
- Comportamiento esperado vs actual
- Logs si es posible

### 2. Sugerir Mejoras

Abrir un Issue con etiqueta "enhancement":

- Descripción de la mejora
- Casos de uso
- Beneficios

### 3. Enviar Código

#### Fork y Clonar

```bash
git clone https://github.com/tu-usuario/SiliconLifeCollective.git
cd SiliconLifeCollective
```

#### Crear Rama

```bash
git checkout -b feature/mi-caracteristica
```

#### Hacer Cambios

Seguir convenciones de código del proyecto.

#### Commits Atómicos

```bash
git add archivos-modificados
git commit -m "feat(modulo): descripción del cambio"
```

#### Push y Pull Request

```bash
git push origin feature/mi-caracteristica
```

Abrir Pull Request en GitHub.

---

## Convenciones de Commits

Seguir [Conventional Commits](https://www.conventionalcommits.org/):

### Tipos

- `feat`: Nueva funcionalidad
- `fix`: Corrección de bug
- `docs`: Documentación
- `style`: Formato de código
- `refactor`: Refactorización
- `test`: Pruebas
- `chore`: Mantenimiento

### Ejemplos

```bash
feat(localization): add Spanish localization support
fix(permission): fix null pointer in permission callback
docs: update architecture documentation
refactor(web): refactor controller structure
```

### Reglas

- **Idioma**: Usar inglés
- **Formato**: Verbo en imperativo
- **Longitud**: Máximo 50 caracteres
- **Scope**: Módulo afectado (opcional)

---

## Estándares de Código

### C#

- Seguir convenciones de Microsoft
- Usar .NET 9 características
- Nullable reference types habilitados
- Documentación XML para APIs públicas

### Nombrado

```csharp
// Clases: PascalCase
public class MyTool { }

// Métodos: PascalCase
public void ExecuteMethod() { }

// Variables: camelCase
string myVariable;

// Constantes: UPPER_SNAKE_CASE
const int MAX_VALUE = 100;

// Interfaces: Prefijo I
interface ITool { }
```

### Estructura de Archivos

- Un archivo por clase
- Namespace completo
- Orden: using, namespace, clase

### Comentarios

```csharp
/// <summary>
/// Descripción del método
/// </summary>
/// <param name="param">Parámetro</param>
/// <returns>Valor de retorno</returns>
public string MyMethod(string param)
{
    // Implementación
}
```

---

## Agregar Localización

### 1. Crear Clase

```csharp
// src/SiliconLife.Default/Localization/LangXX.cs
public class LangXX : DefaultLocalizationBase
{
    public override string LanguageCode => "lang-XX";
    public override string LanguageName => "Language Name";
    
    // Implementar todos los miembros
}
```

### 2. Agregar al Enum

```csharp
// src/SiliconLife.Core/Localization/Language.cs
public enum Language
{
    // ...
    LangXX
}
```

### 3. Traducir Documentos

Crear directorio en `docs/lang-XX/` con todos los documentos traducidos.

---

## Agregar Herramienta

### 1. Implementar ITool

```csharp
public class MyTool : ITool
{
    public string Name => "MiHerramienta";
    public string Description => "Descripción";
    
    public ToolResult Execute(string method, object parameters)
    {
        // Lógica
    }
}
```

### 2. Pruebas

```csharp
[Fact]
public void TestMyTool()
{
    var tool = new MyTool();
    var result = tool.Execute("Test", new { });
    Assert.True(result.Success);
}
```

---

## Agregar Calendario

### 1. Implementar ICalendar

```csharp
public class MyCalendar : ICalendar
{
    public string Name => "MiCalendario";
    
    public DateTime ToGregorian(int year, int month, int day) { }
    public (int, int, int) FromGregorian(DateTime date) { }
}
```

---

## Agregar Piel Web

### 1. Implementar ISkin

```csharp
public class MySkin : ISkin
{
    public string Name => "MiPiel";
    public string Description => "Descripción";
    
    public string BuildPage(ViewModelBase model) { }
    public string GetCss() { }
}
```

---

## Proceso de Review

1. **CI/CD**: Verificar que build pasa
2. **Code Review**: Mantenedores revisan código
3. **Pruebas**: Verificar funcionalidad
4. **Merge**: Integrar cambios

---

## Licencia

Al contribuir, aceptas que tu contribución está bajo la Licencia Apache 2.0.

---

## Código de Conducta

- Ser respetuoso
- Ser constructivo
- Seguir guías del proyecto
- Ayudar a otros

---

## Recursos

- [Documentación](../es-ES/README.md)
- [Guía de Desarrollo](development-guide.md)
- [Arquitectura](architecture.md)
- [Issues](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
