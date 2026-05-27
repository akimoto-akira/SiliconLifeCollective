# 금지된 파일 I/O 작업 안티패턴

플러그인 시스템에서 **금지된** 파일 I/O 작업을 시연합니다. 본 예제는 안티패턴 참조로서, 해서는 안 되는 것을 보여주고 각 위반에 대한 올바른 대안을 제공합니다.

## 왜 System.IO가 전면 금지인가?

전체 `System.IO` 네임스페이스는 플러그인 수준에서 차단됩니다. 직접적인 파일 접근은 심각한 보안 위험을 초래하기 때문입니다:

1. **무단 파일 접근**: 플러그인이 워크스페이스 외부의 민감한 파일(암호, 키, 개인 데이터)을 읽을 수 있음
2. **파일 덮어쓰기 공격**: 악의적인 플러그인이 중요한 시스템 또는 구성 파일을 덮어쓸 수 있음
3. **디렉터리 순회**: 플러그인이 `../` 경로로 워크스페이스 경계를 벗어날 수 있음
4. **리소스 고갈**: 통제되지 않는 파일 생성이 디스크 공간을 가득 채울 수 있음
5. **감사 추적 부재**: 직접 파일 작업은 플러그인 보안 감사 시스템을 우회

## 금지되는 유형

파일 시스템에 직접 접근하는 모든 `System.IO` 유형이 차단됩니다:

| 금지되는 유형 | 차단되는 메서드 | 위험 수준 |
|-------------|---------------|----------|
| `File` | `ReadAllText`, `WriteAllText`, `AppendAllText` 등 | 🔴 심각 |
| `FileStream` | 파일 경로 포함 생성자 | 🔴 심각 |
| `Directory` | `GetFiles`, `GetDirectories`, `CreateDirectory` | 🔴 심각 |
| `StreamReader` | 파일 경로(문자열) 포함 생성자 | 🔴 심각 |
| `StreamWriter` | 파일 경로(문자열) 포함 생성자 | 🔴 심각 |
| `FileInfo` | 모든 메서드 | 🔴 심각 |
| `DirectoryInfo` | 모든 메서드 | 🔴 심각 |

## 허용되는 유형 (화이트리스트 예외)

**순수 메모리 작업**(파일 시스템에 직접 접근하지 않음)을 수행하는 유형은 허용됩니다:

| 허용되는 유형 | 용도 | 왜 안전한가 |
|-------------|------|-----------|
| `MemoryStream` | 메모리 바이트 스트림 | 파일 시스템 접근 없음 |
| `BinaryReader` | 기존 스트림에서 읽기 | 스트림을 래핑, 파일 열지 않음 |
| `BinaryWriter` | 기존 스트림에 쓰기 | 스트림을 래핑, 파일 생성하지 않음 |
| `GZipStream` | 압축/압축 해제 | 스트림을 래핑, 파일 접근 없음 |
| `StreamReader` | `Stream` 매개변수 포함 생성자 | 감사된 스트림 래핑 시 안전 |
| `StreamWriter` | `Stream` 매개변수 포함 생성자 | 감사된 스트림 래핑 시 안전 |

자세한 내용은 **04-SafeSystemIO** 예제를 참조하세요.

## PermissionedStreamFactory를 통한 안전한 파일 접근

`PermissionedStreamFactory`는 플러그인에서 파일 작업의 **통제된 진입점**입니다:

```csharp
// ✅ 올바름: 파일 읽기
using var readStream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(readStream);
string content = reader.ReadToEnd();

// ✅ 올바름: 파일 쓰기
using var writeStream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(writeStream);
writer.Write("로그 데이터");
```

**PermissionedStreamFactory가 제공하는 기능:**
1. **경로 검증**: 디렉터리 순회 공격(`../`) 방지
2. **권한 확인**: 파일이 허용된 워크스페이스 내에 있는지 확인
3. **감사 로그**: 모든 파일 접근이 보안 검토를 위해 기록됨
4. **리소스 정리**: 열린 스트림을 추적하고 누수 방지

## 본 예제의 위반 행위

### 위반 1: File.ReadAllText

```csharp
// ❌ 금지 — ⚠️ VIOLATION: [TypeRef] System.IO.File::ReadAllText
string content = File.ReadAllText("config.json");

// ✅ 올바른 대안
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
string content = reader.ReadToEnd();
```

### 위반 2: File.WriteAllText

```csharp
// ❌ 금지 — ⚠️ VIOLATION: [TypeRef] System.IO.File::WriteAllText
File.WriteAllText("output.log", "some data");

// ✅ 올바른 대안
using var stream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(stream);
writer.Write("some data");
```

### 위반 3: 직접 FileStream

```csharp
// ❌ 금지 — ⚠️ VIOLATION: [TypeRef] System.IO.FileStream::.ctor
using var fs = new FileStream("data.bin", FileMode.Open);

// ✅ 올바른 대안
using var fs = PermissionedStreamFactory.OpenRead("data.bin");
```

### 위반 4: Directory.GetFiles

```csharp
// ❌ 금지 — ⚠️ VIOLATION: [TypeRef] System.IO.Directory::GetFiles
string[] files = Directory.GetFiles("./logs", "*.txt");

// ✅ 올바른 대안 (SpeedyPack 사용)
using var pack = SpeedyPack.Open("logs.spk");
var entries = pack.ListEntries("/");
```

### 위반 5: 직접 경로 포함 StreamReader

```csharp
// ❌ 금지 — ⚠️ VIOLATION: [TypeRef] System.IO.StreamReader::.ctor(string)
using var reader = new StreamReader("config.json");

// ✅ 올바른 대안
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
```

## 다른 예제와의 비교

| 예제 | 핵심 내용 | 필요한 권한 |
|------|----------|-----------|
| **04-SafeSystemIO** | 허용된 메모리 유형 (MemoryStream, GZipStream) | 없음 |
| **07-ForbiddenFileIO** | 금지된 파일 접근 패턴 (본 예제) | 해당 없음 (차단됨) |
| **14-CapabilityFileIO** | FileIO 능력을 선언하여 제한 우회 | `Capability.FileIO` |

## PluginLoader 보안 스캔 메커니즘

PluginLoader가 이 플러그인을 스캔할 때:

1. **TypeRef 스캔**: 금지된 `System.IO` 유형에 대한 참조 감지
2. **MemberRef 스캔**: 차단된 메서드 호출 감지
3. **IL 문자열 스캔**: 문자열 기반 리플렉션 우회 시도 감지
4. **거부**: 플러그인이 로드 시 상세한 오류 메시지와 함께 거부됨

문자열 연결, 리플렉션, 동적 로딩, 난독화를 통한 우회는 불가능합니다 — IL 수준 스캔으로 포착됩니다 (**12-ForbiddenStringBypass** 참조).

## 보안 참고

파일에 대한 무제한 접근이 정말로 필요한 경우 `Capability.FileIO`를 선언할 수 있습니다 (14-CapabilityFileIO 참조). 하지만 모범 사례는:
- 구조화된 데이터 저장에는 **SpeedyPack**을 우선 사용 (권한 선언 불필요)
- 파일 접근이 필요한 경우 **PermissionedStreamFactory** 사용 (통제된 진입점)
- 위 방안으로 충족할 수 없는 경우에만 `Capability.FileIO` 선언
