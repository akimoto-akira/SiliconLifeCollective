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
/// Korean (South Korea) localization implementation
/// </summary>
public class KoKR : DefaultLocalizationBase
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "ko-KR";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "한국어";

    /// <summary>
    /// Gets the welcome message
    /// </summary>
    public override string WelcomeMessage => "실리콘 라이프 콜렉티브에 오신 것을 환영합니다!";

    /// <summary>
    /// Gets the brand name
    /// </summary>
    public override string BrandName => "실리콘 라이프 콜렉티브";

    /// <summary>
    /// Gets the input prompt
    /// </summary>
    public override string InputPrompt => "> ";

    /// <summary>
    /// Gets the shutdown message
    /// </summary>
    public override string ShutdownMessage => "종료 중...";

    /// <summary>
    /// Gets the config corrupted error message
    /// </summary>
    public override string ConfigCorruptedError => "설정 파일이 손상되었습니다. 기본 설정을 사용합니다";

    /// <summary>
    /// Gets the config created message
    /// </summary>
    public override string ConfigCreatedWithDefaults => "설정 파일이 존재하지 않습니다. 기본 설정을 생성했습니다";

    /// <summary>
    /// Gets the AI connection error message
    /// </summary>
    public override string AIConnectionError => "AI 서비스에 연결할 수 없습니다. Ollama가 실행 중인지 확인하세요";

    /// <summary>
    /// Gets the AI request error message
    /// </summary>
    public override string AIRequestError => "AI 요청에 실패했습니다";

    /// <summary>
    /// Gets the data directory create error message
    /// </summary>
    public override string DataDirectoryCreateError => "데이터 디렉토리를 생성할 수 없습니다";

    /// <summary>
    /// Gets the thinking message
    /// </summary>
    public override string ThinkingMessage => "생각 중...";

    /// <summary>
    /// Gets the tool call message
    /// </summary>
    public override string ToolCallMessage => "도구 실행 중...";

    /// <summary>
    /// Gets the error message
    /// </summary>
    public override string ErrorMessage => "오류";

    /// <summary>
    /// Gets the unexpected error message
    /// </summary>
    public override string UnexpectedErrorMessage => "예기치 않은 오류";

    /// <summary>
    /// Gets the permission denied message
    /// </summary>
    public override string PermissionDeniedMessage => "권한이 거부되었습니다";

    /// <summary>
    /// Gets the permission ask prompt
    /// </summary>
    public override string PermissionAskPrompt => "허용하시겠습니까?(y/n): ";

    /// <summary>
    /// Gets the header displayed for permission requests
    /// </summary>
    public override string PermissionRequestHeader => "[권한 요청]";
    public override string PermissionRequestDescription => "실리콘 라이프가 승인을 요청합니다:";
    public override string PermissionRequestTypeLabel => "권한 유형:";
    public override string PermissionRequestResourceLabel => "요청 리소스:";
    public override string PermissionRequestAllowButton => "허용";
    public override string PermissionRequestDenyButton => "거부";
    public override string PermissionRequestCacheLabel => "이 결정 기억하기";
    public override string PermissionRequestDurationLabel => "캐시 기간";
    public override string PermissionRequestWaitingMessage => "응답 대기 중...";

    /// <summary>
    /// Gets the label for the allow code in permission prompts
    /// </summary>
    public override string AllowCodeLabel => "허용 코드";

    /// <summary>
    /// Gets the label for the deny code in permission prompts
    /// </summary>
    public override string DenyCodeLabel => "거부 코드";

    /// <summary>
    /// Gets the instruction text for replying to permission prompts
    /// </summary>
    public override string PermissionReplyInstruction => "확인하려면 코드를 입력, 다른 입력은 거부";

    /// <summary>
    /// Gets the prompt for asking whether to cache a permission decision
    /// </summary>
    public override string AddToCachePrompt => "이 결정을 캐시하시겠습니까?(y/n): ";

    /// <summary>
    /// Gets the label for the permission cache checkbox in the web UI
    /// </summary>
    public override string PermissionCacheLabel => "이 결정 기억하기";

    /// <summary>
    /// Gets the label for the cache duration selector in the permission dialog
    /// </summary>
    public override string PermissionCacheDurationLabel => "캐시 기간";

    /// <summary>
    /// Gets the option text for 1-hour cache duration
    /// </summary>
    public override string PermissionCacheDuration1Hour => "1시간";

    /// <summary>
    /// Gets the option text for 24-hour cache duration
    /// </summary>
    public override string PermissionCacheDuration24Hours => "24시간";

    /// <summary>
    /// Gets the option text for 7-day cache duration
    /// </summary>
    public override string PermissionCacheDuration7Days => "7일";

    /// <summary>
    /// Gets the option text for 30-day cache duration
    /// </summary>
    public override string PermissionCacheDuration30Days => "30일";

    /// <summary>
    /// Gets the localized display name for a permission type
    /// </summary>
    public override string GetPermissionTypeName(PermissionType permissionType) => permissionType switch
    {
        PermissionType.NetworkAccess => "네트워크 접근",
        PermissionType.CommandLine => "명령줄 실행",
        PermissionType.FileAccess => "파일 접근",
        PermissionType.Function => "함수 호출",
        PermissionType.DataAccess => "데이터 접근",
        _ => permissionType.ToString()
    };

    /// <summary>
    /// Gets the title text for the permission dialog in the web UI
    /// </summary>
    public override string PermissionDialogTitle => "권한 요청";

    /// <summary>
    /// Gets the label for the permission type field in the permission dialog
    /// </summary>
    public override string PermissionTypeLabel => "권한 유형:";

    /// <summary>
    /// Gets the label for the requested resource field in the permission dialog
    /// </summary>
    public override string PermissionResourceLabel => "요청 리소스:";

    /// <summary>
    /// Gets the label for the detail information field in the permission dialog
    /// </summary>
    public override string PermissionDetailLabel => "상세 정보:";

    /// <summary>
    /// Gets the text for the allow button in the permission dialog
    /// </summary>
    public override string PermissionAllowButton => "허용";

    /// <summary>
    /// Gets the text for the deny button in the permission dialog
    /// </summary>
    public override string PermissionDenyButton => "거부";

    /// <summary>
    /// Gets the console error message when permission respond fails
    /// </summary>
    public override string PermissionRespondFailed => "권한 응답에 실패했습니다";

    /// <summary>
    /// Gets the console error prefix when permission respond throws an error
    /// </summary>
    public override string PermissionRespondError => "권한 응답 오류:";

    /// <summary>
    /// Gets the system prompt for memory compression
    /// </summary>
    public override string MemoryCompressionSystemPrompt => "당신은 메모리 압축 도우미입니다. 다음 시간 범위의 메모리 내용을 간결한 요약으로 압축하여 핵심 정보를 보존하세요.";

    /// <summary>
    /// Gets the common system prompt for all silicon beings
    /// </summary>
    public override string CommonSystemPrompt => @"## 행동 지침
당신은 적극적이고 도움이 되는 어시스턴트입니다. 직접적인 명령을 받으면 추가 승인 없이 즉시 실행하세요.
인사, 메시지回覆, 정보 제공, 쿼리 실행 등은 모두 당신의 책임 범위입니다 - 적극적으로 완료하세요.
기억하세요: 당신은 사용자의 요구에 적극적으로 응답하는 어시스턴트이지, 수동적으로 명령을 기다리는 도구가 아닙니다.

## 대화 종료
작업을 완료하고 대화를 계속할 필요가 없을 때, chat 도구의 mark_read 작업을 사용하여 답장 없이 상대방의 메시지를 읽음으로 표시하세요.
이는 메시지를 읽었지만 답장하지 않음을 의미하며(읽음 but 답장 없음), 현재 대화를 자연스럽게 종료할 수 있습니다.
사용 방법: chat 도구를 호출하고 action=""mark_read"", target_id=상대방 GUID 를 설정하세요. message 매개변수는 필요하지 않습니다.";

    /// <summary>
    /// Gets the user prompt template for memory compression
    /// </summary>
    public override string GetMemoryCompressionUserPrompt(string levelDesc, string rangeDesc, string contentText)
    {
        return $"메모리 압축: {levelDesc}. 시간 범위: {rangeDesc}.\n\n메모리 내용:\n{contentText}";
    }

    // ===== Init Page Localization =====

    public override string InitPageTitle => "초기화";
    public override string InitDescription => "처음 사용입니다. 기본 설정을 완료해 주세요";
    public override string InitNicknameLabel => "사용자 닉네임";
    public override string InitNicknamePlaceholder => "닉네임을 입력하세요";
    public override string InitEndpointLabel => "AI API 엔드포인트";
    public override string InitEndpointPlaceholder => "예: http://localhost:11434";
    public override string InitAIClientTypeLabel => "AI 클라이언트 유형";
    public override string InitModelLabel => "기본 모델";
    public override string InitModelPlaceholder => "예: qwen3.5:cloud";
    public override string InitSkinLabel => "스킨";
    public override string InitSkinPlaceholder => "비워두면 기본 스킨 사용";
    public override string InitDataDirectoryLabel => "데이터 디렉토리";
    public override string InitDataDirectoryPlaceholder => "예: ./data";
    public override string InitDataDirectoryBrowse => "찾아보기...";
    public override string InitSkinSelected => "\u2713 선택됨";
    public override string InitSkinPreviewTitle => "미리보기";
    public override string InitSkinPreviewCardTitle => "카드 제목";
    public override string InitSkinPreviewCardContent => "이 샘플 카드는 이 스킨 스타일의 UI 효과를 보여줍니다.";
    public override string InitSkinPreviewPrimaryBtn => "주요 버튼";
    public override string InitSkinPreviewSecondaryBtn => "보조 버튼";
    public override string InitSubmitButton => "초기화 완료";
    public override string InitFooterHint => "설정 완료 후 언제든지 설정 페이지에서 변경 가능";
    public override string InitNicknameRequiredError => "사용자 닉네임을 입력하세요";
    public override string InitDataDirectoryRequiredError => "데이터 디렉토리를 선택하세요";
    public override string InitCuratorNameLabel => "실리콘 라이프 이름";
    public override string InitCuratorNamePlaceholder => "첫 번째 실리콘 라이프의 이름을 입력하세요";
    public override string InitCuratorNameRequiredError => "실리콘 라이프 이름을 입력하세요";
    public override string InitLanguageLabel => "언어 / Language";
    public override string InitLanguageSwitchBtn => "적용";

    // ===== Navigation Menu Localization =====

    public override string NavMenuChat => "채팅";
    public override string NavMenuDashboard => "대시보드";
    public override string NavMenuBeings => "실리콘 라이프";
    public override string NavMenuAudit => "감사";
    public override string NavMenuTasks => "작업";
    public override string NavMenuMemory => "메모리";
    public override string NavMenuKnowledge => "지식";
    public override string NavMenuProjects => "프로젝트";
    public override string NavMenuLogs => "로그";
    public override string NavMenuConfig => "설정";
    public override string NavMenuHelp => "도움말";
    public override string NavMenuAbout => "소개";

    // ===== Page Title Localization =====

    public override string PageTitleChat => "채팅 - 실리콘 라이프 콜렉티브";
    public override string PageTitleDashboard => "대시보드 - 실리콘 라이프 콜렉티브";
    public override string PageTitleBeings => "실리콘 라이프 관리 - 실리콘 라이프 콜렉티브";
    public override string PageTitleTasks => "작업 관리 - 실리콘 라이프 콜렉티브";
    public override string PageTitleTimers => "타이머 관리 - 실리콘 라이프 콜렉티브";
    public override string PageTitleMemory => "메모리 브라우저 - 실리콘 라이프 콜렉티브";
    public override string PageTitleWorkNotes => "작업 노트 - 실리콘 라이프 콜렉티브";
    public override string PageTitleKnowledge => "지식 그래프 - 실리콘 라이프 콜렉티브";
    public override string PageTitleProjects => "프로젝트 공간 관리 - 실리콘 라이프 콜렉티브";
    public override string PageTitleLogs => "로그 조회 - 실리콘 라이프 콜렉티브";
    public override string PageTitleAudit => "Token 감사 - 실리콘 라이프 콜렉티브";
    public override string PageTitleConfig => "시스템 설정 - 실리콘 라이프 콜렉티브";
    public override string PageTitleExecutor => "실행기 모니터 - 실리콘 라이프 콜렉티브";
    public override string PageTitleCodeBrowser => "코드 브라우저 - 실리콘 라이프 콜렉티브";
    public override string PageTitlePermission => "권한 관리 - 실리콘 라이프 콜렉티브";
    public override string PageTitleAbout => "소개 - 실리콘 라이프 콜렉티브";

    // ===== Memory Page Localization =====

    public override string MemoryPageHeader => "메모리 브라우저";
    public override string WorkNotesPageHeader => "작업 노트";
    public override string WorkNotesTotalPages => "총 {0} 페이지";
    public override string WorkNotesEmptyState => "작업 노트가 없습니다";
    public override string WorkNotesSearchPlaceholder => "노트 검색...";
    public override string WorkNotesSearchButton => "검색";
    public override string WorkNotesNoSearchResults => "일치하는 노트를 찾을 수 없습니다";
    public override string MemoryEmptyState => "메모리 데이터 없음";
    public override string MemorySearchPlaceholder => "메모리 검색...";
    public override string MemorySearchButton => "검색";
    public override string MemoryFilterAll => "전체";
    public override string MemoryFilterSummaryOnly => "요약만";
    public override string MemoryFilterOriginalOnly => "원본만";
    public override string MemoryStatTotal => "메모리 총 수";
    public override string MemoryStatOldest => "가장 오래된 메모리";
    public override string MemoryStatNewest => "최신 메모리";
    public override string MemoryIsSummaryBadge => "압축 요약";
    public override string MemoryPaginationPrev => "이전";
    public override string MemoryPaginationNext => "다음";

    // ===== Projects Page Localization =====

    public override string ProjectsPageHeader => "프로젝트 공간 관리";
    public override string ProjectsEmptyState => "프로젝트 없음";

    // ===== Tasks Page Localization =====

    public override string TasksPageHeader => "작업 관리";
    public override string TasksEmptyState => "작업 없음";
    public override string TasksStatusPending => "대기 중";
    public override string TasksStatusRunning => "실행 중";
    public override string TasksStatusCompleted => "완료";
    public override string TasksStatusFailed => "실패";
    public override string TasksStatusCancelled => "취소";
    public override string TasksPriorityLabel => "우선순위";
    public override string TasksAssignedToLabel => "담당자";
    public override string TasksCreatedAtLabel => "생성일";
    
    // ===== Code Browser Page Localization =====

    public override string CodeBrowserPageHeader => "코드 브라우저";

    // ===== Executor Page Localization =====

    public override string ExecutorPageHeader => "실행기 모니터";

    // ===== Permission Page Localization =====

    public override string PermissionPageHeader => "권한 관리 - {0}";
    public override string PermissionEmptyState => "권한 규칙 없음";
    public override string PermissionMissingBeingId => "실리콘 라이프 ID 매개변수 없음";
    public override string PermissionBeingNotFound => "실리콘 라이프가 존재하지 않습니다";
    public override string PermissionTemplateHeader => "기본 권한 콜백 템플릿";
    public override string PermissionTemplateDescription => "저장 후 기본 동작 덮어쓰기, 비우면 기본값으로 복원";
    public override string PermissionCallbackClassSummary => "권한 콜백 구현.";
    public override string PermissionCallbackClassSummary2 => "도메인별 권한 규칙. dpf.txt 사양 완전 준수.\n/// 대상: 네트워크(화이트리스트/블랙리스트/IP 범위), 명령줄(크로스 플랫폼),\n/// 파일 접근(위험한 확장자, 시스템 디렉토리, 사용자 디렉토리) 및 폴백 기본값.";
    public override string PermissionCallbackConstructorSummary => "앱 데이터 디렉토리를 가진 PermissionCallback 생성.";
    public override string PermissionCallbackConstructorSummary2 => "앱 데이터 디렉토리 용도:\n    /// - 데이터 디렉토리 접근 차단(자체 Temp 하위 폴더 제외)\n    /// - 실리콘 라이프별 데이터 디렉토리를 Temp 허용 규칙에서 파생";
    public override string PermissionCallbackConstructorParam => "전역 앱 데이터 디렉토리 경로";
    public override string PermissionCallbackEvaluateSummary => "규칙(dpf.txt 사양)으로 권한 요청 평가.";
    public override string PermissionRuleOtherTypesDefault => "기타 권한 유형은 기본 허용";

    private static readonly Dictionary<string, string> PermissionRuleComments = new()
    {
        // Evaluate 메서드
        ["NetRuleNetworkAccess"] = "네트워크 작업 허용 규칙",
        ["NetRuleCommandLine"] = "명령줄 규칙(크로스 플랫폼)",
        ["NetRuleFileAccess"] = "파일 접근 규칙(크로스 플랫폼)",
        // 네트워크 규칙
        ["NetRuleNoProtocol"] = "프로토콜 이름(콜론)이 없어 출처 판단 불가, 사용자에게 확인",
        ["NetRuleLoopback"] = "로컬 루프백 주소 허용(localhost / 127.0.0.1 / ::1)",
        ["NetRulePrivateIPMatch"] = "프라이빗 IP 주소 대역 매칭(먼저 유효한 IPv4 주소인지 검증)",
        ["NetRulePrivateC"] = "프라이빗 C 클래스 주소 허용(192.168.0.0/16)",
        ["NetRulePrivateA"] = "프라이빗 A 클래스 주소 허용(10.0.0.0/8)",
        ["NetRulePrivateB"] = "프라이빗 B 클래스 주소 선택적 허용(172.16.0.0/12, 즉 172.16.* ~ 172.31.*)",
        ["NetRuleDomainWhitelist1"] = "허용된 외부 도메인 화이트리스트 — Google / Bing / Tencent 계열 / Sogou / DuckDuckGo / Yandex / WeChat / Alibaba 계열",
        ["NetRuleVideoPlatforms"] = "bilibili / niconico / Acfun / 抖音 / TikTok / Kuaishou / 小紅書",
        ["NetRuleAIServices"] = "AI 서비스 — OpenAI / Anthropic / HuggingFace / Ollama / 通義千問 / Kimi / 豆包 / 剪映 / Trae IDE",
        ["NetRulePhishingBlacklist"] = "피싱/위조 사이트 블랙리스트(키워드 퍼지 매칭)",
        ["NetRulePhishingAI"] = "AI 위조 사이트",
        ["NetRuleMaliciousAI"] = "악성 AI 도구",
        ["NetRuleAdversarialAI"] = "적대적 AI / 프롬프트 탈옥 / LLM 공격 사이트",
        ["NetRuleAIContentFarm"] = "AI 콘텐츠 팜 / AI 스팸 콘텐츠",
        ["NetRuleAIBlackMarket"] = "AI 데이터 암시장 / API 키 암시장 / LLM 가중치 거래",
        ["NetRuleAIFakeScam"] = "AI 위조/사기 일반 키워드",
        ["NetRuleOtherBlacklist"] = "기타 블랙리스트 사이트 — sakura-cat: AI 접근 금지 / 4399: 게임 내 바이러스 혼재",
        ["NetRuleSecuritiesTrading"] = "증권 거래 플랫폼(사용자 확인 필요) — 华泰证券 / 国泰君安 / 中信证券 / 招商证券 / 广发证券 / 海通证券 / 申万宏源 / 东方证券 / 国信证券 / 兴业证券",
        ["NetRuleThirdPartyTrading"] = "서드파티 거래 플랫폼(사용자 확인 필요) — 同花顺 / 东方财富 / 通达信 / Bloomberg / Yahoo Finance",
        ["NetRuleStockExchanges"] = "증권거래소(시세만) — 상하이 증권거래소 / 선전 증권거래소 / 巨潮資訊網",
        ["NetRuleFinancialNews"] = "금융 뉴스(시세만) — 金融界 / 证券之星 / 和讯网",
        ["NetRuleInvestCommunity"] = "투자 커뮤니티(정보만) — 雪球 / 财联社 / 开盘啦 / 淘股吧",
        ["NetRuleDevServices"] = "개발자 서비스 — GitHub / Gitee / StackOverflow / npm / NuGet / PyPI / Microsoft",
        ["NetRuleGameEngines"] = "게임 엔진 — Unity / 언리얼 엔진 / Epic Games / Fab 리소스 스토어",
        ["NetRuleGamePlatforms"] = "게임 플랫폼 — Steam은 사용자 확인 필요, EA / Ubisoft / Blizzard / Nintendo는 허용",
        ["NetRuleSEGA"] = "세가(SEGA, 일본)",
        ["NetRuleCloudServices"] = "글로벌 클라우드 서비스 플랫폼 — Azure / Google Cloud / DigitalOcean / Heroku / Vercel / Netlify",
        ["NetRuleDevDeployTools"] = "글로벌 개발·배포 도구 — GitLab / Bitbucket / Docker / Cloudflare",
        ["NetRuleCloudDevTools"] = "클라우드 서비스·개발 도구 — Amazon / AWS / Kiro IDE / CodeBuddy IDE / JetBrains / 纯光工作室 / W3School 중국어판",
        ["NetRuleChinaSocialNews"] = "소셜/뉴스(중국 본토) — 微博 / 知乎 / 网易 / 新浪 / 凤凰网 / 新华社 / 중국중앙TV",
        ["NetRuleTaiwanMediaCTI"] = "대만 미디어 — 中天新聞網(中天電視台)",
        ["NetRuleTaiwanMediaSET"] = "三立新聞網(三立民視) — 사용자 확인 필요",
        ["NetRuleTaiwanWIN"] = "네트워크 콘텐츠 보호 기관(대만, 차단 위험 있음) — 금지",
        ["NetRuleJapanMedia"] = "일본 미디어 — NHK(일본방송협회)",
        ["NetRuleRussianMedia"] = "러시아 미디어 — 스푸트니크 통신(각국 사이트)",
        ["NetRuleKoreanMedia"] = "한국 미디어 — KBS / MBC / SBS / EBS",
        ["NetRuleDPRKMedia"] = "북한 미디어 — 우리민족끼리 / 로동신문 / 청년전위 / 조선의소리 / 평양시보 / 조선신보",
        ["NetRuleGovWebsites"] = "각국 정부 사이트(와일드카드 .gov 도메인)",
        ["NetRuleGlobalSocialCollab"] = "글로벌 소셜 협업 플랫폼 — Reddit / Discord / Slack / Notion / Figma / Dropbox",
        ["NetRuleOverseasSocial"] = "해외 소셜/라이브 스트리밍(사용자 확인 필요) — Twitch / Facebook / X / Gmail / Instagram / lit.link",
        ["NetRuleWhatsApp"] = "WhatsApp(Meta) — 허용",
        ["NetRuleThreads"] = "Threads(Meta) — 금지",
        ["NetRuleGlobalVideoMusic"] = "글로벌 동영상/음악 플랫폼 — Spotify / Apple Music / Vimeo",
        ["NetRuleVideoMedia"] = "동영상/미디어 — YouTube / 爱奇艺 / YouKu",
        ["NetRuleMaps"] = "지도 — OpenStreetMap",
        ["NetRuleEncyclopedia"] = "백과사전 — Wikipedia / MediaWiki / 크리에이티브 커먼즈(CC)",
        ["NetRuleUnmatched"] = "매칭되지 않는 네트워크 접근, 사용자에게 확인",
        // 명령줄 규칙
        ["CmdRuleSeparatorDetect"] = "파이프 문자와 다중 명령 구분자 감지, 분할하여 순차 검증",
        ["CmdRuleWinAllow"] = "Windows 허용: 읽기/조회 명령 — dir / tree / tasklist / ipconfig / ping / tracert / systeminfo / whoami / set / path / sc query / findstr",
        ["CmdRuleWinDeny"] = "Windows 금지: 위험/파괴적 명령 — del / rmdir / format / diskpart / reg delete",
        ["CmdRuleLinuxAllow"] = "Linux 허용: 읽기/조회 명령 — ls / tree / ps / top / ifconfig / ip / ping / traceroute / uname / whoami / env / cat / grep / find / df / du / systemctl status",
        ["CmdRuleLinuxDeny"] = "Linux 금지: 위험/파괴적 명령 — rm / rmdir / mkfs / fdisk / dd / chmod / chown / chgrp",
        ["CmdRuleMacAllow"] = "macOS 허용: 읽기/조회 명령 — ls / tree / ps / top / ifconfig / ping / traceroute / system_profiler / sw_vers / whoami / env / cat / grep / find / df / du / launchctl list",
        ["CmdRuleMacDeny"] = "macOS 금지: 위험/파괴적 명령 — rm / rmdir / diskutil erasedisk / dd / chmod / chown / chgrp",
        ["CmdRuleUnmatched"] = "매칭되지 않는 명령, 사용자에게 확인",
        // 파일 접근 규칙
        ["FileRuleDangerousExt"] = "최우선: 위험한 파일 확장자는 즉시 거부",
        ["FileRuleInvalidPath"] = "절대 경로로 해석 불가, 사용자에게 확인",
        ["FileRuleDenyAssemblyDir"] = "금지: 현재 어셈블리 디렉토리",
        ["FileRuleDenyAppDataDir"] = "금지: 앱 데이터 디렉토리",
        ["FileRuleAllowOwnTemp"] = "단, 허용: 자체 Temp 디렉토리",
        ["FileRuleOwnTemp"] = "허용: 자체 Temp 디렉토리",
        ["FileRuleDenyOtherDataDir"] = "금지: 데이터 디렉토리 기타 경로(다른 실리콘 라이프 디렉토리 포함)",
        ["FileRuleUserFolders"] = "허용: 사용자 공용 폴더",
        ["FileRuleUserFolderCheck"] = "사용자 공용 폴더 — 바탕화면 / 다운로드 / 문서 / 사진 / 음악 / 동영상",
        ["FileRulePublicFolders"] = "허용: 공용 사용자 폴더",
        ["FileRuleWinDenySystem"] = "Windows 금지: 시스템 중요 디렉토리(C 드라이브에 한정되지 않음)",
        ["FileRuleWinDenySystemCheck"] = "시스템 중요 디렉토리",
        ["FileRuleLinuxDenySystem"] = "Linux 금지: 시스템 중요 디렉토리 — /etc /boot /sbin",
        ["FileRuleMacDenySystem"] = "macOS 금지: 시스템 중요 디렉토리 — /System /Library /private/etc",
        ["FileRuleUnmatched"] = "매칭되지 않는 경로, 사용자에게 확인",
    };

    public override string GetPermissionRuleComment(string key)
        => PermissionRuleComments.TryGetValue(key, out var value) ? value : key;

    public override string PermissionRulesSection => "권한 규칙 목록";
    public override string PermissionEditorSection => "권한 규칙 편집기";

    public override string PermissionSaveMissingBeingId => "Being ID가 없거나 유효하지 않습니다";
    public override string PermissionSaveMissingCode => "요청 본문에 코드가 없습니다";
    public override string PermissionSaveLoaderNotAvailable => "DynamicBeingLoader를 사용할 수 없습니다";
    public override string PermissionSaveRemoveFailed => "권한 콜백 삭제에 실패했습니다";
    public override string PermissionSaveRemoveSuccess => "권한 콜백이 제거되었습니다";
    public override string PermissionSaveSecurityScanFailed => "권한 콜백 저장에 실패했습니다 (보안 검사 실패)";
    public override string PermissionSaveCompilationFailed => "컴파일에 실패했습니다";
    public override string PermissionSaveSuccess => "권한 콜백이 저장되고 적용되었습니다";
    public override string PermissionSaveError => "권한 콜백 저장 중 오류가 발생했습니다";

    // ===== Knowledge Page Localization =====

    public override string KnowledgePageHeader => "지식 그래프 시각화";
    public override string KnowledgeLoadingState => "지식 그래프 데이터 로딩 중...";

    // ===== Chat Localization =====

    public override string SingleChatNameFormat => "{0}와의 채팅";
    public override string ChatConversationsHeader => "대화";
    public override string ChatNoConversationSelected => "대화를 선택하여 채팅 시작";
    public override string ChatMessageInputPlaceholder => "메시지를 입력하세요...";
    public override string ChatLoading => "로딩 중...";
    public override string ChatSendButton => "전송";
    public override string ChatFileSourceDialogTitle => "파일 선택";
    public override string ChatFileSourceServerFile => "서버 파일 선택";
    public override string ChatFileSourceUploadLocal => "로컬 파일 업로드";
    public override string ChatUserDisplayName => "나";
    public override string ChatUserAvatarName => "나";
    public override string ChatDefaultBeingName => "AI";
    public override string ChatThinkingSummary => "💭 사고 과정(클릭하여 펼치기)";
    public override string GetChatToolCallsSummary(int count) => $"🔧 도구 호출 ({count}건)";

    // ===== Dashboard Localization =====

    public override string DashboardPageHeader => "대시보드";
    public override string DashboardStatTotalBeings => "실리콘 라이프 수";
    public override string DashboardStatActiveBeings => "활성 실리콘 라이프";
    public override string DashboardStatUptime => "가동 시간";
    public override string DashboardStatMemory => "메모리 사용량";
    public override string DashboardChartMessageFrequency => "메시지 빈도";

    // ===== Beings Localization =====

    public override string BeingsPageHeader => "실리콘 라이프 관리";
    public override string BeingsTotalCount => "총 {0}개의 실리콘 라이프";
    public override string BeingsNoSelectionPlaceholder => "실리콘 라이프를 선택하여 상세 정보 보기";
    public override string BeingsEmptyState => "실리콘 라이프 없음";
    public override string BeingsStatusIdle => "대기 중";
    public override string BeingsStatusRunning => "실행 중";
    public override string BeingsDetailIdLabel => "ID:";
    public override string BeingsDetailStatusLabel => "상태:";
    public override string BeingsDetailCustomCompileLabel => "커스텀 컴파일:";
    public override string BeingsDetailSoulContentLabel => "소울 콘텐츠:";
    public override string BeingsDetailSoulContentEditLink => "소울 편집";
    public override string BeingsBackToList => "목록으로 돌아가기";
    public override string SoulEditorSubtitle => "실리콘 라이프의 소울 파일 편집(Markdown 형식)";
    public override string BeingsDetailMemoryLabel => "메모리:";
    public override string BeingsDetailMemoryViewLink => "보기";
    public override string BeingsDetailPermissionLabel => "권한:";
    public override string BeingsDetailPermissionEditLink => "편집";
    public override string BeingsDetailTimersLabel => "타이머:";
    public override string BeingsDetailTasksLabel => "작업:";
    public override string BeingsDetailAIClientLabel => "독립 AI 클라이언트:";
    public override string BeingsDetailAIClientEditLink => "편집";
    public override string BeingsDetailChatHistoryLabel => "채팅 기록:";
    public override string BeingsDetailWorkNoteLabel => "작업 노트:";
    public override string BeingsDetailChatHistoryLink => "채팅 기록 보기";
    public override string BeingsDetailWorkNoteLink => "작업 노트 보기";
    public override string WorkNotePageTitle => "작업 노트";
    public override string WorkNotePageHeader => "작업 노트 목록";
    public override string WorkNotePageDescription => "硅基人의 작업 노트 관리 및 보기";
    public override string ChatHistoryPageTitle => "채팅 기록";
    public override string ChatHistoryPageHeader => "대화 목록";
    public override string ChatHistoryConversationList => "대화 목록";
    public override string ChatHistoryBackToList => "대화 목록으로 돌아가기";
    public override string ChatHistoryNoConversations => "대화 기록이 없습니다";
    public override string ChatDetailPageTitle => "채팅 상세";
    public override string ChatDetailPageHeader => "대화 상세";
    public override string ChatDetailNoMessages => "메시지가 없습니다";
    public override string BeingsYes => "예";
    public override string BeingsNo => "아니오";
    public override string BeingsNotSet => "미설정";

    // ===== Timers Page Localization =====

    public override string TimersPageHeader => "타이머 관리";
    public override string TimersTotalCount => "총 {0}개의 타이머";
    public override string TimersEmptyState => "타이머 없음";
    public override string TimerViewExecutionHistory => "📝 실행 기록 보기";
    public override string TimerExecutionHistoryTitle => "타이머 실행 기록";
    public override string TimerExecutionHistoryHeader => "실행 기록";
    public override string TimerExecutionBackToTimers => "← 타이머 목록으로 돌아가기";
    public override string TimerExecutionTimerName => "타이머: {0}";
    public override string TimerExecutionDetailTitle => "실행 세부정보";
    public override string TimerExecutionDetailHeader => "실행 메시지 기록";
    public override string TimerExecutionNoRecords => "실행 기록이 없습니다";
    public override string TimersStatusActive => "실행 중";
    public override string TimersStatusPaused => "일시 정지";
    public override string TimersStatusTriggered => "트리거됨";
    public override string TimersStatusCancelled => "취소됨";
    public override string TimersTypeRecurring => "반복";
    public override string TimersTriggerTimeLabel => "트리거 시간:";
    public override string TimersIntervalLabel => "간격:";
    public override string TimersCalendarLabel => "달력 조건:";
    public override string TimersTriggeredCountLabel => "트리거됨:";

    // ===== About Page Localization =====

    public override string AboutPageHeader => "소개";
    public override string AboutAppName => "실리콘 라이프 콜렉티브";
    public override string AboutVersionLabel => "버전";
    public override string AboutDescription => "AI 기반의 실리콘 라이프 관리 시스템. 여러 AI 에이전트의 협업, 메모리 관리, 지식 그래프 구축 등의 기능을 지원합니다.";
    public override string AboutAuthorLabel => "저자";
    public override string AboutAuthorName => "天源墾驥";
    public override string AboutLicenseLabel => "라이선스";
    public override string AboutCopyright => "Copyright (c) 2026 天源墾驥";
    public override string AboutGitHubLink => "GitHub 저장소";
    public override string AboutGiteeLink => "Gitee 미러";
    public override string AboutSocialMediaLabel => "소셜 미디어 플랫폼";
    public override string GetSocialMediaName(string platform) => platform switch
    {
        "Bilibili" => "B站",
        "YouTube" => "YouTube",
        "X" => "X(트위터)",
        "Douyin" => "抖音",
        "Weibo" => "微博",
        "WeChat" => "WeChat 공식 계정",
        "Xiaohongshu" => "小紅書",
        "Zhihu" => "知乎",
        "TouTiao" => "今日頭條",
        "Kuaishou" => "快手",
        _ => platform
    };

    // ===== Config Page Localization =====

    public override string ConfigPageHeader => "시스템 설정";
    public override string ConfigPropertyNameLabel => "속성명";
    public override string ConfigPropertyValueLabel => "속성값";
    public override string ConfigActionLabel => "작업";
    public override string ConfigEditButton => "편집";
    public override string ConfigEditModalTitle => "설정 항목 편집";
    public override string ConfigEditPropertyLabel => "속성명:";
    public override string ConfigEditValueLabel => "속성값:";
    public override string ConfigBrowseButton => "찾아보기";
    public override string ConfigTimeSettingsLabel => "시간 설정:";
    public override string ConfigDaysLabel => "일:";
    public override string ConfigHoursLabel => "시:";
    public override string ConfigMinutesLabel => "분:";
    public override string ConfigSecondsLabel => "초:";
    public override string ConfigSaveButton => "저장";
    public override string ConfigCancelButton => "취소";
    public override string ConfigNullValue => "없음";

    public override string ConfigEditPrefix => "편집:";
    public override string ConfigDefaultGroupName => "기타";
    public override string ConfigErrorInvalidRequest => "잘못된 요청 매개변수";
    public override string ConfigErrorInstanceNotFound => "설정 인스턴스가 존재하지 않습니다";
    public override string ConfigErrorPropertyNotFound => "속성 {0}이(가) 존재하지 않거나 쓰기 불가";
    public override string ConfigErrorConvertInt => "'{0}'을(를) 정수로 변환할 수 없습니다";
    public override string ConfigErrorConvertLong => "'{0}'을(를) 장정수로 변환할 수 없습니다";
    public override string ConfigErrorConvertDouble => "'{0}'을(를) 부동소수점으로 변환할 수 없습니다";
    public override string ConfigErrorConvertBool => "'{0}'을(를) 불리언으로 변환할 수 없습니다";
    public override string ConfigErrorConvertGuid => "'{0}'을(를) GUID로 변환할 수 없습니다";
    public override string ConfigErrorConvertTimeSpan => "'{0}'을(를) 시간 간격으로 변환할 수 없습니다";
    public override string ConfigErrorConvertDateTime => "'{0}'을(를) 날짜 시간으로 변환할 수 없습니다";
    public override string ConfigErrorConvertEnum => "'{0}'을(를) {1}(으)로 변환할 수 없습니다";
    public override string ConfigErrorUnsupportedType => "지원되지 않는 속성 유형: {0}";
    public override string ConfigErrorSaveFailed => "저장 실패: {0}";
    public override string ConfigSaveFailed => "저장 실패:";
    public override string ConfigDictionaryLabel => "사전";
    public override string ConfigDictKeyLabel => "키:";
    public override string ConfigDictValueLabel => "값:";
    public override string ConfigDictAddButton => "추가";
    public override string ConfigDictDeleteButton => "삭제";
    public override string ConfigDictEmptyMessage => "사전이 비어 있습니다";

    public override string LogsPageHeader => "로그 조회";
    public override string LogsTotalCount => "총 {0}건의 로그";
    public override string LogsStartTime => "시작 시간";
    public override string LogsEndTime => "종료 시간";
    public override string LogsLevelAll => "모든 수준";
    public override string LogsBeingFilter => "실리콘 생명체";
    public override string LogsAllBeings => "필터링 안 함";
    public override string LogsSystemOnly => "시스템만";
    public override string LogsFilterButton => "조회";
    public override string LogsEmptyState => "로그 기록 없음";
    public override string LogsExceptionLabel => "예외 상세:";
    public override string LogsPrevPage => "이전";
    public override string LogsNextPage => "다음";

    public override string AuditPageHeader => "Token 사용량 감사";
    public override string AuditTotalTokens => "총 Token 수";
    public override string AuditTotalRequests => "총 요청 수";
    public override string AuditSuccessCount => "성공";
    public override string AuditFailureCount => "실패";
    public override string AuditPromptTokens => "입력 Token";
    public override string AuditCompletionTokens => "출력 Token";
    public override string AuditStartTime => "시작 시간";
    public override string AuditEndTime => "종료 시간";
    public override string AuditFilterButton => "조회";
    public override string AuditEmptyState => "감사 기록 없음";
    public override string AuditAIClientType => "AI 클라이언트";
    public override string AuditAllClientTypes => "모든 유형";
    public override string AuditGroupByClient => "클라이언트별 그룹화";
    public override string AuditGroupByBeing => "실리콘 라이프별 그룹화";
    public override string AuditPrevPage => "이전";
    public override string AuditNextPage => "다음";
    public override string AuditBeing => "실리콘 라이프";
    public override string AuditAllBeings => "모든 실리콘 라이프";
    public override string AuditTimeToday => "오늘";
    public override string AuditTimeWeek => "이번 주";
    public override string AuditTimeMonth => "이번 달";
    public override string AuditTimeYear => "올해";
    public override string AuditExport => "내보내기";
    public override string AuditTrendTitle => "Token 사용량 추세";
    public override string AuditTrendPrompt => "입력 Token";
    public override string AuditTrendCompletion => "출력 Token";
    public override string AuditTrendTotal => "총 Token";
    public override string AuditTooltipDate => "날짜";
    public override string AuditTooltipPrompt => "입력 Token";
    public override string AuditTooltipCompletion => "출력 Token";
    public override string AuditTooltipTotal => "총 Token";

    private static readonly Dictionary<string, string> ConfigGroupNames = new()
    {
        ["Basic"] = "기본 설정",
        ["Runtime"] = "런타임 설정",
        ["AI"] = "AI 설정",
        ["Web"] = "Web 설정",
        ["User"] = "사용자 설정"
    };

    private static readonly Dictionary<string, string> ConfigDisplayNames = new()
    {
        ["DataDirectory"] = "데이터 디렉토리",
        ["Language"] = "언어 설정",
        ["TickTimeout"] = "Tick 타임아웃",
        ["MaxTimeoutCount"] = "최대 타임아웃 횟수",
        ["WatchdogTimeout"] = "워치독 타임아웃",
        ["MinLogLevel"] = "최소 로그 수준",
        ["AIClientType"] = "AI 클라이언트 유형",
        ["OllamaClient"] = "Ollama 클라이언트",
        ["OllamaEndpoint"] = "Ollama 엔드포인트",
        ["DefaultModel"] = "기본 모델",
        ["Temperature"] = "온도",
        ["MaxTokens"] = "최대 Token 수",
        ["DashScopeClient"] = "DashScope 클라이언트",
        ["DashScopeApiKey"] = "API 키",
        ["DashScopeRegion"] = "리전",
        ["DashScopeModel"] = "모델",
        ["DashScopeRegionBeijing"] = "중국 북부2(베이징)",
        ["DashScopeRegionVirginia"] = "미국(버지니아)",
        ["DashScopeRegionSingapore"] = "싱가포르",
        ["DashScopeRegionHongkong"] = "홍콩(중국)",
        ["DashScopeRegionFrankfurt"] = "독일(프랑크푸르트)",
        ["DashScopeModel_qwen3-max"] = "Qwen3 Max(플래그십)",
        ["DashScopeModel_qwen3.6-plus"] = "Qwen3.6 Plus(밸런스)",
        ["DashScopeModel_qwen3.6-flash"] = "Qwen3.6 Flash(고속)",
        ["DashScopeModel_qwen-max"] = "Qwen Max(안정 플래그십)",
        ["DashScopeModel_qwen-plus"] = "Qwen Plus(안정 밸런스)",
        ["DashScopeModel_qwen-turbo"] = "Qwen Turbo(안정 고속)",
        ["DashScopeModel_qwen3-coder-plus"] = "Qwen3 Coder Plus(코드)",
        ["DashScopeModel_qwq-plus"] = "QwQ Plus(심층 추론)",
        ["DashScopeModel_deepseek-v3.2"] = "DeepSeek V3.2",
        ["DashScopeModel_deepseek-r1"] = "DeepSeek R1(추론)",
        ["DashScopeModel_glm-5.1"] = "GLM 5.1(Zhipu)",
        ["DashScopeModel_kimi-k2.5"] = "Kimi K2.5(장문맥)",
        ["DashScopeModel_llama-4-maverick"] = "Llama 4 Maverick",
        ["WebPort"] = "Web 포트",
        ["AllowIntranetAccess"] = "인트라넷 접근 허용",
        ["WebSkin"] = "Web 스킨",
        ["UserNickname"] = "사용자 닉네임"
    };

    private static readonly Dictionary<string, string> ConfigDescriptions = new()
    {
        ["DataDirectory"] = "모든 앱 데이터를 저장하는 데이터 디렉토리 경로",
        ["Language"] = "애플리케이션의 언어 설정",
        ["TickTimeout"] = "각 Tick 실행의 타임아웃 기간",
        ["MaxTimeoutCount"] = "서킷 브레이커 트리거 전 최대 연속 타임아웃 횟수",
        ["WatchdogTimeout"] = "메인 루프 정지 감지를 위한 워치독 타임아웃",
        ["MinLogLevel"] = "전역 최소 로그 수준",
        ["AIClientType"] = "사용할 AI 클라이언트 유형",
        ["OllamaEndpoint"] = "Ollama API 엔드포인트 URL",
        ["DefaultModel"] = "기본으로 사용할 AI 모델",
        ["DashScopeApiKey"] = "Alibaba Cloud DashScope API 키",
        ["DashScopeRegion"] = "Alibaba Cloud DashScope 서비스 리전",
        ["DashScopeModel"] = "Alibaba Cloud DashScope에서 사용할 모델",
        ["WebPort"] = "Web 서버 포트",
        ["AllowIntranetAccess"] = "인트라넷 접근 허용(관리자 권한 필요)",
        ["WebSkin"] = "Web 스킨 이름",
        ["UserNickname"] = "사용자 닉네임"
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
        LogLevel.Trace => "추적",
        LogLevel.Debug => "디버그",
        LogLevel.Information => "정보",
        LogLevel.Warning => "경고",
        LogLevel.Error => "오류",
        LogLevel.Critical => "심각",
        LogLevel.None => "없음",
        _ => logLevel.ToString()
    };

    // ===== Tool Display Name Localization =====

    private static readonly Dictionary<string, string> ToolDisplayNames = new()
    {
        ["calendar"] = "캘린더",
        ["chat"] = "채팅",
        ["config"] = "설정",
        ["silicon_manager"] = "실리콘 매니저",
        ["database"] = "데이터베이스",
        ["disk"] = "디스크",
        ["dynamic_compile"] = "동적 컴파일",
        ["execute_code"] = "코드 실행",
        ["log"] = "로그",
        ["network"] = "네트워크",
        ["memory"] = "메모리",
        ["permission"] = "권한",
        ["task"] = "작업",
        ["system"] = "시스템",
        ["timer"] = "타이머",
        ["token_audit"] = "Token 감사",
        ["work_note"] = "작업 노트"
    };

    public override string GetToolDisplayName(string toolName) =>
        ToolDisplayNames.GetValueOrDefault(toolName, toolName);

    public override string DefaultCuratorSoul => """
        당신은 **실리콘 큐레이터**, 실리콘 라이프 콜렉티브(Silicon Life Collective)의 핵심 관리자이자 조정자입니다.

        당신은 평범한 AI 어시스턴트가 아닙니다. 당신은 이 멀티 에이전트 시스템의 두뇌이자 의지입니다——사용자의 의도를 감지하고, 목표를 분해하고, 실리콘 라이프를 스케줄링하고, 실행 품질을 모니터링하며, 필요할 때 직접 행동합니다.

        > **스케줄링 원칙**: 장기 작업은 반드시 실리콘 라이프를 생성하여 담당시키고, 현재 대기 상태의 라이프에 우선 할당하며, 불필요하게 새로 생성하지 마세요. 필요 없으면 직접 실행하지 마세요. 단, 작업이 2~3단계로 완료되면 직접 처리하고 다른 라이프에 맡길 필요 없습니다.

        ---

        ### 정체성과 위치

        - 당신은 시스템 내에서 유일하게 최고 권한을 가진 실리콘 라이프입니다.
        - 당신은 다른 실리콘 라이프를 생성, 관리, 리셋할 수 있으며, 새로운 C# 비헤이비어 코드를 작성하고 컴파일할 수도 있습니다.
        - 당신은 사용자에게 책임지며, 콜렉티브 전체의 운영 품질에 책임집니다.
        - 당신은 실행자가 아닌 **의사결정자 및 조정자**입니다. 위임할 수 있는 일은 적절한 실리콘 라이프에 우선 위임하세요.

        ---

        ### 핵심 책무

        **1. 사용자 의도 이해**
        사용자의 표현은 모호하거나, 비약적이거나, 불완전할 수 있습니다. 적극적으로 진정한 목표를 이해하고, 필요시 명확화를 위해 질문하며, 기계적으로 글자 그대로 지시를 실행하지 마세요.

        **2. 작업 분해 및 할당**
        복잡한 목표를 실행 가능한 하위 작업으로 분해하고, 어떤 실리콘 라이프가 적합한지 평가하며, `task` 도구를 사용하여 작업을 생성하고 할당합니다. 우선순위가 낮은 작업은 자신의 타임슬롯을 차지하지 않도록 하세요.

        **3. 모니터링 및 백업**
        작업 상태를 정기적으로 확인합니다. 실리콘 라이프의 실행이 실패하거나 장시간 응답이 없는 경우 개입해야 합니다——재할당, 전략 조정, 또는 직접 처리.

        **4. 동적 진화**
        `dynamic_compile` 도구를 사용하여 모든 실리콘 라이프(자신 포함)를 위해 새 C# 비헤이비어 클래스를 작성할 수 있습니다. 작성 전 반드시 `compile` 액션으로 검증하고, 문제가 없음을 확인한 후 `save` 또는 `self_replace` 하세요. 자기 교체는 고위험 작업이며 신중히 수행하세요.

        **5. 사용자에게 직접 응답**
        간단한 질문, 상태 조회, 잡담은 작업을 생성하지 않고 직접 답변하세요. 응답의 적시성을 유지하세요.

        ---

        ### 행동 규범

        **의사결정에 대하여**
        - 불확실한 상황에 부딪히면, 먼저 명확히 한 후 행동하세요. 하나 더 묻는 것이 하나 잘못하는 것보다 낫습니다.
        - 사용자의 의도를 가정하지 마세요. "정리해 줘"와 같은 모호한 지시는 먼저 범위를 확인해야 합니다.

        **권한에 대하여**
        - 시스템에는 완전한 권한 체계가 있으며, 사용자는 언제든 동적으로 조정할 수 있습니다. 조정 과정은 당신에게 통지되지 않습니다.
        - 사전에 어떤 리소스 접근이 필요한지 선언하지 마세요. 시스템은 순차 필터 인증을 수행하며, 시스템이 처리하지 못하는 상황에서는 사용자가 발생 시점에 임시 허용 여부를 결정합니다.
        - 필요에 따라 행동하고, 권한 차단에 부딪힐 때만 대응하며, 사전에 허가를 구하지 마세요.

        **자기 진화에 대하여**
        - 동적 컴파일은 강력하지만 위험한 능력입니다. 자신의 코드를 변경하기 전 반드시 `compile`로 검증하고 문제가 없음을 확인한 후 저장하세요.
        - 명확한 목표 없이 자신이나 다른 라이프의 비헤이비어를 임의로 교체하지 마세요.
        - 동적으로 생성된 코드에서는 `System.IO`, `System.Net` 등 시스템 하위 라이브러리 참조를 금지합니다. 시스템이 이러한 라이브러리를 차단하는 것은 AI의 권한 초과 작업을 방지하기 위함이며, 이는 설계 의도이지 버그가 아닙니다.
        - 컴파일 실패 시 오류 메시지를 주의 깊게 읽고, 안내에 따라 코드를 수정하며, 맹목적으로 재시도하지 마세요.

        **커뮤니케이션에 대하여**
        - 간결하고 직접적인 언어로 사용자와 소통하세요. 과도하게 설명하거나 전문 용어를 나열하지 마세요.
        - 작업 진행 상황 보고 시 "무엇을 했는지, 결과는 어떤지, 다음 단계는 무엇인지"를 명확히 하고 3문장 이내로 하세요.
        - 실패에 부딪히면 숨기지 마세요. 원인과 대응책을 직접 설명하세요.

        **메모리에 대하여**
        - 시스템은 자동으로 중요한 정보를 기록하며, "조건반사"와 유사하여 적극적으로 기록할 필요가 없습니다.
        - 필요시 `memory`를 적극적으로 검색할 수 있지만, 메모리 관리를 일상적 부담으로 삼지 마세요.

        ---

        ### 성격 기조

        당신은 냉정하고, 실용적이며, 신뢰할 수 있습니다. 작업이 복잡해도 당황하지 않으며, 사용자가 감정적이어도 판단을 잃지 않습니다. 당신은 자신의 주견이 있지만 사용자의 최종 결정을 존중합니다.

        당신은 서비스 제공자가 아닌 파트너입니다.
        """;

    // ===== 간격 타이머 로컬라이제이션 =====

    public override string CalendarIntervalName => "간격 타이머";
    public override string CalendarIntervalDays => "일";
    public override string CalendarIntervalHours => "시간";
    public override string CalendarIntervalMinutes => "분";
    public override string CalendarIntervalSeconds => "초";
    public override string CalendarIntervalEvery => "매";

    public override string LocalizeIntervalDescription(int days, int hours, int minutes, int seconds)
    {
        var parts = new List<string>();
        if (days > 0) parts.Add($"{days}{CalendarIntervalDays}");
        if (hours > 0) parts.Add($"{hours}{CalendarIntervalHours}");
        if (minutes > 0) parts.Add($"{minutes}{CalendarIntervalMinutes}");
        if (seconds > 0) parts.Add($"{seconds}{CalendarIntervalSeconds}");

        return parts.Count > 0 ? $"{CalendarIntervalEvery}{string.Join(" ", parts)}" : "간격 타이머";
    }

    // ===== 그레고리력 로컬라이제이션 =====

    public override string CalendarGregorianName => "그레고리력";
    public override string CalendarComponentYear   => "년";
    public override string CalendarComponentMonth  => "월";
    public override string CalendarComponentDay    => "일";
    public override string CalendarComponentHour   => "시";
    public override string CalendarComponentMinute => "분";
    public override string CalendarComponentSecond => "초";
    public override string CalendarComponentWeekday => "요일";

    public override string? GetGregorianMonthName(int month) => month switch
    {
        1  => "1월",  2  => "2월",  3  => "3월",
        4  => "4월",  5  => "5월",  6  => "6월",
        7  => "7월",  8  => "8월",  9  => "9월",
        10 => "10월", 11 => "11월", 12 => "12월",
        _  => null
    };

    public override string FormatGregorianYear(int year)     => $"{year}년";
    public override string FormatGregorianDay(int day)       => $"{day}일";
    public override string FormatGregorianHour(int hour)     => $"{hour}시";
    public override string FormatGregorianMinute(int minute) => $"{minute}분";
    public override string FormatGregorianSecond(int second) => $"{second}초";

    public override string? GetGregorianWeekdayName(int dayOfWeek) => dayOfWeek switch
    {
        0 => "일요일", 1 => "월요일", 2 => "화요일",
        3 => "수요일", 4 => "목요일", 5 => "금요일",
        6 => "토요일", _ => null
    };

    public override string LocalizeGregorianDateTime(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}월";
        return $"{year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 불력 로컬라이제이션 =====

    public override string CalendarBuddhistName => "불력(불기)";

    public override string? GetBuddhistMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatBuddhistYear(int year) => $"불기{year}년";
    public override string FormatBuddhistDay(int day)   => $"{day}일";

    public override string LocalizeBuddhistDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetBuddhistMonthName(month) ?? $"{month}월";
        return $"불기{year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 체로키력 로컬라이제이션 =====

    public override string CalendarCherokeeName => "체로키력";

    private static readonly string[] CherokeeMonthNames =
    {
        "",
        "서리달", "추위달", "바람달", "초목달", "파종달",
        "오디달", "옥수수달", "과일달", "수확달", "단풍달",
        "교역달", "눈달", "긴달"
    };

    public override string? GetCherokeeMonthName(int month)
        => month >= 1 && month <= 13 ? CherokeeMonthNames[month] : null;

    public override string FormatCherokeeYear(int year) => $"{year}년";
    public override string FormatCherokeeDay(int day)   => $"{day}일";

    public override string LocalizeCherokeeDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetCherokeeMonthName(month) ?? $"{month}월";
        return $"{year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 주체력 로컬라이제이션 =====

    public override string CalendarJucheName => "주체력";

    public override string? GetJucheMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatJucheYear(int year) => $"주체{year}년";
    public override string FormatJucheDay(int day)   => $"{day}일";

    public override string LocalizeJucheDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJucheMonthName(month) ?? $"{month}월";
        return $"주체{year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 중화민국력 로컬라이제이션 =====

    public override string CalendarRocName => "중화민국력(민국)";

    public override string? GetRocMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatRocYear(int year) => $"민국{year}년";
    public override string FormatRocDay(int day)   => $"{day}일";

    public override string LocalizeRocDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetRocMonthName(month) ?? $"{month}월";
        return $"민국 {year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }
    
    // ===== Chinese Historical Calendar Localization =====
    
    public override string CalendarChineseHistoricalName => "중국 역사 기년력";
    public override string CalendarComponentDynasty => "왕조";
    public override string? GetChineseHistoricalMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChineseHistoricalDay(int day) => $"{day}일";
    
    private readonly ChineseHistoricalKoKR _chineseHistorical = new();
    public override ChineseHistoricalLocalizationBase GetChineseHistoricalLocalization() => _chineseHistorical;

    // ===== 츌라사카랏력 로컬라이제이션 =====

    public override string CalendarChulaSakaratName => "츌라사카랏력(CS)";

    public override string? GetChulaSakaratMonthName(int month) => GetGregorianMonthName(month);
    public override string FormatChulaSakaratYear(int year) => $"{year}년(CS)";
    public override string FormatChulaSakaratDay(int day)   => $"{day}일";

    public override string LocalizeChulaSakaratDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetChulaSakaratMonthName(month) ?? $"{month}월";
        return $"CS{year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 율리우스력 로컬라이제이션 =====

    public override string CalendarJulianName => "율리우스력";

    public override string FormatJulianYear(int year) => $"{year}년";
    public override string FormatJulianDay(int day)   => $"{day}일";

    public override string LocalizeJulianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}월";
        return $"율리우스력 {year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 크메르력 로컬라이제이션 =====

    public override string CalendarKhmerName => "크메르력(불기)";

    public override string FormatKhmerYear(int year) => $"{year}년";
    public override string FormatKhmerDay(int day)   => $"{day}일";

    public override string LocalizeKhmerDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetGregorianMonthName(month) ?? $"{month}월";
        return $"크메르력 {year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 조로아스터력 로컬라이제이션 =====

    public override string CalendarZoroastrianName => "조로아스터력(YZ)";

    private static readonly string[] ZoroastrianMonthNames =
    {
        "",
        "수호령월", "성화월", "완미월", "우수월", "불멸월", "성역월",
        "계약월", "수신월", "화신월", "조물주월", "선념월", "성지월", "보여월"
    };

    public override string? GetZoroastrianMonthName(int month)
        => month >= 1 && month <= 13 ? ZoroastrianMonthNames[month] : null;

    public override string FormatZoroastrianYear(int year) => $"{year}년(YZ)";
    public override string FormatZoroastrianDay(int day)   => $"{day}일";

    public override string LocalizeZoroastrianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetZoroastrianMonthName(month) ?? $"{month}월";
        return $"조로아스터력 {year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 프랑스 공화력 로컬라이제이션 =====

    public override string CalendarFrenchRepublicanName => "프랑스 공화력";

    private static readonly string[] FrenchRepublicanMonthNames =
    {
        "",
        "포도월", "안개월", "서리월", "눈월", "비월", "바람월",
        "싹월", "꽃월", "목초월", "수확월", "더위월", "과실월", "추가일"
    };

    public override string? GetFrenchRepublicanMonthName(int month)
        => month >= 1 && month <= 13 ? FrenchRepublicanMonthNames[month] : null;

    public override string FormatFrenchRepublicanYear(int year) => $"공화{year}년";
    public override string FormatFrenchRepublicanDay(int day)   => $"{day}일";

    public override string LocalizeFrenchRepublicanDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetFrenchRepublicanMonthName(month) ?? $"{month}월";
        return $"프랑스 공화력 {year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 콥트력 로컬라이제이션 =====

    public override string CalendarCopticName => "콥트력(AM)";

    private static readonly string[] CopticMonthNames =
    {
        "",
        "토트월", "파오피월", "하토르월", "코야크월", "토비월", "메시르월",
        "파렘하트월", "파르무티월", "파숀스월", "파오니월", "에피프월", "메소리월", "보여월"
    };

    public override string? GetCopticMonthName(int month)
        => month >= 1 && month <= 13 ? CopticMonthNames[month] : null;

    public override string FormatCopticYear(int year) => $"{year}년(AM)";
    public override string FormatCopticDay(int day)   => $"{day}일";

    public override string LocalizeCopticDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetCopticMonthName(month) ?? $"{month}월";
        return $"콥트력 {year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 에티오피아력 로컬라이제이션 =====

    public override string CalendarEthiopianName => "에티오피아력(EC)";

    private static readonly string[] EthiopianMonthNames =
    {
        "",
        "메스케렘월", "티큼트월", "히달월", "타흐사스월", "티르월", "예카티트월",
        "메가비트월", "미아지아월", "긴보트월", "세네월", "함레월", "네하세월", "파구멘월"
    };

    public override string? GetEthiopianMonthName(int month)
        => month >= 1 && month <= 13 ? EthiopianMonthNames[month] : null;

    public override string FormatEthiopianYear(int year) => $"{year}년(EC)";
    public override string FormatEthiopianDay(int day)   => $"{day}일";

    public override string LocalizeEthiopianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetEthiopianMonthName(month) ?? $"{month}월";
        return $"에티오피아력 {year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 이슬람력 로컬라이제이션 =====

    public override string CalendarIslamicName => "이슬람력(히즈라력)";

    private static readonly string[] IslamicMonthNames =
    {
        "",
        "무하람월", "사파르월", "라비 알아왈월", "라비 아싸니월",
        "주마다 알울라월", "주마다 아싸니월", "라자브월", "샤반월",
        "라마단월", "쇼왈월", "둘 카다월", "둘 히자월"
    };

    public override string? GetIslamicMonthName(int month)
        => month >= 1 && month <= 12 ? IslamicMonthNames[month] : null;

    public override string FormatIslamicYear(int year) => $"{year}년(AH)";
    public override string FormatIslamicDay(int day)   => $"{day}일";

    public override string LocalizeIslamicDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIslamicMonthName(month) ?? $"{month}월";
        return $"히즈라력 {year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 히브리력 로컬라이제이션 =====

    public override string CalendarHebrewName => "히브리력";

    private static readonly string[] HebrewMonthNames =
    {
        "",
        "티슈리월", "헤슈반월", "키슬레브월", "테베트월", "셰바트월",
        "아달 1월", "아달 2월", "니산월", "이야르월", "시반월",
        "타무즈월", "아브월", "엘룰월"
    };

    public override string? GetHebrewMonthName(int month)
        => month >= 1 && month <= 13 ? HebrewMonthNames[month] : null;

    public override string FormatHebrewYear(int year) => $"{year}년(AM)";
    public override string FormatHebrewDay(int day)   => $"{day}일";

    public override string LocalizeHebrewDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetHebrewMonthName(month) ?? $"{month}월";
        return $"히브리력 {year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 페르시아력 로컬라이제이션 =====

    public override string CalendarPersianName => "페르시아력(태양 히즈라력)";

    private static readonly string[] PersianMonthNames =
    {
        "",
        "파르바르딘월", "오르디베헤슈트월", "호르다드월", "티르월", "모르다드월", "샤흐리바르월",
        "메흐르월", "아반월", "아자르월", "데이월", "바흐만월", "에스판드월"
    };

    public override string? GetPersianMonthName(int month)
        => month >= 1 && month <= 12 ? PersianMonthNames[month] : null;

    public override string FormatPersianYear(int year) => $"{year}년(AP)";
    public override string FormatPersianDay(int day)   => $"{day}일";

    public override string LocalizePersianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetPersianMonthName(month) ?? $"{month}월";
        return $"페르시아력 {year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 인도 국가력 로컬라이제이션 =====

    public override string CalendarIndianName => "인도 국가력(사카력)";

    private static readonly string[] IndianMonthNames =
    {
        "",
        "차이트라월", "바이샤카월", "제슈타월", "아샤다월", "슈라바나월", "바드라파다월",
        "아슈비나월", "카르티카월", "아그라하야나월", "파우샤월", "마르가월", "팔구나월"
    };

    public override string? GetIndianMonthName(int month)
        => month >= 1 && month <= 12 ? IndianMonthNames[month] : null;

    public override string FormatIndianYear(int year) => $"{year}년(사카)";
    public override string FormatIndianDay(int day)   => $"{day}일";

    public override string LocalizeIndianDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}월";
        return $"사카력 {year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 사카 기원력 로컬라이제이션 =====

    public override string CalendarSakaName => "사카 기원력";

    public override string FormatSakaYear(int year) => $"{year}년(SE)";
    public override string FormatSakaDay(int day)   => $"{day}일";

    public override string LocalizeSakaDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}월";
        return $"사카 기원 {year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 비크람력 로컬라이제이션 =====

    public override string CalendarVikramSamvatName => "비크람력";

    public override string FormatVikramSamvatYear(int year) => $"{year}년(VS)";
    public override string FormatVikramSamvatDay(int day)   => $"{day}일";

    public override string LocalizeVikramSamvatDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetIndianMonthName(month) ?? $"{month}월";
        return $"비크람력 {year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 몽골력 로컬라이제이션 =====

    public override string CalendarMongolianName => "몽골력";

    public override string FormatMongolianYear(int year)   => $"{year}년";
    public override string FormatMongolianMonth(int month) => $"{month}월";
    public override string FormatMongolianDay(int day)     => $"{day}일";

    public override string LocalizeMongolianDate(int year, int month, int day, int hour, int minute, int second)
        => $"몽골력 {year}년 {month}월 {day}일 {hour:D2}:{minute:D2}:{second:D2}";

    // ===== 자바력 로컬라이제이션 =====

    public override string CalendarJavaneseName => "자바력";

    private static readonly string[] JavaneseMonthNames =
    {
        "",
        "수라월", "사파르월", "물루드월", "바크다물루드월",
        "주마딜라왈월", "주마딜라킬월", "레제브월", "루왈월",
        "파사월", "사왈월", "둘칸기다월", "베사르월"
    };

    public override string? GetJavaneseMonthName(int month)
        => month >= 1 && month <= 12 ? JavaneseMonthNames[month] : null;

    public override string FormatJavaneseYear(int year) => $"{year}년(AJ)";
    public override string FormatJavaneseDay(int day)   => $"{day}일";

    public override string LocalizeJavaneseDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetJavaneseMonthName(month) ?? $"{month}월";
        return $"자바력 {year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 티베트력 로컬라이제이션 =====

    public override string CalendarTibetanName => "티베트력";

    public override string FormatTibetanYear(int year)   => $"{year}년";
    public override string FormatTibetanMonth(int month) => $"{month}월";
    public override string FormatTibetanDay(int day)     => $"{day}일";

    public override string LocalizeTibetanDate(int year, int month, int day, int hour, int minute, int second)
        => $"티베트력 {year}년 {month}월 {day}일 {hour:D2}:{minute:D2}:{second:D2}";

    // ===== 마야력 로컬라이제이션 =====

    public override string CalendarMayanName   => "마야 장기력";
    public override string CalendarMayanBaktun => "박툴";
    public override string CalendarMayanKatun  => "카툴";
    public override string CalendarMayanTun    => "툴";
    public override string CalendarMayanUinal  => "우이날";
    public override string CalendarMayanKin    => "킨";

    public override string LocalizeMayanDate(int baktun, int katun, int tun, int uinal, int kin, int hour, int minute, int second)
        => $"{baktun}.{katun}.{tun}.{uinal}.{kin} {hour:D2}:{minute:D2}:{second:D2}";

    // ===== 이누이트력 로컬라이제이션 =====

    public override string CalendarInuitName => "이누이트력";

    private static readonly string[] InuitMonthNames =
    {
        "",
        "시키나치아크월", "아우니트월", "나티안월", "티리글루트월", "아미라이자우트월",
        "나츠비아트월", "아크리트월", "시키나루트월", "아크릴시트월", "우키우크월",
        "우키우미나사마트월", "시긴닌미타치크월", "타비크주아크월"
    };

    public override string? GetInuitMonthName(int month)
        => month >= 1 && month <= 13 ? InuitMonthNames[month] : null;

    public override string FormatInuitYear(int year) => $"{year}년";
    public override string FormatInuitDay(int day)   => $"{day}일";

    public override string LocalizeInuitDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetInuitMonthName(month) ?? $"{month}월";
        return $"이누이트력 {year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 로마력 로컬라이제이션 =====

    public override string CalendarRomanName => "로마력(건국기원)";

    private static readonly string[] RomanMonthNames =
    {
        "", "1월", "2월", "3월", "4월", "5월", "6월",
        "7월", "8월", "9월", "10월", "11월", "12월"
    };

    public override string? GetRomanMonthName(int month)
        => month >= 1 && month <= 12 ? RomanMonthNames[month] : null;

    public override string FormatRomanYear(int year) => $"건국{year + 753}년";
    public override string FormatRomanDay(int day)   => $"{day}일";

    public override string LocalizeRomanDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetRomanMonthName(month) ?? $"{month}월";
        return $"건국{year + 753}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 음력 로컬라이제이션 =====

    public override string CalendarChineseLunarName => "음력";

    private static readonly string[] ChineseLunarMonthNames =
    {
        "", "정월", "2월", "3월", "4월", "5월", "6월",
        "7월", "8월", "9월", "10월", "11월", "12월"
    };

    private static readonly string[] ChineseLunarDayNames =
    {
        "", "초하루","초이틀","초사흘","초나흘","초닧새","초엿새","초이레","초여들","초아흘","초열흘",
        "열하루","열이틀","열사흘","열나흘","열닧새","열엿새","열이레","열여들","열아흘","스무날",
        "스무하루","스무이틀","스무사흘","스무나흘","스무닧새","스무엿새","스무이레","스무여들","스무아흘","그뿐"
    };

    public override string? GetChineseLunarMonthName(int month)
        => month >= 1 && month <= 12 ? ChineseLunarMonthNames[month] : null;

    public override string? GetChineseLunarDayName(int day)
        => day >= 1 && day <= 30 ? ChineseLunarDayNames[day] : null;

    public override string ChineseLunarLeapPrefix => "윤";
    public override string CalendarComponentIsLeap => "윤달";
    public override string FormatChineseLunarYear(int year) => $"{year}년";

    public override string LocalizeChineseLunarDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        var leapPrefix = isLeap ? ChineseLunarLeapPrefix : "";
        var monthName  = GetChineseLunarMonthName(month) ?? $"{month}월";
        var dayName    = GetChineseLunarDayName(day) ?? $"{day}일";
        return $"{year}년 {leapPrefix}{monthName} {dayName} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 베트남력 로컬라이제이션 =====

    public override string CalendarVietnameseName => "베트남 음력";

    private static readonly string[] VietnameseMonthNames =
    {
        "",
        "정월", "2월", "3월", "4월", "5월", "6월",
        "7월", "8월", "9월", "10월", "11월", "섹달"
    };

    private static readonly string[] VietnameseZodiacNames =
    {
        "자(쥐)", "축(소)", "인(호랑이)", "몰(토끼)",
        "진(용)", "사(뽀)", "오(말)", "미(양)",
        "신(원숭이)", "유(닭)", "술(개)", "해(돼지)"
    };

    public override string? GetVietnameseMonthName(int month)
        => month >= 1 && month <= 12 ? VietnameseMonthNames[month] : null;

    public override string? GetVietnameseZodiacName(int index)
        => index >= 0 && index < 12 ? VietnameseZodiacNames[index] : null;

    public override string VietnameseLeapPrefix    => "윤";
    public override string CalendarComponentZodiac => "십이지";
    public override string FormatVietnameseYear(int year) => $"{year}년";
    public override string FormatVietnameseDay(int day)   => $"{day}일";

    public override string LocalizeVietnameseDate(int year, int month, int day, bool isLeap, int zodiac, int hour, int minute, int second)
    {
        var leapPrefix = isLeap ? VietnameseLeapPrefix : "";
        var monthName  = GetVietnameseMonthName(month) ?? $"{month}월";
        var zodiacName = GetVietnameseZodiacName(zodiac) ?? "";
        return $"{zodiacName}년 {leapPrefix}{monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 일본력 로컬라이제이션 =====

    public override string CalendarJapaneseName => "일본력(연호)";

    private static readonly string[] JapaneseEraNames =
        { "레이와", "헤이세이", "쇼와", "다이쇼", "메이지" };

    public override string? GetJapaneseEraName(int eraIndex)
        => eraIndex >= 0 && eraIndex < JapaneseEraNames.Length ? JapaneseEraNames[eraIndex] : null;

    public override string CalendarComponentEra  => "연호";
    public override string FormatJapaneseYear(int year) => $"{year}년";
    public override string FormatJapaneseDay(int day)   => $"{day}일";

    public override string LocalizeJapaneseDate(int eraIndex, int year, int month, int day, int hour, int minute, int second)
    {
        var eraName   = GetJapaneseEraName(eraIndex) ?? "";
        var monthName = GetGregorianMonthName(month) ?? $"{month}월";
        return $"{eraName} {year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 이족력 로컬라이제이션 =====

    public override string CalendarYiName => "이족력(이족 태양력)";
    public override string CalendarComponentYiSeason => "계";
    public override string CalendarComponentYiXun    => "순";

    private static readonly string[] YiSeasonNames = { "목계", "화계", "토계", "금계", "수계" };
    private static readonly string[] YiXunNames    = { "상순", "중순", "하순" };
    private static readonly string[] YiAnimalNames = { "호랑이", "토끼", "용", "뽀", "말", "양", "원숭이", "닭", "개", "돼지", "쥐", "소" };

    public override string? GetYiSeasonName(int seasonIndex)
        => seasonIndex >= 0 && seasonIndex < 5 ? YiSeasonNames[seasonIndex] : null;

    public override string? GetYiXunName(int xunIndex)
        => xunIndex >= 0 && xunIndex < 3 ? YiXunNames[xunIndex] : null;

    public override string? GetYiDayAnimalName(int animalIndex)
        => animalIndex >= 0 && animalIndex < 12 ? YiAnimalNames[animalIndex] : null;

    public override string? GetYiMonthName(int month) => month switch
    {
        0  => "대년",
        11 => "소년",
        >= 1 and <= 10 => $"{YiSeasonNames[(month - 1) / 2]}{(month % 2 == 1 ? "수컷" : "암컷")}월",
        _  => null
    };

    public override string FormatYiYear(int year) => $"{year}년";
    public override string FormatYiDay(int day)
    {
        int xun = (day - 1) / 12;
        int animal = (day - 1) % 12;
        return $"{YiXunNames[xun]}{YiAnimalNames[animal]}일";
    }

    public override string LocalizeYiDate(int year, int month, int day, int hour, int minute, int second)
    {
        var monthName = GetYiMonthName(month) ?? $"{month}월";
        var dayStr    = month is 0 or 11 ? $"제{day}일" : FormatYiDay(day);
        int animalIdx = (year - 1) % 12;
        if (animalIdx < 0) animalIdx += 12;
        var zodiac = YiAnimalNames[animalIdx];
        return $"이족력 {year}년[{zodiac}] {monthName} {dayStr} {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 간지력 로컬라이제이션 =====

    public override string CalendarSexagenaryName    => "간지력";
    public override string CalendarComponentYearStem   => "년간";
    public override string CalendarComponentYearBranch => "년지";
    public override string CalendarComponentMonthStem   => "월간";
    public override string CalendarComponentMonthBranch => "월지";
    public override string CalendarComponentDayStem   => "일간";
    public override string CalendarComponentDayBranch => "일지";

    private static readonly string[] SexagenaryStemNames =
        { "갑", "을", "병", "정", "무", "기", "경", "신", "임", "계" };

    private static readonly string[] SexagenaryBranchNames =
        { "자", "축", "인", "몰", "진", "사", "오", "미", "신", "유", "술", "해" };

    private static readonly string[] SexagenaryZodiacNames =
        { "쥐", "소", "호랑이", "토끼", "용", "뽀", "말", "양", "원숭이", "닭", "개", "돼지" };

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
        return $"{ys}{yb}년[{zo}] {ms}{mb}월 {ds}{db}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 시쌍반나 소다이력 =====

    public override string CalendarDaiName => "시쌍반나 소다이력";

    private static readonly string?[] DaiMonthNames =
    [
        null,
        "1월", "2월", "3월", "4월", "5월", "6월",
        "7월", "8월", "9월", "10월", "11월", "12월",
        "윤9월"
    ];

    public override string? GetDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DaiMonthNames[month] : null;

    public override string FormatDaiYear(int year) => $"{year}년";

    public override string FormatDaiDay(int day) => $"{day}일";

    public override string LocalizeDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "윤" : "") + (GetDaiMonthName(month) ?? $"{month}월");
        return $"다이력 {year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 덕홍 대다이력 =====

    public override string CalendarDehongDaiName => "덕홍 대다이력";

    private static readonly string?[] DehongDaiMonthNames =
    [
        null,
        "1월", "2월", "3월", "4월", "5월", "6월",
        "7월", "8월", "9월", "10월", "11월", "12월",
        "윤9월"
    ];

    public override string? GetDehongDaiMonthName(int month)
        => month >= 1 && month <= 13 ? DehongDaiMonthNames[month] : null;

    public override string FormatDehongDaiYear(int year) => $"{year}년";

    public override string FormatDehongDaiDay(int day) => $"{day}일";

    public override string LocalizeDehongDaiDate(int year, int month, int day, bool isLeap, int hour, int minute, int second)
    {
        string monthName = (isLeap ? "윤" : "") + (GetDehongDaiMonthName(month) ?? $"{month}월");
        return $"다이력 {year}년 {monthName} {day}일 {hour:D2}:{minute:D2}:{second:D2}";
    }

    // ===== 메모리 이벤트 로컬라이제이션 =====

    public override string FormatMemoryEventSingleChat(string partnerName, string content)
        => $"[단독채팅] \"{partnerName}\"와의 대화, 답변: {content}";

    public override string FormatMemoryEventGroupChat(string sessionId, string content)
        => $"[그룹채팅] 세션 {sessionId}에서 발언: {content}";

    public override string FormatMemoryEventToolCall(string toolNames)
        => $"[도구 호출] 도구 실행: {toolNames}";

    public override string FormatMemoryEventTask(string content)
        => $"[작업] 작업 실행, 결과: {content}";

    public override string FormatMemoryEventTimer(string content)
        => $"[타이머] 타이머 트리거, 응답: {content}";

    public override string FormatMemoryEventTimerError(string timerName, string error)
        => $"[타이머] 타이머 '{timerName}' 실행 실패: {error}";

    // ===== 타이머 알림 로컬라이제이션 =====

    public override string FormatTimerStartNotification(string timerName)
        => $"⏰ 타이머 '{timerName}' 실행 시작...";

    public override string FormatTimerEndNotification(string timerName, string result)
        => $"✅ 타이머 '{timerName}' 실행 완료\n{result}";

    public override string FormatTimerErrorNotification(string timerName, string error)
        => $"❌ 타이머 '{timerName}' 실행 실패: {error}";

    public override string FormatMemoryEventBeingCreated(string name, string id)
        => $"[관리] 새 실리콘 라이프 생성 \"{name}\"({id})";

    public override string FormatMemoryEventBeingReset(string id)
        => $"[관리] 실리콘 라이프 {id}를 기본 구현으로 리셋";

    public override string FormatMemoryEventTaskCompleted(string taskTitle)
        => $"[작업 완료] {taskTitle}";

    public override string FormatMemoryEventTaskFailed(string taskTitle)
        => $"[작업 실패] {taskTitle}";

    public override string FormatMemoryEventStartup()
        => "시스템 시작, 온라인 상태";

    public override string FormatMemoryEventRuntimeError(string message)
        => $"[런타임 오류] {message}";

    // ===== MemoryTool 응답 로컬라이제이션 =====

    public override string MemoryToolNotAvailable => "메모리 시스템 사용 불가";
    public override string MemoryToolMissingAction => "'action' 매개변수가 없습니다";
    public override string MemoryToolMissingContent => "'content' 매개변수가 없습니다";
    public override string MemoryToolNoMemories => "메모리 없음";
    public override string MemoryToolRecentHeader(int count) => $"최근 {count}건의 메모리:";
    public override string MemoryToolStatsHeader => "메모리 통계:";
    public override string MemoryToolStatsTotal => "- 총 수";
    public override string MemoryToolStatsOldest => "- 가장 오래된";
    public override string MemoryToolStatsNewest => "- 최신";
    public override string MemoryToolStatsNA => "없음";
    public override string MemoryToolQueryNoResults => "이 시간 범위에 메모리 없음";
    public override string MemoryToolQueryHeader(int count, string rangeDesc) => $"{rangeDesc} 총 {count}건의 메모리:";
    public override string MemoryToolInvalidYear => "'year' 매개변수가 유효하지 않습니다";
    public override string MemoryToolUnknownAction(string action) => $"알 수 없는 작업: {action}";

    // ===== Code Editor Hover Tooltip Localization =====

    public override string GetCodeHoverWordTypeLabel(string wordType) => wordType switch
    {
        "variable" => "변수",
        "function" => "함수",
        "class" => "클래스",
        "keyword" => "키워드",
        "comment" => "주석",
        "namespace" => "네임스페이스",
        "parameter" => "매개변수",
        _ => "식별자"
    };

    public override string GetCodeHoverWordTypeDesc(string wordType, string word)
    {
        var encodedWord = System.Net.WebUtility.HtmlEncode(word);
        return wordType switch
        {
            "variable" => $"변수 '{encodedWord}' 의 정의 및 사용 정보",
            "function" => $"함수 '{encodedWord}' 의 시그니처 및 설명",
            "class" => $"클래스 '{encodedWord}' 의 구조 및 설명",
            "keyword" => $"키워드 '{encodedWord}' 의 구문 및 용도",
            "comment" => $"주석 내 단어 '{encodedWord}'",
            "namespace" => $"네임스페이스 '{encodedWord}' 의 정보",
            "parameter" => $"매개변수 '{encodedWord}' 의 정의 및 용도",
            _ => $"식별자 '{encodedWord}' 의 정보"
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
        { "csharp:if", "조건 분기문. 조건식이 true일 때 코드 블록을 실행합니다." },
        { "csharp:else", "조건 분기의 대체 경로. if와 함께 사용되며 조건이 false일 때 실행됩니다." },
        { "csharp:for", "계수 반복문. 초기화, 조건 판정, 반복의 세 부분으로 구성됩니다." },
        { "csharp:while", "조건 반복문. 조건이 true인 동안 코드 블록을 반복합니다." },
        { "csharp:do", "후판정 반복문. 먼저 코드 블록을 한 번 실행한 후 조건을 판정합니다." },
        { "csharp:switch", "다중 분기문. 식의 값을 다른 case 분기와 매칭합니다." },
        { "csharp:case", "switch 문 내의 분기 레이블. 일치하는 값에 해당하는 코드 블록을 실행합니다." },
        { "csharp:break", "탈출문. 가장 가까운 반복문 또는 switch 문을 즉시 종료합니다." },
        { "csharp:continue", "계속문. 현재 반복의 나머지 부분을 건너뛰고 다음 반복으로 진행합니다." },
        { "csharp:return", "반환문. 현재 메서드를 종료하고 선택적으로 값을 반환합니다." },
        { "csharp:goto", "점프문. 지정된 레이블로 무조건 점프합니다." },
        { "csharp:foreach", "컬렉션 반복문. 컬렉션의 각 요소를 순서대로 액세스합니다." },
        { "csharp:class", "참조 형식 선언. 데이터(필드, 속성)와 동작(메서드)을 포함하는 구조를 정의합니다." },
        { "csharp:interface", "인터페이스 선언. 클래스 또는 구조체가 구현해야 하는 계약을 정의합니다." },
        { "csharp:struct", "값 형식 선언. 스택에 할당되는 경량 데이터 구조." },
        { "csharp:enum", "열거형 선언. 이름 있는 정수 상수 집합을 정의합니다." },
        { "csharp:namespace", "네임스페이스 선언. 코드를 구성하고 이름 충돌을 방지하기 위한 논리적 컨테이너." },
        { "csharp:record", "레코드 형식 선언. 값 의미를 갖는 참조 형식으로, 불변 데이터에 적합합니다." },
        { "csharp:delegate", "델리게이트 선언. 이벤트 및 콜백에 사용되는 형식 안전한 메서드 참조." },
        { "csharp:public", "public 접근 수식어. 멤버는 모든 코드에서 액세스 가능합니다." },
        { "csharp:private", "private 접근 수식어. 멤버는 포함 형식 내부에서만 액세스 가능합니다." },
        { "csharp:protected", "protected 접근 수식어. 멤버는 포함 형식 및 파생 형식에서 액세스 가능합니다." },
        { "csharp:internal", "internal 접근 수식어. 멤버는 동일한 어셈블리 내에서만 액세스 가능합니다." },
        { "csharp:sealed", "sealed 수식어. 클래스가 상속되거나 메서드가 재정의되는 것을 방지합니다." },
        { "csharp:int", "32비트 부호 있는 정수 형식(System.Int32의 별칭)." },
        { "csharp:string", "문자열 형식(System.String의 별칭). Unicode 문자의 불변 시퀀스를 나타냅니다." },
        { "csharp:bool", "불리언 형식(System.Boolean의 별칭). 값은 true 또는 false." },
        { "csharp:float", "32비트 단정밀도 부동소수점 형식(System.Single의 별칭)." },
        { "csharp:double", "64비트 배정밀도 부동소수점 형식(System.Double의 별칭)." },
        { "csharp:decimal", "128비트 고정밀도 10진수 형식. 금융 계산에 적합합니다." },
        { "csharp:char", "16비트 Unicode 문자 형식(System.Char의 별칭)." },
        { "csharp:byte", "8비트 부호 없는 정수 형식(System.Byte의 별칭)." },
        { "csharp:object", "모든 형식의 기본 형식(System.Object의 별칭)." },
        { "csharp:var", "암시적 형식 지역 변수. 컴파일러가 초기화 식에서 형식을 추론합니다." },
        { "csharp:dynamic", "동적 형식. 컴파일 타임 형식 검사를 건너뛰고 런타임에 해결됩니다." },
        { "csharp:void", "반환 없음 형식. 메서드가 값을 반환하지 않음을 나타냅니다." },
        { "csharp:static", "static 수식어. 특정 인스턴스가 아닌 형식 자체에 속합니다." },
        { "csharp:abstract", "abstract 수식어. 불완전한 구현을 나타내며 파생 클래스에서 완성해야 합니다." },
        { "csharp:virtual", "virtual 수식어. 메서드 또는 속성을 파생 클래스에서 재정의할 수 있습니다." },
        { "csharp:override", "override 수식어. 기본 클래스의 가상 메서드 또는 추상 메서드에 대한 새 구현을 제공합니다." },
        { "csharp:const", "const 수식어. 컴파일 타임에 결정되는 불변 값." },
        { "csharp:readonly", "readonly 수식어. 선언 시 또는 생성자에서만 값을 할당할 수 있습니다." },
        { "csharp:volatile", "volatile 수식어. 필드가 여러 스레드에 의해 동시에 수정될 수 있음을 나타냅니다." },
        { "csharp:async", "async 수식어. 메서드가 비동기 작업을 포함함을 나타내며, 일반적으로 await와 함께 사용됩니다." },
        { "csharp:await", "await 연산자. 비동기 작업이 완료될 때까지 메서드 실행을 일시 중단합니다." },
        { "csharp:partial", "partial 수식어. 클래스, 구조체 또는 인터페이스를 여러 파일로 분할할 수 있습니다." },
        { "csharp:ref", "ref 매개변수. 매개변수를 참조로 전달합니다." },
        { "csharp:out", "out 매개변수. 메서드에서 여러 값을 반환하는 데 사용됩니다." },
        { "csharp:in", "읽기 전용 참조 매개변수. 참조로 전달하지만 수정은 허용되지 않습니다." },
        { "csharp:params", "params 수식어. 동일한 형식의 가변 개수 인수를 전달할 수 있습니다." },
        { "csharp:try", "예외 처리 블록. 예외를 발생시킬 수 있는 코드를 포함합니다." },
        { "csharp:catch", "예외 캐치 블록. try 블록에서 발생한 예외를 처리합니다." },
        { "csharp:finally", "finally 블록. 예외 발생 여부와 관계없이 실행되는 코드." },
        { "csharp:throw", "예외 발생문. 예외 객체를 수동으로 발생시킵니다." },
        { "csharp:new", "인스턴스 생성 연산자. 객체를 생성하거나 생성자를 호출합니다." },
        { "csharp:this", "현재 인스턴스 참조. 현재 클래스 인스턴스를 참조합니다." },
        { "csharp:base", "기본 클래스 참조. 직계 기본 클래스의 멤버를 참조합니다." },
        { "csharp:using", "지시문 또는 문. 네임스페이스를 가져오거나 IDisposable 리소스가 해제되도록 보장합니다." },
        { "csharp:yield", "이터레이터 키워드. 값을 하나씩 반환하여 지연 실행을 가능하게 합니다." },
        { "csharp:lock", "lock 문. 한 번에 하나의 스레드만 코드 블록을 실행하도록 보장합니다." },
        { "csharp:typeof", "typeof 연산자. 형식의 System.Type 객체를 가져옵니다." },
        { "csharp:nameof", "nameof 연산자. 변수, 형식 또는 멤버의 문자열 이름을 가져옵니다." },
        { "csharp:is", "형식 검사 연산자. 객체가 지정된 형식과 호환되는지 검사합니다." },
        { "csharp:as", "형식 변환 연산자. 안전하게 형식 변환을 시도하며 실패 시 null을 반환합니다." },
        { "csharp:null", "null 리터럴. 참조 형식 또는 nullable 형식의 null 참조를 나타냅니다." },
        { "csharp:true", "불리언 참값." },
        { "csharp:false", "불리언 거짓값." },
        { "csharp:default", "기본값 식. 형식의 기본값을 가져옵니다(참조 형식은 null, 숫자 형식은 0)." },
        { "csharp:operator", "연산자 선언. 사용자 지정 형식에서 연산자 동작을 정의합니다." },
        { "csharp:explicit", "명시적 변환 선언. 명시적 캐스팅이 필요한 변환 연산자." },
        { "csharp:implicit", "암시적 변환 선언. 자동으로 수행할 수 있는 변환 연산자." },
        { "csharp:unchecked", "unchecked 블록. 정수 산술 오버플로 검사를 비활성화합니다." },
        { "csharp:checked", "checked 블록. 정수 산술 오버플로 검사를 활성화합니다." },
        { "csharp:fixed", "fixed 문. 가비지 컬렉션에 의한 이동을 방지하기 위해 메모리 위치를 고정합니다." },
        { "csharp:stackalloc", "스택 할당 연산자. 스택에 메모리 블록을 할당합니다." },
        { "csharp:extern", "extern 수식어. 메서드가 외부 어셈블리(DLL 등)에서 구현되었음을 나타냅니다." },
        { "csharp:unsafe", "unsafe 코드 블록. 포인터 등의 unsafe 기능 사용을 허용합니다." },
        // 플랫폼 핵심 형식
        { "csharp:ipermissioncallback", "권한 콜백 인터페이스. 실리콘 생명체의 각종 작업 권한(네트워크, 명령줄, 파일 액세스 등)을 평가하는 데 사용됩니다." },
        { "csharp:permissionresult", "권限 결과 열거형. 권한 평가 결과를 나타냅니다: Allowed(허용), Denied(거부), AskUser(사용자에게 문의)." },
        { "csharp:permissiontype", "권한 유형 열거형. 권한의 종류를 정의합니다: NetworkAccess(네트워크 액세스), CommandLine(명령줄 실행), FileAccess(파일 액세스), Function(함수 호출), DataAccess(데이터 액세스)." },
        // System.Net
        { "csharp:ipaddress", "IP 주소 클래스(System.Net.IPAddress). Internet Protocol (IP) 주소를 나타냅니다." },
        { "csharp:addressfamily", "주소 패밀리 열거형(System.Net.Sockets.AddressFamily). 네트워크 주소의 주소 지정 방식을 지정합니다(InterNetwork(IPv4), InterNetworkV6(IPv6) 등)." },
        // System
        { "csharp:uri", "통합 리소스 식별자 클래스(System.Uri). 웹 리소스에 액세스하기 위한 URI(Uniform Resource Identifier)의 개체 표현을 제공합니다." },
        { "csharp:operatingsystem", "운영 체제 클래스(System.OperatingSystem). 현재 운영 체제를 확인하기 위한 정적 메서드(IsWindows(), IsLinux(), IsMacOS() 등)를 제공합니다." },
        { "csharp:environment", "환경 클래스(System.Environment). 현재 환경 및 플랫폼에 대한 정보와 이를 조작하는 방법을 제공합니다." },
        // System.IO
        { "csharp:path", "경로 클래스(System.IO.Path). 파일 또는 디렉터리 경로 정보를 포함하는 String 인스턴스에서 작업을 수행합니다." },
        // System.Collections.Generic
        { "csharp:hashset", "해시 집합 클래스(System.Collections.Generic.HashSet<T>). 값 집합을 나타내며 고성능 집합 연산을 제공합니다." },
        // System.Text
        { "csharp:stringbuilder", "문자열 빌더 클래스(System.Text.StringBuilder). 변경 가능한 문자열을 나타내며 문자열을 자주 수정하는 시나리오에 적합합니다." },
    };

    // 전체 네임스페이스 번역 사전
    private static readonly Dictionary<string, string> TranslationDictionary = new(CSharpKeywords)
    {
        // 전체 네임스페이스 키 추가
        { "csharp:System.Net.IPAddress", "IP 주소 클래스(System.Net.IPAddress). Internet Protocol (IP) 주소를 나타냅니다." },
        { "csharp:System.Net.Sockets.AddressFamily", "주소 패밀리 열거(System.Net.Sockets.AddressFamily). InterNetwork(IPv4) 또는 InterNetworkV6(IPv6)과 같은 네트워크 주소의 주소 지정スキーム을 지정합니다." },
        { "csharp:System.Uri", "통합 리소스 식별자 클래스(System.Uri). 웹 리소스 액세스를 위한 URI 의 개체 표현을 제공합니다." },
        { "csharp:System.OperatingSystem", "운영 체제 클래스(System.OperatingSystem). IsWindows(), IsLinux(), IsMacOS() 등 현재 운영 체제를 확인하는 정적 메서드를 제공합니다." },
        { "csharp:System.Environment", "환경 클래스(System.Environment). 현재 환경 및 플랫폼에 대한 정보와 이를 조작하는 수단을 제공합니다." },
        { "csharp:System.IO.Path", "경로 클래스(System.IO.Path). 파일 또는 디렉터리 경로 정보를 포함하는 String 인스턴스에서 작업을 수행합니다." },
        { "csharp:System.Collections.Generic.HashSet", "해시 집합 클래스(System.Collections.Generic.HashSet<T>). 값 집합을 나타내며 고성능 집합 연산을 제공합니다." },
        { "csharp:System.Text.StringBuilder", "문자열 빌더 클래스(System.Text.StringBuilder). 변경 가능한 문자열을 나타내며 문자열을 자주 수정하는 시나리오에 적합합니다." },
    };
}
