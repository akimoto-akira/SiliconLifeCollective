# PluginDemo-13: Capability.Network — 선언적 네트워크 권한

## 개요

이 플러그인은 `[PluginCapability(Capability.Network)]`를 사용하여 플러그인이 네트워크 액세스가 필요함을 선언하는 방법을 보여줍니다. 이 기능을 선언하면 PluginLoader 보안 스캔에서 차단되는 `System.Net.*` 유형에 액세스할 수 있습니다.

## PluginCapability 선언 구문

```csharp
[PluginCapability(Capability.Network, Reason = "Calls weather REST API to retrieve forecast data")]
public class CapabilityNetworkPlugin : IPlugin { ... }
```

**핵심 요소:**
- **속성 대상**: `IPlugin`을 직접 구현하는 클래스에 지정해야 함
- **AllowMultiple = true**: 여러 `[PluginCapability]` 속성 스택 가능 (17-CapabilityStacked 참조)
- **Reason 필드**: 로드 시 보안 감사 로그에 기록되는 사람이 읽을 수 있는 설명. **모든 프로덕션 플러그인에서 명확한 Reason 제공을 강력히 권장합니다.**

## PluginLoader가 기능 선언을 처리하는 방법

1. **PE 메타데이터 읽기**: PluginLoader는 보안 스캔 시작 **전에** PE 파일의 CustomAttribute 테이블에서 기능 선언을 읽습니다
2. **스캔 규칙 완화**: 선언된 기능은 해당 유형 참조를 금지된 네임스페이스 및 금지된 유형 검사에서 면제합니다
3. **감사 로그**: 모든 선언(Reason 포함)이 보안 감사 로그에 기록됩니다
4. **선언 불가능한 기능**: P/Invoke, Unsafe, Reflection.Emit 등은 선언 여부와 관계없이 항상 차단됩니다

## Capability.Network 면제 범위

### TypeRef 면제

`Capability.Network`를 선언하면 다음 네임스페이스 기반 및 유형 기반 금지 규칙이 완화됩니다:

| 면제 네임스페이스 | 허용되는 유형 |
|------------------|-------------|
| `System.Net.Http` | `HttpClient`, `HttpRequestMessage`, `HttpResponseMessage` 등 |
| `System.Net.WebSockets` | `ClientWebSocket`, `WebSocket` 등 |
| `System.Net.Sockets` | `TcpClient`, `UdpClient`, `Socket` 등 |
| `System.Net.Mail` | `SmtpClient`, `MailMessage` 등 |
| `System.Net.NetworkInformation` | `Ping`, `NetworkInterface` 등 |
| `System.Net.Security` | `SslStream` 등 |
| `System.Net` (유형 수준 금지) | `HttpWebRequest`, `WebClient`, `Dns`, `FtpWebRequest` 등 |

### ILString 면제

이러한 접두사로 시작하는 문자열 상수는 #US 힙 스캔에서 플래그되지 않습니다:
- `"System.Net.Http"`
- `"System.Net.WebSockets"`
- `"System.Net.Sockets"`
- `"System.Net.Mail"`
- `"System.Net.NetworkInformation"`
- `"System.Net.Security"`

### 선언해도 여전히 금지되는 기능

`Capability.Network`를 선언해도 다음 기능은 **항상** 차단됩니다 (선언 불가능한 기능):

| 범주 | 차단되는 유형 | 선언 불가능한 이유 |
|------|-------------|------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` | 런타임에 안전하게 감사할 수 없음 |
| Unsafe 코드 | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | 유형 안전성 보장을 우회함 |
| IL 내보내기 | `System.Reflection.Emit.*` | 런타임에 임의의 코드를 생성할 수 있음 |
| 어셈블리 로딩 | `System.Runtime.Loader`, `Assembly.Load*` | 검사되지 않은 DLL을 로드하여 보안 스캔을 우회할 수 있음 |
| 레지스트리 | `Microsoft.Win32.*` | 플러그인 샌드박스 외부의 OS 수준 시스템 액세스 |

## Reason 필드의 감사 역할

`Reason` 필드는 기능 선언의 **감사 추적** 역할을 합니다:

```
Security audit: [CapabilityNetworkPlugin] com.siliconlife.demo.capabilitynetwork declared Capability.Network — reason: Calls weather REST API to retrieve forecast data
```

**Reason이 중요한 이유:**
1. **보안 검토**: 감사자가 선언된 기능이 실제 플러그인 동작과 일치하는지 확인할 수 있음
2. **최소 권한 원칙**: 플러그인 작성자에게 각 기능이 필요한 이유를 설명하도록 강제함
3. **규정 준수**: 보안 인증 및 사건 조사에 필요함
4. **런타임 모니터링**: 보안 도구가 선언된 기능 사용이 명시된 이유를 초과할 경우 경고를 발생시킬 수 있음

## 08-ForbiddenNetwork와의 비교

| 측면 | 08-ForbiddenNetwork | 13-CapabilityNetwork |
|------|-------------------|---------------------|
| 선언 | 없음 | `[PluginCapability(Capability.Network)]` |
| 로드 결과 | ❌ PluginLoader에서 거부 | ✅ 정상적으로 로드 |
| HttpClient 사용 | TypeRef 스캔에서 차단 | 기능 선언으로 면제 |
| TcpClient 사용 | TypeRef 스캔에서 차단 | 기능 선언으로 면제 |
| Reason | 해당 없음 | 감사 로그에 기록 |

**핵심 차이**: 08-ForbiddenNetwork는 기능을 **선언하지 않고** 네트워크 유형을 사용할 때의 결과를 보여줍니다. 13-CapabilityNetwork는 선언적 방식으로 네트워크 액세스를 요청하는 **올바른** 방법을 보여줍니다.

## 보안 모범 사례

1. **필요한 것만 선언**: HTTP만 필요한 경우라고 해서 할 수 있다고 Capability.Network를 선언하지 마세요——단, Capability.Network는 유일한 네트워크 관련 기능이며 더 세분화된 옵션은 없습니다
2. **NetworkExecutor 우선**: `NetworkExecutor`는 네트워크 액세스의 제어된 진입점이며 기능 선언이 필요 없습니다
3. **명확한 Reason 제공**: "네트워크 액세스"와 같은 모호한 이유는 보안 검토에서 위험 신호입니다
4. **선언 불가능한 제한 기억**: P/Invoke, Unsafe, Reflection.Emit 금지를 우회할 수 있는 기능 선언은 없습니다

## 파일

- `Plugin.cs` — Capability.Network를 선언하는 데모 플러그인
- `README.md` — English
- `README.zh-CN.md` — 简体中文
- `README.zh-HK.md` — 繁體中文
- `README.ja-JP.md` — 日本語
- `README.ko-KR.md` — 이 파일 (한국어)
- `README.de-DE.md` — Deutsch
- `README.fr-FR.md` — Français
- `README.es-ES.md` — Español
- `README.it-IT.md` — Italiano
- `README.ru-RU.md` — Русский
- `README.pt-PT.md` — Português
- `README.pl-PL.md` — Polski
- `README.cs-CZ.md` — Čeština

## 관련 예제

- **08-ForbiddenNetwork**: 차단된 네트워크 작업 반례
- **14-CapabilityFileIO**: 선언적 FileIO 기능
- **15-CapabilityProcess**: 선언적 Process 기능
- **16-CapabilityAI**: 선언적 AI 서비스 기능
- **17-CapabilityStacked**: 다중 기능 스택
- **18-CapabilityDenied**: 선언 불가능한 기능 반례
