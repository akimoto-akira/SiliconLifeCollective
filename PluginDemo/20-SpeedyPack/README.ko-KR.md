# PluginDemo-20: SpeedyPack — 구조화된 데이터 스토리지

## 개요

이 플러그인은 `SpeedyPack`을 사용하여 구조화된 데이터 저장소를 구현하는 방법을 보여줍니다. **기능 선언이 전혀 필요하지 않습니다**. SpeedyPack은 플러그인 데이터 지속성에 **권장되는** 방법입니다.

## 왜 SpeedyPack인가?

| 기능 | SpeedyPack | PermissionedStreamFactory | Capability.FileIO + System.IO |
|------|-----------|--------------------------|------------------------------|
| 필요한 기능 선언 | **없음** | 없음 | `Capability.FileIO` |
| 캐싱 | ✅ 내장 | ❌ | ❌ |
| WAL (크래시 복구) | ✅ | ❌ | ❌ |
| 트랜잭션 | ✅ `IPackTransaction` | ❌ | ❌ |
| 스레드 안전 | ✅ | ❌ | ❌ |
| 구조화된 직렬화 | ✅ `Read<T>` | ❌ 원시 바이트 | ❌ 수동 |
| 감사 추적 | ✅ 자동 | ✅ 자동 | ❌ 수동 |

## 기본 CRUD

```csharp
// SpeedyPack 데이터 파일 열기
using var pack = SpeedyPack.Open("mydata.spk");

// 키-값 쌍 쓰기
pack.Write("user:name", "Alice");
pack.Write("user:age", 30);

// 값 읽기 (형식화)
string name = pack.Read<string>("user:name");  // "Alice"
int age = pack.Read<int>("user:age");           // 30

// 키 삭제
pack.Delete("user:age");

// 존재 확인
bool exists = pack.Contains("user:name");  // true
```

## 형식화된 액세스와 구조화된 객체

```csharp
public class UserProfile
{
    public string Name { get; set; }
    public int Level { get; set; }
    public string[] Tags { get; set; }
}

// 구조화된 객체 쓰기
var profile = new UserProfile { Name = "Bob", Level = 42, Tags = new[] { "admin" } };
pack.Write("profile:bob", profile);

// 형식화된 객체 읽기
var loaded = pack.Read<UserProfile>("profile:bob");
Console.WriteLine($"{loaded.Name}, Level {loaded.Level}");
```

## 트랜잭션

```csharp
using (var tx = pack.BeginTransaction())
{
    try
    {
        tx.Write("account:a", 1000);
        tx.Write("account:b", 500);
        tx.Commit();   // 원자성 — 두 쓰기가 모두 지속되거나 둘 다 지속되지 않음
    }
    catch
    {
        tx.Rollback();  // 이 트랜잭션의 모든 쓰기를 폐기
    }
}
```

### IPackTransaction 메서드

| 메서드 | 설명 |
|--------|------|
| `Write(key, value)` | 쓰기 작업을 큐에 추가 |
| `Delete(key)` | 삭제 작업을 큐에 추가 |
| `Commit()` | 큐의 모든 작업을 원자적으로 적용 |
| `Rollback()` | 큐의 모든 작업을 폐기 |

## SpeedyPackOptions 구성

```csharp
var options = new SpeedyPackOptions
{
    MaxCacheSize = 1024 * 1024,              // 1 MB 캐시
    AutoFlushInterval = TimeSpan.FromSeconds(30),
    CompressionLevel = CompressionLevel.Optimal
};
using var pack = SpeedyPack.Open("data.spk", options);
```

### SpeedyPackOptions 속성

| 속성 | 유형 | 기본값 | 설명 |
|------|------|--------|------|
| `MaxCacheSize` | `long` | 64 MB | 최대 메모리 캐시 크기 |
| `AutoFlushInterval` | `TimeSpan` | 10초 | 캐시를 디스크에 플러시하는 간격 |
| `CompressionLevel` | `CompressionLevel` | `Fastest` | 저장 데이터의 압축 수준 |

## 보안 참고

SpeedyPack은 기능 선언이 **불필요**합니다. 안전한 제어된 데이터 스토리지 진입점입니다:
- 모든 경로를 작업 공간 경계 내에서 검증
- 모든 읽기/쓰기 작업의 전체 감사 추적 제공
- 디렉토리 순회 공격 방지
- 리소스 수명 주기 자동 관리

## 파일

- `Plugin.cs` — SpeedyPack 데모 플러그인
- `README.md` — 이 파일 (영어)
- `README.zh-CN.md` — 중국어 간체
- 번역: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 관련 예제

- **04-SafeSystemIO**: 허용된 인메모리 System.IO 형식 (선언 불필요)
- **07-ForbiddenFileIO**: 차단된 파일 작업 안티패턴
- **14-CapabilityFileIO**: SpeedyPack으로 충분하지 않은 경우
