# Roteiro

> **Versão: v0.2.0-alpha**

[English](../en/roadmap.md) | [Deutsch](../de-DE/roadmap.md) | [中文](../zh-CN/roadmap.md) | [繁體中文](../zh-HK/roadmap.md) | [Español](../es-ES/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Čeština](../cs-CZ/roadmap.md) | [Русский](../ru-RU/roadmap.md)

## Roteiro de Duas Versões

### SiliconLife.Default (Versão Padrão)
- **Posicionamento**: Implementação padrão, usada principalmente para verificar a viabilidade da arquitectura
- **Estado actual**: Fases 1-10.6 concluídas, sistema em funcionamento estável
- **Descrição do papel**: Como implementação de referência para verificação da arquitectura, garante a correcção e viabilidade do desenho arquitectónico principal

### SiliconLife.Fast (Versão de Alto Desempenho)
- **Posicionamento**: Versão de produção recomendada
- **Estado actual**: Migração da arquitectura base concluída, motor de armazenamento SpeedyPack e sistema de plugins implementados
- **Descrição do papel**: Com base na arquitectura verificada pela versão Default, realiza optimização profunda de desempenho e melhorias de funcionalidades de nível de produção, sendo a escolha preferida para implantação real

**Plano de desenvolvimento da versão Fast**:
- ✅ Fase 1: Migração da estrutura do projecto base e sistema de configuração
- ✅ Fase 2: Migração da Web UI e controladores
- ✅ Fase 3: Optimização do sistema de armazenamento (armazenamento em memória SpeedyPack + persistência assíncrona)
- ✅ Fase 3.5: Ferramenta de gestão SpeedyPack (aplicação Avalonia UI SiliconLife.Speedy.Manager)
- ✅ Fase 3.6: Sistema de plugins (interface IPlugin, sandbox segura, isolamento AssemblyLoadContext)
- ✅ Fase 4: Aplicação de janela Avalonia (aplicação de ambiente de trabalho multiplataforma, bandeja do sistema Windows/macOS, janela de estado Linux)

---

## Princípios Orientadores

Cada fase termina com um sistema **executável e observável**. Nenhuma fase produz "um monte de infraestrutura sem nada para demonstrar".

---

## ~~Fase 1: Pode Conversar~~ ✅ Concluída

**Objectivo**: Entrada na consola → Chamada de IA → Saída na consola. Unidade mínima verificável.

| # | Módulo | Descrição |
|---|--------|-------------|
| 1.1 | Estrutura da solução e projecto | Criar `SiliconLifeCollective.sln`, contendo `src/SiliconLife.Core/` (biblioteca principal) e `src/SiliconLife.Default/` (implementação padrão + ponto de entrada) |
| 1.2 | Configuração (mínima) | Singleton + desserialização JSON. Ler `config.json`. Auto-gerar valores predefinidos se ausente |
| 1.3 | Localização (mínima) | Classe abstracta `LocalizationBase`, implementação `ZhCN`. Adicionar `Language` à configuração |
| 1.4 | OllamaClient (mínimo) | Interface `IAIClient`, chamada HTTP ao Ollama local `/api/chat`. Sem streaming, sem chamadas de ferramentas |
| 1.5 | I/O da consola | `while(true) + Console.ReadLine()`, ler entrada → chamar IA → imprimir resposta |
| 1.6 | Cabeçalho de copyright | Adicionar cabeçalho Apache 2.0 a todos os ficheiros fonte C# |

**Entregável**: Programa de chat por consola que conversa com um modelo Ollama local.

**Verificação**: Executar o programa, digitar "olá", ver a resposta da IA.

---

## ~~Fase 2: Com Esqueleto~~ ✅ Concluída

**Objectivo**: Substituir o "loop nu" pela estrutura do framework. Comportamento inalterado.

| # | Módulo | Descrição |
|---|--------|-------------|
| 2.1 | Armazenamento (mínimo) | Interface `IStorage` (Read/Write/Exists/Delete, pares chave-valor). Implementação `FileSystemStorage`. Classe de instância (não estática). Acesso directo ao sistema de ficheiros — **A IA não controla o IStorage** |
| 2.2 | Ciclo principal + Objectos Tick | Loop infinito, intervalo de relógio preciso (`Stopwatch` + `Thread.Sleep`). Escalonamento por prioridade |
| 2.3 | Padronização do IAIClient | Interface `IAIClientFactory`. OllamaClient refactorizado para implementar a interface padrão |
| 2.4 | Migração da consola | Migrar `while(true)` para objectos Tick orientados pelo ciclo principal. Comportamento idêntico à Fase 1 |

**Entregável**: Ciclo principal executando relógios, chat por consola ainda funciona.

**Verificação**: Registar um objecto Tick de teste que imprima a contagem de relógio a cada segundo; chat por consola ainda funciona.

---

## ~~Fase 3: Com Alma~~ ✅ Concluída

**Objectivo**: O primeiro Silicon Being vive no framework.

| # | Módulo | Descrição |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Classe abstracta base, contendo Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService. Abstract `Tick()` e `ExecuteOneRound()` |
| 3.2 | Carregamento do Ficheiro da Alma | `SoulFileManager`: ler `soul.md` do directório de dados do being |
| 3.3 | ContextManager (mínimo) | Ligar Ficheiro da Alma + mensagens recentes → chamar IA → obter resposta. Sem chamadas de ferramentas, sem persistência |
| 3.4 | ISiliconBeingFactory | Interface de fábrica para criar instâncias de beings |
| 3.5 | SiliconBeingManager (mínimo) | Herda de objecto Tick (prioridade=0). Iterar todos os beings, chamar os seus Tick sequencialmente |
| 3.6 | DefaultSiliconBeing | Implementação de comportamento padrão. Verificar mensagens não lidas → criar ContextManager → ExecuteOneRound → saída |
| 3.7 | Estrutura de directório do being | `DataDirectory/SiliconManager/{GUID}/`, contendo `soul.md` e `state.json` |

**Entregável**: Silicon Being orientado pelo ciclo principal, recebendo entrada da consola, carregando o Ficheiro da Alma, chamando a IA.

**Verificação**: Entrada na consola → relógio do ciclo principal dispara → being processa (com comportamento orientado pelo Ficheiro da Alma) → resposta da IA. O estilo de resposta deve ser diferente da Fase 1.

---

## ~~Fase 4: Com Memória~~ ✅ Concluída

**Objectivo**: As conversas persistem após reinício.

| # | Módulo | Descrição |
|---|--------|-------------|
| 4.1 | ChatSystem | Conceito de canal (dois GUIDs = um canal). Modelo de mensagens com persistência. Sem chat de grupo |
| 4.2 | IIMProvider + IMManager | Interface `IIMProvider`. `ConsoleProvider` como canal de IM formal. `IMManager` encaminha mensagens |
| 4.3 | Melhoria do ContextManager | Extrair histórico do sistema de chat. Persistir respostas da IA. Suportar continuação de chamadas de ferramentas em múltiplas rondas |
| 4.4 | Modelo IMessage | Modelo de mensagem unificado partilhado pelo sistema de chat e gestor de IM |

**Entregável**: Sistema de chat com memória persistente.

**Verificação**: Conversar algumas rondas → sair → reiniciar → perguntar "Sobre o que conversámos?" → o being consegue responder.

---

## ~~Fase 5: Pode Agir (Sistema de Ferramentas)~~ ✅ Concluída

**Objectivo**: Os Silicon Beings podem executar operações, não apenas conversar.

| # | Módulo | Descrição |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | Interface `ITool`, contendo Name, Description, Execute. `ToolResult` contém Success, Message, Data |
| 5.2 | ToolManager | Instância por being. Descoberta de ferramentas baseada em reflexão. Suporte ao atributo `[SiliconManagerOnly]` |
| 5.3 | IAIClient: Suporte a chamadas de ferramentas | Analisar tool_calls da IA. Ciclo: executar ferramenta → enviar resultado de volta → IA continua → até texto puro |
| 5.4 | Classe base do Executor | Classe abstracta base, com thread de despacho independente, fila de pedidos, controlo de timeout |
| 5.5 | NetworkExecutor | Pedidos HTTP via executor. Timeout, fila |
| 5.6 | CommandLineExecutor | Execução de shell via executor. Detecção de separadores multiplataforma |
| 5.7 | DiskExecutor | Operações de ficheiros via executor. Sem verificação de permissões (Fase 6) |
| 5.8–5.12 | Ferramentas incorporadas | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**Entregável**: Os Silicon Beings podem chamar ferramentas para executar operações.

**Verificação**: Perguntar "Que dia é hoje" → CalendarTool responde; perguntar "verificar processos" → SystemTool executa; dizer ao being para enviar mensagem a outro being → ChatTool funciona.

---

## ~~Fase 6: Segue as Regras (Sistema de Permissões)~~ ✅ Concluída

**Objectivo**: Os Silicon Beings não podem aceder a recursos sensíveis sem autorização.

| # | Módulo | Descrição |
|---|--------|-------------|
| 6.1 | PermissionManager | Instância privada por being. Baseado em callback, resultado de três estados (Allowed/Deny/AskUser). Prioridade de consulta: HighDeny → HighAllow → Callback. Flag IsCurator |
| 6.2 | Enumeração PermissionType | NetworkAccess, CommandLine, FileAccess, Function, DataAccess |
| 6.3 | DefaultPermissionCallback | Lista branca/negra de rede, classificação CLI, regras de segurança de caminhos de ficheiros |
| 6.4 | GlobalACL | Tabela de regras de correspondência por prefixo, persistida no armazenamento |
| 6.5 | UserFrequencyCache | Listas HighAllow/HighDeny. Escolha do utilizador (não detecção automática). Correspondência por prefixo, apenas em memória, expiração configurável |
| 6.6 | Mecanismo UserAsk (consola) | Quando retorna AskUser, a consola solicita y/n |
| 6.7 | Integração de permissões nos executores | Todos os executores verificam permissões antes de executar |
| 6.8 | Nota de isolamento do IStorage | IStorage é persistência interna do sistema — acesso directo a ficheiros, **não** encaminhado via executores, **não** controlável pela IA. Os executores gerem apenas IO iniciado por ferramentas de IA |
| 6.9 | Registo de auditoria | Registar todas as decisões de permissões, com timestamp, requerente, recurso, resultado |

**Entregável**: Avisos de permissão quando os beings tentam operações sensíveis.

**Verificação**: Dizer ao being para apagar um ficheiro → a consola mostra aviso de permissão → digitar `n` → operação negada. Dizer ao being para aceder a um site na lista branca → permitido imediatamente.

---

## ~~Fase 7: Pode Evoluir (Compilação Dinâmica)~~ ✅ Concluída

**Objectivo**: Os Silicon Beings podem reescrever o seu próprio código.

| # | Módulo | Descrição |
|---|--------|-------------|
| 7.1 | CodeEncryption | Encriptação/desencriptação AES-256. Chave derivada do GUID via PBKDF2 |
| 7.2 | DynamicCompilationExecutor | Sandbox de compilação em memória baseada em Roslyn. Controlo de referências de assembly na compilação (defesa primária: excluir System.IO, Reflection, etc.) |
| 7.3 | Scanner de segurança | Análise estática em tempo de execução de padrões de código perigoso (defesa secundária). Bloquear carregamento se a análise falhar |
| 7.4 | Melhoria do ciclo de vida do being | Carregamento: desencriptar → analisar → compilar → instanciar. Tempo de execução: compilar em memória → substituição atómica → persistir encriptado |
| 7.5 | SiliconCurator | Classe abstracta base do Curator. IsCurator=true. Permissões mais elevadas |
| 7.6 | DefaultCurator | Implementação padrão do Curator, com Ficheiro da Alma incorporado e ferramentas de gestão |
| 7.7 | CuratorTool | Ferramenta `[SiliconManagerOnly]`: list_beings, create_being, get_code, reset |
| 7.8 | Sobrescrever callback de permissões | Os beings podem compilar callbacks de permissões personalizados |
| 7.9 | Melhoria do SiliconBeingManager | Método Replace (troca de instância em tempo de execução). MigrateState (transferir estado entre instância antiga e nova) |

**Entregável**: Os Silicon Beings podem gerar novo código via IA, compilar e substituir-se.

**Verificação**: Dizer ao being "adicione uma nova funcionalidade a si mesmo" → observar compilação → reiniciar → nova funcionalidade funciona.

---

## ~~Fase 8: Memória e Planeamento~~ ✅ Concluída

**Objectivo**: Memória de longo prazo, gestão de tarefas, activação temporizada.

| # | Módulo | Descrição |
|---|--------|-------------|
| 8.1 | FileSystemMemory | Armazenamento segmentado curto/longo prazo. Decaimento temporal. Compressão (fundir memórias similares). Pesquisa multidimensional |
| 8.2 | TaskSystem | Tarefas únicas + dependências DAG. Escalonamento por prioridade. Rastreamento de estado |
| 8.3 | TimerSystem | Alarmes únicos + temporizadores periódicos. Precisão de milissegundos. Persistido no armazenamento |
| 8.4 | IncompleteDate | Estrutura de intervalo de datas difuso (por exemplo "Abril de 2026", "Primavera de 2026") |
| 8.5–8.7 | Ferramentas de memória/tarefa/temporizador | Ferramentas para os beings consultarem memória, gerirem tarefas, definirem alarmes |

**Entregável**: Os beings podem lembrar pontos-chave, criar/rastrear tarefas, definir alarmes.

**Verificação**: Criar tarefa → verificar lista de tarefas → definir alarme de 1 minuto → receber notificação quando o tempo expirar.

---

## ~~Fase 9: Framework Completo~~ ✅ Concluída

**Objectivo**: Ponto de entrada unificado, colaboração multi-being.

| # | Módulo | Descrição |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | Host unificado usando padrão builder. Encerramento elegante (Ctrl+C / SIGTERM) |
| 9.2 | Refactorização do Program.Main | Migrar para o padrão CoreHostBuilder |
| 9.3 | Melhoria do SiliconBeingManager | Resposta prioritária do Curator. Isolamento de excepções. Persistência periódica |
| 9.4 | Carregamento de múltiplos beings | Carregar múltiplos beings do directório de dados. Comunicação entre beings via ChatTool |
| 9.5 | Monitorização de desempenho | Rastreamento do tempo de execução de cada objecto Tick |
| 9.6 | ServiceLocator | Localizador de serviços global, com métodos Register/Get |

**Entregável**: Múltiplos beings em execução simultânea, colaborando, geridos pelo CoreHost.

**Verificação**: Criar dois beings → A envia mensagem a B → B recebe e responde → escalonamento do framework sem erros. O Curator responde com prioridade quando uma mensagem do utilizador chega.

---

## ~~Fase 10: Ir para a Web~~ ✅ Concluída

**Objectivo**: Migrar da consola para a interface do navegador.

| # | Módulo | Descrição |
|---|--------|-------------|
| 10.1 | Router | Encaminhador de pedidos HTTP. Rotas de parâmetros de série e serviço de ficheiros estáticos |
| 10.2 | Classe base Controller | Contexto de pedido/resposta. Suporte a respostas HTML e JSON |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | Construtores do lado do servidor em C#. Zero dependência de frameworks frontend |
| 10.6 | SSE (Server-Sent Events) | Actualizações em tempo real push para chat, estado dos beings e eventos do sistema. Mais simples que WebSocket, com reconexão automática do cliente |
| 10.7 | WebUIProvider | Canal de IM em tempo real baseado em SSE. Substitui a consola como interface principal |
| 10.8 | Segurança Web | Lista negra/branca de IPs. Atributo `[WebCode]`. Actualização dinâmica |
| 10.9–10.17 | Controladores Web | Chat, dashboard, beings, tarefas, permissões, pedidos de permissão, executores, registos, configuração, memória, temporizadores, inicialização, sobre, navegador de código, conhecimento, projecto, auditoria |

**Entregável**: Web UI completa acessível a partir do navegador.

**Verificação**: Abrir navegador → conversar com beings → ver dashboard → gerir permissões → todas as funcionalidades operacionais.

---

## ~~Fase 10.5: Melhorias Incrementais~~ ✅ Concluída

**Objectivo**: Reforçar o sistema existente com novas funcionalidades descobertas durante o desenvolvimento.

| # | Módulo | Descrição |
|---|--------|-------------|
| 10.5.1 | BroadcastChannel | Novo tipo de sessão para anúncios em todo o sistema. ID de canal fixo, subscrição dinâmica, filtragem de mensagens pendentes |
| 10.5.2 | Melhoria do ChatMessage | Campos ToolCallId, ToolCallsJson, Thinking para contexto de IA; PromptTokens, CompletionTokens, TotalTokens para rastreamento de tokens; tipo de mensagem SystemNotification |
| 10.5.3 | TokenUsageAuditManager | Rastreamento do consumo de tokens por pedido em todos os beings. Estatísticas agregadas, consultas de séries temporais, armazenamento persistido |
| 10.5.4 | TokenAuditTool | Ferramenta `[SiliconManagerOnly]`, para o Curator consultar e resumir a utilização de tokens |
| 10.5.5 | ConfigTool | Ferramenta `[SiliconManagerOnly]`, para o Curator ler e modificar a configuração do sistema |
| 10.5.6 | AuditController | Dashboard Web para auditoria de utilização de tokens, com gráficos de tendência e exportação de dados |
| 10.5.7 | Expansão do sistema de calendário | 32 implementações de calendário, cobrindo os sistemas de calendário do mundo (Budista, Lunar Chinês, Islâmico, Hebraico, Japonês, Persa, Maia, etc.) |
| 10.5.8 | Melhoria do DiskTool | Novas operações: count_lines, read_lines, clear_file, replace_lines, replace_text, replace_text_all, list_drives |
| 10.5.9 | Melhoria do SystemTool | Novas operações: find_process (suporte a wildcards), resource_usage |
| 10.5.10 | Melhoria do CalendarTool | Novas operações: diff, list_calendars, get_components, get_now_components, convert (conversão entre calendários) |
| 10.5.11 | DashScopeClient | Cliente de IA Alibaba Cloud DashScope, compatível com API OpenAI. Suporta streaming, chamadas de ferramentas, conteúdo de raciocínio |
| 10.5.12 | DashScopeClientFactory | Fábrica para criar clientes DashScope. Descoberta dinâmica de modelos via API. Suporte a múltiplas regiões (Pequim, Virgínia, Singapura, Hong Kong, Frankfurt) |
| 10.5.13 | Sistema de configuração de clientes de IA | Configuração de clientes de IA por being. Opções dinâmicas de chaves de configuração (modelos, regiões). Nomes de exibição localizados |
| 10.5.14 | Expansão da localização | Localização em chinês simplificado, chinês tradicional, inglês e japonês para opções de configuração DashScope, nomes de modelos e nomes de regiões |

**Entregável**: Ferramentas melhoradas, observabilidade, cobertura de calendários e suporte a múltiplos backends de IA.

**Verificação**: O Curator consulta a utilização de tokens via TokenAuditTool → dashboard de auditoria mostra tendências → CalendarTool converte datas entre 32 sistemas de calendário → trocar backend de IA para DashScope → conversar com modelo Qwen via API na nuvem.

---

## ~~Fase 10.6: Refinamento e Optimização~~ ✅ Concluída

**Objectivo**: Refinar as funcionalidades do sistema, adicionar novas características, optimizar a experiência do utilizador.

| # | Módulo | Descrição |
|---|--------|-------------|
| 10.6.1 | WebViewBrowserTool | Ferramenta de automação de navegador multiplataforma baseada em Playwright, suporta modo headless, isolamento individual, suporte completo a JS/CSS |
| 10.6.2 | HelpTool | Ferramenta do sistema de documentação de ajuda, suporta consulta e exibição de documentação multilingue |
| 10.6.3 | ProjectWorkNoteTool | Ferramenta de notas de trabalho do projecto, suporta registos e gestão de trabalho na dimensão do projecto |
| 10.6.4 | ProjectTaskTool | Ferramenta de gestão de tarefas do projecto, suporta atribuição de tarefas e rastreamento de progresso |
| 10.6.5 | KnowledgeTool | Ferramenta de rede de conhecimento, suporta CRUD de conhecimento em triplas e descoberta de caminhos |
| 10.6.6 | ChatHistoryController | Controlador de visualização do histórico de chat, suporta lista de sessões e detalhes de mensagens |
| 10.6.7 | CodeHoverController | Controlador de dicas flutuantes de código, suporta destaque de sintaxe e dicas de código |
| 10.6.8 | WorkNoteController | Controlador de gestão de notas de trabalho, suporta pesquisa e geração de directório |
| 10.6.9 | TimerExecutionHistory | Funcionalidade de histórico de execução de temporizadores, regista e visualiza o histórico de activações |
| 10.6.10 | Expansão da localização | Adicionar suporte de localização em checo (cs-CZ), totalizando 21 variantes linguísticas |
| 10.6.11 | Optimização da Web UI | Suporte a carregamento de ficheiros, indicadores de carregamento, optimização da renderização de chamadas de ferramentas, correcção de modal de notas de trabalho |
| 10.6.12 | Melhoria da gestão de memória | Filtragem avançada, estatísticas, vista de detalhes, optimização do algoritmo de compressão |
| 10.6.13 | Refactorização do sistema de registos | Separação de registos do sistema/Silicon Beings, API de leitura de registos, filtro por Silicon Being |
| 10.6.14 | Melhoria do sistema de permissões | Pré-validação de compilação de callback de permissões, validação de referências de assembly, lista branca do serviço meteorológico wttr.in |

**Entregável**: Automação completa do navegador WebView, sistema de documentação de ajuda, espaço de trabalho de projecto, rede de conhecimento, visualização do histórico de chat e outras melhorias.

**Verificação**: Os Silicon Beings podem operar o navegador via WebViewBrowserTool → obter documentação de ajuda via HelpTool → gerir notas de trabalho e tarefas do projecto → consultar a rede de conhecimento → visualizar o histórico de chat.

---

## ~~Fase 10.7: Colaboração em Projecto e Fluxos de Trabalho~~ ✅ Concluída

**Objectivo**: Adicionar espaço de trabalho de projecto, motor de fluxos de trabalho, mecanismo de desvanecimento da memória e sistema de permissões de ferramentas.

| # | Módulo | Descrição |
|---|--------|-------------|
| 10.7.1 | Gestão de funções do projecto | ProjectTool adiciona operações assign_role, remove_role, list_roles |
| 10.7.2 | Motor de fluxos de trabalho | Motor principal WorkflowEngine, suporta definição de modelos, transições de estado, execução orientada por Tick |
| 10.7.3 | Modelos de fluxo de trabalho | Classe base WorkflowTemplate, define conjuntos de estados e regras de transição |
| 10.7.4 | Instâncias de fluxo de trabalho | Gestão de instâncias WorkflowInstance, vinculada a projectos específicos, rastreia estado actual |
| 10.7.5 | Registos de fluxo de trabalho | WorkflowLog regista o histórico de transições de estado |
| 10.7.6 | Mecanismo de desvanecimento da memória | Serviço de decaimento temporizado MemoryFadeService, aplica automaticamente decaimento de importância e arquivamento à memória a cada hora |
| 10.7.7 | Sistema de permissões de ferramentas | Permissões de ferramentas em dois níveis (nível do Silicon Being + nível do projecto), modelos de permissões, controlo granular por operação |
| 10.7.8 | ToolPermissionController | Controlador Web de gestão de permissões de ferramentas |
| 10.7.9 | ProjectWorkTool | Ferramenta de operações de trabalho do projecto ([SiliconManagerOnly], [ToolScenario(Project)]) |
| 10.7.10 | Sistema de cenários de ferramentas | ToolScenarioAttribute e ChatOnlyAttribute, suporta filtragem de cenários Chat/Task/Timer/MemoryCompression/Project |
| 10.7.11 | Expansão da localização | Adicionar localização em russo, português, italiano, holandês, polaco, sueco, totalizando 34 variantes linguísticas |

**Entregável**: Sistema completo de colaboração em projecto, motor de fluxos de trabalho, mecanismo de desvanecimento da memória e gestão de permissões de ferramentas.

**Verificação**: Criar projecto → atribuir funções → vincular modelo de fluxo de trabalho → beings colaboram no espaço do projecto → memória desvanece e arquiva automaticamente → isolamento de permissões de ferramentas activo.

---

## Fase 11: Integração com Mensageiros Externos

**Objectivo**: Ligar a plataformas de mensagens externas para maior acessibilidade dos utilizadores.

| # | Módulo | Descrição |
|---|--------|-------------|
| 11.1 | FeishuProvider | Integração com robô Feishu (Lark), suporta cartões |
| 11.2 | WhatsAppProvider | Integração com WhatsApp Business API |
| 11.3 | TelegramProvider | Integração com Telegram Bot API, suporta teclado inline |
| 11.4 | Melhoria do IMManager | Encaminhamento multi-fornecedor, formato de mensagem unificado, processamento de pedidos de permissão multiplataforma |

**Entregável**: Os utilizadores podem interagir com os Silicon Beings através de plataformas de IM externas.

---

## Fase 11.5: Sistema de Competências e Integração MCP

**Objectivo**: Camada de abstracção de capacidades reutilizáveis e integração com o ecossistema de ferramentas externas.

| # | Módulo | Descrição |
|---|--------|-------------|
| 11.5.1 | ~~Sistema de Competências~~ ✅ Concluído | Camada de abstracção reutilizável de orquestração de ferramentas + modelo de prompt (SkillManager, modos de duplo acionamento, recarregamento a quente, arquivo de versões, preenchimento automático de metadados por IA) |
| 11.5.2 | ~~Integração MCP~~ ✅ Concluído | Integração de ferramentas de servidores MCP externos (transporte duplo stdio/http, injecção de nomenclatura `mcp_{serverId}_{toolName}`, página de gestão Web, integração com matriz de permissões) |

**Entregável**: Página de gestão de competências (/skill), página de gestão MCP (/mcp), ferramentas incorporadas `skill` e `mcp`, documentação de ajuda de competências/MCP.

---

## Fase 12: Funcionalidades Avançadas

**Objectivo**: Funcionalidades avançadas opcionais para melhorar as capacidades.

| # | Módulo | Descrição |
|---|--------|-------------|
| 12.1 | ~~Rede de conhecimento~~ ✅ Concluída | Grafo de conhecimento baseado em estrutura de triplas (sujeito-predicado-objecto), suporta CRUD, descoberta de caminhos, consultas avançadas e travessia do grafo |
| 12.2 | ~~Sistema de plugins~~ ✅ Concluída | Carregamento de plugins externos, com verificação de segurança e sandbox (interface IPlugin, PluginLoader, isolamento AssemblyLoadContext) |
| 12.3 | Ecossistema de competências | Mercado de competências reutilizáveis para capacidades dos beings |
