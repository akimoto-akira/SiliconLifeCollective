# Silicon Life Collective

**버전: v0.1.0-alpha** | **실리콘 생명군** — .NET 9 기반의 멀티 에이전트 협업 플랫폼으로, AI 에이전트를 **실리콘 생명체(Silicon Being)**라고 부르며, Roslyn 동적 컴파일을 통해 자가 진화를 구현합니다.

[English](../README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | **한국어** | [Deutsch](../de-DE/README.md) | [Čeština](../cs-CZ/README.md)

## 🌟 핵심 기능

### 에이전트 시스템
- **멀티 에이전트 오케스트레이션** — *실리콘 큐레이터*가 통합 관리하며, 클록 기반 타임슬롯 공정 스케줄링 메커니즘 사용
- **소울 파일 기반** — 각 실리콘 생명체는 핵심 프롬프트 파일(`soul.md`)에 의해 구동되며, 독특한 개성과 행동 패턴을 정의
- **신체-두뇌 아키텍처** — *신체*(SiliconBeing)는 생명 상태를 유지하고 트리거 시나리오를 감지; *두뇌*(ContextManager)는 기록 로드, AI 호출, 도구 실행 및 응답 영속화 담당
- **자가 진화 능력** — Roslyn 동적 컴파일 기술을 통해 실리콘 생명체가 자신의 코드를 재작성하여 진화 가능

### 도구 및 실행
- **23개 내장 도구** — 달력, 채팅, 설정, 디스크, 네트워크, 메모리, 작업, 타이머, 지식베이스, 작업 노트, WebView 브라우저 등 포괄
- **도구 호출 루프** — AI가 도구 호출 반환 → 도구 실행 → 결과를 AI에 피드백 → 순수 텍스트 응답 반환까지 지속 루프
- **실행기-권한 보안** — 모든 I/O 작업은 실행기를 통해 엄격한 권한 검증 수행
  - 5단계 권한 체인: IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - 모든 권한 결정을 완전한 감사 로그로 기록

### AI 및 지식
- **다중 AI 백엔드 지원**
  - **Ollama** — 로컬 모델 배포, 네이티브 HTTP API 사용
  - **알리바바 클라우드 Bailian(DashScope)** — 클라우드 AI 서비스, OpenAI API 호환, 13개 이상 모델 지원, 다중 지역 배포
- **32가지 달력 시스템** — 전 세계 주요 달력 완전 커버, 양력, 음력, 이슬람력, 히브리력, 일본력, 페르시아력, 마야력, 중국 역사력 등 포함
- **지식 네트워크 시스템** — 트리플(주어-관계-목적어) 기반 지식 그래프, 저장, 쿼리 및 경로 발견 지원

### 웹 인터페이스
- **모던 Web UI** — 내장 HTTP 서버, SSE 실시간 업데이트 지원
- **4가지 스킨 테마** — 관리자 버전, 채팅 버전, 창작 버전, 개발 버전, 자동 감지 및 전환 지원
- **20개 이상 컨트롤러** — 완전한 시스템 관리, 채팅, 설정, 모니터링 기능
- **프론트엔드 프레임워크 의존성 제로** — `H`, `CssBuilder`, `JsBuilder`를 통해 서버에서 HTML/CSS/JS 생성

### 국제화 및 지역화
- **21개 언어 변형** 전면 지원
  - 중국어: zh-CN, zh-HK, zh-SG, zh-MO, zh-TW, zhMY (6개)
  - 영어: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10개)
  - 스페인어: es-ES, es-MX (2개)
  - 일본어: ja-JP | 한국어: ko-KR | 체코어: cs-CZ

### 데이터 및 스토리지
- **데이터베이스 의존성 제로** — 순수 파일 시스템 스토리지 (JSON 형식)
- **시간 인덱스 쿼리** — `ITimeStorage` 인터페이스를 통해 시간 범위별 효율적 쿼리 지원
- **최소 의존성** — 핵심 라이브러리는 동적 컴파일을 위한 Microsoft.CodeAnalysis.CSharp만 의존

## 🛠️ 기술 스택

| 구성 요소 | 기술 |
|------|------|
| 런타임 | .NET 9 |
| 프로그래밍 언어 | C# |
| AI 통합 | Ollama (로컬), 알리바바 클라우드 Bailian (클라우드) |
| 데이터 스토리지 | 파일 시스템 (JSON + 시간 인덱스 디렉토리) |
| 웹 서버 | HttpListener (.NET 내장) |
| 동적 컴파일 | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| 브라우저 자동화 | Playwright (WebView) |
| 라이선스 | Apache-2.0 |

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
│   └── SiliconLife.Default/               # 기본 구현 + 애플리케이션 진입점
│       ├── Program.cs                     # 진입점 (모든 구성 요소 조립)
│       ├── AI/                            # Ollama 클라이언트, Bailian 클라이언트
│       ├── Calendar/                      # 32가지 달력 구현
│       ├── Config/                        # 기본 설정 데이터
│       ├── Executors/                     # 기본 실행기 구현
│       ├── Help/                          # 도움말 문서 시스템
│       ├── IM/                            # WebUI 제공자
│       ├── Knowledge/                     # 지식 네트워크 구현
│       ├── Localization/                  # 21개 언어 지역화
│       ├── Logging/                       # 로깅 제공자 구현
│       ├── Project/                       # 프로젝트 시스템 구현
│       ├── Runtime/                       # 테스트 클록 객체
│       ├── Security/                      # 기본 권한 콜백
│       ├── SiliconBeing/                  # 기본 실리콘 생명체 구현
│       ├── Storage/                       # 파일 시스템 스토리지 구현
│       ├── Tools/                         # 23개 내장 도구 구현
│       ├── WebView/                       # Playwright WebView 구현
│       └── Web/                           # Web UI 구현
│           ├── Controllers/               # 20개 이상 컨트롤러
│           ├── Models/                    # 뷰 모델
│           ├── Views/                     # HTML 뷰
│           └── Skins/                     # 4가지 스킨 테마
│
├── docs/                                  # 다국어 문서
│   ├── zh-CN/                             # 중국어 간체 문서
│   ├── en/                                # 영어 문서
│   └── ...                                # 기타 언어 문서
│
└── 总文档/                                 # 요구사항 문서 및 아키텍처 문서
    ├── 需求文档.md
    ├── 架构大纲.md
    └── 实现顺序.md
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
도구 호출 → 실행기 → 권한 관리자 → [IsCurator → 주파수 캐시 → 글로벌 ACL → 콜백 → 사용자에게 문의]
```

## 🚀 빠른 시작

### 사전 요구사항

- **.NET 9 SDK** — [다운로드 링크](https://dotnet.microsoft.com/download/dotnet/9.0)
- **AI 백엔드** (둘 중 하나 선택):
  - **Ollama**: [Ollama 설치](https://ollama.com) 및 모델 풀 (예: `ollama pull llama3`)
  - **알리바바 클라우드 Bailian**: [Bailian 콘솔](https://bailian.console.aliyun.com/)에서 API 키 획득

### 프로젝트 빌드

```bash
dotnet restore
dotnet build
```

### 시스템 실행

```bash
dotnet run --project src/SiliconLife.Default
```

애플리케이션이 웹 서버를 시작하고 자동으로 브라우저에서 Web UI를 엽니다.

### 단일 파일 게시

```bash
# Windows
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 개발 로드맵

### ✅ 완료됨
- [x] 단계 1: 콘솔 AI 채팅
- [x] 단계 2: 프레임워크 스켈레톤 (메인 루프 + 클록 객체 + 워치독 + 서킷 브레이커)
- [x] 단계 3: 첫 번째 소울 파일 포함 실리콘 생명체 (신체-두뇌 아키텍처)
- [x] 단계 4: 영속성 메모리 (채팅 시스템 + 시간 스토리지 인터페이스)
- [x] 단계 5: 도구 시스템 + 실행기
- [x] 단계 6: 권한 시스템 (5단계 체인, 감사 로거, 글로벌 액세스 제어 목록)
- [x] 단계 7: 동적 컴파일 + 자가 진화 (Roslyn)
- [x] 단계 8: 장기 메모리 + 작업 + 타이머
- [x] 단계 9: 핵심 호스트 + 멀티 에이전트 협업
- [x] 단계 10: Web UI (HTTP + SSE, 20개 이상 컨트롤러, 4가지 스킨)
- [x] 단계 10.5: 점진적 향상 (브로드캐스트 채널, Token 감사, 32개 달력, 도구 향상, 21개 언어 지역화)
- [x] 단계 10.6: 완성 및 최적화 (WebView, 도움말 시스템, 프로젝트 워크스페이스, 지식 네트워크)

### 🚧 계획 중
- [ ] 단계 11: 외부 인스턴트 메시징 통합 (Feishu / WhatsApp / Telegram)
- [ ] 단계 12: 플러그인 시스템 및 스킬 생태계

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

## 📄 라이선스

이 프로젝트는 Apache License 2.0 라이선스를 따릅니다 — 자세한 내용은 [LICENSE](../../LICENSE) 파일을 참조하세요.

## 👨‍💻 저자

**Hoshino Kennji**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- Gitee: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili: [617827040](https://space.bilibili.com/617827040)

## 🙏 감사의 말

이 프로젝트에 기여한 모든 개발자와 AI 플랫폼 제공업체에게 감사드립니다.

---

**Silicon Life Collective** — AI 에이전트가 진정으로 "살아있게" 만들다
