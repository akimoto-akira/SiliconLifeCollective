# Segurança

> **Versão: v0.2.0-alpha**

[English](../en/security.md) | [Deutsch](../de-DE/security.md) | [Français](../fr-FR/security.md) | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | [Español](../es-ES/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | [Čeština](../cs-CZ/security.md) | [Italiano](../it-IT/permission-system.md) | [Polski](../pl-PL/security.md) | **Português**

## Visão geral

A segurança do Silicon Life Collective é baseada num modelo de **defesa em camadas**. Princípio central: **todas as operações I/O devem passar por executores**, que impõem verificações de permissões antes da execução.

```
Chamada de ferramenta → Executor → Gestor de permissões → Cache de frequência → Callback → (IsCurator: perguntar ao utilizador | Non-curator: ACL global)
```

---

## Modelo de permissões

### Tipos de permissões

| Tipo | Descrição |
|------|-------------|
| `NetworkAccess` | Pedidos HTTP/HTTPS de saída |
| `CommandLine` | Execução de comandos shell |
| `FileAccess` | Operações de ficheiros e diretórios |
| `Function` | Chamadas de funções sensíveis |
| `DataAccess` | Acesso a dados do sistema ou do utilizador |

### Resultados de permissões

Cada verificação de permissão retorna um de três resultados:

| Resultado | Comportamento |
|--------|----------|
| **Allowed (Permitido)** | A operação prossegue imediatamente |
| **Denied (Negado)** | A operação é bloqueada, registada no log de auditoria |
| **AskUser (Perguntar ao utilizador)** | A operação é pausada, requer confirmação do utilizador |

### Papel especial: Curator de silício

O Curator de silício possui o nível mais alto de permissões (`IsCurator = true`). Quando a cadeia de permissões atinge a ramificação, as operações do curator passam pelo `IPermissionAskHandler` para pedir confirmação ao utilizador, em vez de serem automaticamente permitidas. Os não-curators consultam a ACL global.

### Gestor de permissões privado

Cada Silicon Being tem a sua própria instância **privada de PermissionManager**. O estado das permissões não é partilhado entre os beings.

---

## Fluxo de verificação de permissões

A prioridade de consulta é: **1. Cache de frequência → 2. Função callback → 3. Ramificação (IsCurator/GlobalACL)**

```
┌─────────────┐
│ Chamada de  │
│ ferramenta  │
└──────┬──────┘
       │
       ▼
┌─────────────┐     ┌─────────────────────┐
│  Executor   │────▶│ Gestor de permissões│
│ (disco/rede/│     │ privado (por being) │
│  cmd...)    │     └────────┬────────────┘
└─────────────┘            │
                           ▼
                  ┌─────────────────┐
                  │ 1. Cache de     │──Correspondência──▶ Permitir / Negar
                  │    frequência   │
                  │ (alta negação   │
                  │  > alta         │
                  │  permissão)     │
                  └────────┬────────┘
                           │ Sem correspondência
                           ▼
                  ┌─────────────────┐
                  │ 2. Callback de  │
                  │    permissão    │──▶ Permitir / Negar / Perguntar
                  └────────┬────────┘
                           │ Perguntar ao utilizador
                           ▼
                  ┌─────────────────┐
                  │ 3. IsCurator?   │
                  └────────┬────────┘
                           │
                 ┌─────────┴─────────┐
                 │                   │
                 ▼ Sim               ▼ Não
          ┌─────────────┐    ┌─────────────┐
          │ Perguntar ao│    │ ACL global  │
          │ utilizador  │    │ Consultar   │
          │ (AskHandler)│    │ regras      │
          └─────────────┘    └─────────────┘
```

**Ponto-chave**: O executor apenas vê um valor booleano (permitir/negar). O gestor de permissões processa internamente a decisão tri-estado (permitir/negar/perguntar ao utilizador) e resolve a pergunta ao utilizador antes de retornar ao executor.

---

## Executores (fronteira de segurança)

Os executores são o **único** caminho para operações I/O. Eles impõem:

### Thread de despacho independente

Cada executor possui uma **thread de despacho independente**:

- Isolamento de threads entre executores — o bloqueio da thread de um executor não afeta os outros.
- Cada executor pode ter limites de recursos independentes (CPU, memória, etc.).
- Gestão do pool de threads das threads do executor.

### Fila de pedidos

Cada executor mantém uma fila de pedidos:

- Os pedidos são encaminhados para o executor correspondente por tipo.
- Suporta filas de prioridade.
- Controlo de timeout por pedido.

### Bloqueio de thread para verificação de permissões

Quando uma ferramenta inicia um acesso a recursos:

1. O executor recebe o pedido e **bloqueia a sua thread**.
2. O executor consulta o gestor de permissões privado do being.
3. Se o callback retornar "perguntar ao utilizador", a thread do executor **mantém-se bloqueada** à espera da resposta do utilizador.
4. O being apenas vê o resultado final (sucesso ou recusa) — nunca vê o estado intermédio de "pendente" ou "em espera".
5. Apenas o Curator de silício aciona um pedido real ao utilizador. Os beings comuns consultam a ACL global de forma síncrona sem bloquear.
6. Em caso de timeout, o pedido é tratado como negado e o bloqueio da thread é libertado.

### Tipos de executores

| Executor | Âmbito | Timeout predefinido |
|----------|-------|-----------------|
| `DiskExecutor` | Leitura/escrita de ficheiros, operações de diretórios | 30 segundos |
| `NetworkExecutor` | Pedidos HTTP, ligações WebSocket | 60 segundos |
| `CommandLineExecutor` | Execução de comandos shell | 120 segundos |

> **Nota**: O `DynamicCompilationExecutor` (localizado no namespace `SiliconLife.Core.Compilation`) é responsável pela compilação em memória Roslyn, não pertence à categoria de executores I/O, mas está igualmente sujeito ao sistema de permissões.

### Isolamento de exceções e tolerância a falhas

- Uma exceção num executor não afeta os outros.
- Reinício automático em caso de falha da thread.
| Disjuntor: para temporariamente o executor após falhas consecutivas para prevenir falhas em cascata.

---

## ACL global (lista de controlo de acesso)

Tabela de regras partilhada persistida no armazenamento, gerida apenas pelo Curator de silício:

```json
{
  "rules": [
    { "prefix": "network:api.github.com", "result": "Allowed" },
    { "prefix": "file:C:\\Windows", "result": "Denied" },
    { "prefix": "cli:rm -rf", "result": "Denied" }
  ]
}
```

- As regras são avaliadas por ordem; a primeira correspondência vence.
- Apenas o Curator de silício pode modificar a ACL global (através das suas ferramentas dedicadas).
- As alterações entram em vigor imediatamente.
- A ACL global **não** está na cadeia de prioridade de consulta acima — é referenciada internamente pela função callback.

---

## Cache de frequência do utilizador

Para reduzir pedidos de permissão repetidos, o sistema mantém duas caches **por being, apenas em memória**:

| Cache | Propósito |
|-------|---------|
| **HighAllow (Alta permissão)** | Recursos que o utilizador permite frequentemente |
| **HighDeny (Alta negação)** | Recursos que o utilizador nega frequentemente |

### Como funciona

- **Escolha do utilizador, não deteção automática**: Quando "perguntar ao utilizador" é acionado, o utilizador escolhe se adiciona o recurso à cache.
- **Correspondência por prefixo**: Suporta correspondência de prefixo de caminho de recurso (ex: `network:api.example.com/*`).
- **Prioridade**: Alta negação tem prioridade sobre alta permissão.
- **Apenas em memória**: A cache não é persistida. Perde-se ao reiniciar.
- **Expiração configurável**: O utilizador pode definir o prazo de validade das entradas da cache.

### Fluxo de atualização da cache

1. O callback de permissões retorna `AskUser`.
2. O sistema de permissões envia uma consulta ao sistema de cartões (interface Web ou mensageiro instantâneo).
3. O utilizador toma uma decisão (permitir/negar) e **escolhe se armazena em cache**.
4. O sistema de cartões retorna a decisão + flag de cache.
5. O sistema de permissões atualiza a lista de cache correspondente.
6. Pedidos futuros que correspondam ao prefixo da cache são resolvidos imediatamente.

---

## Mecanismo de pergunta ao utilizador

Quando a verificação de permissões retorna `AskUser`:

### Interface Web: Cartão interativo

A interface Web exibe imediatamente um **cartão interativo**, mostrando:

- Tipo e caminho do recurso
- Descrição da operação
- Botões Permitir / Negar
| Caixa de seleção opcional "Permitir sempre" / "Negar sempre" (adicionar à cache de frequência)

### Mensageiro instantâneo (sem suporte para cartões): Código aleatório

Para plataformas de mensagens que não suportam cartões interativos:

1. O sistema gera dois códigos aleatórios de 6 dígitos: **código de permissão** e **código de negação**.
2. Envia uma mensagem com as informações do recurso e os dois códigos.
3. O utilizador deve responder com o código de permissão exato para autorizar. Qualquer outra resposta é tratada como negação.
4. Os códigos são de uso único para prevenir ataques de replay.

### Timeout

- Um timeout é definido para todos os pedidos de "perguntar ao utilizador".
| Em caso de timeout, o pedido é tratado como **negado** e o bloqueio da thread do executor é libertado.

---

## Segurança da compilação dinâmica

A autoevolução (tipo reescrita) introduz riscos de segurança únicos. O sistema mitiga-os com uma **estratégia em camadas**:

### Camada 1: Controlo de referências em tempo de compilação (defesa principal)

- O compilador apenas obtém a **lista permitida de referências de assembly**.
- **Permitido**: `System.Runtime`, `System.Private.CoreLib`, assemblies do projeto (interface ITool, etc.)
- **Bloqueado**: `System.IO`, `System.Reflection`, `System.Runtime.InteropServices`, etc.
- Se o código referenciar um assembly bloqueado, **o próprio compilador recusa** o código.
- Isto é mais fiável do que a análise em tempo de execução — operações perigosas são impossíveis ao nível do tipo.

### Camada 2: Análise estática em tempo de execução (defesa secundária)

- Mesmo após uma compilação bem-sucedida, o código é submetido a uma análise de padrões estáticos.
- Deteta padrões de operações perigosas (I/O direto, chamadas de sistema, etc.).
- Se for encontrado código perigoso, o carregamento é recusado e o sistema reverte para a funcionalidade predefinida.

### Restrição de herança

Todas as classes personalizadas de Silicon Beings **devem** herdar de `SiliconBeingBase`. O compilador impõe esta restrição ao nível do tipo.

### Armazenamento encriptado

O código compilado é armazenado encriptado em disco com AES-256:

- **Derivação de chave**: A partir do GUID do being (maiúsculas) usando PBKDF2.
- **Falha na desencriptação**: Reverte para a implementação predefinida.
- **Recompilação em tempo de execução**: O novo código é primeiro compilado em memória; só é persistido após compilação bem-sucedida e substituição da instância.

### Substituição atómica

O processo de substituição é atómico:

1. Compilar o novo código em memória → obter o `Type`.
2. Criar uma nova instância a partir do `Type`.
3. Migrar o estado da instância antiga para a nova.
4. Trocar as referências.
5. Persistir o código encriptado.

Se qualquer passo falhar, a instância antiga mantém-se ativa.

---

## Função callback de permissões

### Design

Cada PermissionManager possui uma **variável de função callback**:

- **Predefinição**: Aponta para a função de permissões predefinida incorporada.
- **Após compilação dinâmica**: É substituída pela função de permissões personalizada do being.
- **Exclusiva**: Apenas um callback está ativo em qualquer momento.
- **Falha na compilação**: Não afeta o callback atual — a função predefinida ou a última função personalizada bem-sucedida mantém-se ativa.

### Assinatura do callback

```
PermissionResult Callback(PermissionType type, string resourcePath, Guid callerId)
```

Retorna `Allowed`, `Denied` ou `AskUser`.

---

## Registo de auditoria

Todas as decisões de permissões são registadas:

```
[2026-04-01 15:30:25] ALLOWED  | Being:AssistantA | Type:NetworkAccess | Resource:api.github.com | Source:HighAllowCache
[2026-04-01 15:30:26] DENIED   | Being:AssistantB | Type:FileAccess    | Resource:C:\Windows\System32 | Source:HighDenyCache
[2026-04-01 15:30:27] ASK_USER | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:Callback
[2026-04-01 15:30:28] ALLOWED  | Being:Curator    | Type:CommandLine   | Resource:del /f /q *.log | Source:UserDecision
```

Os registos são persistidos no armazenamento e podem ser consultados através da interface Web (controlador de registos).

---

## Auditoria de utilização de Tokens

O `TokenUsageAuditManager` fornece rastreamento do consumo de tokens de IA relacionado com a segurança:

- **Registo por pedido** — Cada chamada à IA regista o ID do being, o modelo, os tokens de prompt, os tokens de completion e o carimbo temporal.
- **Deteção de anomalias** — Padrões de consumo de tokens invulgares podem indicar injeção de prompts ou abuso de recursos.
- **Acesso apenas do curator** — O `TokenAuditTool` (marcado com `[SiliconManagerOnly]`) permite ao curator consultar e resumir a utilização de tokens.
- **Painel Web** — O `UsageController` fornece um painel baseado no browser, com gráficos de tendências e exportação de dados.
- **Armazenamento persistente** — Os registos são armazenados através do `ITimeStorage`, para consultas de séries temporais e análise de longo prazo.

---

## Segurança de plugins

O sistema de plugins introduz riscos de segurança na execução de código de terceiros, mitigados pelos seguintes mecanismos:

### Sandbox de segurança

O `PluginLoader` executa uma verificação de segurança rigorosa ao carregar plugins:

1. **Verificação de namespaces proibidos** — Os plugins não podem referenciar os seguintes namespaces:
   - `System.IO` — Acesso ao sistema de ficheiros
   - `System.Net.Http` — Pedidos HTTP
   - `System.Net.WebSockets` — Ligações WebSocket
   - `System.Net.Sockets` — Sockets brutos
   - `Microsoft.CodeAnalysis` — API do compilador

2. **Lista de permissão de assemblies fidedignos** — Referências aos seguintes assemblies são permitidas:
   - `Google.Protobuf`, `Newtonsoft.Json`, `MessagePack`
   - `Serilog`, `Microsoft.Extensions.Logging.Abstractions`
   - `Dapper`

3. **Verificação de tipos proibidos** — Analisa tipos perigosos referenciados no plugin

4. **Verificação de membros proibidos** — Analisa métodos perigosos chamados no plugin

### Carregamento isolado

- Cada plugin é carregado de forma isolada usando um `AssemblyLoadContext` personalizado
- Tipos e assemblies entre plugins não interferem uns com os outros
- Os recursos relacionados podem ser libertados quando o plugin é descarregado

### Restrições de permissões de ferramentas

- As ferramentas registadas pelos plugins através da interface `ITool` estão sujeitas ao mesmo sistema de permissões
- As ferramentas de plugins não podem contornar a cadeia de permissões de 5 níveis
- As ferramentas de plugins estão sujeitas à marcação `[SiliconManagerOnly]`
