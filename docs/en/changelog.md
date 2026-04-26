# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

**Note: This project has not yet released any official versions, all content is under development.**

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

## [Unreleased]

### 2026-04-26

#### Help Documentation System Enhancement
- Add Memory System, Ollama Setup, and Bailian DashScope help documents (3 new documents)
- Complete translation of all 10 help documents to 9 languages (zh-CN, zh-HK, en-US, de-DE, cs-CZ, es-ES, ja-JP, ko-KR)
- Simplify HelpView rendering logic
- Total: ~14,000 lines of documentation across all languages

### 2026-04-25

#### Project Workspace Management
- `785c551` - Implement project workspace management with work notes and task system
  - New project workspace management system
  - Work notes functionality for tracking project progress
  - Task management system integration
  - Improved project organization and tracking capabilities

#### Czech Localization
- `b4bbf39` - Add complete Czech (cs-CZ) localization and update all language documentation
  - Complete Czech language support
  - Update all language documentation to include Czech
  - Full localization for UI elements and messages
- `faf078f` - Fix Czech localization compilation errors
  - Resolve compilation issues in Czech localization files
  - Ensure Czech language pack integrates correctly

#### Knowledge System Enhancement
- `20adaac` - Add KnowledgeTool with full localization support
  - New KnowledgeTool for knowledge management
  - Complete multi-language localization support
  - Enhanced knowledge network capabilities

### 2026-04-24

#### Memory Management Enhancement
- `c7b2ecc` - Enhance memory management features with advanced filtering, statistics, and detail views
  - New advanced memory filtering feature, supporting multi-dimensional filtering by type, time range, tags, etc.
  - Implemented memory statistics feature, displaying statistics such as memory count, type distribution, etc.
  - Added memory detail view page, supporting viewing complete information for single memory entries
  - Optimized memory management interface, improving user experience and operational efficiency
  - Multi-language localization support (6 languages)

#### Permission System Extension
- `4489ad6` - Add wttr.in weather service to network whitelist
  - Allow silicon beings to access wttr.in weather API for weather information
  - Update permission system documentation explaining weather service permission configuration
  - Complete multi-language documentation synchronization (6 languages)

#### Chat History Optimization
- `057b09d` - Optimize chat history detail display, improve tool call rendering
  - Chat history detail page tool call display optimization
  - Improved formatted display of tool call parameters
  - Enhanced readability of historical messages
- `0df599c` - Fix tool results being rendered as separate chat messages
  - Tool execution results now correctly associated with original messages
  - Avoid tool results appearing as separate AI replies
  - Improve chat message coherence

#### Web Interface Fixes
- `d9d72e9` - Fix work note detail modal CSS priority issue
  - Work note modal style fix
  - CSS priority adjustment to ensure styles apply correctly
  - Improved modal visual effects

#### Core Feature Improvements
- `1e7c7b2` - Improve memory compression and tool execution tracking
  - Memory compression algorithm optimization
  - Tool execution tracking mechanism enhancement
  - Improved silicon being memory management efficiency

#### Localization Enhancement
- `c13cb17` - Register Spanish language variant
  - Spanish (Spain) localization support
  - Multi-language system expansion
- `9c44f34` - Add Chinese historical calendar multi-language localization support
  - Full language localization for Chinese historical calendar
  - Multi-language support for historical era names, dynasty tables, and other information
- `192fc6e` - Add missing tool name localization for 5 tools
  - Supplement tool localized name display
  - Improve multi-language experience in tool interface

### 2026-04-23

#### Chat History & Loading Indicator
- `e483348` - Implement silicon being chat history viewing feature
  - New ChatHistoryController with session list and message detail APIs
  - Create ChatHistoryViewModel for data transfer
  - Implement ChatHistoryListView and ChatHistoryDetailView pages
  - Add localization keys for chat history (5 languages)
  - Update Router to include chat history routes
  - Add chat history entry link in BeingView detail page
- `65c157b` - Add loading indicator for chat page and auto-select curator session
  - Chat page loading status indicator
  - Auto-select curator session feature
  - Multi-language support (6 languages)

#### AI Stream Control Enhancement
- `30a2d4e` - Enhance AI stream cancellation, IM integration, and core host initialization
  - ContextManager stream cancellation mechanism enhancement
  - IM system integration improvement
  - CoreHost initialization optimization
  - DiskExecutor functionality enhancement
  - WebUIProvider updates

#### File Upload Support
- `28fb344` - Implement file source dialog and file upload support
  - Web interface file source dialog
  - File upload functionality implementation
- `1d3e2cc` - Add file source dialog localization strings
  - Multi-language support for file source dialog (6 languages)

#### Chat Message Queue
- `db48c51` - Add chat message queue, file metadata, and stream cancellation support
  - New ChatMessageQueue chat message queue system
  - New FileMetadata file metadata management
  - New StreamCancellationManager stream cancellation manager

### 2026-04-22

#### Localization Enhancement
- `b574b2b` - Add senderName to historical messages for AI identification
  - Add sender name field to SSE historical messages
  - Support identity identification for AI messages
- `0a8d750` - Add generic system prompt for proactive silicon being behaviors
  - New generic system prompt template in localization system
  - Support proactive behavior guidance for silicon beings

#### Tool System Extension
- `70ce7fb` - Implement DatabaseTool for structured database queries
  - New database query tool
  - Support structured data operations
- `be29a09` - Implement LogTool for operation and conversation history queries
  - New log query tool
  - Support operation history and conversation history retrieval
- `4ea7702` - Implement PermissionTool for dynamic permission management
  - New permission management tool
  - Support dynamic permission query and management
- `1384ff4` - Implement ExecuteCodeTool for multi-language code execution
  - New code execution tool
  - Support multi-language code compilation and execution
- `82d1e11` - Implement SearchTool for information retrieval
  - New information search tool
  - Support external information retrieval

#### Logging System Refactoring
- `8f6cb1e` - Add beingId parameter to ILogger interface, implement system/silicon being log separation
  - ILogger interface extension
  - Support system log and silicon being log separation
  - New beingId parameter
- `2b771f3` - Decouple LogController from file I/O, add log reading API
  - LogController architecture refactoring
  - New independent log reading API
  - Separate file I/O operations
- `12da302` - Add silicon being filter to log view
  - New silicon being filtering feature in web interface
  - Filter log records by silicon being

#### Permission System Improvements
- `4c747ad` - Refactor PermissionTool, ExecuteCodeTool, add EvaluatePermission API
  - PermissionTool and ExecuteCodeTool refactoring
  - Integrate EvaluatePermission API

#### Web Interface Optimization
- `702b3f3` - Enhance task view with status badges and metadata display
  - Task view UI improvement
  - New status badges and metadata display
- `6ed9a79` - Improve chat message storage and view rendering
  - Chat message storage mechanism optimization
  - View rendering performance improvement
- `0675c45` - Optimize markdown code block highlighting in preview pane
  - Markdown preview code highlighting optimization
  - Improved code block display effects

#### Tool Integration
- `135710d` - Remove SearchTool, move local search to DiskTool
  - SearchTool removal
  - Local search functionality integrated into DiskTool
- `7a03a19` - Improve LogTool conversation query flexibility
  - LogTool conversation query logic optimization
  - Improved query flexibility

#### Configuration Management
- `4305769` - Add .gitattributes for line ending management
  - Line ending configuration for cross-platform compatibility

#### Bug Fixes
- `1c96e99` - Fix search_files and search_content root directory search failure
  - DiskTool root directory search functionality fix
  - File and content search logic correction

### 2026-04-21

#### Permission System Enhancement
- `5879621` - Add permission callback pre-compilation verification and enhanced error handling
  - Permission callback pre-compilation verification feature
  - Compile permission callback code before saving to verify correctness
  - Prevent invalid code from being saved to disk
  - Detailed error handling for permission save operations
  - Enhanced error messages with localization support
  - Separate compilation and security scanning steps for better error reporting
- `833ead2` - Add assembly reference verification for dynamic compilation
  - Dynamic compilation security enhancement
  - Assembly reference verification mechanism

#### Web Interface Improvements
- `0a826f5` - Add save success prompt in code editor
  - Code editor save operation success prompt feature
  - Improved user operation feedback
- `2940373` - Enhance web interface with code hover hints and UI improvements
  - Code editor hover hint feature
  - Web interface UI optimization
- `6ba591d` - Add independent AI configuration editor for silicon beings
- `634e8ca` - Add return to list link on permission page
- `188c6f8` - Register task list API route and add empty state display

#### Bug Fixes
- `592c7ab` - Fix callback instantiation and registration order
  - Permission callback system fix
  - Callback registration order optimization
- `c6b518b` - Fix timer message delivery and chat message storage
  - Timer message delivery mechanism correction
  - Chat message storage optimization

#### Localization
- `7940d9c` - Add Korean localization support
  - Korean localization files
  - Multi-language system enhancement
- `4ff98ad` - Refactor documentation for multi-language support
  - Documentation structure reorganization
  - Multi-language documentation synchronization

#### AI & Calendar
- `646813e` - Improve AI client factory implementation
  - AI client factory refactoring
  - Client discovery mechanism optimization
- `928a96d` - Fix calendar calculation implementation
  - Calendar calculation logic correction
  - Improved calculation accuracy for multiple calendar types

#### Configuration & Entry
- `0fc1693` - Update program entry and project configuration
  - Program entry point optimization
  - Project configuration improvement

### 2026-04-20

#### Core Feature Completion
- `28905b5` - Complete multi-language support, AI client factory, permission system, and localization settings
  - Logging system with manager, entries, and different log levels
  - Token audit system for querying and tracking token usage
  - AI client factory for automatic discovery of different AI platforms
  - Permission callback system with its own storage
  - Console logger implementation
  - Multi-language support for English and Simplified Chinese
  - WebUI messenger with WebSocket for real-time chat
  - Enhanced default silicon being with localization

### 2026-04-19

#### Timers & Calendars
- `c933fd8` - Update localization, timer system, web views and add tools
  - Better localization manager
  - Scheduling system for timed tasks
  - AI configuration and context management
  - Calendar tool supporting 32 calendar types
  - Web controller for calendar API
  - Task management tool

**Architecture Improvements**
- Redesigned web view architecture for better skin support
- Improved being management system with better state handling

### 2026-04-18

- `9f585e1` - Update localization, timer system, web views and add tools
  - Timer and scheduling improvements
  - Better web views with improved UI components
  - More tool implementations

### 2026-04-17

- `9b71fcd` - Update core modules, add zh-HK documentation, broadcast channels, config tools, and audit web view
  - Broadcast channels for multiple beings to chat together
  - Configuration tool system
  - Audit web view
  - Traditional Chinese documentation

### 2026-04-16

- `5040f05` - Update core and default modules
  - Module optimization and bug fixes
  - Implementation updates and improvements

### 2026-04-15

- `3efab5f` - Update multiple modules: AI, Chat, IM, Tools, Web, Localization, Storage
  - AI client improvements
  - Chat system enhancement
  - Messenger provider updates
  - Tool system optimization
  - Web infrastructure improvements
  - Localization optimization
  - Storage system updates

### 2026-04-14

- `4241a2f` - Chat feature basically complete, UI upload optimization
  - Chat system functionality completed
  - UI optimization for file upload

### 2026-04-13

- `c498c31` - Code updates
  - General code improvements and optimization

### 2026-04-12

- `2161002` - Refactor documentation and enhance localization
  - Documentation reorganization
  - Localization system improvement
- `03d94e4` - Enhance configuration system and localization
  - Configuration system improvement
  - Additional language support
- `9976a35` - Add about page and localization
  - About page
  - Localization enhancement
- `0c8ccfc` - Enhance chat system, localization, and web views
  - Chat system improvements
  - Localization updates
  - Web view enhancement
- `a8f1342` - Redesign web communication layer, switch from WebSocket to SSE
  - Web communication now uses server-sent events

### 2026-04-11

- `e8fe259` - Add logging system and code optimization
  - Logging infrastructure
  - Logger implementation
- `f01c519` - Add logging system, update AI interface and web views
  - AI interface updates
  - Web view improvements

### 2026-04-10

- `4962924` - Enhance WebSocket handler, chat view, and messenger interaction
  - Context manager improvements
  - Chat system enhancement
  - Messenger provider interface updates
  - WebUI provider redesign
  - JavaScript builder and router updates
  - Chat view optimization
  - WebSocket handler improvements

### 2026-04-09

- `f9302bf` - Enhance messenger provider interface, chat system, and Web UI interaction
  - Messenger provider interface extension
  - Chat message and system improvements
  - Context manager optimization
  - Default silicon being enhancement
  - Web UI chat view improvements
  - WebSocket handler updates

### 2026-04-07

- `6831ee8` - Redesign web views and JavaScript builder
  - Complete web controller redesign
  - JavaScript builder complete rewrite
  - All view components updated (Being view, Chat view, Code Browser view, Config view, etc.)
  - Skin system improvements (Admin, Chat, Creative, Dev skins)
  - View base class architecture enhancement

### 2026-04-05

- `41e97fb` - Update multiple core modules and web controllers
  - Context manager improvements
  - Chat system and session management
  - Service locator redesign
  - Silicon being base class and manager updates
  - Web controllers comprehensive update (17 controllers)
  - Default silicon being factory improvements
- `67988d4` - Improve Web UI modules, add executor view, clean up views and core modules
  - Executor view
  - Module cleanup and organization

### 2026-04-04

- `b58bb1c` - Add initialization controller and redesign web modules
  - Initialization controller
  - Configuration module redesign
  - Localization module updates
  - Skin system improvements
  - Router enhancement
- `f03ac0b` - Add Web UI module, improve messenger functionality
  - Web UI module
  - Messenger functionality improvements

### 2026-04-03

- `192e57b` - Update project structure and core runtime components
  - Runtime system updates
  - Project structure improvement
- `59faec8` - Core and default implementation updates
  - Core module enhancement
  - Default implementation updates
- `d488485` - Add dynamic compilation functionality and curator tool module
  - Dynamic compilation executor
  - Curator tool implementation
- `753d1d9` - Add security module, update executors, messenger providers, localization, and tools
  - Security system
  - Executor updates
  - Messenger provider enhancement
  - Localization improvement
  - Tool system updates
- `a378697` - Complete Stage 5 - Tool system + Executors
  - Tool management and definition
  - Command line executor
  - Disk executor
  - Network executor
  - Tool implementations

### 2026-04-02

- `e6ad94b` - Fix chat history loading failure when deleting configuration file during testing
  - Ollama client error handling improvement
  - Configuration data validation
  - Project reference cleanup
- `daa56f5` - Complete Stage 4: Persistent memory (Chat system + Messenger channels)
  - Chat system with group and private chat
  - Messenger provider and manager interfaces
  - Time-indexed storage
  - Incomplete date handling
  - File system time storage
  - Console messenger provider
  - Silicon being factory improvements
  - Program initialization updates

### 2026-04-01

- `bbe2dbb` - Fix configuration loading and chat service message routing
  - Context manager implementation (added 188 lines of code)
  - AI client interface
  - Chat service interface and simple implementation
  - Configuration system with converters
  - Main loop scheduler improvements
  - Silicon being management system
  - Ollama client implementation
  - Localization system setup
  - Program initialization redesign
- `2fa6305` - Implement Stage 2: Main loop framework and clock object system
  - Main loop scheduler
  - Clock object base class
  - AI client factory interface
  - Storage interface
  - Ollama client factory
  - Console clock object
  - Test clock object
  - File system storage
- `32b99a1` - Implement Stage 1 - Basic chat functionality
  - AI request and response models
  - AI client interface
  - Message model
  - Localization system (Language, Localization base class, Localization manager)
  - Configuration data base class
  - Ollama client implementation
  - Configuration management
  - Multi-language localization (English, Simplified Chinese)
  - Program entry point
- `358e368` - Initial commit: project documentation and license
  - Project README (English and Chinese)
  - Architecture documentation
  - Roadmap documentation
  - Security documentation
  - License (Apache 2.0)
  - Git ignore configuration
