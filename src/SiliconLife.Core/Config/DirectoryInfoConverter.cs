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
/// Custom JSON converter for <see cref="DirectoryInfo"/> that serializes
/// the directory path as a string.
/// </summary>
public class DirectoryInfoConverter : JsonConverter<DirectoryInfo>
{
    /// <summary>
    /// Reads a <see cref="DirectoryInfo"/> from JSON string path.
    /// Returns a new DirectoryInfo with the path, or an empty DirectoryInfo for null/empty strings.
    /// </summary>
    public override DirectoryInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string? path = reader.GetString();
            if (string.IsNullOrEmpty(path))
            {
                return new DirectoryInfo(".");
            }
            return new DirectoryInfo(path);
        }

        return new DirectoryInfo(".");
    }

    /// <summary>
    /// Writes a <see cref="DirectoryInfo"/> to JSON as its full path string.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, DirectoryInfo value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.FullName);
    }
}
