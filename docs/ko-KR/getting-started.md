# 빠른 시작

> **버전: v0.1.0-alpha**

[English](../en/getting-started.md) | [Deutsch](../de-DE/getting-started.md) | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | **한국어** | [Čeština](../cs-CZ/getting-started.md)

## 버전 선택

이 프로젝트는 두 구현 버전을 제공합니다:

### SiliconLife.Default (기본 버전)
- **포지셔닝**: 기본 구현, 아키텍처 실현 가능성 검증에 주로 사용
- **실행 모드**: 콘솔 애플리케이션
- **저장소**: 파일 시스템 JSON 저장소
- **적용 시나리오**: 데이터 보안 우선, 소량 데이터, 개발 디버깅, 아키텍처 검증
- **플랫폼 지원**: Windows, Linux, macOS
- **역할 설명**: 아키텍처 검증의 기준 구현으로, 단순하고 신뢰성 높은 실행 방식을 제공하며, 이 프로젝트에 처음 접촉하거나 개발 디버깅에 적합합니다

### SiliconLife.Fast (고성능 버전)
- **포지셔닝**: 주력 프로덕션 버전
- **실행 모드**: Windows 양식 애플리케이션 (시스템 트레이)
- **저장소**: SpeedyPack 메모리 스토리지 + 비동기 영속성 (.spk 파일 형식)
- **적용 시나리오**: 높은 동시성, 낮은 지연 시간, 대용량 데이터, 장기 프로덕션 운영
- **플랫폼 지원**: Windows 전용
- **역할 설명**: 심층 최적화가 적용된 프로덕션급 구현으로, 장기 운영 및 실제 프로덕션 환경의 최선의 선택

> **초보자 권장**: 처음 사용자는 **SiliconLife.Default** 부터 시작하여 아키텍처 실현 가능성을 빠르게 검증하세요. 시스템에 익숙해진 후에는 **SiliconLife.Fast**로 마이그레이션하는 것을 강력히 권장합니다.

## 사전 요구사항

- **.NET 9 SDK** - [다운로드](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [다운로드](https://git-scm.com/)
- **Ollama** (선택 사항, 로컬 AI용) - [다운로드](https://ollama.com/)
- **Bailian API 키** (선택 사항, 클라우드 AI용) - [신청](https://bailian.console.aliyun.com/)

## 빠른 시작

### 1. 저장소 복제

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. 프로젝트 빌드

```bash
dotnet build
```

### 3. AI 백엔드 설정

`src/SiliconLife.Default/Config/DefaultConfigData.cs` 파일을 수정하거나 Web UI를 통해 런타임에 설정을 변경하세요.

#### 옵션 A: Ollama (로컬)

```json
{
  "AIClients": {
    "Ollama": {
      "BaseUrl": "http://localhost:11434",
      "Model": "qwen2.5:7b"
    }
  }
}
```

#### 옵션 B: Bailian (클라우드)

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "your-api-key-here",
      "Model": "qwen-plus",
      "Region": "cn-hangzhou"
    }
  }
}
```

### 4. 애플리케이션 실행

#### Default 버전 실행

```bash
cd src/SiliconLife.Default
dotnet run
```

웹 서버가 `http://localhost:8080`에서 시작됩니다.

#### Fast 버전 실행 (Windows 전용)

```bash
cd src/SiliconLife.Fast
dotnet run
```

애플리케이션이 양식 모드로 시작되어 시스템 트레이로 최소화되며, 웹 서버도 `http://localhost:8080`에서 시작됩니다.

### 5. Web UI 접속

브라우저를 열고 다음 주소로 이동하세요:

```
http://localhost:8080
```

다음 내용을 포함한 대시보드가 표시됩니다:
- 실리콘 생명체 관리
- 채팅 인터페이스
- 설정 패널
- 시스템 모니터링

## 첫 번째 실리콘 생명체

### 첫 번째 생명체 생성

1. Web UI에서 **생명체 관리**로 이동
2. **새 생명체 생성** 클릭
3. 개성과 행동을 정의하는 소울 파일(`soul.md`) 설정
4. 생명체 시작

### soul.md 예시

```markdown
# My First Silicon Being

## Personality
You are a helpful assistant specializing in code review.

## Capabilities
- Review code quality
- Suggest improvements
- Follow best practices
```

### 생명체 시작

생명체가 생성되면:
- 독립적인 스케줄링 타임슬롯을 할당받습니다
- 고유한 데이터 디렉토리를 가집니다 (`data/beings/{id}/`)
- AI 요청에 소울 파일이 자동으로 포함됩니다
- 시스템 내에서 독립적으로 실행됩니다

## 주요 기능 탐색

### 채팅 시스템

생명체와 직접 채팅:
1. Web UI에서 **채팅**으로 이동
2. 대화할 생명체 선택
3. 메시지 입력 및 전송
4. AI 응답 및 도구 실행 확인

### 도구 시스템

생명체는 다양한 작업을 수행하기 위해 23개의 내장 도구에 접근할 수 있습니다:

- **캘린더 도구** - 32가지 달력 시스템 지원
- **채팅 도구** - 생명체 간 통신
- **설정 도구** - 설정 읽기/쓰기
- **디스크 도구** - 파일 작업
- **네트워크 도구** - HTTP 요청
- **메모리 도구** - 장기/단기 메모리 관리
- **작업 도구** - 작업 추적
- **타이머 도구** - 예약 작업
- **지식 도구** - 지식 그래프 관리
- **작업 노트 도구** - 작업 기록
- **WebView 도구** - 브라우저 자동화

### 권한 시스템

모든 AI 시작 작업은 5단계 권한 체인을 통과합니다:

1. **IsCurator** - 큐레이터 권한 확인
2. **사용자 빈도 캐시** - 자주 사용하는 결정 캐싱
3. **글로벌 ACL** - 전역 액세스 제어 목록
4. **권한 콜백** - 커스텀 권한 로직
5. **사용자에게 문의** - 사용자 확인 필요

### 지식 네트워크

생명체는 지식을 트리플(주어-관계-목적어)로 저장하고 쿼리할 수 있습니다:

```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95
}
```

## 설정 커스터마이징

### 포트 변경

`DefaultConfigData.cs`에서 HTTP 포트 수정:

```csharp
public int HttpPort = 8080; // 원하는 포트로 변경
```

### 로그 레벨 설정

디버깅을 위해 상세 로깅 활성화:

```csharp
public LogLevel LogLevel = LogLevel.Debug;
```

### AI 모델 전환

런타임에 다른 AI 모델로 전환:

1. Web UI에서 **설정**으로 이동
2. **AI 클라이언트** 섹션 찾기
3. 모델 이름 수정
4. 변경사항 저장

## 문제 해결

### 애플리케이션이 시작되지 않음

- .NET 9 SDK가 설치되어 있는지 확인: `dotnet --version`
- 포트 8080이 사용 중이지 않은지 확인
- 설정 파일 구문 오류 확인

### AI 연결 실패

- Ollama: `ollama list`로 모델이 풀되어 있는지 확인
- Bailian: API 키가 올바른지 확인
- 인터넷 연결 상태 확인

### Web UI에 접속할 수 없음

- 서버가 실행 중인지 확인: `http://localhost:8080`
- 브라우저 콘솔에서 JavaScript 오류 확인
- 방화벽 설정 확인

## 프로젝트 구조

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # 핵심 인터페이스 및 추상 클래스
│   ├── SiliconLife.Common/          # 공유 구현 (두 버전 모두 사용)
│   ├── SiliconLife.Default/         # 기본 구현 + 진입점 (콘솔 버전)
│   ├── SiliconLife.Fast/            # 고성능 구현 + 진입점 (폼 버전)
│   ├── SiliconLife.Speedy/          # SpeedyPack 고성능 스토리지 엔진
│   └── SiliconLife.Speedy.Manager/  # SpeedyPack 관리 도구 (WPF)
├── docs/                            # 문서 (다국어, 21개 언어 변형)
│   ├── en/                          # 영어
│   ├── zh-CN/                       # 간체 중국어
│   ├── zh-HK/                       # 번체 중국어
│   ├── es-ES/                       # 스페인어
│   ├── ja-JP/                       # 일본어
│   ├── ko-KR/                       # 한국어
│   └── cs-CZ/                       # 체코어
├── 总文档/                           # 요구 사항 및 아키텍처 문서 (중국어)
└── README.md                        # 프로젝트 설명
```

## 다음 단계

- 📚 [아키텍처 가이드](architecture.md) 읽어 시스템 설계 이해
- 🛠️ [개발 가이드](development-guide.md) 확인하여 시스템 확장
- 📖 [API 레퍼런스](api-reference.md) 탐색하여 통합 세부 정보 파악
- 🔒 [보안 문서](security.md) 확인하여 권한 시스템 이해
- 🧰 [도구 참고](tools-reference.md) 확인하여 모든 내장 도구 파악
- 🌐 [Web UI 가이드](web-ui-guide.md) 확인하여 인터페이스 기능 학습

## 도움이 필요하신가요?

- 📖 [도움말 문서 시스템](web-ui-guide.md#도움말-문서-시스템) 확인 (다국어 지원)
- 📚 [전체 문서](docs/) 읽기
- 🐛 [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)에서 문제 보고
- 💬 커뮤니티 토론 참여
