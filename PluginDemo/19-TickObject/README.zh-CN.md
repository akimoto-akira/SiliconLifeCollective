# PluginDemo-19: TickObject — MainLoop 中的定时任务

## 概述

本插件演示如何使用 `TickObject` 与 `MainLoop` 集成来实现定时/持续逻辑。TickObject 是可被 MainLoop 主循环 tick 的对象基类，提供了 `System.Threading.Timer` 或 `Task.Delay` 的统一替代方案。

## TickObject 生命周期

```
Constructor(interval, autoRegister)
    │
    ├── autoRegister=true → 构造时自动调用 MainLoop.Register(this)
    │
    ├── autoRegister=false → 稍后手动调用 MainLoop.Register(this)
    │
    ▼
MainLoop.Tick() 循环
    │
    ├── 按 Priority 升序排序所有已注册的 TickObject
    ├── 为每个 TickObject 累积 elapsedTime
    ├── 如果 elapsedTime >= Interval → 调用 OnTick(deltaTime)
    │
    ├── 熔断器：如果 OnTick 超过 TickTimeout → 增加超时计数
    │   └── 连续 maxTimeoutCount 次超时后 → 1 分钟冷却期
    │
    ▼
MainLoop.Unregister(tickObject) — 在 OnStop 中清理
```

## 关键属性

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `Interval` | `TimeSpan` | 必需 | OnTick 调用间隔 |
| `Priority` | `int` | 100 | 执行顺序（数值越小优先级越高） |
| `autoRegister` | `bool` | `true` | 是否在构造时自动注册到 MainLoop |

## 关键方法

| 方法 | 说明 |
|------|------|
| `OnTick(TimeSpan deltaTime)` | 重写以实现定时逻辑 |
| `MainLoop.Register(TickObject)` | 手动注册到 MainLoop |
| `MainLoop.Unregister(TickObject)` | 从 MainLoop 移除（清理） |

## 演示场景

### 1. 基本定时器（autoRegister=true）
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

### 2. 手动注册（autoRegister=false）
```csharp
// 在构造函数中：不自动注册
_heartbeatTimer = new HeartbeatTimer(autoRegister: false);

// 在 OnStart 中：手动注册
MainLoop.Register(_heartbeatTimer);
```

### 3. 优先级排序
- `Priority = 10` → 高优先级，优先执行
- `Priority = 200` → 低优先级，后执行

### 4. 清理
```csharp
// 在 OnStop 中：始终取消注册以防泄漏
MainLoop.Unregister(_statusTimer);
```

## MainLoop 熔断器

MainLoop 内置熔断器，防止慢 TickObject 阻塞整个循环：

1. 如果 `OnTick` 超过 `TickTimeout`（默认 1 秒）→ 超时计数增加
2. 连续 `maxTimeoutCount`（默认 3）次超时 → 熔断器跳闸
3. 被熔断的 TickObject **跳过** 1 分钟冷却期
4. 冷却后，TickObject 重新获得一次执行机会

## TickObject vs System.Threading.Timer

| 方面 | TickObject + MainLoop | System.Threading.Timer |
|------|----------------------|----------------------|
| 线程模型 | 单主循环线程 | 线程池线程 |
| 执行顺序 | 确定性（按 Priority） | 非确定性 |
| 熔断器 | 内置 | 无 |
| 调试 | 容易（单线程） | 困难（竞态条件） |
| 资源占用 | 最小（无线程池） | 线程池开销 |
| 间隔精度 | 尽力而为（受其他 TickObject 影响） | 更精确 |

## 安全说明

TickObject 本身**不需要**任何能力声明，是安全的内置框架机制。

## 文件

- `Plugin.cs` — 演示 TickObject 的插件
- `README.md` — 本文件（英文）
- `README.zh-CN.md` — 简体中文
- 翻译版本：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 相关示例

- **13-CapabilityNetwork**：Capability.Network 声明
- **20-SpeedyPack**：无需 Capability.FileIO 的数据存储
