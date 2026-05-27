# PluginDemo-09: Anti-patrón de operaciones de proceso prohibidas

## Descripción general

Este plugin demuestra operaciones de ejecución de procesos **prohibidas** en el sistema de plugins de SiliconLife. Sirve como referencia de anti-patrón, mostrando lo que NO se debe hacer y proporcionando alternativas correctas para cada violación.

## ¿Por qué están prohibidos los tipos Process?

`System.Diagnostics.Process` y `ProcessStartInfo` están bloqueados en los plugins porque la ejecución directa de procesos presenta graves riesgos de seguridad:

1. **Ejecución arbitraria de comandos**: Los plugins podrían ejecutar cualquier comando sin auditoría ni verificación de permisos
2. **Lanzamiento de malware**: Plugins maliciosos podrían ejecutar aplicaciones o scripts no deseados
3. **Acceso a recursos del sistema**: Los procesos podrían acceder a recursos sensibles fuera del sandbox del plugin
4. **Sin validación de comandos**: Process.Start directo no tiene protección integrada contra inyección de comandos
5. **Sin rastro de auditoría**: Las operaciones directas de proceso eluden el sistema de auditoría de seguridad
6. **Escalada de privilegios**: Podría generar procesos con privilegios más altos que los del plugin

## ¿Qué tipos están prohibidos?

Solo los tipos relacionados con Process están prohibidos, **NO todo el espacio de nombres System.Diagnostics**:

| Tipo prohibido | Método bloqueado | Nivel de riesgo |
|---------------|-----------------|----------------|
| `Process` | `Start()`, `Kill()`, `WaitForExit()` | 🔴 Crítico |
| `ProcessStartInfo` | Constructor, todas las propiedades | 🔴 Crítico |
| `Process` | `StandardInput`, `StandardOutput`, `StandardError` | 🔴 Crítico |
| `Process` | `GetProcesses()`, `GetProcessesByName()` | 🟡 Alto |

## ¿Qué tipos están permitidos?

Otros tipos de `System.Diagnostics` que no involucran ejecución de procesos siguen disponibles:

| Tipo permitido | Uso | Por qué es seguro |
|---------------|-----|-------------------|
| `Stopwatch` | Medición de tiempo | Sin ejecución de procesos |
| `Debug` | Salida de depuración | Sin riesgo de seguridad |
| `Trace` | Trazado/registro | Sin riesgo de seguridad |
| `PerformanceCounter` | Monitoreo de rendimiento | Solo lectura, auditado |

## ¿Cómo ejecutar comandos de forma segura?

### Usar CommandLineExecutor (la única forma segura)

`CommandLineExecutor` es el **punto de entrada controlado** para la ejecución de comandos en plugins:

```csharp
// ✅ CORRECTO: Ejecutar un comando
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);

if (result.Success)
{
    Console.WriteLine(result.Output);
}
else
{
    Console.WriteLine($"Error: {result.Error}");
}
```

**Lo que proporciona CommandLineExecutor:**
1. **Protección contra inyección de comandos**: Bloquea separadores peligrosos (`||`, `&&`, `|`, `&`, `;`)
2. **Aplicación de timeout**: Timeout predeterminado de 30 segundos (configurable)
3. **Registro de auditoría**: Todas las ejecuciones de comandos se registran para revisión de seguridad
4. **Captura de salida**: Captura automática de stdout y stderr
5. **Soporte multiplataforma**: Usa `cmd.exe` en Windows, `/bin/bash` en Unix
6. **Manejo de errores**: Devuelve resultado estructurado con estado de éxito/fallo

## Violaciones demostradas

Este plugin muestra 5 violaciones comunes de ejecución de procesos:

### Violación 1: Process.Start

```csharp
// ❌ PROHIBIDO
Process.Start("notepad.exe");

// ✅ CORRECTO
var request = new ExecutorRequest { ResourcePath = "notepad.exe" };
var result = CommandLineExecutor.Execute(request);
```

**TypeRef bloqueado**: `System.Diagnostics.Process::Start(System.String)`

### Violación 2: ProcessStartInfo

```csharp
// ❌ PROHIBIDO
var psi = new ProcessStartInfo {
    FileName = "cmd.exe",
    Arguments = "/c dir",
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = new Process { StartInfo = psi };
process.Start();

// ✅ CORRECTO
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);
Console.WriteLine(result.Output);
```

**TypeRef bloqueado**: `System.Diagnostics.ProcessStartInfo::.ctor()`

### Violación 3: Process con argumentos

```csharp
// ❌ PROHIBIDO
var psi = new ProcessStartInfo("ping", "127.0.0.1 -n 4") {
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = Process.Start(psi);
process.WaitForExit();

// ✅ CORRECTO
var request = new ExecutorRequest { ResourcePath = "ping 127.0.0.1 -n 4" };
var result = CommandLineExecutor.Execute(request, TimeSpan.FromSeconds(10));
Console.WriteLine(result.Output);
```

**TypeRef bloqueado**: `System.Diagnostics.Process::Start(ProcessStartInfo)`

### Violación 4: Redirección de salida de proceso

```csharp
// ❌ PROHIBIDO
var psi = new ProcessStartInfo("ipconfig") {
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true
};
using var process = Process.Start(psi);
string output = process.StandardOutput.ReadToEnd();
string error = process.StandardError.ReadToEnd();

// ✅ CORRECTO
var request = new ExecutorRequest { ResourcePath = "ipconfig" };
var result = CommandLineExecutor.Execute(request);
if (result.Success) Console.WriteLine(result.Output);
else Console.WriteLine(result.Error);
```

**TypeRef bloqueado**: `System.Diagnostics.Process::StandardOutput`

### Violación 5: Process.Kill

```csharp
// ❌ PROHIBIDO
Process[] processes = Process.GetProcessesByName("notepad");
foreach (var p in processes) p.Kill();

// ✅ CORRECTO
// Por razones de seguridad, CommandLineExecutor no soporta la terminación de procesos.
// Contacte al administrador del sistema si es necesario.
```

**TypeRef bloqueado**: `System.Diagnostics.Process::Kill()`

## ¿Por qué solo Process y no todo System.Diagnostics?

El sistema de plugins adopta un enfoque **quirúrgico** de seguridad:

- **Solo bloquear tipos peligrosos**: Process/ProcessStartInfo permiten la ejecución de código arbitrario
- **Permitir tipos seguros**: Stopwatch, Debug, Trace no tienen implicaciones de seguridad
- **Minimizar impacto**: Los desarrolladores pueden seguir usando herramientas de diagnóstico sin riesgo
- **Frontera clara**: Solo los tipos que pueden crear/terminar procesos están prohibidos

## Mecanismo de seguridad de PluginLoader

Cuando PluginLoader analiza este plugin:

1. **Escaneo TypeRef**: Detecta referencias a tipos prohibidos `Process`/`ProcessStartInfo`
2. **Escaneo MemberRef**: Detecta llamadas a métodos bloqueados (ej: `Process.Start`)
3. **Escaneo IL String**: Detecta intentos de reflexión basados en cadenas
4. **Rechazo**: El plugin es rechazado durante la carga con un mensaje de error detallado

## Mejores prácticas

1. **Siempre usar CommandLineExecutor**: Nunca usar `Process.Start` directamente
2. **Establecer timeouts razonables**: Evitar que los comandos se bloqueen indefinidamente
3. **Verificar resultados**: Siempre verificar `result.Success` antes de usar la salida
4. **Sanitizar entrada**: Nunca pasar entrada de usuario directamente a comandos
5. **Declarar Capability si es necesario**: Si se necesita ejecución de procesos sin restricciones, declarar `Capability.Process` (ver 15-CapabilityProcess)

## Archivos

- `Plugin.cs` - Plugin de demostración anti-patrón
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Este archivo (Español)
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## Ejemplos relacionados

- **08-ForbiddenNetwork**: Operaciones de red prohibidas
- **15-CapabilityProcess**: Permiso declarativo de Process
- **10-ForbiddenReflection**: Operaciones de reflexión prohibidas
- **12-ForbiddenStringBypass**: Intentos de elusión por reflexión basada en cadenas
