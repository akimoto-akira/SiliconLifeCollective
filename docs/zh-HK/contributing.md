# 貢獻指南

[English](contributing.md) | [简体中文](docs/zh-CN/contributing.md) | [繁體中文](docs/zh-HK/contributing.md) | [Español](docs/es-ES/contributing.md) | [日本語](docs/ja-JP/contributing.md) | [한국어](docs/ko-KR/contributing.md) | [Čeština](docs/cs-CZ/contributing.md)

感谢您有興趣為 SiliconLifeCollective 做出貢献！

## 行為准則

本項目遵循 Apache 2.0 授權證。在所有互動中保持尊重和專業。

---

## 快速開始

### 1. Fork 倉程式庫

点擊 GitHub 上的"Fork"按钮建立您自己的副本。

### 2. 克隆您的 Fork

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. 設定開發环境

```bash
# 安裝 .NET 9 SDK
# https://dotnet.microsoft.com/download/dotnet/9.0

# 復原依赖
dotnet restore

# 构建項目
dotnet build

# 執行測試
dotnet test
```

### 4. 建立功能分支

```bash
git checkout -b feature/your-feature-name
```

---

## 開發工作流程

### 程式碼風格

- 遵循 C# 编码約定
- 類別名使用 PascalCase
- 方法參數使用 camelCase
- 私有字段使用 `_camelCase`
- 所有公共 API 必須有 XML 文檔

### 提交訊息

遵循**約定式提交**格式：

```
<type>(<scope>): <description>
```

**類型**：
- `feat`：新功能
- `fix`：Bug 修復
- `docs`：文檔变更
- `style`：程式碼格式
- `refactor`：程式碼重构
- `test`：測試变更
- `chore`：构建/工具变更

**示例**：
```bash
feat(localization): add Korean language support
fix(permission): fix null pointer in callback
docs: update contributing guide
refactor(web): simplify controller structure
```

### 進行更改

1. **编寫程式碼**
   - 遵循现有模式
   - 為新功能添加測試
   - 更新文檔

2. **測試您的更改**
   ```bash
   # 執行所有測試
   dotnet test
   
   # 以發佈模式构建
   dotnet build --configuration Release
   ```

3. **格式化程式碼**
   ```bash
   dotnet format
   ```

4. **提交更改**
   ```bash
   git add .
   git commit -m "feat(scope): description"
   ```

5. **推送到您的 Fork**
   ```bash
   git push origin feature/your-feature-name
   ```

6. **建立拉取要求**
   - 转到原始倉程式庫
   - 点擊"Compare & pull request"
   - 填寫 PR 模板
   - 提交

---

## 拉取要求指南

### PR 标題

使用與提交訊息相同的格式：
```
feat(localization): add Korean language support
```

### PR 描述

包括：

1. **什麼** - 这個 PR 做什麼？
2. **為什麼** - 為什麼需要这個变更？
3. **如何** - 您是如何實現的？
4. **測試** - 如何測試的？

### PR 描述示例

```markdown
## 什麼
為所有 UI 元件和文檔添加韩语在地化。

## 為什麼
擴大項目對韩语使用者的可訪問性。

## 如何
- 建立 KoKR.cs 在地化檔案
- 添加 500+ 翻译键
- 更新所有檢視以使用在地化
- 在 docs/ko-KR/ 中建立韩语文檔

## 測試
- 驗證所有 UI 元素正确顯示韩语
- 測試语言切换功能
- 與母语者审阅翻译
```

---

## 貢献類型

### 1. Bug 修復

**流程**：
1. 檢查现有問題
2. 如果不存在則建立問題
3. 修復 bug
4. 添加測試用例
5. 提交 PR

**要求**：
- 清晰描述 bug
- 重现步驟
- 防止回歸的測試

### 2. 新功能

**流程**：
1. 在 Issues/Discussions 中討論功能
2. 获得維護者核准
3. 實現功能
4. 添加全面的測試
5. 更新文檔
6. 提交 PR

**要求**：
- 功能提案已核准
- 完整測試覆盖
- 文檔已更新
- 向後相容

### 3. 文檔

**流程**：
1. 識別文檔空白
2. 编寫/更新文檔
3. 提交 PR

**要求**：
- 清晰简洁
- 包含示例
- 如适用支援多语言

### 4. 程式碼重构

**流程**：
1. 在 Issue 中提議重构
2. 获得核准
3. 重构程式碼
4. 确保所有測試通過
5. 提交 PR

**要求**：
- 无功能变更
- 所有測試通過
- 改善程式碼质量
- 清晰解释

---

## 測試指南

### 单元測試

```csharp
[TestMethod]
public void MyFeature_ShouldWork_AsExpected()
{
    // 安排
    var service = new MyService();
    
    // 執行
    var result = service.DoSomething();
    
    // 断言
    Assert.IsTrue(result.Success);
}
```

### 集成測試

測試完整工作流程：
- AI 交互
- 工具執行
- 權限驗證
- 儲存操作

### 手動測試

對於 UI 变更：
- 在多個瀏覽器中測試
- 驗證回應式設計
- 檢查可訪問性

---

## 文檔指南

### 程式碼註解

- 所有公共 API 使用 XML 註解
- 复雜逻辑使用內联註解
- 程式碼註解使用英语

### 文檔檔案

- 放置在 `docs/{language}/` 中
- 更新所有语言版本
- 遵循现有結構

### 多语言文檔

添加文檔时：
1. 首先建立英语版本
2. 翻译到其他语言
3. 保持內容同步

---

## 审阅流程

### 維護者檢查什麼

1. **程式碼质量**
   - 遵循約定
   - 清晰易读
   - 文檔完善

2. **測試**
   - 覆盖充分
   - 所有測試通過
   - 覆盖邊界情况

3. **文檔**
   - 已更新
   - 解释清晰
   - 多语言

4. **相容性**
   - 向後相容
   - 无破壞性变更（除非通知）
   - 遵循语義化版本控制

### 审阅時間线

- 初始审阅：1-3 天
- 回饋整合：按需
- 合並：核准後

---

## 常見問題

### PR 被拒絕

**原因**：
- 不遵循指南
- 測試不足
- 未通知的破壞性变更
- 程式碼质量差

**解決方案**：
- 解決回饋
- 更新 PR
- 重新提交

### 合並衝突

**解決方案**：
```bash
# 更新您的分支
git fetch origin
git rebase origin/master

# 解決衝突
# 编辑衝突檔案
git add .
git rebase --continue

# 强制推送
git push --force-with-lease
```

---

## 获取幫助

### 資源

- **文檔**：[docs/](../)
- **問題**：GitHub Issues
- **討論**：GitHub Discussions
- **行為准則**：CODE_OF_CONDUCT.md

### 联系

- 為 bug 建立 Issue
- 為問題啟動 Discussion
- 為紧急事項标記維護者

---

## 致谢

貢献者将在以下位置获得認可：
- README.md 貢献者部分
- 發佈說明
- 項目文檔

---

## 授權證

通過貢献，您同意您的貢献将在 Apache 2.0 授權證下获得許可。

---

## 下一步

- 📚 阅读[文檔](../)
- 🐛 查看[開放問題](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 開始[討論](https://github.com/akimoto-akira/SiliconLifeCollective/discussions)
- 🚀 Fork 並開始貢献！

感谢您為 SiliconLifeCollective 做出貢献！🎉
