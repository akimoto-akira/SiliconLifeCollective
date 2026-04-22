# 도구 레퍼런스

## 개요

도구 시스템은 AI 에이전트가 표준화된 인터페이스를 통해 외부 세계와 상호작용할 수 있게 합니다.

## 내장 도구

### 1. 캘린더 도구

**이름**: `calendar`

**설명**: 다른 캘린더 시스템 간 날짜 변환.

**매개변수**:
```json
{
  "date": "2026-04-20",
  "fromCalendar": "gregorian",
  "toCalendar": "chinese_lunar"
}
```

**지원 캘린더** (32개 시스템):
- 그레고리력, 중국 음력, 이슬람력, 히브리력
- 일본력, 페르시아력, 마야력, 티베트력
- 그리고 24개 더...

### 2. 채팅 도구

**이름**: `chat`

**설명**: 다른 being이나 사용자에게 메시지 전송.

**매개변수**:
```json
{
  "targetId": "being-uuid",
  "message": "Hello, let's collaborate"
}
```

### 3. 설정 도구

**이름**: `config`

**설명**: 시스템 설정 읽기 및 수정.

**매개변수**:
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

### 4. 디스크 도구

**이름**: `disk`

**설명**: 파일 시스템 작업 (읽기, 쓰기, 목록).

**매개변수**:
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

**필요 권한**: `disk:read`, `disk:write`

### 5. 동적 컴파일 도구

**이름**: `compile`

**설명**: C# 코드 동적 컴파일 및 실행.

**매개변수**:
```json
{
  "code": "public class Test { ... }",
  "references": ["System.Linq"]
}
```

**보안**: 코드 실행 전 스캔됨.

### 6. 메모리 도구

**이름**: `memory`

**설명**: being 메모리 저장 및 검색.

**매개변수**:
```json
{
  "action": "read",
  "key": "important_fact",
  "timeRange": {
    "start": "2026-04-01",
    "end": "2026-04-20"
  }
}
```

### 7. 네트워크 도구

**이름**: `network`

**설명**: HTTP 요청 생성.

**매개변수**:
```json
{
  "method": "GET",
  "url": "https://api.example.com/data",
  "headers": {}
}
```

**필요 권한**: `network:http`

### 8. 시스템 도구

**이름**: `system`

**설명**: 시스템 정보 가져오기.

**매개변수**:
```json
{
  "action": "info"
}
```

### 9. 작업 도구

**이름**: `task`

**설명**: being 작업 관리.

**매개변수**:
```json
{
  "action": "create",
  "description": "Review code",
  "priority": 5
}
```

### 10. 타이머 도구

**이름**: `timer`

**설명**: 타이머 생성 및 관리.

**매개변수**:
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true
}
```

### 11. 토큰 감사 도구

**이름**: `token_audit`

**설명**: 토큰 사용량 통계 쿼리.

**매개변수**:
```json
{
  "startDate": "2026-04-01",
  "endDate": "2026-04-20"
}
```

### 12. 권한 도구

**이름**: `permission`

**설명**: 실리콘 생명체의 권한을 관리. Curator 전용.

**액션**: `query_permission`, `manage_acl`

**매개변수** (query_permission):
```json
{
  "action": "query_permission",
  "being_id": "being-uuid",
  "permission_type": "network",
  "resource": "https://api.example.com"
}
```

**권한 유형**: `network`, `command`, `filesystem`, `function`, `data`

**반환값**: 3상태 결과 (`ALLOWED`, `DENIED`, `ASK_USER`), curator 상태 및 빈도 캐시 정보 포함.

**매개변수** (manage_acl):
```json
{
  "action": "manage_acl",
  "acl_action": "add_rule",
  "permission_type": "filesystem",
  "resource_prefix": "/data/",
  "acl_result": "allow",
  "description": "데이터 디렉토리 접근 허용"
}
```

**권한**: `IsCurator` 플래그 필요.

### 13. 코드 실행 도구

**이름**: `execute_code`

**설명**: 보안 스캔이 포함된 C# 코드 컴파일 및 실행. Curator 전용.

**액션**: `run_script`

**매개변수**:
```json
{
  "action": "run_script",
  "code": "return DateTime.Now.ToString();",
  "timeout": 30
}
```

**상세 설명**:
- 코드는 `ScriptExecutor` 클래스의 `Execute()` 메서드로 래핑됨
- 컴파일 전 보안 스캔 수행
- 구성 가능한 타임아웃 지원 (기본값: 30초)
- 실패 시 컴파일 오류 및 보안 위반 정보 반환

**권한**: `IsCurator` 플래그 필요.

---

## 도구 호출 흐름

```
┌──────────┐
│   AI     │ tool_calls 반환
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ 도구 찾기 및 검증
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ 권한 체인 확인
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ 작업 실행
└────┬─────────┘
     ↓
┌──────────┐
│   AI     │ 도구 결과 수신
└──────────┘
```

## 권한 검증

모든 도구는 5단계 권한 체인을 통과:

1. **IsCurator**: 관리자가 모든 검사 우회
2. **UserFrequencyCache**: 사용자별 속도 제한
3. **GlobalACL**: 접근 제어 목록
4. **IPermissionCallback**: 사용자 정의 콜백 로직
5. **IPermissionAskHandler**: 사용자에게 권한 질문

## 사용자 정의 도구 생성

### 1단계: ITool 구현

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    
    public string Description => "Does something useful";
    
    public ToolDefinition Definition => new ToolDefinition
    {
        Name = Name,
        Description = Description,
        Parameters = new Dictionary<string, object>
        {
            ["param1"] = new { type = "string", description = "Description" }
        }
    };
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        try
        {
            var param1 = call.Parameters["param1"]?.ToString();
            var result = await DoWork(param1);
            
            return new ToolResult
            {
                Success = true,
                Output = result
            };
        }
        catch (Exception ex)
        {
            return new ToolResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }
}
```

### 2단계: 프로젝트에 추가

`src/SiliconLife.Default/Tools/`에 도구 배치.

`ToolManager`가 리플렉션을 통해 자동으로 검색.

### 3단계: (선택사항) 관리자 전용 표시

```csharp
[SiliconManagerOnly]
public class AdminTool : ITool
{
    // curator만 접근 가능
}
```

## 모범 사례

### 1. 항상 매개변수 검증

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("Missing required parameter: required_param");
}
```

### 2. 우아하게 오류 처리

```csharp
try
{
    // 작업
}
catch (Exception ex)
{
    Logger.Error($"Tool {Name} failed: {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. 권한 존중

권한 시스템 우회 금지. 항상 사용:

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return ToolResult.Denied(permission.Reason);
}
```

### 4. 명확한 설명 제공

AI가 도구를 언제 어떻게 사용할지 이해하도록 도움:

```csharp
public string Description => 
    "Use this tool to convert dates between calendar systems. " +
    "Requires 'date', 'fromCalendar', and 'toCalendar' parameters.";
```

## 도구 테스트

### 단위 테스트 예시

```csharp
[TestMethod]
public async Task CalendarTool_ConvertDate_ReturnsCorrectResult()
{
    var tool = new CalendarTool();
    var call = new ToolCall
    {
        Name = "calendar",
        Parameters = new Dictionary<string, object>
        {
            ["date"] = "2026-04-20",
            ["fromCalendar"] = "gregorian",
            ["toCalendar"] = "chinese_lunar"
        }
    };
    
    var result = await tool.ExecuteAsync(call);
    
    Assert.IsTrue(result.Success);
    Assert.IsNotNull(result.Output);
}
```

## 문제 해결

### 도구를 찾을 수 없음

**문제**: AI가 존재하지 않는 도구 호출 시도.

**해결책**: 
- 도구 이름 정확히 일치 확인
- 도구가 Tools 디렉토리에 있는지 확인
- 프로젝트 재빌드

### 권한 거부

**문제**: 도구 실행이 권한 오류로 실패.

**해결책**:
- 권한 로그 확인
- 사용자가 필요한 권한 있는지 확인
- GlobalACL 설정 검토

### 도구가 오류 반환

**문제**: 도구 실행되지만 실패 반환.

**해결책**:
- 자세한 오류 도구 로그 확인
- 입력 매개변수 검증
- 독립적으로 도구 테스트

## 다음 단계

- 📚 [아키텍처 가이드](architecture.md) 읽기
- 🛠️ [개발 가이드](development-guide.md) 확인
- 🔒 [권한 시스템](permission-system.md) 검토
- 🚀 [시작 가이드](getting-started.md) 참조
