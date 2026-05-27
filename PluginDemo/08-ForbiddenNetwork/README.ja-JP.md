# PluginDemo-08: 禁止のネットワーク操作アンチパターン

## 概要

このプラグインは、SiliconLife プラグインシステムにおける**禁止**されたネットワーク操作を示します。逆のパターンを示し、何をすべきでないか、そして各違反に対して正しい代替手段を提供します。

## なぜ直接的なネットワークアクセスはグローバルに禁止されているのですか？

直接的なネットワークアクセスパターンはプラグインレベルでブロックされています：

1. **悪意のあるサーバーへの接続**: プラグインが攻撃コマンドを受信するために悪意のあるサーバーに接続する可能性があります
2. **データ漏洩**: プラグインがサンドボックスから外部サーバーに機密データを漏洩する可能性があります
3. **DNS リバインディング攻撃**: プラグインが DNS 操作を通じてセキュリティチェックをバイパスする可能性があります
4. **ネットワーク ACL バイパス**: 直接的なネットワークアクセスはグローバル ACL と権限システムをバイパスします
5. **監査証跡なし**: 直接的なネットワーク操作はプラグインのセキュリティ監査システムをバイパスします

## 禁止されている型

ネットワークに直接アクセスするすべての `System.Net` 型がブロックされています：

| 禁止型 | ブロックされる名前空間 | リスクレベル |
|----------|----------------|----------|
| `HttpClient` | `System.Net.Http` | 🔴 重大 |
| `TcpClient` | `System.Net.Sockets` | 🔴 重大 |
| `Socket` | `System.Net.Sockets` | 🔴 重大 |
| `Dns` | `System.Net` | 🔴 重大 |
| `WebClient` | `System.Net` | 🔴 重大 |

## 安全にアクセスする方法

### NetworkExecutor（推奨）

`NetworkExecutor` はプラグインのネットワーク操作の**制御されたエントリーポイント**です：

```csharp
// ✅ 正しい: 単純な GET リクエスト
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com/data"
});
```

**NetworkExecutor が提供する機能:**
1. 権限チェック
2. 監査ログ
3. サーキットブレーカー
4. タイムアウト制御
5. リクエストキュー

## 違反の演示

### 違反 1: HttpClient

```csharp
// ❌ 禁止
using var client = new HttpClient();
var response = await client.GetStringAsync("https://api.example.com");

// ✅ 正しい
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com"
});
```

### 違反 2: TcpClient

```csharp
// ❌ 禁止
using var client = new TcpClient("example.com", 8080);

// ✅ 正しい
// NetworkExecutor を使用するか、Capability.Network を宣言
```

### 違反 3: Dns

```csharp
// ❌ 禁止
var hostEntry = Dns.GetHostEntry("example.com");

// ✅ 正しい
// NetworkExecutor が必要な DNS 解決を内部的に処理
```

## PluginLoader のセキュリティ機構

PluginLoader はこのプラグインをスキャンする際に：
1. **TypeRef スキャン**: 禁止された `System.Net.*` 型への参照を検出
2. **MemberRef スキャン**: ブロックされたメソッドへの呼び出しを検出
3. **IL 文字列スキャン**: 反射による禁止型の読み込みを試みる検出
4. **拒否**: プラグインは読み込み時に拒否されます

## ファイル

- `Plugin.cs` - アンチパターンの演示プラグイン
- `README.md` - このファイル（英語）
- `README.ja-JP.md` - このファイル（日本語）
- 他の言語バージョン...

## 関連する例

- **13-CapabilityNetwork**: 宣言的な Network 権限
- **07-ForbiddenFileIO**: 禁止されたファイルアクセスパターン