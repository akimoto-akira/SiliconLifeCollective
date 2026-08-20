# C++ 编译常见错误：包含顺序与定义顺序

## generated.h 包含顺序
### 错误现象
编译报错：`undefined type`、`incomplete type`、`cannot open file 'YourClass.generated.h'`

### 原因
UE 的反射系统要求 `.generated.h` 文件必须在类声明**之前**包含，且必须在所有其他项目头文件之后。

### 正确顺序
```cpp
#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "YourClass.generated.h"  // ← 必须最后包含

UCLASS()
class AYourClass : public AActor
{
    GENERATED_BODY()
    // ...
};
```

### 常见错误
```cpp
// ❌ 错误：generated.h 在其他头文件之前
#include "YourClass.generated.h"
#include "GameFramework/Actor.h"

// ❌ 错误：遗漏 CoreMinimal.h
#include "GameFramework/Actor.h"
#include "YourClass.generated.h"
```

---

## 变量定义顺序
### 错误现象
编译报错：`use of undefined type`、`incomplete type is not allowed`

### 原因
C++ 是单遍编译，类成员变量必须在使用前声明。若变量 A 的类型依赖变量 B 的定义，B 必须先声明。

### 正确示例
```cpp
UCLASS()
class ATestActor : public AActor
{
    GENERATED_BODY()
    
public:
    // ✅ 简单类型在前
    UPROPERTY(BlueprintReadWrite)
    int32 TestInt = 42;
    
    UPROPERTY(BlueprintReadWrite)
    float TestFloat = 3.14f;
    
    // ✅ 复杂类型在后
    UPROPERTY(BlueprintReadWrite)
    FVector TestVector = FVector::ZeroVector;
    
    UPROPERTY(BlueprintReadWrite)
    TArray<int32> TestArray;
    
    // ✅ 对象引用最后
    UPROPERTY(BlueprintReadWrite)
    ACameraActor* TestCamera = nullptr;
};
```

### 常见错误
```cpp
// ❌ 错误：自定义结构体未前置声明
UPROPERTY(BlueprintReadWrite)
TArray<FMyStruct> MyArray;  // FMyStruct 未定义

// ✅ 修复：前置声明或调整头文件包含
USTRUCT(BlueprintType)
struct FMyStruct { /* ... */ };

UPROPERTY(BlueprintReadWrite)
TArray<FMyStruct> MyArray;
```

---

## 验证日期
2026-08-20
