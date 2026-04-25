# Troubleshooting Guide

[English](troubleshooting.md) | [简体中文](docs/zh-CN/troubleshooting.md) | [繁體中文](docs/zh-HK/troubleshooting.md) | [Español](docs/es-ES/troubleshooting.md) | [日本語](docs/ja-JP/troubleshooting.md) | [한국어](docs/ko-KR/troubleshooting.md) | [Čeština](docs/cs-CZ/troubleshooting.md)

## Common Issues

### Build & Compilation

#### Issue: Build Fails with Missing Dependencies

**Symptoms**:
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**Solution**:
```bash
dotnet restore
dotnet build
```

#### Issue: .NET SDK Not Found

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

#### Issue: Ollama Connection Refused

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

#### Issue: Model Not Found

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

#### Issue: DashScope 404 Error

**Symptoms**:
```
HTTP 404: Model not found
```

**Solution**:
1. Verify API key is correct
2. Check model name matches DashScope catalog
3. Verify region endpoint is correct
4. Check account has access to that model

---

### Runtime Issues

#### Issue: Port Already in Use

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

**Or change port** in configuration.

#### Issue: Being Won't Start

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

#### Issue: Out of Memory

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

#### Issue: Permission Denied

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

#### Issue: Permission Not Expiring

**Symptoms**:
- Permission persists beyond expiry time

**Solution**:
1. Check system clock synchronization
2. Verify `expiresAt` field is set correctly
3. Clear permission cache

---

### Web UI Issues

#### Issue: Can't Access Web UI

**Symptoms**:
- Browser shows "Connection refused"

**Solution**:
1. Verify server is running
2. Check correct URL: `http://localhost:8080`
3. Check firewall settings
4. Check logs for startup errors

#### Issue: SSE Not Working

**Symptoms**:
- Real-time updates not appearing
- Chat doesn't stream

**Solution**:
1. Check browser supports SSE
2. Disable proxy buffering for SSE
3. Check network stability
4. Try different browser

#### Issue: UI Looks Broken

**Symptoms**:
- Styling is incorrect
- Layout is broken

**Solution**:
1. Clear browser cache
2. Try different skin: Settings > Skin
3. Check browser console for errors
4. Disable browser extensions

---

### Storage Issues

#### Issue: Can't Read/Write Data

**Symptoms**:
```
IOException: Access denied
```

**Solution**:
1. Check file permissions
2. Verify storage path exists
3. Check disk space
4. Run with appropriate privileges

#### Issue: Data Corruption

**Symptoms**:
- JSON parse errors
- Missing data

**Solution**:
1. Restore from backup
2. Check storage integrity:
```bash
# Via Web UI: System > Storage Check
```

3. Fix corrupted files manually

---

### Tool Execution Issues

#### Issue: Tool Not Found

**Symptoms**:
```
Tool "xyz" not found
```

**Solution**:
1. Verify tool name is correct
2. Check tool is in Tools directory
3. Rebuild project
4. Check tool is properly implemented

#### Issue: Tool Returns Error

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

#### Issue: Cannot Create Work Notes

**Symptoms**:
```
Failed to create work note
```

**Solution**:
1. Check if being exists and is running
2. Verify storage path has write permissions
3. Check if content is empty (content required)
4. Check logs for detailed error information

#### Issue: Note Search Returns No Results

**Symptoms**:
- Search keyword returns empty results
- But certain related notes exist

**Solution**:
1. Check if keyword spelling is correct
2. Try a more general keyword
3. Verify note contains that keyword (case-sensitive)
4. Increase `max_results` parameter value

#### Issue: Note Directory Generation is Slow

**Symptoms**:
- Long response time when generating directory
- Being has many notes (>1000 pages)

**Solution**:
1. This is normal, needs to iterate through all notes
2. Consider archiving old notes periodically
3. Use search function instead of directory browsing
4. Planned optimization: add directory caching mechanism

---

### Knowledge Network Issues

#### Issue: Knowledge Query Returns Empty Results

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

#### Issue: Knowledge Path Finding Fails

**Symptoms**:
```
No path found between concepts
```

**Solution**:
1. Verify both concepts exist in knowledge network
2. Check if associative path exists (may have no direct or indirect relationship)
3. Try adding more knowledge to establish connection
4. Lower path length limit (if configured)

#### Issue: Knowledge Validation Fails

**Symptoms**:
```
Knowledge validation failed
```

**Solution**:
1. Check if triple format is correct (subject, predicate, object required)
2. Verify confidence is within 0.0-1.0 range
3. Check for duplicate triples
4. Review validation error details to understand specific problem

#### Issue: Knowledge Network Statistics Inaccurate

**Symptoms**:
- Statistics numbers don't match expectations
- Statistics not updated after adding knowledge

**Solution**:
1. Statistics may take a few seconds to update (cache)
2. Check if delete operation executed successfully
3. Restart application to force statistics refresh
4. Re-query statistics information through API

---

### Project Management Issues

#### Issue: Cannot Create Project

**Symptoms**:
```
Failed to create project
```

**Solution**:
1. Check if project name is empty (required)
2. Verify project name is not duplicated
3. Check storage path has write permissions
4. Check logs for detailed error information

#### Issue: Project Data Loss

**Symptoms**:
- Cannot load project information
- Project file corrupted

**Solution**:
1. Check if project storage directory exists
2. Restore project data from backup
3. Verify JSON file format is correct
4. Manually repair corrupted project file

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

View in real-time:
```bash
tail -f logs/*.log
```

### Use Debugger

```bash
# Run with debugger
dotnet run --project src/SiliconLife.Default --configuration Debug

# Attach debugger
# Via IDE: Attach to Process > SiliconLife.Default
```

---

## Performance Issues

### Slow Response Times

**Optimize**:
1. Reduce AI model complexity
2. Enable caching
3. Clean old data
4. Increase system resources

### High CPU Usage

**Check**:
- Running too many beings
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

### Check Documentation

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
   - Test restore procedure

2. **Monitor Resources**
   - Watch CPU/Memory usage
   - Monitor disk space
   - Check network connectivity

3. **Keep Updated**
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
- 🔒 Review the [Security Documentation](security.md)
