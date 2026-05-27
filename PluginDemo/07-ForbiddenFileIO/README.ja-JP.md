# 禁止されたファイル I/O 操作のアンチパターン

プラグインシステムで**禁止されている**ファイル I/O 操作を実演します。本サンプルはアンチパターンの参照として、やってはいけないことを示し、各違反に対する正しい代替手段を提供します。

## なぜ System.IO が全面禁止なのか？

`System.IO` 名前空間全体がプラグインレベルでブロックされています。直接的なファイルアクセスは深刻なセキュリティリスクをもたらすためです：

1. **未承認のファイルアクセス**：プラグインがワークスペース外の機密ファイル（パスワード、鍵、個人データ）を読み取る可能性
2. **ファイル上書き攻撃**：悪意あるプラグインが重要なシステムファイルや設定ファイルを上書きする可能性
3. **ディレクトリトラバーサル**：プラグインが `../` パスでワークスペース境界を逸脱する可能性
4. **リソース枯渇**：制御されないファイル作成がディスク容量を使い切る可能性
5. **監査証跡なし**：直接ファイル操作はプラグインセキュリティ監査システムを迂回

## 禁止される型

ファイルシステムに直接アクセスするすべての `System.IO` 型がブロックされます：

| 禁止される型 | ブロックされるメソッド | リスクレベル |
|-------------|---------------------|------------|
| `File` | `ReadAllText`, `WriteAllText`, `AppendAllText` 等 | 🔴 重大 |
| `FileStream` | ファイルパス付きコンストラクタ | 🔴 重大 |
| `Directory` | `GetFiles`, `GetDirectories`, `CreateDirectory` | 🔴 重大 |
| `StreamReader` | ファイルパス（文字列）付きコンストラクタ | 🔴 重大 |
| `StreamWriter` | ファイルパス（文字列）付きコンストラクタ | 🔴 重大 |
| `FileInfo` | すべてのメソッド | 🔴 重大 |
| `DirectoryInfo` | すべてのメソッド | 🔴 重大 |

## 許可される型（ホワイトリスト例外）

**純粋なメモリ操作**（ファイルシステムへの直接アクセスなし）を行う型は許可されます：

| 許可される型 | 用途 | なぜ安全か |
|-------------|------|-----------|
| `MemoryStream` | メモリ内バイトストリーム | ファイルシステムアクセスなし |
| `BinaryReader` | 既存ストリームから読み取り | ストリームをラップ、ファイルを開かない |
| `BinaryWriter` | 既存ストリームに書き込み | ストリームをラップ、ファイルを作成しない |
| `GZipStream` | 圧縮/展開 | ストリームをラップ、ファイルアクセスなし |
| `StreamReader` | `Stream` パラメータ付きコンストラクタ | 監査済みストリームをラップする場合は安全 |
| `StreamWriter` | `Stream` パラメータ付きコンストラクタ | 監査済みストリームをラップする場合は安全 |

詳しくは **04-SafeSystemIO** サンプルを参照してください。

## PermissionedStreamFactory による安全なファイルアクセス

`PermissionedStreamFactory` はプラグインにおけるファイル操作の**制御されたエントリポイント**です：

```csharp
// ✅ 正しい：ファイル読み取り
using var readStream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(readStream);
string content = reader.ReadToEnd();

// ✅ 正しい：ファイル書き込み
using var writeStream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(writeStream);
writer.Write("ログデータ");
```

**PermissionedStreamFactory が提供する機能：**
1. **パス検証**：ディレクトリトラバーサル攻撃（`../`）を防止
2. **権限チェック**：ファイルが許可されたワークスペース内にあることを確認
3. **監査ログ**：すべてのファイルアクセスがセキュリティレビューのために記録
4. **リソースクリーンアップ**：オープンストリームを追跡しリークを防止

## 本サンプルの違反行為

### 違反 1：File.ReadAllText

```csharp
// ❌ 禁止 — ⚠️ VIOLATION: [TypeRef] System.IO.File::ReadAllText
string content = File.ReadAllText("config.json");

// ✅ 正しい代替手段
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
string content = reader.ReadToEnd();
```

### 違反 2：File.WriteAllText

```csharp
// ❌ 禁止 — ⚠️ VIOLATION: [TypeRef] System.IO.File::WriteAllText
File.WriteAllText("output.log", "some data");

// ✅ 正しい代替手段
using var stream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(stream);
writer.Write("some data");
```

### 違反 3：直接 FileStream

```csharp
// ❌ 禁止 — ⚠️ VIOLATION: [TypeRef] System.IO.FileStream::.ctor
using var fs = new FileStream("data.bin", FileMode.Open);

// ✅ 正しい代替手段
using var fs = PermissionedStreamFactory.OpenRead("data.bin");
```

### 違反 4：Directory.GetFiles

```csharp
// ❌ 禁止 — ⚠️ VIOLATION: [TypeRef] System.IO.Directory::GetFiles
string[] files = Directory.GetFiles("./logs", "*.txt");

// ✅ 正しい代替手段（SpeedyPack 使用）
using var pack = SpeedyPack.Open("logs.spk");
var entries = pack.ListEntries("/");
```

### 違反 5：直接パス付き StreamReader

```csharp
// ❌ 禁止 — ⚠️ VIOLATION: [TypeRef] System.IO.StreamReader::.ctor(string)
using var reader = new StreamReader("config.json");

// ✅ 正しい代替手段
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
```

## 他のサンプルとの比較

| サンプル | 重点 | 必要な権限 |
|---------|------|-----------|
| **04-SafeSystemIO** | 許可されたメモリ型（MemoryStream、GZipStream） | なし |
| **07-ForbiddenFileIO** | 禁止されたファイルアクセスパターン（本サンプル） | 該当なし（ブロック） |
| **14-CapabilityFileIO** | FileIO 能力を宣言して制限を回避 | `Capability.FileIO` |

## PluginLoader セキュリティスキャン機構

PluginLoader がこのプラグインをスキャンする際：

1. **TypeRef スキャン**：禁止された `System.IO` 型への参照を検出
2. **MemberRef スキャン**：ブロックされたメソッドの呼び出しを検出
3. **IL 文字列スキャン**：文字列ベースのリフレクション回避試行を検出
4. **拒否**：プラグインはロード時に詳細なエラーメッセージとともに拒否

文字列連結、リフレクション、動的ロード、難読化による回避は不可能——IL レベルのスキャンで捕捉されます（**12-ForbiddenStringBypass** 参照）。

## セキュリティ注記

ファイルへの無制限アクセスが本当に必要な場合は `Capability.FileIO` を宣言できます（14-CapabilityFileIO 参照）。ただしベストプラクティスは：
- 構造化データの保存には **SpeedyPack** を優先（権限宣言不要）
- ファイルアクセスが必要な場合は **PermissionedStreamFactory** を使用（制御されたエントリポイント）
- 上記で対応できない場合にのみ `Capability.FileIO` を宣言
