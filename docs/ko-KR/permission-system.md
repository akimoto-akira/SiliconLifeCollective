# 권한 시스템

> **버전: v0.1.0-alpha**

[English](../en/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | **한국어** | [Deutsch](../de-DE/permission-system.md) | [Čeština](../cs-CZ/permission-system.md)

## 개요

권한 시스템은 모든 AI 시작 작업이 적절하게 검증되고 감사되도록 보장합니다.

## 5단계 권한 체인

```
┌─────────────────────────────────────────────┐
│          권한 검증                           │
├─────────────────────────────────────────────┤
│  레벨 1: IsCurator                           │
│  ↓ 참이면 우회                               │
│  레벨 2: UserFrequencyCache                  │
│  ↓ 속도 제한                                 │
│  레벨 3: GlobalACL                           │
│  ↓ 접근 제어 목록                            │
│  레벨 4: IPermissionCallback                 │
│  ↓ 맞춤형 로직                               │
│  레벨 5: IPermissionAskHandler               │
│  ↓ 사용자에게 문의                           │
│  결과: 허용 또는 거부                        │
└─────────────────────────────────────────────┘
```

## 레벨 1: IsCurator

관리자/큐레이터는 모든 권한 검사를 우회합니다.

```csharp
if (user.IsCurator)
{
    return PermissionResult.Allowed("Curator access");
}
```

## 레벨 2: UserFrequencyCache

남용 방지를 위한 사용자별 속도 제한.

```csharp
var cache = new UserFrequencyCache();
if (!cache.CheckLimit(userId, resource))
{
    return PermissionResult.Denied("Rate limit exceeded");
}
```

## 레벨 3: GlobalACL

명확한 규칙을 정의하는 전역 접근 제어 목록.

### ACL 구조

```json
{
  "rules": [
    {
      "userId": "user-uuid",
      "resource": "disk:read",
      "allowed": true,
      "expiresAt": "2026-04-21T00:00:00Z"
    }
  ]
}
```

### 리소스 형식

```
{type}:{action}

예시:
- disk:read
- disk:write
- network:http
- compile:execute
- system:info
```

## 레벨 4: IPermissionCallback

동적 권한 로직을 위한 맞춤형 콜백.

### DefaultPermissionCallback 기본 구현

`DefaultPermissionCallback`은 다음을 포함한 포괄적인 기본 권한 규칙을 제공합니다:

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
  - AI 사칭 웹사이트: chatgpt, openai, deepseek 등 피싱 도메인
  - 악성 AI 도구: wormgpt, darkgpt, fraudgpt 등
  - AI 콘텐츠 팜 및 블랙 마켓 관련 도메인

```csharp
public class DefaultPermissionCallback : IPermissionCallback
{
    public async Task<PermissionResult> CheckAsync(PermissionRequest request)
    {
        // 맞춤형 로직
        if (IsSafeOperation(request))
        {
            return PermissionResult.Allowed("Safe operation");
        }
        
        return PermissionResult.Undecided("Needs user confirmation");
    }
}
```

## 레벨 5: IPermissionAskHandler

다른 모든 레벨이 미결정일 때 사용자에게 권한 문의.

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public async Task<AskPermissionResult> AskAsync(PermissionRequest request)
    {
        // 인스턴트 메시지로 사용자에게 메시지 전송
        await SendMessageAsync($"Allow {request.Resource}?");
        
        // 사용자 응답 대기
        var response = await WaitForResponseAsync();
        
        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

## 감사 시스템

모든 권한 결정이 기록됩니다:

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "level": "GlobalACL",
  "reason": "Explicit rule granted"
}
```

## 프로그래매틱 권한 평가

### EvaluatePermission API

`PermissionManager.EvaluatePermission()` 메서드는 사용자 프롬프트를 트리거하지 않는 읽기 전용 권한 사전 평가를 제공합니다. `PermissionTool`은 이 메서드를 사용하여 AI가 작업 시도 전 권한 상태를 확인합니다.

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
1. **주파수 캐시** - 캐시된 사용자 결정 확인
2. **IPermissionCallback** - 맞춤형 콜백 평가
3. **큐레이터 상태** - 큐레이터인 경우 `AskUser` 반환 (확인 필요)
4. **전역 ACL** - 접근 제어 규칙 확인
5. **기본값** - 일치하는 규칙 없을 시 거부

> **참고**: 전체 권한 체인과 달리, `EvaluatePermission`은 `IPermissionAskHandler`를 호출하지 **않습니다**. 실행 시 결과가 *무엇일지*만 보고합니다.

## 권한 관리

### 권한 부여

**Web UI 통해**:
1. **권한 관리**로 이동
2. **규칙 추가** 클릭
3. 설정:
   - 사용자
   - 리소스
   - 허용/거부
   - 지속 시간

**API 통해**:
```bash
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "user-uuid",
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

### 권한 취소

```bash
curl -X DELETE http://localhost:8080/api/permissions/{rule-id}
```

### 권한 보기

```bash
curl http://localhost:8080/api/permissions?userId=user-uuid
```

## 모범 사례

### 1. 최소 권한 원칙

필요한 최소 권한만 부여:

```json
{
  "resource": "disk:read",  // disk:* 아님
  "allowed": true,
  "expiresAt": "2026-04-21T00:00:00Z"  // 항상 만료 설정
}
```

### 2. 시간 제한 권한 사용

절대적으로 필요하지 않은 한 영구 권한 부여 금지.

### 3. 권한 로그 모니터링

정기적으로 감사 로그 확인:
- 거부된 접근 시도
- 비정상 패턴
- 권한 상승

### 4. 맞춤형 콜백 구현

복잡한 로직의 경우 `IPermissionCallback` 사용:

```csharp
public async Task<PermissionResult> CheckAsync(PermissionRequest request)
{
    // 시간 기반 권한
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied("Outside business hours");
    }
    
    // 리소스 기반 권한
    if (IsSensitiveResource(request.Resource))
    {
        return PermissionResult.Undecided("Requires approval");
    }
    
    return PermissionResult.Allowed();
}
```

## 일반 시나리오

### 시나리오 1: AI가 파일 읽기 원함

```
AI: "config.json을 읽어야 합니다"
↓
권한 체인:
1. IsCurator? 아니오
2. 속도 제한? 정상
3. GlobalACL? 규칙 찾음: disk:read = 허용
4. 결과: 허용
```

### 시나리오 2: AI가 코드 실행 원함

```
AI: "코드를 컴파일하고 실행하고 싶습니다"
↓
권한 체인:
1. IsCurator? 아니오
2. 속도 제한? 정상
3. GlobalACL? 규칙 없음
4. 콜백? 미결정 반환
5. 사용자에게 문의? 사용자 승인
6. 결과: 허용
```

### 시나리오 3: 속도 제한 초과

```
AI: "100개의 HTTP 요청을 보내야 합니다"
↓
권한 체인:
1. IsCurator? 아니오
2. 속도 제한? 이미 초과
3. 결과: 거부
```

## 문제 해결

### 예기치 않은 권한 거부

**확인**:
1. 사용자의 IsCurator 상태
2. 속도 제한 설정
3. GlobalACL 규칙
4. 콜백 로직
5. 사용자 응답 타임아웃

### 권한 만료 안 됨

**확인**:
- `expiresAt` 필드 올바르게 설정
- 시간대 정확
- 클록 동기화

### 감사 로그 기록 안 됨

**확인**:
- 감사 로거 등록됨
- 저장소 백엔드 접근 가능
- 디스크 공간 충분

## 다음 단계

- 📚 [아키텍처 가이드](architecture.md) 읽기
- 🛠️ [개발 가이드](development-guide.md) 보기
- 🔒 [보안 문서](security.md) 보기
- 🚀 [빠른 시작 가이드](getting-started.md) 보기
