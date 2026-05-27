# PluginDemo-18: Capacidade negada — Anti-padrão de capacidade não declarável

## Visão geral

Este plugin é um **anti-padrão** que demonstra que declarar uma capacidade NÃO contorna as proibições de capacidades não declaráveis. Mesmo com `[PluginCapability(Capability.Network)]`, P/Invoke, Unsafe, Reflection.Emit e o acesso ao registo permanecem **sempre** bloqueados.

## Capacidades declaráveis vs. não declaráveis

### ✅ Declaráveis (existem valores do enum Capability)

| Capacidade | O que isenta |
|-----------|-------------|
| `Capability.Network` | Namespaces System.Net.* e proibições por tipo |
| `Capability.FileIO` | Namespace System.IO (além da lista branca) |
| `Capability.Process` | Tipos Process* sob System.Diagnostics |
| `Capability.AI` | Permite injeção de IAIService (sem isenção TypeRef) |

### ❌ Não declaráveis (NÃO existe valor do enum Capability)

| Categoria | Tipos bloqueados | Por que não declarável |
|----------|-----------------|----------------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory`, `NativeLibrary` | Impossível auditar código nativo arbitrário em tempo de execução |
| Código unsafe | `UnverifiableCodeAttribute`, `System.Runtime.CompilerServices.Unsafe` | Contorna a segurança de tipos CLR e verificação de limites |
| Emissão IL | `System.Reflection.Emit.*` | Pode gerar IL arbitrário em tempo de execução |
| Carregamento de assemblies | `System.Runtime.Loader`, `Assembly.Load*` | Pode carregar DLLs não verificadas, contornando a verificação de segurança |
| Registo | `Microsoft.Win32.*` | Acesso de sistema ao nível do SO fora da sandbox do plugin |
| Compilação dinâmica | `Microsoft.CodeAnalysis.*` | Pode compilar e executar código arbitrário |
| Reflexão perigosa | `Type.GetType(string)`, `Activator.CreateInstance` | Pode instanciar tipos proibidos por string |

## Por que estas capacidades não podem ser declaradas

O motivo fundamental: **não podem ser auditadas em segurança em tempo de execução.**

1. **P/Invoke**: Uma vez chamado o código nativo, o CLR não tem visibilidade — sem garantias de segurança
2. **Unsafe**: Contorna o sistema de segurança de tipos de que o modelo de segurança do plugin depende
3. **Reflection.Emit**: Pode gerar novo IL em tempo de execução nunca verificado pelo PluginLoader
4. **AssemblyLoadContext**: Pode carregar DLLs nunca verificadas para segurança
5. **Registry**: Fornece acesso a configuração ao nível do SO fora da sandbox do plugin

## Processamento de «declaração inválida» do PluginLoader

Quando o PluginLoader encontra uma declaração de capacidade:

1. Lê o valor enum int32 do blob CustomAttribute
2. Verifica `Enum.IsDefined(typeof(Capability), value)`
3. Se o valor não for um membro Capability definido → **ignorado silenciosamente**
4. Se o valor estiver definido → as regras de isenção são aplicadas
5. **As verificações não declaráveis são SEMPRE aplicadas** independentemente de qualquer capacidade declarada

Isto impede que os plugins declarem capacidades «futuras» que ainda não existem.

## Comparação com 13-CapabilityNetwork

| Aspeto | 13-CapabilityNetwork (positivo) | 18-CapabilityDenied (anti-padrão) |
|--------|-------------------------------|----------------------------------|
| Declaração | `[PluginCapability(Capability.Network)]` | `[PluginCapability(Capability.Network)]` |
| Usa HttpClient | ✅ Isentado | ✅ Isentado |
| Usa DllImport | N/A | ❌ SEMPRE bloqueado |
| Usa Unsafe | N/A | ❌ SEMPRE bloqueado |
| Resultado do carregamento | ✅ CARREGADO | ❌ REJEITADO |

## Ficheiros

- `Plugin.cs` — Plugin de demonstração anti-padrão de capacidade não declarável
- `README.md` — Este ficheiro (Inglês)
- `README.zh-CN.md` — Chinês simplificado
- Traduções: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Exemplos relacionados

- **13-CapabilityNetwork**: Exemplo positivo de Capability.Network
- **11-ForbiddenPInvoke**: Anti-padrão P/Invoke (nenhuma capacidade pode ajudar)
- **10-ForbiddenReflection**: Anti-padrão Reflection (nenhuma capacidade pode ajudar)
