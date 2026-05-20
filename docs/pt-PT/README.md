![Silicon Life Collective](../../icon/wordIcon.png)

# Silicon Life Collective

**Versão: v0.2.0-alpha** | **Silicon Life Collective** — Uma plataforma de colaboração multi-agente baseada em .NET 9, onde os agentes IA são chamados **Silicon Beings** e podem auto-evoluir-se através da compilação dinâmica Roslyn.

[English](../README.md) | [Deutsch](../de-DE/README.md) | [Français](../fr-FR/README.md) | [中文](../zh-CN/README.md) | [繁體中文](../zh-HK/README.md) | [Español](../es-ES/README.md) | [日本語](../ja-JP/README.md) | [한국어](../ko-KR/README.md) | [Čeština](../cs-CZ/README.md) | [Italiano](../it-IT/README.md) | [Polski](../pl-PL/README.md) | **Português**

## 🌟 Funcionalidades principais

### Sistema de agentes
- **Orquestração multi-agente** — Gestão centralizada pelo *Silicon Curator*, com mecanismo de agendamento equitativo por fatia de tempo controlado por relógio
- **Orientado por ficheiro da alma** — Cada Silicon Being é controlado por um ficheiro prompt central (`soul.md`) que define uma personalidade única e padrões de comportamento
- **Arquitetura Body-Brain** — O *Body* (SiliconBeing) recebe os sinais vitais e deteta os cenários de ativação; o *Brain* (ContextManager) é responsável pelo carregamento do histórico, chamadas IA, execução de ferramentas e persistência das respostas
- **Capacidade de auto-desenvolvimento** — Através da tecnologia de compilação dinâmica Roslyn, os Silicon Beings podem reescrever o seu próprio código para realizar a evolução
- **Gestão dos estados de atividade** — Suporte para quatro estados de atividade: Idle (inativo), Working (em trabalho), Error (erro), Stopped (parado). Entrada automática no estado Stopped após 10 erros consecutivos

### Sistema de plugins
- **Arquitetura de extensão via plugins** — Extensão de funcionalidades através da interface IPlugin, suporta carregamento dinâmico de DLLs de plugins a partir de um diretório
- **Sandbox segura** — O carregador de plugins executa análises de segurança rigorosas, proíbe o acesso a System.IO, System.Net e outros namespaces
- **Carregamento isolado** — Utilização de um AssemblyLoadContext personalizado para carregamento isolado, impedindo que os plugins comprometam a estabilidade do programa principal
- **Integração de ferramentas** — Os plugins podem registar ferramentas personalizadas através da interface ITool, automaticamente integradas no ciclo de chamada de ferramentas

### Ferramentas e Execução
- **24 ferramentas integradas** — Cobrem calendário, chat, configuração, disco, rede, memória, tarefas, temporizador, base de conhecimento, notas de trabalho, browser WebView, hot reload, etc.
- **Ferramenta de hot reload** — Suporta compilação automática, atualização de ficheiros e reinício do SiliconLife.Fast durante a execução, sem intervenção manual
- **Ciclo de chamada de ferramentas** — A IA retorna uma chamada de ferramenta → Executa a ferramenta → Retorna os resultados à IA → Continua o ciclo até uma resposta em texto puro
- **Segurança de permissões do executor** — Todas as operações I/O passam por uma validação rigorosa de permissões através dos executores
  - Cadeia de permissões de 5 níveis: IsCurator → UserFrequencyCache → GlobalACL → IPermissionCallback → IPermissionAskHandler
  - Registo de auditoria completo de todas as decisões de permissões

### IA e Conhecimento
- **Suporte para múltiplos backends IA**
  - **Ollama** — Implementação de modelos locais, com API HTTP nativa
  - **Alibaba Cloud DashScope (Bailian)** — Serviço IA na nuvem, compatível com API OpenAI, suporta 13+ modelos, implementação multi-região
  - **Volcengine Ark (VolcengineArk)** — Serviço IA na nuvem da ByteDance, suporta modos streaming e não-streaming, controlo de velocidade integrado
- **32 sistemas de calendário** — Cobertura completa dos principais calendários mundiais, incluindo calendário gregoriano, calendário lunar chinês, calendário islâmico, calendário hebraico, calendário japonês, calendário persa, calendário maia, calendário histórico chinês, etc.
- **Sistema de rede de conhecimento** — Grafo de conhecimento baseado em triplas (sujeito-relação-objeto), suporta armazenamento, consulta e descoberta de caminhos

### Interface Web
- **Interface Web moderna** — Servidor HTTP integrado com atualizações em tempo real SSE
- **7 temas visuais** — Versões Admin, Chat, Creative, Dev, Alto contraste, Light, Minimal, suporta deteção e comutação automática
- **20+ controladores** — Gestão completa do sistema, chat, configuração, funcionalidades de monitorização
- **Zero dependências de framework frontend** — HTML/CSS/JS gerados no lado do servidor através de `H`, `CssBuilder` e `JsBuilder`

### Internacionalização e Localização
- **Suporte completo para 32 implementações linguísticas**, cobrindo 2 sistemas de escrita e múltiplas variantes regionais
  - **Chinês simplificado**: zh-CN (China continental), zh-SG (Singapura), zh-MY (Malásia) (3 variantes)
  - **Chinês tradicional**: zh-HK (Hong Kong), zh-TW (Taiwan), zh-MO (Macau) (3 variantes)
  - **Inglês**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY (10 variantes)
  - **Espanhol**: es-ES, es-MX (2 variantes)
  - **Alemão**: de-DE, de-AT, de-CH, de-LU, de-LI (5 variantes)
  - **Francês**: fr-FR, fr-CA, fr-CH (3 variantes)
  - **Italiano**: it-IT (1 variante)
  - **Português**: pt-PT, pt-BR (2 variantes)
  - **Japonês**: ja-JP | **Coreano**: ko-KR | **Checo**: cs-CZ | **Polaco**: pl-PL (4 variantes)

### Dados e Armazenamento
- **Armazenamento SpeedyPack de alto desempenho** — A versão Fast utiliza o motor de armazenamento proprietário .spk, mapeamento de diretórios em memória + cache de entradas + fila de escrita assíncrona
- **Armazenamento em sistema de ficheiros** — A versão Default utiliza armazenamento JSON puro em sistema de ficheiros
- **Consulta por índice temporal** — Consultas eficientes por intervalo de tempo através da interface `ITimeStorage`
- **Compressão automática** — SpeedyPack suporta compressão automática agendada para recuperar espaço de armazenamento
- **Dependências mínimas** — A biblioteca principal depende apenas de Microsoft.CodeAnalysis.CSharp para compilação dinâmica

## 🔄 Arquitetura de dupla versão

Este projeto oferece duas versões de implementação para satisfazer diferentes necessidades de cenários:

### SiliconLife.Default (Versão padrão)
- **Posicionamento**: Implementação padrão, principalmente para verificação de viabilidade arquitetural
- **Modo de execução**: Aplicação de consola
- **Método de armazenamento**: Armazenamento JSON puro em sistema de ficheiros
- **Cenários aplicáveis**: Elevados requisitos de segurança de dados, recursos de memória limitados, pequeno volume de dados
- **Características**: Simples e fiável, persistência imediata dos dados, sem risco de perda de dados
- **Descrição do papel**: Implementação de referência para verificação arquitetural, adequada para primeiro contacto, depuração de desenvolvimento ou cenários com prioridade à segurança dos dados
- **Comando de arranque**: `dotnet run --project src/SiliconLife.Default`

### SiliconLife.Fast (Versão de alto desempenho)
- **Posicionamento**: Versão principal de produção
- **Modo de execução**: Aplicação desktop (Windows/macOS bandeja do sistema / Linux janela de estado)
- **Método de armazenamento**: Armazenamento em memória SpeedyPack + persistência em lote assíncrona (formato de ficheiro .spk)
- **Cenários aplicáveis**: Alta concorrência, baixa latência, grandes volumes de dados
- **Suporte de plataforma**: Windows/macOS (funcionalidades completas, incluindo bandeja do sistema), Linux (janela de estado, sem ícone na bandeja)
- **Características**:
  - Otimização de desempenho extrema
  - Windows/macOS execução em segundo plano na bandeja do sistema com monitorização em tempo real; Linux janela de estado exibida diretamente
  - Motor SpeedyPack + compressão automática que garante a segurança dos dados
  - Arquitetura Component UI, 30+ componentes declarativos
  - 7 temas visuais, suporta deteção e comutação automática
  - Ferramenta de hot reload para atualizações e reinícios online
- **Melhoria de desempenho**: Latência de leitura em memória reduzida 1000x, latência de escrita reduzida 15000x, capacidade de processamento paralelo aumentada 50x
- **Descrição do papel**: Implementação pronta para produção com otimização aprofundada, a melhor escolha para exploração a longo prazo e verdadeiros ambientes de produção
- **Comando de arranque**: `dotnet run --project src/SiliconLife.Fast`

### Comparação de versões

| Característica | SiliconLife.Default | SiliconLife.Fast |
|---------|---------------------|------------------|
| **Modo de execução** | Aplicação de consola | Aplicação Forms (bandeja de estado) |
| **Interface do utilizador** | Interface Web (acesso browser) | Ícone da bandeja + Janela da bandeja + Interface Web |
| **Bandeja do sistema** | ❌ Não | ✅ Suporta minimização para a bandeja do sistema |
| **Execução em segundo plano** | ❌ Termina ao fechar a consola | ✅ Execução contínua em segundo plano na bandeja |
| **Método de armazenamento** | Armazenamento JSON em sistema de ficheiros | Armazenamento em memória SpeedyPack + persistência assíncrona |
| **Motor de armazenamento** | I/O em sistema de ficheiros | SiliconLife.Speedy (formato .spk) |
| **Latência de leitura** | ~10ms (I/O em disco) | ~0.01ms (operação em memória) |
| **Latência de escrita** | ~15ms (escrita síncrona) | ~0.001ms (escrita assíncrona) |
| **Concorrência** | ~100 req/s | ~5000 req/s |
| **Utilização de memória** | ~200MB | ~500MB |
| **Segurança de dados** | Extremamente elevada (persistência imediata) | Elevada (persistência assíncrona + compressão automática) |
| **Cenário recomendado** | Prioridade à segurança de dados, dados pequenos | Prioridade ao desempenho, dados grandes, alta concorrência |

## 🛠️ Stack tecnológica

| Componente | SiliconLife.Default | SiliconLife.Fast |
|------|---------------------|------------------|
| Runtime | .NET 9 | .NET 9 (Windows/macOS/Linux) |
| Linguagem de programação | C# | C# |
| Tipo de aplicação | Aplicação de consola | Aplicação desktop (Windows/macOS bandeja do sistema / Linux janela de estado) |
| Integração IA | Ollama (local), Alibaba Cloud DashScope (nuvem) | Ollama (local), Alibaba Cloud DashScope (nuvem), Volcengine Ark (nuvem) |
| Armazenamento de dados | Sistema de ficheiros (JSON + diretório de índice temporal) | SpeedyPack (formato .spk, mapeamento em memória + persistência assíncrona) |
| Servidor Web | HttpListener (.NET integrado) | HttpListener (.NET integrado) |
| Compilação dinâmica | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) | Roslyn (Microsoft.CodeAnalysis.CSharp 4.13.0) |
| Automação de browser | Playwright (WebView) | Playwright (WebView) |
| Sistema de plugins | ✅ Suportado (IPlugin + PluginLoader) | ✅ Suportado (IPlugin + PluginLoader) |
| Bandeja do sistema | ❌ Não suportado | ✅ Suportado (NotifyIcon) |
| Licença | Apache-2.0 | Apache-2.0 |

## 📁 Estrutura do projeto

```
SiliconLifeCollective.sln
├── src/
│   ├── SiliconLife.Core/                  # Biblioteca principal (interfaces, classes abstratas)
│   │   ├── AI/                            # Interfaces cliente IA, ContextManager, modelos de mensagens
│   │   ├── Audit/                         # Sistema de auditoria de utilização de tokens
│   │   ├── Chat/                          # Sistema de chat, gestão de sessões, canais de broadcast
│   │   ├── Compilation/                   # Compilação dinâmica, análise de segurança, encriptação de código
│   │   ├── Config/                        # Sistema de gestão de configuração
│   │   ├── Executors/                     # Executores (disco, rede, linha de comando)
│   │   ├── IM/                            # Interfaces de fornecedor de mensagens instantâneas
│   │   ├── Knowledge/                     # Sistema de rede de conhecimento
│   │   ├── Localization/                  # Sistema de localização
│   │   ├── Logging/                       # Sistema de registo
│   │   ├── Plugins/                       # Sistema de plugins (interface IPlugin, PluginLoader)
│   │   ├── Project/                       # Sistema de gestão de projetos
│   │   ├── Runtime/                       # Loop principal, objetos de relógio, host principal
│   │   ├── Security/                      # Sistema de gestão de permissões
│   │   ├── SiliconBeing/                  # Classe base Silicon Being, gestor, fábrica
│   │   ├── Storage/                       # Interfaces de armazenamento
│   │   ├── Time/                          # Datas incompletas (consulta por intervalo temporal)
│   │   ├── Tools/                         # Interfaces de ferramentas e gestor de ferramentas
│   │   ├── WebView/                       # Interfaces de browser WebView
│   │   └── ServiceLocator.cs              # Localizador de serviços global
│   │
│   ├── SiliconLife.Common/                # Implementação comum (ambas as versões)
│   │   ├── AI/                            # Clientes IA e fábricas (Ollama, DashScope, VolcengineArk)
│   │   ├── Calendar/                      # 32 implementações de calendários
│   │   ├── Localization/                  # Classe base de localização e 31 variantes linguísticas/regionais
│   │   ├── Resources/                     # Ficheiros de recursos partilhados
│   │   ├── Security/                      # Gestor de Permissões
│   │   ├── SiliconBeing/                  # Implementação padrão do Silicon Being
│   │   ├── Tools/                         # 23 ferramentas comuns (incluindo hot reload)
│   │   ├── Web/                           # Infraestrutura Web
│   │   └── WebView/                       # Implementação Playwright WebView
│   │
│   ├── SiliconLife.App/                   # Camada de aplicação (Web UI + ajuda, partilhada entre Default e Fast)
│   │   ├── Config/                        # Configuração da aplicação
│   │   ├── Data/                          # Diretório de dados
│   │   ├── Help/                          # Localização da documentação de ajuda (multilingue)
│   │   └── Web/                           # Implementação da interface Web
│   │       ├── Component/                 # Biblioteca de componentes UI (30+ componentes)
│   │       ├── Controllers/               # 22 controladores
│   │       ├── Models/                    # ViewModels
│   │       ├── Views/                     # Vistas HTML
│   │       └── Skins/                     # 7 temas visuais
│   │
│   ├── SiliconLife.Default/               # Implementação padrão + ponto de entrada (versão consola)
│   │   ├── Program.cs                     # Ponto de entrada (montagem de todos os componentes)
│   │   ├── Config/                        # Dados de configuração padrão
│   │   ├── IM/                            # Fornecedor WebUI
│   │   ├── Knowledge/                     # Implementação da rede de conhecimento
│   │   ├── Logging/                       # Implementações de fornecedores de registo
│   │   ├── Project/                       # Implementação do sistema de projetos
│   │   ├── Security/                      # Callback de permissões padrão
│   │   ├── Storage/                       # Implementação de armazenamento em sistema de ficheiros
│   │   └── Tools/                         # Ferramentas específicas da versão (HelpTool)
│   │
│   ├── SiliconLife.Fast/                  # Implementação de alto desempenho + ponto de entrada (versão Forms)
│   │   ├── Program.cs                     # Ponto de entrada (aplicação Forms)
│   │   ├── Config/                        # Dados de configuração (partilhados com Default)
│   │   ├── IM/                            # Fornecedor WebUI
│   │   ├── Knowledge/                     # Implementação da rede de conhecimento (otimizada para memória)
│   │   ├── Logging/                       # Fornecedores de registo de alto desempenho
│   │   ├── Project/                       # Implementação do sistema de projetos
│   │   ├── Security/                      # Callback de permissões otimizado
│   │   ├── Storage/                       # Adaptador de armazenamento SpeedyPack
│   │   ├── Tools/                         # Ferramentas específicas da versão (HelpTool)
│   │   └── Tray/                          # Bandeja do sistema (31 variantes linguísticas)
│   │
│   ├── SiliconLife.Speedy/                # Motor de armazenamento de alto desempenho SpeedyPack
│   │   ├── SpeedyPack.cs                  # Classe principal (mapeamento de diretórios em memória + cache + escrita assíncrona)
│   │   ├── SpeedyPackOptions.cs           # Opções de configuração (TTL da cache, máx. entradas, etc.)
│   │   ├── IPackTransaction.cs            # Interface de transação
│   │   ├── SpkFileInfo.cs                 # Informações do ficheiro
│   │   └── Internal/                      # Implementação interna
│       │   ├── DirectoryMap.cs            # Mapeamento de diretórios em memória
│       │   ├── EntryCache.cs              # Cache de entradas
│       │   ├── FreeList.cs                # Gestão de espaço livre
│       │   ├── PackFileReader.cs          # Leitor de ficheiros de pacote
│       │   ├── PackFileWriter.cs          # Escritor de ficheiros de pacote
│       │   ├── WriteQueue.cs              # Fila de escrita assíncrona
│       │   ├── WriteOperation.cs          # Operação de escrita
│       │   ├── SpeedyTransaction.cs       # Implementação de transação
│       │   ├── SpkHeader.cs              # Cabeçalho do ficheiro de pacote
│       │   └── PathNormalizer.cs          # Normalização de caminho
│   │
│   └── SiliconLife.Speedy.Manager/        # Ferramenta de gestão SpeedyPack (Windows Forms)
│       ├── MainForm.cs                    # Formulário principal
│       ├── Program.cs                     # Ponto de entrada
│       └── slc.ico                        # Ícone da aplicação
│
├── docs/                                  # Documentação multilingue
│   ├── zh-CN/                             # Chinês simplificado
│   ├── en/                                # Inglês
│   └── ...                                # Outras línguas
```

## 🏗️ Visão geral da arquitetura

### Arquitetura de agendamento
```
Loop principal (thread dedicado, watchdog + circuit breaker)
  └── Objeto de relógio (ordenado por prioridade)
       └── Gestor de Silicon Beings
            └── Executor do Silicon Being (thread temporário, timeout + circuit breaker)
                 └── SiliconBeing.Tick()
                      └── ContextManager.Pensar()
                           └── Cliente-IA.Chat()
                                └── Loop de chamada de ferramentas → Persistência no sistema de chat
```

### Arquitetura de segurança
Todas as operações I/O iniciadas pela IA devem passar por uma cadeia de segurança rigorosa:

```
Chamada de ferramenta → Executor → Gestor de permissões → [IsCurator → Cache de frequência → GlobalACL → Callback → Pedido ao utilizador]
```

## 🚀 Arranque rápido

### Pré-requisitos

- **.NET 9 SDK** — [Link de download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Backend IA** (escolha um):
  - **Ollama**: [Instalar Ollama](https://ollama.com) e descarregar um modelo (ex. `ollama pull llama3`)
  - **Alibaba Cloud DashScope**: Obtenha uma chave API na [consola DashScope](https://bailian.console.aliyun.com/)
  - **Volcengine Ark**: Obtenha uma chave API na [consola Volcengine](https://console.volcengine.com/ark)

### Compilar o projeto

```bash
dotnet restore
dotnet build
```

### Executar o sistema

#### Método 1: Executar a versão Default (aplicação de consola)

```bash
dotnet run --project src/SiliconLife.Default
```

**Silicon Life Collective** — Tornar os agentes IA verdadeiramente "vivos"
