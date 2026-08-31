# 변경 로그

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | **한국어** | [Čeština](../cs-CZ/changelog.md) | [Русский](../ru-RU/changelog.md)

이 프로젝트의 모든 중요한 변경 사항은 이 파일에 기록됩니다.

형식은 [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)를 기반으로 하며,
이 프로젝트는 [시맨틱 버전 관리](https://semver.org/spec/v2.0.0.html)를 따릅니다.

---

## 이 변경 로그에 대하여

### 프로젝트 듀얼 버전

이 프로젝트는 두 가지 구현 버전을 제공합니다:

- **SiliconLife.Default**: 기본 구현으로, 주로 아키텍처 타당성 검증에 사용됩니다. 콘솔 애플리케이션, 파일 시스템 JSON 저장소.
- **SiliconLife.Fast**: 주력 프로덕션 버전입니다. 크로스 플랫폼 데스크톱 애플리케이션(Windows / macOS / Linux), SpeedyPack 메모리 저장소 + 비동기 영속화, 심층 성능 최적화가 적용되었습니다.

두 버전은 동일한 인터페이스와 기능을 공유하며, 저장소 구현과 실행 모드만 다릅니다. SiliconLife.Default는 아키텍처 검증 기준으로, SiliconLife.Fast는 프로덕션 환경 주력 버전으로 사용됩니다.

### 프로젝트 기원

- 이 프로젝트는 2026년 3월 20일에 시작되었습니다.
- 이 프로젝트 이전에 아키텍처 설계가 부적절하여 실패한 검증 Demo가 있었으며, 여러 AI 플랫폼과의 통합이 불가능했습니다.

### 사용된 AI IDE 도구

#### Kiro (Amazon AWS)
- 프로젝트는 처음에 Kiro로 유지보수되었으며, Spec 모드로 시작되었습니다.
- Kiro는 Amazon AWS가 구축한 agentic AI 개발 환경입니다.
- Code OSS(VS Code) 기반으로, VS Code 설정 및 Open VSX 호환 플러그인을 지원합니다.
- 구조화된 AI 코딩을 위한 스펙 기반 개발 워크플로우를 갖추고 있습니다.

#### Comate AI IDE / 文心快码 (바이두)
- 문안 및 문서 작업에 간헐적으로 사용되었습니다.
- Comate AI IDE는 바이두 문심이 2025년 6월 23일에 발표한 AI 네이티브 개발 환경 도구입니다.
- 업계 최초의 멀티모달, 멀티 에이전트 협업 AI IDE입니다.
- 디자인-코드 변환 및 전 과정 AI 보조 코딩 기능을 포함합니다.
- 바이두 문심 4.0 X1 Turbo 모델로 구동됩니다.

#### Trae (바이트댄스)
- 2025년 10월부터 2026년 4월까지 사용되었습니다.
- 스마트 코드 생성 및 프로젝트 관리를 지원하는 AI IDE입니다.

#### Qoder (알리바바)
- 2026년 4월 18일부터 프로젝트 유지보수에 사용되었습니다.
- 코드 분석, 문서 생성, 멀티 에이전트 협업을 지원하는 AI 코딩 플랫폼입니다.

#### CatPaw (메이퇀)
- 2026년 5월 6일부터 Qoder와 혼합하여 사용 중입니다.
- 메이퇀 자체 개발 LongCat 시리즈 모델 기반으로, 강력한 전체 코드 아키텍처 리팩토링 능력을 갖추고 있습니다.

#### DuMate (Baidu Qianfan)
- 2026년 7월부터 코드 개발, 현지화, 문서 작성에 사용.
- 천판 데스크톱 플랫폼에서 실행되는 범용 AI 어시스턴트. 다중 도구 오케스트레이션, 파일 조작, 브라우저 자동화, 다단계 작업 실행 가능.
- 사용자의 Windows 데스크톱에서 로컬 파일 읽기/쓰기, 셸 명령 실행, 웹 검색을 직접 수행.

### 요구사항 문서

- 이 프로젝트의 요구사항 문서는 공개되지 않았습니다.
- 요구사항은 12개 이상의 국제 AI 플랫폼 및 대형 모델 시리즈를 통한 반복 검증을 거쳤으며, 2000줄 이상의 거의 인간이 이해할 수 없는 사용자 스토리 기반 요구사항 문서가 산출되었습니다.

---

## [미발표]

### 2026-08-27

#### 새로운 기능
- LongCat 클라이언트를 LongCat-2.0 모델로 업그레이드
  - 컨텍스트 윈도우를 128K에서 1M(1,048,576 Tokens)로 확장, 최대 출력 128K
  - 사고 모드(thinking) 지원 추가, 요청 본문에 thinking type enabled 포함
  - 사전 정의된 모델 목록 추가(LongCat-2.0 / Flash-Chat / Pro-Chat / Max-Chat), API 동적 가져오기 실패 시 폴백
  - 기본 엔드포인트를 https://api.longcat.chat/openai 로 업데이트
  - 13개 언어 파일에 모델 표시 이름 추가, 도움말 문서의 요금 정보 및 등록 링크 업데이트


### 2026-08-17

#### 새로운 기능
- `c7b575b` - MCP 통합 구현——외부 서버 도구 접속, 설정 관리 및 도움말 문서
  - MCP 코어 신규 추가(SiliconLife.Core/Mcp/): McpManager 서버 라이프사이클 관리, stdio/http 듀얼 전송, McpClientConnection 연결 래핑, 서버별 도구 래핑 및 `mcp_{serverId}_{toolName}` 명명으로 모든 실리콘 비잉에 주입
  - Web 관리 페이지(/mcp) 및 7개 API 엔드포인트 신규 추가(list-servers/list-tools/add-server/toggle/remove-server/reconnect/test-tool)
  - McpTool 쿼리 도구 신규 추가(status/list_servers/list_tools, 읽기 전용); 서버 추가/삭제는 사용자가 Web UI를 통해서만 가능, AI는 서버 목록 수정 불가
  - 설정 페이지에서 MCP 서버 배열 에디터 지원(모달 창 내 인라인 추가/삭제)
  - MCP 도움말 주제 등록(🔌), 10개 언어로 완전한 도움말 문서 구현
  - MCP 래핑 도구는 권한 매트릭스에서 `execute` 액션으로 표시, 비잉/프로젝트별 비활성화 지원
  - 45 파일 변경

### 2026-08-16

#### 새로운 기능
- `5d76c5a` - 스킬 시스템 구현——도구 오케스트레이션 및 프롬프트 템플릿 재사용 추상화 계층
  - SkillDefinition 신규 추가(id/설명/매개변수 schema/시스템 프롬프트 템플릿/도구 화이트리스트/액션 제한/최대 라운드/타임아웃/완료 액션/트리거 모드)
  - SkillManager 신규 추가: 스킬 등록 센터 + 실행 엔진(하위 AIRequest 루프, 재귀 방지, 글로벌 라운드 및 타임아웃 제한)
  - 듀얼 트리거 모드: Manual(AI 함수 호출, 스킬이 ToolDefinition으로 주입, 스케줄링 측 우선 라우팅) + Auto(schedule 스케줄링, `HH:mm` / `N s|m|h|d` / cron 서브셋 지원)
  - Markdown 우선 저장(YAML 프론트 + 프롬프트 본문), 순수 Markdown은 AI가 자동으로 메타데이터 보완(사용자 필드는 덮어쓰지 않음)
  - 핫 리로드(30초 지문 감지), 버전 아카이브(skills/archive/), 3개 내장 스킬(summarize_document/code_review/research_topic)
  - skill 도구 신규 추가(create/list/update/update_from_md/delete/export/export_md/import/import_md)
  - 스킬 관리 페이지(/skill) 및 10개 API 엔드포인트 신규 추가; 할당량 MaxCustomSkillsPerBeing(기본값 50)
  - 권한: 스킬 레벨 `execute` 액션 권한, 스킬 내 도구 화이트리스트와 비잉 권한의 엄격한 측 합집합
- `b60fc68` - 첸팬 모델 목록 및 컨텍스트 윈도우 매핑 업데이트 - glm-5.2/glm-5.1/deepseek-v4-pro/deepseek-v4-flash/kimi-k2.6/ernie-5.1/qianfan-code-latest 모델 신규 추가, 1M/128K 계층별 컨텍스트 윈도우 및 시각 능력 매핑

### 2026-08-15

#### 새로운 기능
- `eaa8417` - IM 플랫폼 OAuth 인증 마법사 및 설정 비밀 환경변수 해석 구현
  - ImOAuthController/ImOAuthService 신규 추가, Feishu OAuth 인증 흐름 지원(authorize/callback/status), state CSRF 방지, 5분 타임아웃, SSE 상태 푸시 포함
  - IMProviderRegistry 신규 추가, IM 플랫폼 메타데이터 통합 관리(설정 필드 schema/OAuth 엔드포인트 템플릿/Provider 팩토리)
  - ConfigSecretResolver 신규 추가, 설정 내 `${ENV_VAR}` 플레이스홀더 해석, 딥카피 교체로 원본 설정에 기록하지 않음
  - 설정 페이지에 IM 인증 마법사 UI 통합(인라인 인증 영역 + SSE 실시간 상태)
  - 13개 언어 파일의 IM 인증 상태/도움말 문안 번역 완성

### 2026-07-26

#### 리팩토링
- `ffc45c2` - IM 플랫폼을 다중 인스턴스 설정 아키텍처로 리팩토링 - IMPlatforms 목록화(각 플랫폼 독립 활성화/비활성화), AggregateIMProvider 다중 플랫폼 메시지 수발신 및 권한 경쟁, 설정 페이지 다중 인스턴스 에디터

### 2026-07-19

#### 새로운 기능
- `9bf2103` - Speedy.Manager 트리뷰에 다중 선택 삭제 및 다중 선택 내보내기 통합

#### 수정
- `0df0674` - Speedy.Manager 다중 선택 삭제 시 첫 번째 항목만 삭제되는 문제 수정

### 2026-07-16

#### 새로운 기능
- `7431312` - 13개 언어 파일의 AI 클라이언트 설정 번역 완성 - CsCZ/PlPL을 stub에서 완전한 딕셔너리 구현으로 변경, 나머지 10개 파일에 7개 신규 클라이언트(DeepSeek/ZhipuGLM/MoonshotKimi/SiliconFlow/MiniMax/Ernie/Hunyuan)의 ConfigDisplayNames/ConfigDescriptions/ConfigGroupNames 항목 추가, 6개 ClientFactory의 설정 키 메타데이터 동기화
  - 20 파일 변경

#### 문서
- `d6608ea` - 모든 13개 언어 버전의 changelog에 DuMate(바이두 첸팬) AI IDE 도구 소개 추가
  - 13 파일 변경

#### 협업 프레임워크
- `c607c97` - DuMate(바이두 첸팬)를 상주 AI 액터로 .ai-collab 레지스트리에 등록
  - 1 파일 변경


### 2026-07-15

#### 새로운 기능
- `c007263` - 10개 AI 클라이언트의 도움말 문서 완성 - HelpTopics에 10개 주제 등록, HelpLocalizationBase에 30개 추상 속성 추가, 12개 언어 파일에서 완전한 Markdown 도움말 내용(플랫폼 소개/등록 단계/설정 방법/사용 가능 모델/과금/FAQ) 구현, Herdsman/LongCat/QiniuAI/DeepSeek/ZhipuGLM/MoonshotKimi/SiliconFlow/MiniMax/Ernie/Hunyuan 포함
  - 12 파일 변경
- `4634e33` - 국내 7개 AI 플랫폼 클라이언트 구현(DeepSeek/지푸GLM/문샷Kimi/실리콘플로우/MiniMax/바이두원심/텐센트혼원) - 14개 독립 클래스 파일, LongCatClient 스타일 준수, 상속 없음, 모두 OpenAI 호환 + Bearer Token, Tool Calling/스트리밍/사고 모드 지원, DefaultSiliconBeing 및 DefaultSiliconBeingFactory에 등록
  - 16 파일 변경

#### 문서
- `108c4ea` - 전체 13개 언어 문서를 업데이트하여 7개 신규 AI 클라이언트 반영 - 상태 📋→✅, 01.AI를 더 이상 사용되지 않음으로 표시
  - 94 파일 변경


### 2026-07-14

#### 문서
- `344b429` - 전체 언어 architecture.md AI 플랫폼 상태에 "더 이상 사용되지 않음" 상태 추가, 01.AI를 더 이상 사용되지 않음으로 표시(신규 사용자 등록 중지)
  - 13 파일 변경


### 2026-07-07

#### 정리
- `e06e6f2` - OsmStore 도구 체인과 TravelCodeWikiWithAI 플러그인 제거 - tools/OsmStore.* 3개 프로젝트 삭제, src/TravelCodeWikiWithAI/ 플러그인 프로젝트 삭제, sln 참조 정리, 프로젝트를 독립 버전 TCW 개발 노선으로 회귀
  - 45 파일 변경


### 2026-07-06

#### 수정
- `1b15886` - OSM 데이터 모델 표준화 및 요소 타입 안전 수정
  - 7 파일 변경


### 2026-07-05

#### 새로운 기능
- `be4320b` - TravelCodeWikiWithAI에 CLDR 데이터 제공자 모듈 추가
  - 4 파일 변경


### 2026-07-04

#### 새로운 기능
- `dbcabf3` - 플러그인 권한 시스템 강화 - 네트워크/파일 IO를 Executor 모드로 리팩토링 + GeneratedCodeAttribute 화이트리스트 면제
  - 34 파일 변경
- `e84bb63` - 컴파일 오류 수정 및 TravelCodeWikiWithAI 프로젝트 추가
  - 53 파일 변경

#### 리팩토링
- `9e5a345` - TravelCodeWikiWithAI의 PBF를 동기 온라인 OSM API로 완전 마이그레이션
  - 4 파일 변경


### 2026-05-31

#### 새로운 기능
- `a5f37bd` - 프로젝트 사고, 대화 시스템 및 저장소 관련 기능 업데이트
  - 13 파일 변경


### 2026-05-30

#### 새로운 기능
- `c3cf429` - QiniuAIClient AI 클라이언트 추가(치니우 클라우드 AI 대규모 모델 추론 서비스) (ref task-409)
  - 20 파일 변경
- `d04131f` - LongCatClient AI 클라이언트 추가(메이퇀 LongCat 대규모 모델) (ref task-408)
  - 19 파일 변경

#### 협업 프레임워크
- `e9564f5` - 수정된 모든 파일 업데이트
  - 140 파일 변경
- `9c8b42f` - 2026-05-29의 sessions 및 changes 아카이브
  - 20 파일 변경


### 2026-05-29

#### 새로운 기능
- `d548e48` - 프로젝트 사고 상세 페이지에서 메시지를 사이클(Cycle)별로 그룹화하여 표시 및 접기 지원 (ref task-407)
  - 23 파일 변경
- `28d893d` - IAIClient에 멀티모달 능력 선언 인터페이스 추가 + ChatMessage에 멀티모달 필드 추가 (ref task-402)
  - 13 파일 변경
- `ebe6a49` - 프로젝트 사고 상세 페이지에 세션 상태, 생성 시간, 완료 시간 표시 추가 (ref task-406)
  - 22 파일 변경
- `9a53d55` - IAIClient에 ContextWindowTokens + Token 예산제 + 팩토리 설정화 추가 (ref task-401, task-403)
  - 26 파일 변경
- `202b99c` - HerdsmanClient AI 클라이언트 추가 + 초기화 화면 드롭다운 새로고침 안 됨 수정 (ref task-399, task-400)
  - 20 파일 변경
- `285ab2f` - 프로젝트 처리 기록 프론트엔드 표시 (ref task-397)
  - 25 파일 변경
- `b4b633f` - ThinkOnProject 의사 Session 다중 라운드 대화 메커니즘 (ref task-395)
  - 13 파일 변경
- `d3e543f` - ThinkOnProject 시나리오 컨텍스트에 사용 가능한 실리콘 비잉 정보 추가 (ref task-394)
  - 21 파일 변경
- `07eb628` - BuildRequest에 실리콘 비잁 프로젝트 소속 정보 동적 주입 (ref task-396)
  - 21 파일 변경
- `2089696` - Tool에 Project 시나리오 지원 추가 + PluginLoader 다중 디렉토리 통합 리팩토링
  - 12 파일 변경

#### 수정
- `b80a33b` - 프로젝트 사고 상세 페이지 로딩 힌트 텍스트 영어 하드코딩 및 로컬라이제이션 누락 수정 (ref task-405)
  - 6 파일 변경
- `90b60c5` - 도구 호출 라운드에서 AI 본문 Content와 Thinking이 숨겨지는 문제 수정 (ref task-404)
  - 8 파일 변경
- `a7d9a97` - ThinkOnProject 다중 라운드 루프 연속 및 프로젝트 알림 메시지 손실 수정
  - 6 파일 변경
- `c0838dd` - ProjectThinkSession 메시지가 Cycle에 기록되지 않고 완료 후 기록이 삭제되는 문제 수정 (ref task-398)
  - 7 파일 변경
- `f3d1794` - 실리콘 비잁 Project/Broadcast/Stopped 상태 로컬라이제이션 누락 및 표시 이상 수정 (ref task-393)
  - 20 파일 변경
- `3eaa90d` - 삭제된 프로젝트 TravelCodeWikiWithAI의 솔루션 참조 제거
  - 1 파일 변경

#### 협업 프레임워크
- `f3cbed7` - task-394~396 등록(ThinkOnProject 강화)
  - 3 파일 변경
- `e1971f5` - task-393 등록(BeingActivity 로컬라이제이션 및 표시 수정)
  - 1 파일 변경
- `e710fa4` - changes commitHash 및 state 세션 종료 업데이트
  - 2 파일 변경
- `4cacc4a` - 2026-05-28의 sessions 및 changes 아카이브
  - 4 파일 변경


### 2026-05-28

#### 새로운 기능
- `ae8b673` - 플러그인 디렉토리 설정을 단일 경로에서 다중 디렉토리 목록으로 업그레이드 (ref task-391)
  - 29 파일 변경
- `aac46c1` - PluginLoader에 CS 소스 모드 추가, DLL 없을 때 플러그인 컴파일 로드 (ref task-389)
  - 6 파일 변경

#### 수정
- `63047b0` - 모든 PluginLoader를 ServiceLocator에 등록, 다중 디렉토리 플러그인 리플렉션 불완전 수정 (ref task-391)
  - 3 파일 변경
- `fcad655` - directoryList 브라우즈 버튼 상호작용 문제 수정 (ref task-392)
  - 9 파일 변경

#### 문서
- `e6d3037` - PluginDemo-22 CS 소스 컴파일 로드 모드 예제 (ref task-390)
  - 21 파일 변경

#### 협업 프레임워크
- `09d9e9c` - 30개 완료된 태스크 아카이브(task-362~task-391)
  - 2 파일 변경
- `66204a1` - 2026-05-28의 sessions(8) 및 changes(8) 아카이브
  - 18 파일 변경
- `308a8d0` - task-391 relatedCommit 업데이트
  - 1 파일 변경
- `6fc4e05` - task-389(CS 소스 모드) 및 task-390(PluginDemo-22) 등록
  - 1 파일 변경


### 2026-05-27

#### 새로운 기능
- `e154a18` - PluginDemo-21 WorkflowTemplate 완전한 비즈니스 워크플로우 예제 완성 (ref task-388)
  - 19 파일 변경
- `aa771b3` - PluginCapability 선언적 권한 시스템 구현 (ref task-379)
  - 9 파일 변경
- `5e5e9d1` - 04-SafeSystemIO System.IO 화이트리스트 안전 타입 예제 추가 (ref task-370)
  - 20 파일 변경

#### 문서
- `48f6702` - 19-TickObject와 20-SpeedyPack의 모든 언어 README 번역을 기준선에 맞춤 (ref task-386, task-387)
  - 119 파일 변경
- `5d570e5` - task-378 금지된 문자열 리플렉션 우회 반례 완성 (ref task-378)
  - 19 파일 변경
- `348c410` - PluginDemo-11 금지된 P/Invoke 및 unsafe 코드 반례 (ref task-377)
  - 19 파일 변경
- `fc92a49` - PluginDemo-10 금지된 리플렉션 작업 반례 (ref task-376)
  - 19 파일 변경
- `826ad2a` - PluginDemo-09 금지된 프로세스 작업 반례 플러그인 생성 (ref task-375)
  - 19 파일 변경
- `7870b05` - PluginDemo-08 금지된 네트워크 작업 반례 추가 (ref task-374)
  - 15 파일 변경
- `8636e31` - PluginDemo-07 금지된 파일 I/O 작업 반례 (ref task-373)
  - 19 파일 변경
- `322312e` - PluginDemo-06 TrustedAssemblies 신뢰된 종속성 예제 추가 (ref task-372)
  - 19 파일 변경
- `6df98a0` - IWorkflowPlugin 워크플로우 플러그인 예제 추가 (ref task-371)
  - 20 파일 변경
- `f3787ba` - PluginDemo-03 IObjectFactory 등록 및 생성 예제 (ref task-369)
  - 20 파일 변경
- `bb4324d` - PluginDemo-02 ITypeRegistry 등록 및 조회 예제 (ref task-368)
  - 20 파일 변경
- `bbdfa3c` - PluginDemo-01 최소 IPlugin 구현 예제 (ref task-367)
  - 19 파일 변경

#### 협업 프레임워크
- `de44057` - 5월 25일과 27일의 sessions 및 changes 아카이브
  - 58 파일 변경
- `9e4a84c` - tasks.json의 lastCommitHash를 48f6702로 업데이트
  - 1 파일 변경
- `beb58b2` - taskIndex 인덱스 보충(8 pending, 19 completed)
  - 1 파일 변경
- `63f7bfc` - task-388 relatedCommit 업데이트 (ref task-388)
  - 1 파일 변경
- `e61be6f` - task-378 relatedCommit 업데이트 (ref task-378)
  - 1 파일 변경
- `dde579b` - WorkflowTemplate 완전 사용 예제 태스크 게시(task-388)
  - 1 파일 변경
- `2294fa7` - TickObject 및 SpeedyPack 예제 태스크 게시(task-386~387)
  - 1 파일 변경
- `82b9f63` - 6개 PluginCapability 예제 태스크 게시(task-380~385)
  - 1 파일 변경
- `588539b` - PluginCapability 선언적 권한 시스템 태스크 게시(task-379)
  - 1 파일 변경
- `37f9c23` - 솔루션 및 프로젝트 파일 참조 업데이트
  - 8 파일 변경
- `e1f7892` - 12개 PluginDemo 대기 태스크 게시(task-367~378)
  - 3 파일 변경
- `87ae858` - PluginDemo 플러그인 정반례 태스크 등록 생성(task-367)
  - 2 파일 변경
- `f77a102` - 2026-05-26의 sessions 및 changes 아카이브
  - 7 파일 변경

## [Alpha-0.2] - 2026-05-16

### 2026-05-16

#### 릴리스 준비
- `476d839` - alpha-0.2 릴리스 태스크 추가
  - task-114(CHANGELOG 작성) 및 task-115(버전 번호 업데이트) 생성
  - 1개 파일 변경

### 2026-05-15

#### 인프라
- `672627b` - Gitee 동기화 워크플로우 추가(권한 설정 포함)
  - sync-from-gitee.yml 워크플로우 권한 설정 업데이트
  - 1개 파일 변경, 7줄 추가, 4줄 삭제

- `3cd5256` - GitHub Actions 자동 Gitee 코드 동기화 추가
  - sync-from-gitee.yml 워크플로우 신규 추가
  - 1개 파일 변경, 50줄 추가

#### 문서 업데이트
- `aa1d2ad` - 전체 11개 언어 README/아키텍처/시작 가이드 업데이트, SiliconLife.Fast 멀티 플랫폼 지원 반영 (ref task-112, task-113)
  - 문서에서 SiliconLife.Fast이 Windows 전용이라는 설명을 수정하여 실제 멀티 플랫폼 지원(Windows / macOS / Linux) 반영
  - 11개 언어의 README.md, architecture.md, getting-started.md 업데이트
  - SelectComponent에 hint 속성 지원 추가
  - ConfigView 열거형 드롭다운에 hint 전달
  - 11개 언어 로컬라이제이션에 SelectSearchHint 키값 신규 추가
  - 53개 파일 변경, 690줄 추가, 194줄 삭제

#### 태스크 시스템
- `3329f3d` - 태스크 시스템 점검 메커니즘 추가 + 로컬라이제이션 버그 수정 태스크
  - task-113 생성: 정보 페이지 로컬라이제이션 문제 수정
  - task-112 업데이트: Fast 버전 문서 Linux 지원 업데이트
  - 완료된 태스크(11개)를 .ai-collab/archive/에 아카이브
  - 점검 메커니즘 구성 완료: 빠른 점검(30분마다) + 전체 점검(매일 06:00)
  - 2개 파일 변경, 148줄 추가, 171줄 삭제

#### 협업 프레임워크
- `6038e22` - coze-agent를 .ai-collab 협업 등록부에 등록
  - 커즈 플랫폼 상주 AI 등록 정보 신규 추가
  - 1개 파일 변경

### 2026-05-14

#### AI 협업 프레임워크
- `7344fbb` - handoff 모드 제거, 태스크 목록 기반으로 변경 (v2.0)
  - .ai-collab 디렉토리 구조 리팩토링, handoff 인계 모드에서 태스크 목록 기반으로 변경
  - tasks.json 태스크 목록 핵심 파일 신규 추가
  - activity.log 조작 로그 신규 추가
  - changes/ 및 sessions/ 디렉토리 신규 추가

- `589a48e` - .ai-collab 세션 기록 추가
  - AI 협업 세션 상태 기록 신규 추가

- `5481bcf` - Qoder AI IDE를 협업 등록부에 등록
  - Qoder AI 프로그래밍 어시스턴트 등록 정보 신규 추가

- `e2d7b61` - tasks.json relatedCommit 및 changes commitHash 보충
  - 태스크 메타데이터 연결 보완

- `a087f0c` - task-101~110 전체 태스크 검수
  - 10개 태스크 수정 전체 완료 확인

#### 버그 수정
- `fac9435` - task-101~110 전체 10개 태스크 수정 및 구현 완료
  - 검색 선택 컴포넌트 힌트 텍스트 누락 수정
  - 정보 페이지 로컬라이제이션 문제 수정
  - 도움말 시스템 검색 JS 오류 수정
  - 39개 파일 변경, 684줄 추가, 121줄 삭제

- `c46dfbc` - 모든 대기 태스크 완료 (task-001~006)
  - 초기 6개 대기 태스크 완료

- `ec176b2` - 태스크 목록 덮어쓰기 - 코드 리뷰에서 10개의 새 버그 발견
  - task-101~110 총 10개의 새 태스크 생성

#### 리팩토링
- `ab15915` - 저작권 헤더 통일 + HelpController BOM 및 HelpView 검색 JS 수정
  - 모든 C# 소스 파일 Apache 2.0 저작권 헤더 통일
  - HelpController BOM 인코딩 문제 수정
  - HelpView 검색 JavaScript 오류 수정

#### 새로운 기능
- `18a6f5d` - MCP 브라우저 기능 서버 생성 (ref task-111)
  - SiliconLife.McpServer 프로젝트 신규 추가
  - Playwright 브라우저 자동화 MCP 서버 구현

- `9eb251a` - SiliconLife.McpServer 모듈 제거 (ref task-111)
  - 독립 MCP 서버 제거, 기능이 메인 프로젝트에 통합됨

### 2026-05-13

#### 로컬라이제이션
- `7a62590` - 폴란드어 로컬라이제이션 지원 추가
  - pl-PL 폴란드어 로컬라이제이션 구현 신규 추가(PlPL.cs, 1089줄)
  - 폴란드어 도움말 문서 로컬라이제이션 신규 추가(HelpLocalizationPlPL.cs, 3972줄)
  - 폴란드어 중국 역사 캘린더 지원 신규 추가(ChineseHistoricalPlPL.cs, 600줄)
  - 폴란드어 트레이 로컬라이제이션 신규 추가(TrayPlPL.cs, 135줄)
  - 폴란드어 전체 문서 세트 신규 추가(15개 문서)
  - Language 열거형에 폴란드어 신규 추가
  - 35개 파일 변경, 14379줄 추가, 11줄 삭제

- `51f9c8e` - 문서의 Ark AI 참조 및 용어 개선 업데이트
  - 다국어 문서의 AI 클라이언트 용어 업데이트

- `7587c12` - 모든 언어에 변경 로그 항목 추가
  - 모든 언어 버전의 changelog 동기화 업데이트

#### 윈도우 시스템 마이그레이션
- `b49a07d` - Avalonia 윈도우 상주 모드로 마이그레이션
  - Windows Forms 의존성 제거, Avalonia UI 프레임워크로 완전 마이그레이션
  - Linux에서 상태 창 정상 표시(원격 데스크톱 검증)
  - 창 제어 추가: 우클릭 메뉴, 더블클릭으로 Web 열기, 닫기 버튼
  - 다중 AI 협업 프레임워크 추가(.ai-collab/)
  - 트레이 아이콘 초기화 수정(우아한 저하)
  - App.axaml 및 App.cs Avalonia 애플리케이션 진입점 신규 추가
  - 13개 파일 변경, 1442줄 추가, 541줄 삭제

- `d335aaf` - Linux 플랫폼 창 항상 표시 + 닫기 확인 대화상자
  - Linux에서 상태 창 자동 표시(트레이 아이콘 없음)
  - Linux에서 창 닫기 시 확인 대화상자 표시
  - Windows/macOS는 기존 트레이 동작 유지
  - --no-tray 매개변수로 트레이 강제 비활성화 지원
  - 확인 대화상자용 ShowMessageBoxAsync 메서드 신규 추가
  - 3개 파일 변경, 206줄 추가, 0줄 삭제

#### 트레이 시스템 리팩토링
- `841d384` - 트레이 시스템 리팩토링 및 AI 협업 프레임워크 초기화
  - TrayLocalizationBase에서 미사용 속성 제거
  - ShowStatus 로컬라이제이션 항목 추가
  - App.cs에 트레이 아이콘 클릭 시 상태 창 표시, 로컬라이제이션 메뉴 항목 추가
  - Program.cs에서 트레이 아이콘 초기화를 StartAsync로 이동
  - TrayStatusWindow 닫기 시 종료 대신 숨김 처리
  - trae-glm5 및 catpaw를 .ai-collab 협업 프레임워크에 등록
  - .gitignore 업데이트하여 .ai-collab의 모든 파일이 추적되도록 보장
  - 22개 파일 변경, 178줄 추가, 1226줄 삭제

#### 문서
- `43653bc` - 저장소 설명 및 AI 등록부 업데이트
  - 프로젝트 README 및 .ai-collab 등록 정보 업데이트

### 2026-05-12

#### 태스크 시스템 Web 뷰
- `0891b3c` - 태스크 실행 상세 및 기록 뷰 추가
  - TaskExecutionDetailView 태스크 실행 상세 뷰 신규 추가
  - TaskExecutionHistoryView 태스크 실행 기록 뷰 신규 추가
  - TaskController에 실행 상세 및 기록 조회 인터페이스 신규 추가
  - TaskViewModel 태스크 뷰 모델 신규 추가
  - TaskCenter 태스크 센터 강화
  - TaskSystem 태스크 시스템 업데이트
  - 9개 언어 로컬라이제이션에 태스크 관련 키값 신규 추가
  - 26개 파일 변경, 803줄 추가, 55줄 삭제

### 2026-05-11

#### Web 컴포넌트 아키텍처 리팩토링
- `5e687ad` - 컴포넌트 렌더링을 문자열에서 H-tree로 마이그레이션
  - ComponentBase 렌더링 메서드를 문자열 모드에서 H-tree 구조로 마이그레이션
  - 전체 28개 컴포넌트 새 렌더링 아키텍처에 적응(A, Accordion, Button, Calendar, Card, Chart 등)
  - SelectComponent 대폭 리팩토링(889줄 개선)
  - 컨트롤러 및 뷰 동기화 업데이트
  - 33개 파일 변경, 667줄 추가, 435줄 삭제

- `bfd332d` - Style을 문자열에서 CssBuilder 인라인 스타일로 마이그레이션
  - CssBuilder 스타일 빌더 신규 추가
  - ComponentBase 스타일 시스템을 문자열에서 구조화된 CssBuilder로 마이그레이션
  - LoadingComponent 대폭 강화(103줄 추가)
  - ConfigController, LogController, MemoryController 컨트롤러 스타일 마이그레이션
  - ChatView, ConfigView, LogView, MemoryView 뷰 스타일 마이그레이션
  - 37개 파일 변경, 351줄 추가, 157줄 삭제

#### 저장소 시스템 최적화
- `d67a7ee` - QueryLatest 대규모 데이터 세트 쿼리 최적화
  - SpeedyTimeStorage QueryLatest 메서드 성능 최적화
  - SpeedyLoggerProvider 로거 프로바이더 강화
  - 2개 파일 변경, 44줄 추가, 5줄 삭제

#### 캘린더 시스템 리팩토링
- `9629f88` - TimerExecution 추출 및 타이머 Web 뷰 강화
  - TimerSystem에서 TimerExecution 로직 추출(175줄 제거)
  - SelectComponent 대폭 강화(427줄 개선)
  - TimerController 및 타이머 뷰 강화
  - ContextManager 컨텍스트 매니저 업데이트
  - 12개 파일 변경, 458줄 추가, 267줄 삭제

#### 로컬라이제이션
- `5d8ca79` - LogsLoading 로컬라이제이션 키값 추가
  - 9개 언어에 LogsLoading 키값 신규 추가
  - DefaultLocalizationBase 기본 클래스에 정의 신규 추가
  - 11개 파일 변경, 15줄 추가

### 2026-05-10

#### 태스크 시스템 리팩토링
- `54394f6` - 태스크 시스템과 채팅 기록 주기 병합
  - ProjectTaskSystem 프로젝트 태스크 시스템 대폭 간소화(411줄 리팩토링)
  - TaskSystem 태스크 시스템 간소화(254줄 리팩토링)
  - TaskCenter 태스크 센터 리팩토링(188줄 개선)
  - ContextManager 컨텍스트 매니저 최적화(347줄 리팩토링)
  - DefaultSiliconBeing 실리콘 비잉 강화
  - TimerSystem 타이머 시스템에 태스크 통합
  - IWorkNoteStorage 인터페이스 업데이트
  - SpeedyWorkNoteStorage 및 FileSystemWorkNoteStorage 적응
  - 16개 파일 변경, 648줄 추가, 897줄 삭제

### 2026-05-09

#### Web 인터페이스 강화
- `bc50dd7` - 채팅 뷰 개선 및 감사 기능 추가
  - AuditController 감사 컨트롤러 신규 추가(261줄)
  - AuditView 감사 뷰 신규 추가(379줄)
  - AuditViewModel 감사 뷰 모델 신규 추가
  - ChatView 채팅 뷰 대폭 개선(171줄 강화)
  - ChatController 채팅 컨트롤러 업데이트
  - MarkdownEditorComponent 컴포넌트 강화
  - InitController 초기화 컨트롤러 개선
  - ChatSystem 채팅 시스템에 기능 신규 추가
  - 14개 파일 변경, 1030줄 추가, 112줄 삭제

- `c9babce` - 채팅 뷰의 툴 콜 렌더링 개선
  - ChatView 툴 콜 블록 렌더링 강화
  - 1개 파일 변경, 54줄 추가, 11줄 삭제

#### AI 툴 시나리오 시스템
- `ff2eddd` - 툴 시나리오 필터링 시스템 구현
  - ToolScenarioAttribute 툴 시나리오 속성 신규 추가(36줄)
  - ChatOnlyAttribute 채팅 전용 시나리오 속성 신규 추가(19줄)
  - ToolManager 툴 매니저에 시나리오 필터링 기능 신규 추가(40줄)
  - ContextManager 컨텍스트 매니저 시나리오 필터링 적응
  - 4개 파일 변경, 115줄 추가, 30줄 삭제

- `5709a33` - 툴 클래스에 시나리오 속성 추가
  - 24개 툴 클래스에 ToolScenario 속성 주석 추가
  - 캘린더, 채팅, 설정, 큐레이션, 데이터베이스, 디스크, 동적 컴파일 등 툴 포함
  - 24개 파일 변경, 46줄 추가, 20줄 삭제

#### 태스크 시스템 리팩토링
- `2f19a5f` - TaskCenter 및 TaskEnumerator를 사용하여 태스크 시스템 리팩토링
  - TaskCenter 태스크 센터 신규 추가(235줄)
  - TaskEnumerator 태스크 열거자 신규 추가(297줄)
  - TaskSystem 태스크 시스템 리팩토링 간소화
  - DefaultSiliconBeing 실리콘 비잉 새 아키텍처 적응
  - DefaultSiliconBeingFactory 팩토리 업데이트
  - SiliconBeingBase 기본 클래스 강화
  - 7개 파일 변경, 796줄 추가, 275줄 삭제

#### 권한 시스템 마이그레이션
- `a06ed09` - IM 및 권한 시스템을 App 프로젝트로 마이그레이션
  - PermissionRequestQueue를 Default/Fast에서 App 프로젝트로 마이그레이션(443줄 추가)
  - Default 버전 WebUIProvider 제거(403줄 삭제)
  - Default 버전 HelpTool 제거(194줄 삭제)
  - Default/Fast 버전의 중복 PermissionRequestQueue 제거
  - Default 버전 IMPermissionAskHandler 제거
  - PermissionRequestController 컨트롤러 업데이트
  - 14개 파일 변경, 496줄 추가, 1183줄 삭제

#### AI 컨텍스트 최적화
- `4c8aaff` - 컨텍스트 매니저 최적화 및 서비스 로케이터 강화
  - ContextManager 컨텍스트 매니저 간소화 최적화
  - ServiceLocator 서비스 로케이터 강화(36줄 추가)
  - ToolManager 툴 매니저 강화(34줄 추가)
  - DashScopeClient 및 VolcengineArkClient 클라이언트 개선
  - 이그제큐터(CommandLine, Disk, Network) 업데이트
  - 8개 파일 변경, 116줄 추가, 98줄 삭제

#### 로컬라이제이션
- `5c5eef7` - 감사 및 태스크 로컬라이제이션 키값 추가
  - DefaultLocalizationBase에 127줄 로컬라이제이션 정의 신규 추가
  - 9개 언어에 감사 및 태스크 관련 키값 신규 추가(각 26줄)
  - 11개 파일 변경, 387줄 추가

#### 프로젝트 설정
- `2067db6` - 프로젝트 설정 및 gitignore 규칙 업데이트
  - .gitignore 규칙 업데이트
  - DefaultConfigData 및 Fast DefaultConfigData 설정 강화
  - SpeedyWorkNoteStorage 저장소 개선
  - SpeedyPack 코어 강화
  - 5개 파일 변경, 32줄 추가, 6줄 삭제

### 2026-05-07

#### 이탈리아어 로컬라이제이션
- `8adc18c` - 이탈리아어 로컬라이제이션 지원 추가 및 다국어 문서 업데이트
  - it-IT 이탈리아어 로컬라이제이션 신규 추가
  - ItIT 로컬라이제이션 구현 신규 추가(1909줄)
  - ChineseHistoricalItIT 중국 역사 캘린더 이탈리아어 지원 신규 추가(586줄)
  - TrayItIT 트레이 이탈리아어 로컬라이제이션 신규 추가(135줄)
  - 이탈리아어 전체 문서 세트 신규 추가(14개 문서: README, API 참조, 아키텍처, 캘린더 시스템, 변경 로그, 기여 가이드 등)
  - 모든 언어 버전의 아키텍처, 개발 가이드, 시작 가이드 등 문서 업데이트
  - Language 열거형에 이탈리아어 신규 추가
  - 86개 파일 변경, 11573줄 추가, 769줄 삭제

#### 문서 동기화
- `12a5deb` - 아키텍처, 변경 로그 및 실리콘 비잉 가이드의 다국어 문서 업데이트
  - 8개 언어의 README 업데이트
  - 8개 언어의 아키텍처 문서 업데이트
  - 8개 언어의 변경 로그 업데이트
  - 8개 언어의 실리콘 비잉 가이드 업데이트
  - 8개 언어의 툴 참조 업데이트
  - 용어집 리팩토링
  - 46개 파일 변경, 1697줄 추가, 442줄 삭제

### 2026-05-06

#### 대규모 모듈 리팩토링
- `eeb3be6` - 대규모 모듈 리팩토링 및 재구성
  - SiliconLife.App 프로젝트 구조 조정
  - SiliconLife.Fast 프로젝트 재구성
  - SiliconLife.Default 프로젝트 재구성
  - SiliconLife.Common 공유 모듈 재구성
  - SiliconLife.Core 코어 모듈 재구성
  - SiliconLife.Speedy 스토리지 엔진 재구성
  - SiliconLife.Speedy.Manager 관리 도구 재구성
  - 119개 파일 변경, 6926줄 추가, 3066줄 삭제

### 2026-05-04

#### AI 클라이언트
- `24d2c86` - VolcengineArkClient 추가 및 Audit을 Usage tracking으로 교체
  - VolcengineArkClient 화산엔진 Ark AI 클라이언트 신규 추가
  - 스트리밍 및 비스트리밍 모드 지원
  - 내장 이중 계층 속도 제어(자기 속도 제어 + 서버 속도 제한)
  - OpenAI API 프로토콜 호환
  - Audit 시스템을 Usage tracking으로 교체
  - 24개 파일 변경, 802줄 추가, 21줄 삭제

#### 툴 시스템
- `f27650a` - Fast 자가 재시작을 위한 핫 리로드 툴 추가
  - HotReloadTool 핫 리로드 툴 신규 추가
  - SiliconLife.Fast 온라인 컴파일, 업데이트 및 재시작 지원
  - HotReload.exe 독립 업데이터 신규 추가
  - 안전한 파일 복사 메커니즘(자기 자신 덮어쓰지 않음)
  - 우아한 종료 및 포트 해제 대기
  - 9개 파일 변경, 581줄 추가

#### 로컬라이제이션
- `6a5aad8` - 모든 파일 업데이트 및 프랑스어 로컬라이제이션 지원 추가
  - fr-FR 프랑스어 로컬라이제이션 신규 추가
  - 모든 언어 버전 업데이트
  - 도움말 문서 프랑스어 번역
  - 인터페이스 프랑스어 번역
  - 100개 이상의 파일 변경

### 2026-05-03

#### 프로젝트 인프라
- `2664b0c` - 프로젝트 인프라 및 의존성 업데이트
  - SiliconLife.Speedy.Manager에 WPF 관리 인터페이스 신규 추가(MainForm.Designer.cs, MainForm.resx)
  - slc.ico 아이콘 리소스 신규 추가(1.5MB)
  - PluginLoader 보안 스캔 대폭 강화(622줄 추가)
  - PermissionedStreamFactory 권한 스트림 팩토리 신규 추가(779줄)
  - PermissionRequestQueue 권한 요청 큐 신규 추가(Default 및 Fast 버전)
  - DebugLoggerProvider 디버그 로거 프로바이더 신규 추가
  - ConfigDataBase 설정 기본 클래스 강화
  - ToolManager에 플러그인 툴 스캔 기능 신규 추가(ScanAllPluginAssemblies)
  - SiliconBeingManager 라이프사이클 관리 강화
  - DashScopeClient 알리클라우드 AI 클라이언트 대폭 강화(227줄 추가)
  - DefaultSiliconBeingFactory 팩토리 강화
  - Web 뷰 및 컨트롤러 업데이트(ChatView, WorkNoteView, PermissionRequestController)
  - 9개 언어 로컬라이제이션에 키값 신규 추가
  - 35개 파일 변경, 28080줄 추가, 336줄 삭제

### 2026-05-02

#### AI 클라이언트 강화
- `c16f99f` - AI 클라이언트, Web UI 및 스토리지 컴포넌트 업데이트
  - DashScopeClient 알리클라우드 클라이언트 대폭 개선
  - SpeedyPackAutoCompactor 자동 컴팩터 최적화
  - Web 뷰 기본 클래스 및 BeingView 개선
  - 6개 파일 변경, 240줄 추가, 81줄 삭제

#### 플러그인 시스템
- `242dc98` - 정보 페이지에 플러그인 목록 추가
  - AboutController에 플러그인 정보 표시 신규 추가
  - AboutViewModel에 플러그인 데이터 모델 신규 추가
  - AboutView에 플러그인 목록 렌더링 신규 추가
  - 9개 언어 로컬라이제이션에 플러그인 관련 키값 신규 추가
  - 14개 파일 변경, 160줄 추가, 1줄 삭제

#### AI 최적화
- `147f8f4` - 컨텍스트 메모리 프롬프트 텍스트 간소화
  - ContextManager AI 프롬프트 최적화
  - 1개 파일 변경, 1줄 추가, 1줄 삭제

#### Speedy 저장소 최적화
- `8bda2d3` - Speedy 저장소 및 메모리 컨트롤러 구현 업데이트
  - SpeedyPackAutoCompactor 간격 수정
  - SpeedyTimeStorage 경로 처리 최적화
  - MemoryController 메모리 컨트롤러 개선
  - SpeedyPack.Manager UI 업데이트
  - 4개 파일 변경, 21줄 추가, 18줄 삭제

#### 트레이 강화
- `8972654` - 트레이 상태 창의 로컬라이제이션 지원 강화
  - 9개 언어 트레이 로컬라이제이션에 Speedy 관리 진입점 신규 추가
  - TrayStatusWindow에 Speedy 관리 메뉴 항목 신규 추가
  - 11개 파일 변경, 72줄 추가

#### Speedy.Manager 최적화
- `6f5db09` - SpeedyPack 관리자 UI 및 내부 컴포넌트 최적화
  - MainForm 인터페이스 리팩토링
  - FreeList 메모리 관리 최적화
  - WriteQueue 쓰기 큐 개선
  - SpeedyPack 코어 최적화
  - 5개 파일 변경, 96줄 추가, 88줄 삭제

#### 저장소 시스템 강화
- `57f9d5d` - 저장소 시스템 개선, 자동 압축 및 불완전 날짜 지원 추가
  - SpeedyPackAutoCompactor 자동 압축 타이머 신규 추가(30분 간격)
  - SpeedyPackRegistry 싱글톤 매니저 강화
  - SpeedyStorage, SpeedyTimeStorage, SpeedyWorkNoteStorage 적응 개선
  - SpeedyPack에 FreeList 여유 공간 관리 신규 추가(149줄)
  - PackFileWriter 라이터 리팩토링 최적화
  - WriteOperation, WriteQueue 쓰기 큐 강화
  - SpeedyPackOptions 설정 옵션 확장
  - IncompleteDate에 비교 메서드 신규 추가
  - PluginLoader 플러그인 로더 개선
  - Default 및 Fast 버전 Program.cs 초기화 흐름 업데이트
  - DefaultConfigData 설정 데이터 간소화
  - KnowledgeNetwork 노리지 네트워크 간소화
  - ChatController, MemoryController 컨트롤러 최적화
  - SpeedyPack.Manager MainForm 기능 강화
  - 22개 파일 변경, 639줄 추가, 253줄 삭제

#### Speedy.Manager 업데이트
- `b04ed33` - Speedy.Manager 파일 업데이트

### 2026-05-01

#### 아키텍처 리팩토링: Speedy 저장소로 LiteDB 교체
- `6600972` - LiteDB를 Speedy 저장소로 교체, 플러그인 시스템 및 Speedy 프로젝트 추가
  - **SiliconLife.Speedy 프로젝트 신규 추가**: 고성능 .spk 스토리지 엔진
    - SpeedyPack 코어 클래스(489줄): 메모리 디렉토리 매핑 + 엔트리 캐시 + 비동기 쓰기 큐
    - SpeedyPackOptions 설정 클래스: 캐시 TTL, 최대 캐시 엔트리 수, 읽기 전용 모드
    - IPackTransaction 트랜잭션 인터페이스: 원자 쓰기 조작 지원
    - SpkFileInfo 파일 정보 클래스
    - Internal 디렉토리: DirectoryMap, EntryCache, PackFileReader, PackFileWriter, WriteQueue, WriteOperation, SpeedyTransaction, SpkHeader, PathNormalizer, FreeList
    - MessagePack 3.1.4를 사용한 바이너리 직렬화(LZ4 압축) 의존
  - **SiliconLife.Speedy.Manager 프로젝트 신규 추가**: WPF 관리 도구
    - MVVM 아키텍처: MainViewModel, DirectoryTreeViewModel, ContentViewerViewModel 등
    - 서비스 레이어: PackService, FileDialogService, RecentFilesService, NotificationService
    - 컨버터: BoolToVisibility, ByteSizeToString, ContentTypeToIcon, NullToCollapsed
    - 뷰: MainWindow, DirectoryTreeView, ContentViewerPanel, MetadataPanel
    - 대화상자: FileInfoDialog, ImportDialog, NewEntryDialog
  - **SiliconLife.Fast 저장소 마이그레이션**: LiteDB → SpeedyPack
    - SpeedyStorage 신규 추가(IStorage 어댑터)
    - SpeedyTimeStorage 신규 추가(ITimeStorage 어댑터)
    - SpeedyWorkNoteStorage 신규 추가(IWorkNoteStorage 어댑터)
    - SpeedyPackRegistry 신규 추가(프로세스 수준 싱글톤 관리)
    - SpeedyPackAutoCompactor 신규 추가(자동 압축 타이머)
    - LiteDB 관련 저장소 구현 제거(LiteDBStorage, LiteDBTimeStorage, LiteDBWorkNoteStorage, LiteDBLoggerProvider, LiteDBManager, LiteDBModels)
    - LiteDB 관리 창 관련 코드 제거
  - **플러그인 시스템**:
    - IPlugin 인터페이스 신규 추가(Core/Plugins/IPlugin.cs)
    - PluginLoader 플러그인 로더 신규 추가(Core/Plugins/PluginLoader.cs)
    - 디렉토리에서 플러그인 DLL 로드 지원
    - 보안 스캔: 네임스페이스 검사 금지(System.IO, System.Net, Microsoft.CodeAnalysis 등)
    - 신뢰할 수 있는 어셈블리 화이트리스트(Google.Protobuf, Newtonsoft.Json, MessagePack 등)
    - 사용자 정의 AssemblyLoadContext 격리 로드
    - ToolManager에 ScanAllPluginAssemblies 메서드 신규 추가
    - CoreHost에 플러그인 로더 통합
  - 119개 파일 변경, 6926줄 추가, 3066줄 삭제

#### 실리콘 비잉 강화
- `3aef4c3` - Stopped 활동 상태 및 오류 처리 개선 추가
  - 실리콘 비잉에 Stopped 상태 신규 추가
  - 오류 처리 및 복구 메커니즘 강화

#### 로컬라이제이션 업데이트
- `513c65d` - 모든 언어 버전 및 문서 업데이트
  - MarkdownEditorComponent 컴포넌트 신규 추가(625줄)
  - DetailsComponent 컴포넌트 신규 추가(130줄)
  - AccordionComponent 아코디언 컴포넌트 신규 추가(285줄)
  - BeingController, ChatController, MemoryController, PermissionController 컨트롤러 업데이트
  - BeingView, ChatView, MemoryView, SoulEditorView 뷰 리팩토링
  - 구 MarkdownEditorView 제거
  - InitController 컴포넌트화 마이그레이션
  - 115개 파일 변경, 5761줄 추가, 2362줄 삭제

### 2026-04-30

#### 시스템 트레이 기능
- `101b203` - 트레이 상태 창 및 ApplicationContext 구현
  - 트레이 아이콘 리소스 신규 추가(alpha.png, noWord.png, slc.ico, wordIcon.png)
  - TrayStatusWindow 상태 창 구현
  - 9개 언어의 트레이 로컬라이제이션 지원(TrayCsCZ, TrayDeDE, TrayEnUS 등)
  - TrayLocalizationBase 추상 기본 클래스
  - 24개 파일 변경, 27995줄 추가, 1줄 삭제(리소스 파일 포함)

#### 컴포넌트화 UI 아키텍처
- `e61cfaa` - 컴포넌트화 UI 아키텍처 완료, 24개 컴포넌트 구현
  - MVP 단계(8개): ComponentBase, Div, Span, Button, Input, Form, Select, Label
  - 2단계(6개): Accordion, Card, Tabs, Table, Modal, Message
  - 3단계(5개): Calendar, Tree, Chart, FileUpload, RichText
  - Js, Behavior, DomUpdate 등 보조 클래스 신규 추가
  - 25개 파일 변경, 2666줄 추가

- `7449e51` - 컴포넌트 시스템 개선 및 새 스킨 테마 추가
  - A, Button, Div, Form, Input 등 컴포넌트 강화
  - 3가지 스킨 테마 신규 추가: HighContrast(고대비), Light(밝은), Minimal(미니멀)
  - 기존 스킨 업데이트(Admin, Chat, Creative, Dev)
  - InitController 컴포넌트화 마이그레이션
  - 32개 파일 변경, 1466줄 추가, 1238줄 삭제

- `1ba8636` - InitController 컴포넌트화 마이그레이션 시작(진행 중)
  - 9개 파일 변경, 574줄 추가, 145줄 삭제

#### 저장소 시스템 통일
- `895dff9` - soul.md 및 state.json이 IStorage 인터페이스를 사용하도록 통일
  - DefaultSiliconBeing이 IStorage로 소울 파일 및 상태 읽기/쓰기
  - StateFileManager 상태 파일 매니저 신규 추가
  - SoulFileManager 리팩토링하여 IStorage에 적응
  - 8개 파일 변경, 201줄 추가, 116줄 삭제

#### LiteDB 관리 강화
- `a34bef4` - LiteDBManager 추가 및 트레이 로컬라이제이션 강화
  - 트레이 메뉴에 LiteDB 관리 진입점 신규 추가
  - 9개 언어 트레이 로컬라이제이션 업데이트
  - 10개 파일 변경, 196줄 추가

- `c4a79ca` - LiteDB 관리 창에 언어 인식 로컬라이제이션 팩토리 추가
  - 1개 파일 변경, 78줄 추가

- `5ebc55e` - LiteDBAdminLocalization을 추상 기본 클래스로 변환
  - 10개 파일 변경, 1356줄 추가

#### 설정 시스템 수정
- `2da5256` - ConfigExists 추상 메서드 추가 및 LiteDB 중복 설정 레코드 수정
  - ConfigDataBase에 ConfigExists 메서드 신규 추가
  - Fast 버전 DefaultConfigData에 LiteDB 설정 존재 여부 검사 구현
  - LiteDB 중복 설정 키 문제 수정
  - 9개 파일 변경, 210줄 추가, 2줄 삭제

#### 채팅 및 뷰 최적화
- `d3618ec` - 채팅 세션, 저장소 시스템, 시간 모델 및 뷰 기본 클래스 최적화
  - BroadcastChannel, GroupChatSession, SingleChatSession 최적화
  - ITimeStorage에 쿼리 메서드 신규 추가
  - FileSystemStorage 및 LiteDBStorage 동기화 업데이트
  - ViewBase 리팩토링 최적화(Default 및 Fast 버전)
  - 11개 파일 변경, 622줄 추가, 392줄 삭제

### 2026-04-29

#### 아키텍처 리팩토링: 공유 모듈 추출
- `a102428` - 공유 모듈을 SiliconLife.Default에서 SiliconLife.Common으로 마이그레이션
  - 32종 캘린더 구현을 Common 프로젝트로 추출
  - 로컬라이제이션 기본 클래스 및 21종 언어 구현을 Common 프로젝트로 추출
  - 권한 매니저, 기본 실리콘 비잉 구현을 Common 프로젝트로 추출
  - 23개 내장 툴 구현을 Common 프로젝트로 추출
  - Playwright WebView 구현을 Common 프로젝트로 추출
  - 네임스페이스를 SiliconLife.Collective로 업데이트
  - 122개 파일 변경, 586줄 추가, 343줄 삭제

#### 코드 품질 개선
- `17566fe` - Core, Common 및 Default 프로젝트의 Console.WriteLine을 로깅 시스템으로 교체
  - ContextManager, AuditLogger, DefaultConfigData 등 6개 파일 업데이트
  - ILogger 인터페이스 통일 사용, 코드 유지보수성 향상
  - 6개 파일 변경, 12줄 추가, 8줄 삭제

#### SiliconLife.Fast 고성능 버전
- `54a0307` - SiliconLife.Fast 프로젝트 추가 및 컴파일 수정 완료
  - 완전한 Windows Forms 애플리케이션 진입점
  - 시스템 트레이 지원(NotifyIcon)
  - 전체 Web UI 컨트롤러 이식(20개 이상)
  - 전체 Web 뷰 컴포넌트 이식
  - 4가지 스킨 테마 이식(Admin, Chat, Creative, Dev)
  - 125개 파일 변경, 61186줄 추가

#### 다국어 문서 동기화
- `265fde8` - 듀얼 버전 아키텍처 문서를 모든 언어로 동기화
  - 7개 언어의 architecture.md, changelog.md 업데이트
  - 6개 언어의 contributing.md 업데이트
  - 7개 언어의 getting-started.md, roadmap.md 업데이트
  - 47개 파일 변경, 1214줄 추가, 38줄 삭제

#### LiteDB 저장소 시스템(Fast 버전)
- `4704862` - LiteDB 의존성 및 인프라 추가
  - LiteDBManager 관리 클래스 신규 추가
  - LiteDBModels 데이터 모델 신규 추가
  - 3개 파일 변경, 252줄 추가

- `4220036` - LiteDB 저장소 클래스 구현
  - LiteDBStorage: IStorage 인터페이스 구현
  - LiteDBTimeStorage: ITimeStorage 인터페이스 구현
  - LiteDBWorkNoteStorage: IWorkNoteStorage 인터페이스 구현
  - 3개 파일 변경, 581줄 추가

- `38ebd23` - 설정 및 로깅 시스템을 LiteDB로 마이그레이션
  - DefaultConfigData LiteDB 저장소 적응
  - LiteDBLoggerProvider 로거 프로바이더 신규 추가
  - 2개 파일 변경, 203줄 추가, 67줄 삭제

- `e687157` - 노리지 네트워크를 파일 시스템에서 LiteDB로 마이그레이션
  - KnowledgeNetwork 전면 리팩토링, LiteDB로 삼중항 데이터 저장
  - 1개 파일 변경, 231줄 추가, 72줄 삭제

- `4220169` - LiteDB 저장소를 Program 및 ProjectManager에 통합
  - Program.cs에서 LiteDB 저장소 초기화
  - ProjectManager LiteDB 워크 노트 저장소 적응
  - 2개 파일 변경, 40줄 추가, 17줄 삭제

- `5f3a709` - 폐기된 파일 시스템 저장소 구현 제거
  - FileSystemLoggerProvider, FileSystemStorage, FileSystemTimeStorage 등 삭제
  - 6개 파일 변경, 1518줄 삭제

- `e1a4ef2` - docs: 모든 문서에 v0.1.0-alpha 버전 식별자 추가
  - 127개 파일 변경, 2297줄 추가, 2471줄 삭제

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### 저장소 시스템 리팩토링
- `8dd26e3` - ITimeStorage 인터페이스의 IncompleteDate 사용 통일 및 계층 쿼리 API 추가
  - ITimeStorage 인터페이스에서 DateTime 오버로드 메서드 제거, IncompleteDate 사용 통일
  - IncompleteDate에 CompareTo(DateTime) 비교 메서드 및 Expand() 전개 메서드 신규 추가
  - GetEarliestTimestamp(), GetLatestTimestamp() 계층 쿼리 API 신규 추가
  - HasSummary() 및 QueryWithLevel() 메서드 신규 추가, 시간 계층별 쿼리 지원
  - Memory.cs 압축 알고리즘 리팩토링, 새 계층 쿼리 API로 효율 향상
  - FileSystemTimeStorage.cs 새 인터페이스 메서드 전체 구현
  - 모든 호출처 동기화 업데이트: ChatSystem, ChatSession, BroadcastChannel, AuditLogger, TokenUsageRecord 등
  - 툴 시스템 업데이트: HelpTool, LogTool, TokenAuditTool 새 인터페이스 적응
  - Web 컨트롤러 업데이트: AuditController, ChatController, ChatHistoryController 새 인터페이스 적응
  - 41개 파일 변경, 1820줄 추가, 903줄 삭제

### 2026-04-27

#### 도움말 문서 시스템 강화
- `9989d79` - 로컬라이제이션, 도움말 시스템 및 Web 뷰 업데이트
  - IAIClientFactoryHelp.cs AI 클라이언트 팩토리 도움말 문서 인터페이스 신규 추가
  - 전체 도움말 문서의 9개 언어 번역 완료
  - HelpTopics.cs에 40개 도움말 주제 정의 신규 추가
  - Web 뷰 전면 업데이트: InitController, AuditView, ConfigView, KnowledgeView, LogView 등
  - 로컬라이제이션 시스템 강화: 모든 언어 버전에 새 로컬라이제이션 키 추가
  - AI 클라이언트 팩토리 업데이트: DashScopeClientFactory, OllamaClientFactory 개선
  - 30개 파일 변경, 10086줄 추가, 15줄 삭제

#### 도움말 문서 신규 내용
- `e7afe94` - 소울 파일 및 감사 로그 도움말 문서 신규 추가
  - 소울 파일 관리 도움말 문서 신규 추가
  - 감사 로그 도움말 문서 신규 추가
  - HelpTopics.cs에 주제 정의 신규 추가
  - HelpView.cs 대폭 리팩토링, 문서 렌더링 로직 개선
  - PermissionView.cs 리팩토링, 권한 관리 인터페이스 개선
  - 코어 모듈 강화: SiliconBeingManager, TaskSystem, ToolManager 개선
  - TaskTool.cs 리팩토링, 태스크 관리 기능 개선
  - Web 뷰 전면 업데이트: 모든 뷰 컴포넌트 동기화 업데이트
  - HelpController.cs 간소화, 컨트롤러 로직 최적화
  - 30개 파일 변경, 7100줄 추가, 897줄 삭제

### 2026-04-26

#### 도움말 문서 시스템
- `07895d7` - 도움말 문서 시스템 강화, 3개 문서 신규 추가 및 9개 언어 번역 완료
  - 메모리 시스템, Ollama 설치 설정, 알리클라우드 바이리안 플랫폼 사용 가이드 신규 추가
  - 전체 10개 도움말 문서의 9개 언어 번역 완료
  - HelpView 렌더링 로직 간소화
  - 18개 파일 변경, 14418줄 추가, 1364줄 삭제

#### 독일어 로컬라이제이션
- `0cfd8a1` - 완전한 독일어(de-DE) 로컬라이제이션 지원 추가
  - 완전한 독일어 로컬라이제이션 파일
  - 중국 역사 캘린더 독일어 지원 신규 추가
  - 도움말 문서 독일어 번역 신규 추가
  - 9개 언어의 모든 문서 전체 동기화
  - 135개 파일 변경, 26186줄 추가, 14371줄 삭제

#### 문서 동기화
- `3aada7d` - 번체 중국어(zh-HK) 문서를 간체 중국어와 일치하도록 동기화
  - 3개 파일 변경, 519줄 추가, 422줄 삭제
- `2f6abff` - 모든 언어에 도움말 툴 표시 이름 로컬라이제이션 추가
  - 7개 파일 변경, 47줄 추가, 7줄 삭제

#### 노리지 시스템 리팩토링
- `60944fe` - 네임스페이스를 SiliconLife.Collective로 통일
  - 8개 파일 변경, 5줄 추가, 8줄 삭제
- `69c51c5` - 도움말 문서 시스템 추가 및 코드 주석을 영어로 번역
  - 29개 파일 변경, 3385줄 추가, 22줄 삭제

### 2026-04-25

#### WebView 브라우저 자동화
- `41757c3` - Playwright 기반 크로스 플랫폼 WebView 브라우저 자동화 구현
  - 6개 파일 변경, 1152줄 추가

#### 문서 업데이트
- `0ff797b` - KnowledgeTool 및 WorkNoteTool 문서 추가(7개 언어)
  - 28개 파일 변경, 4983줄 추가
- `ad77415` - 모든 changelog 파일 업데이트, 2026-04-25 Git 기록 추가
  - 7개 파일 변경, 168줄 추가

#### 프로젝트 워크스페이스 관리
- `785c551` - 프로젝트 워크스페이스 관리 구현, 워크 노트 및 태스크 시스템 포함
  - 프로젝트 워크스페이스 관리 시스템 신규 추가
  - 프로젝트 진행 상황 추적을 위한 워크 노트 기능
  - 태스크 관리 시스템 통합
  - 29개 파일 변경, 4256줄 추가, 36줄 삭제

#### 체코어 로컬라이제이션
- `b4bbf39` - 완전한 체코어(cs-CZ) 로컬라이제이션 추가 및 모든 언어 문서 업데이트
  - 116개 파일 변경, 4933줄 추가, 222줄 삭제
- `faf078f` - 체코어 로컬라이제이션 컴파일 오류 수정
  - 3개 파일 변경, 910줄 추가, 1줄 삭제

#### 노리지 시스템 강화
- `20adaac` - KnowledgeTool 추가 및 전체 로컬라이제이션 지원
  - 34개 파일 변경, 2331줄 추가, 56줄 삭제

### 2026-04-24

#### 메모리 관리 시스템 강화
- `c7b2ecc` - 메모리 관리 기능 강화, 고급 필터링, 통계 및 상세 뷰 기능 추가
  - 메모리 고급 필터링 기능 신규 추가
  - 메모리 통계 기능 구현
  - 메모리 상세 뷰 페이지 추가
  - 다국어 로컬라이제이션 지원(6개 언어)
  - 13개 파일 변경, 840줄 추가, 86줄 삭제

#### 권한 시스템 확장
- `4489ad6` - wttr.in 날씨 서비스를 네트워크 화이트리스트에 추가
  - 완전한 다국어 문서 동기화 업데이트(6개 언어)
  - 14개 파일 변경, 417줄 추가, 1줄 삭제

#### Web 인터페이스 수정
- `d9d72e9` - 워크 노트 상세 모달 CSS 우선순위 문제 수정
  - 19개 파일 변경, 1744줄 추가, 6줄 삭제

#### 채팅 기록 최적화
- `0df599c` - 툴 리절트가 독립 채팅 메시지로 렌더링되는 문제 수정
  - 1개 파일 변경, 222줄 추가, 21줄 삭제
- `057b09d` - 채팅 기록 상세 표시 최적화, 툴 콜 렌더링 개선
  - 3개 파일 변경, 389줄 추가, 68줄 삭제

#### 타이머 실행 기록
- `fa3f06f` - 타이머 실행 기록 기능 추가, 상세 뷰 포함
  - 8개 파일 변경, 937줄 추가, 10줄 삭제
- `d824835` - 타이머 실행 기록 로컬라이제이션 키값 추가(모든 언어)
  - 7개 파일 변경, 88줄 추가

#### 로컬라이제이션 강화
- `c13cb17` - 스페인어 언어 변형 등록
  - 1개 파일 변경, 4줄 추가
- `9c44f34` - 중국 역사 캘린더 다국어 로컬라이제이션 지원 추가
  - 16개 파일 변경, 6049줄 추가, 1줄 삭제

#### 핵심 기능 개선
- `1e7c7b2` - 메모리 압축 및 툴 실행 추적 개선
  - 4개 파일 변경, 338줄 추가, 86줄 삭제

### 2026-04-23

#### 툴 로컬라이제이션
- `192fc6e` - 5개 툴에 누락된 툴 이름 로컬라이제이션 추가
  - 6개 파일 변경, 30줄 추가

#### 문서 업데이트
- `882c08f` - 모든 changelog 파일 업데이트, 전체 Git 기록 추가 및 가짜 버전 번호 제거
  - 45개 파일 변경, 8815줄 추가, 1611줄 삭제

#### 채팅 페이지 강화
- `65c157b` - 채팅 페이지에 로딩 인디케이터 추가 및 큐레이터 세션 자동 선택
  - 10개 파일 변경, 211줄 추가, 7줄 삭제

#### 채팅 기록 기능
- `e483348` - 실리콘 비잉 채팅 기록 보기 기능 구현
  - ChatHistoryController 신규 추가
  - ChatHistoryViewModel 생성
  - ChatHistoryListView 및 ChatHistoryDetailView 페이지 구현
  - 채팅 기록 로컬라이제이션 키 추가(5개 언어)
  - 12개 파일 변경, 1178줄 추가

#### AI 스트림 제어 강화
- `30a2d4e` - AI 스트림 취소, IM 통합 및 코어 호스트 초기화 강화
  - 11개 파일 변경, 387줄 추가, 12줄 삭제

#### 채팅 메시지 큐
- `db48c51` - 채팅 메시지 큐, 파일 메타데이터 및 스트림 취소 지원 추가
  - 4개 파일 변경, 357줄 추가

#### 파일 업로드 지원
- `28fb344` - 파일 소스 대화상자 및 파일 업로드 지원 구현
  - 3개 파일 변경, 1100줄 추가, 2줄 삭제
- `1d3e2cc` - 파일 소스 대화상자 로컬라이제이션 문자열 추가(6개 언어)
  - 6개 파일 변경, 30줄 추가

#### 문서 업데이트
- `8111e92` - README의 저장소 섹션에 Wiki 링크 추가
  - 1개 파일 변경, 3줄 추가, 1줄 삭제

### 2026-04-22

#### 문서 로컬라이제이션
- `66c11eb` - 중국어 주석을 영어로 번역 및 모든 changelog 업데이트
  - 11개 파일 변경, 373줄 추가, 163줄 삭제

#### SSE 메시지 강화
- `b574b2b` - AI 식별을 위해 기록 메시지에 senderName 추가
  - 1개 파일 변경, 9줄 추가

#### 채팅 기능
- `601fc14` - 세션 종료 표시를 위한 mark_read 조작 추가
  - 7개 파일 변경, 196줄 추가, 36줄 삭제

#### 툴 시스템 최적화
- `7a03a19` - LogTool 대화 쿼리 유연성 개선
  - 1개 파일 변경, 57줄 추가, 24줄 삭제

#### 로컬라이제이션 강화
- `0a8d750` - 능동적 실리콘 비잉 행동을 위한 범용 시스템 프롬프트 추가
  - 8개 파일 변경, 460줄 추가, 48줄 삭제

#### 로깅 시스템 리팩토링
- `2b771f3` - LogController와 파일 I/O 분리, 로그 읽기 API 추가
  - 4개 파일 변경, 172줄 추가, 137줄 삭제
- `12da302` - 로그 뷰에 실리콘 비잉 필터 추가
  - 9개 파일 변경, 147줄 추가, 10줄 삭제
- `8f6cb1e` - ILogger 인터페이스에 beingId 매개변수 추가, 시스템/실리콘 비잉 로그 분리 구현
  - 47개 파일 변경, 524줄 추가, 490줄 삭제

#### 권한 시스템 개선
- `4c747ad` - PermissionTool, ExecuteCodeTool 리팩토링, EvaluatePermission API 추가
  - 18개 파일 변경, 680줄 추가, 492줄 삭제

#### 버그 수정
- `1c96e99` - search_files 및 search_content 루트 디렉토리 검색 실패 수정
  - 1개 파일 변경, 98줄 추가, 41줄 삭제

#### 툴 통합
- `135710d` - SearchTool 제거, 로컬 검색을 DiskTool로 이동
  - 2개 파일 변경, 185줄 추가, 365줄 삭제

#### 툴 시스템 확장
- `70ce7fb` - 구조화된 데이터베이스 쿼리를 위한 DatabaseTool 구현
  - 1개 파일 변경, 382줄 추가
- `be29a09` - 조작 및 대화 기록 쿼리를 위한 LogTool 구현
  - 1개 파일 변경, 298줄 추가
- `4ea7702` - 동적 권한 관리를 위한 PermissionTool 구현
  - 1개 파일 변경, 457줄 추가
- `1384ff4` - 다국어 코드 실행을 위한 ExecuteCodeTool 구현
  - 1개 파일 변경, 477줄 추가
- `82d1e11` - 정보 검색을 위한 SearchTool 구현
  - 1개 파일 변경, 363줄 추가

#### Web 인터페이스 최적화
- `0675c45` - 미리보기 창의 markdown 코드 블록 하이라이트 최적화
  - 1개 파일 변경, 4줄 추가, 23줄 삭제
- `702b3f3` - 태스크 뷰 강화, 상태 배지 및 메타데이터 표시 추가
  - 8개 파일 변경, 221줄 추가, 9줄 삭제
- `6ed9a79` - 채팅 메시지 저장 및 뷰 렌더링 개선
  - 8개 파일 변경, 140줄 추가, 29줄 삭제

### 2026-04-21

#### 버그 수정
- `c6b518b` - 타이머 메시지 전달 및 채팅 메시지 저장 수정
  - 3개 파일 변경, 297줄 추가, 124줄 삭제

#### 설정 관리
- `4305769` - 줄바꿈 관리를 위한 .gitattributes 추가
  - 1개 파일 변경, 32줄 추가

#### Web 인터페이스 개선
- `188c6f8` - 태스크 목록 API 라우트 등록 및 빈 상태 표시 추가
  - 2개 파일 변경, 35줄 추가, 2줄 삭제
- `634e8ca` - 권한 페이지에 목록으로 돌아가기 링크 추가
  - 1개 파일 변경, 16줄 추가
- `6ba591d` - 실리콘 비잉용 독립 AI 설정 편집기 추가
  - 11개 파일 변경, 842줄 추가, 18줄 삭제
- `0a826f5` - 코드 편집기에 저장 성공 알림 추가
  - 1개 파일 변경, 9줄 추가, 2줄 삭제
- `2940373` - Web 인터페이스 강화, 코드 호버 팁 및 UI 개선 추가
  - 11개 파일 변경, 1054줄 추가, 75줄 삭제

#### 권한 시스템 수정
- `592c7ab` - 콜백 인스턴스화 및 등록 순서 수정
  - 2개 파일 변경, 38줄 추가, 7줄 삭제

#### 보안 강화
- `833ead2` - 동적 컴파일에 어셈블리 참조 검증 추가
  - 4개 파일 변경, 135줄 추가, 8줄 삭제

#### 권한 시스템 강화
- `5879621` - 권한 콜백 사전 컴파일 검증 및 오류 처리 강화 추가
  - 21개 파일 변경, 617줄 추가, 26줄 삭제

#### 문서 업데이트
- `4dbf659` - changelog를 v0.5.1로 업데이트, GitHub 플레이스홀더 URL 교체, Gitee 미러 추가, 언어별 Bilibili 이름 로컬라이제이션, 이메일 업데이트
  - 32개 파일 변경, 489줄 추가, 180줄 삭제

#### 설정 및 진입점
- `0fc1693` - 프로그램 진입점 및 프로젝트 설정 업데이트
  - 2개 파일 변경, 7줄 추가

#### 권한 시스템 리팩토링
- `ea9179a` - 권한 시스템 구현 개선
  - 5개 파일 변경, 358줄 추가, 152줄 삭제

#### 버그 수정
- `928a96d` - 캘린더 계산 구현 수정
  - 4개 파일 변경, 12줄 추가, 12줄 삭제

#### AI 및 캘린더
- `646813e` - AI 클라이언트 팩토리 구현 개선
  - 2개 파일 변경, 21줄 추가, 20줄 삭제

#### 로컬라이제이션
- `7940d9c` - 한국어 로컬라이제이션 지원 추가
  - 7개 파일 변경, 2424줄 추가, 10줄 삭제
- `4ff98ad` - 문서 리팩토링, 다국어 지원
  - 81개 파일 변경, 23818줄 추가, 1886줄 삭제

### 2026-04-20

#### 핵심 기능 완성
- `28905b5` - 완전한 다국어 지원, AI 클라이언트 팩토리, 권한 시스템 및 로컬라이제이션 설정
  - 매니저, 엔트리 및 다양한 로그 레벨이 있는 로깅 시스템
  - 토큰 사용 쿼리 및 추적을 위한 토큰 감사 시스템
  - 다양한 AI 플랫폼 자동 발견 AI 클라이언트 팩토리
  - 자체 저장소가 있는 권한 콜백 시스템
  - 콘솔 로거 구현
  - 영어 및 간체 중국어 다국어 지원
  - 실시간 채팅을 위한 WebSocket이 있는 WebUI 메신저
  - 로컬라이제이션으로 기본 실리콘 비잉 강화
  - 39개 파일 변경, 4670줄 추가, 175줄 삭제

### 2026-04-19

#### 타이머 및 캘린더
- `c933fd8` - 로컬라이제이션, 타이머 시스템, Web 뷰 업데이트 및 툴 추가
  - 개선된 로컬라이제이션 매니저
  - 정기 태스크 스케줄링 시스템
  - AI 설정 및 컨텍스트 관리
  - 32종 캘린더 유형 지원 캘린더 툴
  - 캘린더 API용 Web 컨트롤러
  - 태스크 관리 툴
  - 46개 파일 변경, 4018줄 추가, 975줄 삭제

**아키텍처 개선**
- 스킨 지원 향상을 위해 Web 뷰 아키텍처 재설계
- 더 나은 상태 처리를 위한 비잉 관리 시스템 개선

### 2026-04-18

- `9f585e1` - 로컬라이제이션, 타이머 시스템, Web 뷰 업데이트 및 툴 추가
  - 타이머 및 스케줄링 개선
  - 개선된 UI 컴포넌트가 있는 더 나은 Web 뷰
  - 더 많은 툴 구현
  - 57개 파일 변경, 3328줄 추가, 389줄 삭제

### 2026-04-17

- `9b71fcd` - 코어 모듈 업데이트, zh-HK 문서, 브로드캐스트 채널, 설정 툴 및 감사 Web 뷰 추가
  - 여러 실리콘 비잉이 함께 채팅하는 브로드캐스트 채널
  - 설정 툴 시스템
  - 감사 Web 뷰
  - 번체 중국어 문서
  - 42개 파일 변경, 3533줄 추가, 268줄 삭제

### 2026-04-16

- `5040f05` - 코어 및 기본 모듈 업데이트
  - 모듈 최적화 및 버그 수정
  - 구현 업데이트 및 개선
  - 58개 파일 변경, 9916줄 추가, 111줄 삭제

### 2026-04-15

- `3efab5f` - 여러 모듈 업데이트: AI, Chat, IM, Tools, Web, Localization, Storage
  - AI 클라이언트 개선
  - 채팅 시스템 강화
  - 메신저 프로바이더 업데이트
  - 툴 시스템 최적화
  - Web 인프라 개선
  - 로컬라이제이션 최적화
  - 저장소 시스템 업데이트
  - 33개 파일 변경, 788줄 추가, 232줄 삭제

### 2026-04-14

- `4241a2f` - 채팅 기능 기본 완료, UI 업로드 최적화
  - 채팅 시스템 기능 완료
  - 파일 업로드 UI 최적화
  - 16개 파일 변경, 1234줄 추가, 102줄 삭제

### 2026-04-13

- `c498c31` - 코드 업데이트
  - 일반 코드 개선 및 최적화
  - 32개 파일 변경, 1045줄 추가, 546줄 삭제

### 2026-04-12

#### 문서 및 로컬라이제이션
- `2161002` - 문서 리팩토링 및 로컬라이제이션 강화
  - 17개 파일 변경, 982줄 추가, 92줄 삭제
- `03d94e4` - 설정 시스템 및 로컬라이제이션 강화
  - 25개 파일 변경, 1378줄 추가, 154줄 삭제
- `9976a35` - 정보 페이지 및 로컬라이제이션 추가
  - 14개 파일 변경, 699줄 추가, 44줄 삭제

#### 채팅 및 Web 뷰
- `0c8ccfc` - 채팅 시스템, 로컬라이제이션 및 Web 뷰 강화
  - 13개 파일 변경, 402줄 추가, 56줄 삭제
- `a8f1342` - Web 통신 레이어 재설계, WebSocket에서 SSE로 전환
  - 27개 파일 변경, 793줄 추가, 935줄 삭제

### 2026-04-11

#### 로깅 시스템
- `e8fe259` - 로깅 시스템 및 코드 최적화 추가
  - 37개 파일 변경, 624줄 추가, 91줄 삭제
- `f01c519` - 로깅 시스템 추가, AI 인터페이스 및 Web 뷰 업데이트
  - 31개 파일 변경, 1758줄 추가, 63줄 삭제

### 2026-04-10

- `4962924` - WebSocket 핸들러, 채팅 뷰 및 메신저 상호작용 강화
  - 컨텍스트 매니저 개선
  - 채팅 시스템 강화
  - 메신저 프로바이더 인터페이스 업데이트
  - WebUI 프로바이더 재설계
  - JavaScript 빌더 및 라우터 업데이트
  - 채팅 뷰 최적화
  - WebSocket 핸들러 개선
  - 9개 파일 변경, 365줄 추가, 134줄 삭제

### 2026-04-09

- `f9302bf` - 메신저 프로바이더 인터페이스, 채팅 시스템 및 Web UI 상호작용 강화
  - 메신저 프로바이더 인터페이스 확장
  - 채팅 메시지 및 시스템 개선
  - 컨텍스트 매니저 최적화
  - 기본 실리콘 비잉 강화
  - Web UI 채팅 뷰 개선
  - WebSocket 핸들러 업데이트
  - 10개 파일 변경, 427줄 추가, 93줄 삭제

### 2026-04-07

- `6831ee8` - Web 뷰 및 JavaScript 빌더 재설계
  - 전체 Web 컨트롤러 재설계
  - JavaScript 빌더 완전 재작성
  - 모든 뷰 컴포넌트 업데이트
  - 스킨 시스템 개선
  - 뷰 기본 클래스 아키텍처 향상
  - 23개 파일 변경, 2004줄 추가, 1983줄 삭제

### 2026-04-05

- `41e97fb` - 여러 코어 모듈 및 Web 컨트롤러 업데이트
  - 컨텍스트 매니저 개선
  - 채팅 시스템 및 세션 관리
  - 서비스 로케이터 재설계
  - 실리콘 비잉 기본 클래스 및 매니저 업데이트
  - Web 컨트롤러 전면 업데이트(17개 컨트롤러)
  - 기본 실리콘 비잉 팩토리 개선
  - 31개 파일 변경, 681줄 추가, 326줄 삭제
- `67988d4` - Web UI 모듈 개선, 이그제큐터 뷰 추가, 뷰 및 코어 모듈 정리
  - 61개 파일 변경, 3148줄 추가, 3726줄 삭제

### 2026-04-04

- `b58bb1c` - 초기화 컨트롤러 추가 및 Web 모듈 재설계
  - 초기화 컨트롤러
  - 설정 모듈 재설계
  - 로컬라이제이션 모듈 업데이트
  - 스킨 시스템 개선
  - 라우터 강화
  - 29개 파일 변경, 1269줄 추가, 289줄 삭제
- `f03ac0b` - Web UI 모듈 추가, 메신저 기능 개선
  - 60개 파일 변경, 8481줄 추가, 165줄 삭제

### 2026-04-03

- `192e57b` - 프로젝트 구조 및 코어 런타임 컴포넌트 업데이트
  - 22개 파일 변경, 446줄 추가, 179줄 삭제
- `59faec8` - 코어 및 기본 구현 업데이트
  - 25개 파일 변경, 3056줄 추가, 18줄 삭제
- `d488485` - 동적 컴파일 기능 및 큐레이터 툴 모듈 추가
  - 19개 파일 변경, 1727줄 추가, 11줄 삭제
- `753d1d9` - 보안 모듈 추가, 이그제큐터, 메신저 프로바이더, 로컬라이제이션 및 툴 업데이트
  - 29개 파일 변경, 2352줄 추가, 93줄 삭제
- `a378697` - 5단계 완료 - 툴 시스템 + 이그제큐터
  - 41개 파일 변경, 2651줄 추가, 363줄 삭제

### 2026-04-02

- `e6ad94b` - 테스트 중 설정 파일 삭제 시 채팅 기록 로드 실패 문제 수정
  - 4개 파일 변경, 49줄 추가, 45줄 삭제
- `daa56f5` - 4단계 완료: 영속 메모리(채팅 시스템 + 메신저 채널)
  - 29개 파일 변경, 2051줄 추가, 538줄 삭제

### 2026-04-01

- `bbe2dbb` - 설정 로드 및 채팅 서비스 메시지 라우팅 수정
  - 27개 파일 변경, 1633줄 추가, 147줄 삭제
- `2fa6305` - 2단계 구현: 메인 루프 프레임워크 및 틱 오브젝트 시스템
  - 9개 파일 변경, 594줄 추가, 41줄 삭제
- `32b99a1` - 1단계 구현 - 기본 채팅 기능
  - 19개 파일 변경, 1185줄 추가
- `358e368` - 초기 커밋: 프로젝트 문서 및 라이선스
  - 10개 파일 변경, 1873줄 추가
