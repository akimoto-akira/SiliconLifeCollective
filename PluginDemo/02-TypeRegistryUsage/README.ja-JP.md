# ITypeRegistry 登録とクエリの例

`ITypeRegistry` の登録と検索機能をデモ：`OnLoad` でカスタム型を登録し、`OnStart` で `FindSubtypesOf` を使って発見します。

## ITypeRegistry インターフェース概要

`ITypeRegistry` は `AppDomain.CurrentDomain.GetAssemblies()` リフレクションスキャンに代わるものです。プラグインは `IPlugin.OnLoad` で公開する型を明示的に登録し、ランタイムはレジストリからのみ型を検索します。

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

### メソッド一覧

| メソッド | 説明 |
|----------|------|
| `RegisterType(Type)` | 単一の型を登録 |
| `RegisterTypes(IEnumerable<Type>)` | 複数の型を一括登録 |
| `RegisterFromAssembly(Assembly, Type)` | 指定アセンブリから `baseType` の非抽象サブタイプをすべて登録 |
| `FindType(string)` | 完全名で型を検索；ジェネリック型名の解決に対応 |
| `FindSubtypesOf(Type)` | 指定基底型の非抽象サブタイプをすべて検索 |
| `FindImplementationsOf(Type)` | 指定インターフェースを実装する非抽象型をすべて検索 |

## 登録とクエリの流れ

```
┌──────────────────────────────────────────────────────────────┐
│  OnLoad                                                      │
│  ├─ ServiceLocator から ITypeRegistry を取得                  │
│  ├─ RegisterType(typeof(GreetingTool))                       │
│  ├─ RegisterType(typeof(FarewellTool))                       │
│  └─ RegisterType(typeof(StatusTool))                         │
│                                                              │
│  代替案：RegisterFromAssembly                                 │
│  └─ RegisterFromAssembly(assembly, typeof(DemoTool))         │
│     → DemoTool サブタイプを一括登録                           │
│                                                              │
│  OnStart                                                     │
│  ├─ FindSubtypesOf(typeof(DemoTool))                         │
│  └─ 結果を反復 → GreetingTool, FarewellTool, StatusTool      │
└──────────────────────────────────────────────────────────────┘
```

## RegisterFromAssembly の使い方

`RegisterFromAssembly` はアセンブリをスキャンし、指定基底型の非抽象サブタイプをすべて登録します：

```csharp
_registry.RegisterFromAssembly(
    typeof(TypeRegistryUsagePlugin).Assembly,  // スキャンするアセンブリ
    typeof(DemoTool)                            // DemoTool サブタイプのみ登録
);
```

これは個別に `RegisterType` を呼び出すのと同等ですが、共通基底クラスを持つ型が多数ある場合に簡潔になります。

## このデモ

> **⚠️ 重要：** `DemoTool` は `ITypeRegistry` の登録とクエリをデモするためだけに定義された**カスタム型**です。システムの AI ツール登録に使用される `ITool` インターフェース（`SiliconLife.Collective.ITool`）とは**一切関係ありません**。名前に「Tool」が含まれるのは偶然であり、任意のカスタムクラス階層で同じように機能します。

| クラス | 役割 |
|--------|------|
| `DemoTool` | カスタム抽象基底クラス — 登録のアンカー（`ITool` とは無関係） |
| `GreetingTool` | `OnLoad` で登録される具象サブタイプ |
| `FarewellTool` | `OnLoad` で登録される具象サブタイプ |
| `StatusTool` | `OnLoad` で登録される具象サブタイプ |
| `TypeRegistryUsagePlugin` | `IPlugin` 実装 — 型の登録とクエリ |

## セキュリティ上の注意

`ITypeRegistry` は制御アクセスセキュリティモデルの一部です。プラグインは `AppDomain.CurrentDomain.GetAssemblies()` や `Assembly.GetTypes()` を使用して型を発見しては**なりません**。代わりに `ITypeRegistry` を使用してください。詳細は[セキュリティドキュメント](../../docs/ja-JP/security.md)を参照してください。
