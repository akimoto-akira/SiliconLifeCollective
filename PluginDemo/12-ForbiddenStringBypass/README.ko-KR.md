# PluginDemo-12: 금지된 문자열 리플렉션 우회 안티패턴

## 개요

이 플러그인은 SiliconLife 플러그인 시스템에서 **금지된** 문자열 기반 리플렉션 우회 시도를 시연합니다. 문자열 연결, 보간, 인코딩 및 기타 난독화 기술이 왜 PluginLoader의 #US(User String) 힙 스캔——**최후의 방어선**——을 **우회할 수 없는지** 보여줍니다.

## #US 힙이란?

.NET PE(Portable Executable) 메타데이터에서 **#US(User String) 힙**은 `ldstr` IL 명령어에 사용되는 모든 문자열 리터럴 피연산자를 저장합니다. C# 코드에서 문자열 리터럴을 작성할 때마다 컴파일러는 이를 이 힙에 저장합니다.

```
C# 소스:    string s = "System.IO.File";
    ↓ 컴파일
IL 코드:    ldstr "System.IO.File"    ← #US 힙의 토큰 참조
    ↓ PluginLoader 스캔
#US 힙:     [..., "System.IO.File", ...]  ← 접두사 매칭으로 탐지!
```

PluginLoader의 `ScanUserStrings()` 메서드는 #US 힙의 **모든 항목**을 순회하며, 금지된 접두사로 시작하는 문자열이 있는지 검사합니다.

## 금지된 문자열 접두사

#US 힙에서 다음 접두사가 발견되면 `[ILString]` 위반이 트리거됩니다:

| 접두사 | 카테고리 |
|--------|---------|
| `System.IO.` | 파일 시스템 타입 |
| `System.Net.Http` | HTTP 클라이언트 |
| `System.Net.WebSockets` | WebSocket |
| `System.Net.Sockets` | 원시 소켓 |
| `System.Net.Mail` | SMTP |
| `System.Net.NetworkInformation` | 네트워크 탐색 |
| `System.Net.Security` | SslStream |
| `System.Diagnostics.Process` | 프로세스/명령줄 |
| `Microsoft.CodeAnalysis` | Roslyn 컴파일러 |
| `System.Reflection.Emit` | IL 방출 |
| `System.Runtime.Loader` | AssemblyLoadContext |
| `System.CodeDom.Compiler` | 레거시 CodeDom |
| `Microsoft.Win32` | Windows 레지스트리 |

## 시연된 위반

### 위반 1: 직접 타입 이름 문자열

```csharp
// ❌ 금지 — 전체 문자열이 #US 힙에 저장됨
Type? fileType = Type.GetType("System.IO.File, System.Runtime");
```

**위반**: `[ILString] "System.IO.File, System.Runtime" matches forbidden prefix "System.IO."`

### 위반 2: 문자열 연결(컴파일 시)

```csharp
// ❌ 금지 — 컴파일러가 const+const를 하나의 #US 항목으로 폴딩
const string ns = "System.Net.Http";
const string typeName = ".HttpClient";
const string assembly = ", System.Net.Http";
Type? type = Type.GetType(ns + typeName + assembly);
// 각 부분과 폴딩된 결과 모두 #US 힙에!
```

**위반**: `[ILString] "System.Net.Http.HttpClient, System.Net.Http" matches forbidden prefix "System.Net.Http"`

### 위반 3: 문자열 보간

```csharp
// ❌ 금지 — 리터럴 부분이 #US 힙에 저장됨
string className = "FileStream";
string fullName = $"System.IO.{className}, System.Runtime";
// "System.IO."만으로도 접두사 매칭!
```

**위반**: `[ILString] "System.IO." matches forbidden prefix "System.IO."`

### 위반 4: Const 필드

```csharp
// ❌ 금지 — const 값이 사용 위치에서 인라인됨 → #US 힙에 출현
private const string ProcessType = "System.Diagnostics.Process";
private const string AssemblyName = ", System.Runtime";
Type? type = Type.GetType(ProcessType + AssemblyName);
```

**위반**: `[ILString] "System.Diagnostics.Process" matches forbidden prefix "System.Diagnostics.Process"`

### 위반 5: 부분 문자열 조각

```csharp
// ❌ 금지 — 각 부분이 독립적인 ldstr로 개별 스캔됨
string part1 = "System.Reflection.Emit";
string part2 = ".AssemblyBuilder";
string fullType = part1 + part2;
// "System.Reflection.Emit"만으로도 접두사 매칭!
```

**위반**: `[ILString] "System.Reflection.Emit" matches forbidden prefix "System.Reflection.Emit"`

### 위반 6: 다수의 금지 대상

```csharp
// ❌ 금지 — 어셈블리 내 모든 문자열이 스캔됨
string tcp = "System.Net.Sockets.TcpClient";
string registry = "Microsoft.Win32.Registry";
string loader = "System.Runtime.Loader.AssemblyLoadContext";
```

**다수의 위반** — 스캐너는 첫 번째 매칭에서 멈추지 않습니다.

## 왜 난독화 기술은 모두 실패하는가

| 기술 | 왜 실패하는가 |
|------|-------------|
| Const 연결 | 컴파일러가 단일 #US 항목으로 폴딩 |
| 문자열 보간 | 리터럴 부분이 #US 힙에 저장됨 |
| Const 필드 | 값이 사용 위치에서 인라인됨 → #US에 출현 |
| 변수로 분할 | 각 `ldstr` 피연산자가 독립적으로 스캔됨 |
| Base64 인코딩 | 디코딩에 런타임 메서드 필요, 그러나 `Type.GetType`이 MemberRef로 차단됨 |
| 문자 배열 구성 | `ldstr`을 생성하지 않지만 `Type.GetType`이 여전히 MemberRef로 차단됨 |
| XOR 암호화 | 암호화된 문자열은 #US에서 읽을 수 없지만, 복호화 + `Type.GetType` = MemberRef 차단 |
| 문자열 반전 | 반전된 리터럴이 접두사와 여전히 매칭될 수 있고, reverse + GetType = MemberRef 차단 |

**핵심 통찰**: #US 스캔은 **문자열**을 차단합니다. MemberRef 스캔은 **메서드**를 차단합니다. 타입을 동적으로 로드하려면 둘 다 필요합니다. PluginLoader는 둘을 독립적으로 차단합니다.

## 완전한 방어 체인

PluginLoader의 5단계 스캔이 돌파 불가능한 방어를 구축합니다:

| 단계 | 메커니즘 | 탐지 내용 |
|------|---------|----------|
| 1 | TypeRef 테이블 | 금지된 타입에 대한 직접 참조 |
| 2 | ExportedType 테이블 | 금지된 네임스페이스의 포워딩된 타입 |
| 3 | MemberRef 테이블 | `Type.GetType`, `Assembly.Load`, `Activator.CreateInstance` 호출 |
| 4 | Unsafe 마커 | `[DllImport]`, unsafe 블록, PinvokeImpl 플래그 |
| **5** | **#US 힙 스캔** | **금지된 접두사와 매칭되는 문자열 상수(이 데모)** |

5단계가 **최후의 방어선**인 이유:
- 1-4단계는 **능동적 코드**(타입, 메서드, 특성)를 탐지
- 5단계는 **수동적 데이터**(우회에 사용될 수 있는 문자열)를 탐지
- MemberRef 스캔이 알려지지 않은 메서드를 놓치더라도, 금지된 타입 이름 문자열은 어셈블리에 반드시 존재해야 함 → #US 스캔이 탐지

## 트레이드오프

#US 힙 스캔은 **경미한 오탐지**를 발생시킬 수 있습니다:
- 로그 메시지: `logger.Info("Failed to connect to System.Net.Http endpoint")` → 플래그됨
- 문서 문자열: XML 문서로 컴파일되는 주석은 #US 힙에 포함되지 않음(안전)
- `nameof()` 표현식: 접두사와 매칭되지 않는 컴파일 시간 문자열은 안전

**오탐지 해결책**: 금지된 접두사를 피하도록 문자열 리터럴을 재구성.

## 다른 예제와의 비교

| 예제 | 초점 | 관계 |
|------|------|------|
| **10-ForbiddenReflection** | 금지된 리플렉션 메서드(MemberRef 스캔) | 메서드 탐지 |
| **11-ForbiddenPInvoke** | 금지된 P/Invoke와 unsafe 코드 | 다른 위협 벡터 |
| **12-ForbiddenStringBypass** | 문자열 기반 우회 시도(이 예제) | 문자열 탐지 |
| **02-TypeRegistryUsage** | 안전한 타입 검색 | 올바른 대안 |
| **03-ObjectFactoryUsage** | 안전한 객체 생성 | 올바른 대안 |

## 모범 사례

1. **모든 문자열에서 금지된 접두사를 피하라** — 로그 메시지와 IL로 컴파일되는 주석 포함
2. **ITypeRegistry/IObjectFactory를 사용하라** — 문자열 리플렉션의 안전한 대안
3. **문자열 분할은 소용없음을 이해하라** — 각 조각이 독립적으로 스캔됨
4. **런타임 구성(char[])은 #US를 우회** — 그러나 Type.GetType의 MemberRef 스캔으로 차단됨
5. **심층 방어가 작동한다** — 한 레이어를 우회해도 다른 레이어가 탐지

## 파일

- `Plugin.cs` - 안티패턴 데모 플러그인
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 이 파일(한국어)
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## 관련 예제

- **10-ForbiddenReflection**: 금지된 리플렉션 메서드(MemberRef 스캔)
- **11-ForbiddenPInvoke**: 금지된 P/Invoke와 unsafe 코드
- **02-TypeRegistryUsage**: ITypeRegistry 올바른 사용법
- **03-ObjectFactoryUsage**: IObjectFactory 올바른 사용법
