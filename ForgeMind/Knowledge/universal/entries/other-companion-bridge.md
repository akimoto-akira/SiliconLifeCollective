# ForgeMindForUE：宿主与编辑器联动桥梁

## 适用场景

希望 SiliconLife 宿主（ForgeMind 插件）感知 UE 编辑器状态、并驱动编辑器内操作（打开资产、执行命令、收集运行数据等）。宿主进程无法直接触碰编辑器进程，必须由安装在 UE 侧的伴侣插件充当桥梁。

## 前置条件

1. `ForgeMindForUE.uplugin` 已放入项目 `Plugins/` 目录，或安装在引擎 `Engine/Plugins/` 下（marketplace 方式）。
2. 插件处于**启用**状态：
   - 项目侧：`.uplugin` 的 `EnabledByDefault=true`，或 `.uproject` Plugins 数组声明 `Enabled: true`；
   - 引擎侧：默认 `EnabledByDefault=false`，必须在 `.uproject` 中显式启用。

## 检查方式（宿主侧程序化探测）

调用 `unreal_project` 工具的 `analyze` 动作，读取输出中的 `companionPlugin` 字段：

```json
{ "installed": true, "enabled": false, "location": "project" }
```

| 状态组合 | 含义 | 依赖它的功能 |
|---|---|---|
| installed=false | 未安装 | 全部关闭，先引导安装 |
| installed=true, enabled=false | 装了但未启用 | 全部关闭，引导启用 |
| installed=true, enabled=true | 就绪 | 允许启用 |

## 安装引导（未安装时）

1. 将 `ForgeMindForUE` 文件夹复制到 `{项目目录}/Plugins/`（不存在则创建）。
2. 用编辑器打开项目，在 Edit → Plugins 中确认 ForgeMindForUE 已勾选。
3. 重启编辑器后用 `unreal_project analyze` 复核 `companionPlugin.enabled == true`。

## 注意事项

1. 伴侣插件不存在或被禁用时，任何依赖编辑器联动的功能必须处于关闭态，不得尝试调用。
2. 项目侧副本优先于引擎侧副本（同名遮蔽）；升级时替换项目侧版本即可，无需动引擎目录。
