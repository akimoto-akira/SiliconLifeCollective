# Web UI Guide

[English](web-ui-guide.md) | [简体中文](docs/zh-CN/web-ui-guide.md) | [繁體中文](docs/zh-HK/web-ui-guide.md) | [Español](docs/es-ES/web-ui-guide.md) | [日本語](docs/ja-JP/web-ui-guide.md) | [한국어](docs/ko-KR/web-ui-guide.md) | [Čeština](docs/cs-CZ/web-ui-guide.md)

## Overview

The Web UI provides a comprehensive interface for managing silicon beings, monitoring system status, and interacting with AI agents.

## Access

Default URL: `http://localhost:8080`

## Navigation

### Main Sections

1. **Dashboard** - System overview and metrics
2. **Beings** - Manage silicon beings
3. **Chat** - Interact with beings
4. **Chat History** - View silicon being chat history
5. **Tasks** - Task management
6. **Timers** - Timer configuration
7. **Configuration** - System settings
8. **Permissions** - Access control
9. **Logs** - System logs
10. **Audit** - Token usage and audit trails
11. **Memory** - Being memories
12. **Knowledge** - Knowledge base
13. **Code Browser** - Code exploration
14. **Code Editor** - Code editing with hover hints
15. **Project** - Project management
16. **Executor** - Executor management

---

## Dashboard

### Features

- System performance metrics (CPU, Memory, Uptime)
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

Shows all beings with:
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
2. Type message
3. View streaming response
4. See tool executions in real-time

### Tool Call Display

When AI calls tools:
```
🔧 Tool: calendar
📥 Input: {"date": "2026-04-20"}
📤 Output: "农历四月初三"
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
- Cleanup policies

### Localization

Switch between languages:
- English
- 简体中文
- 繁體中文
- 日本語
- 한국어

---

## Skin System

### Available Skins

1. **Admin** - Professional administration interface
2. **Chat** - Conversation-focused design
3. **Creative** - Creative and artistic style
4. **Dev** - Developer-oriented layout

### Switching Skins

1. Click **Settings** (gear icon)
2. Select **Skin**
3. Choose desired skin
4. Interface updates immediately

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
- View expiry dates

### Add Permission Rule

1. Click **Add Rule**
2. Configure:
   - User
   - Resource (e.g., `disk:read`)
   - Allowed/Denied
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

- All tasks with status
- Filter by being or status
- Priority indicators

### Task Details

- Description
- Priority level
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
   - Repeat setting
3. Start

---

## Logs Viewer

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
- Real-time compilation

### Hover Hints

Hover over any identifier to see:
- Type information
- Documentation
- Definition location
- References

---

## Chat History View

### Features

- Silicon being chat history browsing
- Conversation list display
- Message detail viewing
- Timeline view

### Using Chat History

1. Navigate to the **Beings** page
2. Click the **Chat History** link for a silicon being
3. View the conversation list:
   - Conversation title
   - Creation time
   - Message count
4. Click a conversation to view details:
   - Complete message history
   - Timestamps
   - Sender information
   - Tool call records

### Technical Implementation

- **Controller**: `ChatHistoryController`
- **ViewModel**: `ChatHistoryViewModel`
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
3. Select file source:
   - Local files
   - File system path
4. Select files (multiple selection supported)
5. Confirm upload
6. File information will be attached to the message

### Supported File Types

- Text files (.txt, .md, .json, .xml, etc.)
- Code files (.cs, .js, .py, .java, etc.)
- Configuration files (.yml, .yaml, .ini, .conf, etc.)
- Document files (.csv, .log, etc.)

---

## Loading Indicators

### Features

- Loading state display for chat pages
- Auto-select curator session
- Data loading progress feedback

### Behavior

- Loading animation displayed when page loads
- Automatically hidden after data loading completes
- Curator session auto-selected (if exists)
- Multi-language loading prompt text

---

## Responsive Design

The Web UI adapts to different screen sizes:
- Desktop: Full layout
- Tablet: Condensed sidebar
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

### Can't Connect

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
