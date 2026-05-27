# PluginDemo-19: TickObject — MainLoop での定期タスク

## 概要

このプラグインは、`TickObject` を使用して `MainLoop` と統合し、定期/継続的ロジックを実装する方法を示します。TickObject は MainLoop のメインループによって tick できるオブジェクトの基底クラスであり、`System.Threading.Timer` や `Task.Delay` の統一代替手段を提供します。

## TickObject ライフサイクル

```
Constructor(interval, autoRegister)
    │
    ├── autoRegister=true → コンストラクタで MainLoop.Register(this) が自動呼び出し
    │
    ├── autoRegister=false → 後で手動で MainLoop.Register(this) を呼び出し
    │
    ▼
MainLoop.Tick() ループ
    │
    ├── 登録済みの全 TickObject を Priority 昇順でソート
    ├── 各 TickObject の elapsedTime を蓄積
    ├── elapsedTime >= Interval の場合 → OnTick(deltaTime) を呼び出し
    │
    ├── サーキットブレーカー：OnTick が TickTimeout を超過 → タイムアウトカウント増加
    │   └── maxTimeoutCount 回連続タイムアウト後 → 1分間のクールダウン
    │
    ▼
MainLoop.Unregister(tickObject) — OnStop でクリーンアップ
```

## 主要プロパティ

| プロパティ | 型 | デフォルト | 説明 |
|-----------|---|----------|------|
| `Interval` | `TimeSpan` | 必須 | OnTick が呼び出される間隔 |
| `Priority` | `int` | 100 | 実行順序（値が小さいほど高優先度） |
| `autoRegister` | `bool` | `true` | コンストラクタで MainLoop に自動登録するか |

## 主要メソッド

| メソッド | 説明 |
|---------|------|
| `OnTick(TimeSpan deltaTime)` | オーバーライドして定期ロジックを実装 |
| `MainLoop.Register(TickObject)` | MainLoop に手動登録 |
| `MainLoop.Unregister(TickObject)` | MainLoop から削除（クリーンアップ） |

## デモシナリオ

### 1. 基本タイマー（autoRegister=true）
```csharp
public class StatusTimer : TickObject
{
    public StatusTimer() : base(interval: TimeSpan.FromSeconds(5), autoRegister: true)
    {
        Priority = 100;
    }

    protected override void OnTick(TimeSpan deltaTime)
    {
        Console.WriteLine($"Tick, deltaTime={deltaTime.TotalMilliseconds:F0}ms");
    }
}
```

### 2. 手動登録（autoRegister=false）
```csharp
// コンストラクタで：自動登録しない
_heartbeatTimer = new HeartbeatTimer(autoRegister: false);

// OnStart で：手動登録
MainLoop.Register(_heartbeatTimer);
```

### 3. 優先度の順序
- `Priority = 10` → 高優先度、先に実行
- `Priority = 200` → 低優先度、後に実行

### 4. クリーンアップ
```csharp
// OnStop で：リークを防ぐため必ず登録解除
MainLoop.Unregister(_statusTimer);
```

## MainLoop サーキットブレーカー

MainLoop には遅い TickObject がループ全体をブロックするのを防ぐ内蔵サーキットブレーカーがあります：

1. `OnTick` が `TickTimeout`（デフォルト1秒）を超過 → タイムアウトカウント増加
2. `maxTimeoutCount`（デフォルト3）回連続タイムアウト → サーキットブレーカーが作動
3. 作動した TickObject は1分間のクールダウン中**スキップ**される
4. クールダウン後、TickObject は再び実行の機会を得る

## TickObject vs System.Threading.Timer

| 側面 | TickObject + MainLoop | System.Threading.Timer |
|------|----------------------|----------------------|
| スレッドモデル | 単一メインループスレッド | スレッドプールスレッド |
| 実行順序 | 決定的（Priority 順） | 非決定的 |
| サーキットブレーカー | 内蔵 | なし |
| デバッグ | 容易（単一スレッド） | 困難（競合状態） |
| リソース使用量 | 最小（スレッドプールなし） | スレッドプールのオーバーヘッド |
| 間隔精度 | ベストエフォート（他の TickObject の影響を受ける） | より正確 |

## セキュリティ上の注意

TickObject 自体は機能宣言が**不要**です。安全な内蔵フレームワーク機構です。

## ファイル

- `Plugin.cs` — TickObject デモプラグイン
- `README.md` — このファイル（英語）
- `README.zh-CN.md` — 簡体字中国語
- 翻訳：zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## 関連例

- **13-CapabilityNetwork**: Capability.Network 宣言
- **20-SpeedyPack**: Capability.FileIO 不要のデータストレージ
