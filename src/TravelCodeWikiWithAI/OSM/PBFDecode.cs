﻿using System.Collections.Concurrent;
using OSMPBF;
using System.IO.Compression;
using Google.Protobuf;

namespace TravelCodeWikiWithAI.Data.OSM;

    // fileformat.proto, osmformat.proto from https://github.com/openstreetmap/OSM-binary/tree/master/osmpbf
    public class PbfFileDataSource
    {
        public PbfFileDataSource(string path)
        {
            throw new NotImplementedException();
        }

        public PbfFileDataSource(Stream stream)
        {
            source = stream;
        }

        /// <summary>
        /// 解析过程中使用的并行度（工作线程数）。默认为 CPU 逻辑核心数。
        /// </summary>
        public int ParseParallelism { get; set; } = Environment.ProcessorCount;

        public void Parse()
        {
            SeekToStart();
            _data = new OSMData();
            long length = GetStreamLength();

            // 待解析块队列：I/O 线程放入原始数据，工作线程取出解析
            using BlockingCollection<PendingBlock> pendingBlocks = new BlockingCollection<PendingBlock>(ParseParallelism * 2);

            // 结果列表：需要按 Position 排序
            List<FileDataPosition> results = new List<FileDataPosition>();

            // I/O 线程：顺序读取 BlobHeader + Blob 原始字节，放入队列
            Task ioTask = Task.Run(() =>
            {
                try
                {
                    while (!IsFileEnd)
                    {
                        long position = GetStreamPosition();
                        int sz = GetIntLe();
                        sz = BinaryPrimitivesReverseEndianness(sz);
                        if (sz > max_blob_header_size)
                        {
                            throw new OutOfMemoryException();
                        }

                        byte[] buffer = GetBuffer(sz);
                        BlobHeader bh = new BlobHeader();
                        bh.MergeFrom(buffer);
                        byte[] outbuffer = Array.Empty<byte>();
                        Blob blobsz = ReadBlob(bh, ref outbuffer);

                        switch (bh.Type)
                        {
                            case "OSMHeader":
                                // OSMHeader 需要先于数据处理，直接在此解析
                                ParseHeaderBlock(outbuffer);
                                break;
                            case "OSMData":
                                pendingBlocks.Add(new PendingBlock
                                {
                                    Position = position,
                                    Header = bh,
                                    Data = outbuffer
                                });
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                }
                finally
                {
                    pendingBlocks.CompleteAdding();
                }
            });

            // 工作线程：并行从队列取出数据块并解析
            int workerCount = ParseParallelism;
            Task[] workers = new Task[workerCount];
            for (int i = 0; i < workerCount; i++)
            {
                workers[i] = Task.Run(() =>
                {
                    foreach (PendingBlock block in pendingBlocks.GetConsumingEnumerable())
                    {
                        OSMData data = ParsePrimitiveBlock(block.Data);

                        long minNodeId = -1, maxNodeId = -1;
                        for (int ni = 0; ni < data.Nodes.Count; ni++)
                        {
                            long nid = data.Nodes[ni].Id;
                            if (ni == 0) { minNodeId = nid; maxNodeId = nid; }
                            else if (nid < minNodeId) minNodeId = nid;
                            else if (nid > maxNodeId) maxNodeId = nid;
                        }

                        long minWayId = -1, maxWayId = -1;
                        for (int wi = 0; wi < data.Ways.Count; wi++)
                        {
                            long wid = data.Ways[wi].Id;
                            if (wi == 0) { minWayId = wid; maxWayId = wid; }
                            else if (wid < minWayId) minWayId = wid;
                            else if (wid > maxWayId) maxWayId = wid;
                        }

                        long minRelId = -1, maxRelId = -1;
                        for (int ri = 0; ri < data.Relations.Count; ri++)
                        {
                            long rid = data.Relations[ri].Id;
                            if (ri == 0) { minRelId = rid; maxRelId = rid; }
                            else if (rid < minRelId) minRelId = rid;
                            else if (rid > maxRelId) maxRelId = rid;
                        }

                        FileDataPosition fdp = new FileDataPosition
                        {
                            Position = block.Position,
                            info = block.Header,
                            NodeCount = data.Nodes.Count,
                            WayCount = data.Ways.Count,
                            RelationCount = data.Relations.Count,
                            MinNodeId = minNodeId,
                            MaxNodeId = maxNodeId,
                            MinWayId = minWayId,
                            MaxWayId = maxWayId,
                            MinRelationId = minRelId,
                            MaxRelationId = maxRelId,
                        };

                        lock (results)
                        {
                            results.Add(fdp);
                        }
                    }
                });
            }

            // 等待 I/O 线程和所有工作线程完成
            ioTask.Wait();
            Task.WaitAll(workers);

            // 按 Position 排序，保证 _fileDataPositions 的顺序与文件中一致
            results.Sort((a, b) => a.Position.CompareTo(b.Position));
            lock (_fileDataPositions)
            {
                _fileDataPositions = results;
            }
        }

        /// <summary>
        /// 待解析块：I/O 线程读取的原始数据
        /// </summary>
        private struct PendingBlock
        {
            public long Position;
            public BlobHeader Header;
            public byte[] Data;
        }

        private byte[] GetBuffer(int sz)
        {
            if (sz <= 0)
            {
                return Array.Empty<byte>();
            }

            byte[] buffer = new byte[sz];
            source.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        private int GetIntLe()
        {
            byte[] bin = new byte[4];
            source.Read(bin, 0, bin.Length);
            int result = BitConverter.ToInt32(bin, 0);
            return result;
        }

        private long GetStreamPosition()
        {
            if (source == null)
            {
                return -1;
            }

            return source.Position;
        }

        public bool IsFileEnd
        {
            get
            {
                if (source == null)
                {
                    return true;
                }

                return source.Position >= source.Length;
            }
        }

        private long GetStreamLength()
        {
            if (source == null)
            {
                return 0;
            }

            return source.Length;
        }

        private void SeekToStart()
        {
            source.Seek(0, SeekOrigin.Begin);
        }

        protected const int max_blob_header_size = 64 * 1024;

        protected const int max_uncompressed_blob_size = 32 * 1024 * 1024;

        protected const int lonlat_resolution = 1000 * 1000 * 1000;

        private Blob ReadBlob(BlobHeader header, ref byte[] outBuffer)
        {
            Blob blob = new Blob();
            int sz = header.Datasize;
            if (sz > max_uncompressed_blob_size)
            {
                throw new OutOfMemoryException();
            }

            byte[] buffer = GetBuffer(sz);
            blob.MergeFrom(buffer);
            if (blob.HasRaw)
            {
                outBuffer = blob.Raw.ToByteArray();
                return blob;
            }

            if (blob.HasZlibData)
            {
                ZLibStream zlib = new ZLibStream(new MemoryStream(blob.ZlibData.ToByteArray()),
                    CompressionMode.Decompress, true);
                byte[] zdbin = new byte[blob.RawSize];
                int p = 0;
                while (p < blob.RawSize)
                {
                    int r = zlib.Read(zdbin, p, zdbin.Length - p);
                    p += r;
                    if (r == 0)
                    {
                        byte[] lessBin = new byte[p];
                        Array.Copy(zdbin, lessBin, p);
                        outBuffer = lessBin;
                        return blob;
                    }
                }

                outBuffer = zdbin;
                return blob;
            }

            if (blob.HasLzmaData)
            {
                throw new NotImplementedException();
            }

            throw new NotImplementedException();
        }

        private OSMData ParsePrimitiveBlock(byte[] bin)
        {
            OSMData currentResult = new OSMData();
            PrimitiveBlock pb = new PrimitiveBlock();
            pb.MergeFrom(bin);
            for (int i = 0, l = pb.Primitivegroup.Count; i < l; i++)
            {
                PrimitiveGroup pg = pb.Primitivegroup[i];
                for (int n = 0; n < pg.Nodes.Count; n++)
                {
                    throw new NotImplementedException();
                }

                long id = 0;
                double lon = 0;
                double lat = 0;
                int currentKv = 0;
                long time = 0;
                int uid = -1;
                int user_sid = 0;
                long changeSet = -1;
                long detalLon = 0;
                DenseNodes dn = pg.Dense;
                if (pb.LonOffset != 0 || pb.LatOffset != 0)
                {
                    throw new NotImplementedException();
                }

                if (pg.Changesets.Count != 0)
                {
                    throw new NotImplementedException();
                }

                if (pg.Nodes.Count != 0)
                {
                    throw new NotImplementedException();
                }

                if (dn != null)
                {
                    for (int did = 0; did < dn.Id.Count; ++did)
                    {
                        id += dn.Id[did];
                        lon += 0.000000001 * (pb.LonOffset + (pb.Granularity * dn.Lon[did]));
                        lat += 0.000000001 * (pb.LatOffset + (pb.Granularity * dn.Lat[did]));
                        Dictionary<string, string> tags = new Dictionary<string, string>();
                        while (currentKv < dn.KeysVals.Count && dn.KeysVals[currentKv] != 0)
                        {
                            int key = dn.KeysVals[currentKv];
                            int val = dn.KeysVals[currentKv + 1];
                            ByteString keys = pb.Stringtable.S[key];
                            ByteString vals = pb.Stringtable.S[val];
                            currentKv += 2;
                            string keyss = keys.ToStringUtf8();
                            string valss = vals.ToStringUtf8();
                            tags.Add(keyss, valss);
                        }

                        currentKv++;
                        bool v = true;
                        if (dn.Denseinfo.Visible.Count >= did + 1)
                        {
                            v = dn.Denseinfo.Visible[did];
                        }

                        int version = dn.Denseinfo.Version[did];
                        if (dn.Denseinfo.Timestamp.Count >= did + 1)
                        {
                            time += dn.Denseinfo.Timestamp[did];
                        }

                        if (dn.Denseinfo.Uid.Count >= did + 1)
                        {
                            uid += dn.Denseinfo.Uid[did];
                        }

                        if (dn.Denseinfo.UserSid.Count >= did + 1)
                        {
                            user_sid += dn.Denseinfo.UserSid[did];
                        }

                        if (dn.Denseinfo.Changeset.Count >= did + 1)
                        {
                            changeSet += dn.Denseinfo.Changeset[did];
                        }

                        ByteString UserS = pb.Stringtable.S[user_sid];
                        currentResult.Nodes.Add(new OSMNode
                        {
                            Id = id,
                            LngLat = new Vector2DD
                            {
                                Y = lat,
                                X = lon
                            },
                            Tags = tags,
                            Time = OSMDataTools.ConvToTime(time),
                            ChangeSet = changeSet,
                            Uid = uid,
                            User = UserS.ToStringUtf8(),
                            Version = version,
                            Visible = v
                        });
                    }
                }

                for (int w = 0; w < pg.Ways.Count; ++w)
                {
                    Way way = pg.Ways[w];
                    long @ref = 0;
                    List<long> refs = new List<long>();
                    for (int rid = 0; rid < way.Refs.Count; ++rid)
                    {
                        @ref += way.Refs[rid];
                        refs.Add(@ref);
                    }

                    long wid = way.Id;
                    Dictionary<string, string> tags = new Dictionary<string, string>();
                    for (int j = 0; j < way.Keys.Count; j++)
                    {
                        long key = way.Keys[j];
                        long val = way.Vals[j];
                        ByteString keys = pb.Stringtable.S[(int)key];
                        ByteString vals = pb.Stringtable.S[(int)val];
                        string keyss = keys.ToStringUtf8();
                        string valss = vals.ToStringUtf8();
                        tags.Add(keyss, valss);
                    }

                    long wchangeSet = 0;
                    if (way.Info.HasChangeset)
                    {
                        wchangeSet = way.Info.Changeset;
                    }

                    DateTime dt = new DateTime(0);
                    if (way.Info.HasTimestamp)
                    {
                        dt = OSMDataTools.ConvToTime(way.Info.Timestamp);
                    }

                    int wv = -1;
                    if (way.Info.HasVersion)
                    {
                        wv = way.Info.Version;
                    }

                    int wuid = 0;
                    if (way.Info.HasUid)
                    {
                        wuid = way.Info.Uid;
                    }

                    string wuser = "";
                    if (way.Info.HasUserSid)
                    {
                        ByteString bwuser = pb.Stringtable.S[(int)way.Info.UserSid];
                        wuser = bwuser.ToStringUtf8();
                    }

                    bool visible = true;
                    if (way.Info.HasVisible)
                    {
                        visible = way.Info.Visible;
                    }

                    currentResult.Ways.Add(new OSMWay
                    {
                        Id = wid,
                        Refs = refs.ToArray(),
                        Tags = tags,
                        ChangeSet = wchangeSet,
                        Time  = dt,
                        Version = wv,
                        Uid = wuid,
                        User = wuser,
                        Visible = visible
                    });
                }

                for (int r = 0; r < pg.Relations.Count; ++r)
                {
                    Relation rel = pg.Relations[r];
                    long rid = 0;
                    List<OSMRelationRef> refs = new List<OSMRelationRef>();
                    for (int rm = 0; rm < rel.Memids.Count; ++rm)
                    {
                        rid += rel.Memids[rm];
                        Relation.Types.MemberType type = rel.Types_[rm];
                        OSMRelationRefType relType = OSMRelationRefType.Node;
                        switch (type)
                        {
                            case Relation.Types.MemberType.Way:
                                relType = OSMRelationRefType.Way;
                                break;
                            case Relation.Types.MemberType.Relation:
                                relType = OSMRelationRefType.Relations;
                                break;
                            case Relation.Types.MemberType.Node:
                                relType = OSMRelationRefType.Node;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("Relation Type", type.ToString());
                        }

                        ByteString role = pb.Stringtable.S[rel.RolesSid[rm]];
                        refs.Add(new OSMRelationRef
                        {
                            Id = rid,
                            Type = relType,
                            Role = role.ToStringUtf8()
                        });
                    }

                    Dictionary<string, string> tags = new Dictionary<string, string>();
                    for (int j = 0; j < rel.Keys.Count; j++)
                    {
                        long key = rel.Keys[j];
                        long val = rel.Vals[j];
                        ByteString keys = pb.Stringtable.S[(int)key];
                        ByteString vals = pb.Stringtable.S[(int)val];
                        string keyss = keys.ToStringUtf8();
                        string valss = vals.ToStringUtf8();
                        tags.Add(keyss, valss);
                    }

                    int rv = -1;
                    if (rel.Info.HasVersion)
                    {
                        rv = rel.Info.Version;
                    }

                    int ruid = 0;
                    if (rel.Info.HasUid)
                    {
                        ruid = rel.Info.Uid;
                    }

                    string ruser = "";
                    if (rel.Info.HasUserSid)
                    {
                        ByteString bwuser = pb.Stringtable.S[(int)rel.Info.UserSid];
                        ruser = bwuser.ToStringUtf8();
                    }

                    DateTime dt = new DateTime(0);
                    if (rel.Info.HasTimestamp)
                    {
                        dt = OSMDataTools.ConvToTime(rel.Info.Timestamp);
                    }

                    long rchangeSet = -1;
                    if (rel.Info.HasChangeset)
                    {
                        rchangeSet = rel.Info.Changeset;
                    }

                    bool rVisible = true;
                    if (rel.Info.HasVisible)
                    {
                        rVisible = rel.Info.Visible;
                    }

                    currentResult.Relations.Add(new OSMRelations
                    {
                        Id = rel.Id,
                        Tags = tags,
                        Version = rv,
                        Time = dt,
                        ChangeSet = rchangeSet,
                        Uid = ruid,
                        User = ruser,
                        Visible = rVisible,
                        Refs = refs
                    });
                }
            }

            return currentResult;
        }

        private void ParseHeaderBlock(byte[] bin)
        {
            HeaderBlock block = new HeaderBlock();
            block.MergeFrom(bin);
            const double multiplier = 0.000000001;
            _data.Bbox = new BoxD
            {
                LU = new Vector2DD
                {
                    X = block.Bbox.Left * multiplier,
                    Y = block.Bbox.Top * multiplier
                },
                RD = new Vector2DD
                {
                    X = block.Bbox.Right * multiplier,
                    Y = block.Bbox.Bottom * multiplier
                }
            };
            foreach (string rf in block.RequiredFeatures)
            {
                switch (rf)
                {
                    case "OsmSchema-V0.6":
                    case "DenseNodes":
                        break;
                    case "HistoricalInformation":
                        _data.HistoricalInformation = true;
                        break;
                    default:
                        throw new NotSupportedException("File requires unknown feature: " + rf);
                }
            }

            if (block.HasOsmosisReplicationBaseUrl)
            {
                _data.BaseURL = block.OsmosisReplicationBaseUrl;
            }

            foreach (string of in block.OptionalFeatures)
            {
                switch (of)
                {
                    case "Sort.Type_then_ID":
                        _data.SortById = true;
                        break;
                    case "Has_Metadata":
                        _data.HasMetadata = true;
                        break;
                    default:
                        if (of.Contains('='))
                        {
                            string[] ofs = of.Split('=');
                            if (ofs.Length == 2)
                            {
                                switch (ofs[0])
                                {
                                    case "timestamp":
                                        DateTime dtemp = DateTime.Parse(ofs[1]);
                                        _data.MetaTime = dtemp;
                                        break;
                                    default:
                                        throw new NotImplementedException();
                                }
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }

                        break;
                }
            }

            if (block.HasOsmosisReplicationTimestamp)
            {
                _data.SaveTime = OSMDataTools.ConvToTime(block.OsmosisReplicationTimestamp);
            }

            if (block.HasOsmosisReplicationSequenceNumber)
            {
                _data.SequenceNumber = block.OsmosisReplicationSequenceNumber;
            }

            if (block.HasSource)
            {
                _data.source = block.Source;
            }
            else
            {
                _data.source = GetSource();
            }
        }

        private string GetSource()
        {
            // 返回 PBF 文件路径，如果 OSMapi.PbfFilePath 已设置则使用它
            return "unknown";
        }

        /// <summary>
        /// 根据ID查询OSM元素的详细信息。
        /// 利用 FileDataPosition 中记录的 ID 范围进行二分查找定位数据块，
        /// 再从磁盘读取并解析该块，精确匹配目标元素。
        /// 同一ID、同一类型可能存在多个版本，因此返回数组。
        /// </summary>
        /// <typeparam name="T">OSM元素类型，必须继承自 OSMBaseData（支持 OSMNode、OSMWay、OSMRelations）</typeparam>
        /// <param name="id">要查询的元素ID</param>
        /// <returns>匹配的OSM元素数组，未找到则返回空数组</returns>
        public List<T> GetDataById<T>(long id) where T : OSMBaseData
        {
            List<T> results = new List<T>();

            List<FileDataPosition> positions;
            lock (_fileDataPositions)
            {
                positions = _fileDataPositions;
            }

            // 获取该类型对应的 ID 范围访问器
            (long minId, long maxId, int count) GetIdRange(FileDataPosition fdp)
            {
                if (typeof(T) == typeof(OSMNode))
                    return (fdp.MinNodeId, fdp.MaxNodeId, fdp.NodeCount);
                else if (typeof(T) == typeof(OSMWay))
                    return (fdp.MinWayId, fdp.MaxWayId, fdp.WayCount);
                else if (typeof(T) == typeof(OSMRelations))
                    return (fdp.MinRelationId, fdp.MaxRelationId, fdp.RelationCount);
                else
                    throw new NotSupportedException($"不支持的类型: {typeof(T).Name}");
            }

            // 从 OSMData 中收集所有匹配 ID 的元素
            void CollectMatches(OSMData data)
            {
                if (typeof(T) == typeof(OSMNode))
                {
                    foreach (var node in data.Nodes)
                    {
                        if (node.Id == id)
                            results.Add((T)(object)node);
                    }
                }
                else if (typeof(T) == typeof(OSMWay))
                {
                    foreach (var way in data.Ways)
                    {
                        if (way.Id == id)
                            results.Add((T)(object)way);
                    }
                }
                else if (typeof(T) == typeof(OSMRelations))
                {
                    foreach (var rel in data.Relations)
                    {
                        if (rel.Id == id)
                            results.Add((T)(object)rel);
                    }
                }
            }

            // 二分查找：找到 MinId <= id <= MaxId 的数据块
            int left = 0, right = positions.Count - 1;
            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                var range = GetIdRange(positions[mid]);
                if (range.count == 0)
                {
                    // 该块无此类型数据，向两边搜索
                    // 先尝试向左
                    bool found = false;
                    for (int i = mid - 1; i >= left; i--)
                    {
                        var r = GetIdRange(positions[i]);
                        if (r.count > 0 && r.minId <= id && id <= r.maxId)
                        {
                            mid = i;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        for (int i = mid + 1; i <= right; i++)
                        {
                            var r = GetIdRange(positions[i]);
                            if (r.count > 0 && r.minId <= id && id <= r.maxId)
                            {
                                mid = i;
                                found = true;
                                break;
                            }
                        }
                    }
                    if (!found) break;
                }

                var midRange = GetIdRange(positions[mid]);
                if (id < midRange.minId)
                    right = mid - 1;
                else if (id > midRange.maxId)
                    left = mid + 1;
                else
                {
                    // id 落在此块范围内，解析该块并收集匹配元素
                    OSMData data = GetDataFormBlockObject(positions[mid]);
                    CollectMatches(data);

                    // 向左右扩展搜索相邻块（ID范围可能重叠，同一ID可能存在于多个块中）
                    for (int i = mid - 1; i >= 0; i--)
                    {
                        var r = GetIdRange(positions[i]);
                        if (r.count > 0 && r.minId <= id && id <= r.maxId)
                        {
                            var expandData = GetDataFormBlockObject(positions[i]);
                            CollectMatches(expandData);
                        }
                        // 如果当前块的最大ID已经小于目标ID，不需要再往左找
                        if (r.maxId < id) break;
                    }
                    for (int i = mid + 1; i < positions.Count; i++)
                    {
                        var r = GetIdRange(positions[i]);
                        if (r.count > 0 && r.minId <= id && id <= r.maxId)
                        {
                            var expandData = GetDataFormBlockObject(positions[i]);
                            CollectMatches(expandData);
                        }
                        // 如果当前块的最小ID已经大于目标ID，不需要再往右找
                        if (r.minId > id) break;
                    }

                    return results;
                }
            }

            return results;
        }

        public object[] GetDataBlockObject<T>()
        {
            lock (this)
            {
                List<object> result = new List<object>();
                if (typeof(T) == typeof(OSMRelations))
                {
                    foreach (FileDataPosition rfdp in _fileDataPositions)
                    {
                        if (rfdp.RelationCount != 0)
                        {
                            result.Add(rfdp);
                        }
                    }
                }
                else if (typeof(T) == typeof(OSMWay))
                {
                    foreach (FileDataPosition rfdp in _fileDataPositions)
                    {
                        if (rfdp.WayCount != 0)
                        {
                            result.Add(rfdp);
                        }
                    }
                }
                else if (typeof(T) == typeof(OSMNode))
                {
                    foreach (FileDataPosition rfdp in _fileDataPositions)
                    {
                        if (rfdp.NodeCount != 0)
                        {
                            result.Add(rfdp);
                        }
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }

                return result.ToArray();
            }
        }

        public OSMData GetDataFormBlockObject(object obj)
        {
            lock (this)
            {
                if (obj is FileDataPosition fdp)
                {
                    long tp = GetStreamPosition();
                    Seek(fdp.Position);
                    int sz = GetIntLe();
                    sz = BinaryPrimitivesReverseEndianness(sz);
                    if (sz > max_blob_header_size)
                    {
                        throw new OutOfMemoryException();
                    }

                    byte[] buffer = GetBuffer(sz);
                    BlobHeader bh = new BlobHeader();
                    bh.MergeFrom(buffer);
                    byte[] outbuffer = Array.Empty<byte>();
                    Blob blobsz = ReadBlob(bh, ref outbuffer);
                    OSMData data = ParsePrimitiveBlock(outbuffer);
                    return data;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void Seek(long fdpPosition)
        {
            source.Seek(fdpPosition, SeekOrigin.Begin);
        }

        public int GetDataBlockSize<T>(object obj)
        {
            if (obj is FileDataPosition fdp)
            {
                if (typeof(T) == typeof(OSMNode))
                {
                    return fdp.NodeCount;
                }
                else if (typeof(T) == typeof(OSMRelations))
                {
                    return fdp.RelationCount;
                }
                else if (typeof(T) == typeof(OSMWay))
                {
                    return fdp.WayCount;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            throw new NotImplementedException();
        }

        private OSMData _data;

        private List<FileDataPosition> _fileDataPositions = new List<FileDataPosition>();

        private Stream source;

        public bool OK;

        /// <summary>
        /// Replaces System.Net.IPAddress.NetworkToHostOrder for little-endian to big-endian conversion.
        /// On little-endian systems (x86/x64/ARM), this reverses the byte order of a 32-bit integer.
        /// This avoids referencing System.Net which is forbidden by the plugin security scanner.
        /// </summary>
        private static int BinaryPrimitivesReverseEndianness(int value)
        {
            // Equivalent to System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value)
            // which is equivalent to IPAddress.NetworkToHostOrder on little-endian systems
            byte lo = (byte)(value & 0xFF);
            byte hi = (byte)((value >> 8) & 0xFF);
            byte hi2 = (byte)((value >> 16) & 0xFF);
            byte hi3 = (byte)((value >> 24) & 0xFF);
            return (lo << 24) | (hi << 16) | (hi2 << 8) | hi3;
        }
    }

    public struct FileDataPosition
    {
        public long Position;

        public object info;

        public int NodeCount, WayCount, RelationCount;

        public long MinNodeId, MaxNodeId, MinWayId, MaxWayId, MinRelationId, MaxRelationId;
    }
