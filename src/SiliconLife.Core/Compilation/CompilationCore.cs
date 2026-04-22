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
/// Core compilation engine based on Roslyn.
/// Provides low-level C# compilation without any security scanning or permission checks.
/// This is the shared compilation core used by both DynamicCompilationExecutor and DynamicBeingLoader.
/// </summary>
public class CompilationCore
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<CompilationCore>();
    private readonly List<MetadataReference> _baseReferences;

    /// <summary>
    /// Creates a new CompilationCore with standard assembly references.
    /// </summary>
    /// <param name="additionalAssemblies">Optional additional assemblies to reference</param>
    public CompilationCore(params Assembly[] additionalAssemblies)
    {
        _baseReferences =
        [
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location),
        ];

        // Add System.Runtime reference assembly (contains type forwards for Enum, Guid, Uri, IEnumerable<>, etc.)
        // Note: typeof(Enum).Assembly returns System.Private.CoreLib (implementation), 
        // but Roslyn needs the reference assembly System.Runtime for compilation
        try
        {
            Assembly runtimeRefAssembly = Assembly.Load("System.Runtime");
            _baseReferences.Add(MetadataReference.CreateFromFile(runtimeRefAssembly.Location));
            _logger.Debug(null, "Added System.Runtime reference assembly from: {0}", runtimeRefAssembly.Location);
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to add System.Runtime reference: {0}", ex.Message);
        }

        // Add System.Private.Uri (contains Uri, UriKind implementation)
        try
        {
            Assembly uriAssembly = typeof(Uri).Assembly;
            if (uriAssembly.GetName().Name != "System.Private.CoreLib")
            {
                _baseReferences.Add(MetadataReference.CreateFromFile(uriAssembly.Location));
                _logger.Debug(null, "Added {0} reference from: {1}", uriAssembly.GetName().Name, uriAssembly.Location);
            }
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to add Uri assembly reference: {0}", ex.Message);
        }

        // Add System.Linq (contains LINQ extension methods like .Any())
        try
        {
            Assembly linqAssembly = typeof(System.Linq.Enumerable).Assembly;
            _baseReferences.Add(MetadataReference.CreateFromFile(linqAssembly.Location));
            _logger.Debug(null, "Added System.Linq reference from: {0}", linqAssembly.Location);
        }
        catch (Exception ex)
        {
            _logger.Warn(null, "Failed to add System.Linq reference: {0}", ex.Message);
        }

        // Add System.Text.Json
        Type? jsonType = Type.GetType("System.Text.Json.JsonSerializer, System.Text.Json");
        if (jsonType != null)
        {
            _baseReferences.Add(MetadataReference.CreateFromFile(jsonType.Assembly.Location));
        }

        // Add System.Text.RegularExpressions
        Type? regexType = Type.GetType("System.Text.RegularExpressions.Regex, System.Text.RegularExpressions");
        if (regexType != null)
        {
            _baseReferences.Add(MetadataReference.CreateFromFile(regexType.Assembly.Location));
        }

        // Add additional assemblies
        foreach (Assembly asm in additionalAssemblies)
        {
            _baseReferences.Add(MetadataReference.CreateFromFile(asm.Location));
        }
    }

    /// <summary>
    /// Compiles C# source code in-memory without any security scanning.
    /// Returns the first type that matches the specified filter.
    /// </summary>
    /// <param name="sourceCode">The C# source code to compile</param>
    /// <param name="typeFilter">Function to filter and select the target type from compiled assembly</param>
    /// <param name="additionalReferences">Optional additional metadata references</param>
    /// <returns>Compilation result with the matching type</returns>
    public CompilationResult Compile(
        string sourceCode,
        Func<Assembly, Type?> typeFilter,
        IEnumerable<MetadataReference>? additionalReferences = null)
    {
        ArgumentNullException.ThrowIfNull(sourceCode);
        ArgumentNullException.ThrowIfNull(typeFilter);

        // Parse source code
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(
            sourceCode,
            CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp13));

        // Combine references
        var allReferences = new List<MetadataReference>(_baseReferences);
        if (additionalReferences != null)
        {
            allReferences.AddRange(additionalReferences);
        }

        // Compile
        string assemblyName = $"DynCore_{Guid.NewGuid():N}";
        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            [syntaxTree],
            allReferences,
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

            _logger.Debug(null, "Compilation failed with {0} errors", errors.Count);
            return new CompilationResult(false, null, errors);
        }

        ms.Seek(0, System.IO.SeekOrigin.Begin);

        // Load the compiled assembly
        Assembly assembly = Assembly.Load(ms.ToArray());

        // Find target type using filter
        Type? targetType = typeFilter(assembly);

        if (targetType == null)
        {
            _logger.Warn(null, "No matching type found in compiled code");
            return new CompilationResult(false, null, ["No matching type found in compiled code."], null, assembly);
        }

        _logger.Debug(null, "Compilation successful: type={0}, assembly={1}", targetType.Name, assemblyName);
        return new CompilationResult(true, targetType, [], null, assembly);
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
