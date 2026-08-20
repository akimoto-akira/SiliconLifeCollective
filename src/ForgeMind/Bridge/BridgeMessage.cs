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

namespace ForgeMind.Bridge;

/// <summary>Envelope type of a bridge message (wire format is lowercase).</summary>
[JsonConverter(typeof(BridgeMessageTypeConverter))]
internal enum BridgeMessageType
{
    Request,
    Response,
    Event
}

/// <summary>Serializes <see cref="BridgeMessageType"/> as lowercase wire strings.</summary>
internal sealed class BridgeMessageTypeConverter : JsonConverter<BridgeMessageType>
{
    public override BridgeMessageType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.GetString()?.ToLowerInvariant() switch
        {
            "request" => BridgeMessageType.Request,
            "response" => BridgeMessageType.Response,
            "event" => BridgeMessageType.Event,
            _ => throw new JsonException("Unknown bridge message type")
        };

    public override void Write(Utf8JsonWriter writer, BridgeMessageType value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            BridgeMessageType.Request => "request",
            BridgeMessageType.Response => "response",
            _ => "event"
        });
}

/// <summary>
/// Unified bridge message envelope. Identical structure in both directions:
/// request/response pairs share an id; events carry no id.
/// </summary>
internal sealed class BridgeMessage
{
    [JsonPropertyName("v")] public int Version { get; set; } = 1;

    [JsonPropertyName("type")] public BridgeMessageType Type { get; set; }

    [JsonPropertyName("id")] public string? Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; } = "";

    [JsonPropertyName("payload")] public JsonElement? Payload { get; set; }

    [JsonPropertyName("error")] public string? Error { get; set; }

    public static BridgeMessage NewRequest(string id, string name, object? payload = null) => new()
    {
        Type = BridgeMessageType.Request,
        Id = id,
        Name = name,
        Payload = payload == null ? null : JsonSerializer.SerializeToElement(payload)
    };

    public static BridgeMessage NewResponse(string id, string name, object? payload = null) => new()
    {
        Type = BridgeMessageType.Response,
        Id = id,
        Name = name,
        Payload = payload == null ? null : JsonSerializer.SerializeToElement(payload)
    };

    public static BridgeMessage NewErrorResponse(string id, string name, string error) => new()
    {
        Type = BridgeMessageType.Response,
        Id = id,
        Name = name,
        Error = error
    };

    public static BridgeMessage NewEvent(string name, object? payload = null) => new()
    {
        Type = BridgeMessageType.Event,
        Name = name,
        Payload = payload == null ? null : JsonSerializer.SerializeToElement(payload)
    };
}
