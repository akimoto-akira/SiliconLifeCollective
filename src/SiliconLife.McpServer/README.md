# SiliconLife.McpServer

浏览器能力开放服务器，为 AI IDE（Cursor、Trae 等）提供浏览器自动化工具。

## 功能

通过 HTTP API 暴露以下浏览器操作能力：

| API | 方法 | 说明 |
|-----|------|------|
| `/api/browser/navigate` | POST | 导航到 URL |
| `/api/browser/click` | POST | 点击元素 |
| `/api/browser/input` | POST | 输入文本 |
| `/api/browser/get_page_text` | GET | 获取页面文本 |
| `/api/browser/screenshot` | POST | 截图 |
| `/api/browser/evaluate` | POST | 执行 JavaScript |
| `/api/browser/wait_for_element` | POST | 等待元素出现 |
| `/api/browser/status` | GET | 获取浏览器状态 |
| `/api/browser/close` | POST | 关闭浏览器 |
| `/api/browser/run_test` | POST | 运行测试脚本 |

## 启动

```bash
cd SiliconLifeCollective/src/SiliconLife.McpServer
dotnet run
```

服务器启动在 `http://localhost:5001`

## API 使用示例

### 导航到页面

```bash
curl -X POST http://localhost:5001/api/browser/navigate \
  -H "Content-Type: application/json" \
  -d '{"url": "https://example.com"}'
```

### 点击元素

```bash
curl -X POST http://localhost:5001/api/browser/click \
  -H "Content-Type: application/json" \
  -d '{"selector": "#submit-button"}'
```

### 执行 JavaScript

```bash
curl -X POST http://localhost:5001/api/browser/evaluate \
  -H "Content-Type: application/json" \
  -d '{"script": "document.title"}'
```

### 截图

```bash
curl -X POST http://localhost:5001/api/browser/screenshot \
  -H "Content-Type: application/json" \
  -d '{"fullPage": true}'
```

## AI IDE 集成

### Cursor 配置

在 Cursor 中配置 HTTP 工具调用，或使用以下脚本：

```javascript
// Cursor 脚本示例
async function browserNavigate(url) {
  const response = await fetch('http://localhost:5001/api/browser/navigate', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ url })
  });
  return await response.json();
}
```

### Trae 配置

类似地，在 Trae 中配置 HTTP 请求工具。

## 架构

```
┌─────────────────┐      HTTP       ┌──────────────────────┐
│  AI IDE         │ ←────────────→  │  SiliconLife.McpServer│
│  (Cursor/Trae)  │                 │  - Playwright WebView │
│                 │                 │  - Browser Pool       │
└─────────────────┘                 └──────────────────────┘
```

## 注意事项

1. **无权限控制**：这是能力开放服务，信任本地环境
2. **单浏览器实例**：所有调用共享同一个浏览器实例
3. **无状态**：每次调用独立，需要自己管理会话状态

## 开发

添加新工具：
1. 在 `Program.cs` 中添加新的 Map 端点
2. 使用 `IBrowserPool.GetWebViewAsync()` 获取 WebView 实例
3. 调用 `PlaywrightWebView` 的方法

## 许可证

Apache License 2.0
