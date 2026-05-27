# IObjectFactory 등록 및 인스턴스 생성 예제

`IObjectFactory`의 등록과 인스턴스 생성을 데모: `OnLoad`에서 `RegisterAutoFactory`로 타입을 등록하고, `OnStart`에서 `CreateInstance`로 인스턴스를 생성합니다.

## IObjectFactory 인터페이스 개요

`IObjectFactory`는 `Activator.CreateInstance()`를 대체합니다. 플러그인은 `IPlugin.OnLoad`에서 팩토리 델리게이트를 등록하고, 런타임은 등록된 델리게이트를 통해서만 인스턴스를 생성하여 임의 타입 인스턴스화를 방지합니다.

```csharp
public interface IObjectFactory
{
    void RegisterFactory(Type type, Func<object?[], object> factory);
    void RegisterFactory<T>(Func<object?[], T> factory) where T : class;
    void RegisterAutoFactory(Type type);
    void RegisterAutoFactoryFromAssembly(System.Reflection.Assembly assembly, Type baseType);
    object? CreateInstance(Type type, params object?[] args);
    T? CreateInstance<T>(params object?[] args) where T : class;
    bool IsRegistered(Type type);
}
```

### 메서드 요약

| 메서드 | 설명 |
|--------|------|
| `RegisterFactory(Type, Func)` | 타입에 커스텀 팩토리 델리게이트 등록 |
| `RegisterFactory<T>(Func)` | `RegisterFactory`의 제네릭 버전 |
| `RegisterAutoFactory(Type)` | 타입의 생성자를 자동 분석하여 팩토리 등록 |
| `RegisterAutoFactoryFromAssembly(Assembly, Type)` | 어셈블리 내 비추상 서브타입의 팩토리를 일괄 등록 |
| `CreateInstance(Type, args)` | 등록된 팩토리로 인스턴스 생성 (비제네릭) |
| `CreateInstance<T>(args)` | 등록된 팩토리로 인스턴스 생성 (제네릭) |
| `IsRegistered(Type)` | 타입의 팩토리가 등록되어 있는지 확인 |

## IObjectFactory가 Activator.CreateInstance를 대체하는 이유

`Activator.CreateInstance`는 임의 타입 인스턴스화를 허용하므로 플러그인 시스템에서 보안 위험입니다. `IObjectFactory`는 화이트리스트 모델을 강제합니다:

- **등록된 팩토리**가 있는 타입만 인스턴스화 가능
- 팩토리는 `OnLoad`에서 명시적으로 등록되어 호스트가 완전 제어
- `RegisterAutoFactory`는 생성자를 자동 분석하는 편의 메서드지만 등록 게이트는 필수

```
❌ Activator.CreateInstance(typeof(SomeType))     → 보안 위험
✅ factory.CreateInstance(typeof(SomeType))         → 등록된 타입만
✅ factory.CreateInstance<SomeType>()               → 제네릭 편의 메서드
```

## RegisterAutoFactory 작동 방식

`RegisterAutoFactory`는 타입의 생성자를 검사하고 팩토리 델리게이트를 생성합니다:

1. **인수 없음** → 매개변수 없는 생성자 호출
2. **인수 있음** → 생성자 매개변수에 타입으로 매치, 불일치 시 매개변수 없는 생성자로 폴백
3. **추상/인터페이스 타입** → 경고와 함께 거부

## 등록 및 생성 흐름

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ ServiceLocator에서 IObjectFactory 가져오기                │
│  ├─ RegisterAutoFactory(typeof(SimpleService))               │
│  └─ RegisterAutoFactory(typeof(ConfiguredService))           │
│                                                              │
│  OnStart                                                     │
│  ├─ CreateInstance(typeof(SimpleService))                    │
│  │  → "SimpleService created via parameterless constructor"  │
│  └─ CreateInstance<ConfiguredService>("DemoPlugin")          │
│     → "ConfiguredService created with name='DemoPlugin'"     │
└──────────────────────────────────────────────────────────────┘
```

## 이 데모

> **⚠️ 참고:** `SimpleService`와 `ConfiguredService`는**이 데모를 위해서만 정의된 커스텀 타입**입니다. 시스템의 서비스 인터페이스와는 관계가 없습니다.

| 클래스 | 역할 |
|--------|------|
| `SimpleService` | 데모 타입, 매개변수 없는 생성자 |
| `ConfiguredService` | 데모 타입, 매개변수 있는 생성자 `(string name)` |
| `ObjectFactoryUsagePlugin` | `IPlugin` 구현 — 팩토리 등록 및 인스턴스 생성 |

## 보안 참고

`IObjectFactory`는 제어된 접근 보안 모델의 일부입니다. 플러그인은 `Activator.CreateInstance`를 사용하여 객체를 생성해서는 **안 됩니다**. 팩토리를 등록하고 `CreateInstance`를 사용해야 합니다. 자세한 내용은[보안 문서](../../docs/ko-KR/security.md)를 참조하세요.
