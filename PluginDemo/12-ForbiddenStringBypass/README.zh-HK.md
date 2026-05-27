# PluginDemo-12: 禁止的字串反射繞過反例

## 概述

本插件演示了 SiliconLife 插件系統中**被禁止**的基於字串的反射繞過嘗試。它展示了為什麼字串拼接、插值、編碼和其他混淆技術**無法**繞過 PluginLoader 的 #US（用戶字串）堆掃描——**最後一道防線**。

## 什麼是 #US 堆？

在 .NET PE（可攜式可執行檔）元資料中，**#US（User String）堆**儲存了所有 `ldstr` IL 指令使用的字串字面量運算元。每當你在 C# 程式碼中編寫一個字串字面量，編譯器就會將其儲存在此堆中。

```
C# 原始碼:  string s = "System.IO.File";
    ↓ 編譯
IL 程式碼:  ldstr "System.IO.File"    ← 參考 #US 堆中的 token
    ↓ PluginLoader 掃描
#US 堆:     [..., "System.IO.File", ...]  ← 被前綴匹配捕獲！
```

PluginLoader 的 `ScanUserStrings()` 方法遍歷 #US 堆中的**每一個條目**，檢查是否有任何字串以禁止前綴開頭。

## 禁止的字串前綴

以下前綴在 #US 堆中被發現時會觸發 `[ILString]` 違規：

| 前綴 | 類別 |
|------|------|
| `System.IO.` | 檔案系統類型 |
| `System.Net.Http` | HTTP 用戶端 |
| `System.Net.WebSockets` | WebSocket |
| `System.Net.Sockets` | 原始通訊端 |
| `System.Net.Mail` | SMTP |
| `System.Net.NetworkInformation` | 網路探測 |
| `System.Net.Security` | SslStream |
| `System.Diagnostics.Process` | 行程/命令列 |
| `Microsoft.CodeAnalysis` | Roslyn 編譯器 |
| `System.Reflection.Emit` | IL 發射 |
| `System.Runtime.Loader` | AssemblyLoadContext |
| `System.CodeDom.Compiler` | 舊版 CodeDom |
| `Microsoft.Win32` | Windows 登錄檔 |

## 演示的違規操作

### 違規 1：直接類型名稱字串

```csharp
// ❌ 禁止 — 完整字串在 #US 堆中
Type? fileType = Type.GetType("System.IO.File, System.Runtime");
```

**違規**：`[ILString] "System.IO.File, System.Runtime" matches forbidden prefix "System.IO."`

### 違規 2：字串拼接（編譯時）

```csharp
// ❌ 禁止 — 編譯器將 const+const 摺疊為一個 #US 條目
const string ns = "System.Net.Http";
const string typeName = ".HttpClient";
const string assembly = ", System.Net.Http";
Type? type = Type.GetType(ns + typeName + assembly);
// 各個部分和摺疊後的結果都在 #US 堆中！
```

**違規**：`[ILString] "System.Net.Http.HttpClient, System.Net.Http" matches forbidden prefix "System.Net.Http"`

### 違規 3：字串插值

```csharp
// ❌ 禁止 — 字面量部分儲存在 #US 堆中
string className = "FileStream";
string fullName = $"System.IO.{className}, System.Runtime";
// "System.IO." 本身就匹配前綴！
```

**違規**：`[ILString] "System.IO." matches forbidden prefix "System.IO."`

### 違規 4：Const 欄位

```csharp
// ❌ 禁止 — const 值在使用處內聯 → 出現在 #US 堆中
private const string ProcessType = "System.Diagnostics.Process";
private const string AssemblyName = ", System.Runtime";
Type? type = Type.GetType(ProcessType + AssemblyName);
```

**違規**：`[ILString] "System.Diagnostics.Process" matches forbidden prefix "System.Diagnostics.Process"`

### 違規 5：部分字串片段

```csharp
// ❌ 禁止 — 每個部分是獨立的 ldstr，單獨掃描
string part1 = "System.Reflection.Emit";
string part2 = ".AssemblyBuilder";
string fullType = part1 + part2;
// "System.Reflection.Emit" 本身就匹配前綴！
```

**違規**：`[ILString] "System.Reflection.Emit" matches forbidden prefix "System.Reflection.Emit"`

### 違規 6：多個禁止目標

```csharp
// ❌ 禁止 — 整個組件中的所有字串都會被掃描
string tcp = "System.Net.Sockets.TcpClient";
string registry = "Microsoft.Win32.Registry";
string loader = "System.Runtime.Loader.AssemblyLoadContext";
```

**多個違規** — 掃描器不會在第一個匹配處停止。

## 為什麼混淆技術全部失敗

| 技術 | 為什麼失敗 |
|------|-----------|
| Const 拼接 | 編譯器摺疊為單個 #US 條目 |
| 字串插值 | 字面量部分儲存在 #US 堆中 |
| Const 欄位 | 值在使用處內聯 → 出現在 #US |
| 拆分為變數 | 每個 `ldstr` 運算元獨立掃描 |
| Base64 編碼 | 解碼需要執行時方法，但 `Type.GetType` 被 MemberRef 攔截 |
| 字元陣列建構 | 不產生 `ldstr`，但 `Type.GetType` 仍被 MemberRef 攔截 |
| XOR 加密 | 加密字串在 #US 中不可讀，但解密 + `Type.GetType` = MemberRef 攔截 |
| 反轉字串 | 反轉後的字面量可能仍匹配前綴，且 reverse + GetType = MemberRef 攔截 |

**核心洞察**：#US 掃描攔截**字串**。MemberRef 掃描攔截**方法**。要動態載入類型，你需要兩者兼備。PluginLoader 獨立地攔截兩者。

## 完整防禦鏈

PluginLoader 的五步掃描構建了堅不可摧的防禦：

| 步驟 | 機制 | 捕獲內容 |
|------|------|---------|
| 1 | TypeRef 表 | 對禁止類型的直接參考 |
| 2 | ExportedType 表 | 禁止命名空間的轉發類型 |
| 3 | MemberRef 表 | 對 `Type.GetType`、`Assembly.Load`、`Activator.CreateInstance` 的呼叫 |
| 4 | 不安全標記 | `[DllImport]`、unsafe 區塊、PinvokeImpl 旗標 |
| **5** | **#US 堆掃描** | **匹配禁止前綴的字串常數（本示例）** |

第 5 步是**最後一道防線**，因為：
- 第 1-4 步捕獲**主動程式碼**（類型、方法、特性）
- 第 5 步捕獲**被動資料**（可能被用於繞過的字串）
- 即使 MemberRef 掃描遺漏了某個未知方法，禁止的類型名稱字串必須存在於組件中 → #US 掃描捕獲它

## 權衡

#US 堆掃描可能產生**輕微誤報**：
- 日誌訊息：`logger.Info("Failed to connect to System.Net.Http endpoint")` → 被標記
- 文件字串：編譯到 XML 文件的註解不在 #US 堆中（安全）
- `nameof()` 運算式：不匹配前綴的編譯時字串是安全的

**誤報解決方案**：重新表述字串字面量以避免禁止前綴。

## 與其他示例的對比

| 示例 | 焦點 | 關係 |
|------|------|------|
| **10-ForbiddenReflection** | 禁止的反射方法（MemberRef 掃描） | 捕獲方法 |
| **11-ForbiddenPInvoke** | 禁止的 P/Invoke 和 unsafe 程式碼 | 不同威脅向量 |
| **12-ForbiddenStringBypass** | 基於字串的繞過嘗試（本示例） | 捕獲字串 |
| **02-TypeRegistryUsage** | 安全的類型發現 | 正確替代方案 |
| **03-ObjectFactoryUsage** | 安全的物件建立 | 正確替代方案 |

## 最佳實踐

1. **避免所有字串中的禁止前綴** — 包括日誌訊息和編譯到 IL 的註解
2. **使用 ITypeRegistry/IObjectFactory** — 字串反射的安全替代方案
3. **理解拆分字串無濟於事** — 每個片段被獨立掃描
4. **執行時建構（char[]）繞過 #US** — 但被 MemberRef 掃描的 Type.GetType 攔截
5. **縱深防禦有效** — 即使繞過一層，其他層仍能捕獲你

## 檔案

- `Plugin.cs` - 反例演示插件
- `README.md` - 英文（English）
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 本檔案（繁體中文）
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

## 相關示例

- **10-ForbiddenReflection**：禁止的反射方法（MemberRef 掃描）
- **11-ForbiddenPInvoke**：禁止的 P/Invoke 和 unsafe 程式碼
- **02-TypeRegistryUsage**：ITypeRegistry 正確用法
- **03-ObjectFactoryUsage**：IObjectFactory 正確用法
