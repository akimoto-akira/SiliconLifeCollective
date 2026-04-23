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

using System.Net;
using System.Text;
using System.Text.Json;
using SiliconLife.Collective;
using SiliconLife.Default.IM;
using SiliconLife.Default.Web.Models;
using SiliconLife.Default.Web.Views;
using System.IO;

namespace SiliconLife.Default.Web;

[WebCode]
public class ChatController : Controller
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<ChatController>();
    private readonly SiliconBeingManager _beingManager;
    private readonly ChatSystem _chatSystem;
    private readonly SkinManager _skinManager;
    private readonly Guid _userId;
    private string _currentPath = "";

    public ChatController()
    {
        var locator = ServiceLocator.Instance;
        _beingManager = locator.BeingManager!;
        _chatSystem = locator.ChatSystem!;
        _skinManager = locator.GetService<SkinManager>()!;
        _userId = Config.Instance.Data.UserGuid;
    }

    public override void Handle()
    {
        _currentPath = Request.Url?.AbsolutePath ?? "/chat";

        if (_currentPath == "/chat" || _currentPath == "/chat/index")
        {
            Index();
        }
        else if (_currentPath == "/api/chat/stream")
        {
            HandleSSE();
        }
        else if (_currentPath == "/api/chat/conversations")
        {
            GetConversations();
        }
        else if (_currentPath == "/api/chat/messages")
        {
            GetMessages();
        }
        else if (_currentPath == "/api/chat/history")
        {
            GetHistory();
        }
        else if (_currentPath == "/api/chat/send")
        {
            SendMessage();
        }
        else if (_currentPath == "/api/chat/stop")
        {
            StopThinking();
        }
        else if (_currentPath == "/api/chat/upload")
        {
            UploadFile();
        }
        else
        {
            Response.StatusCode = 404;
            Response.Close();
        }
    }

    private void Index()
    {
        var skin = _skinManager.GetSkin() ?? new Skins.ChatSkin();
        var view = new ChatView();
        var beings = _beingManager.GetAllBeings();
        var beingDict = beings.ToDictionary(b => b.Id);
        var userNickname = Config.Instance?.Data?.UserNickname ?? "User";
        var loc = LocalizationManager.Instance.GetLocalization(Config.Instance?.Data?.Language ?? Language.ZhCN);

        var sessions = new List<ChatSessionItem>();
        Guid? currentSessionId = null;
        string currentBeingName = "";
        Guid? curatorSessionId = null;
        string curatorBeingName = "";

        foreach (var session in _chatSystem.GetSessionsForUser(_userId, beings.Select(b => b.Id)))
        {
            var messages = session.GetMessages(0, 1);
            var lastMsg = messages.LastOrDefault();

            string displayName;
            if (session.Type == SessionType.SingleChat)
            {
                var otherId = session.Members.FirstOrDefault(id => id != _userId);
                if (otherId != Guid.Empty && beingDict.TryGetValue(otherId, out var being))
                {
                    displayName = loc.GetSingleChatDisplayName(being.Name);
                    // Track curator session separately to prioritize it
                    if (being.IsCurator)
                    {
                        curatorSessionId = session.Id;
                        curatorBeingName = displayName;
                    }
                    // Fall back to first session if no curator session found yet
                    if (currentSessionId == null)
                    {
                        currentSessionId = session.Id;
                        currentBeingName = displayName;
                    }
                }
                else if (otherId != Guid.Empty)
                {
                    displayName = userNickname;
                }
                else
                {
                    continue;
                }
            }
            else
            {
                var memberNames = session.Members
                    .Select(id => id == _userId ? userNickname : beingDict.GetValueOrDefault(id)?.Name ?? id.ToString("N").Substring(0, 8))
                    .ToArray();
                displayName = string.Join(", ", memberNames);
            }

            sessions.Add(new ChatSessionItem
            {
                Id = session.Id,
                Name = displayName,
                LastMessage = lastMsg?.Content ?? "",
                LastMessageAt = lastMsg?.Timestamp ?? DateTime.MinValue
            });
        }

        // Prioritize curator session over the first found session
        if (curatorSessionId.HasValue)
        {
            currentSessionId = curatorSessionId;
            currentBeingName = curatorBeingName;
        }

        var vm = new ChatViewModel
        {
            Skin = skin,
            ActiveMenu = "chat",
            UserId = _userId,
            Sessions = sessions,
            CurrentBeingId = currentSessionId,
            CurrentBeingName = currentBeingName
        };

        // Build tool display name map for the frontend lookup table
        var language = Config.Instance?.Data?.Language ?? Language.ZhCN;
        foreach (var being in beings)
        {
            if (being.ToolManager == null) continue;
            foreach (var toolName in being.ToolManager.GetToolNames())
            {
                if (vm.ToolDisplayNames.ContainsKey(toolName)) continue;
                var tool = being.ToolManager.GetTool(toolName);
                if (tool != null)
                    vm.ToolDisplayNames[toolName] = tool.GetDisplayName(language);
            }
        }

        var html = view.Render(vm);
        RenderHtml(html);
    }

    private void HandleSSE()
    {
        var channelIdStr = Request.QueryString["channelId"];
        Guid? channelId = null;

        if (!string.IsNullOrEmpty(channelIdStr) && Guid.TryParse(channelIdStr, out var parsedChannelId))
        {
            channelId = parsedChannelId;
        }

        var router = ServiceLocator.Instance.GetService<Router>();
        if (router != null)
        {
            _ = router.HandleSSE(Context, _userId, channelId);
        }
        else
        {
            Response.StatusCode = 500;
            Response.Close();
        }
    }

    private void GetConversations()
    {
        try
        {
            var conversations = new List<object>();
            var beings = _beingManager.GetAllBeings();
            var beingDict = beings.ToDictionary(b => b.Id);
            var userNickname = Config.Instance?.Data?.UserNickname ?? "User";

            foreach (var session in _chatSystem.GetSessionsForUser(_userId, _beingManager.GetAllBeings().Select(b => b.Id)))
            {
                var messages = session.GetMessages(0, 1);
                var lastMsg = messages.LastOrDefault();

                string displayName;
                string type;

                if (session.Type == SessionType.SingleChat)
                {
                    type = "single";
                    var otherId = session.Members.FirstOrDefault(id => id != _userId);
                    if (otherId != Guid.Empty && beingDict.TryGetValue(otherId, out var being))
                    {
                        var loc = LocalizationManager.Instance.GetLocalization(Config.Instance?.Data?.Language ?? Language.ZhCN);
                        displayName = loc.GetSingleChatDisplayName(being.Name);
                    }
                    else if (otherId != Guid.Empty)
                    {
                        displayName = userNickname;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    type = "group";
                    var memberNames = session.Members
                        .Select(id => id == _userId ? userNickname : beingDict.GetValueOrDefault(id)?.Name ?? id.ToString("N").Substring(0, 8))
                        .ToArray();
                    displayName = string.Join(", ", memberNames);
                }

                conversations.Add(new
                {
                    sessionId = session.Id.ToString(),
                    type,
                    displayName,
                    lastMessage = lastMsg?.Content ?? "",
                    lastTime = lastMsg?.Timestamp.ToString("HH:mm") ?? ""
                });
            }

            RenderJson(conversations);
        }
        catch (Exception ex)
        {
            RenderJson(new { error = ex.Message });
        }
    }

    private void GetMessages()
    {
        var beingIdStr = Request.QueryString["beingId"];

        if (string.IsNullOrEmpty(beingIdStr) || !Guid.TryParse(beingIdStr, out var beingId))
        {
            RenderJson(Array.Empty<object>());
            return;
        }

        var messages = _chatSystem.GetMessages(_userId, beingId);

        var result = messages.Select(m => new
        {
            id = m.Id.ToString(),
            senderId = m.SenderId.ToString(),
            content = m.Content,
            timestamp = m.Timestamp.ToString("HH:mm"),
            isOwn = m.SenderId == _userId
        });

        RenderJson(result);
    }

    private void GetHistory()
    {
        try
        {
            var sessionIdStr = Request.QueryString["sessionId"];

            if (string.IsNullOrEmpty(sessionIdStr) || !Guid.TryParse(sessionIdStr, out var sessionId))
            {
                RenderJson(new { messages = Array.Empty<object>() });
                return;
            }

            var session = _chatSystem.GetSession(sessionId);
            if (session == null)
            {
                RenderJson(new { messages = Array.Empty<object>() });
                return;
            }

            var beings = _beingManager.GetAllBeings();
            var beingDict = beings.ToDictionary(b => b.Id);
            var messages = session.GetMessages(0, 50);

            var result = messages.Select(m =>
            {
                var senderBeing = m.SenderId != _userId && beingDict.TryGetValue(m.SenderId, out var b) ? b : null;
                string roleStr = m.Role.ToString();
                return new
                {
                    id = m.Id.ToString(),
                    senderId = m.SenderId.ToString(),
                    content = m.Content,
                    thinking = m.Thinking,
                    role = roleStr,
                    senderName = senderBeing?.Name ?? "",
                    timestamp = m.Timestamp,
                    toolCallsJson = m.ToolCallsJson,
                    toolCallId = m.ToolCallId
                };
            });

            RenderJson(new { messages = result });
        }
        catch (Exception ex)
        {
            RenderJson(new { messages = Array.Empty<object>(), error = ex.Message });
        }
    }

    private void SendMessage()
    {
        try
        {
            var body = GetJsonBody<SendRequest>();
            if (body == null || !Guid.TryParse(body.ChannelId, out var channelId) || string.IsNullOrEmpty(body.Content))
            {
                RenderJson(new { success = false, error = "Invalid request" });
                return;
            }

            var imProvider = ServiceLocator.Instance.GetService<IIMProvider>();
            if (imProvider is not WebUIProvider webUIProvider)
            {
                RenderJson(new { success = false, error = "Provider not available" });
                return;
            }

            webUIProvider.HandleChatMessage(_userId, channelId, body.Content);

            RenderJson(new { success = true });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private class SendRequest
    {
        public string? ChannelId { get; set; }
        public string? Content { get; set; }
    }

    /// <summary>
    /// Stops the AI thinking process for the specified channel.
    /// </summary>
    private void StopThinking()
    {
        try
        {
            var body = GetJsonBody<StopRequest>();
            if (body == null || !Guid.TryParse(body.ChannelId, out var channelId))
            {
                _logger.Warn(null, "StopThinking: Invalid request, body={0}", body?.ChannelId ?? "null");
                RenderJson(new { success = false, error = "Invalid request" });
                return;
            }

            _logger.Info(null, "StopThinking: Attempting to cancel stream for channel {0}", channelId);
            
            var cancellationManager = ServiceLocator.Instance.GetService<StreamCancellationManager>();
            if (cancellationManager == null)
            {
                _logger.Error(null, "StopThinking: StreamCancellationManager not available");
                RenderJson(new { success = false, error = "Cancellation manager not available" });
                return;
            }
            
            _logger.Info(null, "StopThinking: HasActiveStream({0})={1}", channelId, cancellationManager.HasActiveStream(channelId));
            
            bool cancelled = cancellationManager.CancelStream(channelId);
            
            _logger.Info(null, "StopThinking: CancelStream({0}) returned {1}", channelId, cancelled);

            if (cancelled)
            {
                // Notify frontend via SSE that the stream was stopped
                var imProvider = ServiceLocator.Instance.GetService<IIMProvider>();
                if (imProvider is WebUIProvider webUIProvider)
                {
                    _ = webUIProvider.SendStreamStoppedAsync(channelId);
                    _logger.Info(null, "StopThinking: StreamStopped event sent for channel {0}", channelId);
                }
            }

            RenderJson(new { success = cancelled });
        }
        catch (Exception ex)
        {
            _logger.Error(null, "StopThinking: Exception occurred: {0}", ex.Message);
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    private class StopRequest
    {
        public string? ChannelId { get; set; }
    }

    /// <summary>
    /// <summary>
    /// Handles file upload. Supports two modes:
    /// 1. JSON body with local file path (isLocalPath=true)
    /// 2. Multipart/form-data with actual file upload
    /// </summary>
    private void UploadFile()
    {
        try
        {
            var contentType = Request.ContentType ?? "";
            
            // Check if it's a multipart/form-data request (file upload)
            if (contentType.StartsWith("multipart/form-data", StringComparison.OrdinalIgnoreCase))
            {
                UploadFileFromMultipart();
                return;
            }
            
            // Otherwise, treat as JSON (local path reference)
            var body = GetJsonBody<UploadFileRequest>();
            if (body == null || string.IsNullOrEmpty(body.FilePath))
            {
                RenderJson(new { success = false, error = "Invalid file path" });
                return;
            }

            if (!Guid.TryParse(body.ChannelId, out var channelId))
            {
                RenderJson(new { success = false, error = "Invalid channelId" });
                return;
            }

            // Resolve full path
            string fullPath = Path.GetFullPath(body.FilePath);

            // Security check: prevent path traversal
            if (!IsPathAllowed(fullPath))
            {
                RenderJson(new { success = false, error = "Path not allowed for security reasons" });
                return;
            }

            // Check file exists
            if (!File.Exists(fullPath))
            {
                RenderJson(new { success = false, error = "File not found" });
                return;
            }

            // Get file info
            var fileInfo = new FileInfo(fullPath);

            // Validate file size (100MB limit)
            const long maxFileSize = 100 * 1024 * 1024;
            if (fileInfo.Length > maxFileSize)
            {
                RenderJson(new { success = false, error = "File too large (max 100MB)" });
                return;
            }

            // Create file message
            var fileMessage = new SiliconLife.Collective.ChatMessage
            {
                Id = Guid.NewGuid(),
                SenderId = _userId,
                ChannelId = channelId,
                Content = fullPath,
                Timestamp = DateTime.Now,
                Type = MessageType.File,
                FileMetadata = new FileMetadata
                {
                    FilePath = fullPath,
                    FileName = fileInfo.Name,
                    FileSize = fileInfo.Length,
                    MimeType = GetMimeType(fileInfo.Extension),
                    IsLocalPath = body.IsLocalPath
                }
            };

            // Add to chat system
            _chatSystem.AddMessage(fileMessage);

            // Push file message via SSE
            var imProvider = ServiceLocator.Instance.GetService<IIMProvider>();
            if (imProvider is WebUIProvider webUIProvider)
            {
                _ = webUIProvider.HandleFileMessage(fileMessage);
            }

            // Trigger AI thinking via IM event
            var webUIProv = imProvider as WebUIProvider;
            if (webUIProv != null)
            {
                // Create a text notification message for the AI to process the file
                var notifyMessage = new SiliconLife.Collective.ChatMessage
                {
                    Id = Guid.NewGuid(),
                    SenderId = _userId,
                    ChannelId = channelId,
                    Content = $"[文件上传] {fileInfo.Name} ({FormatFileSize(fileInfo.Length)}), 路径: {fullPath}",
                    Timestamp = DateTime.Now,
                    Type = MessageType.Text
                };
                webUIProv.HandleChatMessage(_userId, channelId, notifyMessage.Content);
            }

            RenderJson(new
            {
                success = true,
                messageId = fileMessage.Id.ToString(),
                fileName = fileInfo.Name,
                fileSize = fileInfo.Length,
                isLocalPath = body.IsLocalPath
            });
        }
        catch (Exception ex)
        {
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Security check: verifies that the path does not access sensitive system directories.
    /// </summary>
    private static bool IsPathAllowed(string fullPath)
    {
        var forbiddenPaths = new[]
        {
            "C:\\Windows",
            "C:\\Program Files",
            "C:\\Program Files (x86)",
        };

        foreach (var forbidden in forbiddenPaths)
        {
            if (fullPath.StartsWith(forbidden, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Resolves MIME type from file extension.
    /// </summary>
    private static string GetMimeType(string extension)
    {
        var mimeTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { ".pdf", "application/pdf" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".txt", "text/plain" },
            { ".md", "text/markdown" },
            { ".json", "application/json" },
            { ".csv", "text/csv" },
            { ".xml", "application/xml" },
            { ".png", "image/png" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".gif", "image/gif" },
            { ".log", "text/plain" },
        };

        return mimeTypes.TryGetValue(extension, out var mime) ? mime : "application/octet-stream";
    }

    /// <summary>
    /// Formats a file size in human-readable format.
    /// </summary>
    private static string FormatFileSize(long bytes)
    {
        if (bytes < 1024) return bytes + " B";
        if (bytes < 1024 * 1024) return (bytes / 1024.0).ToString("F1") + " KB";
        return (bytes / (1024.0 * 1024.0)).ToString("F1") + " MB";
    }

    /// <summary>
    /// Handles multipart/form-data file upload.
    /// </summary>
    private void UploadFileFromMultipart()
    {
        try
        {
            // Parse channelId from query string or form data
            string? channelIdStr = Request.QueryString["channelId"];
            if (string.IsNullOrEmpty(channelIdStr) || !Guid.TryParse(channelIdStr, out var channelId))
            {
                RenderJson(new { success = false, error = "Invalid or missing channelId" });
                return;
            }

            // Read the multipart body
            var boundary = GetMultipartBoundary();
            if (string.IsNullOrEmpty(boundary))
            {
                RenderJson(new { success = false, error = "Invalid multipart request" });
                return;
            }

            // Parse multipart form data
            var parts = ParseMultipartFormData(boundary);
            if (!parts.TryGetValue("file", out var fileData) || fileData == null)
            {
                RenderJson(new { success = false, error = "No file uploaded" });
                return;
            }

            // Validate file size (100MB limit)
            const long maxFileSize = 100 * 1024 * 1024;
            if (fileData.Data.Length > maxFileSize)
            {
                RenderJson(new { success = false, error = "File too large (max 100MB)" });
                return;
            }

            // Save file to temporary directory
            var tempDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploads");
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            var fileName = SanitizeFileName(fileData.FileName ?? "uploaded_file");
            var tempFilePath = Path.Combine(tempDir, $"{Guid.NewGuid()}_{fileName}");
            File.WriteAllBytes(tempFilePath, fileData.Data);

            // Create file message
            var fileMessage = new SiliconLife.Collective.ChatMessage
            {
                Id = Guid.NewGuid(),
                SenderId = _userId,
                ChannelId = channelId,
                Content = $"[Uploaded File] {fileName}",
                Timestamp = DateTime.Now,
                Type = MessageType.File,
                FileMetadata = new FileMetadata
                {
                    FilePath = tempFilePath,
                    FileName = fileName,
                    FileSize = fileData.Data.Length,
                    MimeType = fileData.ContentType ?? GetMimeType(Path.GetExtension(fileName)),
                    IsLocalPath = true
                }
            };

            // Add to chat system
            _chatSystem.AddMessage(fileMessage);

            // Push file message via SSE
            var imProvider = ServiceLocator.Instance.GetService<IIMProvider>();
            if (imProvider is WebUIProvider webUIProvider)
            {
                _ = webUIProvider.HandleFileMessage(fileMessage);
            }

            // Trigger AI thinking via IM event
            var webUIProv = imProvider as WebUIProvider;
            if (webUIProv != null)
            {
                // Create a text notification message for the AI to process the file
                var notifyMessage = new SiliconLife.Collective.ChatMessage
                {
                    Id = Guid.NewGuid(),
                    SenderId = _userId,
                    ChannelId = channelId,
                    Content = $"[文件已上传] {fileName} ({FormatFileSize(fileData.Data.Length)})",
                    Timestamp = DateTime.Now,
                    Type = MessageType.Text
                };
                webUIProv.HandleChatMessage(_userId, channelId, notifyMessage.Content);
            }

            RenderJson(new { success = true, fileName = fileName, fileSize = fileData.Data.Length });
        }
        catch (Exception ex)
        {
            _logger.Error(null, "UploadFileFromMultipart failed: {0}", ex.Message);
            RenderJson(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Extracts multipart boundary from Content-Type header.
    /// </summary>
    private string? GetMultipartBoundary()
    {
        var contentType = Request.ContentType ?? "";
        var boundaryIndex = contentType.IndexOf("boundary=");
        if (boundaryIndex < 0)
            return null;
        
        return contentType.Substring(boundaryIndex + 9).Trim('"');
    }

    /// <summary>
    /// Simple multipart form data parser.
    /// </summary>
    private Dictionary<string, MultipartPart> ParseMultipartFormData(string boundary)
    {
        var result = new Dictionary<string, MultipartPart>();
        
        using var memoryStream = new MemoryStream();
        Request.InputStream.CopyTo(memoryStream);
        var rawData = memoryStream.ToArray();
        
        var boundaryBytes = Encoding.UTF8.GetBytes($"--{boundary}");
        var endBoundaryBytes = Encoding.UTF8.GetBytes($"--{boundary}--");
        
        int position = 0;
        
        while (position < rawData.Length)
        {
            // Find next boundary
            int boundaryStart = FindBytes(rawData, boundaryBytes, position);
            if (boundaryStart < 0)
                break;
            
            position = boundaryStart + boundaryBytes.Length;
            
            // Skip CRLF
            if (position + 2 <= rawData.Length && rawData[position] == 13 && rawData[position + 1] == 10)
                position += 2;
            
            // Read headers
            int headerEnd = FindBytes(rawData, new byte[] { 13, 10, 13, 10 }, position);
            if (headerEnd < 0)
                break;
            
            var headers = Encoding.UTF8.GetString(rawData, position, headerEnd - position);
            position = headerEnd + 4;
            
            // Find content
            int nextBoundary = FindBytes(rawData, boundaryBytes, position);
            if (nextBoundary < 0)
                break;
            
            // Remove trailing CRLF
            int contentEnd = nextBoundary;
            if (contentEnd >= 2 && rawData[contentEnd - 2] == 13 && rawData[contentEnd - 1] == 10)
                contentEnd -= 2;
            
            var contentLength = contentEnd - position;
            var content = new byte[contentLength];
            Array.Copy(rawData, position, content, 0, contentLength);
            
            // Parse headers to get field name and filename
            var part = ParseMultipartHeaders(headers, content);
            if (part != null && !string.IsNullOrEmpty(part.Name))
            {
                result[part.Name] = part;
            }
            
            position = nextBoundary;
        }
        
        return result;
    }

    private MultipartPart? ParseMultipartHeaders(string headers, byte[] content)
    {
        var part = new MultipartPart { Data = content };
        
        // Parse Content-Disposition
        var dispIndex = headers.IndexOf("Content-Disposition:", StringComparison.OrdinalIgnoreCase);
        if (dispIndex >= 0)
        {
            var dispLine = headers.Substring(dispIndex);
            var nameStart = dispLine.IndexOf("name=\"");
            if (nameStart >= 0)
            {
                nameStart += 6;
                var nameEnd = dispLine.IndexOf("\"", nameStart);
                if (nameEnd > nameStart)
                {
                    part.Name = dispLine.Substring(nameStart, nameEnd - nameStart);
                }
            }
            
            var filenameStart = dispLine.IndexOf("filename=\"");
            if (filenameStart >= 0)
            {
                filenameStart += 10;
                var filenameEnd = dispLine.IndexOf("\"", filenameStart);
                if (filenameEnd > filenameStart)
                {
                    part.FileName = dispLine.Substring(filenameStart, filenameEnd - filenameStart);
                }
            }
        }
        
        // Parse Content-Type
        var typeIndex = headers.IndexOf("Content-Type:", StringComparison.OrdinalIgnoreCase);
        if (typeIndex >= 0)
        {
            var typeLine = headers.Substring(typeIndex);
            var endLine = typeLine.IndexOf("\r\n");
            if (endLine < 0) endLine = typeLine.Length;
            
            part.ContentType = typeLine.Substring(13, endLine - 13).Trim();
        }
        
        return part;
    }

    private int FindBytes(byte[] source, byte[] pattern, int startIndex)
    {
        if (pattern.Length == 0 || startIndex + pattern.Length > source.Length)
            return -1;
        
        for (int i = startIndex; i <= source.Length - pattern.Length; i++)
        {
            bool found = true;
            for (int j = 0; j < pattern.Length; j++)
            {
                if (source[i + j] != pattern[j])
                {
                    found = false;
                    break;
                }
            }
            if (found)
                return i;
        }
        return -1;
    }

    private string SanitizeFileName(string fileName)
    {
        return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), "_"));
    }

    private class MultipartPart
    {
        public string? Name { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public byte[] Data { get; set; } = Array.Empty<byte>();
    }

    private class UploadFileRequest
    {
        public string? ChannelId { get; set; }
        public string? FilePath { get; set; }
        public bool IsLocalPath { get; set; } = true;
    }
}
