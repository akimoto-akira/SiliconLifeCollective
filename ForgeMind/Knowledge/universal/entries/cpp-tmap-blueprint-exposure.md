# TMap 蓝图暴露限制：不支持嵌套复杂类型

## 问题
在 C++ 中定义 `UPROPERTY(BlueprintReadWrite)` 的 TMap 成员变量时，编译报错提示无法在蓝图中暴露复杂类型。

## 原因
UE 蓝图系统对 TMap 的支持有严格限制：
- **支持**：`TMap<FString, int32>`、`TMap<FString, float>` 等简单键值对
- **不支持**：`TMap<FString, TArray<int32>>`、`TMap<FString, FMyStruct>` 等嵌套复杂类型

这是引擎层面的限制，蓝图虚拟机无法处理复杂的 TMap 值类型。

## 解决方案
1. 将嵌套结构打平：改用 `TMap<FString, int32>` 替代 `TMap<FString, TArray<int32>>`
2. 或改用 `TArray<FKeyValuePair>` 自定义结构体数组，用函数封装增删查改逻辑
3. 如必须使用复杂 TMap，用 `UPROPERTY()`（无 BlueprintReadWrite），提供 `UFUNCTION(BlueprintCallable)` 的 getter/setter

## 示例
```cpp
// ❌ 编译报错：蓝图无法暴露
UPROPERTY(BlueprintReadWrite)
TMap<FString, TArray<int32>> StringToIntArrays;

// ✅ 改用简单类型
UPROPERTY(BlueprintReadWrite)
TMap<FString, int32> StringToInt;
```

## 影响版本
所有 UE 版本（引擎固有限制）

## 验证日期
2026-08-20
