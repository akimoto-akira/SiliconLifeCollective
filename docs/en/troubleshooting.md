# Troubleshooting Guide

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
