# 最小プラグインデモ

ハードコードされた値でプラグインライフサイクルを示す、最小の `IPlugin` 実装です。

## IPlugin インターフェース全貌

すべての SiliconLife プラグインは `SiliconLife.Collective` で定義された `IPlugin` インターフェースを実装する必要があります：

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### プロパティ概要

| メンバー | 型 | 説明 |
|----------|-----|------|
| `Id` | `string` | 一意識別子。バージョン間で安定している必要があります（例：`"com.siliconlife.demo.minimal"`） |
| `GetName(Language)` | `string` | 人間が読める表示名。`Language` 列挙型によるローカライズ |
| `Version` | `string` | セマンティックバージョン文字列（例：`"1.0.0"`） |
| `GetDescription(Language)` | `string` | プラグインの機能の短い説明 |
| `GetAuthor(Language)` | `string` | 作者または組織名 |

## ライフサイクル呼び出し順序

ホストは厳密な順序でライフサイクルメソッドを呼び出します：

```
OnLoad → OnStart → [実行中] → OnStop → OnUnload
```

| メソッド | 呼び出しタイミング | 典型的な用途 |
|----------|------------------|-------------|
| `OnLoad()` | プラグイン DLL がホストプロセスにロードされた時、1回だけ呼び出される | 設定の検証、型の登録、リソースの準備 |
| `OnStart()` | ホストが完全に起動し、すべてのプラグインがロードされた後 | 他のプラグインとの連携、バックグラウンドタスクの開始 |
| `OnStop()` | ホストがグレースフルシャットダウンする時 | リソースの解放、バッファのフラッシュ、状態の保存 |
| `OnUnload()` | プラグインがホストプロセスからアンロードされる時 | 最終クリーンアップ |

## このデモ

このプラグインはすべてのプロパティにハードコードされた値を返し、ライフサイクルメソッドは空です。プラグイン開発の最もシンプルな出発点です。

## セキュリティに関する注意

プラグインは分離された `AssemblyLoadContext` でロードされ、禁止された名前空間の参照（例：`System.IO`、`System.Net.Http`）がスキャンされます。詳細は[セキュリティドキュメント](../../docs/ja-JP/security.md)を参照してください。
