# Operaciones de E/S de archivo prohibidas — Antipatrón

Demuestra operaciones de E/S de archivo **prohibidas** en el sistema de plugins. Este ejemplo sirve como referencia de antipatrón, mostrando qué NO hacer y proporcionando alternativas correctas para cada violación.

## ¿Por qué System.IO está prohibido globalmente?

Todo el namespace `System.IO` está bloqueado a nivel de plugin porque el acceso directo a archivos presenta graves riesgos de seguridad:

1. **Acceso no autorizado a archivos**: Los plugins podrían leer archivos sensibles fuera del workspace (contraseñas, claves, datos personales)
2. **Ataques de sobrescritura**: Plugins maliciosos podrían sobrescribir archivos críticos del sistema o configuración
3. **Recorrido de directorios**: Los plugins podrían usar rutas `../` para escapar de los límites del workspace
4. **Agotamiento de recursos**: La creación descontrolada de archivos podría llenar el espacio en disco
5. **Sin rastro de auditoría**: Las operaciones directas de archivo evitan el sistema de auditoría de seguridad de plugins

## Tipos prohibidos

Todos los tipos `System.IO` que acceden directamente al sistema de archivos están bloqueados:

| Tipo prohibido | Método bloqueado | Nivel de riesgo |
|----------------|-----------------|-----------------|
| `File` | `ReadAllText`, `WriteAllText`, `AppendAllText` etc. | 🔴 Crítico |
| `FileStream` | Constructor con ruta de archivo | 🔴 Crítico |
| `Directory` | `GetFiles`, `GetDirectories`, `CreateDirectory` | 🔴 Crítico |
| `StreamReader` | Constructor con ruta (cadena) | 🔴 Crítico |
| `StreamWriter` | Constructor con ruta (cadena) | 🔴 Crítico |
| `FileInfo` | Todos los métodos | 🔴 Crítico |
| `DirectoryInfo` | Todos los métodos | 🔴 Crítico |

## Tipos permitidos (excepciones de lista blanca)

Los tipos que realizan **operaciones puramente en memoria** (sin acceso directo al sistema de archivos) están permitidos:

| Tipo permitido | Uso | Por qué es seguro |
|---------------|-----|-------------------|
| `MemoryStream` | Flujo de bytes en memoria | Sin acceso al sistema de archivos |
| `BinaryReader` | Lectura de flujo existente | Envuelve flujo, no abre archivos |
| `BinaryWriter` | Escritura en flujo existente | Envuelve flujo, no crea archivos |
| `GZipStream` | Compresión/descompresión | Envuelve flujo, sin acceso a archivos |
| `StreamReader` | Constructor con parámetro `Stream` | Seguro al envolver flujos auditados |
| `StreamWriter` | Constructor con parámetro `Stream` | Seguro al envolver flujos auditados |

Consulte el ejemplo **04-SafeSystemIO** para tipos permitidos.

## Acceso seguro a archivos mediante PermissionedStreamFactory

`PermissionedStreamFactory` es el **punto de entrada controlado** para operaciones de archivo en plugins:

```csharp
// ✅ Correcto: leer archivo
using var readStream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(readStream);
string content = reader.ReadToEnd();

// ✅ Correcto: escribir archivo
using var writeStream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(writeStream);
writer.Write("Datos de registro");
```

**PermissionedStreamFactory proporciona:**
1. **Validación de ruta**: Previene ataques de recorrido de directorios (`../`)
2. **Verificación de permisos**: Asegura que el archivo está dentro del workspace permitido
3. **Registro de auditoría**: Todo acceso a archivos se registra para revisión de seguridad
4. **Limpieza de recursos**: Rastrea flujos abiertos y previene fugas

## Violaciones en este ejemplo

### Violación 1: File.ReadAllText

```csharp
// ❌ Prohibido — ⚠️ VIOLATION: [TypeRef] System.IO.File::ReadAllText
string content = File.ReadAllText("config.json");

// ✅ Alternativa correcta
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
string content = reader.ReadToEnd();
```

### Violación 2: File.WriteAllText

```csharp
// ❌ Prohibido — ⚠️ VIOLATION: [TypeRef] System.IO.File::WriteAllText
File.WriteAllText("output.log", "some data");

// ✅ Alternativa correcta
using var stream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(stream);
writer.Write("some data");
```

### Violación 3: FileStream directo

```csharp
// ❌ Prohibido — ⚠️ VIOLATION: [TypeRef] System.IO.FileStream::.ctor
using var fs = new FileStream("data.bin", FileMode.Open);

// ✅ Alternativa correcta
using var fs = PermissionedStreamFactory.OpenRead("data.bin");
```

### Violación 4: Directory.GetFiles

```csharp
// ❌ Prohibido — ⚠️ VIOLATION: [TypeRef] System.IO.Directory::GetFiles
string[] files = Directory.GetFiles("./logs", "*.txt");

// ✅ Alternativa correcta (usando SpeedyPack)
using var pack = SpeedyPack.Open("logs.spk");
var entries = pack.ListEntries("/");
```

### Violación 5: StreamReader con ruta directa

```csharp
// ❌ Prohibido — ⚠️ VIOLATION: [TypeRef] System.IO.StreamReader::.ctor(string)
using var reader = new StreamReader("config.json");

// ✅ Alternativa correcta
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
```

## Comparación con otros ejemplos

| Ejemplo | Enfoque | Permiso requerido |
|---------|---------|-------------------|
| **04-SafeSystemIO** | Tipos de memoria permitidos (MemoryStream, GZipStream) | Ninguno |
| **07-ForbiddenFileIO** | Patrones de acceso a archivos prohibidos (este ejemplo) | No aplica (bloqueado) |
| **14-CapabilityFileIO** | Declarar capacidad FileIO para eludir restricciones | `Capability.FileIO` |

## Mecanismo de escaneo de seguridad del PluginLoader

Cuando PluginLoader escanea este plugin:

1. **Escaneo TypeRef**: Detecta referencias a tipos `System.IO` prohibidos
2. **Escaneo MemberRef**: Detecta llamadas a métodos bloqueados
3. **Escaneo de cadenas IL**: Detecta intentos de elusión por reflexión basada en cadenas
4. **Rechazo**: El plugin es rechazado durante la carga con un mensaje de error detallado

La elusión mediante concatenación de cadenas, reflexión, carga dinámica u ofuscación es imposible — se captura por escaneo a nivel IL (ver **12-ForbiddenStringBypass**).

## Nota de seguridad

Si realmente necesita acceso irrestricto a archivos, puede declarar `Capability.FileIO` (ver 14-CapabilityFileIO). Sin embargo, las mejores prácticas son:
- Preferir **SpeedyPack** para almacenamiento de datos estructurados (no requiere declaración de permisos)
- Usar **PermissionedStreamFactory** cuando se necesite acceso a archivos (punto de entrada controlado)
- Declarar `Capability.FileIO` solo si las soluciones anteriores no son suficientes
