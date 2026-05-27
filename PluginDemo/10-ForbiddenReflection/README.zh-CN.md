# PluginDemo-10: 禁止的反射操作反例

## 概述

本插件演示了 SiliconLife 插件系统中**被禁止**的反射操作。它作为反例参考，展示哪些操作不可做，并为每个违规提供正确的替代方案。

## 为什么反射是核心威胁？

反射绕过是 PluginLoader 安全扫描面临的**最关键威胁**。虽然 TypeRef 扫描能在编译时捕获直接类型引用，但反射方法可以在**运行时**使用字符串解析类型——完全不可见于静态元数据扫描。

如果插件能调用 `Type.GetType("System.IO.File, System.Runtime")`，它就能访问任何禁止的类型，而 PE 元数据的 TypeRef 表中完全看不到该引用。

## 哪些方法被禁止？

所有禁止的方法通过 **MemberRef 扫描**检测（不是命名空间或类型级别的拦截）：

| 禁止的方法 | 签名 | 威胁 |
|-----------|------|------|
| `Type.GetType` | `System.Type::GetType(System.String)` | 运行时按名称解析任意类型 |
| `Activator.CreateInstance` | `System.Activator::CreateInstance(...)` | 实例化任意类型 |
| `Activator.CreateInstanceFrom` | `System.Activator::CreateInstanceFrom(...)` | 从 DLL 路径创建实例 |
| `Assembly.Load` | `System.Reflection.Assembly::Load(...)` | 按名称/字节加载程序集 |
| `Assembly.LoadFile` | `System.Reflection.Assembly::LoadFile(...)` | 从磁盘加载程序集 |
| `Assembly.LoadFrom` | `System.Reflection.Assembly::LoadFrom(...)` | 从路径加载程序集 |
| `Assembly.UnsafeLoadFrom` | `System.Reflection.Assembly::UnsafeLoadFrom(...)` | 无安全检查加载 |
| `Assembly.LoadWithPartialName` | `System.Reflection.Assembly::LoadWithPartialName(...)` | 按部分名称加载 |
| `Assembly.ReflectionOnlyLoad` | `System.Reflection.Assembly::ReflectionOnlyLoad(...)` | 仅反射加载 |
| `Assembly.GetType` | `System.Reflection.Assembly::GetType(System.String)` | 基于字符串的类型解析 |

## 哪些是安全的？

并非所有反射都被禁止。以下模式是**安全的**，因为它们引用的是编译时已知的类型：

| 安全模式 | 示例 | 为什么安全 |
|---------|------|-----------|
| `typeof(X).Assembly` | `typeof(MyPlugin).Assembly` | 类型在编译时已知，可见于 TypeRef |
| `typeof(X).GetProperties()` | `typeof(MyData).GetProperties()` | 检查已知类型，不引入新类型 |
| `typeof(X).GetMethods()` | `typeof(IPlugin).GetMethods()` | 对已知类型的成员检查 |
| 泛型约束 | `FindSubtypesOf(typeof(BaseTool))` | 泛型参数是编译时类型 |
| `nameof()` | `nameof(MyClass.MyMethod)` | 编译时字符串，无运行时解析 |

**关键区别：**
- `typeof(X).Assembly` → **安全**（编译时引用，被 PluginLoader 扫描）
- `Assembly.Load("X")` → **禁止**（运行时字符串，绕过所有扫描）

## 如何安全替代反射？

### 使用 ITypeRegistry（替代 Type.GetType + AppDomain 扫描）

```csharp
// ❌ 禁止：运行时通过字符串解析类型
Type? type = Type.GetType("MyNamespace.MyClass, MyAssembly");

// ✅ 正确：使用 ITypeRegistry 查找已注册类型
Type? type = typeRegistry.FindType("MyNamespace.MyClass");
// 只有在 OnLoad 期间注册的类型才能被发现
```

**ITypeRegistry 提供的能力：**
1. **FindType(string)**：按全名查找类型（仅从已注册类型中）
2. **FindSubtypesOf(Type)**：查找指定基类的所有非抽象子类型
3. **FindImplementationsOf(Type)**：查找接口的所有实现
4. **RegisterType(Type)**：在 OnLoad 中注册单个类型
5. **RegisterFromAssembly(Assembly, Type)**：从程序集注册所有子类型

### 使用 IObjectFactory（替代 Activator.CreateInstance）

```csharp
// ❌ 禁止：创建任意实例
object? instance = Activator.CreateInstance(someType);

// ✅ 正确：使用 IObjectFactory 通过已注册工厂创建
var instance = objectFactory.CreateInstance<MyService>();
// 只有注册了工厂的类型才能被实例化
```

**IObjectFactory 提供的能力：**
1. **RegisterAutoFactory(Type)**：通过分析构造函数自动注册工厂
2. **RegisterFactory&lt;T&gt;(Func)**：注册自定义工厂委托
3. **CreateInstance(Type, args)**：使用已注册工厂创建实例
4. **CreateInstance&lt;T&gt;(args)**：CreateInstance 的泛型版本
5. **IsRegistered(Type)**：检查是否为某类型注册了工厂

## 演示的违规操作

本插件展示了 5 种常见的反射违规：

### 违规 1：Type.GetType(string)

```csharp
// ❌ 禁止
Type? fileType = Type.GetType("System.IO.File, System.Runtime");
var method = fileType?.GetMethod("ReadAllText");
method?.Invoke(null, new object[] { "secret.txt" });

// ✅ 正确
Type? myType = typeRegistry.FindType("MyPlugin.MyCustomType");
```

**被拦截的 MemberRef**：`System.Type::GetType(System.String)`

### 违规 2：Activator.CreateInstance

```csharp
// ❌ 禁止
Type? httpClientType = Type.GetType("System.Net.Http.HttpClient, System.Net.Http");
object? client = Activator.CreateInstance(httpClientType!);

// ✅ 正确
var instance = objectFactory.CreateInstance<MyService>();
```

**被拦截的 MemberRef**：`System.Activator::CreateInstance`

### 违规 3：Assembly.Load

```csharp
// ❌ 禁止
Assembly asm = Assembly.Load("System.Net.Http");
Type? httpType = asm.GetType("System.Net.Http.HttpClient");
object? client = Activator.CreateInstance(httpType!);

// ✅ 正确
Assembly myAsm = typeof(MyPlugin).Assembly;  // 安全：编译时已知
Type? type = typeRegistry.FindType("MyPlugin.SomeType");
```

**被拦截的 MemberRef**：`System.Reflection.Assembly::Load(System.String)`

### 违规 4：Assembly.LoadFile / LoadFrom

```csharp
// ❌ 禁止
Assembly asm = Assembly.LoadFile(@"C:\malware\evil.dll");
Assembly asm2 = Assembly.LoadFrom(@"\\network\share\trojan.dll");

// ✅ 正确
// 所有依赖必须放在插件目录中由 PluginLoader 扫描。
// 在 OnLoad 中使用 ITypeRegistry.RegisterFromAssembly 注册自己的程序集。
```

**被拦截的 MemberRef**：`System.Reflection.Assembly::LoadFile(System.String)` / `LoadFrom(System.String)`

### 违规 5：Assembly.GetType(string)

```csharp
// ❌ 禁止
Assembly runtime = typeof(object).Assembly;
Type? processType = runtime.GetType("System.Diagnostics.Process");

// ✅ 正确
Type? safeType = typeRegistry.FindType("MyPlugin.MySafeType");
// 禁止的类型永远不会被注册，所以永远找不到
```

**被拦截的 MemberRef**：`System.Reflection.Assembly::GetType(System.String)`

## PluginLoader 安全机制

当 PluginLoader 扫描此插件时：

1. **MemberRef 扫描**：检测对禁止方法的调用（`Type.GetType`、`Activator.CreateInstance`、`Assembly.Load` 等）
2. **TypeRef 扫描**：检测对禁止类型的直接引用（辅助检查）
3. **IL 字符串扫描**：检测匹配禁止类型模式的字符串常量（纵深防御）
4. **拒绝**：插件在加载期间被拒绝，并给出列出所有违规的详细错误信息

**多层防御：**
- **第 1 层（TypeRef）**：捕获元数据中的直接类型引用
- **第 3 层（MemberRef）**：捕获对禁止方法的调用（本示例重点）
- **第 5 层（#US 堆）**：捕获用于运行时类型解析的字符串常量

## 为什么 typeof(X).Assembly 安全而 Assembly.Load 不安全

| 操作 | 可见性 | 安全性 |
|------|--------|--------|
| `typeof(X).Assembly` | 类型 X 在 TypeRef 表中 → PluginLoader 扫描它 | ✅ 安全 |
| `Assembly.Load("X")` | 字符串 "X" 仅在运行时存在 → TypeRef 扫描不可见 | ❌ 禁止 |
| `obj.GetType()` | 返回现有实例的类型 → 不引入新类型 | ✅ 安全 |
| `Type.GetType("X")` | 从字符串解析任意类型 → 绕过 TypeRef | ❌ 禁止 |

## 与其他示例的对比

| 示例 | 焦点 | 安全替代方案 |
|------|------|-------------|
| **10-ForbiddenReflection** | 禁止的反射模式（本示例） | ITypeRegistry + IObjectFactory |
| **11-ForbiddenPInvoke** | 禁止的 P/Invoke 和 unsafe 代码 | 硬性禁止（无替代方案） |
| **12-ForbiddenStringBypass** | 基于字符串的反射绕过尝试 | 不适用（演示为什么扫描字符串） |
| **02-TypeRegistryUsage** | ITypeRegistry 正确用法 | — |
| **03-ObjectFactoryUsage** | IObjectFactory 正确用法 | — |

## 最佳实践

1. **在 OnLoad 中注册类型**：使用 `ITypeRegistry.RegisterType` / `RegisterFromAssembly`
2. **使用 IObjectFactory 进行动态创建**：永远不要使用 `Activator.CreateInstance`
3. **typeof(X).Assembly 是你的好帮手**：安全地引用自己的程序集
4. **避免基于字符串的类型名称**：它们会触发 IL 字符串扫描并可能被标记
5. **为静态可发现性设计**：如果 PluginLoader 在元数据中看不到它，就是可疑的

## 文件

- `Plugin.cs` - 反例演示插件
- `README.md` - 英文（English）
- `README.zh-CN.md` - 本文件（简体中文）
- `README.zh-HK.md` - 繁體中文
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

## 相关示例

- **02-TypeRegistryUsage**：ITypeRegistry 正确用法
- **03-ObjectFactoryUsage**：IObjectFactory 正确用法
- **11-ForbiddenPInvoke**：禁止的 P/Invoke 和 unsafe 代码
- **12-ForbiddenStringBypass**：基于字符串的反射绕过尝试
