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
/// Chinese (Simplified) localization for LiteDB admin window.
/// </summary>
public class LiteDBAdminLocalizationZhCN : LiteDBAdminLocalization
{
    public override string WindowTitle => "LiteDB 管理";

    // Toolbar buttons
    public override string Refresh => "刷新";
    public override string NewCollection => "新建集合";
    public override string DropCollection => "删除集合";
    public override string RenameCollection => "重命名";
    public override string AddDocument => "添加";
    public override string EditDocument => "编辑";
    public override string DeleteDocument => "删除";

    // Labels / columns
    public override string ColumnJson => "文档 (JSON)";
    public override string CollectionInfoFormat => "集合: {0}   文档数: {1}   显示: {2}";
    public override string NoCollectionSelected => "未选择集合";
    public override string NoDocumentSelected => "未选择文档";
    public override string Ready => "就绪";

    // Prompts
    public override string PromptCollectionName => "集合名称:";
    public override string PromptCollectionNewName => "新集合名称:";
    public override string PromptDocumentJson => "文档 (JSON):";

    // Confirmations
    public override string ConfirmDropCollection => "确定要删除集合 \"{0}\" 及其所有文档吗?";
    public override string ConfirmDeleteDocument => "确定要删除 _id = {0} 的文档吗?";

    // Status messages
    public override string StatusCollectionsLoaded => "已加载 {0} 个集合。";
    public override string StatusCollectionCreated => "集合 \"{0}\" 已创建。";
    public override string StatusCollectionDropped => "集合 \"{0}\" 已删除。";
    public override string StatusCollectionRenamed => "集合已重命名: {0} -> {1}。";
    public override string StatusDocumentsLoaded => "已从 \"{1}\" 加载 {0} 个文档。";
    public override string StatusDocumentInserted => "文档已插入。";
    public override string StatusDocumentUpdated => "文档已更新。";
    public override string StatusDocumentDeleted => "文档已删除。";

    // Errors
    public override string ErrorLoadCollections => "加载集合失败。";
    public override string ErrorLoadDocuments => "加载文档失败。";
    public override string ErrorCreateCollection => "创建集合失败。";
    public override string ErrorDropCollection => "删除集合失败。";
    public override string ErrorRenameCollection => "重命名集合失败。";
    public override string ErrorCollectionExists => "已存在同名集合。";
    public override string ErrorInsertDocument => "插入文档失败。";
    public override string ErrorUpdateDocument => "更新文档失败。";
    public override string ErrorDeleteDocument => "删除文档失败。";
    public override string ErrorMissingId => "文档必须包含非空的 _id 字段。";
}
