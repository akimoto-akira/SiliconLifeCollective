# Demo ITypeRegistry – Registrace a dotazy

Ukazuje registraci a vyhledávání pomocí `ITypeRegistry`: zaregistrovat vlastní typy v `OnLoad`, objevit je pomocí `FindSubtypesOf` v `OnStart`.

## Přehled rozhraní ITypeRegistry

`ITypeRegistry` nahrazuje skenování reflexí `AppDomain.CurrentDomain.GetAssemblies()`. Pluginy explicitně registrují své vystavené typy v `IPlugin.OnLoad` a runtime vyhledává typy pouze z registru.

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

### Shrnutí metod

| Metoda | Popis |
|--------|-------|
| `RegisterType(Type)` | Zaregistruje jeden typ |
| `RegisterTypes(IEnumerable<Type>)` | Zaregistruje více typů najednou |
| `RegisterFromAssembly(Assembly, Type)` | Zaregistruje všechny neabstraktní podtypy `baseType` ze zadaného sestavení |
| `FindType(string)` | Najde typ podle plného jména; podporuje rozlišení jmen generických typů |
| `FindSubtypesOf(Type)` | Najde všechny neabstraktní podtypy zadaného základního typu |
| `FindImplementationsOf(Type)` | Najde všechny neabstraktní typy implementující zadané rozhraní |

## Tok registrace a dotazů

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ Získat ITypeRegistry ze ServiceLocator                   │
│  ├─ RegisterType(typeof(GreetingTool))                       │
│  ├─ RegisterType(typeof(FarewellTool))                       │
│  └─ RegisterType(typeof(StatusTool))                         │
│                                                              │
│  Alternativa: RegisterFromAssembly                           │
│  └─ RegisterFromAssembly(assembly, typeof(DemoTool))         │
│     → zaregistrovat všechny podtypy DemoTool najednou        │
│                                                              │
│  OnStart                                                     │
│  ├─ FindSubtypesOf(typeof(DemoTool))                         │
│  └─ Iterovat výsledky → GreetingTool, FarewellTool, …       │
└──────────────────────────────────────────────────────────────┘
```

## Použití RegisterFromAssembly

`RegisterFromAssembly` proskenuje sestavení a zaregistruje všechny neabstraktní podtypy zadaného základního typu:

```csharp
_registry.RegisterFromAssembly(
    typeof(TypeRegistryUsagePlugin).Assembly,  // sestavení k proskenování
    typeof(DemoTool)                            // registrovat pouze podtypy DemoTool
);
```

To je ekvivalentní volání `RegisterType` pro každý podtyp zvlášť, ale je stručnější, když plugin definuje mnoho typů sdílejících společnou základní třídu.

## Tato demo

> **⚠️ Důležité:** `DemoTool` je **vlastní typ definovaný výhradně pro toto demo**, aby ukázal registraci a dotazy přes `ITypeRegistry`. **Nemá žádnou souvislost** se systémovým rozhraním `ITool` (`SiliconLife.Collective.ITool`) používaným pro registraci AI nástrojů. Název „Tool" je náhodný — jakákoliv vlastní hierarchie tříd by fungovala stejně.

| Třída | Role |
|-------|------|
| `DemoTool` | Vlastní abstraktní základní třída — kotva registrace (nesouvisí s `ITool`) |
| `GreetingTool` | Konkrétní podtyp zaregistrovaný v `OnLoad` |
| `FarewellTool` | Konkrétní podtyp zaregistrovaný v `OnLoad` |
| `StatusTool` | Konkrétní podtyp zaregistrovaný v `OnLoad` |
| `TypeRegistryUsagePlugin` | Implementace `IPlugin` — registrace a dotazy typů |

## Bezpečnostní poznámka

`ITypeRegistry` je součástí modelu řízeného přístupu k zabezpečení. Pluginy **nesmí** používat `AppDomain.CurrentDomain.GetAssemblies()` nebo `Assembly.GetTypes()` k objevování typů — musí používat `ITypeRegistry`. Viz[dokumentace zabezpečení](../../docs/cs-CZ/security.md).
