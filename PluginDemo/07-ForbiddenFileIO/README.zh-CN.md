# 禁止的文件 I/O 操作反例

演示插件系统中**禁止的**文件 I/O 操作。本示例作为反例参考，展示不应该做什么，并为每个违规行为提供正确的替代方案。

## 为什么 System.IO 被全局禁止？

整个 `System.IO` 命名空间在插件级别被阻止，因为直接文件访问存在严重安全风险：

1. **未授权文件访问**：插件可能读取工作区外的敏感文件（密码、密钥、个人数据）
2. **文件覆盖攻击**：恶意插件可能覆盖关键的系统或配置文件
3. **目录遍历**：插件可能使用 `../` 路径逃离工作区边界
4. **资源耗尽**：不受控制的文件创建可能填满磁盘空间
5. **无审计跟踪**：直接文件操作绕过插件安全审计系统

## 禁止的类型

所有直接访问文件系统的 `System.IO` 类型都被阻止：

| 禁止的类型 | 被阻止的方法 | 风险级别 |
|-----------|-------------|---------|
| `File` | `ReadAllText`, `WriteAllText`, `AppendAllText` 等 | 🔴 严重 |
| `FileStream` | 带文件路径的构造函数 | 🔴 严重 |
| `Directory` | `GetFiles`, `GetDirectories`, `CreateDirectory` | 🔴 严重 |
| `StreamReader` | 带文件路径（字符串）的构造函数 | 🔴 严重 |
| `StreamWriter` | 带文件路径（字符串）的构造函数 | 🔴 严重 |
| `FileInfo` | 所有方法 | 🔴 严重 |
| `DirectoryInfo` | 所有方法 | 🔴 严重 |

## 允许的类型（白名单例外）

执行**纯内存操作**（不直接访问文件系统）的类型被允许：

| 允许的类型 | 用途 | 为什么安全 |
|-----------|------|-----------|
| `MemoryStream` | 内存字节流 | 无文件系统访问 |
| `BinaryReader` | 从现有流读取 | 包装流，不打开文件 |
| `BinaryWriter` | 写入现有流 | 包装流，不创建文件 |
| `GZipStream` | 压缩/解压缩 | 包装流，无文件访问 |
| `StreamReader` | 带 `Stream` 参数的构造函数 | 包装受审计流时安全 |
| `StreamWriter` | 带 `Stream` 参数的构造函数 | 包装受审计流时安全 |

详见 **04-SafeSystemIO** 示例。

## 如何通过 PermissionedStreamFactory 安全访问文件

`PermissionedStreamFactory` 是插件中文件操作的**受控入口**：

```csharp
// ✅ 正确：读取文件
using var readStream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(readStream);
string content = reader.ReadToEnd();

// ✅ 正确：写入文件
using var writeStream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(writeStream);
writer.Write("日志数据");
```

**PermissionedStreamFactory 提供：**
1. **路径验证**：防止目录遍历攻击（`../`）
2. **权限检查**：确保文件在允许的工作区内
3. **审计日志**：所有文件访问都被记录以供安全审查
4. **资源清理**：跟踪打开的流并防止泄漏

## 本示例中的违规行为

### 违规 1：File.ReadAllText

```csharp
// ❌ 禁止 — ⚠️ VIOLATION: [TypeRef] System.IO.File::ReadAllText
string content = File.ReadAllText("config.json");

// ✅ 正确替代
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
string content = reader.ReadToEnd();
```

### 违规 2：File.WriteAllText

```csharp
// ❌ 禁止 — ⚠️ VIOLATION: [TypeRef] System.IO.File::WriteAllText
File.WriteAllText("output.log", "some data");

// ✅ 正确替代
using var stream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(stream);
writer.Write("some data");
```

### 违规 3：直接 FileStream

```csharp
// ❌ 禁止 — ⚠️ VIOLATION: [TypeRef] System.IO.FileStream::.ctor
using var fs = new FileStream("data.bin", FileMode.Open);

// ✅ 正确替代
using var fs = PermissionedStreamFactory.OpenRead("data.bin");
```

### 违规 4：Directory.GetFiles

```csharp
// ❌ 禁止 — ⚠️ VIOLATION: [TypeRef] System.IO.Directory::GetFiles
string[] files = Directory.GetFiles("./logs", "*.txt");

// ✅ 正确替代（使用 SpeedyPack）
using var pack = SpeedyPack.Open("logs.spk");
var entries = pack.ListEntries("/");
```

### 违规 5：带直接路径的 StreamReader

```csharp
// ❌ 禁止 — ⚠️ VIOLATION: [TypeRef] System.IO.StreamReader::.ctor(string)
using var reader = new StreamReader("config.json");

// ✅ 正确替代
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
```

## 与其他示例的对比

| 示例 | 重点 | 所需权限 |
|------|------|---------|
| **04-SafeSystemIO** | 允许的内存类型（MemoryStream、GZipStream） | 无 |
| **07-ForbiddenFileIO** | 禁止的文件访问模式（本示例） | 不适用（被阻止） |
| **14-CapabilityFileIO** | 声明 FileIO 能力以绕过限制 | `Capability.FileIO` |

## PluginLoader 安全扫描机制

当 PluginLoader 扫描此插件时：

1. **TypeRef 扫描**：检测对禁止的 `System.IO` 类型的引用
2. **MemberRef 扫描**：检测对被阻止方法的调用
3. **IL 字符串扫描**：检测基于字符串的反射绕过尝试
4. **拒绝**：插件在加载期间被拒绝，并提供详细错误消息

字符串拼接、反射、动态加载、混淆等绕过手段均无效——这些会被 IL 级扫描捕获（参见 **12-ForbiddenStringBypass**）。

## 安全说明

若你确实需要不受限制的文件访问，可以声明 `Capability.FileIO`（参见 14-CapabilityFileIO）。但最佳实践是：
- 优先使用 **SpeedyPack** 存储结构化数据（无需任何权限声明）
- 必须访问文件时使用 **PermissionedStreamFactory**（受控入口）
- 仅在上述方案无法满足需求时才声明 `Capability.FileIO`
