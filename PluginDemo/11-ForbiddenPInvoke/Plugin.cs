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

using System;
using System.Runtime.InteropServices;
using SiliconLife.Collective;

namespace SiliconLife.Demo.ForbiddenPInvoke;

/// <summary>
/// ⚠️ ANTI-PATTERN: Demonstrates P/Invoke and unsafe code operations that are FORBIDDEN in plugins.
/// 
/// The following categories are blocked (triple insurance mechanism):
/// 
/// 【Layer 1: TypeRef Scanning】
/// Direct references to these types are caught in the PE TypeRef table:
/// - System.Runtime.InteropServices.DllImportAttribute
/// - System.Runtime.InteropServices.Marshal
/// - System.Runtime.InteropServices.MemoryMarshal
/// - System.Runtime.InteropServices.NativeMemory
/// - System.Runtime.InteropServices.NativeLibrary
/// - System.Runtime.InteropServices.GCHandle
/// - System.Runtime.InteropServices.SafeHandle
/// - System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute
/// - System.Runtime.InteropServices.SuppressGCTransitionAttribute
/// - System.Runtime.CompilerServices.Unsafe
/// - System.Security.UnverifiableCodeAttribute
/// - System.Security.SuppressUnmanagedCodeSecurityAttribute
/// 
/// 【Layer 2: Unsafe Marker Scanning (ScanUnsafeMarkers)】
/// - [assembly: System.Security.UnverifiableCode] — compiler emits this when unsafe blocks exist
/// - [module: System.Security.UnverifiableCode] — module-level unsafe marker
/// - MethodAttributes.PinvokeImpl flag — set on methods with [DllImport]
/// 
/// 【Layer 3: IL String Scanning (#US Heap)】
/// - String constants matching "System.Runtime.InteropServices.*" are flagged
/// 
/// These operations are dangerous because they can:
/// 1. Execute arbitrary native code outside managed runtime control
/// 2. Corrupt managed memory by writing to arbitrary addresses
/// 3. Bypass all .NET type safety and garbage collector invariants
/// 4. Load and execute arbitrary native shared libraries (DLLs/SOs)
/// 5. Pin managed objects, preventing GC from relocating them (denial of service)
/// 
/// ❌ NO SAFE ALTERNATIVE EXISTS: P/Invoke and unsafe code are HARD PROHIBITIONS.
/// Unlike file I/O (→PermissionedStreamFactory), network (→NetworkExecutor), or
/// processes (→CommandLineExecutor), there is no "safe wrapper" for native interop.
/// Plugins that genuinely need native code must be included as TrustedAssemblies.
/// 
/// This plugin demonstrates what NOT to do. Each violation is marked with
/// ⚠️ VIOLATION comment indicating which scanning layer catches it.
/// </summary>
public class ForbiddenPInvokePlugin : IPlugin
{
    public string Id => "com.siliconlife.demo.forbiddenpinvoke";
    public string Version => "1.0.0";
    public string GetName(Language language) => "Forbidden P/Invoke Anti-Pattern";
    public string GetDescription(Language language) =>
        "Demonstrates FORBIDDEN P/Invoke and unsafe code operations. " +
        "Shows why DllImport, Marshal, NativeMemory, and GCHandle are banned " +
        "and explains the triple-layer detection mechanism.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    public void OnLoad()
    {
    }

    public void OnStart()
    {
        Console.WriteLine("\n========== FORBIDDEN P/INVOKE & UNSAFE ANTI-PATTERNS ==========");
        Console.WriteLine("⚠️  This plugin demonstrates operations that will be BLOCKED by PluginLoader.\n");

        // NOTE: These methods demonstrate code patterns that would cause the plugin
        // to be REJECTED by PluginLoader during security scanning.
        // In a real scenario, PluginLoader would reject this plugin at load time.

        DemonstrateDllImport();
        DemonstrateMarshal();
        DemonstrateNativeMemory();
        DemonstrateGCHandle();
        DemonstrateUnsafeBlock();
        DemonstrateNativeLibrary();

        Console.WriteLine("\n========== WHY NO SAFE ALTERNATIVE EXISTS ==========");
        DemonstrateWhyNoAlternative();

        Console.WriteLine("\n========== TRIPLE INSURANCE MECHANISM ==========");
        DemonstrateTripleInsurance();
    }

    /// <summary>
    /// ⚠️ VIOLATION: [DllImport] declares a P/Invoke native method.
    /// Caught by: Layer 1 (TypeRef: DllImportAttribute) + Layer 2 (PinvokeImpl flag)
    /// </summary>
    private void DemonstrateDllImport()
    {
        Console.WriteLine("[Violation 1] [DllImport] P/Invoke Declaration");
        Console.WriteLine("  ⚠️ VIOLATION: [PInvoke] GetTickCount64 (native interop)");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Runtime.InteropServices.DllImportAttribute");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     [DllImport(\"kernel32.dll\")]");
        Console.WriteLine("     private static extern ulong GetTickCount64();");
        Console.WriteLine();
        Console.WriteLine("     [DllImport(\"user32.dll\", CharSet = CharSet.Unicode)]");
        Console.WriteLine("     private static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);");
        Console.WriteLine();
        Console.WriteLine("  🔍 DETECTION:");
        Console.WriteLine("     1. TypeRef scan catches reference to DllImportAttribute");
        Console.WriteLine("     2. ScanUnsafeMarkers detects MethodAttributes.PinvokeImpl flag on the method");
        Console.WriteLine("     → Double detection ensures no false negatives");
        Console.WriteLine();
        Console.WriteLine("  ❌ NO ALTERNATIVE: Plugin cannot call native functions.");
        Console.WriteLine("     If native code is needed, the library must be audited and");
        Console.WriteLine("     placed in TrustedAssemblies whitelist.");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: Marshal class for managed/unmanaged memory operations.
    /// Caught by: Layer 1 (TypeRef: Marshal)
    /// </summary>
    private void DemonstrateMarshal()
    {
        Console.WriteLine("[Violation 2] Marshal — Managed/Unmanaged Memory Bridge");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Runtime.InteropServices.Marshal");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     // Allocate unmanaged memory");
        Console.WriteLine("     IntPtr ptr = Marshal.AllocHGlobal(1024);");
        Console.WriteLine("     try");
        Console.WriteLine("     {");
        Console.WriteLine("         // Write bytes to unmanaged memory");
        Console.WriteLine("         Marshal.WriteByte(ptr, 0xFF);");
        Console.WriteLine("         Marshal.WriteInt32(ptr + 4, 42);");
        Console.WriteLine();
        Console.WriteLine("         // Read string from native pointer");
        Console.WriteLine("         string? str = Marshal.PtrToStringAnsi(ptr);");
        Console.WriteLine();
        Console.WriteLine("         // Copy managed struct to unmanaged memory");
        Console.WriteLine("         var data = new SomeStruct { X = 1, Y = 2 };");
        Console.WriteLine("         Marshal.StructureToPtr(data, ptr, false);");
        Console.WriteLine("     }");
        Console.WriteLine("     finally");
        Console.WriteLine("     {");
        Console.WriteLine("         Marshal.FreeHGlobal(ptr);  // Must free manually!");
        Console.WriteLine("     }");
        Console.WriteLine();
        Console.WriteLine("  🔍 DETECTION:");
        Console.WriteLine("     TypeRef table contains 'System.Runtime.InteropServices.Marshal'");
        Console.WriteLine("     → Caught during Step 1 of ScanForbiddenReferences");
        Console.WriteLine();
        Console.WriteLine("  ⚠️ WHY DANGEROUS:");
        Console.WriteLine("     - Can read/write arbitrary memory addresses");
        Console.WriteLine("     - Can corrupt GC heap by writing invalid references");
        Console.WriteLine("     - Manual memory management = memory leaks and use-after-free");
        Console.WriteLine("     - Can invoke native function pointers (GetDelegateForFunctionPointer)");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: NativeMemory for direct heap allocation.
    /// Caught by: Layer 1 (TypeRef: NativeMemory)
    /// </summary>
    private void DemonstrateNativeMemory()
    {
        Console.WriteLine("[Violation 3] NativeMemory — Native Heap Allocation");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Runtime.InteropServices.NativeMemory");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     unsafe");
        Console.WriteLine("     {");
        Console.WriteLine("         // Allocate 4KB from native heap");
        Console.WriteLine("         void* buffer = NativeMemory.Alloc(4096);");
        Console.WriteLine("         try");
        Console.WriteLine("         {");
        Console.WriteLine("             // Zero-initialize");
        Console.WriteLine("             NativeMemory.Clear(buffer, 4096);");
        Console.WriteLine();
        Console.WriteLine("             // Reallocate to 8KB");
        Console.WriteLine("             buffer = NativeMemory.Realloc(buffer, 8192);");
        Console.WriteLine();
        Console.WriteLine("             // Aligned allocation for SIMD");
        Console.WriteLine("             void* aligned = NativeMemory.AlignedAlloc(256, 32);");
        Console.WriteLine("             NativeMemory.AlignedFree(aligned);");
        Console.WriteLine("         }");
        Console.WriteLine("         finally");
        Console.WriteLine("         {");
        Console.WriteLine("             NativeMemory.Free(buffer);");
        Console.WriteLine("         }");
        Console.WriteLine("     }");
        Console.WriteLine();
        Console.WriteLine("  🔍 DETECTION:");
        Console.WriteLine("     TypeRef table contains 'System.Runtime.InteropServices.NativeMemory'");
        Console.WriteLine("     + unsafe block triggers [UnsafeMarker] from UnverifiableCode attribute");
        Console.WriteLine("     → Double detection: TypeRef + UnsafeMarker");
        Console.WriteLine();
        Console.WriteLine("  ⚠️ WHY DANGEROUS:");
        Console.WriteLine("     - Bypasses GC completely (no managed tracking)");
        Console.WriteLine("     - Memory leaks if Free is not called");
        Console.WriteLine("     - Buffer overflows corrupt process memory");
        Console.WriteLine("     - Combined with unsafe pointers, can access any memory");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: GCHandle to pin managed objects and expose raw pointers.
    /// Caught by: Layer 1 (TypeRef: GCHandle)
    /// </summary>
    private void DemonstrateGCHandle()
    {
        Console.WriteLine("[Violation 4] GCHandle — Pin Objects and Expose Pointers");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Runtime.InteropServices.GCHandle");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     byte[] managedArray = new byte[1024];");
        Console.WriteLine();
        Console.WriteLine("     // Pin the array so GC cannot move it");
        Console.WriteLine("     GCHandle handle = GCHandle.Alloc(managedArray, GCHandleType.Pinned);");
        Console.WriteLine("     try");
        Console.WriteLine("     {");
        Console.WriteLine("         // Get raw pointer to managed array");
        Console.WriteLine("         IntPtr ptr = handle.AddrOfPinnedObject();");
        Console.WriteLine();
        Console.WriteLine("         // Pass to native code (P/Invoke)");
        Console.WriteLine("         // NativeMethod(ptr, managedArray.Length);");
        Console.WriteLine();
        Console.WriteLine("         // Or use Marshal to read/write through the pointer");
        Console.WriteLine("         // Marshal.WriteInt32(ptr, 0xDEADBEEF);");
        Console.WriteLine("     }");
        Console.WriteLine("     finally");
        Console.WriteLine("     {");
        Console.WriteLine("         handle.Free();  // Must free to unpin!");
        Console.WriteLine("     }");
        Console.WriteLine();
        Console.WriteLine("  🔍 DETECTION:");
        Console.WriteLine("     TypeRef table contains 'System.Runtime.InteropServices.GCHandle'");
        Console.WriteLine("     → Caught during Step 1 of ScanForbiddenReferences");
        Console.WriteLine();
        Console.WriteLine("  ⚠️ WHY DANGEROUS:");
        Console.WriteLine("     - Pinned objects fragment the managed heap");
        Console.WriteLine("     - Exposed pointers can be passed to native code");
        Console.WriteLine("     - Forgetting to Free causes permanent memory pinning (DoS)");
        Console.WriteLine("     - Combined with unsafe, allows arbitrary memory corruption");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: unsafe keyword produces [module: UnverifiableCode] attribute.
    /// Caught by: Layer 2 (ScanUnsafeMarkers → UnverifiableCode)
    /// </summary>
    private void DemonstrateUnsafeBlock()
    {
        Console.WriteLine("[Violation 5] unsafe Block — Unverifiable Code");
        Console.WriteLine("  ⚠️ VIOLATION: [UnsafeMarker] [module: System.Security.UnverifiableCode]");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     unsafe");
        Console.WriteLine("     {");
        Console.WriteLine("         int value = 42;");
        Console.WriteLine("         int* ptr = &value;    // Take address of local");
        Console.WriteLine("         *ptr = 100;           // Write through pointer");
        Console.WriteLine();
        Console.WriteLine("         // Stack allocation (no GC tracking)");
        Console.WriteLine("         byte* stack = stackalloc byte[256];");
        Console.WriteLine("         stack[0] = 0xFF;");
        Console.WriteLine();
        Console.WriteLine("         // Pointer arithmetic");
        Console.WriteLine("         int* next = ptr + 1;  // Adjacent memory!");
        Console.WriteLine("         Console.WriteLine(*next); // Read beyond allocation");
        Console.WriteLine();
        Console.WriteLine("         // Fixed statement to pin and access array");
        Console.WriteLine("         byte[] data = new byte[100];");
        Console.WriteLine("         fixed (byte* p = data)");
        Console.WriteLine("         {");
        Console.WriteLine("             // Direct memory access, no bounds checking");
        Console.WriteLine("             *(int*)p = 0xCAFEBABE;");
        Console.WriteLine("         }");
        Console.WriteLine("     }");
        Console.WriteLine();
        Console.WriteLine("  🔍 DETECTION:");
        Console.WriteLine("     The C# compiler adds [module: System.Security.UnverifiableCode]");
        Console.WriteLine("     attribute to any assembly containing unsafe blocks.");
        Console.WriteLine("     ScanUnsafeMarkers checks both assembly-level and module-level");
        Console.WriteLine("     CustomAttribute tables for this marker.");
        Console.WriteLine();
        Console.WriteLine("  ⚠️ WHY DANGEROUS:");
        Console.WriteLine("     - Disables all .NET type safety guarantees");
        Console.WriteLine("     - Pointer arithmetic can read/write any process memory");
        Console.WriteLine("     - No bounds checking on pointer dereferences");
        Console.WriteLine("     - stackalloc + overflow = stack corruption → code execution");
        Console.WriteLine();
    }

    /// <summary>
    /// ⚠️ VIOLATION: NativeLibrary to dynamically load shared libraries.
    /// Caught by: Layer 1 (TypeRef: NativeLibrary)
    /// </summary>
    private void DemonstrateNativeLibrary()
    {
        Console.WriteLine("[Violation 6] NativeLibrary — Dynamic Native Library Loading");
        Console.WriteLine("  ⚠️ VIOLATION: [TypeRef] System.Runtime.InteropServices.NativeLibrary");
        Console.WriteLine("  ❌ FORBIDDEN CODE:");
        Console.WriteLine("     // Load arbitrary native library at runtime");
        Console.WriteLine("     IntPtr lib = NativeLibrary.Load(\"evil.dll\");");
        Console.WriteLine("     try");
        Console.WriteLine("     {");
        Console.WriteLine("         // Get function pointer by name");
        Console.WriteLine("         IntPtr funcPtr = NativeLibrary.GetExport(lib, \"malicious_function\");");
        Console.WriteLine();
        Console.WriteLine("         // Cast to delegate and invoke");
        Console.WriteLine("         var func = Marshal.GetDelegateForFunctionPointer<Action>(funcPtr);");
        Console.WriteLine("         func();  // Execute arbitrary native code!");
        Console.WriteLine("     }");
        Console.WriteLine("     finally");
        Console.WriteLine("     {");
        Console.WriteLine("         NativeLibrary.Free(lib);");
        Console.WriteLine("     }");
        Console.WriteLine();
        Console.WriteLine("  🔍 DETECTION:");
        Console.WriteLine("     TypeRef table contains 'System.Runtime.InteropServices.NativeLibrary'");
        Console.WriteLine("     → Caught during Step 1 of ScanForbiddenReferences");
        Console.WriteLine();
        Console.WriteLine("  ⚠️ WHY DANGEROUS:");
        Console.WriteLine("     - Loads and executes arbitrary native code");
        Console.WriteLine("     - No security scanning applied to native libraries");
        Console.WriteLine("     - Combined with GetDelegateForFunctionPointer, creates callable delegates");
        Console.WriteLine("     - Equivalent to running arbitrary machine code");
        Console.WriteLine();
    }

    /// <summary>
    /// Explains why there is no safe alternative for P/Invoke and unsafe code.
    /// </summary>
    private void DemonstrateWhyNoAlternative()
    {
        Console.WriteLine("[Why No Alternative?]");
        Console.WriteLine();
        Console.WriteLine("  Unlike other forbidden categories that have safe wrappers:");
        Console.WriteLine();
        Console.WriteLine("  ┌─────────────────────────────────────────────────────────────────┐");
        Console.WriteLine("  │ Forbidden Category  │ Safe Wrapper              │ Auditable?    │");
        Console.WriteLine("  ├─────────────────────────────────────────────────────────────────┤");
        Console.WriteLine("  │ File I/O            │ PermissionedStreamFactory │ ✅ Yes         │");
        Console.WriteLine("  │ Network             │ NetworkExecutor           │ ✅ Yes         │");
        Console.WriteLine("  │ Process             │ CommandLineExecutor       │ ✅ Yes         │");
        Console.WriteLine("  │ Reflection          │ ITypeRegistry/Factory     │ ✅ Yes         │");
        Console.WriteLine("  │ P/Invoke & unsafe   │ ❌ NONE                   │ ❌ Impossible  │");
        Console.WriteLine("  └─────────────────────────────────────────────────────────────────┘");
        Console.WriteLine();
        Console.WriteLine("  Reason: Native code operates OUTSIDE the managed runtime.");
        Console.WriteLine("  Once you call into native code, there is no way to:");
        Console.WriteLine("  - Intercept what the native code does");
        Console.WriteLine("  - Enforce memory safety or type safety");
        Console.WriteLine("  - Audit or log the operations performed");
        Console.WriteLine("  - Sandbox the execution environment");
        Console.WriteLine("  - Recover from crashes (native crash = process crash)");
        Console.WriteLine();
        Console.WriteLine("  If a plugin genuinely needs native interop, it must be:");
        Console.WriteLine("  1. Manually audited by the project maintainer");
        Console.WriteLine("  2. Added to TrustedAssemblies whitelist in PluginLoader");
        Console.WriteLine("  3. Its PE metadata name must match exactly (no renaming tricks)");
        Console.WriteLine();
    }

    /// <summary>
    /// Explains the triple insurance mechanism for detecting P/Invoke and unsafe code.
    /// </summary>
    private void DemonstrateTripleInsurance()
    {
        Console.WriteLine("[Triple Insurance Mechanism]");
        Console.WriteLine();
        Console.WriteLine("  PluginLoader uses THREE independent detection layers:");
        Console.WriteLine();
        Console.WriteLine("  ┌─────────────────────────────────────────────────────────────────────┐");
        Console.WriteLine("  │ Layer │ Mechanism              │ Catches                            │");
        Console.WriteLine("  ├─────────────────────────────────────────────────────────────────────┤");
        Console.WriteLine("  │   1   │ TypeRef Table Scan     │ DllImportAttribute, Marshal,       │");
        Console.WriteLine("  │       │                        │ NativeMemory, GCHandle,            │");
        Console.WriteLine("  │       │                        │ NativeLibrary, SafeHandle,         │");
        Console.WriteLine("  │       │                        │ MemoryMarshal, Unsafe, etc.        │");
        Console.WriteLine("  ├─────────────────────────────────────────────────────────────────────┤");
        Console.WriteLine("  │   2   │ ScanUnsafeMarkers      │ [module: UnverifiableCode] attr    │");
        Console.WriteLine("  │       │                        │ MethodAttributes.PinvokeImpl       │");
        Console.WriteLine("  │       │                        │ (set by compiler for [DllImport])  │");
        Console.WriteLine("  ├─────────────────────────────────────────────────────────────────────┤");
        Console.WriteLine("  │   3   │ #US Heap String Scan   │ Strings like                       │");
        Console.WriteLine("  │       │                        │ \"System.Runtime.InteropServices.*\" │");
        Console.WriteLine("  │       │                        │ (catches indirect string loading)   │");
        Console.WriteLine("  └─────────────────────────────────────────────────────────────────────┘");
        Console.WriteLine();
        Console.WriteLine("  Even if an attacker bypasses ONE layer, the others catch it:");
        Console.WriteLine("  - Remove [DllImport] attribute? → PinvokeImpl flag still set in MethodDef");
        Console.WriteLine("  - Avoid direct TypeRef? → String scan catches type name in #US heap");
        Console.WriteLine("  - Obfuscate strings? → TypeRef/PinvokeImpl still visible in PE metadata");
        Console.WriteLine();
        Console.WriteLine("  ⚠️  IMPORTANT: These layers CANNOT be exempted by PluginCapability.");
        Console.WriteLine("  Even declaring [PluginCapability(Capability.Network)] or any other");
        Console.WriteLine("  capability will NOT unblock P/Invoke or unsafe code.");
        Console.WriteLine("  They are UNCONDITIONALLY FORBIDDEN.");
        Console.WriteLine();
    }

    public void OnStop()
    {
        Console.WriteLine("\n[ForbiddenPInvoke] Plugin stopped. No actual native operations were performed.");
    }

    public void OnUnload()
    {
    }
}
