# PluginDemo-14: Capability.FileIO — 声明式文件 I/O 权限

## 概述

本插件演示如何使用 `[PluginCapability(Capability.FileIO)]` 声明插件需要直接文件系统访问权限。声明此能力后，插件可以访问所有 `System.IO` 类型，不再受 `SystemIOAllowedTypes` 白名单限制。

## PluginCapability 声明语法

```csharp
[PluginCapability(Capability.FileIO, Reason = "Direct log file access for audit trail")]
public class CapabilityFileIOPlugin : IPlugin { ... }
```

## Capability.FileIO 的工作方式

1. **默认状态**：`System.IO` 命名空间被整体禁止，仅 `SystemIOAllowedTypes` 白名单类型可用（MemoryStream、BinaryReader、GZipStream 等）
2. **声明后**：整个 `System.IO` 命名空间禁令被解除——File、FileStream、Directory、StreamReader(string) 等全部可访问
3. **ILString 放行**：以 `"System.IO."` 开头的字符串常量不会被标记
4. **不可声明限制**：P/Invoke、Unsafe、Reflection.Emit 等仍被阻止

## Capability.FileIO 放行范围

### TypeRef 放行

所有 `System.IO` 类型都被放行：

| 类别 | 放行类型 |
|------|---------|
| 文件操作 | `File`、`FileInfo` |
| 目录操作 | `Directory`、`DirectoryInfo` |
| 流类型 | `FileStream`、`StreamReader(path)`、`StreamWriter(path)` |
| 文件系统 | `FileSystemWatcher`、`DriveInfo`、`Path` |

### ILString 放行

- 以 `"System.IO."` 开头的字符串不会被标记

### 仍被禁止的能力

| 类别 | 仍被阻止 |
|------|---------|
| P/Invoke | `DllImportAttribute`、`Marshal`、`NativeMemory` |
| 不安全代码 | `UnverifiableCodeAttribute`、`Unsafe` |
| IL 发射 | `System.Reflection.Emit.*` |
| 程序集加载 | `System.Runtime.Loader`、`Assembly.Load*` |
| 注册表 | `Microsoft.Win32.*` |

## 与其他示例的对比

| 示例 | 声明 | 文件访问 | 说明 |
|------|------|---------|------|
| **04-SafeSystemIO** | 无 | MemoryStream、BinaryReader、GZipStream | 仅使用白名单类型 |
| **07-ForbiddenFileIO** | 无 | ❌ 被拒绝 | 反例展示 |
| **14-CapabilityFileIO** | `[PluginCapability(Capability.FileIO)]` | ✅ 完整 System.IO 访问 | 本示例 |
| **20-SpeedyPack** | 无 | 通过 SpeedyPack API（无需 Capability） | 推荐的数据存储方式 |

## 文件访问优先级

1. **SpeedyPack** — 无需能力声明。内置缓存、WAL、事务。**推荐用于结构化数据存储。**
2. **PermissionedStreamFactory** — 无需能力声明。带审计的访问，含路径验证和访问控制。
3. **Capability.FileIO + 直接 System.IO** — 仅在上述方案不满足需求时使用。

## 为什么推荐 PermissionedStreamFactory / SpeedyPack？

即使拥有 `Capability.FileIO`，仍然推荐使用受控入口，因为：

1. **审计跟踪**：所有访问都被记录和追踪
2. **路径验证**：防止目录遍历攻击（`../`）
3. **访问控制**：工作区边界强制执行
4. **资源跟踪**：防止流泄漏和资源耗尽
5. **合规性**：使用受控访问模式更容易通过安全审查

## 安全最佳实践

1. **仅在真正需要时声明 FileIO**：能用 SpeedyPack 或 PermissionedStreamFactory 吗？
2. **提供清晰的 Reason**："Direct log file access for audit trail" 比 "file access" 更好
3. **自行验证路径**：即使有 Capability.FileIO，使用前也要验证所有文件路径
4. **使用 using 语句**：始终释放 FileStream/StreamReader/StreamWriter
5. **最小权限原则**：只声明插件真正需要的能力

## 文件

- `Plugin.cs` — 声明 Capability.FileIO 的演示插件
- `README.md` — 本文件（英文）
- `README.zh-CN.md` — 简体中文
- 翻译版本：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 相关示例

- **04-SafeSystemIO**：允许的内存 System.IO 类型（无需声明）
- **07-ForbiddenFileIO**：被阻止的文件操作反例
- **20-SpeedyPack**：无需任何能力声明的推荐数据存储
- **18-CapabilityDenied**：不可声明能力反例
