# PluginDemo-16: Capability.AI — 선언적 AI 서비스 권한

## 개요

이 플러그인은 `[PluginCapability(Capability.AI)]`를 사용하여 플러그인이 AI 서비스에 액세스해야 함을 선언하는 방법을 보여줍니다. 다른 기능과 달리 `Capability.AI`는 금지된 네임스페이스를 면제하지 **않습니다** — 대신 호스트가 플러그인에 `IAIService` 참조를 주입할 수 있게 합니다.

## 핵심 개념: Capability.AI는 네트워크 액세스를 부여하지 않음

`Capability.AI`는 다른 기능과 근본적으로 다릅니다:

| 기능 | 면제 내용 | 작동 방식 |
|------|---------|---------|
| `Capability.Network` | `System.Net.*` 네임스페이스 | TypeRef/ILString 스캔 규칙 완화 |
| `Capability.FileIO` | `System.IO` 네임스페이스 | TypeRef/ILString 스캔 규칙 완화 |
| `Capability.Process` | `Process*` 유형 | TypeRef/ILString 스캔 규칙 완화 |
| `Capability.AI` | **없음** | 호스트에 의한 IAIService 주입 활성화 |

`IAIService`는 `SiliconLife.Collective` 네임스페이스에 있습니다 — 금지 목록에 포함된 적이 없습니다. 기능 선언은 이 플러그인이 AI 서비스 참조를 받아야 함을 호스트에 알리는 **옵트인 신호**입니다.

## 기능 스택: AI + Network

AI 클라이언트가 직접 네트워크 액세스를 필요로 하는 경우(예: 원격 AI 엔드포인트 호출), **두** 기능을 모두 선언해야 합니다:

```csharp
[PluginCapability(Capability.Network, Reason = "Calls remote AI endpoint")]
[PluginCapability(Capability.AI, Reason = "Provides IAIService to downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

전체 스택 예제는 **17-CapabilityStacked**을 참조하십시오.

## 제어된 진입점 패턴

| 리소스 | 제어된 진입점 | 기능 선언 필요 |
|--------|-------------|-------------|
| 파일 | `PermissionedStreamFactory` | 없음 |
| 네트워크 | `NetworkExecutor` | 없음 |
| 프로세스 | `CommandLineExecutor` | 없음 |
| 데이터 저장소 | `SpeedyPack` | 없음 |
| AI 서비스 | `IAIService` | `Capability.AI` |

`IAIService`는 고유합니다: 기능 선언이 **필요**합니다. AI 서비스 액세스는 옵트인 기능이며, 모든 플러그인에서 사용 가능한 기본 기능이 아니기 때문입니다.

## 파일

- `Plugin.cs` — Capability.AI를 선언하는 데모 플러그인
- `README.md` — 이 파일 (영어)
- `README.zh-CN.md` — 중국어 간체
- 번역: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 관련 예제

- **17-CapabilityStacked**: 다중 기능 스택 (Network + AI)
- **18-CapabilityDenied**: 선언 불가 기능 안티패턴
