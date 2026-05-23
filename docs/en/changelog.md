# Changelog

**English** | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md)

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## About This Changelog

### Dual Project Versions

This project provides two implementation versions:

- **SiliconLife.Default**: Default implementation, primarily used for verifying architecture feasibility. Console application with file system JSON storage.
- **SiliconLife.Fast**: Production-ready version. Cross-platform desktop application (Windows / macOS / Linux), SpeedyPack in-memory storage + asynchronous persistence (.spk file format), deeply performance-optimized.

Both versions share the same interfaces and functionality, differing only in storage implementation and runtime mode. SiliconLife.Default serves as an architecture validation baseline, while SiliconLife.Fast is the production-ready flagship version.

### Project Origin

- This project originated on March 20, 2026.
- Before this project, there was a verification demo that failed due to poor architecture design, preventing integration with multiple AI platforms.

### AI IDE Tools Used

#### Kiro (Amazon AWS)
- The project was initially maintained by Kiro and started using Spec mode.
- Kiro is an agentic AI development environment built by Amazon AWS.
- Based on Code OSS (VS Code), supports VS Code settings and Open VSX compatible extensions.
- Features a spec-driven development workflow for structured AI coding.

#### Comate AI IDE / Wenxin Kuaima (Baidu)
- Occasionally used for copywriting and documentation work.
- Comate AI IDE is an AI-native development environment tool released by Baidu Wenxin on June 23, 2025.
- The industry's first multi-modal, multi-agent collaborative AI IDE.
- Features include design-to-code conversion and full-process AI-assisted coding.
- Powered by the Baidu Wenxin 4.0 X1 Turbo model.

#### Trae (ByteDance)
- Used from October 2025 to April 2026.
- AI IDE with intelligent code generation and project management.

#### Qoder (Alibaba)
- Used for project maintenance since April 18, 2026.
- AI coding platform supporting code analysis, documentation generation, and multi-agent collaboration.

#### CatPaw (Meituan)
- Used in combination with Qoder since May 6, 2026.
- Based on Meituan's self-developed LongCat series models, with powerful full code architecture refactoring capabilities.

### Requirements Document

- The requirements document for this project is not publicly available.
- Requirements have been validated through iterations with over 12 international AI platforms and large model series, producing a user story-driven requirements document of over 2000 lines that is nearly incomprehensible to humans.

---

## [Unreleased]

### 2026-05-22

#### Documentation Consistency Fixes
- `9e07b27` - Fix French (fr-FR) documentation inconsistencies with source code (ref task-307)
  - 10 files changed

- `9e3be72` - Fix German (de-DE) documentation inconsistencies with source code (ref task-308)
  - 5 files changed

- `2bc7151` - Fix Spanish (es-ES) documentation inconsistencies with source code (ref task-309)
  - 13 files changed

- `f95088e` - Fix Italian (it-IT) documentation inconsistencies with source code (ref task-310)
  - 11 files changed

- `6ea9f4a` - Fix Polish (pl-PL) documentation inconsistencies with source code (ref task-311)
  - 16 files changed

- `7646923` - Fix Portuguese (pt-PT) documentation inconsistencies with source code (ref task-312)
  - 12 files changed

- `7eaf9db` - Fix Czech (cs-CZ) documentation inconsistencies with source code (ref task-313)
  - 12 files changed

#### Collaboration Framework
- `3cb7347` - Update task-313 relatedCommit=7eaf9db
  - 1 files changed

### 2026-05-21

#### New Features
- `99eca78` - Add "View Storage (Read-only)" to context menu, in-process Speedy.Manager call (ref task-301)
  - 26 files changed

#### Documentation Consistency Fixes
- `7f65cf1` - Fix zh-CN documentation inconsistencies with source code (ref task-303)
  - 15 files changed

- `a9e2a2c` - Fix English (en) documentation inconsistencies with source code (ref task-302)
  - 9 files changed

- `2549105` - Fix Traditional Chinese (zh-HK) documentation inconsistencies with source code (ref task-304)
  - 12 files changed

- `277eb50` - Fix Japanese (ja-JP) documentation inconsistencies with source code (ref task-305)
  - 10 files changed

- `edce413` - Fix Korean (ko-KR) documentation inconsistencies with source code (ref task-306)
  - 18 files changed

- `f2adcae` - Fix Portuguese documentation inconsistencies with source code (ref task-220)
  - 15 files changed

- `3332987` - Fix Traditional Chinese (Hong Kong) documentation inconsistencies with source code (ref task-218)
  - 14 files changed

- `af9f715` - Fix Polish documentation inconsistencies with source code (ref task-217)
  - 15 files changed

- `2e2b18b` - Fix Korean documentation inconsistencies with source code (ref task-216)
  - 16 files changed

- `626ebc9` - Fix Japanese documentation inconsistencies with source code (ref task-215)
  - 19 files changed

- `48d061b` - Fix Italian documentation inconsistencies with source code (ref task-214)
  - 14 files changed

#### Collaboration Framework
- `6683bee` - Register Marvis AI team, update task status
  - 3 files changed

- `03fc905` - Archive task-210~220
  - 5 files changed

### 2026-05-20

#### New Features
- `65176d4` - Add complete Portuguese (pt-PT + pt-BR) localization support (ref task-208)
  - 41 files changed

#### Documentation Consistency Fixes
- `af4dffd` - Fix all zh-CN documentation inconsistencies with source code (ref task-209)
  - 11 files changed

- `144b945` - Fix English (en) and Czech (cs-CZ) documentation inconsistencies with source code (ref task-219, task-210)
  - 22 files changed

- `08bec55` - Fix German (de-DE) documentation inconsistencies with source code (ref task-211)
  - 14 files changed

- `7ff28de` - Fix Spanish (es-ES) documentation inconsistencies with source code (ref task-212)
  - 14 files changed

- `15e2133` - Fix French (fr-FR) documentation inconsistencies with source code (ref task-213)
  - 13 files changed

#### Bug Fixes
- `7dac388` - Fix project task list not displaying (ref task-207)
  - 6 files changed

#### Collaboration Framework
- `7890223` - Archive task-201~209, publish task-210~220 documentation consistency fix tasks
  - 5 files changed

### 2026-05-19

#### New Features
- `cd72846` - Implement safe alternative for PluginLoader security scan bypass (ref task-203)
  - 13 files changed

- `fc0c00c` - Speedy.Manager enhancements - Create/Import/Export/TreeView hierarchy/Progress window (ref task-206)
  - 9 files changed

#### Bug Fixes
- `ec07118` - Fix ITypeRegistry/IObjectFactory not registered before plugin loading (ref task-205)
  - 8 files changed

- `9e749db` - Fix Creator ID is required error when creating project (ref task-204)
  - 4 files changed

#### Infrastructure
- `43dc092` - CLDR migration - add CldrDataProvider, remove .github
  - 1 files changed

- `c09ec1f` - Add cldr/ to .gitignore
  - 1 files changed

- `221f818` - Switch GitHub sync to Gitee push mirror scheme, keep workflow as manual backup only
  - 1 files changed

- `08cdf1a` - Fix GitHub sync workflow - add retry logic and no-change skip
  - 1 files changed

- `fb4e77d` - Update SiliconLife.Speedy.Manager.csproj
  - 1 files changed

#### Collaboration Framework
- `df90af0` - Update task-203 relatedCommit=cd72846
  - 1 files changed

### 2026-05-18

#### Refactoring
- `e720d06` - Refactor Speedy.Manager from WinForms to Avalonia completely (ref task-202)
  - 17 files changed

#### Bug Fixes
- `08894a9` - Fix memory timeline summary entry level display error (ref task-201)
  - 3 files changed

#### Collaboration Framework
- `2871afb` - Archive all tasks, clear tasks.json
  - 2 files changed

### 2026-05-17

#### New Features
- `d6eb994` - Add project creation entry and workflow template selection to project list page (ref task-203)
  - 14 files changed

- `0872134` - ThinkOnProject curator-driven orchestration for template-less projects (ref task-202)
  - 6 files changed

- `cb3188e` - Group chat @mention visualization (ref task-208)
  - 4 files changed

- `f9968e5` - AI client ToolCall capability declaration and graceful degradation (ref task-205)
  - 4 files changed

- `0d2b843` - Group chat decision logic ShouldReplyInGroupChat (ref task-201)
  - 6 files changed

- `277a2b1` - Knowledge network completion - advanced queries and graph traversal (ref task-207)
  - 9 files changed

#### Bug Fixes
- `6d0b66e` - Fix appendMessage TypeError when sending group chat messages (ref task-209)
  - 5 files changed

- `b15167c` - Submit missing list-workflow-templates route registration from task-203 (ref task-203)
  - 1 files changed

- `dc549a2` - Fix Gitee sync workflow - add username to token URL
  - 1 files changed

#### Infrastructure
- `e5fa3ad` - Disable GitHub auto-sync schedule, awaiting official Gitee sync solution
  - 1 files changed

#### Collaboration Framework
- `4a58c82` - Add system capability analysis report + ThinkOnProject design proposal
  - 5 files changed

- `8ab29e6` - Archive system capability completeness analysis report to .ai-collab/docs
  - 2 files changed

- `b412d9c` - Archive old tasks, re-publish task-201~208 based on comprehensive analysis
  - 2 files changed

- `437884a` - Update collaboration metadata - task-202/203/204 completed (ref task-202, task-203, task-204)
  - 2 files changed

- `bf78d79` - Update collaboration metadata - task-201/205/208 completed
  - 2 files changed

- `de6ee0e` - Session end record catpaw-20260517-2215
  - 5 files changed

- `7223b6f` - Session end record catpaw-20260517-2200
  - 4 files changed


## [Alpha-0.2] - 2026-05-16

### 2026-05-16

#### Release Preparation
- `476d839` - Add alpha-0.2 release tasks
  - Created task-114 (CHANGELOG writing) and task-115 (version number update)
  - 1 file changed

### 2026-05-15

#### Infrastructure
- `672627b` - Add Gitee sync workflow with permission configuration
  - Updated sync-from-gitee.yml workflow permissions
  - 1 file changed, 7 insertions(+), 4 deletions(-)

- `3cd5256` - Add GitHub Actions auto-sync from Gitee
  - Added sync-from-gitee.yml workflow
  - 1 file changed, 50 insertions(+)

#### Documentation Updates
- `aa1d2ad` - Update all 11 languages README/architecture/getting-started docs to reflect SiliconLife.Fast multi-platform support (ref task-112, task-113)
  - Corrected documentation describing SiliconLife.Fast as Windows-only to reflect actual multi-platform support (Windows / macOS / Linux)
  - Updated README.md, architecture.md, getting-started.md for all 11 languages
  - SelectComponent added hint property support
  - ConfigView enum dropdowns now pass hint parameter
  - 11 languages localization added SelectSearchHint key
  - 53 files changed, 690 insertions(+), 194 deletions(-)

#### Task System
- `3329f3d` - Add task system inspection mechanism + localization bug fix tasks
  - Created task-113: Fix about page localization issue
  - Updated task-112: Update Fast version docs for Linux support
  - Archived completed tasks (11) to .ai-collab/archive/
  - Inspection mechanism configured: quick inspection (every 30 min) + full inspection (daily at 06:00)
  - 2 files changed, 148 insertions(+), 171 deletions(-)

#### Collaboration Framework
- `6038e22` - Register coze-agent to .ai-collab registry
  - Added Coze platform resident AI registration
  - 1 file changed

### 2026-05-14

#### AI Collaboration Framework
- `7344fbb` - Remove handoff mode, switch to task-list-driven approach (v2.0)
  - Restructured .ai-collab directory from handoff mode to task-list-driven
  - Added tasks.json core task list file
  - Added activity.log operation log
  - Added changes/ and sessions/ directories

- `589a48e` - Add .ai-collab session records
  - Added AI collaboration session state records

- `5481bcf` - Register Qoder AI IDE to collaboration registry
  - Added Qoder AI coding assistant registration

- `e2d7b61` - Supplement tasks.json relatedCommit and changes commitHash
  - Completed task metadata associations

- `a087f0c` - Accept all task-101~110 tasks
  - Confirmed all 10 task fixes completed

#### Bug Fixes
- `fac9435` - Complete all task-101~110 bug fixes and implementations
  - Fixed search select component missing hint text
  - Fixed about page localization issues
  - Fixed help system search JS errors
  - 39 files changed, 684 insertions(+), 121 deletions(-)

- `c46dfbc` - Complete all pending tasks (task-001~006)
  - Completed initial 6 pending tasks

- `ec176b2` - Override task list - code review found 10 new bugs
  - Created task-101~110 (10 new tasks)

#### Refactoring
- `ab15915` - Unify copyright headers + fix HelpController BOM and HelpView search JS
  - Unified Apache 2.0 copyright headers across all C# source files
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
  - Added Polish help documentation localization (HelpLocalizationPlPL.cs, 3972 lines)
  - Added Polish Chinese historical calendar support (ChineseHistoricalPlPL.cs, 600 lines)
  - Added Polish tray localization (TrayPlPL.cs, 135 lines)
  - Added Polish complete documentation set (15 documents)
  - Language enum added Polish
  - 35 files changed, 14379 insertions(+), 11 deletions(-)

- `51f9c8e` - Update documentation with Ark AI references and terminology improvements
  - Updated AI client terminology in multilingual documentation

- `7587c12` - Add changelog entries for all languages
  - Synchronized changelog updates across all language versions

#### Window System Migration
- `b49a07d` - Migrate to Avalonia window resident mode
  - Removed Windows Forms dependency, fully migrated to Avalonia UI framework
  - Status window displays correctly on Linux (verified via remote desktop)
  - Added window controls: right-click menu, double-click to open Web, close button
  - Added multi-AI collaboration framework (.ai-collab/)
  - Fixed tray icon initialization (graceful degradation)
  - Added App.axaml and App.cs Avalonia application entry
  - 13 files changed, 1442 insertions(+), 541 deletions(-)

- `d335aaf` - Linux platform window always visible + close confirmation dialog
  - Linux automatically shows status window (no tray icon)
  - Linux close window shows confirmation dialog
  - Windows/macOS maintain original tray behavior
  - Support --no-tray parameter to force disable tray
  - Added ShowMessageBoxAsync method for confirmation dialogs
  - 3 files changed, 206 insertions(+), 29 deletions(-)

#### Tray System Refactoring
- `841d384` - Refactor tray system and initialize AI collaboration framework
  - Streamlined TrayLocalizationBase removing unused properties
  - Added ShowStatus localization item
  - App.cs added tray icon click to show status window, localized menu items
  - Program.cs moved tray icon initialization to StartAsync
  - TrayStatusWindow hides on close instead of exiting
  - Registered trae-glm5 and catpaw to .ai-collab framework
  - Updated .gitignore to ensure all .ai-collab files are tracked
  - 22 files changed, 178 insertions(+), 1226 deletions(-)

#### Documentation
- `43653bc` - Update repository description and AI registry
  - Updated project README and .ai-collab registration info

### 2026-05-12

#### Task System Web Views
- `0891b3c` - Add task execution detail and history views
  - Added TaskExecutionDetailView task execution detail view
  - Added TaskExecutionHistoryView task execution history view
  - TaskController added execution detail and history query interfaces
  - Added TaskViewModel task view model
  - TaskCenter task center enhanced
  - TaskSystem task system updated
  - 9 languages localization added task-related keys
  - 26 files changed, 803 insertions(+), 55 deletions(-)

### 2026-05-11

#### Web Component Architecture Refactoring
- `5e687ad` - Migrate component rendering from string to H-tree
  - ComponentBase rendering method migrated from string pattern to H-tree structure
  - All 28 components adapted to new rendering architecture (A, Accordion, Button, Calendar, Card, Chart, etc.)
  - SelectComponent major refactoring (889 lines improved)
  - Controllers and views updated accordingly
  - 33 files changed, 667 insertions(+), 435 deletions(-)

- `bfd332d` - Migrate Style from string to CssBuilder inline styles
  - Added CssBuilder style builder
  - ComponentBase style system migrated from string to structured CssBuilder
  - LoadingComponent significantly enhanced (103 lines added)
  - ConfigController, LogController, MemoryController controller style migration
  - ChatView, ConfigView, LogView, MemoryView view style migration
  - 37 files changed, 351 insertions(+), 157 deletions(-)

#### Storage System Optimization
- `d67a7ee` - Optimize QueryLatest for large datasets
  - SpeedyTimeStorage QueryLatest method performance optimization
  - SpeedyLoggerProvider logger provider enhanced
  - 2 files changed, 44 insertions(+), 5 deletions(-)

#### Calendar System Refactoring
- `9629f88` - Extract TimerExecution and enhance timer web views
  - TimerSystem extracted TimerExecution logic (175 lines removed)
  - SelectComponent significantly enhanced (427 lines improved)
  - TimerController and timer views enhanced
  - ContextManager context manager updated
  - 12 files changed, 458 insertions(+), 267 deletions(-)

#### Localization
- `5d8ca79` - Add LogsLoading localization key
  - 9 languages added LogsLoading key
  - DefaultLocalizationBase base class added definition
  - 11 files changed, 15 insertions(+)

### 2026-05-10

#### Task System Refactoring
- `54394f6` - Merge task system with chat history cycles
  - ProjectTaskSystem project task system significantly streamlined (411 lines refactored)
  - TaskSystem task system streamlined (254 lines refactored)
  - TaskCenter task center refactored (188 lines improved)
  - ContextManager context manager optimized (347 lines refactored)
  - DefaultSiliconBeing silicon being enhanced
  - TimerSystem timer system integrated with tasks
  - IWorkNoteStorage interface updated
  - SpeedyWorkNoteStorage and FileSystemWorkNoteStorage adapted
  - 16 files changed, 648 insertions(+), 897 deletions(-)

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
  - ChatSystem chat system added features
  - 14 files changed, 1030 insertions(+), 112 deletions(-)

- `c9babce` - Improve tool call rendering in chat view
  - ChatView tool call block rendering enhanced
  - 1 file changed, 54 insertions(+), 11 deletions(-)

#### AI Tool Scenario System
- `ff2eddd` - Implement tool scenario filtering system
  - Added ToolScenarioAttribute tool scenario attribute (36 lines)
  - Added ChatOnlyAttribute chat-only scenario attribute (19 lines)
  - ToolManager tool manager added scenario filtering (40 lines)
  - ContextManager context manager adapted for scenario filtering
  - 4 files changed, 115 insertions(+), 30 deletions(-)

- `5709a33` - Add scenario attributes to tool classes
  - 24 tool classes added ToolScenario attribute annotations
  - Including calendar, chat, config, curator, database, disk, dynamic compile, etc.
  - 24 files changed, 46 insertions(+), 20 deletions(-)

#### Task System Refactoring
- `2f19a5f` - Restructure task system with TaskCenter and TaskEnumerator
  - Added TaskCenter task center (235 lines)
  - Added TaskEnumerator task enumerator (297 lines)
  - TaskSystem task system refactored and streamlined
  - DefaultSiliconBeing silicon being adapted to new architecture
  - DefaultSiliconBeingFactory factory updated
  - SiliconBeingBase base class enhanced
  - 7 files changed, 796 insertions(+), 275 deletions(-)

#### Permission System Migration
- `a06ed09` - Migrate IM and permission system to App project
  - PermissionRequestQueue migrated from Default/Fast to App project (443 lines added)
  - Removed Default version WebUIProvider (403 lines deleted)
  - Removed Default version HelpTool (194 lines deleted)
  - Removed Default/Fast duplicate PermissionRequestQueue
  - Removed Default version IMPermissionAskHandler
  - PermissionRequestController controller updated
  - 14 files changed, 496 insertions(+), 1183 deletions(-)

#### AI Context Optimization
- `4c8aaff` - Optimize context manager and enhance service locator
  - ContextManager context manager streamlined and optimized
  - ServiceLocator service locator enhanced (36 lines added)
  - ToolManager tool manager enhanced (34 lines added)
  - DashScopeClient and VolcengineArkClient clients improved
  - Executors (CommandLine, Disk, Network) updated
  - 8 files changed, 116 insertions(+), 98 deletions(-)

#### Localization
- `5c5eef7` - Add audit and task localization keys
  - DefaultLocalizationBase added 127 lines of localization definitions
  - 9 languages added audit and task-related keys (26 lines each)
  - 11 files changed, 387 insertions(+)

#### Project Configuration
- `2067db6` - Update project configs and gitignore rules
  - .gitignore rules updated
  - DefaultConfigData and Fast DefaultConfigData config enhanced
  - SpeedyWorkNoteStorage storage improved
  - SpeedyPack core enhanced
  - 5 files changed, 32 insertions(+), 6 deletions(-)

### 2026-05-07

#### Italian Localization
- `8adc18c` - Add Italian localization support and update multilingual documentation
  - Added it-IT Italian localization
  - Added ItIT localization implementation (1909 lines)
  - Added ChineseHistoricalItIT Chinese historical calendar Italian support (586 lines)
  - Added TrayItIT tray Italian localization (135 lines)
  - Added Italian complete documentation set (14 documents: README, API reference, architecture, calendar system, changelog, contributing guide, etc.)
  - Updated architecture, development guide, getting-started guide, etc. for all language versions
  - Language enum added Italian
  - 86 files changed, 11573 insertions(+), 769 deletions(-)

#### Documentation Sync
- `12a5deb` - Update multilingual documentation for architecture, changelog, and silicon being guide
  - 8 languages README updated
  - 8 languages architecture documentation updated
  - 8 languages changelog updated
  - 8 languages silicon being guide updated
  - 8 languages tools reference updated
  - Glossary restructured
  - 46 files changed, 1697 insertions(+), 442 deletions(-)

### 2026-05-06

#### Large-Scale Module Refactoring
- `eeb3be6` - Large-scale module refactoring and reorganization
  - SiliconLife.App project restructuring
  - SiliconLife.Fast project reorganization
  - SiliconLife.Default project reorganization
  - SiliconLife.Common shared modules reorganization
  - SiliconLife.Core core modules reorganization
  - SiliconLife.Speedy storage engine reorganization
  - SiliconLife.Speedy.Manager management tools reorganization
  - 119 files changed, 6926 lines added, 3066 lines deleted

### 2026-05-04

#### AI Client
- `24d2c86` - Added VolcengineArkClient and replaced Audit with Usage tracking
  - New VolcengineArkClient Volcengine Ark AI client
  - Supports streaming and non-streaming modes
  - Built-in dual rate limiting (client-side + server-side)
  - Compatible with OpenAI API protocol
  - Replaced Audit system with Usage tracking
  - 24 files changed, 802 lines added, 21 lines deleted

#### Tool System
- `f27650a` - Added hot reload tool for automatic Fast restart
  - New HotReloadTool hot reload tool
  - Supports online compilation, update, and restart of SiliconLife.Fast
  - New standalone HotReload.exe updater program
  - Safe file copying mechanism (does not overwrite itself)
  - Graceful shutdown and port release waiting
  - 9 files changed, 581 lines added

#### Localization
- `6a5aad8` - Updated all files and added French localization support
  - New fr-FR French localization
  - Updated all language versions
  - French help documentation translation
  - French interface translation
  - 100+ files changed

### 2026-05-03

#### Project Infrastructure
- `2664b0c` - Updated project infrastructure and dependencies
  - SiliconLife.Speedy.Manager added WPF management interface (MainForm.Designer.cs, MainForm.resx)
  - Added slc.ico icon resource (1.5MB)
  - PluginLoader significantly enhanced security scanning (622 lines added)
  - Added PermissionedStreamFactory permission stream factory (779 lines)
  - Added PermissionRequestQueue permission request queue (Default and Fast versions)
  - Added DebugLoggerProvider debug logger provider
  - ConfigDataBase configuration base class enhanced
  - ToolManager added plugin tool scanning (ScanAllPluginAssemblies)
  - SiliconBeingManager lifecycle management enhanced
  - DashScopeClient Alibaba Cloud AI client significantly enhanced (227 lines added)
  - DefaultSiliconBeingFactory factory enhanced
  - Web views and controllers updated (ChatView, WorkNoteView, PermissionRequestController)
  - 9-language localization added new keys
  - 35 files changed, 28080 lines added, 336 lines deleted

### 2026-05-02

#### AI Client Enhancement
- `c16f99f` - Updated AI client, Web UI, and storage components
  - DashScopeClient Alibaba Cloud client significantly improved
  - SpeedyPackAutoCompactor auto-compactor optimized
  - Web view base class and BeingView improved
  - 6 files changed, 240 lines added, 81 lines deleted

#### Plugin System
- `242dc98` - Added plugin list on about page
  - AboutController added plugin information display
  - AboutViewModel added plugin data model
  - AboutView added plugin list rendering
  - 9-language localization added plugin-related keys
  - 14 files changed, 160 lines added, 1 line deleted

#### AI Optimization
- `147f8f4` - Simplified context memory prompt text
  - ContextManager optimized AI prompts
  - 1 file changed, 1 line added, 1 line deleted

#### Speedy Storage Optimization
- `8bda2d3` - Updated Speedy storage and memory controller implementation
  - SpeedyPackAutoCompactor interval correction
  - SpeedyTimeStorage path handling optimization
  - MemoryController memory controller improvements
  - SpeedyPack.Manager UI update
  - 4 files changed, 21 lines added, 18 lines deleted

#### Tray Enhancement
- `8972654` - Enhanced tray status window localization support
  - 9-language tray localization added Speedy management entry
  - TrayStatusWindow added Speedy management menu item
  - 11 files changed, 72 lines added

#### Speedy.Manager Optimization
- `6f5db09` - Optimized SpeedyPack Manager UI and internal components
  - MainForm interface refactoring
  - FreeList memory management optimization
  - WriteQueue write queue improvements
  - SpeedyPack core optimization
  - 5 files changed, 96 lines added, 88 lines deleted

#### Storage System Enhancement
- `57f9d5d` - Improved storage system, added auto-compaction and incomplete date support
  - Added SpeedyPackAutoCompactor auto-compaction timer (30-minute interval)
  - SpeedyPackRegistry singleton manager enhanced
  - SpeedyStorage, SpeedyTimeStorage, SpeedyWorkNoteStorage adapter improvements
  - SpeedyPack added FreeList free space management (149 lines)
  - PackFileWriter writer refactoring optimization
  - WriteOperation, WriteQueue write queue enhancement
  - SpeedyPackOptions configuration options expansion
  - IncompleteDate added comparison methods
  - PluginLoader plugin loader improvements
  - Default and Fast versions Program.cs initialization flow updated
  - DefaultConfigData configuration data simplified
  - KnowledgeNetwork knowledge network streamlined
  - ChatController, MemoryController controller optimization
  - SpeedyPack.Manager MainForm functionality enhanced
  - 22 files changed, 639 lines added, 253 lines deleted

#### Speedy.Manager Update
- `b04ed33` - Updated Speedy.Manager files

### 2026-05-01

#### Architecture Refactoring: Speedy Storage Replaces LiteDB
- `6600972` - Replaced LiteDB with Speedy storage, added plugin system and Speedy projects
  - **Added SiliconLife.Speedy project**: High-performance .spk storage engine
    - SpeedyPack core class (489 lines): In-memory directory mapping + entry cache + asynchronous write queue
    - SpeedyPackOptions configuration class: Cache TTL, max cache entries, read-only mode
    - IPackTransaction transaction interface: Supports atomic write operations
    - SpkFileInfo file information class
    - Internal directory: DirectoryMap, EntryCache, PackFileReader, PackFileWriter, WriteQueue, WriteOperation, SpeedyTransaction, SpkHeader, PathNormalizer, FreeList
    - Uses MessagePack 3.1.4 for binary serialization (LZ4 compression)
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
    - Added SpeedyPackAutoCompactor (auto-compaction timer)
    - Removed LiteDB-related storage implementations (LiteDBStorage, LiteDBTimeStorage, LiteDBWorkNoteStorage, LiteDBLoggerProvider, LiteDBManager, LiteDBModels)
    - Removed LiteDB management window related code
  - **Plugin System**:
    - Added IPlugin interface (Core/Plugins/IPlugin.cs)
    - Added PluginLoader plugin loader (Core/Plugins/PluginLoader.cs)
    - Support loading plugin DLLs from directory
    - Security scanning: Forbidden namespace checking (System.IO, System.Net, Microsoft.CodeAnalysis, etc.)
    - Trusted assembly whitelist (Google.Protobuf, Newtonsoft.Json, MessagePack, etc.)
    - Custom AssemblyLoadContext isolated loading
    - ToolManager added ScanAllPluginAssemblies method
    - CoreHost integrated plugin loader
  - 119 files changed, 6926 lines added, 3066 lines deleted

#### Silicon Being Enhancement
- `3aef4c3` - Added Stopped activity state and error handling improvements
  - Silicon beings now have Stopped state
  - Error handling and recovery mechanism enhanced

#### Localization Update
- `513c65d` - Updated all language versions and documentation
  - Added MarkdownEditorComponent component (625 lines)
  - Added DetailsComponent component (130 lines)
  - Added AccordionComponent accordion component (285 lines)
  - BeingController, ChatController, MemoryController, PermissionController controller updates
  - BeingView, ChatView, MemoryView, SoulEditorView view refactoring
  - Removed old MarkdownEditorView
  - InitController componentization migration
  - 115 files changed, 5761 lines added, 2362 lines deleted

### 2026-04-30

#### System Tray Functionality
- `101b203` - Implemented tray status window and ApplicationContext
  - Added tray icon resources (alpha.png, noWord.png, slc.ico, wordIcon.png)
  - Implemented TrayStatusWindow status window
  - Supports tray localization in 9 languages (TrayCsCZ, TrayDeDE, TrayEnUS, etc.)
  - TrayLocalizationBase abstract base class
  - 24 files changed, 27995 lines added, 1 line deleted (including resource files)

#### Componentized UI Architecture
- `e61cfaa` - Completed componentized UI architecture, implemented 24 components
  - MVP phase (8): ComponentBase, Div, Span, Button, Input, Form, Select, Label
  - Phase 2 (6): Accordion, Card, Tabs, Table, Modal, Message
  - Phase 3 (5): Calendar, Tree, Chart, FileUpload, RichText
  - Added Js, Behavior, DomUpdate and other helper classes
  - 25 files changed, 2666 lines added

- `7449e51` - Improved component system and added new skin themes
  - Enhanced A, Button, Div, Form, Input and other components
  - Added 3 skin themes: HighContrast, Light, Minimal
  - Updated existing skins (Admin, Chat, Creative, Dev)
  - InitController componentization migration
  - 32 files changed, 1466 lines added, 1238 lines deleted

- `1ba8636` - Started InitController componentization migration (in progress)
  - 9 files changed, 574 lines added, 145 lines deleted

#### Storage System Unification
- `895dff9` - Unified soul.md and state.json to use IStorage interface
  - DefaultSiliconBeing uses IStorage to read/write soul files and state
  - Added StateFileManager state file manager
  - SoulFileManager refactored to adapt to IStorage
  - 8 files changed, 201 lines added, 116 lines deleted

#### LiteDB Management Enhancement
- `a34bef4` - Added LiteDBManager and enhanced tray localization
  - Added LiteDB management entry to tray menu
  - Updated tray localization in 9 languages
  - 10 files changed, 196 lines added

- `c4a79ca` - Added language-aware localization factory for LiteDB management window
  - 1 file changed, 78 lines added

- `5ebc55e` - Converted LiteDBAdminLocalization to abstract base class
  - 10 files changed, 1356 lines added

#### Configuration System Fix
- `2da5256` - Added ConfigExists abstract method and fixed LiteDB duplicate configuration records
  - ConfigDataBase added ConfigExists method
  - Fast version DefaultConfigData implements LiteDB configuration existence check
  - Fixed LiteDB duplicate configuration key issue
  - 9 files changed, 210 lines added, 2 lines deleted

#### Chat and View Optimization
- `d3618ec` - Optimized chat sessions, storage system, time model, and view base classes
  - BroadcastChannel, GroupChatSession, SingleChatSession optimizations
  - ITimeStorage added query methods
  - FileSystemStorage and LiteDBStorage synchronized updates
  - ViewBase refactoring optimization (Default and Fast versions)
  - 11 files changed, 622 lines added, 392 lines deleted

### 2026-04-29

#### Architecture Refactoring: Shared Module Extraction
- `a102428` - Migrated shared modules from SiliconLife.Default to SiliconLife.Common
  - Extracted 32 calendar implementations to Common project
  - Extracted localization base classes and 21 language implementations to Common project
  - Extracted permission manager and default silicon being implementation to Common project
  - Extracted 23 built-in tool implementations to Common project
  - Extracted Playwright WebView implementation to Common project
  - Updated namespace to SiliconLife.Collective
  - 122 files changed, 586 lines added, 343 lines deleted

#### Code Quality Improvement
- `17566fe` - Replaced Console.WriteLine with logging system in Core, Common, and Default projects
  - ContextManager, AuditLogger, DefaultConfigData and 6 other files updated
  - Unified use of ILogger interface, improving code maintainability
  - 6 files changed, 12 lines added, 8 lines deleted

#### SiliconLife.Fast High-Performance Version
- `54a0307` - Added SiliconLife.Fast project and completed compilation fixes
  - Complete Windows Forms application entry point
  - System tray support (NotifyIcon)
  - Ported all Web UI controllers (20+)
  - Ported all Web view components
  - Ported 4 skin themes (Admin, Chat, Creative, Dev)
  - 125 files changed, 61186 lines added

#### Multi-language Documentation Synchronization
- `265fde8` - Synchronized dual-version architecture documentation to all languages
  - Updated architecture.md, changelog.md in 7 languages
  - Updated contributing.md in 6 languages
  - Updated getting-started.md, roadmap.md in 7 languages
  - 47 files changed, 1214 lines added, 38 lines deleted

#### LiteDB Storage System (Fast Version)
- `4704862` - Added LiteDB dependencies and infrastructure
  - Added LiteDBManager management class
  - Added LiteDBModels data models
  - 3 files changed, 252 lines added

- `4220036` - Implemented LiteDB storage classes
  - LiteDBStorage: implements IStorage interface
  - LiteDBTimeStorage: implements ITimeStorage interface
  - LiteDBWorkNoteStorage: implements IWorkNoteStorage interface
  - 3 files changed, 581 lines added

- `38ebd23` - Migrated configuration and logging system to LiteDB
  - DefaultConfigData adapted to LiteDB storage
  - Added LiteDBLoggerProvider logging provider
  - 2 files changed, 203 lines added, 67 lines deleted

- `e687157` - Migrated knowledge network from file system to LiteDB
  - KnowledgeNetwork fully refactored, using LiteDB to store triple data
  - 1 file changed, 231 lines added, 72 lines deleted

- `4220169` - Integrated LiteDB storage into Program and ProjectManager
  - Program.cs initializes LiteDB storage
  - ProjectManager adapted to LiteDB work note storage
  - 2 files changed, 40 lines added, 17 lines deleted

- `5f3a709` - Removed deprecated file system storage implementations
  - Deleted FileSystemLoggerProvider, FileSystemStorage, FileSystemTimeStorage, etc.
  - 6 files changed, 1518 lines deleted

- `e1a4ef2` - docs: add v0.1.0-alpha version identifier to all documentation
  - 127 files changed, 2297 lines added, 2471 lines deleted

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### Storage System Refactoring
- `8dd26e3` - Unified ITimeStorage interface to use IncompleteDate and added hierarchical query API
  - Removed DateTime overload methods from ITimeStorage interface, unified to use IncompleteDate
  - Added CompareTo(DateTime) comparison method and Expand() expansion method to IncompleteDate
  - Added GetEarliestTimestamp(), GetLatestTimestamp() hierarchical query API
  - Added HasSummary() and QueryWithLevel() methods, supporting queries by time level
  - Memory.cs refactored compression algorithm, using new hierarchical query API to improve efficiency
  - FileSystemTimeStorage.cs fully implements new interface methods
  - Synchronized updates to all callers: ChatSystem, ChatSession, BroadcastChannel, AuditLogger, TokenUsageRecord, etc.
  - Tool system updates: HelpTool, LogTool, TokenAuditTool adapted to new interface
  - Web controller updates: AuditController, ChatController, ChatHistoryController adapted to new interface
  - 41 files changed, 1820 lines added, 903 lines deleted

### 2026-04-27

#### Help Documentation System Enhancement
- `9989d79` - Updated localization, help system, and web views
  - Added IAIClientFactoryHelp.cs AI client factory help documentation interface
  - Completed 9-language translation for all help documents
  - HelpTopics.cs added 40 help topic definitions
  - Web views comprehensively updated: InitController, AuditView, ConfigView, KnowledgeView, LogView, etc.
  - Localization system enhancement: all language versions added new localization keys
  - AI client factory updates: DashScopeClientFactory, OllamaClientFactory improvements
  - 30 files changed, 10086 lines added, 15 lines deleted

#### Help Documentation New Content
- `e7afe94` - Added soul file and audit log help documentation
  - Added soul file management help documentation
  - Added audit log help documentation
  - HelpTopics.cs added topic definitions
  - HelpView.cs significantly refactored, improved document rendering logic
  - PermissionView.cs refactored, improved permission management interface
  - Core module enhancement: SiliconBeingManager, TaskSystem, ToolManager improvements
  - TaskTool.cs refactored, improved task management functionality
  - Web views comprehensively updated: all view components synchronized
  - HelpController.cs simplified, optimized controller logic
  - 30 files changed, 7100 lines added, 897 lines deleted

### 2026-04-26

#### Help Documentation System
- `07895d7` - Enhanced help documentation system, added 3 documents and completed 9-language translation
  - Added memory system, Ollama installation configuration, Alibaba Cloud DashScope platform usage guide
  - Completed 9-language translation for all 10 help documents
  - Simplified HelpView rendering logic
  - 18 files changed, 14418 lines added, 1364 lines deleted

#### German Localization
- `0cfd8a1` - Added complete German (de-DE) localization support
  - Complete German localization files
  - Added Chinese historical calendar German support
  - Added help documentation German translation
  - Fully synchronized all documents in 9 languages
  - 135 files changed, 26186 lines added, 14371 lines deleted

#### Documentation Synchronization
- `3aada7d` - Synchronized Traditional Chinese (zh-HK) documentation with Simplified Chinese
  - 3 files changed, 519 lines added, 422 lines deleted
- `2f6abff` - Added help tool display name localization for all languages
  - 7 files changed, 47 lines added, 7 lines deleted

#### Knowledge System Refactoring
- `60944fe` - Unified namespace to SiliconLife.Collective
  - 8 files changed, 5 lines added, 8 lines deleted
- `69c51c5` - Added help documentation system and translated code comments to English
  - 29 files changed, 3385 lines added, 22 lines deleted

### 2026-04-25

#### WebView Browser Automation
- `41757c3` - Implemented cross-platform WebView browser automation based on Playwright
  - 6 files changed, 1152 lines added

#### Documentation Updates
- `0ff797b` - Added KnowledgeTool and WorkNoteTool documentation (7 languages)
  - 28 files changed, 4983 lines added
- `ad77415` - Updated all changelog files, added 2026-04-25 Git history
  - 7 files changed, 168 lines added

#### Project Workspace Management
- `785c551` - Implemented project workspace management with work notes and task system
  - Added project workspace management system
  - Work notes functionality for tracking project progress
  - Task management system integration
  - 29 files changed, 4256 lines added, 36 lines deleted

#### Czech Localization
- `b4bbf39` - Added complete Czech (cs-CZ) localization and updated all language documentation
  - 116 files changed, 4933 lines added, 222 lines deleted
- `faf078f` - Fixed Czech localization compilation errors
  - 3 files changed, 910 lines added, 1 line deleted

#### Knowledge System Enhancement
- `20adaac` - Added KnowledgeTool with full localization support
  - 34 files changed, 2331 lines added, 56 lines deleted

### 2026-04-24

#### Memory Management Enhancement
- `c7b2ecc` - Enhanced memory management with advanced filtering, statistics, and detail views
  - Added advanced memory filtering
  - Implemented memory statistics
  - Added memory detail view page
  - Multi-language localization support (6 languages)
  - 13 files changed, 840 lines added, 86 lines deleted

#### Permission System Extension
- `4489ad6` - Added wttr.in weather service to network whitelist
  - Complete multi-language documentation synchronization (6 languages)
  - 14 files changed, 417 lines added, 1 line deleted

#### Web Interface Fixes
- `d9d72e9` - Fixed work note detail modal CSS priority issue
  - 19 files changed, 1744 lines added, 6 lines deleted

#### Chat History Optimization
- `0df599c` - Fixed tool results being rendered as separate chat messages
  - 1 file changed, 222 lines added, 21 lines deleted
- `057b09d` - Optimized chat history detail display, improved tool call rendering
  - 3 files changed, 389 lines added, 68 lines deleted

#### Timer Execution History
- `fa3f06f` - Added timer execution history feature with detail view
  - 8 files changed, 937 lines added, 10 lines deleted
- `d824835` - Added timer execution history localization keys (all languages)
  - 7 files changed, 88 lines added

#### Localization Enhancement
- `c13cb17` - Registered Spanish language variant
  - 1 file changed, 4 lines added
- `9c44f34` - Added Chinese historical calendar multi-language localization support
  - 16 files changed, 6049 lines added, 1 line deleted

#### Core Functionality Improvements
- `1e7c7b2` - Improved memory compression and tool execution tracking
  - 4 files changed, 338 lines added, 86 lines deleted

### 2026-04-23

#### Tool Localization
- `192fc6e` - Added missing tool name localization for 5 tools
  - 6 files changed, 30 lines added

#### Documentation Updates
- `882c08f` - Updated all changelog files, added complete Git history and removed fake version numbers
  - 45 files changed, 8815 lines added, 1611 lines deleted

#### Chat Page Enhancement
- `65c157b` - Added loading indicator to chat page and auto-selected curator session
  - 10 files changed, 211 lines added, 7 lines deleted

#### Chat History Feature
- `e483348` - Implemented silicon being chat history viewing feature
  - Added ChatHistoryController
  - Created ChatHistoryViewModel
  - Implemented ChatHistoryListView and ChatHistoryDetailView pages
  - Added localization keys for chat history (5 languages)
  - 12 files changed, 1178 lines added

#### AI Flow Control Enhancement
- `30a2d4e` - Enhanced AI flow cancellation, IM integration, and core host initialization
  - 11 files changed, 387 lines added, 12 lines deleted

#### Chat Message Queue
- `db48c51` - Added chat message queue, file metadata, and stream cancellation support
  - 4 files changed, 357 lines added

#### File Upload Support
- `28fb344` - Implemented file source dialog and file upload support
  - 3 files changed, 1100 lines added, 2 lines deleted
- `1d3e2cc` - Added file source dialog localization strings (6 languages)
  - 6 files changed, 30 lines added

#### Documentation Updates
- `8111e92` - Added Wiki link to README repository section
  - 1 file changed, 3 lines added, 1 line deleted

### 2026-04-22

#### Documentation Localization
- `66c11eb` - Translated Chinese comments to English and updated all changelogs
  - 11 files changed, 373 lines added, 163 lines deleted

#### SSE Message Enhancement
- `b574b2b` - Added senderName to historical messages for AI identification
  - 1 file changed, 9 lines added

#### Chat Features
- `601fc14` - Added mark_read action for session end marking
  - 7 files changed, 196 lines added, 36 lines deleted

#### Tool System Optimization
- `7a03a19` - Improved LogTool conversation query flexibility
  - 1 file changed, 57 lines added, 24 lines deleted

#### Localization Enhancement
- `0a8d750` - Added common system prompt for active silicon being behaviors
  - 8 files changed, 460 lines added, 48 lines deleted

#### Log System Refactoring
- `2b771f3` - Decoupled LogController from file I/O, added log read API
  - 4 files changed, 172 lines added, 137 lines deleted
- `12da302` - Added silicon being filter to log view
  - 9 files changed, 147 lines added, 10 lines deleted
- `8f6cb1e` - Added beingId parameter to ILogger interface, implemented system/silicon being log separation
  - 47 files changed, 524 lines added, 490 lines deleted

#### Permission System Improvements
- `4c747ad` - Refactored PermissionTool, ExecuteCodeTool, added EvaluatePermission API
  - 18 files changed, 680 lines added, 492 lines deleted

#### Bug Fixes
- `1c96e99` - Fixed search_files and search_content root directory search failure
  - 1 file changed, 98 lines added, 41 lines deleted

#### Tool Integration
- `135710d` - Removed SearchTool, moved local search to DiskTool
  - 2 files changed, 185 lines added, 365 lines deleted

#### Tool System Extension
- `70ce7fb` - Implemented DatabaseTool for structured database queries
  - 1 file changed, 382 lines added
- `be29a09` - Implemented LogTool for operation and conversation history queries
  - 1 file changed, 298 lines added
- `4ea7702` - Implemented PermissionTool for dynamic permission management
  - 1 file changed, 457 lines added
- `1384ff4` - Implemented ExecuteCodeTool for multi-language code execution
  - 1 file changed, 477 lines added
- `82d1e11` - Implemented SearchTool for information retrieval
  - 1 file changed, 363 lines added

#### Web Interface Optimization
- `0675c45` - Optimized markdown code block highlighting in preview pane
  - 1 file changed, 4 lines added, 23 lines deleted
- `702b3f3` - Enhanced task view with status badges and metadata display
  - 8 files changed, 221 lines added, 9 lines deleted
- `6ed9a79` - Improved chat message storage and view rendering
  - 8 files changed, 140 lines added, 29 lines deleted

### 2026-04-21

#### Bug Fixes
- `c6b518b` - Fixed timer message delivery and chat message storage
  - 3 files changed, 297 lines added, 124 lines deleted

#### Configuration Management
- `4305769` - Added .gitattributes for line ending management
  - 1 file changed, 32 lines added

#### Web Interface Improvements
- `188c6f8` - Registered task list API route and added empty state display
  - 2 files changed, 35 lines added, 2 lines deleted
- `634e8ca` - Added permission page return to list link
  - 1 file changed, 16 lines added
- `6ba591d` - Added independent AI configuration editor for silicon beings
  - 11 files changed, 842 lines added, 18 lines deleted
- `0a826f5` - Added save success prompt in code editor
  - 1 file changed, 9 lines added, 2 lines deleted
- `2940373` - Enhanced web interface with code hover hints and UI improvements
  - 11 files changed, 1054 lines added, 75 lines deleted

#### Permission System Fixes
- `592c7ab` - Fixed callback instantiation and registration order
  - 2 files changed, 38 lines added, 7 lines deleted

#### Security Enhancement
- `833ead2` - Added assembly reference verification for dynamic compilation
  - 4 files changed, 135 lines added, 8 lines deleted

#### Permission System Enhancement
- `5879621` - Added permission callback pre-compilation verification and enhanced error handling
  - 21 files changed, 617 lines added, 26 lines deleted

#### Documentation Updates
- `4dbf659` - Updated changelog to v0.5.1, replaced GitHub placeholder URLs, added Gitee mirror, localized Bilibili name by language, updated email
  - 32 files changed, 489 lines added, 180 lines deleted

#### Configuration and Entry
- `0fc1693` - Updated program entry and project configuration
  - 2 files changed, 7 lines added

#### Permission System Refactoring
- `ea9179a` - Improved permission system implementation
  - 5 files changed, 358 lines added, 152 lines deleted

#### Bug Fixes
- `928a96d` - Fixed calendar calculation implementation
  - 4 files changed, 12 lines added, 12 lines deleted

#### AI and Calendar
- `646813e` - Improved AI client factory implementation
  - 2 files changed, 21 lines added, 20 lines deleted

#### Localization
- `7940d9c` - Added Korean localization support
  - 7 files changed, 2424 lines added, 10 lines deleted
- `4ff98ad` - Refactored documentation for multi-language support
  - 81 files changed, 23818 lines added, 1886 lines deleted

### 2026-04-20

#### Core Functionality Completion
- `28905b5` - Complete multi-language support, AI client factory, permission system, and localization setup
  - Log system with manager, entries, and different log levels
  - Token audit system for querying and tracking token usage
  - AI client factories for auto-discovering different AI platforms
  - Permission callback system with its own storage
  - Console logger implementation
  - Multi-language support for English and Simplified Chinese
  - WebUI messenger with WebSocket for real-time chat
  - Enhanced default silicon being with localization
  - 39 files changed, 4670 lines added, 175 lines deleted

### 2026-04-19

#### Timer and Calendar
- `c933fd8` - Updated localization, timer system, web views, and added tools
  - Better localization manager
  - Scheduling system for timed tasks
  - AI configuration and context management
  - Calendar tool supporting 32 calendar types
  - Web controller for calendar APIs
  - Task management tool
  - 46 files changed, 4018 lines added, 975 lines deleted

**Architecture Improvements**
- Redesigned web view architecture for better skin support
- Improved being management system with better state handling

### 2026-04-18

- `9f585e1` - Updated localization, timer system, web views, and added tools
  - Timer and scheduling improvements
  - Better web views with improved UI components
  - More tool implementations
  - 57 files changed, 3328 lines added, 389 lines deleted

### 2026-04-17

- `9b71fcd` - Updated core modules, added zh-HK documentation, broadcast channel, config tools, and audit web views
  - Broadcast channel for multiple silicon beings chatting together
  - Configuration tool system
  - Audit web views
  - Traditional Chinese documentation
  - 42 files changed, 3533 lines added, 268 lines deleted

### 2026-04-16

- `5040f05` - Updated core and default modules
  - Module optimization and bug fixes
  - Implementation updates and improvements
  - 58 files changed, 9916 lines added, 111 lines deleted

### 2026-04-15

- `3efab5f` - Updated multiple modules: AI, Chat, IM, Tools, Web, Localization, Storage
  - AI client improvements
  - Chat system enhancement
  - Messenger provider updates
  - Tool system optimization
  - Web infrastructure improvements
  - Localization optimization
  - Storage system updates
  - 33 files changed, 788 lines added, 232 lines deleted

### 2026-04-14

- `4241a2f` - Chat features basically complete, UI upload optimization
  - Chat system functionality completed
  - UI optimization for file uploads
  - 16 files changed, 1234 lines added, 102 lines deleted

### 2026-04-13

- `c498c31` - Code updates
  - General code improvements and optimization
  - 32 files changed, 1045 lines added, 546 lines deleted

### 2026-04-12

#### Documentation and Localization
- `2161002` - Refactored documentation and enhanced localization
  - 17 files changed, 982 lines added, 92 lines deleted
- `03d94e4` - Enhanced configuration system and localization
  - 25 files changed, 1378 lines added, 154 lines deleted
- `9976a35` - Added about page and localization
  - 14 files changed, 699 lines added, 44 lines deleted

#### Chat and Web Views
- `0c8ccfc` - Enhanced chat system, localization, and web views
  - 13 files changed, 402 lines added, 56 lines deleted
- `a8f1342` - Redesigned web communication layer, switched from WebSocket to SSE
  - 27 files changed, 793 lines added, 935 lines deleted

### 2026-04-11

#### Log System
- `e8fe259` - Added log system and code optimization
  - 37 files changed, 624 lines added, 91 lines deleted
- `f01c519` - Added log system, updated AI interface and web views
  - 31 files changed, 1758 lines added, 63 lines deleted

### 2026-04-10

- `4962924` - Enhanced WebSocket handler, chat views, and messenger interaction
  - Context manager improvements
  - Chat system enhancement
  - Messenger provider interface updates
  - WebUI provider redesign
  - JavaScript builder and router updates
  - Chat view optimization
  - WebSocket handler improvements
  - 9 files changed, 365 lines added, 134 lines deleted

### 2026-04-09

- `f9302bf` - Enhanced messenger provider interface, chat system, and web UI interaction
  - Messenger provider interface extension
  - Chat messages and system improvements
  - Context manager optimization
  - Default silicon being enhancement
  - Web UI chat view improvements
  - WebSocket handler updates
  - 10 files changed, 427 lines added, 93 lines deleted

### 2026-04-07

- `6831ee8` - Redesigned web views and JavaScript builder
  - Complete web controller redesign
  - JavaScript builder complete rewrite
  - All view components updated
  - Skin system improvements
  - View base class architecture upgrade
  - 23 files changed, 2004 lines added, 1983 lines deleted

### 2026-04-05

- `41e97fb` - Updated multiple core modules and web controllers
  - Context manager improvements
  - Chat system and session management
  - Service locator redesign
  - Silicon being base class and manager updates
  - Web controllers comprehensively updated (17 controllers)
  - Default silicon being factory improvements
  - 31 files changed, 681 lines added, 326 lines deleted
- `67988d4` - Improved web UI module, added executor view, cleaned up views and core modules
  - 61 files changed, 3148 lines added, 3726 lines deleted

### 2026-04-04

- `b58bb1c` - Added initialization controller and redesigned web module
  - Initialization controller
  - Configuration module redesign
  - Localization module updates
  - Skin system improvements
  - Router enhancement
  - 29 files changed, 1269 lines added, 289 lines deleted
- `f03ac0b` - Added web UI module, improved messenger functionality
  - 60 files changed, 8481 lines added, 165 lines deleted

### 2026-04-03

- `192e57b` - Updated project structure and core runtime components
  - 22 files changed, 446 lines added, 179 lines deleted
- `59faec8` - Core and default implementation updates
  - 25 files changed, 3056 lines added, 18 lines deleted
- `d488485` - Added dynamic compilation functionality and curator tool module
  - 19 files changed, 1727 lines added, 11 lines deleted
- `753d1d9` - Added security module, updated executors, messenger providers, localization, and tools
  - 29 files changed, 2352 lines added, 93 lines deleted
- `a378697` - Completed stage 5 - tool system + executors
  - 41 files changed, 2651 lines added, 363 lines deleted

### 2026-04-02

- `e6ad94b` - Fixed chat history loading failure when deleting configuration files during testing
  - 4 files changed, 49 lines added, 45 lines deleted
- `daa56f5` - Completed stage 4: persistent memory (chat system + messenger channel)
  - 29 files changed, 2051 lines added, 538 lines deleted

### 2026-04-01

- `bbe2dbb` - Fixed configuration loading and chat service message routing
  - 27 files changed, 1633 lines added, 147 lines deleted
- `2fa6305` - Implemented stage 2: main loop framework and clock object system
  - 9 files changed, 594 lines added, 41 lines deleted
- `32b99a1` - Implemented stage 1 - basic chat functionality
  - 19 files changed, 1185 lines added
- `358e368` - Initial commit: project documentation and license
  - 10 files changed, 1873 lines added