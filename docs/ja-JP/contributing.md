# コントリビューションガイド

> **バージョン: v0.2.0-alpha**

[English](../en/contributing.md) | [Deutsch](../de-DE/contributing.md) | [中文](../zh-CN/contributing.md) | [繁體中文](../zh-HK/contributing.md) | [Español](../es-ES/contributing.md) | **日本語** | [한국어](../ko-KR/contributing.md) | [Čeština](../cs-CZ/contributing.md) | [Русский](../ru-RU/contributing.md)

SiliconLifeCollective へのコントリビューションに興味をお寄せいただきありがとうございます！

## デュアルバージョンコントリビューション

本プロジェクトには2つの実装バージョンがあります。興味に応じてコントリビューション方向を選択できます：

### SiliconLife.Default（デフォルトバージョン）
- **技術スタック**：.NET 9 コンソールアプリ
- **コントリビューション方向**：コア機能開発、ツール実装、ローカライゼーション、ドキュメント
- **適した人**：すべての開発者

### SiliconLife.Fast（高性能バージョン）
- **技術スタック**：.NET 9 Avalonia UI デスクトップアプリ
- **コントリビューション方向**：パフォーマンス最適化、SpeedyPack ストレージ、システムトレイ、ロックフリー並行性
- **適した人**：Windows開発経験があり、パフォーマンス最適化に興味がある開発者

> **重要なお知らせ**：両バージョンはSiliconLife.CoreとSiliconLife.Commonプロジェクトを共有しており、コアインターフェースの改善は両バージョンに同時に影響します。

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

### 5. 開発プロジェクトを選択

コントリビューションタイプに応じて適切なプロジェクトを選択：

- **コアインターフェース/抽象クラス** → `SiliconLife.Core` を修正
- **共有実装** → `SiliconLife.Common` を修正
- **Default バージョン固有** → `SiliconLife.Default` を修正
- **Fast バージョン固有** → `SiliconLife.Fast` を修正
- **ストレージエンジン** → `SiliconLife.Speedy` を修正
- **ストレージ管理ツール** → `SiliconLife.Speedy.Manager` を修正
- **プラグイン開発** → `SiliconLife.Core/Plugins` を修正
- **多言語ドキュメント** → `docs/` ディレクトリを修正

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

3. **コードをフォーマット**
   ```bash
   dotnet format
   ```

4. **変更をコミット**
   ```bash
   git add .
   git commit -m "feat(scope): description"
   ```

5. **フォークにプッシュ**
   ```bash
   git push origin feature/your-feature-name
   ```

6. **プルリクエストを作成**
   - 元のリポジトリに移動
   - 「Compare & pull request」をクリック
   - PR テンプレートに記入
   - 提出

---

## プルリクエストガイド

### PR タイトル

コミットメッセージと同じ形式を使用：
```
feat(localization): add Korean language support
```

### PR 説明

以下を含める：

1. **何を** - この PR は何をするか？
2. **なぜ** - なぜこの変更が必要か？
3. **どのように** - どのように実装したか？
4. **テスト** - どのようにテストしたか？

### PR 説明の例

```markdown
## 何を
すべての UI コンポーネントとドキュメントに韓国語ローカライゼーションを追加。

## なぜ
韓国語ユーザーに対するプロジェクトのアクセシビリティを拡大。

## どのように
- KoKR.cs ローカライゼーションファイルを作成
- 500+ 翻訳キーを追加
- すべてのビューでローカライゼーションを使用するように更新
- docs/ko-KR/ に韓国語ドキュメントを作成

## テスト
- すべての UI 要素が正しく韓国語を表示することを確認
- 言語切り替え機能をテスト
- ネイティブスピーカーと翻訳をレビュー
```

---

## コントリビューションの種類

### 1. バグ修正

**フロー**：
1. 既存のイシューを確認
2. 存在しない場合はイシューを作成
3. バグを修正
4. テストケースを追加
5. PR を提出

**要件**：
- バグの明確な説明
- 再現手順
- リグレッション防止のテスト

### 2. 新機能

**フロー**：
1. Issues/Discussions で機能を議論
2. メンテナーの承認を得る
3. 機能を実装
4. 包括的なテストを追加
5. ドキュメントを更新
6. PR を提出

**要件**：
- 機能提案が承認済み
- 完全なテストカバレッジ
- ドキュメントが更新済み
- 後方互換性

### 3. ドキュメント

**フロー**：
1. ドキュメントのギャップを特定
2. ドキュメントを作成/更新
3. PR を提出

**要件**：
- 明確で簡潔
- 例を含む
- 該当する場合は多言語サポート

### 4. コードリファクタリング

**フロー**：
1. Issue でリファクタリングを提案
2. 承認を得る
3. コードをリファクタリング
4. すべてのテストがパスすることを確認
5. PR を提出

**要件**：
- 機能変更なし
- すべてのテストがパス
- コード品質の向上
- 明確な説明

---

## テストガイド

### 単体テスト

```csharp
[TestMethod]
public void MyFeature_ShouldWork_AsExpected()
{
    // 配置
    var service = new MyService();
    
    // 実行
    var result = service.DoSomething();
    
    // 検証
    Assert.IsTrue(result.Success);
}
```

### 統合テスト

完全なワークフローをテスト：
- AI インタラクション
- ツール実行
- 権限検証
- ストレージ操作

### 手動テスト

UI 変更の場合：
- 複数のブラウザでテスト
- レスポンシブデザインを確認
- アクセシビリティをチェック

---

## ドキュメントガイド

### コードコメント

- すべての public API に XML コメントを使用
- 複雑なロジックにインラインコメントを使用
- コードコメントは英語を使用

### ドキュメントファイル

- `docs/{language}/` に配置
- すべての言語バージョンを更新
- 既存の構造に従う

### 多言語ドキュメント

ドキュメントを追加する場合：
1. まず英語版を作成
2. 他の言語に翻訳
3. コンテンツの同期を維持

---

## レビュープロセス

### メンテナーが確認する点

1. **コード品質**
   - 規約に従っている
   - 明確で読みやすい
   - ドキュメントが整備されている

2. **テスト**
   - 十分なカバレッジ
   - すべてのテストがパス
   - エッジケースをカバー

3. **ドキュメント**
   - 更新済み
   - 明確な説明
   - 多言語

4. **互換性**
   - 後方互換
   - 破壊的変更なし（通知がない限り）
   - セマンティックバージョニングに従う

### レビュー期間

- 初回レビュー：1-3日
- フィードバック統合：必要に応じて
- マージ：承認後

---

## よくある質問

### PR が拒否された場合

**理由**：
- ガイドラインに従っていない
- テストが不十分
- 通知のない破壊的変更
- コード品質が低い

**解決策**：
- フィードバックに対応
- PR を更新
- 再提出

### マージコンフリクト

**解決策**：
```bash
# ブランチを更新
git fetch origin
git rebase origin/master

# コンフリクトを解決
# コンフリクトファイルを編集
git add .
git rebase --continue

# 強制プッシュ
git push --force-with-lease
```

---

## ヘルプの入手

### リソース

- **ドキュメント**：[docs/](../)
- **イシュー**：GitHub Issues
- **ディスカッション**：GitHub Discussions
- **行動規範**：CODE_OF_CONDUCT.md

### 連絡先

- バグの場合は Issue を作成
- 質問の場合は Discussion を開始
- 緊急事項の場合はメンテナーにメンション

ご協力ありがとうございます！🎉

---

## 謝辞

コントリビューターは以下の場所で認められます：
- README.md コントリビューターセクション
- リリースノート
- プロジェクトドキュメント

---

## ライセンス

コントリビューションを行うことで、あなたのコントリビューションが Apache 2.0 ライセンスの下でライセンスされることに同意したことになります。

---

## 次のステップ

- 📚 [ドキュメント](../)を読む
- 🐛 [オープンなイシュー](https://github.com/akimoto-akira/SiliconLifeCollective/issues)を確認
- 💬 [ディスカッション](https://github.com/akimoto-akira/SiliconLifeCollective/discussions)を開始
- 🚀 フォークしてコントリビューションを開始！

SiliconLifeCollective へのコントリビューションに感謝します！🎉
