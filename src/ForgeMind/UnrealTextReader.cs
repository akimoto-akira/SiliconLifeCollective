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

using System.Text;

namespace ForgeMind;

/// <summary>
/// Text file loading helpers with BOM-based encoding detection.
/// Ported from UnrealEngineCacheCtrl.Utilities.FileUtilities.
/// </summary>
internal static class UnrealTextReader
{
    /// <summary>
    /// Loads a text file with the specified encoding.
    /// Returns <see langword="null"/> on any failure.
    /// </summary>
    public static string? LoadString(FileInfo file, Encoding encoding)
    {
        try
        {
            byte[] binary = File.ReadAllBytes(file.FullName);
            return encoding.GetString(binary);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Loads a text file, auto-detecting the encoding from the BOM
    /// (UTF-8 / UTF-16 LE / UTF-16 BE / UTF-32), falling back to a
    /// heuristic UTF-8 validation. Returns <see langword="null"/> on failure.
    /// </summary>
    public static string? LoadStringAuto(FileInfo fileInfo)
    {
        if (!fileInfo.Exists)
            return null;

        try
        {
            byte[] binary = File.ReadAllBytes(fileInfo.FullName);
            Encoding encoding = DetectEncoding(binary, out int startIndex);
            return encoding.GetString(binary, startIndex, binary.Length - startIndex);
        }
        catch (UnauthorizedAccessException)
        {
            // Insufficient permission — try fallback encodings
            return TryFallbackRead(fileInfo);
        }
        catch (IOException)
        {
            // File locked or other IO error — try fallback encodings
            return TryFallbackRead(fileInfo);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Detects the file encoding from the leading bytes.
    /// </summary>
    private static Encoding DetectEncoding(byte[] binary, out int startIndex)
    {
        startIndex = 0;

        if (binary.Length == 0)
            return Encoding.UTF8;

        // UTF-32 LE BOM must be checked before UTF-16 LE (shares the FF FE prefix)
        if (binary.Length >= 4 && binary[0] == 0xff && binary[1] == 0xfe && binary[2] == 0x00 && binary[3] == 0x00)
        {
            startIndex = 4;
            return Encoding.UTF32;
        }

        // UTF-8 BOM
        if (binary.Length >= 3 && binary[0] == 0xef && binary[1] == 0xbb && binary[2] == 0xbf)
        {
            startIndex = 3;
            return Encoding.UTF8;
        }

        // Unicode (UTF-16 LE) BOM
        if (binary.Length >= 2 && binary[0] == 0xff && binary[1] == 0xfe)
        {
            startIndex = 2;
            return Encoding.Unicode;
        }

        // Big Endian Unicode (UTF-16 BE) BOM
        if (binary.Length >= 2 && binary[0] == 0xfe && binary[1] == 0xff)
        {
            startIndex = 2;
            return Encoding.BigEndianUnicode;
        }

        // UTF-32 BE BOM
        if (binary.Length >= 4 && binary[0] == 0x00 && binary[1] == 0x00 && binary[2] == 0xfe && binary[3] == 0xff)
        {
            startIndex = 4;
            return new UTF32Encoding(bigEndian: true, byteOrderMark: true);
        }

        // No BOM — validate as UTF-8, otherwise fall back to UTF-8 anyway
        // (on .NET Core Encoding.Default is always UTF-8)
        return Encoding.UTF8;
    }

    /// <summary>
    /// Fallback read that retries several encodings.
    /// </summary>
    private static string? TryFallbackRead(FileInfo fileInfo)
    {
        Encoding[] fallbackEncodings =
        [
            Encoding.UTF8,
            Encoding.Unicode,
            Encoding.BigEndianUnicode,
            Encoding.ASCII
        ];

        foreach (Encoding encoding in fallbackEncodings)
        {
            string? content = LoadString(fileInfo, encoding);
            if (content != null)
                return content;
        }

        return null;
    }
}