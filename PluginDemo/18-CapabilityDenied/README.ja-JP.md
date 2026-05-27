# PluginDemo-18: 機能拒否 — 宣言不可能な機能のアンチパターン

## 概要

このプラグインは**アンチパターン**であり、機能を宣言しても宣言不可能な機能の禁止を回避できないことを示します。`[PluginCapability(Capability.Network)]` を宣言しても、P/Invoke、Unsafe、Reflection.Emit、レジストリアクセスは**常に**ブロックされます。

## 宣言可能 vs. 宣言不可能な機能

### ✅ 宣言可能（Capability 列挙値が存在）

| 機能 | 免除内容 |
|------|---------|
| `Capability.Network` | System.Net.* 名前空間と型レベル禁止 |
| `Capability.FileIO` | System.IO 名前空間（ホワイトリスト以降） |
| `Capability.Process` | System.Diagnostics 下の Process* 型 |
| `Capability.AI` | IAIService 注入を有効化（TypeRef 免除なし） |

### ❌ 宣言不可能（Capability 列挙値が存在しない）

| カテゴリ | ブロックされる型 | 宣言不可能な理由 |
|---------|---------------|----------------|
| P/Invoke | `DllImportAttribute`、`Marshal`、`NativeMemory`、`NativeLibrary` | 実行時に任意のネイティブコードを監査できない |
| Unsafe コード | `UnverifiableCodeAttribute`、`System.Runtime.CompilerServices.Unsafe` | CLR の型安全性と境界チェックをバイパス |
| IL エミッション | `System.Reflection.Emit.*` | 実行時に任意の IL を生成可能 |
| アセンブリ読み込み | `System.Runtime.Loader`、`Assembly.Load*` | スキャンされていない DLL を読み込み、セキュリティスキャンを回避可能 |
| レジストリ | `Microsoft.Win32.*` | プラグインサンドボックス外の OS レベルのシステムアクセス |
| 動的コンパイル | `Microsoft.CodeAnalysis.*` | 任意のコードをコンパイルして実行可能 |
| 危険なリフレクション | `Type.GetType(string)`、`Activator.CreateInstance` | 文字列で禁止された型をインスタンス化可能 |

## なぜこれらの機能は宣言不可能か

根本的な理由：**実行時に安全に監査できない。**

1. **P/Invoke**：ネイティブコードが呼び出されると、CLR は何が起こるかを追跡できない — 安全性の保証がない
2. **Unsafe**：プラグインセキュリティモデルが依存する型安全システムをバイパスする
3. **Reflection.Emit**：PluginLoader がスキャンしていない新しい IL を実行時に生成可能
4. **AssemblyLoadContext**：セキュリティスキャンされていない DLL を読み込み可能
5. **Registry**：プラグインサンドボックス外の OS レベル設定へのアクセスを提供

## PluginLoader の「宣言無効」処理

PluginLoader が機能宣言に遭遇した場合：

1. CustomAttribute blob から int32 列挙値を読み取る
2. `Enum.IsDefined(typeof(Capability), value)` をチェック
3. 値が定義済みの Capability メンバーでない場合 → **黙って無視**
4. 値が定義済みの場合 → 免除ルールを適用
5. **宣言不可能チェックは常に強制** — 宣言された機能に関係なく

これにより、まだ存在しない「将来の」機能を宣言するプラグインを防止します。

## 13-CapabilityNetwork との比較

| 側面 | 13-CapabilityNetwork（正例） | 18-CapabilityDenied（アンチパターン） |
|------|---------------------------|-----------------------------------|
| 宣言 | `[PluginCapability(Capability.Network)]` | `[PluginCapability(Capability.Network)]` |
| HttpClient 使用 | ✅ 免除 | ✅ 免除 |
| DllImport 使用 | N/A | ❌ 常にブロック |
| Unsafe 使用 | N/A | ❌ 常にブロック |
| 読み込み結果 | ✅ 読み込み成功 | ❌ 拒否 |

## ファイル

- `Plugin.cs` — 宣言不可能な機能のアンチパターンを示すデモプラグイン
- `README.md` — このファイル（英語）
- `README.zh-CN.md` — 簡体字中国語
- 翻訳：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 関連例

- **13-CapabilityNetwork**: Capability.Network の正例
- **11-ForbiddenPInvoke**: P/Invoke アンチパターン（機能宣言で解決不可）
- **10-ForbiddenReflection**: Reflection アンチパターン（機能宣言で解決不可）
