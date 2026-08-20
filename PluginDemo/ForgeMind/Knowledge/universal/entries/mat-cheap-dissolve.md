# 低成本溶解（Dissolve）效果

## 适用场景

怪物死亡、道具拾取等"逐渐消失"表现。顶点动画方案（World Position Offset 扰动）成本高且需要额外贴图；本方案只做片元裁切，一次纹理采样即可完成。

## 实现方案（材质节点链）

节点链书写约定：`节点A.输出 → 节点B.输入`，材质域为 Surface、Blend Mode 为 Masked。

```
Texture Sample（噪声纹理，如云噪声）.R
  → Scalar Parameter（DissolveAmount, 0~1） Subtract
  → OpacityMask

Texture Sample.R
  → Scalar Parameter（EdgeWidth, 建议 0.05） Subtract 后取反
  → Clamp 0~1
  → Lerp（A=基础色, B=边缘发光色） → Emissive Color（乘 3~5 倍强度）
```

参数说明：

| 参数 | 含义 | 动画方向 |
|---|---|---|
| DissolveAmount | 溶解进度 | 0=完整，1=完全消失 |
| EdgeWidth | 灼烧边缘宽度 | 越小边缘越锐利 |

## 播放驱动

蓝图 Timeline 或 C++ 的 `SetScalarParameterValueOnMaterials` 按时间推进 DissolveAmount；配合 Timer 或 Tick 均可（低频即可，参考蓝图降频课目）。

## 注意事项

1. Masked 材质不写半透明，天然适合粒子不冲突的场景，但边缘锯齿比 Translucent 明显——用较小的 EdgeWidth + 较高噪声纹理分辨率缓解。
2. 噪声纹理不要用默认压缩设置下的极小尺寸（<128），裁切边缘会出现块状伪影。
3. 角色身上使用时，材质需设为"用作蒙皮网格"（Used with Skeletal Meshes），否则编译不通过。

## 验证

材质编辑器内拖动 DissolveAmount 预览；运行时确认 GPU 耗时无明显上升（`stat unit` GPU 行）。
