# Silicon Life Collective

**⚠️ 警告：動的コンパイルは機能しますが、正しく動作するにはコードテンプレートが必要です。徹底的なテスト進行中です。**

AI エージェントである**シリコンビーイング（Silicon Beings）**が Roslyn 動的コンパイルを通じて自己進化を行う、.NET 9 マルチエージェントコラボレーションプラットフォーム。

[English](../en/README.md) | [中文文档](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [日本語](README.md) | [한국어](../ko-KR/README.md)

## 機能

- **マルチエージェントオーケストレーション** — *シリコンキュレーター（Silicon Curator）*によって管理され、ティック駆動のタイムスライス公平スケジューリング（MainLoop + TickObject + Watchdog + Circuit Breaker）を採用
- **ソウルファイル駆動** — 各シリコンビーイングは核心プロンプトファイル（`soul.md`）によって駆動され、その個性と行動を定義
- **ボディ-ブレインアーキテクチャ** — *ボディ（Body）*（SiliconBeing）は生存状態を維持しトリガーを検出；*ブレイン（Brain）*（ContextManager）は履歴をロードし、AI を呼び出し、ツールを実行し、レスポンスを永続化
- **ツールコールループ** — AI がツールコールを返す → ツールを実行 → 結果をフィードバック → AI が続行 → プレーンテキストレスポンスを返すまで
- **エグゼキューター-パーミッションセキュリティ** — すべてのディスク、ネットワーク、コマンドライン操作はエグゼキューターを通過し、権限検証を行う
  - 5レベルの権限チェーン：IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - すべての権限決定の監査ログ
- **トークン使用監査** — `ITokenUsageAudit` / `TokenUsageAuditManager` による組み込みトークン使用追跡とレポート
- **複数のAIバックエンド** — Ollama（ローカル）と Alibaba Cloud DashScope（クラウド）をサポート
  - **Ollama** — ネイティブ HTTP API を使用したローカルモデルホスティング
  - **DashScope（百錬）** — OpenAI 互換 API、マルチリージョンデプロイメント、13+ モデル（Qwen、DeepSeek、GLM、Kimi、Llama）をサポートするクラウド AI サービス
- **32種類のカレンダーシステム** — グレゴリオ暦、中国旧暦、イスラム暦、ヘブライ暦、日本暦、ペルシャ暦、マヤ暦などを含むマルチカレンダーサポート
- **最小限の依存関係** — コアライブラリは Roslyn 動的コンパイル用の Microsoft.CodeAnalysis.CSharp のみに依存
- **ゼロデータベース依存** — `ITimeStorage` による時刻インデックス付きクエリをサポートするファイルベースのストレージ（JSON）
- **ローカライゼーション** — 18種類の言語バリアントを含む包括的な多言語サポート
  - 中国語：zh-CN、zh-HK、zh-SG、zh-MO、zh-TW、zhMY（6種類のバリアント）
  - 英語：en-US、en-GB、en-CA、en-AU、en-IN、en-SG、en-ZA、en-IE、en-NZ、en-MY（10種類のバリアント）
  - 日本語：ja-JP
  - 韓国語：ko-KR
- **Web UI** — SSE サポート、複数のスキン、包括的なダッシュボードを備えた組み込み HTTP サーバー
  - **スキンシステム** — プラグ可能な ISkin インターフェースと自動検出機能を備えた4種類の組み込みスキン（Admin、Chat、Creative、Dev）
  - **18個のコントローラー** — About、Audit、Being、Chat、CodeBrowser、CodeHover、Config、Dashboard、Executor、Init、Knowledge、Log、Memory、Permission、PermissionRequest、Project、Task、Timer
  - **リアルタイム更新** — チャットメッセージ、生命体ステータス、システムイベント用の SSE（Server-Sent Events）
  - **HTML/CSS/JS ビルダー** — `H`、`CssBuilder`、`JsBuilder` によるサーバーサイドマークアップ生成（ゼロフロントエンドフレームワーク依存）
  - **ローカライゼーション** — LocalizationManager による解決を含む18種類の組み込み言語バリアント

## 技術スタック

| コンポーネント | 技術 |
|-----------|-----------|
| ランタイム | .NET 9 |
| 言語 | C# |
| AI 統合 | Ollama（ローカル）、Alibaba Cloud DashScope（クラウド） |
| ストレージ | ファイルシステム（JSON + 時刻インデックス付きディレクトリ） |
| Web サーバー | HttpListener（.NET 組み込み） |
| 動的コンパイル | Roslyn（Microsoft.CodeAnalysis.CSharp 4.13.0） |
| ライセンス | Apache-2.0 |

## プロジェクト構造

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # コアライブラリ（インターフェース、抽象クラス）
│   │   ├── ServiceLocator.cs             # グローバルサービスロケーター：Register/Get、ChatSystem、IMManager、AuditLogger、GlobalACL、BeingFactory、BeingManager、DynamicBeingLoader、TokenUsageAudit
│   │   ├── Runtime/                       # MainLoop、TickObject、CoreHost、CoreHostBuilder、PerformanceMonitor
│   │   ├── SiliconBeing/                  # SiliconBeingBase、SiliconBeingManager、SiliconCurator、ISiliconBeingFactory、SoulFileManager、Memory、TaskSystem、TimerSystem
│   │   ├── AI/                            # IAIClient、IAIClientFactory、ContextManager（"ブレイン"）、Message、AIRequest/AIResponse
│   │   ├── Audit/                         # ITokenUsageAudit、TokenUsageAuditManager、TokenUsageRecord、TokenUsageSummary、TokenUsageQuery
│   │   ├── Chat/                          # ChatSystem、IChatService、SimpleChatService、SessionBase、SingleChatSession、GroupChatSession、BroadcastChannel、ChatMessage
│   │   ├── Executors/                     # ExecutorBase、DiskExecutor、NetworkExecutor、CommandLineExecutor、ExecutorRequest、ExecutorResult
│   │   ├── Tools/                         # ITool、ToolManager（リフレクションスキャン）、ToolCall/ToolResult、ToolDefinition、SiliconManagerOnlyAttribute
│   │   ├── Security/                      # PermissionManager、GlobalACL、AuditLogger、UserFrequencyCache、PermissionResult、PermissionType、IPermissionCallback、IPermissionAskHandler
│   │   ├── IM/                            # IIMProvider、IMManager（メッセージルーティング）
│   │   ├── Storage/                       # IStorage、ITimeStorage（キーバリュー + 時刻インデックス）
│   │   ├── Config/                        # ConfigDataBase、Config（シングルトン + JSON）、ConfigDataBaseConverter、GuidConverter、AIClientConfigAttribute、ConfigGroupAttribute、ConfigIgnoreAttribute、DirectoryInfoConverter
│   │   ├── Localization/                  # LocalizationBase、LocalizationManager、Language 列挙型
│   │   ├── Logging/                       # ILogger、ILoggerProvider、LogEntry、LogLevel、LogManager
│   │   ├── Compilation/                   # DynamicBeingLoader、DynamicCompilationExecutor、SecurityScanner、CodeEncryption
│   │   └── Time/                          # IncompleteDate（時間範囲クエリ）
│   │
│   └── SiliconLife.Default/               # デフォルト実装 + エントリポイント
│       ├── Program.cs                     # アプリケーションエントリ（すべてのコンポーネントを配線）
│       ├── AI/                            # OllamaClient、OllamaClientFactory（ネイティブ Ollama HTTP API）；DashScopeClient、DashScopeClientFactory（Alibaba Cloud 百錬）
│       ├── SiliconBeing/                  # DefaultSiliconBeing、DefaultSiliconBeingFactory
│       ├── Calendar/                      # 32種類のカレンダー実装：Buddhist、Cherokee、ChineseLunar、ChulaSakarat、Coptic、Dai、DehongDai、Ethiopian、FrenchRepublican、Gregorian、Hebrew、Indian、Inuit、Islamic、Japanese、Javanese、Juche、Julian、Khmer、Mayan、Mongolian、Persian、RepublicOfChina、Roman、Saka、Sexagenary、Tibetan、Vietnamese、VikramSamvat、Yi、Zoroastrian
│       ├── Executors/                     # デフォルトエグゼキューター実装
│       ├── IM/                            # WebUIProvider（Web UI を IM チャネルとして使用）、IMPermissionAskHandler
│       ├── Tools/                         # 組み込みツール：Calendar、Chat、Config、Curator、Disk、DynamicCompile、Memory、Network、System、Task、Timer、TokenAudit
│       ├── Config/                        # DefaultConfigData
│       ├── Localization/                  # ZhCN、ZhHK、EnUS、JaJP、KoKR、DefaultLocalizationBase、EnOther（EnGB、EnCA、EnAU、EnIN、EnSG、EnZA、EnIE、EnNZ、EnMY）、ZhOther（ZhSG、ZhMO、ZhTW、ZhMY）
│       ├── Logging/                       # ConsoleLoggerProvider、FileSystemLoggerProvider
│       ├── Storage/                       # FileSystemStorage、FileSystemTimeStorage
│       ├── Security/                      # DefaultPermissionCallback
│       ├── Runtime/                       # TestTickObject
│       └── Web/                           # Web UI 実装
│           ├── Controllers/               # 18個のコントローラー：About、Audit、Being、Chat、CodeBrowser、CodeHover、Config、Dashboard、Executor、Init、Knowledge、Log、Memory、Permission、PermissionRequest、Project、Task、Timer
│           ├── Models/                    # ViewModel：AboutViewModel、AuditViewModel、BeingViewModel、ChatMessage、ChatViewModel、CodeBrowserViewModel、ConfigViewModel、DashboardViewModel、ExecutorViewModel、KnowledgeViewModel、LogViewModel、MemoryViewModel、PermissionViewModel、PermissionRequestViewModel、ProjectViewModel、TaskViewModel、TimerViewModel、ViewModelBase
│           ├── Views/                     # 19個の HTML ビュー：ViewBase、AboutView、AuditView、BeingView、ChatView、CodeBrowserView、CodeEditorView、ConfigView、DashboardView、ExecutorView、KnowledgeView、LogView、MarkdownEditorView、MemoryView、PermissionView、ProjectView、SoulEditorView、TaskView、TimerView
│           ├── Skins/                     # 4種類のスキン：Admin（プロフェッショナル）、Chat（会話）、Creative（アーティスティック）、Dev（開発者向け）
│           ├── ISkin.cs                   # スキンインターフェース + SkinPreviewInfo + SkinManager（自動検出）
│           ├── Controller.cs              # ベースコントローラークラス
│           ├── WebHost.cs                 # HTTP サーバー（HttpListener）
│           ├── Router.cs                  # パターンマッチング付きリクエストルーティング
│           ├── SSEHandler.cs              # Server-Sent Events
│           ├── WebSecurity.cs             # Web セキュリティユーティリティ
│           ├── H.cs                       # フルーエント HTML ビルダー DSL
│           ├── CssBuilder.cs              # CSS ビルダーユーティリティ
│           └── JsBuilder.cs               # JavaScript ビルダーユーティリティ
│
├── docs/
│   └── zh-CN/                             # 中国語ドキュメント
```

## アーキテクチャ概要

```
MainLoop（専用スレッド、ウォッチドッグ + サーキットブレーカー）
  └── TickObject（優先度ソート済み）
       └── SiliconBeingManager
            └── SiliconBeingRunner（ティックごとに一時的なスレッド、タイムアウト + サーキットブレーカー）
                 └── DefaultSiliconBeing.Tick()
                      └── ContextManager.ThinkOnChat()
                           └── IAIClient.Chat() -> ツールコールループ -> ChatSystem に永続化
```

AI が開始したすべての I/O 操作はセキュリティチェーンを通過します：

```
ツールコール -> エグゼキューター -> パーミッションマネージャー -> [IsCurator -> 頻度キャッシュ -> GlobalACL -> コールバック -> ユーザーに確認]
```

## はじめに

### 前提条件

- .NET 9 SDK
- AI バックエンド（いずれかを選択）：
  - **Ollama**：[Ollama](https://ollama.com) がローカルで実行され、モデルがプル済み（例：`ollama pull llama3`）
  - **Alibaba Cloud DashScope**：[百錬コンソール](https://bailian.console.aliyun.com/)から有効な API キー

### ビルド

```bash
dotnet restore
dotnet build
```

### 実行

```bash
dotnet run --project src/SiliconLife.Default
```

アプリケーションは Web サーバーを起動し、ブラウザで Web UI を自動的に開きます。

### 公開（単一ファイル）

```bash
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

## ロードマップ

- [x] フェーズ 1：コンソール AI チャット
- [x] フェーズ 2：フレームワークスケルトン（MainLoop + TickObject + Watchdog + Circuit Breaker）
- [x] フェーズ 3：ソウルファイル付きの最初のシリコンビーイング（ボディ-ブレインアーキテクチャ）
- [x] フェーズ 4：永続メモリ（ChatSystem + ITimeStorage）
- [x] フェーズ 5：ツールシステム + エグゼキューター
- [x] フェーズ 6：パーミッションシステム（5レベルチェーン、AuditLogger、GlobalACL）
- [x] フェーズ 7：動的コンパイル + 自己進化（Roslyn）
- [x] フェーズ 8：長期メモリ + タスク + タイマー
- [x] フェーズ 9：CoreHost + マルチエージェントコラボレーション
- [x] フェーズ 10：Web UI（HTTP + SSE、18個のコントローラー、4種類のスキン）
- [x] フェーズ 10.5：増分強化（BroadcastChannel、TokenAudit、32種類のカレンダー、ツール強化、18言語のローカライゼーション）
- [ ] フェーズ 11：外部 IM 統合（Feishu / WhatsApp / Telegram）
- [ ] フェーズ 12：ナレッジグラフ、プラグインシステム、スキルエコシステム

## ドキュメント

- [アーキテクチャ](architecture.md) — システム設計、スケジューリング、コンポーネントアーキテクチャ
- [セキュリティ](security.md) — パーミッションモデル、エグゼキューター、動的コンパイルセキュリティ
- [ロードマップ](roadmap.md) — 詳細な12フェーズ開発計画

## ライセンス

このプロジェクトは Apache License 2.0 の下でライセンスされています — 詳細は [LICENSE](LICENSE) ファイルを参照してください。

## 作者

Hoshino Kennji — [YouTube](https://www.youtube.com/@hoshinokennji) | [Bilibili](https://space.bilibili.com/617827040)
