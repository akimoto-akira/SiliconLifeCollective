# PluginDemo-13: Capability.Network — Permissão de rede declarativa

## Visão geral

Este plugin demonstra o uso de `[PluginCapability(Capability.Network)]` para declarar acesso à rede. Com esta declaração, o plugin pode aceder aos tipos `System.Net.*` que seriam bloqueados pela análise de segurança do PluginLoader.

## Sintaxe de declaração

```csharp
[PluginCapability(Capability.Network, Reason = "Calls weather REST API to retrieve forecast data")]
public class CapabilityNetworkPlugin : IPlugin { ... }
```

**Elementos-chave:**
- **Alvo do atributo**: Deve estar na classe que implementa diretamente `IPlugin`
- **AllowMultiple = true**: Podem ser empilhados múltiplos atributos `[PluginCapability]` (ver 17-CapabilityStacked)
- **Campo Reason**: Explicação legível por humanos escrita no registo de auditoria de segurança ao carregar. **Fornecer um Reason claro é fortemente recomendado para todos os plugins em produção.**

## Como o PluginLoader processa as declarações de capacidade

1. **Leitura de metadados PE**: O PluginLoader lê as declarações de capacidade da tabela CustomAttribute do ficheiro PE **antes** de a análise de segurança começar
2. **Relaxamento das regras de análise**: As capacidades declaradas isentam as referências de tipo correspondentes das verificações de namespaces e tipos proibidos
3. **Registo de auditoria**: Todas as declarações (incluindo Reason) são escritas no registo de auditoria de segurança
4. **Capacidades não declaráveis**: P/Invoke, Unsafe, Reflection.Emit, etc. permanecem bloqueados independentemente de qualquer declaração

## Âmbito de isenção de Capability.Network

### Isenções TypeRef

Quando `Capability.Network` é declarada, as seguintes regras de proibição baseadas em namespaces e tipos são relaxadas:

| Namespace isento | Tipos permitidos |
|-----------------|-----------------|
| `System.Net.Http` | `HttpClient`, `HttpRequestMessage`, `HttpResponseMessage`, etc. |
| `System.Net.WebSockets` | `ClientWebSocket`, `WebSocket`, etc. |
| `System.Net.Sockets` | `TcpClient`, `UdpClient`, `Socket`, etc. |
| `System.Net.Mail` | `SmtpClient`, `MailMessage`, etc. |
| `System.Net.NetworkInformation` | `Ping`, `NetworkInterface`, etc. |
| `System.Net.Security` | `SslStream`, etc. |
| `System.Net` (proibições por tipo) | `HttpWebRequest`, `WebClient`, `Dns`, `FtpWebRequest`, etc. |

### Isenções ILString

As constantes de cadeia que começam com estes prefixos não são sinalizadas na análise da heap #US:
- `"System.Net.Http"`
- `"System.Net.WebSockets"`
- `"System.Net.Sockets"`
- `"System.Net.Mail"`
- `"System.Net.NetworkInformation"`
- `"System.Net.Security"`

### O que permanece proibido

Mesmo com `Capability.Network`, estas capacidades estão **sempre** bloqueadas (capacidades não declaráveis):

| Categoria | Tipos bloqueados | Porque não declarável |
|----------|-----------------|---------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` | Não pode ser auditado em segurança em tempo de execução |
| Código unsafe | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | Contorna as garantias de segurança de tipos |
| Emissão IL | `System.Reflection.Emit.*` | Pode gerar código arbitrário em tempo de execução |
| Carregamento de assemblies | `System.Runtime.Loader`, `Assembly.Load*` | Pode contornar a análise de segurança carregando DLLs não verificadas |
| Registo | `Microsoft.Win32.*` | Acesso de sistema ao nível do SO fora do sandbox do plugin |

## Campo Reason — Papel de auditoria

O campo `Reason` serve como **trilha de auditoria** para as declarações de capacidade:

```
Security audit: [CapabilityNetworkPlugin] com.siliconlife.demo.capabilitynetwork declared Capability.Network — reason: Calls weather REST API to retrieve forecast data
```

**Porque Reason é importante:**
1. **Revisão de segurança**: Os auditores podem verificar se as capacidades declaradas correspondem ao comportamento real do plugin
2. **Princípio do menor privilégio**: Obriga os autores de plugins a justificar a necessidade de cada capacidade
3. **Conformidade**: Necessário para certificações de segurança e investigações de incidentes
4. **Monitorização em tempo de execução**: As ferramentas de segurança podem alertar se a utilização da capacidade declarada excede o motivo indicado

## Comparação com 08-ForbiddenNetwork

| Aspeto | 08-ForbiddenNetwork | 13-CapabilityNetwork |
|--------|-------------------|---------------------|
| Declaração | Nenhuma | `[PluginCapability(Capability.Network)]` |
| Resultado do carregamento | ❌ Rejeitado | ✅ Carregado com sucesso |
| Uso de HttpClient | Bloqueado pela análise TypeRef | Isento pela capacidade |
| Uso de TcpClient | Bloqueado pela análise TypeRef | Isento pela capacidade |
| Reason | Não aplicável | Escrito no registo de auditoria |

**Diferença-chave**: 08-ForbiddenNetwork mostra o que acontece quando se usam tipos de rede **sem** declarar a capacidade. 13-CapabilityNetwork mostra a maneira **correta** de solicitar declarativamente o acesso à rede.

## Boas práticas de segurança

1. **Declarar apenas o necessário**: Se só precisa de HTTP, não declare Capability.Network só porque pode — mas note que Capability.Network é a única capacidade relacionada com a rede; não há opções mais granulares
2. **Preferir NetworkExecutor**: `NetworkExecutor` é o ponto de entrada controlado para acesso à rede e não requer nenhuma declaração de capacidade
3. **Fornecer um Reason claro**: Razões vagas como "acesso à rede" são um sinal de alerta nas revisões de segurança
4. **Respeitar os limites não declaráveis**: Nenhuma declaração de capacidade pode contornar as proibições de P/Invoke, Unsafe ou Reflection.Emit

## Ficheiros

- `Plugin.cs` — Plugin de demonstração declarando Capability.Network
- `README.md` — English
- `README.zh-CN.md` — 简体中文
- `README.zh-HK.md` — 繁體中文
- `README.ja-JP.md` — 日本語
- `README.ko-KR.md` — 한국어
- `README.de-DE.md` — Deutsch
- `README.fr-FR.md` — Français
- `README.es-ES.md` — Español
- `README.it-IT.md` — Italiano
- `README.ru-RU.md` — Русский
- `README.pt-PT.md` — Este ficheiro (Português)
- `README.pl-PL.md` — Polski
- `README.cs-CZ.md` — Čeština

## Exemplos relacionados

- **08-ForbiddenNetwork**: Anti-padrão mostrando operações de rede bloqueadas
- **14-CapabilityFileIO**: Capacidade FileIO declarativa
- **15-CapabilityProcess**: Capacidade Process declarativa
- **16-CapabilityAI**: Capacidade de serviço IA declarativa
- **17-CapabilityStacked**: Empilhamento de capacidades múltiplas
- **18-CapabilityDenied**: Anti-padrão de capacidade não declarável
