# PluginDemo-13: Capability.Network — 宣言型ネットワーク権限

## 概要

このプラグインは、`[PluginCapability(Capability.Network)]` を使用してプラグインがネットワークアクセスを必要とすることを宣言する方法を示します。この機能を宣言することで、PluginLoaderのセキュリティスキャンでブロックされる `System.Net.*` 型にアクセスできるようになります。

## PluginCapability 宣言構文

```csharp
[PluginCapability(Capability.Network, Reason = "Calls weather REST API to retrieve forecast data")]
public class CapabilityNetworkPlugin : IPlugin { ... }
```

**主要要素：**
- **属性ターゲット**：`IPlugin` を直接実装するクラスに指定する必要があります
- **AllowMultiple = true**：複数の `[PluginCapability]` 属性をスタック可能（17-CapabilityStacked 参照）
- **Reason フィールド**：読み込み時にセキュリティ監査ログに書き込まれる人間が読める説明。**本番プラグインでは明確な Reason の提供を強く推奨します。**

## PluginLoader が機能宣言を処理する方法

1. **PE メタデータ読み取り**：PluginLoader はセキュリティスキャン開始**前**に PE ファイルの CustomAttribute テーブルから機能宣言を読み取ります
2. **スキャンルールの緩和**：宣言された機能は、対応する型参照を禁止名前空間および禁止型チェックから免除します
3. **監査ログ**：すべての宣言（Reason を含む）がセキュリティ監査ログに書き込まれます
4. **宣言不可能な機能**：P/Invoke、Unsafe、Reflection.Emit などは宣言の有無にかかわらず常にブロックされます

## Capability.Network の免除範囲

### TypeRef 免除

`Capability.Network` を宣言すると、以下の名前空間ベースおよび型ベースの禁止ルールが緩和されます：

| 免除名前空間 | 許可される型 |
|-------------|-----------|
| `System.Net.Http` | `HttpClient`、`HttpRequestMessage`、`HttpResponseMessage` 等 |
| `System.Net.WebSockets` | `ClientWebSocket`、`WebSocket` 等 |
| `System.Net.Sockets` | `TcpClient`、`UdpClient`、`Socket` 等 |
| `System.Net.Mail` | `SmtpClient`、`MailMessage` 等 |
| `System.Net.NetworkInformation` | `Ping`、`NetworkInterface` 等 |
| `System.Net.Security` | `SslStream` 等 |
| `System.Net`（型レベル禁止） | `HttpWebRequest`、`WebClient`、`Dns`、`FtpWebRequest` 等 |

### ILString 免除

これらのプレフィックスで始まる文字列定数は #US ヒープスキャンでフラグされません：
- `"System.Net.Http"`
- `"System.Net.WebSockets"`
- `"System.Net.Sockets"`
- `"System.Net.Mail"`
- `"System.Net.NetworkInformation"`
- `"System.Net.Security"`

### 宣言しても禁止される機能

`Capability.Network` を宣言しても、以下の機能は**常に**ブロックされます（宣言不可能な機能）：

| カテゴリ | ブロックされる型 | 宣言不可能な理由 |
|----------|---------------|----------------|
| P/Invoke | `DllImportAttribute`、`Marshal`、`NativeMemory` | 実行時に安全に監査できない |
| Unsafe コード | `UnverifiableCodeAttribute`、`System.Runtime.CompilerServices.Unsafe` | 型安全性の保証をバイパスする |
| IL 発行 | `System.Reflection.Emit.*` | 実行時に任意のコードを生成できる |
| アセンブリ読み込み | `System.Runtime.Loader`、`Assembly.Load*` | 未チェックの DLL を読み込んでセキュリティスキャンをバイパスできる |
| レジストリ | `Microsoft.Win32.*` | プラグインサンドボックス外の OS レベルシステムアクセス |

## Reason フィールドの監査役割

`Reason` フィールドは機能宣言の**監査証跡**として機能します：

```
Security audit: [CapabilityNetworkPlugin] com.siliconlife.demo.capabilitynetwork declared Capability.Network — reason: Calls weather REST API to retrieve forecast data
```

**Reason が重要な理由：**
1. **セキュリティレビュー**：監査者が宣言された機能が実際のプラグイン動作と一致するかを検証できる
2. **最小権限の原則**：プラグイン作者に各機能が必要な理由の説明を強制する
3. **コンプライアンス**：セキュリティ認証やインシデント調査に必要
4. **ランタイム監視**：セキュリティツールが宣言された機能の使用が記載された理由を超えた場合にアラートを出せる

## 08-ForbiddenNetwork との比較

| 側面 | 08-ForbiddenNetwork | 13-CapabilityNetwork |
|------|-------------------|---------------------|
| 宣言 | なし | `[PluginCapability(Capability.Network)]` |
| 読み込み結果 | ❌ PluginLoader により拒否 | ✅ 正常に読み込み |
| HttpClient 使用 | TypeRef スキャンでブロック | 機能宣言により免除 |
| TcpClient 使用 | TypeRef スキャンでブロック | 機能宣言により免除 |
| Reason | 該当なし | 監査ログに書き込み |

**重要な違い**：08-ForbiddenNetwork は機能を**宣言せずに**ネットワーク型を使用した場合の結果を示します。13-CapabilityNetwork は宣言型でネットワークアクセスを要求する**正しい**方法を示します。

## セキュリティベストプラクティス

1. **必要なものだけ宣言する**：HTTP だけが必要な場合、だからといって Capability.Network を宣言しないでください——ただし Capability.Network は唯一のネットワーク関連機能であり、より細かい粒度のオプションはないことに注意してください
2. **NetworkExecutor を優先**：`NetworkExecutor` はネットワークアクセスの制御されたエントリポイントであり、機能宣言が不要です
3. **明確な Reason を提供**：「ネットワークアクセス」のような曖昧な理由はセキュリティレビューで懸念されます
4. **宣言不可能な制限を忘れない**：P/Invoke、Unsafe、Reflection.Emit の禁止を回避できる機能宣言はありません

## ファイル

- `Plugin.cs` — Capability.Network を宣言するデモプラグイン
- `README.md` — English
- `README.zh-CN.md` — 简体中文
- `README.zh-HK.md` — 繁體中文
- `README.ja-JP.md` — 本ファイル（日本語）
- `README.ko-KR.md` — 한국어
- `README.de-DE.md` — Deutsch
- `README.fr-FR.md` — Français
- `README.es-ES.md` — Español
- `README.it-IT.md` — Italiano
- `README.ru-RU.md` — Русский
- `README.pt-PT.md` — Português
- `README.pl-PL.md` — Polski
- `README.cs-CZ.md` — Čeština

## 関連例

- **08-ForbiddenNetwork**：ブロックされるネットワーク操作の反例
- **14-CapabilityFileIO**：宣言型 FileIO 機能
- **15-CapabilityProcess**：宣言型 Process 機能
- **16-CapabilityAI**：宣言型 AI サービス機能
- **17-CapabilityStacked**：複数機能のスタック
- **18-CapabilityDenied**：宣言不可能な機能の反例
