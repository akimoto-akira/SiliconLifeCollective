# 故障排除指南

> **版本：v0.2.0-alpha**

[English](../en/troubleshooting.md) | [Deutsch](../de-DE/troubleshooting.md) | [简体中文](../zh-CN/troubleshooting.md) | **繁體中文** | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | [Čeština](../cs-CZ/troubleshooting.md) | [Русский](../ru-RU/troubleshooting.md)

## 常見問題

### 建構和編譯

#### 問題：建構失敗，缺少依賴

**症狀**：
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**解決方案**：
```bash
dotnet restore
dotnet build
```

#### 問題：未找到 .NET SDK

**症狀**：
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

**症狀**：
```
Failed to connect to Ollama at http://localhost:11434
```

**解決方案**：
```bash
# 檢查 Ollama 是否正在運行
ollama list

# 啟動 Ollama
ollama serve

# 測試連接
curl http://localhost:11434/api/tags
```

#### 問題：未找到模型

**症狀**：
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

#### 問題：百鍊 404 錯誤

**症狀**：
```
HTTP 404: Model not found
```

**解決方案**：
1. 驗證 API 金鑰正確
2. 檢查模型名稱與百鍊目錄匹配
3. 驗證區域端點正確
4. 檢查帳戶有權存取該模型

#### 問題：火山引擎 Ark 連接失敗

**症狀**：
```
HTTP 401: Unauthorized
或
HTTP 404: Endpoint not found
```

**解決方案**：
1. 驗證 API 金鑰正確
2. 檢查 Endpoint URL 格式正確（預設：`https://ark.cn-beijing.volces.com/api/v3/chat/completions`）
3. 確認 Model 參數使用推理接入點 ID（例如 `ep-20241212123456-abcde`），而非模型名稱
4. 檢查帳戶有權存取該接入點

---

### 運行時問題

#### 問題：埠已被佔用

**症狀**：
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

**或更改設定中的埠**。

#### 問題：生命體無法啟動

**症狀**：
- 生命體狀態顯示"Error"
- 日誌顯示初始化失敗

**解決方案**：
1. 檢查靈魂檔案存在且有效
2. 驗證 AI 用戶端已設定
3. 檢查日誌以獲取具體錯誤：
```bash
tail -f logs/*.log
```

#### 問題：記憶體不足

**症狀**：
```
OutOfMemoryException
```

**解決方案**：
1. **SiliconLife.Default**：增加堆疊大小：
```bash
dotnet run --project src/SiliconLife.Default --server.gcHeapCount 4
```

2. **SiliconLife.Fast**：Fast 版本本身記憶體佔用較高（~500MB），如記憶體持續不足，建議：
   - 減少並發矽基生命體數量
   - 清理舊資料釋放記憶體

3. 清理舊資料：
```bash
# 歸檔舊日誌
mv logs/ logs-archive/
mkdir logs

# 清理舊記憶
# 透過 Web UI：記憶管理 > 清理
```

> **提示**：SiliconLife.Default 記憶體佔用較低（~200MB），適合記憶體受限環境；SiliconLife.Fast 記憶體佔用較高但效能更好，適合生產環境。

---

### 權限問題

#### 問題：權限被拒絕

**症狀**：
```
Permission denied: FileAccess C:\Windows
```

**解決方案**：
1. 檢查當前權限：
```bash
curl http://localhost:8080/api/permissions/list
```

2. 授予權限：
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "permissionType": "FileAccess",
    "resourcePrefix": "C:\\Projects",
    "result": "Allowed",
    "description": "Allow project directory access"
  }'
```

3. 或使用 Web UI：權限管理

#### 問題：權限未過期

**症狀**：
- 權限在過期時間後仍然有效

**解決方案**：
1. 檢查系統時鐘同步
2. 驗證 `expiresAt` 欄位設定正確
3. 清除權限快取

---

### Web UI 問題

#### 問題：無法存取 Web UI

**症狀**：
- 瀏覽器顯示"Connection refused"

**解決方案**：
1. 驗證伺服器正在運行
2. 檢查正確 URL：`http://localhost:8080`
3. 檢查防火牆設定
4. 檢查日誌以獲取啟動錯誤

#### 問題：SSE 不工作

**症狀**：
- 即時更新未出現
- 聊天不串流傳輸

**解決方案**：
1. 檢查瀏覽器支援 SSE
2. 停用 SSE 的代理緩衝
3. 檢查網路穩定性
4. 嘗試不同瀏覽器

#### 問題：UI 看起來損壞

**症狀**：
- 樣式不正確
- 佈局損壞

**解決方案**：
1. 清除瀏覽器快取
2. 嘗試不同面板：設定 > 面板
3. 檢查瀏覽器主控台中的錯誤
4. 停用瀏覽器擴充功能

---

### 儲存問題

#### 問題：無法讀取/寫入資料

**症狀**：
```
IOException: Access denied
```

**解決方案**：
1. 檢查檔案權限
2. 驗證儲存路徑存在
3. 檢查磁碟空間
4. 以適當權限運行

#### 問題：資料損壞

**症狀**：
- JSON 解析錯誤
- 資料遺失

**解決方案**：
1. 從備份還原
2. 檢查儲存完整性：
```bash
# 透過 Web UI：系統 > 儲存檢查
```

3. 手動修復損壞的檔案

#### 問題：SpeedyPack 儲存檔案損壞（Fast 版本）

**症狀**：
- `.spk` 檔案無法載入
- SpeedyStorage 初始化失敗

**解決方案**：
1. 使用 `SiliconLife.Speedy.Manager` 工具檢查和修復 `.spk` 檔案
2. 檢查 `.spk.idx` 索引檔案是否與 `.spk` 檔案匹配
3. 如果索引檔案損壞，刪除 `.spk.idx` 檔案，系統會自動重建索引
4. 從備份還原 `.spk` 檔案

#### 問題：SpeedyPack 自動壓縮失敗（Fast 版本）

**症狀**：
- `.spk` 檔案持續增長
- 磁碟空間不足

**解決方案**：
1. 檢查 `SpeedyPackAutoCompactor` 是否正常運行
2. 手動觸發壓縮操作
3. 檢查壓縮閾值設定
4. 使用 `SiliconLife.Speedy.Manager` 工具手動壓縮

---

### 工具執行問題

#### 問題：未找到工具

**症狀**：
```
Tool "xyz" not found
```

**解決方案**：
1. 驗證工具名稱正確
2. 檢查工具在 Tools 目錄中
3. 重新建構專案
4. 檢查工具是否正確實作

#### 問題：工具回傳錯誤

**症狀**：
```
Tool execution failed: ...
```

**解決方案**：
1. 檢查工具日誌
2. 驗證輸入參數
3. 獨立測試工具
4. 檢查權限

---

### 外掛程式問題

#### 問題：外掛程式載入失敗

**症狀**：
```
Plugin load failed: Security check failed
```

**解決方案**：
1. 檢查外掛程式是否參照了未宣告的禁止命名空間（如 `System.Runtime.InteropServices`、`System.Reflection.Emit`、`Microsoft.CodeAnalysis`）
2. 如果外掛程式需要 `System.IO` 或 `System.Net.Http`，確認外掛程式已透過 `[PluginCapability]` 宣告 `FileIO` 或 `Network` 能力
3. 驗證外掛程式只參照了可信組件白名單中的組件
4. 檢查外掛程式是否正確實作 `IPlugin` 介面
5. 檢視日誌獲取詳細的安全檢查失敗原因

#### 問題：外掛程式工具未註冊

**症狀**：
- 外掛程式載入成功但工具未出現在工具清單中

**解決方案**：
1. 確認外掛程式中的工具類別正確實作了 `ITool` 介面
2. 檢查工具類別是否為 public
3. 驗證 `ToolManager.ScanAllPluginAssemblies()` 是否被呼叫
4. 重新建構外掛程式並重啟應用

---

### 工作筆記問題

#### 問題：無法建立工作筆記

**症狀**：
```
Failed to create work note
```

**解決方案**：
1. 檢查生命體是否存在且處於運行狀態
2. 驗證儲存路徑有寫入權限
3. 檢查內容是否為空（內容必填）
4. 檢視日誌獲取詳細錯誤資訊

#### 問題：筆記搜尋無結果

**症狀**：
- 搜尋關鍵詞回傳空結果
- 但確定有相關筆記

**解決方案**：
1. 檢查關鍵詞拼寫是否正確
2. 嘗試使用更通用的關鍵詞
3. 驗證筆記中是否包含該關鍵詞（區分大小寫）
4. 增加 `max_results` 參數值

#### 問題：筆記目錄生成緩慢

**症狀**：
- 生成目錄時回應時間長
- 生命體有大量筆記（>1000 頁）

**解決方案**：
1. 這是正常現象，需要遍歷所有筆記
2. 考慮定期歸檔舊筆記
3. 使用搜尋功能代替目錄瀏覽
4. 計劃中的最佳化：新增目錄快取機制

---

### 知識網絡問題

#### 問題：知識查詢回傳空結果

**症狀**：
```
No knowledge triples found
```

**解決方案**：
1. 驗證主語和謂語的拼寫
2. 檢查知識是否已新增到網絡中
3. 使用搜尋功能進行模糊匹配：
```json
{
  "action": "search",
  "query": "關鍵詞"
}
```

#### 問題：知識路徑尋找失敗

**症狀**：
```
No path found between concepts
```

**解決方案**：
1. 驗證兩個概念是否存在於知識網絡中
2. 檢查是否存在關聯路徑（可能沒有直接或間接關係）
3. 嘗試新增更多知識以建立連接
4. 降低路徑長度限制（如果設定了的話）

#### 問題：知識驗證失敗

**症狀**：
```
Knowledge validation failed
```

**解決方案**：
1. 檢查三元組格式是否正確（主語、謂語、賓語必填）
2. 驗證置信度在 0.0-1.0 範圍內
3. 檢查是否有重複的三元組
4. 檢視驗證錯誤詳情以了解具體問題

#### 問題：知識網絡統計資訊不準確

**症狀**：
- 統計數字與預期不符
- 新增知識後統計未更新

**解決方案**：
1. 統計資訊可能需要幾秒鐘更新（快取）
2. 檢查是否有刪除操作未成功執行
3. 重啟應用程式強制重新整理統計
4. 透過 API 重新查詢統計資訊

---

### 專案管理問題

#### 問題：無法建立專案

**症狀**：
```
Failed to create project
```

**解決方案**：
1. 檢查專案名稱是否為空（必填）
2. 驗證專案名稱不重複
3. 檢查儲存路徑有寫入權限
4. 檢視日誌獲取詳細錯誤資訊

#### 問題：專案資料遺失

**症狀**：
- 專案資訊無法載入
- 專案檔案損壞

**解決方案**：
1. 檢查專案儲存目錄是否存在
2. 從備份還原專案資料
3. 驗證 JSON 檔案格式正確
4. 手動修復損壞的專案檔案

#### 問題：專案角色分配失敗

**症狀**：
```
Failed to assign role
```

**解決方案**：
1. 確認矽基生命體已加入專案
2. 檢查角色名稱是否有效
3. 驗證操作者是否為矽基主理人
4. 檢視日誌獲取詳細錯誤資訊

#### 問題：工作流無法啟動

**症狀**：
- 工作流實例建立失敗
- 狀態轉換不執行

**解決方案**：
1. 檢查工作流範本是否已定義
2. 驗證初始狀態是否正確設定
3. 確認專案已綁定工作流範本
4. 檢查工作流日誌以獲取轉換錯誤

---

### 工具權限問題

#### 問題：工具操作被拒絕

**症狀**：
```
Tool operation denied: network:post
```

**解決方案**：
1. 檢查矽基生命體的工具權限設定：
```bash
curl http://localhost:8080/api/beings/tool-permissions?beingId=<id>
```

2. 更新工具權限：
```bash
curl -X PUT http://localhost:8080/api/beings/tool-permissions \
  -H "Content-Type: application/json" \
  -d '{
    "beingId": "being-uuid",
    "permissions": {
      "network:post": "allowed"
    }
  }'
```

3. 或使用 Web UI：生命體 → 工具權限

#### 問題：專案工具權限不生效

**症狀**：
- 專案級別的工具權限未按預期工作

**解決方案**：
1. 確認專案級別的權限已正確設定
2. 檢查矽基生命體級別和專案級別權限是否衝突
3. 專案級別權限獨立於矽基生命體級別，兩者取交集
4. 檢視審計日誌確認權限檢查結果

---

## 除錯

### 啟用詳細日誌

編輯設定：
```json
{
  "logging": {
    "level": "debug"
  }
}
```

### 檢查日誌

日誌儲存在：
```
logs/
├── system.log
├── ai.log
├── permission.log
└── error.log
```

即時檢視：
```bash
tail -f logs/*.log
```

### 使用除錯器

**SiliconLife.Default（預設實作）**：
```bash
# 使用除錯器運行
dotnet run --project src/SiliconLife.Default --configuration Debug

# 附加除錯器
# 透過 IDE：附加到處理程序 > SiliconLife.Default
```

**SiliconLife.Fast（高效能版本）**：
```bash
# 使用除錯器運行
dotnet run --project src/SiliconLife.Fast --configuration Debug

# 附加除錯器
# 透過 IDE：附加到處理程序 > SiliconLife.Fast
```

> **建議**：開發除錯階段推薦使用 SiliconLife.Default，架構驗證通過後再使用 SiliconLife.Fast 進行生產部署。

---

## 效能問題

### 回應時間慢

**最佳化**：
1. 降低 AI 模型複雜度
2. 啟用快取
3. 清理舊資料
4. 增加系統資源

### CPU 使用率高

**檢查**：
- 運行太多生命體
- 工具中的無限迴圈
- 頻繁的定時器執行

**解決方案**：
- 減少並發生命體
- 最佳化工具程式碼
- 調整定時器間隔

### 記憶體使用率高

**監控**：
```bash
# 透過 Web UI：儀表板 > 記憶體
```

**最佳化**：
- 清理舊記憶
- 減少上下文大小
- 實作分頁

---

## 獲取幫助

### 檢視文件

- [快速開始指南](getting-started.md)
- [開發指南](development-guide.md)
- [API 參考](api-reference.md)
- [架構指南](architecture.md)

### 檢查日誌

始終首先檢查日誌以獲取錯誤詳情。

### 社群支援

- GitHub Issues：回報 bug
- Discussions：提問
- 文件：搜尋解決方案

---

## 緊急程序

### 系統當機

1. 檢查日誌以獲取原因
2. 重啟應用程式：

**SiliconLife.Default（預設實作）**：
```bash
dotnet run --project src/SiliconLife.Default
```

**SiliconLife.Fast（主推生產版本）**：
```bash
dotnet run --project src/SiliconLife.Fast
```

3. 如需從備份還原

### 資料遺失

1. 立即停止應用程式
2. 檢查備份檔案
3. 還原資料
4. 驗證完整性

### 安全漏洞

1. 停止所有生命體
2. 撤銷所有權限
3. 檢查審計日誌
4. 檢視存取控制
5. 以限制權限重啟

---

## 預防

### 最佳實踐

1. **定期備份**
   - 備份資料目錄
   - 備份設定
   - 測試還原過程

2. **監控資源**
   - 監視 CPU/記憶體使用
   - 監控磁碟空間
   - 檢查網路連接

3. **保持更新**
   - 更新 .NET SDK
   - 更新依賴
   - 套用安全修補程式

4. **測試變更**
   - 首先在開發中測試
   - 使用版本控制
   - 記錄變更

---

## 下一步

- 📚 閱讀[架構指南](architecture.md)
- 🛠️ 檢視[開發指南](development-guide.md)
- 🚀 檢視[快速開始指南](getting-started.md)
- 🔒 檢視[安全文件](security.md)
