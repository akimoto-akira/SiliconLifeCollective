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

using System;
using System.IO;
using System.IO.Compression;

namespace SiliconLife.Collective;

/// <summary>
/// 宿主托管的只读文件句柄：支持按偏移随机读取，供插件安全地访问 OSM PBF 等大文件。
/// 插件安全模型禁止插件直接使用 System.IO 类型（含 Stream），因此宿主打开文件、
/// 以"句柄 + 偏移读取"的方式暴露给插件；插件只接触 byte[]，不接触流对象。
/// </summary>
public sealed class OsmFileHandle : IDisposable
{
    private readonly FileStream _stream;

    internal OsmFileHandle(string path)
    {
        _stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 8192);
    }

    /// <summary>文件字节长度。</summary>
    public long Length => _stream.Length;

    /// <summary>从指定偏移读取最多 count 字节到 buffer，返回实际读取字节数（EOF 时小于 count）。</summary>
    public int Read(long offset, byte[] buffer, int index, int count)
    {
        _stream.Position = offset;
        return _stream.Read(buffer, index, count);
    }

    /// <summary>从指定偏移读取 count 字节；EOF 时返回实际可用字节。</summary>
    public byte[] ReadBytes(long offset, int count)
    {
        var buffer = new byte[count];
        int n = Read(offset, buffer, 0, count);
        if (n == count)
            return buffer;
        var result = new byte[n];
        Array.Copy(buffer, result, n);
        return result;
    }

    public void Dispose() => _stream.Dispose();
}

/// <summary>
/// 宿主托管的 OSM PBF 文件访问服务。注册于 ServiceLocator，供插件经
/// <c>ServiceLocator.Instance.GetService&lt;OsmFileAccessService&gt;()</c> 获取。
///
/// 能力范围（宿主内部使用 System.IO，不受插件安全扫描约束）：
///   1. 打开 PBF 数据文件只读句柄（随机偏移读取，适合流式逐块解码）；
///   2. 读写/删除 PBF 旁路缓存文件（.spidx 块索引、.namedb 名称数据库）；
///   3. zlib 解压（PBF blob 压缩格式）。
///
/// 注意：本服务是受控通道，当前不做路径白名单校验（信任已加载插件）；
/// 若需收紧，可在宿主侧增加按插件 Id 的路径授权。
/// </summary>
public sealed class OsmFileAccessService
{
    /// <summary>打开一个只读文件句柄（文件不存在时抛 FileNotFoundException）。</summary>
    public OsmFileHandle OpenRead(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        return new OsmFileHandle(path);
    }

    /// <summary>判断文件是否存在。</summary>
    public bool FileExists(string path) => !string.IsNullOrEmpty(path) && File.Exists(path);

    /// <summary>获取文件字节长度；文件不存在时返回 -1。</summary>
    public long GetFileLength(string path)
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path)) return -1;
        return new FileInfo(path).Length;
    }

    /// <summary>按路径读取指定偏移的字节（内部临时打开只读句柄）。</summary>
    public byte[] ReadBytes(string path, long offset, int count)
    {
        using var handle = OpenRead(path);
        return handle.ReadBytes(offset, count);
    }

    /// <summary>删除文件（不存在时静默）。</summary>
    public void DeleteFile(string path)
    {
        if (!string.IsNullOrEmpty(path) && File.Exists(path))
            File.Delete(path);
    }

    /// <summary>整体写入文件（覆盖）；用于保存 .spidx 等旁路缓存。</summary>
    public void WriteAllBytes(string path, byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(path);
        ArgumentNullException.ThrowIfNull(bytes);
        File.WriteAllBytes(path, bytes);
    }

    /// <summary>
    /// zlib 解压：输入为带 2 字节 zlib 头的 deflate 数据（OSM PBF Blob 的 ZlibData 格式）。
    /// </summary>
    public byte[] DecompressZlib(byte[] compressed, int rawSize)
    {
        ArgumentNullException.ThrowIfNull(compressed);
        if (rawSize < 0)
            throw new ArgumentOutOfRangeException(nameof(rawSize));

        using var compressedStream = new MemoryStream(compressed, 2, Math.Max(0, compressed.Length - 2));
        using var deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress);
        var result = new byte[rawSize];
        int totalRead = 0;
        while (totalRead < rawSize)
        {
            int read = deflateStream.Read(result, totalRead, rawSize - totalRead);
            if (read == 0)
                break;
            totalRead += read;
        }
        if (totalRead < rawSize)
            Array.Resize(ref result, totalRead);
        return result;
    }
}
