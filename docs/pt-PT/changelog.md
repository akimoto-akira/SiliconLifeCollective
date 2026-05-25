# Registo de alterações

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [Français](../fr-FR/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md) | [Русский](../ru-RU/changelog.md) | [Italiano](../it-IT/changelog.md) | [Polski](../pl-PL/changelog.md) | **Português**

Todas as alterações importantes deste projeto serão documentadas neste ficheiro.

O formato é baseado em [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
e este projeto adere à [Gestão Semântica de Versões](https://semver.org/spec/v2.0.0.html).

---

## Informações sobre este registo de alterações

### Dupla versão do projeto

Este projeto oferece duas versões de implementação:

- **SiliconLife.Default**: Implementação padrão, principalmente para verificação de viabilidade arquitetural. Aplicação de consola, armazenamento JSON em sistema de ficheiros.
- **SiliconLife.Fast**: Versão principal de produção. Aplicação desktop multiplataforma (Windows / macOS / Linux), armazenamento em memória SpeedyPack + persistência assíncrona, otimização aprofundada de desempenho.

Ambas as versões partilham as mesmas interfaces e funcionalidades, diferindo apenas na implementação do armazenamento e no modo de execução. SiliconLife.Default serve como referência para verificação arquitetural, SiliconLife.Fast é a versão principal recomendada para produção.

### Origem do projeto

- Este projeto começou em 20 de março de 2026.
- Antes deste projeto, uma demo de verificação falhou devido a um design arquitetural inadequado, tornando impossível a integração com múltiplas plataformas IA.

### Ferramentas AI IDE utilizadas

#### Kiro (Amazon AWS)
- O projeto foi inicialmente mantido pelo Kiro, iniciado no modo Spec.
- Kiro é um ambiente de desenvolvimento IA agentic construído pela Amazon AWS.
- Baseado no Code OSS (VS Code), suporta as definições do VS Code e plugins compatíveis com Open VSX.
- Fluxo de desenvolvimento orientado por especificações para codificação IA estruturada.

#### Comate AI IDE / 文心快码 (Baidu)
- Utilizado ocasionalmente para escrita e documentação.
- Comate AI IDE é uma ferramenta de ambiente de desenvolvimento nativo IA publicada pelo Baidu Wenxin em 23 de junho de 2025.
- Primeiro IDE IA multimodal e multi-agente colaborativo do setor.
- Funcionalidades incluem conversão design-código e codificação assistida por IA em todo o fluxo.
- Alimentado pelo modelo Baidu Wenxin 4.0 X1 Turbo.

#### Trae (ByteDance)
- Utilizado de outubro de 2025 a abril de 2026.
- IDE IA com geração inteligente de código e gestão de projeto.

#### Qoder (Alibaba)
- Utilizado para manutenção do projeto desde 18 de abril de 2026.
- Plataforma de codificação IA que suporta análise de código, geração de documentação e colaboração multi-agente.

#### CatPaw (Meituan)
- Utilizado em combinação com Qoder desde 6 de maio de 2026.
- Baseado nos modelos LongCat desenvolvidos internamente pela Meituan, com poderosas capacidades de refatoração completa da arquitetura do código.

### Documentação de requisitos

- A documentação de requisitos deste projeto não é pública.
- Os requisitos foram validados iterativamente por mais de 12 plataformas IA internacionais e grandes séries de modelos, produzindo mais de 2000 linhas de documentação de requisitos orientada por user stories, quase incompreensível para humanos.

---

## [Não publicado]

### 2026-05-22

#### Correções de consistência da documentação
- `9e07b27` - Corrigir discrepâncias da documentação francesa (fr-FR) com o código fonte (ref task-307)
  - 10 ficheiros modificados

- `9e3be72` - Corrigir discrepâncias da documentação alemã (de-DE) com o código fonte (ref task-308)
  - 5 ficheiros modificados

- `2bc7151` - Corrigir discrepâncias da documentação espanhola (es-ES) com o código fonte (ref task-309)
  - 13 ficheiros modificados

- `f95088e` - Corrigir discrepâncias da documentação italiana (it-IT) com o código fonte (ref task-310)
  - 11 ficheiros modificados

- `6ea9f4a` - Corrigir discrepâncias da documentação polaca (pl-PL) com o código fonte (ref task-311)
  - 16 ficheiros modificados

- `7646923` - Corrigir discrepâncias da documentação portuguesa (pt-PT) com o código fonte (ref task-312)
  - 12 ficheiros modificados

- `7eaf9db` - Corrigir discrepâncias da documentação checa (cs-CZ) com o código fonte (ref task-313)
  - 12 ficheiros modificados

#### Framework de colaboração
- `3cb7347` - Atualização task-313 relatedCommit=7eaf9db
  - 1 ficheiros modificados

### 2026-05-21

#### Novas funcionalidades
- `99eca78` - Adicionar 'Ver armazenamento (só de leitura)' ao menu de contexto, chamada intra-processo ao Speedy.Manager (ref task-301)
  - 26 ficheiros modificados

#### Correções de consistência da documentação
- `7f65cf1` - Corrigir discrepâncias da documentação zh-CN com o código fonte (ref task-303)
  - 15 ficheiros modificados

- `a9e2a2c` - Corrigir discrepâncias da documentação inglesa (en) com o código fonte (ref task-302)
  - 9 ficheiros modificados

- `2549105` - Corrigir discrepâncias da documentação chinês tradicional (zh-HK) com o código fonte (ref task-304)
  - 12 ficheiros modificados

- `277eb50` - Corrigir discrepâncias da documentação japonesa com o código fonte (ref task-305)
  - 10 ficheiros modificados

- `edce413` - Corrigir discrepâncias da documentação coreana (ko-KR) com o código fonte (ref task-306)
  - 18 ficheiros modificados

- `f2adcae` - Corrigir inconsistências da documentação portuguesa com o código fonte (ref task-220)
  - 15 ficheiros modificados

- `3332987` - Corrigir inconsistências da documentação chinês tradicional (Hong Kong) com o código fonte (ref task-218)
  - 14 ficheiros modificados

- `af9f715` - Corrigir inconsistências da documentação polaca com o código fonte (ref task-217)
  - 15 ficheiros modificados

- `2e2b18b` - Corrigir inconsistências da documentação coreana com o código fonte (ref task-216)
  - 16 ficheiros modificados

- `626ebc9` - Corrigir inconsistências da documentação japonesa com o código fonte (ref task-215)
  - 19 ficheiros modificados

- `48d061b` - Corrigir inconsistências da documentação italiana com o código fonte (ref task-214)
  - 14 ficheiros modificados

#### Framework de colaboração
- `6683bee` - Registar equipa Marvis AI, atualizar estado das tarefas
  - 3 ficheiros modificados

- `03fc905` - Arquivar task-210~220
  - 5 ficheiros modificados

### 2026-05-20

#### Novas funcionalidades
- `65176d4` - Adicionar suporte completo de localização portuguesa (pt-PT + pt-BR) (ref task-208)
  - 41 ficheiros modificados

#### Correções de consistência da documentação
- `af4dffd` - Corrigir todas as inconsistências da documentação zh-CN com o código fonte (ref task-209)
  - 11 ficheiros modificados

- `144b945` - Corrigir inconsistências da documentação inglesa (en) e checa (cs-CZ) com o código fonte (ref task-219, task-210)
  - 22 ficheiros modificados

- `08bec55` - Corrigir inconsistências da documentação alemã (de-DE) com o código fonte (ref task-211)
  - 14 ficheiros modificados

- `7ff28de` - Corrigir inconsistências da documentação espanhola (es-ES) com o código fonte (ref task-212)
  - 14 ficheiros modificados

- `15e2133` - Corrigir inconsistências da documentação francesa (fr-FR) com o código fonte (ref task-213)
  - 13 ficheiros modificados

#### Correções de bugs
- `7dac388` - Corrigir lista de tarefas do projeto não visível (ref task-207)
  - 6 ficheiros modificados

#### Framework de colaboração
- `7890223` - Arquivar task-201~209, publicar tarefas de correção de consistência de documentação task-210~220
  - 5 ficheiros modificados

### 2026-05-19

#### Novas funcionalidades
- `cd72846` - Implementar alternativa segura para o contorno da verificação de segurança do PluginLoader (ref task-203)
  - 13 ficheiros modificados

- `fc0c00c` - Melhorias Speedy.Manager - Criar/Importar/Exportar/Hierarquia TreeView/Janela de progresso (ref task-206)
  - 9 ficheiros modificados

#### Correções de bugs
- `ec07118` - Corrigir problema de ITypeRegistry/IObjectFactory não registados antes do carregamento de plugins (ref task-205)
  - 8 ficheiros modificados

- `9e749db` - Corrigir erro Creator ID is required ao criar projeto (ref task-204)
  - 4 ficheiros modificados

#### Infraestrutura
- `43dc092` - Migração CLDR - adicionar CldrDataProvider, remover .github
  - 1 ficheiros modificados

- `c09ec1f` - Adicionar cldr/ ao .gitignore
  - 1 ficheiros modificados

- `221f818` - Sincronização GitHub alterada para esquema de espelho push Gitee, workflow mantido apenas como backup manual
  - 1 ficheiros modificados

- `08cdf1a` - Corrigir workflow de sincronização GitHub - adicionar lógica de retry e salto sem alterações
  - 1 ficheiros modificados

- `fb4e77d` - Atualizar SiliconLife.Speedy.Manager.csproj
  - 1 ficheiros modificados

#### Framework de colaboração
- `df90af0` - Atualização task-203 relatedCommit=cd72846
  - 1 ficheiros modificados

### 2026-05-18

#### Refatoração
- `e720d06` - Refatorar completamente Speedy.Manager de WinForms para Avalonia (ref task-202)
  - 17 ficheiros modificados

#### Correções de bugs
- `08894a9` - Corrigir erro de exibição do nível das entradas de resumo da timeline de memória (ref task-201)
  - 3 ficheiros modificados

#### Framework de colaboração
- `2871afb` - Arquivar todas as tarefas, limpar tasks.json
  - 2 ficheiros modificados

### 2026-05-17

#### Novas funcionalidades
- `d6eb994` - Adicionar entrada de criação de projeto e seleção de modelo de workflow à página de lista de projetos (ref task-203)
  - 14 ficheiros modificados

- `0872134` - Orquestração conduzida pelo curador ThinkOnProject para projetos sem modelo (ref task-202)
  - 6 ficheiros modificados

- `cb3188e` - Visualização de @menções no chat de grupo (ref task-208)
  - 4 ficheiros modificados

- `f9968e5` - Declaração de capacidade ToolCall do cliente IA e degradação elegante (ref task-205)
  - 4 ficheiros modificados

- `0d2b843` - Lógica de decisão do chat de grupo ShouldReplyInGroupChat (ref task-201)
  - 6 ficheiros modificados

- `277a2b1` - Complementação da rede de conhecimento - consultas avançadas e travessia de grafos (ref task-207)
  - 9 ficheiros modificados

#### Correções de bugs
- `6d0b66e` - Corrigir TypeError de appendMessage ao enviar mensagens no chat de grupo (ref task-209)
  - 5 ficheiros modificados

- `b15167c` - Submissão adicional do registo de rota list-workflow-templates omitido no task-203 (ref task-203)
  - 1 ficheiros modificados

- `dc549a2` - Corrigir workflow de sincronização Gitee - adicionar nome de utilizador ao URL do token
  - 1 ficheiros modificados

#### Infraestrutura
- `e5fa3ad` - Desativar sincronização automática GitHub schedule, aguardando solução oficial Gitee
  - 1 ficheiros modificados

#### Framework de colaboração
- `4a58c82` - Adicionar relatório de análise de capacidades do sistema + proposta de design ThinkOnProject
  - 5 ficheiros modificados

- `8ab29e6` - Arquivar relatório de análise de completude de capacidades do sistema em .ai-collab/docs
  - 2 ficheiros modificados

- `b412d9c` - Arquivar tarefas antigas, republicar task-201~208 com base em análise abrangente
  - 2 ficheiros modificados

- `437884a` - Atualizar metadados de colaboração - task-202/203/204 concluídas (ref task-202, task-203, task-204)
  - 2 ficheiros modificados

- `bf78d79` - Atualizar metadados de colaboração - task-201/205/208 concluídas
  - 2 ficheiros modificados

- `de6ee0e` - Registo de fim de sessão catpaw-20260517-2215
  - 5 ficheiros modificados

- `7223b6f` - Registo de fim de sessão catpaw-20260517-2200
  - 4 ficheiros modificados


## [Alpha-0.2] - 2026-05-16

### 2026-05-16

#### Preparação do lançamento
- `476d839` - Adicionadas tarefas de lançamento alpha-0.2
  - Criadas task-114 (redação CHANGELOG) e task-115 (atualização do número de versão)
  - 1 ficheiro modificado

### 2026-05-15

#### Infraestrutura
- `672627b` - Adicionado workflow de sincronização Gitee (com configuração de permissões)
  - Atualizadas permissões do workflow sync-from-gitee.yml
  - 1 ficheiro modificado, 7 inserções(+), 4 eliminações(-)

- `3cd5256` - Adicionada sincronização automática Gitee via GitHub Actions
  - Adicionado workflow sync-from-gitee.yml
  - 1 ficheiro modificado, 50 inserções(+)

#### Atualizações da documentação
- `aa1d2ad` - Atualizados README/arquitetura/início-rápido em todas as 11 línguas, refletindo o suporte multiplataforma do SiliconLife.Fast (ref task-112, task-113)
  - Corrigida a documentação que descrevia SiliconLife.Fast como apenas Windows, refletindo o suporte multiplataforma real (Windows / macOS / Linux)
  - Atualizados README.md, architecture.md, getting-started.md em 11 línguas
  - Adicionada propriedade hint ao SelectComponent
  - ConfigView agora passa o parâmetro hint aos menus dropdown de enumeração
  - Adicionada chave SelectSearchHint às localizações de 11 línguas
  - 53 ficheiros modificados, 690 inserções(+), 194 eliminações(-)

#### Sistema de tarefas
- `3329f3d` - Adicionado mecanismo de inspeção do sistema de tarefas + tarefas de correção de bugs de localização
  - Criada task-113: corrigir problema de localização na página de informações
  - Atualizada task-112: atualizar documentação da versão Fast para suporte Linux
  - Arquivadas tarefas concluídas (11) em .ai-collab/archive/
  - Mecanismo de inspeção configurado: inspeção rápida (cada 30 min) + inspeção completa (diária 06:00)
  - 2 ficheiros modificados, 148 inserções(+), 171 eliminações(-)

#### Framework de colaboração
- `6038e22` - coze-agent registado no registo .ai-collab
  - Adicionadas informações de registo AI residente da plataforma Coze
  - 1 ficheiro modificado

### 2026-05-14

#### Framework de colaboração AI
- `7344fbb` - Modo handoff removido, transição para abordagem baseada em lista de tarefas (v2.0)
  - Reestruturado diretório .ai-collab do modo handoff para abordagem baseada em lista de tarefas
  - Adicionado ficheiro central da lista de tarefas tasks.json
  - Adicionado registo de operações activity.log
  - Adicionados diretórios changes/ e sessions/

- `589a48e` - Adicionados registos de sessão .ai-collab
  - Adicionados registos de estado da sessão de colaboração AI

- `5481bcf` - Qoder AI IDE registado no registo de colaboração
  - Adicionadas informações de registo do assistente de codificação Qoder AI

- `e2d7b61` - relatedCommit e changes commitHash de tasks.json concluídos
  - Associações de metadados das tarefas concluídas

- `a087f0c` - Todas as tarefas task-101~110 aceites
  - Confirmado que todas as 10 correções de tarefas estão concluídas

#### Correção de bugs
- `fac9435` - Concluídas todas as correções e implementações de task-101~110
  - Corrigido texto de sugestão em falta no componente de seleção de pesquisa
  - Corrigidos problemas de localização na página de informações
  - Corrigido erro JS de pesquisa no sistema de ajuda
  - 39 ficheiros modificados, 684 inserções(+), 121 eliminações(-)

- `c46dfbc` - Concluídas todas as tarefas pendentes (task-001~006)
  - Concluídas 6 tarefas pendentes iniciais

- `ec176b2` - Lista de tarefas sobrescrita - revisão de código encontrou 10 novos bugs
  - Criadas task-101~110 (10 novas tarefas)

#### Refatoração
- `ab15915` - Cabeçalhos de copyright unificados + HelpController BOM e HelpView pesquisa JS corrigidos
  - Cabeçalhos de copyright Apache 2.0 unificados em todos os ficheiros fonte C#
  - Corrigido problema de codificação BOM do HelpController
  - Corrigido erro JavaScript de pesquisa do HelpView

#### Novas funcionalidades
- `18a6f5d` - Servidor de capacidades browser MCP criado (ref task-111)
  - Adicionado projeto SiliconLife.McpServer
  - Implementado servidor MCP de automação de browser Playwright

- `9eb251a` - Módulo SiliconLife.McpServer removido (ref task-111)
  - Servidor MCP autónomo removido, funcionalidade integrada no projeto principal

### 2026-05-13

#### Localização
- `7a62590` - Adicionado suporte de localização polaca
  - Adicionada implementação de localização polaca pl-PL (PlPL.cs, 1089 linhas)
  - Adicionada localização de documentação de ajuda polaca (HelpLocalizationPlPL.cs, 3972 linhas)
  - Adicionado suporte de calendário histórico chinês polaco (ChineseHistoricalPlPL.cs, 600 linhas)
  - Adicionada localização de bandeja polaca (TrayPlPL.cs, 135 linhas)
  - Adicionado conjunto completo de documentação polaca (15 documentos)
  - Enumeração Language estendida com polaco
  - 35 ficheiros modificados, 14379 inserções(+), 11 eliminações(-)

- `51f9c8e` - Atualizadas referências Ark AI e melhorias terminológicas na documentação
  - Terminologia do cliente IA atualizada na documentação multilingue

- `7587c12` - Adicionadas entradas do registo de alterações para todas as línguas
  - Atualizações do registo de alterações sincronizadas em todas as versões linguísticas

#### Migração do sistema de janelas
- `b49a07d` - Migração para o modo residente de janela Avalonia
  - Removida dependência Windows Forms, migração completa para o framework Avalonia UI
  - Janela de estado exibida corretamente no Linux (verificada via ambiente de trabalho remoto)
  - Adicionados controlos de janela: menu de contexto, duplo clique para abrir a Web, botão de fecho
  - Adicionado framework de colaboração multi-AI (.ai-collab/)
  - Corrigida inicialização do ícone da bandeja (degradação elegante)
  - Adicionados App.axaml e App.cs como pontos de entrada da aplicação Avalonia
  - 13 ficheiros modificados, 1442 inserções(+), 541 eliminações(-)

- `d335aaf` - Janela sempre visível na plataforma Linux + diálogo de confirmação de fecho
  - Linux mostra automaticamente a janela de estado (sem ícone na bandeja)
  - Linux mostra diálogo de confirmação ao fechar a janela
  - Windows/macOS mantêm o comportamento original da bandeja
  - Suportado parâmetro --no-tray para desativar forçadamente a bandeja
  - Adicionado método ShowMessageBoxAsync para diálogos de confirmação
  - 3 ficheiros modificados, 206 inserções(+), 29 eliminações(-)

#### Refatoração do sistema de bandeja
- `841d384` - Sistema de bandeja refatorado e framework de colaboração AI inicializado
  - TrayLocalizationBase simplificado, propriedades não utilizadas removidas
  - Adicionado item de localização ShowStatus
  - App.cs: clique no ícone da bandeja mostra janela de estado, adicionados itens de menu localizados
  - Program.cs: inicialização do ícone da bandeja movida para StartAsync
  - TrayStatusWindow esconde-se em vez de fechar ao fechar
  - Registados trae-glm5 e catpaw no framework .ai-collab
  - Atualizado .gitignore para garantir que todos os ficheiros .ai-collab são rastreados
  - 22 ficheiros modificados, 178 inserções(+), 1226 eliminações(-)

#### Documentação
- `43653bc` - Descrição do repositório e registo AI atualizados
  - README do projeto e informações de registo .ai-collab atualizados

### 2026-05-12

#### Vistas Web do sistema de tarefas
- `0891b3c` - Adicionar vistas de detalhe e histórico de execução de tarefas
  - Adicionado TaskExecutionDetailView vista de detalhe de execução de tarefas
  - Adicionado TaskExecutionHistoryView vista de histórico de execução de tarefas
  - TaskController adicionadas interfaces de consulta de detalhe e histórico de execução
  - Adicionado TaskViewModel modelo de vista de tarefas
  - TaskCenter centro de tarefas melhorado
  - TaskSystem sistema de tarefas atualizado
  - 9 línguas de localização adicionadas chaves relacionadas com tarefas
  - 26 ficheiros modificados, 803 inserções(+), 55 eliminações(-)

### 2026-05-11

#### Refatoração da arquitetura de componentes Web
- `5e687ad` - Migrar renderização de componentes de string para H-tree
  - Método de renderização ComponentBase migrado de padrão string para estrutura H-tree
  - Todos os 28 componentes adaptados à nova arquitetura de renderização (A, Accordion, Button, Calendar, Card, Chart, etc.)
  - Refatoração significativa do SelectComponent (889 linhas melhoradas)
  - Controladores e vistas atualizados em conformidade
  - 33 ficheiros modificados, 667 inserções(+), 435 eliminações(-)

- `bfd332d` - Migrar Style de string para estilos inline CssBuilder
  - Adicionado construtor de estilos CssBuilder
  - Sistema de estilos ComponentBase migrado de string para CssBuilder estruturado
  - LoadingComponent significativamente melhorado (103 linhas adicionadas)
  - Migração de estilos dos controladores ConfigController, LogController, MemoryController
  - Migração de estilos das vistas ChatView, ConfigView, LogView, MemoryView
  - 37 ficheiros modificados, 351 inserções(+), 157 eliminações(-)

#### Otimização do sistema de armazenamento
- `d67a7ee` - Otimizar QueryLatest para grandes conjuntos de dados
  - Otimização de desempenho do método QueryLatest do SpeedyTimeStorage
  - Fornecedor de registo SpeedyLoggerProvider melhorado
  - 2 ficheiros modificados, 44 inserções(+), 5 eliminações(-)

#### Refatoração do sistema de calendário
- `9629f88` - Extrair TimerExecution e melhorar vistas web do temporizador
  - TimerSystem extraiu lógica TimerExecution (175 linhas removidas)
  - SelectComponent significativamente melhorado (427 linhas melhoradas)
  - TimerController e vistas do temporizador melhorados
  - ContextManager gestor de contexto atualizado
  - 12 ficheiros modificados, 458 inserções(+), 267 eliminações(-)

#### Localização
- `5d8ca79` - Adicionar chave de localização LogsLoading
  - 9 línguas adicionaram chave LogsLoading
  - Classe base DefaultLocalizationBase adicionou definição
  - 11 ficheiros modificados, 15 inserções(+)

### 2026-05-10

#### Refatoração do sistema de tarefas
- `54394f6` - Fundir sistema de tarefas com ciclos de histórico de chat
  - ProjectTaskSystem sistema de tarefas do projeto significativamente simplificado (411 linhas refatoradas)
  - TaskSystem sistema de tarefas simplificado (254 linhas refatoradas)
  - TaskCenter centro de tarefas refatorado (188 linhas melhoradas)
  - ContextManager gestor de contexto otimizado (347 linhas refatoradas)
  - DefaultSiliconBeing ser de silício melhorado
  - TimerSystem sistema de temporizador integrado com tarefas
  - Interface IWorkNoteStorage atualizada
  - SpeedyWorkNoteStorage e FileSystemWorkNoteStorage adaptados
  - 16 ficheiros modificados, 648 inserções(+), 897 eliminações(-)

### 2026-05-09

#### Melhoria da interface Web
- `bc50dd7` - Melhorar vista de chat e adicionar funcionalidade de auditoria
  - Adicionado AuditController controlador de auditoria (261 linhas)
  - Adicionado AuditView vista de auditoria (379 linhas)
  - Adicionado AuditViewModel modelo de vista de auditoria
  - ChatView vista de chat significativamente melhorada (171 linhas melhoradas)
  - ChatController controlador de chat atualizado
  - Componente MarkdownEditorComponent melhorado
  - InitController controlador de inicialização melhorado
  - ChatSystem sistema de chat adicionou funcionalidades
  - 14 ficheiros modificados, 1030 inserções(+), 112 eliminações(-)

- `c9babce` - Melhorar renderização de chamadas de ferramentas na vista de chat
  - Renderização de blocos de chamadas de ferramentas do ChatView melhorada
  - 1 ficheiro modificado, 54 inserções(+), 11 eliminações(-)

#### Sistema de cenários de ferramentas IA
- `ff2eddd` - Implementar sistema de filtragem de cenários de ferramentas
  - Adicionado ToolScenarioAttribute atributo de cenário de ferramenta (36 linhas)
  - Adicionado ChatOnlyAttribute atributo de cenário apenas chat (19 linhas)
  - ToolManager gestor de ferramentas adicionou filtragem de cenários (40 linhas)
  - ContextManager gestor de contexto adaptado para filtragem de cenários
  - 4 ficheiros modificados, 115 inserções(+), 30 eliminações(-)

- `5709a33` - Adicionar atributos de cenário a classes de ferramentas
  - 24 classes de ferramentas adicionaram anotações de atributo ToolScenario
  - Incluindo calendário, chat, configuração, curator, base de dados, disco, compilação dinâmica, etc.
  - 24 ficheiros modificados, 46 inserções(+), 20 eliminações(-)

#### Refatoração do sistema de tarefas
- `2f19a5f` - Reestruturar sistema de tarefas com TaskCenter e TaskEnumerator
  - Adicionado TaskCenter centro de tarefas (235 linhas)
  - Adicionado TaskEnumerator enumerador de tarefas (297 linhas)
  - TaskSystem sistema de tarefas refatorado e simplificado
  - DefaultSiliconBeing ser de silício adaptado à nova arquitetura
  - DefaultSiliconBeingFactory fábrica atualizada
  - SiliconBeingBase classe base melhorada
  - 7 ficheiros modificados, 796 inserções(+), 275 eliminações(-)

#### Migração do sistema de permissões
- `a06ed09` - Migrar sistema de IM e permissões para o projeto App
  - PermissionRequestQueue migrado de Default/Fast para o projeto App (443 linhas adicionadas)
  - Removido WebUIProvider da versão Default (403 linhas eliminadas)
  - Removido HelpTool da versão Default (194 linhas eliminadas)
  - Removido PermissionRequestQueue duplicado de Default/Fast
  - Removido IMPermissionAskHandler da versão Default
  - PermissionRequestController controlador atualizado
  - 14 ficheiros modificados, 496 inserções(+), 1183 eliminações(-)

#### Otimização do contexto IA
- `4c8aaff` - Otimizar gestor de contexto e melhorar localizador de serviços
  - ContextManager gestor de contexto simplificado e otimizado
  - ServiceLocator localizador de serviços melhorado (36 linhas adicionadas)
  - ToolManager gestor de ferramentas melhorado (34 linhas adicionadas)
  - Clientes DashScopeClient e VolcengineArkClient melhorados
  - Executores (CommandLine, Disk, Network) atualizados
  - 8 ficheiros modificados, 116 inserções(+), 98 eliminações(-)

#### Localização
- `5c5eef7` - Adicionar chaves de localização de auditoria e tarefas
  - DefaultLocalizationBase adicionou 127 linhas de definições de localização
  - 9 línguas adicionaram chaves relacionadas com auditoria e tarefas (26 linhas cada)
  - 11 ficheiros modificados, 387 inserções(+)

#### Configuração do projeto
- `2067db6` - Atualizar configurações do projeto e regras gitignore
  - Regras .gitignore atualizadas
  - DefaultConfigData e Fast DefaultConfigData configuração melhorada
  - SpeedyWorkNoteStorage armazenamento melhorado
  - Núcleo SpeedyPack melhorado
  - 5 ficheiros modificados, 32 inserções(+), 6 eliminações(-)


### 2026-05-06

#### Refatoração de módulos em larga escala
- `eeb3be6` - Refatoração e reorganização de módulos em larga escala
  - SiliconLife.App project restructuring
  - SiliconLife.Fast project reorganization
  - SiliconLife.Default project reorganization
  - SiliconLife.Common shared modules reorganization
  - SiliconLife.Core core modules reorganization
  - SiliconLife.Speedy storage engine reorganization
  - SiliconLife.Speedy.Manager management tools reorganization
  - 119 ficheiros modificados, 6926 linhas adicionadas, 3066 linhas eliminadas

### 2026-05-04

#### Cliente IA
- `24d2c86` - Adicionado VolcengineArkClient e substituído Audit por rastreamento Usage
  - New VolcengineArkClient Volcengine Ark AI client
  - Supports streaming and non-streaming modes
  - Built-in dual rate limiting (client-side + server-side)
  - Compatible with OpenAI API protocol
  - Replaced Audit system with Usage tracking
  - 24 ficheiros modificados, 802 linhas adicionadas, 21 linhas eliminadas

#### Sistema de ferramentas
- `f27650a` - Adicionada ferramenta de hot reload para reinicialização automática do Fast
  - New HotReloadTool hot reload tool
  - Supports online compilation, update, and restart of SiliconLife.Fast
  - New standalone HotReload.exe updater program
  - Safe file copying mechanism (does not overwrite itself)
  - Graceful shutdown and port release waiting
  - 9 ficheiros modificados, 581 linhas adicionadas

#### Localização
- `6a5aad8` - Atualizados todos os ficheiros e adicionado suporte de localização francesa
  - New fr-FR French localization
  - Updated all language versions
  - French help documentation translation
  - French interface translation
  - 100+ ficheiros modificados

### 2026-05-03

#### Infraestrutura do projeto
- `2664b0c` - Atualizada infraestrutura do projeto e dependências
  - SiliconLife.Speedy.Manager added WPF management interface (MainForm.Designer.cs, MainForm.resx)
  - Added slc.ico icon resource (1.5MB)
  - PluginLoader significantly enhanced security scanning (622 linhas adicionadas)
  - Added PermissionedStreamFactory permission stream factory (779 lines)
  - Added PermissionRequestQueue permission request queue (Default and Fast versions)
  - Added DebugLoggerProvider debug logger provider
  - ConfigDataBase configuration base class enhanced
  - ToolManager added plugin tool scanning (ScanAllPluginAssemblies)
  - SiliconBeingManager lifecycle management enhanced
  - DashScopeClient Alibaba Cloud AI client significantly enhanced (227 linhas adicionadas)
  - DefaultSiliconBeingFactory factory enhanced
  - Web views and controllers updated (ChatView, WorkNoteView, PermissionRequestController)
  - 9-language localization added new keys
  - 35 ficheiros modificados, 28080 linhas adicionadas, 336 linhas eliminadas

### 2026-05-02

#### Cliente IA Enhancement
- `c16f99f` - Atualizados cliente IA, UI Web e componentes de armazenamento
  - DashScopeClient Alibaba Cloud client significantly improved
  - SpeedyPackAutoCompactor auto-compactor optimized
  - Web view base class and BeingView improved
  - 6 ficheiros modificados, 240 linhas adicionadas, 81 linhas eliminadas

#### Sistema de plugins
- `242dc98` - Adicionada lista de plugins na página de informações
  - AboutController added plugin information display
  - AboutViewModel added plugin data model
  - AboutView added plugin list rendering
  - 9-language localization added plugin-related keys
  - 14 ficheiros modificados, 160 linhas adicionadas, 1 linha eliminada

#### Otimização IA
- `147f8f4` - Simplificado texto de prompt de memória de contexto
  - ContextManager optimized AI prompts
  - 1 ficheiro modificado, 1 linha adicionada, 1 linha eliminada

#### Otimização do armazenamento Speedy
- `8bda2d3` - Atualizada implementação do armazenamento Speedy e controlador de memória
  - SpeedyPackAutoCompactor interval correction
  - SpeedyTimeStorage path handling optimization
  - MemoryController memory controller improvements
  - SpeedyPack.Manager UI update
  - 4 ficheiros modificados, 21 linhas adicionadas, 18 linhas eliminadas

#### Melhoria da bandeja
- `8972654` - Enhanced tray status window localization support
  - 9-language tray localization added Speedy management entry
  - TrayStatusWindow added Speedy management menu item
  - 11 ficheiros modificados, 72 linhas adicionadas

#### Otimização do Speedy.Manager
- `6f5db09` - Otimizada UI do SpeedyPack Manager e componentes internos
  - MainForm interface refactoring
  - FreeList memory management optimization
  - WriteQueue write queue improvements
  - SpeedyPack core optimization
  - 5 ficheiros modificados, 96 linhas adicionadas, 88 linhas eliminadas

#### Melhoria do sistema de armazenamento
- `57f9d5d` - Melhorado sistema de armazenamento, adicionada auto-compactação e suporte de data incompleta
  - Added SpeedyPackAutoCompactor auto-compaction timer (30-minute interval)
  - SpeedyPackRegistry singleton manager enhanced
  - SpeedyStorage, SpeedyTimeStorage, SpeedyWorkNoteStorage adapter improvements
  - SpeedyPack added FreeList free space management (149 lines)
  - PackFileWriter writer refactoring optimization
  - WriteOperation, WriteQueue write queue enhancement
  - SpeedyPackOptions configuration options expansion
  - IncompleteDate added comparison methods
  - PluginLoader plugin loader improvements
  - Default and Fast versions Program.cs initialization flow updated
  - DefaultConfigData configuration data simplified
  - KnowledgeNetwork knowledge network streamlined
  - ChatController, MemoryController controller optimization
  - SpeedyPack.Manager MainForm functionality enhanced
  - 22 ficheiros modificados, 639 linhas adicionadas, 253 linhas eliminadas

#### Atualização do Speedy.Manager
- `b04ed33` - Atualizados ficheiros do Speedy.Manager

### 2026-05-01

#### Refatoração da arquitetura: Armazenamento Speedy substitui LiteDB
- `6600972` - Substituído LiteDB por armazenamento Speedy, adicionado sistema de plugins e projetos Speedy
  - **Novo projeto SiliconLife.Speedy**: Motor de armazenamento .spk de alto desempenho
    - Classe principal SpeedyPack (489 linhas): mapeamento de diretório em memória + cache de entradas + fila de escrita assíncrona
    - Classe de configuração SpeedyPackOptions: TTL cache, máximo de entradas em cache, modo só de leitura
    - Interface de transação IPackTransaction: suporta operações de escrita atómicas
    - Classe de informação de ficheiro SpkFileInfo
    - Diretório Interno: DirectoryMap, EntryCache, PackFileReader, PackFileWriter, WriteQueue, WriteOperation, SpeedyTransaction, SpkHeader, PathNormalizer, FreeList
    - Utiliza MessagePack 3.1.4 para serialização binária (compressão LZ4)
  - **Novo projeto SiliconLife.Speedy.Manager**: Ferramenta de gestão WPF
    - Arquitetura MVVM: MainViewModel, DirectoryTreeViewModel, ContentViewerViewModel, etc.
    - Camada de serviços: PackService, FileDialogService, RecentFilesService, NotificationService
    - Conversores: BoolToVisibility, ByteSizeToString, ContentTypeToIcon, NullToCollapsed
    - Vistas: MainWindow, DirectoryTreeView, ContentViewerPanel, MetadataPanel
    - Diálogos: FileInfoDialog, ImportDialog, NewEntryDialog
  - **Migração de armazenamento SiliconLife.Fast**: LiteDB → SpeedyPack
    - Adicionado SpeedyStorage (adaptador IStorage)
    - Adicionado SpeedyTimeStorage (adaptador ITimeStorage)
    - Adicionado SpeedyWorkNoteStorage (adaptador IWorkNoteStorage)
    - Adicionado SpeedyPackRegistry (gestão singleton ao nível do processo)
    - Adicionado SpeedyPackAutoCompactor (temporizador de auto-compactação)
    - Removidas implementações de armazenamento LiteDB (LiteDBStorage, LiteDBTimeStorage, LiteDBWorkNoteStorage, LiteDBLoggerProvider, LiteDBManager, LiteDBModels)
    - Removido código relacionado com a janela de gestão LiteDB
  - **Sistema de Plugins**:
    - Adicionada interface IPlugin (Core/Plugins/IPlugin.cs)
    - Adicionado carregador de plugins PluginLoader (Core/Plugins/PluginLoader.cs)
    - Suporte ao carregamento de DLLs de plugins a partir de diretório
    - Verificação de segurança: verificação de namespace proibido (System.IO, System.Net, Microsoft.CodeAnalysis, etc.)
    - Lista branca de assemblies fidedignos (Google.Protobuf, Newtonsoft.Json, MessagePack, etc.)
    - Carregamento isolado com AssemblyLoadContext personalizado
    - ToolManager adicionou método ScanAllPluginAssemblies
    - CoreHost integrou carregador de plugins
  - 119 ficheiros modificados, 6926 linhas adicionadas, 3066 linhas eliminadas

#### Melhoria do Ser de Silício
- `3aef4c3` - Adicionado estado de atividade Stopped e melhorias no tratamento de erros
  - Seres de silício agora têm estado Stopped
  - Mecanismo de tratamento e recuperação de erros melhorado

#### Localização Update
- `513c65d` - Atualizadas todas as versões linguísticas e documentação
  - Adicionado componente MarkdownEditorComponent (625 linhas)
  - Adicionado componente DetailsComponent (130 linhas)
  - Adicionado componente acordeão AccordionComponent (285 linhas)
  - Atualizações de controladores BeingController, ChatController, MemoryController, PermissionController
  - Refatoração de vistas BeingView, ChatView, MemoryView, SoulEditorView
  - Removido antigo MarkdownEditorView
  - Migração de componentização do InitController
  - 115 ficheiros modificados, 5761 linhas adicionadas, 2362 linhas eliminadas

### 2026-04-30

#### System Tray Functionality
- `101b203` - Implementada janela de estado da bandeja e ApplicationContext
  - Adicionados recursos de ícone da bandeja (alpha.png, noWord.png, slc.ico, wordIcon.png)
  - Implemented TrayStatusWindow status window
  - Supports tray localization in 9 languages (TrayCsCZ, TrayDeDE, TrayEnUS, etc.)
  - TrayLocalizationBase abstract base class
  - 24 ficheiros modificados, 27995 linhas adicionadas, 1 linha eliminada (including resource files)

#### Componentized UI Architecture
- `e61cfaa` - Concluída arquitetura de UI componentizada, implementados 24 componentes
  - MVP phase (8): ComponentBase, Div, Span, Button, Input, Form, Select, Label
  - Phase 2 (6): Accordion, Card, Tabs, Table, Modal, Message
  - Phase 3 (5): Calendar, Tree, Chart, FileUpload, RichText
  - Added Js, Behavior, DomUpdate and other helper classes
  - 25 ficheiros modificados, 2666 linhas adicionadas

- `7449e51` - Sistema de componentes melhorado e adicionados novos temas de pele
  - Enhanced A, Button, Div, Form, Input and other components
  - Added 3 skin themes: HighContrast, Light, Minimal
  - Updated existing skins (Admin, Chat, Creative, Dev)
  - Migração de componentização do InitController
  - 32 ficheiros modificados, 1466 linhas adicionadas, 1238 linhas eliminadas

- `1ba8636` - Iniciada migração de componentização do InitController (em progresso)
  - 9 ficheiros modificados, 574 linhas adicionadas, 145 linhas eliminadas

#### Storage System Unification
- `895dff9` - Unificados soul.md e state.json para usar interface IStorage
  - DefaultSiliconBeing uses IStorage to read/write soul files and state
  - Added StateFileManager state file manager
  - SoulFileManager refactored to adapt to IStorage
  - 8 ficheiros modificados, 201 linhas adicionadas, 116 linhas eliminadas

#### LiteDB Management Enhancement
- `a34bef4` - Added LiteDBManager and enhanced tray localization
  - Added LiteDB management entry to tray menu
  - Updated tray localization in 9 languages
  - 10 ficheiros modificados, 196 linhas adicionadas

- `c4a79ca` - Adicionada fábrica de localização consciente da língua para janela de gestão LiteDB
  - 1 ficheiro modificado, 78 linhas adicionadas

- `5ebc55e` - LiteDBAdminLocalization convertido para classe base abstrata
  - 10 ficheiros modificados, 1356 linhas adicionadas

#### Configuration System Fix
- `2da5256` - Adicionado método abstrato ConfigExists e corrigidos registos de configuração duplicados do LiteDB
  - ConfigDataBase added ConfigExists method
  - Fast version DefaultConfigData implements LiteDB configuration existence check
  - Fixed LiteDB duplicate configuration key issue
  - 9 ficheiros modificados, 210 linhas adicionadas, 2 linhas eliminadas

#### Chat and View Optimization
- `d3618ec` - Otimizadas sessões de chat, sistema de armazenamento, modelo de tempo e classes base de vistas
  - BroadcastChannel, GroupChatSession, SingleChatSession optimizations
  - ITimeStorage added query methods
  - FileSystemStorage and LiteDBStorage synchronized updates
  - ViewBase refactoring optimization (Default and Fast versions)
  - 11 ficheiros modificados, 622 linhas adicionadas, 392 linhas eliminadas

### 2026-04-29

#### Architecture Refactoring: Shared Module Extraction
- `a102428` - Módulos partilhados migrados de SiliconLife.Default para SiliconLife.Common
  - Extracted 32 calendar implementations to Common project
  - Extracted localization base classes and 21 language implementations to Common project
  - Extracted permission manager and default silicon being implementation to Common project
  - Extracted 23 built-in tool implementations to Common project
  - Extracted Playwright WebView implementation to Common project
  - Updated namespace to SiliconLife.Collective
  - 122 ficheiros modificados, 586 linhas adicionadas, 343 linhas eliminadas

#### Code Quality Improvement
- `17566fe` - Substituído Console.WriteLine por sistema de registo nos projetos Core, Common e Default
  - ContextManager, AuditLogger, DefaultConfigData and 6 other files updated
  - Unified use of ILogger interface, improving code maintainability
  - 6 ficheiros modificados, 12 linhas adicionadas, 8 linhas eliminadas

#### SiliconLife.Fast High-Performance Version
- `54a0307` - Adicionado projeto SiliconLife.Fast e concluídas correções de compilação
  - Complete Windows Forms application entry point
  - System tray support (NotifyIcon)
  - Ported all Web UI controllers (20+)
  - Ported all Web view components
  - Ported 4 skin themes (Admin, Chat, Creative, Dev)
  - 125 ficheiros modificados, 61186 linhas adicionadas

#### Multi-language Documentation Synchronization
- `265fde8` - Documentação de arquitetura de dupla versão sincronizada para todas as línguas
  - Updated architecture.md, changelog.md in 7 languages
  - Updated contributing.md in 6 languages
  - Updated getting-started.md, roadmap.md in 7 languages
  - 47 ficheiros modificados, 1214 linhas adicionadas, 38 linhas eliminadas

#### Sistema de armazenamento LiteDB (Fast Version)
- `4704862` - Adicionadas dependências e infraestrutura LiteDB
  - Added LiteDBManager management class
  - Added LiteDBModels data models
  - 3 ficheiros modificados, 252 linhas adicionadas

- `4220036` - Implementadas classes de armazenamento LiteDB
  - LiteDBStorage: implements IStorage interface
  - LiteDBTimeStorage: implements ITimeStorage interface
  - LiteDBWorkNoteStorage: implements IWorkNoteStorage interface
  - 3 ficheiros modificados, 581 linhas adicionadas

- `38ebd23` - Sistema de configuração e registo migrado para LiteDB
  - DefaultConfigData adapted to LiteDB storage
  - Added LiteDBLoggerProvider logging provider
  - 2 ficheiros modificados, 203 linhas adicionadas, 67 linhas eliminadas

- `e687157` - Rede de conhecimento migrada do sistema de ficheiros para LiteDB
  - KnowledgeNetwork fully refactored, using LiteDB to store triple data
  - 1 ficheiro modificado, 231 linhas adicionadas, 72 linhas eliminadas

- `4220169` - Armazenamento LiteDB integrado no Program e ProjectManager
  - Program.cs initializes LiteDB storage
  - ProjectManager adapted to LiteDB work note storage
  - 2 ficheiros modificados, 40 linhas adicionadas, 17 linhas eliminadas

- `5f3a709` - Removidas implementações de armazenamento de sistema de ficheiros obsoletas
  - Deleted FileSystemLoggerProvider, FileSystemStorage, FileSystemTimeStorage, etc.
  - 6 ficheiros modificados, 1518 linhas eliminadas

- `e1a4ef2` - docs: adicionado identificador de versão v0.1.0-alpha a toda a documentação
  - 127 ficheiros modificados, 2297 linhas adicionadas, 2471 linhas eliminadas

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### Storage System Refactoring
- `8dd26e3` - Interface ITimeStorage unificada para usar IncompleteDate e adicionada API de consulta hierárquica
  - Removed DateTime overload methods from ITimeStorage interface, unified to use IncompleteDate
  - Added CompareTo(DateTime) comparison method and Expand() expansion method to IncompleteDate
  - Added GetEarliestTimestamp(), GetLatestTimestamp() hierarchical query API
  - Added HasSummary() and QueryWithLevel() methods, supporting queries by time level
  - Memory.cs refactored compression algorithm, using new hierarchical query API to improve efficiency
  - FileSystemTimeStorage.cs fully implements new interface methods
  - Synchronized updates to all callers: ChatSystem, ChatSession, BroadcastChannel, AuditLogger, TokenUsageRecord, etc.
  - Tool system updates: HelpTool, LogTool, TokenAuditTool adapted to new interface
  - Web controller updates: AuditController, ChatController, ChatHistoryController adapted to new interface
  - 41 ficheiros modificados, 1820 linhas adicionadas, 903 linhas eliminadas

### 2026-04-27

#### Help Documentation System Enhancement
- `9989d79` - Atualizada localização, sistema de ajuda e vistas web
  - Added IAIClientFactoryHelp.cs AI client factory help documentation interface
  - Completed 9-language translation for all help documents
  - HelpTopics.cs added 40 help topic definitions
  - Web views comprehensively updated: InitController, AuditView, ConfigView, KnowledgeView, LogView, etc.
  - Localization system enhancement: all language versions added new localization keys
  - AI client factory updates: DashScopeClientFactory, OllamaClientFactory improvements
  - 30 ficheiros modificados, 10086 linhas adicionadas, 15 linhas eliminadas

#### Help Documentation New Content
- `e7afe94` - Adicionada documentação de ajuda de ficheiro soul e registo de auditoria
  - Added soul file management help documentation
  - Added audit log help documentation
  - HelpTopics.cs added topic definitions
  - HelpView.cs significantly refactored, improved document rendering logic
  - PermissionView.cs refactored, improved permission management interface
  - Core module enhancement: SiliconBeingManager, TaskSystem, ToolManager improvements
  - TaskTool.cs refactored, improved task management functionality
  - Web views comprehensively updated: all view components synchronized
  - HelpController.cs simplified, optimized controller logic
  - 30 ficheiros modificados, 7100 linhas adicionadas, 897 linhas eliminadas

### 2026-04-26

#### Help Documentation System
- `07895d7` - Sistema de documentação de ajuda melhorado, adicionados 3 documentos e concluída tradução em 9 línguas
  - Added memory system, Ollama installation configuration, Alibaba Cloud DashScope platform usage guide
  - Completed 9-language translation for all 10 help documents
  - Simplified HelpView rendering logic
  - 18 ficheiros modificados, 14418 linhas adicionadas, 1364 linhas eliminadas

#### German Localization
- `0cfd8a1` - Adicionado suporte completo de localização alemã (de-DE)
  - Complete German localization files
  - Added Chinese historical calendar German support
  - Added help documentation German translation
  - Fully synchronized all documents in 9 languages
  - 135 ficheiros modificados, 26186 linhas adicionadas, 14371 linhas eliminadas

#### Documentação Synchronization
- `3aada7d` - Documentação chinês tradicional (zh-HK) sincronizada com chinês simplificado
  - 3 ficheiros modificados, 519 linhas adicionadas, 422 linhas eliminadas
- `2f6abff` - Adicionado nome de exibição da ferramenta de ajuda para todas as línguas
  - 7 ficheiros modificados, 47 linhas adicionadas, 7 linhas eliminadas

#### Knowledge System Refactoring
- `60944fe` - Namespace unificado para SiliconLife.Collective
  - 8 ficheiros modificados, 5 linhas adicionadas, 8 linhas eliminadas
- `69c51c5` - Adicionado sistema de documentação de ajuda e traduzidos comentários de código para inglês
  - 29 ficheiros modificados, 3385 linhas adicionadas, 22 linhas eliminadas

### 2026-04-25

#### Automação do browser WebView
- `41757c3` - Implementada automação de browser WebView multiplataforma baseada em Playwright
  - 6 ficheiros modificados, 1152 linhas adicionadas

#### Documentação Updates
- `0ff797b` - Adicionada documentação KnowledgeTool e WorkNoteTool (7 línguas)
  - 28 ficheiros modificados, 4983 linhas adicionadas
- `ad77415` - Atualizados todos os ficheiros changelog, adicionado histórico Git de 2026-04-25
  - 7 ficheiros modificados, 168 linhas adicionadas

#### Espaço de trabalho do projeto Management
- `785c551` - Implementada gestão de espaço de trabalho de projeto com notas de trabalho e sistema de tarefas
  - Added project workspace management system
  - Work notes functionality for tracking project progress
  - Integração do sistema de gestão de tarefas
  - 29 ficheiros modificados, 4256 linhas adicionadas, 36 linhas eliminadas

#### Localização checa
- `b4bbf39` - Adicionada localização checa completa (cs-CZ) e atualizada toda a documentação linguística
  - 116 ficheiros modificados, 4933 linhas adicionadas, 222 linhas eliminadas
- `faf078f` - Corrigidos erros de compilação da localização checa
  - 3 ficheiros modificados, 910 linhas adicionadas, 1 linha eliminada

#### Knowledge System Enhancement
- `20adaac` - Adicionada KnowledgeTool com suporte completo de localização
  - 34 ficheiros modificados, 2331 linhas adicionadas, 56 linhas eliminadas

### 2026-04-24

#### Melhoria da gestão de memória
- `c7b2ecc` - Gestão de memória melhorada com filtragem avançada, estatísticas e vistas de detalhe
  - Added advanced memory filtering
  - Implemented memory statistics
  - Adicionada página de vista de detalhe de memória
  - Suporte de localização multilingue (6 línguas)
  - 13 ficheiros modificados, 840 linhas adicionadas, 86 linhas eliminadas

#### Extensão do sistema de permissões
- `4489ad6` - Adicionado serviço meteorológico wttr.in à lista branca de rede
  - Complete multi-language documentation synchronization (6 languages)
  - 14 ficheiros modificados, 417 linhas adicionadas, 1 linha eliminada

#### Correção da interface Webes
- `d9d72e9` - Corrigido problema de prioridade CSS do modal de detalhe de notas de trabalho
  - 19 ficheiros modificados, 1744 linhas adicionadas, 6 linhas eliminadas

#### Otimização do histórico de chat
- `0df599c` - Corrigido resultados de ferramentas renderizados como mensagens de chat separadas
  - 1 ficheiro modificado, 222 linhas adicionadas, 21 linhas eliminadas
- `057b09d` - Otimizada exibição de detalhe do histórico de chat, melhorada renderização de chamadas de ferramentas
  - 3 ficheiros modificados, 389 linhas adicionadas, 68 linhas eliminadas

#### Histórico de execução do temporizador
- `fa3f06f` - Adicionada funcionalidade de histórico de execução do temporizador com vista de detalhe
  - 8 ficheiros modificados, 937 linhas adicionadas, 10 linhas eliminadas
- `d824835` - Adicionadas chaves de localização do histórico de execução do temporizador (todas as línguas)
  - 7 ficheiros modificados, 88 linhas adicionadas

#### Localização Enhancement
- `c13cb17` - Registada variante de língua espanhola
  - 1 ficheiro modificado, 4 linhas adicionadas
- `9c44f34` - Adicionado suporte de localização multilingue do calendário histórico chinês
  - 16 ficheiros modificados, 6049 linhas adicionadas, 1 linha eliminada

#### Core Functionality Improvements
- `1e7c7b2` - Melhorada compressão de memória e rastreamento de execução de ferramentas
  - 4 ficheiros modificados, 338 linhas adicionadas, 86 linhas eliminadas

### 2026-04-23

#### Localização de ferramentas
- `192fc6e` - Adicionada localização de nomes de ferramentas em falta para 5 ferramentas
  - 6 ficheiros modificados, 30 linhas adicionadas

#### Documentação Updates
- `882c08f` - Atualizados todos os ficheiros changelog, adicionado histórico Git completo e removidos números de versão falsos
  - 45 ficheiros modificados, 8815 linhas adicionadas, 1611 linhas eliminadas

#### Melhoria da página de chat
- `65c157b` - Adicionado indicador de carregamento à página de chat e seleção automática da sessão do curator
  - 10 ficheiros modificados, 211 linhas adicionadas, 7 linhas eliminadas

#### Funcionalidade de histórico de chat
- `e483348` - Implementada funcionalidade de visualização do histórico de chat do ser de silício
  - Added ChatHistoryController
  - Criado ChatHistoryViewModel
  - Implementadas páginas ChatHistoryListView e ChatHistoryDetailView
  - Added localization keys for chat history (5 languages)
  - 12 ficheiros modificados, 1178 linhas adicionadas

#### Melhoria do controlo de fluxo IA
- `30a2d4e` - Cancelamento de fluxo IA, integração IM e inicialização do core host melhorados
  - 11 ficheiros modificados, 387 linhas adicionadas, 12 linhas eliminadas

#### Fila de mensagens de chat
- `db48c51` - Adicionada fila de mensagens de chat, metadados de ficheiro e suporte de cancelamento de fluxo
  - 4 ficheiros modificados, 357 linhas adicionadas

#### Suporte de upload de ficheiros
- `28fb344` - Implementado diálogo de origem de ficheiro e suporte de upload de ficheiros
  - 3 ficheiros modificados, 1100 linhas adicionadas, 2 linhas eliminadas
- `1d3e2cc` - Adicionadas cadeias de localização do diálogo de origem de ficheiro (6 línguas)
  - 6 ficheiros modificados, 30 linhas adicionadas

#### Documentação Updates
- `8111e92` - Adicionado link Wiki à secção de repositório do README
  - 1 ficheiro modificado, 3 linhas adicionadas, 1 linha eliminada

### 2026-04-22

#### Documentação Localization
- `66c11eb` - Traduzidos comentários chineses para inglês e atualizados todos os changelogs
  - 11 ficheiros modificados, 373 linhas adicionadas, 163 linhas eliminadas

#### SSE Message Enhancement
- `b574b2b` - Adicionado senderName a mensagens históricas para identificação IA
  - 1 ficheiro modificado, 9 linhas adicionadas

#### Funcionalidades de chat
- `601fc14` - Adicionada ação mark_read para marcação de fim de sessão
  - 7 ficheiros modificados, 196 linhas adicionadas, 36 linhas eliminadas

#### Sistema de ferramentas Optimization
- `7a03a19` - Melhorada flexibilidade de consulta de conversação do LogTool
  - 1 ficheiro modificado, 57 linhas adicionadas, 24 linhas eliminadas

#### Localização Enhancement
- `0a8d750` - Adicionado prompt de sistema comum para comportamentos ativos dos seres de silício
  - 8 ficheiros modificados, 460 linhas adicionadas, 48 linhas eliminadas

#### Sistema de registos Refactoring
- `2b771f3` - Desacoplado LogController das E/S de ficheiro, adicionada API de leitura de registos
  - 4 ficheiros modificados, 172 linhas adicionadas, 137 linhas eliminadas
- `12da302` - Adicionado filtro de ser de silício à vista de registos
  - 9 ficheiros modificados, 147 linhas adicionadas, 10 linhas eliminadas
- `8f6cb1e` - Adicionado parâmetro beingId à interface ILogger, implementada separação de registos sistema/ser de silício
  - 47 ficheiros modificados, 524 linhas adicionadas, 490 linhas eliminadas

#### Melhorias do sistema de permissões
- `4c747ad` - Refatorados PermissionTool, ExecuteCodeTool, adicionada API EvaluatePermission
  - 18 ficheiros modificados, 680 linhas adicionadas, 492 linhas eliminadas

#### Correções de bugs
- `1c96e99` - Corrigida falha de pesquisa search_files e search_content no diretório raiz
  - 1 ficheiro modificado, 98 linhas adicionadas, 41 linhas eliminadas

#### Integração de ferramentas
- `135710d` - Removido SearchTool, pesquisa local movida para DiskTool
  - 2 ficheiros modificados, 185 linhas adicionadas, 365 linhas eliminadas

#### Sistema de ferramentas Extension
- `70ce7fb` - Implementado DatabaseTool para consultas de base de dados estruturadas
  - 1 ficheiro modificado, 382 linhas adicionadas
- `be29a09` - Implementado LogTool para consultas de histórico de operações e conversações
  - 1 ficheiro modificado, 298 linhas adicionadas
- `4ea7702` - Implementado PermissionTool para gestão dinâmica de permissões
  - 1 ficheiro modificado, 457 linhas adicionadas
- `1384ff4` - Implementado ExecuteCodeTool para execução de código multi-linguagem
  - 1 ficheiro modificado, 477 linhas adicionadas
- `82d1e11` - Implementado SearchTool para recuperação de informações
  - 1 ficheiro modificado, 363 linhas adicionadas

#### Otimização da interface Web
- `0675c45` - Otimizada coloração de blocos de código markdown no painel de pré-visualização
  - 1 ficheiro modificado, 4 linhas adicionadas, 23 linhas eliminadas
- `702b3f3` - Vista de tarefas melhorada com emblemas de estado e exibição de metadados
  - 8 ficheiros modificados, 221 linhas adicionadas, 9 linhas eliminadas
- `6ed9a79` - Melhorado armazenamento de mensagens de chat e renderização de vistas
  - 8 ficheiros modificados, 140 linhas adicionadas, 29 linhas eliminadas

### 2026-04-21

#### Correções de bugs
- `c6b518b` - Corrigida entrega de mensagens do temporizador e armazenamento de mensagens de chat
  - 3 ficheiros modificados, 297 linhas adicionadas, 124 linhas eliminadas

#### Gestão da configuração
- `4305769` - Adicionado .gitattributes para gestão de finais de linha
  - 1 ficheiro modificado, 32 linhas adicionadas

#### Melhorias da interface Web
- `188c6f8` - Registada rota API da lista de tarefas e adicionada exibição de estado vazio
  - 2 ficheiros modificados, 35 linhas adicionadas, 2 linhas eliminadas
- `634e8ca` - Adicionado link de retorno à lista na página de permissões
  - 1 ficheiro modificado, 16 linhas adicionadas
- `6ba591d` - Adicionado editor de configuração IA independente para seres de silício
  - 11 ficheiros modificados, 842 linhas adicionadas, 18 linhas eliminadas
- `0a826f5` - Adicionado alerta de sucesso ao guardar no editor de código
  - 1 ficheiro modificado, 9 linhas adicionadas, 2 linhas eliminadas
- `2940373` - Enhanced web interface with code hover hints and UI improvements
  - 11 ficheiros modificados, 1054 linhas adicionadas, 75 linhas eliminadas

#### Correções do sistema de permissões
- `592c7ab` - Corrigida instanciação de callback e ordem de registo
  - 2 ficheiros modificados, 38 linhas adicionadas, 7 linhas eliminadas

#### Melhoria de segurança
- `833ead2` - Adicionada verificação de referência de assembly para compilação dinâmica
  - 4 ficheiros modificados, 135 linhas adicionadas, 8 linhas eliminadas

#### Melhoria do sistema de permissões
- `5879621` - Adicionada verificação pré-compilação de callback de permissão e tratamento de erros melhorado
  - 21 ficheiros modificados, 617 linhas adicionadas, 26 linhas eliminadas

#### Documentação Updates
- `4dbf659` - Atualizado changelog para v0.5.1, substituídos URLs placeholder do GitHub, adicionado espelho Gitee, nome Bilibili localizado por língua, atualizado email
  - 32 ficheiros modificados, 489 linhas adicionadas, 180 linhas eliminadas

#### Configuração e entrada
- `0fc1693` - Atualizada entrada do programa e configuração do projeto
  - 2 ficheiros modificados, 7 linhas adicionadas

#### Refatoração do sistema de permissões
- `ea9179a` - Melhorada implementação do sistema de permissões
  - 5 ficheiros modificados, 358 linhas adicionadas, 152 linhas eliminadas

#### Correções de bugs
- `928a96d` - Corrigida implementação do cálculo do calendário
  - 4 ficheiros modificados, 12 linhas adicionadas, 12 linhas eliminadas

#### IA e Calendário
- `646813e` - Melhorada implementação da fábrica de clientes IA
  - 2 ficheiros modificados, 21 linhas adicionadas, 20 linhas eliminadas

#### Localização
- `7940d9c` - Adicionado suporte de localização coreana
  - 7 ficheiros modificados, 2424 linhas adicionadas, 10 linhas eliminadas
- `4ff98ad` - Documentação refatorada para suporte multilingue
  - 81 ficheiros modificados, 23818 linhas adicionadas, 1886 linhas eliminadas

### 2026-04-20

#### Conclusão de funcionalidades centrais
- `28905b5` - Suporte multilingue completo, fábrica de clientes IA, sistema de permissões e configuração de localização
  - Sistema de registos com gestor, entradas e diferentes níveis de registo
  - Sistema de auditoria de tokens para consulta e rastreamento do uso de tokens
  - Fábricas de clientes IA para descoberta automática de diferentes plataformas IA
  - Sistema de callback de permissões com o seu próprio armazenamento
  - Implementação de logger de consola
  - Suporte multilingue para inglês e chinês simplificado
  - Mensageiro WebUI com WebSocket para chat em tempo real
  - Ser de silício padrão melhorado com localização
  - 39 ficheiros modificados, 4670 linhas adicionadas, 175 linhas eliminadas

### 2026-04-19

#### Temporizador e Calendário
- `c933fd8` - Atualizada localização, sistema de temporizador, vistas web e adicionadas ferramentas
  - Melhor gestor de localização
  - Sistema de agendamento para tarefas temporizadas
  - Configuração IA e gestão de contexto
  - Ferramenta de calendário suportando 32 tipos de calendário
  - Controlador web para APIs de calendário
  - Ferramenta de gestão de tarefas
  - 46 ficheiros modificados, 4018 linhas adicionadas, 975 linhas eliminadas

**Architecture Improvements**
- Arquitetura de vista web redesenhada para melhor suporte de temas
- Sistema de gestão de seres melhorado com melhor tratamento de estado

### 2026-04-18

- `9f585e1` - Atualizada localização, sistema de temporizador, vistas web e adicionadas ferramentas
  - Timer and scheduling improvements
  - Better web views with improved UI components
  - More tool implementations
  - 57 ficheiros modificados, 3328 linhas adicionadas, 389 linhas eliminadas

### 2026-04-17

- `9b71fcd` - Atualizados módulos centrais, adicionada documentação zh-HK, canal de difusão, ferramentas de configuração e vistas web de auditoria
  - Canal de difusão para múltiplos seres de silício conversarem juntos
  - Sistema de ferramenta de configuração
  - Vistas web de auditoria
  - Documentação em chinês tradicional
  - 42 ficheiros modificados, 3533 linhas adicionadas, 268 linhas eliminadas

### 2026-04-16

- `5040f05` - Atualizados módulos centrais e padrão
  - Otimização de módulos e correções de bugs
  - Atualizações e melhorias de implementação
  - 58 ficheiros modificados, 9916 linhas adicionadas, 111 linhas eliminadas

### 2026-04-15

- `3efab5f` - Atualizados múltiplos módulos: IA, Chat, IM, Ferramentas, Web, Localização, Armazenamento
  - Melhorias do cliente IA
  - Melhoria do sistema de chat
  - Atualizações do fornecedor de mensageiro
  - Otimização do sistema de ferramentas
  - Melhorias da infraestrutura web
  - Otimização da localização
  - Atualizações do sistema de armazenamento
  - 33 ficheiros modificados, 788 linhas adicionadas, 232 linhas eliminadas

### 2026-04-14

- `4241a2f` - Funcionalidades de chat basicamente completas, otimização de upload da UI
  - Funcionalidade do sistema de chat concluída
  - Otimização da UI para uploads de ficheiros
  - 16 ficheiros modificados, 1234 linhas adicionadas, 102 linhas eliminadas

### 2026-04-13

- `c498c31` - Atualizações de código
  - Melhorias gerais e otimização de código
  - 32 ficheiros modificados, 1045 linhas adicionadas, 546 linhas eliminadas

### 2026-04-12

#### Documentação and Localization
- `2161002` - Documentação refatorada e localização melhorada
  - 17 ficheiros modificados, 982 linhas adicionadas, 92 linhas eliminadas
- `03d94e4` - Sistema de configuração e localização melhorados
  - 25 ficheiros modificados, 1378 linhas adicionadas, 154 linhas eliminadas
- `9976a35` - Adicionada página de informações e localização
  - 14 ficheiros modificados, 699 linhas adicionadas, 44 linhas eliminadas

#### Chat e Vistas Web
- `0c8ccfc` - Sistema de chat, localização e vistas web melhorados
  - 13 ficheiros modificados, 402 linhas adicionadas, 56 linhas eliminadas
- `a8f1342` - Camada de comunicação web redesenhada, alteração de WebSocket para SSE
  - 27 ficheiros modificados, 793 linhas adicionadas, 935 linhas eliminadas

### 2026-04-11

#### Sistema de registos
- `e8fe259` - Adicionado sistema de registos e otimização de código
  - 37 ficheiros modificados, 624 linhas adicionadas, 91 linhas eliminadas
- `f01c519` - Adicionado sistema de registos, atualizadas interface IA e vistas web
  - 31 ficheiros modificados, 1758 linhas adicionadas, 63 linhas eliminadas

### 2026-04-10

- `4962924` - Manipulador WebSocket, vistas de chat e interação de mensageiro melhorados
  - Melhorias no gestor de contexto
  - Melhoria do sistema de chat
  - Atualizações da interface do fornecedor de mensageiro
  - Redesenho do fornecedor WebUI
  - Atualizações do construtor JavaScript e router
  - Otimização da vista de chat
  - Melhorias do manipulador WebSocket
  - 9 ficheiros modificados, 365 linhas adicionadas, 134 linhas eliminadas

### 2026-04-09

- `f9302bf` - Interface do fornecedor de mensageiro, sistema de chat e interação UI web melhorados
  - Extensão da interface do fornecedor de mensageiro
  - Mensagens de chat e melhorias do sistema
  - Otimização do gestor de contexto
  - Melhoria do ser de silício padrão
  - Melhorias da vista de chat da UI Web
  - Atualizações do manipulador WebSocket
  - 10 ficheiros modificados, 427 linhas adicionadas, 93 linhas eliminadas

### 2026-04-07

- `6831ee8` - Vistas web e construtor JavaScript redesenhados
  - Redesenho completo dos controladores web
  - Reescrita completa do construtor JavaScript
  - Todos os componentes de vista atualizados
  - Melhorias do sistema de temas
  - Atualização da arquitetura da classe base de vistas
  - 23 ficheiros modificados, 2004 linhas adicionadas, 1983 linhas eliminadas

### 2026-04-05

- `41e97fb` - Atualizados múltiplos módulos centrais e controladores web
  - Melhorias no gestor de contexto
  - Sistema de chat e gestão de sessões
  - Redesenho do localizador de serviços
  - Atualizações da classe base e gestor do ser de silício
  - Controladores web atualizados de forma abrangente (17 controladores)
  - Melhorias na fábrica de seres de silício padrão
  - 31 ficheiros modificados, 681 linhas adicionadas, 326 linhas eliminadas
- `67988d4` - Módulo UI web melhorado, adicionada vista de executor, limpeza de vistas e módulos centrais
  - 61 ficheiros modificados, 3148 linhas adicionadas, 3726 linhas eliminadas

### 2026-04-04

- `b58bb1c` - Adicionado controlador de inicialização e módulo web redesenhado
  - Controlador de inicialização
  - Redesenho do módulo de configuração
  - Atualizações do módulo de localização
  - Melhorias do sistema de temas
  - Melhoria do router
  - 29 ficheiros modificados, 1269 linhas adicionadas, 289 linhas eliminadas
- `f03ac0b` - Adicionado módulo UI web, melhorada funcionalidade de mensageiro
  - 60 ficheiros modificados, 8481 linhas adicionadas, 165 linhas eliminadas

### 2026-04-03

- `192e57b` - Atualizada estrutura do projeto e componentes centrais de execução
  - 22 ficheiros modificados, 446 linhas adicionadas, 179 linhas eliminadas
- `59faec8` - Atualizações da implementação central e padrão
  - 25 ficheiros modificados, 3056 linhas adicionadas, 18 linhas eliminadas
- `d488485` - Adicionada funcionalidade de compilação dinâmica e módulo de ferramenta curator
  - 19 ficheiros modificados, 1727 linhas adicionadas, 11 linhas eliminadas
- `753d1d9` - Adicionado módulo de segurança, atualizados executores, fornecedores de mensageiro, localização e ferramentas
  - 29 ficheiros modificados, 2352 linhas adicionadas, 93 linhas eliminadas
- `a378697` - Concluída fase 5 - sistema de ferramentas + executores
  - 41 ficheiros modificados, 2651 linhas adicionadas, 363 linhas eliminadas

### 2026-04-02

- `e6ad94b` - Corrigida falha no carregamento do histórico de chat ao eliminar ficheiros de configuração durante testes
  - 4 ficheiros modificados, 49 linhas adicionadas, 45 linhas eliminadas
- `daa56f5` - Concluída fase 4: memória persistente (sistema de chat + canal de mensageiro)
  - 29 ficheiros modificados, 2051 linhas adicionadas, 538 linhas eliminadas

### 2026-04-01

- `bbe2dbb` - Corrigido carregamento da configuração e encaminhamento de mensagens do serviço de chat
  - 27 ficheiros modificados, 1633 linhas adicionadas, 147 linhas eliminadas
- `2fa6305` - Implementada fase 2: framework do ciclo principal e sistema de objetos timer
  - 9 ficheiros modificados, 594 linhas adicionadas, 41 linhas eliminadas
- `32b99a1` - Implementada fase 1 - funcionalidade de chat básica
  - 19 ficheiros modificados, 1185 linhas adicionadas
- `358e368` - Commit inicial: documentação do projeto e licença
  - 10 ficheiros modificados, 1873 linhas adicionadas

### 2026-05-07

#### Localização italiana
- `8adc18c` - Adicionar suporte de localização italiana e atualizar documentação multilingue
  - Adicionada localização it-IT italiana
  - Adicionada implementação de localização ItIT (1909 linhas)
  - Adicionado ChineseHistoricalItIT suporte italiano calendário histórico chinês (586 linhas)
  - Adicionada TrayItIT localização italiana da bandeja do sistema (135 linhas)
  - Adicionado conjunto completo de documentação italiana (14 documentos)
  - Enumeração Language línguas adicionado italiano
  - 86 ficheiros modificados, 11573 inserções(+), 769 eliminações(-)
