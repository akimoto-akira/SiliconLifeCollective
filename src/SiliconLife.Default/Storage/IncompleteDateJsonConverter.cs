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
using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// JSON converter for <see cref="IncompleteDate"/>.
/// Required because IncompleteDate is a readonly struct with no parameterless constructor.
/// </summary>
public class IncompleteDateJsonConverter : JsonConverter<IncompleteDate>
{
    public override IncompleteDate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected StartObject token for IncompleteDate.");

        int year = 1;
        int? month = null;
        int? day = null;
        int? hour = null;
        int? minute = null;
        int? second = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType != JsonTokenType.PropertyName)
                continue;

            string propertyName = reader.GetString() ?? string.Empty;
            reader.Read();

            switch (propertyName)
            {
                case "Year":
                    year = reader.GetInt32();
                    break;
                case "Month":
                    month = reader.TokenType == JsonTokenType.Null ? null : reader.GetInt32();
                    break;
                case "Day":
                    day = reader.TokenType == JsonTokenType.Null ? null : reader.GetInt32();
                    break;
                case "Hour":
                    hour = reader.TokenType == JsonTokenType.Null ? null : reader.GetInt32();
                    break;
                case "Minute":
                    minute = reader.TokenType == JsonTokenType.Null ? null : reader.GetInt32();
                    break;
                case "Second":
                    second = reader.TokenType == JsonTokenType.Null ? null : reader.GetInt32();
                    break;
            }
        }

        return new IncompleteDate(year, month, day, hour, minute, second);
    }

    public override void Write(Utf8JsonWriter writer, IncompleteDate value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("Year", value.Year);
        if (value.Month.HasValue)
            writer.WriteNumber("Month", value.Month.Value);
        else
            writer.WriteNull("Month");
        if (value.Day.HasValue)
            writer.WriteNumber("Day", value.Day.Value);
        else
            writer.WriteNull("Day");
        if (value.Hour.HasValue)
            writer.WriteNumber("Hour", value.Hour.Value);
        else
            writer.WriteNull("Hour");
        if (value.Minute.HasValue)
            writer.WriteNumber("Minute", value.Minute.Value);
        else
            writer.WriteNull("Minute");
        if (value.Second.HasValue)
            writer.WriteNumber("Second", value.Second.Value);
        else
            writer.WriteNull("Second");
        writer.WriteEndObject();
    }
}
