# 빠른 시작

> **버전: v0.1.0-alpha**

[English](../en/getting-started.md) | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | **한국어** | [Deutsch](../de-DE/getting-started.md) | [Čeština](../cs-CZ/getting-started.md)

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

```bash
cd src/SiliconLife.Default
dotnet run
```

웹 서버가 `http://localhost:8080`에서 시작됩니다.

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

## 다음 단계

- 📚 [아키텍처 가이드](architecture.md) 읽기
- 🛠️ [도구 참고](tools-reference.md) 탐색
- 🔒 [보안 모델](security.md) 이해
- 🎨 [Web UI 가이드](web-ui-guide.md) 학습
- 🤖 [실리콘 생명체 가이드](silicon-being-guide.md)로 생명체 만들기

## 도움이 필요하신가요?

- [문제 해결](troubleshooting.md) 가이드 확인
- [기여 가이드](contributing.md) 읽기
- GitHub에서 [이슈 제출](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
