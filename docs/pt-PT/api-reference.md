# Referência API

> **Versão: v0.2.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md) | [Русский](../ru-RU/api-reference.md)

## Endpoints da Web API

URL base: `http://localhost:8080`

### Autenticação

A maioria dos endpoints requer autenticação via cookie de sessão gerido pela Web UI. Antes da inicialização do sistema, todos os pedidos, excepto a página de ajuda, serão redireccionados para a página de inicialização.

---

## Painel

### Obter Estatísticas do Painel

**GET** `/api/dashboard/stats`

Retorna dados de visão geral do sistema (número de beings, estado de execução, etc.).

### Obter Métricas de Desempenho

**GET** `/api/dashboard/metrics`

Retorna dados de métricas de desempenho em tempo real.

---

## Sistema de Chat

### Página de Chat

**GET** `/chat`

Retorna a página da interface de chat.

### Chat em Streaming (SSE)

**GET** `/api/chat/stream`

Chat em streaming via Server-Sent Events (SSE).

**Resposta**: Fluxo de eventos enviados pelo servidor

```
data: {"type": "chunk", "content": "I"}
data: {"type": "chunk", "content": "'m"}
data: {"type": "chunk", "content": " thinking..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### Obter Lista de Sessões

**GET** `/api/chat/conversations`

Retorna a lista de todas as sessões de chat activas.

**Exemplo de resposta**:
```json
{
  "conversations": [
    {
      "sessionId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
      "beingId": "being-uuid",
      "type": "single",
      "displayName": "Chat com Xiaoyou",
      "lastMessage": "Conteúdo da última mensagem",
      "lastTime": "2026-05-20T10:30:00Z"
    }
  ]
}
```

### Obter Histórico de Mensagens

**GET** `/api/chat/messages`

Parâmetro de consulta: `channelId` — ID do canal/sessão

Retorna o histórico de mensagens da sessão especificada.

### Obter Histórico de Chat

**GET** `/api/chat/history`

Retorna o histórico global de chat.

### Enviar Mensagem

**POST** `/api/chat/send`

**Corpo do pedido**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
  "content": "Conteúdo da mensagem de teste"
}
```

**Resposta**:
```json
{
  "success": true,
  "messageId": "50156b26-f3b9-4735-be3d-51e547bd3a4a"
}
```

### Parar Pensamento da IA

**POST** `/api/chat/stop`

Para a geração de resposta da IA actualmente em curso.

**Corpo do pedido**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d"
}
```

### Carregar Ficheiro

**POST** `/api/chat/upload`

Carrega um ficheiro para a sessão de chat (suporta multipart/form-data).

---

## Gestão de Silicon Beings

### Página de Gestão de Beings

**GET** `/beings`

Retorna a página da interface de gestão de Silicon Beings.

### Obter Lista de Beings

**GET** `/api/beings` ou **GET** `/api/beings/list`

Retorna a lista de todos os Silicon Beings registados.

**Exemplo de resposta**:
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

**Valores de estado**: `idle` | `running` | `waiting_permission` | `stopped`

### Obter Detalhes do Being

**GET** `/api/beings/detail`

Parâmetro de consulta: `beingId` — ID do Silicon Being

Retorna informações detalhadas do Silicon Being especificado.

### Obter Estado de Actividade dos Beings

**GET** `/api/beings/activity`

Retorna informações sobre o estado de actividade de cada Silicon Being.

### Página do Editor do Ficheiro da Alma

**GET** `/beings/soul`

Retorna a interface do editor do Ficheiro da Alma.

### Guardar Ficheiro da Alma

**POST** `/api/beings/soul/save`

**Corpo do pedido**:
```json
{
  "beingId": "being-uuid",
  "soulContent": "# Personality\nYou are helpful..."
}
```

### Página do Editor de Configuração de IA

**GET** `/beings/ai-config`

Retorna a interface do editor de configuração de IA.

### Guardar Configuração de IA

**POST** `/api/beings/ai-config/save`

**Corpo do pedido**:
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

### Obter Lista de Modelos de IA Disponíveis

**GET** `/api/beings/ai-config/models`

Parâmetros de consulta: `clientType`, `apiKey`, `region`

Retorna a lista de modelos disponíveis para o cliente de IA especificado.

---

## Visualização do Histórico de Chat

### Página do Histórico de Chat

**GET** `/chat-history`

Retorna a página principal do histórico de chat.

### Página de Detalhes do Histórico de Chat

**GET** `/chat-history-detail`

Retorna a página de detalhes do histórico de chat para a sessão especificada.

### Página de Detalhes do Histórico de Chat de Grupo

**GET** `/group-chat-history-detail`

Retorna a página de detalhes do histórico de chat de grupo.

### Página de Detalhes do Histórico de Difusão

**GET** `/broadcast-history-detail`

Retorna a página de detalhes do histórico do canal de difusão.

### Obter Lista de Sessões Históricas

**GET** `/api/chat-history/conversations`

Retorna a lista de todas as sessões históricas.

### Obter Mensagens Históricas

**GET** `/api/chat-history/messages`

Parâmetro de consulta: `sessionId` — ID da sessão

Retorna o registo de mensagens da sessão histórica especificada.

---

## Gestão de Temporizadores

### Página de Temporizadores

**GET** `/timers`

Retorna a página da interface de gestão de temporizadores.

### Obter Lista de Temporizadores

**GET** `/api/timers/list`

Retorna a lista de todos os temporizadores.

### Página de Detalhes do Ciclo do Temporizador

**GET** `/timer-cycles/{timerId}`

Retorna a página de detalhes do ciclo de execução do temporizador especificado.

### Obter Lista de Ciclos do Temporizador

**GET** `/api/timer-cycles/list`

Parâmetro de consulta: `timerId` — ID do temporizador

Retorna a lista de todos os ciclos de execução do temporizador especificado.

### Página de Detalhes de um Ciclo de Execução

**GET** `/timer-cycle/{cycleIndex}`

Retorna a página de detalhes de uma execução individual.

### Obter Mensagens do Ciclo

**GET** `/api/timer-cycle/messages`

Parâmetro de consulta: `cycleIndex` — Índice do ciclo

Retorna as mensagens relacionadas com o ciclo de execução especificado.

---

## Gestão de Tarefas

### Página de Tarefas

**GET** `/tasks`

Retorna a página da interface de gestão de tarefas.

### Obter Lista de Tarefas

**GET** `/api/tasks/list`

Retorna a lista de todas as tarefas.

### Página de Detalhes do Ciclo da Tarefa

**GET** `/task-cycles/{taskId}`

Retorna a página de detalhes do ciclo de execução da tarefa especificada.

### Obter Lista de Ciclos da Tarefa

**GET** `/api/task-cycles/list`

Parâmetro de consulta: `taskId` — ID da tarefa

Retorna a lista de todos os ciclos de execução da tarefa especificada.

### Página de Detalhes de um Ciclo de Execução

**GET** `/task-cycle/{cycleIndex}`

Retorna a página de detalhes de uma execução de tarefa individual.

### Obter Mensagens do Ciclo

**GET** `/api/task-cycle/messages`

Parâmetro de consulta: `cycleIndex` — Índice do ciclo

Retorna as mensagens relacionadas com o ciclo de execução da tarefa especificada.

---

## Sistema de Permissões

### Página de Gestão de Permissões

**GET** `/permissions`

Retorna a página da interface de gestão de permissões.

### Obter Lista de Regras de Permissões

**GET** `/api/permissions/list`

Retorna todas as regras de permissões actualmente configuradas.

**Exemplo de resposta**:
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

### Guardar Regra de Permissão

**POST** `/api/permissions/save`

**Corpo do pedido**:
```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects",
  "result": "Allowed",
  "description": "Allow project directory access"
}
```

### Página de Pedido de Permissão

**GET** `/permission/request`

Exibe a página de pedido de permissão, permitindo ao utilizador aprovar ou negar pedidos de permissão dos Silicon Beings.

**Parâmetros de consulta**:

| Parâmetro | Tipo | Descrição |
|------|------|------|
| `userId` | `Guid` | ID do Silicon Being que solicita a permissão |
| `type` | `string` | Tipo de permissão |
| `resource` | `string` | Caminho do recurso solicitado |
| `allowCode` | `string` | Código de identificação para permitir a operação |
| `denyCode` | `string` | Código de identificação para negar a operação |

### Verificar Pedidos de Permissão Pendentes

**GET** `/permission/check`

Parâmetro de consulta: `userId` — ID do Silicon Being

**Resposta**:
```json
{
  "pending": true
}
```

### Responder a Pedido de Permissão

**GET** `/permission/respond`

**Parâmetros de consulta**:

| Parâmetro | Tipo | Descrição |
|------|------|------|
| `userId` | `Guid` | ID do Silicon Being |
| `allowed` | `bool` | Se a permissão é concedida |
| `addToCache` | `bool` | Se a decisão deve ser cacheada |
| `cacheDuration` | `double` | Duração do cache (horas) |

**Resposta**:
```json
{
  "success": true
}
```

---

## Sistema de Registos

### Página de Registos

**GET** `/logs`

Retorna a página da interface de visualização de registos.

### Obter Lista de Registos

**GET** `/api/logs/list`

Os parâmetros de consulta suportam filtragem por nível e intervalo de tempo.

**Exemplo de resposta**:
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

### Obter Registos Agrupados por Being

**GET** `/api/logs/beings`

Estatísticas de registos agrupadas por Silicon Being.

### Obter Níveis de Registo Disponíveis

**GET** `/api/logs/levels`

Retorna a lista de níveis de registo disponíveis no sistema.

---

## Estatísticas de Utilização

### Página de Estatísticas de Utilização

**GET** `/usage`

Retorna a página da interface de estatísticas de utilização.

### Obter Resumo de Utilização

**GET** `/api/usage/summary`

Retorna o resumo de utilização de tokens e custos.

### Obter Dados de Tendência

**GET** `/api/usage/trend`

Parâmetros de consulta: `startDate`, `endDate`

Retorna dados de tendência de utilização no período especificado.

### Exportar Dados de Utilização

**GET** `/api/usage/export`

Exporta dados de utilização num formato descarregável.

---

## Registo de Auditoria

### Página de Auditoria

**GET** `/audit`

Retorna a página da interface de registo de auditoria.

### Obter Lista de Auditoria

**GET** `/api/audit/list`

Retorna a lista de entradas do registo de auditoria.

### Obter Resumo de Auditoria

**GET** `/api/audit/summary`

Retorna estatísticas sumárias dos dados de auditoria.

### Obter Auditoria Agrupada por Being

**GET** `/api/audit/beings`

Estatísticas de auditoria agrupadas por Silicon Being.

---

## Gestão de Configuração

### Página de Configuração

**GET** `/config`

Retorna a página da interface de configuração do sistema.

### Guardar Configuração

**POST** `/config/save`

**Corpo do pedido**:
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
      "model": "..."
    },
    "Zhipu": {
      "apiKey": "...",
      "model": "..."
    },
    "Ernie": {
      "apiKey": "...",
      "model": "..."
    },
    "Hunyuan": {
      "apiKey": "...",
      "model": "..."
    },
    "MiniMax": {
      "apiKey": "...",
      "model": "..."
    },
    "Moonshot": {
      "apiKey": "...",
      "model": "..."
    },
    "SiliconFlow": {
      "apiKey": "...",
      "model": "..."
    }
  }
}
```

### Obter Opções de Configuração de IA

**GET** `/config/aioptions`

Retorna os tipos de clientes de IA disponíveis e as suas opções dinâmicas (modelos disponíveis, regiões, etc.).

### Obter Opções de Plataforma IM

**GET** `/config/imoptions`

Retorna metadados das plataformas IM (para o assistente de configuração renderizar formulários dinamicamente):

```json
{
  "success": true,
  "platforms": [
    {
      "value": "feishu",
      "display": "飞书 (Feishu)",
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

### Navegar Configuração

**GET** `/config/browse`

Retorna os dados de navegação dos itens de configuração (para apresentação agrupada na interface de configuração).

---

## Sistema de Memória

### Página de Memória

**GET** `/memory`

Retorna a página da interface de gestão de memória.

### Obter Lista de Memórias

**GET** `/api/memory/list`

Retorna a lista de entradas de memória dos Silicon Beings.

### Obter Detalhes da Memória

**GET** `/api/memory/detail/{id}`

Parâmetro de caminho: `id` — ID da entrada de memória

Retorna o conteúdo completo da entrada de memória especificada.

### Obter Estatísticas de Memória

**GET** `/api/memory/stats`

Retorna informações estatísticas do sistema de memória.

### Pesquisar Memória

**GET** `/api/memory/search`

Parâmetro de consulta: `keyword` — Palavra-chave de pesquisa

Pesquisa entradas de memória correspondentes.

### Obter Memória Agrupada por Being

**GET** `/api/memory/beings`

Estatísticas de memória agrupadas por Silicon Being.

### Obter Rastreamento de Memória

**GET** `/api/memory/trace/{id}`

Parâmetro de caminho: `id` — ID da entrada de memória

Retorna a cadeia de rastreamento da origem da entrada de memória especificada.

### Obter HTML da Linha Temporal de Memória

**GET** `/api/memory/timeline-html`

Retorna a vista HTML da linha temporal de memória.

---

## Notas de Trabalho

### Página de Notas de Trabalho

**GET** `/work-notes`

Retorna a página da interface de notas de trabalho.

### Obter Lista de Notas de Trabalho

**GET** `/api/work-notes/list`

Retorna a lista de notas de trabalho.

### Ler Nota de Trabalho

**GET** `/api/work-notes/read`

Parâmetro de consulta: `noteId` — ID da nota

Retorna o conteúdo da nota especificada.

### Obter Directório de Notas

**GET** `/api/work-notes/directory`

Retorna a estrutura do directório de notas.

### Pesquisar Notas de Trabalho

**GET** `/api/work-notes/search`

Parâmetro de consulta: `keyword` — Palavra-chave de pesquisa

Pesquisa notas de trabalho correspondentes.

### Criar Nota de Trabalho

**POST** `/api/work-notes/create`

**Corpo do pedido**:
```json
{
  "title": "Título da nota",
  "content": "Conteúdo da nota",
  "keywords": ["palavra-chave1", "palavra-chave2"]
}
```

### Actualizar Nota de Trabalho

**POST** `/api/work-notes/update`

**Corpo do pedido**:
```json
{
  "noteId": "note-uuid",
  "title": "Título actualizado",
  "content": "Conteúdo actualizado"
}
```

### Eliminar Nota de Trabalho

**POST** `/api/work-notes/delete`

**Corpo do pedido**:
```json
{
  "noteId": "note-uuid"
}
```

---

## Rede de Conhecimento

### Página da Rede de Conhecimento

**GET** `/knowledge`

Retorna a página da interface de gestão da rede de conhecimento.

### Obter Grafo de Conhecimento

**GET** `/api/knowledge/graph`

Retorna dados do grafo de triplas de conhecimento (sujeito-relação-objecto).

---

## Gestão de Projectos

### Página de Projectos

**GET** `/project`

Retorna a página da interface de gestão de projectos.

### Página de Notas de Trabalho do Projecto

**GET** `/project/{id}/work-notes`

Parâmetro de caminho: `id` — ID do projecto

Retorna a página de notas de trabalho do projecto especificado.

### Página de Tarefas do Projecto

**GET** `/project/{id}/tasks`

Parâmetro de caminho: `id` — ID do projecto

Retorna a página de gestão de tarefas do projecto especificado.

### Página de Permissões de Ferramentas do Projecto

**GET** `/project/{id}/tool-permissions`

Parâmetro de caminho: `id` — ID do projecto

Retorna a página de gestão de permissões de ferramentas do projecto especificado.

### Página de Fluxo de Trabalho do Projecto

**GET** `/project/{id}/workflow`

Parâmetro de caminho: `id` — ID do projecto

Retorna a página de gestão do fluxo de trabalho do projecto especificado.

### Obter Detalhes do Fluxo de Trabalho do Projecto

**GET** `/api/projects/workflow-detail`

Parâmetro de consulta: `projectId` — ID do projecto

Retorna os detalhes do fluxo de trabalho associado ao projecto.

### Atribuir Função no Projecto

**POST** `/api/projects/assign-role`

**Corpo do pedido**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### Remover Função no Projecto

**POST** `/api/projects/remove-role`

**Corpo do pedido**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid",
  "roleName": "developer"
}
```

### Obter Lista de Projectos

**GET** `/api/projects/list`

Retorna a lista de todos os projectos.

### Obter Lista de Modelos de Fluxo de Trabalho do Projecto

**GET** `/api/projects/list-workflow-templates`

Retorna a lista de modelos de fluxo de trabalho disponíveis.

### Criar Projecto

**POST** `/api/projects/create`

**Corpo do pedido**:
```json
{
  "name": "My Project",
  "description": "Project description"
}
```

### Arquivar Projecto

**POST** `/api/projects/{id}/archive`

Parâmetro de caminho: `id` — ID do projecto

Arquiva o projecto especificado.

### Restaurar Projecto

**POST** `/api/projects/{id}/restore`

Parâmetro de caminho: `id` — ID do projecto

Restaura um projecto arquivado.

### Destruir Projecto

**POST** `/api/projects/{id}/destroy`

Parâmetro de caminho: `id` — ID do projecto

Elimina permanentemente o projecto especificado (irreversível).

### Obter Detalhes do Projecto

**GET** `/api/projects/detail`

Parâmetro de consulta: `projectId` — ID do projecto

Retorna informações detalhadas do projecto.

### Actualizar Projecto

**POST** `/api/projects/update`

**Corpo do pedido**:
```json
{
  "projectId": "project-uuid",
  "name": "Updated Name",
  "description": "Updated description"
}
```

### Atribuir Membro ao Projecto

**POST** `/api/projects/assign`

**Corpo do pedido**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Remover Membro do Projecto

**POST** `/api/projects/remove`

**Corpo do pedido**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Obter Lista de Notas de Trabalho do Projecto

**GET** `/api/projects/{id}/work-notes/list`

Parâmetro de caminho: `id` — ID do projecto

Retorna a lista de notas de trabalho do projecto especificado.

### Ler Notas de Trabalho do Projecto

**GET** `/api/projects/{id}/work-notes/read`

Parâmetro de caminho: `id` — ID do projecto

Retorna o conteúdo das notas de trabalho do projecto especificado.

### Criar Nota de Trabalho do Projecto

**POST** `/api/projects/{id}/work-notes/create`

Parâmetro de caminho: `id` — ID do projecto

Cria uma nova nota de trabalho no projecto especificado.

### Actualizar Nota de Trabalho do Projecto

**POST** `/api/projects/{id}/work-notes/update`

Parâmetro de caminho: `id` — ID do projecto

Actualiza uma nota de trabalho no projecto especificado.

### Eliminar Nota de Trabalho do Projecto

**POST** `/api/projects/{id}/work-notes/delete`

Parâmetro de caminho: `id` — ID do projecto

Elimina uma nota de trabalho no projecto especificado.

### Obter Lista de Tarefas do Projecto

**GET** `/api/projects/{id}/tasks/list`

Parâmetro de caminho: `id` — ID do projecto

Retorna a lista de tarefas do projecto especificado.

### Criar Tarefa do Projecto

**POST** `/api/projects/{id}/tasks/create`

Parâmetro de caminho: `id` — ID do projecto

Cria uma nova tarefa no projecto especificado.

### Actualizar Tarefa do Projecto

**POST** `/api/projects/{id}/tasks/update`

Parâmetro de caminho: `id` — ID do projecto

Actualiza uma tarefa no projecto especificado.

### Eliminar Tarefa do Projecto

**POST** `/api/projects/{id}/tasks/delete`

Parâmetro de caminho: `id` — ID do projecto

Elimina uma tarefa no projecto especificado.

### Atribuir Responsável à Tarefa

**POST** `/api/projects/{id}/tasks/assign`

Parâmetro de caminho: `id` — ID do projecto

Atribui um responsável à tarefa do projecto.

### Remover Responsável da Tarefa

**POST** `/api/projects/{id}/tasks/remove-assignee`

Parâmetro de caminho: `id` — ID do projecto

Remove o responsável da tarefa do projecto.

### Marcar Tarefa como Concluída

**POST** `/api/projects/{id}/tasks/complete`

Parâmetro de caminho: `id` — ID do projecto

Marca a tarefa do projecto como concluída.

### Marcar Tarefa como Falhada

**POST** `/api/projects/{id}/tasks/fail`

Parâmetro de caminho: `id` — ID do projecto

Marca a tarefa do projecto como falhada.

### Cancelar Tarefa

**POST** `/api/projects/{id}/tasks/cancel`

Parâmetro de caminho: `id` — ID do projecto

Cancela a tarefa do projecto.

---

## Gestão de Permissões de Ferramentas

### Obter Permissões de Ferramentas do Silicon Being

**GET** `/api/beings/tool-permissions`

Parâmetro de consulta: `beingId` — ID do Silicon Being

Retorna a configuração de permissões de ferramentas do Silicon Being especificado.

### Actualizar Permissões de Ferramentas do Silicon Being

**PUT** `/api/beings/tool-permissions`

**Corpo do pedido**:
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

### Obter Modelos de Permissões de Ferramentas

**GET** `/api/beings/tool-permissions/templates`

Retorna a lista de modelos de permissões de ferramentas disponíveis.

### Aplicar Modelo de Permissões de Ferramentas

**POST** `/api/beings/tool-permissions/apply-template`

**Corpo do pedido**:
```json
{
  "beingId": "being-uuid",
  "templateName": "readonly"
}
```

### Obter Permissões de Ferramentas do Projecto

**GET** `/api/projects/{id}/tool-permissions`

Parâmetro de caminho: `id` — ID do projecto

Retorna a configuração de permissões de ferramentas do projecto especificado.

### Actualizar Permissões de Ferramentas do Projecto

**PUT** `/api/projects/{id}/tool-permissions`

Parâmetro de caminho: `id` — ID do projecto

**Corpo do pedido**:
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

## Gestão de Executores

### Página de Executores

**GET** `/executor`

Retorna a página da interface de gestão de executores.

### Obter Estado dos Executores

**GET** `/api/executors/status`

Retorna o estado de execução de cada executor (disco, rede, linha de comandos).

---

## Navegador de Código

### Página do Navegador de Código

**GET** `/code`

Retorna a página da interface do navegador de código.

### Obter Lista de Tipos de Código

**GET** `/api/code/types`

Retorna a lista de tipos/linguagens de código suportados.

### Obter Detalhes do Código

**GET** `/api/code/detail`

Parâmetros de consulta: `filePath`, `lineNumber`

Retorna os detalhes do código do ficheiro especificado.

---

## Dicas Flutuantes de Código

### Obter Dica Flutuante

**GET** `/api/code/hover`
**POST** `/api/code/hover`

Obtém informações de dica flutuante para uma posição no código (semelhante ao IntelliSense do IDE).

### Registar Posição de Código

**POST** `/api/code/register`

Regista uma posição de código a monitorizar.

### Actualizar Posição de Código

**POST** `/api/code/update`

Actualiza informações de uma posição de código registada.

### Cancelar Registo de Posição de Código

**POST** `/api/code/unregister`

Cancela o registo de monitorização de uma posição de código.

---

## Gestão de Competências

### Página de Gestão de Competências

**GET** `/skill` ou **GET** `/skill/index`

Parâmetros de consulta: `beingId` — ID do Being (obrigatório)

Retorna a página de gestão de competências do Silicon Being especificado (lista de competências + editor Markdown).

### Obter Lista de Competências

**GET** `/api/skills/list`

Parâmetros de consulta: `beingId` — ID do Being (obrigatório)

Retorna todas as competências do Being (id, description, version, tags, source, triggerMode, toolWhitelist, maxToolRound, timeoutSeconds, parameterCount), bem como estatísticas (número total de competências / competências personalizadas / limite de quota).

### Obter Markdown da Competência

**GET** `/api/skills/get-md`

Parâmetros de consulta: `beingId`, `skillId`

Retorna o texto Markdown da competência especificada (metadados YAML frontmatter + corpo do prompt).

### Guardar Markdown da Competência

**POST** `/api/skills/update-md?beingId={beingId}`

Corpo do pedido (`application/json`):

```json
{
  "markdown": "---\nid: my_skill\n...\n---\n\nCorpo do prompt",
  "skillId": "my_skill"
}
```

Actualiza ou cria uma competência em Markdown (semântica upsert). Os metadados em falta são preenchidos automaticamente pela IA; as competências guardadas através da interface Web têm `Source` marcada como `User`. Sujeita à quota `MaxCustomSkillsPerBeing`.

### Importar Competência (JSON)

**POST** `/api/skills/import?beingId={beingId}`

Corpo do pedido: `{ "json": "<JSON de definição da competência>" }`

Importa uma competência a partir de JSON, igualmente sujeita à quota.

### Importar Competência (Markdown)

**POST** `/api/skills/import-md?beingId={beingId}`

Corpo do pedido: `{ "markdown": "<texto Markdown>" }`

Importa uma nova competência a partir de Markdown; os metadados em falta são preenchidos automaticamente pela IA.

### Eliminar Competência

**POST** `/api/skills/delete?beingId={beingId}`

Corpo do pedido: `{ "skillId": "my_skill" }`

Elimina a competência (bem como os ficheiros de persistência `.md` e `.json` correspondentes).

### Exportar Competência (JSON)

**GET** `/api/skills/export?beingId={beingId}&skillId={skillId}`

Descarrega a definição da competência como anexo JSON (`{id}.json`).

### Exportar Competência (Markdown)

**GET** `/api/skills/export-md?beingId={beingId}&skillId={skillId}`

Descarrega a competência como anexo Markdown (`{id}.md`).

### Testar Execução de Competência

**POST** `/api/skills/test?beingId={beingId}`

Corpo do pedido:

```json
{
  "skillId": "my_skill",
  "parametersJson": "{ \"topic\": \"AI 新闻\" }"
}
```

Executa a competência uma vez com os parâmetros fornecidos e retorna o `ToolResult` (incluindo o número de rondas de execução da IA e o resultado final).

---

## Gestão MCP

### Página de Gestão MCP

**GET** `/mcp`

Parâmetros de consulta: `beingId` — ID do Being (opcional, usado para mostrar as ferramentas MCP visíveis para esse Being)

Retorna a página de gestão de servidores MCP.

### Obter Lista de Servidores

**GET** `/api/mcp/list-servers`

Retorna o estado de todos os servidores MCP configurados:

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

Valores de `state`: `connected` / `disconnected` / `connecting` / `error`.

### Obter Lista de Ferramentas do Servidor

**GET** `/api/mcp/list-tools?serverId={serverId}`

Retorna as ferramentas fornecidas pelo servidor especificado (`name` é o nome completo com prefixo `mcp_{serverId}_{toolName}`, `description`, `schema`). Retorna um erro se o servidor não estiver ligado.

### Adicionar Servidor

**POST** `/api/mcp/add-server`

Corpo do pedido (`McpServerConfig`):

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

`transport` suporta `stdio` (processo local: `command` + `arguments`) e `http` (endpoint remoto: `endpoint`). O ID do servidor apenas permite letras minúsculas, números e underscores. Após adicionar, liga imediatamente e sincroniza com todos os Silicon Beings.

### Activar/Desactivar Servidor

**POST** `/api/mcp/toggle`

Corpo do pedido: `{ "serverId": "filesystem", "enabled": true }`

### Eliminar Servidor

**POST** `/api/mcp/remove-server`

Corpo do pedido: `{ "serverId": "filesystem" }`

Elimina a configuração do servidor e remove as suas ferramentas de todos os Beings.

### Religar Servidor

**POST** `/api/mcp/reconnect`

Corpo do pedido: `{ "serverId": "filesystem" }`

Força a desconexão e restabelece a ligação, actualizando a lista de ferramentas.

### Testar Chamada de Ferramenta

**POST** `/api/mcp/test-tool`

Corpo do pedido:

```json
{
  "serverId": "filesystem",
  "toolName": "read_file",
  "argumentsJson": "{ \"path\": \"/data/hello.txt\" }"
}
```

Chama directamente a ferramenta do servidor MCP (sem intervenção da IA), para verificar a conectividade.

---

## Autorização OAuth de Plataforma IM

### Iniciar Autorização

**GET** `/im/{platform}/authorize`

Parâmetros de caminho: `platform` — identificador da plataforma IM (ex.: `feishu`)

Gera um `state` aleatório anti-CSRF, regista uma sessão de autorização válida por 5 minutos, retorna o URL de autorização e abre automaticamente o navegador predefinido do sistema. Iniciar novamente para a mesma plataforma substitui a sessão anterior.

### Callback de Autorização

**GET** `/im/{platform}/callback?code={code}&state={state}`

Chamado pelo redireccionamento da plataforma IM. Valida o `state`, troca o código de autorização por um token de acesso, escreve `accessToken`, `refreshToken`, `tokenExpiresAt`, `authMode=oauth` na configuração da plataforma e persiste, e por fim renderiza a página de resultado da autorização (sucesso/insucesso).

### Consultar Estado de Autorização

**GET** `/im/{platform}/status`

Retorna `{ platform, status, tokenExpiresAt }`. Valores de `status`: `pending` / `success` / `failed` / `timeout` / `none`. O frontend recebe preferencialmente o estado através do evento SSE `im_auth_status`; esta interface serve como fallback de consulta periódica.

---

## Sistema de Documentação de Ajuda

### Página de Ajuda

**GET** `/help` ou **GET** `/help/index`

Retorna a página principal da documentação de ajuda.

### Página de Tópico de Ajuda

**GET** `/help/{topic}`

Parâmetro de caminho: `topic` — Identificador do tópico

Retorna a página de documentação de ajuda do tópico especificado.

### Pesquisar Documentação de Ajuda

**GET** `/api/help/search`

Parâmetro de consulta: `keyword` — Palavra-chave de pesquisa

Pesquisa tópicos de documentação de ajuda correspondentes.

---

## Inicialização

### Página do Assistente de Inicialização

**GET** `/init`

Retorna a página do assistente de inicialização para primeira execução.

### Submeter Inicialização

**POST** `/init`

Submete a configuração de inicialização para primeira execução.

### Navegar para Seleccionar Directório de Dados

**GET** `/init/browse`

Abre o navegador de directórios para seleccionar a localização de armazenamento de dados.

### Obter Metadados de Configuração de IA

**GET** `/init/ai-config-metadata`

Retorna os tipos de clientes de IA disponíveis e os metadados dos campos de configuração.

---

## Controlo do Sistema

### Encerramento Elegante

**POST** `/api/system/shutdown`

> **Nota**: Apenas são permitidos pedidos de localhost

Inicia o processo de encerramento elegante da aplicação:

1. Parar o ciclo principal (MainLoop)
2. Guardar a configuração actual
3. Fechar o listener HTTP

**Resposta**:
```json
{
  "status": "shutting_down",
  "message": "Application is shutting down gracefully"
}
```

---

## Sobre

### Página Sobre

**GET** `/about`

Retorna a página sobre, com informações do sistema e a lista de plugins carregados.

**Dados da lista de plugins**:
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

## Respostas de Erro

Todos os endpoints retornam respostas de erro padronizadas:

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "You don't have permission to access this resource",
    "details": "Required: FileAccess, Denied by GlobalACL"
  }
}
```

### Códigos de Erro Comuns

| Código | Estado HTTP | Descrição |
|------|-------------|------|
| `PERMISSION_DENIED` | 403 | Permissões insuficientes |
| `NOT_FOUND` | 404 | Recurso não encontrado |
| `VALIDATION_ERROR` | 400 | Parâmetros do pedido inválidos |
| `INTERNAL_ERROR` | 500 | Erro interno do servidor |
| `SERVICE_UNAVAILABLE` | 503 | Serviço de IA indisponível |

---

## Eventos SSE

Os Server-Sent Events são usados para actualizações em tempo real:

### Eventos de Chat

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

---

### Eventos de Estado de Autorização IM

O assistente de autorização OAuth de plataforma IM envia o estado através de uma ligação SSE partilhada (nome do evento `im_auth_status`):

```javascript
eventSource.addEventListener('im_auth_status', (event) => {
  const data = JSON.parse(event.data);
  // data.platform — identificador da plataforma (feishu / wecom / dingtalk)
  // data.status  — pending / success / failed / timeout
  // data.message — descrição adicional
  updateAuthStatus(data.platform, data.status);
});
```

---

## Interface do Cliente de IA

### Interface IAIClient

```csharp
public interface IAIClient
{
    string Name { get; }
    
    Task<AIResponse> ChatAsync(AIRequest request);
    
    IAsyncEnumerable<string> StreamChatAsync(AIRequest request);
}
```

### Estrutura AIRequest

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

### Estrutura AIResponse

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

## Interface do Sistema de Ferramentas

### Interface ITool

```csharp
public interface ITool
{
    string Name { get; }
    string Description { get; }
    ToolDefinition Definition { get; }
    
    Task<ToolResult> ExecuteAsync(ToolCall call);
}
```

### Estrutura ToolCall

```csharp
public class ToolCall
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}
```

### Estrutura ToolResult

```csharp
public class ToolResult
{
    public bool Success { get; set; }
    public string Output { get; set; }
    public string Error { get; set; }
}
```

---

## Próximos Passos

- 🚀 Consulte o [guia de início rápido](getting-started.md)
- 🛠️ Leia o [guia de desenvolvimento](development-guide.md)
- 📚 Consulte a [documentação de arquitectura](architecture.md)
- 🔒 Compreenda o [modelo de segurança](security.md)
