# 문제 해결 가이드

## 일반적인 문제

### 빌드 및 컴파일

#### 문제: 종속성 누락으로 빌드 실패

**증상**:
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**해결책**:
```bash
dotnet restore
dotnet build
```

#### 문제: .NET SDK를 찾을 수 없음

**증상**:
```
The .NET SDK could not be found
```

**해결책**:
1. .NET 9 SDK 설치: https://dotnet.microsoft.com/download/dotnet/9.0
2. 설치 확인:
```bash
dotnet --version
```

---

### AI 연결 문제

#### 문제: Ollama 연결 거부

**증상**:
```
Failed to connect to Ollama at http://localhost:11434
```

**해결책**:
```bash
# Ollama 실행 확인
ollama list

# Ollama 시작
ollama serve

# 연결 테스트
curl http://localhost:11434/api/tags
```

#### 문제: 모델을 찾을 수 없음

**증상**:
```
model "qwen2.5:7b" not found
```

**해결책**:
```bash
# 필요한 모델 풀
ollama pull qwen2.5:7b

# 사용 가능한 모델 목록
ollama list
```

#### 문제: DashScope 404 오류

**증상**:
```
HTTP 404: Model not found
```

**해결책**:
1. API 키 올바른지 확인
2. 모델 이름이 DashScope 카탈로그와 일치하는지 확인
3. 리전 엔드포인트 올바른지 확인
4. 계정에 해당 모델 접근 권한 있는지 확인

---

### 런타임 문제

#### 문제: 포트가 이미 사용 중

**증상**:
```
HttpListenerException: Address already in use
```

**해결책**:

**Windows**:
```bash
netstat -ano | findstr :8080
taskkill /PID <PID> /F
```

**Linux/Mac**:
```bash
lsof -ti:8080 | xargs kill -9
```

**또는** 설정에서 포트 변경.

#### 문제: Being이 시작되지 않음

**증상**:
- Being 상태가 "Error" 표시
- 로그에 초기화 실패 표시

**해결책**:
1. 소울 파일 존재 및 유효한지 확인
2. AI 클라이언트 설정되었는지 확인
3. 특정 오류 로그 확인:
```bash
tail -f logs/*.log
```

#### 문제: 메모리 부족

**증상**:
```
OutOfMemoryException
```

**해결책**:
1. 힙 크기 증가:
```bash
dotnet run --server.gcHeapCount 4
```

2. 오래된 데이터 정리:
```bash
# 오래된 로그 아카이브
mv logs/ logs-archive/
mkdir logs

# 오래된 메모리 정리
# Web UI 통함: Memory Management > Cleanup
```

---

### 권한 문제

#### 문제: 권한 거부

**증상**:
```
Permission denied: disk:write
```

**해결책**:
1. 현재 권한 확인:
```bash
curl http://localhost:8080/api/permissions
```

2. 권한 부여:
```bash
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

3. 또는 Web UI 사용: Permission Management

#### 문제: 권한이 만료되지 않음

**증상**:
- 권한이 만료 시간 지나도 지속

**해결책**:
1. 시스템 시계 동기화 확인
2. `expiresAt` 필드 올바르게 설정되었는지 확인
3. 권한 캐시 지우기

---

### Web UI 문제

#### 문제: Web UI 접근 불가

**증상**:
- 브라우저에 "Connection refused" 표시

**해결책**:
1. 서버 실행 중인지 확인
2. 올바른 URL 확인: `http://localhost:8080`
3. 방화벽 설정 확인
4. 시작 오류 로그 확인

#### 문제: SSE 작동 안 함

**증상**:
- 실시간 업데이트 나타나지 않음
- 채팅 스트리밍 안 됨

**해결책**:
1. 브라우저가 SSE 지원하는지 확인
2. SSE 프록시 버퍼링 비활성화
3. 네트워크 안정성 확인
4. 다른 브라우저 시도

#### 문제: UI 깨짐

**증상**:
- 스타일링 잘못됨
- 레이아웃 깨짐

**해결책**:
1. 브라우저 캐시 지우기
2. 다른 스킨 시도: Settings > Skin
3. 브라우저 콘솔에서 오류 확인
4. 브라우저 확장 프로그램 비활성화

---

### 저장소 문제

#### 문제: 데이터 읽기/쓰기 불가

**증상**:
```
IOException: Access denied
```

**해결책**:
1. 파일 권한 확인
2. 저장소 경로 존재하는지 확인
3. 디스크 공간 확인
4. 적절한 권한으로 실행

#### 문제: 데이터 손상

**증상**:
- JSON 구문 오류
- 데이터 누락

**해결책**:
1. 백업에서 복원
2. 저장소 무결성 확인:
```bash
# Web UI 통함: System > Storage Check
```

3. 손상된 파일 수동으로 수정

---

### 도구 실행 문제

#### 문제: 도구를 찾을 수 없음

**증상**:
```
Tool "xyz" not found
```

**해결책**:
1. 도구 이름 올바른지 확인
2. 도구가 Tools 디렉토리에 있는지 확인
3. 프로젝트 재빌드
4. 도구가 올바르게 구현되었는지 확인

#### 문제: 도구가 오류 반환

**증상**:
```
Tool execution failed: ...
```

**해결책**:
1. 도구 로그 확인
2. 입력 매개변수 검증
3. 독립적으로 도구 테스트
4. 권한 확인

---

## 디버깅

### 상세 로깅 활성화

설정 편집:
```json
{
  "logging": {
    "level": "debug"
  }
}
```

### 로그 확인

로그 저장 위치:
```
logs/
├── system.log
├── ai.log
├── permission.log
└── error.log
```

실시간 보기:
```bash
tail -f logs/*.log
```

### 디버거 사용

```bash
# 디버거와 함께 실행
dotnet run --project src/SiliconLife.Default --configuration Debug

# 디버거 연결
# IDE 통해: Attach to Process > SiliconLife.Default
```

---

## 성능 문제

### 느린 응답 시간

**최적화**:
1. AI 모델 복잡도 축소
2. 캐싱 활성화
3. 오래된 데이터 정리
4. 시스템 리소스 증가

### 높은 CPU 사용량

**확인**:
- 너무 많은 being 실행
- 도구의 무한 루프
- 빈번한 타이머 실행

**해결책**:
- 동시 being 축소
- 도구 코드 최적화
- 타이머 간격 조정

### 높은 메모리 사용량

**모니터링**:
```bash
# Web UI 통함: Dashboard > Memory
```

**최적화**:
- 오래된 메모리 정리
- 컨텍스트 크기 축소
- 페이지네이션 구현

---

## 도움 받기

### 문서 확인

- [시작 가이드](getting-started.md)
- [개발 가이드](development-guide.md)
- [API 레퍼런스](api-reference.md)
- [아키텍처 가이드](architecture.md)

### 로그 확인

항상 오류 세부정보를 위해 로그 먼저 확인.

### 커뮤니티 지원

- GitHub Issues: 버그 보고
- Discussions: 질문
- Documentation: 솔루션 검색

---

## 비상 절차

### 시스템 크래시

1. 원인 로그 확인
2. 애플리케이션 재시작:
```bash
dotnet run --project src/SiliconLife.Default
```

3. 필요 시 백업에서 복원

### 데이터 손실

1. 즉시 애플리케이션 중지
2. 백업 파일 확인
3. 데이터 복원
4. 무결성 검증

### 보안 침해

1. 모든 being 중지
2. 모든 권한 취소
3. 감사 로그 확인
4. 접근 제어 검토
5. 제한된 권한으로 재시작

---

## 예방

### 모범 사례

1. **정기 백업**
   - 데이터 디렉토리 백업
   - 설정 백업
   - 복원 절차 테스트

2. **리소스 모니터링**
   - CPU/메모리 사용량 감시
   - 디스크 공간 모니터링
   - 네트워크 연결 확인

3. **최신 상태 유지**
   - .NET SDK 업데이트
   - 종속성 업데이트
   - 보안 패치 적용

4. **변경사항 테스트**
   - 먼저 개발 환경에서 테스트
   - 버전 관리 사용
   - 변경사항 문서화

---

## 다음 단계

- 📚 [아키텍처 가이드](architecture.md) 읽기
- 🛠️ [개발 가이드](development-guide.md) 확인
- 🚀 [시작 가이드](getting-started.md) 참조
- 🔒 [보안 문서](security.md) 검토
