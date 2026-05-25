# 権限システム

> **バージョン: v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | **日本語** | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | [Русский](../ru-RU/permission-system.md)

## 概要

権限システムは、AI が開始するすべての操作が適切に検証および監査されることを保証します。

## 3段階分岐権限チェーン

```
┌─────────────────────────────────────────────┐
│          権限検証                            │
├─────────────────────────────────────────────┤
│  レベル 1：UserFrequencyCache                │
│  ↓ キャッシュされたユーザー決定 (HighDeny/HighAllow)│
│  レベル 2：IPermissionCallback               │
│  ↓ カスタムロジック (Allowed/Denied/AskUser)  │
│  レベル 3：IsCurator?                        │
│  ↓ はい → IPermissionAskHandler（ユーザーに確認）│
│  ↓ いいえ → GlobalACL → デフォルト拒否        │
│  結果：許可または拒否                         │
└─────────────────────────────────────────────┘
```

> **注意**：`PermissionManager.CheckPermission()` の実際のクエリ優先度は以下の通り：
> 1. **UserFrequencyCache** — キャッシュされた高頻度ユーザー決定を最初に確認
> 2. **IPermissionCallback** — カスタムコールバックルールを評価
> 3. **主理人分岐** — コールバックが AskUser を返す、またはコールバックなしの場合：
>    - **主理人** → `IPermissionAskHandler`（IM 経由でユーザーにプロンプト）
>    - **非主理人** → `GlobalACL` → デフォルト拒否

## レベル 1：UserFrequencyCache

生命体ごとの、メモリのみの高頻度ユーザー決定キャッシュ（HighDeny/HighAllow）。

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **高拒否（HighDeny）**は**高許可（HighAllow）**より優先度が高い
- **メモリのみ**：キャッシュは永続化されない。再起動時に失われる
- **設定可能な有効期限**：ユーザーはキャッシュエントリの有効期間を設定可能

## レベル 2：IPermissionCallback

動的権限ロジック用のカスタムコールバック。

### DefaultPermissionCallback デフォルト実装

`DefaultPermissionCallback` は包括的なデフォルト権限ルールを提供します。以下を含む：

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
  - AI 偽装サイト：chatgpt, openai, deepseek などのフィッシングドメイン
  - 悪意のある AI ツール：wormgpt, darkgpt, fraudgpt など
  - AI コンテンツファームとブラックマーケット関連ドメイン

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

## レベル 3：主理人分岐（IsCurator → AskHandler / GlobalACL）

コールバックが `AskUser` を返す、またはコールバックが設定されていない場合、システムは主理人ステータスに基づいて分岐します：

### 主理人パス：IPermissionAskHandler

シリコン主理人の場合、システムは IM を介してユーザーに決定を求めます。

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        SendMessage($"Allow {resource}?");

        var response = WaitForResponse();

        return new AskPermissionResult
        {
            Allowed = response.Approved,
            AddToCache = response.AddToCache,
            CacheDuration = response.CacheDuration
        };
    }
}
```

### 非主理人パス：GlobalACL → デフォルト拒否

非主理人の生命体の場合、システムはグローバルアクセスコントロールリストを確認します。一致するルールが見つからない場合、リクエストはデフォルトで拒否されます。

### GlobalACL 構造

```json
{
  "rules": [
    {
      "prefix": "network:api.github.com",
      "result": "Allowed"
    },
    {
      "prefix": "file:C:\\Windows",
      "result": "Denied"
    }
  ]
}
```

ルールは順番に評価され、最初のマッチが勝利します。シリコン主理人のみがグローバル ACL を変更可能です。

### リソース形式

```
{type}:{path}

例：
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## IPermissionAskHandler

主理人の操作にユーザー確認が必要な場合、`IPermissionAskHandler` を通じてユーザーに権限を確認します。上記の `IMPermissionAskHandler` 実装を参照してください。

## 監査システム

すべての権限決定が記録されます：

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "level": "GlobalACL",
  "reason": "Explicit rule granted"
}
```

## プログラムによる権限評価

### EvaluatePermission API

`PermissionManager.EvaluatePermission()` メソッドは、ユーザープロンプトをトリガーしない読み取り専用の権限事前評価を提供します。`PermissionTool` はこのメソッドを使用して、AI が操作を試みる前に権限状態を確認します。

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
1. **周波数キャッシュ** - キャッシュされたユーザー決定を確認
2. **IPermissionCallback** - カスタムコールバック評価
3. **主理人分岐** - 主理人の場合、`AskUser` を返す（確認が必要）。非主理人の場合、**GlobalACL** を確認し、デフォルト拒否

> **注意**：完全な権限チェーンとは異なり、`EvaluatePermission` は `IPermissionAskHandler` を呼び出し**ません**。実行時の結果が*どうなるか*のみを報告します。

## 権限の管理

### 権限の付与

**Web UI 経由**：
1. **権限管理**に移動
2. **ルールを追加**をクリック
3. 設定：
   - ユーザー
   - リソース
   - 許可/拒否
   - 期間

**API 経由**：
```bash
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "user-uuid",
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

### 権限の取消

```bash
curl -X DELETE http://localhost:8080/api/permissions/{rule-id}
```

### 権限の表示

```bash
curl http://localhost:8080/api/permissions?userId=user-uuid
```

## ベストプラクティス

### 1. 最小権限の原則

必要な最小限の権限のみを付与：

```json
{
  "resource": "disk:read",  // disk:* ではない
  "allowed": true,
  "expiresAt": "2026-04-21T00:00:00Z"  // 常に有効期限を設定
}
```

### 2. 時間制限付き権限を使用

絶対に必要でない限り、永久権限を付与しないでください。

### 3. 権限ログをモニタリング

定期的に監査ログを確認：
- 拒否されたアクセストライ
- 異常なパターン
- 権限昇格

### 4. カスタムコールバックを実装

複雑なロジックには `IPermissionCallback` を使用：

```csharp
public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
{
    // 時間ベースの権限
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }
    
    // リソースベースの権限
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
権限チェーン：
1. 周波数キャッシュ？キャッシュされた決定なし
2. IPermissionCallback？AskUser を返す（明示的に許可されていない）
3. IsCurator？いいえ → GlobalACL を確認
4. GlobalACL？ルール発見：file:... = Allowed
5. 結果：許可
```

### シナリオ 2：AI がコードを実行したい

```
AI：「コードをコンパイルして実行したい」
↓
権限チェーン：
1. 周波数キャッシュ？キャッシュされた決定なし
2. IPermissionCallback？AskUser を返す
3. IsCurator？はい → IPermissionAskHandler
4. ユーザーが承認
5. 結果：許可
```

### シナリオ 3：キャッシュされた拒否

```
AI：「C:\Windows にアクセスする必要があります」
↓
権限チェーン：
1. 周波数キャッシュ？高拒否キャッシュに発見
2. 結果：拒否（以降のチェック不要）
```

## トラブルシューティング

### 予期しない権限拒否

**確認**：
1. ユーザーの IsCurator 状態
2. レートリミット設定
3. GlobalACL ルール
4. コールバックロジック
5. ユーザー応答タイムアウト

### 権限が期限切れにならない

**確認**：
- `expiresAt` フィールドが正しく設定
- タイムゾーンが正確
- クロックが同期

### 監査ログが記録されない

**確認**：
- 監査ロガーが登録済み
- ストレージバックエンドがアクセス可能
- ディスク容量が十分

## 次のステップ

- 📚 [アーキテクチャガイド](architecture.md)を読む
- 🛠️ [開発ガイド](development-guide.md)をチェック
- 🔒 [セキュリティドキュメント](security.md)を確認
- 🚀 [クイックスタートガイド](getting-started.md)を見る
