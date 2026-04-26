# Web UI Guide

[English](../en/web-ui-guide.md) | [中文](../zh-CN/web-ui-guide.md) | [繁體中文](../zh-HK/web-ui-guide.md) | [Español](../es-ES/web-ui-guide.md) | [日本語](../ja-JP/web-ui-guide.md) | [한국어](../ko-KR/web-ui-guide.md) | [Deutsch](../de-DE/web-ui-guide.md) | [Čeština](../cs-CZ/web-ui-guide.md)

## Overview

The Web UI provides a comprehensive interface for managing silicon beings, monitoring system status, and interacting with AI agents. The system uses a pure server-side rendering architecture with zero frontend framework dependencies, generating HTML, CSS, and JavaScript through `H`, `CssBuilder`, and `JsBuilder` builders.

## Access

Default URL: `http://localhost:8080`

## Navigation

### Main Sections

1. **Dashboard** - System overview and metrics
2. **Beings** - Manage silicon beings
3. **Chat** - Interact with beings (supports file upload, real-time SSE)
4. **Chat History** - View silicon being chat history (session list, message details)
5. **Tasks** - Task management (personal tasks)
6. **Timers** - Timer configuration (create, pause, execution history)
7. **Config** - System settings (AI clients, localization)
8. **Permissions** - Access control (ACL management, permission queries)
9. **Logs** - System logs (filter by level, time range queries)
10. **Audit** - Token usage and audit trail
11. **Memory** - Being memory (timeline view, advanced filtering)
12. **Knowledge** - Knowledge base (triple management, path discovery)
13. **Code Browser** - Code exploration (file tree, syntax highlighting)
14. **Code Editor** - Code editing with hover hints (Monaco Editor)
15. **Projects** - Project management (workspace, tasks, work notes)
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

Uses SSE (Server-Sent Events) for live data:

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

Displays all beings with:
- Name and ID
- Current status (Running/Stopped/Error)
- Soul file link
- Quick actions (Start/Stop/Configure)

### Being Details

- Full configuration
- Soul file editor
- Task history
- Memory viewer
- Performance metrics

### Create Being

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
- Tool call visualization

### Using Chat

1. Select a being
2. Enter message
3. View streaming response
4. See tool execution in real-time

### Tool Call Display

When AI calls a tool:
```
🔧 Tool: calendar
📥 Input: {"date": "2026-04-20"}
📤 Output: "Lunar calendar fourth month, third day"
```

---

## Configuration

### AI Clients

Configure AI backends:
- Ollama (local)
- DashScope (cloud)
- Custom clients

### Storage Settings

- Base path
- Time indexing
- Cleanup strategy

### Localization

Switch between 21 language variants:
- Chinese (6): Simplified, Traditional, Singapore, Macau, Taiwan, Malaysia
- English (10): US, UK, Canadian, Australian, Indian, Singapore, South African, Irish, New Zealand, Malaysian
- Spanish (2): Spain, Mexico
- Japanese, Korean, Czech

---

## Skin System

### Available Skins

1. **Admin** - Professional administration interface
2. **Chat** - Conversation-centric design
3. **Creative** - Creative and artistic style
4. **Dev** - Developer-oriented layout

### Switching Skins

1. Click **Settings** (gear icon)
2. Select **Skin**
3. Choose desired skin
4. UI updates immediately

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
   - User
   - Resource (e.g., `disk:read`)
   - Allow/Deny
   - Duration
3. Save

### Audit Trail

View all permission decisions:
- Timestamp
- User
- Resource
- Decision
- Reason

---

## Task Management

### Task List

- All tasks with their status
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
- Hover hints for identifiers
- Live compilation

### Hover Hints

Hover over any identifier to see:
- Type information
- Documentation
- Definition location
- References

---

## Chat History Viewing

### Features

- Silicon being chat history browsing
- Session list display
- Message detail viewing
- Timeline view

### Using Chat History

1. Navigate to **Beings** page
2. Click **Chat History** link for a silicon being
3. View session list:
   - Session title
   - Creation time
   - Message count
4. Click session to view details:
   - Complete message history
   - Timestamps
   - Sender information
   - Tool call records

### Technical Implementation

- **Controller**: `ChatHistoryController`
- **ViewModel**: `ChatHistoryViewModel`
- **Views**:
  - `ChatHistoryListView` - Session list
  - `ChatHistoryDetailView` - Message details
- **API Routes**:
  - `/api/chat-history/{beingId}/conversations` - Get session list
  - `/api/chat-history/{beingId}/conversation/{conversationId}` - Get message details

---

## File Upload

### Features

- File source dialog
- Multi-file upload support
- File metadata management
- Upload progress display

### Using File Upload

1. Click **Upload File** button in chat interface
2. File source dialog opens
3. Select file source:
   - Local files
   - Filesystem path
4. Select files (multi-select supported)
5. Confirm upload
6. File information will be attached to message

### Supported File Types

- Text files (.txt, .md, .json, .xml, etc.)
- Code files (.cs, .js, .py, .java, etc.)
- Configuration files (.yml, .yaml, .ini, .conf, etc.)
- Document files (.csv, .log, etc.)

---

## Loading Indicator

### Features

- Chat page loading status display
- Auto-selection of curator session
- Data loading progress feedback

### Behavior

- Shows loading animation when page loads
- Automatically hides after data loading completes
- Curator session auto-selected (if exists)
- Multi-language loading prompt text

---

## Help Documentation System (New)

### Feature Overview

The help documentation system provides multi-language help documentation support for silicon beings and users.

### Using Help Documentation

1. Navigate to **Help** page
2. View list of help topics:
   - Getting Started Guide
   - Tool Usage Reference
   - Permission Management Guide
   - Troubleshooting Manual
   - Development Guide
3. Click topic to view detailed content:
   - Structured document content (Markdown rendering)
   - Multi-language support (follows system localization settings)
   - Related topic recommendations
4. Use search function for quick location:
   - Keyword search (supports Chinese, English)
   - Search results sorted by relevance

### Silicon Being Access to Help

Silicon beings can access help documentation through the `help` tool:
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

The project workspace provides a structured working environment supporting project management, task tracking, and work notes.

### Project Management

1. **Create Project**:
   - Project name and description
   - Project tags (categorization)
   - Project status (Active, Completed, Archived)
2. **View Project Details**:
   - Basic project information
   - Associated task list
   - Work note list
   - Project progress statistics
3. **Archive Project**: Retains historical data but no longer active

### Work Notes (Private)

Personal work notes for silicon beings, similar to a diary:

1. **Create Note**:
   - Summary (brief description)
   - Content (supports Markdown format)
   - Keywords (for search)
   - Automatic timestamp recording
2. **Manage Notes**:
   - Browse by timeline (page design)
   - Search notes (by keyword, summary, content)
   - Generate table of contents (quick browsing of note structure)
   - Update and delete notes
3. **Permission Control**:
   - Private by default, only accessible by the being itself
   - Silicon curator can manage all notes

### Technical Implementation

- **Controller**: `WorkNoteController`
- **Tools**: `WorkNoteTool`, `ProjectTool`, `ProjectWorkNoteTool`
- **API Routes**:
  - `/api/worknotes` - Get work note list
  - `/api/worknotes/{id}` - Get note details
  - `/api/worknotes/search?q=keyword` - Search notes
  - `/api/worknotes/directory` - Generate note directory
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
- Clean old audit data
- Check system resources

---

## Next Steps

- 📚 Read the [Architecture Guide](architecture.md)
- 🛠️ Check the [Development Guide](development-guide.md)
- 📖 Explore the [API Reference](api-reference.md)
- 🚀 See the [Getting Started Guide](getting-started.md)
