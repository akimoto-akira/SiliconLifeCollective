# ITypeRegistry 注册与查询示例

演示 `ITypeRegistry` 的注册与查找功能：在 `OnLoad` 中注册自定义类型，在 `OnStart` 中通过 `FindSubtypesOf` 发现它们。

## ITypeRegistry 接口全貌

`ITypeRegistry` 替代了 `AppDomain.CurrentDomain.GetAssemblies()` 反射扫描。插件在 `IPlugin.OnLoad` 中显式注册其暴露的类型，运行时只从注册表查找类型。

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

### 方法说明

| 方法 | 说明 |
|------|------|
| `RegisterType(Type)` | 注册单个类型 |
| `RegisterTypes(IEnumerable<Type>)` | 批量注册多个类型 |
| `RegisterFromAssembly(Assembly, Type)` | 从指定程序集中注册所有 `baseType` 的非抽象子类型 |
| `FindType(string)` | 按全名查找类型；支持泛型类型名称解析 |
| `FindSubtypesOf(Type)` | 查找指定基类型的所有非抽象子类型 |
| `FindImplementationsOf(Type)` | 查找实现指定接口的所有非抽象类型 |

## 注册与查询流程

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ 从 ServiceLocator 获取 ITypeRegistry                     │
│  ├─ RegisterType(typeof(GreetingTool))                       │
│  ├─ RegisterType(typeof(FarewellTool))                       │
│  └─ RegisterType(typeof(StatusTool))                         │
│                                                              │
│  替代方案：RegisterFromAssembly                               │
│  └─ RegisterFromAssembly(assembly, typeof(DemoTool))         │
│     → 一次性注册所有 DemoTool 子类型                          │
│                                                              │
│  OnStart                                                     │
│  ├─ FindSubtypesOf(typeof(DemoTool))                         │
│  └─ 遍历结果 → GreetingTool, FarewellTool, StatusTool        │
└──────────────────────────────────────────────────────────────┘
```

## RegisterFromAssembly 用法

`RegisterFromAssembly` 扫描程序集，注册所有指定基类型的非抽象子类型：

```csharp
_registry.RegisterFromAssembly(
    typeof(TypeRegistryUsagePlugin).Assembly,  // 要扫描的程序集
    typeof(DemoTool)                            // 只注册 DemoTool 子类型
);
```

这与逐个调用 `RegisterType` 等效，但当插件定义了多个共享基类的类型时更为简洁。

## 本示例

> **⚠️ 重要：** `DemoTool` 是**仅为本示例定义的自定义类型**，用于演示 `ITypeRegistry` 的注册与查询功能。它与系统中用于 AI 工具注册的 `ITool` 接口（`SiliconLife.Collective.ITool`）**毫无关系**。名称中的"Tool"纯属巧合——任何自定义类层次结构都可以用相同方式工作。

| 类 | 角色 |
|----|------|
| `DemoTool` | 自定义抽象基类 — 注册锚点（与 `ITool` 无关） |
| `GreetingTool` | 在 `OnLoad` 中注册的具体子类型 |
| `FarewellTool` | 在 `OnLoad` 中注册的具体子类型 |
| `StatusTool` | 在 `OnLoad` 中注册的具体子类型 |
| `TypeRegistryUsagePlugin` | `IPlugin` 实现 — 注册并查询类型 |

## 安全说明

`ITypeRegistry` 是受控访问安全模型的一部分。插件**不得**使用 `AppDomain.CurrentDomain.GetAssemblies()` 或 `Assembly.GetTypes()` 来发现类型，必须通过 `ITypeRegistry` 进行。详见[安全文档](../../docs/zh-CN/security.md)。
