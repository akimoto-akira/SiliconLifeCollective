# PluginDemo-18: 기능 거부 — 선언 불가능한 기능 반례

## 개요

이 플러그인은**반례**로, 기능을 선언해도 선언 불가능한 기능 금지를 우회할 수 없음을 보여줍니다. `[PluginCapability(Capability.Network)]`을 선언해도 P/Invoke, Unsafe, Reflection.Emit, 레지스트리 액세스는**항상** 차단됩니다.

## 선언 가능 vs. 선언 불가능한 기능

### ✅ 선언 가능 (Capability 열거형 값 존재)

| 기능 | 면제 내용 |
|------|---------|
| `Capability.Network` | System.Net.* 네임스페이스 및 유형별 금지 |
| `Capability.FileIO` | System.IO 네임스페이스 (허용 목록 이상) |
| `Capability.Process` | System.Diagnostics 하위 Process* 유형 |
| `Capability.AI` | IAIService 주입 활성화 (TypeRef 면제 없음) |

### ❌ 선언 불가능 (Capability 열거형 값 없음)

| 범주 | 차단되는 유형 | 선언 불가능한 이유 |
|------|-------------|-----------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory`, `NativeLibrary` | 런타임에 임의의 네이티브 코드를 감사할 수 없음 |
| Unsafe 코드 | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | CLR 타입 안전성 및 경계 검사를 우회 |
| IL 방출 | `System.Reflection.Emit.*` | 런타임에 임의의 IL을 생성 가능 |
| 어셈블리 로딩 | `System.Runtime.Loader`, `Assembly.Load*` | 스캔되지 않은 DLL을 로드하여 보안 스캔을 우회 가능 |
| 레지스트리 | `Microsoft.Win32.*` | 플러그인 샌드박스 외부의 OS 수준 시스템 액세스 |
| 동적 컴파일 | `Microsoft.CodeAnalysis.*` | 임의의 코드를 컴파일하고 실행 가능 |
| 위험한 리플렉션 | `Type.GetType(string)`, `Activator.CreateInstance` | 문자열로 금지된 유형을 인스턴스화 가능 |

## 왜 이러한 기능은 선언 불가능한가

근본적 이유: **런타임에 안전하게 감사할 수 없다.**

1. **P/Invoke**: 네이티브 코드가 호출되면 CLR이 실행 내용을 추적할 수 없음 — 안전성 보장 없음
2. **Unsafe**: 플러그인 보안 모델이 의존하는 타입 안전 시스템을 우회
3. **Reflection.Emit**: PluginLoader가 스캔하지 않은 새 IL을 런타임에 생성 가능
4. **AssemblyLoadContext**: 보안 스캔되지 않은 DLL을 로드 가능
5. **Registry**: 플러그인 샌드박스 외부의 OS 수준 구성에 대한 액세스 제공

## PluginLoader의 "선언 무효" 처리

PluginLoader가 기능 선언을 만나면:

1. CustomAttribute blob에서 int32 열거값을 읽음
2. `Enum.IsDefined(typeof(Capability), value)` 확인
3. 값이 정의된 Capability 멤버가 아니면 → **조용히 무시**
4. 값이 정의되어 있으면 → 면제 규칙 적용
5. **선언 불가능 검사는 항상 강제** — 선언된 기능과 관계없이

이는 아직 존재하지 않는 "미래" 기능을 선언하는 플러그인을 방지합니다.

## 13-CapabilityNetwork와의 비교

| 측면 | 13-CapabilityNetwork (정례) | 18-CapabilityDenied (반례) |
|------|---------------------------|--------------------------|
| 선언 | `[PluginCapability(Capability.Network)]` | `[PluginCapability(Capability.Network)]` |
| HttpClient 사용 | ✅ 면제 | ✅ 면제 |
| DllImport 사용 | 해당 없음 | ❌ 항상 차단 |
| Unsafe 사용 | 해당 없음 | ❌ 항상 차단 |
| 로드 결과 | ✅ 성공 | ❌ 거부 |

## 파일

- `Plugin.cs` — 선언 불가능한 기능 반례를 보여주는 데모 플러그인
- `README.md` — 이 파일 (영어)
- `README.zh-CN.md` — 중국어 간체
- 번역: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 관련 예제

- **13-CapabilityNetwork**: Capability.Network 정례
- **11-ForbiddenPInvoke**: P/Invoke 반례 (기능 선언으로 해결 불가)
- **10-ForbiddenReflection**: Reflection 반례 (기능 선언으로 해결 불가)
