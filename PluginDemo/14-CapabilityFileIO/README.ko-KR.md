# PluginDemo-14: Capability.FileIO — 선언적 파일 I/O 권한

## 개요

이 플러그인은 `[PluginCapability(Capability.FileIO)]`를 사용하여 플러그인이 직접 파일 시스템 액세스가 필요함을 선언하는 방법을 보여줍니다. 이 기능을 선언하면 `SystemIOAllowedTypes` 허용 목록을 넘어 모든 `System.IO` 유형에 액세스할 수 있습니다.

## PluginCapability 선언 구문

```csharp
[PluginCapability(Capability.FileIO, Reason = "Direct log file access for audit trail")]
public class CapabilityFileIOPlugin : IPlugin { ... }
```

## Capability.FileIO의 작동 방식

1. **기본 상태**: `System.IO` 네임스페이스가 전면 금지되며, `SystemIOAllowedTypes` 허용 목록 유형만 사용 가능 (MemoryStream, BinaryReader, GZipStream 등)
2. **선언 후**: `System.IO` 네임스페이스 금지가 해제됨 — File, FileStream, Directory, StreamReader(string) 등 모두 액세스 가능
3. **ILString 면제**: `"System.IO."`로 시작하는 문자열 상수는 플래그되지 않음
4. **선언 불가 제한**: P/Invoke, Unsafe, Reflection.Emit 등은 여전히 차단됨

## Capability.FileIO 면제 범위

### TypeRef 면제

모든 `System.IO` 유형이 면제됩니다:

| 범주 | 면제 유형 |
|------|----------|
| 파일 작업 | `File`, `FileInfo` |
| 디렉토리 작업 | `Directory`, `DirectoryInfo` |
| 스트림 유형 | `FileStream`, `StreamReader(path)`, `StreamWriter(path)` |
| 파일 시스템 | `FileSystemWatcher`, `DriveInfo`, `Path` |

### ILString 면제

- `"System.IO."`로 시작하는 문자열은 플래그되지 않음

### 여전히 금지되는 기능

| 범주 | 여전히 차단됨 |
|------|-------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` |
| 안전하지 않은 코드 | `UnverifiableCodeAttribute`, `Unsafe` |
| IL 방출 | `System.Reflection.Emit.*` |
| 어셈블리 로딩 | `System.Runtime.Loader`, `Assembly.Load*` |
| 레지스트리 | `Microsoft.Win32.*` |

## 다른 예제와의 비교

| 예제 | 선언 | 파일 액세스 | 비고 |
|------|------|-----------|------|
| **04-SafeSystemIO** | 없음 | MemoryStream, BinaryReader, GZipStream만 | 허용 목록 유형만 사용 |
| **07-ForbiddenFileIO** | 없음 | ❌ 거부됨 | 안티패턴 예시 |
| **14-CapabilityFileIO** | `[PluginCapability(Capability.FileIO)]` | ✅ 전체 System.IO 액세스 | 이 예제 |
| **20-SpeedyPack** | 없음 | SpeedyPack API를 통해 (Capability 불필요) | 권장되는 데이터 스토리지 |

## 파일 액세스 우선 순위

1. **SpeedyPack** — 기능 선언 불필요. 내장 캐싱, WAL, 트랜잭션. **구조화된 데이터 저장소에 권장.**
2. **PermissionedStreamFactory** — 기능 선언 불필요. 감사된 액세스, 경로 검증 및 액세스 제어 포함.
3. **Capability.FileIO + 직접 System.IO** — 위 옵션으로 충분하지 않을 때만 사용.

## PermissionedStreamFactory / SpeedyPack를 권장하는 이유는?

`Capability.FileIO`가 있어도 제어된 진입점 사용이 권장되는 이유:

1. **감사 추적**: 모든 액세스가 기록되고 추적 가능
2. **경로 검증**: 디렉토리 순회 공격 방지 (`../`)
3. **액세스 제어**: 워크스페이스 경계 강제 실행
4. **리소스 추적**: 스트림 누수 및 리소스 고갈 방지
5. **규정 준수**: 제어된 액세스 패턴으로 보안 검토 통과가 용이

## 보안 모범 사례

1. **정말 필요한 경우에만 FileIO 선언**: SpeedyPack이나 PermissionedStreamFactory로 대체할 수 없는가?
2. **명확한 Reason 제공**: "Direct log file access for audit trail"이 "file access"보다 나음
3. **경로를 직접 검증**: Capability.FileIO가 있어도 사용 전 모든 파일 경로를 검증
4. **using 문 사용**: 항상 FileStream/StreamReader/StreamWriter를 해제
5. **최소 권한 원칙**: 플러그인이 실제로 필요로 하는 기능만 선언

## 파일

- `Plugin.cs` — Capability.FileIO를 선언하는 데모 플러그인
- `README.md` — 이 파일 (영어)
- `README.zh-CN.md` — 중국어 간체
- 번역: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 관련 예제

- **04-SafeSystemIO**: 허용되는 인메모리 System.IO 유형 (선언 불필요)
- **07-ForbiddenFileIO**: 차단되는 파일 작업 안티패턴
- **20-SpeedyPack**: 기능 선언이 불필요한 권장 데이터 스토리지
- **18-CapabilityDenied**: 선언 불가 기능 안티패턴
