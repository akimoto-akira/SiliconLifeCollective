# ITypeRegistry – Registrierung und Abfrage – Demo

Demonstriert `ITypeRegistry`-Registrierung und -Abfrage: benutzerdefinierte Typen in `OnLoad` registrieren, mit `FindSubtypesOf` in `OnStart` entdecken.

## ITypeRegistry-Schnittstellenübersicht

`ITypeRegistry` ersetzt das Reflection-Scanning über `AppDomain.CurrentDomain.GetAssemblies()`. Plugins registrieren ihre exponierten Typen explizit in `IPlugin.OnLoad`, und die Runtime sucht Typen nur über die Registry.

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

### Methodenübersicht

| Methode | Beschreibung |
|---------|--------------|
| `RegisterType(Type)` | Registriert einen einzelnen Typ |
| `RegisterTypes(IEnumerable<Type>)` | Registriert mehrere Typen auf einmal |
| `RegisterFromAssembly(Assembly, Type)` | Registriert alle nicht-abstrakten Untertypen von `baseType` aus der angegebenen Assembly |
| `FindType(string)` | Findet einen Typ anhand seines vollständigen Namens; unterstützt generische Typnamenauflösung |
| `FindSubtypesOf(Type)` | Findet alle nicht-abstrakten Untertypen des angegebenen Basistyps |
| `FindImplementationsOf(Type)` | Findet alle nicht-abstrakten Typen, die das angegebene Interface implementieren |

## Registrierungs- und Abfrageablauf

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ ITypeRegistry vom ServiceLocator abrufen                 │
│  ├─ RegisterType(typeof(GreetingTool))                       │
│  ├─ RegisterType(typeof(FarewellTool))                       │
│  └─ RegisterType(typeof(StatusTool))                         │
│                                                              │
│  Alternative: RegisterFromAssembly                           │
│  └─ RegisterFromAssembly(assembly, typeof(DemoTool))         │
│     → alle DemoTool-Untertypen auf einmal registrieren       │
│                                                              │
│  OnStart                                                     │
│  ├─ FindSubtypesOf(typeof(DemoTool))                         │
│  └─ Ergebnisse durchlaufen → GreetingTool, FarewellTool, …  │
└──────────────────────────────────────────────────────────────┘
```

## RegisterFromAssembly-Verwendung

`RegisterFromAssembly` scannt eine Assembly und registriert alle nicht-abstrakten Untertypen des angegebenen Basistyps:

```csharp
_registry.RegisterFromAssembly(
    typeof(TypeRegistryUsagePlugin).Assembly,  // zu scannende Assembly
    typeof(DemoTool)                            // nur DemoTool-Untertypen registrieren
);
```

Dies entspricht dem einzelnen Aufruf von `RegisterType` für jeden Untertyp, ist aber prägnanter, wenn ein Plugin viele Typen mit gemeinsamer Basisklasse definiert.

## Diese Demo

> **⚠️ Wichtig:** `DemoTool` ist ein **benutzerdefinierter Typ, der ausschließlich für diese Demo definiert wurde**, um `ITypeRegistry`-Registrierung und -Abfrage zu demonstrieren. Er hat **nichts** mit dem `ITool`-Interface des Systems (`SiliconLife.Collective.ITool`) zu tun, das für AI-Tool-Registrierung verwendet wird. Der Name „Tool" ist zufällig — jede benutzerdefinierte Klassenhierarchie funktioniert auf dieselbe Weise.

| Klasse | Rolle |
|--------|-------|
| `DemoTool` | Benutzerdefinierte abstrakte Basisklasse — Registrierungsanker (nicht verwandt mit `ITool`) |
| `GreetingTool` | Konkreter Untertyp, in `OnLoad` registriert |
| `FarewellTool` | Konkreter Untertyp, in `OnLoad` registriert |
| `StatusTool` | Konkreter Untertyp, in `OnLoad` registriert |
| `TypeRegistryUsagePlugin` | `IPlugin`-Implementierung — Typen registrieren und abfragen |

## Sicherheitshinweis

`ITypeRegistry` ist Teil des kontrollierten Zugriffs-Sicherheitsmodells. Plugins dürfen **nicht** `AppDomain.CurrentDomain.GetAssemblies()` oder `Assembly.GetTypes()` verwenden, um Typen zu entdecken — sie müssen stattdessen `ITypeRegistry` verwenden. Siehe [Sicherheitsdokumentation](../../docs/de-DE/security.md).
