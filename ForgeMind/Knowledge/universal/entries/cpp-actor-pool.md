# 对象池解决高频 Actor 生成卡顿

## 适用场景

子弹、特效、掉落物等需要高频生成/销毁 Actor 的玩法。`SpawnActor` 含完整的组件初始化与注册开销，`Destroy` 会触发 GC 标记，两者叠加是常见的帧率杀手。

## 实现方案（C++）

```cpp
UCLASS()
class AActorPoolManager : public AActor
{
    GENERATED_BODY()

public:
    // 预热：游戏开始时一次性生成
    UFUNCTION(BlueprintCallable)
    void WarmUp(TSubclassOf<AActor> ActorClass, int32 Count)
    {
        for (int32 i = 0; i < Count; ++i)
        {
            AActor* actor = GetWorld()->SpawnActor<AActor>(ActorClass);
            actor->SetActorHiddenInGame(true);
            actor->SetActorEnableCollision(false);
            actor->SetActorTickEnabled(false);
            FreeActors.Add(actor);
        }
    }

    AActor* Acquire()
    {
        if (FreeActors.Num() == 0)
            return nullptr; // 池耗尽：按需扩容或丢弃

        AActor* actor = FreeActors.Pop();
        actor->SetActorHiddenInGame(false);
        actor->SetActorEnableCollision(true);
        actor->SetActorTickEnabled(true);
        return actor;
    }

    void Release(AActor* actor)
    {
        actor->SetActorHiddenInGame(true);
        actor->SetActorEnableCollision(false);
        actor->SetActorTickEnabled(false);
        FreeActors.Add(actor);
    }

private:
    UPROPERTY()
    TArray<AActor*> FreeActors;
};
```

## 集成要点

1. `WarmUp` 放在关卡初始化阶段调用，池大小取峰值并发量的 1.2~1.5 倍。
2. 归还（`Release`）时必须重置 Actor 状态（血量、计时器等），由使用方自行实现 `Reset()`。
3. 若 Actor 带粒子/声音组件，隐藏后确认组件也停止（`Deactivate`），避免隐藏对象持续消耗。
4. 网络游戏中对象池需放在服务器权威侧，客户端走复制生成，不套用本方案。

## 验证

用 `stat unit` 对比改造前后 Game 线程耗时；用 `stat game` 观察 SpawnActor 条目是否消失。
