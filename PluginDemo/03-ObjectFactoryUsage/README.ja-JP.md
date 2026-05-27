# IObjectFactory 登録とインスタンス生成の例

`IObjectFactory` の登録とインスタンス生成をデモ：`OnLoad` で `RegisterAutoFactory` を使って型を登録し、`OnStart` で `CreateInstance` を使ってインスタンスを生成します。

## IObjectFactory インターフェース概要

`IObjectFactory` は `Activator.CreateInstance()` に代わるものです。プラグインは `IPlugin.OnLoad` でファクトリデリゲートを登録し、ランタイムは登録されたデリゲートのみを通じてインスタンスを生成し、任意の型のインスタンス化を防ぎます。

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

### メソッド一覧

| メソッド | 説明 |
|----------|------|
| `RegisterFactory(Type, Func)` | 型にカスタムファクトリデリゲートを登録 |
| `RegisterFactory<T>(Func)` | `RegisterFactory` のジェネリック版 |
| `RegisterAutoFactory(Type)` | 型のコンストラクタを自動分析してファクトリを登録 |
| `RegisterAutoFactoryFromAssembly(Assembly, Type)` | アセンブリ内の非抽象サブタイプのファクトリを一括登録 |
| `CreateInstance(Type, args)` | 登録済みファクトリでインスタンスを生成（非ジェネリック） |
| `CreateInstance<T>(args)` | 登録済みファクトリでインスタンスを生成（ジェネリック） |
| `IsRegistered(Type)` | 型のファクトリが登録されているか確認 |

## なぜ IObjectFactory が Activator.CreateInstance を置き換えるのか

`Activator.CreateInstance` は任意の型のインスタンス化を許可するため、プラグインシステムではセキュリティリスクです。`IObjectFactory` はホワイトリストモデルを強制します：

- **登録済みファクトリ**を持つ型のみインスタンス化可能
- ファクトリは `OnLoad` で明示的に登録され、ホストが完全に制御
- `RegisterAutoFactory` はコンストラクタを自動分析する便利メソッドだが、登録ゲートは必須

```
❌ Activator.CreateInstance(typeof(SomeType))     → セキュリティリスク
✅ factory.CreateInstance(typeof(SomeType))         → 登録済み型のみ
✅ factory.CreateInstance<SomeType>()               → ジェネリック便利メソッド
```

## RegisterAutoFactory の仕組み

`RegisterAutoFactory` は型のコンストラクタを検査し、ファクトリデリゲートを生成します：

1. **引数なし** → パラメータなしコンストラクタを呼び出し
2. **引数あり** → コンストラクタパラメータに型でマッチ、不一致時はパラメータなしにフォールバック
3. **抽象/インターフェース型** → 警告付きで拒否

## 登録と生成の流れ

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ ServiceLocator から IObjectFactory を取得                 │
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

## このデモ

> **⚠️ 注意：** `SimpleService` と `ConfiguredService` は**このデモのためだけに定義されたカスタム型**です。システムのサービスインターフェースとは関係ありません。

| クラス | 役割 |
|--------|------|
| `SimpleService` | デモ型、パラメータなしコンストラクタ |
| `ConfiguredService` | デモ型、パラメータ付きコンストラクタ `(string name)` |
| `ObjectFactoryUsagePlugin` | `IPlugin` 実装 — ファクトリの登録とインスタンス生成 |

## セキュリティ上の注意

`IObjectFactory` は制御アクセスセキュリティモデルの一部です。プラグインは `Activator.CreateInstance` を使用してオブジェクトを作成しては**なりません**。ファクトリを登録し `CreateInstance` を使用してください。詳細は[セキュリティドキュメント](../../docs/ja-JP/security.md)を参照してください。
