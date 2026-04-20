# 貢献ガイド

## はじめに

SiliconLifeCollective への貢献に興味を持っていただきありがとうございます！このガイドは、プロジェクトへの貢献方法を説明します。

---

## コミュニティ行動規範

このプロジェクトおよびコミュニティは、オープンで歓迎的な環境を維持するために[貢献者規約](https://www.contributor-covenant.org/)を採用しています。

---

## バグ報告

### バグを見つける

バグを見つけることは、プロジェクトを改善するための貴重な貢献です！

### 問題の報告方法

1. [GitHub Issues](https://github.com/your-org/SiliconLifeCollective/issues) に移動
2. **新しい Issue** をクリック
3. 以下の情報を含める：
   - 明確なタイトル
   - 再現手順
   - 期待される動作
   - 実際の動作
   - 環境詳細（OS、.NET バージョンなど）
   - エラーメッセージとログ

### バグ報告テンプレート

```markdown
**説明**
何が起こったかの明確で簡潔な説明。

**再現手順**
1. ...
2. ...
3. ...

**期待される動作**
何が起きるべきだったか。

**実際の動作**
実際に何が起きたか。

**環境**
- OS: [例：Windows 11]
- .NET: [例：9.0.100]
- AI バックエンド: [例：Ollama 0.1.0]

**追加コンテキスト**
問題に役立つその他の情報。
```

---

## 機能提案

### 新しい機能を提案

1. [GitHub Discussions](https://github.com/your-org/SiliconLifeCollective/discussions) を確認
2. すでに提案されていないことを確認
3. 新しいディスカッションを作成
4. 機能の目的とユースケースを説明

### 機能リクエストテンプレート

```markdown
**問題の説明**
この機能が解決する問題を説明。

**提案する解決策**
どのように実装すべきか。

**代替案**
考慮した他の解決策。

**追加コンテキスト**
モックアップ、例、その他の情報。
```

---

## コード貢献

### 開発環境の設定

1. リポジトリをフォーク
2. クローン：
```bash
git clone https://github.com/YOUR-USERNAME/SiliconLifeCollective.git
cd SiliconLifeCollective
```

3. アップストリームを追加：
```bash
git remote add upstream https://github.com/your-org/SiliconLifeCollective.git
```

4. 依存関係をインストール：
```bash
dotnet restore
```

### 作業開始

1. 最新の変更を取得：
```bash
git checkout main
git pull upstream main
```

2. 機能ブランチを作成：
```bash
git checkout -b feature/amazing-feature
```

### コードスタイル

#### 命名規則
- **クラス**：PascalCase（例：`DefaultSiliconBeing`）
- **メソッド**：PascalCase（例：`ProcessMessage`）
- **変数**：camelCase（例：`messageCount`）
- **定数**：UPPER_CASE（例：`MAX_RETRIES`）

#### ドキュメント
- すべてのパブリック API に XML コメント
- 複雑なロジックにインラインコメント
- 例を含める

```csharp
/// <summary>
/// メッセージを処理し、レスポンスを返します。
/// </summary>
/// <param name="message">処理するメッセージ</param>
/// <returns>AI レスポンス</returns>
public async Task<string> ProcessMessage(string message)
{
    // 実装
}
```

#### コード構成
- インターフェースは `I` で始める
- 実装は具体的な名前を使用
- ツールは `Tool` で終わる

### テスト

#### ユニットテストの作成

```csharp
[TestClass]
public class MyToolTests
{
    [TestMethod]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccess()
    {
        // 手配
        var tool = new MyTool();
        
        // 実行
        var result = await tool.ExecuteAsync(call);
        
        // 検証
        Assert.IsTrue(result.Success);
    }
}
```

#### テストの実行

```bash
# すべてのテスト
dotnet test

# 特定のプロジェクト
dotnet test tests/SiliconLife.Core.Tests

# カバレッジ付き
dotnet test /p:CollectCoverage=true
```

### Git ワークフロー

#### コミットメッセージ

[慣例的コミット](https://www.conventionalcommits.org/) を使用：

```
<type>(<scope>): <description>

例：
feat(calendar): add Japanese calendar support
fix(permission): resolve null pointer in callback
docs: update architecture guide
refactor(web): simplify controller logic
```

#### タイプ
- `feat` - 新しい機能
- `fix` - バグ修正
- `docs` - ドキュメント
- `style` - フォーマット
- `refactor` - リファクタリング
- `test` - テスト
- `chore` - メンテナンス

#### 原子コミット

1 つのコミットは 1 つのことだけを行う：

```bash
# 良い
git add src/Calendar/JapaneseCalendar.cs
git commit -m "feat(calendar): add Japanese calendar support"

git add docs/ja-JP/calendar-system.md
git commit -m "docs(calendar): document Japanese calendar"

# 悪い
git add .
git commit -m "update everything"
```

### プルリクエスト

#### 提出前

1. 最新の変更と統合：
```bash
git fetch upstream
git rebase upstream/main
```

2. テストを実行：
```bash
dotnet test
dotnet build
```

3. コードをフォーマット：
```bash
dotnet format
```

#### PR の作成

1. GitHub で **New Pull Request** をクリック
2. 説明を含める：
   - 変更の目的
   - 関連する Issue
   - テスト方法
   - スクリーンショット（UI 変更の場合）

#### PR テンプレート

```markdown
## 説明
この PR の目的。

## 関連 Issue
Fixes #123

## 変更の種類
- [ ] バグ修正
- [ ] 新しい機能
- [ ] 破壊的変更
- [ ] ドキュメント更新

## テスト
- [ ] ユニットテストを追加/更新
- [ ] すべてのテストが合格
- [ ] 手動テスト済み

## チェックリスト
- [ ] コードがプロジェクトのスタイルガイドに従っている
- [ ] ドキュメントを更新
- [ ] テストを追加/更新
```

---

## ドキュメント貢献

### 翻訳

多言語ドキュメントの翻訳を歓迎します！

1. `docs/en/` の英語原文を確認
2. 対応する言語ディレクトリに翻訳を作成
3. 技術用語の一貫性を維持
4. コード例を翻訳しないでください

### ドキュメントスタイル

- 明確で簡潔な言語を使用
- 例を含める
- スクリーンショットを追加（UI の場合）
- 相互参照を更新

---

## デザイン貢献

### UI/UX 改善

- 新しいスキンを提案
- アクセシビリティを改善
- レスポンシブデザインを向上

### アーキテクチャ提案

- デザインパターンを提案
- パフォーマンスを最適化
- セキュリティを強化

---

## レビュープロセス

### 何が期待できるか

1. メンテナーが PR をレビュー（1-3 営業日）
2. フィードバックを提供
3. 変更を要求（必要な場合）
4. 承認後、マージ

### レビュー基準

- [ ] コードが機能する
- [ ] テストが合格
- [ ] ドキュメントが更新されている
- [ ] プロジェクトの標準に従っている
- [ ] セキュリティ懸念がない

---

## コミュニティ

### ディスカッションに参加

- [GitHub Discussions](https://github.com/your-org/SiliconLifeCollective/discussions)
- 質問をする
- アイデアを共有
- 他の貢献者を支援

### 行動規範

- 親切で敬意を持つ
- 建設的なフィードバックを提供
- 多様性を尊重
- コミュニティを歓迎する

---

## ライセンス

このプロジェクトに貢献することで、あなたの貢献が Apache License 2.0 の下でライセンスされることに同意します。

---

## 質問がある場合

- 📖 [ドキュメント](../) を確認
- 💬 GitHub Discussions で質問
- 🐛 Issue を開く

貢献していただきありがとうございます！🎉
