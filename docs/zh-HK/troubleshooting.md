# 故障排除指南

[English](troubleshooting.md) | [简体中文](docs/zh-CN/troubleshooting.md) | [繁體中文](docs/zh-HK/troubleshooting.md) | [Español](docs/es-ES/troubleshooting.md) | [日本語](docs/ja-JP/troubleshooting.md) | [한국어](docs/ko-KR/troubleshooting.md) | [Čeština](docs/cs-CZ/troubleshooting.md)

## 常見問題

### 构建和編譯

#### 問題：构建失敗，缺少依赖

**症状**：
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**解決方案**：
```bash
dotnet restore
dotnet build
```

#### 問題：未找到 .NET SDK

**症状**：
```
The .NET SDK could not be found
```

**解決方案**：
1. 安裝 .NET 9 SDK：https://dotnet.microsoft.com/download/dotnet/9.0
2. 驗證安裝：
```bash
dotnet --version
```

---

### AI 連接問題

#### 問題：Ollama 連接被拒絕

**症状**：
```
Failed to connect to Ollama at http://localhost:11434
```

**解決方案**：
```bash
# 檢查 Ollama 是否正在執行
ollama list

# 啟動 Ollama
ollama serve

# 測試連接
curl http://localhost:11434/api/tags
```

#### 問題：未找到模型

**症状**：
```
model "qwen2.5:7b" not found
```

**解決方案**：
```bash
# 拉取所需模型
ollama pull qwen2.5:7b

# 列出可用模型
ollama list
```

#### 問題：百炼 404 錯誤

**症状**：
```
HTTP 404: Model not found
```

**解決方案**：
1. 驗證 API 金鑰正确
2. 檢查模型名称與百炼目錄匹配
3. 驗證區域端点正确
4. 檢查帐戶有權訪問该模型

---

### 執行时問題

#### 問題：端口已被占用

**症状**：
```
HttpListenerException: Address already in use
```

**解決方案**：

**Windows**：
```bash
netstat -ano | findstr :8080
taskkill /PID <PID> /F
```

**Linux/Mac**：
```bash
lsof -ti:8080 | xargs kill -9
```

**或更改設定中的端口**。

#### 問題：生命体无法啟動

**症状**：
- 生命体狀態顯示"Error"
- 記錄顯示初始化失敗

**解決方案**：
1. 檢查靈魂檔案存在且有效
2. 驗證 AI 客戶端已設定
3. 檢查記錄以获取具体錯誤：
```bash
tail -f logs/*.log
```

#### 問題：記憶體不足

**症状**：
```
OutOfMemoryException
```

**解決方案**：
1. 增加堆大小：
```bash
dotnet run --server.gcHeapCount 4
```

2. 清理舊資料：
```bash
# 歸档舊記錄
mv logs/ logs-archive/
mkdir logs

# 清理舊記憶
# 通過 Web UI：記憶管理 > 清理
```

---

### 權限問題

#### 問題：權限被拒絕

**症状**：
```
Permission denied: disk:write
```

**解決方案**：
1. 檢查當前權限：
```bash
curl http://localhost:8080/api/permissions
```

2. 授予權限：
```bash
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

3. 或使用 Web UI：權限管理

#### 問題：權限未過期

**症状**：
- 權限在過期時間後仍然有效

**解決方案**：
1. 檢查系統时钟同步
2. 驗證 `expiresAt` 字段設定正确
3. 清除權限快取

---

### Web UI 問題

#### 問題：无法訪問 Web UI

**症状**：
- 瀏覽器顯示"Connection refused"

**解決方案**：
1. 驗證伺服器正在執行
2. 檢查正确 URL：`http://localhost:8080`
3. 檢查防火墙設定
4. 檢查記錄以获取啟動錯誤

#### 問題：SSE 不工作

**症状**：
- 实时更新未出现
- 聊天不流式傳输

**解決方案**：
1. 檢查瀏覽器支援 SSE
2. 禁用 SSE 的代理缓衝
3. 檢查網路穩定性
4. 尝试不同瀏覽器

#### 問題：UI 看起來损壞

**症状**：
- 样式不正确
- 布局损壞

**解決方案**：
1. 清除瀏覽器快取
2. 尝试不同皮肤：設定 > 皮肤
3. 檢查瀏覽器控制臺中的錯誤
4. 禁用瀏覽器擴充

---

### 儲存問題

#### 問題：无法读取/寫入資料

**症状**：
```
IOException: Access denied
```

**解決方案**：
1. 檢查檔案權限
2. 驗證儲存路徑存在
3. 檢查磁碟空间
4. 以适當權限執行

#### 問題：資料损壞

**症状**：
- JSON 解析錯誤
- 資料丟失

**解決方案**：
1. 從備份復原
2. 檢查儲存完整性：
```bash
# 通過 Web UI：系統 > 儲存檢查
```

3. 手動修復损壞的檔案

---

### 工具執行問題

#### 問題：未找到工具

**症状**：
```
Tool "xyz" not found
```

**解決方案**：
1. 驗證工具名称正确
2. 檢查工具在 Tools 目錄中
3. 重新构建項目
4. 檢查工具是否正确實現

#### 問題：工具返回錯誤

**症状**：
```
Tool execution failed: ...
```

**解決方案**：
1. 檢查工具記錄
2. 驗證输入參數
3. 独立測試工具
4. 檢查權限

---

## 偵錯

### 啟用详细記錄

编辑設定：
```json
{
  "logging": {
    "level": "debug"
  }
}
```

### 檢查記錄

記錄儲存在：
```
logs/
├── system.log
├── ai.log
├── permission.log
└── error.log
```

实时查看：
```bash
tail -f logs/*.log
```

### 使用偵錯器

```bash
# 使用偵錯器執行
dotnet run --project src/SiliconLife.Default --configuration Debug

# 附加偵錯器
# 通過 IDE：附加到進程 > SiliconLife.Default
```

---

## 效能問題

### 回應時間慢

**最佳化**：
1. 降低 AI 模型复雜度
2. 啟用快取
3. 清理舊資料
4. 增加系統資源

### CPU 使用率高

**檢查**：
- 執行太多生命体
- 工具中的无限循环
- 频繁的定时器執行

**解決方案**：
- 減少併發生命体
- 最佳化工具程式碼
- 调整定时器间隔

### 記憶體使用率高

**監控**：
```bash
# 通過 Web UI：儀表板 > 記憶體
```

**最佳化**：
- 清理舊記憶
- 減少上下文大小
- 實現分頁

---

## 获取幫助

### 查看文檔

- [快速開始指南](getting-started.md)
- [開發指南](development-guide.md)
- [API 参考](api-reference.md)
- [架構指南](architecture.md)

### 檢查記錄

始终首先檢查記錄以获取錯誤详情。

### 社群支援

- GitHub Issues：報告 bug
- Discussions：提問
- 文檔：搜尋解決方案

---

## 紧急程式

### 系統崩溃

1. 檢查記錄以获取原因
2. 重啟應用程式程式：
```bash
dotnet run --project src/SiliconLife.Default
```

3. 如需從備份復原

### 資料丟失

1. 立即停止應用程式程式
2. 檢查備份檔案
3. 復原資料
4. 驗證完整性

### 安全漏洞

1. 停止所有生命体
2. 撤销所有權限
3. 檢查稽核記錄
4. 查看訪問控制
5. 以限制權限重啟

---

## 預防

### 最佳实践

1. **定期備份**
   - 備份資料目錄
   - 備份設定
   - 測試復原過程

2. **監控資源**
   - 监视 CPU/記憶體使用
   - 監控磁碟空间
   - 檢查網路連接

3. **保持更新**
   - 更新 .NET SDK
   - 更新依赖
   - 應用程式安全修補程式

4. **測試变更**
   - 首先在開發中測試
   - 使用版本控制
   - 記录变更

---

## 下一步

- 📚 阅读[架構指南](architecture.md)
- 🛠️ 查看[開發指南](development-guide.md)
- 🚀 查看[快速開始指南](getting-started.md)
- 🔒 查看[安全文檔](security.md)
