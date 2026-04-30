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
/// Japanese localization for LiteDB admin window.
/// </summary>
public class LiteDBAdminLocalizationJaJP : LiteDBAdminLocalization
{
    public override string WindowTitle => "LiteDB 管理";

    // Toolbar buttons
    public override string Refresh => "更新";
    public override string NewCollection => "新規コレクション";
    public override string DropCollection => "コレクション削除";
    public override string RenameCollection => "名前の変更";
    public override string AddDocument => "追加";
    public override string EditDocument => "編集";
    public override string DeleteDocument => "削除";

    // Labels / columns
    public override string ColumnJson => "ドキュメント (JSON)";
    public override string CollectionInfoFormat => "コレクション: {0}   ドキュメント数: {1}   表示: {2}";
    public override string NoCollectionSelected => "コレクションが選択されていません";
    public override string NoDocumentSelected => "ドキュメントが選択されていません";
    public override string Ready => "準備完了";

    // Prompts
    public override string PromptCollectionName => "コレクション名:";
    public override string PromptCollectionNewName => "新しいコレクション名:";
    public override string PromptDocumentJson => "ドキュメント (JSON):";

    // Confirmations
    public override string ConfirmDropCollection => "コレクション \"{0}\" とそのすべてのドキュメントを削除してもよろしいですか?";
    public override string ConfirmDeleteDocument => "_id = {0} のドキュメントを削除してもよろしいですか?";

    // Status messages
    public override string StatusCollectionsLoaded => "{0} 件のコレクションを読み込みました。";
    public override string StatusCollectionCreated => "コレクション \"{0}\" を作成しました。";
    public override string StatusCollectionDropped => "コレクション \"{0}\" を削除しました。";
    public override string StatusCollectionRenamed => "コレクションの名前を変更しました: {0} -> {1}。";
    public override string StatusDocumentsLoaded => "\"{1}\" から {0} 件のドキュメントを読み込みました。";
    public override string StatusDocumentInserted => "ドキュメントを挿入しました。";
    public override string StatusDocumentUpdated => "ドキュメントを更新しました。";
    public override string StatusDocumentDeleted => "ドキュメントを削除しました。";

    // Errors
    public override string ErrorLoadCollections => "コレクションの読み込みに失敗しました。";
    public override string ErrorLoadDocuments => "ドキュメントの読み込みに失敗しました。";
    public override string ErrorCreateCollection => "コレクションの作成に失敗しました。";
    public override string ErrorDropCollection => "コレクションの削除に失敗しました。";
    public override string ErrorRenameCollection => "コレクションの名の変更に失敗しました。";
    public override string ErrorCollectionExists => "その名前のコレクションは既に存在します。";
    public override string ErrorInsertDocument => "ドキュメントの挿入に失敗しました。";
    public override string ErrorUpdateDocument => "ドキュメントの更新に失敗しました。";
    public override string ErrorDeleteDocument => "ドキュメントの削除に失敗しました。";
    public override string ErrorMissingId => "ドキュメントには null ではない _id フィールドが必要です。";
}
