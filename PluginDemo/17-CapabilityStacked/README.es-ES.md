# PluginDemo-17: Apilamiento de capacidades — Permisos declarativos múltiples

## Resumen

Este plugin demuestra el apilamiento de múltiples atributos `[PluginCapability]` en una sola clase de plugin. `PluginCapabilityAttribute` tiene `AllowMultiple = true`, por lo que puede declarar tantas capacidades como necesite.

## Sintaxis de apilamiento

```csharp
[PluginCapability(Capability.Network, Reason = "API endpoint access for remote AI models")]
[PluginCapability(Capability.AI, Reason = "AI service provider for downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

## Cómo PluginLoader procesa las capacidades apiladas

1. **Lee todas las declaraciones** de la tabla CustomAttribute de metadatos PE
2. **Fusiona** las reglas de exención de todas las capacidades declaradas
3. **Registra independientemente** cada declaración con su propio campo Reason
4. **Sigue aplicando** las prohibiciones de capacidades no declarables independientemente del apilamiento

## Reglas de exención fusionadas

Al apilar `Capability.Network` + `Capability.AI`:

| Fuente | Exención |
|--------|---------|
| Capability.Network | System.Net.Http.*, System.Net.WebSockets.*, System.Net.Sockets.*, System.Net.Mail.*, System.Net.NetworkInformation.*, System.Net.Security.*, System.Net (prohibiciones por tipo) |
| Capability.AI | Inyección de IAIService habilitada |
| **Combinado** | El plugin puede usar HttpClient E IAIService |

## El apilamiento no otorga poder ilimitado

Incluso con múltiples capacidades apiladas, estas permanecen **siempre bloqueadas**:

- ❌ P/Invoke (`DllImport`, `Marshal`, `NativeMemory`)
- ❌ Código unsafe (`UnverifiableCodeAttribute`, `Unsafe`)
- ❌ Emisión IL (`System.Reflection.Emit.*`)
- ❌ Carga de ensamblados (`System.Runtime.Loader`, `Assembly.Load*`)
- ❌ Registro (`Microsoft.Win32.*`)

No existe ningún valor del enum `Capability` para estos — son **no declarables por diseño**.

## Pista de auditoría para capacidades apiladas

Cada capacidad se registra independientemente:

```
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.Network — reason: API endpoint access for remote AI models
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.AI — reason: AI service provider for downstream plugins
```

## Archivos

- `Plugin.cs` — Plugin de demostración con apilamiento Capability.Network + Capability.AI
- `README.md` — Este archivo (Inglés)
- `README.zh-CN.md` — Chino simplificado
- Traducciones: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Ejemplos relacionados

- **13-CapabilityNetwork**: Capacidad Network única
- **16-CapabilityAI**: Capacidad IA única
- **18-CapabilityDenied**: Anti-patrón de capacidades no declarables
