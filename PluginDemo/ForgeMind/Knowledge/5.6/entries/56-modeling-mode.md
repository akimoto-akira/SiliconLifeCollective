# UE 5.6 启用内置建模工具 ModelingToolsEditorMode

> 版本限定：本课目针对 UE 5.6。其他版本的内置建模插件名称或行为可能不同，请勿跨版本套用。

## 适用场景

不借助外部 DCC（Blender/Maya）直接在编辑器内做白盒建模、网格修改、布尔运算与 UV 调整。UE 5.6 将该能力收敛在引擎插件 `ModelingToolsEditorMode` 中，默认不一定启用。

## 启用方式（二选一）

### 方式 A：编辑器插件面板

1. Edit → Plugins → 搜索 "Modeling"。
2. 勾选 Modeling Tools Editor Mode。
3. 重启编辑器。

### 方式 B：直接写 .uproject

在 `.uproject` 的 `Plugins` 数组加入：

```json
{
	"Name": "ModelingToolsEditorMode",
	"Enabled": true
}
```

下次打开项目即生效——这也是 ForgeMind 测试项目 `ForgeMind_5_6` 采用的方式。

## 使用

编辑器左上角模式选择器（默认显示 Select）切换为 **Modeling**，顶部出现建模工具栏：

| 分组 | 常用工具 |
|---|---|
| Shapes | Box / Cylinder / Cone 快速白盒 |
| Mesh Ops | Extrude / Inset / Bevel 网格编辑 |
| Booleans | Union / Difference / Intersection |
| UVs | 展开与投影调整 |

## 注意事项

1. 建模结果直接生成 Static Mesh 资产，注意及时保存到合适的目录，白盒散落在 Maps 目录下难以管理。
2. 布尔运算在大网格上偶发失败，失败时先简化网格或分块运算。
3. 该插件仅编辑器可用，打包产物不包含建模功能，不影响运行时。

## 验证

用 `unreal_project analyze` 检查输出的 plugins 列表包含 `ModelingToolsEditorMode` 且处于启用状态。
