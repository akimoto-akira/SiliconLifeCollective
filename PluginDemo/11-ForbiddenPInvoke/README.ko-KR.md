# PluginDemo-11: 금지된 P/Invoke 및 unsafe 코드 안티패턴

## 개요

이 플러그인은 SiliconLife 플러그인 시스템에서 **금지된** P/Invoke 및 unsafe 코드 작업을 시연합니다. 안전한 래퍼 대안이 있는 다른 금지 범주(파일 I/O, 네트워크, 프로세스, 리플렉션)와 달리, P/Invoke와 unsafe 코드는 **절대 금지**이며, 안전한 대안이 없고, 어떤 `PluginCapability` 선언으로도 면제할 수 없습니다.

## 왜 P/Invoke가 궁극적인 위협인가?

P/Invoke와 unsafe 코드는 **관리 런타임 완전 외부**에서 작동하므로 플러그인 보안에 대한 **가장 근본적인 위협**입니다:

- 네이티브 코드는 전체 프로세스 권한으로 실행
- 관리형 타입 안전성, 메모리 안전성, 가비지 컬렉션 없음
- 네이티브 호출을 가로채기, 감사, 샌드박스화 불가능
- 네이티브 코드 충돌 = 전체 프로세스 충돌 (예외 처리 없음)
- 프로세스 공간의 모든 메모리 주소 접근 가능

## 삼중 보험 메커니즘

PluginLoader는 **세 가지 독립적인 탐지 계층**을 사용하여 P/Invoke와 unsafe 코드가 절대 탐지를 회피하지 못하도록 합니다:

### 계층 1: TypeRef 테이블 스캔

PE 메타데이터에서 금지된 타입에 대한 직접 참조를 탐지:

| 금지 타입 | 네임스페이스 | 위협 |
|-----------|-------------|------|
| `DllImportAttribute` | System.Runtime.InteropServices | 네이티브 함수 임포트 선언 |
| `Marshal` | System.Runtime.InteropServices | 관리/비관리 메모리 브리지 |
| `NativeMemory` | System.Runtime.InteropServices | 네이티브 힙 malloc/free |
| `NativeLibrary` | System.Runtime.InteropServices | 네이티브 공유 라이브러리 동적 로드 |
| `GCHandle` | System.Runtime.InteropServices | 관리 객체 고정, 포인터 노출 |
| `SafeHandle` | System.Runtime.InteropServices | 네이티브 리소스 핸들 기본 클래스 |
| `Unsafe` | System.Runtime.CompilerServices | Unsafe 도우미 클래스 |
| `UnverifiableCodeAttribute` | System.Security | 검증 불가 코드 마커 |

### 계층 2: Unsafe 마커 스캔 (ScanUnsafeMarkers)

타입 참조와 독립적으로 컴파일러 생성 마커를 탐지:

| 마커 | 탐지 방법 | 소스 |
|------|-----------|------|
| `[assembly: UnverifiableCode]` | 어셈블리 CustomAttribute 테이블 | C# `unsafe` 키워드 |
| `[module: UnverifiableCode]` | 모듈 CustomAttribute 테이블 | C# `unsafe` 키워드 |
| `MethodAttributes.PinvokeImpl` | MethodDef 테이블 플래그 | `[DllImport]` 특성 |

### 계층 3: IL 문자열 스캔 (#US 힙)

InteropServices 타입을 참조하는 문자열 상수를 캐치:

```
"System.Runtime.InteropServices.Marshal"  → 플래그 지정
"System.Runtime.InteropServices.*"        → 접두사 매치로 플래그 지정
```

## 시연된 위반 사항

### 위반 1: [DllImport] 선언

```csharp
// ❌ 금지
[DllImport("kernel32.dll")]
private static extern ulong GetTickCount64();
```

**탐지 방법:**
- `[TypeRef] System.Runtime.InteropServices.DllImportAttribute`
- `[PInvoke] GetTickCount64 (native interop)` (PinvokeImpl 플래그)

### 위반 2: Marshal 사용

```csharp
// ❌ 금지
IntPtr ptr = Marshal.AllocHGlobal(1024);
string? str = Marshal.PtrToStringAnsi(ptr);
Marshal.FreeHGlobal(ptr);
```

**탐지 방법:** `[TypeRef] System.Runtime.InteropServices.Marshal`

### 위반 3: NativeMemory 사용

```csharp
// ❌ 금지
unsafe
{
    void* buffer = NativeMemory.Alloc(4096);
    NativeMemory.Free(buffer);
}
```

**탐지 방법:**
- `[TypeRef] System.Runtime.InteropServices.NativeMemory`
- `[UnsafeMarker] [module: System.Security.UnverifiableCode]`

### 위반 4: GCHandle 고정

```csharp
// ❌ 금지
GCHandle handle = GCHandle.Alloc(managedArray, GCHandleType.Pinned);
IntPtr ptr = handle.AddrOfPinnedObject();
handle.Free();
```

**탐지 방법:** `[TypeRef] System.Runtime.InteropServices.GCHandle`

### 위반 5: unsafe 블록

```csharp
// ❌ 금지
unsafe
{
    int* ptr = &value;
    *ptr = 100;
    byte* stack = stackalloc byte[256];
}
```

**탐지 방법:** `[UnsafeMarker] [module: System.Security.UnverifiableCode]`

### 위반 6: NativeLibrary 로드

```csharp
// ❌ 금지
IntPtr lib = NativeLibrary.Load("evil.dll");
IntPtr funcPtr = NativeLibrary.GetExport(lib, "malicious_function");
NativeLibrary.Free(lib);
```

**탐지 방법:** `[TypeRef] System.Runtime.InteropServices.NativeLibrary`

## 안전한 대안 없음 — 비교

| 금지 범주 | 안전 래퍼 | 감사 가능 | PluginCapability로 선언 가능 |
|-----------|-----------|-----------|------------------------------|
| 파일 I/O | PermissionedStreamFactory | ✅ 예 | ✅ Capability.FileIO |
| 네트워크 | NetworkExecutor | ✅ 예 | ✅ Capability.Network |
| 프로세스 | CommandLineExecutor | ✅ 예 | ✅ Capability.Process |
| 리플렉션 | ITypeRegistry + IObjectFactory | ✅ 예 | ❌ 항상 금지 |
| **P/Invoke 및 unsafe** | **❌ 없음** | **❌ 불가능** | **❌ 항상 금지** |

## 플러그인이 정말로 네이티브 코드가 필요한 경우

라이브러리가 P/Invoke 또는 unsafe 코드를 합법적으로 사용하는 경우:

1. **프로젝트 관리자의 수동 감사** 필요
2. **PluginLoader의 `TrustedAssemblies` 허용 목록에 추가** 필요
3. **PE 메타데이터의 `AssemblyDefinition.Name`으로 식별** (파일 이름 아님 — 이름 변경 공격 방지)

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

- **04-SafeSystemIO**: System.IO 허용 목록 안전 타입
- **06-TrustedDependency**: TrustedAssemblies 허용 목록 메커니즘
- **10-ForbiddenReflection**: 금지된 리플렉션 작업
- **12-ForbiddenStringBypass**: 문자열 기반 리플렉션 우회 시도
