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
/// Abstract base class for LiteDB admin window localization.
/// Language-specific subclasses must override all members to provide
/// translated strings for their respective locales.
/// </summary>
public abstract class LiteDBAdminLocalization
{
    /// <summary>
    /// Gets the window title for the LiteDB management interface.
    /// </summary>
    public abstract string WindowTitle { get; }

    // Toolbar buttons
    /// <summary>
    /// Gets the label for the refresh button.
    /// </summary>
    public abstract string Refresh { get; }

    /// <summary>
    /// Gets the label for the new collection button.
    /// </summary>
    public abstract string NewCollection { get; }

    /// <summary>
    /// Gets the label for the drop collection button.
    /// </summary>
    public abstract string DropCollection { get; }

    /// <summary>
    /// Gets the label for the rename collection button.
    /// </summary>
    public abstract string RenameCollection { get; }

    /// <summary>
    /// Gets the label for the add document button.
    /// </summary>
    public abstract string AddDocument { get; }

    /// <summary>
    /// Gets the label for the edit document button.
    /// </summary>
    public abstract string EditDocument { get; }

    /// <summary>
    /// Gets the label for the delete document button.
    /// </summary>
    public abstract string DeleteDocument { get; }

    // Labels / columns
    /// <summary>
    /// Gets the column header for document JSON display.
    /// </summary>
    public abstract string ColumnJson { get; }

    /// <summary>
    /// Gets the format string for collection info display.
    /// Parameters: collection name, document count, showing count.
    /// </summary>
    public abstract string CollectionInfoFormat { get; }

    /// <summary>
    /// Gets the message when no collection is selected.
    /// </summary>
    public abstract string NoCollectionSelected { get; }

    /// <summary>
    /// Gets the message when no document is selected.
    /// </summary>
    public abstract string NoDocumentSelected { get; }

    /// <summary>
    /// Gets the ready status message.
    /// </summary>
    public abstract string Ready { get; }

    // Prompts
    /// <summary>
    /// Gets the prompt for collection name input.
    /// </summary>
    public abstract string PromptCollectionName { get; }

    /// <summary>
    /// Gets the prompt for new collection name input.
    /// </summary>
    public abstract string PromptCollectionNewName { get; }

    /// <summary>
    /// Gets the prompt for document JSON input.
    /// </summary>
    public abstract string PromptDocumentJson { get; }

    // Confirmations
    /// <summary>
    /// Gets the confirmation message for dropping a collection.
    /// Parameter: collection name.
    /// </summary>
    public abstract string ConfirmDropCollection { get; }

    /// <summary>
    /// Gets the confirmation message for deleting a document.
    /// Parameter: document _id.
    /// </summary>
    public abstract string ConfirmDeleteDocument { get; }

    // Status messages
    /// <summary>
    /// Gets the status message when collections are loaded.
    /// Parameter: collection count.
    /// </summary>
    public abstract string StatusCollectionsLoaded { get; }

    /// <summary>
    /// Gets the status message when a collection is created.
    /// Parameter: collection name.
    /// </summary>
    public abstract string StatusCollectionCreated { get; }

    /// <summary>
    /// Gets the status message when a collection is dropped.
    /// Parameter: collection name.
    /// </summary>
    public abstract string StatusCollectionDropped { get; }

    /// <summary>
    /// Gets the status message when a collection is renamed.
    /// Parameters: old name, new name.
    /// </summary>
    public abstract string StatusCollectionRenamed { get; }

    /// <summary>
    /// Gets the status message when documents are loaded.
    /// Parameters: document count, collection name.
    /// </summary>
    public abstract string StatusDocumentsLoaded { get; }

    /// <summary>
    /// Gets the status message when a document is inserted.
    /// </summary>
    public abstract string StatusDocumentInserted { get; }

    /// <summary>
    /// Gets the status message when a document is updated.
    /// </summary>
    public abstract string StatusDocumentUpdated { get; }

    /// <summary>
    /// Gets the status message when a document is deleted.
    /// </summary>
    public abstract string StatusDocumentDeleted { get; }

    // Errors
    /// <summary>
    /// Gets the error message when loading collections fails.
    /// </summary>
    public abstract string ErrorLoadCollections { get; }

    /// <summary>
    /// Gets the error message when loading documents fails.
    /// </summary>
    public abstract string ErrorLoadDocuments { get; }

    /// <summary>
    /// Gets the error message when creating a collection fails.
    /// </summary>
    public abstract string ErrorCreateCollection { get; }

    /// <summary>
    /// Gets the error message when dropping a collection fails.
    /// </summary>
    public abstract string ErrorDropCollection { get; }

    /// <summary>
    /// Gets the error message when renaming a collection fails.
    /// </summary>
    public abstract string ErrorRenameCollection { get; }

    /// <summary>
    /// Gets the error message when a collection already exists.
    /// </summary>
    public abstract string ErrorCollectionExists { get; }

    /// <summary>
    /// Gets the error message when inserting a document fails.
    /// </summary>
    public abstract string ErrorInsertDocument { get; }

    /// <summary>
    /// Gets the error message when updating a document fails.
    /// </summary>
    public abstract string ErrorUpdateDocument { get; }

    /// <summary>
    /// Gets the error message when deleting a document fails.
    /// </summary>
    public abstract string ErrorDeleteDocument { get; }

    /// <summary>
    /// Gets the error message when a document is missing _id field.
    /// </summary>
    public abstract string ErrorMissingId { get; }
}
