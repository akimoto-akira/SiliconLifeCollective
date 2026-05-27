# PluginDemo-11: 禁止的 P/Invoke 和 unsafe 代碼反例

## 概述

本插件演示了 SiliconLife 插件系統中**被禁止的** P/Invoke 和 unsafe 代碼操作。與其他有安全包裝替代方案的禁止類別（檔案 I/O、網路、行程、反射）不同，P/Invoke 和 unsafe 代碼是**硬性禁止**，沒有安全替代方案——它們不能通過任何 `PluginCapability` 聲明來豁免。

## 為什麼 P/Invoke 是終極威脅？

P/Invoke 和 unsafe 代碼代表了對插件安全的**最根本威脅**，因為它們完全在**受管理執行階段之外**運行：

- 原生代碼以完整行程權限執行
- 沒有受管理類型安全、記憶體安全或垃圾回收
- 無法攔截、稽核或沙箱化原生呼叫
- 原生代碼崩潰 = 整個行程崩潰（無例外處理）
- 可以存取行程空間中的任何記憶體位址

## 三重保險機制

PluginLoader 使用**三個獨立的偵測層**確保 P/Invoke 和 unsafe 代碼永遠無法逃過偵測：

### 第一層：TypeRef 表掃描

偵測 PE 中繼資料中對禁止類型的直接參考：

| 禁止類型 | 命名空間 | 威脅 |
|----------|----------|------|
| `DllImportAttribute` | System.Runtime.InteropServices | 宣告原生函式匯入 |
| `UnmanagedFunctionPointerAttribute` | System.Runtime.InteropServices | 原生函式指標 |
| `SuppressGCTransitionAttribute` | System.Runtime.InteropServices | 跳過 GC 轉換 |
| `Marshal` | System.Runtime.InteropServices | 受管理/非受管理記憶體橋接 |
| `MemoryMarshal` | System.Runtime.InteropServices | Span 和原始記憶體交換 |
| `NativeMemory` | System.Runtime.InteropServices | 原生堆積 malloc/free |
| `NativeLibrary` | System.Runtime.InteropServices | 動態載入原生共用程式庫 |
| `GCHandle` | System.Runtime.InteropServices | 固定受管理物件，暴露指標 |
| `SafeHandle` | System.Runtime.InteropServices | 原生資源控制代碼基底類別 |
| `Unsafe` | System.Runtime.CompilerServices | Unsafe 輔助類別 |
| `UnverifiableCodeAttribute` | System.Security | 不可驗證代碼標記 |
| `SuppressUnmanagedCodeSecurityAttribute` | System.Security | 抑制安全檢查 |

### 第二層：Unsafe 標記掃描（ScanUnsafeMarkers）

獨立於類型參考，偵測編譯器產生的標記：

| 標記 | 偵測方法 | 來源 |
|------|----------|------|
| `[assembly: UnverifiableCode]` | 組件 CustomAttribute 表 | C# `unsafe` 關鍵字 |
| `[module: UnverifiableCode]` | 模組 CustomAttribute 表 | C# `unsafe` 關鍵字 |
| `MethodAttributes.PinvokeImpl` | MethodDef 表旗標 | `[DllImport]` 屬性 |

### 第三層：IL 字串掃描（#US 堆積）

捕獲參考 InteropServices 類型的字串常數：

```
"System.Runtime.InteropServices.Marshal"  → 被標記
"System.Runtime.InteropServices.*"        → 前綴匹配被標記
```

## 演示的違規操作

### 違規 1：[DllImport] 宣告

```csharp
// ❌ 禁止
[DllImport("kernel32.dll")]
private static extern ulong GetTickCount64();
```

**捕獲方式：**
- `[TypeRef] System.Runtime.InteropServices.DllImportAttribute`
- `[PInvoke] GetTickCount64 (native interop)`（PinvokeImpl 旗標）

### 違規 2：Marshal 使用

```csharp
// ❌ 禁止
IntPtr ptr = Marshal.AllocHGlobal(1024);
Marshal.WriteByte(ptr, 0xFF);
string? str = Marshal.PtrToStringAnsi(ptr);
Marshal.FreeHGlobal(ptr);
```

**捕獲方式：** `[TypeRef] System.Runtime.InteropServices.Marshal`

### 違規 3：NativeMemory 使用

```csharp
// ❌ 禁止
unsafe
{
    void* buffer = NativeMemory.Alloc(4096);
    NativeMemory.Free(buffer);
}
```

**捕獲方式：**
- `[TypeRef] System.Runtime.InteropServices.NativeMemory`
- `[UnsafeMarker] [module: System.Security.UnverifiableCode]`

### 違規 4：GCHandle 固定

```csharp
// ❌ 禁止
GCHandle handle = GCHandle.Alloc(managedArray, GCHandleType.Pinned);
IntPtr ptr = handle.AddrOfPinnedObject();
handle.Free();
```

**捕獲方式：** `[TypeRef] System.Runtime.InteropServices.GCHandle`

### 違規 5：unsafe 代碼區塊

```csharp
// ❌ 禁止
unsafe
{
    int* ptr = &value;
    *ptr = 100;
    byte* stack = stackalloc byte[256];
}
```

**捕獲方式：** `[UnsafeMarker] [module: System.Security.UnverifiableCode]`

### 違規 6：NativeLibrary 載入

```csharp
// ❌ 禁止
IntPtr lib = NativeLibrary.Load("evil.dll");
IntPtr funcPtr = NativeLibrary.GetExport(lib, "malicious_function");
NativeLibrary.Free(lib);
```

**捕獲方式：** `[TypeRef] System.Runtime.InteropServices.NativeLibrary`

## 無安全替代方案——對比

| 禁止類別 | 安全包裝 | 可稽核 | 可通過 PluginCapability 宣告 |
|----------|----------|--------|------------------------------|
| 檔案 I/O | PermissionedStreamFactory | ✅ 是 | ✅ Capability.FileIO |
| 網路 | NetworkExecutor | ✅ 是 | ✅ Capability.Network |
| 行程 | CommandLineExecutor | ✅ 是 | ✅ Capability.Process |
| 反射 | ITypeRegistry + IObjectFactory | ✅ 是 | ❌ 始終禁止 |
| **P/Invoke 和 unsafe** | **❌ 無** | **❌ 不可能** | **❌ 始終禁止** |

## 如果插件確實需要原生代碼怎麼辦？

如果程式庫合法使用了 P/Invoke 或 unsafe 代碼，必須：

1. **由專案維護者手動稽核**
2. **新增到 PluginLoader 中的 `TrustedAssemblies` 白名單**
3. **通過 PE 中繼資料 `AssemblyDefinition.Name` 識別**（非檔案名稱——防止重新命名攻擊）

## 檔案

- `Plugin.cs` - 反例演示插件
- `README.md` - English
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

## 相關範例

- **04-SafeSystemIO**：System.IO 白名單安全類型
- **06-TrustedDependency**：TrustedAssemblies 白名單機制
- **10-ForbiddenReflection**：禁止的反射操作
- **12-ForbiddenStringBypass**：基於字串的反射繞過嘗試
