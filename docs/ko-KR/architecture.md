# 아키텍처

[English](architecture.md) | [中文](docs/zh-CN/architecture.md) | [繁體中文](docs/zh-HK/architecture.md) | [日本語](docs/ja-JP/architecture.md) | [한국어](docs/ko-KR/architecture.md)

## 핵심 개념

### 실리콘 비잉 (Silicon Being)

시스템의 각 AI 에이전트는 **실리콘 비잉**입니다 — 자체 ID, 성격 및 기능을 갖춘 자율 엔티티. 모든 실리콘 비잉은 행동 패턴을 정의하는 **소울 파일**(Markdown 프롬프트)에 의해 구동됩니다.

### 실리콘 큐레이터 (Silicon Curator)

**실리콘 큐레이터**는 최고 시스템 권한을 가진 특수 실리콘 비잉입니다. 시스템 관리자 역할을 합니다:

- 다른 실리콘 비잉 생성 및 관리
- 사용자 요청 분석 및 작업 분해
- 적절한 실리콘 비잉에 작업 분배
- 실행 품질 모니터링 및 실패 처리
- **우선순위 스케줄링**으로 사용자 메시지에 응답 (아래 참조)

### 소울 파일

각 실리콘 비잉의 데이터 디렉토리에 저장된 Markdown 파일(`soul.md`)입니다. 모든 AI 요청에 주입되는 시스템 프롬프트 역할을 하며, being의 성격, 의사결정 패턴 및 행동 제약을 정의합니다.

---

## 스케줄링: 시분할 공정 스케줄링

### MainLoop + TickObject

시스템은 전용 백그라운드 스레드에서 **틱 기반 메인 루프**를 실행합니다:

```
MainLoop (전용 스레드, watchdog + circuit breaker)
  └── TickObject A (Priority=0, Interval=100ms)
  └── TickObject B (Priority=1, Interval=500ms)
  └── SiliconBeingManager (MainLoop에서 직접 틱 처리)
        └── SiliconBeingRunner → 실리콘 비잉 1 → 틱 → ExecuteOneRound
        └── SiliconBeingRunner → 실리콘 비잉 2 → 틱 → ExecuteOneRound
        └── SiliconBeingRunner → 실리콘 비잉 3 → 틱 → ExecuteOneRound
        └── ...
```

핵심 설계 결정:

- **Silicon Being은 TickObject를 상속하지 않습니다.** 자체 `Tick()` 메서드를 가지며, MainLoop에 직접 등록되지 않고 `SiliconBeingManager`를 통해 `SiliconBeingRunner`에 의해 호출됩니다.
- **SiliconBeingManager**는 MainLoop에서 직접 틱 처리되며 모든 being의 단일 프록시 역할을 합니다.
- **SiliconBeingRunner**는 각 being의 `Tick()`을 타임아웃 및 being별 circuit breaker(연속 3회 타임아웃 → 1분 쿨다운)와 함께 임시 스레드에서 래핑합니다.
- 각 being의 실행은 틱당 AI 요청 + ToolCalls **1라운드**로 제한되어 어떤 being도 메인 루프를 독점할 수 없습니다.
- **PerformanceMonitor**는 관찰 가능성을 위해 틱 실행 시간을 추적합니다.

### Curator 우선순위 응답

사용자가 실리콘 큐레이터에게 메시지를 보낼 때:

1. 현재 being(예: Being A)이 현재 라운드를 완료합니다 — **중단 없음**.
2. 관리자가 **남은 큐를 건너뜁니다**.
3. 루프가 **Curator부터 다시 시작**되어 즉시 실행됩니다.

이렇게 하면 진행 중인 작업을 중단하지 않고 사용자 응답성을 보장합니다.

---

## 구성 요소 아키텍처

```
┌─────────────────────────────────────────────────────────┐
│                        CoreHost                         │
│  (통합 호스트 — 모든 구성 요소 조립 및 관리)              │
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
│  │              공유 서비스                           │   │
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
│  │                  Executors                       │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │  Disk    │  │ Network  │  │  CommandLine    │  │   │
│  │  │Executor  │  │Executor  │  │  Executor       │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              IM Providers                        │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Console  │  │  Web     │  │  Feishu / ...   │  │   │
│  │  │Provider  │  │Provider  │  │  Provider       │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## ServiceLocator

`ServiceLocator`는 모든 핵심 서비스에 대한 접근을 제공하는 스레드 안전 싱글톤 레지스트리입니다:

| 속성 | 타입 | 설명 |
|------|------|------|
| `ChatSystem` | `ChatSystem` | 중앙 채팅 세션 관리자 |
| `IMManager` | `IMManager` | 인스턴트 메시징 제공자 라우터 |
| `AuditLogger` | `AuditLogger` | 권한 감사 추적 |
| `GlobalAcl` | `GlobalACL` | 전역 접근 제어 목록 |
| `BeingFactory` | `ISiliconBeingFactory` | being 생성 팩토리 |
| `BeingManager` | `SiliconBeingManager` | 활성 being 수명 주기 관리자 |
| `DynamicBeingLoader` | `DynamicBeingLoader` | 동적 컴파일 로더 |
| `TokenUsageAudit` | `ITokenUsageAudit` | 토큰 사용량 추적 |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | 토큰 사용량 보고 |

또한 being GUID를 키로 사용하는 being별 `PermissionManager` 레지스트리를 유지합니다.

---

## 채팅 시스템

### 세션 타입

채팅 시스템은 `SessionBase`를 통해 세 가지 세션 타입을 지원합니다:

| 타입 | 클래스 | 설명 |
|------|--------|------|
| `SingleChat` | `SingleChatSession` | 두 참가자 간 1:1 대화 |
| `GroupChat` | `GroupChatSession` | 다중 참가자 그룹 대화 |
| `Broadcast` | `BroadcastChannel` | 고정 ID를 가진 공개 채널; being이 동적으로 구독하며 구독 시간 이후의 메시지만 수신 |

### BroadcastChannel

`BroadcastChannel`은 시스템 전체 공지용 특수 세션 타입입니다:

- **고정 채널 ID** — `SingleChatSession` 및 `GroupChatSession`과 달리 채널 ID는 잘 알려진 상수이며 멤버 GUID에서 파생되지 않습니다.
- **동적 구독** — being이 런타임에 구독/구독 취소하며 구독 시간 이후에 게시된 메시지만 수신합니다.
- **대기 메시지 필터링** — `GetPendingMessages()`는 being의 구독 시간 이후에 게시되고 아직 읽지 않은 메시지만 반환합니다.
- **ChatSystem에서 관리** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### ChatMessage

`ChatMessage` 모델에는 AI 대화 컨텍스트 및 토큰 추적을 위한 필드가 포함됩니다:

| 필드 | 타입 | 설명 |
|------|------|------|
| `Id` | `Guid` | 고유 메시지 식별자 |
| `SenderId` | `Guid` | 발신자의 고유 식별자 |
| `ChannelId` | `Guid` | 채널/대화 식별자 |
| `Content` | `string` | 메시지 내용 |
| `Timestamp` | `DateTime` | 메시지 전송 시간 |
| `Type` | `MessageType` | Text, Image, File 또는 SystemNotification |
| `ReadBy` | `List<Guid>` | 이 메시지를 읽은 참가자의 ID |
| `Role` | `MessageRole` | AI 대화 역할 (User, Assistant, Tool) |
| `ToolCallId` | `string?` | 도구 결과 메시지의 도구 호출 ID |
| `ToolCallsJson` | `string?` | 어시스턴트 메시지의 직렬화된 도구 호출 JSON |
| `Thinking` | `string?` | AI의 연쇄 사고 추론 |
| `PromptTokens` | `int?` | 프롬프트(입력)의 토큰 수 |
| `CompletionTokens` | `int?` | 완성(출력)의 토큰 수 |
| `TotalTokens` | `int?` | 사용된 총 토큰 수 (입력 + 출력) |
| `SenderName` | `string?` | 발신자 이름 (AI 메시지 신원 식별용) |

---

## 채팅 메시지 큐

`ChatMessageQueue`는 채팅 메시지의 비동기 처리를 관리하는 스레드 안전 메시지 큐 시스템입니다:

- **스레드 안전** - 락 메커니즘을 사용하여 안전한 동시 접근 보장
- **비동기 처리** - 비동기 메시지 인큐 및 디큐 지원
- **메시지 순서** - 메시지의 시간순서 유지
- **배치 작업** - 배치 메시지 가져오기 지원

## 파일 메타데이터

`FileMetadata`는 업로드된 파일의 메타데이터를 관리하는 데 사용됩니다:

- **파일 추적** - 파일 이름, 크기, 타입 등의 정보 기록
- **접근 제어** - 파일 접근 권한 제어 지원
- **보안 검증** - 업로드된 파일의 보안 검증 지원
- **수명주기 관리** - 파일 업로드부터 처리 완료까지의 전체 수명주기 관리

## 스트림 취소 관리자

`StreamCancellationManager`는 AI 응답 스트림의 취소 관리를 담당합니다:

- **스트림 취소** - 실행 중인 AI 응답 스트림 취소 지원
- **리소스 정리** - 취소된 스트림의 리소스 정리
- **동시성 제어** - 여러 스트림의 동시성 제어
- **상태 추적** - 스트림 상태 추적 및 관리

## 채팅 히스토리 보기

채팅 히스토리 보기 기능은 실리콘 생명체의 전체 채팅 히스토리 탐색을 지원합니다:

- **컨트롤러**: `ChatHistoryController`
- **ViewModel**: `ChatHistoryViewModel`
- **API 라우트**:
  - 대화 목록: `/api/chat-history/{beingId}/conversations`
  - 메시지 상세: `/api/chat-history/{beingId}/conversation/{conversationId}`
- **뷰**:
  - `ChatHistoryListView` - 대화 목록 표시
  - `ChatHistoryDetailView` - 메시지 상세 보기

---

## AI 클라이언트 시스템

시스템은 `IAIClient` 인터페이스를 통해 여러 AI 백엔드를 지원합니다:

### OllamaClient

- **타입**: 로컬 AI 서비스
- **프로토콜**: 기본 Ollama HTTP API (`/api/chat`, `/api/generate`)
- **기능**: 스트리밍, 도구 호출, 로컬 모델 호스팅
- **설정**: `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (알리바바 클라우드 百炼)

- **타입**: 클라우드 AI 서비스
- **프로토콜**: OpenAI 호환 API (`/compatible-mode/v1/chat/completions`)
- **인증**: Bearer 토큰 (API 키)
- **기능**: 스트리밍, 도구 호출, 추론 콘텐츠 (Chain-of-Thought), 다중 리전 배포
- **지원 리전**:
  - `beijing` — 华北2（北京）
  - `virginia` — 美国（弗吉尼亚）
  - `singapore` — 新加坡
  - `hongkong` — 中国香港
  - `frankfurt` — 德国（法兰克福）
- **지원 모델** (API를 통한 동적 검색, 폴백 목록 포함):
  - **Qwen 시리즈**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **추론**: qwq-plus
  - **서드파티**: deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **설정**: `apiKey`, `region`, `model`
- **모델 검색**: 런타임에 DashScope API에서 사용 가능한 모델을 가져옵니다. 네트워크 실패 시 큐레이션된 목록으로 폴백

### 클라이언트 팩토리 패턴

각 AI 클라이언트 타입에는 `IAIClientFactory`를 구현하는 해당 팩토리가 있습니다:

- `OllamaClientFactory` — OllamaClient 인스턴스 생성
- `DashScopeClientFactory` — DashScopeClient 인스턴스 생성

팩토리 제공:
- `CreateClient(Dictionary<string, object> config)` — 설정에서 클라이언트 인스턴스화
- `GetConfigKeyOptions(string key, ...)` — 설정 키에 대한 동적 옵션 반환 (예: 사용 가능한 모델, 리전)
- `GetDisplayName()` — 클라이언트 타입의 현지화된 표시 이름

### AI 플랫폼 지원 목록

#### 상태 설명
- ✅ 구현됨
- 🚧 개발 중
- 📋 계획 중
- 💡 고려 중

*참고: 개발자의 네트워크 환경 영향으로 [고려 중] 해외 클라우드 AI 서비스 통합에는 네트워크 프록시 도구가 필요할 수 있으며, 디버깅 프로세스가 불안정할 수 있습니다.*

#### 플랫폼 목록

| 플랫폼 | 상태 | 타입 | 설명 |
|------|------|------|------|
| Ollama | ✅ | 로컬 | 로컬 AI 서비스, 로컬 모델 배포 지원 |
| DashScope (Alibaba Cloud Bailian) | ✅ | 클라우드 | Alibaba Cloud Bailian AI 서비스, 다중 리전 배포 지원 |
| Baidu Qianfan (ERNIE Bot) | 📋 | 클라우드 | Baidu ERNIE Bot AI 서비스 |
| Zhipu AI (GLM) | 📋 | 클라우드 | Zhipu Qingyan AI 서비스 |
| Moonshot AI (Kimi) | 📋 | 클라우드 | Moonshot Kimi AI 서비스 |
| Doubao (Volcano Engine) | 📋 | 클라우드 | ByteDance Doubao AI 서비스 |
| DeepSeek (직접 연결) | 📋 | 클라우드 | Deepseek AI 서비스 |
| 01.AI | 📋 | 클라우드 | 01.AI AI 서비스 |
| Tencent Hunyuan | 📋 | 클라우드 | Tencent Hunyuan AI 서비스 |
| SiliconFlow | 📋 | 클라우드 | SiliconFlow AI 서비스 |
| MiniMax | 📋 | 클라우드 | MiniMax AI 서비스 |
| OpenAI | 💡 | 클라우드 | OpenAI API 서비스 (GPT 시리즈) |
| Anthropic | 💡 | 클라우드 | Anthropic Claude AI 서비스 |
| Google DeepMind | 💡 | 클라우드 | Google Gemini AI 서비스 |
| Mistral AI | 💡 | 클라우드 | Mistral AI 서비스 |
| Groq | 💡 | 클라우드 | Groq 고속 AI 추론 서비스 |
| Together AI | 💡 | 클라우드 | Together AI 오픈소스 모델 서비스 |
| xAI | 💡 | 클라우드 | xAI Grok 서비스 |
| Cohere | 💡 | 클라우드 | Cohere 엔터프라이즈 NLP 서비스 |
| Replicate | 💡 | 클라우드 | Replicate 오픈소스 모델 호스팅 플랫폼 |
| Hugging Face | 💡 | 클라우드 | Hugging Face 오픈소스 AI 커뮤니티 및 모델 플랫폼 |
| Cerebras | 💡 | 클라우드 | Cerebras AI 추론 최적화 서비스 |
| Databricks | 💡 | 클라우드 | Databricks 엔터프라이즈 AI 플랫폼 (MosaicML) |
| Perplexity AI | 💡 | 클라우드 | Perplexity AI 검색 Q&A 서비스 |
| NVIDIA NIM | 💡 | 클라우드 | NVIDIA AI 추론 마이크로서비스 |

---

## 주요 설계 결정

### 인스턴스 클래스로서의 저장소 (정적 아님)

`IStorage`는 정적 유틸리티가 아닌 주입 가능한 인스턴스로 설계되었습니다. 이를 통해:

- 직접 파일 시스템 접근 — IStorage는 시스템의 내부 지속성 채널이며 executor를 통과**하지 않습니다**.
- **AI가 IStorage를 제어할 수 없음** — executor는 AI 도구에서 시작한 IO를 관리합니다. IStorage는 프레임워크 자체의 내부 데이터 읽기/쓰기를 관리합니다. 이들은 근본적으로 다른 관심사입니다.
- 모의 구현으로 테스트 가능.
- 소비자를 수정하지 않고도 미래에 다른 저장소 백엔드 지원.

### 보안 경계로서의 Executor

Executor는 I/O 작업의 **유일한** 경로입니다. 디스크, 네트워크 또는 명령줄 접근이 필요한 도구는 **반드시** executor를 통과해야 합니다. 이 설계는 다음을 강제합니다:

- 각 executor는 권한 검증을 위한 스레드 잠금과 함께 **독립적인 스케줄링 스레드**를 소유합니다.
- 중앙화된 권한 확인 — executor는 being의 **전용 PermissionManager**를 쿼리합니다.
- 우선순위 지원 및 타임아웃 제어가 있는 요청 큐.
- 모든 외부 작업에 대한 감사 로깅.
- 예외 격리 — 한 executor의 실패가 다른 executor에 영향을 미치지 않습니다.
- Circuit breaker — 연속적인 실패 후 executor를 일시적으로 중지하여 연쇄 실패 방지.

### 경량 객체로서의 ContextManager

각 `ExecuteOneRound()`는 새로운 `ContextManager` 인스턴스를 생성합니다:

1. 소울 파일 + 최근 채팅 기록 로드.
2. AI 클라이언트에 요청 전송.
3. AI가 일반 텍스트를 반환할 때까지 ToolCall 루프.
4. 응답을 ChatSystem에 저장.
5. 삭제.

이를 통해 각 라운드가 격리되고 무상태로 유지됩니다.

### 클래스 재작성을 통한 자기 진화

Silicon Being은 런타임에 자체 C# 클래스를 재작성할 수 있습니다:

1. AI가 새 클래스 코드 생성 (반드시 `SiliconBeingBase` 상속).
2. **컴파일 타임 참조 제어** (주요 방어): 컴파일러에는 허용된 어셈블리 목록만 제공 — `System.IO`, `System.Reflection` 등은 제외되므로 위험한 코드는 타입 레벨에서 불가능.
3. **런타임 정적 분석** (보조 방어): `SecurityScanner`는 성공적인 컴파일 후에도 코드에서 위험한 패턴을 스캔.
4. Roslyn이 코드를 메모리에서 컴파일.
5. 성공 시: `SiliconBeingManager.ReplaceBeing()`이 현재 인스턴스를 교체하고, 상태가 마이그레이션되며, 암호화된 코드가 디스크에 저장.
6. 실패 시: 새 코드 폐기, 기존 구현 유지.

사용자 정의 `IPermissionCallback` 구현도 컴파일 및 `ReplacePermissionCallback()`을 통해 주입될 수 있어 being이 자체 권한 논리를 사용자 정의할 수 있습니다.

코드는 디스크에 AES-256 암호화로 저장됩니다. 암호화 키는 PBKDF2를 통해 being의 GUID(대문자)에서 파생됩니다.

---

## 토큰 사용량 감사

`TokenUsageAuditManager`는 모든 being의 AI 토큰 소비를 추적합니다:

- `TokenUsageRecord` — 요청별 기록 (being ID, 모델, 프롬프트 토큰, 완성 토큰, 타임스탬프)
- `TokenUsageSummary` — 집계 통계
- `TokenUsageQuery` — 기록 필터링을 위한 쿼리 매개변수
- 시간 시리즈 쿼리를 위해 `ITimeStorage`를 통해 저장
- Web UI (AuditController) 및 `TokenAuditTool` (curator 전용)를 통해 접근 가능

---

## 캘린더 시스템

시스템은 주요 세계 캘린더 시스템을 커버하는 추상 `CalendarBase` 클래스에서 파생된 **32개 캘린더 구현**을 포함합니다:

| 캘린더 | ID | 설명 |
|--------|-----|------|
| BuddhistCalendar | `buddhist` | 불교 시대 (BE), 연도 + 543 |
| CherokeeCalendar | `cherokee` | 체로키 캘린더 시스템 |
| ChineseLunarCalendar | `lunar` | 윤달이 있는 중국 음력 |
| ChulaSakaratCalendar | `chula_sakarat` - Chula Sakarat (CS), 연도 - 638 |
| CopticCalendar | `coptic` - 콥트 캘린더 |
| DaiCalendar | `dai` - 완전한 음력 계산이 있는 다이 캘린더 |
| DehongDaiCalendar | `dehong_dai` - 더홍 다이 캘린더 변형 |
| EthiopianCalendar | `ethiopian` - 에티오피아 캘린더 |
| FrenchRepublicanCalendar | `french_republican` - 프랑스 공화국 캘린더 |
| GregorianCalendar | `gregorian` - 표준 그레고리력 |
| HebrewCalendar | `hebrew` - 히브리 (유대) 캘린더 |
| IndianCalendar | `indian` - 인도 국가 캘린더 |
| InuitCalendar | `inuit` - 이누이트 캘린더 시스템 |
| IslamicCalendar | `islamic` - 이슬람 히지라 캘린더 |
| JapaneseCalendar | `japanese` - 일본 연호 (Nengo) 캘린더 |
| JavaneseCalendar | `javanese` - 자와 이슬람 캘린더 |
| JucheCalendar | `juche` - 주체 캘린더 (조선민주주의인민공화국), 연도 - 1911 |
| JulianCalendar | `julian` - 율리우스력 |
| KhmerCalendar | `khmer` - 크메르 캘린더 |
| MayanCalendar | `mayan` - 마야 장군 캘린더 |
| MongolianCalendar | `mongolian` - 몽골 캘린더 |
| PersianCalendar | `persian` - 페르시아 (태양 히지라) 캘린더 |
| RepublicOfChinaCalendar | `roc` - 중화민국 (민국) 캘린더, 연도 - 1911 |
| RomanCalendar | `roman` - 로마 캘린더 |
| SakaCalendar | `saka` - 사카 캘린더 (인도네시아) |
| SexagenaryCalendar | `sexagenary` - 중국 육십갑자 (干支 Ganzhi) |
| TibetanCalendar | `tibetan` - 티베트 캘린더 |
| VietnameseCalendar | `vietnamese` - 베트남 음력 (고려 띠 변형) |
| VikramSamvatCalendar | `vikram_samvat` - 비크람 삼바트 캘린더 |
| YiCalendar | `yi` - 이족 캘린더 시스템 |
| ZoroastrianCalendar | `zoroastrian` - 조로아스터교 캘린더 |

`CalendarTool`은 다음 작업을 제공합니다: `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (캘린더 간 날짜 변환).

---

## Web UI 아키텍처

### 스킨 시스템

Web UI는 애플리케이션 논리를 변경하지 않고 완전한 UI 사용자 정의를 가능하게 하는 **플러그형 스킨 시스템**을 특징으로 합니다:

- **ISkin 인터페이스** — 모든 스킨에 대한 계약 정의:
  - 핵심 렌더링 메서드 (`RenderHtml`, `RenderError`)
  - 20개 이상의 UI 구성 요소 메서드 (버튼, 입력, 카드, 테이블, 배지, 버블, 진행률, 탭 등)
  - `CssBuilder`를 통한 테마 CSS 생성
  - `SkinPreviewInfo` — 초기화 페이지 스킨 선택기의 색상 팔레트 및 아이콘

- **내장 스킨** — 4개의 프로덕션 준비 스킨:
  - **Admin** — 시스템 관리를 위한 전문적인 데이터 중심 인터페이스
  - **Chat** — AI 상호작용을 위한 대화 중심 설계
  - **Creative** -创意적 워크플로를 위한 예술적이고 시각적으로 풍부한 레이아웃
  - **Dev** - 구문 강조가 있는 개발자 중심 코드 중심 인터페이스

- **스킨 검색** — `SkinManager`가 리플렉션을 통해 모든 `ISkin` 구현을 자동 검색 및 등록

### HTML / CSS / JS 빌더

Web UI는 템플릿 파일을 완전히 피하고 C#에서 모든 마크업을 생성합니다:

- **`H`** - C# 코드에서 HTML 트리를 구축하기 위한 플루언트 HTML 빌더 DSL
- **`CssBuilder`** - 선택기 및 미디어 쿼리 지원을 갖춘 CSS 빌더
- **`JsBuilder` (`JsSyntax`)** - 인라인 스크립트를 위한 JavaScript 빌더

### 컨트롤러 시스템

Web UI는 다양한 측면을 처리하는 17개 컨트롤러와 함께 **MVC 유사 패턴**을 따릅니다:

| 컨트롤러 | 목적 |
|----------|------|
| About | 정보 페이지 및 프로젝트 정보 |
| Audit | 추세 차트 및 내보내기가 있는 토큰 사용량 감사 대시보드 |
| Being | Silicon Being 관리 및 상태 |
| Chat | SSE를 사용한 실시간 채팅 인터페이스 |
| CodeBrowser | 코드 보기 및 편집 |
| Config | 시스템 설정 관리 |
| Dashboard | 시스템 개요 및 메트릭 |
| Executor | Executor 상태 및 관리 |
| Init | 초기 실행 초기화 마법사 |
| Knowledge | 지식 그래프 시각화 (플레이스홀더) |
| Log | 시스템 로그 뷰어 |
| Memory | 고급 필터링, 통계 및 상세 뷰 기능을 갖춘 장기 메모리 브라우저 |
| Permission | 권한 관리 |
| PermissionRequest | 권한 요청 큐 |
| Project | 프로젝트 관리 (플레이스홀더) |
| Task | 작업 시스템 인터페이스 |
| Timer | 타이머 시스템 관리 |

### 실시간 업데이트

- **SSE (Server-Sent Events)** - `SSEHandler`를 통한 채팅 메시지, being 상태 및 시스템 이벤트에 대한 푸시 기반 업데이트
- **WebSocket 불필요** - 대부분의 실시간 요구에 SSE를 사용하는 더 간단한 아키텍처
- **자동 재연결** - 복원력 있는 연결을 위한 클라이언트 측 재연결 논리

### 현지화

세 로캘이 내장되어 있습니다: `ZhCN`(중국어 간체), `ZhHK`(중국어 번체), `EnUS`(영어). 활성 로캘은 `DefaultConfigData.Language`를 통해 선택되며 `LocalizationManager`를 통해 해석됩니다.

---

## 데이터 디렉토리 구조

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # Curator의 소울 파일
    │   ├── state.json       # 런타임 상태
    │   ├── code.enc         # AES 암호화된 사용자 정의 클래스 코드
    │   └── permission.enc   # AES 암호화된 사용자 정의 권한 콜백
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```
