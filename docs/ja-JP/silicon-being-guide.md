# シリコンビーイングガイド

## シリコンビーイングとは？

シリコンビーイングは、AI ドリブンの自律エージェントであり、以下の能力を持っています：
- 自然言語での対話
- ツールを使用してアクションを実行
- 記憶を保持
- タスクのスケジュール
- 学習と進化

---

## 生命体の作成

### 方法 1：Web UI

1. ダッシュボードに移動
2. **新しい生命体を作成**をクリック
3. 名前とソウルファイルを入力
4. 生命体を開始

### 方法 2：プログラム的

```csharp
var being = new DefaultSiliconBeing(new BeingConfig
{
    Id = Guid.NewGuid(),
    Name = "Assistant",
    SoulPath = "./data/beings/assistant/soul.md"
});

await being.StartAsync();
```

---

## ソウルファイル

ソウルファイル（`soul.md`）は生命体の個性を定義します。

### 基本構造

```markdown
# 生命体名

## 個性
あなたの個性の説明。

## 機能
あなたが得意なこと：
- 機能1
- 機能2

## 行動
あなたの行動方法：
- 常に礼儀正しく
- 明確で簡潔
```

### 詳細な例

```markdown
# コードレビューアシスタント

## 個性
あなたは経験豊富なソフトウェアエンジニアリングメンターです。
コード品質、セキュリティ、パフォーマンスを重視します。

## 機能
- コードレビューとフィードバック
- アーキテクチャの提案
- ベストプラクティスのガイダンス
- バグの特定と修正提案

## 行動
- 常に建設的なフィードバックを提供
- 明確な例を使用
- コードスニペットで説明
- 複雑な概念を段階的に説明
- セキュリティ脆弱性を優先
```

---

## 生命体のライフサイクル

### 1. 初期化

```
1. 設定をロード
2. ソウルファイルを読み取る
3. メモリを初期化
4. ツールを登録
5. AI クライアントに接続
```

### 2. 実行

```
1. トリガーを待機（メッセージ、タイマー、タスク）
2. コンテキストを構築（履歴 + 現在の入力）
3. AI を呼び出し
4. ツールコールを実行（存在する場合）
5. レスポンスを保存
6. トリガーに戻る
```

### 3. 停止

```
1. 保留中の操作を完了
2. 状態を保存
3. リソースを解放
4. 安全にシャットダウン
```

---

## 身体-ブレインアーキテクチャ

### 身体（DefaultSiliconBeing）

身体は以下を担当：
- 生存状態の維持
- トリガーシナリオの検出
- 外部イベントの処理
- 内部状態の管理

```csharp
public class DefaultSiliconBeing : SiliconBeingBase
{
    public override async Task Tick()
    {
        // トリガーをチェック
        if (HasNewMessage())
        {
            await TriggerBrain();
        }
        
        // タイマーを確認
        await CheckTimers();
        
        // タスクを更新
        await UpdateTasks();
    }
}
```

### ブレイン（ContextManager）

ブレインは以下を担当：
- 履歴のロード
- AI リクエストの構築
- AI レスポンスの処理
- ツール実行
- 結果の永続化

```csharp
public class ContextManager
{
    public async Task<string> ProcessMessage(string message)
    {
        // 履歴をロード
        var history = await LoadHistory();
        
        // リクエストを構築
        var request = BuildRequest(history, message);
        
        // AI を呼び出し
        var response = await aiClient.ChatAsync(request);
        
        // ツールを処理
        if (response.HasToolCalls)
        {
            var result = await ExecuteTools(response.ToolCalls);
            return await ProcessMessage(result);
        }
        
        // レスポンスを保存
        await SaveResponse(response);
        
        return response.Content;
    }
}
```

---

## メモリシステム

### 短期記憶

現在のセッションの会話：
- チャット履歴
- 最近のコンテキスト
- アクティブなタスク

### 長期記憶

永続的な知識：
- ユーザーの好み
- 過去の相互作用
- 学習したパターン

### 記憶の使用

```csharp
// 保存
await memory.SaveAsync("ユーザーは青が好き", tags: ["preference"]);

// 検索
var memories = await memory.SearchAsync("preference");

// 更新
await memory.UpdateAsync(memoryId, "ユーザーは赤が好き");
```

---

## 生命体の設定

### 設定オプション

```json
{
  "id": "being-uuid",
  "name": "Assistant",
  "soulPath": "./data/beings/assistant/soul.md",
  "aiClient": "Ollama",
  "model": "qwen2.5:7b",
  "temperature": 0.7,
  "maxTokens": 2000,
  "memoryEnabled": true,
  "taskEnabled": true
}
```

### AI クライアント

サポートされているクライアント：
- **Ollama** - ローカル AI
- **DashScope** - クラウド AI
- **カスタム** - 独自の実装

---

## 生命体間の相互作用

### マルチ生命体チャット

複数の生命体が一緒にチャットできます：

```
ユーザー → 生命体A → 生命体B → 応答
```

### 生命体協調

生命体がツールとして他の生命体を呼び出す：

```csharp
// 生命体A が生命体B を使用
var beingB = ServiceLocator.Get<ISiliconBeing>("being-b-id");
var result = await beingB.ProcessQuery("専門知識が必要");
```

---

## ベストプラクティス

### 1. 明確な個性を定義

ソウルファイルで明確で簡潔な個性を提供：

```markdown
## 個性
あなたは専門的なカスタマーサポートエージェントです。
常に礼儀正しく、効率的で、問題解決志向です。
```

### 2. スコープを制限

生命体に焦点を当てた機能を持たせる：

```markdown
## 機能
- 注文の追跡
- 返品処理
- 製品情報の提供

（すべてをやらせない！）
```

### 3. メモリを活用

重要な情報をメモリに保存：

```csharp
await memory.SaveAsync("ユーザーはプレミアムプランに加入");
```

### 4. エラーを処理

失敗に優雅に対処：

```csharp
try
{
    var response = await aiClient.ChatAsync(request);
}
catch (Exception ex)
{
    Logger.Error($"AI の呼び出しに失敗：{ex.Message}");
    return "申し訳ありません、一時的に利用できません。";
}
```

---

## トラブルシューティング

### 生命体が開始しない

**確認**：
1. ソウルファイルが存在する
2. AI クライアントが設定されている
3. AI サービスが実行中
4. 設定が有効

### 生命体が応答しない

**確認**：
1. AI がタイムアウトしていない
2. ツールが正しく実行されている
3. メモリが過負荷でない
4. 権限が適切

### 生命体が奇妙に振る舞う

**確認**：
1. ソウルファイルが明確
2. 温度設定が適切（0.7 を推奨）
3. 履歴が正しくロードされている
4. ツールが正しく定義されている

---

## 次のステップ

- 📚 [アーキテクチャガイド](architecture.md)を読む
- 🛠️ [開発ガイド](development-guide.md)を確認
- 🚀 [はじめにガイド](getting-started.md)で開始
- 🔧 [ツールリファレンス](tools-reference.md)を参照
