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
- **Suporte de plataforma**: Windows (funcionalidades completas, incluindo bandeja do sistema), Linux (janela de estado, sem ícone na bandeja)
- **Características**:
  - Windows/macOS execução em segundo plano na bandeja do sistema, monitorização em tempo real através da janela de estado; Linux janela de estado exibida diretamente
  - Motor SpeedyPack + compressão automática que garante a segurança dos dados
  - Arquitetura Component UI, 30+ componentes declarativos
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
