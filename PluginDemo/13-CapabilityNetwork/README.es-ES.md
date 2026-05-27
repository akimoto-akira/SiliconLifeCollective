# PluginDemo-13: Capability.Network — Permiso de red declarativo

## Resumen

Este plugin demuestra el uso de `[PluginCapability(Capability.Network)]` para declarar acceso a la red. Con esta declaración, el plugin puede acceder a los tipos `System.Net.*` que de otro modo serían bloqueados por el análisis de seguridad de PluginLoader.

## Sintaxis de declaración

```csharp
[PluginCapability(Capability.Network, Reason = "Calls weather REST API to retrieve forecast data")]
public class CapabilityNetworkPlugin : IPlugin { ... }
```

**Elementos clave:**
- **Objetivo del atributo**: Debe estar en la clase que implementa directamente `IPlugin`
- **AllowMultiple = true**: Se pueden apilar múltiples atributos `[PluginCapability]` (ver 17-CapabilityStacked)
- **Campo Reason**: Explicación legible por humanos escrita en el registro de auditoría de seguridad al cargar. **Se recomienda encarecidamente proporcionar un Reason claro para todos los plugins en producción.**

## Cómo PluginLoader procesa las declaraciones de capacidad

1. **Lectura de metadatos PE**: PluginLoader lee las declaraciones de capacidad de la tabla CustomAttribute del archivo PE **antes** de que comience el análisis de seguridad
2. **Relajación de reglas de escaneo**: Las capacidades declaradas eximen las referencias de tipo correspondientes de las verificaciones de espacios de nombres y tipos prohibidos
3. **Registro de auditoría**: Todas las declaraciones (incluido Reason) se escriben en el registro de auditoría de seguridad
4. **Capacidades no declarables**: P/Invoke, Unsafe, Reflection.Emit, etc. permanecen bloqueados independientemente de cualquier declaración

## Alcance de exención de Capability.Network

### Exenciones TypeRef

Cuando se declara `Capability.Network`, se relajan las siguientes reglas de prohibición basadas en espacios de nombres y tipos:

| Namespace exento | Tipos permitidos |
|-----------------|-----------------|
| `System.Net.Http` | `HttpClient`, `HttpRequestMessage`, `HttpResponseMessage`, etc. |
| `System.Net.WebSockets` | `ClientWebSocket`, `WebSocket`, etc. |
| `System.Net.Sockets` | `TcpClient`, `UdpClient`, `Socket`, etc. |
| `System.Net.Mail` | `SmtpClient`, `MailMessage`, etc. |
| `System.Net.NetworkInformation` | `Ping`, `NetworkInterface`, etc. |
| `System.Net.Security` | `SslStream`, etc. |
| `System.Net` (prohibiciones por tipo) | `HttpWebRequest`, `WebClient`, `Dns`, `FtpWebRequest`, etc. |

### Exenciones ILString

Las constantes de cadena que comienzan con estos prefijos no se marcan en el escaneo del montículo #US:
- `"System.Net.Http"`
- `"System.Net.WebSockets"`
- `"System.Net.Sockets"`
- `"System.Net.Mail"`
- `"System.Net.NetworkInformation"`
- `"System.Net.Security"`

### Lo que sigue prohibido

Incluso con `Capability.Network`, estas capacidades están **siempre** bloqueadas (capacidades no declarables):

| Categoría | Tipos bloqueados | Por qué no declarable |
|----------|-----------------|---------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` | No se puede auditar de forma segura en tiempo de ejecución |
| Código unsafe | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | Elude las garantías de seguridad de tipos |
| Emisión IL | `System.Reflection.Emit.*` | Puede generar código arbitrario en tiempo de ejecución |
| Carga de ensamblados | `System.Runtime.Loader`, `Assembly.Load*` | Puede eludir el análisis de seguridad cargando DLLs no verificadas |
| Registro | `Microsoft.Win32.*` | Acceso de sistema a nivel de SO fuera del sandbox del plugin |

## Campo Reason — Rol de auditoría

El campo `Reason` sirve como **pista de auditoría** para las declaraciones de capacidad:

```
Security audit: [CapabilityNetworkPlugin] com.siliconlife.demo.capabilitynetwork declared Capability.Network — reason: Calls weather REST API to retrieve forecast data
```

**Por qué Reason es importante:**
1. **Revisión de seguridad**: Los auditores pueden verificar que las capacidades declaradas coincidan con el comportamiento real del plugin
2. **Principio de mínimo privilegio**: Obliga a los autores de plugins a justificar por qué necesitan cada capacidad
3. **Cumplimiento**: Necesario para certificaciones de seguridad e investigaciones de incidentes
4. **Monitoreo en tiempo de ejecución**: Las herramientas de seguridad pueden alertar si el uso de la capacidad declarada excede la razón indicada

## Comparación con 08-ForbiddenNetwork

| Aspecto | 08-ForbiddenNetwork | 13-CapabilityNetwork |
|---------|-------------------|---------------------|
| Declaración | Ninguna | `[PluginCapability(Capability.Network)]` |
| Resultado de carga | ❌ Rechazado | ✅ Cargado exitosamente |
| Uso de HttpClient | Bloqueado por análisis TypeRef | Exento por la capacidad |
| Uso de TcpClient | Bloqueado por análisis TypeRef | Exento por la capacidad |
| Reason | No aplicable | Escrito en registro de auditoría |

**Diferencia clave**: 08-ForbiddenNetwork muestra qué pasa cuando se usan tipos de red **sin** declarar la capacidad. 13-CapabilityNetwork muestra la manera **correcta** de solicitar declarativamente el acceso a la red.

## Mejores prácticas de seguridad

1. **Declarar solo lo necesario**: Si solo necesita HTTP, no declare Capability.Network solo porque puede — pero note que Capability.Network es la única capacidad relacionada con la red; no hay opciones más granulares
2. **Preferir NetworkExecutor**: `NetworkExecutor` es el punto de entrada controlado para acceso a la red y no requiere ninguna declaración de capacidad
3. **Proporcionar un Reason claro**: Las razones vagas como "acceso a la red" son una señal de alarma en las revisiones de seguridad
4. **Respetar los límites no declarables**: Ninguna declaración de capacidad puede eludir las prohibiciones de P/Invoke, Unsafe o Reflection.Emit

## Archivos

- `Plugin.cs` — Plugin de demostración declarando Capability.Network
- `README.md` — English
- `README.zh-CN.md` — 简体中文
- `README.zh-HK.md` — 繁體中文
- `README.ja-JP.md` — 日本語
- `README.ko-KR.md` — 한국어
- `README.de-DE.md` — Deutsch
- `README.fr-FR.md` — Français
- `README.es-ES.md` — Este archivo (Español)
- `README.it-IT.md` — Italiano
- `README.ru-RU.md` — Русский
- `README.pt-PT.md` — Português
- `README.pl-PL.md` — Polski
- `README.cs-CZ.md` — Čeština

## Ejemplos relacionados

- **08-ForbiddenNetwork**: Anti-patrón mostrando operaciones de red bloqueadas
- **14-CapabilityFileIO**: Capacidad FileIO declarativa
- **15-CapabilityProcess**: Capacidad Process declarativa
- **16-CapabilityAI**: Capacidad de servicio IA declarativa
- **17-CapabilityStacked**: Apilamiento de capacidades múltiples
- **18-CapabilityDenied**: Anti-patrón de capacidad no declarable
