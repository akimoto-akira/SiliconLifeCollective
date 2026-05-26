# Troubleshooting Guide

> **Version: v0.2.0-alpha**

**English** | [Deutsch](../de-DE/troubleshooting.md) | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | [Čeština](../cs-CZ/troubleshooting.md) | [Русский](../ru-RU/troubleshooting.md)

## Common Issues

### Build and Compilation

#### Issue: Build fails with missing dependencies

**Symptoms**:
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**Solution**:
```bash
dotnet restore
dotnet build
```

#### Issue: .NET SDK not found

**Symptoms**:
```
The .NET SDK could not be found
```

**Solution**:
1. Install .NET 9 SDK: https://dotnet.microsoft.com/download/dotnet/9.0
2. Verify installation:
```bash
dotnet --version
```

---

### AI Connection Issues

#### Issue: Ollama connection refused

**Symptoms**:
```
Failed to connect to Ollama at http://localhost:11434
```

**Solution**:
```bash
# Check if Ollama is running
ollama list

# Start Ollama
ollama serve

# Test connection
curl http://localhost:11434/api/tags
```

#### Issue: Model not found

**Symptoms**:
```
model "qwen2.5:7b" not found
```

**Solution**:
```bash
# Pull the required model
ollama pull qwen2.5:7b

# List available models
ollama list
```

#### Issue: Bailian 404 error

**Symptoms**:
```
HTTP 404: Model not found
```

**Solution**:
1. Verify the API key is correct
2. Check that the model name matches the Bailian catalog
3. Verify the regional endpoint is correct
4. Check that the account has access to the model

#### Issue: Volcengine Ark connection failed

**Symptoms**:
```
HTTP 401: Unauthorized
or
HTTP 404: Endpoint not found
```

**Solution**:
1. Verify the API key is correct
2. Check that the Endpoint URL format is correct (default: `https://ark.cn-beijing.volces.com/api/v3/chat/completions`)
3. Confirm the Model parameter uses the inference endpoint ID (e.g., `ep-20241212123456-abcde`), not the model name
4. Check that the account has access to the endpoint

---

### Runtime Issues

#### Issue: Port already in use

**Symptoms**:
```
HttpListenerException: Address already in use
```

**Solution**:

**Windows**:
```bash
netstat -ano | findstr :8080
taskkill /PID <PID> /F
```

**Linux/Mac**:
```bash
lsof -ti:8080 | xargs kill -9
```

**Or change the port in the configuration.**

#### Issue: Silicon Being fails to start

**Symptoms**:
- Silicon Being status shows "Error"
- Logs show initialization failure

**Solution**:
1. Check that the Soul File exists and is valid
2. Verify the AI client is configured
3. Check logs for specific errors:
```bash
tail -f logs/*.log
```

#### Issue: Out of memory

**Symptoms**:
```
OutOfMemoryException
```

**Solution**:
1. **SiliconLife.Default**: Increase heap size:
```bash
dotnet run --project src/SiliconLife.Default --server.gcHeapCount 4
```

2. **SiliconLife.Fast**: The Fast version inherently has higher memory usage (~500MB). If memory remains insufficient, consider:
   - Reducing the number of concurrent Silicon Beings
   - Cleaning up old data to free memory

3. Clean up old data:
```bash
# Archive old logs
mv logs/ logs-archive/
mkdir logs

# Clean up old memories
# Via Web UI: Memory Management > Clean
```

> **Tip**: SiliconLife.Default has lower memory usage (~200MB), suitable for memory-constrained environments; SiliconLife.Fast has higher memory usage but better performance, suitable for production environments.

---

### Permission Issues

#### Issue: Permission denied

**Symptoms**:
```
Permission denied: FileAccess C:\Windows
```

**Solution**:
1. Check current permissions:
```bash
curl http://localhost:8080/api/permissions/list
```

2. Grant permissions:
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "permissionType": "FileAccess",
    "resourcePrefix": "C:\\Projects",
    "result": "Allowed",
    "description": "Allow project directory access"
  }'
```

3. Or use the Web UI: Permission Management

#### Issue: Permissions not expiring

**Symptoms**:
- Permissions remain valid after expiration time

**Solution**:
1. Check system clock synchronization
2. Verify the `expiresAt` field is set correctly
3. Clear the permission cache

---

### Web UI Issues

#### Issue: Cannot access Web UI

**Symptoms**:
- Browser shows "Connection refused"

**Solution**:
1. Verify the server is running
2. Check the correct URL: `http://localhost:8080`
3. Check firewall settings
4. Check logs for startup errors

#### Issue: SSE not working

**Symptoms**:
- Real-time updates not appearing
- Chat not streaming

**Solution**:
1. Check browser SSE support
2. Disable proxy buffering for SSE
3. Check network stability
4. Try a different browser

#### Issue: UI looks broken

**Symptoms**:
- Styles are incorrect
- Layout is broken

**Solution**:
1. Clear browser cache
2. Try a different skin: Settings > Skin
3. Check browser console for errors
4. Disable browser extensions

---

### Storage Issues

#### Issue: Cannot read/write data

**Symptoms**:
```
IOException: Access denied
```

**Solution**:
1. Check file permissions
2. Verify the storage path exists
3. Check disk space
4. Run with appropriate permissions

#### Issue: Data corruption

**Symptoms**:
- JSON parse errors
- Data loss

**Solution**:
1. Restore from backup
2. Check storage integrity:
```bash
# Via Web UI: System > Storage Check
```

3. Manually repair corrupted files

#### Issue: SpeedyPack storage file corruption (Fast version)

**Symptoms**:
- `.spk` files cannot be loaded
- SpeedyStorage initialization fails

**Solution**:
1. Use the `SiliconLife.Speedy.Manager` tool to check and repair `.spk` files
2. Check that the `.spk.idx` index file matches the `.spk` file
3. If the index file is corrupted, delete the `.spk.idx` file and the system will automatically rebuild the index
4. Restore `.spk` files from backup

#### Issue: SpeedyPack auto-compaction failure (Fast version)

**Symptoms**:
- `.spk` files keep growing
- Disk space running low

**Solution**:
1. Check that `SpeedyPackAutoCompactor` is running properly
2. Manually trigger a compaction operation
3. Check compaction threshold configuration
4. Use the `SiliconLife.Speedy.Manager` tool for manual compaction

---

### Tool Execution Issues

#### Issue: Tool not found

**Symptoms**:
```
Tool "xyz" not found
```

**Solution**:
1. Verify the tool name is correct
2. Check that the tool is in the Tools directory
3. Rebuild the project
4. Check that the tool is properly implemented

#### Issue: Tool returns error

**Symptoms**:
```
Tool execution failed: ...
```

**Solution**:
1. Check tool logs
2. Verify input parameters
3. Test the tool independently
4. Check permissions

---

### Plugin Issues

#### Issue: Plugin load failure

**Symptoms**:
```
Plugin load failed: Security check failed
```

**Solution**:
1. Check if the plugin references forbidden namespaces (`System.IO`, `System.Net.Http`, `System.Net.WebSockets`, `System.Net.Sockets`, `Microsoft.CodeAnalysis`)
2. Verify the plugin only references assemblies in the trusted assembly whitelist
3. Check that the plugin correctly implements the `IPlugin` interface
4. Check logs for detailed security check failure reasons

#### Issue: Plugin tools not registered

**Symptoms**:
- Plugin loads successfully but tools do not appear in the tool list

**Solution**:
1. Confirm that the tool class in the plugin correctly implements the `ITool` interface
2. Check that the tool class is public
3. Verify that `ToolManager.ScanAllPluginAssemblies()` is being called
4. Rebuild the plugin and restart the application

---

### Work Note Issues

#### Issue: Cannot create work note

**Symptoms**:
```
Failed to create work note
```

**Solution**:
1. Check that the Silicon Being exists and is in a running state
2. Verify the storage path has write permissions
3. Check that the content is not empty (content is required)
4. Check logs for detailed error information

#### Issue: Note search returns no results

**Symptoms**:
- Search keywords return empty results
- But relevant notes are known to exist

**Solution**:
1. Check keyword spelling
2. Try using more general keywords
3. Verify that the notes contain the keyword (case-sensitive)
4. Increase the `max_results` parameter value

#### Issue: Note directory generation is slow

**Symptoms**:
- Long response time when generating directory
- Silicon Being has a large number of notes (>1000 pages)

**Solution**:
1. This is normal behavior — all notes need to be traversed
2. Consider periodically archiving old notes
3. Use search functionality instead of directory browsing
4. Planned optimization: add directory caching mechanism

---

### Knowledge Network Issues

#### Issue: Knowledge query returns empty results

**Symptoms**:
```
No knowledge triples found
```

**Solution**:
1. Verify the spelling of subject and predicate
2. Check that the knowledge has been added to the network
3. Use the search function for fuzzy matching:
```json
{
  "action": "search",
  "query": "keyword"
}
```

#### Issue: Knowledge path lookup failure

**Symptoms**:
```
No path found between concepts
```

**Solution**:
1. Verify that both concepts exist in the Knowledge Network
2. Check if there is a connecting path (there may be no direct or indirect relationship)
3. Try adding more knowledge to establish connections
4. Lower the path length limit (if one is set)

#### Issue: Knowledge validation failure

**Symptoms**:
```
Knowledge validation failed
```

**Solution**:
1. Check that the triple format is correct (subject, predicate, object are all required)
2. Verify the confidence score is in the 0.0–1.0 range
3. Check for duplicate triples
4. Review validation error details for specific issues

#### Issue: Knowledge Network statistics are inaccurate

**Symptoms**:
- Statistics don't match expectations
- Statistics not updated after adding knowledge

**Solution**:
1. Statistics may take a few seconds to update (caching)
2. Check if any delete operations failed
3. Restart the application to force a statistics refresh
4. Re-query statistics via API

---

### Project Management Issues

#### Issue: Cannot create project

**Symptoms**:
```
Failed to create project
```

**Solution**:
1. Check that the project name is not empty (required)
2. Verify the project name is not a duplicate
3. Check that the storage path has write permissions
4. Check logs for detailed error information

#### Issue: Project data loss

**Symptoms**:
- Project information cannot be loaded
- Project files are corrupted

**Solution**:
1. Check that the project storage directory exists
2. Restore project data from backup
3. Verify the JSON file format is correct
4. Manually repair corrupted project files

#### Issue: Project role assignment failure

**Symptoms**:
```
Failed to assign role
```

**Solution**:
1. Confirm that the Silicon Being has joined the project
2. Check that the role name is valid
3. Verify that the operator is a Silicon Curator
4. Check logs for detailed error information

#### Issue: Workflow cannot start

**Symptoms**:
- Workflow instance creation fails
- State transitions not executing

**Solution**:
1. Check that a workflow template has been defined
2. Verify the initial state is set correctly
3. Confirm the project has a workflow template bound to it
4. Check workflow logs for transition errors

---

### Tool Permission Issues

#### Issue: Tool operation denied

**Symptoms**:
```
Tool operation denied: network:post
```

**Solution**:
1. Check the Silicon Being's tool permission configuration:
```bash
curl http://localhost:8080/api/beings/tool-permissions?beingId=<id>
```

2. Update tool permissions:
```bash
curl -X PUT http://localhost:8080/api/beings/tool-permissions \
  -H "Content-Type: application/json" \
  -d '{
    "beingId": "being-uuid",
    "permissions": {
      "network:post": "allowed"
    }
  }'
```

3. Or use the Web UI: Silicon Being → Tool Permissions

#### Issue: Project tool permissions not taking effect

**Symptoms**:
- Project-level tool permissions not working as expected

**Solution**:
1. Confirm that project-level permissions are configured correctly
2. Check for conflicts between Silicon Being-level and project-level permissions
3. Project-level permissions are independent of Silicon Being-level permissions; the intersection of both is used
4. Check audit logs to confirm permission check results

---

## Debugging

### Enable Verbose Logging

Edit configuration:
```json
{
  "logging": {
    "level": "debug"
  }
}
```

### Check Logs

Logs are stored in:
```
logs/
├── system.log
├── ai.log
├── permission.log
└── error.log
```

View in real time:
```bash
tail -f logs/*.log
```

### Using the Debugger

**SiliconLife.Default (default implementation)**:
```bash
# Run with debugger
dotnet run --project src/SiliconLife.Default --configuration Debug

# Attach debugger
# Via IDE: Attach to Process > SiliconLife.Default
```

**SiliconLife.Fast (high-performance version)**:
```bash
# Run with debugger
dotnet run --project src/SiliconLife.Fast --configuration Debug

# Attach debugger
# Via IDE: Attach to Process > SiliconLife.Fast
```

> **Recommendation**: Use SiliconLife.Default during development and debugging, then switch to SiliconLife.Fast for production deployment after architecture validation is complete.

---

## Performance Issues

### Slow Response Time

**Optimization**:
1. Reduce AI model complexity
2. Enable caching
3. Clean up old data
4. Increase system resources

### High CPU Usage

**Check for**:
- Too many Silicon Beings running
- Infinite loops in tools
- Frequent timer executions

**Solution**:
- Reduce concurrent Silicon Beings
- Optimize tool code
- Adjust timer intervals

### High Memory Usage

**Monitor**:
```bash
# Via Web UI: Dashboard > Memory
```

**Optimize**:
- Clean up old memories
- Reduce context size
- Implement pagination

---

## Getting Help

### Read the Documentation

- [Getting Started Guide](getting-started.md)
- [Development Guide](development-guide.md)
- [API Reference](api-reference.md)
- [Architecture Guide](architecture.md)

### Check Logs

Always check logs first for error details.

### Community Support

- GitHub Issues: Report bugs
- Discussions: Ask questions
- Documentation: Search for solutions

---

## Emergency Procedures

### System Crash

1. Check logs for the cause
2. Restart the application:

**SiliconLife.Default (default implementation)**:
```bash
dotnet run --project src/SiliconLife.Default
```

**SiliconLife.Fast (recommended production version)**:
```bash
dotnet run --project src/SiliconLife.Fast
```

3. Restore from backup if needed

### Data Loss

1. Stop the application immediately
2. Check backup files
3. Restore data
4. Verify integrity

### Security Breach

1. Stop all Silicon Beings
2. Revoke all permissions
3. Check audit logs
4. Review access controls
5. Restart with restricted permissions

---

## Prevention

### Best Practices

1. **Regular Backups**
   - Back up data directories
   - Back up configurations
   - Test restore procedures

2. **Monitor Resources**
   - Monitor CPU/memory usage
   - Monitor disk space
   - Check network connections

3. **Stay Updated**
   - Update .NET SDK
   - Update dependencies
   - Apply security patches

4. **Test Changes**
   - Test in development first
   - Use version control
   - Document changes

---

## Next Steps

- 📚 Read the [Architecture Guide](architecture.md)
- 🛠️ Check the [Development Guide](development-guide.md)
- 🚀 See the [Getting Started Guide](getting-started.md)
- 🔒 Check the [Security Documentation](security.md)
