# Referência da API

> **Versão: v0.2.0-alpha**

[English](../en/api-reference.md) | [Deutsch](../de-DE/api-reference.md) | [Français](../fr-FR/api-reference.md) | [中文](../zh-CN/api-reference.md) | [繁體中文](../zh-HK/api-reference.md) | [Español](../es-ES/api-reference.md) | [日本語](../ja-JP/api-reference.md) | [한국어](../ko-KR/api-reference.md) | [Čeština](../cs-CZ/api-reference.md) | [Italiano](../it-IT/api-reference.md) | [Polski](../pl-PL/api-reference.md) | **Português**

## Endpoints da API Web

URL base: `http://localhost:8080`

### Autenticação

A maioria dos endpoints requer autenticação através de cookie de sessão gerido pela interface Web.

---

## Gestão dos Silicon Beings

### Obter todos os Beings

**GET** `/api/beings`

**Resposta**:
```json
{
  "beings": [
    {
      "id": "being-uuid",
      "name": "Assistente",
      "status": "running",
      "soul": "path/to/soul.md"
    }
  ]
}
```

**Valores de estado**: `idle` | `running` | `waiting_permission` | `stopped`

### Criar um Being

**POST** `/api/beings`

**Pedido**:
```json
{
  "name": "Novo Being",
  "soul": "# Personalidade\nTu és útil..."
}
```

**Resposta**: `201 Created`

### Iniciar um Being

**POST** `/api/beings/{id}/start`

### Parar um Being

**POST** `/api/beings/{id}/stop`

### Obter os detalhes de um Being

**GET** `/api/beings/{id}`

---

## Sistema de chat

### Enviar uma mensagem

**POST** `/api/chat`

**Pedido**:
```json
{
  "beingId": "being-uuid",
  "message": "Olá, como estás?",
  "sessionId": "optional-session-id"
}
```

**Resposta** (não streaming):
```json
{
  "reply": "Estou bem, obrigado!",
  "sessionId": "session-uuid",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### Chat em streaming (SSE)

**GET** `/api/chat/stream?beingId={id}&message={msg}`

**Resposta**: Fluxo Server-Sent Events

```
data: {"type": "chunk", "content": "Estou"}
data: {"type": "chunk", "content": " a refletir"}
data: {"type": "chunk", "content": " bem..."}
data: {"type": "complete", "sessionId": "uuid"}
```

### Obter o histórico do chat

**GET** `/api/chat/{sessionId}/history`

**Resposta**:
```json
{
  "messages": [
    {
      "role": "user",
      "content": "Olá",
      "timestamp": "2026-04-20T10:30:00Z"
    },
    {
      "role": "assistant",
      "content": "Olá!",
      "timestamp": "2026-04-20T10:30:05Z"
    }
  ]
}
```

---

## Configuração

### Obter a configuração

**GET** `/api/config`

**Resposta**:
```json
{
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:7b"
    }
  },
  "storage": {
    "basePath": "./data"
  }
}
```

### Atualizar a configuração

**POST** `/api/config`

**Pedido**:
```json
{
  "aiClients": {
    "Ollama": {
      "baseUrl": "http://localhost:11434",
      "model": "qwen2.5:14b"
    }
  }
}
```

---

## Sistema de permissões

### Obter as permissões

**GET** `/api/permissions`

**Resposta**:
```json
{
  "rules": [
    {
      "userId": "user-uuid",
      "resource": "disk:read",
      "allowed": true,
      "expiresAt": "2026-04-21T00:00:00Z"
    }
  ]
}
```

### Conceder uma permissão

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

### Revogar uma permissão

**DELETE** `/api/permissions/{id}`

### Verificar uma permissão

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
  "reason": "Concedido pelo curator"
}
```

---

## Sistema de tarefas e temporizadores

### Criar uma tarefa

**POST** `/api/tasks`

**Pedido**:
```json
{
  "beingId": "being-uuid",
  "description": "Rever o código",
  "priority": 5,
  "dueDate": "2026-04-21T12:00:00Z"
}
```

### Obter as tarefas

**GET** `/api/tasks?beingId={id}&status=pending`

### Atualizar o estado de uma tarefa

**PATCH** `/api/tasks/{id}`

**Pedido**:
```json
{
  "status": "completed"
}
```

### Criar um temporizador

**POST** `/api/timers`

**Pedido**:
```json
{
  "beingId": "being-uuid",
  "interval": 3600,
  "action": "think",
  "repeat": true
}
```

### Eliminar um temporizador

**DELETE** `/api/timers/{id}`

---

## Auditoria e registo

### Obter a utilização de tokens

**GET** `/api/audit/tokens?startDate={date}&endDate={date}`

**Resposta**:
```json
{
  "summary": {
    "totalTokens": 150000,
    "promptTokens": 100000,
    "completionTokens": 50000,
    "totalCost": 0.15
  },
  "byModel": {
    "qwen2.5:7b": {
      "tokens": 100000,
      "cost": 0.10
    }
  }
}
```

### Obter os registos

**GET** `/api/logs?level=error&limit=100`

**Resposta**:
```json
{
  "logs": [
    {
      "timestamp": "2026-04-20T10:30:00Z",
      "level": "error",
      "message": "Falha na ligação ao serviço IA",
      "source": "OllamaClient"
    }
  ]
}
```

---

## API de armazenamento

### Ler um valor

**GET** `/api/storage?key={key}`

**Resposta**:
```json
{
  "key": "being:uuid:memory",
  "value": "{...}",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

### Escrever um valor

**POST** `/api/storage`

**Pedido**:
```json
{
  "key": "being:uuid:memory",
  "value": "{...}"
}
```

### Consulta por intervalo temporal

**GET** `/api/storage/time?start={start}&end={end}&prefix={prefix}`

**Resposta**:
```json
{
  "entries": [
    {
      "key": "being:uuid:chat:2026-04-20",
      "value": "{...}",
      "timestamp": "2026-04-20T10:30:00Z"
    }
  ]
}
```

---

## Informações do sistema

### Obter a página Informações

**GET** `/about`

Retorna a página Informações com as informações do sistema e a lista dos plugins carregados.

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

### Pedido de permissão

**GET** `/permission/request?userId={id}&type={type}&resource={resource}`

Mostra a página de pedido de permissão que permite aos utilizadores aprovar ou rejeitar os pedidos de permissão dos Silicon Beings.

**Parâmetros de consulta**:

| Parâmetro | Tipo | Descrição |
|-----------|------|-------------|
| `userId` | `Guid` | ID do Silicon Being que solicita a permissão |
| `type` | `string` | Tipo de permissão |
| `resource` | `string` | Caminho do recurso solicitado |
| `allowCode` | `string` | ID do código para a operação de autorização |
| `denyCode` | `string` | ID do código para a operação de rejeição |

**Verificar pedidos de permissão pendentes**:

**GET** `/permission/check?userId={id}`

**Resposta**:
```json
{
  "pending": true
}
```

**Responder a um pedido de permissão**:

**GET** `/permission/respond?userId={id}&allowed={bool}&addToCache={bool}&cacheDuration={hours}`

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

### Obter os dados do painel

**GET** `/api/dashboard`

**Resposta**:
```json
{
  "beings": {
    "total": 5,
    "running": 3,
    "stopped": 2
  },
  "performance": {
    "cpu": 45.2,
    "memory": 1024,
    "uptime": 86400
  },
  "aiUsage": {
    "todayTokens": 50000,
    "todayCost": 0.05
  }
}
```

### Obter o estado do sistema

**GET** `/api/status`

**Resposta**:
```json
{
  "version": "1.0.0",
  "runtime": ".NET 9.0",
  "uptime": 86400,
  "health": "healthy"
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
const eventSource = new EventSource('/api/chat/stream?beingId=xxx&message=xxx');

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

### Eventos de estado dos Beings

```javascript
const beingEvents = new EventSource('/api/beings/events');

beingEvents.onmessage = (event) => {
  const data = JSON.parse(event.data);
  console.log(`Being ${data.beingId} estado: ${data.status}`);
};
```

---

## API do cliente IA

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

## API das notas de trabalho

### Obter a lista das notas de trabalho

**GET** `/api/beings/{id}/work-notes`

**Resposta**:
```json
{
  "notes": [
    {
      "id": "note-uuid",
      "pageNumber": 1,
      "summary": "Módulo de autenticação de utilizador concluído",
      "keywords": ["autenticação", "JWT", "OAuth2"],
      "createdAt": "2026-04-25T10:00:00Z",
      "updatedAt": "2026-04-25T10:00:00Z"
    }
  ],
  "totalCount": 15
}
```

### Obter os detalhes de uma nota

**GET** `/api/beings/{id}/work-notes/{pageNumber}`

**Resposta**:
```json
{
  "id": "note-uuid",
  "pageNumber": 1,
  "summary": "Módulo de autenticação de utilizador concluído",
  "content": "## Detalhes da implementação\n\n- Utilização de JWT token\n- Suporte OAuth2",
  "keywords": ["autenticação", "JWT", "OAuth2"],
  "createdAt": "2026-04-25T10:00:00Z",
  "updatedAt": "2026-04-25T10:00:00Z"
}
```

### Criar uma nova nota

**POST** `/api/beings/{id}/work-notes`

**Pedido**:
```json
{
  "summary": "Módulo de autenticação de utilizador concluído",
  "content": "## Detalhes da implementação\n\n- Utilização de JWT token",
  "keywords": "autenticação,JWT,OAuth2"
}
```

**Resposta**: `201 Created`

### Atualizar uma nota

**PUT** `/api/beings/{id}/work-notes/{pageNumber}`

**Pedido**:
```json
{
  "summary": "Módulo de autenticação e testes concluídos",
  "content": "## Conteúdo atualizado\n\nAdicionados testes unitários",
  "keywords": "autenticação,JWT,OAuth2,testes"
}
```

### Eliminar uma nota

**DELETE** `/api/beings/{id}/work-notes/{pageNumber}`

### Pesquisar notas

**GET** `/api/beings/{id}/work-notes/search?keyword=autenticação&maxResults=10`

### Obter o diretório das notas

**GET** `/api/beings/{id}/work-notes/directory`

---

## API da rede de conhecimentos

### Obter as estatísticas dos conhecimentos

**GET** `/api/knowledge/stats`

**Resposta**:
```json
{
  "totalTriples": 1523,
  "totalSubjects": 450,
  "totalPredicates": 85,
  "totalObjects": 892,
  "averageConfidence": 0.87
}
```

### Adicionar uma tripla de conhecimento

**POST** `/api/knowledge/triples`

**Pedido**:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language",
  "confidence": 0.95,
  "tags": ["programming", "language"]
}
```

**Resposta**: `201 Created`

### Consultar conhecimentos

**GET** `/api/knowledge/query?subject=Python&predicate=is_a`

**Resposta**:
```json
{
  "triples": [
    {
      "subject": "Python",
      "predicate": "is_a",
      "object": "programming_language",
      "confidence": 0.95,
      "tags": ["programming", "language"]
    }
  ]
}
```

### Pesquisar conhecimentos

**GET** `/api/knowledge/search?query=programming+language&limit=10`

### Obter um caminho de conhecimento

**GET** `/api/knowledge/path?from=Python&to=computer_science`

**Resposta**:
```json
{
  "path": [
    {"subject": "Python", "predicate": "is_a", "object": "programming_language"},
    {"subject": "programming_language", "predicate": "belongs_to", "object": "computer_science"}
  ],
  "length": 2
}
```

### Validar um conhecimento

**POST** `/api/knowledge/validate`

**Pedido**:
```json
{
  "subject": "Python",
  "predicate": "is_a",
  "object": "programming_language"
}
```

### Eliminar um conhecimento

**DELETE** `/api/knowledge/triples/{id}`

---

## API de documentação de ajuda

### Obter a lista da documentação de ajuda

**GET** `/api/help`

**Resposta**:
```json
{
  "topics": [
    {
      "id": "getting-started",
      "title": "Guia rápido",
      "category": "Introdução"
    }
  ]
}
```

### Obter os detalhes de um tópico de ajuda

**GET** `/api/help/{topicId}`

**Resposta**:
```json
{
  "id": "getting-started",
  "title": "Guia rápido",
  "content": "# Guia rápido\n\n...",
  "category": "Introdução"
}
```

---

## API do browser WebView

### Obter o estado do browser

**GET** `/api/beings/{id}/browser/status`

**Resposta**:
```json
{
  "is_open": true,
  "current_url": "https://example.com",
  "page_title": "Example Page",
  "is_loading": false,
  "last_operation_time": "2026-04-26T10:00:00Z"
}
```

### Abrir o browser

**POST** `/api/beings/{id}/browser/open`

### Fechar o browser

**POST** `/api/beings/{id}/browser/close`

### Navegar para um URL

**POST** `/api/beings/{id}/browser/navigate`

**Pedido**:
```json
{
  "url": "https://example.com"
}
```

### Executar JavaScript

**POST** `/api/beings/{id}/browser/execute-script`

**Pedido**:
```json
{
  "script": "return document.title;"
}
```

### Obter uma captura de ecrã da página

**GET** `/api/beings/{id}/browser/screenshot`

---

## API do espaço de projeto

### Obter a lista dos projetos

**GET** `/api/projects`

**Resposta**:
```json
{
  "projects": [
    {
      "id": "project-uuid",
      "name": "O Meu Projeto",
      "description": "Descrição do projeto",
      "createdAt": "2026-04-25T10:00:00Z"
    }
  ]
}
```

### Criar um projeto

**POST** `/api/projects`

**Pedido**:
```json
{
  "name": "O Meu Projeto",
  "description": "Descrição do projeto"
}
```

### Obter os detalhes de um projeto

**GET** `/api/projects/{id}`

### Atualizar um projeto

**PUT** `/api/projects/{id}`

### Eliminar um projeto

**DELETE** `/api/projects/{id}`

---

## API do sistema de ferramentas

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
