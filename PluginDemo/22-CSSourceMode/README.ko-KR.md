# CS 소스 컴파일 로딩 모드 데모

사전 컴파일된 DLL이 아닌 원본 `.cs` 소스 파일에서 로드되는 플러그인으로, PluginLoader의 CS 소스 컴파일 모드(task-389에서 도입)를 시연합니다.

## CS 소스 모드 작동 방식

PluginLoader가 플러그인 디렉토리를 스캔할 때 **DLL을 찾지 못하면** 자동으로 CS 소스 모드에 진입합니다:

```
1. PluginLoader가 플러그인 디렉토리 스캔 → DLL 없음
2. CS 소스 모드 진입
3. cs.txt 발견 → 줄별로 읽어 나열된 .cs 파일만 로드
   (cs.txt 없음 → 디렉토리의 모든 *.cs 파일 로드)
4. 형제 DLL 스캔 → 신뢰할 수 있는 DLL은 직접 참조로 추가;
   신뢰할 수 없는 DLL은 ScanForbiddenReferences 보안 스캔을 통과해야 참조 가능
5. CompilationCore(제한 모드)가 .cs 파일을 메모리 내 DLL로 컴파일
6. 메모리 내 DLL 바이트를 임시 파일에 쓰고 ScanForbiddenReferences 스캔 실행
7. 스캔 통과 → 리플렉션으로 IPlugin 구현 찾기 → 인스턴스화
8. 로그 표시: "Plugin loaded [CS-Source]: {Id} v{Version} from {DirName}"
```

## cs.txt — 선택적 로딩 허용 목록

`cs.txt` 파일은 컴파일할 `.cs` 파일을 지정합니다. 한 줄에 하나의 파일명:

```
Plugin.cs
```

- **나열된 파일**: 컴파일 및 로드됨 (예: `Plugin.cs`)
- **나열되지 않은 파일**: 컴파일러가 무시함 (예: `Helpers.cs`)
- **`#`로 시작하는 줄**: 주석으로 처리
- **빈 줄**: 무시됨
- **cs.txt 없음**: 디렉토리의 모든 `*.cs` 파일이 로드됨

이 데모는 의도적으로 cs.txt에 `Plugin.cs`만 나열합니다. `Helpers.cs`는 디렉토리에 존재하지만 컴파일에 **참여하지 않습니다**. 선택적 로딩 메커니즘을 시연하기 위한 것입니다.

## 디렉토리 구조

```
22-CSSourceMode/
├── Plugin.cs          — IPlugin 구현 (cs.txt에 나열됨 → 컴파일됨)
├── Helpers.cs         — 헬퍼 클래스 (cs.txt에 없음 → 컴파일 안 됨)
├── cs.txt             — 컴파일할 .cs 파일을 지정하는 허용 목록
├── README.md          — 영어 문서
├── README.ko-KR.md    — 한국어 문서
└── ...                — 기타 언어 README
```

## CS 소스 모드 vs DLL 모드

| 측면 | DLL 모드 | CS 소스 모드 |
|------|---------|-------------|
| 플러그인 형식 | 사전 컴파일된 `.dll` | 원본 `.cs` 소스 파일 |
| 로딩 트리거 | 플러그인 디렉토리에서 DLL 발견 | DLL 없음, `.cs` 파일 존재 |
| 컴파일 | 빌드 시 수행 | 로드 시 PluginLoader가 수행 |
| 성능 | 컴파일 오버헤드 없음 | 시작 시 Roslyn 컴파일 오버헤드 |
| 보안 스캔 | 직접 PE 메타데이터 스캔 | 컴파일 → 임시 DLL → PE 메타데이터 스캔 |
| 로그 접두사 | `Plugin loaded:` | `Plugin loaded [CS-Source]:` |
| 적합한 용도 | 프로덕션 배포 | 개발 반복 |

## 01-MinimalPlugin과의 비교

| 측면 | 01-MinimalPlugin | 22-CSSourceMode |
|------|------------------|-----------------|
| 플러그인 형식 | 사전 컴파일된 DLL | CS 소스 파일 |
| 로딩 경로 | 표준 DLL 로딩 | CS 소스 컴파일 |
| 기능 | 동등 | 동등 |
| IPlugin.Id | `com.siliconlife.demo.minimal` | `com.siliconlife.demo.cssource` |

## 오류 처리

| 시나리오 | 동작 |
|----------|------|
| DLL도 .cs 파일도 없음 | 경고: "No DLL and no CS source files found" |
| 컴파일 오류 | 오류: 상세 진단 메시지 로그 |
| 보안 스캔 실패 | 오류: 모든 위반 항목 나열, 플러그인 거부 |
| cs.txt 항목을 찾을 수 없음 | 경고: "cs.txt entry not found or not a .cs file" |
| 형제 DLL 스캔 실패 | 경고: DLL이 참조로 추가되지 않음, 컴파일 계속 |

## 보안 참고

CS 소스 모드 플러그인은 DLL 모드 플러그인과 **동일한 보안 스캔**을 받습니다. 컴파일된 어셈블리는 임시 DLL 파일에 기록되고 `ScanForbiddenReferences`로 스캔됩니다. 이는 사전 컴파일된 DLL이 받는 스캔과 동일합니다. 모든 금지된 네임스페이스/유형/멤버/문자열 규칙이 동일하게 적용됩니다.

플러그인은 여전히 격리된 컨텍스트에서 로드되며 금지된 네임스페이스 참조(예: `System.IO`, `System.Net.Http`)를 스캔합니다. 자세한 내용은 [보안 문서](../../docs/ko-KR/security.md)를 참조하세요.
