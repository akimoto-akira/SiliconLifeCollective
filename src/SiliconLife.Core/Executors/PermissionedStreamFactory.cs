// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace SiliconLife.Collective;

using System.IO;
using System.Buffers.Binary;

/// <summary>
/// Enhanced permissioned stream: provides secure file access and supports direct read/write of various data types in big-endian/little-endian
/// Byte order is distinguished by method name: ReadBigEndianInt32() / ReadLittleEndianInt32()
/// </summary>
public sealed class PermissionedStream : Stream
{
    private readonly Stream _innerStream;
    
    public PermissionedStream(Stream innerStream)
    {
        _innerStream = innerStream ?? throw new ArgumentNullException(nameof(innerStream));
    }
    
    // Base Stream overrides
    public override bool CanRead => _innerStream.CanRead;
    public override bool CanSeek => _innerStream.CanSeek;
    public override bool CanWrite => _innerStream.CanWrite;
    public override long Length => _innerStream.Length;
    public override long Position 
    { 
        get => _innerStream.Position; 
        set => _innerStream.Position = value; 
    }
    
    public override void Flush() => _innerStream.Flush();
    public override int Read(byte[] buffer, int offset, int count) => _innerStream.Read(buffer, offset, count);
    public override long Seek(long offset, SeekOrigin origin) => _innerStream.Seek(offset, origin);
    public override void SetLength(long value) => _innerStream.SetLength(value);
    public override void Write(byte[] buffer, int offset, int count) => _innerStream.Write(buffer, offset, count);
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _innerStream?.Dispose();
        }
        base.Dispose(disposing);
    }
    
    // ========== Big-Endian Read Methods ==========
    public short ReadBigEndianInt16()
    {
        Span<byte> buffer = stackalloc byte[2];
        _innerStream.ReadExactly(buffer);
        return BinaryPrimitives.ReadInt16BigEndian(buffer);
    }
    
    public ushort ReadBigEndianUInt16()
    {
        Span<byte> buffer = stackalloc byte[2];
        _innerStream.ReadExactly(buffer);
        return BinaryPrimitives.ReadUInt16BigEndian(buffer);
    }
    
    public int ReadBigEndianInt32()
    {
        Span<byte> buffer = stackalloc byte[4];
        _innerStream.ReadExactly(buffer);
        return BinaryPrimitives.ReadInt32BigEndian(buffer);
    }
    
    public uint ReadBigEndianUInt32()
    {
        Span<byte> buffer = stackalloc byte[4];
        _innerStream.ReadExactly(buffer);
        return BinaryPrimitives.ReadUInt32BigEndian(buffer);
    }
    
    public long ReadBigEndianInt64()
    {
        Span<byte> buffer = stackalloc byte[8];
        _innerStream.ReadExactly(buffer);
        return BinaryPrimitives.ReadInt64BigEndian(buffer);
    }
    
    public ulong ReadBigEndianUInt64()
    {
        Span<byte> buffer = stackalloc byte[8];
        _innerStream.ReadExactly(buffer);
        return BinaryPrimitives.ReadUInt64BigEndian(buffer);
    }
    
    public float ReadBigEndianSingle()
    {
        Span<byte> buffer = stackalloc byte[4];
        _innerStream.ReadExactly(buffer);
        return BinaryPrimitives.ReadSingleBigEndian(buffer);
    }
    
    public double ReadBigEndianDouble()
    {
        Span<byte> buffer = stackalloc byte[8];
        _innerStream.ReadExactly(buffer);
        return BinaryPrimitives.ReadDoubleBigEndian(buffer);
    }
    
    // ========== Little-Endian Read Methods ==========
    public short ReadLittleEndianInt16()
    {
        Span<byte> buffer = stackalloc byte[2];
        _innerStream.ReadExactly(buffer);
        return BinaryPrimitives.ReadInt16LittleEndian(buffer);
    }
    
    public ushort ReadLittleEndianUInt16()
    {
        Span<byte> buffer = stackalloc byte[2];
        _innerStream.ReadExactly(buffer);
        return BinaryPrimitives.ReadUInt16LittleEndian(buffer);
    }
    
    public int ReadLittleEndianInt32()
    {
        Span<byte> buffer = stackalloc byte[4];
        _innerStream.ReadExactly(buffer);
        return BinaryPrimitives.ReadInt32LittleEndian(buffer);
    }
    
    public uint ReadLittleEndianUInt32()
    {
        Span<byte> buffer = stackalloc byte[4];
        _innerStream.ReadExactly(buffer);
        return BinaryPrimitives.ReadUInt32LittleEndian(buffer);
    }
    
    public long ReadLittleEndianInt64()
    {
        Span<byte> buffer = stackalloc byte[8];
        _innerStream.ReadExactly(buffer);
        return BinaryPrimitives.ReadInt64LittleEndian(buffer);
    }
    
    public ulong ReadLittleEndianUInt64()
    {
        Span<byte> buffer = stackalloc byte[8];
        _innerStream.ReadExactly(buffer);
        return BinaryPrimitives.ReadUInt64LittleEndian(buffer);
    }
    
    public float ReadLittleEndianSingle()
    {
        Span<byte> buffer = stackalloc byte[4];
        _innerStream.ReadExactly(buffer);
        return BinaryPrimitives.ReadSingleLittleEndian(buffer);
    }
    
    public double ReadLittleEndianDouble()
    {
        Span<byte> buffer = stackalloc byte[8];
        _innerStream.ReadExactly(buffer);
        return BinaryPrimitives.ReadDoubleLittleEndian(buffer);
    }
    
    // ========== Big-Endian Write Methods ==========
    public void WriteBigEndian(short value)
    {
        Span<byte> buffer = stackalloc byte[2];
        BinaryPrimitives.WriteInt16BigEndian(buffer, value);
        _innerStream.Write(buffer);
    }
    
    public void WriteBigEndian(ushort value)
    {
        Span<byte> buffer = stackalloc byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(buffer, value);
        _innerStream.Write(buffer);
    }
    
    public void WriteBigEndian(int value)
    {
        Span<byte> buffer = stackalloc byte[4];
        BinaryPrimitives.WriteInt32BigEndian(buffer, value);
        _innerStream.Write(buffer);
    }
    
    public void WriteBigEndian(uint value)
    {
        Span<byte> buffer = stackalloc byte[4];
        BinaryPrimitives.WriteUInt32BigEndian(buffer, value);
        _innerStream.Write(buffer);
    }
    
    public void WriteBigEndian(long value)
    {
        Span<byte> buffer = stackalloc byte[8];
        BinaryPrimitives.WriteInt64BigEndian(buffer, value);
        _innerStream.Write(buffer);
    }
    
    public void WriteBigEndian(ulong value)
    {
        Span<byte> buffer = stackalloc byte[8];
        BinaryPrimitives.WriteUInt64BigEndian(buffer, value);
        _innerStream.Write(buffer);
    }
    
    public void WriteBigEndian(float value)
    {
        Span<byte> buffer = stackalloc byte[4];
        BinaryPrimitives.WriteSingleBigEndian(buffer, value);
        _innerStream.Write(buffer);
    }
    
    public void WriteBigEndian(double value)
    {
        Span<byte> buffer = stackalloc byte[8];
        BinaryPrimitives.WriteDoubleBigEndian(buffer, value);
        _innerStream.Write(buffer);
    }
    
    // ========== Little-Endian Write Methods ==========
    public void WriteLittleEndian(short value)
    {
        Span<byte> buffer = stackalloc byte[2];
        BinaryPrimitives.WriteInt16LittleEndian(buffer, value);
        _innerStream.Write(buffer);
    }
    
    public void WriteLittleEndian(ushort value)
    {
        Span<byte> buffer = stackalloc byte[2];
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, value);
        _innerStream.Write(buffer);
    }
    
    public void WriteLittleEndian(int value)
    {
        Span<byte> buffer = stackalloc byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(buffer, value);
        _innerStream.Write(buffer);
    }
    
    public void WriteLittleEndian(uint value)
    {
        Span<byte> buffer = stackalloc byte[4];
        BinaryPrimitives.WriteUInt32LittleEndian(buffer, value);
        _innerStream.Write(buffer);
    }
    
    public void WriteLittleEndian(long value)
    {
        Span<byte> buffer = stackalloc byte[8];
        BinaryPrimitives.WriteInt64LittleEndian(buffer, value);
        _innerStream.Write(buffer);
    }
    
    public void WriteLittleEndian(ulong value)
    {
        Span<byte> buffer = stackalloc byte[8];
        BinaryPrimitives.WriteUInt64LittleEndian(buffer, value);
        _innerStream.Write(buffer);
    }
    
    public void WriteLittleEndian(float value)
    {
        Span<byte> buffer = stackalloc byte[4];
        BinaryPrimitives.WriteSingleLittleEndian(buffer, value);
        _innerStream.Write(buffer);
    }
    
    public void WriteLittleEndian(double value)
    {
        Span<byte> buffer = stackalloc byte[8];
        BinaryPrimitives.WriteDoubleLittleEndian(buffer, value);
        _innerStream.Write(buffer);
    }
    
    // ========== String Read/Write ==========
    public string ReadString(int length)
    {
        Span<byte> buffer = stackalloc byte[length];
        _innerStream.ReadExactly(buffer);
        return System.Text.Encoding.UTF8.GetString(buffer);
    }
    
    public void WriteString(string value, int? fixedLength = null)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value);
        if (fixedLength.HasValue)
        {
            if (bytes.Length > fixedLength.Value)
                Array.Resize(ref bytes, fixedLength.Value);
            else if (bytes.Length < fixedLength.Value)
                Array.Resize(ref bytes, fixedLength.Value);
        }
        _innerStream.Write(bytes);
    }
    
    // ========== VarInt Read/Write ==========
    public ulong ReadVarInt()
    {
        ulong result = 0;
        int shift = 0;
        while (true)
        {
            int b = _innerStream.ReadByte();
            if (b == -1) throw new EndOfStreamException();
            result |= (ulong)(b & 0x7F) << shift;
            if ((b & 0x80) == 0)
                break;
            shift += 7;
        }
        return result;
    }
    
    public void WriteVarInt(ulong value)
    {
        while (value >= 0x80)
        {
            _innerStream.WriteByte((byte)(value | 0x80));
            value >>= 7;
        }
        _innerStream.WriteByte((byte)value);
    }
}

/// <summary>
/// Permissioned Stream factory: provides secure Stream creation capabilities for plugins.
/// All Stream operations involving file I/O must go through this factory, obtaining a real Stream
/// only after permission checks pass.
/// <para>Pure in-memory operations (MemoryStream, compression streams, etc.) do not require permission checks;
/// plugins can use those types directly.</para>
/// </summary>
public static class PermissionedStreamFactory
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger(typeof(PermissionedStreamFactory));

    /// <summary>
    /// Creates a permission-checked file read stream.
    /// </summary>
    /// <param name="callerId">Caller silicon being ID</param>
    /// <param name="path">File path</param>
    /// <returns>PermissionedStream if permission check passes, otherwise null</returns>
    public static PermissionedStream? CreateReadStream(Guid callerId, string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            _logger.Debug(callerId, "Stream creation rejected: path is empty");
            return null;
        }

        path = Path.GetFullPath(path);

        if (!CheckPermission(callerId, path))
        {
            _logger.Debug(callerId, "Stream creation denied: no file access permission for '{0}'", path);
            return null;
        }

        try
        {
            if (!File.Exists(path))
            {
                _logger.Debug(callerId, "Stream creation failed: file not found '{0}'", path);
                return null;
            }

            FileStream stream = new(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            PermissionedStream permStream = new(stream);
            _logger.Info(callerId, "Stream created (read): {0}", path);
            return permStream;
        }
        catch (Exception ex)
        {
            _logger.Error(callerId, "Stream creation failed: {0}, {1}", ex, path);
            return null;
        }
    }

    /// <summary>
    /// Creates a permission-checked file write stream.
    /// </summary>
    /// <param name="callerId">Caller silicon being ID</param>
    /// <param name="path">File path</param>
    /// <param name="append">Whether to use append mode (true=append, false=overwrite)</param>
    /// <returns>PermissionedStream if permission check passes, otherwise null</returns>
    public static PermissionedStream? CreateWriteStream(Guid callerId, string path, bool append = false)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            _logger.Debug(callerId, "Stream creation rejected: path is empty");
            return null;
        }

        path = Path.GetFullPath(path);

        if (!CheckPermission(callerId, path))
        {
            _logger.Debug(callerId, "Stream creation denied: no file access permission for '{0}'", path);
            return null;
        }

        try
        {
            string? directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            FileMode mode = append ? FileMode.Append : FileMode.Create;
            FileStream stream = new(path, mode, FileAccess.Write, FileShare.None);
            PermissionedStream permStream = new(stream);
            _logger.Info(callerId, "Stream created (write, append={0}): {1}", append, path);
            return permStream;
        }
        catch (Exception ex)
        {
            _logger.Error(callerId, "Stream creation failed: {0}, {1}", ex, path);
            return null;
        }
    }

    /// <summary>
    /// Creates a permission-checked file read-write stream.
    /// </summary>
    /// <param name="callerId">Caller silicon being ID</param>
    /// <param name="path">File path</param>
    /// <returns>PermissionedStream if permission check passes, otherwise null</returns>
    public static PermissionedStream? CreateReadWriteStream(Guid callerId, string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            _logger.Debug(callerId, "Stream creation rejected: path is empty");
            return null;
        }

        path = Path.GetFullPath(path);

        if (!CheckPermission(callerId, path))
        {
            _logger.Debug(callerId, "Stream creation denied: no file access permission for '{0}'", path);
            return null;
        }

        try
        {
            if (!File.Exists(path))
            {
                _logger.Debug(callerId, "Stream creation failed: file not found '{0}'", path);
                return null;
            }

            FileStream stream = new(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            PermissionedStream permStream = new(stream);
            _logger.Info(callerId, "Stream created (read-write): {0}", path);
            return permStream;
        }
        catch (Exception ex)
        {
            _logger.Error(callerId, "Stream creation failed: {0}, {1}", ex, path);
            return null;
        }
    }

    /// <summary>
    /// Checks the caller's file access permission for the specified path.
    /// </summary>
    private static bool CheckPermission(Guid callerId, string path)
    {
        PermissionManager? pm = ServiceLocator.Instance.GetPermissionManager(callerId);
        if (pm == null)
        {
            // No permission manager = bootstrap phase or unrestricted, allow access
            return true;
        }

        return pm.CheckPermission(callerId, PermissionType.FileAccess, path);
    }
}

/// <summary>
/// Safe path operations class: fully emulates System.IO.Path but with integrated permission checks.
/// All path operations go through permission verification, ensuring plugins can only access authorized paths.
/// </summary>
public static class SafePath
{
    private static readonly ILogger _logger = LogManager.Instance.GetLogger(typeof(SafePath));

    /// <summary>
    /// Gets the permission manager for the specified callerId (if it exists).
    /// </summary>
    private static PermissionManager? GetPermissionManager(Guid callerId)
    {
        return ServiceLocator.Instance.GetPermissionManager(callerId);
    }

    /// <summary>
    /// Checks the caller's access permission for the specified path.
    /// </summary>
    private static bool CheckPermission(Guid callerId, string path)
    {
        PermissionManager? pm = GetPermissionManager(callerId);
        if (pm == null)
        {
            // No permission manager = bootstrap phase or unrestricted, allow access
            return true;
        }

        return pm.CheckPermission(callerId, PermissionType.FileAccess, path);
    }

    /// <summary>
    /// Gets the absolute path (with permission check - resolves relative paths).
    /// </summary>
    public static string? GetFullPath(Guid callerId, string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        string fullPath = Path.GetFullPath(path);

        if (!CheckPermission(callerId, fullPath))
        {
            _logger.Warn(callerId, "SafePath.GetFullPath denied: no permission for '{0}'", fullPath);
            return null;
        }

        return fullPath;
    }

    /// <summary>
    /// Gets the directory name of the path (pure string operation, no permission check).
    /// </summary>
    public static string? GetDirectoryName(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        return Path.GetDirectoryName(path);
    }

    /// <summary>
    /// Gets the file name of the path (pure string operation, no permission check).
    /// </summary>
    public static string? GetFileName(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        return Path.GetFileName(path);
    }

    /// <summary>
    /// Gets the file name without extension (pure string operation, no permission check).
    /// </summary>
    public static string? GetFileNameWithoutExtension(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        return Path.GetFileNameWithoutExtension(path);
    }

    /// <summary>
    /// Gets the extension of the path (pure string operation, no permission check).
    /// </summary>
    public static string? GetExtension(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        return Path.GetExtension(path);
    }

    /// <summary>
    /// Gets the root directory of the path (pure string operation, no permission check).
    /// </summary>
    public static string? GetPathRoot(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        return Path.GetPathRoot(path);
    }

    /// <summary>
    /// Gets a random file name (pure string operation, no permission check).
    /// </summary>
    public static string GetRandomFileName()
    {
        return Path.GetRandomFileName();
    }

    /// <summary>
    /// Gets a temporary file name (pure string operation, no permission check).
    /// </summary>
    public static string GetTempFileName()
    {
        return Path.GetTempFileName();
    }

    /// <summary>
    /// Gets the temporary folder path (with permission check - accesses system temp directory).
    /// </summary>
    public static string? GetTempPath(Guid callerId)
    {
        string tempPath = Path.GetTempPath();

        if (!CheckPermission(callerId, tempPath))
        {
            _logger.Warn(callerId, "SafePath.GetTempPath denied: no permission for '{0}'", tempPath);
            return null;
        }

        return tempPath;
    }

    /// <summary>
    /// Combines two paths (pure string operation, no permission check).
    /// </summary>
    public static string? Combine(string path1, string path2)
    {
        if (string.IsNullOrWhiteSpace(path1) || string.IsNullOrWhiteSpace(path2))
            return null;

        return Path.Combine(path1, path2);
    }

    /// <summary>
    /// Combines multiple paths (pure string operation, no permission check).
    /// </summary>
    public static string? Combine(params string[] paths)
    {
        if (paths == null || paths.Length == 0)
            return null;

        return Path.Combine(paths);
    }

    /// <summary>
    /// Checks if the path has an extension (pure string operation, no permission check).
    /// </summary>
    public static bool HasExtension(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;

        return Path.HasExtension(path);
    }

    /// <summary>
    /// Checks if the path is an absolute path (pure string operation, no permission check).
    /// </summary>
    public static bool IsPathRooted(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;

        return Path.IsPathRooted(path);
    }

    /// <summary>
    /// Changes the extension of the path (pure string operation, no permission check).
    /// </summary>
    public static string? ChangeExtension(string path, string? extension)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        return Path.ChangeExtension(path, extension);
    }

    /// <summary>
    /// Gets the volume label of the path (with permission check - accesses disk information).
    /// </summary>
    public static string? GetVolumeLabel(Guid callerId, string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        string fullPath = Path.GetFullPath(path);

        if (!CheckPermission(callerId, fullPath))
        {
            _logger.Warn(callerId, "SafePath.GetVolumeLabel denied: no permission for '{0}'", fullPath);
            return null;
        }

        try
        {
            DriveInfo drive = new(fullPath);
            return drive.IsReady ? drive.VolumeLabel : null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Gets the relative path (with permission check - resolves path relationships).
    /// </summary>
    public static string? GetRelativePath(Guid callerId, string relativeTo, string path)
    {
        if (string.IsNullOrWhiteSpace(relativeTo) || string.IsNullOrWhiteSpace(path))
            return null;

        string fullRelativeTo = Path.GetFullPath(relativeTo);
        string fullPath = Path.GetFullPath(path);

        if (!CheckPermission(callerId, fullRelativeTo) || !CheckPermission(callerId, fullPath))
        {
            _logger.Warn(callerId, "SafePath.GetRelativePath denied: no permission for '{0}' or '{1}'", fullRelativeTo,
                fullPath);
            return null;
        }

        return Path.GetRelativePath(fullRelativeTo, fullPath);
    }

    /// <summary>
    /// Gets the alternate data stream name (pure string operation, no permission check).
    /// </summary>
    public static string? GetAlternateDataStreamName(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        // Windows specific feature
        // For cross-platform compatibility, we'll just return null
        return null;
    }

    /// <summary>
    /// Gets the directory info (with permission check - creates DirectoryInfo object).
    /// </summary>
    public static DirectoryInfo? GetDirectoryInfo(Guid callerId, string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        string fullPath = Path.GetFullPath(path);

        if (!CheckPermission(callerId, fullPath))
        {
            _logger.Warn(callerId, "SafePath.GetDirectoryInfo denied: no permission for '{0}'", fullPath);
            return null;
        }

        return new DirectoryInfo(fullPath);
    }

    /// <summary>
    /// Gets the file info (with permission check - creates FileInfo object).
    /// </summary>
    public static FileInfo? GetFileInfo(Guid callerId, string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        string fullPath = Path.GetFullPath(path);

        if (!CheckPermission(callerId, fullPath))
        {
            _logger.Warn(callerId, "SafePath.GetFileInfo denied: no permission for '{0}'", fullPath);
            return null;
        }

        return new FileInfo(fullPath);
    }
}