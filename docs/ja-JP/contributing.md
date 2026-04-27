# コントリビューションガイド

> **バージョン: v0.1.0-alpha**

[English](../en/contributing.md) | [中文](../zh-CN/contributing.md) | [繁體中文](../zh-HK/contributing.md) | [Español](../es-ES/contributing.md) | **日本語** | [한국어](../ko-KR/contributing.md) | [Čeština](../cs-CZ/contributing.md)

SiliconLifeCollective へのコントリビューションに興味をお寄せいただきありがとうございます！

## 行動規範

このプロジェクトは Apache 2.0 ライセンスに従います。すべての相互作用において、尊重と専門性を保ってください。

---

## クイックスタート

### 1. リポジトリをフォーク

GitHub の「Fork」ボタンをクリックして、自分のコピーを作成。

### 2. フォークをクローン

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. 開発環境の設定

```bash
# .NET 9 SDK をインストール
# https://dotnet.microsoft.com/download/dotnet/9.0

# 依存関係を復元
dotnet restore

# プロジェクトをビルド
dotnet build

# テストを実行
dotnet test
```

### 4. 機能ブランチを作成

```bash
git checkout -b feature/your-feature-name
```

---

## 開発ワークフロー

### コードスタイル

- C# コーディング規約に従う
- クラス名は PascalCase
- メソッドパラメータは camelCase
- プライベートフィールドは `_camelCase`
- すべての public API に XML ドキュメントが必要

### コミットメッセージ

**Conventional Commits** 形式に従う：

```
<type>(<scope>): <description>
```

**タイプ**：
- `feat`：新機能
- `fix`：バグ修正
- `docs`：ドキュメント変更
- `style`：コードフォーマット
- `refactor`：コードリファクタリング
- `test`：テスト変更
- `chore`：ビルド/ツール変更

**例**：
```bash
feat(localization): add Korean language support
fix(permission): fix null pointer in callback
docs: update contributing guide
refactor(web): simplify controller structure
```

### 変更を行う

1. **コードを書く**
   - 既存のパターンに従う
   - 新機能にテストを追加
   - ドキュメントを更新

2. **変更をテスト**
   ```bash
   # すべてのテストを実行
   dotnet test
   
   # リリースモードでビルド
   dotnet build --configuration Release
   ```

3. **変更をコミット**
   ```bash
   git add .
   git commit -m "feat(scope): description"
   ```

4. **ブランチにプッシュ**
   ```bash
   git push origin feature/your-feature-name
   ```

5. **プルリクエストを作成**

---

## プルリクエスト

### PR の作成

1. [リポジトリ](https://github.com/akimoto-akira/SiliconLifeCollective) に移動
2. 「Pull Request」をクリック
3. 変更を説明：
   - 何を解決するか
   - どうやって解決するか
   - テスト方法
4. レビューを依頼

### PR チェックリスト

プルリクエストを作成する前に確認：
- [ ] コードがコンパイルできる
- [ ] すべてのテストがパスする
- [ ] ドキュメントを更新
- [ ] コミットメッセージが明確
- [ ] 不要なファイルを含まない

---

## ドキュメント

### ドキュメントの更新

コードを変更する場合、関連するドキュメントも更新：

1. **コードコメント**：XML ドキュメントを追加/更新
2. **README**：新機能/変更を反映
3. **ガイド**：関連ガイドを更新
4. **API リファレンス**：新しいエンドポイント/パラメータを文書化

### 多言語ドキュメント

変更をすべての言語バージョンに反映：
- `docs/en/` - 英語
- `docs/zh-CN/` - 簡体字中国語
- `docs/zh-HK/` - 繁体字中国語
- `docs/es-ES/` - スペイン語
- `docs/ja-JP/` - 日本語
- `docs/ko-KR/` - 韓国語
- `docs/cs-CZ/` - チェコ語

---

## テスト

### 単体テスト

すべての新機能に単体テストを追加：

```csharp
public class MyFeatureTests
{
    [Fact]
    public async Task Should_WorkCorrectly()
    {
        // 配置
        var service = new MyService();
        
        // 実行
        var result = await service.DoSomething();
        
        // 検証
        Assert.NotNull(result);
        Assert.Equal(expected, result);
    }
}
```

### テストの実行

```bash
# すべてのテスト
dotnet test

# 特定のテストプロジェクト
dotnet test tests/SiliconLife.Tests

# カバレッジ付き
dotnet test /p:CollectCoverage=true
```

---

## コードレビュー

### レビューの依頼

1. PR を作成
2. レビュー担当者を指名
3. コンテキストを提供：
   - 変更の理由
   - 考慮した代替案
   - テスト結果

### レビューの受け取り

1. フィードバックに感謝
2. 質問に明確に回答
3. 必要に応じて変更
4. 議論を建設的に保つ

---

## 報告

### バグ報告

[Issues](https://github.com/akimoto-akira/SiliconLifeCollective/issues) でバグを報告：

1. 「New Issue」をクリック
2. 「Bug Report」テンプレートを選択
3. 以下を記載：
   - 問題の説明
   - 再現手順
   - 期待される動作
   - 実際の動作
   - 環境（OS、.NET バージョン）
   - ログ/スクリーンショット

### 機能リクエスト

1. 「New Issue」をクリック
2. 「Feature Request」テンプレートを選択
3. 以下を記載：
   - リクエストする機能
   - 使用シナリオ
   - 解決する問題
   - 代替案（あれば）

---

## ベストプラクティス

### 1. 小さな変更

- 小さな、焦点を絞った PR を作成
- 1つの PR = 1つの機能/修正
- 大きな変更は段階的に分割

### 2. クリーンコード

- 明確で意味のある名前
- 簡潔な関数（1つのことだけを行う）
- 適切なコメント
- ドライ原則に従う

### 3. テスト

- テスト駆動開発を考慮
- エッジケースをテスト
- 統合テストを追加
- カバレッジを維持

### 4. ドキュメント

- コードに変更があれば、ドキュメントも更新
- 例を提供
- 変更理由を説明

---

## ライセンス

このプロジェクトにコントリビューションすることで、あなたはあなたのコントリビューションが Apache 2.0 ライセンスの下でライセンスされることに同意します。

---

## 質問がありますか？

- 📖 [ドキュメント](docs/)を読む
- 💬 [ディスカッション](https://github.com/akimoto-akira/SiliconLifeCollective/discussions)に参加
- 🐛 [問題](https://github.com/akimoto-akira/SiliconLifeCollective/issues)を報告
- 📧 開発者に連絡

---

ご協力ありがとうございます！🎉
