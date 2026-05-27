# 최소 플러그인 데모

하드코딩된 값으로 플러그인 수명 주기를 보여주는 최소한의 `IPlugin` 구현입니다.

## IPlugin 인터페이스 전체

모든 SiliconLife 플러그인은 `SiliconLife.Collective`에 정의된 `IPlugin` 인터페이스를 구현해야 합니다：

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### 속성 요약

| 멤버 | 유형 | 설명 |
|------|------|------|
| `Id` | `string` | 고유 식별자, 버전 간 안정적이어야 함 (예: `"com.siliconlife.demo.minimal"`) |
| `GetName(Language)` | `string` | 사람이 읽을 수 있는 표시 이름, `Language` 열거형으로 현지화 |
| `Version` | `string` | 시맨틱 버전 문자열 (예: `"1.0.0"`) |
| `GetDescription(Language)` | `string` | 플러그인 기능의 간단한 설명 |
| `GetAuthor(Language)` | `string` | 작성자 또는 조직 이름 |

## 수명 주기 호출 순서

호스트는 엄격한 순서로 수명 주기 메서드를 호출합니다：

```
OnLoad → OnStart → [실행 중] → OnStop → OnUnload
```

| 메서드 | 호출 시점 | 일반적인 용도 |
|--------|----------|-------------|
| `OnLoad()` | 플러그인 DLL이 호스트 프로세스에 로드될 때 한 번 호출 | 구성 검증, 형식 등록, 리소스 준비 |
| `OnStart()` | 호스트가 완전히 시작되고 모든 플러그인이 로드된 후 | 다른 플러그인과 상호작용, 백그라운드 작업 시작 |
| `OnStop()` | 호스트가 정상적으로 종료될 때 | 리소스 해제, 버퍼 플러시, 상태 저장 |
| `OnUnload()` | 플러그인이 호스트 프로세스에서 언로드될 때 | 최종 정리 |

## 이 데모

이 플러그인은 모든 속성에 하드코딩된 값을 반환하고 수명 주기 메서드는 비어 있습니다. 플러그인 개발의 가장 간단한 시작점입니다.

## 보안 참고

플러그인은 격리된 `AssemblyLoadContext`에서 로드되며, 금지된 네임스페이스 참조(예: `System.IO`, `System.Net.Http`)가 스캔됩니다. 자세한 내용은[보안 문서](../../docs/ko-KR/security.md)를 참조하세요.
