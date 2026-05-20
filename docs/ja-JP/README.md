![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**バージョン: v0.2.0-alpha** | **シリコン生命群** — .NET 9 をベースとしたマルチエージェント協力プラットフォーム。AI エージェントは**シリコン生命体**と呼ばれ、Roslyn 動的コンパイル技術による自己進化を実現します。

[English](../README.md) | [Deutsch](../de-DE/README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | **日本語** | [한국어](../ko-KR/README.md) | [Français](../fr-FR/README.md) | [Čeština](../cs-CZ/README.md) | [Italiano](../it-IT/README.md) | [Polski](../pl-PL/README.md) | [Português](../pt-PT/README.md)

## 🌟 コア機能

### エージェントシステム
- **マルチエージェントオーケストレーション** — *シリコン管理人*によって一元管理され、クロック駆動のタイムスロット公平スケジューリングメカニズムを採用
- **ソウルファイル駆動** — 各シリコン生命体はコアプロンプトファイル（`soul.md`）によって駆動され、独自の個性と行動パターンを定義
- **身体-大脳アーキテクチャ** — *身体*（SiliconBeing）は生命体征を維持しトリガーシナリオを検出；*大脳*（ContextManager）は履歴の読み込み、AI 呼び出し、ツール実行、応答の永続化を担当
- **自己進化能力** — Roslyn 動的コンパイル技術により、シリコン生命体は自分のコードを書き換えて進化できる
- **活動状態管理** — Idle（待機）、Working（作業）、Error（エラー）、Stopped（停止）の4つの活動状態をサポート、連続10回のエラーで自動的に Stopped 状態に進入

### プラグインシステム
- **プラグイン拡張アーキテクチャ** — IPlugin インターフェースによる機能拡張を実現、ディレクトリからのプラグイン DLL 動的読み込みをサポート
- **セキュリティサンドボックス** — プラグインローダーが厳格なセキュリティスキャンを実行し、System.IO、System.Net などの名前空間へのアクセスを禁止
- **隔離読み込み** — カスタム AssemblyLoadContext による隔離読み込み、プラグインがメインプログラムの安定性に影響するのを防止
- **ツール統合** — プラグインは ITool インターフェースを通じてカスタムツールを登録し、ツール呼び出しサイクルに自動統合

### ツールと実行
- **24個の組み込みツール** — カレンダー、チャット、設定、ディスク、ネットワーク、メモリ、タスク、タイマー、ナレッジベース、作業ノート、WebView ブラウザ、ホットリロードなどを網羅
- **ホットリロードツール** — SiliconLife.Fast が実行中に自動的にコンパイル、ファイル更新および再起動をサポート、手動介入不要
- **ツール呼び出しサイクル** — AI がツール呼び出しを返す → ツールを実行 → 結果を AI にフィードバック → プレーンテキスト応答を返すまで継続
- **実行器-権限セキュリティ** — すべての I/O 操作は実行器を通じて厳格な権限検証を行う
  - 5段階権限チェーン：IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - 完全な監査ログがすべての権限決定を記録

### AI とナレッジ
- **マルチ AI バックエンドサポート**
  - **Ollama** — ローカルモデルデプロイメント、ネイティブ HTTP API を使用
  - **Alibaba Cloud DashScope（百煉）** — クラウド AI サービス、OpenAI API 互換、13+ モデルをサポート、マルチリージョンデプロイメント
  - **Volcengine Ark（VolcengineArk）** — ByteDance クラウド AI サービス、ストリーミングと非ストリーミングモードをサポート、内蔵速度制御
- **32種類のカレンダーシステム** — グローバル主要暦法を完全にカバー。グレゴリオ暦、旧暦、イスラム暦、ヘブライ暦、日本暦、ペルシャ暦、マヤ暦、中国歴史暦など
- **ナレッジネットワークシステム** — 三つ組（主語-関係-目的語）ベースのナレッジグラフ。保存、検索、パス発見をサポート

### Web インターフェース
- **モダン Web UI** — 組み込み HTTP サーバー、SSE リアルタイム更新をサポート
- **7種類のスキンのテーマ** — 管理版、チャット版、創作版、開発版、ハイコントラスト、ライト、ミニマル。自動発見と切り替えをサポート
- **22 のコントローラー** — 完全なシステム管理、チャット、設定、モニタリング機能
- **ゼロフロントエンドフレームワーク依存** — `H`、`CssBuilder`、`JsBuilder` を介してサーバーサイドで HTML/CSS/JS を生成

### 国際化とローカライゼーション
- **29 種類の言語実装**を完全にサポートし、2 種類の書体システムと複数の地域バリアントをカバー
  - **簡体字中国語**：zh-CN（中国本土）、zh-SG（シンガポール）、zh-MY（マレーシア）（3 種類）
  - **繁体字中国語**：zh-HK（香港）、zh-TW（台湾）、zh-MO（マカオ）（3 種類）
  - **英語**：en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY（10 種類）
  - **スペイン語**：es-ES, es-MX（2 種類）
  - **ドイツ語**：de-DE, de-AT, de-CH, de-LU, de-LI（5 種類）
  - **フランス語**：fr-FR, fr-CA, fr-CH（3 種類）
  - **日本語**：ja-JP | **韓国語**：ko-KR | **チェコ語**：cs-CZ（3 種類）

### データとストレージ
- **SpeedyPack 高性能ストレージ** — Fast バージョンで独自開発 .spk ストレージエンジンを使用、メモリディレクトリマッピング + エントリキャッシュ + 非同期書き込みキュー
- **ファイルシステムストレージ** — Default バージョンで純ファイルシステム JSON ストレージを使用
- **時間インデックスクエリ** — `ITimeStorage` インターフェースを通じて時間範囲による効率的なクエリをサポート
- **自動圧縮** — SpeedyPack は定期的な自動圧縮をサポートし、空き領域を回収
- **最小依存** — コアライブラリは動的コンパイル用の Microsoft.CodeAnalysis.CSharp のみ依存

## 🔄 デュアルバージョンアーキテクチャ

本プロジェクトは、異なるシナリオのニーズに対応する2つの実装バージョンを提供します：

### SiliconLife.Default（デフォルトバージョン）
- **ポジショニング**：デフォルト実装、アーキテクチャの実現可能性検証に主に使用
- **実行モード**：コンソールアプリケーション
- **ストレージ方式**：純ファイルシステム JSON ストレージ
- **適用シナリオ**：データセキュリティ要件が高い、メモリリソースが制限されている、データ量が少ないシナリオ
- **特徴**：シンプルで信頼性が高い、データ永続化が即時、メモリ消失のリスクなし
- **役割説明**：アーキテクチャ検証のベースライン実装として、初めての接触、開発デバッグ、またはデータセキュリティ優先のシナリオに適しています
- **起動コマンド**：`dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast（高性能バージョン）
- **ポジショニング**：主力本番バージョン
- **実行モード**：Windows フォームアプリケーション（システムトレイ対応）
- **ストレージ方式**：SpeedyPack メモリストレージ + 非同期バッチ永続化（.spk ファイル形式）
- **適用シナリオ**：高同時実行、低レイテンシ、大データ量シナリオ
- **特徴**：
  - 極限パフォーマンス最適化
  - トレイバックグラウンド実行、トレイ状態ウィンドウによるリアルタイムモニタリング
  - SpeedyPack エンジン + 自動圧縮でデータセキュリティを保証
  - Component UI アーキテクチャ、30個以上の宣言的コンポーネント
  - 7種類のスキンのテーマ、自動発見と切り替えをサポート
  - ホットリロードツールでオンライン更新および再起動をサポート
- **パフォーマンス向上**：ストレージ読み込みレイテンシ1000倍削減、書き込みレイテンシ15000倍削減、同時実行処理能力50倍向上
- **役割説明**：深い最適化が施された本番グレードの実装であり、長期運用と実際の本番環境の第一選択です
- **起動コマンド**：`dotnet run --project src/SiliconLife.Fast`

### バージョン比較

| 機能 | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| **実行モード** | コンソールアプリケーション | フォームアプリケーション（システムトレイ） |
| **ユーザーインターフェース** | Web UI（ブラウザアクセス） | トレイアイコン + トレイウィンドウ + Web UI |
| **システムトレイ** | ❌ なし | ✅ トレイへの最小化に対応 |
| **バックグラウンド実行** | ❌ コンソールを閉じると終了 | ✅ トレイで継続的にバックグラウンド実行 |
| **ストレージ方式** | ファイルシステム JSON ストレージ | SpeedyPack メモリストレージ + 非同期永続化 |
| **ストレージエンジン** | ファイルシステム I/O | SiliconLife.Speedy（.spk 形式） |
| **読み込みレイテンシ** | ~10ms（ディスク I/O） | ~0.01ms（メモリ操作） |
| **書き込みレイテンシ** | ~15ms（同期書き込み） | ~0.001ms（非同期書き込み） |
| **同時実行能力** | ~100 req/s | ~5000 req/s |
| **メモリ使用量** | ~200MB | ~500MB |
| **データセキュリティ** | 極めて高い（即時永続化） | 高い（非同期永続化 + 自動圧縮） |
| **適用シナリオ** | データセキュリティ優先、小データ量 | パフォーマンス優先、大データ量、高同時実行 |

## 🛠️ 技術スタック

| コンポーネント | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| ランタイム | .NET 9 | .NET 9（Windows/macOS/Linux） |
| プログラミング言語 | C# | C# |
| アプリケーションタイプ | コンソールアプリケーション | デスクトップアプリケーション（Windows/macOS システムトレイ / Linux ステータスウィンドウ） |
| AI 統合 | Ollama（ローカル）、Alibaba Cloud DashScope（クラウド） | Ollama（ローカル）、Alibaba Cloud DashScope（クラウド）、Volcengine Ark（クラウド） |
| データストレージ | ファイルシステム（JSON + 時間インデックスディレクトリ） | SpeedyPack（.spk 形式、メモリマッピング + 非同期永続化） |
| Web サーバー | HttpListener（.NET 組み込み） | HttpListener（.NET 組み込み） |
| 動的コンパイル | Roslyn（Microsoft.CodeAnalysis.CSharp 4.13.0） | Roslyn（Microsoft.CodeAnalysis.CSharp 4.13.0） |
| ブラウザ自動化 | Playwright（WebView） | Playwright（WebView） |
| プラグインシステム | ✅ サポート（IPlugin + PluginLoader） | ✅ サポート（IPlugin + PluginLoader） |
| システムトレイ | ❌ サポートなし | ✅ Windows/macOS サポート（NotifyIcon）；Linux トレイアイコンなし |
| ライセンス | Apache-2.0 | Apache-2.0 |

## 📁 プロジェクト構造

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # コアライブラリ（インターフェース、抽象クラス）
│   │   ├── AI/                            # AI クライアントインターフェース、コンテキストマネージャー、メッセージモデル
│   │   ├── Audit/                         # トークン使用監査システム
│   │   ├── Chat/                          # チャットシステム、セッション管理、ブロードキャストチャンネル
│   │   ├── Compilation/                   # 動的コンパイル、セキュリティスキャン、コード暗号化
│   │   ├── Config/                        # 設定管理システム
│   │   ├── Executors/                     # 実行器（ディスク、ネットワーク、コマンドライン）
│   │   ├── IM/                            # インスタントメッセージ提供者インターフェース
│   │   ├── Knowledge/                     # ナレッジネットワークシステム
│   │   ├── Localization/                  # ローカライゼーションシステム
│   │   ├── Logging/                       # ログシステム
│   │   ├── Plugins/                       # プラグインシステム（IPlugin インターフェース、PluginLoader ローダー）
│   │   ├── Project/                       # プロジェクト管理システム
│   │   ├── Runtime/                       # メインループ、クロックオブジェクト、コアホスト
│   │   ├── Security/                      # 権限管理システム
│   │   ├── SiliconBeing/                  # シリコン生命体ベースクラス、マネージャー、ファクトリー
│   │   ├── Storage/                       # ストレージインターフェース
│   │   ├── Time/                          # 不完全な日付（時間範囲クエリ）
│   │   ├── Tools/                         # ツールインターフェースとツールマネージャー
│   │   ├── WebView/                       # WebView ブラウザインターフェース
│   │   └── ServiceLocator.cs              # グローバルサービスロケーター
│   │
│   ├── SiliconLife.Common/                # 共有実装（2つのバージョンで共用）
│   │   ├── AI/                            # AI クライアントとファクトリー（Ollama、DashScope、VolcengineArk）
│   │   ├── Calendar/                      # 32種類のカレンダー実装
│   │   ├── Localization/                  # ローカライゼーションベースクラスと29種類の言語/地域バリアント実装
│   │   ├── Resources/                     # 共有リソースファイル
│   │   ├── Security/                      # 権限マネージャー
│   │   ├── SiliconBeing/                  # デフォルトシリコン生命体実装
│   │   ├── Tools/                         # 23個の共通ツール実装（ホットリロードツール含む）
│   │   ├── Web/                           # Web 基盤
│   │   └── WebView/                       # Playwright WebView 実装
│   │
│   ├── SiliconLife.App/                   # アプリケーション層（Web UI + ヘルプドキュメント、Default と Fast で共用）
│   │   ├── Config/                        # アプリケーション設定
│   │   ├── Data/                          # データディレクトリ
│   │   ├── Help/                          # ヘルプドキュメントローカライゼーション（多言語）
│   │   └── Web/                           # Web UI 実装
│   │       ├── Component/                 # UI コンポーネントライブラリ（30+ コンポーネント）
│   │       ├── Controllers/               # 22 のコントローラー
│   │       ├── Models/                    # ビューモデル
│   │       ├── Views/                     # HTML ビュー
│   │       └── Skins/                     # 7 種類のスキンのテーマ
│   │
│   ├── SiliconLife.Default/               # デフォルト実装 + アプリケーションエントリーポイント（コンソール版）
│   │   ├── Program.cs                     # エントリーポイント（すべてのコンポーネントをアセンブル）
│   │   ├── Config/                        # デフォルト設定データ
│   │   ├── IM/                            # WebUI 提供者
│   │   ├── Knowledge/                     # ナレッジネットワーク実装
│   │   ├── Logging/                       # ログ提供者実装
│   │   ├── Project/                       # プロジェクトシステム実装
│   │   ├── Security/                      # デフォルト権限コールバック
│   │   ├── Storage/                       # ファイルシステムストレージ実装
│   │   └── Tools/                         # バージョン固有のツール実装（HelpTool）
│   │
│   ├── SiliconLife.Fast/                  # 高性能実装 + アプリケーションエントリーポイント（フォーム版）
│   │   ├── Program.cs                     # エントリーポイント（フォームアプリケーション）
│   │   ├── Config/                        # 設定データ（Default と共有）
│   │   ├── IM/                            # WebUI 提供者
│   │   ├── Knowledge/                     # ナレッジネットワーク実装（メモリ最適化）
│   │   ├── Logging/                       # 高性能ログ提供者
│   │   ├── Project/                       # プロジェクトシステム実装
│   │   ├── Security/                      # 最適化権限コールバック
│   │   ├── Storage/                       # SpeedyPack ストレージアダプター
│   │   ├── Tools/                         # バージョン固有のツール実装（HelpTool）
│   │   └── Tray/                          # システムトレイ（29種類の言語ローカライゼーション）
│   │
│   ├── SiliconLife.Speedy/                # SpeedyPack 高性能ストレージエンジン
│   │   ├── SpeedyPack.cs                  # コアクラス（メモリディレクトリマッピング + キャッシュ + 非同期書き込み）
│   │   ├── SpeedyPackOptions.cs           # 設定オプション（キャッシュ TTL、最大エントリ数など）
│   │   ├── IPackTransaction.cs            # トランザクションインターフェース
│   │   ├── SpkFileInfo.cs                 # ファイル情報
│   │   └── Internal/                      # 内部実装
│       │   ├── DirectoryMap.cs            # メモリディレクトリマッピング
│       │   ├── EntryCache.cs              # エントリキャッシュ
│       │   ├── FreeList.cs                # 空き領域管理
│       │   ├── PackFileReader.cs          # パッケージファイルリーダー
│       │   ├── PackFileWriter.cs          # パッケージファイルライター
│       │   ├── WriteQueue.cs              # 非同期書き込みキュー
│       │   ├── WriteOperation.cs          # 書き込み操作
│       │   ├── SpeedyTransaction.cs       # トランザクション実装
│       │   ├── SpkHeader.cs              # パッケージファイルヘッダー
│       │   └── PathNormalizer.cs          # パス正規化
│   │
│   └── SiliconLife.Speedy.Manager/        # SpeedyPack 管理ツール（Windows Forms）
│       ├── MainForm.cs                    # メインフォーム
│       ├── Program.cs                     # エントリーポイント
│       └── slc.ico                        # アプリケーションアイコン
│
├── docs/                                  # 多言語ドキュメント
│   ├── zh-CN/                             # 簡体字中国語ドキュメント
│   ├── en/                                # 英語ドキュメント
│   └── ...                                # その他の言語ドキュメント
```

## 🏗️ アーキテクチャ概要

### スケジューリングアーキテクチャ
```
メインループ（専用スレッド、ウォッチドッグ + サーキットブレーカー）
  └── クロックオブジェクト（優先度でソート）
       └── シリコン生命体マネージャー
            └── シリコン生命体ランナー（一時スレッド、タイムアウト + サーキットブレーカー）
                 └── シリコン生命体.Tick()
                      └── コンテキストマネージャー.思考()
                           └── AI クライアント.チャット()
                                └── ツール呼び出しサイクル → チャットシステムに永続化
```

### セキュリティアーキテクチャ
すべての AI 発起の I/O 操作は厳格なセキュリティチェーンを通過する必要があります：

```
ツール呼び出し → 実行器 → 権限マネージャー → [IsCurator → 周波数キャッシュ → グローバルACL → コールバック → ユーザーに確認]
```

## 🚀 クイックスタート

### 前提条件

- **.NET 9 SDK** — [ダウンロードリンク](https://dotnet.microsoft.com/download/dotnet/9.0)
- **AI バックエンド**（いずれかを選択）：
  - **Ollama**：[Ollama をインストール](https://ollama.com) しモデルをプル（例：`ollama pull llama3`）
  - **Alibaba Cloud DashScope**：[DashScope コンソール](https://bailian.console.aliyun.com/) から API キーを取得
  - **Volcengine Ark**：[Volcengine コンソール](https://console.volcengine.com/ark) から API キーを取得

### プロジェクトのビルド

```bash
dotnet restore
dotnet build
```

### システムの実行

#### 方法 1：Default バージョンを実行（コンソールアプリケーション）

```bash
dotnet run --project src/SiliconLife.Default
```

アプリケーションは Web サーバーを起動し、自動的にブラウザで Web UI を開きます。

**適用シナリオ**：
- ✅ データセキュリティ要件が極めて高い
- ✅ メモリリソースが制限されている（RAM < 2GB）
- ✅ データ量が少なく、短期間の使用
- ✅ 開発デバッグ段階

#### 方法 2：Fast バージョンを実行（Windows フォームアプリケーション）

```bash
dotnet run --project src/SiliconLife.Fast
```

アプリケーションはフォームモードで起動し、システムトレイに最小化され、バックグラウンドで継続的に実行されます。

**適用シナリオ**：
- ✅ 高同時実行シナリオ（> 5 ユーザー）
- ✅ 大データ量（3ヶ月以上の使用）
- ✅ 低レイテンシ応答が必要
- ✅ トレイバックグラウンド実行が必要

### シングルファイルの公開

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

### ✅ 完了済み
- [x] フェーズ 1：コンソール AI チャット
- [x] フェーズ 2：フレームワークスケルトン（メインループ + クロックオブジェクト + ウォッチドッグ + サーキットブレーカー）
- [x] フェーズ 3：最初のソウルファイル付きシリコン生命体（身体-大脳アーキテクチャ）
- [x] フェーズ 4：永続メモリ（チャットシステム + 時間ストレージインターフェース）
- [x] フェーズ 5：ツールシステム + 実行器
- [x] フェーズ 6：権限システム（5段階チェーン、監査ロガー、グローバルアクセスコントロールリスト）
- [x] フェーズ 7：動的コンパイル + 自己進化（Roslyn）
- [x] フェーズ 8：長期メモリ + タスク + タイマー
- [x] フェーズ 9：コアホスト + マルチエージェント協力
- [x] フェーズ 10：Web UI（HTTP + SSE、20+ コントローラー、7種類のスキン）
- [x] フェーズ 10.5：増分強化（ブロードキャストチャンネル、トークン監査、32種類のカレンダー、ツール強化、29言語ローカライゼーション）
- [x] フェーズ 10.6：完成と最適化（WebView、ヘルプシステム、プロジェクトワークスペース、ナレッジネットワーク）
- [x] フェーズ 11：SpeedyPack ストレージエンジン（LiteDB 置き換え、メモリマッピング、非同期書き込みキュー、自動圧縮）
- [x] フェーズ 12：プラグインシステム（IPlugin インターフェース、PluginLoader セキュリティサンドボックス、隔離読み込み、ツール統合）

### 🚧 計画中
- [ ] フェーズ 13：外部インスタントメッセージ統合（Feishu / WhatsApp / Telegram）
- [ ] フェーズ 14：スキルエコシステム（プラグインマーケット、スキルパッケージ配布）

## 📚 ドキュメント

- [アーキテクチャ設計](architecture.md) — システム設計、スケジューリングメカニズム、コンポーネントアーキテクチャ
- [セキュリティモデル](security.md) — 権限モデル、実行器、動的コンパイルセキュリティ
- [開発ガイド](development-guide.md) — ツール開発、拡張ガイド
- [API リファレンス](api-reference.md) — Web API エンドポイントドキュメント
- [ツールリファレンス](tools-reference.md) — 組み込みツールの詳細説明
- [Web UI ガイド](web-ui-guide.md) — Web インターフェース使用ガイド
- [シリコン生命体ガイド](silicon-being-guide.md) — エージェント開発ガイド
- [権限システム](permission-system.md) — 権限管理詳細
- [カレンダーシステム](calendar-system.md) — 32種類のカレンダーシステム説明
- [クイックスタート](getting-started.md) — 詳細な入門ガイド
- [トラブルシューティング](troubleshooting.md) — よくある質問
- [ロードマップ](roadmap.md) — 完全な開発計画
- [変更ログ](changelog.md) — バージョン更新履歴
- [コントリビューションガイド](contributing.md) — プロジェクトへの参加方法

## 🤝 コントリビューション

あらゆる形式のコントリビューションを歓迎します！詳細は[コントリビューションガイド](contributing.md)をご覧ください。

### 開発ワークフロー
1. このリポジトリをフォーク
2. 機能ブランチを作成（`git checkout -b feature/AmazingFeature`）
3. 変更をコミット（`git commit -m 'feat: add some AmazingFeature'`）
4. ブランチにプッシュ（`git push origin feature/AmazingFeature`）
5. プルリクエストを提出

## � バージョン選択ガイド

### どちらのバージョンを使用すべきか？

**SiliconLife.Default（デフォルト実装 — アーキテクチャ実現可能性検証）：**
- 📌 初めてこのプロジェクトに触れ、システムアーキテクチャを素早く理解したい場合
- 📌 開発デバッグ中で、シンプルで直接的な実行方法が必要な場合

**SiliconLife.Fast（高性能バージョン — 本番環境推奨）：**
- 📌 長期運用や本番環境でのデプロイが必要な場合
- 📌 高パフォーマンスと低レイテンシが求められる場合

## �📄 ライセンス

このプロジェクトは Apache License 2.0 の下でライセンスされています — 詳細は [LICENSE](../../LICENSE) ファイルをご覧ください。

## 👨‍💻 作者

**天源墾驥**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- 码云: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili: [617827040](https://space.bilibili.com/617827040)

## 🙏 謝辞

このプロジェクトにコントリビューションしてくださったすべての開発者と AI プラットフォーム提供者に感謝します。

---

**Silicon Life Collective** — AI エージェントを本当に「生きた」存在にする
