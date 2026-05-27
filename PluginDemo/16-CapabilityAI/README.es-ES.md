# PluginDemo-16: Capability.AI — Permiso de servicio IA declarativo

## Resumen

Este plugin demuestra el uso de `[PluginCapability(Capability.AI)]` para declarar que un plugin requiere acceso al servicio de IA. A diferencia de otras capacidades, `Capability.AI` **no** exime ningún namespace prohibido — en su lugar, permite al host inyectar una referencia `IAIService` en el plugin.

## Concepto clave: Capability.AI no otorga acceso a la red

`Capability.AI` es fundamentalmente diferente de las otras capacidades:

| Capacidad | Lo que exime | Cómo funciona |
|----------|-------------|--------------|
| `Capability.Network` | Namespaces `System.Net.*` | Relaja reglas de escaneo TypeRef/ILString |
| `Capability.FileIO` | Namespace `System.IO` | Relaja reglas de escaneo TypeRef/ILString |
| `Capability.Process` | Tipos `Process*` | Relaja reglas de escaneo TypeRef/ILString |
| `Capability.AI` | **Nada** | Habilita la inyección de IAIService por el host |

`IAIService` se encuentra en el namespace `SiliconLife.Collective` — nunca está en ninguna lista de prohibición. La declaración de capacidad es una **señal de opt-in** al host de que este plugin debe recibir la referencia del servicio de IA.

## Apilamiento de capacidades: IA + Red

Si su cliente de IA necesita acceso directo a la red (p. ej., llamar a un endpoint de IA remoto), debe declarar **ambas** capacidades:

```csharp
[PluginCapability(Capability.Network, Reason = "Calls remote AI endpoint")]
[PluginCapability(Capability.AI, Reason = "Provides IAIService to downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

Vea **17-CapabilityStacked** para ejemplos completos de apilamiento.

## Patrón de punto de entrada controlado

| Recurso | Punto de entrada controlado | Capacidad necesaria |
|---------|---------------------------|-------------------|
| Archivos | `PermissionedStreamFactory` | Ninguna |
| Red | `NetworkExecutor` | Ninguna |
| Procesos | `CommandLineExecutor` | Ninguna |
| Almacenamiento de datos | `SpeedyPack` | Ninguna |
| Servicio de IA | `IAIService` | `Capability.AI` |

`IAIService` es único: **requiere** una declaración de capacidad. El acceso al servicio de IA es una función opt-in, no una capacidad predeterminada disponible para todos los plugins.

## Archivos

- `Plugin.cs` — Plugin de demostración que declara Capability.AI
- `README.md` — Este archivo (Inglés)
- `README.zh-CN.md` — Chino simplificado
- Traducciones: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Ejemplos relacionados

- **17-CapabilityStacked**: Apilamiento de múltiples capacidades (Red + IA)
- **18-CapabilityDenied**: Anti-patrón de capacidades no declarables
