# 신뢰할 수 있는 종속성 데모

`Newtonsoft.Json` — 내부적으로 리플렉션을 많이 사용하는 라이브러리 — 을 신뢰할 수 있는 어셈블리로 사용하는 방법을 시연합니다. PluginLoader 보안 스캐너는 신뢰할 수 있는 어셈블리를 완전히 건너뛰어 플러그인이 위반을 트리거하지 않고 참조할 수 있도록 합니다.

## TrustedAssemblies 화이트리스트 메커니즘

`PluginLoader`는 **기본적으로 신뢰할 수 있는** 오픈소스 라이브러리의 정적 화이트리스트를 유지합니다:

```csharp
private static readonly HashSet<string> TrustedAssemblies = new(StringComparer.Ordinal)
{
    // 직렬화
    "Google.Protobuf",
    "protobuf-net",
    "Newtonsoft.Json",        // ← 이 데모에서 사용
    "MessagePack",
    "YamlDotNet",

    // 로깅
    "Serilog", "NLog",

    // Microsoft.Extensions.*
    "Microsoft.Extensions.Logging.Abstractions",
    "Microsoft.Extensions.DependencyInjection.Abstractions",
    // ...

    // 데이터 액세스 / 매핑
    "Dapper", "AutoMapper",

    // 유효성 검사 및 메시지 디스패치
    "FluentValidation", "MediatR",
};
```

### 승인 기준

라이브러리는 **세 가지 모든** 기준을 충족해야 `TrustedAssemblies`에 추가할 수 있습니다:

| # | 기준 | 이유 |
|---|------|------|
| 1 | 널리 사용되는 오픈소스 프로젝트 (MIT / Apache 2.0 / BSD) | 공개 감사 가능한 코드 |
| 2 | 소스 코드가 공개적으로 접근 가능 | 커뮤니티 감독으로 악의적 행동 방지 보장 |
| 3 | 신뢰할 수 있는 공급업체/커뮤니티가 관리하는 NuGet 패키지 | 공급망 무결성 |

### 식별 기반

스캐너는 PE 메타데이터의 `AssemblyDefinition.Name`으로 신뢰할 수 있는 어셈블리를 식별합니다 — **DLL 파일 이름이 아닙니다**. 이를 통해 공격자가 악성 DLL을 `Newtonsoft.Json.dll`로 이름 변경하여 검사를 우회하는 것을 방지합니다.

## CollectTrustedTypeRefs — 전이적 면제

PluginLoader가 플러그인 디렉토리를 로드할 때 2단계 스캔을 수행합니다:

```
단계 1: CollectTrustedTypeRefs(pluginDir)
├── 플러그인 디렉토리의 모든 *.dll 파일 열거
├── 각 DLL에 대해: PE 메타데이터 읽기 → AssemblyDefinition.Name 확인
├── 이름 ∈ TrustedAssemblies인 경우:
│   └── 모든 TypeReference 항목 수집 → (namespace, typeName) 쌍
└── 반환: HashSet<(string Namespace, string Name)>

단계 2: ScanForbiddenReferences(pluginMainDll, trustedTypeRefs)
├── 레이어 0:   화이트리스트 조기 종료 (메인 DLL 자체가 신뢰됨 → 통과)
├── 레이어 0.5: 전이적 면제 (trustedTypeRefs 세트의 TypeRef 건너뛰기)
├── 레이어 1:   TypeRef 테이블 스캔
├── 레이어 2:   ExportedType 테이블 스캔
├── 레이어 3:   MemberRef 테이블 스캔 (위험한 메서드)
├── 레이어 4:   안전하지 않은 코드 마커 + P/Invoke
└── 레이어 5:   #US 사용자 문자열 힙 스캔
```

### 전이적 면제가 중요한 이유

Newtonsoft.Json은 내부적으로 `System.Reflection.MemberInfo`, `System.IO.TextReader` 등의 타입을 참조합니다. 플러그인이 Newtonsoft.Json을 참조하면 컴파일러가 이러한 전이적 TypeRef를 **당신의** 플러그인 DLL에 포함시킬 수 있습니다. 전이적 면제가 없으면 플러그인은 `System.IO.TextReader`를 참조했다는 이유로 위반으로 표시됩니다 — 직접 사용한 적이 없더라도.

`CollectTrustedTypeRefs`는 신뢰할 수 있는 DLL에서 모든 TypeRef를 미리 수집하고 메인 스캔 중에 "알려진 안전"으로 표시하여 이 문제를 해결합니다.

## 새 신뢰할 수 있는 종속성 추가 방법

화이트리스트에 새 라이브러리를 추가하려면:

1. 위의 세 가지 승인 기준을 충족하는지 확인
2. `PluginLoader.cs`의 `TrustedAssemblies` HashSet에 한 줄 추가:
   ```csharp
   "YourLibraryName",  // 신뢰 이유 간략 설명
   ```
3. 라이브러리 DLL을 플러그인 디렉토리(플러그인 메인 DLL 옆)에 배치
4. 스캐너가 자동으로 TypeRef를 수집하고 면제

> **⚠️ 중요:** 라이브러리를 `TrustedAssemblies`에 추가하면 스캐너가 내부 코드를 **검사하지 않습니다**. 완전히 신뢰하는 라이브러리만 추가하세요.

## 이 데모

이 플러그인은 `PluginCapability` 선언 없이 Newtonsoft.Json을 사용합니다:

| 기능 | Newtonsoft.Json 내부 동작 | 작동 이유 |
|------|---------------------------|----------|
| `JsonConvert.SerializeObject` | 리플렉션으로 속성 열거 | Newtonsoft.Json DLL이 레이어 0 화이트리스트 통과 |
| `JsonConvert.DeserializeObject<T>` | `Activator.CreateInstance` 호출, 리플렉션으로 속성 설정 | 전이적 TypeRef가 레이어 0.5에서 면제 |
| `JObject` / `JArray` 조작 | `System.Linq.Expressions`, 동적 디스패치 사용 | 모든 내부 참조가 `CollectTrustedTypeRefs`로 수집 |

### PluginCapability와의 주요 차이점

| 메커니즘 | 범위 | 사용 사례 |
|---------|------|----------|
| `TrustedAssemblies` | 전체 **라이브러리**(및 전이적 참조)를 스캔에서 면제 | 알려진 오픈소스 종속성 |
| `PluginCapability` | **플러그인 코드**를 특정 네임스페이스 금지에서 면제 | 플러그인이 System.Net/IO/Process에 직접 액세스 필요 |

신뢰할 수 있는 종속성만 사용하는 플러그인은 `PluginCapability` 선언이 **필요 없습니다**. 스캐너가 모든 것을 자동으로 처리합니다.

## 보안 참고사항

신뢰할 수 있는 어셈블리는 감사 가능한 오픈소스 프로젝트이므로 보안 스캔에서 면제됩니다. 그러나 **플러그인 코드**는 여전히 완전히 스캔됩니다. 플러그인이 `System.IO.File`이나 `System.Net.Http.HttpClient`를 직접 참조하면 해당 `PluginCapability`를 선언하지 않는 한 여전히 차단됩니다. [보안 문서](../../docs/ko-KR/security.md)를 참조하세요.
