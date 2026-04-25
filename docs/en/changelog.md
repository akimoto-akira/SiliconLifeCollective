# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

**Note: This project has not yet released any official versions. All content below is under development.**

---

## About This Changelog

### Project Origin

- This project originated on March 20, 2026.
- Prior to this project, there was a validation Demo that failed due to unreasonable architecture design, which made it impossible to integrate with multiple AI platforms.

### AI IDE Tools Used

#### Kiro (Amazon AWS)
- The project was initially maintained by Kiro and launched using the Spec mode.
- Kiro is an agentic AI development environment built by Amazon AWS.
- Based on Code OSS (VS Code), supporting VS Code settings and Open VSX compatible plugins.
- Features spec-driven development workflow for structured AI coding.

#### Comate AI IDE / Wenxin Kuaima (Baidu)
- Occasionally used for copywriting and documentation work.
- Comate AI IDE is an AI-native development environment tool released by Baidu Wenxin on June 23, 2025.
- The industry's first multi-modal, multi-agent collaborative AI IDE.
- Features include design-to-code conversion and full-process AI-assisted coding.
- Powered by Baidu's Wenxin 4.0 X1 Turbo model.

#### Trae (ByteDance)
- This project was maintained primarily using Trae for the majority of the time.
- Trae is an AI IDE developed by ByteDance's Singapore-based subsidiary, SPRING PTE.
- Functions as a 10x AI Engineer capable of independently building software solutions.
- Features intelligent productivity tools, flexible development rhythm adaptation, and collaborative project delivery.
- Offers enterprise-grade performance with configurable agent systems.

#### Qoder (Alibaba)
- Since April 18, 2026, this project has been maintained using Qoder.
- Qoder excels at source code analysis and domain documentation generation, demonstrating exceptional capability in understanding complex codebases.
- Operates on a zero-compute-cost pricing model, making it highly cost-effective for automated documentation processing and routine task handling.
- An AI-powered agentic coding platform designed for real-world software development.
- Features intelligent code generation, conversation-based programming, advanced context analysis engine, and multi-agent collaboration.
- Provides deep code comprehension with minimal resource consumption, ideal for long-term project maintenance and knowledge accumulation.

### Requirements Documentation

- The requirements documentation for this project is not publicly disclosed.
- The requirements were repeatedly validated by over 12 international AI platforms and large model series, resulting in a user story-driven requirements document of over 2,000 lines that is almost incomprehensible to humans.

---

## [Unreleased]

### 2026-04-24

#### Memory Management Enhancement
- `c7b2ecc` - Enhance memory management with advanced filtering, statistics, and detail view
  - New advanced memory filtering feature with multi-dimensional filtering by type, time range, tags, etc.
  - Implemented memory statistics feature showing memory count, type distribution, and other statistics
  - Added memory detail view page for viewing complete information of individual memories
  - Optimized memory management interface for improved user experience and operational efficiency
  - Multi-language localization support (6 languages)

#### Permission System Extension
- `4489ad6` - Add wttr.in weather service to network whitelist
  - Allow silicon beings to access wttr.in weather API for weather information
  - Updated permission system documentation explaining weather service permission configuration
  - Complete multi-language documentation synchronization (6 languages)
- `fa3f06f` - Add timer execution history feature with detail view
  - New timer execution history view feature
  - Implemented execution history detail page
  - Support viewing detailed records of each timer execution
- `d824835` - Add timer execution history localization keys for all languages
  - Full language localization support for timer execution history (6 languages)
  - Includes localization for execution status, time, results, and other information

#### Chat History Optimization
- `057b09d` - Optimize chat history detail display with improved tool call rendering
  - Tool call display optimization in chat history detail page
  - Improved formatting display of tool call parameters
  - Enhanced readability of historical messages
- `0df599c` - Fix tool results being rendered as separate chat messages
  - Tool execution results now correctly associated with original messages
  - Prevent tool results from displaying as separate AI replies
  - Improve chat message coherence

#### Web Interface Fixes
- `d9d72e9` - Fix work note detail modal CSS priority issue
  - Work note modal style fix
  - CSS priority adjustment to ensure styles apply correctly
  - Improved modal visual effects

#### Core Functionality Improvements
- `1e7c7b2` - Improve memory compression and tool execution tracking
  - Memory compression algorithm optimization
  - Tool execution tracking mechanism enhancement
  - Improved silicon being memory management efficiency

#### Localization Enhancement
- `c13cb17` - Register Spanish language variants
  - Spanish (Spain) localization support
  - Multi-language system expansion
- `9c44f34` - Add Chinese Historical Calendar multi-language localization support
  - Full language localization for Chinese Historical Calendar
  - Multi-language support for historical era names, dynasty tables, and other information
- `192fc6e` - Add missing tool name localizations for 5 tools
  - Supplement tool localization name display
  - Improve tool interface multi-language experience

### 2026-04-23

#### Chat History & Loading Indicators
- `e483348` - Implement silicon being chat history view feature
  - New ChatHistoryController with conversation list and message detail APIs
  - Created ChatHistoryViewModel for data transfer
  - Implemented ChatHistoryListView and ChatHistoryDetailView pages
  - Added chat history localization keys (5 languages)
  - Updated Router to include chat history routes
  - Added chat history entry link in BeingView detail page
- `65c157b` - Add loading indicators for chat pages and auto-select curator session
  - Chat page loading status indicator
  - Auto-select curator session feature
  - Multi-language support (6 languages)

#### AI Stream Control Enhancement
- `30a2d4e` - Enhance AI stream cancellation, IM integration, and core host initialization
  - ContextManager stream cancellation mechanism enhancement
  - IM system integration improvement
  - CoreHost initialization optimization
  - DiskExecutor functionality enhancement
  - WebUIProvider update

#### File Upload Support
- `28fb344` - Implement file source dialog and file upload support
  - Web UI file source dialog
  - File upload functionality implementation
- `1d3e2cc` - Add file source dialog localization strings
  - File source dialog multi-language support (6 languages)

#### Chat Message Queue
- `db48c51` - Add chat message queue, file metadata, and stream cancellation support
  - New ChatMessageQueue chat message queue system
  - New FileMetadata file metadata management
  - New StreamCancellationManager stream cancellation manager

### 2026-04-22

#### Localization Enhancement
- `b574b2b` - Add senderName to history messages for AI identification
  - Added sender name field to SSE history messages
  - Support AI message identity identification
- `0a8d750` - Add common system prompt for proactive silicon being behavior
  - Added common system prompt template to localization system
  - Support proactive behavior guidance for silicon beings

#### Tool System Expansion
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
- `8f6cb1e` - Add beingId parameter to ILogger interface for system/silicon being log separation
  - ILogger interface extension
  - Support system log and silicon being log separation
  - New beingId parameter
- `2b771f3` - Decouple LogController from file I/O and add log reading API
  - LogController architecture refactoring
  - New independent log reading API
  - Separated file I/O operations
- `12da302` - Add silicon being filter to log view
  - Web UI added silicon being filter feature
  - Can filter log records by silicon being

#### Permission System Improvement
- `4c747ad` - Refactor PermissionTool, ExecuteCodeTool and add EvaluatePermission API
  - PermissionTool and ExecuteCodeTool refactoring
  - Integrated EvaluatePermission API

#### Web UI Optimization
- `702b3f3` - Enhance task view with status badges and metadata display
  - Task view UI improvement
  - New status badges and metadata display
- `6ed9a79` - Improve chat message storage and view rendering
  - Chat message storage mechanism optimization
  - View rendering performance improvement
- `0675c45` - Optimize markdown code block highlighting in preview pane
  - Markdown preview code highlighting optimization
  - Improved code block display effect

#### Tool Integration
- `135710d` - Remove SearchTool, move local search to DiskTool
  - SearchTool removal
  - Local search functionality integrated into DiskTool
- `7a03a19` - Improve LogTool conversation query flexibility
  - LogTool conversation query logic optimization
  - Enhanced query flexibility

#### Configuration Management
- `4305769` - Add .gitattributes for line ending management
  - Line ending configuration for cross-platform compatibility

#### Bug Fixes
- `1c96e99` - Fix search_files and search_content root directory search failure
  - DiskTool root directory search functionality fix
  - File and content search logic correction

### 2026-04-21

#### Permission System Enhancement
- `5879621` - Add permission callback pre-compilation validation and enhanced error handling
  - Permission callback pre-compilation validation feature
  - Compile permission callback code before saving to verify correctness
  - Prevent invalid code from being saved to disk
  - Detailed error handling for permission save operations
  - Enhanced error messages with localization support
  - Separated compilation and security scanning steps for better error reporting
- `833ead2` - Add assembly reference validation for dynamic compilation
  - Dynamic compilation security enhancement
  - Assembly reference validation mechanism

#### Web UI Improvement
- `0a826f5` - Add save success alert in code editor
  - Code editor save operation success alert feature
  - Improved user operation feedback
- `2940373` - Enhance web interface with code hover hints and UI improvements
  - Code editor hover hint feature
  - Web interface UI optimization
- `6ba591d` - Add independent AI configuration editor for silicon beings
- `634e8ca` - Add back to list link in permission page
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
- `4ff98ad` - Restructure documentation with multi-language support
  - Documentation structure reorganization
  - Multi-language documentation synchronization

#### AI & Calendar
- `646813e` - Improve AI client factory implementation
  - AI client factory refactoring
  - Client discovery mechanism optimization
- `928a96d` - Fix calendar calculation implementations
  - Calendar calculation logic correction
  - Improved accuracy for multiple calendar types

#### Configuration & Entry
- `0fc1693` - Update program entry and project configuration
  - Program entry point optimization
  - Project configuration improvement

### 2026-04-20

#### Core Feature Completion
- `28905b5` - Complete multi-language support, AI client factory, permission system and localization configuration
  - Log system with manager, entries and different log levels
  - Token audit system for querying and tracking token usage
  - AI client factory for automatic discovery of different AI platforms
  - Permission callback system with its own storage
  - Console logger implementation
  - Multi-language support for English and Simplified Chinese
  - WebUI messenger with WebSocket for real-time chat
  - Enhanced default silicon being with localization

### 2026-04-19

#### Timer & Calendar
- `c933fd8` - Update localization, timer system, web views and add new tools
  - Better localization manager
  - Scheduling system for timed tasks
  - AI configuration and context management
  - Calendar tool supporting 32 calendar types
  - Web controller for calendar API
  - Task management tool

**Architecture Improvement**
- Redesigned web view architecture to better support skins
- Improved being management system with better state handling

### 2026-04-18

- `9f585e1` - Update localization, timer system, web views and add new tools
  - Timer and scheduling improvements
  - Better web views with improved UI components
  - More tool implementations

### 2026-04-17

- `9b71fcd` - Update core modules, add zh-HK docs, broadcast channel, config tool and audit web views
  - Broadcast channel for multiple silicon beings to chat together
  - Configuration tool system
  - Audit web views
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

- `4241a2f` - Chat functionality basically complete, UI upload optimization
  - Chat system functionality completion
  - UI optimization for file uploads

### 2026-04-13

- `c498c31` - Code update
  - General code improvements and optimization

### 2026-04-12

- `2161002` - Restructure documentation and enhance localization
  - Documentation reorganization
  - Localization system improvement
- `03d94e4` - Enhance configuration system and localization
  - Configuration system improvement
  - Additional language support
- `9976a35` - Add about page and localization
  - About page
  - Localization enhancement
- `0c8ccfc` - Enhance chat system, localization and web views
  - Chat system improvements
  - Localization updates
  - Web view enhancement
- `a8f1342` - Redesign web communication layer, switch from WebSocket to SSE
  - Web communication now uses Server-Sent Events

### 2026-04-11

- `e8fe259` - Add logging system and code optimization
  - Logging infrastructure
  - Logger implementation
- `f01c519` - Add logging system, update AI interface and web views
  - AI interface updates
  - Web view improvements

### 2026-04-10

- `4962924` - Enhance WebSocket handler, chat views and messenger interaction
  - Context manager improvements
  - Chat system enhancement
  - Messenger provider interface updates
  - WebUI provider redesign
  - JavaScript builder and router updates
  - Chat view optimization
  - WebSocket handler improvements

### 2026-04-09

- `f9302bf` - Enhance messenger provider interface, chat system and Web UI interaction
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
  - All view component updates (being views, chat views, code browser views, config views, etc.)
  - Skin system improvements (Admin, Chat, Creative, Dev skins)
  - View base class architecture enhancement

### 2026-04-05

- `41e97fb` - Update multiple core modules and web controllers
  - Context manager improvements
  - Chat system and session management
  - Service locator redesign
  - Silicon being base class and manager updates
  - Comprehensive web controller updates (17 controllers)
  - Default silicon being factory improvement
- `67988d4` - Improve Web UI module, add executor view, clean up views and core modules
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
- `753d1d9` - Add security module, update executors, messenger providers, localization and tools
  - Security system
  - Executor updates
  - Messenger provider enhancement
  - Localization improvements
  - Tool system updates
- `a378697` - Complete Phase 5 - Tool System + Executors
  - Tool management and definition
  - Command line executor
  - Disk executor
  - Network executor
  - Tool implementations

### 2026-04-02

- `e6ad94b` - Fix chat history loading failure when deleting config file during testing
  - Ollama client error handling improvement
  - Configuration data validation
  - Project reference cleanup
- `daa56f5` - Complete Phase 4: Persistent Memory (Chat System + Messenger Channel)
  - Chat system with group chat and private chat
  - Messenger provider and manager interfaces
  - Time-indexed storage
  - Incomplete date handling
  - File system time storage
  - Console messenger provider
  - Silicon being factory improvement
  - Program initialization updates

### 2026-04-01

- `bbe2dbb` - Fix config loading and chat service message routing
  - Context manager implementation (added 188 lines of code)
  - AI client interface
  - Chat service interface and simple implementation
  - Configuration system with converters
  - Main loop scheduler improvements
  - Silicon being management system
  - Ollama client implementation
  - Localization system setup
  - Program initialization redesign
- `2fa6305` - Implement Phase 2: Main Loop Framework and Tick Object System
  - Main loop scheduler
  - Tick object base class
  - AI client factory interface
  - Storage interface
  - Ollama client factory
  - Console tick object
  - Test tick object
  - File system storage
- `32b99a1` - Implement Phase 1 - Basic Chat Functionality
  - AI request and response models
  - AI client interface
  - Message model
  - Localization system (language, localization base class, localization manager)
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
