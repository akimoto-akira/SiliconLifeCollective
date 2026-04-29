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
/// Application configuration record for LiteDB storage
/// </summary>
internal class AppConfig
{
    public ObjectId Id { get; set; } = ObjectId.NewObjectId();
    public string ConfigType { get; set; } = "Default";
    public string DataDirectory { get; set; } = "./data";
    public Guid CuratorGuid { get; set; }
    public Language Language { get; set; } = Language.ZhCN;
    public int TickTimeoutMinutes { get; set; } = 10;
    public int MaxTimeoutCount { get; set; } = 3;
    public int WatchdogTimeoutMinutes { get; set; } = 10;
    public LogLevel MinimumLogLevel { get; set; } = LogLevel.Trace;
    public string AIClientType { get; set; } = "OllamaClient";
    public BsonDocument AIConfig { get; set; } = new BsonDocument();
    public int WebPort { get; set; } = 8080;
    public bool AllowIntranet { get; set; } = false;
    public string? WebSkin { get; set; }
    public string UserNickname { get; set; } = "User";
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Generic storage record for LiteDB
/// </summary>
internal class StorageRecord
{
    public ObjectId Id { get; set; } = ObjectId.NewObjectId();
    public string Key { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public BsonDocument Data { get; set; } = new BsonDocument();
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Time-series storage record for LiteDB
/// </summary>
internal class TimeRecord
{
    public ObjectId Id { get; set; } = ObjectId.NewObjectId();
    public string Key { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string DataType { get; set; } = string.Empty;
    public BsonDocument Data { get; set; } = new BsonDocument();
}

/// <summary>
/// Work note record for LiteDB
/// </summary>
internal class WorkNoteRecord
{
    public ObjectId Id { get; set; } = ObjectId.NewObjectId();
    public Guid NoteId { get; set; }
    public WorkNoteOwnerType OwnerType { get; set; }
    public string OwnerId { get; set; } = string.Empty;
    public int PageNumber { get; set; }
    public BsonDocument Data { get; set; } = new BsonDocument();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
