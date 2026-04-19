# シリコンライフコレクティブ

**⚠️ 警告: 動的コンパイルは機能しますが、適切に動作するにはコードテンプレートが必要です。詳細なテストが進行中です。**

Roslyn動的コンパイルを通じて自己進化するAIエージェント**シリコンビーイング**による.NET 9マルチエージェントコラボレーションプラットフォーム。

[中文文档](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [English](../README.md)

## 特徴

- **マルチエージェントオーケストレーション** — チック駆動の時間分割公平スケジューリング(MainLoop + TickObject + ウォッチドッグ + サーキットブレーカー)により*シリコンキュレーター*が管理
- **ソウルファイル駆動** — 各シリコンビーイングは、その個性と行動を定義するコアプロンプトファイル(`soul.md`)によって駆動
- **ボディ-ブレインアーキテクチャ** — *ボディ*(SiliconBeing)は生存状態を維持しトリガーを検出;*ブレイン*(ContextManager)は履歴を読み込み、AIを呼び出し、ツールを実行し、応答を永続化
- **ツールコールループ** — AIがtool_callsを返す → ツールを実行 → 結果をフィードバック → AIが継続 → プレーンテキスト応答まで
- **エグゼキューター-パーミッションセキュリティ** — すべてのディスク、ネットワーク、コマンドライン操作は、パーミッション検証付きエグゼキューターを通過
  - 5レベルのパーミッションチェーン: IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - すべてのパーミッション決定の監査ログ
- **トークン使用監査** — `ITokenUsageAudit` / `TokenUsageAuditManager`による組み込みトークン使用追跡とレポート
- **複数AIバックエンド** — Ollama(ローカル)とAlibaba Cloud DashScope(クラウド)をサポート
  - **Ollama** — ローカルモデルホスティング、ネイティブHTTP API
  - **DashScope (百煉)** — クラウドAIサービス、OpenAI互換API、マルチリージョンデプロイメント、13+モデル(Qwen、DeepSeek、GLM、Kimi、Llama)
- **32のカレンダーシステム** — グレゴリオ暦、中国太陰暦、イスラム暦、ヘブライ暦、日本暦、ペルシャ暦、マヤ暦などを含むマルチカレンダーサポート
- **最小限の依存関係** — コアライブラリはRoslyn動的コンパイル用のMicrosoft.CodeAnalysis.CSharpのみに依存
- **データベース依存ゼロ** — `ITimeStorage`による時間インデックス付きクエリを使用したファイルベースストレージ(JSON)
- **ローカリゼーション** — 中国語(簡体字と繁体字)と英語の組み込みサポート
- **Web UI** — SSEサポート、複数のスキン、包括的なダッシュボード付き組み込みHTTPサーバー
  - **スキンシステム** — 4つの組み込みスキン(Admin、Chat、Creative、Dev)、プラグイン可能なISkinインターフェースと自動検出
  - **17のコントローラー** — About、Audit、Being、Chat、CodeBrowser、Config、Dashboard、Executor、Init、Knowledge、Log、Memory、Permission、PermissionRequest、Project、Task、Timer
  - **リアルタイム更新** — チャットメッセージ、ビーイングステータス、システムイベント用のSSE(サーバー送信イベント)
  - **HTML/CSS/JSビルダー** — `H`、`CssBuilder`、`JsBuilder`によるサーバーサイドマークアップ生成(ゼロフロントエンドフレームワーク依存)
  - **ローカリゼーション** — LocalizationManagerによる3つの組み込みロケール(zh-CN、zh-HK、en-US)

## 技術スタック

| コンポーネント | テクノロジー |
|-----------|-----------|
| ランタイム | .NET 9 |
| 言語 | C# |
| AI統合 | Ollama(ローカル)、Alibaba Cloud DashScope(クラウド) |
| ストレージ | ファイルシステム(JSON + 時間インデックス付きディレクトリ) |
| Webサーバー | HttpListener(組み込み.NET) |
| 動的コンパイル | Roslyn(Microsoft.CodeAnalysis.CSharp 4.13.0) |
| ライセンス | Apache-2.0 |

## プロジェクト構造

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # コアライブラリ(インターフェース、抽象化)
│   │   ├── ServiceLocator.cs             # グローバルサービスロケーター: Register/Get、ChatSystem、IMManager、AuditLogger、GlobalACL、BeingFactory、BeingManager、DynamicBeingLoader、TokenUsageAudit
│   │   ├── Runtime/                       # MainLoop、TickObject、CoreHost、CoreHostBuilder、PerformanceMonitor
│   │   ├── SiliconBeing/                  # SiliconBeingBase、SiliconBeingManager、SiliconCurator、ISiliconBeingFactory、SoulFileManager、Memory、TaskSystem、TimerSystem
│   │   ├── AI/                            # IAIClient、IAIClientFactory、ContextManager("ブレイン")、Message、AIRequest/AIResponse
│   │   ├── Audit/                         # ITokenUsageAudit、TokenUsageAuditManager、TokenUsageRecord、TokenUsageSummary、TokenUsageQuery
│   │   ├── Chat/                          # ChatSystem、IChatService、SimpleChatService、SessionBase、SingleChatSession、GroupChatSession、BroadcastChannel、ChatMessage
│   │   ├── Executors/                     # ExecutorBase、DiskExecutor、NetworkExecutor、CommandLineExecutor、ExecutorRequest、ExecutorResult
│   │   ├── Tools/                         # ITool、ToolManager(リフレクションスキャニング)、ToolCall/ToolResult、ToolDefinition、SiliconManagerOnlyAttribute
│   │   ├── Security/                      # PermissionManager、GlobalACL、AuditLogger、UserFrequencyCache、PermissionResult、PermissionType、IPermissionCallback、IPermissionAskHandler
│   │   ├── IM/                            # IIMProvider、IMManager(メッセージルーティング)
│   │   ├── Storage/                       # IStorage、ITimeStorage(キー値 + 時間インデックス付き)
│   │   ├── Config/                        # ConfigDataBase、Config(シングルトン + JSON)、ConfigDataBaseConverter、GuidConverter、AIClientConfigAttribute、ConfigGroupAttribute、ConfigIgnoreAttribute、DirectoryInfoConverter
│   │   ├── Localization/                  # LocalizationBase、LocalizationManager、Language列挙型
│   │   ├── Logging/                       # ILogger、ILoggerProvider、LogEntry、LogLevel、LogManager
│   │   ├── Compilation/                   # DynamicBeingLoader、DynamicCompilationExecutor、SecurityScanner、CodeEncryption
│   │   └── Time/                          # IncompleteDate(時間範囲クエリ)
│   │
│   └── SiliconLife.Default/               # デフォルト実装 + エントリーポイント
│       ├── Program.cs                     # アプリケーションエントリー(すべてのコンポーネントの配線)
│       ├── AI/                            # OllamaClient、OllamaClientFactory(ネイティブOllama HTTP API)；DashScopeClient、DashScopeClientFactory(Alibaba Cloud百煉)
│       ├── SiliconBeing/                  # DefaultSiliconBeing、DefaultSiliconBeingFactory
│       ├── Calendar/                      # 32のカレンダー実装: Buddhist、Cherokee、ChineseLunar、ChulaSakarat、Coptic、Dai、DehongDai、Ethiopian、FrenchRepublican、Gregorian、Hebrew、Indian、Inuit、Islamic、Japanese、Javanese、Juche、Julian、Khmer、Mayan、Mongolian、Persian、RepublicOfChina、Roman、Saka、Sexagenary、Tibetan、Vietnamese、VikramSamvat、Yi、Zoroastrian
│       ├── Executors/                     # デフォルトエグゼキューター実装
│       ├── IM/                            # WebUIProvider(IMチャネルとしてのWeb UI)、IMPermissionAskHandler
│       ├── Tools/                         # 組み込みツール: Calendar、Chat、Config、Curator、Disk、DynamicCompile、Memory、Network、System、Task、Timer、TokenAudit
│       ├── Config/                        # DefaultConfigData
│       ├── Localization/                  # ZhCN、ZhHK、EnUS、DefaultLocalizationBase
│       ├── Logging/                       # ConsoleLoggerProvider、FileSystemLoggerProvider
│       ├── Storage/                       # FileSystemStorage、FileSystemTimeStorage
│       ├── Security/                      # DefaultPermissionCallback
│       ├── Runtime/                       # TestTickObject
│       └── Web/                           # Web UI実装
│           ├── Controllers/               # 17のコントローラー: About、Audit、Being、Chat、CodeBrowser、Config、Dashboard、Executor、Init、Knowledge、Log、Memory、Permission、PermissionRequest、Project、Task、Timer
│           ├── Models/                    # ViewModel: AboutViewModel、AuditViewModel、BeingViewModel、ChatMessage、ChatViewModel、CodeBrowserViewModel、ConfigViewModel、DashboardViewModel、ExecutorViewModel、KnowledgeViewModel、LogViewModel、MemoryViewModel、PermissionViewModel、PermissionRequestViewModel、ProjectViewModel、TaskViewModel、TimerViewModel、ViewModelBase
│           ├── Views/                     # HTMLビュー: ViewBase、AboutView、AuditView、BeingView、ChatView、CodeBrowserView、CodeEditorView、ConfigView、DashboardView、ExecutorView、KnowledgeView、LogView、MarkdownEditorView、MemoryView、PermissionView、ProjectView、TaskView、TimerView
│           ├── Skins/                     # 4つのスキン: Admin(プロフェッショナル)、Chat(会話型)、Creative(芸術的)、Dev(開発者向け)
│           ├── ISkin.cs                   # スキンインターフェース + SkinPreviewInfo + SkinManager(自動検出)
│           ├── Controller.cs              # ベースコントローラークラス
│           ├── WebHost.cs                 # HTTPサーバー(HttpListener)
│           ├── Router.cs                  # パターンマッチング付きリクエストルーティング
│           ├── SSEHandler.cs              # サーバー送信イベント
│           ├── WebSecurity.cs             # Webセキュリティユーティリティ
│           ├── H.cs                       # フルーエントHTMLビルダーDSL
│           ├── CssBuilder.cs              # CSSビルダーユーティリティ
│           └── JsBuilder.cs               # JavaScriptビルダーユーティリティ
│
├── docs/
│   └── ja-JP/                             # 日本語ドキュメント
```

## アーキテクチャ概要

```
MainLoop(専用スレッド、ウォッチドッグ + サーキットブレーカー)
  └── TickObject(優先度ソート済み)
       └── SiliconBeingManager
            └── SiliconBeingRunner(チックごとの一時スレッド、タイムアウト + サーキットブレーカー)
                 └── DefaultSiliconBeing.Tick()
                      └── ContextManager.ThinkOnChat()
                           └── IAIClient.Chat() -> ツールコールループ -> ChatSystemに永続化
```

AIによって開始されたすべてのI/O操作はセキュリティチェーンを通過します:

```
ツールコール -> エグゼキューター -> PermissionManager -> [IsCurator -> FrequencyCache -> GlobalACL -> Callback -> AskUser]
```

## はじめに

### 前提条件

- .NET 9 SDK
- ローカルで実行中の[Ollama](https://ollama.com)とプルされたモデル(例: `ollama pull llama3`)

### ビルド

```bash
dotnet restore
dotnet build
```

### 実行

```bash
dotnet run --project src/SiliconLife.Default
```

アプリケーションはWebサーバーを起動し、Web UIをブラウザで自動的に開きます。

### パブリッシュ(単一ファイル)

```bash
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

## ロードマップ

- [x] フェーズ1: コンソールAIチャット
- [x] フェーズ2: フレームワークスケルトン(MainLoop + TickObject + ウォッチドッグ + サーキットブレーカー)
- [x] フェーズ3: ソウルファイル付きの最初のシリコンビーイング(ボディ-ブレインアーキテクチャ)
- [x] フェーズ4: 永続メモリ(ChatSystem + ITimeStorage)
- [x] フェーズ5: ツールシステム + エグゼキューター
- [x] フェーズ6: パーミッションシステム(5レベルチェーン、AuditLogger、GlobalACL)
- [x] フェーズ7: 動的コンパイル + 自己進化(Roslyn)
- [x] フェーズ8: 長期メモリ + タスク + タイマー
- [x] フェーズ9: CoreHost + マルチエージェントコラボレーション
- [x] フェーズ10: Web UI(HTTP + SSE、17のコントローラー、4つのスキン)
- [x] フェーズ10.5: 増分機能強化(BroadcastChannel、TokenAudit、32のカレンダー、ツール機能強化)
- [ ] フェーズ11: 外部IM統合(Feishu / WhatsApp / Telegram)
- [ ] フェーズ12: ナレッジグラフ、プラグインシステム、スキルエコシステム

## ドキュメント

- [アーキテクチャ](architecture.md) — システム設計、スケジューリング、コンポーネントアーキテクチャ
- [セキュリティ](security.md) — パーミッションモデル、エグゼキューター、動的コンパイルセキュリティ
- [ロードマップ](roadmap.md) — 詳細な12フェーズ開発計画

## ライセンス

このプロジェクトはApache License 2.0の下でライセンスされています — 詳細は[LICENSE](../LICENSE)ファイルを参照してください。

## 作者

Hoshino Kennji — [YouTube](https://www.youtube.com/@hoshinokennji) | [Bilibili](https://space.bilibili.com/617827040)