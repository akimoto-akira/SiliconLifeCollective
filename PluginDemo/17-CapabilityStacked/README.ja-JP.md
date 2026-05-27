# PluginDemo-17: 機能スタッキング — 複数の宣言型権限

## 概要

このプラグインは、単一のプラグインクラスに複数の `[PluginCapability]` 属性をスタックする方法を示します。`PluginCapabilityAttribute` は `AllowMultiple = true` なので、必要なだけ多くの機能を宣言できます。

## スタッキング構文

```csharp
[PluginCapability(Capability.Network, Reason = "API endpoint access for remote AI models")]
[PluginCapability(Capability.AI, Reason = "AI service provider for downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

## PluginLoader がスタックされた機能を処理する方法

1. PE メタデータの CustomAttribute テーブルから**すべての**宣言を読み取る
2. 宣言されたすべての機能の免除ルールを**マージ**する
3. 各宣言を独自の Reason フィールドで**独立して記録**する
4. スタッキングに関わらず宣言不可能な機能の禁止を**引き続き強制**する

## マージされた免除ルール

`Capability.Network` + `Capability.AI` をスタックする場合：

| ソース | 免除内容 |
|--------|---------|
| Capability.Network | System.Net.Http.*、System.Net.WebSockets.*、System.Net.Sockets.*、System.Net.Mail.*、System.Net.NetworkInformation.*、System.Net.Security.*、System.Net（型レベル禁止） |
| Capability.AI | IAIService 注入の有効化 |
| **統合結果** | プラグインは HttpClient と IAIService の両方を使用可能 |

## スタッキングは無制限の権限を与えない

複数の機能をスタックしても、以下は**常にブロック**されます：

- ❌ P/Invoke（`DllImport`、`Marshal`、`NativeMemory`）
- ❌ アンセーフコード（`UnverifiableCodeAttribute`、`Unsafe`）
- ❌ IL エミッション（`System.Reflection.Emit.*`）
- ❌ アセンブリ読み込み（`System.Runtime.Loader`、`Assembly.Load*`）
- ❌ レジストリ（`Microsoft.Win32.*`）

これらに対応する `Capability` 列挙値は存在しません — 設計上**宣言不可能**です。

## スタックされた機能の監査証跡

各機能は独立して記録されます：

```
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.Network — reason: API endpoint access for remote AI models
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.AI — reason: AI service provider for downstream plugins
```

## ファイル

- `Plugin.cs` — Capability.Network + Capability.AI をスタックするデモプラグイン
- `README.md` — このファイル（英語）
- `README.zh-CN.md` — 簡体字中国語
- 翻訳：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 関連例

- **13-CapabilityNetwork**: 単一の Network 機能
- **16-CapabilityAI**: 単一の AI 機能
- **18-CapabilityDenied**: 宣言不可機能のアンチパターン
