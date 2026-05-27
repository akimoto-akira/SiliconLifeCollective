# IObjectFactory 注册与创建示例

演示 `IObjectFactory` 的注册与实例创建功能：在 `OnLoad` 中通过 `RegisterAutoFactory` 注册类型，在 `OnStart` 中通过 `CreateInstance` 创建实例。

## IObjectFactory 接口全貌

`IObjectFactory` 替代了 `Activator.CreateInstance()`。插件在 `IPlugin.OnLoad` 中注册工厂委托，运行时只通过已注册的委托创建实例，防止任意类型实例化。

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

### 方法说明

| 方法 | 说明 |
|------|------|
| `RegisterFactory(Type, Func)` | 为类型注册自定义工厂委托 |
| `RegisterFactory<T>(Func)` | `RegisterFactory` 的泛型版本 |
| `RegisterAutoFactory(Type)` | 自动分析类型的构造函数并注册工厂 |
| `RegisterAutoFactoryFromAssembly(Assembly, Type)` | 为程序集中所有非抽象子类型自动注册工厂 |
| `CreateInstance(Type, args)` | 使用已注册的工厂创建实例（非泛型） |
| `CreateInstance<T>(args)` | 使用已注册的工厂创建实例（泛型） |
| `IsRegistered(Type)` | 检查某类型是否已注册工厂 |

## 为什么 IObjectFactory 替代 Activator.CreateInstance

`Activator.CreateInstance` 允许任意类型实例化，在插件系统中是安全风险。`IObjectFactory` 强制白名单模型：

- 只有**已注册工厂**的类型才能被实例化
- 工厂在 `OnLoad` 中显式注册，宿主拥有完全控制权
- `RegisterAutoFactory` 是便捷方法，虽然自动分析构造函数，但仍需经过注册关卡

```
❌ Activator.CreateInstance(typeof(SomeType))     → 安全风险
✅ factory.CreateInstance(typeof(SomeType))         → 仅限已注册类型
✅ factory.CreateInstance<SomeType>()               → 泛型便捷方法
```

## RegisterAutoFactory 工作原理

`RegisterAutoFactory` 检查类型的构造函数并生成工厂委托：

1. **无参数** → 调用无参构造函数
2. **有参数** → 按类型匹配构造函数参数，无匹配时回退到无参构造
3. **抽象/接口类型** → 被拒绝并输出警告

```
┌──────────────────────────────────────────────────────────────┐
│  RegisterAutoFactory(typeof(SimpleService))                  │
│  → 找到无参构造函数                                          │
│  → factory: args => new SimpleService()                      │
│                                                              │
│  RegisterAutoFactory(typeof(ConfiguredService))              │
│  → 找到构造函数 (string name)                                │
│  → factory: args => new ConfiguredService((string)args[0])   │
└──────────────────────────────────────────────────────────────┘
```

## 注册与创建流程

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ 从 ServiceLocator 获取 IObjectFactory                    │
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

## 本示例

> **⚠️ 注意：** `SimpleService` 和 `ConfiguredService` 是**仅为本示例定义的自定义类型**，与系统中的任何服务接口无关。

| 类 | 角色 |
|----|------|
| `SimpleService` | 演示类型，无参构造函数 |
| `ConfiguredService` | 演示类型，带参构造函数 `(string name)` |
| `ObjectFactoryUsagePlugin` | `IPlugin` 实现 — 注册工厂并创建实例 |

## 安全说明

`IObjectFactory` 是受控访问安全模型的一部分。插件**不得**使用 `Activator.CreateInstance` 创建对象，必须注册工厂并使用 `CreateInstance`。详见[安全文档](../../docs/zh-CN/security.md)。
