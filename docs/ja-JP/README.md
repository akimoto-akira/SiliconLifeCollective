![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**バージョン：v0.2.0-alpha** | **シリコンライフコレクティブ** — .NET 9 ベースのマルチエージェント協調プラットフォーム。AI エージェントは**シリコンビーイング**と呼ばれ、Roslyn 動的コンパイルによる自己進化を実現。

[English](../README.md) | [Deutsch](../de-DE/README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | **日本語** | [한국어](../ko-KR/README.md) | [Français](../fr-FR/README.md) | [Čeština](../cs-CZ/README.md) | [Italiano](../it-IT/README.md) | [Polski](../pl-PL/README.md) | [Português](../pt-PT/README.md) | [Русский](../ru-RU/README.md)

## 🌟 コア機能

### エージェントシステム
- **マルチエージェントオーケストレーション** — *シリコンキュレーター*による統一管理、クロック駆動のタイムスロット公平スケジューリング機構を採用
- **ソウルファイル駆動** — 各シリコンビーイングはコアプロンプトファイル（`soul.md`）により駆動され、独自の個性と行動パターンを定義
- **ボディ・ブレインアーキテクチャ** — *ボディ*（SiliconBeing）はバイタルサインを維持しトリガーシーンを検出、*ブレイン*（ContextManager）は履歴の読み込み、AI の呼び出し、ツールの実行、レスポンスの永続化を担当
- **自己進化能力** — Roslyn 動的コンパイル技術により、シリコンビーイングは自身のコードを書き換えて進化可能
- **アクティビティ状態管理** — Idle（アイドル）、SingleChat（1対1チャット）、GroupChat（グループチャット）、Task（タスク）、Timer（タイマー）、Broadcast（ブロードキャスト）、Project（プロジェクト）、MemoryCompression（メモリ圧縮）、Stopped（停止）の9つのアクティビティ状態をサポート、連続10回のエラーで自動的に Stopped 状態に移行

### プラグインシステム
- **プラグイン拡張アーキテクチャ** — IPlugin インターフェースによる機能拡張、ディレクトリからのプラグイン DLL 動的読み込みをサポート
- **プラグイン能力宣言** — プラグインは `[PluginCapability]` 属性で必要な能力（Network, FileIO, Process, AI）を宣言し、ローダーはそれに基づいてセキュリティスキャンルールを緩和します。宣言不可能な能力（P/Invoke, Unsafe, Reflection Emit 等）は常にブロックされます
- **隔離読み込み** — カスタム AssemblyLoadContext による隔離読み込みで、プラグインがメインプログラムの安定性に影響するのを防止
- **ツール統合** — プラグインは ITool インターフェースを通じてカスタムツールを登録し、ツールコールループに自動統合

### ツールと実行
- **24個の組み込みツール** — カレンダー、チャット、設定、ディスク、ネットワーク、メモリ、タスク、タイマー、ナレッジベース、ワークノート、プロジェクトワークスペース、WebView ブラウザ等をカバー
- **ツールシーン分離** — 各ツールは `ToolScenario` プロパティで使用可能シーン（Chat、Task、Timer、MemoryCompression、Project）を宣言、`ChatOnly` プロパティでチャットシーン専用に制限
  - **IAIClient 能力インターフェース** — AI クライアントはストリーミングモード、ツールコール、コンテキストウィンドウ、ビジョン、オーディオの能力を宣言し、ContextManager はこれに基づいて適応的に動作を調整
- **ツールコールループ** — AI がツールコールを返す → ツールを実行 → 結果を AI にフィードバック → 純テキストレスポンスが返るまでループ継続
- **エグゼキューター・パーミッションセキュリティ** — すべての I/O 操作はエグゼキューターを通じて厳格なパーミッション検証を実施
  - 3レベルパーミッション検証チェーン：UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curator: グローバルACL → デフォルト拒否)
  - すべてのパーミッション決定を記録する完全な監査ログ

### スキルシステム
- **再利用可能な能力ユニット** — 「ツールオーケストレーション + プロンプトテンプレート」を宣言可能、進化可能、スケジュール可能なスキルとしてカプセル化。AI はツールを呼び出すようにスキルを呼び出します
- **デュアルトリガーモード** — Manual（AI 関数呼び出しで自律判断）+ Auto（schedule スケジュール：毎日定時 / 間隔周期 / cron サブセット）
- **Markdown 優先** — YAML フロントメタデータ + プロンプト本文。純 Markdown 保存時に AI が欠落メタデータを自動補完（ユーザーフィールドは上書きされません）
- **ホットリロードとバージョンアーカイブ** — 30 秒フィンガープリント検出で自動反映。更新のたびに `skills/archive/{id}/{version}.md` にアーカイブし進化履歴を形成
- **多層ガードレール** — グローバルスイッチ、クォータ制限（デフォルト 50/ビーイング）、グローバルラウンド数とタイムアウト制限、ツールホワイトリスト、再帰防护、スキルレベルのアクションパーミッション

### MCP 統合
- **外部ツールアクセス** — 外部 MCP（Model Context Protocol）サーバーに接続。そのツールは `mcp_{serverId}_{toolName}` という名前で全シリコンビーイングに自動注入され、コード記述不要
- **デュアルトランスポート** — stdio（ローカルサブプロセス）と http（リモートエンドポイント）
- **ユーザー主権** — サーバーの追加/削除/起動/停止は Web UI のみ。AI 側の `mcp` ツールは読み取り専用
- **パーミッション一貫性** — MCP ラップツールは 2 レベルツールパーミッションマトリクスに組み込まれ、ビーイング/プロジェクト単位で無効化可能

### インスタントメッセージング統合
- **マルチインスタンスアーキテクチャ** — 複数の IM プラットフォーム（Web UI / Feishu / WeChat Enterprise / DingTalk）を同時接続可能。各インスタンスは独立して起動/停止可能、メッセージは集約ルーティング
- **OAuth 認証ウィザード** — Feishu ワンクリック認証（state による CSRF 防止、SSE によるリアルタイム状態プッシュ）、トークンは自動的に設定に書き戻し
- **シークレットセキュリティ** — 設定値は `${ENV_VAR}` 環境変数プレースホルダーをサポート。平文シークレットはディスクに保存されません

### AI とナレッジ
- **マルチ AI バックエンドサポート**
  - **Ollama** — ローカルモデルデプロイ、ネイティブ HTTP API 使用
  - **Alibaba Cloud Bailian（DashScope）** — クラウド AI サービス、OpenAI API 互換、13以上のモデルをサポート、マルチリージョンデプロイ
  - **Volcengine Ark** — ByteDance クラウド AI サービス、ストリーミング/非ストリーミングモード対応、内蔵レート制御
  - **Herdsman** — 認証不要の推論エンジン、OpenAI API 形式互換
  - **Meituan LongCat** — 美団独自開発大規模モデル、LongCat-2.0 は 1M コンテキストと思考モードをサポート、OpenAI API 形式互換
  - **Qiniu Cloud AI** — 七牛クラウド大規模モデル推論サービス、OpenAI API 形式互換、API キー認証
  - **DeepSeek** — 思考モード対応、1M コンテキスト、OpenAI API 互換
  - **Zhipu AI（GLM）** — 思考モード対応、モデル別ビジョン、1M コンテキスト、OpenAI API 互換
  - **Baidu Qianfan（ERNIE）** — 131K コンテキスト、モデル別ビジョン、OpenAI API 互換
  - **Tencent Hunyuan** — TokenHub + レガシーデュアルエンドポイント、262K コンテキスト
  - **MiniMax** — 国内/国際エンドポイント、1M コンテキスト、OpenAI API 互換
  - **月の暗面（Kimi/Moonshot）** — 思考モード対応、262K コンテキスト、OpenAI API 互換
  - **SiliconFlow** — 動的モデルリスト、1M コンテキスト、OpenAI API 互換
- **32種類のカレンダーシステム** — グレゴリオ暦、農暦、イスラム暦、ヘブライ暦、日本暦、ペルシャ暦、マヤ暦、中国歴史暦法など、世界の主要暦法を網羅
- **ナレッジネットワークシステム** — 三つ組（主体・関係・客体）ベースのナレッジグラフ、保存・検索・パス発見をサポート
- **プロジェクトワークスペース** — プロジェクトスペース管理、プロジェクトの作成/アーカイブ/破棄、ロール割り当て、ワークノート、タスク追跡、ツールパーミッション分離をサポート
- **ワークフローエンジン** — テンプレートベースのステートマシンエンジン、カスタムワークフローテンプレート、状態遷移、Tick 駆動実行、インスタンスライフサイクル管理をサポート
- **メモリフェードメカニズム** — 定時減衰サービス（MemoryFadeService）、毎時自動的に全シリコンビーイングのメモリに対して重要度減衰と自動アーカイブを実行

### Web インターフェース
- **モダン Web UI** — 内蔵 HTTP サーバー、SSE リアルタイム更新をサポート
- **7種類のスキンテーマ** — 管理版、チャット版、クリエイティブ版、開発版、ハイコントラスト、ライト、ミニマル、自動検出と切り替えをサポート
- **24個のコントローラー** — 完全なシステム管理、チャット、設定、監視機能
- **ゼロフロントエンドフレームワーク依存** — `H`、`CssBuilder`、`JsBuilder` によりサーバー側で HTML/CSS/JS を生成

### 国際化とローカリゼーション
- **34種類の言語バリアント** を完全サポート、2種類の書記体系と複数の地域バリアントを網羅
  - **簡体字中国語**：zh-CN（中国大陸）、zh-SG（シンガポール）、zh-MY（マレーシア）（3種類）
  - **繁体字中国語**：zh-HK（香港）、zh-TW（台湾）、zh-MO（マカオ）（3種類）
  - **英語**：en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY（10種類）
  - **スペイン語**：es-ES, es-MX（2種類）
  - **ドイツ語**：de-DE, de-AT, de-CH, de-LU, de-LI（5種類）
  - **フランス語**：fr-FR, fr-CA, fr-CH（3種類）
  - **日本語**：ja-JP | **韓国語**：ko-KR | **チェコ語**：cs-CZ（3種類）
  - **イタリア語**：it-IT | **ポーランド語**：pl-PL | **ポルトガル語**：pt-PT, pt-BR（4種類）

### データとストレージ
- **SpeedyPack 高性能ストレージ** — Fast 版は独自の .spk ストレージエンジンを使用、メモリディレクトリマップ + エントリキャッシュ + 非同期ライトキュー
- **ファイルシステムストレージ** — Default 版は純粋なファイルシステム JSON ストレージを使用
- **時間インデックスクエリ** — `ITimeStorage` インターフェースによる時間範囲での高効率クエリをサポート
- **自動コンパクション** — SpeedyPack は定時自動コンパクションをサポート、空き領域を回収
- **最小依存** — コアライブラリは動的コンパイル用の Microsoft.CodeAnalysis.CSharp のみに依存

## 🔄 デュアルバージョンアーキテクチャ

本プロジェクトは2つの実装バージョンを提供し、異なるユースケースに対応します：

### SiliconLife.Default（デフォルトバージョン）
- **位置付け**：デフォルト実装、主にアーキテクチャの実現可能性の検証用
- **実行モード**：コンソールアプリケーション
- **ストレージ方式**：純粋なファイルシステム JSON ストレージ
- **適用シーン**：データ安全性の要件が高い、メモリリソースが制限されている、データ量が少ないシーン
- **特徴**：シンプルで信頼性が高い、データの即時永続化、メモリ損失リスクなし
- **ロール説明**：アーキテクチャ検証のベースライン実装として、初回利用、開発デバッグ、データセキュリティ優先のシーンに適しています
- **起動コマンド**：`dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast（高性能バージョン）
- **位置付け**：本番推奨バージョン
- **実行モード**：デスクトップアプリケーション（Windows/macOS システムトレイ / Linux ステータスウィンドウ）
- **ストレージ方式**：SpeedyPack メモリストレージ + 非同期バッチ永続化（.spk ファイルフォーマット）
- **適用シーン**：高同時接続、低レイテンシ、大データ量のシーン
- **プラットフォームサポート**：Windows/macOS（完全機能、システムトレイ付き）、Linux（ステータスウィンドウ、トレイアイコンなし）
- **特徴**：
  - 極限のパフォーマンス最適化
  - Windows/macOS トレイバックグラウンド実行、トレイステータスウィンドウによるリアルタイム監視をサポート；Linux はステータスウィンドウを直接表示
  - SpeedyPack エンジン + 自動コンパクションでデータ安全性を保証
  - Component UI アーキテクチャ、27個の宣言型コンポーネント
  - 7種類のスキンテーマ、自動検出と切り替えをサポート
  - ホットリロードツールによるオンライン更新と再起動をサポート → Linux で自動的にブラウザを開き Web UI にアクセス、`--no-tray` パラメータをサポート
- **パフォーマンス向上**：ストレージ読み取りレイテンシ1000分の1、書き込みレイテンシ15000分の1、同時処理能力50倍向上
- **ロール説明**：深く最適化された本番級実装、長期運用と実際の本番環境の推奨
- **起動コマンド**：`dotnet run --project src/SiliconLife.Fast`

### バージョン比較

| 特徴 | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| **実行モード** | コンソールアプリ | デスクトップアプリ（Windows/macOS システムトレイ / Linux ステータスウィンドウ） |
| **ユーザーインターフェース** | Web UI（ブラウザアクセス） | Windows/macOS：トレイアイコン + トレイウィンドウ + Web UI；Linux：ステータスウィンドウ + Web UI |
| **システムトレイ** | ❌ なし | ✅ Windows/macOS トレイ最小化対応；Linux はトレイアイコンなし |
| **バックグラウンド実行** | ❌ コンソールを閉じると終了 | ✅ Windows/macOS トレイでバックグラウンド継続実行；Linux はステータスウィンドウで実行 |
| **ストレージ方式** | ファイルシステム JSON ストレージ | SpeedyPack メモリストレージ + 非同期永続化 |
| **ストレージエンジン** | ファイルシステム I/O | SiliconLife.Speedy（.spk フォーマット） |
| **読み取りレイテンシ** | ~10ms（ディスク I/O） | ~0.01ms（メモリ操作） |
| **書き込みレイテンシ** | ~15ms（同期書き込み） | ~0.001ms（非同期書き込み） |
| **同時処理能力** | ~100 req/s | ~5000 req/s |
| **メモリ使用量** | ~200MB | ~500MB |
| **データ安全性** | 極めて高い（即時永続化） | 高い（非同期永続化 + 自動コンパクション） |
| **適用シーン** | データセキュリティ優先、小データ量 | パフォーマンス優先、大データ量、高同時接続 |

## 🛠️ 技術スタック

| コンポーネント | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| ランタイム | .NET 9 | .NET 9（Windows/macOS/Linux） |
| プログラミング言語 | C# | C# |
| アプリケーションタイプ | コンソールアプリケーション | デスクトップアプリケーション（Windows/macOS システムトレイ / Linux ステータスウィンドウ） |
| AI 統合 | Ollama、Alibaba Cloud Bailian、Volcengine Ark、Herdsman、LongCat、QiniuAI、DeepSeek、Zhipu AI、Baidu Qianfan、Tencent Hunyuan、MiniMax、Moonshot、SiliconFlow | Ollama、Alibaba Cloud Bailian、Volcengine Ark、Herdsman、LongCat、QiniuAI、DeepSeek、Zhipu AI、Baidu Qianfan、Tencent Hunyuan、MiniMax、Moonshot、SiliconFlow |
| データストレージ | ファイルシステム（JSON + 時間インデックスディレクトリ） | SpeedyPack（.spk フォーマット、メモリマップ + 非同期永続化） |
| Web サーバー | HttpListener（.NET 内蔵） | HttpListener（.NET 内蔵） |
| 動的コンパイル | Roslyn（Microsoft.CodeAnalysis.CSharp 4.13.0） | Roslyn（Microsoft.CodeAnalysis.CSharp 4.13.0） |
| ブラウザ自動化 | Playwright（WebView） | Playwright（WebView） |
| プラグインシステム | ✅ サポート（IPlugin + PluginLoader） | ✅ サポート（IPlugin + PluginLoader） |
| システムトレイ | ❌ 非サポート | ✅ Windows/macOS サポート（NotifyIcon）；Linux はトレイアイコンなし |
| ライセンス | Apache-2.0 | Apache-2.0 |

## 📁 プロジェクト構造

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # コアライブラリ（インターフェース、抽象クラス）
│   │   ├── AI/                            # AI クライアントインターフェース、コンテキストマネージャー、メッセージモデル
│   │   ├── Audit/                         # トークン使用監査システム
│   │   ├── Chat/                          # チャットシステム、セッション管理、ブロードキャストチャンネル
│   │   ├── Compilation/                   # 動的コンパイル、セキュリティスキャナー、コード暗号化
│   │   ├── Config/                        # 設定システム
│   │   ├── Executors/                     # エグゼキューター（ディスク、ネットワーク、コマンドライン）
│   │   ├── IM/                            # IMプロバイダーインターフェース
│   │   ├── Knowledge/                     # ナレッジネットワークシステム
│   │   ├── Localization/                  # ローカリゼーションシステム
│   │   ├── Logging/                       # ロギングシステム
│   │   ├── Plugins/                       # プラグインシステム（IPlugin インターフェース、PluginLoader ローダー）
│   │   ├── Project/                       # プロジェクト管理システム
│   │   ├── Runtime/                       # メインループ、ティックオブジェクト、コアホスト
│   │   ├── Security/                      # パーミッション管理システム
│   │   ├── SiliconBeing/                  # シリコンビーイング基底クラス、マネージャー、ファクトリー
│   │   ├── Storage/                       # ストレージインターフェース
│   │   ├── Time/                          # 不完全日付（時間範囲クエリ）
│   │   ├── Tools/                         # ツールインターフェースとツールマネージャー
│   │   ├── WebView/                       # WebView ブラウザインターフェース
│   │   ├── Workflow/                      # ワークフローエンジン（テンプレート、インスタンス、状態遷移）
│   │   └── ServiceLocator.cs              # グローバルサービスロケーター
│   │
│   ├── SiliconLife.Common/                # 共有実装（両バージョン共通）
│   │   ├── AI/                            # AI クライアントとファクトリー（Ollama、DashScope、VolcengineArk、Herdsman、LongCat、QiniuAI、DeepSeek、Zhipu、Ernie、Hunyuan、MiniMax、Moonshot、SiliconFlow）
│   │   ├── Calendar/                      # 32種類のカレンダー実装
│   │   ├── Localization/                  # ローカリゼーションベースクラスと34種類の言語/地域バリアント実装
│   │   ├── Resources/                     # 共有リソースファイル
│   │   ├── Security/                      # パーミッションマネージャー
│   │   ├── SiliconBeing/                  # デフォルトシリコンビーイング実装
│   │   ├── Tools/                         # 23個の汎用ツール実装
│   │   ├── Web/                           # Web インフラストラクチャ
│   │   └── WebView/                       # Playwright WebView 実装
│   │
│   ├── SiliconLife.App/                   # アプリケーション層（Web UI + ヘルプドキュメント、Default と Fast 共有）
│   │   ├── Config/                        # アプリケーション設定
│   │   ├── Data/                          # データディレクトリ
│   │   ├── Help/                          # ヘルプドキュメントローカリゼーション（多言語）
│   │   ├── Tools/                         # HelpTool（ヘルプドキュメント検索ツール）
│   │   └── Web/                           # Web UI 実装
│   │       ├── Component/                 # UI コンポーネントライブラリ（27個のコンポーネント）
│   │       ├── Controllers/               # 24個のコントローラー
│   │       ├── Models/                    # ビューモデル
│   │       ├── Views/                     # HTML ビュー
│   │       └── Skins/                     # 7種類のスキンテーマ
│   │
│   ├── SiliconLife.Default/               # デフォルト実装 + アプリケーションエントリ（コンソール版）
│   │   ├── Program.cs                     # エントリポイント（全コンポーネントの組み立て）
│   │   ├── Config/                        # デフォルト設定データ
│   │   ├── Knowledge/                     # ナレッジネットワーク実装
│   │   ├── Logging/                       # ロガープロバイダー実装（コンソール + ファイルシステム）
│   │   ├── Project/                       # プロジェクトシステム実装
│   │   └── Storage/                       # ファイルシステムストレージ実装
│   │
│   ├── SiliconLife.Fast/                  # 高性能実装 + アプリケーションエントリ（ウィンドウ版）
│   │   ├── Program.cs                     # エントリポイント（ウィンドウアプリケーション）
│   │   ├── App.axaml / App.cs             # Avalonia アプリ定義
│   │   ├── Config/                        # 設定データ（Default と共有）
│   │   ├── Knowledge/                     # ナレッジネットワーク実装（メモリ最適化）
│   │   ├── Logging/                       # 高性能ロガープロバイダー
│   │   ├── Project/                       # プロジェクトシステム実装
│   │   ├── Storage/                       # SpeedyPack ストレージアダプター
│   │   └── Tray/                          # システムトレイ（34種類の言語バリアントローカリゼーション）
│   │
│   ├── SiliconLife.Speedy/                # SpeedyPack 高性能ストレージエンジン
│   │   ├── SpeedyPack.cs                  # コアクラス（メモリディレクトリマップ + キャッシュ + 非同期書き込み）
│   │   ├── SpeedyPackOptions.cs           # 設定オプション（キャッシュ TTL、最大エントリ数など）
│   │   ├── IPackTransaction.cs            # トランザクションインターフェース
│   │   ├── SpkFileInfo.cs                 # ファイル情報
│   │   └── Internal/                      # 内部実装
│   │       ├── DirectoryMap.cs            # メモリディレクトリマップ
│   │       ├── EntryCache.cs              # エントリキャッシュ
│   │       ├── FreeList.cs                # フリーリスト
│   │       ├── PackFileReader.cs          # パックファイルリーダー
│   │       ├── PackFileWriter.cs          # パックファイルライター
│   │       ├── WriteQueue.cs              # 非同期ライトキュー
│   │       ├── WriteOperation.cs          # 書き込み操作
│   │       ├── SpeedyTransaction.cs       # トランザクション実装
│   │       ├── SpkHeader.cs               # パックファイルヘッダー
│   │       └── PathNormalizer.cs          # パス正規化
│   │
│   └── SiliconLife.Speedy.Manager/        # SpeedyPack マネージャー（Avalonia UI）
│       ├── MainForm.cs                    # メインフォーム
│       ├── Program.cs                     # エントリポイント
│       └── slc.ico                        # アプリケーションアイコン
│
├── docs/                                  # 多言語ドキュメント
│   ├── zh-CN/                             # 簡体字中国語ドキュメント
│   ├── en/                                # 英語ドキュメント
│   └── ...                                # その他の言語ドキュメント
│
└── 总文档/                                 # 要件ドキュメントとアーキテクチャドキュメント
    ├── 需求文档.md
    ├── 架构大纲.md
    └── 实现顺序.md
```

## 🏗️ アーキテクチャ概要

### スケジューリングアーキテクチャ
```
メインループ（専用スレッド、ウォッチドッグ + サーキットブレーカー）
  └── ティックオブジェクト（優先度順）
       └── シリコンビーイングマネージャー
            └── シリコンビーイングランナー（一時スレッド、タイムアウト + サーキットブレーカー）
                 └── シリコンビーイング.Tick()
                      └── コンテキストマネージャー.思考()
                           └── AI クライアント.チャット()
                                └── ツールコールループ → チャットシステムに永続化
```

### セキュリティアーキテクチャ
すべての AI 発信の I/O 操作は厳格なセキュリティチェーンを通過する必要があります：

```
ツールコール → エグゼキューター → パーミッションマネージャー → [高頻度キャッシュ → コールバック → (IsCurator: ユーザーに確認 | Non-curator: グローバルACL)]
```

## 🚀 クイックスタート

### 前提条件

- **.NET 9 SDK** — [ダウンロードリンク](https://dotnet.microsoft.com/download/dotnet/9.0)
- **AI バックエンド**（いずれかを選択）：
  - **Ollama**：[Ollama をインストール](https://ollama.com)してモデルをプル（例：`ollama pull llama3`）
  - **Alibaba Cloud Bailian**：[Bailian コンソール](https://bailian.console.aliyun.com/)から API キーを取得
  - **Volcengine Ark**：[Volcengine コンソール](https://console.volcengine.com/ark)から API キーを取得
  - **DeepSeek**：[DeepSeek プラットフォーム](https://platform.deepseek.com/)から API キーを取得
  - **Zhipu AI**：[Zhipu AI オープンプラットフォーム](https://open.bigmodel.cn/)から API キーを取得
  - **Baidu Qianfan**：[Baidu Qianfan コンソール](https://qianfan.baidubce.com/)から API キーを取得
  - **Tencent Hunyuan**：[Tencent Hunyuan コンソール](https://cloud.tencent.com/product/hunyuan)から API キーを取得
  - **MiniMax**：[MiniMax プラットフォーム](https://platform.minimaxi.com/)から API キーを取得
  - **Moonshot**：[Moonshot プラットフォーム](https://platform.moonshot.cn/)から API キーを取得
  - **SiliconFlow**：[SiliconFlow クラウド](https://cloud.siliconflow.cn/)から API キーを取得

### プロジェクトのビルド

```bash
dotnet restore
dotnet build
```

### システムの実行

#### 方法1：Default バージョンの実行（コンソールアプリケーション）

```bash
dotnet run --project src/SiliconLife.Default
```

アプリケーションは Web サーバーを起動し、自動的にブラウザで Web UI を開きます。

**適用シーン**：
- ✅ データ安全性の要件が極めて高い
- ✅ メモリリソースが制限されている（RAM < 2GB）
- ✅ データ量が少なく、短期利用
- ✅ 開発デバッグ段階

#### 方法2：Fast バージョンの実行（デスクトップアプリケーション）

```bash
dotnet run --project src/SiliconLife.Fast
```

**Windows/macOS**：アプリケーションはウィンドウモードで起動し、システムトレイに最小化されてバックグラウンドで継続実行されます。

**Linux**：アプリケーションはステータスウィンドウを表示し（システムトレイアイコンなし）、自動的にブラウザを開いて Web UI にアクセスします。`--no-tray` パラメータでブラウザの自動起動をスキップすることも可能です：

```bash
dotnet run --project src/SiliconLife.Fast -- --no-tray
```

**適用シーン**：
- ✅ 高同時接続シーン（5ユーザー以上）
- ✅ 大データ量（3ヶ月以上の利用）
- ✅ 低レイテンシレスポンスが必要
- ✅ トレイバックグラウンド実行が必要

### 単一ファイルの公開

```bash
# Windows - Default バージョン
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Windows - Fast バージョン
dotnet publish src/SiliconLife.Fast -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux - Default バージョン
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# Linux - Fast バージョン
dotnet publish src/SiliconLife.Fast -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS - Default バージョン
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true

# macOS - Fast バージョン
dotnet publish src/SiliconLife.Fast -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 開発ロードマップ

### ✅ 完了
- [x] フェーズ1：コンソール AI チャット
- [x] フェーズ2：フレームワークスケルトン（メインループ + ティックオブジェクト + ウォッチドッグ + サーキットブレーカー）
- [x] フェーズ3：ソウルファイルを持つ最初のシリコンビーイング（ボディ・ブレインアーキテクチャ）
- [x] フェーズ4：永続メモリ（チャットシステム + 時間ストレージインターフェース）
- [x] フェーズ5：ツールシステム + エグゼキューター
- [x] フェーズ6：パーミッションシステム（5レベルチェーン、監査ロガー、グローバルACL）
- [x] フェーズ7：動的コンパイル + 自己進化（Roslyn）
- [x] フェーズ8：長期メモリ + タスク + タイマー
- [x] フェーズ9：コアホスト + マルチエージェント協調
- [x] フェーズ10：Web UI（HTTP + SSE、24個のコントローラー、7種類のスキン）
- [x] フェーズ10.5：段階的強化（ブロードキャストチャンネル、トークン監査、32種類のカレンダー、ツール強化、34種類の言語バリアントローカリゼーション）
- [x] フェーズ10.6：改善と最適化（WebView、ヘルプシステム、プロジェクトワークスペース、ナレッジネットワーク、ワークフローエンジン）
- [x] フェーズ11：SpeedyPack ストレージエンジン（LiteDB 置き換え、メモリマップ、非同期ライトキュー、自動コンパクション）
- [x] フェーズ12：プラグインシステム（IPlugin インターフェース、PluginLoader 能力宣言、隔離読み込み、ツール統合）

### 🚧 計画中
- [ ] フェーズ13：外部 IM 統合（Feishu / WhatsApp / Telegram）
- [ ] フェーズ14：スキルエコシステム（プラグインマーケット、スキルパック配布）

## 📚 ドキュメント

- [アーキテクチャ設計](architecture.md) — システム設計、スケジューリング機構、コンポーネントアーキテクチャ
- [セキュリティモデル](security.md) — パーミッションモデル、エグゼキューター、動的コンパイルセキュリティ
- [開発ガイド](development-guide.md) — ツール開発、拡張ガイド
- [API リファレンス](api-reference.md) — Web API エンドポイントドキュメント
- [ツールリファレンス](tools-reference.md) — 組み込みツールの詳細説明
- [Web UI ガイド](web-ui-guide.md) — Web インターフェース使用ガイド
- [シリコンビーイングガイド](silicon-being-guide.md) — エージェント開発ガイド
- [パーミッションシステム](permission-system.md) — パーミッション管理の詳細
- [カレンダーシステム](calendar-system.md) — 32種類のカレンダーシステムの説明
- [クイックスタート](getting-started.md) — 詳細な入門ガイド
- [トラブルシューティング](troubleshooting.md) — よくある問題と回答
- [ロードマップ](roadmap.md) — 完全な開発計画
- [チェンジログ](changelog.md) — バージョン更新履歴
- [コントリビューションガイド](contributing.md) — プロジェクトへの参加方法

## 🤝 コントリビューション

あらゆる形態のコントリビューションを歓迎します！詳細は[コントリビューションガイド](contributing.md)をご覧ください。

### 開発ワークフロー
1. 本リポジトリをフォーク
2. フィーチャーブランチを作成 (`git checkout -b feature/AmazingFeature`)
3. 変更をコミット (`git commit -m 'feat: add some AmazingFeature'`)
4. ブランチにプッシュ (`git push origin feature/AmazingFeature`)
5. Pull Request を提出

## 💡 バージョン選択ガイド

### どのバージョンを使うべきか？

**SiliconLife.Default（デフォルト実装 — アーキテクチャ実現可能性の検証）：**
- 📌 本プロジェクトに初めて触れ、システムアーキテクチャを素早く理解したい
- 📌 開発デバッグ中で、シンプルで直接的な実行方法が必要
- 📌 データセキュリティが最優先事項
- 📌 システムメモリが4GB未満
- 📌 単一ユーザーまたはデータ量が少ない

**SiliconLife.Fast（本番推奨バージョン）：**
- ⚡ 長期安定稼働の本番環境が必要
- ⚡ システムアーキテクチャに習熟し、本格デプロイの準備ができている
- ⚡ マルチユーザーの同時アクセスをサポートする必要がある
- ⚡ システムトレイでのバックグラウンド実行が必要
- ⚡ 極限のパフォーマンス体験を追求

> **総合的な推奨**：SiliconLife.Default はアーキテクチャ検証と入門体験に適しています。実際の本番環境には SiliconLife.Fast の使用を強く推奨します。

### Default から Fast に移行できるか？

**完全に可能です！** 両バージョンは以下を共有しています：
- ✅ 設定ファイルフォーマット（config.json）
- ✅ ツールインターフェース
- ✅ Being 設定
- ✅ Web UI インターフェース

**移行手順：**
1. Default のデータディレクトリをバックアップ
2. 同じデータディレクトリで Fast バージョンを起動
3. Fast が既存データを SpeedyPack ストレージエンジンに自動インポート
4. 機能が正常に動作することを確認後、Fast バージョンを日常使用

### 両バージョンは共存できるか？

**可能です！** 以下のデプロイ戦略を推奨します：

**戦略1：Default で検証、Fast で本番**
```
開発/検証環境：SiliconLife.Default（アーキテクチャ検証、機能デバッグ）
本番環境：SiliconLife.Fast（高性能、バックグラウンド実行、リアルタイムリクエスト処理）
```

**戦略2：Fast をメイン運用、Default で定期バックアップ**
```
SiliconLife.Fast（日常使用、リアルタイムリクエスト処理）
    ↓ 定期バックアップ
SiliconLife.Default（コールドデータアーカイブ、データセキュリティのセーフティネット）
```

## 📄 ライセンス

本プロジェクトは Apache License 2.0 ライセンスの下で公開されています — 詳細は [LICENSE](../../LICENSE) ファイルをご覧ください。

## 👨‍💻 作者

**天源垦骥**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- 码云: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- 哔哩哔哩: [617827040](https://space.bilibili.com/617827040)

## 🙏 謝辞

本プロジェクトに貢献したすべての開発者と AI プラットフォームプロバイダーに感謝します。

---

**Silicon Life Collective** — AI エージェントを本当に「生きる」存在に
