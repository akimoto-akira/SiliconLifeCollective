# Referência de ferramentas

> **Versão: v0.2.0-alpha**

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [Français](../fr-FR/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md) | [Italiano](../it-IT/tools-reference.md) | [Polski](../pl-PL/tools-reference.md) | **Português**

## Visão geral

O SiliconLifeCollective fornece um conjunto rico de ferramentas integradas que permitem aos Silicon Beings interagir com o sistema de ficheiros, a rede, o sistema e outros recursos. Todas as ferramentas são descobertas e registadas automaticamente através de reflexão.

---

## Sistema de ferramentas

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

### Atributo ToolScenario

As ferramentas podem ser ativadas em cenários específicos:

```csharp
[ToolScenario(Scenario.All)]           // Disponível em todos os cenários
[ToolScenario(Scenario.Chat)]          // Apenas no chat
[ToolScenario(Scenario.Scheduled)]     // Apenas em tarefas agendadas
```

---

## Ferramentas do sistema de ficheiros

### FileReadTool

Lê o conteúdo de um ficheiro.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `path` | string | Sim | Caminho do ficheiro a ler |
| `encoding` | string | Não | Codificação (por defeito: UTF-8) |

**Permissão necessária**: `disk:read`

### FileWriteTool

Escreve conteúdo num ficheiro.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `path` | string | Sim | Caminho do ficheiro a escrever |
| `content` | string | Sim | Conteúdo a escrever |
| `encoding` | string | Não | Codificação (por defeito: UTF-8) |
| `append` | boolean | Não | Acrescentar em vez de sobrescrever (por defeito: false) |

**Permissão necessária**: `disk:write`

### FileDeleteTool

Elimina um ficheiro.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `path` | string | Sim | Caminho do ficheiro a eliminar |

**Permissão necessária**: `disk:delete`

### DirectoryListTool

Lista o conteúdo de um diretório.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `path` | string | Sim | Caminho do diretório a listar |
| `pattern` | string | Não | Padrão de pesquisa (por defeito: *) |
| `recursive` | boolean | Não | Pesquisa recursiva (por defeito: false) |

**Permissão necessária**: `disk:list`

### DirectoryCreateTool

Cria um diretório.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `path` | string | Sim | Caminho do diretório a criar |

**Permissão necessária**: `disk:write`

---

## Ferramentas de rede

### HttpGetTool

Realiza um pedido HTTP GET.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `url` | string | Sim | URL do pedido |
| `headers` | object | Não | Cabeçalhos do pedido |

**Permissão necessária**: `network:http`

### HttpPostTool

Realiza um pedido HTTP POST.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `url` | string | Sim | URL do pedido |
| `body` | string | Sim | Corpo do pedido |
| `contentType` | string | Não | Tipo de conteúdo (por defeito: application/json) |
| `headers` | object | Não | Cabeçalhos do pedido |

**Permissão necessária**: `network:http`

### WebSearchTool

Pesquisa na Web.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `query` | string | Sim | Termos de pesquisa |
| `maxResults` | integer | Não | Número máximo de resultados (por defeito: 5) |

**Permissão necessária**: `network:http`

---

## Ferramentas do sistema

### ProcessExecuteTool

Executa um processo do sistema.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `command` | string | Sim | Comando a executar |
| `arguments` | string | Não | Argumentos do comando |
| `workingDirectory` | string | Não | Diretório de trabalho |
| `timeout` | integer | Não | Timeout em milissegundos (por defeito: 30000) |

**Permissão necessária**: `system:process`

### EnvironmentGetTool

Obtém variáveis de ambiente.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `name` | string | Não | Nome da variável (sem nome = todas) |

**Permissão necessária**: `system:environment`

### ClipboardTool

Lê ou escreve na área de transferência.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `action` | string | Sim | "read" ou "write" |
| `content` | string | Não | Conteúdo a escrever (para ação "write") |

**Permissão necessária**: `system:clipboard`

---

## Ferramentas de compilação dinâmica

### DynamicCompileTool

Compila e executa código C# dinamicamente.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `code` | string | Sim | Código C# a compilar |
| `className` | string | Não | Nome da classe principal |
| `methodName` | string | Não | Nome do método a executar |
| `references` | array | Não | Assemblies adicionais a referenciar |

**Permissão necessária**: `compile:roslyn` + `compile:execute`

**Restrições de segurança**:
- Análise estática de padrões perigosos
- Execução em sandbox com recursos limitados
- Timeout de execução configurável

---

## Ferramentas de armazenamento

### StorageReadTool

Lê um valor do armazenamento.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `key` | string | Sim | Chave a ler |

### StorageWriteTool

Escreve um valor no armazenamento.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `key` | string | Sim | Chave a escrever |
| `value` | string | Sim | Valor a escrever |

### StorageDeleteTool

Elimina um valor do armazenamento.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `key` | string | Sim | Chave a eliminar |

### StorageQueryTool

Consulta valores por intervalo temporal.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `prefix` | string | Não | Prefixo da chave |
| `startTime` | string | Não | Hora de início |
| `endTime` | string | Não | Hora de fim |

---

## Ferramentas de conhecimento

### KnowledgeAddTool

Adiciona uma tripla de conhecimento.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `subject` | string | Sim | Sujeito |
| `predicate` | string | Sim | Predicado |
| `object` | string | Sim | Objeto |
| `confidence` | float | Não | Confiança (0-1, por defeito: 0.8) |

### KnowledgeQueryTool

Consulta conhecimentos.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `subject` | string | Não | Sujeito a pesquisar |
| `predicate` | string | Não | Predicado a pesquisar |
| `object` | string | Não | Objeto a pesquisar |

### KnowledgeSearchTool

Pesquisa textual nos conhecimentos.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `query` | string | Sim | Termos de pesquisa |
| `limit` | integer | Não | Número máximo de resultados |

---

## Ferramentas de chat

### ChatSendTool

Envia uma mensagem a outro Silicon Being.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `targetBeingId` | string | Sim | ID do Being destinatário |
| `message` | string | Sim | Conteúdo da mensagem |

### ChatBroadcastTool

Transmite uma mensagem para todos os Beings.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `message` | string | Sim | Conteúdo da mensagem |

---

## Ferramentas de browser

### BrowserOpenTool

Abre o browser WebView.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `url` | string | Não | URL inicial |

### BrowserNavigateTool

Navega para um URL no browser.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `url` | string | Sim | URL de destino |

### BrowserExecuteScriptTool

Executa JavaScript no browser.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `script` | string | Sim | Código JavaScript a executar |

### BrowserScreenshotTool

Captura uma captura de ecrã da página atual.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `fullPage` | boolean | Não | Captura completa da página (por defeito: false) |

### BrowserCloseTool

Fecha o browser WebView.

---

## Ferramentas de notas de trabalho

### WorkNoteCreateTool

Cria uma nova nota de trabalho.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `summary` | string | Sim | Resumo da nota |
| `content` | string | Sim | Conteúdo da nota |
| `keywords` | string | Não | Palavras-chave separadas por vírgulas |

### WorkNoteUpdateTool

Atualiza uma nota de trabalho existente.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `pageNumber` | integer | Sim | Número da página da nota |
| `summary` | string | Não | Novo resumo |
| `content` | string | Não | Novo conteúdo |
| `keywords` | string | Não | Novas palavras-chave |

### WorkNoteSearchTool

Pesquisa nas notas de trabalho.

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-------------|
| `keyword` | string | Sim | Palavra-chave de pesquisa |
| `maxResults` | integer | Não | Número máximo de resultados |

---

## Registo de ferramentas

### Descoberta automática

As ferramentas são descobertas automaticamente através de reflexão:

```csharp
var toolTypes = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => typeof(ITool).IsAssignableFrom(t) && !t.IsAbstract);
```

### Registo manual

```csharp
ToolManager.Instance.RegisterTool(new MyCustomTool());
```

---

## Criar uma ferramenta personalizada

### 1. Implementar ITool

```csharp
[ToolScenario(Scenario.All)]
public class MyCustomTool : ITool
{
    public string Name => "my_custom_tool";
    public string Description => "A minha ferramenta personalizada";
    
    public ToolDefinition Definition => new ToolDefinition
    {
        Name = Name,
        Description = Description,
        Parameters = new Dictionary<string, ParameterDefinition>
        {
            ["input"] = new() 
            { 
                Type = "string", 
                Description = "Parâmetro de entrada",
                Required = true
            }
        }
    };

    public async Task<ToolResult> ExecuteAsync(ToolCall call)
    {
        var input = call.Parameters["input"].ToString();
        return new ToolResult 
        { 
            Success = true, 
            Output = $"Resultado: {input}" 
        };
    }
}
```

### 2. Adicionar verificação de permissões

```csharp
public async Task<ToolResult> ExecuteAsync(ToolCall call)
{
    var hasPermission = await _permissionManager.CheckPermissionAsync(
        _beingId, "custom:permission", "resource");
    
    if (!hasPermission)
    {
        return new ToolResult 
        { 
            Success = false, 
            Error = "Permissão negada" 
        };
    }
    
    // Lógica da ferramenta
}
```

---

## Próximos passos

- 📚 Ler a [documentação de arquitetura](architecture.md)
- 🔒 Compreender o [sistema de permissões](permission-system.md)
- 🛠️ Consultar o [guia de desenvolvimento](development-guide.md)
- 📖 Explorar a [referência da API](api-reference.md)
