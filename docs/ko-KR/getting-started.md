# 시작 가이드

[English](getting-started.md) | [简体中文](docs/zh-CN/getting-started.md) | [繁體中文](docs/zh-HK/getting-started.md) | [Español](docs/es-ES/getting-started.md) | [日本語](docs/ja-JP/getting-started.md) | [한국어](docs/ko-KR/getting-started.md) | [Čeština](docs/cs-CZ/getting-started.md)

## 필수 조건

- **.NET 9 SDK** - [다운로드](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [다운로드](https://git-scm.com/)
- **Ollama** (선택사항, 로컬 AI용) - [다운로드](https://ollama.com/)
- **DashScope API 키** (선택사항, 클라우드 AI용) - [신청](https://bailian.console.aliyun.com/)

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

### 3. AI 백엔드 설정

`src/SiliconLife.Default/Config/DefaultConfigData.cs`를 편집하거나 Web UI를 통해 런타임에 설정을 수정합니다.

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

#### 옵션 B: DashScope (클라우드)

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

브라우저를 열고 다음으로 이동:

```
http://localhost:8080
```

다음 내용을 볼 수 있습니다:
- 실리콘 비잉 관리
- 채팅 인터페이스
- 설정 패널
- 시스템 모니터링

## 첫 번째 실리콘 비잉

### 첫 번째 Being 생성

1. Web UI에서 **Being 관리**로 이동
2. **새 Being 생성** 클릭
3. 성격과 행동이 포함된 소울 파일(`soul.md`) 설정
4. Being 시작

### soul.md 예시

```markdown
# 나의 첫 번째 실리콘 비잉

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

## 일반적인 문제

### Ollama 연결 거부

**문제**: `http://localhost:11434`에서 Ollama에 연결할 수 없음

**해결책**:
```bash
# Ollama 실행 확인
ollama list

# 필요 시 Ollama 시작
ollama serve
```

### 모델을 찾을 수 없음

**문제**: `model "qwen2.5:7b" not found`

**해결책**:
```bash
# 필요한 모델 풀
ollama pull qwen2.5:7b
```

### 포트가 이미 사용 중

**문제**: `HttpListenerException: Address already in use`

**해결책**:
- 설정에서 포트 변경
- 또는 포트 8080을 사용하는 프로세스 종료:

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## 다음 단계

- 📚 시스템 설계를 이해하려면 [아키텍처 가이드](architecture.md) 읽기
- 🛠️ 시스템 확장을 위해 [개발 가이드](development-guide.md) 확인
- 📖 통합 세부정보는 [API 레퍼런스](api-reference.md) 탐색
- 🔒 권한 시스템은 [보안 문서](security.md) 검토

## 프로젝트 구조

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/      # 핵심 인터페이스 및 추상화
│   └── SiliconLife.Default/   # 기본 구현 + 진입점
├── docs/                      # 문서 (다국어)
│   ├── en/                    # 영어
│   ├── zh-CN/                 # 중국어 간체
│   ├── zh-HK/                 # 중국어 번체
│   ├── ja-JP/                 # 일본어
│   └── ko-KR/                 # 한국어
└── README.md                  # 이 파일
```

## 도움이 필요하신가요?

- 📖 [문서](docs/) 확인
- 🐛 GitHub에서 이슈 보고
- 💬 커뮤니티 토론 참여
