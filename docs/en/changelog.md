# Changelog

**English** | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md) | [Русский](../ru-RU/changelog.md)

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## About This Changelog

### Dual Project Versions

This project provides two implementation versions:

- **SiliconLife.Default**: Default implementation, primarily used for verifying architecture feasibility. Console application with file system JSON storage.
- **SiliconLife.Fast**: Production-ready version. Cross-platform desktop application (Windows / macOS / Linux), SpeedyPack in-memory storage + asynchronous persistence, deeply performance-optimized.

Both versions share the same interfaces and functionality, differing only in storage implementation and runtime mode. SiliconLife.Default serves as the architecture verification baseline, while SiliconLife.Fast is the recommended production version.

### Project Origin

- This project originated on March 20, 2026.
- Prior to this project, a verification Demo failed due to unreasonable architecture design, making it impossible to integrate with multiple AI platforms.

### AI IDE Tools Used

#### Kiro (Amazon AWS)
- The project was initially maintained by Kiro, started using Spec mode.
- Kiro is an agentic AI development environment built by Amazon AWS.
- Based on Code OSS (VS Code), supports VS Code settings and Open VSX compatible plugins.
- Features spec-driven development workflow for structured AI coding.

#### Comate AI IDE (Baidu)
- Occasionally used for copywriting and documentation work.
- Comate AI IDE is an AI-native development environment tool released by Baidu Wenxin on June 23, 2025.
- The industry's first multi-modal, multi-agent collaborative AI IDE.
- Features include design-to-code conversion and full-process AI-assisted coding.
- Powered by Baidu Wenxin 4.0 X1 Turbo model.

#### Trae (ByteDance)
- Used from October 2025 to April 2026.
- AI IDE supporting intelligent code generation and project management.

#### Qoder (Alibaba)
- Used for project maintenance since April 18, 2026.
- AI coding platform supporting code analysis, documentation generation, and multi-agent collaboration.

#### CatPaw (Meituan)
- Used in combination with Qoder since May 6, 2026.
- Based on Meituan's self-developed LongCat series models, with strong full-code architecture refactoring capabilities.

#### DuMate (Baidu Qianfan)
- Used since July 2026 for code development, localization, and documentation.
- A general-purpose AI assistant running on the Qianfan desktop platform, capable of multi-tool orchestration, file operations, browser automation, and multi-step task execution.
- Directly reads and writes local files, executes shell commands, and performs web searches on the user's Windows desktop.

### Requirements Documentation

- The requirements documentation for this project is not publicly available.
- Requirements were iteratively validated across 12+ international AI platforms and large model series, producing over 2,000 lines of nearly incomprehensible user-story-driven requirements documentation.

---

## [Unreleased]

### 2026-08-17

#### New Features
- `c7b575b` - Implement MCP integration — external server tool access, configuration management, and help documentation
  - New MCP core (SiliconLife.Core/Mcp/): McpManager server lifecycle management, stdio/http dual transport, McpClientConnection connection wrapper, per-server tool wrapping and injection into all Silicon Beings with `mcp_{serverId}_{toolName}` naming
  - New web management page (/mcp) with 7 API endpoints (list-servers/list-tools/add-server/toggle/remove-server/reconnect/test-tool)
  - New McpTool query tool (status/list_servers/list_tools, read-only); server add/remove is restricted to user via Web UI, AI cannot modify server list
  - Config page supports MCP server array editor (inline add/remove within modal window)
  - Register MCP help topic (🔌), 10 languages with complete help documentation
  - MCP wrapped tools appear as `execute` action in permission matrix, can be disabled per Silicon Being/project
  - 45 files changed

### 2026-08-16

#### New Features
- `5d76c5a` - Implement Skill system — reusable abstraction layer for tool orchestration and prompt templates
  - New SkillDefinition (id/description/parameter schema/system prompt template/tool whitelist/action restrictions/max rounds/timeout/completion action/trigger mode)
  - New SkillManager: skill registration center + execution engine (sub AIRequest loop, recursion guard, global round and timeout clamping)
  - Dual trigger modes: Manual (AI function call, skill injected as ToolDefinition, scheduler-side priority routing) + Auto (schedule-based, supports `HH:mm` / `N s|m|h|d` / cron subset)
  - Markdown-first storage (YAML frontmatter + prompt body), pure Markdown auto-completes metadata by AI (user fields not overwritten)
  - Hot-reload (30-second fingerprint detection), version archiving (skills/archive/), 3 built-in skills (summarize_document/code_review/research_topic)
  - New skill tool (create/list/update/update_from_md/delete/export/export_md/import/import_md)
  - New skill management page (/skill) with 10 API endpoints; quota MaxCustomSkillsPerBeing (default 50)
  - Permissions: skill-level `execute` action permission, skill-internal tool whitelist and Silicon Being permissions take strict-side union
- `b60fc68` - Update Qianfan model list and context window mapping — add glm-5.2/glm-5.1/deepseek-v4-pro/deepseek-v4-flash/kimi-k2.6/ernie-5.1/qianfan-code-latest models, 1M/128K tiered context window and vision capability mapping

### 2026-08-15

#### New Features
- `eaa8417` - Implement IM platform OAuth authorization wizard and config secret environment variable resolution
  - New ImOAuthController/ImOAuthService supporting Feishu OAuth authorization flow (authorize/callback/status), with state for CSRF prevention, 5-minute timeout, SSE status push
  - New IMProviderRegistry for unified IM platform metadata management (config field schema/OAuth endpoint templates/Provider factory)
  - New ConfigSecretResolver for resolving `${ENV_VAR}` placeholders in config, deep-copy replacement without writing back to original config
  - Config page integrates IM authorization wizard UI (inline authorization area + SSE real-time status)
  - Complete IM authorization status/help text translations for 13 language files

### 2026-07-26

#### Refactoring
- `ffc45c2` - Refactor IM platform to multi-instance configuration architecture — IMPlatforms list-ified (each platform independently start/stop), AggregateIMProvider aggregates multi-platform message send/receive and permission racing, config page multi-instance editor

### 2026-07-19

#### New Features
- `9bf2103` - Speedy.Manager tree view integrates multi-select delete and multi-select export

#### Fixes
- `0df0674` - Fix Speedy.Manager multi-select delete only deleting first item

### 2026-07-16

#### New Features
- `7431312` - Complete AI client config translations for 13 language files - CsCZ/PlPL upgraded from stub to full dictionary implementation, remaining 10 files add ConfigDisplayNames/ConfigDescriptions/ConfigGroupNames entries for 7 new clients (DeepSeek/ZhipuGLM/MoonshotKimi/SiliconFlow/MiniMax/Ernie/Hunyuan), sync update 6 ClientFactory config key metadata
  - 20 files changed

#### Documentation
- `ce36036` - Rewrite all 13 language versions of changelog content after 2026-05-26 based on git records
- `d6608ea` - Add DuMate (Baidu Qianfan) AI IDE tool introduction to all 13 language versions of changelog
  - 13 files changed

#### Collaboration Framework
- `c607c97` - Register DuMate (Baidu Qianfan) as resident AI actor in .ai-collab registry
  - 1 file changed


### 2026-07-15

#### New Features
- `c007263` - Complete help documentation for 10 AI clients - HelpTopics registers 10 topics, HelpLocalizationBase adds 30 abstract properties, 12 language files implement full Markdown help content (platform intro/registration steps/configuration methods/available models/billing/FAQ), covering Herdsman/LongCat/QiniuAI/DeepSeek/ZhipuGLM/MoonshotKimi/SiliconFlow/MiniMax/Ernie/Hunyuan
  - 12 files changed
- `4634e33` - Implement 7 domestic AI platform clients (DeepSeek/Zhipu GLM/Moonshot Kimi/SiliconFlow/MiniMax/Baidu Ernie/Tencent Hunyuan) - 14 independent class files, following LongCatClient style, no inheritance, all OpenAI compatible + Bearer Token, supporting Tool Calling/streaming/thinking mode, registered in DefaultSiliconBeing and DefaultSiliconBeingFactory
  - 16 files changed

#### Documentation
- `108c4ea` - Update all 13 language docs to reflect 7 new AI clients - status 📋→✅, 01.AI marked deprecated
  - 94 files changed


### 2026-07-14

#### Documentation
- `344b429` - Add "Deprecated" status to AI platform status in all language architecture.md, mark 01.AI as deprecated (new user registration stopped)
  - 13 files changed


### 2026-07-07

#### Cleanup
- `e06e6f2` - Remove OsmStore toolchain and TravelCodeWikiWithAI plugin - delete tools/OsmStore.* three projects, delete src/TravelCodeWikiWithAI/ plugin project, clean sln references, project returns to standalone TCW development route
  - 45 files changed


### 2026-07-06

#### Fixes
- `1b15886` - OSM data model standardization and element type safety fix
  - 7 files changed


### 2026-07-05

#### New Features
- `be4320b` - TravelCodeWikiWithAI adds CLDR data provider module
  - 4 files changed


### 2026-07-04

#### New Features
- `dbcabf3` - Plugin permission system enhancement - refactor network/file IO to Executor mode + GeneratedCodeAttribute whitelist exemption
  - 34 files changed
- `e84bb63` - Fix compilation errors and add TravelCodeWikiWithAI project
  - 53 files changed

#### Refactoring
- `9e5a345` - TravelCodeWikiWithAI fully migrates PBF to synchronous online OSM API
  - 4 files changed


### 2026-05-31

#### New Features
- `a5f37bd` - Update project thinking, conversation system and storage related features
  - 13 files changed


### 2026-05-30

#### New Features
- `c3cf429` - Add QiniuAIClient AI client (Qiniu Cloud AI large model inference service) (ref task-409)
  - 20 files changed
- `d04131f` - Add LongCatClient AI client (Meituan LongCat large model) (ref task-408)
  - 19 files changed

#### Collaboration Framework
- `e9564f5` - Update all modified files
  - 140 files changed
- `9c8b42f` - Archive sessions and changes from 2026-05-29
  - 20 files changed


### 2026-05-29

#### New Features
- `d548e48` - Project thinking detail page groups messages by Cycle with collapsible sections (ref task-407)
  - 23 files changed
- `28d893d` - IAIClient adds multimodal capability declaration interface + ChatMessage adds multimodal fields (ref task-402)
  - 13 files changed
- `ebe6a49` - Project thinking detail page adds session status, creation time, completion time display (ref task-406)
  - 22 files changed
- `9a53d55` - IAIClient adds ContextWindowTokens + Token budget system + factory configuration (ref task-401, task-403)
  - 26 files changed
- `202b99c` - Add HerdsmanClient AI client + fix initialization UI dropdown not refreshing (ref task-399, task-400)
  - 20 files changed
- `285ab2f` - Project processing record frontend display (ref task-397)
  - 25 files changed
- `b4b633f` - ThinkOnProject pseudo-Session multi-round dialogue mechanism (ref task-395)
  - 13 files changed
- `d3e543f` - ThinkOnProject scenario context adds available silicon being information (ref task-394)
  - 21 files changed
- `07eb628` - BuildRequest dynamically injects silicon being project ownership information (ref task-396)
  - 21 files changed
- `2089696` - Tool adds Project scenario support + PluginLoader multi-directory unified refactoring
  - 12 files changed

#### Fixes
- `b80a33b` - Fix project thinking detail page loading hint text hardcoded in English and missing localization (ref task-405)
  - 6 files changed
- `90b60c5` - Fix AI body Content and Thinking being hidden in tool call rounds (ref task-404)
  - 8 files changed
- `a7d9a97` - Fix ThinkOnProject multi-round loop continuation and project reminder message loss
  - 6 files changed
- `c0838dd` - Fix ProjectThinkSession messages not written to Cycle and history deleted after completion (ref task-398)
  - 7 files changed
- `f3d1794` - Fix silicon being Project/Broadcast/Stopped status localization missing and display abnormality (ref task-393)
  - 20 files changed
- `3eaa90d` - Remove solution references to deleted project TravelCodeWikiWithAI
  - 1 file changed

#### Collaboration Framework
- `f3cbed7` - Register task-394~396 (ThinkOnProject enhancements)
  - 3 files changed
- `e1971f5` - Register task-393 (BeingActivity localization & display fix)
  - 1 file changed
- `e710fa4` - Update changes commitHash and state session end
  - 2 files changed
- `4cacc4a` - Archive sessions and changes from 2026-05-28
  - 4 files changed


### 2026-05-28

#### New Features
- `ae8b673` - Plugin directory configuration upgraded from single path to multi-directory list (ref task-391)
  - 29 files changed
- `aac46c1` - PluginLoader adds CS source mode, compile-load plugins when no DLL exists (ref task-389)
  - 6 files changed

#### Fixes
- `63047b0` - Register all PluginLoaders to ServiceLocator, fix incomplete multi-directory plugin reflection (ref task-391)
  - 3 files changed
- `fcad655` - Fix directoryList browse button interaction issue (ref task-392)
  - 9 files changed

#### Documentation
- `e6d3037` - PluginDemo-22 CS source code compile-load mode example (ref task-390)
  - 21 files changed

#### Collaboration Framework
- `09d9e9c` - Archive 30 completed tasks (task-362~task-391)
  - 2 files changed
- `66204a1` - Archive 2026-05-28 sessions (8) and changes (8)
  - 18 files changed
- `308a8d0` - Update task-391 relatedCommit
  - 1 file changed
- `6fc4e05` - Register task-389 (CS source mode) and task-390 (PluginDemo-22)
  - 1 file changed


### 2026-05-27

#### New Features
- `e154a18` - Complete PluginDemo-21 WorkflowTemplate full business workflow example (ref task-388)
  - 19 files changed
- `aa771b3` - Implement PluginCapability declarative permission system (ref task-379)
  - 9 files changed
- `5e5e9d1` - Add 04-SafeSystemIO System.IO whitelist safe type example (ref task-370)
  - 20 files changed

#### Documentation
- `48f6702` - Align 19-TickObject and 20-SpeedyPack all language README translations to baseline (ref task-386, task-387)
  - 119 files changed
- `5d570e5` - Complete task-378 forbidden string reflection bypass counter-example (ref task-378)
  - 19 files changed
- `348c410` - PluginDemo-11 forbidden P/Invoke and unsafe code counter-example (ref task-377)
  - 19 files changed
- `fc92a49` - PluginDemo-10 forbidden reflection operation counter-example (ref task-376)
  - 19 files changed
- `826ad2a` - Create PluginDemo-09 forbidden process operation counter-example plugin (ref task-375)
  - 19 files changed
- `7870b05` - Add PluginDemo-08 forbidden network operation counter-example (ref task-374)
  - 15 files changed
- `8636e31` - PluginDemo-07 forbidden file I/O operation counter-example (ref task-373)
  - 19 files changed
- `322312e` - Add PluginDemo-06 TrustedAssemblies trusted dependency example (ref task-372)
  - 19 files changed
- `6df98a0` - Add IWorkflowPlugin workflow plugin example (ref task-371)
  - 20 files changed
- `f3787ba` - PluginDemo-03 IObjectFactory registration and creation example (ref task-369)
  - 20 files changed
- `bb4324d` - PluginDemo-02 ITypeRegistry registration and query example (ref task-368)
  - 20 files changed
- `bbdfa3c` - PluginDemo-01 minimal IPlugin implementation example (ref task-367)
  - 19 files changed

#### Collaboration Framework
- `de44057` - Archive sessions and changes from May 25 and 27
  - 58 files changed
- `9e4a84c` - Update tasks.json lastCommitHash to 48f6702
  - 1 file changed
- `beb58b2` - Add taskIndex index (8 pending, 19 completed)
  - 1 file changed
- `63f7bfc` - Update task-388 relatedCommit (ref task-388)
  - 1 file changed
- `e61be6f` - Update task-378 relatedCommit (ref task-378)
  - 1 file changed
- `dde579b` - Publish WorkflowTemplate complete usage example task (task-388)
  - 1 file changed
- `2294fa7` - Publish TickObject and SpeedyPack example tasks (task-386~387)
  - 1 file changed
- `82b9f63` - Publish 6 PluginCapability example tasks (task-380~385)
  - 1 file changed
- `588539b` - Publish PluginCapability declarative permission system task (task-379)
  - 1 file changed
- `37f9c23` - Update solution and project file references
  - 8 files changed
- `e1f7892` - Publish 12 PluginDemo pending tasks (task-367~378)
  - 3 files changed
- `87ae858` - Create PluginDemo plugin positive/negative example task registration (task-367)
  - 2 files changed
- `f77a102` - Archive sessions and changes from 2026-05-26
  - 7 files changed

## [Alpha-0.2] - 2026-05-16

### 2026-05-16

#### Release Preparation
- `476d839` - Add alpha-0.2 release tasks
  - Created task-114 (CHANGELOG writing) and task-115 (version number update)
  - 1 file changed

### 2026-05-15

#### Infrastructure
- `672627b` - Add Gitee sync workflow (with permission configuration)
  - Updated sync-from-gitee.yml workflow permission configuration
  - 1 file changed, 7 lines added, 4 lines deleted

- `3cd5256` - Add GitHub Actions auto-sync from Gitee
  - Added sync-from-gitee.yml workflow
  - 1 file changed, 50 lines added

#### Documentation Update
- `aa1d2ad` - Update all 11-language README/architecture/getting-started docs to reflect SiliconLife.Fast multi-platform support (ref task-112, task-113)
  - Corrected documentation describing SiliconLife.Fast as Windows-only, reflecting actual multi-platform support (Windows / macOS / Linux)
  - Updated README.md, architecture.md, getting-started.md for 11 languages
  - SelectComponent adds hint property support
  - ConfigView enum dropdown passes hint
  - 11-language localization adds SelectSearchHint key
  - 53 files changed, 690 lines added, 194 lines deleted

#### Task System
- `3329f3d` - Add task system patrol mechanism + localization bug fix tasks
  - Created task-113: Fix about page localization issue
  - Updated task-112: Update Fast version docs for Linux support
  - Archived completed tasks (11) to .ai-collab/archive/
  - Patrol mechanism configured: quick patrol (every 30 minutes) + full patrol (daily at 06:00)
  - 2 files changed, 148 lines added, 171 lines deleted

#### Collaboration Framework
- `6038e22` - Register coze-agent to .ai-collab collaboration registry
  - Added Coze platform resident AI registration info
  - 1 file changed

### 2026-05-14

#### AI Collaboration Framework
- `7344fbb` - Remove handoff mode, switch to task list driven (v2.0)
  - Restructured .ai-collab directory from handoff mode to task list driven
  - Added tasks.json task list core file
  - Added activity.log operation log
  - Added changes/ and sessions/ directories

- `589a48e` - Add .ai-collab session records
  - Added AI collaboration session state records

- `5481bcf` - Register Qoder AI IDE to collaboration registry
  - Added Qoder AI coding assistant registration info

- `e2d7b61` - Add relatedCommit and changes commitHash to tasks.json
  - Improved task metadata association

- `a087f0c` - Accept all task-101~110 tasks
  - Confirmed all 10 task fixes completed

#### Bug Fixes
- `fac9435` - Complete all 10 task-101~110 fixes and implementations
  - Fix search select component missing hint text
  - Fix about page localization issue
  - Fix help system search JS error
  - 39 files changed, 684 lines added, 121 lines deleted

- `c46dfbc` - Complete all pending tasks (task-001~006)
  - Completed initial 6 pending tasks

- `ec176b2` - Override task list - code review found 10 new bugs
  - Created task-101~110, 10 new tasks

#### Refactoring
- `ab15915` - Unify copyright headers + fix HelpController BOM and HelpView search JS
  - Unified Apache 2.0 copyright headers for all C# source files
  - Fixed HelpController BOM encoding issue
  - Fixed HelpView search JavaScript error

#### New Features
- `18a6f5d` - Create MCP browser capability server (ref task-111)
  - Added SiliconLife.McpServer project
  - Implemented Playwright browser automation MCP server

- `9eb251a` - Remove SiliconLife.McpServer module (ref task-111)
  - Removed standalone MCP server, functionality integrated into main project

### 2026-05-13

#### Localization
- `7a62590` - Add Polish localization support
  - Added pl-PL Polish localization implementation (PlPL.cs, 1089 lines)
  - Added Polish help document localization (HelpLocalizationPlPL.cs, 3972 lines)
  - Added Polish Chinese historical calendar support (ChineseHistoricalPlPL.cs, 600 lines)
  - Added Polish tray localization (TrayPlPL.cs, 135 lines)
  - Added complete Polish document set (15 documents)
  - Language enum adds Polish
  - 35 files changed, 14379 lines added, 11 lines deleted

- `51f9c8e` - Update Ark AI references and terminology improvements in documentation
  - Updated AI client terminology in multi-language documentation

- `7587c12` - Add changelog entries for all languages
  - Synchronized changelog updates for all language versions

#### Window System Migration
- `b49a07d` - Migrate to Avalonia window resident mode
  - Removed Windows Forms dependency, fully migrated to Avalonia UI framework
  - Status window displays correctly on Linux (remote desktop verified)
  - Added window controls: context menu, double-click to open Web, close button
  - Added multi-AI collaboration framework (.ai-collab/)
  - Fixed tray icon initialization (graceful degradation)
  - Added App.axaml and App.cs Avalonia application entry points
  - 13 files changed, 1442 lines added, 541 lines deleted

- `d335aaf` - Linux platform window always visible + close confirmation dialog
  - Auto-show status window on Linux (no tray icon)
  - Show confirmation dialog when closing window on Linux
  - Windows/macOS maintain existing tray behavior
  - Support --no-tray parameter to force-disable tray
  - Added ShowMessageBoxAsync method for confirmation dialog
  - 3 files changed, 206 lines added, 29 lines deleted

#### Tray System Refactoring
- `841d384` - Refactor tray system and initialize AI collaboration framework
  - Streamlined TrayLocalizationBase removing unused properties
  - Added ShowStatus localization item
  - App.cs adds tray icon click to show status window, localized menu items
  - Program.cs moves tray icon initialization to StartAsync
  - TrayStatusWindow hides instead of exiting on close
  - Registered trae-glm5 and catpaw to .ai-collab collaboration framework
  - Updated .gitignore to ensure all .ai-collab files are tracked
  - 22 files changed, 178 lines added, 1226 lines deleted

#### Documentation
- `43653bc` - Update repository description and AI registry
  - Updated project README and .ai-collab registration info

### 2026-05-12

#### Task System Web View
- `0891b3c` - Add task execution detail and history views
  - Added TaskExecutionDetailView task execution detail view
  - Added TaskExecutionHistoryView task execution history view
  - TaskController adds execution detail and history query interfaces
  - Added TaskViewModel task view model
  - TaskCenter task center enhancement
  - TaskSystem task system update
  - 9-language localization adds task-related keys
  - 26 files changed, 803 lines added, 55 lines deleted

### 2026-05-11

#### Web Component Architecture Refactoring
- `5e687ad` - Migrate component rendering from strings to H-tree
  - ComponentBase rendering method migrated from string pattern to H-tree structure
  - All 28 components adapted to new rendering architecture (A, Accordion, Button, Calendar, Card, Chart, etc.)
  - SelectComponent significantly refactored (889 lines improved)
  - Controllers and views synchronized updates
  - 33 files changed, 667 lines added, 435 lines deleted

- `bfd332d` - Migrate Style from strings to CssBuilder inline styles
  - Added CssBuilder style builder
  - ComponentBase style system migrated from strings to structured CssBuilder
  - LoadingComponent significantly enhanced (103 lines added)
  - ConfigController, LogController, MemoryController controller style migration
  - ChatView, ConfigView, LogView, MemoryView view style migration
  - 37 files changed, 351 lines added, 157 lines deleted

#### Storage System Optimization
- `d67a7ee` - Optimize QueryLatest for large dataset queries
  - SpeedyTimeStorage QueryLatest method performance optimization
  - SpeedyLoggerProvider logger provider enhancement
  - 2 files changed, 44 lines added, 5 lines deleted

#### Calendar System Refactoring
- `9629f88` - Extract TimerExecution and enhance timer web view
  - TimerSystem extracts TimerExecution logic (175 lines removed)
  - SelectComponent significantly enhanced (427 lines improved)
  - TimerController and timer view enhanced
  - ContextManager context manager updated
  - 12 files changed, 458 lines added, 267 lines deleted

#### Localization
- `5d8ca79` - Add LogsLoading localization key
  - 9 languages add LogsLoading key
  - DefaultLocalizationBase base class adds definition
  - 11 files changed, 15 lines added

### 2026-05-10

#### Task System Refactoring
- `54394f6` - Merge task system with chat history cycle
  - ProjectTaskSystem project task system significantly streamlined (411 lines refactored)
  - TaskSystem task system streamlined (254 lines refactored)
  - TaskCenter task center refactored (188 lines improved)
  - ContextManager context manager optimized (347 lines refactored)
  - DefaultSiliconBeing silicon being enhanced
  - TimerSystem timer system integrates tasks
  - IWorkNoteStorage interface updated
  - SpeedyWorkNoteStorage and FileSystemWorkNoteStorage adapted
  - 16 files changed, 648 lines added, 897 lines deleted

### 2026-05-09

#### Web Interface Enhancement
- `bc50dd7` - Improve chat view and add audit functionality
  - Added AuditController audit controller (261 lines)
  - Added AuditView audit view (379 lines)
  - Added AuditViewModel audit view model
  - ChatView chat view significantly improved (171 lines enhanced)
  - ChatController chat controller updated
  - MarkdownEditorComponent component enhanced
  - InitController initialization controller improved
  - ChatSystem chat system adds functionality
  - 14 files changed, 1030 lines added, 112 lines deleted

- `c9babce` - Improve tool call rendering in chat view
  - ChatView tool call block rendering enhanced
  - 1 file changed, 54 lines added, 11 lines deleted

#### AI Tool Scenario System
- `ff2eddd` - Implement tool scenario filtering system
  - Added ToolScenarioAttribute tool scenario attribute (36 lines)
  - Added ChatOnlyAttribute chat-only scenario attribute (19 lines)
  - ToolManager tool manager adds scenario filtering functionality (40 lines)
  - ContextManager context manager adapts to scenario filtering
  - 4 files changed, 115 lines added, 30 lines deleted

- `5709a33` - Add scenario attributes to tool classes
  - 24 tool classes add ToolScenario attribute annotations
  - Including calendar, chat, config, curator, database, disk, dynamic compilation, and other tools
  - 24 files changed, 46 lines added, 20 lines deleted

#### Task System Refactoring
- `2f19a5f` - Refactor task system with TaskCenter and TaskEnumerator
  - Added TaskCenter task center (235 lines)
  - Added TaskEnumerator task enumerator (297 lines)
  - TaskSystem task system refactored and streamlined
  - DefaultSiliconBeing silicon being adapts to new architecture
  - DefaultSiliconBeingFactory factory updated
  - SiliconBeingBase base class enhanced
  - 7 files changed, 796 lines added, 275 lines deleted

#### Permission System Migration
- `a06ed09` - Migrate IM and permission system to App project
  - PermissionRequestQueue migrated from Default/Fast to App project (443 lines added)
  - Removed Default version WebUIProvider (403 lines deleted)
  - Removed Default version HelpTool (194 lines deleted)
  - Removed Default/Fast version duplicate PermissionRequestQueue
  - Removed Default version IMPermissionAskHandler
  - PermissionRequestController controller updated
  - 14 files changed, 496 lines added, 1183 lines deleted

#### AI Context Optimization
- `4c8aaff` - Optimize context manager and enhance service locator
  - ContextManager context manager streamlined and optimized
  - ServiceLocator service locator enhanced (36 lines added)
  - ToolManager tool manager enhanced (34 lines added)
  - DashScopeClient and VolcengineArkClient client improvements
  - Executors (CommandLine, Disk, Network) updated
  - 8 files changed, 116 lines added, 98 lines deleted

#### Localization
- `5c5eef7` - Add audit and task localization keys
  - DefaultLocalizationBase adds 127 lines of localization definitions
  - 9 languages add audit and task related keys (26 lines each)
  - 11 files changed, 387 lines added

#### Project Configuration
- `2067db6` - Update project configuration and gitignore rules
  - .gitignore rules updated
  - DefaultConfigData and Fast DefaultConfigData config enhancements
  - SpeedyWorkNoteStorage storage improvements
  - SpeedyPack core enhancements
  - 5 files changed, 32 lines added, 6 lines deleted

### 2026-05-07

#### Italian Localization
- `8adc18c` - Add Italian localization support and update multi-language documentation
  - Added it-IT Italian localization
  - Added ItIT localization implementation (1909 lines)
  - Added ChineseHistoricalItIT Chinese historical calendar Italian support (586 lines)
  - Added TrayItIT tray Italian localization (135 lines)
  - Added complete Italian document set (14 documents: README, API reference, architecture, calendar system, changelog, contributing guide, etc.)
  - Updated architecture, development guide, getting-started docs for all language versions
  - Language enum adds Italian
  - 86 files changed, 11573 lines added, 769 lines deleted

#### Documentation Sync
- `12a5deb` - Update multi-language documentation for architecture, changelog, and silicon being guide
  - 8-language README updates
  - 8-language architecture document updates
  - 8-language changelog updates
  - 8-language silicon being guide updates
  - 8-language tool reference updates
  - Glossary restructuring
  - 46 files changed, 1697 lines added, 442 lines deleted

### 2026-05-06

#### Large-Scale Module Refactoring
- `eeb3be6` - Large-scale module refactoring and reorganization
  - SiliconLife.App project structure adjustment
  - SiliconLife.Fast project reorganization
  - SiliconLife.Default project reorganization
  - SiliconLife.Common shared module reorganization
  - SiliconLife.Core core module reorganization
  - SiliconLife.Speedy storage engine reorganization
  - SiliconLife.Speedy.Manager management tool reorganization
  - 119 files changed, 6926 lines added, 3066 lines deleted

### 2026-05-04

#### AI Client
- `24d2c86` - Add VolcengineArkClient and replace Audit with Usage tracking
  - Added VolcengineArkClient Volcengine Ark AI client
  - Supports streaming and non-streaming modes
  - Built-in dual-layer rate control (self rate control + server rate limit)
  - Compatible with OpenAI API protocol
  - Audit system replaced with Usage tracking
  - 24 files changed, 802 lines added, 21 lines deleted

#### Tool System
- `f27650a` - Add hot reload tool for Fast self-restart
  - Added HotReloadTool hot reload tool
  - Supports SiliconLife.Fast online compilation, update, and restart
  - Added HotReload.exe standalone updater
  - Safe file copy mechanism (does not overwrite itself)
  - Graceful shutdown and port release wait
  - 9 files changed, 581 lines added

#### Localization
- `6a5aad8` - Update all files and add French localization support
  - Added fr-FR French localization
  - Updated all language versions
  - Help document French translation
  - Interface French translation
  - 100+ files changed

### 2026-05-03

#### Project Infrastructure
- `2664b0c` - Update project infrastructure and dependencies
  - SiliconLife.Speedy.Manager adds WPF management interface (MainForm.Designer.cs, MainForm.resx)
  - Added slc.ico icon resource (1.5MB)
  - PluginLoader significantly enhanced security scanning (622 lines added)
  - Added PermissionedStreamFactory permission stream factory (779 lines)
  - Added PermissionRequestQueue permission request queue (Default and Fast versions)
  - Added DebugLoggerProvider debug logger provider
  - ConfigDataBase config base class enhanced
  - ToolManager adds plugin tool scanning functionality (ScanAllPluginAssemblies)
  - SiliconBeingManager lifecycle management enhanced
  - DashScopeClient Alibaba Cloud AI client significantly enhanced (227 lines added)
  - DefaultSiliconBeingFactory factory enhanced
  - Web views and controllers updated (ChatView, WorkNoteView, PermissionRequestController)
  - 9-language localization adds keys
  - 35 files changed, 28080 lines added, 336 lines deleted

### 2026-05-02

#### AI Client Enhancement
- `c16f99f` - Update AI clients, Web UI, and storage components
  - DashScopeClient Alibaba Cloud client significantly improved
  - SpeedyPackAutoCompactor auto compactor optimized
  - Web view base class and BeingView improved
  - 6 files changed, 240 lines added, 81 lines deleted

#### Plugin System
- `242dc98` - Add plugin list to about page
  - AboutController adds plugin info display
  - AboutViewModel adds plugin data model
  - AboutView adds plugin list rendering
  - 9-language localization adds plugin-related keys
  - 14 files changed, 160 lines added, 1 line deleted

#### AI Optimization
- `147f8f4` - Simplify context memory prompt text
  - ContextManager optimizes AI prompts
  - 1 file changed, 1 line added, 1 line deleted

#### Speedy Storage Optimization
- `8bda2d3` - Update Speedy storage and memory controller implementation
  - SpeedyPackAutoCompactor interval correction
  - SpeedyTimeStorage path handling optimization
  - MemoryController memory controller improved
  - SpeedyPack.Manager UI update
  - 4 files changed, 21 lines added, 18 lines deleted

#### Tray Enhancement
- `8972654` - Enhance localization support for tray status window
  - 9-language tray localization adds Speedy management entry
  - TrayStatusWindow adds Speedy management menu item
  - 11 files changed, 72 lines added

#### Speedy.Manager Optimization
- `6f5db09` - Optimize SpeedyPack Manager UI and internal components
  - MainForm interface restructured
  - FreeList memory management optimized
  - WriteQueue write queue improved
  - SpeedyPack core optimized
  - 5 files changed, 96 lines added, 88 lines deleted

#### Storage System Enhancement
- `57f9d5d` - Improve storage system, add auto-compaction and incomplete date support
  - Added SpeedyPackAutoCompactor auto compaction timer (30-minute interval)
  - SpeedyPackRegistry singleton manager enhanced
  - SpeedyStorage, SpeedyTimeStorage, SpeedyWorkNoteStorage adaptation improvements
  - SpeedyPack adds FreeList free space management (149 lines)
  - PackFileWriter writer refactored and optimized
  - WriteOperation, WriteQueue write queue enhanced
  - SpeedyPackOptions config options extended
  - IncompleteDate adds comparison methods
  - PluginLoader plugin loader improved
  - Default and Fast version Program.cs initialization flow updated
  - DefaultConfigData config data simplified
  - KnowledgeNetwork knowledge network streamlined
  - ChatController, MemoryController controllers optimized
  - SpeedyPack.Manager MainForm functionality enhanced
  - 22 files changed, 639 lines added, 253 lines deleted

#### Speedy.Manager Update
- `b04ed33` - Update Speedy.Manager files

### 2026-05-01

#### Architecture Refactoring: Speedy Storage Replaces LiteDB
- `6600972` - Replace LiteDB with Speedy storage, add plugin system and Speedy project
  - **Added SiliconLife.Speedy project**: High-performance .spk storage engine
    - SpeedyPack core class (489 lines): In-memory directory mapping + entry cache + async write queue
    - SpeedyPackOptions config class: Cache TTL, max cache entries, read-only mode
    - IPackTransaction transaction interface: Supports atomic write operations
    - SpkFileInfo file info class
    - Internal directory: DirectoryMap, EntryCache, PackFileReader, PackFileWriter, WriteQueue, WriteOperation, SpeedyTransaction, SpkHeader, PathNormalizer, FreeList
    - Depends on MessagePack 3.1.4 for binary serialization (LZ4 compression)
  - **Added SiliconLife.Speedy.Manager project**: WPF management tool
    - MVVM architecture: MainViewModel, DirectoryTreeViewModel, ContentViewerViewModel, etc.
    - Service layer: PackService, FileDialogService, RecentFilesService, NotificationService
    - Converters: BoolToVisibility, ByteSizeToString, ContentTypeToIcon, NullToCollapsed
    - Views: MainWindow, DirectoryTreeView, ContentViewerPanel, MetadataPanel
    - Dialogs: FileInfoDialog, ImportDialog, NewEntryDialog
  - **SiliconLife.Fast storage migration**: LiteDB → SpeedyPack
    - Added SpeedyStorage (IStorage adapter)
    - Added SpeedyTimeStorage (ITimeStorage adapter)
    - Added SpeedyWorkNoteStorage (IWorkNoteStorage adapter)
    - Added SpeedyPackRegistry (process-level singleton management)
    - Added SpeedyPackAutoCompactor (auto compaction timer)
    - Removed LiteDB-related storage implementations (LiteDBStorage, LiteDBTimeStorage, LiteDBWorkNoteStorage, LiteDBLoggerProvider, LiteDBManager, LiteDBModels)
    - Removed LiteDB management window related code
  - **Plugin System**:
    - Added IPlugin interface (Core/Plugins/IPlugin.cs)
    - Added PluginLoader plugin loader (Core/Plugins/PluginLoader.cs)
    - Support loading plugin DLLs from directory
    - Security scanning: Prohibited namespace checking (System.IO, System.Net, Microsoft.CodeAnalysis, etc.)
    - Trusted assembly whitelist (Google.Protobuf, Newtonsoft.Json, MessagePack, etc.)
    - Custom AssemblyLoadContext isolated loading
    - ToolManager adds ScanAllPluginAssemblies method
    - CoreHost integrates plugin loader
  - 119 files changed, 6926 lines added, 3066 lines deleted

#### Silicon Being Enhancement
- `3aef4c3` - Add Stopped activity state and error handling improvements
  - Silicon Being adds Stopped state
  - Error handling and recovery mechanism enhanced

#### Localization Update
- `513c65d` - Update all language versions and documentation
  - Added MarkdownEditorComponent component (625 lines)
  - Added DetailsComponent component (130 lines)
  - Added AccordionComponent accordion component (285 lines)
  - BeingController, ChatController, MemoryController, PermissionController controllers updated
  - BeingView, ChatView, MemoryView, SoulEditorView views refactored
  - Removed old MarkdownEditorView
  - InitController component migration
  - 115 files changed, 5761 lines added, 2362 lines deleted

### 2026-04-30

#### System Tray Functionality
- `101b203` - Implement tray status window and ApplicationContext
  - Added tray icon resources (alpha.png, noWord.png, slc.ico, wordIcon.png)
  - Implemented TrayStatusWindow status window
  - Supports 9-language tray localization (TrayCsCZ, TrayDeDE, TrayEnUS, etc.)
  - TrayLocalizationBase abstract base class
  - 24 files changed, 27995 lines added, 1 line deleted (including resource files)

#### Component-Based UI Architecture
- `e61cfaa` - Complete component-based UI architecture, implement 24 components
  - MVP stage (8): ComponentBase, Div, Span, Button, Input, Form, Select, Label
  - Second stage (6): Accordion, Card, Tabs, Table, Modal, Message
  - Third stage (5): Calendar, Tree, Chart, FileUpload, RichText
  - Added Js, Behavior, DomUpdate, and other helper classes
  - 25 files changed, 2666 lines added

- `7449e51` - Improve component system and add new skin themes
  - Enhanced A, Button, Div, Form, Input, and other components
  - Added 3 skin themes: HighContrast, Light, Minimal
  - Updated existing skins (Admin, Chat, Creative, Dev)
  - InitController component migration
  - 32 files changed, 1466 lines added, 1238 lines deleted

- `1ba8636` - Start InitController component migration (in progress)
  - 9 files changed, 574 lines added, 145 lines deleted

#### Storage System Unification
- `895dff9` - Unify soul.md and state.json to use IStorage interface
  - DefaultSiliconBeing uses IStorage to read/write soul files and state
  - Added StateFileManager state file manager
  - SoulFileManager refactored to adapt to IStorage
  - 8 files changed, 201 lines added, 116 lines deleted

#### LiteDB Management Enhancement
- `a34bef4` - Add LiteDBManager and enhance tray localization
  - Tray menu adds LiteDB management entry
  - 9-language tray localization updated
  - 10 files changed, 196 lines added

- `c4a79ca` - Add language-aware localization factory for LiteDB management window
  - 1 file changed, 78 lines added

- `5ebc55e` - Convert LiteDBAdminLocalization to abstract base class
  - 10 files changed, 1356 lines added

#### Config System Fix
- `2da5256` - Add ConfigExists abstract method and fix LiteDB duplicate config records
  - ConfigDataBase adds ConfigExists method
  - Fast version DefaultConfigData implements LiteDB config existence check
  - Fixed LiteDB duplicate config key issue
  - 9 files changed, 210 lines added, 2 lines deleted

#### Chat and View Optimization
- `d3618ec` - Optimize chat sessions, storage system, time model, and view base classes
  - BroadcastChannel, GroupChatSession, SingleChatSession optimized
  - ITimeStorage adds query methods
  - FileSystemStorage and LiteDBStorage synchronized updates
  - ViewBase refactored and optimized (Default and Fast versions)
  - 11 files changed, 622 lines added, 392 lines deleted

### 2026-04-29

#### Architecture Refactoring: Shared Module Extraction
- `a102428` - Migrate shared modules from SiliconLife.Default to SiliconLife.Common
  - Extracted 32 calendar implementations to Common project
  - Extracted localization base classes and 21 language implementations to Common project
  - Extracted permission manager and default silicon being implementation to Common project
  - Extracted 23 built-in tool implementations to Common project
  - Extracted Playwright WebView implementation to Common project
  - Updated namespaces to SiliconLife.Collective
  - 122 files changed, 586 lines added, 343 lines deleted

#### Code Quality Improvement
- `17566fe` - Replace Console.WriteLine with logging system in Core, Common, and Default projects
  - ContextManager, AuditLogger, DefaultConfigData, and 6 other files updated
  - Unified use of ILogger interface, improved code maintainability
  - 6 files changed, 12 lines added, 8 lines deleted

#### SiliconLife.Fast High-Performance Version
- `54a0307` - Add SiliconLife.Fast project and complete compilation fixes
  - Complete Windows Forms application entry point
  - System tray support (NotifyIcon)
  - Ported all Web UI controllers (20+)
  - Ported all Web view components
  - Ported 4 skin themes (Admin, Chat, Creative, Dev)
  - 125 files changed, 61186 lines added

#### Multi-Language Documentation Sync
- `265fde8` - Sync dual-version architecture docs to all languages
  - Updated architecture.md, changelog.md for 7 languages
  - Updated contributing.md for 6 languages
  - Updated getting-started.md, roadmap.md for 7 languages
  - 47 files changed, 1214 lines added, 38 lines deleted

#### LiteDB Storage System (Fast Version)
- `4704862` - Add LiteDB dependency and infrastructure
  - Added LiteDBManager management class
  - Added LiteDBModels data model
  - 3 files changed, 252 lines added

- `4220036` - Implement LiteDB storage classes
  - LiteDBStorage: Implements IStorage interface
  - LiteDBTimeStorage: Implements ITimeStorage interface
  - LiteDBWorkNoteStorage: Implements IWorkNoteStorage interface
  - 3 files changed, 581 lines added

- `38ebd23` - Migrate config and logging system to LiteDB
  - DefaultConfigData adapts to LiteDB storage
  - Added LiteDBLoggerProvider logger provider
  - 2 files changed, 203 lines added, 67 lines deleted

- `e687157` - Migrate knowledge network from file system to LiteDB
  - KnowledgeNetwork fully refactored, using LiteDB to store triple data
  - 1 file changed, 231 lines added, 72 lines deleted

- `4220169` - Integrate LiteDB storage into Program and ProjectManager
  - Program.cs initializes LiteDB storage
  - ProjectManager adapts to LiteDB work note storage
  - 2 files changed, 40 lines added, 17 lines deleted

- `5f3a709` - Remove deprecated file system storage implementations
  - Deleted FileSystemLoggerProvider, FileSystemStorage, FileSystemTimeStorage, etc.
  - 6 files changed, 1518 lines deleted

- `e1a4ef2` - docs: add v0.1.0-alpha version identifier to all documentation
  - 127 files changed, 2297 lines added, 2471 lines deleted

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### Storage System Refactoring
- `8dd26e3` - Unify ITimeStorage interface to use IncompleteDate and add hierarchical query API
  - Removed DateTime overload methods from ITimeStorage interface, unified to use IncompleteDate
  - IncompleteDate adds CompareTo(DateTime) comparison method and Expand() expansion method
  - Added GetEarliestTimestamp(), GetLatestTimestamp() hierarchical query API
  - Added HasSummary() and QueryWithLevel() methods, supporting time-level-based queries
  - Memory.cs refactored compression algorithm, using new hierarchical query API for improved efficiency
  - FileSystemTimeStorage.cs fully implements new interface methods
  - Synchronized all callers: ChatSystem, ChatSession, BroadcastChannel, AuditLogger, TokenUsageRecord, etc.
  - Tool system updated: HelpTool, LogTool, TokenAuditTool adapted to new interface
  - Web controllers updated: AuditController, ChatController, ChatHistoryController adapted to new interface
  - 41 files changed, 1820 lines added, 903 lines deleted

### 2026-04-27

#### Help Documentation System Enhancement
- `9989d79` - Update localization, help system, and web views
  - Added IAIClientFactoryHelp.cs AI client factory help documentation interface
  - Completed 9-language translation for all help documents
  - HelpTopics.cs adds 40 help topic definitions
  - Web views fully updated: InitController, AuditView, ConfigView, KnowledgeView, LogView, etc.
  - Localization system enhanced: all language versions add new localization keys
  - AI client factory updated: DashScopeClientFactory, OllamaClientFactory improvements
  - 30 files changed, 10086 lines added, 15 lines deleted

#### Help Documentation New Content
- `e7afe94` - Add soul file and audit log help documentation
  - Added soul file management help documentation
  - Added audit log help documentation
  - HelpTopics.cs adds topic definitions
  - HelpView.cs significantly refactored, improved document rendering logic
  - PermissionView.cs refactored, improved permission management interface
  - Core modules enhanced: SiliconBeingManager, TaskSystem, ToolManager improvements
  - TaskTool.cs refactored, improved task management functionality
  - Web views fully updated: all view components synchronized updates
  - HelpController.cs simplified, optimized controller logic
  - 30 files changed, 7100 lines added, 897 lines deleted

### 2026-04-26

#### Help Documentation System
- `07895d7` - Enhance help documentation system, add 3 documents and complete 9-language translation
  - Added memory system, Ollama installation configuration, and Alibaba Cloud Bailian platform usage guides
  - Completed 9-language translation for all 10 help documents
  - Simplified HelpView rendering logic
  - 18 files changed, 14418 lines added, 1364 lines deleted

#### German Localization
- `0cfd8a1` - Add complete German (de-DE) localization support
  - Complete German localization files
  - Added Chinese historical calendar German support
  - Added help document German translation
  - Fully synchronized all documents for 9 languages
  - 135 files changed, 26186 lines added, 14371 lines deleted

#### Documentation Sync
- `3aada7d` - Sync Traditional Chinese (zh-HK) documentation with Simplified Chinese
  - 3 files changed, 519 lines added, 422 lines deleted
- `2f6abff` - Add help tool display name localization for all languages
  - 7 files changed, 47 lines added, 7 lines deleted

#### Knowledge System Refactoring
- `60944fe` - Unify namespace to SiliconLife.Collective
  - 8 files changed, 5 lines added, 8 lines deleted
- `69c51c5` - Add help documentation system and translate code comments to English
  - 29 files changed, 3385 lines added, 22 lines deleted

### 2026-04-25

#### WebView Browser Automation
- `41757c3` - Implement Playwright-based cross-platform WebView browser automation
  - 6 files changed, 1152 lines added

#### Documentation Update
- `0ff797b` - Add KnowledgeTool and WorkNoteTool documentation (7 languages)
  - 28 files changed, 4983 lines added
- `ad77415` - Update all changelog files, add 2026-04-25 Git history records
  - 7 files changed, 168 lines added

#### Project Workspace Management
- `785c551` - Implement project workspace management, including work notes and task system
  - Added project workspace management system
  - Work note functionality for tracking project progress
  - Task management system integration
  - 29 files changed, 4256 lines added, 36 lines deleted

#### Czech Localization
- `b4bbf39` - Add complete Czech (cs-CZ) localization and update all language documentation
  - 116 files changed, 4933 lines added, 222 lines deleted
- `faf078f` - Fix Czech localization compilation error
  - 3 files changed, 910 lines added, 1 line deleted

#### Knowledge System Enhancement
- `20adaac` - Add KnowledgeTool and support complete localization
  - 34 files changed, 2331 lines added, 56 lines deleted

### 2026-04-24

#### Memory Management System Enhancement
- `c7b2ecc` - Enhance memory management features, add advanced filtering, statistics, and detail view functionality
  - Added memory advanced filtering functionality
  - Implemented memory statistics functionality
  - Added memory detail view page
  - Multi-language localization support (6 languages)
  - 13 files changed, 840 lines added, 86 lines deleted

#### Permission System Extension
- `4489ad6` - Add wttr.in weather service to network whitelist
  - Complete multi-language documentation synchronized updates (6 languages)
  - 14 files changed, 417 lines added, 1 line deleted

#### Web Interface Fix
- `d9d72e9` - Fix work note detail modal CSS priority issue
  - 19 files changed, 1744 lines added, 6 lines deleted

#### Chat History Optimization
- `0df599c` - Fix tool results being rendered as independent chat messages
  - 1 file changed, 222 lines added, 21 lines deleted
- `057b09d` - Optimize chat history detail display, improve tool call rendering
  - 3 files changed, 389 lines added, 68 lines deleted

#### Timer Execution History
- `fa3f06f` - Add timer execution history functionality, including detail view
  - 8 files changed, 937 lines added, 10 lines deleted
- `d824835` - Add timer execution history localization keys (all languages)
  - 7 files changed, 88 lines added

#### Localization Enhancement
- `c13cb17` - Register Spanish language variant
  - 1 file changed, 4 lines added
- `9c44f34` - Add Chinese historical calendar multi-language localization support
  - 16 files changed, 6049 lines added, 1 line deleted

#### Core Functionality Improvement
- `1e7c7b2` - Improve memory compression and tool execution tracking
  - 4 files changed, 338 lines added, 86 lines deleted

### 2026-04-23

#### Tool Localization
- `192fc6e` - Add missing tool name localization for 5 tools
  - 6 files changed, 30 lines added

#### Documentation Update
- `882c08f` - Update all changelog files, add complete Git history records and remove fake version numbers
  - 45 files changed, 8815 lines added, 1611 lines deleted

#### Chat Page Enhancement
- `65c157b` - Add loading indicator to chat page and auto-select curator session
  - 10 files changed, 211 lines added, 7 lines deleted

#### Chat History Feature
- `e483348` - Implement silicon being chat history viewing functionality
  - Added ChatHistoryController
  - Created ChatHistoryViewModel
  - Implemented ChatHistoryListView and ChatHistoryDetailView pages
  - Added chat history localization keys (5 languages)
  - 12 files changed, 1178 lines added

#### AI Flow Control Enhancement
- `30a2d4e` - Enhance AI flow cancellation, IM integration, and core host initialization
  - 11 files changed, 387 lines added, 12 lines deleted

#### Chat Message Queue
- `db48c51` - Add chat message queue, file metadata, and stream cancellation support
  - 4 files changed, 357 lines added

#### File Upload Support
- `28fb344` - Implement file source dialog and file upload support
  - 3 files changed, 1100 lines added, 2 lines deleted
- `1d3e2cc` - Add file source dialog localization strings (6 languages)
  - 6 files changed, 30 lines added

#### Documentation Update
- `8111e92` - Add Wiki link to README repository section
  - 1 file changed, 3 lines added, 1 line deleted

### 2026-04-22

#### Documentation Localization
- `66c11eb` - Translate Chinese comments to English and update all changelogs
  - 11 files changed, 373 lines added, 163 lines deleted

#### SSE Message Enhancement
- `b574b2b` - Add senderName to history messages for AI identification
  - 1 file changed, 9 lines added

#### Chat Functionality
- `601fc14` - Add mark_read action for session end marking
  - 7 files changed, 196 lines added, 36 lines deleted

#### Tool System Optimization
- `7a03a19` - Improve LogTool conversation query flexibility
  - 1 file changed, 57 lines added, 24 lines deleted

#### Localization Enhancement
- `0a8d750` - Add proactive silicon being behavior universal system prompt
  - 8 files changed, 460 lines added, 48 lines deleted

#### Logging System Refactoring
- `2b771f3` - Decouple LogController from file I/O, add log reading API
  - 4 files changed, 172 lines added, 137 lines deleted
- `12da302` - Add silicon being filter to log view
  - 9 files changed, 147 lines added, 10 lines deleted
- `8f6cb1e` - Add beingId parameter to ILogger interface, implement system/silicon being log separation
  - 47 files changed, 524 lines added, 490 lines deleted

#### Permission System Improvement
- `4c747ad` - Refactor PermissionTool, ExecuteCodeTool, add EvaluatePermission API
  - 18 files changed, 680 lines added, 492 lines deleted

#### Bug Fixes
- `1c96e99` - Fix search_files and search_content root directory search failure
  - 1 file changed, 98 lines added, 41 lines deleted

#### Tool Integration
- `135710d` - Remove SearchTool, move local search to DiskTool
  - 2 files changed, 185 lines added, 365 lines deleted

#### Tool System Extension
- `70ce7fb` - Implement DatabaseTool for structured database queries
  - 1 file changed, 382 lines added
- `be29a09` - Implement LogTool for operation and conversation history queries
  - 1 file changed, 298 lines added
- `4ea7702` - Implement PermissionTool for dynamic permission management
  - 1 file changed, 457 lines added
- `1384ff4` - Implement ExecuteCodeTool for multi-language code execution
  - 1 file changed, 477 lines added
- `82d1e11` - Implement SearchTool for information retrieval
  - 1 file changed, 363 lines added

#### Web Interface Optimization
- `0675c45` - Optimize markdown code block highlighting in preview pane
  - 1 file changed, 4 lines added, 23 lines deleted
- `702b3f3` - Enhance task view, add status badges and metadata display
  - 8 files changed, 221 lines added, 9 lines deleted
- `6ed9a79` - Improve chat message storage and view rendering
  - 8 files changed, 140 lines added, 29 lines deleted

### 2026-04-21

#### Bug Fixes
- `c6b518b` - Fix timer message passing and chat message storage
  - 3 files changed, 297 lines added, 124 lines deleted

#### Configuration Management
- `4305769` - Add .gitattributes for line ending management
  - 1 file changed, 32 lines added

#### Web Interface Improvement
- `188c6f8` - Register task list API route and add empty state display
  - 2 files changed, 35 lines added, 2 lines deleted
- `634e8ca` - Add return to list link on permission page
  - 1 file changed, 16 lines added
- `6ba591d` - Add standalone AI config editor for silicon beings
  - 11 files changed, 842 lines added, 18 lines deleted
- `0a826f5` - Add save success notification in code editor
  - 1 file changed, 9 lines added, 2 lines deleted
- `2940373` - Enhance Web interface, add code hover tooltips and UI improvements
  - 11 files changed, 1054 lines added, 75 lines deleted

#### Permission System Fix
- `592c7ab` - Fix callback instantiation and registration order
  - 2 files changed, 38 lines added, 7 lines deleted

#### Security Enhancement
- `833ead2` - Add assembly reference validation for dynamic compilation
  - 4 files changed, 135 lines added, 8 lines deleted

#### Permission System Enhancement
- `5879621` - Add permission callback pre-compilation validation and enhanced error handling
  - 21 files changed, 617 lines added, 26 lines deleted

#### Documentation Update
- `4dbf659` - Update changelog to v0.5.1, replace GitHub placeholder URLs, add Gitee mirror, localize Bilibili name by language, update email
  - 32 files changed, 489 lines added, 180 lines deleted

#### Configuration and Entry
- `0fc1693` - Update program entry and project configuration
  - 2 files changed, 7 lines added

#### Permission System Refactoring
- `ea9179a` - Improve permission system implementation
  - 5 files changed, 358 lines added, 152 lines deleted

#### Bug Fixes
- `928a96d` - Fix calendar calculation implementation
  - 4 files changed, 12 lines added, 12 lines deleted

#### AI and Calendar
- `646813e` - Improve AI client factory implementation
  - 2 files changed, 21 lines added, 20 lines deleted

#### Localization
- `7940d9c` - Add Korean localization support
  - 7 files changed, 2424 lines added, 10 lines deleted
- `4ff98ad` - Refactor documentation to support multiple languages
  - 81 files changed, 23818 lines added, 1886 lines deleted

### 2026-04-20

#### Core Functionality Completion
- `28905b5` - Complete multi-language support, AI client factory, permission system, and localization settings
  - Logging system with managers, entries, and different log levels
  - Token Usage Audit system for querying and tracking token usage
  - AI client factory for auto-discovering different AI platforms
  - Permission callback system with its own storage
  - Console logger implementation
  - Multi-language support for English and Simplified Chinese
  - WebUI messenger with WebSocket for real-time chat
  - Enhanced default silicon being with localization
  - 39 files changed, 4670 lines added, 175 lines deleted

### 2026-04-19

#### Timer and Calendar
- `c933fd8` - Update localization, timer system, web views, and add tools
  - Better localization manager
  - Scheduled task system for timed tasks
  - AI configuration and context management
  - Calendar tool supporting 32 calendar types
  - Web controller for calendar API
  - Task management tool
  - 46 files changed, 4018 lines added, 975 lines deleted

**Architecture Improvements**
- Redesigned web view architecture for better skin support
- Improved being management system with better state handling

### 2026-04-18

- `9f585e1` - Update localization, timer system, web views, and add tools
  - Timer and scheduling improvements
  - Better web views with improved UI components
  - More tool implementations
  - 57 files changed, 3328 lines added, 389 lines deleted

### 2026-04-17

- `9b71fcd` - Update core modules, add zh-HK docs, broadcast channel, config tool, and audit web view
  - Broadcast channel for multiple silicon beings to chat together
  - Config tool system
  - Audit web view
  - Traditional Chinese documentation
  - 42 files changed, 3533 lines added, 268 lines deleted

### 2026-04-16

- `5040f05` - Update core and default modules
  - Module optimization and bug fixes
  - Implementation updates and improvements
  - 58 files changed, 9916 lines added, 111 lines deleted

### 2026-04-15

- `3efab5f` - Update multiple modules: AI, Chat, IM, Tools, Web, Localization, Storage
  - AI client improvements
  - Chat system enhancements
  - Messenger provider updates
  - Tool system optimization
  - Web infrastructure improvements
  - Localization optimization
  - Storage system updates
  - 33 files changed, 788 lines added, 232 lines deleted

### 2026-04-14

- `4241a2f` - Chat functionality basically complete, UI upload optimization
  - Chat system functionality complete
  - File upload UI optimization
  - 16 files changed, 1234 lines added, 102 lines deleted

### 2026-04-13

- `c498c31` - Code updates
  - General code improvements and optimizations
  - 32 files changed, 1045 lines added, 546 lines deleted

### 2026-04-12

#### Documentation and Localization
- `2161002` - Refactor documentation and enhance localization
  - 17 files changed, 982 lines added, 92 lines deleted
- `03d94e4` - Enhance configuration system and localization
  - 25 files changed, 1378 lines added, 154 lines deleted
- `9976a35` - Add about page and localization
  - 14 files changed, 699 lines added, 44 lines deleted

#### Chat and Web Views
- `0c8ccfc` - Enhance chat system, localization, and web views
  - 13 files changed, 402 lines added, 56 lines deleted
- `a8f1342` - Redesign web communication layer, switch from WebSocket to SSE
  - 27 files changed, 793 lines added, 935 lines deleted

### 2026-04-11

#### Logging System
- `e8fe259` - Add logging system and code optimization
  - 37 files changed, 624 lines added, 91 lines deleted
- `f01c519` - Add logging system, update AI interface and web views
  - 31 files changed, 1758 lines added, 63 lines deleted

### 2026-04-10

- `4962924` - Enhance WebSocket handler, chat view, and messenger interaction
  - Context manager improvements
  - Chat system enhancements
  - Messenger provider interface updates
  - WebUI provider redesign
  - JavaScript builder and router updates
  - Chat view optimization
  - WebSocket handler improvements
  - 9 files changed, 365 lines added, 134 lines deleted

### 2026-04-09

- `f9302bf` - Enhance messenger provider interface, chat system, and web UI interaction
  - Messenger provider interface extension
  - Chat message and system improvements
  - Context manager optimization
  - Default silicon being enhancement
  - Web UI chat view improvements
  - WebSocket handler updates
  - 10 files changed, 427 lines added, 93 lines deleted

### 2026-04-07

- `6831ee8` - Redesign web views and JavaScript builder
  - Complete web controller redesign
  - JavaScript builder complete rewrite
  - All view components updated
  - Skin system improvements
  - View base class architecture upgrade
  - 23 files changed, 2004 lines added, 1983 lines deleted

### 2026-04-05

- `41e97fb` - Update multiple core modules and web controllers
  - Context manager improvements
  - Chat system and session management
  - Service locator redesign
  - Silicon being base class and manager updates
  - Comprehensive web controller updates (17 controllers)
  - Default silicon being factory improvements
  - 31 files changed, 681 lines added, 326 lines deleted
- `67988d4` - Improve web UI module, add executor view, clean up views and core modules
  - 61 files changed, 3148 lines added, 3726 lines deleted

### 2026-04-04

- `b58bb1c` - Add initialization controller and redesign web module
  - Initialization controller
  - Config module redesign
  - Localization module update
  - Skin system improvements
  - Router enhancements
  - 29 files changed, 1269 lines added, 289 lines deleted
- `f03ac0b` - Add web UI module, improve messenger functionality
  - 60 files changed, 8481 lines added, 165 lines deleted

### 2026-04-03

- `192e57b` - Update project structure and core runtime components
  - 22 files changed, 446 lines added, 179 lines deleted
- `59faec8` - Core and default implementation updates
  - 25 files changed, 3056 lines added, 18 lines deleted
- `d488485` - Add dynamic compilation functionality and curator tool module
  - 19 files changed, 1727 lines added, 11 lines deleted
- `753d1d9` - Add security module, update executors, messenger providers, localization, and tools
  - 29 files changed, 2352 lines added, 93 lines deleted
- `a378697` - Complete phase 5 - tool system + executors
  - 41 files changed, 2651 lines added, 363 lines deleted

### 2026-04-02

- `e6ad94b` - Fix chat history loading failure when config file is deleted during testing
  - 4 files changed, 49 lines added, 45 lines deleted
- `daa56f5` - Complete phase 4: persistent memory (chat system + messenger channels)
  - 29 files changed, 2051 lines added, 538 lines deleted

### 2026-04-01

- `bbe2dbb` - Fix config loading and chat service message routing
  - 27 files changed, 1633 lines added, 147 lines deleted
- `2fa6305` - Implement phase 2: main loop framework and tick object system
  - 9 files changed, 594 lines added, 41 lines deleted
- `32b99a1` - Implement phase 1 - basic chat functionality
  - 19 files changed, 1185 lines added
- `358e368` - Initial commit: project documentation and license
  - 10 files changed, 1873 lines added
