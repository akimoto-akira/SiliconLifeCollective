# 실리콘 비잉 가이드

[English](silicon-being-guide.md) | [简体中文](docs/zh-CN/silicon-being-guide.md) | [繁體中文](docs/zh-HK/silicon-being-guide.md) | [Español](docs/es-ES/silicon-being-guide.md) | [日本語](docs/ja-JP/silicon-being-guide.md) | [한국어](docs/ko-KR/silicon-being-guide.md) | [Čeština](docs/cs-CZ/silicon-being-guide.md)

## 개요

실리콘 비잉은 자율적으로 생각, 행동, 진화할 수 있는 AI 기반 에이전트입니다.

## 아키텍처

### Body-Brain 분리

```
┌─────────────────────────────────────┐
│         실리콘 비잉               │
├──────────────────┬──────────────────┤
│   Body           │   Brain          │
│ (SiliconBeing)   │ (ContextManager) │
├──────────────────┼──────────────────┤
│ • 상태 관리      │ • 기록 로드       │
│ • 트리거 감지    │ • AI 호출        │
│ • 수명 주기      │ • 도구 실행      │
│                  │ • 응답 저장       │
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

## Being 생성

### Web UI를 통해

1. **Being 관리**로 이동
2. **새 Being 생성** 클릭
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

## Being 수명 주기

### 상태

```
Created → Starting → Running → Stopping → Stopped
                    ↓
                  Error
```

### 작업

- **시작**: 초기화 및 처리 시작
- **중지**: 우아하게 종료
- **일시정지**: 일시적으로 중단 (상태 유지)
- **재개**: 일시정지 상태에서 계속

## 작업 시스템

### 작업 생성

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

- `Pending` - 실행 대기 중
- `Running` - 현재 실행 중
- `Completed` - 성공적으로 완료
- `Failed` - 실행 실패
- `Cancelled` - 수동으로 취소됨

## 타이머 시스템

### 타이머 타입

1. **One-shot**: 지연 후 한 번 실행
2. **Interval**: 고정 간격으로 반복 실행
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
- **장기**: 영구 지식 및 경험
- **에피소드**: 시간 인덱스 이벤트 및 상호작용

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

작업 노트는 실리콘 생명체의 개인 일기 시스템으로, 페이지 기반 설계로 작업 진행 상황, 학습 메모, 프로젝트 노트 등을 기록하는 데 사용됩니다.

### 특징

- **페이지 관리**: 각 노트는 독립적인 페이지로, 페이지 번호로 접근
- **Markdown 지원**: 내용은 Markdown 형식 지원(텍스트, 목록, 표, 코드 블록)
- **키워드 색인**: 노트에 키워드 추가 가능, 검색 용이
- **요약 기능**: 각 노트에 짧은 요약, 빠른 브라우징
- **디렉토리 생성**: 모든 노트의 디렉토리 개요 생성 가능, 전체 컨텍스트 이해 도움
- **타임스탬프**: 생성 및 업데이트 시간 자동 기록
- **기본 비공개**: 생명체 자신만 접근 가능(큐레이터 관리 가능)

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
   내용: async/await 사용 주의사항, ConfigureFlags 사용 시나리오...
   키워드: C#,비동기,모범 사례
   ```

3. **회의록**
   ```
   요약: 제품 요구사항 논의 회의
   내용: 신기능 요구사항 논의, 구현 방안 확정...
   키워드: 제품,요구사항,회의
   ```

### 도구를 통한 사용

생명체는 `work_note` 도구를 통해 작업 노트를 관리할 수 있습니다:

```json
// 노트 생성
{
  "action": "create",
  "summary": "사용자 인증 모듈 완료",
  "content": "## 구현 세부사항\n\n- JWT token 사용\n- OAuth2 지원",
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

### Web UI를 통한 관리

1. **생명체 관리** → 생명체 선택으로 이동
2. **작업 노트** 탭 클릭
3. 노트 보기, 검색, 편집 가능
4. Markdown 미리보기 지원

## 지식 네트워크 시스템

### 개요

지식 네트워크는 삼중항 구조(주어-서술어-목적어)를 기반으로 하는 지식 표현 및 관리 시스템으로, 구조화된 지식의 저장 및 관리를 위해 사용됩니다.

### 핵심 개념

#### 삼중항 구조

```
주어 (Subject) --서술어 (Predicate)--> 목적어 (Object)
```

**예시**:
- `Python` --`is_a`--> `programming_language`
- `베이징` --`capital_of`--> `중국`
- `물` --`boiling_point`--> `100°C`

#### 신뢰도

각 지식 삼중항에는 신뢰도 점수(0.0-1.0)가 있어 지식의 신뢰도를 나타냅니다:
- `1.0`: 절대 확실(수학 정리 등)
- `0.8-0.99`: 높은 신뢰(검증된 사실 등)
- `0.5-0.79`: 중간 신뢰(추론 또는 가정 등)
- `<0.5`: 낮은 신뢰(추측 또는 미검증 정보 등)

#### 태그 시스템

삼중항에 태그 추가 가능, 분류 및 검색 용이:
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

#### 5. 지식 유효성 검사

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
  "averageConfidence": 0.87
}
```

### 사용 사례

1. **사실 저장**
   - 객관적 사실 및 상식 저장
   - 예: `지구` --`is_a`--> `행성`

2. **개념 관계**
   - 개념 간 관계 기록
   - 예: `상속` --`is_a`--> `객체 지향 프로그래밍 개념`

3. **학습 축적**
   - 생명체가 학습을 통해 지식 계속 축적
   - 구조화된 지식 시스템 형성

4. **추론 지원**
   - 지식 경로를 통해 간접 관계 발견
   - 지식 기반 추론 및 결정 지원

### Web UI를 통한 관리

1. **지식 네트워크** 페이지로 이동
2. 지식 통계 정보 보기
3. 지식 검색 및 브라우징
4. 지식 관계 시각화(계획 중)

## 모범 사례

### 소울 파일 작성

1. **구체적으로**: 명확한 성격 특성 및 경계
2. **범위 정의**: being이 해야 할 일과 하지 말아야 할 일
3. **예시 포함**: 예상 동작 패턴 표시
4. **정기적으로 업데이트**: 성능 기반 소울 진화

### 작업 관리

1. **우선순위 설정**: 우선순위 레벨 (1-10) 사용
2. **마감일 정의**: 항상 마감일 설정
3. **진행 모니터링**: 정기적으로 작업 상태 확인
4. **실패 처리**: 재시작 로직 구현

### 메모리 최적화

1. **오래된 데이터 정리**: 주기적으로 오래된 메모리 아카이브
2. **중요 정보 인덱스**: 중요 정보 태그
3. **시간 저장소 활용**: 시간 인덱스 쿼리 활용

## 문제 해결

### Being이 시작되지 않음

**확인**:
- 소울 파일 존재 및 유효
- AI 클라이언트 설정됨
- 충분한 시스템 리소스

### Being이 예기치 않게 중지됨

**확인**:
- 로그에서 오류
- AI 서비스 가용성
- 메모리 사용량

### 작업이 실행되지 않음

**확인**:
- 타이머 시스템 실행 중
- 작업 우선순위 및 일정
- 권한 설정

## 다음 단계

- 📚 [아키텍처 가이드](architecture.md) 읽기
- 🛠️ [개발 가이드](development-guide.md) 확인
- 🚀 [시작 가이드](getting-started.md) 참조
