# 贡献指南

感谢您有兴趣为 SiliconLifeCollective 做出贡献！

## 行为准则

本项目遵循 Apache 2.0 许可证。在所有互动中保持尊重和专业。

---

## 快速开始

### 1. Fork 仓库

点击 GitHub 上的"Fork"按钮创建您自己的副本。

### 2. 克隆您的 Fork

```bash
git clone https://github.com/your-username/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. 设置开发环境

```bash
# 安装 .NET 9 SDK
# https://dotnet.microsoft.com/download/dotnet/9.0

# 恢复依赖
dotnet restore

# 构建项目
dotnet build

# 运行测试
dotnet test
```

### 4. 创建功能分支

```bash
git checkout -b feature/your-feature-name
```

---

## 开发工作流程

### 代码风格

- 遵循 C# 编码约定
- 类名使用 PascalCase
- 方法参数使用 camelCase
- 私有字段使用 `_camelCase`
- 所有公共 API 必须有 XML 文档

### 提交消息

遵循**约定式提交**格式：

```
<type>(<scope>): <description>
```

**类型**：
- `feat`：新功能
- `fix`：Bug 修复
- `docs`：文档变更
- `style`：代码格式
- `refactor`：代码重构
- `test`：测试变更
- `chore`：构建/工具变更

**示例**：
```bash
feat(localization): add Korean language support
fix(permission): fix null pointer in callback
docs: update contributing guide
refactor(web): simplify controller structure
```

### 进行更改

1. **编写代码**
   - 遵循现有模式
   - 为新功能添加测试
   - 更新文档

2. **测试您的更改**
   ```bash
   # 运行所有测试
   dotnet test
   
   # 以发布模式构建
   dotnet build --configuration Release
   ```

3. **格式化代码**
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

6. **创建拉取请求**
   - 转到原始仓库
   - 点击"Compare & pull request"
   - 填写 PR 模板
   - 提交

---

## 拉取请求指南

### PR 标题

使用与提交消息相同的格式：
```
feat(localization): add Korean language support
```

### PR 描述

包括：

1. **什么** - 这个 PR 做什么？
2. **为什么** - 为什么需要这个变更？
3. **如何** - 您是如何实现的？
4. **测试** - 如何测试的？

### PR 描述示例

```markdown
## 什么
为所有 UI 组件和文档添加韩语本地化。

## 为什么
扩大项目对韩语用户的可访问性。

## 如何
- 创建 KoKR.cs 本地化文件
- 添加 500+ 翻译键
- 更新所有视图以使用本地化
- 在 docs/ko-KR/ 中创建韩语文档

## 测试
- 验证所有 UI 元素正确显示韩语
- 测试语言切换功能
- 与母语者审阅翻译
```

---

## 贡献类型

### 1. Bug 修复

**流程**：
1. 检查现有问题
2. 如果不存在则创建问题
3. 修复 bug
4. 添加测试用例
5. 提交 PR

**要求**：
- 清晰描述 bug
- 重现步骤
- 防止回归的测试

### 2. 新功能

**流程**：
1. 在 Issues/Discussions 中讨论功能
2. 获得维护者批准
3. 实现功能
4. 添加全面的测试
5. 更新文档
6. 提交 PR

**要求**：
- 功能提案已批准
- 完整测试覆盖
- 文档已更新
- 向后兼容

### 3. 文档

**流程**：
1. 识别文档空白
2. 编写/更新文档
3. 提交 PR

**要求**：
- 清晰简洁
- 包含示例
- 如适用支持多语言

### 4. 代码重构

**流程**：
1. 在 Issue 中提议重构
2. 获得批准
3. 重构代码
4. 确保所有测试通过
5. 提交 PR

**要求**：
- 无功能变更
- 所有测试通过
- 改进代码质量
- 清晰解释

---

## 测试指南

### 单元测试

```csharp
[TestMethod]
public void MyFeature_ShouldWork_AsExpected()
{
    // 安排
    var service = new MyService();
    
    // 执行
    var result = service.DoSomething();
    
    // 断言
    Assert.IsTrue(result.Success);
}
```

### 集成测试

测试完整工作流程：
- AI 交互
- 工具执行
- 权限验证
- 存储操作

### 手动测试

对于 UI 变更：
- 在多个浏览器中测试
- 验证响应式设计
- 检查可访问性

---

## 文档指南

### 代码注释

- 所有公共 API 使用 XML 注释
- 复杂逻辑使用内联注释
- 代码注释使用英语

### 文档文件

- 放置在 `docs/{language}/` 中
- 更新所有语言版本
- 遵循现有结构

### 多语言文档

添加文档时：
1. 首先创建英语版本
2. 翻译到其他语言
3. 保持内容同步

---

## 审阅流程

### 维护者检查什么

1. **代码质量**
   - 遵循约定
   - 清晰易读
   - 文档完善

2. **测试**
   - 覆盖充分
   - 所有测试通过
   - 覆盖边界情况

3. **文档**
   - 已更新
   - 解释清晰
   - 多语言

4. **兼容性**
   - 向后兼容
   - 无破坏性变更（除非通知）
   - 遵循语义化版本控制

### 审阅时间线

- 初始审阅：1-3 天
- 反馈整合：按需
- 合并：批准后

---

## 常见问题

### PR 被拒绝

**原因**：
- 不遵循指南
- 测试不足
- 未通知的破坏性变更
- 代码质量差

**解决方案**：
- 解决反馈
- 更新 PR
- 重新提交

### 合并冲突

**解决方案**：
```bash
# 更新您的分支
git fetch origin
git rebase origin/master

# 解决冲突
# 编辑冲突文件
git add .
git rebase --continue

# 强制推送
git push --force-with-lease
```

---

## 获取帮助

### 资源

- **文档**：[docs/](../)
- **问题**：GitHub Issues
- **讨论**：GitHub Discussions
- **行为准则**：CODE_OF_CONDUCT.md

### 联系

- 为 bug 创建 Issue
- 为问题启动 Discussion
- 为紧急事项标记维护者

---

## 致谢

贡献者将在以下位置获得认可：
- README.md 贡献者部分
- 发布说明
- 项目文档

---

## 许可证

通过贡献，您同意您的贡献将在 Apache 2.0 许可证下获得许可。

---

## 下一步

- 📚 阅读[文档](../)
- 🐛 查看[开放问题](https://github.com/your-org/SiliconLifeCollective/issues)
- 💬 开始[讨论](https://github.com/your-org/SiliconLifeCollective/discussions)
- 🚀 Fork 并开始贡献！

感谢您为 SiliconLifeCollective 做出贡献！🎉
