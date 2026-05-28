# CS 源码编译加载模式示例

一个从原始 `.cs` 源码文件而非预编译 DLL 加载的插件，演示 PluginLoader 的 CS 源码编译模式（由 task-389 引入）。

## CS 源码模式工作原理

当 PluginLoader 扫描插件目录时发现**没有 DLL**，它自动进入 CS 源码模式：

```
1. PluginLoader 扫描插件目录 → 无 DLL
2. 进入 CS 源码模式
3. 发现 cs.txt → 按行读取，只加载列出的 .cs 文件
   （无 cs.txt → 加载目录下所有 *.cs 文件）
4. 扫描同级 DLL → 受信 DLL 直接添加为引用；
   非受信 DLL 需通过 ScanForbiddenReferences 安全扫描才能作为引用
5. CompilationCore（受限模式）编译 .cs 文件为内存 DLL
6. 内存 DLL 字节写入临时文件，走 ScanForbiddenReferences 安全扫描
7. 扫描通过 → 反射查找 IPlugin 实现 → 实例化
8. 日志显示："Plugin loaded [CS-Source]: {Id} v{Version} from {DirName}"
```

## cs.txt — 选择性加载白名单

`cs.txt` 文件指定要编译哪些 `.cs` 文件，每行一个文件名：

```
Plugin.cs
```

- **列出的文件**：被编译和加载（如 `Plugin.cs`）
- **未列出的文件**：编译器忽略（如 `Helpers.cs`）
- **以 `#` 开头的行**：视为注释
- **空行**：忽略
- **没有 cs.txt**：加载目录下所有 `*.cs` 文件

本示例故意在 cs.txt 中只列出 `Plugin.cs`。`Helpers.cs` 存在于目录中但**不**参与编译，用于演示选择性加载机制。

## 目录结构

```
22-CSSourceMode/
├── Plugin.cs          — IPlugin 实现（在 cs.txt 中 → 被编译）
├── Helpers.cs         — 辅助类（不在 cs.txt 中 → 不被编译）
├── cs.txt             — 白名单，指定编译哪些 .cs 文件
├── README.md          — 英文文档
├── README.zh-CN.md    — 中文文档
└── ...                — 其他语言 README
```

## CS 源码模式 vs DLL 模式

| 方面 | DLL 模式 | CS 源码模式 |
|------|---------|------------|
| 插件格式 | 预编译 `.dll` | 原始 `.cs` 源码文件 |
| 加载触发 | 插件目录中找到 DLL | 无 DLL，但有 `.cs` 文件 |
| 编译时机 | 构建时 | 加载时由 PluginLoader 编译 |
| 性能 | 无编译开销 | 启动时有 Roslyn 编译开销 |
| 安全扫描 | 直接 PE 元数据扫描 | 编译 → 临时 DLL → PE 元数据扫描 |
| 日志前缀 | `Plugin loaded:` | `Plugin loaded [CS-Source]:` |
| 适用场景 | 生产部署 | 开发迭代 |

## 与 01-MinimalPlugin 的对比

| 方面 | 01-MinimalPlugin | 22-CSSourceMode |
|------|------------------|-----------------|
| 插件格式 | 预编译 DLL | CS 源码文件 |
| 加载路径 | 标准 DLL 加载 | CS 源码编译 |
| 功能 | 等价 | 等价 |
| IPlugin.Id | `com.siliconlife.demo.minimal` | `com.siliconlife.demo.cssource` |

## 错误处理

| 场景 | 行为 |
|------|------|
| 无 DLL，无 .cs 文件 | 警告："No DLL and no CS source files found" |
| 编译错误 | 错误：记录详细诊断信息 |
| 安全扫描失败 | 错误：列出所有违规项，插件被拒绝 |
| cs.txt 条目未找到 | 警告："cs.txt entry not found or not a .cs file" |
| 同级 DLL 扫描失败 | 警告：DLL 不作为引用，编译继续 |

## 安全说明

CS 源码模式插件接受与 DLL 模式插件**相同的安全扫描**。编译后的程序集被写入临时 DLL 文件，使用 `ScanForbiddenReferences` 扫描——与预编译 DLL 接受的扫描完全相同。所有禁止的命名空间/类型/成员/字符串规则同等适用。

插件仍在隔离的上下文中加载，并扫描禁止的命名空间引用（如 `System.IO`、`System.Net.Http`）。详见[安全文档](../../docs/zh-CN/security.md)。
