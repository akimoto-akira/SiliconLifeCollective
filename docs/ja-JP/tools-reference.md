# ツールリファレンス

> **バージョン: v0.2.0-alpha**

本ドキュメントでは、Silicon Life Collective プラットフォームのすべての組み込みツールについて詳しく説明します。

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | **日本語** | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md)

## 概要

ツールシステムは、シリコン生命体が標準化されたインターフェースを介して外部世界と対話できるようにします。各ツールは `ITool` インターフェースを実装し、`ToolManager` がリフレクションを介して自動的に検出および登録。

### ツール分類

- **システム管理ツール** — 設定、権限、動的コンパイル
- **通信ツール** — チャット、ネットワークリクエスト
- **データストレージツール** — ディスク操作、データベース、記憶、作業ノート
- **時間管理ツール** — カレンダー、タイマー、タスク
- **開発ツール** — コード実行、ログクエリ
- **ユーティリティツール** — システム情報、Token 監査、ヘルプドキュメント、ナレッジネットワーク
- **ブラウザツール** — WebView ブラウザ自動化
- **プラグインツール** — プラグインシステムを介して登録されたサードパーティツール

---

## 組み込みツール一覧

### 1. カレンダーツール (CalendarTool)

**ツール名**: `calendar`

**機能説明**: 32 種類のカレンダーシステムに対応した日付変換と計算。

**サポートされる操作**:
- `now` — 現在時刻を取得
- `format` — 日付のフォーマット
- `add_days` — 日付の加減算
- `diff` — 日付差の計算
- `list_calendars` — サポートされるすべてのカレンダーを一覧表示
- `get_components` — 日付コンポーネントを取得
- `get_now_components` — 現在時刻のコンポーネントを取得
- `convert` — カレンダーシステム間の変換

**サポートされるカレンダーシステム** (32 種類):
- グレゴリオ暦 (Gregorian)
- 中国農暦 (Chinese Lunar)
- 中国歴史暦法 (Chinese Historical) — 干支紀年、帝王年号
- イスラム暦 (Islamic)
- ヘブライ暦 (Hebrew)
- 日本暦 (Japanese)
- ペルシャ暦 (Persian)
- マヤ暦 (Mayan)
- 仏暦 (Buddhist)
- チベット暦 (Tibetan)
- 他 24 種類のカレンダー...

**使用例**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_lunar"
}
```

---

### 2. チャットツール (ChatTool)

**ツール名**: `chat`

**機能説明**: チャットセッションの管理とメッセージ送信。

**サポートされる操作**:
- `send_message` — メッセージの送信
- `get_messages` — 履歴メッセージの取得
- `create_group` — グループチャットの作成
- `add_member` — グループメンバーの追加
- `remove_member` — グループメンバーの削除
- `get_chat_info` — チャット情報の取得
- `terminate_chat` — チャットの終了（既読無視）

**使用例**:
```json
{
  "action": "send_message",
  "target_id": "being-uuid-or-user-0",
  "message": "こんにちは、協力しましょう！"
}
```

---

### 3. 設定ツール (ConfigTool)

**ツール名**: `config`

**機能説明**: システム設定の読み取りと変更。

**サポートされる操作**:
- `read` — 設定項目の読み取り
- `write` — 設定項目の書き込み
- `list` — すべての設定を一覧表示
- `get_ai_config` — AI クライアント設定の取得
- `set_ai_config` — AI クライアント設定の設定

**使用例**:
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

---

### 4. 主理人ツール (CuratorTool) 🔒

**ツール名**: `curator`

**権限要件**: シリコン主理人専用

**機能説明**: シリコン主理人専用のシステム管理ツール。

**サポートされる操作**:
- `create_being` — 新しいシリコン生命体を作成
- `list_beings` — すべてのシリコン生命体を一覧表示
- `get_being_info` — 生命体情報の取得
- `assign_task` — タスクの割り当て
- `manage_permissions` — 権限の管理

**使用例**:
```json
{
  "action": "create_being",
  "name": "アシスタント",
  "soul_file": "assistant_soul.md"
}
```

---

### 5. データベースツール (DatabaseTool)

**ツール名**: `database`

**機能説明**: 構造化データベースのクエリと操作。

**サポートされる操作**:
- `query` — データのクエリ
- `insert` — データの挿入
- `update` — データの更新
- `delete` — データの削除
- `create_table` — テーブルの作成
- `list_tables` — すべてのテーブルを一覧表示

**使用例**:
```json
{
  "action": "query",
  "table": "users",
  "conditions": {"status": "active"},
  "limit": 100
}
```

---

### 6. ディスクツール (DiskTool)

**ツール名**: `disk`

**機能説明**: ファイルシステム操作とローカル検索。

**サポートされる操作**:
- `read` — ファイルの読み取り
- `write` — ファイルの書き込み
- `list` — ディレクトリの一覧表示
- `delete` — ファイルの削除
- `create_directory` — ディレクトリの作成
- `search_files` — ファイルの検索
- `search_content` — ファイル内容の検索
- `count_lines` — 行数のカウント
- `read_lines` — 指定行の読み取り
- `replace_text` — テキストの置換

**権限要件**: `disk:read`, `disk:write`

**使用例**:
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

---

### 7. 動的コンパイルツール (DynamicCompileTool) 🔒

**ツール名**: `compile`

**機能説明**: C# コードの動的コンパイル（シリコン生命体の自己進化用）。

**サポートされる操作**:
- `compile_class` — クラスのコンパイル
- `compile_callback` — 権限コールバック関数のコンパイル
- `validate_code` — コードのセキュリティ検証

**セキュリティメカニズム**:
- コンパイル時の参照制御（危険なアセンブリを除外）
- 実行時の静的コードスキャン
- AES-256 暗号化ストレージ

**使用例**:
```json
{
  "action": "compile_class",
  "code": "public class MyBeing : SiliconBeingBase { ... }"
}
```

---

### 8. コード実行ツール (ExecuteCodeTool) 🔒

**ツール名**: `execute_code`

**権限要件**: シリコン主理人専用

**機能説明**: C# コードスニペットのコンパイルと実行。

**サポートされる操作**:
- `run_script` — コードスクリプトの実行

**使用例**:
```json
{
  "action": "run_script",
  "code": "return DateTime.Now.ToString();",
  "timeout": 30
}
```

---

### 9. ヘルプツール (HelpTool)

**ツール名**: `help`

**機能説明**: システムのヘルプドキュメントと使用ガイドの取得。

**サポートされる操作**:
- `get_topics` — ヘルプトピック一覧の取得
- `get_topic` — 特定トピックの詳細取得
- `search` — ヘルプドキュメントの検索

**使用例**:
```json
{
  "action": "get_topics"
}
```

---

### 10. ナレッジネットワークツール (KnowledgeTool)

**ツール名**: `knowledge`

**機能説明**: ナレッジグラフ操作（トリプル構造：主語-述語-目的語に基づく）。

**サポートされる操作**:
- `add` — ナレッジトリプルの追加
- `query` — ナレッジの照会
- `update` — ナレッジの更新
- `delete` — ナレッジの削除
- `search` — ナレッジの検索
- `get_path` — ナレッジパスの取得
- `validate` — ナレッジの検証
- `stats` — 統計情報の取得

**使用例**:
```json
{
  "action": "add",
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95
}
```

---

### 11. ログツール (LogTool)

**ツール名**: `log`

**機能説明**: 操作履歴と会話履歴のクエリ。

**サポートされる操作**:
- `query_logs` — システムログのクエリ
- `query_conversations` — 会話履歴のクエリ
- `get_stats` — ログ統計の取得

**使用例**:
```json
{
  "action": "query_logs",
  "being_id": "being-uuid",
  "start_time": "2026-04-20T00:00:00Z",
  "end_time": "2026-04-26T23:59:59Z",
  "level": "info"
}
```

---

### 12. 記憶ツール (MemoryTool)

**ツール名**: `memory`

**機能説明**: シリコン生命体の長期および短期記憶の管理。

**サポートされる操作**:
- `read` — 記憶の読み取り
- `write` — 記憶の書き込み
- `search` — 記憶の検索
- `delete` — 記憶の削除
- `list` — 記憶の一覧表示
- `get_stats` — 記憶統計の取得
- `compress` — 記憶の圧縮

**使用例**:
```json
{
  "action": "read",
  "key": "important_fact",
  "time_range": {
    "start": "2026-04-01",
    "end": "2026-04-26"
  }
}
```

---

### 13. ネットワークツール (NetworkTool)

**ツール名**: `network`

**機能説明**: HTTP/HTTPS リクエストの送信。

**サポートされる操作**:
- `get` — GET リクエスト
- `post` — POST リクエスト
- `put` — PUT リクエスト
- `delete` — DELETE リクエスト
- `download` — ファイルのダウンロード
- `upload` — ファイルのアップロード

**権限要件**: `network:http`

**使用例**:
```json
{
  "action": "get",
  "url": "https://api.example.com/data"
}
```

---

### 14. 権限ツール (PermissionTool) 🔒

**ツール名**: `permission`

**権限要件**: シリコン主理人専用

**機能説明**: 権限とアクセス制御リストの管理。

**サポートされる操作**:
- `query_permission` — 権限の照会
- `manage_acl` — グローバル ACL の管理
- `get_callback` — 権限コールバック関数の取得
- `set_callback` — 権限コールバック関数の設定

**使用例**:
```json
{
  "action": "manage_acl",
  "acl_action": "add_rule",
  "permission_type": "filesystem",
  "resource_prefix": "/data/",
  "acl_result": "allow"
}
```

---

### 15. プロジェクトツール (ProjectTool)

**ツール名**: `project`

**機能説明**: プロジェクトワークスペースの管理。

**サポートされる操作**:
- `create` — プロジェクトの作成
- `list` — プロジェクトの一覧表示
- `get_info` — プロジェクト情報の取得
- `update` — プロジェクトの更新
- `archive` — プロジェクトのアーカイブ

**使用例**:
```json
{
  "action": "create",
  "name": "My Project",
  "description": "プロジェクトの説明"
}
```

---

### 16. プロジェクトタスクツール (ProjectTaskTool)

**ツール名**: `project_task`

**機能説明**: プロジェクトタスクの管理。

**サポートされる操作**:
- `create` — タスクの作成
- `list` — タスクの一覧表示
- `update` — タスクの更新
- `complete` — タスクの完了
- `get_stats` — タスク統計の取得

**使用例**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "タスクの説明を完了",
  "priority": 5
}
```

---

### 17. プロジェクト作業ノートツール (ProjectWorkNoteTool)

**ツール名**: `project_work_note`

**機能説明**: プロジェクト作業ノートの管理（公開、作業手帳に類似）。

**サポートされる操作**:
- `create` — ノートの作成
- `read` — ノートの読み取り
- `update` — ノートの更新
- `delete` — ノートの削除
- `list` — ノートの一覧表示
- `search` — ノートの検索
- `directory` — 目次の生成

**使用例**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "ユーザー認証モジュール完了",
  "content": "## 実装詳細\n\n- JWT token使用",
  "keywords": "認証,JWT"
}
```

---

### 18. システムツール (SystemTool)

**ツール名**: `system`

**機能説明**: システム情報とリソース使用状況の取得。

**サポートされる操作**:
- `info` — システム情報の取得
- `resource_usage` — リソース使用状況の取得
- `find_process` — プロセスの検索
- `list_beings` — シリコン生命体の一覧表示

**使用例**:
```json
{
  "action": "info"
}
```

---

### 19. タスクツール (TaskTool)

**ツール名**: `task`

**機能説明**: シリコン生命体の個人タスク管理。

**サポートされる操作**:
- `create` — タスクの作成
- `list` — タスクの一覧表示
- `update` — タスクの更新
- `complete` — タスクの完了
- `delete` — タスクの削除
- `get_dependencies` — 依存関係の取得

**使用例**:
```json
{
  "action": "create",
  "description": "コードをレビュー",
  "priority": 5
}
```

---

### 20. タイマーツール (TimerTool)

**ツール名**: `timer`

**機能説明**: タイマーの作成と管理。

**サポートされる操作**:
- `create` — タイマーの作成
- `list` — タイマーの一覧表示
- `delete` — タイマーの削除
- `pause` — タイマーの一時停止
- `resume` — タイマーの再開
- `get_execution_history` — 実行履歴の取得

**使用例**:
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true,
  "message": "毎時リマインダー"
}
```

---

### 21. Token 監査ツール (TokenAuditTool) 🔒

**ツール名**: `token_audit`

**権限要件**: シリコン主理人専用

**機能説明**: AI token 使用状況の照会と集計。

**サポートされる操作**:
- `get_usage` — token 使用統計の取得
- `get_by_being` — 生命体別使用状況の取得
- `get_by_model` — モデル別使用状況の取得
- `get_trend` — 使用傾向の取得
- `export` — データのエクスポート

**使用例**:
```json
{
  "action": "get_usage",
  "start_date": "2026-04-01",
  "end_date": "2026-04-26"
}
```

---

### 22. WebView ブラウザツール (WebViewBrowserTool)

**ツール名**: `webview`

**機能説明**: Playwright ベースのブラウザ自動化操作。

**サポートされる操作**:
- `open_browser` — ブラウザを開く
- `close_browser` — ブラウザを閉じる
- `navigate` — URL にナビゲート
- `click` — 要素をクリック
- `input` — テキストを入力
- `get_page_text` — ページテキストを取得
- `get_screenshot` — スクリーンショットを取得
- `execute_script` — JavaScript を実行
- `wait_for_element` — 要素の出現を待機
- `get_browser_status` — ブラウザのステータスを取得

**特徴**:
- 各シリコン生命体の独立インスタンス
- 完全に分離された Cookie とセッション
- ユーザーには完全に不可視（ヘッドレスモード）
- 完全な JavaScript と CSS サポート

**使用例**:
```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

---

### 23. 作業ノートツール (WorkNoteTool)

**ツール名**: `work_note`

**機能説明**: シリコン生命体の個人作業ノート管理（非公開、日記帳に類似）。

**サポートされる操作**:
- `create` — ノートの作成
- `read` — ノートの読み取り
- `update` — ノートの更新
- `delete` — ノートの削除
- `list` — ノートの一覧表示
- `search` — ノートの検索
- `directory` — 目次の生成

**使用例**:
```json
{
  "action": "create",
  "summary": "ユーザー認証モジュール完了",
  "content": "## 実装詳細\n\n- JWT token使用\n- OAuth2対応",
  "keywords": "認証,JWT,OAuth2"
}
```

---

### 24. ホットリロードツール (HotReloadTool)

**ツール名**: `hot_reload`

**機能説明**: SiliconLife.Fast が実行中に自動的にコンパイル、ファイル更新および再起動をサポート、手動介入不要。

**サポートされる操作**:
- `execute` — 全体のビルド、コピーおよび再起動プロセスを実行
- `build_only` — プロジェクトのビルドのみ実行、コピーおよび再起動は行わない

**ワークフロー**:
1. SiliconLife.Fast プロジェクトをコンパイル
2. 現在実行中の Fast インスタンスを正常に終了（HTTP API 経由）
3. プロセスの終了とポートの解放を待機
4. ビルド出力を対象ディレクトリにコピー（HotReload 自身のファイルは除外）
5. Fast インスタンスを再起動

**特徴**:
- 旧プロセスの自動検出と終了
- 安全なファイルコピー（HotReload.exe を上書きしない）
- ポート解放待機メカニズム
- カスタムポート設定をサポート

**使用例**:
```json
{
  "action": "execute",
  "project_path": "src/SiliconLife.Fast",
  "source_path": "src/SiliconLife.Fast/bin/Debug/net9.0",
  "configuration": "Debug",
  "port": 8080
}
```

**パラメータ説明**:
- `project_path`: プロジェクトパス（ソリューションルートディレクトリ基準）
- `source_path`: ビルド出力ディレクトリ
- `configuration`: ビルド構成（Debug/Release）
- `port`: Fast インスタンスの Web ポート（デフォルト 8080）

**注意事項**:
- SiliconLife.Fast バージョンのみ対応
- tools/HotReload ディレクトリに HotReload.exe が必要
- 再起動过程中に短いサービス中断がある（約 3-5 秒）

---

## ツール呼び出しフロー

```
┌──────────┐
│   AI     │ tool_calls を返す
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ ツールの検索と使用権限の検証
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ 権限チェーンをチェック
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ リソースアクセス操作を実行
└────┬─────────┘
     ↓
┌──────────┐
│   AI     │ ツール結果を受信し、思考を継続
└──────────┘
```

## 権限検証

すべてのツール実行は 5 段階権限チェーンを通過します：

1. **IsCurator** — シリコン主理人はすべてのチェックをバイパス
2. **UserFrequencyCache** — ユーザーの高頻度許可/拒否キャッシュ
3. **GlobalACL** — グローバルアクセス制御リスト
4. **IPermissionCallback** — カスタム権限コールバック関数
5. **IPermissionAskHandler** — ユーザーに確認

## カスタムツールの作成

### ステップ 1: ITool インターフェースを実装

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    
    public string Description => "ツールの説明";
    
    public ToolDefinition Definition => new ToolDefinition
    {
        Name = Name,
        Description = Description,
        Parameters = new Dictionary<string, object>
        {
            ["param1"] = new { type = "string", description = "パラメータの説明" }
        }
    };
    
    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        try
        {
            var param1 = call.Parameters["param1"]?.ToString();
            var result = await DoWork(param1);
            
            return new ToolResult
            {
                Success = true,
                Output = result
            };
        }
        catch (Exception ex)
        {
            return new ToolResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }
}
```

### ステップ 2: プロジェクトに追加

ツールファイルを `src/SiliconLife.Common/Tools/` ディレクトリ（共有ツール）または `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` ディレクトリ（バージョン固有ツール）に配置。`ToolManager` は起動時にリフレクションを介して自動的に検出および登録。

### ステップ 2a: プラグイン経由でツールを登録

プラグインシステムを介してカスタムツールを登録することも可能：

1. プラグインプロジェクトで `ITool` インターフェースを実装
2. プラグイン DLL をコンパイルしてプラグインディレクトリに配置
3. `ToolManager.ScanAllPluginAssemblies()` がロード済みのすべてのプラグイン内の ITool 実装を自動的にスキャン
4. プラグインツールは同じ権限システムの制約を受ける

### ステップ 3: （オプション）主理人専用としてマーク

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // シリコン主理人のみアクセス可能
}
```

## ベストプラクティス

### 1. 常にパラメータを検証

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("必須パラメータが不足: required_param");
}
```

### 2. エラーを適切に処理

```csharp
try
{
    // 操作を実行
}
catch (Exception ex)
{
    Logger.Error($"ツール {Name} の実行に失敗: {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. 権限システムを尊重

権限チェックをバイパスしないでください。常にエグゼキューターを介してリソースにアクセス：

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return ToolResult.Denied(permission.Reason);
}
```

### 4. 明確なツール説明を提供

AI がいつどのようにツールを使用するかを理解できるようにします：

```csharp
public string Description => 
    "異なるカレンダーシステム間で日付を変換。" +
    "'date'、'from_calendar'、'to_calendar' パラメータが必要です。";
```

## トラブルシューティング

### ツールが見つからない

**問題**: AI が存在しないツールを呼び出そうとした。

**解決策**:
- ツール名が完全に一致しているか確認
- ツールファイルが `Tools/` ディレクトリにあるか検証
- プロジェクトを再ビルド (`dotnet build`)

### 権限が拒否された

**問題**: ツールの実行が失敗し、権限エラーが返された。

**解決策**:
- 権限監査ログを確認
- シリコン生命体に必要な権限があるか検証
- グローバル ACL 設定を確認
- 主理人の場合、`[SiliconManagerOnly]` マークが使用されているか確認

### ツールの実行がエラーを返した

**問題**: ツールは実行されたが、失敗結果が返された。

**解決策**:
- ツールが返すエラーメッセージを確認
- 入力パラメータのフォーマットが正しいか検証
- 詳細なエラー情報についてシステムログを確認
- ツール機能を個別にテスト

## 次のステップ

- 📚 [アーキテクチャガイド](architecture.md)を読む
- 🛠️ [開発ガイド](development-guide.md)を確認
- 🔒 [権限システム](permission-system.md)を理解
- 🚀 [クイックスタートガイド](getting-started.md)で始める
