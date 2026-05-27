# Demo IObjectFactory – Registo e criação de instâncias

Demonstra o registo e a criação de instâncias com `IObjectFactory`: registar tipos com `RegisterAutoFactory` em `OnLoad`, criar instâncias com `CreateInstance` em `OnStart`.

## Visão geral da interface IObjectFactory

`IObjectFactory` substitui `Activator.CreateInstance()`. Os plugins registam delegados de fábrica em `IPlugin.OnLoad`, e o runtime cria instâncias apenas através de delegados registados, impedindo a instanciação arbitrária de tipos.

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

### Resumo dos métodos

| Método | Descrição |
|--------|-----------|
| `RegisterFactory(Type, Func)` | Regista um delegado de fábrica personalizado para um tipo |
| `RegisterFactory<T>(Func)` | Versão genérica de `RegisterFactory` |
| `RegisterAutoFactory(Type)` | Analisa automaticamente os construtores do tipo e regista uma fábrica |
| `RegisterAutoFactoryFromAssembly(Assembly, Type)` | Regista automaticamente fábricas para todos os subtipos não abstratos num assembly |
| `CreateInstance(Type, args)` | Cria uma instância usando uma fábrica registada (não genérica) |
| `CreateInstance<T>(args)` | Cria uma instância usando uma fábrica registada (genérica) |
| `IsRegistered(Type)` | Verifica se uma fábrica está registada para um tipo |

## Porque IObjectFactory substitui Activator.CreateInstance

`Activator.CreateInstance` permite a instanciação arbitrária de tipos, o que é um risco de segurança num sistema de plugins. `IObjectFactory` impõe um modelo de lista branca:

- Apenas tipos com uma **fábrica registada** podem ser instanciados
- As fábricas são registadas explicitamente em `OnLoad`, o host tem controlo total
- `RegisterAutoFactory` é um método de conveniência que analisa construtores mas ainda requer registo

```
❌ Activator.CreateInstance(typeof(SomeType))     → risco de segurança
✅ factory.CreateInstance(typeof(SomeType))         → apenas tipos registados
✅ factory.CreateInstance<SomeType>()               → método genérico conveniente
```

## Como funciona RegisterAutoFactory

`RegisterAutoFactory` inspeciona os construtores do tipo e gera um delegado de fábrica:

1. **Sem argumentos** → chama o construtor sem parâmetros
2. **Com argumentos** → corresponde aos parâmetros do construtor por tipo, recua para o construtor sem parâmetros
3. **Tipos abstratos/interfaces** → rejeitados com aviso

## Fluxo de registo e criação

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ Obter IObjectFactory do ServiceLocator                   │
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

> **⚠️ Nota:** `SimpleService` e `ConfiguredService` são **tipos personalizados definidos exclusivamente para esta demo**. Não estão relacionados com quaisquer interfaces de serviço do sistema.

| Classe | Papel |
|--------|-------|
| `SimpleService` | Tipo de demo, construtor sem parâmetros |
| `ConfiguredService` | Tipo de demo, construtor parametrizado `(string name)` |
| `ObjectFactoryUsagePlugin` | Implementação `IPlugin` — regista fábricas e cria instâncias |

## Nota de segurança

`IObjectFactory` faz parte do modelo de segurança de acesso controlado. Os plugins **não devem** usar `Activator.CreateInstance` para criar objetos — devem registar fábricas e usar `CreateInstance`. Consulte a [documentação de segurança](../../docs/pt-PT/security.md).
