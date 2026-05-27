# Demo de ITypeRegistry – Registro y consulta

Demuestra el registro y la búsqueda con `ITypeRegistry`: registrar tipos personalizados en `OnLoad`, descubrirlos con `FindSubtypesOf` en `OnStart`.

## Resumen de la interfaz ITypeRegistry

`ITypeRegistry` reemplaza el escaneo por reflexión `AppDomain.CurrentDomain.GetAssemblies()`. Los plugins registran explícitamente sus tipos expuestos en `IPlugin.OnLoad`, y el runtime solo busca tipos desde el registro.

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

### Resumen de métodos

| Método | Descripción |
|--------|-------------|
| `RegisterType(Type)` | Registra un solo tipo |
| `RegisterTypes(IEnumerable<Type>)` | Registra múltiples tipos a la vez |
| `RegisterFromAssembly(Assembly, Type)` | Registra todos los subtipos no abstractos de `baseType` desde el ensamblado especificado |
| `FindType(string)` | Busca un tipo por su nombre completo; soporta resolución de nombres de tipos genéricos |
| `FindSubtypesOf(Type)` | Busca todos los subtipos no abstractos del tipo base especificado |
| `FindImplementationsOf(Type)` | Busca todos los tipos no abstractos que implementan la interfaz especificada |

## Flujo de registro y consulta

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ Obtener ITypeRegistry desde ServiceLocator               │
│  ├─ RegisterType(typeof(GreetingTool))                       │
│  ├─ RegisterType(typeof(FarewellTool))                       │
│  └─ RegisterType(typeof(StatusTool))                         │
│                                                              │
│  Alternativa: RegisterFromAssembly                           │
│  └─ RegisterFromAssembly(assembly, typeof(DemoTool))         │
│     → registrar todos los subtipos DemoTool de una vez       │
│                                                              │
│  OnStart                                                     │
│  ├─ FindSubtypesOf(typeof(DemoTool))                         │
│  └─ Iterar resultados → GreetingTool, FarewellTool, …       │
└──────────────────────────────────────────────────────────────┘
```

## Uso de RegisterFromAssembly

`RegisterFromAssembly` escanea un ensamblado y registra todos los subtipos no abstractos del tipo base especificado:

```csharp
_registry.RegisterFromAssembly(
    typeof(TypeRegistryUsagePlugin).Assembly,  // el ensamblado a escanear
    typeof(DemoTool)                            // registrar solo subtipos DemoTool
);
```

Esto equivale a llamar a `RegisterType` para cada subtipo individualmente, pero es más conciso cuando un plugin define muchos tipos que comparten una clase base común.

## Esta demo

> **⚠️ Importante:** `DemoTool` es un **tipo personalizado definido únicamente para esta demo** para demostrar el registro y la consulta de `ITypeRegistry`. No tiene **ninguna relación** con la interfaz `ITool` del sistema (`SiliconLife.Collective.ITool`) utilizada para el registro de herramientas de IA. El nombre «Tool» es coincidental — cualquier jerarquía de clases personalizada funcionaría de la misma manera.

| Clase | Rol |
|-------|-----|
| `DemoTool` | Clase base abstracta personalizada — ancla de registro (sin relación con `ITool`) |
| `GreetingTool` | Subtipo concreto registrado en `OnLoad` |
| `FarewellTool` | Subtipo concreto registrado en `OnLoad` |
| `StatusTool` | Subtipo concreto registrado en `OnLoad` |
| `TypeRegistryUsagePlugin` | Implementación `IPlugin` — registra y consulta tipos |

## Nota de seguridad

`ITypeRegistry` es parte del modelo de seguridad de acceso controlado. Los plugins **no deben** usar `AppDomain.CurrentDomain.GetAssemblies()` o `Assembly.GetTypes()` para descubrir tipos — deben usar `ITypeRegistry` en su lugar. Consulta la [documentación de seguridad](../../docs/es-ES/security.md).
