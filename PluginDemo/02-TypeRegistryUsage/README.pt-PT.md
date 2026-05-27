# Demo ITypeRegistry – Registo e consulta

Demonstra o registo e a pesquisa com `ITypeRegistry`: registar tipos personalizados em `OnLoad`, descobri-los com `FindSubtypesOf` em `OnStart`.

## Visão geral da interface ITypeRegistry

`ITypeRegistry` substitui a verificação por reflexão `AppDomain.CurrentDomain.GetAssemblies()`. Os plugins registam explicitamente os seus tipos expostos em `IPlugin.OnLoad`, e o runtime apenas pesquisa tipos no registo.

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

### Resumo dos métodos

| Método | Descrição |
|--------|-----------|
| `RegisterType(Type)` | Regista um único tipo |
| `RegisterTypes(IEnumerable<Type>)` | Regista múltiplos tipos de uma vez |
| `RegisterFromAssembly(Assembly, Type)` | Regista todos os subtipos não abstratos de `baseType` do assembly especificado |
| `FindType(string)` | Encontra um tipo pelo nome completo; suporta resolução de nomes de tipos genéricos |
| `FindSubtypesOf(Type)` | Encontra todos os subtipos não abstratos do tipo base especificado |
| `FindImplementationsOf(Type)` | Encontra todos os tipos não abstratos que implementam a interface especificada |

## Fluxo de registo e consulta

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ Obter ITypeRegistry do ServiceLocator                    │
│  ├─ RegisterType(typeof(GreetingTool))                       │
│  ├─ RegisterType(typeof(FarewellTool))                       │
│  └─ RegisterType(typeof(StatusTool))                         │
│                                                              │
│  Alternativa: RegisterFromAssembly                           │
│  └─ RegisterFromAssembly(assembly, typeof(DemoTool))         │
│     → registar todos os subtipos DemoTool de uma vez         │
│                                                              │
│  OnStart                                                     │
│  ├─ FindSubtypesOf(typeof(DemoTool))                         │
│  └─ Iterar resultados → GreetingTool, FarewellTool, …       │
└──────────────────────────────────────────────────────────────┘
```

## Utilização de RegisterFromAssembly

`RegisterFromAssembly` verifica um assembly e regista todos os subtipos não abstratos do tipo base especificado:

```csharp
_registry.RegisterFromAssembly(
    typeof(TypeRegistryUsagePlugin).Assembly,  // o assembly a verificar
    typeof(DemoTool)                            // registar apenas subtipos DemoTool
);
```

Isto é equivalente a chamar `RegisterType` para cada subtipo individualmente, mas é mais conciso quando um plugin define muitos tipos que partilham uma classe base comum.

## Esta demo

> **⚠️ Importante:** `DemoTool` é um **tipo personalizado definido exclusivamente para esta demo** para demonstrar o registo e a consulta via `ITypeRegistry`. **Não tem qualquer relação** com a interface `ITool` do sistema (`SiliconLife.Collective.ITool`) utilizada para o registo de ferramentas de IA. O nome «Tool» é coincidental — qualquer hierarquia de classes personalizada funcionaria da mesma forma.

| Classe | Papel |
|--------|-------|
| `DemoTool` | Classe base abstrata personalizada — âncora de registo (sem relação com `ITool`) |
| `GreetingTool` | Subtipo concreto registado em `OnLoad` |
| `FarewellTool` | Subtipo concreto registado em `OnLoad` |
| `StatusTool` | Subtipo concreto registado em `OnLoad` |
| `TypeRegistryUsagePlugin` | Implementação `IPlugin` — regista e consulta tipos |

## Nota de segurança

`ITypeRegistry` faz parte do modelo de segurança de acesso controlado. Os plugins **não devem** utilizar `AppDomain.CurrentDomain.GetAssemblies()` ou `Assembly.GetTypes()` para descobrir tipos — devem utilizar `ITypeRegistry`. Consulte a [documentação de segurança](../../docs/pt-PT/security.md).
