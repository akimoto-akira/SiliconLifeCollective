# PluginDemo-19: TickObject — Tarea periódica en MainLoop

## Resumen

Este plugin demuestra cómo usar `TickObject` para integrarse con `MainLoop` e implementar lógica periódica/continua. TickObject es la clase base para objetos que pueden ser ticados por el bucle principal de MainLoop, proporcionando una alternativa unificada a `System.Threading.Timer` o `Task.Delay`.

## Ciclo de vida de TickObject

```
Constructor(interval, autoRegister)
    │
    ├── autoRegister=true → MainLoop.Register(this) se llama automáticamente en el constructor
    │
    ├── autoRegister=false → llamar MainLoop.Register(this) manualmente después
    │
    ▼
MainLoop.Tick() bucle
    │
    ├── Ordenar todos los TickObjects registrados por Priority (ascendente)
    ├── Acumular elapsedTime para cada TickObject
    ├── Si elapsedTime >= Interval → llamar OnTick(deltaTime)
    │
    ├── Circuit breaker: si OnTick excede TickTimeout → incrementar contador de timeout
    │   └── Tras maxTimeoutCount timeouts consecutivos → enfriamiento de 1 minuto
    │
    ▼
MainLoop.Unregister(tickObject) — limpieza en OnStop
```

## Propiedades clave

| Propiedad | Tipo | Predeterminado | Descripción |
|----------|------|-------------|-------------|
| `Interval` | `TimeSpan` | Requerido | Con qué frecuencia se llama OnTick |
| `Priority` | `int` | 100 | Orden de ejecución (menor = mayor prioridad) |
| `autoRegister` | `bool` | `true` | Auto-registrar en MainLoop en el constructor |

## Métodos clave

| Método | Descripción |
|--------|-------------|
| `OnTick(TimeSpan deltaTime)` | Sobrescribir para implementar lógica periódica |
| `MainLoop.Register(TickObject)` | Registrar manualmente en MainLoop |
| `MainLoop.Unregister(TickObject)` | Eliminar de MainLoop (limpieza) |

## Escenarios de demostración

### 1. Temporizador básico (autoRegister=true)
```csharp
public class StatusTimer : TickObject
{
    public StatusTimer() : base(interval: TimeSpan.FromSeconds(5), autoRegister: true)
    {
        Priority = 100;
    }

    protected override void OnTick(TimeSpan deltaTime)
    {
        Console.WriteLine($"Tick, deltaTime={deltaTime.TotalMilliseconds:F0}ms");
    }
}
```

### 2. Registro manual (autoRegister=false)
```csharp
// En constructor: no auto-registrar
_heartbeatTimer = new HeartbeatTimer(autoRegister: false);

// En OnStart: registrar manualmente
MainLoop.Register(_heartbeatTimer);
```

### 3. Orden de prioridad
- `Priority = 10` → Alta prioridad, se ejecuta primero
- `Priority = 200` → Baja prioridad, se ejecuta después

### 4. Limpieza
```csharp
// En OnStop: siempre desregistrar para prevenir fugas
MainLoop.Unregister(_statusTimer);
```

## Circuit breaker de MainLoop

MainLoop tiene un circuit breaker integrado para evitar que TickObjects lentos bloqueen todo el bucle:

1. Si `OnTick` excede `TickTimeout` (1 segundo por defecto) → el contador de timeout aumenta
2. Tras `maxTimeoutCount` (3 por defecto) timeouts consecutivos → el circuit breaker se dispara
3. El TickObject disparado es **saltado** durante 1 minuto de enfriamiento
4. Tras el enfriamiento, el TickObject recibe otra oportunidad

## TickObject vs System.Threading.Timer

| Aspecto | TickObject + MainLoop | System.Threading.Timer |
|---------|----------------------|----------------------|
| Modelo de hilos | Hilo de bucle principal único | Hilos del pool de hilos |
| Orden de ejecución | Determinista (por Priority) | No determinista |
| Circuit breaker | Integrado | Ninguno |
| Depuración | Fácil (hilo único) | Difícil (condiciones de carrera) |
| Uso de recursos | Mínimo (sin pool de hilos) | Sobrecarga del pool de hilos |
| Precisión del intervalo | Best-effort (afectado por otros TickObjects) | Más preciso |

## Nota de seguridad

TickObject en sí mismo **no requiere** declaración de capacidad. Es un mecanismo de framework integrado seguro.

## Archivos

- `Plugin.cs` — Plugin de demostración TickObject
- `README.md` — Este archivo (Inglés)
- `README.zh-CN.md` — Chino simplificado
- Traducciones: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Ejemplos relacionados

- **13-CapabilityNetwork**: Declaración Capability.Network
- **20-SpeedyPack**: Almacenamiento de datos sin Capability.FileIO
