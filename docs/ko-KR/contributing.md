# 기여 가이드

> **버전: v0.2.0-alpha**

[English](../en/contributing.md) | [Deutsch](../de-DE/contributing.md) | [中文](../zh-CN/contributing.md) | [繁體中文](../zh-HK/contributing.md) | [Español](../es-ES/contributing.md) | [日本語](../ja-JP/contributing.md) | **한국어** | [Čeština](../cs-CZ/contributing.md) | [Русский](../ru-RU/contributing.md)

실리콘 라이프 콜렉티브에 기여해 주셔서 감사합니다!

## 듀얼 버전 기여

이 프로젝트에는 두 가지 구현 버전이 있으며, 관심에 따라 기여 방향을 선택할 수 있습니다:

### SiliconLife.Default (디폴트 버전)
- **기술 스택**: .NET 9 콘솔 애플리케이션
- **기여 방향**: 핵심 기능 개발, 툴 구현, 로컬라이제이션, 문서
- **적합 대상**: 모든 개발자

### SiliconLife.Fast (고성능 버전)
- **기술 스택**: .NET 9 크로스 플랫폼 데스크톱 애플리케이션 (Avalonia UI)
- **기여 방향**: 퍼포먼스 최적화, SpeedyPack 스토리지, 시스템 트레이, 락프리 동시성
- **적합 대상**: 데스크톱 개발 경험이 있고 퍼포먼스 최적화에 관심 있는 개발자

> **중요**: 두 버전은 실리콘 라이프 코어 라이브러리와 SiliconLife.Common 프로젝트를 공유하며, 핵심 인터페이스 개선은 두 버전 모두에 동시에 영향을 미칩니다.

## 행동 강령

이 프로젝트는 Apache 2.0 라이선스를 따릅니다. 모든 상호작용에서 존중과 전문성을 유지해 주세요.

---

## 빠른 시작

### 1. 저장소 포크

GitHub에서 "Fork" 버튼을 클릭하여 본인의 복사본을 생성합니다.

### 2. 포크 클론

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. 개발 환경 설정

```bash
# .NET 9 SDK 설치
# https://dotnet.microsoft.com/download/dotnet/9.0

# 의존성 복원
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

### 5. 개발 프로젝트 선택

기여 유형에 따라 적절한 프로젝트를 선택하세요:

- **핵심 인터페이스/추상 클래스** → `SiliconLife.Core` 수정
- **공유 구현** → `SiliconLife.Common` 수정
- **디폴트 버전 전용** → `SiliconLife.Default` 수정
- **Fast 버전 전용** → `SiliconLife.Fast` 수정
- **스토리지 엔진** → `SiliconLife.Speedy` 수정
- **스토리지 관리 툴** → `SiliconLife.Speedy.Manager` 수정
- **플러그인 개발** → `SiliconLife.Core/Plugins` 수정
- **다국어 문서** → `docs/` 디렉토리 수정

---

## 개발 워크플로우

### 코드 스타일

- C# 코딩 규칙 준수
- 클래스명은 PascalCase 사용
- 메서드 매개변수는 camelCase 사용
- 비공개 필드는 `_camelCase` 사용
- 모든 공용 API에 XML 문서 필수

### 커밋 메시지

**컨벤셔널 커밋** 형식을 따릅니다:

```
<type>(<scope>): <description>
```

**타입**:
- `feat`: 새 기능
- `fix`: 버그 수정
- `docs`: 문서 변경
- `style`: 코드 포맷
- `refactor`: 코드 리팩토링
- `test`: 테스트 변경
- `chore`: 빌드/툴 변경

**예시**:
```bash
feat(localization): add Korean language support
fix(permission): fix null pointer in callback
docs: update contributing guide
refactor(web): simplify controller structure
```

### 변경 사항 적용

1. **코드 작성**
   - 기존 패턴 따르기
   - 새 기능에 테스트 추가
   - 문서 업데이트

2. **변경 사항 테스트**
   ```bash
   # 모든 테스트 실행
   dotnet test

   # 릴리스 모드로 빌드
   dotnet build --configuration Release
   ```

3. **코드 포맷팅**
   ```bash
   dotnet format
   ```

4. **변경 커밋**
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

## 풀 리퀘스트 가이드

### PR 제목

커밋 메시지와 동일한 형식을 사용합니다:
```
feat(localization): add Korean language support
```

### PR 설명

다음 내용을 포함하세요:

1. **무엇** - 이 PR은 무엇을 하나요?
2. **왜** - 이 변경이 필요한 이유는 무엇인가요?
3. **어떻게** - 어떻게 구현했나요?
4. **테스트** - 어떻게 테스트했나요?

### PR 설명 예시

```markdown
## 무엇
모든 UI 컴포넌트와 문서에 한국어 로컬라이제이션을 추가합니다.

## 왜
한국어 사용자의 프로젝트 접근성을 확대하기 위해서입니다.

## 어떻게
- KoKR.cs 로컬라이제이션 파일 생성
- 500개 이상의 번역 키 추가
- 모든 뷰에서 로컬라이제이션을 사용하도록 업데이트
- docs/ko-KR/에 한국어 문서 생성

## 테스트
- 모든 UI 요소가 한국어를 올바르게 표시하는지 확인
- 언어 전환 기능 테스트
- 원어민과 번역 검토
```

---

## 기여 유형

### 1. 버그 수정

**프로세스**:
1. 기존 이슈 확인
2. 이슈가 없으면 생성
3. 버그 수정
4. 테스트 케이스 추가
5. PR 제출

**요구사항**:
- 버그를 명확하게 설명
- 재현 단계 포함
- 회귀 방지 테스트 포함

### 2. 새 기능

**프로세스**:
1. Issues/Discussions에서 기능 논의
2. 유지관리자 승인 획득
3. 기능 구현
4. 포괄적인 테스트 추가
5. 문서 업데이트
6. PR 제출

**요구사항**:
- 기능 제안이 승인됨
- 완전한 테스트 커버리지
- 문서 업데이트 완료
- 하위 호환성 유지

### 3. 문서

**프로세스**:
1. 문서 공백 식별
2. 문서 작성/업데이트
3. PR 제출

**요구사항**:
- 명확하고 간결할 것
- 예시 포함
- 해당하는 경우 다국어 지원

### 4. 코드 리팩토링

**프로세스**:
1. Issue에서 리팩토링 제안
2. 승인 획득
3. 코드 리팩토링
4. 모든 테스트 통과 확인
5. PR 제출

**요구사항**:
- 기능 변경 없음
- 모든 테스트 통과
- 코드 품질 향상
- 명확한 설명

---

## 테스트 가이드

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

전체 워크플로우를 테스트합니다:
- AI 상호작용
- 툴 실행
- 퍼미션 검증
- 스토리지 작업

### 수동 테스트

UI 변경의 경우:
- 여러 브라우저에서 테스트
- 반응형 디자인 확인
- 접근성 검사

---

## 문서 가이드

### 코드 주석

- 모든 공용 API에 XML 주석 사용
- 복잡한 로직에 인라인 주석 사용
- 코드 주석은 영어로 작성

### 문서 파일

- `docs/{language}/`에 배치
- 모든 언어 버전 업데이트
- 기존 구조 따르기

### 다국어 문서

문서 추가 시:
1. 먼저 영어 버전 생성
2. 다른 언어로 번역
3. 콘텐츠 동기화 유지

---

## 리뷰 프로세스

### 유지관리자가 확인하는 사항

1. **코드 품질**
   - 규칙 준수
   - 명확하고 읽기 쉬움
   - 문서화 잘 됨

2. **테스트**
   - 충분한 커버리지
   - 모든 테스트 통과
   - 엣지 케이스 포함

3. **문서**
   - 업데이트됨
   - 명확한 설명
   - 다국어 지원

4. **호환성**
   - 하위 호환성 유지
   - 통지 없는 파괴적 변경 없음
   - 시맨틱 버전 규칙 준수

### 리뷰 타임라인

- 초기 리뷰: 1-3일
- 피드백 반영: 필요 시
- 병합: 승인 후

---

## 자주 묻는 질문

### PR이 거부된 경우

**이유**:
- 가이드라인 미준수
- 테스트 불충분
- 통지 없는 파괴적 변경
- 낮은 코드 품질

**해결 방법**:
- 피드백 해결
- PR 업데이트
- 재제출

### 병합 충돌

**해결 방법**:
```bash
# 브랜치 업데이트
git fetch origin
git rebase origin/master

# 충돌 해결
# 충돌 파일 편집
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
- 긴급 사항: 유지관리자 태그

---

## 감사의 말

기여자는 다음에서 인정받습니다:
- README.md 기여자 섹션
- 릴리스 노트
- 프로젝트 문서

---

## 라이선스

기여함으로써, 귀하의 기여가 Apache 2.0 라이선스에 따라 라이선스되는 것에 동의하게 됩니다.

---

## 다음 단계

- 📚 [문서](../) 읽기
- 🐛 [열린 이슈](https://github.com/akimoto-akira/SiliconLifeCollective/issues) 확인
- 💬 [토론](https://github.com/akimoto-akira/SiliconLifeCollective/discussions) 시작
- 🚀 포크하고 기여 시작하기!

실리콘 라이프 콜렉티브에 기여해 주셔서 감사합니다!🎉
