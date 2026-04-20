# 보안 설계

[English](security.md) | [中文](docs/zh-CN/security.md) | [繁體中文](docs/zh-HK/security.md) | [日本語](docs/ja-JP/security.md) | [한국어](docs/ko-KR/security.md)

## 개요

Silicon Life Collective의 보안은 **계층적 방어** 모델 위에 구축됩니다. 핵심 원칙: **모든 I/O 작업은 executor를 통과**해야 하며, executor는 실행 전 권한 검사를 적용합니다.

```
Tool Call → Executor → PermissionManager → HighDeny 캐시 → HighAllow 캐시 → Callback → AskUser
```

---

## 권한 모델

### 권한 타입

| 타입 | 설명 |
|------|------|
| `NetworkAccess` | 아웃바운드 HTTP/HTTPS 요청 |
| `CommandLine` | 셸 명령 실행 |
| `FileAccess` | 파일 및 디렉토리 작업 |
| `Function` | 민감한 함수 호출 |
| `DataAccess` | 시스템 또는 사용자 데이터 접근 |

### 권한 결과

모든 권한 검사는 세 가지 결과 중 하나를 반환합니다:

| 결과 | 동작 |
|------|------|
| **Allowed** | 작업이 즉시 진행 |
| **Denied** | 작업 차단, 감사 로그 기록 |
| **AskUser** | 작업 일시 중지, 사용자 확인 필요 |

### 특수 역할: 실리콘 큐레이터

실리콘 큐레이터는 최고 권한 수준을 보유합니다 (`IsCurator = true`). 큐레이터의 권한 검사는 사용자가 명시적으로 재정의하지 않는 한 **Allowed**로 단락됩니다.

### 전용 PermissionManager

각 실리콘 비잉은 자체 **전용 PermissionManager** 인스턴스를 가집니다. 권한 상태는 being 간에 공유되지 않습니다.

---

## 권한 검증 흐름

쿼리 우선순위: **1. 사용자 HighDeny → 2. 사용자 HighAllow → 3. 콜백 함수**

```
┌─────────────┐
│ Tool Call   │
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Executor   │────▶│ 전용 Permission     │
│ (Disk/Net/  │     │ Manager (being별)   │
│  CLI/...)   │     └────────┬────────────┘
└─────────────┘            │
                           ▼
                  ┌─────────────────┐
                  │ 1. IsCurator?   │──Yes──▶ Allowed
                  └────────┬────────┘
                           │ No
                           ▼
                  ┌─────────────────┐
                  │ 2. 사용자 HighDeny│──일치──▶ Denied
                  │ (메모리 캐시)    │
                  └────────┬────────┘
                           │ 불일치
                           ▼
                  ┌─────────────────┐
                  │ 3. 사용자 HighAllow│──일치──▶ Allowed
                  │ (메모리 캐시)    │
                  └────────┬────────┘
                           │ 불일치
                           ▼
                  ┌─────────────────┐
                  │ 4. 권한         │
                  │  콜백 함수      │──▶ Allowed / Denied / AskUser
                  └─────────────────┘
```

**핵심**: executor는 boolean (Allowed/Denied)만 봅니다. PermissionManager는 내부적으로 3-way 결정 (Allowed/Denied/AskUser)을 처리하고 executor에 반환하기 전 AskUser를 해결합니다.

---

## Executors (보안 경계)

Executor는 I/O 작업의 **유일한** 경로입니다. 다음을 적용합니다:

### 독립 스케줄링 스레드

각 executor는 **독립 스케줄링 스레드**를 소유합니다:

- 스레드는 executor 간 격리 — 한 executor의 스레드 차단이 다른 executor에 영향 없음.
- 각 executor는 독립적인 리소스 제한 (CPU, 메모리 등) 설정 가능.
- executor 스레드를 위한 스레드 풀 관리.

### 요청 큐

각 executor는 요청 큐를 유지합니다:

- 요청은 타입별 해당 executor로 라우팅.
- 우선순위 큐 지원.
- 요청별 타임아웃 제어.

### 권한 검증을 위한 스레드 잠금

도구가 리소스 접근 시작 시:

1. executor가 요청을 받고 **스레드 잠금**.
2. executor가 being의 전용 PermissionManager 쿼리.
3. 콜백이 AskUser 반환 시, executor 스레드는 **잠금 상태 유지**하며 사용자 응답 대기.
4. being은 최종 결과 (성공 또는 거부)만 봄 — 중간 "대기 중" 또는 "기다리는" 상태 절대 보지 않음.
5. 실리콘 큐레이터만 실제 사용자 프롬프트 트리거. 일반 being은 블로킹 없이 전역 ACL 동기 쿼리.
6. 타임아웃 시, 요청은 거부된 것으로 처리되고 스레드 잠금 해제.

### Executor 타입

| Executor | 범위 | 기본 타임아웃 |
|----------|------|---------------|
| `DiskExecutor` | 파일 읽기/쓰기, 디렉토리 작업 | 30초 |
| `NetworkExecutor` | HTTP 요청, WebSocket 연결 | 60초 |
| `CommandLineExecutor` | 셸 명령 실행 | 120초 |
| `DynamicCompilationExecutor` | Roslyn 메모리 컴파일 | 60초 |

### 예외 격리 및 내고장성

- 한 executor의 예외가 다른 executor에 영향 없음.
- 스레드 크래시 시 자동 재시작.
- Circuit breaker: 연속 실패 후 executor 일시 중지로 연쇄 실패 방지.

---

## 전역 ACL (접근 제어 목록)

저장소에 저장된 공유 규칙 테이블, 실리콘 큐레이터가 독점 관리:

```json
{
  "rules": [
    { "prefix": "network:api.github.com", "result": "Allowed" },
    { "prefix": "file:C:\\Windows", "result": "Denied" },
    { "prefix": "cli:rm -rf", "result": "Denied" }
  ]
}
```

- 규칙은 순서대로 평가; 첫 일치 승리.
- 실리콘 큐레이터만 전역 ACL 수정 가능 (전용 도구 통해).
- 변경 즉시 적용.
- 전역 ACL은 위의 쿼리 우선순위 체인에 **없음** — 콜백 함수가 내부적으로 참조.

---

## 사용자 빈도 캐시

반복적인 권한 프롬프트 감소를 위해 시스템은 두 개의 **being별, 메모리 전용** 캐시를 유지합니다:

| 캐시 | 목적 |
|------|------|
| **HighAllow** | 사용자가 자주 허용한 리소스 |
| **HighDeny** | 사용자가 자주 거부한 리소스 |

### 작동 방식

- **사용자 선택, 자동 감지 아님**: AskUser 트리거 시, 사용자가 캐시에 리소스 추가 여부 선택.
- **접두사 일치**: 리소스 경로 접두사 일치 지원 (예: `network:api.example.com/*`).
- **우선순위**: HighDeny가 HighAllow보다 우선순위 높음.
- **메모리 전용**: 캐시가 저장되지 않음. 재시작 시 손실.
- **구성 가능한 만료**: 사용자가 캐시 항목 유효 기간 설정 가능.

### 캐시 업데이트 흐름

1. 권한 콜백이 `AskUser` 반환.
2. 권한 시스템이 카드 시스템 (Web UI 또는 IM)에 문의 전송.
3. 사용자가 결정 (허용/거부) 및 **캐시 여부 선택**.
4. 카드 시스템이 결정 + 캐시 플래그 반환.
5. 권한 시스템이 해당 캐시 목록 업데이트.
6. 캐시된 접두사와 일치하는 향후 요청 즉시 해결.

---

## 사용자 질문 메커니즘

권한 검사가 `AskUser` 반환 시:

### Web UI: 대화형 카드

웹 프론트엔드가 즉시 **대화형 카드** 표시:

- 리소스 타입 및 경로
- 작업 설명
- 허용 / 거부 버튼
- 선택적 "항상 허용" / "항상 거부" 체크박스 (빈도 캐시에 추가)

### IM (카드 지원 안 함): 랜덤 코드

대화형 카드를 지원하지 않는 메시징 플랫폼:

1. 시스템이 두 개의 6자리 랜덤 코드 생성: **허용 코드** 및 **거부 코드**.
2. 리소스 정보와 두 코드가 포함된 메시지 전송.
3. 사용자가 승인하려면 정확한 허용 코드로 회신해야 함. 다른 회신은 거부로 처리.
4. 코드는 일회용으로 리플레이 공격 방지.

### 타임아웃

- 모든 AskUser 요청에 타임아웃 설정.
- 타임아웃 시, 요청은 **거부**로 처리되고 executor 스레드 잠금 해제.

---

## 동적 컴파일 보안

자기 진화 (클래스 재작성)는 고유한 보안 위험을 초래합니다. 시스템은 **계층적 전략**으로 완화:

### 1단계: 컴파일 타임 참조 제어 (주요 방어)

- 컴파일러에는 **허용된 어셈블리 참조 목록**만 제공.
- **허용**: `System.Runtime`, `System.Private.CoreLib`, 프로젝트 어셈블리 (ITool 인터페이스 등)
- **차단**: `System.IO`, `System.Reflection`, `System.Runtime.InteropServices` 등.
- 코드가 차단된 어셈블리 참조 시, **컴파일러 자체가** 코드 거부.
- 런타임 스캔보다 신뢰성 높음 — 위험한 작업이 타입 레벨에서 불가능.

### 2단계: 런타임 정적 분석 (보조 방어)

- 성공적인 컴파일 후에도 코드가 정적 패턴 스캔 적용.
- 위험한 작업 패턴 감지 (직접 I/O, 시스템 호출 등).
- 위험한 코드 발견 시, 로딩 거부 및 기본 기능으로 폴백.

### 상속 제약

모든 사용자 정의 실리콘 비잉 클래스는 **반드시** `SiliconBeingBase` 상속. 컴파일러가 타입 레벨에서 강제.

### 암호화된 저장

컴파일된 코드가 디스크에 AES-256 암호화로 저장:

- **키 파생**: being GUID (대문자)에서 PBKDF2.
- **복호화 실패**: 기본 구현으로 폴백.
- **런타임 재컴파일**: 새 코드가 메모리에서 먼저 컴파일; 성공적인 컴파일 및 인스턴스 교체 후에만 저장.

### 원자적 교체

교체 프로세스는 원자적:

1. 메모리에서 새 코드 컴파일 → `Type` 획득.
2. `Type`에서 새 인스턴스 생성.
3. 이전 인스턴스에서 새 인스턴스로 상태 마이그레이션.
4. 참조 교체.
5. 암호화된 코드 저장.

단계 실패 시, 이전 인스턴스 활성 상태 유지.

---

## 권한 콜백 함수

### 설계

각 PermissionManager는 **콜백 함수 변수** 보유:

- **기본**: 내장 기본 권한 함수 가리킴.
- **동적 컴파일 후**: being의 사용자 정의 권한 함수로 재정의.
- **둘 중 하나**: 언제든지 오직 하나의 콜백만 활성.
- **컴파일 실패**: 현재 콜백에 영향 없음 — 기본 또는 마지막 성공한 사용자 정의 함수 유지.

### 콜백 시그니처

```
PermissionResult Callback(PermissionType type, string resourcePath, Guid callerId)
```

`Allowed`, `Denied` 또는 `AskUser` 반환.

---

## 감사 로깅

모든 권한 결정 기록:

```
[2026-04-01 15:30:25] ALLOWED  | Being:AssistantA | Type:NetworkAccess | Resource:api.github.com | Source:HighAllowCache
[2026-04-01 15:30:26] DENIED   | Being:AssistantB | Type:FileAccess    | Resource:C:\Windows\System32 | Source:HighDenyCache
[2026-04-01 15:30:27] ASK_USER | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:Callback
[2026-04-01 15:30:28] ALLOWED  | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:UserDecision
```

로그가 저장소에 저장되며 Web UI (Log Controller)에서 볼 수 있음.

---

## 토큰 사용량 감사

`TokenUsageAuditManager`는 AI 토큰 소비의 보안 관련 추적 제공:

- **요청별 기록** — 모든 AI 호출이 being ID, 모델, 프롬프트 토큰, 완성 토큰, 타임스탬프 로깅.
- **이상 감지** — 비정상적인 토큰 소비 패턴이 프롬프트 삽입 또는 리소스 남용 표시 가능.
- **Curator 전용 접근** — `TokenAuditTool` (`[SiliconManagerOnly]` 표시)이 Curator에게 토큰 사용량 쿼리 및 요약 허용.
- **웹 대시보드** — `AuditController`가 추세 차트 및 데이터 내보내기를 갖춘 브라우저 기반 대시보드 제공.
- **영구 저장** — 기록이 시간 시리즈 쿼리 및 장기 분석을 위해 `ITimeStorage`를 통해 저장.
