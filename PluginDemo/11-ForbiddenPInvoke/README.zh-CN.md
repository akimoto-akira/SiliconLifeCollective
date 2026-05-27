# PluginDemo-11: 禁止的 P/Invoke 和 unsafe 代码反例

## 概述

本插件演示了 SiliconLife 插件系统中**被禁止的** P/Invoke 和 unsafe 代码操作。与其他有安全包装替代方案的禁止类别（文件 I/O、网络、进程、反射）不同，P/Invoke 和 unsafe 代码是**硬性禁止**，没有安全替代方案——它们不能通过任何 `PluginCapability` 声明来豁免。

## 为什么 P/Invoke 是终极威胁？

P/Invoke 和 unsafe 代码代表了对插件安全的**最根本威胁**，因为它们完全在**托管运行时之外**运行：

- 本地代码以完整进程权限执行
- 没有托管类型安全、内存安全或垃圾回收
- 无法拦截、审计或沙箱化本地调用
- 本地代码崩溃 = 整个进程崩溃（无异常处理）
- 可以访问进程空间中的任何内存地址

## 三重保险机制

PluginLoader 使用**三个独立的检测层**确保 P/Invoke 和 unsafe 代码永远无法逃过检测：

### 第一层：TypeRef 表扫描

检测 PE 元数据中对禁止类型的直接引用：

| 禁止类型 | 命名空间 | 威胁 |
|----------|----------|------|
| `DllImportAttribute` | System.Runtime.InteropServices | 声明本地函数导入 |
| `UnmanagedFunctionPointerAttribute` | System.Runtime.InteropServices | 本地函数指针 |
| `SuppressGCTransitionAttribute` | System.Runtime.InteropServices | 跳过 GC 转换 |
| `Marshal` | System.Runtime.InteropServices | 托管/非托管内存桥接 |
| `MemoryMarshal` | System.Runtime.InteropServices | Span 和原始内存交换 |
| `NativeMemory` | System.Runtime.InteropServices | 本地堆 malloc/free |
| `NativeLibrary` | System.Runtime.InteropServices | 动态加载本地共享库 |
| `GCHandle` | System.Runtime.InteropServices | 固定托管对象，暴露指针 |
| `SafeHandle` | System.Runtime.InteropServices | 本地资源句柄基类 |
| `Unsafe` | System.Runtime.CompilerServices | Unsafe 辅助类 |
| `UnverifiableCodeAttribute` | System.Security | 不可验证代码标记 |
| `SuppressUnmanagedCodeSecurityAttribute` | System.Security | 抑制安全检查 |

### 第二层：Unsafe 标记扫描（ScanUnsafeMarkers）

独立于类型引用，检测编译器生成的标记：

| 标记 | 检测方法 | 来源 |
|------|----------|------|
| `[assembly: UnverifiableCode]` | 程序集 CustomAttribute 表 | C# `unsafe` 关键字 |
| `[module: UnverifiableCode]` | 模块 CustomAttribute 表 | C# `unsafe` 关键字 |
| `MethodAttributes.PinvokeImpl` | MethodDef 表标志位 | `[DllImport]` 属性 |

### 第三层：IL 字符串扫描（#US 堆）

捕获引用 InteropServices 类型的字符串常量：

```
"System.Runtime.InteropServices.Marshal"  → 被标记
"System.Runtime.InteropServices.*"        → 前缀匹配被标记
```

## 为什么需要三层？

每一层都能捕获其他层可能遗漏的情况：

| 绕过尝试 | 第一层 | 第二层 | 第三层 |
|----------|--------|--------|--------|
| 正常 `[DllImport]` 使用 | ✅ 捕获 DllImportAttribute TypeRef | ✅ 捕获 PinvokeImpl 标志 | — |
| 混淆属性名称 | ❌ 可能遗漏 | ✅ PinvokeImpl 是原始标志，无法隐藏 | — |
| Type.GetType("...Marshal...") | ❌ 无直接 TypeRef | — | ✅ 字符串扫描捕获 |
| 无 P/Invoke 的 unsafe 块 | — | ✅ UnverifiableCode 属性 | — |

**结果：没有单一绕过能击败所有三层。**

## 演示的违规操作

### 违规 1：[DllImport] 声明

```csharp
// ❌ 禁止
[DllImport("kernel32.dll")]
private static extern ulong GetTickCount64();

[DllImport("user32.dll", CharSet = CharSet.Unicode)]
private static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);
```

**捕获方式：**
- `[TypeRef] System.Runtime.InteropServices.DllImportAttribute`
- `[PInvoke] GetTickCount64 (native interop)`（PinvokeImpl 标志）

### 违规 2：Marshal 使用

```csharp
// ❌ 禁止
IntPtr ptr = Marshal.AllocHGlobal(1024);
Marshal.WriteByte(ptr, 0xFF);
string? str = Marshal.PtrToStringAnsi(ptr);
Marshal.StructureToPtr(data, ptr, false);
Marshal.FreeHGlobal(ptr);
```

**捕获方式：** `[TypeRef] System.Runtime.InteropServices.Marshal`

### 违规 3：NativeMemory 使用

```csharp
// ❌ 禁止
unsafe
{
    void* buffer = NativeMemory.Alloc(4096);
    NativeMemory.Clear(buffer, 4096);
    buffer = NativeMemory.Realloc(buffer, 8192);
    NativeMemory.Free(buffer);
}
```

**捕获方式：**
- `[TypeRef] System.Runtime.InteropServices.NativeMemory`
- `[UnsafeMarker] [module: System.Security.UnverifiableCode]`

### 违规 4：GCHandle 固定

```csharp
// ❌ 禁止
byte[] managedArray = new byte[1024];
GCHandle handle = GCHandle.Alloc(managedArray, GCHandleType.Pinned);
IntPtr ptr = handle.AddrOfPinnedObject();
handle.Free();
```

**捕获方式：** `[TypeRef] System.Runtime.InteropServices.GCHandle`

### 违规 5：unsafe 代码块

```csharp
// ❌ 禁止
unsafe
{
    int value = 42;
    int* ptr = &value;
    *ptr = 100;
    byte* stack = stackalloc byte[256];
    int* next = ptr + 1;  // 指针算术！
}
```

**捕获方式：** `[UnsafeMarker] [module: System.Security.UnverifiableCode]`

### 违规 6：NativeLibrary 加载

```csharp
// ❌ 禁止
IntPtr lib = NativeLibrary.Load("evil.dll");
IntPtr funcPtr = NativeLibrary.GetExport(lib, "malicious_function");
var func = Marshal.GetDelegateForFunctionPointer<Action>(funcPtr);
func();  // 执行任意本地代码！
NativeLibrary.Free(lib);
```

**捕获方式：** `[TypeRef] System.Runtime.InteropServices.NativeLibrary`

## 无安全替代方案——对比

| 禁止类别 | 安全包装 | 可审计 | 可通过 PluginCapability 声明 |
|----------|----------|--------|------------------------------|
| 文件 I/O | PermissionedStreamFactory | ✅ 是 | ✅ Capability.FileIO |
| 网络 | NetworkExecutor | ✅ 是 | ✅ Capability.Network |
| 进程 | CommandLineExecutor | ✅ 是 | ✅ Capability.Process |
| 反射 | ITypeRegistry + IObjectFactory | ✅ 是 | ❌ 始终禁止 |
| **P/Invoke 和 unsafe** | **❌ 无** | **❌ 不可能** | **❌ 始终禁止** |

## 如果插件确实需要本地代码怎么办？

如果库合法使用了 P/Invoke 或 unsafe 代码（例如 Google.Protobuf 使用 `MemoryMarshal` 实现零拷贝序列化），必须：

1. **由项目维护者手动审计**
2. **添加到 PluginLoader 中的 `TrustedAssemblies` 白名单**
3. **通过 PE 元数据 `AssemblyDefinition.Name` 识别**（非文件名——防止重命名攻击）

## 文件

- `Plugin.cs` - 反例演示插件
- `README.md` - English
- `README.zh-CN.md` - 本文件（简体中文）
- `README.zh-HK.md` - 繁體中文
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

## 相关示例

- **04-SafeSystemIO**：System.IO 白名单安全类型
- **06-TrustedDependency**：TrustedAssemblies 白名单机制
- **10-ForbiddenReflection**：禁止的反射操作
- **12-ForbiddenStringBypass**：基于字符串的反射绕过尝试
