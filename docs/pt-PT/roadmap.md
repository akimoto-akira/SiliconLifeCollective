# Roteiro

> **Versão: v0.2.0-alpha**

[English](../en/roadmap.md) | [Deutsch](../de-DE/roadmap.md) | [Français](../fr-FR/roadmap.md) | [中文](../zh-CN/roadmap.md) | [繁體中文](../zh-HK/roadmap.md) | [Español](../es-ES/roadmap.md) | [日本語](../ja-JP/roadmap.md) | [한국어](../ko-KR/roadmap.md) | [Čeština](../cs-CZ/roadmap.md) | [Русский](../ru-RU/roadmap.md) | [Italiano](../it-IT/roadmap.md) | [Polski](../pl-PL/roadmap.md) | **Português**

## Roteiro de dupla versão

### SiliconLife.Default (Versão padrão)
- **Posicionamento**: Implementação padrão, principalmente para verificação de viabilidade arquitetural
- **Estado atual**: Fases 1-10.6 concluídas, o sistema funciona de forma estável
- **Descrição do papel**: Implementação de referência para verificação arquitetural, garante a correção e viabilidade do design arquitetural principal

### SiliconLife.Fast (Versão de alto desempenho)
- **Posicionamento**: Versão principal de produção
- **Estado atual**: Migração base da arquitetura concluída, motor de armazenamento SpeedyPack e sistema de plugins implementados
- **Descrição do papel**: Com base na arquitetura verificada na versão Default, realiza uma otimização aprofundada de desempenho e reforça as funcionalidades de produção, a melhor escolha para implementação real

**Plano de desenvolvimento da versão Fast**:
- ✅ Fase 1: Migração da estrutura do projeto base e sistema de configuração
- ✅ Fase 2: Migração da interface Web e controladores
- ✅ Fase 3: Otimização do sistema de armazenamento (armazenamento em memória SpeedyPack + persistência assíncrona)
- ✅ Fase 3.5: Ferramenta de gestão SpeedyPack (aplicação Windows Forms SiliconLife.Speedy.Manager)
- 📋 Fase 5: Otimização de desempenho (pool de ligações, pool de objetos, concorrência sem lock)
- 📋 Fase 6: Substituição do servidor Web Kestrel
- 📋 Fase 7: Serialização binária MessagePack

---

## Princípios orientadores

Cada fase termina com um sistema **funcional e observável**. Nenhuma fase produz "muita infraestrutura sem nada visível".

---

## ~~Fase 1: Pode conversar~~ ✅ Concluída

**Objetivo**: Entrada de consola → Chamada IA → Saída de consola. Unidade mínima verificável.

| # | Módulo | Descrição |
|---|--------|-------------|
| 1.1 | Estrutura da solução e projeto | Criar `SiliconLifeCollective.sln`, com `src/SiliconLife.Core/` (biblioteca principal) e `src/SiliconLife.Default/` (implementação padrão + ponto de entrada) |
| 1.2 | Configuração (mínima) | Singleton + desserialização JSON. Lê `config.json`. Gera automaticamente os valores predefinidos se ausente |
| 1.3 | Localização (mínima) | Classe abstrata `LocalizationBase`, implementação `ZhCN`. Adicionar `Language` à configuração |
| 1.4 | OllamaClient (mínimo) | Interface `IAIClient`, chamada HTTP ao Ollama local `/api/chat`. Ainda sem streaming, sem chamadas de ferramenta |
| 1.5 | I/O de consola | `while(true) + Console.ReadLine()`, ler entrada → chamar IA → imprimir resposta |
| 1.6 | Cabeçalho de copyright | Adicionar cabeçalho Apache 2.0 a todos os ficheiros fonte C# |

**Entregável**: Programa de chat de consola para conversar com o modelo Ollama local.

**Verificação**: Executar o programa, escrever "olá", ver a resposta da IA.

---

## ~~Fase 2: Tem um esqueleto~~ ✅ Concluída

**Objetivo**: Substituir o "ciclo nu" por uma estrutura de framework. Comportamento inalterado.

| # | Módulo | Descrição |
|---|--------|-------------|
| 2.1 | Armazenamento (mínimo) | Interface `IStorage` (Read/Write/Exists/Delete, pares chave-valor). Implementação `FileSystemStorage`. Classe de instância (não estática). Acesso direto ao sistema de ficheiros —— **A IA não pode controlar IStorage** |
| 2.2 | Loop principal + Objeto de relógio | Loop infinito, intervalo de relógio preciso (`Stopwatch` + `Thread.Sleep`). Agendamento por prioridade |
| 2.3 | Padronização IAIClient | Interface `IAIClientFactory`. OllamaClient refatorizado para a interface padrão |
| 2.4 | Migração de consola | Migrar `while(true)` para um objeto de relógio orientado pelo loop principal. Comportamento idêntico à Fase 1 |

**Entregável**: O loop principal executa o relógio, o chat de consola ainda funciona.

**Verificação**: Registar um objeto de relógio de teste, conta os ticks a cada segundo; o chat de consola ainda funciona.

---

## ~~Fase 3: Tem uma alma~~ ✅ Concluída

**Objetivo**: O primeiro Silicon Being vive no framework.

| # | Módulo | Descrição |
|---|--------|-------------|
| 3.1 | SiliconBeingBase | Classe base abstrata com Id, Name, ToolManager, AIClient, ChatService, Storage, PermissionService. `Tick()` e `ExecuteOneRound()` abstratos |
| 3.2 | Carregamento do ficheiro da alma | `SoulFileManager`: Lê `soul.md` do diretório de dados do Being |
| 3.3 | ContextManager (mínimo) | Concatena ficheiro da alma + mensagens recentes → chama IA → obtém resposta. Ainda sem chamadas de ferramenta, sem persistência |
| 3.4 | ISiliconBeingFactory | Interface factory para criar instâncias de Being |
| 3.5 | SiliconBeingManager (mínimo) | Herda do objeto de relógio (Prioridade=0). Itera todos os Beings, chama o seu Tick sequencialmente |
| 3.6 | DefaultSiliconBeing | Implementação do comportamento padrão. Verifica mensagens não lidas → cria ContextManager → ExecuteOneRound → saída |
| 3.7 | Estrutura do diretório do Being | `DataDirectory/SiliconManager/{GUID}/`, contém `soul.md` e `state.json` |

**Entregável**: Silicon Being orientado pelo loop principal, recebe entrada de consola, carrega o ficheiro da alma, chama a IA.

**Verificação**: Entrada de consola → Tick do relógio do loop principal ativado → o Being processa (com comportamento orientado pelo ficheiro da alma) → resposta da IA. O estilo de resposta deve diferir da Fase 1.

---

## ~~Fase 4: Tem memória~~ ✅ Concluída

**Objetivo**: As conversas persistem após o reinício.

| # | Módulo | Descrição |
|---|--------|-------------|
| 4.1 | ChatSystem | Conceito de canal (dois GUIDs = um canal). Modelo de mensagem com persistência. Ainda sem chat de grupo |
| 4.2 | IIMProvider + IMManager | Interface `IIMProvider`. `ConsoleProvider` como canal IM formal. `IMManager` encaminha as mensagens |
| 4.3 | Extensão do ContextManager | Obtém o histórico do sistema de chat. Persiste as respostas da IA. Suporta a continuação de chamadas de ferramenta multinível |
| 4.4 | Modelo IMessage | Modelo de mensagem unificado partilhado entre o sistema de chat e o gestor IM |

**Entregável**: Sistema de chat com armazenamento persistente.

**Verificação**: Conversar vários turnos → Sair → Reiniciar → Perguntar "De que é que falámos?" → O Being consegue responder.

---

## ~~Fase 5: Pode agir (Sistema de ferramentas)~~ ✅ Concluída

**Objetivo**: Os Silicon Beings podem executar ações, não apenas conversar.

| # | Módulo | Descrição |
|---|--------|-------------|
| 5.1 | ITool + ToolResult | Interface `ITool` com Name, Description, Execute. `ToolResult` com Success, Message, Data |
| 5.2 | ToolManager | Instância por Being. Descoberta de ferramentas baseada em reflexão. Suporte ao atributo `[SiliconManagerOnly]` |
| 5.3 | IAIClient: Suporte a chamadas de ferramenta | Analisa os tool_calls da IA. Ciclo: executar ferramentas → retornar resultados → IA continua → até texto simples |
| 5.4 | Classe base Executor | Classe base abstrata com o seu próprio thread dispatcher, fila de pedidos, controlo de timeout |
| 5.5 | NetworkExecutor | Pedidos HTTP através do executor. Timeout, fila |
| 5.6 | CommandLineExecutor | Execução shell através do executor. Deteção de separadores multiplataforma |
| 5.7 | DiskExecutor | Operações de ficheiros através do executor. Ainda sem verificação de permissões (Fase 6) |
| 5.8–5.12 | Ferramentas integradas | CalendarTool, SystemTool, NetworkTool, ChatTool, DiskTool |

**Entregável**: Os Silicon Beings podem chamar ferramentas para executar ações.

**Verificação**: Perguntar "Que dia é hoje" → CalendarTool responde; Perguntar "Verifica os processos" → SystemTool executa; Pedir ao Being para enviar uma mensagem a outro Being → ChatTool funciona.

---

## ~~Fase 6: Segue as regras (Sistema de permissões)~~ ✅ Concluída

**Objetivo**: Os Silicon Beings não podem aceder a recursos sensíveis sem autorização.

| # | Módulo | Descrição |
|---|--------|-------------|
| 6.1 | PermissionManager | Instância privada por Being. Baseado em callback, resultado ternário (Allowed/Deny/AskUser). Prioridade do pedido: HighDeny → HighAllow → Callback. Flag IsCurator |
| 6.2 | Enum PermissionType | NetworkAccess, CommandLine, FileAccess, Function, DataAccess |
| 6.3 | DefaultPermissionCallback | Whitelist/blacklist de rede, classificação CLI, regras de segurança de caminho de ficheiro |
| 6.4 | GlobalACL | Tabela de regras por correspondência de prefixo, persistida no armazenamento |
| 6.5 | UserFrequencyCache | Listas HighAllow/HighDeny. Escolha do utilizador (sem deteção automática). Correspondência de prefixo, apenas em memória, expiração configurável |
| 6.6 | Mecanismo UserAsk (Consola) | Em caso de retorno AskUser, prompt de consola s/n |
| 6.7 | Integração de permissões do executor | Todos os executores verificam a permissão antes da execução |
| 6.8 | Nota de isolamento IStorage | IStorage é a persistência interna do sistema —— acesso direto a ficheiros, **não** encaminhado através do executor, **não** controlável pela IA. Os executores gerem apenas I/O iniciado pelas ferramentas IA |
| 6.9 | Registo de auditoria | Regista todas as decisões de permissões com timestamp, requerente, recurso, resultado |

**Entregável**: Prompt de permissão quando o Being tenta uma operação sensível.

**Verificação**: Pedir ao Being para eliminar um ficheiro → A consola mostra um prompt de permissão → Digitar `n` → Operação recusada. Pedir ao Being para visitar um site na whitelist → Imediatamente autorizado.

---

## ~~Fase 7: Pode evoluir (Compilação dinâmica)~~ ✅ Concluída

**Objetivo**: Os Silicon Beings podem reescrever o seu próprio código.

| # | Módulo | Descrição |
|---|--------|-------------|
| 7.1 | CodeEncryption | Encriptação/desencriptação AES-256. Chave PBKDF2 derivada do GUID |
| 7.2 | DynamicCompilationExecutor | Sandbox de compilação em memória baseado em Roslyn. Controlo das referências de assembly na compilação (defesa principal: excluir System.IO, Reflection, etc.) |
| 7.3 | Análise de segurança | Análise estática em tempo de execução de padrões de código perigosos (defesa secundária). Bloqueia o carregamento se a análise falhar |
| 7.4 | Extensão do ciclo de vida do Being | Carregamento: Desencriptar → Analisar → Compilar → Instanciar. Execução: Compilar em memória → Substituição atómica → Persistir encriptado |
| 7.5 | SiliconCurator | Classe base abstrata do Curator. IsCurator=true. Permissão mais elevada |
| 7.6 | DefaultCurator | Implementação padrão do Curator com ficheiro da alma integrado e ferramentas de administração |
| 7.7 | CuratorTool | Ferramentas `[SiliconManagerOnly]`: list_beings, create_being, get_code, reset |
| 7.8 | Override do callback de permissões | Os Beings podem compilar callbacks de permissões personalizados |
| 7.9 | Extensão do SiliconBeingManager | Método Replace (troca de instância em tempo de execução). MigrateState (transferência de estado entre instância antiga e nova) |

**Entregável**: Os Silicon Beings podem compilar e substituir-se com novo código gerado pela IA.

**Verificação**: Pedir ao Being "Adiciona-te uma nova funcionalidade" → Observar a compilação → Reinício → A nova funcionalidade funciona.

---

## ~~Fase 8: Memória e planeamento~~ ✅ Concluída

**Objetivo**: Armazenamento de longo prazo, gestão de tarefas, triggers de temporizador.

| # | Módulo | Descrição |
|---|--------|-------------|
| 8.1 | FileSystemMemory | Armazenamento segmentado curto prazo/longo prazo. Expiração temporal. Compressão (fusão de memórias semelhantes). Pesquisa multidimensional |
| 8.2 | TaskSystem | Tarefas únicas + dependências DAG. Agendamento por prioridade. Monitorização de estado |
| 8.3 | TimerSystem | Alarme único + temporizadores periódicos. Precisão ao milissegundo. Persistido no armazenamento |
| 8.4 | IncompleteDate | Estrutura de intervalo de datas aproximadas (ex. "abril 2026", "primavera 2026") |
| 8.5–8.7 | Ferramentas de memória/tarefas/temporizadores | Ferramentas para os Beings consultarem memórias, gerirem tarefas, definirem temporizadores |

**Entregável**: Os Beings podem lembrar pontos-chave, criar/monitorizar tarefas, definir alarmes.

**Verificação**: Criar uma tarefa → Verificar a lista de tarefas → Definir um alarme de um minuto → Receber notificação ao soar.

---

## ~~Fase 9: Framework concluído~~ ✅ Concluída

**Objetivo**: Ponto de entrada unificado, colaboração multi-Being.

| # | Módulo | Descrição |
|---|--------|-------------|
| 9.1 | CoreHost + CoreHostBuilder | Host unificado com padrão Builder. Encerramento graceful (Ctrl+C / SIGTERM) |
| 9.2 | Refatoração de Program.Main | Migração para o padrão CoreHostBuilder |
| 9.3 | Extensão do SiliconBeingManager | Resposta Curator-first. Isolamento de exceções. Persistência regular |
| 9.4 | Carregamento multi-Being | Carrega múltiplos Beings do diretório de dados. Comunicação Being-a-Being através do ChatTool |
| 9.5 | Monitorização de desempenho | Acompanhamento do tempo de execução por objeto de relógio |
| 9.6 | ServiceLocator | Localizador de serviços global com métodos Register/Get |

**Entregável**: Múltiplos Beings funcionam simultaneamente, colaboram, geridos pelo CoreHost.

**Verificação**: Criar dois Beings → A envia uma mensagem a B → B recebe e responde → Agendamento do framework sem erros. O Curator responde primeiro às mensagens do utilizador.

---

## ~~Fase 10: Rumo à Web~~ ✅ Concluída

**Objetivo**: Migrar da consola para a interface do browser.

| # | Módulo | Descrição |
|---|--------|-------------|
| 10.1 | Router | Router de pedidos HTTP. Encaminhamento por parâmetros sequenciais e serviço de ficheiros estáticos |
| 10.2 | Classe base Controller | Contexto de pedido/resposta. Suporte para respostas HTML e JSON |
| 10.3–10.5 | HtmlBuilder / CssBuilder / JsBuilder | Builders do lado do servidor em C#. Zero dependência de framework frontend |
| 10.6 | SSE (Server-Sent Events) | Atualizações em tempo real push para chat, estado dos Beings e eventos de sistema. Mais simples que WebSocket, com reconexão automática do cliente |
| 10.7 | WebUIProvider | Canal IM em tempo real baseado em SSE. Substitui a consola como interface principal |
| 10.8 | Segurança Web | Blacklist/whitelist de IP. Atributo `[WebCode]`. Atualizações dinâmicas |
| 10.9–10.17 | Controladores Web | Chat, Dashboard, Beings, Tarefas, Permissões, Pedidos de permissão, Executors, Logs, Configuração, Memória, Temporizadores, Inicialização, Informações, Browser de código, Conhecimentos, Projetos, Auditoria |

**Entregável**: Interface Web completa, acessível a partir do browser.

**Verificação**: Abrir o browser → Conversar com um Being → Ver o dashboard → Gerir permissões → Tudo funciona.

---

## ~~Fase 10.5: Extensões incrementais~~ ✅ Concluída

**Objetivo**: Estender o sistema existente com novas funcionalidades descobertas durante o desenvolvimento.

| # | Módulo | Descrição |
|---|--------|-------------|
| 10.5.1 | BroadcastChannel | Novo tipo de sessão para anúncios de sistema. ID do canal fixo, subscrição dinâmica, filtragem de mensagens pendentes |
| 10.5.2 | Extensão do ChatMessage | Campos ToolCallId, ToolCallsJson, Thinking para contexto IA; PromptTokens, CompletionTokens, TotalTokens para monitorização de tokens; tipo de mensagem SystemNotification |
| 10.5.3 | TokenUsageAuditManager | Monitorização do consumo de tokens por todos os Beings por pedido. Estatísticas agregadas, consultas de séries temporais, armazenamento persistente |
| 10.5.4 | TokenAuditTool | Ferramenta `[SiliconManagerOnly]` para o Curator consultar e resumir a utilização de tokens |
| 10.5.5 | ConfigTool | Ferramenta `[SiliconManagerOnly]` para o Curator ler e modificar a configuração do sistema |
| 10.5.6 | AuditController | Dashboard Web de auditoria da utilização de tokens com gráficos de tendência e exportação de dados |
| 10.5.7 | Extensão do sistema de calendário | 32 implementações de calendários, cobrindo os sistemas calendariais mundiais (Budista, Lunar chinês, Islâmico, Hebraico, Japonês, Persa, Maia, etc.) |
| 10.5.8 | Extensão do DiskTool | Novas operações: count_lines, read_lines, clear_file, replace_lines, replace_text, replace_text_all, list_drives |
| 10.5.9 | Extensão do SystemTool | Novas operações: find_process (com suporte a carateres universais), resource_usage |
| 10.5.10 | Extensão do CalendarTool | Novas operações: diff, list_calendars, get_components, get_now_components, convert (conversão inter-calendários) |
| 10.5.11 | DashScopeClient | Cliente IA Alibaba Cloud DashScope, compatível com API OpenAI. Suporta streaming, chamadas de ferramenta, conteúdo de raciocínio |
| 10.5.12 | DashScopeClientFactory | Factory para criar clientes DashScope. Descoberta dinâmica de modelos via API. Suporte multi-região (Pequim, Virgínia, Singapura, Hong Kong, Frankfurt) |
| 10.5.13 | Sistema de configuração do cliente IA | Configuração do cliente IA por Being. Opções de chaves de configuração dinâmicas (modelo, região). Nomes de exibição localizados |
| 10.5.14 | Extensão da localização | Localização em chinês simplificado, chinês tradicional, inglês e japonês para as opções de configuração DashScope, nomes de modelos e nomes de regiões |

**Entregável**: Ferramentas estendidas, observabilidade, cobertura de calendários e suporte multi-backend IA.

**Verificação**: O Curator consulta a utilização de tokens através do TokenAuditTool → O dashboard de auditoria mostra as tendências → O CalendarTool converte a data entre 32 sistemas de calendário → Mudar o backend IA para DashScope → Conversar com o modelo Qwen através da API cloud.

---

## ~~Fase 10.6: Refinamento e otimização~~ ✅ Concluída

**Objetivo**: Refinar as funcionalidades do sistema, adicionar novas funcionalidades, otimizar a experiência do utilizador.

| # | Módulo | Descrição |
|---|--------|-------------|
| 10.6.1 | WebViewBrowserTool | Ferramenta de automação de browser multiplataforma baseada em Playwright, com modo headless, isolamento individual, suporte JS/CSS completo |
| 10.6.2 | HelpTool | Ferramenta do sistema de documentação de ajuda, suporta a consulta e visualização de documentação multilingue |
| 10.6.3 | ProjectWorkNoteTool | Ferramenta de notas de trabalho de projeto, suporta o registo de trabalho associado ao projeto e gestão |
| 10.6.4 | ProjectTaskTool | Ferramenta de gestão de tarefas de projeto, suporta a atribuição de tarefas, monitorização do progresso |
| 10.6.5 | KnowledgeTool | Ferramenta de rede de conhecimentos, suporta CRUD de conhecimentos em triplas e pesquisa de caminhos |
| 10.6.6 | ChatHistoryController | Controlador de visualização do histórico de chat, suporta a lista de sessões e detalhes das mensagens |
| 10.6.7 | CodeHoverController | Controlador de sugestões ao passar o rato sobre o código, suporta coloração sintática e completagem de código |
| 10.6.8 | WorkNoteController | Controlador de gestão de notas de trabalho, suporta pesquisa e geração de diretório |
| 10.6.9 | TimerExecutionHistory | Funcionalidade de histórico de execução de temporizadores, regista o histórico de ticks e permite a consulta |
| 10.6.10 | Extensão da localização | Adicionado suporte de localização checa (cs-CZ), total de 24 variantes linguísticas |
| 10.6.11 | Otimização da interface Web | Suporte a upload de ficheiros, indicador de carregamento, otimização da renderização de chamadas de ferramenta, correção do modal de notas de trabalho |
| 10.6.12 | Extensão da gestão de memória | Filtragem avançada, estatísticas, vista detalhada, otimização do algoritmo de compressão |
| 10.6.13 | Refatoração do sistema de logs | Separação de logs sistema/Silicon Being, API de leitura de logs, filtro por Being |
| 10.6.14 | Extensão do sistema de permissões | Pré-validação do callback de permissões, validação de referências de assembly, whitelist do serviço meteorológico wttr.in |

**Entregável**: Automação completa do browser WebView, sistema de documentação de ajuda, espaço de projeto, rede de conhecimentos, visualização do histórico de chat e outras funcionalidades avançadas.
