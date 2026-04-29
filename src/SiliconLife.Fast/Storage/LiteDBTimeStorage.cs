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

using LiteDB;
using SiliconLife.Collective;

namespace SiliconLife.Fast;

/// <summary>
/// LiteDB implementation of ITimeStorage interface
/// </summary>
public class LiteDBTimeStorage : ITimeStorage
{
    private readonly ILiteCollection<TimeRecord> _collection;

    public LiteDBTimeStorage()
    {
        _collection = LiteDBManager.TimeStorageCollection;
    }

    // IStorage interface implementation
    public T? Read<T>(string key)
    {
        var record = _collection.FindOne(x => x.Key == key);
        if (record == null)
        {
            return default;
        }

        return BsonMapper.Global.Deserialize<T>(record.Data);
    }

    public void Write<T>(string key, T data)
    {
        var existing = _collection.FindOne(x => x.Key == key);
        var bsonData = BsonMapper.Global.Serialize(data);

        if (existing != null)
        {
            existing.Data = bsonData.AsDocument;
            existing.DataType = typeof(T).FullName ?? typeof(T).Name;
            _collection.Update(existing);
        }
        else
        {
            var record = new TimeRecord
            {
                Key = key,
                DataType = typeof(T).FullName ?? typeof(T).Name,
                Data = bsonData.AsDocument,
                Timestamp = DateTime.UtcNow
            };
            _collection.Insert(record);
        }
    }

    public bool Exists(string key)
    {
        return _collection.Exists(x => x.Key == key);
    }

    public void Delete(string key)
    {
        _collection.DeleteMany(x => x.Key == key);
    }

    // ITimeStorage interface implementation
    public void Write<T>(string key, IncompleteDate timestamp, T data)
    {
        var (start, _) = timestamp.GetRange();
        var bsonData = BsonMapper.Global.Serialize(data);

        var record = new TimeRecord
        {
            Key = key,
            Timestamp = start,
            DataType = typeof(T).FullName ?? typeof(T).Name,
            Data = bsonData.AsDocument
        };

        _collection.Insert(record);
    }

    public T? Read<T>(string key, IncompleteDate timestamp)
    {
        var (start, _) = timestamp.GetRange();
        var record = _collection.FindOne(x => x.Key == key && x.Timestamp == start);
        
        if (record == null)
        {
            return default;
        }

        return BsonMapper.Global.Deserialize<T>(record.Data);
    }

    public bool Exists(string key, IncompleteDate timestamp)
    {
        var (start, _) = timestamp.GetRange();
        return _collection.Exists(x => x.Key == key && x.Timestamp == start);
    }

    public void Delete(string key, IncompleteDate timestamp)
    {
        var (start, _) = timestamp.GetRange();
        _collection.DeleteMany(x => x.Key == key && x.Timestamp == start);
    }

    public List<TimeEntry<T>> Query<T>(string key, IncompleteDate? range)
    {
        var query = _collection.Query();
        
        // Filter by key prefix
        query = query.Where(x => x.Key.StartsWith(key));
        
        // Filter by time range if provided
        if (range.HasValue)
        {
            var (start, end) = range.Value.GetRange();
            query = query.Where(x => x.Timestamp >= start && x.Timestamp <= end);
        }
        
        var records = query.OrderBy(x => x.Timestamp).ToList();
        
        return records.Select(r => new TimeEntry<T>(
            r.Key,
            new IncompleteDate(r.Timestamp.Year, r.Timestamp.Month, r.Timestamp.Day, 
                             r.Timestamp.Hour, r.Timestamp.Minute, r.Timestamp.Second),
            BsonMapper.Global.Deserialize<T>(r.Data)
        )).ToList();
    }

    public List<TimeEntry<T>> Query<T>(IncompleteDate? range)
    {
        var query = _collection.Query();
        
        if (range.HasValue)
        {
            var (start, end) = range.Value.GetRange();
            query = query.Where(x => x.Timestamp >= start && x.Timestamp <= end);
        }
        
        var records = query.OrderBy(x => x.Timestamp).ToList();
        
        return records.Select(r => new TimeEntry<T>(
            r.Key,
            new IncompleteDate(r.Timestamp.Year, r.Timestamp.Month, r.Timestamp.Day, 
                             r.Timestamp.Hour, r.Timestamp.Minute, r.Timestamp.Second),
            BsonMapper.Global.Deserialize<T>(r.Data)
        )).ToList();
    }

    public int Count(string key, IncompleteDate range)
    {
        var (start, end) = range.GetRange();
        return _collection.Count(x => x.Key.StartsWith(key) && x.Timestamp >= start && x.Timestamp <= end);
    }

    public int Count(IncompleteDate range)
    {
        var (start, end) = range.GetRange();
        return _collection.Count(x => x.Timestamp >= start && x.Timestamp <= end);
    }

    public int DeleteRange(string key, IncompleteDate range)
    {
        var (start, end) = range.GetRange();
        return _collection.DeleteMany(x => x.Key.StartsWith(key) && x.Timestamp >= start && x.Timestamp <= end);
    }

    public List<TimeEntry<T>> Search<T>(string key, string keyword, int maxCount = 0)
    {
        var query = _collection.Query()
            .Where(x => x.Key.StartsWith(key))
            .OrderByDescending(x => x.Timestamp);
        
        var records = query.ToList();
        var keywordLower = keyword.ToLowerInvariant();
        
        var results = records.Where(r =>
        {
            var data = r.Data;
            // Search in all BsonDocument values
            foreach (var element in data)
            {
                if (element.Value.IsString && element.Value.AsString.ToLowerInvariant().Contains(keywordLower))
                {
                    return true;
                }
            }
            return false;
        });
        
        if (maxCount > 0)
        {
            results = results.Take(maxCount);
        }
        
        return results.Select(r => new TimeEntry<T>(
            r.Key,
            new IncompleteDate(r.Timestamp.Year, r.Timestamp.Month, r.Timestamp.Day, 
                             r.Timestamp.Hour, r.Timestamp.Minute, r.Timestamp.Second),
            BsonMapper.Global.Deserialize<T>(r.Data)
        )).ToList();
    }

    public IncompleteDate? GetEarliestTimestamp(string key)
    {
        var record = _collection.Query()
            .Where(x => x.Key.StartsWith(key))
            .OrderBy(x => x.Timestamp)
            .Limit(1)
            .FirstOrDefault();
        
        if (record == null) return null;
        
        return new IncompleteDate(record.Timestamp.Year, record.Timestamp.Month, record.Timestamp.Day,
                                record.Timestamp.Hour, record.Timestamp.Minute, record.Timestamp.Second);
    }

    public IncompleteDate? GetLatestTimestamp(string key)
    {
        var record = _collection.Query()
            .Where(x => x.Key.StartsWith(key))
            .OrderByDescending(x => x.Timestamp)
            .Limit(1)
            .FirstOrDefault();
        
        if (record == null) return null;
        
        return new IncompleteDate(record.Timestamp.Year, record.Timestamp.Month, record.Timestamp.Day,
                                record.Timestamp.Hour, record.Timestamp.Minute, record.Timestamp.Second);
    }

    public IncompleteDate? GetEarliestTimestamp()
    {
        var record = _collection.Query()
            .OrderBy(x => x.Timestamp)
            .Limit(1)
            .FirstOrDefault();
        
        if (record == null) return null;
        
        return new IncompleteDate(record.Timestamp.Year, record.Timestamp.Month, record.Timestamp.Day,
                                record.Timestamp.Hour, record.Timestamp.Minute, record.Timestamp.Second);
    }

    public IncompleteDate? GetLatestTimestamp()
    {
        var record = _collection.Query()
            .OrderByDescending(x => x.Timestamp)
            .Limit(1)
            .FirstOrDefault();
        
        if (record == null) return null;
        
        return new IncompleteDate(record.Timestamp.Year, record.Timestamp.Month, record.Timestamp.Day,
                                record.Timestamp.Hour, record.Timestamp.Minute, record.Timestamp.Second);
    }

    public bool HasSummary<T>(string key, IncompleteDate timestamp, Func<T, bool> summaryPropertySelector)
    {
        var (start, _) = timestamp.GetRange();
        var records = _collection.Find(x => x.Key.StartsWith(key) && x.Timestamp == start);
        
        foreach (var record in records)
        {
            var data = BsonMapper.Global.Deserialize<T>(record.Data);
            if (data != null && summaryPropertySelector(data))
            {
                return true;
            }
        }
        
        return false;
    }

    public List<TimeEntry<T>> QueryWithLevel<T>(string key, IncompleteDate level)
    {
        var (start, end) = level.GetRange();
        
        var query = _collection.Query()
            .Where(x => x.Key.StartsWith(key) && x.Timestamp >= start && x.Timestamp <= end)
            .OrderBy(x => x.Timestamp);
        
        var records = query.ToList();
        
        return records.Select(r => new TimeEntry<T>(
            r.Key,
            new IncompleteDate(r.Timestamp.Year, r.Timestamp.Month, r.Timestamp.Day, 
                             r.Timestamp.Hour, r.Timestamp.Minute, r.Timestamp.Second),
            BsonMapper.Global.Deserialize<T>(r.Data)
        )).ToList();
    }
}
