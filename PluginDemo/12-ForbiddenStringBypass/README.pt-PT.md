# PluginDemo-12: Anti-Padrão de Bypass por Strings de Reflexão Proibidas

## Visão Geral

Este plugin demonstra tentativas **proibidas** de bypass baseadas em strings de reflexão no sistema de plugins SiliconLife. Mostra por que a concatenação, interpolação, codificação e outras técnicas de ofuscação **não podem** contornar a varredura do heap #US (User String) do PluginLoader — a **última linha de defesa**.

## O que é o heap #US?

Nos metadados .NET PE (Portable Executable), o **heap #US (User String)** armazena todos os operandos de literais de string usados por instruções IL `ldstr`. Cada vez que você escreve um literal de string em código C#, o compilador o armazena neste heap.

```
Fonte C#:     string s = "System.IO.File";
    ↓ compilação
Código IL:    ldstr "System.IO.File"    ← referencia token no heap #US
    ↓ varredura PluginLoader
Heap #US:     [..., "System.IO.File", ...]  ← DETECTADO por correspondência de prefixo!
```

O método `ScanUserStrings()` do PluginLoader itera sobre **cada entrada** do heap #US, verificando se alguma string começa com um prefixo proibido.

## Prefixos de strings proibidos

Os seguintes prefixos disparam violações `[ILString]` quando encontrados no heap #US:

| Prefixo | Categoria |
|---------|-----------|
| `System.IO.` | Tipos de sistema de ficheiros |
| `System.Net.Http` | Cliente HTTP |
| `System.Net.WebSockets` | WebSocket |
| `System.Net.Sockets` | Sockets em bruto |
| `System.Net.Mail` | SMTP |
| `System.Net.NetworkInformation` | Sondagem de rede |
| `System.Net.Security` | SslStream |
| `System.Diagnostics.Process` | Processo/linha de comandos |
| `Microsoft.CodeAnalysis` | Compilador Roslyn |
| `System.Reflection.Emit` | Emissão IL |
| `System.Runtime.Loader` | AssemblyLoadContext |
| `System.CodeDom.Compiler` | CodeDom legado |
| `Microsoft.Win32` | Registo Windows |

## Violações demonstradas

### Violação 1: String direta de nome de tipo

```csharp
// ❌ PROIBIDO — a string completa está no heap #US
Type? fileType = Type.GetType("System.IO.File, System.Runtime");
```

**Violação**: `[ILString] "System.IO.File, System.Runtime" matches forbidden prefix "System.IO."`

### Violação 2: Concatenação de strings (tempo de compilação)

```csharp
// ❌ PROIBIDO — o compilador dobra const+const numa entrada #US
const string ns = "System.Net.Http";
const string typeName = ".HttpClient";
const string assembly = ", System.Net.Http";
Type? type = Type.GetType(ns + typeName + assembly);
```

**Violação**: `[ILString] "System.Net.Http.HttpClient, System.Net.Http" matches forbidden prefix "System.Net.Http"`

### Violação 3: Interpolação de strings

```csharp
// ❌ PROIBIDO — partes literais são armazenadas no heap #US
string className = "FileStream";
string fullName = $"System.IO.{className}, System.Runtime";
```

**Violação**: `[ILString] "System.IO." matches forbidden prefix "System.IO."`

### Violação 4: Campos Const

```csharp
// ❌ PROIBIDO — valores const são inline → aparecem no heap #US
private const string ProcessType = "System.Diagnostics.Process";
private const string AssemblyName = ", System.Runtime";
Type? type = Type.GetType(ProcessType + AssemblyName);
```

**Violação**: `[ILString] "System.Diagnostics.Process" matches forbidden prefix "System.Diagnostics.Process"`

### Violação 5: Fragmentos de strings parciais

```csharp
// ❌ PROIBIDO — cada parte é um ldstr separado, varrido independentemente
string part1 = "System.Reflection.Emit";
string part2 = ".AssemblyBuilder";
string fullType = part1 + part2;
```

**Violação**: `[ILString] "System.Reflection.Emit" matches forbidden prefix "System.Reflection.Emit"`

## Por que as técnicas de ofuscação falham

| Técnica | Por que falha |
|---------|--------------|
| Concatenação const | Compilador dobra em única entrada #US |
| Interpolação de strings | Partes literais armazenadas no heap #US |
| Campos const | Valores inline → aparecem no #US |
| Divisão em variáveis | Cada operando `ldstr` varrido independentemente |
| Codificação Base64 | Descodificação precisa de métodos runtime, mas `Type.GetType` bloqueado por MemberRef |
| Construção por array de char | Não gera `ldstr`, mas `Type.GetType` permanece bloqueado por MemberRef |
| Encriptação XOR | String encriptada ilegível no #US, mas desencriptação + `Type.GetType` = MemberRef bloqueado |

**Insight chave**: A varredura #US bloqueia a **string**. A varredura MemberRef bloqueia o **método**. Para carregar dinamicamente um tipo, precisa de AMBOS. PluginLoader bloqueia AMBOS independentemente.

## A cadeia de defesa completa

| Passo | Mecanismo | O que deteta |
|-------|-----------|-------------|
| 1 | Tabela TypeRef | Referências diretas a tipos proibidos |
| 2 | Tabela ExportedType | Tipos reencaminhados de namespaces proibidos |
| 3 | Tabela MemberRef | Chamadas a `Type.GetType`, `Assembly.Load`, `Activator.CreateInstance` |
| 4 | Marcadores Unsafe | `[DllImport]`, blocos unsafe, flag PinvokeImpl |
| **5** | **Varredura do heap #US** | **Constantes string que correspondem a prefixos proibidos (esta demo)** |

## Ficheiros

- `Plugin.cs` - Plugin de demonstração anti-padrão
- `README.md` - English
- `README.zh-CN.md` - 简体中文
- `README.zh-HK.md` - 繁體中文
- `README.ja-JP.md` - 日本語
- `README.ko-KR.md` - 한국어
- `README.de-DE.md` - Deutsch
- `README.fr-FR.md` - Français
- `README.es-ES.md` - Español
- `README.it-IT.md` - Italiano
- `README.ru-RU.md` - Русский
- `README.pt-PT.md` - Este ficheiro (Português)
- `README.pl-PL.md` - Polski
- `README.cs-CZ.md` - Čeština

## Exemplos relacionados

- **10-ForbiddenReflection**: Métodos de reflexão proibidos (varredura MemberRef)
- **11-ForbiddenPInvoke**: P/Invoke e código unsafe proibidos
- **02-TypeRegistryUsage**: Uso correto de ITypeRegistry
- **03-ObjectFactoryUsage**: Uso correto de IObjectFactory
