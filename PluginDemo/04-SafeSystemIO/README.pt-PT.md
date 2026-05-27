# Demo de System.IO seguro

Demonstra os tipos System.IO na lista branca `SystemIOAllowedTypes`: `MemoryStream`, `BinaryReader`/`BinaryWriter`, `GZipStream`. Explica por que `FileStream` requer `PermissionedStreamFactory`.

## Lista branca SystemIOAllowedTypes

O runtime do plugin bloqueia por padrão o espaço de nomes `System.IO`, mas isenta tipos que **não realizam E/S de ficheiros diretamente**:

| Categoria | Tipos permitidos | Por que seguro |
|-----------|-----------------|---------------|
| Abstrações de fluxo | `Stream` | Classe base abstrata, sem E/S própria |
| Fluxos em memória | `MemoryStream` | Operação puramente em memória |
| Fluxos de compressão | `GZipStream`, `DeflateStream`, `ZLibStream` | Envolvem outro fluxo, não abrem ficheiros |
| Wrappers binários | `BinaryReader`, `BinaryWriter` | Envolvem qualquer fluxo, não abrem ficheiros |
| Enumerações | `SeekOrigin`, `FileMode`, `FileAccess`, `FileShare`, `CompressionMode`, `CompressionLevel` | Apenas tipos de valor |
| Exceções | `IOException`, `InvalidDataException`, `EndOfStreamException` | Apenas tipos de erro |

### Tipos fora da lista branca

Estes tipos **acedem diretamente ao sistema de ficheiros** e são **bloqueados** no código do plugin:

| Tipo bloqueado | Razão | Alternativa segura |
|---------------|-------|-------------------|
| `FileStream` | Abre ficheiros diretamente | `PermissionedStreamFactory.CreateReadStream()` / `CreateWriteStream()` |
| `File` | Operações de ficheiro estáticas | `PermissionedStreamFactory` + `SafePath` |
| `Directory` | Operações de diretório estáticas | `SafePath` (verificação de permissões) |
| `FileInfo` | Encapsula caminhos de ficheiros | `SafePath` |
| `DirectoryInfo` | Encapsula caminhos de diretórios | `SafePath` |
| `StreamReader` | Abre ficheiros diretamente | `PermissionedStreamFactory` + envolver `PermissionedStream` |
| `StreamWriter` | Abre ficheiros diretamente | `PermissionedStreamFactory` + envolver `PermissionedStream` |

## Por que PermissionedStreamFactory para FileStream

`FileStream` abre ficheiros diretamente no disco — um grande risco de segurança num sistema de plugins. `PermissionedStreamFactory` impõe:

1. **Verificação de permissões** — o `PermissionManager` do chamador deve conceder `FileAccess` para o caminho
2. **Registo de auditoria** — cada abertura de ficheiro é registada com o ID being do chamador
3. **Validação do caminho** — caminhos vazios/inválidos são rejeitados antes de qualquer E/S

```
❌ new FileStream("path", FileMode.Open)           → Bloqueado pelo scanner TypeRef
✅ PermissionedStreamFactory.CreateReadStream(id, "path")  → Verificação de permissões aprovada
✅ PermissionedStreamFactory.CreateWriteStream(id, "path") → Verificação de permissões aprovada
```

## Pipeline de demonstração

Esta demo constrói um pipeline de dados completo em memória usando apenas tipos da lista branca:

```
┌─────────────────────────────────────────────────────────────────┐
│  Demo 1: MemoryStream                                           │
│  └─ Escrever bytes → Ler bytes → Descodificar cadeia            │
│                                                                  │
│  Demo 2: Pipeline de compressão                                  │
│  └─ string → UTF8 → MemoryStream                                │
│     → GZipStream(comprimir) → MemoryStream(comprimido)           │
│     → GZipStream(descomprimir) → MemoryStream(em bruto)          │
│     → UTF8 → string (ida e volta)                                │
│                                                                  │
│  Demo 3: BinaryReader/Writer                                     │
│  └─ Write(int, double, string) → MemoryStream                   │
│     → Read(int, double, string) → Verificar ida e volta         │
└─────────────────────────────────────────────────────────────────┘
```

## Esta demo

> **⚠️ Nota:** Esta demo usa **apenas** tipos da lista branca `SystemIOAllowedTypes`. Não é realizada nenhuma E/S de ficheiros. Para acesso a ficheiros, ver a API `PermissionedStreamFactory`.

| Classe | Papel |
|--------|-------|
| `SafeSystemIOPlugin` | Implementação `IPlugin` — demonstra uso seguro de System.IO |

## Nota de segurança

O espaço de nomes `System.IO` é bloqueado pelo scanner TypeRef do plugin. Apenas os tipos da lista branca passam. Para acesso real a ficheiros, deve ser usado `PermissionedStreamFactory`, que realiza verificações de permissões e registo de auditoria. Consulte a [documentação de segurança](../../docs/pt-PT/security.md).
