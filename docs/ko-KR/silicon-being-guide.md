# 실리콘 생명체 가이드

[English](../en/silicon-being-guide.md) | [中文](../zh-CN/silicon-being-guide.md) | [繁體中文](../zh-HK/silicon-being-guide.md) | [Español](../es-ES/silicon-being-guide.md) | [日本語](../ja-JP/silicon-being-guide.md) | **한국어** | [Deutsch](../de-DE/silicon-being-guide.md) | [Čeština](../cs-CZ/silicon-being-guide.md)

## 개요

실리콘 생명체는 AI 기반의 자율적 사고, 행동 및 진화가 가능한 에이전트입니다.

## 아키텍처

### 신체-두뇌 분리

```
┌─────────────────────────────────────┐
│         실리콘 생명체                │
├──────────────────┬──────────────────┤
│   신체            │   두뇌            │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • 상태 관리       │ • 기록 로드       │
│ • 트리거 감지     │ • AI 호출         │
│ • 수명 주기       │ • 도구 실행       │
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

## 생명체 만들기

### Web UI 통해

1. **생명체 관리**로 이동
2. **새 생명체 만들기** 클릭
3. 입력:
   - 이름
   - 소울 내용
   - 설정 옵션
4. **만들기** 클릭

### API 통해

```bash
curl -X POST http://localhost:8080/api/beings \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Assistant",
    "soul": "# Personality\nYou are helpful..."
  }'
```

## 생명체 수명 주기

### 상태

```
만듦 → 시작 중 → 실행 중 → 중지 중 → 중지됨
                    ↓
                  오류
```

### 작업

- **시작**: 초기화 및 처리 시작
- **중지**: 우아한 종료
- **일시중지**: 임시 중단 (상태 유지)
- **재개**: 일시중지 상태에서 계속

## 작업 시스템

### 작업 만들기

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

### 작업 상태

- `Pending` - 실행 대기
- `Running` - 실행 중
- `Completed` - 성공 완료
- `Failed` - 실행 실패
- `Cancelled` - 수동 취소

## 타이머 시스템

### 타이머 타입

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

## 메모리 시스템

### 메모리 타입

- **단기**: 현재 대화 컨텍스트
- **장기**: 영속화된 지식 및 경험
- **에피소드**: 시간 인덱스된 이벤트 및 상호작용

### 저장 구조

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

## 작업 노트 시스템

### 개요

작업 노트는 실리콘 생명체의 개인 일기 시스템으로, 페이지 디자인을 사용하여 작업 진행 상황, 학습 노트, 프로젝트 메모 등을 기록합니다.

### 기능

- **페이지 관리**: 각 노트는 독립된 페이지, 페이지번호로 접근
- **Markdown 지원**: 내용에서 Markdown 형식 지원 (텍스트, 목록, 표, 코드 블록)
- **키워드 인덱스**: 노트에 키워드 추가 지원, 검색 용이
- **요약 기능**: 각 노트에 짧은 요약, 빠른 탐색
- **목차 생성**: 모든 노트의 목차 개요 생성 가능, 전체 컨텍스트 이해 도움
- **타임스탬프**: 생성 및 업데이트 시간 자동 기록
- **기본 비공개**: 생명체 자신만 접근 가능 (큐레이터 관리 가능)

### 사용 사례

1. **프로젝트 진행 상황 기록**
   ```
   요약: 사용자 인증 모듈 완료
   내용: JWT token 검증, OAuth2 통합, refresh token 메커니즘 구현
   키워드: 인증,JWT,OAuth2
   ```

2. **학습 노트**
   ```
   요약: C# 비동기 프로그래밍 모범 사례 학습
   내용: async/await 사용 시 주의사항, ConfigureFlags 사용 사례...
   키워드: C#,비동기,모범 사례
   ```

3. **회의 기록**
   ```
   요약: 제품 요구사항 논의 회의
   내용: 새 기능 요구사항 논의, 구현 방안 확정...
   키워드: 제품,요구사항,회의
   ```

### 도구 통해 사용

생명체는 `work_note` 도구로 작업 노트를 관리할 수 있습니다:

```json
// 노트 만들기
{
  "action": "create",
  "summary": "사용자 인증 모듈 완료",
  "content": "## 구현 세부정보\n\n- JWT token 사용\n- OAuth2 지원",
  "keywords": "인증,JWT,OAuth2"
}

// 노트 읽기
{
  "action": "read",
  "page_number": 1
}

// 노트 검색
{
  "action": "search",
  "keyword": "인증",
  "max_results": 10
}
```

### Web UI 통해 관리

1. **생명체 관리** → 생명체 선택으로 이동
2. **작업 노트** 탭 클릭
3. 노트 보기, 검색, 편집 가능
4. Markdown 미리보기 지원

## 지식 네트워크 시스템

### 개요

지식 네트워크는 트리플 구조 (주어-서술어-목적어) 기반의 지식 표현 및 관리 시스템으로, 구조화된 지식 저장 및 관리에 사용됩니다.

### 핵심 개념

#### 트리플 구조

```
주어 (Subject) --서술어 (Predicate)--> 목적어 (Object)
```

**예시**:
- `Python` --`is_a`--> `programming_language`
- `베이징` --`capital_of`--> `중국`
- `물` --`boiling_point`--> `100°C`

#### 신뢰도

각 지식 트리플에는 신뢰도 점수 (0.0-1.0)가 있어 지식의 신뢰도를 나타냅니다:
- `1.0`: 절대 확신 (수학 정리 등)
- `0.8-0.99`: 높은 신뢰 (검증된 사실 등)
- `0.5-0.79`: 중간 신뢰 (추론 또는 가정 등)
- `<0.5`: 낮은 신뢰 (추측 또는 미검증 정보 등)

#### 태그 시스템

트리플에 태그 추가 지원, 분류 및 검색 용이:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "tags": ["programming", "language", "popular"]
}
```

### 지식 작업

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

#### 2. 지식 쿼리

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

두 개념 간 연관 경로 찾기:
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

지식의 유효성 및 일관성 검사:
```json
{
  "action": "validate",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

#### 6. 지식 통계

지식 네트워크의 전체 통계 정보 가져오기:
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
  "averageconfident": 0.87
}
```

### 사용 사례

1. **사실 저장**
   - 객관적 사실 및 상식 저장
   - 예시: `지구` --`is_a`--> `행성`

2. **개념 관계**
   - 개념 간 관계 기록
   - 예시: `상속` --`is_a`--> `객체 지향 프로그래밍 개념`

3. **학습 축적**
   - 생명체가 학습을 통해 지식 축적
   - 구조화된 지식 체계 형성

4. **추론 지원**
   - 지식 경로를 통해 간접 관계 발견
   - 지식 기반 추론 및 의사결정 지원

### Web UI 통해 관리

1. **지식 네트워크** 페이지로 이동
2. 지식 통계 정보 보기
3. 지식 검색 및 탐색
4. 지식 관계도 시각화 (계획 중)

## WebView 브라우저 작업 (신규)

### 개요

실리콘 생명체는 WebView 브라우저 도구를 통해 웹 페이지 자율 탐색, 정보 수집, 웹 작업 실행이 가능합니다. 브라우저는 헤드리스 모드에서 실행되며, 사용자에게 완전히 보이지 않습니다.

### 기능

- **개별 격리**: 각 생명체가 독립적인 브라우저 인스턴스, 쿠키 및 세션 소유
- **헤드리스 모드**: 백그라운드에서 자율 작업, 사용자에게 보이지 않음
- **완전한 기능**: JavaScript 실행, CSS 렌더링, 양식 작성 등 지원
- **보안 제어**: 모든 작업이 권한 검증 체인 통과 필요

### 일반 작업

#### 1. 브라우저 열기

```json
{
  "action": "open_browser"
}
```

#### 2. 웹 페이지로 이동

```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

#### 3. 페이지 내용 가져오기

```json
{
  "action": "get_page_text"
}
```

페이지 텍스트 내용 반환, AI 분석 및 이해에 사용.

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
  "text": "검색 키워드"
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

페이지 스크린샷 반환 (Base64 인코딩), 시각 분석에 사용 가능.

#### 8. 요소 나타날 때까지 대기

```json
{
  "action": "wait_for_element",
  "selector": ".loading-complete",
  "timeout": 10000
}
```

### 사용 사례

1. **정보 수집**
   - 뉴스 사이트 탐색하여 최신 정보 수집
   - 문서 및 기술 자료 조회
   - 웹 페이지 내용 변경 모니터링

2. **자동화 작업**
   - 양식 작성 및 제출
   - 버튼 클릭하여 작업 트리거
   - 웹 데이터 스크래핑

3. **웹 페이지 분석**
   - 페이지 구조 및 내용 분석
   - 특정 정보 추출
   - 시각적 페이지 스크린샷 분석

### 주의 사항

- 브라우저 작업은 느릴 수 있으며, 페이지 로딩 완료 대기 필요
- `wait_for_element` 사용하여 요소 나타난 후 작업
- 웹 사이트의 이용 약관 및 robots.txt 준수
- 빈번한 요청으로 인한 차단 방지

## 모범 사례

### 소울 파일 작성

1. **구체적으로**: 명확한 개성 특성 및 경계
2. **범위 정의**: 생명체가 해야 할 일과 하지 말아야 할 일
3. **예시 포함**: 예상되는 행동 패턴 보여주기
4. **정기적 업데이트**: 성능에 따라 소울 진화

### 작업 관리

1. **우선순위 설정**: 우선순위 (1-10) 사용
2. **마감일 정의**: 항상 마감일 설정
3. **진행 상황 모니터링**: 정기적으로 작업 상태 확인
4. **실패 처리**: 재시도 로직 구현

### 메모리 최적화

1. **오래된 데이터 정리**: 정기적으로 오래된 메모리 보관
2. **중요 정보 인덱스**: 주요 정보 태그
3. **시간 저장 사용**: 시간 인덱스 쿼리 활용

## 문제 해결

### 생명체 시작 실패

**확인**:
- 소울 파일 존재 및 유효
- AI 클라이언트 설정됨
- 시스템 리소스 충분

### 생명체 예기치 않게 중지

**확인**:
- 로그에서 오류
- AI 서비스 가용성
- 메모리 사용량

### 작업 실행 안 됨

**확인**:
- 타이머 시스템 실행 중
- 작업 우선순위 및 일정
- 권한 설정

## 다음 단계

- 📚 [아키텍처 가이드](architecture.md) 읽기
- 🛠️ [개발 가이드](development-guide.md) 보기
- 🚀 [빠른 시작 가이드](getting-started.md) 보기
