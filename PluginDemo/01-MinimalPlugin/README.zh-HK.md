# 最簡插件範例

一個最簡的 `IPlugin` 實作，使用硬編碼值演示插件生命週期。

## IPlugin 介面全貌

每個 SiliconLife 插件必須實作 `SiliconLife.Collective` 中定義的 `IPlugin` 介面：

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### 屬性說明

| 成員 | 類型 | 說明 |
|------|------|------|
| `Id` | `string` | 唯一識別符，跨版本必須穩定（如 `"com.siliconlife.demo.minimal"`） |
| `GetName(Language)` | `string` | 人類可讀的顯示名稱，透過 `Language` 列舉在地化 |
| `Version` | `string` | 語意化版本字串（如 `"1.0.0"`） |
| `GetDescription(Language)` | `string` | 插件功能的簡短描述 |
| `GetAuthor(Language)` | `string` | 作者或組織名稱 |

## 生命週期呼叫順序

宿主按嚴格順序呼叫生命週期方法：

```
OnLoad → OnStart → [執行中] → OnStop → OnUnload
```

| 方法 | 呼叫時機 | 典型用途 |
|------|---------|---------|
| `OnLoad()` | 插件 DLL 載入到宿主程序時呼叫一次 | 驗證設定、註冊型別、準備資源 |
| `OnStart()` | 宿主完全啟動且所有插件已載入後呼叫 | 與其他插件互動、啟動背景任務 |
| `OnStop()` | 宿主優雅關閉時呼叫 | 釋放資源、清除緩衝區、儲存狀態 |
| `OnUnload()` | 插件從宿主程序卸載時呼叫 | 最終清理 |

## 本範例

本插件所有屬性回傳硬編碼值，生命週期方法為空，是插件開發的最簡起點。

## 安全說明

插件在隔離的 `AssemblyLoadContext` 中載入，並掃描禁止的命名空間引用（如 `System.IO`、`System.Net.Http`）。詳見[安全文件](../../docs/zh-HK/security.md)。
