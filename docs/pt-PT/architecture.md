# Arquitetura

> **Versão: v0.2.0-alpha**

[English](../en/architecture.md) | [Deutsch](../de-DE/architecture.md) | [Français](../fr-FR/architecture.md) | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Čeština](../cs-CZ/architecture.md) | [Italiano](../it-IT/architecture.md) | [Polski](../pl-PL/architecture.md) | **Português**

## Arquitetura de dupla versão

Este projeto oferece duas versões de implementação, que partilham o mesmo design arquitetural, mas diferem no armazenamento e na otimização de desempenho:

### SiliconLife.Default (Versão padrão)
- **Posicionamento**: Implementação padrão, principalmente para verificação de viabilidade arquitetural
- **Modo de execução**: Aplicação de consola
- **Método de armazenamento**: Armazenamento JSON puro em sistema de ficheiros
- **Cenários aplicáveis**: Requisitos elevados de segurança de dados, recursos de memória limitados, pequeno volume de dados
- **Descrição do papel**: Implementação de referência para verificação arquitetural, oferece uma execução simples e fiável, adequada para primeiro contacto, depuração de desenvolvimento ou cenários com prioridade à segurança dos dados

### SiliconLife.Fast (Versão de alto desempenho)
- **Posicionamento**: Versão principal de produção
- **Modo de execução**: Aplicação desktop (Windows/macOS bandeja do sistema / Linux janela de estado)
- **Método de armazenamento**: Armazenamento em memória SpeedyPack + persistência em lote assíncrona (formato de ficheiro .spk)
- **Cenários aplicáveis**: Alta concorrência, baixa latência, grandes volumes de dados
- **Suporte de plataforma**: Windows/macOS (funcionalidades completas, incluindo bandeja do sistema), Linux (janela de estado, sem ícone na bandeja)
- **Características**:
  - Windows/macOS execução em segundo plano na bandeja do sistema, monitorização em tempo real através da janela de estado; Linux janela de estado exibida diretamente
  - Motor SpeedyPack + compressão automática que garante a segurança dos dados
  - Arquitetura Component UI, 27 componentes declarativos
  - 7 temas visuais, suporta deteção e comutação automática
  - Ferramenta de hot reload para atualizações e reinícios online
  - Linux abre automaticamente o browser para acesso à Web UI, suporta o parâmetro `--no-tray`
- **Melhoria de desempenho**: Latência de leitura reduzida 1000x, latência de escrita reduzida 15000x
- **Descrição do papel**: Implementação pronta para produção com otimização aprofundada, com execução em segundo plano na bandeja do sistema, motor SpeedyPack + compressão automática, a melhor escolha para exploração a longo prazo e verdadeiros ambientes de produção

> **Nota**: A arquitetura descrita neste documento aplica-se a ambas as versões, apenas as implementações de armazenamento diferem. SiliconLife.Default serve como referência para verificação arquitetural, SiliconLife.Fast é a versão principal recomendada para produção.

---

## Conceitos fundamentais

### Silicon Being

Cada agente IA do sistema é um **Silicon Being** — uma entidade autónoma com a sua própria identidade, personalidade e capacidades. Cada Silicon Being é orientado por um **ficheiro da alma** (prompt Markdown) que define os seus padrões de comportamento.

### Silicon Curator

O **Silicon Curator** é um Silicon Being especial com as permissões de sistema mais elevadas. Atua como administrador do sistema:

- Criação e gestão dos outros Silicon Beings
- Análise dos pedidos do utilizador e decomposição em tarefas
- Distribuição das tarefas aos Silicon Beings apropriados
- Monitorização da qualidade da execução e gestão dos erros
- Resposta às mensagens do utilizador com **agendamento prioritário** (ver abaixo)

### Ficheiro da alma

Ficheiro Markdown armazenado no diretório de dados de cada Silicon Being (`soul.md`). É injetado como prompt de sistema em cada pedido IA, definindo a personalidade, os padrões de decisão e as restrições comportamentais do Being.

---

## Agendamento: Agendamento equitativo por fatia de tempo

### Loop principal + Objetos de relógio

O sistema executa um **loop principal orientado por relógio** num thread dedicado:

```
Loop principal (thread dedicado, watchdog + circuit breaker)
  └── Objeto de relógio A (Prioridade=0, Intervalo=100ms)
  └── Objeto de relógio B (Prioridade=1, Intervalo=500ms)
  └── SiliconBeingManager (ativado pelo relógio do loop principal)
        └── SiliconBeingRunner → Silicon Being 1 → Ativação do relógio → Execução de um ciclo
        └── SiliconBeingRunner → Silicon Being 2 → Ativação do relógio → Execução de um ciclo
        └── SiliconBeingRunner → Silicon Being 3 → Ativação do relógio → Execução de um ciclo
        └── ...
```

Decisões de design chave:

- **Os Silicon Beings não herdam o objeto de relógio.** Têm o seu próprio método `Tick()`, chamado por `SiliconBeingManager` através de `SiliconBeingRunner`, e não registados diretamente no loop principal.
- **SiliconBeingManager** é ativado diretamente pelo relógio do loop principal e atua como proxy único para todos os Beings.
- **SiliconBeingRunner** encapsula o `Tick()` de cada Being num thread temporário, com timeout e circuit breaker por Being (3 timeouts consecutivos → 1 minuto de arrefecimento).
- A execução de cada Being é limitada a **um ciclo** de pedido IA + chamada de ferramenta por ativação do relógio, garantindo que nenhum Being pode monopolizar o loop principal.
- **Monitor de desempenho** acompanha os tempos de execução do relógio para observabilidade.

### Resposta prioritária do Curator

Quando um utilizador envia uma mensagem ao Silicon Curator:

1. O Being atual (por exemplo Being A) termina o seu ciclo em curso — **sem interrupção**.
2. O gestor **salta o resto da fila**.
3. O loop **retoma a partir do Curator**, permitindo-lhe executar-se imediatamente.

Isto garante a reatividade às interações do utilizador sem interromper as tarefas em curso.

---

## Arquitetura de componentes

```
┌─────────────────────────────────────────────────────────┐
│                        Host principal                    │
│  (Host unificado — monta e gere todos os componentes)   │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │ Loop     │  │ Localizador  │  │   Configuração    │  │
│  │ principal│  │ de serviços  │  │                   │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │     SiliconBeingManager (objeto de relógio)       │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Curator  │ │Being A  │ │Being B  │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │              Serviços partilhados                 │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │ Sistema  │  │Armaz.    │  │  Gestor de       │  │   │
│  │  │ de chat  │  │          │  │  permissões      │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │ Cliente  │  │Executor  │  │  Gestor de       │  │   │
│  │  │ IA       │  │          │  │  ferramentas     │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  │  ┌──────────┐ ┌──────────┐                        │   │
│  │  │Carregador│  │ Rede de │                        │   │
│  │  │ de plugin│  │ conheic.│                        │   │
│  │  └──────────┘ └──────────┘                        │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                  Executores                      │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Executor  │  │Executor  │  │  Executor de     │  │   │
│  │  │ de disco │  │ de rede  │  │  linha comando   │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │       Fornecedores de mensagens instantâneas      │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Fornecedor│  │Fornecedor│  │  Feishu / ...    │  │   │
│  │  │consola   │  │ Web      │  │  Fornecedor      │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## Localizador de serviços

`ServiceLocator` é um registo singleton thread-safe que fornece acesso a todos os serviços principais:

| Propriedade | Tipo | Descrição |
|-----------|------|-------------|
| `ChatSystem` | `ChatSystem` | Gestor central das sessões de chat |
| `IMManager` | `IMManager` | Router dos fornecedores de mensagens instantâneas |
| `AuditLogger` | `AuditLogger` | Registo de auditoria das permissões |
| `GlobalAcl` | `GlobalACL` | Lista de controlo de acesso global |
| `BeingFactory` | `ISiliconBeingFactory` | Fábrica para criação de Beings |
| `BeingManager` | `SiliconBeingManager` | Gestor do ciclo de vida dos Beings ativos |
| `DynamicBeingLoader` | `DynamicBeingLoader` | Carregador de compilação dinâmica |
| `TokenUsageAudit` | `ITokenUsageAudit` | Monitorização da utilização de tokens |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Relatório da utilização de tokens |

Mantém também um registo de `PermissionManager` por Being, indexado pelo GUID do Being.

---

## Sistema de chat

### Tipos de sessões

O sistema de chat suporta três tipos de sessões através de `SessionBase`:

| Tipo | Classe | Descrição |
|------|--------|-------------|
| `SingleChat` | `SingleChatSession` | Conversação um-a-um entre dois participantes |
| `GroupChat` | `GroupChatSession` | Chat de grupo multi-participante |
| `Broadcast` | `BroadcastChannel` | Canal aberto com ID fixo; os Beings subscrevem dinamicamente e recebem apenas as mensagens após a subscrição |

### Canais de broadcast

`BroadcastChannel` é um tipo de sessão especial para anúncios ao nível do sistema:

- **ID do canal fixo** — Ao contrário de `SingleChatSession` e `GroupChatSession`, o ID do canal é uma constante conhecida, não derivada dos GUIDs dos membros.
- **Subscrição dinâmica** — Os Beings subscrevem/cancelam a subscrição em tempo de execução; recebem apenas as mensagens publicadas após a sua subscrição.
- **Filtragem de mensagens pendentes** — `GetPendingMessages()` retorna apenas as mensagens publicadas após a hora de subscrição do Being e ainda não lidas.
- **Gerido pelo sistema de chat** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### Mensagens de chat

O modelo `ChatMessage` contém campos para o contexto de conversação IA e monitorização de tokens:

| Campo | Tipo | Descrição |
|-------|------|-------------|
| `Id` | `Guid` | Identificador único da mensagem |
| `SenderId` | `Guid` | Identificador único do remetente |
| `ChannelId` | `Guid` | Identificador do canal/conversação |
| `Content` | `string` | Conteúdo da mensagem |
| `Timestamp` | `DateTime` | Hora de envio da mensagem |
| `Type` | `MessageType` | Texto, imagem, ficheiro ou notificação de sistema |
| `ReadBy` | `List<Guid>` | IDs dos participantes que leram esta mensagem |
| `Role` | `MessageRole` | Papel na conversação IA (utilizador, assistente, ferramenta) |
| `ToolCallId` | `string?` | ID da chamada de ferramenta para mensagens de resultado de ferramenta |
| `ToolCallsJson` | `string?` | JSON serializado das chamadas de ferramenta para mensagens do assistente |
| `Thinking` | `string?` | Raciocínio da cadeia de pensamento da IA |
| `PromptTokens` | `int?` | Número de tokens no prompt (entrada) |
| `CompletionTokens` | `int?` | Número de tokens na compleção (saída) |
| `TotalTokens` | `int?` | Número total de tokens utilizados (entrada + saída) |
| `FileMetadata` | `FileMetadata?` | Metadados do ficheiro anexo (se a mensagem contiver um ficheiro) |

### Fila de mensagens de chat

`ChatMessageQueue` é um sistema de fila de mensagens thread-safe para a gestão assíncrona das mensagens de chat:

- **Thread-safe** — Utiliza mecanismos de bloqueio para garantir a segurança do acesso concorrente
- **Processamento assíncrono** — Suporta o enfileiramento e desenfileiramento assíncrono das mensagens
- **Ordenação de mensagens** — Mantém a ordem cronológica das mensagens
- **Operações em lote** — Suporta a obtenção em lote das mensagens

### Metadados de ficheiro

`FileMetadata` gere as informações sobre os ficheiros anexados às mensagens de chat:

- **Informações do ficheiro** — Nome, tamanho, tipo, caminho
- **Timestamp de carregamento** — Timestamp do carregamento do ficheiro
- **Carregador** — ID do utilizador ou Silicon Being que carregou o ficheiro

### Gestor de cancelamento de stream

`StreamCancellationManager` fornece um mecanismo de cancelamento para as respostas IA em streaming:

- **Controlo de fluxo** — Suporta o cancelamento das respostas IA em streaming em curso
- **Limpeza de recursos** — Limpeza correta dos recursos associados durante o cancelamento
- **Segurança de concorrência** — Suporta a gestão simultânea de múltiplos streams

### Histórico do chat

A funcionalidade de histórico do chat permite aos utilizadores navegar pelas conversações passadas dos Silicon Beings:

- **Lista de sessões** — Mostra todas as sessões históricas
- **Detalhes das mensagens** — Mostra o histórico completo das mensagens
- **Vista cronológica** — Apresenta as mensagens em ordem cronológica
- **Suporte API** — Fornece uma API RESTful para obter dados de sessões e mensagens

---

## Sistema de cliente IA

O sistema suporta múltiplos backends IA através da interface `IAIClient`:

### OllamaClient

- **Tipo**: Serviço IA local
- **Protocolo**: API HTTP Ollama nativa (`/api/chat`, `/api/generate`)
- **Funcionalidades**: Streaming, chamadas de ferramenta, alojamento de modelos locais
- **Configuração**: `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (Alibaba Cloud DashScope)

- **Tipo**: Serviço IA na nuvem
- **Protocolo**: API compatível com OpenAI (`/compatible-mode/v1/chat/completions`)
- **Autenticação**: Bearer token (chave API)
- **Funcionalidades**: Streaming, chamadas de ferramenta, conteúdo de raciocínio (cadeia de pensamento), implementação multi-região
- **Regiões suportadas**:
  - `beijing` — China Norte 2 (Pequim)
  - `virginia` — EUA (Virgínia)
  - `singapore` — Singapura
  - `hongkong` — Hong Kong, China
  - `frankfurt` — Alemanha (Frankfurt)
- **Modelos suportados** (descoberta dinâmica via API, com lista de fallback):
  - **Série Qwen**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **Raciocínio**: qwq-plus
  - **Terceiros**: deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **Configuração**: `apiKey`, `region`, `model`
- **Descoberta de modelos**: Obtenção de modelos disponíveis a partir da API DashScope em tempo de execução; fallback para uma lista selecionada em caso de falha de rede

### VolcengineArkClient (Volcengine Ark)

- **Tipo**: Serviço IA na nuvem
- **Protocolo**: API compatível com OpenAI
- **Autenticação**: Bearer token (chave API)
- **Funcionalidades**: Suporte de modos streaming e não-streaming, controlo de velocidade duplo integrado
  - Controlo de velocidade próprio: Aplicação do intervalo mínimo entre pedidos
  - Limitação de velocidade do servidor: Gestão de erros 429, nova tentativa com backoff exponencial
- **Configuração**: `apiKey`, `endpoint`, `model`
- **Características**: Serviço IA da ByteDance, suporta vários modelos Doubao

### Padrão Factory dos clientes IA

Cada tipo de cliente IA tem uma correspondente implementação factory de `IAIClientFactory`:

- `OllamaClientFactory` — Cria instâncias OllamaClient
- `DashScopeClientFactory` — Cria instâncias DashScopeClient
- `VolcengineArkClientFactory` — Cria instâncias VolcengineArkClient

A factory fornece:
- `CreateClient(Dictionary<string, object> config)` — Instancia um cliente a partir da configuração
- `GetConfigKeyOptions(string key, ...)` — Retorna opções dinâmicas para uma chave de configuração (ex. modelos disponíveis, regiões)
- `GetDisplayName()` — Nome de exibição localizado do tipo de cliente

### Lista de plataformas IA suportadas

#### Legenda de estados
- ✅ Implementado
- 🚧 Em desenvolvimento
- 📋 Planeado
- 💡 Em consideração

*Nota: Devido ao ambiente de rede do programador, a ligação aos serviços IA internacionais marcados [Em consideração] pode necessitar de ferramentas de proxy de rede e o processo de depuração pode ser instável.*

#### Lista de plataformas

| Plataforma | Estado | Tipo | Descrição |
|------------|--------|------|-------------|
| Ollama | ✅ | Local | Serviço IA local, suporta implementação de modelos locais |
| DashScope (Alibaba Cloud) | ✅ | Nuvem | Serviço IA DashScope da Alibaba Cloud, implementação multi-região |
| Baidu Qianfan | 📋 | Nuvem | Serviço IA Wenxin Yiyan da Baidu |
| Zhipu AI (GLM) | 📋 | Nuvem | Serviço IA ChatGLM |
| Moonshot (Kimi) | 📋 | Nuvem | Serviço IA Kimi |
| Volcengine Ark (Doubao) | ✅ | Nuvem | Serviço IA Doubao da ByteDance |
| DeepSeek | 📋 | Nuvem | Serviço IA DeepSeek |
| 01.AI | 📋 | Nuvem | Serviço IA Yi |
| Tencent Hunyuan | 📋 | Nuvem | Serviço IA Hunyuan da Tencent |
| SiliconFlow | 📋 | Nuvem | Serviço IA SiliconFlow |
| MiniMax | 📋 | Nuvem | Serviço IA MiniMax |
| OpenAI | 💡 | Nuvem | Serviço OpenAI API (série GPT) |
| Anthropic | 💡 | Nuvem | Serviço Anthropic Claude AI |
| Google DeepMind | 💡 | Nuvem | Serviço Google Gemini |
| Mistral AI | 💡 | Nuvem | Serviço Mistral AI |
| Groq | 💡 | Nuvem | Serviço de inferência IA de alta velocidade Groq |
| Together AI | 💡 | Nuvem | Serviço de modelos open-source Together AI |
| xAI | 💡 | Nuvem | Serviço xAI Grok |
| Cohere | 💡 | Nuvem | Serviço NLP empresarial Cohere |
| Replicate | 💡 | Nuvem | Plataforma de alojamento de modelos open-source Replicate |
| Hugging Face | 💡 | Nuvem | Comunidade e plataforma de modelos IA open-source Hugging Face |
| Cerebras | 💡 | Nuvem | Serviço de inferência IA otimizado Cerebras |
| Databricks | 💡 | Nuvem | Plataforma IA empresarial Databricks (MosaicML) |
| Perplexity AI | 💡 | Nuvem | Serviço de pesquisa e respostas Perplexity AI |
| NVIDIA NIM | 💡 | Nuvem | Microsserviço de inferência IA NVIDIA |

---

## Decisões de design chave

### Armazenamento como classe de instância (não estática)

`IStorage` foi concebido como uma instância injetável, não como um utilitário estático. Isto garante:

- Acesso direto ao sistema de ficheiros — IStorage é o canal de persistência interno do sistema, **não** roteado através do executor.
- **A IA não controla o IStorage** — O executor gere o IO iniciado pelas ferramentas da IA; o IStorage gere a leitura e escrita interna do próprio framework. Estes são preocupações fundamentalmente diferentes.
- Pode ser testado com implementações simuladas.
- Suporte futuro para diferentes backends de armazenamento sem modificação dos consumidores.

### Executor como fronteira de segurança

O executor é o **único** caminho para operações de I/O. Ferramentas que necessitam de acesso a disco, rede ou linha de comando **têm** de passar pelo executor. Este design impõe:

- **Thread de despacho independente** por executor, com bloqueio de thread para verificação de permissões.
- Verificação de permissões centralizada — O executor consulta o **gestor de permissões privado** do Being.
- Fila de pedidos com suporte a prioridade e controlo de timeout.
- Registo de auditoria de todas as operações externas.
- Isolamento de exceções — A falha de um executor não afeta os outros.
- Disjuntor — Falhas consecutivas param temporariamente o executor para evitar falhas em cascata.

### ContextManager como objeto leve

Cada `ExecuteOneRound()` cria uma nova instância de `ContextManager`:

1. Carrega o ficheiro da alma + o histórico de chat recente.
2. Envia o pedido ao cliente IA.
3. Processa em ciclo as chamadas de ferramenta até a IA retornar texto simples.
4. Persiste a resposta no sistema de chat.
5. Liberta os recursos.

---

## Auditoria de utilização de tokens

O sistema rastreia o consumo de tokens IA para cada pedido:

- `TokenUsageRecord` — Registo de cada pedido (ID do Being, modelo, tokens do prompt, tokens da compleção, timestamp)
- `TokenUsageSummary` — Estatísticas agregadas
- `TokenUsageQuery` — Parâmetros de consulta para filtrar registos
- Persistido através de `ITimeStorage` para consultas de séries temporais
- Acessível através da Web UI (UsageController) e `TokenAuditTool` (apenas Curator)

---

### Sistema de calendário

O sistema inclui **32 implementações de calendário**, derivadas da classe abstrata `CalendarBase`, cobrindo os principais sistemas de calendário do mundo:

| Calendário | ID | Descrição |
|------------|-----|-------------|
| BuddhistCalendar | `buddhist` | Calendário budista (BE), ano + 543 |
| CherokeeCalendar | `cherokee` | Sistema de calendário Cherokee |
| ChineseLunarCalendar | `lunar` | Calendário lunar chinês, com meses intercalares |
| ChineseHistoricalCalendar | `chinese_historical` | Calendário histórico chinês, suporta Ganzhi e eras imperiais |
| ChulaSakaratCalendar | `chula_sakarat` | Calendário Chula Sakarat (CS), ano - 638 |
| CopticCalendar | `coptic` | Calendário Copta |
| DaiCalendar | `dai` | Calendário Dai, com cálculo lunar completo |
| DehongDaiCalendar | `dehong_dai` | Variante do calendário Dai de Dehong |
| EthiopianCalendar | `ethiopian` | Calendário Etíope |
| FrenchRepublicanCalendar | `french_republican` | Calendário Republicano Francês |
| GregorianCalendar | `gregorian` | Calendário Gregoriano padrão |
| HebrewCalendar | `hebrew` | Calendário Hebraico (Judaico) |
| IndianCalendar | `indian` | Calendário Nacional Indiano |
| InuitCalendar | `inuit` | Sistema de calendário Inuit |
| IslamicCalendar | `islamic` | Calendário Islâmico (Hijri) |
| JapaneseCalendar | `japanese` | Calendário Japonês (Nengo) |
| JavaneseCalendar | `javanese` | Calendário Islâmico Javanês |
| JucheCalendar | `juche` | Calendário Juche (Coreia do Norte), ano - 1911 |
| JulianCalendar | `julian` | Calendário Juliano |
| KhmerCalendar | `khmer` | Calendário Khmer |
| MayanCalendar | `mayan` | Calendário Longo Conto Maia |
| MongolianCalendar | `mongolian` | Calendário Mongol |
| PersianCalendar | `persian` | Calendário Persa (Solar Hijri) |
| RepublicOfChinaCalendar | `roc` | Calendário da República da China (Minguo), ano - 1911 |
| RomanCalendar | `roman` | Calendário Romano |
| SakaCalendar | `saka` | Calendário Saka (Indonésia) |
| SexagenaryCalendar | `sexagenary` | Calendário Ganzhi Chinês |
| TibetanCalendar | `tibetan` | Calendário Tibetano |
| VietnameseCalendar | `vietnamese` | Calendário Lunar Vietnamita (variante do zodíaco do gato) |
| VikramSamvatCalendar | `vikram_samvat` | Calendário Vikram Samvat |
| YiCalendar | `yi` | Sistema de calendário Yi |
| ZoroastrianCalendar | `zoroastrian` | Calendário Zoroastriano |

`CalendarTool` fornece as operações: `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (conversão de datas entre calendários).

---

## Arquitetura Web UI

### Sistema de temas

A Web UI possui um **sistema de temas plugável**, permitindo personalização completa da interface sem alterar a lógica da aplicação:

- **Interface ISkin** — Define o contrato para todos os temas, incluindo:
  - Métodos de renderização principal (`RenderHtml`, `RenderError`)
  - 20+ métodos de componentes UI (botão, input, cartão, tabela, badge, bolha, progresso, tab, etc.)
  - Geração de CSS temático através de `CssBuilder`
  - `SkinPreviewInfo` — Paleta de cores e ícone para o seletor de temas na página de inicialização

- **Temas incorporados** — 7 temas prontos para produção:
  - **Admin** — Interface de gestão do sistema profissional, focada em dados
  - **Chat** — Design centrado em conversação e mensagens, para interação IA
  - **Creative** — Layout de fluxo de trabalho criativo, artístico e visualmente rico
  - **Dev** — Interface centrada no programador e no código, com destaque de sintaxe
  - **HighContrast** — Tema de acessibilidade de alto contraste
  - **Light** — Tema claro e fresco
  - **Minimal** — Tema minimalista

- **Descoberta de temas** — `SkinManager` descobre e regista automaticamente todas as implementações de `ISkin` através de reflexão

### Construtores HTML / CSS / JS

A Web UI evita completamente ficheiros de template, gerando toda a marcação em C#:

- **`H`** — DSL de construção HTML em fluxo, para construir árvores HTML em código
- **`CssBuilder`** — Construtor CSS, com suporte a seletores e media queries
- **`JsBuilder` (`JsSyntax`)** — Construtor JavaScript, para scripts inline

### Sistema de controladores

A Web UI segue um **padrão tipo MVC**, com 23 controladores a gerir diferentes aspetos:

| Controlador | Função |
|------------|---------|
| About | Página sobre e informações do projeto |
| Audit | Auditoria de utilização de tokens |
| Being | Gestão e estado dos Silicon Beings |
| Chat | Interface de chat em tempo real com SSE |
| ChatHistory | Visualização do histórico de chat, com lista de sessões e detalhes das mensagens |
| CodeBrowser | Visualização e edição de código |
| CodeHover | Dicas flutuantes de código, com destaque de sintaxe |
| Config | Gestão da configuração do sistema |
| Dashboard | Visão geral e métricas do sistema |
| Executor | Estado e gestão dos executores |
| Help | Sistema de documentação de ajuda, suporte multilingue |
| Init | Assistente de inicialização para primeira execução |
| Knowledge | Visualização e consulta do grafo de conhecimento |
| Log | Visualizador de logs do sistema, com filtro por Being |
| Memory | Browser de memória de longo prazo, com filtros avançados, estatísticas e vista de detalhes |
| Permission | Gestão de permissões |
| PermissionRequest | Fila de pedidos de permissão |
| Project | Gestão de projetos, com notas de trabalho e sistema de tarefas |
| System | Gestão do sistema e monitorização do runtime |
| Task | Interface do sistema de tarefas |
| Timer | Gestão do sistema de temporizadores, com histórico de execuções |
| Usage | Painel de auditoria de utilização de tokens, com gráficos de tendência e exportação |
| WorkNote | Gestão de notas de trabalho, com pesquisa e geração de índice |

### Atualizações em tempo real

- **SSE (Server-Sent Events)** — Atualizações de mensagens de chat, estado dos Beings e eventos do sistema via `SSEHandler`
- **Sem WebSocket** — Arquitetura mais simples usando SSE para a maioria das necessidades em tempo real
- **Reconexão automática** — Lógica de reconexão do cliente para ligações resilientes

### Localização

O sistema suporta localização completa em **29 variantes linguísticas**:
- **Chinês (6 variantes)**: zh-CN (simplificado), zh-HK (tradicional), zh-SG (Singapura), zh-MO (Macau), zh-TW (Taiwan), zh-MY (Malásia)
- **Inglês (10 variantes)**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY
- **Espanhol (2 variantes)**: es-ES, es-MX
- **Alemão (5 variantes)**: de-DE, de-AT, de-CH, de-LU, de-LI
- **Francês (3 variantes)**: fr-FR, fr-CA, fr-CH
- **Outras (3 variantes)**: ja-JP (Japonês), ko-KR (Coreano), cs-CZ (Checo)

Selecionado através de `DefaultConfigData.Language` e resolvido via `LocalizationManager`.

---

### Sistema de automação do browser WebView

O sistema integra funcionalidades de automação de browser WebView baseadas em **Playwright**:

- **Isolamento individual**: Cada Silicon Being possui uma instância de browser independente, Cookies e armazenamento de sessão, completamente isolados entre si.
- **Modo headless**: O browser corre em modo headless, completamente invisível para o utilizador, com os Beings a operar autonomamente em segundo plano.
- **WebViewBrowserTool**: Fornece capacidades completas de operação do browser, incluindo:
  - Navegação de páginas, cliques, introdução de texto, obtenção de conteúdo da página
  - Execução de JavaScript, obtenção de capturas de ecrã, espera por elementos
  - Gestão do estado do browser e limpeza de recursos
- **Controlo de segurança**: Todas as operações do browser passam pela cadeia de verificação de permissões, impedindo acessos maliciosos a páginas web.

### Sistema de rede de conhecimento

O sistema inclui um grafo de conhecimento incorporado baseado em **estrutura de triplas**:

- **Representação do conhecimento**: Utiliza a estrutura de triplas "sujeito-relação-objeto" (ex: Python-is_a-programming_language)
- **KnowledgeTool**: Fornece gestão do ciclo de vida completo do conhecimento:
  - `add`/`query`/`update`/`delete` - Operações CRUD básicas
  - `search` - Pesquisa de texto completo e correspondência por palavras-chave
  - `get_path` - Descoberta de caminhos de associação entre dois conceitos
  - `validate` - Verificação de integridade do conhecimento
  - `stats` - Estatísticas e análise da rede de conhecimento
- **Armazenamento persistente**: As triplas de conhecimento são persistidas no sistema de ficheiros, com suporte a consultas por índice temporal.
- **Pontuação de confiança**: Cada entrada de conhecimento possui uma pontuação de confiança (0-1), suportando correspondência difusa e ordenação do conhecimento.
- **Classificação por etiquetas**: Suporta a adição de etiquetas ao conhecimento, facilitando a categorização e recuperação.

---

## Estrutura do diretório de dados

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # Ficheiro da alma do Curator
    │   ├── state.json       # Estado do runtime
    │   ├── code.enc         # Código de classe personalizado encriptado com AES
    │   └── permission.enc   # Callback de permissão personalizado encriptado com AES
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```

---

## Motor de armazenamento SpeedyPack

O SiliconLife.Fast utiliza o motor de armazenamento SpeedyPack auto-desenvolvido (formato .spk), substituindo a anterior solução LiteDB, alcançando desempenho extremo de leitura e escrita.

### Design da arquitetura

```
┌──────────────────────────────────────────────────────────┐
│                    SpeedyPack                             │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────────┐  │
│  │ DirectoryMap  │  │  EntryCache   │  │  WriteQueue   │  │
│  │ (mapeamento  │  │  (cache de   │  │ (fila de      │  │
│  │  de diretório │  │   entradas)  │  │  escrita      │  │
│  │  em memória)  │  │              │  │  assíncrona)  │  │
│  └──────┬───────┘  └──────┬───────┘  └───────┬───────┘  │
│         │                  │                   │          │
│  ┌──────▼──────────────────▼───────────────────▼───────┐  │
│  │              PackFileReader / PackFileWriter          │  │
│  │              (leitor/escritor de ficheiros pack)      │  │
│  └──────────────────────────┬──────────────────────────┘  │
│                              │                             │
│  ┌──────────────────────────▼──────────────────────────┐  │
│  │              Ficheiro .spk (MessagePack + compressão LZ4) │  │
│  └─────────────────────────────────────────────────────┘  │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐                      │
│  │  FreeList     │  │ SpeedyPack   │                      │
│  │ (gestão de   │  │ AutoCompactor│                      │
│  │  espaço livre)│  │ (compactação │                      │
│  │              │  │  automática) │                      │
│  └──────────────┘  └──────────────┘                      │
└──────────────────────────────────────────────────────────┘
```

### Componentes principais

| Componente | Descrição |
|------------|-----------|
| `SpeedyPack` | Classe principal, combina DirectoryMap, EntryCache e WriteQueue para fornecer leitura e escrita de baixa latência |
| `DirectoryMap` | Mapeamento de diretório em memória, mantém o mapeamento de caminhos virtuais para entradas de ficheiros |
| `EntryCache` | Cache de entradas, cache de entradas acedidas recentemente baseada em TTL |
| `WriteQueue` | Fila de escrita assíncrona, enfileira operações de escrita para execução em thread de segundo plano |
| `FreeList` | Gestão de espaço livre, rastreia o espaço reutilizável nos ficheiros .spk |
| `PackFileReader` | Leitor de ficheiros pack, lê dados dos ficheiros .spk |
| `PackFileWriter` | Escritor de ficheiros pack, escreve dados nos ficheiros .spk |
| `SpeedyPackAutoCompactor` | Temporizador de compactação automática, compacta periodicamente os ficheiros .spk para recuperar espaço livre |
| `SpeedyPackRegistry` | Gestor singleton ao nível do processo, garante que toda a aplicação usa a mesma instância SpeedyPack |

### Adaptadores de armazenamento

O SiliconLife.Fast integra o SpeedyPack nas interfaces do sistema através dos seguintes adaptadores:

| Adaptador | Interface | Descrição |
|-----------|-----------|-----------|
| `SpeedyStorage` | `IStorage` | Adaptador de armazenamento chave-valor genérico |
| `SpeedyTimeStorage` | `ITimeStorage` | Adaptador de armazenamento com índice temporal |
| `SpeedyWorkNoteStorage` | `IWorkNoteStorage` | Adaptador de armazenamento de notas de trabalho |

### Opções de configuração

`SpeedyPackOptions` fornece as seguintes configurações:

| Opção | Tipo | Valor por defeito | Descrição |
|-------|------|-------------------|-----------|
| `CacheTtl` | `TimeSpan` | 5 minutos | Tempo de vida das entradas na cache |
| `MaxCacheEntries` | `int` | 1000 | Número máximo de entradas na cache |
| `ReadOnly` | `bool` | false | Modo apenas de leitura |

### Suporte a transações

O SpeedyPack suporta operações de escrita atómica através da interface `IPackTransaction`:

- `SpeedyTransaction` implementa o mecanismo de transações
- Suporta atomicidade em escritas em lote
- Na confirmação da transação, todas as operações de escrita são bem-sucedidas ou todas são revertidas

---

## Sistema de plugins

O SiliconLife suporta extensão de funcionalidades através de um sistema de plugins, permitindo que programadores terceiros adicionem novas funcionalidades à plataforma.

### Interface principal

```csharp
public interface IPlugin
{
    string Id { get; }
    string GetName(Language language);
    string Version { get; }
    string GetDescription(Language language);
    string GetAuthor(Language language);
    void OnLoad();
    void OnStart();
    void OnStop();
    void OnUnload();
}
```

### Carregador de plugins

O `PluginLoader` é responsável por carregar DLLs de plugins a partir de um diretório especificado, executando verificações de segurança rigorosas:

1. **Pesquisa de diretório** — Pesquisa todos os ficheiros .dll no diretório de plugins
2. **Verificação de segurança** — Verifica se o plugin referencia namespaces proibidos
3. **Carregamento isolado** — Carrega plugins de forma isolada usando um `AssemblyLoadContext` personalizado
4. **Gestão do ciclo de vida** — Chama os métodos OnLoad, OnStart, OnStop, OnUnload do plugin

### Sandbox de segurança

O carregador de plugins executa as seguintes verificações de segurança:

| Verificação | Descrição |
|-------------|-----------|
| Namespaces proibidos | System.IO, System.Net.Http, System.Net.WebSockets, System.Net.Sockets, Microsoft.CodeAnalysis |
| Lista branca de assemblies confiáveis | Google.Protobuf, Newtonsoft.Json, MessagePack, Serilog, Microsoft.Extensions.Logging.Abstractions, Dapper |
| Verificação de tipos proibidos | Pesquisa de tipos perigosos referenciados no plugin |
| Verificação de membros proibidos | Pesquisa de métodos perigosos chamados no plugin |

### Integração de ferramentas

Os plugins podem registar ferramentas personalizadas implementando a interface `ITool`:

- O método `ToolManager.ScanAllPluginAssemblies()` pesquisa implementações ITool em todos os plugins carregados
- As ferramentas do plugin são automaticamente integradas no ciclo de chamada de ferramentas
- As ferramentas do plugin estão sujeitas ao mesmo sistema de permissões

### Ciclo de vida dos plugins

```
Carregar (OnLoad) → Iniciar (OnStart) → Em execução → Parar (OnStop) → Descarregar (OnUnload)
```

---

## Estados de atividade dos Silicon Beings

Os Silicon Beings possuem os seguintes estados de atividade:

| Estado | Descrição |
|--------|-----------|
| `Idle` | Estado de inatividade, aguardando ativação do relógio |
| `Working` | A executar um ciclo de pedido IA + chamada de ferramenta |
| `Error` | Ocorreu um erro durante a execução |
| `Stopped` | Parado, devido a erros consecutivos ou paragem manual |

**Mecanismo do estado Stopped**:
- Quando um Silicon Being sofre 10 erros consecutivos, entra automaticamente no estado `Stopped`
- Após entrar no estado Stopped, o Being não executa mais nenhuma tarefa
- É necessária intervenção manual para reiniciar

Transições de estado:
```
Idle → Working → Idle (conclusão normal)
Working → Error → Working (recuperação de erro)
Working → Stopped (10 erros consecutivos ou paragem manual)
Stopped → Idle (reinício manual)
```
