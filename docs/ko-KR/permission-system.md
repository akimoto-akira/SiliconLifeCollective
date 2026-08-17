# 퍼미션 시스템

> **버전: v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | **한국어** | [Čeština](../cs-CZ/permission-system.md) | [Русский](../ru-RU/permission-system.md)

## 개요

퍼미션 시스템은 모든 AI가 시작한 작업이 적절하게 검증되고 감사되도록 보장합니다.

## 퍼미션 검증 체인

```
┌─────────────────────────────────────────────┐
│          퍼미션 검증                         │
├─────────────────────────────────────────────┤
│  레벨 1: UserFrequencyCache                  │
│  ↓ 고빈도 사용자 결정 캐시 (HighDeny/HighAllow)│
│  레벨 2: IPermissionCallback                 │
│  ↓ 맞춤형 로직 (Allowed/Denied/AskUser)      │
│  레벨 3: IsCurator?                          │
│  ↓ 예 → IPermissionAskHandler (사용자에게 문의)│
│  ↓ 아니오 → GlobalACL → 기본 거부            │
│  결과: 허용 또는 거부                        │
└─────────────────────────────────────────────┘
```

> **참고**: `PermissionManager.CheckPermission()`의 실제 쿼리 우선순위는 다음과 같습니다:
> 1. **UserFrequencyCache** — 고빈도 사용자 결정 캐시를 먼저 확인
> 2. **IPermissionCallback** — 맞춤형 콜백 규칙 평가
> 3. **큐레이터 분기** — 콜백이 AskUser를 반환하거나 콜백이 없는 경우:
>    - **큐레이터** → `IPermissionAskHandler` (IM을 통해 사용자에게 문의)
>    - **비큐레이터** → `GlobalACL` → 기본 거부

## 레벨 1: UserFrequencyCache

각 비잉의 고빈도 사용자 결정 캐시(HighDeny/HighAllow)로, 메모리에만 존재합니다.

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny가 HighAllow보다 우선**
- **메모리 전용**: 캐시는 영속화되지 않으며, 재시작 시 손실
- **구성 가능한 만료 시간**: 사용자가 캐시 항목의 유효 기간을 설정 가능

## 레벨 2: IPermissionCallback

동적 퍼미션 로직을 위한 맞춤형 콜백입니다.

### DefaultPermissionCallback 기본 구현

`DefaultPermissionCallback`은 다음을 포함한 포괄적인 기본 퍼미션 규칙을 제공합니다:

#### 네트워크 접근 규칙
- **루프백 주소**: localhost, 127.0.0.1, ::1 허용
- **사설 IP 주소**:
  - 192.168.x.x (Class C) - 허용
  - 10.x.x.x (Class A) - 허용
  - 172.16-31.x.x (Class B) - 사용자에게 문의
- **도메인 화이트리스트**:
  - 검색엔진: Google, Bing, DuckDuckGo, Yandex, Sogou 등
  - AI 서비스: OpenAI, Anthropic, HuggingFace, Ollama 등
  - 개발자 서비스: GitHub, StackOverflow, npm, NuGet 등
  - 소셜 미디어: 웨이보, 즈후, Reddit, Discord 등
  - 비디오 플랫폼: YouTube, Bilibili, 더우인, TikTok 등
  - **날씨 정보**: wttr.in
  - 정부 웹사이트: .gov, .go.jp, .go.kr
- **도메인 블랙리스트**:
  - AI 사칭 웹사이트: chatgpt, openai, deepseek 등 사칭 도메인
  - 악성 AI 도구: wormgpt, darkgpt, fraudgpt 등
  - AI 콘텐츠 팜 및 블랙 마켓 관련 도메인

```csharp
public class DefaultPermissionCallback : IPermissionCallback
{
    public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
    {
        if (IsSafeOperation(permissionType, resource))
        {
            return PermissionResult.Allowed;
        }

        return PermissionResult.AskUser;
    }
}
```

## 레벨 3: 분기 판정 (IsCurator / GlobalACL)

콜백이 `AskUser`를 반환하거나 콜백이 구성되지 않은 경우, 시스템은 큐레이터 신원에 따라 분기합니다:

### 큐레이터 분기 (IsCurator = true)

실리콘 큐레이터의 경우, 시스템은 인스턴트 메시지를 통해 사용자에게 결정을 요청합니다:

```csharp
if (IsCurator)
{
    if (_askHandler != null)
    {
        AskPermissionResult userDecision = _askHandler.AskUser(callerId, permissionType, resource);
        // 사용자가 Web UI에서 확인 또는 거부
    }
}
```

### 비큐레이터 분기 (IsCurator = false)

비큐레이터 비잉의 경우, 시스템은 글로벌 ACL을 확인합니다. 일치하는 규칙이 없으면 기본적으로 요청을 거부합니다.

### GlobalACL 구조

```json
{
  "rules": [
    {
      "permissionType": "NetworkAccess",
      "resourcePrefix": "api.github.com",
      "result": "Allowed"
    },
    {
      "permissionType": "FileAccess",
      "resourcePrefix": "C:\\Windows",
      "result": "Denied"
    }
  ]
}
```

규칙은 순서대로 평가되며, 첫 번째로 일치하는 규칙이 적용됩니다. 실리콘 큐레이터만 글로벌 ACL을 수정할 수 있습니다.

### 리소스 형식

```
{type}:{path}

예시:
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## IPermissionAskHandler

큐레이터 작업에 사용자 확인이 필요한 경우, `IPermissionAskHandler`를 통해 사용자에게 퍼미션을 문의합니다.

### IMPermissionAskHandler 구현

`IMPermissionAskHandler`는 Web UI를 통해 사용자에게 퍼미션 요청을 전송합니다:

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        // 인스턴트 메시지로 사용자에게 메시지 전송
        SendMessageAsync($"Allow {resource}?");

        // 사용자 응답 대기
        var response = WaitForResponseAsync();

        return response.Approved
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

### PermissionRequestQueue 퍼미션 요청 큐

`PermissionRequestQueue`는 대기 중인 퍼미션 요청을 관리하며, 사용자 응답을 비동기적으로 대기합니다:

- **요청 인큐** — 퍼미션 체인이 레벨 5에 도달하면 `TaskCompletionSource<AskPermissionResult>`를 생성하여 인큐
- **Web UI 표시** — `PermissionRequestController`를 통해 Web UI에 대기 중인 퍼미션 요청 표시
- **사용자 응답** — 사용자가 Web UI에서 승인 또는 거부, 선택적으로 결정 캐시 및 캐시 지속 시간 설정 가능
- **캐시 옵션** — 사용자는 퍼미션 결정을 1시간, 24시간, 7일 또는 30일 동안 캐시 가능
- **타임아웃 메커니즘** — 60초 무응답 시 자동으로 요청 페이지 닫힘

## 감사 시스템

모든 퍼미션 결정이 기록됩니다:

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "callerId": "being-uuid",
  "permissionType": "FileAccess",
  "resource": "C:\\data\\config.json",
  "result": "Allowed",
  "reason": "Global ACL"
}
```

## 프로그래매틱 퍼미션 평가

### EvaluatePermission API

`PermissionManager.EvaluatePermission()` 메서드는 사용자 프롬프트를 트리거하지 않는 읽기 전용 퍼미션 사전 평가를 제공합니다. `PermissionTool`은 이 메서드를 사용하여 AI가 작업 시도 전 퍼미션 상태를 확인합니다.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**반환값**: 3상태 `PermissionResult`:
- `Allowed` - 작업 허용
- `Denied` - 작업 거부
- `AskUser` - 실행 시 사용자 확인 필요

**평가 순서**:
1. **빈도 캐시** - 캐시된 사용자 결정 확인
2. **IPermissionCallback** - 맞춤형 콜백 평가
3. **큐레이터 상태** - 큐레이터인 경우 `AskUser` 반환 (확인 필요)
4. **글로벌 ACL** - 접근 제어 규칙 확인
5. **기본값** - 일치하는 규칙 없을 시 거부

> **참고**: 전체 퍼미션 체인과 달리, `EvaluatePermission`은 `IPermissionAskHandler`를 호출하지 **않습니다**. 실행 시 결과가 *무엇일지*만 보고합니다.

## 퍼미션 관리

### 퍼미션 부여

**Web UI를 통해**:
1. **퍼미션 관리**로 이동
2. **규칙 추가** 클릭
3. 설정:
   - 사용자
   - 리소스
   - 허용/거부
   - 지속 시간

**API를 통해**:
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "permissionType": "FileAccess",
    "resourcePrefix": "C:\\Projects",
    "result": "Allowed",
    "description": "Allow project directory access"
  }'
```

### 퍼미션 취소

Web UI의 퍼미션 관리 페이지에서 조작합니다.

### 퍼미션 보기

```bash
curl http://localhost:8080/api/permissions/list
```

## 툴 퍼미션 시스템

작업 레벨의 퍼미션 검증 체인 외에도, 시스템은 실리콘 비잉이 사용할 수 있는 툴을 제어하기 위한 **툴 퍼미션** 관리 메커니즘을 제공합니다.

### 2단계 툴 퍼미션

툴 퍼미션은 두 가지 레벨로 나뉩니다:

1. **실리콘 비잉 레벨** — 개별 실리콘 비잉이 사용할 수 있는 툴 작업을 제어
2. **프로젝트 레벨** — 프로젝트 공간 내에서 사용 가능한 툴 작업을 제어하며, 실리콘 비잉 레벨 퍼미션과 독립적

### 툴 퍼미션 구성

각 툴의 각 작업은 독립적으로 허용 또는 거부로 구성할 수 있습니다:

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "network:get": "allowed",
    "network:post": "denied",
    "disk:read": "allowed",
    "disk:write": "denied",
    "database:query": "allowed"
  }
}
```

### 퍼미션 템플릿

시스템은 실리콘 비잉에 빠르게 적용할 수 있는 사전 정의된 툴 퍼미션 템플릿을 제공합니다:

- **readonly** — 읽기 전용 퍼미션 (읽기 작업 허용, 쓰기 작업 거부)
- **full** — 전체 퍼미션 (모든 작업 허용)
- **restricted** — 제한된 퍼미션 (기본 작업만 허용)

### Web UI 관리

Web UI를 통해 툴 퍼미션 관리:

- **실리콘 비잉 툴 퍼미션 페이지** — `/beings/tool-permissions`
- **프로젝트 툴 퍼미션 페이지** — `/project/{id}/tool-permissions`

### API 엔드포인트

| 엔드포인트 | 메서드 | 설명 |
|------|------|------|
| `/api/beings/tool-permissions` | GET | 실리콘 비잉 툴 퍼미션 조회 |
| `/api/beings/tool-permissions` | PUT | 실리콘 비잉 툴 퍼미션 업데이트 |
| `/api/beings/tool-permissions/templates` | GET | 퍼미션 템플릿 목록 조회 |
| `/api/beings/tool-permissions/apply-template` | POST | 퍼미션 템플릿 적용 |
| `/api/projects/{id}/tool-permissions` | GET | 프로젝트 툴 퍼미션 조회 |
| `/api/projects/{id}/tool-permissions` | PUT | 프로젝트 툴 퍼미션 업데이트 |

### 스킬 액션 권한

스킬은 도구 액션 권한 메커니즘을 재사용: 스킬 id가 도구 이름으로 사용되고, 액션은 `execute`.

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "daily_news_digest:execute": "denied",
    "code_review:execute": "allowed"
  }
}
```

- 비활성화된 스킬은 AI에게 보이는 도구 정의에 나타나지 않음(AI가 전혀 "볼 수" 없음)
- 스킬 실행 시 런타임 재검증이 있으며, 오래된 스키마도 우회할 수 없음
- 스킬 내부 도구 권한 = 비잉 권한 ∪ 스킬 자체 제한(제한 측의 교집합, 좁히기만 가능하고 확장 불가)

### MCP 래핑 도구 액션 권한

MCP 서버가 주입하는 각 래핑 도구(`mcp_{serverId}_{toolName}`)는 단일 `execute` 액션을 자동 선언:

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "mcp_filesystem_read_file:execute": "denied",
    "mcp_github_create_issue:execute": "allowed"
  }
}
```

- 비잉 또는 프로젝트별로 외부 도구의 가용성을 정밀하게 제어 가능
- 서버의 모든 `execute` 액션이 거부되면, 해당 도구는 AI에게 보이는 스키마에서 전체 제거
- 서버 비활성화/삭제(Web UI 조작) 시 모든 도구가 즉시 등록 해제

---

## 모범 사례

### 1. 최소 권한 원칙

필요한 최소 퍼미션만 부여:

```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects\\MyApp\\config.json",
  "result": "Allowed"
}
```

### 2. 시간 제한 퍼미션 사용

절대적으로 필요하지 않은 한 영구 퍼미션을 부여하지 마세요.

### 3. 퍼미션 로그 모니터링

정기적으로 감사 로그를 확인하여 다음 사항을 파악:
- 거부된 접근 시도
- 비정상 패턴
- 퍼미션 권한 상승

### 4. 맞춤형 콜백 구현

복잡한 로직의 경우 `IPermissionCallback` 사용:

```csharp
public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
{
    // 시간 기반 퍼미션
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }

    // 리소스 기반 퍼미션
    if (IsSensitiveResource(resource))
    {
        return PermissionResult.AskUser;
    }

    return PermissionResult.Allowed;
}
```

## 일반 시나리오

### 시나리오 1: AI가 파일을 읽으려는 경우

```
AI: "config.json을 읽어야 합니다"
↓
퍼미션 체인:
1. UserFrequencyCache? 캐시된 결정 없음
2. IPermissionCallback? AskUser 반환 (명시적으로 허용되지 않음)
3. IsCurator? 아니오 → GlobalACL 확인
4. GlobalACL? 규칙 발견: file:... = Allowed
5. 결과: 허용
```

### 시나리오 2: AI가 코드를 실행하려는 경우

```
AI: "코드를 컴파일하고 실행하고 싶습니다"
↓
퍼미션 체인:
1. UserFrequencyCache? 캐시된 결정 없음
2. IPermissionCallback? AskUser 반환
3. IsCurator? 예 → IPermissionAskHandler
4. 사용자 승인
5. 결과: 허용
```

### 시나리오 3: 캐시된 거부

```
AI: "C:\Windows에 접근해야 합니다"
↓
퍼미션 체인:
1. UserFrequencyCache? HighDeny 캐시에서 발견
2. 결과: 거부 (추가 확인 없음)
```

## 문제 해결

### 예기치 않은 퍼미션 거부

**확인**:
1. 사용자의 IsCurator 상태
2. 빈도 캐시의 HighDeny 항목
3. GlobalACL 규칙
4. 콜백 로직
5. 사용자 응답 타임아웃

### 퍼미션이 만료되지 않는 경우

**확인**:
- `expiresAt` 필드가 올바르게 설정됨
- 시간대가 정확함
- 클록 동기화

### 감사 로그가 기록되지 않는 경우

**확인**:
- 감사 로거가 등록됨
- 스토리지 백엔드에 접근 가능
- 디스크 공간 충분

## 다음 단계

- 📚 [아키텍처 가이드](architecture.md) 읽기
- 🛠️ [개발 가이드](development-guide.md) 보기
- 🔒 [보안 문서](security.md) 보기
- 🚀 [빠른 시작 가이드](getting-started.md) 보기
