# ツールリファレンス

> **バージョン: v0.2.0-alpha**

本ドキュメントでは、Silicon Life Collective プラットフォームのすべての組み込みツールについて詳しく説明します。

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | **日本語** | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md) | [Русский](../ru-RU/tools-reference.md)

## 概要

ツールシステムは、シリコンビーイングが標準化されたインターフェースを介して外部世界と対話できるようにします。各ツールは `ITool` インターフェースを実装し、ツールマネージャーがリフレクションを介して自動的に検出および登録します。

### ツール分類

- **システム管理ツール** — 設定、権限、動的コンパイル、キュレーター管理
- **通信ツール** — チャット、ネットワークリクエスト
- **データストレージツール** — ディスク操作、データベース、メモリ、作業ノート
- **時間管理ツール** — カレンダー、タイマー、タスク
- **開発ツール** — コード実行、ログクエリ
- **ユーティリティツール** — システム情報、トークン使用監査、ヘルプドキュメント、ナレッジネットワーク
- **ブラウザツール** — WebView ブラウザ自動化
- **プロジェクトツール** — プロジェクト管理、プロジェクトタスク、プロジェクト作業ノート、プロジェクト作業
- **プラグインツール** — プラグインシステムを介して登録されたサードパーティツール

### ツールシナリオシステム

各ツールは `[ToolScenario]` 属性を通じて使用可能なシナリオを宣言します：

| シナリオフラグ | 値 | 説明 |
|----------|------|-------------|
| `Chat` | `1 << 0` | チャットシナリオ（ユーザーとシリコンビーイングが対話する時） |
| `Task` | `1 << 1` | タスクシナリオ（シリコンビーイングがタスクを実行する時） |
| `Timer` | `1 << 2` | タイマーシナリオ（シリコンビーイングが定期タスクを実行する時） |
| `MemoryCompression` | `1 << 3` | メモリ圧縮シナリオ |
| `Project` | `1 << 4` | プロジェクトシナリオ（ThinkOnProject モード） |
| `All` | 上記すべて | すべてのシナリオで使用可能 |

また、`[ChatOnly]` 属性でマークされたツールはチャットシナリオでのみ使用可能（HelpTool など）、タスクやタイマーシナリオでは表示されません。

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

### 4. キュレーターツール (CuratorTool) 🔒

**ツール名**: `silicon_manager`

**権限要件**: シリコンキュレーター専用（`[SiliconManagerOnly]`）

**使用可能シナリオ**: Chat、Task、Timer

**機能説明**: シリコンキュレーター専用のシステム管理ツール。シリコンビーイングの作成、確認、リセットを管理します。

**サポートされる操作**:
- `list_beings` — すべてのシリコンビーイングとその状態を一覧表示
- `create_being` — 新しいシリコンビーイングを作成（`name` と `soul` パラメータが必要）
- `get_code` — シリコンビーイングのカスタムソースコードを確認
- `reset` — シリコンビーイングをデフォルト実装にリセット

**使用例**:
```json
{
  "action": "create_being",
  "name": "アシスタント",
  "soul": "あなたは役立つアシスタントです..."
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

**権限要件**: `FileAccess`

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

**機能説明**: C# コードの動的コンパイル（シリコンビーイングの自己進化用）。

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

**権限要件**: シリコンキュレーター専用

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

**使用可能シナリオ**: Chat（`[ChatOnly]`、チャットシナリオでのみ使用可能）

**機能説明**: システムヘルプドキュメントの検索と取得。AI がシステム機能の使用方法を照会できるようにします。

**サポートされる操作**:
- `list` — すべてのヘルプトピック ID を一覧表示
- `search` — キーワードでヘルプドキュメントを検索
- `get` — 指定 ID のヘルプドキュメント内容を取得

**使用例**:
```json
{
  "action": "search",
  "keyword": "権限"
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

### 11. MCP クエリツール (McpTool)

**ツール名**: `mcp`

**機能説明**: MCP（Model Context Protocol）統合状態のクエリ —— 接続済みの外部サーバー、それらが提供するツール、および呼び出し方法。これは読み取り専用ツールです：サーバーの追加/削除はユーザーが Web UI でのみ実行可能で、AI はサーバーリストを変更できません。

**サポートされる操作**:
- `status` — グローバル概要（有効状態、サーバー数、ツール数）
- `list_servers` — 設定済みサーバーの一覧（接続状態とツール数を含む）
- `list_tools` — 利用可能ツールの一覧（`mcp_{server}_{tool}` プレフィックス名、説明、パラメータ schema 付き。オプション `server_id` で単一サーバーをフィルタリング）

**使用例**:
```json
{
  "action": "list_tools",
  "server_id": "filesystem",
  "include_schema": true
}
```

**MCP ラップツール**: 各接続済み MCP サーバーが提供するツールは、独立したツールとしてシリコンビーイングに動的に登録され、命名形式は `mcp_{serverId}_{toolName}`（例：`mcp_filesystem_read_file`）。AI は通常のツールと同様にプレフィックス名で直接呼び出し可能で、本クエリツールを経由する必要はありません。ラップツールはパーミッションマトリクスで単一の `execute` アクションとして表現され、個別に無効化できます。

**シナリオ**: 全シナリオ（`All`）

---

### 12. ログツール (LogTool)

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

### 13. メモリツール (MemoryTool)

**ツール名**: `memory`

**機能説明**: シリコンビーイングの長期および短期メモリの管理。

**サポートされる操作**:
- `read` — メモリの読み取り
- `write` — メモリの書き込み
- `search` — メモリの検索
- `delete` — メモリの削除
- `list` — メモリの一覧表示
- `get_stats` — メモリ統計の取得
- `compress` — メモリの圧縮

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

### 14. ネットワークツール (NetworkTool)

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

### 15. 権限ツール (PermissionTool) 🔒

**ツール名**: `permission`

**権限要件**: シリコンキュレーター専用

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

### 16. プロジェクトツール (ProjectTool) 🔒

**ツール名**: `project`

**権限要件**: シリコンキュレーター専用（`[SiliconManagerOnly]`）

**使用可能シナリオ**: Chat、Task、Timer

**機能説明**: プロジェクトワークスペースの管理。プロジェクトライフサイクル管理、メンバー割り当て、ロール管理をサポートします。

**サポートされる操作**:
- `create` — 新しいプロジェクトスペースを作成
- `archive` — プロジェクトをアーカイブ
- `restore` — アーカイブされたプロジェクトを復元
- `destroy` — プロジェクトを破棄しデータをクリーンアップ（元に戻せない）
- `list` — すべてのプロジェクトを一覧表示
- `get` — プロジェクト詳細の取得
- `assign` — シリコンビーイングをプロジェクトに割り当て
- `remove` — シリコンビーイングをプロジェクトから削除
- `update` — プロジェクト名/説明の更新
- `list-workflow-templates` — 利用可能なワークフローテンプレートを一覧表示
- `assign_role` — シリコンビーイングにプロジェクトロールを割り当て
- `remove_role` — シリコンビーイングのプロジェクトロールを削除
- `list_roles` — プロジェクトのロール割り当てを一覧表示

**使用例**:
```json
{
  "action": "create",
  "name": "My Project",
  "description": "プロジェクトの説明"
}
```

---

### 17. プロジェクトタスクツール (ProjectTaskTool)

**ツール名**: `project_task`

**使用可能シナリオ**: Chat、Task、Timer

**機能説明**: プロジェクトスペース内のタスク管理。完全なタスクライフサイクルをサポートします。

**サポートされる操作**:
- `create` — プロジェクトタスクの作成
- `list` — プロジェクトタスクの一覧表示
- `get` — タスク詳細の取得
- `update` — タスクのタイトル/説明/優先度の更新
- `assign` — タスクに担当者を割り当て
- `remove_assignee` — タスク担当者の削除
- `start` — タスクの開始
- `complete` — タスクの完了マーク
- `fail` — タスクの失敗マーク
- `cancel` — タスクのキャンセル
- `delete` — タスクの削除
- `stats` — タスク統計の取得

**使用例**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "タスク説明を完了",
  "priority": 5
}
```

---

### 18. プロジェクト作業ノートツール (ProjectWorkNoteTool)

**ツール名**: `project_work_note`

**使用可能シナリオ**: Chat、Task、Timer

**機能説明**: プロジェクトスペース内の作業ノート管理（公開、作業手帳に類似）。ページ式ノート管理をサポートします。

**サポートされる操作**:
- `create` — ノートページの作成（`project_id`、`summary`、`content` が必要、`keywords` はオプション）
- `read` — ノートページの読み取り（`project_id` と `page_number` または `note_id` が必要）
- `update` — ノートページの更新（`project_id`、`page_number`、`content` が必要、`summary` と `keywords` はオプション）
- `delete` — ノートページの削除（`project_id` と `page_number` または `note_id` が必要）
- `list` — プロジェクトのすべてのノートページサマリーを一覧表示
- `directory` — ノート目次/概要の生成
- `search` — キーワードでノートを検索（`project_id` と `keyword` が必要、`max_results` はオプション）

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

### 19. プロジェクト作業ツール (ProjectWorkTool) 🔒

**ツール名**: `project_work`

**権限要件**: シリコンキュレーター専用（`[SiliconManagerOnly]`）

**使用可能シナリオ**: Project（`[ToolScenario(ToolScenarioFlag.Project)]`、プロジェクトシナリオでのみ使用可能）

**機能説明**: プロジェクト作業操作ツール。キュレーターが ThinkOnProject シナリオでプロジェクトワークフローを管理するために使用します。

**サポートされる操作**:
- `create-task` — プロジェクトタスクの作成
- `assign-task` — タスクにシリコンビーイングを割り当て
- `chat` — プロジェクトグループチャットにメッセージを送信
- `broadcast` — プロジェクトチャンネルにメッセージをブロードキャスト
- `complete` — プロジェクトを完了マーク
- `status` — プロジェクトステータスの取得

**使用例**:
```json
{
  "action": "create-task",
  "project_id": "project-uuid",
  "title": "ユーザー認証の実装"
}
```

---

### 20. スキルツール (SkillTool)

**ツール名**: `skill`

**機能説明**: シリコンビーイングのスキル（再利用可能な「ツールオーケストレーション + プロンプトテンプレート」能力ユニット）を管理。作成、一覧、更新、削除、インポート/エクスポートをサポート。欠落したメタデータ（id、説明、パラメータ schema など）は AI が自動補完します。

**サポートされる操作**:
- `create` — 新規スキル作成（`id` と `system_prompt` 必須、オプション `description`、`parameter_schema`、`tool_whitelist`、`tags`、`max_tool_round`、`timeout`、`on_complete`、`trigger_mode`、`auto_trigger_condition`）
- `list` — 利用可能な全スキルの一覧（サマリー付き）
- `update` — パラメータによる既存スキルの更新（`skill_id` 必須）
- `update_from_md` — Markdown 文字列からのスキル更新（YAML フロントメタデータ + プロンプト本文）
- `delete` — スキルの削除（`skill_id` 必須）
- `export` — スキルを JSON でエクスポート（`skill_id` 必須）
- `export_md` — スキルを Markdown でエクスポート（`skill_id` 必須）
- `import` — JSON からスキルをインポート（`json` 必須）
- `import_md` — Markdown からスキルをインポート（`markdown` 必須）

**使用例**:
```json
{
  "action": "create",
  "id": "daily_news_digest",
  "description": "今日のテクノロジーニュースを検索して要約を生成",
  "system_prompt": "network ツールを使用して {topic} の最新ニュースを検索し、500 字の要約を生成してください。",
  "parameter_schema": {
    "type": "object",
    "properties": {
      "topic": { "type": "string", "description": "ニューストピック" }
    },
    "required": ["topic"]
  },
  "tool_whitelist": ["network", "work_note"],
  "trigger_mode": "Auto",
  "auto_trigger_condition": "schedule",
  "metadata": { "schedule": "0 9 * * *" }
}
```

**変更パーミッション**: シリコンキュレーターは全スキルを変更可能。通常ビーイングはソースが `Being` または `User` のスキルのみ変更可能（組み込み・プラグインスキルは変更不可）。

**数量制限**: 各ビーイングのカスタムスキル数は設定 `MaxCustomSkillsPerBeing`（デフォルト 50）で制限されます。

**シナリオ**: 全シナリオ（`All`）

> スキルシステム（トリガーモード、ホワイトリスト、ホットリロード、自動スケジュールなど）の完全な説明については、[シリコンビーイングガイド](silicon-being-guide.md#スキルシステム)を参照してください。

---

### 21. システムツール (SystemTool)

**ツール名**: `system`

**機能説明**: システム情報とリソース使用状況の取得。

**サポートされる操作**:
- `info` — システム情報の取得
- `resource_usage` — リソース使用状況の取得
- `find_process` — プロセスの検索
- `list_beings` — シリコンビーイングの一覧表示

**使用例**:
```json
{
  "action": "info"
}
```

---

### 22. タスクツール (TaskTool)

**ツール名**: `task`

**機能説明**: シリコンビーイングの個人タスク管理。

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
  "description": "コードレビュー",
  "priority": 5
}
```

---

### 23. タイマーツール (TimerTool)

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

### 24. トークン使用監査ツール (TokenAuditTool) 🔒

**ツール名**: `token_audit`

**権限要件**: シリコンキュレーター専用（`[SiliconManagerOnly]`）

**使用可能シナリオ**: Chat、Task、Timer

**機能説明**: AI トークン使用統計と傾向データの照会。

**サポートされる操作**:
- `summary` — トークン使用サマリー統計の取得
- `trend` — トークン使用傾向データポイントの取得

**サポートされる時間範囲**:
- `today` — 直近 24 時間
- `week` — 直近 7×24 時間
- `month` — 日別統計
- `year` — 月別統計

**使用例**:
```json
{
  "action": "summary",
  "time_range": "week"
}
```

---

### 25. WebView ブラウザツール (WebViewBrowserTool)

**ツール名**: `webview_browser`

**使用可能シナリオ**: Chat、Task、Timer

**機能説明**: Playwright ベースのブラウザ自動化操作。完全なウェブナビゲーション、インタラクション、データ抽出機能を提供します。

**サポートされる操作**:
- `open` — ブラウザを開く
- `close` — ブラウザを閉じる
- `navigate` — URL にナビゲート
- `click` — 要素をクリック
- `input` — テキストを入力
- `scroll` — ページをスクロール
- `execute_script` — JavaScript を実行
- `get_page_text` — ページテキストを取得
- `get_screenshot` — スクリーンショットを取得
- `wait_for_element` — 要素の出現を待機
- `get_element_info` — 要素情報を取得
- `upload_file` — ファイルをアップロード
- `get_browser_status` — ブラウザのステータスを取得
- `set_timeout` — タイムアウト時間の設定
- `clear_session` — ブラウザセッションのクリア

**特徴**:
- 各シリコンビーイングの独立インスタンス
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

### 26. 作業ノートツール (WorkNoteTool)

**ツール名**: `work_note`

**機能説明**: シリコンビーイングの個人作業ノート管理（非公開、日記帳に類似）。

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

すべてのツール実行は権限検証チェーンを通過します：

1. **UserFrequencyCache** — ユーザー高頻度キャッシュ（HighDeny が HighAllow より優先）
2. **IPermissionCallback** — カスタム権限コールバック関数（Allowed/Denied/AskUser）
3. **IsCurator 分岐** — キュレーターはパーミッション問合せハンドラーを介してユーザーに確認；非キュレーターはグローバル ACL を照会し、一致するルールがない場合はデフォルトで拒否

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

ツールファイルを `src/SiliconLife.Common/Tools/` ディレクトリ（共有ツール）または `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` ディレクトリ（バージョン固有ツール）に配置します。ツールマネージャーは起動時にリフレクションを介して自動的に検出および登録します。

### ステップ 2a: プラグイン経由でツールを登録

プラグインシステムを介してカスタムツールを登録することも可能です：

1. プラグインプロジェクトで `ITool` インターフェースを実装
2. プラグイン DLL をコンパイルしてプラグインディレクトリに配置
3. `ToolManager.ScanAllPluginAssemblies()` がロード済みのすべてのプラグイン内の ITool 実装を自動的にスキャン
4. プラグインツールは同じ権限システムの制約を受ける

### ステップ 3: （オプション）キュレーター専用としてマーク

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // シリコンキュレーターのみアクセス可能
}
```

### 代替案：スキルと MCP ツール

C# ツールクラスを作成する以外に、コンパイル不要の拡張方法が 2 つあります：

- **スキル（Skill）**：Web UI または `skill` ツールで「ツールオーケストレーション + プロンプトテンプレート」を作成。よく使うワークフローを再利用可能な能力としてカプセル化するのに適しています。[シリコンビーイングガイド — スキルシステム](silicon-being-guide.md#スキルシステム)を参照してください。
- **MCP サーバー**：Web UI で外部 MCP サーバーを設定すると、そのツールが `mcp_{serverId}_{toolName}` 形式で自動注入され、コード記述不要です。[Web UI ガイド — MCP 管理](web-ui-guide.md)を参照してください。

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

権限チェックをバイパスしないでください。常にエグゼキューターを介してリソースにアクセスします：

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return ToolResult.Denied("Permission denied");
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
- シリコンビーイングに必要な権限があるか検証
- グローバル ACL 設定を確認
- キュレーターの場合、`[SiliconManagerOnly]` マークが使用されているか確認

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
