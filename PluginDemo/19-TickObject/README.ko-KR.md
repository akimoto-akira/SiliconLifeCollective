# PluginDemo-19: TickObject — MainLoop의 주기적 작업

## 개요

이 플러그인은 `TickObject`를 사용하여 `MainLoop`와 통합하여 주기적/지속적 로직을 구현하는 방법을 보여줍니다. TickObject는 MainLoop의 메인 루프에 의해 tick될 수 있는 객체의 기본 클래스로, `System.Threading.Timer` 또는 `Task.Delay`의 통합 대안을 제공합니다.

## TickObject 수명 주기

```
Constructor(interval, autoRegister)
    │
    ├── autoRegister=true → 생성자에서 MainLoop.Register(this) 자동 호출
    │
    ├── autoRegister=false → 나중에 수동으로 MainLoop.Register(this) 호출
    │
    ▼
MainLoop.Tick() 루프
    │
    ├── 등록된 모든 TickObject를 Priority 오름차순으로 정렬
    ├── 각 TickObject의 elapsedTime 누적
    ├── elapsedTime >= Interval → OnTick(deltaTime) 호출
    │
    ├── 서킷 브레이커: OnTick이 TickTimeout 초과 → 타임아웃 카운트 증가
    │   └── maxTimeoutCount회 연속 타임아웃 후 → 1분 쿨다운
    │
    ▼
MainLoop.Unregister(tickObject) — OnStop에서 정리
```

## 주요 속성

| 속성 | 유형 | 기본값 | 설명 |
|------|------|--------|------|
| `Interval` | `TimeSpan` | 필수 | OnTick 호출 간격 |
| `Priority` | `int` | 100 | 실행 순서 (값이 작을수록 높은 우선순위) |
| `autoRegister` | `bool` | `true` | 생성자에서 MainLoop에 자동 등록 여부 |

## 주요 메서드

| 메서드 | 설명 |
|--------|------|
| `OnTick(TimeSpan deltaTime)` | 재정의하여 주기적 로직 구현 |
| `MainLoop.Register(TickObject)` | MainLoop에 수동 등록 |
| `MainLoop.Unregister(TickObject)` | MainLoop에서 제거 (정리) |

## 데모 시나리오

### 1. 기본 타이머 (autoRegister=true)
```csharp
public class StatusTimer : TickObject
{
    public StatusTimer() : base(interval: TimeSpan.FromSeconds(5), autoRegister: true)
    {
        Priority = 100;
    }

    protected override void OnTick(TimeSpan deltaTime)
    {
        Console.WriteLine($"Tick, deltaTime={deltaTime.TotalMilliseconds:F0}ms");
    }
}
```

### 2. 수동 등록 (autoRegister=false)
```csharp
// 생성자에서: 자동 등록하지 않음
_heartbeatTimer = new HeartbeatTimer(autoRegister: false);

// OnStart에서: 수동 등록
MainLoop.Register(_heartbeatTimer);
```

### 3. 우선순위 순서
- `Priority = 10` → 높은 우선순위, 먼저 실행
- `Priority = 200` → 낮은 우선순위, 나중에 실행

### 4. 정리
```csharp
// OnStop에서: 누수 방지를 위해 항상 등록 해제
MainLoop.Unregister(_statusTimer);
```

## MainLoop 서킷 브레이커

MainLoop에는 느린 TickObject가 전체 루프를 차단하는 것을 방지하는 내장 서킷 브레이커가 있습니다:

1. `OnTick`이 `TickTimeout`(기본 1초)을 초과하면 → 타임아웃 카운트 증가
2. `maxTimeoutCount`(기본 3)회 연속 타임아웃 → 서킷 브레이커 작동
3. 작동한 TickObject는 1분 쿨다운 동안 **건너뜀**
4. 쿨다운 후 TickObject는 다시 실행 기회를 얻음

## TickObject vs System.Threading.Timer

| 측면 | TickObject + MainLoop | System.Threading.Timer |
|------|----------------------|----------------------|
| 스레드 모델 | 단일 메인 루프 스레드 | 스레드 풀 스레드 |
| 실행 순서 | 결정적 (Priority 순) | 비결정적 |
| 서킷 브레이커 | 내장 | 없음 |
| 디버깅 | 용이 (단일 스레드) | 어려움 (경쟁 상태) |
| 리소스 사용 | 최소 (스레드 풀 없음) | 스레드 풀 오버헤드 |
| 간격 정확도 | 베스트에포트 (다른 TickObject의 영향을 받음) | 더 정확함 |

## 보안 참고

TickObject 자체는 기능 선언이 **불필요**합니다. 안전한 내장 프레임워크 메커니즘입니다.

## 파일

- `Plugin.cs` — TickObject 데모 플러그인
- `README.md` — 이 파일 (영어)
- `README.zh-CN.md` — 중국어 간체
- 번역: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 관련 예제

- **13-CapabilityNetwork**: Capability.Network 선언
- **20-SpeedyPack**: Capability.FileIO 불필요한 데이터 스토리지
