# 受信依賴示例

演示使用 `Newtonsoft.Json` —— 一個內部大量使用反射的庫 —— 作為受信程序集。PluginLoader 安全掃描器會完全跳過受信程序集，允許插件引用它們而不觸發違規。

## TrustedAssemblies 白名單機制

`PluginLoader` 維護一個靜態白名單，列出**默認受信**的開源庫：

```csharp
private static readonly HashSet<string> TrustedAssemblies = new(StringComparer.Ordinal)
{
    // 序列化庫
    "Google.Protobuf",
    "protobuf-net",
    "Newtonsoft.Json",        // ← 本示例使用此庫
    "MessagePack",
    "YamlDotNet",

    // 日誌庫
    "Serilog", "NLog",

    // Microsoft.Extensions.*
    "Microsoft.Extensions.Logging.Abstractions",
    "Microsoft.Extensions.DependencyInjection.Abstractions",
    // ...

    // 數據訪問/映射
    "Dapper", "AutoMapper",

    // 驗證與消息分發
    "FluentValidation", "MediatR",
};
```

### 準入標準

庫必須滿足**全部三項**標準才能加入 `TrustedAssemblies`：

| # | 標準 | 理由 |
|---|------|------|
| 1 | 廣泛使用的開源項目（MIT / Apache 2.0 / BSD） | 代碼可公開審計 |
| 2 | 源碼可公開審查 | 社區監督確保無惡意行為 |
| 3 | 由可信供應商/社區維護的 NuGet 包 | 供應鏈完整性 |

### 識別依據

掃描器通過 PE 元數據中的 `AssemblyDefinition.Name` 識別受信程序集 —— **而非 DLL 文件名**。這可防止攻擊者將惡意 DLL 重命名為 `Newtonsoft.Json.dll` 來繞過檢查。

## CollectTrustedTypeRefs —— 傳遞性豁免

當 PluginLoader 加載插件目錄時，執行兩階段掃描：

```
階段 1: CollectTrustedTypeRefs(pluginDir)
├── 枚舉插件目錄中所有 *.dll 文件
├── 對每個 DLL：讀取 PE 元數據 → 檢查 AssemblyDefinition.Name
├── 若名稱 ∈ TrustedAssemblies：
│   └── 收集該 DLL 的全部 TypeReference 條目 → (命名空間, 類型名) 對
└── 返回：HashSet<(string Namespace, string Name)>

階段 2: ScanForbiddenReferences(pluginMainDll, trustedTypeRefs)
├── 第 0 層：白名單快速退出（若主 DLL 本身是受信的 → 直接通過）
├── 第 0.5 層：傳遞性豁免（跳過 trustedTypeRefs 集合中的 TypeRef）
├── 第 1 層：TypeRef 表掃描
├── 第 2 層：ExportedType 表掃描
├── 第 3 層：MemberRef 表掃描（危險方法）
├── 第 4 層：不安全代碼標記 + P/Invoke
└── 第 5 層：#US 用戶字符串堆掃描
```

### 為什麼需要傳遞性豁免

Newtonsoft.Json 內部引用了 `System.Reflection.MemberInfo`、`System.IO.TextReader` 等類型。當你的插件引用 Newtonsoft.Json 時，編譯器可能會將這些傳遞性 TypeRef 嵌入到**你的**插件 DLL 中。若沒有傳遞性豁免，你的插件會因引用 `System.IO.TextReader` 被標記為違規 —— 即使你從未直接使用過它。

`CollectTrustedTypeRefs` 通過預先收集受信 DLL 的所有 TypeRef，在主掃描中將其標記為「已知安全」來解決此問題。

## 如何添加新的受信依賴

要將新庫加入白名單：

1. 確認其滿足上述三項準入標準
2. 在 `PluginLoader.cs` 的 `TrustedAssemblies` HashSet 中添加一行：
   ```csharp
   "YourLibraryName",  // 簡述為何受信
   ```
3. 將庫 DLL 放入插件目錄（與插件主 DLL 同目錄）
4. 掃描器會自動收集其 TypeRef 並豁免

> **⚠️ 重要：** 將庫加入 `TrustedAssemblies` 意味著掃描器**不會**檢查其內部代碼。只添加你完全信任的庫。

## 本示例

本插件使用 Newtonsoft.Json，無需任何 `PluginCapability` 聲明：

| 功能 | Newtonsoft.Json 內部行為 | 為何能通過 |
|------|--------------------------|-----------|
| `JsonConvert.SerializeObject` | 使用反射枚舉屬性 | Newtonsoft.Json DLL 通過第 0 層白名單 |
| `JsonConvert.DeserializeObject<T>` | 調用 `Activator.CreateInstance`、通過反射設置屬性 | 傳遞性 TypeRef 在第 0.5 層被豁免 |
| `JObject` / `JArray` 操作 | 使用 `System.Linq.Expressions`、動態分派 | 所有內部引用由 `CollectTrustedTypeRefs` 收集 |

### 與 PluginCapability 的關鍵區別

| 機制 | 作用範圍 | 使用場景 |
|------|---------|---------|
| `TrustedAssemblies` | 豁免整個**庫**（及其傳遞引用）的掃描 | 知名開源依賴 |
| `PluginCapability` | 豁免你的**插件代碼**對特定命名空間的使用 | 插件需要直接訪問 System.Net/IO/Process |

僅使用受信依賴的插件**無需**聲明任何 `PluginCapability`。掃描器會自動處理一切。

## 安全說明

受信程序集因是可審計的開源項目而免於安全掃描。但**你的插件代碼**仍會被完整掃描。若你的插件直接引用 `System.IO.File` 或 `System.Net.Http.HttpClient`，仍會被攔截 —— 除非聲明相應的 `PluginCapability`。詳見[安全文檔](../../docs/zh-HK/security.md)。
