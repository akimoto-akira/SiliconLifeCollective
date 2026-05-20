![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**버전: v0.2.0-alpha** | **실리콘 생명군** — .NET 9 기반의 멀티 에이전트 협업 플랫폼으로, AI 에이전트를 **실리콘 생명체(Silicon Being)**라고 부르며, Roslyn 동적 컴파일을 통해 자가 진화를 구현합니다.

[English](../README.md) | [Deutsch](../de-DE/README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | **한국어** | [Français](../fr-FR/README.md) | [Čeština](../cs-CZ/README.md) | [Italiano](../it-IT/README.md) | [Polski](../pl-PL/README.md) | [Português](../pt-PT/README.md)

## 🌟 핵심 기능

### 에이전트 시스템
- **멀티 에이전트 오케스트레이션** — *실리콘 큐레이터*가 통합 관리하며, 클록 기반 타임슬롯 공정 스케줄링 메커니즘 사용
- **소울 파일 기반** — 각 실리콘 생명체는 핵심 프롬프트 파일(`soul.md`)에 의해 구동되며, 독특한 개성과 행동 패턴을 정의
- **신체-두뇌 아키텍처** — *신체*(SiliconBeing)는 생명 상태를 유지하고 트리거 시나리오를 감지; *두뇌*(ContextManager)는 기록 로드, AI 호출, 도구 실행 및 응답 영속화 담당
- **자가 진화 능력** — Roslyn 동적 컴파일 기술을 통해 실리콘 생명체가 자신의 코드를 재작성하여 진화 가능
- **활동 상태 관리** — Idle(대기), Working(작업), Error(오류), Stopped(중지) 네 가지 활동 상태 지원, 연속 10 회 오류 발생 시 자동으로 Stopped 상태 진입

### 플러그인 시스템
- **플러그인 확장 아키텍처** — IPlugin 인터페이스를 통해 기능 확장 구현, 디렉토리에서 플러그인 DLL 동적 로드 지원
- **보안 샌드박스** — 플러그인 로더가 엄격한 보안 스캔을 실행하여 System.IO, System.Net 등 네임스페이스 접근 금지
- **격리 로딩** — 커스텀 AssemblyLoadContext를 사용한 격리 로딩으로 플러그인이 메인 프로그램 안정성에 영향을 주지 않음
- **도구 통합** — 플러그인은 ITool 인터페이스를 통해 커스텀 도구를 등록할 수 있으며, 도구 호출 루프에 자동 통합

### 도구 및 실행
- **24개 내장 도구** — 달력, 채팅, 설정, 디스크, 네트워크, 메모리, 작업, 타이머, 지식베이스, 작업 노트, WebView 브라우저, 핫 리로드 등 포괄
- **핫 리로드 도구** — SiliconLife.Fast 가 실행 중인 동안 자동으로 컴파일, 파일 업데이트 및 재시작 지원, 수동 개입 불필요
- **도구 호출 루프** — AI가 도구 호출 반환 → 도구 실행 → 결과를 AI에 피드백 → 순수 텍스트 응답 반환까지 지속 루프
- **실행기-권한 보안** — 모든 I/O 작업은 실행기를 통해 엄격한 권한 검증 수행
  - 3단계 분기 권한 체인: UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curator: GlobalACL)
  - 모든 권한 결정을 완전한 감사 로그로 기록

### AI 및 지식
- **다중 AI 백엔드 지원**
  - **Ollama** — 로컬 모델 배포, 네이티브 HTTP API 사용
  - **알리바바 클라우드 Bailian(DashScope)** — 클라우드 AI 서비스, OpenAI API 호환, 13개 이상 모델 지원, 다중 지역 배포
  - **Volcengine Ark(VolcengineArk)** — ByteDance 클라우드 AI 서비스, 스트리밍 및 비스트리밍 모드 지원, 내장 속도 제어
- **32가지 달력 시스템** — 전 세계 주요 달력 완전 커버, 양력, 음력, 이슬람력, 히브리력, 일본력, 페르시아력, 마야력, 중국 역사력 등 포함
- **지식 네트워크 시스템** — 트리플(주어-관계-목적어) 기반 지식 그래프, 저장, 쿼리 및 경로 발견 지원

### 웹 인터페이스
- **모던 Web UI** — 내장 HTTP 서버, SSE 실시간 업데이트 지원
- **7가지 스킨 테마** — 관리자 버전, 채팅 버전, 창작 버전, 개발 버전, 고대비, 라이트, 미니멀, 자동 감지 및 전환 지원
- **23개 컨트롤러** — 완전한 시스템 관리, 채팅, 설정, 모니터링 기능
- **프론트엔드 프레임워크 의존성 제로** — `H`, `CssBuilder`, `JsBuilder`를 통해 서버에서 HTML/CSS/JS 생성

### 국제화 및 지역화
- **33 개 언어 변형** 전면 지원, 2 가지 문자 시스템과 여러 지역 변형 커버
  - **중국어 간체**：zh-CN (중국 본토), zh-SG (싱가포르), zh-MY (말레이시아) (3 개)
  - **중국어 번체**：zh-HK (홍콩), zh-TW (대만), zh-MO (마카오) (3 개)
  - **영어**：en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 개)
  - **스페인어**：es-ES, es-MX (2 개)
  - **독일어**：de-DE, de-AT, de-CH, de-LU, de-LI (5 개)
  - **프랑스어**：fr-FR, fr-CA, fr-CH (3 개)
  - **일본어**：ja-JP | **한국어**：ko-KR | **체코어**：cs-CZ (3 개)
  - **이탈리아어**：it-IT | **폴란드어**：pl-PL | **포르투갈어**：pt-PT, pt-BR (4 개)

### 데이터 및 스토리지
- **SpeedyPack 고성능 스토리지** — Fast 버전에서 자체 개발 .spk 스토리지 엔진 사용, 메모리 디렉토리 매핑 + 항목 캐시 + 비동기 쓰기 큐
- **파일 시스템 스토리지** — Default 버전에서 순수 파일 시스템 JSON 스토리지 사용
- **시간 인덱스 쿼리** — `ITimeStorage` 인터페이스를 통해 시간 범위별 효율적 쿼리 지원
- **자동 압축** — SpeedyPack 정기 자동 압축 지원, 여유 공간 회수
- **최소 의존성** — 핵심 라이브러리는 동적 컴파일을 위한 Microsoft.CodeAnalysis.CSharp만 의존

## 🔄 듀얼 버전 아키텍처

본 프로젝트는 다양한 시나리오 요구를 충족하기 위해 두 가지 구현 버전을 제공합니다:

### SiliconLife.Default (기본 버전)
- **포지셔닝**: 기본 구현, 아키텍처 실현 가능성 검증에 주로 사용
- **실행 모드**: 콘솔 애플리케이션
- **저장 방식**: 순수 파일 시스템 JSON 저장소
- **적용 시나리오**: 데이터 보안 요구가 높은, 메모리 리소스가 제한된, 데이터 양이 적은 시나리오
- **특징**: 단순하고 신뢰성 높음, 데이터 영속화 즉시, 메모리 손실 위험 없음
- **역할 설명**: 아키텍처 검증의 기준 구현으로, 처음 접촉, 개발 디버깅 또는 데이터 보안 우선 시나리오에 적합
- **시작 명령**: `dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast (고성능 버전)
- **포지셔닝**: 주력 프로덕션 버전
- **실행 모드**: 데스크톱 애플리케이션 (Windows/macOS 시스템 트레이 / Linux 상태 창)
- **저장소**: SpeedyPack 메모리 스토리지 + 비동기 일괄 영속성 (.spk 파일 형식)
- **적용 시나리오**: 높은 동시성, 낮은 지연 시간, 대용량 데이터 시나리오
- **플랫폼 지원**: Windows/macOS (전체 기능, 시스템 트레이 포함), Linux (상태 창, 트레이 아이콘 없음)
- **특징**:
  - 극한 성능 최적화
  - Windows/macOS 시스템 트레이 백그라운드 실행, 트레이 상태 창 실시간 모니터링; Linux 상태 창 직접 표시 지원
  - SpeedyPack 엔진 + 자동 압축으로 데이터 보안 보장
  - Component UI 아키텍처, 27개 선언형 컴포넌트
  - 7가지 스킨 테마, 자동 감지 및 전환 지원
  - 핫 리로드 도구로 온라인 업데이트 및 재시작 지원
- **성능 향상**: 저장소 읽기 지연 시간 1000배 감소, 쓰기 지연 시간 15000배 감소, 동시 실행 처리 능력 50배 향상
- **역할 설명**: 심층 최적화가 적용된 프로덕션급 구현으로, 장기 운영 및 실제 프로덕션 환경의 최선의 선택
- **시작 명령**: `dotnet run --project src/SiliconLife.Fast`

### 버전 비교

| 기능 | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| **실행 모드** | 콘솔 애플리케이션 | 데스크톱 애플리케이션 (Windows/macOS 시스템 트레이 / Linux 상태 창) |
| **사용자 인터페이스** | Web UI (브라우저 액세스) | Windows/macOS: 트레이 아이콘 + 트레이 윈도우 + Web UI; Linux: 상태 창 + Web UI |
| **시스템 트레이** | ❌ 없음 | ✅ Windows/macOS 트레이 최소화 지원; Linux 트레이 아이콘 없음 |
| **백그라운드 실행** | ❌ 콘솔 닫으면 종료 | ✅ Windows/macOS 트레이 백그라운드 지속 실행; Linux 상태 창 실행 |
| **저장 방식** | 파일 시스템 JSON 저장소 | SpeedyPack 메모리 저장소 + 비동기 영속화 |
| **저장 엔진** | 파일 시스템 I/O | SiliconLife.Speedy (.spk 형식) |
| **읽기 지연 시간** | ~10ms (디스크 I/O) | ~0.01ms (메모리 작업) |
| **쓰기 지연 시간** | ~15ms (동기 쓰기) | ~0.001ms (비동기 쓰기) |
| **동시 실행 능력** | ~100 req/s | ~5000 req/s |
| **메모리 사용량** | ~200MB | ~500MB |
| **데이터 보안** | 매우 높음 (즉시 영속화) | 높음 (비동기 영속화 + 자동 압축) |
| **적용 시나리오** | 데이터 보안 우선, 소량 데이터 | 성능 우선, 대용량 데이터, 높은 동시 실행 |

## 🛠️ 기술 스택

| 구성 요소 | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| 런타임 | .NET 9 | .NET 9 (Windows/macOS/Linux) |
| 프로그래밍 언어 | C# | C# |
| 애플리케이션 유형 | 콘솔 애플리케이션 | 데스크톱 애플리케이션 (Windows/macOS 시스템 트레이 / Linux 상태 창) |
| AI 통합 | Ollama (로컬), 알리바바 클라우드 Bailian (클라우드) | Ollama (로컬), 알리바바 클라우드 Bailian (클라우드), Volcengine Ark (클라우드) |
| 데이터 스토리지 | 파일 시스템 (JSON + 시간 인덱스 디렉토리) | SpeedyPack (.spk 형식, 메모리 매핑 + 비동기 영속화) |
| 웹 서버 | HttpListener (.NET 내장) | HttpListener (.NET 내장) |
| 동적 컴파일 | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| 브라우저 자동화 | Playwright (WebView) | Playwright (WebView) |
| 플러그인 시스템 | ✅ 지원 (IPlugin + PluginLoader) | ✅ 지원 (IPlugin + PluginLoader) |
| 시스템 트레이 | ❌ 미지원 | ✅ 지원 (NotifyIcon) |
| 라이선스 | Apache-2.0 | Apache-2.0 |

## 📁 프로젝트 구조

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # 핵심 라이브러리 (인터페이스, 추상 클래스)
│   │   ├── AI/                            # AI 클라이언트 인터페이스, 컨텍스트 관리자, 메시지 모델
│   │   ├── Audit/                         # Token 사용 감사 시스템
│   │   ├── Chat/                          # 채팅 시스템, 세션 관리, 브로드캐스트 채널
│   │   ├── Compilation/                   # 동적 컴파일, 보안 스캔, 코드 암호화
│   │   ├── Config/                        # 설정 관리 시스템
│   │   ├── Executors/                     # 실행기 (디스크, 네트워크, 명령줄)
│   │   ├── IM/                            # 인스턴트 메시징 제공자 인터페이스
│   │   ├── Knowledge/                     # 지식 네트워크 시스템
│   │   ├── Localization/                  # 지역화 시스템
│   │   ├── Logging/                       # 로깅 시스템
│   │   ├── Plugins/                       # 플러그인 시스템 (IPlugin 인터페이스, PluginLoader 로더)
│   │   ├── Project/                       # 프로젝트 관리 시스템
│   │   ├── Runtime/                       # 메인 루프, 클록 객체, 핵심 호스트
│   │   ├── Security/                      # 권한 관리 시스템
│   │   ├── SiliconBeing/                  # 실리콘 생명체 베이스 클래스, 관리자, 팩토리
│   │   ├── Storage/                       # 스토리지 인터페이스
│   │   ├── Time/                          # 불완전한 날짜 (시간 범위 쿼리)
│   │   ├── Tools/                         # 도구 인터페이스 및 도구 관리자
│   │   ├── WebView/                       # WebView 브라우저 인터페이스
│   │   └── ServiceLocator.cs              # 글로벌 서비스 로케이터
│   │
│   ├── SiliconLife.Common/                # 공유 구현 (두 버전 공용)
│   │   ├── AI/                            # AI 클라이언트 및 팩토리 (Ollama, DashScope, VolcengineArk)
│   │   ├── Calendar/                      # 32가지 달력 구현
│   │   ├── Localization/                  # 지역화 베이스 클래스 및 33개 언어/지역 변형 구현
│   │   ├── Resources/                     # 공유 리소스 파일
│   │   ├── Security/                      # 권한 관리자
│   │   ├── SiliconBeing/                  # 기본 실리콘 생명체 구현
│   │   ├── Tools/                         # 24개 일반 도구 구현 (핫 리로드 도구 포함)
│   │   ├── Web/                           # Web 기반 시설
│   │   └── WebView/                       # Playwright WebView 구현
│   │
│   ├── SiliconLife.App/                   # 애플리케이션 레이어 (Web UI + 도움말 문서, Default와 Fast 공유)
│   │   ├── Config/                        # 애플리케이션 설정
│   │   ├── Data/                          # 데이터 디렉토리
│   │   ├── Help/                          # 도움말 문서 지역화 (다국어)
│   │   └── Web/                           # Web UI 구현
│   │       ├── Component/                 # UI 컴포넌트 라이브러리 (27개 컴포넌트)
│   │       ├── Controllers/               # 23개 컨트롤러
│   │       ├── Models/                    # 뷰 모델
│   │       ├── Views/                     # HTML 뷰
│   │       └── Skins/                     # 7가지 스킨 테마
│   │
│   ├── SiliconLife.Default/               # 기본 구현 + 애플리케이션 진입점 (콘솔 버전)
│   │   ├── Program.cs                     # 진입점 (모든 구성 요소 조립)
│   │   ├── Config/                        # 기본 설정 데이터
│   │   ├── IM/                            # WebUI 제공자
│   │   ├── Knowledge/                     # 지식 네트워크 구현
│   │   ├── Logging/                       # 로깅 제공자 구현
│   │   ├── Project/                       # 프로젝트 시스템 구현
│   │   ├── Security/                      # 기본 권한 콜백
│   │   ├── Storage/                       # 파일 시스템 스토리지 구현
│   │   └── Tools/                         # 버전별 도구 구현 (HelpTool)
│   │
│   ├── SiliconLife.Fast/                  # 고성능 구현 + 애플리케이션 진입점 (데스크톱 버전)
│   │   ├── Program.cs                     # 진입점 (데스크톱 애플리케이션)
│   │   ├── App.axaml / App.cs             # Avalonia 애플리케이션 정의
│   │   ├── Config/                        # 설정 데이터 (Default와 공유)
│   │   ├── IM/                            # WebUI 제공자
│   │   ├── Knowledge/                     # 지식 네트워크 구현 (메모리 최적화)
│   │   ├── Logging/                       # 고성능 로깅 제공자
│   │   ├── Project/                       # 프로젝트 시스템 구현
│   │   ├── Security/                      # 최적화 권한 콜백
│   │   ├── Storage/                       # SpeedyPack 스토리지 어댑터
│   │   ├── Tools/                         # 버전별 도구 구현 (HelpTool)
│   │   └── Tray/                          # 시스템 트레이 (33개 언어 변형 지역화)
│   │
│   ├── SiliconLife.Speedy/                # SpeedyPack 고성능 스토리지 엔진
│   │   ├── SpeedyPack.cs                  # 핵심 클래스 (메모리 디렉토리 매핑 + 캐시 + 비동기 쓰기)
│   │   ├── SpeedyPackOptions.cs           # 설정 옵션 (캐시 TTL, 최대 항목 수 등)
│   │   ├── IPackTransaction.cs            # 트랜잭션 인터페이스
│   │   ├── SpkFileInfo.cs                 # 파일 정보
│   │   └── Internal/                      # 내부 구현
│   │       ├── DirectoryMap.cs            # 메모리 디렉토리 매핑
│   │       ├── EntryCache.cs              # 항목 캐시
│   │       ├── FreeList.cs                # 여유 공간 관리
│   │       ├── PackFileReader.cs          # 팩 파일 리더
│   │       ├── PackFileWriter.cs          # 팩 파일 라이터
│   │       ├── WriteQueue.cs              # 비동기 쓰기 큐
│   │       ├── WriteOperation.cs          # 쓰기 작업
│   │       ├── SpeedyTransaction.cs       # 트랜잭션 구현
│   │       ├── SpkHeader.cs               # 팩 파일 헤더
│   │       └── PathNormalizer.cs          # 경로 정규화
│   │
│   └── SiliconLife.Speedy.Manager/        # SpeedyPack 관리 도구 (Avalonia UI)
│       ├── MainWindow.axaml               # 메인 윈도우
│       ├── MainWindow.axaml.cs            # 메인 윈도우 로직
│       ├── Program.cs                     # 진입점
│       └── slc.ico                        # 애플리케이션 아이콘
│
├── docs/                                  # 다국어 문서
│   ├── en/                                # 영어
│   ├── zh-CN/                             # 중국어 간체
│   ├── zh-HK/                             # 중국어 번체
│   ├── de-DE/                             # 독일어
│   ├── ja-JP/                             # 일본어
│   ├── ko-KR/                             # 한국어
│   ├── es-ES/                             # 스페인어
│   ├── fr-FR/                             # 프랑스어
│   ├── cs-CZ/                             # 체코어
│   └── ...                                # 기타 언어 문서
```

## 🏗️ 아키텍처 개요

### 스케줄링 아키텍처
```
메인 루프 (전용 스레드, 워치독 + 서킷 브레이커)
  └── 클록 객체 (우선순위별 정렬)
       └── 실리콘 생명체 관리자
            └── 실리콘 생명체 러너 (임시 스레드, 타임아웃 + 서킷 브레이커)
                 └── 실리콘 생명체.Tick()
                      └── 컨텍스트 관리자.생각()
                           └── AI 클라이언트.채팅()
                                └── 도구 호출 루프 → 채팅 시스템에 영속화
```

### 보안 아키텍처
모든 AI가 시작하는 I/O 작업은 엄격한 보안 체인을 통과해야 합니다:

```
도구 호출 → 실행기 → 권한 관리자 → [주파수 캐시 → 콜백 → (IsCurator: 사용자에게 문의 | Non-curator: 글로벌 ACL)]
```

## 🚀 빠른 시작

### 사전 요구사항

- **.NET 9 SDK** — [다운로드 링크](https://dotnet.microsoft.com/download/dotnet/9.0)
- **AI 백엔드** (둘 중 하나 선택):
  - **Ollama**: [Ollama 설치](https://ollama.com) 및 모델 풀 (예: `ollama pull llama3`)
  - **알리바바 클라우드 Bailian**: [Bailian 콘솔](https://bailian.console.aliyun.com/)에서 API 키 획득
  - **Volcengine Ark**: [Volcengine 콘솔](https://console.volcengine.com/ark)에서 API 키 획득

### 프로젝트 빌드

```bash
dotnet restore
dotnet build
```

### 시스템 실행

#### 방식 1: Default 버전 실행 (콘솔 애플리케이션)

```bash
dotnet run --project src/SiliconLife.Default
```

애플리케이션이 웹 서버를 시작하고 자동으로 브라우저에서 Web UI를 엽니다.

**적용 시나리오**:
- ✅ 데이터 보안 요구가 매우 높음
- ✅ 메모리 리소스가 제한됨 (RAM < 2GB)
- ✅ 데이터 양이 적고 단기 사용
- ✅ 개발 디버깅 단계

#### 방식 2: Fast 버전 실행 (크로스 플랫폼 데스크톱 애플리케이션)

```bash
dotnet run --project src/SiliconLife.Fast
```

애플리케이션이 데스크톱 모드로 시작되어 시스템 트레이로 최소화되고 백그라운드에서 지속 실행됩니다.

**적용 시나리오**:
- ✅ 높은 동시 실행 시나리오 (> 5 사용자)
- ✅ 대용량 데이터 (3개월 이상 사용)
- ✅ 낮은 지연 시간 응답 필요
- ✅ 트레이 백그라운드 실행 필요

### 단일 파일 게시

```bash
# Windows - Default 버전
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Windows - Fast 버전
dotnet publish src/SiliconLife.Fast -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux - Default 버전
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# Linux - Fast 버전
dotnet publish src/SiliconLife.Fast -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS - Default 버전
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true

# macOS - Fast 버전
dotnet publish src/SiliconLife.Fast -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 개발 로드맵

### ✅ 완료됨
- [x] 단계 1: 콘솔 AI 채팅
- [x] 단계 2: 프레임워크 스켈레톤 (메인 루프 + 클록 객체 + 워치독 + 서킷 브레이커)
- [x] 단계 3: 첫 번째 소울 파일 포함 실리콘 생명체 (신체-두뇌 아키텍처)
- [x] 단계 4: 영속성 메모리 (채팅 시스템 + 시간 스토리지 인터페이스)
- [x] 단계 5: 도구 시스템 + 실행기
- [x] 단계 6: 권한 시스템 (3단계 분기 체인, 감사 로거, 글로벌 액세스 제어 목록)
- [x] 단계 7: 동적 컴파일 + 자가 진화 (Roslyn)
- [x] 단계 8: 장기 메모리 + 작업 + 타이머
- [x] 단계 9: 핵심 호스트 + 멀티 에이전트 협업
- [x] 단계 10: Web UI (HTTP + SSE, 23개 컨트롤러, 7가지 스킨)
- [x] 단계 10.5: 점진적 향상 (브로드캐스트 채널, Token 감사, 32개 달력, 도구 향상, 33개 언어 변형 지역화)
- [x] 단계 10.6: 완성 및 최적화 (WebView, 도움말 시스템, 프로젝트 워크스페이스, 지식 네트워크)
- [x] 단계 11: SpeedyPack 스토리지 엔진 (LiteDB 교체, 메모리 매핑, 비동기 쓰기 큐, 자동 압축)
- [x] 단계 12: 플러그인 시스템 (IPlugin 인터페이스, PluginLoader 보안 샌드박스, 격리 로딩, 도구 통합)

### 🚧 계획 중
- [ ] 단계 13: 외부 인스턴트 메시징 통합 (Feishu / WhatsApp / Telegram)
- [ ] 단계 14: 스킬 생태계 (플러그인 마켓플레이스, 스킬 팩 배포)

## 📚 문서

- [아키텍처 설계](architecture.md) — 시스템 설계, 스케줄링 메커니즘, 구성 요소 아키텍처
- [보안 모델](security.md) — 권한 모델, 실행기, 동적 컴파일 보안
- [개발 가이드](development-guide.md) — 도구 개발, 확장 가이드
- [API 참고](api-reference.md) — Web API 엔드포인트 문서
- [도구 참고](tools-reference.md) — 내장 도구 상세 설명
- [Web UI 가이드](web-ui-guide.md) — 웹 인터페이스 사용 가이드
- [실리콘 생명체 가이드](silicon-being-guide.md) — 에이전트 개발 가이드
- [권한 시스템](permission-system.md) — 권한 관리 상세
- [달력 시스템](calendar-system.md) — 32가지 달력 시스템 설명
- [빠른 시작](getting-started.md) — 상세 입문 가이드
- [문제 해결](troubleshooting.md) — 자주 묻는 질문
- [로드맵](roadmap.md) — 완전한 개발 계획
- [변경 로그](changelog.md) — 버전 업데이트 이력
- [기여 가이드](contributing.md) — 프로젝트 참여 방법

## 🤝 기여하기

모든 형태의 기여를 환영합니다! 자세한 내용은 [기여 가이드](contributing.md)를 참조하세요.

### 개발 워크플로우
1. 이 저장소를 Fork
2. 기능 브랜치 생성 (`git checkout -b feature/AmazingFeature`)
3. 변경사항 커밋 (`git commit -m 'feat: add some AmazingFeature'`)
4. 브랜치에 푸시 (`git push origin feature/AmazingFeature`)
5. Pull Request 제출

## � 버전 선택 가이드

### 어떤 버전을 사용해야 하나요?

**SiliconLife.Default (기본 구현 — 아키텍처 실현 가능성 검증):**
- 📌 이 프로젝트를 처음 접하고 시스템 아키텍처를 빠르게 이해하고 싶음
- 📌 개발 디버깅 중이며 간단하고 직접적인 실행 방식이 필요
- 📌 데이터 보안이 최우선 고려사항
- 📌 시스템 메모리가 4GB 미만
- 📌 1인 사용 또는 데이터 양이 적음

**SiliconLife.Fast (주력 프로덕션 버전):**
- ⚡ 장기 안정 운영되는 프로덕션 환경이 필요
- ⚡ 시스템 아키텍처에 익숙하며 정식 배포 준비 완료
- ⚡ 다중 사용자 동시 접속 지원이 필요
- ⚡ 시스템 트레이 백그라운드 실행이 필요
- ⚡ 극한의 성능 경험을 추구

> **전체 권장 사항**: SiliconLife.Default는 아키텍처 검증 및 입문 체험에 적합; 실제 프로덕션 환경에는 SiliconLife.Fast 사용을 강력히 권장.

### Default에서 Fast로 마이그레이션할 수 있나요?

**완전히 가능합니다!** 두 버전은 동일한 것을 공유합니다:
- ✅ 설정 파일 형식 (config.json)
- ✅ 도구 인터페이스
- ✅ Being 설정
- ✅ Web UI 인터페이스

**마이그레이션 단계:**
1. Default 데이터 디렉토리 백업
2. 동일한 데이터 디렉토리로 Fast 버전 시작
3. Fast가 기존 데이터를 SpeedyPack 스토리지 엔진에 자동 가져오기
4. 기능 정상 확인 후 Fast 버전을 일상적으로 사용

### 두 버전을 공존시킬 수 있나요?

**가능합니다!** 다음 배포 전략을 권장합니다:

**전략 1: Default 검증, Fast 프로덕션**
```
개발/검증 환경: SiliconLife.Default (아키텍처 검증, 기능 디버깅)
프로덕션 환경: SiliconLife.Fast (고성능, 백그라운드 실행, 실시간 요청 처리)
```

**전략 2: Fast 메인 실행, Default 정기 백업**
```
SiliconLife.Fast (일상 사용, 실시간 요청 처리)
    ↓ 정기 백업
SiliconLife.Default (콜드 데이터 아카이빙, 데이터 보안 보장)
```

## �📄 라이선스

본 프로젝트는 Apache License 2.0 라이선스를 따릅니다 — 자세한 내용은 [LICENSE](../../LICENSE) 파일을 참조하세요.

## 👨‍💻 저자

**天源垦骥**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- 码云: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- 哔哩哔哩: [617827040](https://space.bilibili.com/617827040)

## 🙏 감사의 말

이 프로젝트에 기여한 모든 개발자와 AI 플랫폼 제공업체에게 감사드립니다.

---

**Silicon Life Collective** — AI 에이전트가 진정으로 "살아있게" 만들다
