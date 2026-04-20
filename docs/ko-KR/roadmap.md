# 로드맵

[English](roadmap.md) | [中文](docs/zh-CN/roadmap.md) | [繁體中文](docs/zh-HK/roadmap.md) | [日本語](docs/ja-JP/roadmap.md) | [한국어](docs/ko-KR/roadmap.md)

## 지도 원칙

모든 단계는 **실행 가능하고 관찰 가능한** 시스템으로 끝납니다. 어떤 단계도 "보여줄 것 없는 인프라 더미"를 생성하지 않습니다.

---

## ~~1단계: 채팅 가능~~ ✅ 완료

**목표**: 콘솔 입력 → AI 호출 → 콘솔 출력. 최소 검증 단위.

| # | 모듈 | 설명 |
|---|------|------|
| 1.1 | 솔루션 및 프로젝트 구조 | `SiliconLifeCollective.sln` 생성, `src/SiliconLife.Core/` (핵심 라이브러리) 및 `src/SiliconLife.Default/` (기본 구현 + 진입점) |
| 1.2 | 설정 (최소) | 싱글톤 + JSON 역직렬화. `config.json` 읽기. 누락 시 기본값 자동 생성 |
| 1.3 | 현지화 (최소) | `LocalizationBase` 추상 클래스, `ZhCN` 구현. 설정에 `Language` 추가 |
| 1.4 | OllamaClient (최소) | `IAIClient` 인터페이스, 로컬 Ollama `/api/chat` HTTP 호출. 스트리밍 없음, ToolCall 아직 없음 |
| 1.5 | 콘솔 I/O | `while(true) + Console.ReadLine()`, 입력 읽기 → AI 호출 → 응답 출력 |
| 1.6 | 저작권 헤더 | 모든 C# 소스 파일에 Apache 2.0 헤더 추가 |

**결과물**: 로컬 Ollama 모델과 대화하는 콘솔 채팅 프로그램.

**검증**: 프로그램 실행, "hello" 입력, AI 응답 확인.

---

## ~~2단계: 스켈레톤 있음~~ ✅ 완료

**목표**: "벌거벗은 루프"를 프레임워크 구조로 교체. 행동 동일.

| # | 모듈 | 설명 |
|---|------|------|
| 2.1 | 저장소 (최소) | `IStorage` 인터페이스 (Read/Write/Exists/Delete, 키-값). `FileSystemStorage` 구현. 인스턴스 클래스 (정적 아님). 직접 파일 시스템 접근 — **AI가 IStorage 제어 불가** |
| 2.2 | MainLoop + TickObject | 정확한 틱 간격 무한 루프 (`Stopwatch` + `Thread.Sleep`). 우선순위 스케줄링 |
| 2.3 | IAIClient 표준화 | `IAIClientFactory` 인터페이스. OllamaClient가 표준 인터페이스 구현하도록 리팩토링 |
| 2.4 | 콘솔 마이그레이션 | `while(true)`를 MainLoop 기반 TickObject로 마이그레이션. 1단계와 동일한 동작 |

**결과물**: MainLoop가 틱 실행, 콘솔 채팅 여전히 작동.

**검증**: 매초 틱 수를 출력하는 테스트 TickObject 등록; 콘솔 채팅 여전히 작동.

---

## ~~3단계: 소울 있음~~ ✅ 완료

**목표**: 첫 번째 실리콘 비잉이 프레임워크 내에서 살아있음.

| # | 모듈 | 설명 |
|---|------|------|
| 3.1 | SiliconBeingBase | Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService를 가진 추상 기본 클래스. 추상 `Tick()` 및 `ExecuteOneRound()` |
| 3.2 | 소울 파일 로딩 | `SoulFileManager`: being 데이터 디렉토리에서 `soul.md` 읽기 |
| 3.3 | ContextManager (최소) | 소울 파일 + 최근 메시지 연결 → AI 호출 → 응답 획득. 아직 ToolCall 없음, 저장 없음 |
| 3.4 | ISiliconBeingFactory | being 인스턴스 생성을 위한 팩토리 인터페이스 |
| 3.5 | SiliconBeingManager (최소) | TickObject 상속 (Priority=0). 모든 being 반복, 순차적으로 Tick 호출 |
| 3.6 | DefaultSiliconBeing | 표준 동작 구현. 읽지 않은 메시지 확인 → ContextManager 생성 → ExecuteOneRound → 출력 |
| 3.7 | Being 디렉토리 구조 | `DataDirectory/SiliconManager/{GUID}/`, `soul.md` 및 `state.json` 포함 |

**결과물**: MainLoop에 의해 구동되는 실리콘 비잉, 콘솔 입력 수신, 소울 파일 로딩, AI 호출.

**검증**: 콘솔 입력 → MainLoop Tick → Being 처리 (소울 파일 유도 동작) → AI 응답. 응답 스타일이 1단계와 달라야 함.

---

## ~~4단계: 메모리 있음~~ ✅ 완료

**목표**: 대화가 재시작 Across 에서도 지속.

| # | 모듈 | 설명 |
|---|------|------|
| 4.1 | ChatSystem | 채널 개념 (두 GUID = 하나의 채널). 지속성을 갖춘 메시지 모델. 아직 그룹 채팅 없음 |
| 4.2 | IIMProvider + IMManager | `IIMProvider` 인터페이스. `ConsoleProvider`를 공식 IM 채널로. `IMManager`가 메시지 라우팅 |
| 4.3 | ContextManager 향상 | ChatSystem에서 기록 가져오기. AI 응답 저장. 다중 라운드 ToolCall 계속 지원 |
| 4.4 | IMessage 모델 | ChatSystem과 IMManager가 공유하는 통합 메시지 모델 |

**결과물**: 영구 메모리를 갖춘 채팅 시스템.

**검증**: 몇 차례 채팅 → 종료 → 재시작 → "무슨 얘기 했지?" 질문 → being이 답변 가능.

---

## ~~5단계: 행동 가능 (도구 시스템)~~ ✅ 완료

**목표**: 실리콘 비잉이 작업 실행 가능, 대화뿐 아님.

| # | 모듈 | 설명 |
|---|------|------|
| 5.1 | ITool + ToolResult | Name, Description, Execute를 갖춘 `ITool` 인터페이스. Success, Message, Data를 갖춘 `ToolResult` |
| 5.2 | ToolManager | being별 인스턴스. 리플렉션 기반 도구 검색. `[SiliconManagerOnly]` 속성 지원 |
| 5.3 | IAIClient: ToolCall 지원 | AI tool_calls 파싱. 루프: 도구 실행 → 결과 다시 전송 → AI 계속 → 일반 텍스트까지 |
| 5.4 | Executor 기본 클래스 | 독립 스케줄링 스레드, 요청 큐, 타임아웃 제어를 갖춘 추상 기본 클래스 |
| 5.5 | NetworkExecutor | executor를 통한 HTTP 요청. 타임아웃, 큐잉 |
| 5.6 | CommandLineExecutor | executor를 통한 셸 실행. 크로스플랫폼 구분자 감지 |
| 5.7 | DiskExecutor | executor를 통한 파일 작업. 아직 권한 검사 없음 (6단계) |
| 5.8–5.12 | 내장 도구 | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**결과물**: 실리콘 비잉이 도구 호출하여 작업 수행 가능.

**검증**: "오늘 며칠이야" 질문 → CalendarTool 답변; "프로세스 확인해줘" 요청 → SystemTool 실행; being에게 다른 being에게 메시지 보내라 지시 → ChatTool 작동.

---

## ~~6단계: 규칙 따름 (권한 시스템)~~ ✅ 완료

**목표**: 실리콘 비잉이 승인 없이 민감한 리소스 접근 불가.

| # | 모듈 | 설명 |
|---|------|------|
| 6.1 | PermissionManager | being별 전용 인스턴스. 콜백 기반, 3진 결과 (Allowed/Deny/AskUser). 쿼리 우선순위: HighDeny → HighAllow → Callback. IsCurator 플래그 |
| 6.2 | PermissionType 열거형 | NetworkAccess, CommandLine, FileAccess, Function, DataAccess |
| 6.3 | DefaultPermissionCallback | 네트워크 화이트리스트/블랙리스트, CLI 분류, 파일 경로 안전 규칙 |
| 6.4 | GlobalACL | 접두사 일치 규칙 테이블, Storage에 저장 |
| 6.5 | UserFrequencyCache | HighAllow/HighDeny 목록. 사용자 선택 (자동 감지 아님). 접두사 일치, 메모리 전용, 구성 가능한 만료 |
| 6.6 | UserAskMechanism (콘솔) | AskUser 반환 시 콘솔 프롬프트로 y/n |
| 6.7 | Executor 권한 통합 | 모든 executor가 실행 전 권한 확인 |
| 6.8 | IStorage 격리 참고 | IStorage는 시스템 내부 지속성 — 직접 파일 접근, **executor 통과 안 함**, **AI 제어 불가**. executor는 AI 도구에서 시작한 IO만 관리 |
| 6.9 | 감사 로깅 | 타임스탬프, 요청자, 리소스, 결과 포함 모든 권한 결정 로깅 |

**결과물**: being이 민감한 작업 시도 시 권한 프롬프트 표시.

**검증**: being에게 파일 삭제 지시 → 콘솔에 권한 프롬프트 표시 → `n` 입력 → 작업 거부. being에게 화이트리스트 사이트 접근 지시 → 즉시 허용.

---

## ~~7단계: 진화 가능 (동적 컴파일)~~ ✅ 완료

**목표**: 실리콘 비잉이 자체 코드 재작성 가능.

| # | 모듈 | 설명 |
|---|------|------|
| 7.1 | CodeEncryption | AES-256 암호화/복호화. GUID에서 PBKDF2 키 파생 |
| 7.2 | DynamicCompilationExecutor | Roslyn 기반 메모리 컴파일 샌드박스. 컴파일 타임 어셈블리 참조 제어 (주요 방어: System.IO, Reflection 등 제외) |
| 7.3 | 보안 스캔 | 위험한 코드 패턴 런타임 정적 분석 (보조 방어). 스캔 실패 시 로딩 차단 |
| 7.4 | Being 수명 주기 향상 | 로딩: 복호화 → 스캔 → 컴파일 → 인스턴스화. 런타임: 메모리에서 컴파일 → 원자적 교체 → 암호화 저장 |
| 7.5 | SiliconCurator | Curator 추상 기본 클래스. IsCurator=true. 최고 권한 |
| 7.6 | DefaultCurator | 내장 소울 파일 및 관리 도구를 갖춘 기본 curator 구현 |
| 7.7 | CuratorTool | `[SiliconManagerOnly]` 도구: list_beings, create_being, get_code, reset |
| 7.8 | 권한 콜백 재정의 | being이 사용자 정의 권한 콜백 컴파일 가능 |
| 7.9 | SiliconBeingManager 향상 | Replace 메서드 (런타임 인스턴스 교체). MigrateState (이전 인스턴스에서 새 인스턴스로 상태 이전) |

**결과물**: 실리콘 비잉이 AI를 통해 새 코드 생성, 컴파일, 자체 교체 가능.

**검증**: being에게 "새 기능 추가해줘" 지시 → 컴파일 관찰 → 재시작 → 새 기능 작동.

---

## ~~8단계: 기억하고 계획~~ ✅ 완료

**목표**: 장기 메모리, 작업 관리, 예약 트리거.

| # | 모듈 | 설명 |
|---|------|------|
| 8.1 | FileSystemMemory | 단기/장기 단계별 저장. 시간 감쇠. 압축 (유사 메모리 병합). 다차원 검색 |
| 8.2 | TaskSystem | 일회성 + DAG 의존성 작업. 우선순위 스케줄링. 상태 추적 |
| 8.3 | TimerSystem | 일회성 알람 + 주기적 타이머. 밀리초 정확도. Storage에 저장 |
| 8.4 | IncompleteDate | 퍼지 날짜 범위 구조체 (예: "2026년 4월", "2026년 봄") |
| 8.5–8.7 | 메모리/작업/타이머 도구 | being이 메모리 쿼리, 작업 관리, 타이머 설정을 위한 도구 |

**결과물**: being이 주요 포인트 기억, 작업 생성/추적, 알람 설정 가능.

**검증**: 작업 생성 → 작업 목록 확인 → 1분 알람 설정 → 시간 도달 시 알림 수신.

---

## ~~9단계: 프레임워크 완료~~ ✅ 완료

**목표**: 통합 진입점, 멀티 being 협업.

| # | 모듈 | 설명 |
|---|------|------|
| 9.1 | CoreHost + CoreHostBuilder | Builder 패턴 통합 호스트. 우아한 종료 (Ctrl+C / SIGTERM) |
| 9.2 | Program.Main 리팩토링 | CoreHostBuilder 패턴으로 마이그레이션 |
| 9.3 | SiliconBeingManager 향상 | Curator 우선순위 응답. 예외 격리. 주기적 저장 |
| 9.4 | 멀티 being 로딩 | 데이터 디렉토리에서 여러 being 로딩. ChatTool을 통한 being 간 통신 |
| 9.5 | 성능 모니터링 | TickObject별 실행 시간 추적 |
| 9.6 | ServiceLocator | Register/Get 메서드를 갖춘 전역 서비스 로케이터 |

**결과물**: 여러 being이 동시에 실행, 협업, CoreHost에 의해 관리.

**검증**: 두 being 생성 → A가 B에게 메시지 전송 → B 수신 및 회신 → 프레임워크가 오류 없이 스케줄링. 사용자 메시지 도착 시 Curator가 우선순위 응답.

---

## ~~10단계: 웹으로~~ ✅ 완료

**목표**: 콘솔에서 브라우저 인터페이스로 마이그레이션.

| # | 모듈 | 설명 |
|---|------|------|
| 10.1 | Router | HTTP 요청 라우터. 시퀀스 파라미터 라우트 및 정적 파일 제공 |
| 10.2 | Controller 기본 | 요청/응답 컨텍스트. HTML 및 JSON 응답 지원 |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | C# 서버 측 빌더. 프론트엔드 프레임워크 의존성 제로 |
| 10.6 | SSE (Server-Sent Events) | 채팅, being 상태, 시스템 이벤트를 위한 푸시 기반 실시간 업데이트. WebSocket보다 단순, 클라이언트 측 자동 재연결 |
| 10.7 | WebUIProvider | SSE 기반 실시간 IM 채널. 콘솔을 기본 인터페이스로 대체 |
| 10.8 | 웹 보안 | IP 블랙리스트/화이트리스트. `[WebCode]` 속성. 동적 업데이트 |
| 10.9–10.17 | 웹 컨트롤러 | Chat, Dashboard, Being, Task, Permission, PermissionRequest, Executor, Log, Config, Memory, Timer, Init, About, CodeBrowser, Knowledge, Project, Audit |

**결과물**: 브라우저에서 접근 가능한 전체 웹 UI.

**검증**: 브라우저 열기 → being과 채팅 → 대시보드 보기 → 권한 관리 → 모든 기능 작동.

---

## ~~10.5단계: 점진적 향상~~ ✅ 완료

**목표**: 개발 중 발견된 새 기능으로 기존 시스템 향상.

| # | 모듈 | 설명 |
|---|------|------|
| 10.5.1 | BroadcastChannel | 시스템 전체 공지용 새 세션 타입. 고정 채널 ID, 동적 구독, 대기 메시지 필터링 |
| 10.5.2 | ChatMessage 향상 | AI 컨텍스트를 위한 ToolCallId, ToolCallsJson, Thinking 필드; 토큰 추적을 위한 PromptTokens, CompletionTokens, TotalTokens; SystemNotification 메시지 타입 |
| 10.5.3 | TokenUsageAuditManager | 모든 being의 요청별 토큰 소비 추적. 집계 통계, 시간 시리즈 쿼리, 영구 저장 |
| 10.5.4 | TokenAuditTool | Curator가 토큰 사용량 쿼리 및 요약을 위한 `[SiliconManagerOnly]` 도구 |
| 10.5.5 | ConfigTool | Curator가 시스템 설정 읽기 및 수정을 위한 `[SiliconManagerOnly]` 도구 |
| 10.5.6 | AuditController | 추세 차트 및 데이터 내보내기를 갖춘 토큰 사용량 감사 웹 대시보드 |
| 10.5.7 | 캘린더 시스템 확장 | 세계 캘린더 시스템 커버하는 32개 캘린더 구현 (불교력, 중국 음력, 이슬람력, 히브리력, 일본력, 페르시아력, 마야력 등) |
| 10.5.8 | DiskTool 향상 | 새 작업: count_lines, read_lines, clear_file, replace_lines, replace_text, replace_text_all, list_drives |
| 10.5.9 | SystemTool 향상 | 새 작업: find_process (와일드카드 지원), resource_usage |
| 10.5.10 | CalendarTool 향상 | 새 작업: diff, list_calendars, get_components, get_now_components, convert (캘린더 간 변환) |
| 10.5.11 | DashScopeClient | OpenAI 호환 API를 갖춘 알리바바 클라우드 百炼 AI 클라이언트. 스트리밍, 도구 호출, 추론 콘텐츠 지원 |
| 10.5.12 | DashScopeClientFactory | DashScope 클라이언트 생성 팩토리. API를 통한 동적 모델 검색. 다중 리전 지원 (베이징, 버지니아, 싱가포르, 홍콩, 프랑크푸르트) |
| 10.5.13 | AI 클라이언트 설정 시스템 | being별 AI 클라이언트 설정. 동적 설정 키 옵션 (모델, 리전). 현지화된 표시 이름 |
| 10.5.14 | 현지화 확장 | DashScope 설정 옵션, 모델 이름, 리전 이름에 대한 중국어, 중국어 번체, 영어, 일본어 현지화 |

**결과물**: 향상된 도구, 관찰 가능성, 캘린더 커버리지, 멀티 AI 백엔드 지원.

**검증**: Curator가 TokenAuditTool로 토큰 사용량 쿼리 → 감사 대시보드에 추세 표시 → CalendarTool이 32개 캘린더 시스템 간 날짜 변환 → AI 백엔드를 DashScope로 전환 → 클라우드 API로 Qwen 모델과 채팅.

---

## 11단계: 외부 IM 통합

**목표**: 더 넓은 사용자 접근성을 위해 외부 메시징 플랫폼 연결.

| # | 모듈 | 설명 |
|---|------|------|
| 11.1 | FeishuProvider | 카드를 지원하는 Feishu (Lark) 봇 통합 |
| 11.2 | WhatsAppProvider | WhatsApp Business API 통합 |
| 11.3 | TelegramProvider | 인라인 키보드를 지원하는 Telegram Bot API 통합 |
| 11.4 | IMManager 향상 | 멀티 제공자 라우팅, 통합 메시지 형식, 플랫폼 간 권한 질문 처리 |

**결과물**: 사용자가 외부 IM 플랫폼을 통해 실리콘 비잉과 상호작용 가능.

---

## 12단계: 고급 기능

**목표**: 향상된 기능을 위한 선택적 고급 기능.

| # | 모듈 | 설명 |
|---|------|------|
| 12.1 | 지식 네트워크 | triple 구조 (주어-술어-목적어)를 사용한 공유 지식 그래프 |
| 12.2 | 플러그인 시스템 | 보안 검사 및 샌드박싱을 갖춘 외부 플러그인 로딩 |
| 12.3 | 스킬 생태계 | being 기능을 위한 재사용 가능한 스킬 마켓플레이스 |
