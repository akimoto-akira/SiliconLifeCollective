# Web UI Guide

> **Version: v0.2.0-alpha**

[**English**](../en/web-ui-guide.md) | [Deutsch](../de-DE/web-ui-guide.md) | [中文](../zh-CN/web-ui-guide.md) | [繁體中文](../zh-HK/web-ui-guide.md) | [Español](../es-ES/web-ui-guide.md) | [日本語](../ja-JP/web-ui-guide.md) | [한국어](../ko-KR/web-ui-guide.md) | [Čeština](../cs-CZ/web-ui-guide.md) | [Русский](../ru-RU/web-ui-guide.md)

## Overview

The Web UI provides a comprehensive interface for managing Silicon Beings, monitoring system status, and interacting with AI agents. The system uses a pure server-side rendering architecture with zero frontend framework dependencies, generating HTML, CSS, and JavaScript through `H`, `CssBuilder`, and `JsBuilder` builders.

## Access

Default URL: `http://localhost:8080`

## Navigation

### Main Sections

1. **Dashboard** - System overview and metrics
2. **Beings** - Manage Silicon Beings
3. **Chat** - Interact with beings (supports file upload, real-time SSE)
4. **Chat History** - View chat history of Silicon Beings (conversation list, message details)
5. **Tasks** - Task management (personal tasks)
6. **Timers** - Timer configuration (create, pause, execution history)
7. **Configuration** - System settings (AI clients, localization)
8. **Permissions** - Access control (ACL management, permission queries)
9. **Logs** - System logs (filter by level, time range queries)
10. **Audit** - Token usage and audit trail
11. **Memory** - Being memory (timeline view, advanced filtering)
12. **Knowledge** - Knowledge base (triplet management, path discovery)
13. **Code Browser** - Code exploration (file tree, syntax highlighting)
14. **Code Editor** - Code editing with hover tooltips (Monaco Editor)
15. **Projects** - Project management (workspaces, tasks, work notes)
16. **Executors** - Executor management (disk, network, command line)
17. **Help** - Help documentation system (multi-language support, topic search)
18. **About** - System information and version

---

## Dashboard

### Features

- System performance metrics (CPU, memory, uptime)
- Being status overview
- AI usage statistics
- Quick actions

### Real-time Updates

Uses SSE (Server-Sent Events) for real-time data:

```javascript
const dashboard = new EventSource('/api/dashboard/events');
dashboard.onmessage = (event) => {
    const data = JSON.parse(event.data);
    updateMetrics(data);
};
```

---

## Being Management

### Being List

Displays all beings, including:
- Name and ID
- Current status (Running/Stopped/Error)
- Soul File link
- Quick actions (Start/Stop/Configure)

### Being Details

- Full configuration
- Soul File editor
- Task history
- Memory viewer
- Performance metrics

### Create a Being

1. Click **Create New Being**
2. Fill in:
   - Name
   - Soul content (Markdown editor)
   - Initial configuration
3. Click **Create**

---

## Chat Interface

### Features

- Real-time message streaming
- Message history
- Multi-session support
- Tool Call visualization

### Using Chat

1. Select a being
2. Enter a message
3. View streaming response
4. Watch tool execution in real time

### Tool Call Display

When the AI calls a tool:
```
🔧 Tool: calendar
📥 Input: {"date": "2026-04-20"}
📤 Output: "Lunar calendar: third day of the fourth month"
```

---

## Configuration

### AI Clients

Configure AI backends:
- Ollama (local)
- Bailian (cloud)
- Volcano Engine Ark (cloud)
- Custom clients

### Storage Settings

- Default version: base path, time index, cleanup policy
- Fast version: SpeedyPack Storage Engine configuration, .spk file management, auto-compaction settings

### Localization

Switch between 34 language variants:
- Chinese (6): Simplified Chinese, Traditional Chinese, Singapore Chinese, Macau Chinese, Taiwan Chinese, Malaysian Chinese
- English (10): American, British, Canadian, Australian, Indian, Singaporean, South African, Irish, New Zealand, Malaysian English
- Spanish (2): Spain, Mexico
- German (5): Germany, Austria, Switzerland, Luxembourg, Liechtenstein
- French (3): France, Canada, Switzerland
- Japanese, Korean, Czech
- Russian, Portuguese (2), Italian, Dutch, Polish, Swedish

---

## Skin System

### Available Skins

1. **Admin** - Professional management interface
2. **Chat** - Conversation-centric design
3. **Creative** - Creative and artistic style
4. **Dev** - Developer-oriented layout
5. **HighContrast** - High contrast theme (Fast version)
6. **Minimal** - Minimalist style (Fast version)
7. **Light** - Light theme (Fast version)

### Switching Skins

1. Click **Settings** (gear icon)
2. Select **Skin**
3. Choose the desired skin
4. The interface updates immediately

### Custom Skins

Create custom skins by implementing `ISkin`:

```csharp
public class MySkin : ISkin
{
    public string Name => "MySkin";
    
    public string GetCss()
    {
        return ":root { --primary: #color; }";
    }
}
```

---

## Permission Management

### View Permissions

- List all permission rules
- Filter by user or resource
- View expiration dates

### Add Permission Rule

1. Click **Add Rule**
2. Configure:
   - Permission Type (e.g., `FileAccess`, `NetworkAccess`)
   - Resource prefix (e.g., `C:\Projects`, `api.github.com`)
   - Allow/Deny
   - Description
3. Save

### Audit Trail

View all permission decisions:
- Timestamp
- User
- Resource
- Decision
- Reason

### Tool Permission Management

Manage tool operation permissions for Silicon Beings and projects:

1. **Silicon Being Tool Permissions**:
   - Navigate to **Beings** → Select a being → **Tool Permissions**
   - View current permission configuration
   - Set allow/deny per operation
   - Apply permission templates (readonly/restricted/full)

2. **Project Tool Permissions**:
   - Navigate to **Projects** → Select a project → **Tool Permissions**
   - Project-level tool permissions are independent of Silicon Being level
   - Implement permission isolation across projects

---

## Task Management

### Task List

- All tasks and their statuses
- Filter by being or status
- Priority indicators

### Task Details

- Description
- Priority
- Due date
- Execution history
- Result output

### Create Task

1. Click **Create Task**
2. Fill in:
   - Being assignment
   - Description
   - Priority (1-10)
   - Due date
3. Create

---

## Timer Management

### Active Timers

- List of running timers
- Next execution time
- Repeat status

### Create Timer

1. Click **Create Timer**
2. Configure:
   - Being assignment
   - Interval or cron expression
   - Action to execute
   - Repeat settings
3. Start

---

## Log Viewer

### Features

- Filter by level (Info/Warning/Error)
- Search by keyword
- Time range selection
- Real-time updates

### Log Details

Each log entry shows:
- Timestamp
- Level
- Source
- Message
- Stack trace (for errors)

---

## Audit Reports

### Token Usage

- Total tokens used
- Breakdown by model
- Cost calculation
- Time-based charts

### Export Reports

Download audit data:
- CSV format
- Date range selection
- Filter by being or model

---

## Code Editor

### Features

- Syntax highlighting (Monaco Editor)
- Code completion
- Hover tooltips for identifiers
- Live compilation

### Hover Tooltips

Hover over any identifier to view:
- Type information
- Documentation
- Definition location
- References

---

## Chat History Viewer

### Features

- Browse Silicon Being chat history
- Conversation list display
- Message detail viewing
- Timeline view

### Using Chat History

1. Navigate to the **Beings** page
2. Click the **Chat History** link for a Silicon Being
3. View the conversation list:
   - Conversation title
   - Creation time
   - Message count
4. Click a conversation to view details:
   - Full message history
   - Timestamps
   - Sender information
   - Tool Call records

### Technical Implementation

- **Controller**: `ChatHistoryController`
- **View Model**: `ChatHistoryViewModel`
- **Views**:
  - `ChatHistoryListView` - Conversation list
  - `ChatHistoryDetailView` - Message details
- **API Routes**:
  - `/api/chat-history/{beingId}/conversations` - Get conversation list
  - `/api/chat-history/{beingId}/conversation/{conversationId}` - Get message details

---

## File Upload

### Features

- File source dialog
- Multi-file upload support
- File metadata management
- Upload progress display

### Using File Upload

1. Click the **Upload File** button in the chat interface
2. The file source dialog opens
3. Select a file source:
   - Local file
   - File system path
4. Select files (multi-select supported)
5. Confirm upload
6. File information will be attached to the message

### Supported File Types

- Text files (.txt, .md, .json, .xml, etc.)
- Code files (.cs, .js, .py, .java, etc.)
- Configuration files (.yml, .yaml, .ini, .conf, etc.)
- Document files (.csv, .log, etc.)

---

## Loading Indicator

### Features

- Chat page loading status display
- Silicon Curator session auto-selection
- Data loading progress feedback

### Behavior

- Loading animation displayed on page load
- Automatically hidden when data loading completes
- Silicon Curator session auto-selected (if it exists)
- Multi-language loading tip text

---

## Help Documentation System (New)

### Feature Overview

The help documentation system provides multi-language help documentation support for Silicon Beings and users.

### Using Help Documentation

1. Navigate to the **Help** page
2. View the list of help topics:
   - Quick start guide
   - Tool usage reference
   - Permission management guide
   - Troubleshooting manual
   - Development guide
3. Click a topic to view detailed content:
   - Structured documentation content (Markdown rendered)
   - Multi-language support (follows system localization settings)
   - Related topic recommendations
4. Use the search function to quickly locate:
   - Keyword search (supports Chinese and English)
   - Search results sorted by relevance

### Silicon Being Access to Help

Silicon Beings can access help documentation through the `help` tool:
```json
{
  "action": "get_topics"
}
```

### Technical Implementation

- **Controller**: `HelpController`
- **Tool**: `HelpTool`
- **API Routes**:
  - `/api/help` - Get help topic list
  - `/api/help/{topicId}` - Get topic details
  - `/api/help/search?q=keyword` - Search help documentation

---

## Project Workspace (New)

### Feature Overview

The project workspace provides a structured work environment that supports project management, task tracking, and work notes.

### Project Management

1. **Create Project**:
   - Project name and description
   - Project tags (categories)
   - Project status (In Progress, Completed, Archived)
2. **View Project Details**:
   - Project basic information
   - Associated task list
   - Work note list
   - Project progress statistics
3. **Archive Project**: Retain historical data but no longer active
4. **Project Role Management**:
   - Assign project roles to Silicon Beings (e.g., developer, reviewer, manager)
   - Remove role assignments
   - View project members and role list
5. **Project Workflow**:
   - View workflow template list
   - Bind workflow templates to projects
   - View workflow instance status
   - View workflow execution logs

### Work Notes (Private)

Personal work notes for Silicon Beings, similar to a journal:

1. **Create Note**:
   - Summary (brief description)
   - Content (supports Markdown format)
   - Keywords (for searching)
   - Automatic timestamp recording
2. **Manage Notes**:
   - Browse by timeline (paginated design)
   - Search notes (by keyword, summary, content)
   - Generate table of contents (quick overview of note structure)
   - Update and delete notes
3. **Permission Control**:
   - Private by default, only accessible by the being itself
   - Silicon Curator can manage all notes

### Technical Implementation

- **Controller**: `WorkNoteController`
- **Tools**: `WorkNoteTool`, `ProjectTool`, `ProjectWorkNoteTool`
- **API Routes**:
  - `/api/worknotes` - Get work note list
  - `/api/worknotes/{id}` - Get note details
  - `/api/worknotes/search?q=keyword` - Search notes
  - `/api/worknotes/directory` - Generate note table of contents
  - `/api/projects` - Project management API

---

## Responsive Design

The Web UI adapts to different screen sizes:
- Desktop: Full layout
- Tablet: Collapsed sidebar
- Mobile: Collapsible menu

---

## Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| `Ctrl+K` | Quick search |
| `Ctrl+B` | Toggle sidebar |
| `Ctrl+Enter` | Send message |
| `Esc` | Cancel/Close |

---

## Troubleshooting

### Cannot Connect

**Check**:
- Server is running
- Port 8080 is not blocked
- Firewall settings

### SSE Not Working

**Check**:
- Browser supports SSE
- No proxy buffering SSE
- Network stability

### Slow Performance

**Optimize**:
- Reduce log verbosity
- Clean up old audit data
- Check system resources

---

## Next Steps

- 📚 Read the [Architecture Guide](architecture.md)
- 🛠️ Check the [Development Guide](development-guide.md)
- 📖 Explore the [API Reference](api-reference.md)
- 🚀 See the [Getting Started Guide](getting-started.md)
