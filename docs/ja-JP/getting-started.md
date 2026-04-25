# はじめに

## 前提条件

- **.NET 9 SDK** - [ダウンロード](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [ダウンロード](https://git-scm.com/)
- **Ollama**（オプション、ローカル AI 用） - [ダウンロード](https://ollama.com/)
- **DashScope API キー**（オプション、クラウド AI 用） - [申請](https://bailian.console.aliyun.com/)

## クイックスタート

[English](getting-started.md) | [简体中文](docs/zh-CN/getting-started.md) | [繁體中文](docs/zh-HK/getting-started.md) | [Español](docs/es-ES/getting-started.md) | [日本語](docs/ja-JP/getting-started.md) | [한국어](docs/ko-KR/getting-started.md) | [Čeština](docs/cs-CZ/getting-started.md)

### 1. リポジトリのクローン

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 2. プロジェクトのビルド

```bash
dotnet build
```

### 3. AI バックエンドの設定

`src/SiliconLife.Default/Config/DefaultConfigData.cs` を編集するか、Web UI を介して実行時に設定を変更します。

#### オプション A：Ollama（ローカル）

```json
{
  "AIClients": {
    "Ollama": {
      "BaseUrl": "http://localhost:11434",
      "Model": "qwen2.5:7b"
    }
  }
}
```

#### オプション B：DashScope（クラウド）

```json
{
  "AIClients": {
    "DashScope": {
      "ApiKey": "your-api-key-here",
      "Model": "qwen-plus",
      "Region": "cn-hangzhou"
    }
  }
}
```

### 4. アプリケーションの実行

```bash
cd src/SiliconLife.Default
dotnet run
```

Web サーバーが `http://localhost:8080` で起動します。

### 5. Web UI へのアクセス

ブラウザを開いて以下に移動します：

```
http://localhost:8080
```

以下を含むダッシュボードが表示されます：
- シリコンビーイング管理
- チャットインターフェース
- 設定パネル
- システムモニタリング

## 最初のシリコンビーイング

### 最初の生命体を作成

1. Web UI で**生命体管理**に移動
2. **新しい生命体を作成**をクリック
3. 個性と行動を含むソウルファイル（`soul.md`）を設定
4. 生命体を開始

### soul.md の例

```markdown
# 私の最初のシリコンビーイング

## 個性
あなたはコードレビューを専門とする役立つアシスタントです。

## 機能
- コード品質のレビュー
- 改善提案
- 複雑な概念の説明

## 行動
- 常に建設的なフィードバックを提供
- 明確な例を使用
- 簡潔かつ徹底的に
```

## よくある問題

### Ollama 接続拒否

**問題**：`http://localhost:11434` の Ollama に接続できません

**解決策**：
```bash
# Ollama が実行中か確認
ollama list

# 必要に応じて Ollama を起動
ollama serve
```

### モデルが見つからない

**問題**：`model "qwen2.5:7b" not found`

**解決策**：
```bash
# 必要なモデルをプル
ollama pull qwen2.5:7b
```

### ポートが既に使用中

**問題**：`HttpListenerException: Address already in use`

**解決策**：
- 設定でポートを変更
- またはポート 8080 を使用しているプロセスをkill：

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## 次のステップ

- 📚 システム設計を理解するための[アーキテクチャガイド](architecture.md)を読む
- 🛠️ システムを拡張するための[開発ガイド](development-guide.md)を確認
- 📖 統合の詳細については[API リファレンス](api-reference.md)を探索
- 🔒 権限システムについては[セキュリティドキュメント](security.md)を確認

## プロジェクト構造

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/      # コアインターフェースと抽象クラス
│   └── SiliconLife.Default/   # デフォルト実装 + エントリポイント
├── docs/                      # ドキュメント（多言語）
│   ├── en/                    # 英語
│   ├── zh-CN/                 # 簡体字中国語
│   ├── zh-HK/                 # 繁体字中国語
│   ├── ja-JP/                 # 日本語
│   └── ko-KR/                 # 韓国語
└── README.md                  # このファイル
```

## サポートが必要な場合

- 📖 [ドキュメント](docs/)を確認
- 🐛 GitHub で問題を報告
- 💬 コミュニティディスカッションに参加
