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
