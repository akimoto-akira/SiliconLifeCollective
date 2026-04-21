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

namespace SiliconLife.Collective;

/// <summary>
/// Manages loading, compiling, and persisting dynamically compiled silicon beings.
/// Handles the full lifecycle: read code.enc → decrypt → compile → instantiate.
/// Falls back to a default type if anything fails.
/// Uses CompilationCore directly for compilation without security scanning.
/// </summary>
public class DynamicBeingLoader
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<DynamicBeingLoader>();
    private const string CodeFileName = "code.enc";
    private const string PermissionCodeFileName = "permission.enc";

    private readonly CompilationCore _compilationCore;

    /// <summary>
    /// Creates a new DynamicBeingLoader.
    /// </summary>
    /// <param name="hostAssemblyPath">Optional path to the host assembly for compilation references</param>
    public DynamicBeingLoader(string? hostAssemblyPath = null)
    {
        // Initialize compilation core with SiliconBeingBase and IPermissionCallback assemblies
        _compilationCore = new CompilationCore(
            typeof(SiliconBeingBase).Assembly,
            typeof(IPermissionCallback).Assembly);
    }

    /// <summary>
    /// Validates that the default assembly references are in the allowed list.
    /// </summary>
    /// <param name="beingId">Optional being ID for logging context</param>
    /// <param name="baseAssembly">The base assembly to validate references for</param>
    /// <returns>Tuple of (isValid, unauthorizedList)</returns>
    private (bool IsValid, string UnauthorizedList) ValidateDefaultReferences(Guid? beingId, Assembly baseAssembly)
    {
        Type? runtimeType = Type.GetType("System.Runtime.CompilerServices.RuntimeHelpers, System.Runtime");
        Type? linqType = Type.GetType("System.Linq.Enumerable, System.Linq");
        Type? jsonType = Type.GetType("System.Text.Json.JsonSerializer, System.Text.Json");
        Type? regexType = Type.GetType("System.Text.RegularExpressions.Regex, System.Text.RegularExpressions");
        
        var referencedNames = new List<string> 
        { 
            "mscorlib.dll", 
            "System.Private.CoreLib.dll",
            "System.Console.dll", 
            "System.Collections.dll"
        };
        if (runtimeType != null) referencedNames.Add("System.Runtime.dll");
        if (linqType != null) referencedNames.Add("System.Linq.dll");
        if (jsonType != null) referencedNames.Add("System.Text.Json.dll");
        if (regexType != null) referencedNames.Add("System.Text.RegularExpressions.dll");
        referencedNames.Add(baseAssembly.GetName().Name + ".dll");

        (bool refsValid, List<string> unauthorized) = SecurityScanner.ValidateReferences(referencedNames);
        
        string unauthorizedList = string.Join(", ", unauthorized);
        if (!refsValid && beingId.HasValue)
        {
            _logger.Error("Unauthorized assembly references for being {0}: {1}", beingId.Value, unauthorizedList);
        }
        else if (!refsValid)
        {
            _logger.Error("Unauthorized assembly references: {0}", unauthorizedList);
        }

        return (refsValid, unauthorizedList);
    }

    /// <summary>
    /// Checks if a silicon being has custom compiled code in its directory.
    /// </summary>
    /// <param name="beingDirectory">The silicon being's data directory</param>
    /// <returns>True if code.enc exists</returns>
    public static bool HasCustomCode(string beingDirectory)
    {
        string codePath = Path.Combine(beingDirectory, CodeFileName);
        return File.Exists(codePath);
    }

    /// <summary>
    /// Loads and compiles a silicon being's custom code from its directory.
    /// Returns null if no custom code exists, the default type should be used.
    /// Throws if compilation fails (caller should catch and fall back).
    /// </summary>
    /// <param name="beingId">The silicon being's GUID (used for decryption key)</param>
    /// <param name="beingDirectory">The silicon being's data directory</param>
    /// <returns>The compiled Type if successful, null if no custom code</returns>
    public Type? LoadBeingType(Guid beingId, string beingDirectory)
    {
        string codePath = Path.Combine(beingDirectory, CodeFileName);
        if (!File.Exists(codePath))
        {
            _logger.Debug("No custom code found for being {0}", beingId);
            return null;
        }

        byte[] encryptedCode = File.ReadAllBytes(codePath);

        if (!CodeEncryption.TryDecryptToString(encryptedCode, beingId, out string? sourceCode) || sourceCode == null)
        {
            _logger.Error("Failed to decrypt custom code for being {0}", beingId);
            throw new InvalidOperationException(
                $"Failed to decrypt custom code for being {beingId}. " +
                "Code may be corrupted or the GUID may have changed.");
        }

        // Validate assembly references against allowed list
        (bool refsValid, string unauthorizedList) = ValidateDefaultReferences(beingId, typeof(SiliconBeingBase).Assembly);
        if (!refsValid)
        {
            throw new InvalidOperationException(
                $"Unauthorized assembly references for being {beingId}: {unauthorizedList}");
        }

        // Compile using CompilationCore directly (no security scan)
        CompilationResult result = _compilationCore.Compile(
            sourceCode,
            assembly =>
            {
                // Find any type inheriting SiliconBeingBase
                foreach (Type type in assembly.GetTypes())
                {
                    if (!type.IsAbstract && typeof(SiliconBeingBase).IsAssignableFrom(type))
                    {
                        return type;
                    }
                }
                return null;
            });

        if (!result.Success)
        {
            string errorList = result.Errors.Count > 0
                ? string.Join("\n  ", result.Errors)
                : "Unknown error";
            _logger.Error("Compilation failed for being {0}: {1}", beingId, errorList);
            throw new InvalidOperationException(
                $"Compilation failed for being {beingId}:\n  {errorList}");
        }

        _logger.Info("Custom code loaded for being {0}, type={1}", beingId, result.CompiledType?.Name);
        return result.CompiledType;
    }

    /// <summary>
    /// Persists a silicon being's custom code to disk (encrypted).
    /// </summary>
    /// <param name="beingId">The silicon being's GUID</param>
    /// <param name="beingDirectory">The silicon being's data directory</param>
    /// <param name="sourceCode">The C# source code</param>
    /// <returns>True if persistence succeeded</returns>
    public bool SaveBeingCode(Guid beingId, string beingDirectory, string sourceCode)
    {
        ArgumentNullException.ThrowIfNull(sourceCode);

        SecurityScanResult scanResult = SecurityScanner.Scan(sourceCode);
        if (!scanResult.Passed)
        {
            _logger.Warn("Security scan failed, not saving code for being {0}", beingId);
            return false;
        }

        try
        {
            if (!Directory.Exists(beingDirectory))
            {
                Directory.CreateDirectory(beingDirectory);
            }

            byte[] encrypted = CodeEncryption.Encrypt(sourceCode, beingId);
            string codePath = Path.Combine(beingDirectory, CodeFileName);
            File.WriteAllBytes(codePath, encrypted);
            _logger.Info("Custom code saved for being {0}", beingId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to save code for being {0}: {1}", beingId, ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Compiles source code for a being and creates an instance.
    /// Does NOT persist — used for preview/verification.
    /// Uses CompilationCore directly (no security scan).
    /// </summary>
    /// <param name="sourceCode">The C# source code</param>
    /// <param name="beingId">The being's GUID</param>
    /// <returns>CompilationResult with CompiledType and GeneratedAssembly</returns>
    public CompilationResult CompileBeing(string sourceCode, Guid beingId)
    {
        _logger.Info("Preview compilation for being {0}", beingId);
        
        // Validate assembly references against allowed list
        (bool refsValid2, string unauthorizedList2) = ValidateDefaultReferences(beingId, typeof(SiliconBeingBase).Assembly);
        if (!refsValid2)
        {
            return new CompilationResult(
                false, 
                null, 
                [$"Unauthorized assembly references: {unauthorizedList2}"]);
        }

        // Compile using CompilationCore directly (no security scan)
        return _compilationCore.Compile(
            sourceCode,
            assembly =>
            {
                // Find any type inheriting SiliconBeingBase
                foreach (Type type in assembly.GetTypes())
                {
                    if (!type.IsAbstract && typeof(SiliconBeingBase).IsAssignableFrom(type))
                    {
                        return type;
                    }
                }
                return null;
            });
    }

    /// <summary>
    /// Checks if a being has a custom permission callback stored.
    /// </summary>
    /// <param name="beingDirectory">The silicon being's data directory</param>
    /// <returns>True if permission.enc exists</returns>
    public static bool HasCustomPermissionCallback(string beingDirectory)
    {
        string permPath = Path.Combine(beingDirectory, PermissionCodeFileName);
        return File.Exists(permPath);
    }

    /// <summary>
    /// Loads and compiles a custom IPermissionCallback from the being's directory.
    /// This is the independent permission callback channel.
    /// </summary>
    /// <param name="beingId">The silicon being's GUID</param>
    /// <param name="beingDirectory">The silicon being's data directory</param>
    /// <returns>CompilationResult — check Success and use CompiledType</returns>
    public CompilationResult LoadPermissionCallback(Guid beingId, string beingDirectory)
    {
        string permPath = Path.Combine(beingDirectory, PermissionCodeFileName);
        if (!File.Exists(permPath))
        {
            _logger.Debug("No permission callback file found for being {0}", beingId);
            return new CompilationResult(false, null, ["No permission callback file found."]);
        }

        byte[] encryptedCode = File.ReadAllBytes(permPath);

        if (!CodeEncryption.TryDecryptToString(encryptedCode, beingId, out string? sourceCode) || sourceCode == null)
        {
            _logger.Error("Failed to decrypt permission callback for being {0}", beingId);
            return new CompilationResult(false, null, ["Failed to decrypt permission callback code."]);
        }

        // Validate assembly references against allowed list
        (bool refsValid3, string unauthorizedList3) = ValidateDefaultReferences(beingId, typeof(IPermissionCallback).Assembly);
        if (!refsValid3)
        {
            return new CompilationResult(
                false, 
                null, 
                [$"Unauthorized assembly references: {unauthorizedList3}"]);
        }

        // Compile using CompilationCore directly (no security scan)
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
            });

        if (result.Success)
        {
            _logger.Info("Permission callback loaded for being {0}, type={1}", beingId, result.CompiledType?.Name);
        }
        else
        {
            _logger.Error("Failed to load permission callback for being {0}", beingId);
        }
        return result;
    }

    /// <summary>
    /// Compiles permission callback source code without saving to disk.
    /// Used for validation before saving.
    /// </summary>
    /// <param name="sourceCode">The C# source code implementing IPermissionCallback</param>
    /// <returns>CompilationResult with CompiledType if successful</returns>
    public CompilationResult CompilePermissionCallback(string sourceCode)
    {
        _logger.Info("Permission callback compilation (preview mode)");
        
        // Validate assembly references against allowed list
        (bool refsValid4, string unauthorizedList4) = ValidateDefaultReferences(null, typeof(IPermissionCallback).Assembly);
        if (!refsValid4)
        {
            return new CompilationResult(
                false, 
                null, 
                [$"Unauthorized assembly references: {unauthorizedList4}"]);
        }

        // Compile using CompilationCore directly (no security scan)
        return _compilationCore.Compile(
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
            });
    }

    /// <summary>
    /// Reads and decrypts the permission callback source code without compiling.
    /// Used by Web UI to display the current code for editing.
    /// </summary>
    /// <param name="beingId">The silicon being's GUID</param>
    /// <param name="beingDirectory">The silicon being's data directory</param>
    /// <returns>The decrypted source code, or empty string if not found or failed</returns>
    public string GetPermissionCallbackSourceCode(Guid beingId, string beingDirectory)
    {
        try
        {
            string permPath = Path.Combine(beingDirectory, PermissionCodeFileName);
            if (!File.Exists(permPath))
            {
                _logger.Debug("No permission callback file found for being {0}", beingId);
                return string.Empty;
            }

            byte[] encryptedCode = File.ReadAllBytes(permPath);

            if (!CodeEncryption.TryDecryptToString(encryptedCode, beingId, out string? sourceCode) || sourceCode == null)
            {
                _logger.Warn("Failed to decrypt permission callback for being {0}", beingId);
                return string.Empty;
            }

            return sourceCode;
        }
        catch (Exception ex)
        {
            _logger.Error("Error reading permission callback source for being {0}: {1}", beingId, ex.Message);
            return string.Empty;
        }
    }

    /// <summary>
    /// Persists a custom permission callback to disk (encrypted).
    /// </summary>
    /// <param name="beingId">The silicon being's GUID</param>
    /// <param name="beingDirectory">The silicon being's data directory</param>
    /// <param name="sourceCode">The C# source code implementing IPermissionCallback</param>
    /// <returns>True if persistence succeeded</returns>
    public bool SavePermissionCallback(Guid beingId, string beingDirectory, string sourceCode)
    {
        ArgumentNullException.ThrowIfNull(sourceCode);

        SecurityScanResult scanResult = SecurityScanner.Scan(sourceCode);
        if (!scanResult.Passed)
        {
            _logger.Warn("Security scan failed for permission callback, not saving for being {0}", beingId);
            return false;
        }

        try
        {
            if (!Directory.Exists(beingDirectory))
            {
                Directory.CreateDirectory(beingDirectory);
            }

            byte[] encrypted = CodeEncryption.Encrypt(sourceCode, beingId);
            string permPath = Path.Combine(beingDirectory, PermissionCodeFileName);
            File.WriteAllBytes(permPath, encrypted);
            _logger.Info("Permission callback saved for being {0}", beingId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to save permission callback for being {0}: {1}", beingId, ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Deletes the custom code file for a silicon being.
    /// After deletion, the being will use its default implementation.
    /// </summary>
    /// <param name="beingDirectory">The silicon being's data directory</param>
    /// <returns>True if the file was deleted or didn't exist</returns>
    public static bool DeleteCustomCode(string beingDirectory)
    {
        string codePath = Path.Combine(beingDirectory, CodeFileName);
        if (File.Exists(codePath))
        {
            File.Delete(codePath);
            return true;
        }
        return true;
    }

    /// <summary>
    /// Deletes the custom permission callback file for a silicon being.
    /// </summary>
    /// <param name="beingDirectory">The silicon being's data directory</param>
    /// <returns>True if the file was deleted or didn't exist</returns>
    public static bool DeleteCustomPermissionCallback(string beingDirectory)
    {
        string permPath = Path.Combine(beingDirectory, PermissionCodeFileName);
        if (File.Exists(permPath))
        {
            File.Delete(permPath);
            return true;
        }
        return true;
    }
}
