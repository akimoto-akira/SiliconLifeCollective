# トラブルシューティングガイド

[English](troubleshooting.md) | [简体中文](docs/zh-CN/troubleshooting.md) | [繁體中文](docs/zh-HK/troubleshooting.md) | [Español](docs/es-ES/troubleshooting.md) | [日本語](docs/ja-JP/troubleshooting.md) | [한국어](docs/ko-KR/troubleshooting.md) | [Čeština](docs/cs-CZ/troubleshooting.md)

## よくある問題と解決策

---

## インストール問題

### .NET SDK が見つからない

**エラー**：`dotnet: command not found`

**解決策**：
1. [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) をダウンロード
2. インストーラーを実行
3. ターミナルを再起動
4. 確認：`dotnet --version`

### ビルドエラー

**エラー**：`error NU1101: Unable to find package`

**解決策**：
```bash
# NuGet キャッシュをクリア
dotnet nuget locals all --clear

# 依存関係を復元
dotnet restore

# 再構築
dotnet build
```

---

## AI 接続問題

### Ollama に接続できない

**エラー**：`Failed to connect to http://localhost:11434`

**解決策**：
```bash
# Ollama が実行中か確認
ollama list

# 実行中でない場合は起動
ollama serve

# ファイアウォール設定を確認
# ポート 11434 がブロックされていないことを確認
```

### モデルが見つからない

**エラー**：`model "qwen2.5:7b" not found`

**解決策**：
```bash
# モデルをダウンロード
ollama pull qwen2.5:7b

# インストール済みモデルを確認
ollama list
```

### DashScope 認証失敗

**エラー**：`Invalid API key`

**解決策**：
1. API キーが正しいことを確認
2. [DashScope コンソール](https://bailian.console.aliyun.com/) でキーを再生成
3. 設定でキーを更新：

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "your-new-api-key"
    }
  }
}
```

---

## ポート問題

### ポート 8080 が既に使用中

**エラー**：`HttpListenerException: Address already in use`

**解決策**：

**Windows**：
```bash
# ポートを使用しているプロセスを検索
netstat -ano | findstr :8080

# プロセスをkill
taskkill /PID <PID> /F
```

**Linux/Mac**：
```bash
# ポートを使用しているプロセスを検索
lsof -ti:8080

# プロセスをkill
kill -9 <PID>
```

**またはポートを変更**：

`DefaultConfigData.cs` で：
```csharp
WebHostPort = 8081  // 別のポート
```

---

## 権限問題

### 権限拒否エラー

**エラー**：`Permission denied: disk:write`

**解決策**：
1. Web UI で権限管理に移動
2. ユーザーに適切な権限を付与
3. または ACL ルールを更新：

```json
{
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "expiresAt": "2026-04-21T00:00:00Z"
}
```

### IsCurator が機能しない

**確認**：
1. ユーザーが正しく設定されている
2. `IsCurator` プロパティが true に設定されている
3. 権限チェーンの最初のチェックである

---

## ストレージ問題

### データが保存されない

**確認**：
1. ストレージパスが存在する
2. ディレクトリに書き込み権限がある
3. ディスク容量が十分にある

**解決策**：
```bash
# ディレクトリを作成
mkdir -p ./data/beings

# 権限を設定（Linux/Mac）
chmod 755 ./data

# Windows で権限を確認
# フォルダのプロパティ → セキュリティタブ
```

### メモリ使用量が高すぎる

**解決策**：
1. メモリ制限を設定：

```csharp
var cache = new MemoryCache(new MemoryCacheOptions
{
    SizeLimit = 1024  // 1GB
});
```

2. 古いデータをクリーンアップ：

```bash
# 古いログを削除
rm -rf ./data/logs/*

# 古いチャット履歴を削除
rm -rf ./data/chats/2025-*
```

---

## チャット問題

### AI が応答しない

**確認**：
1. AI サービスが実行中
2. タイムアウトが発生していない
3. リクエストが正しい形式

**タイムアウトを増加**：
```csharp
var request = new AIRequest
{
    MaxTokens = 4000,  // 増加
    Timeout = TimeSpan.FromMinutes(5)  // タイムアウトを延長
};
```

### ストリーミングが切断される

**解決策**：
1. SSE 接続が安定していることを確認
2. プロキシ設定を確認
3. ブラウザのコンソールでエラーを確認

---

## パフォーマンス問題

### 応答が遅い

**解決策**：
1. より高速な AI モデルを使用
2. キャッシュを有効化
3. 不要なツールを無効化

**キャッシュの有効化**：
```csharp
ServiceLocator.Instance.Register<ICache>(new MemoryCache());
```

### CPU 使用率が高い

**確認**：
1. 無限ループがない
2. MainLoop が適切に実行されている
3. 実行中の生命体が多すぎない

**生命体の数を制限**：
```csharp
// 同時に実行される生命体の最大数
MaxConcurrentBeings = 5;
```

---

## ツール問題

### ツールが見つからない

**確認**：
1. ツールが `Tools/` ディレクトリにある
2. `ITool` インターフェースを実装
3. クラスが public である
4. コンパイルエラーがない

**ツールをデバッグ**：
```csharp
public class DebugTool : ITool
{
    public string Name => "debug";
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        Console.WriteLine($"ツール呼び出し：{call.Name}");
        Console.WriteLine($"パラメータ：{call.Parameters}");
        
        return ToolResult.Success("デバッグ出力を確認");
    }
}
```

### ツール実行が失敗

**確認**：
1. パラメータが正しい
2. 権限が十分
3. 依存関係がインストールされている

---

## カレンダー問題

### 日付変換が正しくない

**確認**：
1. 正しいカレンダーが指定されている
2. 日付がそのカレンダーで有効
3. 閏月が正しく処理されている

**カレンダーをデバッグ**：
```csharp
var calendar = new ChineseLunarCalendar();
var result = calendar.ConvertFromGregorian(new GregorianDate(2026, 4, 20));
Console.WriteLine($"結果：{result}");
```

---

## ログの確認

### ログの場所

```
./data/logs/
├── system.log      # システムログ
├── ai.log          # AI 相互作用
├── permission.log  # 権限決定
└── error.log       # エラーのみ
```

### ログレベルを設定

```csharp
LoggingManager.SetLogLevel(LogLevel.Debug);  // 詳細
LoggingManager.SetLogLevel(LogLevel.Info);   // 標準
LoggingManager.SetLogLevel(LogLevel.Error);  // エラーのみ
```

### ログを分析

```bash
# エラーを検索
grep "ERROR" ./data/logs/system.log

# 特定のコンポーネントを検索
grep "OllamaClient" ./data/logs/ai.log

# タイムスタンプでフィルター
grep "2026-04-20" ./data/logs/system.log
```

---

## システムをリセット

### 全面的なリセット

**警告**：これによりすべてのデータが削除されます！

```bash
# データディレクトリを削除
rm -rf ./data

# ビルドキャッシュをクリア
dotnet clean

# 再構築
dotnet build

# 再起動
dotnet run
```

### 設定のみをリセット

```bash
# 設定ファイルを削除
rm ./data/config.json

# デフォルト設定で再起動
dotnet run
```

---

## サポートが必要な場合

### リソース

- 📖 [ドキュメント](../) を確認
- 🐛 [GitHub Issues](https://github.com/akimoto-akira/SiliconLifeCollective/issues) で問題を報告
- 💬 コミュニティディスカッションに参加

### バグレポート

バグを報告する際は以下を含めてください：

1. **環境**：
   - OS: Windows/Linux/Mac
   - .NET バージョン: `dotnet --version`
   - AI バックエンド: Ollama/DashScope

2. **エラーメッセージ**：
   - 完全なエラー出力
   - ログファイルの関連部分

3. **再現手順**：
   - 問題を再現する手順
   - 期待される動作
   - 実際の動作

4. **設定**：
   - 関連する設定ファイル（API キーは削除）

---

## 作業ノート問題

### 問題: 作業ノート作成不可

**症状**:
```
Failed to create work note
```

**解決策**:
1. 生命体が存在し実行中か確認
2. 保存パスに書き込み権限があるか検証
3. 内容が空でないか確認（内容必須）
4. ログで詳細エラー情報確認

### 問題: ノート検索結果なし

**症状**:
- 検索キーワードが空の結果を返す
- しかし関連ノートがあることが確実

**解決策**:
1. キーワードの綴りが正しいか確認
2. より一般的なキーワードを試す
3. ノートに当該キーワードが含まれているか検証（大文字小文字区別）
4. `max_results` パラメータ値増加

### 問題: ノート目次生成が遅い

**症状**:
- 目次生成時に応答時間長い
- 生命体に多くのノートがある（>1000ページ）

**解決策**:
1. これは正常な現象、全ノートを巡回する必要がある
2. 古いノートの定期的なアーカイブを検討
3. 目次探索代わりに検索機能使用
4. 計画された最適化: 目次キャッシュメカニズム追加

---

## ナレッジネットワーク問題

### 問題: ナレッジ照会が空の結果を返す

**症状**:
```
No knowledge triples found
```

**解決策**:
1. 主語と述語の綴りを検証
2. ナレッジがネットワークに追加されたか確認
3. 検索機能でファジーマッチング使用:
```json
{
  "action": "search",
  "query": "キーワード"
}
```

### 問題: ナレッジパス発見失敗

**症状**:
```
No path found between concepts
```

**解決策**:
1. 2つの概念がナレッジネットワークに存在するか検証
2. 関連パスがあるか確認（直接的または間接的な関係がない可能性）
3. 接続構築のためにより多くのナレッジ追加を試す
4. パス長さ制限を下げる（設定されている場合）

### 問題: ナレッジ検証失敗

**症状**:
```
Knowledge validation failed
```

**解決策**:
1. トリプル形式が正しいか確認（主語、述語、目的語必須）
2. 信頼度が0.0-1.0範囲内にあるか検証
3. 重複したトリプルがないか確認
4. 検証エラー詳細情報確認して具体的問題把握

### 問題: ナレッジネットワーク統計情報が不正確

**症状**:
- 統計数字が予想と異なる
- ナレッジ追加後統計が更新されない

**解決策**:
1. 統計情報には数秒かかる可能性あり（キャッシュ）
2. 削除操作が成功裏に実行されたか確認
3. アプリケーション再起動して統計強制更新
4. APIを通じて統計情報再照会

---

## プロジェクト管理問題

### 問題: プロジェクト作成不可

**症状**:
```
Failed to create project
```

**解決策**:
1. プロジェクト名が空でないか確認（必須）
2. プロジェクト名が重複していないか検証
3. 保存パスに書き込み権限があるか確認
4. ログで詳細エラー情報確認

### 問題: プロジェクトデータ損失

**症状**:
- プロジェクト情報ロード不可
- プロジェクトファイル破損

**解決策**:
1. プロジェクト保存ディレクトリが存在するか確認
2. バックアップからプロジェクトデータ復元
3. JSONファイル形式が正しいか検証
4. 破損したプロジェクトファイル手動修复

---

## 次のステップ

- 📚 [アーキテクチャガイド](architecture.md)を読む
- 🛠️ [開発ガイド](development-guide.md)を確認
- 🚀 [はじめにガイド](getting-started.md)で開始
- 📖 他のドキュメントを探索
