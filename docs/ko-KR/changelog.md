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
- **SiliconLife.Fast**: 프로덕션 환경을 위한 주요 버전입니다. Windows 폼 애플리케이션, SpeedyPack 메모리 스토리지 + 비동기 지속성, 심층적인 성능 최적화가 적용되었습니다.

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
- 이 프로젝트의 대부분은 주로 Trae를 사용하여 유지 관리되었습니다.
- Trae는 바이트댄스 싱가포르 자회사 SPRING PTE에서 개발한 AI IDE입니다.
- 10x AI 엔지니어로서 소프트웨어 솔루션을 독립적으로 구축할 수 있습니다.
- 지능형 생산성 도구, 유연한 개발 속도 적응, 협업 프로젝트 배포 기능을 갖추고 있습니다.
- 기업급 성능과 구성 가능한 에이전트 시스템을 제공합니다.

#### Qoder (알리바바)
- 2026년 4월 18일부터 이 프로젝트는 Qoder를 사용하여 유지 관리됩니다.
- Qoder는 소스 코드 분석 및 영역 문서 생성에 뛰어나며, 복잡한 코드베이스를 이해하는 데 탁월합니다.
- 제로 컴퓨팅 비용 가격 모델을 채택하여 자동화된 문서 처리 및 일반 작업 처리에서 매우 비용 효율적입니다.
- 실제 소프트웨어 개발을 위해 설계된 AI 기반 agentic 코딩 플랫폼입니다.
- 지능형 코드 생성, 대화형 프로그래밍, 고급 컨텍스트 분석 엔진 및 다중 에이전트 협업 기능을 갖추고 있습니다.
- 최소한의 리소스 소비로 심층적인 코드 이해를 제공하여 장기 프로젝트 유지 관리 및 지식 축적에 매우 적합합니다.

### 요구 사항 문서

- 이 프로젝트의 요구 사항 문서는 공개되지 않았습니다.
- 요구 사항은 12개 이상의 국제 AI 플랫폼 및 대형 모델 시리즈를 통해 반복적으로 검증되었으며, 2000행 이상의 거의 인간이 이해할 수 없는 사용자 스토리 기반 요구 사항 문서가 생성되었습니다.

---

## [미출시]

### 2026-05-03

#### 프로젝트 인프라
- `2664b0c` - 프로젝트 인프라 및 종속성 업데이트
  - SiliconLife.Speedy.Manager에 새 WPF 관리 인터페이스 추가 (MainForm.Designer.cs, MainForm.resx)
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
  - **새 SiliconLife.Speedy.Manager 프로젝트**: WPF 관리 도구
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
  - 완전한 Windows 폼 애플리케이션 엔트리 포인트
  - 시스템 트레이 지원 (NotifyIcon)
  - 모든 Web UI 컨트롤러 이식 (20+개)
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
  - 새 메모리 시스템, Ollama 설치 구성, 알리바이 백련 플랫폼 사용 가이드 추가
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