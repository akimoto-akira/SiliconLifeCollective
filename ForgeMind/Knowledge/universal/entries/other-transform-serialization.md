# Transform 序列化格式要求

## 问题
通过 ForgeMindForUE 插件设置蓝图变量或 Actor 属性时，Transform 类型的值被静默忽略或设置失败。

## 原因
UE 的 Transform 序列化对字段名称和旋转格式有严格要求：

### 强制规则
1. **Rotation 必须为四元数**：不能使用 Rotator（Pitch/Yaw/Roll）
2. **缩放键名必须为 Scale3D**：不是 "Scale"，不是 "Scale3D"，必须是完全匹配的键名

### 支持的格式
```json
{
  "Translation": {"X": 100, "Y": 0, "Z": 50},
  "Rotation": {"X": 0, "Y": 0, "Z": 0, "W": 1},
  "Scale3D": {"X": 1, "Y": 1, "Z": 1}
}
```

或字符串管道格式：
```
"Translation=(X=100,Y=0,Z=50)|Rotation=(X=0,Y=0,Z=0,W=1)|Scale3D=(X=1,Y=1,Z=1)"
```

### 常见错误
- ❌ `"Rotation": {"Pitch": 0, "Yaw": 90, "Roll": 0}` —— Rotator 格式不被接受
- ❌ `"Scale": {"X": 1, "Y": 1, "Z": 1}` —— 键名错误，必须是 Scale3D
- ❌ `"Scale": "1,1,1"` —— 字符串格式不解析

## 解决方案
使用四元数格式设置旋转。若只有欧拉角，先转换为四元数：
- Pitch = 0°, Yaw = 90°, Roll = 0°
- 对应四元数：X=0, Y=0.707, Z=0, W=0.707（约）

## 影响范围
ForgeMindForUE 插件、exec_console 命令、JSON 序列化接口

## 验证日期
2026-08-20
