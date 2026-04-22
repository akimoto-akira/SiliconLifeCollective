# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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

## Git History


## [Unreleased]

### Added
- `6ba591d` - Add independent AI configuration editor for silicon beings
  - Added BeingAIConfigViewModel view model
  - Added BeingAIConfigView view component
  - Support configuring independent AI client type and parameters for each silicon being
  - Support switching between global and independent configuration
  - Support dynamic loading of AI model lists (e.g., DashScope)
  - Added AI configuration related API routes in BeingController
  - Optimized AI client type resolution logic in DefaultSiliconBeing
  - Updated AI configuration edit link in BeingView
- `634e8ca` - Add back to list link in permission page
  - Navigation improvement for permission management
- `188c6f8` - Register task list API route and add empty state display
  - Task list API route registration
  - Empty state UI improvement

### Changed
- `4305769` - Add .gitattributes for line ending management
  - Line ending configuration for cross-platform compatibility

### Fixed
- `c6b518b` - Fix timer message delivery and chat message storage
  - Timer message delivery mechanism correction
  - Chat message storage optimization

---

## [0.5.2] - 2026-04-21

### Added
- `0a826f5` - Add save success alert in code editor
  - Save operation success notification for code editor
  - Improved user operation feedback
- `833ead2` - Add assembly reference validation for dynamic compilation
  - Dynamic compilation security enhancement
  - Assembly reference validation mechanism
- `5879621` - Add permission callback pre-compilation validation and enhanced error handling
  - Permission callback pre-compilation validation feature
  - Compile permission callback code before saving to validate correctness
  - Prevent invalid code from being saved to disk
  - Detailed error handling for permission save operations
  - Enhanced error messages with localization support
  - Separate compilation and security scan steps for better error reporting
- JsBuilder error handling improvements in CodeEditorView and MarkdownEditorView
  - Enhanced client-side error display with detailed error messages
  - Better HTTP error handling and status code reporting
- Permission controller API route registration
  - Added `/api/permissions/list` route
  - Added `/api/permissions/save` POST route

### Changed
- Permission save workflow refactoring
  - Separated compilation validation from file saving
  - Improved security scan integration
  - Enhanced localization support for all error messages
- CodeEditorView and MarkdownEditorView save response handling
  - More detailed success/error feedback
  - Improved alert messages with error details

### Fixed
- `592c7ab` - Fix callback instantiation and registration order
  - Permission callback system fix
  - Callback registration order optimization
- Missing permission controller API routes
- Inconsistent error message handling in permission save operations

---

## [0.5.1] - 2026-04-21

### Added
- `0fc1693` - Update program entry and project configuration
  - Program entry point optimization
  - Project configuration improvements
- `2940373` - Enhance web interface with code hover hints and UI improvements
  - Code editor hover hint feature
  - Web interface UI optimization
- `7940d9c` - Add Korean localization support
  - Korean localization files
  - Multi-language system enhancements
- `4ff98ad` - Restructure documentation with multi-language support
  - Documentation structure reorganization
  - Multi-language documentation synchronization

### Changed
- `ea9179a` - Improve permission system implementation
  - Permission system refactoring
  - Permission validation logic optimization
- `646813e` - Improve AI client factory implementation
  - AI client factory refactoring
  - Client discovery mechanism optimization

### Fixed
- `928a96d` - Fix calendar calculation implementations
  - Calendar calculation logic correction
  - Improved accuracy for multiple calendar types
- Full multi-language support - you can now use English, Simplified Chinese, Traditional Chinese, Japanese, and Korean
- Token usage tracking and querying - keeps track of how much each AI call costs
- AI client factory - automatically finds and sets up different AI platforms
- Security system with 5-layer permission checks to protect all sensitive operations
- Web interface with 4 skin themes and 17 functional pages
- Calendar system supporting 32 different international calendar types
- Memory system that stores chat logs with time indexing
- Broadcast channel - multiple silicon beings can chat together
- Config tool for dynamically managing system settings

### Changed
- Reworked the permission callback system with its own storage layer
- Boosted AI request structure to support scheduled thinking
- Improved config metadata and dynamic UI controls
- Made frontend interactions more reliable and polished the JavaScript
- Switched Web communication from WebSocket to SSE

### Fixed
- Fixed the issue where chat history wouldn't load after deleting the config file
- Fixed message routing problems in the chat service

---

## [0.5.0] - 2026-04-20

### Added
- `28905b5` - Complete multi-language support, AI client factory, permission system, and localization setup
  - Logging system with manager, entries, and different log levels
  - Token audit system to query and track token usage
  - AI client factory that auto-discovers different AI platforms
  - Permission callback system with its own storage
  - Console logger implementation
  - Multi-language support for English and Simplified Chinese
  - WebUI messenger with WebSocket for real-time chat
  - Enhanced the default silicon being with localization

---

## [0.4.0] - 2026-04-19

### Added
- `c933fd8` - Update localization, timer system, Web views, and add new tools
  - Better localization manager
  - Scheduling system for timed tasks
  - AI config and context management
  - Calendar tool supporting 32 calendar types
  - Web controller for calendar API
  - Task management tools

### Changed
- Reworked Web view architecture for better skin support
- Improved being management system with better state handling

---

## [0.3.5] - 2026-04-18

### Added
- `9f585e1` - Update localization, timer system, web views and add new tools
  - Timer and scheduling improvements
  - Better Web views with improved UI components
  - More tool implementations

---

## [0.3.4] - 2026-04-17

### Added
- `9b71fcd` - Update core modules, add zh-HK docs, broadcast channel, config tool and audit web views
  - Broadcast channel for multiple silicon beings to chat together
  - Config tool system
  - Audit web views
  - Traditional Chinese documentation

---

## [0.3.3] - 2026-04-16

### Changed
- `5040f05` - Update core and default modules
  - Module optimizations and bug fixes
  - Implementation updates and improvements

---

## [0.3.2] - 2026-04-15

### Changed
- `3efab5f` - Update multiple modules: AI, Chat, IM, Tools, Web, Localization, Storage
  - AI client improvements
  - Chat system enhancements
  - Messenger provider updates
  - Tool system optimizations
  - Web infrastructure improvements
  - Localization refinements
  - Storage system updates

---

## [0.3.1] - 2026-04-14

### Added
- `4241a2f` - Chat functionality basically done, UI upload optimization
  - Chat system feature complete
  - UI optimization for file uploads

---

## [0.3.0] - 2026-04-13

### Changed
- `c498c31` - Code update
  - General code improvements and optimizations

---

## [0.2.5] - 2026-04-12

### Added
- `2161002` - Restructure docs and enhance localization
  - Documentation reorganization
  - Localization system improvements
- `03d94e4` - Enhance config system and localization
  - Config system improvements
  - Extra language support
- `9976a35` - Add About page and localization
  - About page
  - Localization enhancements
- `0c8ccfc` - Enhance chat system, localization and Web views
  - Chat system improvements
  - Localization updates
  - Web view enhancements

### Changed
- `a8f1342` - Rework Web communication layer, switched from WebSocket to SSE
  - Web communication now uses Server-Sent Events

---

## [0.2.4] - 2026-04-11

### Added
- `e8fe259` - Add logging system and code optimization
  - Logging infrastructure
  - Logger implementation
- `f01c519` - Add logging system, update AI interface and Web views
  - AI interface updates
  - Web view improvements

---

## [0.2.3] - 2026-04-10

### Changed
- `4962924` - Enhance WebSocket handler, chat view, and messenger interactions
  - Context manager improvements
  - Chat system enhancements
  - Messenger provider interface updates
  - WebUI provider rework
  - JavaScript builder and router updates
  - Chat view optimizations
  - WebSocket handler improvements

---

## [0.2.2] - 2026-04-09

### Changed
- `f9302bf` - Enhance messenger provider interface, chat system, and Web UI interactions
  - Messenger provider interface expansion
  - Chat message and system improvements
  - Context manager optimizations
  - Default silicon being enhancements
  - Web UI chat view improvements
  - WebSocket handler updates

---

## [0.2.1] - 2026-04-07

### Changed
- `6831ee8` - Rework Web views and JavaScript builder
  - Complete Web controller rework
  - JavaScript builder full rewrite
  - All view components updated (being view, chat view, code browser view, config view, etc.)
  - Skin system improvements (admin, chat, creative, dev skins)
  - View base class architecture boost

---

## [0.2.0] - 2026-04-05

### Added
- `41e97fb` - Update multiple core modules and Web controllers
  - Context manager improvements
  - Chat system and session management
  - Service locator rework
  - Silicon being base and manager updates
  - Web controllers comprehensive update (17 controllers)
  - Default silicon being factory improvements
- `67988d4` - Improve Web UI modules, add executor view, clean up views and core modules
  - Executor view
  - Module cleanup and organization

---

## [0.1.5] - 2026-04-04

### Added
- `b58bb1c` - Add init controller and rework Web modules (config, localization, controllers, skins, router)
  - Init controller
  - Config module rework
  - Localization module updates
  - Skin system improvements
  - Router enhancements
- `f03ac0b` - Add Web UI module, improve messenger features
  - Web UI module
  - Messenger feature improvements

---

## [0.1.4] - 2026-04-03

### Added
- `192e57b` - Update project structure and core runtime components
  - Runtime system updates
  - Project structure improvements
- `59faec8` - Core and default implementation updates
  - Core module enhancements
  - Default implementation updates
- `d488485` - Add dynamic compilation feature and curator tool module
  - Dynamic compilation executor
  - Curator tool implementation
- `753d1d9` - Add security module, update executors, messenger provider, localization and tools
  - Security system
  - Executor updates
  - Messenger provider enhancements
  - Localization improvements
  - Tool system updates
- `a378697` - Complete Phase 5 - Tool System + Executors
  - Tool manager and definitions
  - Command line executor
  - Disk executor
  - Network executor
  - Tool implementations

---

## [0.1.3] - 2026-04-02

### Fixed
- `e6ad94b` - Fixed chat history loading failure when config file got deleted during testing, which broke the AI model loading
  - Ollama client error handling improvements
  - Config data validation
  - Project reference cleanup

### Added
- `daa56f5` - Complete Phase 4: Persistent Memory (Chat System + Messenger Channel)
  - Chat system with group and private conversations
  - Messenger provider and manager interfaces
  - Time-indexed storage
  - Incomplete date handling
  - File system time storage
  - Console messenger provider
  - Silicon being factory improvements
  - Program init updates

---

## [0.1.2] - 2026-04-01

### Fixed
- `bbe2dbb` - Fixed config loading and chat service message routing
  - Context manager implementation (added 188 lines of code)
  - AI client interface
  - Chat service interfaces and simple implementation
  - Config system with converters
  - Main loop scheduler improvements
  - Silicon being management system
  - Ollama client implementation
  - Localization system setup
  - Program init rework

---

## [0.1.1] - 2026-04-01

### Added
- `2fa6305` - Implement Phase 2: Main Loop Framework and Tick Object System
  - Main loop scheduler
  - Tick object base class
  - AI client factory interface
  - Storage interface
  - Ollama client factory
  - Console tick object
  - Test tick object
  - File system storage

---

## [0.1.0] - 2026-04-01

### Added
- `32b99a1` - Implement Phase 1 - Basic Chat Capability
  - AI request and response models
  - AI client interface
  - Message model
  - Localization system (language, localization base, localization manager)
  - Config data base
  - Ollama client implementation
  - Config management
  - Multi-language localization (English, Simplified Chinese)
  - Program entry point

---

## [0.0.1] - 2026-04-01

### Added
- `358e368` - Initial commit: project docs and license
  - Project README (English and Chinese)
  - Architecture docs
  - Roadmap docs
  - Security docs
  - License (Apache 2.0)
  - Git ignore config
