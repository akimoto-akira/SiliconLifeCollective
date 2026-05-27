# Demo IObjectFactory – Registrace a vytváření instancí

Demonstruje registraci a vytváření instancí pomocí `IObjectFactory`: registrace typů pomocí `RegisterAutoFactory` v `OnLoad`, vytváření instancí pomocí `CreateInstance` v `OnStart`.

## Přehled rozhraní IObjectFactory

`IObjectFactory` nahrazuje `Activator.CreateInstance()`. Pluginy registrují delegáty továrny v `IPlugin.OnLoad` a runtime vytváří instance pouze přes registrované delegáty, čímž zabraňuje libovolné instanciaci typů.

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

### Shrnutí metod

| Metoda | Popis |
|--------|-------|
| `RegisterFactory(Type, Func)` | Zaregistruje vlastní delegát továrny pro typ |
| `RegisterFactory<T>(Func)` | Generická verze `RegisterFactory` |
| `RegisterAutoFactory(Type)` | Automaticky analyzuje konstruktory typu a registruje továrnu |
| `RegisterAutoFactoryFromAssembly(Assembly, Type)` | Automaticky registruje továrny pro všechny neabstraktní podtypy v assembly |
| `CreateInstance(Type, args)` | Vytvoří instanci pomocí registrované továrny (negenerická) |
| `CreateInstance<T>(args)` | Vytvoří instanci pomocí registrované továrny (generická) |
| `IsRegistered(Type)` | Zkontroluje, zda je pro typ registrována továrna |

## Proč IObjectFactory nahrazuje Activator.CreateInstance

`Activator.CreateInstance` umožňuje libovolnou instanciaci typů, což je bezpečnostní riziko v pluginovém systému. `IObjectFactory` vynucuje model whitelistu:

- Pouze typy s **registrovanou továrnou** mohou být instanciovány
- Továrny jsou registrovány explicitně v `OnLoad`, host má plnou kontrolu
- `RegisterAutoFactory` je pohodlná metoda analyzující konstruktory, ale registrace je povinná

```
❌ Activator.CreateInstance(typeof(SomeType))     → bezpečnostní riziko
✅ factory.CreateInstance(typeof(SomeType))         → pouze registrované typy
✅ factory.CreateInstance<SomeType>()               → generická pohodlná metoda
```

## Jak funguje RegisterAutoFactory

`RegisterAutoFactory` prozkoumá konstruktory typu a vygeneruje delegát továrny:

1. **Bez argumentů** → zavolá bezparametrický konstruktor
2. **S argumenty** → přiřadí parametry konstruktoru podle typu, fallback na bezparametrický konstruktor
3. **Abstraktní typy/rozhraní** → odmítnuty s varováním

## Tok registrace a vytváření

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ Získat IObjectFactory ze ServiceLocator                  │
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

## Toto demo

> **⚠️ Poznámka:** `SimpleService` a `ConfiguredService` jsou **vlastní typy definované výhradně pro toto demo**. Nesouvisí s žádnými systémovými servisními rozhraními.

| Třída | Role |
|-------|------|
| `SimpleService` | Demo typ, bezparametrický konstruktor |
| `ConfiguredService` | Demo typ, parametrický konstruktor `(string name)` |
| `ObjectFactoryUsagePlugin` | Implementace `IPlugin` — registruje továrny a vytváří instance |

## Bezpečnostní poznámka

`IObjectFactory` je součástí modelu řízeného přístupu k zabezpečení. Pluginy **nesmí** používat `Activator.CreateInstance` k vytváření objektů — musí registrovat továrny a používat `CreateInstance`. Viz [dokumentace zabezpečení](../../docs/cs-CZ/security.md).
