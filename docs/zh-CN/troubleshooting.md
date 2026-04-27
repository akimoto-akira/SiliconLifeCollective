# 故障排除指南

> **版本：v0.1.0-alpha**

[English](../en/troubleshooting.md) | [Deutsch](../de-DE/troubleshooting.md) | **中文** | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | [Čeština](../cs-CZ/troubleshooting.md)

## 常见问题

### 构建和编译

#### 问题：构建失败，缺少依赖

**症状**：
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**解决方案**：
```bash
dotnet restore
dotnet build
```

#### 问题：未找到 .NET SDK

**症状**：
```
The .NET SDK could not be found
```

**解决方案**：
1. 安装 .NET 9 SDK：https://dotnet.microsoft.com/download/dotnet/9.0
2. 验证安装：
```bash
dotnet --version
```

---

### AI 连接问题

#### 问题：Ollama 连接被拒绝

**症状**：
```
Failed to connect to Ollama at http://localhost:11434
```

**解决方案**：
```bash
# 检查 Ollama 是否正在运行
ollama list

# 启动 Ollama
ollama serve

# 测试连接
curl http://localhost:11434/api/tags
```

#### 问题：未找到模型

**症状**：
```
model "qwen2.5:7b" not found
```

**解决方案**：
```bash
# 拉取所需模型
ollama pull qwen2.5:7b

# 列出可用模型
ollama list
```

#### 问题：百炼 404 错误

**症状**：
```
HTTP 404: Model not found
```

**解决方案**：
1. 验证 API 密钥正确
2. 检查模型名称与百炼目录匹配
3. 验证区域端点正确
4. 检查帐户有权访问该模型

---

### 运行时问题

#### 问题：端口已被占用

**症状**：
```
HttpListenerException: Address already in use
```

**解决方案**：

**Windows**：
```bash
netstat -ano | findstr :8080
taskkill /PID <PID> /F
```

**Linux/Mac**：
```bash
lsof -ti:8080 | xargs kill -9
```

**或更改配置中的端口**。

#### 问题：生命体无法启动

**症状**：
- 生命体状态显示"Error"
- 日志显示初始化失败

**解决方案**：
1. 检查灵魂文件存在且有效
2. 验证 AI 客户端已配置
3. 检查日志以获取具体错误：
```bash
tail -f logs/*.log
```

#### 问题：内存不足

**症状**：
```
OutOfMemoryException
```

**解决方案**：
1. 增加堆大小：
```bash
dotnet run --server.gcHeapCount 4
```

2. 清理旧数据：
```bash
# 归档旧日志
mv logs/ logs-archive/
mkdir logs

# 清理旧记忆
# 通过 Web UI：记忆管理 > 清理
```

---

### 权限问题

#### 问题：权限被拒绝

**症状**：
```
Permission denied: disk:write
```

**解决方案**：
1. 检查当前权限：
```bash
curl http://localhost:8080/api/permissions
```

2. 授予权限：
```bash
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

3. 或使用 Web UI：权限管理

#### 问题：权限未过期

**症状**：
- 权限在过期时间后仍然有效

**解决方案**：
1. 检查系统时钟同步
2. 验证 `expiresAt` 字段设置正确
3. 清除权限缓存

---

### Web UI 问题

#### 问题：无法访问 Web UI

**症状**：
- 浏览器显示"Connection refused"

**解决方案**：
1. 验证服务器正在运行
2. 检查正确 URL：`http://localhost:8080`
3. 检查防火墙设置
4. 检查日志以获取启动错误

#### 问题：SSE 不工作

**症状**：
- 实时更新未出现
- 聊天不流式传输

**解决方案**：
1. 检查浏览器支持 SSE
2. 禁用 SSE 的代理缓冲
3. 检查网络稳定性
4. 尝试不同浏览器

#### 问题：UI 看起来损坏

**症状**：
- 样式不正确
- 布局损坏

**解决方案**：
1. 清除浏览器缓存
2. 尝试不同皮肤：设置 > 皮肤
3. 检查浏览器控制台中的错误
4. 禁用浏览器扩展

---

### 存储问题

#### 问题：无法读取/写入数据

**症状**：
```
IOException: Access denied
```

**解决方案**：
1. 检查文件权限
2. 验证存储路径存在
3. 检查磁盘空间
4. 以适当权限运行

#### 问题：数据损坏

**症状**：
- JSON 解析错误
- 数据丢失

**解决方案**：
1. 从备份恢复
2. 检查存储完整性：
```bash
# 通过 Web UI：系统 > 存储检查
```

3. 手动修复损坏的文件

---

### 工具执行问题

#### 问题：未找到工具

**症状**：
```
Tool "xyz" not found
```

**解决方案**：
1. 验证工具名称正确
2. 检查工具在 Tools 目录中
3. 重新构建项目
4. 检查工具是否正确实现

#### 问题：工具返回错误

**症状**：
```
Tool execution failed: ...
```

**解决方案**：
1. 检查工具日志
2. 验证输入参数
3. 独立测试工具
4. 检查权限

---

### 工作笔记问题

#### 问题：无法创建工作笔记

**症状**：
```
Failed to create work note
```

**解决方案**：
1. 检查生命体是否存在且处于运行状态
2. 验证存储路径有写入权限
3. 检查内容是否为空（内容必填）
4. 查看日志获取详细错误信息

#### 问题：笔记搜索无结果

**症状**：
- 搜索关键词返回空结果
- 但确定有相关笔记

**解决方案**：
1. 检查关键词拼写是否正确
2. 尝试使用更通用的关键词
3. 验证笔记中是否包含该关键词（区分大小写）
4. 增加 `max_results` 参数值

#### 问题：笔记目录生成缓慢

**症状**：
- 生成目录时响应时间长
- 生命体有大量笔记（>1000 页）

**解决方案**：
1. 这是正常现象，需要遍历所有笔记
2. 考虑定期归档旧笔记
3. 使用搜索功能代替目录浏览
4. 计划中的优化：添加目录缓存机制

---

### 知识网络问题

#### 问题：知识查询返回空结果

**症状**：
```
No knowledge triples found
```

**解决方案**：
1. 验证主语和谓语的拼写
2. 检查知识是否已添加到网络中
3. 使用搜索功能进行模糊匹配：
```json
{
  "action": "search",
  "query": "关键词"
}
```

#### 问题：知识路径查找失败

**症状**：
```
No path found between concepts
```

**解决方案**：
1. 验证两个概念是否存在于知识网络中
2. 检查是否存在关联路径（可能没有直接或间接关系）
3. 尝试添加更多知识以建立连接
4. 降低路径长度限制（如果设置了的话）

#### 问题：知识验证失败

**症状**：
```
Knowledge validation failed
```

**解决方案**：
1. 检查三元组格式是否正确（主语、谓语、宾语必填）
2. 验证置信度在 0.0-1.0 范围内
3. 检查是否有重复的三元组
4. 查看验证错误详情以了解具体问题

#### 问题：知识网络统计信息不准确

**症状**：
- 统计数字与预期不符
- 添加知识后统计未更新

**解决方案**：
1. 统计信息可能需要几秒钟更新（缓存）
2. 检查是否有删除操作未成功执行
3. 重启应用程序强制刷新统计
4. 通过 API 重新查询统计信息

---

### 项目管理问题

#### 问题：无法创建项目

**症状**：
```
Failed to create project
```

**解决方案**：
1. 检查项目名称是否为空（必填）
2. 验证项目名称不重复
3. 检查存储路径有写入权限
4. 查看日志获取详细错误信息

#### 问题：项目数据丢失

**症状**：
- 项目信息无法加载
- 项目文件损坏

**解决方案**：
1. 检查项目存储目录是否存在
2. 从备份恢复项目数据
3. 验证 JSON 文件格式正确
4. 手动修复损坏的项目文件

---

## 调试

### 启用详细日志

编辑配置：
```json
{
  "logging": {
    "level": "debug"
  }
}
```

### 检查日志

日志存储在：
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

### 使用调试器

```bash
# 使用调试器运行
dotnet run --project src/SiliconLife.Default --configuration Debug

# 附加调试器
# 通过 IDE：附加到进程 > SiliconLife.Default
```

---

## 性能问题

### 响应时间慢

**优化**：
1. 降低 AI 模型复杂度
2. 启用缓存
3. 清理旧数据
4. 增加系统资源

### CPU 使用率高

**检查**：
- 运行太多生命体
- 工具中的无限循环
- 频繁的定时器执行

**解决方案**：
- 减少并发生命体
- 优化工具代码
- 调整定时器间隔

### 内存使用率高

**监控**：
```bash
# 通过 Web UI：仪表板 > 内存
```

**优化**：
- 清理旧记忆
- 减少上下文大小
- 实现分页

---

## 获取帮助

### 查看文档

- [快速开始指南](getting-started.md)
- [开发指南](development-guide.md)
- [API 参考](api-reference.md)
- [架构指南](architecture.md)

### 检查日志

始终首先检查日志以获取错误详情。

### 社区支持

- GitHub Issues：报告 bug
- Discussions：提问
- 文档：搜索解决方案

---

## 紧急程序

### 系统崩溃

1. 检查日志以获取原因
2. 重启应用程序：
```bash
dotnet run --project src/SiliconLife.Default
```

3. 如需从备份恢复

### 数据丢失

1. 立即停止应用程序
2. 检查备份文件
3. 恢复数据
4. 验证完整性

### 安全漏洞

1. 停止所有生命体
2. 撤销所有权限
3. 检查审计日志
4. 查看访问控制
5. 以限制权限重启

---

## 预防

### 最佳实践

1. **定期备份**
   - 备份数据目录
   - 备份配置
   - 测试恢复过程

2. **监控资源**
   - 监视 CPU/内存使用
   - 监控磁盘空间
   - 检查网络连接

3. **保持更新**
   - 更新 .NET SDK
   - 更新依赖
   - 应用安全补丁

4. **测试变更**
   - 首先在开发中测试
   - 使用版本控制
   - 记录变更

---

## 下一步

- 📚 阅读[架构指南](architecture.md)
- 🛠️ 查看[开发指南](development-guide.md)
- 🚀 查看[快速开始指南](getting-started.md)
- 🔒 查看[安全文档](security.md)
