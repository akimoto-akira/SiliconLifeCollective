# PluginDemo-20: SpeedyPack — 構造化データストレージ

## 概要

このプラグインは、`SpeedyPack` を使用して構造化データストレージを実装する方法を示します。**機能宣言は一切不要**です。SpeedyPack はプラグインのデータ永続化に**推奨される**方法です。

## なぜ SpeedyPack？

| 機能 | SpeedyPack | PermissionedStreamFactory | Capability.FileIO + System.IO |
|------|-----------|--------------------------|------------------------------|
| 必要な機能宣言 | **なし** | なし | `Capability.FileIO` |
| キャッシュ | ✅ 内蔵 | ❌ | ❌ |
| WAL（クラッシュリカバリ） | ✅ | ❌ | ❌ |
| トランザクション | ✅ `IPackTransaction` | ❌ | ❌ |
| スレッドセーフ | ✅ | ❌ | ❌ |
| 構造化シリアライズ | ✅ `Read<T>` | ❌ 生バイト | ❌ 手動 |
| 監査トレイル | ✅ 自動 | ✅ 自動 | ❌ 手動 |

## 基本 CRUD

```csharp
// SpeedyPack データファイルを開く
using var pack = SpeedyPack.Open("mydata.spk");

// キーと値のペアを書き込み
pack.Write("user:name", "Alice");
pack.Write("user:age", 30);

// 値を読み取り（型付き）
string name = pack.Read<string>("user:name");  // "Alice"
int age = pack.Read<int>("user:age");           // 30

// キーを削除
pack.Delete("user:age");

// 存在確認
bool exists = pack.Contains("user:name");  // true
```

## 型付きアクセスと構造化オブジェクト

```csharp
public class UserProfile
{
    public string Name { get; set; }
    public int Level { get; set; }
    public string[] Tags { get; set; }
}

// 構造化オブジェクトを書き込み
var profile = new UserProfile { Name = "Bob", Level = 42, Tags = new[] { "admin" } };
pack.Write("profile:bob", profile);

// 型付きオブジェクトを読み取り
var loaded = pack.Read<UserProfile>("profile:bob");
Console.WriteLine($"{loaded.Name}, Level {loaded.Level}");
```

## トランザクション

```csharp
using (var tx = pack.BeginTransaction())
{
    try
    {
        tx.Write("account:a", 1000);
        tx.Write("account:b", 500);
        tx.Commit();   // 原子的 — 両方の書き込みが永続化されるか、どちらもされない
    }
    catch
    {
        tx.Rollback();  // このトランザクションのすべての書き込みを破棄
    }
}
```

### IPackTransaction メソッド

| メソッド | 説明 |
|---------|------|
| `Write(key, value)` | 書き込み操作をキューに追加 |
| `Delete(key)` | 削除操作をキューに追加 |
| `Commit()` | キュー内のすべての操作を原子的に適用 |
| `Rollback()` | キュー内のすべての操作を破棄 |

## SpeedyPackOptions 設定

```csharp
var options = new SpeedyPackOptions
{
    MaxCacheSize = 1024 * 1024,              // 1 MB キャッシュ
    AutoFlushInterval = TimeSpan.FromSeconds(30),
    CompressionLevel = CompressionLevel.Optimal
};
using var pack = SpeedyPack.Open("data.spk", options);
```

### SpeedyPackOptions プロパティ

| プロパティ | 型 | デフォルト | 説明 |
|-----------|---|----------|------|
| `MaxCacheSize` | `long` | 64 MB | 最大メモリキャッシュサイズ |
| `AutoFlushInterval` | `TimeSpan` | 10 秒 | キャッシュをディスクにフラッシュする間隔 |
| `CompressionLevel` | `CompressionLevel` | `Fastest` | 保存データの圧縮レベル |

## セキュリティ上の注意

SpeedyPack は機能宣言が**不要**です。安全な制御されたデータストレージのエントリポイントです：
- すべてのパスをワークスペース境界内で検証
- すべての読み取り/書き込み操作の完全な監査トレイルを提供
- ディレクトリトラバーサル攻撃を防止
- リソースライフサイクルを自動管理

## ファイル

- `Plugin.cs` — SpeedyPack デモプラグイン
- `README.md` — このファイル（英語）
- `README.zh-CN.md` — 簡体字中国語
- 翻訳：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 関連例

- **04-SafeSystemIO**: 許可されたインメモリ System.IO 型（宣言不要）
- **07-ForbiddenFileIO**: ブロックされたファイル操作のアンチパターン
- **14-CapabilityFileIO**: SpeedyPack では不十分な場合
