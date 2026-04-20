# 기여 가이드

SiliconLifeCollective에 관심을 가져주셔서 감사합니다!

## 행동 강령

이 프로젝트는 Apache 2.0 라이선스를 따릅니다. 모든 상호작용에서 존중하고 전문적으로 대하십시오.

---

## 시작하기

### 1. 저장소 포크

GitHub에서 "Fork" 버튼을 클릭하여 자체 복사본을 만듭니다.

### 2. 포크 클론

```bash
git clone https://github.com/your-username/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. 개발 환경 설정

```bash
# .NET 9 SDK 설치
# https://dotnet.microsoft.com/download/dotnet/9.0

# 종속성 복원
dotnet restore

# 프로젝트 빌드
dotnet build

# 테스트 실행
dotnet test
```

### 4. 기능 브랜치 생성

```bash
git checkout -b feature/your-feature-name
```

---

## 개발 워크플로우

### 코드 스타일

- C# 코딩 규칙 준수
- 클래스 이름에 PascalCase 사용
- 메서드 매개변수에 camelCase 사용
- private 필드에 _camelCase 사용
- 모든 공개 API에는 XML 문서 필수

### 커밋 메시지

**Conventional Commits** 형식 따르기:

```
<type>(<scope>): <description>
```

**타입**:
- `feat`: 새 기능
- `fix`: 버그 수정
- `docs`: 문서 변경
- `style`: 코드 포맷팅
- `refactor`: 코드 리팩토링
- `test`: 테스트 변경
- `chore`: 빌드/도구 변경

**예시**:
```bash
feat(localization): add Korean language support
fix(permission): fix null pointer in callback
docs: update contributing guide
refactor(web): simplify controller structure
```

### 변경사항 만들기

1. **코드 작성**
   - 기존 패턴 따르기
   - 새 기능에 테스트 추가
   - 문서 업데이트

2. **변경사항 테스트**
   ```bash
   # 모든 테스트 실행
   dotnet test
   
   # 릴리스 모드로 빌드
   dotnet build --configuration Release
   ```

3. **코드 포맷**
   ```bash
   dotnet format
   ```

4. **변경사항 커밋**
   ```bash
   git add .
   git commit -m "feat(scope): description"
   ```

5. **포크에 푸시**
   ```bash
   git push origin feature/your-feature-name
   ```

6. **풀 리퀘스트 생성**
   - 원본 저장소로 이동
   - "Compare & pull request" 클릭
   - PR 템플릿 작성
   - 제출

---

## 풀 리퀘스트 가이드라인

### PR 제목

커밋 메시지와 동일한 형식 사용:
```
feat(localization): add Korean language support
```

### PR 설명

포함할 내용:

1. **무엇** - 이 PR은 무엇을 하나요?
2. **왜** - 왜 이 변경이 필요한가요?
3. **어떻게** - 어떻게 구현했나요?
4. **테스트** - 어떻게 테스트했나요?

### PR 설명 예시

```markdown
## What
모든 UI 구성 요소 및 문서에 한국어 현지화 추가.

## Why
한국어 사용자의 프로젝트 접근성 확대.

## How
- KoKR.cs 현지화 파일 생성
- 500개 이상의 번역 키 추가
- 모든 뷰가 현지화 사용하도록 업데이트
- docs/ko-KR/에 한국어 문서 생성

## Testing
- 모든 UI 요소가 한국어를 올바르게 표시하는지 확인
- 언어 전환 기능 테스트
- 원어민과 번역 검토
```

---

## 기여 유형

### 1. 버그 수정

**프로세스**:
1. 기존 이슈 확인
2. 없는 경우 이슈 생성
3. 버그 수정
4. 테스트 케이스 추가
5. PR 제출

**요구사항**:
- 버그에 대한 명확한 설명
- 재현 단계
- 회귀를 방지하는 테스트

### 2. 새 기능

**프로세스**:
1. Issues/Discussions에서 기능 논의
2. 관리자로부터 승인 받기
3. 기능 구현
4. 포괄적인 테스트 추가
5. 문서 업데이트
6. PR 제출

**요구사항**:
- 기능 제안 승인
- 전체 테스트 커버리지
- 문서 업데이트
- 하위 호환성

### 3. 문서

**프로세스**:
1. 문서 격차 식별
2. 문서 작성/업데이트
3. PR 제출

**요구사항**:
- 명확하고 간결함
- 예시 포함
- 해당되는 경우 다국어

### 4. 코드 리팩토링

**프로세스**:
1. Issue에서 리팩토링 제안
2. 승인 받기
3. 코드 리팩토링
4. 모든 테스트 통과 확인
5. PR 제출

**요구사항**:
- 기능 변경 없음
- 모든 테스트 통과
- 코드 품질 개선
- 명확한 설명

---

## 테스트 가이드라인

### 단위 테스트

```csharp
[TestMethod]
public void MyFeature_ShouldWork_AsExpected()
{
    // Arrange
    var service = new MyService();
    
    // Act
    var result = service.DoSomething();
    
    // Assert
    Assert.IsTrue(result.Success);
}
```

### 통합 테스트

전체 워크플로우 테스트:
- AI 상호작용
- 도구 실행
- 권한 검증
- 저장소 작업

### 수동 테스트

UI 변경사항:
- 여러 브라우저에서 테스트
- 반응형 디자인 확인
- 접근성 확인

---

## 문서 가이드라인

### 코드 주석

- 모든 공개 API에 XML 주석
- 복잡한 로직에는 인라인 주석
- 코드 주석은 영어 사용

### 문서 파일

- `docs/{language}/`에 배치
- 모든 언어 버전 업데이트
- 기존 구조 따르기

### 다국어 문서

문서 추가 시:
1. 영어 버전 먼저 생성
2. 다른 언어로 번역
3. 콘텐츠 동기화 유지

---

## 검토 프로세스

### 관리자가 확인하는 사항

1. **코드 품질**
   - 규칙 준수
   - 깔끔하고 가독성 좋음
   - 잘 문서화됨

2. **테스트**
   - 적절한 커버리지
   - 모든 테스트 통과
   - 엣지 케이스 커버

3. **문서**
   - 업데이트됨
   - 명확한 설명
   - 다국어

4. **호환성**
   - 하위 호환성
   - 고지 없이 변경 없음
   - 시맨틱 버전 관리 준수

### 검토 타임라인

- 초기 검토: 1-3일
- 피드백 반영: 필요 시
- 병합: 승인 후

---

## 일반적인 문제

### PR 거부

**이유**:
- 가이드라인 미준수
- 테스트 부족
- 고지 없이 변경
- 낮은 코드 품질

**해결책**:
- 피드백 해결
- PR 업데이트
- 재제출

### 병합 충돌

**해결책**:
```bash
# 브랜치 업데이트
git fetch origin
git rebase origin/master

# 충돌 해결
# 충돌된 파일 편집
git add .
git rebase --continue

# 강제 푸시
git push --force-with-lease
```

---

## 도움 받기

### 리소스

- **문서**: [docs/](../)
- **이슈**: GitHub Issues
- **토론**: GitHub Discussions
- **행동 강령**: CODE_OF_CONDUCT.md

### 연락처

- 버그: 이슈 생성
- 질문: 토론 시작
- 긴급 사항: 관리자 태그

---

## 인정

기여자는 다음에서 인정받습니다:
- README.md 기여자 섹션
- 릴리스 노트
- 프로젝트 문서

---

## 라이선스

기여함으로써, 귀하의 기여가 Apache 2.0 라이선스에 따라 라이선스된다는 데 동의합니다.

---

## 다음 단계

- 📚 [문서](../) 읽기
- 🐛 [열린 이슈](https://github.com/your-org/SiliconLifeCollective/issues) 확인
- 💬 [토론](https://github.com/your-org/SiliconLifeCollective/discussions) 시작
- 🚀 포크하고 기여 시작!

SiliconLifeCollective에 기여해 주셔서 감사합니다! 🎉
