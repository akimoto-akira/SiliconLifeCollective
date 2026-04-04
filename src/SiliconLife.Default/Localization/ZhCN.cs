// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Chinese (Simplified) localization implementation
/// </summary>
public class ZhCN : DefaultLocalizationBase
{
    /// <summary>
    /// Gets the language code
    /// </summary>
    public override string LanguageCode => "zh-CN";

    /// <summary>
    /// Gets the language name
    /// </summary>
    public override string LanguageName => "简体中文";

    /// <summary>
    /// Gets the welcome message
    /// </summary>
    public override string WelcomeMessage => "欢迎使用硅基生命群！";

    /// <summary>
    /// Gets the input prompt
    /// </summary>
    public override string InputPrompt => "> ";

    /// <summary>
    /// Gets the shutdown message
    /// </summary>
    public override string ShutdownMessage => "正在关闭...";

    /// <summary>
    /// Gets the config corrupted error message
    /// </summary>
    public override string ConfigCorruptedError => "配置文件损坏，已使用默认配置";

    /// <summary>
    /// Gets the config created message
    /// </summary>
    public override string ConfigCreatedWithDefaults => "配置文件不存在，已创建默认配置";

    /// <summary>
    /// Gets the AI connection error message
    /// </summary>
    public override string AIConnectionError => "无法连接到 AI 服务，请检查 Ollama 是否正在运行";

    /// <summary>
    /// Gets the AI request error message
    /// </summary>
    public override string AIRequestError => "AI 请求失败";

    /// <summary>
    /// Gets the data directory create error message
    /// </summary>
    public override string DataDirectoryCreateError => "无法创建数据目录";

    /// <summary>
    /// Gets the thinking message
    /// </summary>
    public override string ThinkingMessage => "思考中...";

    /// <summary>
    /// Gets the tool call message
    /// </summary>
    public override string ToolCallMessage => "执行工具中...";

    /// <summary>
    /// Gets the error message
    /// </summary>
    public override string ErrorMessage => "错误";

    /// <summary>
    /// Gets the unexpected error message
    /// </summary>
    public override string UnexpectedErrorMessage => "意外错误";

    /// <summary>
    /// Gets the permission denied message
    /// </summary>
    public override string PermissionDeniedMessage => "权限被拒绝";

    /// <summary>
    /// Gets the permission ask prompt
    /// </summary>
    public override string PermissionAskPrompt => "是否允许？(y/n): ";

    /// <summary>
    /// Gets the header displayed for permission requests
    /// </summary>
    public override string PermissionRequestHeader => "[权限请求]";

    /// <summary>
    /// Gets the label for the allow code in permission prompts
    /// </summary>
    public override string AllowCodeLabel => "允许码";

    /// <summary>
    /// Gets the label for the deny code in permission prompts
    /// </summary>
    public override string DenyCodeLabel => "拒绝码";

    /// <summary>
    /// Gets the instruction text for replying to permission prompts
    /// </summary>
    public override string PermissionReplyInstruction => "输入验证码确认，或输入其他内容拒绝";

    /// <summary>
    /// Gets the prompt for asking whether to cache a permission decision
    /// </summary>
    public override string AddToCachePrompt => "是否缓存此决定？(y/n): ";

    /// <summary>
    /// Gets the localized display name for a permission type
    /// </summary>
    public override string GetPermissionTypeName(PermissionType permissionType) => permissionType switch
    {
        PermissionType.NetworkAccess => "网络访问",
        PermissionType.CommandLine => "命令行执行",
        PermissionType.FileAccess => "文件访问",
        PermissionType.Function => "函数调用",
        PermissionType.DataAccess => "数据访问",
        _ => permissionType.ToString()
    };

    /// <summary>
    /// Gets the system prompt for memory compression
    /// </summary>
    public override string MemoryCompressionSystemPrompt => "你是一个记忆压缩助手。请将以下时间范围内的记忆内容压缩成简洁的摘要，保留关键信息。";

    /// <summary>
    /// Gets the user prompt template for memory compression
    /// </summary>
    public override string GetMemoryCompressionUserPrompt(string levelDesc, string rangeDesc, string contentText)
    {
        return $"记忆压缩：{levelDesc}。时间范围：{rangeDesc}。\n\n记忆内容：\n{contentText}";
    }

    // ===== Init Page Localization =====

    public override string InitPageTitle => "初始化";
    public override string InitDescription => "首次使用，请完成基本配置";
    public override string InitNicknameLabel => "用户昵称";
    public override string InitNicknamePlaceholder => "请输入你的昵称";
    public override string InitEndpointLabel => "AI API 端点";
    public override string InitEndpointPlaceholder => "例如: http://localhost:11434";
    public override string InitSkinLabel => "皮肤";
    public override string InitSkinPlaceholder => "留空使用默认皮肤";
    public override string InitDataDirectoryLabel => "数据目录";
    public override string InitDataDirectoryPlaceholder => "例如: ./data";
    public override string InitDataDirectoryBrowse => "浏览...";
    public override string InitSkinSelected => "\u2713 已选择";
    public override string InitSkinPreviewTitle => "预览效果";
    public override string InitSkinPreviewCardTitle => "卡片标题";
    public override string InitSkinPreviewCardContent => "这是一个示例卡片，展示了该皮肤风格下的UI效果。";
    public override string InitSkinPreviewPrimaryBtn => "主要按钮";
    public override string InitSkinPreviewSecondaryBtn => "次要按钮";
    public override string InitSubmitButton => "完成初始化";
    public override string InitFooterHint => "配置完成后可随时在设置页面修改";
    public override string InitNicknameRequiredError => "请输入用户昵称";
    public override string InitDataDirectoryRequiredError => "请选择数据目录";
    public override string InitCuratorNameLabel => "硅基人名称";
    public override string InitCuratorNamePlaceholder => "请输入第一个硅基人的名称";
    public override string InitCuratorNameRequiredError => "请输入硅基人名称";
    public override string InitLanguageLabel => "语言 / Language";
    public override string InitLanguageSwitchBtn => "应用";

    // ===== Chat Localization =====

    public override string SingleChatNameFormat => "与{0}聊天";
}
