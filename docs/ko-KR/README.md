# Silicon Life Collective

**⚠️ 경고: 동적 컴파일은 작동하지만 정상적으로 기능하려면 코드 템플릿이 필요합니다. 철저한 테스트가 진행 중입니다.**

AI 에이전트인 **실리콘 비잉(Silicon Being)**이 Roslyn 동적 컴파일을 통해 자기 진화하는 .NET 9 멀티 에이전트 협업 플랫폼입니다.

[English](README.md) | [中文文档](docs/zh-CN/README.md) | [繁體中文](docs/zh-HK/README.md) | [日本語](docs/ja-JP/README.md) | [한국어](docs/ko-KR/README.md)

## 기능

- **멀티 에이전트 오케스트레이션** — *실리콘 큐레이터(Silicon Curator)*가 관리하며 틱 기반 시분할 공정 스케줄링 (MainLoop + TickObject + Watchdog + Circuit Breaker)
- **소울 파일 기반** — 각 Silicon Being은 성격과 행동을 정의하는 핵심 프롬프트 파일(`soul.md`)에 의해 구동됩니다
- **Body-Brain 아키텍처** — *Body* (SiliconBeing)는 생존 상태를 유지하고 트리거를 감지; *Brain* (ContextManager)은 기록을 로드하고, AI를 호출하며, 도구를 실행하고, 응답을 저장합니다
- **Tool Call 루프** — AI가 tool_calls 반환 → 도구 실행 → 결과 피드백 → AI 계속 → 일반 텍스트 응답까지
- **Executor-Permission 보안** — 모든 디스크, 네트워크, 명령줄 작업이 권한 검증과 함께 executor를 통과합니다
  - 5단계 권한 체인: IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - 모든 권한 결정에 대한 감사 로깅
- **토큰 사용량 감사** — `ITokenUsageAudit` / `TokenUsageAuditManager`를 통한 내장 토큰 사용량 추적 및 보고
- **다중 AI 백엔드** — Ollama(로컬) 및 알리바바 클라우드 DashScope(클라우드) 지원
  - **Ollama** — 기본 HTTP API를 사용한 로컬 모델 호스팅
  - **DashScope(百炼)** — OpenAI 호환 API, 다중 리전 배포, 13개 이상의 모델(Qwen, DeepSeek, GLM, Kimi, Llama)을 갖춘 클라우드 AI 서비스
- **32개 캘린더 시스템** — 그레고리력, 중국 음력, 이슬람력, 히브리력, 일본력, 페르시아력, 마야력 등 다중 캘린더 지원
- **최소 의존성** — Core 라이브러리는 Roslyn 동적 컴파일을 위해 Microsoft.CodeAnalysis.CSharp에만 의존
- **데이터베이스 의존성 제로** — `ITimeStorage`를 통한 시간 인덱스 쿼리가 가능한 파일 기반 저장소 (JSON)
- **현지화** — 18개 언어 변형을 포함한 포괄적인 다국어 지원
  - 중국어: zh-CN, zh-HK, zh-SG, zh-MO, zh-TW, zhMY (6개 변형)
  - 영어: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10개 변형)
  - 일본어: ja-JP
  - 한국어: ko-KR
- **Web UI** — SSE 지원, 다중 스킨, 포괄적인 대시보드가 포함된 내장 HTTP 서버
  - **스킨 시스템** — 플러그형 ISkin 인터페이스와 자동 검색 기능을 갖춘 4개 내장 스킨 (Admin, Chat, Creative, Dev)
  - **18개 컨트롤러** — About, Audit, Being, Chat, CodeBrowser, CodeHover, Config, Dashboard, Executor, Init, Knowledge, Log, Memory, Permission, PermissionRequest, Project, Task, Timer
  - **실시간 업데이트** — 채팅 메시지, Being 상태, 시스템 이벤트를 위한 SSE (Server-Sent Events)
  - **HTML/CSS/JS 빌더** — `H`, `CssBuilder`, `JsBuilder`를 통한 서버 측 마크업 생성 (프론트엔드 프레임워크 의존성 제로)
  - **현지화** — LocalizationManager 해석을 갖춘 18개 내장 언어 변형

## 기술 스택

| 구성 요소 | 기술 |
|-----------|------|
| 런타임 | .NET 9 |
| 언어 | C# |
| AI 통합 | Ollama (로컬), 알리바바 클라우드 DashScope (클라우드) |
| 저장소 | 파일 시스템 (JSON + 시간 인덱스 디렉토리) |
| 웹 서버 | HttpListener (내장 .NET) |
| 동적 컴파일 | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| 라이선스 | Apache-2.0 |

## 프로젝트 구조

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Core 라이브러리 (인터페이스, 추상화)
│   │   ├── ServiceLocator.cs             # 전역 서비스 로케이터: Register/Get, ChatSystem, IMManager, AuditLogger, GlobalACL, BeingFactory, BeingManager, DynamicBeingLoader, TokenUsageAudit
│   │   ├── Runtime/                       # MainLoop, TickObject, CoreHost, CoreHostBuilder, PerformanceMonitor
│   │   ├── SiliconBeing/                  # SiliconBeingBase, SiliconBeingManager, SiliconCurator, ISiliconBeingFactory, SoulFileManager, Memory, TaskSystem, TimerSystem
│   │   ├── AI/                            # IAIClient, IAIClientFactory, ContextManager ("brain"), Message, AIRequest/AIResponse
│   │   ├── Audit/                         # ITokenUsageAudit, TokenUsageAuditManager, TokenUsageRecord, TokenUsageSummary, TokenUsageQuery
│   │   ├── Chat/                          # ChatSystem, IChatService, SimpleChatService, SessionBase, SingleChatSession, GroupChatSession, BroadcastChannel, ChatMessage
│   │   ├── Executors/                     # ExecutorBase, DiskExecutor, NetworkExecutor, CommandLineExecutor, ExecutorRequest, ExecutorResult
│   │   ├── Tools/                         # ITool, ToolManager (리플렉션 스캔), ToolCall/ToolResult, ToolDefinition, SiliconManagerOnlyAttribute
│   │   ├── Security/                      # PermissionManager, GlobalACL, AuditLogger, UserFrequencyCache, PermissionResult, PermissionType, IPermissionCallback, IPermissionAskHandler
│   │   ├── IM/                            # IIMProvider, IMManager (메시지 라우팅)
│   │   ├── Storage/                       # IStorage, ITimeStorage (키-값 + 시간 인덱스)
│   │   ├── Config/                        # ConfigDataBase, Config (싱글톤 + JSON), ConfigDataBaseConverter, GuidConverter, AIClientConfigAttribute, ConfigGroupAttribute, ConfigIgnoreAttribute, DirectoryInfoConverter
│   │   ├── Localization/                  # LocalizationBase, LocalizationManager, Language 열거형
│   │   ├── Logging/                       # ILogger, ILoggerProvider, LogEntry, LogLevel, LogManager
│   │   ├── Compilation/                   # DynamicBeingLoader, DynamicCompilationExecutor, SecurityScanner, CodeEncryption
│   │   └── Time/                          # IncompleteDate (시간 범위 쿼리)
│   │
│   └── SiliconLife.Default/               # 기본 구현 + 진입점
│       ├── Program.cs                     # 애플리케이션 진입점 (모든 구성 요소 연결)
│       ├── AI/                            # OllamaClient, OllamaClientFactory (기본 Ollama HTTP API); DashScopeClient, DashScopeClientFactory (알리바바 클라우드 百炼)
│       ├── SiliconBeing/                  # DefaultSiliconBeing, DefaultSiliconBeingFactory
│       ├── Calendar/                      # 32개 캘린더 구현: Buddhist, Cherokee, ChineseLunar, ChulaSakarat, Coptic, Dai, DehongDai, Ethiopian, FrenchRepublican, Gregorian, Hebrew, Indian, Inuit, Islamic, Japanese, Javanese, Juche, Julian, Khmer, Mayan, Mongolian, Persian, RepublicOfChina, Roman, Saka, Sexagenary, Tibetan, Vietnamese, VikramSamvat, Yi, Zoroastrian
│       ├── Executors/                     # 기본 executor 구현
│       ├── IM/                            # WebUIProvider (Web UI를 IM 채널로 사용), IMPermissionAskHandler
│       ├── Tools/                         # 내장 도구: Calendar, Chat, Config, Curator, Disk, DynamicCompile, Memory, Network, System, Task, Timer, TokenAudit
│       ├── Config/                        # DefaultConfigData
│       ├── Localization/                  # ZhCN, ZhHK, EnUS, JaJP, KoKR, DefaultLocalizationBase, EnOther (EnGB, EnCA, EnAU, EnIN, EnSG, EnZA, EnIE, EnNZ, EnMY), ZhOther (ZhSG, ZhMO, ZhTW, ZhMY)
│       ├── Logging/                       # ConsoleLoggerProvider, FileSystemLoggerProvider
│       ├── Storage/                       # FileSystemStorage, FileSystemTimeStorage
│       ├── Security/                      # DefaultPermissionCallback
│       ├── Runtime/                       # TestTickObject
│       └── Web/                           # Web UI 구현
│           ├── Controllers/               # 18개 컨트롤러: About, Audit, Being, Chat, CodeBrowser, CodeHover, Config, Dashboard, Executor, Init, Knowledge, Log, Memory, Permission, PermissionRequest, Project, Task, Timer
│           ├── Models/                    # ViewModel: AboutViewModel, AuditViewModel, BeingViewModel, ChatMessage, ChatViewModel, CodeBrowserViewModel, ConfigViewModel, DashboardViewModel, ExecutorViewModel, KnowledgeViewModel, LogViewModel, MemoryViewModel, PermissionViewModel, PermissionRequestViewModel, ProjectViewModel, TaskViewModel, TimerViewModel, ViewModelBase
│           ├── Views/                     # 19개 HTML 뷰: ViewBase, AboutView, AuditView, BeingView, ChatView, CodeBrowserView, CodeEditorView, ConfigView, DashboardView, ExecutorView, KnowledgeView, LogView, MarkdownEditorView, MemoryView, PermissionView, ProjectView, SoulEditorView, TaskView, TimerView
│           ├── Skins/                     # 4개 스킨: Admin (전문가용), Chat (대화형), Creative (예술적), Dev (개발자 중심)
│           ├── ISkin.cs                   # 스킨 인터페이스 + SkinPreviewInfo + SkinManager (자동 검색)
│           ├── Controller.cs              # 기본 컨트롤러 클래스
│           ├── WebHost.cs                 # HTTP 서버 (HttpListener)
│           ├── Router.cs                  # 패턴 매칭을 통한 요청 라우팅
│           ├── SSEHandler.cs              # Server-Sent Events
│           ├── WebSecurity.cs             # 웹 보안 유틸리티
│           ├── H.cs                       # 플루언트 HTML 빌더 DSL
│           ├── CssBuilder.cs              # CSS 빌더 유틸리티
│           └── JsBuilder.cs               # JavaScript 빌더 유틸리티
│
├── docs/
│   └── zh-CN/                             # 중국어 문서
```

## 아키텍처 개요

```
MainLoop (전용 스레드, watchdog + circuit breaker)
  └── TickObject (우선순위 정렬)
       └── SiliconBeingManager
            └── SiliconBeingRunner (틱당 임시 스레드, 타임아웃 + circuit breaker)
                 └── DefaultSiliconBeing.Tick()
                      └── ContextManager.ThinkOnChat()
                           └── IAIClient.Chat() -> Tool Call 루프 -> ChatSystem에 저장
```

모든 AI 시작 I/O 작업은 보안 체인을 통과합니다:

```
Tool Call -> Executor -> PermissionManager -> [IsCurator -> FrequencyCache -> GlobalACL -> Callback -> AskUser]
```

## 시작하기

### 필수 조건

- .NET 9 SDK
- AI 백엔드 (하나 선택):
  - **Ollama**: [Ollama](https://ollama.com) 로컬에서 실행 및 모델 풀 (예: `ollama pull llama3`)
  - **알리바바 클라우드 DashScope**: [Bailian Console](https://bailian.console.aliyun.com/)에서 유효한 API 키

### 빌드

```bash
dotnet restore
dotnet build
```

### 실행

```bash
dotnet run --project src/SiliconLife.Default
```

애플리케이션이 웹 서버를 시작하고 브라우저에서 Web UI를 자동으로 엽니다.

### 게시 (단일 파일)

```bash
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

## 로드맵

- [x] 1단계: 콘솔 AI 채팅
- [x] 2단계: 프레임워크 스켈레톤 (MainLoop + TickObject + Watchdog + Circuit Breaker)
- [x] 3단계: 소울 파일이 있는 첫 번째 실리콘 비잉 (Body-Brain 아키텍처)
- [x] 4단계: 영구 메모리 (ChatSystem + ITimeStorage)
- [x] 5단계: 도구 시스템 + Executors
- [x] 6단계: 권한 시스템 (5단계 체인, AuditLogger, GlobalACL)
- [x] 7단계: 동적 컴파일 + 자기 진화 (Roslyn)
- [x] 8단계: 장기 메모리 + 작업 + 타이머
- [x] 9단계: CoreHost + 멀티 에이전트 협업
- [x] 10단계: Web UI (HTTP + SSE, 18개 컨트롤러, 4개 스킨)
- [x] 10.5단계: 점진적 개선 (BroadcastChannel, TokenAudit, 32개 캘린더, 도구 개선, 18개 언어 현지화)
- [ ] 11단계: 외부 IM 통합 (Feishu / WhatsApp / Telegram)
- [ ] 12단계: 지식 그래프, 플러그인 시스템, 스킬 생태계

## 문서

- [아키텍처](architecture.md) — 시스템 설계, 스케줄링, 구성 요소 아키텍처
- [보안](security.md) — 권한 모델, executors, 동적 컴파일 보안
- [로드맵](roadmap.md) — 상세한 12단계 개발 계획

## 라이선스

이 프로젝트는 Apache License 2.0에 따라 라이선스가 부여됩니다 — 자세한 내용은 [LICENSE](LICENSE) 파일을 참조하십시오.

## 저자

Hoshino Kennji — [YouTube](https://www.youtube.com/@hoshinokennji) | [Bilibili](https://space.bilibili.com/617827040)
