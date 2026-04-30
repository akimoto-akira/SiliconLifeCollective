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
/// Korean localization for LiteDB admin window.
/// </summary>
public class LiteDBAdminLocalizationKoKR : LiteDBAdminLocalization
{
    public override string WindowTitle => "LiteDB 관리";

    // Toolbar buttons
    public override string Refresh => "새로 고침";
    public override string NewCollection => "새 컬렉션";
    public override string DropCollection => "컬렉션 삭제";
    public override string RenameCollection => "이름 변경";
    public override string AddDocument => "추가";
    public override string EditDocument => "편집";
    public override string DeleteDocument => "삭제";

    // Labels / columns
    public override string ColumnJson => "문서 (JSON)";
    public override string CollectionInfoFormat => "컬렉션: {0}   문서 수: {1}   표시: {2}";
    public override string NoCollectionSelected => "선택된 컬렉션이 없습니다";
    public override string NoDocumentSelected => "선택된 문서가 없습니다";
    public override string Ready => "준비";

    // Prompts
    public override string PromptCollectionName => "컬렉션 이름:";
    public override string PromptCollectionNewName => "새 컬렉션 이름:";
    public override string PromptDocumentJson => "문서 (JSON):";

    // Confirmations
    public override string ConfirmDropCollection => "컬렉션 \"{0}\"과(와) 모든 문서를 삭제하시겠습니까?";
    public override string ConfirmDeleteDocument => "_id = {0}인 문서를 삭제하시겠습니까?";

    // Status messages
    public override string StatusCollectionsLoaded => "{0}개의 컬렉션을 로드했습니다.";
    public override string StatusCollectionCreated => "컬렉션 \"{0}\"이(가) 생성되었습니다.";
    public override string StatusCollectionDropped => "컬렉션 \"{0}\"이(가) 삭제되었습니다.";
    public override string StatusCollectionRenamed => "컬렉션 이름이 변경되었습니다: {0} -> {1}.";
    public override string StatusDocumentsLoaded => "\"{1}\"에서 {0}개의 문서를 로드했습니다.";
    public override string StatusDocumentInserted => "문서가 삽입되었습니다.";
    public override string StatusDocumentUpdated => "문서가 업데이트되었습니다.";
    public override string StatusDocumentDeleted => "문서가 삭제되었습니다.";

    // Errors
    public override string ErrorLoadCollections => "컬렉션 로드 실패.";
    public override string ErrorLoadDocuments => "문서 로드 실패.";
    public override string ErrorCreateCollection => "컬렉션 생성 실패.";
    public override string ErrorDropCollection => "컬렉션 삭제 실패.";
    public override string ErrorRenameCollection => "컬렉션 이름 변경 실패.";
    public override string ErrorCollectionExists => "해당 이름의 컬렉션이 이미 존재합니다.";
    public override string ErrorInsertDocument => "문서 삽입 실패.";
    public override string ErrorUpdateDocument => "문서 업데이트 실패.";
    public override string ErrorDeleteDocument => "문서 삭제 실패.";
    public override string ErrorMissingId => "문서에는 null이 아닌 _id 필드가 있어야 합니다.";
}
