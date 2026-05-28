# Demo do Modo de Carregamento por Compilação de Código Fonte CS

Um plugin carregado a partir de ficheiros fonte `.cs` em vez de uma DLL pré-compilada, demonstrando o modo de compilação de código fonte CS do PluginLoader (introduzido pela task-389).

## Como Funciona o Modo de Código Fonte CS

Quando o PluginLoader analisa um diretório de plugins e **não encontra DLL**, entra automaticamente no modo de código fonte CS:

```
1. PluginLoader analisa o diretório → sem DLL
2. Entra no modo de código fonte CS
3. cs.txt encontrado → lê linha a linha, carrega apenas os ficheiros .cs listados
   (Sem cs.txt → carrega todos os ficheiros *.cs no diretório)
4. Análise de DLLs irmãs → DLLs fiáveis adicionadas diretamente como referências;
   DLLs não fiáveis devem passar ScanForbiddenReferences
5. CompilationCore (modo restrito) compila ficheiros .cs em DLL na memória
6. Os bytes da DLL na memória são escritos num ficheiro temporário para análise ScanForbiddenReferences
7. Análise aprovada → reflexão encontra a implementação IPlugin → instanciação
8. Registo mostra: "Plugin loaded [CS-Source]: {Id} v{Version} from {DirName}"
```

## cs.txt — Lista Branca de Carregamento Seletivo

O ficheiro `cs.txt` especifica quais ficheiros `.cs` compilar, um nome de ficheiro por linha:

```
Plugin.cs
```

- **Ficheiros listados**: Compilados e carregados (ex: `Plugin.cs`)
- **Ficheiros não listados**: Ignorados pelo compilador (ex: `Helpers.cs`)
- **Linhas começadas com `#`**: Tratadas como comentários
- **Linhas vazias**: Ignoradas
- **Sem cs.txt**: Todos os ficheiros `*.cs` no diretório são carregados

## Modo Código Fonte CS vs Modo DLL

| Aspeto | Modo DLL | Modo Código Fonte CS |
|--------|----------|---------------------|
| Formato do plugin | DLL pré-compilada `.dll` | Ficheiros fonte `.cs` |
| Acionador de carregamento | DLL encontrada no diretório | Sem DLL, ficheiros `.cs` presentes |
| Compilação | No momento da build | No momento do carregamento pelo PluginLoader |
| Desempenho | Sem overhead de compilação | Overhead de compilação Roslyn no arranque |
| Análise de segurança | Análise direta de metadados PE | Compilação → DLL temporária → Análise metadados PE |
| Prefixo do registo | `Plugin loaded:` | `Plugin loaded [CS-Source]:` |
| Ideal para | Implantação em produção | Iteração de desenvolvimento |

## Tratamento de Erros

| Cenário | Comportamento |
|---------|--------------|
| Sem DLL, sem ficheiros .cs | Aviso: "No DLL and no CS source files found" |
| Erros de compilação | Erro: Mensagens de diagnóstico detalhadas registadas |
| Falha na análise de segurança | Erro: Todas as violações listadas, plugin rejeitado |
| Entrada cs.txt não encontrada | Aviso: "cs.txt entry not found or not a .cs file" |
| Falha na análise de DLL irmã | Aviso: DLL não adicionada como referência, compilação continua |

## Nota de Segurança

Os plugins em modo de código fonte CS são submetidos à **mesma análise de segurança** que os plugins em modo DLL. A assembly compilada é escrita num ficheiro DLL temporário e analisada com `ScanForbiddenReferences` — a mesma análise que as DLLs pré-compiladas recebem. Todas as regras de espaços de nomes/tipos/membros/strings proibidos aplicam-se de forma idêntica.

Os plugins continuam a ser carregados num contexto isolado e analisados quanto a referências de espaços de nomes proibidos (ex: `System.IO`, `System.Net.Http`). Consulte a [Documentação de Segurança](../../docs/pt-PT/security.md) para mais detalhes.
