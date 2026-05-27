# Demo de dependencia confiable

Demuestra el uso de `Newtonsoft.Json` — una biblioteca que internamente depende en gran medida de la reflexión — como ensamblado confiable. El escáner de seguridad de PluginLoader omite completamente los ensamblados confiables, permitiendo a los plugins referenciarlos sin activar violaciones.

## Mecanismo de lista blanca TrustedAssemblies

`PluginLoader` mantiene una lista blanca estática de bibliotecas de código abierto que son **confiables por defecto**:

```csharp
private static readonly HashSet<string> TrustedAssemblies = new(StringComparer.Ordinal)
{
    // Serialización
    "Google.Protobuf",
    "protobuf-net",
    "Newtonsoft.Json",        // ← Este demo usa esta biblioteca
    "MessagePack",
    "YamlDotNet",

    // Registro
    "Serilog", "NLog",

    // Microsoft.Extensions.*
    "Microsoft.Extensions.Logging.Abstractions",
    "Microsoft.Extensions.DependencyInjection.Abstractions",
    // ...

    // Acceso a datos / mapeo
    "Dapper", "AutoMapper",

    // Validación y distribución de mensajes
    "FluentValidation", "MediatR",
};
```

### Criterios de admisión

Una biblioteca puede añadirse a `TrustedAssemblies` si cumple **los tres** criterios:

| # | Criterio | Justificación |
|---|----------|---------------|
| 1 | Proyecto de código abierto ampliamente utilizado (MIT / Apache 2.0 / BSD) | Código auditable públicamente |
| 2 | Código fuente públicamente accesible | La supervisión comunitaria asegura la ausencia de comportamiento malicioso |
| 3 | Paquete NuGet mantenido por proveedor/comunidad de confianza | Integridad de la cadena de suministro |

### Base de identificación

El escáner identifica ensamblados confiables por su `AssemblyDefinition.Name` en metadatos PE — **no por el nombre del archivo DLL**. Esto evita que atacantes renombren una DLL maliciosa a `Newtonsoft.Json.dll` para eludir las verificaciones.

## CollectTrustedTypeRefs — Exención transitiva

Cuando PluginLoader carga un directorio de plugin, realiza un escaneo en dos fases:

```
Fase 1: CollectTrustedTypeRefs(pluginDir)
├── Enumerar todos los archivos *.dll en el directorio del plugin
├── Para cada DLL: leer metadatos PE → verificar AssemblyDefinition.Name
├── Si nombre ∈ TrustedAssemblies:
│   └── Recopilar TODAS las entradas TypeReference → pares (namespace, typeName)
└── Retorna: HashSet<(string Namespace, string Name)>

Fase 2: ScanForbiddenReferences(pluginMainDll, trustedTypeRefs)
├── Capa 0:   Salida rápida por lista blanca (si DLL principal es confiable → pasa)
├── Capa 0.5: Exención transitiva (omitir TypeRefs en conjunto trustedTypeRefs)
├── Capa 1:   Escaneo de tabla TypeRef
├── Capa 2:   Escaneo de tabla ExportedType
├── Capa 3:   Escaneo de tabla MemberRef (métodos peligrosos)
├── Capa 4:   Marcadores de código inseguro + P/Invoke
└── Capa 5:   Escaneo de heap de cadenas #US
```

### Por qué importa la exención transitiva

Newtonsoft.Json referencia internamente tipos como `System.Reflection.MemberInfo`, `System.IO.TextReader`, etc. Cuando tu plugin referencia Newtonsoft.Json, el compilador puede incrustar estos TypeRefs transitivos en **tu** DLL de plugin. Sin exención transitiva, tu plugin sería marcado por referenciar `System.IO.TextReader` — aunque nunca lo uses directamente.

`CollectTrustedTypeRefs` resuelve esto recopilando previamente todos los TypeRefs de DLLs confiables y marcándolos como "conocidos seguros" durante el escaneo principal.

## Cómo añadir una nueva dependencia confiable

Para añadir una nueva biblioteca a la lista blanca:

1. Verificar que cumple los tres criterios de admisión anteriores
2. Añadir una línea al HashSet `TrustedAssemblies` en `PluginLoader.cs`:
   ```csharp
   "YourLibraryName",  // Breve descripción de por qué es confiable
   ```
3. Colocar la DLL de la biblioteca en el directorio del plugin (junto a la DLL principal del plugin)
4. El escáner recopilará automáticamente sus TypeRefs y los eximirá

> **⚠️ Importante:** Añadir una biblioteca a `TrustedAssemblies` significa que el escáner **no** verificará su código interno. Solo añade bibliotecas en las que confíes plenamente.

## Este demo

Este plugin usa Newtonsoft.Json sin ninguna declaración `PluginCapability`:

| Función | Comportamiento interno de Newtonsoft.Json | Por qué funciona |
|---------|-------------------------------------------|-----------------|
| `JsonConvert.SerializeObject` | Usa reflexión para enumerar propiedades | DLL de Newtonsoft.Json pasa la lista blanca de capa 0 |
| `JsonConvert.DeserializeObject<T>` | Llama a `Activator.CreateInstance`, establece propiedades vía reflexión | TypeRefs transitivos eximidos en capa 0.5 |
| Manipulación de `JObject` / `JArray` | Usa `System.Linq.Expressions`, dispatch dinámico | Todas las refs internas recopiladas por `CollectTrustedTypeRefs` |

### Diferencia clave con PluginCapability

| Mecanismo | Alcance | Caso de uso |
|-----------|---------|-------------|
| `TrustedAssemblies` | Exime una **biblioteca** completa (y sus refs transitivas) del escaneo | Dependencias de código abierto conocidas |
| `PluginCapability` | Exime el **código de tu plugin** de prohibiciones de namespaces específicos | El plugin necesita acceso directo a System.Net/IO/Process |

Un plugin que solo usa dependencias confiables **no necesita** ninguna declaración `PluginCapability`. El escáner maneja todo automáticamente.

## Nota de seguridad

Los ensamblados confiables están exentos del escaneo de seguridad porque son proyectos de código abierto auditables. Sin embargo, **el código de tu plugin** sigue siendo completamente escaneado. Si tu plugin referencia directamente `System.IO.File` o `System.Net.Http.HttpClient`, seguirá siendo bloqueado — a menos que declares la `PluginCapability` correspondiente. Ver la [documentación de seguridad](../../docs/es-ES/security.md).
