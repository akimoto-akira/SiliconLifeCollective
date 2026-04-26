# Troubleshooting Guide

[English](../en/troubleshooting.md) | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | [日本語](../ja-JP/troubleshooting.md) | [한국어](../ko-KR/troubleshooting.md) | [Deutsch](../de-DE/troubleshooting.md) | [Čeština](../cs-CZ/troubleshooting.md)

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

#### Issue: DashScope 404 error

**Symptoms**:
```
HTTP 404: Model not found
```

**Solution**:
1. Verify API key is correct
2. Check model name matches DashScope catalog
3. Verify region endpoint is correct
4. Check account has access to the model

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

**Or change the port in configuration**.

#### Issue: Being won't start

**Symptoms**:
- Being status shows "Error"
- Logs show initialization failure

**Solution**:
1. Check soul file exists and is valid
2. Verify AI client is configured
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
1. Increase heap size:
```bash
dotnet run --server.gcHeapCount 4
```

2. Clean old data:
```bash
# Archive old logs
mv logs/ logs-archive/
mkdir logs

# Clean old memories
# Via Web UI: Memory Management > Cleanup
```

---

### Permission Issues

#### Issue: Permission denied

**Symptoms**:
```
Permission denied: disk:write
```

**Solution**:
1. Check current permissions:
```bash
curl http://localhost:8080/api/permissions
```

2. Grant permission:
```bash
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

3. Or use Web UI: Permission Management

#### Issue: Permissions not expiring

**Symptoms**:
- Permissions remain active after expiration time

**Solution**:
1. Check system clock synchronization
2. Verify `expiresAt` field set correctly
3. Clear permission cache

---

### Web UI Issues

#### Issue: Cannot access Web UI

**Symptoms**:
- Browser shows "Connection refused"

**Solution**:
1. Verify server is running
2. Check correct URL: `http://localhost:8080`
3. Check firewall settings
4. Check logs for startup errors

#### Issue: SSE not working

**Symptoms**:
- Real-time updates not appearing
- Chat not streaming

**Solution**:
1. Check browser supports SSE
2. Disable proxy buffering for SSE
3. Check network stability
4. Try different browser

#### Issue: UI looks broken

**Symptoms**:
- Styles incorrect
- Layout broken

**Solution**:
1. Clear browser cache
2. Try different skin: Settings > Skin
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
2. Verify storage path exists
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

3. Manually fix corrupted files

---

### Tool Execution Issues

#### Issue: Tool not found

**Symptoms**:
```
Tool "xyz" not found
```

**Solution**:
1. Verify tool name is correct
2. Check tool exists in Tools directory
3. Rebuild the project
4. Check tool implements correctly

#### Issue: Tool returns error

**Symptoms**:
```
Tool execution failed: ...
```

**Solution**:
1. Check tool logs
2. Verify input parameters
3. Test tool independently
4. Check permissions

---

### Work Notes Issues

#### Issue: Cannot create work note

**Symptoms**:
```
Failed to create work note
```

**Solution**:
1. Check being exists and is running
2. Verify storage path has write permissions
3. Check content is not empty (content required)
4. View logs for detailed error

#### Issue: Note search returns no results

**Symptoms**:
- Search keyword returns empty results
- But certain relevant notes exist

**Solution**:
1. Check keyword spelling is correct
2. Try using more general keywords
3. Verify note contains the keyword (case-sensitive)
4. Increase `max_results` parameter value

#### Issue: Note directory generation is slow

**Symptoms**:
- Long response time when generating directory
- Being has large number of notes (>1000 pages)

**Solution**:
1. This is normal, needs to iterate through all notes
2. Consider archiving old notes regularly
3. Use search function instead of directory browsing
4. Planned optimization: Add directory caching mechanism

---

### Knowledge Network Issues

#### Issue: Knowledge query returns empty results

**Symptoms**:
```
No knowledge triples found
```

**Solution**:
1. Verify subject and predicate spelling
2. Check if knowledge has been added to network
3. Use search function for fuzzy matching:
```json
{
  "action": "search",
  "query": "keyword"
}
```

#### Issue: Knowledge path finding fails

**Symptoms**:
```
No path found between concepts
```

**Solution**:
1. Verify both concepts exist in knowledge network
2. Check if association path exists (may have no direct or indirect relationship)
3. Try adding more knowledge to establish connections
4. Reduce path length limit (if set)

#### Issue: Knowledge validation fails

**Symptoms**:
```
Knowledge validation failed
```

**Solution**:
1. Check triple format is correct (subject, predicate, object required)
2. Verify confidence is in 0.0-1.0 range
3. Check for duplicate triples
4. View validation error details for specific issues

#### Issue: Knowledge network statistics inaccurate

**Symptoms**:
- Statistics don't match expectations
- Statistics not updated after adding knowledge

**Solution**:
1. Statistics may take a few seconds to update (caching)
2. Check if delete operation didn't execute successfully
3. Restart application to force statistics refresh
4. Re-query statistics via API

---

### Project Management Issues

#### Issue: Cannot create project

**Symptoms**:
```
Failed to create project
```

**Solution**:
1. Check project name is not empty (required)
2. Verify project name is not duplicate
3. Check storage path has write permissions
4. View logs for detailed error

#### Issue: Project data lost

**Symptoms**:
- Project information cannot load
- Project files corrupted

**Solution**:
1. Check if project storage directory exists
2. Restore project data from backup
3. Verify JSON file format is correct
4. Manually fix corrupted project files

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

Logs are stored at:
```
logs/
├── system.log
├── ai.log
├── permission.log
└── error.log
```

View in real-time:
```bash
tail -f logs/*.log
```

### Using Debugger

```bash
# Run with debugger
dotnet run --project src/SiliconLife.Default --configuration Debug

# Attach debugger
# Via IDE: Attach to Process > SiliconLife.Default
```

---

## Performance Issues

### Slow Response Time

**Optimize**:
1. Reduce AI model complexity
2. Enable caching
3. Clean old data
4. Increase system resources

### High CPU Usage

**Check**:
- Too many beings running
- Infinite loops in tools
- Frequent timer executions

**Solution**:
- Reduce concurrent beings
- Optimize tool code
- Adjust timer intervals

### High Memory Usage

**Monitor**:
```bash
# Via Web UI: Dashboard > Memory
```

**Optimize**:
- Clean old memories
- Reduce context size
- Implement pagination

---

## Getting Help

### View Documentation

- [Quick Start Guide](getting-started.md)
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

1. Check logs for cause
2. Restart application:
```bash
dotnet run --project src/SiliconLife.Default
```

3. Restore from backup if needed

### Data Loss

1. Stop application immediately
2. Check backup files
3. Restore data
4. Verify integrity

### Security Breach

1. Stop all beings
2. Revoke all permissions
3. Check audit logs
4. Review access controls
5. Restart with restricted permissions

---

## Prevention

### Best Practices

1. **Regular Backups**
   - Backup data directory
   - Backup configuration
   - Test recovery process

2. **Monitor Resources**
   - Watch CPU/memory usage
   - Monitor disk space
   - Check network connectivity

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
- 🚀 See the [Quick Start Guide](getting-started.md)
- 🔒 Review the [Security Documentation](security.md)
