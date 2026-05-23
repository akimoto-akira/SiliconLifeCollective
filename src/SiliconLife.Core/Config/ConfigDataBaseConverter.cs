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

using System.Text.Json;
using System.Text.Json.Serialization;

namespace SiliconLife.Collective;

/// <summary>
/// Polymorphic JSON converter for <see cref="ConfigDataBase"/> that dispatches
/// deserialization based on a <c>"configType"</c> discriminator property.
/// Concrete config types must be registered via <see cref="RegisterConfigType"/>
/// before deserialization.
/// </summary>
public class ConfigDataBaseConverter : JsonConverter<ConfigDataBase>
{
    private static readonly Dictionary<string, Type> _configTypes = new();

    /// <summary>
    /// Registers a concrete <see cref="ConfigDataBase"/> subtype for a given
    /// <paramref name="configType"/> discriminator string.
    /// </summary>
    /// <param name="configType">The discriminator value found in JSON.</param>
    /// <param name="type">The concrete CLR type to deserialize into.</param>
    public static void RegisterConfigType(string configType, Type type)
    {
        _configTypes[configType] = type;
    }

    /// <summary>
    /// Reads a JSON object, extracts the <c>"configType"</c> discriminator,
    /// resolves the registered concrete type, and deserializes the payload.
    /// </summary>
    /// <exception cref="JsonException">
    /// Thrown when the <c>"configType"</c> property is missing or the discriminator
    /// value is not registered.
    /// </exception>
    public override ConfigDataBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        JsonElement root = doc.RootElement;

        if (!root.TryGetProperty("configType", out JsonElement typeProperty))
        {
            throw new JsonException("Missing 'configType' property for polymorphic deserialization");
        }

        string configType = typeProperty.GetString() ?? "";

        if (!_configTypes.TryGetValue(configType, out Type? targetType))
        {
            throw new JsonException($"Unknown config type: {configType}");
        }

        string json = root.GetRawText();
        return (ConfigDataBase?)JsonSerializer.Deserialize(json, targetType, options);
    }

    /// <summary>
    /// Serializes a <see cref="ConfigDataBase"/> instance using its actual runtime type
    /// so that the concrete JSON structure (including the discriminator) is preserved.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, ConfigDataBase value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
