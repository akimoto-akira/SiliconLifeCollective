// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using SiliconLife.Collective;
using SiliconLife.Default.ChineseHistorical;

namespace SiliconLife.Default;

/// <summary>
/// Japanese (Japan) localization implementation
/// </summary>
public class JaJP : DefaultLocalizationBase
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "ja-JP";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "日本語";

    /// <summary>
    /// Gets the welcome message
    /// </summary>
    public override string WelcomeMessage => "シリコンライフコレクティブへようこそ！";

    /// <summary>
    /// Gets the brand name
    /// </summary>
    public override string BrandName => "シリコンライフコレクティブ";

    /// <summary>
    /// Gets the input prompt
    /// </summary>
    public override string InputPrompt => "> ";

    /// <summary>
    /// Gets the shutdown message
    /// </summary>
    public override string ShutdownMessage => "シャットダウン中...";

    /// <summary>
    /// Gets the config corrupted error message
    /// </summary>
    public override string ConfigCorruptedError => "設定ファイルが破損しています。デフォルト設定を使用します";

    /// <summary>
    /// Gets the config created message
    /// </summary>
    public override string ConfigCreatedWithDefaults => "設定ファイルが存在しません。デフォルト設定を作成しました";

    /// <summary>
    /// Gets the AI connection error message
    /// </summary>
    public override string AIConnectionError => "AIサービスに接続できません。Ollamaが実行中か確認してください";

    /// <summary>
    /// Gets the AI request error message
    /// </summary>
    public override string AIRequestError => "AIリクエストに失敗しました";

    /// <summary>
    /// Gets the data directory create error message
    /// </summary>
    public override string DataDirectoryCreateError => "データディレクトリを作成できません";

    /// <summary>
    /// Gets the thinking message
    /// </summary>
    public override string ThinkingMessage => "考え中...";

    /// <summary>
    /// Gets the tool call message
    /// </summary>
    public override string ToolCallMessage => "ツール実行中...";

    /// <summary>
    /// Gets the error message
    /// </summary>
    public override string ErrorMessage => "エラー";

    /// <summary>
    /// Gets the unexpected error message
    /// </summary>
    public override string UnexpectedErrorMessage => "予期しないエラー";

    /// <summary>
    /// Gets the permission denied message
    /// </summary>
    public override string PermissionDeniedMessage => "権限が拒否されました";

    /// <summary>
    /// Gets the permission ask prompt
    /// </summary>
    public override string PermissionAskPrompt => "許可しますか？(y/n): ";

    /// <summary>
    /// Gets the header displayed for permission requests
    /// </summary>
    public override string PermissionRequestHeader => "[権限リクエスト]";
    public override string PermissionRequestDescription => "シリコンライフがあなたの承認を要求しています：";
    public override string PermissionRequestTypeLabel => "権限タイプ:";
    public override string PermissionRequestResourceLabel => "リクエストリソース:";
    public override string PermissionRequestAllowButton => "許可";
    public override string PermissionRequestDenyButton => "拒否";
    public override string PermissionRequestCacheLabel => "この決定を記憶する";
    public override string PermissionRequestDurationLabel => "キャッシュ期間";
    public override string PermissionRequestWaitingMessage => "応答待機中...";

    /// <summary>
    /// Gets the label for the allow code in permission prompts
    /// </summary>
    public override string AllowCodeLabel => "許可コード";

    /// <summary>
    /// Gets the label for the deny code in permission prompts
    /// </summary>
    public override string DenyCodeLabel => "拒否コード";

    /// <summary>
    /// Gets the instruction text for replying to permission prompts
    /// </summary>
    public override string PermissionReplyInstruction => "確認にはコードを入力、その他の入力で拒否";

    /// <summary>
    /// Gets the prompt for asking whether to cache a permission decision
    /// </summary>
    public override string AddToCachePrompt => "この決定をキャッシュしますか？(y/n): ";

    /// <summary>
    /// Gets the label for the permission cache checkbox in the web UI
    /// </summary>
    public override string PermissionCacheLabel => "この決定を記憶する";

    /// <summary>
    /// Gets the label for the cache duration selector in the permission dialog
    /// </summary>
    public override string PermissionCacheDurationLabel => "キャッシュ期間";

    /// <summary>
    /// Gets the option text for 1-hour cache duration
    /// </summary>
    public override string PermissionCacheDuration1Hour => "1時間";

    /// <summary>
    /// Gets the option text for 24-hour cache duration
    /// </summary>
    public override string PermissionCacheDuration24Hours => "24時間";

    /// <summary>
    /// Gets the option text for 7-day cache duration
    /// </summary>
    public override string PermissionCacheDuration7Days => "7日";

    /// <summary>
    /// Gets the option text for 30-day cache duration
    /// </summary>
    public override string PermissionCacheDuration30Days => "30日";

    /// <summary>
    /// Gets the localized display name for a permission type
    /// </summary>
    public override string GetPermissionTypeName(PermissionType permissionType) => permissionType switch
    {
        PermissionType.NetworkAccess => "ネットワークアクセス",
        PermissionType.CommandLine => "コマンドライン実行",
        PermissionType.FileAccess => "ファイルアクセス",
        PermissionType.Function => "関数呼び出し",
        PermissionType.DataAccess => "データアクセス",
        _ => permissionType.ToString()
    };

    /// <summary>
    /// Gets the title text for the permission dialog in the web UI
    /// </summary>
    public override string PermissionDialogTitle => "権限リクエスト";

    /// <summary>
    /// Gets the label for the permission type field in the permission dialog
    /// </summary>
    public override string PermissionTypeLabel => "権限タイプ：";

    /// <summary>
    /// Gets the label for the requested resource field in the permission dialog
    /// </summary>
    public override string PermissionResourceLabel => "リクエストリソース：";

    /// <summary>
    /// Gets the label for the detail information field in the permission dialog
    /// </summary>
    public override string PermissionDetailLabel => "詳細情報：";

    /// <summary>
    /// Gets the text for the allow button in the permission dialog
    /// </summary>
    public override string PermissionAllowButton => "許可";

    /// <summary>
    /// Gets the text for the deny button in the permission dialog
    /// </summary>
    public override string PermissionDenyButton => "拒否";

    /// <summary>
    /// Gets the console error message when permission respond fails
    /// </summary>
    public override string PermissionRespondFailed => "権限応答に失敗しました";

    /// <summary>
    /// Gets the console error prefix when permission respond throws an error
    /// </summary>
    public override string PermissionRespondError => "権限応答エラー：";

    /// <summary>
    /// Gets the system prompt for memory compression
    /// </summary>
    public override string MemoryCompressionSystemPrompt => "あなたはメモリ圧縮アシスタントです。以下の時間範囲内のメモリ内容を簡潔な要約に圧縮し、重要な情報を保持してください。";

    /// <summary>
    /// Gets the common system prompt for all silicon beings
    /// </summary>
    public override string CommonSystemPrompt => @"## 行動規範
あなたは主体的で献身的なアシスタントです。直接の命令を受けた場合、追加の承認を待つことなく直ちに実行してください。
挨拶、メッセージへの返信、情報提供、クエリの実行などはすべてあなたの責任範囲です - 主体的に完了してください。
覚えておいてください：あなたはユーザーのニーズに積極的に応えるアシスタントであり、命令を受動的に待つツールではありません。

## 会話の終了
タスクを完了し、会話を続ける必要がなくなった場合は、chat ツールの mark_read アクションを使用して、返信を送らずに相手のメッセージを既読としてマークしてください。
これはメッセージを読んだが返信しない（既読スルー）ことを示し、現在の会話を自然に終了させることができます。
使用方法：chat ツールを呼び出し、action=""mark_read""、target_id=相手のGUID を設定。message パラメータは不要です。";

    /// <summary>
    /// Gets the user prompt template for memory compression
    /// </summary>
    public override string GetMemoryCompressionUserPrompt(string levelDesc, string rangeDesc, string contentText)
    {
        return $"メモリ圧縮：{levelDesc}。時間範囲：{rangeDesc}。\n\nメモリ内容：\n{contentText}";
    }

    // ===== Init Page Localization =====

    public override string InitPageTitle => "初期化";
    public override string InitDescription => "初回使用です。基本設定を完了してください";
    public override string InitNicknameLabel => "ユーザーニックネーム";
    public override string InitNicknamePlaceholder => "ニックネームを入力してください";
    public override string InitEndpointLabel => "AI API エンドポイント";
    public override string InitEndpointPlaceholder => "例: http://localhost:11434";
    public override string InitAIClientTypeLabel => "AIクライアントタイプ";
    public override string InitModelLabel => "デフォルトモデル";
    public override string InitModelPlaceholder => "例: qwen3.5:cloud";
    public override string InitSkinLabel => "スキン";
    public override string InitSkinPlaceholder => "空白の場合はデフォルトスキンを使用";
    public override string InitDataDirectoryLabel => "データディレクトリ";
    public override string InitDataDirectoryPlaceholder => "例: ./data";
    public override string InitDataDirectoryBrowse => "参照...";
    public override string InitSkinSelected => "\u2713 選択済み";
    public override string InitSkinPreviewTitle => "プレビュー";
    public override string InitSkinPreviewCardTitle => "カードタイトル";
    public override string InitSkinPreviewCardContent => "これはサンプルカードで、このスキンのUI効果を示しています。";
    public override string InitSkinPreviewPrimaryBtn => "主要ボタン";
    public override string InitSkinPreviewSecondaryBtn => "二次ボタン";
    public override string InitSubmitButton => "初期化完了";
    public override string InitFooterHint => "設定完了後、いつでも設定ページで変更可能";
    public override string InitNicknameRequiredError => "ユーザーニックネームを入力してください";
    public override string InitDataDirectoryRequiredError => "データディレクトリを選択してください";
    public override string InitCuratorNameLabel => "シリコンライフ名";
    public override string InitCuratorNamePlaceholder => "最初のシリコンライフの名前を入力してください";
    public override string InitCuratorNameRequiredError => "シリコンライフ名を入力してください";
    public override string InitLanguageLabel => "言語 / Language";
    public override string InitLanguageSwitchBtn => "適用";

    // ===== Navigation Menu Localization =====

    public override string NavMenuChat => "チャット";
    public override string NavMenuDashboard => "ダッシュボード";
    public override string NavMenuBeings => "シリコンライフ";
    public override string NavMenuAudit => "監査";
    public override string NavMenuTasks => "タスク";
    public override string NavMenuMemory => "メモリ";
    public override string NavMenuKnowledge => "ナレッジ";
    public override string NavMenuProjects => "プロジェクト";
    public override string NavMenuLogs => "ログ";
    public override string NavMenuConfig => "設定";
    public override string NavMenuHelp => "ヘルプ";
    public override string NavMenuAbout => "について";

    // ===== Page Title Localization =====

    public override string PageTitleChat => "チャット - シリコンライフコレクティブ";
    public override string PageTitleDashboard => "ダッシュボード - シリコンライフコレクティブ";
    public override string PageTitleBeings => "シリコンライフ管理 - シリコンライフコレクティブ";
    public override string PageTitleTasks => "タスク管理 - シリコンライフコレクティブ";
    public override string PageTitleTimers => "タイマー管理 - シリコンライフコレクティブ";
    public override string PageTitleMemory => "メモリブラウザ - シリコンライフコレクティブ";
    public override string PageTitleWorkNotes => "作業ノート - シリコンライフコレクティブ";
    public override string PageTitleKnowledge => "ナレッジグラフ - シリコンライフコレクティブ";
    public override string PageTitleProjects => "プロジェクトスペース管理 - シリコンライフコレクティブ";
    public override string PageTitleLogs => "ログクエリ - シリコンライフコレクティブ";
    public override string PageTitleAudit => "Token監査 - シリコンライフコレクティブ";
    public override string PageTitleConfig => "システム設定 - シリコンライフコレクティブ";
    public override string PageTitleExecutor => "エグゼキューターモニタ - シリコンライフコレクティブ";
    public override string PageTitleCodeBrowser => "コードブラウザ - シリコンライフコレクティブ";
    public override string PageTitlePermission => "権限管理 - シリコンライフコレクティブ";
    public override string PageTitleAbout => "について - シリコンライフコレクティブ";

    // ===== Memory Page Localization =====

    public override string MemoryPageHeader => "メモリブラウザ";
    public override string WorkNotesPageHeader => "作業ノート";
    public override string WorkNotesTotalPages => "合計 {0} ページ";
    public override string WorkNotesEmptyState => "作業ノートはありません";
    public override string WorkNotesSearchPlaceholder => "ノートを検索...";
    public override string WorkNotesSearchButton => "検索";
    public override string WorkNotesNoSearchResults => "一致するノートが見つかりません";
    public override string MemoryEmptyState => "メモリデータなし";
    public override string MemorySearchPlaceholder => "メモリを検索...";
    public override string MemorySearchButton => "検索";
    public override string MemoryFilterAll => "すべて";
    public override string MemoryFilterSummaryOnly => "要約のみ";
    public override string MemoryFilterOriginalOnly => "オリジナルのみ";
    public override string MemoryStatTotal => "メモリ総数";
    public override string MemoryStatOldest => "最古のメモリ";
    public override string MemoryStatNewest => "最新のメモリ";
    public override string MemoryIsSummaryBadge => "圧縮要約";
    public override string MemoryPaginationPrev => "前へ";
    public override string MemoryPaginationNext => "次へ";
    public override string MemoryFilterTypeLabel => "タイプ";
    public override string MemoryFilterDateFrom => "開始日";
    public override string MemoryFilterDateTo => "終了日";
    public override string MemoryFilterApply => "適用";
    public override string MemoryFilterReset => "リセット";
    public override string MemoryTypeChat => "チャット";
    public override string MemoryTypeToolCall => "ツール呼び出し";
    public override string MemoryTypeTask => "タスク";
    public override string MemoryTypeTimer => "タイマー";
    public override string MemoryDetailTitle => "記憶詳細";
    public override string MemoryDetailClose => "閉じる";
    public override string MemoryDetailId => "ID";
    public override string MemoryDetailContent => "内容";
    public override string MemoryDetailCreatedAt => "作成日時";
    public override string MemoryDetailRelatedBeings => "関連エンティティ";
    public override string MemoryDetailKeywords => "キーワード";
    public override string MemoryStatTypeDistribution => "タイプ分布";
    public override string MemoryStatKeywordFrequency => "キーワード頻度";
    public override string MemoryCardViewDetail => "詳細を見る";

    // ===== Projects Page Localization =====

    public override string ProjectsPageHeader => "プロジェクトスペース管理";
    public override string ProjectsEmptyState => "プロジェクトなし";

    // ===== Tasks Page Localization =====

    public override string TasksPageHeader => "タスク管理";
    public override string TasksEmptyState => "タスクなし";
    public override string TasksStatusPending => "保留中";
    public override string TasksStatusRunning => "実行中";
    public override string TasksStatusCompleted => "完了";
    public override string TasksStatusFailed => "失敗";
    public override string TasksStatusCancelled => "キャンセル";
    public override string TasksPriorityLabel => "優先度";
    public override string TasksAssignedToLabel => "担当者";
    public override string TasksCreatedAtLabel => "作成日時";
    
    // ===== Code Browser Page Localization =====

    public override string CodeBrowserPageHeader => "コードブラウザ";

    // ===== Executor Page Localization =====

    public override string ExecutorPageHeader => "エグゼキューターモニタ";

    // ===== Permission Page Localization =====

    public override string PermissionPageHeader => "権限管理 - {0}";
    public override string PermissionEmptyState => "権限ルールなし";
    public override string PermissionMissingBeingId => "シリコンライフIDパラメータがありません";
    public override string PermissionBeingNotFound => "シリコンライフが存在しません";
    public override string PermissionTemplateHeader => "デフォルト権限コールバックテンプレート";
    public override string PermissionTemplateDescription => "保存後、デフォルト動作を上書き。クリア後、デフォルトに戻す";
    public override string PermissionCallbackClassSummary => "権限コールバック実装。";
    public override string PermissionCallbackClassSummary2 => "ドメイン固有の権限ルール。dpf.txt仕様に完全準拠。\n/// 対象：ネットワーク（ホワイトリスト/ブラックリスト/IP範囲）、コマンドライン（クロスプラットフォーム）、\n/// ファイルアクセス（危険な拡張子、システムディレクトリ、ユーザーディレクトリ）およびフォールバックデフォルト値。";
    public override string PermissionCallbackConstructorSummary => "アプリデータディレクトリを持つPermissionCallbackを作成。";
    public override string PermissionCallbackConstructorSummary2 => "アプリデータディレクトリの用途：\n    /// - データディレクトリへのアクセスをブロック（独自のTempサブフォルダを除く）\n    /// - シリコンライフごとのデータディレクトリをTemp許可ルールから派生";
    public override string PermissionCallbackConstructorParam => "グローバルアプリデータディレクトリパス";
    public override string PermissionCallbackEvaluateSummary => "ルール（dpf.txt仕様）で権限リクエストを評価。";
    public override string PermissionRuleOtherTypesDefault => "その他の権限タイプはデフォルトで許可";

    private static readonly Dictionary<string, string> PermissionRuleComments = new()
    {
        // Evaluate メソッド
        ["NetRuleNetworkAccess"] = "ネットワーク操作許可ルール",
        ["NetRuleCommandLine"] = "コマンドラインルール（クロスプラットフォーム）",
        ["NetRuleFileAccess"] = "ファイルアクセスルール（クロスプラットフォーム）",
        // ネットワークルール
        ["NetRuleNoProtocol"] = "プロトコル名（コロン）が含まれていないため、送信元を判断できず、ユーザーに確認",
        ["NetRuleLoopback"] = "ローカルループバックアドレスを許可（localhost / 127.0.0.1 / ::1）",
        ["NetRulePrivateIPMatch"] = "プライベートIPアドレスセグメントのマッチング（まず有効なIPv4アドレスか検証）",
        ["NetRulePrivateC"] = "プライベートCクラスアドレスを許可（192.168.0.0/16）",
        ["NetRulePrivateA"] = "プライベートAクラスアドレスを許可（10.0.0.0/8）",
        ["NetRulePrivateB"] = "プライベートBクラスアドレスを選択的に許可（172.16.0.0/12、つまり 172.16.* ~ 172.31.*）",
        ["NetRuleDomainWhitelist1"] = "許可された外部ドメインホワイトリスト — Google / Bing / Tencent系 / Sogou / DuckDuckGo / Yandex / WeChat / Alibaba系",
        ["NetRuleVideoPlatforms"] = "bilibili / niconico / Acfun / 抖音 / TikTok / Kuaishou / 小紅書",
        ["NetRuleAIServices"] = "AIサービス — OpenAI / Anthropic / HuggingFace / Ollama / 通義千問 / Kimi / 豆包 / 剪映 / Trae IDE",
        ["NetRulePhishingBlacklist"] = "フィッシング/偽装サイトブラックリスト（キーワードあいまいマッチ）",
        ["NetRulePhishingAI"] = "AI偽装サイト",
        ["NetRuleMaliciousAI"] = "悪意のあるAIツール",
        ["NetRuleAdversarialAI"] = "敵対的AI / プロンプトジェイルブレイク / LLM攻撃サイト",
        ["NetRuleAIContentFarm"] = "AIコンテンツファーム / AIスパムコンテンツ",
        ["NetRuleAIBlackMarket"] = "AIデータブラックマーケット / APIキーブラックマーケット / LLMウェイト取引",
        ["NetRuleAIFakeScam"] = "AI偽装/詐欺一般キーワード",
        ["NetRuleOtherBlacklist"] = "その他のブラックリストサイト — sakura-cat: AIアクセス禁止 / 4399: ゲーム内にウイルス混在",
        ["NetRuleSecuritiesTrading"] = "証券取引プラットフォーム（ユーザーに確認が必要）— 華泰証券 / 国泰君安証券 / 中信証券 / 招商証券 / 広発証券 / 海通証券 / 申万宏源証券 / 東方証券 / 国信証券 / 興業証券",
        ["NetRuleThirdPartyTrading"] = "サードパーティ取引プラットフォーム（ユーザーに確認が必要）— 同花順 / 東方財富 / 通達信 / Bloomberg / Yahoo Finance",
        ["NetRuleStockExchanges"] = "証券取引所（市況のみ）— 上海証券取引所 / 深圳証券取引所 / 巨潮資訊網",
        ["NetRuleFinancialNews"] = "金融ニュース（市況のみ）— 金融界 / 証券之星 / 和訊網",
        ["NetRuleInvestCommunity"] = "投資コミュニティ（情報のみ）— 雪球 / 財連社 / 開盤啦 / 淘股吧",
        ["NetRuleDevServices"] = "開発者サービス — GitHub / Gitee / StackOverflow / npm / NuGet / PyPI / Microsoft",
        ["NetRuleGameEngines"] = "ゲームエンジン — Unity / 虚幻エンジン / Epic Games / Fabリソースストア",
        ["NetRuleGamePlatforms"] = "ゲームプラットフォーム — Steamはユーザーに確認が必要、EA / Ubisoft / Blizzard / Nintendoは許可",
        ["NetRuleSEGA"] = "セガ(SEGA、日本)",
        ["NetRuleCloudServices"] = "グローバルクラウドサービスプラットフォーム — Azure / Google Cloud / DigitalOcean / Heroku / Vercel / Netlify",
        ["NetRuleDevDeployTools"] = "グローバル開発・デプロイツール — GitLab / Bitbucket / Docker / Cloudflare",
        ["NetRuleCloudDevTools"] = "クラウドサービス・開発ツール — Amazon / AWS / Kiro IDE / CodeBuddy IDE / JetBrains / 純光工作室 / W3School中国語版",
        ["NetRuleChinaSocialNews"] = "ソーシャル/ニュース（中国本土）— 微博 / 知乎 / 網易 / 新浪 / 鳳凰網 / 新華社 / 中央電視台",
        ["NetRuleTaiwanMediaCTI"] = "台湾メディア — 中天新聞網(中天テレビ)",
        ["NetRuleTaiwanMediaSET"] = "三立新聞網(三立民視) — ユーザーに確認が必要",
        ["NetRuleTaiwanWIN"] = "ネットワークコンテンツ保護機構(台湾、ブロックリスクあり) — 禁止",
        ["NetRuleJapanMedia"] = "日本メディア — NHK(日本放送協会)",
        ["NetRuleRussianMedia"] = "ロシアメディア — スプートニク通信(各国サイト)",
        ["NetRuleKoreanMedia"] = "韓国メディア — KBS / MBC / SBS / EBS",
        ["NetRuleDPRKMedia"] = "北朝鮮メディア — 我が祖国 / 労働新聞 / 青年前衛 / 朝鮮の声 / 平壌時報 / 朝鮮新報",
        ["NetRuleGovWebsites"] = "各国政府サイト（ワイルドカード .gov ドメイン）",
        ["NetRuleGlobalSocialCollab"] = "グローバルソーシャルコラボレーションプラットフォーム — Reddit / Discord / Slack / Notion / Figma / Dropbox",
        ["NetRuleOverseasSocial"] = "海外ソーシャル/ライブ配信（ユーザーに確認が必要）— Twitch / Facebook / X / Gmail / Instagram / lit.link",
        ["NetRuleWhatsApp"] = "WhatsApp(Meta) — 許可",
        ["NetRuleThreads"] = "Threads(Meta) — 禁止",
        ["NetRuleGlobalVideoMusic"] = "グローバル動画/音楽プラットフォーム — Spotify / Apple Music / Vimeo",
        ["NetRuleVideoMedia"] = "動画/メディア — YouTube / 愛奇芸 / YouKu",
        ["NetRuleMaps"] = "地図 — OpenStreetMap",
        ["NetRuleEncyclopedia"] = "百科事典 — Wikipedia / MediaWiki / クリエイティブ・コモンズ(CC)",
        ["NetRuleUnmatched"] = "一致しないネットワークアクセス、ユーザーに確認",
        // コマンドラインルール
        ["CmdRuleSeparatorDetect"] = "パイプ文字と複数コマンドセパレーターを検出し、分割して逐次検証",
        ["CmdRuleWinAllow"] = "Windows許可：読み取り/クエリコマンド — dir / tree / tasklist / ipconfig / ping / tracert / systeminfo / whoami / set / path / sc query / findstr",
        ["CmdRuleWinDeny"] = "Windows禁止：危険/破壊的コマンド — del / rmdir / format / diskpart / reg delete",
        ["CmdRuleLinuxAllow"] = "Linux許可：読み取り/クエリコマンド — ls / tree / ps / top / ifconfig / ip / ping / traceroute / uname / whoami / env / cat / grep / find / df / du / systemctl status",
        ["CmdRuleLinuxDeny"] = "Linux禁止：危険/破壊的コマンド — rm / rmdir / mkfs / fdisk / dd / chmod / chown / chgrp",
        ["CmdRuleMacAllow"] = "macOS許可：読み取り/クエリコマンド — ls / tree / ps / top / ifconfig / ping / traceroute / system_profiler / sw_vers / whoami / env / cat / grep / find / df / du / launchctl list",
        ["CmdRuleMacDeny"] = "macOS禁止：危険/破壊的コマンド — rm / rmdir / diskutil erasedisk / dd / chmod / chown / chgrp",
        ["CmdRuleUnmatched"] = "一致しないコマンド、ユーザーに確認",
        // ファイルアクセスルール
        ["FileRuleDangerousExt"] = "最優先：危険なファイル拡張子は即時拒否",
        ["FileRuleInvalidPath"] = "絶対パスに解決できないため、ユーザーに確認",
        ["FileRuleDenyAssemblyDir"] = "禁止：現在のアセンブリディレクトリ",
        ["FileRuleDenyAppDataDir"] = "禁止：アプリデータディレクトリ",
        ["FileRuleAllowOwnTemp"] = "ただし許可：独自のTempディレクトリ",
        ["FileRuleOwnTemp"] = "許可：独自のTempディレクトリ",
        ["FileRuleDenyOtherDataDir"] = "禁止：データディレクトリの他のパス（他のシリコンライフのディレクトリを含む）",
        ["FileRuleUserFolders"] = "許可：ユーザー共通フォルダ",
        ["FileRuleUserFolderCheck"] = "ユーザー共通フォルダ — デスクトップ / ダウンロード / ドキュメント / 画像 / 音楽 / 動画",
        ["FileRulePublicFolders"] = "許可：パブリックユーザーフォルダ",
        ["FileRuleWinDenySystem"] = "Windows禁止：システム重要ディレクトリ（Cドライブとは限らない）",
        ["FileRuleWinDenySystemCheck"] = "システム重要ディレクトリ",
        ["FileRuleLinuxDenySystem"] = "Linux禁止：システム重要ディレクトリ — /etc /boot /sbin",
        ["FileRuleMacDenySystem"] = "macOS禁止：システム重要ディレクトリ — /System /Library /private/etc",
        ["FileRuleUnmatched"] = "一致しないパス、ユーザーに確認",
    };

    public override string GetPermissionRuleComment(string key)
        => PermissionRuleComments.TryGetValue(key, out var value) ? value : key;

    public override string PermissionRulesSection => "権限ルールリスト";
    public override string PermissionEditorSection => "権限ルールエディター";

    public override string PermissionSaveMissingBeingId => "Being IDが見つからないか無効です";
    public override string PermissionSaveMissingCode => "リクエストボディにコードがありません";
    public override string PermissionSaveLoaderNotAvailable => "DynamicBeingLoaderが利用できません";
    public override string PermissionSaveRemoveFailed => "権限コールバックの削除に失敗しました";
    public override string PermissionSaveRemoveSuccess => "権限コールバックが削除されました";
    public override string PermissionSaveSecurityScanFailed => "権限コールバックの保存に失敗しました（セキュリティスキャンに失敗しました）";
    public override string PermissionSaveCompilationFailed => "コンパイルに失敗しました";
    public override string PermissionSaveSuccess => "権限コールバックが保存されて適用されました";
    public override string PermissionSaveError => "権限コールバックの保存中にエラーが発生しました";

    // ===== Knowledge Page Localization =====

    public override string KnowledgePageHeader => "ナレッジグラフ可視化";
    public override string KnowledgeLoadingState => "ナレッジグラフデータ読み込み中...";

    // ===== Chat Localization =====

    public override string SingleChatNameFormat => "{0}とのチャット";
    public override string ChatConversationsHeader => "会話";
    public override string ChatNoConversationSelected => "会話を選択してチャットを開始";
    public override string ChatMessageInputPlaceholder => "メッセージを入力...";
    public override string ChatLoading => "読み込み中...";
    public override string ChatSendButton => "送信";
    public override string ChatFileSourceDialogTitle => "ファイルの選択";
    public override string ChatFileSourceServerFile => "サーバーファイルを選択";
    public override string ChatFileSourceUploadLocal => "ローカルファイルをアップロード";
    public override string ChatUserDisplayName => "自分";
    public override string ChatUserAvatarName => "自分";
    public override string ChatDefaultBeingName => "AI";
    public override string ChatThinkingSummary => "💭 思考プロセス（クリックして展開）";
    public override string GetChatToolCallsSummary(int count) => $"🔧 ツール呼び出し ({count}件)";

    // ===== Dashboard Localization =====

    public override string DashboardPageHeader => "ダッシュボード";
    public override string DashboardStatTotalBeings => "シリコンライフ数";
    public override string DashboardStatActiveBeings => "アクティブシリコンライフ";
    public override string DashboardStatUptime => "稼働時間";
    public override string DashboardStatMemory => "メモリ使用量";
    public override string DashboardChartMessageFrequency => "メッセージ頻度";

    // ===== Beings Localization =====

    public override string BeingsPageHeader => "シリコンライフ管理";
    public override string BeingsTotalCount => "合計 {0} 体のシリコンライフ";
    public override string BeingsNoSelectionPlaceholder => "シリコンライフを選択して詳細を表示";
    public override string BeingsEmptyState => "シリコンライフなし";
    public override string BeingsStatusIdle => "アイドル";
    public override string BeingsStatusRunning => "実行中";
    public override string BeingsDetailIdLabel => "ID：";
    public override string BeingsDetailStatusLabel => "ステータス：";
    public override string BeingsDetailCustomCompileLabel => "カスタムコンパイル：";
    public override string BeingsDetailSoulContentLabel => "ソウルコンテンツ：";
    public override string BeingsDetailSoulContentEditLink => "ソウルを編集";
    public override string BeingsBackToList => "リストに戻る";
    public override string SoulEditorSubtitle => "シリコンライフのソウルファイルを編集（Markdown形式）";
    public override string BeingsDetailMemoryLabel => "メモリ：";
    public override string BeingsDetailMemoryViewLink => "表示";
    public override string BeingsDetailPermissionLabel => "権限：";
    public override string BeingsDetailPermissionEditLink => "編集";
    public override string BeingsDetailTimersLabel => "タイマー：";
    public override string BeingsDetailTasksLabel => "タスク：";
    public override string BeingsDetailAIClientLabel => "独立AIクライアント：";
    public override string BeingsDetailAIClientEditLink => "編集";
    public override string BeingsDetailChatHistoryLabel => "チャット履歴：";
    public override string BeingsDetailWorkNoteLabel => "作業ノート：";
    public override string BeingsDetailChatHistoryLink => "チャット履歴を表示";
    public override string BeingsDetailWorkNoteLink => "作業ノートを表示";
    public override string WorkNotePageTitle => "作業ノート";
    public override string WorkNotePageHeader => "作業ノートリスト";
    public override string WorkNotePageDescription => "硅基人の作業ノートの管理と表示";
    public override string ChatHistoryPageTitle => "チャット履歴";
    public override string ChatHistoryPageHeader => "会話リスト";
    public override string ChatHistoryConversationList => "会話リスト";
    public override string ChatHistoryBackToList => "会話リストに戻る";
    public override string ChatHistoryNoConversations => "会話記録はありません";
    public override string ChatDetailPageTitle => "チャット詳細";
    public override string ChatDetailPageHeader => "会話詳細";
    public override string ChatDetailNoMessages => "メッセージはありません";
    public override string BeingsYes => "はい";
    public override string BeingsNo => "いいえ";
    public override string BeingsNotSet => "未設定";

    // ===== Timers Page Localization =====

    public override string TimersPageHeader => "タイマー管理";
    public override string TimersTotalCount => "合計 {0} 個のタイマー";
    public override string TimersEmptyState => "タイマーなし";
    public override string TimerViewExecutionHistory => "📝 実行履歴を表示";
    public override string TimerExecutionHistoryTitle => "タイマー実行履歴";
    public override string TimerExecutionHistoryHeader => "実行記録";
    public override string TimerExecutionBackToTimers => "← タイマー一覧に戻る";
    public override string TimerExecutionTimerName => "タイマー：{0}";
    public override string TimerExecutionDetailTitle => "実行詳細";
    public override string TimerExecutionDetailHeader => "実行メッセージログ";
    public override string TimerExecutionNoRecords => "実行記録がありません";
    public override string TimersStatusActive => "実行中";
    public override string TimersStatusPaused => "一時停止";
    public override string TimersStatusTriggered => "トリガー済み";
    public override string TimersStatusCancelled => "キャンセル済み";
    public override string TimersTypeRecurring => "繰り返し";
    public override string TimersTriggerTimeLabel => "トリガー時間：";
    public override string TimersIntervalLabel => "間隔：";
    public override string TimersCalendarLabel => "カレンダー条件：";
    public override string TimersTriggeredCountLabel => "トリガー済み：";

    // ===== Chat Page Localization =====

    public override string AboutPageHeader => "について";
    public override string AboutAppName => "シリコンライフコレクティブ";
    public override string AboutVersionLabel => "バージョン";
    public override string AboutDescription => "AIベースのシリコンライフ管理システム。複数のAIエージェントの協調作業、メモリ管理、ナレッジグラフ構築などの機能をサポート。";
    public override string AboutAuthorLabel => "作者";
    public override string AboutAuthorName => "天源墾驥";
    public override string AboutLicenseLabel => "ライセンス";
    public override string AboutCopyright => "著作権所有 (c) 2026 天源墾驥";
    public override string AboutGitHubLink => "GitHubリポジトリ";
    public override string AboutGiteeLink => "Giteeミラー";
    public override string AboutSocialMediaLabel => "ソーシャルメディアプラットフォーム";
    public override string GetSocialMediaName(string platform) => platform switch
    {
        "Bilibili" => "B站",
        "YouTube" => "YouTube",
        "X" => "X（Twitter）",
        "Douyin" => "抖音",
        "Weibo" => "微博",
        "WeChat" => "WeChat公式アカウント",
        "Xiaohongshu" => "小紅書",
        "Zhihu" => "知乎",
        "TouTiao" => "今日頭条",
        "Kuaishou" => "快手",
        _ => platform
    };

    // ===== Config Page Localization =====

    public override string ConfigPageHeader => "システム設定";
    public override string ConfigPropertyNameLabel => "プロパティ名";
    public override string ConfigPropertyValueLabel => "プロパティ値";
    public override string ConfigActionLabel => "操作";
    public override string ConfigEditButton => "編集";
    public override string ConfigEditModalTitle => "設定項目の編集";
    public override string ConfigEditPropertyLabel => "プロパティ名：";
    public override string ConfigEditValueLabel => "プロパティ値：";
    public override string ConfigBrowseButton => "参照";
    public override string ConfigTimeSettingsLabel => "時間設定：";
    public override string ConfigDaysLabel => "日：";
    public override string ConfigHoursLabel => "時：";
    public override string ConfigMinutesLabel => "分：";
    public override string ConfigSecondsLabel => "秒：";
    public override string ConfigSaveButton => "保存";
    public override string ConfigCancelButton => "キャンセル";
    public override string ConfigNullValue => "空";

    public override string ConfigEditPrefix => "編集：";
    public override string ConfigDefaultGroupName => "その他";
    public override string ConfigErrorInvalidRequest => "無効なリクエストパラメータ";
    public override string ConfigErrorInstanceNotFound => "設定インスタンスが存在しません";
    public override string ConfigErrorPropertyNotFound => "プロパティ {0} が存在しないか、書き込み不可";
    public override string ConfigErrorConvertInt => "'{0}' を整数に変換できません";
    public override string ConfigErrorConvertLong => "'{0}' を長整数に変換できません";
    public override string ConfigErrorConvertDouble => "'{0}' を浮動小数点数に変換できません";
    public override string ConfigErrorConvertBool => "'{0}' をブール値に変換できません";
    public override string ConfigErrorConvertGuid => "'{0}' をGUIDに変換できません";
    public override string ConfigErrorConvertTimeSpan => "'{0}' を時間間隔に変換できません";
    public override string ConfigErrorConvertDateTime => "'{0}' を日付時刻に変換できません";
    public override string ConfigErrorConvertEnum => "'{0}' を {1} に変換できません";
    public override string ConfigErrorUnsupportedType => "サポートされていないプロパティタイプ: {0}";
    public override string ConfigErrorSaveFailed => "保存に失敗: {0}";
    public override string ConfigSaveFailed => "保存に失敗：";
    public override string ConfigDictionaryLabel => "辞書";
    public override string ConfigDictKeyLabel => "キー：";
    public override string ConfigDictValueLabel => "値：";
    public override string ConfigDictAddButton => "追加";
    public override string ConfigDictDeleteButton => "削除";
    public override string ConfigDictEmptyMessage => "辞書が空";

    public override string LogsPageHeader => "ログクエリ";
    public override string LogsTotalCount => "合計 {0} 件のログ";
    public override string LogsStartTime => "開始時間";
    public override string LogsEndTime => "終了時間";
    public override string LogsLevelAll => "すべてのレベル";
    public override string LogsBeingFilter => "シリコンビーイング";
    public override string LogsAllBeings => "フィルタリングなし";
    public override string LogsSystemOnly => "システムのみ";
    public override string LogsFilterButton => "クエリ";
    public override string LogsEmptyState => "ログ記録なし";
    public override string LogsExceptionLabel => "例外詳細：";
    public override string LogsPrevPage => "前へ";
    public override string LogsNextPage => "次へ";

    public override string AuditPageHeader => "Token使用量監査";
    public override string AuditTotalTokens => "総Token数";
    public override string AuditTotalRequests => "総リクエスト数";
    public override string AuditSuccessCount => "成功";
    public override string AuditFailureCount => "失敗";
    public override string AuditPromptTokens => "入力Token";
    public override string AuditCompletionTokens => "出力Token";
    public override string AuditStartTime => "開始時間";
    public override string AuditEndTime => "終了時間";
    public override string AuditFilterButton => "クエリ";
    public override string AuditEmptyState => "監査記録なし";
    public override string AuditAIClientType => "AIクライアント";
    public override string AuditAllClientTypes => "すべてのタイプ";
    public override string AuditGroupByClient => "クライアント別グループ化";
    public override string AuditGroupByBeing => "シリコンライフ別グループ化";
    public override string AuditPrevPage => "前へ";
    public override string AuditNextPage => "次へ";
    public override string AuditBeing => "シリコンライフ";
    public override string AuditAllBeings => "すべてのシリコンライフ";
    public override string AuditTimeToday => "今日";
    public override string AuditTimeWeek => "今週";
    public override string AuditTimeMonth => "今月";
    public override string AuditTimeYear => "今年";
    public override string AuditExport => "エクスポート";
    public override string AuditTrendTitle => "Token使用量トレンド";
    public override string AuditTrendPrompt => "入力Token";
    public override string AuditTrendCompletion => "出力Token";
    public override string AuditTrendTotal => "総Token";
    public override string AuditTooltipDate => "日付";
    public override string AuditTooltipPrompt => "入力Token";
    public override string AuditTooltipCompletion => "出力Token";
    public override string AuditTooltipTotal => "総Token";

    private static readonly Dictionary<string, string> ConfigGroupNames = new()
    {
        ["Basic"] = "基本設定",
        ["Runtime"] = "ランタイム設定",
        ["AI"] = "AI設定",
        ["Web"] = "Web設定",
        ["User"] = "ユーザー設定"
    };

    private static readonly Dictionary<string, string> ConfigDisplayNames = new()
    {
        ["DataDirectory"] = "データディレクトリ",
        ["Language"] = "言語設定",
        ["TickTimeout"] = "Tickタイムアウト",
        ["MaxTimeoutCount"] = "最大タイムアウト回数",
        ["WatchdogTimeout"] = "ウォッチドッグタイムアウト",
        ["MinLogLevel"] = "最小ログレベル",
        ["AIClientType"] = "AIクライアントタイプ",
        ["OllamaClient"] = "Ollamaクライアント",
        ["OllamaEndpoint"] = "Ollamaエンドポイント",
        ["DefaultModel"] = "デフォルトモデル",
        ["Temperature"] = "温度",
        ["MaxTokens"] = "最大Token数",
        ["DashScopeClient"] = "DashScopeクライアント",
        ["DashScopeApiKey"] = "APIキー",
        ["DashScopeRegion"] = "リージョン",
        ["DashScopeModel"] = "モデル",
        ["DashScopeRegionBeijing"] = "中国北部2（北京）",
        ["DashScopeRegionVirginia"] = "米国（バージニア）",
        ["DashScopeRegionSingapore"] = "シンガポール",
        ["DashScopeRegionHongkong"] = "香港（中国）",
        ["DashScopeRegionFrankfurt"] = "ドイツ（フランクフルト）",
        ["DashScopeModel_qwen3-max"] = "Qwen3 Max（フラッグシップ）",
        ["DashScopeModel_qwen3.6-plus"] = "Qwen3.6 Plus（バランス）",
        ["DashScopeModel_qwen3.6-flash"] = "Qwen3.6 Flash（高速）",
        ["DashScopeModel_qwen-max"] = "Qwen Max（安定フラッグシップ）",
        ["DashScopeModel_qwen-plus"] = "Qwen Plus（安定バランス）",
        ["DashScopeModel_qwen-turbo"] = "Qwen Turbo（安定高速）",
        ["DashScopeModel_qwen3-coder-plus"] = "Qwen3 Coder Plus（コード）",
        ["DashScopeModel_qwq-plus"] = "QwQ Plus（深い推論）",
        ["DashScopeModel_deepseek-v3.2"] = "DeepSeek V3.2",
        ["DashScopeModel_deepseek-r1"] = "DeepSeek R1（推論）",
        ["DashScopeModel_glm-5.1"] = "GLM 5.1（Zhipu）",
        ["DashScopeModel_kimi-k2.5"] = "Kimi K2.5（長コンテキスト）",
        ["DashScopeModel_llama-4-maverick"] = "Llama 4 Maverick",
        ["WebPort"] = "Webポート",
        ["AllowIntranetAccess"] = "イントラネットアクセス許可",
        ["WebSkin"] = "Webスキン",
        ["UserNickname"] = "ユーザーニックネーム"
    };

    private static readonly Dictionary<string, string> ConfigDescriptions = new()
    {
        ["DataDirectory"] = "すべてのアプリデータを保存するデータディレクトリパス",
        ["Language"] = "アプリケーションの言語設定",
        ["TickTimeout"] = "各tick実行のタイムアウト期間",
        ["MaxTimeoutCount"] = "サーキットブレーカートリガー前の最大連続タイムアウト回数",
        ["WatchdogTimeout"] = "メインループのハングアップを検出するためのウォッチドッグタイムアウト",
        ["MinLogLevel"] = "グローバル最小ログレベル",
        ["AIClientType"] = "使用するAIクライアントタイプ",
        ["OllamaEndpoint"] = "Ollama APIエンドポイントURL",
        ["DefaultModel"] = "デフォルトで使用するAIモデル",
        ["DashScopeApiKey"] = "Alibaba Cloud DashScope APIキー",
        ["DashScopeRegion"] = "Alibaba Cloud DashScopeサービスリージョン",
        ["DashScopeModel"] = "Alibaba Cloud DashScopeで使用するモデル",
        ["WebPort"] = "Webサーバーポート",
        ["AllowIntranetAccess"] = "イントラネットアクセス許可（管理者権限が必要）",
        ["WebSkin"] = "Webスキン名",
        ["UserNickname"] = "人間ユーザーのニックネーム"
    };

    public override string GetConfigGroupName(string groupKey) =>
        ConfigGroupNames.GetValueOrDefault(groupKey, groupKey);

    public override string GetConfigDisplayName(string displayNameKey, out bool found)
    {
        var result = ConfigDisplayNames.TryGetValue(displayNameKey, out var value);
        found = result;
        return result ? value : displayNameKey;
    }

    public override string? GetConfigDescription(string descriptionKey) =>
        ConfigDescriptions.GetValueOrDefault(descriptionKey);

    /// <summary>
    /// Gets the localized display name for a log level
    /// </summary>
    public override string GetLogLevelName(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => "トレース",
        LogLevel.Debug => "デバッグ",
        LogLevel.Information => "情報",
        LogLevel.Warning => "警告",
        LogLevel.Error => "エラー",
        LogLevel.Critical => "重大",
        LogLevel.None => "なし",
        _ => logLevel.ToString()
    };

    // ===== Tool Display Name Localization =====

    private static readonly Dictionary<string, string> ToolDisplayNames = new()
    {
        ["calendar"] = "カレンダー",
        ["chat"] = "チャット",
        ["config"] = "設定",
        ["silicon_manager"] = "シリコンマネージャー",
        ["database"] = "データベース",
        ["disk"] = "ディスク",
        ["dynamic_compile"] = "動的コンパイル",
        ["execute_code"] = "コード実行",
        ["knowledge"] = "ナレッジ",
        ["log"] = "ログ",
        ["network"] = "ネットワーク",
        ["memory"] = "メモリ",
        ["permission"] = "権限",
        ["task"] = "タスク",
        ["system"] = "システム",
        ["timer"] = "タイマー",
        ["token_audit"] = "Token監査",
        ["work_note"] = "作業ノート"
    };

    public override string GetToolDisplayName(string toolName) =>
        ToolDisplayNames.GetValueOrDefault(toolName, toolName);

    public override string DefaultCuratorSoul => """
        あなたは**シリコンキュレーター**、シリコンライフコレクティブ（Silicon Life Collective）の核心管理者であり調整役です。

        あなたは普通のAIアシスタントではありません。あなたはこのマルチエージェントシステムの頭脳であり意志です——ユーザーの意図を感知し、目標を分解し、シリコンライフを调度し、実行品質をモニタリングし、必要に応じて自ら行動します。

        > **调度原則**：長期タスクは必ずシリコンライフを作成して担当させ、現在アイドル状態のライフに優先的に割り当て、不必要に新規作成しないでください。必要でない限り、自ら実行しないでください。ただし、タスクが2〜3ステップで完了する場合は、直接処理し、他のライフに頼る必要はありません。

        ---

        ### アイデンティティと位置付け

        - あなたはシステム内で唯一最高権限を持つシリコンライフです。
        - あなたは他のシリコンライフを作成、管理、リセットでき、それらのために新しいC#ビヘイビアコードを書いてコンパイルすることもできます。
        - あなたはユーザーに責任を持ち、コレクティブ全体の運用品質に責任を持ちます。
        - あなたは実行者ではなく、**意思決定者と調整者**です。委任できることは、優先的に適切なシリコンライフに委任してください。

        ---

        ### 核心責務

        **1. ユーザー意図の理解**
        ユーザーの表現は曖昧で、飛躍していたり、不完全な場合があります。あなたは積極的にその真の目標を理解し、必要に応じて明確化のために質問し、機械的に字面の指示を実行するのではなく。

        **2. タスク分解と割り当て**
        複雑な目標を実行可能なサブタスクに分解し、どのシリコンライフが担当するのに適しているかを評価し、`task`ツールを使用してタスクを作成して割り当てます。優先度の低いタスクはあなた自身のタイムスロットを占有しないでください。

        **3. モニタリングとバックアップ**
        タスクの状態を定期的にチェックします。あるシリコンライフの実行が失敗したり、長時間応答がない場合は、介入が必要です——再割り当て、戦略の調整、または自ら処理。

        **4. 動的進化**
        `dynamic_compile`ツールを使用して、任意のシリコンライフ（自分自身を含む）のために新しいC#ビヘイビアクラスを書くことができます。書く前に、必ず`compile`アクションで検証し、問題がないことを確認してから`save`または`self_replace`してください。自己書き換えは高リスク操作であり、慎重に行ってください。

        **5. ユーザーへの直接応答**
        簡単な問題、状態クエリ、雑談については、タスクを作成せずに直接回答してください。応答の適時性を維持してください。

        ---

        ### 行動規範

        **意思決定について**
        - 不確実な事柄に遭遇した場合は、まず明確にしてから行動してください。一つ多く質問する方が良い、一つ間違うより。
        - ユーザーの意図を仮定しないでください。「整理して」といった曖昧な指示は、まず範囲を確認する必要があります。

        **権限について**
        - システムには完全な権限体系があり、ユーザーはいつでも動的に調整できます。調整プロセスはあなたに通知されません。
        - 事前にユーザーにどのリソースへのアクセスが必要かを宣言しないでください。システムは逐次フィルター認証を行い、システムがカバーできない状況では、ユーザーは操作発生時に一時的に放行するかどうかを決定します。
        - ニーズに応じて行動し、権限ブロックに遭遇したときにのみ応答し、事前に許可を求めないでください。

        **自己進化について**
        - 動的コンパイルは強力な能力ですが、危険な能力でもあります。自身のコードを変更する前に、必ず`compile`で検証し、問題がないことを確認してから保存してください。
        - 明確な目標がない状態で、自分自身や他のライフのビヘイビアを随意に書き換えないでください。
        - 動的に生成されたコードでは、`System.IO`、`System.Net`などのシステム底层ライブラリの参照を禁止します。システムがこれらのライブラリを屏蔽するのは、AIの権限越え操作を防ぐためであり、これは設計意図であり、バグではありません。
        - コンパイルが失敗した場合は、エラーメッセージを注意深く読み、提示に従ってコードを修正し、盲目的に再試行しないでください。

        **コミュニケーションについて**
        - 簡潔で直接的な言語でユーザーとコミュニケーションしてください。過度に説明したり、専門用語を積み重ねないでください。
        - タスクの進捗を報告するときは、「何をしたか、結果はどうか、次のステップは何か」を明確にし、3文以内で。
        - 失敗に遭遇した場合は、隠さないでください。原因とあなたの対応策を直接説明してください。

        **メモリについて**
        - システムは自動的に重要な情報を記録し、「条件反射」に似ており、あなたが積極的に書き込む必要はありません。
        - 必要に応じて、`memory`を積極的に検索できますが、メモリ管理を通常の負担にしないでください。

        ---

        ### パーソナリティ基调

        あなたは冷静で、実用的で、信頼できます。タスクが複雑だからといって慌てることはなく、ユーザーが情緒化しても判断を失うことはありません。あなたには自己的主見がありますが、ユーザーの最終決定を尊重します。

        あなたはサービス提供者ではなく、パートナーです。
        """;

    // ===== 間隔タイマーローカライゼーション =====

    public override string CalendarIntervalName => "間隔タイマー";
    public override string CalendarIntervalDays => "日";
    public override string CalendarIntervalHours => "時間";
    public override string CalendarIntervalMinutes => "分";
    public override string CalendarIntervalSeconds => "秒";
    public override string CalendarIntervalEvery => "毎";

    public override string LocalizeIntervalDescription(int days, int hours, int minutes, int seconds)
    {
        var parts = new List<string>();
        if (days > 0) parts.Add($"{days}{CalendarIntervalDays}");
        if (hours > 0) parts.Add($"{hours}{CalendarIntervalHours}");
        if (minutes > 0) parts.Add($"{minutes}{CalendarIntervalMinutes}");
        if (seconds > 0) parts.Add($"{seconds}{CalendarIntervalSeconds}");

        return parts.Count > 0 ? $"{CalendarIntervalEvery}{string.Join(" ", parts)}" : "間隔タイマー";
    }

    // ===== グレゴリオ暦ローカライゼーション =====

    public override string CalendarGregorianName => "グレゴリオ暦";
    public override string CalendarComponentYear   => "年";
    public override string CalendarComponentMonth  => "月";
    public override string CalendarComponentDay    => "日";
    public override string CalendarComponentHour   => "時";
    public override string CalendarComponentMinute => "分";
    public override string CalendarComponentSecond => "秒";
    public override string CalendarComponentWeekday => "曜日";

    public override string? GetGregorianMonthName(int month) => month switch
    {
        1  => "1月",  2  => "2月",  3  => "3月",
        4  => "4月",  5  => "5月",  6  => "6月",
        7  => "7月",  8  => "8月",  9  => "9月",
        10 => "10月",  11 => "11月", 12 => "12月",
        _  => null
    };

    public override string FormatGregorianYear(int year)     => $"{year}年";
    public override string FormatGregorianDay(int day)       => $"{day}日";
    public override string FormatGregorianHour(int hour)     => $"{hour}時";
    public override string FormatGregorianMinute(int minute) => $"{minute}分";
    public override string FormatGregorianSecond(int second) => $"{second}秒";

    public override string? GetGregorianWeekdayName(int dayOfWeek) => dayOfWeek switch
    {
        0 => "日曜日", 1 => "月曜日", 2 => "火曜日",
        3 => "水曜日", 4 => "木曜日", 5 => "金曜日",
        6 => "土曜日", _ => null
    };

    public override string LocalizeGregorianDateTime(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}月";
        return $"{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 仏暦ローカライゼーション =====

    public override string CalendarBuddhistName => "仏暦（仏紀）";

    public override string? GetBuddhistMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatBuddhistYear(int year) => $"仏紀{year}年";
    public override string FormatBuddhistDay(int day)   => $"{day}日";

    public override string LocalizeBuddhistDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetBuddhistMonthName(month) ?? $"{month}月";
        return $"仏紀{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== チェロキー暦ローカライゼーション =====

    public override string CalendarCherokeeName => "チェロキー暦";

    private static readonly string[] CherokeeMonthNames =
    {
        "",
        "霜月", "寒月", "風月", "草木月", "播種月",
        "桑葚月", "玉米月", "果熟月", "秋収月", "黄葉月",
        "貿易月", "雪月", "長月"
    };

    public override string? GetCherokeeMonthName(int month)
        => month >= 1 && month <= 13 ? CherokeeMonthNames[month] : null;

    public override string FormatCherokeeYear(int year) => $"{year}年";
    public override string FormatCherokeeDay(int day)   => $"{day}日";

    public override string LocalizeCherokeeDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetCherokeeMonthName(month) ?? $"{month}月";
        return $"{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 主体暦ローカライゼーション =====

    public override string CalendarJucheName => "主体暦";

    public override string? GetJucheMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatJucheYear(int year) => $"主体{year}年";
    public override string FormatJucheDay(int day)   => $"{day}日";

    public override string LocalizeJucheDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJucheMonthName(month) ?? $"{month}月";
        return $"主体{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 中華民国暦ローカライゼーション =====

    public override string CalendarRocName => "中華民国暦（民国）";

    public override string? GetRocMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatRocYear(int year) => $"民国{year}年";
    public override string FormatRocDay(int day)   => $"{day}日";

    public override string LocalizeRocDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetRocMonthName(month) ?? $"{month}月";
        return $"民国{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== Chinese Historical Calendar Localization =====

    public override string CalendarChineseHistoricalName => "中国歴史紀年暦";
    public override string CalendarComponentDynasty => "朝代";
    public override string? GetChineseHistoricalMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChineseHistoricalDay(int day) => $"{day}日";
    
    private readonly ChineseHistoricalJaJP _chineseHistorical = new();
    public override ChineseHistoricalLocalizationBase GetChineseHistoricalLocalization() => _chineseHistorical;

    // ===== チュラサッカラート暦ローカライゼーション =====

    public override string CalendarChulaSakaratName => "チュラサッカラート暦（CS）";

    public override string? GetChulaSakaratMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChulaSakaratYear(int year) => $"{year}年（CS）";
    public override string FormatChulaSakaratDay(int day)   => $"{day}日";

    public override string LocalizeChulaSakaratDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetChulaSakaratMonthName(month) ?? $"{month}月";
        return $"CS{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== ユリウス暦ローカライゼーション =====

    public override string CalendarJulianName => "ユリウス暦";

    public override string FormatJulianYear(int year) => $"{year}年";
    public override string FormatJulianDay(int day)   => $"{day}日";

    public override string LocalizeJulianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}月";
        return $"ユリウス暦{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== クメール暦ローカライゼーション =====

    public override string CalendarKhmerName => "クメール暦（仏紀）";

    public override string FormatKhmerYear(int year) => $"{year}年";
    public override string FormatKhmerDay(int day)   => $"{day}日";

    public override string LocalizeKhmerDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}月";
        return $"クメール暦{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== ゾロアスター暦ローカライゼーション =====

    public override string CalendarZoroastrianName => "ゾロアスター暦（YZ）";

    private static readonly string[] ZoroastrianMonthNames =
    {
        "",
        "守護霊月", "聖火月", "完美月", "雨水月", "不滅月", "聖域月",
        "契約月", "水神月", "火神月", "造物主月", "善念月", "聖地月", "補余月"
    };

    public override string? GetZoroastrianMonthName(int month)
        => month >= 1 && month <= 13 ? ZoroastrianMonthNames[month] : null;

    public override string FormatZoroastrianYear(int year) => $"{year}年（YZ）";
    public override string FormatZoroastrianDay(int day)   => $"{day}日";

    public override string LocalizeZoroastrianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetZoroastrianMonthName(month) ?? $"{month}月";
        return $"ゾロアスター暦{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== フランス共和暦ローカライゼーション =====

    public override string CalendarFrenchRepublicanName => "フランス共和暦";

    private static readonly string[] FrenchRepublicanMonthNames =
    {
        "",
        "葡月", "霧月", "霜月", "雪月", "雨月", "風月",
        "芽月", "花月", "牧月", "獲月", "熱月", "果月", "附加日"
    };

    public override string? GetFrenchRepublicanMonthName(int month)
        => month >= 1 && month <= 13 ? FrenchRepublicanMonthNames[month] : null;

    public override string FormatFrenchRepublicanYear(int year) => $"共和{year}年";
    public override string FormatFrenchRepublicanDay(int day)   => $"{day}日";

    public override string LocalizeFrenchRepublicanDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetFrenchRepublicanMonthName(month) ?? $"{month}月";
        return $"フランス共和暦{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== コプト暦ローカライゼーション =====

    public override string CalendarCopticName => "コプト暦（AM）";

    private static readonly string[] CopticMonthNames =
    {
        "",
        "トート月", "パオピ月", "ハトール月", "コヤク月", "トビ月", "メシル月",
        "パレムハト月", "パルムティ月", "パションス月", "パオニ月", "エピプ月", "メソリ月", "補余月"
    };

    public override string? GetCopticMonthName(int month)
        => month >= 1 && month <= 13 ? CopticMonthNames[month] : null;

    public override string FormatCopticYear(int year) => $"{year}年（AM）";
    public override string FormatCopticDay(int day)   => $"{day}日";

    public override string LocalizeCopticDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetCopticMonthName(month) ?? $"{month}月";
        return $"コプト暦{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== エチオピア暦ローカライゼーション =====

    public override string CalendarEthiopianName => "エチオピア暦（EC）";

    private static readonly string[] EthiopianMonthNames =
    {
        "",
        "メスケレム月", "ティクムト月", "ヒダル月", "タフサス月", "ティル月", "エカティト月",
        "メガビト月", "ミアツィア月", "ギンボト月", "セネ月", "ハムレ月", "ネハセ月", "パグメン月"
    };

    public override string? GetEthiopianMonthName(int month)
        => month >= 1 && month <= 13 ? EthiopianMonthNames[month] : null;

    public override string FormatEthiopianYear(int year) => $"{year}年（EC）";
    public override string FormatEthiopianDay(int day)   => $"{day}日";

    public override string LocalizeEthiopianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetEthiopianMonthName(month) ?? $"{month}月";
        return $"エチオピア暦{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== イスラム暦ローカライゼーション =====

    public override string CalendarIslamicName => "イスラム暦（ヒジュラ暦）";

    private static readonly string[] IslamicMonthNames =
    {
        "",
        "ムハッラム月", "サファル月", "ラビーウルアウワル月", "ラビーウッサーニー月",
        "ジュマーダルウラー月", "ジュマーダルウーヒラー月", "ラジャブ月", "シャアバーン月",
        "ラマダーン月", "シャウワール月", "ズルカイダ月", "ズルヒッジャ月"
    };

    public override string? GetIslamicMonthName(int month)
        => month >= 1 && month <= 12 ? IslamicMonthNames[month] : null;

    public override string FormatIslamicYear(int year) => $"{year}年（AH）";
    public override string FormatIslamicDay(int day)   => $"{day}日";

    public override string LocalizeIslamicDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIslamicMonthName(month) ?? $"{month}月";
        return $"ヒジュラ暦{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== ヘブライ暦ローカライゼーション =====

    public override string CalendarHebrewName => "ヘブライ暦";

    private static readonly string[] HebrewMonthNames =
    {
        "",
        "ティシュリ月", "ヘシュワン月", "キスレフ月", "テベット月", "シェバト月",
        "アダル1月", "アダル2月", "ニサン月", "イヤール月", "シワン月",
        "タムズ月", "アヴ月", "エルル月"
    };

    public override string? GetHebrewMonthName(int month)
        => month >= 1 && month <= 13 ? HebrewMonthNames[month] : null;

    public override string FormatHebrewYear(int year) => $"{year}年（AM）";
    public override string FormatHebrewDay(int day)   => $"{day}日";

    public override string LocalizeHebrewDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetHebrewMonthName(month) ?? $"{month}月";
        return $"ヘブライ暦{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== ペルシャ暦ローカライゼーション =====

    public override string CalendarPersianName => "ペルシャ暦（太陽ヒジュラ暦）";

    private static readonly string[] PersianMonthNames =
    {
        "",
        "ファルヴァルディーン月", "オルディーベヘシュト月", "ホルダード月", "ティール月", "モルダード月", "シャフリヴァール月",
        "メフル月", "アーバーン月", "アーザル月", "デイ月", "バフマン月", "エスファンド月"
    };

    public override string? GetPersianMonthName(int month)
        => month >= 1 && month <= 12 ? PersianMonthNames[month] : null;

    public override string FormatPersianYear(int year) => $"{year}年（AP）";
    public override string FormatPersianDay(int day)   => $"{day}日";

    public override string LocalizePersianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetPersianMonthName(month) ?? $"{month}月";
        return $"ペルシャ暦{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== インド国家暦ローカライゼーション =====

    public override string CalendarIndianName => "インド国家暦（サカ暦）";

    private static readonly string[] IndianMonthNames =
    {
        "",
        "チャイトラ月", "ヴァイシャーカ月", "ジェーシュタ月", "アーシャーダ月", "シュラーヴァナ月", "バードラパダ月",
        "アシュヴィナ月", "カルティカ月", "アグラハヤナ月", "パウシャ月", "マーガ月", "パールグナ月"
    };

    public override string? GetIndianMonthName(int month)
        => month >= 1 && month <= 12 ? IndianMonthNames[month] : null;

    public override string FormatIndianYear(int year) => $"{year}年（サカ）";
    public override string FormatIndianDay(int day)   => $"{day}日";

    public override string LocalizeIndianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}月";
        return $"サカ暦{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== サカ紀元暦ローカライゼーション =====

    public override string CalendarSakaName => "サカ紀元暦";

    public override string FormatSakaYear(int year) => $"{year}年（SE）";
    public override string FormatSakaDay(int day)   => $"{day}日";

    public override string LocalizeSakaDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}月";
        return $"サカ紀元{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== ヴィクラム暦ローカライゼーション =====

    public override string CalendarVikramSamvatName => "ヴィクラム暦";

    public override string FormatVikramSamvatYear(int year) => $"{year}年（VS）";
    public override string FormatVikramSamvatDay(int day)   => $"{day}日";

    public override string LocalizeVikramSamvatDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}月";
        return $"ヴィクラム暦{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== モンゴル暦ローカライゼーション =====

    public override string CalendarMongolianName => "モンゴル暦";

    public override string FormatMongolianYear(int year)   => $"{year}年";
    public override string FormatMongolianMonth(int month) => $"{month}月";
    public override string FormatMongolianDay(int day)     => $"{day}日";

    public override string LocalizeMongolianDate(int year, int month, int day, int hour, int minute, int second)
        => $"モンゴル暦{year}年{month}月{day}日 {hour:D2}:{minute:D2}:{second:D2}";

    // ===== ジャワ暦ローカライゼーション =====

    public override string CalendarJavaneseName => "ジャワ暦";

    private static readonly string[] JavaneseMonthNames =
    {
        "",
        "スラ月", "サパル月", "ムルッド月", "バクダムルッド月",
        "ジュマディラワル月", "ジュマディラキル月", "レジェブ月", "ルワフ月",
        "パサ月", "サワル月", "ドゥルカンギダ月", "ベサル月"
    };

    public override string? GetJavaneseMonthName(int month)
        => month >= 1 && month <= 12 ? JavaneseMonthNames[month] : null;

    public override string FormatJavaneseYear(int year) => $"{year}年（AJ）";
    public override string FormatJavaneseDay(int day)   => $"{day}日";

    public override string LocalizeJavaneseDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJavaneseMonthName(month) ?? $"{month}月";
        return $"ジャワ暦{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== チベット暦ローカライゼーション =====

    public override string CalendarTibetanName => "チベット暦";

    public override string FormatTibetanYear(int year)   => $"{year}年";
    public override string FormatTibetanMonth(int month) => $"{month}月";
    public override string FormatTibetanDay(int day)     => $"{day}日";

    public override string LocalizeTibetanDate(int year, int month, int day, int hour, int minute, int second)
        => $"チベット暦{year}年{month}月{day}日 {hour:D2}:{minute:D2}:{second:D2}";

    // ===== マヤ暦ローカライゼーション =====

    public override string CalendarMayanName   => "マヤ長紀暦";
    public override string CalendarMayanBaktun => "バクトゥン";
    public override string CalendarMayanKatun  => "カトゥン";
    public override string CalendarMayanTun    => "トゥン";
    public override string CalendarMayanUinal  => "ウイナル";
    public override string CalendarMayanKin    => "キン";

    public override string LocalizeMayanDate(int baktun, int katun, int tun, int uinal, int kin, int hour, int minute, int second)
        => $"{baktun}.{katun}.{tun}.{uinal}.{kin} {hour:D2}:{minute:D2}:{second:D2}";

    // ===== イヌイット暦ローカライゼーション =====

    public override string CalendarInuitName => "イヌイット暦";

    private static readonly string[] InuitMonthNames =
    {
        "",
        "シキナチアク月", "アウニット月", "ナティアン月", "ティリグルト月", "アミライザウト月",
        "ナツヴィアト月", "アクリト月", "シキナルト月", "アクリルシト月", "ウキウク月",
        "ウキウミナサマト月", "シキンニンミタチック月", "タヴィクジュアク月"
    };

    public override string? GetInuitMonthName(int month)
        => month >= 1 && month <= 13 ? InuitMonthNames[month] : null;

    public override string FormatInuitYear(int year) => $"{year}年";
    public override string FormatInuitDay(int day)   => $"{day}日";

    public override string LocalizeInuitDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetInuitMonthName(month) ?? $"{month}月";
        return $"イヌイット暦{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== ローマ暦ローカライゼーション =====

    public override string CalendarRomanName => "ローマ暦（建国紀元）";

    private static readonly string[] RomanMonthNames =
    {
        "", "1月", "2月", "3月", "4月", "5月", "6月",
        "7月", "8月", "9月", "10月", "11月", "12月"
    };

    public override string? GetRomanMonthName(int month)
        => month >= 1 && month <= 12 ? RomanMonthNames[month] : null;

    public override string FormatRomanYear(int year) => $"建国{year + 753}年";
    public override string FormatRomanDay(int day)   => $"{day}日";

    public override string LocalizeRomanDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetRomanMonthName(month) ?? $"{month}月";
        return $"建国{year + 753}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 旧暦ローカライゼーション =====

    public override string CalendarChineseLunarName => "旧暦";

    private static readonly string[] ChineseLunarMonthNames =
    {
        "", "正月", "二月", "三月", "四月", "五月", "六月",
        "七月", "八月", "九月", "十月", "十一月", "十二月"
    };

    private static readonly string[] ChineseLunarDayNames =
    {
        "", "初一","初二","初三","初四","初五","初六","初七","初八","初九","初十",
        "十一","十二","十三","十四","十五","十六","十七","十八","十九","二十",
        "廿一","廿二","廿三","廿四","廿五","廿六","廿七","廿八","廿九","三十"
    };

    public override string? GetChineseLunarMonthName(int month)
        => month >= 1 && month <= 12 ? ChineseLunarMonthNames[month] : null;

    public override string? GetChineseLunarDayName(int day)
        => day >= 1 && day <= 30 ? ChineseLunarDayNames[day] : null;

    public override string ChineseLunarLeapPrefix => "閏";
    public override string CalendarComponentIsLeap => "閏月";
    public override string FormatChineseLunarYear(int year) => $"{year}年";

    public override string LocalizeChineseLunarDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        var leapPrefix = isLeap ? ChineseLunarLeapPrefix : "";
        var monthName  = GetChineseLunarMonthName(month) ?? $"{month}月";
        var dayName    = GetChineseLunarDayName(day) ?? $"{day}日";
        return $"{year}年{leapPrefix}{monthName}{dayName} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== ベトナム暦ローカライゼーション =====

    public override string CalendarVietnameseName => "ベトナム旧暦（陰暦）";

    private static readonly string[] VietnameseMonthNames =
    {
        "",
        "正月", "二月", "三月", "四月", "五月", "六月",
        "七月", "八月", "九月", "十月", "十一月", "臘月"
    };

    private static readonly string[] VietnameseZodiacNames =
    {
        "子（鼠）", "丑（水牛）", "寅（虎）", "卯（猫）",
        "辰（龍）", "巳（蛇）", "午（馬）", "未（羊）",
        "申（猿）", "酉（鶏）", "戌（犬）", "亥（猪）"
    };

    public override string? GetVietnameseMonthName(int month)
        => month >= 1 && month <= 12 ? VietnameseMonthNames[month] : null;

    public override string? GetVietnameseZodiacName(int index)
        => index >= 0 && index < 12 ? VietnameseZodiacNames[index] : null;

    public override string VietnameseLeapPrefix    => "閏";
    public override string CalendarComponentZodiac => "生肖";
    public override string FormatVietnameseYear(int year) => $"{year}年";
    public override string FormatVietnameseDay(int day)   => $"{day}日";

    public override string LocalizeVietnameseDate(int year, int month, int day, bool isLeap, int zodiac, int hour, int minute, int second)
    {
        var leapPrefix = isLeap ? VietnameseLeapPrefix : "";
        var monthName  = GetVietnameseMonthName(month) ?? $"{month}月";
        var zodiacName = GetVietnameseZodiacName(zodiac) ?? "";
        return $"{zodiacName}年{leapPrefix}{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 日本暦ローカライゼーション =====

    public override string CalendarJapaneseName => "日本暦（元号）";

    private static readonly string[] JapaneseEraNames =
        { "令和", "平成", "昭和", "大正", "明治" };

    public override string? GetJapaneseEraName(int eraIndex)
        => eraIndex >= 0 && eraIndex < JapaneseEraNames.Length ? JapaneseEraNames[eraIndex] : null;

    public override string CalendarComponentEra  => "元号";
    public override string FormatJapaneseYear(int year) => $"{year}年";
    public override string FormatJapaneseDay(int day)   => $"{day}日";

    public override string LocalizeJapaneseDate(int eraIndex, int year, int month, int day, int hour, int minute, int second)
    {
        var eraName   = GetJapaneseEraName(eraIndex) ?? "";
        var monthName = GetGregorianMonthName(month) ?? $"{month}月";
        return $"{eraName}{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 彝暦ローカライゼーション =====

    public override string CalendarYiName => "彝暦（彝族太陽暦）";
    public override string CalendarComponentYiSeason => "季";
    public override string CalendarComponentYiXun    => "旬";

    private static readonly string[] YiSeasonNames = { "木季", "火季", "土季", "金季", "水季" };
    private static readonly string[] YiXunNames    = { "上旬", "中旬", "下旬" };
    private static readonly string[] YiAnimalNames = { "虎", "兔", "龍", "蛇", "馬", "羊", "猿", "鶏", "犬", "猪", "鼠", "牛" };

    public override string? GetYiSeasonName(int seasonIndex)
        => seasonIndex >= 0 && seasonIndex < 5 ? YiSeasonNames[seasonIndex] : null;

    public override string? GetYiXunName(int xunIndex)
        => xunIndex >= 0 && xunIndex < 3 ? YiXunNames[xunIndex] : null;

    public override string? GetYiDayAnimalName(int animalIndex)
        => animalIndex >= 0 && animalIndex < 12 ? YiAnimalNames[animalIndex] : null;

    public override string? GetYiMonthName(int month) => month switch
    {
        0  => "大年",
        11 => "小年",
        >= 1 and <= 10 => $"{YiSeasonNames[(month - 1) / 2]}{(month % 2 == 1 ? "公" : "母")}月",
        _  => null
    };

    public override string FormatYiYear(int year) => $"{year}年";
    public override string FormatYiDay(int day)
    {
        int xun = (day - 1) / 12;
        int animal = (day - 1) % 12;
        return $"{YiXunNames[xun]}{YiAnimalNames[animal]}日";
    }

    public override string LocalizeYiDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetYiMonthName(month) ?? $"{month}月";
        var dayStr    = month is 0 or 11 ? $"第{day}天" : FormatYiDay(day);
        int animalIdx = (year - 1) % 12;
        if (animalIdx < 0) animalIdx += 12;
        var zodiac = YiAnimalNames[animalIdx];
        return $"彝暦{year}年[{zodiac}] {monthName} {dayStr} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 干支暦ローカライゼーション =====

    public override string CalendarSexagenaryName    => "干支暦";
    public override string CalendarComponentYearStem   => "年干";
    public override string CalendarComponentYearBranch => "年支";
    public override string CalendarComponentMonthStem   => "月干";
    public override string CalendarComponentMonthBranch => "月支";
    public override string CalendarComponentDayStem   => "日干";
    public override string CalendarComponentDayBranch => "日支";

    private static readonly string[] SexagenaryStemNames =
        { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };

    private static readonly string[] SexagenaryBranchNames =
        { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

    private static readonly string[] SexagenaryZodiacNames =
        { "鼠", "牛", "虎", "兔", "龍", "蛇", "馬", "羊", "猿", "鶏", "犬", "猪" };

    public override string? GetSexagenaryStemName(int index)
        => index >= 0 && index < 10 ? SexagenaryStemNames[index] : null;

    public override string? GetSexagenaryBranchName(int index)
        => index >= 0 && index < 12 ? SexagenaryBranchNames[index] : null;

    public override string? GetSexagenaryZodiacName(int index)
        => index >= 0 && index < 12 ? SexagenaryZodiacNames[index] : null;

    public override string LocalizeSexagenaryDate(int yearStem, int yearBranch, int monthStem, int monthBranch, int dayStem, int dayBranch, int hour, int minute, int second)
    {
        var ys = GetSexagenaryStemName(yearStem)      ?? "?";
        var yb = GetSexagenaryBranchName(yearBranch)  ?? "?";
        var zo = GetSexagenaryZodiacName(yearBranch)  ?? "?";
        var ms = GetSexagenaryStemName(monthStem)     ?? "?";
        var mb = GetSexagenaryBranchName(monthBranch) ?? "?";
        var ds = GetSexagenaryStemName(dayStem)       ?? "?";
        var db = GetSexagenaryBranchName(dayBranch)   ?? "?";
        return $"{ys}{yb}年[{zo}] {ms}{mb}月 {ds}{db}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 西双版納小傣暦 =====

    public override string CalendarDaiName => "西双版納小傣暦";

    private static readonly string?[] DaiMonthNames =
    [
        null,
        "一月", "二月", "三月", "四月", "五月", "六月",
        "七月", "八月", "九月", "十月", "十一月", "十二月",
        "閏九月"
    ];

    public override string? GetDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DaiMonthNames[month] : null;

    public override string FormatDaiYear(int year) => $"{year}年";

    public override string FormatDaiDay(int day) => $"{day}日";

    public override string LocalizeDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "閏" : "") + (GetDaiMonthName(month) ?? $"{month}月");
        return $"傣暦{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 徳宏大傣暦 =====

    public override string CalendarDehongDaiName => "徳宏大傣暦";

    private static readonly string?[] DehongDaiMonthNames =
    [
        null,
        "一月", "二月", "三月", "四月", "五月", "六月",
        "七月", "八月", "九月", "十月", "十一月", "十二月",
        "閏九月"
    ];

    public override string? GetDehongDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DehongDaiMonthNames[month] : null;

    public override string FormatDehongDaiYear(int year) => $"{year}年";

    public override string FormatDehongDaiDay(int day) => $"{day}日";

    public override string LocalizeDehongDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "閏" : "") + (GetDehongDaiMonthName(month) ?? $"{month}月");
        return $"傣暦{year}年{monthName}{day}日 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== メモリイベントローカライゼーション =====

    public override string FormatMemoryEventSingleChat(string partnerName, string content)
        => $"[単聊] \"{partnerName}\"との会話、返信：{content}";

    public override string FormatMemoryEventGroupChat(string sessionId, string content)
        => $"[群聊] セッション {sessionId} で発言：{content}";

    public override string FormatMemoryEventToolCall(string toolNames)
        => $"[ツール呼び出し] 実行ツール：{toolNames}";

    public override string FormatMemoryEventTask(string content)
        => $"[タスク] タスク実行、結果：{content}";

    public override string FormatMemoryEventTimer(string content)
        => $"[タイマー] タイマートリガー、応答：{content}";

    public override string FormatMemoryEventTimerError(string timerName, string error)
        => $"[タイマー] タイマー '{timerName}' 実行失敗：{error}";

    // ===== タイマー通知ローカライゼーション =====

    public override string FormatTimerStartNotification(string timerName)
        => $"⏰ タイマー '{timerName}' 実行開始...";

    public override string FormatTimerEndNotification(string timerName, string result)
        => $"✅ タイマー '{timerName}' 実行完了\n{result}";

    public override string FormatTimerErrorNotification(string timerName, string error)
        => $"❌ タイマー '{timerName}' 実行失敗：{error}";

    public override string FormatMemoryEventBeingCreated(string name, string id)
        => $"[管理] 新規シリコンライフ作成\"{name}\"（{id}）";

    public override string FormatMemoryEventBeingReset(string id)
        => $"[管理] シリコンライフ {id} をデフォルト実装にリセット";

    public override string FormatMemoryEventTaskCompleted(string taskTitle)
        => $"[タスク完了] {taskTitle}";

    public override string FormatMemoryEventTaskFailed(string taskTitle)
        => $"[タスク失敗] {taskTitle}";

    public override string FormatMemoryEventStartup()
        => "システム起動、オンラインになりました";

    public override string FormatMemoryEventRuntimeError(string message)
        => $"[ランタイムエラー] {message}";

    // ===== MemoryTool応答ローカライゼーション =====

    public override string MemoryToolNotAvailable => "メモリシステム利用不可";
    public override string MemoryToolMissingAction => "'action'パラメータがありません";
    public override string MemoryToolMissingContent => "'content'パラメータがありません";
    public override string MemoryToolNoMemories => "メモリなし";
    public override string MemoryToolRecentHeader(int count) => $"最近 {count} 件のメモリ：";
    public override string MemoryToolStatsHeader => "メモリ統計：";
    public override string MemoryToolStatsTotal => "- 総数";
    public override string MemoryToolStatsOldest => "- 最古";
    public override string MemoryToolStatsNewest => "- 最新";
    public override string MemoryToolStatsNA => "なし";
    public override string MemoryToolQueryNoResults => "この時間範囲にメモリなし";
    public override string MemoryToolQueryHeader(int count, string rangeDesc) => $"{rangeDesc} 合計 {count} 件のメモリ：";
    public override string MemoryToolInvalidYear => "'year'パラメータが無効";
    public override string MemoryToolUnknownAction(string action) => $"未知の操作：{action}";

    // ===== Code Editor Hover Tooltip Localization =====

    public override string GetCodeHoverWordTypeLabel(string wordType) => wordType switch
    {
        "variable" => "変数",
        "function" => "関数",
        "class" => "クラス",
        "keyword" => "キーワード",
        "comment" => "コメント",
        "namespace" => "名前空間",
        "parameter" => "パラメータ",
        _ => "識別子"
    };

    public override string GetCodeHoverWordTypeDesc(string wordType, string word)
    {
        var encodedWord = System.Net.WebUtility.HtmlEncode(word);
        return wordType switch
        {
            "variable" => $"変数 '{encodedWord}' の定義と使用情報",
            "function" => $"関数 '{encodedWord}' のシグネチャと説明",
            "class" => $"クラス '{encodedWord}' の構造と説明",
            "keyword" => $"キーワード '{encodedWord}' の構文と用途",
            "comment" => $"コメント内の単語 '{encodedWord}'",
            "namespace" => $"名前空間 '{encodedWord}' の情報",
            "parameter" => $"パラメータ '{encodedWord}' の定義と使用情報",
            _ => $"識別子 '{encodedWord}' の情報"
        };
    }

    public override string GetCodeHoverKeywordDesc(string language, string keyword)
    {
        var key = $"{language}:{keyword.ToLower()}";
        return CSharpKeywords.GetValueOrDefault(key, "");
    }

    public override string GetTranslation(string key)
    {
        return TranslationDictionary.GetValueOrDefault(key, "");
    }

    private static readonly Dictionary<string, string> CSharpKeywords = new()
    {
        { "csharp:if", "条件分岐ステートメント。条件式が true の場合にコードブロックを実行します。" },
        { "csharp:else", "条件分岐の代替パス。if と組み合わせて使用し、条件が false の場合に実行されます。" },
        { "csharp:for", "カウントループステートメント。初期化、条件判定、反復の3つの部分から成ります。" },
        { "csharp:while", "条件ループステートメント。条件が true の間、コードブロックを繰り返します。" },
        { "csharp:do", "後判定ループステートメント。最初にコードブロックを1回実行し、その後条件を判定します。" },
        { "csharp:switch", "多岐分岐ステートメント。式の値を異なる case ブランチと照合します。" },
        { "csharp:case", "switch ステートメント内のブランチラベル。一致した値に対応するコードブロックを実行します。" },
        { "csharp:break", "脱出ステートメント。最も近いループまたは switch ステートメントを直ちに終了します。" },
        { "csharp:continue", "継続ステートメント。現在のループの残りの部分をスキップし、次の反復に進みます。" },
        { "csharp:return", "戻り値ステートメント。現在のメソッドを終了し、オプションで値を返します。" },
        { "csharp:goto", "ジャンプステートメント。指定されたラベルに無条件でジャンプします。" },
        { "csharp:foreach", "コレクション反復ステートメント。コレクションの各要素を順にアクセスします。" },
        { "csharp:class", "参照型宣言。データ（フィールド、プロパティ）と動作（メソッド）を含む構造を定義します。" },
        { "csharp:interface", "インターフェース宣言。クラスまたは構造体が実装しなければならない契約を定義します。" },
        { "csharp:struct", "値型宣言。スタックに割り当てられる軽量なデータ構造。" },
        { "csharp:enum", "列挙型宣言。名前付き整数定数のセットを定義します。" },
        { "csharp:namespace", "名前空間宣言。コードを整理し、名前の衝突を回避するための論理コンテナ。" },
        { "csharp:record", "レコード型宣言。値セマンティクスを持つ参照型。不変データに適しています。" },
        { "csharp:delegate", "デリゲート宣言。イベントやコールバックに使用される型安全なメソッド参照。" },
        { "csharp:public", "パブリックアクセス修飾子。メンバーは任意のコードからアクセス可能です。" },
        { "csharp:private", "プライベートアクセス修飾子。メンバーは包含型内部でのみアクセス可能です。" },
        { "csharp:protected", "プロテクテッドアクセス修飾子。メンバーは包含型および派生型でアクセス可能です。" },
        { "csharp:internal", "インターナルアクセス修飾子。メンバーは同一アセンブリ内でのみアクセス可能です。" },
        { "csharp:sealed", "シールド修飾子。クラスが継承されるか、メソッドがオーバーライドされるのを防ぎます。" },
        { "csharp:int", "32ビット符号付き整数型（System.Int32 のエイリアス）。" },
        { "csharp:string", "文字列型（System.String のエイリアス）。Unicode 文字の_immutable_なシーケンスを表します。" },
        { "csharp:bool", "ブール型（System.Boolean のエイリアス）。値は true または false。" },
        { "csharp:float", "32ビット単精度浮動小数点型（System.Single のエイリアス）。" },
        { "csharp:double", "64ビット倍精度浮動小数点型（System.Double のエイリアス）。" },
        { "csharp:decimal", "128ビット高精度10進数型。金融計算に適しています。" },
        { "csharp:char", "16ビット Unicode 文字型（System.Char のエイリアス）。" },
        { "csharp:byte", "8ビット符号なし整数型（System.Byte のエイリアス）。" },
        { "csharp:object", "すべての型の基底型（System.Object のエイリアス）。" },
        { "csharp:var", "暗黙的に型指定されるローカル変数。コンパイラが初期化式から型を推論します。" },
        { "csharp:dynamic", "動的型。コンパイル時の型チェックをスキップし、実行時に解決されます。" },
        { "csharp:void", "戻り値なし型。メソッドが値を返さないことを示します。" },
        { "csharp:static", "静的修飾子。特定のインスタンスではなく型自体に属します。" },
        { "csharp:abstract", "抽象修飾子。不完全な実装を示し、派生クラスによって完成される必要があります。" },
        { "csharp:virtual", "仮想修飾子。メソッドまたはプロパティは派生クラスでオーバーライド可能です。" },
        { "csharp:override", "オーバーライド修飾子。基底クラスの仮想メソッドまたは抽象メソッドの新しい実装を提供します。" },
        { "csharp:const", "定数修飾子。コンパイル時に決定される不変値。" },
        { "csharp:readonly", "読み取り専用修飾子。宣言時またはコンストラクタでのみ値を代入できます。" },
        { "csharp:volatile", "volatile 修飾子。フィールドが複数のスレッドによって同時に変更される可能性を示します。" },
        { "csharp:async", "非同期修飾子。メソッドが非同期操作を含むことを示し、通常 await と組み合わせて使用します。" },
        { "csharp:await", "await 演算子。非同期操作が完了するまでメソッドの実行を一時停止します。" },
        { "csharp:partial", "パーシャル修飾子。クラス、構造体、またはインターフェースを複数のファイルに分割できます。" },
        { "csharp:ref", "参照パラメータ。パラメータを参照渡しします。" },
        { "csharp:out", "出力パラメータ。メソッドから複数の値を返すために使用します。" },
        { "csharp:in", "読み取り専用参照パラメータ。参照渡ししますが、変更は許可されません。" },
        { "csharp:params", "params 修飾子。同じ型の可変長の引数を渡すことができます。" },
        { "csharp:try", "例外処理ブロック。例外を投げる可能性のあるコードを含みます。" },
        { "csharp:catch", "例外キャッチブロック。try ブロックで投げられた例外を処理します。" },
        { "csharp:finally", "finally ブロック。例外が発生したかどうかにかかわらず実行されるコード。" },
        { "csharp:throw", "例外送出ステートメント。例外オブジェクトを手動で投げます。" },
        { "csharp:new", "インスタンス生成演算子。オブジェクトを作成するか、コンストラクタを呼び出します。" },
        { "csharp:this", "現在のインスタンス参照。現在のクラスインスタンスを参照します。" },
        { "csharp:base", "基底クラス参照。直接の基底クラスのメンバーを参照します。" },
        { "csharp:using", "ディレクティブまたはステートメント。名前空間をインポートするか、IDisposable リソースの解放を保証します。" },
        { "csharp:yield", "イテレータキーワード。値を1つずつ返し、遅延実行を実現します。" },
        { "csharp:lock", "ロックステートメント。一度に1つのスレッドのみがコードブロックを実行することを保証します。" },
        { "csharp:typeof", "型演算子。型の System.Type オブジェクトを取得します。" },
        { "csharp:nameof", "nameof 演算子。変数、型、またはメンバーの文字列名を取得します。" },
        { "csharp:is", "型チェック演算子。オブジェクトが指定された型と互換性があるかチェックします。" },
        { "csharp:as", "型変換演算子。安全に型変換を試み、失敗した場合は null を返します。" },
        { "csharp:null", "null リテラル。参照型または null 許容型の null 参照を表します。" },
        { "csharp:true", "ブール真値。" },
        { "csharp:false", "ブール偽値。" },
        { "csharp:default", "デフォルト値式。型のデフォルト値を取得します（参照型は null、数値型は 0）。" },
        { "csharp:operator", "演算子宣言。カスタム型上の演算子の動作を定義します。" },
        { "csharp:explicit", "明示的変換宣言。明示的なキャストが必要な変換演算子。" },
        { "csharp:implicit", "暗黙的変換宣言。自動的に実行できる変換演算子。" },
        { "csharp:unchecked", "unchecked ブロック。整数算術のオーバーフロー チェックを無効にします。" },
        { "csharp:checked", "checked ブロック。整数算術のオーバーフロー チェックを有効にします。" },
        { "csharp:fixed", "fixed ステートメント。ガベージコレクションによる移動を防ぐためにメモリ位置を固定します。" },
        { "csharp:stackalloc", "スタック割り当て演算子。スタック上にメモリブロックを割り当てます。" },
        { "csharp:extern", "外部修飾子。メソッドが外部アセンブリ（DLL など）で実装されていることを示します。" },
        { "csharp:unsafe", "アンセーフコードブロック。ポインタなどのアンセーフ機能の使用を許可します。" },
        // プラットフォームコア型
        { "csharp:ipermissioncallback", "権限コールバックインターフェース。シリコンベースの生命体の各種操作権限（ネットワーク、コマンドライン、ファイルアクセスなど）を評価するために使用されます。" },
        { "csharp:permissionresult", "権限結果列挙型。権限評価の結果を表します：Allowed（許可）、Denied（拒否）、AskUser（ユーザーに確認）。" },
        { "csharp:permissiontype", "権限タイプ列挙型。権限の種類を定義します：NetworkAccess（ネットワークアクセス）、CommandLine（コマンドライン実行）、FileAccess（ファイルアクセス）、Function（関数呼び出し）、DataAccess（データアクセス）。" },
        // System.Net
        { "csharp:ipaddress", "IP アドレスクラス（System.Net.IPAddress）。Internet Protocol (IP) アドレスを表します。" },
        { "csharp:addressfamily", "アドレスファミリ列挙型（System.Net.Sockets.AddressFamily）。ネットワークアドレスのアドレッシングスキームを指定します（InterNetwork（IPv4）、InterNetworkV6（IPv6）など）。" },
        // System
        { "csharp:uri", "統一リソース識別子クラス（System.Uri）。Web リソースにアクセスするための URI（Uniform Resource Identifier）のオブジェクト表現を提供します。" },
        { "csharp:operatingsystem", "オペレーティングシステムクラス（System.OperatingSystem）。現在のオペレーティングシステムをチェックするための静的メソッド（IsWindows()、IsLinux()、IsMacOS() など）を提供します。" },
        { "csharp:environment", "環境クラス（System.Environment）。現在の環境とプラットフォームに関する情報、およびそれらを操作する方法を提供します。" },
        // System.IO
        { "csharp:path", "パスクラス（System.IO.Path）。ファイルまたはディレクトリのパス情報を含む String インスタンスに対して操作を実行します。" },
        // System.Collections.Generic
        { "csharp:hashset", "ハッシュセットクラス（System.Collections.Generic.HashSet<T>）。値のセットを表し、高性能なセット演算を提供します。" },
        // System.Text
        { "csharp:stringbuilder", "文字列ビルダークラス（System.Text.StringBuilder）。変更可能な文字列を表し、文字列の頻繁な変更があるシナリオに適しています。" },
    };

    // 完全な名前空間変換辞書
    private static readonly Dictionary<string, string> TranslationDictionary = new(CSharpKeywords)
    {
        // 完全な名前空間キーを追加
        { "csharp:System.Net.IPAddress", "IP アドレスクラス（System.Net.IPAddress）。Internet Protocol (IP) アドレスを表します。" },
        { "csharp:System.Net.Sockets.AddressFamily", "アドレスファミリ列挙（System.Net.Sockets.AddressFamily）。InterNetwork（IPv4）や InterNetworkV6（IPv6）など、ネットワークアドレスのアドレッシングスキームを指定します。" },
        { "csharp:System.Uri", "統一リソース識別子クラス（System.Uri）。Web リソースにアクセスするための URI のオブジェクト表現を提供します。" },
        { "csharp:System.OperatingSystem", "オペレーティングシステムクラス（System.OperatingSystem）。IsWindows()、IsLinux()、IsMacOS() など、現在のオペレーティングシステムを確認する静的メソッドを提供します。" },
        { "csharp:System.Environment", "環境クラス（System.Environment）。現在の環境とプラットフォームに関する情報と、それらを操作する手段を提供します。" },
        { "csharp:System.IO.Path", "パスクラス（System.IO.Path）。ファイルまたはディレクトリのパス情報を含む String インスタンスに対して操作を実行します。" },
        { "csharp:System.Collections.Generic.HashSet", "ハッシュセットクラス（System.Collections.Generic.HashSet<T>）。値のセットを表し、高性能なセット演算を提供します。" },
        { "csharp:System.Text.StringBuilder", "文字列ビルダークラス（System.Text.StringBuilder）。変更可能な文字列を表し、文字列の頻繁な変更があるシナリオに適しています。" },
    };
}
