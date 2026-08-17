# 툴 참고

> **버전: v0.2.0-alpha**

이 문서는 실리콘 라이프 콜렉티브 플랫폼의 모든 내장 툴을 자세히 소개합니다.

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | **한국어** | [Čeština](../cs-CZ/tools-reference.md) | [Русский](../ru-RU/tools-reference.md)

## 개요

툴 시스템은 실리콘 비잉이 표준화된 인터페이스를 통해 외부 세계와 상호작용할 수 있게 합니다. 각 툴은 `ITool` 인터페이스를 구현하며, `ToolManager`가 리플렉션을 통해 자동으로 발견하고 등록합니다.

### 툴 분류

- **시스템 관리 툴** — 설정, 권한, 동적 컴파일, 큐레이터 관리
- **통신 툴** — 채팅, 네트워크 요청
- **데이터 저장 툴** — 디스크 작업, 데이터베이스, 메모리, 워크 노트
- **시간 관리 툴** — 캘린더, 타이머, 태스크
- **개발 툴** — 코드 실행, 로그 쿼리
- **유틸리티 툴** — 시스템 정보, 토큰 사용 감사, 도움말 문서, 노리지 네트워크
- **브라우저 툴** — WebView 브라우저 자동화
- **프로젝트 툴** — 프로젝트 관리, 프로젝트 태스크, 프로젝트 워크 노트, 프로젝트 워크
- **플러그인 툴** — 플러그인 시스템을 통해 등록된 서드파티 툴

### 툴 시나리오 시스템

각 툴은 `[ToolScenario]` 속성을 통해 사용 가능한 시나리오를 선언합니다:

| 시나리오 플래그 | 값 | 설명 |
|----------|------|-------------|
| `Chat` | `1 << 0` | 채팅 시나리오 (사용자가 실리콘 비잉과 대화할 때) |
| `Task` | `1 << 1` | 태스크 시나리오 (실리콘 비잉이 태스크를 실행할 때) |
| `Timer` | `1 << 2` | 타이머 시나리오 (실리콘 비잉이 타이머 태스크를 실행할 때) |
| `MemoryCompression` | `1 << 3` | 메모리 압축 시나리오 |
| `Project` | `1 << 4` | 프로젝트 시나리오 (ThinkOnProject 모드) |
| `All` | 상기 모두 | 모든 시나리오에서 사용 가능 |

또한 `[ChatOnly]` 속성이 표시된 툴은 채팅 시나리오에서만 사용 가능하며(예: HelpTool), 태스크 및 타이머 시나리오에는 나타나지 않습니다.

---

## 내장 툴 목록

### 1. 캘린더 툴 (CalendarTool)

**툴 이름**: `calendar`

**기능 설명**: 32가지 캘린더 시스템의 날짜 변환 및 계산을 지원합니다.

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

### 2. 채팅 툴 (ChatTool)

**툴 이름**: `chat`

**기능 설명**: 채팅 세션 및 메시지 전송 관리.

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

### 3. 설정 툴 (ConfigTool)

**툴 이름**: `config`

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

### 4. 큐레이터 툴 (CuratorTool) 🔒

**툴 이름**: `silicon_manager`

**권한 요구사항**: 실리콘 큐레이터 전용 (`[SiliconManagerOnly]`)

**사용 가능 시나리오**: Chat, Task, Timer

**기능 설명**: 실리콘 큐레이터 전용 시스템 관리 툴로, 실리콘 비잉의 생성, 조회 및 리셋을 관리합니다.

**지원 작업**:
- `list_beings` — 모든 실리콘 비잉 및 상태 나열
- `create_being` — 새 실리콘 비잉 만들기 (`name` 및 `soul` 매개변수 필요)
- `get_code` — 실리콘 비잉의 커스텀 소스 코드 보기
- `reset` — 실리콘 비잉을 기본 구현으로 리셋

**사용 예시**:
```json
{
  "action": "create_being",
  "name": "어시스턴트",
  "soul": "당신은 유용한 어시스턴트입니다..."
}
```

---

### 5. 데이터베이스 툴 (DatabaseTool)

**툴 이름**: `database`

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

### 6. 디스크 툴 (DiskTool)

**툴 이름**: `disk`

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

**권한 요구사항**: `FileAccess`

**사용 예시**:
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

---

### 7. 동적 컴파일 툴 (DynamicCompileTool) 🔒

**툴 이름**: `compile`

**기능 설명**: C# 코드 동적 컴파일 (실리콘 비잉 자가 진화용).

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

### 8. 코드 실행 툴 (ExecuteCodeTool) 🔒

**툴 이름**: `execute_code`

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

### 9. 도움말 툴 (HelpTool)

**툴 이름**: `help`

**사용 가능 시나리오**: Chat (`[ChatOnly]`, 채팅 시나리오에서만 사용 가능)

**기능 설명**: 시스템 도움말 문서 콘텐츠 검색 및 가져오기, AI가 시스템 기능 사용 방법을 쿼리할 수 있도록 합니다.

**지원 작업**:
- `list` — 모든 도움말 주제 ID 나열
- `search` — 키워드로 도움말 문서 검색
- `get` — 지정된 ID의 도움말 문서 콘텐츠 가져오기

**사용 예시**:
```json
{
  "action": "search",
  "keyword": "권한"
}
```

---

### 10. 노리지 네트워크 툴 (KnowledgeTool)

**툴 이름**: `knowledge`

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

### 11. MCP 조회 툴 (McpTool)

**툴 이름**: `mcp`

**기능 설명**: MCP(Model Context Protocol) 통합 상태 조회 — 연결된 외부 서버, 제공하는 도구 및 호출 방법. 읽기 전용 툴: 서버 추가/삭제는 사용자가 Web UI를 통해서만 수행 가능, AI는 서버 목록을 수정할 수 없음.

**지원 작업**:
- `status` — 전역 개요(활성화 상태, 서버 수, 도구 수)
- `list_servers` — 구성된 서버 목록(연결 상태 및 도구 수 포함)
- `list_tools` — 사용 가능한 도구 목록(`mcp_{server}_{tool}` 접두사 이름, 설명 및 매개변수 스키마 포함; 선택적 `server_id`로 단일 서버 필터링)

**사용 예시**:
```json
{
  "action": "list_tools",
  "server_id": "filesystem",
  "include_schema": true
}
```

**MCP 래핑 도구**: 연결된 각 MCP 서버가 제공하는 도구는 독립 도구로서 실리존 비잉에 동적 등록되며, 명명 형식은 `mcp_{serverId}_{toolName}`(예: `mcp_filesystem_read_file`). AI는 일반 도구처럼 접두사 이름으로 직접 호출할 수 있으며, 이 조회 툴을 경유할 필요 없음. 래핑 도구는 권한 매트릭스에서 단일 `execute` 액션으로 표시되며, 개별적으로 비활성화 가능.

**시나리오**: 모든 시나리오(`All`)

---

### 12. 로그 툴 (LogTool)

**툴 이름**: `log`

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

### 13. 메모리 툴 (MemoryTool)

**툴 이름**: `memory`

**기능 설명**: 실리콘 비잉의 장기 및 단기 메모리 관리.

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

### 14. 네트워크 툴 (NetworkTool)

**툴 이름**: `network`

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

### 15. 퍼미션 툴 (PermissionTool) 🔒

**툴 이름**: `permission`

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

### 16. 프로젝트 툴 (ProjectTool) 🔒

**툴 이름**: `project`

**권한 요구사항**: 실리콘 큐레이터 전용 (`[SiliconManagerOnly]`)

**사용 가능 시나리오**: Chat, Task, Timer

**기능 설명**: 프로젝트 워크스페이스 관리, 프로젝트 라이프사이클 관리, 멤버 할당 및 역할 관리를 지원합니다.

**지원 작업**:
- `create` — 새 프로젝트 공간 만들기
- `archive` — 프로젝트 보관
- `restore` — 보관된 프로젝트 복원
- `destroy` — 프로젝트 삭제 및 데이터 정리 (복구 불가)
- `list` — 모든 프로젝트 나열
- `get` — 프로젝트 상세 정보 가져오기
- `assign` — 실리콘 비잉을 프로젝트에 할당
- `remove` — 프로젝트에서 실리콘 비잉 제거
- `update` — 프로젝트 이름/설명 업데이트
- `list-workflow-templates` — 사용 가능한 워크플로 템플릿 나열
- `assign_role` — 실리콘 비잉에게 프로젝트 역할 할당
- `remove_role` — 실리콘 비잉의 프로젝트 역할 제거
- `list_roles` — 프로젝트의 역할 할당 나열

**사용 예시**:
```json
{
  "action": "create",
  "name": "My Project",
  "description": "프로젝트 설명"
}
```

---

### 17. 프로젝트 태스크 툴 (ProjectTaskTool)

**툴 이름**: `project_task`

**사용 가능 시나리오**: Chat, Task, Timer

**기능 설명**: 프로젝트 공간 내 태스크 관리, 완전한 태스크 라이프사이클을 지원합니다.

**지원 작업**:
- `create` — 프로젝트 태스크 만들기
- `list` — 프로젝트 태스크 나열
- `get` — 태스크 상세 정보 가져오기
- `update` — 태스크 제목/설명/우선순위 업데이트
- `assign` — 태스크에 담당자 할당
- `remove_assignee` — 태스크 담당자 제거
- `start` — 태스크 시작
- `complete` — 태스크 완료 표시
- `fail` — 태스크 실패 표시
- `cancel` — 태스크 취소
- `delete` — 태스크 삭제
- `stats` — 태스크 통계 가져오기

**사용 예시**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "태스크 설명 완료",
  "priority": 5
}
```

---

### 18. 프로젝트 워크 노트 툴 (ProjectWorkNoteTool)

**툴 이름**: `project_work_note`

**사용 가능 시나리오**: Chat, Task, Timer

**기능 설명**: 프로젝트 공간 내 워크 노트 관리 (공개, 작업장 비슷), 페이지식 노트 관리를 지원합니다.

**지원 작업**:
- `create` — 노트 페이지 만들기 (`project_id`, `summary` 및 `content` 필요, `keywords` 선택)
- `read` — 노트 페이지 읽기 (`project_id` 및 `page_number` 또는 `note_id` 필요)
- `update` — 노트 페이지 업데이트 (`project_id`, `page_number` 및 `content` 필요, `summary` 및 `keywords` 선택)
- `delete` — 노트 페이지 삭제 (`project_id` 및 `page_number` 또는 `note_id` 필요)
- `list` — 프로젝트의 모든 노트 페이지 요약 나열
- `directory` — 노트 목차/개요 생성
- `search` — 키워드로 노트 검색 (`project_id` 및 `keyword` 필요, `max_results` 선택)

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

### 19. 프로젝트 워크 툴 (ProjectWorkTool) 🔒

**툴 이름**: `project_work`

**권한 요구사항**: 실리콘 큐레이터 전용 (`[SiliconManagerOnly]`)

**사용 가능 시나리오**: Project (`[ToolScenario(ToolScenarioFlag.Project)]`, 프로젝트 시나리오에서만 사용 가능)

**기능 설명**: 프로젝트 워크 작업 툴로, 큐레이터가 ThinkOnProject 시나리오에서 프로젝트 워크플로를 관리하는 데 사용합니다.

**지원 작업**:
- `create-task` — 프로젝트 태스크 만들기
- `assign-task` — 태스크에 실리콘 비잉 할당
- `chat` — 프로젝트 그룹 채팅에 메시지 보내기
- `broadcast` — 프로젝트 채널에 메시지 브로드캐스트
- `complete` — 프로젝트 완료 표시
- `status` — 프로젝트 상태 가져오기

**사용 예시**:
```json
{
  "action": "create-task",
  "project_id": "project-uuid",
  "title": "사용자 인증 구현"
}
```

---

### 20. 스킬 툴 (SkillTool)

**툴 이름**: `skill`

**기능 설명**: 실리존 비잉의 스킬(재사용 가능한 "도구 오케스트레이션 + 프롬프트 템플릿" 능력 단위) 관리, 생성, 목록 조회, 업데이트, 삭제, 가져오기/내보내기 지원. 누락된 메타데이터(id, 설명, 매개변수 스키마 등)는 AI가 자동으로 보완.

**지원 작업**:
- `create` — 새 스킬 생성(`id` 및 `system_prompt` 필요; 선택: `description`, `parameter_schema`, `tool_whitelist`, `tags`, `max_tool_round`, `timeout`, `on_complete`, `trigger_mode`, `auto_trigger_condition`)
- `list` — 사용 가능한 모든 스킬 목록(요약 포함)
- `update` — 매개변수로 기존 스킬 업데이트(`skill_id` 필요)
- `update_from_md` — Markdown 문자열에서 스킬 업데이트(YAML 프론트매터 메타데이터 + 프롬프트 본문)
- `delete` — 스킬 삭제(`skill_id` 필요)
- `export` — 스킬을 JSON으로 내보내기(`skill_id` 필요)
- `export_md` — 스킬을 Markdown으로 내보내기(`skill_id` 필요)
- `import` — JSON에서 스킬 가져오기(`json` 필요)
- `import_md` — Markdown에서 스킬 가져오기(`markdown` 필요)

**사용 예시**:
```json
{
  "action": "create",
  "id": "daily_news_digest",
  "description": "오늘의 기술 뉴스를 검색하고 요약을 생성",
  "system_prompt": "network 도구를 사용하여 {topic}의 최신 뉴스를 검색하고 500자 요약을 생성해 주세요.",
  "parameter_schema": {
    "type": "object",
    "properties": {
      "topic": { "type": "string", "description": "뉴스 주제" }
    },
    "required": ["topic"]
  },
  "tool_whitelist": ["network", "work_note"],
  "trigger_mode": "Auto",
  "auto_trigger_condition": "schedule",
  "metadata": { "schedule": "0 9 * * *" }
}
```

**수정 권한**: 실리콘 큐레이터는 모든 스킬을 수정할 수 있음; 일반 비잉은 출처가 `Being` 또는 `User`인 스킬만 수정 가능(내장 및 플러그인 스킬은 수정 불가).

**수량 제한**: 비잉당 커스텀 스킬 수는 설정 `MaxCustomSkillsPerBeing`(기본 50)에 의해 제한.

**시나리오**: 모든 시나리오(`All`)

> 스킬 시스템(트리거 모드, 화이트리스트, 핫 리로드, 자동 스케줄링 등)의 전체 설명은 [실리존 비잉 가이드](silicon-being-guide.md#스킬-시스템)를 참조.

---

### 21. 시스템 툴 (SystemTool)

**툴 이름**: `system`

**기능 설명**: 시스템 정보 및 리소스 사용 현황 가져오기.

**지원 작업**:
- `info` — 시스템 정보 가져오기
- `resource_usage` — 리소스 사용 현황 가져오기
- `find_process` — 프로세스 찾기
- `list_beings` — 실리콘 비잉 나열

**사용 예시**:
```json
{
  "action": "info"
}
```

---

### 22. 태스크 툴 (TaskTool)

**툴 이름**: `task`

**기능 설명**: 실리콘 비잉 개인 태스크 관리.

**지원 작업**:
- `create` — 태스크 만들기
- `list` — 태스크 나열
- `update` — 태스크 업데이트
- `complete` — 태스크 완료
- `delete` — 태스크 삭제
- `get_dependencies` — 의존성 가져오기

**사용 예시**:
```json
{
  "action": "create",
  "description": "코드 리뷰",
  "priority": 5
}
```

---

### 23. 타이머 툴 (TimerTool)

**툴 이름**: `timer`

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

### 24. 토큰 사용 감사 툴 (TokenAuditTool) 🔒

**툴 이름**: `token_audit`

**권한 요구사항**: 실리콘 큐레이터 전용 (`[SiliconManagerOnly]`)

**사용 가능 시나리오**: Chat, Task, Timer

**기능 설명**: AI 토큰 사용 통계 및 추세 데이터 쿼리.

**지원 작업**:
- `summary` — 토큰 사용 요약 통계 가져오기
- `trend` — 토큰 사용 추세 데이터 포인트 가져오기

**지원 시간 범위**:
- `today` — 최근 24시간
- `week` — 최근 7×24시간
- `month` — 일별 통계
- `year` — 월별 통계

**사용 예시**:
```json
{
  "action": "summary",
  "time_range": "week"
}
```

---

### 25. WebView 브라우저 툴 (WebViewBrowserTool)

**툴 이름**: `webview_browser`

**사용 가능 시나리오**: Chat, Task, Timer

**기능 설명**: Playwright 기반 브라우저 자동화 작업으로, 완전한 웹 페이지 탐색, 상호작용 및 데이터 추출 기능을 제공합니다.

**지원 작업**:
- `open` — 브라우저 열기
- `close` — 브라우저 닫기
- `navigate` — URL로 이동
- `click` — 요소 클릭
- `input` — 텍스트 입력
- `scroll` — 페이지 스크롤
- `execute_script` — JavaScript 실행
- `get_page_text` — 페이지 텍스트 가져오기
- `get_screenshot` — 스크린샷 가져오기
- `wait_for_element` — 요소 나타날 때까지 대기
- `get_element_info` — 요소 정보 가져오기
- `upload_file` — 파일 업로드
- `get_browser_status` — 브라우저 상태 가져오기
- `set_timeout` — 타임아웃 설정
- `clear_session` — 브라우저 세션 초기화

**특징**:
- 각 실리콘 비잉 독립 인스턴스
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

### 26. 워크 노트 툴 (WorkNoteTool)

**툴 이름**: `work_note`

**기능 설명**: 실리콘 비잉 개인 워크 노트 관리 (비공개, 일기장 비슷).

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

## 툴 호출 프로세스

```
┌──────────┐
│   AI     │ tool_calls 반환
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ 툴 사용 권한 찾기 및 검증
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
│   AI     │ 툴 결과 수신, 계속 생각
└──────────┘
```

## 권한 검증

모든 툴 실행은 권한 검증 체인을 통과합니다:

1. **UserFrequencyCache** — 사용자 빈도 캐시 (HighDeny가 HighAllow보다 우선)
2. **IPermissionCallback** — 커스텀 권한 콜백 함수 (Allowed/Denied/AskUser)
3. **IsCurator 분기** — 큐레이터는 IPermissionAskHandler를 통해 사용자에게 문의; 비큐레이터는 GlobalACL을 쿼리하며, 일치하는 규칙이 없으면 기본 거부

## 커스텀 툴 만들기

### 단계 1: ITool 인터페이스 구현

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";

    public string Description => "툴 설명";

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

툴 파일을 `src/SiliconLife.Common/Tools/` 디렉토리(공유 툴) 또는 `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` 디렉토리(버전별 툴)에 배치합니다. `ToolManager`는 시작 시 리플렉션을 통해 자동으로 발견하고 등록합니다.

### 단계 2a: 플러그인을 통해 툴 등록

플러그인 시스템을 통해 커스텀 툴을 등록할 수도 있습니다:

1. 플러그인 프로젝트에서 `ITool` 인터페이스 구현
2. 플러그인 DLL을 컴파일하여 플러그인 디렉토리에 배치
3. `ToolManager.ScanAllPluginAssemblies()`가 모든 로드된 플러그인에서 ITool 구현을 자동 스캔
4. 플러그인 툴은 동일한 권한 시스템의 제약을 받음

### 단계 3: (선택) 큐레이터 전용으로 표시

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // 실리콘 큐레이터만 접근 가능
}
```

### 대안: 스킬 및 MCP 도구

C# 도구 클래스를 작성하는 것 외에, 컴파일 없이 확장할 수 있는 두 가지 방법이 있습니다:

- **스킬(Skill)**: Web UI 또는 `skill` 도구를 통해 "도구 오케스트레이션 + 프롬프트 템플릿" 조합을 생성하여, 자주 사용하는 워크플로를 재사용 가능한 능력으로 캡슐화. [실리존 비잉 가이드 — 스킬 시스템](silicon-being-guide.md#스킬-시스템) 참조.
- **MCP 서버**: Web UI에서 외부 MCP 서버를 구성하면, 해당 도구가 `mcp_{serverId}_{toolName}` 형식으로 자동 주입되며, 코드를 작성할 필요 없음. [Web UI 가이드 — MCP 관리](web-ui-guide.md) 참조.

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
    Logger.Error($"툴 {Name} 실행 실패: {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. 권한 시스템 존중

권한 검사를 절대 우회하지 마세요. 항상 이그제큐터를 통해 리소스에 접근하세요:

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return ToolResult.Denied("Permission denied");
}
```

### 4. 명확한 툴 설명 제공

AI가 툴을 언제, 어떻게 사용할지 이해하도록 도와주세요:

```csharp
public string Description =>
    "다양한 캘린더 시스템 간 날짜 변환에 사용." +
    "'date', 'from_calendar' 및 'to_calendar' 매개변수가 필요합니다.";
```

## 문제 해결

### 툴을 찾을 수 없음

**문제**: AI가 존재하지 않는 툴을 호출하려고 함.

**해결 방법**:
- 툴 이름이 정확히 일치하는지 확인
- 툴 파일이 `Tools/` 디렉토리에 있는지 확인
- 프로젝트 재빌드 (`dotnet build`)

### 권한 거부됨

**문제**: 툴 실행 실패, 권한 오류 반환.

**해결 방법**:
- 권한 감사 로그 확인
- 실리콘 비잉에 필요한 권한이 있는지 확인
- 글로벌 ACL 설정 확인
- 큐레이터인 경우 `[SiliconManagerOnly]` 마크 사용 여부 확인

### 툴 실행 오류 반환

**문제**: 툴이 실행되지만 실패 결과 반환.

**해결 방법**:
- 툴이 반환한 오류 메시지 확인
- 입력 매개변수 형식이 올바른지 확인
- 시스템 로그에서 상세 오류 정보 확인
- 툴 기능 독립 테스트

## 다음 단계

- 📚 [아키텍처 가이드](architecture.md) 읽기
- 🛠️ [개발 가이드](development-guide.md) 보기
- 🔒 [권한 시스템](permission-system.md) 이해
- 🚀 [빠른 시작 가이드](getting-started.md) 보기
