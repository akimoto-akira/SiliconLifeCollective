# 변경 로그

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | **한국어** | [Čeština](../cs-CZ/changelog.md)

이 프로젝트의 모든 중요한 변경 사항이 이 파일에 기록됩니다.

형식은 [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)을 기반으로 하며,
이 프로젝트는 [시맨틱 버저닝](https://semver.org/spec/v2.0.0.html)을 따릅니다.

---

## 이 변경 로그에 대하여

### 프로젝트 이중 버전

이 프로젝트는 두 가지 구현 버전을 제공합니다:

- **SiliconLife.Default**: 기본 구현으로, 주로 아키텍처 실현 가능성을 검증하는 데 사용됩니다. 콘솔 애플리케이션, 파일 시스템 JSON 스토리지.
- **SiliconLife.Fast**: 프로덕션 환경을 위한 주요 버전입니다. 크로스 플랫폼 데스크톱 애플리케이션(Windows / macOS / Linux), SpeedyPack 메모리 스토리지 + 비동기 지속성, 심층적인 성능 최적화가 적용되었습니다.

두 버전은 동일한 인터페이스와 기능을 공유하며, 스토리지 구현 및 실행 모드에서만 차이가 있습니다. SiliconLife.Default는 아키텍처 검증 기준으로 사용되고, SiliconLife.Fast는 프로덕션 환경을 위한 주요 버전입니다.

### 프로젝트 기원

- 이 프로젝트는 2026년 3월 20일에 시작되었습니다.
- 이 프로젝트 이전에 아키텍처 설계가 부적절하여 실패한 검증 데모가 있었으며, 여러 AI 플랫폼과 통합할 수 없었습니다.

### 사용된 AI IDE 도구

#### Kiro (Amazon AWS)
- 프로젝트는 초기에 Kiro가 유지 관리했으며, Spec 모드를 사용하여 시작되었습니다.
- Kiro는 Amazon AWS에서 구축한 agentic AI 개발 환경입니다.
- Code OSS(VS Code)를 기반으로 하며, VS Code 설정 및 Open VSX 호환 플러그인을 지원합니다.
- 구조화된 AI 코딩을 위한 사양 기반 개발 워크플로우를 갖추고 있습니다.

#### Comate AI IDE / 문심쾌마 (바이두)
- 문서 및 문서 작업에 가끔 사용됩니다.
- Comate AI IDE는 바이두 문심이 2025년 6월 23일에 출시한 AI 기반 개발 환경 도구입니다.
- 업계 최초의 다중 모달, 다중 지능형 에이전트 협업 AI IDE입니다.
- 설계에서 코드 변환 및 전체 프로세스 AI 지원 코딩 기능을 포함합니다.
- 바이두 문심 4.0 X1 터보 모델로 구동됩니다.

#### Trae (바이트댄스)
- 2025 년 10 월부터 2026 년 4 월까지 사용.
- 지능형 코드 생성 및 프로젝트 관리를 지원하는 AI IDE.

#### Qoder (알리바바)
- 2026 년 4 월 18 일부터 프로젝트 유지 관리에 사용.
- 코드 분석, 문서 생성 및 다중 에이전트 협업을 지원하는 AI 코딩 플랫폼.

#### CatPaw (메이퇀)
- 2026 년 5 월 6 일부터 Qoder 와 혼합 사용.
- 메이퇀 자체 개발 LongCat 시리즈 모델 기반, 강력한 전체 코드 아키텍처 리팩토링 기능 보유.

### 요구 사항 문서

- 이 프로젝트의 요구 사항 문서는 공개되지 않았습니다.
- 요구 사항은 12개 이상의 국제 AI 플랫폼 및 대형 모델 시리즈를 통해 반복적으로 검증되었으며, 2000행 이상의 거의 인간이 이해할 수 없는 사용자 스토리 기반 요구 사항 문서가 생성되었습니다.

---

## [미출시]

## Alpha-0.2

### 2026-05-22

#### 문서 일관성 수정
- `9e07b27` - 프랑스어(fr-FR) 문서와 소스 코드의 일관성 차이 수정 (ref task-307)
  - 10 개 파일 변경

- `9e3be72` - 독일어(de-DE) 문서와 소스 코드의 일관성 수정 (ref task-308)
  - 5 개 파일 변경

- `2bc7151` - 스페인어(es-ES) 문서와 소스 코드의 일관성 차이 수정 (ref task-309)
  - 13 개 파일 변경

- `f95088e` - 이탈리아어(it-IT) 문서와 소스 코드의 일관성 수정 (ref task-310)
  - 11 개 파일 변경

- `6ea9f4a` - 폴란드어(pl-PL) 문서와 소스 코드의 일관성 수정 (ref task-311)
  - 16 개 파일 변경

- `7646923` - 포르투갈어(pt-PT) 문서와 소스 코드의 일관성 수정 (ref task-312)
  - 12 개 파일 변경

- `7eaf9db` - 체코어(cs-CZ) 문서와 소스 코드의 일관성 수정 (ref task-313)
  - 12 개 파일 변경

#### 협업 프레임워크
- `3cb7347` - task-313 relatedCommit=7eaf9db 업데이트
  - 1 개 파일 변경

### 2026-05-21

#### 새로운 기능
- `99eca78` - 우클릭 메뉴에 '저장소 보기(읽기 전용)' 기능 추가, 프로세스 내 Speedy.Manager 호출 (ref task-301)
  - 26 개 파일 변경

#### 문서 일관성 수정
- `7f65cf1` - zh-CN 문서와 소스 코드의 일관성 차이 수정 (ref task-303)
  - 15 개 파일 변경

- `a9e2a2c` - 영어(en) 문서와 소스 코드의 일관성 차이 수정 (ref task-302)
  - 9 개 파일 변경

- `2549105` - 번체중국어(zh-HK) 문서와 소스 코드의 일관성 차이 수정 (ref task-304)
  - 12 개 파일 변경

- `277eb50` - 일본어(ja-JP) 문서와 소스 코드의 일관성 차이 수정 (ref task-305)
  - 10 개 파일 변경

- `edce413` - 한국어(ko-KR) 문서와 소스 코드의 일관성 차이 수정 (ref task-306)
  - 18 개 파일 변경

- `f2adcae` - 포르투갈어 문서와 소스 코드 불일치 문제 수정 (ref task-220)
  - 15 개 파일 변경

- `3332987` - 번체중국어(홍콩) 문서와 소스 코드 불일치 문제 수정 (ref task-218)
  - 14 개 파일 변경

- `af9f715` - 폴란드어 문서와 소스 코드 불일치 문제 수정 (ref task-217)
  - 15 개 파일 변경

- `2e2b18b` - 한국어 문서와 소스 코드 불일치 문제 수정 (ref task-216)
  - 16 개 파일 변경

- `626ebc9` - 일본어 문서와 소스 코드 불일치 문제 수정 (ref task-215)
  - 19 개 파일 변경

- `48d061b` - 이탈리아어 문서와 소스 코드 불일치 문제 수정 (ref task-214)
  - 14 개 파일 변경

#### 협업 프레임워크
- `6683bee` - Marvis AI 팀 등록, 작업 상태 업데이트
  - 3 개 파일 변경

- `03fc905` - task-210~220 보관
  - 5 개 파일 변경

### 2026-05-20

#### 새로운 기능
- `65176d4` - 포르투갈어(pt-PT + pt-BR) 완전 로컬라이제이션 지원 추가 (ref task-208)
  - 41 개 파일 변경

#### 문서 일관성 수정
- `af4dffd` - zh-CN 문서와 소스 코드의 모든 불일치 문제 수정 (ref task-209)
  - 11 개 파일 변경

- `144b945` - 영어(en) 및 체코어(cs-CZ) 문서와 소스 코드 불일치 문제 수정 (ref task-219, task-210)
  - 22 개 파일 변경

- `08bec55` - 독일어(de-DE) 문서와 소스 코드 불일치 문제 수정 (ref task-211)
  - 14 개 파일 변경

- `7ff28de` - 스페인어(es-ES) 문서와 소스 코드 불일치 문제 수정 (ref task-212)
  - 14 개 파일 변경

- `15e2133` - 프랑스어(fr-FR) 문서와 소스 코드 불일치 문제 수정 (ref task-213)
  - 13 개 파일 변경

#### 버그 수정
- `7dac388` - 프로젝트 작업 목록이 표시되지 않는 문제 수정 (ref task-207)
  - 6 개 파일 변경

#### 협업 프레임워크
- `7890223` - task-201~209 보관, task-210~220 문서 일관성 수정 작업 게시
  - 5 개 파일 변경

### 2026-05-19

#### 새로운 기능
- `cd72846` - PluginLoader 보안 스캔 우회의 안전한 대안 구현 (ref task-203)
  - 13 개 파일 변경

- `fc0c00c` - Speedy.Manager 기능 강화 - 생성/가져오기/내보내기/TreeView 계층/진행 창 (ref task-206)
  - 9 개 파일 변경

#### 버그 수정
- `ec07118` - ITypeRegistry/IObjectFactory가 플러그인 로드 전에 등록되지 않은 문제 수정 (ref task-205)
  - 8 개 파일 변경

- `9e749db` - 프로젝트 생성 시 Creator ID is required 오류 수정 (ref task-204)
  - 4 개 파일 변경

#### 인프라
- `43dc092` - CLDR 마이그레이션 - CldrDataProvider 추가, .github 제거
  - 1 개 파일 변경

- `c09ec1f` - cldr/를 .gitignore에 추가
  - 1 개 파일 변경

- `221f818` - GitHub 동기화를 Gitee 푸시 미러 방식으로 변경, workflow는 수동 백업만 유지
  - 1 개 파일 변경

- `08cdf1a` - GitHub 동기화 workflow 수정 - 재시도 로직 및 변경 없음 건너뛰기 추가
  - 1 개 파일 변경

- `fb4e77d` - SiliconLife.Speedy.Manager.csproj 업데이트
  - 1 개 파일 변경

#### 협업 프레임워크
- `df90af0` - task-203 relatedCommit=cd72846 업데이트
  - 1 개 파일 변경

### 2026-05-18

#### 리팩토링
- `e720d06` - Speedy.Manager를 WinForms에서 Avalonia로 완전 리팩토링 (ref task-202)
  - 17 개 파일 변경

#### 버그 수정
- `08894a9` - 메모리 타임라인 요약 항목 레벨 표시 오류 수정 (ref task-201)
  - 3 개 파일 변경

#### 협업 프레임워크
- `2871afb` - 모든 작업 보관, tasks.json 초기화
  - 2 개 파일 변경

### 2026-05-17

#### 새로운 기능
- `d6eb994` - 프로젝트 목록 페이지에 프로젝트 생성 진입점 및 워크플로 템플릿 선택 추가 (ref task-203)
  - 14 개 파일 변경

- `0872134` - ThinkOnProject 템플릿 없는 프로젝트 큐레이터 주도 오케스트레이션 (ref task-202)
  - 6 개 파일 변경

- `cb3188e` - 그룹 채팅 @멘션 시각화 (ref task-208)
  - 4 개 파일 변경

- `f9968e5` - AI 클라이언트 ToolCall 기능 선언 및 우아한 성능 저하 (ref task-205)
  - 4 개 파일 변경

- `0d2b843` - 그룹 채팅 결정 로직 ShouldReplyInGroupChat (ref task-201)
  - 6 개 파일 변경

- `277a2b1` - 지식 네트워크 보완 - 고급 쿼리 및 그래프 순회 (ref task-207)
  - 9 개 파일 변경

#### 버그 수정
- `6d0b66e` - 그룹 채팅 메시지 전송 시 appendMessage TypeError 수정 (ref task-209)
  - 5 개 파일 변경

- `b15167c` - task-203에서 누락된 list-workflow-templates 라우트 등록 추가 커밋 (ref task-203)
  - 1 개 파일 변경

- `dc549a2` - Gitee 동기화 workflow 수정 - 토큰 URL에 사용자 이름 추가
  - 1 개 파일 변경

#### 인프라
- `e5fa3ad` - GitHub 자동 동기화 스케줄 비활성화, Gitee 공식 동기화 방안 대기
  - 1 개 파일 변경

#### 협업 프레임워크
- `4a58c82` - 시스템 능력 분석 보고서 + ThinkOnProject 설계안 추가
  - 5 개 파일 변경

- `8ab29e6` - 시스템 능력 완전성 분석 보고서를 .ai-collab/docs에 보관
  - 2 개 파일 변경

- `b412d9c` - 이전 작업 보관, 종합 분석 기반 task-201~208 재게시
  - 2 개 파일 변경

- `437884a` - 협업 메타데이터 업데이트 - task-202/203/204 완료 (ref task-202, task-203, task-204)
  - 2 개 파일 변경

- `bf78d79` - 협업 메타데이터 업데이트 - task-201/205/208 완료
  - 2 개 파일 변경

- `de6ee0e` - 세션 종료 기록 catpaw-20260517-2215
  - 5 개 파일 변경

- `7223b6f` - 세션 종료 기록 catpaw-20260517-2200
  - 4 개 파일 변경

### 2026-05-16

#### 릴리스 준비
- `476d839` - alpha-0.2 릴리스 작업 추가
  - task-114(CHANGELOG 작성) 및 task-115(버전 번호 업데이트) 생성
  - 1개 파일 변경

### 2026-05-15

#### 인프라
- `672627b` - Gitee 동기화 워크플로우 추가(권한 구성 포함)
  - sync-from-gitee.yml 워크플로우 권한 업데이트
  - 1개 파일 변경, 7행 추가, 4행 삭제

- `3cd5256` - GitHub Actions Gitee 자동 동기화 추가
  - sync-from-gitee.yml 워크플로우 추가
  - 1개 파일 변경, 50행 추가

#### 문서 업데이트
- `aa1d2ad` - 전체 11개 언어 README/아키텍처/시작 가이드 문서 업데이트, SiliconLife.Fast 멀티플랫폼 지원 반영 (ref task-112, task-113)
  - SiliconLife.Fast을 Windows 전용으로 설명한 문서를 수정하여 실제 멀티플랫폼 지원(Windows / macOS / Linux) 반영
  - 11개 언어의 README.md, architecture.md, getting-started.md 업데이트
  - SelectComponent에 hint 속성 지원 추가
  - ConfigView 열거형 드롭다운에 hint 매개변수 전달
  - 11개 언어 현지화에 SelectSearchHint 키 추가
  - 53개 파일 변경, 690행 추가, 194행 삭제

#### 작업 시스템
- `3329f3d` - 작업 시스템 검사 메커니즘 + 현지화 버그 수정 작업 추가
  - task-113 생성: 정보 페이지 현지화 문제 수정
  - task-112 업데이트: Fast 버전 문서 Linux 지원 업데이트
  - 완료된 작업(11개)을 .ai-collab/archive/에 보관
  - 검사 메커니즘 구성 완료: 빠른 검사(30분마다) + 전체 검사(매일 06:00)
  - 2개 파일 변경, 148행 추가, 171행 삭제

#### 협업 프레임워크
- `6038e22` - coze-agent를 .ai-collab 레지스트리에 등록
  - Coze 플랫폼 상주 AI 등록 정보 추가
  - 1개 파일 변경

### 2026-05-14

#### AI 협업 프레임워크
- `7344fbb` - handoff 모드 제거, 작업 목록 기반 방식으로 전환 (v2.0)
  - .ai-collab 디렉토리 구조를 handoff 모드에서 작업 목록 기반으로 재구성
  - tasks.json 핵심 작업 목록 파일 추가
  - activity.log 작업 로그 추가
  - changes/ 및 sessions/ 디렉토리 추가

- `589a48e` - .ai-collab 세션 기록 추가
  - AI 협업 세션 상태 기록 추가

- `5481bcf` - Qoder AI IDE를 협업 레지스트리에 등록
  - Qoder AI 코딩 어시스턴트 등록 정보 추가

- `e2d7b61` - tasks.json relatedCommit 및 changes commitHash 보완
  - 작업 메타데이터 연결 완료

- `a087f0c` - task-101~110 전체 작업 검수
  - 10개 작업 수정 모두 완료 확인

#### 버그 수정
- `fac9435` - task-101~110 전체 10개 작업 수정 및 구현 완료
  - 검색 선택 컴포넌트 힌트 텍스트 누락 수정
  - 정보 페이지 현지화 문제 수정
  - 도움말 시스템 검색 JS 오류 수정
  - 39개 파일 변경, 684행 추가, 121행 삭제

- `c46dfbc` - 모든 대기 작업 완료 (task-001~006)
  - 초기 6개 대기 작업 완료

- `ec176b2` - 작업 목록 덮어쓰기 - 코드 리뷰에서 10개의 새 버그 발견
  - task-101~110(10개 새 작업) 생성

#### 리팩토링
- `ab15915` - 저작권 헤더 통일 + HelpController BOM 및 HelpView 검색 JS 수정
  - 모든 C# 소스 파일의 Apache 2.0 저작권 헤더 통일
  - HelpController BOM 인코딩 문제 수정
  - HelpView 검색 JavaScript 오류 수정

#### 새 기능
- `18a6f5d` - MCP 브라우저 기능 서버 생성 (ref task-111)
  - SiliconLife.McpServer 프로젝트 추가
  - Playwright 브라우저 자동화 MCP 서버 구현

- `9eb251a` - SiliconLife.McpServer 모듈 제거 (ref task-111)
  - 독립 MCP 서버 제거, 기능이 메인 프로젝트에 통합됨

### 2026-05-13

#### 현지화
- `7a62590` - 폴란드어 현지화 지원 추가
  - pl-PL 폴란드어 현지화 구현 추가(PlPL.cs, 1089행)
  - 폴란드어 도움말 문서 현지화 추가(HelpLocalizationPlPL.cs, 3972행)
  - 폴란드어 중국 역사 캘린더 지원 추가(ChineseHistoricalPlPL.cs, 600행)
  - 폴란드어 트레이 현지화 추가(TrayPlPL.cs, 135행)
  - 폴란드어 완전 문서 세트 추가(15개 문서)
  - Language 열거형에 폴란드어 추가
  - 35개 파일 변경, 14379행 추가, 11행 삭제

- `51f9c8e` - 문서의 Ark AI 참조 및 용어 개선 업데이트
  - 다국어 문서의 AI 클라이언트 용어 업데이트

- `7587c12` - 모든 언어의 변경 로그 항목 추가
  - 모든 언어 버전의 변경 로그 동기화 업데이트

#### 창 시스템 마이그레이션
- `b49a07d` - Avalonia 창 상주 모드로 마이그레이션
  - Windows Forms 의존성 제거, Avalonia UI 프레임워크로 완전 마이그레이션
  - 상태 창이 Linux에서 정상 표시(원격 데스크톱 검증)
  - 창 컨트롤 추가: 우클릭 메뉴, 더블클릭으로 Web 열기, 닫기 버튼
  - 다중 AI 협업 프레임워크(.ai-collab/) 추가
  - 트레이 아이콘 초기화 수정(우아한 성능 저하)
  - App.axaml 및 App.cs Avalonia 애플리케이션 진입점 추가
  - 13개 파일 변경, 1442행 추가, 541행 삭제

- `d335aaf` - Linux 플랫폼 창 항상 표시 + 종료 확인 대화상자
  - Linux에서 상태 창 자동 표시(트레이 아이콘 없음)
  - Linux에서 창 종료 시 확인 대화상자 표시
  - Windows/macOS는 기존 트레이 동작 유지
  - --no-tray 매개변수로 트레이 강제 비활성화 지원
  - 확인 대화상자용 ShowMessageBoxAsync 메서드 추가
  - 3개 파일 변경, 206행 추가, 29행 삭제

#### 트레이 시스템 리팩토링
- `841d384` - 트레이 시스템 리팩토링 및 AI 협업 프레임워크 초기화
  - TrayLocalizationBase 정리 및 미사용 속성 제거
  - ShowStatus 현지화 항목 추가
  - App.cs에 트레이 아이콘 클릭 시 상태 창 표시, 현지화 메뉴 항목 추가
  - Program.cs에서 트레이 아이콘 초기화를 StartAsync로 이동
  - TrayStatusWindow 종료 시 종료 대신 숨김으로 변경
  - trae-glm5 및 catpaw를 .ai-collab 프레임워크에 등록
  - .gitignore 업데이트하여 .ai-collab의 모든 파일이 추적되도록 변경
  - 22개 파일 변경, 178행 추가, 1226행 삭제

#### 문서
- `43653bc` - 저장소 설명 및 AI 레지스트리 업데이트
  - 프로젝트 README 및 .ai-collab 등록 정보 업데이트

### 2026-05-12

#### 작업 시스템 웹 뷰
- `0891b3c` - 작업 실행 상세 및 기록 뷰 추가
  - TaskExecutionDetailView 작업 실행 상세 뷰 추가
  - TaskExecutionHistoryView 작업 실행 기록 뷰 추가
  - TaskController 실행 상세 및 기록 쿼리 인터페이스 추가
  - TaskViewModel 작업 뷰 모델 추가
  - TaskCenter 작업 센터 강화
  - TaskSystem 작업 시스템 업데이트
  - 9개 언어 현지화 작업 관련 키 추가
  - 26개 파일 변경, 803행 추가, 55행 삭제

### 2026-05-11

#### 웹 컴포넌트 아키텍처 리팩토링
- `5e687ad` - 컴포넌트 렌더링을 문자열에서 H-tree로 마이그레이션
  - ComponentBase 렌더링 메서드를 문자열 패턴에서 H-tree 구조로 마이그레이션
  - 모든 28개 컴포넌트를 새 렌더링 아키텍처에 적응 (A, Accordion, Button, Calendar, Card, Chart 등)
  - SelectComponent 대규모 리팩토링 (889행 개선)
  - 컨트롤러 및 뷰 동기화 업데이트
  - 33개 파일 변경, 667행 추가, 435행 삭제

- `bfd332d` - Style을 문자열에서 CssBuilder 인라인 스타일로 마이그레이션
  - CssBuilder 스타일 빌더 추가
  - ComponentBase 스타일 시스템을 문자열에서 구조화된 CssBuilder로 마이그레이션
  - LoadingComponent 대폭 강화 (103행 추가)
  - ConfigController, LogController, MemoryController 컨트롤러 스타일 마이그레이션
  - ChatView, ConfigView, LogView, MemoryView 뷰 스타일 마이그레이션
  - 37개 파일 변경, 351행 추가, 157행 삭제

#### 스토리지 시스템 최적화
- `d67a7ee` - 대규모 데이터셋을 위한 QueryLatest 최적화
  - SpeedyTimeStorage QueryLatest 메서드 성능 최적화
  - SpeedyLoggerProvider 로그 제공자 강화
  - 2개 파일 변경, 44행 추가, 5행 삭제

#### 캘린더 시스템 리팩토링
- `9629f88` - TimerExecution 추출 및 타이머 웹 뷰 강화
  - TimerSystem TimerExecution 로직 추출 (175행 제거)
  - SelectComponent 대폭 강화 (427행 개선)
  - TimerController 및 타이머 뷰 강화
  - ContextManager 컨텍스트 관리자 업데이트
  - 12개 파일 변경, 458행 추가, 267행 삭제

#### 현지화
- `5d8ca79` - LogsLoading 현지화 키 추가
  - 9개 언어 LogsLoading 키 추가
  - DefaultLocalizationBase 기본 클래스 정의 추가
  - 11개 파일 변경, 15행 추가

### 2026-05-10

#### 작업 시스템 리팩토링
- `54394f6` - 작업 시스템과 채팅 기록 주기 병합
  - ProjectTaskSystem 프로젝트 작업 시스템 대폭 간소화 (411행 리팩토링)
  - TaskSystem 작업 시스템 간소화 (254행 리팩토링)
  - TaskCenter 작업 센터 리팩토링 (188행 개선)
  - ContextManager 컨텍스트 관리자 최적화 (347행 리팩토링)
  - DefaultSiliconBeing 실리콘 생명체 강화
  - TimerSystem 타이머 시스템 작업 통합
  - IWorkNoteStorage 인터페이스 업데이트
  - SpeedyWorkNoteStorage 및 FileSystemWorkNoteStorage 적응
  - 16개 파일 변경, 648행 추가, 897행 삭제

### 2026-05-09

#### 웹 인터페이스 강화
- `bc50dd7` - 채팅 뷰 개선 및 감사 기능 추가
  - AuditController 감사 컨트롤러 추가 (261행)
  - AuditView 감사 뷰 추가 (379행)
  - AuditViewModel 감사 뷰 모델 추가
  - ChatView 채팅 뷰 대폭 개선 (171행 강화)
  - ChatController 채팅 컨트롤러 업데이트
  - MarkdownEditorComponent 컴포넌트 강화
  - InitController 초기화 컨트롤러 개선
  - ChatSystem 채팅 시스템 기능 추가
  - 14개 파일 변경, 1030행 추가, 112행 삭제

- `c9babce` - 채팅 뷰의 도구 호출 렌더링 개선
  - ChatView 도구 호출 블록 렌더링 강화
  - 1개 파일 변경, 54행 추가, 11행 삭제

#### AI 도구 시나리오 시스템
- `ff2eddd` - 도구 시나리오 필터링 시스템 구현
  - ToolScenarioAttribute 도구 시나리오 속성 추가 (36행)
  - ChatOnlyAttribute 채팅 전용 시나리오 속성 추가 (19행)
  - ToolManager 도구 관리자 시나리오 필터링 추가 (40행)
  - ContextManager 컨텍스트 관리자 시나리오 필터링 적응
  - 4개 파일 변경, 115행 추가, 30행 삭제

- `5709a33` - 도구 클래스에 시나리오 속성 추가
  - 24개 도구 클래스 ToolScenario 속성 어노테이션 추가
  - 캘린더, 채팅, 설정, 큐레이터, 데이터베이스, 디스크, 동적 컴파일 등 포함
  - 24개 파일 변경, 46행 추가, 20행 삭제

#### 작업 시스템 리팩토링
- `2f19a5f` - TaskCenter 및 TaskEnumerator로 작업 시스템 재구성
  - TaskCenter 작업 센터 추가 (235행)
  - TaskEnumerator 작업 열거자 추가 (297행)
  - TaskSystem 작업 시스템 리팩토링 및 간소화
  - DefaultSiliconBeing 실리콘 생명체 새 아키텍처 적응
  - DefaultSiliconBeingFactory 팩토리 업데이트
  - SiliconBeingBase 기본 클래스 강화
  - 7개 파일 변경, 796행 추가, 275행 삭제

#### 권한 시스템 마이그레이션
- `a06ed09` - IM 및 권한 시스템을 App 프로젝트로 마이그레이션
  - PermissionRequestQueue를 Default/Fast에서 App 프로젝트로 마이그레이션 (443행 추가)
  - Default 버전 WebUIProvider 제거 (403행 삭제)
  - Default 버전 HelpTool 제거 (194행 삭제)
  - Default/Fast 버전 중복 PermissionRequestQueue 제거
  - Default 버전 IMPermissionAskHandler 제거
  - PermissionRequestController 컨트롤러 업데이트
  - 14개 파일 변경, 496행 추가, 1183행 삭제

#### AI 컨텍스트 최적화
- `4c8aaff` - 컨텍스트 관리자 최적화 및 서비스 로케이터 강화
  - ContextManager 컨텍스트 관리자 간소화 및 최적화
  - ServiceLocator 서비스 로케이터 강화 (36행 추가)
  - ToolManager 도구 관리자 강화 (34행 추가)
  - DashScopeClient 및 VolcengineArkClient 클라이언트 개선
  - 실행자 (CommandLine, Disk, Network) 업데이트
  - 8개 파일 변경, 116행 추가, 98행 삭제

#### 현지화
- `5c5eef7` - 감사 및 작업 현지화 키 추가
  - DefaultLocalizationBase 127행 현지화 정의 추가
  - 9개 언어 감사 및 작업 관련 키 추가 (각 26행)
  - 11개 파일 변경, 387행 추가

#### 프로젝트 설정
- `2067db6` - 프로젝트 설정 및 gitignore 규칙 업데이트
  - .gitignore 규칙 업데이트
  - DefaultConfigData 및 Fast DefaultConfigData 설정 강화
  - SpeedyWorkNoteStorage 스토리지 개선
  - SpeedyPack 코어 강화
  - 5개 파일 변경, 32행 추가, 6행 삭제

### 2026-05-07

#### 이탈리아어 현지화
- `8adc18c` - 이탈리아어 현지화 지원 추가 및 다국어 문서 업데이트
  - it-IT 이탈리아어 현지화 추가
  - ItIT 현지화 구현 추가 (1909행)
  - ChineseHistoricalItIT 중국 역사 캘린더 이탈리아어 지원 추가 (586행)
  - TrayItIT 트레이 이탈리아어 현지화 추가 (135행)
  - 이탈리아어 전체 문서 세트 추가 (14개 문서: README, API 참조, 아키텍처, 캘린더 시스템, 변경 로그, 기여 가이드 등)
  - 모든 언어 버전의 아키텍처, 개발 가이드, 시작 가이드 등 업데이트
  - Language 언어 열거에 이탈리아어 추가
  - 86개 파일 변경, 11573행 추가, 769행 삭제

#### 문서 동기화
- `12a5deb` - 아키텍처, 변경 로그, 실리콘 생명체 가이드의 다국어 문서 업데이트
  - 8개 언어 README 업데이트
  - 8개 언어 아키텍처 문서 업데이트
  - 8개 언어 변경 로그 업데이트
  - 8개 언어 실리콘 생명체 가이드 업데이트
  - 8개 언어 도구 참조 업데이트
  - 용어집 재구성
  - 46개 파일 변경, 1697행 추가, 442행 삭제

### 2026-05-06

#### 대규모 모듈 리팩토링
- `eeb3be6` - 대규모 모듈 리팩토링 및 재구성
  - SiliconLife.App 프로젝트 구조 조정
  - SiliconLife.Fast 프로젝트 재구성
  - SiliconLife.Default 프로젝트 재구성
  - SiliconLife.Common 공유 모듈 재구성
  - SiliconLife.Core 핵심 모듈 재구성
  - SiliconLife.Speedy 스토리지 엔진 재구성
  - SiliconLife.Speedy.Manager 관리 도구 재구성
  - 119개 파일 변경, 6926행 추가, 3066행 삭제

### 2026-05-04

#### AI 클라이언트
- `24d2c86` - VolcengineArkClient 추가 및 Audit 을 Usage tracking 으로 교체
  - 새 VolcengineArkClient Volcengine Ark AI 클라이언트 추가
  - 스트리밍 및 비스트리밍 모드 지원
  - 내장 이중 속도 제어 (자체 속도 제어 + 서버 속도 제한)
  - OpenAI API 프로토콜 호환
  - Audit 시스템을 Usage tracking 으로 교체
  - 24개 파일 변경, 802행 추가, 21행 삭제

#### 도구 시스템
- `f27650a` - Fast 자체 재시작을 위한 핫 리로드 도구 추가
  - 새 HotReloadTool 핫 리로드 도구 추가
  - SiliconLife.Fast 온라인 컴파일, 업데이트 및 재시작 지원
  - 새 HotReload.exe 독립 업데이트기 추가
  - 안전한 파일 복사 메커니즘 (자체 덮어쓰지 않음)
  - 우아한 종료 및 포트释放 대기
  - 9개 파일 변경, 581행 추가

#### 지역화
- `6a5aad8` - 모든 파일 업데이트 및 프랑스어 지역화 지원 추가
  - 새 fr-FR 프랑스어 지역화 추가
  - 모든 언어 버전 업데이트
  - 도움말 문서 프랑스어 번역
  - 인터페이스 프랑스어 번역
  - 100+ 개 파일 변경

### 2026-05-03

#### 프로젝트 인프라
- `2664b0c` - 프로젝트 인프라 및 종속성 업데이트
  - SiliconLife.Speedy.Manager에 새 Avalonia 관리 인터페이스 추가
  - 새 slc.ico 아이콘 리소스 추가 (1.5MB)
  - PluginLoader 보안 스캔 대폭 강화 (622행 추가)
  - 새 PermissionedStreamFactory 권한 스트림 팩토리 추가 (779행)
  - 새 PermissionRequestQueue 권한 요청 큐 추가 (Default 및 Fast 버전)
  - 새 DebugLoggerProvider 디버그 로그 제공자 추가
  - ConfigDataBase 구성 기본 클래스 강화
  - ToolManager에 플러그인 도구 스캔 기능 추가 (ScanAllPluginAssemblies)
  - SiliconBeingManager 수명 주기 관리 강화
  - DashScopeClient 알리바바 클라우드 AI 클라이언트 대폭 강화 (227행 추가)
  - DefaultSiliconBeingFactory 팩토리 강화
  - Web 뷰 및 컨트롤러 업데이트 (ChatView, WorkNoteView, PermissionRequestController)
  - 9개 언어 지역화 새 키값 추가
  - 35개의 파일 변경, 28080행 추가, 336행 삭제

### 2026-05-02

#### AI 클라이언트 강화
- `c16f99f` - AI 클라이언트, Web UI 및 스토리지 컴포넌트 업데이트
  - DashScopeClient 알리바바 클라우드 클라이언트 대폭 개선
  - SpeedyPackAutoCompactor 자동 압축기 최적화
  - Web 뷰 기본 클래스 및 BeingView 개선
  - 6개의 파일 변경, 240행 추가, 81행 삭제

#### 플러그인 시스템
- `242dc98` - 정보 페이지에 플러그인 목록 추가
  - AboutController에 플러그인 정보 표시 추가
  - AboutViewModel에 플러그인 데이터 모델 추가
  - AboutView에 플러그인 목록 렌더링 추가
  - 9개 언어 지역화에 플러그인 관련 키값 추가
  - 14개의 파일 변경, 160행 추가, 1행 삭제

#### AI 최적화
- `147f8f4` - 컨텍스트 메모리 프롬프트 텍스트 단순화
  - ContextManager AI 프롬프트 최적화
  - 1개의 파일 변경, 1행 추가, 1행 삭제

#### Speedy 스토리지 최적화
- `8bda2d3` - Speedy 스토리지 및 메모리 컨트롤러 구현 업데이트
  - SpeedyPackAutoCompactor 간격 수정
  - SpeedyTimeStorage 경로 처리 최적화
  - MemoryController 메모리 컨트롤러 개선
  - SpeedyPack.Manager UI 업데이트
  - 4개의 파일 변경, 21행 추가, 18행 삭제

#### 트레이 강화
- `8972654` - 트레이 상태 창의 지역화 지원 강화
  - 9개 언어 트레이 지역화에 Speedy 관리 엔트리 추가
  - TrayStatusWindow에 Speedy 관리 메뉴 항목 추가
  - 11개의 파일 변경, 72행 추가

#### Speedy.Manager 최적화
- `6f5db09` - SpeedyPack 관리자 UI 및 내부 컴포넌트 최적화
  - MainForm 인터페이스 리팩토링
  - FreeList 메모리 관리 최적화
  - WriteQueue 쓰기 큐 개선
  - SpeedyPack 핵심 최적화
  - 5개의 파일 변경, 96행 추가, 88행 삭제

#### 스토리지 시스템 강화
- `57f9d5d` - 스토리지 시스템 개선, 자동 압축 및 불완전한 날짜 지원 추가
  - 새 SpeedyPackAutoCompactor 자동 압축 타이머 추가 (30분 간격)
  - SpeedyPackRegistry 싱글톤 관리자 강화
  - SpeedyStorage, SpeedyTimeStorage, SpeedyWorkNoteStorage 어댑터 개선
  - SpeedyPack에 새 FreeList 여유 공간 관리 추가 (149행)
  - PackFileWriter 라이터 리팩토링 최적화
  - WriteOperation, WriteQueue 쓰기 큐 강화
  - SpeedyPackOptions 구성 옵션 확장
  - IncompleteDate에 새 비교 메서드 추가
  - PluginLoader 플러그인 로더 개선
  - Default 및 Fast 버전 Program.cs 초기화 흐름 업데이트
  - DefaultConfigData 구성 데이터 단순화
  - KnowledgeNetwork 지식 네트워크 간소화
  - ChatController, MemoryController 컨트롤러 최적화
  - SpeedyPack.Manager MainForm 기능 강화
  - 22개의 파일 변경, 639행 추가, 253행 삭제

#### Speedy.Manager 업데이트
- `b04ed33` - Speedy.Manager 파일 업데이트

### 2026-05-01

#### 아키텍처 리팩토링: Speedy 스토리지가 LiteDB 대체
- `6600972` - LiteDB를 Speedy 스토리지로 교체, 플러그인 시스템 및 Speedy 프로젝트 추가
  - **새 SiliconLife.Speedy 프로젝트**: 고성능 .spk 스토리지 엔진
    - SpeedyPack 핵심 클래스 (489행): 메모리 디렉토리 매핑 + 엔트리 캐시 + 비동기 쓰기 큐
    - SpeedyPackOptions 구성 클래스: 캐시 TTL, 최대 캐시 엔트리 수, 읽기 전용 모드
    - IPackTransaction 트랜잭션 인터페이스: 원자적 쓰기 작업 지원
    - SpkFileInfo 파일 정보 클래스
    - Internal 디렉토리: DirectoryMap, EntryCache, PackFileReader, PackFileWriter, WriteQueue, WriteOperation, SpeedyTransaction, SpkHeader, PathNormalizer, FreeList
    - 이진 직렬화를 위한 MessagePack 3.1.4 종속성 (LZ4 압축)
  - **새 SiliconLife.Speedy.Manager 프로젝트**: Avalonia 관리 도구
    - MVVM 아키텍처: MainViewModel, DirectoryTreeViewModel, ContentViewerViewModel 등
    - 서비스 계층: PackService, FileDialogService, RecentFilesService, NotificationService
    - 변환기: BoolToVisibility, ByteSizeToString, ContentTypeToIcon, NullToCollapsed
    - 뷰: MainWindow, DirectoryTreeView, ContentViewerPanel, MetadataPanel
    - 대화 상자: FileInfoDialog, ImportDialog, NewEntryDialog
  - **SiliconLife.Fast 스토리지 마이그레이션**: LiteDB → SpeedyPack
    - 새 SpeedyStorage (IStorage 어댑터)
    - 새 SpeedyTimeStorage (ITimeStorage 어댑터)
    - 새 SpeedyWorkNoteStorage (IWorkNoteStorage 어댑터)
    - 새 SpeedyPackRegistry (프로세스 수준 싱글톤 관리)
    - 새 SpeedyPackAutoCompactor (자동 압축 타이머)
    - LiteDB 관련 스토리지 구현 제거 (LiteDBStorage, LiteDBTimeStorage, LiteDBWorkNoteStorage, LiteDBLoggerProvider, LiteDBManager, LiteDBModels)
    - LiteDB 관리 창 관련 코드 제거
  - **플러그인 시스템**:
    - 새 IPlugin 인터페이스 (Core/Plugins/IPlugin.cs)
    - 새 PluginLoader 플러그인 로더 (Core/Plugins/PluginLoader.cs)
    - 디렉토리에서 플러그인 DLL 로드 지원
    - 보안 스캔: 네임스페이스 검사 금지 (System.IO, System.Net, Microsoft.CodeAnalysis 등)
    - 신뢰할 수 있는 어셈블리 화이트리스트 (Google.Protobuf, Newtonsoft.Json, MessagePack 등)
    - 사용자 정의 AssemblyLoadContext 격리 로드
    - ToolManager에 ScanAllPluginAssemblies 메서드 추가
    - CoreHost에 플러그인 로더 통합
  - 119개의 파일 변경, 6926행 추가, 3066행 삭제

#### 규소 기반 생명체 강화
- `3aef4c3` - Stopped 활동 상태 및 오류 처리 개선 추가
  - 규소 기반 생명체에 Stopped 상태 추가
  - 오류 처리 및 복구 메커니즘 강화

#### 지역화 업데이트
- `513c65d` - 모든 언어 버전 및 문서 업데이트
  - 새 MarkdownEditorComponent 컴포넌트 추가 (625행)
  - 새 DetailsComponent 컴포넌트 추가 (130행)
  - 새 AccordionComponent 아코디언 컴포넌트 추가 (285행)
  - BeingController, ChatController, MemoryController, PermissionController 컨트롤러 업데이트
  - BeingView, ChatView, MemoryView, SoulEditorView 뷰 리팩토링
  - 기존 MarkdownEditorView 제거
  - InitController 컴포넌트화 마이그레이션
  - 115개의 파일 변경, 5761행 추가, 2362행 삭제

### 2026-04-30

#### 시스템 트레이 기능
- `101b203` - 트레이 상태 창 및 ApplicationContext 구현
  - 새 트레이 아이콘 리소스 추가 (alpha.png, noWord.png, slc.ico, wordIcon.png)
  - TrayStatusWindow 상태 창 구현
  - 9개 언어의 트레이 지역화 지원 (TrayCsCZ, TrayDeDE, TrayEnUS 등)
  - TrayLocalizationBase 추상 기본 클래스
  - 24개의 파일 변경, 27995행 추가, 1행 삭제 (리소스 파일 포함)

#### 컴포넌트화 UI 아키텍처
- `e61cfaa` - 컴포넌트화 UI 아키텍처 완료, 24개의 컴포넌트 구현
  - MVP 단계 (8개): ComponentBase, Div, Span, Button, Input, Form, Select, Label
  - 두 번째 단계 (6개): Accordion, Card, Tabs, Table, Modal, Message
  - 세 번째 단계 (5개): Calendar, Tree, Chart, FileUpload, RichText
  - Js, Behavior, DomUpdate 등 보조 클래스 추가
  - 25개의 파일 변경, 2666행 추가

- `7449e51` - 컴포넌트 시스템 개선 및 새 스킨 테마 추가
  - A, Button, Div, Form, Input 등 컴포넌트 강화
  - 3가지 새 스킨 테마 추가: HighContrast(고대비), Light(밝은 색), Minimal(최소화)
  - 기존 스킨 업데이트 (Admin, Chat, Creative, Dev)
  - InitController 컴포넌트화 마이그레이션
  - 32개의 파일 변경, 1466행 추가, 1238행 삭제

- `1ba8636` - InitController 컴포넌트화 마이그레이션 시작 (진행 중)
  - 9개의 파일 변경, 574행 추가, 145행 삭제

#### 스토리지 시스템 통일
- `895dff9` - soul.md와 state.json을 IStorage 인터페이스로 통일
  - DefaultSiliconBeing이 IStorage를 사용하여 영혼 파일 및 상태를 읽고 씀
  - 새 StateFileManager 상태 파일 관리자 추가
  - SoulFileManager 리팩토링하여 IStorage에 맞춤
  - 8개의 파일 변경, 201행 추가, 116행 삭제

#### LiteDB 관리 강화
- `a34bef4` - LiteDBManager 추가 및 트레이 지역화 강화
  - 트레이 메뉴에 LiteDB 관리 엔트리 추가
  - 9개 언어 트레이 지역화 업데이트
  - 10개의 파일 변경, 196행 추가

- `c4a79ca` - LiteDB 관리 창을 위한 언어 인식 로컬 팩토리 추가
  - 1개의 파일 변경, 78행 추가

- `5ebc55e` - LiteDBAdminLocalization을 추상 기본 클래스로 변환
  - 10개의 파일 변경, 1356행 추가

#### 구성 시스템 수정
- `2da5256` - ConfigExists 추상 메서드 추가 및 LiteDB 중복 구성 레코드 수정
  - ConfigDataBase에 ConfigExists 메서드 추가
  - Fast 버전 DefaultConfigData가 LiteDB 구성 존재성 검사 구현
  - LiteDB 중복 구성 키 문제 수정
  - 9개의 파일 변경, 210행 추가, 2행 삭제

#### 채팅 및 뷰 최적화
- `d3618ec` - 채팅 세션, 스토리지 시스템, 시간 모델 및 뷰 기본 클래스 최적화
  - BroadcastChannel, GroupChatSession, SingleChatSession 최적화
  - ITimeStorage에 새 쿼리 메서드 추가
  - FileSystemStorage 및 LiteDBStorage 동기화 업데이트
  - ViewBase 리팩토링 최적화 (Default 및 Fast 버전)
  - 11개의 파일 변경, 622행 추가, 392행 삭제

### 2026-04-29

#### 아키텍처 리팩토링: 공유 모듈 추출
- `a102428` - 공유 모듈을 SiliconLife.Default에서 SiliconLife.Common으로 마이그레이션
  - 32가지 달력 구현을 Common 프로젝트로 추출
  - 지역화 기본 클래스 및 21개 언어 구현을 Common 프로젝트로 추출
  - 권한 관리자, 기본 규소 기반 생명체 구현을 Common 프로젝트로 추출
  - 23개의 기본 도구 구현을 Common 프로젝트로 추출
  - Playwright WebView 구현을 Common 프로젝트로 추출
  - 네임스페이스를 SiliconLife.Collective로 업데이트
  - 122개의 파일 변경, 586행 추가, 343행 삭제

#### 코드 품질 개선
- `17566fe` - Core, Common 및 Default 프로젝트의 Console.WriteLine을 로그 시스템으로 교체
  - ContextManager, AuditLogger, DefaultConfigData 등 6개의 파일 업데이트
  - ILogger 인터페이스를 통일하여 코드 유지보수성 향상
  - 6개의 파일 변경, 12행 추가, 8행 삭제

#### SiliconLife.Fast 고성능 버전
- `54a0307` - SiliconLife.Fast 프로젝트 추가 및 컴파일 오류 수정 완료
  - 완전한 크로스 플랫폼 데스크톱 애플리케이션 엔트리 포인트 (Windows / macOS / Linux)
  - 시스템 트레이 지원 (NotifyIcon)
  - 모든 Web UI 컨트롤러 이식 (23개)
  - 모든 Web 뷰 컴포넌트 이식
  - 4가지 스킨 테마 이식 (Admin, Chat, Creative, Dev)
  - 125개의 파일 변경, 61186행 추가

#### 다국어 문서 동기화
- `265fde8` - 이중 버전 아키텍처 문서를 모든 언어로 동기화
  - 7개 언어의 architecture.md, changelog.md 업데이트
  - 6개 언어의 contributing.md 업데이트
  - 7개 언어의 getting-started.md, roadmap.md 업데이트
  - 47개의 파일 변경, 1214행 추가, 38행 삭제

#### LiteDB 스토리지 시스템 (Fast 버전)
- `4704862` - LiteDB 종속성 및 인프라 추가
  - 새 LiteDBManager 관리 클래스 추가
  - 새 LiteDBModels 데이터 모델 추가
  - 3개의 파일 변경, 252행 추가

- `4220036` - LiteDB 스토리지 클래스 구현
  - LiteDBStorage: IStorage 인터페이스 구현
  - LiteDBTimeStorage: ITimeStorage 인터페이스 구현
  - LiteDBWorkNoteStorage: IWorkNoteStorage 인터페이스 구현
  - 3개의 파일 변경, 581행 추가

- `38ebd23` - 구성 및 로그 시스템을 LiteDB로 마이그레이션
  - DefaultConfigData가 LiteDB 스토리지에 맞춤
  - 새 LiteDBLoggerProvider 로그 공급자 추가
  - 2개의 파일 변경, 203행 추가, 67행 삭제

- `e687157` - 지식 네트워크를 파일 시스템에서 LiteDB로 마이그레이션
  - KnowledgeNetwork 완전 리팩토링, LiteDB로 삼중 그룹 데이터 저장
  - 1개의 파일 변경, 231행 추가, 72행 삭제

- `4220169` - LiteDB 스토리지를 Program 및 ProjectManager에 통합
  - Program.cs에서 LiteDB 스토리지 초기화
  - ProjectManager가 LiteDB 작업 노트 스토리지에 맞춤
  - 2개의 파일 변경, 40행 추가, 17행 삭제

- `5f3a709` - 폐기된 파일 시스템 스토리지 구현 제거
  - FileSystemLoggerProvider, FileSystemStorage, FileSystemTimeStorage 등 삭제
  - 6개의 파일 변경, 1518행 삭제

- `e1a4ef2` - docs: 모든 문서에 v0.1.0-alpha 버전 식별자 추가
  - 127개의 파일 변경, 2297행 추가, 2471행 삭제

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### 스토리지 시스템 리팩토링
- `8dd26e3` - ITimeStorage 인터페이스를 IncompleteDate로 통일하고 계층적 쿼리 API 추가
  - ITimeStorage 인터페이스에서 DateTime 오버로드 메서드 제거, IncompleteDate로 통일
  - IncompleteDate에 CompareTo(DateTime) 비교 메서드 및 Expand() 확장 메서드 추가
  - 새 GetEarliestTimestamp(), GetLatestTimestamp() 계층적 쿼리 API 추가
  - 새 HasSummary() 및 QueryWithLevel() 메서드 추가, 시간 계층별 쿼리 지원
  - Memory.cs 리팩토링 압축 알고리즘, 새로운 계층적 쿼리 API 사용으로 효율성 향상
  - FileSystemTimeStorage.cs에 새로운 인터페이스 메서드 완전 구현
  - 모든 호출자 동기화 업데이트: ChatSystem, ChatSession, BroadcastChannel, AuditLogger, TokenUsageRecord 등
  - 도구 시스템 업데이트: HelpTool, LogTool, TokenAuditTool이 새 인터페이스에 맞춤
  - Web 컨트롤러 업데이트: AuditController, ChatController, ChatHistoryController가 새 인터페이스에 맞춤
  - 41개의 파일 변경, 1820행 추가, 903행 삭제

### 2026-04-27

#### 도움말 문서 시스템 강화
- `9989d79` - 지역화, 도움말 시스템 및 Web 뷰 업데이트
  - 새 IAIClientFactoryHelp.cs AI 클라이언트 팩토리 도움말 문서 인터페이스 추가
  - 모든 도움말 문서의 9개 언어 번역 완료
  - HelpTopics.cs에 40개의 새 도움말 주제 정의 추가
  - Web 뷰 완전 업데이트: InitController, AuditView, ConfigView, KnowledgeView, LogView 등
  - 지역화 시스템 강화: 모든 언어 버전에 새 지역화 키 추가
  - AI 클라이언트 팩토리 업데이트: DashScopeClientFactory, OllamaClientFactory 개선
  - 30개의 파일 변경, 10086행 추가, 15행 삭제

#### 도움말 문서 새 내용
- `e7afe94` - 영혼 파일 및 감사 로그 도움말 문서 추가
  - 새 영혼 파일 관리 도움말 문서 추가
  - 새 감사 로그 도움말 문서 추가
  - HelpTopics.cs에 새 주제 정의 추가
  - HelpView.cs 대규모 리팩토링, 문서 렌더링 로직 개선
  - PermissionView.cs 리팩토링, 권한 관리 인터페이스 개선
  - 핵심 모듈 강화: SiliconBeingManager, TaskSystem, ToolManager 개선
  - TaskTool.cs 리팩토링, 작업 관리 기능 개선
  - Web 뷰 완전 업데이트: 모든 뷰 컴포넌트 동기화 업데이트
  - HelpController.cs 단순화, 컨트롤러 로직 최적화
  - 30개의 파일 변경, 7100행 추가, 897행 삭제

### 2026-04-26

#### 도움말 문서 시스템
- `07895d7` - 도움말 문서 시스템 강화, 3개의 새 문서 추가 및 9개 언어 번역 완료
  - 새 메모리 시스템, Ollama 설치 구성, 알리바바 클라우드 DashScope 플랫폼 사용 가이드 추가
  - 모든 10개의 도움말 문서의 9개 언어 번역 완료
  - HelpView 렌더링 로직 단순화
  - 18개의 파일 변경, 14418행 추가, 1364행 삭제

#### 독일어 지역화
- `0cfd8a1` - 완전한 독일어 (de-DE) 지역화 지원 추가
  - 완전한 독일어 지역화 파일
  - 중국 역사 달력 독일어 지원 추가
  - 도움말 문서 독일어 번역 추가
  - 9개 언어의 모든 문서 완전 동기화
  - 135개의 파일 변경, 26186행 추가, 14371행 삭제

#### 문서 동기화
- `3aada7d` - 번체 중국어 (zh-HK) 문서를 간체 중국어와 일치하도록 동기화
  - 3개의 파일 변경, 519행 추가, 422행 삭제
- `2f6abff` - 모든 언어에 도움말 도구 표시 이름 지역화 추가
  - 7개의 파일 변경, 47행 추가, 7행 삭제

#### 지식 시스템 리팩토링
- `60944fe` - 네임스페이스를 SiliconLife.Collective로 통일
  - 8개의 파일 변경, 5행 추가, 8행 삭제
- `69c51c5` - 도움말 문서 시스템 추가 및 코드 주석을 영어로 번역
  - 29개의 파일 변경, 3385행 추가, 22행 삭제

### 2026-04-25

#### WebView 브라우저 자동화
- `41757c3` - Playwright 기반 크로스 플랫폼 WebView 브라우저 자동화 구현
  - 6개의 파일 변경, 1152행 추가

#### 문서 업데이트
- `0ff797b` - KnowledgeTool 및 WorkNoteTool 문서 추가 (7개 언어)
  - 28개의 파일 변경, 4983행 추가
- `ad77415` - 모든 changelog 파일 업데이트, 2026-04-25 Git 히스토리 추가
  - 7개의 파일 변경, 168행 추가

#### 프로젝트 워크스페이스 관리
- `785c551` - 프로젝트 워크스페이스 관리 구현, 작업 노트 및 작업 시스템 포함
  - 새 프로젝트 워크스페이스 관리 시스템 추가
  - 프로젝트 진행 상황 추적을 위한 작업 노트 기능
  - 작업 관리 시스템 통합
  - 29개의 파일 변경, 4256행 추가, 36행 삭제

#### 체코어 지역화
- `b4bbf39` - 완전한 체코어 (cs-CZ) 지역화 추가 및 모든 언어 문서 업데이트
  - 116개의 파일 변경, 4933행 추가, 222행 삭제
- `faf078f` - 체코어 지역화 컴파일 오류 수정
  - 3개의 파일 변경, 910행 추가, 1행 삭제

#### 지식 시스템 강화
- `20adaac` - KnowledgeTool 추가 및 완전 지역화 지원
  - 34개의 파일 변경, 2331행 추가, 56행 삭제

### 2026-04-24

#### 기억 관리 시스템 강화
- `c7b2ecc` - 기억 관리 기능 강화, 고급 필터링, 통계 및 세부 정보 뷰 기능 추가
  - 새 기억 고급 필터링 기능 추가
  - 기억 통계 기능 구현
  - 기억 세부 정보 뷰 페이지 추가
  - 다국어 지역화 지원 (6개 언어)
  - 13개의 파일 변경, 840행 추가, 86행 삭제

#### 권한 시스템 확장
- `4489ad6` - wttr.in 날씨 서비스를 네트워크 화이트리스트에 추가
  - 완전한 다국어 문서 동기화 업데이트 (6개 언어)
  - 14개의 파일 변경, 417행 추가, 1행 삭제

#### Web 인터페이스 수정
- `d9d72e9` - 작업 노트 세부 정보 모달 CSS 우선순위 문제 수정
  - 19개의 파일 변경, 1744행 추가, 6행 삭제

#### 채팅 기록 최적화
- `0df599c` - 도구 결과가 독립적인 채팅 메시지로 렌더링되는 문제 수정
  - 1개의 파일 변경, 222행 추가, 21행 삭제
- `057b09d` - 채팅 기록 세부 정보 표시 최적화, 도구 호출 렌더링 개선
  - 3개의 파일 변경, 389행 추가, 68행 삭제

#### 타이머 실행 기록
- `fa3f06f` - 타이머 실행 기록 기능 추가, 세부 정보 뷰 포함
  - 8개의 파일 변경, 937행 추가, 10행 삭제
- `d824835` - 타이머 실행 기록 지역화 키 추가 (모든 언어)
  - 7개의 파일 변경, 88행 추가

#### 지역화 강화
- `c13cb17` - 스페인어 언어 변형 등록
  - 1개의 파일 변경, 4행 추가
- `9c44f34` - 중국 역사 달력 다국어 지역화 지원 추가
  - 16개의 파일 변경, 6049행 추가, 1행 삭제

#### 핵심 기능 개선
- `1e7c7b2` - 기억 압축 및 도구 실행 추적 개선
  - 4개의 파일 변경, 338행 추가, 86행 삭제

### 2026-04-23

#### 도구 지역화
- `192fc6e` - 5개의 도구에 누락된 도구 이름 지역화 추가
  - 6개의 파일 변경, 30행 추가

#### 문서 업데이트
- `882c08f` - 모든 changelog 파일 업데이트, 완전한 Git 히스토리 추가 및 가짜 버전 번호 제거
  - 45개의 파일 변경, 8815행 추가, 1611행 삭제

#### 채팅 페이지 강화
- `65c157b` - 채팅 페이지에 로딩 인디케이터 추가 및 자동으로 주인 세션 선택
  - 10개의 파일 변경, 211행 추가, 7행 삭제

#### 채팅 기록 기능
- `e483348` - 규소 기반 생명체 채팅 기록 보기 기능 구현
  - 새 ChatHistoryController 추가
  - ChatHistoryViewModel 생성
  - ChatHistoryListView 및 ChatHistoryDetailView 페이지 구현
  - 채팅 기록 지역화 키 추가 (5개 언어)
  - 12개의 파일 변경, 1178행 추가

#### AI 스트림 제어 강화
- `30a2d4e` - AI 스트림 취소, IM 통합 및 핵심 호스트 초기화 강화
  - 11개의 파일 변경, 387행 추가, 12행 삭제

#### 채팅 메시지 큐
- `db48c51` - 채팅 메시지 큐, 파일 메타데이터 및 스트림 취소 지원 추가
  - 4개의 파일 변경, 357행 추가

#### 파일 업로드 지원
- `28fb344` - 파일 소스 대화 상자 및 파일 업로드 지원 구현
  - 3개의 파일 변경, 1100행 추가, 2행 삭제
- `1d3e2cc` - 파일 소스 대화 상자 지역화 문자열 추가 (6개 언어)
  - 6개의 파일 변경, 30행 추가

#### 문서 업데이트
- `8111e92` - README의 리포지토리 섹션에 Wiki 링크 추가
  - 1개의 파일 변경, 3행 추가, 1행 삭제

### 2026-04-22

#### 문서 지역화
- `66c11eb` - 중국어 주석을 영어로 번역 및 모든 changelog 업데이트
  - 11개의 파일 변경, 373행 추가, 163행 삭제

#### SSE 메시지 강화
- `b574b2b` - AI 식별을 위해 역사 메시지에 senderName 추가
  - 1개의 파일 변경, 9행 추가

#### 채팅 기능
- `601fc14` - 세션 종료 표시를 위한 mark_read 작업 추가
  - 7개의 파일 변경, 196행 추가, 36행 삭제

#### 도구 시스템 최적화
- `7a03a19` - LogTool 대화 쿼리 유연성 개선
  - 1개의 파일 변경, 57행 추가, 24행 삭제

#### 지역화 강화
- `0a8d750` - 능동적 규소 기반 생명체 행동을 위한 일반 시스템 프롬프트 추가
  - 8개의 파일 변경, 460행 추가, 48행 삭제

#### 로그 시스템 리팩토링
- `2b771f3` - LogController와 파일 I/O 분리, 로그 읽기 API 추가
  - 4개의 파일 변경, 172행 추가, 137행 삭제
- `12da302` - 로그 뷰에 규소 기반 생명체 필터 추가
  - 9개의 파일 변경, 147행 추가, 10행 삭제
- `8f6cb1e` - ILogger 인터페이스에 beingId 매개변수 추가, 시스템/규소 기반 생명체 로그 분리 구현
  - 47개의 파일 변경, 524행 추가, 490행 삭제

#### 권한 시스템 개선
- `4c747ad` - PermissionTool, ExecuteCodeTool 리팩토링, EvaluatePermission API 추가
  - 18개의 파일 변경, 680행 추가, 492행 삭제

#### 버그 수정
- `1c96e99` - search_files 및 search_content 루트 디렉토리 검색 실패 수정
  - 1개의 파일 변경, 98행 추가, 41행 삭제

#### 도구 통합
- `135710d` - SearchTool 제거, 로컬 검색을 DiskTool로 이동
  - 2개의 파일 변경, 185행 추가, 365행 삭제

#### 도구 시스템 확장
- `70ce7fb` - 구조화된 데이터베이스 쿼리를 위한 DatabaseTool 구현
  - 1개의 파일 변경, 382행 추가
- `be29a09` - 작업 및 대화 기록 쿼리를 위한 LogTool 구현
  - 1개의 파일 변경, 298행 추가
- `4ea7702` - 동적 권한 관리를 위한 PermissionTool 구현
  - 1개의 파일 변경, 457행 추가
- `1384ff4` - 다국어 코드 실행을 위한 ExecuteCodeTool 구현
  - 1개의 파일 변경, 477행 추가
- `82d1e11` - 정보 검색을 위한 SearchTool 구현
  - 1개의 파일 변경, 363행 추가

#### Web 인터페이스 최적화
- `0675c45` - 미리보기 창의 마크다운 코드 블록 하이라이트 최적화
  - 1개의 파일 변경, 4행 추가, 23행 삭제
- `702b3f3` - 작업 뷰 강화, 상태 배지 및 메타데이터 표시 추가
  - 8개의 파일 변경, 221행 추가, 9행 삭제
- `6ed9a79` - 채팅 메시지 스토리지 및 뷰 렌더링 개선
  - 8개의 파일 변경, 140행 추가, 29행 삭제

### 2026-04-21

#### 버그 수정
- `c6b518b` - 타이머 메시지 전달 및 채팅 메시지 스토리지 수정
  - 3개의 파일 변경, 297행 추가, 124행 삭제

#### 구성 관리
- `4305769` - 행 끝 관리용 .gitattributes 추가
  - 1개의 파일 변경, 32행 추가

#### Web 인터페이스 개선
- `188c6f8` - 작업 목록 API 라우트 등록 및 빈 상태 표시 추가
  - 2개의 파일 변경, 35행 추가, 2행 삭제
- `634e8ca` - 권한 페이지에 목록 반환 링크 추가
  - 1개의 파일 변경, 16행 추가
- `6ba591d` - 규소 기반 생명체를 위한 독립 AI 구성 편집기 추가
  - 11개의 파일 변경, 842행 추가, 18행 삭제
- `0a826f5` - 코드 편집기에 저장 성공 메시지 추가
  - 1개의 파일 변경, 9행 추가, 2행 삭제
- `2940373` - Web 인터페이스 강화, 코드 툴팁 및 UI 개선 추가
  - 11개의 파일 변경, 1054행 추가, 75행 삭제

#### 권한 시스템 수정
- `592c7ab` - 콜백 인스턴스화 및 등록 순서 수정
  - 2개의 파일 변경, 38행 추가, 7행 삭제

#### 보안 강화
- `833ead2` - 동적 컴파일에 어셈블리 참조 검증 추가
  - 4개의 파일 변경, 135행 추가, 8행 삭제

#### 권한 시스템 강화
- `5879621` - 권한 콜백 미리 컴파일 검증 및 강화된 오류 처리 추가
  - 21개의 파일 변경, 617행 추가, 26행 삭제

#### 문서 업데이트
- `4dbf659` - changelog를 v0.5.1로 업데이트, GitHub 플레이스홀더 URL 교체, Gitee 미러 추가, 언어별로 Bilibili 이름 지역화, 이메일 업데이트
  - 32개의 파일 변경, 489행 추가, 180행 삭제

#### 구성 및 엔트리
- `0fc1693` - 프로그램 엔트리 및 프로젝트 구성 업데이트
  - 2개의 파일 변경, 7행 추가

#### 권한 시스템 리팩토링
- `ea9179a` - 권한 시스템 구현 개선
  - 5개의 파일 변경, 358행 추가, 152행 삭제

#### 버그 수정
- `928a96d` - 달력 계산 구현 수정
  - 4개의 파일 변경, 12행 추가, 12행 삭제

#### AI 및 달력
- `646813e` - AI 클라이언트 팩토리 구현 개선
  - 2개의 파일 변경, 21행 추가, 20행 삭제

#### 지역화
- `7940d9c` - 한국어 지역화 지원 추가
  - 7개의 파일 변경, 2424행 추가, 10행 삭제
- `4ff98ad` - 문서 리팩토링, 다국어 지원
  - 81개의 파일 변경, 23818행 추가, 1886행 삭제

### 2026-04-20

#### 핵심 기능 개선
- `28905b5` - 완전한 다국어 지원, AI 클라이언트 팩토리, 권한 시스템 및 지역화 설정
  - 관리자, 엔트리 및 다양한 로그 수준이 있는 로그 시스템
  - 토큰 사용량을 쿼리하고 추적하기 위한 토큰 감사 시스템
  - 다양한 AI 플랫폼을 자동으로 발견하는 AI 클라이언트 팩토리
  - 자신의 스토리지가 있는 권한 콜백 시스템
  - 콘솔 로거 구현
  - 영어 및 간체 중국어 다국어 지원
  - 실시간 채팅을 위한 WebSocket이 있는 WebUI 메신저
  - 지역화로 기본 규소 기반 생명체 강화
  - 39개의 파일 변경, 4670행 추가, 175행 삭제

### 2026-04-19

#### 타이머 및 달력
- `c933fd8` - 지역화, 타이머 시스템, Web 뷰 업데이트 및 도구 추가
  - 개선된 지역화 관리자
  - 예약된 작업을 위한 스케줄링 시스템
  - AI 구성 및 컨텍스트 관리
  - 32가지 달력 유형을 지원하는 달력 도구
  - 달력 API를 위한 Web 컨트롤러
  - 작업 관리 도구
  - 46개의 파일 변경, 4018행 추가, 975행 삭제

**아키텍처 개선**
- 스킨을 더 잘 지원하기 위해 Web 뷰 아키텍처 재설계
- 향상된 상태 처리 기능을 갖춘 생명체 관리 시스템 개선

### 2026-04-18

- `9f585e1` - 지역화, 타이머 시스템, Web 뷰 업데이트 및 도구 추가
  - 타이머 및 스케줄링 개선
  - 향상된 UI 컴포넌트가 있는 개선된 Web 뷰
  - 더 많은 도구 구현
  - 57개의 파일 변경, 3328행 추가, 389행 삭제

### 2026-04-17

- `9b71fcd` - 핵심 모듈 업데이트, zh-HK 문서, 브로드캐스트 채널, 구성 도구 및 감사 Web 뷰 추가
  - 여러 규소 기반 생명체가 함께 채팅하기 위한 브로드캐스트 채널
  - 구성 도구 시스템
  - 감사 Web 뷰
  - 번체 중국어 문서
  - 42개의 파일 변경, 3533행 추가, 268행 삭제

### 2026-04-16

- `5040f05` - 핵심 및 기본 모듈 업데이트
  - 모듈 최적화 및 버그 수정
  - 구현 업데이트 및 개선
  - 58개의 파일 변경, 9916행 추가, 111행 삭제

### 2026-04-15

- `3efab5f` - 여러 모듈 업데이트: AI, Chat, IM, Tools, Web, Localization, Storage
  - AI 클라이언트 개선
  - 채팅 시스템 강화
  - 메신저 공급자 업데이트
  - 도구 시스템 최적화
  - Web 인프라 개선
  - 지역화 최적화
  - 스토리지 시스템 업데이트
  - 33개의 파일 변경, 788행 추가, 232행 삭제

### 2026-04-14

- `4241a2f` - 채팅 기능 기본 완료, UI 업로드 최적화
  - 채팅 시스템 기능 완료
  - 파일 업로드 UI 최적화
  - 16개의 파일 변경, 1234행 추가, 102행 삭제

### 2026-04-13

- `c498c31` - 코드 업데이트
  - 일반 코드 개선 및 최적화
  - 32개의 파일 변경, 1045행 추가, 546행 삭제

### 2026-04-12

#### 문서 및 지역화
- `2161002` - 문서 리팩토링 및 지역화 강화
  - 17개의 파일 변경, 982행 추가, 92행 삭제
- `03d94e4` - 구성 시스템 및 지역화 강화
  - 25개의 파일 변경, 1378행 추가, 154행 삭제
- `9976a35` - 정보 페이지 및 지역화 추가
  - 14개의 파일 변경, 699행 추가, 44행 삭제

#### 채팅 및 Web 뷰
- `0c8ccfc` - 채팅 시스템, 지역화 및 Web 뷰 강화
  - 13개의 파일 변경, 402행 추가, 56행 삭제
- `a8f1342` - WebSocket에서 SSE로 전환하여 Web 통신 계층 재설계
  - 27개의 파일 변경, 793행 추가, 935행 삭제

### 2026-04-11

#### 로그 시스템
- `e8fe259` - 로그 시스템 추가 및 코드 최적화
  - 37개의 파일 변경, 624행 추가, 91행 삭제
- `f01c519` - 로그 시스템 추가, AI 인터페이스 및 Web 뷰 업데이트
  - 31개의 파일 변경, 1758행 추가, 63행 삭제

### 2026-04-10

- `4962924` - WebSocket 처리기, 채팅 뷰 및 메신저 상호 작용 강화
  - 컨텍스트 관리자 개선
  - 채팅 시스템 강화
  - 메신저 공급자 인터페이스 업데이트
  - WebUI 공급자 재설계
  - JavaScript 빌더 및 라우터 업데이트
  - 채팅 뷰 최적화
  - WebSocket 처리기 개선
  - 9개의 파일 변경, 365행 추가, 134행 삭제

### 2026-04-09

- `f9302bf` - 메신저 공급자 인터페이스, 채팅 시스템 및 Web UI 상호 작용 강화
  - 메신저 공급자 인터페이스 확장
  - 채팅 메시지 및 시스템 개선
  - 컨텍스트 관리자 최적화
  - 기본 규소 기반 생명체 강화
  - Web UI 채팅 뷰 개선
  - WebSocket 처리기 업데이트
  - 10개의 파일 변경, 427행 추가, 93행 삭제

### 2026-04-07

- `6831ee8` - Web 뷰 및 JavaScript 빌더 재설계
  - 완전한 Web 컨트롤러 재설계
  - JavaScript 빌더 완전히 다시 작성
  - 모든 뷰 컴포넌트 업데이트
  - 스킨 시스템 개선
  - 뷰 기본 클래스 아키텍처 향상
  - 23개의 파일 변경, 2004행 추가, 1983행 삭제

### 2026-04-05

- `41e97fb` - 여러 핵심 모듈 및 Web 컨트롤러 업데이트
  - 컨텍스트 관리자 개선
  - 채팅 시스템 및 세션 관리
  - 서비스 로케이터 재설계
  - 규소 기반 생명체 기본 클래스 및 관리자 업데이트
  - Web 컨트롤러 완전 업데이트 (17개 컨트롤러)
  - 기본 규소 기반 생명체 팩토리 개선
  - 31개의 파일 변경, 681행 추가, 326행 삭제
- `67988d4` - Web UI 모듈 개선, 실행기 뷰 추가, 뷰 및 핵심 모듈 정리
  - 61개의 파일 변경, 3148행 추가, 3726행 삭제

### 2026-04-04

- `b58bb1c` - 초기화 컨트롤러 추가 및 Web 모듈 재설계
  - 초기화 컨트롤러
  - 구성 모듈 재설계
  - 지역화 모듈 업데이트
  - 스킨 시스템 개선
  - 라우터 강화
  - 29개의 파일 변경, 1269행 추가, 289행 삭제
- `f03ac0b` - Web UI 모듈 추가, 메신저 기능 개선
  - 60개의 파일 변경, 8481행 추가, 165행 삭제

### 2026-04-03

- `192e57b` - 프로젝트 구조 및 핵심 런타임 컴포넌트 업데이트
  - 22개의 파일 변경, 446행 추가, 179행 삭제
- `59faec8` - 핵심 및 기본 구현 업데이트
  - 25개의 파일 변경, 3056행 추가, 18행 삭제
- `d488485` - 동적 컴파일 기능 및 주인 도구 모듈 추가
  - 19개의 파일 변경, 1727행 추가, 11행 삭제
- `753d1d9` - 보안 모듈 추가, 실행기, 메신저 공급자, 지역화 및 도구 업데이트
  - 29개의 파일 변경, 2352행 추가, 93행 삭제
- `a378697` - 단계 5 완료 - 도구 시스템 + 실행기
  - 41개의 파일 변경, 2651행 추가, 363행 삭제

### 2026-04-02

- `e6ad94b` - 테스트 중 구성 파일이 삭제될 때 채팅 기록 로드 실패 문제 수정
  - 4개의 파일 변경, 49행 추가, 45행 삭제
- `daa56f5` - 단계 4 완료: 영구 기억 (채팅 시스템 + 메신저 채널)
  - 29개의 파일 변경, 2051행 추가, 538행 삭제

### 2026-04-01

- `bbe2dbb` - 구성 로드 및 채팅 서비스 메시지 라우팅 수정
  - 27개의 파일 변경, 1633행 추가, 147행 삭제
- `2fa6305` - 단계 2 구현: 주요 루프 프레임워크 및 클록 객체 시스템
  - 9개의 파일 변경, 594행 추가, 41행 삭제
- `32b99a1` - 단계 1 구현 - 기본 채팅 기능
  - 19개의 파일 변경, 1185행 추가
- `358e368` - 초기 커밋: 프로젝트 문서 및 라이선스
  - 10개의 파일 변경, 1873행 추가