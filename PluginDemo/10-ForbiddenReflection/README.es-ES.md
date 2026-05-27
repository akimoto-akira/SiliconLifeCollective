# PluginDemo-10: Anti-patrón de reflexión prohibida

## Descripción general

Este plugin demuestra operaciones de reflexión **prohibidas** en el sistema de plugins de SiliconLife. Sirve como referencia de anti-patrón, mostrando lo que NO se debe hacer y proporcionando las alternativas correctas para cada violación.

## ¿Por qué la reflexión es la amenaza principal?

La elusión por reflexión es la **amenaza más crítica** para el escaneo de seguridad del PluginLoader. Mientras el escaneo TypeRef captura referencias de tipos directas en tiempo de compilación, los métodos de reflexión pueden resolver tipos en **tiempo de ejecución** usando cadenas — completamente invisibles al escaneo estático de metadatos.

Si un plugin puede llamar a `Type.GetType("System.IO.File, System.Runtime")`, puede acceder a CUALQUIER tipo prohibido sin que aparezca ninguna referencia en la tabla TypeRef de los metadatos PE.

## ¿Qué métodos están prohibidos?

Todos los métodos prohibidos se detectan mediante **escaneo MemberRef** (no bloqueo a nivel de namespace o tipo):

| Método prohibido | Firma | Amenaza |
|-----------------|-------|---------|
| `Type.GetType` | `System.Type::GetType(System.String)` | Resolver tipo arbitrario por nombre en tiempo de ejecución |
| `Activator.CreateInstance` | `System.Activator::CreateInstance(...)` | Instanciar tipos arbitrarios |
| `Assembly.Load` | `System.Reflection.Assembly::Load(...)` | Cargar ensamblado por nombre/bytes |
| `Assembly.LoadFile` | `System.Reflection.Assembly::LoadFile(...)` | Cargar ensamblado desde disco |
| `Assembly.LoadFrom` | `System.Reflection.Assembly::LoadFrom(...)` | Cargar ensamblado desde ruta |
| `Assembly.GetType` | `System.Reflection.Assembly::GetType(System.String)` | Resolución de tipo basada en cadenas |

## ¿Qué es seguro?

No toda la reflexión está prohibida. Los siguientes patrones son **seguros** porque referencian tipos conocidos en tiempo de compilación:

| Patrón seguro | Ejemplo | Por qué es seguro |
|---------------|---------|-------------------|
| `typeof(X).Assembly` | `typeof(MyPlugin).Assembly` | Tipo conocido en compilación, visible en TypeRef |
| `typeof(X).GetProperties()` | `typeof(MyData).GetProperties()` | Inspección de tipo conocido, sin nuevos tipos |
| Restricciones genéricas | `FindSubtypesOf(typeof(BaseTool))` | Parámetro genérico es tipo de compilación |
| `nameof()` | `nameof(MyClass.MyMethod)` | Cadena de compilación, sin resolución en ejecución |

**Distinción clave:**
- `typeof(X).Assembly` → **Seguro** (referencia de compilación, escaneado por PluginLoader)
- `Assembly.Load("X")` → **Prohibido** (cadena de ejecución, elude todos los escaneos)

## ¿Cómo reemplazar la reflexión de forma segura?

### Usar ITypeRegistry (Reemplaza Type.GetType + escaneo AppDomain)

```csharp
// ❌ PROHIBIDO: Resolver tipo por cadena en tiempo de ejecución
Type? type = Type.GetType("MyNamespace.MyClass, MyAssembly");

// ✅ CORRECTO: Usar ITypeRegistry para buscar tipos registrados
Type? type = typeRegistry.FindType("MyNamespace.MyClass");
// Solo los tipos registrados durante OnLoad son descubribles
```

### Usar IObjectFactory (Reemplaza Activator.CreateInstance)

```csharp
// ❌ PROHIBIDO: Crear instancia arbitraria
object? instance = Activator.CreateInstance(someType);

// ✅ CORRECTO: Usar IObjectFactory con factory registrada
var instance = objectFactory.CreateInstance<MyService>();
// Solo los tipos con factories registradas pueden ser instanciados
```

## Violaciones demostradas

### Violación 1: Type.GetType(string)

```csharp
// ❌ PROHIBIDO
Type? fileType = Type.GetType("System.IO.File, System.Runtime");

// ✅ CORRECTO
Type? myType = typeRegistry.FindType("MyPlugin.MyCustomType");
```

**MemberRef bloqueada**: `System.Type::GetType(System.String)`

### Violación 2: Activator.CreateInstance

```csharp
// ❌ PROHIBIDO
object? client = Activator.CreateInstance(httpClientType!);

// ✅ CORRECTO
var instance = objectFactory.CreateInstance<MyService>();
```

**MemberRef bloqueada**: `System.Activator::CreateInstance`

### Violación 3: Assembly.Load

```csharp
// ❌ PROHIBIDO
Assembly asm = Assembly.Load("System.Net.Http");

// ✅ CORRECTO
Assembly myAsm = typeof(MyPlugin).Assembly;  // Seguro: conocido en compilación
```

**MemberRef bloqueada**: `System.Reflection.Assembly::Load(System.String)`

### Violación 4: Assembly.LoadFile / LoadFrom

```csharp
// ❌ PROHIBIDO
Assembly asm = Assembly.LoadFile(@"C:\malware\evil.dll");

// ✅ CORRECTO
// Todas las dependencias deben estar en el directorio del plugin y ser escaneadas por PluginLoader.
```

**MemberRef bloqueada**: `System.Reflection.Assembly::LoadFile(System.String)`

### Violación 5: Assembly.GetType(string)

```csharp
// ❌ PROHIBIDO
Type? processType = runtime.GetType("System.Diagnostics.Process");

// ✅ CORRECTO
Type? safeType = typeRegistry.FindType("MyPlugin.MySafeType");
```

**MemberRef bloqueada**: `System.Reflection.Assembly::GetType(System.String)`

## Por qué typeof(X).Assembly es seguro y Assembly.Load no

| Operación | Visibilidad | Seguridad |
|-----------|------------|-----------|
| `typeof(X).Assembly` | Tipo X en tabla TypeRef → PluginLoader lo escanea | ✅ Seguro |
| `Assembly.Load("X")` | Cadena "X" solo existe en ejecución → invisible al escaneo TypeRef | ❌ Prohibido |
| `obj.GetType()` | Devuelve tipo de instancia existente → no introduce nuevo tipo | ✅ Seguro |
| `Type.GetType("X")` | Resuelve tipo arbitrario desde cadena → elude TypeRef | ❌ Prohibido |

## Mejores prácticas

1. **Registrar tipos en OnLoad**: Usar `ITypeRegistry.RegisterType` / `RegisterFromAssembly`
2. **Usar IObjectFactory para creación dinámica**: Nunca usar `Activator.CreateInstance`
3. **Usar typeof(X).Assembly**: Acceso seguro a su propio ensamblado
4. **Evitar nombres de tipo basados en cadenas**: Activa el escaneo de cadenas IL
5. **Diseñar para descubribilidad estática**: No visible en metadatos = sospechoso

## Archivos

- `Plugin.cs` - Plugin de demostración anti-patrón
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Este archivo (Español)
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## Ejemplos relacionados

- **02-TypeRegistryUsage**: Uso correcto de ITypeRegistry
- **03-ObjectFactoryUsage**: Uso correcto de IObjectFactory
- **11-ForbiddenPInvoke**: P/Invoke y código unsafe prohibidos
- **12-ForbiddenStringBypass**: Intentos de elusión por reflexión mediante cadenas
