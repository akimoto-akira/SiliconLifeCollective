# API リファレンス

> **バージョン：v0.2.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | **日本語** | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md) | [Русский](../ru-RU/api-reference.md)

## Web API エンドポイント

ベース URL：`http://localhost:8080`

### 認証

ほとんどのエンドポイントは、Web UI によって管理されるセッションクッキーを介した認証が必要です。システム初期化前は、ヘルプページを除くすべてのリクエストが初期化ページにリダイレクトされます。

---

## ダッシュボード

### ダッシュボード統計データを取得

**GET** `/api/dashboard/stats`

システム概要データ（ビーイング数、稼働状況など）を返します。

### パフォーマンスメトリクスを取得

**GET** `/api/dashboard/metrics`

リアルタイムのパフォーマンスメトリクスデータを返します。

---

## チャットシステム

### チャットページ

**GET** `/chat`

チャットインターフェースのページを返します。

### ストリームチャット（SSE）

**GET** `/api/chat/stream`

サーバー送信イベント（SSE）によるストリームチャット。

**レスポンス**：Server-Sent Events ストリーム

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### セッションリストを取得

**GET** `/api/chat/conversations`

すべてのアクティブなチャットセッションのリストを返します。

**レスポンス例**：
```json
{
  "conversations": [
    {
      "sessionId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
      "beingId": "being-uuid",
      "type": "single",
      "displayName": "小遊とのチャット",
      "lastMessage": "最後のメッセージ内容",
      "lastTime": "2026-05-20T10:30:00Z"
    }
  ]
}
```

### メッセージ履歴を取得

**GET** `/api/chat/messages`

クエリパラメータ：`channelId` — チャンネル/セッション ID

指定されたセッションのメッセージ履歴を返します。

### チャット履歴を取得

**GET** `/api/chat/history`

グローバルチャット履歴を返します。

### メッセージを送信

**POST** `/api/chat/send`

**リクエストボディ**：
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
  "content": "テストメッセージ内容"
}
```

**レスポンス**：
```json
{
  "success": true,
  "messageId": "50156b26-f3b9-4735-be3d-51e547bd3a4a"
}
```

### AI 思考を停止

**POST** `/api/chat/stop`

現在進行中の AI レスポンス生成を停止します。

**リクエストボディ**：
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d"
}
```

### ファイルをアップロード

**POST** `/api/chat/upload`

チャットセッションにファイルをアップロードします（multipart/form-data 対応）。

---

## シリコンビーイング管理

### ビーイング管理ページ

**GET** `/beings`

シリコンビーイング管理インターフェースのページを返します。

### ビーイングリストを取得

**GET** `/api/beings` または **GET** `/api/beings/list`

すべての登録済みシリコンビーイングのリストを返します。

**レスポンス例**：
```json
{
  "beings": [
    {
      "id": "being-uuid",
      "name": "Assistant",
      "status": "running",
      "soulPath": "path/to/soul.md"
    }
  ]
}
```

**ステータス値**：`idle` | `running` | `waiting_permission` | `stopped`

### ビーイング詳細を取得

**GET** `/api/beings/detail`

クエリパラメータ：`beingId` — ビーイング ID

指定されたビーイングの詳細情報を返します。

### ビーイング活動状態を取得

**GET** `/api/beings/activity`

各ビーイングの活動状態情報を返します。

### ソウルファイルエディタページ

**GET** `/beings/soul`

ソウルファイルエディタインターフェースを返します。

### ソウルファイルを保存

**POST** `/api/beings/soul/save`

**リクエストボディ**：
```json
{
  "beingId": "being-uuid",
  "soulContent": "# Personality\nYou are helpful..."
}
```

### AI 設定エディタページ

**GET** `/beings/ai-config`

AI 設定エディタインターフェースを返します。

### AI 設定を保存

**POST** `/api/beings/ai-config/save`

**リクエストボディ**：
```json
{
  "beingId": "being-uuid",
  "aiClientType": "DashScope",
  "config": {
    "apiKey": "...",
    "region": "beijing",
    "model": "qwen3.6-plus"
  }
}
```

### 利用可能な AI モデルリストを取得

**GET** `/api/beings/ai-config/models`

クエリパラメータ：`clientType`, `apiKey`, `region`

指定された AI クライアントの利用可能なモデルリストを返します。

---

## チャット履歴閲覧

### チャット履歴ページ

**GET** `/chat-history`

チャット履歴メインページを返します。

### チャット履歴詳細ページ

**GET** `/chat-history-detail`

指定されたセッションのチャット履歴詳細ページを返します。

### グループチャット履歴詳細ページ

**GET** `/group-chat-history-detail`

グループチャットセッションの履歴詳細ページを返します。

### ブロードキャスト履歴詳細ページ

**GET** `/broadcast-history-detail`

ブロードキャストチャンネルの履歴詳細ページを返します。

### 履歴セッションリストを取得

**GET** `/api/chat-history/conversations`

すべての履歴セッションリストを返します。

### 履歴メッセージを取得

**GET** `/api/chat-history/messages`

クエリパラメータ：`sessionId` — セッション ID

指定された履歴セッションのメッセージ記録を返します。

---

## タイマー管理

### タイマーページ

**GET** `/timers`

タイマー管理インターフェースのページを返します。

### タイマーリストを取得

**GET** `/api/timers/list`

すべてのタイマーのリストを返します。

### タイマーサイクル詳細ページ

**GET** `/timer-cycles/{timerId}`

指定されたタイマーの実行サイクル詳細ページを返します。

### タイマーサイクルリストを取得

**GET** `/api/timer-cycles/list`

クエリパラメータ：`timerId` — タイマー ID

指定されたタイマーのすべての実行サイクルリストを返します。

### 単一実行サイクル詳細ページ

**GET** `/timer-cycle/{cycleIndex}`

単一実行の詳細ページを返します。

### サイクルメッセージを取得

**GET** `/api/timer-cycle/messages`

クエリパラメータ：`cycleIndex` — サイクルインデックス

指定された実行サイクルの関連メッセージを返します。

---

## タスク管理

### タスクページ

**GET** `/tasks`

タスク管理インターフェースのページを返します。

### タスクリストを取得

**GET** `/api/tasks/list`

すべてのタスクのリストを返します。

### タスクサイクル詳細ページ

**GET** `/task-cycles/{taskId}`

指定されたタスクの実行サイクル詳細ページを返します。

### タスクサイクルリストを取得

**GET** `/api/task-cycles/list`

クエリパラメータ：`taskId` — タスク ID

指定されたタスクのすべての実行サイクルリストを返します。

### 単一実行サイクル詳細ページ

**GET** `/task-cycle/{cycleIndex}`

単一タスク実行の詳細ページを返します。

### サイクルメッセージを取得

**GET** `/api/task-cycle/messages`

クエリパラメータ：`cycleIndex` — サイクルインデックス

指定されたタスク実行サイクルの関連メッセージを返します。

---

## パーミッションシステム

### パーミッション管理ページ

**GET** `/permissions`

パーミッション管理インターフェースのページを返します。

### パーミッションルールリストを取得

**GET** `/api/permissions/list`

現在設定されているすべてのパーミッションルールを返します。

**レスポンス例**：
```json
{
  "rules": [
    {
      "permissionType": "NetworkAccess",
      "resourcePrefix": "api.github.com",
      "result": "Allowed",
      "description": "Allow GitHub API access"
    }
  ]
}
```

### パーミッションルールを保存

**POST** `/api/permissions/save`

**リクエストボディ**：
```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects",
  "result": "Allowed",
  "description": "Allow project directory access"
}
```

### パーミッションリクエストページ

**GET** `/permission/request`

パーミッションリクエストページを表示し、ユーザーがシリコンビーイングのパーミッションリクエストを承認または拒否できるようにします。

**クエリパラメータ**：

| パラメータ | タイプ | 説明 |
|------|------|------|
| `userId` | `Guid` | パーミッションをリクエストするシリコンビーイング ID |
| `type` | `string` | パーミッションタイプ |
| `resource` | `string` | リクエストするリソースパス |
| `allowCode` | `string` | 許可操作のコード識別子 |
| `denyCode` | `string` | 拒否操作のコード識別子 |

### 保留中のパーミッションリクエストを確認

**GET** `/permission/check`

クエリパラメータ：`userId` — シリコンビーイング ID

**レスポンス**：
```json
{
  "pending": true
}
```

### パーミッションリクエストに応答

**GET** `/permission/respond`

**クエリパラメータ**：

| パラメータ | タイプ | 説明 |
|------|------|------|
| `userId` | `Guid` | シリコンビーイング ID |
| `allowed` | `bool` | 許可するかどうか |
| `addToCache` | `bool` | 決定をキャッシュするかどうか |
| `cacheDuration` | `double` | キャッシュ期間（時間） |

**レスポンス**：
```json
{
  "success": true
}
```

---

## ロギングシステム

### ログページ

**GET** `/logs`

ログ閲覧インターフェースのページを返します。

### ログリストを取得

**GET** `/api/logs/list`

クエリパラメータでレベル、時間範囲によるフィルタリングをサポートします。

**レスポンス例**：
```json
{
  "logs": [
    {
      "timestamp": "2026-04-20T10:30:00Z",
      "level": "error",
      "message": "Failed to connect to AI service",
      "source": "OllamaClient"
    }
  ]
}
```

### ビーイング別ロググループを取得

**GET** `/api/logs/beings`

シリコンビーイング別にグループ化されたログ統計を返します。

### 利用可能なログレベルを取得

**GET** `/api/logs/levels`

システムで利用可能なログレベルのリストを返します。

---

## 使用統計

### 使用統計ページ

**GET** `/usage`

使用統計インターフェースのページを返します。

### 使用サマリーを取得

**GET** `/api/usage/summary`

トークン使用量と費用のサマリーを返します。

### トレンドデータを取得

**GET** `/api/usage/trend`

クエリパラメータ：`startDate`, `endDate`

指定された期間の使用トレンドデータを返します。

### 使用データをエクスポート

**GET** `/api/usage/export`

使用データをダウンロード可能な形式でエクスポートします。

---

## 監査トレイル

### 監査ページ

**GET** `/audit`

監査トレイルインターフェースのページを返します。

### 監査リストを取得

**GET** `/api/audit/list`

監査ログエントリのリストを返します。

### 監査サマリーを取得

**GET** `/api/audit/summary`

監査データの集計統計を返します。

### ビーイング別監査グループを取得

**GET** `/api/audit/beings`

シリコンビーイング別にグループ化された監査統計を返します。

---

## 設定管理

### 設定ページ

**GET** `/config`

システム設定インターフェースのページを返します。

### 設定を保存

**POST** `/config/save`

**リクエストボディ**：
```json
{
  "language": "ZhCN",
  "port": 8080,
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:7b"
    },
    "DashScope": {
      "apiKey": "...",
      "region": "beijing",
      "model": "qwen3.6-plus"
    },
    "VolcengineArk": {
      "apiKey": "...",
      "endpoint": "...",
      "model": "..."
    },
    "Herdsman": {
      "endpoint": "http://localhost:8000",
      "model": "..."
    },
    "LongCat": {
      "apiKey": "...",
      "endpoint": "...",
      "model": "..."
    },
    "QiniuAI": {
      "apiKey": "...",
      "endpoint": "...",
      "model": "..."
    },
    "DeepSeek": {
      "apiKey": "...",
      "endpoint": "https://api.deepseek.com",
      "model": "..."
    },
    "Zhipu": {
      "apiKey": "...",
      "endpoint": "https://open.bigmodel.cn/api/paas/v4",
      "model": "..."
    },
    "Ernie": {
      "apiKey": "...",
      "endpoint": "https://qianfan.baidubce.com/v2",
      "model": "..."
    },
    "Hunyuan": {
      "apiKey": "...",
      "model": "hy3"
    },
    "MiniMax": {
      "apiKey": "...",
      "model": "..."
    },
    "Moonshot": {
      "apiKey": "...",
      "endpoint": "https://api.moonshot.cn/v1",
      "model": "..."
    },
    "SiliconFlow": {
      "apiKey": "...",
      "endpoint": "https://api.siliconflow.cn/v1",
      "model": "..."
    }
  }
}
```

### AI 設定オプションを取得

**GET** `/config/aioptions`

利用可能な AI クライアントタイプとその動的オプション（利用可能なモデル、リージョンなど）を返します。

### IM プラットフォームオプションを取得

**GET** `/config/imoptions`

IM プラットフォームのメタデータを返します（設定ウィザードがフォームを動的にレンダリングするために使用）：

```json
{
  "success": true,
  "platforms": [
    {
      "value": "feishu",
      "display": "Feishu",
      "authModes": ["manual", "oauth"],
      "needsPublicCallback": false,
      "help": "...",
      "helpUrl": "https://open.feishu.cn/app",
      "fields": [
        { "key": "appId", "label": "App ID", "type": "text", "required": true },
        { "key": "appSecret", "label": "App Secret", "type": "password", "required": true, "isSecret": true }
      ]
    }
  ]
}
```

### 設定を参照

**GET** `/config/browse`

設定項目の参照データを返します（設定インターフェースのグループ表示に使用）。

---

## メモリシステム

### メモリページ

**GET** `/memory`

メモリ管理インターフェースのページを返します。

### メモリリストを取得

**GET** `/api/memory/list`

シリコンビーイングのメモリエントリリストを返します。

### メモリ詳細を取得

**GET** `/api/memory/detail/{id}`

パスパラメータ：`id` — メモリエントリ ID

指定されたメモリエントリの完全な内容を返します。

### メモリ統計を取得

**GET** `/api/memory/stats`

メモリシステムの統計情報を返します。

### メモリを検索

**GET** `/api/memory/search`

クエリパラメータ：`keyword` — 検索キーワード

一致するメモリエントリを検索します。

### ビーイング別メモリグループを取得

**GET** `/api/memory/beings`

シリコンビーイング別にグループ化されたメモリ統計を返します。

### メモリトレースを取得

**GET** `/api/memory/trace/{id}`

パスパラメータ：`id` — メモリエントリ ID

指定されたメモリエントリのソーストレースチェーンを返します。

### メモリタイムライン HTML を取得

**GET** `/api/memory/timeline-html`

メモリタイムラインの HTML ビューを返します。

---

## ワークノート

### ワークノートページ

**GET** `/work-notes`

ワークノートインターフェースのページを返します。

### ワークノートリストを取得

**GET** `/api/work-notes/list`

ワークノートのリストを返します。

### ワークノートを読み取り

**GET** `/api/work-notes/read`

クエリパラメータ：`noteId` — ノート ID

指定されたノートの内容を返します。

### ノートディレクトリを取得

**GET** `/api/work-notes/directory`

ノートのディレクトリ構造を返します。

### ワークノートを検索

**GET** `/api/work-notes/search`

クエリパラメータ：`keyword` — 検索キーワード

一致するワークノートを検索します。

### ワークノートを作成

**POST** `/api/work-notes/create`

**リクエストボディ**：
```json
{
  "title": "ノートタイトル",
  "content": "ノート内容",
  "keywords": ["キーワード1", "キーワード2"]
}
```

### ワークノートを更新

**POST** `/api/work-notes/update`

**リクエストボディ**：
```json
{
  "noteId": "note-uuid",
  "title": "更新後のタイトル",
  "content": "更新後の内容"
}
```

### ワークノートを削除

**POST** `/api/work-notes/delete`

**リクエストボディ**：
```json
{
  "noteId": "note-uuid"
}
```

---

## ナレッジネットワーク

### ナレッジネットワークページ

**GET** `/knowledge`

ナレッジネットワーク管理インターフェースのページを返します。

### ナレッジグラフを取得

**GET** `/api/knowledge/graph`

ナレッジトリプルグラフデータ（主語-関係-目的語）を返します。

---

## プロジェクト管理

### プロジェクトページ

**GET** `/project`

プロジェクト管理インターフェースのページを返します。

### プロジェクトワークノートページ

**GET** `/project/{id}/work-notes`

パスパラメータ：`id` — プロジェクト ID

指定されたプロジェクトのワークノートページを返します。

### プロジェクトタスクページ

**GET** `/project/{id}/tasks`

パスパラメータ：`id` — プロジェクト ID

指定されたプロジェクトのタスク管理ページを返します。

### プロジェクトツールパーミッションページ

**GET** `/project/{id}/tool-permissions`

パスパラメータ：`id` — プロジェクト ID

指定されたプロジェクトのツールパーミッション管理ページを返します。

### プロジェクトワークフローページ

**GET** `/project/{id}/workflow`

パスパラメータ：`id` — プロジェクト ID

指定されたプロジェクトのワークフロー管理ページを返します。

### プロジェクトワークフロー詳細を取得

**GET** `/api/projects/workflow-detail`

クエリパラメータ：`projectId` — プロジェクト ID

プロジェクトに関連するワークフロー詳細を返します。

### プロジェクトロールを割り当て

**POST** `/api/projects/assign-role`

**リクエストボディ**：
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### プロジェクトロールを削除

**POST** `/api/projects/remove-role`

**リクエストボディ**：
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### プロジェクトリストを取得

**GET** `/api/projects/list`

すべてのプロジェクトのリストを返します。

### プロジェクトワークフローテンプレートリストを取得

**GET** `/api/projects/list-workflow-templates`

利用可能なワークフローテンプレートのリストを返します。

### プロジェクトを作成

**POST** `/api/projects/create`

**リクエストボディ**：
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### プロジェクトをアーカイブ

**POST** `/api/projects/{id}/archive`

パスパラメータ：`id` — プロジェクト ID

指定されたプロジェクトをアーカイブします。

### プロジェクトを復元

**POST** `/api/projects/{id}/restore`

パスパラメータ：`id` — プロジェクト ID

アーカイブ済みのプロジェクトを復元します。

### プロジェクトを破棄

**POST** `/api/projects/{id}/destroy`

パスパラメータ：`id` — プロジェクト ID

指定されたプロジェクトを永久に削除します（復元不可）。

### プロジェクト詳細を取得

**GET** `/api/projects/detail`

クエリパラメータ：`projectId` — プロジェクト ID

プロジェクトの詳細情報を返します。

### プロジェクトを更新

**POST** `/api/projects/update`

**リクエストボディ**：
```json
{
  "projectId": "project-uuid",
  "name": "Updated Name",
  "description": "Updated description"
}
```

### メンバーをプロジェクトに割り当て

**POST** `/api/projects/assign`

**リクエストボディ**：
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### メンバーをプロジェクトから削除

**POST** `/api/projects/remove`

**リクエストボディ**：
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### プロジェクトワークノートリストを取得

**GET** `/api/projects/{id}/work-notes/list`

パスパラメータ：`id` — プロジェクト ID

指定されたプロジェクトのワークノートリストを返します。

### プロジェクトワークノートを読み取り

**GET** `/api/projects/{id}/work-notes/read`

パスパラメータ：`id` — プロジェクト ID

指定されたプロジェクトのワークノート内容を返します。

### プロジェクトワークノートを作成

**POST** `/api/projects/{id}/work-notes/create`

パスパラメータ：`id` — プロジェクト ID

指定されたプロジェクトに新しいワークノートを作成します。

### プロジェクトワークノートを更新

**POST** `/api/projects/{id}/work-notes/update`

パスパラメータ：`id` — プロジェクト ID

指定されたプロジェクトのワークノートを更新します。

### プロジェクトワークノートを削除

**POST** `/api/projects/{id}/work-notes/delete`

パスパラメータ：`id` — プロジェクト ID

指定されたプロジェクトのワークノートを削除します。

### プロジェクトタスクリストを取得

**GET** `/api/projects/{id}/tasks/list`

パスパラメータ：`id` — プロジェクト ID

指定されたプロジェクトのタスクリストを返します。

### プロジェクトタスクを作成

**POST** `/api/projects/{id}/tasks/create`

パスパラメータ：`id` — プロジェクト ID

指定されたプロジェクトに新しいタスクを作成します。

### プロジェクトタスクを更新

**POST** `/api/projects/{id}/tasks/update`

パスパラメータ：`id` — プロジェクト ID

指定されたプロジェクトのタスクを更新します。

### プロジェクトタスクを削除

**POST** `/api/projects/{id}/tasks/delete`

パスパラメータ：`id` — プロジェクト ID

指定されたプロジェクトのタスクを削除します。

### タスク担当者を割り当て

**POST** `/api/projects/{id}/tasks/assign`

パスパラメータ：`id` — プロジェクト ID

プロジェクトタスクに担当者を割り当てます。

### タスク担当者を削除

**POST** `/api/projects/{id}/tasks/remove-assignee`

パスパラメータ：`id` — プロジェクト ID

プロジェクトタスクの担当者を削除します。

### タスクを完了としてマーク

**POST** `/api/projects/{id}/tasks/complete`

パスパラメータ：`id` — プロジェクト ID

プロジェクトタスクを完了としてマークします。

### タスクを失敗としてマーク

**POST** `/api/projects/{id}/tasks/fail`

パスパラメータ：`id` — プロジェクト ID

プロジェクトタスクを失敗としてマークします。

### タスクをキャンセル

**POST** `/api/projects/{id}/tasks/cancel`

パスパラメータ：`id` — プロジェクト ID

プロジェクトタスクをキャンセルします。

---

## ツールパーミッション管理

### シリコンビーイングツールパーミッションを取得

**GET** `/api/beings/tool-permissions`

クエリパラメータ：`beingId` — シリコンビーイング ID

指定されたシリコンビーイングのツールパーミッション設定を返します。

### シリコンビーイングツールパーミッションを更新

**PUT** `/api/beings/tool-permissions`

**リクエストボディ**：
```json
{
  "beingId": "being-uuid",
  "permissions": {
    "network": "allowed",
    "disk_read": "allowed",
    "disk_write": "denied"
  }
}
```

### ツールパーミッションテンプレートを取得

**GET** `/api/beings/tool-permissions/templates`

利用可能なツールパーミッションテンプレートのリストを返します。

### ツールパーミッションテンプレートを適用

**POST** `/api/beings/tool-permissions/apply-template`

**リクエストボディ**：
```json
{
  "beingId": "being-uuid",
  "templateName": "readonly"
}
```

### プロジェクトツールパーミッションを取得

**GET** `/api/projects/{id}/tool-permissions`

パスパラメータ：`id` — プロジェクト ID

指定されたプロジェクトのツールパーミッション設定を返します。

### プロジェクトツールパーミッションを更新

**PUT** `/api/projects/{id}/tool-permissions`

パスパラメータ：`id` — プロジェクト ID

**リクエストボディ**：
```json
{
  "permissions": {
    "network": "allowed",
    "disk_read": "allowed",
    "disk_write": "denied"
  }
}
```

---

## エグゼキューター管理

### エグゼキューターページ

**GET** `/executor`

エグゼキューター管理インターフェースのページを返します。

### エグゼキューターステータスを取得

**GET** `/api/executors/status`

各エグゼキューター（ディスク、ネットワーク、コマンドライン）の稼働状態を返します。

---

## コードブラウザ

### コードブラウザページ

**GET** `/code`

コードブラウザインターフェースのページを返します。

### コードタイプリストを取得

**GET** `/api/code/types`

サポートされているコードタイプ/言語のリストを返します。

### コード詳細を取得

**GET** `/api/code/detail`

クエリパラメータ：`filePath`, `lineNumber`

指定されたファイルのコード詳細を返します。

---

## コードホバー

### ホバー情報を取得

**GET** `/api/code/hover`
**POST** `/api/code/hover`

コード位置のホバー情報を取得します（IDE のインテリセンスのような機能）。

### コード位置を登録

**POST** `/api/code/register`

監視が必要なコード位置を登録します。

### コード位置を更新

**POST** `/api/code/update`

登録済みのコード位置情報を更新します。

### コード位置を登録解除

**POST** `/api/code/unregister`

不要になったコード位置監視を登録解除します。

---

## スキル管理

### スキル管理ページ

**GET** `/skill` または **GET** `/skill/index`

クエリパラメータ：`beingId` — ビーイング ID（必須）

指定されたシリコンビーイングのスキル管理ページ（スキルリスト + Markdown エディタ）を返します。

### スキルリストを取得

**GET** `/api/skills/list`

クエリパラメータ：`beingId` — ビーイング ID（必須）

ビーイングのすべてのスキル（id、description、version、tags、source、triggerMode、toolWhitelist、maxToolRound、timeoutSeconds、parameterCount）、および統計情報（スキル総数 / カスタムスキル数 / クォータ上限）を返します。

### スキル Markdown を取得

**GET** `/api/skills/get-md`

クエリパラメータ：`beingId`、`skillId`

指定されたスキルの Markdown テキスト（YAML フロントメタデータ + プロンプト本文）を返します。

### スキル Markdown を保存

**POST** `/api/skills/update-md?beingId={beingId}`

リクエストボディ（`application/json`）：

```json
{
  "markdown": "---\nid: my_skill\n...\n---\n\n提示词正文",
  "skillId": "my_skill"
}
```

Markdown でスキルを更新または新規作成します（upsert セマンティクス）。欠落しているメタデータは AI によって自動補完されます。Web UI 経由で保存されたスキルの `Source` は `User` とマークされます。クォータ `MaxCustomSkillsPerBeing` の制限を受けます。

### スキルをインポート（JSON）

**POST** `/api/skills/import?beingId={beingId}`

リクエストボディ：`{ "json": "<スキル定義 JSON>" }`

JSON からスキルをインポートします。同じくクォータ制限を受けます。

### スキルをインポート（Markdown）

**POST** `/api/skills/import-md?beingId={beingId}`

リクエストボディ：`{ "markdown": "<Markdown テキスト>" }`

Markdown から新規スキルをインポートします。欠落しているメタデータは AI によって自動補完されます。

### スキルを削除

**POST** `/api/skills/delete?beingId={beingId}`

リクエストボディ：`{ "skillId": "my_skill" }`

スキルを削除します（対応する `.md` および `.json` 永続化ファイルも同時に削除されます）。

### スキルをエクスポート（JSON）

**GET** `/api/skills/export?beingId={beingId}&skillId={skillId}`

スキル定義を JSON 添付ファイルとしてダウンロードします（`{id}.json`）。

### スキルをエクスポート（Markdown）

**GET** `/api/skills/export-md?beingId={beingId}&skillId={skillId}`

スキルを Markdown 添付ファイルとしてダウンロードします（`{id}.md`）。

### スキルのテスト実行

**POST** `/api/skills/test?beingId={beingId}`

リクエストボディ：

```json
{
  "skillId": "my_skill",
  "parametersJson": "{ \"topic\": \"AI 新闻\" }"
}
```

指定されたパラメータでスキルを一度実行し、`ToolResult`（AI 実行ラウンド数と最終出力を含む）を返します。

---

## MCP 管理

### MCP 管理ページ

**GET** `/mcp`

クエリパラメータ：`beingId` — ビーイング ID（省略可能、該当ビーイングが参照可能な MCP ツールを表示するために使用）

MCP サーバー管理ページを返します。

### サーバーリストを取得

**GET** `/api/mcp/list-servers`

設定済みのすべての MCP サーバーの状態を返します：

```json
{
  "success": true,
  "data": [
    {
      "id": "filesystem",
      "name": "Filesystem",
      "transport": "stdio",
      "state": "connected",
      "enabled": true,
      "toolCount": 8,
      "endpoint": null,
      "lastError": null
    }
  ],
  "mcpEnabled": true,
  "connected": 1,
  "toolTotal": 8
}
```

`state` の値：`connected` / `disconnected` / `connecting` / `error`。

### サーバーツールリストを取得

**GET** `/api/mcp/list-tools?serverId={serverId}`

指定されたサーバーが提供するツール（`name` はプレフィックス付きの完全名 `mcp_{serverId}_{toolName}`、`description`、`schema`）を返します。サーバーが未接続の場合はエラーを返します。

### サーバーを追加

**POST** `/api/mcp/add-server`

リクエストボディ（`McpServerConfig`）：

```json
{
  "id": "filesystem",
  "name": "Filesystem",
  "transport": "stdio",
  "command": "npx",
  "arguments": ["-y", "@modelcontextprotocol/server-filesystem", "/data"],
  "env": {},
  "endpoint": null,
  "enabled": true
}
```

`transport` は `stdio`（ローカルプロセス：`command` + `arguments`）と `http`（リモートエンドポイント：`endpoint`）をサポートします。サーバー ID には小文字、数字、アンダースコアのみ使用できます。追加後、すぐに接続し、すべてのシリコンビーイングに同期されます。

### サーバーを有効化/無効化

**POST** `/api/mcp/toggle`

リクエストボディ：`{ "serverId": "filesystem", "enabled": true }`

### サーバーを削除

**POST** `/api/mcp/remove-server`

リクエストボディ：`{ "serverId": "filesystem" }`

サーバー設定を削除し、すべてのビーイングからそのツールを登録解除します。

### サーバーを再接続

**POST** `/api/mcp/reconnect`

リクエストボディ：`{ "serverId": "filesystem" }`

強制的に切断して再接続し、ツールリストを更新します。

### ツール呼び出しをテスト

**POST** `/api/mcp/test-tool`

リクエストボディ：

```json
{
  "serverId": "filesystem",
  "toolName": "read_file",
  "argumentsJson": "{ \"path\": \"/data/hello.txt\" }"
}
```

MCP サーバーのツールを直接呼び出します（AI を介さず）、接続性を検証するために使用します。

---

## IM プラットフォーム OAuth 認証

### 認証を開始

**GET** `/im/{platform}/authorize`

パスパラメータ：`platform` — IM プラットフォーム識別子（例：`feishu`）

CSRF 対策のランダム `state` を生成し、5 分間有効な認証セッションを登録し、認証 URL を返してシステムのデフォルトブラウザを自動的に開きます。同じプラットフォームで重複して開始すると、古いセッションが上書きされます。

### 認証コールバック

**GET** `/im/{platform}/callback?code={code}&state={state}`

IM プラットフォームからのリダイレクトによって呼び出されます。`state` を検証した後、認可コードをアクセストークンと交換し、`accessToken`、`refreshToken`、`tokenExpiresAt`、`authMode=oauth` を該当プラットフォームの設定に書き戻して永続化し、最後に認証結果のランディングページ（成功/失敗）をレンダリングします。

### 認証状態を照会

**GET** `/im/{platform}/status`

`{ platform, status, tokenExpiresAt }` を返します。`status` の値：`pending` / `success` / `failed` / `timeout` / `none`。フロントエンドは主に SSE イベント `im_auth_status` でステータス通知を受信し、このインターフェースはポーリングのフォールバックとして使用されます。

---

## ヘルプドキュメントシステム

### ヘルプページ

**GET** `/help` または **GET** `/help/index`

ヘルプドキュメントのメインページを返します。

### ヘルプトピックページ

**GET** `/help/{topic}`

パスパラメータ：`topic` — トピック識別子

指定されたトピックのヘルプドキュメントページを返します。

### ヘルプドキュメントを検索

**GET** `/api/help/search`

クエリパラメータ：`keyword` — 検索キーワード

一致するヘルプドキュメントトピックを検索します。

---

## 初期化

### 初期化ウィザードページ

**GET** `/init`

初回起動時の初期化ウィザードページを返します。

### 初期化を送信

**POST** `/init`

初回起動時の初期化設定を送信します。

### データディレクトリの参照選択

**GET** `/init/browse`

データ保存場所を選択するためのディレクトリブラウザを開きます。

### AI 設定メタデータを取得

**GET** `/init/ai-config-metadata`

利用可能な AI クライアントタイプとその設定フィールドメタデータを返します。

---

## システム制御

### グレースフルシャットダウン

**POST** `/api/system/shutdown`

> **注意**：localhost からのリクエストのみ許可されています

アプリケーションのグレースフルシャットダウンをトリガーします：

1. メインループ（MainLoop）を停止
2. 現在の設定を保存
3. HTTP リスナーを閉じる

**レスポンス**：
```json
{
  "status": "shutting_down",
  "message": "Application is shutting down gracefully"
}
```

---

## バージョン情報

### バージョン情報ページ

**GET** `/about`

システム情報と読み込み済みプラグインリストを含むバージョン情報ページを返します。

**プラグインリストデータ**：
```json
{
  "plugins": {
    "plugin-id": {
      "name": "My Plugin",
      "version": "1.0.0",
      "description": "Plugin description",
      "author": "Author Name"
    }
  }
}
```

---

## エラーレスポンス

すべてのエンドポイントは標準化されたエラーレスポンスを返します：

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: FileAccess, Denied by GlobalACL"
  }
}
```

### 一般的なエラーコード

| コード | HTTP ステータス | 説明 |
|------|-------------|-------------|
| `PERMISSION_DENIED` | 403 | パーミッション不足 |
| `NOT_FOUND` | 404 | リソースが見つからない |
| `VALIDATION_ERROR` | 400 | リクエストパラメータが無効 |
| `INTERNAL_ERROR` | 500 | 内部サーバーエラー |
| `SERVICE_UNAVAILABLE` | 503 | AI サービスが利用不可 |

---

## SSE イベント

Server-Sent Events はリアルタイム更新に使用されます：

### チャットイベント

```javascript
const eventSource = new EventSource('/api/chat/stream');

eventSource.onmessage = (event) => {
  const data = JSON.parse(event.data);

  switch(data.type) {
    case 'chunk':
      console.log('Streaming:', data.content);
      break;
    case 'tool_call':
      console.log('Tool executing:', data.tool);
      break;
    case 'complete':
      console.log('Chat complete, session:', data.sessionId);
      break;
    case 'error':
      console.error('Error:', data.message);
      break;
  }
};
```

### IM 認証状態イベント

IM プラットフォーム OAuth 認証ウィザードは、共有 SSE 接続を通じてステータスをプッシュします（イベント名 `im_auth_status`）：

```javascript
eventSource.addEventListener('im_auth_status', (event) => {
  const data = JSON.parse(event.data);
  // data.platform — プラットフォーム識別子（feishu / wecom / dingtalk）
  // data.status  — pending / success / failed / timeout
  // data.message — 追加説明
  updateAuthStatus(data.platform, data.status);
});
```

---

## AI クライアントインターフェース

### IAIClient インターフェース

```csharp
public interface IAIClient
{
    string Name { get; }

    Task<AIResponse> ChatAsync(AIRequest request);

    IAsyncEnumerable<string> StreamChatAsync(AIRequest request);
}
```

### AIRequest 構造体

```csharp
public class AIRequest
{
    public List<Message> Messages { get; set; }
    public List<ToolDefinition> Tools { get; set; }
    public double Temperature { get; set; } = 0.7;
    public int MaxTokens { get; set; } = 2000;
    public string Model { get; set; }
}
```

### AIResponse 構造体

```csharp
public class AIResponse
{
    public string Content { get; set; }
    public List<ToolCall> ToolCalls { get; set; }
    public TokenUsage Usage { get; set; }
    public string Model { get; set; }
}
```

---

## ツールシステムインターフェース

### ITool インターフェース

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    ToolDefinition Definition { get; }

    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### ToolCall 構造体

```csharp
public class ToolCall
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}
```

### ToolResult 構造体

```csharp
public class ToolResult
{
    public bool Success { get; set; }
    public string Output { get; set; }
    public string Error { get; set; }
}
```

---

## 次のステップ

- 🚀 [クイックスタートガイド](getting-started.md)を確認
- 🛠️ [開発ガイド](development-guide.md)を読む
- 📚 [アーキテクチャドキュメント](architecture.md)を確認
- 🔒 [セキュリティモデル](security.md)を理解