# セーフ System.IO デモ

`SystemIOAllowedTypes` ホワイトリストの System.IO 型をデモ：`MemoryStream`、`BinaryReader`/`BinaryWriter`、`GZipStream`。`FileStream` に `PermissionedStreamFactory` が必要な理由を説明。

## SystemIOAllowedTypes ホワイトリスト

プラグインランタイムはデフォルトで `System.IO` 名前空間をブロックしますが、**直接ファイル I/O を実行しない**型は除外されます：

| カテゴリ | 許可される型 | 安全な理由 |
|----------|-------------|-----------|
| ストリーム抽象 | `Stream` | 抽象基底クラス、自体は I/O なし |
| メモリストリーム | `MemoryStream` | 純粋なメモリ操作 |
| 圧縮ストリーム | `GZipStream`、`DeflateStream`、`ZLibStream` | 別のストリームをラップ、ファイルを開かない |
| バイナリラッパー | `BinaryReader`、`BinaryWriter` | 任意のストリームをラップ、ファイルを開かない |
| 列挙型 | `SeekOrigin`、`FileMode`、`FileAccess`、`FileShare`、`CompressionMode`、`CompressionLevel` | 値型のみ |
| 例外 | `IOException`、`InvalidDataException`、`EndOfStreamException` | エラー型のみ |

### ホワイトリストにない型

これらの型は**直接ファイルシステムにアクセス**するため、プラグインコードで**ブロック**されます：

| ブロックされる型 | ブロック理由 | 安全な代替 |
|----------------|-------------|-----------|
| `FileStream` | 直接ファイルを開く | `PermissionedStreamFactory.CreateReadStream()` / `CreateWriteStream()` |
| `File` | 静的ファイル操作 | `PermissionedStreamFactory` + `SafePath` |
| `Directory` | 静的ディレクトリ操作 | `SafePath`（権限チェック付き） |
| `FileInfo` | ファイルパスをラップ | `SafePath` |
| `DirectoryInfo` | ディレクトリパスをラップ | `SafePath` |
| `StreamReader` | 直接ファイルを開く | `PermissionedStreamFactory` + `PermissionedStream` をラップ |
| `StreamWriter` | 直接ファイルを開く | `PermissionedStreamFactory` + `PermissionedStream` をラップ |

## なぜ FileStream に PermissionedStreamFactory が必要か

`FileStream` は直接ディスクファイルを開きます — プラグインシステムで重大なセキュリティリスクです。`PermissionedStreamFactory` は以下を強制します：

1. **権限チェック** — 呼び出し元の `PermissionManager` がパスの `FileAccess` 権限を付与する必要がある
2. **監査ログ** — ファイルを開くたびに呼び出し元の being ID が記録される
3. **パス検証** — 空/無効パスは I/O の前に拒否される

```
❌ new FileStream("path", FileMode.Open)           → TypeRef スキャナーでブロック
✅ PermissionedStreamFactory.CreateReadStream(id, "path")  → 権限チェック通過
✅ PermissionedStreamFactory.CreateWriteStream(id, "path") → 権限チェック通過
```

## デモパイプライン

このデモはホワイトリスト型のみを使用して完全なメモリ内データパイプラインを構築します：

```
┌─────────────────────────────────────────────────────────────────┐
│  デモ 1: MemoryStream                                           │
│  └─ バイト書き込み → バイト読み取り → 文字列デコード              │
│                                                                  │
│  デモ 2: 圧縮パイプライン                                        │
│  └─ string → UTF8 → MemoryStream                                │
│     → GZipStream(圧縮) → MemoryStream(圧縮済み)                  │
│     → GZipStream(展開) → MemoryStream(元のデータ)                │
│     → UTF8 → string (往復確認)                                   │
│                                                                  │
│  デモ 3: BinaryReader/Writer                                     │
│  └─ Write(int, double, string) → MemoryStream                   │
│     → Read(int, double, string) → 往復確認                       │
└─────────────────────────────────────────────────────────────────┘
```

## このデモ

> **⚠️ 注意：** このデモは `SystemIOAllowedTypes` ホワイトリスト型**のみ**を使用し、ファイル I/O は一切行いません。ファイルアクセスについては `PermissionedStreamFactory` API を参照してください。

| クラス | 役割 |
|--------|------|
| `SafeSystemIOPlugin` | `IPlugin` 実装 — セーフな System.IO 使用法をデモ |

## セキュリティノート

`System.IO` 名前空間はプラグイン TypeRef スキャナーでブロックされ、ホワイトリスト型のみ通過できます。実際のファイルアクセスには `PermissionedStreamFactory` を使用する必要があります。これは権限チェックと監査ログを実行します。詳細は[セキュリティドキュメント](../../docs/ja-JP/security.md)を参照してください。
