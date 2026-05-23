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

using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Runtime.Loader;

namespace SiliconLife.Collective;

public class PluginLoader
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<PluginLoader>();
    private readonly List<LoadedPlugin> _loadedPlugins = [];
    private readonly string _pluginDirectory;

    public PluginLoader(string pluginDirectory)
    {
        _pluginDirectory = pluginDirectory;
    }

    public IReadOnlyList<IPlugin> Plugins => _loadedPlugins.Select(p => p.Plugin).ToList();

    public void LoadAll()
    {
        if (!Directory.Exists(_pluginDirectory))
        {
            _logger.Warn(null, "Plugin directory does not exist: {0}", _pluginDirectory);
            return;
        }

        foreach (string subDir in Directory.GetDirectories(_pluginDirectory))
        {
            LoadPluginFromDirectory(subDir);
        }

        _logger.Info(null, "Loaded {0} plugin(s) from {1}", _loadedPlugins.Count, _pluginDirectory);
    }

    private void LoadPluginFromDirectory(string pluginDir)
    {
        string dirName = Path.GetFileName(pluginDir);
        string? dllPath = Directory.GetFiles(pluginDir, $"{dirName}.dll")
            .Concat(Directory.GetFiles(pluginDir, "*.dll"))
            .FirstOrDefault();

        if (dllPath == null)
        {
            _logger.Warn(null, "No DLL found in plugin directory: {0}", pluginDir);
            return;
        }

        // Collect TypeRefs from all whitelisted DLLs in the plugin directory for transitive exemption
        HashSet<(string Namespace, string Name)> trustedTypeRefs = CollectTrustedTypeRefs(pluginDir);

        // Pre-load scan: enforce plugin security rules, accumulate all violations and report at once
        if (!ScanForbiddenReferences(dllPath, out List<string> violations, trustedTypeRefs))
        {
            _logger.Error(null,
                "Plugin rejected: {0} violated {1} security rule(s):\n  - {2}",
                dllPath,
                violations.Count,
                string.Join("\n  - ", violations));
            return;
        }

        try
        {
            var context = new PluginLoadContext(dirName, pluginDir, isCollectible: true);
            Assembly assembly = context.LoadFromAssemblyPath(dllPath);

            Type[] pluginTypes = assembly.GetTypes()
                .Where(t => typeof(IPlugin).IsAssignableFrom(t) && t != typeof(IPlugin) && !t.IsAbstract)
                .ToArray();

            if (pluginTypes.Length == 0)
            {
                _logger.Warn(null, "No IPlugin implementation found in {0}", dllPath);
                context.Unload();
                return;
            }

            if (pluginTypes.Length > 1)
            {
                string typeNames = string.Join(", ", pluginTypes.Select(t => t.Name));
                _logger.Error(null, "Multiple IPlugin implementations found in {0}: [{1}]. Only one is allowed.", dllPath, typeNames);
                context.Unload();
                return;
            }

            IPlugin plugin = (IPlugin)Activator.CreateInstance(pluginTypes[0])!;
            plugin.OnLoad();

            _loadedPlugins.Add(new LoadedPlugin(plugin, context, dllPath));
            _logger.Info(null, "Plugin loaded: {0} v{1} from {2}", plugin.Id, plugin.Version, dirName);
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to load plugin from {0}: {1}", pluginDir, ex.Message);
        }
    }

    public void NotifyAllStarted()
    {
        foreach (var loaded in _loadedPlugins)
        {
            try
            {
                loaded.Plugin.OnStart();
                _logger.Debug(null, "Plugin started: {0}", loaded.Plugin.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(null, "Plugin OnStart failed for {0}: {1}", loaded.Plugin.Id, ex.Message);
            }
        }
    }

    public void NotifyAllStopping()
    {
        foreach (var loaded in _loadedPlugins)
        {
            try
            {
                loaded.Plugin.OnStop();
                _logger.Debug(null, "Plugin stopped: {0}", loaded.Plugin.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(null, "Plugin OnStop failed for {0}: {1}", loaded.Plugin.Id, ex.Message);
            }
        }
    }

    public void UnloadAll()
    {
        foreach (var loaded in _loadedPlugins)
        {
            try
            {
                loaded.Plugin.OnUnload();
                _logger.Debug(null, "Plugin unloaded: {0}", loaded.Plugin.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(null, "Plugin OnUnload failed for {0}: {1}", loaded.Plugin.Id, ex.Message);
            }
        }

        _loadedPlugins.Clear();
        _logger.Info(null, "All plugins unloaded");
    }

    private record LoadedPlugin(IPlugin Plugin, AssemblyLoadContext Context, string DllPath);

    /// <summary>
    /// Trusted open-source dependency assembly whitelist: the scanner passes these assemblies directly,
    /// avoiding false positives from their internal use of MemoryMarshal / Unsafe / HttpClient / GeneratedCode
    /// and other optimization or implementation details.
    /// <para>Identification basis: <c>AssemblyDefinition.Name</c> in PE metadata, not the DLL filename,
    /// preventing disguise as a trusted library via renaming.</para>
    /// <para>Admission criteria: ① Widely-used open-source projects (MIT / Apache 2.0 / BSD permissive licenses);
    /// ② Publicly auditable source code; ③ Mainstream NuGet packages maintained by trusted vendors/communities.</para>
    /// <para>To add new members, simply add a line to the array — after all, they're open-source and self-auditable.</para>
    /// </summary>
    private static readonly HashSet<string> TrustedAssemblies = new(StringComparer.Ordinal)
    {
        // —— Serialization ——
        "Google.Protobuf",                 // Protocol Buffers runtime (uses MemoryMarshal/Unsafe for zero-copy)
        "protobuf-net",                    // Marc Gravell's protobuf implementation
        "Newtonsoft.Json",                 // Json.NET, heavy reflection
        "MessagePack",                     // neuecc MessagePack-CSharp
        "MessagePack.Annotations",
        "YamlDotNet",                      // YAML parser

        // —— Logging ——
        "Serilog",
        "Serilog.Sinks.Console",
        "Serilog.Sinks.File",
        "NLog",

        // —— Microsoft.Extensions.* abstractions (pure interfaces, no I/O) ——
        "Microsoft.Extensions.Logging.Abstractions",
        "Microsoft.Extensions.DependencyInjection.Abstractions",
        "Microsoft.Extensions.Configuration.Abstractions",
        "Microsoft.Extensions.Options",
        "Microsoft.Extensions.Primitives",

        // —— Data access / mapping ——
        "Dapper",
        "AutoMapper",

        // —— Validation and message dispatch ——
        "FluentValidation",
        "MediatR",
        "MediatR.Contracts",
    };

    /// <summary>
    /// Forbidden namespace prefixes. All types under any namespace matching a prefix in this array are forbidden.
    /// </summary>
    private static readonly string[] ForbiddenNamespaces =
    [
        // Rule 1: File I/O (corresponds to DiskExecutor)
        // Note: System.IO.Stream / MemoryStream / Compression.* / BinaryReader/Writer and other types
        //       that don't directly perform file I/O are exempted via Rule 7;
        //       plugins can obtain FileStream through PermissionedStreamFactory.
        "System.IO",

        // Rule 2: Network access (corresponds to NetworkExecutor)
        "System.Net.Http",             // HttpClient / HttpRequestMessage / HttpResponseMessage
        "System.Net.WebSockets",       // ClientWebSocket / WebSocket
        "System.Net.Sockets",          // TcpClient / UdpClient / Socket and other raw sockets
        "System.Net.Mail",             // SmtpClient / MailMessage
        "System.Net.NetworkInformation", // Ping / NetworkInterface
        "System.Net.Security",         // SslStream and other types carrying network streams

        // Rule 4: Compilation / dynamic code generation (corresponds to Compilation module)
        "Microsoft.CodeAnalysis",      // Roslyn compiler (includes CSharp, VisualBasic, Syntax sub-namespaces)
        "System.Reflection.Emit",      // Runtime IL emission (DynamicMethod / AssemblyBuilder etc.)
        "System.Runtime.Loader",       // AssemblyLoadContext: prevent plugins from bypassing scan to dynamically load another DLL
        "System.CodeDom.Compiler",     // Legacy CodeDom compiler (CSharpCodeProvider etc.)
        "Microsoft.CSharp.RuntimeBinder", // Runtime binder for dynamic type (can be used to bypass type checking)

        // Rule 6: Registry and Win32 interop
        "Microsoft.Win32",             // Registry / RegistryKey / SystemEvents (includes SafeHandles sub-namespace)
    ];

    /// <summary>
    /// Forbidden specific types (where the namespace itself is not suitable for blanket prohibition, only certain types are forbidden).
    /// </summary>
    private static readonly (string Namespace, string TypeName)[] ForbiddenTypes =
    [
        // Rule 2: HTTP / Web / FTP / DNS related types in System.Net
        ("System.Net", "HttpWebRequest"),
        ("System.Net", "HttpWebResponse"),
        ("System.Net", "HttpListener"),
        ("System.Net", "HttpListenerContext"),
        ("System.Net", "HttpListenerRequest"),
        ("System.Net", "HttpListenerResponse"),
        ("System.Net", "HttpListenerPrefixCollection"),
        ("System.Net", "HttpListenerTimeoutManager"),
        ("System.Net", "WebClient"),
        ("System.Net", "WebRequest"),
        ("System.Net", "WebResponse"),
        ("System.Net", "FtpWebRequest"),
        ("System.Net", "FtpWebResponse"),
        ("System.Net", "Dns"),
        ("System.Net", "DnsEndPoint"),

        // Rule 3: Process / command line (corresponds to CommandLineExecutor)
        // Safe types like Stopwatch / Debug / Trace / Activity under System.Diagnostics need to be preserved, so only Process-related types are forbidden
        ("System.Diagnostics", "Process"),
        ("System.Diagnostics", "ProcessStartInfo"),
        ("System.Diagnostics", "ProcessThread"),
        ("System.Diagnostics", "ProcessThreadCollection"),
        ("System.Diagnostics", "ProcessModule"),
        ("System.Diagnostics", "ProcessModuleCollection"),
        ("System.Diagnostics", "ProcessPriorityClass"),
        ("System.Diagnostics", "ProcessWindowStyle"),

        // Rule 4: Compilation-related — for cases where the namespace itself cannot be blanket-prohibited
        // Note: System.Reflection.Assembly / AssemblyName / System.Linq.Expressions.Expression are referenced
        //       extensively in regular code (typeof(X).Assembly, LINQ IQueryable, etc.), with high risk of
        //       false positives, so they are not included in the type blacklist. Instead, namespace-level
        //       prohibition of System.Reflection.Emit and System.Runtime.Loader intercepts truly dangerous usage.
        ("System", "AppDomain"),       // AppDomain.Load / CreateInstance can dynamically load assemblies

        // Rule 5: Unsafe code markers and native interop
        // Module-level attribute UnverifiableCodeAttribute is checked separately in ScanUnsafeMarkers;
        // here we enumerate key types for P/Invoke and native memory operations.
        ("System.Runtime.InteropServices", "DllImportAttribute"),       // [DllImport] declares native export
        ("System.Runtime.InteropServices", "UnmanagedFunctionPointerAttribute"), // Native function pointer
        ("System.Runtime.InteropServices", "SuppressGCTransitionAttribute"),     // Skip GC transition
        ("System.Runtime.InteropServices", "Marshal"),                  // Managed/unmanaged memory copy
        ("System.Runtime.InteropServices", "MemoryMarshal"),            // Span<T> and raw memory interchange
        ("System.Runtime.InteropServices", "NativeMemory"),             // Native heap malloc/free
        ("System.Runtime.InteropServices", "NativeLibrary"),            // Dynamically load native shared library
        ("System.Runtime.InteropServices", "GCHandle"),                 // Pin managed object and expose pointer
        ("System.Runtime.InteropServices", "SafeHandle"),               // Native resource handle base class
        ("System.Runtime.CompilerServices", "Unsafe"),                  // System.Runtime.CompilerServices.Unsafe helper class
        ("System.Security", "UnverifiableCodeAttribute"),               // Unverifiable code attribute
        ("System.Security", "SuppressUnmanagedCodeSecurityAttribute"),  // Suppress native code security check
    ];

    /// <summary>
    /// Rule 7: Whitelist of types in the System.IO namespace that **do not directly perform file I/O**.
    /// These types are exempted even if they fall under the System.IO prefix match in <c>ForbiddenNamespaces</c>.
    /// <para>Criteria: ① Pure in-memory operations; ② Compression/decompression streams; ③ Enum/exception classes; ④ Wrapper streams (do not directly open files).</para>
    /// <para>Types that actually perform file I/O such as FileStream / File / Directory are NOT in the exemption list.</para>
    /// </summary>
    private static readonly HashSet<string> SystemIOAllowedTypes = new(StringComparer.Ordinal)
    {
        // —— Stream abstractions and pure in-memory streams ——
        "Stream",             // Abstract base class
        "MemoryStream",       // Pure in-memory operation

        // —— Compression/decompression (based on memory streams, do not directly read files) ——
        "ZLibStream",
        "GZipStream",
        "DeflateStream",
        "CompressionMode",    // Enum
        "CompressionLevel",   // Enum

        // —— Binary read/write wrappers (wrap any Stream, do not directly open files) ——
        "BinaryReader",
        "BinaryWriter",

        // —— Enums and basic types ——
        "SeekOrigin",         // Enum
        "FileMode",           // Enum (used with PermissionedStreamFactory)
        "FileAccess",         // Enum (used with PermissionedStreamFactory)
        "FileShare",          // Enum (used with PermissionedStreamFactory)

        // —— Exception classes ——
        "IOException",        // Exception base class
        "InvalidDataException", // Common exception for compression/deserialization
        "EndOfStreamException", // End-of-stream read exception
    };

    /// <summary>
    /// Forbidden specific members (namespace and type are allowed, but specific methods/properties are forbidden).
    /// Used to block core bypass channels that dynamically load arbitrary types via reflection strings.
    /// </summary>
    private static readonly (string Namespace, string TypeName, string MemberName)[] ForbiddenMembers =
    [
        // Rule 6: Reflection dynamic loading — core bypass channel for TypeRef scanning
        ("System", "Type", "GetType"),                      // Type.GetType("System.IO.File, ...") static overload
        ("System", "Activator", "CreateInstance"),          // Activator.CreateInstance(Type, ...) arbitrary type instantiation
        ("System", "Activator", "CreateInstanceFrom"),      // Create instance from DLL path
        ("System.Reflection", "Assembly", "Load"),          // Assembly.Load(string / byte[] / AssemblyName)
        ("System.Reflection", "Assembly", "LoadFile"),
        ("System.Reflection", "Assembly", "LoadFrom"),
        ("System.Reflection", "Assembly", "UnsafeLoadFrom"),
        ("System.Reflection", "Assembly", "LoadWithPartialName"),
        ("System.Reflection", "Assembly", "ReflectionOnlyLoad"),
        ("System.Reflection", "Assembly", "ReflectionOnlyLoadFrom"),
        ("System.Reflection", "Assembly", "GetType"),       // asm.GetType("System.IO.File")

        // Rule 6: Environment dangerous methods (entire type not forbidden, only these members to avoid false positives on NewLine/UserName etc.)
        ("System", "Environment", "Exit"),                  // Force process exit
        ("System", "Environment", "FailFast"),              // Immediately terminate process
        ("System", "Environment", "SetEnvironmentVariable"),// Tamper with environment variables

        // Rule 6: Expression tree compilation — can compile arbitrary expressions into executable delegates
        ("System.Linq.Expressions", "LambdaExpression", "Compile"),
        ("System.Linq.Expressions", "Expression", "Compile"),
    ];

    /// <summary>
    /// Blacklist of string constant prefixes for ldstr instructions in IL (scanning the #US heap).
    /// Used to intercept bypass techniques that locate forbidden types via reflection strings, e.g., Type.GetType("System.IO.File, System.Runtime").
    /// Note: May produce minor false positives on log/error messages containing the same-name strings; this is an accepted trade-off for closing the loophole.
    /// </summary>
    private static readonly string[] ForbiddenStringPrefixes =
    [
        "System.IO.",                  // File system types
        "System.Net.Http",             // HTTP
        "System.Net.WebSockets",       // WebSocket
        "System.Net.Sockets",          // Raw sockets
        "System.Net.Mail",             // SMTP
        "System.Net.NetworkInformation", // Network probing
        "System.Net.Security",         // SslStream
        "System.Diagnostics.Process",  // Process / command line
        "Microsoft.CodeAnalysis",      // Roslyn
        "System.Reflection.Emit",      // IL emission
        "System.Runtime.Loader",       // AssemblyLoadContext
        "System.CodeDom.Compiler",     // Legacy CodeDom
        "Microsoft.Win32",             // Registry
    ];

    /// <summary>
    /// Scans all whitelisted DLLs in the plugin directory, collects their TypeRef tables,
    /// and builds a set of trusted type references. These TypeRefs are exempted during
    /// the main DLL scan to prevent false positives from whitelisted library implementation details.
    /// </summary>
    /// <param name="pluginDir">Plugin directory path</param>
    /// <returns>Set of trusted (namespace, type name) pairs</returns>
    private static HashSet<(string Namespace, string Name)> CollectTrustedTypeRefs(string pluginDir)
    {
        var trusted = new HashSet<(string, string)>();

        foreach (string dllFile in Directory.GetFiles(pluginDir, "*.dll"))
        {
            try
            {
                using var stream = File.OpenRead(dllFile);
                using var peReader = new PEReader(stream);
                if (!peReader.HasMetadata)
                    continue;

                MetadataReader reader = peReader.GetMetadataReader();
                if (!reader.IsAssembly)
                    continue;

                string asmName = reader.GetString(reader.GetAssemblyDefinition().Name);
                if (!TrustedAssemblies.Contains(asmName))
                    continue;

                // Collect all TypeRefs from this whitelisted DLL
                foreach (TypeReferenceHandle handle in reader.TypeReferences)
                {
                    TypeReference typeRef = reader.GetTypeReference(handle);
                    string ns = reader.GetString(typeRef.Namespace);
                    string name = reader.GetString(typeRef.Name);
                    trusted.Add((ns, name));
                }

                _logger.Debug(null, "Collected {0} trusted TypeRef(s) from {1}", trusted.Count, asmName);
            }
            catch (Exception ex)
            {
                _logger.Warn(null, "Failed to collect trusted TypeRefs from {0}: {1}", dllFile, ex.Message);
            }
        }

        return trusted;
    }

    /// <summary>
    /// Scans plugin DLL metadata and collects all violations. Scan flow (no longer short-circuits on first violation):
    ///   0. Whitelist early exit — open-source libraries matching <see cref="TrustedAssemblies"/> are passed directly
    ///   0.5. Transitive exemption — skip type references in <paramref name="trustedTypeRefs"/>
    ///   1. TypeRef table — directly referenced external types
    ///   2. ExportedType table — prevent bypass via type forwarding
    ///   3. MemberRef table — exact match by (type, member name) for reflection dynamic loading / Environment.Exit /
    ///      Expression.Compile and other member-level dangerous methods
    ///   4. Module-level / assembly-level [UnverifiableCode] attribute and P/Invoke method flags
    ///   5. #US user string heap — intercept reflection string bypasses like Type.GetType("System.IO.File")
    /// All violations are accumulated into the violations list; the caller writes a single Error-level log entry.
    /// </summary>
    /// <param name="dllPath">Plugin DLL path</param>
    /// <param name="violations">List of all violation descriptions (empty list means passed)</param>
    /// <param name="trustedTypeRefs">TypeRef set from whitelisted DLLs (transitive exemption)</param>
    /// <returns>Returns true if scan passes, false if any violations exist</returns>
    private static bool ScanForbiddenReferences(string dllPath, out List<string> violations, HashSet<(string Namespace, string Name)> trustedTypeRefs)
    {
        violations = new List<string>();
        try
        {
            using var stream = File.OpenRead(dllPath);
            using var peReader = new PEReader(stream);
            if (!peReader.HasMetadata)
            {
                return true;
            }

            MetadataReader reader = peReader.GetMetadataReader();

            // 0. Whitelist early exit: trusted open-source libraries are passed directly without any rule checks.
            //    Based on AssemblyDefinition.Name in metadata; modules/nameless assemblies that can't be read won't be mistakenly passed.
            if (reader.IsAssembly)
            {
                string asmName = reader.GetString(reader.GetAssemblyDefinition().Name);
                if (TrustedAssemblies.Contains(asmName))
                {
                    _logger.Debug(null, "Skipping security scan for trusted assembly: {0}", asmName);
                    return true;
                }
            }

            // 1. TypeRef table
            foreach (TypeReferenceHandle handle in reader.TypeReferences)
            {
                TypeReference typeRef = reader.GetTypeReference(handle);
                string ns = reader.GetString(typeRef.Namespace);
                string name = reader.GetString(typeRef.Name);

                // 0.5. Transitive exemption: skip TypeRefs introduced by whitelisted DLLs
                if (trustedTypeRefs.Contains((ns, name)))
                {
                    continue;
                }

                if (IsForbidden(ns, name))
                {
                    violations.Add($"[TypeRef] {FormatTypeName(ns, name)}");
                }
            }

            // 2. ExportedType table
            foreach (ExportedTypeHandle handle in reader.ExportedTypes)
            {
                ExportedType exported = reader.GetExportedType(handle);
                string ns = reader.GetString(exported.Namespace);
                string name = reader.GetString(exported.Name);
                if (IsForbidden(ns, name))
                {
                    violations.Add($"[ExportedType] {FormatTypeName(ns, name)}");
                }
            }

            // 3. MemberRef table — member-level blacklist
            ScanMemberReferences(reader, violations);

            // 4. Unsafe code markers + P/Invoke
            ScanUnsafeMarkers(reader, violations);

            // 5. IL string constants (#US heap)
            ScanUserStrings(reader, violations);

            return violations.Count == 0;
        }
        catch (Exception ex)
        {
            _logger.Error(null, "Failed to scan plugin DLL {0}: {1}", dllPath, ex.Message);
            violations.Add($"<scan error: {ex.Message}>");
            return false;
        }
    }

    /// <summary>
    /// Scans the MemberRef table to intercept calls to forbidden members (e.g., Assembly.Load / Type.GetType / Environment.Exit).
    /// Only matches method references whose Parent is a TypeReference, avoiding internal methods.
    /// </summary>
    private static void ScanMemberReferences(MetadataReader reader, List<string> violations)
    {
        foreach (MemberReferenceHandle handle in reader.MemberReferences)
        {
            MemberReference memberRef = reader.GetMemberReference(handle);
            if (memberRef.GetKind() != MemberReferenceKind.Method)
            {
                continue;
            }

            EntityHandle parent = memberRef.Parent;
            if (parent.Kind != HandleKind.TypeReference)
            {
                // TypeSpecification (generic instances) / ModuleReference etc. are not processed for now
                continue;
            }

            TypeReference parentType = reader.GetTypeReference((TypeReferenceHandle)parent);
            string ns = reader.GetString(parentType.Namespace);
            string typeName = reader.GetString(parentType.Name);
            string memberName = reader.GetString(memberRef.Name);

            if (IsForbiddenMember(ns, typeName, memberName))
            {
                violations.Add($"[MemberRef] {FormatTypeName(ns, typeName)}::{memberName}");
            }
        }
    }

    /// <summary>
    /// Scans the #US (UserString) heap — i.e., all string operands of ldstr instructions in IL.
    /// Matching any forbidden prefix is considered a violation, used to intercept reflection string dynamic loading.
    /// </summary>
    private static void ScanUserStrings(MetadataReader reader, List<string> violations)
    {
        // Start from offset 0 (Nil); GetNextHandle returns the first actual user string in the heap
        UserStringHandle handle = MetadataTokens.UserStringHandle(0);
        while (true)
        {
            handle = reader.GetNextHandle(handle);
            if (handle.IsNil)
            {
                break;
            }

            string value = reader.GetUserString(handle);
            if (string.IsNullOrEmpty(value))
            {
                continue;
            }

            foreach (string prefix in ForbiddenStringPrefixes)
            {
                if (value.StartsWith(prefix, StringComparison.Ordinal))
                {
                    string display = value.Length > 80 ? value.Substring(0, 80) + "..." : value;
                    violations.Add($"[ILString] \"{display}\" matches forbidden prefix \"{prefix}\"");
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Checks for unsafe code markers:
    ///   - Module-level / assembly-level [UnverifiableCode] attribute (compilation product of unsafe blocks)
    ///   - Methods with MethodAttributes.PinvokeImpl flag ([DllImport] declarations)
    /// Appends to violations on discovery; does not short-circuit.
    /// </summary>
    private static void ScanUnsafeMarkers(MetadataReader reader, List<string> violations)
    {
        if (reader.IsAssembly)
        {
            AssemblyDefinition asm = reader.GetAssemblyDefinition();
            if (ContainsAttribute(reader, asm.GetCustomAttributes(), "System.Security", "UnverifiableCodeAttribute"))
            {
                violations.Add("[UnsafeMarker] [assembly: System.Security.UnverifiableCode]");
            }
        }

        ModuleDefinition mod = reader.GetModuleDefinition();
        if (ContainsAttribute(reader, mod.GetCustomAttributes(), "System.Security", "UnverifiableCodeAttribute"))
        {
            violations.Add("[UnsafeMarker] [module: System.Security.UnverifiableCode]");
        }

        foreach (MethodDefinitionHandle handle in reader.MethodDefinitions)
        {
            MethodDefinition method = reader.GetMethodDefinition(handle);
            if ((method.Attributes & MethodAttributes.PinvokeImpl) != 0)
            {
                string methodName = reader.GetString(method.Name);
                violations.Add($"[PInvoke] {methodName} (native interop)");
            }
        }
    }

    /// <summary>
    /// Checks whether the custom attribute collection contains an attribute with the specified namespace and class name.
    /// </summary>
    private static bool ContainsAttribute(MetadataReader reader, CustomAttributeHandleCollection handles, string ns, string name)
    {
        foreach (CustomAttributeHandle handle in handles)
        {
            CustomAttribute attr = reader.GetCustomAttribute(handle);
            EntityHandle typeHandle;
            switch (attr.Constructor.Kind)
            {
                case HandleKind.MemberReference:
                    typeHandle = reader.GetMemberReference((MemberReferenceHandle)attr.Constructor).Parent;
                    break;
                case HandleKind.MethodDefinition:
                    typeHandle = reader.GetMethodDefinition((MethodDefinitionHandle)attr.Constructor).GetDeclaringType();
                    break;
                default:
                    continue;
            }

            string attrNs;
            string attrName;
            if (typeHandle.Kind == HandleKind.TypeReference)
            {
                TypeReference tr = reader.GetTypeReference((TypeReferenceHandle)typeHandle);
                attrNs = reader.GetString(tr.Namespace);
                attrName = reader.GetString(tr.Name);
            }
            else if (typeHandle.Kind == HandleKind.TypeDefinition)
            {
                TypeDefinition td = reader.GetTypeDefinition((TypeDefinitionHandle)typeHandle);
                attrNs = reader.GetString(td.Namespace);
                attrName = reader.GetString(td.Name);
            }
            else
            {
                continue;
            }

            if (attrNs == ns && attrName == name)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified type reference is forbidden (namespace blanket prohibition or specific type prohibition).
    /// </summary>
    private static bool IsForbidden(string ns, string name)
    {
        if (!string.IsNullOrEmpty(ns))
        {
            // Rule A: Namespace blanket prohibition
            foreach (string forbidden in ForbiddenNamespaces)
            {
                if (ns == forbidden || ns.StartsWith(forbidden + ".", StringComparison.Ordinal))
                {
                    // Rule 7: System.IO namespace exemption — types that don't directly perform file I/O are allowed
                    if (ns.StartsWith("System.IO", StringComparison.Ordinal) && SystemIOAllowedTypes.Contains(name))
                    {
                        continue; // Skip this namespace rule, continue checking other rules
                    }
                    return true;
                }
            }
        }

        // Rule B: Specific type prohibition
        foreach (var (forbiddenNs, forbiddenName) in ForbiddenTypes)
        {
            if (ns == forbiddenNs && name == forbiddenName)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified (namespace, type, member) triple is in the ForbiddenMembers blacklist.
    /// </summary>
    private static bool IsForbiddenMember(string ns, string typeName, string memberName)
    {
        foreach (var (forbiddenNs, forbiddenType, forbiddenMember) in ForbiddenMembers)
        {
            if (ns == forbiddenNs && typeName == forbiddenType && memberName == forbiddenMember)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Formats a type full name (namespace + class name).
    /// </summary>
    private static string FormatTypeName(string ns, string name)
    {
        return string.IsNullOrEmpty(ns) ? name : $"{ns}.{name}";
    }
}

/// <summary>
/// Custom AssemblyLoadContext that supports loading dependency assemblies from the plugin directory
/// </summary>
internal sealed class PluginLoadContext : AssemblyLoadContext
{
    private readonly string _pluginDirectory;

    public PluginLoadContext(string name, string pluginDirectory, bool isCollectible)
        : base(name, isCollectible)
    {
        _pluginDirectory = pluginDirectory;
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        if (assemblyName.Name == null)
            return null;

        // 1. For shared dependencies already loaded by the main program (e.g., Core, Common), use the main program's version
        var loadedAssemblies = AssemblyLoadContext.Default.Assemblies
            .FirstOrDefault(a => a.GetName().Name == assemblyName.Name);
        
        if (loadedAssemblies != null)
        {
            // Shared dependency: use the main program's assembly directly
            return loadedAssemblies;
        }

        // 2. For plugin-specific dependencies (e.g., Google.Protobuf), load from the plugin directory
        string dllPath = Path.Combine(_pluginDirectory, $"{assemblyName.Name}.dll");
        if (File.Exists(dllPath))
        {
            return LoadFromAssemblyPath(dllPath);
        }

        // 3. Not found anywhere, fall back to the default load context
        return null;
    }
}
