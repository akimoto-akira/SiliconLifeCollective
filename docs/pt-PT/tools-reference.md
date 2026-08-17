# Referência de Ferramentas

> **Versão: v0.2.0-alpha**

Este documento descreve detalhadamente todas as ferramentas incorporadas da plataforma Silicon Life Collective.

[English](../en/tools-reference.md) | [Deutsch](../de-DE/tools-reference.md) | [中文](../zh-CN/tools-reference.md) | [繁體中文](../zh-HK/tools-reference.md) | [Español](../es-ES/tools-reference.md) | [日本語](../ja-JP/tools-reference.md) | [한국어](../ko-KR/tools-reference.md) | [Čeština](../cs-CZ/tools-reference.md) | [Русский](../ru-RU/tools-reference.md)

## Visão Geral

O sistema de ferramentas permite que os Silicon Beings interajam com o mundo exterior através de uma interface padronizada. Cada ferramenta implementa a interface `ITool`, descoberta e registada automaticamente pelo `ToolManager` via reflexão.

### Categorização de Ferramentas

- **Ferramentas de gestão do sistema** — Configuração, permissões, compilação dinâmica, gestão do Curator
- **Ferramentas de comunicação** — Chat, pedidos de rede
- **Ferramentas de armazenamento de dados** — Operações de disco, base de dados, memória, notas de trabalho
- **Ferramentas de gestão de tempo** — Calendário, temporizadores, tarefas
- **Ferramentas de desenvolvimento** — Execução de código, consulta de registos
- **Ferramentas utilitárias** — Informação do sistema, auditoria de tokens, documentação de ajuda, rede de conhecimento
- **Ferramentas de navegador** — Automação do navegador WebView
- **Ferramentas de projecto** — Gestão de projecto, tarefas de projecto, notas de trabalho de projecto, trabalho de projecto
- **Ferramentas de plugins** — Ferramentas de terceiros registadas através do sistema de plugins

### Sistema de Cenários de Ferramentas

Cada ferramenta declara os seus cenários disponíveis através do atributo `[ToolScenario]`:

| Flag de Cenário | Valor | Descrição |
|----------|------|-------------|
| `Chat` | `1 << 0` | Cenário de chat (quando o utilizador conversa com o Silicon Being) |
| `Task` | `1 << 1` | Cenário de tarefa (quando o Silicon Being executa uma tarefa) |
| `Timer` | `1 << 2` | Cenário de temporizador (quando o Silicon Being executa uma tarefa temporizada) |
| `MemoryCompression` | `1 << 3` | Cenário de compressão de memória |
| `Project` | `1 << 4` | Cenário de projecto (modo ThinkOnProject) |
| `All` | Todos os acima | Disponível em todos os cenários |

Além disso, as ferramentas marcadas com `[ChatOnly]` estão disponíveis apenas no cenário de chat (como o HelpTool), não aparecendo nos cenários de tarefa e temporizador.

---

## Lista de Ferramentas Incorporadas

### 1. Ferramenta de Calendário (CalendarTool)

**Nome da ferramenta**: `calendar`

**Descrição**: Suporta conversão e cálculo de datas em 32 sistemas de calendário.

**Operações suportadas**:
- `now` — Obter a hora actual
- `format` — Formatar data
- `add_days` — Adicionar/subtrair dias
- `diff` — Calcular diferença entre datas
- `list_calendars` — Listar todos os calendários suportados
- `get_components` — Obter componentes da data
- `get_now_components` — Obter componentes da hora actual
- `convert` — Conversão entre sistemas de calendário

**Sistemas de calendário suportados** (32):
- Gregoriano (Gregorian)
- Lunar Chinês (Chinese Lunar)
- Histórico Chinês (Chinese Historical) — Era Ganzhi, era imperial
- Islâmico (Islamic)
- Hebraico (Hebrew)
- Japonês (Japanese)
- Persa (Persian)
- Maia (Mayan)
- Budista (Buddhist)
- Tibetano (Tibetan)
- E 24 outros calendários...

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

### 2. Ferramenta de Chat (ChatTool)

**Nome da ferramenta**: `chat`

**Descrição**: Gerir sessões de chat e envio de mensagens.

**Operações suportadas**:
- `send_message` — Enviar mensagem
- `get_messages` — Obter mensagens do histórico
- `create_group` — Criar chat de grupo
- `add_member` — Adicionar membro ao grupo
- `remove_member` — Remover membro do grupo
- `get_chat_info` — Obter informações do chat
- `terminate_chat` — Terminar chat (lido sem responder)

**Exemplo de uso**:
```json
{
  "action": "send_message",
  "target_id": "being-uuid-or-user-0",
  "message": "Olá, vamos colaborar!"
}
```

---

### 3. Ferramenta de Configuração (ConfigTool)

**Nome da ferramenta**: `config`

**Descrição**: Ler e modificar a configuração do sistema.

**Operações suportadas**:
- `read` — Ler item de configuração
- `write` — Escrever item de configuração
- `list` — Listar todas as configurações
- `get_ai_config` — Obter configuração do cliente de IA
- `set_ai_config` — Definir configuração do cliente de IA

**Exemplo de uso**:
```json
{
  "action": "read",
  "key": "AIClients.Ollama.Model"
}
```

---

### 4. Ferramenta do Curator (CuratorTool) 🔒

**Nome da ferramenta**: `silicon_manager`

**Requisito de permissão**: Apenas para o Silicon Curator (`[SiliconManagerOnly]`)

**Cenários disponíveis**: Chat, Task, Timer

**Descrição**: Ferramenta de gestão do sistema exclusiva do Silicon Curator, usada para gerir a criação, visualização e reposição dos Silicon Beings.

**Operações suportadas**:
- `list_beings` — Listar todos os Silicon Beings e o seu estado
- `create_being` — Criar novo Silicon Being (requer parâmetros `name` e `soul`)
- `get_code` — Visualizar o código fonte personalizado do Silicon Being
- `reset` — Repor o Silicon Being para a implementação padrão

**Exemplo de uso**:
```json
{
  "action": "create_being",
  "name": "Assistente",
  "soul": "És um assistente útil..."
}
```

---

### 5. Ferramenta de Base de Dados (DatabaseTool)

**Nome da ferramenta**: `database`

**Descrição**: Consultas e operações em base de dados estruturada.

**Operações suportadas**:
- `query` — Consultar dados
- `insert` — Inserir dados
- `update` — Actualizar dados
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

### 6. Ferramenta de Disco (DiskTool)

**Nome da ferramenta**: `disk`

**Descrição**: Operações no sistema de ficheiros e pesquisa local.

**Operações suportadas**:
- `read` — Ler ficheiro
- `write` — Escrever ficheiro
- `list` — Listar directório
- `delete` — Eliminar ficheiro
- `create_directory` — Criar directório
- `search_files` — Pesquisar ficheiros
- `search_content` — Pesquisar conteúdo de ficheiros
- `count_lines` — Contar linhas
- `read_lines` — Ler linhas específicas
- `replace_text` — Substituir texto

**Requisito de permissão**: `FileAccess`

**Exemplo de uso**:
```json
{
  "action": "read",
  "path": "/data/file.txt"
}
```

---

### 7. Ferramenta de Compilação Dinâmica (DynamicCompileTool) 🔒

**Nome da ferramenta**: `compile`

**Descrição**: Compilação dinâmica de código C# (usada para auto-evolução dos Silicon Beings).

**Operações suportadas**:
- `compile_class` — Compilar classe
- `compile_callback` — Compilar função de callback de permissões
- `validate_code` — Validar segurança do código

**Mecanismos de segurança**:
- Controlo de referências na compilação (exclusão de assemblies perigosos)
- Análise estática de código em tempo de execução
- Armazenamento encriptado com AES-256

**Exemplo de uso**:
```json
{
  "action": "compile_class",
  "code": "public class MyBeing : SiliconBeingBase { ... }"
}
```

---

### 8. Ferramenta de Execução de Código (ExecuteCodeTool) 🔒

**Nome da ferramenta**: `execute_code`

**Requisito de permissão**: Apenas para o Silicon Curator

**Descrição**: Compilar e executar fragmentos de código C#.

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

### 9. Ferramenta de Ajuda (HelpTool)

**Nome da ferramenta**: `help`

**Cenários disponíveis**: Chat (`[ChatOnly]`, apenas disponível no cenário de chat)

**Descrição**: Pesquisar e obter conteúdo da documentação de ajuda do sistema, permitindo à IA consultar métodos de utilização das funcionalidades do sistema.

**Operações suportadas**:
- `list` — Listar todos os IDs de tópicos de ajuda
- `search` — Pesquisar documentação de ajuda por palavra-chave
- `get` — Obter conteúdo da documentação de ajuda para o ID especificado

**Exemplo de uso**:
```json
{
  "action": "search",
  "keyword": "permissões"
}
```

---

### 10. Ferramenta de Rede de Conhecimento (KnowledgeTool)

**Nome da ferramenta**: `knowledge`

**Descrição**: Operações no grafo de conhecimento (baseado em triplas: sujeito-relação-objecto).

**Operações suportadas**:
- `add` — Adicionar tripla de conhecimento
- `query` — Consultar conhecimento
- `update` — Actualizar conhecimento
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

### 11. Ferramenta de Consulta MCP (McpTool)

**Nome da ferramenta**: `mcp`

**Descrição**: Consultar o estado da integração MCP (Model Context Protocol) — servidores externos ligados, as ferramentas que fornecem e como as chamar. Esta é uma ferramenta apenas de leitura: a adição/remoção de servidores só pode ser feita pelo utilizador através da Web UI; a IA não pode modificar a lista de servidores.

**Operações suportadas**:
- `status` — Visão global (estado de ativação, número de servidores, número de ferramentas)
- `list_servers` — Listar servidores configurados (com estado de ligação e número de ferramentas)
- `list_tools` — Listar ferramentas disponíveis (com nome de prefixo `mcp_{server}_{tool}`, descrição e esquema de parâmetros; `server_id` opcional para filtrar um único servidor)

**Exemplo de utilização**:
```json
{
  "action": "list_tools",
  "server_id": "filesystem",
  "include_schema": true
}
```

**Ferramentas embrulhadas MCP**: As ferramentas fornecidas por cada servidor MCP ligado são registadas dinamicamente como ferramentas independentes no Ser de Silício, com o formato de nome `mcp_{serverId}_{toolName}` (ex.: `mcp_filesystem_read_file`). A IA pode chamá-las diretamente pelo nome de prefixo como ferramentas normais, sem necessidade de as encaminhar através desta ferramenta de consulta. As ferramentas embrulhadas são apresentadas na matriz de permissões com uma única ação `execute`, podendo ser desativadas individualmente.

**Cenários**: Todos os cenários (`All`)

---

### 12. Ferramenta de Registos (LogTool)

**Nome da ferramenta**: `log`

**Descrição**: Consultar histórico de operações e histórico de conversações.

**Operações suportadas**:
- `query_logs` — Consultar registos do sistema
- `query_conversations` — Consultar histórico de conversações
- `get_stats` — Obter estatísticas de registos

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

### 13. Ferramenta de Memória (MemoryTool)

**Nome da ferramenta**: `memory`

**Descrição**: Gerir a memória de longo e curto prazo dos Silicon Beings.

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

### 14. Ferramenta de Rede (NetworkTool)

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

### 15. Ferramenta de Permissões (PermissionTool) 🔒

**Nome da ferramenta**: `permission`

**Requisito de permissão**: Apenas para o Silicon Curator

**Descrição**: Gerir permissões e listas de controlo de acesso.

**Operações suportadas**:
- `query_permission` — Consultar permissões
- `manage_acl` — Gerir ACL Global
- `get_callback` — Obter função de callback de permissões
- `set_callback` — Definir função de callback de permissões

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

### 16. Ferramenta de Projecto (ProjectTool) 🔒

**Nome da ferramenta**: `project`

**Requisito de permissão**: Apenas para o Silicon Curator (`[SiliconManagerOnly]`)

**Cenários disponíveis**: Chat, Task, Timer

**Descrição**: Gerir espaços de trabalho de projecto, suportando gestão do ciclo de vida do projecto, atribuição de membros e gestão de funções.

**Operações suportadas**:
- `create` — Criar novo espaço de projecto
- `archive` — Arquivar projecto
- `restore` — Restaurar projecto arquivado
- `destroy` — Destruir projecto e limpar dados (irreversível)
- `list` — Listar todos os projectos
- `get` — Obter detalhes do projecto
- `assign` — Atribuir Silicon Being ao projecto
- `remove` — Remover Silicon Being do projecto
- `update` — Actualizar nome/descrição do projecto
- `list-workflow-templates` — Listar modelos de fluxos de trabalho disponíveis
- `assign_role` — Atribuir função de projecto ao Silicon Being
- `remove_role` — Remover função de projecto do Silicon Being
- `list_roles` — Listar atribuições de funções do projecto

**Exemplo de uso**:
```json
{
  "action": "create",
  "name": "My Project",
  "description": "Descrição do projecto"
}
```

---

### 17. Ferramenta de Tarefas de Projecto (ProjectTaskTool)

**Nome da ferramenta**: `project_task`

**Cenários disponíveis**: Chat, Task, Timer

**Descrição**: Gerir tarefas dentro do espaço de projecto, suportando o ciclo de vida completo das tarefas.

**Operações suportadas**:
- `create` — Criar tarefa de projecto
- `list` — Listar tarefas do projecto
- `get` — Obter detalhes da tarefa
- `update` — Actualizar título/descrição/prioridade da tarefa
- `assign` — Atribuir responsável à tarefa
- `remove_assignee` — Remover responsável da tarefa
- `start` — Iniciar tarefa
- `complete` — Marcar tarefa como concluída
- `fail` — Marcar tarefa como falhada
- `cancel` — Cancelar tarefa
- `delete` — Eliminar tarefa
- `stats` — Obter estatísticas das tarefas

**Exemplo de uso**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "description": "Descrição da tarefa a concluir",
  "priority": 5
}
```

---

### 18. Ferramenta de Notas de Trabalho de Projecto (ProjectWorkNoteTool)

**Nome da ferramenta**: `project_work_note`

**Cenários disponíveis**: Chat, Task, Timer

**Descrição**: Gerir notas de trabalho dentro do espaço de projecto (públicas, semelhante a um caderno de trabalho), suportando gestão de notas em formato de página.

**Operações suportadas**:
- `create` — Criar página de nota (requer `project_id`, `summary` e `content`, opcional `keywords`)
- `read` — Ler página de nota (requer `project_id` e `page_number` ou `note_id`)
- `update` — Actualizar página de nota (requer `project_id`, `page_number` e `content`, opcional `summary` e `keywords`)
- `delete` — Eliminar página de nota (requer `project_id` e `page_number` ou `note_id`)
- `list` — Listar resumos de todas as páginas de notas do projecto
- `directory` — Gerar directório/visão geral das notas
- `search` — Pesquisar notas por palavra-chave (requer `project_id` e `keyword`, opcional `max_results`)

**Exemplo de uso**:
```json
{
  "action": "create",
  "project_id": "project-uuid",
  "summary": "Módulo de autenticação de utilizadores concluído",
  "content": "## Detalhes de Implementação\n\n- Usar JWT token",
  "keywords": "autenticação,JWT"
}
```

---

### 19. Ferramenta de Trabalho de Projecto (ProjectWorkTool) 🔒

**Nome da ferramenta**: `project_work`

**Requisito de permissão**: Apenas para o Silicon Curator (`[SiliconManagerOnly]`)

**Cenários disponíveis**: Project (`[ToolScenario(ToolScenarioFlag.Project)]`, apenas disponível no cenário de projecto)

**Descrição**: Ferramenta de operações de trabalho do projecto, usada pelo Curator para gerir fluxos de trabalho do projecto no cenário ThinkOnProject.

**Operações suportadas**:
- `create-task` — Criar tarefa de projecto
- `assign-task` — Atribuir Silicon Being à tarefa
- `chat` — Enviar mensagem para o chat de grupo do projecto
- `broadcast` — Difundir mensagem para o canal do projecto
- `complete` — Marcar projecto como concluído
- `status` — Obter estado do projecto

**Exemplo de uso**:
```json
{
  "action": "create-task",
  "project_id": "project-uuid",
  "title": "Implementar autenticação de utilizadores"
}
```

---

### 20. Ferramenta de Competências (SkillTool)

**Nome da ferramenta**: `skill`

**Descrição**: Gerir as competências do ser de silício (unidades de capacidade reutilizáveis de "orquestração de ferramentas + modelo de prompt"), suportando criação, listagem, atualização, eliminação, importação e exportação. Os metadados em falta (id, descrição, esquema de parâmetros, etc.) são preenchidos automaticamente pela IA.

**Operações suportadas**:
- `create` — Criar uma nova competência (requer `id` e `system_prompt`; opcionais: `description`, `parameter_schema`, `tool_whitelist`, `tags`, `max_tool_round`, `timeout`, `on_complete`, `trigger_mode`, `auto_trigger_condition`)
- `list` — Listar todas as competências disponíveis (com resumo)
- `update` — Atualizar uma competência existente através de parâmetros (requer `skill_id`)
- `update_from_md` — Atualizar uma competência a partir de uma string Markdown (metadados YAML + corpo do prompt)
- `delete` — Eliminar uma competência (requer `skill_id`)
- `export` — Exportar uma competência como JSON (requer `skill_id`)
- `export_md` — Exportar uma competência como Markdown (requer `skill_id`)
- `import` — Importar uma competência a partir de JSON (requer `json`)
- `import_md` — Importar uma competência a partir de Markdown (requer `markdown`)

**Exemplo de utilização**:
```json
{
  "action": "create",
  "id": "daily_news_digest",
  "description": "Pesquisar notícias de tecnologia de hoje e gerar um resumo",
  "system_prompt": "Por favor, utilize a ferramenta network para pesquisar as últimas notícias sobre {topic} e gerar um resumo de 500 palavras.",
  "parameter_schema": {
    "type": "object",
    "properties": {
      "topic": { "type": "string", "description": "Tópico de notícias" }
    },
    "required": ["topic"]
  },
  "tool_whitelist": ["network", "work_note"],
  "trigger_mode": "Auto",
  "auto_trigger_condition": "schedule",
  "metadata": { "schedule": "0 9 * * *" }
}
```

**Permissões de modificação**: O Curator de Silício pode modificar todas as competências; os seres comuns só podem modificar competências cuja origem seja `Being` ou `User` (não podem modificar competências incorporadas e de plugins).

**Limite de quantidade**: O número de competências personalizadas por ser é limitado pela configuração `MaxCustomSkillsPerBeing` (predefinição: 50).

**Cenário**: Todos os cenários (`All`)

> Para uma descrição completa do sistema de competências (modos de acionamento, lista de permissões, recarregamento a quente, agendamento automático, etc.), consulte o [Guia do Ser de Silício](silicon-being-guide.md#sistema-de-competências).

---

### 21. Ferramenta do Sistema (SystemTool)

**Nome da ferramenta**: `system`

**Descrição**: Obter informações do sistema e utilização de recursos.

**Operações suportadas**:
- `info` — Obter informações do sistema
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

### 22. Ferramenta de Tarefas (TaskTool)

**Nome da ferramenta**: `task`

**Descrição**: Gerir tarefas pessoais dos Silicon Beings.

**Operações suportadas**:
- `create` — Criar tarefa
- `list` — Listar tarefas
- `update` — Actualizar tarefa
- `complete` — Concluir tarefa
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

### 23. Ferramenta de Temporizadores (TimerTool)

**Nome da ferramenta**: `timer`

**Descrição**: Criar e gerir temporizadores.

**Operações suportadas**:
- `create` — Criar temporizador
- `list` — Listar temporizadores
- `delete` — Eliminar temporizador
- `pause` — Pausar temporizador
- `resume` — Retomar temporizador
- `get_execution_history` — Obter histórico de execução

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

### 24. Ferramenta de Auditoria de Tokens (TokenAuditTool) 🔒

**Nome da ferramenta**: `token_audit`

**Requisito de permissão**: Apenas para o Silicon Curator (`[SiliconManagerOnly]`)

**Cenários disponíveis**: Chat, Task, Timer

**Descrição**: Consultar estatísticas e dados de tendência de utilização de Tokens de IA.

**Operações suportadas**:
- `summary` — Obter estatísticas sumárias de utilização de Tokens
- `trend` — Obter pontos de dados de tendência de utilização de Tokens

**Intervalos de tempo suportados**:
- `today` — Últimas 24 horas
- `week` — Últimas 7×24 horas
- `month` — Estatísticas por dia
- `year` — Estatísticas por mês

**Exemplo de uso**:
```json
{
  "action": "summary",
  "time_range": "week"
}
```

---

### 25. Ferramenta de Navegador WebView (WebViewBrowserTool)

**Nome da ferramenta**: `webview_browser`

**Cenários disponíveis**: Chat, Task, Timer

**Descrição**: Automação de navegador baseada em Playwright, fornecendo capacidades completas de navegação web, interacção e extracção de dados.

**Operações suportadas**:
- `open` — Abrir navegador
- `close` — Fechar navegador
- `navigate` — Navegar para URL
- `click` — Clicar em elemento
- `input` — Introduzir texto
- `scroll` — Deslocar página
- `execute_script` — Executar JavaScript
- `get_page_text` — Obter texto da página
- `get_screenshot` — Obter captura de ecrã
- `wait_for_element` — Aguardar aparecimento de elemento
- `get_element_info` — Obter informações do elemento
- `upload_file` — Carregar ficheiro
- `get_browser_status` — Obter estado do navegador
- `set_timeout` — Definir tempo limite
- `clear_session` — Limpar sessão do navegador

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

### 26. Ferramenta de Notas de Trabalho (WorkNoteTool)

**Nome da ferramenta**: `work_note`

**Descrição**: Gerir notas de trabalho pessoais dos Silicon Beings (privadas, semelhante a um diário).

**Operações suportadas**:
- `create` — Criar nota
- `read` — Ler nota
- `update` — Actualizar nota
- `delete` — Eliminar nota
- `list` — Listar notas
- `search` — Pesquisar notas
- `directory` — Gerar directório

**Exemplo de uso**:
```json
{
  "action": "create",
  "summary": "Módulo de autenticação de utilizadores concluído",
  "content": "## Detalhes de Implementação\n\n- Usar JWT token\n- Suportar OAuth2",
  "keywords": "autenticação,JWT,OAuth2"
}
```

---

## Fluxo de Chamada de Ferramentas

```
┌──────────┐
│   IA     │ Retorna tool_calls
└────┬─────┘
     ↓
┌──────────────┐
│ ToolManager  │ Procurar e validar direito de uso da ferramenta
└────┬─────────┘
     ↓
┌──────────────┐
│ Permission   │ Verificar cadeia de permissões
│   Manager    │
└────┬─────────┘
     ↓
┌──────────────┐
│  Executor    │ Executar operação de acesso a recursos
└────┬─────────┘
     ↓
┌──────────┐
│   IA     │ Receber resultado da ferramenta, continuar a pensar
└──────────┘
```

## Verificação de Permissões

Todas as execuções de ferramentas passam pela cadeia de verificação de permissões:

1. **UserFrequencyCache** — Cache de decisões frequentes do utilizador (HighDeny tem prioridade sobre HighAllow)
2. **IPermissionCallback** — Função de callback de permissões personalizada (Allowed/Denied/AskUser)
3. **Ramificação IsCurator** — O Curator pergunta ao utilizador via IPermissionAskHandler; os não Curator consultam a GlobalACL, sem regra correspondente resulta em negação por defeito

## Criar Ferramentas Personalizadas

### Passo 1: Implementar a interface ITool

```csharp
public class MyCustomTool : ITool
{
    public string Name => "my_tool";
    
    public string Description => "Descrição da ferramenta";
    
    public ToolDefinition Definition => new ToolDefinition
    {
        Name = Name,
        Description = Description,
        Parameters = new Dictionary<string, object>
        {
            ["param1"] = new { type = "string", description = "Descrição do parâmetro" }
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

### Passo 2: Adicionar ao Projecto

Colocar o ficheiro da ferramenta no directório `src/SiliconLife.Common/Tools/` (ferramentas partilhadas) ou nos directórios `src/SiliconLife.Default/Tools/` / `src/SiliconLife.Fast/Tools/` (ferramentas específicas da versão). O `ToolManager` descobrirá e registará automaticamente via reflexão no arranque.

### Passo 2a: Registar Ferramenta via Plugin

Também é possível registar ferramentas personalizadas através do sistema de plugins:

1. Implementar a interface `ITool` no projecto do plugin
2. Compilar a DLL do plugin e colocá-la no directório de plugins
3. O `ToolManager.ScanAllPluginAssemblies()` analisará automaticamente as implementações de ITool em todos os plugins carregados
4. As ferramentas do plugin estão sujeitas ao mesmo sistema de permissões

### Passo 3: (Opcional) Marcar como Exclusiva do Curator

```csharp
[SiliconManagerOnly]
public class AdminOnlyTool : ITool
{
    // Apenas o Silicon Curator tem acesso
}
```

### Alternativas: Competências e Ferramentas MCP

Além de escrever classes de ferramentas em C#, existem duas formas de extensão sem necessidade de compilação:

- **Competências (Skill)**: Criar combinações de "orquestração de ferramentas + modelo de prompt" através da Web UI ou da ferramenta `skill`, adequadas para encapsular fluxos de trabalho frequentes em capacidades reutilizáveis. Consulte o [Guia do Ser de Silício — Sistema de Competências](silicon-being-guide.md#sistema-de-competências).
- **Servidor MCP**: Após configurar um servidor MCP externo na Web UI, as suas ferramentas são automaticamente injetadas no formato `mcp_{serverId}_{toolName}`, sem necessidade de escrever qualquer código. Consulte o [Guia da Web UI — Gestão MCP](web-ui-guide.md).

## Melhores Práticas

### 1. Validar Sempre os Parâmetros

```csharp
if (!call.Parameters.ContainsKey("required_param"))
{
    return ToolResult.Failure("Parâmetro obrigatório em falta: required_param");
}
```

### 2. Tratar Erros Elegantemente

```csharp
try
{
    // Executar operação
}
catch (Exception ex)
{
    Logger.Error($"Falha na execução da ferramenta {Name}: {ex.Message}");
    return ToolResult.Failure(ex.Message);
}
```

### 3. Respeitar o Sistema de Permissões

Nunca contornar as verificações de permissões. Aceder sempre aos recursos através de executores:

```csharp
bool allowed = permissionManager.CheckPermission(callerId, permissionType, resource);
if (!allowed)
{
    return ToolResult.Denied("Permission denied");
}
```

### 4. Fornecer Descrições Claras da Ferramenta

Ajudar a IA a compreender quando e como usar a ferramenta:

```csharp
public string Description => 
    "Usado para converter datas entre diferentes sistemas de calendário." +
    "Requer os parâmetros 'date', 'from_calendar' e 'to_calendar'.";
```

## Resolução de Problemas

### Ferramenta Não Encontrada

**Problema**: A IA tenta chamar uma ferramenta que não existe.

**Solução**:
- Verificar se o nome da ferramenta corresponde exactamente
- Confirmar que o ficheiro da ferramenta está no directório `Tools/`
- Reconstruir o projecto (`dotnet build`)

### Permissão Negada

**Problema**: A execução da ferramenta falha, retornando erro de permissão.

**Solução**:
- Verificar os registos de auditoria de permissões
- Confirmar que o Silicon Being tem as permissões necessárias
- Verificar as definições da ACL Global
- Se for o Curator, verificar se a marca `[SiliconManagerOnly]` foi usada

### Execução da Ferramenta Retorna Erro

**Problema**: A ferramenta executa mas retorna um resultado de falha.

**Solução**:
- Verificar a mensagem de erro retornada pela ferramenta
- Validar que o formato dos parâmetros de entrada está correcto
- Consultar os registos do sistema para obter informações detalhadas do erro
- Testar a funcionalidade da ferramenta independentemente

## Próximos Passos

- 📚 Leia o [guia de arquitectura](architecture.md)
- 🛠️ Consulte o [guia de desenvolvimento](development-guide.md)
- 🔒 Compreenda o [sistema de permissões](permission-system.md)
- 🚀 Consulte o [guia de início rápido](getting-started.md)
