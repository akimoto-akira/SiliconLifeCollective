# 개발 가이드

> **버전: v0.2.0-alpha**

[English](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | **한국어** | [Čeština](../cs-CZ/development-guide.md) | [Русский](../ru-RU/development-guide.md)

## 아키텍처 개요

SiliconLifeCollective는 **Body-Brain 아키텍처**를 따르며, 핵심 인터페이스와 기본 구현이 엄격하게 분리되어 있습니다.

### 프로젝트 구조

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # 인터페이스, 추상 클래스, 공통 인프라
│   ├── SiliconLife.Common/          # 공유 구현 (두 버전 모두 사용)
│   ├── SiliconLife.Default/         # 기본 구현, 진입점 (아키텍처 실현 가능성 검증)
│   ├── SiliconLife.Fast/            # 고성능 구현, 진입점 (주력 프로덕션 버전)
│   ├── SiliconLife.Speedy/          # SpeedyPack 고성능 스토리지 엔진
│   └── SiliconLife.Speedy.Manager/  # SpeedyPack 관리 도구 (Avalonia UI)
└── docs/                            # 다국어 문서
```

**의존 방향**:
- `SiliconLife.Default` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Fast` → `SiliconLife.Common` → `SiliconLife.Core`
- `SiliconLife.Common` → `SiliconLife.Core` (단방향)

**버전 역할 설명**:
- **SiliconLife.Default**: 기본 구현, 주로 아키텍처 실현 가능성 검증에 사용됩니다. 단순하고 신뢰성 높은 파일 시스템 스토리지 구현을 제공하며, 개발 디버깅 및 아키텍처 검증에 적합합니다.
- **SiliconLife.Fast**: 주력 프로덕션 버전입니다. Default에서 검증된 아키텍처를 기반으로 SpeedyPack 메모리 스토리지 + 비동기 영속화를 채택하여 극한의 성능 최적화를 제공하며, 장기 운영 및 실제 프로덕션 환경에 적합합니다.

## 핵심 개념

### 1. 실리콘 비잉 (실리콘 비잉)

각 AI 에이전트는 다음으로 구성됩니다:
- **Body** (`DefaultSiliconBeing`): 생존 상태 유지, 트리거 시나리오 감지
- **Brain** (`컨텍스트 매니저`): 기록 로드, AI 호출, 툴 실행, 응답 영속화

### 2. 툴 시스템

툴은 리플렉션을 통해 자동으로 검색 및 등록됩니다:

```csharp
// 모든 툴은 ITool 인터페이스를 구현합니다
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. 퍼미션 시스템

3단계 퍼미션 검증 체인:
```
사용자 빈도 캐시 → 퍼미션 콜백 인터페이스 → (IsCurator: 퍼미션 요청 핸들러 | Non-curator: 글로벌 ACL → 기본 거부)
```

### 4. 서비스 로케이터

전역 서비스 등록 및 검색:
```csharp
// 등록
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// 가져오기
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## 확장 시스템

### 새 툴 추가

1. `src/SiliconLife.Common/Tools/`에 새 클래스를 생성합니다 (두 버전이 공유하는 툴):

> **참고**: `SiliconLife.Default`와 `SiliconLife.Fast`에는 더 이상 독립적인 `Tools/` 디렉토리가 없으며, 모든 공유 툴은 `SiliconLife.Common/Tools/`에 통합되어 있습니다.

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "Description of what this tool does";

    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        // 매개변수 파싱
        var param1 = call.Parameters["param1"]?.ToString();

        // 로직 실행
        var result = await DoSomething(param1);

        // 결과 반환
        return new ToolResult
        {
            Success = true,
            Output = result
        };
    }
}
```

2. 툴은 리플렉션을 통해 자동으로 검색됩니다 - 수동 등록 불필요!

3. (선택사항) 관리자 전용으로 표시:
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

4. (선택사항) 툴 사용 시나리오 표시:
```csharp
[ToolScenario(ToolScenarioFlag.Chat | ToolScenarioFlag.Task)]
public class MyTool : ITool { ... }
```

5. (선택사항) 채팅 시나리오 전용으로 표시:
```csharp
[ChatOnly]
public class HelpTool : ITool { ... }
```

6. (선택사항) 프로젝트 시나리오 전용으로 표시:
```csharp
[ToolScenario(ToolScenarioFlag.Project)]
[SiliconManagerOnly]
public class ProjectWorkTool : ITool { ... }
```

### 새 AI 클라이언트 추가

1. `src/SiliconLife.Common/AI/`에 `IAIClient`를 구현합니다:

```csharp
public class MyAIClient : IAIClient
{
    public string Name => "my_ai";

    public async Task<AIResponse> ChatAsync(AIRequest request)
    {
        // AI API 호출
        var response = await CallMyAPI(request);

        return new AIResponse
        {
            Content = response.Message,
            ToolCalls = response.ToolCalls,
            Usage = response.Usage
        };
    }

    public async IAsyncEnumerable<string> StreamChatAsync(AIRequest request)
    {
        // 스트리밍 구현
        await foreach (var chunk in StreamFromAPI(request))
        {
            yield return chunk;
        }
    }
}
```

2. 팩토리 생성:

```csharp
public class MyAIClientFactory : IAIClientFactory
{
    public IAIClient CreateClient(AIClientConfig config)
    {
        return new MyAIClient(config);
    }
}
```

3. 팩토리는 자동으로 검색 및 등록됩니다.

### 새 스토리지 백엔드 추가

1. `src/SiliconLife.Default/Storage/` (파일 시스템 구현) 또는 `src/SiliconLife.Fast/Storage/` (SpeedyPack 어댑터)에 `IStorage` 및 `ITimeStorage`를 구현합니다:

```csharp
public class DatabaseStorage : IStorage, ITimeStorage
{
    public async Task<string> ReadAsync(string key)
    {
        // 데이터베이스에서 읽기
    }

    public async Task WriteAsync(string key, string value)
    {
        // 데이터베이스에 쓰기
    }

    public async Task<IEnumerable<string>> ReadByTimeAsync(DateTime start, DateTime end)
    {
        // 시간 인덱스 쿼리
    }
}
```

### 새 플러그인 추가

1. 클래스 라이브러리 프로젝트를 생성하고 `IPlugin` 인터페이스를 구현합니다:

```csharp
using SiliconLife.Collective;
using SiliconLife.Collective.Localization;
using SiliconLife.Collective.Tools;

public class MyPlugin : IPlugin
{
    public string Id => "my-plugin";
    public string Version => "1.0.0";

    public string GetName(Language language) => "My Plugin";
    public string GetDescription(Language language) => "A custom plugin";
    public string GetAuthor(Language language) => "Author Name";

    public void OnLoad() { }
    public void OnStart() { }
    public void OnStop() { }
    public void OnUnload() { }
}
```

2. (선택사항) 플러그인에서 `ITool` 인터페이스를 구현하여 커스텀 툴을 등록합니다:

```csharp
public class MyPluginTool : ITool
{
    public string Name => "my_plugin_tool";
    public string Description => "A tool provided by my plugin";

    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        return new ToolResult { Success = true, Output = "Done" };
    }
}
```

3. 컴파일된 DLL을 플러그인 디렉토리에 넣으면 `플러그인 로더`가 자동으로 로드합니다.

> **보안 제한**: 기본적으로 플러그인은 `System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis` 등의 네임스페이스를 참조할 수 없습니다. 단, 플러그인이 `[PluginCapability]` 속성으로 필요한 역량(Network, FileIO, Process, AI)을 선언하면, 로더가 해당 보안 스캔 규칙을 완화합니다. 선언 불가능한 역량(P/Invoke, Unsafe, Reflection Emit 등)은 항상 차단됩니다. 플러그인은 `AssemblyLoadContext`를 통해 격리 로드됩니다.

### 새 스킨 추가

1. `src/SiliconLife.App/Web/Skins/`에 `ISkin`을 구현합니다:

```csharp
public class MyCustomSkin : ISkin
{
    public string Name => "MySkin";
    public string Description => "A custom skin description";

    public string GetCss()
    {
        return @"
            :root {
                --primary-color: #your-color;
                --bg-color: #your-bg;
            }
            /* Your custom styles */
        ";
    }
}
```

2. 스킨은 `SkinManager`에 의해 자동으로 검색됩니다.

## 코드 스타일 가이드

### 명명 규칙

- **클래스**: 기능 접두사가 있는 PascalCase (예: `DefaultSiliconBeing`)
- **인터페이스**: `I`로 시작 (예: `IAIClient`, `ITool`)
- **구현**: 인터페이스 이름으로 끝남 (예: `OllamaClient`는 `IAIClient` 구현)
- **툴**: `Tool`로 끝남 (예: `CalendarTool`, `ChatTool`)
- **뷰 모델**: `ViewModel`로 끝남 (예: `BeingViewModel`)

### 코드 구성

```
SiliconLife.Common/
├── AI/                    # AI 클라이언트 및 팩토리 구현 (Ollama, DashScope, VolcengineArk, DeepSeek, Zhipu, Ernie, Hunyuan, MiniMax, Moonshot, SiliconFlow, Herdsman, LongCat, QiniuAI)
├── Calendar/              # 32가지 캘린더 구현
├── Localization/          # 로컬라이제이션 베이스 클래스 및 34개 언어 변형 구현
├── Security/              # 퍼미션 매니저
├── SiliconBeing/          # 기본 실리콘 비잉 구현
├── Tools/                 # 공유 내장 툴 (25개)
├── Web/                   # 웹 인프라
└── WebView/               # Playwright WebView 구현

SiliconLife.App/          # Default와 Fast가 공유하는 애플리케이션 계층
├── Config/                # 애플리케이션 설정
├── Help/                  # 도움말 문서 로컬라이제이션
├── Project/               # 프로젝트 시스템 (워크플로 엔진, 프로젝트 역할)
└── Web/                   # Web UI 구현
    ├── Component/         # 27개 UI 컴포넌트
    ├── Controllers/       # 24개 라우트 컨트롤러
    ├── Models/            # 뷰 모델
    ├── Views/             # HTML 뷰
    └── Skins/             # 7개 스킨 테마

SiliconLife.Default/      # 버전별 디렉토리
├── Config/                # 기본 설정 데이터
├── Knowledge/             # 노리지 네트워크 구현
├── Logging/               # 로거 프로바이더 구현 (콘솔 + 파일 시스템)
├── Project/               # 프로젝트 시스템 구현
└── Storage/               # 파일 시스템 스토리지 구현

SiliconLife.Fast/         # 버전별 디렉토리
├── Config/                # Fast 버전 설정 데이터
├── Logging/               # 로거 프로바이더 구현 (콘솔 + 파일 시스템)
├── Storage/               # SpeedyPack 스토리지 어댑터
└── Tray/                  # 시스템 트레이 로컬라이제이션
```

### 문서

- 모든 공개 API에는 XML 문서 주석이 필수입니다
- 모든 소스 파일에 Apache 2.0 라이선스 헤더를 사용합니다
- .NET 9 기능을 활용합니다 (암시적 using, nullable 참조 타입)

## 개발 워크플로우

### 1. 개발 환경 설정

```bash
# 저장소 클론
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective

# 종속성 복원
dotnet restore

# 빌드
dotnet build
```

### 2. 테스트 실행

```bash
# 모든 테스트 실행
dotnet test

# 특정 테스트 프로젝트 실행
dotnet test tests/SiliconLife.Core.Tests
```

### 3. 디버그

```bash
# 디버그 출력으로 실행
dotnet run --project src/SiliconLife.Default --configuration Debug
```

### 4. 코드 포맷팅

```bash
# 코드 포맷
dotnet format
```

## 커스텀 기능 빌드

### 예시: 커스텀 캘린더 추가

```csharp
public class MyCustomCalendar : CalendarBase
{
    public override string Name => "MyCalendar";

    public override CalendarDate ConvertFromGregorian(GregorianDate date)
    {
        // 변환 로직
        return new CalendarDate(year, month, day);
    }

    public override GregorianDate ConvertToGregorian(CalendarDate date)
    {
        // 역변환
        return new GregorianDate(year, month, day);
    }
}
```

### 예시: 커스텀 이그제큐터 추가

```csharp
public class CustomExecutor : ExecutorBase
{
    public override string Name => "custom";

    public override async Task<ExecutorResult> ExecuteAsync(ExecutorRequest request)
    {
        var permission = await CheckPermissionAsync(request);
        if (!permission.Allowed)
        {
            return ExecutorResult.Denied(permission.Reason);
        }

        var result = await PerformOperation(request);

        return ExecutorResult.Success(result);
    }
}
```

### 예시: 커스텀 워크플로 템플릿 추가

```csharp
public class MyWorkflowTemplate : WorkflowTemplate
{
    public override string Name => "my_workflow";
    public override string Description => "A custom workflow template";

    public override void DefineStates()
    {
        AddState("start", "시작", isInitial: true);
        AddState("processing", "처리 중");
        AddState("review", "검토");
        AddState("done", "완료", isFinal: true);
    }

    public override void DefineTransitions()
    {
        AddTransition("start", "processing", "처리 시작");
        AddTransition("processing", "review", "검토 제출");
        AddTransition("review", "done", "검토 통과");
        AddTransition("review", "processing", "검토 반려");
    }
}
```

### 예시: 프로젝트 역할 추가

프로젝트 역할은 `ProjectTool`의 `assign_role` 및 `remove_role` 작업을 통해 관리됩니다. 역할 이름은 커스텀 문자열이며, 워크플로 및 태스크 할당에서 실리콘 비잉의 역할을 구분하는 데 사용됩니다.

## 테스트 가이드

### 단위 테스트

```csharp
[TestClass]
public class MyToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccess()
    {
        // Arrange
        var tool = new MyCustomTool();
        var call = new ToolCall
        {
            Name = "my_custom_tool",
            Parameters = new Dictionary<string, object>
            {
                ["param1"] = "test"
            }
        };

        // Act
        var result = await tool.ExecuteAsync(call);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Output);
    }
}
```

### 통합 테스트

전체 흐름 테스트:
1. AI가 툴 콜을 반환
2. 툴 실행
3. 결과를 AI에 피드백
4. AI가 최종 응답 반환

## 성능 고려사항

### 스토리지 시스템

- Default 버전은 파일 기반 JSON 스토리지를 사용합니다
- Fast 버전은 SpeedyPack 메모리 스토리지 엔진을 사용합니다 (.spk 형식)
- SpeedyPack은 메모리 디렉토리 맵 + 엔트리 캐시 + 비동기 쓰기 큐를 채택합니다
- 시간 인덱스 쿼리는 `ITimeStorage` 인터페이스를 사용합니다

### 메인 루프 스케줄러

- 클럭 기반 시분할 공정 스케줄링
- 멈춘 작업 감지를 위한 워치독 타이머
- 연쇄 실패 방지를 위한 서킷 브레이커

## 모범 사례

### 1. 항상 퍼미션 검증

AI가 시작한 모든 작업은 퍼미션 체인을 통과해야 합니다:

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return Result.Denied("Permission denied");
}
```

### 2. 서비스 로케이터 사용

전역 서비스 등록 및 검색:

```csharp
// 초기화 중
ServiceLocator.Instance.Register<ICustomService>(myService);

// 필요 시
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. Body-Brain 분리 준수

- Body는 상태 및 트리거를 처리합니다
- Brain은 AI 상호작용 및 툴 실행을 처리합니다

### 4. 적절한 오류 처리 구현

```csharp
try
{
    var result = await operation();
    return Result.Success(result);
}
catch (Exception ex)
{
    Logger.Error($"Operation failed: {ex.Message}");
    return Result.Failure(ex.Message);
}
```

## 기여 가이드

1. 저장소를 포크합니다
2. 기능 브랜치를 생성합니다 (`git checkout -b feature/amazing-feature`)
3. Conventional Commits를 사용하여 변경사항을 커밋합니다
4. 브랜치에 푸시합니다 (`git push origin feature/amazing-feature`)
5. Pull Request를 생성합니다

### 커밋 메시지 형식

```
<type>(<scope>): <description>

예시:
feat(tool): add custom calendar tool
fix(permission): fix null pointer in callback
docs: update development guide
```

## 다음 단계

- 📚 [아키텍처 가이드](architecture.md) 읽기
- 📖 [API 레퍼런스](api-reference.md) 탐색
- 🔒 [보안 문서](security.md) 검토
- 🚀 [시작 가이드](getting-started.md) 확인
