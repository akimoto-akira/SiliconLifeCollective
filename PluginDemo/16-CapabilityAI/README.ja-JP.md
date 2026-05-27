# PluginDemo-16: Capability.AI — 宣言型 AI サービス権限

## 概要

このプラグインは、`[PluginCapability(Capability.AI)]` を使用してプラグインが AI サービスへのアクセスを必要とすることを宣言する方法を示します。他の機能とは異なり、`Capability.AI` は禁止名前空間を**免除しません** — 代わりにホストがプラグインに `IAIService` 参照を注入できるようにします。

## 主要概念：Capability.AI はネットワークアクセスを付与しない

`Capability.AI` は他の機能と根本的に異なります：

| 機能 | 免除内容 | 動作方式 |
|------|---------|---------|
| `Capability.Network` | `System.Net.*` 名前空間 | TypeRef/ILString スキャンルールを緩和 |
| `Capability.FileIO` | `System.IO` 名前空間 | TypeRef/ILString スキャンルールを緩和 |
| `Capability.Process` | `Process*` 型 | TypeRef/ILString スキャンルールを緩和 |
| `Capability.AI` | **なし** | ホストによる IAIService 注入を有効化 |

`IAIService` は `SiliconLife.Collective` 名前空間にあります — 禁止リストに含まれることはありません。機能宣言は、このプラグインが AI サービス参照を受け取るべきであることをホストに伝える**オプトインシグナル**です。

## 機能スタッキング：AI + Network

AI クライアントが直接ネットワークアクセスを必要とする場合（例：リモート AI エンドポイントの呼び出し）、**両方**の機能を宣言する必要があります：

```csharp
[PluginCapability(Capability.Network, Reason = "Calls remote AI endpoint")]
[PluginCapability(Capability.AI, Reason = "Provides IAIService to downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

完全なスタッキング例については **17-CapabilityStacked** を参照してください。

## 制御されたエントリポイントパターン

| リソース | 制御されたエントリポイント | 機能宣言の必要性 |
|---------|------------------------|----------------|
| ファイル | `PermissionedStreamFactory` | 不要 |
| ネットワーク | `NetworkExecutor` | 不要 |
| プロセス | `CommandLineExecutor` | 不要 |
| データストア | `SpeedyPack` | 不要 |
| AI サービス | `IAIService` | `Capability.AI` |

`IAIService` はユニークです：機能宣言が**必要**です。これは、AI サービスアクセスがオプトイン機能であり、すべてのプラグインで利用可能なデフォルトの機能ではないためです。

## ファイル

- `Plugin.cs` — Capability.AI を宣言するデモプラグイン
- `README.md` — このファイル（英語）
- `README.zh-CN.md` — 簡体字中国語
- 翻訳：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 関連例

- **17-CapabilityStacked**: 複数機能のスタッキング（Network + AI）
- **18-CapabilityDenied**: 宣言不可機能のアンチパターン
