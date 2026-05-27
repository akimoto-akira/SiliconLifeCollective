# IObjectFactory 註冊與建立示例

演示 `IObjectFactory` 的註冊與實例建立功能：在 `OnLoad` 中透過 `RegisterAutoFactory` 註冊類型，在 `OnStart` 中透過 `CreateInstance` 建立實例。

## IObjectFactory 介面全貌

`IObjectFactory` 替代了 `Activator.CreateInstance()`。插件在 `IPlugin.OnLoad` 中註冊工廠委派，執行時只透過已註冊的委派建立實例，防止任意類型實例化。

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

### 方法說明

| 方法 | 說明 |
|------|------|
| `RegisterFactory(Type, Func)` | 為類型註冊自訂工廠委派 |
| `RegisterFactory<T>(Func)` | `RegisterFactory` 的泛型版本 |
| `RegisterAutoFactory(Type)` | 自動分析類型的建構函式並註冊工廠 |
| `RegisterAutoFactoryFromAssembly(Assembly, Type)` | 為組件中所有非抽象子類型自動註冊工廠 |
| `CreateInstance(Type, args)` | 使用已註冊的工廠建立實例（非泛型） |
| `CreateInstance<T>(args)` | 使用已註冊的工廠建立實例（泛型） |
| `IsRegistered(Type)` | 檢查某類型是否已註冊工廠 |

## 為什麼 IObjectFactory 替代 Activator.CreateInstance

`Activator.CreateInstance` 允許任意類型實例化，在插件系統中是安全風險。`IObjectFactory` 強制白名單模型：

- 只有**已註冊工廠**的類型才能被實例化
- 工廠在 `OnLoad` 中顯式註冊，宿主擁有完全控制權
- `RegisterAutoFactory` 是便捷方法，雖然自動分析建構函式，但仍需經過註冊關卡

```
❌ Activator.CreateInstance(typeof(SomeType))     → 安全風險
✅ factory.CreateInstance(typeof(SomeType))         → 僅限已註冊類型
✅ factory.CreateInstance<SomeType>()               → 泛型便捷方法
```

## RegisterAutoFactory 運作原理

`RegisterAutoFactory` 檢查類型的建構函式並產生工廠委派：

1. **無參數** → 呼叫無參建構函式
2. **有參數** → 按類型匹配建構函式參數，無匹配時回退到無參建構
3. **抽象/介面類型** → 被拒絕並輸出警告

## 註冊與建立流程

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ 從 ServiceLocator 取得 IObjectFactory                    │
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

> **⚠️ 注意：** `SimpleService` 和 `ConfiguredService` 是**僅為本示例定義的自訂類型**，與系統中的任何服務介面無關。

| 類別 | 角色 |
|------|------|
| `SimpleService` | 演示類型，無參建構函式 |
| `ConfiguredService` | 演示類型，帶參建構函式 `(string name)` |
| `ObjectFactoryUsagePlugin` | `IPlugin` 實作 — 註冊工廠並建立實例 |

## 安全說明

`IObjectFactory` 是受控存取安全模型的一部分。插件**不得**使用 `Activator.CreateInstance` 建立物件，必須註冊工廠並使用 `CreateInstance`。詳見[安全文件](../../docs/zh-HK/security.md)。
