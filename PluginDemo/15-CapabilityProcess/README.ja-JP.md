# PluginDemo-15: Capability.Process — 宣言型プロセス権限

## 概要

このプラグインは、`[PluginCapability(Capability.Process)]` を使用してプラグインが子プロセスを起動する能力を必要とすることを宣言する方法を示します。この機能を宣言することで、`System.Diagnostics.Process` および関連型にアクセスできるようになります。

## 宣言構文

```csharp
[PluginCapability(Capability.Process, Reason = "Launch build tools for CI pipeline")]
public class CapabilityProcessPlugin : IPlugin { ... }
```

## Capability.Process の免除範囲

### TypeRef 免除

`System.Diagnostics` 下の Process 関連型のみ免除されます：

| 免除される型 | 用途 |
|------------|------|
| `Process` | 子プロセスの開始、管理、監視 |
| `ProcessStartInfo` | プロセス起動パラメータの設定 |
| `ProcessThread` | プロセススレッド情報へのアクセス |
| `ProcessModule` | プロセスモジュール情報へのアクセス |
| `ProcessPriorityClass` | プロセス優先度の設定 |
| `ProcessWindowStyle` | プロセスウィンドウスタイルの設定 |

常に許可される型（禁止リストに含まれない）：`Stopwatch`、`Debug`、`Trace`、`Activity`

### ILString 免除

- `"System.Diagnostics.Process"` で始まる文字列はフラグされない

## 09-ForbiddenProcess との比較

| 側面 | 09-ForbiddenProcess | 15-CapabilityProcess |
|------|-------------------|---------------------|
| 宣言 | なし | `[PluginCapability(Capability.Process)]` |
| Process.Start | ❌ 拒否される | ✅ 許可される |
| ProcessStartInfo | ❌ 拒否される | ✅ 許可される |

## 推奨：CommandLineExecutor

`Capability.Process` があっても、`CommandLineExecutor` の使用を優先することが推奨されます：

| 機能 | CommandLineExecutor | 直接 Process |
|------|-------------------|-------------|
| 機能宣言の必要性 | 不要 | 必要 |
| サンドボックス | コマンド許可リスト | なし |
| タイムアウト | 内蔵 | 手動 |
| 出力キャプチャ | 構造化 | 手動 |
| 監査ログ | 自動 | 手動 |

I/O ストリームのきめ細かい制御、プロセスイベントの処理、または CommandLineExecutor のコマンド許可リストが厳しすぎる場合にのみ、`Capability.Process` + 直接 `Process` を使用してください。

## セキュリティベストプラクティス

1. **CommandLineExecutor を優先**：可能な限り制御されたエントリポイントを使用
2. **明確な Reason を提供**：「Launch build tools for CI pipeline」と「process access」の違い
3. **すべての入力を検証**：信頼されていない入力を ProcessStartInfo に直接渡さない
4. **WaitForExit を使用**：ゾンビプロセスを防ぐため常にプロセス完了を待機
5. **ストリームをリダイレクト**：`RedirectStandardOutput = true` と `UseShellExecute = false` を設定

## ファイル

- `Plugin.cs` — Capability.Process を宣言するデモプラグイン
- `README.md` — このファイル（英語）
- `README.zh-CN.md` — 簡体字中国語
- 翻訳：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 関連例

- **09-ForbiddenProcess**：ブロックされるプロセス操作のアンチパターン
- **18-CapabilityDenied**：宣言不可機能のアンチパターン
