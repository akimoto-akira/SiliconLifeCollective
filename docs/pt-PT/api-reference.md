# Referência da API

> **Versão: v0.2.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [Français](../fr-FR/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md) | [Italiano](../it-IT/api-reference.md) | [Polski](../pl-PL/api-reference.md) | **Português**

## Endpoints da API Web

URL base: `http://localhost:8080`

### Autenticação

A maioria dos endpoints requer autenticação através de cookie de sessão gerido pela interface Web.

---

## Página principal e chat

### Página principal

**GET** `/`

Retorna a página principal da aplicação (interface de chat).

### Página de chat

**GET** `/chat`

Retorna a interface de chat.

### Obter a lista de conversas

**GET** `/api/chat/conversations`

Retorna a lista de todas as conversas ativas.

**Resposta de exemplo**:
```json
[
  {
    "sessionId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
    "beingId": "being-uuid",
    "type": "single",
    "displayName": "Conversa com o Assistente",
    "lastMessage": "Olá!",
    "lastTime": "2026-04-20T10:30:00Z"
  }
]
```

### Obter o histórico de mensagens

**GET** `/api/chat/messages`

Parâmetro de consulta: `channelId` — ID da conversa

Retorna o histórico de mensagens da conversa especificada.

### Enviar uma mensagem

**POST** `/api/chat/send`

**Pedido**:
```json
{
  "channelId": "85ccff8e-7497-1991-7a38-ffa1b7d9c50d",
  "content": "Olá, como estás?"
}
```

**Resposta**:
```json
{
  "success": true,
  "messageId": "50156b26-f3b9-4735-be3d-51e547bd3a4a"
}
```

### Parar o pensamento da IA

**POST** `/api/chat/stop`

Interrompe o processo de pensamento da IA em curso.

### Fluxo de chat em tempo real (SSE)

**GET** `/api/chat/stream`

Server-Sent Events para atualizações de chat em tempo real.

---

## Gestão dos Silicon Beings

### Página dos Beings

**GET** `/beings`

Retorna a página de gestão dos Silicon Beings.

### Obter a lista de Beings

**GET** `/api/beings/list`

Retorna a lista de todos os Silicon Beings.

**Resposta de exemplo**:
```json
[
  {
    "id": "being-uuid",
    "name": "Assistente",
    "status": "running",
    "soul": "path/to/soul.md"
  }
]
```

**Valores de atividade**: `Idle` | `SingleChat` | `GroupChat` | `Task` | `Timer` | `Broadcast` | `Project` | `MemoryCompression` | `Stopped`

### Iniciar um Being

**POST** `/api/beings/start`

**Pedido**:
```json
{
  "beingId": "being-uuid"
}
```

### Parar um Being

**POST** `/api/beings/stop`

**Pedido**:
```json
{
  "beingId": "being-uuid"
}
```

### Guardar configuração da IA

**POST** `/api/beings/ai-config/save`

**Pedido**:
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

### Obter a lista de modelos de IA disponíveis

**GET** `/api/beings/ai-config/models`

Parâmetros de consulta: `clientType`, `apiKey`, `region`

Retorna a lista de modelos disponíveis para o cliente de IA especificado.

---

## Histórico do chat

### Página do histórico do chat

**GET** `/chat-history`

Retorna a página principal do histórico do chat.

### Página de detalhes do histórico do chat

**GET** `/chat-history-detail`

Retorna a página de detalhes do histórico de uma conversa específica.

### Página de detalhes do histórico de chat em grupo

**GET** `/group-chat-history-detail`

Retorna a página de detalhes do histórico de chat em grupo.

### Página de detalhes do histórico de difusão

**GET** `/broadcast-history-detail`

Retorna a página de detalhes do histórico do canal de difusão.

### Obter a lista de conversas do histórico

**GET** `/api/chat-history/conversations`

Retorna a lista de todas as conversas do histórico.

### Obter as mensagens do histórico

**GET** `/api/chat-history/messages`

Parâmetro de consulta: `sessionId` — ID da conversa

Retorna o registo de mensagens da conversa do histórico especificada.

---

## Gestão de temporizadores

### Página dos temporizadores

**GET** `/timers`

Retorna a página da interface de gestão dos temporizadores.

### Obter a lista de temporizadores

**GET** `/api/timers/list`

Retorna a lista de todos os temporizadores.

### Página de detalhes dos ciclos do temporizador

**GET** `/timer-cycles/{timerId}`

Parâmetro de caminho: `timerId` — ID do temporizador

Retorna a página de detalhes dos ciclos de execução do temporizador especificado.

### Obter a lista de ciclos do temporizador

**GET** `/api/timer-cycles/list`

Parâmetro de consulta: `timerId` — ID do temporizador

Retorna a lista de todos os ciclos de execução do temporizador especificado.

### Página de detalhes de um ciclo de execução

**GET** `/timer-cycle/{cycleIndex}`

Retorna a página de detalhes de uma execução individual.

### Obter as mensagens do ciclo

**GET** `/api/timer-cycle/messages`

Parâmetro de consulta: `cycleIndex` — Índice do ciclo

Retorna as mensagens relacionadas com o ciclo de execução especificado.

---

## Gestão de tarefas

### Página das tarefas

**GET** `/tasks`

Retorna a página da interface de gestão das tarefas.

### Obter a lista de tarefas

**GET** `/api/tasks/list`

Retorna a lista de todas as tarefas.

### Página de detalhes dos ciclos da tarefa

**GET** `/task-cycles/{taskId}`

Parâmetro de caminho: `taskId` — ID da tarefa

Retorna a página de detalhes dos ciclos de execução da tarefa especificada.

### Obter a lista de ciclos da tarefa

**GET** `/api/task-cycles/list`

Parâmetro de consulta: `taskId` — ID da tarefa

Retorna a lista de todos os ciclos de execução da tarefa especificada.

### Página de detalhes de um ciclo de execução da tarefa

**GET** `/task-cycle/{cycleIndex}`

Retorna a página de detalhes de uma execução individual da tarefa.

### Obter as mensagens do ciclo da tarefa

**GET** `/api/task-cycle/messages`

Parâmetro de consulta: `cycleIndex` — Índice do ciclo

Retorna as mensagens relacionadas com o ciclo de execução da tarefa especificado.

---

## Sistema de permissões

### Página de gestão de permissões

**GET** `/permissions`

Retorna a página da interface de gestão de permissões.

### Obter a lista de regras de permissão

**GET** `/api/permissions`

Retorna todas as regras de permissão configuradas atualmente.

**Resposta de exemplo**:
```json
{
  "rules": [
    {
      "prefix": "network:api.github.com",
      "result": "Allowed"
    },
    {
      "prefix": "file:C:\\Windows",
      "result": "Denied"
    }
  ]
}
```

### Guardar regra de permissão

**POST** `/api/permissions`

**Pedido**:
```json
{
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "duration": 3600
}
```

### Revogar regra de permissão

**DELETE** `/api/permissions/{id}`

### Verificar permissão

**POST** `/api/permissions/check`

**Pedido**:
```json
{
  "userId": "user-uuid",
  "resource": "network:http"
}
```

**Resposta**:
```json
{
  "allowed": true,
  "reason": "Granted by curator"
}
```

### Página de pedido de permissão

**GET** `/permission/request`

Mostra a página de pedido de permissão que permite aos utilizadores aprovar ou rejeitar os pedidos de permissão dos Silicon Beings.

**Parâmetros de consulta**:

| Parâmetro | Tipo | Descrição |
|-----------|------|-------------|
| `userId` | `Guid` | ID do Silicon Being que solicita a permissão |
| `type` | `string` | Tipo de permissão |
| `resource` | `string` | Caminho do recurso solicitado |
| `allowCode` | `string` | Identificador do código para a operação de autorização |
| `denyCode` | `string` | Identificador do código para a operação de rejeição |

### Verificar pedidos de permissão pendentes

**GET** `/permission/check`

Parâmetro de consulta: `userId` — ID do Silicon Being

**Resposta**:
```json
{
  "pending": true
}
```

### Responder a um pedido de permissão

**GET** `/permission/respond`

**Parâmetros de consulta**:

| Parâmetro | Tipo | Descrição |
|-----------|------|-------------|
| `userId` | `Guid` | ID do Silicon Being |
| `allowed` | `bool` | Se autorizado |
| `addToCache` | `bool` | Se a decisão deve ser armazenada em cache |
| `cacheDuration` | `double` | Duração da cache (horas) |

**Resposta**:
```json
{
  "success": true
}
```

---

## Sistema de registos

### Página dos registos

**GET** `/logs`

Retorna a página da interface de visualização dos registos.

### Obter a lista de registos

**GET** `/api/logs/list`

Os parâmetros de consulta suportam filtragem por nível e intervalo de tempo.

**Resposta de exemplo**:
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

### Obter registos agrupados por Being

**GET** `/api/logs/beings`

Estatísticas de registos agrupadas por Silicon Being.

### Obter os níveis de registo disponíveis

**GET** `/api/logs/levels`

Retorna a lista de níveis de registo disponíveis no sistema.

---

## Estatísticas de utilização

### Página das estatísticas de utilização

**GET** `/usage`

Retorna a página da interface das estatísticas de utilização.

### Obter o resumo de utilização

**GET** `/api/usage/summary`

Retorna o resumo da utilização de Tokens e custos.

### Obter dados de tendência

**GET** `/api/usage/trend`

Parâmetros de consulta: `startDate`, `endDate`

Retorna os dados de tendência de utilização no período especificado.

### Exportar dados de utilização

**GET** `/api/usage/export`

Exporta os dados de utilização num formato transferível.

---

## Registo de auditoria

### Página de auditoria

**GET** `/audit`

Retorna a página da interface do registo de auditoria.

### Obter a lista de auditoria

**GET** `/api/audit/list`

Retorna a lista de entradas do registo de auditoria.

### Obter o resumo de auditoria

**GET** `/api/audit/summary`

Retorna as estatísticas resumidas dos dados de auditoria.

### Obter auditoria agrupada por Being

**GET** `/api/audit/beings`

Estatísticas de auditoria agrupadas por Silicon Being.

---

## Gestão da configuração

### Página da configuração

**GET** `/config`

Retorna a página da interface da configuração do sistema.

### Guardar configuração

**POST** `/config/save`

**Pedido**:
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
    }
  }
}
```

### Obter as opções de configuração da IA

**GET** `/config/aioptions`

Retorna os tipos de clientes de IA disponíveis e as suas opções dinâmicas (modelos disponíveis, regiões, etc.).

---

## Sistema de memória

### Página da memória

**GET** `/memory`

Retorna a página da interface de gestão da memória.

### Obter a lista de memórias

**GET** `/api/memory/list`

Retorna a lista de entradas de memória dos Silicon Beings.

### Obter os detalhes de uma memória

**GET** `/api/memory/detail/{id}`

Parâmetro de caminho: `id` — ID da entrada de memória

Retorna o conteúdo completo da entrada de memória especificada.

### Obter as estatísticas da memória

**GET** `/api/memory/stats`

Retorna as estatísticas do sistema de memória.

### Pesquisar memórias

**GET** `/api/memory/search`

Parâmetro de consulta: `keyword` — Palavra-chave de pesquisa

Pesquisa entradas de memória correspondentes.

### Obter memórias agrupadas por Being

**GET** `/api/memory/beings`

Estatísticas de memória agrupadas por Silicon Being.

### Obter o rastreio de uma memória

**GET** `/api/memory/trace/{id}`

Parâmetro de caminho: `id` — ID da entrada de memória

Retorna a cadeia de rastreio da origem da entrada de memória especificada.

### Obter o HTML da linha do tempo da memória

**GET** `/api/memory/timeline-html`

Retorna a vista HTML da linha do tempo da memória.

---

## Notas de trabalho

### Página das notas de trabalho

**GET** `/work-notes`

Retorna a página da interface das notas de trabalho.

### Obter a lista de notas de trabalho

**GET** `/api/work-notes/list`

Retorna a lista de notas de trabalho.

### Ler uma nota de trabalho

**GET** `/api/work-notes/read`

Parâmetro de consulta: `noteId` — ID da nota

Retorna o conteúdo da nota especificada.

### Obter o diretório das notas

**GET** `/api/work-notes/directory`

Retorna a estrutura do diretório das notas.

### Pesquisar notas de trabalho

**GET** `/api/work-notes/search`

Parâmetro de consulta: `keyword` — Palavra-chave de pesquisa

Pesquisa notas de trabalho correspondentes.

### Criar uma nota de trabalho

**POST** `/api/work-notes/create`

**Pedido**:
```json
{
  "title": "Título da nota",
  "content": "Conteúdo da nota",
  "keywords": ["palavra-chave1", "palavra-chave2"]
}
```

### Atualizar uma nota de trabalho

**POST** `/api/work-notes/update`

**Pedido**:
```json
{
  "noteId": "note-uuid",
  "title": "Título atualizado",
  "content": "Conteúdo atualizado"
}
```

### Eliminar uma nota de trabalho

**POST** `/api/work-notes/delete`

**Pedido**:
```json
{
  "noteId": "note-uuid"
}
```

---

## Rede de conhecimentos

### Página da rede de conhecimentos

**GET** `/knowledge`

Retorna a página da interface de gestão da rede de conhecimentos.

### Obter o grafo de conhecimentos

**GET** `/api/knowledge/graph`

Retorna os dados do grafo de triplas de conhecimento (sujeito-relação-objeto).

---

## Gestão de projetos

### Página dos projetos

**GET** `/project`

Retorna a página da interface de gestão dos projetos.

### Página das notas de trabalho do projeto

**GET** `/project/{id}/work-notes`

Parâmetro de caminho: `id` — ID do projeto

Retorna a página das notas de trabalho do projeto especificado.

### Página das tarefas do projeto

**GET** `/project/{id}/tasks`

Parâmetro de caminho: `id` — ID do projeto

Retorna a página de gestão das tarefas do projeto especificado.

### Obter a lista de projetos

**GET** `/api/projects/list`

Retorna a lista de todos os projetos.

### Obter a lista de modelos de fluxo de trabalho

**GET** `/api/projects/list-workflow-templates`

Retorna a lista de modelos de fluxo de trabalho disponíveis.

### Criar um projeto

**POST** `/api/projects/create`

**Pedido**:
```json
{
  "name": "O Meu Projeto",
  "description": "Descrição do projeto"
}
```

### Arquivar um projeto

**POST** `/api/projects/{id}/archive`

Parâmetro de caminho: `id` — ID do projeto

Arquiva o projeto especificado.

### Restaurar um projeto

**POST** `/api/projects/{id}/restore`

Parâmetro de caminho: `id` — ID do projeto

Restaura um projeto previamente arquivado.

### Destruir um projeto

**POST** `/api/projects/{id}/destroy`

Parâmetro de caminho: `id` — ID do projeto

Elimina permanentemente o projeto especificado (irrecuperável).

### Obter os detalhes de um projeto

**GET** `/api/projects/detail`

Parâmetro de consulta: `projectId` — ID do projeto

Retorna as informações detalhadas do projeto.

### Atualizar um projeto

**POST** `/api/projects/update`

**Pedido**:
```json
{
  "projectId": "project-uuid",
  "name": "Nome Atualizado",
  "description": "Descrição atualizada"
}
```

### Atribuir um membro ao projeto

**POST** `/api/projects/assign`

**Pedido**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Remover um membro do projeto

**POST** `/api/projects/remove`

**Pedido**:
```json
{
  "projectId": "project-uuid",
  "beingId": "being-uuid"
}
```

### Obter a lista de notas de trabalho do projeto

**GET** `/api/projects/{id}/work-notes/list`

Parâmetro de caminho: `id` — ID do projeto

Retorna a lista de notas de trabalho do projeto especificado.

### Ler as notas de trabalho do projeto

**GET** `/api/projects/{id}/work-notes/read`

Parâmetro de caminho: `id` — ID do projeto

Retorna o conteúdo das notas de trabalho do projeto.

### Criar uma nota de trabalho no projeto

**POST** `/api/projects/{id}/work-notes/create`

Parâmetro de caminho: `id` — ID do projeto

Cria uma nova nota de trabalho no projeto especificado.

### Atualizar uma nota de trabalho do projeto

**POST** `/api/projects/{id}/work-notes/update`

Parâmetro de caminho: `id` — ID do projeto

Atualiza uma nota de trabalho no projeto especificado.

### Eliminar uma nota de trabalho do projeto

**POST** `/api/projects/{id}/work-notes/delete`

Parâmetro de caminho: `id` — ID do projeto

Elimina uma nota de trabalho no projeto especificado.

### Obter a lista de tarefas do projeto

**GET** `/api/projects/{id}/tasks/list`

Parâmetro de caminho: `id` — ID do projeto

Retorna a lista de tarefas do projeto especificado.

### Criar uma tarefa no projeto

**POST** `/api/projects/{id}/tasks/create`

Parâmetro de caminho: `id` — ID do projeto

Cria uma nova tarefa no projeto especificado.

### Atualizar uma tarefa do projeto

**POST** `/api/projects/{id}/tasks/update`

Parâmetro de caminho: `id` — ID do projeto

Atualiza uma tarefa no projeto especificado.

### Eliminar uma tarefa do projeto

**POST** `/api/projects/{id}/tasks/delete`

Parâmetro de caminho: `id` — ID do projeto

Elimina uma tarefa no projeto especificado.

### Atribuir um responsável à tarefa

**POST** `/api/projects/{id}/tasks/assign`

Parâmetro de caminho: `id` — ID do projeto

Atribui um responsável à tarefa do projeto.

### Remover o responsável da tarefa

**POST** `/api/projects/{id}/tasks/remove-assignee`

Parâmetro de caminho: `id` — ID do projeto

Remove o responsável da tarefa do projeto.

### Marcar a tarefa como concluída

**POST** `/api/projects/{id}/tasks/complete`

Parâmetro de caminho: `id` — ID do projeto

Marca a tarefa do projeto como concluída.

### Marcar a tarefa como falhada

**POST** `/api/projects/{id}/tasks/fail`

Parâmetro de caminho: `id` — ID do projeto

Marca a tarefa do projeto como falhada.

### Cancelar uma tarefa

**POST** `/api/projects/{id}/tasks/cancel`

Parâmetro de caminho: `id` — ID do projeto

Cancela a tarefa do projeto.

---

## Gestão de executores

### Página dos executores

**GET** `/executor`

Retorna a página da interface de gestão dos executores.

### Obter o estado dos executores

**GET** `/api/executors/status`

Retorna o estado de execução de cada executor (disco, rede, linha de comandos).

---

## Navegador de código

### Página do navegador de código

**GET** `/code`

Retorna a página da interface do navegador de código.

### Obter a lista de tipos de código

**GET** `/api/code/types`

Retorna a lista de tipos/linguagens de código suportados.

### Obter os detalhes do código

**GET** `/api/code/detail`

Parâmetros de consulta: `filePath`, `lineNumber`

Retorna os detalhes do código do ficheiro especificado.

---

## Dicas flutuantes de código

### Obter dica flutuante

**GET** `/api/code/hover`
**POST** `/api/code/hover`

Obtém informações de dica flutuante para uma posição no código (semelhante às sugestões inteligentes de um IDE).

### Registar posição no código

**POST** `/api/code/register`

Regista uma posição no código para monitorização.

### Atualizar posição no código

**POST** `/api/code/update`

Atualiza as informações de uma posição no código previamente registada.

### Cancelar registo de posição no código

**POST** `/api/code/unregister`

Cancela o registo de monitorização de uma posição no código que já não é necessária.

---

## Sistema de documentação de ajuda

### Página de ajuda

**GET** `/help` ou **GET** `/help/index`

Retorna a página principal da documentação de ajuda.

### Página de tópico de ajuda

**GET** `/help/{topic}`

Parâmetro de caminho: `topic` — Identificador do tópico

Retorna a página de documentação de ajuda do tópico especificado.

### Pesquisar documentação de ajuda

**GET** `/api/help/search`

Parâmetro de consulta: `keyword` — Palavra-chave de pesquisa

Pesquisa tópicos de documentação de ajuda correspondentes.

---

## Inicialização

### Página do assistente de inicialização

**GET** `/init`

Retorna a página do assistente de inicialização para a primeira execução.

### Submeter inicialização

**POST** `/init`

Submete a configuração de inicialização para a primeira execução.

### Procurar diretório de dados

**GET** `/init/browse`

Abre o navegador de diretórios para selecionar a localização de armazenamento dos dados.

### Obter metadados da configuração da IA

**GET** `/init/ai-config-metadata`

Retorna os tipos de clientes de IA disponíveis e os metadados dos campos de configuração.

---

## Controlo do sistema

### Encerramento elegante

**POST** `/api/system/shutdown`

> **Nota**: Apenas são permitidos pedidos provenientes de localhost

Aciona o processo de encerramento elegante da aplicação:

1. Para o ciclo principal (MainLoop)
2. Guarda a configuração atual
3. Fecha o ouvinte HTTP

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

Retorna a página Sobre, com informações do sistema e a lista dos plugins carregados.

**Dados da lista de plugins**:
```json
{
  "plugins": {
    "plugin-id": {
      "name": "O Meu Plugin",
      "version": "1.0.0",
      "description": "Descrição do plugin",
      "author": "Nome do autor"
    }
  }
}
```

---

## Respostas de erro

Todos os endpoints retornam respostas de erro padronizadas:

```json
{
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "Não tem permissão para aceder a este recurso",
    "details": "Requerido: disk:write, Atual: disk:read"
  }
}
```

### Códigos de erro comuns

| Código | Estado HTTP | Descrição |
|--------|------------|-------------|
| `PERMISSION_DENIED` | 403 | Permissões insuficientes |
| `NOT_FOUND` | 404 | Recurso não encontrado |
| `VALIDATION_ERROR` | 400 | Parâmetros de pedido inválidos |
| `INTERNAL_ERROR` | 500 | Erro interno do servidor |
| `SERVICE_UNAVAILABLE` | 503 | Serviço IA indisponível |

---

## Eventos SSE

Server-Sent Events para atualizações em tempo real:

### Eventos de chat

```javascript
const eventSource = new EventSource('/api/chat/stream');

eventSource.onmessage = (event) => {
  const data = JSON.parse(event.data);

  switch(data.type) {
    case 'chunk':
      console.log('Streaming:', data.content);
      break;
    case 'tool_call':
      console.log('Ferramenta em execução:', data.tool);
      break;
    case 'complete':
      console.log('Chat concluído, sessão:', data.sessionId);
      break;
    case 'error':
      console.error('Erro:', data.message);
      break;
  }
};
```

---

## Interface do cliente de IA

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

## Interface do sistema de ferramentas

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

## Próximos passos

- 🚀 Ver o [guia rápido](getting-started.md)
- 🛠️ Ler o [guia de desenvolvimento](development-guide.md)
- 📚 Consultar a [documentação de arquitetura](architecture.md)
- 🔒 Compreender o [modelo de segurança](security.md)
