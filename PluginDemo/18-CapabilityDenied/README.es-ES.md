# PluginDemo-18: Capacidad denegada — Anti-patrón de capacidad no declarable

## Resumen

Este plugin es un **anti-patrón** que demuestra que declarar una capacidad NO evita las prohibiciones de capacidades no declarables. Incluso con `[PluginCapability(Capability.Network)]`, P/Invoke, Unsafe, Reflection.Emit y el acceso al registro permanecen **siempre** bloqueados.

## Capacidades declarables vs. no declarables

### ✅ Declarables (existen valores del enum Capability)

| Capacidad | Lo que exime |
|----------|-------------|
| `Capability.Network` | Espacios de nombres System.Net.* y prohibiciones por tipo |
| `Capability.FileIO` | Espacio de nombres System.IO (más allá de la lista blanca) |
| `Capability.Process` | Tipos Process* bajo System.Diagnostics |
| `Capability.AI` | Habilita inyección de IAIService (sin exención TypeRef) |

### ❌ No declarables (NO existe valor del enum Capability)

| Categoría | Tipos bloqueados | Por qué no declarable |
|----------|-----------------|---------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory`, `NativeLibrary` | No se puede auditar código nativo arbitrario en tiempo de ejecución |
| Código unsafe | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | Evita la seguridad de tipos CLR y la comprobación de límites |
| Emisión IL | `System.Reflection.Emit.*` | Puede generar IL arbitrario en tiempo de ejecución |
| Carga de ensamblados | `System.Runtime.Loader`, `Assembly.Load*` | Puede cargar DLLs no escaneadas, evadiendo el escaneo de seguridad |
| Registro | `Microsoft.Win32.*` | Acceso a sistema a nivel de SO fuera del sandbox del plugin |
| Compilación dinámica | `Microsoft.CodeAnalysis.*` | Puede compilar y ejecutar código arbitrario |
| Reflexión peligrosa | `Type.GetType(string)`, `Activator.CreateInstance` | Puede instanciar tipos prohibidos mediante cadena |

## Por qué estas capacidades no pueden declararse

La razón fundamental: **no pueden auditarse de forma segura en tiempo de ejecución.**

1. **P/Invoke**: Una vez llamado el código nativo, el CLR no tiene visibilidad — sin garantías de seguridad
2. **Unsafe**: Evita el sistema de seguridad de tipos del que depende el modelo de seguridad del plugin
3. **Reflection.Emit**: Puede generar nuevo IL en tiempo de ejecución que nunca fue escaneado por PluginLoader
4. **AssemblyLoadContext**: Puede cargar DLLs que nunca fueron escaneadas para seguridad
5. **Registry**: Proporciona acceso a configuración a nivel de SO fuera del sandbox del plugin

## Procesamiento de «declaración inválida» de PluginLoader

Cuando PluginLoader encuentra una declaración de capacidad:

1. Lee el valor enum int32 del blob CustomAttribute
2. Verifica `Enum.IsDefined(typeof(Capability), value)`
3. Si el valor no es un miembro Capability definido → **ignorado silenciosamente**
4. Si el valor está definido → se aplican las reglas de exención
5. **Las verificaciones no declarables se aplican SIEMPRE** independientemente de cualquier capacidad declarada

Esto evita que los plugins declaren capacidades «futuras» que aún no existen.

## Comparación con 13-CapabilityNetwork

| Aspecto | 13-CapabilityNetwork (positivo) | 18-CapabilityDenied (anti-patrón) |
|---------|-------------------------------|----------------------------------|
| Declaración | `[PluginCapability(Capability.Network)]` | `[PluginCapability(Capability.Network)]` |
| Usa HttpClient | ✅ Exento | ✅ Exento |
| Usa DllImport | N/A | ❌ SIEMPRE bloqueado |
| Usa Unsafe | N/A | ❌ SIEMPRE bloqueado |
| Resultado de carga | ✅ CARGADO | ❌ RECHAZADO |

## Archivos

- `Plugin.cs` — Plugin de demostración anti-patrón de capacidad no declarable
- `README.md` — Este archivo (Inglés)
- `README.zh-CN.md` — Chino simplificado
- Traducciones: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Ejemplos relacionados

- **13-CapabilityNetwork**: Ejemplo positivo de Capability.Network
- **11-ForbiddenPInvoke**: Anti-patrón P/Invoke (ninguna capacidad puede ayudar)
- **10-ForbiddenReflection**: Anti-patrón Reflection (ninguna capacidad puede ayudar)
