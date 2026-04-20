# 権限システム

## 概要

権限システムは、AI が開始したすべての操作が適切に検証および監査されることを保証します。

## 5レベル権限チェーン

```
┌─────────────────────────────────────────────┐
│          権限検証                            │
├─────────────────────────────────────────────┤
│  レベル1: IsCurator                         │
│  ↓ true の場合はバイパス                     │
│  レベル2: UserFrequencyCache                │
│  ↓ レート制限                                │
│  レベル3: GlobalACL                         │
│  ↓ アクセス制御リスト                        │
│  レベル4: IPermissionCallback               │
│  ↓ カスタムロジック                          │
│  レベル5: IPermissionAskHandler             │
│  ↓ ユーザーに確認                            │
│  結果: 許可 または 拒否                      │
└─────────────────────────────────────────────┘
```

## レベル 1: IsCurator

マネージャー/キュレーターはすべての権限チェックをバイパスします。

```csharp
if (user.IsCurator)
{
    return PermissionResult.Allowed("キュレーターアクセス");
}
```

## レベル 2: UserFrequencyCache

ユーザーごとのレート制限で悪用を防止します。

```csharp
var cache = new UserFrequencyCache();
if (!cache.CheckLimit(userId, resource))
{
    return PermissionResult.Denied("レート制限超過");
}
```

## レベル 3: GlobalACL

グローバルアクセス制御リストが明示的なルールを定義します。

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

## レベル 4: IPermissionCallback

動的権限ロジックのためのカスタムコールバック。

```csharp
public class DefaultPermissionCallback : IPermissionCallback
{
    public async Task<PermissionResult> CheckAsync(PermissionRequest request)
    {
        // ここにカスタムロジック
        if (IsSafeOperation(request))
        {
            return PermissionResult.Allowed("安全な操作");
        }
        
        return PermissionResult.Undecided("ユーザー確認が必要");
    }
}
```

## レベル 5: IPermissionAskHandler

不明なリクエストはユーザーに直接確認します。

```csharp
public class ConsolePermissionAskHandler : IPermissionAskHandler
{
    public async Task<PermissionResult> AskAsync(PermissionRequest request)
    {
        Console.WriteLine($"操作を許可しますか？{request.Resource}");
        var answer = Console.ReadLine();
        
        return answer.ToLower() == "yes" 
            ? PermissionResult.Allowed("ユーザーが許可")
            : PermissionResult.Denied("ユーザーが拒否");
    }
}
```

---

## 権限検証フロー

### 典型的なリクエスト

```
1. AI が「file.txt を削除」ツールを呼び出す
   ↓
2. 実行リクエストを作成
   ↓
3. 権限チェーンを通過：
   - IsCurator? いいえ
   - レート制限？通過
   - GlobalACL？ルールなし
   - コールバック？未決定
   - ユーザーに確認？「はい」
   ↓
4. 許可 → ファイルを削除
```

### 拒否シナリオ

```
1. AI が「システムをシャットダウン」しようとする
   ↓
2. IsCurator? いいえ
   ↓
3. GlobalACL？拒否ルールあり
   ↓
4. 拒否：「管理者権限が必要」
```

---

## 権限リソースタイプ

### ディスク操作

- `disk:read` - ファイルを読み取る
- `disk:write` - ファイルに書き込む
- `disk:delete` - ファイルを削除
- `disk:execute` - プログラムを実行

### ネットワーク操作

- `network:http` - HTTP リクエスト
- `network:tcp` - TCP 接続
- `network:udp` - UDP データグラム

### コンパイル＆実行

- `compile:execute` - コードをコンパイルして実行
- `compile:safe` - 安全なコードのみ

### システム操作

- `system:info` - システム情報を取得
- `system:config` - 設定を変更
- `system:restart` - サービスを再起動

---

## ベストプラクティス

### 1. 最小権限の原則

必要最小限の権限のみを付与：

```json
{
  "userId": "user-123",
  "resource": "disk:read",  // 書き込みではない！
  "allowed": true
}
```

### 2. 時間制限付き権限

永続的な権限を避ける：

```json
{
  "userId": "user-123",
  "resource": "disk:write",
  "allowed": true,
  "expiresAt": "2026-04-20T12:00:00Z"  // 1時間後
}
```

### 3. 操作を監査

すべての権限決定をログに記録：

```csharp
Logger.Info($"権限 {request.Resource} ユーザー {userId}: {(allowed ? "許可" : "拒否")}");
```

---

## トラブルシューティング

### 「権限拒否」エラー

**確認**：
1. ユーザーが IsCurator であるか
2. GlobalACL に許可ルールがあるか
3. 権限が期限切れでないか
4. レート制限に達していないか

### レート制限が早すぎる

**調整**：
```csharp
var cache = new UserFrequencyCache(
    maxRequests: 100,      // 上限を増加
    windowMinutes: 60      // 時間枠を延長
);
```

### カスタムコールバックが機能しない

**確認**：
1. コールバックが正しく登録されている
2. チェーンから Undecided を返している
3. 例外をスローしていない

---

## 次のステップ

- 🔒 [セキュリティドキュメント](security.md)を確認
- 📚 [アーキテクチャガイド](architecture.md)を読む
- 🛠️ [開発ガイド](development-guide.md)を確認
- 🚀 [はじめにガイド](getting-started.md)で開始
