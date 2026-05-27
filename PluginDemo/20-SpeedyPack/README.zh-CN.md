# PluginDemo-20: SpeedyPack — 结构化数据存储

## 概述

本插件演示使用 `SpeedyPack` 进行结构化数据存储，**无需任何能力声明**。SpeedyPack 是插件持久化数据的**推荐方式**。

## 为什么选择 SpeedyPack？

| 特性 | SpeedyPack | PermissionedStreamFactory | Capability.FileIO + System.IO |
|------|-----------|--------------------------|------------------------------|
| 需要能力声明 | **无** | 无 | `Capability.FileIO` |
| 缓存 | ✅ 内置 | ❌ | ❌ |
| WAL（崩溃恢复） | ✅ | ❌ | ❌ |
| 事务 | ✅ `IPackTransaction` | ❌ | ❌ |
| 线程安全 | ✅ | ❌ | ❌ |
| 结构化序列化 | ✅ `Read<T>` | ❌ 原始字节 | ❌ 手动 |
| 审计跟踪 | ✅ 自动 | ✅ 自动 | ❌ 手动 |

## 基本 CRUD

```csharp
// 打开 SpeedyPack 数据文件
using var pack = SpeedyPack.Open("mydata.spk");

// 写入键值对
pack.Write("user:name", "Alice");
pack.Write("user:age", 30);

// 读取值（带类型）
string name = pack.Read<string>("user:name");  // "Alice"
int age = pack.Read<int>("user:age");           // 30

// 删除键
pack.Delete("user:age");

// 检查是否存在
bool exists = pack.Contains("user:name");  // true
```

## 类型化访问与结构化对象

```csharp
public class UserProfile
{
    public string Name { get; set; }
    public int Level { get; set; }
    public string[] Tags { get; set; }
}

// 写入结构化对象
var profile = new UserProfile { Name = "Bob", Level = 42, Tags = new[] { "admin" } };
pack.Write("profile:bob", profile);

// 读取类型化对象
var loaded = pack.Read<UserProfile>("profile:bob");
Console.WriteLine($"{loaded.Name}, Level {loaded.Level}");
```

## 事务

```csharp
using (var tx = pack.BeginTransaction())
{
    try
    {
        tx.Write("account:a", 1000);
        tx.Write("account:b", 500);
        tx.Commit();   // 原子性 — 两个写入要么都持久化，要么都不
    }
    catch
    {
        tx.Rollback();  // 丢弃此事务中的所有写入
    }
}
```

### IPackTransaction 方法

| 方法 | 说明 |
|------|------|
| `Write(key, value)` | 排队写入操作 |
| `Delete(key)` | 排队删除操作 |
| `Commit()` | 原子性地应用所有排队操作 |
| `Rollback()` | 丢弃所有排队操作 |

## 配置 SpeedyPackOptions

```csharp
var options = new SpeedyPackOptions
{
    MaxCacheSize = 1024 * 1024,              // 1 MB 缓存
    AutoFlushInterval = TimeSpan.FromSeconds(30),
    CompressionLevel = CompressionLevel.Optimal
};
using var pack = SpeedyPack.Open("data.spk", options);
```

### SpeedyPackOptions 属性

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `MaxCacheSize` | `long` | 64 MB | 最大内存缓存大小 |
| `AutoFlushInterval` | `TimeSpan` | 10 秒 | 缓存刷写到磁盘的间隔 |
| `CompressionLevel` | `CompressionLevel` | `Fastest` | 存储数据的压缩级别 |

## 安全说明

SpeedyPack **不需要**任何能力声明。它是安全的受控数据存储入口：
- 验证所有路径在工作区边界内
- 提供所有读/写操作的完整审计跟踪
- 防止目录遍历攻击
- 自动管理资源生命周期

## 文件

- `Plugin.cs` — SpeedyPack 演示插件
- `README.md` — 本文件（英文）
- `README.zh-CN.md` — 简体中文
- 翻译版本：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 相关示例

- **04-SafeSystemIO**：允许的内存 System.IO 类型（无需声明）
- **07-ForbiddenFileIO**：被阻止的文件操作反例
- **14-CapabilityFileIO**：当 SpeedyPack 不满足需求时
