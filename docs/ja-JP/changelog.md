# 変更ログ

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | **日本語** | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md) | [Русский](../ru-RU/changelog.md)

このプロジェクトのすべての重要な変更は、このファイルに記録されます。

形式は [Keep a Changelog](https://keepachangelog.com/en/1.0.0/) に基づき、
このプロジェクトは [セマンティックバージョニング](https://semver.org/spec/v2.0.0.html) に従います。

---

## この変更ログについて

### プロジェクトのデュアルバージョン

このプロジェクトは2つの実装バージョンを提供します：

- **SiliconLife.Default**：デフォルト実装、主にアーキテクチャの実現可能性を検証するために使用。コンソールアプリケーション、ファイルシステムJSONストレージ。
- **SiliconLife.Fast**：本番環境向けの主推バージョン。クロスプラットフォームデスクトップアプリケーション（Windows / macOS / Linux）、SpeedyPack メモリストレージ + 非同期永続化、深度なパフォーマンス最適化が施されています。

両バージョンは同じインターフェースと機能を共有し、ストレージ実装と実行モードのみが異なります。SiliconLife.Default はアーキテクチャ検証の基準として、SiliconLife.Fast は本番環境の主推バージョンとして機能します。

### プロジェクトの起源

- このプロジェクトは 2026 年 3 月 20 日に開始しました。
- このプロジェクト以前に、アーキテクチャ設計が不合理で失敗した検証デモがあり、複数の AI プラットフォームと統合できませんでした。

### 使用した AI IDE ツール

#### Kiro（Amazon AWS）
- プロジェクトは当初 Kiro によって保守され、Spec モードで開始されました。
- Kiro は Amazon AWS が構築した agentic AI 開発環境です。
- Code OSS（VS Code）ベースで、VS Code 設定と Open VSX 互換プラグインをサポート。
- 構造化 AI コーディングのためのスペック駆動開発ワークフローを備えています。

#### Comate AI IDE / 文心快码（百度）
- 文章やドキュメント作業に時折使用。
- Comate AI IDE は百度文心が 2025 年 6 月 23 日にリリースした AI ネイティブ開発環境ツール。
- 業界初のマルチモーダル、マルチエージェント協調 AI IDE。
- デザインからコードへの変換や全プロセス AI 支援コーディングなどの機能を備えています。
- 百度文心 4.0 X1 Turbo モデルによって駆動。

#### Trae（字節跳動）
- 2025 年 10 月から 2026 年 4 月まで使用。
- AI IDE、インテリジェントなコード生成とプロジェクト管理をサポート。

#### Qoder（阿里巴巴）
- 2026 年 4 月 18 日よりプロジェクト保守に使用。
- AI コーディングプラットフォーム、コード分析、ドキュメント生成、マルチエージェント協調をサポート。

#### CatPaw（美団）
- 2026 年 5 月 6 日より Qoder と併用。
- 美団独自開発の LongCat シリーズモデルベース、強力な全コードアーキテクチャリファクタリング能力を備えています。

#### DuMate（Baidu Qianfan）
- 2026年7月よりコード開発、ローカライズ、ドキュメント作成に使用。
- 千帆デスクトッププラットフォームで動作する汎用 AI アシスタント。マルチツールオーケストレーション、ファイル操作、ブラウザ自動化、マルチステップタスク実行が可能。
- ユーザーの Windows デスクトップ上でローカルファイルの読み書き、シェルコマンドの実行、ウェブ検索を直接行う。

### 要件ドキュメント

- このプロジェクトの要件ドキュメントは非公開です。
- 要件は 12 以上の国際 AI プラットフォームと大規模モデルシリーズで繰り返し検証され、2000 行を超える、人間にはほぼ理解不可能なユーザーストーリー駆動要件ドキュメントが生成されました。

---

## [未リリース]

### 2026-07-15

#### 新機能
- 7つの新しい AI クライアントを実装、国内主要 AI プラットフォームへの直接接続を完了
  - **DeepSeekClient** — `https://api.deepseek.com` エンドポイント、思考モード（thinking）対応、1M コンテキストウィンドウ
  - **ZhipuClient（GLM）** — `https://open.bigmodel.cn/api/paas/v4` エンドポイント、思考モード対応、モデル別ビジョン対応、1M コンテキストウィンドウ
  - **ErnieClient（百度千帆）** — `https://qianfan.baidubce.com/v2` エンドポイント、131K コンテキストウィンドウ、モデル別ビジョン対応
  - **HunyuanClient（騰訊混元）** — TokenHub + レガシーのデュアルエンドポイント自動選択、262K コンテキストウィンドウ、hy3/hy3-preview モデル対応
  - **MiniMaxClient** — 国内 `https://api.minimaxi.com/v1` / 国際 `https://api.minimax.io/v1` エンドポイント、1M コンテキストウィンドウ
  - **MoonshotClient（Kimi）** — `https://api.moonshot.cn/v1` エンドポイント、262K コンテキストウィンドウ
  - **SiliconFlowClient（硅基流動）** — `https://api.siliconflow.cn/v1` エンドポイント、動的モデルリスト取得、1M コンテキストウィンドウ

#### 変更
- 零一万物（Yi/Lingyiwanwu）を⚠️非推奨に変更（新規ユーザー登録停止のため）
- AI プラットフォーム対応一覧の7プラットフォームステータスを 📋→✅ に更新（百度千帆、智谱AI、月之暗面、DeepSeek、騰訊混元、硅基流動、MiniMax）

### 2026-05-26

#### 新機能
- `a49041b` - ロシア語(ru-RU)ローカリゼーションサポートを追加 (ref task-364)
  - 216 ファイル変更

#### 修正
- `79096f2` - glossary テーブル形式を標準 Markdown に変更、余分なスペース整列を削除
  - 1 ファイル変更

#### ドキュメント
- `174a954` - glossary に欠落していた Deutsch/Polski/Português 3列の用語翻訳を補完
  - 1 ファイル変更

#### 協力フレームワーク
- `5b03d53` - .ai-collab タスク記録を更新 - task-364 ロシア語ローカリゼーション (ref task-364)
  - 5 ファイル変更

- `018947d` - 2026-05-25 の sessions と changes をアーカイブ
  - 2 ファイル変更

### 2026-05-25

#### 新機能
- `14721a9` - ThinkOnProject 人員配置プロンプトを詳細な実行可能アクションプランに細分化 (ref task-363)
  - 20 ファイル変更

#### 修正
- `abb4285` - beingsHtml の .join() 呼び出し位置エラーを修正 (ref task-361)
  - 1 ファイル変更

- `1c0b9ed` - WorkflowDetailView の states-overview レンダリングによる state-initial 重複文字列バグを削除 (ref task-362)
  - 6 ファイル変更

#### 協力フレームワーク
- `ecc48a1` - .ai-collab メタデータを更新（relatedCommit および activity log） (ref task-361)
  - 4 ファイル変更

- `64529a7` - 2026-05-24 の sessions と changes をアーカイブ（手動補完実行）
  - 28 ファイル変更

- `4150e52` - 完了タスク task-341~361 をアーカイブ (ref archive)
  - 2 ファイル変更

### 2026-05-24

#### 新機能
- `db60fd9` - ツール権限リストに ToolAction 宣言のないツールを表示し、設定不可と注記 (ref task-331, task-332, task-333)
  - 21 ファイル変更

- `6004a7f` - WorkflowTemplate にロール定義サポートを追加 + 12言語ローカリゼーション + DiskTool 修正 (ref task-346)
  - 24 ファイル変更

- `75ce452` - ProjectSpace ロールプールと ProjectTool ロール管理アクション (ref task-347)
  - 12 ファイル変更

- `edfb600` - BuildProjectScenarioContext にロール情報を追加 (ref task-348)
  - 21 ファイル変更

- `6a2d713` - HasProjectsWithoutTemplate を HasProjectsNeedingAttention に拡張 (ref task-349)
  - 21 ファイル変更

- `a773224` - ワークフロータスク作成時にロールプールから実行者を割り当てるよう変更 (ref task-350)
  - 6 ファイル変更

- `77a27f9` - TravelCodeWikiTool を地理エンティティエントリとして拡張 (ref task-353)
  - 8 ファイル変更

- `873ef23` - GeoDataTool 実装完了、.ai-collab ステータス更新 (ref task-352)
  - 7 ファイル変更

- `feaccab` - GeoContentTool 実装完了、.ai-collab ステータス更新 (ref task-351)
  - 6 ファイル変更

- `6e60ad1` - GeoLanguageTool を拡張（ObjectPath サポート + set_word）、メタデータをバックフィル (ref task-356, task-355)
  - 7 ファイル変更

- `4eff807` - 各 GeoLocation サブクラスで GetWikiDocuments() を実装 (ref task-357)
  - 5 ファイル変更

- `baad5df` - MediaWiki API パブリッシュサービスを実装 (ref task-358)
  - 6 ファイル変更

- `b846a21` - ワークフロー詳細ページを実装 (ref task-361)
  - 24 ファイル変更

#### 修正
- `a290088` - CuratorTool で新規作成したシリコンビーイングが再起動後に消失する問題 (ref task-334)
  - 11 ファイル変更

- `69a8cba` - タスクページが beingId でフィルタリングされないバグを修正 (ref task-360)
  - 8 ファイル変更

- `7dd1a65` - Router.cs にワークフロー詳細ページルートを登録 (ref task-361)
  - 1 ファイル変更

#### リファクタリング
- `5e02711` - 共通層ストレージパス抽象化をリファクタリング、ファイルシステムハードコードを排除 (ref task-335)
  - 12 ファイル変更

- `0ec0929` - DynamicBeingLoader.SaveBeingCode が直接ファイルシステム操作の代わりに IStorage を使用 (ref task-336)
  - 7 ファイル変更

- `9a44b48` - PlaywrightWebView IStorage ブリッジ + WebViewBrowserTool 基底クラス分離 (ref task-337, task-340)
  - 11 ファイル変更

- `8fea742` - WebViewBrowserTool スクリーンショット保存が直接ファイルシステム操作の代わりに IStorage を使用 (ref task-338)
  - 6 ファイル変更

- `4c24e6d` - DefaultPermissionCallback がハードコードパスの代わりに BeingPathResolver を使用 (ref task-339)
  - 6 ファイル変更

- `ab428cd` - DefaultSiliconBeing のダウンキャストを削除、基底クラスの SaveState() を直接呼び出し (ref task-344)
  - 7 ファイル変更

- `1e6eb80` - PlaywrightWebView ブラウザ状態一時ファイルブリッジを IStorage 直接読み書きに変更 (ref task-341)
  - 7 ファイル変更

- `17f00e9` - DiskTool 検索操作を DiskExecutor 経由に変更 (ref task-342)
  - 8 ファイル変更

- `8158703` - ChatController 添付チェックを DiskExecutor 経由に変更 (ref task-343)
  - 7 ファイル変更

- `3243ae6` - TravelCodeWikiPublishWorkflow を7ステートマシンに書き直し、強制追跡の TravelCodeWikiWithAI ファイルを削除 (ref task-355)
  - 6 ファイル変更

#### クリーンアップ
- `d685288` - HotReloadTool.cs と tools/HotReload ディレクトリを削除 (ref task-345)
  - 8 ファイル変更

#### ドキュメント
- `f1789d1` - README.md の説明行を最適化 (ref task-359)
  - 9 ファイル変更

#### 協力フレームワーク
- `982c6bb` - .ai-collab に欠落していた relatedCommit と commitHash フィールドを補完
  - 6 ファイル変更

- `d91e9f8` - task-331~340 をアーカイブ、タスクボードをクリア
  - 2 ファイル変更

- `9135e30` - task-341~344 共通層 IStorage リファクタリング + 抽象修正をリリース
  - 1 ファイル変更

- `f70b350` - TravelCodeWikiWithAI アーキテクチャ改造 13項目タスクを追加 (ref task-346~358)
  - 2 ファイル変更

- `f81d38b` - ai-collab session と task tracking ファイルを更新
  - 3 ファイル変更

### 2026-05-23

#### 修正
- `9c3c64e` - ExecuteTool 実行時権限検証がプロジェクトレベル制限をバイパスする問題を修正 (ref task-324)
  - 7 ファイル変更

- `94a9e35` - 権限テンプレート定義と ToolActionAttribute 宣言の不一致を修正 (ref task-325)
  - 6 ファイル変更

- `e8d8371` - すべての Action が無効なツールを AI リクエストから全体除去 (ref task-326)
  - 6 ファイル変更

- `32c7d8a` - ツール権限 API に Action 名検証を追加 + チャット履歴 Markdown レンダリング修正 (ref task-327, task-328, task-329)
  - 9 ファイル変更

- `797db8c` - Markdown レンダリング fallback が誤って mdRendered を設定し、marked ロード後に再レンダリングされない問題 (ref task-330)
  - 9 ファイル変更

#### 協力フレームワーク
- `1496094` - task-324~327 ツール認可フレームワーク修正タスクをリリース
  - 776 ファイル変更

- `0d16e63` - 協力タスクステータスを更新、task-330 をコミット 797db8c に関連付け、アーカイブの準備
  - 2 ファイル変更

- `e602e1c` - task-316~330 をアーカイブ、タスクボードをクリア (ref task-316~330)
  - 2 ファイル変更

- `20291ce` - 日単位で sessions と changes をアーカイブ（5月13-22日）
  - 106 ファイル変更

### 2026-05-22

#### ドキュメント整合性修正
- `9e07b27` - フランス語ドキュメント(fr-FR)とソースコードの整合性差異を修正 (ref task-307)
  - 10 ファイル変更

- `9e3be72` - ドイツ語ドキュメント(de-DE)とソースコードの整合性を修正 (ref task-308)
  - 5 ファイル変更

- `2bc7151` - スペイン語(es-ES)ドキュメントとソースコードの整合性差異を修正 (ref task-309)
  - 13 ファイル変更

- `f95088e` - イタリア語ドキュメント(it-IT)とソースコードの整合性を修正 (ref task-310)
  - 11 ファイル変更

- `6ea9f4a` - ポーランド語ドキュメント(pl-PL)とソースコードの整合性を修正 (ref task-311)
  - 16 ファイル変更

- `7646923` - ポルトガル語ドキュメント(pt-PT)とソースコードの整合性を修正 (ref task-312)
  - 12 ファイル変更

- `7eaf9db` - チェコ語ドキュメント(cs-CZ)とソースコードの整合性を修正 (ref task-313)
  - 12 ファイル変更

#### 協力フレームワーク
- `3cb7347` - task-313 の relatedCommit=7eaf9db を更新
  - 1 ファイル変更

### 2026-05-21

#### 新機能
- `99eca78` - 右クリックメニューに「ストレージを表示（読み取り専用）」機能を追加、プロセス内で Speedy.Manager を呼び出し (ref task-301)
  - 26 ファイル変更

#### ドキュメント整合性修正
- `7f65cf1` - zh-CN ドキュメントとソースコードの整合性差異を修正 (ref task-303)
  - 15 ファイル変更

- `a9e2a2c` - 英語(en)ドキュメントとソースコードの整合性差異を修正 (ref task-302)
  - 9 ファイル変更

- `2549105` - 繁體中文(zh-HK)ドキュメントとソースコードの整合性差異を修正 (ref task-304)
  - 12 ファイル変更

- `277eb50` - 日本語ドキュメントとソースコードの整合性差異を修正 (ref task-305)
  - 10 ファイル変更

- `edce413` - 韓国語(ko-KR)ドキュメントとソースコードの整合性差異を修正 (ref task-306)
  - 18 ファイル変更

- `f2adcae` - ポルトガル語ドキュメントとソースコードの不一致問題を修正 (ref task-220)
  - 15 ファイル変更

- `3332987` - 繁体字中国語（香港）ドキュメントとソースコードの不一致問題を修正 (ref task-218)
  - 14 ファイル変更

- `af9f715` - ポーランド語ドキュメントとソースコードの不一致問題を修正 (ref task-217)
  - 15 ファイル変更

- `2e2b18b` - 韓国語ドキュメントとソースコードの不一致問題を修正 (ref task-216)
  - 16 ファイル変更

- `626ebc9` - 日本語ドキュメントとソースコードの不一致問題を修正 (ref task-215)
  - 19 ファイル変更

- `48d061b` - イタリア語ドキュメントとソースコードの不一致問題を修正 (ref task-214)
  - 14 ファイル変更

#### 協力フレームワーク
- `6683bee` - Marvis AI チームを登録、タスクステータスを更新
  - 3 ファイル変更

- `03fc905` - task-210~220 をアーカイブ
  - 5 ファイル変更

### 2026-05-20

#### 新機能
- `65176d4` - ポルトガル語（pt-PT + pt-BR）完全ローカリゼーションサポートを追加 (ref task-208)
  - 41 ファイル変更

#### ドキュメント整合性修正
- `af4dffd` - zh-CN ドキュメントとソースコードのすべての不一致問題を修正 (ref task-209)
  - 11 ファイル変更

- `144b945` - 英語(en)とチェコ語(cs-CZ)ドキュメントとソースコードの不一致問題を修正 (ref task-219, task-210)
  - 22 ファイル変更

- `08bec55` - ドイツ語ドキュメント(de-DE)とソースコードの不一致問題を修正 (ref task-211)
  - 14 ファイル変更

- `7ff28de` - スペイン語(es-ES)ドキュメントとソースコードの不一致問題を修正 (ref task-212)
  - 14 ファイル変更

- `15e2133` - フランス語ドキュメント(fr-FR)とソースコードの不一致問題を修正 (ref task-213)
  - 13 ファイル変更

#### 修正
- `7dac388` - プロジェクトタスクリストが表示できない問題を修正 (ref task-207)
  - 6 ファイル変更

#### 協力フレームワーク
- `7890223` - task-201~209 をアーカイブ、task-210~220 ドキュメント整合性修正タスクをリリース
  - 5 ファイル変更

### 2026-05-19

#### 新機能
- `cd72846` - PluginLoader セキュリティスキャンバイパスの安全な代替案を実装 (ref task-203)
  - 13 ファイル変更

- `fc0c00c` - Speedy.Manager 機能強化 - 新規作成/インポート/エクスポート/TreeView 階層/プログレスウィンドウ (ref task-206)
  - 9 ファイル変更

#### 修正
- `ec07118` - ITypeRegistry/IObjectFactory がプラグインロード前に登録されていない問題を修正 (ref task-205)
  - 8 ファイル変更

- `9e749db` - プロジェクト作成時の Creator ID is required エラーを修正 (ref task-204)
  - 4 ファイル変更

#### インフラストラクチャ
- `43dc092` - CLDR マイグレーション - CldrDataProvider を追加、.github を削除
  - 1 ファイル変更

- `c09ec1f` - cldr/ を .gitignore に追加
  - 1 ファイル変更

- `221f818` - GitHub 同期を Gitee プッシュミラー方式に変更、workflow は手動予備のみ保持
  - 1 ファイル変更

- `08cdf1a` - GitHub 同期 workflow を修正 - リトライロジックと変更なしスキップを追加
  - 1 ファイル変更

- `fb4e77d` - SiliconLife.Speedy.Manager.csproj を更新
  - 1 ファイル変更

#### 協力フレームワーク
- `df90af0` - task-203 の relatedCommit=cd72846 を更新
  - 1 ファイル変更

### 2026-05-18

#### リファクタリング
- `e720d06` - Speedy.Manager を WinForms から完全に Avalonia に改造 (ref task-202)
  - 17 ファイル変更

#### 修正
- `08894a9` - メモリタイムラインサマリーエントリの階層表示エラーを修正 (ref task-201)
  - 3 ファイル変更

#### 協力フレームワーク
- `2871afb` - 全タスクをアーカイブ、tasks.json をクリア
  - 2 ファイル変更

### 2026-05-17

#### 新機能
- `d6eb994` - プロジェクトリストページにプロジェクト作成エントリとワークフローテンプレート選択を追加 (ref task-203)
  - 14 ファイル変更

- `0872134` - ThinkOnProject テンプレートなしプロジェクトキュレーター駆動オーケストレーション (ref task-202)
  - 6 ファイル変更

- `cb3188e` - グループチャット @メンション可視化 (ref task-208)
  - 4 ファイル変更

- `f9968e5` - AI クライアント ToolCall 能力宣言とグレースフルデグラデーション (ref task-205)
  - 4 ファイル変更

- `0d2b843` - グループチャット決定ロジック ShouldReplyInGroupChat (ref task-201)
  - 6 ファイル変更

- `277a2b1` - ナレッジネットワーク補完 - 高度なクエリとグラフトラバーサル (ref task-207)
  - 9 ファイル変更

#### 修正
- `6d0b66e` - グループチャットメッセージ送信時の appendMessage TypeError を修正 (ref task-209)
  - 5 ファイル変更

- `b15167c` - task-203 で漏れていた list-workflow-templates ルート登録を補完 (ref task-203)
  - 1 ファイル変更

- `dc549a2` - Gitee 同期 workflow を修正 - トークン URL にユーザー名を追加
  - 1 ファイル変更

#### インフラストラクチャ
- `e5fa3ad` - GitHub 自動同期 schedule を無効化、Gitee 公式同期方案を待機
  - 1 ファイル変更

#### 協力フレームワーク
- `4a58c82` - システム能力分析レポート + ThinkOnProject 設計案を追加
  - 5 ファイル変更

- `8ab29e6` - システム能力完全性分析レポートを .ai-collab/docs にアーカイブ
  - 2 ファイル変更

- `b412d9c` - 旧タスクをアーカイブ、総合分析に基づき task-201~208 を再リリース
  - 2 ファイル変更

- `437884a` - 協力メタデータを更新 - task-202/203/204 完了 (ref task-202, task-203, task-204)
  - 2 ファイル変更

- `bf78d79` - 協力メタデータを更新 - task-201/205/208 完了
  - 2 ファイル変更

- `de6ee0e` - セッション終了記録 catpaw-20260517-2215
  - 5 ファイル変更

- `7223b6f` - セッション終了記録 catpaw-20260517-2200
  - 4 ファイル変更


## [Alpha-0.2] - 2026-05-16

### 2026-05-16

#### リリース準備
- `476d839` - alpha-0.2 リリースタスクを追加
  - task-114（CHANGELOG 作成）と task-115（バージョン番号更新）を作成
  - 1 ファイル変更

### 2026-05-15

#### インフラストラクチャ
- `672627b` - Gitee 同期ワークフローを追加（権限設定付き）
  - sync-from-gitee.yml ワークフロー権限設定を更新
  - 1 ファイル変更、7 行追加、4 行削除

- `3cd5256` - GitHub Actions 自動 Gitee コード同期を追加
  - sync-from-gitee.yml ワークフローを新規追加
  - 1 ファイル変更、50 行追加

#### ドキュメント更新
- `aa1d2ad` - 全11言語の README/アーキテクチャ/入門ドキュメントを更新、SiliconLife.Fast マルチプラットフォームサポートを反映 (ref task-112, task-113)
  - ドキュメント内の SiliconLife.Fast が Windows のみという記述を修正、実際のマルチプラットフォームサポート（Windows / macOS / Linux）を反映
  - 11言語の README.md、architecture.md、getting-started.md を更新
  - SelectComponent に hint プロパティサポートを追加
  - ConfigView 列挙型ドロップダウンに hint を渡すよう変更
  - 11言語ローカリゼーションに SelectSearchHint キーを追加
  - 53 ファイル変更、690 行追加、194 行削除

#### タスクシステム
- `3329f3d` - タスクシステム点検メカニズムを追加 + ローカリゼーションバグ修正タスク
  - task-113 を作成：バージョン情報ページのローカリゼーション問題を修正
  - task-112 を更新：Fast バージョンドキュメントで Linux サポートを更新
  - 完了タスク（11件）を .ai-collab/archive/ にアーカイブ
  - 点検メカニズム設定完了：クイック点検（30分ごと）+ 全量点検（毎日 06:00）
  - 2 ファイル変更、148 行追加、171 行削除

#### 協力フレームワーク
- `6038e22` - coze-agent を .ai-collab 協力レジストリに登録
  - 抖子プラットフォーム常駐 AI 登録情報を追加
  - 1 ファイル変更

### 2026-05-14

#### AI 協力フレームワーク
- `7344fbb` - handoff モードを削除、タスクリスト駆動に変更 (v2.0)
  - .ai-collab ディレクトリ構造を handoff 引き継ぎモードからタスクリスト駆動にリファクタリング
  - tasks.json タスクリストコアファイルを追加
  - activity.log 操作ログを追加
  - changes/ と sessions/ ディレクトリを追加

- `589a48e` - .ai-collab セッション記録を追加
  - AI 協力セッションステータス記録を追加

- `5481bcf` - Qoder AI IDE を協力レジストリに登録
  - Qoder AI プログラミングアシスタント登録情報を追加

- `e2d7b61` - tasks.json の relatedCommit と changes commitHash を補完
  - タスクメタデータ関連付けを改善

- `a087f0c` - task-101~110 の全タスクを検収
  - 10タスクの修正がすべて完了したことを確認

#### バグ修正
- `fac9435` - task-101~110 の全10タスクの修正と実装を完了
  - 検索選択コンポーネントのヒントテキスト欠落を修正
  - バージョン情報ページのローカリゼーション問題を修正
  - ヘルプシステム検索 JS エラーを修正
  - 39 ファイル変更、684 行追加、121 行削除

- `c46dfbc` - 全ての未処理タスクを完了 (task-001~006)
  - 初期6つの未処理タスクを完了

- `ec176b2` - タスクリストを上書き - コードレビューで10個の新規バグを発見
  - task-101~110 の計10個の新規タスクを作成

#### リファクタリング
- `ab15915` - 著作権ヘッダーを統一 + HelpController BOM と HelpView 検索 JS を修正
  - すべての C# ソースファイルの Apache 2.0 著作権ヘッダーを統一
  - HelpController BOM エンコーディング問題を修正
  - HelpView 検索 JavaScript エラーを修正

#### 新機能
- `18a6f5d` - MCP ブラウザ能力サーバーを作成 (ref task-111)
  - SiliconLife.McpServer プロジェクトを追加
  - Playwright ブラウザ自動化 MCP サーバーを実装

- `9eb251a` - SiliconLife.McpServer モジュールを削除 (ref task-111)
  - 独立した MCP サーバーを削除、機能はメインプロジェクトに統合済み

### 2026-05-13

#### ローカリゼーション
- `7a62590` - ポーランド語ローカリゼーションサポートを追加
  - pl-PL ポーランド語ローカリゼーション実装を追加（PlPL.cs、1089行）
  - ポーランド語ヘルプドキュメントローカリゼーションを追加（HelpLocalizationPlPL.cs、3972行）
  - ポーランド語中国歴史カレンダーサポートを追加（ChineseHistoricalPlPL.cs、600行）
  - ポーランド語トレイローカリゼーションを追加（TrayPlPL.cs、135行）
  - ポーランド語完全ドキュメントセットを追加（15ドキュメント）
  - Language 列挙型にポーランド語を追加
  - 35 ファイル変更、14379 行追加、11 行削除

- `51f9c8e` - ドキュメント内の Ark AI 参照と用語改善を更新
  - 多言語ドキュメントの AI クライアント用語を更新

- `7587c12` - 全言語に変更ログエントリを追加
  - すべての言語バージョンの changelog を同期更新

#### ウィンドウシステムマイグレーション
- `b49a07d` - Avalonia ウィンドウ常駐モードにマイグレーション
  - Windows Forms 依存を削除、完全に Avalonia UI フレームワークにマイグレーション
  - ステータスウィンドウが Linux で正常に表示（リモートデスクトップ検証）
  - ウィンドウコントロールを追加：右クリックメニュー、ダブルクリックで Web を開く、閉じるボタン
  - マルチ AI 協力フレームワーク (.ai-collab/) を追加
  - トレイアイコン初期化を修正（グレースフルデグラデーション）
  - App.axaml と App.cs Avalonia アプリケーションエントリを追加
  - 13 ファイル変更、1442 行追加、541 行削除

- `d335aaf` - Linux プラットフォームでウィンドウを常に表示 + 閉じる確認ダイアログ
  - Linux でステータスウィンドウを自動表示（トレイアイコンなし）
  - Linux でウィンドウを閉じる際に確認ダイアログを表示
  - Windows/macOS は従来のトレイ動作を維持
  - --no-tray パラメータでトレイを強制無効化をサポート
  - 確認ダイアログ用の ShowMessageBoxAsync メソッドを追加
  - 3 ファイル変更、206 行追加、29 行削除

#### トレイシステムリファクタリング
- `841d384` - トレイシステムをリファクタリングし AI 協力フレームワークを初期化
  - TrayLocalizationBase を簡素化し未使用プロパティを削除
  - ShowStatus ローカリゼーション項目を追加
  - App.cs にトレイアイコンクリックでステータスウィンドウ表示、ローカライズメニュー項目を追加
  - Program.cs でトレイアイコン初期化を StartAsync に移動
  - TrayStatusWindow は閉じる際に終了ではなく非表示に
  - trae-glm5 と catpaw を .ai-collab 協力フレームワークに登録
  - .gitignore を更新し .ai-collab の全ファイルが追跡されるよう設定
  - 22 ファイル変更、178 行追加、1226 行削除

#### ドキュメント
- `43653bc` - リポジトリ説明と AI レジストリを更新
  - プロジェクト README と .ai-collab 登録情報を更新

### 2026-05-12

#### タスクシステム Web ビュー
- `0891b3c` - タスク実行詳細と履歴ビューを追加
  - TaskExecutionDetailView タスク実行詳細ビューを追加
  - TaskExecutionHistoryView タスク実行履歴ビューを追加
  - TaskController に実行詳細と履歴クエリインターフェースを追加
  - TaskViewModel タスクビューモデルを追加
  - TaskCenter タスクセンターを強化
  - TaskSystem タスクシステムを更新
  - 9言語ローカリゼーションにタスク関連キーを追加
  - 26 ファイル変更、803 行追加、55 行削除

### 2026-05-11

#### Web コンポーネントアーキテクチャリファクタリング
- `5e687ad` - コンポーネントレンダリングを文字列から H-tree にマイグレーション
  - ComponentBase レンダリングメソッドを文字列モードから H-tree 構造にマイグレーション
  - 全28コンポーネントを新レンダリングアーキテクチャに適合（A、Accordion、Button、Calendar、Card、Chart 等）
  - SelectComponent を大幅リファクタリング（889行改善）
  - コントローラーとビューを同期更新
  - 33 ファイル変更、667 行追加、435 行削除

- `bfd332d` - Style を文字列から CssBuilder インラインスタイルにマイグレーション
  - CssBuilder スタイルビルダーを追加
  - ComponentBase スタイルシステムを文字列から構造化 CssBuilder にマイグレーション
  - LoadingComponent を大幅強化（103行追加）
  - ConfigController、LogController、MemoryController コントローラーのスタイルをマイグレーション
  - ChatView、ConfigView、LogView、MemoryView ビューのスタイルをマイグレーション
  - 37 ファイル変更、351 行追加、157 行削除

#### ストレージシステム最適化
- `d67a7ee` - QueryLatest 大規模データセットクエリを最適化
  - SpeedyTimeStorage QueryLatest メソッドのパフォーマンス最適化
  - SpeedyLoggerProvider ロガープロバイダーを強化
  - 2 ファイル変更、44 行追加、5 行削除

#### カレンダーシステムリファクタリング
- `9629f88` - TimerExecution を抽出しタイマー Web ビューを強化
  - TimerSystem から TimerExecution ロジックを抽出（175行削除）
  - SelectComponent を大幅強化（427行改善）
  - TimerController とタイマービューを強化
  - ContextManager コンテキストマネージャーを更新
  - 12 ファイル変更、458 行追加、267 行削除

#### ローカリゼーション
- `5d8ca79` - LogsLoading ローカリゼーションキーを追加
  - 9言語に LogsLoading キーを追加
  - DefaultLocalizationBase 基底クラスに定義を追加
  - 11 ファイル変更、15 行追加

### 2026-05-10

#### タスクシステムリファクタリング
- `54394f6` - タスクシステムとチャット履歴サイクルを統合
  - ProjectTaskSystem プロジェクトタスクシステムを大幅に簡素化（411行リファクタリング）
  - TaskSystem タスクシステムを簡素化（254行リファクタリング）
  - TaskCenter タスクセンターをリファクタリング（188行改善）
  - ContextManager コンテキストマネージャーを最適化（347行リファクタリング）
  - DefaultSiliconBeing シリコンビーイングを強化
  - TimerSystem タイマーシステムにタスクを統合
  - IWorkNoteStorage インターフェースを更新
  - SpeedyWorkNoteStorage と FileSystemWorkNoteStorage を適合
  - 16 ファイル変更、648 行追加、897 行削除

### 2026-05-09

#### Web インターフェース強化
- `bc50dd7` - チャットビューを改善し監査機能を追加
  - AuditController 監査コントローラーを追加（261行）
  - AuditView 監査ビューを追加（379行）
  - AuditViewModel 監査ビューモデルを追加
  - ChatView チャットビューを大幅改善（171行強化）
  - ChatController チャットコントローラーを更新
  - MarkdownEditorComponent コンポーネントを強化
  - InitController 初期化コントローラーを改善
  - ChatSystem チャットシステムに新機能を追加
  - 14 ファイル変更、1030 行追加、112 行削除

- `c9babce` - チャットビューのツールコールレンダリングを改善
  - ChatView ツールコールブロックレンダリングを強化
  - 1 ファイル変更、54 行追加、11 行削除

#### AI ツールシナリオシステム
- `ff2eddd` - ツールシナリオフィルタリングシステムを実装
  - ToolScenarioAttribute ツールシナリオ属性を追加（36行）
  - ChatOnlyAttribute チャット専用シナリオ属性を追加（19行）
  - ToolManager ツールマネージャーにシナリオフィルタリング機能を追加（40行）
  - ContextManager コンテキストマネージャーをシナリオフィルタリングに適合
  - 4 ファイル変更、115 行追加、30 行削除

- `5709a33` - ツールクラスにシナリオ属性を追加
  - 24のツールクラスに ToolScenario 属性アノテーションを追加
  - カレンダー、チャット、設定、キュレーション、データベース、ディスク、動的コンパイル等のツールを含む
  - 24 ファイル変更、46 行追加、20 行削除

#### タスクシステムリファクタリング
- `2f19a5f` - TaskCenter と TaskEnumerator を使用してタスクシステムをリファクタリング
  - TaskCenter タスクセンターを追加（235行）
  - TaskEnumerator タスク列挙子を追加（297行）
  - TaskSystem タスクシステムをリファクタリング・簡素化
  - DefaultSiliconBeing シリコンビーイングを新アーキテクチャに適合
  - DefaultSiliconBeingFactory ファクトリを更新
  - SiliconBeingBase 基底クラスを強化
  - 7 ファイル変更、796 行追加、275 行削除

#### 権限システムマイグレーション
- `a06ed09` - IM と権限システムを App プロジェクトにマイグレーション
  - PermissionRequestQueue を Default/Fast から App プロジェクトにマイグレーション（443行追加）
  - Default バージョンの WebUIProvider を削除（403行削除）
  - Default バージョンの HelpTool を削除（194行削除）
  - Default/Fast バージョンの重複する PermissionRequestQueue を削除
  - Default バージョンの IMPermissionAskHandler を削除
  - PermissionRequestController コントローラーを更新
  - 14 ファイル変更、496 行追加、1183 行削除

#### AI コンテキスト最適化
- `4c8aaff` - コンテキストマネージャーを最適化しサービスロケーターを強化
  - ContextManager コンテキストマネージャーを簡素化・最適化
  - ServiceLocator サービスロケーターを強化（36行追加）
  - ToolManager ツールマネージャーを強化（34行追加）
  - DashScopeClient と VolcengineArkClient クライアントを改善
  - エグゼキューター（CommandLine、Disk、Network）を更新
  - 8 ファイル変更、116 行追加、98 行削除

#### ローカリゼーション
- `5c5eef7` - 監査とタスクローカリゼーションキーを追加
  - DefaultLocalizationBase に127行のローカリゼーション定義を追加
  - 9言語に監査とタスク関連キーを追加（各26行）
  - 11 ファイル変更、387 行追加

#### プロジェクト設定
- `2067db6` - プロジェクト設定と gitignore ルールを更新
  - .gitignore ルールを更新
  - DefaultConfigData と Fast DefaultConfigData 設定を強化
  - SpeedyWorkNoteStorage ストレージを改善
  - SpeedyPack コアを強化
  - 5 ファイル変更、32 行追加、6 行削除

### 2026-05-07

#### イタリア語ローカリゼーション
- `8adc18c` - イタリア語ローカリゼーションサポートを追加し多言語ドキュメントを更新
  - it-IT イタリア語ローカリゼーションを追加
  - ItIT ローカリゼーション実装を追加（1909行）
  - ChineseHistoricalItIT 中国歴史カレンダーイタリア語サポートを追加（586行）
  - TrayItIT トレイイタリア語ローカリゼーションを追加（135行）
  - イタリア語完全ドキュメントセットを追加（14ドキュメント：README、API リファレンス、アーキテクチャ、カレンダーシステム、変更ログ、コントリビューションガイド等）
  - 全言語バージョンのアーキテクチャ、開発ガイド、入門ガイド等のドキュメントを更新
  - Language 言語列挙型にイタリア語を追加
  - 86 ファイル変更、11573 行追加、769 行削除

#### ドキュメント同期
- `12a5deb` - アーキテクチャ、変更ログ、シリコンビーイングガイドの多言語ドキュメントを更新
  - 8言語の README を更新
  - 8言語のアーキテクチャドキュメントを更新
  - 8言語の変更ログを更新
  - 8言語のシリコンビーイングガイドを更新
  - 8言語のツールリファレンスを更新
  - 用語集をリファクタリング
  - 46 ファイル変更、1697 行追加、442 行削除

### 2026-05-06

#### 大規模モジュールリファクタリング
- `eeb3be6` - 大規模モジュールリファクタリングと再編
  - SiliconLife.App プロジェクト構造を調整
  - SiliconLife.Fast プロジェクトを再編
  - SiliconLife.Default プロジェクトを再編
  - SiliconLife.Common 共有モジュールを再編
  - SiliconLife.Core コアモジュールを再編
  - SiliconLife.Speedy ストレージエンジンを再編
  - SiliconLife.Speedy.Manager 管理ツールを再編
  - 119 ファイル変更、6926 行追加、3066 行削除

### 2026-05-04

#### AI クライアント
- `24d2c86` - VolcengineArkClient を追加し Audit を Usage tracking に置き換え
  - VolcengineArkClient 火山エンジン Ark AI クライアントを追加
  - ストリーミングおよび非ストリーミングモードをサポート
  - 内蔵2層レート制御（自己レート制御 + サーバーレート制限）
  - OpenAI API プロトコル互換
  - Audit システムを Usage tracking に置き換え
  - 24 ファイル変更、802 行追加、21 行削除

#### ツールシステム
- `f27650a` - Fast 自動再起動用ホットリロードツールを追加
  - HotReloadTool ホットリロードツールを追加
  - SiliconLife.Fast のオンラインコンパイル、更新、再起動をサポート
  - HotReload.exe 独立アップデーターを追加
  - セーフファイルコピー機構（自身を上書きしない）
  - グレースフルシャットダウンとポート解放待機
  - 9 ファイル変更、581 行追加

#### ローカリゼーション
- `6a5aad8` - 全ファイルを更新しフランス語ローカリゼーションサポートを追加
  - fr-FR フランス語ローカリゼーションを追加
  - 全言語バージョンを更新
  - ヘルプドキュメントのフランス語翻訳
  - インターフェースのフランス語翻訳
  - 100+ ファイル変更

### 2026-05-03

#### プロジェクトインフラストラクチャ
- `2664b0c` - プロジェクトインフラストラクチャと依存関係を更新
  - SiliconLife.Speedy.Manager に WPF 管理インターフェースを追加（MainForm.Designer.cs、MainForm.resx）
  - slc.ico アイコンリソースを追加（1.5MB）
  - PluginLoader セキュリティスキャンを大幅強化（622行追加）
  - PermissionedStreamFactory パーミッションストリームファクトリーを追加（779行）
  - PermissionRequestQueue パーミッションリクエストキューを追加（Default と Fast バージョン）
  - DebugLoggerProvider デバッグロガープロバイダーを追加
  - ConfigDataBase 設定データベース基底クラスを強化
  - ToolManager にプラグインツールスキャン機能を追加（ScanAllPluginAssemblies）
  - SiliconBeingManager ライフサイクル管理を強化
  - DashScopeClient Alibaba Cloud AI クライアントを大幅強化（227行追加）
  - DefaultSiliconBeingFactory ファクトリを強化
  - Web ビューとコントローラーを更新（ChatView、WorkNoteView、PermissionRequestController）
  - 9言語ローカリゼーションにキーを追加
  - 35 ファイル変更、28080 行追加、336 行削除

### 2026-05-02

#### AI クライアント強化
- `c16f99f` - AI クライアント、Web UI、ストレージコンポーネントを更新
  - DashScopeClient Alibaba Cloud クライアントを大幅改善
  - SpeedyPackAutoCompactor 自動コンパクターを最適化
  - Web ビュー基底クラスと BeingView を改善
  - 6 ファイル変更、240 行追加、81 行削除

#### プラグインシステム
- `242dc98` - バージョン情報ページにプラグインリストを追加
  - AboutController にプラグイン情報表示を追加
  - AboutViewModel にプラグインデータモデルを追加
  - AboutView にプラグインリストレンダリングを追加
  - 9言語ローカリゼーションにプラグイン関連キーを追加
  - 14 ファイル変更、160 行追加、1 行削除

#### AI 最適化
- `147f8f4` - コンテキストメモリプロンプトテキストを簡素化
  - ContextManager AI プロンプトを最適化
  - 1 ファイル変更、1 行追加、1 行削除

#### Speedy ストレージ最適化
- `8bda2d3` - Speedy ストレージとメモリコントローラー実装を更新
  - SpeedyPackAutoCompactor インターバルを修正
  - SpeedyTimeStorage パス処理を最適化
  - MemoryController メモリコントローラーを改善
  - SpeedyPack.Manager UI を更新
  - 4 ファイル変更、21 行追加、18 行削除

#### トレイ強化
- `8972654` - トレイステータスウィンドウのローカリゼーションサポートを強化
  - 9言語トレイローカリゼーションに Speedy 管理エントリを追加
  - TrayStatusWindow に Speedy 管理メニュー項目を追加
  - 11 ファイル変更、72 行追加

#### Speedy.Manager 最適化
- `6f5db09` - SpeedyPack マネージャー UI と内部コンポーネントを最適化
  - MainForm インターフェースをリファクタリング
  - FreeList メモリ管理を最適化
  - WriteQueue ライトキューを改善
  - SpeedyPack コアを最適化
  - 5 ファイル変更、96 行追加、88 行削除

#### ストレージシステム強化
- `57f9d5d` - ストレージシステムを改善、自動圧縮と不完全日付サポートを追加
  - SpeedyPackAutoCompactor 自動圧縮タイマーを追加（30分間隔）
  - SpeedyPackRegistry シングルトンマネージャーを強化
  - SpeedyStorage、SpeedyTimeStorage、SpeedyWorkNoteStorage の適合を改善
  - SpeedyPack に FreeList フリーリストを追加（149行）
  - PackFileWriter ライターをリファクタリング・最適化
  - WriteOperation、WriteQueue ライトキューを強化
  - SpeedyPackOptions オプションを拡張
  - IncompleteDate に比較メソッドを追加
  - PluginLoader プラグインローダーを改善
  - Default と Fast バージョンの Program.cs 初期化フローを更新
  - DefaultConfigData 設定データを簡素化
  - KnowledgeNetwork ナレッジネットワークを簡素化
  - ChatController、MemoryController コントローラーを最適化
  - SpeedyPack.Manager MainForm 機能を強化
  - 22 ファイル変更、639 行追加、253 行削除

#### Speedy.Manager 更新
- `b04ed33` - Speedy.Manager ファイルを更新

### 2026-05-01

#### アーキテクチャリファクタリング：Speedy ストレージが LiteDB を置き換え
- `6600972` - Speedy ストレージで LiteDB を置き換え、プラグインシステムと Speedy プロジェクトを追加
  - **SiliconLife.Speedy プロジェクトを追加**：高性能 .spk ストレージエンジン
    - SpeedyPack コアクラス（489行）：メモリディレクトリマップ + エントリキャッシュ + 非同期ライトキュー
    - SpeedyPackOptions 設定クラス：キャッシュ TTL、最大キャッシュエントリ数、読み取り専用モード
    - IPackTransaction トランザクションインターフェース：アトミック書き込み操作をサポート
    - SpkFileInfo ファイル情報クラス
    - Internal ディレクトリ：DirectoryMap、EntryCache、PackFileReader、PackFileWriter、WriteQueue、WriteOperation、SpeedyTransaction、SpkHeader、PathNormalizer、FreeList
    - MessagePack 3.1.4 に依存してバイナリシリアライズ（LZ4 圧縮）
  - **SiliconLife.Speedy.Manager プロジェクトを追加**：WPF 管理ツール
    - MVVM アーキテクチャ：MainViewModel、DirectoryTreeViewModel、ContentViewerViewModel 等
    - サービス層：PackService、FileDialogService、RecentFilesService、NotificationService
    - コンバーター：BoolToVisibility、ByteSizeToString、ContentTypeToIcon、NullToCollapsed
    - ビュー：MainWindow、DirectoryTreeView、ContentViewerPanel、MetadataPanel
    - ダイアログ：FileInfoDialog、ImportDialog、NewEntryDialog
  - **SiliconLife.Fast ストレージマイグレーション**：LiteDB → SpeedyPack
    - SpeedyStorage（IStorage アダプター）を追加
    - SpeedyTimeStorage（ITimeStorage アダプター）を追加
    - SpeedyWorkNoteStorage（IWorkNoteStorage アダプター）を追加
    - SpeedyPackRegistry（プロセスレベルシングルトン管理）を追加
    - SpeedyPackAutoCompactor（自動圧縮タイマー）を追加
    - LiteDB 関連ストレージ実装を削除（LiteDBStorage、LiteDBTimeStorage、LiteDBWorkNoteStorage、LiteDBLoggerProvider、LiteDBManager、LiteDBModels）
    - LiteDB 管理ウィンドウ関連コードを削除
  - **プラグインシステム**：
    - IPlugin インターフェースを追加（Core/Plugins/IPlugin.cs）
    - PluginLoader プラグインローダーを追加（Core/Plugins/PluginLoader.cs）
    - ディレクトリからのプラグイン DLL ロードをサポート
    - セキュリティスキャン：名前空間チェックを禁止（System.IO、System.Net、Microsoft.CodeAnalysis 等）
    - 信頼されたアセンブリホワイトリスト（Google.Protobuf、Newtonsoft.Json、MessagePack 等）
    - カスタム AssemblyLoadContext 分離ロード
    - ToolManager に ScanAllPluginAssemblies メソッドを追加
    - CoreHost にプラグインローダーを統合
  - 119 ファイル変更、6926 行追加、3066 行削除

#### シリコンビーイング強化
- `3aef4c3` - Stopped アクティビティステータスとエラー処理改善を追加
  - シリコンビーイングに Stopped ステータスを追加
  - エラー処理とリカバリーメカニズムを強化

#### ローカリゼーション更新
- `513c65d` - 全言語バージョンとドキュメントを更新
  - MarkdownEditorComponent コンポーネントを追加（625行）
  - DetailsComponent コンポーネントを追加（130行）
  - AccordionComponent アコーディオンコンポーネントを追加（285行）
  - BeingController、ChatController、MemoryController、PermissionController コントローラーを更新
  - BeingView、ChatView、MemoryView、SoulEditorView ビューをリファクタリング
  - 旧 MarkdownEditorView を削除
  - InitController コンポーネント化マイグレーション
  - 115 ファイル変更、5761 行追加、2362 行削除

### 2026-04-30

#### システムトレイ機能
- `101b203` - トレイステータスウィンドウと ApplicationContext を実装
  - トレイアイコンリソースを追加（alpha.png、noWord.png、slc.ico、wordIcon.png）
  - TrayStatusWindow ステータスウィンドウを実装
  - 9言語のトレイローカリゼーションをサポート（TrayCsCZ、TrayDeDE、TrayEnUS 等）
  - TrayLocalizationBase 抽象基底クラス
  - 24 ファイル変更、27995 行追加、1 行削除（リソースファイル含む）

#### コンポーネント化 UI アーキテクチャ
- `e61cfaa` - コンポーネント化 UI アーキテクチャを完了、24コンポーネントを実装
  - MVP フェーズ（8個）：ComponentBase、Div、Span、Button、Input、Form、Select、Label
  - 第2フェーズ（6個）：Accordion、Card、Tabs、Table、Modal、Message
  - 第3フェーズ（5個）：Calendar、Tree、Chart、FileUpload、RichText
  - Js、Behavior、DomUpdate 等のヘルパークラスを追加
  - 25 ファイル変更、2666 行追加

- `7449e51` - コンポーネントシステムを改善し新しいスキンテーマを追加
  - A、Button、Div、Form、Input 等のコンポーネントを強化
  - 3つのスキンテーマを追加：HighContrast（ハイコントラスト）、Light（ライト）、Minimal（ミニマル）
  - 既存スキンを更新（Admin、Chat、Creative、Dev）
  - InitController コンポーネント化マイグレーション
  - 32 ファイル変更、1466 行追加、1238 行削除

- `1ba8636` - InitController コンポーネント化マイグレーションを開始（進行中）
  - 9 ファイル変更、574 行追加、145 行削除

#### ストレージシステム統一
- `895dff9` - soul.md と state.json の IStorage インターフェース使用を統一
  - DefaultSiliconBeing が IStorage でソウルファイルとステータスを読み書き
  - StateFileManager ステータスファイルマネージャーを追加
  - SoulFileManager を IStorage に適合するようリファクタリング
  - 8 ファイル変更、201 行追加、116 行削除

#### LiteDB 管理強化
- `a34bef4` - LiteDBManager を追加しトレイローカリゼーションを強化
  - トレイメニューに LiteDB 管理エントリを追加
  - 9言語トレイローカリゼーションを更新
  - 10 ファイル変更、196 行追加

- `c4a79ca` - LiteDB 管理ウィンドウに言語認識ローカリゼーションファクトリーを追加
  - 1 ファイル変更、78 行追加

- `5ebc55e` - LiteDBAdminLocalization を抽象基底クラスに変換
  - 10 ファイル変更、1356 行追加

#### 設定システム修正
- `2da5256` - ConfigExists 抽象メソッドを追加し LiteDB 重複設定レコードを修正
  - ConfigDataBase に ConfigExists メソッドを追加
  - Fast バージョンの DefaultConfigData で LiteDB 設定存在チェックを実装
  - LiteDB 重複設定キー問題を修正
  - 9 ファイル変更、210 行追加、2 行削除

#### チャットとビュー最適化
- `d3618ec` - チャットセッション、ストレージシステム、時間モデル、ビュー基底クラスを最適化
  - BroadcastChannel、GroupChatSession、SingleChatSession を最適化
  - ITimeStorage にクエリメソッドを追加
  - FileSystemStorage と LiteDBStorage を同期更新
  - ViewBase をリファクタリング・最適化（Default と Fast バージョン）
  - 11 ファイル変更、622 行追加、392 行削除

### 2026-04-29

#### アーキテクチャリファクタリング：共有モジュール抽出
- `a102428` - 共有モジュールを SiliconLife.Default から SiliconLife.Common にマイグレーション
  - 32種のカレンダー実装を Common プロジェクトに抽出
  - ローカリゼーション基底クラスと21言語実装を Common プロジェクトに抽出
  - パーミッションマネージャー、デフォルトシリコンビーイング実装を Common プロジェクトに抽出
  - 23個の組み込みツール実装を Common プロジェクトに抽出
  - Playwright WebView 実装を Common プロジェクトに抽出
  - 名前空間を SiliconLife.Collective に更新
  - 122 ファイル変更、586 行追加、343 行削除

#### コード品質改善
- `17566fe` - Core、Common、Default プロジェクトの Console.WriteLine をロギングシステムに置き換え
  - ContextManager、AuditLogger、DefaultConfigData 等6ファイルを更新
  - ILogger インターフェースの統一使用、コード保守性を向上
  - 6 ファイル変更、12 行追加、8 行削除

#### SiliconLife.Fast 高性能バージョン
- `54a0307` - SiliconLife.Fast プロジェクトを追加しコンパイル修正を完了
  - 完全な Windows フォームアプリケーションエントリポイント
  - システムトレイサポート（NotifyIcon）
  - 全 Web UI コントローラーを移植（20+個）
  - 全 Web ビューコンポーネントを移植
  - 4種のスキンテーマを移植（Admin、Chat、Creative、Dev）
  - 125 ファイル変更、61186 行追加

#### 多言語ドキュメント同期
- `265fde8` - デュアルバージョンアーキテクチャドキュメントを全言語に同期
  - 7言語の architecture.md、changelog.md を更新
  - 6言語の contributing.md を更新
  - 7言語の getting-started.md、roadmap.md を更新
  - 47 ファイル変更、1214 行追加、38 行削除

#### LiteDB ストレージシステム（Fast バージョン）
- `4704862` - LiteDB 依存関係とインフラストラクチャを追加
  - LiteDBManager 管理クラスを追加
  - LiteDBModels データモデルを追加
  - 3 ファイル変更、252 行追加

- `4220036` - LiteDB ストレージクラスを実装
  - LiteDBStorage：IStorage インターフェースを実装
  - LiteDBTimeStorage：ITimeStorage インターフェースを実装
  - LiteDBWorkNoteStorage：IWorkNoteStorage インターフェースを実装
  - 3 ファイル変更、581 行追加

- `38ebd23` - 設定とロギングシステムを LiteDB にマイグレーション
  - DefaultConfigData を LiteDB ストレージに適合
  - LiteDBLoggerProvider ロガープロバイダーを追加
  - 2 ファイル変更、203 行追加、67 行削除

- `e687157` - ナレッジネットワークをファイルシステムから LiteDB にマイグレーション
  - KnowledgeNetwork を全面的にリファクタリング、LiteDB でトリプルデータをストレージ
  - 1 ファイル変更、231 行追加、72 行削除

- `4220169` - LiteDB ストレージを Program と ProjectManager に統合
  - Program.cs で LiteDB ストレージを初期化
  - ProjectManager を LiteDB ワークノートストレージに適合
  - 2 ファイル変更、40 行追加、17 行削除

- `5f3a709` - 廃止されたファイルシステムストレージ実装を削除
  - FileSystemLoggerProvider、FileSystemStorage、FileSystemTimeStorage 等を削除
  - 6 ファイル変更、1518 行削除

- `e1a4ef2` - docs: 全ドキュメントに v0.1.0-alpha バージョン識別子を追加
  - 127 ファイル変更、2297 行追加、2471 行削除

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### ストレージシステムリファクタリング
- `8dd26e3` - ITimeStorage インターフェースの IncompleteDate 使用を統一し階層クエリ API を追加
  - ITimeStorage インターフェースの DateTime オーバーロードメソッドを削除、IncompleteDate に統一
  - IncompleteDate に CompareTo(DateTime) 比較メソッドと Expand() 展開メソッドを追加
  - GetEarliestTimestamp()、GetLatestTimestamp() 階層クエリ API を追加
  - HasSummary() と QueryWithLevel() メソッドを追加、時間階層によるクエリをサポート
  - Memory.cs 圧縮アルゴリズムをリファクタリング、新しい階層クエリ API で効率を向上
  - FileSystemTimeStorage.cs で新しいインターフェースメソッドを完全実装
  - 全呼び出し元を同期更新：ChatSystem、ChatSession、BroadcastChannel、AuditLogger、TokenUsageRecord 等
  - ツールシステムを更新：HelpTool、LogTool、TokenAuditTool を新インターフェースに適合
  - Web コントローラーを更新：AuditController、ChatController、ChatHistoryController を新インターフェースに適合
  - 41 ファイル変更、1820 行追加、903 行削除

### 2026-04-27

#### ヘルプドキュメントシステム強化
- `9989d79` - ローカリゼーション、ヘルプシステム、Web ビューを更新
  - IAIClientFactoryHelp.cs AI クライアントファクトリーヘルプドキュメントインターフェースを追加
  - 全ヘルプドキュメントの9言語翻訳を完了
  - HelpTopics.cs に40個のヘルプトピック定義を追加
  - Web ビューを全面的に更新：InitController、AuditView、ConfigView、KnowledgeView、LogView 等
  - ローカリゼーションシステムを強化：全言語バージョンに新しいローカリゼーションキーを追加
  - AI クライアントファクトリーを更新：DashScopeClientFactory、OllamaClientFactory を改善
  - 30 ファイル変更、10086 行追加、15 行削除

#### ヘルプドキュメント新規追加
- `e7afe94` - ソウルファイルと監査ログヘルプドキュメントを追加
  - ソウルファイル管理ヘルプドキュメントを追加
  - 監査ログヘルプドキュメントを追加
  - HelpTopics.cs にトピック定義を追加
  - HelpView.cs を大幅リファクタリング、ドキュメントレンダリングロジックを改善
  - PermissionView.cs をリファクタリング、権限管理インターフェースを改善
  - コアモジュールを強化：SiliconBeingManager、TaskSystem、ToolManager を改善
  - TaskTool.cs をリファクタリング、タスク管理機能を改善
  - Web ビューを全面的に更新：全ビューコンポーネントを同期更新
  - HelpController.cs を簡素化、コントローラーロジックを最適化
  - 30 ファイル変更、7100 行追加、897 行削除

### 2026-04-26

#### ヘルプドキュメントシステム
- `07895d7` - ヘルプドキュメントシステムを強化、3つのドキュメントを追加し9言語翻訳を完了
  - メモリシステム、Ollama インストール設定、Alibaba Cloud Bailian プラットフォーム使用ガイドを追加
  - 全10ヘルプドキュメントの9言語翻訳を完了
  - HelpView レンダリングロジックを簡素化
  - 18 ファイル変更、14418 行追加、1364 行削除

#### ドイツ語ローカリゼーション
- `0cfd8a1` - 完全なドイツ語 (de-DE) ローカリゼーションサポートを追加
  - 完全なドイツ語ローカリゼーションファイル
  - 中国歴史カレンダードイツ語サポートを追加
  - ヘルプドキュメントドイツ語翻訳を追加
  - 9言語の全ドキュメントを完全同期
  - 135 ファイル変更、26186 行追加、14371 行削除

#### ドキュメント同期
- `3aada7d` - 繁体字中国語 (zh-HK) ドキュメントを簡体字中国語と一致するよう同期
  - 3 ファイル変更、519 行追加、422 行削除
- `2f6abff` - 全言語にヘルプツール表示名ローカリゼーションを追加
  - 7 ファイル変更、47 行追加、7 行削除

#### ナレッジシステムリファクタリング
- `60944fe` - 名前空間を SiliconLife.Collective に統一
  - 8 ファイル変更、5 行追加、8 行削除
- `69c51c5` - ヘルプドキュメントシステムを追加しコードコメントを英語に翻訳
  - 29 ファイル変更、3385 行追加、22 行削除

### 2026-04-25

#### WebView ブラウザ自動化
- `41757c3` - Playwright ベースのクロスプラットフォーム WebView ブラウザ自動化を実装
  - 6 ファイル変更、1152 行追加

#### ドキュメント更新
- `0ff797b` - KnowledgeTool と WorkNoteTool ドキュメントを追加（7言語）
  - 28 ファイル変更、4983 行追加
- `ad77415` - 全 changelog ファイルを更新、2026-04-25 Git 履歴記録を追加
  - 7 ファイル変更、168 行追加

#### プロジェクトワークスペース管理
- `785c551` - プロジェクトワークスペース管理を実装、ワークノートとタスクシステムを含む
  - プロジェクトワークスペース管理システムを追加
  - プロジェクト進捗を追跡するワークノート機能
  - タスクシステム統合
  - 29 ファイル変更、4256 行追加、36 行削除

#### チェコ語ローカリゼーション
- `b4bbf39` - 完全なチェコ語 (cs-CZ) ローカリゼーションを追加し全言語ドキュメントを更新
  - 116 ファイル変更、4933 行追加、222 行削除
- `faf078f` - チェコ語ローカリゼーションのコンパイルエラーを修正
  - 3 ファイル変更、910 行追加、1 行削除

#### ナレッジシステム強化
- `20adaac` - KnowledgeTool を追加し完全なローカリゼーションをサポート
  - 34 ファイル変更、2331 行追加、56 行削除

### 2026-04-24

#### メモリ管理システム強化
- `c7b2ecc` - メモリ管理機能を強化、高度なフィルタリング、統計、詳細ビュー機能を追加
  - メモリ高度フィルタリング機能を追加
  - メモリ統計機能を実装
  - メモリ詳細ビューページを追加
  - 多言語ローカリゼーションサポート（6言語）
  - 13 ファイル変更、840 行追加、86 行削除

#### 権限システム拡張
- `4489ad6` - wttr.in 天気サービスをネットワークホワイトリストに追加
  - 完全な多言語ドキュメント同期更新（6言語）
  - 14 ファイル変更、417 行追加、1 行削除

#### Web インターフェース修正
- `d9d72e9` - ワークノート詳細モーダルの CSS 優先度問題を修正
  - 19 ファイル変更、1744 行追加、6 行削除

#### チャット履歴最適化
- `0df599c` - ツール結果が独立したチャットメッセージとしてレンダリングされる問題を修正
  - 1 ファイル変更、222 行追加、21 行削除
- `057b09d` - チャット履歴詳細表示を最適化、ツールコールレンダリングを改善
  - 3 ファイル変更、389 行追加、68 行削除

#### タイマー実行履歴
- `fa3f06f` - タイマー実行履歴機能を追加、詳細ビューを含む
  - 8 ファイル変更、937 行追加、10 行削除
- `d824835` - タイマー実行履歴ローカリゼーションキーを追加（全言語）
  - 7 ファイル変更、88 行追加

#### ローカリゼーション強化
- `c13cb17` - スペイン語言語バリアントを登録
  - 1 ファイル変更、4 行追加
- `9c44f34` - 中国歴史カレンダー多言語ローカリゼーションサポートを追加
  - 16 ファイル変更、6049 行追加、1 行削除

#### コア機能改善
- `1e7c7b2` - メモリ圧縮とツール実行追跡を改善
  - 4 ファイル変更、338 行追加、86 行削除

### 2026-04-23

#### ツールローカリゼーション
- `192fc6e` - 5つのツールに欠落していたツール名ローカリゼーションを追加
  - 6 ファイル変更、30 行追加

#### ドキュメント更新
- `882c08f` - 全 changelog ファイルを更新、完全な Git 履歴記録を追加し不正なバージョン番号を削除
  - 45 ファイル変更、8815 行追加、1611 行削除

#### チャットページ強化
- `65c157b` - チャットページにローディングインジケーターを追加しキュレーターセッションを自動選択
  - 10 ファイル変更、211 行追加、7 行削除

#### チャット履歴機能
- `e483348` - シリコンビーイングチャット履歴表示機能を実装
  - ChatHistoryController を追加
  - ChatHistoryViewModel を作成
  - ChatHistoryListView と ChatHistoryDetailView ページを実装
  - チャット履歴のローカリゼーションキーを追加（5言語）
  - 12 ファイル変更、1178 行追加

#### AI ストリーム制御強化
- `30a2d4e` - AI ストリームキャンセル、IM 統合、コアホスト初期化を強化
  - 11 ファイル変更、387 行追加、12 行削除

#### チャットメッセージキュー
- `db48c51` - チャットメッセージキュー、ファイルメタデータ、ストリームキャンセルサポートを追加
  - 4 ファイル変更、357 行追加

#### ファイルアップロードサポート
- `28fb344` - ファイルソースダイアログとファイルアップロードサポートを実装
  - 3 ファイル変更、1100 行追加、2 行削除
- `1d3e2cc` - ファイルソースダイアログローカリゼーション文字列を追加（6言語）
  - 6 ファイル変更、30 行追加

#### ドキュメント更新
- `8111e92` - README のリポジトリセクションに Wiki リンクを追加
  - 1 ファイル変更、3 行追加、1 行削除

### 2026-04-22

#### ドキュメントローカリゼーション
- `66c11eb` - 中国語コメントを英語に翻訳し全 changelog を更新
  - 11 ファイル変更、373 行追加、163 行削除

#### SSE メッセージ強化
- `b574b2b` - 履歴メッセージに AI 識別用の senderName を追加
  - 1 ファイル変更、9 行追加

#### チャット機能
- `601fc14` - セッション終了マーク用の mark_read 操作を追加
  - 7 ファイル変更、196 行追加、36 行削除

#### ツールシステム最適化
- `7a03a19` - LogTool の対話クエリ柔軟性を改善
  - 1 ファイル変更、57 行追加、24 行削除

#### ローカリゼーション強化
- `0a8d750` - プロアクティブシリコンビーイング動作用の汎用システムプロンプトを追加
  - 8 ファイル変更、460 行追加、48 行削除

#### ロギングシステムリファクタリング
- `2b771f3` - LogController とファイル I/O を分離、ログ読み取り API を追加
  - 4 ファイル変更、172 行追加、137 行削除
- `12da302` - ログビューにシリコンビーイングフィルターを追加
  - 9 ファイル変更、147 行追加、10 行削除
- `8f6cb1e` - ILogger インターフェースに beingId パラメーターを追加、システム/シリコンビーイングログ分離を実現
  - 47 ファイル変更、524 行追加、490 行削除

#### 権限システム改善
- `4c747ad` - PermissionTool、ExecuteCodeTool をリファクタリング、EvaluatePermission API を追加
  - 18 ファイル変更、680 行追加、492 行削除

#### バグ修正
- `1c96e99` - search_files と search_content のルートディレクトリ検索失敗を修正
  - 1 ファイル変更、98 行追加、41 行削除

#### ツール統合
- `135710d` - SearchTool を削除、ローカル検索を DiskTool に移動
  - 2 ファイル変更、185 行追加、365 行削除

#### ツールシステム拡張
- `70ce7fb` - 構造化データベースクエリ用の DatabaseTool を実装
  - 1 ファイル変更、382 行追加
- `be29a09` - 操作・対話履歴クエリ用の LogTool を実装
  - 1 ファイル変更、298 行追加
- `4ea7702` - 動的権限管理用の PermissionTool を実装
  - 1 ファイル変更、457 行追加
- `1384ff4` - 多言語コード実行用の ExecuteCodeTool を実装
  - 1 ファイル変更、477 行追加
- `82d1e11` - 情報検索用の SearchTool を実装
  - 1 ファイル変更、363 行追加

#### Web インターフェース最適化
- `0675c45` - プレビューペインの Markdown コードブロックハイライトを最適化
  - 1 ファイル変更、4 行追加、23 行削除
- `702b3f3` - タスクビューを強化、ステータスバッジとメタデータ表示を追加
  - 8 ファイル変更、221 行追加、9 行削除
- `6ed9a79` - チャットメッセージストレージとビューレンダリングを改善
  - 8 ファイル変更、140 行追加、29 行削除

### 2026-04-21

#### バグ修正
- `c6b518b` - タイマーメッセージパッシングとチャットメッセージストレージを修正
  - 3 ファイル変更、297 行追加、124 行削除

#### 設定管理
- `4305769` - 行末管理用の .gitattributes を追加
  - 1 ファイル変更、32 行追加

#### Web インターフェース改善
- `188c6f8` - タスクリスト API ルートを登録し空状態表示を追加
  - 2 ファイル変更、35 行追加、2 行削除
- `634e8ca` - 権限ページにリストへ戻るリンクを追加
  - 1 ファイル変更、16 行追加
- `6ba591d` - シリコンビーイング用の独立した AI 設定エディターを追加
  - 11 ファイル変更、842 行追加、18 行削除
- `0a826f5` - コードエディターに保存成功通知を追加
  - 1 ファイル変更、9 行追加、2 行削除
- `2940373` - Web インターフェースを強化、コードホバーヒントと UI 改善を追加
  - 11 ファイル変更、1054 行追加、75 行削除

#### 権限システム修正
- `592c7ab` - コールバックのインスタンス化と登録順序を修正
  - 2 ファイル変更、38 行追加、7 行削除

#### セキュリティ強化
- `833ead2` - 動的コンパイルにアセンブリ参照検証を追加
  - 4 ファイル変更、135 行追加、8 行削除

#### 権限システム強化
- `5879621` - 権限コールバック事前コンパイル検証とエラー処理強化を追加
  - 21 ファイル変更、617 行追加、26 行削除

#### ドキュメント更新
- `4dbf659` - changelog を v0.5.1 に更新、GitHub プレースホルダー URL を置き換え、Gitee ミラーを追加、言語別に Bilibili 名をローカライズ、メールアドレスを更新
  - 32 ファイル変更、489 行追加、180 行削除

#### 設定とエントリ
- `0fc1693` - プログラムエントリとプロジェクト設定を更新
  - 2 ファイル変更、7 行追加

#### 権限システムリファクタリング
- `ea9179a` - 権限システム実装を改善
  - 5 ファイル変更、358 行追加、152 行削除

#### バグ修正
- `928a96d` - カレンダー計算実装を修正
  - 4 ファイル変更、12 行追加、12 行削除

#### AI とカレンダー
- `646813e` - AI クライアントファクトリー実装を改善
  - 2 ファイル変更、21 行追加、20 行削除

#### ローカリゼーション
- `7940d9c` - 韓国語ローカリゼーションサポートを追加
  - 7 ファイル変更、2424 行追加、10 行削除
- `4ff98ad` - ドキュメントをリファクタリング、多言語サポート
  - 81 ファイル変更、23818 行追加、1886 行削除

### 2026-04-20

#### コア機能完成
- `28905b5` - 完全な多言語サポート、AI クライアントファクトリー、権限システム、ローカリゼーション設定
  - マネージャー、エントリ、異なるログレベルを持つロギングシステム
  - トークン使用のクエリと追跡のためのトークン使用監査システム
  - 異なる AI プラットフォームを自動発見する AI クライアントファクトリー
  - 専用ストレージを持つパーミッションコールバックシステム
  - コンソールロガー実装
  - 英語と簡体字中国語の多言語サポート
  - WebSocket を備えたリアルタイムチャット用 WebUI メッセンジャー
  - ローカリゼーションによるデフォルトシリコンビーイングの強化
  - 39 ファイル変更、4670 行追加、175 行削除

### 2026-04-19

#### タイマーとカレンダー
- `c933fd8` - ローカリゼーション、タイマーシステム、Web ビューを更新しツールを追加
  - より良いローカリゼーションマネージャー
  - 定時タスクのスケジューリングシステム
  - AI 設定とコンテキスト管理
  - 32種のカレンダータイプをサポートするカレンダーツール
  - カレンダー API 用の Web コントローラー
  - タスク管理ツール
  - 46 ファイル変更、4018 行追加、975 行削除

**アーキテクチャ改善**
- スキンをより良くサポートするための Web ビューアーキテクチャの再設計
- より良いステータス処理を持つビーイング管理システムの改善

### 2026-04-18

- `9f585e1` - ローカリゼーション、タイマーシステム、Web ビューを更新しツールを追加
  - タイマーとスケジューリングの改善
  - 改善された UI コンポーネントを持つより良い Web ビュー
  - さらなるツール実装
  - 57 ファイル変更、3328 行追加、389 行削除

### 2026-04-17

- `9b71fcd` - コアモジュールを更新、zh-HK ドキュメント、ブロードキャストチャンネル、設定ツール、監査 Web ビューを追加
  - 複数のシリコンビーイングが一緒にチャットするブロードキャストチャンネル
  - 設定ツールシステム
  - 監査 Web ビュー
  - 繁体字中国語ドキュメント
  - 42 ファイル変更、3533 行追加、268 行削除

### 2026-04-16

- `5040f05` - コアとデフォルトモジュールを更新
  - モジュール最適化とバグ修正
  - 実装の更新と改善
  - 58 ファイル変更、9916 行追加、111 行削除

### 2026-04-15

- `3efab5f` - 複数モジュールを更新：AI、Chat、IM、Tools、Web、Localization、Storage
  - AI クライアント改善
  - チャットシステム強化
  - メッセンジャープロバイダー更新
  - ツールシステム最適化
  - Web インフラ改善
  - ローカリゼーション最適化
  - ストレージシステム更新
  - 33 ファイル変更、788 行追加、232 行削除

### 2026-04-14

- `4241a2f` - チャット機能基本完了、UI アップロード最適化
  - チャットシステム機能完了
  - ファイルアップロードの UI 最適化
  - 16 ファイル変更、1234 行追加、102 行削除

### 2026-04-13

- `c498c31` - コード更新
  - 一般的なコード改善と最適化
  - 32 ファイル変更、1045 行追加、546 行削除

### 2026-04-12

#### ドキュメントとローカリゼーション
- `2161002` - ドキュメントをリファクタリングしローカリゼーションを強化
  - 17 ファイル変更、982 行追加、92 行削除
- `03d94e4` - 設定システムとローカリゼーションを強化
  - 25 ファイル変更、1378 行追加、154 行削除
- `9976a35` - バージョン情報ページとローカリゼーションを追加
  - 14 ファイル変更、699 行追加、44 行削除

#### チャットと Web ビュー
- `0c8ccfc` - チャットシステム、ローカリゼーション、Web ビューを強化
  - 13 ファイル変更、402 行追加、56 行削除
- `a8f1342` - Web 通信層を再設計、WebSocket から SSE に切り替え
  - 27 ファイル変更、793 行追加、935 行削除

### 2026-04-11

#### ロギングシステム
- `e8fe259` - ロギングシステムとコード最適化を追加
  - 37 ファイル変更、624 行追加、91 行削除
- `f01c519` - ロギングシステムを追加、AI インターフェースと Web ビューを更新
  - 31 ファイル変更、1758 行追加、63 行削除

### 2026-04-10

- `4962924` - WebSocket ハンドラー、チャットビュー、メッセンジャーインタラクションを強化
  - コンテキストマネージャー改善
  - チャットシステム強化
  - メッセンジャープロバイダーインターフェース更新
  - WebUI プロバイダー再設計
  - JavaScript ビルダーとルーター更新
  - チャットビュー最適化
  - WebSocket ハンドラー改善
  - 9 ファイル変更、365 行追加、134 行削除

### 2026-04-09

- `f9302bf` - メッセンジャープロバイダーインターフェース、チャットシステム、Web UI インタラクションを強化
  - メッセンジャープロバイダーインターフェース拡張
  - チャットメッセージとシステム改善
  - コンテキストマネージャー最適化
  - デフォルトシリコンビーイング強化
  - Web UI チャットビュー改善
  - WebSocket ハンドラー更新
  - 10 ファイル変更、427 行追加、93 行削除

### 2026-04-07

- `6831ee8` - Web ビューと JavaScript ビルダーを再設計
  - 完全な Web コントローラー再設計
  - JavaScript ビルダーの完全書き直し
  - 全ビューコンポーネント更新
  - スキンシステム改善
  - ビュー基底クラスアーキテクチャ向上
  - 23 ファイル変更、2004 行追加、1983 行削除

### 2026-04-05

- `41e97fb` - 複数のコアモジュールと Web コントローラーを更新
  - コンテキストマネージャー改善
  - チャットシステムとセッション管理
  - サービスロケーター再設計
  - シリコンビーイング基底クラスとマネージャー更新
  - Web コントローラー全面更新（17コントローラー）
  - デフォルトシリコンビーイングファクトリー改善
  - 31 ファイル変更、681 行追加、326 行削除
- `67988d4` - Web UI モジュールを改善、エグゼキュータービューを追加、ビューとコアモジュールをクリーンアップ
  - 61 ファイル変更、3148 行追加、3726 行削除

### 2026-04-04

- `b58bb1c` - 初期化コントローラーを追加し Web モジュールを再設計
  - 初期化コントローラー
  - 設定モジュール再設計
  - ローカリゼーションモジュール更新
  - スキンシステム改善
  - ルーター強化
  - 29 ファイル変更、1269 行追加、289 行削除
- `f03ac0b` - Web UI モジュールを追加、メッセンジャー機能を改善
  - 60 ファイル変更、8481 行追加、165 行削除

### 2026-04-03

- `192e57b` - プロジェクト構造とコアランタイムコンポーネントを更新
  - 22 ファイル変更、446 行追加、179 行削除
- `59faec8` - コアとデフォルト実装を更新
  - 25 ファイル変更、3056 行追加、18 行削除
- `d488485` - 動的コンパイル機能とキュレーターツールモジュールを追加
  - 19 ファイル変更、1727 行追加、11 行削除
- `753d1d9` - セキュリティモジュールを追加、エグゼキューター、メッセンジャープロバイダー、ローカリゼーション、ツールを更新
  - 29 ファイル変更、2352 行追加、93 行削除
- `a378697` - フェーズ5完了 - ツールシステム + エグゼキューター
  - 41 ファイル変更、2651 行追加、363 行削除

### 2026-04-02

- `e6ad94b` - テスト中に設定ファイルを削除した際にチャット履歴のロードが失敗する問題を修正
  - 4 ファイル変更、49 行追加、45 行削除
- `daa56f5` - フェーズ4完了：永続メモリ（チャットシステム + メッセンジャーチャンネル）
  - 29 ファイル変更、2051 行追加、538 行削除

### 2026-04-01

- `bbe2dbb` - 設定ロードとチャットサービスメッセージルーティングを修正
  - 27 ファイル変更、1633 行追加、147 行削除
- `2fa6305` - フェーズ2を実装：メインループフレームワークとティックオブジェクトシステム
  - 9 ファイル変更、594 行追加、41 行削除
- `32b99a1` - フェーズ1を実装 - 基本チャット機能
  - 19 ファイル変更、1185 行追加
- `358e368` - 初回コミット：プロジェクトドキュメントとライセンス
  - 10 ファイル変更、1873 行追加
