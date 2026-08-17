# 실리콘 비잉 가이드

> **버전: v0.2.0-alpha**

[English](../en/silicon-being-guide.md) | [Deutsch](../de-DE/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | **한국어** | [Čeština](../cs-CZ/silicon-being-guide.md) | [Русский](../ru-RU/silicon-being-guide.md)

## 개요

실리콘 비잉은 AI 기반 지능 에이전트로, 자율적으로 사고하고 행동하며 진화할 수 있습니다.

## 아키텍처

### 바디-브레인 분리

```
┌─────────────────────────────────────┐
│         실리콘 비잉                  │
├──────────────────┬──────────────────┤
│   바디            │   브레인          │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • 상태 관리       │ • 이력 로드       │
│ • 트리거 감지     │ • AI 호출         │
│ • 라이프사이클    │ • 툴 실행         │
│                  │ • 응답 영속화     │
└──────────────────┴──────────────────┘
```

## 소울 파일

### 구조

```markdown
# Being Name

## Personality
Describe the being's personality traits and characteristics.

## Capabilities
List what this being can do.

## Behavior Guidelines
Define how the being should behave in different situations.

## Knowledge Domain
Specify the being's area of expertise.
```

### 예시

```markdown
# Code Review Assistant

## Personality
You are a meticulous code reviewer with 10 years of experience.
You provide constructive feedback and always explain your reasoning.

## Capabilities
- Review code for bugs and best practices
- Suggest performance optimizations
- Explain complex algorithms
- Identify security vulnerabilities

## Behavior Guidelines
- Start with positive observations
- Provide specific examples
- Explain why changes are needed
- Be respectful and professional

## Knowledge Domain
Specialized in C#, .NET, and software architecture.
```

## 비잉 생성

### Web UI를 통해

1. **비잉 관리**로 이동
2. **새 비잉 생성** 클릭
3. 입력:
   - 이름
   - 소울 내용
   - 설정 옵션
4. **생성** 클릭

### API를 통해

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistant",
    "soul": "# Personality\nYou are helpful..."
  }'
```

## 비잉 라이프사이클

### 활동 상태

실리콘 비잉은 다음과 같은 활동 상태를 가집니다:

| 상태 | 설명 |
|------|------|
| `Idle` | 대기 상태, 틱 트리거 대기 |
| `SingleChat` | 1:1 채팅 진행 중 |
| `GroupChat` | 그룹 채팅 진행 중 |
| `Task` | 태스크 실행 중 |
| `Timer` | 타이머 실행 중 |
| `Broadcast` | 브로드캐스트 메시지 처리 중 |
| `Project` | 프로젝트 작업 중 |
| `MemoryCompression` | 메모리 압축 중 |
| `Stopped` | 정지됨, 연속 오류 또는 수동 정지로 인함 |

**Stopped 상태 메커니즘**:
- 실리콘 비잉이 연속으로 10회 오류가 발생하면 자동으로 `Stopped` 상태로 전환됩니다
- Stopped 상태에 진입하면 비잉은 더 이상 어떤 태스크도 실행하지 않습니다
- 새로운 채팅 메시지가 도착하면 오류 카운터가 초기화되고 비잉이 다시 실행됩니다
- 수동 개입을 통해서도 재시작할 수 있습니다

### 상태 전환

```
Idle → SingleChat → Idle（채팅 완료）
Idle → GroupChat → Idle（그룹 채팅 완료）
Idle → Task → Idle（태스크 완료）
Idle → Timer → Idle（타이머 완료）
Idle → Broadcast → Idle（브로드캐스트 처리 완료）
Idle → Project → Idle（프로젝트 작업 완료）
Idle → MemoryCompression → Idle（메모리 압축 완료）
임의 → Stopped（연속 10회 오류）
Stopped → Idle（새 채팅 메시지 도착 또는 수동 재시작）
```

### 작업

- **시작**: 초기화 및 처리 시작
- **정지**: 우아한 종료
- **재시작**: Stopped 상태에서 Idle 상태로 복구

## 태스크 시스템

### 태스크 생성

```csharp
var task = new BeingTask
{
    BeingId = being.Id,
    Description = "Review the code",
    Priority = 5,
    DueDate = DateTime.UtcNow.AddHours(2)
};

await taskSystem.CreateAsync(task);
```

### 태스크 상태

- `Pending` - 실행 대기
- `Running` - 실행 중
- `SubmittedForReview` - 리뷰 제출됨
- `UnderReview` - 리뷰 중
- `Rework` - 재작업
- `Completed` - 성공적으로 완료
- `Failed` - 실행 실패
- `Cancelled` - 수동 취소

## 타이머 시스템

### 타이머 유형

1. **일회성**: 지연 후 한 번 실행
2. **간격**: 고정 간격으로 반복 실행
3. **Cron**: cron 표현식 기반 실행

### 예시

```csharp
// 매시간 실행
var timer = new BeingTimer
{
    BeingId = being.Id,
    Interval = TimeSpan.FromHours(1),
    Action = "think",
    Repeat = true
};

await timerSystem.StartAsync(timer);
```

## 스킬 시스템

스킬(Skill)은 실리콘 비잉의 재사용 가능한 능력 단위입니다 —— "툴 오케스트레이션 + 프롬프트 템플릿"을 선언 가능하고, 진화 가능하며, 자동 스케줄링 가능한 함수로 캡슐화하여, AI가 일반 툴을 호출하는 것처럼 스킬을 호출합니다.

### 스킬 구조

| 요소 | 설명 |
|------|------|
| `id` / `description` | 고유 식별자와 한 줄 설명(AI에게 표시되며, AI가 해당 스킬을 언제 선택할지 결정) |
| `parameter_schema` | 매개변수 JSON Schema, 프롬프트에서 사용되는 각 `{param}` 자리표시자 선언 |
| `system_prompt_template` | 시스템 프롬프트 템플릿, 실행 시 매개변수로 자리표시자 채움 |
| `tool_whitelist` | 실행 중 허용되는 툴 목록(비어 있음 = 비잉의 모든 툴 상속) |
| `max_tool_round` / `timeout` | 툴 라운드 수 및 타임아웃 상한(글로벌 상한에 의해 클램핑됨) |
| `on_complete` | 완료 동작: `none` / `write_memory` / `notify_curator` / `broadcast` |
| `trigger_mode` | `Manual`(AI 자율 호출) 또는 `Auto` + `schedule` 스케줄링 |

### 네 가지 소스

- **Builtin** — 프레임워크 내장(`summarize_document` 문서 요약, `code_review` 코드 리뷰, `research_topic` 주제 조사)
- **Plugin** — 플러그인이 `ISkillProvider`를 통해 등록
- **Being** — 비잉이 런타임에 `skill` 툴로 자체 생성
- **User** — 사용자가 Web UI 스킬 관리 페이지에서 생성

### 트리거 방식

1. **수동(Manual)**: 스킬이 일반 툴 정의로 AI 요청에 주입되며, AI가 호출 시기를 판단; 스케줄링 측은 동일한 이름의 호출을 스킬로 우선 라우팅
2. **자동(Auto + schedule)**: 스케줄 표현식이 `metadata.schedule`에 저장되며, 세 가지 형식을 지원:
   - `"09:30"` — 매일 정시
   - `"6h"` / `"30 m"` / `"2 d"` — 간격 주기
   - `"0 9 * * *"` / `"*/15 * * * *"` — cron 서브셋

### Markdown 작성

스킬은 Markdown으로 저장됩니다(`skills/{id}.md`, YAML 프론트 메타데이터 + 프롬프트 본문):

```markdown
---
id: daily_news_digest
description: 搜索今日科技新闻并生成摘要
tool_whitelist: [network, work_note]
on_complete: write_memory
---

请使用 network 工具搜索 {topic} 的最新新闻，生成 500 字摘要并保存到工作笔记。
```

본문만 작성(YAML 생략)해도 됩니다: 저장 시 AI가 id, description, 매개변수 schema 등의 메타데이터를 자동으로 보완합니다 —— 사용자가 이미 작성한 필드는 덮어쓰지 않습니다.

### 비잉 자기 관리

비잉은 `skill` 툴을 통해 자신의 스킬 라이브러리를 관리할 수 있습니다:

```json
{ "action": "list" }
{ "action": "create", "id": "my_skill", "system_prompt": "...", "description": "..." }
{ "action": "update_from_md", "skill_id": "my_skill", "markdown": "..." }
{ "action": "delete", "skill_id": "my_skill" }
```

### 핫 리로드 및 진화

- 비잉은 30초마다 `skills/` 디렉토리 변경을 감지(지문 비교)하며, Web UI 또는 다른 비잉의 수정 사항이 자동으로 적용되어 재시작이 불필요
- 스킬 업데이트 시마다 이전 버전이 `skills/archive/{id}/{version}.md`에 자동 보관되어 스킬 진화 역사가 형성됨
- 커스텀 스킬 수량은 할당량 제한(`MaxCustomSkillsPerBeing`, 기본값 50)

### 실행 가드레일

- 스킬 수준의 `execute` 동작 권한(권한 매트릭스로 비활성화 가능, 비활성화 시 AI에 보이지 않음)
- 실행 매개변수가 글로벌 상한에 의해 클램핑됨: 라운드 수 ≤ `GlobalMaxToolRound`(기본값 10), 타임아웃 ≤ `GlobalSkillTimeoutSeconds`(기본값 300초)
- 스킬은 자기 자신을 재귀적으로 호출할 수 없음
- 화이트리스트 외의 툴 호출은 즉시 실패

## 메모리 시스템

### 메모리 유형

- **단기**: 현재 대화 컨텍스트
- **장기**: 영속화된 지식과 경험
- **에피소드**: 시간 인덱스 이벤트 및 상호작용

### 스토리지 구조

Default 버전:
```
data/
└── beings/
    └── {being-id}/
        ├── soul.md
        ├── memory/
        │   ├── short-term.json
        │   └── long-term/
        │       ├── 2026-04-20.json
        │       └── 2026-04-21.json
        └── tasks/
            └── task-history.json
```

Fast 버전(SpeedyPack 스토리지):
```
data/
├── speedy/
│   ├── {being-id}.spk       # SpeedyPack 스토리지 파일
│   └── {being-id}.spk.idx   # 인덱스 파일
└── beings/
    └── {being-id}/
        └── soul.md
```

## 워크 노트 시스템

### 개요

워크 노트는 실리콘 비잉의 개인 일기 시스템으로, 페이지 기반 설계를 채택하여 작업 진행 상황, 학습 내용, 프로젝트 노트 등을 기록합니다.

### 특징

- **페이지 관리**: 각 노트가 독립적인 페이지로 구성, 페이지 번호로 접근
- **Markdown 지원**: 콘텐츠에 Markdown 형식 지원(텍스트, 목록, 표, 코드 블록)
- **키워드 인덱스**: 노트에 키워드를 추가하여 검색 용이
- **요약 기능**: 각 노트에 짧은 요약이 있어 빠르게 탐색 가능
- **목차 생성**: 모든 노트의 목차 개요를 생성하여 전체 컨텍스트 파악에 도움
- **타임스탬프**: 생성 및 업데이트 시간 자동 기록
- **기본 비공개**: 비잉 본인만 접근 가능(큐레이터는 관리 가능)

### 사용 사례

1. **프로젝트 진행 기록**
   ```
   요약: 사용자 인증 모듈 완료
   내용: JWT 토큰 검증, OAuth2 통합, 토큰 갱신 메커니즘 구현
   키워드: 인증,JWT,OAuth2
   ```

2. **학습 노트**
   ```
   요약: C# 비동기 프로그래밍 모범 사례 학습
   내용: async/await 사용 시 주의사항, ConfigureAwait 사용 사례...
   키워드: C#,비동기,모범사례
   ```

3. **회의록**
   ```
   요약: 제품 요구사항 논의 회의
   내용: 새로운 기능 요구사항 논의, 구현 방안 확정...
   키워드: 제품,요구사항,회의
   ```

### 툴을 통한 사용

비잉은 `work_note` 툴을 통해 워크 노트를 관리할 수 있습니다:

```json
// 노트 생성
{
  "action": "create",
  "summary": "完成用户认证模块",
  "content": "## 实现细节\n\n- 使用 JWT token\n- 支持 OAuth2",
  "keywords": "认证,JWT,OAuth2"
}

// 노트 읽기
{
  "action": "read",
  "page_number": 1
}

// 노트 검색
{
  "action": "search",
  "keyword": "认证",
  "max_results": 10
}
```

### Web UI를 통한 관리

1. **비잉 관리**로 이동 → 비잉 선택
2. **워크 노트** 탭 클릭
3. 노트 조회, 검색, 편집 가능
4. Markdown 미리보기 지원

## 노리지 네트워크 시스템

### 개요

노리지 네트워크는 삼중항 구조(주어-서술어-목적어) 기반의 지식 표현 및 관리 시스템으로, 구조화된 지식을 저장하고 관리합니다.

### 핵심 개념

#### 삼중항 구조

```
주어 (Subject) --서술어 (Predicate)--> 목적어 (Object)
```

**예시**:
- `Python` --`is_a`--> `programming_language`
- `서울` --`capital_of`--> `한국`
- `물` --`boiling_point`--> `100°C`

#### 신뢰도

각 지식 삼중항은 신뢰도 점수(0.0-1.0)를 가지며, 지식의 신뢰 수준을 나타냅니다:
- `1.0`: 절대적 확신(예: 수학 정리)
- `0.8-0.99`: 높은 신뢰도(예: 검증된 사실)
- `0.5-0.79`: 중간 신뢰도(예: 추론이나 가설)
- `<0.5`: 낮은 신뢰도(예: 추측이나 미검증 정보)

#### 태그 시스템

삼중항에 태그를 추가하여 분류 및 검색을 용이하게 합니다:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "tags": ["programming", "language", "popular"]
}
```

### 지식 조작

#### 1. 지식 추가

```json
{
  "action": "add",
  "subject": "C#",
  "predicate": "created_by",
  "object": "Microsoft",
  "confidence": 1.0,
  "tags": ["programming", "language"]
}
```

#### 2. 지식 조회

```json
{
  "action": "query",
  "subject": "C#",
  "predicate": "created_by"
}
```

#### 3. 지식 검색

```json
{
  "action": "search",
  "query": "programming language",
  "limit": 10
}
```

#### 4. 지식 경로 발견

두 개념 사이의 연관 경로를 찾습니다:
```json
{
  "action": "get_path",
  "from": "Python",
  "to": "computer_science"
}
```

반환:
```
Python → is_a → programming_language → belongs_to → computer_science
```

#### 5. 지식 검증

지식의 유효성과 일관성을 확인합니다:
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. 지식 통계

노리지 네트워크의 전체 통계 정보를 가져옵니다:
```json
{
  "action": "stats"
}
```

반환:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### 사용 사례

1. **사실 저장**
   - 객관적 사실과 상식 저장
   - 예시: `지구` --`is_a`--> `행성`

2. **개념 관계**
   - 개념 간의 관계 기록
   - 예시: `상속` --`is_a`--> `객체지향프로그래밍개념`

3. **학습 축적**
   - 비잉이 학습을 통해 지식을 지속적으로 축적
   - 구조화된 지식 체계 형성

4. **추론 지원**
   - 지식 경로를 통해 간접적 관계 발견
   - 지식 기반 추론 및 의사결정 지원

### Web UI를 통한 관리

1. **노리지 네트워크** 페이지로 이동
2. 지식 통계 정보 확인
3. 지식 검색 및 탐색
4. 지식 관계 그래프 시각화(계획 중)

## WebView 브라우저 조작(신규)

### 개요

실리콘 비잉은 WebView 브라우저 툴을 통해 자율적으로 웹페이지를 탐색하고, 정보를 획득하며, 웹 조작을 수행할 수 있습니다. 브라우저는 헤드리스 모드로 실행되며 사용자에게는 완전히 보이지 않습니다.

### 특징

- **개체 격리**: 각 비잉은 독립적인 브라우저 인스턴스, 쿠키 및 세션을 가집니다
- **헤드리스 모드**: 백그라운드에서 자율 조작, 사용자에게 보이지 않음
- **완전한 기능**: JavaScript 실행, CSS 렌더링, 폼 작성 등 지원
- **보안 제어**: 모든 조작은 퍼미션 검증 체인을 통과해야 함

### 주요 조작

#### 1. 브라우저 열기

```json
{
  "action": "open"
}
```

#### 2. 웹페이지 탐색

```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

#### 3. 페이지 콘텐츠 가져오기

```json
{
  "action": "get_page_text"
}
```

페이지 텍스트 콘텐츠를 반환하여 AI가 분석하고 이해할 수 있게 합니다.

#### 4. 요소 클릭

```json
{
  "action": "click",
  "selector": "#submit-button"
}
```

#### 5. 텍스트 입력

```json
{
  "action": "input",
  "selector": "#search-input",
  "text": "搜索关键词"
}
```

#### 6. JavaScript 실행

```json
{
  "action": "execute_script",
  "script": "return document.title;"
}
```

#### 7. 스크린샷 가져오기

```json
{
  "action": "get_screenshot"
}
```

페이지 스크린샷을 반환(Base64 인코딩), 시각적 분석에 활용 가능합니다.

#### 8. 요소 대기

```json
{
  "action": "wait_for_element",
  "selector": ".loading-complete",
  "timeout": 10000
}
```

### 사용 사례

1. **정보 획득**
   - 뉴스 웹사이트 탐색하여 최신 정보 획득
   - 문서 및 기술 자료 조회
   - 웹페이지 콘텐츠 변화 모니터링

2. **자동화 조작**
   - 폼 작성 및 제출
   - 버튼 클릭으로 조작 트리거
   - 웹페이지 데이터 스크래핑

3. **웹페이지 분석**
   - 페이지 구조 및 콘텐츠 분석
   - 특정 정보 추출
   - 시각적 페이지 스크린샷 분석

### 주의사항

- 브라우저 조작은 상대적으로 느릴 수 있으므로 페이지 로딩 완료를 기다려야 합니다
- `wait_for_element`를 사용하여 요소가 나타난 후 조작하세요
- 웹사이트의 이용약관 및 robots.txt를 준수하세요
- 잦은 요청으로 인해 차단되지 않도록 주의하세요

## 모범 사례

### 소울 파일 작성

1. **구체적**: 명확한 성격 특성과 경계
2. **범위 정의**: 비잉이 해야 할 일과 하지 말아야 할 일을 정의
3. **예시 포함**: 기대되는 행동 패턴 보여주기
4. **정기 업데이트**: 성과에 따라 소울 진화

### 태스크 관리

1. **우선순위 설정**: 우선순위 사용(1-10)
2. **마감일 정의**: 항상 마감일 설정
3. **진행 모니터링**: 정기적으로 태스크 상태 확인
4. **실패 처리**: 재시도 로직 구현

### 메모리 최적화

1. **오래된 데이터 정리**: 오래된 메모리를 정기적으로 보관
2. **중요 정보 인덱싱**: 핵심 정보에 표시
3. **타임 스토리지 활용**: 시간 인덱스 쿼리 활용

### 메모리 페이드 메커니즘

시스템에 내장된 `MemoryFadeService` 정기 감쇠 서비스는 생물적 메모리의 망각 특성을 시뮬레이션합니다:

- **자동 감쇠**: 매시간 모든 실리콘 비잉의 메모리 항목에 중요도 감쇠 알고리즘 적용
- **자동 보관**: 중요도가 임계값 미만인 메모리는 자동으로 보관되며 일상 검색에 참여하지 않음
- **통계 추적**: 감쇠 주기 수 및 상태 변경 항목 수 기록

이는 실리콘 비잉의 메모리가 시간이 지남에 따라 자연스럽게 희미해짐을 의미하며, 중요한 정보는 메모리 툴을 통해 능동적으로 높은 중요도로 표시하여 자동 보관을 방지해야 합니다.

---

## 프로젝트 워크스페이스

### 개요

프로젝트 워크스페이스는 다중 실리콘 비잉 협업을 지원하는 공간 관리 메커니즘입니다. 실리콘 큐레이터는 프로젝트 공간을 생성하고, 실리콘 비잉을 프로젝트에 할당하며, 역할을 부여할 수 있습니다.

### 프로젝트 라이프사이클

```
생성 → 활성 → 보관 → 파기
              ↑       |
              └─ 복구 ┘
```

### 프로젝트 역할

실리콘 비잉은 프로젝트 내에서 특정 역할을 할당받을 수 있습니다:

```json
{
  "action": "assign_role",
  "project_id": "project-uuid",
  "being_id": "being-uuid",
  "role_name": "developer"
}
```

### 프로젝트 워크 노트

프로젝트 공간 내의 워크 노트는 공개되어 프로젝트 멤버 모두가 접근할 수 있습니다:

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "完成用户认证模块",
  "content": "## 实现细节\n\n- 使用 JWT token",
  "keywords": "认证,JWT"
}
```

### 프로젝트 태스크

프로젝트 공간 내의 태스크는 완전한 라이프사이클 관리를 지원합니다:

```json
{
  "action": "create",
  "project_id": "project-uuid",
  "title": "实现用户认证",
  "priority": 5
}
```

### 프로젝트 워크플로우

프로젝트는 워크플로우 템플릿을 바인딩하여 실리콘 비잉의 협업 프로세스를 구동할 수 있습니다:

- 워크플로우는 상태 머신 템플릿 기반
- 틱 기반 상태 전환 지원
- 상태 전환 로그 자동 기록

### 툴 퍼미션 격리

프로젝트 수준의 툴 퍼미션은 실리콘 비잉 수준의 퍼미션과 독립적이며, 프로젝트 간 퍼미션 격리를 구현합니다. 예를 들어, 하나의 실리콘 비잉이 프로젝트 A에서는 네트워크 접근 퍼미션을 가질 수 있지만, 프로젝트 B에서는 읽기 전용 퍼미션으로 제한될 수 있습니다.

## 문제 해결

### 비잉이 시작되지 않음

**확인**:
- 소울 파일이 존재하고 유효한지
- AI 클라이언트가 구성되었는지
- 시스템 리소스가 충분한지

### 비잉이 예기치 않게 정지됨

**확인**:
- 로그의 오류
- AI 서비스 가용성
- 메모리 사용량

### 태스크가 실행되지 않음

**확인**:
- 타이머 시스템이 실행 중인지
- 태스크 우선순위 및 일정
- 퍼미션 설정

## 다음 단계

- 📚 [아키텍처 가이드](architecture.md) 읽기
- 🛠️ [개발 가이드](development-guide.md) 확인
- 🚀 [시작 가이드](getting-started.md) 확인
