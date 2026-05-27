# PluginDemo-15: Capability.Process — Permissão declarativa de processo

## Visão geral

Este plugin demonstra o uso de `[PluginCapability(Capability.Process)]` para declarar que um plugin requer a capacidade de lançar processos filhos. Com esta declaração, o plugin acede a `System.Diagnostics.Process` e tipos relacionados.

## Sintaxe de declaração

```csharp
[PluginCapability(Capability.Process, Reason = "Launch build tools for CI pipeline")]
public class CapabilityProcessPlugin : IPlugin { ... }
```

## Âmbito de isenção do Capability.Process

### Isenções TypeRef

Apenas os tipos relacionados com Process sob `System.Diagnostics` são isentados:

| Tipo isentado | Utilização |
|-------------|-----------|
| `Process` | Iniciar, gerir e monitorizar processos filhos |
| `ProcessStartInfo` | Configurar parâmetros de arranque do processo |
| `ProcessThread` | Aceder a informações de threads do processo |
| `ProcessModule` | Aceder a informações de módulos do processo |
| `ProcessPriorityClass` | Definir prioridade do processo |
| `ProcessWindowStyle` | Configurar estilo de janela do processo |

Tipos sempre permitidos (nunca na lista de proibição): `Stopwatch`, `Debug`, `Trace`, `Activity`

### Isenção ILString

- Cadeias que começam com `"System.Diagnostics.Process"` não são sinalizadas

## Comparação com 09-ForbiddenProcess

| Aspeto | 09-ForbiddenProcess | 15-CapabilityProcess |
|--------|-------------------|---------------------|
| Declaração | Nenhuma | `[PluginCapability(Capability.Process)]` |
| Process.Start | ❌ REJEITADO | ✅ PERMITIDO |
| ProcessStartInfo | ❌ REJEITADO | ✅ PERMITIDO |

## Recomendação: CommandLineExecutor

Mesmo com `Capability.Process`, recomenda-se privilegiar `CommandLineExecutor`:

| Funcionalidade | CommandLineExecutor | Processo direto |
|---------------|-------------------|----------------|
| Declaração de capacidade necessária | Não | Sim |
| Sandbox | Lista branca de comandos | Nenhum |
| Timeouts | Integrado | Manual |
| Captura de saída | Estruturada | Manual |
| Registo de auditoria | Automático | Manual |

Use `Capability.Process` + `Process` direto apenas quando necessitar de controlo detalhado sobre fluxos de I/O, tratamento de eventos de processo, ou quando a lista branca de CommandLineExecutor for demasiado restritiva.

## Melhores práticas de segurança

1. **Preferir CommandLineExecutor**: Usar ponto de entrada controlado quando possível
2. **Fornecer uma Reason clara**: "Launch build tools for CI pipeline" vs vago "process access"
3. **Validar todas as entradas**: Nunca passar entradas não fidedignas diretamente para ProcessStartInfo
4. **Usar WaitForExit**: Esperar sempre pela conclusão do processo para prevenir processos zombie
5. **Redirecionar fluxos**: Definir `RedirectStandardOutput = true` e `UseShellExecute = false`

## Ficheiros

- `Plugin.cs` — Plugin de demonstração que declara Capability.Process
- `README.md` — Este ficheiro (Inglês)
- `README.zh-CN.md` — Chinês simplificado
- Traduções: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Exemplos relacionados

- **09-ForbiddenProcess**: Anti-padrão de operações de processo bloqueadas
- **18-CapabilityDenied**: Anti-padrão de capacidades não declaráveis
