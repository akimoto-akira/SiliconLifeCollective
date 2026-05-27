# Demo IObjectFactory – Rejestracja i tworzenie instancji

Demonstruje rejestrację i tworzenie instancji z `IObjectFactory`: rejestruj typy z `RegisterAutoFactory` w `OnLoad`, twórz instancje z `CreateInstance` w `OnStart`.

## Przegląd interfejsu IObjectFactory

`IObjectFactory` zastępuje `Activator.CreateInstance()`. Pluginy rejestrują delegaty fabryk w `IPlugin.OnLoad`, a środowisko uruchomieniowe tworzy instancje tylko przez zarejestrowane delegaty, zapobiegając arbitralnej instancjacji typów.

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

### Podsumowanie metod

| Metoda | Opis |
|--------|------|
| `RegisterFactory(Type, Func)` | Rejestruje niestandardowy delegat fabryki dla typu |
| `RegisterFactory<T>(Func)` | Ogólna wersja `RegisterFactory` |
| `RegisterAutoFactory(Type)` | Automatycznie analizuje konstruktory typu i rejestruje fabrykę |
| `RegisterAutoFactoryFromAssembly(Assembly, Type)` | Automatycznie rejestruje fabryki dla wszystkich nieabstrakcyjnych podtypów w assembly |
| `CreateInstance(Type, args)` | Tworzy instancję używając zarejestrowanej fabryki (nieogólna) |
| `CreateInstance<T>(args)` | Tworzy instancję używając zarejestrowanej fabryki (ogólna) |
| `IsRegistered(Type)` | Sprawdza czy fabryka jest zarejestrowana dla typu |

## Dlaczego IObjectFactory zastępuje Activator.CreateInstance

`Activator.CreateInstance` pozwala na arbitralną instancjację typów, co stanowi ryzyko bezpieczeństwa w systemie pluginów. `IObjectFactory` wymusza model białej listy:

- Tylko typy z **zarejestrowaną fabryką** mogą być instancjonowane
- Fabryki są rejestrowane jawnie w `OnLoad`, host ma pełną kontrolę
- `RegisterAutoFactory` to metoda ułatwiająca analizująca konstruktory, ale rejestracja jest obowiązkowa

```
❌ Activator.CreateInstance(typeof(SomeType))     → ryzyko bezpieczeństwa
✅ factory.CreateInstance(typeof(SomeType))         → tylko zarejestrowane typy
✅ factory.CreateInstance<SomeType>()               → ogólna metoda ułatwiająca
```

## Jak działa RegisterAutoFactory

`RegisterAutoFactory` bada konstruktory typu i generuje delegat fabryki:

1. **Bez argumentów** → wywołuje konstruktor bezparametrowy
2. **Z argumentami** → dopasowuje parametry konstruktora według typu, powrót do konstruktora bezparametrowego
3. **Typy abstrakcyjne/interfejsy** → odrzucane z ostrzeżeniem

## Przepływ rejestracji i tworzenia

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ Pobierz IObjectFactory z ServiceLocator                  │
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

## To demo

> **⚠️ Uwaga:** `SimpleService` i `ConfiguredService` to **niestandardowe typy zdefiniowane wyłącznie dla tego demo**. Nie są powiązane z żadnymi interfejsami usług systemowych.

| Klasa | Rola |
|-------|------|
| `SimpleService` | Typ demo, konstruktor bezparametrowy |
| `ConfiguredService` | Typ demo, konstruktor sparametryzowany `(string name)` |
| `ObjectFactoryUsagePlugin` | Implementacja `IPlugin` — rejestruje fabryki i tworzy instancje |

## Uwaga dotycząca bezpieczeństwa

`IObjectFactory` jest częścią modelu bezpieczeństwa kontrolowanego dostępu. Pluginy **nie mogą** używać `Activator.CreateInstance` do tworzenia obiektów — muszą rejestrować fabryki i używać `CreateInstance`. Zobacz [dokumentację bezpieczeństwa](../../docs/pl-PL/security.md).
