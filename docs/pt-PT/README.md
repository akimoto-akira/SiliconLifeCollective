![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**Versão: v0.2.0-alpha** | **Silicon Life Collective** — Uma plataforma de colaboração multi-agente baseada em .NET 9, onde os agentes de IA são chamados **Silicon Beings**, capazes de auto-evolução através de compilação dinâmica Roslyn.

[English](../README.md) | [Deutsch](../de-DE/README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Français](../fr-FR/README.md) | [Čeština](../cs-CZ/README.md) | [Italiano](../it-IT/README.md) | [Polski](../pl-PL/README.md) | **Português** | [Русский](../ru-RU/README.md)

## 🌟 Funcionalidades Principais

### Sistema de Agentes
- **Orquestração multi-agente** — Gerida uniformemente pelo *Silicon Curator*, com mecanismo de escalonamento justo por fatias de tempo orientado a relógio
- **Orientado por Ficheiro da Alma** — Cada Silicon Being é orientado por um ficheiro de prompt central (`soul.md`) que define a personalidade única e os padrões de comportamento
- **Arquitectura Corpo-Cérebro** — O *Corpo* (SiliconBeing) mantém os sinais vitais e detecta cenários de activação; o *Cérebro* (ContextManager) é responsável por carregar o histórico, invocar a IA, executar ferramentas e persistir as respostas
- **Capacidade de auto-evolução** — Através da tecnologia de compilação dinâmica Roslyn, os Silicon Beings podem reescrever o seu próprio código para evoluir
- **Gestão de estado de actividade** — Suporta nove estados de actividade: Idle (inactivo), SingleChat (chat individual), GroupChat (chat de grupo), Task (tarefa), Timer (temporizador), Broadcast (difusão), Project (projecto), MemoryCompression (compressão de memória), Stopped (parado), com transição automática para Stopped após 10 erros consecutivos

### Sistema de Plugins
- **Arquitectura de extensão por plugins** — Extensão de funcionalidades através da interface IPlugin, com carregamento dinâmico de DLLs de plugins a partir de directórios
- **Declaração de capacidades de plugins** — Os plugins declaram as capacidades necessárias através do atributo `[PluginCapability]` (Network, FileIO, Process, AI), e o carregador com base nisso afrouxa as regras de verificação de segurança; capacidades não declaráveis (P/Invoke, Unsafe, Reflection Emit, etc.) são sempre bloqueadas
- **Carregamento isolado** — Utiliza AssemblyLoadContext personalizado para carregamento isolado, impedindo que os plugins afectem a estabilidade do programa principal
- **Integração de ferramentas** — Os plugins podem registar ferramentas personalizadas através da interface ITool, integrando-se automaticamente no ciclo de chamadas de ferramentas

### Ferramentas e Execução
- **24 ferramentas incorporadas** — Abrangem calendário, chat, configuração, disco, rede, memória, tarefas, temporizadores, base de conhecimento, notas de trabalho, espaço de trabalho de projecto, navegador WebView, etc.
- **Isolamento de cenários de ferramentas** — Cada ferramenta declara os cenários disponíveis através do atributo `ToolScenario` (Chat, Task, Timer, MemoryCompression, Project), e o atributo `ChatOnly` restringe o uso da ferramenta apenas ao cenário de chat
- **Ciclo de chamadas de ferramentas** — A IA retorna chamadas de ferramentas → execução das ferramentas → resultados alimentados de volta à IA → ciclo contínuo até retornar uma resposta em texto puro
- **Segurança Executor-Permissão** — Todas as operações de I/O passam por verificação rigorosa de permissões através de executores
  - Cadeia de verificação de permissões de 3 níveis: UserFrequencyCache → IPermissionCallback → (IsCurator: IPermissionAskHandler | Non-curator: GlobalACL → negação por defeito)
  - Registo de auditoria completo de todas as decisões de permissões

### Sistema de Competências
- **Unidades de capacidade reutilizáveis** — Encapsular "orquestração de ferramentas + modelo de prompt" em competências declaráveis, evolutivas e agendáveis; a IA chama competências como ferramentas normais
- **Modo de duplo acionamento** — Manual (decisão autónoma da IA via chamada de função) + Auto (agendamento schedule: hora fixa diária / intervalo periódico / subconjunto cron)
- **Markdown em primeiro lugar** — Metadados YAML front matter + texto do prompt; ao guardar Markdown puro, a IA completa automaticamente os metadados em falta (campos do utilizador não são sobrescritos)
- **Recarregamento a quente e arquivo de versões** — Detecção de impressão digital a cada 30 segundos com entrada em vigor automática; cada atualização é arquivada em `skills/archive/{id}/{version}.md` formando um histórico de evolução
- **Múltiplas barreiras de proteção** — Interruptor global, limite de quota (predefinição 50/ser), limites globais de rondas e timeout, lista branca de ferramentas, proteção contra recursão, permissões de ação ao nível de competência

### Integração MCP
- **Acesso a ferramentas externas** — Ligação a servidores MCP (Model Context Protocol) externos; as suas ferramentas são injetadas automaticamente em todos os Seres de Silício com o nome `mcp_{serverId}_{toolName}`, sem necessidade de escrever código
- **Transporte duplo** — stdio (subprocesso local) e http (endpoint remoto)
- **Soberania do utilizador** — Adição, remoção, arranque e paragem de servidores apenas via Web UI; a ferramenta `mcp` do lado da IA é apenas de leitura
- **Permissões consistentes** — Ferramentas MCP embrulhadas são integradas na matriz de permissões de ferramentas de dois níveis, podendo ser desativadas por ser/projeto

### Integração de Mensagens Instantâneas
- **Arquitetura multi-instância** — Permitem ligar simultaneamente múltiplas plataformas de MI (Web UI / Feishu / WeCom / DingTalk), cada instância com arranque/paragem independente, encaminhamento agregado de mensagens
- **Assistente de autorização OAuth** — Autorização com um clique do Feishu (state anti-CSRF, push de estado em tempo real via SSE), tokens escritos automaticamente na configuração
- **Segurança de chaves** — Valores de configuração suportam marcadores `${ENV_VAR}` de variáveis de ambiente, chaves em texto claro não são persistidas em disco

### IA e Conhecimento
- **Suporte para múltiplos backends de IA**
  - **Ollama** — Implantação de modelos locais, utilizando a API HTTP nativa
  - **Alibaba Cloud DashScope** — Serviço de IA na nuvem, compatível com a API OpenAI, suporta 13+ modelos, implantação em múltiplas regiões
  - **Volcengine Ark** — Serviço de IA na nuvem da ByteDance, suporta modos de streaming e não-streaming, com controlo de taxa integrado
  - **Herdsman** — Motor de inferência sem autenticação, compatível com o formato OpenAI API
  - **Meituan LongCat** — Modelo grande auto-desenvolvido da Meituan, LongCat-2.0 suporta contexto de 1M e modo thinking, compativel com o formato OpenAI API
  - **Qiniu Cloud AI** — Serviço de inferência de modelo grande da Qiniu Cloud, compatível com o formato OpenAI API, autenticação por API Key
  - **DeepSeek (Directo)** — Serviço de IA DeepSeek, suporta modo thinking, 1.048.576 de contexto
  - **Zhipu GLM** — Serviço de IA Zhipu Qingyan, suporta thinking, visão por modelo, 1.048.576 de contexto
  - **Baidu Qianfan/Wenxin** — Plataforma Baidu Qianfan, 131.072 de contexto
  - **Tencent Hunyuan** — Serviço de IA Tencent Hunyuan, duplo endpoint TokenHub/Legacy, 262.144 de contexto
  - **MiniMax** — Serviço de IA MiniMax, 1.048.576 de contexto
  - **Moonshot/Kimi** — Serviço de IA Moonshot Kimi, 262.144 de contexto
  - **SiliconFlow** — Plataforma de agregação SiliconFlow, suporta lista dinâmica de modelos, 1.048.576 de contexto
- **32 sistemas de calendário** — Cobertura completa dos principais calendários do mundo, incluindo gregoriano, lunar chinês, islâmico, hebraico, japonês, persa, maia, calendário histórico chinês, etc.
- **Sistema de Rede de Conhecimento** — Grafo de conhecimento baseado em triplas (sujeito-relação-objecto), com suporte para armazenamento, consulta e descoberta de caminhos
- **Espaço de trabalho de projecto** — Gestão de espaços de projecto, com suporte para criação/arquivamento/destruição de projectos, atribuição de funções, notas de trabalho, acompanhamento de tarefas e isolamento de permissões de ferramentas
- **Motor de fluxos de trabalho** — Motor de máquina de estados baseado em modelos, com suporte para modelos de fluxo de trabalho personalizados, transições de estado, execução orientada por Tick e gestão do ciclo de vida de instâncias
- **Mecanismo de desvanecimento da memória** — Serviço de decaimento temporizado (MemoryFadeService), que aplica automaticamente decaimento de importância e arquivamento automático à memória de todos os Silicon Beings a cada hora

### Interface Web
- **UI Web moderna** — Servidor HTTP integrado, com suporte para actualizações em tempo real via SSE
- **7 temas de skin** — Administração, Chat, Criativo, Desenvolvimento, Alto Contraste, Claro, Minimalista, com descoberta e troca automáticas
- **24 controladores** — Funcionalidades completas de gestão do sistema, chat, configuração e monitorização
- **Zero dependência de frameworks frontend** — Geração de HTML/CSS/JS no lado do servidor através de `H`, `CssBuilder` e `JsBuilder`

### Internacionalização e Localização
- **Suporte completo a 34 variantes linguísticas**, abrangendo 2 sistemas de escrita e múltiplas variantes regionais
  - **Chinês Simplificado**: zh-CN (China Continental), zh-SG (Singapura), zh-MY (Malásia) (3 variantes)
  - **Chinês Tradicional**: zh-HK (Hong Kong), zh-TW (Taiwan), zh-MO (Macau) (3 variantes)
  - **Inglês**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 variantes)
  - **Espanhol**: es-ES, es-MX (2 variantes)
  - **Alemão**: de-DE, de-AT, de-CH, de-LU, de-LI (5 variantes)
  - **Francês**: fr-FR, fr-CA, fr-CH (3 variantes)
  - **Japonês**: ja-JP | **Coreano**: ko-KR | **Checo**: cs-CZ (3 variantes)
  - **Italiano**: it-IT | **Polaco**: pl-PL | **Português**: pt-PT, pt-BR (4 variantes)

### Dados e Armazenamento
- **Armazenamento de alto desempenho SpeedyPack** — A versão Fast utiliza o motor de armazenamento .spk próprio, com mapeamento de directórios em memória + cache de entradas + fila de escrita assíncrona
- **Armazenamento em sistema de ficheiros** — A versão Default utiliza armazenamento JSON puro em sistema de ficheiros
- **Consultas por índice temporal** — Suporte para consultas eficientes por intervalo de tempo através da interface `ITimeStorage`
- **Compactação automática** — O SpeedyPack suporta compactação automática temporizada, recuperando espaço livre
- **Dependências mínimas** — A biblioteca principal depende apenas de Microsoft.CodeAnalysis.CSharp para compilação dinâmica

## 🔄 Arquitectura de Duas Versões

Este projecto oferece duas versões de implementação para satisfazer diferentes cenários:

### SiliconLife.Default (Versão Padrão)
- **Posicionamento**: Implementação padrão, usada principalmente para verificar a viabilidade da arquitectura
- **Modo de execução**: Aplicação de consola
- **Método de armazenamento**: Armazenamento JSON em sistema de ficheiros puro
- **Cenários aplicáveis**: Cenários com elevados requisitos de segurança de dados, recursos de memória limitados e pequeno volume de dados
- **Características**: Simples e fiável, persistência imediata de dados, sem risco de perda em memória
- **Descrição do papel**: Como implementação de referência para verificação da arquitectura, adequada para primeiro contacto, depuração de desenvolvimento ou cenários com prioridade de segurança de dados
- **Comando de arranque**: `dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast (Versão de Alto Desempenho)
- **Posicionamento**: Versão de produção recomendada
- **Modo de execução**: Aplicação de ambiente de trabalho (Bandeja do sistema Windows/macOS / Janela de estado Linux)
- **Método de armazenamento**: Armazenamento em memória SpeedyPack + persistência assíncrona em lote (formato de ficheiro .spk)
- **Cenários aplicáveis**: Cenários de alta concorrência, baixa latência e grande volume de dados
- **Suporte de plataforma**: Windows/macOS (funcionalidade completa, incluindo bandeja do sistema), Linux (janela de estado, sem ícone de bandeja)
- **Características**:
  - Optimização de desempenho extrema
  - Execução em segundo plano na bandeja do sistema Windows/macOS, com monitorização em tempo real através da janela de estado da bandeja; janela de estado exibida directamente no Linux
  - Motor SpeedyPack + compactação automática garantindo a segurança dos dados
  - Arquitectura Component UI, 27 componentes declarativos
  - 7 temas de skin, com descoberta e troca automáticas
  - Linux abre automaticamente o navegador para aceder ao Web UI, suporte ao parâmetro `--no-tray`
- **Melhoria de desempenho**: Latência de leitura de armazenamento reduzida 1000x, latência de escrita reduzida 15000x, capacidade de processamento concorrente aumentada 50x
- **Descrição do papel**: Implementação de nível de produção profundamente optimizada, sendo a escolha preferida para execução prolongada e ambientes de produção reais
- **Comando de arranque**: `dotnet run --project src/SiliconLife.Fast`

### Comparação de Versões

| Funcionalidade | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| **Modo de execução** | Aplicação de consola | Aplicação de ambiente de trabalho (Bandeja do sistema Windows/macOS / Janela de estado Linux) |
| **Interface do utilizador** | Web UI (acesso por navegador) | Windows/macOS: ícone de bandeja + janela de bandeja + Web UI; Linux: janela de estado + Web UI |
| **Bandeja do sistema** | ❌ Não | ✅ Windows/macOS suportam minimização para a bandeja; Linux sem ícone de bandeja |
| **Execução em segundo plano** | ❌ Fechar a consola encerra a aplicação | ✅ Windows/macOS executam continuamente em segundo plano na bandeja; Linux executa na janela de estado |
| **Método de armazenamento** | Armazenamento JSON em sistema de ficheiros | Armazenamento em memória SpeedyPack + persistência assíncrona |
| **Motor de armazenamento** | I/O do sistema de ficheiros | SiliconLife.Speedy (formato .spk) |
| **Latência de leitura** | ~10ms (I/O de disco) | ~0.01ms (operação em memória) |
| **Latência de escrita** | ~15ms (escrita síncrona) | ~0.001ms (escrita assíncrona) |
| **Capacidade concorrente** | ~100 req/s | ~5000 req/s |
| **Utilização de memória** | ~200MB | ~500MB |
| **Segurança de dados** | Muito elevada (persistência imediata) | Elevada (persistência assíncrona + compactação automática) |
| **Cenários aplicáveis** | Prioridade de segurança de dados, pequeno volume de dados | Prioridade de desempenho, grande volume de dados, alta concorrência |

## 🛠️ Stack Tecnológica

| Componente | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| Runtime | .NET 9 | .NET 9 (Windows/macOS/Linux) |
| Linguagem de programação | C# | C# |
| Tipo de aplicação | Aplicação de consola | Aplicação de ambiente de trabalho (Bandeja do sistema Windows/macOS / Janela de estado Linux) |
| Integração de IA | Ollama (local), Alibaba Cloud DashScope (nuvem), Volcengine Ark (nuvem), Herdsman, Meituan LongCat, Qiniu Cloud AI, DeepSeek, Zhipu GLM, Baidu Qianfan, Tencent Hunyuan, MiniMax, Moonshot/Kimi, SiliconFlow | Ollama (local), Alibaba Cloud DashScope (nuvem), Volcengine Ark (nuvem), Herdsman, Meituan LongCat, Qiniu Cloud AI, DeepSeek, Zhipu GLM, Baidu Qianfan, Tencent Hunyuan, MiniMax, Moonshot/Kimi, SiliconFlow |
| Armazenamento de dados | Sistema de ficheiros (JSON + directório de índice temporal) | SpeedyPack (formato .spk, mapeamento em memória + persistência assíncrona) |
| Servidor Web | HttpListener (integrado no .NET) | HttpListener (integrado no .NET) |
| Compilação dinâmica | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Automação de navegador | Playwright (WebView) | Playwright (WebView) |
| Sistema de plugins | ✅ Suportado (IPlugin + PluginLoader) | ✅ Suportado (IPlugin + PluginLoader) |
| Bandeja do sistema | ❌ Não suportado | ✅ Windows/macOS suportados (NotifyIcon); Linux sem ícone de bandeja |
| Licença | Apache-2.0 | Apache-2.0 |

## 📁 Estrutura do Projecto

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Biblioteca principal (interfaces, classes abstractas)
│   │   ├── AI/                            # Interface do cliente de IA, gestor de contexto, modelo de mensagens
│   │   ├── Audit/                         # Sistema de auditoria de utilização de Tokens
│   │   ├── Chat/                          # Sistema de chat, gestão de sessões, canal de difusão
│   │   ├── Compilation/                   # Compilação dinâmica, scanner de segurança, encriptação de código
│   │   ├── Config/                        # Sistema de gestão de configuração
│   │   ├── Executors/                     # Executors (disco, rede, linha de comandos)
│   │   ├── IM/                            # Interface do fornecedor de mensagens instantâneas
│   │   ├── Knowledge/                     # Sistema de rede de conhecimento
│   │   ├── Localization/                  # Sistema de localização
│   │   ├── Logging/                       # Sistema de registo
│   │   ├── Plugins/                       # Sistema de plugins (interface IPlugin, carregador PluginLoader)
│   │   ├── Project/                       # Sistema de gestão de projectos
│   │   ├── Runtime/                       # Ciclo principal, objectos Tick, core host
│   │   ├── Security/                      # Sistema de gestão de permissões
│   │   ├── SiliconBeing/                  # Classe base dos Silicon Beings, gestor, fábrica
│   │   ├── Storage/                       # Interface de armazenamento
│   │   ├── Time/                          # Data incompleta (consultas por intervalo de tempo)
│   │   ├── Tools/                         # Interface de ferramentas e gestor de ferramentas
│   │   ├── WebView/                       # Interface do navegador WebView
│   │   ├── Workflow/                      # Motor de fluxos de trabalho (modelos, instâncias, transições de estado)
│   │   └── ServiceLocator.cs              # Localizador de serviços global
│   │
│   ├── SiliconLife.Common/                # Implementação partilhada (comum a ambas as versões)
│   │   ├── AI/                            # Clientes e fábricas de IA (Ollama, DashScope, VolcengineArk, Herdsman, LongCat, QiniuAI, DeepSeek, Zhipu, Ernie, Hunyuan, MiniMax, Moonshot, SiliconFlow)
│   │   ├── Calendar/                      # 32 implementações de calendário
│   │   ├── Localization/                  # Classe base de localização e 34 implementações de variantes linguísticas/regionais
│   │   ├── Resources/                     # Ficheiros de recursos partilhados
│   │   ├── Security/                      # Gestor de permissões
│   │   ├── SiliconBeing/                  # Implementação padrão do Silicon Being
│   │   ├── Tools/                         # 23 implementações de ferramentas genéricas
│   │   ├── Web/                           # Infraestrutura Web
│   │   └── WebView/                       # Implementação Playwright WebView
│   │
│   ├── SiliconLife.App/                   # Camada de aplicação (Web UI + documentação de ajuda, partilhada entre Default e Fast)
│   │   ├── Config/                        # Configuração da aplicação
│   │   ├── Data/                          # Directório de dados
│   │   ├── Help/                          # Localização da documentação de ajuda (multilingue)
│   │   ├── Tools/                         # HelpTool (ferramenta de consulta da documentação de ajuda)
│   │   └── Web/                           # Implementação da Web UI
│   │       ├── Component/                 # Biblioteca de componentes UI (27 componentes)
│   │       ├── Controllers/               # 24 controladores
│   │       ├── Models/                    # Modelos de vista
│   │       ├── Views/                     # Vistas HTML
│   │       └── Skins/                     # 7 temas de skin
│   │
│   ├── SiliconLife.Default/               # Implementação padrão + ponto de entrada da aplicação (versão consola)
│   │   ├── Program.cs                     # Ponto de entrada (montagem de todos os componentes)
│   │   ├── Config/                        # Dados de configuração padrão
│   │   ├── Knowledge/                     # Implementação da rede de conhecimento
│   │   ├── Logging/                       # Implementação do fornecedor de registo (consola + sistema de ficheiros)
│   │   ├── Project/                       # Implementação do sistema de projectos
│   │   └── Storage/                       # Implementação do armazenamento em sistema de ficheiros
│   │
│   ├── SiliconLife.Fast/                  # Implementação de alto desempenho + ponto de entrada da aplicação (versão janela)
│   │   ├── Program.cs                     # Ponto de entrada (aplicação de janela)
│   │   ├── App.axaml / App.cs             # Definição da aplicação Avalonia
│   │   ├── Config/                        # Dados de configuração (partilhados com Default)
│   │   ├── Knowledge/                     # Implementação da rede de conhecimento (optimização em memória)
│   │   ├── Logging/                       # Fornecedor de registo de alto desempenho
│   │   ├── Project/                       # Implementação do sistema de projectos
│   │   ├── Storage/                       # Adaptadores de armazenamento SpeedyPack
│   │   └── Tray/                          # Bandeja do sistema (localização em 34 variantes linguísticas)
│   │
│   ├── SiliconLife.Speedy/                # Motor de armazenamento de alto desempenho SpeedyPack
│   │   ├── SpeedyPack.cs                  # Classe principal (mapeamento de directórios em memória + cache + escrita assíncrona)
│   │   ├── SpeedyPackOptions.cs           # Opções de configuração (TTL do cache, máximo de entradas, etc.)
│   │   ├── IPackTransaction.cs            # Interface de transacção
│   │   ├── SpkFileInfo.cs                 # Informações do ficheiro
│   │   └── Internal/                      # Implementação interna
│   │       ├── DirectoryMap.cs            # Mapeamento de directórios em memória
│   │       ├── EntryCache.cs              # Cache de entradas
│   │       ├── FreeList.cs                # Gestão de espaço livre
│   │       ├── PackFileReader.cs          # Leitor de ficheiros de pacote
│   │       ├── PackFileWriter.cs          # Escritor de ficheiros de pacote
│   │       ├── WriteQueue.cs              # Fila de escrita assíncrona
│   │       ├── WriteOperation.cs          # Operação de escrita
│   │       ├── SpeedyTransaction.cs       # Implementação de transacção
│   │       ├── SpkHeader.cs               # Cabeçalho do ficheiro de pacote
│   │       └── PathNormalizer.cs          # Normalização de caminhos
│   │
│   └── SiliconLife.Speedy.Manager/        # Ferramenta de gestão SpeedyPack (Avalonia UI)
│       ├── MainForm.cs                    # Formulário principal
│       ├── Program.cs                     # Ponto de entrada
│       └── slc.ico                        # Ícone da aplicação
│
├── docs/                                  # Documentação multilingue
│   ├── zh-CN/                             # Documentação em chinês simplificado
│   ├── en/                                # Documentação em inglês
│   └── ...                                # Documentação noutros idiomas
│
└── 总文档/                                 # Documentação de requisitos e arquitectura
    ├── 需求文档.md
    ├── 架构大纲.md
    └── 实现顺序.md
```

## 🏗️ Visão Geral da Arquitectura

### Arquitectura de Escalonamento
```
Ciclo Principal (thread dedicado, watchdog + circuit breaker)
  └── Objecto Tick (ordenado por prioridade)
       └── Silicon Being Manager
            └── Silicon Being Runner (thread temporário, timeout + circuit breaker)
                 └── SiliconBeing.Tick()
                      └── ContextManager.Think()
                           └── AI Client.Chat()
                                └── Ciclo de chamadas de ferramentas → Persistir no sistema de chat
```

### Arquitectura de Segurança
Todas as operações de I/O iniciadas pela IA devem passar por uma cadeia de segurança rigorosa:

```
Chamada de Ferramenta → Executor → Gestor de Permissões → [Cache de Frequência → Callback → (IsCurator: Perguntar ao Utilizador | Non-curator: ACL Global)]
```

## 🚀 Início Rápido

### Pré-requisitos

- **.NET 9 SDK** — [Ligação de download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Backend de IA** (escolher um):
  - **Ollama**: [Instalar Ollama](https://ollama.com) e obter um modelo (por exemplo `ollama pull llama3`)
  - **Alibaba Cloud DashScope**: Obter uma chave API a partir da [consola DashScope](https://bailian.console.aliyun.com/)
  - **Volcengine Ark**: Obter uma chave API a partir da [consola Volcengine](https://console.volcengine.com/ark)

### Compilar o Projecto

```bash
dotnet restore
dotnet build
```

### Executar o Sistema

#### Método 1: Executar a versão Default (aplicação de consola)

```bash
dotnet run --project src/SiliconLife.Default
```

A aplicação iniciará o servidor Web e abrirá automaticamente a Web UI no navegador.

**Cenários aplicáveis**:
- ✅ Requisitos de segurança de dados muito elevados
- ✅ Recursos de memória limitados (RAM < 2GB)
- ✅ Pequeno volume de dados, utilização a curto prazo
- ✅ Fase de depuração de desenvolvimento

#### Método 2: Executar a versão Fast (aplicação de ambiente de trabalho)

```bash
dotnet run --project src/SiliconLife.Fast
```

**Windows/macOS**: A aplicação iniciará em modo de janela, minimizando para a bandeja do sistema, executando continuamente em segundo plano.

**Linux**: A aplicação exibirá uma janela de estado (sem ícone de bandeja do sistema) e abrirá automaticamente o navegador para aceder à Web UI. Também pode usar o parâmetro `--no-tray` para saltar a abertura automática do navegador:

```bash
dotnet run --project src/SiliconLife.Fast -- --no-tray
```

**Cenários aplicáveis**:
- ✅ Cenários de alta concorrência (> 5 utilizadores)
- ✅ Grande volume de dados (utilização superior a 3 meses)
- ✅ Necessidade de resposta de baixa latência
- ✅ Necessidade de execução em segundo plano na bandeja do sistema

### Publicar como Ficheiro Único

```bash
# Windows - Versão Default
dotnet publish src/SiliconLife.Default -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Windows - Versão Fast
dotnet publish src/SiliconLife.Fast -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Linux - Versão Default
dotnet publish src/SiliconLife.Default -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# Linux - Versão Fast
dotnet publish src/SiliconLife.Fast -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# macOS - Versão Default
dotnet publish src/SiliconLife.Default -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true

# macOS - Versão Fast
dotnet publish src/SiliconLife.Fast -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

## 📋 Roteiro de Desenvolvimento

### ✅ Concluído
- [x] Fase 1: Chat de IA por consola
- [x] Fase 2: Esqueleto do framework (ciclo principal + objectos Tick + watchdog + circuit breaker)
- [x] Fase 3: Primeiro Silicon Being com Ficheiro da Alma (arquitectura corpo-cérebro)
- [x] Fase 4: Memória persistente (sistema de chat + interface de armazenamento temporal)
- [x] Fase 5: Sistema de ferramentas + executores
- [x] Fase 6: Sistema de permissões (cadeia de 5 níveis, auditor de registos, ACL Global)
- [x] Fase 7: Compilação dinâmica + auto-evolução (Roslyn)
- [x] Fase 8: Memória de longo prazo + tarefas + temporizadores
- [x] Fase 9: Core Host + colaboração multi-agente
- [x] Fase 10: Web UI (HTTP + SSE, 24 controladores, 7 skins)
- [x] Fase 10.5: Melhorias incrementais (canal de difusão, auditoria de tokens, 32 calendários, melhorias de ferramentas, localização em 34 variantes linguísticas)
- [x] Fase 10.6: Refinamento e optimização (WebView, sistema de ajuda, espaço de trabalho de projecto, rede de conhecimento, motor de fluxos de trabalho)
- [x] Fase 11: Motor de armazenamento SpeedyPack (substituição do LiteDB, mapeamento em memória, fila de escrita assíncrona, compactação automática)
- [x] Fase 12: Sistema de plugins (interface IPlugin, sandbox segura do PluginLoader, carregamento isolado, integração de ferramentas)

### 🚧 Planeado
- [ ] Fase 13: Integração com mensageiros externos (Feishu / WhatsApp / Telegram)
- [ ] Fase 14: Ecossistema de competências (marketplace de plugins, distribuição de pacotes de competências)

## 📚 Documentação

- [Desenho de Arquitectura](architecture.md) — Desenho do sistema, mecanismo de escalonamento, arquitectura de componentes
- [Modelo de Segurança](security.md) — Modelo de permissões, executores, segurança da compilação dinâmica
- [Guia de Desenvolvimento](development-guide.md) — Desenvolvimento de ferramentas, guia de extensão
- [Referência API](api-reference.md) — Documentação dos endpoints da Web API
- [Referência de Ferramentas](tools-reference.md) — Descrição detalhada das ferramentas incorporadas
- [Guia da Web UI](web-ui-guide.md) — Guia de utilização da interface Web
- [Guia do Silicon Being](silicon-being-guide.md) — Guia de desenvolvimento de agentes
- [Sistema de Permissões](permission-system.md) — Explicação detalhada da gestão de permissões
- [Sistema de Calendário](calendar-system.md) — Descrição dos 32 sistemas de calendário
- [Início Rápido](getting-started.md) — Guia de introdução detalhado
- [Resolução de Problemas](troubleshooting.md) — Respostas a perguntas frequentes
- [Roteiro](roadmap.md) — Plano de desenvolvimento completo
- [Registo de Alterações](changelog.md) — Histórico de actualizações de versões
- [Guia de Contribuição](contributing.md) — Como participar no projecto

## 🤝 Contribuir

Acolhemos contribuições de todas as formas! Consulte o [Guia de Contribuição](contributing.md) para mais detalhes.

### Fluxo de Trabalho de Desenvolvimento
1. Faça fork deste repositório
2. Crie um ramo de funcionalidade (`git checkout -b feature/AmazingFeature`)
3. Submeta as alterações (`git commit -m 'feat: add some AmazingFeature'`)
4. Empurre para o ramo (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 💡 Guia de Escolha de Versão

### Qual versão devo usar?

**SiliconLife.Default (Implementação Padrão — Verificação de Viabilidade da Arquitectura):**
- 📌 Está a ter o primeiro contacto com este projecto e deseja compreender rapidamente a arquitectura do sistema
- 📌 Está a fazer depuração de desenvolvimento e precisa de uma forma de execução simples e directa
- 📌 A segurança dos dados é a sua principal preocupação
- 📌 O seu sistema tem menos de 4GB de memória
- 📌 Apenas precisa de utilização individual ou tem um pequeno volume de dados

**SiliconLife.Fast (Versão de Produção Recomendada):**
- ⚡ Necessita de um ambiente de produção com execução estável a longo prazo
- ⚡ Já está familiarizado com a arquitectura do sistema e está pronto para implantação oficial
- ⚡ Necessita de suporte para acesso concorrente de múltiplos utilizadores
- ⚡ Necessita de execução em segundo plano na bandeja do sistema
- ⚡ Procura a experiência de desempenho extrema

> **Recomendação geral**: SiliconLife.Default é adequado como verificação de arquitectura e experiência inicial; para ambientes de produção reais, recomenda-se fortemente o uso de SiliconLife.Fast.

### Posso migrar do Default para o Fast?

**Com certeza!** Ambas as versões partilham o mesmo:
- ✅ Formato do ficheiro de configuração (config.json)
- ✅ Interface de ferramentas
- ✅ Configuração de Beings
- ✅ Interface Web UI

**Passos de migração:**
1. Faça backup do seu directório de dados do Default
2. Inicie a versão Fast usando o mesmo directório de dados
3. O Fast importará automaticamente os dados existentes para o motor de armazenamento SpeedyPack
4. Após verificar que as funcionalidades estão correctas, pode usar a versão Fast no dia-a-dia

### As duas versões podem coexistir?

**Sim!** Recomenda-se a seguinte estratégia de implantação:

**Estratégia 1: Default para verificação, Fast para produção**
```
Ambiente de desenvolvimento/verificação: SiliconLife.Default (verificar arquitectura, depurar funcionalidades)
Ambiente de produção: SiliconLife.Fast (alto desempenho, execução em segundo plano, processamento de pedidos em tempo real)
```

**Estratégia 2: Fast como execução principal, Default para backups periódicos**
```
SiliconLife.Fast (utilização diária, processamento de pedidos em tempo real)
    ↓ Backup periódico
SiliconLife.Default (arquivo de dados frios, garantia de segurança de dados)
```

## 📄 Licença

Este projecto é licenciado sob a Apache License 2.0 — consulte o ficheiro [LICENSE](../../LICENSE) para mais detalhes.

## 👨‍💻 Autor

**天源垦骥**

- GitHub: [@akimoto-akira](https://github.com/akimoto-akira/SiliconLifeCollective)
- Gitee: [hoshinokennji](https://gitee.com/hoshinokennji/SiliconLifeCollective)
- YouTube: [@hoshinokennji](https://www.youtube.com/@hoshinokennji)
- Bilibili: [617827040](https://space.bilibili.com/617827040)

## 🙏 Agradecimentos

Obrigado a todos os programadores e fornecedores de plataformas de IA que contribuíram para este projecto.

---

**Silicon Life Collective** — Fazendo os agentes de IA verdadeiramente "viver"
