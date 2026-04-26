# 権限システム

[English](../en/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | **日本語** | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md)

## 概要

権限システムは、AI が開始するすべての操作が適切に検証および監査されることを保証します。

## 5段階権限チェーン

```
┌─────────────────────────────────────────────┐
│          権限検証                            │
├─────────────────────────────────────────────┤
│  レベル 1：IsCurator                         │
│  ↓ 真の場合はバイパス                         │
│  レベル 2：UserFrequencyCache                │
│  ↓ レートリミット                             │
│  レベル 3：GlobalACL                         │
│  ↓ アクセスコントロールリスト                 │
│  レベル 4：IPermissionCallback               │
│  ↓ カスタムロジック                           │
│  レベル 5：IPermissionAskHandler             │
│  ↓ ユーザーに確認                             │
│  結果：許可または拒否                         │
└─────────────────────────────────────────────┘
```

## レベル 1：IsCurator

管理者/管理人はすべての権限チェックをバイパス。

```csharp
if (user.IsCurator)
{
    return PermissionResult.Allowed("Curator access");
}
```

## レベル 2：UserFrequencyCache

ユーザーごとのレートリミットで不正使用を防止。

```csharp
var cache = new UserFrequencyCache();
if (!cache.CheckLimit(userId, resource))
{
    return PermissionResult.Denied("Rate limit exceeded");
}
```

## レベル 3：GlobalACL

グローバルアクセスコントロールリストが明確なルールを定義。

### ACL 構造

```json
{
  "rules": [
    {
      "userId": "user-uuid",
      "resource": "disk:read",
      "allowed": true,
      "expiresAt": "2026-04-21T00:00:00Z"
    }
  ]
}
```

### リソース形式

```
{type}:{action}

例：
- disk:read
- disk:write
- network:http
- compile:execute
- system:info
```

## レベル 4：IPermissionCallback

動的権限ロジック用のカスタムコールバック。

### DefaultPermissionCallback デフォルト実装

`DefaultPermissionCallback` は包括的なデフォルト権限ルールを提供：

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
    public async Task<PermissionResult> CheckAsync(PermissionRequest request)
    {
        // カスタムロジック
        if (IsSafeOperation(request))
        {
            return PermissionResult.Allowed("Safe operation");
        }
        
        return PermissionResult.Undecided("Needs user confirmation");
    }
}
```

## レベル 5：IPermissionAskHandler

他のすべてのレベルが未決定の場合、ユーザーに権限を確認。

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public async Task<AskPermissionResult> AskAsync(PermissionRequest request)
    {
        // IM を介してユーザーにメッセージを送信
        await SendMessageAsync($"Allow {request.Resource}?");
        
        // ユーザーの応答を待機
        var response = await WaitForResponseAsync();
        
        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

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
3. **管理人状態** - 管理人の場合、`AskUser` を返す（確認が必要）
4. **グローバル ACL** - アクセスコントロールルールを確認
5. **デフォルト** - ルールが一致しない場合、拒否

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
public async Task<PermissionResult> CheckAsync(PermissionRequest request)
{
    // 時間ベースの権限
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied("Outside business hours");
    }
    
    // リソースベースの権限
    if (IsSensitiveResource(request.Resource))
    {
        return PermissionResult.Undecided("Requires approval");
    }
    
    return PermissionResult.Allowed();
}
```

## 一般的なシナリオ

### シナリオ 1：AI がファイルを読み取りたい

```
AI：「config.json を読み取る必要があります」
↓
権限チェーン：
1. IsCurator？いいえ
2. レートリミット？正常
3. GlobalACL？ルール発見：disk:read = 許可
4. 結果：許可
```

### シナリオ 2：AI がコードを実行したい

```
AI：「コードをコンパイルして実行したい」
↓
権限チェーン：
1. IsCurator？いいえ
2. レートリミット？正常
3. GlobalACL？ルールなし
4. コールバック？未決定を返す
5. ユーザーに確認？ユーザー承認
6. 結果：許可
```

### シナリオ 3：レートリミット超過

```
AI：「100回の HTTP リクエストが必要です」
↓
権限チェーン：
1. IsCurator？いいえ
2. レートリミット？超過
3. 結果：拒否
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
