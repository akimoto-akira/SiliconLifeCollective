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
/// Custom JSON converter for <see cref="Guid"/> that supports both string
/// and byte-array representations during deserialization.
/// Serialization always outputs the standard string format.
/// </summary>
public class GuidConverter : JsonConverter<Guid>
{
    /// <summary>
    /// Reads a <see cref="Guid"/> from JSON.
    /// Supports string format (e.g. "550e8400-e29b-41d4-a716-446655440000")
    /// and byte-array format (16-element JSON array).
    /// Returns <see cref="Guid.Empty"/> for empty strings or unrecognised tokens.
    /// </summary>
    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string? value = reader.GetString();
            if (string.IsNullOrEmpty(value))
            {
                return Guid.Empty;
            }
            return Guid.Parse(value);
        }

        if (reader.TokenType == JsonTokenType.StartArray)
        {
            byte[] bytes = new byte[16];
            int i = 0;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }
                bytes[i++] = reader.GetByte();
            }
            return new Guid(bytes);
        }

        return Guid.Empty;
    }

    /// <summary>
    /// Writes a <see cref="Guid"/> to JSON in standard string format.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
