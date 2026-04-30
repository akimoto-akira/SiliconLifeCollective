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
/// English (US) localization for LiteDB admin window.
/// </summary>
public class LiteDBAdminLocalizationEnUS : LiteDBAdminLocalization
{
    public override string WindowTitle => "LiteDB Management";

    // Toolbar buttons
    public override string Refresh => "Refresh";
    public override string NewCollection => "New Collection";
    public override string DropCollection => "Drop Collection";
    public override string RenameCollection => "Rename";
    public override string AddDocument => "Add";
    public override string EditDocument => "Edit";
    public override string DeleteDocument => "Delete";

    // Labels / columns
    public override string ColumnJson => "Document (JSON)";
    public override string CollectionInfoFormat => "Collection: {0}   Documents: {1}   Showing: {2}";
    public override string NoCollectionSelected => "No collection selected";
    public override string NoDocumentSelected => "No document selected";
    public override string Ready => "Ready";

    // Prompts
    public override string PromptCollectionName => "Collection name:";
    public override string PromptCollectionNewName => "New collection name:";
    public override string PromptDocumentJson => "Document (JSON):";

    // Confirmations
    public override string ConfirmDropCollection => "Drop collection \"{0}\" and all its documents?";
    public override string ConfirmDeleteDocument => "Delete document with _id = {0}?";

    // Status messages
    public override string StatusCollectionsLoaded => "{0} collection(s) loaded.";
    public override string StatusCollectionCreated => "Collection \"{0}\" created.";
    public override string StatusCollectionDropped => "Collection \"{0}\" dropped.";
    public override string StatusCollectionRenamed => "Collection renamed: {0} -> {1}.";
    public override string StatusDocumentsLoaded => "{0} document(s) loaded from \"{1}\".";
    public override string StatusDocumentInserted => "Document inserted.";
    public override string StatusDocumentUpdated => "Document updated.";
    public override string StatusDocumentDeleted => "Document deleted.";

    // Errors
    public override string ErrorLoadCollections => "Failed to load collections.";
    public override string ErrorLoadDocuments => "Failed to load documents.";
    public override string ErrorCreateCollection => "Failed to create collection.";
    public override string ErrorDropCollection => "Failed to drop collection.";
    public override string ErrorRenameCollection => "Failed to rename collection.";
    public override string ErrorCollectionExists => "A collection with that name already exists.";
    public override string ErrorInsertDocument => "Failed to insert document.";
    public override string ErrorUpdateDocument => "Failed to update document.";
    public override string ErrorDeleteDocument => "Failed to delete document.";
    public override string ErrorMissingId => "Document must contain a non-null _id field.";
}
