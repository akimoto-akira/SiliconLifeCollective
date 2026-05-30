# 빠른 시작

> **버전: v0.2.0-alpha**

[English](../en/getting-started.md) | [Deutsch](../de-DE/getting-started.md) | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | [日本語](../ja-JP/getting-started.md) | **한국어** | [Čeština](../cs-CZ/getting-started.md) | [Русский](../ru-RU/getting-started.md)

## 버전 선택

이 프로젝트는 두 가지 구현 버전을 제공합니다:

### SiliconLife.Default (기본 버전)
- **포지션**: 기본 구현, 주로 아키텍처 타당성 검증에 사용
- **실행 모드**: 콘솔 애플리케이션
- **저장 방식**: 파일 시스템 JSON 저장
- **적용 시나리오**: 데이터 보안 우선, 소규모 데이터, 개발 디버깅, 아키텍처 검증
- **플랫폼 지원**: Windows, Linux, macOS
- **역할 설명**: 아키텍처 검증의 기준 구현으로, 간단하고 안정적인 실행 방식을 제공하며, 처음 접하는 사용자나 개발 디버깅에 적합

### SiliconLife.Fast (고성능 버전)
- **포지션**: 주력 프로덕션 버전
- **실행 모드**: 데스크톱 애플리케이션 (Windows/macOS 시스템 트레이 / Linux 상태 창)
- **저장 방식**: SpeedyPack 메모리 저장 + 비동기 영속화 (.spk 파일 형식)
- **적용 시나리오**: 고동시성, 저지연, 대규모 데이터, 장기 프로덕션 운영
- **플랫폼 지원**: Windows/macOS (시스템 트레이 포함 전체 기능), Linux (상태 창, 트레이 아이콘 없음)
- **역할 설명**: 심층 최적화된 프로덕션급 구현으로, 장기 실행 및 실제 프로덕션 환경에 최적의 선택

> **초보자 권장**: 처음 사용 시 **SiliconLife.Default**부터 시작하여 아키텍처 타당성을 빠르게 검증하는 것을 권장합니다. 시스템에 익숙해진 후, 프로덕션 환경 실행 버전으로 **SiliconLife.Fast**로 마이그레이션할 것을 강력히 권장합니다.

## 사전 조건

- **.NET 9 SDK** - [다운로드](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [다운로드](https://git-scm.com/)
- **Ollama** (선택, 로컬 AI용) - [다운로드](https://ollama.com/)
- **바이리안 API 키** (선택, 클라우드 AI용) - [신청](https://bailian.console.aliyun.com/)
- **Volcano Engine Ark API 키** (선택, 클라우드 AI용) - [신청](https://console.volcengine.com/ark)
- **Herdsman** (선택, 로컬/클라우드 추론 엔진) - 인증 없음, OpenAI API 형식 호환
- **Meituan LongCat API 키** (선택, 클라우드 AI용) - API 키 인증
- **Qiniu Cloud AI API 키** (선택, 클라우드 AI용) - API 키 인증

## 빠른 시작

### 1. 저장소 클론

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. 프로젝트 빌드

```bash
dotnet build
```

### 3. AI 백엔드 구성

`src/SiliconLife.Default/Config/DefaultConfigData.cs`를 편집하거나 Web UI를 통해 런타임에 구성을 수정합니다.

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

#### 옵션 B: 바이리안 (클라우드)

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "your-api-key-here",
      "Model": "qwen-plus",
      "Region": "beijing"
    }
  }
}
```

> **사용 가능한 리전**: `beijing` (베이징), `virginia` (버지니아), `singapore` (싱가포르), `hongkong` (홍콩), `frankfurt` (프랑크푸르트)

#### 옵션 C: Volcano Engine Ark (클라우드)

```json
{
  "AIClients": {
    "VolcengineArk": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://ark.cn-beijing.volces.com/api/v3/chat/completions",
      "Model": "ep-xxxxxxxxxxxxx-xxxxx"
    }
  }
}
```

> **참고**: Volcano Engine Ark의 Model 매개변수는 모델 이름이 아닌 추론 엔드포인트 ID(예: `ep-20241212123456-abcde`)를 받습니다.

#### 옵션 D: Herdsman (로컬/클라우드)

```json
{
  "AIClients": {
    "Herdsman": {
      "Endpoint": "http://localhost:8000",
      "Model": "모델-이름"
    }
  }
}
```

> **특징**: 인증 없음, OpenAI API 형식 호환, 툴 콜 및 추론 콘텐츠 지원.

#### 옵션 E: Meituan LongCat (클라우드)

```json
{
  "AIClients": {
    "LongCat": {
      "ApiKey": "API-키",
      "Endpoint": "https://api.longcat.ai/v1/chat/completions",
      "Model": "모델-이름"
    }
  }
}
```

#### 옵션 F: Qiniu Cloud AI (클라우드)

```json
{
  "AIClients": {
    "QiniuAI": {
      "ApiKey": "API-키",
      "Endpoint": "https://api.qiniu.com/v1/chat/completions",
      "Model": "모델-이름"
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

웹 서버가 `http://localhost:8080`에서 시작됩니다

#### Fast 버전 실행

```bash
cd src/SiliconLife.Fast
dotnet run
```

**Windows/macOS**: 애플리케이션이 창 모드로 시작되어 시스템 트레이로 최소화되며, 웹 서버 역시 `http://localhost:8080`에서 시작됩니다

**Linux**: 애플리케이션이 상태 창을 표시하며(시스템 트레이 아이콘 없음), 자동으로 브라우저를 열어 Web UI에 접속합니다. `--no-tray` 매개변수를 사용하여 브라우저 자동 열기를 건너뛸 수도 있습니다:

```bash
dotnet run -- --no-tray
```

### 5. Web UI 접속

브라우저를 열고 다음 주소로 이동합니다:

```
http://localhost:8080
```

다음 내용이 포함된 대시보드를 볼 수 있습니다:
- 실리콘 비잉 관리
- 채팅 인터페이스
- 구성 패널
- 시스템 모니터링

## 첫 번째 실리콘 비잉

### 첫 번째 비잉 만들기

1. Web UI에서 **비잉 관리**로 이동합니다
2. **새 비잉 만들기**를 클릭합니다
3. 성격과 행동이 포함된 소울 파일(`soul.md`)을 구성합니다
4. 비잉을 시작합니다

### soul.md 예시

```markdown
# My First Silicon Being

## Personality
You are a helpful assistant specializing in code review.

## Capabilities
- Review code quality
- Suggest improvements
- Explain complex concepts

## Behavior
- Always provide constructive feedback
- Use clear examples
- Be concise but thorough
```

## 자주 묻는 질문

### Ollama 연결 거부

**문제**: `http://localhost:11434`의 Ollama에 연결할 수 없음

**해결 방법**:
```bash
# Ollama가 실행 중인지 확인
ollama list

# Ollama 시작이 필요한 경우
ollama serve
```

### 모델을 찾을 수 없음

**문제**: `model "qwen2.5:7b" not found`

**해결 방법**:
```bash
# 필요한 모델 다운로드
ollama pull qwen2.5:7b
```

### 포트가 이미 사용 중임

**문제**: `HttpListenerException: Address already in use`

**해결 방법**:
- 구성에서 포트 변경
- 또는 포트 8080을 사용하는 프로세스 종료:

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## 다음 단계

- 📚 [아키텍처 가이드](architecture.md)를 읽고 시스템 설계 이해하기
- 🛠️ [개발 가이드](development-guide.md)를 확인하고 시스템 확장하기
- 📖 [API 레퍼런스](api-reference.md)를 탐색하고 통합 세부사항 알아보기
- 🔒 [보안 문서](security.md)를 확인하고 권한 시스템 이해하기
- 🧰 [툴 레퍼런스](tools-reference.md)를 확인하고 모든 내장 툴 알아보기
- 🌐 [Web UI 가이드](web-ui-guide.md)를 확인하고 인터페이스 기능 알아보기

## 프로젝트 구조

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # 코어 인터페이스 및 추상 클래스
│   ├── SiliconLife.Common/          # 공유 구현 (두 버전 공통)
│   ├── SiliconLife.App/             # Default와 Fast 공유 애플리케이션 계층
│   ├── SiliconLife.Default/         # 기본 구현 + 진입점 (콘솔 버전)
│   ├── SiliconLife.Fast/            # 고성능 구현 + 진입점 (창 버전)
│   ├── SiliconLife.Speedy/          # SpeedyPack 고성능 스토리지 엔진
│   └── SiliconLife.Speedy.Manager/  # SpeedyPack 매니저 (Avalonia UI)
├── docs/                            # 문서 (다국어, 34개 언어 변형)
│   ├── en/                          # 영어
│   ├── zh-CN/                       # 간체 중국어
│   ├── zh-HK/                       # 번체 중국어
│   ├── es-ES/                       # 스페인어
│   ├── ja-JP/                       # 일본어
│   ├── ko-KR/                       # 한국어
│   └── cs-CZ/                       # 체코어
├── 总文档/                           # 요구사항 및 아키텍처 문서 (중국어)
└── README.md                        # 프로젝트 설명
```

## 도움이 필요하신가요?

- 📖 [도움말 문서 시스템](web-ui-guide.md#帮助文档系统新增) 확인 (다국어 지원)
- 📚 [전체 문서](docs/) 읽기
- 🐛 [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues)에 문제 보고
- 💬 커뮤니티 토론 참여
