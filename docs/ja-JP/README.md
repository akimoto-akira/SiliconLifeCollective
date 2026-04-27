# Silicon Life Collective

**バージョン: v0.1.0-alpha** | **シリコン生命群** — .NET 9 をベースとしたマルチエージェント協力プラットフォーム。AI エージェントは**シリコン生命体**と呼ばれ、Roslyn 動的コンパイル技術による自己進化を実現します。

[English](../README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | **日本語** | [한국어](../ko-KR/README.md) | [Deutsch](../de-DE/README.md) | [Čeština](../cs-CZ/README.md)

## 🌟 コア機能

### エージェントシステム
- **マルチエージェントオーケストレーション** — *シリコン管理人*によって一元管理され、クロック駆動のタイムスロット公平スケジューリングメカニズムを採用
- **ソウルファイル駆動** — 各シリコン生命体はコアプロンプトファイル（`soul.md`）によって駆動され、独自の個性と行動パターンを定義
- **身体-大脳アーキテクチャ** — *身体*（SiliconBeing）は生命体征を維持しトリガーシナリオを検出；*大脳*（ContextManager）は履歴の読み込み、AI 呼び出し、ツール実行、応答の永続化を担当
- **自己進化能力** — Roslyn 動的コンパイル技術により、シリコン生命体は自分のコードを書き換えて進化できる

### ツールと実行
- **23個の組み込みツール** — カレンダー、チャット、設定、ディスク、ネットワーク、メモリ、タスク、タイマー、ナレッジベース、作業ノート、WebView ブラウザなどを網羅
- **ツール呼び出しサイクル** — AI がツール呼び出しを返す → ツールを実行 → 結果を AI にフィードバック → プレーンテキスト応答を返すまで継続
- **実行器-権限セキュリティ** — すべての I/O 操作は実行器を通じて厳格な権限検証を行う
  - 5段階権限チェーン：IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - 完全な監査ログがすべての権限決定を記録

### AI とナレッジ
- **マルチ AI バックエンドサポート**
  - **Ollama** — ローカルモデルデプロイメント、ネイティブ HTTP API を使用
  - **Alibaba Cloud DashScope（百煉）** — クラウド AI サービス、OpenAI API 互換、13+ モデルをサポート、マルチリージョンデプロイメント
- **32種類のカレンダーシステム** — グローバル主要暦法を完全にカバー。グレゴリオ暦、旧暦、イスラム暦、ヘブライ暦、日本暦、ペルシャ暦、マヤ暦、中国歴史暦など
- **ナレッジネットワークシステム** — 三つ組（主語-関係-目的語）ベースのナレッジグラフ。保存、検索、パス発見をサポート

### Web インターフェース
- **モダン Web UI** — 組み込み HTTP サーバー、SSE リアルタイム更新をサポート
- **4種類のスキンのテーマ** — 管理版、チャット版、創作版、開発版。自動発見と切り替えをサポート
- **20+ のコントローラー** — 完全なシステム管理、チャット、設定、モニタリング機能
- **ゼロフロントエンドフレームワーク依存** — `H`、`CssBuilder`、`JsBuilder` を介してサーバーサイドで HTML/CSS/JS を生成

### 国際化とローカライゼーション
- **21種類の言語バリアント**を完全にサポート
  - 中国語：zh-CN, zh-HK, zh-SG, zh-MO, zh-TW, zhMY（6種類）
  - 英語：en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY（10種類）
  - スペイン語：es-ES, es-MX（2種類）
  - 日本語：ja-JP | 韓国語：ko-KR | チェコ語：cs-CZ

### データとストレージ
- **ゼロデータベース依存** — 純ファイルシステムストレージ（JSON 形式）
- **時間インデックスクエリ** — `ITimeStorage` インターフェースを通じて時間範囲による効率的なクエリをサポート
- **最小依存** — コアライブラリは動的コンパイル用の Microsoft.CodeAnalysis.CSharp のみ依存

## 🛠️ 技術スタック

| コンポーネント | 技術 |
|------|------|
| ランタイム | .NET 9 |
| プログラミング言語 | C# |
| AI 統合 | Ollama（ローカル）、Alibaba Cloud DashScope（クラウド） |
| データストレージ | ファイルシステム（JSON + 時間インデックスディレクトリ） |
| Web サーバー | HttpListener（.NET 組み込み） |
| 動的コンパイル | Roslyn（Microsoft.CodeAnalysis.CSharp 4.13.0） |
| ブラウザ自動化 | Playwright（WebView） |
| ライセンス | Apache-2.0 |

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
│   └── SiliconLife.Default/               # デフォルト実装 + アプリケーションエントリーポイント
│       ├── Program.cs                     # エントリーポイント（すべてのコンポーネントをアセンブル）
│       ├── AI/                            # Ollama クライアント、DashScope クライアント
│       ├── Calendar/                      # 32種類のカレンダー実装
│       ├── Config/                        # デフォルト設定データ
│       ├── Executors/                     # デフォルト実行器実装
│       ├── Help/                          # ヘルプドキュメントシステム
│       ├── IM/                            # WebUI 提供者
│       ├── Knowledge/                     # ナレッジネットワーク実装
│       ├── Localization/                  # 21種類の言語ローカライゼーション
│       ├── Logging/                       # ログ提供者実装
│       ├── Project/                       # プロジェクトシステム実装
│       ├── Runtime/                       # テストクロックオブジェクト
│       ├── Security/                      # デフォルト権限コールバック
│       ├── SiliconBeing/                  # デフォルトシリコン生命体実装
│       ├── Storage/                       # ファイルシステムストレージ実装
│       ├── Tools/                         # 23個の組み込みツール実装
│       ├── WebView/                       # Playwright WebView 実装
│       └── Web/                           # Web UI 実装
│           ├── Controllers/               # 20+ のコントローラー
│           ├── Models/                    # ビューモデル
│           ├── Views/                     # HTML ビュー
│           └── Skins/                     # 4種類のスキンのテーマ
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
  └── クロックオブジェクト（優先度でソート）
       └── シリコン生命体マネージャー
            └── シリコン生命体ランナー（一時スレッド、タイムアウト + サーキットブレーカー）
                 └── シリコン生命体.Tick()
                      └── コンテキストマネージャー.思考()
                           └── AI クライアント.チャット()
                                └── ツール呼び出しサイクル → チャットシステムに永続化
```

### セキュリティアーキテクチャ
すべての AI 发起の I/O 操作は厳格なセキュリティチェーンを通過する必要があります：

```
ツール呼び出し → 実行器 → 権限マネージャー → [IsCurator → 周波数キャッシュ → グローバルACL → コールバック → ユーザーに確認]
```

## 🚀 クイックスタート

### 前提条件

- **.NET 9 SDK** — [ダウンロードリンク](https://dotnet.microsoft.com/download/dotnet/9.0)
- **AI バックエンド**（いずれかを選択）：
  - **Ollama**：[Ollama をインストール](https://ollama.com) しモデルをプル（例：`ollama pull llama3`）
  - **Alibaba Cloud DashScope**：[DashScope コンソール](https://bailian.console.aliyun.com/) から API キーを取得

### プロジェクトのビルド

```bash
dotnet restore
dotnet build
```

### システムの実行

```bash
dotnet run --project src/SiliconLife.Default
```

アプリケーションは Web サーバーを起動し、自動的にブラウザで Web UI を開きます。

### シングルファイルの公開

```bash
# Windows
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
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
- [x] フェーズ 10：Web UI（HTTP + SSE、20+ コントローラー、4種類のスキン）
- [x] フェーズ 10.5：増分強化（ブロードキャストチャンネル、トークン監査、32種類のカレンダー、ツール強化、21言語ローカライゼーション）
- [x] フェーズ 10.6：完成と最適化（WebView、ヘルプシステム、プロジェクトワークスペース、ナレッジネットワーク）

### 🚧 計画中
- [ ] フェーズ 11：外部インスタントメッセージ統合（Feishu / WhatsApp / Telegram）
- [ ] フェーズ 12：プラグインシステムとスキルエコシステム

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

## 📄 ライセンス

このプロジェクトは Apache License 2.0 の下でライセンスされています — 詳細は [LICENSE](../../LICENSE) ファイルをご覧ください。

## 👨‍💻 作者

**Hoshino Kennji**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- 码云: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili: [617827040](https://space.bilibili.com/617827040)

## 🙏 謝辞

このプロジェクトにコントリビューションしてくださったすべての開発者と AI プラットメント提供者に感謝します。

---

**Silicon Life Collective** — AI エージェントを本当に「生きた」存在にする
