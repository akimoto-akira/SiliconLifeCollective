# PluginDemo-16: Capability.AI — Permissão de serviço IA declarativa

## Visão geral

Este plugin demonstra o uso de `[PluginCapability(Capability.AI)]` para declarar que um plugin requer acesso ao serviço de IA. Ao contrário de outras capacidades, `Capability.AI` **não** isenta nenhum namespace proibido — em vez disso, permite ao host injetar uma referência `IAIService` no plugin.

## Conceito-chave: Capability.AI não concede acesso à rede

`Capability.AI` é fundamentalmente diferente das outras capacidades:

| Capacidade | O que isenta | Como funciona |
|-----------|-------------|--------------|
| `Capability.Network` | Namespaces `System.Net.*` | Relaxa as regras de verificação TypeRef/ILString |
| `Capability.FileIO` | Namespace `System.IO` | Relaxa as regras de verificação TypeRef/ILString |
| `Capability.Process` | Tipos `Process*` | Relaxa as regras de verificação TypeRef/ILString |
| `Capability.AI` | **Nada** | Permite a injeção de IAIService pelo host |

`IAIService` encontra-se no namespace `SiliconLife.Collective` — nunca está em qualquer lista de proibição. A declaração de capacidade é um **sinal de opt-in** ao host de que este plugin deve receber a referência do serviço de IA.

## Empilhamento de capacidades: IA + Rede

Se o seu cliente de IA necessita de acesso direto à rede (ex: chamar um endpoint de IA remoto), deve declarar **ambas** as capacidades:

```csharp
[PluginCapability(Capability.Network, Reason = "Calls remote AI endpoint")]
[PluginCapability(Capability.AI, Reason = "Provides IAIService to downstream plugins")]
public class AiConnectorPlugin : IPlugin { ... }
```

Ver **17-CapabilityStacked** para exemplos completos de empilhamento.

## Padrão de ponto de entrada controlado

| Recurso | Ponto de entrada controlado | Capacidade necessária |
|---------|---------------------------|---------------------|
| Ficheiros | `PermissionedStreamFactory` | Nenhuma |
| Rede | `NetworkExecutor` | Nenhuma |
| Processos | `CommandLineExecutor` | Nenhuma |
| Armazenamento de dados | `SpeedyPack` | Nenhuma |
| Serviço de IA | `IAIService` | `Capability.AI` |

`IAIService` é único: **requer** uma declaração de capacidade. O acesso ao serviço de IA é uma funcionalidade opt-in, não uma capacidade predefinida disponível para todos os plugins.

## Ficheiros

- `Plugin.cs` — Plugin de demonstração que declara Capability.AI
- `README.md` — Este ficheiro (Inglês)
- `README.zh-CN.md` — Chinês simplificado
- Traduções: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Exemplos relacionados

- **17-CapabilityStacked**: Empilhamento de capacidades múltiplas (Rede + IA)
- **18-CapabilityDenied**: Anti-padrão de capacidades não declaráveis
