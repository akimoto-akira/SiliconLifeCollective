# PluginDemo-17: 기능 스태킹 — 여러 선언적 권한

## 개요

이 플러그인은 단일 플러그인 클래스에 여러 `[PluginCapability]` 속성을 스택하는 방법을 보여줍니다. `PluginCapabilityAttribute`는 `AllowMultiple = true`이므로 필요한 만큼 많은 기능을 선언할 수 있습니다.

## 스태킹 구문

```csharp
[PluginCapability(Capability.Network, Reason = "API endpoint access for remote AI models")]
[PluginCapability(Capability.AI, Reason = "AI service provider for downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

## PluginLoader가 스택된 기능을 처리하는 방법

1. PE 메타데이터 CustomAttribute 테이블에서 **모든** 선언을 읽음
2. 선언된 모든 기능의 면제 규칙을 **병합**
3. 각 선언을 고유한 Reason 필드와 함께 **독립적으로 기록**
4. 스태킹에 관계없이 선언 불가능한 기능 금지를 **계속 강제**

## 병합된 면제 규칙

`Capability.Network` + `Capability.AI` 스태킹 시:

| 소스 | 면제 내용 |
|------|---------|
| Capability.Network | System.Net.Http.*, System.Net.WebSockets.*, System.Net.Sockets.*, System.Net.Mail.*, System.Net.NetworkInformation.*, System.Net.Security.*, System.Net (유형별 금지) |
| Capability.AI | IAIService 주입 활성화 |
| **병합 결과** | 플러그인이 HttpClient와 IAIService를 모두 사용 가능 |

## 스태킹은 무제한 권한을 부여하지 않음

여러 기능을 스택해도 다음은 **항상 차단**됩니다:

- ❌ P/Invoke (`DllImport`, `Marshal`, `NativeMemory`)
- ❌ 안전하지 않은 코드 (`UnverifiableCodeAttribute`, `Unsafe`)
- ❌ IL 방출 (`System.Reflection.Emit.*`)
- ❌ 어셈블리 로딩 (`System.Runtime.Loader`, `Assembly.Load*`)
- ❌ 레지스트리 (`Microsoft.Win32.*`)

이들에 해당하는 `Capability` 열거형 값은 존재하지 않습니다 — 설계상 **선언 불가능**합니다.

## 스택된 기능의 감사 추적

각 기능은 독립적으로 기록됩니다:

```
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.Network — reason: API endpoint access for remote AI models
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.AI — reason: AI service provider for downstream plugins
```

## 파일

- `Plugin.cs` — Capability.Network + Capability.AI 스택 데모 플러그인
- `README.md` — 이 파일 (영어)
- `README.zh-CN.md` — 중국어 간체
- 번역: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 관련 예제

- **13-CapabilityNetwork**: 단일 Network 기능
- **16-CapabilityAI**: 단일 AI 기능
- **18-CapabilityDenied**: 선언 불가 기능 안티패턴
