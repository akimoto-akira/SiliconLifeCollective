# PluginDemo-10: 금지된 리플렉션 작업 안티패턴

## 개요

이 플러그인은 SiliconLife 플러그인 시스템에서 **금지된** 리플렉션 작업을 시연합니다. 안티패턴 참조로서, 해서는 안 되는 것을 보여주고 각 위반에 대한 올바른 대안을 제공합니다.

## 왜 리플렉션이 핵심 위협인가?

리플렉션 우회는 PluginLoader의 보안 스캔에서 **가장 심각한 위협**입니다. TypeRef 스캔은 컴파일 시 직접 타입 참조를 잡을 수 있지만, 리플렉션 메서드는 **런타임**에 문자열을 사용하여 타입을 해석할 수 있어 정적 메타데이터 스캔에서 완전히 보이지 않습니다.

플러그인이 `Type.GetType("System.IO.File, System.Runtime")`을 호출할 수 있다면, PE 메타데이터의 TypeRef 테이블에 참조 없이 모든 금지된 타입에 접근할 수 있습니다.

## 어떤 메서드가 금지되는가?

모든 금지된 메서드는 **MemberRef 스캔**으로 감지됩니다 (네임스페이스나 타입 수준의 차단이 아님):

| 금지된 메서드 | 시그니처 | 위협 |
|-------------|---------|------|
| `Type.GetType` | `System.Type::GetType(System.String)` | 런타임에 이름으로 임의 타입 해석 |
| `Activator.CreateInstance` | `System.Activator::CreateInstance(...)` | 임의 타입 인스턴스화 |
| `Activator.CreateInstanceFrom` | `System.Activator::CreateInstanceFrom(...)` | DLL 경로에서 인스턴스 생성 |
| `Assembly.Load` | `System.Reflection.Assembly::Load(...)` | 이름/바이트로 어셈블리 로드 |
| `Assembly.LoadFile` | `System.Reflection.Assembly::LoadFile(...)` | 디스크에서 어셈블리 로드 |
| `Assembly.LoadFrom` | `System.Reflection.Assembly::LoadFrom(...)` | 경로에서 어셈블리 로드 |
| `Assembly.UnsafeLoadFrom` | `System.Reflection.Assembly::UnsafeLoadFrom(...)` | 보안 검사 없이 로드 |
| `Assembly.LoadWithPartialName` | `System.Reflection.Assembly::LoadWithPartialName(...)` | 부분 이름으로 로드 |
| `Assembly.ReflectionOnlyLoad` | `System.Reflection.Assembly::ReflectionOnlyLoad(...)` | 리플렉션 전용 로드 |
| `Assembly.GetType` | `System.Reflection.Assembly::GetType(System.String)` | 문자열 기반 타입 해석 |

## 무엇이 안전한가?

모든 리플렉션이 금지된 것은 아닙니다. 다음 패턴은 컴파일 시 알려진 타입을 참조하므로 **안전**합니다:

| 안전한 패턴 | 예시 | 왜 안전한가 |
|------------|-----|-----------|
| `typeof(X).Assembly` | `typeof(MyPlugin).Assembly` | 타입이 컴파일 시 알려짐, TypeRef에서 보임 |
| `typeof(X).GetProperties()` | `typeof(MyData).GetProperties()` | 알려진 타입 검사, 새 타입 도입 없음 |
| `typeof(X).GetMethods()` | `typeof(IPlugin).GetMethods()` | 알려진 타입의 멤버 검사 |
| 제네릭 제약 | `FindSubtypesOf(typeof(BaseTool))` | 제네릭 매개변수는 컴파일 시 타입 |
| `nameof()` | `nameof(MyClass.MyMethod)` | 컴파일 시 문자열, 런타임 해석 없음 |

**핵심 구분:**
- `typeof(X).Assembly` → **안전** (컴파일 시 참조, PluginLoader가 스캔)
- `Assembly.Load("X")` → **금지** (런타임 문자열, 모든 스캔 우회)

## 리플렉션을 안전하게 대체하는 방법

### ITypeRegistry 사용 (Type.GetType + AppDomain 스캔 대체)

```csharp
// ❌ 금지: 런타임에 문자열로 타입 해석
Type? type = Type.GetType("MyNamespace.MyClass, MyAssembly");

// ✅ 올바름: ITypeRegistry로 등록된 타입 검색
Type? type = typeRegistry.FindType("MyNamespace.MyClass");
// OnLoad 중에 등록된 타입만 발견 가능
```

### IObjectFactory 사용 (Activator.CreateInstance 대체)

```csharp
// ❌ 금지: 임의의 인스턴스 생성
object? instance = Activator.CreateInstance(someType);

// ✅ 올바름: IObjectFactory로 등록된 팩토리에서 생성
var instance = objectFactory.CreateInstance<MyService>();
// 팩토리가 등록된 타입만 인스턴스화 가능
```

## 시연된 위반

이 플러그인은 5가지 일반적인 리플렉션 위반을 보여줍니다:

### 위반 1: Type.GetType(string)

```csharp
// ❌ 금지
Type? fileType = Type.GetType("System.IO.File, System.Runtime");
var method = fileType?.GetMethod("ReadAllText");
method?.Invoke(null, new object[] { "secret.txt" });

// ✅ 올바름
Type? myType = typeRegistry.FindType("MyPlugin.MyCustomType");
```

**차단된 MemberRef**: `System.Type::GetType(System.String)`

### 위반 2: Activator.CreateInstance

```csharp
// ❌ 금지
Type? httpClientType = Type.GetType("System.Net.Http.HttpClient, System.Net.Http");
object? client = Activator.CreateInstance(httpClientType!);

// ✅ 올바름
var instance = objectFactory.CreateInstance<MyService>();
```

**차단된 MemberRef**: `System.Activator::CreateInstance`

### 위반 3: Assembly.Load

```csharp
// ❌ 금지
Assembly asm = Assembly.Load("System.Net.Http");
Type? httpType = asm.GetType("System.Net.Http.HttpClient");
object? client = Activator.CreateInstance(httpType!);

// ✅ 올바름
Assembly myAsm = typeof(MyPlugin).Assembly;  // 안전: 컴파일 시 알려짐
Type? type = typeRegistry.FindType("MyPlugin.SomeType");
```

**차단된 MemberRef**: `System.Reflection.Assembly::Load(System.String)`

### 위반 4: Assembly.LoadFile / LoadFrom

```csharp
// ❌ 금지
Assembly asm = Assembly.LoadFile(@"C:\malware\evil.dll");
Assembly asm2 = Assembly.LoadFrom(@"\\network\share\trojan.dll");

// ✅ 올바름
// 모든 의존성은 플러그인 디렉토리에 배치하여 PluginLoader가 스캔해야 합니다.
// OnLoad에서 ITypeRegistry.RegisterFromAssembly를 사용하여 자체 어셈블리를 등록하세요.
```

**차단된 MemberRef**: `System.Reflection.Assembly::LoadFile(System.String)` / `LoadFrom(System.String)`

### 위반 5: Assembly.GetType(string)

```csharp
// ❌ 금지
Assembly runtime = typeof(object).Assembly;
Type? processType = runtime.GetType("System.Diagnostics.Process");

// ✅ 올바름
Type? safeType = typeRegistry.FindType("MyPlugin.MySafeType");
// 금지된 타입은 절대 등록되지 않으므로 찾을 수 없습니다
```

**차단된 MemberRef**: `System.Reflection.Assembly::GetType(System.String)`

## PluginLoader 보안 메커니즘

PluginLoader가 이 플러그인을 스캔할 때:

1. **MemberRef 스캔**: 금지된 메서드 호출 감지
2. **TypeRef 스캔**: 금지된 타입에 대한 직접 참조 감지 (보조 검사)
3. **IL 문자열 스캔**: 금지된 타입 패턴과 일치하는 문자열 상수 감지 (심층 방어)
4. **거부**: 모든 위반을 나열하는 상세 오류 메시지와 함께 로드 시 플러그인 거부

## typeof(X).Assembly가 안전하고 Assembly.Load가 안전하지 않은 이유

| 작업 | 가시성 | 보안 |
|------|--------|------|
| `typeof(X).Assembly` | 타입 X가 TypeRef 테이블에 존재 → PluginLoader가 스캔 | ✅ 안전 |
| `Assembly.Load("X")` | 문자열 "X"는 런타임에만 존재 → TypeRef 스캔에 보이지 않음 | ❌ 금지 |
| `obj.GetType()` | 기존 인스턴스의 타입 반환 → 새 타입 도입 없음 | ✅ 안전 |
| `Type.GetType("X")` | 문자열에서 임의 타입 해석 → TypeRef 우회 | ❌ 금지 |

## 모범 사례

1. **OnLoad에서 타입 등록**: `ITypeRegistry.RegisterType` / `RegisterFromAssembly` 사용
2. **동적 생성에 IObjectFactory 사용**: `Activator.CreateInstance`를 절대 사용하지 마세요
3. **typeof(X).Assembly 활용**: 자체 어셈블리를 안전하게 참조
4. **문자열 기반 타입 이름 피하기**: IL 문자열 스캔을 트리거하고 플래그될 수 있음
5. **정적 발견 가능성을 위해 설계**: PluginLoader가 메타데이터에서 볼 수 없으면 의심스러움

## 파일

- `Plugin.cs` - 안티패턴 데모 플러그인
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 본 파일 (한국어)
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## 관련 예제

- **02-TypeRegistryUsage**: ITypeRegistry 올바른 사용법
- **03-ObjectFactoryUsage**: IObjectFactory 올바른 사용법
- **11-ForbiddenPInvoke**: 금지된 P/Invoke 및 unsafe 코드
- **12-ForbiddenStringBypass**: 문자열 기반 리플렉션 우회 시도
