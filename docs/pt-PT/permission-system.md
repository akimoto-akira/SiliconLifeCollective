# Sistema de permissões

> **Versão: v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [Français](../fr-FR/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | [Italiano](../it-IT/permission-system.md) | [Polski](../pl-PL/permission-system.md) | **Português**

## Visão geral

O sistema de permissões garante que todas as operações iniciadas pela IA sejam devidamente verificadas e auditadas.

## Cadeia de permissões de 3 níveis

```
┌─────────────────────────────────────────────┐
│          Verificação de permissões           │
├─────────────────────────────────────────────┤
│  Nível 1: UserFrequencyCache                │
│  ↓ Decisões de alta frequência do utilizador (HighDeny/HighAllow)│
│  Nível 2: IPermissionCallback               │
│  ↓ Lógica personalizada (Allowed/Denied/AskUser)│
│  Nível 3: IsCurator?                        │
│  ↓ Sim → IPermissionAskHandler (perguntar ao utilizador)│
│  ↓ Não → GlobalACL → Negar por predefinição │
│  Resultado: Permitir ou Negar               │
└─────────────────────────────────────────────┘
```

> **Nota**: A prioridade de consulta real em `PermissionManager.CheckPermission()` é:
> 1. **UserFrequencyCache** — Verifica primeiro as decisões de alta frequência do utilizador em cache
> 2. **IPermissionCallback** — Avalia as regras de callback personalizadas
> 3. **Ramificação do Curator** — Se o callback retornar AskUser ou não houver callback:
>    - **Curator** → `IPermissionAskHandler` (solicitar ao utilizador via IM)
>    - **Não-curator** → `GlobalACL` → negar por predefinição

## Nível 1: UserFrequencyCache

Cache **por being, apenas em memória** de decisões de alta frequência do utilizador (HighDeny/HighAllow).

```csharp
var cache = new UserFrequencyCache();
PermissionResult? cachedResult = cache.Query(permissionType, resource);
if (cachedResult.HasValue)
{
    return cachedResult.Value == PermissionResult.Allowed;
}
```

- **HighDeny** tem prioridade sobre **HighAllow**
- **Apenas em memória**: As caches não são persistidas, perdem-se ao reiniciar
- **Expiração configurável**: O utilizador pode definir o prazo de validade das entradas da cache

## Nível 2: IPermissionCallback

Callback personalizado para lógica dinâmica de permissões.

### Implementação padrão DefaultPermissionCallback

O `DefaultPermissionCallback` fornece regras de permissão padrão abrangentes, incluindo:

#### Regras de acesso à rede
- **Endereços de loopback**: Permite localhost, 127.0.0.1, ::1
- **Endereços IP privados**:
  - 192.168.x.x (Classe C) - Permitido
  - 10.x.x.x (Classe A) - Permitido
  - 172.16-31.x.x (Classe B) - Perguntar ao utilizador
- **Lista de domínios permitidos**:
  - Motores de busca: Google, Bing, DuckDuckGo, Yandex, Sogou, etc.
  - Serviços de IA: OpenAI, Anthropic, HuggingFace, Ollama, etc.
  - Serviços para programadores: GitHub, StackOverflow, npm, NuGet, etc.
  - Redes sociais: Weibo, Zhihu, Reddit, Discord, etc.
  - Plataformas de vídeo: YouTube, Bilibili, Douyin, TikTok, etc.
  - **Informações meteorológicas**: wttr.in
  - Sites governamentais: .gov, .go.jp, .go.kr
- **Lista de domínios bloqueados**:
  - Sites de imitação de IA: chatgpt, openai, deepseek e outros domínios falsificados
  - Ferramentas de IA maliciosas: wormgpt, darkgpt, fraudgpt, etc.
  - Domínios relacionados com fazendas de conteúdo de IA e mercados negros

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

## Nível 3: Ramificação do Curator (IsCurator → AskHandler / GlobalACL)

Quando o callback retorna `AskUser` ou não há callback configurado, o sistema ramifica com base no estado de curator:

### Caminho do Curator: IPermissionAskHandler

Para o curator de silício, o sistema solicita ao utilizador uma decisão via IM.

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        SendMessage($"Permitir {resource}?");

        var response = WaitForResponse();

        return new AskPermissionResult
        {
            Allowed = response.Approved,
            AddToCache = response.AddToCache,
            CacheDuration = response.CacheDuration
        };
    }
}
```

### Caminho do Não-Curator: GlobalACL → Negar por Predefinição

Para beings não-curators, o sistema verifica a Lista de Controlo de Acesso Global. Se não for encontrada uma regra correspondente, o pedido é negado por predefinição.

### Estrutura do GlobalACL

```json
{
  "rules": [
    {
      "prefix": "network:api.github.com",
      "result": "Allowed"
    },
    {
      "prefix": "file:C:\\Windows",
      "result": "Denied"
    }
  ]
}
```

As regras são avaliadas por ordem; a primeira correspondência vence. Apenas o curator de silício pode modificar a ACL global.

### Formato dos recursos

```
{tipo}:{caminho}

Exemplos:
- network:api.github.com
- file:C:\\Windows
- cli:rm -rf
```

## IPermissionAskHandler

Quando uma operação do curator requer confirmação do utilizador, o `IPermissionAskHandler` solicita a permissão.

### Implementação IMPermissionAskHandler

O `IMPermissionAskHandler` envia pedidos de permissão ao utilizador através da interface Web:

```csharp
public class IMPermissionAskHandler : IPermissionAskHandler
{
    public AskPermissionResult AskUser(Guid callerId, PermissionType permissionType, string resource)
    {
        SendMessage($"Permitir {resource}?");

        var response = WaitForResponse();

        return new AskPermissionResult
        {
            Allowed = response.Approved,
            AddToCache = response.AddToCache,
            CacheDuration = response.CacheDuration
        };
    }
}
```

### Fila de pedidos de permissão PermissionRequestQueue

O `PermissionRequestQueue` gere os pedidos de permissão pendentes, suportando a espera assíncrona pela resposta do utilizador:

- **Entrada na fila** — Quando a cadeia de permissões atinge o ramo do Curator, cria um `TaskCompletionSource<AskPermissionResult>` e insere-o na fila
- **Apresentação na interface Web** — Apresenta os pedidos de permissão pendentes através do `PermissionRequestController` na interface Web
- **Resposta do utilizador** — O utilizador aprova ou rejeita na interface Web, podendo optar por armazenar a decisão em cache e definir a duração da cache
- **Opções de cache** — O utilizador pode armazenar a decisão de permissão em cache durante 1 hora, 24 horas, 7 dias ou 30 dias
- **Mecanismo de timeout** — O pedido é automaticamente encerrado após 60 segundos sem resposta

## Sistema de auditoria

Todas as decisões de permissões são registadas:

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "userId": "user-uuid",
  "resource": "disk:write",
  "allowed": true,
  "level": "GlobalACL",
  "reason": "Explicit rule granted"
}
```

## Avaliação programática de permissões

### API EvaluatePermission

O método `PermissionManager.EvaluatePermission()` fornece uma pré-avaliação de permissões apenas de leitura, sem acionar pedidos ao utilizador. O `PermissionTool` utiliza este método para permitir que a IA verifique o estado das permissões antes de tentar uma operação.

```csharp
public PermissionResult EvaluatePermission(
    Guid callerId,
    PermissionType permissionType,
    string resource)
```

**Valores de retorno**: Resultado de três estados `PermissionResult`:
- `Allowed` - Operação permitida
- `Denied` - Operação negada
- `AskUser` - Requer confirmação do utilizador durante a execução

**Ordem de avaliação**:
1. **UserFrequencyCache** - Verifica as decisões do utilizador em cache
2. **IPermissionCallback** - Avaliação por callback personalizado
3. **Ramificação do Curator** - Se for curator, retorna `AskUser` (requer confirmação); se não for curator, verifica **GlobalACL**, depois nega por predefinição

> **Nota**: Ao contrário da cadeia de permissões completa, o `EvaluatePermission` **não** chama o `IPermissionAskHandler`. Apenas informa qual será o resultado *quando* a operação for executada.

## Gestão de permissões

### Conceder permissões

**Através da interface Web**:
1. Navegar até **Gestão de permissões**
2. Clicar em **Adicionar regra**
3. Configurar:
   - Utilizador
   - Recurso
   - Permitir/Negar
   - Duração

**Através da API**:
```bash
curl -X POST http://localhost:8080/api/permissions \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "user-uuid",
    "resource": "disk:write",
    "allowed": true,
    "duration": 3600
  }'
```

### Revogar permissões

```bash
curl -X DELETE http://localhost:8080/api/permissions/{rule-id}
```

### Ver permissões

```bash
curl http://localhost:8080/api/permissions?userId=user-uuid
```

## Boas práticas

### 1. Princípio do menor privilégio

Conceder apenas as permissões estritamente necessárias:

```json
{
  "resource": "disk:read",
  "allowed": true,
  "expiresAt": "2026-04-21T00:00:00Z"
}
```

### 2. Utilizar permissões com limite temporal

Nunca conceder permissões permanentes, a menos que seja absolutamente necessário.

### 3. Monitorizar os registos de permissões

Rever regularmente os registos de auditoria para compreender:
- Tentativas de acesso negadas
- Padrões anómalos
- Escalonamento de permissões

### 4. Implementar callbacks personalizados

Para lógica complexa, utilizar `IPermissionCallback`:

```csharp
public PermissionResult Evaluate(Guid callerId, PermissionType permissionType, string resource)
{
    if (IsOutsideBusinessHours())
    {
        return PermissionResult.Denied;
    }

    if (IsSensitiveResource(resource))
    {
        return PermissionResult.AskUser;
    }

    return PermissionResult.Allowed;
}
```

## Cenários comuns

### Cenário 1: A IA quer ler um ficheiro

```
IA: "Preciso de ler o config.json"
↓
Cadeia de permissões:
1. UserFrequencyCache? Sem decisão em cache
2. IPermissionCallback? Retorna AskUser (não explicitamente permitido)
3. IsCurator? Não → Verificar GlobalACL
4. GlobalACL? Encontrou regra: file:... = Permitido
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
IA: "Preciso de aceder ao C:\Windows"
↓
Cadeia de permissões:
1. UserFrequencyCache? Encontrado na cache HighDeny
2. Resultado: Negado (sem necessidade de verificações adicionais)
```

## Resolução de problemas

### Permissão inesperadamente negada

**Verificar**:
1. Estado IsCurator do utilizador
2. Configuração do limite de taxa
3. Regras do GlobalACL
4. Lógica do callback
5. Timeout da resposta do utilizador

### Permissão não expira

**Verificar**:
- Campo `expiresAt` definido corretamente
- Fuso horário correto
- Relógios sincronizados

### Registo de auditoria não gravado

**Verificar**:
- Registo de auditoria registado
- Backend de armazenamento acessível
- Espaço em disco suficiente

## Próximos passos

- 📚 Ler a [documentação de arquitetura](architecture.md)
- 🛠️ Consultar o [guia de desenvolvimento](development-guide.md)
- 🔐 Ler a [documentação de segurança](security.md)
- 🚀 Ver o [guia rápido](getting-started.md)
