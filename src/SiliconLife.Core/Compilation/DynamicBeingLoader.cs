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
/// Handles the full lifecycle: read code.enc → decrypt → security scan → compile → instantiate.
/// Falls back to a default type if anything fails.
/// </summary>
public class DynamicBeingLoader
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger<DynamicBeingLoader>();
    private const string CodeFileName = "code.enc";
    private const string PermissionCodeFileName = "permission.enc";

    private readonly DynamicCompilationExecutor _executor;

    /// <summary>
    /// Creates a new DynamicBeingLoader.
    /// </summary>
    /// <param name="hostAssemblyPath">Optional path to the host assembly for compilation references</param>
    public DynamicBeingLoader(string? hostAssemblyPath = null)
    {
        _executor = new DynamicCompilationExecutor(hostAssemblyPath);
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
            _logger.Debug("No custom code found for being {BeingId}", beingId);
            return null;
        }

        byte[] encryptedCode = File.ReadAllBytes(codePath);

        if (!CodeEncryption.TryDecryptToString(encryptedCode, beingId, out string? sourceCode) || sourceCode == null)
        {
            _logger.Error("Failed to decrypt custom code for being {BeingId}", beingId);
            throw new InvalidOperationException(
                $"Failed to decrypt custom code for being {beingId}. " +
                "Code may be corrupted or the GUID may have changed.");
        }

        CompilationResult result = _executor.Compile<SiliconBeingBase>(sourceCode);
        if (!result.Success)
        {
            string errorList = result.Errors.Count > 0
                ? string.Join("\n  ", result.Errors)
                : "Unknown error";
            _logger.Error("Compilation failed for being {BeingId}: {Errors}", beingId, errorList);
            throw new InvalidOperationException(
                $"Compilation failed for being {beingId}:\n  {errorList}");
        }

        _logger.Info("Custom code loaded for being {BeingId}, type={TypeName}", beingId, result.CompiledType?.Name);
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
            _logger.Warn("Security scan failed, not saving code for being {BeingId}", beingId);
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
            _logger.Info("Custom code saved for being {BeingId}", beingId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to save code for being {BeingId}: {Exception}", beingId, ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Compiles source code for a being and creates an instance.
    /// Does NOT persist — used for preview/verification.
    /// </summary>
    /// <param name="sourceCode">The C# source code</param>
    /// <param name="beingId">The being's GUID</param>
    /// <returns>CompilationResult with CompiledType and GeneratedAssembly</returns>
    public CompilationResult CompileBeing(string sourceCode, Guid beingId)
    {
        _logger.Info("Preview compilation for being {BeingId}", beingId);
        return _executor.Compile<SiliconBeingBase>(sourceCode);
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
            _logger.Debug("No permission callback file found for being {BeingId}", beingId);
            return new CompilationResult(false, null, ["No permission callback file found."]);
        }

        byte[] encryptedCode = File.ReadAllBytes(permPath);

        if (!CodeEncryption.TryDecryptToString(encryptedCode, beingId, out string? sourceCode) || sourceCode == null)
        {
            _logger.Error("Failed to decrypt permission callback for being {BeingId}", beingId);
            return new CompilationResult(false, null, ["Failed to decrypt permission callback code."]);
        }

        CompilationResult result = _executor.CompilePermissionCallback(sourceCode);
        if (result.Success)
        {
            _logger.Info("Permission callback loaded for being {BeingId}, type={TypeName}", beingId, result.CompiledType?.Name);
        }
        else
        {
            _logger.Error("Failed to load permission callback for being {BeingId}", beingId);
        }
        return result;
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
            _logger.Warn("Security scan failed for permission callback, not saving for being {BeingId}", beingId);
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
            _logger.Info("Permission callback saved for being {BeingId}", beingId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to save permission callback for being {BeingId}: {Exception}", beingId, ex.Message);
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
