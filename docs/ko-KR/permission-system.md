# 권한 시스템

## 개요

권한 시스템은 AI가 시작한 모든 작업이 적절하게 검증되고 감사되도록 보장합니다.

## 5단계 권한 체인

```
┌─────────────────────────────────────────────┐
│          권한 검증                           │
├─────────────────────────────────────────────┤
│  1단계: IsCurator                           │
│  ↓ true이면 우회                            │
│  2단계: UserFrequencyCache                  │
│  ↓ 속도 제한                                │
│  3단계: GlobalACL                           │
│  ↓ 접근 제어 목록                           │
│  4단계: IPermissionCallback                 │
│  ↓ 사용자 정의 로직                         │
│  5단계: IPermissionAskHandler               │
│  ↓ 사용자에게 질문                          │
│  결과: 허용 또는 거부                       │
└─────────────────────────────────────────────┘
```

## 1단계: IsCurator

관리자/curator는 모든 권한 검사를 우회합니다.

```csharp
if (user.IsCurator)
{
    return PermissionResult.Allowed("Curator access");
}
```

## 2단계: UserFrequencyCache

남용을 방지하기 위해 사용자별 속도 제한.

```csharp
var cache = new UserFrequencyCache();
if (!cache.CheckLimit(userId, resource))
{
    return PermissionResult.Denied("Rate limit exceeded");
}
```

## 3단계: GlobalACL

전역 접근 제어 목록은 명시적 규칙을 정의합니다.

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

## 4단계: IPermissionCallback

동적 권한 로직을 위한 사용자 정의 콜백.

### DefaultPermissionCallback 기본 구현

`DefaultPermissionCallback`은 포괄적인 기본 권한 규칙을 제공합니다. 포함 내용:

#### 네트워크 액세스 규칙
- **루프백 주소**: localhost, 127.0.0.1, ::1 허용
- **개인 IP 주소**:
  - 192.168.x.x (Class C) - 허용
  - 10.x.x.x (Class A) - 허용
  - 172.16-31.x.x (Class B) - 사용자에게 확인
- **도메인 화이트리스트**:
  - 검색 엔진: Google, Bing, DuckDuckGo, Yandex, Sogou 등
  - AI 서비스: OpenAI, Anthropic, HuggingFace, Ollama 등
  - 개발자 서비스: GitHub, StackOverflow, npm, NuGet 등
  - 소셜 미디어: Weibo, Zhihu, Reddit, Discord 등
  - 비디오 플랫폼: YouTube, Bilibili, Douyin(틱톡), TikTok 등
  - **날씨 정보**: wttr.in
  - 정부 웹사이트: .gov, .go.jp, .go.kr
- **도메인 블랙리스트**:
  - AI 사칭 사이트: chatgpt, openai, deepseek 등 (가짜 도메인)
  - 악의적인 AI 도구: wormgpt, darkgpt, fraudgpt 등
  - AI 콘텐츠 팜 및 블랙마켓 관련 도메인

```csharp
public class DefaultPermissionCallback : IPermissionCallback
{
    public async Task<PermissionResult> CheckAsync(PermissionRequest request)
    {
        // 여기에 사용자 정의 로직
        if (IsSafeOperation(request))
        {
            return PermissionResult.Allowed("Safe operation");
        }
        
        return PermissionResult.Undecided("Needs user confirmation");
    }
}
```

## 5단계: IPermissionAskHandler

다른 모든 단계가 결정되지 않았을 때 사용자에게 권한을 요청합니다.

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public async Task<AskPermissionResult> AskAsync(PermissionRequest request)
    {
        // IM을 통해 사용자에게 메시지 전송
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

## 프로그래마틱 권한 평가

### EvaluatePermission API

`PermissionManager.EvaluatePermission()` 메서드는 사용자 프롬프트를 트리거하지 않고 권한의 읽기 전용 사전 평가를 제공합니다. `PermissionTool`은 이 메서드를 사용하여 AI가 작업을 시도하기 전에 권한 상태를 확인할 수 있습니다.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**반환값**: 3상태 `PermissionResult`:
- `Allowed` - 작업이 허용됨
- `Denied` - 작업이 차단됨
- `AskUser` - 실행 시 사용자 확인이 필요

**평가 순서**:
1. **빈도 캐시** - 캐시된 사용자 결정 확인
2. **IPermissionCallback** - 사용자 정의 콜백 평가
3. **Curator 상태** - curator인 경우 `AskUser` 반환 (확인 필요)
4. **전역 ACL** - 접근 제어 규칙 확인
5. **기본값** - 일치하는 규칙이 없으면 거부

> **참고**: 완전한 권한 체인과 달리 `EvaluatePermission`은 `IPermissionAskHandler`를 호출하지 **않습니다**. 실행 시 결과가 *어떻게 될지*만 보고합니다.

## 권한 관리

### 권한 부여

**Web UI를 통해**:
1. **권한 관리**로 이동
2. **규칙 추가** 클릭
3. 설정:
   - 사용자
   - 리소스
   - 허용/거부
   - 기간

**API를 통해**:
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

### 1. 최소 권한의 원칙

필요한 최소 권한만 부여:

```json
{
  "resource": "disk:read",  // disk:* 아님
  "allowed": true,
  "expiresAt": "2026-04-21T00:00:00Z"  // 항상 만료 설정
}
```

### 2. 시간 제한 권한 사용

절대적으로 필요한 경우가 아니면 영구 권한을 부여하지 마십시오.

### 3. 권한 로그 모니터링

감사 로그를 정기적으로 검토:
- 거부된 접근 시도
- 비정상적인 패턴
- 권한 승격

### 4. 사용자 정의 콜백 구현

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

## 일반적인 시나리오

### 시나리오 1: AI가 파일 읽기 원함

```
AI: "config.json을 읽어야 합니다"
↓
권한 체인:
1. IsCurator? 아님
2. 속도 제한? 정상
3. GlobalACL? 규칙 발견: disk:read = 허용
4. 결과: 허용
```

### 시나리오 2: AI가 코드 실행 원함

```
AI: "코드를 컴파일하고 실행하고 싶습니다"
↓
권한 체인:
1. IsCurator? 아님
2. 속도 제한? 정상
3. GlobalACL? 규칙 없음
4. 콜백? 결정되지 않음 반환
5. 사용자에게 질문? 사용자 승인
6. 결과: 허용
```

### 시나리오 3: 속도 제한 초과

```
AI: "100개의 HTTP 요청을 해야 합니다"
↓
권한 체인:
1. IsCurator? 아님
2. 속도 제한? 초과
3. 결과: 거부
```

## 문제 해결

### 예상치 못한 권한 거부

**확인**:
1. 사용자의 IsCurator 상태
2. 속도 제한 설정
3. GlobalACL 규칙
4. 콜백 로직
5. 사용자 응답 타임아웃

### 권한이 만료되지 않음

**확인**:
- `expiresAt` 필드가 올바르게 설정됨
- 시간대가 올바름
- 시계 동기화

### 감사 로그가 기록되지 않음

**확인**:
- 감사 로거가 등록됨
- 저장소 백엔드에 접근 가능
- 충분한 디스크 공간

## 다음 단계

- 📚 [아키텍처 가이드](architecture.md) 읽기
- 🛠️ [개발 가이드](development-guide.md) 확인
- 🔒 [보안 문서](security.md) 검토
- 🚀 [시작 가이드](getting-started.md) 참조
