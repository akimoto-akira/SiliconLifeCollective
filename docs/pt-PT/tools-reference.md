# Referência de ferramentas

> **Versão: v0.2.0-alpha**

Este documento detalha todas as ferramentas incorporadas da plataforma Silicon Life Collective.

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [Français](../fr-FR/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md) | [Italiano](../it-IT/tools-reference.md) | [Polski](../pl-PL/tools-reference.md) | **Português**

## Visão geral

O sistema de ferramentas permite aos Silicon Beings interagir com o mundo exterior através de uma interface padronizada. Cada ferramenta implementa a interface `ITool`, descoberta e registada automaticamente pelo `ToolManager` através de reflexão.

### Categorias de ferramentas

- **Ferramentas de gestão do sistema** — Configuração, permissões, compilação dinâmica
- **Ferramentas de comunicação** — Chat, pedidos de rede
- **Ferramentas de armazenamento de dados** — Operações de disco, base de dados, memória, notas de trabalho
- **Ferramentas de gestão de tempo** — Calendário, temporizadores, tarefas
- **Ferramentas de desenvolvimento** — Execução de código, consulta de logs
- **Ferramentas utilitárias** — Informação do sistema, auditoria de tokens, documentação de ajuda, rede de conhecimento
- **Ferramentas de browser** — Automação do browser WebView
- **Ferramentas de plugins** — Ferramentas de terceiros registadas através do sistema de plugins

---

## Lista de ferramentas incorporadas

### 1. Ferramenta de calendário (CalendarTool)

**Nome da ferramenta**: `calendar`

**Descrição**: Conversão e cálculo de datas em 32 sistemas de calendário.

**Operações suportadas**:
- `now` — Obter a hora atual
- `format` — Formatar data
- `add_days` — Adicionar/subtrair dias
- `diff` — Calcular diferença entre datas
- `list_calendars` — Listar todos os calendários suportados
- `get_components` — Obter componentes da data
- `get_now_components` — Obter componentes da hora atual
- `convert` — Converter entre sistemas de calendário

**Sistemas de calendário suportados** (32):
- Gregoriano (Gregorian)
- Lunar Chinês (Chinese Lunar)
- Histórico Chinês (Chinese Historical) — Ganzhi, eras imperiais
- Islâmico (Islamic)
- Hebraico (Hebrew)
- Japonês (Japanese)
- Persa (Persian)
- Maia (Mayan)
- Budista (Buddhist)
- Tibetano (Tibetan)
- E mais 24 outros calendários...

**Exemplo de uso**:
```json
{
  "action": "convert",
  "date": "2026-04-26",
  "from_calendar": "gregorian",
  "to_calendar": "chinese_lunar"
}
```

---

### 2. Ferramenta de chat (ChatTool)

**Nome da ferramenta**: `chat`

**Descrição**: Gestão de sessões de chat e envio de mensagens.

**Operações suportadas**:
- `send_message` — Enviar mensagem
- `get_messages` — Obter histórico de mensagens
- `create_group` — Criar chat de grupo
- `add_member` — Adicionar membro ao grupo
- `remove_member` — Remover membro do grupo
- `get_chat_info` — Obter informação do chat
- `terminate_chat` — Terminar chat (lido sem resposta)

**Exemplo de uso**:
```json
{
  "action": "send_message",
  "target_id": "being-uuid-or-user-0",
  "message": "Olá, vamos colaborar!"
}
```

---

### 3. Ferramenta de configuração (ConfigTool)

**Nome da ferramenta**: `config`

**Descrição**: Leitura e modificação da configuração do sistema.

**Operações suportadas**:
- `read` — Ler item de configuração
- `write` — Escrever item de configuração
- `list` — Listar todas as configurações
- `get_ai_config` — Obter configuração do cliente IA
- `set_ai_config` — Definir configuração do cliente IA

**Exemplo de uso**:
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

---

### 4. Ferramenta do Curator (CuratorTool) 🔒

**Nome da ferramenta**: `curator`

**Requisito de permissão**: Apenas para o Silicon Curator

**Descrição**: Ferramenta de gestão do sistema exclusiva do Silicon Curator.

**Operações suportadas**:
- `create_being` — Criar novo Silicon Being
- `list_beings` — Listar todos os Silicon Beings
- `get_being_info` — Obter informação do Being
- `assign_task` — Atribuir tarefa
- `manage_permissions` — Gerir permissões

**Exemplo de uso**:
```json
{
  "action": "create_being",
  "name": "Assistente",
  "soul_file": "assistant_soul.md"
}
```

---

### 5. Ferramenta de base de dados (DatabaseTool)

**Nome da ferramenta**: `database`

**Descrição**: Consultas e operações em base de dados estruturada.

**Operações suportadas**:
- `query` — Consultar dados
- `insert` — Inserir dados
- `update` — Atualizar dados
- `delete` — Eliminar dados
- `create_table` — Criar tabela
- `list_tables` — Listar todas as tabelas

**Exemplo de uso**:
```json
{
  "action": "query",
  "table": "users",
  "conditions": {"status": "active"},
  "limit": 100
}
```

---

### 6. Ferramenta de disco (DiskTool)

**Nome da ferramenta**: `disk`

**Descrição**: Operações no sistema de ficheiros e pesquisa local.

**Operações suportadas**:
- `read` — Ler ficheiro
- `write` — Escrever ficheiro
- `list` — Listar diretório
- `delete` — Eliminar ficheiro
- `create_directory` — Criar diretório
- `search_files` — Pesquisar ficheiros
- `search_content` — Pesquisar conteúdo de ficheiros
- `count_lines` — Contar linhas
- `read_lines` — Ler linhas específicas
- `replace_text` — Substituir texto

**Requisito de permissão**: `disk:read`, `disk:write`

**Exemplo de uso**:
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

---

### 7. Ferramenta de compilação dinâmica (DynamicCompileTool) 🔒

**Nome da ferramenta**: `compile`

**Descrição**: Compilação dinâmica de código C# (para auto-evolução dos Silicon Beings).

**Operações suportadas**:
- `compile_class` — Compilar classe
- `compile_callback` — Compilar função de callback de permissão
- `validate_code` — Validar segurança do código

**Mecanismos de segurança**:
- Controlo de referências na compilação (exclusão de assemblies perigosos)
- Análise estática de código no runtime
- Armazenamento encriptado com AES-256

**Exemplo de uso**:
```json
{
  "action": "compile_class",
  "code": "public class MyBeing : SiliconBeingBase { ... }"
}
```

---

### 8. Ferramenta de execução de código (ExecuteCodeTool) 🔒

**Nome da ferramenta**: `execute_code`

**Requisito de permissão**: Apenas para o Silicon Curator

**Descrição**: Compila e executa fragmentos de código C#.

**Operações suportadas**:
- `run_script` — Executar script de código

**Exemplo de uso**:
```json
{
  "action": "run_script",
  "code": "return DateTime.Now.ToString();",
  "timeout": 30
}
```

---

### 9. Ferramenta de ajuda (HelpTool)

**Nome da ferramenta**: `help`

**Descrição**: Obter documentação de ajuda do sistema e guias de utilização.

**Operações suportadas**:
- `get_topics` — Obter lista de tópicos de ajuda
- `get_topic` — Obter detalhes de um tópico específico
- `search` — Pesquisar documentação de ajuda

**Exemplo de uso**:
```json
{
  "action": "get_topics"
}
```

---

### 10. Ferramenta de rede de conhecimento (KnowledgeTool)

**Nome da ferramenta**: `knowledge`

**Descrição**: Operações no grafo de conhecimento (baseado em triplas: sujeito-relação-objeto).

**Operações suportadas**:
- `add` — Adicionar tripla de conhecimento
- `query` — Consultar conhecimento
- `update` — Atualizar conhecimento
- `delete` — Eliminar conhecimento
- `search` — Pesquisar conhecimento
- `get_path` — Obter caminho de conhecimento
- `validate` — Validar conhecimento
- `stats` — Obter estatísticas

**Exemplo de uso**:
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

### 11. Ferramenta de logs (LogTool)

**Nome da ferramenta**: `log`

**Descrição**: Consulta do histórico de operações e histórico de conversações.

**Operações suportadas**:
- `query_logs` — Consultar logs do sistema
- `query_conversations` — Consultar histórico de conversações
- `get_stats` — Obter estatísticas de logs

**Exemplo de uso**:
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

### 12. Ferramenta de memória (MemoryTool)

**Nome da ferramenta**: `memory`

**Descrição**: Gestão da memória de longo e curto prazo dos Silicon Beings.

**Operações suportadas**:
- `read` — Ler memória
- `write` — Escrever memória
- `search` — Pesquisar memória
- `delete` — Eliminar memória
- `list` — Listar memórias
- `get_stats` — Obter estatísticas de memória
- `compress` — Comprimir memória

**Exemplo de uso**:
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

### 13. Ferramenta de rede (NetworkTool)

**Nome da ferramenta**: `network`

**Descrição**: Fazer pedidos HTTP/HTTPS.

**Operações suportadas**:
- `get` — Pedido GET
- `post` — Pedido POST
- `put` — Pedido PUT
- `delete` — Pedido DELETE
- `download` — Descarregar ficheiro
- `upload` — Carregar ficheiro

**Requisito de permissão**: `network:http`

**Exemplo de uso**:
```json
{
  "action": "get",
  "url": "https://api.example.com/data"
}
```

---

### 14. Ferramenta de permissões (PermissionTool) 🔒

**Nome da ferramenta**: `permission`

**Requisito de permissão**: Apenas para o Silicon Curator

**Descrição**: Gestão de permissões e listas de controlo de acesso.

**Operações suportadas**:
- `query_permission` — Consultar permissão
- `manage_acl` — Gerir ACL global
- `get_callback` — Obter função de callback de permissão
- `set_callback` — Definir função de callback de permissão

**Exemplo de uso**:
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

### 15. Ferramenta de projeto (ProjectTool)

**Nome da ferramenta**: `project`

**Descrição**: Gestão de espaços de trabalho de projeto.

**Operações suportadas**:
- `create` — Criar projeto
- `list` — Listar projetos
- `get_info` — Obter informação do projeto
- `update` — Atualizar projeto
- `archive` — Arquivar projeto

**Exemplo de uso**:
```json
{
  "action": "create",
  "name": "O Meu Projeto",
  "description": "Descrição do projeto"
}
```

---

### 16. Ferramenta de tarefas de projeto (ProjectTaskTool)

**Nome da ferramenta**: `project_task`

**Descrição**: Gestão de tarefas de projeto.

**Operações suportadas**:
- `create` — Criar tarefa
- `list` — Listar tarefas
- `update` — Atualizar tarefa
- `complete` — Completar tarefa
- `get_stats` — Obter estatísticas de tarefas

**Exemplo de uso**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "Descrição da tarefa a completar",
  "priority": 5
}
```

---

### 17. Ferramenta de notas de trabalho de projeto (ProjectWorkNoteTool)

**Nome da ferramenta**: `project_work_note`

**Descrição**: Gestão de notas de trabalho de projeto (públicas, semelhante a um caderno de trabalho).

**Operações suportadas**:
- `create` — Criar nota
- `read` — Ler nota
- `update` — Atualizar nota
- `delete` — Eliminar nota
- `list` — Listar notas
- `search` — Pesquisar notas
- `directory` — Gerar índice

**Exemplo de uso**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "Módulo de autenticação de utilizadores concluído",
  "content": "## Detalhes de implementação\n\n- Utilização de JWT token",
  "keywords": "autenticação,JWT"
}
```

---

### 18. Ferramenta do sistema (SystemTool)

**Nome da ferramenta**: `system`

**Descrição**: Obter informação do sistema e utilização de recursos.

**Operações suportadas**:
- `info` — Obter informação do sistema
- `resource_usage` — Obter utilização de recursos
- `find_process` — Encontrar processo
- `list_beings` — Listar Silicon Beings

**Exemplo de uso**:
```json
{
  "action": "info"
}
```

---

### 19. Ferramenta de tarefas (TaskTool)

**Nome da ferramenta**: `task`

**Descrição**: Gestão de tarefas pessoais dos Silicon Beings.

**Operações suportadas**:
- `create` — Criar tarefa
- `list` — Listar tarefas
- `update` — Atualizar tarefa
- `complete` — Completar tarefa
- `delete` — Eliminar tarefa
- `get_dependencies` — Obter dependências

**Exemplo de uso**:
```json
{
  "action": "create",
  "description": "Rever código",
  "priority": 5
}
```

---

### 20. Ferramenta de temporizador (TimerTool)

**Nome da ferramenta**: `timer`

**Descrição**: Criação e gestão de temporizadores.

**Operações suportadas**:
- `create` — Criar temporizador
- `list` — Listar temporizadores
- `delete` — Eliminar temporizador
- `pause` — Pausar temporizador
- `resume` — Retomar temporizador
- `get_execution_history` — Obter histórico de execuções

**Exemplo de uso**:
```json
{
  "action": "create",
  "interval": 3600,
  "repeat": true,
  "message": "Lembrete horário"
}
```

---

### 21. Ferramenta de auditoria de tokens (TokenAuditTool) 🔒

**Nome da ferramenta**: `token_audit`

**Requisito de permissão**: Apenas para o Silicon Curator

**Descrição**: Consulta e agregação da utilização de tokens IA.

**Operações suportadas**:
- `get_usage` — Obter estatísticas de utilização de tokens
- `get_by_being` — Obter utilização por Being
- `get_by_model` — Obter utilização por modelo
- `get_trend` — Obter tendência de utilização
- `export` — Exportar dados

**Exemplo de uso**:
```json
{
  "action": "get_usage",
  "start_date": "2026-04-01",
  "end_date": "2026-04-26"
}
```

---

### 22. Ferramenta de browser WebView (WebViewBrowserTool)

**Nome da ferramenta**: `webview`

**Descrição**: Automação de browser baseada em Playwright.

**Operações suportadas**:
- `open_browser` — Abrir browser
- `close_browser` — Fechar browser
- `navigate` — Navegar para URL
- `click` — Clicar em elemento
- `input` — Introduzir texto
- `get_page_text` — Obter texto da página
- `get_screenshot` — Obter captura de ecrã
- `execute_script` — Executar JavaScript
- `wait_for_element` — Aguardar que elemento apareça
- `get_browser_status` — Obter estado do browser

**Características**:
- Instância independente por Silicon Being
- Cookies e sessão completamente isolados
- Completamente invisível para o utilizador (modo headless)
- Suporte completo a JavaScript e CSS

**Exemplo de uso**:
```json
{
  "action": "navigate",
  "url": "https://example.com"
}
```

---

### 23. Ferramenta de notas de trabalho (WorkNoteTool)

**Nome da ferramenta**: `work_note`

**Descrição**: Gestão de notas de trabalho pessoais dos Silicon Beings (privadas, semelhante a um diário).

**Operações suportadas**:
- `create` — Criar nota
- `read` — Ler nota
- `update` — Atualizar nota
- `delete` — Eliminar nota
- `list` — Listar notas
- `search` — Pesquisar notas
- `directory` — Gerar índice

**Exemplo de uso**:
```json
{
  "action": "create",
  "summary": "Módulo de autenticação de utilizadores concluído",
  "content": "## Detalhes de implementação\n\n- Utilização de JWT token\n- Suporte a OAuth2",
  "keywords": "autenticação,JWT,OAuth2"
}
```

---

### 24. Ferramenta de hot reload (HotReloadTool)

**Nome da ferramenta**: `hot_reload`

**Descrição**: Suporta a compilação automática, atualização de ficheiros e reinício do SiliconLife.Fast em execução, sem intervenção manual.

**Operações suportadas**:
- `execute` — Executar o fluxo completo de compilação, cópia e reinício
- `build_only` — Apenas compilar o projeto, sem copiar e reiniciar

**Fluxo de trabalho**:
1. Compilar o projeto SiliconLife.Fast
2. Encerrar graciosamente a instância Fast atualmente em execução (via HTTP API)
3. Aguardar que o processo termine e a porta seja libertada
4. Copiar o resultado da compilação para o diretório de destino (saltando os ficheiros do HotReload)
5. Reiniciar a instância Fast

**Características**:
- Deteção e encerramento automático do processo antigo
- Cópia segura de ficheiros (não sobrescreve o HotReload.exe)
- Mecanismo de espera pela libertação da porta
- Suporte a configuração de porta personalizada

**Exemplo de uso**:
```json
{
  "action": "execute",
  "project_path": "src/SiliconLife.Fast",
  "source_path": "src/SiliconLife.Fast/bin/Debug/net9.0",
  "configuration": "Debug",
  "port": 8080
}
```

**Descrição dos parâmetros**:
- `project_path`: Caminho do projeto (relativo à raiz da solução)
- `source_path`: Diretório de saída da compilação
- `configuration`: Configuração de compilação (Debug/Release)
- `port`: Porta Web da instância Fast (por defeito 8080)

**Notas**:
- Aplicável apenas à versão SiliconLife.Fast
- Requer que o HotReload.exe esteja no diretório tools/HotReload
- Haverá uma breve interrupção do serviço durante o reinício (aprox. 3-5 segundos)

---

## Fluxo de chamada de ferramentas

```
┌──────────┐
│   IA     │ Retorna tool_calls
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ Pesquisa e valida o direito de uso da ferramenta
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ Verifica a cadeia de permissões
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ Executa operações de acesso a recursos
└────┬─────────┘
     ↓
┌──────────┐
│   IA     │ Recebe o resultado da ferramenta, continua a pensar
└──────────┘
```

## Verificação de permissões

Todas as execuções de ferramentas passam pela cadeia de permissões de 5 níveis:

1. **UserFrequencyCache** — Cache de frequência de permissão/negação do utilizador
2. **IPermissionCallback** — Função de callback de permissão personalizada
3. **Ramificação**:
   - IsCurator → **IPermissionAskHandler** — O Curator pede confirmação ao utilizador
   - Non-curador → **GlobalACL** — Lista de controlo de acesso global

## Criar uma ferramenta personalizada

### Passo 1: Implementar a interface ITool

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    
    public string Description => "Descrição da ferramenta";
    
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

### Passo 2: Adicionar ao projeto

Colocar o ficheiro da ferramenta no diretório `src/SiliconLife.Common/Tools/` (ferramentas partilhadas) ou no diretório `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` (ferramentas específicas da versão). O `ToolManager` descobre e regista automaticamente as ferramentas através de reflexão no arranque.

### Passo 2a: Registar ferramenta através de plugin

Também é possível registar ferramentas personalizadas através do sistema de plugins:

1. Implementar a interface `ITool` no projeto do plugin
2. Compilar a DLL do plugin e colocá-la no diretório de plugins
3. `ToolManager.ScanAllPluginAssemblies()` pesquisa automaticamente implementações ITool em todos os plugins carregados
4. As ferramentas do plugin estão sujeitas ao mesmo sistema de permissões

### Passo 3: (Opcional) Marcar como exclusiva do Curator

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // Apenas o Silicon Curator pode aceder
}
```

## Boas práticas

### 1. Validar sempre os parâmetros

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("Parâmetro obrigatório em falta: required_param");
}
```

### 2. Lidar com erros de forma elegante

```csharp
try
{
    // Executar operação
}
catch (Exception ex)
{
    Logger.Error($"Ferramenta {Name} falhou: {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. Respeitar o sistema de permissões

Nunca contornar as verificações de permissões. Aceder sempre aos recursos através do executor:

```csharp
var permission = await permissionManager.CheckAsync(request);
if (!permission.Allowed)
{
    return ToolResult.Denied(permission.Reason);
}
```

### 4. Fornecer descrições claras das ferramentas

Ajudar a IA a compreender quando e como usar a ferramenta:

```csharp
public string Description => 
    "Utilizado para converter datas entre diferentes sistemas de calendário." +
    "Requer os parâmetros 'date', 'from_calendar' e 'to_calendar'.";
```

## Resolução de problemas

### Ferramenta não encontrada

**Problema**: A IA tenta chamar uma ferramenta que não existe.

**Solução**:
- Verificar se o nome da ferramenta corresponde exatamente
- Validar que o ficheiro da ferramenta está no diretório `Tools/`
- Reconstruir o projeto (`dotnet build`)

### Permissão negada

**Problema**: A execução da ferramenta falha, retornando erro de permissão.

**Solução**:
- Verificar o registo de auditoria de permissões
- Validar que o Silicon Being possui as permissões necessárias
- Verificar as definições da ACL global
- Se for o Curator, verificar se a ferramenta está marcada com `[SiliconManagerOnly]`

### Execução da ferramenta retorna erro

**Problema**: A ferramenta executa mas retorna um resultado de falha.

**Solução**:
- Verificar a mensagem de erro retornada pela ferramenta
- Validar que o formato dos parâmetros de entrada está correto
- Consultar os logs do sistema para informação detalhada do erro
- Testar a funcionalidade da ferramenta de forma independente

## Próximos passos

- 📚 Ler a [documentação de arquitetura](architecture.md)
- 🛠️ Consultar o [guia de desenvolvimento](development-guide.md)
- 🔒 Compreender o [sistema de permissões](permission-system.md)
- 🚀 Ver o [guia de introdução](getting-started.md)
