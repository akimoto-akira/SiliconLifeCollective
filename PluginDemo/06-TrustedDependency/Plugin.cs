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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SiliconLife.Collective;

namespace SiliconLife.Demo.TrustedDependency;

/// <summary>
/// Demonstrates how a plugin can freely use Newtonsoft.Json — a library that internally
/// relies on reflection, dynamic type resolution, and other techniques that would normally
/// be flagged by the PluginLoader security scanner.
/// <para>
/// Because "Newtonsoft.Json" is listed in <c>PluginLoader.TrustedAssemblies</c>, the scanner:
/// <list type="number">
///   <item>Skips the entire Newtonsoft.Json DLL when scanning dependencies (Layer 0 whitelist)</item>
///   <item>Collects all TypeRefs from Newtonsoft.Json via <c>CollectTrustedTypeRefs</c>,
///         then exempts those references in the plugin's own DLL (Layer 0.5 transitive exemption)</item>
/// </list>
/// This means the plugin can reference Newtonsoft.Json types without triggering violations,
/// even though Newtonsoft.Json internally references System.Reflection, System.IO, etc.
/// </para>
/// </summary>
public class TrustedDependencyPlugin : IPlugin
{
    public string Id => "com.siliconlife.demo.trusteddependency";
    public string Version => "1.0.0";
    public string GetName(Language language) => "Trusted Dependency Demo";
    public string GetDescription(Language language) =>
        "Demonstrates using Newtonsoft.Json (a trusted assembly) for serialization/deserialization. " +
        "Shows that TrustedAssemblies bypass the security scanner even though they use reflection internally.";
    public string GetAuthor(Language language) => "SiliconLife Collective";

    public void OnLoad()
    {
        // Newtonsoft.Json is a trusted assembly — no PluginCapability declaration needed.
    }

    public void OnStart()
    {
        DemoSerializeObject();
        DemoDeserializeJson();
        DemoJObjectDynamic();
    }

    /// <summary>
    /// Serialize a POCO to JSON using JsonConvert.SerializeObject.
    /// Internally, Newtonsoft.Json uses reflection to enumerate properties —
    /// but since it's a trusted assembly, no violation is raised.
    /// </summary>
    private void DemoSerializeObject()
    {
        var config = new PluginConfig
        {
            Name = "TrustedDependencyDemo",
            MaxRetries = 3,
            Timeout = TimeSpan.FromSeconds(30),
            Tags = new[] { "demo", "serialization", "trusted" }
        };

        string json = JsonConvert.SerializeObject(config, Formatting.Indented);
        Console.WriteLine($"[TrustedDependency] Serialized config:\n{json}");
    }

    /// <summary>
    /// Deserialize JSON back to a strongly-typed object.
    /// Newtonsoft.Json calls Activator.CreateInstance and property setters via reflection —
    /// all exempted because the library is trusted.
    /// </summary>
    private void DemoDeserializeJson()
    {
        const string json = """
            {
                "Name": "Roundtrip",
                "MaxRetries": 5,
                "Timeout": "00:01:00",
                "Tags": ["alpha", "beta"]
            }
            """;

        var config = JsonConvert.DeserializeObject<PluginConfig>(json);
        Console.WriteLine($"[TrustedDependency] Deserialized: Name={config?.Name}, Retries={config?.MaxRetries}, Tags=[{string.Join(", ", config?.Tags ?? Array.Empty<string>())}]");
    }

    /// <summary>
    /// Use JObject for dynamic JSON manipulation without a pre-defined type.
    /// JObject/JArray/JToken heavily use dynamic dispatch and System.Linq.Expressions —
    /// safe because Newtonsoft.Json is in the trusted whitelist.
    /// </summary>
    private void DemoJObjectDynamic()
    {
        var obj = new JObject
        {
            ["plugin"] = "TrustedDependencyDemo",
            ["version"] = 1,
            ["features"] = new JArray("serialization", "dynamic-json", "linq-queries")
        };

        // Query with LINQ-to-JSON
        string? pluginName = obj["plugin"]?.ToString();
        int featureCount = obj["features"]?.Count() ?? 0;

        Console.WriteLine($"[TrustedDependency] JObject: plugin={pluginName}, features={featureCount}");

        // Merge additional data
        obj.Merge(JObject.Parse("{\"author\": \"SiliconLife\", \"version\": 2}"), new JsonMergeSettings
        {
            MergeArrayHandling = MergeArrayHandling.Union
        });

        Console.WriteLine($"[TrustedDependency] After merge: {obj.ToString(Formatting.None)}");
    }

    public void OnStop()
    {
    }

    public void OnUnload()
    {
    }
}

/// <summary>
/// A simple configuration POCO used to demonstrate JSON serialization roundtrip.
/// </summary>
internal class PluginConfig
{
    public string Name { get; set; } = string.Empty;
    public int MaxRetries { get; set; }
    public TimeSpan Timeout { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();
}
