# PluginDemo-20: SpeedyPack — Almacenamiento de datos estructurados

## Resumen

Este plugin demuestra el uso de `SpeedyPack` para almacenamiento de datos estructurados **sin ninguna declaración de capacidad**. SpeedyPack es la forma **recomendada** para que los plugins persistan datos.

## ¿Por qué SpeedyPack?

| Característica | SpeedyPack | PermissionedStreamFactory | Capability.FileIO + System.IO |
|---------------|-----------|--------------------------|------------------------------|
| Capacidad necesaria | **Ninguna** | Ninguna | `Capability.FileIO` |
| Caché | ✅ Integrado | ❌ | ❌ |
| WAL (recuperación de fallos) | ✅ | ❌ | ❌ |
| Transacciones | ✅ `IPackTransaction` | ❌ | ❌ |
| Seguro para hilos | ✅ | ❌ | ❌ |
| Serialización estructurada | ✅ `Read<T>` | ❌ Bytes crudos | ❌ Manual |
| Pista de auditoría | ✅ Automática | ✅ Automática | ❌ Manual |

## CRUD básico

```csharp
// Abrir un archivo de datos SpeedyPack
using var pack = SpeedyPack.Open("mydata.spk");

// Escribir pares clave-valor
pack.Write("user:name", "Alice");
pack.Write("user:age", 30);

// Leer valores (tipados)
string name = pack.Read<string>("user:name");  // "Alice"
int age = pack.Read<int>("user:age");           // 30

// Eliminar una clave
pack.Delete("user:age");

// Verificar existencia
bool exists = pack.Contains("user:name");  // true
```

## Acceso tipado con objetos estructurados

```csharp
public class UserProfile
{
    public string Name { get; set; }
    public int Level { get; set; }
    public string[] Tags { get; set; }
}

// Escribir objeto estructurado
var profile = new UserProfile { Name = "Bob", Level = 42, Tags = new[] { "admin" } };
pack.Write("profile:bob", profile);

// Leer objeto tipado
var loaded = pack.Read<UserProfile>("profile:bob");
Console.WriteLine($"{loaded.Name}, Level {loaded.Level}");
```

## Transacciones

```csharp
using (var tx = pack.BeginTransaction())
{
    try
    {
        tx.Write("account:a", 1000);
        tx.Write("account:b", 500);
        tx.Commit();   // Atómico — ambas escrituras se persisten o ninguna
    }
    catch
    {
        tx.Rollback();  // Descartar todas las escrituras de esta transacción
    }
}
```

### Métodos IPackTransaction

| Método | Descripción |
|--------|-------------|
| `Write(key, value)` | Encolar una operación de escritura |
| `Delete(key)` | Encolar una operación de eliminación |
| `Commit()` | Aplicar atómicamente todas las operaciones encoladas |
| `Rollback()` | Descartar todas las operaciones encoladas |

## Configuración con SpeedyPackOptions

```csharp
var options = new SpeedyPackOptions
{
    MaxCacheSize = 1024 * 1024,              // 1 MB de caché
    AutoFlushInterval = TimeSpan.FromSeconds(30),
    CompressionLevel = CompressionLevel.Optimal
};
using var pack = SpeedyPack.Open("data.spk", options);
```

### Propiedades SpeedyPackOptions

| Propiedad | Tipo | Predeterminado | Descripción |
|----------|------|-------------|-------------|
| `MaxCacheSize` | `long` | 64 MB | Tamaño máximo de caché en memoria |
| `AutoFlushInterval` | `TimeSpan` | 10 segundos | Intervalo para vaciar caché al disco |
| `CompressionLevel` | `CompressionLevel` | `Fastest` | Nivel de compresión para datos almacenados |

## Nota de seguridad

SpeedyPack **no requiere** declaración de capacidad. Es un punto de entrada de almacenamiento de datos seguro y controlado que:
- Valida todas las rutas contra los límites del espacio de trabajo
- Proporciona pista de auditoría completa de todas las operaciones de lectura/escritura
- Previene ataques de recorrido de directorios
- Gestiona automáticamente el ciclo de vida de los recursos

## Archivos

- `Plugin.cs` — Plugin de demostración SpeedyPack
- `README.md` — Este archivo (Inglés)
- `README.zh-CN.md` — Chino simplificado
- Traducciones: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Ejemplos relacionados

- **04-SafeSystemIO**: Tipos System.IO en memoria permitidos (sin declaración necesaria)
- **07-ForbiddenFileIO**: Anti-patrón de operaciones de archivo bloqueadas
- **14-CapabilityFileIO**: Cuando SpeedyPack no es suficiente
