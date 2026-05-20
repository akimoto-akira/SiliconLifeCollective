# Registo de alterações

[English](../en/changelog.md) | [Deutsch](../de-DE/changelog.md) | [Français](../fr-FR/changelog.md) | [中文](../zh-CN/changelog.md) | [繁體中文](../zh-HK/changelog.md) | [Español](../es-ES/changelog.md) | [日本語](../ja-JP/changelog.md) | [한국어](../ko-KR/changelog.md) | [Čeština](../cs-CZ/changelog.md) | [Italiano](../it-IT/changelog.md) | [Polski](../pl-PL/changelog.md) | **Português**

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

## Alpha-0.2

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
