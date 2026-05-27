# Operações de I/O de ficheiros proibidas — Anti-padrão

Demonstra operações de I/O de ficheiros **proibidas** no sistema de plugins. Este exemplo serve como referência de anti-padrão, mostrando o que NÃO fazer e fornecendo alternativas corretas para cada violação.

## Porquê System.IO é globalmente proibido?

Todo o namespace `System.IO` é bloqueado ao nível de plugin porque o acesso direto a ficheiros apresenta graves riscos de segurança:

1. **Acesso não autorizado a ficheiros**: Plugins podem ler ficheiros sensíveis fora do workspace (palavras-passe, chaves, dados pessoais)
2. **Ataques de sobrescrita**: Plugins maliciosos podem sobrescrever ficheiros de sistema ou configuração críticos
3. **Travessia de diretórios**: Plugins podem usar caminhos `../` para escapar dos limites do workspace
4. **Esgotamento de recursos**: A criação descontrolada de ficheiros pode encher o espaço em disco
5. **Sem trilha de auditoria**: Operações diretas em ficheiros contornam o sistema de auditoria de segurança de plugins

## Tipos proibidos

Todos os tipos `System.IO` que acedem diretamente ao sistema de ficheiros são bloqueados:

| Tipo proibido | Método bloqueado | Nível de risco |
|--------------|-----------------|----------------|
| `File` | `ReadAllText`, `WriteAllText`, `AppendAllText` etc. | 🔴 Crítico |
| `FileStream` | Construtor com caminho de ficheiro | 🔴 Crítico |
| `Directory` | `GetFiles`, `GetDirectories`, `CreateDirectory` | 🔴 Crítico |
| `StreamReader` | Construtor com caminho (string) | 🔴 Crítico |
| `StreamWriter` | Construtor com caminho (string) | 🔴 Crítico |
| `FileInfo` | Todos os métodos | 🔴 Crítico |
| `DirectoryInfo` | Todos os métodos | 🔴 Crítico |

## Tipos permitidos (exceções da lista branca)

Tipos que executam **operações puramente em memória** (sem acesso direto ao sistema de ficheiros) são permitidos:

| Tipo permitido | Utilização | Porquê seguro |
|---------------|-----------|----------------|
| `MemoryStream` | Fluxo de bytes em memória | Sem acesso ao sistema de ficheiros |
| `BinaryReader` | Leitura de fluxo existente | Envolve fluxo, não abre ficheiros |
| `BinaryWriter` | Escrita em fluxo existente | Envolve fluxo, não cria ficheiros |
| `GZipStream` | Compressão/descompressão | Envolve fluxo, sem acesso a ficheiros |
| `StreamReader` | Construtor com parâmetro `Stream` | Seguro ao envolver fluxos auditados |
| `StreamWriter` | Construtor com parâmetro `Stream` | Seguro ao envolver fluxos auditados |

Consulte o exemplo **04-SafeSystemIO** para tipos permitidos.

## Acesso seguro a ficheiros via PermissionedStreamFactory

`PermissionedStreamFactory` é o **ponto de entrada controlado** para operações de ficheiros em plugins:

```csharp
// ✅ Correto: ler ficheiro
using var readStream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(readStream);
string content = reader.ReadToEnd();

// ✅ Correto: escrever ficheiro
using var writeStream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(writeStream);
writer.Write("Dados de log");
```

**PermissionedStreamFactory fornece:**
1. **Validação de caminho**: Previne ataques de travessia de diretórios (`../`)
2. **Verificação de permissões**: Garante que o ficheiro está dentro do workspace permitido
3. **Registo de auditoria**: Todos os acessos a ficheiros são registados para revisão de segurança
4. **Limpeza de recursos**: Rastreia fluxos abertos e previne fugas

## Violações neste exemplo

### Violação 1: File.ReadAllText

```csharp
// ❌ Proibido — ⚠️ VIOLATION: [TypeRef] System.IO.File::ReadAllText
string content = File.ReadAllText("config.json");

// ✅ Alternativa correta
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
string content = reader.ReadToEnd();
```

### Violação 2: File.WriteAllText

```csharp
// ❌ Proibido — ⚠️ VIOLATION: [TypeRef] System.IO.File::WriteAllText
File.WriteAllText("output.log", "some data");

// ✅ Alternativa correta
using var stream = PermissionedStreamFactory.OpenWrite("output.log");
using var writer = new StreamWriter(stream);
writer.Write("some data");
```

### Violação 3: FileStream direto

```csharp
// ❌ Proibido — ⚠️ VIOLATION: [TypeRef] System.IO.FileStream::.ctor
using var fs = new FileStream("data.bin", FileMode.Open);

// ✅ Alternativa correta
using var fs = PermissionedStreamFactory.OpenRead("data.bin");
```

### Violação 4: Directory.GetFiles

```csharp
// ❌ Proibido — ⚠️ VIOLATION: [TypeRef] System.IO.Directory::GetFiles
string[] files = Directory.GetFiles("./logs", "*.txt");

// ✅ Alternativa correta (usando SpeedyPack)
using var pack = SpeedyPack.Open("logs.spk");
var entries = pack.ListEntries("/");
```

### Violação 5: StreamReader com caminho direto

```csharp
// ❌ Proibido — ⚠️ VIOLATION: [TypeRef] System.IO.StreamReader::.ctor(string)
using var reader = new StreamReader("config.json");

// ✅ Alternativa correta
using var stream = PermissionedStreamFactory.OpenRead("config.json");
using var reader = new StreamReader(stream);
```

## Comparação com outros exemplos

| Exemplo | Foco | Permissão necessária |
|---------|------|---------------------|
| **04-SafeSystemIO** | Tipos de memória permitidos (MemoryStream, GZipStream) | Nenhuma |
| **07-ForbiddenFileIO** | Padrões de acesso a ficheiros proibidos (este exemplo) | Não aplicável (bloqueado) |
| **14-CapabilityFileIO** | Declarar capacidade FileIO para contornar restrições | `Capability.FileIO` |

## Mecanismo de scan de segurança do PluginLoader

Quando o PluginLoader analisa este plugin:

1. **Scan TypeRef**: Deteta referências a tipos `System.IO` proibidos
2. **Scan MemberRef**: Deteta chamadas a métodos bloqueados
3. **Scan de strings IL**: Deteta tentativas de contorno por reflexão baseada em strings
4. **Rejeição**: O plugin é rejeitado durante o carregamento com uma mensagem de erro detalhada

O contorno através de concatenação de strings, reflexão, carregamento dinâmico ou ofuscação é impossível — estes são capturados pelo scan ao nível IL (ver **12-ForbiddenStringBypass**).

## Nota de segurança

Se realmente necessitar de acesso irrestrito a ficheiros, pode declarar `Capability.FileIO` (ver 14-CapabilityFileIO). No entanto, as melhores práticas são:
- Preferir **SpeedyPack** para armazenamento de dados estruturados (sem necessidade de declaração de permissões)
- Usar **PermissionedStreamFactory** quando o acesso a ficheiros é necessário (ponto de entrada controlado)
- Declarar `Capability.FileIO` apenas se as soluções acima não forem suficientes
