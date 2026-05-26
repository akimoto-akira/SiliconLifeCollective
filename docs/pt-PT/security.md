# Desenho de Segurança

> **Versão: v0.2.0-alpha**

[English](../en/security.md) | [Deutsch](../de-DE/security.md) | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | [Español](../es-ES/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | [Čeština](../cs-CZ/security.md) | [Русский](../ru-RU/security.md)

## Visão Geral

A segurança do Silicon Life Collective é construída sobre um modelo de **defesa em camadas**. Princípio central: **Todas as operações de I/O devem passar por executores**, que impõem verificação de permissões antes da execução.

```
Chamada de Ferramenta → Executor → Gestor de Permissões → Cache de Frequência → Callback → (IsCurator: Perguntar ao Utilizador | Non-curator: ACL Global)
```

---

## Modelo de Permissões

### Tipos de Permissão

| Tipo | Descrição |
|------|-------------|
| `NetworkAccess` | Pedidos HTTP/HTTPS de saída |
| `CommandLine` | Execução de comandos shell |
| `FileAccess` | Operações de ficheiros e directórios |
| `Function` | Chamadas de funções sensíveis |
| `DataAccess` | Acesso a dados do sistema ou do utilizador |

### Resultados de Permissão

Cada verificação de permissões retorna um de três resultados:

| Resultado | Comportamento |
|--------|----------|
| **Allowed (Permitido)** | A operação prossegue imediatamente |
| **Denied (Negado)** | A operação é bloqueada, registo de auditoria gravado |
| **AskUser (Perguntar ao Utilizador)** | A operação é suspensa, requer confirmação do utilizador |

### Papel Especial: Silicon Curator

O Silicon Curator possui o nível mais elevado de permissões (`IsCurator = true`). Quando a cadeia de permissões atinge a ramificação, as operações do Curator solicitam confirmação do utilizador via `IPermissionAskHandler`, em vez de serem automaticamente permitidas. Os beings não Curator consultam a ACL Global.

### Gestor de Permissões Privado

Cada Silicon Being tem a sua própria instância **privada de PermissionManager**. O estado das permissões não é partilhado entre beings.

---

## Fluxo de Verificação de Permissões

A prioridade de consulta é: **1. Cache de Frequência → 2. Função Callback → 3. Ramificação (IsCurator/GlobalACL)**

```
┌─────────────┐
│ Chamada de   │
│ Ferramenta   │
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Executor    │────▶│ Gestor de           │
│ (Disco/Rede/ │     │ Permissões Privado  │
│  Linha Cmd.) │     │ (por being)         │
└─────────────┘     └────────┬────────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │ 1. Cache de     │──Correspondência──▶ Permitido / Negado
                    │    Frequência   │
                    │ (HighDeny tem   │
                    │  prioridade     │
                    │  sobre HighAllow│
                    └────────┬────────┘
                             │ Sem correspondência
                             ▼
                    ┌─────────────────┐
                    │ 2. Callback de  │──▶ Permitido / Negado / Perguntar ao Utilizador
                    │    Permissões   │
                    └────────┬────────┘
                             │ Perguntar ao Utilizador
                             ▼
                    ┌─────────────────┐
                    │ 3. IsCurator?   │
                    └────────┬────────┘
                             │
                   ┌─────────┴─────────┐
                   │                   │
                   ▼ Sim               ▼ Não
            ┌─────────────┐    ┌─────────────┐
            │ Perguntar   │    │ ACL Global  │
            │ ao Utiliz.  │    │ Consultar   │
            │ (AskHandler)│    │ regras      │
            └─────────────┘    └─────────────┘
```

**Ponto chave**: Os executores apenas veem valores booleanos (permitido/negado). O Gestor de Permissões processa internamente a decisão de três estados (permitido/negado/perguntar ao utilizador) e resolve a pergunta ao utilizador antes de retornar ao executor.

---

## Executors (Fronteira de Segurança)

Os executores são o **único** caminho para operações de I/O. Eles impõem:

### Thread de Despacho Independente

Cada executor possui uma **thread de despacho independente**:

- Isolamento de threads entre executores — o bloqueio da thread de um executor não afecta os outros.
- Cada executor pode definir limites de recursos independentes (CPU, memória, etc.).
- Gestão de pool de threads para as threads dos executores.

### Fila de Pedidos

Cada executor mantém uma fila de pedidos:

- Os pedidos são encaminhados para o executor correspondente por tipo.
- Suporte a fila com prioridade.
- Controlo de timeout por pedido.

### Bloqueio de Thread para Verificação de Permissões

Quando uma ferramenta inicia o acesso a recursos:

1. O executor recebe o pedido e **bloqueia a sua thread**.
2. O executor consulta o Gestor de Permissões privado do being.
3. Se o callback retornar Perguntar ao Utilizador, a thread do executor **mantém-se bloqueada** aguardando a resposta do utilizador.
4. O being apenas vê o resultado final (sucesso ou recusa) — nunca vê o estado intermédio "pendente" ou "aguardando".
5. Apenas o Silicon Curator acciona verdadeiramente prompts ao utilizador. Os beings comuns consultam a ACL Global de forma síncrona sem bloquear.
6. Em caso de timeout, o pedido é tratado como negado e o bloqueio da thread é libertado.

### Tipos de Executor

| Executor | Âmbito | Timeout Predefinido |
|----------|-------|-----------------|
| `DiskExecutor` | Leitura/escrita de ficheiros, operações de directório | 30 segundos |
| `NetworkExecutor` | Pedidos HTTP, ligações WebSocket | 60 segundos |
| `CommandLineExecutor` | Execução de comandos shell | 120 segundos |

> **Nota**: O `DynamicCompilationExecutor` (localizado no namespace `SiliconLife.Core.Compilation`) é responsável pela compilação em memória Roslyn, não pertence à categoria de executores de I/O, mas está igualmente sujeito ao sistema de permissões.

### Isolamento de Excepções e Tolerância a Falhas

- As excepções de um executor não afectam os outros executores.
- Reinício automático em caso de falha da thread.
- Circuit breaker: Após falhas consecutivas, o executor é temporariamente parado para prevenir falhas em cascata.

---

## ACL Global (Lista de Controlo de Acesso)

Tabela de regras partilhada persistida no armazenamento, gerida apenas pelo Silicon Curator:

```json
{
  "rules": [
    { "prefix": "network:api.github.com", "result": "Allowed" },
    { "prefix": "file:C:\\Windows", "result": "Denied" },
    { "prefix": "cli:rm -rf", "result": "Denied" }
  ]
}
```

- As regras são avaliadas por ordem; a primeira correspondência prevalece.
- Apenas o Silicon Curator pode modificar a ACL Global (através das suas ferramentas dedicadas).
- As alterações produzem efeito imediatamente.
- A ACL Global **não está** na cadeia de prioridade por consulta acima — é referenciada internamente pela função callback.

---

## Cache de Frequência do Utilizador

Para reduzir prompts de permissões repetidos, o sistema mantém dois caches **por being, apenas em memória**:

| Cache | Propósito |
|-------|---------|
| **HighAllow (Alta Permissão)** | Recursos que o utilizador permite frequentemente |
| **HighDeny (Alta Negação)** | Recursos que o utilizador nega frequentemente |

### Como Funciona

- **Escolha do utilizador, não detecção automática**: Quando Perguntar ao Utilizador é accionado, o utilizador escolhe se adiciona o recurso ao cache.
- **Correspondência por prefixo**: Suporta correspondência por prefixo de caminho de recurso (por exemplo `network:api.example.com/*`).
- **Prioridade**: HighDeny tem prioridade sobre HighAllow.
- **Apenas em memória**: O cache não é persistido. Perde-se após reinício.
- **Expiração configurável**: O utilizador pode definir o período de validade das entradas do cache.

### Fluxo de Actualização do Cache

1. O callback de permissões retorna `AskUser`.
2. O sistema de permissões envia uma consulta ao sistema de cartões (Web UI ou IM).
3. O utilizador toma uma decisão (permitir/negar) e **escolhe se deve cachear**.
4. O sistema de cartões retorna a decisão + flag de cache.
5. O sistema de permissões actualiza a lista de cache correspondente.
6. Pedidos futuros que correspondam ao prefixo do cache são resolvidos imediatamente.

---

## Mecanismo de Pergunta ao Utilizador

Quando a verificação de permissões retorna `AskUser`:

### Web UI: Cartão Interactivo

O frontend Web exibe imediatamente um **cartão interactivo**, mostrando:

- Tipo e caminho do recurso
- Descrição da operação
- Botões Permitir / Negar
- Caixa de selecção opcional "Permitir sempre" / "Negar sempre" (adiciona ao cache de frequência)

### Mensageiro Instantâneo (sem suporte a cartões): Código Aleatório

Para plataformas de mensagens que não suportam cartões interactivos:

1. O sistema gera dois códigos aleatórios de 6 dígitos: **código de permissão** e **código de negação**.
2. Envia uma mensagem contendo as informações do recurso e os dois códigos.
3. O utilizador deve responder com o código de permissão exacto para autorizar. Qualquer outra resposta é tratada como negação.
4. Os códigos são de uso único, para prevenir ataques de replay.

### Timeout

- É definido um timeout para todos os pedidos de pergunta ao utilizador.
- Em caso de timeout, o pedido é tratado como **negado** e o bloqueio da thread do executor é libertado.

---

## Segurança da Compilação Dinâmica

A auto-evolução (reescrita de classes) introduz riscos de segurança únicos. O sistema mitiga-os usando uma **estratégia em camadas**:

### Camada 1: Controlo de Referências na Compilação (Defesa Primária)

- O compilador recebe apenas a **lista de referências de assembly permitidas**.
- **Permitido**: `System.Runtime`, `System.Private.CoreLib`, assemblies do projecto (interface ITool, etc.)
- **Bloqueado**: `System.IO`, `System.Reflection`, `System.Runtime.InteropServices`, etc.
- Se o código referenciar um assembly bloqueado, **o próprio compilador recusa** o código.
- Isto é mais fiável do que a análise em tempo de execução — operações perigosas são impossíveis ao nível do tipo.

### Camada 2: Análise Estática em Tempo de Execução (Defesa Secundária)

- Mesmo após compilação bem-sucedida, o código é submetido a análise de padrões estáticos.
- Detecta padrões de operações perigosas (I/O directo, chamadas de sistema, etc.).
- Se código perigoso for encontrado, o carregamento é recusado e o sistema reverte para a funcionalidade padrão.

### Restrição de Herança

Todas as classes personalizadas de Silicon Beings **devem** herdar de `SiliconBeingBase`. O compilador impõe esta restrição ao nível do tipo.

### Armazenamento Encriptado

O código compilado é armazenado no disco com encriptação AES-256:

- **Derivação de chave**: Do GUID do being (maiúsculas) usando PBKDF2.
- **Falha na desencriptação**: Reverte para a implementação padrão.
- **Recompilação em tempo de execução**: O novo código é primeiro compilado em memória; apenas após compilação bem-sucedida e substituição da instância é persistido.

### Substituição Atómica

O processo de substituição é atómico:

1. Compilar o novo código em memória → obter o `Type`.
2. Criar nova instância a partir do `Type`.
3. Migrar o estado da instância antiga para a nova.
4. Trocar as referências.
5. Persistir o código encriptado.

Se qualquer passo falhar, a instância antiga mantém-se activa.

---

## Função Callback de Permissões

### Desenho

Cada PermissionManager detém uma **variável de função callback**:

- **Padrão**: Aponta para a função de permissões padrão incorporada.
- **Após compilação dinâmica**: É sobrescrita pela função de permissões personalizada do being.
- **Um ou outro**: Apenas um callback está activo em qualquer momento.
- **Falha na compilação**: Não afecta o callback actual — a função padrão ou a última função personalizada bem-sucedida mantém-se activa.

### Assinatura do Callback

```
PermissionResult Callback(PermissionType type, string resourcePath, Guid callerId)
```

Retorna `Allowed`, `Denied` ou `AskUser`.

---

## Registo de Auditoria

Todas as decisões de permissões são registadas:

```
[2026-04-01 15:30:25] ALLOWED  | Being:AssistantA | Type:NetworkAccess | Resource:api.github.com | Source:HighAllowCache
[2026-04-01 15:30:26] DENIED   | Being:AssistantB | Type:FileAccess    | Resource:C:\Windows\System32 | Source:HighDenyCache
[2026-04-01 15:30:27] ASK_USER | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:Callback
[2026-04-01 15:30:28] ALLOWED  | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:UserDecision
```

Os registos são persistidos no armazenamento e podem ser visualizados através da Web UI (controlador de registos).

---

## Auditoria de Utilização de Tokens

O `TokenUsageAuditManager` fornece rastreamento do consumo de tokens de IA relacionado com segurança:

- **Registo por pedido** — Cada chamada de IA regista o ID do being, modelo, tokens do prompt, tokens de conclusão e timestamp.
- **Detecção de anomalias** — Padrões anómalos de consumo de tokens podem indicar injeção de prompt ou abuso de recursos.
- **Acesso apenas do Curator** — O `TokenAuditTool` (marcado com `[SiliconManagerOnly]`) permite ao Curator consultar e resumir a utilização de tokens.
- **Dashboard Web** — O `UsageController` fornece um dashboard baseado no navegador, com gráficos de tendência e exportação de dados.
- **Armazenamento persistido** — Os registos são armazenados via `ITimeStorage`, para consultas de séries temporais e análise de longo prazo.

---

## Segurança de Plugins

O sistema de plugins introduz riscos de segurança de execução de código de terceiros, mitigados pelos seguintes mecanismos:

### Sandbox Segura

O `PluginLoader` executa uma verificação de segurança rigorosa ao carregar plugins:

1. **Verificação de namespaces proibidos** — Os plugins não podem referenciar os seguintes namespaces:
   - `System.IO` — Acesso ao sistema de ficheiros
   - `System.Net.Http` — Pedidos HTTP
   - `System.Net.WebSockets` — Ligações WebSocket
   - `System.Net.Sockets` — Sockets raw
   - `Microsoft.CodeAnalysis` — API do compilador

2. **Lista branca de assemblies fiáveis** — Referências aos seguintes assemblies são permitidas:
   - `Google.Protobuf`, `Newtonsoft.Json`, `MessagePack`
   - `Serilog`, `Microsoft.Extensions.Logging.Abstractions`
   - `Dapper`

3. **Verificação de tipos proibidos** — Analisa tipos perigosos referenciados no plugin

4. **Verificação de membros proibidos** — Analisa métodos perigosos chamados no plugin

### Carregamento Isolado

- Usa `AssemblyLoadContext` personalizado para carregar cada plugin de forma isolada
- Tipos e assemblies entre plugins não interferem entre si
- Os recursos associados podem ser libertados quando o plugin é descarregado

### Restrições de Permissões de Ferramentas

- As ferramentas registadas pelos plugins via interface `ITool` estão sujeitas ao mesmo sistema de permissões
- As ferramentas dos plugins não podem contornar a cadeia de verificação de permissões
- As ferramentas dos plugins estão sujeitas à marca `[SiliconManagerOnly]`

---

## Segurança de Permissões de Ferramentas

O sistema de permissões de ferramentas fornece uma camada de segurança adicional, controlando quais operações de ferramentas os Silicon Beings podem usar:

### Isolamento de Permissões em Dois Níveis

1. **Nível do Silicon Being** — Cada Silicon Being tem uma configuração de permissões de ferramentas independente
2. **Nível do Projecto** — As permissões de ferramentas no espaço do projecto são independentes do nível do Silicon Being, realizando isolamento de permissões entre projectos

### Modelos de Permissões

O sistema fornece modelos de permissões predefinidos, garantindo uma linha de base de segurança:

- **readonly** — Privilégios mínimos, apenas permite operações de leitura
- **restricted** — Permissões restritas, apenas permite operações básicas
- **full** — Permissões completas (usado apenas pelo Curator)

### Características de Segurança

- **Negação por defeito** — Operações de ferramentas não explicitamente permitidas são negadas por defeito
- **Granularidade por operação** — Cada operação de cada ferramenta é controlada independentemente (por exemplo `network:get` permitido mas `network:post` negado)
- **Gestão pelo Curator** — As permissões de ferramentas só podem ser configuradas pelo Silicon Curator
- **Rasto de auditoria** — As alterações de permissões de ferramentas são registadas no registo de auditoria
