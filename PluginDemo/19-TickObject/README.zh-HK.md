# PluginDemo-19：TickObject — MainLoop 中的定時任務

## 概述

本插件演示如何使用 `TickObject` 與 `MainLoop` 整合來實現定時/持續邏輯。TickObject 是可被 MainLoop 主迴圈 tick 的物件基類，提供了 `System.Threading.Timer` 或 `Task.Delay` 的統一替代方案。

## TickObject 生命週期

```
Constructor(interval, autoRegister)
    │
    ├── autoRegister=true → 建構時自動呼叫 MainLoop.Register(this)
    │
    ├── autoRegister=false → 稍後手動呼叫 MainLoop.Register(this)
    │
    ▼
MainLoop.Tick() 迴圈
    │
    ├── 按 Priority 升序排序所有已註冊的 TickObject
    ├── 為每個 TickObject 累積 elapsedTime
    ├── 如果 elapsedTime >= Interval → 呼叫 OnTick(deltaTime)
    │
    ├── 熔斷器：如果 OnTick 超過 TickTimeout → 增加超時計數
    │   └── 連續 maxTimeoutCount 次超時後 → 1 分鐘冷卻期
    │
    ▼
MainLoop.Unregister(tickObject) — 在 OnStop 中清理
```

## 關鍵屬性

| 屬性 | 型別 | 預設值 | 說明 |
|------|------|--------|------|
| `Interval` | `TimeSpan` | 必需 | OnTick 呼叫間隔 |
| `Priority` | `int` | 100 | 執行順序（數值越小優先級越高） |
| `autoRegister` | `bool` | `true` | 是否在建構時自動註冊到 MainLoop |

## 關鍵方法

| 方法 | 說明 |
|------|------|
| `OnTick(TimeSpan deltaTime)` | 覆寫以實現定時邏輯 |
| `MainLoop.Register(TickObject)` | 手動註冊到 MainLoop |
| `MainLoop.Unregister(TickObject)` | 從 MainLoop 移除（清理） |

## 演示場景

### 1. 基本定時器（autoRegister=true）
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

### 2. 手動註冊（autoRegister=false）
```csharp
// 在建構函式中：不自動註冊
_heartbeatTimer = new HeartbeatTimer(autoRegister: false);

// 在 OnStart 中：手動註冊
MainLoop.Register(_heartbeatTimer);
```

### 3. 優先級排序
- `Priority = 10` → 高優先級，優先執行
- `Priority = 200` → 低優先級，後執行

### 4. 清理
```csharp
// 在 OnStop 中：始終取消註冊以防洩漏
MainLoop.Unregister(_statusTimer);
```

## MainLoop 熔斷器

MainLoop 內建熔斷器，防止慢 TickObject 阻塞整個迴圈：

1. 如果 `OnTick` 超過 `TickTimeout`（預設 1 秒）→ 超時計數增加
2. 連續 `maxTimeoutCount`（預設 3）次超時 → 熔斷器跳閘
3. 被熔斷的 TickObject **跳過** 1 分鐘冷卻期
4. 冷卻後，TickObject 重新獲得一次執行機會

## TickObject vs System.Threading.Timer

| 方面 | TickObject + MainLoop | System.Threading.Timer |
|------|----------------------|----------------------|
| 執行緒模型 | 單主迴圈執行緒 | 執行緒池執行緒 |
| 執行順序 | 確定性（按 Priority） | 非確定性 |
| 熔斷器 | 內建 | 無 |
| 除錯 | 容易（單執行緒） | 困難（競態條件） |
| 資源佔用 | 最小（無執行緒池） | 執行緒池開銷 |
| 間隔精度 | 盡力而為（受其他 TickObject 影響） | 更精確 |

## 安全說明

TickObject 本身**不需要**任何能力聲明，是安全的內建框架機制。

## 檔案

- `Plugin.cs` — 演示 TickObject 的插件
- `README.md` — 本檔案（英文）
- `README.zh-CN.md` — 簡體中文
- 翻譯版本：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 相關示例

- **13-CapabilityNetwork**：Capability.Network 聲明
- **20-SpeedyPack**：無需 Capability.FileIO 的資料儲存
