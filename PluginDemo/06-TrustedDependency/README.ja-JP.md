# 信頼済み依存関係デモ

`Newtonsoft.Json`（内部で大量のリフレクションを使用するライブラリ）を信頼済みアセンブリとして使用する方法を示します。PluginLoaderのセキュリティスキャナーは信頼済みアセンブリを完全にスキップし、プラグインが違反をトリガーせずにそれらを参照できるようにします。

## TrustedAssemblies ホワイトリストメカニズム

`PluginLoader`は**デフォルトで信頼される**オープンソースライブラリの静的ホワイトリストを維持します：

```csharp
private static readonly HashSet<string> TrustedAssemblies = new(StringComparer.Ordinal)
{
    // シリアライズ
    "Google.Protobuf",
    "protobuf-net",
    "Newtonsoft.Json",        // ← このデモで使用
    "MessagePack",
    "YamlDotNet",

    // ログ
    "Serilog", "NLog",

    // Microsoft.Extensions.*
    "Microsoft.Extensions.Logging.Abstractions",
    "Microsoft.Extensions.DependencyInjection.Abstractions",
    // ...

    // データアクセス/マッピング
    "Dapper", "AutoMapper",

    // バリデーションとメッセージディスパッチ
    "FluentValidation", "MediatR",
};
```

### 承認基準

ライブラリは**3つすべて**の基準を満たす場合に`TrustedAssemblies`に追加できます：

| # | 基準 | 理由 |
|---|------|------|
| 1 | 広く使用されているオープンソースプロジェクト（MIT / Apache 2.0 / BSD） | 公開監査可能なコード |
| 2 | ソースコードが公開されている | コミュニティの監視により悪意のある動作がないことを保証 |
| 3 | 信頼できるベンダー/コミュニティが管理するNuGetパッケージ | サプライチェーンの整合性 |

### 識別基盤

スキャナーはPEメタデータの`AssemblyDefinition.Name`で信頼済みアセンブリを識別します — **DLLファイル名ではありません**。これにより、攻撃者が悪意のあるDLLを`Newtonsoft.Json.dll`にリネームしてチェックを回避することを防ぎます。

## CollectTrustedTypeRefs — 推移的免除

PluginLoaderがプラグインディレクトリを読み込む際、2段階のスキャンを実行します：

```
フェーズ1: CollectTrustedTypeRefs(pluginDir)
├── プラグインディレクトリ内のすべての*.dllファイルを列挙
├── 各DLLについて：PEメタデータを読み取り → AssemblyDefinition.Nameを確認
├── 名前 ∈ TrustedAssembliesの場合：
│   └── すべてのTypeReferenceエントリを収集 → (namespace, typeName)ペア
└── 戻り値：HashSet<(string Namespace, string Name)>

フェーズ2: ScanForbiddenReferences(pluginMainDll, trustedTypeRefs)
├── レイヤー0：ホワイトリスト早期退出（メインDLL自体が信頼済み → パス）
├── レイヤー0.5：推移的免除（trustedTypeRefsセット内のTypeRefをスキップ）
├── レイヤー1：TypeRefテーブルスキャン
├── レイヤー2：ExportedTypeテーブルスキャン
├── レイヤー3：MemberRefテーブルスキャン（危険なメソッド）
├── レイヤー4：アンセーフコードマーカー + P/Invoke
└── レイヤー5：#USユーザー文字列ヒープスキャン
```

### 推移的免除が重要な理由

Newtonsoft.Jsonは内部で`System.Reflection.MemberInfo`、`System.IO.TextReader`などの型を参照しています。プラグインがNewtonsoft.Jsonを参照すると、コンパイラがこれらの推移的TypeRefを**あなたの**プラグインDLLに埋め込む場合があります。推移的免除がなければ、プラグインは`System.IO.TextReader`を参照したとして違反マークされます — 直接使用していないにもかかわらず。

`CollectTrustedTypeRefs`は信頼済みDLLからすべてのTypeRefを事前に収集し、メインスキャン中に「既知の安全」としてマークすることでこの問題を解決します。

## 新しい信頼済み依存関係の追加方法

ホワイトリストに新しいライブラリを追加するには：

1. 上記の3つの承認基準を満たしていることを確認
2. `PluginLoader.cs`の`TrustedAssemblies` HashSetに1行追加：
   ```csharp
   "YourLibraryName",  // 信頼の理由を簡潔に記述
   ```
3. ライブラリDLLをプラグインディレクトリ（プラグインメインDLLの隣）に配置
4. スキャナーが自動的にTypeRefを収集し免除

> **⚠️ 重要：** ライブラリを`TrustedAssemblies`に追加すると、スキャナーはその内部コードを**チェックしません**。完全に信頼するライブラリのみを追加してください。

## このデモ

このプラグインは`PluginCapability`宣言なしでNewtonsoft.Jsonを使用します：

| 機能 | Newtonsoft.Jsonの内部動作 | 動作する理由 |
|------|---------------------------|-------------|
| `JsonConvert.SerializeObject` | リフレクションでプロパティを列挙 | Newtonsoft.Json DLLがレイヤー0ホワイトリストを通過 |
| `JsonConvert.DeserializeObject<T>` | `Activator.CreateInstance`を呼び出し、リフレクションでプロパティを設定 | 推移的TypeRefがレイヤー0.5で免除 |
| `JObject` / `JArray`操作 | `System.Linq.Expressions`、動的ディスパッチを使用 | すべての内部参照が`CollectTrustedTypeRefs`で収集 |

### PluginCapabilityとの主な違い

| メカニズム | スコープ | ユースケース |
|-----------|---------|------------|
| `TrustedAssemblies` | **ライブラリ**全体（とその推移的参照）をスキャンから免除 | 既知のオープンソース依存関係 |
| `PluginCapability` | **プラグインコード**を特定のネームスペース禁止から免除 | プラグインがSystem.Net/IO/Processへの直接アクセスが必要 |

信頼済み依存関係のみを使用するプラグインは`PluginCapability`宣言が**不要**です。スキャナーがすべてを自動的に処理します。

## セキュリティ注意事項

信頼済みアセンブリは監査可能なオープンソースプロジェクトであるため、セキュリティスキャンから免除されます。ただし、**プラグインコード**は引き続き完全にスキャンされます。プラグインが`System.IO.File`や`System.Net.Http.HttpClient`を直接参照している場合、対応する`PluginCapability`を宣言しない限りブロックされます。[セキュリティドキュメント](../../docs/ja-JP/security.md)を参照してください。
