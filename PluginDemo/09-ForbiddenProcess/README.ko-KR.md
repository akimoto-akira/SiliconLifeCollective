# PluginDemo-09: 금지된 프로세스 작업 안티패턴

## 개요

이 플러그인은 SiliconLife 플러그인 시스템에서 **금지된** 프로세스 실행 작업을 시연합니다. 안티패턴 참조로서, 하면 안 되는 것을 보여주고 각 위반에 대한 올바른 대안을 제공합니다.

## Process 타입이 금지된 이유

`System.Diagnostics.Process`와 `ProcessStartInfo`는 플러그인에서 차단됩니다. 직접 프로세스 실행은 심각한 보안 위험을 초래하기 때문입니다:

1. **임의 명령 실행**: 감사나 권한 검사 없이 모든 명령 실행 가능
2. **악성코드 실행**: 악의적인 플러그인이 원치 않는 애플리케이션이나 스크립트 실행 가능
3. **시스템 리소스 접근**: 플러그인 샌드박스 외부의 민감한 시스템 리소스 접근 가능
4. **명령 검증 없음**: 직접 Process.Start에는 명령 주입 보호 기능이 내장되어 있지 않음
5. **감사 추적 없음**: 직접 프로세스 작업은 플러그인 보안 감사 시스템을 우회
6. **권한 상승**: 플러그인의 권한 수준보다 높은 프로세스 생성 가능

## 금지된 타입

Process 관련 타입만 금지되며, **전체 System.Diagnostics 네임스페이스가 아닙니다**:

| 금지된 타입 | 차단되는 메서드 | 위험 수준 |
|------------|---------------|----------|
| `Process` | `Start()`, `Kill()`, `WaitForExit()` | 🔴 심각 |
| `ProcessStartInfo` | 생성자, 모든 속성 | 🔴 심각 |
| `Process` | `StandardInput`, `StandardOutput`, `StandardError` | 🔴 심각 |
| `Process` | `GetProcesses()`, `GetProcessesByName()` | 🟡 높음 |

## 허용된 타입

프로세스 실행과 무관한 다른 `System.Diagnostics` 타입은 계속 사용 가능:

| 허용된 타입 | 용도 | 안전한 이유 |
|-----------|------|-----------|
| `Stopwatch` | 시간 측정 | 프로세스 실행 없음 |
| `Debug` | 디버그 출력 | 보안 위험 없음 |
| `Trace` | 추적/로깅 | 보안 위험 없음 |
| `PerformanceCounter` | 성능 모니터링 | 읽기 전용, 감사 대상 |

## 안전하게 명령을 실행하는 방법

### CommandLineExecutor 사용 (유일하게 안전한 방법)

`CommandLineExecutor`는 플러그인에서 명령 실행의 **제어된 진입점**입니다:

```csharp
// ✅ 올바름: 명령 실행
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);

if (result.Success)
{
    Console.WriteLine(result.Output);
}
else
{
    Console.WriteLine($"오류: {result.Error}");
}
```

**CommandLineExecutor가 제공하는 기능:**
1. **명령 주입 보호**: 위험한 구분자 차단 (`||`, `&&`, `|`, `&`, `;`)
2. **타임아웃 적용**: 기본 30초 타임아웃 (구성 가능)
3. **감사 로그**: 모든 명령 실행이 보안 검토를 위해 기록
4. **출력 캡처**: stdout과 stderr 자동 캡처
5. **크로스 플랫폼 지원**: Windows에서는 `cmd.exe`, Unix에서는 `/bin/bash`
6. **오류 처리**: 성공/실패 상태를 포함한 구조화된 결과 반환

## 시연되는 위반

이 플러그인은 5가지 일반적인 프로세스 실행 위반을 보여줍니다:

### 위반 1: Process.Start

```csharp
// ❌ 금지
Process.Start("notepad.exe");

// ✅ 올바름
var request = new ExecutorRequest { ResourcePath = "notepad.exe" };
var result = CommandLineExecutor.Execute(request);
```

**차단되는 TypeRef**: `System.Diagnostics.Process::Start(System.String)`

### 위반 2: ProcessStartInfo

```csharp
// ❌ 금지
var psi = new ProcessStartInfo {
    FileName = "cmd.exe",
    Arguments = "/c dir",
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = new Process { StartInfo = psi };
process.Start();

// ✅ 올바름
var request = new ExecutorRequest { ResourcePath = "dir" };
var result = CommandLineExecutor.Execute(request);
Console.WriteLine(result.Output);
```

**차단되는 TypeRef**: `System.Diagnostics.ProcessStartInfo::.ctor()`

### 위반 3: 인수가 있는 Process

```csharp
// ❌ 금지
var psi = new ProcessStartInfo("ping", "127.0.0.1 -n 4") {
    UseShellExecute = false,
    RedirectStandardOutput = true
};
using var process = Process.Start(psi);
process.WaitForExit();

// ✅ 올바름
var request = new ExecutorRequest { ResourcePath = "ping 127.0.0.1 -n 4" };
var result = CommandLineExecutor.Execute(request, TimeSpan.FromSeconds(10));
Console.WriteLine(result.Output);
```

**차단되는 TypeRef**: `System.Diagnostics.Process::Start(ProcessStartInfo)`

### 위반 4: 프로세스 출력 리다이렉션

```csharp
// ❌ 금지
var psi = new ProcessStartInfo("ipconfig") {
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true
};
using var process = Process.Start(psi);
string output = process.StandardOutput.ReadToEnd();
string error = process.StandardError.ReadToEnd();

// ✅ 올바름
var request = new ExecutorRequest { ResourcePath = "ipconfig" };
var result = CommandLineExecutor.Execute(request);
if (result.Success) Console.WriteLine(result.Output);
else Console.WriteLine(result.Error);
```

**차단되는 TypeRef**: `System.Diagnostics.Process::StandardOutput`

### 위반 5: Process.Kill

```csharp
// ❌ 금지
Process[] processes = Process.GetProcessesByName("notepad");
foreach (var p in processes) p.Kill();

// ✅ 올바름
// 보안상의 이유로 CommandLineExecutor는 프로세스 종료를 지원하지 않습니다.
// 필요한 경우 시스템 관리자에게 문의하십시오.
```

**차단되는 TypeRef**: `System.Diagnostics.Process::Kill()`

## 왜 System.Diagnostics 전체가 아닌 Process만 금지되는가?

플러그인 시스템은 **정밀 타격** 보안 전략을 채택합니다:

- **위험한 타입만 차단**: Process/ProcessStartInfo는 임의 코드 실행을 가능하게 함
- **안전한 타입 허용**: Stopwatch, Debug, Trace는 보안 문제 없음
- **영향 최소화**: 개발자는 위험하지 않은 진단 도구를 계속 사용 가능
- **명확한 경계**: 프로세스를 생성/종료할 수 있는 타입만 금지

이는 `System.IO`와 다릅니다. IO는 **전체가 차단**됩니다. 대부분의 IO 타입이 파일 시스템에 직접 접근하기 때문입니다.

## PluginLoader 보안 메커니즘

PluginLoader가 이 플러그인을 스캔할 때:

1. **TypeRef 스캔**: 금지된 `Process`/`ProcessStartInfo` 타입에 대한 참조 감지
2. **MemberRef 스캔**: 차단된 메서드 호출 감지 (예: `Process.Start`)
3. **IL 문자열 스캔**: 문자열 기반 리플렉션을 통한 금지 타입 로딩 시도 감지
4. **거부**: 플러그인이 로딩 시 상세한 오류 메시지와 함께 거부

## 모범 사례

1. **항상 CommandLineExecutor 사용**: `Process.Start`를 직접 사용하지 않기
2. **합리적인 타임아웃 설정**: 명령의 무한 행 방지
3. **결과 확인**: 출력 사용 전 항상 `result.Success` 검증
4. **입력 정제**: 사용자 입력을 명령에 직접 전달하지 않기
5. **필요시 Capability 선언**: 제한 없는 프로세스 실행이 필요하면 `Capability.Process` 선언 (15-CapabilityProcess 참조)

## 파일

- `Plugin.cs` - 안티패턴 시연 플러그인
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 이 파일 (한국어)
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## 관련 예제

- **08-ForbiddenNetwork**: 금지된 네트워크 작업
- **15-CapabilityProcess**: 선언적 Process 권한
- **10-ForbiddenReflection**: 금지된 리플렉션 작업
- **12-ForbiddenStringBypass**: 문자열 기반 리플렉션 우회 시도
