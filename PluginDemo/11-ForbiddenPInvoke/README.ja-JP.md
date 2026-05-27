# PluginDemo-11: 禁止された P/Invoke と unsafe コードのアンチパターン

## 概要

本プラグインは、SiliconLife プラグインシステムにおいて**禁止されている** P/Invoke および unsafe コード操作をデモンストレーションします。安全なラッパー代替手段がある他の禁止カテゴリ（ファイル I/O、ネットワーク、プロセス、リフレクション）とは異なり、P/Invoke と unsafe コードは**絶対禁止**であり、安全な代替手段がなく、いかなる `PluginCapability` 宣言でも免除できません。

## なぜ P/Invoke が究極の脅威なのか？

P/Invoke と unsafe コードは、**マネージドランタイムの完全に外側**で動作するため、プラグインセキュリティに対する**最も根本的な脅威**です：

- ネイティブコードは完全なプロセス権限で実行
- マネージド型安全性、メモリ安全性、ガベージコレクションなし
- ネイティブ呼び出しを傍受、監査、サンドボックス化不可能
- ネイティブコードのクラッシュ = プロセス全体のクラッシュ（例外処理なし）
- プロセス空間の任意のメモリアドレスにアクセス可能

## 三重保険メカニズム

PluginLoader は**3つの独立した検出レイヤー**を使用して P/Invoke と unsafe コードの検出漏れを防ぎます：

### レイヤー1：TypeRef テーブルスキャン

PE メタデータ内の禁止型への直接参照を検出：

| 禁止型 | 名前空間 | 脅威 |
|--------|----------|------|
| `DllImportAttribute` | System.Runtime.InteropServices | ネイティブ関数インポート宣言 |
| `Marshal` | System.Runtime.InteropServices | マネージド/アンマネージドメモリブリッジ |
| `NativeMemory` | System.Runtime.InteropServices | ネイティブヒープ malloc/free |
| `NativeLibrary` | System.Runtime.InteropServices | ネイティブ共有ライブラリの動的ロード |
| `GCHandle` | System.Runtime.InteropServices | マネージドオブジェクトの固定、ポインタ公開 |
| `SafeHandle` | System.Runtime.InteropServices | ネイティブリソースハンドル基底クラス |
| `Unsafe` | System.Runtime.CompilerServices | Unsafe ヘルパークラス |
| `UnverifiableCodeAttribute` | System.Security | 検証不可能コードマーカー |

### レイヤー2：Unsafe マーカースキャン（ScanUnsafeMarkers）

型参照とは独立して、コンパイラ生成マーカーを検出：

| マーカー | 検出方法 | ソース |
|----------|----------|--------|
| `[assembly: UnverifiableCode]` | アセンブリ CustomAttribute テーブル | C# `unsafe` キーワード |
| `[module: UnverifiableCode]` | モジュール CustomAttribute テーブル | C# `unsafe` キーワード |
| `MethodAttributes.PinvokeImpl` | MethodDef テーブルフラグ | `[DllImport]` 属性 |

### レイヤー3：IL 文字列スキャン（#US ヒープ）

InteropServices 型を参照する文字列定数をキャッチ：

```
"System.Runtime.InteropServices.Marshal"  → フラグ付き
"System.Runtime.InteropServices.*"        → プレフィックスマッチでフラグ付き
```

## デモンストレーションされる違反

### 違反 1：[DllImport] 宣言

```csharp
// ❌ 禁止
[DllImport("kernel32.dll")]
private static extern ulong GetTickCount64();
```

**検出方法：**
- `[TypeRef] System.Runtime.InteropServices.DllImportAttribute`
- `[PInvoke] GetTickCount64 (native interop)`（PinvokeImpl フラグ）

### 違反 2：Marshal 使用

```csharp
// ❌ 禁止
IntPtr ptr = Marshal.AllocHGlobal(1024);
string? str = Marshal.PtrToStringAnsi(ptr);
Marshal.FreeHGlobal(ptr);
```

**検出方法：** `[TypeRef] System.Runtime.InteropServices.Marshal`

### 違反 3：NativeMemory 使用

```csharp
// ❌ 禁止
unsafe
{
    void* buffer = NativeMemory.Alloc(4096);
    NativeMemory.Free(buffer);
}
```

**検出方法：**
- `[TypeRef] System.Runtime.InteropServices.NativeMemory`
- `[UnsafeMarker] [module: System.Security.UnverifiableCode]`

### 違反 4：GCHandle 固定

```csharp
// ❌ 禁止
GCHandle handle = GCHandle.Alloc(managedArray, GCHandleType.Pinned);
IntPtr ptr = handle.AddrOfPinnedObject();
handle.Free();
```

**検出方法：** `[TypeRef] System.Runtime.InteropServices.GCHandle`

### 違反 5：unsafe ブロック

```csharp
// ❌ 禁止
unsafe
{
    int* ptr = &value;
    *ptr = 100;
    byte* stack = stackalloc byte[256];
}
```

**検出方法：** `[UnsafeMarker] [module: System.Security.UnverifiableCode]`

### 違反 6：NativeLibrary ロード

```csharp
// ❌ 禁止
IntPtr lib = NativeLibrary.Load("evil.dll");
IntPtr funcPtr = NativeLibrary.GetExport(lib, "malicious_function");
NativeLibrary.Free(lib);
```

**検出方法：** `[TypeRef] System.Runtime.InteropServices.NativeLibrary`

## 安全な代替手段なし——比較

| 禁止カテゴリ | 安全ラッパー | 監査可能 | PluginCapability で宣言可能 |
|--------------|------------|----------|---------------------------|
| ファイル I/O | PermissionedStreamFactory | ✅ はい | ✅ Capability.FileIO |
| ネットワーク | NetworkExecutor | ✅ はい | ✅ Capability.Network |
| プロセス | CommandLineExecutor | ✅ はい | ✅ Capability.Process |
| リフレクション | ITypeRegistry + IObjectFactory | ✅ はい | ❌ 常に禁止 |
| **P/Invoke と unsafe** | **❌ なし** | **❌ 不可能** | **❌ 常に禁止** |

## プラグインが本当にネイティブコードを必要とする場合

ライブラリが P/Invoke や unsafe コードを正当に使用している場合：

1. **プロジェクトメンテナによる手動監査**が必要
2. **PluginLoader の `TrustedAssemblies` ホワイトリストに追加**が必要
3. **PE メタデータの `AssemblyDefinition.Name` で識別**（ファイル名ではない——リネーム攻撃を防止）

## ファイル

- `Plugin.cs` - アンチパターンデモプラグイン
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 本ファイル（日本語）
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## 関連サンプル

- **04-SafeSystemIO**：System.IO ホワイトリスト安全型
- **06-TrustedDependency**：TrustedAssemblies ホワイトリストメカニズム
- **10-ForbiddenReflection**：禁止されたリフレクション操作
- **12-ForbiddenStringBypass**：文字列ベースのリフレクションバイパス試行
