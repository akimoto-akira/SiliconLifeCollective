# Demo de dependência confiável

Demonstra a utilização de `Newtonsoft.Json` — uma biblioteca que internamente faz uso intensivo de reflexão — como assembly confiável. O scanner de segurança do PluginLoader ignora completamente assemblies confiáveis, permitindo que plugins os referenciem sem ativar violações.

## Mecanismo de lista branca TrustedAssemblies

O `PluginLoader` mantém uma lista branca estática de bibliotecas open-source que são **confiáveis por padrão**:

```csharp
private static readonly HashSet<string> TrustedAssemblies = new(StringComparer.Ordinal)
{
    // Serialização
    "Google.Protobuf",
    "protobuf-net",
    "Newtonsoft.Json",        // ← Este demo utiliza esta biblioteca
    "MessagePack",
    "YamlDotNet",

    // Registo
    "Serilog", "NLog",

    // Microsoft.Extensions.*
    "Microsoft.Extensions.Logging.Abstractions",
    "Microsoft.Extensions.DependencyInjection.Abstractions",
    // ...

    // Acesso a dados / mapeamento
    "Dapper", "AutoMapper",

    // Validação e distribuição de mensagens
    "FluentValidation", "MediatR",
};
```

### Critérios de admissão

Uma biblioteca pode ser adicionada a `TrustedAssemblies` se cumprir **todos os três** critérios:

| # | Critério | Justificação |
|---|----------|--------------|
| 1 | Projeto open-source amplamente utilizado (MIT / Apache 2.0 / BSD) | Código publicamente auditável |
| 2 | Código-fonte acessível publicamente | Supervisão comunitária garante ausência de comportamento malicioso |
| 3 | Pacote NuGet mantido por fornecedor/comunidade de confiança | Integridade da cadeia de fornecimento |

### Base de identificação

O scanner identifica assemblies confiáveis pelo seu `AssemblyDefinition.Name` nos metadados PE — **não pelo nome do ficheiro DLL**. Isto impede que atacantes renomeiem uma DLL maliciosa para `Newtonsoft.Json.dll` para contornar as verificações.

## CollectTrustedTypeRefs — Isenção transitiva

Quando o PluginLoader carrega um diretório de plugin, executa uma verificação em duas fases:

```
Fase 1: CollectTrustedTypeRefs(pluginDir)
├── Enumerar todos os ficheiros *.dll no diretório do plugin
├── Para cada DLL: ler metadados PE → verificar AssemblyDefinition.Name
├── Se nome ∈ TrustedAssemblies:
│   └── Recolher TODAS as entradas TypeReference → pares (namespace, typeName)
└── Retorna: HashSet<(string Namespace, string Name)>

Fase 2: ScanForbiddenReferences(pluginMainDll, trustedTypeRefs)
├── Camada 0:   Saída rápida da lista branca (se DLL principal é confiável → passa)
├── Camada 0.5: Isenção transitiva (ignorar TypeRefs no conjunto trustedTypeRefs)
├── Camada 1:   Verificação da tabela TypeRef
├── Camada 2:   Verificação da tabela ExportedType
├── Camada 3:   Verificação da tabela MemberRef (métodos perigosos)
├── Camada 4:   Marcadores de código não seguro + P/Invoke
└── Camada 5:   Verificação do heap de strings #US
```

### Porque é que a isenção transitiva é importante

O Newtonsoft.Json referencia internamente tipos como `System.Reflection.MemberInfo`, `System.IO.TextReader`, etc. Quando o seu plugin referencia o Newtonsoft.Json, o compilador pode incorporar estes TypeRefs transitivos na DLL do **seu** plugin. Sem isenção transitiva, o seu plugin seria marcado por referenciar `System.IO.TextReader` — mesmo que nunca o utilize diretamente.

`CollectTrustedTypeRefs` resolve isto pré-recolhendo todos os TypeRefs de DLLs confiáveis e marcando-os como "conhecidos seguros" durante a verificação principal.

## Como adicionar uma nova dependência confiável

Para adicionar uma nova biblioteca à lista branca:

1. Verificar que cumpre os três critérios de admissão acima
2. Adicionar uma linha ao HashSet `TrustedAssemblies` em `PluginLoader.cs`:
   ```csharp
   "YourLibraryName",  // Breve descrição de porque é confiável
   ```
3. Colocar a DLL da biblioteca no diretório do plugin (ao lado da DLL principal do plugin)
4. O scanner recolherá automaticamente os seus TypeRefs e isenta-los-á

> **⚠️ Importante:** Adicionar uma biblioteca a `TrustedAssemblies` significa que o scanner **não** verificará o seu código interno. Adicione apenas bibliotecas em que confia plenamente.

## Este demo

Este plugin utiliza Newtonsoft.Json sem qualquer declaração `PluginCapability`:

| Função | Comportamento interno do Newtonsoft.Json | Porque funciona |
|--------|------------------------------------------|-----------------|
| `JsonConvert.SerializeObject` | Utiliza reflexão para enumerar propriedades | DLL do Newtonsoft.Json passa a lista branca da camada 0 |
| `JsonConvert.DeserializeObject<T>` | Chama `Activator.CreateInstance`, define propriedades via reflexão | TypeRefs transitivos isentos na camada 0.5 |
| Manipulação `JObject` / `JArray` | Utiliza `System.Linq.Expressions`, dispatch dinâmico | Todas as refs internas recolhidas por `CollectTrustedTypeRefs` |

### Diferença chave em relação ao PluginCapability

| Mecanismo | Âmbito | Caso de uso |
|-----------|--------|-------------|
| `TrustedAssemblies` | Isenta uma **biblioteca** inteira (e as suas refs transitivas) da verificação | Dependências open-source conhecidas |
| `PluginCapability` | Isenta o **código do seu plugin** de proibições de namespaces específicos | O plugin precisa de acesso direto a System.Net/IO/Process |

Um plugin que utiliza apenas dependências confiáveis **não precisa** de qualquer declaração `PluginCapability`. O scanner trata de tudo automaticamente.

## Nota de segurança

Os assemblies confiáveis estão isentos da verificação de segurança porque são projetos open-source auditáveis. No entanto, **o código do seu plugin** continua a ser completamente verificado. Se o seu plugin referenciar diretamente `System.IO.File` ou `System.Net.Http.HttpClient`, continuará a ser bloqueado — a menos que declare a `PluginCapability` correspondente. Consulte a [documentação de segurança](../../docs/pt-PT/security.md).
