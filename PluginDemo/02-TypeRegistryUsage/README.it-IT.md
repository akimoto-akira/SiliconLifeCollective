# Demo ITypeRegistry – Registrazione e query

Dimostra la registrazione e la ricerca con `ITypeRegistry`: registrare tipi personalizzati in `OnLoad`, scoprirli con `FindSubtypesOf` in `OnStart`.

## Panoramica dell'interfaccia ITypeRegistry

`ITypeRegistry` sostituisce la scansione tramite reflection `AppDomain.CurrentDomain.GetAssemblies()`. I plugin registrano esplicitamente i loro tipi esposti in `IPlugin.OnLoad`, e il runtime cerca i tipi solo dal registro.

```csharp
public interface ITypeRegistry
{
    void RegisterType(Type type);
    void RegisterTypes(IEnumerable<Type> types);
    void RegisterFromAssembly(System.Reflection.Assembly assembly, Type baseType);
    Type? FindType(string fullName);
    IEnumerable<Type> FindSubtypesOf(Type baseType);
    IEnumerable<Type> FindImplementationsOf(Type interfaceType);
}
```

### Riepilogo dei metodi

| Metodo | Descrizione |
|--------|-------------|
| `RegisterType(Type)` | Registra un singolo tipo |
| `RegisterTypes(IEnumerable<Type>)` | Registra più tipi contemporaneamente |
| `RegisterFromAssembly(Assembly, Type)` | Registra tutti i sottotipi non astratti di `baseType` dall'assembly specificato |
| `FindType(string)` | Trova un tipo per nome completo; supporta la risoluzione dei nomi di tipi generici |
| `FindSubtypesOf(Type)` | Trova tutti i sottotipi non astratti del tipo base specificato |
| `FindImplementationsOf(Type)` | Trova tutti i tipi non astratti che implementano l'interfaccia specificata |

## Flusso di registrazione e query

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ Ottenere ITypeRegistry da ServiceLocator                 │
│  ├─ RegisterType(typeof(GreetingTool))                       │
│  ├─ RegisterType(typeof(FarewellTool))                       │
│  └─ RegisterType(typeof(StatusTool))                         │
│                                                              │
│  Alternativa: RegisterFromAssembly                           │
│  └─ RegisterFromAssembly(assembly, typeof(DemoTool))         │
│     → registrare tutti i sottotipi DemoTool in una volta     │
│                                                              │
│  OnStart                                                     │
│  ├─ FindSubtypesOf(typeof(DemoTool))                         │
│  └─ Iterare i risultati → GreetingTool, FarewellTool, …     │
└──────────────────────────────────────────────────────────────┘
```

## Utilizzo di RegisterFromAssembly

`RegisterFromAssembly` scansiona un assembly e registra tutti i sottotipi non astratti del tipo base specificato:

```csharp
_registry.RegisterFromAssembly(
    typeof(TypeRegistryUsagePlugin).Assembly,  // l'assembly da scansionare
    typeof(DemoTool)                            // registrare solo i sottotipi DemoTool
);
```

Ciò equivale a chiamare `RegisterType` per ogni sottotipo individualmente, ma è più conciso quando un plugin definisce molti tipi che condividono una classe base comune.

## Questa demo

> **⚠️ Importante:** `DemoTool` è un **tipo personalizzato definito esclusivamente per questa demo** per dimostrare la registrazione e la ricerca tramite `ITypeRegistry`. Non ha **alcuna relazione** con l'interfaccia `ITool` del sistema (`SiliconLife.Collective.ITool`) utilizzata per la registrazione degli strumenti IA. Il nome «Tool» è casuale — qualsiasi gerarchia di classi personalizzata funzionerebbe allo stesso modo.

| Classe | Ruolo |
|--------|-------|
| `DemoTool` | Classe base astratta personalizzata — ancora di registrazione (non correlata a `ITool`) |
| `GreetingTool` | Sottotipo concreto registrato in `OnLoad` |
| `FarewellTool` | Sottotipo concreto registrato in `OnLoad` |
| `StatusTool` | Sottotipo concreto registrato in `OnLoad` |
| `TypeRegistryUsagePlugin` | Implementazione `IPlugin` — registra e interroga i tipi |

## Nota di sicurezza

`ITypeRegistry` fa parte del modello di sicurezza ad accesso controllato. I plugin **non devono** utilizzare `AppDomain.CurrentDomain.GetAssemblies()` o `Assembly.GetTypes()` per scoprire i tipi — devono utilizzare `ITypeRegistry`. Consultare la [documentazione di sicurezza](../../docs/it-IT/security.md).
