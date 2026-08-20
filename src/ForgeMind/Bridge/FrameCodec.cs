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

using System.Buffers.Binary;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ForgeMind.Bridge;

/// <summary>
/// Length-prefix framing for the bridge TCP channel.
/// Frame layout: 4-byte little-endian unsigned payload length + UTF-8 JSON
/// envelope (length excludes the header). Implemented on both ends.
/// </summary>
internal static class FrameCodec
{
    /// <summary>Hard cap for a single frame payload (16 MB is far beyond any protocol message).</summary>
    public const int MaxFrameBytes = 16 * 1024 * 1024;

    // Null envelope fields (id/payload/error) are omitted on the wire
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>Reads exactly one frame from the stream; returns null on clean EOF.</summary>
    public static async Task<BridgeMessage?> ReadFrameAsync(Stream stream, CancellationToken ct)
    {
        byte[] header = await ReadExactAsync(stream, 4, ct).ConfigureAwait(false);
        if (header == null)
            return null;

        int length = (int)BinaryPrimitives.ReadUInt32LittleEndian(header);
        if (length <= 0 || length > MaxFrameBytes)
            throw new InvalidDataException($"Invalid frame length: {length}");

        byte[] payload = await ReadExactAsync(stream, length, ct).ConfigureAwait(false)
            ?? throw new EndOfStreamException("Connection closed mid-frame");

        return JsonSerializer.Deserialize<BridgeMessage>(payload, JsonOptions);
    }

    /// <summary>Writes one frame atomically (header + payload in one flush).</summary>
    public static async Task WriteFrameAsync(Stream stream, BridgeMessage message, CancellationToken ct)
    {
        byte[] payload = JsonSerializer.SerializeToUtf8Bytes(message, JsonOptions);
        if (payload.Length > MaxFrameBytes)
            throw new InvalidDataException($"Frame too large: {payload.Length}");

        byte[] header = new byte[4];
        BinaryPrimitives.WriteUInt32LittleEndian(header, (uint)payload.Length);

        await stream.WriteAsync(header, ct).ConfigureAwait(false);
        await stream.WriteAsync(payload, ct).ConfigureAwait(false);
        await stream.FlushAsync(ct).ConfigureAwait(false);
    }

    /// <summary>Reads exactly count bytes; returns null when the stream ends before any byte.</summary>
    private static async Task<byte[]?> ReadExactAsync(Stream stream, int count, CancellationToken ct)
    {
        var buffer = new byte[count];
        int read = 0;
        while (read < count)
        {
            int n = await stream.ReadAsync(buffer.AsMemory(read, count - read), ct).ConfigureAwait(false);
            if (n <= 0)
                return read == 0 ? null : throw new EndOfStreamException("Connection closed mid-frame");
            read += n;
        }

        return buffer;
    }
}
