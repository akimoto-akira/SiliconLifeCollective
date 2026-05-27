# ITypeRegistry 註冊與查詢示例

演示 `ITypeRegistry` 的註冊與查找功能：在 `OnLoad` 中註冊自訂類型，在 `OnStart` 中透過 `FindSubtypesOf` 發現它們。

## ITypeRegistry 介面全貌

`ITypeRegistry` 替代了 `AppDomain.CurrentDomain.GetAssemblies()` 反射掃描。插件在 `IPlugin.OnLoad` 中顯式註冊其暴露的類型，執行時只從註冊表查找類型。

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

### 方法說明

| 方法 | 說明 |
|------|------|
| `RegisterType(Type)` | 註冊單個類型 |
| `RegisterTypes(IEnumerable<Type>)` | 批次註冊多個類型 |
| `RegisterFromAssembly(Assembly, Type)` | 從指定組件中註冊所有 `baseType` 的非抽象子類型 |
| `FindType(string)` | 按全名查找類型；支援泛型類型名稱解析 |
| `FindSubtypesOf(Type)` | 查找指定基類型的所有非抽象子類型 |
| `FindImplementationsOf(Type)` | 查找實作指定介面的所有非抽象類型 |

## 註冊與查詢流程

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ 從 ServiceLocator 取得 ITypeRegistry                     │
│  ├─ RegisterType(typeof(GreetingTool))                       │
│  ├─ RegisterType(typeof(FarewellTool))                       │
│  └─ RegisterType(typeof(StatusTool))                         │
│                                                              │
│  替代方案：RegisterFromAssembly                               │
│  └─ RegisterFromAssembly(assembly, typeof(DemoTool))         │
│     → 一次性註冊所有 DemoTool 子類型                          │
│                                                              │
│  OnStart                                                     │
│  ├─ FindSubtypesOf(typeof(DemoTool))                         │
│  └─ 遍歷結果 → GreetingTool, FarewellTool, StatusTool        │
└──────────────────────────────────────────────────────────────┘
```

## RegisterFromAssembly 用法

`RegisterFromAssembly` 掃描組件，註冊所有指定基類型的非抽象子類型：

```csharp
_registry.RegisterFromAssembly(
    typeof(TypeRegistryUsagePlugin).Assembly,  // 要掃描的組件
    typeof(DemoTool)                            // 只註冊 DemoTool 子類型
);
```

這與逐個呼叫 `RegisterType` 等效，但當插件定義了多個共享基類的類型時更為簡潔。

## 本示例

> **⚠️ 重要：** `DemoTool` 是**僅為本示例定義的自訂類型**，用於演示 `ITypeRegistry` 的註冊與查詢功能。它與系統中用於 AI 工具註冊的 `ITool` 介面（`SiliconLife.Collective.ITool`）**毫無關係**。名稱中的「Tool」純屬巧合——任何自訂類別層次結構都可以用相同方式運作。

| 類別 | 角色 |
|------|------|
| `DemoTool` | 自訂抽象基底類別 — 註冊錨點（與 `ITool` 無關） |
| `GreetingTool` | 在 `OnLoad` 中註冊的具體子類型 |
| `FarewellTool` | 在 `OnLoad` 中註冊的具體子類型 |
| `StatusTool` | 在 `OnLoad` 中註冊的具體子類型 |
| `TypeRegistryUsagePlugin` | `IPlugin` 實作 — 註冊並查詢類型 |

## 安全說明

`ITypeRegistry` 是受控存取安全模型的一部分。插件**不得**使用 `AppDomain.CurrentDomain.GetAssemblies()` 或 `Assembly.GetTypes()` 來發現類型，必須透過 `ITypeRegistry` 進行。詳見[安全文件](../../docs/zh-HK/security.md)。
