# Demo del Modo de Carga por Compilación de Código Fuente CS

Un plugin cargado desde archivos fuente `.cs` sin procesar en lugar de una DLL precompilada, demostrando el modo de compilación de código fuente CS de PluginLoader (introducido por task-389).

## Cómo Funciona el Modo de Código Fuente CS

Cuando PluginLoader escanea un directorio de plugins y **no encuentra DLL**, entra automáticamente en el modo de código fuente CS:

```
1. PluginLoader escanea el directorio → sin DLL
2. Entra en el modo de código fuente CS
3. cs.txt encontrado → lee línea por línea, solo carga los archivos .cs listados
   (Sin cs.txt → carga todos los archivos *.cs del directorio)
4. Escanea DLLs hermanas → las DLLs de confianza se añaden directamente como referencias;
   las DLLs no confiables deben pasar ScanForbiddenReferences
5. CompilationCore (modo restringido) compila archivos .cs en DLL en memoria
6. Los bytes de la DLL en memoria se escriben en un archivo temporal para el escaneo ScanForbiddenReferences
7. Escaneo superado → reflexión encuentra la implementación IPlugin → instanciación
8. Registro muestra: "Plugin loaded [CS-Source]: {Id} v{Version} from {DirName}"
```

## cs.txt — Lista Blanca de Carga Selectiva

El archivo `cs.txt` especifica qué archivos `.cs` compilar, un nombre de archivo por línea:

```
Plugin.cs
```

- **Archivos listados**: Compilados y cargados (ej: `Plugin.cs`)
- **Archivos no listados**: Ignorados por el compilador (ej: `Helpers.cs`)
- **Líneas que comienzan con `#`**: Tratadas como comentarios
- **Líneas vacías**: Ignoradas
- **Sin cs.txt**: Todos los archivos `*.cs` del directorio se cargan

## Modo Código Fuente CS vs Modo DLL

| Aspecto | Modo DLL | Modo Código Fuente CS |
|---------|----------|----------------------|
| Formato del plugin | DLL precompilada `.dll` | Archivos fuente `.cs` sin procesar |
| Disparador de carga | DLL encontrada en el directorio | Sin DLL, archivos `.cs` presentes |
| Compilación | En tiempo de compilación | En tiempo de carga por PluginLoader |
| Rendimiento | Sin sobrecarga de compilación | Sobrecarga de compilación Roslyn al inicio |
| Escaneo de seguridad | Escaneo directo de metadatos PE | Compilación → DLL temporal → Escaneo metadatos PE |
| Prefijo de registro | `Plugin loaded:` | `Plugin loaded [CS-Source]:` |
| Mejor para | Despliegue en producción | Iteración de desarrollo |

## Manejo de Errores

| Escenario | Comportamiento |
|-----------|---------------|
| Sin DLL, sin archivos .cs | Advertencia: "No DLL and no CS source files found" |
| Errores de compilación | Error: Mensajes de diagnóstico detallados registrados |
| Fallo del escaneo de seguridad | Error: Todas las violaciones listadas, plugin rechazado |
| Entrada cs.txt no encontrada | Advertencia: "cs.txt entry not found or not a .cs file" |
| Fallo del escaneo de DLL hermana | Advertencia: DLL no añadida como referencia, compilación continúa |

## Nota de Seguridad

Los plugins en modo de código fuente CS pasan por el **mismo escaneo de seguridad** que los plugins en modo DLL. El ensamblado compilado se escribe en un archivo DLL temporal y se escanea con `ScanForbiddenReferences` — el mismo escaneo que reciben las DLLs precompiladas. Todas las reglas de espacios de nombres/tipos/miembros/cadenas prohibidos se aplican de manera idéntica.

Los plugins siguen cargándose en un contexto aislado y se escanean para referencias de espacios de nombres prohibidos (ej: `System.IO`, `System.Net.Http`). Consulte la [Documentación de Seguridad](../../docs/es-ES/security.md) para más detalles.
