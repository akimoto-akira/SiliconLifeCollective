// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SiliconLife.Collective;

/// <summary>
/// Result of a dynamic compilation attempt.
/// </summary>
public sealed class CompilationResult
{
    /// <summary>Whether compilation succeeded</summary>
    public bool Success { get; }

    /// <summary>The compiled Type if successful, null otherwise</summary>
    public Type? CompiledType { get; }

    /// <summary>Compilation errors if failed</summary>
    public List<string> Errors { get; }

    /// <summary>Security scan result</summary>
    public SecurityScanResult? SecurityResult { get; }

    /// <summary>The generated Assembly if successful (caller must manage lifetime)</summary>
    public Assembly? GeneratedAssembly { get; }

    public CompilationResult(bool success, Type? compiledType, List<string> errors,
        SecurityScanResult? securityResult = null, Assembly? generatedAssembly = null)
    {
        Success = success;
        CompiledType = compiledType;
        Errors = errors;
        SecurityResult = securityResult;
        GeneratedAssembly = generatedAssembly;
    }
}

/// <summary>
/// Dynamic compilation executor based on Roslyn.
/// Compiles C# source code in-memory with security scanning.
/// </summary>
public class DynamicCompilationExecutor
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<DynamicCompilationExecutor>();
    private readonly string[] _allowedAssemblyPaths;

    /// <summary>
    /// Creates a new DynamicCompilationExecutor.
    /// </summary>
    /// <param name="hostAssemblyPath">Path to the host assembly (SiliconLife.Core) to reference</param>
    public DynamicCompilationExecutor(string? hostAssemblyPath = null)
    {
        if (hostAssemblyPath != null)
        {
            _allowedAssemblyPaths = [hostAssemblyPath];
        }
        else
        {
            // Default: reference the assembly containing SiliconLifeBase
            _allowedAssemblyPaths =
            [
                typeof(SiliconBeingBase).Assembly.Location
            ];
        }
    }

    /// <summary>
    /// Compiles source code and returns the first type that matches the given base type.
    /// Performs security scan before compilation.
    /// </summary>
    /// <typeparam name="TBase">The base type the compiled code must inherit from</typeparam>
    /// <param name="sourceCode">The C# source code to compile</param>
    /// <param name="typeNameHint">Optional hint for the expected type name</param>
    /// <returns>Compilation result with the matching type</returns>
    public CompilationResult Compile<TBase>(string sourceCode, string? typeNameHint = null)
        where TBase : class
    {
        ArgumentNullException.ThrowIfNull(sourceCode);

        _logger.Info("Starting compilation for type hint: {0}", typeNameHint ?? "(auto-detect)");

        SecurityScanResult scanResult = SecurityScanner.Scan(sourceCode);
        if (!scanResult.Passed)
        {
            _logger.Error("Security scan failed: {0}", string.Join("; ", scanResult.Violations));
            return new CompilationResult(
                false, null,
                ["Security scan failed: " + string.Join("; ", scanResult.Violations)],
                scanResult);
        }

        // Step 2: Parse and compile
        CSharpParseOptions parseOptions = CSharpParseOptions.Default
            .WithLanguageVersion(LanguageVersion.CSharp13);

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode, options: parseOptions);

        // Collect referenced assemblies
        var referencedAssemblies = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(TBase).Assembly.Location),
        };

        // Add System.Text.Json
        Type? jsonType = Type.GetType("System.Text.Json.JsonSerializer, System.Text.Json");
        if (jsonType != null)
        {
            referencedAssemblies.Add(MetadataReference.CreateFromFile(jsonType.Assembly.Location));
        }

        // Add System.Text.RegularExpressions
        Type? regexType = Type.GetType("System.Text.RegularExpressions.Regex, System.Text.RegularExpressions");
        if (regexType != null)
        {
            referencedAssemblies.Add(MetadataReference.CreateFromFile(regexType.Assembly.Location));
        }

        // Validate references
        var referencedNames = referencedAssemblies
            .Select(r => Path.GetFileName(r.Display))
            .ToList();

        (bool refsValid, List<string> unauthorized) = SecurityScanner.ValidateReferences(referencedNames);
        if (!refsValid)
        {
            _logger.Error("Unauthorized assembly references: {0}", string.Join(", ", unauthorized));
            return new CompilationResult(
                false, null,
                [$"Unauthorized assembly references: {string.Join(", ", unauthorized)}"],
                scanResult);
        }

        // Compile
        string assemblyName = $"DynCompile_{Guid.NewGuid():N}";
        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            [syntaxTree],
            referencedAssemblies,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOptimizationLevel(OptimizationLevel.Release)
                .WithAllowUnsafe(false));

        using var ms = new System.IO.MemoryStream();
        var emitResult = compilation.Emit(ms);

        if (!emitResult.Success)
        {
            var errors = emitResult.Diagnostics
                .Where(d => d.Severity == DiagnosticSeverity.Error)
                .Select(d => d.ToString())
                .ToList();

            _logger.Error("Compilation failed with {0} errors", errors.Count);
            return new CompilationResult(false, null, errors, scanResult);
        }

        ms.Seek(0, System.IO.SeekOrigin.Begin);

        // Load the compiled assembly
        Assembly assembly = Assembly.Load(ms.ToArray());

        // Find the type that inherits from TBase
        Type? targetType = null;
        if (!string.IsNullOrEmpty(typeNameHint))
        {
            targetType = assembly.GetType(typeNameHint);
        }

        if (targetType == null || !typeof(TBase).IsAssignableFrom(targetType))
        {
            // Fallback: find any type inheriting TBase
            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsAbstract && typeof(TBase).IsAssignableFrom(type))
                {
                    targetType = type;
                    break;
                }
            }
        }

        if (targetType == null)
        {
            _logger.Warn("No type inheriting {0} found in compiled code", typeof(TBase).Name);
            return new CompilationResult(
                false, null,
                [$"No type inheriting {typeof(TBase).Name} found in compiled code."],
                scanResult, assembly);
        }

        _logger.Info("Compilation successful: type={0}, assembly={1}", targetType.Name, assemblyName);
        return new CompilationResult(true, targetType, [], scanResult, assembly);
    }

    /// <summary>
    /// Compiles source code for an IPermissionCallback implementation.
    /// This is the stealth channel — uses the same mechanism but targets a different interface.
    /// </summary>
    /// <param name="sourceCode">The C# source code implementing IPermissionCallback</param>
    /// <returns>Compilation result with the callback type</returns>
    public CompilationResult CompilePermissionCallback(string sourceCode)
    {
        ArgumentNullException.ThrowIfNull(sourceCode);

        _logger.Info("Starting permission callback compilation");

        SecurityScanResult scanResult = SecurityScanner.Scan(sourceCode);
        if (!scanResult.Passed)
        {
            _logger.Error("Security scan failed for permission callback: {0}", string.Join("; ", scanResult.Violations));
            return new CompilationResult(
                false, null,
                ["Security scan failed: " + string.Join("; ", scanResult.Violations)],
                scanResult);
        }

        // Parse and compile
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode,
            CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp13));

        var referencedAssemblies = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IPermissionCallback).Assembly.Location),
        };

        string assemblyName = $"DynPermCompile_{Guid.NewGuid():N}";
        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            [syntaxTree],
            referencedAssemblies,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOptimizationLevel(OptimizationLevel.Release)
                .WithAllowUnsafe(false));

        using var ms = new System.IO.MemoryStream();
        var emitResult = compilation.Emit(ms);

        if (!emitResult.Success)
        {
            var errors = emitResult.Diagnostics
                .Where(d => d.Severity == DiagnosticSeverity.Error)
                .Select(d => d.ToString())
                .ToList();

            _logger.Error("Permission callback compilation failed with {0} errors", errors.Count);
            return new CompilationResult(false, null, errors, scanResult);
        }

        ms.Seek(0, System.IO.SeekOrigin.Begin);
        Assembly assembly = Assembly.Load(ms.ToArray());

        // Find IPermissionCallback implementation
        Type? targetType = null;
        foreach (Type type in assembly.GetTypes())
        {
            if (!type.IsAbstract && typeof(IPermissionCallback).IsAssignableFrom(type))
            {
                targetType = type;
                break;
            }
        }

        if (targetType == null)
        {
            _logger.Warn("No type implementing IPermissionCallback found in compiled code");
            return new CompilationResult(
                false, null,
                ["No type implementing IPermissionCallback found in compiled code."],
                scanResult, assembly);
        }

        _logger.Info("Permission callback compilation successful: type={0}", targetType.Name);
        return new CompilationResult(true, targetType, [], scanResult, assembly);
    }

    /// <summary>
    /// Creates an instance of the compiled type using its parameterless constructor.
    /// </summary>
    /// <param name="result">The compilation result with a valid CompiledType</param>
    /// <returns>The instantiated object, or null if instantiation fails</returns>
    public static object? CreateInstance(CompilationResult result)
    {
        if (result?.CompiledType == null)
        {
            return null;
        }

        try
        {
            return Activator.CreateInstance(result.CompiledType);
        }
        catch
        {
            return null;
        }
    }
}
