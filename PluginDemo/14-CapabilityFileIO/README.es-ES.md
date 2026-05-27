# PluginDemo-14: Capability.FileIO — Permiso declarativo de E/S de archivos

## Resumen

Este plugin demuestra el uso de `[PluginCapability(Capability.FileIO)]` para declarar acceso directo al sistema de archivos. Con esta declaración, el plugin accede a todos los tipos `System.IO` más allá de la lista blanca `SystemIOAllowedTypes`.

## Sintaxis de declaración PluginCapability

```csharp
[PluginCapability(Capability.FileIO, Reason = "Direct log file access for audit trail")]
public class CapabilityFileIOPlugin : IPlugin { ... }
```

## Funcionamiento de Capability.FileIO

1. **Estado predeterminado**: El espacio de nombres `System.IO` está prohibido globalmente; solo los tipos de la lista blanca `SystemIOAllowedTypes` están permitidos (MemoryStream, BinaryReader, GZipStream, etc.)
2. **Con declaración**: La prohibición de todo el espacio de nombres `System.IO` se levanta — File, FileStream, Directory, StreamReader(string), etc. se vuelven accesibles
3. **Exención ILString**: Las constantes de cadena que comienzan con `"System.IO."` no se marcan
4. **Límites no declarables**: P/Invoke, Unsafe, Reflection.Emit, etc. permanecen bloqueados

## Alcance de exención de Capability.FileIO

### Exenciones TypeRef

Todos los tipos `System.IO` están exentos:

| Categoría | Tipos exentos |
|-----------|--------------|
| Operaciones de archivos | `File`, `FileInfo` |
| Operaciones de directorios | `Directory`, `DirectoryInfo` |
| Tipos de flujo | `FileStream`, `StreamReader(path)`, `StreamWriter(path)` |
| Sistema de archivos | `FileSystemWatcher`, `DriveInfo`, `Path` |

### Exención ILString

- Las cadenas que comienzan con `"System.IO."` no se marcan

### Lo que sigue prohibido

| Categoría | Aún bloqueado |
|-----------|-------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` |
| Código unsafe | `UnverifiableCodeAttribute`, `Unsafe` |
| Emisión IL | `System.Reflection.Emit.*` |
| Carga de ensamblados | `System.Runtime.Loader`, `Assembly.Load*` |
| Registro | `Microsoft.Win32.*` |

## Comparación con otros ejemplos

| Ejemplo | Declaración | Acceso a archivos | Notas |
|---------|-----------|------------------|-------|
| **04-SafeSystemIO** | Ninguna | MemoryStream, BinaryReader, GZipStream | Solo usa tipos de la lista blanca |
| **07-ForbiddenFileIO** | Ninguna | ❌ RECHAZADO | Ejemplo de antipatrón |
| **14-CapabilityFileIO** | `[PluginCapability(Capability.FileIO)]` | ✅ Acceso completo a System.IO | Este ejemplo |
| **20-SpeedyPack** | Ninguna | Vía API SpeedyPack (sin Capability necesaria) | Almacenamiento de datos recomendado |

## Orden de prioridad para acceso a archivos

1. **SpeedyPack** — Sin declaración de capacidad necesaria. Caché integrado, WAL, transacciones. **Recomendado para almacenamiento de datos estructurados.**
2. **PermissionedStreamFactory** — Sin declaración necesaria. Acceso auditado con validación de ruta y control de acceso.
3. **Capability.FileIO + System.IO directo** — Solo cuando las opciones anteriores no son suficientes.

## ¿Por qué preferir PermissionedStreamFactory / SpeedyPack?

Incluso con `Capability.FileIO`, se recomienda usar puntos de entrada controlados porque:

1. **Pista de auditoría**: Todo el acceso se registra y es rastreable
2. **Validación de ruta**: Previene ataques de recorrido de directorios (`../`)
3. **Control de acceso**: Aplicación de los límites del espacio de trabajo
4. **Seguimiento de recursos**: Previene fugas de flujos y agotamiento de recursos
5. **Cumplimiento**: Los patrones de acceso controlado facilitan las revisiones de seguridad

## Mejores prácticas de seguridad

1. **Declarar FileIO solo cuando sea realmente necesario**: ¿Se puede usar SpeedyPack o PermissionedStreamFactory en su lugar?
2. **Proporcionar una Reason clara**: "Direct log file access for audit trail" es mejor que "file access"
3. **Validar rutas uno mismo**: Incluso con Capability.FileIO, validar todas las rutas de archivos antes de usarlas
4. **Usar instrucciones using**: Siempre dispose FileStream/StreamReader/StreamWriter
5. **Principio de mínimo privilegio**: Declarar solo las capacidades que el plugin realmente necesita

## Archivos

- `Plugin.cs` — Plugin de demostración que declara Capability.FileIO
- `README.md` — Este archivo (Inglés)
- `README.zh-CN.md` — Chino simplificado
- Traducciones: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Ejemplos relacionados

- **04-SafeSystemIO**: Tipos System.IO en memoria permitidos (sin declaración necesaria)
- **07-ForbiddenFileIO**: Antipatrón de operaciones de archivo bloqueadas
- **20-SpeedyPack**: Almacenamiento de datos recomendado sin declaración de capacidad
- **18-CapabilityDenied**: Antipatrón de capacidades no declarables
