# 権限システム

> **バージョン：v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | **日本語** | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | [Русский](../ru-RU/permission-system.md)

## 概要

権限システムは、すべての AI が開始する操作が適切に検証および監査されることを保証します。

## パーミッション検証チェーン

```
┌─────────────────────────────────────────────┐
│          パーミッション検証                   │
├─────────────────────────────────────────────┤
│  レベル 1：UserFrequencyCache                │
│  ↓ 高頻度ユーザー決定キャッシュ（HighDeny/HighAllow）│
│  レベル 2：IPermissionCallback               │
│  ↓ カスタムロジック（Allowed/Denied/AskUser） │
│  レベル 3：IsCurator?                        │
│  ↓ はい → IPermissionAskHandler（ユーザーに確認）│
│  ↓ いいえ → グローバルACL → デフォルト拒否     │
│  結果：許可または拒否                         │
└─────────────────────────────────────────────┘
```

> **注意**：`PermissionManager.CheckPermission()` の実際のクエリ優先度は以下の通り：
> 1. **UserFrequencyCache** — まず高頻度ユーザー決定キャッシュを確認
> 2. **IPermissionCallback** — カスタムコールバックルールを評価
> 3. **キュレーター分岐** — コールバックが AskUser を返す、またはコールバックなしの場合：
>    - **キュレーター** → `IPermissionAskHandler`（IM 経由でユーザーに確認）
>    - **非キュレーター** → `グローバルACL` → デフォルト拒否

## レベル 1：UserFrequencyCache

各ビーイングの高頻度ユーザー決定キャッシュ（HighDeny/HighAllow）、メモリ内のみに存在。

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny は HighAllow より優先**
- **メモリのみ**：キャッシュは永続化されず、再起動後に消失
- **設定可能な有効期限**：ユーザーはキャッシュエントリの有効期間を設定可能

## レベル 2：IPermissionCallback

動的パーミッションロジック用のカスタムコールバック。

### DefaultPermissionCallback デフォルト実装

`DefaultPermissionCallback` は包括的なデフォルトパーミッションルールを提供します。以下を含む：

#### ネットワークアクセスルール
- **ループバックアドレス**：localhost, 127.0.0.1, ::1 を許可
- **プライベート IP アドレス**：
  - 192.168.x.x（クラス C）- 許可
  - 10.x.x.x（クラス A）- 許可
  - 172.16-31.x.x（クラス B）- ユーザーに確認
- **ドメインホワイトリスト**：
  - 検索エンジン：Google, Bing, DuckDuckGo, Yandex, Sogou など
  - AI サービス：OpenAI, Anthropic, HuggingFace, Ollama など
  - 開発者サービス：GitHub, StackOverflow, npm, NuGet など
  - ソーシャルメディア：微博、知乎、Reddit、Discord など
  - 動画プラットフォーム：YouTube, Bilibili, 抖音、TikTok など
  - **天気情報**：wttr.in
  - 政府サイト：.gov, .go.jp, .go.kr
- **ドメインブラックリスト**：
  - AI 偽装サイト：chatgpt, openai, deepseek などの偽装ドメイン
  - 悪意のある AI ツール：wormgpt, darkgpt, fraudgpt など
  - AI コンテンツファームおよびブラックマーケット関連ドメイン

```csharp
public class DefaultPermissionCallback : IPermissionCallback
{
    public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
    {
        if (IsSafeOperation(permissionType, resource))
        {
            return PermissionResult.Allowed;
        }
        
        return PermissionResult.AskUser;
    }
}
```

## レベル 3：分岐判定（IsCurator / グローバルACL）

コールバックが `AskUser` を返す、またはコールバックが設定されていない場合、システムはキュレーター身份に基づいて分岐します：

### キュレーター分岐（IsCurator = true）

シリコンキュレーターの場合、システムは IM を介してユーザーに決定を求めます：

```csharp
if (IsCurator)
{
    if (_askHandler != null)
    {
        AskPermissionResult userDecision = _askHandler.AskUser(callerId, permissionType, resource);
        // ユーザーが Web UI で確認または拒否
    }
}
```

### 非キュレーター分岐（IsCurator = false）

非キュレーターのビーイングの場合、システムはグローバルACLを確認します。一致するルールがない場合、リクエストはデフォルトで拒否されます。

### グローバルACL 構造

```json
{
  "rules": [
    {
      "permissionType": "NetworkAccess",
      "resourcePrefix": "api.github.com",
      "result": "Allowed"
    },
    {
      "permissionType": "FileAccess",
      "resourcePrefix": "C:\\Windows",
      "result": "Denied"
    }
  ]
}
```

ルールは順番に評価され、最初に一致したルールが適用されます。シリコンキュレーターのみがグローバルACLを変更できます。

### リソース形式

```
{type}:{path}

例：
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## IPermissionAskHandler

キュレーターの操作にユーザー確認が必要な場合、`IPermissionAskHandler` を通じてユーザーにパーミッションを確認します。

### IMPermissionAskHandler 実装

`IMPermissionAskHandler` は Web UI を介してユーザーにパーミッションリクエストを送信します：

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        // IM 経由でユーザーにメッセージを送信
        SendMessageAsync($"Allow {resource}?");

        // ユーザー応答を待機
        var response = WaitForResponseAsync();

        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

### PermissionRequestQueue パーミッションリクエストキュー

`PermissionRequestQueue` は保留中のパーミッションリクエストを管理し、非同期でのユーザー応答待機をサポートします：

- **リクエストのエンキュー** — パーミッションチェーンがレベル 5 に到達した際、`TaskCompletionSource<AskPermissionResult>` を作成してエンキュー
- **Web UI 表示** — `PermissionRequestController` を介して Web UI に保留中のパーミッションリクエストを表示
- **ユーザー応答** — ユーザーが Web UI で承認または拒否、決定のキャッシュとキャッシュ期間の設定が可能
- **キャッシュオプション** — ユーザーはパーミッション決定を 1 時間、24 時間、7 日間、または 30 日間キャッシュ可能
- **タイムアウトメカニズム** — 60 秒間応答がない場合、リクエストページを自動的に閉じる

## 監査システム

すべてのパーミッション決定が記録されます：

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "callerId": "being-uuid",
  "permissionType": "FileAccess",
  "resource": "C:\\data\\config.json",
  "result": "Allowed",
  "reason": "Global ACL"
}
```

## プログラム的パーミッション評価

### EvaluatePermission API

`PermissionManager.EvaluatePermission()` メソッドは、ユーザープロンプトをトリガーしない読み取り専用のパーミッション事前評価を提供します。`PermissionTool` はこのメソッドを使用して、AI が操作を試みる前にパーミッション状態を確認します。

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**戻り値**：三状態 `PermissionResult`：
- `Allowed` - 操作が許可される
- `Denied` - 操作が拒否される
- `AskUser` - 実行時にユーザー確認が必要

**評価順序**：
1. **頻度キャッシュ** - キャッシュされたユーザー決定を確認
2. **IPermissionCallback** - カスタムコールバック評価
3. **キュレーター状態** - キュレーターの場合、`AskUser` を返す（確認が必要）
4. **グローバルACL** - アクセス制御ルールを確認
5. **デフォルト** - 一致するルールがない場合は拒否

> **注意**：完全なパーミッションチェーンとは異なり、`EvaluatePermission` は `IPermissionAskHandler` を呼び出し**ません**。実行時の結果が*どうなるか*のみを報告します。

## パーミッションの管理

### パーミッションの付与

**Web UI 経由**：
1. **パーミッション管理**に移動
2. **ルールを追加**をクリック
3. 設定：
   - ユーザー
   - リソース
   - 許可/拒否
   - 期間

**API 経由**：
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "permissionType": "FileAccess",
    "resourcePrefix": "C:\\Projects",
    "result": "Allowed",
    "description": "Allow project directory access"
  }'
```

### パーミッションの取り消し

Web UI のパーミッション管理ページから操作します。

### パーミッションの表示

```bash
curl http://localhost:8080/api/permissions/list
```

## ツールパーミッションシステム

操作レベルのパーミッション検証チェーンに加えて、システムはシリコンビーイングが使用できるツールを制御するための**ツールパーミッション**管理メカニズムを提供します。

### 2レベルのツールパーミッション

ツールパーミッションは2つのレベルに分かれます：

1. **シリコンビーイングレベル** — 個々のシリコンビーイングが使用できるツール操作を制御
2. **プロジェクトレベル** — プロジェクトスペース内で使用可能なツール操作を制御、シリコンビーイングレベルのパーミッションとは独立

### ツールパーミッション設定

各ツールの各操作は、個別に許可または拒否として設定できます：

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "network:get": "allowed",
    "network:post": "denied",
    "disk:read": "allowed",
    "disk:write": "denied",
    "database:query": "allowed"
  }
}
```

### パーミッションテンプレート

システムは定義済みのツールパーミッションテンプレートを提供し、シリコンビーイングに迅速に適用できます：

- **readonly** — 読み取り専用パーミッション（読み取り操作を許可、書き込み操作を拒否）
- **full** — フルパーミッション（すべての操作を許可）
- **restricted** — 制限付きパーミッション（基本操作のみ許可）

### Web UI 管理

Web UI でツールパーミッションを管理：

- **シリコンビーイングツールパーミッションページ** — `/beings/tool-permissions`
- **プロジェクトツールパーミッションページ** — `/project/{id}/tool-permissions`

### API エンドポイント

| エンドポイント | メソッド | 説明 |
|------|------|------|
| `/api/beings/tool-permissions` | GET | シリコンビーイングツールパーミッションの取得 |
| `/api/beings/tool-permissions` | PUT | シリコンビーイングツールパーミッションの更新 |
| `/api/beings/tool-permissions/templates` | GET | パーミッションテンプレート一覧の取得 |
| `/api/beings/tool-permissions/apply-template` | POST | パーミッションテンプレートの適用 |
| `/api/projects/{id}/tool-permissions` | GET | プロジェクトツールパーミッションの取得 |
| `/api/projects/{id}/tool-permissions` | PUT | プロジェクトツールパーミッションの更新 |

### スキルのアクションパーミッション

スキルはツールアクションパーミッション機構を再利用します：スキル id をツール名とし、アクションは `execute` とします。

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "daily_news_digest:execute": "denied",
    "code_review:execute": "allowed"
  }
}
```

- 無効化されたスキルは AI から見えるツール定義に表示されません（AI はそれを「見る」ことができません）
- スキル実行時にランタイム再チェックがあり、古い Schema でもバイパスできません
- スキル内部のツールパーミッション = ビーイングパーミッション ∪ スキル自身の制限（厳格側の和集合、狭くすることはできても権限を広げることはできません）

### MCP ラップツールのアクションパーミッション

各 MCP サーバーが注入するラップツール（`mcp_{serverId}_{toolName}`）は自動的に単一の `execute` アクションを宣言します：

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "mcp_filesystem_read_file:execute": "denied",
    "mcp_github_create_issue:execute": "allowed"
  }
}
```

- ビーイングまたはプロジェクト単位で外部ツールの可用性を精密に制御可能
- あるサーバーの全 `execute` アクションが無効化されると、そのツールは AI から見える Schema から全体が削除されます
- サーバーの無効化/削除（Web UI 操作）は即座にその全ツールを登録解除します

---

## ベストプラクティス

### 1. 最小権限の原則

必要な最小限のパーミッションのみを付与：

```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects\\MyApp\\config.json",
  "result": "Allowed"
}
```

### 2. 時間制限付きパーミッションを使用

絶対に必要でない限り、永久パーミッションを付与しないでください。

### 3. パーミッションログをモニタリング

定期的に監査ログを確認して以下を把握：
- 拒否されたアクセス試行
- 異常なパターン
- パーミッションのエスカレーション

### 4. カスタムコールバックの実装

複雑なロジックには `IPermissionCallback` を使用：

```csharp
public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
{
    // 時間ベースのパーミッション
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }
    
    // リソースベースのパーミッション
    if (IsSensitiveResource(resource))
    {
        return PermissionResult.AskUser;
    }
    
    return PermissionResult.Allowed;
}
```

## 一般的なシナリオ

### シナリオ 1：AI がファイルを読み取りたい

```
AI：「config.json を読み取る必要があります」
↓
パーミッションチェーン：
1. UserFrequencyCache？キャッシュされた決定なし
2. IPermissionCallback？AskUser を返す（明示的に許可されていない）
3. IsCurator？いいえ → グローバルACLを確認
4. グローバルACL？ルール発見：file:... = Allowed
5. 結果：許可
```

### シナリオ 2：AI がコードを実行したい

```
AI：「コードをコンパイルして実行したい」
↓
パーミッションチェーン：
1. UserFrequencyCache？キャッシュされた決定なし
2. IPermissionCallback？AskUser を返す
3. IsCurator？はい → IPermissionAskHandler
4. ユーザーが承認
5. 結果：許可
```

### シナリオ 3：キャッシュされた拒否

```
AI：「C:\Windows にアクセスする必要があります」
↓
パーミッションチェーン：
1. UserFrequencyCache？HighDeny キャッシュに発見
2. 結果：拒否（以降のチェック不要）
```

## トラブルシューティング

### 予期しないパーミッション拒否

**確認**：
1. ユーザーの IsCurator 状態
2. 頻度キャッシュの HighDeny エントリ
3. グローバルACLルール
4. コールバックロジック
5. ユーザー応答タイムアウト

### パーミッションが期限切れにならない

**確認**：
- `expiresAt` フィールドが正しく設定されている
- タイムゾーンが正確
- クロックが同期されている

### 監査ログが記録されない

**確認**：
- 監査ロガーが登録済み
- ストレージバックエンドがアクセス可能
- ディスク容量が十分

## 次のステップ

- 📚 [アーキテクチャガイド](architecture.md)を読む
- 🛠️ [開発ガイド](development-guide.md)を確認
- 🔒 [セキュリティドキュメント](security.md)を確認
- 🚀 [クイックスタートガイド](getting-started.md)を確認
