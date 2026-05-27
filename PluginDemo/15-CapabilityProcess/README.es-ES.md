# PluginDemo-15: Capability.Process — Permiso declarativo de proceso

## Resumen

Este plugin demuestra el uso de `[PluginCapability(Capability.Process)]` para declarar que un plugin requiere la capacidad de lanzar procesos hijos. Con esta declaración, el plugin accede a `System.Diagnostics.Process` y tipos relacionados.

## Sintaxis de declaración

```csharp
[PluginCapability(Capability.Process, Reason = "Launch build tools for CI pipeline")]
public class CapabilityProcessPlugin : IPlugin { ... }
```

## Alcance de exención de Capability.Process

### Exenciones TypeRef

Solo los tipos relacionados con Process bajo `System.Diagnostics` están exentos:

| Tipo exento | Uso |
|------------|-----|
| `Process` | Iniciar, gestionar y monitorear procesos hijos |
| `ProcessStartInfo` | Configurar parámetros de inicio del proceso |
| `ProcessThread` | Acceder a información de hilos del proceso |
| `ProcessModule` | Acceder a información de módulos del proceso |
| `ProcessPriorityClass` | Establecer prioridad del proceso |
| `ProcessWindowStyle` | Configurar estilo de ventana del proceso |

Tipos siempre permitidos (nunca en la lista de prohibición): `Stopwatch`, `Debug`, `Trace`, `Activity`

### Exención ILString

- Las cadenas que comienzan con `"System.Diagnostics.Process"` no se marcan

## Comparación con 09-ForbiddenProcess

| Aspecto | 09-ForbiddenProcess | 15-CapabilityProcess |
|---------|-------------------|---------------------|
| Declaración | Ninguna | `[PluginCapability(Capability.Process)]` |
| Process.Start | ❌ RECHAZADO | ✅ PERMITIDO |
| ProcessStartInfo | ❌ RECHAZADO | ✅ PERMITIDO |

## Recomendación: CommandLineExecutor

Incluso con `Capability.Process`, se recomienda priorizar `CommandLineExecutor`:

| Característica | CommandLineExecutor | Process directo |
|---------------|-------------------|----------------|
| Declaración de capacidad necesaria | No | Sí |
| Sandbox | Lista blanca de comandos | Ninguno |
| Tiempos de espera | Integrado | Manual |
| Captura de salida | Estructurada | Manual |
| Registro de auditoría | Automático | Manual |

Use `Capability.Process` + `Process` directo solo cuando necesite control detallado sobre flujos de I/O, manejo de eventos de proceso, o cuando la lista blanca de CommandLineExecutor sea demasiado restrictiva.

## Mejores prácticas de seguridad

1. **Preferir CommandLineExecutor**: Usar punto de entrada controlado cuando sea posible
2. **Proporcionar una Reason clara**: "Launch build tools for CI pipeline" vs vago "process access"
3. **Validar todas las entradas**: Nunca pasar entradas no confiables directamente a ProcessStartInfo
4. **Usar WaitForExit**: Siempre esperar la finalización del proceso para prevenir procesos zombie
5. **Redirigir flujos**: Establecer `RedirectStandardOutput = true` y `UseShellExecute = false`

## Archivos

- `Plugin.cs` — Plugin de demostración que declara Capability.Process
- `README.md` — Este archivo (Inglés)
- `README.zh-CN.md` — Chino simplificado
- Traducciones: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Ejemplos relacionados

- **09-ForbiddenProcess**: Anti-patrón de operaciones de proceso bloqueadas
- **18-CapabilityDenied**: Anti-patrón de capacidades no declarables
