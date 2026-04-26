# トラブルシューティングガイド

[English](../en/troubleshooting.md) | [中文](../zh-CN/troubleshooting.md) | [繁體中文](../zh-HK/troubleshooting.md) | [Español](../es-ES/troubleshooting.md) | **日本語** | [한국어](../ko-KR/troubleshooting.md) | [Čeština](../cs-CZ/troubleshooting.md)

## 一般的な問題

### ビルドとコンパイル

#### 問題：依存関係の欠落でビルド失敗

**症状**：
```
error CS0246: The type or namespace name 'Microsoft.CodeAnalysis' could not be found
```

**解決策**：
```bash
dotnet restore
dotnet build
```

#### 問題：.NET SDK が見つからない

**症状**：
```
The .NET SDK could not be found
```

**解決策**：
1. .NET 9 SDK をインストール：https://dotnet.microsoft.com/download/dotnet/9.0
2. インストールを検証：
```bash
dotnet --version
```

---

### AI 接続問題

#### 問題：Ollama 接続拒否

**症状**：
```
Failed to connect to Ollama at http://localhost:11434
```

**解決策**：
```bash
# Ollama が実行中か確認
ollama list

# Ollama を起動
ollama serve

# 接続をテスト
curl http://localhost:11434/api/tags
```

#### 問題：モデルが見つからない

**症状**：
```
model "qwen2.5:7b" not found
```

**解決策**：
```bash
# 必要なモデルをプル
ollama pull qwen2.5:7b

# 利用可能なモデルをリスト
ollama list
```

#### 問題：DashScope 404 エラー

**症状**：
```
HTTP 404: Model not found
```

**解決策**：
1. API キーが正しいことを確認
2. モデル名が DashScope カタログと一致するか確認
3. リージョンエンドポイントが正しいことを確認
4. アカウントがモデルにアクセスできるか確認

---

### 実行時問題

#### 問題：ポートが既に使用されている

**症状**：
```
HttpListenerException: Address already in use
```

**解決策**：

**Windows**：
```bash
netstat -ano | findstr :8080
taskkill /PID <PID> /F
```

**Linux/Mac**：
```bash
lsof -ti:8080 | xargs kill -9
```

**または設定でポートを変更**。

#### 問題：生命体が起動しない

**症状**：
- 生命体状態が「Error」を表示
- ログに初期化失敗が表示

**解決策**：
1. ソウルファイルが存在し、有効か確認
2. AI クライアントが設定されていることを確認
3. ログで具体的なエラーを確認：
```bash
tail -f logs/*.log
```

#### 問題：メモリ不足

**症状**：
```
OutOfMemoryException
```

**解決策**：
1. ヒープサイズを増加：
```bash
dotnet run --server.gcHeapCount 4
```

2. 古いデータをクリーンアップ：
```bash
# 古いログをアーカイブ
mv logs/ logs-archive/
mkdir logs

# 古いメモリをクリーンアップ
# Web UI 経由：メモリ管理 > クリーンアップ
```

---

### 権限問題

#### 問題：予期しない権限拒否

**解決策**：
1. ユーザーの IsCurator 状態を確認
2. レートリミット設定を確認
3. GlobalACL ルールを確認
4. コールバックロジックを確認
5. ユーザー応答タイムアウトを確認

#### 問題：権限が期限切れにならない

**解決策**：
- `expiresAt` フィールドが正しく設定されていることを確認
- タイムゾーンが正確であることを確認
- クロックが同期していることを確認

---

### Web UI 問題

#### 問題：Web UI にアクセスできない

**解決策**：
1. サーバーが実行中であることを確認
2. ポート 8080 がブロックされていないことを確認
3. ファイアウォール設定を確認
4. ブラウザのコンソールでエラーを確認

#### 問題：SSE が機能しない

**解決策**：
1. ブラウザが SSE をサポートしていることを確認
2. プロキシが SSE をバッファリングしていないことを確認
3. ネットワークの安定性を確認
4. ブラウザの開発者ツールで接続を確認

---

### カレンダー問題

#### 問題：日付変換が正しくない

**解決策**：
1. カレンダー ID が正しいことを確認
2. 日付形式が正しいことを確認（YYYY-MM-DD）
3. 閏月処理を確認
4. カレンダーの既知の制限を確認

---

### ツール問題

#### 問題：ツールが実行されない

**解決策**：
1. ツール名が正しいことを確認
2. パラメータが正しい形式であることを確認
3. 権限が適切に付与されていることを確認
4. 実行器が実行中であることを確認

#### 問題：ツールがタイムアウト

**解決策**：
1. 実行器のタイムアウト設定を増加
2. ネットワーク接続を確認
3. リソースの利用可能性を確認
4. サーキットブレーカー状態を確認

---

## ログの表示

### ログファイルの場所

```
logs/
├── application.log
├── error.log
└── being-{id}.log
```

### リアルタイムログモニタリング

```bash
# すべてのログをフォロー
tail -f logs/*.log

# エラーログのみ
tail -f logs/error.log

# 特定の生命体
tail -f logs/being-{id}.log
```

### Web UI でのログ表示

1. **ログ**ページに移動
2. レベルでフィルタ（情報/警告/エラー）
3. キーワードで検索
4. 時間範囲を選択

---

## デバッグ

### デバッグモードの有効化

```bash
# デバッグビルド
dotnet build --configuration Debug

# デバッグで実行
dotnet run --configuration Debug
```

### Visual Studio でのデバッグ

1. プロジェクトを開く
2. ブレークポイントを設定
3. F5 でデバッグ開始
4. 変数を検査
5. コールスタックを確認

### VS Code でのデバッグ

1. デバッグ拡張をインストール
2. `launch.json` を設定
3. F5 でデバッグ開始
4. デバッグコンソールを使用

---

## パフォーマンス

### CPU 使用率が高い

**解決策**：
1. プロファイラーを実行：
```bash
dotnet trace collect --process-id <PID>
```

2. ホットパスを特定
3. アルゴリズムを最適化
4. キャッシュを使用

### メモリ使用率が高い

**解決策**：
1. メモリダンプを分析：
```bash
dotnet-dump collect --process-id <PID>
```

2. メモリリークを特定
3. 大きなオブジェクトを破棄
4. GC 設定を調整

---

## 一般的なエラーコード

| エラーコード | 説明 | 解決策 |
|----------|------|------|
| 400 | 不正なリクエスト | リクエストパラメータを確認 |
| 401 | 認証失敗 | 認証情報を確認 |
| 403 | 権限なし | 権限を確認 |
| 404 | 見つからない | URL/パスを確認 |
| 500 | サーバーエラー | ログを確認 |
| 503 | サービス利用不可 | サーバー状態を確認 |

---

## サポートの取得

### ドキュメント

- 📚 [完全なドキュメント](docs/)
- 🛠️ [開発ガイド](development-guide.md)
- 📖 [API リファレンス](api-reference.md)

### コミュニティ

- 🐛 [GitHub Issues](https://github.com/akimoto-akira/SiliconLifeCollective/issues) で問題を報告
- 💬 ディスカッションに参加
- 📧 開発者に連絡

---

## 次のステップ

- 📚 [アーキテクチャガイド](architecture.md)を読む
- 🛠️ [開発ガイド](development-guide.md)をチェック
- 📖 [API リファレンス](api-reference.md)を探る
- 🚀 [クイックスタートガイド](getting-started.md)で始める
