# CS Source Mode Demo

A plugin loaded from raw `.cs` source files instead of a pre-compiled DLL, demonstrating PluginLoader's CS source compilation mode (introduced by task-389).

## How CS Source Mode Works

When PluginLoader scans a plugin directory and finds **no DLL**, it automatically enters CS source mode:

```
1. PluginLoader scans plugin directory → no DLL found
2. Enters CS source mode
3. cs.txt found → reads line by line, only loads listed .cs files
   (No cs.txt → loads all *.cs files in the directory)
4. Scans sibling DLLs → trusted DLLs added as references directly;
   non-trusted DLLs must pass ScanForbiddenReferences to be used as references
5. CompilationCore (restricted mode) compiles .cs files into in-memory DLL
6. In-memory DLL bytes are written to a temp file for ScanForbiddenReferences scanning
7. Scan passes → reflection finds IPlugin implementation → instantiate
8. Log shows: "Plugin loaded [CS-Source]: {Id} v{Version} from {DirName}"
```

## cs.txt — Selective Loading Whitelist

The `cs.txt` file specifies which `.cs` files to compile, one filename per line:

```
Plugin.cs
```

- **Listed files**: Compiled and loaded (e.g. `Plugin.cs`)
- **Unlisted files**: Ignored by the compiler (e.g. `Helpers.cs`)
- **Lines starting with `#`**: Treated as comments
- **Blank lines**: Ignored
- **No cs.txt**: All `*.cs` files in the directory are loaded

This demo intentionally lists only `Plugin.cs` in cs.txt. `Helpers.cs` exists in the directory but is **not** compiled, demonstrating the selective loading mechanism.

## Directory Structure

```
22-CSSourceMode/
├── Plugin.cs          — IPlugin implementation (listed in cs.txt → compiled)
├── Helpers.cs         — Helper class (NOT in cs.txt → not compiled)
├── cs.txt             — Whitelist specifying which .cs files to compile
├── README.md          — English documentation
├── README.zh-CN.md    — Chinese documentation
└── ...                — Other language READMEs
```

## CS Source Mode vs DLL Mode

| Aspect | DLL Mode | CS Source Mode |
|--------|----------|----------------|
| Plugin format | Pre-compiled `.dll` | Raw `.cs` source files |
| Loading trigger | DLL found in plugin directory | No DLL, but `.cs` files exist |
| Compilation | Done at build time | Done at load time by PluginLoader |
| Performance | No compilation overhead | Roslyn compilation at startup |
| Security scan | Direct PE metadata scan | Compile → temp DLL → PE metadata scan |
| Log prefix | `Plugin loaded:` | `Plugin loaded [CS-Source]:` |
| Best for | Production deployment | Development iteration |

## Comparison with 01-MinimalPlugin

| Aspect | 01-MinimalPlugin | 22-CSSourceMode |
|--------|------------------|-----------------|
| Plugin format | Pre-compiled DLL | CS source files |
| Loading path | Standard DLL loading | CS source compilation |
| Functionality | Equivalent | Equivalent |
| IPlugin.Id | `com.siliconlife.demo.minimal` | `com.siliconlife.demo.cssource` |

## Error Handling

| Scenario | Behavior |
|----------|----------|
| No DLL, no .cs files | Warn: "No DLL and no CS source files found" |
| Compilation errors | Error: Detailed diagnostic messages logged |
| Security scan failure | Error: All violations listed, plugin rejected |
| cs.txt entry not found | Warn: "cs.txt entry not found or not a .cs file" |
| Sibling DLL scan failure | Warn: DLL not added as reference, compilation proceeds without it |

## Security Note

CS source mode plugins undergo the **same security scanning** as DLL-mode plugins. The compiled assembly is written to a temporary DLL file and scanned with `ScanForbiddenReferences` — the same scan that pre-compiled DLLs receive. All forbidden namespace/type/member/string rules apply identically.

Plugins are still loaded in an isolated context and scanned for forbidden references (e.g. `System.IO`, `System.Net.Http`). See the [Security Documentation](../../docs/en/security.md) for details.
