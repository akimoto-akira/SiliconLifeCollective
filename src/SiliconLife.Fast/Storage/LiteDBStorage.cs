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
/// LiteDB implementation of IStorage interface
/// </summary>
public class LiteDBStorage : IStorage
{
    private readonly ILiteCollection<StorageRecord> _collection;

    public LiteDBStorage()
    {
        _collection = LiteDBManager.StorageCollection;
    }

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
            existing.UpdatedAt = DateTime.UtcNow;
            _collection.Update(existing);
        }
        else
        {
            var record = new StorageRecord
            {
                Key = key,
                DataType = typeof(T).FullName ?? typeof(T).Name,
                Data = bsonData.AsDocument,
                UpdatedAt = DateTime.UtcNow
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
}
