# 最简插件示例

一个最简的 `IPlugin` 实现，使用硬编码值演示插件生命周期。

## IPlugin 接口全貌

每个 SiliconLife 插件必须实现 `SiliconLife.Collective` 中定义的 `IPlugin` 接口：

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### 属性说明

| 成员 | 类型 | 说明 |
|------|------|------|
| `Id` | `string` | 唯一标识符，跨版本必须稳定（如 `"com.siliconlife.demo.minimal"`） |
| `GetName(Language)` | `string` | 人类可读的显示名称，通过 `Language` 枚举本地化 |
| `Version` | `string` | 语义化版本字符串（如 `"1.0.0"`） |
| `GetDescription(Language)` | `string` | 插件功能的简短描述 |
| `GetAuthor(Language)` | `string` | 作者或组织名称 |

## 生命周期调用顺序

宿主按严格顺序调用生命周期方法：

```
OnLoad → OnStart → [运行中] → OnStop → OnUnload
```

| 方法 | 调用时机 | 典型用途 |
|------|---------|---------|
| `OnLoad()` | 插件 DLL 加载到宿主进程时调用一次 | 验证配置、注册类型、准备资源 |
| `OnStart()` | 宿主完全启动且所有插件已加载后调用 | 与其他插件交互、启动后台任务 |
| `OnStop()` | 宿主优雅关闭时调用 | 释放资源、刷新缓冲区、保存状态 |
| `OnUnload()` | 插件从宿主进程卸载时调用 | 最终清理 |

## 本示例

本插件所有属性返回硬编码值，生命周期方法为空，是插件开发的最简起点。

## 安全说明

插件在隔离的 `AssemblyLoadContext` 中加载，并扫描禁止的命名空间引用（如 `System.IO`、`System.Net.Http`）。详见[安全文档](../../docs/zh-CN/security.md)。
