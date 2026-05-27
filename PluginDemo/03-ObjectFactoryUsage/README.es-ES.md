# Demo de IObjectFactory – Registro y creación de instancias

Demuestra el registro y la creación de instancias con `IObjectFactory`: registrar tipos con `RegisterAutoFactory` en `OnLoad`, crear instancias con `CreateInstance` en `OnStart`.

## Resumen de la interfaz IObjectFactory

`IObjectFactory` reemplaza `Activator.CreateInstance()`. Los plugins registran delegados de fábrica en `IPlugin.OnLoad`, y el runtime solo crea instancias a través de delegados registrados, previniendo la instanciación arbitraria de tipos.

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

### Resumen de métodos

| Método | Descripción |
|--------|-------------|
| `RegisterFactory(Type, Func)` | Registra un delegado de fábrica personalizado para un tipo |
| `RegisterFactory<T>(Func)` | Versión genérica de `RegisterFactory` |
| `RegisterAutoFactory(Type)` | Analiza automáticamente los constructores del tipo y registra una fábrica |
| `RegisterAutoFactoryFromAssembly(Assembly, Type)` | Registra automáticamente fábricas para todos los subtipos no abstractos en un ensamblado |
| `CreateInstance(Type, args)` | Crea una instancia usando una fábrica registrada (no genérica) |
| `CreateInstance<T>(args)` | Crea una instancia usando una fábrica registrada (genérica) |
| `IsRegistered(Type)` | Verifica si hay una fábrica registrada para un tipo |

## Por qué IObjectFactory reemplaza Activator.CreateInstance

`Activator.CreateInstance` permite la instanciación arbitraria de tipos, lo cual es un riesgo de seguridad en un sistema de plugins. `IObjectFactory` impone un modelo de lista blanca:

- Solo los tipos con una **fábrica registrada** pueden ser instanciados
- Las fábricas se registran explícitamente en `OnLoad`, el host tiene control total
- `RegisterAutoFactory` es un método conveniente que analiza constructores pero aún requiere registro

```
❌ Activator.CreateInstance(typeof(SomeType))     → riesgo de seguridad
✅ factory.CreateInstance(typeof(SomeType))         → solo tipos registrados
✅ factory.CreateInstance<SomeType>()               → método genérico conveniente
```

## Cómo funciona RegisterAutoFactory

`RegisterAutoFactory` inspecciona los constructores del tipo y genera un delegado de fábrica:

1. **Sin argumentos** → llama al constructor sin parámetros
2. **Con argumentos** → coincide con los parámetros del constructor por tipo, recurre al constructor sin parámetros si no hay coincidencia
3. **Tipos abstractos/interfaces** → rechazados con advertencia

## Flujo de registro y creación

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ Obtener IObjectFactory desde ServiceLocator              │
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

## Esta demo

> **⚠️ Nota:** `SimpleService` y `ConfiguredService` son **tipos personalizados definidos únicamente para esta demo**. No están relacionados con ninguna interfaz de servicio del sistema.

| Clase | Rol |
|-------|-----|
| `SimpleService` | Tipo de demo, constructor sin parámetros |
| `ConfiguredService` | Tipo de demo, constructor parametrizado `(string name)` |
| `ObjectFactoryUsagePlugin` | Implementación `IPlugin` — registra fábricas y crea instancias |

## Nota de seguridad

`IObjectFactory` es parte del modelo de seguridad de acceso controlado. Los plugins **no deben** usar `Activator.CreateInstance` para crear objetos — deben registrar fábricas y usar `CreateInstance`. Consulte la [documentación de seguridad](../../docs/es-ES/security.md).
