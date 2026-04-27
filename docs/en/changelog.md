# Changelog

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md)

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## About This Changelog

### Project Origin

- This project originated on March 20, 2026.
- Before this project, there was a verification demo that failed due to irrational architecture design, which prevented integration with multiple AI platforms.

### AI IDE Tools Used

#### Kiro (Amazon AWS)
- The project was initially maintained by Kiro and started using Spec mode.
- Kiro is an agentic AI development environment built by Amazon AWS.
- Based on Code OSS (VS Code), supports VS Code settings and Open VSX compatible extensions.
- Features spec-driven development workflow for structured AI coding.

#### Comate AI IDE / Wenxin Kuaima (Baidu)
- Occasionally used for copywriting and documentation work.
- Comate AI IDE is an AI-native development environment tool released by Baidu Wenxin on June 23, 2025.
- Industry's first multi-modal, multi-agent collaborative AI IDE.
- Features include design-to-code conversion and full-process AI-assisted coding.
- Powered by Baidu Wenxin 4.0 X1 Turbo model.

#### Trae (ByteDance)
- This project was primarily maintained using Trae for most of the time.
- Trae is an AI IDE developed by SPRING PTE, a Singapore subsidiary of ByteDance.
- As a 10x AI Engineer, capable of independently building software solutions.
- Features intelligent productivity tools, flexible development rhythm adaptation, and collaborative project delivery.
- Provides enterprise-grade performance with configurable agent system.

#### Qoder (Alibaba)
- Since April 18, 2026, this project has been maintained using Qoder.
- Qoder excels at source code analysis and domain documentation generation, performing exceptionally well in understanding complex codebases.
- Adopts zero-computation-cost pricing model, making it highly cost-effective for automated documentation processing and routine task handling.
- An AI-powered agentic coding platform designed for real-world software development.
- Features intelligent code generation, conversational programming, advanced context analysis engine, and multi-agent collaboration.
- Provides deep code understanding with minimal resource consumption, ideal for long-term project maintenance and knowledge accumulation.

### Requirements Document

- The requirements document for this project is not publicly available.
- Requirements have been validated through iterations with 12+ international AI platforms and large model series, producing a user story-driven requirements document of over 2000 lines that is almost incomprehensible to humans.

---

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
  - Total: 41 files changed (+1820/-903 lines)

### 2026-04-27

#### Help Documentation System Enhancement
- `9989d79` - Updated localization, help system, and web views
  - Added IAIClientFactoryHelp.cs AI client factory help documentation interface
  - Completed 9-language translation for all help documents
  - HelpTopics.cs added 40 help topic definitions
  - Web views comprehensively updated: InitController, AuditView, ConfigView, KnowledgeView, LogView, etc.
  - Localization system enhancement: all language versions added new localization keys
  - AI client factory updates: DashScopeClientFactory, OllamaClientFactory improvements

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

### 2026-04-26

#### Help Documentation System
- `07895d7` - Enhanced help documentation system, added 3 documents and completed 9-language translation
  - Added memory system, Ollama installation configuration, Alibaba Cloud Bailian platform usage guide
  - Completed 9-language translation for all 10 help documents
  - Simplified HelpView rendering logic

#### German Localization
- `0cfd8a1` - Added complete German (de-DE) localization support
  - Complete German localization files
  - Added Chinese historical calendar German support
  - Added help documentation German translation
  - Fully synchronized all documents in 9 languages

#### Documentation Synchronization
- `3aada7d` - Synchronized Traditional Chinese (zh-HK) documentation with Simplified Chinese
- `2f6abff` - Added help tool display name localization for all languages

#### Knowledge System Refactoring
- `60944fe` - Unified namespace to SiliconLife.Collective
- `69c51c5` - Added help documentation system and translated code comments to English

### 2026-04-25

#### WebView Browser Automation
- `41757c3` - Implemented cross-platform WebView browser automation based on Playwright

#### Documentation Updates
- `0ff797b` - Added KnowledgeTool and WorkNoteTool documentation (7 languages)
- `ad77415` - Updated all changelog files, added 2026-04-25 Git history

#### Project Workspace Management
- `785c551` - Implemented project workspace management with work notes and task system
  - Added project workspace management system
  - Work notes functionality for tracking project progress
  - Task management system integration

#### Czech Localization
- `b4bbf39` - Added complete Czech (cs-CZ) localization and updated all language documentation
- `faf078f` - Fixed Czech localization compilation errors

#### Knowledge System Enhancement
- `20adaac` - Added KnowledgeTool with full localization support

### 2026-04-24

#### Memory Management Enhancement
- `c7b2ecc` - Enhanced memory management with advanced filtering, statistics, and detail views
  - Added advanced memory filtering
  - Implemented memory statistics
  - Added memory detail view page
  - Multi-language localization support (6 languages)

#### Permission System Extension
- `4489ad6` - Added wttr.in weather service to network whitelist
  - Complete multi-language documentation synchronization (6 languages)

#### Web Interface Fixes
- `d9d72e9` - Fixed work note detail modal CSS priority issue

#### Chat History Optimization
- `0df599c` - Fixed tool results being rendered as separate chat messages
- `057b09d` - Optimized chat history detail display, improved tool call rendering

#### Timer Execution History
- `fa3f06f` - Added timer execution history feature with detail view
- `d824835` - Added timer execution history localization keys (all languages)

#### Localization Enhancement
- `c13cb17` - Registered Spanish language variant
- `9c44f34` - Added Chinese historical calendar multi-language localization support

#### Core Functionality Improvements
- `1e7c7b2` - Improved memory compression and tool execution tracking

### 2026-04-23

#### Tool Localization
- `192fc6e` - Added missing tool name localization for 5 tools

#### Documentation Updates
- `882c08f` - Updated all changelog files, added complete Git history and removed fake version numbers

#### Chat Page Enhancement
- `65c157b` - Added loading indicator to chat page and auto-selected curator session

#### Chat History Feature
- `e483348` - Implemented silicon being chat history viewing feature
  - Added ChatHistoryController
  - Created ChatHistoryViewModel
  - Implemented ChatHistoryListView and ChatHistoryDetailView pages
  - Added localization keys for chat history (5 languages)

#### AI Flow Control Enhancement
- `30a2d4e` - Enhanced AI flow cancellation, IM integration, and core host initialization

#### Chat Message Queue
- `db48c51` - Added chat message queue, file metadata, and stream cancellation support

#### File Upload Support
- `28fb344` - Implemented file source dialog and file upload support
- `1d3e2cc` - Added file source dialog localization strings (6 languages)

#### Documentation Updates
- `8111e92` - Added Wiki link to README repository section

### 2026-04-22

#### Documentation Localization
- `66c11eb` - Translated Chinese comments to English and updated all changelogs

#### SSE Message Enhancement
- `b574b2b` - Added senderName to historical messages for AI identification

#### Chat Features
- `601fc14` - Added mark_read action for session end marking

#### Tool System Optimization
- `7a03a19` - Improved LogTool conversation query flexibility

#### Localization Enhancement
- `0a8d750` - Added common system prompt for active silicon being behaviors

#### Log System Refactoring
- `2b771f3` - Decoupled LogController from file I/O, added log read API
- `12da302` - Added silicon being filter to log view
- `8f6cb1e` - Added beingId parameter to ILogger interface, implemented system/silicon being log separation

#### Permission System Improvements
- `4c747ad` - Refactored PermissionTool, ExecuteCodeTool, added EvaluatePermission API

#### Bug Fixes
- `1c96e99` - Fixed search_files and search_content root directory search failure

#### Tool Integration
- `135710d` - Removed SearchTool, moved local search to DiskTool

#### Tool System Extension
- `70ce7fb` - Implemented DatabaseTool for structured database queries
- `be29a09` - Implemented LogTool for operation and conversation history queries
- `4ea7702` - Implemented PermissionTool for dynamic permission management
- `1384ff4` - Implemented ExecuteCodeTool for multi-language code execution
- `82d1e11` - Implemented SearchTool for information retrieval

#### Web Interface Optimization
- `0675c45` - Optimized markdown code block highlighting in preview pane
- `702b3f3` - Enhanced task view with status badges and metadata display
- `6ed9a79` - Improved chat message storage and view rendering

### 2026-04-21

#### Bug Fixes
- `c6b518b` - Fixed timer message delivery and chat message storage

#### Configuration Management
- `4305769` - Added .gitattributes for line ending management

#### Web Interface Improvements
- `188c6f8` - Registered task list API route and added empty state display
- `634e8ca` - Added permission page return to list link
- `6ba591d` - Added independent AI configuration editor for silicon beings
- `0a826f5` - Added save success prompt in code editor
- `2940373` - Enhanced web interface with code hover hints and UI improvements

#### Permission System Fixes
- `592c7ab` - Fixed callback instantiation and registration order

#### Security Enhancement
- `833ead2` - Added assembly reference verification for dynamic compilation

#### Permission System Enhancement
- `5879621` - Added permission callback pre-compilation verification and enhanced error handling

#### Documentation Updates
- `4dbf659` - Updated changelog to v0.5.1, replaced GitHub placeholder URLs, added Gitee mirror, localized Bilibili name by language, updated email

#### Configuration and Entry
- `0fc1693` - Updated program entry and project configuration

#### Permission System Refactoring
- `ea9179a` - Improved permission system implementation

#### Bug Fixes
- `928a96d` - Fixed calendar calculation implementation

#### AI and Calendar
- `646813e` - Improved AI client factory implementation

#### Localization
- `7940d9c` - Added Korean localization support
- `4ff98ad` - Refactored documentation for multi-language support

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

### 2026-04-19

#### Timer and Calendar
- `c933fd8` - Updated localization, timer system, web views, and added tools
  - Better localization manager
  - Scheduling system for timed tasks
  - AI configuration and context management
  - Calendar tool supporting 32 calendar types
  - Web controller for calendar APIs
  - Task management tool

**Architecture Improvements**
- Redesigned web view architecture for better skin support
- Improved being management system with better state handling

### 2026-04-18

- `9f585e1` - Updated localization, timer system, web views, and added tools
  - Timer and scheduling improvements
  - Better web views with improved UI components
  - More tool implementations

### 2026-04-17

- `9b71fcd` - Updated core modules, added zh-HK documentation, broadcast channel, config tools, and audit web views
  - Broadcast channel for multiple silicon beings chatting together
  - Configuration tool system
  - Audit web views
  - Traditional Chinese documentation

### 2026-04-16

- `5040f05` - Updated core and default modules
  - Module optimization and bug fixes
  - Implementation updates and improvements

### 2026-04-15

- `3efab5f` - Updated multiple modules: AI, Chat, IM, Tools, Web, Localization, Storage
  - AI client improvements
  - Chat system enhancement
  - Messenger provider updates
  - Tool system optimization
  - Web infrastructure improvements
  - Localization optimization
  - Storage system updates

### 2026-04-14

- `4241a2f` - Chat features basically complete, UI upload optimization
  - Chat system functionality completed
  - UI optimization for file uploads

### 2026-04-13

- `c498c31` - Code updates
  - General code improvements and optimization

### 2026-04-12

#### Documentation and Localization
- `2161002` - Refactored documentation and enhanced localization
- `03d94e4` - Enhanced configuration system and localization
- `9976a35` - Added about page and localization

#### Chat and Web Views
- `0c8ccfc` - Enhanced chat system, localization, and web views
- `a8f1342` - Redesigned web communication layer, switched from WebSocket to SSE

### 2026-04-11

#### Log System
- `e8fe259` - Added log system and code optimization
- `f01c519` - Added log system, updated AI interface and web views

### 2026-04-10

- `4962924` - Enhanced WebSocket handler, chat views, and messenger interaction
  - Context manager improvements
  - Chat system enhancement
  - Messenger provider interface updates
  - WebUI provider redesign
  - JavaScript builder and router updates
  - Chat view optimization
  - WebSocket handler improvements

### 2026-04-09

- `f9302bf` - Enhanced messenger provider interface, chat system, and web UI interaction
  - Messenger provider interface extension
  - Chat messages and system improvements
  - Context manager optimization
  - Default silicon being enhancement
  - Web UI chat view improvements
  - WebSocket handler updates

### 2026-04-07

- `6831ee8` - Redesigned web views and JavaScript builder
  - Complete web controller redesign
  - JavaScript builder complete rewrite
  - All view components updated
  - Skin system improvements
  - View base class architecture upgrade

### 2026-04-05

- `41e97fb` - Updated multiple core modules and web controllers
  - Context manager improvements
  - Chat system and session management
  - Service locator redesign
  - Silicon being base class and manager updates
  - Web controllers comprehensively updated (17 controllers)
  - Default silicon being factory improvements
- `67988d4` - Improved web UI module, added executor view, cleaned up views and core modules

### 2026-04-04

- `b58bb1c` - Added initialization controller and redesigned web module
  - Initialization controller
  - Configuration module redesign
  - Localization module updates
  - Skin system improvements
  - Router enhancement
- `f03ac0b` - Added web UI module, improved messenger functionality

### 2026-04-03

- `192e57b` - Updated project structure and core runtime components
- `59faec8` - Core and default implementation updates
- `d488485` - Added dynamic compilation functionality and curator tool module
- `753d1d9` - Added security module, updated executors, messenger providers, localization, and tools
- `a378697` - Completed stage 5 - tool system + executors

### 2026-04-02

- `e6ad94b` - Fixed chat history loading failure when deleting configuration files during testing
- `daa56f5` - Completed stage 4: persistent memory (chat system + messenger channel)

### 2026-04-01

- `bbe2dbb` - Fixed configuration loading and chat service message routing
- `2fa6305` - Implemented stage 2: main loop framework and clock object system
- `32b99a1` - Implemented stage 1 - basic chat functionality
- `358e368` - Initial commit: project documentation and license
