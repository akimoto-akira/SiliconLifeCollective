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
/// Delegates actual compilation to CompilationCore.
/// </summary>
public class DynamicCompilationExecutor
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<DynamicCompilationExecutor>();
    private readonly CompilationCore _compilationCore;
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

        // Initialize compilation core with SiliconBeingBase assembly
        _compilationCore = new CompilationCore(typeof(SiliconBeingBase).Assembly);
    }

    /// <summary>
    /// Compiles source code and returns the first type that matches the given base type.
    /// Performs security scan before compilation, then delegates to CompilationCore.
    /// </summary>
    /// <typeparam name="TBase">The base type the compiled code must inherit from</typeparam>
    /// <param name="sourceCode">The C# source code to compile</param>
    /// <param name="typeNameHint">Optional hint for the expected type name</param>
    /// <returns>Compilation result with the matching type</returns>
    public CompilationResult Compile<TBase>(string sourceCode, string? typeNameHint = null)
        where TBase : class
    {
        ArgumentNullException.ThrowIfNull(sourceCode);

        _logger.Info(null, "Starting compilation for type hint: {0}", typeNameHint ?? "(auto-detect)");

        // Security scan before compilation
        SecurityScanResult scanResult = SecurityScanner.Scan(sourceCode);
        if (!scanResult.Passed)
        {
            _logger.Error(null, "Security scan failed: {0}", string.Join("; ", scanResult.Violations));
            return new CompilationResult(
                false, null,
                ["Security scan failed: " + string.Join("; ", scanResult.Violations)],
                scanResult);
        }

        // Validate assembly references
        Type? jsonType = Type.GetType("System.Text.Json.JsonSerializer, System.Text.Json");
        Type? regexType = Type.GetType("System.Text.RegularExpressions.Regex, System.Text.RegularExpressions");
        
        var referencedNames = new List<string> { "mscorlib.dll", "System.Console.dll", "System.Collections.dll" };
        if (jsonType != null) referencedNames.Add("System.Text.Json.dll");
        if (regexType != null) referencedNames.Add("System.Text.RegularExpressions.dll");
        referencedNames.Add(typeof(TBase).Assembly.GetName().Name + ".dll");

        (bool refsValid, List<string> unauthorized) = SecurityScanner.ValidateReferences(referencedNames);
        if (!refsValid)
        {
            _logger.Error(null, "Unauthorized assembly references: {0}", string.Join(", ", unauthorized));
            return new CompilationResult(
                false, null,
                [$"Unauthorized assembly references: {string.Join(", ", unauthorized)}"],
                scanResult);
        }

        // Delegate to compilation core
        CompilationResult result = _compilationCore.Compile(
            sourceCode,
            assembly =>
            {
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

                return targetType;
            });

        // Attach security result to the compilation result
        if (result.Success)
        {
            _logger.Info(null, "Compilation successful: type={0}", result.CompiledType?.Name);
        }
        else
        {
            _logger.Error(null, "Compilation failed with {0} errors", result.Errors.Count);
        }

        return new CompilationResult(
            result.Success,
            result.CompiledType,
            result.Errors,
            scanResult,
            result.GeneratedAssembly);
    }

    /// <summary>
    /// Compiles source code for an IPermissionCallback implementation.
    /// This is the stealth channel â€?uses the same mechanism but targets a different interface.
    /// Performs security scan before compilation, then delegates to CompilationCore.
    /// </summary>
    /// <param name="sourceCode">The C# source code implementing IPermissionCallback</param>
    /// <returns>Compilation result with the callback type</returns>
    public CompilationResult CompilePermissionCallback(string sourceCode)
    {
        ArgumentNullException.ThrowIfNull(sourceCode);

        _logger.Info(null, "Starting permission callback compilation");

        // Security scan before compilation
        SecurityScanResult scanResult = SecurityScanner.Scan(sourceCode);
        if (!scanResult.Passed)
        {
            _logger.Error(null, "Security scan failed for permission callback: {0}", string.Join("; ", scanResult.Violations));
            return new CompilationResult(
                false, null,
                ["Security scan failed: " + string.Join("; ", scanResult.Violations)],
                scanResult);
        }

        // Delegate to compilation core
        CompilationResult result = _compilationCore.Compile(
            sourceCode,
            assembly =>
            {
                // Find IPermissionCallback implementation
                foreach (Type type in assembly.GetTypes())
                {
                    if (!type.IsAbstract && typeof(IPermissionCallback).IsAssignableFrom(type))
                    {
                        return type;
                    }
                }
                return null;
            },
            additionalReferences: [MetadataReference.CreateFromFile(typeof(IPermissionCallback).Assembly.Location)]);

        // Attach security result to the compilation result
        if (result.Success)
        {
            _logger.Info(null, "Permission callback compilation successful: type={0}", result.CompiledType?.Name);
        }
        else
        {
            _logger.Error(null, "Permission callback compilation failed with {0} errors", result.Errors.Count);
        }

        return new CompilationResult(
            result.Success,
            result.CompiledType,
            result.Errors,
            scanResult,
            result.GeneratedAssembly);
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
