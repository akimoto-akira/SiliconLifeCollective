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
/// German localization for LiteDB admin window.
/// </summary>
public class LiteDBAdminLocalizationDeDE : LiteDBAdminLocalization
{
    public override string WindowTitle => "LiteDB-Verwaltung";

    // Toolbar buttons
    public override string Refresh => "Aktualisieren";
    public override string NewCollection => "Neue Sammlung";
    public override string DropCollection => "Sammlung löschen";
    public override string RenameCollection => "Umbenennen";
    public override string AddDocument => "Hinzufügen";
    public override string EditDocument => "Bearbeiten";
    public override string DeleteDocument => "Löschen";

    // Labels / columns
    public override string ColumnJson => "Dokument (JSON)";
    public override string CollectionInfoFormat => "Sammlung: {0}   Dokumente: {1}   Anzeige: {2}";
    public override string NoCollectionSelected => "Keine Sammlung ausgewählt";
    public override string NoDocumentSelected => "Kein Dokument ausgewählt";
    public override string Ready => "Bereit";

    // Prompts
    public override string PromptCollectionName => "Sammlungsname:";
    public override string PromptCollectionNewName => "Neuer Sammlungsname:";
    public override string PromptDocumentJson => "Dokument (JSON):";

    // Confirmations
    public override string ConfirmDropCollection => "Sammlung \"{0}\" und alle ihre Dokumente löschen?";
    public override string ConfirmDeleteDocument => "Dokument mit _id = {0} löschen?";

    // Status messages
    public override string StatusCollectionsLoaded => "{0} Sammlung(en) geladen.";
    public override string StatusCollectionCreated => "Sammlung \"{0}\" erstellt.";
    public override string StatusCollectionDropped => "Sammlung \"{0}\" gelöscht.";
    public override string StatusCollectionRenamed => "Sammlung umbenannt: {0} -> {1}.";
    public override string StatusDocumentsLoaded => "{0} Dokument(e) aus \"{1}\" geladen.";
    public override string StatusDocumentInserted => "Dokument eingefügt.";
    public override string StatusDocumentUpdated => "Dokument aktualisiert.";
    public override string StatusDocumentDeleted => "Dokument gelöscht.";

    // Errors
    public override string ErrorLoadCollections => "Sammlungen konnten nicht geladen werden.";
    public override string ErrorLoadDocuments => "Dokumente konnten nicht geladen werden.";
    public override string ErrorCreateCollection => "Sammlung konnte nicht erstellt werden.";
    public override string ErrorDropCollection => "Sammlung konnte nicht gelöscht werden.";
    public override string ErrorRenameCollection => "Sammlung konnte nicht umbenannt werden.";
    public override string ErrorCollectionExists => "Eine Sammlung mit diesem Namen existiert bereits.";
    public override string ErrorInsertDocument => "Dokument konnte nicht eingefügt werden.";
    public override string ErrorUpdateDocument => "Dokument konnte nicht aktualisiert werden.";
    public override string ErrorDeleteDocument => "Dokument konnte nicht gelöscht werden.";
    public override string ErrorMissingId => "Dokument muss ein nicht-null _id-Feld enthalten.";
}
