# PluginDemo-20：SpeedyPack — 結構化資料儲存

## 概述

本插件演示使用 `SpeedyPack` 進行結構化資料儲存，**無需任何能力聲明**。SpeedyPack 是插件持久化資料的**推薦方式**。

## 為什麼選擇 SpeedyPack？

| 特性 | SpeedyPack | PermissionedStreamFactory | Capability.FileIO + System.IO |
|------|-----------|--------------------------|------------------------------|
| 需要能力聲明 | **無** | 無 | `Capability.FileIO` |
| 快取 | ✅ 內建 | ❌ | ❌ |
| WAL（崩潰恢復） | ✅ | ❌ | ❌ |
| 交易 | ✅ `IPackTransaction` | ❌ | ❌ |
| 執行緒安全 | ✅ | ❌ | ❌ |
| 結構化序列化 | ✅ `Read<T>` | ❌ 原始位元組 | ❌ 手動 |
| 稽核追蹤 | ✅ 自動 | ✅ 自動 | ❌ 手動 |

## 基本 CRUD

```csharp
// 開啟 SpeedyPack 資料檔案
using var pack = SpeedyPack.Open("mydata.spk");

// 寫入鍵值對
pack.Write("user:name", "Alice");
pack.Write("user:age", 30);

// 讀取值（帶型別）
string name = pack.Read<string>("user:name");  // "Alice"
int age = pack.Read<int>("user:age");           // 30

// 刪除鍵
pack.Delete("user:age");

// 檢查是否存在
bool exists = pack.Contains("user:name");  // true
```

## 型別化存取與結構化物件

```csharp
public class UserProfile
{
    public string Name { get; set; }
    public int Level { get; set; }
    public string[] Tags { get; set; }
}

// 寫入結構化物件
var profile = new UserProfile { Name = "Bob", Level = 42, Tags = new[] { "admin" } };
pack.Write("profile:bob", profile);

// 讀取型別化物件
var loaded = pack.Read<UserProfile>("profile:bob");
Console.WriteLine($"{loaded.Name}, Level {loaded.Level}");
```

## 交易

```csharp
using (var tx = pack.BeginTransaction())
{
    try
    {
        tx.Write("account:a", 1000);
        tx.Write("account:b", 500);
        tx.Commit();   // 原子性 — 兩個寫入要麼都持久化，要麼都不
    }
    catch
    {
        tx.Rollback();  // 丟棄此交易中的所有寫入
    }
}
```

### IPackTransaction 方法

| 方法 | 說明 |
|------|------|
| `Write(key, value)` | 排隊寫入操作 |
| `Delete(key)` | 排隊刪除操作 |
| `Commit()` | 原子性地應用所有排隊操作 |
| `Rollback()` | 丟棄所有排隊操作 |

## 設定 SpeedyPackOptions

```csharp
var options = new SpeedyPackOptions
{
    MaxCacheSize = 1024 * 1024,              // 1 MB 快取
    AutoFlushInterval = TimeSpan.FromSeconds(30),
    CompressionLevel = CompressionLevel.Optimal
};
using var pack = SpeedyPack.Open("data.spk", options);
```

### SpeedyPackOptions 屬性

| 屬性 | 型別 | 預設值 | 說明 |
|------|------|--------|------|
| `MaxCacheSize` | `long` | 64 MB | 最大記憶體快取大小 |
| `AutoFlushInterval` | `TimeSpan` | 10 秒 | 快取刷写到磁碟的間隔 |
| `CompressionLevel` | `CompressionLevel` | `Fastest` | 儲存資料的壓縮級別 |

## 安全說明

SpeedyPack **不需要**任何能力聲明。它是安全的受控資料儲存入口：
- 驗證所有路徑在工作區邊界內
- 提供所有所有讀/寫操作的完整稽核追蹤
- 防止目錄遍歷攻擊
- 自動管理資源生命週期

## 檔案

- `Plugin.cs` — SpeedyPack 演示插件
- `README.md` — 本檔案（英文）
- `README.zh-CN.md` — 簡體中文
- 翻譯版本：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 相關示例

- **04-SafeSystemIO**：允許的記憶體 System.IO 型別（無需聲明）
- **07-ForbiddenFileIO**：被阻止的檔案操作反例
- **14-CapabilityFileIO**：當 SpeedyPack 不滿足需求時
