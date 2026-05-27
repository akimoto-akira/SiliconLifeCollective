# PluginDemo-08: Anti-Patrón de Operaciones de Red Prohibidas

## Descripción general

Este plugin demuestra operaciones de red **PROHIBIDAS** en el sistema de plugins SiliconLife. Sirve como referencia de anti-patrón, mostrando lo que NO se debe hacer y proporcionando alternativas correctas.

## ¿Por qué el acceso directo a la red está prohibido globalmente?

Los patrones de acceso directo a la red están bloqueados a nivel de plugin:

1. **Conexión a servidores maliciosos**: Los plugins podrían conectarse a servidores maliciosos
2. **Exfiltración de datos**: Los plugins podrían filtrar datos sensibles del sandbox
3. **Ataques DNS Rebinding**: Los plugins podrían evadir verificaciones de seguridad
4. **Omisión de ACL de red**: El acceso directo a la red omite el sistema ACL global

## Tipos prohibidos

Todos los tipos `System.Net` que acceden directamente a la red están bloqueados:

| Tipo prohibido | Espacio de nombres bloqueado | Nivel de riesgo |
|----------|----------------|----------|
| `HttpClient` | `System.Net.Http` | 🔴 Crítico |
| `TcpClient` | `System.Net.Sockets` | 🔴 Crítico |
| `Socket` | `System.Net.Sockets` | 🔴 Crítico |
| `Dns` | `System.Net` | 🔴 Crítico |
| `WebClient` | `System.Net` | 🔴 Crítico |

## Métodos de acceso seguro

### NetworkExecutor (Recomendado)

`NetworkExecutor` es el **punto de entrada controlado** para operaciones de red:

```csharp
// ✅ CORRECTO: Solicitud GET simple
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com/data"
});
```

**Lo que proporciona NetworkExecutor:**
1. Verificación de permisos
2. Registro de auditoría
3. Interruptor automático
4. Control de tiempo de espera
5. Cola de solicitudes

## Violaciones demostradas

### Violación 1: HttpClient

```csharp
// ❌ PROHIBIDO
using var client = new HttpClient();

// ✅ CORRECTO
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com"
});
```

### Violación 2: TcpClient

```csharp
// ❌ PROHIBIDO
using var client = new TcpClient("example.com", 8080);

// ✅ CORRECTO
// Usar NetworkExecutor o declarar Capability.Network
```

## Mecanismo de seguridad de PluginLoader

PluginLoader analiza este plugin y:
1. **Escaneo TypeRef**: Detecta referencias a tipos prohibidos
2. **Escaneo MemberRef**: Detecta llamadas a métodos bloqueados
3. **Escaneo de cadena IL**: Detecta intentos de reflexión
4. **Rechazo**: El plugin se rechaza al cargar

## Archivos

- `Plugin.cs` - Plugin de demostración anti-patrón
- `README.md` - Este archivo (Inglés)
- `README.es-ES.md` - Este archivo (Español)
- Otras versiones de idiomas...

## Ejemplos relacionados

- **13-CapabilityNetwork**: Capacidad de red declarativa
- **07-ForbiddenFileIO**: Patrones de acceso a archivos prohibidos