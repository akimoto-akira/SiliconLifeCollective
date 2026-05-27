# PluginDemo-10: 禁止的反射操作反例

## 概述

本插件演示了 SiliconLife 插件系統中**被禁止**的反射操作。它作為反例參考，展示哪些操作不可做，並為每個違規提供正確的替代方案。

## 為什麼反射是核心威脅？

反射繞過是 PluginLoader 安全掃描面臨的**最關鍵威脅**。雖然 TypeRef 掃描能在編譯時捕獲直接類型引用，但反射方法可以在**運行時**使用字串解析類型——完全不可見於靜態元資料掃描。

如果插件能調用 `Type.GetType("System.IO.File, System.Runtime")`，它就能存取任何禁止的類型，而 PE 元資料的 TypeRef 表中完全看不到該引用。

## 哪些方法被禁止？

所有禁止的方法透過 **MemberRef 掃描**檢測（不是命名空間或類型級別的攔截）：

| 禁止的方法 | 簽名 | 威脅 |
|-----------|------|------|
| `Type.GetType` | `System.Type::GetType(System.String)` | 運行時按名稱解析任意類型 |
| `Activator.CreateInstance` | `System.Activator::CreateInstance(...)` | 實例化任意類型 |
| `Activator.CreateInstanceFrom` | `System.Activator::CreateInstanceFrom(...)` | 從 DLL 路徑建立實例 |
| `Assembly.Load` | `System.Reflection.Assembly::Load(...)` | 按名稱/位元組載入組件 |
| `Assembly.LoadFile` | `System.Reflection.Assembly::LoadFile(...)` | 從磁碟載入組件 |
| `Assembly.LoadFrom` | `System.Reflection.Assembly::LoadFrom(...)` | 從路徑載入組件 |
| `Assembly.UnsafeLoadFrom` | `System.Reflection.Assembly::UnsafeLoadFrom(...)` | 無安全檢查載入 |
| `Assembly.LoadWithPartialName` | `System.Reflection.Assembly::LoadWithPartialName(...)` | 按部分名稱載入 |
| `Assembly.ReflectionOnlyLoad` | `System.Reflection.Assembly::ReflectionOnlyLoad(...)` | 僅反射載入 |
| `Assembly.GetType` | `System.Reflection.Assembly::GetType(System.String)` | 基於字串的類型解析 |

## 哪些是安全的？

並非所有反射都被禁止。以下模式是**安全的**，因為它們引用的是編譯時已知的類型：

| 安全模式 | 示例 | 為什麼安全 |
|---------|------|-----------|
| `typeof(X).Assembly` | `typeof(MyPlugin).Assembly` | 類型在編譯時已知，可見於 TypeRef |
| `typeof(X).GetProperties()` | `typeof(MyData).GetProperties()` | 檢查已知類型，不引入新類型 |
| `typeof(X).GetMethods()` | `typeof(IPlugin).GetMethods()` | 對已知類型的成員檢查 |
| 泛型約束 | `FindSubtypesOf(typeof(BaseTool))` | 泛型參數是編譯時類型 |
| `nameof()` | `nameof(MyClass.MyMethod)` | 編譯時字串，無運行時解析 |

**關鍵區別：**
- `typeof(X).Assembly` → **安全**（編譯時引用，被 PluginLoader 掃描）
- `Assembly.Load("X")` → **禁止**（運行時字串，繞過所有掃描）

## 如何安全替代反射？

### 使用 ITypeRegistry（替代 Type.GetType + AppDomain 掃描）

```csharp
// ❌ 禁止：運行時透過字串解析類型
Type? type = Type.GetType("MyNamespace.MyClass, MyAssembly");

// ✅ 正確：使用 ITypeRegistry 查找已註冊類型
Type? type = typeRegistry.FindType("MyNamespace.MyClass");
// 只有在 OnLoad 期間註冊的類型才能被發現
```

**ITypeRegistry 提供的能力：**
1. **FindType(string)**：按全名查找類型（僅從已註冊類型中）
2. **FindSubtypesOf(Type)**：查找指定基類的所有非抽象子類型
3. **FindImplementationsOf(Type)**：查找介面的所有實現
4. **RegisterType(Type)**：在 OnLoad 中註冊單個類型
5. **RegisterFromAssembly(Assembly, Type)**：從組件註冊所有子類型

### 使用 IObjectFactory（替代 Activator.CreateInstance）

```csharp
// ❌ 禁止：建立任意實例
object? instance = Activator.CreateInstance(someType);

// ✅ 正確：使用 IObjectFactory 透過已註冊工廠建立
var instance = objectFactory.CreateInstance<MyService>();
// 只有註冊了工廠的類型才能被實例化
```

**IObjectFactory 提供的能力：**
1. **RegisterAutoFactory(Type)**：透過分析建構函數自動註冊工廠
2. **RegisterFactory&lt;T&gt;(Func)**：註冊自訂工廠委託
3. **CreateInstance(Type, args)**：使用已註冊工廠建立實例
4. **CreateInstance&lt;T&gt;(args)**：CreateInstance 的泛型版本
5. **IsRegistered(Type)**：檢查是否為某類型註冊了工廠

## 演示的違規操作

本插件展示了 5 種常見的反射違規：

### 違規 1：Type.GetType(string)

```csharp
// ❌ 禁止
Type? fileType = Type.GetType("System.IO.File, System.Runtime");
var method = fileType?.GetMethod("ReadAllText");
method?.Invoke(null, new object[] { "secret.txt" });

// ✅ 正確
Type? myType = typeRegistry.FindType("MyPlugin.MyCustomType");
```

**被攔截的 MemberRef**：`System.Type::GetType(System.String)`

### 違規 2：Activator.CreateInstance

```csharp
// ❌ 禁止
Type? httpClientType = Type.GetType("System.Net.Http.HttpClient, System.Net.Http");
object? client = Activator.CreateInstance(httpClientType!);

// ✅ 正確
var instance = objectFactory.CreateInstance<MyService>();
```

**被攔截的 MemberRef**：`System.Activator::CreateInstance`

### 違規 3：Assembly.Load

```csharp
// ❌ 禁止
Assembly asm = Assembly.Load("System.Net.Http");
Type? httpType = asm.GetType("System.Net.Http.HttpClient");
object? client = Activator.CreateInstance(httpType!);

// ✅ 正確
Assembly myAsm = typeof(MyPlugin).Assembly;  // 安全：編譯時已知
Type? type = typeRegistry.FindType("MyPlugin.SomeType");
```

**被攔截的 MemberRef**：`System.Reflection.Assembly::Load(System.String)`

### 違規 4：Assembly.LoadFile / LoadFrom

```csharp
// ❌ 禁止
Assembly asm = Assembly.LoadFile(@"C:\malware\evil.dll");
Assembly asm2 = Assembly.LoadFrom(@"\\network\share\trojan.dll");

// ✅ 正確
// 所有依賴必須放在插件目錄中由 PluginLoader 掃描。
// 在 OnLoad 中使用 ITypeRegistry.RegisterFromAssembly 註冊自己的組件。
```

**被攔截的 MemberRef**：`System.Reflection.Assembly::LoadFile(System.String)` / `LoadFrom(System.String)`

### 違規 5：Assembly.GetType(string)

```csharp
// ❌ 禁止
Assembly runtime = typeof(object).Assembly;
Type? processType = runtime.GetType("System.Diagnostics.Process");

// ✅ 正確
Type? safeType = typeRegistry.FindType("MyPlugin.MySafeType");
// 禁止的類型永遠不會被註冊，所以永遠找不到
```

**被攔截的 MemberRef**：`System.Reflection.Assembly::GetType(System.String)`

## PluginLoader 安全機制

當 PluginLoader 掃描此插件時：

1. **MemberRef 掃描**：檢測對禁止方法的呼叫（`Type.GetType`、`Activator.CreateInstance`、`Assembly.Load` 等）
2. **TypeRef 掃描**：檢測對禁止類型的直接引用（輔助檢查）
3. **IL 字串掃描**：檢測匹配禁止類型模式的字串常量（縱深防禦）
4. **拒絕**：插件在載入期間被拒絕，並給出列出所有違規的詳細錯誤資訊

**多層防禦：**
- **第 1 層（TypeRef）**：捕獲元資料中的直接類型引用
- **第 3 層（MemberRef）**：捕獲對禁止方法的呼叫（本示例重點）
- **第 5 層（#US 堆）**：捕獲用於運行時類型解析的字串常量

## 為什麼 typeof(X).Assembly 安全而 Assembly.Load 不安全

| 操作 | 可見性 | 安全性 |
|------|--------|--------|
| `typeof(X).Assembly` | 類型 X 在 TypeRef 表中 → PluginLoader 掃描它 | ✅ 安全 |
| `Assembly.Load("X")` | 字串 "X" 僅在運行時存在 → TypeRef 掃描不可見 | ❌ 禁止 |
| `obj.GetType()` | 返回現有實例的類型 → 不引入新類型 | ✅ 安全 |
| `Type.GetType("X")` | 從字串解析任意類型 → 繞過 TypeRef | ❌ 禁止 |

## 與其他示例的對比

| 示例 | 焦點 | 安全替代方案 |
|------|------|-------------|
| **10-ForbiddenReflection** | 禁止的反射模式（本示例） | ITypeRegistry + IObjectFactory |
| **11-ForbiddenPInvoke** | 禁止的 P/Invoke 和 unsafe 程式碼 | 硬性禁止（無替代方案） |
| **12-ForbiddenStringBypass** | 基於字串的反射繞過嘗試 | 不適用（演示為什麼掃描字串） |
| **02-TypeRegistryUsage** | ITypeRegistry 正確用法 | — |
| **03-ObjectFactoryUsage** | IObjectFactory 正確用法 | — |

## 最佳實踐

1. **在 OnLoad 中註冊類型**：使用 `ITypeRegistry.RegisterType` / `RegisterFromAssembly`
2. **使用 IObjectFactory 進行動態建立**：永遠不要使用 `Activator.CreateInstance`
3. **typeof(X).Assembly 是你的好幫手**：安全地引用自己的組件
4. **避免基於字串的類型名稱**：它們會觸發 IL 字串掃描並可能被標記
5. **為靜態可發現性設計**：如果 PluginLoader 在元資料中看不到它，就是可疑的

## 文件

- `Plugin.cs` - 反例演示插件
- `README.md` - 英文（English）
- `README.zh-CN.md` - 簡體中文
- `README.zh-HK.md` - 本文件（繁體中文）
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## 相關示例

- **02-TypeRegistryUsage**：ITypeRegistry 正確用法
- **03-ObjectFactoryUsage**：IObjectFactory 正確用法
- **11-ForbiddenPInvoke**：禁止的 P/Invoke 和 unsafe 程式碼
- **12-ForbiddenStringBypass**：基於字串的反射繞過嘗試
