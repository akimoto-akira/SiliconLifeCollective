# PluginDemo-19: TickObject — Tarefa periódica no MainLoop

## Visão geral

Este plugin demonstra como usar `TickObject` para integrar com `MainLoop` para lógica periódica/contínua. TickObject é a classe base para objetos que podem ser tickados pelo ciclo principal do MainLoop, fornecendo uma alternativa unificada a `System.Threading.Timer` ou `Task.Delay`.

## Ciclo de vida do TickObject

```
Constructor(interval, autoRegister)
    │
    ├── autoRegister=true → MainLoop.Register(this) chamado automaticamente no construtor
    │
    ├── autoRegister=false → chamar MainLoop.Register(this) manualmente depois
    │
    ▼
MainLoop.Tick() ciclo
    │
    ├── Ordenar todos os TickObjects registados por Priority (ascendente)
    ├── Acumular elapsedTime para cada TickObject
    ├── Se elapsedTime >= Interval → chamar OnTick(deltaTime)
    │
    ├── Circuit breaker: se OnTick excede TickTimeout → incrementar contador de timeout
    │   └── Após maxTimeoutCount timeouts consecutivos → arrefecimento de 1 minuto
    │
    ▼
MainLoop.Unregister(tickObject) — limpeza em OnStop
```

## Propriedades-chave

| Propriedade | Tipo | Predefinido | Descrição |
|------------|------|-----------|----------|
| `Interval` | `TimeSpan` | Obrigatório | Com que frequência OnTick é chamado |
| `Priority` | `int` | 100 | Ordem de execução (menor = prioridade mais alta) |
| `autoRegister` | `bool` | `true` | Auto-registar no MainLoop no construtor |

## Métodos-chave

| Método | Descrição |
|--------|----------|
| `OnTick(TimeSpan deltaTime)` | Sobrescrever para implementar lógica periódica |
| `MainLoop.Register(TickObject)` | Registar manualmente no MainLoop |
| `MainLoop.Unregister(TickObject)` | Remover do MainLoop (limpeza) |

## Cenários de demonstração

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

### 2. Registo manual (autoRegister=false)
```csharp
// No construtor: não auto-registar
_heartbeatTimer = new HeartbeatTimer(autoRegister: false);

// No OnStart: registar manualmente
MainLoop.Register(_heartbeatTimer);
```

### 3. Ordem de prioridade
- `Priority = 10` → Alta prioridade, executa primeiro
- `Priority = 200` → Baixa prioridade, executa depois

### 4. Limpeza
```csharp
// No OnStop: sempre desregistar para prevenir fugas
MainLoop.Unregister(_statusTimer);
```

## Circuit breaker do MainLoop

O MainLoop tem um circuit breaker integrado para impedir que TickObjects lentos bloqueiem todo o ciclo:

1. Se `OnTick` excede `TickTimeout` (1 segundo por defeito) → o contador de timeout aumenta
2. Após `maxTimeoutCount` (3 por defeito) timeouts consecutivos → o circuit breaker dispara
3. O TickObject disparado é **ignorado** durante 1 minuto de arrefecimento
4. Após o arrefecimento, o TickObject recebe outra oportunidade

## TickObject vs System.Threading.Timer

| Aspeto | TickObject + MainLoop | System.Threading.Timer |
|--------|----------------------|----------------------|
| Modelo de threads | Thread de ciclo principal único | Threads do pool de threads |
| Ordem de execução | Determinista (por Priority) | Não determinista |
| Circuit breaker | Integrado | Nenhum |
| Depuração | Fácil (thread único) | Difícil (condições de corrida) |
| Uso de recursos | Mínimo (sem pool de threads) | Sobrecarga do pool de threads |
| Precisão do intervalo | Best-effort (afetado por outros TickObjects) | Mais preciso |

## Nota de segurança

O TickObject em si **não requer** declaração de capacidade. É um mecanismo de framework integrado seguro.

## Ficheiros

- `Plugin.cs` — Plugin de demonstração TickObject
- `README.md` — Este ficheiro (Inglês)
- `README.zh-CN.md` — Chinês simplificado
- Traduções: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Exemplos relacionados

- **13-CapabilityNetwork**: Declaração Capability.Network
- **20-SpeedyPack**: Armazenamento de dados sem Capability.FileIO
