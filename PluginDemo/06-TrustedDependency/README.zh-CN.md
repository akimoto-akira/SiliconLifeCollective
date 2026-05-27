# 受信依赖示例

演示使用 `Newtonsoft.Json` —— 一个内部大量使用反射的库 —— 作为受信程序集。PluginLoader 安全扫描器会完全跳过受信程序集，允许插件引用它们而不触发违规。

## TrustedAssemblies 白名单机制

`PluginLoader` 维护一个静态白名单，列出**默认受信**的开源库：

```csharp
private static readonly HashSet<string> TrustedAssemblies = new(StringComparer.Ordinal)
{
    // 序列化库
    "Google.Protobuf",
    "protobuf-net",
    "Newtonsoft.Json",        // ← 本示例使用此库
    "MessagePack",
    "YamlDotNet",

    // 日志库
    "Serilog", "NLog",

    // Microsoft.Extensions.*
    "Microsoft.Extensions.Logging.Abstractions",
    "Microsoft.Extensions.DependencyInjection.Abstractions",
    // ...

    // 数据访问/映射
    "Dapper", "AutoMapper",

    // 验证与消息分发
    "FluentValidation", "MediatR",
};
```

### 准入标准

库必须满足**全部三项**标准才能加入 `TrustedAssemblies`：

| # | 标准 | 理由 |
|---|------|------|
| 1 | 广泛使用的开源项目（MIT / Apache 2.0 / BSD） | 代码可公开审计 |
| 2 | 源码可公开审查 | 社区监督确保无恶意行为 |
| 3 | 由可信供应商/社区维护的 NuGet 包 | 供应链完整性 |

### 识别依据

扫描器通过 PE 元数据中的 `AssemblyDefinition.Name` 识别受信程序集 —— **而非 DLL 文件名**。这可防止攻击者将恶意 DLL 重命名为 `Newtonsoft.Json.dll` 来绕过检查。

## CollectTrustedTypeRefs —— 传递性豁免

当 PluginLoader 加载插件目录时，执行两阶段扫描：

```
阶段 1: CollectTrustedTypeRefs(pluginDir)
├── 枚举插件目录中所有 *.dll 文件
├── 对每个 DLL：读取 PE 元数据 → 检查 AssemblyDefinition.Name
├── 若名称 ∈ TrustedAssemblies：
│   └── 收集该 DLL 的全部 TypeReference 条目 → (命名空间, 类型名) 对
└── 返回：HashSet<(string Namespace, string Name)>

阶段 2: ScanForbiddenReferences(pluginMainDll, trustedTypeRefs)
├── 第 0 层：白名单快速退出（若主 DLL 本身是受信的 → 直接通过）
├── 第 0.5 层：传递性豁免（跳过 trustedTypeRefs 集合中的 TypeRef）
├── 第 1 层：TypeRef 表扫描
├── 第 2 层：ExportedType 表扫描
├── 第 3 层：MemberRef 表扫描（危险方法）
├── 第 4 层：不安全代码标记 + P/Invoke
└── 第 5 层：#US 用户字符串堆扫描
```

### 为什么需要传递性豁免

Newtonsoft.Json 内部引用了 `System.Reflection.MemberInfo`、`System.IO.TextReader` 等类型。当你的插件引用 Newtonsoft.Json 时，编译器可能会将这些传递性 TypeRef 嵌入到**你的**插件 DLL 中。若没有传递性豁免，你的插件会因引用 `System.IO.TextReader` 被标记为违规 —— 即使你从未直接使用过它。

`CollectTrustedTypeRefs` 通过预先收集受信 DLL 的所有 TypeRef，在主扫描中将其标记为"已知安全"来解决此问题。

## 如何添加新的受信依赖

要将新库加入白名单：

1. 确认其满足上述三项准入标准
2. 在 `PluginLoader.cs` 的 `TrustedAssemblies` HashSet 中添加一行：
   ```csharp
   "YourLibraryName",  // 简述为何受信
   ```
3. 将库 DLL 放入插件目录（与插件主 DLL 同目录）
4. 扫描器会自动收集其 TypeRef 并豁免

> **⚠️ 重要：** 将库加入 `TrustedAssemblies` 意味着扫描器**不会**检查其内部代码。只添加你完全信任的库。

## 本示例

本插件使用 Newtonsoft.Json，无需任何 `PluginCapability` 声明：

| 功能 | Newtonsoft.Json 内部行为 | 为何能通过 |
|------|--------------------------|-----------|
| `JsonConvert.SerializeObject` | 使用反射枚举属性 | Newtonsoft.Json DLL 通过第 0 层白名单 |
| `JsonConvert.DeserializeObject<T>` | 调用 `Activator.CreateInstance`、通过反射设置属性 | 传递性 TypeRef 在第 0.5 层被豁免 |
| `JObject` / `JArray` 操作 | 使用 `System.Linq.Expressions`、动态分派 | 所有内部引用由 `CollectTrustedTypeRefs` 收集 |

### 与 PluginCapability 的关键区别

| 机制 | 作用范围 | 使用场景 |
|------|---------|---------|
| `TrustedAssemblies` | 豁免整个**库**（及其传递引用）的扫描 | 知名开源依赖 |
| `PluginCapability` | 豁免你的**插件代码**对特定命名空间的使用 | 插件需要直接访问 System.Net/IO/Process |

仅使用受信依赖的插件**无需**声明任何 `PluginCapability`。扫描器会自动处理一切。

## 安全说明

受信程序集因是可审计的开源项目而免于安全扫描。但**你的插件代码**仍会被完整扫描。若你的插件直接引用 `System.IO.File` 或 `System.Net.Http.HttpClient`，仍会被拦截 —— 除非声明相应的 `PluginCapability`。详见[安全文档](../../docs/zh-CN/security.md)。
