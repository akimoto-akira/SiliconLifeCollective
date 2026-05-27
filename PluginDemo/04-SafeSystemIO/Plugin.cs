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

using System.IO;
using System.IO.Compression;
using System.Text;
using SiliconLife.Collective;

namespace SiliconLife.Demo.SafeSystemIO;

/// <summary>
/// Demonstrates using System.IO types that are on the <c>SystemIOAllowedTypes</c> whitelist.
/// These types perform pure in-memory operations and do NOT directly access the file system,
/// so plugins can use them freely without going through <see cref="PermissionedStreamFactory"/>.
/// <para>
/// The demo builds a complete pipeline:
/// raw string → BinaryWriter → MemoryStream → GZipStream → compressed MemoryStream →
/// GZipStream (decompress) → BinaryReader → original string.
/// </para>
/// </summary>
public class SafeSystemIOPlugin : IPlugin
{
    public string Id => "com.siliconlife.demo.safesystemio";
    public string Version => "1.0.0";
    public string GetName(Language language) => "Safe System.IO Demo";
    public string GetDescription(Language language) =>
        "Demonstrates System.IO whitelist types: MemoryStream, BinaryReader/Writer, GZipStream. " +
        "Shows why FileStream requires PermissionedStreamFactory.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    public void OnLoad()
    {
    }

    public void OnStart()
    {
        DemoMemoryStream();
        DemoCompressionPipeline();
        DemoBinaryReaderWriter();
    }

    private void DemoMemoryStream()
    {
        using var ms = new MemoryStream();
        byte[] data = Encoding.UTF8.GetBytes("Hello from MemoryStream!");
        ms.Write(data, 0, data.Length);
        ms.Position = 0;

        byte[] buffer = new byte[data.Length];
        ms.Read(buffer, 0, buffer.Length);
        string result = Encoding.UTF8.GetString(buffer);

        Console.WriteLine($"[SafeSystemIO] MemoryStream demo: {result}");
    }

    private void DemoCompressionPipeline()
    {
        string original = "This is a test string that will be compressed and then decompressed.";

        using var rawData = new MemoryStream(Encoding.UTF8.GetBytes(original));
        using var compressedData = new MemoryStream();

        using (var gzip = new GZipStream(compressedData, CompressionLevel.Optimal, leaveOpen: true))
        {
            rawData.CopyTo(gzip);
        }

        compressedData.Position = 0;
        using var decompressedData = new MemoryStream();
        using (var gzip = new GZipStream(compressedData, CompressionMode.Decompress))
        {
            gzip.CopyTo(decompressedData);
        }

        string roundtrip = Encoding.UTF8.GetString(decompressedData.ToArray());
        Console.WriteLine($"[SafeSystemIO] Compression pipeline: {roundtrip}");
        Console.WriteLine($"[SafeSystemIO] Original {original.Length} bytes → Compressed {compressedData.Length} bytes → Decompressed {roundtrip.Length} chars");
    }

    private void DemoBinaryReaderWriter()
    {
        using var ms = new MemoryStream();
        using (var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true))
        {
            writer.Write(42);
            writer.Write(3.14);
            writer.Write("BinaryWriter demo");
        }

        ms.Position = 0;
        using (var reader = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true))
        {
            int intValue = reader.ReadInt32();
            double doubleValue = reader.ReadDouble();
            string stringValue = reader.ReadString();
            Console.WriteLine($"[SafeSystemIO] BinaryReader: int={intValue}, double={doubleValue}, string={stringValue}");
        }
    }

    public void OnStop()
    {
    }

    public void OnUnload()
    {
    }
}
