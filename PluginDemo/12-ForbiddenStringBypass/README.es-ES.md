# PluginDemo-12: Anti-Patrón de Elusión por Cadenas de Reflexión Prohibidas

## Descripción

Este plugin demuestra intentos **prohibidos** de elusión basados en cadenas de reflexión en el sistema de plugins SiliconLife. Muestra por qué la concatenación, interpolación, codificación y otras técnicas de ofuscación **no pueden** eludir el escaneo del heap #US (User String) de PluginLoader — la **última línea de defensa**.

## ¿Qué es el heap #US?

En los metadatos .NET PE (Portable Executable), el **heap #US (User String)** almacena todos los operandos de literales de cadena utilizados por las instrucciones IL `ldstr`. Cada vez que escribes un literal de cadena en código C#, el compilador lo almacena en este heap.

```
Fuente C#:   string s = "System.IO.File";
    ↓ compilación
Código IL:   ldstr "System.IO.File"    ← referencia token en el heap #US
    ↓ escaneo PluginLoader
Heap #US:    [..., "System.IO.File", ...]  ← ¡DETECTADO por coincidencia de prefijo!
```

El método `ScanUserStrings()` de PluginLoader itera sobre **cada entrada** del heap #US, verificando si alguna cadena comienza con un prefijo prohibido.

## Prefijos de cadenas prohibidos

Los siguientes prefijos activan violaciones `[ILString]` cuando se encuentran en el heap #US:

| Prefijo | Categoría |
|---------|-----------|
| `System.IO.` | Tipos de sistema de archivos |
| `System.Net.Http` | Cliente HTTP |
| `System.Net.WebSockets` | WebSocket |
| `System.Net.Sockets` | Sockets crudos |
| `System.Net.Mail` | SMTP |
| `System.Net.NetworkInformation` | Sondeo de red |
| `System.Net.Security` | SslStream |
| `System.Diagnostics.Process` | Proceso/línea de comandos |
| `Microsoft.CodeAnalysis` | Compilador Roslyn |
| `System.Reflection.Emit` | Emisión IL |
| `System.Runtime.Loader` | AssemblyLoadContext |
| `System.CodeDom.Compiler` | CodeDom heredado |
| `Microsoft.Win32` | Registro de Windows |

## Violaciones demostradas

### Violación 1: Cadena directa de nombre de tipo

```csharp
// ❌ PROHIBIDO — la cadena completa está en el heap #US
Type? fileType = Type.GetType("System.IO.File, System.Runtime");
```

**Violación**: `[ILString] "System.IO.File, System.Runtime" matches forbidden prefix "System.IO."`

### Violación 2: Concatenación de cadenas (tiempo de compilación)

```csharp
// ❌ PROHIBIDO — el compilador pliega const+const en una entrada #US
const string ns = "System.Net.Http";
const string typeName = ".HttpClient";
const string assembly = ", System.Net.Http";
Type? type = Type.GetType(ns + typeName + assembly);
```

**Violación**: `[ILString] "System.Net.Http.HttpClient, System.Net.Http" matches forbidden prefix "System.Net.Http"`

### Violación 3: Interpolación de cadenas

```csharp
// ❌ PROHIBIDO — las partes literales se almacenan en el heap #US
string className = "FileStream";
string fullName = $"System.IO.{className}, System.Runtime";
```

**Violación**: `[ILString] "System.IO." matches forbidden prefix "System.IO."`

### Violación 4: Campos Const

```csharp
// ❌ PROHIBIDO — los valores const se insertan en línea → aparecen en el heap #US
private const string ProcessType = "System.Diagnostics.Process";
private const string AssemblyName = ", System.Runtime";
Type? type = Type.GetType(ProcessType + AssemblyName);
```

**Violación**: `[ILString] "System.Diagnostics.Process" matches forbidden prefix "System.Diagnostics.Process"`

### Violación 5: Fragmentos de cadenas parciales

```csharp
// ❌ PROHIBIDO — cada parte es un ldstr separado, escaneado independientemente
string part1 = "System.Reflection.Emit";
string part2 = ".AssemblyBuilder";
string fullType = part1 + part2;
```

**Violación**: `[ILString] "System.Reflection.Emit" matches forbidden prefix "System.Reflection.Emit"`

## Por qué las técnicas de ofuscación fallan

| Técnica | Por qué falla |
|---------|--------------|
| Concatenación const | El compilador pliega en una sola entrada #US |
| Interpolación de cadenas | Partes literales almacenadas en el heap #US |
| Campos const | Valores insertados en línea → aparecen en #US |
| División en variables | Cada operando `ldstr` escaneado independientemente |
| Codificación Base64 | La decodificación necesita métodos runtime, pero `Type.GetType` está bloqueado por MemberRef |
| Construcción por array de char | No genera `ldstr`, pero `Type.GetType` sigue bloqueado por MemberRef |
| Cifrado XOR | Cadena cifrada ilegible en #US, pero descifrado + `Type.GetType` = MemberRef bloqueado |

**Insight clave**: El escaneo #US bloquea la **cadena**. El escaneo MemberRef bloquea el **método**. Para cargar dinámicamente un tipo, necesitas AMBOS. PluginLoader bloquea AMBOS independientemente.

## La cadena de defensa completa

| Paso | Mecanismo | Lo que detecta |
|------|-----------|---------------|
| 1 | Tabla TypeRef | Referencias directas a tipos prohibidos |
| 2 | Tabla ExportedType | Tipos reenviados desde espacios de nombres prohibidos |
| 3 | Tabla MemberRef | Llamadas a `Type.GetType`, `Assembly.Load`, `Activator.CreateInstance` |
| 4 | Marcadores Unsafe | `[DllImport]`, bloques unsafe, flag PinvokeImpl |
| **5** | **Escaneo del heap #US** | **Constantes de cadena que coinciden con prefijos prohibidos (este demo)** |

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

- **10-ForbiddenReflection**: Métodos de reflexión prohibidos (escaneo MemberRef)
- **11-ForbiddenPInvoke**: P/Invoke y código unsafe prohibidos
- **02-TypeRegistryUsage**: Uso correcto de ITypeRegistry
- **03-ObjectFactoryUsage**: Uso correcto de IObjectFactory
