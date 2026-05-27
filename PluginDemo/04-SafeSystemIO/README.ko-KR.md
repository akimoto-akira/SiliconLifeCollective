# 안전한 System.IO 데모

`SystemIOAllowedTypes` 화이트리스트의 System.IO 타입을 데모: `MemoryStream`, `BinaryReader`/`BinaryWriter`, `GZipStream`. `FileStream`에 `PermissionedStreamFactory`가 필요한 이유를 설명합니다.

## SystemIOAllowedTypes 화이트리스트

플러그인 런타임은 기본적으로 `System.IO` 네임스페이스를 차단하지만, **직접 파일 I/O를 수행하지 않는** 타입은 예외입니다:

| 카테고리 | 허용된 타입 | 안전한 이유 |
|----------|------------|------------|
| 스트림 추상 | `Stream` | 추상 기본 클래스, 자체 I/O 없음 |
| 메모리 스트림 | `MemoryStream` | 순수 메모리 작업 |
| 압축 스트림 | `GZipStream`, `DeflateStream`, `ZLibStream` | 다른 스트림을 래핑, 파일을 열지 않음 |
| 바이너리 래퍼 | `BinaryReader`, `BinaryWriter` | 임의의 스트림을 래핑, 파일을 열지 않음 |
| 열거형 | `SeekOrigin`, `FileMode`, `FileAccess`, `FileShare`, `CompressionMode`, `CompressionLevel` | 값 타입만 |
| 예외 | `IOException`, `InvalidDataException`, `EndOfStreamException` | 오류 타입만 |

### 화이트리스트에 없는 타입

이 타입들은**직접 파일 시스템에 접근**하므로 플러그인 코드에서**차단**됩니다:

| 차단된 타입 | 차단 이유 | 안전한 대안 |
|------------|----------|------------|
| `FileStream` | 직접 파일 열기 | `PermissionedStreamFactory.CreateReadStream()` / `CreateWriteStream()` |
| `File` | 정적 파일 작업 | `PermissionedStreamFactory` + `SafePath` |
| `Directory` | 정적 디렉토리 작업 | `SafePath` (권한 확인) |
| `FileInfo` | 파일 경로 래핑 | `SafePath` |
| `DirectoryInfo` | 디렉토리 경로 래핑 | `SafePath` |
| `StreamReader` | 직접 파일 열기 | `PermissionedStreamFactory` + `PermissionedStream` 래핑 |
| `StreamWriter` | 직접 파일 열기 | `PermissionedStreamFactory` + `PermissionedStream` 래핑 |

## 왜 FileStream에 PermissionedStreamFactory가 필요한가

`FileStream`은 직접 디스크 파일을 엽니다 — 플러그인 시스템에서 중대한 보안 위험입니다. `PermissionedStreamFactory`는 다음을 강제합니다:

1. **권한 확인** — 호출자의 `PermissionManager`가 해당 경로의 `FileAccess` 권한을 부여해야 함
2. **감사 로그** — 파일을 열 때마다 호출자의 being ID가 기록됨
3. **경로 검증** — 빈/잘못된 경로는 I/O 전에 거부됨

```
❌ new FileStream("path", FileMode.Open)           → TypeRef 스캐너에 의해 차단
✅ PermissionedStreamFactory.CreateReadStream(id, "path")  → 권한 확인 통과
✅ PermissionedStreamFactory.CreateWriteStream(id, "path") → 권한 확인 통과
```

## 데모 파이프라인

이 데모는 화이트리스트 타입만 사용하여 완전한 메모리 내 데이터 파이프라인을 구축합니다:

```
┌─────────────────────────────────────────────────────────────────┐
│  데모 1: MemoryStream                                           │
│  └─ 바이트 쓰기 → 바이트 읽기 → 문자열 디코딩                   │
│                                                                  │
│  데모 2: 압축 파이프라인                                         │
│  └─ string → UTF8 → MemoryStream                                │
│     → GZipStream(압축) → MemoryStream(압축됨)                    │
│     → GZipStream(압축해제) → MemoryStream(원본)                  │
│     → UTF8 → string (왕복 확인)                                  │
│                                                                  │
│  데모 3: BinaryReader/Writer                                     │
│  └─ Write(int, double, string) → MemoryStream                   │
│     → Read(int, double, string) → 왕복 확인                      │
└─────────────────────────────────────────────────────────────────┘
```

## 이 데모

> **⚠️ 참고:** 이 데모는 `SystemIOAllowedTypes` 화이트리스트 타입**만** 사용하며, 파일 I/O를 수행하지 않습니다. 파일 접근은 `PermissionedStreamFactory` API를 참조하세요.

| 클래스 | 역할 |
|--------|------|
| `SafeSystemIOPlugin` | `IPlugin` 구현 — 안전한 System.IO 사용법 데모 |

## 보안 참고

`System.IO` 네임스페이스는 플러그인 TypeRef 스캐너에 의해 차단되며, 화이트리스트 타입만 통과할 수 있습니다. 실제 파일 접근에는 `PermissionedStreamFactory`를 사용해야 하며, 이는 권한 확인과 감사 로그를 수행합니다. 자세한 내용은[보안 문서](../../docs/ko-KR/security.md)를 참조하세요.
