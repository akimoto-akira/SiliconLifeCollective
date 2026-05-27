# PluginDemo-17: Empilhamento de capacidades — Permissões declarativas múltiplas

## Visão geral

Este plugin demonstra o empilhamento de múltiplos atributos `[PluginCapability]` numa única classe de plugin. `PluginCapabilityAttribute` tem `AllowMultiple = true`, pelo que pode declarar tantas capacidades quantas necessite.

## Sintaxe de empilhamento

```csharp
[PluginCapability(Capability.Network, Reason = "API endpoint access for remote AI models")]
[PluginCapability(Capability.AI, Reason = "AI service provider for downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

## Como o PluginLoader processa capacidades empilhadas

1. **Lê todas as declarações** da tabela CustomAttribute dos metadados PE
2. **Funde** as regras de isenção de todas as capacidades declaradas
3. **Regista independentemente** cada declaração com o seu próprio campo Reason
4. **Continua a impor** as proibições de capacidades não declaráveis independentemente do empilhamento

## Regras de isenção fundidas

Ao empilhar `Capability.Network` + `Capability.AI`:

| Fonte | Isenção |
|-------|--------|
| Capability.Network | System.Net.Http.*, System.Net.WebSockets.*, System.Net.Sockets.*, System.Net.Mail.*, System.Net.NetworkInformation.*, System.Net.Security.*, System.Net (proibições por tipo) |
| Capability.AI | Injeção de IAIService ativada |
| **Combinado** | O plugin pode usar HttpClient E IAIService |

## O empilhamento não concede poder ilimitado

Mesmo com múltiplas capacidades empilhadas, estas permanecem **sempre bloqueadas**:

- ❌ P/Invoke (`DllImport`, `Marshal`, `NativeMemory`)
- ❌ Código unsafe (`UnverifiableCodeAttribute`, `Unsafe`)
- ❌ Emissão IL (`System.Reflection.Emit.*`)
- ❌ Carregamento de assemblies (`System.Runtime.Loader`, `Assembly.Load*`)
- ❌ Registo (`Microsoft.Win32.*`)

Nenhum valor do enum `Capability` existe para estes — são **não declaráveis por conceção**.

## Trilha de auditoria para capacidades empilhadas

Cada capacidade é registada independentemente:

```
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.Network — reason: API endpoint access for remote AI models
Security audit: [AiConnectorPlugin] com.example.connector declared Capability.AI — reason: AI service provider for downstream plugins
```

## Ficheiros

- `Plugin.cs` — Plugin de demonstração com empilhamento Capability.Network + Capability.AI
- `README.md` — Este ficheiro (Inglês)
- `README.zh-CN.md` — Chinês simplificado
- Traduções: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Exemplos relacionados

- **13-CapabilityNetwork**: Capacidade Network única
- **16-CapabilityAI**: Capacidade IA única
- **18-CapabilityDenied**: Anti-padrão de capacidades não declaráveis
