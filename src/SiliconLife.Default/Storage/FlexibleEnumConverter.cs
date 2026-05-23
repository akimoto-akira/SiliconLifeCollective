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

namespace SiliconLife.Default.Storage;

/// <summary>
/// Flexible enum converter factory that supports both string and number formats during deserialization.
/// During serialization, enums are written as strings for better readability.
/// During deserialization, both string names and numeric values are accepted.
/// This converter automatically applies to all enum types.
/// </summary>
public class FlexibleEnumConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum || (Nullable.GetUnderlyingType(typeToConvert)?.IsEnum ?? false);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type enumType = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;
        Type converterType = typeof(FlexibleEnumConverterInner<>).MakeGenericType(enumType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }

    private class FlexibleEnumConverterInner<T> : JsonConverter<T> where T : struct, Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string enumString = reader.GetString() ?? string.Empty;
                if (Enum.TryParse<T>(enumString, ignoreCase: true, out T result))
                {
                    return result;
                }
                throw new JsonException($"Unable to convert \"{enumString}\" to enum {typeof(T).Name}.");
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                int enumValue = reader.GetInt32();
                if (Enum.IsDefined(typeof(T), enumValue))
                {
                    return (T)(object)enumValue;
                }
                throw new JsonException($"Unable to convert {enumValue} to enum {typeof(T).Name}. Value is not defined.");
            }

            throw new JsonException($"Unexpected token type {reader.TokenType} when parsing enum {typeof(T).Name}.");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
