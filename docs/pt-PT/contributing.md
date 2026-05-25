# Guia de contribuição

> **Versão: v0.2.0-alpha**

[English](../en/contributing.md) | [Deutsch](../de-DE/contributing.md) | [Français](../fr-FR/contributing.md) | [中文](../zh-CN/contributing.md) | [繁體中文](../zh-HK/contributing.md) | [Español](../es-ES/contributing.md) | [日本語](../ja-JP/contributing.md) | [한국어](../ko-KR/contributing.md) | [Čeština](../cs-CZ/contributing.md) | [Русский](../ru-RU/contributing.md) | [Italiano](../it-IT/contributing.md) | [Polski](../pl-PL/contributing.md) | **Português**

Obrigado pelo teu interesse em contribuir para o SiliconLifeCollective!

## Contribuições para dupla versão

Este projeto tem duas versões de implementação. Podes contribuir de acordo com os teus interesses:

### SiliconLife.Default (Versão padrão)
- **Stack tecnológica**: Aplicação de consola .NET 9
- **Direção de contribuição**: Desenvolvimento de funcionalidades principais, implementação de ferramentas, localização, documentação
- **Público-alvo**: Todos os programadores

### SiliconLife.Fast (Versão de alto desempenho)
- **Stack tecnológica**: Aplicação Windows Forms .NET 9
- **Direção de contribuição**: Otimização de desempenho, armazenamento SpeedyPack, bandeja do sistema, concorrência sem lock
- **Público-alvo**: Programadores com experiência Windows e interesse em otimização de desempenho

> **Nota importante**: Ambas as versões partilham os projetos SiliconLife.Core e SiliconLife.Common. Melhorias nas interfaces principais afetam ambas as versões.

## Código de conduta

Este projeto segue a licença Apache 2.0. Mantém respeito e profissionalismo em todas as interações.

---

## Arranque rápido

### 1. Fazer fork do repositório

Clica no botão "Fork" no GitHub para criar a tua cópia.

### 2. Clonar o teu fork

```bash
git clone https://github.com/akimoto-akira/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. Configurar o ambiente de desenvolvimento

```bash
# Instalar .NET 9 SDK
# https://dotnet.microsoft.com/download/dotnet/9.0

# Restaurar dependências
dotnet restore

# Compilar o projeto
dotnet build

# Executar os testes
dotnet test
```

### 4. Criar um ramo de funcionalidade

```bash
git checkout -b feature/nome-da-tua-funcionalidade
```

### 5. Escolher o projeto de desenvolvimento

Com base no teu tipo de contribuição, escolhe o projeto apropriado:

- **Interfaces principais/classes abstratas** → Modificar `SiliconLife.Core`
- **Implementações partilhadas** → Modificar `SiliconLife.Common`
- **Específico para versão Default** → Modificar `SiliconLife.Default`
- **Específico para versão Fast** → Modificar `SiliconLife.Fast`
- **Motor de armazenamento** → Modificar `SiliconLife.Speedy`
- **Ferramenta de gestão de armazenamento** → Modificar `SiliconLife.Speedy.Manager`
- **Desenvolvimento de plugins** → Modificar `SiliconLife.Core/Plugins`
- **Documentação multilingue** → Modificar o diretório `docs/`

---

## Fluxo de trabalho de desenvolvimento

### Estilo do código

- Seguir as convenções C#
- Nomes de classes em PascalCase
- Parâmetros de métodos em camelCase
- Campos privados em `_camelCase`
- Todas as APIs públicas devem ter documentação XML

### Mensagens de commit

Seguir o formato dos **commits convencionais**:

```
<tipo>(<âmbito>): <descrição>
```

**Tipos**:
- `feat`: Nova funcionalidade
- `fix`: Correção de bug
- `docs`: Modificação de documentação
- `style`: Formatação de código
- `refactor`: Refatoração de código
- `test`: Modificação de testes
- `chore`: Modificação de build/ferramentas

**Exemplos**:
```bash
feat(localization): add Korean language support
fix(permission): fix null pointer in callback
docs: update contributing guide
refactor(web): simplify controller structure
```

### Fazer alterações

1. **Escrever o código**
   - Seguir os padrões existentes
   - Adicionar testes para novas funcionalidades
   - Atualizar a documentação

2. **Testar as alterações**
   ```bash
   # Executar todos os testes
   dotnet test

   # Compilar em modo release
   dotnet build --configuration Release
   ```

3. **Formatar o código**
   ```bash
   dotnet format
   ```

4. **Commit das alterações**
   ```bash
   git add .
   git commit -m "feat(âmbito): descrição"
   ```

5. **Push para o teu fork**
   ```bash
   git push origin feature/nome-da-tua-funcionalidade
   ```

6. **Criar um Pull Request**
   - Ir ao repositório original
   - Clicar em "Compare & pull request"
   - Preencher o modelo de PR
   - Submeter

---

## Guia de Pull Request

### Título da PR

Usar o mesmo formato das mensagens de commit:
```
feat(localization): add Korean language support
```

### Descrição da PR

Incluir:

1. **O quê** - O que faz esta PR?
2. **Porquê** - Porque é que esta alteração é necessária?
3. **Como** - Como implementaste?
4. **Testes** - Como foi testado?

---

## Tipos de contribuições

### 1. Correção de bugs

**Processo**:
1. Verificar as issues existentes
2. Criar uma issue se não existir
3. Corrigir o bug
4. Adicionar casos de teste
5. Submeter uma PR

### 2. Novas funcionalidades

**Processo**:
1. Discutir a funcionalidade em Issues/Discussions
2. Obter aprovação dos mantenedores
3. Implementar a funcionalidade
4. Adicionar testes abrangentes
5. Atualizar a documentação
6. Submeter uma PR

### 3. Documentação

**Processo**:
1. Identificar lacunas na documentação
2. Escrever/atualizar a documentação
3. Submeter uma PR

### 4. Refatoração de código

**Processo**:
1. Propor a refatoração numa Issue
2. Obter aprovação
3. Refatorar o código
4. Garantir que todos os testes passam
5. Submeter uma PR

---

## Guia de testes

### Testes unitários

```csharp
[TestMethod]
public void MyFeature_ShouldWork_AsExpected()
{
    // Arrange
    var service = new MyService();

    // Act
    var result = service.DoSomething();

    // Assert
    Assert.IsTrue(result.Success);
}
```

### Testes de integração

Testar os fluxos de trabalho completos:
- Interação IA
- Execução de ferramentas
- Validação de permissões
- Operações de armazenamento

---

## Guia de documentação

### Comentários do código

- Usar comentários XML para todas as APIs públicas
- Usar comentários inline para lógica complexa
- Os comentários do código devem estar em inglês

### Ficheiros de documentação

- Colocar em `docs/{língua}/`
- Atualizar todas as versões linguísticas
- Seguir a estrutura existente

### Documentação multilingue

Ao adicionar documentação:
1. Criar primeiro a versão inglesa
2. Traduzir para as outras línguas
3. Manter o conteúdo sincronizado

---

## Processo de revisão

### O que os mantenedores verificam

1. **Qualidade do código**
   - Segue as convenções
   - Claro e legível
   - Bem documentado

2. **Testes**
   - Cobertura adequada
   - Todos os testes passam
   - Cobre os casos limite

3. **Documentação**
   - Atualizada
   - Explicações claras
   - Multilingue

4. **Compatibilidade**
   - Compatível com versões anteriores
   - Sem alterações disruptivas (salvo notificação)
   - Segue a gestão semântica de versões

---

## Perguntas frequentes

### PR rejeitada

**Motivos**:
- Não segue as diretrizes
- Testes insuficientes
- Alterações disruptivas não notificadas
- Qualidade de código fraca

**Soluções**:
- Resolver os feedbacks
- Atualizar a PR
- Submeter novamente

### Conflitos de fusão

**Soluções**:
```bash
# Atualizar o teu ramo
git fetch origin
git rebase origin/master

# Resolver os conflitos
# Modificar os ficheiros em conflito
git add .
git rebase --continue

# Push forçado
git push --force-with-lease
```

---

## Obter ajuda

### Recursos

- **Documentação**: [docs/](../)
- **Issues**: GitHub Issues
- **Discussions**: GitHub Discussions
- **Código de conduta**: CODE_OF_CONDUCT.md

---

## Reconhecimentos

Os contribuidores serão reconhecidos em:
- A secção de contribuidores do README.md
- As notas de lançamento
- A documentação do projeto

---

## Licença

Ao contribuir, aceitas que as tuas contribuições estejam sob licença Apache 2.0.

---

## Próximos passos

- 📚 Ler a [documentação](../)
- 🐛 Ver as [issues abertas](https://github.com/akimoto-akira/SiliconLifeCollective/issues)
- 💬 Iniciar uma [discussão](https://github.com/akimoto-akira/SiliconLifeCollective/discussions)
- 🚀 Fazer fork e começar a contribuir!

Obrigado por contribuires para o SiliconLifeCollective! 🎉
