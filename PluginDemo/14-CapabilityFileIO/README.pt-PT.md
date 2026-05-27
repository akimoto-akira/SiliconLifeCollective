# PluginDemo-14: Capability.FileIO — Permissão declarativa de E/S de ficheiros

## Visão geral

Este plugin demonstra o uso de `[PluginCapability(Capability.FileIO)]` para declarar acesso direto ao sistema de ficheiros. Com esta declaração, o plugin acede a todos os tipos `System.IO` além da lista branca `SystemIOAllowedTypes`.

## Sintaxe de declaração PluginCapability

```csharp
[PluginCapability(Capability.FileIO, Reason = "Direct log file access for audit trail")]
public class CapabilityFileIOPlugin : IPlugin { ... }
```

## Como funciona o Capability.FileIO

1. **Estado predefinido**: O namespace `System.IO` está totalmente proibido; apenas os tipos da lista branca `SystemIOAllowedTypes` são permitidos (MemoryStream, BinaryReader, GZipStream, etc.)
2. **Com declaração**: A proibição de todo o namespace `System.IO` é levantada — File, FileStream, Directory, StreamReader(string), etc. tornam-se acessíveis
3. **Isenção ILString**: Constantes de cadeia que começam com `"System.IO."` não são sinalizadas
4. **Limites não declaráveis**: P/Invoke, Unsafe, Reflection.Emit, etc. permanecem bloqueados

## Âmbito de isenção do Capability.FileIO

### Isenções TypeRef

Todos os tipos `System.IO` são isentados:

| Categoria | Tipos isentados |
|-----------|----------------|
| Operações de ficheiros | `File`, `FileInfo` |
| Operações de diretórios | `Directory`, `DirectoryInfo` |
| Tipos de fluxo | `FileStream`, `StreamReader(path)`, `StreamWriter(path)` |
| Sistema de ficheiros | `FileSystemWatcher`, `DriveInfo`, `Path` |

### Isenção ILString

- Cadeias que começam com `"System.IO."` não são sinalizadas

### O que permanece proibido

| Categoria | Ainda bloqueado |
|-----------|----------------|
| P/Invoke | `DllImportAttribute`, `Marshal`, `NativeMemory` |
| Código unsafe | `UnverifiableCodeAttribute`, `Unsafe` |
| Emissão IL | `System.Reflection.Emit.*` |
| Carregamento de assemblies | `System.Runtime.Loader`, `Assembly.Load*` |
| Registo | `Microsoft.Win32.*` |

## Comparação com outros exemplos

| Exemplo | Declaração | Acesso a ficheiros | Notas |
|---------|-----------|-------------------|-------|
| **04-SafeSystemIO** | Nenhuma | MemoryStream, BinaryReader, GZipStream | Usa apenas tipos da lista branca |
| **07-ForbiddenFileIO** | Nenhuma | ❌ REJEITADO | Exemplo de anti-padrão |
| **14-CapabilityFileIO** | `[PluginCapability(Capability.FileIO)]` | ✅ Acesso completo ao System.IO | Este exemplo |
| **20-SpeedyPack** | Nenhuma | Via API SpeedyPack (sem Capability necessária) | Armazenamento de dados recomendado |

## Ordem de prioridade para acesso a ficheiros

1. **SpeedyPack** — Sem declaração de capacidade necessária. Cache integrado, WAL, transações. **Recomendado para armazenamento de dados estruturados.**
2. **PermissionedStreamFactory** — Sem declaração necessária. Acesso auditado com validação de caminho e controlo de acesso.
3. **Capability.FileIO + System.IO direto** — Apenas quando as opções acima não são suficientes.

## Porque preferir PermissionedStreamFactory / SpeedyPack?

Mesmo com `Capability.FileIO`, o uso de pontos de entrada controlados é recomendado porque:

1. **Rasto de auditoria**: Todo o acesso é registado e rastreável
2. **Validação de caminho**: Previne ataques de travessia de diretórios (`../`)
3. **Controlo de acesso**: Imposição dos limites do espaço de trabalho
4. **Monitorização de recursos**: Previne fugas de fluxos e esgotamento de recursos
5. **Conformidade**: Padrões de acesso controlado facilitam revisões de segurança

## Melhores práticas de segurança

1. **Declarar FileIO apenas quando realmente necessário**: Pode usar SpeedyPack ou PermissionedStreamFactory?
2. **Fornecer uma Reason clara**: "Direct log file access for audit trail" é melhor que "file access"
3. **Validar caminhos manualmente**: Mesmo com Capability.FileIO, validar todos os caminhos de ficheiros antes de usar
4. **Usar instruções using**: Descartar sempre FileStream/StreamReader/StreamWriter
5. **Princípio do menor privilégio**: Declarar apenas as capacidades que o plugin realmente precisa

## Ficheiros

- `Plugin.cs` — Plugin de demonstração que declara Capability.FileIO
- `README.md` — Este ficheiro (Inglês)
- `README.zh-CN.md` — Chinês simplificado
- Traduções: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Exemplos relacionados

- **04-SafeSystemIO**: Tipos System.IO em memória permitidos (sem declaração necessária)
- **07-ForbiddenFileIO**: Anti-padrão de operações de ficheiro bloqueadas
- **20-SpeedyPack**: Armazenamento de dados recomendado sem declaração de capacidade
- **18-CapabilityDenied**: Anti-padrão de capacidades não declaráveis
