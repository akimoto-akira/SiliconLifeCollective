// Copyright (c) 2026 Silicon Life Collective
// Licensed under the Apache License, Version 2.0

using SiliconLife.Collective;

namespace SiliconLife.Default.Help;

/// <summary>
/// Chinese (Hong Kong) help documentation - Traditional Chinese
/// </summary>
public class HelpLocalizationZhHK : HelpLocalizationBase
{
    #region Help Documents
    
    public override string GettingStarted_Title => "快速入門";
    public override string BeingManagement_Title => "硅基人管理";
    public override string ChatSystem_Title => "聊天功能";
    public override string TaskTimer_Title => "任務與定時器";
    public override string Permission_Title => "權限管理";
    public override string Config_Title => "配置管理";
    public override string FAQ_Title => "常見問題";

    public override string[] GettingStarted_Tags => new[] { "安裝", "啟動", "入門", "快速入門", "開始使用", "初始化", "運行", "配置環境" };
    public override string[] BeingManagement_Tags => new[] { "硅基人", "創建", "配置", "硅基人管理", "生命體", "檔案", "設定", "管理" };
    public override string[] ChatSystem_Tags => new[] { "聊天", "對話", "訊息", "聊天功能", "交流", "通訊", "交談", "討論" };
    public override string[] TaskTimer_Tags => new[] { "任務", "定時器", "調度", "任務與定時器", "自動化", "定時", "提醒", "週期" };
    public override string[] Permission_Tags => new[] { "權限", "安全", "存取控制", "權限管理", "授權", "私隱", "保護", "認證" };
    public override string[] Config_Tags => new[] { "配置", "設定", "選項", "配置管理", "偏好", "自訂", "系統", "參數" };
    public override string[] FAQ_Tags => new[] { "常見問題", "幫助", "問題", "支援", "故障排除", "指南", "協助", "解答" };
    
    public override string GettingStarted => @"
# 快速入門

## 啟動系統

在命令列中執行程式，系統會自動啟動並監聽 8080 連接埠。

```bash
dotnet run
```

## 訪問Web介面

開啟瀏覽器，訪問 `http://localhost:8080` 即可進入Web管理介面。

## 首次啟動

首次啟動時，系統會自動建立硅基主理人（Curator），您需要：

1. 設定主理人名稱
2. 配置靈魂檔案（提示詞）
3. 選擇AI模型

## 基本操作

- **儀表板**: 查看系統狀態和統計資訊
- **硅基人**: 建立和管理硅基生命體
- **聊天**: 與硅基人對話
- **任務**: 設定定時任務
- **配置**: 調整系統設定
- **幫助**: 查看本文檔

## 快捷鍵

- `F1` - 開啟幫助文檔
- `Ctrl+F` - 聚焦搜尋框
";

    public override string BeingManagement => @"
# 硅基人管理

## 建立硅基人

1. 進入「硅基人」頁面
2. 點擊「建立新硅基人」
3. 填寫以下資訊：
   - **名稱**: 硅基人的顯示名稱
   - **靈魂檔案**: 核心提示詞，決定行為模式（支援Markdown格式）
   - **AI模型**: 選擇使用的AI模型
4. 點擊「建立」完成

## 配置靈魂檔案

靈魂檔案是硅基人的核心提示詞，決定其行為模式、性格特點和能力範圍。

### 編寫建議

```markdown
# 角色設定

你是一個專業的編程助手，擅長：
- C# 開發
- 架構設計
- 程式碼審查

# 行為準則

1. 始終提供可執行的程式碼範例
2. 解釋程式碼的關鍵部分
3. 提供最佳實踐建議
```

## 管理硅基人

- **編輯**: 修改名稱、靈魂檔案等配置
- **刪除**: 永久刪除硅基人（不可復原）
- **克隆**: 基於現有硅基人建立新版本
";

    public override string ChatSystem => @"
# 聊天功能

## 開始對話

1. 選擇要對話的硅基人
2. 在輸入框中輸入訊息
3. 按 Enter 或點擊傳送按鈕

## 對話特性

- **即時回應**: 使用SSE技術實現串流式輸出
- **工具調用**: AI可以調用工具執行操作
- **上下文記憶**: 儲存對話歷史
- **多語言**: 支援多種語言對話

## 工具使用

硅基人可以自動調用工具來：
- 查詢日曆資訊
- 管理系統配置
- 執行程式碼
- 存取檔案系統（需權限）

## 中斷對話

如果AI正在思考，您可以：
- 點擊「停止」按鈕
- 或傳送新訊息自動中斷
";

    public override string TaskTimer => @"
# 任務與定時器

## 建立定時任務

1. 進入「任務」頁面
2. 點擊「新建任務」
3. 配置任務：
   - **任務名稱**: 描述性名稱
   - **觸發條件**: 時間表達式（Cron格式）
   - **執行動作**: 選擇要執行的操作
   - **目標硅基人**: 選擇執行者

## Cron表達式

```
分鐘 (0-59)
| 小時 (0-23)
| | 日期 (1-31)
| | | 月份 (1-12)
| | | | 星期 (0-6)
| | | | |
* * * * *
```

### 範例

- `0 * * * *` - 每小時整點
- `0 9 * * *` - 每天上午9點
- `*/5 * * * *` - 每5分鐘
- `0 9 * * 1-5` - 工作日早上9點

## 任務管理

- **啟用/停用**: 臨時停用任務
- **編輯**: 修改任務配置
- **刪除**: 移除任務
- **執行歷史**: 查看任務執行記錄
";

    public override string Permission => @"
# 權限管理

## 權限級別

系統採用5級權限控制：

1. **IsCurator**: 主理人擁有最高權限
2. **UserFrequencyCache**: 使用者頻率快取限制
3. **GlobalACL**: 全域存取控制清單
4. **IPermissionCallback**: 自訂權限回呼
5. **IPermissionAskHandler**: 詢問使用者權限

## 權限類型

- **讀取**: 查看資訊和資料
- **寫入**: 修改和建立資料
- **執行**: 執行工具和命令
- **管理**: 管理系統配置

## 配置權限

1. 進入「配置」頁面
2. 選擇「權限設定」
3. 配置各項權限：
   - 允許/拒絕特定操作
   - 設定頻率限制
   - 配置白名單/黑名單

## 安全建議

- 定期審查權限配置
- 限制敏感操作的存取
- 啟用操作日誌記錄
- 使用最小權限原則
";

    public override string Config => @"
# 配置管理

## 系統配置

### AI模型配置

```json
{
  ""AI"": {
    ""DefaultProvider"": ""ollama"",
    ""Models"": {
      ""ollama"": {
        ""Endpoint"": ""http://localhost:11434"",
        ""Model"": ""llama3""
      }
    }
  }
}
```

### 網路配置

- **連接埠**: 預設 8080
- **主機**: 預設 localhost
- **HTTPS**: 可選啟用

### 儲存配置

- **資料目錄**: 硅基人資料儲存位置
- **日誌級別**: Debug/Info/Warning/Error
- **備份策略**: 自動備份頻率

## 面板主題

系統支援多種介面主題：

- **Light**: 淺色主題（預設）
- **Dark**: 深色主題
- **自訂**: 建立自己的主題

## 本地化

支援的語言：
- 簡體中文 (zh-CN)
- 繁體中文 (zh-HK)
- 英文 (en-US, en-GB)
- 日文 (ja-JP)
- 韓文 (ko-KR)
- 西班牙文 (es-ES, es-MX)
- 捷克文 (cs-CZ)
";

    public override string FAQ => @"
# 常見問題

## 系統相關

### Q: 系統啟動失敗怎麼辦？

A: 檢查以下幾點：
1. 8080 連接埠是否被佔用
2. .NET 9 執行階段是否正確安裝
3. 查看日誌檔案取得詳細錯誤資訊

### Q: 如何修改監聽連接埠？

A: 在配置檔案中修改 `WebHost:Port` 設定，或使用命令列參數。

### Q: 資料儲存在哪個目錄？

A: 預設儲存在硅基人根目錄下的 `data` 資料夾。

## AI相關

### Q: AI回應很慢怎麼辦？

A: 可能的原因：
1. 模型較大，需要更多運算資源
2. 網路延遲（使用雲端模型時）
3. 考慮使用本機模型（如 Ollama）

### Q: 如何切換AI模型？

A: 在配置檔案中修改 `AI:DefaultProvider`，或在硅基人配置中選擇不同模型。

### Q: AI無法調用工具？

A: 檢查：
1. 工具是否正確註冊
2. 權限是否允許工具調用
3. AI模型是否支援工具調用功能

## 硅基人相關

### Q: 如何備份硅基人資料？

A: 複製硅基人目錄下的所有檔案到備份位置。

### Q: 可以匯入匯出靈魂檔案嗎？

A: 支援。在硅基人編輯頁面可以匯入/匯出Markdown格式的靈魂檔案。

### Q: 如何克隆硅基人？

A: 在硅基人清單中選擇「克隆」，系統會建立一個副本，您可以修改配置。

## 取得幫助

如果以上無法解決您的問題：

1. 查看[專案文檔](https://github.com/your-repo/docs)
2. 提交Issue報告問題
3. 加入社群討論
";
    
    #endregion
}
