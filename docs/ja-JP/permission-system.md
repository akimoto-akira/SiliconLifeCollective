# 権限システム

> **バージョン: v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | **日本語** | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md)

## 概要

権限システムは、AI が開始するすべての操作が適切に検証および監査されることを保証します。

## 3段階分岐権限チェーン

```
┌─────────────────────────────────────────────┐
│          権限検証                            │
├─────────────────────────────────────────────┤
│  レベル 1：UserFrequencyCache                │
│  ↓ レートリミットキャッシュ                   │
│  レベル 2：IPermissionCallback               │
│  ↓ カスタムロジック                           │
│  レベル 3：分岐判断                           │
│  ├─ IsCurator → IPermissionAskHandler        │
│  │  ↓ 主理人：ユーザー確認を尋ねる             │
│  └─ Non-curator → GlobalACL                  │
│     ↓ 非主理人：アクセスコントロールリスト       │
│  結果：許可または拒否                         │
└─────────────────────────────────────────────┘
```

## レベル 1：UserFrequencyCache

ユーザーごとのレートリミットで不正使用を防止。

```csharp
var cache = new UserFrequencyCache();
if (!cache.CheckLimit(userId, resource))
{
    return PermissionResult.Denied("Rate limit exceeded");
}
```

## レベル 2：IPermissionCallback

動的権限ロジック用のカスタムコールバック。

### DefaultPermissionCallback デフォルト実装

`DefaultPermissionCallback` は包括的なデフォルト権限ルールを提供します：

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

## レベル 3：分岐判断（IsCurator / GlobalACL）

レベル 1 とレベル 2 のいずれも決定を下さなかった場合、システムは呼び出し元の身分に基づいて分岐します：

### 主理人分岐（IsCurator = true）

呼び出し元が主理人の場合、`IPermissionAskHandler` を介してユーザー確認を求めます：

```csharp
if (IsCurator)
{
    if (_askHandler != null)
    {
        var result = await _askHandler.AskAsync(request);
        // ユーザーが Web UI で承認または拒否
    }
}
```

### 非主理人分岐（IsCurator = false）

呼び出し元が主理人でない場合、`GlobalACL` アクセスコントロールリストを確認します：

### GlobalACL 構造

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

## IPermissionAskHandler

主理人の操作にユーザー確認が必要な場合、`IPermissionAskHandler` を通じてユーザーに権限を確認します。

### IMPermissionAskHandler 実装

`IMPermissionAskHandler` は Web UI を介してユーザーに権限リクエストを送信します：

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public async Task<AskPermissionResult> AskAsync(PermissionRequest request)
    {
        // インスタントメッセージでユーザーにメッセージを送信
        await SendMessageAsync($"Allow {request.Resource}?");
        
        // ユーザーの応答を待機
        var response = await WaitForResponseAsync();
        
        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

### PermissionRequestQueue 権限リクエストキュー

`PermissionRequestQueue` は保留中の権限リクエストを管理し、ユーザー応答の非同期待機をサポートします：

- **リクエストのエンキュー** — 権限チェーンがレベル 3 の主理人分岐に達した場合、`TaskCompletionSource<AskPermissionResult>` を作成してエンキュー
- **Web UI 表示** — `PermissionRequestController` を介して Web UI に保留中の権限リクエストを表示
- **ユーザー応答** — ユーザーが Web UI で承認または拒否、決定のキャッシュとキャッシュ期間の設定が可能
- **キャッシュオプション** — ユーザーは権限決定を 1 時間、24 時間、7 日間、または 30 日間キャッシュ可能
- **タイムアウトメカニズム** — 30 分間応答がない場合、リクエストページが自動的に閉じる

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
3. **主理人状態** - 主理人の場合、`AskUser` を返す（確認が必要）
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
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "user-uuid",
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

### 権限の取消

Web UI の権限管理ページから操作します。

### 権限の表示

```bash
curl http://localhost:8080/api/permissions/list
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
1. レートリミット？正常
2. コールバック？未決定を返す
3. IsCurator？いいえ → GlobalACL？ルール発見：disk:read = 許可
4. 結果：許可
```

### シナリオ 2：AI がコードを実行したい

```
AI：「コードをコンパイルして実行したい」
↓
権限チェーン：
1. レートリミット？正常
2. コールバック？未決定を返す
3. IsCurator？はい → IPermissionAskHandler → ユーザー承認
4. 結果：許可
```

### シナリオ 3：レートリミット超過

```
AI：「100回の HTTP リクエストが必要です」
↓
権限チェーン：
1. レートリミット？超過
2. 結果：拒否
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
