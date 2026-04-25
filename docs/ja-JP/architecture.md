# アーキテクチャ

[English](../en/architecture.md) | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [日本語](architecture.md) | [한국어](../ko-KR/architecture.md)

## 核心概念

### シリコンビーイング（Silicon Being）

システム内の各 AI エージェントは**シリコンビーイング**です。これは独自の ID、個性、能力を持つ自律的なエンティティです。各シリコンビーイングは、その行動パターンを定義する**ソウルファイル**（Markdown プロンプト）によって駆動されます。

### シリコンキュレーター（Silicon Curator）

**シリコンキュレーター**は最高のシステム権限を持つ特別なシリコンビーイングです。システム管理者として機能します：

- 他のシリコンビーイングを作成および管理
- ユーザーリクエストを分析し、タスクに分解
- 適切なシリコンビーイングにタスクを配布
- 実行品質を監視し、障害を処理
- **優先度スケジューリング**でユーザーメッセージに応答（下記参照）

### ソウルファイル（Soul File）

各シリコンビーイングのデータディレクトリに保存される Markdown ファイル（`soul.md`）です。これはすべての AI リクエストに注入されるシステムプロンプトとして機能し、ビーイングの個性、意思決定パターン、行動制約を定義します。

---

## スケジューリング：タイムスライス公平スケジューリング

### MainLoop + TickObject

システムは専用バックグラウンドスレッドで**ティック駆動のメインループ**を実行します：

```
MainLoop（専用スレッド、ウォッチドッグ + サーキットブレーカー）
  └── TickObject A（Priority=0, Interval=100ms）
  └── TickObject B（Priority=1, Interval=500ms）
  └── SiliconBeingManager（MainLoop によって直接ティック）
        └── SiliconBeingRunner → シリコンビーイング 1 → ティック → ExecuteOneRound
        └── SiliconBeingRunner → シリコンビーイング 2 → ティック → ExecuteOneRound
        └── SiliconBeingRunner → シリコンビーイング 3 → ティック → ExecuteOneRound
        └── ...
```

主要な設計判断：

- **シリコンビーイングは TickObject を継承しません。** 独自の `Tick()` メソッドを持ち、`SiliconBeingManager` によって `SiliconBeingRunner` を介して呼び出され、MainLoop に直接登録されません。
- **SiliconBeingManager** は MainLoop によって直接ティックされ、すべての生命体の単一プロキシとして機能します。
- **SiliconBeingRunner** は各生命体の `Tick()` をタイムアウトと生命体ごとのサーキットブレーカー（連続 3 回タイムアウト → 1 分間クールダウン）付きの一時的なスレッドでラップします。
- 各生命体の実行はティックごとに AI リクエスト + ToolCalls の**1 ラウンド**に制限され、どの生命体もメインループを独占できないようにします。
- **PerformanceMonitor** は観察可能性のためにティック実行時間を追跡します。

### キュレーター優先度レスポンス

ユーザーがシリコンキュレーターにメッセージを送信した場合：

1. 現在の生命体（例えば生命体 A）は現在のラウンドを完了します — **中断しません**。
2. マネージャーは**残りのキューをスキップ**します。
3. ループは**キュレーターから再開**され、即座に実行されます。

これにより、進行中のタスクを妨げることなく、応答性の高いユーザーインタラクションを確保します。

---

## コンポーネントアーキテクチャ

```
┌─────────────────────────────────────────────────────────┐
│                        CoreHost                         │
│  （統合ホスト — すべてのコンポーネントを組み立てて管理）      │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ MainLoop │  │ ServiceLocator│  │      Config      │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │           SiliconBeingManager（TickObject）        │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Curator  │ │Being A  │ │Being B  │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              共有サービス                          │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ChatSystem│  │ Storage  │  │  PermissionMgr  │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │ IAIClient│  │Executor  │  │   ToolManager   │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  エグゼキューター                   │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │  Disk    │  │ Network  │  │  CommandLine    │  │   │
│  │  │Executor  │  │Executor  │  │  Executor       │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              IM プロバイダー                       │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Console  │  │  Web     │  │  Feishu / ...   │  │   │
│  │  │Provider  │  │Provider  │  │  Provider       │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## ServiceLocator

`ServiceLocator` はすべてのコアサービスへのアクセスを提供するスレッドセーフなシングルトンレジストリです：

| プロパティ | 型 | 説明 |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | 中央チャットセッションマネージャー |
| `IMManager` | `IMManager` | インスタントメッセージングプロバイダールーター |
| `AuditLogger` | `AuditLogger` | 権限監査証跡 |
| `GlobalAcl` | `GlobalACL` | グローバルアクセスコントロールリスト |
| `BeingFactory` | `ISiliconBeingFactory` | 生命体を作成するファクトリ |
| `BeingManager` | `SiliconBeingManager` | アクティブな生命体ライフサイクルマネージャー |
| `DynamicBeingLoader` | `DynamicBeingLoader` | 動的コンパイルローダー |
| `TokenUsageAudit` | `ITokenUsageAudit` | トークン使用追跡 |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | トークン使用レポート |

また、生命体 GUID をキーとする生命体ごとの `PermissionManager` レジストリも維持します。

---

## チャットシステム

### セッションタイプ

チャットシステムは `SessionBase` を介して 3 種類のセッションタイプをサポートします：

| 型 | クラス | 説明 |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | 2 人の参加者間の 1 対 1 の会話 |
| `GroupChat` | `GroupChatSession` | 複数参加者のグループ会話 |
| `Broadcast` | `BroadcastChannel` | 固定 ID を持つオープンチャネル；生命体は動的にサブスクライブし、サブスクライブ時間以降のメッセージのみを受信 |

### BroadcastChannel

`BroadcastChannel` はシステム全体の告知のための特別なセッションタイプです：

- **固定チャネル ID** — `SingleChatSession` および `GroupChatSession` とは異なり、チャネル ID はメンバー GUID から派生しない既知の定数です。
- **動的サブスクリプション** — 生命体は実行時にサブスクライブ/サブスクライブ解除します；サブスクライブ時間以降に投稿されたメッセージのみを受信します。
- **保留メッセージフィルタリング** — `GetPendingMessages()` は、生命体のサブスクライブ時間以降に投稿され、まだ読んでいないメッセージのみを返します。
- **ChatSystem によって管理** — `GetOrCreateBroadcastChannel()`、`Broadcast()`、`GetPendingBroadcasts()`。

### ChatMessage

`ChatMessage` モデルには、AI 会話コンテキストとトークン追跡のフィールドが含まれています：

| フィールド | 型 | 説明 |
|-------|------|-------------|
| `Id` | `Guid` | ユニークなメッセージ ID |
| `SenderId` | `Guid` | 送信者のユニーク ID |
| `ChannelId` | `Guid` | チャネル/会話 ID |
| `Content` | `string` | メッセージ内容 |
| `Timestamp` | `DateTime` | メッセージ送信時刻 |
| `Type` | `MessageType` | テキスト、画像、ファイル、またはシステム通知 |
| `ReadBy` | `List<Guid>` | このメッセージを読んだ参加者の ID |
| `Role` | `MessageRole` | AI 会話ロール（User、Assistant、Tool） |
| `ToolCallId` | `string?` | ツール結果メッセージのツールコール ID |
| `ToolCallsJson` | `string?` | アシスタントメッセージのシリアル化されたツールコール JSON |
| `Thinking` | `string?` | AI からの連鎖推論 |
| `PromptTokens` | `int?` | プロンプト（入力）のトークン数 |
| `CompletionTokens` | `int?` | 完了（出力）のトークン数 |
| `TotalTokens` | `int?` | 使用された総トークン数（入力 + 出力） |
| `FileMetadata` | `FileMetadata?` | 添付ファイルメタデータ（メッセージにファイルが含まれる場合） |

### チャットメッセージキュー

`ChatMessageQueue`は、チャットメッセージの非同期処理を管理するスレッドセーフなメッセージキューシステムです：

- **スレッドセーフ** - ロック機構を使用して安全な同時アクセスを確保
- **非同期処理** - 非同期メッセージのエンキューとデキューをサポート
- **メッセージ順序** - メッセージの時系列順序を維持
- **バッチ操作** - バッチメッセージ取得をサポート

### ファイルメタデータ

`FileMetadata`は、チャットメッセージに添付されたファイル情報を管理するために使用されます：

- **ファイル情報** - ファイル名、サイズ、タイプ、パス
- **アップロード時刻** - ファイルアップロードのタイムスタンプ
- **アップローダー** - ファイルをアップロードしたユーザーまたはシリコンビーイングの ID

### ストリームキャンセルマネージャー

`StreamCancellationManager`は、AIストリーミング応答のキャンセルメカニズムを提供します：

- **ストリーム制御** - 進行中のAIストリーミング応答のキャンセルをサポート
- **リソースクリーンアップ** - キャンセル時に関連リソースを適切にクリーンアップ
- **同時実行安全性** - 複数のストリームを同時に管理をサポート

### チャット履歴ビュー

新しいチャット履歴ビュー機能により、ユーザーはシリコンビーイングの過去の会話を閲覧できます：

- **会話リスト** - すべての過去の会話表示
- **メッセージ詳細** - 完全なメッセージ履歴表示
- **タイムラインビュー** - 時系列順序でメッセージ表示
- **APIサポート** - 会話とメッセージデータを取得するRESTful APIを提供

---

## AI クライアントシステム

システムは `IAIClient` インターフェースを通じて複数の AI バックエンドをサポートします：

### OllamaClient

- **型**：ローカル AI サービス
- **プロトコル**：ネイティブ Ollama HTTP API（`/api/chat`、`/api/generate`）
- **機能**：ストリーミング、ツールコール、ローカルモデルホスティング
- **設定**：`endpoint`、`model`、`temperature`、`maxTokens`

### DashScopeClient（Alibaba Cloud 百錬）

- **型**：クラウド AI サービス
- **プロトコル**：OpenAI 互換 API（`/compatible-mode/v1/chat/completions`）
- **認証**：Bearer トークン（API キー）
- **機能**：ストリーミング、ツールコール、推論コンテンツ（Chain-of-Thought）、マルチリージョンデプロイメント
- **サポートされているリージョン**：
  - `beijing` — 華北 2（北京）
  - `virginia` — 米国（バージニア）
  - `singapore` — シンガポール
  - `hongkong` — 中国香港
  - `frankfurt` — ドイツ（フランクフルト）
- **サポートされているモデル**（API による動的検出、フォールバックリストあり）：
  - **Qwen シリーズ**：qwen3-max、qwen3.6-plus、qwen3.6-flash、qwen-max、qwen-plus、qwen-turbo、qwen3-coder-plus
  - **推論**：qwq-plus
  - **サードパーティ**：deepseek-v3.2、deepseek-r1、glm-5.1、kimi-k2.5、llama-4-maverick
- **設定**：`apiKey`、`region`、`model`
- **モデル検出**：実行時に DashScope API から利用可能なモデルを取得；ネットワーク障害時はキュレートされたリストにフォールバック

### クライアントファクトリパターン

各 AI クライアントタイプには、`IAIClientFactory` を実装する対応するファクトリがあります：

- `OllamaClientFactory` — OllamaClient インスタンスを作成
- `DashScopeClientFactory` — DashScopeClient インスタンスを作成

ファクトリは以下を提供します：
- `CreateClient(Dictionary<string, object> config)` — 設定からクライアントをインスタンス化
- `GetConfigKeyOptions(string key, ...)` — 設定キーの動的オプションを返す（例：利用可能なモデル、リージョン）
- `GetDisplayName()` — クライアントタイプのローカライズされた表示名

### AIプラットフォームサポートリスト

#### ステータス説明
- ✅ 実装済み
- 🚧 開発中
- 📋 計画中
- 💡 検討中

*注：開発者のネットワーク環境の影響により、[検討中]の海外クラウドAIサービスの統合にはネットワークプロキシツールの使用が必要になる場合があり、デバッグプロセスが不安定になる可能性があります。*

#### プラットフォームリスト

| プラットフォーム | ステータス | タイプ | 説明 |
|------|------|------|------|
| Ollama | ✅ | ローカル | ローカルAIサービス、ローカルモデルデプロイをサポート |
| DashScope（Alibaba Cloud Bailian） | ✅ | クラウド | Alibaba Cloud Bailian AIサービス、マルチリージョンデプロイをサポート |
| 百度千帆（文心一言） | 📋 | クラウド | Baidu ERNIE Bot AIサービス |
| 智譜AI（GLM） | 📋 | クラウド | Zhipu Qingyan AIサービス |
| 月之暗面（Kimi） | 📋 | クラウド | Moonshot Kimi AIサービス |
| 火山エンジン.豆包 | 📋 | クラウド | ByteDance Doubao AIサービス |
| DeepSeek（直接接続） | 📋 | クラウド | Deepseek AIサービス |
| 零一万物 | 📋 | クラウド | 01.AI AIサービス |
| 騰訊混元 | 📋 | クラウド | Tencent Hunyuan AIサービス |
| SiliconFlow | 📋 | クラウド | SiliconFlow AIサービス |
| MiniMax | 📋 | クラウド | MiniMax AIサービス |
| OpenAI | 💡 | クラウド | OpenAI APIサービス（GPTシリーズ） |
| Anthropic | 💡 | クラウド | Anthropic Claude AIサービス |
| Google DeepMind | 💡 | クラウド | Google Gemini AIサービス |
| Mistral AI | 💡 | クラウド | Mistral AIサービス |
| Groq | 💡 | クラウド | Groq高速AI推論サービス |
| Together AI | 💡 | クラウド | Together AIオープンソースモデルサービス |
| xAI | 💡 | クラウド | xAI Grokサービス |
| Cohere | 💡 | クラウド | CohereエンタープライズNLPサービス |
| Replicate | 💡 | クラウド | Replicateオープンソースモデルホスティングプラットフォーム |
| Hugging Face | 💡 | クラウド | Hugging FaceオープンソースAIコミュニティとモデルプラットフォーム |
| Cerebras | 💡 | クラウド | Cerebras AI推論最適化サービス |
| Databricks | 💡 | クラウド | DatabricksエンタープライズAIプラットフォーム（MosaicML） |
| Perplexity AI | 💡 | クラウド | Perplexity AI検索Q&Aサービス |
| NVIDIA NIM | 💡 | クラウド | NVIDIA AI推論マイクロサービス |

---

## 主要な設計判断

### インスタンスクラスとしてのストレージ（静的ではない）

`IStorage` は静的ユーティリティではなく、注入可能なインスタンスとして設計されています。これにより以下が保証されます：

- 直接ファイルシステムアクセス — IStorage はシステムの内部永続化チャネルであり、エグゼキューターを介して**ルーティングされません**。
- **AI は IStorage を制御できません** — エグゼキューターは AI ツールによって開始された IO を管理します；IStorage はフレームワーク自身の内部データ読み取り/書き込みを管理します。これらは根本的に異なる関心事です。
- モック実装でのテスト可能性。
- コンシューマーを変更せずに、将来の異なるストレージバックエンドのサポート。

### セキュリティ境界としてのエグゼキューター

エグゼキューターは I/O 操作の**唯一の**パスです。ディスク、ネットワーク、またはコマンドラインアクセスを必要とするツールは**必ず**エグゼキューターを通過する必要があります。この設計は以下を強制します：

- 各エグゼキューターは、権限検証のためのスレッドロック付きの**独立したスケジューリングスレッド**を所有します。
- 集中化された権限チェック — エグゼキューターは生命体の**プライベート PermissionManager** にクエリを実行します。
- 優先度サポートとタイムアウト制御付きのリクエストキューイング。
- すべての外部操作の監査ログ。
- 例外の分離 — 1 つのエグゼキューターの障害が他のエグゼキューターに影響を与えません。
- サーキットブレーカー — 連続した障害は一時的にエグゼキューターを停止し、カスケード障害を防止します。

### 軽量オブジェクトとしての ContextManager

各 `ExecuteOneRound()` は新しい `ContextManager` インスタンスを作成します：

1. ソウルファイル + 最近のチャット履歴をロード。
2. AI クライアントにリクエストを送信。
3. AI がプレーンテキストを返すまで ToolCalls をループ。
4. レスポンスを ChatSystem に永続化。
5. 破棄。

これにより、各ラウンドが分離され、ステートレスになります。

### クラス書き換えによる自己進化

シリコンビーイングは実行時に自分の C# クラスを書き換えることができます：

1. AI が新しいクラスコードを生成（`SiliconBeingBase` を継承する必要があります）。
2. **コンパイル時参照制御**（主要防御）：コンパイラには許可されたアセンブリリストのみが提供されます — `System.IO`、`System.Reflection` などは除外されるため、危険なコードは型レベルで不可能です。
3. **実行時静的分析**（二次防御）：`SecurityScanner` はコンパイル成功後でもコードの危険なパターンをスキャンします。
4. Roslyn がコードをメモリ内でコンパイル。
5. 成功時：`SiliconBeingManager.ReplaceBeing()` が現在のインスタンスをスワップし、状態が移行され、暗号化されたコードがディスクに永続化されます。
6. 失敗時：新しいコードを破棄し、既存の実装を保持。

カスタム `IPermissionCallback` 実装もコンパイルし、`ReplacePermissionCallback()` を介して注入でき、生命体が独自の権限ロジックをカスタマイズできます。

コードはディスク上で AES-256 暗号化で保存されます。暗号化キーは PBKDF2 を介して生命体の GUID（大文字）から派生します。

---

## トークン使用監査

`TokenUsageAuditManager` はすべての生命体にわたる AI トークン消費を追跡します：

- `TokenUsageRecord` — リクエストごとのレコード（生命体 ID、モデル、プロンププトークン、完了トークン、タイムスタンプ）
- `TokenUsageSummary` — 集計統計
- `TokenUsageQuery` — レコードをフィルタリングするためのクエリパラメータ
- 時系列クエリのために `ITimeStorage` を介して永続化
- Web UI（AuditController）および `TokenAuditTool`（キュレーター専用）を介してアクセス可能

---

## カレンダーシステム

システムには、主要な世界のカレンダーシステムをカバーする抽象 `CalendarBase` クラスから派生した**32 種類のカレンダー実装**が含まれています：

| カレンダー | ID | 説明 |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | 仏暦（BE）、年 + 543 |
| CherokeeCalendar | `cherokee` | チェロキーカレンダーシステム |
| ChineseLunarCalendar | `lunar` | 閏月付き中国旧暦 |
| ChulaSakaratCalendar | `chula_sakarat` | Chula Sakarat（CS）、年 - 638 |
| CopticCalendar | `coptic` | コプト暦 |
| DaiCalendar | `dai` | 完全な旧暦計算付き Dai カレンダー |
| DehongDaiCalendar | `dehong_dai` | Dehong Dai カレンダーバリアント |
| EthiopianCalendar | `ethiopian` | エチオピア暦 |
| FrenchRepublicanCalendar | `french_republican` | フランス革命暦 |
| GregorianCalendar | `gregorian` | 標準グレゴリオ暦 |
| HebrewCalendar | `hebrew` | ヘブライ（ユダヤ）暦 |
| IndianCalendar | `indian` | インド国立カレンダー |
| InuitCalendar | `inuit` | イヌイットカレンダーシステム |
| IslamicCalendar | `islamic` | イスラム Hijri 暦 |
| JapaneseCalendar | `japanese` | 日本元号（Nengo）暦 |
| JavaneseCalendar | `javanese` | ジャワイスラム暦 |
| JucheCalendar | `juche` | 主体暦（DPRK）、年 - 1911 |
| JulianCalendar | `julian` | ユリウス暦 |
| KhmerCalendar | `khmer` | クメール暦 |
| MayanCalendar | `mayan` | マヤ長期暦 |
| MongolianCalendar | `mongolian` | モンゴル暦 |
| PersianCalendar | `persian` | ペルシャ（Solar Hijri）暦 |
| RepublicOfChinaCalendar | `roc` | 中華民國（民国）暦、年 - 1911 |
| RomanCalendar | `roman` | ローマ暦 |
| SakaCalendar | `saka` | Saka カレンダー（インドネシア） |
| SexagenaryCalendar | `sexagenary` | 中国干支（Ganzhi） |
| TibetanCalendar | `tibetan` | チベット暦 |
| VietnameseCalendar | `vietnamese` | ベトナム旧暦（Cat 十二支バリアント） |
| VikramSamvatCalendar | `vikram_samvat` | Vikram Samvat 暦 |
| YiCalendar | `yi` | Yi カレンダーシステム |
| ZoroastrianCalendar | `zoroastrian` | ゾロアスター暦 |

`CalendarTool` はアクションを提供します：`now`、`format`、`add_days`、`diff`、`list_calendars`、`get_components`、`get_now_components`、`convert`（カレンダー間日付変換）。

---

## Web UI アーキテクチャ

### スキンシステム

Web UI には、アプリケーションロジックを変更せずに完全な UI カスタマイズを可能にする**プラグ可能なスキンシステム**があります：

- **ISkin インターフェース** — すべてのスキンの契約を定義します：
  - コアレンダリングメソッド（`RenderHtml`、`RenderError`）
  - 20 以上の UI コンポーネントメソッド（ボタン、入力、カード、テーブル、バッジ、バブル、プログレス、タブなど）
  - `CssBuilder` を介したテーマ CSS 生成
  - `SkinPreviewInfo` — 初期化ページスキンセレクターのカラーパレットとアイコン

- **組み込みスキン** — 4 種類の本番環境対応スキン：
  - **Admin** — システム管理用のプロフェッショナルでデータ重視のインターフェース
  - **Chat** — AI インタラクション用の会話中心のデザイン
  - **Creative** — クリエイティブワークフロー用のアーティスティックで視覚的に豊富なレイアウト
  - **Dev** — 構文強調表示付きの開発者向けコード中心インターフェース

- **スキン検出** — `SkinManager` はリフレクションを介してすべての `ISkin` 実装を自動検出して登録します

### HTML / CSS / JS ビルダー

Web UI はテンプレートファイルを完全に回避し、C# ですべてのマークアップを生成します：

- **`H`** — コードで HTML ツリーを構築するためのフルーエント HTML ビルダー DSL
- **`CssBuilder`** — セレクターとメディアクエリサポート付き CSS ビルダー
- **`JsBuilder`（`JsSyntax`）** — インラインスクリプト用 JavaScript ビルダー

### コントローラーシステム

Web UI は異なる側面を処理する 17 個のコントローラーを持つ**MVC ライクなパターン**に従います：

| コントローラー | 目的 |
|------------|---------|
| About | 关于ページおよびプロジェクト情報 |
| Audit | トレンドチャートとエクスポート付きトークン使用監査ダッシュボード |
| Being | シリコンビーイング管理およびステータス |
| Chat | SSE 付きリアルタイムチャットインターフェース |
| CodeBrowser | コード表示および編集 |
| Config | システム設定管理 |
| Dashboard | システム概要およびメトリクス |
| Executor | エグゼキューターステータスおよび管理 |
| Init | 初回実行初期化ウィザード |
| Knowledge | ナレッジグラフの可視化（プレースホルダー） |
| Log | システムログビューア |
| Memory | 高度なフィルタリング、統計、詳細ビュー機能を備えた長期メモリブラウザ |
| Permission | 権限管理 |
| PermissionRequest | 権限リクエストキュー |
| Project | プロジェクト管理（プレースホルダー） |
| Task | タスクシステムインターフェース |
| Timer | タイマーシステム管理 |

### リアルタイム更新

- **SSE（Server-Sent Events）** — `SSEHandler` を介したチャットメッセージ、生命体ステータス、システムイベントのプッシュベースの更新
- **WebSocket 不要** — ほとんどのリアルタイムニーズに SSE を使用するよりシンプルなアーキテクチャ
- **自動再接続** — レジリエントな接続のためのクライアント側再接続ロジック

### ローカライゼーション

3 つのロケールが組み込まれています：`ZhCN`（簡体字中国語）、`ZhHK`（繁体字中国語）、および `EnUS`（英語）。アクティブなロケールは `DefaultConfigData.Language` を介して選択され、`LocalizationManager` を介して解決されます。

---

## データディレクトリ構造

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # キュレーターのソウルファイル
    │   ├── state.json       # 実行時状態
    │   ├── code.enc         # AES 暗号化カスタムクラスコード
    │   └── permission.enc   # AES 暗号化カスタム権限コールバック
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```
