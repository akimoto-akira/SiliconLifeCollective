# 도구 참고

> **버전: v0.2.0-alpha**

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | **한국어** | [Čeština](../cs-CZ/tools-reference.md)

## 개요

도구 시스템은 실리콘 생명체가 표준화된 인터페이스를 통해 외부 세계와 상호작용할 수 있게 합니다. 각 도구는 `ITool` 인터페이스를 구현하며, `ToolManager`가 리플렉션을 통해 자동으로 발견하고 등록합니다.

### 도구 분류

- **시스템 관리 도구** — 설정, 권한, 동적 컴파일
- **통신 도구** — 채팅, 네트워크 요청
- **데이터 저장 도구** — 디스크 작업, 데이터베이스, 메모리, 작업 노트
- **시간 관리 도구** — 캘린더, 타이머, 작업
- **개발 도구** — 코드 실행, 로그 쿼리
- **유틸리티 도구** — 시스템 정보, Token 감사, 도움말 문서, 지식 네트워크
- **브라우저 도구** — WebView 브라우저 자동화
- **플러그인 도구** — 플러그인 시스템을 통해 등록된 서드파티 도구

---

## 내장 도구 목록

### 1. 캘린더 도구 (CalendarTool)

**도구 이름**: `calendar`

**기능 설명**: 32가지 캘린더 시스템의 날짜 변환 및 계산 지원.

**지원 작업**:
- `now` — 현재 시간 가져오기
- `format` — 날짜 포맷
- `add_days` — 날짜 더하기/빼기
- `diff` — 날짜 차이 계산
- `list_calendars` — 지원되는 모든 캘린더 나열
- `get_components` — 날짜 구성 요소 가져오기
- `get_now_components` — 현재 시간 구성 요소 가져오기
- `convert` — 캘린더 시스템 간 변환

**지원 캘린더 시스템** (32가지):
- 그레고리력 (Gregorian)
- 중국 음력 (Chinese Lunar)
- 중국 역사력 (Chinese Historical) — 간지 기년, 제왕 연호
- 이슬람력 (Islamic)
- 히브리력 (Hebrew)
- 일본력 (Japanese)
- 페르시아력 (Persian)
- 마야력 (Mayan)
- 불력 (Buddhist)
- 티베트력 (Tibetan)
- 기타 24가지 캘린더...

**사용 예시**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_lunar"
}
```

---

### 2. 채팅 도구 (ChatTool)

**도구 이름**: `chat`

**기능 설명**: 채팅 세션 관리 및 메시지 전송.

**지원 작업**:
- `send_message` — 메시지 보내기
- `get_messages` — 히스토리 메시지 가져오기
- `create_group` — 그룹 채팅 만들기
- `add_member` — 그룹 멤버 추가
- `remove_member` — 그룹 멤버 제거
- `get_chat_info` — 채팅 정보 가져오기
- `terminate_chat` — 채팅 종료 (읽씹)

**사용 예시**:
```json
{
  "action": "send_message",
  "target_id": "being-uuid-or-user-0",
  "message": "안녕하세요, 협업합시다!"
}
```

---

### 3. 설정 도구 (ConfigTool)

**도구 이름**: `config`

**기능 설명**: 시스템 설정 읽기 및 수정.

**지원 작업**:
- `read` — 설정 항목 읽기
- `write` — 설정 항목 쓰기
- `list` — 모든 설정 나열
- `get_ai_config` — AI 클라이언트 설정 가져오기
- `set_ai_config` — AI 클라이언트 설정하기

**사용 예시**:
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

---

### 4. 큐레이터 도구 (CuratorTool) 🔒

**도구 이름**: `curator`

**권한 요구사항**: 실리콘 큐레이터 전용

**기능 설명**: 실리콘 큐레이터 전용 시스템 관리 도구.

**지원 작업**:
- `create_being` — 새 실리콘 생명체 만들기
- `list_beings` — 모든 실리콘 생명체 나열
- `get_being_info` — 생명체 정보 가져오기
- `assign_task` — 작업 할당
- `manage_permissions` — 권한 관리

**사용 예시**:
```json
{
  "action": "create_being",
  "name": "어시스턴트",
  "soul_file": "assistant_soul.md"
}
```

---

### 5. 데이터베이스 도구 (DatabaseTool)

**도구 이름**: `database`

**기능 설명**: 구조화된 데이터베이스 쿼리 및 작업.

**지원 작업**:
- `query` — 데이터 쿼리
- `insert` — 데이터 삽입
- `update` — 데이터 업데이트
- `delete` — 데이터 삭제
- `create_table` — 테이블 만들기
- `list_tables` — 모든 테이블 나열

**사용 예시**:
```json
{
  "action": "query",
  "table": "users",
  "conditions": {"status": "active"},
  "limit": 100
}
```

---

### 6. 디스크 도구 (DiskTool)

**도구 이름**: `disk`

**기능 설명**: 파일 시스템 작업 및 로컬 검색.

**지원 작업**:
- `read` — 파일 읽기
- `write` — 파일 쓰기
- `list` — 디렉토리 나열
- `delete` — 파일 삭제
- `create_directory` — 디렉토리 만들기
- `search_files` — 파일 검색
- `search_content` — 파일 내용 검색
- `count_lines` — 행 수 통계
- `read_lines` — 지정된 행 읽기
- `replace_text` — 텍스트 교체

**권한 요구사항**: `disk:read`, `disk:write`

**사용 예시**:
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

---

### 7. 동적 컴파일 도구 (DynamicCompileTool) 🔒

**도구 이름**: `compile`

**기능 설명**: C# 코드 동적 컴파일 (실리콘 생명체 자가 진화용).

**지원 작업**:
- `compile_class` — 클래스 컴파일
- `compile_callback` — 권한 콜백 함수 컴파일
- `validate_code` — 코드 보안 검증

**보안 메커니즘**:
- 컴파일 타임 참조 제어 (위험 어셈블리 제외)
- 런타임 정적 코드 스캔
- AES-256 암호화 저장

**사용 예시**:
```json
{
  "action": "compile_class",
  "code": "public class MyBeing : SiliconBeingBase { ... }"
}
```

---

### 8. 코드 실행 도구 (ExecuteCodeTool) 🔒

**도구 이름**: `execute_code`

**권한 요구사항**: 실리콘 큐레이터 전용

**기능 설명**: C# 코드 조각 컴파일 및 실행.

**지원 작업**:
- `run_script` — 코드 스크립트 실행

**사용 예시**:
```json
{
  "action": "run_script",
  "code": "return DateTime.Now.ToString();",
  "timeout": 30
}
```

---

### 9. 도움말 도구 (HelpTool)

**도구 이름**: `help`

**기능 설명**: 시스템 도움말 문서 및 사용 가이드 가져오기.

**지원 작업**:
- `get_topics` — 도움말 주제 목록 가져오기
- `get_topic` — 특정 주제 세부정보 가져오기
- `search` — 도움말 문서 검색

**사용 예시**:
```json
{
  "action": "get_topics"
}
```

---

### 10. 지식 네트워크 도구 (KnowledgeTool)

**도구 이름**: `knowledge`

**기능 설명**: 지식 그래프 작업 (트리플 기반: 주어-관계-목적어).

**지원 작업**:
- `add` — 지식 트리플 추가
- `query` — 지식 쿼리
- `update` — 지식 업데이트
- `delete` — 지식 삭제
- `search` — 지식 검색
- `get_path` — 지식 경로 가져오기
- `validate` — 지식 검증
- `stats` — 통계 정보 가져오기

**사용 예시**:
```json
{
  "action": "add",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95
}
```

---

### 11. 로그 도구 (LogTool)

**도구 이름**: `log`

**기능 설명**: 작업 히스토리 및 대화 히스토리 쿼리.

**지원 작업**:
- `query_logs` — 시스템 로그 쿼리
- `query_conversations` — 대화 히스토리 쿼리
- `get_stats` — 로그 통계 가져오기

**사용 예시**:
```json
{
  "action": "query_logs",
  "being_id": "being-uuid",
  "start_time": "2026-04-20T00:00:00Z",
  "end_time": "2026-04-26T23:59:59Z",
  "level": "info"
}
```

---

### 12. 메모리 도구 (MemoryTool)

**도구 이름**: `memory`

**기능 설명**: 실리콘 생명체의 장기 및 단기 메모리 관리.

**지원 작업**:
- `read` — 메모리 읽기
- `write` — 메모리 쓰기
- `search` — 메모리 검색
- `delete` — 메모리 삭제
- `list` — 메모리 나열
- `get_stats` — 메모리 통계 가져오기
- `compress` — 메모리 압축

**사용 예시**:
```json
{
  "action": "read",
  "key": "important_fact",
  "time_range": {
    "start": "2026-04-01",
    "end": "2026-04-26"
  }
}
```

---

### 13. 네트워크 도구 (NetworkTool)

**도구 이름**: `network`

**기능 설명**: HTTP/HTTPS 요청 시작.

**지원 작업**:
- `get` — GET 요청
- `post` — POST 요청
- `put` — PUT 요청
- `delete` — DELETE 요청
- `download` — 파일 다운로드
- `upload` — 파일 업로드

**권한 요구사항**: `network:http`

**사용 예시**:
```json
{
  "action": "get",
  "url": "https://api.example.com/data"
}
```

---

### 14. 권한 도구 (PermissionTool) 🔒

**도구 이름**: `permission`

**권한 요구사항**: 실리콘 큐레이터 전용

**기능 설명**: 권한 및 접근 제어 목록 관리.

**지원 작업**:
- `query_permission` — 권한 쿼리
- `manage_acl` — 글로벌 ACL 관리
- `get_callback` — 권한 콜백 함수 가져오기
- `set_callback` — 권한 콜백 함수 설정

**사용 예시**:
```json
{
  "action": "manage_acl",
  "acl_action": "add_rule",
  "permission_type": "filesystem",
  "resource_prefix": "/data/",
  "acl_result": "allow"
}
```

---

### 15. 프로젝트 도구 (ProjectTool)

**도구 이름**: `project`

**기능 설명**: 프로젝트 워크스페이스 관리.

**지원 작업**:
- `create` — 프로젝트 만들기
- `list` — 프로젝트 나열
- `get_info` — 프로젝트 정보 가져오기
- `update` — 프로젝트 업데이트
- `archive` — 프로젝트 보관

**사용 예시**:
```json
{
  "action": "create",
  "name": "My Project",
  "description": "프로젝트 설명"
}
```

---

### 16. 프로젝트 작업 도구 (ProjectTaskTool)

**도구 이름**: `project_task`

**기능 설명**: 프로젝트 작업 관리.

**지원 작업**:
- `create` — 작업 만들기
- `list` — 작업 나열
- `update` — 작업 업데이트
- `complete` — 작업 완료
- `get_stats` — 작업 통계 가져오기

**사용 예시**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "작업 설명 완료",
  "priority": 5
}
```

---

### 17. 프로젝트 작업 노트 도구 (ProjectWorkNoteTool)

**도구 이름**: `project_work_note`

**기능 설명**: 프로젝트 작업 노트 관리 (공개, 작업장 비슷).

**지원 작업**:
- `create` — 노트 만들기
- `read` — 노트 읽기
- `update` — 노트 업데이트
- `delete` — 노트 삭제
- `list` — 노트 나열
- `search` — 노트 검색
- `directory` — 목차 생성

**사용 예시**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "사용자 인증 모듈 완료",
  "content": "## 구현 세부정보\n\n- JWT token 사용",
  "keywords": "인증,JWT"
}
```

---

### 18. 시스템 도구 (SystemTool)

**도구 이름**: `system`

**기능 설명**: 시스템 정보 및 리소스 사용 현황 가져오기.

**지원 작업**:
- `info` — 시스템 정보 가져오기
- `resource_usage` — 리소스 사용 현황 가져오기
- `find_process` — 프로세스 찾기
- `list_beings` — 실리콘 생명체 나열

**사용 예시**:
```json
{
  "action": "info"
}
```

---

### 19. 작업 도구 (TaskTool)

**도구 이름**: `task`

**기능 설명**: 실리콘 생명체 개인 작업 관리.

**지원 작업**:
- `create` — 작업 만들기
- `list` — 작업 나열
- `update` — 작업 업데이트
- `complete` — 작업 완료
- `delete` — 작업 삭제
- `get_dependencies` — 의존성 가져오기

**사용 예시**:
```json
{
  "action": "create",
  "description": "코드 검토",
  "priority": 5
}
```

---

### 20. 타이머 도구 (TimerTool)

**도구 이름**: `timer`

**기능 설명**: 타이머 만들기 및 관리.

**지원 작업**:
- `create` — 타이머 만들기
- `list` — 타이머 나열
- `delete` — 타이머 삭제
- `pause` — 타이머 일시중지
- `resume` — 타이머 재개
- `get_execution_history` — 실행 기록 가져오기

**사용 예시**:
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true,
  "message": "매시간 알림"
}
```

---

### 21. Token 감사 도구 (TokenAuditTool) 🔒

**도구 이름**: `token_audit`

**권한 요구사항**: 실리콘 큐레이터 전용

**기능 설명**: AI token 사용 현황 쿼리 및 집계.

**지원 작업**:
- `get_usage` — token 사용 통계 가져오기
- `get_by_being` — 생명체별 사용 현황 가져오기
- `get_by_model` — 모델별 사용 현황 가져오기
- `get_trend` — 사용 추세 가져오기
- `export` — 데이터 내보내기

**사용 예시**:
```json
{
  "action": "get_usage",
  "start_date": "2026-04-01",
  "end_date": "2026-04-26"
}
```

---

### 22. WebView 브라우저 도구 (WebViewBrowserTool)

**도구 이름**: `webview`

**기능 설명**: Playwright 기반 브라우저 자동화 작업.

**지원 작업**:
- `open_browser` — 브라우저 열기
- `close_browser` — 브라우저 닫기
- `navigate` — URL로 이동
- `click` — 요소 클릭
- `input` — 텍스트 입력
- `get_page_text` — 페이지 텍스트 가져오기
- `get_screenshot` — 스크린샷 가져오기
- `execute_script` — JavaScript 실행
- `wait_for_element` — 요소 나타날 때까지 대기
- `get_browser_status` — 브라우저 상태 가져오기

**기능**:
- 각 실리콘 생명체 독립 인스턴스
- 완전히 격리된 Cookie 및 세션
- 사용자에게 완전히 보이지 않음 (헤드리스 모드)
- 완전한 JavaScript 및 CSS 지원

**사용 예시**:
```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

---

### 23. 작업 노트 도구 (WorkNoteTool)

**도구 이름**: `work_note`

**기능 설명**: 실리콘 생명체 개인 작업 노트 관리 (비공개, 일기장 비슷).

**지원 작업**:
- `create` — 노트 만들기
- `read` — 노트 읽기
- `update` — 노트 업데이트
- `delete` — 노트 삭제
- `list` — 노트 나열
- `search` — 노트 검색
- `directory` — 목차 생성

**사용 예시**:
```json
{
  "action": "create",
  "summary": "사용자 인증 모듈 완료",
  "content": "## 구현 세부정보\n\n- JWT token 사용\n- OAuth2 지원",
  "keywords": "인증,JWT,OAuth2"
}
```

---

### 24. 핫 리로드 도구 (HotReloadTool)

**도구 이름**: `hot_reload`

**기능 설명**: SiliconLife.Fast 가 실행 중인 동안 자동으로 컴파일, 파일 업데이트 및 재시작 지원, 수동 개입 불필요.

**지원 작업**:
- `execute` — 전체 빌드, 복사 및 재시작 프로세스 실행
- `build_only` — 프로젝트 빌드만 실행, 복사 및 재시작 안 함

**워크플로우**:
1. SiliconLife.Fast 프로젝트 컴파일
2. 현재 실행 중인 Fast 인스턴스 정상 종료 (HTTP API 통해)
3. 프로세스 종료 및 포트释放 대기
4. 빌드 출력물을 대상 디렉토리로 복사 (HotReload 자체 파일 제외)
5. Fast 인스턴스 재시작

**특징**:
- 이전 프로세스 자동 감지 및 종료
- 안전한 파일 복사 (HotReload.exe 덮어쓰지 않음)
- 포트释放 대기 메커니즘
- 사용자 정의 포트 구성 지원

**사용 예시**:
```json
{
  "action": "execute",
  "project_path": "src/SiliconLife.Fast",
  "source_path": "src/SiliconLife.Fast/bin/Debug/net9.0",
  "configuration": "Debug",
  "port": 8080
}
```

**매개변수 설명**:
- `project_path`: 프로젝트 경로 (솔루션 루트 디렉토리 기준)
- `source_path`: 빌드 출력 디렉토리
- `configuration`: 빌드 구성 (Debug/Release)
- `port`: Fast 인스턴스의 Web 포트 (기본값 8080)

**주의 사항**:
- SiliconLife.Fast 버전에서만 적용 가능
- tools/HotReload 디렉토리에 HotReload.exe 필요
- 재시작 과정에서 짧은 서비스 중단 있음 (약 3-5 초)

---

## 도구 호출 프로세스

```
┌──────────┐
│   AI     │ tool_calls 반환
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ 도구 사용 권한 찾기 및 검증
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ 권한 체인 검사
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ 리소스 접근 작업 실행
└────┬─────────┘
     ↓
┌──────────┐
│   AI     │ 도구 결과 수신, 계속 생각
└──────────┘
```

## 권한 검증

모든 도구 실행은 3단계 분기 권한 체인을 통과합니다:

1. **UserFrequencyCache** — 사용자 고빈도 허용/거부 캐시
2. **IPermissionCallback** — 맞춤형 권한 콜백 함수 (허용/거부/AskUser)
3. **IsCurator 분기** — 큐레이터: IPermissionAskHandler (사용자에게 문의) / 비큐레이터: GlobalACL → 기본 거부

## 맞춤형 도구 만들기

### 단계 1: ITool 인터페이스 구현

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    
    public string Description => "도구 설명";
    
    public ToolDefinition Definition => new ToolDefinition
    {
        Name = Name,
        Description = Description,
        Parameters = new Dictionary<string, object>
        {
            ["param1"] = new { type = "string", description = "매개변수 설명" }
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

### 단계 2: 프로젝트에 추가

도구 파일을 `src/SiliconLife.Common/Tools/` 디렉토리(공유 도구) 또는 `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` 디렉토리(버전별 도구)에 배치합니다. `ToolManager`는 시작 시 리플렉션을 통해 자동으로 발견하고 등록합니다.

### 단계 2a: 플러그인을 통해 도구 등록

플러그인 시스템을 통해 맞춤형 도구를 등록할 수도 있습니다:

1. 플러그인 프로젝트에서 `ITool` 인터페이스 구현
2. 플러그인 DLL을 컴파일하여 플러그인 디렉토리에 배치
3. `ToolManager.ScanAllPluginAssemblies()`가 모든 로드된 플러그인에서 ITool 구현을 자동 스캔
4. 플러그인 도구는 동일한 권한 시스템의 제약을 받음

### 단계 3: (선택) 큐레이터 전용으로 표시

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // 실리콘 큐레이터만 접근 가능
}
```

### 1. 항상 매개변수 검증

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("필수 매개변수 누락: required_param");
}
```

### 2. 우아한 오류 처리

```csharp
try
{
    // 작업 실행
}
catch (Exception ex)
{
    Logger.Error($"도구 {Name} 실행 실패: {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. 권한 시스템 존중

권한 검사를 절대 우회하지 마세요. 항상 실행기를 통해 리소스에 접근하세요:

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return ToolResult.Denied(permission.Reason);
}
```

### 4. 명확한 도구 설명 제공

AI가 도구를 언제, 어떻게 사용할지 이해하도록 도와주세요:

```csharp
public string Description => 
    "다양한 캘린더 시스템 간 날짜 변환에 사용." +
    "'date', 'from_calendar' 및 'to_calendar' 매개변수가 필요합니다.";
```

## 문제 해결

### 도구를 찾을 수 없음

**문제**: AI가 존재하지 않는 도구를 호출하려고 함.

**해결 방법**:
- 도구 이름이 정확히 일치하는지 확인
- 도구 파일이 `Tools/` 디렉토리에 있는지 확인
- 프로젝트 재빌드 (`dotnet build`)

### 권한 거부됨

**문제**: 도구 실행 실패, 권한 오류 반환.

**해결 방법**:
- 권한 감사 로그 확인
- 실리콘 생명체에 필요한 권한이 있는지 확인
- 글로벌 ACL 설정 확인
- 큐레이터인 경우 `[SiliconManagerOnly]` 마크 사용 여부 확인

### 도구 실행 오류 반환

**문제**: 도구가 실행되지만 실패 결과 반환.

**해결 방법**:
- 도구가 반환한 오류 메시지 확인
- 입력 매개변수 형식이 올바른지 확인
- 시스템 로그에서 상세 오류 정보 확인
- 도구 기능 독립 테스트

---

## 다음 단계

- 📚 [아키텍처 가이드](architecture.md) 읽기
- 🛠️ [개발 가이드](development-guide.md) 보기
- 🔒 [권한 시스템](permission-system.md) 이해
- 🚀 [빠른 시작 가이드](getting-started.md) 보기
