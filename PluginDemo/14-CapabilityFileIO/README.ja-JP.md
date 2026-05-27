# PluginDemo-14: Capability.FileIO — 宣言型ファイル I/O 権限

## 概要

このプラグインは、`[PluginCapability(Capability.FileIO)]` を使用してプラグインが直接ファイルシステムアクセスを必要とすることを宣言する方法を示します。この機能を宣言することで、`SystemIOAllowedTypes` ホワイトリストの制限を超えて、すべての `System.IO` 型にアクセスできるようになります。

## PluginCapability 宣言構文

```csharp
[PluginCapability(Capability.FileIO, Reason = "Direct log file access for audit trail")]
public class CapabilityFileIOPlugin : IPlugin { ... }
```

## Capability.FileIO の動作方式

1. **デフォルト状態**：`System.IO` 名前空間は全面的に禁止され、`SystemIOAllowedTypes` ホワイトリストの型のみ使用可能（MemoryStream、BinaryReader、GZipStream など）
2. **宣言後**：`System.IO` 名前空間の禁止が解除される — File、FileStream、Directory、StreamReader(string) などすべてアクセス可能
3. **ILString 免除**：`"System.IO."` で始まる文字列定数はフラグされない
4. **宣言不可の制限**：P/Invoke、Unsafe、Reflection.Emit などは引き続きブロックされる

## Capability.FileIO 免除範囲

### TypeRef 免除

すべての `System.IO` 型が免除されます：

| カテゴリ | 免除される型 |
|---------|------------|
| ファイル操作 | `File`、`FileInfo` |
| ディレクトリ操作 | `Directory`、`DirectoryInfo` |
| ストリーム型 | `FileStream`、`StreamReader(path)`、`StreamWriter(path)` |
| ファイルシステム | `FileSystemWatcher`、`DriveInfo`、`Path` |

### ILString 免除

- `"System.IO."` で始まる文字列はフラグされない

### 引き続き禁止される機能

| カテゴリ | 引き続きブロック |
|---------|---------------|
| P/Invoke | `DllImportAttribute`、`Marshal`、`NativeMemory` |
| アンセーフコード | `UnverifiableCodeAttribute`、`Unsafe` |
| IL エミッション | `System.Reflection.Emit.*` |
| アセンブリ読み込み | `System.Runtime.Loader`、`Assembly.Load*` |
| レジストリ | `Microsoft.Win32.*` |

## 他の例との比較

| 例 | 宣言 | ファイルアクセス | 備考 |
|--|------|---------------|------|
| **04-SafeSystemIO** | なし | MemoryStream、BinaryReader、GZipStream のみ | ホワイトリスト型のみ使用 |
| **07-ForbiddenFileIO** | なし | ❌ 拒否される | アンチパターンの例 |
| **14-CapabilityFileIO** | `[PluginCapability(Capability.FileIO)]` | ✅ 完全な System.IO アクセス | この例 |
| **20-SpeedyPack** | なし | SpeedyPack API 経由（Capability 不要） | 推奨されるデータストレージ |

## ファイルアクセスの優先順位

1. **SpeedyPack** — Capability 宣言不要。内蔵キャッシュ、WAL、トランザクション。**構造化データストレージに推奨。**
2. **PermissionedStreamFactory** — Capability 宣言不要。監査付きアクセス、パス検証とアクセス制御付き。
3. **Capability.FileIO + 直接 System.IO** — 上記の選択肢では不十分な場合のみ使用。

## なぜ PermissionedStreamFactory / SpeedyPack を推奨するのか？

`Capability.FileIO` があっても、制御されたエントリポイントの使用が推奨される理由：

1. **監査証跡**：すべてのアクセスが記録され追跡可能
2. **パス検証**：ディレクトリトラバーサル攻撃を防止（`../`）
3. **アクセス制御**：ワークスペース境界の強制実行
4. **リソース追跡**：ストリームリークとリソース枯渇を防止
5. **コンプライアンス**：制御されたアクセスパターンによりセキュリティレビューが容易

## セキュリティベストプラクティス

1. **本当に必要な場合のみ FileIO を宣言する**：SpeedyPack や PermissionedStreamFactory で代用できないか？
2. **明確な Reason を提供する**："Direct log file access for audit trail" は "file access" より適切
3. **パスを自分で検証する**：Capability.FileIO があっても、使用前にすべてのファイルパスを検証する
4. **using ステートメントを使用する**：必ず FileStream/StreamReader/StreamWriter を破棄する
5. **最小権限の原則**：プラグインが実際に必要とする機能のみを宣言する

## ファイル

- `Plugin.cs` — Capability.FileIO を宣言するデモプラグイン
- `README.md` — このファイル（英語）
- `README.zh-CN.md` — 簡体字中国語
- 翻訳：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 関連例

- **04-SafeSystemIO**：許可されるインメモリ System.IO 型（宣言不要）
- **07-ForbiddenFileIO**：ブロックされるファイル操作のアンチパターン
- **20-SpeedyPack**：Capability 宣言不要の推奨データストレージ
- **18-CapabilityDenied**：宣言不可機能のアンチパターン
