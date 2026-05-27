# Demo de System.IO seguro

Demuestra los tipos System.IO en la lista blanca `SystemIOAllowedTypes`: `MemoryStream`, `BinaryReader`/`BinaryWriter`, `GZipStream`. Explica por qué `FileStream` requiere `PermissionedStreamFactory`.

## Lista blanca SystemIOAllowedTypes

El runtime del plugin bloquea por defecto el espacio de nombres `System.IO`, pero exime los tipos que **no realizan E/S de archivos directamente**:

| Categoría | Tipos permitidos | Por qué es seguro |
|-----------|-----------------|------------------|
| Abstracciones de flujo | `Stream` | Clase base abstracta, sin E/S propia |
| Flujos en memoria | `MemoryStream` | Operación puramente en memoria |
| Flujos de compresión | `GZipStream`, `DeflateStream`, `ZLibStream` | Envuelven otro flujo, no abren archivos |
| Envoltorios binarios | `BinaryReader`, `BinaryWriter` | Envuelven cualquier flujo, no abren archivos |
| Enumeraciones | `SeekOrigin`, `FileMode`, `FileAccess`, `FileShare`, `CompressionMode`, `CompressionLevel` | Solo tipos de valor |
| Excepciones | `IOException`, `InvalidDataException`, `EndOfStreamException` | Solo tipos de error |

### Tipos no incluidos en la lista blanca

Estos tipos **acceden directamente al sistema de archivos** y están **bloqueados** en el código del plugin:

| Tipo bloqueado | Razón | Alternativa segura |
|---------------|-------|-------------------|
| `FileStream` | Abre archivos directamente | `PermissionedStreamFactory.CreateReadStream()` / `CreateWriteStream()` |
| `File` | Operaciones de archivo estáticas | `PermissionedStreamFactory` + `SafePath` |
| `Directory` | Operaciones de directorio estáticas | `SafePath` (verificación de permisos) |
| `FileInfo` | Envuelve rutas de archivos | `SafePath` |
| `DirectoryInfo` | Envuelve rutas de directorios | `SafePath` |
| `StreamReader` | Abre archivos directamente | `PermissionedStreamFactory` + envolver `PermissionedStream` |
| `StreamWriter` | Abre archivos directamente | `PermissionedStreamFactory` + envolver `PermissionedStream` |

## Por qué PermissionedStreamFactory para FileStream

`FileStream` abre archivos directamente en disco — un riesgo de seguridad importante en un sistema de plugins. `PermissionedStreamFactory` impone:

1. **Verificación de permisos** — el `PermissionManager` del llamante debe otorgar `FileAccess` para la ruta
2. **Registro de auditoría** — cada apertura de archivo se registra con el ID being del llamante
3. **Validación de ruta** — las rutas vacías/inválidas se rechazan antes de cualquier E/S

```
❌ new FileStream("path", FileMode.Open)           → Bloqueado por el escáner TypeRef
✅ PermissionedStreamFactory.CreateReadStream(id, "path")  → Verificación de permisos superada
✅ PermissionedStreamFactory.CreateWriteStream(id, "path") → Verificación de permisos superada
```

## Pipeline de demostración

Esta demo construye un pipeline de datos completo en memoria usando solo tipos de la lista blanca:

```
┌─────────────────────────────────────────────────────────────────┐
│  Demo 1: MemoryStream                                           │
│  └─ Escribir bytes → Leer bytes → Decodificar cadena            │
│                                                                  │
│  Demo 2: Pipeline de compresión                                  │
│  └─ string → UTF8 → MemoryStream                                │
│     → GZipStream(comprimir) → MemoryStream(comprimido)           │
│     → GZipStream(descomprimir) → MemoryStream(sin procesar)      │
│     → UTF8 → string (ida y vuelta)                               │
│                                                                  │
│  Demo 3: BinaryReader/Writer                                     │
│  └─ Write(int, double, string) → MemoryStream                   │
│     → Read(int, double, string) → Verificar ida y vuelta        │
└─────────────────────────────────────────────────────────────────┘
```

## Esta demo

> **⚠️ Nota:** Esta demo usa **solo** tipos de la lista blanca `SystemIOAllowedTypes`. No se realiza ninguna E/S de archivos. Para acceso a archivos, ver la API `PermissionedStreamFactory`.

| Clase | Rol |
|-------|-----|
| `SafeSystemIOPlugin` | Implementación `IPlugin` — demuestra el uso seguro de System.IO |

## Nota de seguridad

El espacio de nombres `System.IO` está bloqueado por el escáner TypeRef del plugin. Solo los tipos de la lista blanca pasan. Para acceso real a archivos, se debe usar `PermissionedStreamFactory`, que realiza verificaciones de permisos y registro de auditoría. Consulte la [documentación de seguridad](../../docs/es-ES/security.md).
