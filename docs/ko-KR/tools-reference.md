# 도구 참고

[English](../en/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | **한국어** | [Deutsch](../de-DE/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md)

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

### 15-23. 기타 도구

시스템에는 다음 도구도 포함되어 있습니다:

15. **ProjectTool** - 프로젝트 워크스페이스 관리
16. **ProjectTaskTool** - 프로젝트 작업 관리
17. **ProjectWorkNoteTool** - 프로젝트 작업 노트
18. **SystemTool** - 시스템 정보 및 리소스 모니터링
19. **TaskTool** - 개인 작업 관리
20. **TimerTool** - 타이머 관리
21. **TokenAuditTool** 🔒 - Token 사용 감사
22. **WebViewBrowserTool** - 브라우저 자동화 (Playwright 기반)
23. **WorkNoteTool** - 개인 작업 노트

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

## 모범 사례

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

권한 검사를 절대 우회하지 마세요. 항상 실행기를 통해 리소스에 접근하세요.

---

## 다음 단계

- 📚 [아키텍처 가이드](architecture.md) 읽기
- 🛠️ [개발 가이드](development-guide.md) 보기
- 🔒 [권한 시스템](permission-system.md) 이해
- 🚀 [빠른 시작 가이드](getting-started.md) 보기
