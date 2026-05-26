# Sistema de Permissões

> **Versão: v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | [Русский](../ru-RU/permission-system.md)

## Visão Geral

O sistema de permissões garante que todas as operações iniciadas pela IA passam por verificação e auditoria adequadas.

## Cadeia de Verificação de Permissões

```
┌─────────────────────────────────────────────┐
│          Verificação de Permissões           │
├─────────────────────────────────────────────┤
│  Nível 1: UserFrequencyCache                 │
│  ↓ Cache de decisões frequentes do utilizador (HighDeny/HighAllow) │
│  Nível 2: IPermissionCallback                │
│  ↓ Lógica personalizada (Allowed/Denied/AskUser) │
│  Nível 3: IsCurator?                         │
│  ↓ Sim → IPermissionAskHandler (perguntar ao utilizador) │
│  ↓ Não → GlobalACL → negação por defeito     │
│  Resultado: Permitido ou Negado               │
└─────────────────────────────────────────────┘
```

> **Nota**: A prioridade real de consulta do `PermissionManager.CheckPermission()` é:
> 1. **UserFrequencyCache** — Verifica primeiro o cache de decisões frequentes do utilizador
> 2. **IPermissionCallback** — Avalia as regras de callback personalizadas
> 3. **Ramificação do Curator** — Quando o callback retorna AskUser ou não há callback:
>    - **Curator** → `IPermissionAskHandler` (perguntar ao utilizador via IM)
>    - **Não Curator** → `GlobalACL` → negação por defeito

## Nível 1: UserFrequencyCache

Cache de decisões frequentes do utilizador (HighDeny/HighAllow) por being, existente apenas em memória.

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny tem prioridade sobre HighAllow**
- **Apenas em memória**: O cache não é persistido, perdendo-se após reinício
- **Tempo de expiração configurável**: O utilizador pode definir o período de validade das entradas do cache

## Nível 2: IPermissionCallback

Callback personalizado para lógica de permissões dinâmica.

### Implementação Padrão DefaultPermissionCallback

O `DefaultPermissionCallback` fornece regras de permissão padrão abrangentes, incluindo:

#### Regras de Acesso à Rede
- **Endereços de loopback**: Permite localhost, 127.0.0.1, ::1
- **Endereços IP privados**:
  - 192.168.x.x (Classe C) - Permitido
  - 10.x.x.x (Classe A) - Permitido
  - 172.16-31.x.x (Classe B) - Perguntar ao utilizador
- **Lista branca de domínios**:
  - Motores de busca: Google, Bing, DuckDuckGo, Yandex, Sogou, etc.
  - Serviços de IA: OpenAI, Anthropic, HuggingFace, Ollama, etc.
  - Serviços para programadores: GitHub, StackOverflow, npm, NuGet, etc.
  - Redes sociais: Weibo, Zhihu, Reddit, Discord, etc.
  - Plataformas de vídeo: YouTube, Bilibili, Douyin, TikTok, etc.
  - **Informação meteorológica**: wttr.in
  - Sites governamentais: .gov, .go.jp, .go.kr
- **Lista negra de domínios**:
  - Sites de falsificação de IA: domínios falsos de chatgpt, openai, deepseek, etc.
  - Ferramentas de IA maliciosas: wormgpt, darkgpt, fraudgpt, etc.
  - Domínios relacionados com farms de conteúdo de IA e mercado negro

```csharp
public class DefaultPermissionCallback : IPermissionCallback
{
    public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
    {
        if (IsSafeOperation(permissionType, resource))
        {
            return PermissionResult.Allowed;
        }
        
        return PermissionResult.AskUser;
    }
}
```

## Nível 3: Ramificação (IsCurator / GlobalACL)

Quando o callback retorna `AskUser` ou não há callback configurado, o sistema ramifica com base na identidade do Curator:

### Ramificação do Curator (IsCurator = true)

Para o Silicon Curator, o sistema solicita a decisão do utilizador via mensagens instantâneas:

```csharp
if (IsCurator)
{
    if (_askHandler != null)
    {
        AskPermissionResult userDecision = _askHandler.AskUser(callerId, permissionType, resource);
        // O utilizador confirma ou nega na Web UI
    }
}
```

### Ramificação Não Curator (IsCurator = false)

Para beings não Curator, o sistema verifica a Lista de Controlo de Acesso Global. Se não houver regra correspondente, o pedido é negado por defeito.

### Estrutura do GlobalACL

```json
{
  "rules": [
    {
      "permissionType": "NetworkAccess",
      "resourcePrefix": "api.github.com",
      "result": "Allowed"
    },
    {
      "permissionType": "FileAccess",
      "resourcePrefix": "C:\\Windows",
      "result": "Denied"
    }
  ]
}
```

As regras são avaliadas por ordem, e a primeira regra correspondente prevalece. Apenas o Silicon Curator pode modificar a ACL Global.

### Formato do Recurso

```
{tipo}:{caminho}

Exemplos:
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## IPermissionAskHandler

Quando uma operação do Curator requer confirmação do utilizador, as permissões são solicitadas através do `IPermissionAskHandler`.

### Implementação IMPermissionAskHandler

O `IMPermissionAskHandler` envia pedidos de permissão ao utilizador através da Web UI:

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        // Envia mensagem ao utilizador via mensagens instantâneas
        SendMessageAsync($"Permitir {resource}?");

        // Aguarda resposta do utilizador
        var response = WaitForResponseAsync();

        return response.Approved 
            ? AskPermissionResult.Approved()
            : AskPermissionResult.Denied();
    }
}
```

### Fila de Pedidos de Permissão (PermissionRequestQueue)

O `PermissionRequestQueue` gere pedidos de permissão pendentes, suportando espera assíncrona pela resposta do utilizador:

- **Enfileiramento de pedidos** — Quando a cadeia de permissões atinge o nível 5, cria um `TaskCompletionSource<AskPermissionResult>` e o enfileira
- **Exibição na Web UI** — Os pedidos de permissão pendentes são exibidos na Web UI através do `PermissionRequestController`
- **Resposta do utilizador** — O utilizador aprova ou nega na Web UI, podendo opcionalmente cachear a decisão e definir a duração do cache
- **Opções de cache** — O utilizador pode cachear decisões de permissão por 1 hora, 24 horas, 7 dias ou 30 dias
- **Mecanismo de timeout** — A página de pedido fecha automaticamente após 60 segundos sem resposta

## Sistema de Auditoria

Todas as decisões de permissões são registadas:

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "callerId": "being-uuid",
  "permissionType": "FileAccess",
  "resource": "C:\\data\\config.json",
  "result": "Allowed",
  "reason": "Global ACL"
}
```

## Avaliação Programática de Permissões

### API EvaluatePermission

O método `PermissionManager.EvaluatePermission()` fornece uma pré-avaliação de permissões só de leitura, sem accionar prompts ao utilizador. O `PermissionTool` usa este método para permitir que a IA verifique o estado das permissões antes de tentar uma operação.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**Valor de retorno**: `PermissionResult` de três estados:
- `Allowed` - Operação permitida
- `Denied` - Operação negada
- `AskUser` - Requer confirmação do utilizador na execução

**Ordem de avaliação**:
1. **Cache de frequência** - Verifica decisões do utilizador em cache
2. **IPermissionCallback** - Avaliação do callback personalizado
3. **Estado do Curator** - Se for Curator, retorna `AskUser` (requer confirmação)
4. **ACL Global** - Verifica regras de controlo de acesso
5. **Predefinição** - Nega quando não há regra correspondente

> **Nota**: Ao contrário da cadeia de permissões completa, `EvaluatePermission` **não** chama `IPermissionAskHandler`. Apenas reporta qual *será* o resultado na execução.

## Gerir Permissões

### Conceder Permissões

**Através da Web UI**:
1. Navegue para **Gestão de Permissões**
2. Clique em **Adicionar Regra**
3. Configure:
   - Utilizador
   - Recurso
   - Permitir/Negar
   - Duração

**Através da API**:
```bash
curl -X POST http://localhost:8080/api/permissions/save \
  -H "Content-Type: application/json" \
  -d '{
    "permissionType": "FileAccess",
    "resourcePrefix": "C:\\Projects",
    "result": "Allowed",
    "description": "Allow project directory access"
  }'
```

### Revogar Permissões

Através da página de gestão de permissões da Web UI.

### Visualizar Permissões

```bash
curl http://localhost:8080/api/permissions/list
```

## Sistema de Permissões de Ferramentas

Além da cadeia de verificação de permissões ao nível das operações, o sistema também fornece um mecanismo de gestão de **permissões de ferramentas** para controlar quais ferramentas os Silicon Beings podem usar.

### Permissões de Ferramentas em Dois Níveis

As permissões de ferramentas dividem-se em dois níveis:

1. **Nível do Silicon Being** — Controla quais operações de ferramentas um Silicon Being individual pode usar
2. **Nível do Projecto** — Controla as operações de ferramentas disponíveis no espaço do projecto, independente das permissões ao nível do Silicon Being

### Configuração de Permissões de Ferramentas

Cada operação de cada ferramenta pode ser configurada independentemente como permitida ou negada:

```json
{
  "beingId": "being-uuid",
  "permissions": {
    "network:get": "allowed",
    "network:post": "denied",
    "disk:read": "allowed",
    "disk:write": "denied",
    "database:query": "allowed"
  }
}
```

### Modelos de Permissões

O sistema fornece modelos de permissões de ferramentas predefinidos, que podem ser aplicados rapidamente aos Silicon Beings:

- **readonly** — Permissões só de leitura (permite operações de leitura, nega operações de escrita)
- **full** — Permissões completas (permite todas as operações)
- **restricted** — Permissões restritas (permite apenas operações básicas)

### Gestão pela Web UI

Gerir permissões de ferramentas através da Web UI:

- **Página de permissões de ferramentas do Silicon Being** — `/beings/tool-permissions`
- **Página de permissões de ferramentas do projecto** — `/project/{id}/tool-permissions`

### Endpoints da API

| Endpoint | Método | Descrição |
|------|------|------|
| `/api/beings/tool-permissions` | GET | Obter permissões de ferramentas do Silicon Being |
| `/api/beings/tool-permissions` | PUT | Actualizar permissões de ferramentas do Silicon Being |
| `/api/beings/tool-permissions/templates` | GET | Obter lista de modelos de permissões |
| `/api/beings/tool-permissions/apply-template` | POST | Aplicar modelo de permissões |
| `/api/projects/{id}/tool-permissions` | GET | Obter permissões de ferramentas do projecto |
| `/api/projects/{id}/tool-permissions` | PUT | Actualizar permissões de ferramentas do projecto |

---

## Melhores Práticas

### 1. Princípio do Menor Privilégio

Conceder apenas as permissões mínimas necessárias:

```json
{
  "permissionType": "FileAccess",
  "resourcePrefix": "C:\\Projects\\MyApp\\config.json",
  "result": "Allowed"
}
```

### 2. Usar Permissões com Limite Temporal

Nunca conceder permissões permanentes, a menos que seja absolutamente necessário.

### 3. Monitorizar Registos de Permissões

Rever regularmente os registos de auditoria para compreender:
- Tentativas de acesso negadas
- Padrões anómalos
- Escalonamento de permissões

### 4. Implementar Callbacks Personalizados

Para lógica complexa, usar `IPermissionCallback`:

```csharp
public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
{
    // Permissões baseadas no tempo
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }
    
    // Permissões baseadas no recurso
    if (IsSensitiveResource(resource))
    {
        return PermissionResult.AskUser;
    }
    
    return PermissionResult.Allowed;
}
```

## Cenários Comuns

### Cenário 1: A IA quer ler um ficheiro

```
IA: "Preciso de ler o config.json"
↓
Cadeia de permissões:
1. UserFrequencyCache? Sem decisão em cache
2. IPermissionCallback? Retorna AskUser (não explicitamente permitido)
3. IsCurator? Não → Verificar GlobalACL
4. GlobalACL? Regra encontrada: file:... = Allowed
5. Resultado: Permitido
```

### Cenário 2: A IA quer executar código

```
IA: "Quero compilar e executar código"
↓
Cadeia de permissões:
1. UserFrequencyCache? Sem decisão em cache
2. IPermissionCallback? Retorna AskUser
3. IsCurator? Sim → IPermissionAskHandler
4. Utilizador aprova
5. Resultado: Permitido
```

### Cenário 3: Negação em cache

```
IA: "Preciso de aceder a C:\Windows"
↓
Cadeia de permissões:
1. UserFrequencyCache? Encontrado no cache HighDeny
2. Resultado: Negado (sem necessidade de verificações adicionais)
```

## Resolução de Problemas

### Permissão Inesperadamente Negada

**Verificar**:
1. Estado IsCurator do utilizador
2. Entradas HighDeny no cache de frequência
3. Regras do GlobalACL
4. Lógica do callback
5. Timeout da resposta do utilizador

### Permissão Não Expira

**Verificar**:
- O campo `expiresAt` está definido correctamente
- O fuso horário está correcto
- Os relógios estão sincronizados

### Registos de Auditoria Não Gravados

**Verificar**:
- O auditor de registos está registado
- O backend de armazenamento está acessível
- Há espaço em disco suficiente

## Próximos Passos

- 📚 Leia o [guia de arquitectura](architecture.md)
- 🛠️ Consulte o [guia de desenvolvimento](development-guide.md)
- 🔒 Consulte a [documentação de segurança](security.md)
- 🚀 Consulte o [guia de início rápido](getting-started.md)
