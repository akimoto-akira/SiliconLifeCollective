# Registo de Alterações

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md) | [Русский](../ru-RU/changelog.md)

Todas as alterações importantes deste projecto serão documentadas neste ficheiro.

O formato é baseado em [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
e este projecto segue [Controlo de Versões Semântico](https://semver.org/spec/v2.0.0.html).

---

## Sobre Este Registo de Alterações

### Duas Versões do Projecto

Este projecto oferece duas versões de implementação:

- **SiliconLife.Default**: Implementação padrão, usada principalmente para verificar a viabilidade da arquitectura. Aplicação de consola, armazenamento JSON em sistema de ficheiros.
- **SiliconLife.Fast**: Versão de produção recomendada. Aplicação de ambiente de trabalho multiplataforma (Windows / macOS / Linux), armazenamento em memória SpeedyPack + persistência assíncrona, com optimização profunda de desempenho.

Ambas as versões partilham as mesmas interfaces e funcionalidades, diferindo apenas na implementação de armazenamento e no modo de execução. SiliconLife.Default serve como referência de verificação da arquitectura, enquanto SiliconLife.Fast é a versão recomendada para ambientes de produção.

### Origem do Projecto

- Este projecto teve início a 20 de Março de 2026.
- Antes deste projecto, existiu um Demo de verificação que falhou devido a um desenho arquitectónico inadequado, impossibilitando a integração com múltiplas plataformas de IA.

### Ferramentas AI IDE Utilizadas

#### Kiro (Amazon AWS)
- O projecto foi inicialmente mantido pelo Kiro, utilizando o modo Spec para arranque.
- Kiro é um ambiente de desenvolvimento AI agentic construído pela Amazon AWS.
- Baseado no Code OSS (VS Code), suporta configurações VS Code e plugins compatíveis com Open VSX.
- Possui um fluxo de trabalho de desenvolvimento orientado por especificações para codificação AI estruturada.

#### Comate AI IDE / 文心快码 (Baidu)
- Ocasionalmente utilizado para trabalho de copywriting e documentação.
- Comate AI IDE é uma ferramenta de ambiente de desenvolvimento nativo de IA lançada pelo Baidu Wenxin a 23 de Junho de 2025.
- Primeiro IDE AI do sector com colaboração multimodal e multi-agente.
- Funcionalidades incluem conversão de design para código e codificação assistida por IA em todo o fluxo.
- Alimentado pelo modelo Baidu Wenxin 4.0 X1 Turbo.

#### Trae (ByteDance)
- Utilizado entre Outubro de 2025 e Abril de 2026.
- IDE AI, suporta geração inteligente de código e gestão de projectos.

#### Qoder (Alibaba)
- Utilizado para manutenção do projecto desde 18 de Abril de 2026.
- Plataforma de codificação AI, suporta análise de código, geração de documentação e colaboração multi-agente.

#### CatPaw (Meituan)
- Utilizado em combinação com o Qoder desde 6 de Maio de 2026.
- Baseado nos modelos auto-desenvolvidos LongCat da Meituan, com forte capacidade de refactoring de arquitectura de código completo.

### Documentação de Requisitos

- A documentação de requisitos deste projecto não é pública.
- Os requisitos foram validados repetidamente por mais de 12 plataformas de IA internacionais e séries de grandes modelos, produzindo mais de 2000 linhas de documentação de requisitos orientada por user stories quase incompreensíveis para humanos.

---

## [Não Publicado]

### 2026-05-26

#### Novas Funcionalidades
- `a49041b` - Adicionar suporte de localização em russo (ru-RU) (ref task-364)
  - 216 ficheiros alterados

#### Correcções
- `79096f2` - Formato da tabela glossary alterado para Markdown padrão, removido alinhamento com espaços desnecessários
  - 1 ficheiro alterado

#### Documentação
- `174a954` - Preenchidas as colunas em falta Deutsch/Polski/Português na tradução de terminologia do glossary
  - 1 ficheiro alterado

#### Framework de Colaboração
- `5b03d53` - Actualização dos registos de tarefas .ai-collab - task-364 localização russa (ref task-364)
  - 5 ficheiros alterados

- `018947d` - Arquivadas as sessions e changes de 2026-05-25
  - 2 ficheiros alterados

### 2026-05-25

#### Novas Funcionalidades
- `14721a9` - Refinamento do prompt de pessoal ThinkOnProject para plano de acção detalhado e executável (ref task-363)
  - 20 ficheiros alterados

#### Correcções
- `abb4285` - Corrigida posição incorrecta da chamada .join() em beingsHtml (ref task-361)
  - 1 ficheiro alterado

- `1c0b9ed` - Eliminado bug de string duplicada state-initial causado pela renderização states-overview em WorkflowDetailView (ref task-362)
  - 6 ficheiros alterados

#### Framework de Colaboração
- `ecc48a1` - Actualização dos metadados .ai-collab (relatedCommit e activity log) (ref task-361)
  - 4 ficheiros alterados

- `64529a7` - Arquivadas as sessions e changes de 2026-05-24 (execução manual complementar)
  - 28 ficheiros alterados

- `4150e52` - Arquivadas tarefas concluídas task-341~361 (ref archive)
  - 2 ficheiros alterados

### 2026-05-24

#### Novas Funcionalidades
- `db60fd9` - Lista de permissões de ferramentas mostra ferramentas sem declaração ToolAction e marca como não configuráveis (ref task-331, task-332, task-333)
  - 21 ficheiros alterados

- `6004a7f` - WorkflowTemplate adiciona suporte a definição de funções + localização em 12 idiomas + correcção do DiskTool (ref task-346)
  - 24 ficheiros alterados

- `75ce452` - Pool de funções do ProjectSpace e acções de gestão de funções do ProjectTool (ref task-347)
  - 12 ficheiros alterados

- `edfb600` - BuildProjectScenarioContext adiciona informações de funções (ref task-348)
  - 21 ficheiros alterados

- `6a2d713` - HasProjectsWithoutTemplate expandido para HasProjectsNeedingAttention (ref task-349)
  - 21 ficheiros alterados

- `a773224` - Criação de tarefas de fluxo de trabalho passa a usar pool de funções para atribuir executores (ref task-350)
  - 6 ficheiros alterados

- `77a27f9` - Expansão do TravelCodeWikiTool como entrada de entidades geográficas (ref task-353)
  - 8 ficheiros alterados

- `873ef23` - Implementação do GeoDataTool concluída, actualização do estado .ai-collab (ref task-352)
  - 7 ficheiros alterados

- `feaccab` - Implementação do GeoContentTool concluída, actualização do estado .ai-collab (ref task-351)
  - 6 ficheiros alterados

- `6e60ad1` - Expansão do GeoLanguageTool (suporte ObjectPath + set_word), retro-preenchimento de metadados (ref task-356, task-355)
  - 7 ficheiros alterados

- `4eff807` - Implementação de GetWikiDocuments() nas subclasses GeoLocation (ref task-357)
  - 5 ficheiros alterados

- `baad5df` - Implementação do serviço de publicação MediaWiki API (ref task-358)
  - 6 ficheiros alterados

- `b846a21` - Implementação da página de detalhes do fluxo de trabalho (ref task-361)
  - 24 ficheiros alterados

#### Correcções
- `a290088` - Silicon Beings criados via CuratorTool perdidos após reinício (ref task-334)
  - 11 ficheiros alterados

- `69a8cba` - Corrigido bug da página de tarefas não filtrar por beingId (ref task-360)
  - 8 ficheiros alterados

- `7dd1a65` - Registo da rota da página de detalhes do fluxo de trabalho no Router.cs (ref task-361)
  - 1 ficheiro alterado

#### Refactorização
- `5e02711` - Refactorização da abstracção de caminhos de armazenamento na camada comum, eliminando hardcoded do sistema de ficheiros (ref task-335)
  - 12 ficheiros alterados

- `0ec0929` - DynamicBeingLoader.SaveBeingCode usa IStorage em vez de operações directas no sistema de ficheiros (ref task-336)
  - 7 ficheiros alterados

- `9a44b48` - Ponte IStorage do PlaywrightWebView + desacoplamento da classe base WebViewBrowserTool (ref task-337, task-340)
  - 11 ficheiros alterados

- `8fea742` - WebViewBrowserTool gravação de capturas de ecrã usa IStorage em vez de operações directas no sistema de ficheiros (ref task-338)
  - 6 ficheiros alterados

- `4c24e6d` - DefaultPermissionCallback usa BeingPathResolver em vez de caminhos hardcoded (ref task-339)
  - 6 ficheiros alterados

- `ab428cd` - Removido downcast do DefaultSiliconBeing, chamada directa ao SaveState() da classe base (ref task-344)
  - 7 ficheiros alterados

- `1e6eb80` - Ficheiros temporários do estado do navegador PlaywrightWebView passam a usar leitura/escrita directa via IStorage (ref task-341)
  - 7 ficheiros alterados

- `17f00e9` - Operações de pesquisa do DiskTool passam pelo DiskExecutor (ref task-342)
  - 8 ficheiros alterados

- `8158703` - Verificação de anexos do ChatController passa pelo DiskExecutor (ref task-343)
  - 7 ficheiros alterados

- `3243ae6` - Reescrita do TravelCodeWikiPublishWorkflow como máquina de estados de 7 passos, remoção do ficheiro TravelCodeWikiWithAI de rastreamento forçado (ref task-355)
  - 6 ficheiros alterados

#### Limpeza
- `d685288` - Eliminação do HotReloadTool.cs e do directório tools/HotReload (ref task-345)
  - 8 ficheiros alterados

#### Documentação
- `f1789d1` - Optimização da linha de descrição do README.md (ref task-359)
  - 9 ficheiros alterados

#### Framework de Colaboração
- `982c6bb` - Preenchimento dos campos relatedCommit e commitHash em falta no .ai-collab
  - 6 ficheiros alterados

- `d91e9f8` - Arquivadas task-331~340, quadro de tarefas limpo
  - 2 ficheiros alterados

- `9135e30` - Publicadas task-341~344 refactorização IStorage da camada comum + correcção de abstracção
  - 1 ficheiro alterado

- `f70b350` - Adicionadas 13 tarefas de reestruturação arquitectónica do TravelCodeWikiWithAI (ref task-346~358)
  - 2 ficheiros alterados

- `f81d38b` - Actualização dos ficheiros de session e task tracking do ai-collab
  - 3 ficheiros alterados

### 2026-05-23

#### Correcções
- `9c3c64e` - Corrigida verificação de permissões em tempo de execução do ExecuteTool que contornava restrições ao nível do projecto (ref task-324)
  - 7 ficheiros alterados

- `94a9e35` - Corrigida inconsistência entre definição de modelos de permissões e declaração ToolActionAttribute (ref task-325)
  - 6 ficheiros alterados

- `e8d8371` - Ferramentas com todas as Actions desactivadas removidas integralmente dos pedidos à IA (ref task-326)
  - 6 ficheiros alterados

- `32c7d8a` - API de permissões de ferramentas adiciona validação de nome de Action + correcção de renderização Markdown do histórico de chat (ref task-327, task-328, task-329)
  - 9 ficheiros alterados

- `797db8c` - Renderização Markdown fallback define erroneamente mdRendered impedindo que marked recarregue e re-renderize (ref task-330)
  - 9 ficheiros alterados

#### Framework de Colaboração
- `1496094` - Publicadas tarefas de reparação do framework de autorização de ferramentas task-324~327
  - 776 ficheiros alterados

- `0d16e63` - Actualização do estado das tarefas de colaboração, associação do task-330 ao commit 797db8c, preparação para arquivo
  - 2 ficheiros alterados

- `e602e1c` - Arquivadas task-316~330, quadro de tarefas limpo (ref task-316~330)
  - 2 ficheiros alterados

- `20291ce` - Arquivamento diário de sessions e changes (13-22 de Maio)
  - 106 ficheiros alterados

### 2026-05-22

#### Correcções de Consistência da Documentação
- `9e07b27` - Corrigidas diferenças de consistência entre documentação francesa (fr-FR) e código fonte (ref task-307)
  - 10 ficheiros alterados

- `9e3be72` - Corrigida consistência entre documentação alemã (de-DE) e código fonte (ref task-308)
  - 5 ficheiros alterados

- `2bc7151` - Corrigidas diferenças de consistência entre documentação espanhola (es-ES) e código fonte (ref task-309)
  - 13 ficheiros alterados

- `f95088e` - Corrigida consistência entre documentação italiana (it-IT) e código fonte (ref task-310)
  - 11 ficheiros alterados

- `6ea9f4a` - Corrigida consistência entre documentação polaca (pl-PL) e código fonte (ref task-311)
  - 16 ficheiros alterados

- `7646923` - Corrigida consistência entre documentação portuguesa (pt-PT) e código fonte (ref task-312)
  - 12 ficheiros alterados

- `7eaf9db` - Corrigida consistência entre documentação checa (cs-CZ) e código fonte (ref task-313)
  - 12 ficheiros alterados

#### Framework de Colaboração
- `3cb7347` - Actualização task-313 relatedCommit=7eaf9db
  - 1 ficheiro alterado

### 2026-05-21

#### Novas Funcionalidades
- `99eca78` - Menu de contexto adiciona funcionalidade "Ver Armazenamento (só de leitura)", chamada intra-processo ao Speedy.Manager (ref task-301)
  - 26 ficheiros alterados

#### Correcções de Consistência da Documentação
- `7f65cf1` - Corrigidas diferenças de consistência entre documentação zh-CN e código fonte (ref task-303)
  - 15 ficheiros alterados

- `a9e2a2c` - Corrigidas diferenças de consistência entre documentação inglesa (en) e código fonte (ref task-302)
  - 9 ficheiros alterados

- `2549105` - Corrigidas diferenças de consistência entre documentação em chinês tradicional (zh-HK) e código fonte (ref task-304)
  - 12 ficheiros alterados

- `277eb50` - Corrigidas diferenças de consistência entre documentação japonesa e código fonte (ref task-305)
  - 10 ficheiros alterados

- `edce413` - Corrigidas diferenças de consistência entre documentação coreana (ko-KR) e código fonte (ref task-306)
  - 18 ficheiros alterados

- `f2adcae` - Corrigido problema de inconsistência entre documentação portuguesa e código fonte (ref task-220)
  - 15 ficheiros alterados

- `3332987` - Corrigido problema de inconsistência entre documentação em chinês tradicional (Hong Kong) e código fonte (ref task-218)
  - 14 ficheiros alterados

- `af9f715` - Corrigido problema de inconsistência entre documentação polaca e código fonte (ref task-217)
  - 15 ficheiros alterados

- `2e2b18b` - Corrigido problema de inconsistência entre documentação coreana e código fonte (ref task-216)
  - 16 ficheiros alterados

- `626ebc9` - Corrigido problema de inconsistência entre documentação japonesa e código fonte (ref task-215)
  - 19 ficheiros alterados

- `48d061b` - Corrigido problema de inconsistência entre documentação italiana e código fonte (ref task-214)
  - 14 ficheiros alterados

#### Framework de Colaboração
- `6683bee` - Registo da equipa Marvis AI, actualização do estado das tarefas
  - 3 ficheiros alterados

- `03fc905` - Arquivadas task-210~220
  - 5 ficheiros alterados

### 2026-05-20

#### Novas Funcionalidades
- `65176d4` - Adicionar suporte completo de localização em português (pt-PT + pt-BR) (ref task-208)
  - 41 ficheiros alterados

#### Correcções de Consistência da Documentação
- `af4dffd` - Corrigidos todos os problemas de inconsistência entre documentação zh-CN e código fonte (ref task-209)
  - 11 ficheiros alterados

- `144b945` - Corrigidos problemas de inconsistência entre documentação inglesa (en) e checa (cs-CZ) e código fonte (ref task-219, task-210)
  - 22 ficheiros alterados

- `08bec55` - Corrigido problema de inconsistência entre documentação alemã (de-DE) e código fonte (ref task-211)
  - 14 ficheiros alterados

- `7ff28de` - Corrigido problema de inconsistência entre documentação espanhola (es-ES) e código fonte (ref task-212)
  - 14 ficheiros alterados

- `15e2133` - Corrigido problema de inconsistência entre documentação francesa (fr-FR) e código fonte (ref task-213)
  - 13 ficheiros alterados

#### Correcções
- `7dac388` - Corrigida impossibilidade de exibir lista de tarefas do projecto (ref task-207)
  - 6 ficheiros alterados

#### Framework de Colaboração
- `7890223` - Arquivadas task-201~209, publicadas task-210~220 tarefas de correcção de consistência da documentação
  - 5 ficheiros alterados

### 2026-05-19

#### Novas Funcionalidades
- `cd72846` - Implementada solução de segurança alternativa para bypass da verificação de segurança do PluginLoader (ref task-203)
  - 13 ficheiros alterados

- `fc0c00c` - Melhorias no Speedy.Manager - Criar/Importar/Exportar/TreeView hierárquica/Janela de progresso (ref task-206)
  - 9 ficheiros alterados

#### Correcções
- `ec07118` - Corrigido problema de ITypeRegistry/IObjectFactory não registados antes do carregamento de plugins (ref task-205)
  - 8 ficheiros alterados

- `9e749db` - Corrigido erro "Creator ID is required" ao criar projecto (ref task-204)
  - 4 ficheiros alterados

#### Infraestrutura
- `43dc092` - Migração CLDR - Adicionar CldrDataProvider, remover .github
  - 1 ficheiro alterado

- `c09ec1f` - Adicionar cldr/ ao .gitignore
  - 1 ficheiro alterado

- `221f818` - Sincronização GitHub alterada para esquema de espelhamento push via Gitee, workflow mantido apenas como backup manual
  - 1 ficheiro alterado

- `08cdf1a` - Corrigido workflow de sincronização GitHub - Adicionar lógica de retry e skip sem alterações
  - 1 ficheiro alterado

- `fb4e77d` - Actualização do SiliconLife.Speedy.Manager.csproj
  - 1 ficheiro alterado

#### Framework de Colaboração
- `df90af0` - Actualização task-203 relatedCommit=cd72846
  - 1 ficheiro alterado

### 2026-05-18

#### Refactorização
- `e720d06` - Reconversão completa do Speedy.Manager de WinForms para Avalonia (ref task-202)
  - 17 ficheiros alterados

#### Correcções
- `08894a9` - Corrigido erro de exibição do nível de entradas de resumo na linha temporal de memória (ref task-201)
  - 3 ficheiros alterados

#### Framework de Colaboração
- `2871afb` - Arquivadas todas as tarefas, tasks.json limpo
  - 2 ficheiros alterados

### 2026-05-17

#### Novas Funcionalidades
- `d6eb994` - Página de lista de projectos adiciona entrada para criar projecto e selecção de modelo de fluxo de trabalho (ref task-203)
  - 14 ficheiros alterados

- `0872134` - Orquestração orientada pelo Curator para projectos sem modelo no ThinkOnProject (ref task-202)
  - 6 ficheiros alterados

- `cb3188e` - Visualização de menções @ em chat de grupo (ref task-208)
  - 4 ficheiros alterados

- `f9968e5` - Declaração de capacidade ToolCall do cliente de IA e degradação elegante (ref task-205)
  - 4 ficheiros alterados

- `0d2b843` - Lógica de decisão em chat de grupo ShouldReplyInGroupChat (ref task-201)
  - 6 ficheiros alterados

- `277a2b1` - Complemento da rede de conhecimento - Consultas avançadas e travessia do grafo (ref task-207)
  - 9 ficheiros alterados

#### Correcções
- `6d0b66e` - Corrigido TypeError appendMessage ao enviar mensagens em chat de grupo (ref task-209)
  - 5 ficheiros alterados

- `b15167c` - Commit complementar do registo de rota list-workflow-templates omitido no task-203 (ref task-203)
  - 1 ficheiro alterado

- `dc549a2` - Corrigido workflow de sincronização Gitee - Adicionar username ao token URL
  - 1 ficheiro alterado

#### Infraestrutura
- `e5fa3ad` - Desactivada sincronização automática GitHub schedule, aguardando solução oficial de sincronização Gitee
  - 1 ficheiro alterado

#### Framework de Colaboração
- `4a58c82` - Adicionado relatório de análise de capacidades do sistema + plano de design ThinkOnProject
  - 5 ficheiros alterados

- `8ab29e6` - Relatório de análise de integridade de capacidades do sistema arquivado em .ai-collab/docs
  - 2 ficheiros alterados

- `b412d9c` - Arquivadas tarefas antigas, republicadas task-201~208 com base na análise abrangente
  - 2 ficheiros alterados

- `437884a` - Actualização dos metadados de colaboração - task-202/203/204 concluídas (ref task-202, task-203, task-204)
  - 2 ficheiros alterados

- `bf78d79` - Actualização dos metadados de colaboração - task-201/205/208 concluídas
  - 2 ficheiros alterados

- `de6ee0e` - Registo de fim de sessão catpaw-20260517-2215
  - 5 ficheiros alterados

- `7223b6f` - Registo de fim de sessão catpaw-20260517-2200
  - 4 ficheiros alterados


## [Alpha-0.2] - 2026-05-16

### 2026-05-16

#### Preparação do Lançamento
- `476d839` - Adicionar tarefas de lançamento alpha-0.2
  - Criada task-114 (escrita do CHANGELOG) e task-115 (actualização do número de versão)
  - 1 ficheiro alterado

### 2026-05-15

#### Infraestrutura
- `672627b` - Adicionar workflow de sincronização Gitee (com configuração de permissões)
  - Actualização da configuração de permissões do workflow sync-from-gitee.yml
  - 1 ficheiro alterado, 7 linhas adicionadas, 4 linhas removidas

- `3cd5256` - Adicionar GitHub Actions para sincronização automática de código do Gitee
  - Novo workflow sync-from-gitee.yml
  - 1 ficheiro alterado, 50 linhas adicionadas

#### Actualização da Documentação
- `aa1d2ad` - Actualização dos documentos README/arquitectura/introdução em todos os 11 idiomas, reflectindo suporte multiplataforma do SiliconLife.Fast (ref task-112, task-113)
  - Corrigida descrição no documento de que SiliconLife.Fast era apenas Windows, reflectindo o suporte multiplataforma real (Windows / macOS / Linux)
  - Actualizados README.md, architecture.md, getting-started.md em 11 idiomas
  - SelectComponent adiciona suporte ao atributo hint
  - ConfigView passa hint para dropdowns de enumeração
  - 11 idiomas de localização adicionam chave SelectSearchHint
  - 53 ficheiros alterados, 690 linhas adicionadas, 194 linhas removidas

#### Sistema de Tarefas
- `3329f3d` - Adicionar mecanismo de inspeção do sistema de tarefas + tarefas de correcção de bugs de localização
  - Criada task-113: Corrigir problema de localização da página sobre
  - Actualizada task-112: Actualizar documentação da versão Fast para suportar Linux
  - Arquivadas tarefas concluídas (11) em .ai-collab/archive/
  - Configuração do mecanismo de inspeção concluída: inspeção rápida (a cada 30 minutos) + inspeção completa (diariamente às 06:00)
  - 2 ficheiros alterados, 148 linhas adicionadas, 171 linhas removidas

#### Framework de Colaboração
- `6038e22` - Registar coze-agent no registo de colaboração .ai-collab
  - Adicionadas informações de registo do AI residente da plataforma Coze
  - 1 ficheiro alterado

### 2026-05-14

#### Framework de Colaboração AI
- `7344fbb` - Removido modo handoff, alterado para orientação por lista de tarefas (v2.0)
  - Reestruturação do directório .ai-collab, de modo de entrega handoff para orientação por lista de tarefas
  - Novo ficheiro central tasks.json da lista de tarefas
  - Novo activity.log de registo de operações
  - Novos directórios changes/ e sessions/

- `589a48e` - Adicionar registo de sessão .ai-collab
  - Novo registo de estado de sessão de colaboração AI

- `5481bcf` - Registar Qoder AI IDE no registo de colaboração
  - Adicionadas informações de registo do assistente de programação AI Qoder

- `e2d7b61` - Preencher relatedCommit e changes commitHash no tasks.json
  - Melhoria da associação de metadados de tarefas

- `a087f0c` - Aceitação de todas as tarefas task-101~110
  - Confirmada conclusão de todas as 10 tarefas de reparação

#### Correcções de Bugs
- `fac9435` - Conclusão de todas as 10 reparações e implementações de tarefas task-101~110
  - Corrigido componente de selecção de pesquisa sem texto de dica
  - Corrigido problema de localização da página sobre
  - Corrigido erro JS de pesquisa do sistema de ajuda
  - 39 ficheiros alterados, 684 linhas adicionadas, 121 linhas removidas

- `c46dfbc` - Conclusão de todas as tarefas pendentes (task-001~006)
  - Concluídas 6 tarefas pendentes iniciais

- `ec176b2` - Substituição da lista de tarefas - Revisão de código descobriu 10 novos bugs
  - Criadas task-101~110, total de 10 novas tarefas

#### Refactorização
- `ab15915` - Unificação de cabeçalhos de copyright + correcção de BOM do HelpController e JS de pesquisa do HelpView
  - Unificação do cabeçalho Apache 2.0 em todos os ficheiros fonte C#
  - Corrigido problema de codificação BOM do HelpController
  - Corrigido erro de JavaScript de pesquisa do HelpView

#### Novas Funcionalidades
- `18a6f5d` - Criar servidor de capacidades MCP browser (ref task-111)
  - Novo projecto SiliconLife.McpServer
  - Implementação do servidor MCP de automação de navegador Playwright

- `9eb251a` - Remover módulo SiliconLife.McpServer (ref task-111)
  - Removido servidor MCP independente, funcionalidade integrada no projecto principal

### 2026-05-13

#### Localização
- `7a62590` - Adicionar suporte de localização em polaco
  - Nova implementação de localização pl-PL polaca (PlPL.cs, 1089 linhas)
  - Nova localização de documentação de ajuda em polaco (HelpLocalizationPlPL.cs, 3972 linhas)
  - Novo suporte de calendário histórico chinês em polaco (ChineseHistoricalPlPL.cs, 600 linhas)
  - Nova localização da bandeja em polaco (TrayPlPL.cs, 135 linhas)
  - Novo conjunto completo de documentação em polaco (15 documentos)
  - Enumeração Language adiciona polaco
  - 35 ficheiros alterados, 14379 linhas adicionadas, 11 linhas removidas

- `51f9c8e` - Actualização de referências ao Ark AI e melhorias de terminologia na documentação
  - Actualização de terminologia de clientes de IA na documentação multilingue

- `7587c12` - Adicionar entradas de registo de alterações para todos os idiomas
  - Sincronização da actualização do changelog em todas as versões linguísticas

#### Migração do Sistema de Janelas
- `b49a07d` - Migração para modo de janela residente Avalonia
  - Removida dependência do Windows Forms, migração completa para o framework Avalonia UI
  - Janela de estado exibida correctamente no Linux (verificação via ambiente de trabalho remoto)
  - Adicionados controlos de janela: menu de contexto, duplo clique para abrir Web, botão de fecho
  - Adicionado framework de colaboração multi-AI (.ai-collab/)
  - Corrigida inicialização do ícone da bandeja (degradação elegante)
  - Novos App.axaml e App.cs como entrada da aplicação Avalonia
  - 13 ficheiros alterados, 1442 linhas adicionadas, 541 linhas removidas

- `d335aaf` - Janela do Linux sempre visível + diálogo de confirmação ao fechar
  - Exibição automática da janela de estado no Linux (sem ícone de bandeja)
  - Diálogo de confirmação ao fechar a janela no Linux
  - Windows/macOS mantêm comportamento original da bandeja
  - Suporte ao parâmetro --no-tray para desactivar forçadamente a bandeja
  - Novo método ShowMessageBoxAsync para diálogo de confirmação
  - 3 ficheiros alterados, 206 linhas adicionadas, 29 linhas removidas

#### Refactorização do Sistema de Bandeja
- `841d384` - Refactorização do sistema de bandeja e inicialização do framework de colaboração AI
  - Simplificação do TrayLocalizationBase removendo propriedades não utilizadas
  - Adicionar item de localização ShowStatus
  - App.cs adiciona clique no ícone da bandeja para mostrar janela de estado, itens de menu localizados
  - Program.cs move inicialização do ícone da bandeja para StartAsync
  - TrayStatusWindow oculta em vez de sair ao fechar
  - Registar trae-glm5 e catpaw no framework de colaboração .ai-collab
  - Actualização do .gitignore garantindo que todos os ficheiros .ai-collab são rastreados
  - 22 ficheiros alterados, 178 linhas adicionadas, 1226 linhas removidas

#### Documentação
- `43653bc` - Actualização da descrição do repositório e registo AI
  - Actualização do README do projecto e informações de registo .ai-collab

### 2026-05-12

#### Vista Web do Sistema de Tarefas
- `0891b3c` - Adicionar vista de detalhes de execução e histórico de tarefas
  - Nova vista de detalhes de execução de tarefas TaskExecutionDetailView
  - Nova vista de histórico de execução de tarefas TaskExecutionHistoryView
  - TaskController adiciona interfaces de consulta de detalhes e histórico de execução
  - Novo modelo de vista de tarefas TaskViewModel
  - Centro de tarefas TaskCenter melhorado
  - Sistema de tarefas TaskSystem actualizado
  - 9 idiomas de localização adicionam chaves relacionadas com tarefas
  - 26 ficheiros alterados, 803 linhas adicionadas, 55 linhas removidas

### 2026-05-11

#### Refactorização da Arquitectura de Componentes Web
- `5e687ad` - Migração da renderização de componentes de strings para H-tree
  - Métodos de renderização do ComponentBase migrados do modo string para estrutura H-tree
  - Todos os 28 componentes adaptados à nova arquitectura de renderização (A, Accordion, Button, Calendar, Card, Chart, etc.)
  - SelectComponent grandemente refactorizado (889 linhas melhoradas)
  - Controladores e vistas sincronizados e actualizados
  - 33 ficheiros alterados, 667 linhas adicionadas, 435 linhas removidas

- `bfd332d` - Migração de Style de strings para estilos inline CssBuilder
  - Novo construtor de estilos CssBuilder
  - Sistema de estilos do ComponentBase migrado de strings para CssBuilder estruturado
  - LoadingComponent grandemente melhorado (103 linhas adicionadas)
  - Migração de estilos dos controladores ConfigController, LogController, MemoryController
  - Migração de estilos das vistas ChatView, ConfigView, LogView, MemoryView
  - 37 ficheiros alterados, 351 linhas adicionadas, 157 linhas removidas

#### Optimização do Sistema de Armazenamento
- `d67a7ee` - Optimização de consultas QueryLatest em grandes conjuntos de dados
  - Optimização de desempenho do método QueryLatest do SpeedyTimeStorage
  - Melhoria do fornecedor de registos SpeedyLoggerProvider
  - 2 ficheiros alterados, 44 linhas adicionadas, 5 linhas removidas

#### Refactorização do Sistema de Calendário
- `9629f88` - Extracção do TimerExecution e melhoria das vistas Web de temporizadores
  - TimerSystem extrai lógica TimerExecution (175 linhas removidas)
  - SelectComponent grandemente melhorado (427 linhas melhoradas)
  - TimerController e vistas de temporizadores melhorados
  - ContextManager actualizado
  - 12 ficheiros alterados, 458 linhas adicionadas, 267 linhas removidas

#### Localização
- `5d8ca79` - Adicionar chave de localização LogsLoading
  - 9 idiomas adicionam chave LogsLoading
  - Classe base DefaultLocalizationBase adiciona definição
  - 11 ficheiros alterados, 15 linhas adicionadas

### 2026-05-10

#### Refactorização do Sistema de Tarefas
- `54394f6` - Fusão do sistema de tarefas com ciclos de histórico de chat
  - Sistema de tarefas do projecto ProjectTaskSystem grandemente simplificado (411 linhas refactoradas)
  - Sistema de tarefas TaskSystem simplificado (254 linhas refactoradas)
  - Centro de tarefas TaskCenter refactorado (188 linhas melhoradas)
  - ContextManager optimizado (347 linhas refactoradas)
  - Silicon Being padrão melhorado
  - Sistema de temporizadores TimerSystem integra tarefas
  - Interface IWorkNoteStorage actualizada
  - SpeedyWorkNoteStorage e FileSystemWorkNoteStorage adaptados
  - 16 ficheiros alterados, 648 linhas adicionadas, 897 linhas removidas

### 2026-05-09

#### Melhorias da Interface Web
- `bc50dd7` - Melhoria da vista de chat e adição de funcionalidade de auditoria
  - Novo controlador de auditoria AuditController (261 linhas)
  - Nova vista de auditoria AuditView (379 linhas)
  - Novo modelo de vista de auditoria AuditViewModel
  - Vista de chat ChatView grandemente melhorada (171 linhas melhoradas)
  - Controlador de chat ChatController actualizado
  - Componente MarkdownEditorComponent melhorado
  - Controlador de inicialização InitController melhorado
  - Sistema de chat ChatSystem adiciona funcionalidades
  - 14 ficheiros alterados, 1030 linhas adicionadas, 112 linhas removidas

- `c9babce` - Melhoria da renderização de chamadas de ferramentas na vista de chat
  - Melhoria da renderização de blocos de chamadas de ferramentas no ChatView
  - 1 ficheiro alterado, 54 linhas adicionadas, 11 linhas removidas

#### Sistema de Cenários de Ferramentas AI
- `ff2eddd` - Implementação do sistema de filtragem de cenários de ferramentas
  - Novo atributo de cenário de ferramenta ToolScenarioAttribute (36 linhas)
  - Novo atributo de cenário apenas chat ChatOnlyAttribute (19 linhas)
  - ToolManager adiciona funcionalidade de filtragem de cenários (40 linhas)
  - ContextManager adaptado à filtragem de cenários
  - 4 ficheiros alterados, 115 linhas adicionadas, 30 linhas removidas

- `5709a33` - Adicionar atributos de cenário a classes de ferramentas
  - 24 classes de ferramentas adicionam atributos ToolScenario
  - Incluindo ferramentas de calendário, chat, configuração, curadoria, base de dados, disco, compilação dinâmica, etc.
  - 24 ficheiros alterados, 46 linhas adicionadas, 20 linhas removidas

#### Refactorização do Sistema de Tarefas
- `2f19a5f` - Refactorização do sistema de tarefas com TaskCenter e TaskEnumerator
  - Novo centro de tarefas TaskCenter (235 linhas)
  - Novo enumerador de tarefas TaskEnumerator (297 linhas)
  - Sistema de tarefas TaskSystem refactorado e simplificado
  - Silicon Being padrão adaptado à nova arquitectura
  - Fábrica DefaultSiliconBeingFactory actualizada
  - Classe base SiliconBeingBase melhorada
  - 7 ficheiros alterados, 796 linhas adicionadas, 275 linhas removidas

#### Migração do Sistema de Permissões
- `a06ed09` - Migração do sistema de IM e permissões para o projecto App
  - PermissionRequestQueue migrado de Default/Fast para o projecto App (443 linhas adicionadas)
  - Removido WebUIProvider da versão Default (403 linhas removidas)
  - Removido HelpTool da versão Default (194 linhas removidas)
  - Removido PermissionRequestQueue duplicado das versões Default/Fast
  - Removido IMPermissionAskHandler da versão Default
  - Controlador PermissionRequestController actualizado
  - 14 ficheiros alterados, 496 linhas adicionadas, 1183 linhas removidas

#### Optimização do Contexto AI
- `4c8aaff` - Optimização do gestor de contexto e melhoria do localizador de serviços
  - ContextManager simplificado e optimizado
  - ServiceLocator melhorado (36 linhas adicionadas)
  - ToolManager melhorado (34 linhas adicionadas)
  - Clientes DashScopeClient e VolcengineArkClient melhorados
  - Executores (CommandLine, Disk, Network) actualizados
  - 8 ficheiros alterados, 116 linhas adicionadas, 98 linhas removidas

#### Localização
- `5c5eef7` - Adicionar chaves de localização de auditoria e tarefas
  - DefaultLocalizationBase adiciona 127 linhas de definições de localização
  - 9 idiomas adicionam chaves relacionadas com auditoria e tarefas (26 linhas cada)
  - 11 ficheiros alterados, 387 linhas adicionadas

#### Configuração do Projecto
- `2067db6` - Actualização da configuração do projecto e regras gitignore
  - Regras .gitignore actualizadas
  - DefaultConfigData e Fast DefaultConfigData melhorados
  - SpeedyWorkNoteStorage melhorado
  - Núcleo do SpeedyPack melhorado
  - 5 ficheiros alterados, 32 linhas adicionadas, 6 linhas removidas

### 2026-05-07

#### Localização Italiana
- `8adc18c` - Adicionar suporte de localização italiano e actualizar documentação multilingue
  - Nova localização it-IT italiana
  - Nova implementação de localização ItIT (1909 linhas)
  - Novo suporte ao calendário histórico chinês em italiano ChineseHistoricalItIT (586 linhas)
  - Nova localização da bandeja em italiano TrayItIT (135 linhas)
  - Novo conjunto completo de documentação em italiano (14 documentos: README, referência API, arquitectura, sistema de calendário, registo de alterações, guia de contribuição, etc.)
  - Actualização de documentos de arquitectura, guia de desenvolvimento, guia de introdução, etc. em todos os idiomas
  - Enumeração Language adiciona italiano
  - 86 ficheiros alterados, 11573 linhas adicionadas, 769 linhas removidas

#### Sincronização da Documentação
- `12a5deb` - Actualização da documentação multilingue de arquitectura, registo de alterações e guia dos Silicon Beings
  - Actualização do README em 8 idiomas
  - Actualização da documentação de arquitectura em 8 idiomas
  - Actualização do registo de alterações em 8 idiomas
  - Actualização do guia dos Silicon Beings em 8 idiomas
  - Actualização da referência de ferramentas em 8 idiomas
  - Glossário refactorado
  - 46 ficheiros alterados, 1697 linhas adicionadas, 442 linhas removidas

### 2026-05-06

#### Grande Refactorização de Módulos
- `eeb3be6` - Grande refactorização e reorganização de módulos
  - Ajuste da estrutura do projecto SiliconLife.App
  - Reorganização do projecto SiliconLife.Fast
  - Reorganização do projecto SiliconLife.Default
  - Reorganização do módulo partilhado SiliconLife.Common
  - Reorganização do módulo central SiliconLife.Core
  - Reorganização do motor de armazenamento SiliconLife.Speedy
  - Reorganização da ferramenta de gestão SiliconLife.Speedy.Manager
  - 119 ficheiros alterados, 6926 linhas adicionadas, 3066 linhas removidas

### 2026-05-04

#### Cliente de IA
- `24d2c86` - Adicionar VolcengineArkClient e substituir Audit por Usage tracking
  - Novo cliente de IA VolcengineArkClient para Volcengine Ark
  - Suporte a modos de streaming e não-streaming
  - Controlo de taxa de dupla camada integrado (auto-controlo de taxa + limite de taxa do servidor)
  - Compatível com protocolo OpenAI API
  - Sistema Audit substituído por Usage tracking
  - 24 ficheiros alterados, 802 linhas adicionadas, 21 linhas removidas

#### Sistema de Ferramentas
- `f27650a` - Adicionar ferramenta de recarregamento a quente para reinício automático do Fast
  - Nova ferramenta de recarregamento a quente HotReloadTool
  - Suporte a compilação online, actualização e reinício do SiliconLife.Fast
  - Novo actualizador independente HotReload.exe
  - Mecanismo seguro de cópia de ficheiros (não sobrescreve a si próprio)
  - Encerramento elegante e espera pela libertação da porta
  - 9 ficheiros alterados, 581 linhas adicionadas

#### Localização
- `6a5aad8` - Actualizar todos os ficheiros e adicionar suporte de localização francesa
  - Nova localização fr-FR francesa
  - Actualização de todas as versões linguísticas
  - Tradução da documentação de ajuda para francês
  - Tradução da interface para francês
  - 100+ ficheiros alterados

### 2026-05-03

#### Infraestrutura do Projecto
- `2664b0c` - Actualização da infraestrutura e dependências do projecto
  - SiliconLife.Speedy.Manager adiciona interface de gestão WPF (MainForm.Designer.cs, MainForm.resx)
  - Novo recurso de ícone slc.ico (1.5MB)
  - PluginLoader grandemente melhorado com verificação de segurança (622 linhas adicionadas)
  - Nova fábrica de streams com permissões PermissionedStreamFactory (779 linhas)
  - Nova fila de pedidos de permissão PermissionRequestQueue (versões Default e Fast)
  - Novo fornecedor de registos de depuração DebugLoggerProvider
  - Classe base de configuração ConfigDataBase melhorada
  - ToolManager adiciona funcionalidade de pesquisa de ferramentas de plugins (ScanAllPluginAssemblies)
  - Gestão do ciclo de vida do SiliconBeingManager melhorada
  - Cliente de IA Alibaba Cloud DashScopeClient grandemente melhorado (227 linhas adicionadas)
  - Fábrica DefaultSiliconBeingFactory melhorada
  - Vistas Web e controladores actualizados (ChatView, WorkNoteView, PermissionRequestController)
  - 9 idiomas de localização adicionam novas chaves
  - 35 ficheiros alterados, 28080 linhas adicionadas, 336 linhas removidas

### 2026-05-02

#### Melhorias do Cliente de IA
- `c16f99f` - Actualização de clientes de IA, Web UI e componentes de armazenamento
  - Cliente Alibaba Cloud DashScopeClient grandemente melhorado
  - Optimização do auto-compactador SpeedyPackAutoCompactor
  - Melhoria da classe base de vistas Web e BeingView
  - 6 ficheiros alterados, 240 linhas adicionadas, 81 linhas removidas

#### Sistema de Plugins
- `242dc98` - Adicionar lista de plugins na página sobre
  - AboutController adiciona exibição de informações de plugins
  - AboutViewModel adiciona modelo de dados de plugins
  - AboutView adiciona renderização da lista de plugins
  - 9 idiomas de localização adicionam chaves relacionadas com plugins
  - 14 ficheiros alterados, 160 linhas adicionadas, 1 linha removida

#### Optimização AI
- `147f8f4` - Simplificação do texto do prompt de memória de contexto
  - ContextManager optimiza prompts de IA
  - 1 ficheiro alterado, 1 linha adicionada, 1 linha removida

#### Optimização do Armazenamento Speedy
- `8bda2d3` - Actualização do armazenamento Speedy e implementação do controlador de memória
  - Correcção do intervalo do SpeedyPackAutoCompactor
  - Optimização do processamento de caminhos do SpeedyTimeStorage
  - Melhoria do controlador de memória MemoryController
  - Actualização da UI do SpeedyPack.Manager
  - 4 ficheiros alterados, 21 linhas adicionadas, 18 linhas removidas

#### Melhorias da Bandeja
- `8972654` - Melhoria do suporte de localização da janela de estado da bandeja
  - 9 idiomas de localização da bandeja adicionam entrada de gestão Speedy
  - TrayStatusWindow adiciona item de menu de gestão Speedy
  - 11 ficheiros alterados, 72 linhas adicionadas

#### Optimização do Speedy.Manager
- `6f5db09` - Optimização da UI do gestor SpeedyPack e componentes internos
  - Refactorização da interface MainForm
  - Optimização da gestão de memória FreeList
  - Melhoria da fila de escrita WriteQueue
  - Optimização do núcleo do SpeedyPack
  - 5 ficheiros alterados, 96 linhas adicionadas, 88 linhas removidas

#### Melhorias do Sistema de Armazenamento
- `57f9d5d` - Melhoria do sistema de armazenamento, adição de compactação automática e suporte a datas incompletas
  - Novo temporizador de compactação automática SpeedyPackAutoCompactor (intervalo de 30 minutos)
  - Gestor singleton SpeedyPackRegistry melhorado
  - SpeedyStorage, SpeedyTimeStorage, SpeedyWorkNoteStorage adaptados e melhorados
  - SpeedyPack adiciona gestão de espaço livre FreeList (149 linhas)
  - Escritor PackFileWriter refactorado e optimizado
  - WriteOperation, WriteQueue da fila de escrita melhorados
  - Opções de configuração SpeedyPackOptions expandidas
  - IncompleteDate adiciona métodos de comparação
  - Carregador de plugins PluginLoader melhorado
  - Fluxo de inicialização do Program.cs das versões Default e Fast actualizado
  - Dados de configuração DefaultConfigData simplificados
  - Rede de conhecimento KnowledgeNetwork simplificada
  - Controladores ChatController, MemoryController optimizados
  - Funcionalidade do MainForm do SpeedyPack.Manager melhorada
  - 22 ficheiros alterados, 639 linhas adicionadas, 253 linhas removidas

#### Actualização do Speedy.Manager
- `b04ed33` - Actualização dos ficheiros do Speedy.Manager

### 2026-05-01

#### Refactorização Arquitectónica: Armazenamento Speedy Substitui LiteDB
- `6600972` - Substituição do LiteDB pelo armazenamento Speedy, adição do sistema de plugins e projecto Speedy
  - **Novo projecto SiliconLife.Speedy**: Motor de armazenamento .spk de alto desempenho
    - Classe central SpeedyPack (489 linhas): mapeamento de directórios em memória + cache de entradas + fila de escrita assíncrona
    - Classe de configuração SpeedyPackOptions: TTL do cache, máximo de entradas em cache, modo só de leitura
    - Interface de transacções IPackTransaction: suporte a operações de escrita atómica
    - Classe de informações de ficheiro SpkFileInfo
    - Directório Internal: DirectoryMap, EntryCache, PackFileReader, PackFileWriter, WriteQueue, WriteOperation, SpeedyTransaction, SpkHeader, PathNormalizer, FreeList
    - Dependência de MessagePack 3.1.4 para serialização binária (compressão LZ4)
  - **Novo projecto SiliconLife.Speedy.Manager**: Ferramenta de gestão WPF
    - Arquitectura MVVM: MainViewModel, DirectoryTreeViewModel, ContentViewerViewModel, etc.
    - Camada de serviços: PackService, FileDialogService, RecentFilesService, NotificationService
    - Conversores: BoolToVisibility, ByteSizeToString, ContentTypeToIcon, NullToCollapsed
    - Vistas: MainWindow, DirectoryTreeView, ContentViewerPanel, MetadataPanel
    - Diálogos: FileInfoDialog, ImportDialog, NewEntryDialog
  - **Migração de armazenamento SiliconLife.Fast**: LiteDB → SpeedyPack
    - Novo SpeedyStorage (adaptador IStorage)
    - Novo SpeedyTimeStorage (adaptador ITimeStorage)
    - Novo SpeedyWorkNoteStorage (adaptador IWorkNoteStorage)
    - Novo SpeedyPackRegistry (gestão singleton a nível de processo)
    - Novo SpeedyPackAutoCompactor (temporizador de compactação automática)
    - Removidas implementações de armazenamento LiteDB (LiteDBStorage, LiteDBTimeStorage, LiteDBWorkNoteStorage, LiteDBLoggerProvider, LiteDBManager, LiteDBModels)
    - Removido código relacionado com a janela de gestão LiteDB
  - **Sistema de Plugins**:
    - Nova interface IPlugin (Core/Plugins/IPlugin.cs)
    - Novo carregador de plugins PluginLoader (Core/Plugins/PluginLoader.cs)
    - Suporte a carregamento de DLLs de plugins a partir de directórios
    - Verificação de segurança: verificação de namespaces proibidos (System.IO, System.Net, Microsoft.CodeAnalysis, etc.)
    - Lista branca de assemblies fiáveis (Google.Protobuf, Newtonsoft.Json, MessagePack, etc.)
    - Carregamento isolado com AssemblyLoadContext personalizado
    - ToolManager adiciona método ScanAllPluginAssemblies
    - CoreHost integra carregador de plugins
  - 119 ficheiros alterados, 6926 linhas adicionadas, 3066 linhas removidas

#### Melhorias dos Silicon Beings
- `3aef4c3` - Adicionar estado de actividade Stopped e melhorias no tratamento de erros
  - Silicon Beings adicionam estado Stopped
  - Tratamento de erros e mecanismo de recuperação melhorados

#### Actualização de Localização
- `513c65d` - Actualização de todas as versões linguísticas e documentação
  - Novo componente MarkdownEditorComponent (625 linhas)
  - Novo componente DetailsComponent (130 linhas)
  - Novo componente AccordionComponent (285 linhas)
  - Controladores BeingController, ChatController, MemoryController, PermissionController actualizados
  - Vistas BeingView, ChatView, MemoryView, SoulEditorView refactoradas
  - Removido antigo MarkdownEditorView
  - Migração de componentes do InitController
  - 115 ficheiros alterados, 5761 linhas adicionadas, 2362 linhas removidas

### 2026-04-30

#### Funcionalidade da Bandeja do Sistema
- `101b203` - Implementação da janela de estado da bandeja e ApplicationContext
  - Novos recursos de ícone da bandeja (alpha.png, noWord.png, slc.ico, wordIcon.png)
  - Implementação da janela de estado TrayStatusWindow
  - Suporte a localização da bandeja em 9 idiomas (TrayCsCZ, TrayDeDE, TrayEnUS, etc.)
  - Classe base abstracta TrayLocalizationBase
  - 24 ficheiros alterados, 27995 linhas adicionadas, 1 linha removida (incluindo ficheiros de recursos)

#### Arquitectura UI Componentizada
- `e61cfaa` - Conclusão da arquitectura UI componentizada, implementação de 24 componentes
  - Fase MVP (8): ComponentBase, Div, Span, Button, Input, Form, Select, Label
  - Segunda fase (6): Accordion, Card, Tabs, Table, Modal, Message
  - Terceira fase (5): Calendar, Tree, Chart, FileUpload, RichText
  - Novas classes auxiliares Js, Behavior, DomUpdate, etc.
  - 25 ficheiros alterados, 2666 linhas adicionadas

- `7449e51` - Melhoria do sistema de componentes e adição de novos temas de skin
  - Componentes A, Button, Div, Form, Input, etc. melhorados
  - Adicionados 3 temas de skin: HighContrast (alto contraste), Light (claro), Minimal (minimalista)
  - Actualização das skins existentes (Admin, Chat, Creative, Dev)
  - Migração de componentes do InitController
  - 32 ficheiros alterados, 1466 linhas adicionadas, 1238 linhas removidas

- `1ba8636` - Início da migração de componentes do InitController (em curso)
  - 9 ficheiros alterados, 574 linhas adicionadas, 145 linhas removidas

#### Unificação do Sistema de Armazenamento
- `895dff9` - Unificação do soul.md e state.json para usar a interface IStorage
  - DefaultSiliconBeing usa IStorage para ler/escrever ficheiros da alma e estado
  - Novo gestor de ficheiros de estado StateFileManager
  - SoulFileManager refactorado e adaptado ao IStorage
  - 8 ficheiros alterados, 201 linhas adicionadas, 116 linhas removidas

#### Melhorias da Gestão LiteDB
- `a34bef4` - Adicionar LiteDBManager e melhorar localização da bandeja
  - Menu da bandeja adiciona entrada de gestão LiteDB
  - 9 idiomas de localização da bandeja actualizados
  - 10 ficheiros alterados, 196 linhas adicionadas

- `c4a79ca` - Adicionar fábrica de localização sensível ao idioma para a janela de gestão LiteDB
  - 1 ficheiro alterado, 78 linhas adicionadas

- `5ebc55e` - Converter LiteDBAdminLocalization em classe base abstracta
  - 10 ficheiros alterados, 1356 linhas adicionadas

#### Correcção do Sistema de Configuração
- `2da5256` - Adicionar método abstracto ConfigExists e corrigir registos duplicados de configuração LiteDB
  - ConfigDataBase adiciona método ConfigExists
  - DefaultConfigData da versão Fast implementa verificação de existência de configuração LiteDB
  - Corrigido problema de chaves de configuração duplicadas no LiteDB
  - 9 ficheiros alterados, 210 linhas adicionadas, 2 linhas removidas

#### Optimização de Chat e Vistas
- `d3618ec` - Optimização de sessões de chat, sistema de armazenamento, modelo temporal e classe base de vistas
  - Optimização de BroadcastChannel, GroupChatSession, SingleChatSession
  - ITimeStorage adiciona métodos de consulta
  - FileSystemStorage e LiteDBStorage sincronizados e actualizados
  - ViewBase refactorado e optimizado (versões Default e Fast)
  - 11 ficheiros alterados, 622 linhas adicionadas, 392 linhas removidas

### 2026-04-29

#### Refactorização Arquitectónica: Extracção de Módulos Partilhados
- `a102428` - Migração de módulos partilhados de SiliconLife.Default para SiliconLife.Common
  - Extracção de 32 implementações de calendário para o projecto Common
  - Extracção da classe base de localização e 21 implementações de idiomas para o projecto Common
  - Extracção do gestor de permissões e implementação padrão do Silicon Being para o projecto Common
  - Extracção de 23 implementações de ferramentas incorporadas para o projecto Common
  - Extracção da implementação Playwright WebView para o projecto Common
  - Actualização dos namespaces para SiliconLife.Collective
  - 122 ficheiros alterados, 586 linhas adicionadas, 343 linhas removidas

#### Melhorias de Qualidade do Código
- `17566fe` - Substituição de Console.WriteLine pelo sistema de registos nos projectos Core, Common e Default
  - 6 ficheiros actualizados incluindo ContextManager, AuditLogger, DefaultConfigData, etc.
  - Unificação do uso da interface ILogger, melhoria da manutenibilidade do código
  - 6 ficheiros alterados, 12 linhas adicionadas, 8 linhas removidas

#### Versão de Alto Desempenho SiliconLife.Fast
- `54a0307` - Adicionar projecto SiliconLife.Fast e completar correcções de compilação
  - Ponto de entrada completo da aplicação Windows Forms
  - Suporte à bandeja do sistema (NotifyIcon)
  - Migração de todos os controladores Web UI (20+)
  - Migração de todos os componentes de vistas Web
  - Migração de 4 temas de skin (Admin, Chat, Creative, Dev)
  - 125 ficheiros alterados, 61186 linhas adicionadas

#### Sincronização de Documentação Multilingue
- `265fde8` - Sincronização da documentação de arquitectura de duas versões para todos os idiomas
  - Actualização de architecture.md, changelog.md em 7 idiomas
  - Actualização de contributing.md em 6 idiomas
  - Actualização de getting-started.md, roadmap.md em 7 idiomas
  - 47 ficheiros alterados, 1214 linhas adicionadas, 38 linhas removidas

#### Sistema de Armazenamento LiteDB (Versão Fast)
- `4704862` - Adicionar dependências e infraestrutura LiteDB
  - Nova classe de gestão LiteDBManager
  - Novos modelos de dados LiteDBModels
  - 3 ficheiros alterados, 252 linhas adicionadas

- `4220036` - Implementação de classes de armazenamento LiteDB
  - LiteDBStorage: implementação da interface IStorage
  - LiteDBTimeStorage: implementação da interface ITimeStorage
  - LiteDBWorkNoteStorage: implementação da interface IWorkNoteStorage
  - 3 ficheiros alterados, 581 linhas adicionadas

- `38ebd23` - Migração do sistema de configuração e registos para LiteDB
  - DefaultConfigData adaptado ao armazenamento LiteDB
  - Novo fornecedor de registos LiteDBLoggerProvider
  - 2 ficheiros alterados, 203 linhas adicionadas, 67 linhas removidas

- `e687157` - Migração da rede de conhecimento do sistema de ficheiros para LiteDB
  - KnowledgeNetwork completamente refactorado, usando armazenamento LiteDB para dados de triplas
  - 1 ficheiro alterado, 231 linhas adicionadas, 72 linhas removidas

- `4220169` - Integração do armazenamento LiteDB no Program e ProjectManager
  - Program.cs inicializa armazenamento LiteDB
  - ProjectManager adaptado ao armazenamento de notas de trabalho LiteDB
  - 2 ficheiros alterados, 40 linhas adicionadas, 17 linhas removidas

- `5f3a709` - Remoção de implementações de armazenamento em sistema de ficheiros obsoletas
  - Eliminação de FileSystemLoggerProvider, FileSystemStorage, FileSystemTimeStorage, etc.
  - 6 ficheiros alterados, 1518 linhas removidas

- `e1a4ef2` - docs: adicionar identificador de versão v0.1.0-alpha a toda a documentação
  - 127 ficheiros alterados, 2297 linhas adicionadas, 2471 linhas removidas

## [v0.1.0-alpha] - 2026-04-28

### 2026-04-28

#### Refactorização do Sistema de Armazenamento
- `8dd26e3` - Unificação da interface ITimeStorage usando IncompleteDate e adição de API de consulta hierárquica
  - Remoção de sobrecargas DateTime na interface ITimeStorage, unificação usando IncompleteDate
  - IncompleteDate adiciona método de comparação CompareTo(DateTime) e método de expansão Expand()
  - Novas APIs de consulta hierárquica GetEarliestTimestamp(), GetLatestTimestamp()
  - Novos métodos HasSummary() e QueryWithLevel(), suportando consulta por nível temporal
  - Memory.cs refactora algoritmo de compressão, usando nova API de consulta hierárquica para melhorar eficiência
  - FileSystemTimeStorage.cs implementação completa dos novos métodos de interface
  - Sincronização de todos os chamadores: ChatSystem, ChatSession, BroadcastChannel, AuditLogger, TokenUsageRecord, etc.
  - Sistema de ferramentas actualizado: HelpTool, LogTool, TokenAuditTool adaptados à nova interface
  - Controladores Web actualizados: AuditController, ChatController, ChatHistoryController adaptados à nova interface
  - 41 ficheiros alterados, 1820 linhas adicionadas, 903 linhas removidas

### 2026-04-27

#### Melhorias do Sistema de Documentação de Ajuda
- `9989d79` - Actualização de localização, sistema de ajuda e vistas Web
  - Novo interface de documentação de ajuda de fábrica de clientes de IA IAIClientFactoryHelp.cs
  - Conclusão da tradução de toda a documentação de ajuda em 9 idiomas
  - HelpTopics.cs adiciona 40 definições de tópicos de ajuda
  - Vistas Web completamente actualizadas: InitController, AuditView, ConfigView, KnowledgeView, LogView, etc.
  - Sistema de localização melhorado: todas as versões linguísticas adicionam novas chaves de localização
  - Fábricas de clientes de IA actualizadas: DashScopeClientFactory, OllamaClientFactory melhoradas
  - 30 ficheiros alterados, 10086 linhas adicionadas, 15 linhas removidas

#### Novo Conteúdo da Documentação de Ajuda
- `e7afe94` - Adicionar documentação de ajuda do Ficheiro da Alma e registos de auditoria
  - Nova documentação de ajuda da gestão do Ficheiro da Alma
  - Nova documentação de ajuda dos registos de auditoria
  - HelpTopics.cs adiciona definições de tópicos
  - HelpView.cs grandemente refactorado, melhoria da lógica de renderização de documentação
  - PermissionView.cs refactorado, melhoria da interface de gestão de permissões
  - Módulos centrais melhorados: SiliconBeingManager, TaskSystem, ToolManager melhorados
  - TaskTool.cs refactorado, melhoria da funcionalidade de gestão de tarefas
  - Vistas Web completamente actualizadas: todos os componentes de vista sincronizados
  - HelpController.cs simplificado, optimização da lógica do controlador
  - 30 ficheiros alterados, 7100 linhas adicionadas, 897 linhas removidas

### 2026-04-26

#### Sistema de Documentação de Ajuda
- `07895d7` - Melhoria do sistema de documentação de ajuda, adição de 3 documentos e conclusão da tradução em 9 idiomas
  - Adicionados sistema de memória, instalação e configuração do Ollama, guia de uso da plataforma Alibaba Cloud Bailian
  - Conclusão da tradução de todos os 10 documentos de ajuda em 9 idiomas
  - Simplificação da lógica de renderização do HelpView
  - 18 ficheiros alterados, 14418 linhas adicionadas, 1364 linhas removidas

#### Localização Alemã
- `0cfd8a1` - Adicionar suporte completo de localização alemã (de-DE)
  - Ficheiro completo de localização alemã
  - Novo suporte ao calendário histórico chinês em alemão
  - Nova tradução de documentação de ajuda em alemão
  - Sincronização completa de toda a documentação em 9 idiomas
  - 135 ficheiros alterados, 26186 linhas adicionadas, 14371 linhas removidas

#### Sincronização da Documentação
- `3aada7d` - Sincronização da documentação em chinês tradicional (zh-HK) com chinês simplificado
  - 3 ficheiros alterados, 519 linhas adicionadas, 422 linhas removidas
- `2f6abff` - Adicionar localização do nome de exibição da ferramenta de ajuda para todos os idiomas
  - 7 ficheiros alterados, 47 linhas adicionadas, 7 linhas removidas

#### Refactorização do Sistema de Conhecimento
- `60944fe` - Unificação do namespace para SiliconLife.Collective
  - 8 ficheiros alterados, 5 linhas adicionadas, 8 linhas removidas
- `69c51c5` - Adicionar sistema de documentação de ajuda e traduzir comentários de código para inglês
  - 29 ficheiros alterados, 3385 linhas adicionadas, 22 linhas removidas

### 2026-04-25

#### Automação do Navegador WebView
- `41757c3` - Implementação de automação de navegador WebView multiplataforma baseada em Playwright
  - 6 ficheiros alterados, 1152 linhas adicionadas

#### Actualização da Documentação
- `0ff797b` - Adicionar documentação do KnowledgeTool e WorkNoteTool (7 idiomas)
  - 28 ficheiros alterados, 4983 linhas adicionadas
- `ad77415` - Actualizar todos os ficheiros changelog, adicionar registos do histórico Git de 2026-04-25
  - 7 ficheiros alterados, 168 linhas adicionadas

#### Gestão do Espaço de Trabalho de Projecto
- `785c551` - Implementação da gestão do espaço de trabalho de projecto, incluindo notas de trabalho e sistema de tarefas
  - Novo sistema de gestão do espaço de trabalho de projecto
  - Funcionalidade de notas de trabalho para acompanhar o progresso do projecto
  - Integração do sistema de gestão de tarefas
  - 29 ficheiros alterados, 4256 linhas adicionadas, 36 linhas removidas

#### Localização Checa
- `b4bbf39` - Adicionar localização checa completa (cs-CZ) e actualizar documentação em todos os idiomas
  - 116 ficheiros alterados, 4933 linhas adicionadas, 222 linhas removidas
- `faf078f` - Corrigir erro de compilação da localização checa
  - 3 ficheiros alterados, 910 linhas adicionadas, 1 linha removida

#### Melhorias do Sistema de Conhecimento
- `20adaac` - Adicionar KnowledgeTool e suportar localização completa
  - 34 ficheiros alterados, 2331 linhas adicionadas, 56 linhas removidas

### 2026-04-24

#### Melhorias do Sistema de Gestão de Memória
- `c7b2ecc` - Melhoria da gestão de memória, adição de filtragem avançada, estatísticas e vista de detalhes
  - Nova funcionalidade de filtragem avançada de memória
  - Implementação de funcionalidade de estatísticas de memória
  - Adição de página de vista de detalhes de memória
  - Suporte de localização multilingue (6 idiomas)
  - 13 ficheiros alterados, 840 linhas adicionadas, 86 linhas removidas

#### Extensão do Sistema de Permissões
- `4489ad6` - Adicionar serviço meteorológico wttr.in à lista branca de rede
  - Actualização de sincronização completa da documentação multilingue (6 idiomas)
  - 14 ficheiros alterados, 417 linhas adicionadas, 1 linha removida

#### Correcções da Interface Web
- `d9d72e9` - Corrigir problema de prioridade CSS do modal de detalhes das notas de trabalho
  - 19 ficheiros alterados, 1744 linhas adicionadas, 6 linhas removidas

#### Optimização do Histórico de Chat
- `0df599c` - Corrigir problema de resultados de ferramentas renderizados como mensagens de chat independentes
  - 1 ficheiro alterado, 222 linhas adicionadas, 21 linhas removidas
- `057b09d` - Optimização da exibição de detalhes do histórico de chat, melhoria da renderização de chamadas de ferramentas
  - 3 ficheiros alterados, 389 linhas adicionadas, 68 linhas removidas

#### Histórico de Execução de Temporizadores
- `fa3f06f` - Adicionar funcionalidade de histórico de execução de temporizadores, incluindo vista de detalhes
  - 8 ficheiros alterados, 937 linhas adicionadas, 10 linhas removidas
- `d824835` - Adicionar chaves de localização do histórico de execução de temporizadores (todos os idiomas)
  - 7 ficheiros alterados, 88 linhas adicionadas

#### Melhorias de Localização
- `c13cb17` - Registar variante de idioma espanhol
  - 1 ficheiro alterado, 4 linhas adicionadas
- `9c44f34` - Adicionar suporte de localização multilingue para calendário histórico chinês
  - 16 ficheiros alterados, 6049 linhas adicionadas, 1 linha removida

#### Melhorias de Funcionalidades Centrais
- `1e7c7b2` - Melhoria da compressão de memória e rastreamento de execução de ferramentas
  - 4 ficheiros alterados, 338 linhas adicionadas, 86 linhas removidas

### 2026-04-23

#### Localização de Ferramentas
- `192fc6e` - Adicionar nomes de ferramentas em falta na localização de 5 ferramentas
  - 6 ficheiros alterados, 30 linhas adicionadas

#### Actualização da Documentação
- `882c08f` - Actualizar todos os ficheiros changelog, adicionar histórico Git completo e remover números de versão falsos
  - 45 ficheiros alterados, 8815 linhas adicionadas, 1611 linhas removidas

#### Melhorias da Página de Chat
- `65c157b` - Adicionar indicador de carregamento à página de chat e selecção automática da sessão do Curator
  - 10 ficheiros alterados, 211 linhas adicionadas, 7 linhas removidas

#### Funcionalidade de Histórico de Chat
- `e483348` - Implementar funcionalidade de visualização do histórico de chat dos Silicon Beings
  - Novo ChatHistoryController
  - Criação do ChatHistoryViewModel
  - Implementação das páginas ChatHistoryListView e ChatHistoryDetailView
  - Adição de chaves de localização do histórico de chat (5 idiomas)
  - 12 ficheiros alterados, 1178 linhas adicionadas

#### Melhoria do Controlo de Fluxo AI
- `30a2d4e` - Melhoria do cancelamento de fluxo AI, integração IM e inicialização do core host
  - 11 ficheiros alterados, 387 linhas adicionadas, 12 linhas removidas

#### Fila de Mensagens de Chat
- `db48c51` - Adicionar fila de mensagens de chat, metadados de ficheiros e suporte a cancelamento de fluxo
  - 4 ficheiros alterados, 357 linhas adicionadas

#### Suporte a Carregamento de Ficheiros
- `28fb344` - Implementar diálogo de origem de ficheiros e suporte a carregamento de ficheiros
  - 3 ficheiros alterados, 1100 linhas adicionadas, 2 linhas removidas
- `1d3e2cc` - Adicionar strings de localização do diálogo de origem de ficheiros (6 idiomas)
  - 6 ficheiros alterados, 30 linhas adicionadas

#### Actualização da Documentação
- `8111e92` - Adicionar ligação Wiki na secção de repositório do README
  - 1 ficheiro alterado, 3 linhas adicionadas, 1 linha removida

### 2026-04-22

#### Localização da Documentação
- `66c11eb` - Tradução de comentários chineses para inglês e actualização de todos os changelogs
  - 11 ficheiros alterados, 373 linhas adicionadas, 163 linhas removidas

#### Melhoria de Mensagens SSE
- `b574b2b` - Adicionar senderName a mensagens históricas para identificação AI
  - 1 ficheiro alterado, 9 linhas adicionadas

#### Funcionalidade de Chat
- `601fc14` - Adicionar operação mark_read para marcação de fim de sessão
  - 7 ficheiros alterados, 196 linhas adicionadas, 36 linhas removidas

#### Optimização do Sistema de Ferramentas
- `7a03a19` - Melhoria da flexibilidade de consulta de conversações do LogTool
  - 1 ficheiro alterado, 57 linhas adicionadas, 24 linhas removidas

#### Melhorias de Localização
- `0a8d750` - Adicionar prompt de sistema genérico para comportamento proactivo dos Silicon Beings
  - 8 ficheiros alterados, 460 linhas adicionadas, 48 linhas removidas

#### Refactorização do Sistema de Registos
- `2b771f3` - Desacoplamento do LogController do I/O de ficheiros, adição de API de leitura de registos
  - 4 ficheiros alterados, 172 linhas adicionadas, 137 linhas removidas
- `12da302` - Adicionar filtro de Silicon Beings à vista de registos
  - 9 ficheiros alterados, 147 linhas adicionadas, 10 linhas removidas
- `8f6cb1e` - Adicionar parâmetro beingId à interface ILogger, realizando separação de registos sistema/Silicon Beings
  - 47 ficheiros alterados, 524 linhas adicionadas, 490 linhas removidas

#### Melhorias do Sistema de Permissões
- `4c747ad` - Refactorização do PermissionTool, ExecuteCodeTool, adição da API EvaluatePermission
  - 18 ficheiros alterados, 680 linhas adicionadas, 492 linhas removidas

#### Correcções de Bugs
- `1c96e99` - Corrigir falha na pesquisa search_files e search_content no directório raiz
  - 1 ficheiro alterado, 98 linhas adicionadas, 41 linhas removidas

#### Integração de Ferramentas
- `135710d` - Remover SearchTool, mover pesquisa local para DiskTool
  - 2 ficheiros alterados, 185 linhas adicionadas, 365 linhas removidas

#### Extensão do Sistema de Ferramentas
- `70ce7fb` - Implementar DatabaseTool para consultas a base de dados estruturada
  - 1 ficheiro alterado, 382 linhas adicionadas
- `be29a09` - Implementar LogTool para consulta de histórico de operações e conversações
  - 1 ficheiro alterado, 298 linhas adicionadas
- `4ea7702` - Implementar PermissionTool para gestão dinâmica de permissões
  - 1 ficheiro alterado, 457 linhas adicionadas
- `1384ff4` - Implementar ExecuteCodeTool para execução de código em múltiplas linguagens
  - 1 ficheiro alterado, 477 linhas adicionadas
- `82d1e11` - Implementar SearchTool para pesquisa de informação
  - 1 ficheiro alterado, 363 linhas adicionadas

#### Optimização da Interface Web
- `0675c45` - Optimização do destaque de blocos de código markdown no painel de pré-visualização
  - 1 ficheiro alterado, 4 linhas adicionadas, 23 linhas removidas
- `702b3f3` - Melhoria da vista de tarefas, adição de badges de estado e exibição de metadados
  - 8 ficheiros alterados, 221 linhas adicionadas, 9 linhas removidas
- `6ed9a79` - Melhoria do armazenamento de mensagens de chat e renderização de vistas
  - 8 ficheiros alterados, 140 linhas adicionadas, 29 linhas removidas

### 2026-04-21

#### Correcções de Bugs
- `c6b518b` - Corrigir passagem de mensagens de temporizadores e armazenamento de mensagens de chat
  - 3 ficheiros alterados, 297 linhas adicionadas, 124 linhas removidas

#### Gestão de Configuração
- `4305769` - Adicionar .gitattributes para gestão de finais de linha
  - 1 ficheiro alterado, 32 linhas adicionadas

#### Melhorias da Interface Web
- `188c6f8` - Registar rota de API da lista de tarefas e adicionar exibição de estado vazio
  - 2 ficheiros alterados, 35 linhas adicionadas, 2 linhas removidas
- `634e8ca` - Adicionar ligação de retorno à lista na página de permissões
  - 1 ficheiro alterado, 16 linhas adicionadas
- `6ba591d` - Adicionar editor de configuração AI independente para Silicon Beings
  - 11 ficheiros alterados, 842 linhas adicionadas, 18 linhas removidas
- `0a826f5` - Adicionar notificação de gravação com sucesso no editor de código
  - 1 ficheiro alterado, 9 linhas adicionadas, 2 linhas removidas
- `2940373` - Melhoria da interface Web, adição de dicas flutuantes de código e melhorias de UI
  - 11 ficheiros alterados, 1054 linhas adicionadas, 75 linhas removidas

#### Correcção do Sistema de Permissões
- `592c7ab` - Corrigir instanciação de callback e ordem de registo
  - 2 ficheiros alterados, 38 linhas adicionadas, 7 linhas removidas

#### Melhorias de Segurança
- `833ead2` - Adicionar validação de referências de assembly para compilação dinâmica
  - 4 ficheiros alterados, 135 linhas adicionadas, 8 linhas removidas

#### Melhorias do Sistema de Permissões
- `5879621` - Adicionar validação de pré-compilação de callback de permissões e melhoria do tratamento de erros
  - 21 ficheiros alterados, 617 linhas adicionadas, 26 linhas removidas

#### Actualização da Documentação
- `4dbf659` - Actualizar changelog para v0.5.1, substituir URLs placeholder do GitHub, adicionar espelho Gitee, localizar nome Bilibili por idioma, actualizar email
  - 32 ficheiros alterados, 489 linhas adicionadas, 180 linhas removidas

#### Configuração e Entrada
- `0fc1693` - Actualizar entrada do programa e configuração do projecto
  - 2 ficheiros alterados, 7 linhas adicionadas

#### Refactorização do Sistema de Permissões
- `ea9179a` - Melhoria da implementação do sistema de permissões
  - 5 ficheiros alterados, 358 linhas adicionadas, 152 linhas removidas

#### Correcções de Bugs
- `928a96d` - Corrigir implementação de cálculo do calendário
  - 4 ficheiros alterados, 12 linhas adicionadas, 12 linhas removidas

#### IA e Calendário
- `646813e` - Melhoria da implementação da fábrica de clientes de IA
  - 2 ficheiros alterados, 21 linhas adicionadas, 20 linhas removidas

#### Localização
- `7940d9c` - Adicionar suporte de localização coreana
  - 7 ficheiros alterados, 2424 linhas adicionadas, 10 linhas removidas
- `4ff98ad` - Refactorização da documentação, suporte a multilingue
  - 81 ficheiros alterados, 23818 linhas adicionadas, 1886 linhas removidas

### 2026-04-20

#### Conclusão de Funcionalidades Centrais
- `28905b5` - Suporte multilingue completo, fábrica de clientes de IA, sistema de permissões e configuração de localização
  - Sistema de registos com gestor, entradas e diferentes níveis de registo
  - Sistema de auditoria de tokens para consulta e rastreamento de uso de tokens
  - Fábrica de clientes de IA com descoberta automática de diferentes plataformas de IA
  - Sistema de callback de permissões com armazenamento próprio
  - Implementação de logger de consola
  - Suporte multilingue em inglês e chinês simplificado
  - Mensageiro WebUI com WebSocket para chat em tempo real
  - Melhoria do Silicon Being padrão com localização
  - 39 ficheiros alterados, 4670 linhas adicionadas, 175 linhas removidas

### 2026-04-19

#### Temporizadores e Calendário
- `c933fd8` - Actualização de localização, sistema de temporizadores, vistas Web e adição de ferramentas
  - Melhor gestor de localização
  - Sistema de agendamento de tarefas temporizadas
  - Configuração de IA e gestão de contexto
  - Ferramenta de calendário suportando 32 tipos de calendário
  - Controlador Web para API de calendário
  - Ferramenta de gestão de tarefas
  - 46 ficheiros alterados, 4018 linhas adicionadas, 975 linhas removidas

**Melhorias arquitectónicas**
- Redesenho da arquitectura de vistas Web para melhor suporte de skins
- Melhoria do sistema de gestão de beings com melhor tratamento de estado

### 2026-04-18

- `9f585e1` - Actualização de localização, sistema de temporizadores, vistas Web e adição de ferramentas
  - Melhorias em temporizadores e agendamento
  - Melhores vistas Web com componentes UI melhorados
  - Mais implementações de ferramentas
  - 57 ficheiros alterados, 3328 linhas adicionadas, 389 linhas removidas

### 2026-04-17

- `9b71fcd` - Actualização de módulos centrais, adição de documentação zh-HK, canal de difusão, ferramenta de configuração e vista Web de auditoria
  - Canal de difusão para múltiplos Silicon Beings conversarem juntos
  - Sistema de ferramenta de configuração
  - Vista Web de auditoria
  - Documentação em chinês tradicional
  - 42 ficheiros alterados, 3533 linhas adicionadas, 268 linhas removidas

### 2026-04-16

- `5040f05` - Actualização dos módulos core e default
  - Optimização de módulos e correcções de bugs
  - Actualizações e melhorias de implementação
  - 58 ficheiros alterados, 9916 linhas adicionadas, 111 linhas removidas

### 2026-04-15

- `3efab5f` - Actualização de múltiplos módulos: IA, Chat, IM, Ferramentas, Web, Localização, Armazenamento
  - Melhorias do cliente de IA
  - Melhorias do sistema de chat
  - Actualização do fornecedor de mensageiro
  - Optimização do sistema de ferramentas
  - Melhorias da infraestrutura Web
  - Optimização da localização
  - Actualização do sistema de armazenamento
  - 33 ficheiros alterados, 788 linhas adicionadas, 232 linhas removidas

### 2026-04-14

- `4241a2f` - Funcionalidade de chat basicamente concluída, optimização de carregamento de UI
  - Funcionalidade do sistema de chat concluída
  - Optimização de UI para carregamento de ficheiros
  - 16 ficheiros alterados, 1234 linhas adicionadas, 102 linhas removidas

### 2026-04-13

- `c498c31` - Actualização de código
  - Melhorias e optimizações gerais de código
  - 32 ficheiros alterados, 1045 linhas adicionadas, 546 linhas removidas

### 2026-04-12

#### Documentação e Localização
- `2161002` - Refactorização da documentação e melhoria da localização
  - 17 ficheiros alterados, 982 linhas adicionadas, 92 linhas removidas
- `03d94e4` - Melhoria do sistema de configuração e localização
  - 25 ficheiros alterados, 1378 linhas adicionadas, 154 linhas removidas
- `9976a35` - Adicionar página sobre e localização
  - 14 ficheiros alterados, 699 linhas adicionadas, 44 linhas removidas

#### Chat e Vistas Web
- `0c8ccfc` - Melhoria do sistema de chat, localização e vistas Web
  - 13 ficheiros alterados, 402 linhas adicionadas, 56 linhas removidas
- `a8f1342` - Redesenho da camada de comunicação Web, mudança de WebSocket para SSE
  - 27 ficheiros alterados, 793 linhas adicionadas, 935 linhas removidas

### 2026-04-11

#### Sistema de Registos
- `e8fe259` - Adicionar sistema de registos e optimização de código
  - 37 ficheiros alterados, 624 linhas adicionadas, 91 linhas removidas
- `f01c519` - Adicionar sistema de registos, actualizar interface de IA e vistas Web
  - 31 ficheiros alterados, 1758 linhas adicionadas, 63 linhas removidas

### 2026-04-10

- `4962924` - Melhoria do handler WebSocket, vista de chat e interacção do mensageiro
  - Melhorias do ContextManager
  - Melhorias do sistema de chat
  - Actualização da interface do fornecedor de mensageiro
  - Redesenho do fornecedor WebUI
  - Actualização do construtor JavaScript e router
  - Optimização da vista de chat
  - Melhoria do handler WebSocket
  - 9 ficheiros alterados, 365 linhas adicionadas, 134 linhas removidas

### 2026-04-09

- `f9302bf` - Melhoria da interface do fornecedor de mensageiro, sistema de chat e interacção Web UI
  - Expansão da interface do fornecedor de mensageiro
  - Melhorias das mensagens e sistema de chat
  - Optimização do ContextManager
  - Melhoria do Silicon Being padrão
  - Melhoria da vista de chat da Web UI
  - Actualização do handler WebSocket
  - 10 ficheiros alterados, 427 linhas adicionadas, 93 linhas removidas

### 2026-04-07

- `6831ee8` - Redesenho das vistas Web e construtor JavaScript
  - Redesenho completo dos controladores Web
  - Reescrita completa do construtor JavaScript
  - Actualização de todos os componentes de vista
  - Melhoria do sistema de skins
  - Elevação da arquitectura da classe base de vistas
  - 23 ficheiros alterados, 2004 linhas adicionadas, 1983 linhas removidas

### 2026-04-05

- `41e97fb` - Actualização de múltiplos módulos centrais e controladores Web
  - Melhorias do ContextManager
  - Sistema de chat e gestão de sessões
  - Redesenho do ServiceLocator
  - Actualização da classe base e gestor dos Silicon Beings
  - Actualização abrangente dos controladores Web (17 controladores)
  - Melhoria da fábrica DefaultSiliconBeingFactory
  - 31 ficheiros alterados, 681 linhas adicionadas, 326 linhas removidas
- `67988d4` - Melhoria dos módulos Web UI, adição de vista de executores, limpeza de vistas e módulos centrais
  - 61 ficheiros alterados, 3148 linhas adicionadas, 3726 linhas removidas

### 2026-04-04

- `b58bb1c` - Adicionar controlador de inicialização e redesenhar módulo Web
  - Controlador de inicialização
  - Redesenho do módulo de configuração
  - Actualização do módulo de localização
  - Melhoria do sistema de skins
  - Melhoria do router
  - 29 ficheiros alterados, 1269 linhas adicionadas, 289 linhas removidas
- `f03ac0b` - Adicionar módulo Web UI, melhoria da funcionalidade do mensageiro
  - 60 ficheiros alterados, 8481 linhas adicionadas, 165 linhas removidas

### 2026-04-03

- `192e57b` - Actualização da estrutura do projecto e componentes centrais de runtime
  - 22 ficheiros alterados, 446 linhas adicionadas, 179 linhas removidas
- `59faec8` - Actualização da implementação core e default
  - 25 ficheiros alterados, 3056 linhas adicionadas, 18 linhas removidas
- `d488485` - Adicionar funcionalidade de compilação dinâmica e módulo de ferramentas do Curator
  - 19 ficheiros alterados, 1727 linhas adicionadas, 11 linhas removidas
- `753d1d9` - Adicionar módulo de segurança, actualizar executores, fornecedor de mensageiro, localização e ferramentas
  - 29 ficheiros alterados, 2352 linhas adicionadas, 93 linhas removidas
- `a378697` - Conclusão da Fase 5 - Sistema de ferramentas + Executores
  - 41 ficheiros alterados, 2651 linhas adicionadas, 363 linhas removidas

### 2026-04-02

- `e6ad94b` - Corrigir falha no carregamento do histórico de chat ao eliminar ficheiro de configuração durante testes
  - 4 ficheiros alterados, 49 linhas adicionadas, 45 linhas removidas
- `daa56f5` - Conclusão da Fase 4: Memória persistente (sistema de chat + canal de mensageiro)
  - 29 ficheiros alterados, 2051 linhas adicionadas, 538 linhas removidas

### 2026-04-01

- `bbe2dbb` - Corrigir carregamento de configuração e encaminhamento de mensagens do serviço de chat
  - 27 ficheiros alterados, 1633 linhas adicionadas, 147 linhas removidas
- `2fa6305` - Implementar Fase 2: Framework do ciclo principal e sistema de objectos Tick
  - 9 ficheiros alterados, 594 linhas adicionadas, 41 linhas removidas
- `32b99a1` - Implementar Fase 1 - Funcionalidade básica de chat
  - 19 ficheiros alterados, 1185 linhas adicionadas
- `358e368` - Commit inicial: documentação do projecto e licença
  - 10 ficheiros alterados, 1873 linhas adicionadas
