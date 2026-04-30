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
/// Czech localization for LiteDB admin window.
/// </summary>
public class LiteDBAdminLocalizationCsCZ : LiteDBAdminLocalization
{
    public override string WindowTitle => "Správa LiteDB";

    // Toolbar buttons
    public override string Refresh => "Aktualizovat";
    public override string NewCollection => "Nová kolekce";
    public override string DropCollection => "Smazat kolekci";
    public override string RenameCollection => "Přejmenovat";
    public override string AddDocument => "Přidat";
    public override string EditDocument => "Upravit";
    public override string DeleteDocument => "Smazat";

    // Labels / columns
    public override string ColumnJson => "Dokument (JSON)";
    public override string CollectionInfoFormat => "Kolekce: {0}   Dokumenty: {1}   Zobrazeno: {2}";
    public override string NoCollectionSelected => "Žádná kolekce nevybrána";
    public override string NoDocumentSelected => "Žádný dokument nevybrán";
    public override string Ready => "Připraveno";

    // Prompts
    public override string PromptCollectionName => "Název kolekce:";
    public override string PromptCollectionNewName => "Nový název kolekce:";
    public override string PromptDocumentJson => "Dokument (JSON):";

    // Confirmations
    public override string ConfirmDropCollection => "Smazat kolekci \"{0}\" a všechny její dokumenty?";
    public override string ConfirmDeleteDocument => "Smazat dokument s _id = {0}?";

    // Status messages
    public override string StatusCollectionsLoaded => "Načteno {0} kolekcí.";
    public override string StatusCollectionCreated => "Kolekce \"{0}\" vytvořena.";
    public override string StatusCollectionDropped => "Kolekce \"{0}\" smazána.";
    public override string StatusCollectionRenamed => "Kolekce přejmenována: {0} -> {1}.";
    public override string StatusDocumentsLoaded => "Načteno {0} dokumentů z \"{1}\".";
    public override string StatusDocumentInserted => "Dokument vložen.";
    public override string StatusDocumentUpdated => "Dokument aktualizován.";
    public override string StatusDocumentDeleted => "Dokument smazán.";

    // Errors
    public override string ErrorLoadCollections => "Nepodařilo se načíst kolekce.";
    public override string ErrorLoadDocuments => "Nepodařilo se načíst dokumenty.";
    public override string ErrorCreateCollection => "Nepodařilo se vytvořit kolekci.";
    public override string ErrorDropCollection => "Nepodařilo se smazat kolekci.";
    public override string ErrorRenameCollection => "Nepodařilo se přejmenovat kolekci.";
    public override string ErrorCollectionExists => "Kolekce s tímto názvem již existuje.";
    public override string ErrorInsertDocument => "Nepodařilo se vložit dokument.";
    public override string ErrorUpdateDocument => "Nepodařilo se aktualizovat dokument.";
    public override string ErrorDeleteDocument => "Nepodařilo se smazat dokument.";
    public override string ErrorMissingId => "Dokument musí obsahovat nenulové pole _id.";
}
