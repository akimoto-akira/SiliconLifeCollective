# stat unit 快速定位性能瓶颈

## 适用场景

帧率不达标但不知道从哪查起。`stat unit` 是 UE 性能排查的第一步：一屏数字告诉你瓶颈在游戏线程、绘制线程还是 GPU。

## 操作

控制台（`~` 键）输入：

```
stat unit
```

输出四列毫秒数：

| 列 | 含义 | 高时说明 |
|---|---|---|
| Frame | 总帧时间 | 对照目标帧率（16.6ms=60fps） |
| Game | 游戏线程（玩法/蓝图/物理） | 逻辑太重 → `stat game` 深入 |
| Draw | 绘制线程（提交绘制指令） | DrawCall 太多 → 合批/减 actor |
| GPU | 显卡实际耗时 | 渲染太重 → `stat scenerendering` |
| RHIT | RHI 线程 | 指令提交阻塞，多见于 DX11 老驱动 |

判定规则：**哪一列的数字最接近 Frame，哪一列就是瓶颈**。

## 后续动作

- Game 高 → `stat game` 找 Top 函数；蓝图热点用 Blueprint Profiler。
- Draw 高 → 检查 `stat rhi` 的三角形数；合并网格、剔除远处物体、降 HLOD。
- GPU 高 → `stat scenerendering` 看阴影/光照/后处理占比；逐项关对照。

## 注意事项

1. Shipping 打包默认关闭 stat 命令，排查要用 Development 打包或编辑器内运行。
2. 编辑器内帧率包含编辑器自身渲染开销，以打包后数据为准。
3. 数字波动大时看 30 秒均值，不要盯单帧。
