# PluginDemo-08: 금지된 네트워크 작업 안티패턴

## 개요

이 플러그인은 SiliconLife 플러그인 시스템에서 **금지**된 네트워크 작업을演示합니다. 안티패턴 참조로 무엇을 하면 안 되는지, 각 위반에 대해 올바른 대안을 제공합니다.

## 왜 직접적인 네트워크 접근이 전역적으로 금지되어 있나요?

직접적인 네트워크 접근 패턴은 플러그인 수준에서 차단됩니다:

1. **악의적인 서버 연결**: 플러그인이 악의적인 서버에 연결하여 공격 명령을 받을 수 있음
2. **데이터 유출**: 플러그인이 샌드박스에서 외부 서버로 민감한 데이터를 유출할 수 있음
3. **DNS 재바인딩 공격**: 플러그인이 DNS 조작을 통해 보안 검사를 우회할 수 있음
4. **네트워크 ACL 우회**: 직접적인 네트워크 접근이 글로벌 ACL과 권한 시스템을 우회함

## 금지된 형식

네트워크에 직접 접근하는 모든 `System.Net` 형식이 차단됩니다:

| 금지 형식 | 차단되는 네임스페이스 | 위험 수준 |
|----------|----------------|----------|
| `HttpClient` | `System.Net.Http` | 🔴 심각 |
| `TcpClient` | `System.Net.Sockets` | 🔴 심각 |
| `Socket` | `System.Net.Sockets` | 🔴 심각 |
| `Dns` | `System.Net` | 🔴 심각 |
| `WebClient` | `System.Net` | 🔴 심각 |

## 안전하게 접근하는 방법

### NetworkExecutor (권장)

`NetworkExecutor`는 플러그인 네트워크 작업의 **제어된 진입점**입니다:

```csharp
// ✅ 올바름: 단순 GET 요청
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com/data"
});
```

**NetworkExecutor가 제공하는 기능:**
1. 권한 검사
2. 감사 로깅
3. 서킷 브레이커
4. 타임아웃 제어
5. 요청 큐

## 위반 사항演示

### 위반 1: HttpClient

```csharp
// ❌ 금지
using var client = new HttpClient();
var response = await client.GetStringAsync("https://api.example.com");

// ✅ 올바름
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com"
});
```

### 위반 2: TcpClient

```csharp
// ❌ 금지
using var client = new TcpClient("example.com", 8080);

// ✅ 올바름
// NetworkExecutor를 사용하거나 Capability.Network를 선언
```

### 위반 3: Dns

```csharp
// ❌ 금지
var hostEntry = Dns.GetHostEntry("example.com");

// ✅ 올바름
// NetworkExecutor가 필요한 DNS 해석을 내부적으로 처리
```

## PluginLoader 보안 메커니즘

PluginLoader가 이 플러그인을 스캔할 때:
1. **TypeRef 스캔**: 금지된 `System.Net.*` 형식에 대한 참조를 감지
2. **MemberRef 스캔**: 차단된 메서드에 대한 호출을 감지
3. **IL 문자열 스캔**: 리플렉션을 통한 금지 형식 로딩 시도 감지
4. **거부**: 플러그인이 로딩 시 거부됨

## 파일

- `Plugin.cs` - 안티패턴演示 플러그인
- `README.md` - 이 파일 (영어)
- `README.ko-KR.md` - 이 파일 (한국어)
- 다른 언어 버전...

## 관련 예제

- **13-CapabilityNetwork**: 선언적 Network 권한
- **07-ForbiddenFileIO**: 금지된 파일 접근 패턴