# Sistema de permissões

> **Versão: v0.2.0-alpha**

[English](../en/permission-system.md) | [Deutsch](../de-DE/permission-system.md) | [Français](../fr-FR/permission-system.md) | [中文](../zh-CN/permission-system.md) | [繁體中文](../zh-HK/permission-system.md) | [Español](../es-ES/permission-system.md) | [日本語](../ja-JP/permission-system.md) | [한국어](../ko-KR/permission-system.md) | [Čeština](../cs-CZ/permission-system.md) | [Italiano](../it-IT/permission-system.md) | [Polski](../pl-PL/permission-system.md) | **Português**

## Visão geral

O sistema de permissões do SiliconLifeCollective adota um modelo de segurança em camadas, garantindo que os Silicon Beings operam dentro dos limites autorizados. O sistema suporta verificação de permissões em múltiplos níveis, mecanismos de callback e gestão de cache.

---

## Arquitetura do sistema de permissões

### Cadeia de verificação de permissões (5 níveis)

```
Pedido de permissão
    │
    ▼
┌─────────────────────┐
│  1. IsCurator       │ ← O Curator tem todas as permissões
│     Verificação      │
└─────────┬───────────┘
          │ Não é Curator
          ▼
┌─────────────────────┐
│  2. UserFrequency   │ ← Cache de frequência do utilizador
│     Cache           │   (permissões temporárias)
└─────────┬───────────┘
          │ Cache expirada
          ▼
┌─────────────────────┐
│  3. GlobalACL       │ ← Lista de controlo de acesso global
│     Verificação      │   (regras de permissão persistentes)
└─────────┬───────────┘
          │ Sem regra ACL
          ▼
┌─────────────────────┐
│  4. IPermission     │ ← Callback de permissão
│     Callback        │   (lógica de autorização personalizada)
└─────────┬───────────┘
          │ Sem callback
          ▼
┌─────────────────────┐
│  5. IPermission     │ ← Pedido interativo de permissão
│     AskHandler      │   (interface de aprovação do utilizador)
└─────────────────────┘
```

### Descrição dos níveis

| Nível | Componente | Descrição | Prioridade |
|-------|-----------|-------------|------------|
| 1 | `IsCurator` | O Silicon Curator tem todas as permissões, sem necessidade de verificação adicional | Mais alta |
| 2 | `UserFrequencyCache` | Cache de permissões temporárias, evita verificações repetidas | Alta |
| 3 | `GlobalACL` | Regras de controlo de acesso persistente, suporta autorização/negação | Média |
| 4 | `IPermissionCallback` | Lógica de autorização personalizada, injetada em tempo de execução | Baixa |
| 5 | `IPermissionAskHandler` | Pedido interativo de permissão, requer aprovação manual do utilizador | Mais baixa |

---

## Tipos de permissões

### Permissões do sistema de ficheiros

| Permissão | Descrição | Exemplo |
|-----------|-------------|---------|
| `disk:read` | Ler ficheiros | Ler ficheiros de configuração |
| `disk:write` | Escrever ficheiros | Modificar ficheiros de configuração |
| `disk:delete` | Eliminar ficheiros | Limpar ficheiros temporários |
| `disk:list` | Listar diretórios | Listar ficheiros de dados |

### Permissões de rede

| Permissão | Descrição | Exemplo |
|-----------|-------------|---------|
| `network:http` | Pedidos HTTP | Chamar APIs externas |
| `network:websocket` | Ligações WebSocket | Comunicação em tempo real |
| `network:dns` | Resolução DNS | Resolver nomes de domínio |

### Permissões do sistema

| Permissão | Descrição | Exemplo |
|-----------|-------------|---------|
| `system:process` | Gestão de processos | Iniciar/parar processos |
| `system:environment` | Variáveis de ambiente | Ler configurações do sistema |
| `system:clipboard` | Área de transferência | Ler/escrever na área de transferência |

### Permissões de compilação dinâmica

| Permissão | Descrição | Exemplo |
|-----------|-------------|---------|
| `compile:roslyn` | Compilação dinâmica | Auto-evolução dos Silicon Beings |
| `compile:execute` | Execução de código | Executar código compilado |

---

## API de permissões

### Verificar uma permissão

```csharp
bool hasPermission = await permissionManager.CheckPermissionAsync(
    beingId,
    "disk:read",
    "/data/config.json"
);
```

### Pedir uma permissão

```csharp
var result = await permissionManager.RequestPermissionAsync(
    beingId,
    "disk:write",
    "/data/output.json",
    reason: "Necessário guardar os resultados do processamento"
);
```

### Conceder uma permissão

```csharp
await permissionManager.GrantPermissionAsync(
    beingId,
    "disk:read",
    "/data/config.json",
    duration: TimeSpan.FromHours(1)
);
```

### Revogar uma permissão

```csharp
await permissionManager.RevokePermissionAsync(
    beingId,
    "disk:read",
    "/data/config.json"
);
```

---

## Cache de frequência

O `UserFrequencyCache` é um mecanismo de cache de permissões temporárias que reduz a frequência dos pedidos de permissão:

- **Duração da cache**: Configurável, por defeito 1 hora
- **Chave da cache**: Combinação do ID do Being + tipo de permissão + recurso
- **Expiração automática**: As entradas da cache expiram automaticamente após a duração definida
- **Limpeza ativa**: Limpeza periódica das entradas expiradas

---

## Lista de controlo de acesso global (GlobalACL)

O `GlobalACL` gere as regras de permissão persistente:

### Estrutura das regras

```csharp
public class ACLRule
{
    public Guid UserId { get; set; }
    public string Resource { get; set; }
    public bool Allowed { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string Reason { get; set; }
}
```

### Operações

- **Adicionar regra**: `AddRule(ACLRule rule)`
- **Remover regra**: `RemoveRule(Guid userId, string resource)`
- **Verificar regra**: `CheckRule(Guid userId, string resource)`
- **Listar regras**: `GetRules(Guid userId)`

---

## Pedido interativo de permissão

Quando um Silicon Being precisa de uma permissão que não está nem na cache nem no ACL, o sistema aciona um pedido interativo:

1. O Being pede a permissão através de `IPermissionAskHandler`
2. O sistema mostra uma notificação na interface Web
3. O utilizador pode aprovar ou rejeitar o pedido
4. A decisão é registada no ACL (opcional)

### Fluxo do pedido de permissão

```
Being pede permissão
    │
    ▼
Notificação enviada à interface Web
    │
    ▼
┌────────────────────────────────┐
│  Interface de aprovação        │
│  ┌──────────────────────────┐  │
│  │ Silicon Being: Assistente│  │
│  │ Permissão: disk:write    │  │
│  │ Recurso: /data/out.json  │  │
│  │ Motivo: Guardar dados    │  │
│  │                          │  │
│  │ [✅ Aprovar] [❌ Rejeitar]│  │
│  │ ☑️ Lembrar decisão (1h)  │  │
│  └──────────────────────────┘  │
└────────────────────────────────┘
    │
    ▼
Decisão registada → Cache/ACL atualizado
```

---

## Registo de auditoria

Todas as operações de permissões são registadas no `AuditLogger`:

### Tipos de registos

| Tipo | Descrição |
|------|-------------|
| `PermissionGranted` | Permissão concedida |
| `PermissionDenied` | Permissão negada |
| `PermissionRevoked` | Permissão revogada |
| `PermissionExpired` | Permissão expirada |
| `PermissionRequested` | Pedido de permissão |

### Estrutura do registo

```csharp
public class AuditLogEntry
{
    public DateTime Timestamp { get; set; }
    public Guid BeingId { get; set; }
    public string Action { get; set; }
    public string Resource { get; set; }
    public bool Allowed { get; set; }
    public string Reason { get; set; }
}
```

---

## Boas práticas

### 1. Princípio do menor privilégio

Conceder apenas as permissões estritamente necessárias:
- ✅ `disk:read` em vez de `disk:write` quando apenas é necessário ler
- ✅ Especificar caminhos exatos em vez de caminhos gerais
- ❌ Evitar conceder `disk:*` ou `network:*`

### 2. Duração razoável da cache

- Operações de leitura: Cache mais longa (1-4 horas)
- Operações de escrita: Cache mais curta (15-30 minutos)
- Operações de eliminação: Sem cache

### 3. Regras ACL claras

- Adicionar uma razão para cada regra
- Definir tempo de expiração
- Rever as regras regularmente

---

## Próximos passos

- 📚 Ler a [documentação de arquitetura](architecture.md)
- 🛠️ Consultar o [guia de desenvolvimento](development-guide.md)
- 🔐 Ler a [documentação de segurança](security.md)
