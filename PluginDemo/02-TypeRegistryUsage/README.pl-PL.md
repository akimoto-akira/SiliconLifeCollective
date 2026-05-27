# Demo ITypeRegistry – Rejestracja i zapytania

Demonstruje rejestrację i wyszukiwanie za pomocą `ITypeRegistry`: rejestruje typy niestandardowe w `OnLoad`, odkrywa je za pomocą `FindSubtypesOf` w `OnStart`.

## Przegląd interfejsu ITypeRegistry

`ITypeRegistry` zastępuje skanowanie refleksyjne `AppDomain.CurrentDomain.GetAssemblies()`. Wtyczki jawnie rejestrują swoje typy w `IPlugin.OnLoad`, a środowisko uruchomieniowe wyszukuje typy wyłącznie z rejestru.

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

### Podsumowanie metod

| Metoda | Opis |
|--------|------|
| `RegisterType(Type)` | Rejestruje pojedynczy typ |
| `RegisterTypes(IEnumerable<Type>)` | Rejestruje wiele typów naraz |
| `RegisterFromAssembly(Assembly, Type)` | Rejestruje wszystkie nieabstrakcyjne podtypy `baseType` z określonego zestawu |
| `FindType(string)` | Znajduje typ po pełnej nazwie; obsługuje rozwiązywanie nazw typów ogólnych |
| `FindSubtypesOf(Type)` | Znajduje wszystkie nieabstrakcyjne podtypy określonego typu bazowego |
| `FindImplementationsOf(Type)` | Znajduje wszystkie nieabstrakcyjne typy implementujące określony interfejs |

## Przepływ rejestracji i zapytań

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ Pobierz ITypeRegistry z ServiceLocator                   │
│  ├─ RegisterType(typeof(GreetingTool))                       │
│  ├─ RegisterType(typeof(FarewellTool))                       │
│  └─ RegisterType(typeof(StatusTool))                         │
│                                                              │
│  Alternatywa: RegisterFromAssembly                           │
│  └─ RegisterFromAssembly(assembly, typeof(DemoTool))         │
│     → zarejestruj wszystkie podtypy DemoTool za jednym razem │
│                                                              │
│  OnStart                                                     │
│  ├─ FindSubtypesOf(typeof(DemoTool))                         │
│  └─ Iteruj wyniki → GreetingTool, FarewellTool, …           │
└──────────────────────────────────────────────────────────────┘
```

## Użycie RegisterFromAssembly

`RegisterFromAssembly` skanuje zestaw i rejestruje wszystkie nieabstrakcyjne podtypy określonego typu bazowego:

```csharp
_registry.RegisterFromAssembly(
    typeof(TypeRegistryUsagePlugin).Assembly,  // zestaw do przeskanowania
    typeof(DemoTool)                            // rejestruj tylko podtypy DemoTool
);
```

Jest to równoważne wywołaniu `RegisterType` dla każdego podtypu osobno, ale bardziej zwięzłe, gdy wtyczka definiuje wiele typów współdzielących wspólną klasę bazową.

## Ta demo

> **⚠️ Ważne:** `DemoTool` to **typ niestandardowy zdefiniowany wyłącznie dla tego demo**, aby zademonstrować rejestrację i zapytania `ITypeRegistry`. **Nie ma żadnego związku** z systemowym interfejsem `ITool` (`SiliconLife.Collective.ITool`) używanym do rejestracji narzędzi AI. Nazwa „Tool" jest przypadkowa — każda niestandardowa hierarchia klas działałaby tak samo.

| Klasa | Rola |
|-------|------|
| `DemoTool` | Niestandardowa abstrakcyjna klasa bazowa — kotwica rejestracji (niezwiązana z `ITool`) |
| `GreetingTool` | Konkretny podtyp zarejestrowany w `OnLoad` |
| `FarewellTool` | Konkretny podtyp zarejestrowany w `OnLoad` |
| `StatusTool` | Konkretny podtyp zarejestrowany w `OnLoad` |
| `TypeRegistryUsagePlugin` | Implementacja `IPlugin` — rejestracja i zapytania typów |

## Uwaga dotycząca bezpieczeństwa

`ITypeRegistry` jest częścią modelu bezpieczeństwa kontrolowanego dostępu. Wtyczki **nie mogą** używać `AppDomain.CurrentDomain.GetAssemblies()` ani `Assembly.GetTypes()` do odkrywania typów — muszą korzystać z `ITypeRegistry`. Zobacz [dokumentację bezpieczeństwa](../../docs/pl-PL/security.md).
