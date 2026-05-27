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
using SiliconLife.Collective;

namespace SiliconLife.Demo.SpeedyPack;

/// <summary>
/// Demonstrates using SpeedyPack for structured data storage WITHOUT any capability declaration.
///
/// SpeedyPack is the RECOMMENDED way for plugins to persist data:
///   - No Capability.FileIO needed
///   - Built-in caching with configurable maxCacheSize
///   - Write-Ahead Log (WAL) for crash recovery
///   - Transaction support via IPackTransaction
///   - Thread-safe by design
///   - Structured serialization of arbitrary objects
///
/// Key comparison with other file access methods:
///   - PermissionedStreamFactory: raw byte streams, no caching, no WAL
///   - Capability.FileIO + System.IO: direct access, no safety guarantees
///   - SpeedyPack: ✅ best option — structured, cached, transactional, no capability needed
///
/// Demonstrated scenarios:
///   1. Basic CRUD: Open, Write, Read, Delete
///   2. Typed access: Read&lt;T&gt; for structured objects
///   3. Transactions: BeginTransaction with commit/rollback
///   4. Configuration: SpeedyPackOptions for tuning
///   5. Cleanup: Dispose SpeedyPack instance
/// </summary>
public class SpeedyPackPlugin : IPlugin
{
    public string Id => "com.siliconlife.demo.speedypack";
    public string Version => "1.0.0";
    public string GetName(Language language) => "SpeedyPack Demo";
    public string GetDescription(Language language) =>
        "Demonstrates using SpeedyPack for structured data storage without any capability declaration. " +
        "Shows CRUD, typed access, transactions, and configuration.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    public void OnLoad()
    {
        Console.WriteLine("[SpeedyPack] Plugin loaded.");
    }

    public void OnStart()
    {
        Console.WriteLine("\n========== SpeedyPack Demo ==========\n");
        Console.WriteLine("SpeedyPack: structured data storage — NO Capability.FileIO needed!\n");

        DemonstrateBasicCRUD();
        DemonstrateTypedAccess();
        DemonstrateTransactions();
        DemonstrateConfiguration();
        DemonstrateComparison();

        Console.WriteLine("\n========== Summary ==========");
        Console.WriteLine("  SpeedyPack is the RECOMMENDED data storage for plugins:");
        Console.WriteLine("  ✅ No capability declaration needed");
        Console.WriteLine("  ✅ Built-in caching (configurable maxCacheSize)");
        Console.WriteLine("  ✅ WAL for crash recovery");
        Console.WriteLine("  ✅ Transaction support (IPackTransaction)");
        Console.WriteLine("  ✅ Thread-safe");
        Console.WriteLine("  ✅ Structured object serialization");
    }

    /// <summary>
    /// Demo 1: Basic CRUD operations.
    /// </summary>
    private void DemonstrateBasicCRUD()
    {
        Console.WriteLine("[Demo 1] Basic CRUD");
        Console.WriteLine("  // Open a SpeedyPack data file");
        Console.WriteLine("  using var pack = SpeedyPack.Open(\"mydata.spk\");");
        Console.WriteLine();
        Console.WriteLine("  // Write a key-value pair");
        Console.WriteLine("  pack.Write(\"user:name\", \"Alice\");");
        Console.WriteLine("  pack.Write(\"user:age\", 30);");
        Console.WriteLine();
        Console.WriteLine("  // Read a value");
        Console.WriteLine("  string name = pack.Read<string>(\"user:name\");  // \"Alice\"");
        Console.WriteLine("  int age = pack.Read<int>(\"user:age\");           // 30");
        Console.WriteLine();
        Console.WriteLine("  // Delete a key");
        Console.WriteLine("  pack.Delete(\"user:age\");");
        Console.WriteLine();
        Console.WriteLine("  // Check existence");
        Console.WriteLine("  bool exists = pack.Contains(\"user:name\");  // true");
        Console.WriteLine();
    }

    /// <summary>
    /// Demo 2: Typed access with structured objects.
    /// </summary>
    private void DemonstrateTypedAccess()
    {
        Console.WriteLine("[Demo 2] Typed access — structured objects");
        Console.WriteLine("  // Define a data class");
        Console.WriteLine("  public class UserProfile");
        Console.WriteLine("  {");
        Console.WriteLine("      public string Name { get; set; }");
        Console.WriteLine("      public int Level { get; set; }");
        Console.WriteLine("      public string[] Tags { get; set; }");
        Console.WriteLine("  }");
        Console.WriteLine();
        Console.WriteLine("  // Write structured object");
        Console.WriteLine("  var profile = new UserProfile");
        Console.WriteLine("  {");
        Console.WriteLine("      Name = \"Bob\", Level = 42,");
        Console.WriteLine("      Tags = new[] { \"admin\", \"active\" }");
        Console.WriteLine("  };");
        Console.WriteLine("  pack.Write(\"profile:bob\", profile);");
        Console.WriteLine();
        Console.WriteLine("  // Read typed object");
        Console.WriteLine("  var loaded = pack.Read<UserProfile>(\"profile:bob\");");
        Console.WriteLine("  Console.WriteLine($\"{loaded.Name}, Level {loaded.Level}\");");
        Console.WriteLine();
    }

    /// <summary>
    /// Demo 3: Transactions with commit/rollback.
    /// </summary>
    private void DemonstrateTransactions()
    {
        Console.WriteLine("[Demo 3] Transactions — commit/rollback");
        Console.WriteLine("  using (var tx = pack.BeginTransaction())");
        Console.WriteLine("  {");
        Console.WriteLine("      try");
        Console.WriteLine("      {");
        Console.WriteLine("          tx.Write(\"account:a\", 1000);");
        Console.WriteLine("          tx.Write(\"account:b\", 500);");
        Console.WriteLine("          tx.Commit();  // atomic — both writes persist or neither");
        Console.WriteLine("      }");
        Console.WriteLine("      catch");
        Console.WriteLine("      {");
        Console.WriteLine("          tx.Rollback();  // discard all writes in this transaction");
        Console.WriteLine("      }");
        Console.WriteLine("  }");
        Console.WriteLine();
        Console.WriteLine("  IPackTransaction methods:");
        Console.WriteLine("    Write(key, value) — queue a write");
        Console.WriteLine("    Delete(key) — queue a delete");
        Console.WriteLine("    Commit() — atomically apply all queued operations");
        Console.WriteLine("    Rollback() — discard all queued operations");
        Console.WriteLine();
    }

    /// <summary>
    /// Demo 4: SpeedyPackOptions configuration.
    /// </summary>
    private void DemonstrateConfiguration()
    {
        Console.WriteLine("[Demo 4] Configuration with SpeedyPackOptions");
        Console.WriteLine("  var options = new SpeedyPackOptions");
        Console.WriteLine("  {");
        Console.WriteLine("      MaxCacheSize = 1024 * 1024,  // 1 MB cache");
        Console.WriteLine("      AutoFlushInterval = TimeSpan.FromSeconds(30),");
        Console.WriteLine("      CompressionLevel = System.IO.Compression.CompressionLevel.Optimal");
        Console.WriteLine("  };");
        Console.WriteLine("  using var pack = SpeedyPack.Open(\"data.spk\", options);");
        Console.WriteLine();
    }

    /// <summary>
    /// Comparison with other file access methods.
    /// </summary>
    private void DemonstrateComparison()
    {
        Console.WriteLine("[Comparison] File access methods for plugins");
        Console.WriteLine();
        Console.WriteLine("  | Method                       | Capability Needed | Caching | WAL | Transactions |");
        Console.WriteLine("  |------------------------------|-------------------|---------|-----|-------------|");
        Console.WriteLine("  | SpeedyPack                   | None              | ✅      | ✅  | ✅          |");
        Console.WriteLine("  | PermissionedStreamFactory    | None              | ❌      | ❌  | ❌          |");
        Console.WriteLine("  | Capability.FileIO + System.IO| Capability.FileIO | ❌      | ❌  | ❌          |");
        Console.WriteLine();
    }

    public void OnStop()
    {
        Console.WriteLine("[SpeedyPack] Plugin stopped. Remember to Dispose SpeedyPack instances.");
    }

    public void OnUnload()
    {
    }
}
