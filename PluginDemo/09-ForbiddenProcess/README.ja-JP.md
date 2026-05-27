# PluginDemo-09: 禁止されたプロセス操作のアンチパターン

## 概要

このプラグインは、SiliconLife プラグインシステムにおける**禁止された**プロセス実行操作を実演します。アンチパターンのリファレンスとして、やってはいけないことを示し、各違反に対する正しい代替手段を提供します。

## なぜ Process 型は禁止されているのか？

`System.Diagnostics.Process` と `ProcessStartInfo` はプラグインでブロックされています。直接的なプロセス実行には深刻なセキュリティリスクがあるためです：

1. **任意コマンド実行**：監査や権限チェックなしに任意のコマンドを実行可能
2. **マルウェア起動**：悪意のあるプラグインが不要なアプリケーションやスクリプトを実行可能
3. **システムリソースアクセス**：プラグインサンドボックス外の機密リソースにアクセス可能
4. **コマンド検証なし**：直接の Process.Start にはコマンドインジェクション保護が組み込まれていない
5. **監査証跡なし**：直接プロセス操作はプラグインセキュリティ監査をバイパス
6. **権限昇格**：プラグインの権限レベルを超えたプロセス生成の可能性

## どの型が禁止されているか？

Process 関連の型のみが禁止されており、**System.Diagnostics 名前空間全体ではありません**：

| 禁止された型 | ブロックされるメソッド | リスクレベル |
|-------------|---------------------|------------|
| `Process` | `Start()`、`Kill()`、`WaitForExit()` | 🔴 重大 |
| `ProcessStartInfo` | コンストラクタ、全プロパティ | 🔴 重大 |
| `Process` | `StandardInput`、`StandardOutput`、`StandardError` | 🔴 重大 |
| `Process` | `GetProcesses()`、`GetProcessesByName()` | 🟡 高 |

## どの型が許可されているか？

プロセス実行に関わらない他の `System.Diagnostics` 型は引き続き利用可能：

| 許可された型 | 用途 | 安全な理由 |
|-------------|------|-----------|
| `Stopwatch` | 時間計測 | プロセス実行なし |
| `Debug` | デバッグ出力 | セキュリティリスクなし |
| `Trace` | トレース/ロギング | セキュリティリスクなし |
| `PerformanceCounter` | パフォーマンス監視 | 読み取り専用、監査対象 |

## 安全にコマンドを実行するには？

### CommandLineExecutor を使用（唯一の安全な方法）

`CommandLineExecutor` はプラグインにおけるコマンド実行の**制御されたエントリポイント**です：

```csharp
// ✅ 正しい：コマンドの実行
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);

if (result.Success)
{
    Console.WriteLine(result.Output);
}
else
{
    Console.WriteLine($"エラー: {result.Error}");
}
```

**CommandLineExecutor が提供する機能：**
1. **コマンドインジェクション保護**：危険なセパレータをブロック（`||`、`&&`、`|`、`&`、`;`）
2. **タイムアウト強制**：デフォルト30秒タイムアウト（設定可能）
3. **監査ログ**：すべてのコマンド実行がセキュリティレビュー用に記録
4. **出力キャプチャ**：stdout と stderr を自動キャプチャ
5. **クロスプラットフォーム対応**：Windows では `cmd.exe`、Unix では `/bin/bash`
6. **エラーハンドリング**：成功/失敗ステータス付きの構造化結果を返却

## 実演される違反

このプラグインは5つの一般的なプロセス実行違反を示します：

### 違反 1：Process.Start

```csharp
// ❌ 禁止
Process.Start("notepad.exe");

// ✅ 正しい
var request = new ExecutorRequest { ResourcePath = "notepad.exe" };
var result = CommandLineExecutor.Execute(request);
```

**ブロックされる TypeRef**：`System.Diagnostics.Process::Start(System.String)`

### 違反 2：ProcessStartInfo

```csharp
// ❌ 禁止
var psi = new ProcessStartInfo {
    FileName = "cmd.exe",
    Arguments = "/c dir",
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = new Process { StartInfo = psi };
process.Start();

// ✅ 正しい
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);
Console.WriteLine(result.Output);
```

**ブロックされる TypeRef**：`System.Diagnostics.ProcessStartInfo::.ctor()`

### 違反 3：引数付き Process

```csharp
// ❌ 禁止
var psi = new ProcessStartInfo("ping", "127.0.0.1 -n 4") {
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = Process.Start(psi);
process.WaitForExit();

// ✅ 正しい
var request = new ExecutorRequest { ResourcePath = "ping 127.0.0.1 -n 4" };
var result = CommandLineExecutor.Execute(request, TimeSpan.FromSeconds(10));
Console.WriteLine(result.Output);
```

**ブロックされる TypeRef**：`System.Diagnostics.Process::Start(ProcessStartInfo)`

### 違反 4：プロセス出力リダイレクト

```csharp
// ❌ 禁止
var psi = new ProcessStartInfo("ipconfig") {
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true
};
using var process = Process.Start(psi);
string output = process.StandardOutput.ReadToEnd();
string error = process.StandardError.ReadToEnd();

// ✅ 正しい
var request = new ExecutorRequest { ResourcePath = "ipconfig" };
var result = CommandLineExecutor.Execute(request);
if (result.Success) Console.WriteLine(result.Output);
else Console.WriteLine(result.Error);
```

**ブロックされる TypeRef**：`System.Diagnostics.Process::StandardOutput`

### 違反 5：Process.Kill

```csharp
// ❌ 禁止
Process[] processes = Process.GetProcessesByName("notepad");
foreach (var p in processes) p.Kill();

// ✅ 正しい
// セキュリティ上の理由から、CommandLineExecutor はプロセスの終了をサポートしていません。
// 必要な場合はシステム管理者にお問い合わせください。
```

**ブロックされる TypeRef**：`System.Diagnostics.Process::Kill()`

## なぜ System.Diagnostics 全体ではなく Process だけが禁止されるのか？

プラグインシステムは**精密なアプローチ**でセキュリティを確保します：

- **危険な型のみブロック**：Process/ProcessStartInfo は任意コード実行を可能にする
- **安全な型は許可**：Stopwatch、Debug、Trace にはセキュリティ上の問題なし
- **影響を最小化**：開発者はリスクのない診断ツールを引き続き使用可能
- **明確な境界**：プロセスの生成/終了が可能な型のみが禁止

これは `System.IO` とは異なります。IO は**全体がブロック**されます。ほとんどの IO 型がファイルシステムに直接アクセスするためです。

## PluginLoader セキュリティメカニズム

PluginLoader がこのプラグインをスキャンする際：

1. **TypeRef スキャン**：禁止された `Process`/`ProcessStartInfo` 型への参照を検出
2. **MemberRef スキャン**：ブロックされたメソッドの呼び出しを検出（例：`Process.Start`）
3. **IL 文字列スキャン**：文字列ベースのリフレクションによる禁止型ロード試行を検出
4. **拒否**：プラグインはロード時に詳細なエラーメッセージと共に拒否

## ベストプラクティス

1. **常に CommandLineExecutor を使用**：`Process.Start` を直接使用しない
2. **適切なタイムアウトを設定**：コマンドの無限ハングを防止
3. **結果を確認**：出力を使用する前に必ず `result.Success` を検証
4. **入力をサニタイズ**：ユーザー入力をコマンドに直接渡さない
5. **必要に応じて Capability を宣言**：無制限のプロセス実行が本当に必要な場合は `Capability.Process` を宣言（15-CapabilityProcess 参照）

## ファイル

- `Plugin.cs` - アンチパターンデモプラグイン
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - このファイル（日本語）
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## 関連例

- **08-ForbiddenNetwork**：禁止されたネットワーク操作
- **15-CapabilityProcess**：宣言的 Process 権限
- **10-ForbiddenReflection**：禁止されたリフレクション操作
- **12-ForbiddenStringBypass**：文字列ベースのリフレクションバイパス試行
