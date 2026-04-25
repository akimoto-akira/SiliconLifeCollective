// Copyright (c) 2026 Silicon Life Collective
// Licensed under the Apache License, Version 2.0

using SiliconLife.Collective;

namespace SiliconLife.Default.Help;

/// <summary>
/// Korean help documentation implementation
/// 한국어 도움말 문서 구현
/// </summary>
public class HelpLocalizationKoKR : HelpLocalizationBase
{
    #region Help Documents
    
    public override string GettingStarted_Title => "빠른 시작";
    public override string BeingManagement_Title => "생명체 관리";
    public override string ChatSystem_Title => "채팅 시스템";
    public override string TaskTimer_Title => "작업 및 타이머";
    public override string Permission_Title => "권한 관리";
    public override string Config_Title => "설정";
    public override string FAQ_Title => "자주 묻는 질문";

    public override string[] GettingStarted_Tags => new[] { "설치", "시작", "설정", "빠른 시작", "시작하기", "개시", "초기화", "환경 구성" };
    public override string[] BeingManagement_Tags => new[] { "생명체", "생성", "구성", "생명체 관리", "실리콘 생명체", "프로필", "설정", "관리" };
    public override string[] ChatSystem_Tags => new[] { "채팅", "메시지", "대화", "채팅 시스템", "대화", "커뮤니케이션", "토크", "토론" };
    public override string[] TaskTimer_Tags => new[] { "작업", "타이머", "일정", "작업 및 타이머", "cron", "자동화", "반복", "알림" };
    public override string[] Permission_Tags => new[] { "권한", "보안", "접근 제어", "권한 관리", "인증", "인가", "개인정보", "보호" };
    public override string[] Config_Tags => new[] { "설정", "구성", "옵션", "설정 관리", "환경설정", "사용자 정의", "시스템", "매개변수" };
    public override string[] FAQ_Tags => new[] { "자주 묻는 질문", "도움말", "문제", "지원", "문제 해결", "가이드", "지원", "답변" };
    
    public override string GettingStarted => @"
# 빠른 시작

## 시스템 시작

명령줄에서 프로그램을 실행하면 시스템이 자동으로 시작되고 포트 8080에서 수신합니다.

```bash
dotnet run
```

## 웹 인터페이스 액세스

브라우저를 열고 `http://localhost:8080`에 접속하여 웹 관리 인터페이스에 진입합니다.

## 첫 번째 시작

첫 번째 시작 시 시스템이 자동으로 실리콘 큐레이터(관리자)를 생성합니다. 다음을 설정해야 합니다:

1. 큐레이터 이름 설정
2. 소울 파일(프롬프트) 구성
3. AI 모델 선택

## 기본 작업

- **대시보드**: 시스템 상태 및 통계 정보 확인
- **생명체**: 실리콘 생명체 생성 및 관리
- **채팅**: 실리콘 생명체와 대화
- **작업**: 정기 작업 설정
- **설정**: 시스템 설정 조정
- **도움말**: 이 문서 보기

## 키보드 단축키

- `F1` - 도움말 문서 열기
- `Ctrl+F` - 검색 상자에 포커스
";

    public override string BeingManagement => @"
# 생명체 관리

## 실리콘 생명체 생성

1. ""생명체"" 페이지로 이동
2. ""새 생명체 생성"" 클릭
3. 다음 정보 입력:
   - **이름**: 생명체의 표시 이름
   - **소울 파일**: 행동을 결정하는 핵심 프롬프트(Markdown 형식 지원)
   - **AI 모델**: 사용할 AI 모델 선택
4. ""생성"" 클릭하여 완료

## 소울 파일 구성

소울 파일은 실리콘 생명체의 핵심 프롬프트로, 행동 패턴, 성격 특성 및 능력 범위를 결정합니다.

### 작성 가이드라인

```markdown
# 역할 설정

당신은 전문 프로그래밍 어시스턴트입니다. 전문 분야:
- C# 개발
- 아키텍처 설계
- 코드 리뷰

# 행동 지침

1. 항상 실행 가능한 코드 예제 제공
2. 코드의 중요한 부분 설명
3. 모범 사례 권장 사항 제공
```

## 생명체 관리

- **편집**: 이름, 소울 파일 등 설정 수정
- **삭제**: 생명체 영구 삭제(복구 불가)
- **복제**: 기존 생명체를 기반으로 새 버전 생성
";

    public override string ChatSystem => @"
# 채팅 시스템

## 대화 시작

1. 대화할 실리콘 생명체 선택
2. 입력 상자에 메시지 입력
3. Enter 키를 누르거나 전송 버튼 클릭

## 채팅 기능

- **실시간 응답**: SSE 기술을 사용한 스트리밍 출력
- **도구 호출**: AI가 도구를 호출하여 작업 실행
- **컨텍스트 메모리**: 대화 기록 저장
- **다국어**: 여러 언어로 대화 지원

## 도구 사용

실리콘 생명체는 자동으로 도구를 호출하여 다음을 수행할 수 있습니다:
- 캘린더 정보 조회
- 시스템 설정 관리
- 코드 실행
- 파일 시스템 액세스(권한 필요)

## 대화 중단

AI가 생각 중일 때 다음을 수행할 수 있습니다:
- ""중지"" 버튼 클릭
- 또는 새 메시지 전송하여 자동 중단
";

    public override string TaskTimer => @"
# 작업 및 타이머

## 정기 작업 생성

1. ""작업"" 페이지로 이동
2. ""새 작업"" 클릭
3. 작업 구성:
   - **작업 이름**: 설명적인 이름
   - **트리거**: Cron 표현식
   - **작업**: 실행할 작업 선택
   - **대상 생명체**: 실행자 선택

## Cron 표현식

```
분 (0-59)
| 시간 (0-23)
| | 일 (1-31)
| | | 월 (1-12)
| | | | 요일 (0-6)
| | | | |
* * * * *
```

### 예제

- `0 * * * *` - 매시간 정각
- `0 9 * * *` - 매일 오전 9시
- `*/5 * * * *` - 5분마다
- `0 9 * * 1-5` - 평일 오전 9시

## 작업 관리

- **활성화/비활성화**: 작업 일시적으로 비활성화
- **편집**: 작업 설정 수정
- **삭제**: 작업 제거
- **실행 기록**: 작업 실행 기록 보기
";

    public override string Permission => @"
# 권한 관리

## 권한 수준

시스템은 5단계 권한 제어를 사용합니다:

1. **IsCurator**: 큐레이터가 최고 권한 보유
2. **UserFrequencyCache**: 사용자 빈도 캐시 제한
3. **GlobalACL**: 전역 접근 제어 목록
4. **IPermissionCallback**: 사용자 정의 권한 콜백
5. **IPermissionAskHandler**: 사용자에게 권한 문의

## 권한 유형

- **읽기**: 정보 및 데이터 보기
- **쓰기**: 데이터 수정 및 생성
- **실행**: 도구 및 명령 실행
- **관리**: 시스템 설정 관리

## 권한 구성

1. ""설정"" 페이지로 이동
2. ""권한 설정"" 선택
3. 권한 구성:
   - 특정 작업 허용/거부
   - 빈도 제한 설정
   - 화이트리스트/블랙리스트 구성

## 보안 권장 사항

- 정기적으로 권한 설정 검토
- 민감한 작업에 대한 접근 제한
- 작업 로깅 활성화
- 최소 권한 원칙 따르기
";

    public override string Config => @"
# 설정

## 시스템 설정

### AI 모델 설정

```json
{
  ""AI"": {
    ""DefaultProvider"": ""ollama"",
    ""Models"": {
      ""ollama"": {
        ""Endpoint"": ""http://localhost:11434"",
        ""Model"": ""llama3""
      }
    }
  }
}
```

### 네트워크 설정

- **포트**: 기본 8080
- **호스트**: 기본 localhost
- **HTTPS**: 선택적 활성화

### 저장소 설정

- **데이터 디렉토리**: 실리콘 생명체 데이터 저장 위치
- **로그 수준**: Debug/Info/Warning/Error
- **백업 전략**: 자동 백업 빈도

## 스킨 테마

시스템은 여러 인터페이스 테마를 지원합니다:

- **Light**: 밝은 테마(기본값)
- **Dark**: 어두운 테마
- **Custom**: 사용자 정의 테마 생성

## 현지화

지원되는 언어:
- 중국어 간체 (zh-CN)
- 중국어 번체 (zh-HK)
- 영어 (en-US, en-GB)
- 일본어 (ja-JP)
- 한국어 (ko-KR)
- 스페인어 (es-ES, es-MX)
- 체코어 (cs-CZ)
";

    public override string FAQ => @"
# 자주 묻는 질문

## 시스템 관련

### Q: 시스템이 시작되지 않으면 어떻게 해야 하나요?

A: 다음을 확인하세요:
1. 포트 8080이 이미 사용 중인지
2. .NET 9 런타임이 올바르게 설치되었는지
3. 로그 파일에서 자세한 오류 메시지 확인

### Q: 수신 포트를 변경하려면?

A: 설정 파일에서 `WebHost:Port`를 수정하거나 명령줄 인수를 사용합니다.

### Q: 데이터는 어디에 저장되나요?

A: 기본적으로 실리콘 생명체 루트 디렉토리 아래의 `data` 폴더에 저장됩니다.

## AI 관련

### Q: AI 응답이 느리면 어떻게 해야 하나요?

A: 가능한 원인:
1. 모델이 커서 더 많은 컴퓨팅 리소스 필요
2. 네트워크 지연(클라우드 모델 사용 시)
3. 로컬 모델(Ollama 등) 사용 고려

### Q: AI 모델을 전환하려면?

A: 설정 파일에서 `AI:DefaultProvider`를 수정하거나 생명체 구성에서 다른 모델을 선택합니다.

### Q: AI가 도구를 호출할 수 없나요?

A: 확인 사항:
1. 도구가 올바르게 등록되었는지
2. 권한이 도구 호출을 허용하는지
3. AI 모델이 도구 호출 기능을 지원하는지

## 생명체 관련

### Q: 생명체 데이터를 백업하려면?

A: 생명체 디렉토리 아래의 모든 파일을 백업 위치에 복사합니다.

### Q: 소울 파일을 가져오기/내보내기할 수 있나요?

A: 네. 생명체 편집 페이지에서 Markdown 형식의 소울 파일을 가져오기/내보내기할 수 있습니다.

### Q: 생명체를 복제하려면?

A: 생명체 목록에서 ""복제""를 선택하면 시스템이 복사본을 생성하며 설정을 수정할 수 있습니다.

## 도움말 얻기

위 내용으로 문제가 해결되지 않으면:

1. [프로젝트 문서](https://github.com/your-repo/docs) 확인
2. Issue를 제출하여 문제 보고
3. 커뮤니티 토론에 참여
";
    
    #endregion
}
