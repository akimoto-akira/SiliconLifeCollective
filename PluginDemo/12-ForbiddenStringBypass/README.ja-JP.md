# PluginDemo-12: 禁止された文字列リフレクションバイパスのアンチパターン

## 概要

本プラグインは、SiliconLife プラグインシステムにおける**禁止された**文字列ベースのリフレクションバイパス試行を実演します。文字列連結、補間、エンコード、その他の難読化技術がなぜ PluginLoader の #US（ユーザー文字列）ヒープスキャン——**最後の防衛線**——を**バイパスできない**かを示します。

## #US ヒープとは？

.NET PE（Portable Executable）メタデータにおいて、**#US（User String）ヒープ**は `ldstr` IL命令で使用されるすべての文字列リテラルオペランドを格納します。C# コードで文字列リテラルを記述するたびに、コンパイラはそれをこのヒープに格納します。

```
C# ソース:   string s = "System.IO.File";
    ↓ コンパイル
IL コード:   ldstr "System.IO.File"    ← #US ヒープ内のトークンを参照
    ↓ PluginLoader スキャン
#US ヒープ:  [..., "System.IO.File", ...]  ← プレフィックスマッチで検出！
```

PluginLoader の `ScanUserStrings()` メソッドは #US ヒープの**すべてのエントリ**を反復処理し、禁止プレフィックスで始まる文字列がないか検査します。

## 禁止文字列プレフィックス

#US ヒープで以下のプレフィックスが検出されると `[ILString]` 違反がトリガーされます：

| プレフィックス | カテゴリ |
|--------------|---------|
| `System.IO.` | ファイルシステム型 |
| `System.Net.Http` | HTTP クライアント |
| `System.Net.WebSockets` | WebSocket |
| `System.Net.Sockets` | 生ソケット |
| `System.Net.Mail` | SMTP |
| `System.Net.NetworkInformation` | ネットワーク調査 |
| `System.Net.Security` | SslStream |
| `System.Diagnostics.Process` | プロセス/コマンドライン |
| `Microsoft.CodeAnalysis` | Roslyn コンパイラ |
| `System.Reflection.Emit` | IL 発行 |
| `System.Runtime.Loader` | AssemblyLoadContext |
| `System.CodeDom.Compiler` | レガシー CodeDom |
| `Microsoft.Win32` | Windows レジストリ |

## 実演された違反

### 違反 1：直接的な型名文字列

```csharp
// ❌ 禁止 — 完全な文字列が #US ヒープに格納される
Type? fileType = Type.GetType("System.IO.File, System.Runtime");
```

**違反**：`[ILString] "System.IO.File, System.Runtime" matches forbidden prefix "System.IO."`

### 違反 2：文字列連結（コンパイル時）

```csharp
// ❌ 禁止 — コンパイラが const+const を1つの #US エントリに畳み込む
const string ns = "System.Net.Http";
const string typeName = ".HttpClient";
const string assembly = ", System.Net.Http";
Type? type = Type.GetType(ns + typeName + assembly);
// 各パーツと畳み込み結果の両方が #US ヒープに！
```

**違反**：`[ILString] "System.Net.Http.HttpClient, System.Net.Http" matches forbidden prefix "System.Net.Http"`

### 違反 3：文字列補間

```csharp
// ❌ 禁止 — リテラル部分が #US ヒープに格納される
string className = "FileStream";
string fullName = $"System.IO.{className}, System.Runtime";
// "System.IO." だけでプレフィックスに一致！
```

**違反**：`[ILString] "System.IO." matches forbidden prefix "System.IO."`

### 違反 4：Const フィールド

```csharp
// ❌ 禁止 — const 値は使用箇所でインライン化 → #US ヒープに出現
private const string ProcessType = "System.Diagnostics.Process";
private const string AssemblyName = ", System.Runtime";
Type? type = Type.GetType(ProcessType + AssemblyName);
```

**違反**：`[ILString] "System.Diagnostics.Process" matches forbidden prefix "System.Diagnostics.Process"`

### 違反 5：部分文字列フラグメント

```csharp
// ❌ 禁止 — 各パーツは独立した ldstr として個別にスキャンされる
string part1 = "System.Reflection.Emit";
string part2 = ".AssemblyBuilder";
string fullType = part1 + part2;
// "System.Reflection.Emit" だけでプレフィックスに一致！
```

**違反**：`[ILString] "System.Reflection.Emit" matches forbidden prefix "System.Reflection.Emit"`

### 違反 6：複数の禁止ターゲット

```csharp
// ❌ 禁止 — アセンブリ内のすべての文字列がスキャンされる
string tcp = "System.Net.Sockets.TcpClient";
string registry = "Microsoft.Win32.Registry";
string loader = "System.Runtime.Loader.AssemblyLoadContext";
```

**複数の違反** — スキャナーは最初のマッチで停止しません。

## なぜ難読化技術はすべて失敗するのか

| 技術 | なぜ失敗するか |
|------|--------------|
| Const 連結 | コンパイラが単一の #US エントリに畳み込む |
| 文字列補間 | リテラル部分が #US ヒープに格納される |
| Const フィールド | 値が使用箇所でインライン化 → #US に出現 |
| 変数に分割 | 各 `ldstr` オペランドが独立してスキャンされる |
| Base64 エンコード | デコードにランタイムメソッドが必要だが `Type.GetType` が MemberRef でブロック |
| 文字配列構築 | `ldstr` を生成しないが `Type.GetType` は MemberRef でブロックされたまま |
| XOR 暗号化 | 暗号化文字列は #US で読めないが、復号 + `Type.GetType` = MemberRef ブロック |
| 文字列反転 | 反転リテラルがプレフィックスに一致する可能性あり、reverse + GetType = MemberRef ブロック |

**核心的洞察**：#US スキャンは**文字列**をブロック。MemberRef スキャンは**メソッド**をブロック。型を動的にロードするには両方が必要。PluginLoader は両方を独立してブロックします。

## 完全な防御チェーン

PluginLoader の5ステップスキャンが突破不可能な防御を構築します：

| ステップ | メカニズム | 検出内容 |
|---------|-----------|---------|
| 1 | TypeRef テーブル | 禁止型への直接参照 |
| 2 | ExportedType テーブル | 禁止名前空間の転送型 |
| 3 | MemberRef テーブル | `Type.GetType`、`Assembly.Load`、`Activator.CreateInstance` の呼び出し |
| 4 | Unsafe マーカー | `[DllImport]`、unsafe ブロック、PinvokeImpl フラグ |
| **5** | **#US ヒープスキャン** | **禁止プレフィックスに一致する文字列定数（本デモ）** |

ステップ5が**最後の防衛線**である理由：
- ステップ1-4は**能動的なコード**（型、メソッド、属性）を検出
- ステップ5は**受動的なデータ**（バイパスに使用される可能性のある文字列）を検出
- MemberRef スキャンが未知のメソッドを見逃しても、禁止型名文字列はアセンブリ内に存在する必要がある → #US スキャンが検出

## トレードオフ

#US ヒープスキャンは**軽微な誤検知**を生じる可能性があります：
- ログメッセージ：`logger.Info("Failed to connect to System.Net.Http endpoint")` → フラグ付き
- ドキュメント文字列：XML ドキュメントにコンパイルされるコメントは #US ヒープに含まれない（安全）
- `nameof()` 式：プレフィックスに一致しないコンパイル時文字列は安全

**誤検知の解決策**：禁止プレフィックスを避けるよう文字列リテラルを書き換える。

## 他の例との比較

| 例 | フォーカス | 関係 |
|----|-----------|------|
| **10-ForbiddenReflection** | 禁止リフレクションメソッド（MemberRef スキャン） | メソッドを検出 |
| **11-ForbiddenPInvoke** | 禁止 P/Invoke と unsafe コード | 異なる脅威ベクター |
| **12-ForbiddenStringBypass** | 文字列ベースのバイパス試行（本例） | 文字列を検出 |
| **02-TypeRegistryUsage** | 安全な型発見 | 正しい代替手段 |
| **03-ObjectFactoryUsage** | 安全なオブジェクト作成 | 正しい代替手段 |

## ベストプラクティス

1. **すべての文字列で禁止プレフィックスを避ける** — ログメッセージや IL にコンパイルされるコメントを含む
2. **ITypeRegistry/IObjectFactory を使用** — 文字列リフレクションの安全な代替手段
3. **文字列の分割は無意味** — 各フラグメントが独立してスキャンされる
4. **ランタイム構築（char[]）は #US をバイパス** — だが Type.GetType の MemberRef スキャンでブロック
5. **多層防御は有効** — 1層をバイパスしても他の層が検出する

## ファイル

- `Plugin.cs` - アンチパターンデモプラグイン
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 本ファイル（日本語）
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Português
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## 関連例

- **10-ForbiddenReflection**：禁止リフレクションメソッド（MemberRef スキャン）
- **11-ForbiddenPInvoke**：禁止 P/Invoke と unsafe コード
- **02-TypeRegistryUsage**：ITypeRegistry の正しい使い方
- **03-ObjectFactoryUsage**：IObjectFactory の正しい使い方
