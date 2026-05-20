# Segurança

> **Versão: v0.2.0-alpha**

[English](../en/security.md) | [Deutsch](../de-DE/security.md) | [Français](../fr-FR/security.md) | [中文](../zh-CN/security.md) | [繁體中文](../zh-HK/security.md) | [Español](../es-ES/security.md) | [日本語](../ja-JP/security.md) | [한국어](../ko-KR/security.md) | [Čeština](../cs-CZ/security.md) | [Italiano](../it-IT/security.md) | [Polski](../pl-PL/security.md) | **Português**

## Visão geral

O SiliconLifeCollective adota um modelo de segurança em camadas para proteger o sistema, os dados e os utilizadores. Este documento descreve as medidas de segurança implementadas e as boas práticas recomendadas.

---

## Modelo de segurança

### Arquitetura em camadas

```
┌─────────────────────────────────────┐
│         Camada de rede              │
│  ┌───────────────────────────────┐  │
│  │     Camada de aplicação       │  │
│  │  ┌─────────────────────────┐  │  │
│  │  │  Camada de permissões   │  │  │
│  │  │  ┌───────────────────┐  │  │  │
│  │  │  │  Camada de dados  │  │  │  │
│  │  │  └───────────────────┘  │  │  │
│  │  └─────────────────────────┘  │  │
│  └───────────────────────────────┘  │
└─────────────────────────────────────┘
```

### Princípios fundamentais

1. **Princípio do menor privilégio** — Cada Silicon Being opera apenas com as permissões estritamente necessárias
2. **Defesa em profundidade** — Múltiplas camadas de proteção em cada nível
3. **Negar por defeito** — Ações não explicitamente autorizadas são negadas
4. **Auditoria completa** — Todas as operações sensíveis são registadas

---

## Sistema de permissões

### Cadeia de verificação de 5 níveis

O sistema de permissões implementa uma cadeia de verificação de 5 níveis (ver [Sistema de permissões](permission-system.md) para detalhes):

1. **IsCurator** — O Curator tem todas as permissões
2. **UserFrequencyCache** — Cache de permissões temporárias
3. **GlobalACL** — Regras de controlo de acesso persistente
4. **IPermissionCallback** — Lógica de autorização personalizada
5. **IPermissionAskHandler** — Pedido interativo de permissão

### Tipos de permissões

| Categoria | Permissões | Descrição |
|-----------|------------|-------------|
| Disco | `disk:read`, `disk:write`, `disk:delete`, `disk:list` | Acesso ao sistema de ficheiros |
| Rede | `network:http`, `network:websocket`, `network:dns` | Acesso à rede |
| Sistema | `system:process`, `system:environment`, `system:clipboard` | Operações do sistema |
| Compilação | `compile:roslyn`, `compile:execute` | Compilação dinâmica |

---

## Segurança da compilação dinâmica

### Análise estática

O sistema realiza uma análise estática em tempo de execução do código antes da compilação:

```csharp
public class DynamicCompilationSecurityAnalyzer
{
    // Padrões de código perigosos bloqueados
    private static readonly string[] DangerousPatterns = {
        "System.Reflection",      // Reflexão
        "System.IO.File.Delete",  // Eliminação de ficheiros
        "Process.Start",          // Início de processos
        "DllImport",              // P/Invoke
        "unsafe",                 // Código inseguro
        "System.Net.WebRequest",  // Pedidos de rede não autorizados
    };
}
```

### Sandbox de execução

O código compilado dinamicamente é executado num ambiente restrito:

- **Sem acesso ao sistema de ficheiros** (exceto através de APIs autorizadas)
- **Sem acesso à rede** (exceto através de APIs autorizadas)
- **Tempo de execução limitado** (timeout configurável)
- **Memória limitada** (limite de alocação de memória)

### Encriptação do código

O código dinâmico é encriptado com AES-256:

```csharp
public class CodeEncryption
{
    // Encriptação AES-256
    public static string Encrypt(string code, byte[] key, byte[] iv);
    public static string Decrypt(string encrypted, byte[] key, byte[] iv);
}
```

---

## Segurança da rede

### Servidor HTTP

- **Ligação local apenas** — O servidor Web escuta apenas em `localhost`
- **Sem acesso remoto** — Ligações externas são rejeitadas por defeito
- **Timeout dos pedidos** — Prevenção contra ataques de negação de serviço

### Comunicação IA

- **HTTPS** — As ligações aos serviços IA na nuvem utilizam HTTPS
- **Chaves API** — As chaves API são armazenadas de forma segura e nunca expostas
- **Sem registo de chaves** — As chaves API nunca são registadas nos logs

---

## Segurança dos dados

### Armazenamento

- **Isolamento dos dados** — Os dados de cada Silicon Being são isolados
- **Permissões de ficheiros** — Os ficheiros de dados têm permissões restritas
- **Cópia de segurança** — Suporte para cópias de segurança manuais e automáticas

### Dados sensíveis

- **Chaves API** — Armazenadas na configuração, nunca registadas
- **Histórico de chat** — Armazenado localmente, nunca partilhado
- **Permissões** — Registadas no registo de auditoria

---

## Registo de auditoria

### Eventos registados

| Evento | Descrição | Nível |
|--------|-------------|-------|
| `PermissionGranted` | Permissão concedida | Informação |
| `PermissionDenied` | Permissão negada | Aviso |
| `PermissionRevoked` | Permissão revogada | Aviso |
| `PermissionRequested` | Pedido de permissão | Informação |
| `DynamicCompilation` | Compilação dinâmica | Aviso |
| `ToolExecution` | Execução de ferramenta | Informação |
| `AIRequest` | Pedido IA | Depuração |
| `SecurityViolation` | Violação de segurança | Erro |

### Formato do registo

```json
{
  "timestamp": "2026-04-20T10:30:00Z",
  "level": "warning",
  "event": "PermissionDenied",
  "beingId": "uuid",
  "resource": "disk:write:/etc/passwd",
  "reason": "Permissão não concedida"
}
```

---

## Boas práticas

### Para utilizadores

1. **Rever as permissões** — Verificar regularmente as permissões concedidas
2. **Duração limitada** — Usar durações curtas para a cache de permissões
3. **Não aprovar cegamente** — Ler os pedidos de permissão antes de aprovar
4. **Manter atualizado** — Manter o sistema atualizado

### Para programadores

1. **Validação de entrada** — Validar sempre as entradas do utilizador e da IA
2. **Parâmetros parametrizados** — Usar parâmetros em vez de concatenação de strings
3. **Tratamento de erros** — Nunca expor informações sensíveis nos erros
4. **Revisão de código** — Rever as alterações que afetam a segurança

---

## Vulnerabilidades conhecidas

### Risco de execução de código

A funcionalidade de compilação dinâmica permite que os Silicon Beings gerem e executem código. Embora existam múltiplas camadas de proteção (análise estática, sandbox, permissões), esta funcionalidade apresenta riscos inerentes.

**Mitigações**:
- Análise estática de padrões perigosos
- Sandbox de execução com recursos limitados
- Sistema de permissões em camadas
- Registo de auditoria completo

### Risco de acesso ao sistema de ficheiros

Os Silicon Beings podem solicitar acesso ao sistema de ficheiros através do sistema de permissões.

**Mitigações**:
- Princípio do menor privilégio
- Aprovação interativa para operações sensíveis
- Registo de auditoria de todas as operações de ficheiros

---

## Reportar vulnerabilidades

Se descobrires uma vulnerabilidade de segurança, por favor reporta-a de forma responsável:

1. **Não** criar uma issue pública
2. Enviar um email para os mantenedores
3. Incluir detalhes sobre a vulnerabilidade
4. Aguardar a resposta antes de divulgar

---

## Próximos passos

- 🔒 Ler o [sistema de permissões](permission-system.md)
- 📚 Ler a [documentação de arquitetura](architecture.md)
- 🛠️ Consultar o [guia de desenvolvimento](development-guide.md)
