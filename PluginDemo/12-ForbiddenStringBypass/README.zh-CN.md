# PluginDemo-12: 禁止的字符串反射绕过反例

## 概述

本插件演示了 SiliconLife 插件系统中**被禁止**的基于字符串的反射绕过尝试。它展示了为什么字符串拼接、插值、编码和其他混淆技术**无法**绕过 PluginLoader 的 #US（用户字符串）堆扫描——**最后一道防线**。

## 什么是 #US 堆？

在 .NET PE（可移植可执行文件）元数据中，**#US（User String）堆**存储了所有 `ldstr` IL 指令使用的字符串字面量操作数。每当你在 C# 代码中编写一个字符串字面量，编译器就会将其存储在此堆中。

```
C# 源码:    string s = "System.IO.File";
    ↓ 编译
IL 代码:    ldstr "System.IO.File"    ← 引用 #US 堆中的 token
    ↓ PluginLoader 扫描
#US 堆:     [..., "System.IO.File", ...]  ← 被前缀匹配捕获！
```

PluginLoader 的 `ScanUserStrings()` 方法遍历 #US 堆中的**每一个条目**，检查是否有任何字符串以禁止前缀开头。

## 禁止的字符串前缀

以下前缀在 #US 堆中被发现时会触发 `[ILString]` 违规：

| 前缀 | 类别 |
|------|------|
| `System.IO.` | 文件系统类型 |
| `System.Net.Http` | HTTP 客户端 |
| `System.Net.WebSockets` | WebSocket |
| `System.Net.Sockets` | 原始套接字 |
| `System.Net.Mail` | SMTP |
| `System.Net.NetworkInformation` | 网络探测 |
| `System.Net.Security` | SslStream |
| `System.Diagnostics.Process` | 进程/命令行 |
| `Microsoft.CodeAnalysis` | Roslyn 编译器 |
| `System.Reflection.Emit` | IL 发射 |
| `System.Runtime.Loader` | AssemblyLoadContext |
| `System.CodeDom.Compiler` | 旧版 CodeDom |
| `Microsoft.Win32` | Windows 注册表 |

## 演示的违规操作

### 违规 1：直接类型名称字符串

```csharp
// ❌ 禁止 — 完整字符串在 #US 堆中
Type? fileType = Type.GetType("System.IO.File, System.Runtime");
```

**违规**：`[ILString] "System.IO.File, System.Runtime" matches forbidden prefix "System.IO."`

### 违规 2：字符串拼接（编译时）

```csharp
// ❌ 禁止 — 编译器将 const+const 折叠为一个 #US 条目
const string ns = "System.Net.Http";
const string typeName = ".HttpClient";
const string assembly = ", System.Net.Http";
Type? type = Type.GetType(ns + typeName + assembly);
// 各个部分和折叠后的结果都在 #US 堆中！
```

**违规**：`[ILString] "System.Net.Http.HttpClient, System.Net.Http" matches forbidden prefix "System.Net.Http"`

### 违规 3：字符串插值

```csharp
// ❌ 禁止 — 字面量部分存储在 #US 堆中
string className = "FileStream";
string fullName = $"System.IO.{className}, System.Runtime";
// "System.IO." 本身就匹配前缀！
```

**违规**：`[ILString] "System.IO." matches forbidden prefix "System.IO."`

### 违规 4：Const 字段

```csharp
// ❌ 禁止 — const 值在使用处内联 → 出现在 #US 堆中
private const string ProcessType = "System.Diagnostics.Process";
private const string AssemblyName = ", System.Runtime";
Type? type = Type.GetType(ProcessType + AssemblyName);
```

**违规**：`[ILString] "System.Diagnostics.Process" matches forbidden prefix "System.Diagnostics.Process"`

### 违规 5：部分字符串片段

```csharp
// ❌ 禁止 — 每个部分是独立的 ldstr，单独扫描
string part1 = "System.Reflection.Emit";
string part2 = ".AssemblyBuilder";
string fullType = part1 + part2;
// "System.Reflection.Emit" 本身就匹配前缀！
```

**违规**：`[ILString] "System.Reflection.Emit" matches forbidden prefix "System.Reflection.Emit"`

### 违规 6：多个禁止目标

```csharp
// ❌ 禁止 — 整个程序集中的所有字符串都会被扫描
string tcp = "System.Net.Sockets.TcpClient";
string registry = "Microsoft.Win32.Registry";
string loader = "System.Runtime.Loader.AssemblyLoadContext";
```

**多个违规** — 扫描器不会在第一个匹配处停止。

## 为什么混淆技术全部失败

| 技术 | 为什么失败 |
|------|-----------|
| Const 拼接 | 编译器折叠为单个 #US 条目 |
| 字符串插值 | 字面量部分存储在 #US 堆中 |
| Const 字段 | 值在使用处内联 → 出现在 #US |
| 拆分为变量 | 每个 `ldstr` 操作数独立扫描 |
| Base64 编码 | 解码需要运行时方法，但 `Type.GetType` 被 MemberRef 拦截 |
| 字符数组构建 | 不产生 `ldstr`，但 `Type.GetType` 仍被 MemberRef 拦截 |
| XOR 加密 | 加密字符串在 #US 中不可读，但解密 + `Type.GetType` = MemberRef 拦截 |
| 反转字符串 | 反转后的字面量可能仍匹配前缀，且 reverse + GetType = MemberRef 拦截 |

**核心洞察**：#US 扫描拦截**字符串**。MemberRef 扫描拦截**方法**。要动态加载类型，你需要两者兼备。PluginLoader 独立地拦截两者。

## 完整防御链

PluginLoader 的五步扫描构建了坚不可摧的防御：

| 步骤 | 机制 | 捕获内容 |
|------|------|---------|
| 1 | TypeRef 表 | 对禁止类型的直接引用 |
| 2 | ExportedType 表 | 禁止命名空间的转发类型 |
| 3 | MemberRef 表 | 对 `Type.GetType`、`Assembly.Load`、`Activator.CreateInstance` 的调用 |
| 4 | 不安全标记 | `[DllImport]`、unsafe 块、PinvokeImpl 标志 |
| **5** | **#US 堆扫描** | **匹配禁止前缀的字符串常量（本示例）** |

第 5 步是**最后一道防线**，因为：
- 第 1-4 步捕获**主动代码**（类型、方法、特性）
- 第 5 步捕获**被动数据**（可能被用于绕过的字符串）
- 即使 MemberRef 扫描遗漏了某个未知方法，禁止的类型名称字符串必须存在于程序集中 → #US 扫描捕获它

## 权衡

#US 堆扫描可能产生**轻微误报**：
- 日志消息：`logger.Info("Failed to connect to System.Net.Http endpoint")` → 被标记
- 文档字符串：编译到 XML 文档的注释不在 #US 堆中（安全）
- `nameof()` 表达式：不匹配前缀的编译时字符串是安全的

**误报解决方案**：重新表述字符串字面量以避免禁止前缀。

## 与其他示例的对比

| 示例 | 焦点 | 关系 |
|------|------|------|
| **10-ForbiddenReflection** | 禁止的反射方法（MemberRef 扫描） | 捕获方法 |
| **11-ForbiddenPInvoke** | 禁止的 P/Invoke 和 unsafe 代码 | 不同威胁向量 |
| **12-ForbiddenStringBypass** | 基于字符串的绕过尝试（本示例） | 捕获字符串 |
| **02-TypeRegistryUsage** | 安全的类型发现 | 正确替代方案 |
| **03-ObjectFactoryUsage** | 安全的对象创建 | 正确替代方案 |

## 最佳实践

1. **避免所有字符串中的禁止前缀** — 包括日志消息和编译到 IL 的注释
2. **使用 ITypeRegistry/IObjectFactory** — 字符串反射的安全替代方案
3. **理解拆分字符串无济于事** — 每个片段被独立扫描
4. **运行时构造（char[]）绕过 #US** — 但被 MemberRef 扫描的 Type.GetType 拦截
5. **纵深防御有效** — 即使绕过一层，其他层仍能捕获你

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

- **10-ForbiddenReflection**：禁止的反射方法（MemberRef 扫描）
- **11-ForbiddenPInvoke**：禁止的 P/Invoke 和 unsafe 代码
- **02-TypeRegistryUsage**：ITypeRegistry 正确用法
- **03-ObjectFactoryUsage**：IObjectFactory 正确用法
