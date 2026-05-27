# PluginDemo-11: Anti-patrón de P/Invoke y código unsafe prohibidos

## Descripción general

Este plugin demuestra operaciones de P/Invoke y código unsafe **prohibidas** en el sistema de plugins de SiliconLife. A diferencia de otras categorías prohibidas (E/S de archivos, red, procesos, reflexión) que tienen alternativas seguras, P/Invoke y el código unsafe son **prohibiciones absolutas** — sin alternativa segura y no exencionables por ninguna declaración `PluginCapability`.

## ¿Por qué P/Invoke es la amenaza definitiva?

P/Invoke y el código unsafe representan la **amenaza más fundamental** porque operan **completamente fuera del runtime gestionado**:

- El código nativo se ejecuta con privilegios completos del proceso
- Sin seguridad de tipos gestionada, seguridad de memoria ni recolección de basura
- Imposible interceptar, auditar o aislar las llamadas nativas
- Fallo del código nativo = fallo de todo el proceso (sin manejo de excepciones)
- Acceso posible a cualquier dirección de memoria del espacio del proceso

## Mecanismo de triple seguro

PluginLoader utiliza **tres capas de detección independientes**:

### Capa 1: Escaneo de tabla TypeRef

Detecta referencias directas a tipos prohibidos en metadatos PE:

| Tipo prohibido | Espacio de nombres | Amenaza |
|----------------|-------------------|---------|
| `DllImportAttribute` | System.Runtime.InteropServices | Declara importación de función nativa |
| `Marshal` | System.Runtime.InteropServices | Puente de memoria gestionada/no gestionada |
| `NativeMemory` | System.Runtime.InteropServices | Malloc/free del heap nativo |
| `NativeLibrary` | System.Runtime.InteropServices | Carga dinámica de bibliotecas nativas |
| `GCHandle` | System.Runtime.InteropServices | Fijar objeto gestionado, exponer puntero |
| `Unsafe` | System.Runtime.CompilerServices | Clase auxiliar Unsafe |
| `UnverifiableCodeAttribute` | System.Security | Marcador de código no verificable |

### Capa 2: Escaneo de marcadores Unsafe (ScanUnsafeMarkers)

| Marcador | Método de detección | Fuente |
|----------|-------------------|--------|
| `[assembly: UnverifiableCode]` | Tabla CustomAttribute del ensamblado | Palabra clave C# `unsafe` |
| `[module: UnverifiableCode]` | Tabla CustomAttribute del módulo | Palabra clave C# `unsafe` |
| `MethodAttributes.PinvokeImpl` | Flag de tabla MethodDef | Atributo `[DllImport]` |

### Capa 3: Escaneo de cadenas IL (heap #US)

```
"System.Runtime.InteropServices.Marshal"  → Marcado
"System.Runtime.InteropServices.*"        → Marcado por coincidencia de prefijo
```

## Violaciones demostradas

### Violación 1: Declaración [DllImport]

```csharp
// ❌ PROHIBIDO
[DllImport("kernel32.dll")]
private static extern ulong GetTickCount64();
```

### Violación 2: Uso de Marshal

```csharp
// ❌ PROHIBIDO
IntPtr ptr = Marshal.AllocHGlobal(1024);
string? str = Marshal.PtrToStringAnsi(ptr);
Marshal.FreeHGlobal(ptr);
```

### Violación 3: Uso de NativeMemory

```csharp
// ❌ PROHIBIDO
unsafe
{
    void* buffer = NativeMemory.Alloc(4096);
    NativeMemory.Free(buffer);
}
```

### Violación 4: Fijación con GCHandle

```csharp
// ❌ PROHIBIDO
GCHandle handle = GCHandle.Alloc(managedArray, GCHandleType.Pinned);
IntPtr ptr = handle.AddrOfPinnedObject();
handle.Free();
```

### Violación 5: Bloque unsafe

```csharp
// ❌ PROHIBIDO
unsafe
{
    int* ptr = &value;
    *ptr = 100;
    byte* stack = stackalloc byte[256];
}
```

### Violación 6: Carga de NativeLibrary

```csharp
// ❌ PROHIBIDO
IntPtr lib = NativeLibrary.Load("evil.dll");
IntPtr funcPtr = NativeLibrary.GetExport(lib, "malicious_function");
NativeLibrary.Free(lib);
```

## Sin alternativa segura — Comparación

| Categoría prohibida | Wrapper seguro | Auditable | Declarable vía PluginCapability |
|--------------------|---------------|-----------|--------------------------------|
| E/S de archivos | PermissionedStreamFactory | ✅ Sí | ✅ Capability.FileIO |
| Red | NetworkExecutor | ✅ Sí | ✅ Capability.Network |
| Proceso | CommandLineExecutor | ✅ Sí | ✅ Capability.Process |
| Reflexión | ITypeRegistry + IObjectFactory | ✅ Sí | ❌ Siempre prohibido |
| **P/Invoke y unsafe** | **❌ Ninguno** | **❌ Imposible** | **❌ Siempre prohibido** |

## Si un plugin realmente necesita código nativo

1. **Auditoría manual por el mantenedor del proyecto**
2. **Agregar a la lista blanca `TrustedAssemblies`** en PluginLoader
3. **Identificación por `AssemblyDefinition.Name` de metadatos PE** (no nombre de archivo)

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

- **04-SafeSystemIO**: Tipos seguros de la lista blanca System.IO
- **06-TrustedDependency**: Mecanismo de lista blanca TrustedAssemblies
- **10-ForbiddenReflection**: Operaciones de reflexión prohibidas
- **12-ForbiddenStringBypass**: Intentos de evasión por cadenas de reflexión
