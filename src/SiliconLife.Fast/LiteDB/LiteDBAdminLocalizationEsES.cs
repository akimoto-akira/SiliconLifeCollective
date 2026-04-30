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
/// Spanish localization for LiteDB admin window.
/// </summary>
public class LiteDBAdminLocalizationEsES : LiteDBAdminLocalization
{
    public override string WindowTitle => "Administración de LiteDB";

    // Toolbar buttons
    public override string Refresh => "Actualizar";
    public override string NewCollection => "Nueva Colección";
    public override string DropCollection => "Eliminar Colección";
    public override string RenameCollection => "Renombrar";
    public override string AddDocument => "Agregar";
    public override string EditDocument => "Editar";
    public override string DeleteDocument => "Eliminar";

    // Labels / columns
    public override string ColumnJson => "Documento (JSON)";
    public override string CollectionInfoFormat => "Colección: {0}   Documentos: {1}   Mostrando: {2}";
    public override string NoCollectionSelected => "Ninguna colección seleccionada";
    public override string NoDocumentSelected => "Ningún documento seleccionado";
    public override string Ready => "Listo";

    // Prompts
    public override string PromptCollectionName => "Nombre de la colección:";
    public override string PromptCollectionNewName => "Nuevo nombre de la colección:";
    public override string PromptDocumentJson => "Documento (JSON):";

    // Confirmations
    public override string ConfirmDropCollection => "¿Eliminar la colección \"{0}\" y todos sus documentos?";
    public override string ConfirmDeleteDocument => "¿Eliminar el documento con _id = {0}?";

    // Status messages
    public override string StatusCollectionsLoaded => "{0} colección(es) cargada(s).";
    public override string StatusCollectionCreated => "Colección \"{0}\" creada.";
    public override string StatusCollectionDropped => "Colección \"{0}\" eliminada.";
    public override string StatusCollectionRenamed => "Colección renombrada: {0} -> {1}.";
    public override string StatusDocumentsLoaded => "{0} documento(s) cargado(s) de \"{1}\".";
    public override string StatusDocumentInserted => "Documento insertado.";
    public override string StatusDocumentUpdated => "Documento actualizado.";
    public override string StatusDocumentDeleted => "Documento eliminado.";

    // Errors
    public override string ErrorLoadCollections => "Error al cargar las colecciones.";
    public override string ErrorLoadDocuments => "Error al cargar los documentos.";
    public override string ErrorCreateCollection => "Error al crear la colección.";
    public override string ErrorDropCollection => "Error al eliminar la colección.";
    public override string ErrorRenameCollection => "Error al renombrar la colección.";
    public override string ErrorCollectionExists => "Ya existe una colección con ese nombre.";
    public override string ErrorInsertDocument => "Error al insertar el documento.";
    public override string ErrorUpdateDocument => "Error al actualizar el documento.";
    public override string ErrorDeleteDocument => "Error al eliminar el documento.";
    public override string ErrorMissingId => "El documento debe contener un campo _id no nulo.";
}
