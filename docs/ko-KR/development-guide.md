# 개발 가이드

> **버전: v0.2.0-alpha**

[English](../en/development-guide.md) | [Deutsch](../de-DE/development-guide.md) | [中文](../zh-CN/development-guide.md) | [繁體中文](../zh-HK/development-guide.md) | [Español](../es-ES/development-guide.md) | [日本語](../ja-JP/development-guide.md) | **한국어** | [Čeština](../cs-CZ/development-guide.md)

## 아키텍처 개요

SiliconLifeCollective는 핵심 인터페이스와 기본 구현 간 엄격한 분리를 갖춘 **Body-Brain 아키텍처**를 따릅니다.

### 프로젝트 구조

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # 인터페이스, 추상화, 공통 인프라
│   ├── SiliconLife.Common/          # 공유 구현 (두 버전 모두 사용)
│   ├── SiliconLife.Default/         # 기본 구현, 진입점 (아키텍처 실현 가능성 검증)
│   ├── SiliconLife.Fast/            # 고성능 구현, 진입점 (주력 프로덕션 버전)
│   ├── SiliconLife.Speedy/          # SpeedyPack 고성능 스토리지 엔진
│   └── SiliconLife.Speedy.Manager/  # SpeedyPack 관리 도구 (Avalonia)
└── docs/                            # 다국어 문서
```

**의존 방향**:
- `SiliconLife.Default` → `SiliconLife.Core` (단방향)
- `SiliconLife.Fast` → `SiliconLife.Core` (단방향)
- `SiliconLife.Common` → `SiliconLife.Core` (단방향)

**버전 역할 설명**:
- **SiliconLife.Default**: 기본 구현, 아키텍처 실현 가능성 검증에 주로 사용. 단순하고 신뢰성 높은 파일 시스템 저장소 구현을 제공하며, 개발 디버깅과 아키텍처 검증에 적합합니다.
- **SiliconLife.Fast**: 주력 프로덕션 버전. Default에서 검증된 아키텍처 기반 위에 SpeedyPack 메모리 스토리지 + 비동기 영속성을 채택하여 극한의 성능 최적화를 제공합니다. 장기 운영 및 실제 프로덕션 환경의 최선의 선택입니다.

## 핵심 개념

### 1. 실리콘 비잉 (Silicon Being)

각 AI 에이전트는 다음으로 구성됩니다:
- **Body** (`DefaultSiliconBeing`): 생존 상태 유지, 트리거 시나리오 감지
- **Brain** (`ContextManager`): 기록 로드, AI 호출, 도구 실행, 응답 저장

### 2. 도구 시스템

도구는 리플렉션을 통해 자동으로 검색 및 등록됩니다:

```csharp
// 모든 도구는 ITool 인터페이스 구현
public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### 3. 권한 시스템

3단계 분기 권한 검증 체인:
```
UserFrequencyCache → IPermissionCallback → IsCurator 분기 (큐레이터: AskHandler / 비큐레이터: GlobalACL → 기본 거부)
```

### 4. 서비스 로케이터

전역 서비스 등록 및 검색:
```csharp
// 등록
ServiceLocator.Instance.Register<IAIClient>(ollamaClient);

// 가져오기
var client = ServiceLocator.Instance.Get<IAIClient>();
```

## 시스템 확장

### 새 도구 추가

1. `src/SiliconLife.Default/Tools/`에 새 클래스 생성:

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

2. 도구는 리플렉션을 통해 자동으로 검색됩니다 - 수동 등록 불필요!

3. (선택사항) 관리자 전용 표시:
```csharp
[SiliconManagerOnly]
public class AdminTool : ITool { ... }
```

### 새 AI 클라이언트 추가

1. `src/SiliconLife.Default/AI/`에 `IAIClient` 구현:

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

### 새 저장소 백엔드 추가

1. `src/SiliconLife.Default/Storage/`에 `IStorage` 및 `ITimeStorage` 구현:

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

### 새 스킨 추가

1. `src/SiliconLife.Default/Web/Skins/`에 `ISkin` 구현:

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
            /* 사용자 정의 스타일 */
        ";
    }
}
```

2. 스킨은 `SkinManager`에 의해 자동으로 검색됩니다.

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

2. (선택사항) 플러그인에서 `ITool` 인터페이스를 구현하여 사용자 정의 도구를 등록합니다:

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

3. 컴파일된 DLL을 플러그인 디렉토리에 넣으면 `PluginLoader`가 자동으로 로드합니다.

> **보안 제한**: 플러그인은 `System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis` 등의 네임스페이스를 참조할 수 없습니다. 플러그인은 `AssemblyLoadContext`를 통해 격리 로드됩니다.

## 코드 스타일 가이드라인

### 명명 규칙

- **클래스**: 기능 접두사가 있는 PascalCase (예: `DefaultSiliconBeing`)
- **인터페이스**: `I`로 시작 (예: `IAIClient`, `ITool`)
- **구현**: 인터페이스 이름으로 끝남 (예: `OllamaClient`는 `IAIClient` 구현)
- **도구**: `Tool`로 끝남 (예: `CalendarTool`, `ChatTool`)
- **ViewModel**: `ViewModel`로 끝남 (예: `BeingViewModel`)

### 코드 구성

```
SiliconLife.Common/
├── AI/                    # AI 클라이언트 및 팩토리 구현
├── Calendar/              # 32가지 캘린더 구현
├── Localization/          # 현지화 기본 클래스 및 33개 언어 구현
├── Security/              # 권한 관리자
├── SiliconBeing/          # 기본 실리콘 비잉 구현
├── Tools/                 # 공유 내장 도구
├── Web/                   # 웹 인프라
└── WebView/               # Playwright WebView 구현

SiliconLife.App/          # Default와 Fast가 공유하는 애플리케이션 계층
├── Config/                # 애플리케이션 설정
├── Help/                  # 도움말 문서 현지화
└── Web/                   # Web UI 구현
    ├── Component/         # UI 컴포넌트 라이브러리
    ├── Controllers/       # 라우트 컨트롤러
    ├── Models/            # 뷰 모델
    ├── Views/             # HTML 뷰
    └── Skins/             # 스킨 테마

SiliconLife.Default/      # 버전별 디렉토리
├── Config/                # 기본 설정 데이터
├── IM/                    # WebUI 제공자
├── Knowledge/             # 지식 네트워크 구현
├── Logging/               # 로그 제공자 구현
├── Project/               # 프로젝트 시스템 구현
├── Security/              # 기본 권한 콜백
├── Storage/               # 파일 시스템 저장소 구현
└── Tools/                 # 버전별 도구 (HelpTool)
```

### 문서

- 모든 공개 API에는 XML 문서 주석 필수
- 모든 소스 파일에 Apache 2.0 라이선스 헤더 사용
- .NET 9 기능 활용 (암시적 using, null 허용 참조 타입)

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

## 사용자 정의 기능 빌드

### 예시: 사용자 정의 캘린더 추가

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

### 예시: 사용자 정의 Executor 추가

```csharp
public class CustomExecutor : ExecutorBase
{
    public override string Name => "custom";
    
    public override async Task<ExecutorResult> ExecuteAsync(ExecutorRequest request)
    {
        // 먼저 권한 검증
        var permission = await CheckPermissionAsync(request);
        if (!permission.Allowed)
        {
            return ExecutorResult.Denied(permission.Reason);
        }
        
        // 작업 실행
        var result = await PerformOperation(request);
        
        return ExecutorResult.Success(result);
    }
}
```

## 테스트 가이드라인

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
1. AI가 도구 호출 반환
2. 도구 실행
3. 결과를 AI에 피드백
4. AI가 최종 응답 반환

## 성능 고려사항

### 저장소 시스템

- Default 버전은 파일 기반 JSON 저장소 사용
- Fast 버전은 SpeedyPack 메모리 스토리지 엔진 사용 (.spk 형식)
- SpeedyPack은 메모리 디렉토리 매핑 + 엔트리 캐시 + 비동기 쓰기 큐 채택
- 시간 인덱스 쿼리는 `ITimeStorage` 인터페이스 사용

### MainLoop 스케줄러

- 공정 스케줄링을 위한 틱 기반 시분할
- 중지된 작업 감지를 위한 watchdog 타이머
- 연쇄 실패 방지를 위한 circuit breaker

## 모범 사례

### 1. 항상 권한 검증

AI가 시작한 모든 작업은 권한 체인을 통과해야 합니다:

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return Result.Denied(permission.Reason);
}
```

### 2. 서비스 로케이터 사용

전역적으로 서비스 등록 및 검색:

```csharp
// 초기화 중
ServiceLocator.Instance.Register<ICustomService>(myService);

// 필요 시
var service = ServiceLocator.Instance.Get<ICustomService>();
```

### 3. Body-Brain 분리 따르기

- Body는 상태 및 트리거 처리
- Brain은 AI 상호작용 및 도구 실행 처리

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

## 기여 가이드라인

1. 저장소 포크
2. 기능 브랜치 생성 (`git checkout -b feature/amazing-feature`)
3. conventional commits를 사용하여 변경사항 커밋
4. 브랜치에 푸시 (`git push origin feature/amazing-feature`)
5. Pull Request 열기

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
