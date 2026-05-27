# IObjectFactory – Registrierung und Instanzerzeugung – Demo

Demonstriert `IObjectFactory`-Registrierung und Instanzerzeugung: Typen in `OnLoad` mit `RegisterAutoFactory` registrieren, Instanzen in `OnStart` mit `CreateInstance` erzeugen.

## IObjectFactory-Schnittstellenübersicht

`IObjectFactory` ersetzt `Activator.CreateInstance()`. Plugins registrieren Factory-Delegaten in `IPlugin.OnLoad`, und die Runtime erzeugt Instanzen nur über registrierte Delegaten, was willkürliche Typinstanziierung verhindert.

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

### Methodenübersicht

| Methode | Beschreibung |
|---------|--------------|
| `RegisterFactory(Type, Func)` | Registriert einen benutzerdefinierten Factory-Delegaten für einen Typ |
| `RegisterFactory<T>(Func)` | Generische Version von `RegisterFactory` |
| `RegisterAutoFactory(Type)` | Analysiert automatisch die Konstruktoren des Typs und registriert eine Factory |
| `RegisterAutoFactoryFromAssembly(Assembly, Type)` | Registriert automatisch Factories für alle nicht-abstrakten Untertypen in einer Assembly |
| `CreateInstance(Type, args)` | Erzeugt eine Instanz über eine registrierte Factory (nicht-generisch) |
| `CreateInstance<T>(args)` | Erzeugt eine Instanz über eine registrierte Factory (generisch) |
| `IsRegistered(Type)` | Prüft, ob eine Factory für einen Typ registriert ist |

## Warum IObjectFactory Activator.CreateInstance ersetzt

`Activator.CreateInstance` erlaubt willkürliche Typinstanziierung, was in einem Plugin-System ein Sicherheitsrisiko darstellt. `IObjectFactory` erzwingt ein Whitelist-Modell:

- Nur Typen mit **registrierter Factory** können instanziiert werden
- Factories werden explizit in `OnLoad` registriert, der Host hat volle Kontrolle
- `RegisterAutoFactory` ist eine Convenience-Methode, die Konstruktoren analysiert, aber die Registrierungspflicht bleibt bestehen

```
❌ Activator.CreateInstance(typeof(SomeType))     → Sicherheitsrisiko
✅ factory.CreateInstance(typeof(SomeType))         → nur registrierte Typen
✅ factory.CreateInstance<SomeType>()               → generische Convenience
```

## Wie RegisterAutoFactory funktioniert

`RegisterAutoFactory` untersucht die Konstruktoren des Typs und generiert einen Factory-Delegaten:

1. **Keine Argumente** → ruft den parameterlosen Konstruktor auf
2. **Mit Argumenten** → matching der Konstruktorparameter nach Typ, Fallback auf parameterlosen Konstruktor
3. **Abstrakte/Interface-Typen** → werden mit Warnung abgelehnt

## Registrierungs- und Erzeugungsablauf

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ IObjectFactory vom ServiceLocator abrufen                │
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

## Diese Demo

> **⚠️ Hinweis:** `SimpleService` und `ConfiguredService` sind **benutzerdefinierte Typen, die ausschließlich für diese Demo definiert wurden**. Sie stehen in keinem Zusammenhang mit System-Service-Interfaces.

| Klasse | Rolle |
|--------|-------|
| `SimpleService` | Demo-Typ mit parameterlosem Konstruktor |
| `ConfiguredService` | Demo-Typ mit parametrisiertem Konstruktor `(string name)` |
| `ObjectFactoryUsagePlugin` | `IPlugin`-Implementierung — Factories registrieren und Instanzen erzeugen |

## Sicherheitshinweis

`IObjectFactory` ist Teil des kontrollierten Zugriffs-Sicherheitsmodells. Plugins dürfen **nicht** `Activator.CreateInstance` verwenden, um Objekte zu erzeugen — sie müssen Factories registrieren und `CreateInstance` verwenden. Siehe [Sicherheitsdokumentation](../../docs/de-DE/security.md).
