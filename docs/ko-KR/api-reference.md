# API 참조

> **버전: v0.2.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | **한국어** | [Čeština](../cs-CZ/api-reference.md) | [Русский](../ru-RU/api-reference.md)

## Web API 엔드포인트

기본 URL: `http://localhost:8080`

### 인증

대부분의 엔드포인트는 Web UI가 관리하는 세션 쿠키를 통한 인증이 필요합니다. 시스템 초기화 전에는 도움말 페이지를 제외한 모든 요청이 초기화 페이지로 리다이렉트됩니다.

---

## 대시보드

### 대시보드 통계 가져오기

**GET** `/api/dashboard/stats`

시스템 개요 데이터(비잉 수, 실행 상태 등)를 반환합니다.

### 성능 지표 가져오기

**GET** `/api/dashboard/metrics`

실시간 성능 지표 데이터를 반환합니다.

---

## 채팅 시스템

### 채팅 페이지

**GET** `/chat`

채팅 인터페이스 페이지를 반환합니다.

### 스트리밍 채팅 (SSE)

**GET** `/api/chat/stream`

서버 전송 이벤트(SSE)를 통한 스트리밍 채팅입니다.

**응답**: 서버 전송 이벤트 스트림

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### 세션 목록 가져오기

**GET** `/api/chat/conversations`

모든 활성 채팅 세션 목록을 반환합니다.

**응답 예시**:
```json
{
  "conversations": [
    {
      "sessionId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
      "beingId": "being-uuid",
      "type": "single",
      "displayName": "小游와 채팅",
      "lastMessage": "마지막 메시지 내용",
      "lastTime": "2026-05-20T10:30:00Z"
    }
  ]
}
```

### 메시지 기록 가져오기

**GET** `/api/chat/messages`

쿼리 매개변수: `channelId` — 채널/세션 ID

지정된 세션의 메시지 기록을 반환합니다.

### 채팅 기록 가져오기

**GET** `/api/chat/history`

전역 채팅 기록을 반환합니다.

### 메시지 보내기

**POST** `/api/chat/send`

**요청 본문**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
  "content": "테스트 메시지 내용"
}
```

**응답**:
```json
{
  "success": true,
  "messageId": "50156b26-f3b9-4735-be3d-51e547bd3a4a"
}
```

### AI 생각 중지

**POST** `/api/chat/stop`

현재 진행 중인 AI 응답 생성을 중지합니다.

**요청 본문**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d"
}
```

### 파일 업로드

**POST** `/api/chat/upload`

채팅 세션에 파일을 업로드합니다(multipart/form-data 지원).

---

## 실리콘 비잉 관리

### 비잉 관리 페이지

**GET** `/beings`

실리콘 비잉 관리 인터페이스 페이지를 반환합니다.

### 비잉 목록 가져오기

**GET** `/api/beings` 또는 **GET** `/api/beings/list`

등록된 모든 실리콘 비잉 목록을 반환합니다.

**응답 예시**:
```json
{
  "beings": [
    {
      "id": "being-uuid",
      "name": "Assistant",
      "status": "running",
      "soulPath": "path/to/soul.md"
    }
  ]
}
```

**상태값**: `idle` | `running` | `waiting_permission` | `stopped`

### 비잉 상세정보 가져오기

**GET** `/api/beings/detail`

쿼리 매개변수: `beingId` — 비잉 ID

지정된 비잉의 상세 정보를 반환합니다.

### 비잉 활동 상태 가져오기

**GET** `/api/beings/activity`

각 비잉의 활동 상태 정보를 반환합니다.

### 소울 파일 편집기 페이지

**GET** `/beings/soul`

소울 파일 편집기 인터페이스를 반환합니다.

### 소울 파일 저장

**POST** `/api/beings/soul/save`

**요청 본문**:
```json
{
  "beingId": "being-uuid",
  "soulContent": "# Personality\nYou are helpful..."
}
```

### AI 설정 편집기 페이지

**GET** `/beings/ai-config`

AI 설정 편집기 인터페이스를 반환합니다.

### AI 설정 저장

**POST** `/api/beings/ai-config/save`

**요청 본문**:
```json
{
  "beingId": "being-uuid",
  "aiClientType": "DashScope",
  "config": {
    "apiKey": "...",
    "region": "beijing",
    "model": "qwen3.6-plus"
  }
}
```

### 사용 가능한 AI 모델 목록 가져오기

**GET** `/api/beings/ai-config/models`

쿼리 매개변수: `clientType`, `apiKey`, `region`

지정된 AI 클라이언트의 사용 가능한 모델 목록을 반환합니다.

---

## 채팅 기록 보기

### 채팅 기록 페이지

**GET** `/chat-history`

채팅 기록 메인 페이지를 반환합니다.

### 채팅 기록 상세 페이지

**GET** `/chat-history-detail`

지정된 세션의 채팅 기록 상세 페이지를 반환합니다.

### 그룹 채팅 기록 상세 페이지

**GET** `/group-chat-history-detail`

그룹 채팅의 기록 상세 페이지를 반환합니다.

### 브로드캐스트 기록 상세 페이지

**GET** `/broadcast-history-detail`

브로드캐스트 채널의 기록 상세 페이지를 반환합니다.

### 기록 세션 목록 가져오기

**GET** `/api/chat-history/conversations`

모든 기록 세션 목록을 반환합니다.

### 기록 메시지 가져오기

**GET** `/api/chat-history/messages`

쿼리 매개변수: `sessionId` — 세션 ID

지정된 기록 세션의 메시지 기록을 반환합니다.

---

## 타이머 관리

### 타이머 페이지

**GET** `/timers`

타이머 관리 인터페이스 페이지를 반환합니다.

### 타이머 목록 가져오기

**GET** `/api/timers/list`

모든 타이머 목록을 반환합니다.

### 타이머 주기 상세 페이지

**GET** `/timer-cycles/{timerId}`

지정된 타이머의 실행 주기 상세 페이지를 반환합니다.

### 타이머 주기 목록 가져오기

**GET** `/api/timer-cycles/list`

쿼리 매개변수: `timerId` — 타이머 ID

지정된 타이머의 모든 실행 주기 목록을 반환합니다.

### 단일 실행 주기 상세 페이지

**GET** `/timer-cycle/{cycleIndex}`

단일 실행의 상세 페이지를 반환합니다.

### 주기 메시지 가져오기

**GET** `/api/timer-cycle/messages`

쿼리 매개변수: `cycleIndex` — 주기 인덱스

지정된 실행 주기의 관련 메시지를 반환합니다.

---

## 태스크 관리

### 태스크 페이지

**GET** `/tasks`

태스크 관리 인터페이스 페이지를 반환합니다.

### 태스크 목록 가져오기

**GET** `/api/tasks/list`

모든 태스크 목록을 반환합니다.

### 태스크 주기 상세 페이지

**GET** `/task-cycles/{taskId}`

지정된 태스크의 실행 주기 상세 페이지를 반환합니다.

### 태스크 주기 목록 가져오기

**GET** `/api/task-cycles/list`

쿼리 매개변수: `taskId` — 태스크 ID

지정된 태스크의 모든 실행 주기 목록을 반환합니다.

### 단일 실행 주기 상세 페이지

**GET** `/task-cycle/{cycleIndex}`

단일 태스크 실행의 상세 페이지를 반환합니다.

### 주기 메시지 가져오기

**GET** `/api/task-cycle/messages`

쿼리 매개변수: `cycleIndex` — 주기 인덱스

지정된 태스크 실행 주기의 관련 메시지를 반환합니다.

---

## 퍼미션 시스템

### 퍼미션 관리 페이지

**GET** `/permissions`

퍼미션 관리 인터페이스 페이지를 반환합니다.

### 퍼미션 규칙 목록 가져오기

**GET** `/api/permissions/list`

현재 구성된 모든 퍼미션 규칙을 반환합니다.

**응답 예시**:
```json
{
  "rules": [
    {
      "permissionType": "NetworkAccess",
      "resourcePrefix": "api.github.com",
      "result": "Allowed",
      "description": "Allow GitHub API access"
    }
  ]
}
```

### 퍼미션 규칙 저장

**POST** `/api/permissions/save`

**요청 본문**:
```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects",
  "result": "Allowed",
  "description": "Allow project directory access"
}
```

### 퍼미션 요청 페이지

**GET** `/permission/request`

퍼미션 요청 페이지를 표시하여 사용자가 실리콘 비잉의 퍼미션 요청을 승인하거나 거부할 수 있도록 합니다.

**쿼리 매개변수**:

| 매개변수 | 유형 | 설명 |
|------|------|------|
| `userId` | `Guid` | 퍼미션을 요청하는 실리콘 비잉 ID |
| `type` | `string` | 퍼미션 타입 |
| `resource` | `string` | 요청된 리소스 경로 |
| `allowCode` | `string` | 허용 작업의 코드 식별자 |
| `denyCode` | `string` | 거부 작업의 코드 식별자 |

### 대기 중인 퍼미션 요청 확인

**GET** `/permission/check`

쿼리 매개변수: `userId` — 실리콘 비잉 ID

**응답**:
```json
{
  "pending": true
}
```

### 퍼미션 요청 응답

**GET** `/permission/respond`

**쿼리 매개변수**:

| 매개변수 | 유형 | 설명 |
|------|------|------|
| `userId` | `Guid` | 실리콘 비잉 ID |
| `allowed` | `bool` | 허용 여부 |
| `addToCache` | `bool` | 결정을 캐시할지 여부 |
| `cacheDuration` | `double` | 캐시 지속 시간 (시간) |

**응답**:
```json
{
  "success": true
}
```

---

## 로깅 시스템

### 로그 페이지

**GET** `/logs`

로그 보기 인터페이스 페이지를 반환합니다.

### 로그 목록 가져오기

**GET** `/api/logs/list`

쿼리 매개변수로 레벨, 시간 범위별 필터링을 지원합니다.

**응답 예시**:
```json
{
  "logs": [
    {
      "timestamp": "2026-04-20T10:30:00Z",
      "level": "error",
      "message": "Failed to connect to AI service",
      "source": "OllamaClient"
    }
  ]
}
```

### 비잉별 로그 가져오기

**GET** `/api/logs/beings`

실리콘 비잉별로 그룹화된 로그 통계입니다.

### 사용 가능한 로그 레벨 가져오기

**GET** `/api/logs/levels`

시스템에서 사용 가능한 로그 레벨 목록을 반환합니다.

---

## 사용 통계

### 사용 통계 페이지

**GET** `/usage`

사용 통계 인터페이스 페이지를 반환합니다.

### 사용 요약 가져오기

**GET** `/api/usage/summary`

토큰 사용량 및 비용 요약을 반환합니다.

### 추세 데이터 가져오기

**GET** `/api/usage/trend`

쿼리 매개변수: `startDate`, `endDate`

지정된 기간의 사용 추세 데이터를 반환합니다.

### 사용 데이터 내보내기

**GET** `/api/usage/export`

사용 데이터를 다운로드 가능한 형식으로 내보냅니다.

---

## 감사 추적

### 감사 페이지

**GET** `/audit`

감사 추적 인터페이스 페이지를 반환합니다.

### 감사 목록 가져오기

**GET** `/api/audit/list`

감사 로그 항목 목록을 반환합니다.

### 감사 요약 가져오기

**GET** `/api/audit/summary`

감사 데이터의 요약 통계를 반환합니다.

### 비잉별 감사 가져오기

**GET** `/api/audit/beings`

실리콘 비잉별로 그룹화된 감사 통계입니다.

---

## 설정 관리

### 설정 페이지

**GET** `/config`

시스템 설정 인터페이스 페이지를 반환합니다.

### 설정 저장

**POST** `/config/save`

**요청 본문**:
```json
{
  "language": "ZhCN",
  "port": 8080,
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:7b"
    },
    "DashScope": {
      "apiKey": "...",
      "region": "beijing",
      "model": "qwen3.6-plus"
    }
  }
}
```

### AI 설정 옵션 가져오기

**GET** `/config/aioptions`

사용 가능한 AI 클라이언트 유형 및 동적 옵션(사용 가능한 모델, 리전 등)을 반환합니다.

---

## 메모리 시스템

### 메모리 페이지

**GET** `/memory`

메모리 관리 인터페이스 페이지를 반환합니다.

### 메모리 목록 가져오기

**GET** `/api/memory/list`

실리콘 비잉의 메모리 항목 목록을 반환합니다.

### 메모리 상세정보 가져오기

**GET** `/api/memory/detail/{id}`

경로 매개변수: `id` — 메모리 항목 ID

지정된 메모리 항목의 전체 내용을 반환합니다.

### 메모리 통계 가져오기

**GET** `/api/memory/stats`

메모리 시스템의 통계 정보를 반환합니다.

### 메모리 검색

**GET** `/api/memory/search`

쿼리 매개변수: `keyword` — 검색 키워드

일치하는 메모리 항목을 검색합니다.

### 비잉별 메모리 가져오기

**GET** `/api/memory/beings`

실리콘 비잉별로 그룹화된 메모리 통계입니다.

### 메모리 추적 가져오기

**GET** `/api/memory/trace/{id}`

경로 매개변수: `id` — 메모리 항목 ID

지정된 메모리 항목의 출처 추적 체인을 반환합니다.

### 메모리 타임라인 HTML 가져오기

**GET** `/api/memory/timeline-html`

메모리 타임라인의 HTML 보기를 반환합니다.

---

## 워크 노트

### 워크 노트 페이지

**GET** `/work-notes`

워크 노트 인터페이스 페이지를 반환합니다.

### 워크 노트 목록 가져오기

**GET** `/api/work-notes/list`

워크 노트 목록을 반환합니다.

### 워크 노트 읽기

**GET** `/api/work-notes/read`

쿼리 매개변수: `noteId` — 노트 ID

지정된 노트의 내용을 반환합니다.

### 노트 디렉토리 가져오기

**GET** `/api/work-notes/directory`

노트 디렉토리 구조를 반환합니다.

### 워크 노트 검색

**GET** `/api/work-notes/search`

쿼리 매개변수: `keyword` — 검색 키워드

일치하는 워크 노트를 검색합니다.

### 워크 노트 만들기

**POST** `/api/work-notes/create`

**요청 본문**:
```json
{
  "title": "노트 제목",
  "content": "노트 내용",
  "keywords": ["키워드1", "키워드2"]
}
```

### 워크 노트 업데이트

**POST** `/api/work-notes/update`

**요청 본문**:
```json
{
  "noteId": "note-uuid",
  "title": "업데이트된 제목",
  "content": "업데이트된 내용"
}
```

### 워크 노트 삭제

**POST** `/api/work-notes/delete`

**요청 본문**:
```json
{
  "noteId": "note-uuid"
}
```

---

## 노리지 네트워크

### 노리지 네트워크 페이지

**GET** `/knowledge`

노리지 네트워크 관리 인터페이스 페이지를 반환합니다.

### 지식 그래프 가져오기

**GET** `/api/knowledge/graph`

지식 삼중항 그래프 데이터(주체-관계-객체)를 반환합니다.

---

## 프로젝트 관리

### 프로젝트 페이지

**GET** `/project`

프로젝트 관리 인터페이스 페이지를 반환합니다.

### 프로젝트 워크 노트 페이지

**GET** `/project/{id}/work-notes`

경로 매개변수: `id` — 프로젝트 ID

지정된 프로젝트의 워크 노트 페이지를 반환합니다.

### 프로젝트 태스크 페이지

**GET** `/project/{id}/tasks`

경로 매개변수: `id` — 프로젝트 ID

지정된 프로젝트의 태스크 관리 페이지를 반환합니다.

### 프로젝트 툴 퍼미션 페이지

**GET** `/project/{id}/tool-permissions`

경로 매개변수: `id` — 프로젝트 ID

지정된 프로젝트의 툴 퍼미션 관리 페이지를 반환합니다.

### 프로젝트 워크플로우 페이지

**GET** `/project/{id}/workflow`

경로 매개변수: `id` — 프로젝트 ID

지정된 프로젝트의 워크플로우 관리 페이지를 반환합니다.

### 프로젝트 워크플로우 상세정보 가져오기

**GET** `/api/projects/workflow-detail`

쿼리 매개변수: `projectId` — 프로젝트 ID

프로젝트에 연결된 워크플로우 상세 정보를 반환합니다.

### 프로젝트 역할 할당

**POST** `/api/projects/assign-role`

**요청 본문**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### 프로젝트 역할 제거

**POST** `/api/projects/remove-role`

**요청 본문**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### 프로젝트 목록 가져오기

**GET** `/api/projects/list`

모든 프로젝트 목록을 반환합니다.

### 프로젝트 워크플로우 템플릿 목록 가져오기

**GET** `/api/projects/list-workflow-templates`

사용 가능한 워크플로우 템플릿 목록을 반환합니다.

### 프로젝트 만들기

**POST** `/api/projects/create`

**요청 본문**:
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### 프로젝트 보관

**POST** `/api/projects/{id}/archive`

경로 매개변수: `id` — 프로젝트 ID

지정된 프로젝트를 보관합니다.

### 프로젝트 복원

**POST** `/api/projects/{id}/restore`

경로 매개변수: `id` — 프로젝트 ID

보관된 프로젝트를 복원합니다.

### 프로젝트 파기

**POST** `/api/projects/{id}/destroy`

경로 매개변수: `id` — 프로젝트 ID

지정된 프로젝트를 영구 삭제합니다(복원 불가).

### 프로젝트 상세정보 가져오기

**GET** `/api/projects/detail`

쿼리 매개변수: `projectId` — 프로젝트 ID

프로젝트의 상세 정보를 반환합니다.

### 프로젝트 업데이트

**POST** `/api/projects/update`

**요청 본문**:
```json
{
  "projectId": "project-uuid",
  "name": "Updated Name",
  "description": "Updated description"
}
```

### 프로젝트에 멤버 할당

**POST** `/api/projects/assign`

**요청 본문**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### 프로젝트에서 멤버 제거

**POST** `/api/projects/remove`

**요청 본문**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### 프로젝트 워크 노트 목록 가져오기

**GET** `/api/projects/{id}/work-notes/list`

경로 매개변수: `id` — 프로젝트 ID

지정된 프로젝트의 워크 노트 목록을 반환합니다.

### 프로젝트 워크 노트 읽기

**GET** `/api/projects/{id}/work-notes/read`

경로 매개변수: `id` — 프로젝트 ID

지정된 프로젝트의 워크 노트 내용을 반환합니다.

### 프로젝트 워크 노트 만들기

**POST** `/api/projects/{id}/work-notes/create`

경로 매개변수: `id` — 프로젝트 ID

지정된 프로젝트에 새 워크 노트를 만듭니다.

### 프로젝트 워크 노트 업데이트

**POST** `/api/projects/{id}/work-notes/update`

경로 매개변수: `id` — 프로젝트 ID

지정된 프로젝트의 워크 노트를 업데이트합니다.

### 프로젝트 워크 노트 삭제

**POST** `/api/projects/{id}/work-notes/delete`

경로 매개변수: `id` — 프로젝트 ID

지정된 프로젝트의 워크 노트를 삭제합니다.

### 프로젝트 태스크 목록 가져오기

**GET** `/api/projects/{id}/tasks/list`

경로 매개변수: `id` — 프로젝트 ID

지정된 프로젝트의 태스크 목록을 반환합니다.

### 프로젝트 태스크 만들기

**POST** `/api/projects/{id}/tasks/create`

경로 매개변수: `id` — 프로젝트 ID

지정된 프로젝트에 새 태스크를 만듭니다.

### 프로젝트 태스크 업데이트

**POST** `/api/projects/{id}/tasks/update`

경로 매개변수: `id` — 프로젝트 ID

지정된 프로젝트의 태스크를 업데이트합니다.

### 프로젝트 태스크 삭제

**POST** `/api/projects/{id}/tasks/delete`

경로 매개변수: `id` — 프로젝트 ID

지정된 프로젝트의 태스크를 삭제합니다.

### 태스크 담당자 할당

**POST** `/api/projects/{id}/tasks/assign`

경로 매개변수: `id` — 프로젝트 ID

프로젝트 태스크에 담당자를 할당합니다.

### 태스크 담당자 제거

**POST** `/api/projects/{id}/tasks/remove-assignee`

경로 매개변수: `id` — 프로젝트 ID

프로젝트 태스크의 담당자를 제거합니다.

### 태스크 완료 표시

**POST** `/api/projects/{id}/tasks/complete`

경로 매개변수: `id` — 프로젝트 ID

프로젝트 태스크를 완료로 표시합니다.

### 태스크 실패 표시

**POST** `/api/projects/{id}/tasks/fail`

경로 매개변수: `id` — 프로젝트 ID

프로젝트 태스크를 실패로 표시합니다.

### 태스크 취소

**POST** `/api/projects/{id}/tasks/cancel`

경로 매개변수: `id` — 프로젝트 ID

프로젝트 태스크를 취소합니다.

---

## 툴 퍼미션 관리

### 실리콘 비잉 툴 퍼미션 가져오기

**GET** `/api/beings/tool-permissions`

쿼리 매개변수: `beingId` — 실리콘 비잉 ID

지정된 실리콘 비잉의 툴 퍼미션 구성을 반환합니다.

### 실리콘 비잉 툴 퍼미션 업데이트

**PUT** `/api/beings/tool-permissions`

**요청 본문**:
```json
{
  "beingId": "being-uuid",
  "permissions": {
    "network": "allowed",
    "disk_read": "allowed",
    "disk_write": "denied"
  }
}
```

### 툴 퍼미션 템플릿 가져오기

**GET** `/api/beings/tool-permissions/templates`

사용 가능한 툴 퍼미션 템플릿 목록을 반환합니다.

### 툴 퍼미션 템플릿 적용

**POST** `/api/beings/tool-permissions/apply-template`

**요청 본문**:
```json
{
  "beingId": "being-uuid",
  "templateName": "readonly"
}
```

### 프로젝트 툴 퍼미션 가져오기

**GET** `/api/projects/{id}/tool-permissions`

경로 매개변수: `id` — 프로젝트 ID

지정된 프로젝트의 툴 퍼미션 구성을 반환합니다.

### 프로젝트 툴 퍼미션 업데이트

**PUT** `/api/projects/{id}/tool-permissions`

경로 매개변수: `id` — 프로젝트 ID

**요청 본문**:
```json
{
  "permissions": {
    "network": "allowed",
    "disk_read": "allowed",
    "disk_write": "denied"
  }
}
```

---

## 이그제큐터 관리

### 이그제큐터 페이지

**GET** `/executor`

이그제큐터 관리 인터페이스 페이지를 반환합니다.

### 이그제큐터 상태 가져오기

**GET** `/api/executors/status`

각 이그제큐터(디스크, 네트워크, 커맨드라인)의 실행 상태를 반환합니다.

---

## 코드 브라우저

### 코드 브라우저 페이지

**GET** `/code`

코드 브라우저 인터페이스 페이지를 반환합니다.

### 코드 유형 목록 가져오기

**GET** `/api/code/types`

지원되는 코드 유형/언어 목록을 반환합니다.

### 코드 상세정보 가져오기

**GET** `/api/code/detail`

쿼리 매개변수: `filePath`, `lineNumber`

지정된 파일의 코드 상세 정보를 반환합니다.

---

## 코드 호버 팁

### 호버 팁 가져오기

**GET** `/api/code/hover`
**POST** `/api/code/hover`

코드 위치의 호버 팁 정보를 가져옵니다(IDE의 스마트 팁과 유사).

### 코드 위치 등록

**POST** `/api/code/register`

모니터링이 필요한 코드 위치를 등록합니다.

### 코드 위치 업데이트

**POST** `/api/code/update`

등록된 코드 위치 정보를 업데이트합니다.

### 코드 위치 등록 해제

**POST** `/api/code/unregister`

더 이상 필요하지 않은 코드 위치 모니터링을 등록 해제합니다.

---

## 도움말 문서 시스템

### 도움말 페이지

**GET** `/help` 또는 **GET** `/help/index`

도움말 문서 메인 페이지를 반환합니다.

### 도움말 주제 페이지

**GET** `/help/{topic}`

경로 매개변수: `topic` — 주제 식별자

지정된 주제의 도움말 문서 페이지를 반환합니다.

### 도움말 문서 검색

**GET** `/api/help/search`

쿼리 매개변수: `keyword` — 검색 키워드

일치하는 도움말 문서 주제를 검색합니다.

---

## 초기화

### 초기화 마법사 페이지

**GET** `/init`

최초 실행 초기화 마법사 페이지를 반환합니다.

### 초기화 제출

**POST** `/init`

최초 실행의 초기화 구성을 제출합니다.

### 데이터 디렉토리 찾아보기

**GET** `/init/browse`

데이터 저장 위치를 선택하기 위한 디렉토리 브라우저를 엽니다.

### AI 설정 메타데이터 가져오기

**GET** `/init/ai-config-metadata`

사용 가능한 AI 클라이언트 유형 및 구성 필드 메타데이터를 반환합니다.

---

## 시스템 제어

### 정상 종료

**POST** `/api/system/shutdown`

> **참고**: localhost에서의 요청만 허용됩니다

애플리케이션의 정상 종료 프로세스를 트리거합니다:

1. 메인 루프 중지
2. 현재 구성 저장
3. HTTP 리스너 종료

**응답**:
```json
{
  "status": "shutting_down",
  "message": "Application is shutting down gracefully"
}
```

---

## 정보

### 정보 페이지

**GET** `/about`

시스템 정보와 로드된 플러그인 목록이 포함된 정보 페이지를 반환합니다.

**플러그인 목록 데이터**:
```json
{
  "plugins": {
    "plugin-id": {
      "name": "My Plugin",
      "version": "1.0.0",
      "description": "Plugin description",
      "author": "Author Name"
    }
  }
}
```

---

## 오류 응답

모든 엔드포인트는 표준화된 오류 응답을 반환합니다:

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: FileAccess, Denied by GlobalACL"
  }
}
```

### 일반 오류 코드

| 코드 | HTTP 상태 | 설명 |
|------|-------------|-------------|
| `PERMISSION_DENIED` | 403 | 퍼미션 부족 |
| `NOT_FOUND` | 404 | 리소스를 찾을 수 없음 |
| `VALIDATION_ERROR` | 400 | 요청 매개변수가 유효하지 않음 |
| `INTERNAL_ERROR` | 500 | 내부 서버 오류 |
| `SERVICE_UNAVAILABLE` | 503 | AI 서비스 사용 불가 |

---

## SSE 이벤트

서버 전송 이벤트는 실시간 업데이트에 사용됩니다:

### 채팅 이벤트

```javascript
const eventSource = new EventSource('/api/chat/stream');

eventSource.onmessage = (event) => {
  const data = JSON.parse(event.data);

  switch(data.type) {
    case 'chunk':
      console.log('Streaming:', data.content);
      break;
    case 'tool_call':
      console.log('Tool executing:', data.tool);
      break;
    case 'complete':
      console.log('Chat complete, session:', data.sessionId);
      break;
    case 'error':
      console.error('Error:', data.message);
      break;
  }
};
```

---

## AI 클라이언트 인터페이스

### IAIClient 인터페이스

```csharp
public interface IAIClient
{
    string Name { get; }

    Task<AIResponse> ChatAsync(AIRequest request);

    IAsyncEnumerable<string> StreamChatAsync(AIRequest request);
}
```

### AIRequest 구조

```csharp
public class AIRequest
{
    public List<Message> Messages { get; set; }
    public List<ToolDefinition> Tools { get; set; }
    public double Temperature { get; set; } = 0.7;
    public int MaxTokens { get; set; } = 2000;
    public string Model { get; set; }
}
```

### AIResponse 구조

```csharp
public class AIResponse
{
    public string Content { get; set; }
    public List<ToolCall> ToolCalls { get; set; }
    public TokenUsage Usage { get; set; }
    public string Model { get; set; }
}
```

---

## 툴 시스템 인터페이스

### ITool 인터페이스

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    ToolDefinition Definition { get; }

    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### ToolCall 구조

```csharp
public class ToolCall
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}
```

### ToolResult 구조

```csharp
public class ToolResult
{
    public bool Success { get; set; }
    public string Output { get; set; }
    public string Error { get; set; }
}
```

---

## 다음 단계

- 🚀 [빠른 시작 가이드](getting-started.md) 보기
- 🛠️ [개발 가이드](development-guide.md) 읽기
- 📚 [아키텍처 문서](architecture.md) 확인
- 🔒 [보안 모델](security.md) 알아보기
