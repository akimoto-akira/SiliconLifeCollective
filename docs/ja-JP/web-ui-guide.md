# Web UI ガイド

## 概要

Web UI は SiliconLifeCollective の主要なユーザーインターフェースを提供します。ASP.NET Core とカスタム MVC フレームワークを使用して構築されています。

---

## アーキテクチャ

### コンポーネント

```
Web/
├── Controllers/       # ルートコントローラー
├── Models/            # ビューモデル
├── Views/             # HTML ビュー
├── Skins/             # スキンテーマ
└── Services/          # Web サービス
```

### リクエストフロー

```
ブラウザ → HTTP リクエスト → コントローラー → ビューモデル → HTML レンダリング → レスポンス
```

---

## ページ

### 1. ダッシュボード（`/`）

システム概要：
- 実行中の生命体
- システムパフォーマンス
- AI 使用状況
- クイックアクション

### 2. 生命体管理（`/beings`）

生命体を作成、編集、開始、停止：
- 生命体一覧
- ソウルファイルエディター
- 設定パネル
- 状態コントロール

### 3. チャット（`/chat`）

生命体との対話：
- リアルタイムメッセージ
- ストリーミングレスポンス
- 履歴表示
- セッション管理

### 4. 設定（`/settings`）

システム設定：
- AI クライアント設定
- ストレージパス
- 権限ルール
- グローバル設定

### 5. 権限管理（`/permissions`）

権限の表示と管理：
- ACL ルール
- ユーザー権限
- 使用頻度モニター
- 監査ログ

### 6. タスク管理（`/tasks`）

タスクとリマインダー：
- タスク一覧
- タスク作成
- 優先度設定
- 期限管理

### 7. モニタリング（`/monitoring`）

システムモニタリング：
- CPU/メモリ使用率
- AI トークン使用
- 応答時間
- エラーログ

---

## スキンシステム

### 組み込みスキン

1. **デフォルト** - クリーンでプロフェッショナル
2. **ダーク** - ダークテーマ
3. **ライト** - ライトテーマ
4. **カスタム** - ユーザー定義

### カスタムスキンの作成

```csharp
public class MyCustomSkin : ISkin
{
    public string Name => "MySkin";
    public string Description => "カスタムスキン";
    
    public string GetCss()
    {
        return @"
            :root {
                --primary-color: #your-color;
                --bg-color: #your-bg;
                --text-color: #your-text;
            }
            
            body {
                font-family: 'Your Font', sans-serif;
            }
        ";
    }
}
```

### スキンの適用

Web UI で：
1. 設定に移動
2. スキンを選択
3. 変更を適用

---

## SSE（Server-Sent Events）

### チャットストリーミング

```javascript
const eventSource = new EventSource(`/api/chat/stream?beingId=${id}&message=${msg}`);

eventSource.onmessage = (event) => {
    const data = JSON.parse(event.data);
    
    if (data.type === 'chunk') {
        appendToChat(data.content);
    } else if (data.type === 'complete') {
        chatComplete(data.sessionId);
    }
};
```

### リアルタイム更新

```javascript
const events = new EventSource('/api/events');

events.onmessage = (event) => {
    const data = JSON.parse(event.data);
    updateDashboard(data);
};
```

---

## ビューモデル

### BeingViewModel

```csharp
public class BeingViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public string SoulPath { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### ChatViewModel

```csharp
public class ChatViewModel
{
    public string BeingId { get; set; }
    public List<MessageViewModel> Messages { get; set; }
    public string SessionId { get; set; }
}
```

---

## ルーティング

### コントローラー例

```csharp
public class BeingController : Controller
{
    [HttpGet("/beings")]
    public IActionResult Index()
    {
        var beings = GetBeings();
        return View(beingViewModel);
    }
    
    [HttpPost("/beings/create")]
    public IActionResult Create(CreateBeingRequest request)
    {
        var being = CreateBeing(request);
        return Redirect($"/beings/{being.Id}");
    }
}
```

---

## カスタマイズ

### 新しいページを追加

1. コントローラーを作成：

```csharp
public class CustomController : Controller
{
    [HttpGet("/custom")]
    public IActionResult Index()
    {
        return View(model);
    }
}
```

2. ビューを作成（`Views/Custom/Index.html`）：

```html
@model CustomViewModel

<h1>@Model.Title</h1>
<p>@Model.Content</p>
```

### カスタム CSS を追加

スキンで追加の CSS を定義：

```csharp
public string GetCss()
{
    return base.GetCss() + @"
        .custom-class {
            color: red;
        }
    ";
}
```

---

## レスポンシブデザイン

Web UI はモバイルデバイスを含むすべての画面サイズで動作します：

```css
@media (max-width: 768px) {
    .sidebar {
        display: none;
    }
    
    .content {
        width: 100%;
    }
}
```

---

## ベストプラクティス

### 1. ビューモデルを使用

コントローラーからビューに直接データを渡さない：

```csharp
// 良い
return View(new BeingViewModel { Name = being.Name });

// 悪い
return View(being);
```

### 2. SSE をストリーミングに使用

長い AI レスポンスにリアルタイム更新を提供：

```csharp
[HttpGet("/chat/stream")]
public async Task StreamChat(string beingId, string message)
{
    Response.ContentType = "text/event-stream";
    
    await foreach (var chunk in aiClient.StreamChatAsync(request))
    {
        await Response.WriteAsync($"data: {chunk}\n\n");
        await Response.Body.FlushAsync();
    }
}
```

### 3. エラーを適切に処理

ユーザーフレンドリーなエラーメッセージを表示：

```csharp
catch (Exception ex)
{
    Logger.Error(ex);
    return View("Error", new ErrorViewModel 
    { 
        Message = "エラーが発生しました。もう一度お試しください。"
    });
}
```

---

## トラブルシューティング

### ページがロードしない

**確認**：
1. Web サーバーが実行中（`http://localhost:8080`）
2. ルーティングが正しく設定されている
3. ビューファイルが存在する
4. コントローラーが public である

### SSE が接続しない

**確認**：
1. ブラウザが SSE をサポート
2. CORS が正しく設定されている
3. コントローラーが `text/event-stream` を返している

### スキンが適用されない

**確認**：
1. スキンが正しく登録されている
2. CSS が有効
3. キャッシュがクリアされている

---

## 次のステップ

- 📚 [アーキテクチャガイド](architecture.md)を読む
- 🛠️ [開発ガイド](development-guide.md)を確認
- 🚀 [はじめにガイド](getting-started.md)で開始
- 🎨 カスタムスキンを作成して UI をパーソナライズ
