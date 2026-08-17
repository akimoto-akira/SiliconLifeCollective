# 아키텍처

> **버전: v0.2.0-alpha**

[English](../en/architecture.md) | [Deutsch](../de-DE/architecture.md) | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | **한국어** | [Čeština](../cs-CZ/architecture.md) | [Русский](../ru-RU/architecture.md)

## 듀얼 버전 아키텍처

본 프로젝트는 두 가지 구현 버전을 제공하며, 동일한 아키텍처 설계를 공유하지만 스토리지 및 성능 최적화 측면에서 차이가 있습니다:

### SiliconLife.Default(기본 버전)
- **포지션**: 기본 구현, 주로 아키텍처 실현 가능성 검증에 사용
- **실행 모드**: 콘솔 애플리케이션
- **스토리지 방식**: 순수 파일 시스템 JSON 스토리지
- **적용 시나리오**: 데이터 안전성 요구가 높고, 메모리 리소스가 제한되며, 데이터량이 적은 시나리오
- **역할 설명**: 아키텍처 검증의 기준 구현으로서, 간단하고 안정적인 실행 방식을 제공하며, 본 프로젝트를 처음 접하거나 개발 디버깅 또는 데이터 안전을 우선시하는 시나리오에 적합

### SiliconLife.Fast(고성능 버전)
- **포지션**: 주력 프로덕션 버전
- **실행 모드**: 데스크톱 애플리케이션(Windows 시스템 트레이 / Linux 상태 창)
- **스토리지 방식**: SpeedyPack 메모리 스토리지 + 비동기 배치 영속화(.spk 파일 형식)
- **적용 시나리오**: 고동시성, 저지연, 대용량 데이터 시나리오
- **플랫폼 지원**: Windows/macOS(전체 기능, 시스템 트레이 포함), Linux(상태 창, 트레이 아이콘 없음)
- **특징**:
  - Windows/macOS 시스템 트레이 백그라운드 실행, 트레이 상태 창으로 실시간 모니터링; Linux 상태 창 직접 표시
  - SpeedyPack 엔진 + 자동 압축으로 데이터 안전 보장
  - Component UI 아키텍처, 27개 선언형 컴포넌트
  - 7종 스킨 테마, 자동 발견 및 전환 지원
  - Linux 자동 브라우저 열기로 Web UI 접속, `--no-tray` 매개변수 지원
- **성능 향상**: 스토리지 읽기 지연 1000배 감소, 쓰기 지연 15000배 감소
- **역할 설명**: 심도 있게 최적화된 프로덕션급 구현으로, 시스템 트레이 백그라운드 실행, SpeedyPack 엔진 + 자동 압축 등의 기능을 갖추고 있어 장기 실행 및 실제 프로덕션 환경의 최적 선택

> **참고**: 본 문서에서 설명하는 아키텍처는 두 버전 모두에 적용되며, 스토리지 구현 부분에서만 차이가 있습니다. SiliconLife.Default는 아키텍처 검증 기준으로, SiliconLife.Fast는 프로덕션 환경 주력 버전으로 사용됩니다.

---

## 핵심 개념

### 실리콘 비잉

시스템의 각 AI 에이전트는 **실리콘 비잉** —— 고유한 정체성, 성격, 능력을 가진 자율적 개체입니다. 각 실리콘 비잉은 **소울 파일**(Markdown 프롬프트)에 의해 구동되며, 그 행동 패턴을 정의합니다.

### 실리콘 큐레이터

**실리콘 큐레이터**는 최고 시스템 권한을 가진 특별한 실리콘 비잉입니다. 시스템 관리자 역할을 수행합니다:

- 다른 실리콘 비잉의 생성 및 관리
- 사용자 요청 분석 및 태스크 분해
- 태스크를 적절한 실리콘 비잉에 분배
- 실행 품질 모니터링 및 실패 처리
- **우선 스케줄링**으로 사용자 메시지에 응답(아래 참조)

### 소울 파일

각 실리콘 비잉 데이터 디렉토리에 저장된 Markdown 파일(`soul.md`)입니다. 각 AI 요청에 시스템 프롬프트로 주입되어 비잉의 성격, 의사결정 패턴, 행동 제약을 정의합니다.

---

## 스케줄링: 타임슬롯 공정 스케줄링

### 메인 루프 + 클록 오브젝트

시스템은 전용 백그라운드 스레드에서 **클록 구동 메인 루프**를 실행합니다:

```
메인 루프（전용 스레드, 워치독 + 서킷 브레이커）
  └── 클록 오브젝트 A（우선순위=0, 간격=100ms）
  └── 클록 오브젝트 B（우선순위=1, 간격=500ms）
  └── 실리콘 비잉 매니저（메인 루프에서 직접 클록 트리거）
        └── 실리콘 비잉 러너 → 실리콘 비잉 1 → 클록 트리거 → 1라운드 실행
        └── 실리콘 비잉 러너 → 실리콘 비잉 2 → 클록 트리거 → 1라운드 실행
        └── 실리콘 비잉 러너 → 실리콘 비잉 3 → 클록 트리거 → 1라운드 실행
        └── ...
```

핵심 설계 결정:

- **실리콘 비잉은 클록 오브젝트를 상속하지 않습니다.** 고유한 `Tick()` 메서드를 가지며, 메인 루프에 직접 등록되지 않고 `SiliconBeingManager`를 통해 `SiliconBeingRunner`가 호출합니다.
- **실리콘 비잉 매니저**는 메인 루프에서 직접 클록 트리거되며, 모든 비잉의 단일 에이전트 역할을 합니다.
- **실리콘 비잉 러너**는 임시 스레드에서 각 비잉의 `Tick()`을 래핑하며, 타임아웃 및 비잉별 서킷 브레이커(연속 3회 타임아웃 → 1분 쿨다운)를 갖습니다.
- 각 비잉의 실행은 클록 트리그마다 **1라운드** AI 요청 + 툴 콜로 제한되어, 어떤 비잉도 메인 루프를 독점할 수 없습니다.
- **퍼포먼스 모니터**가 클록 실행 시간을 추적하여 관측 가능성을 제공합니다.

### 큐레이터 우선 응답

사용자가 실리콘 큐레이터에게 메시지를 보낼 때:

1. 현재 비잉(예: 비잉 A)이 현재 라운드를 완료합니다 —— **중단 없음**.
2. 매니저가 **나머지 큐를 건너뜁니다**.
3. 루프가 **큐레이터부터 다시 시작**되어 즉시 실행됩니다.

이는 진행 중인 태스크를 방해하지 않으면서 사용자 상호작용에 응답함을 보장합니다.

---

## 컴포넌트 아키텍처

```
┌─────────────────────────────────────────────────────────┐
│                        코어 호스트                        │
│  （통합 호스트 —— 모든 컴포넌트를 조립하고 관리）            │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ 메인 루프 │  │ 서비스 로케이터│  │      설정         │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │        실리콘 비잉 매니저（클록 오브젝트）            │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │큐레이터   │ │비잉 A    │ │비잉 B    │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              공유 서비스                            │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │채팅 시스템│  │ 스토리지 │  │  퍼미션 매니저    │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │AI 클라이언트│ │이그제큐터 │  │   툴 매니저       │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  │  ┌──────────┐ ┌──────────┐                        │   │
│  │  │플러그인 로더│ │노리지 네트워크│                      │   │
│  │  └──────────┘ └──────────┘                        │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  이그제큐터                         │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │  디스크   │  │  네트워크 │  │  커맨드라인       │  │   │
│  │  │이그제큐터 │  │이그제큐터 │  │  이그제큐터       │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              IM 프로바이더                          │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ 콘솔     │  │  Web     │  │  Feishu / ...    │  │   │
│  │  │프로바이더 │  │프로바이더 │  │  프로바이더       │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## 서비스 로케이터

`ServiceLocator`는 스레드 안전 싱글톤 레지스트리로, 모든 핵심 서비스에 대한 접근을 제공합니다:

| 속성 | 타입 | 설명 |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | 중앙 채팅 세션 매니저 |
| `IMManager` | `IMManager` | IM 프로바이더 라우터 |
| `AuditLogger` | `AuditLogger` | 권한 감사 추적 |
| `GlobalAcl` | `GlobalACL` | 글로벌 ACL |
| `BeingFactory` | `ISiliconBeingFactory` | 비잉 생성 팩토리 |
| `BeingManager` | `SiliconBeingManager` | 활성 비잉 라이프사이클 매니저 |
| `DynamicBeingLoader` | `DynamicBeingLoader` | 동적 비잉 로더 |
| `TokenUsageAudit` | `ITokenUsageAudit` | 토큰 사용 추적 |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | 토큰 사용 보고서 |

또한 비잉 GUID를 키로 하는 각 비잉의 `PermissionManager` 레지스트리를 유지합니다.

---

## 채팅 시스템

### 세션 유형

채팅 시스템은 `SessionBase`를 통해 세 가지 세션 유형을 지원합니다:

| 유형 | 클래스 | 설명 |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | 두 참여자 간의 1:1 대화 |
| `GroupChat` | `GroupChatSession` | 다중 참여자 그룹 채팅 |
| `Broadcast` | `BroadcastChannel` | 고정 ID를 가진 개방형 채널; 비잉이 동적으로 구독하며, 구독 후에만 메시지를 수신 |

### 브로드캐스트 채널

`BroadcastChannel`은 시스템 전체 공지를 위한 특별한 세션 유형입니다:

- **고정 채널 ID** —— `SingleChatSession` 및 `GroupChatSession`과 달리, 채널 ID는 멤버 GUID에서 파생되지 않고 잘 알려진 상수입니다.
- **동적 구독** —— 비잉이 런타임에 구독/구독 취소; 구독 후에 게시된 메시지만 수신합니다.
- **대기 메시지 필터링** —— `GetPendingMessages()`는 비잉의 구독 시간 이후에 게시되고 아직 읽지 않은 메시지만 반환합니다.
- **채팅 시스템에서 관리** —— `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### 채팅 메시지

`ChatMessage` 모델은 AI 대화 컨텍스트 및 토큰 추적 필드를 포함합니다:

| 필드 | 타입 | 설명 |
|-------|------|-------------|
| `Id` | `Guid` | 고유 메시지 식별자 |
| `SenderId` | `Guid` | 발신자의 고유 식별자 |
| `ChannelId` | `Guid` | 채널/대화 식별자 |
| `Content` | `string` | 메시지 내용 |
| `Timestamp` | `DateTime` | 메시지 발송 시간 |
| `Type` | `MessageType` | 텍스트, 이미지, 파일 또는 시스템 알림 |
| `ReadBy` | `List<Guid>` | 이 메시지를 읽은 참여자 ID |
| `Role` | `MessageRole` | AI 대화 역할(사용자, 어시스턴트, 툴) |
| `ToolCallId` | `string?` | 툴 리절트 메시지의 툴 콜 ID |
| `ToolCallsJson` | `string?` | 어시스턴트 메시지의 직렬화된 툴 콜 JSON |
| `Thinking` | `string?` | AI의 사고망 추론 |
| `PromptTokens` | `int?` | 프롬프트의 토큰 수(입력) |
| `CompletionTokens` | `int?` | 컴플리션의 토큰 수(출력) |
| `TotalTokens` | `int?` | 사용된 총 토큰 수(입력 + 출력) |
| `FileMetadata` | `FileMetadata?` | 첨부 파일 메타데이터(메시지에 파일이 포함된 경우) |

### 채팅 메시지 큐

`ChatMessageQueue`는 채팅 메시지의 비동기 처리를 관리하는 스레드 안전 메시지 큐 시스템입니다:

- **스레드 안전** - 락 메커니즘으로 동시 접근 안전 보장
- **비동기 처리** - 비동기 메시지 인큐 및 디큐 지원
- **메시지 정렬** - 메시지의 시간 순서 유지
- **배치 작업** - 배치 메시지 가져오기 지원

### 파일 메타데이터

`FileMetadata`는 채팅 메시지에 첨부된 파일 정보를 관리합니다:

- **파일 정보** - 파일명, 크기, 타입, 경로
- **업로드 시간** - 파일 업로드 타임스탬프
- **업로더** - 파일을 업로드한 사용자 또는 실리콘 비잉 ID

### 스트림 취소 매니저

`StreamCancellationManager`는 AI 스트리밍 응답의 취소 메커니즘을 제공합니다:

- **스트림 제어** - 진행 중인 AI 스트리밍 응답 취소 지원
- **리소스 정리** - 취소 시 관련 리소스 올바르게 정리
- **동시성 안전** - 여러 스트림 동시 관리 지원

### 채팅 기록 보기

새로 추가된 채팅 기록 보기 기능은 사용자가 실리콘 비잉의 과거 대화를 탐색할 수 있게 합니다:

- **세션 목록** - 모든 과거 세션 표시
- **메시지 상세** - 전체 메시지 기록 보기
- **타임라인 뷰** - 시간 순서로 메시지 표시
- **API 지원** - 세션 및 메시지 데이터를 가져오는 RESTful API 제공

---

## AI 클라이언트 시스템

시스템은 `IAIClient` 인터페이스를 통해 여러 AI 백엔드를 지원합니다:

### OllamaClient

- **타입**: 로컬 AI 서비스
- **프로토콜**: 네이티브 Ollama HTTP API(`/api/chat`, `/api/generate`)
- **기능**: 스트리밍, 툴 콜, 로컬 모델 호스팅
- **설정**: `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient(알리윈 바이리안)

- **타입**: 클라우드 AI 서비스
- **프로토콜**: OpenAI 호환 API(`/compatible-mode/v1/chat/completions`)
- **인증**: Bearer 토큰(API 키)
- **기능**: 스트리밍, 툴 콜, 추론 콘텐츠(사고망), 다중 리전 배포
- **지원 리전**:
  - `beijing` —— 화베이2(베이징)
  - `virginia` —— 미국(버지니아)
  - `singapore` —— 싱가포르
  - `hongkong` —— 중국 홍콩
  - `frankfurt` —— 독일(프랑크푸르트)
- **지원 모델**(API를 통한 동적 발견, 폴백 목록 포함):
  - **통의천문 시리즈**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **추론**: qwq-plus
  - **서드파티**: deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **설정**: `apiKey`, `region`, `model`
- **모델 발견**: 런타임에 바이리안 API에서 사용 가능한 모델을 가져옴; 네트워크 장애 시 큐레이션된 목록으로 폴백

### VolcengineArkClient(바이트댄스 Ark)

- **타입**: 클라우드 AI 서비스
- **프로토콜**: OpenAI 호환 API
- **인증**: Bearer 토큰(API 키)
- **기능**: 스트리밍 및 비스트리밍 모드 지원, 내장 이중 계층 속도 제어
  - 자기 속도 제어: 요청 간 최소 간격 강제
  - 서버 속도 제한: 429 오류 처리, 지수 백오프 재시도
- **설정**: `apiKey`, `endpoint`, `model`
- **특징**: 바이트댄스 AI 서비스, 다양한 더우바오 모델 지원

### DeepSeekClient(DeepSeek)

- **타입**: 클라우드 AI 서비스
- **프로토콜**: OpenAI 호환 API
- **엔드포인트**: `https://api.deepseek.com`
- **인증**: Bearer 토큰(API 키)
- **기능**: 스트리밍, 툴 콜, 사고망(thinking) 모드, 추론 콘텐츠
- **컨텍스트 윈도우**: 최대 1M 토큰(DeepSeek-V4 시리즈), 기본 131K 토큰
- **설정**: `apiKey`, `endpoint`, `model`, `contextWindowTokens`

### ZhipuClient(즈푸 GLM)

- **타입**: 클라우드 AI 서비스
- **프로토콜**: OpenAI 호환 API
- **엔드포인트**: `https://open.bigmodel.cn/api/paas/v4`
- **인증**: Bearer 토큰(API 키)
- **기능**: 스트리밍, 툴 콜, 사고망(thinking) 모드, 비전(모델별 지원), 무료 모델(glm-4-flash) 지원
- **컨텍스트 윈도우**: 최대 1M 토큰(GLM-4-Long, GLM-5.2), 기본 131K 토큰
- **설정**: `apiKey`, `endpoint`, `model`, `contextWindowTokens`

### ErnieClient(바이두 천판/원이이옌)

- **타입**: 클라우드 AI 서비스
- **프로토콜**: OpenAI 호환 API(천판 v2)
- **엔드포인트**: `https://qianfan.baidubce.com/v2`
- **인증**: Bearer 토큰(API 키)
- **기능**: 스트리밍, 툴 콜, 무료 모델(ernie-speed, ernie-tiny) 지원
- **컨텍스트 윈도우**: 최대 131K 토큰
- **설정**: `apiKey`, `endpoint`, `model`, `contextWindowTokens`

### HunyuanClient(텐센트 혼위안)

- **타입**: 클라우드 AI 서비스
- **프로토콜**: OpenAI 호환 API
- **엔드포인트**: TokenHub(권장) `https://tokenhub.tencentmaas.com/v1`, 레거시 `https://api.hunyuan.cloud.tencent.com/v1`
- **인증**: Bearer 토큰(API 키)
- **기능**: 스트리밍, 툴 콜, 사고망(thinking) 모드, 추론 콘텐츠
- **컨텍스트 윈도우**: 최대 262K 토큰
- **특징**: 모델명에 따라 TokenHub/레거시 엔드포인트 자동 선택(hy3 시리즈 → TokenHub)
- **설정**: `apiKey`, `endpoint`, `model`, `contextWindowTokens`

### MiniMaxClient

- **타입**: 클라우드 AI 서비스
- **프로토콜**: OpenAI 호환 API
- **엔드포인트**: 국내 `https://api.minimaxi.com/v1`, 국제 `https://api.minimax.io/v1`
- **인증**: Bearer 토큰(API 키)
- **기능**: 스트리밍, 툴 콜, 사고망(thinking) 모드, 적응형 추론(reasoning_split), 멀티모달
- **컨텍스트 윈도우**: 최대 1M 토큰
- **설정**: `apiKey`, `endpoint`, `model`, `contextWindowTokens`

### MoonshotClient(위즈덤 문/Kimi)

- **타입**: 클라우드 AI 서비스
- **프로토콜**: OpenAI 호환 API
- **엔드포인트**: `https://api.moonshot.cn/v1`
- **인증**: Bearer 토큰(API 키)
- **기능**: 스트리밍, 툴 콜, 사고망(thinking) 모드, 추론 콘텐츠
- **컨텍스트 윈도우**: 최대 262K 토큰
- **설정**: `apiKey`, `endpoint`, `model`, `contextWindowTokens`

### SiliconFlowClient(실리콘 플로우)

- **타입**: 클라우드 AI 서비스
- **프로토콜**: OpenAI 호환 API
- **엔드포인트**: `https://api.siliconflow.cn/v1`
- **인증**: Bearer 토큰(API 키)
- **기능**: 스트리밍, 툴 콜, 사고망(thinking) 모드, 동적 모델 목록, 100개 이상 오픈소스 모델 집합
- **컨텍스트 윈도우**: 최대 1M 토큰
- **설정**: `apiKey`, `endpoint`, `model`, `contextWindowTokens`

### 클라이언트 팩토리 패턴

각 AI 클라이언트 타입에는 `IAIClientFactory`를 구현하는 해당 팩토리가 있습니다:

- `OllamaClientFactory` —— OllamaClient 인스턴스 생성
- `DashScopeClientFactory` —— DashScopeClient 인스턴스 생성
- `VolcengineArkClientFactory` —— VolcengineArkClient 인스턴스 생성
- `HerdsmanClientFactory` —— HerdsmanClient 인스턴스 생성
- `LongCatClientFactory` —— LongCatClient 인스턴스 생성
- `QiniuAIClientFactory` —— QiniuAIClient 인스턴스 생성
- `DeepSeekClientFactory` —— DeepSeekClient 인스턴스 생성
- `ZhipuClientFactory` —— ZhipuClient 인스턴스 생성
- `ErnieClientFactory` —— ErnieClient 인스턴스 생성
- `HunyuanClientFactory` —— HunyuanClient 인스턴스 생성
- `MiniMaxClientFactory` —— MiniMaxClient 인스턴스 생성
- `MoonshotClientFactory` —— MoonshotClient 인스턴스 생성
- `SiliconFlowClientFactory` —— SiliconFlowClient 인스턴스 생성

팩토리가 제공하는 기능:
- `CreateClient(Dictionary<string, object> config)` —— 설정에서 클라이언트 인스턴스화
- `GetConfigKeyOptions(string key, ...)` —— 설정 키의 동적 옵션 반환(예: 사용 가능한 모델, 리전)
- `GetDisplayName()` —— 클라이언트 타입의 로컬라이제이션된 표시 이름

### IAIClient 역량 인터페이스

`IAIClient` 인터페이스는 AI 클라이언트의 역량 선언 속성을 정의하며, `ContextManager`는 이를 기반으로 동작을 적응적으로 조정합니다:

| 속성 | 타입 | 설명 |
|------|------|------|
| `StreamingMode` | `bool?` | 스트리밍 모드 지원: true=스트리밍 전용, false=비스트리밍 전용, null=둘 다 지원(기본값: 스트리밍) |
| `SupportsToolCalls` | `bool?` | 툴 콜 지원: true=지원, false=미지원(툴 주입 무시), null=알 수 없음(기본값: 지원) |
| `ContextWindowTokens` | `int?` | 컨텍스트 윈도우 크기(토큰 수), 고정 MaxContextMessages 대신 토큰 예산 커팅에 사용 |
| `SupportsVision` | `bool?` | 비전 입력 지원: true=이미지 지원, false=미지원, null=알 수 없음(기본값: 미지원) |
| `SupportsAudio` | `bool?` | 오디오 입력 지원: true=오디오 지원, false=미지원, null=알 수 없음(기본값: 미지원) |

### AI 플랫폼 지원 목록

#### 상태 설명
- ✅ 구현 완료
- 🚧 개발 중
- 📋 계획 중
- 💡 검토 중
- ⚠️ 폐기됨

*참고: 개발자의 네트워크 환경 영향으로, [검토 중]인 해외 클라우드 AI 서비스 연동은 네트워크 프록시 도구를 통한 접근이 필요할 수 있으며, 디버깅 과정에 불안정성이 있을 수 있습니다.*

#### 플랫폼 목록

| 플랫폼 | 상태 | 타입 | 설명 |
|------|------|------|------|
| Ollama | ✅ | 로컬 | 로컬 AI 서비스, 로컬 모델 배포 지원 |
| DashScope(알리윈 바이리안) | ✅ | 클라우드 | 알리윈 바이리안 AI 서비스, 다중 리전 배포 지원 |
| DeepSeek | ✅ | 클라우드 | DeepSeek AI 서비스, 사고망 모드, 1M 컨텍스트 |
| 즈푸 AI(GLM) | ✅ | 클라우드 | 즈푸칭옌 AI 서비스, 사고망 모드, 비전, 1M 컨텍스트 |
| 바이두 천판(원이이옌) | ✅ | 클라우드 | 바이두 원이이옌 AI 서비스, 131K 컨텍스트, 무료 모델 지원 |
| 텐센트 혼위안 | ✅ | 클라우드 | 텐센트 혼위안 AI 서비스, 듀얼 엔드포인트, 262K 컨텍스트 |
| MiniMax | ✅ | 클라우드 | MiniMax AI 서비스, 1M 컨텍스트, 멀티모달 |
| 위즈덤 문(Kimi) | ✅ | 클라우드 | 위즈덤 문 Kimi AI 서비스, 262K 컨텍스트 |
| 실리콘 플로우 | ✅ | 클라우드 | 실리콘 플로우 AI 서비스, 100+ 모델 집합, 1M 컨텍스트 |
| 바이트댄스 Ark.더우바오 | ✅ | 클라우드 | 바이트댄스 더우바오 AI 서비스 |
| Herdsman | ✅ | 로컬/클라우드 | 인증 없는 추론 엔진, OpenAI API 형식 호환 |
| Meituan LongCat | ✅ | 클라우드 | 메이퇀 자체 개발 대형 모델, OpenAI API 형식 호환, API 키 인증 |
| Qiniu Cloud AI | ✅ | 클라우드 | 치니우 클라우드 대형 모델 추론 서비스, OpenAI API 형식 호환, API 키 인증 |
| 링이완우(01.AI) | ⚠️ | 클라우드 | 폐기됨: 신규 사용자 등록 중단 |
| OpenAI | 💡 | 클라우드 | OpenAI API 서비스(GPT 시리즈) |
| Anthropic | 💡 | 클라우드 | Anthropic Claude AI 서비스 |
| Google DeepMind | 💡 | 클라우드 | Google Gemini AI 서비스 |
| Mistral AI | 💡 | 클라우드 | Mistral AI 서비스 |
| Groq | 💡 | 클라우드 | Groq 고속 AI 추론 서비스 |
| Together AI | 💡 | 클라우드 | Together AI 오픈소스 모델 서비스 |
| xAI | 💡 | 클라우드 | xAI Grok 서비스 |
| Cohere | 💡 | 클라우드 | Cohere 엔터프라이즈급 NLP 서비스 |
| Replicate | 💡 | 클라우드 | Replicate 오픈소스 모델 호스팅 플랫폼 |
| Hugging Face | 💡 | 클라우드 | Hugging Face 오픈소스 AI 커뮤니티 및 모델 플랫폼 |
| Cerebras | 💡 | 클라우드 | Cerebras AI 추론 최적화 서비스 |
| Databricks | 💡 | 클라우드 | Databricks 엔터프라이즈 AI 플랫폼(MosaicML) |
| Perplexity AI | 💡 | 클라우드 | Perplexity AI 검색 질의응답 서비스 |
| NVIDIA NIM | 💡 | 클라우드 | NVIDIA AI 추론 마이크로서비스 |

---

## 핵심 설계 결정

### 스토리지를 인스턴스 클래스로 설계(정적이 아님)

`IStorage`는 주입 가능한 인스턴스로 설계되었으며, 정적 유틸리티가 아닙니다. 이는 다음을 보장합니다:

- 직접 파일 시스템 접근 —— IStorage는 시스템의 내부 영속화 채널이며, 이그제큐터를 통해 라우팅되지 **않습니다**.
- **AI가 IStorage를 제어할 수 없음** —— 이그제큐터는 AI 툴이 시작한 IO를 관리; IStorage는 프레임워크 자체의 내부 데이터 읽기/쓰기를 관리합니다. 이는 근본적으로 다른 관심사입니다.
- 모의 구현을 사용한 테스트 가능.
- 향후 다른 스토리지 백엔드 지원 시 소비자 수정 불필요.

### 이그제큐터를 보안 경계로

이그제큐터는 I/O 작업의 **유일한** 경로입니다. 디스크, 네트워크 또는 커맨드라인 접근이 필요한 툴은 **반드시** 이그제큐터를 거쳐야 합니다. 이 설계는 다음을 강제합니다:

- 각 이그제큐터는 권한 검증을 위한 스레드 락이 있는 **독립적인 디스패치 스레드**를 소유.
- 중앙 집중식 권한 검사 —— 이그제큐터는 비잉의 **프라이빗 퍼미션 매니저**에 쿼리합니다.
- 우선순위 및 타임아웃 제어가 있는 요청 큐 지원.
- 모든 외부 작업의 감사 로그.
- 예외 격리 —— 한 이그제큐터의 실패가 다른 이그제큐터에 영향을 주지 않음.
- 서킷 브레이커 —— 연속 실패 시 이그제큐터를 일시 중지하여 연쇄 실패 방지.

### 컨텍스트 매니저를 경량 오브젝트로

매번 `ExecuteOneRound()`는 새로운 `ContextManager` 인스턴스를 생성합니다:

1. 소울 파일 + 최근 채팅 기록 로드.
2. AI 클라이언트에 요청 전송.
3. AI가 순수 텍스트를 반환할 때까지 툴 콜 루프 처리.
4. 채팅 시스템에 응답 영속화.
5. 해제.

이를 통해 각 라운드가 격리되고 무상태로 유지됩니다.

### 클래스 오버라이드를 통한 자기 진화

실리콘 비잉은 런타임에 자신의 C# 클래스를 오버라이드할 수 있습니다:

1. AI가 새 클래스 코드를 생성(`SiliconBeingBase`를 상속해야 함).
2. **컴파일 시 참조 제어**(주요 방어): 컴파일러는 허용된 어셈블리 목록만 획득 —— `System.IO`, `System.Reflection` 등은 제외되므로 위험한 코드가 타입 수준에서 불가능.
3. **런타임 정적 분석**(보조 방어): `SecurityScanner`가 성공적인 컴파일 후 코드에서 위험한 패턴을 스캔.
4. Roslyn이 메모리에서 코드를 컴파일.
5. 성공 시: `SiliconBeingManager.ReplaceBeing()`이 현재 인스턴스를 교체하고, 상태를 마이그레이션하며, 암호화된 코드를 디스크에 영속화.
6. 실패 시: 새 코드를 폐기하고 기존 구현 유지.

커스텀 `IPermissionCallback` 구현도 `ReplacePermissionCallback()`을 통해 컴파일 및 주입될 수 있어, 비잉이 자체 권한 로직을 커스터마이즈할 수 있습니다.

코드는 디스크에 AES-256으로 암호화되어 저장됩니다. 암호화 키는 비잉의 GUID(대문자)에서 PBKDF2를 통해 파생됩니다.

---

## 토큰 사용 감사

`TokenUsageAuditManager`는 모든 비잉의 AI 토큰 소비를 추적합니다:

- `TokenUsageRecord` —— 요청별 기록(비잉 ID, 모델, 프롬프트 토큰, 컴플리션 토큰, 타임스탬프)
- `TokenUsageSummary` —— 집계 통계
- `TokenUsageQuery` —— 기록 필터링을 위한 쿼리 매개변수
- `ITimeStorage`를 통한 영속화로 시계열 쿼리 지원
- Web UI(UsageController) 및 `TokenAuditTool`(큐레이터 전용)를 통해 접근 가능

---

### 캘린더 시스템

시스템은 추상 `CalendarBase` 클래스에서 파생된 **32종의 캘린더 구현**을 포함하며, 세계 주요 달력 시스템을 다룹니다:

| 캘린더 | ID | 설명 |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | 불력(BE), 연도 + 543 |
| CherokeeCalendar | `cherokee` | 체로키 달력 시스템 |
| ChineseLunarCalendar | `lunar` | 중국 음력, 윤월 포함 |
| ChineseHistoricalCalendar | `chinese_historical` | 중국 역법, 간지기년 및 제왕연호 지원 |
| ChulaSakaratCalendar | `chula_sakarat` | 쭐라사카랏력(CS), 연도 - 638 |
| CopticCalendar | `coptic` | 콥트력 |
| DaiCalendar | `dai` | 태력, 완전한 음력 계산 포함 |
| DehongDaiCalendar | `dehong_dai` | 더홍 태력 변형 |
| EthiopianCalendar | `ethiopian` | 에티오피아력 |
| FrenchRepublicanCalendar | `french_republican` | 프랑스 공화력 |
| GregorianCalendar | `gregorian` | 표준 그레고리력 |
| HebrewCalendar | `hebrew` | 히브리(유대)력 |
| IndianCalendar | `indian` | 인도 국력 |
| InuitCalendar | `inuit` | 이누이트 달력 시스템 |
| IslamicCalendar | `islamic` | 이슬람 회교력 |
| JapaneseCalendar | `japanese` | 일본 연호(엔고)력 |
| JavaneseCalendar | `javanese` | 자바 이슬람력 |
| JucheCalendar | `juche` | 주체력(조선), 연도 - 1911 |
| JulianCalendar | `julian` | 율리우스력 |
| KhmerCalendar | `khmer` | 크메르력 |
| MayanCalendar | `mayan` | 마야 장기력 |
| MongolianCalendar | `mongolian` | 몽골력 |
| PersianCalendar | `persian` | 페르시아(태양 회교)력 |
| RepublicOfChinaCalendar | `roc` | 중화민국력, 연도 - 1911 |
| RomanCalendar | `roman` | 로마력 |
| SakaCalendar | `saka` | 사카력(인도네시아) |
| SexagenaryCalendar | `sexagenary` | 중국 간지력(Ganzhi) |
| TibetanCalendar | `tibetan` | 티베트력 |
| VietnameseCalendar | `vietnamese` | 베트남 음력(고양이 띠 변형) |
| VikramSamvatCalendar | `vikram_samvat` | 비크람 삼바트력 |
| YiCalendar | `yi` | 이족 달력 시스템 |
| ZoroastrianCalendar | `zoroastrian` | 조로아스터력 |

`CalendarTool`은 다음 작업을 제공합니다: `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert`(교차 캘린더 날짜 변환).

---

## Web UI 아키텍처

### 스킨 시스템

Web UI는 **플러그형 스킨 시스템**을 갖추고 있어, 애플리케이션 로직 변경 없이 완전한 UI 커스터마이징이 가능합니다:

- **ISkin 인터페이스** —— 모든 스킨의 계약을 정의하며, 다음을 포함:
  - 핵심 렌더링 메서드(`RenderHtml`, `RenderError`)
  - 20개 이상의 UI 컴포넌트 메서드(버튼, 입력, 카드, 테이블, 배지, 말풍선, 프로그레스, 탭 등)
  - `CssBuilder`를 통한 테마 CSS 생성
  - `SkinPreviewInfo` —— 초기화 페이지 스킨 선택기의 색상 팔레트 및 아이콘

- **내장 스킨** —— 7종의 프로덕션 준비 스킨:
  - **Admin** —— 전문적, 데이터 중심의 시스템 관리 인터페이스
  - **Chat** —— 대화형, 메시지 중심의 AI 상호작용 디자인
  - **Creative** —— 예술적, 시각적으로 풍부한 크리에이티브 워크플로 레이아웃
  - **Dev** —— 개발자 중심, 코드 중심 인터페이스, 구문 강조 포함
  - **HighContrast** —— 고대비 접근성 테마
  - **Light** —— 산뜻한 라이트 테마
  - **Minimal** —— 미니멀리즘 테마

- **스킨 발견** —— `SkinManager`가 리플렉션을 통해 모든 `ISkin` 구현을 자동 발견 및 등록

### HTML / CSS / JS 빌더

Web UI는 템플릿 파일을 완전히 배제하고, C#에서 모든 마크업을 생성합니다:

- **`H`** —— 코드에서 HTML 트리를 구축하는 플루언트 HTML 빌더 DSL
- **`CssBuilder`** —— 선택자 및 미디어 쿼리를 지원하는 CSS 빌더
- **`JsBuilder`(`JsSyntax`)** —— 인라인 스크립트용 JavaScript 빌더

### 컨트롤러 시스템

Web UI는 **MVC 유사 패턴**을 따르며, 24개의 컨트롤러가 다양한 측면을 처리합니다:

| 컨트롤러 | 용도 |
|------------|---------|
| About | 소개 페이지 및 프로젝트 정보 |
| Audit | 토큰 사용 감사 대시보드 |
| Being | 실리콘 비잉 관리 및 상태 |
| Chat | SSE가 포함된 실시간 채팅 인터페이스 |
| ChatHistory | 채팅 기록 보기, 세션 목록 및 메시지 상세 지원 |
| CodeBrowser | 코드 보기 및 편집 |
| CodeHover | 구문 강조가 포함된 코드 호버 팁 |
| Config | 시스템 설정 관리 |
| Dashboard | 시스템 개요 및 지표 |
| Executor | 이그제큐터 상태 및 관리 |
| Help | 도움말 문서 시스템, 다국어 지원 |
| Init | 최초 실행 초기화 마법사 |
| Knowledge | 지식 그래프 시각화 및 쿼리 |
| Log | 시스템 로그 뷰어, 실리콘 비잉 필터링 지원 |
| Memory | 장기 기억 브라우저, 고급 필터링, 통계 및 상세 보기 지원 |
| Permission | 권한 관리 |
| PermissionRequest | 퍼미션 요청 큐 |
| Project | 프로젝트 관리, 워크 노트, 태스크 시스템 및 툴 권한 포함 |
| System | 시스템 관리 및 런타임 모니터링 |
| Task | 태스크 시스템 인터페이스 |
| Timer | 타이머 시스템 관리, 실행 기록 포함 |
| ToolPermission | 툴 권한 관리, 실리콘 비잉 및 프로젝트 수준 권한 설정 지원 |
| Usage | 토큰 사용 감사 대시보드, 트렌드 차트 및 내보내기 포함 |
| WorkNote | 워크 노트 관리, 검색 및 디렉토리 생성 지원 |

### 실시간 업데이트

- **SSE(서버 전송 이벤트)** —— `SSEHandler`를 통해 채팅 메시지, 비잉 상태 및 시스템 이벤트 업데이트 푸시
- **WebSocket 불필요** —— 대부분의 실시간 요구를 SSE로 충족하는 더 간단한 아키텍처
- **자동 재연결** —— 클라이언트 재연결 로직으로 탄력적 연결 구현

### 로컬라이제이션

시스템은 **34개 언어 변형**의 포괄적인 로컬라이제이션을 지원합니다:
- **중국어(6종)**: zh-CN(간체), zh-HK(번체), zh-SG(싱가포르), zh-MO(마카오), zh-TW(대만), zh-MY(말레이시아)
- **영어(10종)**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY
- **스페인어(2종)**: es-ES, es-MX
- **독일어(5종)**: de-DE, de-AT, de-CH, de-LU, de-LI
- **프랑스어(3종)**: fr-FR, fr-CA, fr-CH
- **기타(8종)**: ja-JP(일본어), ko-KR(한국어), cs-CZ(체코어), it-IT(이탈리아어), pl-PL(폴란드어), pt-PT(포르투갈어), pt-BR(브라질 포르투갈어), ru-RU(러시아어)

`DefaultConfigData.Language`를 통해 활성 언어 환경을 선택하고, `LocalizationManager`를 통해 해석합니다.

---

### WebView 브라우저 자동화 시스템(신규)

시스템은 **Playwright** 기반의 WebView 브라우저 자동화 기능을 통합합니다:

- **개체 격리**: 각 실리콘 비잉은 독립적인 브라우저 인스턴스, 쿠키 및 세션 스토리지를 가지며, 완전히 격리되어 상호 간섭이 없습니다.
- **헤드리스 모드**: 브라우저는 사용자에게 완전히 보이지 않는 헤드리스 모드에서 실행되며, 실리콘 비잉이 백그라운드에서 자율적으로 조작합니다.
- **WebViewBrowserTool**: 다음을 포함한 완전한 브라우저 조작 능력 제공:
  - 페이지 탐색, 클릭, 텍스트 입력, 페이지 콘텐츠 가져오기
  - JavaScript 실행, 스크린샷 획득, 요소 대기
  - 브라우저 상태 관리 및 리소스 정리
- **보안 제어**: 모든 브라우저 작업은 권한 검증 체인을 거쳐야 하며, 악의적인 웹 접근을 방지합니다.

### 노리지 네트워크 시스템(신규)

시스템은 **트리플렛 구조** 기반의 지식 그래프 시스템을 내장합니다:

- **지식 표현**: "주체-관계-객체" 트리플렛 구조 채용(예: Python-is_a-programming_language)
- **KnowledgeTool**: 지식의 전체 라이프사이클 관리 제공:
  - `add`/`query`/`update`/`delete` - 기본 CRUD 작업
  - `search` - 전문 검색 및 키워드 매칭
  - `get_path` - 두 개념 간의 연관 경로 발견
  - `validate` - 지식 완전성 검사
  - `stats` - 지식 네트워크 통계 분석
- **영속화 스토리지**: 지식 트리플렛을 파일 시스템에 영속화하며, 시간 인덱스 쿼리 지원.
- **신뢰도 점수**: 각 지식 항목에 신뢰도 점수(0-1)가 부여되어, 지식의 퍼지 매칭 및 정렬을 지원.
- **태그 분류**: 지식에 태그 추가를 지원하여 분류 및 검색이 용이.

---

## 데이터 디렉토리 구조

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # 큐레이터의 소울 파일
    │   ├── state.json       # 런타임 상태
    │   ├── code.enc         # AES 암호화된 커스텀 클래스 코드
    │   └── permission.enc   # AES 암호화된 커스텀 권한 콜백
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```

---

## SpeedyPack 스토리지 엔진

SiliconLife.Fast은 자체 개발한 SpeedyPack 스토리지 엔진(.spk 형식)을 사용하여, 기존 LiteDB 방식을 대체하고 극한의 읽기/쓰기 성능을 실현합니다.

### 아키텍처 설계

```
┌──────────────────────────────────────────────────────────┐
│                    SpeedyPack                             │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────────┐  │
│  │ DirectoryMap  │  │  EntryCache   │  │  WriteQueue   │  │
│  │ (메모리 디렉토리 맵) │  │  (엔트리 캐시)  │  │ (비동기 쓰기 큐) │  │
│  └──────┬───────┘  └──────┬───────┘  └───────┬───────┘  │
│         │                  │                   │          │
│  ┌──────▼──────────────────▼───────────────────▼───────┐  │
│  │              PackFileReader / PackFileWriter          │  │
│  │              (팩 파일 리더/라이터)                      │  │
│  └──────────────────────────┬──────────────────────────┘  │
│                              │                             │
│  ┌──────────────────────────▼──────────────────────────┐  │
│  │              .spk 파일 (MessagePack + LZ4 압축)       │  │
│  └─────────────────────────────────────────────────────┘  │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐                      │
│  │  FreeList     │  │ SpeedyPack   │                      │
│  │ (프리 리스트)  │  │ AutoCompactor│                      │
│  │              │  │ (자동 컴팩터) │                      │
│  └──────────────┘  └──────────────┘                      │
└──────────────────────────────────────────────────────────┘
```

### 핵심 컴포넌트

| 컴포넌트 | 설명 |
|------|------|
| `SpeedyPack` | 핵심 클래스, DirectoryMap, EntryCache 및 WriteQueue를 조합하여 저지연 읽기/쓰기 제공 |
| `DirectoryMap` | 메모리 디렉토리 맵, 가상 경로에서 파일 엔트리로의 매핑 관계 유지 |
| `EntryCache` | 엔트리 캐시, TTL 기반의 최근 액세스 엔트리 캐시 |
| `WriteQueue` | 비동기 쓰기 큐, 쓰기 작업을 백그라운드 스레드에서 실행하도록 큐잉 |
| `FreeList` | 프리 리스트, .spk 파일 내의 재사용 가능한 공간 추적 |
| `PackFileReader` | 팩 파일 리더, .spk 파일에서 데이터 읽기 |
| `PackFileWriter` | 팩 파일 라이터, .spk 파일에 데이터 쓰기 |
| `SpeedyPackAutoCompactor` | 자동 컴팩트 타이머, 정기적으로 .spk 파일을 컴팩트하여 프리 공간 회수 |
| `SpeedyPackRegistry` | 프로세스 수준 싱글톤 매니저, 전체 애플리케이션이 동일한 SpeedyPack 인스턴스를 사용하도록 보장 |

### 스토리지 어댑터

SiliconLife.Fast은 다음 어댑터를 통해 SpeedyPack을 시스템 인터페이스에 통합합니다:

| 어댑터 | 인터페이스 | 설명 |
|--------|------|------|
| `SpeedyStorage` | `IStorage` | 범용 키-값 스토리지 어댑터 |
| `SpeedyTimeStorage` | `ITimeStorage` | 타임 인덱스 스토리지 어댑터 |
| `SpeedyWorkNoteStorage` | `IWorkNoteStorage` | 워크 노트 스토리지 어댑터 |

### 설정 옵션

`SpeedyPackOptions`는 다음 설정을 제공합니다:

| 옵션 | 타입 | 기본값 | 설명 |
|------|------|--------|------|
| `CacheTtl` | `TimeSpan` | 5분 | 캐시 엔트리의 생존 시간 |
| `MaxCacheEntries` | `int` | 1000 | 최대 캐시 엔트리 수 |
| `ReadOnly` | `bool` | false | 읽기 전용 모드 |

### 트랜잭션 지원

SpeedyPack은 `IPackTransaction` 인터페이스를 통해 원자적 쓰기 작업을 지원합니다:

- `SpeedyTransaction`이 트랜잭션 메커니즘을 구현
- 배치 쓰기의 원자성 지원
- 트랜잭션 커밋 시 모든 쓰기 작업이 전부 성공하거나 전부 롤백

---

## 플러그인 시스템

SiliconLife는 플러그인 시스템을 통해 기능 확장을 지원하며, 서드파티 개발자가 플랫폼에 새로운 기능을 추가할 수 있습니다.

### 핵심 인터페이스

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### 플러그인 로더

`PluginLoader`는 지정된 디렉토리에서 플러그인 DLL을 로드하고 엄격한 보안 검사를 수행합니다:

1. **디렉토리 스캔** — 플러그인 디렉토리의 모든 .dll 파일 스캔
2. **보안 스캔** — 플러그인이 금지된 네임스페이스를 참조하는지 검사
3. **격리 로드** — 커스텀 `AssemblyLoadContext`를 사용하여 플러그인을 격리하여 로드
4. **라이프사이클 관리** — 플러그인의 OnLoad, OnStart, OnStop, OnUnload 메서드 호출

### 보안 샌드박스

플러그인 로더는 다음 보안 검사를 수행합니다:

| 검사 항목 | 설명 |
|--------|------|
| 금지된 네임스페이스 | System.IO, System.Net.Http, System.Net.WebSockets, System.Net.Sockets, Microsoft.CodeAnalysis |
| 신뢰할 수 있는 어셈블리 화이트리스트 | Google.Protobuf, Newtonsoft.Json, MessagePack, Serilog, Microsoft.Extensions.Logging.Abstractions, Dapper |
| 금지된 타입 검사 | 플러그인에서 참조하는 위험한 타입 스캔 |
| 금지된 멤버 검사 | 플러그인에서 호출하는 위험한 메서드 스캔 |

### 툴 통합

플러그인은 `ITool` 인터페이스를 구현하여 커스텀 툴을 등록할 수 있습니다:

- `ToolManager.ScanAllPluginAssemblies()` 메서드가 로드된 모든 플러그인에서 ITool 구현을 스캔
- 플러그인 툴이 툴 콜 루프에 자동 통합
- 플러그인 툴은 동일한 권한 시스템의 제약을 받음

### 플러그인 라이프사이클

```
로드(OnLoad) → 시작(OnStart) → 실행 중 → 중지(OnStop) → 언로드(OnUnload)
```

---

## 실리콘 비잉 활동 상태

실리콘 비잉은 다음과 같은 활동 상태를 가집니다:

| 상태 | 설명 |
|------|------|
| `Idle` | 대기 상태, 클록 트리거 대기 |
| `SingleChat` | 1:1 채팅 진행 중 |
| `GroupChat` | 그룹 채팅 진행 중 |
| `Task` | 태스크 실행 중 |
| `Timer` | 타이머 실행 중 |
| `Stopped` | 정지됨, 연속 오류 또는 수동 중지로 인해 |

**Stopped 상태 메커니즘**:
- 실리콘 비잉이 연속 10회 오류 발생 시, 자동으로 `Stopped` 상태로 전환
- Stopped 상태 진입 후, 비잉은 더 이상 어떤 태스크도 실행하지 않음
- 새로운 채팅 메시지가 도착하면 오류 카운터가 리셋되고, 비잉이 다시 실행됨

상태 전환:
```
Idle → SingleChat → Idle（채팅 완료）
Idle → GroupChat → Idle（그룹 채팅 완료）
Idle → Task → Idle（태스크 완료）
Idle → Timer → Idle（타이머 완료）
임의 → Stopped（연속 10회 오류）
Stopped → Idle（새 채팅 메시지 도착 또는 수동 재시작）
```

---

## 워크플로 엔진

워크플로 엔진은 템플릿 기반 상태 머신 시스템으로, 실리콘 비잉이 프로젝트 공간에서의 협업 프로세스를 구동합니다:

### 핵심 컴포넌트

| 컴포넌트 | 설명 |
|------|------|
| `WorkflowEngine` | 워크플로 엔진 코어, 템플릿 및 인스턴스 관리, Tick 구동 상태 전환 실행 |
| `WorkflowTemplate` | 워크플로 템플릿, 상태 집합 및 전환 규칙 정의 |
| `WorkflowInstance` | 워크플로 인스턴스, 특정 프로젝트에 바인딩, 현재 상태 추적 |
| `WorkflowLog` | 워크플로 로그, 상태 전환 기록 |

### 작동 메커니즘

- **템플릿 등록**: `RegisterTemplate()`으로 워크플로 템플릿을 등록하고, 상태 및 전환 규칙 정의
- **인스턴스 생성**: 템플릿에서 인스턴스를 생성하여 프로젝트 공간에 바인딩
- **Tick 구동**: 상태 전환은 메인 루프의 Tick 메커니즘에 의해 구동
- **로그 기록**: 모든 상태 전환이 자동으로 로그에 기록

---

## 기억 페이드 메커니즘

`MemoryFadeService`는 생물학적 기억의 망각 특성을 시뮬레이션하는 정기 감쇠 서비스입니다:

### 작동 메커니즘

- **정기 실행**: `TickObject`에서 상속, 기본적으로 매시간 한 번 감쇠 주기 실행
- **중요도 감쇠**: 각 실리콘 비잉의 기억 항목에 감쇠 알고리즘을 적용하여 중요도 점수 저하
- **자동 보관**: 중요도가 임계값 미만인 기억 자동 보관(`ArchiveFadingMemories()`)
- **통계 추적**: 감쇠 주기 수, 상태 변경 항목 수 등의 통계 데이터 기록

### 감쇠 흐름

```
MemoryFadeService.OnTick()
  └── 모든 실리콘 비잉 순회
       └── being.Memory.ApplyDecay()      # 중요도 감쇠 적용
       └── being.Memory.ArchiveFadingMemories()  # 저중요도 기억 보관
```

---

## 프로젝트 워크스페이스 시스템

프로젝트 워크스페이스는 다중 실리콘 비잉 협업을 지원하는 공간 관리 메커니즘입니다:

### 핵심 기능

- **프로젝트 라이프사이클**: 생성 → 활성 → 보관 → 파기
- **역할 할당**: 실리콘 비잉에 프로젝트 역할 할당 지원
- **툴 권한 격리**: 프로젝트 수준의 툴 권한 설정, 실리콘 비잉 수준 권한과 독립적
- **워크 노트**: 프로젝트 공간 내 페이지형 노트 시스템, 디렉토리 생성 및 키워드 검색 지원
- **태스크 추적**: 프로젝트 수준의 태스크 관리, 생성, 할당, 상태 추적 지원
- **워크플로 통합**: 프로젝트에 워크플로 템플릿을 바인딩하여 협업 프로세스 구동

### 관련 툴

| 툴 | 용도 |
|------|------|
| `ProjectTool` | 프로젝트 공간 관리(생성, 보관, 파기, 역할 할당) |
| `ProjectTaskTool` | 프로젝트 태스크 관리(생성, 할당, 상태 업데이트) |
| `ProjectWorkNoteTool` | 프로젝트 워크 노트(생성, 검색, 디렉토리 생성) |
| `ProjectWorkTool` | 프로젝트 작업(태스크 생성, 그룹 채팅, 브로드캐스트, 프로젝트 완료) |

---

## 스킬 시스템

스킬(Skill)은 "툴 오케스트레이션 + 프롬프트 템플릿"의 재사용 가능한 추상화 계층으로, 일반적인 워크플로우를 선언 가능하고, 진화 가능하며, 스케줄링 가능한 능력 단위로 캡슐화합니다.

### 계층 구조

| 계층 | 위치 | 역할 |
|------|------|------|
| 코어 계층 | `SiliconLife.Core/Skills/` | SkillDefinition, SkillManager(등록+실행 엔진), SkillMarkdownParser, SkillFileManager, AutoSkillTickObject, SkillMetadataCompleter |
| 공통 계층 | `SiliconLife.Common` | BuiltinSkills(3개 내장 스킬), SkillTool(`skill` 툴) |
| 애플리케이션 계층 | `SiliconLife.App/Web/` | SkillController + SkillView(스킬 관리 페이지) |

### 실행 흐름

```
AI 함수 호출(스킬 id) 또는 스케줄러 트리거
        ↓
SkillManager.ExecuteSkill
  ├─ 글로벌 스위치 / 권한 / 재귀 방호 검사
  ├─ 매개변수 클램핑: maxToolRound = Min(스킬값, GlobalMaxToolRound)
  │            timeout = Min(스킬값, GlobalSkillTimeoutSeconds)
  ├─ MergePermissions: 비잉 권한 ∪ 스킬 제한(엄격한 측이 승리)
  ├─ FillTemplate: {param} 자리표시자 채우기 → 하위 AIRequest
  └─ 하위 루프(최대 maxToolRound 라운드): AI ↔ 툴(화이트리스트 내만)
        ↓
HandleCompletion(OnCompleteAction)
  none / write_memory / notify_curator / broadcast
```

### 핵심 설계

- **투명 스케줄링**: 스킬이 `ToolDefinition` 형태로 `AIRequest.Tools`에 주입되며, AI는 인지하지 못함; `ContextManager.ExecuteToolCalls`에서 스킬 호출이 동일한 이름의 툴보다 우선
- **네 가지 소스**: `Builtin`(프레임워크) / `Plugin`(ISkillProvider) / `Being`(비잉 런타임) / `User`(Web UI), 핫 리로드 시 전자 두 종류는 유지하고 후자 두 종류는 교체
- **Markdown 우선**: `skills/{id}.md`(YAML 프론트 + 본문)이 `.json`보다 우선; 순수 Markdown 저장 시 AI가 메타데이터를 보완(사용자 필드는 덮어쓰지 않음)
- **자동 스케줄링**: `AutoSkillTickObject`(30초 검사 간격)가 `HH:mm`, `N s|m|h|d`, cron 서브셋 세 가지 스케줄 표현식을 지원하며, 재진입 방지 보호 내장
- **다중 가드레일**: 글로벌 스위치, 커스텀 할당량(`MaxCustomSkillsPerBeing`, 기본값 50), 글로벌 라운드/타임아웃 상한, 스킬 수준 `execute` 동작 권한, 툴 화이트리스트, 재귀 방호

---

## MCP 통합

MCP(Model Context Protocol) 통합을 통해 실리콘 비잉이 외부 MCP 서버가 제공하는 툴을 호출할 수 있으며, 코드 작성 없이 능력 경계를 확장할 수 있습니다.

### 아키텍처

```
사용자(Web UI /mcp) ──추가/활성화·비활성화/삭제──→ McpManager(싱글톤)
                                           │
                               ┌───────────┼───────────┐
                               ↓           ↓           ↓
                         McpClientConnection × N(stdio / http)
                               │
                               └→ ListTools → SiliconLife.Collective.McpTool로 래핑
                                             명명 mcp_{serverId}_{toolName}
                                                   │
                           McpManager.SyncToolsForBeing(being) 주입
                                                   ↓
                                     ToolManager(내장 툴과 동등한 대우)
```

### 핵심 설계

- **듀얼 전송**: `stdio`(로컬 서브프로세스: command + arguments + env)와 `http`(원격 엔드포인트)
- **툴 명명 격리**: `mcp_{serverId}_{toolName}` 접두사로 내장/플러그인 툴과의 충돌 방지
- **사용자 주권**: 서버 추가/삭제/활성화/비활성화는 Web UI를 통해서만 가능, AI 측 `mcp` 툴은 읽기 전용 쿼리만 제공(status/list_servers/list_tools)
- **권한 일관성**: 래핑된 툴은 단일 `execute` 동작을 자동 선언하며, 툴 동작 권한 매트릭스에 편입되어 비잉/프로젝트별로 비활성화 가능
- **설정 영속화**: `McpServers` 목록은 config.json에 저장, `McpEnabled` 글로벌 스위치

---

## IM 플랫폼 다중 인스턴스 아키텍처

IM 플랫폼은 "다중 인스턴스 설정 + 집계 프로바이더" 아키텍처를 채택하여, 여러 채팅 플랫폼을 동시에 연동할 수 있습니다.

### 핵심 컴포넌트

| 컴포넌트 | 역할 |
|------|------|
| `IMPlatformConfig` | 단일 인스턴스 설정(platform/enabled/config 사전), `IMPlatforms`는 목록이며 각 인스턴스가 독립적으로 활성화/비활성화 |
| `IMProviderRegistry` | 플랫폼 메타데이터 레지스트리: 설정 필드 schema, OAuth 엔드포인트 템플릿, Provider 팩토리, 도움말 링크 |
| `AggregateIMProvider` | 다중 플랫폼 집계: 메시지 수신(임의의 플랫폼 트리거), 메시지 송신(브로드캐스트, 단일 플랫폼 실패 시 자동 격리), 권한 문의(첫 응답자 승리 경쟁) |
| `ImOAuthService` | OAuth 인증 마법사(싱글톤): state CSRF 방지, 5분 타임아웃, 토큰 설정에 기록, SSE 상태 푸시 |
| `ConfigSecretResolver` | `${ENV_VAR}` 자리표시자 해석: 깊은 복사 교체, 평문 비밀키는 config.json에 기록하지 않음 |
| `IMManager` | 메시지 라우팅: ChannelId별 큐잉(직렬 처리) → ChatSystem → 실리콘 비잉 사고 트리거 |

### 지원 플랫폼

| 플랫폼 | AuthModes | 이벤트 연동 | 비고 |
|------|-----------|---------|------|
| Web UI | manual | SSE(내장) | 항상 사용 가능, 자동 보완 |
| Feishu | manual / **oauth** | HTTP 콜백(서명 검증 + AES 복호화) | 원클릭 OAuth 인증 마법사 지원 |
| WeChat Enterprise | manual | HTTP 콜백(WXBizMsgCrypt) | 공용망 콜백 필요 |
| DingTalk | manual | Stream(WebSocket) / HTTP | 기본 Stream 모드, 공용망 불필요 |

### 메시지 흐름

```
Feishu/WeChat Enterprise/DingTalk/WebUI(인바운드)
  → IIMProvider.MessageReceived
  → IMManager.OnMessageReceived(ChannelId별 큐잉, 직렬 처리)
  → ChatSystem.AddMessage → 실리콘 비잉 AI 사고
  → IMManager.SendMessageAsync / SendStreamChunkAsync(아웃바운드)
  → AggregateIMProvider가 모든 활성화된 플랫폼에 브로드캐스트
```
