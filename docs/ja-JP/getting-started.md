# クイックスタート

> **バージョン: v0.1.0-alpha**

[English](../en/getting-started.md) | [Deutsch](../de-DE/getting-started.md) | [中文](../zh-CN/getting-started.md) | [繁體中文](../zh-HK/getting-started.md) | [Español](../es-ES/getting-started.md) | **日本語** | [한국어](../ko-KR/getting-started.md) | [Čeština](../cs-CZ/getting-started.md)

## バージョンの選択

本プロジェクトは2つの実装バージョンを提供します：

### SiliconLife.Default（デフォルトバージョン）
- **ポジショニング**：デフォルト実装、アーキテクチャの実現可能性検証に主に使用
- **実行モード**：コンソールアプリケーション
- **ストレージ**：ファイルシステムJSONストレージ
- **適用シナリオ**：データセキュリティ優先、小データ量、開発デバッグ、アーキテクチャ検証
- **プラットフォームサポート**：Windows、Linux、macOS
- **役割説明**：アーキテクチャ検証のベースライン実装として、シンプルで信頼性の高い実行方式を提供し、本プロジェクトへの初めての接触や開発デバッグに適しています

### SiliconLife.Fast（高性能バージョン）
- **ポジショニング**：主力本番バージョン
- **実行モード**：Windowsフォームアプリケーション（システムトレイ）
- **ストレージ**：SpeedyPack メモリストレージ + 非同期永続化（.spk ファイル形式）
- **適用シナリオ**：高同時実行性、低レイテンシ、大データ量、長期本番運用
- **プラットフォームサポート**：Windowsのみ
- **役割説明**：深い最適化が施された本番グレードの実装であり、長期運用と実際の本番環境の第一選択です

> **初心者向け推奨**：初めての方は **SiliconLife.Default** から開始し、アーキテクチャの実現可能性を迅速に検証してください。システムに慣れた後は、**SiliconLife.Fast** への移行を強く推奨します。

## 前提条件

- **.NET 9 SDK** - [ダウンロード](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [ダウンロード](https://git-scm.com/)
- **Ollama**（オプション、ローカル AI 用） - [ダウンロード](https://ollama.com/)
- **DashScope API キー**（オプション、クラウド AI 用） - [申請](https://bailian.console.aliyun.com/)
- **Volcengine Ark API キー**（オプション、クラウド AI 用） - [申請](https://console.volcengine.com/ark)

## クイックスタート

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

`src/SiliconLife.Default/Config/DefaultConfigData.cs` を編集するか、実行時に Web UI から設定を変更します。

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

#### オプション C：Volcengine Ark（クラウド）

```json
{
  "AIClients": {
    "VolcengineArk": {
      "ApiKey": "your-api-key-here",
      "Endpoint": "https://ark.cn-beijing.volces.com/api/v3/chat/completions",
      "Model": "ep-xxxxxxxxxxxxx-xxxxx"
    }
  }
}
```

> **注意**：Volcengine Ark の Model パラメータには、モデル名ではなく推論エンドポイント ID（例：`ep-20241212123456-abcde`）を使用します。

### 4. アプリケーションの実行

#### Default バージョンの実行

```bash
cd src/SiliconLife.Default
dotnet run
```

Web サーバーが `http://localhost:8080` で起動します

#### Fast バージョンの実行（Windowsのみ）

```bash
cd src/SiliconLife.Fast
dotnet run
```

アプリケーションはフォームモードで起動し、システムトレイに最小化され、Web サーバーも `http://localhost:8080` で起動します

### 5. Web UI へのアクセス

ブラウザを開き、次のアドレスに移動します：

```
http://localhost:8080
```

以下の内容を含むダッシュボードが表示されます：
- シリコン生命体管理
- チャットインターフェース
- 設定パネル
- システムモニタリング

## 最初のシリコン生命体

### 最初の生命体を作成

1. Web UI で**生命体管理**に移動
2. **新生命体を作成**をクリック
3. ソウルファイル（`soul.md`）を設定。個性と行動を定義
4. 生命体を起動

### soul.md の例

```markdown
# My First Silicon Being

## Personality
You are a helpful assistant specializing in code review.

## Capabilities
- Review code quality
- Suggest improvements
- Explain complex concepts

## Behavior
- Always provide constructive feedback
- Use clear examples
- Be concise but thorough
```

## よくある質問

### Ollama 接続が拒否される

**問題**：`http://localhost:11434` の Ollama に接続できない

**解決策**：
```bash
# Ollama が実行中か確認
ollama list

# Ollama を起動
ollama serve
```

### モデルが見つからない

**問題**：`model "qwen2.5:7b" not found`

**解決策**：
```bash
# 必要なモデルをプル
ollama pull qwen2.5:7b
```

### ポートが既に使用されている

**問題**：`HttpListenerException: Address already in use`

**解決策**：
- 設定でポートを変更
- またはポート 8080 を使用しているプロセスを終了：

```bash
# Windows
netstat -ano | findstr :8080
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8080 | xargs kill -9
```

## 次のステップ

- 📚 [アーキテクチャガイド](architecture.md)を読んでシステム設計を理解
- 🛠️ [開発ガイド](development-guide.md)をチェックしてシステムを拡張
- 📖 [API リファレンス](api-reference.md)を探って統合詳細を確認
- 🔒 [セキュリティドキュメント](security.md)を見て権限システムを理解
- 🧰 [ツールリファレンス](tools-reference.md)ですべての組み込みツールを確認
- 🌐 [Web UI ガイド](web-ui-guide.md)でインターフェース機能を理解

## プロジェクト構造

```
SiliconLifeCollective/
├── src/
│   ├── SiliconLife.Core/            # コアインターフェースと抽象クラス
│   ├── SiliconLife.Common/          # 共有実装（両バージョンで共用）
│   ├── SiliconLife.App/             # Default と Fast で共有のアプリケーション層
│   ├── SiliconLife.Default/         # デフォルト実装 + エントリーポイント（コンソール版）
│   ├── SiliconLife.Fast/            # 高性能実装 + エントリーポイント（フォーム版）
│   ├── SiliconLife.Speedy/          # SpeedyPack 高性能ストレージエンジン
│   └── SiliconLife.Speedy.Manager/  # SpeedyPack 管理ツール（Windows Forms）
├── docs/                      # ドキュメント（多言語、29種類の言語バリアント）
│   ├── en/                    # 英語
│   ├── zh-CN/                 # 簡体字中国語
│   ├── zh-HK/                 # 繁体字中国語
│   ├── es-ES/                 # スペイン語
│   ├── ja-JP/                 # 日本語
│   ├── ko-KR/                 # 韓国語
│   └── cs-CZ/                 # チェコ語
├── 总文档/                     # 要件とアーキテクチャドキュメント（中国語）
└── README.md                  # プロジェクト説明
```

## サポートが必要ですか？

- 📖 [ヘルプドキュメントシステム](web-ui-guide.md#帮助文档系统新增)（多言語サポート）を確認
- 📚 [完全なドキュメント](docs/)を読む
- 🐛 [GitHub](https://github.com/akimoto-akira/SiliconLifeCollective/issues) で問題を報告
- 💬 コミュニティディスカッションに参加
