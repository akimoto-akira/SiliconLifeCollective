# 아키텍처

> **버전: v0.1.0-alpha**

[English](../en/architecture.md) | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | **한국어** | [Deutsch](../de-DE/architecture.md) | [Čeština](../cs-CZ/architecture.md)

## 핵심 개념

### 실리콘 생명체

시스템의 각 AI 에이전트는 **실리콘 생명체**입니다 — 고유한 정체성, 개성 및 능력을 갖춘 자율 엔티티. 각 실리콘 생명체는 **소울 파일**(Markdown 프롬프트)에 의해 구동되며, 행동 패턴을 정의합니다.

### 실리콘 큐레이터

**실리콘 큐레이터**는 최고 시스템 권한을 가진 특별한 실리콘 생명체입니다. 시스템 관리자 역할을 합니다:

- 다른 실리콘 생명체 생성 및 관리
- 사용자 요청을 분석하여 작업으로 분해
- 적절한 실리콘 생명체에 작업 할당
- 실행 품질 모니터링 및 실패 처리
- **우선 스케줄링**으로 사용자 메시지 응답 (아래 참조)

### 소울 파일

각 실리콘 생명체 데이터 디렉토리에 저장되는 Markdown 파일(`soul.md`). 시스템 프롬프트로 각 AI 요청에 삽입되어 생명체의 개성, 의사결정 패턴 및 행동 제약을 정의합니다.

---

## 스케줄링: 타임슬롯 공정 스케줄링

### 메인 루프 + 클록 객체

시스템은 전용 백그라운드 스레드에서 **클록 기반 메인 루프**를 실행합니다:

```
메인 루프 (전용 스레드, 워치독 + 서킷 브레이커)
  └── 클록 객체 A (우선순위=0, 간격=100ms)
  └── 클록 객체 B (우선순위=1, 간격=500ms)
  └── 실리콘 생명체 관리자 (메인 루프에서 직접 클록 트리거)
        └── 실리콘 생명체 러너 → 실리콘 생명체 1 → 클록 트리거 → 1라운드 실행
        └── 실리콘 생명체 러너 → 실리콘 생명체 2 → 클록 트리거 → 1라운드 실행
        └── 실리콘 생명체 러너 → 실리콘 생명체 3 → 클록 트리거 → 1라운드 실행
        └── ...
```

핵심 설계 결정:

- **실리콘 생명체는 클록 객체를 상속하지 않습니다.** 자체 `Tick()` 메서드를 가지며, `SiliconBeingManager`가 `SiliconBeingRunner`를 통해 호출합니다. 메인 루프에 직접 등록되지 않습니다.
- **실리콘 생명체 관리자**는 메인 루프에서 직접 클록 트리거되며, 모든 생명체의 단일 에이전트 역할을 합니다.
- **실리콘 생명체 러너**는 각 생명체의 `Tick()`을 임시 스레드에서 래핑하며, 타임아웃 및 생명체별 서킷 브레이커(연속 3회 타임아웃 → 1분 쿨다운)를 가집니다.
- 각 생명체의 실행은 클록 트리거당 **1라운드** AI 요청 + 도구 호출로 제한되어, 어떤 생명체도 메인 루프를 독점할 수 없습니다.
- **성능 모니터**는 관찰 가능성을 위해 클록 실행 시간을 추적합니다.

### 큐레이터 우선 응답

사용자가 실리콘 큐레이터에게 메시지를 보낼 때:

1. 현재 생명체(예: 생명체 A)가 현재 라운드를 완료합니다 — **중단 없음**.
2. 관리자가 **남은 큐리를 건너뜁니다**.
3. 루프가 **큐레이터에서 다시 시작**되어 즉시 실행됩니다.

이로써 사용자 응답성을 보장하면서도 진행 중인 작업을 방해하지 않습니다.

---

## 컴포넌트 아키텍처

```
┌─────────────────────────────────────────────────────────┐
│                        코어 호스트                       │
│  (통합 호스트 — 모든 컴포넌트 조립 및 관리)                │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ 메인 루프 │  │ 서비스 로케이터│  │      설정         │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │      실리콘 생명체 관리자 (클록 객체)               │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │큐레이터  │ │생명체 A  │ │생명체 B  │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              공유 서비스                            │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │채팅 시스템│  │ 스토리지  │  │  권한 관리자       │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │ AI 클라이언트│  │실행기    │  │   도구 관리자      │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  실행기                            │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │  디스크   │  │ 네트워크  │  │  명령줄          │  │   │
│  │  │실행기     │  │실행기     │  │  실행기          │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              인스턴트 메시징 제공자                  │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ 콘솔     │  │  Web     │  │  Feishu / ...    │  │   │
│  │  │제공자     │  │제공자     │  │  제공자          │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## 서비스 로케이터

`ServiceLocator`는 모든 코어 서비스에 대한 접근을 제공하는 스레드 안전 싱글톤 레지스트리입니다:

| 속성 | 타입 | 설명 |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | 중앙 채팅 세션 관리자 |
| `IMManager` | `IMManager` | 인스턴트 메시징 제공자 라우터 |
| `AuditLogger` | `AuditLogger` | 권한 감사 추적 |
| `GlobalAcl` | `GlobalACL` | 글로벌 접근 제어 목록 |
| `BeingFactory` | `ISiliconBeingFactory` | 생명체 생성 팩토리 |
| `BeingManager` | `SiliconBeingManager` | 활성 생명체 수명주기 관리자 |
| `DynamicBeingLoader` | `DynamicBeingLoader` | 동적 컴파일 로더 |
| `TokenUsageAudit` | `ITokenUsageAudit` | Token 사용 추적 |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Token 사용 보고 |

또한 생명체 GUID를 키로 사용하여 각 생명체의 `PermissionManager` 레지스트리를 유지합니다.

---

## 채팅 시스템

### 세션 타입

채팅 시스템은 `SessionBase`를 통해 세 가지 세션 타입을 지원합니다:

| 타입 | 클래스 | 설명 |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | 두 참가자 간 1:1 대화 |
| `GroupChat` | `GroupChatSession` | 다중 참가자 그룹 채팅 |
| `Broadcast` | `BroadcastChannel` | 고정 ID를 가진 개방형 채널; 생명체가 동적으로 구독하며, 구독 후 메시지 수신 |

### 브로드캐스트 채널

`BroadcastChannel`은 시스템 전체 공지용 특수 세션 타입입니다:

- **고정 채널 ID** — `SingleChatSession` 및 `GroupChatSession`과 달리, 채널 ID는 멤버 GUID에서 파생되지 않는 잘 알려진 상수입니다.
- **동적 구독** — 생명체가 런타임에 구독/구독 취소; 구독 후 게시된 메시지만 수신합니다.
- **대기 메시지 필터링** — `GetPendingMessages()`는 생명체 구독 시간 이후에 게시되고 아직 읽지 않은 메시지만 반환합니다.
- **채팅 시스템 관리** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### 채팅 메시지

`ChatMessage` 모델은 AI 대화 컨텍스트 및 token 추적을 위한 필드를 포함합니다:

| 필드 | 타입 | 설명 |
|-------|------|-------------|
| `Id` | `Guid` | 고유 메시지 식별자 |
| `SenderId` | `Guid` | 발신자 고유 식별자 |
| `ChannelId` | `Guid` | 채널/대화 식별자 |
| `Content` | `string` | 메시지 내용 |
| `Timestamp` | `DateTime` | 메시지 전송 시간 |
| `Type` | `MessageType` | 텍스트, 이미지, 파일 또는 시스템 알림 |
| `ReadBy` | `List<Guid>` | 이 메시지를 읽은 참가자 ID |
| `Role` | `MessageRole` | AI 대화 역할 (사용자, 어시스턴트, 도구) |
| `ToolCallId` | `string?` | 도구 결과 메시지의 도구 호출 ID |
| `ToolCallsJson` | `string?` | 어시스턴트 메시지의 직렬화된 도구 호출 JSON |
| `Thinking` | `string?` | AI의 사고 연쇄 추론 |
| `PromptTokens` | `int?` | 프롬프트의 token 수 (입력) |
| `CompletionTokens` | `int?` | 완성의 token 수 (출력) |
| `TotalTokens` | `int?` | 사용된 총 token 수 (입력 + 출력) |
| `FileMetadata` | `FileMetadata?` | 첨부된 파일 메타데이터 (메시지에 파일 포함 시) |

### 채팅 메시지 큐

`ChatMessageQueue`는 채팅 메시지 비동기 처리를 관리하는 스레드 안전 메시지 큐 시스템입니다:

- **스레드 안전** - 락 메커니즘으로 동시 접근 안전 보장
- **비동기 처리** - 비동기 메시지 인큐 및 디큐 지원
- **메시지 정렬** - 메시지 시간 순서 유지
- **일괄 작업** - 메시지 일괄 가져오기 지원

### 파일 메타데이터

`FileMetadata`는 채팅 메시지에 첨부된 파일 정보를 관리합니다:

- **파일 정보** - 파일명, 크기, 타입, 경로
- **업로드 시간** - 파일 업로드 타임스탬프
- **업로더** - 파일 업로드한 사용자 또는 실리콘 생명체 ID

### 스트림 취소 관리자

`StreamCancellationManager`는 AI 스트리밍 응답의 취소 메커니즘을 제공합니다:

- **스트림 제어** - 진행 중인 AI 스트리밍 응답 취소 지원
- **리소스 정리** - 취소 시 관련 리소스 올바르게 정리
- **동시 안전** - 여러 스트림 동시 관리 지원

### 채팅 기록 보기

새로운 채팅 기록 보기 기능으로 사용자가 실리콘 생명체의 과거 대화를 탐색할 수 있습니다:

- **세션 목록** - 모든 과거 세션 표시
- **메시지 세부정보** - 전체 메시지 기록 보기
- **타임라인 뷰** - 시간순으로 메시지 표시
- **API 지원** - 세션 및 메시지 데이터 가져오기 위한 RESTful API 제공

---

## AI 클라이언트 시스템

시스템은 `IAIClient` 인터페이스를 통해 여러 AI 백엔드를 지원합니다:

### OllamaClient

- **타입**: 로컬 AI 서비스
- **프로토콜**: 네이티브 Ollama HTTP API (`/api/chat`, `/api/generate`)
- **기능**: 스트리밍, 도구 호출, 로컬 모델 호스팅
- **설정**: `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (알리바바 클라우드 Bailian)

- **타입**: 클라우드 AI 서비스
- **프로토콜**: OpenAI 호환 API (`/compatible-mode/v1/chat/completions`)
- **인증**: Bearer token (API 키)
- **기능**: 스트리밍, 도구 호출, 추론 콘텐츠 (사고 연쇄), 다중 지역 배포
- **지원 지역**:
  - `beijing` — 화북2 (베이징)
  - `virginia` — 미국 (버지니아)
  - `singapore` — 싱가포르
  - `hongkong` — 중국 홍콩
  - `frankfurt` — 독일 (프랑크푸르트)
- **지원 모델** (API를 통한 동적 발견, 폴백 목록 포함):
  - **통이치엔원 시리즈**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **추론**: qwq-plus
  - **서드파티**: deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **설정**: `apiKey`, `region`, `model`
- **모델 발견**: 런타임에 Bailian API에서 사용 가능한 모델 가져오기; 네트워크 장애 시 선별된 목록으로 폴백

### 클라이언트 팩토리 패턴

각 AI 클라이언트 타입은 `IAIClientFactory`를 구현하는 해당 팩토리를 가집니다:

- `OllamaClientFactory` — OllamaClient 인스턴스 생성
- `DashScopeClientFactory` — DashScopeClient 인스턴스 생성

팩토리 제공:
- `CreateClient(Dictionary<string, object> config)` — 설정에서 클라이언트 인스턴스화
- `GetConfigKeyOptions(string key, ...)` — 설정 키의 동적 옵션 반환 (예: 사용 가능한 모델, 지역)
- `GetDisplayName()` — 클라이언트 타입의 지역화된 표시 이름

### AI 플랫폼 지원 목록

#### 상태 설명
- ✅ 구현 완료
- 🚧 개발 중
- 📋 계획 중
- 💡 고려 중

*참고: 개발자의 네트워크 환경에 따라, [고려 중]인 해외 클라우드 AI 서비스 접속에는 네트워크 프록시 도구가 필요할 수 있으며, 디버깅 과정이 불안정할 수 있습니다.*

#### 플랫폼 목록

| 플랫폼 | 상태 | 타입 | 설명 |
|------|------|------|------|
| Ollama | ✅ | 로컬 | 로컬 AI 서비스, 로컬 모델 배포 지원 |
| DashScope (알리바바 클라우드 Bailian) | ✅ | 클라우드 | 알리바바 클라우드 Bailian AI 서비스, 다중 지역 배포 지원 |
| 바이두 치엔판 (원심일언) | 📋 | 클라우드 | 바이두 원심일언 AI 서비스 |
| 즈푸 AI (GLM) | 📋 | 클라우드 | 즈푸칭옌 AI 서비스 |
| 위즈안몐 (Kimi) | 📋 | 클라우드 | 위즈안몐 Kimi AI 서비스 |
| 훠산팡저우엔진.더우바오 | 📋 | 클라우드 | 바이트댄스 더우바오 AI 서비스 |
| DeepSeek (직접 연결) | 📋 | 클라우드 | 셴두추구소 AI 서비스 |
| 링이완무 | 📋 | 클라우드 | 링이완무 AI 서비스 |
| 텅쉰 혼위안 | 📋 | 클라우드 | 텅쉰 혼위안 AI 서비스 |
| 구이지리우둥 | 📋 | 클라우드 | 구이지리우둥 AI 서비스 |
| MiniMax | 📋 | 클라우드 | MiniMax AI 서비스 |
| OpenAI | 💡 | 클라우드 | OpenAI API 서비스 (GPT 시리즈) |
| Anthropic | 💡 | 클라우드 | Anthropic Claude AI 서비스 |
| Google DeepMind | 💡 | 클라우드 | Google Gemini AI 서비스 |
| Mistral AI | 💡 | 클라우드 | Mistral AI 서비스 |
| Groq | 💡 | 클라우드 | Groq 고속 AI 추론 서비스 |
| Together AI | 💡 | 클라우드 | Together AI 오픈소스 모델 서비스 |
| xAI | 💡 | 클라우드 | xAI Grok 서비스 |
| Cohere | 💡 | 클라우드 | Cohere 기업용 NLP 서비스 |
| Replicate | 💡 | 클라우드 | Replicate 오픈소스 모델 호스팅 플랫폼 |
| Hugging Face | 💡 | 클라우드 | Hugging Face 오픈소스 AI 커뮤니티 및 모델 플랫폼 |
| Cerebras | 💡 | 클라우드 | Cerebras AI 추론 최적화 서비스 |
| Databricks | 💡 | 클라우드 | Databricks 기업용 AI 플랫폼 (MosaicML) |
| Perplexity AI | 💡 | 클라우드 | Perplexity AI 검색 질의응답 서비스 |
| NVIDIA NIM | 💡 | 클라우드 | NVIDIA AI 추론 마이크로서비스 |

---

## 핵심 설계 결정

### 인스턴스 클래스로서의 스토리지 (정적 아님)

`IStorage`는 정적 유틸리티가 아닌 주입 가능한 인스턴스로 설계되었습니다. 이로써:

- 직접 파일 시스템 접근 — IStorage는 시스템의 내부 영속화 채널이며, 실행기를 통해 라우팅되지 **않습니다**.
- **AI가 IStorage를 제어할 수 없음** — 실행기는 AI 도구가 시작한 IO를 관리; IStorage는 프레임워크 자체의 내부 데이터 읽기/쓰기를 관리합니다. 이들은 근본적으로 다른 관심사입니다.
- 목업 구현으로 테스트 가능.
- 소비자 수정 없이 미래에 다른 스토리지 백엔드 지원 가능.

### 안전 경계로서의 실행기

실행기는 I/O 작업의 **유일** 경로입니다. 디스크, 네트워크 또는 명령줄 접근이 필요한 도구는 **반드시** 실행기를 통해야 합니다. 이 설계는 다음을 강제합니다:

- 각 실행기는 권한 검증을 위한 스레드 락이 있는 **독립 스케줄링 스레드**를 가집니다.
- 중앙 집중식 권한 검사 — 실행기는 생명체의 **전용 권한 관리자**에 쿼리합니다.
- 우선순위 및 타임아웃 제어를 지원하는 요청 큐.
- 모든 외부 작업의 감사 로그.
- 예외 격리 — 한 실행기의 실패가 다른 실행기에 영향을 미치지 않음.
- 서킷 브레이커 — 연속 실패 시 실행기 일시 중지로 연쇄 실패 방지.

### 경량 객체로서의 ContextManager

각 `ExecuteOneRound()`는 새 `ContextManager` 인스턴스를 생성합니다:

1. 소울 파일 + 최근 채팅 기록 로드.
2. AI 클라이언트에 요청 전송.
3. AI가 순수 텍스트를 반환할 때까지 도구 호출 처리 루프.
4. 응답을 채팅 시스템에 영속화.
5. 해제.

이로써 각 라운드를 격리되고 무상태로 유지합니다.

### 클래스 재작성을 통한 자가 진화

실리콘 생명체는 런타임에 자체 C# 클래스를 재작성할 수 있습니다:

1. AI가 새 클래스 코드 생성 (`SiliconBeingBase` 상속 필수).
2. **컴파일 타임 참조 제어** (주요 방어): 컴파일러는 허용된 어셈블리 목록만 받음 — `System.IO`, `System.Reflection` 등은 제외되므로, 위험한 코드는 타입 레벨에서 불가능합니다.
3. **런타임 정적 분석** (보조 방어): `SecurityScanner`가 성공적인 컴파일 후 코드에서 위험한 패턴 스캔.
4. Roslyn이 메모리에서 코드 컴파일.
5. 성공 시: `SiliconBeingManager.ReplaceBeing()`이 현재 인스턴스를 교환하고, 상태를 마이그레이션하며, 암호화된 코드를 디스크에 영속화.
6. 실패 시: 새 코드 폐기, 기존 구현 유지.

맞춤형 `IPermissionCallback` 구현도 `ReplacePermissionCallback()`을 통해 컴파일 및 주입될 수 있어, 생명체가 자체 권한 로직을 맞춤화할 수 있습니다.

코드는 디스크에 AES-256으로 암호화되어 저장됩니다. 암호화 키는 생명체 GUID(대문자)에서 PBKDF2를 통해 파생됩니다.

---

## Token 사용 감사

`TokenUsageAuditManager`는 모든 생명체의 AI token 소모를 추적합니다:

- `TokenUsageRecord` — 각 요청의 기록 (생명체 ID, 모델, 프롬프트 token, 완성 token, 타임스탬프)
- `TokenUsageSummary` — 집계 통계
- `TokenUsageQuery` — 기록 필터링을 위한 쿼리 매개변수
- 시계열 쿼리를 위해 `ITimeStorage`를 통해 영속화
- Web UI (AuditController) 및 `TokenAuditTool` (큐레이터 전용)를 통해 접근 가능

---

### 캘린더 시스템

시스템은 추상 `CalendarBase` 클래스에서 파생된 **32개 캘린더 구현**을 포함하며, 세계 주요 캘린더 시스템을 커버합니다:

| 캘린더 | ID | 설명 |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | 불력 (BE), 년도 + 543 |
| CherokeeCalendar | `cherokee` | 체로키 캘린더 시스템 |
| ChineseLunarCalendar | `lunar` | 중국 음력, 윤달 포함 |
| ChineseHistoricalCalendar | `chinese_historical` | 중국 역사력, 간지 기년 및 제왕 연호 지원 |
| ChulaSakaratCalendar | `chula_sakarat` | 추라사까랏력 (CS), 년도 - 638 |
| CopticCalendar | `coptic` | 콥트력 |
| DaiCalendar | `dai` | 태족력, 완전한 음력 계산 포함 |
| DehongDaiCalendar | `dehong_dai` | 더홍 태족력 변형 |
| EthiopianCalendar | `ethiopian` | 에티오피아력 |
| FrenchRepublicanCalendar | `french_republican` | 프랑스 공화력 |
| GregorianCalendar | `gregorian` | 표준 그레고리력 |
| HebrewCalendar | `hebrew` | 히브리 (유대) 력 |
| IndianCalendar | `indian` | 인도 국력 |
| InuitCalendar | `inuit` | 이누이트 캘린더 시스템 |
| IslamicCalendar | `islamic` | 이슬람 회교력 |
| JapaneseCalendar | `japanese` | 일본 연호 (Nengo) 력 |
| JavaneseCalendar | `javanese` | 자바 이슬람력 |
| JucheCalendar | `juche` | 주체력 (조선), 년도 - 1911 |
| JulianCalendar | `julian` | 율리우스력 |
| KhmerCalendar | `khmer` | 크메르력 |
| MayanCalendar | `mayan` | 마야 장기력 |
| MongolianCalendar | `mongolian` | 몽골력 |
| PersianCalendar | `persian` | 페르시아 (태양 회교) 력 |
| RepublicOfChinaCalendar | `roc` | 중화민국 (민국) 력, 년도 - 1911 |
| RomanCalendar | `roman` | 로마력 |
| SakaCalendar | `saka` | 사카력 (인도네시아) |
| SexagenaryCalendar | `sexagenary` | 중국 간지력 (Ganzhi) |
| TibetanCalendar | `tibetan` | 티베트력 |
| VietnameseCalendar | `vietnamese` | 베트남 음력 (고양 띠 변형) |
| VikramSamvatCalendar | `vikram_samvat` | 비크람 삼바트력 |
| YiCalendar | `yi` | 이족력 시스템 |
| ZoroastrianCalendar | `zoroastrian` | 조로아스터력 |

`CalendarTool`은 작업 제공: `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (캘린더 간 날짜 변환).

---

## Web UI 아키텍처

### 스킨 시스템

Web UI는 애플리케이션 로직 변경 없이 완전한 UI 커스터마이제이션을 허용하는 **플러그형 스킨 시스템**을 가집니다:

- **ISkin 인터페이스** — 모든 스킨의 계약 정의, 포함:
  - 코어 렌더링 메서드 (`RenderHtml`, `RenderError`)
  - 20개 이상 UI 컴포넌트 메서드 (버튼, 입력, 카드, 테이블, 배지, 말풍선, 진행률, 태그 등)
  - `CssBuilder`를 통한 테마 CSS 생성
  - `SkinPreviewInfo` — 초기화 페이지 스킨 선택기의 컬러 팔레트 및 아이콘

- **내장 스킨** — 4개 프로덕션 준비 스킨:
  - **Admin** — 전문적이고 데이터 중심의 시스템 관리 인터페이스
  - **Chat** — AI 상호작용을 위한 대화형, 메시지 중심 설계
  - **Creative** — 예술적이고 시각적으로 풍부한 창작 워크플로우 레이아웃
  - **Dev** — 개발자 중심, 코드 중심 인터페이스, 구문 강조 포함

- **스킨 발견** — `SkinManager`가 리플렉션을 통해 모든 `ISkin` 구현 자동 발견 및 등록

### HTML / CSS / JS 빌더

Web UI는 템플릿 파일을 완전히 피하고, C#에서 모든 마크업 생성:

- **`H`** — 코드에서 HTML 트리를 구축하기 위한 스트리밍 HTML 빌더 DSL
- **`CssBuilder`** — 선택기 및 미디어 쿼리 지원 CSS 빌더
- **`JsBuilder` (`JsSyntax`)** — 인라인 스크립트용 JavaScript 빌더

### 컨트롤러 시스템

Web UI는 20개 이상의 컨트롤러가 다양한 측면을 처리하는 **유사 MVC 패턴**을 따릅니다:

| 컨트롤러 | 용도 |
|------------|---------|
| About | 정보 페이지 및 프로젝트 정보 |
| Audit | 추세 차트 및 내보내기가 포함된 Token 사용 감사 대시보드 |
| Being | 실리콘 생명체 관리 및 상태 |
| Chat | SSE를 사용한 실시간 채팅 인터페이스 |
| ChatHistory | 세션 목록 및 메시지 세부정보를 지원하는 채팅 기록 보기 |
| CodeBrowser | 코드 보기 및 편집 |
| CodeHover | 구문 강조를 지원하는 코드 호버 힌트 |
| Config | 시스템 설정 관리 |
| Dashboard | 시스템 개요 및 지표 |
| Executor | 실행기 상태 및 관리 |
| Help | 다국어 지원 도움말 문서 시스템 |
| Init | 첫 실행 초기화 마법사 |
| Knowledge | 지식 그래프 시각화 및 쿼리 |
| Log | 실리콘 생명체 필터링을 지원하는 시스템 로그 뷰어 |
| Memory | 고급 필터링, 통계 및 세부정보 뷰를 지원하는 장기 메모리 브라우저 |
| Permission | 권한 관리 |
| PermissionRequest | 권한 요청 큐 |
| Project | 작업 노트 및 작업 시스템을 포함한 프로젝트 관리 |
| Task | 작업 시스템 인터페이스 |
| Timer | 실행 기록을 포함한 타이머 시스템 관리 |
| WorkNote | 검색 및 목차 생성을 지원하는 작업 노트 관리 |

### 실시간 업데이트

- **SSE (서버 전송 이벤트)** — `SSEHandler`를 통해 채팅 메시지, 생명체 상태 및 시스템 이벤트 업데이트 푸시
- **WebSocket 불필요** — 대부분의 실시간 요구사항에 더 간단한 아키텍처로 SSE 사용
- **자동 재연결** — 클라이언트 재연결 로직으로 탄력적 연결

### 지역화

시스템은 **21개 언어 변형**에 대한 포괄적인 지역화를 지원합니다:
- **중국어 (6개)**: zh-CN (간체), zh-HK (번체), zh-SG (싱가포르), zh-MO (마카오), zh-TW (대만), zhMY (말레이시아)
- **영어 (10개)**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY
- **스페인어 (2개)**: es-ES, es-MX
- **기타 (3개)**: ja-JP (일본어), ko-KR (한국어), cs-CZ (체코어)

`DefaultConfigData.Language`를 통해 활성 로캘 선택 및 `LocalizationManager`를 통해 확인.

---

### WebView 브라우저 자동화 시스템 (신규)

시스템은 **Playwright** 기반 WebView 브라우저 자동화 기능을 통합했습니다:

- **개별 격리**: 각 실리콘 생명체가 독립적인 브라우저 인스턴스, 쿠키 및 세션 저장을 가지며, 완전히 격리되어 서로 간섭하지 않음.
- **헤드리스 모드**: 브라우저가 사용자가 전혀 볼 수 없는 헤드리스 모드에서 실행되며, 실리콘 생명체가 백그라운드에서 자율적으로 작동.
- **WebViewBrowserTool**: 완전한 브라우저 작업 기능 제공:
  - 페이지 탐색, 클릭, 텍스트 입력, 페이지 콘텐츠 가져오기
  - JavaScript 실행, 스크린샷 가져오기, 요소 표시 대기
  - 브라우저 상태 관리 및 리소스 정리
- **보안 제어**: 모든 브라우저 작업이 권한 검증 체인을 통과해야 하며, 악성 웹 페이지 접근 방지.

### 지식 네트워크 시스템 (신규)

시스템은 **트리플 구조** 기반 지식 그래프 시스템을 내장했습니다:

- **지식 표현**: "주어-관계-목적어" 트리플 구조 채택 (예: Python-is_a-programming_language)
- **KnowledgeTool**: 지식의 전 생애주기 관리 제공:
  - `add`/`query`/`update`/`delete` - 기본 CRUD 작업
  - `search` - 전체 텍스트 검색 및 키워드 매칭
  - `get_path` - 두 개념 간 연관 경로 발견
  - `validate` - 지식 무결성 검사
  - `stats` - 지식 네트워크 통계 분석
- **영속화 저장**: 지식 트리플을 파일 시스템에 영속화하며, 시간 인덱스 쿼리 지원.
- **신뢰도 점수**: 각 지식 항목에 신뢰도 점수 (0-1)가 있어, 지식의 퍼지 매칭 및 정렬 지원.
- **태그 분류**: 지식에 태그 추가 지원, 분류 및 검색 용이.

---

## 데이터 디렉토리 구조

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # 큐레이터의 소울 파일
    │   ├── state.json       # 런타임 상태
    │   ├── code.enc         # AES 암호화된 맞춤 클래스 코드
    │   └── permission.enc   # AES 암호화된 맞춤 권한 콜백
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```
