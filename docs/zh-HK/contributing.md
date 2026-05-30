# 貢獻指南

> **版本：v0.2.0-alpha**

[English](../en/contributing.md) | [Deutsch](../de-DE/contributing.md) | [中文](../zh-CN/contributing.md) | **繁體中文** | [Español](../es-ES/contributing.md) | [日本語](../ja-JP/contributing.md) | [한국어](../ko-KR/contributing.md) | [Čeština](../cs-CZ/contributing.md) | [Русский](../ru-RU/contributing.md)

感謝您有興趣為 SiliconLifeCollective 做出貢獻！

## 雙版本貢獻

本專案有兩個實作版本，您可以根據興趣選擇貢獻方向：

### SiliconLife.Default（預設版本）
- **技術棧**：.NET 9 主控台應用
- **貢獻方向**：核心功能開發、工具實作、在地化、文件
- **適合對象**：所有開發者

### SiliconLife.Fast（高效能版本）
- **技術棧**：.NET 9 跨平台桌面應用（Avalonia UI）
- **貢獻方向**：效能最佳化、SpeedyPack 儲存、系統匣、無鎖並行
- **適合對象**：有桌面開發經驗、對效能最佳化感興趣的開發者

> **重要提示**：兩個版本共用 SiliconLife.Core 和 SiliconLife.Common 專案，對核心介面的改進會同時影響兩個版本。

## 行為準則

本專案遵循 Apache 2.0 授權條款。在所有互動中保持尊重與專業。

---

## 快速開始

### 1. Fork 儲存庫

點擊 GitHub 上的「Fork」按鈕建立您自己的複本。

### 2. 複製您的 Fork

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. 設定開發環境

```bash
# 安裝 .NET 9 SDK
# https://dotnet.microsoft.com/download/dotnet/9.0

# 還原相依性
dotnet restore

# 建置專案
dotnet build

# 執行測試
dotnet test
```

### 4. 建立功能分支

```bash
git checkout -b feature/your-feature-name
```

### 5. 選擇開發專案

根據您的貢獻類型選擇合適的專案：

- **核心介面/抽象類別** → 修改 `SiliconLife.Core`
- **共用實作** → 修改 `SiliconLife.Common`
- **Default 版本特定** → 修改 `SiliconLife.Default`
- **Fast 版本特定** → 修改 `SiliconLife.Fast`
- **儲存引擎** → 修改 `SiliconLife.Speedy`
- **儲存管理工具** → 修改 `SiliconLife.Speedy.Manager`
- **外掛程式開發** → 修改 `SiliconLife.Core/Plugins`
- **多語系文件** → 修改 `docs/` 目錄

---

## 開發工作流程

### 程式碼風格

- 遵循 C# 編碼慣例
- 類別名稱使用 PascalCase
- 方法參數使用 camelCase
- 私用欄位使用 `_camelCase`
- 所有公用 API 必須有 XML 文件

### 提交訊息

遵循**約定式提交**格式：

```
<type>(<scope>): <description>
```

**類型**：
- `feat`：新功能
- `fix`：Bug 修復
- `docs`：文件變更
- `style`：程式碼格式
- `refactor`：程式碼重構
- `test`：測試變更
- `chore`：建置/工具變更

**範例**：
```bash
feat(localization): add Korean language support
fix(permission): fix null pointer in callback
docs: update contributing guide
refactor(web): simplify controller structure
```

### 進行變更

1. **撰寫程式碼**
   - 遵循現有模式
   - 為新功能新增測試
   - 更新文件

2. **測試您的變更**
   ```bash
   # 執行所有測試
   dotnet test

   # 以發佈模式建置
   dotnet build --configuration Release
   ```

3. **格式化程式碼**
   ```bash
   dotnet format
   ```

4. **提交變更**
   ```bash
   git add .
   git commit -m "feat(scope): description"
   ```

5. **推送到您的 Fork**
   ```bash
   git push origin feature/your-feature-name
   ```

6. **建立拉取請求**
   - 前往原始儲存庫
   - 點擊「Compare & pull request」
   - 填寫 PR 範本
   - 提交

---

## 拉取請求指南

### PR 標題

使用與提交訊息相同的格式：
```
feat(localization): add Korean language support
```

### PR 描述

包含：

1. **什麼** - 這個 PR 做什麼？
2. **為什麼** - 為什麼需要這個變更？
3. **如何** - 您是如何實作的？
4. **測試** - 如何測試的？

### PR 描述範例

```markdown
## 什麼
為所有 UI 元件和文件新增韓語在地化。

## 為什麼
擴大專案對韓語使用者的可存取性。

## 如何
- 建立 KoKR.cs 在地化檔案
- 新增 500+ 翻譯鍵
- 更新所有檢視以使用在地化
- 在 docs/ko-KR/ 中建立韓語文件

## 測試
- 驗證所有 UI 元素正確顯示韓語
- 測試語言切換功能
- 與母語者審閱翻譯
```

---

## 貢獻類型

### 1. Bug 修復

**流程**：
1. 檢查現有議題
2. 如果不存在則建立議題
3. 修復 bug
4. 新增測試個案
5. 提交 PR

**要求**：
- 清晰描述 bug
- 重現步驟
- 防止迴歸的測試

### 2. 新功能

**流程**：
1. 在 Issues/Discussions 中討論功能
2. 取得維護者核准
3. 實作功能
4. 新增全面的測試
5. 更新文件
6. 提交 PR

**要求**：
- 功能提案已核准
- 完整測試覆蓋
- 文件已更新
- 向後相容

### 3. 文件

**流程**：
1. 識別文件空白
2. 撰寫/更新文件
3. 提交 PR

**要求**：
- 清晰簡潔
- 包含範例
- 如適用支援多語系

### 4. 程式碼重構

**流程**：
1. 在 Issue 中提議重構
2. 取得核准
3. 重構程式碼
4. 確保所有測試通過
5. 提交 PR

**要求**：
- 無功能變更
- 所有測試通過
- 改進程式碼品質
- 清晰解釋

---

## 測試指南

### 單元測試

```csharp
[TestMethod]
public void MyFeature_ShouldWork_AsExpected()
{
    // 安排
    var service = new MyService();

    // 執行
    var result = service.DoSomething();

    // 斷言
    Assert.IsTrue(result.Success);
}
```

### 整合測試

測試完整工作流程：
- AI 互動
- 工具執行
- 權限驗證
- 儲存操作

### 手動測試

對於 UI 變更：
- 在多個瀏覽器中測試
- 驗證回應式設計
- 檢查可存取性

---

## 文件指南

### 程式碼註解

- 所有公用 API 使用 XML 註解
- 複雜邏輯使用行內註解
- 程式碼註解使用英語

### 文件檔案

- 放置在 `docs/{language}/` 中
- 更新所有語系版本
- 遵循現有結構

### 多語系文件

新增文件時：
1. 首先建立英語版本
2. 翻譯到其他語系
3. 保持內容同步

---

## 審閱流程

### 維護者檢查什麼

1. **程式碼品質**
   - 遵循慣例
   - 清晰易讀
   - 文件完善

2. **測試**
   - 覆蓋充分
   - 所有測試通過
   - 覆蓋邊界情況

3. **文件**
   - 已更新
   - 解釋清晰
   - 多語系

4. **相容性**
   - 向後相容
   - 無破壞性變更（除非通知）
   - 遵循語意化版本控制

### 審閱時間線

- 初始審閱：1-3 天
- 回饋整合：按需
- 合併：核准後

---

## 常見問題

### PR 被拒絕

**原因**：
- 不遵循指南
- 測試不足
- 未通知的破壞性變更
- 程式碼品質差

**解決方案**：
- 解決回饋
- 更新 PR
- 重新提交

### 合併衝突

**解決方案**：
```bash
# 更新您的分支
git fetch origin
git rebase origin/master

# 解決衝突
# 編輯衝突檔案
git add .
git rebase --continue

# 強制推送
git push --force-with-lease
```

---

## 取得協助

### 資源

- **文件**：[docs/](../)
- **議題**：GitHub Issues
- **討論**：GitHub Discussions
- **行為準則**：CODE_OF_CONDUCT.md

### 聯繫

- 為 bug 建立 Issue
- 為議題發起 Discussion
- 為緊急事項標記維護者

---

## 致謝

貢獻者將在以下位置獲得認可：
- README.md 貢獻者部分
- 發佈說明
- 專案文件

---

## 授權條款

透過貢獻，您同意您的貢獻將在 Apache 2.0 授權條款下獲得授權。

---

## 下一步

- 📚 閱讀[文件](../)
- 🐛 檢視[開放議題](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 開始[討論](https://github.com/akimoto-akira/SiliconLifeCollective/discussions)
- 🚀 Fork 並開始貢獻！

感謝您為 SiliconLifeCollective 做出貢獻！🎉
