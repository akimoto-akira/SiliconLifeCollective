# Arquitectura

> **Versão: v0.2.0-alpha**

[English](../en/architecture.md) | [Deutsch](../de-DE/architecture.md) | [中文](../zh-CN/architecture.md) | [繁體中文](../zh-HK/architecture.md) | [Español](../es-ES/architecture.md) | [日本語](../ja-JP/architecture.md) | [한국어](../ko-KR/architecture.md) | [Čeština](../cs-CZ/architecture.md) | [Русский](../ru-RU/architecture.md)

## Arquitectura de Duas Versões

Este projecto oferece duas versões de implementação que partilham o mesmo desenho de arquitectura, mas diferem no armazenamento e na optimização de desempenho:

### SiliconLife.Default (Versão Padrão)
- **Posicionamento**: Implementação padrão, usada principalmente para verificar a viabilidade da arquitectura
- **Modo de execução**: Aplicação de consola
- **Método de armazenamento**: Armazenamento JSON puro em sistema de ficheiros
- **Cenários aplicáveis**: Cenários com elevados requisitos de segurança de dados, recursos de memória limitados e pequeno volume de dados
- **Descrição do papel**: Como implementação de referência para verificação da arquitectura, fornece uma forma de execução simples e fiável, adequada para primeiro contacto com o projecto, depuração de desenvolvimento ou cenários com prioridade de segurança de dados

### SiliconLife.Fast (Versão de Alto Desempenho)
- **Posicionamento**: Versão de produção recomendada
- **Modo de execução**: Aplicação de ambiente de trabalho (Bandeja do sistema Windows / Janela de estado Linux)
- **Método de armazenamento**: Armazenamento em memória SpeedyPack + persistência assíncrona em lote (formato de ficheiro .spk)
- **Cenários aplicáveis**: Cenários de alta concorrência, baixa latência e grande volume de dados
- **Suporte de plataforma**: Windows/macOS (funcionalidade completa, incluindo bandeja do sistema), Linux (janela de estado, sem ícone de bandeja)
- **Características**:
  - Execução em segundo plano na bandeja do sistema Windows/macOS, com monitorização em tempo real através da janela de estado da bandeja; janela de estado exibida directamente no Linux
  - Motor SpeedyPack + compactação automática garantindo a segurança dos dados
  - Arquitectura Component UI, 27 componentes declarativos
  - 7 temas de skin, com descoberta e troca automáticas
  - Linux abre automaticamente o navegador para aceder ao Web UI, suporte ao parâmetro `--no-tray`
- **Melhoria de desempenho**: Latência de leitura de armazenamento reduzida 1000x, latência de escrita reduzida 15000x
- **Descrição do papel**: Implementação de nível de produção profundamente optimizada, com funcionalidades como execução em segundo plano na bandeja do sistema, motor SpeedyPack + compactação automática, sendo a escolha preferida para execução prolongada e ambientes de produção reais

> **Nota**: A arquitectura descrita neste documento aplica-se a ambas as versões, diferindo apenas na implementação do armazenamento. SiliconLife.Default serve como referência de verificação da arquitectura, enquanto SiliconLife.Fast é a versão de produção recomendada.

---

## Conceitos Principais

### Silicon Being

Cada agente de IA no sistema é um **Silicon Being** — uma entidade autónoma com a sua própria identidade, personalidade e capacidades. Cada Silicon Being é orientado por um **Ficheiro da Alma** (prompt em Markdown) que define os seus padrões de comportamento.

### Silicon Curator

O **Silicon Curator** é um Silicon Being especial com as mais altas permissões do sistema. Actua como administrador do sistema:

- Cria e gere outros Silicon Beings
- Analisa pedidos dos utilizadores e decompõe-os em tarefas
- Distribui tarefas aos Silicon Beings adequados
- Monitoriza a qualidade da execução e trata falhas
- Responde a mensagens dos utilizadores usando **escalonamento prioritário** (ver abaixo)

### Ficheiro da Alma

Um ficheiro Markdown (`soul.md`) armazenado no directório de dados de cada Silicon Being. É injectado como prompt do sistema em cada pedido de IA, definindo a personalidade, padrões de decisão e restrições de comportamento do being.

---

## Escalonamento: Escalonamento Justo por Fatias de Tempo

### Ciclo Principal + Objectos Tick

O sistema executa um **ciclo principal orientado por relógio** numa thread dedicada em segundo plano:

```
Ciclo Principal (thread dedicado, watchdog + circuit breaker)
  └── Objecto Tick A (prioridade=0, intervalo=100ms)
  └── Objecto Tick B (prioridade=1, intervalo=500ms)
  └── Silicon Being Manager (activado por relógio directamente pelo ciclo principal)
        └── Silicon Being Runner → Silicon Being 1 → Activado por relógio → Executa uma ronda
        └── Silicon Being Runner → Silicon Being 2 → Activado por relógio → Executa uma ronda
        └── Silicon Being Runner → Silicon Being 3 → Activado por relógio → Executa uma ronda
        └── ...
```

Decisões de desenho chave:

- **Os Silicon Beings não herdam de objectos Tick.** Têm o seu próprio método `Tick()`, chamado pelo `SiliconBeingManager` através do `SiliconBeingRunner`, em vez de se registarem directamente no ciclo principal.
- O **Silicon Being Manager** é activado por relógio directamente pelo ciclo principal e actua como único proxy para todos os beings.
- O **Silicon Being Runner** envolve o `Tick()` de cada being numa thread temporária, com timeout e circuit breaker por being (3 timeouts consecutivos → 1 minuto de arrefecimento).
- A execução de cada being é limitada a **uma ronda** de pedido de IA + chamadas de ferramentas por activação do relógio, garantindo que nenhum being monopolize o ciclo principal.
- O **Monitor de Desempenho** rastreia os tempos de execução do relógio para observabilidade.

### Resposta Prioritária do Curator

Quando um utilizador envia uma mensagem ao Silicon Curator:

1. O being actual (por exemplo, Being A) completa a sua ronda actual — **sem interrupção**.
2. O gestor **salta a fila restante**.
3. O ciclo **recomeça pelo Curator**, fazendo-o executar imediatamente.

Isto garante responsividade às interacções do utilizador sem perturbar as tarefas em curso.

---

## Arquitectura de Componentes

```
┌─────────────────────────────────────────────────────────┐
│                        Core Host                         │
│  (Host unificado — monta e gere todos os componentes)    │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │Ciclo      │  │Localizador   │  │   Configuração    │  │
│  │Principal  │  │de Serviços   │  │                   │  │
│  └────┬─────┘  └──────────────┘  └──────────────────┘  │
│       │                                                  │
│  ┌────▼─────────────────────────────────────────────┐   │
│  │        Silicon Being Manager (Objecto Tick)        │   │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐            │   │
│  │  │Curator   │ │Being A  │ │Being B  │  ...       │   │
│  │  └────┬────┘ └────┬────┘ └────┬────┘            │   │
│  └───────┼───────────┼───────────┼──────────────────┘   │
│          │           │           │                      │
│  ┌───────▼───────────▼───────────▼──────────────────┐   │
│  │           Serviços Partilhados                     │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Sistema   │  │Armaz.    │  │Gestor de         │  │   │
│  │  │de Chat   │  │          │  │Permissões        │  │   │
│  │  └──────────┘ └────┬─────┘ └──────────────────┘  │   │
│  │                   │                               │   │
│  │  ┌──────────┐ ┌────▼─────┐ ┌──────────────────┐  │   │
│  │  │Cliente   │  │Executors │  │Gestor de         │  │   │
│  │  │de IA     │  │          │  │Ferramentas       │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  │  ┌──────────┐ ┌──────────┐                        │   │
│  │  │Carregador│  │Rede de   │                        │   │
│  │  │de Plugins│  │Conhecim. │                        │   │
│  │  └──────────┘ └──────────┘                        │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │                   Executors                        │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Executor  │  │Executor  │  │Executor de       │  │   │
│  │  │de Disco  │  │de Rede   │  │Linha Comandos    │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │           Fornecedores de IM                       │   │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────────────┐  │   │
│  │  │Fornecedor│  │Fornecedor│  │Fornecedor       │  │   │
│  │  │Consola   │  │Web       │  │Feishu / ...     │  │   │
│  │  └──────────┘ └──────────┘ └──────────────────┘  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

---

## Localizador de Serviços

O `ServiceLocator` é um registo singleton thread-safe que fornece acesso a todos os serviços principais:

| Propriedade | Tipo | Descrição |
|----------|------|-------------|
| `ChatSystem` | `ChatSystem` | Gestor central de sessões de chat |
| `IMManager` | `IMManager` | Encaminhador de fornecedores de mensagens instantâneas |
| `AuditLogger` | `AuditLogger` | Registo de auditoria de permissões |
| `GlobalAcl` | `GlobalACL` | Lista de Controlo de Acesso Global |
| `BeingFactory` | `ISiliconBeingFactory` | Fábrica para criar beings |
| `BeingManager` | `SiliconBeingManager` | Gestor de ciclo de vida dos beings activos |
| `DynamicBeingLoader` | `DynamicBeingLoader` | Carregador de compilação dinâmica |
| `TokenUsageAudit` | `ITokenUsageAudit` | Registo de utilização de tokens |
| `TokenUsageAuditManager` | `TokenUsageAuditManager` | Relatórios de utilização de tokens |

Também mantém um registo de `PermissionManager` por being, indexado pelo GUID do being.

---

## Sistema de Chat

### Tipos de Sessão

O sistema de chat suporta três tipos de sessão através de `SessionBase`:

| Tipo | Classe | Descrição |
|------|-------|-------------|
| `SingleChat` | `SingleChatSession` | Conversa um-a-um entre dois participantes |
| `GroupChat` | `GroupChatSession` | Chat de grupo com múltiplos participantes |
| `Broadcast` | `BroadcastChannel` | Canal aberto com ID fixo; os beings subscrevem dinamicamente, recebendo mensagens apenas após a subscrição |

### Canal de Difusão

O `BroadcastChannel` é um tipo especial de sessão usado para anúncios em todo o sistema:

- **ID de canal fixo** — Ao contrário de `SingleChatSession` e `GroupChatSession`, o ID do canal é uma constante conhecida, não derivada dos GUIDs dos membros.
- **Subscrição dinâmica** — Os beings subscrevem/cancelam a subscrição em tempo de execução; só recebem mensagens publicadas após a subscrição.
- **Filtragem de mensagens pendentes** — `GetPendingMessages()` retorna apenas mensagens publicadas após o tempo de subscrição do being e que ainda não foram lidas.
- **Gerido pelo sistema de chat** — `GetOrCreateBroadcastChannel()`, `Broadcast()`, `GetPendingBroadcasts()`.

### Mensagem de Chat

O modelo `ChatMessage` contém campos para o contexto de conversação da IA e rastreamento de tokens:

| Campo | Tipo | Descrição |
|-------|------|-------------|
| `Id` | `Guid` | Identificador único da mensagem |
| `SenderId` | `Guid` | Identificador único do remetente |
| `ChannelId` | `Guid` | Identificador do canal/conversa |
| `Content` | `string` | Conteúdo da mensagem |
| `Timestamp` | `DateTime` | Hora de envio da mensagem |
| `Type` | `MessageType` | Texto, imagem, ficheiro ou notificação do sistema |
| `ReadBy` | `List<Guid>` | IDs dos participantes que leram esta mensagem |
| `Role` | `MessageRole` | Papel na conversação da IA (utilizador, assistente, ferramenta) |
| `ToolCallId` | `string?` | ID da chamada de ferramenta para mensagens de resultado |
| `ToolCallsJson` | `string?` | JSON serializado de chamadas de ferramentas para mensagens do assistente |
| `Thinking` | `string?` | Raciocínio da cadeia de pensamento da IA |
| `PromptTokens` | `int?` | Número de tokens no prompt (entrada) |
| `CompletionTokens` | `int?` | Número de tokens na conclusão (saída) |
| `TotalTokens` | `int?` | Número total de tokens utilizados (entrada + saída) |
| `FileMetadata` | `FileMetadata?` | Metadados do ficheiro anexado (se a mensagem contiver um ficheiro) |

### Fila de Mensagens de Chat

O `ChatMessageQueue` é um sistema de fila de mensagens thread-safe para gerir o processamento assíncrono de mensagens de chat:

- **Thread-safe** - Utiliza mecanismos de bloqueio para garantir segurança no acesso concorrente
- **Processamento assíncrono** - Suporta enfileiramento e desenfileiramento assíncrono de mensagens
- **Ordenação de mensagens** - Mantém a ordem temporal das mensagens
- **Operações em lote** - Suporta obtenção de mensagens em lote

### Metadados de Ficheiro

O `FileMetadata` gere informações de ficheiros anexados a mensagens de chat:

- **Informações do ficheiro** - Nome, tamanho, tipo, caminho do ficheiro
- **Hora de carregamento** - Timestamp do carregamento do ficheiro
- **Carregador** - ID do utilizador ou Silicon Being que carregou o ficheiro

### Gestor de Cancelamento de Stream

O `StreamCancellationManager` fornece um mecanismo de cancelamento para respostas de stream da IA:

- **Controlo de stream** - Suporta cancelamento de respostas de stream da IA em curso
- **Limpeza de recursos** - Limpeza correcta dos recursos associados ao cancelar
- **Segurança concorrente** - Suporta a gestão de múltiplos streams simultaneamente

### Visualização do Histórico de Chat

A funcionalidade de visualização do histórico de chat permite aos utilizadores navegar nas conversas anteriores dos Silicon Beings:

- **Lista de sessões** - Mostra todas as sessões históricas
- **Detalhes das mensagens** - Visualizar o histórico completo de mensagens
- **Vista de linha temporal** - Apresentar mensagens em ordem cronológica
- **Suporte API** - Fornece API RESTful para obter dados de sessões e mensagens

---

## Sistema de Clientes de IA

O sistema suporta múltiplos backends de IA através da interface `IAIClient`:

### OllamaClient

- **Tipo**: Serviço de IA local
- **Protocolo**: API HTTP nativa do Ollama (`/api/chat`, `/api/generate`)
- **Funcionalidades**: Streaming, chamadas de ferramentas, alojamento de modelos locais
- **Configuração**: `endpoint`, `model`, `temperature`, `maxTokens`

### DashScopeClient (Alibaba Cloud DashScope)

- **Tipo**: Serviço de IA na nuvem
- **Protocolo**: API compatível com OpenAI (`/compatible-mode/v1/chat/completions`)
- **Autenticação**: Bearer token (chave API)
- **Funcionalidades**: Streaming, chamadas de ferramentas, conteúdo de raciocínio (cadeia de pensamento), implantação em múltiplas regiões
- **Regiões suportadas**:
  - `beijing` — Norte da China 2 (Pequim)
  - `virginia` — EUA (Virgínia)
  - `singapore` — Singapura
  - `hongkong` — Hong Kong, China
  - `frankfurt` — Alemanha (Frankfurt)
- **Modelos suportados** (descobertos dinamicamente via API, com lista de fallback):
  - **Série Qwen**: qwen3-max, qwen3.6-plus, qwen3.6-flash, qwen-max, qwen-plus, qwen-turbo, qwen3-coder-plus
  - **Raciocínio**: qwq-plus
  - **Terceiros**: deepseek-v3.2, deepseek-r1, glm-5.1, kimi-k2.5, llama-4-maverick
- **Configuração**: `apiKey`, `region`, `model`
- **Descoberta de modelos**: Obtém modelos disponíveis da API DashScope em tempo de execução; em caso de falha de rede, recorre a uma lista curada

### VolcengineArkClient (Volcengine Ark)

- **Tipo**: Serviço de IA na nuvem
- **Protocolo**: API compatível com OpenAI
- **Autenticação**: Bearer token (chave API)
- **Funcionalidades**: Suporta modos de streaming e não-streaming, com controlo de taxa de dupla camada integrado
  - Auto-controlo de taxa: Impõe um intervalo mínimo entre pedidos
  - Limite de taxa do servidor: Trata erros 429 com retry com backoff exponencial
- **Configuração**: `apiKey`, `endpoint`, `model`
- **Características**: Serviço de IA da ByteDance, suporta múltiplos modelos Doubao

### DeepSeekClient

- **Tipo**: Serviço de IA na nuvem
- **Protocolo**: API compatível com OpenAI (`https://api.deepseek.com`)
- **Autenticação**: Bearer token (chave API)
- **Funcionalidades**: Streaming, chamadas de ferramentas, modo thinking (reasoning_content), parâmetro reasoning_effort
- **Janela de contexto**: 1.048.576 tokens
- **Configuração**: `apiKey`, `model`

### ZhipuClient (智谱 GLM)

- **Tipo**: Serviço de IA na nuvem
- **Protocolo**: API compatível com OpenAI (`https://open.bigmodel.cn/api/paas/v4`)
- **Autenticação**: Bearer token (chave API)
- **Funcionalidades**: Streaming, chamadas de ferramentas, modo thinking, suporte a visão por modelo
- **Janela de contexto**: 1.048.576 tokens
- **Configuração**: `apiKey`, `model`

### ErnieClient (Baidu Qianfan/Wenxin)

- **Tipo**: Serviço de IA na nuvem
- **Protocolo**: API compatível com OpenAI (`https://qianfan.baidubce.com/v2`)
- **Autenticação**: Bearer token (chave API)
- **Funcionalidades**: Streaming, chamadas de ferramentas, suporte a visão por modelo
- **Janela de contexto**: 131.072 tokens
- **Configuração**: `apiKey`, `model`

### HunyuanClient (Tencent Hunyuan)

- **Tipo**: Serviço de IA na nuvem
- **Protocolo**: API compatível com OpenAI (duplo endpoint: TokenHub recomendado + Legacy `https://api.hunyuan.cloud.tencent.com/v1`)
- **Autenticação**: Bearer token (chave API)
- **Funcionalidades**: Streaming, chamadas de ferramentas por modelo, sem suporte a visão
- **Janela de contexto**: 262.144 tokens
- **Modelos suportados**: hy3 (recomendado), hy3-preview
- **Configuração**: `apiKey`, `model`

### MiniMaxClient

- **Tipo**: Serviço de IA na nuvem
- **Protocolo**: API compatível com OpenAI (`https://api.minimaxi.com/v1`)
- **Autenticação**: Bearer token (chave API)
- **Funcionalidades**: Streaming, chamadas de ferramentas, suporte a visão por modelo
- **Janela de contexto**: 1.048.576 tokens
- **Configuração**: `apiKey`, `model`

### MoonshotClient (Moonshot/Kimi)

- **Tipo**: Serviço de IA na nuvem
- **Protocolo**: API compatível com OpenAI (`https://api.moonshot.cn/v1`)
- **Autenticação**: Bearer token (chave API)
- **Funcionalidades**: Streaming, chamadas de ferramentas, suporte a visão por modelo
- **Janela de contexto**: 262.144 tokens
- **Configuração**: `apiKey`, `model`

### SiliconFlowClient (SiliconFlow)

- **Tipo**: Serviço de IA na nuvem (plataforma de agregação)
- **Protocolo**: API compatível com OpenAI (`https://api.siliconflow.cn/v1`)
- **Autenticação**: Bearer token (chave API)
- **Funcionalidades**: Streaming, chamadas de ferramentas, suporte a visão por modelo, obtenção dinâmica da lista de modelos disponíveis (API /models)
- **Janela de contexto**: 1.048.576 tokens
- **Configuração**: `apiKey`, `model`

### Padrão de Fábrica de Clientes

Cada tipo de cliente de IA tem uma implementação de fábrica correspondente `IAIClientFactory`:

- `OllamaClientFactory` — Cria instâncias de OllamaClient
- `DashScopeClientFactory` — Cria instâncias de DashScopeClient
- `VolcengineArkClientFactory` — Cria instâncias de VolcengineArkClient
- `HerdsmanClientFactory` — Cria instâncias de HerdsmanClient
- `LongCatClientFactory` — Cria instâncias de LongCatClient
- `QiniuAIClientFactory` — Cria instâncias de QiniuAIClient
- `DeepSeekClientFactory` — Cria instâncias de DeepSeekClient
- `ZhipuClientFactory` — Cria instâncias de ZhipuClient
- `ErnieClientFactory` — Cria instâncias de ErnieClient
- `HunyuanClientFactory` — Cria instâncias de HunyuanClient
- `MiniMaxClientFactory` — Cria instâncias de MiniMaxClient
- `MoonshotClientFactory` — Cria instâncias de MoonshotClient
- `SiliconFlowClientFactory` — Cria instâncias de SiliconFlowClient

As fábricas fornecem:
- `CreateClient(Dictionary<string, object> config)` — Instancia o cliente a partir da configuração
- `GetConfigKeyOptions(string key, ...)` — Retorna opções dinâmicas para a chave de configuração (por exemplo, modelos disponíveis, regiões)
- `GetDisplayName()` — Nome de exibição localizado do tipo de cliente

### Interface de Capacidades IAIClient

A interface `IAIClient` define propriedades de declaração de capacidades do cliente de IA, com base nas quais o `ContextManager` ajusta adaptativamente o comportamento:

| Propriedade | Tipo | Descrição |
|------|------|------|
| `StreamingMode` | `bool?` | Suporte ao modo de streaming: true=apenas streaming, false=apenas não-streaming, null=ambos suportados (predefinição: streaming) |
| `SupportsToolCalls` | `bool?` | Suporte a chamadas de ferramentas: true=suportado, false=não suportado (ignorar injeção de ferramentas), null=desconhecido (predefinição: suportado) |
| `ContextWindowTokens` | `int?` | Tamanho da janela de contexto (número de tokens), usado para corte de orçamento de tokens em vez de MaxContextMessages fixo |
| `SupportsVision` | `bool?` | Suporte a entrada visual: true=suporta imagens, false=não suporta, null=desconhecido (predefinição: não suporta) |
| `SupportsAudio` | `bool?` | Suporte a entrada de áudio: true=suporta áudio, false=não suporta, null=desconhecido (predefinição: não suporta) |

### Lista de Suporte de Plataformas de IA

#### Legenda de Estados
- ✅ Implementado
- 🚧 Em desenvolvimento
- 📋 Planeado
- 💡 Em consideração
- ⚠️ Obsoleto

*Nota: Devido ao ambiente de rede do desenvolvedor, a integração com serviços de IA na nuvem estrangeiros [em consideração] pode exigir ferramentas de proxy de rede para acesso, e o processo de depuração pode ser instável.*

#### Lista de Plataformas

| Plataforma | Estado | Tipo | Descrição |
|------|------|------|------|
| Ollama | ✅ | Local | Serviço de IA local, suporta implantação de modelos locais |
| DashScope (Alibaba Cloud) | ✅ | Nuvem | Serviço de IA Alibaba Cloud DashScope, suporta implantação em múltiplas regiões |
| Baidu Qianfan (Wenxin) | ✅ | Nuvem | Serviço de IA Baidu Wenxin — ErnieClient |
| Zhipu AI (GLM) | ✅ | Nuvem | Serviço de IA Zhipu Qingyan — ZhipuClient |
| Moonshot (Kimi) | ✅ | Nuvem | Serviço de IA Moonshot Kimi — MoonshotClient |
| Volcengine Ark (Doubao) | ✅ | Nuvem | Serviço de IA Doubao da ByteDance |
| Herdsman | ✅ | Local/Nuvem | Motor de inferência sem autenticação, compatível com o formato OpenAI API |
| Meituan LongCat | ✅ | Nuvem | Modelo grande auto-desenvolvido da Meituan, compatível com o formato OpenAI API, autenticação por API Key |
| Qiniu Cloud AI | ✅ | Nuvem | Serviço de inferência de modelo grande da Qiniu Cloud, compatível com o formato OpenAI API, autenticação por API Key |
| DeepSeek (Directo) | ✅ | Nuvem | Serviço de IA DeepSeek — DeepSeekClient, suporta modo thinking |
| 01.AI (Yi) | ⚠️ | Nuvem | Serviço de IA 01.AI (Obsoleto: registo de novos utilizadores interrompido) |
| Tencent Hunyuan | ✅ | Nuvem | Serviço de IA Tencent Hunyuan — HunyuanClient, duplo endpoint TokenHub/Legacy |
| SiliconFlow | ✅ | Nuvem | Serviço de IA SiliconFlow — SiliconFlowClient, suporta lista dinâmica de modelos |
| MiniMax | ✅ | Nuvem | Serviço de IA MiniMax — MiniMaxClient |
| OpenAI | 💡 | Nuvem | Serviço OpenAI API (série GPT) |
| Anthropic | 💡 | Nuvem | Serviço de IA Anthropic Claude |
| Google DeepMind | 💡 | Nuvem | Serviço de IA Google Gemini |
| Mistral AI | 💡 | Nuvem | Serviço de IA Mistral |
| Groq | 💡 | Nuvem | Serviço de inferência de IA de alta velocidade Groq |
| Together AI | 💡 | Nuvem | Serviço de modelos open source Together AI |
| xAI | 💡 | Nuvem | Serviço xAI Grok |
| Cohere | 💡 | Nuvem | Serviço de NLP empresarial Cohere |
| Replicate | 💡 | Nuvem | Plataforma de alojamento de modelos open source Replicate |
| Hugging Face | 💡 | Nuvem | Comunidade e plataforma de modelos de IA open source Hugging Face |
| Cerebras | 💡 | Nuvem | Serviço de inferência de IA optimizada Cerebras |
| Databricks | 💡 | Nuvem | Plataforma de IA empresarial Databricks (MosaicML) |
| Perplexity AI | 💡 | Nuvem | Serviço de pesquisa e Q&A Perplexity AI |
| NVIDIA NIM | 💡 | Nuvem | Microsserviço de inferência de IA NVIDIA |

---

## Decisões de Desenho Chave

### Armazenamento como Instância (não Estática)

O `IStorage` é desenhado como uma instância injectável, não como um utilitário estático. Isto garante:

- Acesso directo ao sistema de ficheiros — IStorage é o canal de persistência interno do sistema, **não** encaminhado através de executores.
- **A IA não controla o IStorage** — Os executores gerem o IO iniciado por ferramentas de IA; o IStorage gere a leitura/escrita interna dos próprios dados do framework. Estas são preocupações fundamentalmente diferentes.
- Testabilidade com implementações simuladas.
- Suporte futuro para diferentes backends de armazenamento sem modificar os consumidores.

### Executors como Fronteira de Segurança

Os executores são o **único** caminho para operações de I/O. Ferramentas que necessitam de acesso a disco, rede ou linha de comandos **devem** passar pelos executores. Este desenho impõe:

- Cada executor tem uma **thread de despacho independente**, com bloqueio para verificação de permissões.
- Verificação centralizada de permissões — Os executores consultam o **Gestor de Permissões privado** do being.
- Fila de pedidos com suporte a prioridade e controlo de timeout.
- Registo de auditoria de todas as operações externas.
- Isolamento de excepções — A falha de um executor não afecta os outros.
- Circuit breaker — Falhas consecutivas param temporariamente o executor para prevenir falhas em cascata.

### ContextManager como Objecto Leve

Cada `ExecuteOneRound()` cria uma nova instância de `ContextManager`:

1. Carrega o Ficheiro da Alma + histórico recente de chat.
2. Envia o pedido ao cliente de IA.
3. Processa chamadas de ferramentas em ciclo até a IA retornar texto puro.
4. Persiste a resposta no sistema de chat.
5. Libertação.

Isto mantém cada ronda isolada e sem estado.

### Auto-evolução por Substituição de Classes

Os Silicon Beings podem reescrever as suas próprias classes C# em tempo de execução:

1. A IA gera novo código de classe (deve herdar de `SiliconBeingBase`).
2. **Controlo de referências na compilação** (defesa primária): O compilador recebe apenas a lista de assemblies permitidos — `System.IO`, `System.Reflection`, etc. são excluídos, tornando código perigoso impossível ao nível do tipo.
3. **Análise estática em tempo de execução** (defesa secundária): O `SecurityScanner` analisa o código em busca de padrões perigosos após a compilação bem-sucedida.
4. O Roslyn compila o código em memória.
5. Em caso de sucesso: `SiliconBeingManager.ReplaceBeing()` troca a instância actual, migra o estado e persiste o código encriptado no disco.
6. Em caso de falha: O novo código é descartado, mantendo a implementação existente.

Implementações personalizadas de `IPermissionCallback` também podem ser compiladas e injectadas via `ReplacePermissionCallback()`, permitindo que os beings personalizem a sua própria lógica de permissões.

O código é armazenado no disco com encriptação AES-256. A chave de encriptação é derivada do GUID do being (maiúsculas) via PBKDF2.

---

## Auditoria de Utilização de Tokens

O `TokenUsageAuditManager` rastreia o consumo de tokens de IA de todos os beings:

- `TokenUsageRecord` — Registo por pedido (ID do being, modelo, tokens do prompt, tokens de conclusão, timestamp)
- `TokenUsageSummary` — Estatísticas agregadas
- `TokenUsageQuery` — Parâmetros de consulta para filtrar registos
- Persistido via `ITimeStorage` para consultas de séries temporais
- Acessível via Web UI (UsageController) e `TokenAuditTool` (apenas Curator)

---

### Sistema de Calendário

O sistema inclui **32 implementações de calendário**, derivadas da classe abstracta `CalendarBase`, cobrindo os principais sistemas de calendário do mundo:

| Calendário | ID | Descrição |
|----------|-----|-------------|
| BuddhistCalendar | `buddhist` | Calendário Budista (BE), ano + 543 |
| CherokeeCalendar | `cherokee` | Sistema de calendário Cherokee |
| ChineseLunarCalendar | `lunar` | Calendário Lunar Chinês, com meses intercalares |
| ChineseHistoricalCalendar | `chinese_historical` | Calendário Histórico Chinês, suporta era Ganzhi e era imperial |
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
| JapaneseCalendar | `japanese` | Calendário de Era Japonesa (Nengo) |
| JavaneseCalendar | `javanese` | Calendário Islâmico Javanês |
| JucheCalendar | `juche` | Calendário Juche (Coreia do Norte), ano - 1911 |
| JulianCalendar | `julian` | Calendário Juliano |
| KhmerCalendar | `khmer` | Calendário Khmer |
| MayanCalendar | `mayan` | Contagem Longa Maia |
| MongolianCalendar | `mongolian` | Calendário Mongol |
| PersianCalendar | `persian` | Calendário Persa (Hijri Solar) |
| RepublicOfChinaCalendar | `roc` | Calendário da República da China, ano - 1911 |
| RomanCalendar | `roman` | Calendário Romano |
| SakaCalendar | `saka` | Calendário Saka (Indonésia) |
| SexagenaryCalendar | `sexagenary` | Calendário Sexagenário Chinês (Ganzhi) |
| TibetanCalendar | `tibetan` | Calendário Tibetano |
| VietnameseCalendar | `vietnamese` | Calendário Lunar Vietnamita (variante do zodíaco do gato) |
| VikramSamvatCalendar | `vikram_samvat` | Calendário Vikram Samvat |
| YiCalendar | `yi` | Sistema de calendário Yi |
| ZoroastrianCalendar | `zoroastrian` | Calendário Zoroastriano |

O `CalendarTool` fornece operações: `now`, `format`, `add_days`, `diff`, `list_calendars`, `get_components`, `get_now_components`, `convert` (conversão de datas entre calendários).

---

## Arquitectura da Web UI

### Sistema de Skins

A Web UI possui um **sistema de skins plugável** que permite personalização completa da UI sem alterar a lógica da aplicação:

- **Interface ISkin** — Define o contrato para todas as skins, incluindo:
  - Métodos de renderização principal (`RenderHtml`, `RenderError`)
  - 20+ métodos de componentes UI (botões, inputs, cards, tabelas, badges, bubbles, progresso, tabs, etc.)
  - Geração de CSS temático via `CssBuilder`
  - `SkinPreviewInfo` — Paleta de cores e ícone para o selector de skins na página inicial

- **Skins incorporadas** — 7 skins prontas para produção:
  - **Admin** — Interface de administração do sistema profissional e focada em dados
  - **Chat** — Desenho conversacional e centrado em mensagens para interacção com IA
  - **Creative** — Layout de fluxo de trabalho criativo, artístico e visualmente rico
  - **Dev** — Interface centrada no programador e no código, com destaque de sintaxe
  - **HighContrast** — Tema de acessibilidade de alto contraste
  - **Light** — Tema claro e limpo
  - **Minimal** — Tema minimalista

- **Descoberta de skins** — O `SkinManager` descobre e regista automaticamente todas as implementações de `ISkin` via reflexão

### Construtores de HTML / CSS / JS

A Web UI evita completamente ficheiros de template, gerindo toda a marcação em C#:

- **`H`** — DSL de construtor de HTML em streaming, para construir árvores HTML em código
- **`CssBuilder`** — Construtor de CSS com suporte a selectores e media queries
- **`JsBuilder` (`JsSyntax`)** — Construtor de JavaScript para scripts inline

### Sistema de Controladores

A Web UI segue um **padrão tipo MVC**, com 24 controladores a tratar diferentes aspectos:

| Controlador | Propósito |
|------------|---------|
| About | Página sobre e informações do projecto |
| Audit | Painel de auditoria de utilização de tokens |
| Being | Gestão e estado dos Silicon Beings |
| Chat | Interface de chat em tempo real com SSE |
| ChatHistory | Visualização do histórico de chat, com lista de sessões e detalhes de mensagens |
| CodeBrowser | Visualização e edição de código |
| CodeHover | Dicas flutuantes de código, com destaque de sintaxe |
| Config | Gestão da configuração do sistema |
| Dashboard | Visão geral e métricas do sistema |
| Executor | Estado e gestão dos executores |
| Help | Sistema de documentação de ajuda, suporte multilingue |
| Init | Assistente de inicialização para primeira execução |
| Knowledge | Visualização e consulta do grafo de conhecimento |
| Log | Visualizador de registos do sistema, com filtro por Silicon Being |
| Memory | Navegador de memória de longo prazo, com filtragem avançada, estatísticas e vista de detalhes |
| Permission | Gestão de permissões |
| PermissionRequest | Fila de pedidos de permissão |
| Project | Gestão de projectos, incluindo notas de trabalho, sistema de tarefas e permissões de ferramentas |
| System | Administração do sistema e monitorização em tempo de execução |
| Task | Interface do sistema de tarefas |
| Timer | Gestão do sistema de temporizadores, incluindo histórico de execução |
| ToolPermission | Gestão de permissões de ferramentas, suporta configuração de permissões ao nível do Silicon Being e do projecto |
| Usage | Painel de auditoria de utilização de tokens, com gráficos de tendência e exportação |
| WorkNote | Gestão de notas de trabalho, com pesquisa e geração de directório |

### Actualizações em Tempo Real

- **SSE (Server-Sent Events)** — Envia actualizações de mensagens de chat, estado dos beings e eventos do sistema via `SSEHandler`
- **Sem WebSocket** — Arquitectura mais simples usando SSE para a maioria das necessidades em tempo real
- **Reconexão automática** — Lógica de reconexão do cliente para ligações resilientes

### Localização

O sistema suporta localização completa em **34 variantes linguísticas**:
- **Chinês (6 variantes)**: zh-CN (simplificado), zh-HK (tradicional), zh-SG (Singapura), zh-MO (Macau), zh-TW (Taiwan), zh-MY (Malásia)
- **Inglês (10 variantes)**: en-US, en-GB, en-CA, en-AU, en-IN, en-SG, en-ZA, en-IE, en-NZ, en-MY
- **Espanhol (2 variantes)**: es-ES, es-MX
- **Alemão (5 variantes)**: de-DE, de-AT, de-CH, de-LU, de-LI
- **Francês (3 variantes)**: fr-FR, fr-CA, fr-CH
- **Outros (8 variantes)**: ja-JP (japonês), ko-KR (coreano), cs-CZ (checo), it-IT (italiano), pl-PL (polaco), pt-PT (português), pt-BR (português brasileiro), ru-RU (russo)

O ambiente linguístico activo é seleccionado via `DefaultConfigData.Language` e resolvido pelo `LocalizationManager`.

---

### Sistema de Automação de Navegador WebView (Novo)

O sistema integra funcionalidades de automação de navegador WebView baseadas em **Playwright**:

- **Isolamento individual**: Cada Silicon Being possui uma instância de navegador independente, cookies e armazenamento de sessão, completamente isolados entre si.
- **Modo headless**: O navegador funciona em modo headless, completamente invisível para o utilizador, com os Silicon Beings a operar autonomamente em segundo plano.
- **WebViewBrowserTool**: Fornece capacidades completas de operação do navegador, incluindo:
  - Navegação de páginas, cliques, introdução de texto, obtenção de conteúdo da página
  - Execução de JavaScript, obtenção de capturas de ecrã, espera pelo aparecimento de elementos
  - Gestão do estado do navegador e limpeza de recursos
- **Controlo de segurança**: Todas as operações do navegador passam pela cadeia de verificação de permissões, impedindo acessos maliciosos a páginas web.

### Sistema de Rede de Conhecimento (Novo)

O sistema inclui um sistema de grafo de conhecimento baseado em **estrutura de triplas**:

- **Representação de conhecimento**: Utiliza estrutura de triplas "sujeito-relação-objecto" (por exemplo: Python-é_uma-linguagem_de_programação)
- **KnowledgeTool**: Fornece gestão do ciclo de vida completo do conhecimento:
  - `add`/`query`/`update`/`delete` - Operações CRUD básicas
  - `search` - Pesquisa de texto completo e correspondência por palavras-chave
  - `get_path` - Descoberta de caminhos de associação entre dois conceitos
  - `validate` - Verificação de integridade do conhecimento
  - `stats` - Estatísticas da rede de conhecimento
- **Armazenamento persistente**: As triplas de conhecimento são persistidas no sistema de ficheiros, com suporte para consultas por índice temporal.
- **Pontuação de confiança**: Cada entrada de conhecimento tem uma pontuação de confiança (0-1), suportando correspondência difusa e ordenação do conhecimento.
- **Classificação por tags**: Suporta a adição de tags ao conhecimento, facilitando a categorização e recuperação.

---

## Estrutura do Directório de Dados

```
data/
└── SiliconManager/
    ├── {curator-guid}/
    │   ├── soul.md          # Ficheiro da Alma do Curator
    │   ├── state.json       # Estado em tempo de execução
    │   ├── code.enc         # Código de classe personalizado encriptado com AES
    │   └── permission.enc   # Callback de permissões personalizado encriptado com AES
    │
    └── {being-guid}/
        ├── soul.md
        ├── state.json
        ├── code.enc
        └── permission.enc
```

---

## Motor de Armazenamento SpeedyPack

O SiliconLife.Fast utiliza o motor de armazenamento SpeedyPack próprio (formato .spk), substituindo a solução LiteDB anterior, alcançando desempenho de leitura/escrita extremo.

### Desenho de Arquitectura

```
┌──────────────────────────────────────────────────────────┐
│                    SpeedyPack                             │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────────┐  │
│  │ DirectoryMap  │  │  EntryCache   │  │  WriteQueue   │  │
│  │ (Mapeamento   │  │  (Cache de    │  │ (Fila de      │  │
│  │  de directó-  │  │   entradas)   │  │  escrita      │  │
│  │  rios em      │  │              │  │  assíncrona)  │  │
│  │  memória)     │  │              │  │              │  │
│  └──────┬───────┘  └──────┬───────┘  └───────┬───────┘  │
│         │                  │                   │          │
│  ┌──────▼──────────────────▼───────────────────▼───────┐  │
│  │              PackFileReader / PackFileWriter          │  │
│  │              (Leitor/Escritor de ficheiros de pacote) │  │
│  └──────────────────────────┬──────────────────────────┘  │
│                              │                             │
│  ┌──────────────────────────▼──────────────────────────┐  │
│  │              Ficheiro .spk (MessagePack + LZ4)        │  │
│  └─────────────────────────────────────────────────────┘  │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐                      │
│  │  FreeList     │  │ SpeedyPack   │                      │
│  │ (Gestão de    │  │ AutoCompactor│                      │
│  │  espaço livre)│  │ (Compactação │                      │
│  │              │  │  automática) │                      │
│  └──────────────┘  └──────────────┘                      │
└──────────────────────────────────────────────────────────┘
```

### Componentes Principais

| Componente | Descrição |
|------|------|
| `SpeedyPack` | Classe principal, combina DirectoryMap, EntryCache e WriteQueue para fornecer leitura/escrita de baixa latência |
| `DirectoryMap` | Mapeamento de directórios em memória, mantém o mapeamento de caminhos virtuais para entradas de ficheiros |
| `EntryCache` | Cache de entradas, cache de entradas acedidas recentemente baseada em TTL |
| `WriteQueue` | Fila de escrita assíncrona, coloca operações de escrita em fila para execução em thread em segundo plano |
| `FreeList` | Gestão de espaço livre, rastreia espaço reutilizável no ficheiro .spk |
| `PackFileReader` | Leitor de ficheiros de pacote, lê dados do ficheiro .spk |
| `PackFileWriter` | Escritor de ficheiros de pacote, escreve dados no ficheiro .spk |
| `SpeedyPackAutoCompactor` | Temporizador de compactação automática, compacta periodicamente o ficheiro .spk para recuperar espaço livre |
| `SpeedyPackRegistry` | Gestor singleton a nível de processo, garante que toda a aplicação usa a mesma instância SpeedyPack |

### Adaptadores de Armazenamento

O SiliconLife.Fast integra o SpeedyPack nas interfaces do sistema através dos seguintes adaptadores:

| Adaptador | Interface | Descrição |
|--------|------|------|
| `SpeedyStorage` | `IStorage` | Adaptador de armazenamento chave-valor genérico |
| `SpeedyTimeStorage` | `ITimeStorage` | Adaptador de armazenamento com índice temporal |
| `SpeedyWorkNoteStorage` | `IWorkNoteStorage` | Adaptador de armazenamento de notas de trabalho |

### Opções de Configuração

O `SpeedyPackOptions` fornece as seguintes configurações:

| Opção | Tipo | Predefinição | Descrição |
|------|------|--------|------|
| `CacheTtl` | `TimeSpan` | 5 minutos | Tempo de vida das entradas em cache |
| `MaxCacheEntries` | `int` | 1000 | Número máximo de entradas em cache |
| `ReadOnly` | `bool` | false | Modo só de leitura |

### Suporte a Transacções

O SpeedyPack suporta operações de escrita atómica através da interface `IPackTransaction`:

- `SpeedyTransaction` implementa o mecanismo de transacção
- Suporta atomicidade para escritas em lote
- Na confirmação da transacção, todas as operações de escrita são bem-sucedidas ou todas são revertidas

---

## Sistema de Plugins

O SiliconLife suporta extensão de funcionalidades através de um sistema de plugins, permitindo que desenvolvedores terceiros adicionem novas funcionalidades à plataforma.

### Interface Principal

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

### Carregador de Plugins

O `PluginLoader` é responsável por carregar DLLs de plugins a partir de um directório especificado e executar verificações de segurança rigorosas:

1. **Análise de directório** — Analisa todos os ficheiros .dll no directório de plugins
2. **Análise de segurança** — Verifica se o plugin referencia namespaces proibidos
3. **Carregamento isolado** — Utiliza `AssemblyLoadContext` personalizado para carregar plugins de forma isolada
4. **Gestão do ciclo de vida** — Chama os métodos OnLoad, OnStart, OnStop, OnUnload do plugin

### Sandbox Segura

O carregador de plugins executa as seguintes verificações de segurança:

| Verificação | Descrição |
|--------|------|
| Namespaces proibidos | System.IO, System.Net.Http, System.Net.WebSockets, System.Net.Sockets, Microsoft.CodeAnalysis |
| Lista branca de assemblies fiáveis | Google.Protobuf, Newtonsoft.Json, MessagePack, Serilog, Microsoft.Extensions.Logging.Abstractions, Dapper |
| Verificação de tipos proibidos | Analisa tipos perigosos referenciados no plugin |
| Verificação de membros proibidos | Analisa métodos perigosos chamados no plugin |

### Integração de Ferramentas

Os plugins podem registar ferramentas personalizadas implementando a interface `ITool`:

- O método `ToolManager.ScanAllPluginAssemblies()` analisa implementações de ITool em todos os plugins carregados
- As ferramentas do plugin integram-se automaticamente no ciclo de chamadas de ferramentas
- As ferramentas do plugin estão sujeitas ao mesmo sistema de permissões

### Ciclo de Vida do Plugin

```
Carregamento (OnLoad) → Início (OnStart) → Em execução → Paragem (OnStop) → Descarregamento (OnUnload)
```

---

## Estado de Actividade dos Silicon Beings

Os Silicon Beings têm os seguintes estados de actividade:

| Estado | Descrição |
|------|------|
| `Idle` | Estado inactivo, aguardando activação do relógio |
| `SingleChat` | Em chat um-a-um |
| `GroupChat` | Em chat de grupo |
| `Task` | A executar tarefa |
| `Timer` | A executar temporizador |
| `Stopped` | Parado, devido a erros consecutivos ou paragem manual |

**Mecanismo do estado Stopped**:
- Quando um Silicon Being sofre 10 erros consecutivos, entra automaticamente no estado `Stopped`
- Após entrar no estado Stopped, o being não executa mais nenhuma tarefa
- Quando uma nova mensagem de chat chega, o contador de erros é reiniciado e o being retoma a execução

Transições de estado:
```
Idle → SingleChat → Idle (chat concluído)
Idle → GroupChat → Idle (chat de grupo concluído)
Idle → Task → Idle (tarefa concluída)
Idle → Timer → Idle (temporizador concluído)
Qualquer → Stopped (10 erros consecutivos)
Stopped → Idle (nova mensagem de chat ou reinício manual)
```

---

## Motor de Fluxos de Trabalho

O motor de fluxos de trabalho é um sistema de máquina de estados baseado em modelos, usado para impulsionar os fluxos de colaboração dos Silicon Beings nos espaços de projecto:

### Componentes Principais

| Componente | Descrição |
|------|------|
| `WorkflowEngine` | Núcleo do motor de fluxos de trabalho, gere modelos e instâncias, executa transições de estado orientadas por Tick |
| `WorkflowTemplate` | Modelo de fluxo de trabalho, define conjuntos de estados e regras de transição |
| `WorkflowInstance` | Instância de fluxo de trabalho, vinculada a um projecto específico, rastreia o estado actual |
| `WorkflowLog` | Registo de fluxo de trabalho, regista o histórico de transições de estado |

### Mecanismo de Funcionamento

- **Registo de modelos**: Registar modelos de fluxo de trabalho via `RegisterTemplate()`, definindo estados e regras de transição
- **Criação de instâncias**: Criar instâncias a partir de modelos, vinculadas a espaços de projecto
- **Orientado por Tick**: As transições de estado são impulsionadas pelo mecanismo Tick do ciclo principal
- **Registo**: Todas as transições de estado são registadas automaticamente no log

---

## Mecanismo de Desvanecimento da Memória

O `MemoryFadeService` é um serviço de decaimento temporizado que simula a característica de esquecimento da memória biológica:

### Mecanismo de Funcionamento

- **Execução temporizada**: Herda de `TickObject`, executa um ciclo de decaimento por padrão a cada hora
- **Decaimento de importância**: Aplica um algoritmo de decaimento às entradas de memória de cada Silicon Being, reduzindo a pontuação de importância
- **Arquivamento automático**: Memórias com importância abaixo do limiar são automaticamente arquivadas (`ArchiveFadingMemories()`)
- **Rastreamento estatístico**: Regista estatísticas como número de ciclos de decaimento, número de entradas com estado alterado

### Fluxo de Decaimento

```
MemoryFadeService.OnTick()
  └── Iterar sobre todos os Silicon Beings
       └── being.Memory.ApplyDecay()      # Aplicar decaimento de importância
       └── being.Memory.ArchiveFadingMemories()  # Arquivar memórias de baixa importância
```

---

## Sistema de Espaço de Trabalho de Projecto

O espaço de trabalho de projecto é um mecanismo de gestão de espaço que suporta a colaboração de múltiplos Silicon Beings:

### Funcionalidades Principais

- **Ciclo de vida do projecto**: Criação → Activo → Arquivado → Destruição
- **Atribuição de funções**: Suporta a atribuição de funções de projecto aos Silicon Beings
- **Isolamento de permissões de ferramentas**: Configuração de permissões de ferramentas ao nível do projecto, independente das permissões ao nível do Silicon Being
- **Notas de trabalho**: Sistema de notas em formato de página dentro do espaço do projecto, com geração de directório e pesquisa por palavras-chave
- **Acompanhamento de tarefas**: Gestão de tarefas ao nível do projecto, com suporte para criação, atribuição e rastreamento de estado
- **Integração de fluxos de trabalho**: Os projectos podem ser vinculados a modelos de fluxos de trabalho, impulsionando os fluxos de colaboração

### Ferramentas Relacionadas

| Ferramenta | Propósito |
|------|------|
| `ProjectTool` | Gestão do espaço do projecto (criação, arquivamento, destruição, atribuição de funções) |
| `ProjectTaskTool` | Gestão de tarefas do projecto (criação, atribuição, actualização de estado) |
| `ProjectWorkNoteTool` | Notas de trabalho do projecto (criação, pesquisa, geração de directório) |
| `ProjectWorkTool` | Operações de trabalho do projecto (criar tarefas, chat de grupo, difusão, concluir projecto) |

---

## Sistema de Competências

As Competências (Skills) são uma camada de abstracção reutilizável de "orquestração de ferramentas + modelos de prompts", que encapsula fluxos de trabalho comuns em unidades de capacidade declarativas, evolutivas e escalonáveis.

### Estrutura Hierárquica

| Camada | Localização | Responsabilidades |
|------|------|------|
| Núcleo | `SiliconLife.Core/Skills/` | SkillDefinition, SkillManager (registo + motor de execução), SkillMarkdownParser, SkillFileManager, AutoSkillTickObject, SkillMetadataCompleter |
| Comum | `SiliconLife.Common` | BuiltinSkills (3 competências integradas), SkillTool (ferramenta `skill`) |
| Aplicação | `SiliconLife.App/Web/` | SkillController + SkillView (página de gestão de competências) |

### Fluxo de Execução

```
Chamada de função da IA (id da competência) ou accionada pelo escalonador
        ↓
SkillManager.ExecuteSkill
  ├─ Verificação de interruptor global / permissões / protecção anti-recursão
  ├─ Clamp de parâmetros: maxToolRound = Min(valor da competência, GlobalMaxToolRound)
  │            timeout = Min(valor da competência, GlobalSkillTimeoutSeconds)
  ├─ MergePermissions: permissões do Being ∪ restrições da competência (lado mais restritivo prevalece)
  ├─ FillTemplate: preenchimento de marcadores {param} → sub-AIRequest
  └─ Sub-ciclo (máximo maxToolRound rondas): IA ↔ ferramentas (apenas na lista de permissões)
        ↓
HandleCompletion (OnCompleteAction)
  none / write_memory / notify_curator / broadcast
```

### Desenho Chave

- **Escalonamento transparente**: As competências são injectadas em `AIRequest.Tools` como `ToolDefinition`, sem percepção pela IA; em `ContextManager.ExecuteToolCalls`, as chamadas de competências têm prioridade sobre ferramentas com o mesmo nome
- **Quatro fontes**: `Builtin` (framework) / `Plugin` (ISkillProvider) / `Being` (tempo de execução do being) / `User` (Web UI); a recarga a quente preserva as duas primeiras e substitui as duas últimas
- **Markdown prioritário**: `skills/{id}.md` (frontmatter YAML + corpo) tem prioridade sobre `.json`; ao guardar Markdown puro, a IA completa os metadados (campos do utilizador não são sobrescritos)
- **Escalonamento automático**: `AutoSkillTickObject` (intervalo de verificação de 30 segundos) suporta `HH:mm`, `N s|m|h|d` e um subconjunto de cron, com protecção anti-reentrada
- **Múltiplas guardas**: Interruptor global, quota personalizada (`MaxCustomSkillsPerBeing`, predefinido 50), limites globais de rondas/tempo limite, permissão de acção `execute` ao nível da competência, lista de permissões de ferramentas, protecção anti-recursão

---

## Integração MCP

A integração MCP (Model Context Protocol) permite que os Silicon Beings invoquem ferramentas fornecidas por servidores MCP externos, expandindo os limites de capacidade sem necessidade de escrever código.

### Arquitectura

```
Utilizador (Web UI /mcp) ──adicionar/activar/desactivar/eliminar──→ McpManager (singleton)
                                          │
                              ┌───────────┼───────────┐
                              ↓           ↓           ↓
                        McpClientConnection × N (stdio / http)
                              │
                              └→ ListTools → envolvido como SiliconLife.Collective.McpTool
                                            nomeado mcp_{serverId}_{toolName}
                                                  │
                          McpManager.SyncToolsForBeing(being) injecta
                                                  ↓
                                    ToolManager (mesmo tratamento que ferramentas integradas)
```

### Desenho Chave

- **Duplo transporte**: `stdio` (subprocesso local: command + arguments + env) e `http` (endpoint remoto)
- **Isolamento de nomenclatura de ferramentas**: O prefixo `mcp_{serverId}_{toolName}` evita conflitos com ferramentas integradas/de plugins
- **Soberania do utilizador**: A adição/remoção/activação/desactivação de servidores só pode ser feita através da Web UI; a ferramenta `mcp` do lado da IA fornece apenas consultas de leitura (status/list_servers/list_tools)
- **Permissões consistentes**: As ferramentas envolvidas declaram automaticamente uma única acção `execute`, integradas na matriz de permissões de acções de ferramentas, podendo ser desactivadas por being/projecto
- **Persistência de configuração**: A lista `McpServers` é armazenada em config.json; `McpEnabled` é o interruptor global

---

## Arquitectura Multi-instância de Plataformas IM

As plataformas IM adoptam uma arquitectura de "configuração multi-instância + provedor agregado", permitindo a ligação simultânea a múltiplas plataformas de chat.

### Componentes Principais

| Componente | Responsabilidade |
|------|------|
| `IMPlatformConfig` | Configuração de instância única (platform/enabled/dicionário config); `IMPlatforms` é uma lista, cada instância é activada/desactivada independentemente |
| `IMProviderRegistry` | Registo de metadados de plataforma: schema de campos de configuração, modelos de endpoints OAuth, fábrica de Provider, links de ajuda |
| `AggregateIMProvider` | Agrega múltiplas plataformas: recepção de mensagens (qualquer plataforma acciona), envio de mensagens (difusão, falha de plataforma única isolada silenciosamente), consulta de permissões (primeiro a responder ganha a corrida) |
| `ImOAuthService` | Assistente de autorização OAuth (singleton): state anti-CSRF, tempo limite de 5 minutos, token escrito de volta na configuração, push de estado via SSE |
| `ConfigSecretResolver` | Resolução de marcadores `${ENV_VAR}`: substituição em cópia profunda, chaves em texto simples não são escritas de volta em config.json |
| `IMManager` | Encaminhamento de mensagens: enfileiramento por ChannelId (processamento serial) → ChatSystem → accionar pensamento do Silicon Being |

### Plataformas Suportadas

| Plataforma | AuthModes | Recepção de Eventos | Notas |
|------|-----------|---------|------|
| Web UI | manual | SSE (integrado) | Sempre disponível, auto-preenchido |
| Feishu | manual / **oauth** | Callback HTTP (verificação de assinatura + desencriptação AES) | Suporta assistente de autorização OAuth num clique |
| WeChat Enterprise | manual | Callback HTTP (WXBizMsgCrypt) | Requer callback de rede pública |
| DingTalk | manual | Stream (WebSocket) / HTTP | Modo Stream predefinido, não requer rede pública |

### Fluxo de Mensagens

```
Feishu/WeCom/DingTalk/WebUI (entrada)
  → IIMProvider.MessageReceived
  → IMManager.OnMessageReceived (enfileiramento por ChannelId, serial)
  → ChatSystem.AddMessage → pensamento da IA do Silicon Being
  → IMManager.SendMessageAsync / SendStreamChunkAsync (saída)
  → AggregateIMProvider difunde para todas as plataformas activas
```
