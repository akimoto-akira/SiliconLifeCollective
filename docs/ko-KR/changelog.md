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

### 요구사항 문서

- 이 프로젝트의 요구사항 문서는 공개되지 않았습니다.
- 요구사항은 12개 이상의 국제 AI 플랫폼 및 대형 모델 시리즈를 통한 반복 검증을 거쳤으며, 2000줄 이상의 거의 인간이 이해할 수 없는 사용자 스토리 기반 요구사항 문서가 산출되었습니다.

---

## [미발표]

### 2026-07-xx

#### 새로운 기능
- 7개 신규 AI 클라이언트 구현 완료:
  - **DeepSeekClient** — DeepSeek API 직접 연결, 사고망(thinking) 모드, 1M 컨텍스트 윈도우
  - **ZhipuClient** — 즈푸 GLM API 연결, 사고망 모드, 비전(모델별), 1M 컨텍스트
  - **ErnieClient** — 바이두 천판 v2 API 연결, 131K 컨텍스트, 무료 모델 지원
  - **HunyuanClient** — 텐센트 혼위안 API 연결, TokenHub/레거시 듀얼 엔드포인트, 262K 컨텍스트
  - **MiniMaxClient** — MiniMax API 연결, 1M 컨텍스트, 멀티모달, 국내/국제 엔드포인트
  - **MoonshotClient** — 위즈덤 문(Kimi) API 연결, 262K 컨텍스트
  - **SiliconFlowClient** — 실리콘 플로우 API 연결, 100+ 오픈소스 모델 집합, 동적 모델 목록, 1M 컨텍스트
- 링이완우(零一万物) 폐기 표시: 신규 사용자 등록 중단

### 2026-05-26

#### 새로운 기능
- `a49041b` - 러시아어(ru-RU) 로컬라이제이션 지원 추가 (ref task-364)
  - 216개 파일 변경

#### 수정
- `79096f2` - glossary 테이블 형식을 표준 Markdown으로 변경, 불필요한 공백 정렬 제거
  - 1개 파일 변경

#### 문서
- `174a954` - glossary에 누락된 Deutsch/Polski/Português 3개 열 용어 번역 보충
  - 1개 파일 변경

#### 협업 프레임워크
- `5b03d53` - .ai-collab 태스크 기록 업데이트 - task-364 러시아어 로컬라이제이션 (ref task-364)
  - 5개 파일 변경

- `018947d` - 2026-05-25의 sessions 및 changes 아카이브
  - 2개 파일 변경

### 2026-05-25

#### 새로운 기능
- `14721a9` - ThinkOnProject 인력 배치 프롬프트를 상세 실행 가능한 행동 계획으로 세분화 (ref task-363)
  - 20개 파일 변경

#### 수정
- `abb4285` - beingsHtml의 .join() 호출 위치 오류 수정 (ref task-361)
  - 1개 파일 변경

- `1c0b9ed` - WorkflowDetailView에서 states-overview 렌더링으로 인한 state-initial 중복 문자열 버그 삭제 (ref task-362)
  - 6개 파일 변경

#### 협업 프레임워크
- `ecc48a1` - .ai-collab 메타데이터 업데이트(relatedCommit 및 activity log) (ref task-361)
  - 4개 파일 변경

- `64529a7` - 2026-05-24의 sessions 및 changes 아카이브(수동 보완 실행)
  - 28개 파일 변경

- `4150e52` - 완료된 태스크 task-341~361 아카이브 (ref archive)
  - 2개 파일 변경

### 2026-05-24

#### 새로운 기능
- `db60fd9` - 툴 권한 목록에 ToolAction 선언이 없는 툴 표시 및 설정 불가 표시 (ref task-331, task-332, task-333)
  - 21개 파일 변경

- `6004a7f` - WorkflowTemplate 역할 정의 지원 추가 + 12개 언어 로컬라이제이션 + DiskTool 수정 (ref task-346)
  - 24개 파일 변경

- `75ce452` - ProjectSpace 역할 풀 및 ProjectTool 역할 관리 액션 (ref task-347)
  - 12개 파일 변경

- `edfb600` - BuildProjectScenarioContext에 역할 정보 추가 (ref task-348)
  - 21개 파일 변경

- `6a2d713` - HasProjectsWithoutTemplate을 HasProjectsNeedingAttention으로 확장 (ref task-349)
  - 21개 파일 변경

- `a773224` - 워크플로우 태스크 생성 시 역할 풀을 사용하여 실행자 할당 (ref task-350)
  - 6개 파일 변경

- `77a27f9` - TravelCodeWikiTool을 지리적 엔티티 진입점으로 확장 (ref task-353)
  - 8개 파일 변경

- `873ef23` - GeoDataTool 구현 완료, .ai-collab 상태 업데이트 (ref task-352)
  - 7개 파일 변경

- `feaccab` - GeoContentTool 구현 완료, .ai-collab 상태 업데이트 (ref task-351)
  - 6개 파일 변경

- `6e60ad1` - GeoLanguageTool 확장(ObjectPath 지원 + set_word), 메타데이터 역충전 (ref task-356, task-355)
  - 7개 파일 변경

- `4eff807` - 각 GeoLocation 하위 클래스에 GetWikiDocuments() 구현 (ref task-357)
  - 5개 파일 변경

- `baad5df` - MediaWiki API 게시 서비스 구현 (ref task-358)
  - 6개 파일 변경

- `b846a21` - 워크플로우 상세 페이지 구현 (ref task-361)
  - 24개 파일 변경

#### 수정
- `a290088` - CuratorTool로 새로 생성한 실리콘 비잉이 재시작 후 사라지는 문제 수정 (ref task-334)
  - 11개 파일 변경

- `69a8cba` - 태스크 페이지가 beingId로 필터링하지 않는 버그 수정 (ref task-360)
  - 8개 파일 변경

- `7dd1a65` - Router.cs에 워크플로우 상세 페이지 라우트 등록 (ref task-361)
  - 1개 파일 변경

#### 리팩토링
- `5e02711` - 공용 레이어 저장소 경로 추상화 리팩토링, 파일 시스템 하드코딩 제거 (ref task-335)
  - 12개 파일 변경

- `0ec0929` - DynamicBeingLoader.SaveBeingCode가 직접 파일 시스템 조작 대신 IStorage 사용 (ref task-336)
  - 7개 파일 변경

- `9a44b48` - PlaywrightWebView IStorage 브릿지 + WebViewBrowserTool 기본 클래스 분리 (ref task-337, task-340)
  - 11개 파일 변경

- `8fea742` - WebViewBrowserTool 스크린샷 저장 시 직접 파일 시스템 조작 대신 IStorage 사용 (ref task-338)
  - 6개 파일 변경

- `4c24e6d` - DefaultPermissionCallback이 하드코딩 경로 대신 BeingPathResolver 사용 (ref task-339)
  - 6개 파일 변경

- `ab428cd` - DefaultSiliconBeing 다운캐스팅 제거, 기본 클래스 SaveState() 직접 호출 (ref task-344)
  - 7개 파일 변경

- `1e6eb80` - PlaywrightWebView 브라우저 상태 임시 파일 브릿지를 IStorage 직접 읽기/쓰기로 변경 (ref task-341)
  - 7개 파일 변경

- `17f00e9` - DiskTool 검색 조작을 DiskExecutor 경유로 변경 (ref task-342)
  - 8개 파일 변경

- `8158703` - ChatController 첨부 파일 검사를 DiskExecutor 경유로 변경 (ref task-343)
  - 7개 파일 변경

- `3243ae6` - TravelCodeWikiPublishWorkflow을 7단계 상태 머신으로 재작성, 강제 추적 TravelCodeWikiWithAI 파일 제거 (ref task-355)
  - 6개 파일 변경

#### 정리
- `d685288` - HotReloadTool.cs 및 tools/HotReload 디렉토리 삭제 (ref task-345)
  - 8개 파일 변경

#### 문서
- `f1789d1` - README.md 설명 행 최적화 (ref task-359)
  - 9개 파일 변경

#### 협업 프레임워크
- `982c6bb` - .ai-collab에 누락된 relatedCommit 및 commitHash 필드 보충
  - 6개 파일 변경

- `d91e9f8` - task-331~340 아카이브, 태스크 보드 비움
  - 2개 파일 변경

- `9135e30` - task-341~344 공용 레이어 IStorage 리팩토링 + 추상화 수정 게시
  - 1개 파일 변경

- `f70b350` - TravelCodeWikiWithAI 아키텍처 개조 13개 태스크 신규 추가 (ref task-346~358)
  - 2개 파일 변경

- `f81d38b` - ai-collab session 및 task tracking 파일 업데이트
  - 3개 파일 변경

### 2026-05-23

#### 수정
- `9c3c64e` - ExecuteTool 런타임 권한 검증이 프로젝트 수준 제한을 우회하는 문제 수정 (ref task-324)
  - 7개 파일 변경

- `94a9e35` - 권한 템플릿 정의와 ToolActionAttribute 선언 불일치 수정 (ref task-325)
  - 6개 파일 변경

- `e8d8371` - 모든 Action이 비활성화된 툴을 AI 요청에서 전체 제거 (ref task-326)
  - 6개 파일 변경

- `32c7d8a` - 툴 권한 API에 Action 이름 검증 추가 + 채팅 기록 Markdown 렌더링 수정 (ref task-327, task-328, task-329)
  - 9개 파일 변경

- `797db8c` - Markdown 렌더링 fallback에서 mdRendered를 잘못 설정하여 marked 로드 후 재렌더링되지 않는 문제 수정 (ref task-330)
  - 9개 파일 변경

#### 협업 프레임워크
- `1496094` - task-324~327 툴 권한 프레임워크 수정 태스크 게시
  - 776개 파일 변경

- `0d16e63` - 협업 태스크 상태 업데이트, task-330을 커밋 797db8c에 연결, 아카이브 준비
  - 2개 파일 변경

- `e602e1c` - task-316~330 아카이브, 태스크 보드 비움 (ref task-316~330)
  - 2개 파일 변경

- `20291ce` - 일별 sessions 및 changes 아카이브(5월 13-22일)
  - 106개 파일 변경

### 2026-05-22

#### 문서 일관성 수정
- `9e07b27` - 프랑스어 문서(fr-FR)와 소스 코드 일관성 차이 수정 (ref task-307)
  - 10개 파일 변경

- `9e3be72` - 독일어 문서(de-DE)와 소스 코드 일관성 수정 (ref task-308)
  - 5개 파일 변경

- `2bc7151` - 스페인어(es-ES) 문서와 소스 코드 일관성 차이 수정 (ref task-309)
  - 13개 파일 변경

- `f95088e` - 이탈리아어 문서(it-IT)와 소스 코드 일관성 수정 (ref task-310)
  - 11개 파일 변경

- `6ea9f4a` - 폴란드어 문서(pl-PL)와 소스 코드 일관성 수정 (ref task-311)
  - 16개 파일 변경

- `7646923` - 포르투갈어 문서(pt-PT)와 소스 코드 일관성 수정 (ref task-312)
  - 12개 파일 변경

- `7eaf9db` - 체코어 문서(cs-CZ)와 소스 코드 일관성 수정 (ref task-313)
  - 12개 파일 변경

#### 협업 프레임워크
- `3cb7347` - task-313 relatedCommit=7eaf9db 업데이트
  - 1개 파일 변경

### 2026-05-21

#### 새로운 기능
- `99eca78` - 우클릭 메뉴에「저장소 보기(읽기 전용)」기능 추가, 프로세스 내 Speedy.Manager 호출 (ref task-301)
  - 26개 파일 변경

#### 문서 일관성 수정
- `7f65cf1` - zh-CN 문서와 소스 코드 일관성 차이 수정 (ref task-303)
  - 15개 파일 변경

- `a9e2a2c` - 영문(en) 문서와 소스 코드 일관성 차이 수정 (ref task-302)
  - 9개 파일 변경

- `2549105` - 번체 중국어(zh-HK) 문서와 소스 코드 일관성 차이 수정 (ref task-304)
  - 12개 파일 변경

- `277eb50` - 일본어 문서와 소스 코드 일관성 차이 수정 (ref task-305)
  - 10개 파일 변경

- `edce413` - 한국어(ko-KR) 문서와 소스 코드 일관성 차이 수정 (ref task-306)
  - 18개 파일 변경

- `f2adcae` - 포르투갈어 문서와 소스 코드 불일치 문제 수정 (ref task-220)
  - 15개 파일 변경

- `3332987` - 번체 중국어(홍콩) 문서와 소스 코드 불일치 문제 수정 (ref task-218)
  - 14개 파일 변경

- `af9f715` - 폴란드어 문서와 소스 코드 불일치 문제 수정 (ref task-217)
  - 15개 파일 변경

- `2e2b18b` - 한국어 문서와 소스 코드 불일치 문제 수정 (ref task-216)
  - 16개 파일 변경

- `626ebc9` - 일본어 문서와 소스 코드 불일치 문제 수정 (ref task-215)
  - 19개 파일 변경

- `48d061b` - 이탈리아어 문서와 소스 코드 불일치 문제 수정 (ref task-214)
  - 14개 파일 변경

#### 협업 프레임워크
- `6683bee` - Marvis AI 팀 등록, 태스크 상태 업데이트
  - 3개 파일 변경

- `03fc905` - task-210~220 아카이브
  - 5개 파일 변경

### 2026-05-20

#### 새로운 기능
- `65176d4` - 포르투갈어(pt-PT + pt-BR) 전체 로컬라이제이션 지원 추가 (ref task-208)
  - 41개 파일 변경

#### 문서 일관성 수정
- `af4dffd` - zh-CN 문서와 소스 코드의 모든 불일치 문제 수정 (ref task-209)
  - 11개 파일 변경

- `144b945` - 영문(en) 및 체코어(cs-CZ) 문서와 소스 코드 불일치 문제 수정 (ref task-219, task-210)
  - 22개 파일 변경

- `08bec55` - 독일어 문서(de-DE)와 소스 코드 불일치 문제 수정 (ref task-211)
  - 14개 파일 변경

- `7ff28de` - 스페인어(es-ES) 문서와 소스 코드 불일치 문제 수정 (ref task-212)
  - 14개 파일 변경

- `15e2133` - 프랑스어 문서(fr-FR)와 소스 코드 불일치 문제 수정 (ref task-213)
  - 13개 파일 변경

#### 수정
- `7dac388` - 프로젝트 태스크 목록이 표시되지 않는 문제 수정 (ref task-207)
  - 6개 파일 변경

#### 협업 프레임워크
- `7890223` - task-201~209 아카이브, task-210~220 문서 일관성 수정 태스크 게시
  - 5개 파일 변경

### 2026-05-19

#### 새로운 기능
- `cd72846` - PluginLoader 보안 스캔 우회의 안전한 대안 구현 (ref task-203)
  - 13개 파일 변경

- `fc0c00c` - Speedy.Manager 기능 강화 - 신규 생성/가져오기/내보내기/TreeView 계층/진행 창 (ref task-206)
  - 9개 파일 변경

#### 수정
- `ec07118` - ITypeRegistry/IObjectFactory가 플러그인 로드 전에 등록되지 않은 문제 수정 (ref task-205)
  - 8개 파일 변경

- `9e749db` - 프로젝트 생성 시 Creator ID is required 오류 수정 (ref task-204)
  - 4개 파일 변경

#### 인프라
- `43dc092` - CLDR 마이그레이션 - CldrDataProvider 추가, .github 제거
  - 1개 파일 변경

- `c09ec1f` - .gitignore에 cldr/ 추가
  - 1개 파일 변경

- `221f818` - GitHub 동기화를 Gitee 푸시 미러링 방식으로 변경, workflow는 수동 예비만 유지
  - 1개 파일 변경

- `08cdf1a` - GitHub 동기화 workflow 수정 - 재시도 로직 및 변경 없음 시 건너뛰기 추가
  - 1개 파일 변경

- `fb4e77d` - SiliconLife.Speedy.Manager.csproj 업데이트
  - 1개 파일 변경

#### 협업 프레임워크
- `df90af0` - task-203 relatedCommit=cd72846 업데이트
  - 1개 파일 변경

### 2026-05-18

#### 리팩토링
- `e720d06` - Speedy.Manager를 WinForms에서 Avalonia로 완전 전환 (ref task-202)
  - 17개 파일 변경

#### 수정
- `08894a9` - 메모리 타임라인 요약 항목 계층 표시 오류 수정 (ref task-201)
  - 3개 파일 변경

#### 협업 프레임워크
- `2871afb` - 모든 태스크 아카이브, tasks.json 비움
  - 2개 파일 변경

### 2026-05-17

#### 새로운 기능
- `d6eb994` - 프로젝트 목록 페이지에 프로젝트 생성 진입점 및 워크플로우 템플릿 선택 추가 (ref task-203)
  - 14개 파일 변경

- `0872134` - ThinkOnProject 템플릿 없는 프로젝트 큐레이터 주도 오케스트레이션 (ref task-202)
  - 6개 파일 변경

- `cb3188e` - 그룹 채팅 @멘션 시각화 (ref task-208)
  - 4개 파일 변경

- `f9968e5` - AI 클라이언트 ToolCall 기능 선언 및 우아한 저하 (ref task-205)
  - 4개 파일 변경

- `0d2b843` - 그룹 채팅 결정 로직 ShouldReplyInGroupChat (ref task-201)
  - 6개 파일 변경

- `277a2b1` - 노리지 네트워크 보완 - 고급 쿼리 및 그래프 순회 (ref task-207)
  - 9개 파일 변경

#### 수정
- `6d0b66e` - 그룹 채팅 메시지 전송 시 appendMessage TypeError 수정 (ref task-209)
  - 5개 파일 변경

- `b15167c` - task-203에서 누락된 list-workflow-templates 라우트 등록 보충 (ref task-203)
  - 1개 파일 변경

- `dc549a2` - Gitee 동기화 workflow 수정 - token URL에 사용자 이름 추가
  - 1개 파일 변경

#### 인프라
- `e5fa3ad` - GitHub 자동 동기화 schedule 비활성화, Gitee 공식 동기화 방안 대기
  - 1개 파일 변경

#### 협업 프레임워크
- `4a58c82` - 시스템 능력 분석 보고서 + ThinkOnProject 설계 방안 신규 추가
  - 5개 파일 변경

- `8ab29e6` - 시스템 능력 완전성 분석 보고서를 .ai-collab/docs에 아카이브
  - 2개 파일 변경

- `b412d9c` - 이전 태스크 아카이브, 종합 분석 기반으로 task-201~208 재게시
  - 2개 파일 변경

- `437884a` - 협업 메타데이터 업데이트 - task-202/203/204 완료 (ref task-202, task-203, task-204)
  - 2개 파일 변경

- `bf78d79` - 협업 메타데이터 업데이트 - task-201/205/208 완료
  - 2개 파일 변경

- `de6ee0e` - 세션 종료 기록 catpaw-20260517-2215
  - 5개 파일 변경

- `7223b6f` - 세션 종료 기록 catpaw-20260517-2200
  - 4개 파일 변경


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
