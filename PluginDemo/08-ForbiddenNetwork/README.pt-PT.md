# PluginDemo-08: Anti-Padrão de Operações de Rede Proibidas

## Visão Geral

Este plugin demonstra operações de rede **PROIBIDAS** no sistema de plugins SiliconLife. Serve como referência anti-padrão, mostrando o que NÃO fazer e fornecendo alternativas corretas.

## Por que o acesso direto à rede é globalmente proibido?

Os padrões de acesso direto à rede são bloqueados no nível do plugin:

1. **Conexão a servidores maliciosos**: Plugins podem se conectar a servidores maliciosos
2. **Exfiltração de dados**: Plugins podem vazar dados sensíveis do sandbox
3. **Ataques DNS Rebinding**: Plugins podem contornar verificações de segurança
4. **Bypass de ACL de rede**: O acesso direto à rede ignora o sistema ACL global

## Tipos Proibidos

Todos os tipos `System.Net` que acessam diretamente a rede são bloqueados:

| Tipo proibido | Espaço de nomes bloqueado | Nível de risco |
|----------|----------------|----------|
| `HttpClient` | `System.Net.Http` | 🔴 Crítico |
| `TcpClient` | `System.Net.Sockets` | 🔴 Crítico |
| `Socket` | `System.Net.Sockets` | 🔴 Crítico |
| `Dns` | `System.Net` | 🔴 Crítico |
| `WebClient` | `System.Net` | 🔴 Crítico |

## Métodos de Acesso Seguro

### NetworkExecutor (Recomendado)

`NetworkExecutor` é o **ponto de entrada controlado** para operações de rede:

```csharp
// ✅ CORRETO: Solicitação GET simples
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com/data"
});
```

**O que o NetworkExecutor fornece:**
1. Verificação de permissões
2. Registro de auditoria
3. Disjuntor
4. Controle de tempo limite
5. Fila de solicitações

## Violações Demonstradas

### Violação 1: HttpClient

```csharp
// ❌ PROIBIDO
using var client = new HttpClient();

// ✅ CORRETO
var result = NetworkExecutor.Execute(new ExecutorRequest
{
    ResourcePath = "https://api.example.com"
});
```

### Violação 2: TcpClient

```csharp
// ❌ PROIBIDO
using var client = new TcpClient("example.com", 8080);

// ✅ CORRETO
// Usar NetworkExecutor ou declarar Capability.Network
```

## Mecanismo de Segurança do PluginLoader

PluginLoader analisa este plugin e:
1. **Escaneamento TypeRef**: Detecta referências a tipos proibidos
2. **Escaneamento MemberRef**: Detecta chamadas a métodos bloqueados
3. **Escaneamento de string IL**: Detecta tentativas de reflexão
4. **Rejeição**: O plugin é rejeitado no carregamento

## Arquivos

- `Plugin.cs` - Plugin de demonstração anti-padrão
- `README.md` - Este arquivo (Inglês)
- `README.pt-PT.md` - Este arquivo (Português)
- Outras versões de idiomas...

## Exemplos Relacionados

- **13-CapabilityNetwork**: Capacidade de rede declarativa
- **07-ForbiddenFileIO**: Padrões de acesso a arquivos proibidos