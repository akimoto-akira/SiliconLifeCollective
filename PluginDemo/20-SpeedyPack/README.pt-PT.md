# PluginDemo-20: SpeedyPack — Armazenamento de dados estruturados

## Visão geral

Este plugin demonstra o uso de `SpeedyPack` para armazenamento de dados estruturados **sem qualquer declaração de capacidade**. SpeedyPack é o método **recomendado** para persistência de dados de plugins.

## Porquê SpeedyPack?

| Funcionalidade | SpeedyPack | PermissionedStreamFactory | Capability.FileIO + System.IO |
|---------------|-----------|--------------------------|------------------------------|
| Capacidade necessária | **Nenhuma** | Nenhuma | `Capability.FileIO` |
| Cache | ✅ Integrado | ❌ | ❌ |
| WAL (recuperação de falhas) | ✅ | ❌ | ❌ |
| Transações | ✅ `IPackTransaction` | ❌ | ❌ |
| Thread-safe | ✅ | ❌ | ❌ |
| Serialização estruturada | ✅ `Read<T>` | ❌ Bytes brutos | ❌ Manual |
| Rasto de auditoria | ✅ Automático | ✅ Automático | ❌ Manual |

## CRUD básico

```csharp
// Abrir um ficheiro de dados SpeedyPack
using var pack = SpeedyPack.Open("mydata.spk");

// Escrever pares chave-valor
pack.Write("user:name", "Alice");
pack.Write("user:age", 30);

// Ler valores (tipados)
string name = pack.Read<string>("user:name");  // "Alice"
int age = pack.Read<int>("user:age");           // 30

// Eliminar uma chave
pack.Delete("user:age");

// Verificar existência
bool exists = pack.Contains("user:name");  // true
```

## Acesso tipado com objetos estruturados

```csharp
public class UserProfile
{
    public string Name { get; set; }
    public int Level { get; set; }
    public string[] Tags { get; set; }
}

// Escrever objeto estruturado
var profile = new UserProfile { Name = "Bob", Level = 42, Tags = new[] { "admin" } };
pack.Write("profile:bob", profile);

// Ler objeto tipado
var loaded = pack.Read<UserProfile>("profile:bob");
Console.WriteLine($"{loaded.Name}, Level {loaded.Level}");
```

## Transações

```csharp
using (var tx = pack.BeginTransaction())
{
    try
    {
        tx.Write("account:a", 1000);
        tx.Write("account:b", 500);
        tx.Commit();   // Atómico — ambas as escritas persistem ou nenhuma
    }
    catch
    {
        tx.Rollback();  // Descartar todas as escritas nesta transação
    }
}
```

### Métodos IPackTransaction

| Método | Descrição |
|--------|----------|
| `Write(key, value)` | Colocar uma operação de escrita na fila |
| `Delete(key)` | Colocar uma operação de eliminação na fila |
| `Commit()` | Aplicar atomicamente todas as operações na fila |
| `Rollback()` | Descartar todas as operações na fila |

## Configuração com SpeedyPackOptions

```csharp
var options = new SpeedyPackOptions
{
    MaxCacheSize = 1024 * 1024,              // 1 MB de cache
    AutoFlushInterval = TimeSpan.FromSeconds(30),
    CompressionLevel = CompressionLevel.Optimal
};
using var pack = SpeedyPack.Open("data.spk", options);
```

### Propriedades SpeedyPackOptions

| Propriedade | Tipo | Predefinido | Descrição |
|------------|------|-----------|----------|
| `MaxCacheSize` | `long` | 64 MB | Tamanho máximo do cache em memória |
| `AutoFlushInterval` | `TimeSpan` | 10 segundos | Intervalo de descarga do cache para o disco |
| `CompressionLevel` | `CompressionLevel` | `Fastest` | Nível de compressão para dados armazenados |

## Nota de segurança

SpeedyPack **não requer** declaração de capacidade. É um ponto de entrada de armazenamento de dados seguro e controlado que:
- Valida todos os caminhos contra os limites do espaço de trabalho
- Fornece rasto de auditoria completo de todas as operações de leitura/escrita
- Previne ataques de travessia de diretórios
- Gere automaticamente o ciclo de vida dos recursos

## Ficheiros

- `Plugin.cs` — Plugin de demonstração SpeedyPack
- `README.md` — Este ficheiro (Inglês)
- `README.zh-CN.md` — Chinês simplificado
- Traduções: zh-HK, ja-JP, ko-KR, de-DE, fr-FR, es-ES, it-IT, ru-RU, pt-PT, pl-PL, cs-CZ

## Exemplos relacionados

- **04-SafeSystemIO**: Tipos System.IO em memória permitidos (sem declaração necessária)
- **07-ForbiddenFileIO**: Anti-padrão de operações de ficheiro bloqueadas
- **14-CapabilityFileIO**: Quando SpeedyPack não é suficiente
