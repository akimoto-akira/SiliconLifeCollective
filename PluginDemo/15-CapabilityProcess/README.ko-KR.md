# PluginDemo-15: Capability.Process — 선언적 프로세스 권한

## 개요

이 플러그인은 `[PluginCapability(Capability.Process)]`를 사용하여 플러그인이 자식 프로세스를 시작하는 능력이 필요함을 선언하는 방법을 보여줍니다. 이 기능을 선언하면 `System.Diagnostics.Process` 및 관련 유형에 액세스할 수 있습니다.

## 선언 구문

```csharp
[PluginCapability(Capability.Process, Reason = "Launch build tools for CI pipeline")]
public class CapabilityProcessPlugin : IPlugin { ... }
```

## Capability.Process 면제 범위

### TypeRef 면제

`System.Diagnostics` 하위의 Process 관련 유형만 면제됩니다:

| 면제 유형 | 용도 |
|----------|------|
| `Process` | 자식 프로세스 시작, 관리 및 모니터링 |
| `ProcessStartInfo` | 프로세스 시작 매개변수 구성 |
| `ProcessThread` | 프로세스 스레드 정보 액세스 |
| `ProcessModule` | 프로세스 모듈 정보 액세스 |
| `ProcessPriorityClass` | 프로세스 우선순위 설정 |
| `ProcessWindowStyle` | 프로세스 창 스타일 구성 |

항상 허용되는 유형 (금지 목록에 없음): `Stopwatch`, `Debug`, `Trace`, `Activity`

### ILString 면제

- `"System.Diagnostics.Process"`로 시작하는 문자열은 플래그되지 않음

## 09-ForbiddenProcess와의 비교

| 측면 | 09-ForbiddenProcess | 15-CapabilityProcess |
|------|-------------------|---------------------|
| 선언 | 없음 | `[PluginCapability(Capability.Process)]` |
| Process.Start | ❌ 거부됨 | ✅ 허용됨 |
| ProcessStartInfo | ❌ 거부됨 | ✅ 허용됨 |

## 권장: CommandLineExecutor

`Capability.Process`가 있어도 `CommandLineExecutor`를 우선 사용하는 것이 권장됩니다:

| 기능 | CommandLineExecutor | 직접 Process |
|------|-------------------|-------------|
| 기능 선언 필요 | 아니오 | 예 |
| 샌드박스 | 명령 허용 목록 | 없음 |
| 타임아웃 | 내장 | 수동 |
| 출력 캡처 | 구조화 | 수동 |
| 감사 로그 | 자동 | 수동 |

I/O 스트림에 대한 세밀한 제어, 프로세스 이벤트 처리, 또는 CommandLineExecutor의 명령 허용 목록이 너무 제한적인 경우에만 `Capability.Process` + 직접 `Process`를 사용하십시오.

## 보안 모범 사례

1. **CommandLineExecutor 우선 사용**: 가능하면 제어된 진입점 사용
2. **명확한 Reason 제공**: "Launch build tools for CI pipeline" vs 모호한 "process access"
3. **모든 입력 검증**: 신뢰할 수 없는 입력을 ProcessStartInfo에 직접 전달하지 않음
4. **WaitForExit 사용**: 좀비 프로세스 방지를 위해 항상 프로세스 완료 대기
5. **스트림 리디렉션**: `RedirectStandardOutput = true` 및 `UseShellExecute = false` 설정

## 파일

- `Plugin.cs` — Capability.Process를 선언하는 데모 플러그인
- `README.md` — 이 파일 (영어)
- `README.zh-CN.md` — 중국어 간체
- 번역: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 관련 예제

- **09-ForbiddenProcess**: 차단되는 프로세스 작업 안티패턴
- **18-CapabilityDenied**: 선언 불가 기능 안티패턴
