# アーキテクチャ

[中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [English](../architecture.md)

## コアコンセプト

### シリコンビーイング

システムの各AIエージェントは**シリコンビーイング**です — 独自のアイデンティティ、個性、機能を持つ自律的なエンティティ。各シリコンビーイングは、その行動パターンを定義する**ソウルファイル**(Markdownプロンプト)によって駆動されます。

### シリコンキュレーター

**シリコンキュレーター**は、最高システム権限を持つ特別なシリコンビーイングです。システム管理者として機能します:

- 他のシリコンビーイングの作成と管理
- ユーザーリクエストの分析とタスクへの分解
- 適切なシリコンビーイングへのタスク配布
- 実行品質の監視と失敗の処理
- **優先スケジューリング**によるユーザーメッセージへの応答(下記参照)

### ソウルファイル

各シリコンビーイングのデータディレクトリに保存されるMarkdownファイル(`soul.md`)。すべてのAIリクエストに注入されるシステムプロンプトとして機能し、ビーイングの個性、意思決定パターン、行動制約を定義します。

---

## スケジューリング: 時間分割公平スケジューリング

### MainLoop + TickObject

システムは専用バックグラウンドスレッドで**チック駆動メインループ**を実行します:

```
MainLoop(専用スレッド、ウォッチドッグ + サーキットブレーカー)
  └── TickObject A (Priority=0, Interval=100ms)
  └── TickObject B (Priority=1, Interval=500ms)
  └── SiliconBeingManager(MainLoopによって直接チック)
        └── SiliconBeingRunner → シリコンビーイング 1 → チック → ExecuteOneRound
        └── SiliconBeingRunner → シリコンビーイング 2 → チック → ExecuteOneRound
        └── SiliconBeingRunner → シリコンビーイング 3 → チック → ExecuteOneRound
        └── ...
```

主要な設計決定:

- **シリコンビーイングはTickObjectを継承しません。** 独自の`Tick()`メソッドを持ち、MainLoopに直接登録するのではなく、`SiliconBeingManager`によって`SiliconBeingRunner`を介して呼び出されます。
- **SiliconBeingManager**はMainLoopによって直接チックされ、すべてのビーイングの単一プロキシとして機能します。
- **SiliconBeingRunner**はタイムアウトとビーイングごとのサーキットブレーカー(連続3回タイムアウト→1分クールダウン)付きの一時スレッドで各ビーイングの`Tick()`をラップします。
- 各ビーイングの実行はチックごとにAIリクエスト + ToolCallの**1ラウンド**に制限され、ビーイングがメインループを独占できないことを保証します。
- **PerformanceMonitor**は観測可能性のためにチック実行時間を追跡します。

### キュレーター優先応答

ユーザーがシリコンキュレーターにメッセージを送信した場合:

1. 現在のビーイング(例: ビーイングA)は現在のラウンドを終了します — **中断なし**。
2. マネージャーは**残りのキューをスキップ**します。
3. ループは**キュレーターから再開**し、即時実行を提供します。

これにより、進行中のタスクを中断することなく、応答性の高いユーザーインタラクションが保証されます。

---

## コンポーネントアーキテクチャ

```
┌─────────────────────────────────────────────────────────┐
│                        CoreHost                         │
│  (統合ホスト — すべてのコンポーネントを組み立て管理)      │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ MainLoop │  │ ServiceLocator│  │      Config      │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │           SiliconBeingManager (TickObject)        │   │
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
│  │                  エグゼキューター                  │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │  Disk    │  │ Network  │  │  CommandLine    │  │   │
│  │  │Executor  │  │Executor  │  │  Executor       │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              IM プロバイダー                      │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Console  │  │  Web     │  │  Feishu / ...   │  │   │
│  │  │Provider  │  │Provider  │  │  Provider       │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## ServiceLocator

`ServiceLocator`は、すべてのコアサービスへのアクセスを提供するスレッドセーフなシングルトンレジストリです:

| プロパティ | 型 | 説明 |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | 中央チャットセッションマネージャー |
| `IMManager` | `IMManager` | 即時通信プロバイダールーター |
| `AuditLogger` | `AuditLogger` | パーミッション監査証跡 |
| `GlobalAcl` | `GlobalACL` | グローバルアクセス制御リスト |
| `BeingFactory` | `ISiliconBeingFactory` | ビーイング作成用ファクトリ |
| `BeingManager` | `SiliconBeingManager` | アクティブビーイングライフサイクルマネージャー |
| `DynamicBeingLoader` | `DynamicBeingLoader` | 動的コンパイルローダー |
| `TokenUsageAudit` | `ITokenUsageAudit` | トークン使用追跡 |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | トークン使用レポート |

また、ビーイングGUIDをキーとするビーイングごとの`PermissionManager`レジストリも維持します。

---

## チャットシステム

### セッションタイプ

チャットシステムは`SessionBase`を介して3つのセッションタイプをサポートします:

| タイプ | クラス | 説明 |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | 2人の参加者間の1対1の会話 |
| `GroupChat` | `GroupChatSession` | 複数参加者のグループ会話 |
| `Broadcast` | `BroadcastChannel` | 固定IDのオープンチャネル; ビーイングは動的にサブスクライブし、サブスクライブ時間後のメッセージのみを受信 |

### BroadcastChannel

`BroadcastChannel`はシステム全体のアナウンス用の特別なセッションタイプです:

- **固定チャネルID** — `SingleChatSession`と`GroupChatSession`とは異なり、チャネルIDはメンバーGUIDから派生したのではなく、既知の定数です。
- **動的サブスクリプション** — ビーイングは実行時にサブスクライブ/サブスクライブ解除; サブスクリプション時間後に投稿されたメッセージのみを受信します。
- **保留メッセージフィルタリング** — `GetPendingMessages()`は、ビーイングがまだ読んでいない、サブスクリプション時間後に投稿されたメッセージのみを返します。
- **ChatSystemによって管理** — `GetOrCreateBroadcastChannel()`、`Broadcast()`、`GetPendingBroadcasts()`。

### ChatMessage

`ChatMessage`モデルには、AI会話コンテキストとトークン追跡用のフィールドが含まれています:

| フィールド | 型 | 説明 |
|-------|------|-------------|
| `Id` | `Guid` | 一意のメッセージ識別子 |
| `SenderId` | `Guid` | 送信者の一意の識別子 |
| `ChannelId` | `Guid` | チャネル/会話識別子 |
| `Content` | `string` | メッセージコンテンツ |
| `Timestamp` | `DateTime` | メッセージ送信時刻 |
| `Type` | `MessageType` | Text、Image、File、またはSystemNotification |
| `ReadBy` | `List<Guid>` | このメッセージを読んだ参加者のID |
| `Role` | `MessageRole` | AI会話ロール(User、Assistant、Tool) |
| `ToolCallId` | `string?` | ツール結果メッセージ用のツールコールID |
| `ToolCallsJson` | `string?` | アシスタントメッセージ用のシリアライズされたツールコールJSON |
| `Thinking` | `string?` | AIからの連鎖推論 |
| `PromptTokens` | `int?` | プロンプト(入力)のトークン数 |
| `CompletionTokens` | `int?` | コンプリート(出力)のトークン数 |
| `TotalTokens` | `int?` | 使用された総トークン数(入力 + 出力) |

---

## AIクライアントシステム

システムは`IAIClient`インターフェースを通じて複数のAIバックエンドをサポートします：

### OllamaClient

- **タイプ**：ローカルAIサービス
- **プロトコル**：ネイティブOllama HTTP API（`/api/chat`、`/api/generate`）
- **機能**：ストリーミング出力、ツールコール、ローカルモデルホスティング
- **設定項目**：`endpoint`、`model`、`temperature`、`maxTokens`

### DashScopeClient（Alibaba Cloud百煉）

- **タイプ**：クラウドAIサービス
- **プロトコル**：OpenAI互換API（`/compatible-mode/v1/chat/completions`）
- **認証方式**：Bearer Token（APIキー）
- **機能**：ストリーミング出力、ツールコール、推論コンテンツ（Reasoning Content）、マルチリージョンデプロイメント
- **サポートリージョン**：
  - `beijing` — 華北2（北京）
  - `virginia` — 米国（バージニア）
  - `singapore` — シンガポール
  - `hongkong` — 中国香港
  - `frankfurt` — ドイツ（フランクフルト）
- **サポートモデル**（APIによる動的発見、フォールバックリスト付き）：
  - **Qwenシリーズ**：qwen3-max、qwen3.6-plus、qwen3.6-flash、qwen-max、qwen-plus、qwen-turbo、qwen3-coder-plus
  - **推論モデル**：qwq-plus
  - **サードパーティモデル**：deepseek-v3.2、deepseek-r1、glm-5.1、kimi-k2.5、llama-4-maverick
- **設定項目**：`apiKey`、`region`、`model`
- **モデル発見**：実行時にDashScope APIから利用可能なモデルリストを取得；ネットワーク障害時はプリセットリストにフォールバック

### クライアントファクトリーパターン

各AIクライアントタイプには`IAIClientFactory`を実装する対応するファクトリーがあります：

- `OllamaClientFactory` — OllamaClientインスタンスを作成
- `DashScopeClientFactory` — DashScopeClientインスタンスを作成

ファクトリーが提供：
- `CreateClient(Dictionary<string, object> config)` — 設定ディクショナリからクライアントインスタンスを作成
- `GetConfigKeyOptions(string key, ...)` — 設定キーの動的オプションを返す（利用可能なモデル、リージョンリストなど）
- `GetDisplayName()` — クライアントタイプのローカライズされた表示名

---

## 主要な設計決定

### インスタンスクラスとしてのストレージ(静的ではない)

`IStorage`は静的ユーティリティではなく、注入可能なインスタンスとして設計されています。これにより以下が保証されます:

- 直接ファイルシステムアクセス — IStorageはシステムの内部永続化チャネルであり、エグゼキューターを介して**ルーティングされません**。
- **AIはIStorageを制御できません** — エグゼキューターはAIツールによって開始されたIOを管理; IStorageはフレームワーク独自の内部データ読み取り/書き込みを管理。これらは根本的に異なる関心事です。
- モック実装でのテスト可能性。
- コンシューマーを変更せずに、将来の異なるストレージバックエンドのサポート。

### セキュリティ境界としてのエグゼキューター

エグゼキューターはI/O操作の**唯一**のパスです。ディスク、ネットワーク、またはコマンドラインアクセスを必要とするツールは**必ず**エグゼキューターを通過する必要があります。この設計は以下を強制します:

- 各エグゼキューターは、パーミッション検証用のスレッドロック付き**独立スケジューリングスレッド**を所有。
- 集中パーミッションチェック — エグゼキューターはビーイングの**プライベートPermissionManager**にクエリを実行。
- 優先度サポートとタイムアウト制御付きリクエストキューイング。
- すべての外部操作の監査ログ。
- 例外分離 — 1つのエグゼキューターの失敗が他のエグゼキューターに影響を与えない。
- サーキットブレーカー — 連続失敗により、カスケード障害を防ぐためにエグゼキューターを一時的に停止。

### 軽量オブジェクトとしてのContextManager

各`ExecuteOneRound()`は新しい`ContextManager`インスタンスを作成します:

1. ソウルファイル + 最近のチャット履歴を読み込み。
2. AIクライアントにリクエストを送信。
3. AIがプレーンテキストを返すまでToolCallをループ。
4. 応答をChatSystemに永続化。
5. 破棄。

これにより、各ラウンドが分離されステートレスになります。

### クラス書き換えによる自己進化

シリコンビーイングは実行時に独自のC#クラスを書き換えることができます:

1. AIが新しいクラスコードを生成(`SiliconBeingBase`を継承する必要があります)。
2. **コンパイル時参照制御**(主要防御): コンパイラーには許可されたアセンブリリストのみが与えられます — `System.IO`、`System.Reflection`などは除外されるため、危険なコードはタイプレベルで不可能です。
3. **実行時静的分析**(二次防御): `SecurityScanner`は、コンパイル成功後でもコードの危険なパターンをスキャン。
4. Roslynがコードをメモリ内でコンパイル。
5. 成功時: `SiliconBeingManager.ReplaceBeing()`が現在のインスタンスをスワップ、状態が移行され、暗号化されたコードがディスクに永続化。
6. 失敗時: 新しいコードを破棄、既存の実装を保持。

カスタム`IPermissionCallback`実装も`ReplacePermissionCallback()`を介してコンパイルおよび注入でき、ビーイングが独自のパーミッションロジックをカスタマイズできます。

コードはディスク上でAES-256暗号化で保存されます。暗号化鍵はPBKDF2を介してビーイングのGUID(大文字)から派生します。

---

## トークン使用監査

`TokenUsageAuditManager`はすべてのビーイングのAIトークン消費を追跡します:

- `TokenUsageRecord` — リクエストごとのレコード(ビーイングID、モデル、プロンプトトークン、コンプリートトークン、タイムスタンプ)
- `TokenUsageSummary` — 集計統計
- `TokenUsageQuery` — レコードフィルタリング用のクエリパラメーター
- 時系列クエリ用に`ITimeStorage`を介して永続化
- Web UI(AuditController)および`TokenAuditTool`(キュレーター専用)を介してアクセス可能

---

## カレンダーシステム

システムには、主要な世界のカレンダーシステムをカバーする抽象`CalendarBase`クラスから派生した**32のカレンダー実装**が含まれています:

| カレンダー | ID | 説明 |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | 仏教紀元(BE)、年 + 543 |
| CherokeeCalendar | `cherokee` | チェロキーカレンダーシステム |
| ChineseLunarCalendar | `lunar` | 閏月付き中国太陰暦 |
| ChulaSakaratCalendar | `chula_sakarat` | チュラサカラート(CS)、年 - 638 |
| CopticCalendar | `coptic` | コプト暦 |
| DaiCalendar | `dai` | 完全な太陰計算付きタイ族暦 |
| DehongDaiCalendar | `dehong_dai` | 徳宏タイ族暦バリアント |
| EthiopianCalendar | `ethiopian` | エチオピア暦 |
| FrenchRepublicanCalendar | `french_republican` | フランス革命暦 |
| GregorianCalendar | `gregorian` | 標準グレゴリオ暦 |
| HebrewCalendar | `hebrew` | ヘブライ(ユダヤ)暦 |
| IndianCalendar | `indian` | インド国立暦 |
| InuitCalendar | `inuit` | イヌイットカレンダーシステム |
| IslamicCalendar | `islamic` | イスラムヒジュラ暦 |
| JapaneseCalendar | `japanese` | 日本元号(Nengo)暦 |
| JavaneseCalendar | `javanese` | ジャワイスラム暦 |
| JucheCalendar | `juche` | 主体暦(北朝鮮)、年 - 1911 |
| JulianCalendar | `julian` | ユリウス暦 |
| KhmerCalendar | `khmer` | クメール暦 |
| MayanCalendar | `mayan` | マヤ長期暦 |
| MongolianCalendar | `mongolian` | モンゴル暦 |
| PersianCalendar | `persian` | ペルシャ(太陽ヒジュラ)暦 |
| RepublicOfChinaCalendar | `roc` | 中華民国(民国)暦、年 - 1911 |
| RomanCalendar | `roman` | ローマ暦 |
| SakaCalendar | `saka` | サカ暦(インドネシア) |
| SexagenaryCalendar | `sexagenary` | 中国六十干支(干支Ganzhi) |
| TibetanCalendar | `tibetan` | チベット暦 |
| VietnameseCalendar | `vietnamese` | 猫の十二支バリアント付きベトナム太陰暦 |
| VikramSamvatCalendar | `vikram_samvat` | ヴィクラム・サムヴァト暦 |
| YiCalendar | `yi` | イー族カレンダーシステム |
| ZoroastrianCalendar | `zoroastrian` | ゾロアスター暦 |

`CalendarTool`はアクションを提供: `now`、`format`、`add_days`、`diff`、`list_calendars`、`get_components`、`get_now_components`、`convert`(カレンダー間日付変換)。

---

## Web UIアーキテクチャ

### スキンシステム

Web UIには、アプリケーションロジックを変更せずに完全なUIカスタマイズを可能にする**プラグイン可能なスキンシステム**があります:

- **ISkinインターフェース** — すべてのスキンの契約を定義:
  - コアレンダリングメソッド(`RenderHtml`、`RenderError`)
  - 20以上のUIコンポーネントメソッド(ボタン、入力、カード、テーブル、バッジ、バブル、プログレス、タブなど)
  - `CssBuilder`を介したテーマCSS生成
  - `SkinPreviewInfo` — 初期ページスキンセレクターのカラースウォッチとアイコン

- **組み込みスキン** — 4つの実用的なスキン:
  - **Admin** — システム管理用のプロフェッショナルでデータ重視のインターフェース
  - **Chat** — AIインタラクション用の会話中心のデザイン
  - **Creative** — クリエイティブワークフロー用の芸術的で視覚的に豊富なレイアウト
  - **Dev** — シンタックスハイライト付きの開発者向けコード中心インターフェース

- **スキン検出** — `SkinManager`はリフレクションを介してすべての`ISkin`実装を自動検出および登録

### HTML / CSS / JS ビルダー

Web UIはテンプレートファイルを完全に回避し、C#ですべてのマークアップを生成:

- **`H`** — コードでHTMLツリーを構築するためのフルーエントHTMLビルダーDSL
- **`CssBuilder`** — セレクターとメディアクエリサポート付きCSSビルダー
- **`JsBuilder` (`JsSyntax`)** — インラインスクリプト用JavaScriptビルダー

### コントローラーシステム

Web UIは、異なる側面を処理する17のコントローラーを持つ**MVC類似パターン**に従います:

| コントローラー | 目的 |
|------------|---------|
| About | 紹介ページとプロジェクト情報 |
| Audit | トレンドチャートとエクスポート付きトークン使用監査ダッシュボード |
| Being | シリコンビーイング管理とステータス |
| Chat | SSE付きリアルタイムチャットインターフェース |
| CodeBrowser | コード閲覧と編集 |
| Config | システム設定管理 |
| Dashboard | システム概要とメトリクス |
| Executor | エグゼキューターステータスと管理 |
| Init | 初回実行初期化ウィザード |
| Knowledge | ナレッジグラフ可視化(プレースホルダー) |
| Log | システムログビューア |
| Memory | 長期メモリブラウザー |
| Permission | パーミッション管理 |
| PermissionRequest | パーミッションリクエストキュー |
| Project | プロジェクト管理(プレースホルダー) |
| Task | タスクシステムインターフェース |
| Timer | タイマーシステム管理 |

### リアルタイム更新

- **SSE(サーバー送信イベント)** — チャットメッセージ、ビーイングステータス、システムイベント用のプッシュベースの更新(`SSEHandler`経由)
- **WebSocket不要** — ほとんどのリアルタイムニーズにSSEを使用するより単純なアーキテクチャ
- **自動再接続** — レジリエント接続用のクライアント側再接続ロジック

### ローカリゼーション

3つのロケールが組み込まれています: `ZhCN`(簡体字中国語)、`ZhHK`(繁体字中国語)、`EnUS`(英語)。アクティブなロケールは`DefaultConfigData.Language`を介して選択され、`LocalizationManager`を介して解決されます。

---

## データディレクトリ構造

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # キュレーターのソウルファイル
    │   ├── state.json       # 実行時状態
    │   ├── code.enc         # AES暗号化されたカスタムクラスコード
    │   └── permission.enc   # AES暗号化されたカスタムパーミッションコールバック
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```