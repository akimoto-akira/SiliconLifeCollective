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

namespace SiliconLife.Fast.LiteDB;

/// <summary>
/// Chinese (Traditional) localization for LiteDB admin window.
/// </summary>
public class LiteDBAdminLocalizationZhHK : LiteDBAdminLocalization
{
    public override string WindowTitle => "LiteDB 管理";

    // Toolbar buttons
    public override string Refresh => "重新整理";
    public override string NewCollection => "新增集合";
    public override string DropCollection => "刪除集合";
    public override string RenameCollection => "重新命名";
    public override string AddDocument => "新增";
    public override string EditDocument => "編輯";
    public override string DeleteDocument => "刪除";

    // Labels / columns
    public override string ColumnJson => "文件 (JSON)";
    public override string CollectionInfoFormat => "集合: {0}   文件數: {1}   顯示: {2}";
    public override string NoCollectionSelected => "未選擇集合";
    public override string NoDocumentSelected => "未選擇文件";
    public override string Ready => "就緒";

    // Prompts
    public override string PromptCollectionName => "集合名稱:";
    public override string PromptCollectionNewName => "新集合名稱:";
    public override string PromptDocumentJson => "文件 (JSON):";

    // Confirmations
    public override string ConfirmDropCollection => "確定要刪除集合 \"{0}\" 及其所有文件嗎?";
    public override string ConfirmDeleteDocument => "確定要刪除 _id = {0} 的文件嗎?";

    // Status messages
    public override string StatusCollectionsLoaded => "已載入 {0} 個集合。";
    public override string StatusCollectionCreated => "集合 \"{0}\" 已建立。";
    public override string StatusCollectionDropped => "集合 \"{0}\" 已刪除。";
    public override string StatusCollectionRenamed => "集合已重新命名: {0} -> {1}。";
    public override string StatusDocumentsLoaded => "已從 \"{1}\" 載入 {0} 個文件。";
    public override string StatusDocumentInserted => "文件已插入。";
    public override string StatusDocumentUpdated => "文件已更新。";
    public override string StatusDocumentDeleted => "文件已刪除。";

    // Errors
    public override string ErrorLoadCollections => "載入集合失敗。";
    public override string ErrorLoadDocuments => "載入文件失敗。";
    public override string ErrorCreateCollection => "建立集合失敗。";
    public override string ErrorDropCollection => "刪除集合失敗。";
    public override string ErrorRenameCollection => "重新命名集合失敗。";
    public override string ErrorCollectionExists => "已存在同名集合。";
    public override string ErrorInsertDocument => "插入文件失敗。";
    public override string ErrorUpdateDocument => "更新文件失敗。";
    public override string ErrorDeleteDocument => "刪除文件失敗。";
    public override string ErrorMissingId => "文件必須包含非空的 _id 欄位。";
}
