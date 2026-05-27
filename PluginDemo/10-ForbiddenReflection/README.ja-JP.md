# PluginDemo-10: 禁止されたリフレクション操作のアンチパターン

## 概要

このプラグインは、SiliconLife プラグインシステムにおいて**禁止された**リフレクション操作を示します。アンチパターンリファレンスとして、やってはいけないことを示し、各違反に対する正しい代替案を提供します。

## なぜリフレクションが核心的な脅威なのか？

リフレクションバイパスは PluginLoader のセキュリティスキャンにとって**最も重大な脅威**です。TypeRef スキャンはコンパイル時に直接型参照を捕捉できますが、リフレクションメソッドは**実行時**に文字列を使って型を解決でき、静的メタデータスキャンからは完全に不可視です。

プラグインが `Type.GetType("System.IO.File, System.Runtime")` を呼び出せれば、PE メタデータの TypeRef テーブルに何の参照もなく、任意の禁止された型にアクセスできます。

## どのメソッドが禁止されているか？

すべての禁止メソッドは **MemberRef スキャン**で検出されます（名前空間やタイプレベルのブロックではありません）：

| 禁止メソッド | シグネチャ | 脅威 |
|-------------|----------|------|
| `Type.GetType` | `System.Type::GetType(System.String)` | 実行時に名前で任意の型を解決 |
| `Activator.CreateInstance` | `System.Activator::CreateInstance(...)` | 任意の型をインスタンス化 |
| `Activator.CreateInstanceFrom` | `System.Activator::CreateInstanceFrom(...)` | DLL パスからインスタンス生成 |
| `Assembly.Load` | `System.Reflection.Assembly::Load(...)` | 名前/バイト配列でアセンブリをロード |
| `Assembly.LoadFile` | `System.Reflection.Assembly::LoadFile(...)` | ディスクからアセンブリをロード |
| `Assembly.LoadFrom` | `System.Reflection.Assembly::LoadFrom(...)` | パスからアセンブリをロード |
| `Assembly.UnsafeLoadFrom` | `System.Reflection.Assembly::UnsafeLoadFrom(...)` | セキュリティチェックなしでロード |
| `Assembly.LoadWithPartialName` | `System.Reflection.Assembly::LoadWithPartialName(...)` | 部分名でロード |
| `Assembly.ReflectionOnlyLoad` | `System.Reflection.Assembly::ReflectionOnlyLoad(...)` | リフレクション専用ロード |
| `Assembly.GetType` | `System.Reflection.Assembly::GetType(System.String)` | 文字列ベースの型解決 |

## 何が安全か？

すべてのリフレクションが禁止されているわけではありません。以下のパターンは、コンパイル時に既知の型を参照するため**安全**です：

| 安全なパターン | 例 | なぜ安全か |
|---------------|---|-----------|
| `typeof(X).Assembly` | `typeof(MyPlugin).Assembly` | 型はコンパイル時に既知、TypeRef で可視 |
| `typeof(X).GetProperties()` | `typeof(MyData).GetProperties()` | 既知の型の検査、新しい型は導入されない |
| `typeof(X).GetMethods()` | `typeof(IPlugin).GetMethods()` | 既知の型のメンバー検査 |
| ジェネリック制約 | `FindSubtypesOf(typeof(BaseTool))` | ジェネリックパラメータはコンパイル時の型 |
| `nameof()` | `nameof(MyClass.MyMethod)` | コンパイル時の文字列、実行時解決なし |

**重要な区別：**
- `typeof(X).Assembly` → **安全**（コンパイル時参照、PluginLoader がスキャン）
- `Assembly.Load("X")` → **禁止**（実行時の文字列、すべてのスキャンをバイパス）

## リフレクションを安全に置き換えるには？

### ITypeRegistry を使用（Type.GetType + AppDomain スキャンの代替）

```csharp
// ❌ 禁止：実行時に文字列で型を解決
Type? type = Type.GetType("MyNamespace.MyClass, MyAssembly");

// ✅ 正しい：ITypeRegistry で登録済み型を検索
Type? type = typeRegistry.FindType("MyNamespace.MyClass");
// OnLoad 中に登録された型のみ発見可能
```

### IObjectFactory を使用（Activator.CreateInstance の代替）

```csharp
// ❌ 禁止：任意のインスタンスを生成
object? instance = Activator.CreateInstance(someType);

// ✅ 正しい：IObjectFactory で登録済みファクトリから生成
var instance = objectFactory.CreateInstance<MyService>();
// ファクトリが登録された型のみインスタンス化可能
```

## 示された違反

このプラグインは 5 つの一般的なリフレクション違反を示します：

### 違反 1：Type.GetType(string)

```csharp
// ❌ 禁止
Type? fileType = Type.GetType("System.IO.File, System.Runtime");
var method = fileType?.GetMethod("ReadAllText");
method?.Invoke(null, new object[] { "secret.txt" });

// ✅ 正しい
Type? myType = typeRegistry.FindType("MyPlugin.MyCustomType");
```

**ブロックされた MemberRef**：`System.Type::GetType(System.String)`

### 違反 2：Activator.CreateInstance

```csharp
// ❌ 禁止
Type? httpClientType = Type.GetType("System.Net.Http.HttpClient, System.Net.Http");
object? client = Activator.CreateInstance(httpClientType!);

// ✅ 正しい
var instance = objectFactory.CreateInstance<MyService>();
```

**ブロックされた MemberRef**：`System.Activator::CreateInstance`

### 違反 3：Assembly.Load

```csharp
// ❌ 禁止
Assembly asm = Assembly.Load("System.Net.Http");
Type? httpType = asm.GetType("System.Net.Http.HttpClient");
object? client = Activator.CreateInstance(httpType!);

// ✅ 正しい
Assembly myAsm = typeof(MyPlugin).Assembly;  // 安全：コンパイル時に既知
Type? type = typeRegistry.FindType("MyPlugin.SomeType");
```

**ブロックされた MemberRef**：`System.Reflection.Assembly::Load(System.String)`

### 違反 4：Assembly.LoadFile / LoadFrom

```csharp
// ❌ 禁止
Assembly asm = Assembly.LoadFile(@"C:\malware\evil.dll");
Assembly asm2 = Assembly.LoadFrom(@"\\network\share\trojan.dll");

// ✅ 正しい
// すべての依存関係はプラグインディレクトリに配置し、PluginLoader でスキャンされる必要があります。
// OnLoad で ITypeRegistry.RegisterFromAssembly を使用して自身のアセンブリを登録してください。
```

**ブロックされた MemberRef**：`System.Reflection.Assembly::LoadFile(System.String)` / `LoadFrom(System.String)`

### 違反 5：Assembly.GetType(string)

```csharp
// ❌ 禁止
Assembly runtime = typeof(object).Assembly;
Type? processType = runtime.GetType("System.Diagnostics.Process");

// ✅ 正しい
Type? safeType = typeRegistry.FindType("MyPlugin.MySafeType");
// 禁止された型は決して登録されないため、見つかることはありません
```

**ブロックされた MemberRef**：`System.Reflection.Assembly::GetType(System.String)`

## PluginLoader セキュリティメカニズム

PluginLoader がこのプラグインをスキャンする際：

1. **MemberRef スキャン**：禁止メソッドの呼び出しを検出
2. **TypeRef スキャン**：禁止型への直接参照を検出（補助チェック）
3. **IL 文字列スキャン**：禁止型パターンに一致する文字列定数を検出（多層防御）
4. **拒否**：すべての違反を列挙した詳細なエラーメッセージと共にプラグインがロード時に拒否

## typeof(X).Assembly が安全で Assembly.Load が安全でない理由

| 操作 | 可視性 | セキュリティ |
|------|--------|------------|
| `typeof(X).Assembly` | 型 X は TypeRef テーブルに存在 → PluginLoader がスキャン | ✅ 安全 |
| `Assembly.Load("X")` | 文字列 "X" は実行時のみ存在 → TypeRef スキャンに不可視 | ❌ 禁止 |
| `obj.GetType()` | 既存インスタンスの型を返す → 新しい型は導入されない | ✅ 安全 |
| `Type.GetType("X")` | 文字列から任意の型を解決 → TypeRef をバイパス | ❌ 禁止 |

## ベストプラクティス

1. **OnLoad で型を登録する**：`ITypeRegistry.RegisterType` / `RegisterFromAssembly` を使用
2. **動的生成には IObjectFactory を使用する**：`Activator.CreateInstance` は決して使わない
3. **typeof(X).Assembly を活用する**：自身のアセンブリを安全に参照
4. **文字列ベースの型名を避ける**：IL 文字列スキャンをトリガーしフラグされる可能性がある
5. **静的発見可能性のために設計する**：PluginLoader がメタデータで見えなければ疑わしい

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

## 関連する例

- **02-TypeRegistryUsage**：ITypeRegistry の正しい使い方
- **03-ObjectFactoryUsage**：IObjectFactory の正しい使い方
- **11-ForbiddenPInvoke**：禁止された P/Invoke と unsafe コード
- **12-ForbiddenStringBypass**：文字列ベースのリフレクションバイパス試行
