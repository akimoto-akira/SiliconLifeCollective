# ITypeRegistry 등록 및 쿼리 예제

`ITypeRegistry`의 등록 및 검색 기능을 데모: `OnLoad`에서 커스텀 타입을 등록하고, `OnStart`에서 `FindSubtypesOf`로 검색합니다.

## ITypeRegistry 인터페이스 개요

`ITypeRegistry`는 `AppDomain.CurrentDomain.GetAssemblies()` 리플렉션 스캔을 대체합니다. 플러그인은 `IPlugin.OnLoad`에서 노출할 타입을 명시적으로 등록하고, 런타임은 레지스트리에서만 타입을 검색합니다.

```csharp
public interface ITypeRegistry
{
    void RegisterType(Type type);
    void RegisterTypes(IEnumerable<Type> types);
    void RegisterFromAssembly(System.Reflection.Assembly assembly, Type baseType);
    Type? FindType(string fullName);
    IEnumerable<Type> FindSubtypesOf(Type baseType);
    IEnumerable<Type> FindImplementationsOf(Type interfaceType);
}
```

### 메서드 요약

| 메서드 | 설명 |
|--------|------|
| `RegisterType(Type)` | 단일 타입 등록 |
| `RegisterTypes(IEnumerable<Type>)` | 여러 타입 일괄 등록 |
| `RegisterFromAssembly(Assembly, Type)` | 지정 어셈블리에서 `baseType`의 비추상 서브타입을 모두 등록 |
| `FindType(string)` | 전체 이름으로 타입 검색; 제네릭 타입 이름 해석 지원 |
| `FindSubtypesOf(Type)` | 지정 기반 타입의 비추상 서브타입을 모두 검색 |
| `FindImplementationsOf(Type)` | 지정 인터페이스를 구현하는 비추상 타입을 모두 검색 |

## 등록 및 쿼리 흐름

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ ServiceLocator에서 ITypeRegistry 가져오기                 │
│  ├─ RegisterType(typeof(GreetingTool))                       │
│  ├─ RegisterType(typeof(FarewellTool))                       │
│  └─ RegisterType(typeof(StatusTool))                         │
│                                                              │
│  대안: RegisterFromAssembly                                   │
│  └─ RegisterFromAssembly(assembly, typeof(DemoTool))         │
│     → DemoTool 서브타입을 한 번에 등록                       │
│                                                              │
│  OnStart                                                     │
│  ├─ FindSubtypesOf(typeof(DemoTool))                         │
│  └─ 결과 반복 → GreetingTool, FarewellTool, StatusTool       │
└──────────────────────────────────────────────────────────────┘
```

## RegisterFromAssembly 사용법

`RegisterFromAssembly`는 어셈블리를 스캔하여 지정 기반 타입의 비추상 서브타입을 모두 등록합니다:

```csharp
_registry.RegisterFromAssembly(
    typeof(TypeRegistryUsagePlugin).Assembly,  // 스캔할 어셈블리
    typeof(DemoTool)                            // DemoTool 서브타입만 등록
);
```

이는 개별적으로 `RegisterType`을 호출하는 것과 동일하지만, 공통 기반 클래스를 공유하는 타입이 많을 때 더 간결합니다.

## 이 데모

> **⚠️ 중요:** `DemoTool`은 `ITypeRegistry`의 등록 및 쿼리를 데모하기 위해서만 정의된**커스텀 타입**입니다. 시스템의 AI 도구 등록에 사용되는 `ITool` 인터페이스(`SiliconLife.Collective.ITool`)와는**전혀 관계가 없습니다**. 이름에 "Tool"이 포함된 것은 우연이며, 어떤 커스텀 클래스 계층 구조도 동일한 방식으로 작동합니다.

| 클래스 | 역할 |
|--------|------|
| `DemoTool` | 커스텀 추상 기반 클래스 — 등록 앵커（`ITool`과 무관） |
| `GreetingTool` | `OnLoad`에서 등록된 구체적 서브타입 |
| `FarewellTool` | `OnLoad`에서 등록된 구체적 서브타입 |
| `StatusTool` | `OnLoad`에서 등록된 구체적 서브타입 |
| `TypeRegistryUsagePlugin` | `IPlugin` 구현 — 타입 등록 및 쿼리 |

## 보안 참고

`ITypeRegistry`는 제어된 접근 보안 모델의 일부입니다. 플러그인은 `AppDomain.CurrentDomain.GetAssemblies()`나 `Assembly.GetTypes()`를 사용하여 타입을 발견해서는 **안 됩니다**. 대신 `ITypeRegistry`를 사용해야 합니다. 자세한 내용은[보안 문서](../../docs/ko-KR/security.md)를 참조하세요.
