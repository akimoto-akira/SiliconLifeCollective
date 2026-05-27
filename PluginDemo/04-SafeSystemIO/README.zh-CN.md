# 安全 System.IO 示例

演示 `SystemIOAllowedTypes` 白名单中的 System.IO 类型：`MemoryStream`、`BinaryReader`/`BinaryWriter`、`GZipStream`。说明为什么 `FileStream` 必须通过 `PermissionedStreamFactory` 获取。

## SystemIOAllowedTypes 白名单

插件运行时默认阻止 `System.IO` 命名空间，但豁免**不直接执行文件 I/O** 的类型：

| 类别 | 允许的类型 | 安全原因 |
|------|-----------|---------|
| 流抽象 | `Stream` | 抽象基类，本身无 I/O |
| 内存流 | `MemoryStream` | 纯内存操作 |
| 压缩流 | `GZipStream`、`DeflateStream`、`ZLibStream` | 包装另一个流，不打开文件 |
| 二进制包装器 | `BinaryReader`、`BinaryWriter` | 包装任意流，不打开文件 |
| 枚举 | `SeekOrigin`、`FileMode`、`FileAccess`、`FileShare`、`CompressionMode`、`CompressionLevel` | 仅值类型 |
| 异常 | `IOException`、`InvalidDataException`、`EndOfStreamException` | 仅错误类型 |

### 不在白名单中的类型

这些类型**直接访问文件系统**，在插件代码中被**阻止**：

| 被阻止的类型 | 阻止原因 | 安全替代方案 |
|-------------|---------|------------|
| `FileStream` | 直接打开文件 | `PermissionedStreamFactory.CreateReadStream()` / `CreateWriteStream()` |
| `File` | 静态文件操作 | `PermissionedStreamFactory` + `SafePath` |
| `Directory` | 静态目录操作 | `SafePath`（权限检查） |
| `FileInfo` | 封装文件路径 | `SafePath` |
| `DirectoryInfo` | 封装目录路径 | `SafePath` |
| `StreamReader` | 直接打开文件 | `PermissionedStreamFactory` + 包装 `PermissionedStream` |
| `StreamWriter` | 直接打开文件 | `PermissionedStreamFactory` + 包装 `PermissionedStream` |

## 为什么 FileStream 必须通过 PermissionedStreamFactory

`FileStream` 直接打开磁盘文件——在插件系统中是重大安全风险。`PermissionedStreamFactory` 强制执行：

1. **权限检查** — 调用者的 `PermissionManager` 必须授予该路径的 `FileAccess` 权限
2. **审计日志** — 每次文件打开都记录调用者的 being ID
3. **路径验证** — 空/无效路径在任何 I/O 之前被拒绝

```
❌ new FileStream("path", FileMode.Open)           → 被 TypeRef 扫描器阻止
✅ PermissionedStreamFactory.CreateReadStream(id, "path")  → 权限检查通过
✅ PermissionedStreamFactory.CreateWriteStream(id, "path") → 权限检查通过
```

## 示例管线

本示例仅使用白名单类型构建完整的内存数据管线：

```
┌─────────────────────────────────────────────────────────────────┐
│  示例 1: MemoryStream                                           │
│  └─ 写入字节 → 读取字节 → 解码字符串                             │
│                                                                  │
│  示例 2: 压缩管线                                                │
│  └─ string → UTF8 → MemoryStream                                │
│     → GZipStream(压缩) → MemoryStream(压缩后)                    │
│     → GZipStream(解压) → MemoryStream(原始)                      │
│     → UTF8 → string (往返验证)                                   │
│                                                                  │
│  示例 3: BinaryReader/Writer                                     │
│  └─ Write(int, double, string) → MemoryStream                   │
│     → Read(int, double, string) → 验证往返                       │
└─────────────────────────────────────────────────────────────────┘
```

## 本示例

> **⚠️ 注意：** 本示例**仅**使用 `SystemIOAllowedTypes` 白名单类型，不执行任何文件 I/O。文件访问请参见 `PermissionedStreamFactory` API。

| 类 | 角色 |
|----|------|
| `SafeSystemIOPlugin` | `IPlugin` 实现 — 演示安全的 System.IO 用法 |

## 安全说明

`System.IO` 命名空间被插件 TypeRef 扫描器阻止，只有白名单类型可通过。实际文件访问必须使用 `PermissionedStreamFactory`，它会执行权限检查和审计日志。详见[安全文档](../../docs/zh-CN/security.md)。
