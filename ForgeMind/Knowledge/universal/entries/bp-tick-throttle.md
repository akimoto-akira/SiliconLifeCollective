# 蓝图 Tick 降频：用 Timer 替代每帧逻辑

## 适用场景

蓝图 `Event Tick` 中执行"并不需要每帧做"的逻辑：周期性检测、状态轮询、距离判断等。蓝图节点本身解释执行成本高于 C++，每帧执行的节点链是常见的蓝图性能坑。

## 实现方案（蓝图节点链）

节点链书写约定：`节点A → 节点B（关键参数）`，引脚连接在括号内注明。

```
Event BeginPlay
  → Set Timer by Function Name（Function Name=ThrottledUpdate, Time=0.2, Looping=true）

Function ThrottledUpdate
  → （原 Tick 中的全部逻辑移到这里）

Event EndPlay
  → Clear Timer by Function Name（Function Name=ThrottledUpdate）
```

原 `Event Tick` 连线全部断开；若蓝图中无其他每帧需求，`Class Defaults → Start with Tick Enabled` 可一并关闭。

## 频率选择参考

| 逻辑类型 | 建议 Time |
|---|---|
| UI 状态刷新 | 0.1 ~ 0.25 |
| AI 感知轮询 | 0.2 ~ 0.5 |
| 低频环境检查 | 1.0+ |

## 注意事项

1. 确需每帧的逻辑（平滑插值、物理跟随）不要用 Timer 硬凑，保留 Tick 但减少节点数量、把热点逻辑下沉到 C++。
2. 多个循环 Timer 共存时，用 `Retriggerable Delay` 反而会造成漂移，坚持用 Set Timer。
3. Timer 绑定的是函数名（字符串），重命名函数后需同步更新节点参数，否则静默失效。

## 验证

改造前后分别运行并观察 `stat game` 中蓝图执行耗时；或用蓝图 Profiler（Blueprint → Blueprint Profiler）对比节点执行次数。
