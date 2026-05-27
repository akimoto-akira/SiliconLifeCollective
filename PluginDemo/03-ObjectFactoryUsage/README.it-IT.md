# Demo IObjectFactory – Registrazione e creazione di istanze

Dimostra la registrazione e la creazione di istanze con `IObjectFactory`: registrare i tipi con `RegisterAutoFactory` in `OnLoad`, creare istanze con `CreateInstance` in `OnStart`.

## Panoramica dell'interfaccia IObjectFactory

`IObjectFactory` sostituisce `Activator.CreateInstance()`. I plugin registrano delegati di factory in `IPlugin.OnLoad`, e il runtime crea istanze solo tramite delegati registrati, impedendo l'istanziazione arbitraria dei tipi.

```csharp
public interface IObjectFactory
{
    void RegisterFactory(Type type, Func<object?[], object> factory);
    void RegisterFactory<T>(Func<object?[], T> factory) where T : class;
    void RegisterAutoFactory(Type type);
    void RegisterAutoFactoryFromAssembly(System.Reflection.Assembly assembly, Type baseType);
    object? CreateInstance(Type type, params object?[] args);
    T? CreateInstance<T>(params object?[] args) where T : class;
    bool IsRegistered(Type type);
}
```

### Riepilogo dei metodi

| Metodo | Descrizione |
|--------|-------------|
| `RegisterFactory(Type, Func)` | Registra un delegato di factory personalizzato per un tipo |
| `RegisterFactory<T>(Func)` | Versione generica di `RegisterFactory` |
| `RegisterAutoFactory(Type)` | Analizza automaticamente i costruttori del tipo e registra una factory |
| `RegisterAutoFactoryFromAssembly(Assembly, Type)` | Registra automaticamente factory per tutti i sottotipi non astratti in un assembly |
| `CreateInstance(Type, args)` | Crea un'istanza usando una factory registrata (non generica) |
| `CreateInstance<T>(args)` | Crea un'istanza usando una factory registrata (generica) |
| `IsRegistered(Type)` | Verifica se una factory è registrata per un tipo |

## Perché IObjectFactory sostituisce Activator.CreateInstance

`Activator.CreateInstance` permette l'istanziazione arbitraria dei tipi, che è un rischio per la sicurezza in un sistema di plugin. `IObjectFactory` impone un modello a whitelist:

- Solo i tipi con una **factory registrata** possono essere istanziati
- Le factory vengono registrate esplicitamente in `OnLoad`, l'host ha il controllo completo
- `RegisterAutoFactory` è un metodo di convenienza che analizza i costruttori ma richiede comunque la registrazione

```
❌ Activator.CreateInstance(typeof(SomeType))     → rischio di sicurezza
✅ factory.CreateInstance(typeof(SomeType))         → solo tipi registrati
✅ factory.CreateInstance<SomeType>()               → metodo generico conveniente
```

## Come funziona RegisterAutoFactory

`RegisterAutoFactory` ispeziona i costruttori del tipo e genera un delegato di factory:

1. **Senza argomenti** → chiama il costruttore senza parametri
2. **Con argomenti** → corrisponde ai parametri del costruttore per tipo, fallback al costruttore senza parametri
3. **Tipi astratti/interfacce** → rifiutati con avviso

## Flusso di registrazione e creazione

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ Ottenere IObjectFactory da ServiceLocator                │
│  ├─ RegisterAutoFactory(typeof(SimpleService))               │
│  └─ RegisterAutoFactory(typeof(ConfiguredService))           │
│                                                              │
│  OnStart                                                     │
│  ├─ CreateInstance(typeof(SimpleService))                    │
│  │  → "SimpleService created via parameterless constructor"  │
│  └─ CreateInstance<ConfiguredService>("DemoPlugin")          │
│     → "ConfiguredService created with name='DemoPlugin'"     │
└──────────────────────────────────────────────────────────────┘
```

## Questa demo

> **⚠️ Nota:** `SimpleService` e `ConfiguredService` sono **tipi personalizzati definiti esclusivamente per questa demo**. Non sono correlati ad alcuna interfaccia di servizio del sistema.

| Classe | Ruolo |
|--------|-------|
| `SimpleService` | Tipo demo, costruttore senza parametri |
| `ConfiguredService` | Tipo demo, costruttore parametrizzato `(string name)` |
| `ObjectFactoryUsagePlugin` | Implementazione `IPlugin` — registra factory e crea istanze |

## Nota di sicurezza

`IObjectFactory` fa parte del modello di sicurezza ad accesso controllato. I plugin **non devono** usare `Activator.CreateInstance` per creare oggetti — devono registrare factory e usare `CreateInstance`. Vedere la [documentazione sulla sicurezza](../../docs/it-IT/security.md).
